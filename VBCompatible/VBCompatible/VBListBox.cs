using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VBCompatible
{
    public class VBListBox : ListBox
    {
        private static object EventScroll = new object();

        public VBListBox() {
            ResetFont();
            ResetForeColor();
            ResetBackColor();
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


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [SR.VB60]
        public int NewIndex { get; private set; } = -1;

        protected override void WndProc(ref Message m) {
            switch (m.Msg) {
                case NativeMethods.LB_ADDSTRING:
                    // 追加されたときは戻り値を NewIndex とする
                    base.WndProc(ref m);
                    NewIndex = m.Result.ToInt32();
                    break;

                case NativeMethods.LB_INSERTSTRING:
                    // 挿入されたときは WParam が NewIndex 
                    NewIndex = m.WParam.ToInt32();
                    base.WndProc(ref m);
                    break;

                case NativeMethods.LB_DELETESTRING:
                case NativeMethods.LB_RESETCONTENT:
                    // 削除、クリアされたときは -1 に戻します
                    base.WndProc(ref m);
                    NewIndex = -1;
                    break;

                case NativeMethods.WM_VSCROLL:
                case NativeMethods.WM_HSCROLL:
                    base.WndProc(ref m);
                    // Scroll イベント
                    OnScroll(EventArgs.Empty);
                    break;

                case NativeMethods.WM_KEYDOWN:
                    int topIndex = base.TopIndex;
                    base.WndProc(ref m);
                    if (topIndex != base.TopIndex) {
                        // Scroll イベント
                        OnScroll(EventArgs.Empty);
                    }
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
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

        /// <summary>
        /// ListBox のすべての項目を更新し、それらの項目の新しい文字列を取得します。
        /// </summary>
        public new void RefreshItems() {
            base.RefreshItems();
        }

        /// <summary>
        /// 指定したインデックスにある項目を更新します。
        /// </summary>
        /// <param name="index">更新する要素の、0 から始まるインデックス。 </param>
        public new void RefreshItem(int index) {
            base.RefreshItem(index);
        }
    }
}
