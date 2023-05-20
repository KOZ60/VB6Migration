namespace VBCompatible.ControlArray
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [ProvideProperty("Index", typeof(VBTreeView))]
    public class VBTreeViewArray : VBControllArray<VBTreeView>
    {
        public VBTreeViewArray() { }

        public VBTreeViewArray(IContainer Container) : base(Container) { }

        protected override void HookUpEvents(VBTreeView o) {
            o.AfterCheck += OnAfterCheck;
            o.AfterCollapse += OnAfterCollapse;
            o.AfterExpand += OnAfterExpand;
            o.AfterLabelEdit += OnAfterLabelEdit;
            o.AfterSelect += OnAfterSelect;
            o.BackgroundImageChanged += OnBackgroundImageChanged;
            o.BackgroundImageLayoutChanged += OnBackgroundImageLayoutChanged;
            o.BeforeCheck += OnBeforeCheck;
            o.BeforeCollapse += OnBeforeCollapse;
            o.BeforeExpand += OnBeforeExpand;
            o.BeforeLabelEdit += OnBeforeLabelEdit;
            o.BeforeSelect += OnBeforeSelect;
            o.DrawNode += OnDrawNode;
            o.DropHighlightChanged += OnDropHighlightChanged;
            o.ItemDrag += OnItemDrag;
            o.NodeMouseClick += OnNodeMouseClick;
            o.NodeMouseDoubleClick += OnNodeMouseDoubleClick;
            o.NodeMouseHover += OnNodeMouseHover;
            o.PaddingChanged += OnPaddingChanged;
            o.Paint += OnPaint;
            o.RightToLeftLayoutChanged += OnRightToLeftLayoutChanged;
            o.TextChanged += OnTextChanged;
        }

        protected override void HookDownEvents(VBTreeView o) {
            o.AfterCheck -= OnAfterCheck;
            o.AfterCollapse -= OnAfterCollapse;
            o.AfterExpand -= OnAfterExpand;
            o.AfterLabelEdit -= OnAfterLabelEdit;
            o.AfterSelect -= OnAfterSelect;
            o.BackgroundImageChanged -= OnBackgroundImageChanged;
            o.BackgroundImageLayoutChanged -= OnBackgroundImageLayoutChanged;
            o.BeforeCheck -= OnBeforeCheck;
            o.BeforeCollapse -= OnBeforeCollapse;
            o.BeforeExpand -= OnBeforeExpand;
            o.BeforeLabelEdit -= OnBeforeLabelEdit;
            o.BeforeSelect -= OnBeforeSelect;
            o.DrawNode -= OnDrawNode;
            o.DropHighlightChanged -= OnDropHighlightChanged;
            o.ItemDrag -= OnItemDrag;
            o.NodeMouseClick -= OnNodeMouseClick;
            o.NodeMouseDoubleClick -= OnNodeMouseDoubleClick;
            o.NodeMouseHover -= OnNodeMouseHover;
            o.PaddingChanged -= OnPaddingChanged;
            o.Paint -= OnPaint;
            o.RightToLeftLayoutChanged -= OnRightToLeftLayoutChanged;
            o.TextChanged -= OnTextChanged;
        }

        private TreeViewEventHandler OnAfterCheck => new TreeViewEventHandler((s, e) => AfterCheck?.Invoke(s, e));
        private TreeViewEventHandler OnAfterCollapse => new TreeViewEventHandler((s, e) => AfterCollapse?.Invoke(s, e));
        private TreeViewEventHandler OnAfterExpand => new TreeViewEventHandler((s, e) => AfterExpand?.Invoke(s, e));
        private NodeLabelEditEventHandler OnAfterLabelEdit => new NodeLabelEditEventHandler((s, e) => AfterLabelEdit?.Invoke(s, e));
        private TreeViewEventHandler OnAfterSelect => new TreeViewEventHandler((s, e) => AfterSelect?.Invoke(s, e));
        private EventHandler OnBackgroundImageChanged => new EventHandler((s, e) => BackgroundImageChanged?.Invoke(s, e));
        private EventHandler OnBackgroundImageLayoutChanged => new EventHandler((s, e) => BackgroundImageLayoutChanged?.Invoke(s, e));
        private TreeViewCancelEventHandler OnBeforeCheck => new TreeViewCancelEventHandler((s, e) => BeforeCheck?.Invoke(s, e));
        private TreeViewCancelEventHandler OnBeforeCollapse => new TreeViewCancelEventHandler((s, e) => BeforeCollapse?.Invoke(s, e));
        private TreeViewCancelEventHandler OnBeforeExpand => new TreeViewCancelEventHandler((s, e) => BeforeExpand?.Invoke(s, e));
        private NodeLabelEditEventHandler OnBeforeLabelEdit => new NodeLabelEditEventHandler((s, e) => BeforeLabelEdit?.Invoke(s, e));
        private TreeViewCancelEventHandler OnBeforeSelect => new TreeViewCancelEventHandler((s, e) => BeforeSelect?.Invoke(s, e));
        private DrawTreeNodeEventHandler OnDrawNode => new DrawTreeNodeEventHandler((s, e) => DrawNode?.Invoke(s, e));
        private EventHandler OnDropHighlightChanged => new EventHandler((s, e) => DropHighlightChanged?.Invoke(s, e));
        private ItemDragEventHandler OnItemDrag => new ItemDragEventHandler((s, e) => ItemDrag?.Invoke(s, e));
        private TreeNodeMouseClickEventHandler OnNodeMouseClick => new TreeNodeMouseClickEventHandler((s, e) => NodeMouseClick?.Invoke(s, e));
        private TreeNodeMouseClickEventHandler OnNodeMouseDoubleClick => new TreeNodeMouseClickEventHandler((s, e) => NodeMouseDoubleClick?.Invoke(s, e));
        private TreeNodeMouseHoverEventHandler OnNodeMouseHover => new TreeNodeMouseHoverEventHandler((s, e) => NodeMouseHover?.Invoke(s, e));
        private EventHandler OnPaddingChanged => new EventHandler((s, e) => PaddingChanged?.Invoke(s, e));
        private PaintEventHandler OnPaint => new PaintEventHandler((s, e) => Paint?.Invoke(s, e));
        private EventHandler OnRightToLeftLayoutChanged => new EventHandler((s, e) => RightToLeftLayoutChanged?.Invoke(s, e));
        private EventHandler OnTextChanged => new EventHandler((s, e) => TextChanged?.Invoke(s, e));

        public event TreeViewEventHandler AfterCheck;
        public event TreeViewEventHandler AfterCollapse;
        public event TreeViewEventHandler AfterExpand;
        public event NodeLabelEditEventHandler AfterLabelEdit;
        public event TreeViewEventHandler AfterSelect;
        public new event EventHandler BackgroundImageChanged;
        public new event EventHandler BackgroundImageLayoutChanged;
        public event TreeViewCancelEventHandler BeforeCheck;
        public event TreeViewCancelEventHandler BeforeCollapse;
        public event TreeViewCancelEventHandler BeforeExpand;
        public event NodeLabelEditEventHandler BeforeLabelEdit;
        public event TreeViewCancelEventHandler BeforeSelect;
        public event DrawTreeNodeEventHandler DrawNode;
        public event EventHandler DropHighlightChanged;
        public event ItemDragEventHandler ItemDrag;
        public event TreeNodeMouseClickEventHandler NodeMouseClick;
        public event TreeNodeMouseClickEventHandler NodeMouseDoubleClick;
        public event TreeNodeMouseHoverEventHandler NodeMouseHover;
        public new event EventHandler PaddingChanged;
        public new event PaintEventHandler Paint;
        public event EventHandler RightToLeftLayoutChanged;
        public new event EventHandler TextChanged;
    }
}
