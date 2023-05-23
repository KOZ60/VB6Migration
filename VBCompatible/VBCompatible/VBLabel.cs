using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace VBCompatible
{
    [DesignerCategory("Code")]
    [Designer(typeof(System.Windows.Forms.Design.ControlDesigner))]
    public class VBLabel : Label
    {
        public VBLabel() {
            ResetFont();
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        protected override void InitLayout() {
            base.InitLayout();
            AutoSize = false;
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
