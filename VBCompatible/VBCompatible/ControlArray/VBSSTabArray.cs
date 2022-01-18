using System;
using System.ComponentModel;
using System.Windows.Forms;
using VBCompatible;

namespace VBCompatible.ControlArray
{
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    [ProvideProperty("Index", typeof(VBSSTab))]
    public class VBSSTabArray : VBControllArray<VBSSTab>
    {
        public VBSSTabArray() { }

        public VBSSTabArray(IContainer Container) : base(Container) { }

        protected override void HookUpControl(VBSSTab target) {
            base.HookUpControl(target);
            if (UseMnemonicChanged != null) target.UseMnemonicChanged += UseMnemonicChanged;
            if (DrawItem != null) target.DrawItem += DrawItem;
            if (RightToLeftLayoutChanged != null) target.RightToLeftLayoutChanged += RightToLeftLayoutChanged;
            if (SelectedIndexChanged != null) target.SelectedIndexChanged += SelectedIndexChanged;
            if (Selecting != null) target.Selecting += Selecting;
            if (Selected != null) target.Selected += Selected;
            if (Deselecting != null) target.Deselecting += Deselecting;
            if (Deselected != null) target.Deselected += Deselected;
        }

        public EventHandler UseMnemonicChanged;
        public DrawItemEventHandler DrawItem;
        public EventHandler RightToLeftLayoutChanged;
        public EventHandler SelectedIndexChanged;
        public TabControlCancelEventHandler Selecting;
        public TabControlEventHandler Selected;
        public TabControlCancelEventHandler Deselecting;
        public TabControlEventHandler Deselected;
    }
}
