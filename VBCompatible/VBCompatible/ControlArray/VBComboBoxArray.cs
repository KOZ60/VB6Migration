namespace VBCompatible.ControlArray
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [ProvideProperty("Index", typeof(VBComboBox))]
    public class VBComboBoxArray : BaseControlArray<VBComboBox>
    {

        public VBComboBoxArray() { }

        public VBComboBoxArray(IContainer Container) : base(Container) { }

        protected override void HookUpEvents(VBComboBox o) {
            o.BackgroundImageChanged += OnBackgroundImageChanged;
            o.BackgroundImageLayoutChanged += OnBackgroundImageLayoutChanged;
            o.DataSourceChanged += OnDataSourceChanged;
            o.DisplayMemberChanged += OnDisplayMemberChanged;
            o.DoubleClick += OnDoubleClick;
            o.DrawItem += OnDrawItem;
            o.DropDown += OnDropDown;
            o.DropDownClosed += OnDropDownClosed;
            o.DropDownStyleChanged += OnDropDownStyleChanged;
            o.Format += OnFormat;
            o.FormatInfoChanged += OnFormatInfoChanged;
            o.FormatStringChanged += OnFormatStringChanged;
            o.FormattingEnabledChanged += OnFormattingEnabledChanged;
            o.MeasureItem += OnMeasureItem;
            o.PaddingChanged += OnPaddingChanged;
            o.Paint += OnPaint;
            o.ReadOnlyChanged += OnReadOnlyChanged;
            o.Scroll += OnScroll;
            o.SelectedIndexChanged += OnSelectedIndexChanged;
            o.SelectedValueChanged += OnSelectedValueChanged;
            o.SelectionChangeCommitted += OnSelectionChangeCommitted;
            o.TextUpdate += OnTextUpdate;
            o.ValueMemberChanged += OnValueMemberChanged;
        }

        protected override void HookDownEvents(VBComboBox o) {
            o.BackgroundImageChanged -= OnBackgroundImageChanged;
            o.BackgroundImageLayoutChanged -= OnBackgroundImageLayoutChanged;
            o.DataSourceChanged -= OnDataSourceChanged;
            o.DisplayMemberChanged -= OnDisplayMemberChanged;
            o.DoubleClick -= OnDoubleClick;
            o.DrawItem -= OnDrawItem;
            o.DropDown -= OnDropDown;
            o.DropDownClosed -= OnDropDownClosed;
            o.DropDownStyleChanged -= OnDropDownStyleChanged;
            o.Format -= OnFormat;
            o.FormatInfoChanged -= OnFormatInfoChanged;
            o.FormatStringChanged -= OnFormatStringChanged;
            o.FormattingEnabledChanged -= OnFormattingEnabledChanged;
            o.MeasureItem -= OnMeasureItem;
            o.PaddingChanged -= OnPaddingChanged;
            o.Paint -= OnPaint;
            o.ReadOnlyChanged -= OnReadOnlyChanged;
            o.Scroll -= OnScroll;
            o.SelectedIndexChanged -= OnSelectedIndexChanged;
            o.SelectedValueChanged -= OnSelectedValueChanged;
            o.SelectionChangeCommitted -= OnSelectionChangeCommitted;
            o.TextUpdate -= OnTextUpdate;
            o.ValueMemberChanged -= OnValueMemberChanged;
        }

        private EventHandler OnBackgroundImageChanged => new EventHandler((s, e) => BackgroundImageChanged?.Invoke(s, e));
        private EventHandler OnBackgroundImageLayoutChanged => new EventHandler((s, e) => BackgroundImageLayoutChanged?.Invoke(s, e));
        private EventHandler OnDataSourceChanged => new EventHandler((s, e) => DataSourceChanged?.Invoke(s, e));
        private EventHandler OnDisplayMemberChanged => new EventHandler((s, e) => DisplayMemberChanged?.Invoke(s, e));
        private EventHandler OnDoubleClick => new EventHandler((s, e) => DoubleClick?.Invoke(s, e));
        private DrawItemEventHandler OnDrawItem => new DrawItemEventHandler((s, e) => DrawItem?.Invoke(s, e));
        private EventHandler OnDropDown => new EventHandler((s, e) => DropDown?.Invoke(s, e));
        private EventHandler OnDropDownClosed => new EventHandler((s, e) => DropDownClosed?.Invoke(s, e));
        private EventHandler OnDropDownStyleChanged => new EventHandler((s, e) => DropDownStyleChanged?.Invoke(s, e));
        private ListControlConvertEventHandler OnFormat => new ListControlConvertEventHandler((s, e) => Format?.Invoke(s, e));
        private EventHandler OnFormatInfoChanged => new EventHandler((s, e) => FormatInfoChanged?.Invoke(s, e));
        private EventHandler OnFormatStringChanged => new EventHandler((s, e) => FormatStringChanged?.Invoke(s, e));
        private EventHandler OnFormattingEnabledChanged => new EventHandler((s, e) => FormattingEnabledChanged?.Invoke(s, e));
        private MeasureItemEventHandler OnMeasureItem => new MeasureItemEventHandler((s, e) => MeasureItem?.Invoke(s, e));
        private EventHandler OnPaddingChanged => new EventHandler((s, e) => PaddingChanged?.Invoke(s, e));
        private PaintEventHandler OnPaint => new PaintEventHandler((s, e) => Paint?.Invoke(s, e));
        private EventHandler OnReadOnlyChanged => new EventHandler((s, e) => ReadOnlyChanged?.Invoke(s, e));
        private EventHandler OnScroll => new EventHandler((s, e) => Scroll?.Invoke(s, e));
        private EventHandler OnSelectedIndexChanged => new EventHandler((s, e) => SelectedIndexChanged?.Invoke(s, e));
        private EventHandler OnSelectedValueChanged => new EventHandler((s, e) => SelectedValueChanged?.Invoke(s, e));
        private EventHandler OnSelectionChangeCommitted => new EventHandler((s, e) => SelectionChangeCommitted?.Invoke(s, e));
        private EventHandler OnTextUpdate => new EventHandler((s, e) => TextUpdate?.Invoke(s, e));
        private EventHandler OnValueMemberChanged => new EventHandler((s, e) => ValueMemberChanged?.Invoke(s, e));

        public new event EventHandler BackgroundImageChanged;
        public new event EventHandler BackgroundImageLayoutChanged;
        public event EventHandler DataSourceChanged;
        public event EventHandler DisplayMemberChanged;
        public new event EventHandler DoubleClick;
        public event DrawItemEventHandler DrawItem;
        public event EventHandler DropDown;
        public event EventHandler DropDownClosed;
        public event EventHandler DropDownStyleChanged;
        public event ListControlConvertEventHandler Format;
        public event EventHandler FormatInfoChanged;
        public event EventHandler FormatStringChanged;
        public event EventHandler FormattingEnabledChanged;
        public event MeasureItemEventHandler MeasureItem;
        public new event EventHandler PaddingChanged;
        public new event PaintEventHandler Paint;
        public event EventHandler ReadOnlyChanged;
        public event EventHandler Scroll;
        public event EventHandler SelectedIndexChanged;
        public event EventHandler SelectedValueChanged;
        public event EventHandler SelectionChangeCommitted;
        public event EventHandler TextUpdate;
        public event EventHandler ValueMemberChanged;
    }
}
