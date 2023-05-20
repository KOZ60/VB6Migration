﻿namespace VBCompatible.ControlArray
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [ProvideProperty("Index", typeof(VBTextBox))]
    public class VBTextBoxArray : VBControllArray<VBTextBox>
    {

        public VBTextBoxArray() { }

        public VBTextBoxArray(IContainer Container) : base(Container) { }

        protected override void HookUpEvents(VBTextBox o) {
            o.AcceptsTabChanged += OnAcceptsTabChanged;
            o.AutoSizeChanged += OnAutoSizeChanged;
            o.BackgroundImageChanged += OnBackgroundImageChanged;
            o.BackgroundImageLayoutChanged += OnBackgroundImageLayoutChanged;
            o.BorderStyleChanged += OnBorderStyleChanged;
            o.Click += OnClick;
            o.HideSelectionChanged += OnHideSelectionChanged;
            o.ModifiedChanged += OnModifiedChanged;
            o.MouseClick += OnMouseClick;
            o.MultilineChanged += OnMultilineChanged;
            o.PaddingChanged += OnPaddingChanged;
            o.Paint += OnPaint;
            o.ReadOnlyChanged += OnReadOnlyChanged;
            o.TextAlignChanged += OnTextAlignChanged;
        }

        protected override void HookDownEvents(VBTextBox o) {
            o.AcceptsTabChanged -= OnAcceptsTabChanged;
            o.AutoSizeChanged -= OnAutoSizeChanged;
            o.BackgroundImageChanged -= OnBackgroundImageChanged;
            o.BackgroundImageLayoutChanged -= OnBackgroundImageLayoutChanged;
            o.BorderStyleChanged -= OnBorderStyleChanged;
            o.Click -= OnClick;
            o.HideSelectionChanged -= OnHideSelectionChanged;
            o.ModifiedChanged -= OnModifiedChanged;
            o.MouseClick -= OnMouseClick;
            o.MultilineChanged -= OnMultilineChanged;
            o.PaddingChanged -= OnPaddingChanged;
            o.Paint -= OnPaint;
            o.ReadOnlyChanged -= OnReadOnlyChanged;
            o.TextAlignChanged -= OnTextAlignChanged;
        }

        private EventHandler OnAcceptsTabChanged => new EventHandler((s, e) => AcceptsTabChanged?.Invoke(s, e));
        private EventHandler OnAutoSizeChanged => new EventHandler((s, e) => AutoSizeChanged?.Invoke(s, e));
        private EventHandler OnBackgroundImageChanged => new EventHandler((s, e) => BackgroundImageChanged?.Invoke(s, e));
        private EventHandler OnBackgroundImageLayoutChanged => new EventHandler((s, e) => BackgroundImageLayoutChanged?.Invoke(s, e));
        private EventHandler OnBorderStyleChanged => new EventHandler((s, e) => BorderStyleChanged?.Invoke(s, e));
        private EventHandler OnClick => new EventHandler((s, e) => Click?.Invoke(s, e));
        private EventHandler OnHideSelectionChanged => new EventHandler((s, e) => HideSelectionChanged?.Invoke(s, e));
        private EventHandler OnModifiedChanged => new EventHandler((s, e) => ModifiedChanged?.Invoke(s, e));
        private MouseEventHandler OnMouseClick => new MouseEventHandler((s, e) => MouseClick?.Invoke(s, e));
        private EventHandler OnMultilineChanged => new EventHandler((s, e) => MultilineChanged?.Invoke(s, e));
        private EventHandler OnPaddingChanged => new EventHandler((s, e) => PaddingChanged?.Invoke(s, e));
        private PaintEventHandler OnPaint => new PaintEventHandler((s, e) => Paint?.Invoke(s, e));
        private EventHandler OnReadOnlyChanged => new EventHandler((s, e) => ReadOnlyChanged?.Invoke(s, e));
        private EventHandler OnTextAlignChanged => new EventHandler((s, e) => TextAlignChanged?.Invoke(s, e));

        public event EventHandler AcceptsTabChanged;
        public new event EventHandler AutoSizeChanged;
        public new event EventHandler BackgroundImageChanged;
        public new event EventHandler BackgroundImageLayoutChanged;
        public event EventHandler BorderStyleChanged;
        public new event EventHandler Click;
        public event EventHandler HideSelectionChanged;
        public event EventHandler ModifiedChanged;
        public new event MouseEventHandler MouseClick;
        public event EventHandler MultilineChanged;
        public new event EventHandler PaddingChanged;
        public new event PaintEventHandler Paint;
        public event EventHandler ReadOnlyChanged;
        public event EventHandler TextAlignChanged;
    }
}
