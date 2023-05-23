using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VBCompatible
{
    [DesignerCategory("Code")]
    public class VBCommandButton : Button
    {
        public VBCommandButton() {
            ResetFont();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
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


        #region VB6.0 互換プロパティ

        private bool m_Value = false;

        /// <summary>
        /// VB6.0 互換 Value プロパティ
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [SR.VB60]
        public bool Value {
            get { return m_Value; }
            set {
                if (value) {
                    OnClick(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Click イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納した EventArgs</param>
        protected override void OnClick(EventArgs e) {
            // Click イベント中なら Value プロパティを True にする
            m_Value = true;
            base.OnClick(e);
            m_Value = false;
        }

        #endregion

    }
}
