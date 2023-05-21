namespace VBCompatible.ControlArray
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [ProvideProperty("Index", typeof(VBPanel))]
    public class VBPanelArray : BaseControlArray<VBPanel>
    {

        public VBPanelArray() { }

        public VBPanelArray(IContainer Container) : base(Container) { }

        protected override void HookUpEvents(VBPanel o) {
            o.AutoSizeChanged += OnAutoSizeChanged;
            o.KeyDown += OnKeyDown;
            o.KeyPress += OnKeyPress;
            o.KeyUp += OnKeyUp;
            o.Scroll += OnScroll;
            o.TextChanged += OnTextChanged;
        }

        protected override void HookDownEvents(VBPanel o) {
            o.AutoSizeChanged -= OnAutoSizeChanged;
            o.KeyDown -= OnKeyDown;
            o.KeyPress -= OnKeyPress;
            o.KeyUp -= OnKeyUp;
            o.Scroll -= OnScroll;
            o.TextChanged -= OnTextChanged;
        }

        private EventHandler OnAutoSizeChanged => new EventHandler((s, e) => AutoSizeChanged?.Invoke(s, e));
        private KeyEventHandler OnKeyDown => new KeyEventHandler((s, e) => KeyDown?.Invoke(s, e));
        private KeyPressEventHandler OnKeyPress => new KeyPressEventHandler((s, e) => KeyPress?.Invoke(s, e));
        private KeyEventHandler OnKeyUp => new KeyEventHandler((s, e) => KeyUp?.Invoke(s, e));
        private ScrollEventHandler OnScroll => new ScrollEventHandler((s, e) => Scroll?.Invoke(s, e));
        private EventHandler OnTextChanged => new EventHandler((s, e) => TextChanged?.Invoke(s, e));

        public new event EventHandler AutoSizeChanged;
        public new event KeyEventHandler KeyDown;
        public new event KeyPressEventHandler KeyPress;
        public new event KeyEventHandler KeyUp;
        public event ScrollEventHandler Scroll;
        public new event EventHandler TextChanged;
    }
}
