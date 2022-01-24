using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VBCompatible
{
    public class VBTreeView : TreeView
    {
        private static object EventDropHighlightChanged = new object();
        private VBOnwerDraw onwerDraw;

        public VBTreeView() {
            ResetFont();
            ResetForeColor();
            ResetBackColor();
            base.DrawMode = TreeViewDrawMode.OwnerDrawText;
            onwerDraw = new VBOnwerDraw(this, false, NativeDrawMode.WmPaint);
        }

        #region Font/ForeColor/BackColor

        private Font _Font = null;
        private Color _ForeColor = Color.Empty;
        private Color _BackColor = Color.Empty;

        // -------------------------------------------------------------------------------
        // Font プロパティ
        // -------------------------------------------------------------------------------
        public override Font Font {
            get {
                if (_Font == null) {
                    Form form = FindForm();
                    if (form != null) {
                        return form.Font;
                    } else {
                        return VBSystem.DefaultFont;
                    }
                }
                return _Font;
            }
            set {
                base.Font = value;
                _Font = value;
            }
        }

        public override void ResetFont() {
            Font = null;
        }

        protected virtual bool ShouldSerializeFont() {
            return _Font != null;
        }

        // -------------------------------------------------------------------------------
        // ForeColor プロパティ
        // -------------------------------------------------------------------------------
        public override Color ForeColor {
            get {
                if (_ForeColor.IsEmpty) {
                    return SystemColors.WindowText;
                }
                return _ForeColor;
            }
            set {
                base.ForeColor = value.IsEmpty ? SystemColors.WindowText : value;
                _ForeColor = value;
                if (IsHandleCreated) {
                    Invalidate();
                }
            }
        }

        public override void ResetForeColor() {
            ForeColor = Color.Empty;
        }

        protected virtual bool ShouldSerializeForeColor() {
            return !_ForeColor.IsEmpty;
        }

        // -------------------------------------------------------------------------------
        // BackColor プロパティ
        // -------------------------------------------------------------------------------

        public override Color BackColor {
            get {
                if (_BackColor.IsEmpty) {
                    return SystemColors.Window;
                }
                return _BackColor;
            }
            set {
                base.BackColor = value.IsEmpty ? SystemColors.Window.ToArgbColor() : value;
                _BackColor = value;
                if (IsHandleCreated) {
                    Invalidate();
                }
            }
        }

        public override void ResetBackColor() {
            BackColor = Color.Empty;
        }

        protected virtual bool ShouldSerializeBackColor() {
            return !_BackColor.IsEmpty;
        }

        #endregion

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete("DrawMode プロパティは使用できません。", false)]
        public new TreeViewDrawMode DrawMode {
            get { return base.DrawMode; }
            set { }
        }

        // -------------------------------------------------------------------------------
        // DropHighlight プロパティ
        // -------------------------------------------------------------------------------

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual TreeNode DropHighlight {
            get {
                IntPtr result = DropHighlightInternal;
                if (result == IntPtr.Zero)
                    return null;
                else
                    return TreeNode.FromHandle(this, result);
            }
            set {
                IntPtr prevHItem = this.DropHighlightInternal;
                IntPtr hItem = (value != null) ? value.Handle : IntPtr.Zero;

                if (hItem != prevHItem) {
                    DropHighlightInternal = hItem;
                }
            }
        }

        protected virtual void OnDropHighlightChanged(EventArgs e) {
            EventHandler eh = Events[EventDropHighlightChanged] as EventHandler;
            if (eh != null) eh(this, e);
        }

        public event EventHandler DropHighlightChanged {
            add {
                Events.AddHandler(EventDropHighlightChanged, value);
            }
            remove {
                Events.RemoveHandler(EventDropHighlightChanged, value);
            }
        }

        internal IntPtr DropHighlightInternal {
            get {
                return NativeMethods.SendMessage(
                                        Handle,
                                        NativeMethods.TVM_GETNEXTITEM,
                                        new IntPtr(NativeMethods.TVGN_DROPHILITE),
                                        IntPtr.Zero);
            }
            set {
                NativeMethods.SendMessage(
                                        Handle,
                                        NativeMethods.TVM_SELECTITEM,
                                        new IntPtr(NativeMethods.TVGN_DROPHILITE),
                                        value);
            }
        }


        protected override void WndProc(ref Message m) {
            switch (m.Msg) {
                case NativeMethods.TVM_SELECTITEM:
                    base.WndProc(ref m);
                    if (m.WParam == new IntPtr(NativeMethods.TVGN_DROPHILITE)) {
                        OnDropHighlightChanged(EventArgs.Empty);
                    }
                    break;
                case NativeMethods.WM_NOTIFY + NativeMethods.WM_REFLECT:
                    WmNotify(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private unsafe void WmNotify(ref Message m) {
            NativeMethods.NMHDR* nmhdr = (NativeMethods.NMHDR*)m.LParam;
            if (nmhdr->code == NativeMethods.NM_CUSTOMDRAW) {
                CustomDraw(ref m);
            } else {
                base.WndProc(ref m);
            }
        }

        // .NET ソースより一部抜粋
        private unsafe void CustomDraw(ref Message m) {

            NativeMethods.NMTVCUSTOMDRAW* nmcd = (NativeMethods.NMTVCUSTOMDRAW*)m.LParam;

            switch (nmcd->nmcd.dwDrawStage) {

                case NativeMethods.CDDS_ITEMPOSTPAINT:
                    //User draws only the text in OwnerDrawText mode, as explained in comments above
                    if (base.DrawMode == TreeViewDrawMode.OwnerDrawText) {

                        // nmcd.nmcd.rc.rc(RECT) が空なら描画しない
                        if (nmcd->nmcd.rc.IsEmpty) {
                            return;
                        }

                        // Get the node
                        TreeNode node = TreeNode.FromHandle(this, nmcd->nmcd.dwItemSpec);
                        if (node == null) {
                            return;
                        }

                        Graphics g = Graphics.FromHdcInternal(nmcd->nmcd.hdc);

                        DrawTreeNodeEventArgs e;

                        try {
                            Rectangle bounds = node.Bounds;
                            // 追加するときに node.TreeView が null を返すタイミングがある
                            //Size textSize = TextRenderer.MeasureText(node.Text, node.TreeView.Font);
                            Font font = GetNodeFont(node);
                            Size textSize = TextRenderer.MeasureText(node.Text, font);

                            Point textLoc = new Point(bounds.X - 1, bounds.Y); // required to center the text
                            bounds = new Rectangle(textLoc, new Size(textSize.Width, bounds.Height));

                            e = new DrawTreeNodeEventArgs(g, node, bounds, (TreeNodeStates)(nmcd->nmcd.uItemState));
                            e.DrawDefault = true;
                            OnDrawNode(e);

                            if (e.DrawDefault) {
                                DrawNodeDefault(g, e, node, bounds);
                            }
                        } finally {
                            g.Dispose();
                        }

                        m.Result = (IntPtr)NativeMethods.CDRF_NOTIFYSUBITEMDRAW;
                        return;
                    }

                    m.Result = (IntPtr)NativeMethods.CDRF_DODEFAULT;
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        // 追加するときに node.TreeView が null を返すタイミングがある
        private Font GetNodeFont(TreeNode node) {
            if (node.NodeFont != null) {
                return node.NodeFont;
            } else if (node.TreeView != null) {
                return node.TreeView.Font;
            } else {
                return this.Font;
            }
        }

        private void DrawNodeDefault(Graphics g, DrawTreeNodeEventArgs e, TreeNode node, Rectangle bounds) {
            TreeNodeStates curState = e.State;
            // OnDrawNode で変更される可能性があるので Font 再取得
            Font font = GetNodeFont(node);
            Color color = (((curState & TreeNodeStates.Selected) ==
                                    TreeNodeStates.Selected) && node.TreeView.Focused) ?
                                                                SystemColors.HighlightText :
                                                                (node.ForeColor != Color.Empty) ? node.ForeColor : node.TreeView.ForeColor;

            // 背景色で描画領域を塗りつぶす
            g.FillRectangle(VBGraphicsCache.GetSolidBrush(BackColor), e.Bounds);

            // テキストを描画する
            TextFormatFlags flags = VBControlPaint.CreateTextFormatFlags(this, ContentAlignment.MiddleLeft, false, false);
            if ((curState & TreeNodeStates.Selected) == TreeNodeStates.Selected) {
                g.FillRectangle(SystemBrushes.Highlight, bounds);
                ControlPaint.DrawFocusRectangle(g, bounds, color, SystemColors.Highlight);
                TextRenderer.DrawText(g, e.Node.Text, font, bounds, SystemColors.HighlightText, flags);
            } else {
                TextRenderer.DrawText(g, e.Node.Text, font, bounds, color, flags);
            }
        }

        private static Rectangle VerticalCenterRectangle(Rectangle rectangle, Size size) {
            int Y = rectangle.Y + ((rectangle.Height - size.Height + 1) / 2);
            return new Rectangle(rectangle.X, Y, size.Width, size.Height);
        }

        protected override void OnDrawNode(DrawTreeNodeEventArgs e) {

            // 高さまたは幅がゼロのものを除く
            if (e.Bounds.Height == 0 || e.Bounds.Width == 0) {
                return;
            }
            // クライアント領域外のものを除く
            if (!ClientRectangle.IntersectsWith(e.Bounds)) {
                return;
            }

            Color foreColor = e.Node.ForeColor.IsEmpty ? this.ForeColor : e.Node.ForeColor;
            Color backColor = e.Node.BackColor.IsEmpty ? this.BackColor : e.Node.BackColor;

            if (foreColor.IsEmpty) foreColor = SystemColors.WindowText;
            if (backColor.IsEmpty) backColor = SystemColors.Window;

            Font nodeFont = e.Node.NodeFont;
            if (nodeFont == null) nodeFont = this.Font;

            if (this.IsEnabled()) {
                if (e.Node.IsSelected && (ContainsFocus || !HideSelection)) {
                    foreColor = SystemColors.HighlightText;
                    backColor = SystemColors.Highlight;
                }
            } else {
                foreColor = SystemColors.ControlDark;
            }

            Rectangle face = e.Bounds;
            var brush = VBGraphicsCache.GetSolidBrush(backColor);
            e.Graphics.FillRectangle(brush, face);

            Rectangle textRect = face;
            textRect.Y -= 1;
            TextRenderer.DrawText(e.Graphics, e.Node.Text, nodeFont, textRect, foreColor, backColor, VBControlPaint.CreateTextFormatFlags(this, ContentAlignment.MiddleLeft));
            if ((e.State & TreeNodeStates.Focused) != 0) {
                Pen focusPen = VBGraphicsCache.GetPen(SystemColors.ControlDark, DashStyle.Dot);
                face.Size = new Size(face.Width - 1, face.Height - 1);
                e.Graphics.DrawRectangle(focusPen, face);
            }
            e.DrawDefault = false;
        }

    }
}
