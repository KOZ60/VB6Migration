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
            if (DockChanged != null) target.DockChanged += DockChanged;
            if (DragDrop != null) target.DragDrop += DragDrop;
            if (DragEnter != null) target.DragEnter += DragEnter;
            if (DragLeave != null) target.DragLeave += DragLeave;
            if (DragOver != null) target.DragOver += DragOver;
            if (EnabledChanged != null) target.EnabledChanged += EnabledChanged;
            if (Enter != null) target.Enter += Enter;
            if (FontChanged != null) target.FontChanged += FontChanged;
            if (ForeColorChanged != null) target.ForeColorChanged += ForeColorChanged;
            if (GiveFeedback != null) target.GiveFeedback += GiveFeedback;
            if (HelpRequested != null) target.HelpRequested += HelpRequested;
            if (KeyDown != null) target.KeyDown += KeyDown;
            if (KeyPress != null) target.KeyPress += KeyPress;
            if (KeyUp != null) target.KeyUp += KeyUp;
            if (Layout != null) target.Layout += Layout;
            if (Leave != null) target.Leave += Leave;
            if (LocationChanged != null) target.LocationChanged += LocationChanged;
            if (MarginChanged != null) target.MarginChanged += MarginChanged;
            if (MouseCaptureChanged != null) target.MouseCaptureChanged += MouseCaptureChanged;
            if (MouseClick != null) target.MouseClick += MouseClick;
            if (MouseDown != null) target.MouseDown += MouseDown;
            if (MouseEnter != null) target.MouseEnter += MouseEnter;
            if (MouseHover != null) target.MouseHover += MouseHover;
            if (MouseLeave != null) target.MouseLeave += MouseLeave;
            if (MouseMove != null) target.MouseMove += MouseMove;
            if (MouseUp != null) target.MouseUp += MouseUp;
            if (Move != null) target.Move += Move;
            if (PaddingChanged != null) target.PaddingChanged += PaddingChanged;
            if (Paint != null) target.Paint += Paint;
            if (ParentChanged != null) target.ParentChanged += ParentChanged;
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

        public event EventHandler DockChanged;

        public event DragEventHandler DragDrop;

        public event DragEventHandler DragEnter;

        public event EventHandler DragLeave;

        public event DragEventHandler DragOver;

        public event EventHandler EnabledChanged;

        public event EventHandler Enter;

        public event EventHandler FontChanged;

        public event EventHandler ForeColorChanged;

        public event GiveFeedbackEventHandler GiveFeedback;

        public event HelpEventHandler HelpRequested;

        public event KeyEventHandler KeyDown;

        public event KeyPressEventHandler KeyPress;

        public event KeyEventHandler KeyUp;

        public event LayoutEventHandler Layout;

        public event EventHandler Leave;

        public event EventHandler LocationChanged;

        public event EventHandler MarginChanged;

        public event EventHandler MouseCaptureChanged;

        public event MouseEventHandler MouseClick;

        public event MouseEventHandler MouseDown;

        public event EventHandler MouseEnter;

        public event EventHandler MouseHover;

        public event EventHandler MouseLeave;

        public event MouseEventHandler MouseMove;

        public event MouseEventHandler MouseUp;

        public event EventHandler Move;

        public event EventHandler PaddingChanged;

        public event PaintEventHandler Paint;

        public event EventHandler ParentChanged;

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
