namespace VBCompatible.VB6
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [ProvideProperty("Index", typeof(VBSSTab))]
    public class VBSSTabArray : ControlArray<VBSSTab>
    {

        public VBSSTabArray() { }

        public VBSSTabArray(IContainer Container) : base(Container) { }

#pragma warning disable IDE0051
        private EventHandler OnBackColorChanged => new EventHandler((s, e) => BackColorChanged?.Invoke(s, e));
        private EventHandler OnBackgroundImageChanged => new EventHandler((s, e) => BackgroundImageChanged?.Invoke(s, e));
        private EventHandler OnBackgroundImageLayoutChanged => new EventHandler((s, e) => BackgroundImageLayoutChanged?.Invoke(s, e));
        private TabControlEventHandler OnDeselected => new TabControlEventHandler((s, e) => Deselected?.Invoke(s, e));
        private TabControlCancelEventHandler OnDeselecting => new TabControlCancelEventHandler((s, e) => Deselecting?.Invoke(s, e));
        private DrawItemEventHandler OnDrawItem => new DrawItemEventHandler((s, e) => DrawItem?.Invoke(s, e));
        private EventHandler OnForeColorChanged => new EventHandler((s, e) => ForeColorChanged?.Invoke(s, e));
        private PaintEventHandler OnPaint => new PaintEventHandler((s, e) => Paint?.Invoke(s, e));
        private EventHandler OnRightToLeftLayoutChanged => new EventHandler((s, e) => RightToLeftLayoutChanged?.Invoke(s, e));
        private TabControlEventHandler OnSelected => new TabControlEventHandler((s, e) => Selected?.Invoke(s, e));
        private EventHandler OnSelectedIndexChanged => new EventHandler((s, e) => SelectedIndexChanged?.Invoke(s, e));
        private TabControlCancelEventHandler OnSelecting => new TabControlCancelEventHandler((s, e) => Selecting?.Invoke(s, e));
        private EventHandler OnTextChanged => new EventHandler((s, e) => TextChanged?.Invoke(s, e));
        private EventHandler OnUseMnemonicChanged => new EventHandler((s, e) => UseMnemonicChanged?.Invoke(s, e));
#pragma warning restore IDE0051 

        public new event EventHandler BackColorChanged;
        public new event EventHandler BackgroundImageChanged;
        public new event EventHandler BackgroundImageLayoutChanged;
        public event TabControlEventHandler Deselected;
        public event TabControlCancelEventHandler Deselecting;
        public event DrawItemEventHandler DrawItem;
        public new event EventHandler ForeColorChanged;
        public new event PaintEventHandler Paint;
        public event EventHandler RightToLeftLayoutChanged;
        public event TabControlEventHandler Selected;
        public event EventHandler SelectedIndexChanged;
        public event TabControlCancelEventHandler Selecting;
        public new event EventHandler TextChanged;
        public event EventHandler UseMnemonicChanged;
    }
}