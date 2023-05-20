namespace VBCompatible.ControlArray
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [ProvideProperty("Index", typeof(VBCommandButton))]
    public class VBCommandButtonArray : VBControllArray<VBCommandButton>
    {

        public VBCommandButtonArray() { }

        public VBCommandButtonArray(IContainer Container) : base(Container) { }

        protected override void HookUpEvents(VBCommandButton o) {
            o.AutoSizeChanged += OnAutoSizeChanged;
            o.DoubleClick += OnDoubleClick;
            o.ImeModeChanged += OnImeModeChanged;
            o.MouseDoubleClick += OnMouseDoubleClick;
        }

        protected override void HookDownEvents(VBCommandButton o) {
            o.AutoSizeChanged -= OnAutoSizeChanged;
            o.DoubleClick -= OnDoubleClick;
            o.ImeModeChanged -= OnImeModeChanged;
            o.MouseDoubleClick -= OnMouseDoubleClick;
        }

        private EventHandler OnAutoSizeChanged => new EventHandler((s, e) => AutoSizeChanged?.Invoke(s, e));
        private EventHandler OnDoubleClick => new EventHandler((s, e) => DoubleClick?.Invoke(s, e));
        private EventHandler OnImeModeChanged => new EventHandler((s, e) => ImeModeChanged?.Invoke(s, e));
        private MouseEventHandler OnMouseDoubleClick => new MouseEventHandler((s, e) => MouseDoubleClick?.Invoke(s, e));

        public new event EventHandler AutoSizeChanged;
        public new event EventHandler DoubleClick;
        public new event EventHandler ImeModeChanged;
        public new event MouseEventHandler MouseDoubleClick;
    }
}
