namespace VBCompatible.VB6
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [ProvideProperty("Index", typeof(VBSplitContainer))]
    public class VBSplitContainerArray : ControlArray<VBSplitContainer>
    {

        public VBSplitContainerArray() { }

        public VBSplitContainerArray(IContainer Container) : base(Container) { }

#pragma warning disable IDE0051
        private EventHandler OnAutoSizeChanged => new EventHandler((s, e) => AutoSizeChanged?.Invoke(s, e));
        private EventHandler OnAutoValidateChanged => new EventHandler((s, e) => AutoValidateChanged?.Invoke(s, e));
        private EventHandler OnBackgroundImageChanged => new EventHandler((s, e) => BackgroundImageChanged?.Invoke(s, e));
        private EventHandler OnBackgroundImageLayoutChanged => new EventHandler((s, e) => BackgroundImageLayoutChanged?.Invoke(s, e));
        private ControlEventHandler OnControlAdded => new ControlEventHandler((s, e) => ControlAdded?.Invoke(s, e));
        private ControlEventHandler OnControlRemoved => new ControlEventHandler((s, e) => ControlRemoved?.Invoke(s, e));
        private EventHandler OnPaddingChanged => new EventHandler((s, e) => PaddingChanged?.Invoke(s, e));
        private ScrollEventHandler OnScroll => new ScrollEventHandler((s, e) => Scroll?.Invoke(s, e));
        private SplitterEventHandler OnSplitterMoved => new SplitterEventHandler((s, e) => SplitterMoved?.Invoke(s, e));
        private SplitterCancelEventHandler OnSplitterMoving => new SplitterCancelEventHandler((s, e) => SplitterMoving?.Invoke(s, e));
        private EventHandler OnTextChanged => new EventHandler((s, e) => TextChanged?.Invoke(s, e));
#pragma warning restore IDE0051 

        public new event EventHandler AutoSizeChanged;
        public event EventHandler AutoValidateChanged;
        public new event EventHandler BackgroundImageChanged;
        public new event EventHandler BackgroundImageLayoutChanged;
        public new event ControlEventHandler ControlAdded;
        public new event ControlEventHandler ControlRemoved;
        public new event EventHandler PaddingChanged;
        public event ScrollEventHandler Scroll;
        public event SplitterEventHandler SplitterMoved;
        public event SplitterCancelEventHandler SplitterMoving;
        public new event EventHandler TextChanged;
    }
}