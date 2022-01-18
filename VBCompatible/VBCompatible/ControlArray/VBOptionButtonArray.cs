using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace VBCompatible.ControlArray
{
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    [ProvideProperty("Index", typeof(VBOptionButton))]
    public class VBOptionButtonArray : VBControllArray<VBOptionButton>
    {
        public VBOptionButtonArray() { }

        public VBOptionButtonArray(IContainer Container) : base(Container) { }

        protected override void HookUpControl(VBOptionButton target) {
            base.HookUpControl(target);
            if (AppearanceChanged != null) target.AppearanceChanged += AppearanceChanged;
            if (CheckedChanged != null) target.CheckedChanged += CheckedChanged;
        }

        public event EventHandler AppearanceChanged;
        public event EventHandler CheckedChanged;
    }
}
