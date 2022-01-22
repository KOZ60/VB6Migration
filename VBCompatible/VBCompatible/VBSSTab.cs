using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace VBCompatible
{
    /// <summary>
    /// VB6.0 SSTab 互換コントロール
    /// </summary>
    public class VBSSTab : TabControl
    {
        [ThreadStatic]
        private static VisualStyleRenderer s_Renderer = null;

        private static VisualStyleRenderer Renderer {
            get {
                if (s_Renderer == null) {
                    s_Renderer = new VisualStyleRenderer(VisualStyleElement.Tab.TabItem.Normal);
                }
                return s_Renderer;
            }
        }

        private VBTabPageCollection m_TabPageCollection;
        private TabVisibleCollection m_TabVisibleCollection;
        private bool m_EventSkip = false;
        private VBOnwerDraw onwerDraw;

        /// <summary>
        /// VBSSTab のインスタンスを作成します。
        /// </summary>
        public VBSSTab() {
            ResetFont();
            m_TabPageCollection = new VBTabPageCollection(this);
            m_TabVisibleCollection = new TabVisibleCollection(m_TabPageCollection);
            base.ResizeRedraw = true;
            base.DrawMode = TabDrawMode.OwnerDrawFixed;
            onwerDraw = new VBOnwerDraw(this, true, NativeDrawMode.WmPaint);
        }

        #region Font

        private Font _Font = null;

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

        #endregion


        #region 描画

        // DrawMode プロパティを隠す
        /// <summary>
        /// コントロールのタブを描画する方法を取得または設定します。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new TabDrawMode DrawMode { 
            get { return base.DrawMode; }
            set { }
        }

        /// <summary>
        /// DrawItem イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納した DrawItemEventArgs</param>
        protected override void OnDrawItem(DrawItemEventArgs e) {
            // TabItem を描画する
            // base.OnDrawItem(e);
            DrawItemBackGround(e);
            switch (this.Alignment) {
                case TabAlignment.Left:
                case TabAlignment.Right:
                    DrawItemRotate(e);
                    break;
                default:
                    DrawItemText(e.Graphics, e.Bounds, e);
                    break;
            }
        }

        // TabItem の背景を描画
        private void DrawItemBackGround(DrawItemEventArgs e) {
            Rectangle rectBackGround = e.Bounds;
            switch (this.Alignment) {
                case TabAlignment.Bottom:
                    rectBackGround.Y -= this.Padding.Y;
                    rectBackGround.Height += this.Padding.Y;
                    break;
                case TabAlignment.Left:
                    rectBackGround.Width += this.Padding.X;
                    break;
                case TabAlignment.Right:
                    rectBackGround.X -= this.Padding.X;
                    rectBackGround.Width += this.Padding.X;
                    break;
                default:
                    rectBackGround.Height += this.Padding.Y;
                    break;
            }
            var brush = VBGraphicsCache.GetSolidBrush(BackColor);
            e.Graphics.FillRectangle(brush, rectBackGround);
        }

        // タブが横にあるとき、画像を回転して描画する
        private void DrawItemRotate(DrawItemEventArgs e) {
            string tabItemText = base.TabPages[e.Index].Text;
            Font font = base.Font;
            Color textColor = base.ForeColor;
            TextFormatFlags flags = CreateTextFormatFlags(e);
            TabItemState state = (TabItemState)((int)e.State & 0xF);
            Rectangle contentBounds = e.Bounds;

            //タブの画像を作成する(縦横を反転)
            using (Bitmap bmp = new Bitmap(contentBounds.Height, contentBounds.Width)) {
                // bmp に文字を描画
                using (Graphics g = Graphics.FromImage(bmp)) {
                    DrawItemText(g, new Rectangle(0, 0, bmp.Width, bmp.Height), e);
                }

                //画像を回転する
                switch (this.Alignment) {
                    case TabAlignment.Left:
                        bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    case TabAlignment.Right:
                        bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                }

                // bmp を描画
                e.Graphics.DrawImage(bmp, contentBounds);
            }
        }

        private TextFormatFlags CreateTextFormatFlags(DrawItemEventArgs e) {
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine;

            // 選択されているときの縦は中央
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) {
                flags |= TextFormatFlags.VerticalCenter;
            } else {
                // 選択されていないときは内に寄せる
                switch (this.Alignment) {
                    case TabAlignment.Top:
                        flags |= TextFormatFlags.Bottom;
                        break;

                    case TabAlignment.Bottom:
                        flags |= TextFormatFlags.Top;
                        break;

                    default:
                        flags |= TextFormatFlags.Bottom;
                        break;
                }
            }
            // & を処理するかどうか
            if (!UseMnemonic) {
                flags = flags | TextFormatFlags.NoPrefix;
            }
            return flags;
        }


        // タブのテキストを描画する
        private void DrawItemText(Graphics g, Rectangle bounds, DrawItemEventArgs e) {
            string tabItemText = base.TabPages[e.Index].Text;
            TextFormatFlags flags = CreateTextFormatFlags(e);
            Color textColor = this.IsEnabled() ? base.ForeColor : SystemColors.ControlDark;
            TextRenderer.DrawText(g,
                            tabItemText,
                            base.Font,
                            bounds,
                            textColor,
                            flags);
        }

        /// <summary>
        /// Paint イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している System.Windows.Forms.PaintEventArgs。</param>
        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            if (Application.RenderWithVisualStyles) {
                Renderer.SetParameters(VisualStyleElement.Tab.Pane.Normal);
                Renderer.DrawParentBackground(e.Graphics, this.ClientRectangle, this);
                if (base.TabCount > 0) {
                    var tabArea = this.DisplayRectangle;
                    tabArea.Y += 1;
                    tabArea.Width += 1;
                    int nDelta = SystemInformation.Border3DSize.Width;
                    tabArea.Inflate(nDelta, nDelta);
                    Renderer.DrawBackground(e.Graphics, tabArea);
                    for (int i = 0; i < base.TabCount; i++) {
                        DrawTabPage(e.Graphics, i);
                    }
                }
            } else {
                // OwnerDrawFixed なので
                // base.WndProc(ref m) を呼び出すと OnDrawItem が呼ばれる
                IntPtr hdc = e.Graphics.GetHdc();
                try {
                    Message m = Message.Create(Handle, NativeMethods.WM_PAINT, hdc, IntPtr.Zero);
                    base.WndProc(ref m);
                } finally {
                    e.Graphics.ReleaseHdc();
                }
            }
        }

        private void DrawTabPage(Graphics g, int nIndex) {
            bool selected = base.SelectedIndex == nIndex;
            // タブ位置を微調整
            Rectangle tabBounds = base.GetTabRect(nIndex);
            Rectangle textFace = tabBounds;
            switch (base.Alignment) {
                case TabAlignment.Top:
                    tabBounds.Width += 1;
                    if (selected) {
                        tabBounds.Height += 2;
                    } else {
                        tabBounds.Y += 1;
                        textFace.Y -= 2;
                    }
                    break;

                case TabAlignment.Left:
                    tabBounds.Y += 1;
                    tabBounds.Height += 1;
                    if (selected) {
                        tabBounds.Width += 1;
                    } else {
                        tabBounds.X += 1;
                        tabBounds.Width -= 1;
                    }
                    break;

                case TabAlignment.Bottom:
                    tabBounds.Width += 1;
                    if (selected) {
                        tabBounds.Height += 1;
                        tabBounds.Y -= 1;
                    } else {
                        tabBounds.Height -= 1;
                        textFace.Y += 2;
                    }
                    break;

                case TabAlignment.Right:
                    tabBounds.Y += 1;
                    tabBounds.Height += 1;
                    if (selected) {
                        tabBounds.X -= 4;
                        tabBounds.Width += 4;
                    } else {
                        tabBounds.X -= 1;
                        tabBounds.Width -= 1;
                    }
                    break;

            }
            int bmpWidth;
            int bmpHeight;
            switch (base.Alignment) {
                case TabAlignment.Left:
                case TabAlignment.Right:
                    bmpWidth = tabBounds.Height;
                    bmpHeight = tabBounds.Width;
                    break;
                default:
                    bmpWidth = tabBounds.Width;
                    bmpHeight = tabBounds.Height;
                    break;
            }

            // bmp へ書き出す
            using (Bitmap bmp = new Bitmap(bmpWidth, bmpHeight)) {
                using (Graphics bmpGraphics = Graphics.FromImage(bmp)) {
                    if (selected) {
                        // Pressed の bmp は下１ドットが空白になるので削る
                        Renderer.SetParameters(VisualStyleElement.Tab.TabItem.Pressed);
                        Renderer.DrawBackground(bmpGraphics, new Rectangle(0, 0, bmpWidth, bmpHeight + 1), new Rectangle(0, 0, bmpWidth, bmpHeight));
                    } else {
                        Renderer.SetParameters(VisualStyleElement.Tab.TabItem.Normal);
                        Renderer.DrawBackground(bmpGraphics, new Rectangle(0, 0, bmpWidth, bmpHeight));
                    }
                }
                // Alignment にあわせて回転
                switch (base.Alignment) {
                    case TabAlignment.Bottom:
                        bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case TabAlignment.Left:
                        bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    case TabAlignment.Right:
                        bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                }
                g.DrawImage(bmp, tabBounds);
                DrawItemEventArgs e = new DrawItemEventArgs(g, base.Font, base.GetTabRect(nIndex), nIndex, selected ? DrawItemState.Selected : DrawItemState.None);
                switch (base.Alignment) {
                    case TabAlignment.Top:
                    case TabAlignment.Bottom:
                        DrawItemText(g, textFace, e);
                        break;
                    default:
                        DrawItemRotate(e);
                        break;
                }
            }

        }

        protected override void OnPaintBackground(PaintEventArgs pevent) {
            base.OnPaintBackground(pevent);
            if (Application.RenderWithVisualStyles) {
                Renderer.DrawParentBackground(pevent.Graphics, ClientRectangle, this);
            } else {

                // VisualStyle が Off のときは、TabItem の背景色を親の背景色で塗りつぶす

                Rectangle rect = this.ClientRectangle;

                // TabItem の表示領域を計算
                Size itemDisplaySize = this.ItemSize;

                // 高さと幅のいずれかがゼロなら描画しない
                if (this.ClientRectangle.Height <= 0) return;
                if (this.ClientRectangle.Width <= 0) return;

                // TabItem の段数を計算
                int stageY = itemDisplaySize.Height * base.TabPages.Count / this.ClientRectangle.Height + 1;
                int stageX = itemDisplaySize.Width * base.TabPages.Count / this.ClientRectangle.Width + 1;

                switch (this.Alignment) {
                    case TabAlignment.Bottom:
                        rect.Y = this.ClientRectangle.Bottom - itemDisplaySize.Height * stageX - this.Padding.Y;
                        rect.Height = this.Height - rect.Y;
                        break;
                    case TabAlignment.Left:
                        rect.Width = itemDisplaySize.Width * stageY + this.ClientRectangle.Left;
                        break;
                    case TabAlignment.Right:
                        rect.X = rect.Right - itemDisplaySize.Width * stageY;
                        rect.Width = itemDisplaySize.Width * stageY;
                        break;
                    default:
                        rect.Height = itemDisplaySize.Height * stageX + this.ClientRectangle.Top + this.Padding.Y;
                        break;
                }
                var brush = VBGraphicsCache.GetSolidBrush(Parent.BackColor);
                pevent.Graphics.FillRectangle(brush, rect);
            }
        }

        #endregion


        #region VB6 互換プロパティ

        // VB6 互換プロパティ

        /// <summary>
        /// タブ ダイアログ (SSTab) コントロール内のタブを表示するか非表示にするかを取得または設定します。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TabVisibleCollection TabVisible {
            get { return m_TabVisibleCollection; }
        }

        /// <summary>
        /// 現在選択されているタブ ページのインデックスを取得または設定します。
        /// </summary>
        public new int SelectedIndex {
            get { return ToSSTabIndex(base.SelectedIndex); }
            set { base.SelectedIndex = FromSSTabIndex(value); }
        }

        /// <summary>
        /// TabControl の Index を VBSSTab の Index 値に変換
        /// </summary>
        /// <param name="value">TabControl の Index 値</param>
        /// <returns>VBSSTab の Index 値</returns>
        private int ToSSTabIndex(int value) {
            int tabControlIndex = 0;
            for (int i = 0; i < m_TabPageCollection.Count; i++) {
                if (m_TabPageCollection.get_Visible(i)) {
                    if (tabControlIndex == value)
                        return i;
                    tabControlIndex++;
                }
            }
            return -1;
        }

        /// <summary>
        /// VBSSTab の Index を TabControl の Index 値に変換
        /// </summary>
        /// <param name="value">TabControl の Index 値</param>
        /// <returns>VBSSTab の Index 値</returns>
        private int FromSSTabIndex(int value) {
            if (m_TabPageCollection.get_Visible(value)) {
                int tabControlIndex = 0;
                for (int i = 0; i < value; i++) {
                    if (m_TabPageCollection.get_Visible(i)) {
                        tabControlIndex++;
                    }
                }
                return tabControlIndex;
            } else
                return -1;
        }

        /// <summary>
        /// SelectedIndexChanged イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納した EventArgs</param>
        protected override void OnSelectedIndexChanged(EventArgs e) {
            if (m_EventSkip == false && base.FindForm() != null)
                base.OnSelectedIndexChanged(e);
        }

        /// <summary>
        /// Selected イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納した TabControlEventArgs</param>
        protected override void OnSelected(TabControlEventArgs e) {
            if (m_EventSkip == false && base.FindForm() != null) {
                TabControlEventArgs newEvent = new TabControlEventArgs(e.TabPage, ToSSTabIndex(e.TabPageIndex), e.Action);
                base.OnSelected(newEvent);
            }
        }

        /// <summary>
        /// Deselected イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納した TabControlEventArgs</param>
        protected override void OnDeselected(TabControlEventArgs e) {
            if (m_EventSkip == false && base.FindForm() != null) {
                TabControlEventArgs newEvent = new TabControlEventArgs(e.TabPage, ToSSTabIndex(e.TabPageIndex), e.Action);
                base.OnDeselected(newEvent);
            }
        }

        /// <summary>
        /// Deselecting イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納した TabControlEventArgs</param>
        protected override void OnDeselecting(TabControlCancelEventArgs e) {
            if (m_EventSkip == false && base.FindForm() != null) {
                TabControlCancelEventArgs newEvent = new TabControlCancelEventArgs(e.TabPage, ToSSTabIndex(e.TabPageIndex), e.Cancel, e.Action);
                base.OnDeselecting(newEvent);
                e.Cancel = newEvent.Cancel;
            }
        }

        /// <summary>
        /// Selecting イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納した TabControlCancelEventArgs</param>
        protected override void OnSelecting(TabControlCancelEventArgs e) {
            if (m_EventSkip == false && base.FindForm() != null) {
                if (e.TabPage.Enabled) {
                    TabControlCancelEventArgs newEvent = new TabControlCancelEventArgs(e.TabPage, ToSSTabIndex(e.TabPageIndex), e.Cancel, e.Action);
                    base.OnSelecting(e);
                    e.Cancel = newEvent.Cancel;
                } else
                    e.Cancel = true;
            }
        }

        // プロパティを Shadow
        /// <summary>
        /// コントロール内に格納されている TabPage のコレクションを取得します。
        /// </summary>
        [Browsable(false)]
        public new VBTabPageCollection Controls {
            get { return m_TabPageCollection; }
        }

        /// <summary>
        /// コントロール内に格納されている TabPage のコレクションを取得します。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new VBTabPageCollection TabPages {
            get { return m_TabPageCollection; }
        }

        // TabPage の表示/非表示を保持するクラス
        private class TabPageVisible
        {
            private bool m_Visible;
            private TabPage m_Page;
            private HookWindow _HookWindow;

            public event EventHandler VisibleChanged;

            public TabPageVisible(TabPage page) {
                m_Page = page;
                m_Visible = true;
                m_Page.HandleCreated += ControlHandleCreated;
                if (m_Page.IsHandleCreated) {
                    ControlHandleCreated(m_Page, EventArgs.Empty);
                }
            }

            public bool Visible {
                get { return m_Visible; }
                set {
                    if (m_Visible != value) {
                        m_Visible = value;
                        OnVisibleChanged(EventArgs.Empty);
                    }
                }
            }

            public TabPage Page {
                get { return m_Page; }
                set { m_Page = value; }
            }

            protected virtual void OnVisibleChanged(EventArgs e) {
                if (VisibleChanged != null) VisibleChanged(m_Page, EventArgs.Empty);
            }

            private void ControlHandleCreated(object sender, EventArgs e) {
                _HookWindow = new HookWindow(this);
                _HookWindow.AssignHandle(m_Page.Handle);
            }

            internal void ReleaseWindow() {
                if (_HookWindow != null) {
                    _HookWindow.ReleaseHandle();
                    _HookWindow = null;
                }
            }

            private class HookWindow : NativeWindow
            {
                TabPageVisible _Owner;
                public HookWindow(TabPageVisible owner) {
                    _Owner = owner;
                }

                /// <summary>
                /// Windows メッセージを処理します。
                /// </summary>
                /// <param name="m">処理対象の Windows Message。 </param>
                protected override void WndProc(ref Message m) {
                    switch (m.Msg) {
                        case NativeMethods.WM_NCDESTROY:
                            base.WndProc(ref m);
                            break;
                        default:
                            // コンテナ共通処理
                            if (VBUtils.IsCtlColor(ref m)) {
                                Control con = Control.FromHandle(m.LParam);
                                if (con != null) {
                                    VBUtils.SetCtlColor(con, ref m);
                                    return;
                                }
                                base.DefWndProc(ref m);
                            } else {
                                base.WndProc(ref m);
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// TabPage の表示/非表示を保持するクラスのコレクション
        /// </summary>
        public class VBTabPageCollection : IEnumerable
        {
            VBSSTab m_Owner;
            List<TabPageVisible> m_List;

            internal VBTabPageCollection(VBSSTab owner) {
                m_Owner = owner;
                m_List = new List<TabPageVisible>();
            }

            /// <summary>
            /// TabPage の表示/非表示を取得します。
            /// </summary>
            /// <param name="index">インデックスを指定します。</param>
            /// <returns>表示するときは True、そうでないときは、False</returns>
            [EditorBrowsable(EditorBrowsableState.Never)]
            public bool get_Visible(int index) {
                return m_List[index].Visible;
            }

            /// <summary>
            /// TabPage の表示/非表示を設定します。
            /// </summary>
            /// <param name="index">インデックスを指定します。</param>
            /// <param name="value">表示するときは True、そうでないときは、False</param>
            [EditorBrowsable(EditorBrowsableState.Never)]
            public void set_Visible(int index, bool value) {
                m_List[index].Visible = value;
            }

            /// <summary>
            /// 指定された位置の TabPage を取得します。
            /// </summary>
            /// <param name="index">インデックスを指定します。</param>
            /// <returns>指定された位置の TabPage。</returns>
            public TabPage this[int index] {
                get { return m_List[index - 1].Page; }
            }

            /// <summary>
            /// コレクションに含まれる TabPage の総数を取得します。
            /// </summary>
            public int Count {
                get { return m_List.Count; }
            }

            private TabPageCollection TabPages {
                get { return ((TabControl)m_Owner).TabPages; }
            }

            /// <summary>
            /// タブ ページをコレクションに追加します。
            /// </summary>
            /// <param name="page">追加する TabPage</param>
            public void Add(TabPage page) {
                page.BackColor = page.BackColor;
                TabPageVisible item = new TabPageVisible(page);
                item.VisibleChanged += OnTabVisibleChanged;
                m_List.Add(item);
                TabPages.Add(page);
            }

            /// <summary>
            /// タブ ページをコレクションから削除します。
            /// </summary>
            /// <param name="index">削除するタブページの位置を指定します。</param>
            public void Remove(int index) {
                TabPageVisible item = m_List[index];
                m_List.RemoveAt(index);
                item.VisibleChanged -= OnTabVisibleChanged;
                item.ReleaseWindow();

                // デザイン中なら TabPages から削除
                // 実行中は TabPage を再構成する

                if (m_Owner.DesignMode)
                    TabPages.RemoveAt(index);
                else
                    m_Owner.CreateVisibleTabs();
            }

            IEnumerator IEnumerable.GetEnumerator() {
                foreach (var item in m_List) {
                    yield return item.Page;
                }
            }

            /// <summary>
            /// タブページの表示/非表示が切り替わったときに発生します。
            /// </summary>
            /// <param name="sender">イベントが発生したタブページ。</param>
            /// <param name="e">イベント データを格納した EventArgs</param>
            protected void OnTabVisibleChanged(object sender, EventArgs e) {
                if (m_Owner.DesignMode) return;     // デザインモードでは何もしない
                m_Owner.CreateVisibleTabs();
            }

            /// <summary>
            /// コレクションがゼロオリジンかどうかを取得します。
            /// </summary>
            public bool ZeroOrigin {
                get { return true; }
            }
        }

        /// <summary>
        /// 表示用タブを作成
        /// </summary>
        protected void CreateVisibleTabs() {

            BeginUpdate();
            m_EventSkip = true;

            try {
                int selectedIndex = base.SelectedIndex;
                base.TabPages.Clear();

                for (int i = 1; i <= TabPages.Count; i++) {
                    TabPage page = TabPages[i];
                    if (TabVisible[i - 1]) {
                        page.Visible = true;
                        base.TabPages.Add(page);
                    }
                }

                if (base.TabPages.Count == 0) {
                    base.SelectedIndex = -1;
                } else {
                    if (selectedIndex == -1) {
                        base.SelectedIndex = 0;
                    } else if (selectedIndex < base.TabPages.Count) {
                        base.SelectedIndex = selectedIndex;
                    } else {
                        base.SelectedIndex = -1;
                    }
                }

            } finally {
                m_EventSkip = false;
                EndUpdate();
            }
            OnSelectedIndexChanged(EventArgs.Empty);
        }

        int updateCount = 0;

        public void BeginUpdate() {
            if (updateCount == 0) {
                if (IsHandleCreated) {
                    NativeMethods.SendMessage(Handle,
                                NativeMethods.WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
                }
            }
            updateCount++;
        }

        public void EndUpdate() {
            if (updateCount > 0) {
                updateCount--;
                if (updateCount == 0) {
                    ForceEndUpdate();
                }
            } else {
                ForceEndUpdate();
            }
        }

        protected void ForceEndUpdate() {
            if (IsHandleCreated) {
                NativeMethods.SendMessage(Handle,
                            NativeMethods.WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
                Refresh();
            }
            updateCount = 0;
        }

        protected override bool CanRaiseEvents {
            get {
                return !m_EventSkip;
            }
        }

        /// <summary>
        /// TabPage の表示/非表示を設定、取得します。
        /// </summary>
        public class TabVisibleCollection
        {
            private VBTabPageCollection m_Owner;
            internal TabVisibleCollection(VBTabPageCollection owner) {
                m_Owner = owner;
            }
            /// <summary>
            /// TabPage の表示/非表示を設定、取得します。
            /// </summary>
            /// <param name="index">設定、取得する TabPage の Index </param>
            /// <returns>表示するときは True しないときは False が帰ります。</returns>
            public bool this[int index] {
                get { return m_Owner.get_Visible(index); }
                set { m_Owner.set_Visible(index, value); }
            }
        }

        #endregion

        #region ニーモニックの処理

        /// <summary>
        /// ニーモニック文字を処理します。
        /// </summary>
        /// <param name="charCode">処理対象の文字。 </param>
        /// <returns>文字がコントロールによってニーモニックとして処理された場合は true。それ以外の場合は false。 </returns>
        protected override bool ProcessMnemonic(char charCode) {
            if (UseMnemonic && base.Enabled && base.Visible) {
                foreach (TabPage p in base.TabPages) {
                    if (Control.IsMnemonic(charCode, p.Text)) {
                        base.SelectedTab = p;
                        return true;
                    }
                }
            }
            return base.ProcessMnemonic(charCode);
        }

        // -------------------------------------------------------------------------------
        // UseMnemonic プロパティ
        // -------------------------------------------------------------------------------
        private bool m_UseMnemonic = true;

        /// <summary>
        /// TabPage の Text プロパティに含まれるアンパサンド文字 (&amp;) をアクセス キーの接頭文字として解釈するかどうかを示す値を取得または設定します。
        /// </summary>
        [DefaultValue(true)]
        public bool UseMnemonic {
            get {
                return m_UseMnemonic;
            }
            set {
                if (UseMnemonic != value) {
                    m_UseMnemonic = value;
                    if (base.IsHandleCreated) {
                        base.Invalidate();
                    }
                    OnUseMnemonicChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// UseMnemonicChanged イベントを発生させます。
        /// </summary>
        protected virtual void OnUseMnemonicChanged(EventArgs e) {
            if (UseMnemonicChanged != null) UseMnemonicChanged(this, e);
        }

        /// <summary>
        /// UseMnemonic プロパティの値が変更された場合に発生します。
        /// </summary>
        public event EventHandler UseMnemonicChanged;

        #endregion
    }
}
