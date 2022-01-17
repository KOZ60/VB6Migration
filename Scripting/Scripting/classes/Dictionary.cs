using System;
using System.Collections;
using System.ComponentModel;
using NET = System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Scripting
{
    /// <summary>
    /// データ キーと組みとなる項目を格納するオブジェクトです。
    /// </summary>
    public class Dictionary : MarshalByRefObject
    {
        internal class NETDictionary : NET.Dictionary<string, object>
        {
            public NETDictionary(NET.IEqualityComparer<string> compare)
                : base(compare)
            {
            }
        }

        private NETDictionary m_Dictionary;
        private CompareMethod m_CompareMode;
        private static Exception OPERATION_EXCEPTION
            = new InvalidOperationException("ﾌﾟﾛｼｰｼﾞｬの呼び出し、または引数が不正です。");

        /// <summary>
        /// Dictionary オブジェクトのインスタンスを作成します。
        /// </summary>
        public Dictionary()
        {
            CreateDictionary(CompareMethod.BinaryCompare);
        }

        private void CreateDictionary(CompareMethod compareMode)
        {
            if (m_Dictionary != null && m_Dictionary.Count > 0)
                throw OPERATION_EXCEPTION;

            switch (compareMode)
            {
                case CompareMethod.BinaryCompare:
                    m_Dictionary = new NETDictionary(StringComparer.Ordinal);
                    break;

                case CompareMethod.TextCompare:
                    m_Dictionary = new NETDictionary(StringComparer.OrdinalIgnoreCase);
                    break;

                case CompareMethod.DatabaseCompare:
                    m_Dictionary = new NETDictionary(StringComparer.CurrentCulture);
                    break;

                default:
                    throw OPERATION_EXCEPTION;
            }
            m_CompareMode = compareMode;
        }

        /// <summary>
        /// Dictionary オブジェクトに文字列比較キーの比較モードを設定します。値の取得も可能です。
        /// </summary>
        /// <value>CompareMethod</value>
        public CompareMethod CompareMode
        {
            get { return m_CompareMode; }
            set
            {
                if (m_CompareMode != value)
                {
                    CreateDictionary(value);
                }
            }
        }

        /// <summary>
        /// Dictionary オブジェクトに含まれる項目の数を返します。値の取得のみ可能です。
        /// </summary>
        /// <value>Dictionary オブジェクトに含まれる項目の数</value>
        public int Count
        {
            get { return m_Dictionary.Count; }
        }

        /// <summary>
        /// 指定したキーのハッシュ コードを返します。
        /// </summary>
        /// <param name="Key">ハッシュコードを調べるキー</param>
        /// <returns>ハッシュコード</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public object HashVal(string Key)
        {
            return Key.GetHashCode();
        }

        /// <summary>
        /// Dictionary オブジェクトにある指定されたキーと関連付ける項目を設定します。
        /// </summary>
        /// <param name="Key">取得または追加する項目と関連付けるキーを指定します。</param>
        /// <returns></returns>
        public object this[string Key]
        {
            get { return m_Dictionary[Key]; }
            set { m_Dictionary[Key] = value; }
        }

        /// <summary>
        /// Dictionary オブジェクトにキーを設定します。値の取得も可能です。
        /// </summary>
        /// <remarks>object.Key(key) = newkey</remarks>
        public KeySetter Key
        {
            get { return new KeySetter(m_Dictionary); }
        }

        /// <summary>
        /// Dictionary オブジェクトにキーを設定するためのクラスです。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public class KeySetter
        {
            NETDictionary m_Owner;
            internal KeySetter(NETDictionary owner)
            {
                m_Owner = owner;
            }
            /// <summary>
            /// Dictionary オブジェクトにキーを設定します。
            /// </summary>
            /// <param name="Key">変更するキーを指定します。</param>
            /// <returns>引数 Key で指定した値と置き換える新しいキーを指定します。</returns>
            public string this[string Key]
            {
                set
                {
                    object item = m_Owner[Key];
                    m_Owner.Add(value, item);
                    m_Owner.Remove(Key);
                }
            }
        }

        /// <summary>
        /// Dictionary オブジェクトにキーと対の項目を追加します。
        /// </summary>
        /// <param name="Key">追加する引数 item と関連付けられた引数 key を指定します。</param>
        /// <param name="Item">追加する引数 key と関連付けられた引数 item を指定します。</param>
        public void Add(string Key, object Item)
        {
            m_Dictionary.Add(Key, Item);
        }

        /// <summary>
        /// 指定されたキーが Dictionary オブジェクトの中に存在するかどうかを取得します。
        /// </summary>
        /// <param name="Key">Dictionary オブジェクトの中から検索するキーの値を指定します。</param>
        /// <returns>指定されたキーが Dictionary オブジェクトの中に存在する場合は、真 (True) を返します。存在しない場合は、偽 (False) を返します。</returns>
        public bool Exists(string Key)
        {
            return m_Dictionary.ContainsKey(Key);
        }


        /// <summary>
        /// Dictionary オブジェクトを反復処理する列挙子を返します。
        /// </summary>
        /// <returns>IEnumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return m_Dictionary.GetEnumerator();
        }

        /// <summary>
        /// Dictionary オブジェクトのすべての項目に含まれる配列を返します。
        /// </summary>
        /// <returns>Dictionary オブジェクトのすべての項目に含まれる配列。</returns>
        public object[] Items()
        {
            object[] valueArray = new object[this.Count];
            m_Dictionary.Values.CopyTo(valueArray, this.Count);
            return valueArray;
        }

        /// <summary>
        /// Dictionary オブジェクトにあるすべてのキーに含まれる配列を返します。
        /// </summary>
        /// <returns>Dictionary オブジェクトにあるすべてのキーに含まれる配列</returns>
        public object Keys()
        {
            string[] keyArray = new string[this.Count];
            m_Dictionary.Keys.CopyTo(keyArray, this.Count);
            return keyArray;
        }

        /// <summary>
        /// Dictionary オブジェクトからキーと項目の組みを削除します。
        /// </summary>
        /// <param name="Key">Dictionary オブジェクトから削除するキーと項目の組みと関連付けられた引数 Key を指定します。</param>
        public void Remove(string Key)
        {
            m_Dictionary.Remove(Key);
        }

        /// <summary>
        /// Dictionary オブジェクトからすべてのキーと項目を削除します。
        /// </summary>
        public void RemoveAll()
        {
            m_Dictionary.Clear();
        }
    }
}