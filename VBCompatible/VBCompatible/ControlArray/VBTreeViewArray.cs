using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace VBCompatible.ControlArray
{
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    [ProvideProperty("Index", typeof(VBTreeView))]
    public class VBTreeViewArray : VBControllArray<VBTreeView>
    {
        public VBTreeViewArray() { }

        public VBTreeViewArray(IContainer Container) : base(Container) { }

        protected override void HookUpControl(VBTreeView target) {
            if (BeforeLabelEdit != null) target.BeforeLabelEdit += BeforeLabelEdit;
            if (AfterLabelEdit != null) target.AfterLabelEdit += AfterLabelEdit;
            if (BeforeCheck != null) target.BeforeCheck += BeforeCheck;
            if (AfterCheck != null) target.AfterCheck += AfterCheck;
            if (BeforeCollapse != null) target.BeforeCollapse += BeforeCollapse;
            if (AfterCollapse != null) target.AfterCollapse += AfterCollapse;
            if (BeforeExpand != null) target.BeforeExpand += BeforeExpand;
            if (AfterExpand != null) target.AfterExpand += AfterExpand;
            if (DrawNode != null) target.DrawNode += DrawNode;
            if (ItemDrag != null) target.ItemDrag += ItemDrag;
            if (NodeMouseHover != null) target.NodeMouseHover += NodeMouseHover;
            if (BeforeSelect != null) target.BeforeSelect += BeforeSelect;
            if (AfterSelect != null) target.AfterSelect += AfterSelect;
            if (NodeMouseClick != null) target.NodeMouseClick += NodeMouseClick;
            if (NodeMouseDoubleClick != null) target.NodeMouseDoubleClick += NodeMouseDoubleClick;
            if (RightToLeftLayoutChanged != null) target.RightToLeftLayoutChanged += RightToLeftLayoutChanged;
        }

        public NodeLabelEditEventHandler BeforeLabelEdit;
        public NodeLabelEditEventHandler AfterLabelEdit;
        public TreeViewCancelEventHandler BeforeCheck;
        public TreeViewEventHandler AfterCheck;
        public TreeViewCancelEventHandler BeforeCollapse;
        public TreeViewEventHandler AfterCollapse;
        public TreeViewCancelEventHandler BeforeExpand;
        public TreeViewEventHandler AfterExpand;
        public DrawTreeNodeEventHandler DrawNode;
        public ItemDragEventHandler ItemDrag;
        public TreeNodeMouseHoverEventHandler NodeMouseHover;
        public TreeViewCancelEventHandler BeforeSelect;
        public TreeViewEventHandler AfterSelect;
        public TreeNodeMouseClickEventHandler NodeMouseClick;
        public TreeNodeMouseClickEventHandler NodeMouseDoubleClick;
        public EventHandler RightToLeftLayoutChanged;
    }
}
