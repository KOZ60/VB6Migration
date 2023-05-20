namespace VBCompatible.ControlArray
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [ProvideProperty("Index", typeof(VBLabel))]
    public class VBLabelArray : VBControllArray<VBLabel>
    {

        public VBLabelArray() { }

        public VBLabelArray(IContainer Container) : base(Container) { }

        protected override void HookUpEvents(VBLabel o) {
            o.AutoSizeChanged += OnAutoSizeChanged;
            o.BackgroundImageChanged += OnBackgroundImageChanged;
            o.BackgroundImageLayoutChanged += OnBackgroundImageLayoutChanged;
            o.ImeModeChanged += OnImeModeChanged;
            o.KeyDown += OnKeyDown;
            o.KeyPress += OnKeyPress;
            o.KeyUp += OnKeyUp;
            o.TabStopChanged += OnTabStopChanged;
            o.TextAlignChanged += OnTextAlignChanged;
        }

        protected override void HookDownEvents(VBLabel o) {
            o.AutoSizeChanged -= OnAutoSizeChanged;
            o.BackgroundImageChanged -= OnBackgroundImageChanged;
            o.BackgroundImageLayoutChanged -= OnBackgroundImageLayoutChanged;
            o.ImeModeChanged -= OnImeModeChanged;
            o.KeyDown -= OnKeyDown;
            o.KeyPress -= OnKeyPress;
            o.KeyUp -= OnKeyUp;
            o.TabStopChanged -= OnTabStopChanged;
            o.TextAlignChanged -= OnTextAlignChanged;
        }

        private EventHandler OnAutoSizeChanged => new EventHandler((s, e) => AutoSizeChanged?.Invoke(s, e));
        private EventHandler OnBackgroundImageChanged => new EventHandler((s, e) => BackgroundImageChanged?.Invoke(s, e));
        private EventHandler OnBackgroundImageLayoutChanged => new EventHandler((s, e) => BackgroundImageLayoutChanged?.Invoke(s, e));
        private EventHandler OnImeModeChanged => new EventHandler((s, e) => ImeModeChanged?.Invoke(s, e));
        private KeyEventHandler OnKeyDown => new KeyEventHandler((s, e) => KeyDown?.Invoke(s, e));
        private KeyPressEventHandler OnKeyPress => new KeyPressEventHandler((s, e) => KeyPress?.Invoke(s, e));
        private KeyEventHandler OnKeyUp => new KeyEventHandler((s, e) => KeyUp?.Invoke(s, e));
        private EventHandler OnTabStopChanged => new EventHandler((s, e) => TabStopChanged?.Invoke(s, e));
        private EventHandler OnTextAlignChanged => new EventHandler((s, e) => TextAlignChanged?.Invoke(s, e));

        public new event EventHandler AutoSizeChanged;
        public new event EventHandler BackgroundImageChanged;
        public new event EventHandler BackgroundImageLayoutChanged;
        public new event EventHandler ImeModeChanged;
        public new event KeyEventHandler KeyDown;
        public new event KeyPressEventHandler KeyPress;
        public new event KeyEventHandler KeyUp;
        public new event EventHandler TabStopChanged;
        public event EventHandler TextAlignChanged;
    }
}
