namespace VBCompatible.ControlArray
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [ProvideProperty("Index", typeof(VBPictureBox))]
    public class VBPictureBoxArray : VBControllArray<VBPictureBox>
    {

        public VBPictureBoxArray() { }

        public VBPictureBoxArray(IContainer Container) : base(Container) { }

        protected override void HookUpEvents(VBPictureBox o) {
            o.CausesValidationChanged += OnCausesValidationChanged;
            o.Enter += OnEnter;
            o.FontChanged += OnFontChanged;
            o.ForeColorChanged += OnForeColorChanged;
            o.ImeModeChanged += OnImeModeChanged;
            o.KeyDown += OnKeyDown;
            o.KeyPress += OnKeyPress;
            o.KeyUp += OnKeyUp;
            o.Leave += OnLeave;
            o.LoadCompleted += OnLoadCompleted;
            o.LoadProgressChanged += OnLoadProgressChanged;
            o.RightToLeftChanged += OnRightToLeftChanged;
            o.SizeModeChanged += OnSizeModeChanged;
            o.TabIndexChanged += OnTabIndexChanged;
            o.TabStopChanged += OnTabStopChanged;
            o.TextChanged += OnTextChanged;
        }

        protected override void HookDownEvents(VBPictureBox o) {
            o.CausesValidationChanged -= OnCausesValidationChanged;
            o.Enter -= OnEnter;
            o.FontChanged -= OnFontChanged;
            o.ForeColorChanged -= OnForeColorChanged;
            o.ImeModeChanged -= OnImeModeChanged;
            o.KeyDown -= OnKeyDown;
            o.KeyPress -= OnKeyPress;
            o.KeyUp -= OnKeyUp;
            o.Leave -= OnLeave;
            o.LoadCompleted -= OnLoadCompleted;
            o.LoadProgressChanged -= OnLoadProgressChanged;
            o.RightToLeftChanged -= OnRightToLeftChanged;
            o.SizeModeChanged -= OnSizeModeChanged;
            o.TabIndexChanged -= OnTabIndexChanged;
            o.TabStopChanged -= OnTabStopChanged;
            o.TextChanged -= OnTextChanged;
        }

        private EventHandler OnCausesValidationChanged => new EventHandler((s, e) => CausesValidationChanged?.Invoke(s, e));
        private EventHandler OnEnter => new EventHandler((s, e) => Enter?.Invoke(s, e));
        private EventHandler OnFontChanged => new EventHandler((s, e) => FontChanged?.Invoke(s, e));
        private EventHandler OnForeColorChanged => new EventHandler((s, e) => ForeColorChanged?.Invoke(s, e));
        private EventHandler OnImeModeChanged => new EventHandler((s, e) => ImeModeChanged?.Invoke(s, e));
        private KeyEventHandler OnKeyDown => new KeyEventHandler((s, e) => KeyDown?.Invoke(s, e));
        private KeyPressEventHandler OnKeyPress => new KeyPressEventHandler((s, e) => KeyPress?.Invoke(s, e));
        private KeyEventHandler OnKeyUp => new KeyEventHandler((s, e) => KeyUp?.Invoke(s, e));
        private EventHandler OnLeave => new EventHandler((s, e) => Leave?.Invoke(s, e));
        private AsyncCompletedEventHandler OnLoadCompleted => new AsyncCompletedEventHandler((s, e) => LoadCompleted?.Invoke(s, e));
        private ProgressChangedEventHandler OnLoadProgressChanged => new ProgressChangedEventHandler((s, e) => LoadProgressChanged?.Invoke(s, e));
        private EventHandler OnRightToLeftChanged => new EventHandler((s, e) => RightToLeftChanged?.Invoke(s, e));
        private EventHandler OnSizeModeChanged => new EventHandler((s, e) => SizeModeChanged?.Invoke(s, e));
        private EventHandler OnTabIndexChanged => new EventHandler((s, e) => TabIndexChanged?.Invoke(s, e));
        private EventHandler OnTabStopChanged => new EventHandler((s, e) => TabStopChanged?.Invoke(s, e));
        private EventHandler OnTextChanged => new EventHandler((s, e) => TextChanged?.Invoke(s, e));

        public new event EventHandler CausesValidationChanged;
        public new event EventHandler Enter;
        public new event EventHandler FontChanged;
        public new event EventHandler ForeColorChanged;
        public new event EventHandler ImeModeChanged;
        public new event KeyEventHandler KeyDown;
        public new event KeyPressEventHandler KeyPress;
        public new event KeyEventHandler KeyUp;
        public new event EventHandler Leave;
        public event AsyncCompletedEventHandler LoadCompleted;
        public event ProgressChangedEventHandler LoadProgressChanged;
        public new event EventHandler RightToLeftChanged;
        public event EventHandler SizeModeChanged;
        public new event EventHandler TabIndexChanged;
        public new event EventHandler TabStopChanged;
        public new event EventHandler TextChanged;
    }
}
