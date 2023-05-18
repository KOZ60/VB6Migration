using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace VBCompatible.ControlArray
{
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    [ProvideProperty("Index", typeof(VBTextBox))]
    public class VBTextBoxArray : VBControllArray<VBTextBox>
    {
        public VBTextBoxArray() { }

        public VBTextBoxArray(IContainer Container) : base(Container) { }

        protected override void HookUpControl(VBTextBox target) {
            if (TextAlignChanged != null) target.TextAlignChanged += TextAlignChanged;
            if (AcceptsTabChanged != null) target.AcceptsTabChanged += AcceptsTabChanged;
            if (BorderStyleChanged != null) target.BorderStyleChanged += BorderStyleChanged;
            if (HideSelectionChanged != null) target.HideSelectionChanged += HideSelectionChanged;
            if (ModifiedChanged != null) target.ModifiedChanged += ModifiedChanged;
            if (MultilineChanged != null) target.MultilineChanged += MultilineChanged;
            if (ReadOnlyChanged != null) target.ReadOnlyChanged += ReadOnlyChanged;
        }

        public EventHandler TextAlignChanged;
        public EventHandler AcceptsTabChanged;
        public EventHandler BorderStyleChanged;
        public EventHandler HideSelectionChanged;
        public EventHandler ModifiedChanged;
        public EventHandler MultilineChanged;
        public EventHandler ReadOnlyChanged;

    }
}
