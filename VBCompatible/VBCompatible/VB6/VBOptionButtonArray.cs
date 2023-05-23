namespace VBCompatible.VB6
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [ProvideProperty("Index", typeof(VBOptionButton))]
    public class VBOptionButtonArray : ControlArray<VBOptionButton>
    {

        public VBOptionButtonArray() { }

        public VBOptionButtonArray(IContainer Container) : base(Container) { }

#pragma warning disable IDE0051
        private EventHandler OnAppearanceChanged => new EventHandler((s, e) => AppearanceChanged?.Invoke(s, e));
        private EventHandler OnAutoSizeChanged => new EventHandler((s, e) => AutoSizeChanged?.Invoke(s, e));
        private EventHandler OnCheckedChanged => new EventHandler((s, e) => CheckedChanged?.Invoke(s, e));
        private EventHandler OnDoubleClick => new EventHandler((s, e) => DoubleClick?.Invoke(s, e));
        private EventHandler OnImeModeChanged => new EventHandler((s, e) => ImeModeChanged?.Invoke(s, e));
        private MouseEventHandler OnMouseDoubleClick => new MouseEventHandler((s, e) => MouseDoubleClick?.Invoke(s, e));
#pragma warning restore IDE0051 

        public event EventHandler AppearanceChanged;
        public new event EventHandler AutoSizeChanged;
        public event EventHandler CheckedChanged;
        public new event EventHandler DoubleClick;
        public new event EventHandler ImeModeChanged;
        public new event MouseEventHandler MouseDoubleClick;
    }
}