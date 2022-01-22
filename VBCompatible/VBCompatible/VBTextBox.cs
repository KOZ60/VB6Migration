using System;
using System.Drawing;
using System.Windows.Forms;

namespace VBCompatible
{
    public class VBTextBox : TextBox
    {
        private readonly VBOnwerDraw ownerDraw;

        public VBTextBox() {
            ResetFont();
            ResetForeColor();
            ResetBackColor();
            ownerDraw = new VBOnwerDraw(this, true, NativeDrawMode.WmPaint);
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

        protected override void OnReadOnlyChanged(EventArgs e) {
            base.OnReadOnlyChanged(e);
            if (IsHandleCreated && ReadOnly) {
                ClearUndo();
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            VBTextBoxRenderer.DrawTextBox(e.Graphics, this, e.ClipRectangle);
            base.OnPaint(e);
        }
    }
}
