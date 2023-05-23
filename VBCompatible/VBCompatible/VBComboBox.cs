using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VBCompatible
{
    [DesignerCategory("Code")]
    public class VBComboBox : ComboBox
    {
        private static object EventReadOnlyChanged = new object();
        private static object EventScroll = new object();

        private bool _ReadOnly = false;

        public VBComboBox() {
            ResetFont();
            ResetForeColor();
            ResetBackColor();
            SetDrawMode();
        }

        #region Font/ForeColor/BackColor

        private Font _Font = null;
        private Color _ForeColor = Color.Empty;
        private Color _BackColor = Color.Empty;

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

        // -------------------------------------------------------------------------------
        // ForeColor プロパティ
        // -------------------------------------------------------------------------------
        public override Color ForeColor {
            get {
                if (_ForeColor.IsEmpty) {
                    return SystemColors.WindowText;
                }
                return _ForeColor;
            }
            set {
                base.ForeColor = value.IsEmpty ? SystemColors.WindowText : value;
                _ForeColor = value;
                if (IsHandleCreated) {
                    Invalidate();
                }
            }
        }

        public override void ResetForeColor() {
            ForeColor = Color.Empty;
        }

        protected virtual bool ShouldSerializeForeColor() {
            return !_ForeColor.IsEmpty;
        }

        // -------------------------------------------------------------------------------
        // BackColor プロパティ
        // -------------------------------------------------------------------------------

        public override Color BackColor {
            get {
                if (_BackColor.IsEmpty) {
                    return SystemColors.Window;
                }
                return _BackColor;
            }
            set {
                base.BackColor = value.IsEmpty ? SystemColors.Window.ToArgbColor() : value;
                _BackColor = value;
                if (IsHandleCreated) {
                    Invalidate();
                }
            }
        }

        public override void ResetBackColor() {
            BackColor = Color.Empty;
        }

        protected virtual bool ShouldSerializeBackColor() {
            return !_BackColor.IsEmpty;
        }

        #endregion

        // -------------------------------------------------------------------------------
        // DropDownList の見た目をVB6互換に
        // -------------------------------------------------------------------------------

        private void SetDrawMode() {
            if (this.DropDownStyle == ComboBoxStyle.DropDownList) {
                if (Application.RenderWithVisualStyles)
                    base.DrawMode = DrawMode.OwnerDrawFixed;
                else
                    base.DrawMode = DrawMode.Normal;
            } else {
                base.DrawMode = DrawMode.Normal;
            }
            // 再描画
            if (IsHandleCreated) {
                Invalidate();
            }
        }

        /// <summary>
        /// DropDownStyleChangedイベントを発生します。
        /// </summary>
        /// <param name="e">イベント データを格納した EventArgs</param>
        protected override void OnDropDownStyleChanged(EventArgs e) {
            base.OnDropDownStyleChanged(e);
            SetDrawMode();
        }

        /// <summary>
        /// DrawItem イベントを発生します。
        /// </summary>
        /// <param name="e">イベント データを格納した DrawItemEventArgs</param>
        protected override void OnDrawItem(DrawItemEventArgs e) {

            Graphics g = e.Graphics;
            TextFormatFlags flags = (this.RightToLeft == RightToLeft.Yes) ? TextFormatFlags.Right : TextFormatFlags.Left;
            flags |= TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding;
            Color foreColor;

            if (e.State.HasFlag(DrawItemState.Selected)) {
                foreColor = SystemColors.HighlightText;
            } else {
                if (this.IsEnabled()) {
                    foreColor = ForeColor;
                } else {
                    foreColor = SystemColors.GrayText;
                }
            }

            string txt;
            if (e.Index >= 0) {
                txt = base.Items[e.Index].ToString();
            } else {
                txt = base.Text;
            }

            e.DrawBackground();
            TextRenderer.DrawText(g, txt, this.Font, e.Bounds, foreColor, flags);

            e.DrawFocusRectangle();
        }

        /// <summary>
        /// EnabledChanged イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している EventArgs。</param>
        protected override void OnEnabledChanged(EventArgs e) {
            base.OnEnabledChanged(e);
            Invalidate(true);
        }

        // -------------------------------------------------------------------------------
        // ReadOnly を実装
        // -------------------------------------------------------------------------------
        protected IntPtr EditBoxHandle;
        protected IntPtr ListBoxHandle;
        private EditWindow EditBoxWindow;
        private ListWindow ListBoxWindow;

        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            var info = new NativeMethods.ComboBoxInfo();
            NativeMethods.GetComboBoxInfo(Handle, info);
            EditBoxHandle = info.EditBoxHandle;
            ListBoxHandle = info.ListBoxHandle;
            EditBoxWindow = new EditWindow(this);
            ListBoxWindow = new ListWindow(this);
            if (ReadOnly) {
                SetReadOnly(ReadOnly);
            }
        }

        protected override void OnHandleDestroyed(EventArgs e) {
            EditBoxWindow.ReleaseHandle();
            ListBoxWindow.ReleaseHandle();
            EditBoxWindow = null;
            ListBoxWindow = null;
            base.OnHandleDestroyed(e);
        }

        private class EditWindow : NativeWindow
        {
            VBComboBox m_Owner;

            public EditWindow(VBComboBox owner) {
                m_Owner = owner;
                this.AssignHandle(owner.EditBoxHandle);
            }

            [DebuggerStepThrough]
            protected override void WndProc(ref Message m) {
                switch (m.Msg) {
                    case NativeMethods.WM_CHAR:
                        if (m.WParam == (IntPtr)NativeMethods.CTRL_A) m_Owner.SelectAll();
                        if ((!m_Owner.ReadOnly) || (m.WParam == (IntPtr)NativeMethods.CTRL_C)) base.WndProc(ref m);
                        break;

                    case NativeMethods.WM_CUT:
                    case NativeMethods.WM_CLEAR:
                    case NativeMethods.WM_UNDO:
                    case NativeMethods.WM_SETTEXT:
                    case NativeMethods.WM_PASTE:
                        if (!m_Owner.ReadOnly) base.WndProc(ref m);
                        break;

                    default:
                        base.WndProc(ref m);
                        break;
                }
            }
        }

        private class ListWindow : NativeWindow
        {
            VBComboBox m_Owner;

            public ListWindow(VBComboBox owner) {
                m_Owner = owner;
                base.AssignHandle(owner.ListBoxHandle);
            }

            [DebuggerStepThrough]
            protected override void WndProc(ref Message m) {
                switch (m.Msg) {
                    case NativeMethods.WM_VSCROLL:
                    case NativeMethods.WM_HSCROLL:
                        base.WndProc(ref m);
                        m_Owner.OnScroll(EventArgs.Empty);
                        break;

                    default:
                        if (m_Owner.ReadOnly && IsIgnoreMessage(ref m)) break;
                        base.WndProc(ref m);
                        break;
                }
            }

            private bool IsIgnoreMessage(ref Message m) {
                // F4 押下メッセージ以外は無視
                if (m.Msg >= NativeMethods.WM_KEYFIRST && m.Msg <= NativeMethods.WM_KEYLAST) {
                    if (m.Msg == NativeMethods.WM_KEYDOWN && (Keys)m.WParam == Keys.F4) return false;
                    return true;
                }

                // リストウインドウ内のメッセージであれば無視
                if (m.Msg >= NativeMethods.WM_MOUSEFIRST && m.Msg <= NativeMethods.WM_MOUSELAST) {
                    var clientRect = new NativeMethods.RECT();
                    if (NativeMethods.GetClientRect(Handle, ref clientRect)) {
                        int x = NativeMethods.SignedLOWORD(m.LParam);
                        int y = NativeMethods.SignedHIWORD(m.LParam);
                        if (clientRect.Contains(x, y)) {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// コントロールが読み取り専用かどうかを取得または設定します。
        /// </summary>
        [SR.VB60]
        [DefaultValue(false)]
        public virtual bool ReadOnly {
            get {
                return _ReadOnly; 
            }
            set {
                if (_ReadOnly != value) {
                    _ReadOnly = value;
                    if (IsHandleCreated) {
                        SetReadOnly(value);
                    }
                    OnReadOnlyChanged(EventArgs.Empty);
                }
            }
        }

        private void SetReadOnly(bool flag) {
            IntPtr wp = flag ? (IntPtr)1 : IntPtr.Zero;
            NativeMethods.SendMessage(EditBoxHandle, NativeMethods.EM_SETREADONLY, wp, IntPtr.Zero);
            if (flag) {
                ClearUndo();
            }
        }

        /// <summary>
        /// ReadOnlyChanged イベントを発生させます。
        /// </summary>
        protected virtual void OnReadOnlyChanged(EventArgs e) {
            EventHandler eh = Events[EventReadOnlyChanged] as EventHandler;
            if (eh != null) eh(this, e);
        }

        /// <summary>
        /// ReadOnly プロパティの値が変更された場合に発生します。
        /// </summary>
        public event EventHandler ReadOnlyChanged {
            add {
                Events.AddHandler(EventReadOnlyChanged, value);
            }
            remove {
                Events.RemoveHandler(EventReadOnlyChanged, value);
            }
        }

        public override string Text { 
            get {
                return base.Text;
            }
            set {
                bool tmp = _ReadOnly;
                _ReadOnly = false;
                base.Text = value;
                _ReadOnly = tmp;
            }
        }

        private IntPtr SendMessageEditBox(int msg) {
            return NativeMethods.SendMessage(EditBoxHandle, msg, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// テキスト ボックス コントロールでユーザーが直前の操作を元に戻すことができるかどうかを示す値を取得します。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [SR.VBCustom]
        public virtual bool CanUndo {
            get {
                IntPtr result = SendMessageEditBox(NativeMethods.EM_CANUNDO);
                return (result != IntPtr.Zero);
            }
        }

        /// <summary>
        /// テキスト ボックスのアンドゥ バッファーから直前に実行された操作に関する情報を削除します。
        /// </summary>
        public virtual void ClearUndo() {
            SendMessageEditBox(NativeMethods.EM_EMPTYUNDOBUFFER);
        }

        /// <summary>
        /// テキスト ボックスの現在の選択項目をクリップボードに移動します。
        /// </summary>
        public void Cut() {
            SendMessageEditBox(NativeMethods.WM_CUT);
        }

        /// <summary>
        /// テキスト ボックスの現在の選択項目をクリップボードにコピーします。
        /// </summary>
        public void Copy() {
            SendMessageEditBox(NativeMethods.WM_COPY);
        }

        /// <summary>
        /// テキスト ボックスの現在の選択項目をクリップボードの内容と置き換えます。
        /// </summary>
        public void Paste() {
            SendMessageEditBox(NativeMethods.WM_PASTE);
        }

        /// <summary>
        /// テキスト ボックスで直前に実行された編集操作を元に戻します。
        /// </summary>
        public void Undo() {
            SendMessageEditBox(NativeMethods.WM_UNDO);
        }

        /// <summary>
        /// Scroll イベントを発生します。
        /// </summary>
        /// <param name="e">イベント データを格納した EventArgs</param>
        protected virtual void OnScroll(EventArgs e) {
            EventHandler eh = Events[EventScroll] as EventHandler;
            if (eh != null) eh(this, e);
        }

        /// <summary>
        /// VB6.0 互換 Scroll イベント
        /// </summary>
        [SR.VB60]
        public event EventHandler Scroll {
            add {
                Events.AddHandler(EventScroll, value);
            }
            remove {
                Events.RemoveHandler(EventScroll, value);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [SR.VB60]
        public int NewIndex { get; private set; } = -1;

        private bool _SetTextIgnore = false;

        protected override void WndProc(ref Message m) {
            switch (m.Msg) {
                case NativeMethods.CB_ADDSTRING:
                    // 追加されたときは戻り値が NewIndex 
                    base.WndProc(ref m);
                    NewIndex = m.Result.ToInt32();
                    break;

                case NativeMethods.CB_INSERTSTRING:
                    // 挿入されたときは WParam が NewIndex 
                    base.WndProc(ref m);
                    NewIndex = m.WParam.ToInt32();
                    break;

                case NativeMethods.CB_DELETESTRING:
                    // 削除されたときは -1 に戻します
                    base.WndProc(ref m);
                    NewIndex = -1;
                    break;

                case NativeMethods.CB_RESETCONTENT:
                    // クリアされたときは -1 に戻します
                    base.WndProc(ref m);
                    NewIndex = -1;
                    // .NET の ComboBox では、Items.Clear する際、テキストをバックアップし、クリアした後戻している
                    // VB6 と互換にするには、次回の WM_SETTEXT を無視する
                    if (this.DropDownStyle != ComboBoxStyle.DropDownList) {
                        _SetTextIgnore = true;
                    }
                    break;

                case NativeMethods.WM_SETTEXT:
                    if (!_SetTextIgnore) {
                        base.WndProc(ref m);
                    }
                    _SetTextIgnore = false;
                    break;

                default:
                    if (VBUtils.IsCtlColor(ref m)) {
                        if (m.LParam == EditBoxHandle || m.LParam == ListBoxHandle) {
                            VBUtils.SetCtlColor(this, ref m);
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
