using System;
using System.ComponentModel;
using System.Windows.Forms;
using VBCompatible;

namespace VBCompatible.ControlArray
{
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    [ProvideProperty("Index", typeof(VBComboBox))]
    public class VBComboBoxArray : VBControllArray<VBComboBox>
    {
        public VBComboBoxArray() { }

        public VBComboBoxArray(IContainer Container) : base(Container) { }

        protected override void HookUpControl(VBComboBox target) {
            if (ReadOnlyChanged != null) target.ReadOnlyChanged += ReadOnlyChanged;
            if (Scroll != null) target.Scroll += Scroll;
            if (DrawItem != null) target.DrawItem += DrawItem;
            if (DropDown != null) target.DropDown += DropDown;
            if (MeasureItem != null) target.MeasureItem += MeasureItem;
            if (SelectedIndexChanged != null) target.SelectedIndexChanged += SelectedIndexChanged;
            if (SelectionChangeCommitted != null) target.SelectionChangeCommitted += SelectionChangeCommitted;
            if (DropDownStyleChanged != null) target.DropDownStyleChanged += DropDownStyleChanged;
            if (TextUpdate != null) target.TextUpdate += TextUpdate;
            if (DropDownClosed != null) target.DropDownClosed += DropDownClosed;
            if (DataSourceChanged != null) target.DataSourceChanged += DataSourceChanged;
            if (DisplayMemberChanged != null) target.DisplayMemberChanged += DisplayMemberChanged;
            if (Format != null) target.Format += Format;
            if (FormatInfoChanged != null) target.FormatInfoChanged += FormatInfoChanged;
            if (FormatStringChanged != null) target.FormatStringChanged += FormatStringChanged;
            if (FormattingEnabledChanged != null) target.FormattingEnabledChanged += FormattingEnabledChanged;
            if (ValueMemberChanged != null) target.ValueMemberChanged += ValueMemberChanged;
            if (SelectedValueChanged != null) target.SelectedValueChanged += SelectedValueChanged;
        }

        public EventHandler ReadOnlyChanged;
        public EventHandler Scroll;
        public DrawItemEventHandler DrawItem;
        public EventHandler DropDown;
        public MeasureItemEventHandler MeasureItem;
        public EventHandler SelectedIndexChanged;
        public EventHandler SelectionChangeCommitted;
        public EventHandler DropDownStyleChanged;
        public EventHandler TextUpdate;
        public EventHandler DropDownClosed;
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
