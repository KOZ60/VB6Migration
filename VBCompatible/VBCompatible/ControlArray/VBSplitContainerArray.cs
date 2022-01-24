using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace VBCompatible.ControlArray
{
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    [ProvideProperty("Index", typeof(VBSplitContainer))]
    public class VBSplitContainerArray : VBControllArray<VBSplitContainer>
    {
        public VBSplitContainerArray() { }

        public VBSplitContainerArray(IContainer Container) : base(Container) { }

        protected override void HookUpControl(VBSplitContainer target)
        {
            base.HookUpControl(target);
            if (AutoValidateChanged != null) target.AutoValidateChanged += AutoValidateChanged;
            if (Scroll != null) target.Scroll += Scroll;
            if (SplitterMoved != null) target.SplitterMoved += SplitterMoved;
            if (SplitterMoving != null) target.SplitterMoving += SplitterMoving;
        }

        public EventHandler AutoValidateChanged;
        public ScrollEventHandler Scroll;
        public SplitterEventHandler SplitterMoved;
        public SplitterCancelEventHandler SplitterMoving;
    }
}
