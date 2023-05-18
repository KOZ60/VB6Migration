using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace VBCompatible.ControlArray
{
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    [ProvideProperty("Index", typeof(VBLabel))]
    public class VBLabelArray : VBControllArray<VBLabel>
    {
        public VBLabelArray() { }

        public VBLabelArray(IContainer Container) : base(Container) { }

        protected override void HookUpControl(VBLabel target) {
            if (TextAlignChanged != null) target.TextAlignChanged += TextAlignChanged;
        }

        public EventHandler TextAlignChanged;

    }
}
