using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAO
{

    /// <summary>
    /// DAO.Fields 互換クラス
    /// </summary>
    public class Fields : IEnumerable<Field>
    {

        Recordset m_Owner;
        Field[] m_Array;

        internal Fields(Recordset owner) {
            m_Owner = owner;
            int nCount  =  owner.DataTable.Columns.Count;
            m_Array = new Field[nCount];
            for (int i = 0; i < nCount; i++) {
                DataColumn dc = owner.DataTable.Columns[i];
                m_Array[dc.Ordinal] = new Field(owner, dc);
            }
        }

        /// <summary>
        /// コレクション内のオブジェクト数を示します。
        /// </summary>
        public int Count {
            get { return m_Owner.DataTable.Columns.Count; }
        }

        /// <summary>
        /// インデックスを指定して Field オブジェクトを取得します。
        /// </summary>
        /// <param name="index">取得する Field オブジェクトのインデックス</param>
        /// <returns>Field オブジェクト</returns>
        public Field this[int index] {
            get {
                DataColumn dc = m_Owner.DataTable.Columns[index];
                return m_Array[dc.Ordinal];
            }
        }

        /// <summary>
        /// キーを指定して Field オブジェクトを取得します。
        /// </summary>
        /// <param name="name">取得する Field オブジェクトのキー</param>
        /// <returns>Field オブジェクト</returns>
        public Field this[string name] {
            get {
                DataColumn dc = m_Owner.DataTable.Columns[name];
                return m_Array[dc.Ordinal];
            }
        }

        IEnumerator<Field> IEnumerable<Field>.GetEnumerator() {
            foreach (var field in m_Array) {
                yield return field;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            foreach (var field in m_Array) {
                yield return field;
            }
        }
    }
}
