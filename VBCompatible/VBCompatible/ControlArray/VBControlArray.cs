namespace VBCompatible.ControlArray
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows.Forms;

    /// <summary>
    /// コントロール配列のベースとなるクラス
    /// </summary>
    /// <typeparam name="T">配列の要素となるコントロールの型。</typeparam>
    /// <remarks>
    /// 継承したクラスで拡張プロパティの定義を行ってください。
    /// 例)
    /// [ProvideProperty("Index", typeof(TextBox))]
    /// </remarks>
    [DesignerCategory("Code")]
    public abstract class VBControllArray<T> : Component, ISupportInitialize,
                            IExtenderProvider, IEnumerable<T>
                            where T : Control {

        private readonly Dictionary<T, int> _Indices = new Dictionary<T, int>();
        private readonly Dictionary<int, T> _Controls = new Dictionary<int, T>();
        private IContainer components;
        private Form _Form;
        private Type _FormType;
        private bool _ToolTipScaned;
        private ToolTip _ToolTip;
        private const string _ToolTip1Name = "ToolTip1";
        private const BindingFlags _BindingFlags =
                                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        private readonly PropertyDescriptorCollection properties =
                                TypeDescriptor.GetProperties(typeof(T));

        protected VBControllArray() { }

        protected VBControllArray(IContainer Container) {
            components = Container;
            components.Add(this);
        }

        public T this[int Index] {
            get {
                return _Controls[Index];
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual bool CanExtend(object extendee) {
            if (extendee != null) {
                return extendee.GetType().Equals(typeof(T));
            } else {
                return false;
            }
        }

        public int GetIndex(object o) {
            var target = o as T;
            if (target != null) {
                if (_Indices.TryGetValue(target, out int index)) {
                    return index;
                }
            }
            return -1;
        }

        public void SetIndex(object o, int Index) {
            if (!CanExtend(o)) {
                throw new ArgumentException("型が違います。");
            }
            if (_Controls.TryGetValue(Index, out T instance)) {
                if (!ReferenceEquals(o, instance)) {
                    throw new ArgumentException("同じインデックスが存在しています。");
                }
            }
            ResetIndex(o);
            var target = (T)o;
            _Indices[target] = Index;
            _Controls[Index] = target;
            HookUpEventsOfControl(target);
            HookUpEvents(target);
        }

        protected abstract void HookUpEvents(T o);
        protected abstract void HookDownEvents(T o);

        public void ResetIndex(object o) {
            var target = o as T;
            if (target != null) {
                if (_Indices.TryGetValue(target, out int index)) {
                    _Indices.Remove(target);
                    _Controls.Remove(index);
                    HookDownEventsOfControl(target);
                    HookDownEvents(target);
                }
            }
        }

        public bool ShouldSerializeIndex(object o) {
            if (o is T target) {
                return _Indices.ContainsKey(target);
            }
            return false;
        }

        public int Count {
            get {
                return _Controls.Count;
            }
        }

        public int LBound {
            get {
                if (_Controls.Count == 0) {
                    return 0;
                }
                int minValue = int.MaxValue;
                foreach (var kp in _Controls) {
                    if (kp.Key < minValue) {
                        minValue = kp.Key;
                    }
                }
                return minValue;
            }
        }

        public int UBound {
            get {
                if (_Controls.Count == 0) {
                    return -1;
                }
                int maxValue = -1;
                foreach (var kp in _Controls) {
                    if (kp.Key > maxValue) {
                        maxValue = kp.Key;
                    }
                }
                return maxValue;
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            foreach (var kp in _Controls) {
                yield return kp.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            foreach (var kp in _Controls) {
                yield return kp.Value;
            }
        }

        void ISupportInitialize.BeginInit() { }
        void ISupportInitialize.EndInit() { }

        protected override void Dispose(bool disposing) {
            components = null;
            base.Dispose(disposing);
        }

        public T Load(int Index) {
            if (Index < 0 || Count == 0 || _Controls.ContainsKey(Index)) {
                throw new IndexOutOfRangeException();
            }
            var clone = CloneControl();
            SetIndex(clone, Index);
            return clone;
        }

        public void Unload(int Index) {
            if (Index < 0 || !_Controls.TryGetValue(Index, out T ctl)) {
                throw new IndexOutOfRangeException();
            }
            ResetIndex(ctl);
            ctl.Parent.Controls.Remove(ctl);
        }

        private T CloneControl() {
            T lowest = _Controls[LBound];
            T ctl = (T)Activator.CreateInstance(typeof(T));
            foreach (PropertyDescriptor p in properties) {
                if (IsSerialized(lowest, p)) {
                    p.SetValue(ctl, p.GetValue(lowest));
                }
            }
            if (ctl is RadioButton radioButton) {
                radioButton.Checked = false;
            }
            // VB6 から移植したフォームは ToolTip1 を持っているので
            // 設定された caption もコピー
            if (ToolTip1 != null) {
                var caption = ToolTip1.GetToolTip(lowest);
                if (!string.IsNullOrEmpty(caption)) {
                    ToolTip1.SetToolTip(ctl, caption);
                }
            }
            lowest.Parent.Controls.Add(ctl);
            return ctl;
        }

        private ToolTip ToolTip1 {
            get {
                if (_ToolTipScaned) {
                    return _ToolTip;
                }
                var lowest = _Controls[LBound];
                if (_Form == null) {
                    _Form = lowest.FindForm();
                    _FormType = _Form.GetType();
                }
                var pi = _FormType.GetProperty(_ToolTip1Name, _BindingFlags);
                if (pi != null) {
                    _ToolTip = pi.GetValue(_Form, null) as ToolTip;
                }
                if (_ToolTip == null) {
                    var fi = _FormType.GetField(_ToolTip1Name, _BindingFlags);
                    if (fi != null) {
                        _ToolTip = fi.GetValue(_Form) as ToolTip;
                    }
                }
                _ToolTipScaned = true;
                return _ToolTip;
            }
        }

        protected virtual bool IsSerialized(T ctl, PropertyDescriptor p) {
            if (p.IsReadOnly) {
                return false;
            }
            if (p.SerializationVisibility != DesignerSerializationVisibility.Visible) {
                return false;
            }
            if (!p.ShouldSerializeValue(ctl)) {
                return false;
            }
            switch (p.Name) {
                case "Visible":
                case "TabIndex":
                case "Index":
                case "MdiList":
                    return false;
            }
            return true;
        }

        private void HookDownEventsOfControl(Control o) {
            o.AutoSizeChanged -= OnAutoSizeChanged;
            o.BackColorChanged -= OnBackColorChanged;
            o.BackgroundImageChanged -= OnBackgroundImageChanged;
            o.BackgroundImageLayoutChanged -= OnBackgroundImageLayoutChanged;
            o.BindingContextChanged -= OnBindingContextChanged;
            o.CausesValidationChanged -= OnCausesValidationChanged;
            o.ChangeUICues -= OnChangeUICues;
            o.Click -= OnClick;
            o.ClientSizeChanged -= OnClientSizeChanged;
            o.ContextMenuChanged -= OnContextMenuChanged;
            o.ContextMenuStripChanged -= OnContextMenuStripChanged;
            o.ControlAdded -= OnControlAdded;
            o.ControlRemoved -= OnControlRemoved;
            o.CursorChanged -= OnCursorChanged;
            o.Disposed -= OnDisposedEvent;
            o.DockChanged -= OnDockChanged;
            o.DoubleClick -= OnDoubleClick;
#if NET48
            o.DpiChangedAfterParent -= OnDpiChangedAfterParent;
            o.DpiChangedBeforeParent -= OnDpiChangedBeforeParent;
#endif
            o.DragDrop -= OnDragDrop;
            o.DragEnter -= OnDragEnter;
            o.DragLeave -= OnDragLeave;
            o.DragOver -= OnDragOver;
            o.EnabledChanged -= OnEnabledChanged;
            o.Enter -= OnEnter;
            o.FontChanged -= OnFontChanged;
            o.ForeColorChanged -= OnForeColorChanged;
            o.GiveFeedback -= OnGiveFeedback;
            o.GotFocus -= OnGotFocus;
            o.HandleCreated -= OnHandleCreated;
            o.HandleDestroyed -= OnHandleDestroyed;
            o.HelpRequested -= OnHelpRequested;
            o.ImeModeChanged -= OnImeModeChanged;
            o.Invalidated -= OnInvalidated;
            o.KeyDown -= OnKeyDown;
            o.KeyPress -= OnKeyPress;
            o.KeyUp -= OnKeyUp;
            o.Layout -= OnLayout;
            o.Leave -= OnLeave;
            o.LocationChanged -= OnLocationChanged;
            o.LostFocus -= OnLostFocus;
            o.MarginChanged -= OnMarginChanged;
            o.MouseCaptureChanged -= OnMouseCaptureChanged;
            o.MouseClick -= OnMouseClick;
            o.MouseDoubleClick -= OnMouseDoubleClick;
            o.MouseDown -= OnMouseDown;
            o.MouseEnter -= OnMouseEnter;
            o.MouseHover -= OnMouseHover;
            o.MouseLeave -= OnMouseLeave;
            o.MouseMove -= OnMouseMove;
            o.MouseUp -= OnMouseUp;
            o.MouseWheel -= OnMouseWheel;
            o.Move -= OnMove;
            o.PaddingChanged -= OnPaddingChanged;
            o.Paint -= OnPaint;
            o.ParentChanged -= OnParentChanged;
            o.PreviewKeyDown -= OnPreviewKeyDown;
            o.QueryAccessibilityHelp -= OnQueryAccessibilityHelp;
            o.QueryContinueDrag -= OnQueryContinueDrag;
            o.RegionChanged -= OnRegionChanged;
            o.Resize -= OnResize;
            o.RightToLeftChanged -= OnRightToLeftChanged;
            o.SizeChanged -= OnSizeChanged;
            o.StyleChanged -= OnStyleChanged;
            o.SystemColorsChanged -= OnSystemColorsChanged;
            o.TabIndexChanged -= OnTabIndexChanged;
            o.TabStopChanged -= OnTabStopChanged;
            o.TextChanged -= OnTextChanged;
            o.Validated -= OnValidated;
            o.Validating -= OnValidating;
            o.VisibleChanged -= OnVisibleChanged;
        }

        private void HookUpEventsOfControl(Control o) {
            o.AutoSizeChanged += OnAutoSizeChanged;
            o.BackColorChanged += OnBackColorChanged;
            o.BackgroundImageChanged += OnBackgroundImageChanged;
            o.BackgroundImageLayoutChanged += OnBackgroundImageLayoutChanged;
            o.BindingContextChanged += OnBindingContextChanged;
            o.CausesValidationChanged += OnCausesValidationChanged;
            o.ChangeUICues += OnChangeUICues;
            o.Click += OnClick;
            o.ClientSizeChanged += OnClientSizeChanged;
            o.ContextMenuChanged += OnContextMenuChanged;
            o.ContextMenuStripChanged += OnContextMenuStripChanged;
            o.ControlAdded += OnControlAdded;
            o.ControlRemoved += OnControlRemoved;
            o.CursorChanged += OnCursorChanged;
            o.Disposed += OnDisposedEvent;
            o.DockChanged += OnDockChanged;
            o.DoubleClick += OnDoubleClick;
#if NET47_OR_GREATER
            o.DpiChangedAfterParent += OnDpiChangedAfterParent;
            o.DpiChangedBeforeParent += OnDpiChangedBeforeParent;
#endif
            o.DragDrop += OnDragDrop;
            o.DragEnter += OnDragEnter;
            o.DragLeave += OnDragLeave;
            o.DragOver += OnDragOver;
            o.EnabledChanged += OnEnabledChanged;
            o.Enter += OnEnter;
            o.FontChanged += OnFontChanged;
            o.ForeColorChanged += OnForeColorChanged;
            o.GiveFeedback += OnGiveFeedback;
            o.GotFocus += OnGotFocus;
            o.HandleCreated += OnHandleCreated;
            o.HandleDestroyed += OnHandleDestroyed;
            o.HelpRequested += OnHelpRequested;
            o.ImeModeChanged += OnImeModeChanged;
            o.Invalidated += OnInvalidated;
            o.KeyDown += OnKeyDown;
            o.KeyPress += OnKeyPress;
            o.KeyUp += OnKeyUp;
            o.Layout += OnLayout;
            o.Leave += OnLeave;
            o.LocationChanged += OnLocationChanged;
            o.LostFocus += OnLostFocus;
            o.MarginChanged += OnMarginChanged;
            o.MouseCaptureChanged += OnMouseCaptureChanged;
            o.MouseClick += OnMouseClick;
            o.MouseDoubleClick += OnMouseDoubleClick;
            o.MouseDown += OnMouseDown;
            o.MouseEnter += OnMouseEnter;
            o.MouseHover += OnMouseHover;
            o.MouseLeave += OnMouseLeave;
            o.MouseMove += OnMouseMove;
            o.MouseUp += OnMouseUp;
            o.MouseWheel += OnMouseWheel;
            o.Move += OnMove;
            o.PaddingChanged += OnPaddingChanged;
            o.Paint += OnPaint;
            o.ParentChanged += OnParentChanged;
            o.PreviewKeyDown += OnPreviewKeyDown;
            o.QueryAccessibilityHelp += OnQueryAccessibilityHelp;
            o.QueryContinueDrag += OnQueryContinueDrag;
            o.RegionChanged += OnRegionChanged;
            o.Resize += OnResize;
            o.RightToLeftChanged += OnRightToLeftChanged;
            o.SizeChanged += OnSizeChanged;
            o.StyleChanged += OnStyleChanged;
            o.SystemColorsChanged += OnSystemColorsChanged;
            o.TabIndexChanged += OnTabIndexChanged;
            o.TabStopChanged += OnTabStopChanged;
            o.TextChanged += OnTextChanged;
            o.Validated += OnValidated;
            o.Validating += OnValidating;
            o.VisibleChanged += OnVisibleChanged;
        }

        private EventHandler OnAutoSizeChanged => new EventHandler((s, e) => { AutoSizeChanged?.Invoke(s, e); });
        private EventHandler OnBackColorChanged => new EventHandler((s, e) => { BackColorChanged?.Invoke(s, e); });
        private EventHandler OnBackgroundImageChanged => new EventHandler((s, e) => { BackgroundImageChanged?.Invoke(s, e); });
        private EventHandler OnBackgroundImageLayoutChanged => new EventHandler((s, e) => { BackgroundImageLayoutChanged?.Invoke(s, e); });
        private EventHandler OnBindingContextChanged => new EventHandler((s, e) => { BindingContextChanged?.Invoke(s, e); });
        private EventHandler OnCausesValidationChanged => new EventHandler((s, e) => { CausesValidationChanged?.Invoke(s, e); });
        private UICuesEventHandler OnChangeUICues => new UICuesEventHandler((s, e) => { ChangeUICues?.Invoke(s, e); });
        private EventHandler OnClick => new EventHandler((s, e) => { Click?.Invoke(s, e); });
        private EventHandler OnClientSizeChanged => new EventHandler((s, e) => { ClientSizeChanged?.Invoke(s, e); });
        private EventHandler OnContextMenuChanged => new EventHandler((s, e) => { ContextMenuChanged?.Invoke(s, e); });
        private EventHandler OnContextMenuStripChanged => new EventHandler((s, e) => { ContextMenuStripChanged?.Invoke(s, e); });
        private ControlEventHandler OnControlAdded => new ControlEventHandler((s, e) => { ControlAdded?.Invoke(s, e); });
        private ControlEventHandler OnControlRemoved => new ControlEventHandler((s, e) => { ControlRemoved?.Invoke(s, e); });
        private EventHandler OnCursorChanged => new EventHandler((s, e) => { CursorChanged?.Invoke(s, e); });
        private EventHandler OnDisposedEvent => new EventHandler((s, e) => { DisposedEvent?.Invoke(s, e); });
        private EventHandler OnDockChanged => new EventHandler((s, e) => { DockChanged?.Invoke(s, e); });
        private EventHandler OnDoubleClick => new EventHandler((s, e) => { DoubleClick?.Invoke(s, e); });
#if NET47_OR_GREATER
        private EventHandler OnDpiChangedAfterParent => new EventHandler((s, e) => { DpiChangedAfterParent?.Invoke(s, e); });
        private EventHandler OnDpiChangedBeforeParent => new EventHandler((s, e) => { DpiChangedBeforeParent?.Invoke(s, e); });
#endif
        private DragEventHandler OnDragDrop => new DragEventHandler((s, e) => { DragDrop?.Invoke(s, e); });
        private DragEventHandler OnDragEnter => new DragEventHandler((s, e) => { DragEnter?.Invoke(s, e); });
        private EventHandler OnDragLeave => new EventHandler((s, e) => { DragLeave?.Invoke(s, e); });
        private DragEventHandler OnDragOver => new DragEventHandler((s, e) => { DragOver?.Invoke(s, e); });
        private EventHandler OnEnabledChanged => new EventHandler((s, e) => { EnabledChanged?.Invoke(s, e); });
        private EventHandler OnEnter => new EventHandler((s, e) => { Enter?.Invoke(s, e); });
        private EventHandler OnFontChanged => new EventHandler((s, e) => { FontChanged?.Invoke(s, e); });
        private EventHandler OnForeColorChanged => new EventHandler((s, e) => { ForeColorChanged?.Invoke(s, e); });
        private GiveFeedbackEventHandler OnGiveFeedback => new GiveFeedbackEventHandler((s, e) => { GiveFeedback?.Invoke(s, e); });
        private EventHandler OnGotFocus => new EventHandler((s, e) => { GotFocus?.Invoke(s, e); });
        private EventHandler OnHandleCreated => new EventHandler((s, e) => { HandleCreated?.Invoke(s, e); });
        private EventHandler OnHandleDestroyed => new EventHandler((s, e) => { HandleDestroyed?.Invoke(s, e); });
        private HelpEventHandler OnHelpRequested => new HelpEventHandler((s, e) => { HelpRequested?.Invoke(s, e); });
        private EventHandler OnImeModeChanged => new EventHandler((s, e) => { ImeModeChanged?.Invoke(s, e); });
        private InvalidateEventHandler OnInvalidated => new InvalidateEventHandler((s, e) => { Invalidated?.Invoke(s, e); });
        private KeyEventHandler OnKeyDown => new KeyEventHandler((s, e) => { KeyDown?.Invoke(s, e); });
        private KeyPressEventHandler OnKeyPress => new KeyPressEventHandler((s, e) => { KeyPress?.Invoke(s, e); });
        private KeyEventHandler OnKeyUp => new KeyEventHandler((s, e) => { KeyUp?.Invoke(s, e); });
        private LayoutEventHandler OnLayout => new LayoutEventHandler((s, e) => { Layout?.Invoke(s, e); });
        private EventHandler OnLeave => new EventHandler((s, e) => { Leave?.Invoke(s, e); });
        private EventHandler OnLocationChanged => new EventHandler((s, e) => { LocationChanged?.Invoke(s, e); });
        private EventHandler OnLostFocus => new EventHandler((s, e) => { LostFocus?.Invoke(s, e); });
        private EventHandler OnMarginChanged => new EventHandler((s, e) => { MarginChanged?.Invoke(s, e); });
        private EventHandler OnMouseCaptureChanged => new EventHandler((s, e) => { MouseCaptureChanged?.Invoke(s, e); });
        private MouseEventHandler OnMouseClick => new MouseEventHandler((s, e) => { MouseClick?.Invoke(s, e); });
        private MouseEventHandler OnMouseDoubleClick => new MouseEventHandler((s, e) => { MouseDoubleClick?.Invoke(s, e); });
        private MouseEventHandler OnMouseDown => new MouseEventHandler((s, e) => { MouseDown?.Invoke(s, e); });
        private EventHandler OnMouseEnter => new EventHandler((s, e) => { MouseEnter?.Invoke(s, e); });
        private EventHandler OnMouseHover => new EventHandler((s, e) => { MouseHover?.Invoke(s, e); });
        private EventHandler OnMouseLeave => new EventHandler((s, e) => { MouseLeave?.Invoke(s, e); });
        private MouseEventHandler OnMouseMove => new MouseEventHandler((s, e) => { MouseMove?.Invoke(s, e); });
        private MouseEventHandler OnMouseUp => new MouseEventHandler((s, e) => { MouseUp?.Invoke(s, e); });
        private MouseEventHandler OnMouseWheel => new MouseEventHandler((s, e) => { MouseWheel?.Invoke(s, e); });
        private EventHandler OnMove => new EventHandler((s, e) => { Move?.Invoke(s, e); });
        private EventHandler OnPaddingChanged => new EventHandler((s, e) => { PaddingChanged?.Invoke(s, e); });
        private PaintEventHandler OnPaint => new PaintEventHandler((s, e) => { Paint?.Invoke(s, e); });
        private EventHandler OnParentChanged => new EventHandler((s, e) => { ParentChanged?.Invoke(s, e); });
        private PreviewKeyDownEventHandler OnPreviewKeyDown => new PreviewKeyDownEventHandler((s, e) => { PreviewKeyDown?.Invoke(s, e); });
        private QueryAccessibilityHelpEventHandler OnQueryAccessibilityHelp => new QueryAccessibilityHelpEventHandler((s, e) => { QueryAccessibilityHelp?.Invoke(s, e); });
        private QueryContinueDragEventHandler OnQueryContinueDrag => new QueryContinueDragEventHandler((s, e) => { QueryContinueDrag?.Invoke(s, e); });
        private EventHandler OnRegionChanged => new EventHandler((s, e) => { RegionChanged?.Invoke(s, e); });
        private EventHandler OnResize => new EventHandler((s, e) => { Resize?.Invoke(s, e); });
        private EventHandler OnRightToLeftChanged => new EventHandler((s, e) => { RightToLeftChanged?.Invoke(s, e); });
        private EventHandler OnSizeChanged => new EventHandler((s, e) => { SizeChanged?.Invoke(s, e); });
        private EventHandler OnStyleChanged => new EventHandler((s, e) => { StyleChanged?.Invoke(s, e); });
        private EventHandler OnSystemColorsChanged => new EventHandler((s, e) => { SystemColorsChanged?.Invoke(s, e); });
        private EventHandler OnTabIndexChanged => new EventHandler((s, e) => { TabIndexChanged?.Invoke(s, e); });
        private EventHandler OnTabStopChanged => new EventHandler((s, e) => { TabStopChanged?.Invoke(s, e); });
        private EventHandler OnTextChanged => new EventHandler((s, e) => { TextChanged?.Invoke(s, e); });
        private EventHandler OnValidated => new EventHandler((s, e) => { Validated?.Invoke(s, e); });
        private CancelEventHandler OnValidating => new CancelEventHandler((s, e) => { Validating?.Invoke(s, e); });
        private EventHandler OnVisibleChanged => new EventHandler((s, e) => { VisibleChanged?.Invoke(s, e); });

        public event EventHandler AutoSizeChanged;
        public event EventHandler BackColorChanged;
        public event EventHandler BackgroundImageChanged;
        public event EventHandler BackgroundImageLayoutChanged;
        public event EventHandler BindingContextChanged;
        public event EventHandler CausesValidationChanged;
        public event UICuesEventHandler ChangeUICues;
        public event EventHandler Click;
        public event EventHandler ClientSizeChanged;
        public event EventHandler ContextMenuChanged;
        public event EventHandler ContextMenuStripChanged;
        public event ControlEventHandler ControlAdded;
        public event ControlEventHandler ControlRemoved;
        public event EventHandler CursorChanged;
        public event EventHandler DisposedEvent;
        public event EventHandler DockChanged;
        public event EventHandler DoubleClick;
#if NET47_OR_GREATER
        public event EventHandler DpiChangedAfterParent;
        public event EventHandler DpiChangedBeforeParent;
#endif
        public event DragEventHandler DragDrop;
        public event DragEventHandler DragEnter;
        public event EventHandler DragLeave;
        public event DragEventHandler DragOver;
        public event EventHandler EnabledChanged;
        public event EventHandler Enter;
        public event EventHandler FontChanged;
        public event EventHandler ForeColorChanged;
        public event GiveFeedbackEventHandler GiveFeedback;
        public event EventHandler GotFocus;
        public event EventHandler HandleCreated;
        public event EventHandler HandleDestroyed;
        public event HelpEventHandler HelpRequested;
        public event EventHandler ImeModeChanged;
        public event InvalidateEventHandler Invalidated;
        public event KeyEventHandler KeyDown;
        public event KeyPressEventHandler KeyPress;
        public event KeyEventHandler KeyUp;
        public event LayoutEventHandler Layout;
        public event EventHandler Leave;
        public event EventHandler LocationChanged;
        public event EventHandler LostFocus;
        public event EventHandler MarginChanged;
        public event EventHandler MouseCaptureChanged;
        public event MouseEventHandler MouseClick;
        public event MouseEventHandler MouseDoubleClick;
        public event MouseEventHandler MouseDown;
        public event EventHandler MouseEnter;
        public event EventHandler MouseHover;
        public event EventHandler MouseLeave;
        public event MouseEventHandler MouseMove;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseWheel;
        public event EventHandler Move;
        public event EventHandler PaddingChanged;
        public event PaintEventHandler Paint;
        public event EventHandler ParentChanged;
        public event PreviewKeyDownEventHandler PreviewKeyDown;
        public event QueryAccessibilityHelpEventHandler QueryAccessibilityHelp;
        public event QueryContinueDragEventHandler QueryContinueDrag;
        public event EventHandler RegionChanged;
        public event EventHandler Resize;
        public event EventHandler RightToLeftChanged;
        public event EventHandler SizeChanged;
        public event EventHandler StyleChanged;
        public event EventHandler SystemColorsChanged;
        public event EventHandler TabIndexChanged;
        public event EventHandler TabStopChanged;
        public event EventHandler TextChanged;
        public event EventHandler Validated;
        public event CancelEventHandler Validating;
        public event EventHandler VisibleChanged;

    }
}
