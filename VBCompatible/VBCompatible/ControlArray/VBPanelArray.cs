using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace VBCompatible.ControlArray
{
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    [ProvideProperty("Index", typeof(VBPanel))]
    public class VBPanelArray : VBControllArray<VBPanel>
    {
        public VBPanelArray() { }

        public VBPanelArray(IContainer Container) : base(Container) { }

        protected override void HookUpControl(VBPanel target) {
            base.HookUpControl(target);
            if (Scroll != null) target.Scroll += Scroll;
        }

        public ScrollEventHandler Scroll;
    }
}
