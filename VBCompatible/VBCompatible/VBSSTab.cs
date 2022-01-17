using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace VBCompatible
{
    /// <summary>
    /// VB6.0 SSTab 互換コントロール
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    public class VBSSTab : TabControl
    {
        [ThreadStatic]
        private static VisualStyleRenderer s_Renderer = null;

        private static VisualStyleRenderer Renderer {
            get {
                if (s_Renderer == null) {
                    s_Renderer = new VisualStyleRenderer(VisualStyleElement.Tab.TabItem.Normal);
                }
                return s_Renderer;
            }
        }

        private VBTabPageCollection m_TabPages;
        private TabVisibleClass m_TabVisibleClass;
        private bool m_EventSkip = false;

        /// <summary>
        /// VBSSTab のインスタンスを作成します。
        /// </summary>
        public VBSSTab() {
            ResetFont();
            m_TabPages = new VBTabPageCollection(this);
            m_TabVisibleClass = new TabVisibleClass(m_TabPages);
            base.Appearance = TabAppearance.Normal;
            base.ResizeRedraw = true;
            base.DrawMode = TabDrawMode.OwnerDrawFixed;
        }


        #region Font

        private Font _Font = null;

        // -------------------------------------------------------------------------------
        // Font プロパティ
        // -------------------------------------------------------------------------------
        public override Font Font {
            get {
                if (_Font == null) {
                    Form form = FindForm();
                    if (form != null) {
                        return form.Font;
                    } else {
                        return VBSystem.DefaultFont;
                    }
                }
                return _Font;
            }
            set {
                base.Font = value;
                _Font = value;
            }
        }

        public override void ResetFont() {
            Font = null;
        }

        protected virtual bool ShouldSerializeFont() {
            return _Font != null;
        }

        #endregion

        #region VB6 互換プロパティ

        // VB6 互換プロパティ

        /// <summary>
        /// タブ ダイアログ (SSTab) コントロール内のタブを表示するか非表示にするかを取得または設定します。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TabVisibleClass TabVisible {
            get { return m_TabVisibleClass; }
        }

        /// <summary>
        /// 現在選択されているタブ ページのインデックスを取得または設定します。
        /// </summary>
        public new int SelectedIndex {
            get { return ToSSTabIndex(base.SelectedIndex); }
            set { base.SelectedIndex = FromSSTabIndex(value); }
        }

        /// <summary>
        /// TabControl の Index を VBSSTab の Index 値に変換
        /// </summary>
        /// <param name="value">TabControl の Index 値</param>
        /// <returns>VBSSTab の Index 値</returns>
        private int ToSSTabIndex(int value) {
            int tabControlIndex = 0;
            for (int i = 0; i < m_TabPages.Count; i++) {
                if (m_TabPages.get_Visible(i)) {
                    if (tabControlIndex == value)
                        return i;
                    tabControlIndex++;
                }
            }
            return -1;
        }

        /// <summary>
        /// VBSSTab の Index を TabControl の Index 値に変換
        /// </summary>
        /// <param name="value">TabControl の Index 値</param>
        /// <returns>VBSSTab の Index 値</returns>
        private int FromSSTabIndex(int value) {
            if (m_TabPages.get_Visible(value)) {
                int tabControlIndex = 0;
                for (int i = 0; i < value; i++) {
                    if (m_TabPages.get_Visible(i)) {
                        tabControlIndex++;
                    }
                }
                return tabControlIndex;
            } else
                return -1;
        }

        /// <summary>
        /// SelectedIndexChanged イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納した EventArgs</param>
        protected override void OnSelectedIndexChanged(EventArgs e) {
            if (m_EventSkip == false && base.FindForm() != null)
                base.OnSelectedIndexChanged(e);
        }

        /// <summary>
        /// Selected イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納した TabControlEventArgs</param>
        protected override void OnSelected(TabControlEventArgs e) {
            if (m_EventSkip == false && base.FindForm() != null) {
                TabControlEventArgs newEvent = new TabControlEventArgs(e.TabPage, ToSSTabIndex(e.TabPageIndex), e.Action);
                base.OnSelected(newEvent);
            }
        }

        /// <summary>
        /// Deselected イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納した TabControlEventArgs</param>
        protected override void OnDeselected(TabControlEventArgs e) {
            if (m_EventSkip == false && base.FindForm() != null) {
                TabControlEventArgs newEvent = new TabControlEventArgs(e.TabPage, ToSSTabIndex(e.TabPageIndex), e.Action);
                base.OnDeselected(newEvent);
            }
        }

        /// <summary>
        /// Deselecting イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納した TabControlEventArgs</param>
        protected override void OnDeselecting(TabControlCancelEventArgs e) {
            if (m_EventSkip == false && base.FindForm() != null) {
                TabControlCancelEventArgs newEvent = new TabControlCancelEventArgs(e.TabPage, ToSSTabIndex(e.TabPageIndex), e.Cancel, e.Action);
                base.OnDeselecting(newEvent);
                e.Cancel = newEvent.Cancel;
            }
        }

        /// <summary>
        /// Selecting イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納した TabControlCancelEventArgs</param>
        protected override void OnSelecting(TabControlCancelEventArgs e) {
            if (m_EventSkip == false && base.FindForm() != null) {
                if (e.TabPage.Enabled) {
                    TabControlCancelEventArgs newEvent = new TabControlCancelEventArgs(e.TabPage, ToSSTabIndex(e.TabPageIndex), e.Cancel, e.Action);
                    base.OnSelecting(e);
                    e.Cancel = newEvent.Cancel;
                } else
                    e.Cancel = true;
            }
        }

        // プロパティを Shadow
        /// <summary>
        /// コントロール内に格納されている TabPage のコレクションを取得します。
        /// </summary>
        [Browsable(false)]
        public new VBTabPageCollection Controls {
            get { return m_TabPages; }
        }

        /// <summary>
        /// コントロール内に格納されている TabPage のコレクションを取得します。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new VBTabPageCollection TabPages {
            get { return m_TabPages; }
        }

        // TabPage の表示/非表示を保持するクラス
        private class TabPageVisible
        {
            private bool m_Visible;
            private TabPage m_Page;
            private HookWindow _HookWindow;

            public event EventHandler VisibleChanged;

            public TabPageVisible(TabPage page) {
                m_Page = page;
                m_Visible = true;
                m_Page.HandleCreated += ControlHandleCreated;
                if (m_Page.IsHandleCreated) {
                    ControlHandleCreated(m_Page, EventArgs.Empty);
                }
            }

            public bool Visible {
                get { return m_Visible; }
                set {
                    if (m_Visible != value) {
                        m_Visible = value;
                        OnVisibleChanged(EventArgs.Empty);
                    }
                }
            }

            public TabPage Page {
                get { return m_Page; }
                set { m_Page = value; }
            }

            protected virtual void OnVisibleChanged(EventArgs e) {
                if (VisibleChanged != null) VisibleChanged(m_Page, EventArgs.Empty);
            }

            private void ControlHandleCreated(object sender, EventArgs e) {
                _HookWindow = new HookWindow(this);
                _HookWindow.AssignHandle(m_Page.Handle);
            }

            internal void ReleaseWindow() {
                if (_HookWindow != null) {
                    _HookWindow.ReleaseHandle();
                    _HookWindow = null;
                }
            }

            private class HookWindow : NativeWindow
            {
                TabPageVisible _Owner;
                public HookWindow(TabPageVisible owner) {
                    _Owner = owner;
                }

                /// <summary>
                /// Windows メッセージを処理します。
                /// </summary>
                /// <param name="m">処理対象の Windows Message。 </param>
                protected override void WndProc(ref Message m) {
                    switch (m.Msg) {
                        case NativeMethods.WM_NCDESTROY:
                            base.WndProc(ref m);
                            break;
                        default:
                            // コンテナ共通処理
                            if (VBUtils.IsCtlColor(ref m)) {
                                Control con = Control.FromHandle(m.LParam);
                                if (con != null) {
                                    VBUtils.SetCtlColor(con, ref m);
                                    return;
                                }
                                base.DefWndProc(ref m);
                            } else {
                                base.WndProc(ref m);
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// TabPage の表示/非表示を保持するクラスのコレクション
        /// </summary>
        public class VBTabPageCollection : IEnumerable
        {
            VBSSTab m_Owner;
            List<TabPageVisible> m_List;

            internal VBTabPageCollection(VBSSTab owner) {
                m_Owner = owner;
                m_List = new List<TabPageVisible>();
            }

            /// <summary>
            /// TabPage の表示/非表示を取得します。
            /// </summary>
            /// <param name="index">インデックスを指定します。</param>
            /// <returns>表示するときは True、そうでないときは、False</returns>
            [EditorBrowsable(EditorBrowsableState.Never)]
            public bool get_Visible(int index) {
                return m_List[index].Visible;
            }

            /// <summary>
            /// TabPage の表示/非表示を設定します。
            /// </summary>
            /// <param name="index">インデックスを指定します。</param>
            /// <param name="value">表示するときは True、そうでないときは、False</param>
            [EditorBrowsable(EditorBrowsableState.Never)]
            public void set_Visible(int index, bool value) {
                m_List[index].Visible = value;
            }

            /// <summary>
            /// 指定された位置の TabPage を取得します。
            /// </summary>
            /// <param name="index">インデックスを指定します。</param>
            /// <returns>指定された位置の TabPage。</returns>
            public TabPage this[int index] {
                get { return m_List[index - 1].Page; }
            }

            /// <summary>
            /// コレクションに含まれる TabPage の総数を取得します。
            /// </summary>
            public int Count {
                get { return m_List.Count; }
            }

            private TabPageCollection TabPages {
                get { return ((TabControl)m_Owner).TabPages; }
            }

            /// <summary>
            /// タブ ページをコレクションに追加します。
            /// </summary>
            /// <param name="page">追加する TabPage</param>
            public void Add(TabPage page) {
                page.BackColor = page.BackColor;
                TabPageVisible item = new TabPageVisible(page);
                item.VisibleChanged += OnTabVisibleChanged;
                m_List.Add(item);
                TabPages.Add(page);
            }

            /// <summary>
            /// タブ ページをコレクションから削除します。
            /// </summary>
            /// <param name="index">削除するタブページの位置を指定します。</param>
            public void Remove(int index) {
                TabPageVisible item = m_List[index];
                m_List.RemoveAt(index);
                item.VisibleChanged -= OnTabVisibleChanged;
                item.ReleaseWindow();

                // デザイン中なら TabPages から削除
                // 実行中は TabPage を再構成する

                if (m_Owner.DesignMode)
                    TabPages.RemoveAt(index);
                else
                    Recreate();
            }

            IEnumerator IEnumerable.GetEnumerator() {
                foreach (var item in m_List) {
                    yield return item.Page;
                }
            }

            /// <summary>
            /// タブページの表示/非表示が切り替わったときに発生します。
            /// </summary>
            /// <param name="sender">イベントが発生したタブページ。</param>
            /// <param name="e">イベント データを格納した EventArgs</param>
            protected void OnTabVisibleChanged(object sender, EventArgs e) {
                if (m_Owner.DesignMode) return;     // デザインモードでは何もしない
                Recreate();
            }

            private void Recreate() {
                m_Owner.m_EventSkip = true;

                NativeMethods.SendMessage(m_Owner.Handle, NativeMethods.WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
                int selectedIndex = ((TabControl)m_Owner).SelectedIndex;

                TabPages.Clear();

                for (int i = 0; i < this.Count; i++) {
                    m_List[i].Page.UseVisualStyleBackColor = false;
                    if (m_List[i].Visible) TabPages.Add(m_List[i].Page);
                }
                if (selectedIndex == -1) {
                    if (TabPages.Count == 0)
                        ((TabControl)m_Owner).SelectedIndex = -1;
                    else
                        ((TabControl)m_Owner).SelectedIndex = 0;
                } else if (selectedIndex < TabPages.Count)
                    ((TabControl)m_Owner).SelectedIndex = selectedIndex;
                else if (TabPages.Count != 0)
                    ((TabControl)m_Owner).SelectedIndex = 0;
                else
                    ((TabControl)m_Owner).SelectedIndex = -1;

                m_Owner.Refresh();

                m_Owner.m_EventSkip = false;
                m_Owner.OnSelectedIndexChanged(EventArgs.Empty);
            }

            /// <summary>
            /// コレクションがゼロオリジンかどうかを取得します。
            /// </summary>
            public bool ZeroOrigin {
                get { return true; }
            }
        }

        /// <summary>
        /// TabPage の表示/非表示を設定、取得します。
        /// </summary>
        public class TabVisibleClass
        {
            private VBTabPageCollection m_Owner;
            internal TabVisibleClass(VBTabPageCollection owner) {
                m_Owner = owner;
            }
            /// <summary>
            /// TabPage の表示/非表示を設定、取得します。
            /// </summary>
            /// <param name="index">設定、取得する TabPage の Index </param>
            /// <returns>表示するときは True しないときは False が帰ります。</returns>
            public bool this[int index] {
                get { return m_Owner.get_Visible(index); }
                set { m_Owner.set_Visible(index, value); }
            }
        }

        #endregion

        #region ニーモニックの処理

        /// <summary>
        /// ニーモニック文字を処理します。
        /// </summary>
        /// <param name="charCode">処理対象の文字。 </param>
        /// <returns>文字がコントロールによってニーモニックとして処理された場合は true。それ以外の場合は false。 </returns>
        protected override bool ProcessMnemonic(char charCode) {
            if (UseMnemonic && base.Enabled && base.Visible) {
                foreach (TabPage p in base.TabPages) {
                    if (Control.IsMnemonic(charCode, p.Text)) {
                        base.SelectedTab = p;
                        return true;
                    }
                }
            }
            return base.ProcessMnemonic(charCode);
        }

        // -------------------------------------------------------------------------------
        // UseMnemonic プロパティ
        // -------------------------------------------------------------------------------
        private bool m_UseMnemonic = true;

        /// <summary>
        /// TabPage の Text プロパティに含まれるアンパサンド文字 (&amp;) をアクセス キーの接頭文字として解釈するかどうかを示す値を取得または設定します。
        /// </summary>
        [DefaultValue(true)]
        public bool UseMnemonic {
            get {
                return m_UseMnemonic;
            }
            set {
                if (UseMnemonic != value) {
                    m_UseMnemonic = value;
                    if (base.IsHandleCreated) {
                        base.Invalidate();
                    }
                    OnUseMnemonicChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// UseMnemonicChanged イベントを発生させます。
        /// </summary>
        protected virtual void OnUseMnemonicChanged(EventArgs e) {
            if (UseMnemonicChanged != null) UseMnemonicChanged(this, e);
        }

        /// <summary>
        /// UseMnemonic プロパティの値が変更された場合に発生します。
        /// </summary>
        public event EventHandler UseMnemonicChanged;

        #endregion
    }
}
