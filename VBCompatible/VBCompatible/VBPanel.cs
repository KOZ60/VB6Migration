using System.Drawing;
using System.Windows.Forms;

namespace VBCompatible
{
    public class VBPanel : Panel
    {
        public VBPanel() {
            ResetFont();
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);
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

        protected override void WndProc(ref Message m) {
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
        }

    }
}
