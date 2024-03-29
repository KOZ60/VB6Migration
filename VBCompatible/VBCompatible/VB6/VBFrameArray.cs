﻿namespace VBCompatible.VB6
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [ProvideProperty("Index", typeof(VBFrame))]
    public class VBFrameArray : ControlArray<VBFrame>
    {

        public VBFrameArray() { }

        public VBFrameArray(IContainer Container) : base(Container) { }

#pragma warning disable IDE0051
        private EventHandler OnAutoSizeChanged => new EventHandler((s, e) => AutoSizeChanged?.Invoke(s, e));
        private EventHandler OnClick => new EventHandler((s, e) => Click?.Invoke(s, e));
        private EventHandler OnDoubleClick => new EventHandler((s, e) => DoubleClick?.Invoke(s, e));
        private KeyEventHandler OnKeyDown => new KeyEventHandler((s, e) => KeyDown?.Invoke(s, e));
        private KeyPressEventHandler OnKeyPress => new KeyPressEventHandler((s, e) => KeyPress?.Invoke(s, e));
        private KeyEventHandler OnKeyUp => new KeyEventHandler((s, e) => KeyUp?.Invoke(s, e));
        private MouseEventHandler OnMouseClick => new MouseEventHandler((s, e) => MouseClick?.Invoke(s, e));
        private MouseEventHandler OnMouseDoubleClick => new MouseEventHandler((s, e) => MouseDoubleClick?.Invoke(s, e));
        private MouseEventHandler OnMouseDown => new MouseEventHandler((s, e) => MouseDown?.Invoke(s, e));
        private EventHandler OnMouseEnter => new EventHandler((s, e) => MouseEnter?.Invoke(s, e));
        private EventHandler OnMouseLeave => new EventHandler((s, e) => MouseLeave?.Invoke(s, e));
        private MouseEventHandler OnMouseMove => new MouseEventHandler((s, e) => MouseMove?.Invoke(s, e));
        private MouseEventHandler OnMouseUp => new MouseEventHandler((s, e) => MouseUp?.Invoke(s, e));
        private EventHandler OnTabStopChanged => new EventHandler((s, e) => TabStopChanged?.Invoke(s, e));
#pragma warning restore IDE0051 

        public new event EventHandler AutoSizeChanged;
        public new event EventHandler Click;
        public new event EventHandler DoubleClick;
        public new event KeyEventHandler KeyDown;
        public new event KeyPressEventHandler KeyPress;
        public new event KeyEventHandler KeyUp;
        public new event MouseEventHandler MouseClick;
        public new event MouseEventHandler MouseDoubleClick;
        public new event MouseEventHandler MouseDown;
        public new event EventHandler MouseEnter;
        public new event EventHandler MouseLeave;
        public new event MouseEventHandler MouseMove;
        public new event MouseEventHandler MouseUp;
        public new event EventHandler TabStopChanged;
    }
}