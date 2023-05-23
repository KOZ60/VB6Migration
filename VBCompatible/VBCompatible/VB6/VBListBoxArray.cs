namespace VBCompatible.VB6
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [ProvideProperty("Index", typeof(VBListBox))]
    public class VBListBoxArray : ControlArray<VBListBox>
    {

        public VBListBoxArray() { }

        public VBListBoxArray(IContainer Container) : base(Container) { }

#pragma warning disable IDE0051
        private EventHandler OnBackgroundImageChanged => new EventHandler((s, e) => BackgroundImageChanged?.Invoke(s, e));
        private EventHandler OnBackgroundImageLayoutChanged => new EventHandler((s, e) => BackgroundImageLayoutChanged?.Invoke(s, e));
        private EventHandler OnClick => new EventHandler((s, e) => Click?.Invoke(s, e));
        private EventHandler OnDataSourceChanged => new EventHandler((s, e) => DataSourceChanged?.Invoke(s, e));
        private EventHandler OnDisplayMemberChanged => new EventHandler((s, e) => DisplayMemberChanged?.Invoke(s, e));
        private DrawItemEventHandler OnDrawItem => new DrawItemEventHandler((s, e) => DrawItem?.Invoke(s, e));
        private ListControlConvertEventHandler OnFormat => new ListControlConvertEventHandler((s, e) => Format?.Invoke(s, e));
        private EventHandler OnFormatInfoChanged => new EventHandler((s, e) => FormatInfoChanged?.Invoke(s, e));
        private EventHandler OnFormatStringChanged => new EventHandler((s, e) => FormatStringChanged?.Invoke(s, e));
        private EventHandler OnFormattingEnabledChanged => new EventHandler((s, e) => FormattingEnabledChanged?.Invoke(s, e));
        private MeasureItemEventHandler OnMeasureItem => new MeasureItemEventHandler((s, e) => MeasureItem?.Invoke(s, e));
        private MouseEventHandler OnMouseClick => new MouseEventHandler((s, e) => MouseClick?.Invoke(s, e));
        private EventHandler OnPaddingChanged => new EventHandler((s, e) => PaddingChanged?.Invoke(s, e));
        private PaintEventHandler OnPaint => new PaintEventHandler((s, e) => Paint?.Invoke(s, e));
        private EventHandler OnScroll => new EventHandler((s, e) => Scroll?.Invoke(s, e));
        private EventHandler OnSelectedIndexChanged => new EventHandler((s, e) => SelectedIndexChanged?.Invoke(s, e));
        private EventHandler OnSelectedValueChanged => new EventHandler((s, e) => SelectedValueChanged?.Invoke(s, e));
        private EventHandler OnTextChanged => new EventHandler((s, e) => TextChanged?.Invoke(s, e));
        private EventHandler OnValueMemberChanged => new EventHandler((s, e) => ValueMemberChanged?.Invoke(s, e));
#pragma warning restore IDE0051 

        public new event EventHandler BackgroundImageChanged;
        public new event EventHandler BackgroundImageLayoutChanged;
        public new event EventHandler Click;
        public event EventHandler DataSourceChanged;
        public event EventHandler DisplayMemberChanged;
        public event DrawItemEventHandler DrawItem;
        public event ListControlConvertEventHandler Format;
        public event EventHandler FormatInfoChanged;
        public event EventHandler FormatStringChanged;
        public event EventHandler FormattingEnabledChanged;
        public event MeasureItemEventHandler MeasureItem;
        public new event MouseEventHandler MouseClick;
        public new event EventHandler PaddingChanged;
        public new event PaintEventHandler Paint;
        public event EventHandler Scroll;
        public event EventHandler SelectedIndexChanged;
        public event EventHandler SelectedValueChanged;
        public new event EventHandler TextChanged;
        public event EventHandler ValueMemberChanged;
    }
}