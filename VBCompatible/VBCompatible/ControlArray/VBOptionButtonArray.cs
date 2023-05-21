﻿namespace VBCompatible.ControlArray
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [ProvideProperty("Index", typeof(VBOptionButton))]
    public class VBOptionButtonArray : BaseControlArray<VBOptionButton>
    {

        public VBOptionButtonArray() { }

        public VBOptionButtonArray(IContainer Container) : base(Container) { }

        protected override void HookUpEvents(VBOptionButton o) {
            o.AppearanceChanged += OnAppearanceChanged;
            o.AutoSizeChanged += OnAutoSizeChanged;
            o.CheckedChanged += OnCheckedChanged;
            o.DoubleClick += OnDoubleClick;
            o.ImeModeChanged += OnImeModeChanged;
            o.MouseDoubleClick += OnMouseDoubleClick;
        }

        protected override void HookDownEvents(VBOptionButton o) {
            o.AppearanceChanged -= OnAppearanceChanged;
            o.AutoSizeChanged -= OnAutoSizeChanged;
            o.CheckedChanged -= OnCheckedChanged;
            o.DoubleClick -= OnDoubleClick;
            o.ImeModeChanged -= OnImeModeChanged;
            o.MouseDoubleClick -= OnMouseDoubleClick;
        }

        private EventHandler OnAppearanceChanged => new EventHandler((s, e) => AppearanceChanged?.Invoke(s, e));
        private EventHandler OnAutoSizeChanged => new EventHandler((s, e) => AutoSizeChanged?.Invoke(s, e));
        private EventHandler OnCheckedChanged => new EventHandler((s, e) => CheckedChanged?.Invoke(s, e));
        private EventHandler OnDoubleClick => new EventHandler((s, e) => DoubleClick?.Invoke(s, e));
        private EventHandler OnImeModeChanged => new EventHandler((s, e) => ImeModeChanged?.Invoke(s, e));
        private MouseEventHandler OnMouseDoubleClick => new MouseEventHandler((s, e) => MouseDoubleClick?.Invoke(s, e));

        public event EventHandler AppearanceChanged;
        public new event EventHandler AutoSizeChanged;
        public event EventHandler CheckedChanged;
        public new event EventHandler DoubleClick;
        public new event EventHandler ImeModeChanged;
        public new event MouseEventHandler MouseDoubleClick;
    }
}
