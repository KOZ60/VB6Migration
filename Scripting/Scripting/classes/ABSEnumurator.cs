using System;
using IO = System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Scripting
{
    /// <summary>
    /// コレクションを列挙するための IEnumerator クラス
    /// </summary>
    /// <typeparam name="TItem">コレクションに格納された型を指定します。</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ABSEnumurator<TItem> : MarshalByRefObject, IEnumerator<TItem>
        where TItem : ABSClass
    {
        ABSCollcetion<TItem> m_Owner;
        int m_StartIndex;
        int m_CurrentIndex;
        TItem m_CurrentItem;

        internal ABSEnumurator(ABSCollcetion<TItem> owner)
        {
            m_Owner = owner;
            m_StartIndex = -1;
            this.Reset(); 
        }

        /// <summary>
        /// 列挙子の現在位置にあるコレクション内の要素を取得します。
        /// </summary>
        public TItem Current
        {
            get { return m_CurrentItem; }
        }

        object IEnumerator.Current
        {
            get { return m_CurrentItem; }
        }

        /// <summary>
        /// 列挙子をコレクションの次の要素に進めます。
        /// </summary>
        /// <returns>列挙子が次の要素に正常に進んだ場合は true。列挙子がコレクションの末尾を越えた場合は false。 </returns>
        public bool MoveNext()
        {
            ++m_CurrentIndex;

            if (m_CurrentIndex >= m_Owner.Count)
            {
                m_CurrentItem = default(TItem);
                return false;
            }

            m_CurrentItem = m_Owner[m_CurrentIndex];
            return true;
        }

        /// <summary>
        /// 列挙子を初期位置、つまりコレクションの最初の要素の前に設定します。
        /// </summary>
        public void Reset()
        {
            m_CurrentIndex = m_StartIndex;
            m_CurrentItem = default(TItem);
        }

        /// <summary>
        /// アンマネージ リソースの解放およびリセットに関連付けられているアプリケーション定義のタスクを実行します。 
        /// </summary>
        public void Dispose()
        {
            this.Reset();
            m_Owner = null;
        }
    }
}
