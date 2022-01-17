using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ADODB
{
    /// <summary>
    /// Recordset オブジェクトのすべての Field オブジェクトを格納します。
    /// </summary>
    public class Fields : IEnumerable, IEnumerable<Field>
    {

        Recordset _Owner;
        Dictionary<DataColumn, Field> _Dictionary;

        internal Fields(Recordset owner)
        {
            _Owner = owner;
            _Dictionary = new Dictionary<DataColumn, Field>();
            foreach (DataColumn dc in _Owner.Table.Columns) {
                var field = new Field(_Owner, dc);
                _Dictionary.Add(dc, field);
            }
        }

        /// <summary>
        /// コレクション内のオブジェクト数を示します。
        /// </summary>
        public int Count {
            get { return _Owner.Table.Columns.Count; }
        }

        /// <summary>
        /// コレクションの特定のメンバをインデックスで示します。
        /// </summary>
        /// <param name="index">コレクション内のインデックスを指定します。</param>
        /// <returns>Field オブジェクトを返します。</returns>
        public Field this[int index]
        {
            get {
                return _Dictionary[_Owner.Table.Columns[index]];
            }
        }

        /// <summary>
        /// コレクションの特定のメンバを名前で示します。
        /// </summary>
        /// <param name="name">コレクション内の名前を指定します。</param>
        /// <returns>Field オブジェクトを返します。</returns>
        public Field this[string name] {
            get {
                return _Dictionary[_Owner.Table.Columns[name]];
            }
        }

        /// <summary>
        /// コレクションにオブジェクトを追加します。コレクションが Fields である場合、コレクションにオブジェクトが追加される前に、新規 Field オブジェクトが作成されることがあります。
        /// </summary>
        /// <param name="name">新規 Field オブジェクトの名前を含む文字列型 (String) の値を指定します。fields に含まれるほかのオブジェクトとは異なる名前にする必要があります。</param>
        /// <param name="dataType">新規フィールドのデータ型を DataTypeEnum 値で指定します。</param>
        /// <param name="definedSize">新規フィールドの指定サイズを文字数またはバイト数で表す整数の値を指定します。</param>
        /// <param name="attrib">新規フィールドの属性を FieldAttributeEnum 値で指定します。</param>
        /// <param name="fieldValue">
        /// 新規フィールドの値を表すバリアント型 (Variant) の値を指定します。
        /// <para>互換性維持のためにありますが、サポートされません。</para>
        /// </param>
        public void Append(string name, DataTypeEnum dataType, int definedSize = 0, FieldAttributeEnum attrib = FieldAttributeEnum.adFldUnspecified, object fieldValue = null)
        {
            DataColumn dc = UTL.CreateDataColumn(name, dataType, definedSize, attrib, fieldValue);
            _Owner.Table.Columns.Add(dc);
            var field = new Field(_Owner, dc);
            _Dictionary.Add(dc, field);
        }

        IEnumerator<Field> IEnumerable<Field>.GetEnumerator() {
            return _Dictionary.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _Dictionary.Values.GetEnumerator();
        }

        
        //public void CancelUpdate()
        //{
        //    throw new NotImplementedException();
        //}

        //public void Delete(object Index)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Refresh()
        //{
        //    throw new NotImplementedException();
        //}

        //public void Resync(ResyncEnum ResyncValues = ResyncEnum.adResyncAllValues)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Update()
        //{
        //    throw new NotImplementedException();
        //}


    }
}
