﻿namespace VBCompatible.ControlArray
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
    public abstract class BaseControlArray<T> :
                            BaseControlArray, ISupportInitialize,
                            IExtenderProvider, IEnumerable<T>
                            where T : Control, new() {

        protected abstract void HookUpEvents(T o);
        protected abstract void HookDownEvents(T o);

        [ThreadStatic]
        private static List<PropertyDescriptor> _Properties;
        private static List<PropertyDescriptor> Properties {
            get {
                if (_Properties == null) {
                    _Properties = new List<PropertyDescriptor>();
                    foreach (PropertyDescriptor p in TypeDescriptor.GetProperties(typeof(T))) {
                        if (p.IsReadOnly) {
                            continue;
                        }
                        if (p.SerializationVisibility != DesignerSerializationVisibility.Visible) {
                            continue;
                        }
                        switch (p.Name) {
                            case "Visible":
                            case "TabIndex":
                            case "Index":
                            case "MdiList":
                                continue;
                        }
                        _Properties.Add(p);
                    }
                }
                return _Properties;
            }
        }

        protected IContainer components;
        protected readonly Dictionary<T, int> indices = new Dictionary<T, int>();
        protected readonly Dictionary<int, T> controls = new Dictionary<int, T>();
        protected readonly HashSet<T> controlAddedAtDesignTime = new HashSet<T>();
        protected bool fIsEndInitCalled;

        private string _Name;
        private Form _Form;
        private Type _FormType;
        private bool _ToolTipScaned;
        private ToolTip _ToolTip;
        private const string _ToolTip1Name = "ToolTip1";
        private const BindingFlags _BindingFlags =
                                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        protected BaseControlArray() { }

        protected BaseControlArray(IContainer Container) {
            components = Container;
            components.Add(this);
        }

        [Browsable(false)]
        public string Name {
            get {
                if (string.IsNullOrEmpty(_Name)) {
                    if (Site != null) {
                        _Name = Site.Name;
                    }
                    if (_Name == null) {
                        _Name = string.Empty;
                    }
                }
                return _Name;
            }
            set {
                if (string.IsNullOrEmpty(value)) {
                    _Name = null;
                } else {
                    _Name = value;
                }
            }
        }

        public T this[int Index] {
            get {
                return controls[Index];
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool CanExtend(object extendee) {
            return BaseCanExtend(extendee as T);
        }

        protected virtual bool BaseCanExtend(T o) {
            if (o == null) {
                return false;
            }
            if (!o.GetType().Equals(typeof(T))) {
                return false;
            }
            if (indices.ContainsKey(o)) {
                return true;
            }
            if (string.IsNullOrEmpty(o.Name)) {
                return false;
            }
            if (o.Name.StartsWith(Name)) {
                return true;
            }
            return false;
        }

        public int GetIndex(object o) {
            return BaseGetIndex(o as T);
        }

        protected virtual int BaseGetIndex(T o) {
            if (o != null) {
                if (indices.TryGetValue(o, out int index)) {
                    return index;
                }
            }
            return -1;
        }

        public void SetIndex(object o, int Index) {
            if (!TrueDesignMode) {
                throw new InvalidOperationException("デザイン時以外は使えません。");
            }
            BaseSetIndex(o as T, Index);
        }

        protected virtual void BaseSetIndex(T o, int Index) {
            if (o == null || !o.GetType().Equals(typeof(T))) {
                throw new ArgumentException("型が違います。");
            }
            if (Index < 0) {
                throw new IndexOutOfRangeException();
            }
            if (controls.TryGetValue(Index, out T instance)) {
                if (!ReferenceEquals(o, instance)) {
                    throw new ArgumentException("同じインデックスが存在しています。");
                }
            }
            indices[o] = Index;
            controls[Index] = o;
            if (TrueDesignMode) {
                controlAddedAtDesignTime.Add(o);
            }
            HookUpEventsOfControl(o);
            HookUpEvents(o);
        }

        public void ResetIndex(object o) {
            if (!TrueDesignMode) {
                throw new InvalidOperationException("デザイン時以外は使えません。");
            }
            BaseResetIndex(o as T);
        }

        protected virtual void BaseResetIndex(T o) {
            if (o == null) {
                throw new ArgumentException("型が違います。");
            }
            if (indices.TryGetValue(o, out int index)) {
                indices.Remove(o);
                controls.Remove(index);
                if (controlAddedAtDesignTime.Contains(o)) {
                    controlAddedAtDesignTime.Remove(o);
                }
                HookDownEventsOfControl(o);
                HookDownEvents(o);
            }
        }

        public bool ShouldSerializeIndex(object o) {
            if (o is T target) {
                return indices.ContainsKey(target);
            }
            return false;
        }

        [Browsable(false)]
        public int Count {
            get {
                return controls.Count;
            }
        }

        [Browsable(false)]
        public int LBound {
            get {
                if (controls.Count == 0) {
                    return 0;
                }
                int minValue = int.MaxValue;
                foreach (var kp in controls) {
                    if (kp.Key < minValue) {
                        minValue = kp.Key;
                    }
                }
                return minValue;
            }
        }

        [Browsable(false)]
        public int UBound {
            get {
                if (controls.Count == 0) {
                    return -1;
                }
                int maxValue = -1;
                foreach (var kp in controls) {
                    if (kp.Key > maxValue) {
                        maxValue = kp.Key;
                    }
                }
                return maxValue;
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            foreach (var kp in controls) {
                yield return kp.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            foreach (var kp in controls) {
                yield return kp.Value;
            }
        }

        void ISupportInitialize.BeginInit() { }

        void ISupportInitialize.EndInit() {
            fIsEndInitCalled = true;
        }

        protected override void Dispose(bool disposing) {
            components = null;
            base.Dispose(disposing);
        }

        public T Load(int Index) {
            if (Index < 0 || Count == 0 || controls.ContainsKey(Index)) {
                throw new IndexOutOfRangeException();
            }
            var clone = CloneControl();
            BaseSetIndex(clone, Index);
            return clone;
        }

        public void Unload(int Index) {
            if (Index < 0 || !controls.TryGetValue(Index, out T ctl)) {
                throw new IndexOutOfRangeException();
            }
            if (controlAddedAtDesignTime.Contains(ctl)) {
                throw new InvalidOperationException("デザイン時に設定したコントロールは Unload できません。");
            }
            BaseResetIndex(ctl);
            ctl.Parent.Controls.Remove(ctl);
            ctl.Dispose();
        }

        private bool TrueDesignMode {
            get {
                return DesignMode || !fIsEndInitCalled;
            }
        }

        private T CloneControl() {
            T lowest = controls[LBound];
            T ctl = new T();
            foreach (PropertyDescriptor p in Properties) {
                if (p.ShouldSerializeValue(lowest)) {
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
                var lowest = controls[LBound];
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

    }

    public class BaseControlArray : Component
    {

        private const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                                                BindingFlags.Instance | BindingFlags.DeclaredOnly;

        private readonly static List<PropertyInfo> EventProperties = new List<PropertyInfo>();

        static BaseControlArray() {
            foreach (var info in typeof(BaseControlArray).GetProperties(flags)) {
                if (info.CanRead && info.Name.StartsWith("On")) {
                    EventProperties.Add(info);
                }
            }
        }

        private Dictionary<string, Delegate> _BaseEvents;

        private Dictionary<string, Delegate> BaseEvents {
            get {
                if (_BaseEvents == null) {
                    _BaseEvents = new Dictionary<string, Delegate>();
                    foreach (var info in EventProperties) {
                        _BaseEvents.Add(info.Name.Substring(2), (Delegate)info.GetValue(this, null));
                    }
                }
                return _BaseEvents;
            }
        }

        protected void HookUpEventsOfControl(Control o) {
            foreach (var info in typeof(Control).GetEvents()) {
                if (BaseEvents.TryGetValue(info.Name, out Delegate invoker)) {
                    info.AddEventHandler(o, invoker);
                }
            }
        }

        protected void HookDownEventsOfControl(Control o) {
            foreach (var info in typeof(Control).GetEvents()) {
                if (BaseEvents.TryGetValue(info.Name, out Delegate invoker)) {
                    info.RemoveEventHandler(o, invoker);
                }
            }
        }

#pragma warning disable IDE0051

        private EventHandler OnAutoSizeChanged => new EventHandler((s, e) => AutoSizeChanged?.Invoke(s, e));
        private EventHandler OnBackColorChanged => new EventHandler((s, e) => BackColorChanged?.Invoke(s, e));
        private EventHandler OnBackgroundImageChanged => new EventHandler((s, e) => BackgroundImageChanged?.Invoke(s, e));
        private EventHandler OnBackgroundImageLayoutChanged => new EventHandler((s, e) => BackgroundImageLayoutChanged?.Invoke(s, e));
        private EventHandler OnBindingContextChanged => new EventHandler((s, e) => BindingContextChanged?.Invoke(s, e));
        private EventHandler OnCausesValidationChanged => new EventHandler((s, e) => CausesValidationChanged?.Invoke(s, e));
        private UICuesEventHandler OnChangeUICues => new UICuesEventHandler((s, e) => ChangeUICues?.Invoke(s, e));
        private EventHandler OnClick => new EventHandler((s, e) => Click?.Invoke(s, e));
        private EventHandler OnClientSizeChanged => new EventHandler((s, e) => ClientSizeChanged?.Invoke(s, e));
        private EventHandler OnContextMenuChanged => new EventHandler((s, e) => ContextMenuChanged?.Invoke(s, e));
        private EventHandler OnContextMenuStripChanged => new EventHandler((s, e) => ContextMenuStripChanged?.Invoke(s, e));
        private ControlEventHandler OnControlAdded => new ControlEventHandler((s, e) => ControlAdded?.Invoke(s, e));
        private ControlEventHandler OnControlRemoved => new ControlEventHandler((s, e) => ControlRemoved?.Invoke(s, e));
        private EventHandler OnCursorChanged => new EventHandler((s, e) => CursorChanged?.Invoke(s, e));
        private EventHandler OnDisposed => new EventHandler((s, e) => Disposed?.Invoke(s, e));
        private EventHandler OnDockChanged => new EventHandler((s, e) => DockChanged?.Invoke(s, e));
        private EventHandler OnDoubleClick => new EventHandler((s, e) => DoubleClick?.Invoke(s, e));
        private EventHandler OnDpiChangedAfterParent => new EventHandler((s, e) => DpiChangedAfterParent?.Invoke(s, e));
        private EventHandler OnDpiChangedBeforeParent => new EventHandler((s, e) => DpiChangedBeforeParent?.Invoke(s, e));
        private DragEventHandler OnDragDrop => new DragEventHandler((s, e) => DragDrop?.Invoke(s, e));
        private DragEventHandler OnDragEnter => new DragEventHandler((s, e) => DragEnter?.Invoke(s, e));
        private EventHandler OnDragLeave => new EventHandler((s, e) => DragLeave?.Invoke(s, e));
        private DragEventHandler OnDragOver => new DragEventHandler((s, e) => DragOver?.Invoke(s, e));
        private EventHandler OnEnabledChanged => new EventHandler((s, e) => EnabledChanged?.Invoke(s, e));
        private EventHandler OnEnter => new EventHandler((s, e) => Enter?.Invoke(s, e));
        private EventHandler OnFontChanged => new EventHandler((s, e) => FontChanged?.Invoke(s, e));
        private EventHandler OnForeColorChanged => new EventHandler((s, e) => ForeColorChanged?.Invoke(s, e));
        private GiveFeedbackEventHandler OnGiveFeedback => new GiveFeedbackEventHandler((s, e) => GiveFeedback?.Invoke(s, e));
        private EventHandler OnGotFocus => new EventHandler((s, e) => GotFocus?.Invoke(s, e));
        private EventHandler OnHandleCreated => new EventHandler((s, e) => HandleCreated?.Invoke(s, e));
        private EventHandler OnHandleDestroyed => new EventHandler((s, e) => HandleDestroyed?.Invoke(s, e));
        private HelpEventHandler OnHelpRequested => new HelpEventHandler((s, e) => HelpRequested?.Invoke(s, e));
        private EventHandler OnImeModeChanged => new EventHandler((s, e) => ImeModeChanged?.Invoke(s, e));
        private InvalidateEventHandler OnInvalidated => new InvalidateEventHandler((s, e) => Invalidated?.Invoke(s, e));
        private KeyEventHandler OnKeyDown => new KeyEventHandler((s, e) => KeyDown?.Invoke(s, e));
        private KeyPressEventHandler OnKeyPress => new KeyPressEventHandler((s, e) => KeyPress?.Invoke(s, e));
        private KeyEventHandler OnKeyUp => new KeyEventHandler((s, e) => KeyUp?.Invoke(s, e));
        private LayoutEventHandler OnLayout => new LayoutEventHandler((s, e) => Layout?.Invoke(s, e));
        private EventHandler OnLeave => new EventHandler((s, e) => Leave?.Invoke(s, e));
        private EventHandler OnLocationChanged => new EventHandler((s, e) => LocationChanged?.Invoke(s, e));
        private EventHandler OnLostFocus => new EventHandler((s, e) => LostFocus?.Invoke(s, e));
        private EventHandler OnMarginChanged => new EventHandler((s, e) => MarginChanged?.Invoke(s, e));
        private EventHandler OnMouseCaptureChanged => new EventHandler((s, e) => MouseCaptureChanged?.Invoke(s, e));
        private MouseEventHandler OnMouseClick => new MouseEventHandler((s, e) => MouseClick?.Invoke(s, e));
        private MouseEventHandler OnMouseDoubleClick => new MouseEventHandler((s, e) => MouseDoubleClick?.Invoke(s, e));
        private MouseEventHandler OnMouseDown => new MouseEventHandler((s, e) => MouseDown?.Invoke(s, e));
        private EventHandler OnMouseEnter => new EventHandler((s, e) => MouseEnter?.Invoke(s, e));
        private EventHandler OnMouseHover => new EventHandler((s, e) => MouseHover?.Invoke(s, e));
        private EventHandler OnMouseLeave => new EventHandler((s, e) => MouseLeave?.Invoke(s, e));
        private MouseEventHandler OnMouseMove => new MouseEventHandler((s, e) => MouseMove?.Invoke(s, e));
        private MouseEventHandler OnMouseUp => new MouseEventHandler((s, e) => MouseUp?.Invoke(s, e));
        private MouseEventHandler OnMouseWheel => new MouseEventHandler((s, e) => MouseWheel?.Invoke(s, e));
        private EventHandler OnMove => new EventHandler((s, e) => Move?.Invoke(s, e));
        private EventHandler OnPaddingChanged => new EventHandler((s, e) => PaddingChanged?.Invoke(s, e));
        private PaintEventHandler OnPaint => new PaintEventHandler((s, e) => Paint?.Invoke(s, e));
        private EventHandler OnParentChanged => new EventHandler((s, e) => ParentChanged?.Invoke(s, e));
        private PreviewKeyDownEventHandler OnPreviewKeyDown => new PreviewKeyDownEventHandler((s, e) => PreviewKeyDown?.Invoke(s, e));
        private QueryAccessibilityHelpEventHandler OnQueryAccessibilityHelp => new QueryAccessibilityHelpEventHandler((s, e) => QueryAccessibilityHelp?.Invoke(s, e));
        private QueryContinueDragEventHandler OnQueryContinueDrag => new QueryContinueDragEventHandler((s, e) => QueryContinueDrag?.Invoke(s, e));
        private EventHandler OnRegionChanged => new EventHandler((s, e) => RegionChanged?.Invoke(s, e));
        private EventHandler OnResize => new EventHandler((s, e) => Resize?.Invoke(s, e));
        private EventHandler OnRightToLeftChanged => new EventHandler((s, e) => RightToLeftChanged?.Invoke(s, e));
        private EventHandler OnSizeChanged => new EventHandler((s, e) => SizeChanged?.Invoke(s, e));
        private EventHandler OnStyleChanged => new EventHandler((s, e) => StyleChanged?.Invoke(s, e));
        private EventHandler OnSystemColorsChanged => new EventHandler((s, e) => SystemColorsChanged?.Invoke(s, e));
        private EventHandler OnTabIndexChanged => new EventHandler((s, e) => TabIndexChanged?.Invoke(s, e));
        private EventHandler OnTabStopChanged => new EventHandler((s, e) => TabStopChanged?.Invoke(s, e));
        private EventHandler OnTextChanged => new EventHandler((s, e) => TextChanged?.Invoke(s, e));
        private EventHandler OnValidated => new EventHandler((s, e) => Validated?.Invoke(s, e));
        private CancelEventHandler OnValidating => new CancelEventHandler((s, e) => Validating?.Invoke(s, e));
        private EventHandler OnVisibleChanged => new EventHandler((s, e) => VisibleChanged?.Invoke(s, e));

#pragma warning restore IDE0051 

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
        public new event EventHandler Disposed;
        public event EventHandler DockChanged;
        public event EventHandler DoubleClick;
        public event EventHandler DpiChangedAfterParent;
        public event EventHandler DpiChangedBeforeParent;
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
