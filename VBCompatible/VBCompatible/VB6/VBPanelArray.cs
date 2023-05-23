namespace VBCompatible.VB6
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [ProvideProperty("Index", typeof(VBPanel))]
    public class VBPanelArray : ControlArray<VBPanel>
    {

        public VBPanelArray() { }

        public VBPanelArray(IContainer Container) : base(Container) { }

#pragma warning disable IDE0051
        private EventHandler OnAutoSizeChanged => new EventHandler((s, e) => AutoSizeChanged?.Invoke(s, e));
        private KeyEventHandler OnKeyDown => new KeyEventHandler((s, e) => KeyDown?.Invoke(s, e));
        private KeyPressEventHandler OnKeyPress => new KeyPressEventHandler((s, e) => KeyPress?.Invoke(s, e));
        private KeyEventHandler OnKeyUp => new KeyEventHandler((s, e) => KeyUp?.Invoke(s, e));
        private ScrollEventHandler OnScroll => new ScrollEventHandler((s, e) => Scroll?.Invoke(s, e));
        private EventHandler OnTextChanged => new EventHandler((s, e) => TextChanged?.Invoke(s, e));
#pragma warning restore IDE0051 

        public new event EventHandler AutoSizeChanged;
        public new event KeyEventHandler KeyDown;
        public new event KeyPressEventHandler KeyPress;
        public new event KeyEventHandler KeyUp;
        public event ScrollEventHandler Scroll;
        public new event EventHandler TextChanged;
    }
}