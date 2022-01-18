using System;
using System.ComponentModel;

namespace VBCompatible.ControlArray
{
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    [ProvideProperty("Index", typeof(VBFrame))]
    public class VBFrameArray : VBControllArray<VBFrame>
    {
        public VBFrameArray() { }

        public VBFrameArray(IContainer Container) : base(Container) { }

        protected override void HookUpControl(VBFrame target) {
            base.HookUpControl(target);
        }
    }
}
