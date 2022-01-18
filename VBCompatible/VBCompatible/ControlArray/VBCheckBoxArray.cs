using System;
using System.ComponentModel;

namespace VBCompatible.ControlArray
{
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    [ProvideProperty("Index", typeof(VBCheckBox))]
    public class VBCheckBoxArray : VBControllArray<VBCheckBox>
    {
        public VBCheckBoxArray() { }

        public VBCheckBoxArray(IContainer Container) : base(Container) { }

        protected override void HookUpControl(VBCheckBox target) {
            base.HookUpControl(target);
            if (AppearanceChanged != null) target.AppearanceChanged += AppearanceChanged;
            if (CheckedChanged != null) target.CheckedChanged += CheckedChanged;
            if (CheckStateChanged != null) target.CheckStateChanged += CheckStateChanged;
        }

        public EventHandler AppearanceChanged;
        public EventHandler CheckedChanged;
        public EventHandler CheckStateChanged;
    }
}
