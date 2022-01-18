using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace VBCompatible.ControlArray
{
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    [ProvideProperty("Index", typeof(Control))]
    public class VBControllArray<T> : VBControlArray, IExtenderProvider where T : Control
    {
        public VBControllArray() { }

        public VBControllArray(IContainer Container) : base(Container) { }

        public T this[int Index] {
            get {
                return (T)base.BaseGetItem(Index);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool CanExtend(object target) {
            bool flag = false;
            if (GetControlInstanceType().Equals(target.GetType())) {
                flag = base.BaseCanExtend(RuntimeHelpers.GetObjectValue(target));
            }
            return flag;
        }

        protected override Type GetControlInstanceType() {
            return typeof(T);
        }

        public int GetIndex(Button o) {
            return base.BaseGetIndex(o);
        }

        protected virtual void HookUpControl(T target) {
            if (AutoSizeChanged != null) target.AutoSizeChanged += AutoSizeChanged;
            if (BackColorChanged != null) target.BackColorChanged += BackColorChanged;
            if (BackgroundImageChanged != null) target.BackgroundImageChanged += BackgroundImageChanged;
            if (BackgroundImageLayoutChanged != null) target.BackgroundImageLayoutChanged += BackgroundImageLayoutChanged;
            if (BindingContextChanged != null) target.BindingContextChanged += BindingContextChanged;
            if (CausesValidationChanged != null) target.CausesValidationChanged += CausesValidationChanged;
            if (ChangeUICues != null) target.ChangeUICues += ChangeUICues;
            if (Click != null) target.Click += Click;
            if (ClientSizeChanged != null) target.ClientSizeChanged += ClientSizeChanged;
            if (ContextMenuChanged != null) target.ContextMenuChanged += ContextMenuChanged;
            if (ContextMenuStripChanged != null) target.ContextMenuStripChanged += ContextMenuStripChanged;
            if (ControlAdded != null) target.ControlAdded += ControlAdded;
            if (ControlRemoved != null) target.ControlRemoved += ControlRemoved;
            if (CursorChanged != null) target.CursorChanged += CursorChanged;
            if (Disposed != null) target.Disposed += Disposed;
            if (DockChanged != null) target.DockChanged += DockChanged;
            if (DoubleClick != null) target.DoubleClick += DoubleClick;
            //if (DpiChangedAfterParent != null) target.DpiChangedAfterParent += DpiChangedAfterParent;
            //if (DpiChangedBeforeParent != null) target.DpiChangedBeforeParent += DpiChangedBeforeParent;
            if (DragDrop != null) target.DragDrop += DragDrop;
            if (DragEnter != null) target.DragEnter += DragEnter;
            if (DragLeave != null) target.DragLeave += DragLeave;
            if (DragOver != null) target.DragOver += DragOver;
            if (EnabledChanged != null) target.EnabledChanged += EnabledChanged;
            if (Enter != null) target.Enter += Enter;
            if (FontChanged != null) target.FontChanged += FontChanged;
            if (ForeColorChanged != null) target.ForeColorChanged += ForeColorChanged;
            if (GiveFeedback != null) target.GiveFeedback += GiveFeedback;
            if (GotFocus != null) target.GotFocus += GotFocus;
            if (HandleCreated != null) target.HandleCreated += HandleCreated;
            if (HandleDestroyed != null) target.HandleDestroyed += HandleDestroyed;
            if (HelpRequested != null) target.HelpRequested += HelpRequested;
            if (ImeModeChanged != null) target.ImeModeChanged += ImeModeChanged;
            if (Invalidated != null) target.Invalidated += Invalidated;
            if (KeyDown != null) target.KeyDown += KeyDown;
            if (KeyPress != null) target.KeyPress += KeyPress;
            if (KeyUp != null) target.KeyUp += KeyUp;
            if (Layout != null) target.Layout += Layout;
            if (Leave != null) target.Leave += Leave;
            if (LocationChanged != null) target.LocationChanged += LocationChanged;
            if (LostFocus != null) target.LostFocus += LostFocus;
            if (MarginChanged != null) target.MarginChanged += MarginChanged;
            if (MouseCaptureChanged != null) target.MouseCaptureChanged += MouseCaptureChanged;
            if (MouseClick != null) target.MouseClick += MouseClick;
            if (MouseDoubleClick != null) target.MouseDoubleClick += MouseDoubleClick;
            if (MouseDown != null) target.MouseDown += MouseDown;
            if (MouseEnter != null) target.MouseEnter += MouseEnter;
            if (MouseHover != null) target.MouseHover += MouseHover;
            if (MouseLeave != null) target.MouseLeave += MouseLeave;
            if (MouseMove != null) target.MouseMove += MouseMove;
            if (MouseUp != null) target.MouseUp += MouseUp;
            if (MouseWheel != null) target.MouseWheel += MouseWheel;
            if (Move != null) target.Move += Move;
            if (PaddingChanged != null) target.PaddingChanged += PaddingChanged;
            if (Paint != null) target.Paint += Paint;
            if (ParentChanged != null) target.ParentChanged += ParentChanged;
            if (PreviewKeyDown != null) target.PreviewKeyDown += PreviewKeyDown;
            if (QueryAccessibilityHelp != null) target.QueryAccessibilityHelp += QueryAccessibilityHelp;
            if (QueryContinueDrag != null) target.QueryContinueDrag += QueryContinueDrag;
            if (RegionChanged != null) target.RegionChanged += RegionChanged;
            if (Resize != null) target.Resize += Resize;
            if (RightToLeftChanged != null) target.RightToLeftChanged += RightToLeftChanged;
            if (SizeChanged != null) target.SizeChanged += SizeChanged;
            if (StyleChanged != null) target.StyleChanged += StyleChanged;
            if (SystemColorsChanged != null) target.SystemColorsChanged += SystemColorsChanged;
            if (TabIndexChanged != null) target.TabIndexChanged += TabIndexChanged;
            if (TabStopChanged != null) target.TabStopChanged += TabStopChanged;
            if (TextChanged != null) target.TextChanged += TextChanged;
            if (Validated != null) target.Validated += Validated;
            if (Validating != null) target.Validating += Validating;
            if (VisibleChanged != null) target.VisibleChanged += VisibleChanged;
        }

        protected override sealed void HookUpControlEvents(object o) {
            HookUpControl((T)o);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetIndex(T o) {
            base.BaseResetIndex(o);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetIndex(T o, int Index) {
            base.BaseSetIndex(o, Index, false);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeIndex(T o) {
            return base.BaseShouldSerializeIndex(o);
        }

        public EventHandler AutoSizeChanged;
        public EventHandler BackColorChanged;
        public EventHandler BackgroundImageChanged;
        public EventHandler BackgroundImageLayoutChanged;
        public EventHandler BindingContextChanged;
        public EventHandler CausesValidationChanged;
        public UICuesEventHandler ChangeUICues;
        public EventHandler Click;
        public EventHandler ClientSizeChanged;
        public EventHandler ContextMenuChanged;
        public EventHandler ContextMenuStripChanged;
        public ControlEventHandler ControlAdded;
        public ControlEventHandler ControlRemoved;
        public EventHandler CursorChanged;
        public new EventHandler Disposed;
        public EventHandler DockChanged;
        public EventHandler DoubleClick;
        //public EventHandler DpiChangedAfterParent;
        //public EventHandler DpiChangedBeforeParent;
        public DragEventHandler DragDrop;
        public DragEventHandler DragEnter;
        public EventHandler DragLeave;
        public DragEventHandler DragOver;
        public EventHandler EnabledChanged;
        public EventHandler Enter;
        public EventHandler FontChanged;
        public EventHandler ForeColorChanged;
        public GiveFeedbackEventHandler GiveFeedback;
        public EventHandler GotFocus;
        public EventHandler HandleCreated;
        public EventHandler HandleDestroyed;
        public HelpEventHandler HelpRequested;
        public EventHandler ImeModeChanged;
        public InvalidateEventHandler Invalidated;
        public KeyEventHandler KeyDown;
        public KeyPressEventHandler KeyPress;
        public KeyEventHandler KeyUp;
        public LayoutEventHandler Layout;
        public EventHandler Leave;
        public EventHandler LocationChanged;
        public EventHandler LostFocus;
        public EventHandler MarginChanged;
        public EventHandler MouseCaptureChanged;
        public MouseEventHandler MouseClick;
        public MouseEventHandler MouseDoubleClick;
        public MouseEventHandler MouseDown;
        public EventHandler MouseEnter;
        public EventHandler MouseHover;
        public EventHandler MouseLeave;
        public MouseEventHandler MouseMove;
        public MouseEventHandler MouseUp;
        public MouseEventHandler MouseWheel;
        public EventHandler Move;
        public EventHandler PaddingChanged;
        public PaintEventHandler Paint;
        public EventHandler ParentChanged;
        public PreviewKeyDownEventHandler PreviewKeyDown;
        public QueryAccessibilityHelpEventHandler QueryAccessibilityHelp;
        public QueryContinueDragEventHandler QueryContinueDrag;
        public EventHandler RegionChanged;
        public EventHandler Resize;
        public EventHandler RightToLeftChanged;
        public EventHandler SizeChanged;
        public EventHandler StyleChanged;
        public EventHandler SystemColorsChanged;
        public EventHandler TabIndexChanged;
        public EventHandler TabStopChanged;
        public EventHandler TextChanged;
        public EventHandler Validated;
        public CancelEventHandler Validating;
        public EventHandler VisibleChanged;
    }
}
