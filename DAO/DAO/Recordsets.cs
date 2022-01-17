using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    /// <summary>
    /// DAO.Recordsets 互換クラス
    /// </summary>
    public class Recordsets : IEnumerable<Recordset>
    {

        private class InnerItem : WeakReference, IEquatable<InnerItem>
        {
            string _ToString;
            int _HashCode;

            public InnerItem(Recordset rec) 
                : base(rec) {
                _ToString = rec.ToString();
                _HashCode = rec.GetHashCode();
            }

            public override string ToString() {
                return _ToString;
            }

            public override int GetHashCode() {
                return _HashCode;
            }

            public override bool Equals(object obj) {
                return EqualsInternal(obj as InnerItem);
            }

            private bool EqualsInternal(InnerItem other) {
                if (object.ReferenceEquals(this, (object)other)) {
                    return true;
                }
                return this._HashCode == other._HashCode;
            }

            bool IEquatable<InnerItem>.Equals(InnerItem other) {
                return EqualsInternal(other);
            }

            public new Recordset Target {
                get {
                    return (Recordset)base.Target;
                }
            }
        }

        private List<InnerItem> _List;

        internal Recordsets() {
            _List = new List<InnerItem>();
        }

        /// <summary>
        /// 実際に格納されている要素の数を取得します。
        /// </summary>
        public int Count {
            get {
                return _List.Count;
            }
        }

        /// <summary>
        /// インデックスを指定して Recordset オブジェクトを取得します。
        /// </summary>
        /// <param name="index">取得する Recordset オブジェクトのインデックス</param>
        /// <returns>Recordset オブジェクト</returns>
        public Recordset this[int index] {
            get {
                return _List[index].Target;
            }
        }

        internal void Add(Recordset rec) {
            _List.Add(new InnerItem(rec));
        }

        internal void Remove(Recordset rec) {
            _List.Remove(new InnerItem(rec));
        }

        /// <summary>
        /// コレクションを反復処理する列挙子を返します。
        /// </summary>
        /// <returns>コレクションを反復処理する列挙子。</returns>
        public IEnumerator<Recordset> GetEnumerator() {
            foreach (var item in _List) {
                yield return item.Target;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            foreach (var item in _List) {
                yield return item.Target;
            }
        }
    }
}
