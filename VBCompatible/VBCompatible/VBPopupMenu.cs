using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VBCompatible
{
    /// <summary>
    /// VB6.0 PopupMenu 互換コントロール
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    public class VBPopupMenu : ContextMenuStrip
    {
        ToolStripMenuItem m_Menu;

        // PerformClick は Enabled && Available でなければイベントを起動しない
        // Reflection で OnClick を呼び出す
        private static void InvokeOnClick(ToolStripMenuItem target) {
            Type type = target.GetType();
            MethodInfo mi;
            // DropDownItem を持っているときは OnDropDownOpened
            if (target.HasDropDownItems) {
                mi = type.GetMethodInfo("OnDropDownOpened", typeof(EventArgs));
            } else {
                mi = type.GetMethodInfo("OnClick", typeof(EventArgs));
            }
            mi.Invoke(target, new object[] { EventArgs.Empty });
        }

        /// <summary>
        /// ToolStripMenuItem を指定して PopupMenu を作成します。
        /// </summary>
        /// <param name="menu">コンテキストメニューのソースとなる ToolStripMenuItem</param>
        public VBPopupMenu(ToolStripMenuItem menu) {
            m_Menu = menu;

            base.Items.Clear();
            foreach (ToolStripItem item in menu.DropDownItems) {
                AddMenu(null, item);
            }
        }

        /// <summary>
        /// ToolStripMenuItem を指定してメニュー階層を作成します。 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="current"></param>
        private void AddMenu(ToolStripMenuItem parent, ToolStripItem current) {
            ToolStripSeparator separator = current as ToolStripSeparator;
            if (separator != null) {
                if (parent == null)
                    base.Items.Add(new ToolStripSeparator());
                else
                    parent.DropDownItems.Add(new ToolStripSeparator());
            } else {
                ToolStripMenuItem menuItem = current as ToolStripMenuItem;
                if (menuItem != null) {
                    ToolStripMenuItem currentMenuItem = new VBPopupMenuItem(menuItem);
                    if (parent == null)
                        base.Items.Add(currentMenuItem);
                    else
                        parent.DropDownItems.Add(currentMenuItem);

                    if (menuItem.HasDropDownItems)
                        foreach (ToolStripItem item in menuItem.DropDownItems) {
                            AddMenu(currentMenuItem, item);
                        }
                }
            }
        }

        /// <summary>
        /// Opening イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納した CancelEventArgs</param>
        protected override void OnOpening(CancelEventArgs e) {
            InvokeOnClick(m_Menu);
            base.OnOpening(e);
        }

        internal class VBPopupMenuItem : ToolStripMenuItem
        {
            ToolStripMenuItem m_MenuItem;

            public VBPopupMenuItem(ToolStripMenuItem menuItem)
                : base(menuItem.Text, menuItem.Image) {
                m_MenuItem = menuItem;

                // プロパティをコピー

                base.Checked = menuItem.Checked;
                base.CheckState = menuItem.CheckState;
                base.Enabled = menuItem.Enabled;
                base.ForeColor = menuItem.ForeColor;
                base.Font = menuItem.Font;
                base.Available = menuItem.Available;    // Available ≒ Visible
                base.ShortcutKeys = menuItem.ShortcutKeys;

                // プロパティの変更をキャッチ
                menuItem.CheckedChanged += menuItem_CheckedChanged;
                menuItem.CheckStateChanged += menuItem_CheckStateChanged;
                menuItem.EnabledChanged += menuItem_EnabledChanged;
                menuItem.ForeColorChanged += menuItem_ForeColorChanged;
                menuItem.TextChanged += menuItem_TextChanged;
                menuItem.VisibleChanged += menuItem_VisibleChanged;
            }

            // メニューが操作されたときのイベント取得

            void menuItem_VisibleChanged(object sender, EventArgs e) {
                base.Visible = m_MenuItem.Visible;
            }

            void menuItem_TextChanged(object sender, EventArgs e) {
                base.Text = m_MenuItem.Text;
            }

            void menuItem_ForeColorChanged(object sender, EventArgs e) {
                base.ForeColor = m_MenuItem.ForeColor;
            }

            void menuItem_EnabledChanged(object sender, EventArgs e) {
                base.Enabled = m_MenuItem.Enabled;
            }

            void menuItem_CheckStateChanged(object sender, EventArgs e) {
                base.CheckState = m_MenuItem.CheckState;
            }

            void menuItem_CheckedChanged(object sender, EventArgs e) {
                base.Checked = m_MenuItem.Checked;
            }

            // クリックされたとき、元メニューのクリックイベントを起こす

            protected override void OnClick(EventArgs e) {
                InvokeOnClick(m_MenuItem);
                base.OnClick(e);
            }
        }
    }
}
