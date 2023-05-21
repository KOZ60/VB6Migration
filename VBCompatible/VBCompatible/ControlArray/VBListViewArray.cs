namespace VBCompatible.ControlArray
{

    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    [ProvideProperty("Index", typeof(VBListView))]
    public class VBListViewArray : BaseControlArray<VBListView>
    {

        public VBListViewArray() { }

        public VBListViewArray(IContainer Container) : base(Container) { }

        protected override void HookUpEvents(VBListView o) {
            o.AfterLabelEdit += OnAfterLabelEdit;
            o.BackgroundImageLayoutChanged += OnBackgroundImageLayoutChanged;
            o.BeforeLabelEdit += OnBeforeLabelEdit;
            o.CacheVirtualItems += OnCacheVirtualItems;
            o.ColumnClick += OnColumnClick;
            o.ColumnReordered += OnColumnReordered;
            o.ColumnWidthChanged += OnColumnWidthChanged;
            o.ColumnWidthChanging += OnColumnWidthChanging;
            o.DrawColumnHeader += OnDrawColumnHeader;
            o.DrawItem += OnDrawItem;
            o.DrawSubItem += OnDrawSubItem;
            o.ItemActivate += OnItemActivate;
            o.ItemCheck += OnItemCheck;
            o.ItemChecked += OnItemChecked;
            o.ItemDrag += OnItemDrag;
            o.ItemMouseHover += OnItemMouseHover;
            o.ItemSelectionChanged += OnItemSelectionChanged;
            o.PaddingChanged += OnPaddingChanged;
            o.Paint += OnPaint;
            o.RetrieveVirtualItem += OnRetrieveVirtualItem;
            o.RightToLeftLayoutChanged += OnRightToLeftLayoutChanged;
            o.SearchForVirtualItem += OnSearchForVirtualItem;
            o.SelectedIndexChanged += OnSelectedIndexChanged;
            o.TextChanged += OnTextChanged;
            o.VirtualItemsSelectionRangeChanged += OnVirtualItemsSelectionRangeChanged;
        }

        protected override void HookDownEvents(VBListView o) {
            o.AfterLabelEdit -= OnAfterLabelEdit;
            o.BackgroundImageLayoutChanged -= OnBackgroundImageLayoutChanged;
            o.BeforeLabelEdit -= OnBeforeLabelEdit;
            o.CacheVirtualItems -= OnCacheVirtualItems;
            o.ColumnClick -= OnColumnClick;
            o.ColumnReordered -= OnColumnReordered;
            o.ColumnWidthChanged -= OnColumnWidthChanged;
            o.ColumnWidthChanging -= OnColumnWidthChanging;
            o.DrawColumnHeader -= OnDrawColumnHeader;
            o.DrawItem -= OnDrawItem;
            o.DrawSubItem -= OnDrawSubItem;
            o.ItemActivate -= OnItemActivate;
            o.ItemCheck -= OnItemCheck;
            o.ItemChecked -= OnItemChecked;
            o.ItemDrag -= OnItemDrag;
            o.ItemMouseHover -= OnItemMouseHover;
            o.ItemSelectionChanged -= OnItemSelectionChanged;
            o.PaddingChanged -= OnPaddingChanged;
            o.Paint -= OnPaint;
            o.RetrieveVirtualItem -= OnRetrieveVirtualItem;
            o.RightToLeftLayoutChanged -= OnRightToLeftLayoutChanged;
            o.SearchForVirtualItem -= OnSearchForVirtualItem;
            o.SelectedIndexChanged -= OnSelectedIndexChanged;
            o.TextChanged -= OnTextChanged;
            o.VirtualItemsSelectionRangeChanged -= OnVirtualItemsSelectionRangeChanged;
        }

        private LabelEditEventHandler OnAfterLabelEdit => new LabelEditEventHandler((s, e) => AfterLabelEdit?.Invoke(s, e));
        private EventHandler OnBackgroundImageLayoutChanged => new EventHandler((s, e) => BackgroundImageLayoutChanged?.Invoke(s, e));
        private LabelEditEventHandler OnBeforeLabelEdit => new LabelEditEventHandler((s, e) => BeforeLabelEdit?.Invoke(s, e));
        private CacheVirtualItemsEventHandler OnCacheVirtualItems => new CacheVirtualItemsEventHandler((s, e) => CacheVirtualItems?.Invoke(s, e));
        private ColumnClickEventHandler OnColumnClick => new ColumnClickEventHandler((s, e) => ColumnClick?.Invoke(s, e));
        private ColumnReorderedEventHandler OnColumnReordered => new ColumnReorderedEventHandler((s, e) => ColumnReordered?.Invoke(s, e));
        private ColumnWidthChangedEventHandler OnColumnWidthChanged => new ColumnWidthChangedEventHandler((s, e) => ColumnWidthChanged?.Invoke(s, e));
        private ColumnWidthChangingEventHandler OnColumnWidthChanging => new ColumnWidthChangingEventHandler((s, e) => ColumnWidthChanging?.Invoke(s, e));
        private DrawListViewColumnHeaderEventHandler OnDrawColumnHeader => new DrawListViewColumnHeaderEventHandler((s, e) => DrawColumnHeader?.Invoke(s, e));
        private DrawListViewItemEventHandler OnDrawItem => new DrawListViewItemEventHandler((s, e) => DrawItem?.Invoke(s, e));
        private DrawListViewSubItemEventHandler OnDrawSubItem => new DrawListViewSubItemEventHandler((s, e) => DrawSubItem?.Invoke(s, e));
        private EventHandler OnItemActivate => new EventHandler((s, e) => ItemActivate?.Invoke(s, e));
        private ItemCheckEventHandler OnItemCheck => new ItemCheckEventHandler((s, e) => ItemCheck?.Invoke(s, e));
        private ItemCheckedEventHandler OnItemChecked => new ItemCheckedEventHandler((s, e) => ItemChecked?.Invoke(s, e));
        private ItemDragEventHandler OnItemDrag => new ItemDragEventHandler((s, e) => ItemDrag?.Invoke(s, e));
        private ListViewItemMouseHoverEventHandler OnItemMouseHover => new ListViewItemMouseHoverEventHandler((s, e) => ItemMouseHover?.Invoke(s, e));
        private ListViewItemSelectionChangedEventHandler OnItemSelectionChanged => new ListViewItemSelectionChangedEventHandler((s, e) => ItemSelectionChanged?.Invoke(s, e));
        private EventHandler OnPaddingChanged => new EventHandler((s, e) => PaddingChanged?.Invoke(s, e));
        private PaintEventHandler OnPaint => new PaintEventHandler((s, e) => Paint?.Invoke(s, e));
        private RetrieveVirtualItemEventHandler OnRetrieveVirtualItem => new RetrieveVirtualItemEventHandler((s, e) => RetrieveVirtualItem?.Invoke(s, e));
        private EventHandler OnRightToLeftLayoutChanged => new EventHandler((s, e) => RightToLeftLayoutChanged?.Invoke(s, e));
        private SearchForVirtualItemEventHandler OnSearchForVirtualItem => new SearchForVirtualItemEventHandler((s, e) => SearchForVirtualItem?.Invoke(s, e));
        private EventHandler OnSelectedIndexChanged => new EventHandler((s, e) => SelectedIndexChanged?.Invoke(s, e));
        private EventHandler OnTextChanged => new EventHandler((s, e) => TextChanged?.Invoke(s, e));
        private ListViewVirtualItemsSelectionRangeChangedEventHandler OnVirtualItemsSelectionRangeChanged => new ListViewVirtualItemsSelectionRangeChangedEventHandler((s, e) => VirtualItemsSelectionRangeChanged?.Invoke(s, e));

        public event LabelEditEventHandler AfterLabelEdit;
        public new event EventHandler BackgroundImageLayoutChanged;
        public event LabelEditEventHandler BeforeLabelEdit;
        public event CacheVirtualItemsEventHandler CacheVirtualItems;
        public event ColumnClickEventHandler ColumnClick;
        public event ColumnReorderedEventHandler ColumnReordered;
        public event ColumnWidthChangedEventHandler ColumnWidthChanged;
        public event ColumnWidthChangingEventHandler ColumnWidthChanging;
        public event DrawListViewColumnHeaderEventHandler DrawColumnHeader;
        public event DrawListViewItemEventHandler DrawItem;
        public event DrawListViewSubItemEventHandler DrawSubItem;
        public event EventHandler ItemActivate;
        public event ItemCheckEventHandler ItemCheck;
        public event ItemCheckedEventHandler ItemChecked;
        public event ItemDragEventHandler ItemDrag;
        public event ListViewItemMouseHoverEventHandler ItemMouseHover;
        public event ListViewItemSelectionChangedEventHandler ItemSelectionChanged;
        public new event EventHandler PaddingChanged;
        public new event PaintEventHandler Paint;
        public event RetrieveVirtualItemEventHandler RetrieveVirtualItem;
        public event EventHandler RightToLeftLayoutChanged;
        public event SearchForVirtualItemEventHandler SearchForVirtualItem;
        public event EventHandler SelectedIndexChanged;
        public new event EventHandler TextChanged;
        public event ListViewVirtualItemsSelectionRangeChangedEventHandler VirtualItemsSelectionRangeChanged;
    }
}
