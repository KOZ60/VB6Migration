using System;

namespace VBCompatible
{
    /// <summary>
    /// VB6.0 ItemData 互換クラス
    /// </summary>
    public class VBListBoxItem
    {

        internal VBListBox Owner = null;

        /// <summary>
        /// VBListBoxItem のインスタンスを作成します。
        /// </summary>
        /// <param name="ItemString">表示される文字列を指定します。</param>
        /// <param name="ItemData">オブジェクトに含まれる項目を示す番号を指定します。</param>
        public VBListBoxItem(string ItemString, int ItemData) {
            this.ItemData = ItemData;
            this.ItemString = ItemString;
        }

        /// <summary>
        /// VBListBoxItem のインスタンスを作成します。
        /// </summary>
        /// <param name="ItemString">表示される文字列。</param>
        public VBListBoxItem(string ItemString) : this(ItemString, 0) { }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>表示される文字列。</returns>
        public override string ToString() {
            return this.ItemString;
        }


        // -------------------------------------------------------------------------------
        // ItemData プロパティ
        // -------------------------------------------------------------------------------
        private int m_ItemData = 0;

        /// <summary>
        /// リスト ボックス (ListBox) コントロールに含まれる各項目に、個別の番号を設定します。値の取得も可能です。
        /// </summary>
        public int ItemData {
            get {
                return m_ItemData;
            }
            set {
                if (ItemData != value) {
                    m_ItemData = value;
                    OnItemDataChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// ItemDataChanged イベントを発生させます。
        /// </summary>
        protected virtual void OnItemDataChanged(EventArgs e) {
            if (ItemDataChanged != null) ItemDataChanged(this, e);
        }

        /// <summary>
        /// ItemData プロパティの値が変更された場合に発生します。
        /// </summary>
        public event EventHandler ItemDataChanged;

        /// <summary>
        /// ItemData の値を既定値に設定します。
        /// </summary>
        internal void ResetItemData() {
            ItemData = 0;
        }

        /// <summary>
        /// ItemData の値が変更されているかを取得します。
        /// </summary>
        internal bool ShouldSerializeItemData() {
            return m_ItemData != 0;
        }

        // -------------------------------------------------------------------------------
        // ItemString プロパティ
        // -------------------------------------------------------------------------------
        private string m_ItemString = string.Empty;

        /// <summary>
        /// リスト ボックス (ListBox) コントロールに含まれる各項目で表示される文字列を設定します。値の取得も可能です。
        /// </summary>
        public string ItemString {
            get {
                return m_ItemString;
            }
            set {
                if (ItemString != value) {
                    m_ItemString = value;
                    OnItemStringChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// ItemStringChanged イベントを発生させます。
        /// </summary>
        protected virtual void OnItemStringChanged(EventArgs e) {
            if (ItemStringChanged != null) ItemStringChanged(this, e);
            if (Owner != null) {
                Owner.RefreshItems();
            }
        }

        /// <summary>
        /// ItemString プロパティの値が変更された場合に発生します。
        /// </summary>
        public event EventHandler ItemStringChanged;

        /// <summary>
        /// ItemString の値を既定値に設定します。
        /// </summary>
        internal void ResetItemString() {
            ItemString = string.Empty;
        }

        /// <summary>
        /// ItemString の値が変更されているかを取得します。
        /// </summary>
        internal bool ShouldSerializeItemString() {
            return m_ItemString != string.Empty;
        }
    }
}
