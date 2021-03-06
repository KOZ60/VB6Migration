using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace VBCompatible.ControlArray
{
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    [ProvideProperty("Index", typeof(VBListBox))]
    public class VBListBoxArray : VBControllArray<VBListBox>
    {
        public VBListBoxArray() { }

        public VBListBoxArray(IContainer Container) : base(Container) { }

        protected override void HookUpControl(VBListBox target) {
            base.HookUpControl(target);
            if (Scroll != null) target.Scroll += Scroll;
            if (DrawItem != null) target.DrawItem += DrawItem;
            if (MeasureItem != null) target.MeasureItem += MeasureItem;
            if (SelectedIndexChanged != null) target.SelectedIndexChanged += SelectedIndexChanged;
            if (DataSourceChanged != null) target.DataSourceChanged += DataSourceChanged;
            if (DisplayMemberChanged != null) target.DisplayMemberChanged += DisplayMemberChanged;
            if (Format != null) target.Format += Format;
            if (FormatInfoChanged != null) target.FormatInfoChanged += FormatInfoChanged;
            if (FormatStringChanged != null) target.FormatStringChanged += FormatStringChanged;
            if (FormattingEnabledChanged != null) target.FormattingEnabledChanged += FormattingEnabledChanged;
            if (ValueMemberChanged != null) target.ValueMemberChanged += ValueMemberChanged;
            if (SelectedValueChanged != null) target.SelectedValueChanged += SelectedValueChanged;
        }

        public EventHandler Scroll;
        public DrawItemEventHandler DrawItem;
        public MeasureItemEventHandler MeasureItem;
        public EventHandler SelectedIndexChanged;
        public EventHandler DataSourceChanged;
        public EventHandler DisplayMemberChanged;
        public ListControlConvertEventHandler Format;
        public EventHandler FormatInfoChanged;
        public EventHandler FormatStringChanged;
        public EventHandler FormattingEnabledChanged;
        public EventHandler ValueMemberChanged;
        public EventHandler SelectedValueChanged;
    }
}
