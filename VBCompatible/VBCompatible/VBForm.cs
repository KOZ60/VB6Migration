using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VBCompatible.VB6;

namespace VBCompatible
{
    [ComVisible(true)]
    public class VBForm : Form, IEnumerable<Control>
    {
        private static object EventTabIndexModeChanged = new object();

        private bool EnterOnChildTabIndexChanged = false;

        private List<Control> m_TabIndexList = new List<Control>(512);         // TabIndex 順に並んだコントロールのリスト
        private TabIndexMode m_TabIndexMode = TabIndexMode.VB6;

        public VBForm() {
            ResetFont();
            ResetForeColor();
            ResetBackColor();
            SetStyle(ControlStyles.ResizeRedraw, true);

            // コントロールが追加/削除されるとき TabIndex リストを更新する
            ControlAdded += ControlAddedEvent;
            ControlRemoved += ControlRemovedEvent;
        }

        /// <summary>
        /// タブキーを押下したときのフォーカス遷移を VB6.0 互換にするかどうかを設定、取得します。
        /// </summary>
        [Description("タブキーを押下したときのフォーカス遷移を VB6.0 互換にするかどうかを設定、取得します。")]
        [DefaultValue(TabIndexMode.VB6)]
        [SR.VBCustom]
        public TabIndexMode TabIndexMode {
            get { 
                return m_TabIndexMode; 
            }
            set {
                if (TabIndexMode != value) {
                    m_TabIndexMode = value;
                    OnTabIndexModeChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// TabIndexModeChanged イベントを発生させます。
        /// </summary>
        protected virtual void OnTabIndexModeChanged(EventArgs e) {
            EventHandler eh = Events[EventTabIndexModeChanged] as EventHandler;
            if (eh != null) eh(this, e);
        }

        /// <summary>
        /// TabIndexMode プロパティの値が変更された場合に発生します。
        /// </summary>
        public event EventHandler TabIndexModeChanged {
            add { Events.AddHandler(EventTabIndexModeChanged, value); }
            remove { Events.RemoveHandler(EventTabIndexModeChanged, value); }
        }

        private void ControlAddedEvent(object sender, ControlEventArgs e) {
            EnumRegistTabIndex(e.Control);          // TabIndex リストへ追加
        }

        // コントロールが削除されるとき
        private void ControlRemovedEvent(object sender, ControlEventArgs e) {
            // TabIndex リストから削除
            EnumRemoveTabIndex(e.Control);
        }

        private void EnumRegistTabIndex(Control con) {
            RegistTabIndex(con, false);
            if (con.HasChildren) {
                for (int i = 0; i < con.Controls.Count; i++) {
                    EnumRegistTabIndex(con.Controls[i]);
                }
            }
        }

        private void EnumRemoveTabIndex(Control con) {
            RemoveTabIndex(con);
            if (con.HasChildren) {
                for (int i = 0; i < con.Controls.Count; i++) {
                    EnumRemoveTabIndex(con.Controls[i]);
                }
            }
        }

        private void RegistTabIndex(Control con, bool renumber) {

            if (!IsRegistControl(con)) return;

            con.TabIndexChanged += OnChildTabIndexChanged;
            con.ControlAdded += ControlAddedEvent;
            con.ControlRemoved += ControlRemovedEvent;

            int start = 0;
            int end = m_TabIndexList.Count - 1;
            int current = con.TabIndex;

            while ((end - start) > 1) {
                int m = (start + end) / 2;
                int mIndex = m_TabIndexList[m].TabIndex;
                if (current > mIndex)
                    start = m;
                else
                    end = m;
            }

            for (int i = start; i <= end; i++) {
                if (m_TabIndexList[i].TabIndex >= current) {
                    m_TabIndexList.Insert(i, con);
                    if (renumber) {
                        int newTabIndex = current + 1;
                        for (int j = i + 1; j < m_TabIndexList.Count; j++) {
                            m_TabIndexList[j].TabIndex = newTabIndex;
                            newTabIndex++;
                        }
                    }
                    return;
                }
            }
            m_TabIndexList.Add(con);
        }

        // DataGridView のように動的にコントロールを追加するものがあるので
        // 親にコンテナで無いものがあったらリストに登録しない

        private static bool IsRegistControl(Control con) {
            Control p = con.Parent;
            while (p != null) {
                if (!p.IsContainer()) {
                    return false;
                }
                p = p.Parent;
            }
            return true;
        }

        private int RemoveTabIndex(Control con) {
            int index = m_TabIndexList.IndexOf(con);
            if (index >= 0) {
                m_TabIndexList.RemoveAt(index);
                con.TabIndexChanged -= OnChildTabIndexChanged;
                con.ControlAdded -= ControlAddedEvent;
                con.ControlRemoved -= ControlRemovedEvent;
            }
            return index;
        }

        // 子コントロールの TabIndex が変更されたとき
        private void OnChildTabIndexChanged(object sender, EventArgs e) {
            // VB6互換モードでなければ処理しない
            if (TabIndexMode != TabIndexMode.VB6) return;

            // TabIndex の振り直しが重複起動されないよう
            if (EnterOnChildTabIndexChanged) return;
            EnterOnChildTabIndexChanged = true;

            // 変更されたコントロール以降の TabIndex を振りなおす
            Control con = (Control)sender;
            RemoveTabIndex(con);
            RegistTabIndex(con, true);

            EnterOnChildTabIndexChanged = false;
        }

        /// <summary>
        /// VB6.0 互換モードで、子コントロールのタブ オーダー内の 1 つ前または 1 つ後ろのコントロールを取得します。
        /// </summary>
        /// <param name="con">検索を開始するコントロール</param>
        /// <param name="forward">タブ オーダー内を前方に検索する場合は true。後方に検索する場合は false。 </param>
        /// <returns>タブ オーダー内の次の Control。 </returns>
        public Control GetNextTabIndexControl(Control con, bool forward) {
            int nextIndex;
            if (con == null)
                nextIndex = forward ? -1 : m_TabIndexList.Count;
            else
                nextIndex = m_TabIndexList.IndexOf(con);

            if (forward) {
                nextIndex++;
                if (nextIndex < m_TabIndexList.Count)
                    return m_TabIndexList[nextIndex];
                else
                    return null;
            } else {
                nextIndex--;
                if (nextIndex >= 0)
                    return m_TabIndexList[nextIndex];
                else
                    return null;
            }
        }

        /// <summary>
        /// VB6.0 互換モードで、子コントロールのタブ オーダー内でフォーカス可能な 1 つ前または 1 つ後ろのコントロールを取得します。
        /// </summary>
        /// <param name="con">検索を開始するコントロール</param>
        /// <param name="forward">タブ オーダー内を前方に検索する場合は true。後方に検索する場合は false。 </param>
        /// <param name="tabStopOnly">TabStop = True のコントロールを検索する場合は true。それ以外も検索する場合は False。</param>
        /// <returns>タブ オーダー内のフォーカス可能な次の Control。 </returns>
        public Control GetNextTabIndexFocus(Control con, bool forward, bool tabStopOnly) {
            Control start = GetNextTabIndexControl(con, forward);
            while (start != null) {
                if (!(start is SplitContainer) &&
                    start.CanSelect &&
                    (!tabStopOnly || start.TabStop))
                    return start;
                start = GetNextTabIndexControl(start, forward);
            };

            if (con == null)
                return null;
            else
                return GetNextTabIndexFocus(null, forward, tabStopOnly);
        }

        /// <summary>
        /// 次のコントロールをアクティブにします。
        /// </summary>
        /// <param name="ctl">検索の開始位置とする Control。</param>
        /// <param name="forward">
        /// タブ オーダー内を前方に移動する場合は true。
        /// 後方に移動する場合は false。
        /// </param>
        /// <param name="tabStopOnly">
        /// TabStop が False に設定されているコントロールを無視する場合は true。
        /// そうでない場合は false。
        /// </param>
        /// <param name="nested">
        /// 入れ子になった (子コントロールの子) 子コントロールを含める場合は true。
        /// それ以外の場合は false。
        /// TabIndexMode == TabIndexMode.VB6 の場合は意味がありません。
        /// </param>
        /// <param name="wrap">
        /// タブ オーダーの最後のコントロールに到達した後、タブ オーダーの最初のコントロールから検索を続行する場合は true。
        /// それ以外の場合は false。
        /// </param>
        /// <returns>コントロールがアクティブにされた場合は true。それ以外の場合は false。</returns>
        public new bool SelectNextControl(Control ctl, bool forward, bool tabStopOnly, bool nested, bool wrap) {
            if (TabIndexMode == TabIndexMode.VB6) {
                Control nextControl = GetNextTabIndexFocus(ctl, forward, tabStopOnly);
                if (nextControl != null) {
                    return nextControl.Focus();
                }
            } else {
                return base.SelectNextControl(ctl, forward, tabStopOnly, nested, wrap);
            }
            return false;
        }

        /// <summary>
        /// 次に使用できるコントロールを選択し、そのコントロールをアクティブにします。
        /// </summary>
        /// <param name="forward">フォーム内のコントロールを循環して選択する場合は true。それ以外の場合は false。 </param>
        /// <returns>コントロールが選択された場合は true。それ以外の場合は false。 </returns>
        protected override bool ProcessTabKey(bool forward) {
            return ProcessTabKeyInternal(forward);
        }

        /// <summary>
        /// 次に使用できるコントロールを選択し、そのコントロールをアクティブにします。
        /// </summary>
        /// <param name="forward">フォーム内のコントロールを循環して選択する場合は true。それ以外の場合は false。 </param>
        /// <returns>コントロールが選択された場合は true。それ以外の場合は false。 </returns>
        public bool ProcessTabKeyInternal(bool forward) {
            if (TabIndexMode == TabIndexMode.VB6) {
                Control nextControl = GetNextTabIndexFocus(FocusedControl, forward, true);
                if (nextControl != null) {
                    return nextControl.Focus();
                }
            } else {
                return base.ProcessTabKey(forward);
            }
            return false;
        }

        /// <summary>
        /// フォーカスを持っているコントロールを返します。
        /// </summary>
        public Control FocusedControl {
            get {
                for (int i = 0; i < m_TabIndexList.Count; i++) {
                    Control con = m_TabIndexList[i];
                    if (con.ContainsFocus && !con.IsContainer()) {
                        return con;
                    }
                }
                return null;
            }
        }

        public IEnumerator<Control> GetEnumerator() {
            return GetControls(this).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetControls(this).GetEnumerator();
        }

        private List<Control> GetControls(Control baseControl) {
            List<Control> result = new List<Control>();
            foreach (Control con in baseControl.Controls) {
                result.Add(con);
                if (!(con is UserControl))
                    result.AddRange(GetControls(con));
            }
            return result;
        }

        public Control this[int index] {
            get {
                return GetControls(this)[index];
            }
        }


        // -------------------------------------------------------------------------------
        // Font プロパティ
        // -------------------------------------------------------------------------------
        private Font m_Font = null;

        /// <summary>
        /// コントロールによって表示されるテキストのフォントを取得または設定します。
        /// </summary>
        /// <remarks>
        /// コントロールによって表示されるテキストに適用される System.Drawing.Font。 既定値は VBCompatible.VBSystem.DefaultFont
        /// </remarks>
        public override Font Font {
            get {
                if (m_Font == null) {
                    return VBSystem.DefaultFont;
                }
                return m_Font;
            }
            set {
                if (value == null) {
                    base.Font = VBSystem.DefaultFont;
                } else {
                    base.Font = value;
                }
                m_Font = value;
            }
        }

        /// <summary>
        /// Font の値を既定値に設定します。
        /// </summary>
        public override void ResetFont() {
            Font = null;
        }

        /// <summary>
        /// Font の値が変更されているかを取得します。
        /// </summary>
        protected virtual bool ShouldSerializeFont() {
            return m_Font != null;
        }

        protected override void WndProc(ref Message m) {
            switch (m.Msg) {
                case NativeMethods.WM_SHOWWINDOW:
                    base.WndProc(ref m);
                    // 表示されるとき ActiveControl が未設定なら VB6 モードで初期フォーカス位置を設定する
                    // Load イベントが終了し、コントロールの Enter イベントが発生する前に行う必要がある
                    if (m.WParam != IntPtr.Zero) {
                        if (this.ActiveControl == null && !this.DesignMode && TabIndexMode == TabIndexMode.VB6) {
                            this.ActiveControl = GetNextTabIndexFocus(null, true, true);
                        }
                    }
                    break;
                default:
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

        /// <summary>
        /// VB6.0互換のショートカット メニューを表示します。
        /// ショートカットメニューの表示される位置は、マウスポインタの位置により自動調整されます。
        /// </summary>
        /// <param name="menu">表示するショートカット メニューの名前を指定します。</param>
        /// <param name="flags">指定しても無視されます。</param>
        /// <param name="x">指定しても無視されます。</param>
        /// <param name="y">指定しても無視されます。</param>
        /// <param name="defaultMenu">指定しても無視されます。</param>
        [SR.VB60]
        public void PopupMenu(ToolStripMenuItem menu,
                              MenuControlConstants flags = MenuControlConstants.vbPopupMenuLeftButton,
                              int x = int.MinValue,
                              int y = int.MinValue,
                              ToolStripMenuItem defaultMenu = null) {
            this.Capture = true;  // マウスキャプチャを取得
            VBPopupMenu popup = new VBPopupMenu(menu);
            Point mouseClientPosition = this.PointToClient(Control.MousePosition);
            popup.Show(this, mouseClientPosition);
        }


    }
}
