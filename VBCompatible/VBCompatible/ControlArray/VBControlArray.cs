
namespace VBCompatible.ControlArray
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;
    using Microsoft.VisualBasic;
    using Microsoft.VisualBasic.CompilerServices;

    // [ProvideProperty("Index", typeof(T))]
    public abstract class VBControllArray<T> : Component, 
                            IExtenderProvider, IEnumerable<T> 
                            where T : Control, new()
    {
        private string _Name;
        protected readonly Dictionary<T, int> indices = new Dictionary<T, int>();
        protected readonly Dictionary<int, T> controls = new Dictionary<int, T>();
        protected IContainer components;

        public VBControllArray() { }

        public VBControllArray(IContainer Container) {
            Container.Add(this);
            components = Container;
        }

        public string Name { 
            get {
                return _Name;
            }
            set {
                if (_Name != value) {
                    _Name = value;
                    OnNameChanged(EventArgs.Empty);
                }
            }
        }

        protected virtual void OnNameChanged(EventArgs e) {
            indices.Clear();
            controls.Clear();
        }

        public T this[int Index] {
            get {
                return controls[Index];
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool CanExtend(object target) {
            var con = target as T;
            if (con != null) {
                return false;
            }
            return con.GetType().Equals(typeof(T)) && con.Name.StartsWith(Name);
        }

        public int GetIndex(T o) {
            if (indices.TryGetValue(o, out int index)) {
                return index;
            }
            return -1;
        }

        public void SetIndex(T o, int Index) {
            indices[o] = Index;
            controls[Index] = o;
            HookUpEventsOfControl(o);
            HookUpControl(o);
        }

        public void ResetIndex(T o) {
            if (indices.TryGetValue(o, out int index)) {
                indices.Remove(o);
                controls.Remove(index);
            }
        }

        public bool ShouldSerializeIndex(T o) {
            return indices.ContainsKey(o);
        }

        public int Count {
            get {
                return controls.Count;
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

        protected override void Dispose(bool disposing) {
            components = null;
            base.Dispose(disposing);
        }

        private void HookUpEventsOfControl(Control target) {
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
            // For .NET Core or NETx
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

        protected abstract void HookUpControl(T o);

        public int LBound() {
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

        public int UBound() {
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

        public void Load(int Index) {
            if (Index < 0 || controls.ContainsKey(Index) || Count == 0) {
                throw new IndexOutOfRangeException();
            }
            var original = controls[LBound()];
            var clone = CloneControl(original);
            SetIndex(clone, Index);
            original.Parent.Controls.Add(clone);
        }

        public void Unload(int Index) {
            if (Index < 0) {
                throw new IndexOutOfRangeException();
            }
            if (!controls.TryGetValue(Index, out T ctl)) {
                throw new IndexOutOfRangeException();
            }
            ctl.Parent.Controls.Remove(ctl);
            controls.Remove(Index);
            indices.Remove(ctl);
        }

        private T CloneControl(T ctl) {
            T newCtl = new T();
            var properties = TypeDescriptor.GetProperties(ctl);
            foreach (PropertyDescriptor item in properties) {
                if (ShouldCopyProperty(ctl, item)) {
                    try {
                        item.SetValue(newCtl, item.GetValue(ctl));
                    } catch {

                    }
                }
            }
            if (newCtl is RadioButton radioButton) {
                radioButton.Checked = false;
            }
            try {
                // VB6 から移植したフォームは ToolTip1 を持っている
                var form = ctl.FindForm();
                var toolTip = Versioned.CallByName(form, "ToolTip1", CallType.Get, new object[0]) as ToolTip;
                if (toolTip != null) {
                    var caption = toolTip.GetToolTip(ctl);
                    if (string.IsNullOrEmpty(caption)) {
                        toolTip.SetToolTip(newCtl, caption);
                    }
                }
            } catch {

            }
            return newCtl;
        }

        protected virtual bool ShouldCopyProperty(T ctl, PropertyDescriptor item) {
            if (item.IsReadOnly) return false;
            if (item.SerializationVisibility != DesignerSerializationVisibility.Visible) {
                return false;
            }
            try {
                if (!item.ShouldSerializeValue(ctl)) {
                    return false;
                }
            } catch {
                return false;
            }
            switch (item.Name) {
                case "TabIndex":
                case "Index":
                case "MdiList":
                    return false;
            }
            return true;
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
        // For .NET Core or NETx
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
