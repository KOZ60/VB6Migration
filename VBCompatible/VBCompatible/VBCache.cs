using System;
using System.Collections.Generic;

namespace VBCompatible
{
    /// <summary>
    /// キャッシュを作成するための基本クラスを提供します。
    /// </summary>
    /// <typeparam name="TKey">要素に対するキーの型を指定します。</typeparam>
    /// <typeparam name="TValue">要素の型を指定します。要素は IDisposable インターフェイスを実装する必要があります。</typeparam>
    public abstract class VBCache<TKey, TValue> : IDisposable
        where TValue : IDisposable
    {
        const int InitialCapacity = 256;

        private List<TKey> m_RemoveList;
        private Dictionary<TKey, TValue> m_Items;
        private int m_Capacity;
        private readonly object m_SyncRoot = new Object();

        /// <summary>
        /// VBCache のインスタンスを作成します。
        /// </summary>
        protected VBCache()
            : this(InitialCapacity) {
        }

        /// <summary>
        /// 容量を指定して VBCache のインスタンスを作成します。
        /// </summary>
        /// <param name="capacity">容量</param>
        protected VBCache(int capacity) {
            m_RemoveList = new List<TKey>(capacity);
            m_Items = new Dictionary<TKey, TValue>(capacity);
            m_Capacity = capacity;
        }

        /// <summary>
        /// キャッシュの容量を取得または設定します。
        /// </summary>
        public int Capacity {
            get { return m_Capacity; }
            set {
                if (value > 0) {
                    m_Capacity = value;
                }
            }
        }

        /// <summary>
        /// キャッシュに格納されている要素の数を取得します。
        /// </summary>
        public int Count {
            get {
                return m_Items.Count;
            }
        }

        /// <summary>
        /// キャッシュ へのアクセスを同期するために使用できるオブジェクトを取得します。
        /// </summary>
        public object SyncRoot {
            get {
                return m_SyncRoot;
            }
        }

        /// <summary>
        /// キーに対する要素を取得します。
        /// </summary>
        /// <param name="key">取得する要素のキー</param>
        /// <returns>要素を返します。</returns>
        public TValue this[TKey key] {
            get {
                TValue item;
                if (!m_Items.TryGetValue(key, out item)) {

                    item = CreateItem(key);
                    m_Items.Add(key, item);
                    m_RemoveList.Add(key);

                    // 容量を超える場合は先頭から削除
                    while (m_RemoveList.Count > Capacity) {
                        TKey deleteKey = m_RemoveList[0];
                        m_RemoveList.RemoveAt(0);
                        TValue tmp = m_Items[deleteKey];
                        tmp.Dispose();
                        m_Items.Remove(deleteKey);
                    }

                } else {
                    // 取得したら後ろに回す
                    m_RemoveList.Remove(key);
                    m_RemoveList.Add(key);
                }
                return item;
            }
        }

        /// <summary>
        /// キーに対する要素を作成します。
        /// </summary>
        /// <param name="key">要素に対するキー</param>
        /// <returns>要素</returns>
        /// <remarks>
        /// キーに対する要素がキャッシュに存在しない場合に呼ばれます。
        /// このクラスを継承するクラスでは、このメソッドをオーバーライドし、キーに対する要素を作成してください。
        /// </remarks>
        protected abstract TValue CreateItem(TKey key);

        public bool IsDisposed { get; private set; }

        protected virtual void Dispose(bool disposing) {
            if (!IsDisposed) {
                IsDisposed = true;
                if (disposing) {
                    if (m_RemoveList == null) {
                        m_RemoveList.Clear();
                        m_RemoveList = null;
                    }
                }

                if (m_Items != null) {
                    foreach (TValue item in m_Items.Values) {
                        item.Dispose();
                    }
                    m_Items.Clear();
                    m_Items = null;
                }
            }
        }

        ~VBCache() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
