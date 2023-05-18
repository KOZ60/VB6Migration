using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace VBCompatible.ControlArray
{
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    [ProvideProperty("Index", typeof(VBListView))]
    public class VBListViewArray : VBControllArray<VBListView>
    {
        public VBListViewArray() { }

        public VBListViewArray(IContainer Container) : base(Container) { }

        protected override void HookUpControl(VBListView target) {
            if (RightToLeftLayoutChanged != null) target.RightToLeftLayoutChanged += RightToLeftLayoutChanged;
            if (AfterLabelEdit != null) target.AfterLabelEdit += AfterLabelEdit;
            if (BeforeLabelEdit != null) target.BeforeLabelEdit += BeforeLabelEdit;
            if (CacheVirtualItems != null) target.CacheVirtualItems += CacheVirtualItems;
            if (ColumnClick != null) target.ColumnClick += ColumnClick;
            if (ColumnReordered != null) target.ColumnReordered += ColumnReordered;
            if (ColumnWidthChanged != null) target.ColumnWidthChanged += ColumnWidthChanged;
            if (ColumnWidthChanging != null) target.ColumnWidthChanging += ColumnWidthChanging;
            if (DrawColumnHeader != null) target.DrawColumnHeader += DrawColumnHeader;
            if (DrawItem != null) target.DrawItem += DrawItem;
            if (DrawSubItem != null) target.DrawSubItem += DrawSubItem;
            if (ItemActivate != null) target.ItemActivate += ItemActivate;
            if (ItemCheck != null) target.ItemCheck += ItemCheck;
            if (ItemChecked != null) target.ItemChecked += ItemChecked;
            if (ItemDrag != null) target.ItemDrag += ItemDrag;
            if (ItemMouseHover != null) target.ItemMouseHover += ItemMouseHover;
            if (ItemSelectionChanged != null) target.ItemSelectionChanged += ItemSelectionChanged;
            if (RetrieveVirtualItem != null) target.RetrieveVirtualItem += RetrieveVirtualItem;
            if (SearchForVirtualItem != null) target.SearchForVirtualItem += SearchForVirtualItem;
            if (SelectedIndexChanged != null) target.SelectedIndexChanged += SelectedIndexChanged;
            if (VirtualItemsSelectionRangeChanged != null) target.VirtualItemsSelectionRangeChanged += VirtualItemsSelectionRangeChanged;
        }

        public EventHandler RightToLeftLayoutChanged;
        public LabelEditEventHandler AfterLabelEdit;
        public LabelEditEventHandler BeforeLabelEdit;
        public CacheVirtualItemsEventHandler CacheVirtualItems;
        public ColumnClickEventHandler ColumnClick;
        public ColumnReorderedEventHandler ColumnReordered;
        public ColumnWidthChangedEventHandler ColumnWidthChanged;
        public ColumnWidthChangingEventHandler ColumnWidthChanging;
        public DrawListViewColumnHeaderEventHandler DrawColumnHeader;
        public DrawListViewItemEventHandler DrawItem;
        public DrawListViewSubItemEventHandler DrawSubItem;
        public EventHandler ItemActivate;
        public ItemCheckEventHandler ItemCheck;
        public ItemCheckedEventHandler ItemChecked;
        public ItemDragEventHandler ItemDrag;
        public ListViewItemMouseHoverEventHandler ItemMouseHover;
        public ListViewItemSelectionChangedEventHandler ItemSelectionChanged;
        public RetrieveVirtualItemEventHandler RetrieveVirtualItem;
        public SearchForVirtualItemEventHandler SearchForVirtualItem;
        public EventHandler SelectedIndexChanged;
        public ListViewVirtualItemsSelectionRangeChangedEventHandler VirtualItemsSelectionRangeChanged;
    }
}
