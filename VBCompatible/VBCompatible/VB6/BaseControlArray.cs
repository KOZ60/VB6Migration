using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBCompatible.VB6
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows.Forms;

    [DesignerCategory("Code")]
    public abstract class BaseControlArray : Component, IExtenderProvider
    {
        private const BindingFlags bindingFlags =
                                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        private const BindingFlags declaredOnlyFlags = bindingFlags | BindingFlags.DeclaredOnly;

        private readonly static PropertyInfo[] EventProperties =
                    typeof(BaseControlArray).GetProperties(declaredOnlyFlags).Where(
                        (ei) => ei.CanRead && ei.Name.StartsWith("On")).ToArray();

        protected abstract Type GetControlInstanceType { get; }

        protected IContainer components;
        protected readonly Dictionary<Control, int> indices = new Dictionary<Control, int>();
        protected readonly Dictionary<int, Control> controls = new Dictionary<int, Control>();
        protected readonly HashSet<Control> controlAddedAtDesignTime = new HashSet<Control>();
        protected bool fIsEndInitCalled;

        private string _Name;
        private Dictionary<string, Delegate> _BaseControlEvents;
        private Form _Form;
        private Type _FormType;
        private bool _ToolTipScaned;
        private ToolTip _ToolTip;
        private const string _ToolTip1Name = "ToolTip1";

        protected BaseControlArray() { }

        protected BaseControlArray(IContainer Container) {
            components = Container;
            components.Add(this);
        }


        private Dictionary<string, Delegate> BaseControlEvents {
            get {
                if (_BaseControlEvents == null) {
                    _BaseControlEvents = new Dictionary<string, Delegate>();
                    foreach (var info in EventProperties) {
                        _BaseControlEvents.Add(info.Name.Substring(2), (Delegate)info.GetValue(this, null));
                    }
                }
                return _BaseControlEvents;
            }
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

        protected void HookUpEventsOfControl(Control o) {
            foreach (var info in typeof(Control).GetEvents()) {
                if (BaseControlEvents.TryGetValue(info.Name, out Delegate invoker)) {
                    info.AddEventHandler(o, invoker);
                }
            }
        }

        protected void HookDownEventsOfControl(Control o) {
            foreach (var info in typeof(Control).GetEvents()) {
                if (BaseControlEvents.TryGetValue(info.Name, out Delegate invoker)) {
                    info.RemoveEventHandler(o, invoker);
                }
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool CanExtend(object extendee) {
            if (!(extendee is Control o)) {
                return false;
            }
            if (!o.GetType().Equals(GetControlInstanceType)) {
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
            if (o is Control c) {
                if (indices.TryGetValue(c, out int index)) {
                    return index;
                }
            }
            return -1;
        }

        protected virtual int BaseGetIndex(T o) {
            if (o != null) {
                if (indices.TryGetValue(o, out int index)) {
                    return index;
                }
            }
            return -1;
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
