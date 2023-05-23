namespace VBCompatible.VB6
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [ProvideProperty("Index", typeof(VBCommandButton))]
    public class VBCommandButtonArray : ControlArray<VBCommandButton>
    {

        public VBCommandButtonArray() { }

        public VBCommandButtonArray(IContainer Container) : base(Container) { }

#pragma warning disable IDE0051
        private EventHandler OnAutoSizeChanged => new EventHandler((s, e) => AutoSizeChanged?.Invoke(s, e));
        private EventHandler OnDoubleClick => new EventHandler((s, e) => DoubleClick?.Invoke(s, e));
        private EventHandler OnImeModeChanged => new EventHandler((s, e) => ImeModeChanged?.Invoke(s, e));
        private MouseEventHandler OnMouseDoubleClick => new MouseEventHandler((s, e) => MouseDoubleClick?.Invoke(s, e));
#pragma warning restore IDE0051 

        public new event EventHandler AutoSizeChanged;
        public new event EventHandler DoubleClick;
        public new event EventHandler ImeModeChanged;
        public new event MouseEventHandler MouseDoubleClick;
    }
}