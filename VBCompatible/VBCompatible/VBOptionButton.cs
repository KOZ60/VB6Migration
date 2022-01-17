using System.Drawing;
using System.Windows.Forms;

namespace VBCompatible
{
    public class VBOptionButton : RadioButton
    {
        private readonly VBOnwerDraw ownerDraw;

        public VBOptionButton() {
            ResetFont();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            ownerDraw = new VBOnwerDraw(this, false, false);
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
    }
}
