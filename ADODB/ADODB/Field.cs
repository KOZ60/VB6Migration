using System;
using System.Data;
using Microsoft.VisualBasic.CompilerServices;

namespace ADODB
{
    /// <summary>
    /// 共通のデータ型を持つデータの列を表します。
    /// </summary>
    public class Field 
    {

        Recordset _Owner;
        DataColumn _DataColumn;

        internal Field(Recordset owner, DataColumn dc)
        {
            _Owner = owner;
            _DataColumn = dc;
        }

        /// <summary>
        /// Field オブジェクトの名前を示す文字列型 (String) の値を取得します。
        /// </summary>
        public string Name
        {
            get { return _DataColumn.ColumnName; }
        }

        /// <summary>
        /// フィールド内のデータを取得または設定します。
        /// </summary>
        public object Value
        {
            get { return _Owner.CurrentRow[_DataColumn.Ordinal]; }
            set { _Owner.CurrentRow[_DataColumn.Ordinal] = value; }
        }

        /// <summary>
        /// フィールド内のデータを文字列に変換します。
        /// </summary>
        /// <returns>現在のオブジェクトを表す文字列。</returns>
        public override string ToString()
        {
            object objecValue = this.Value;
            if (objecValue == null || objecValue == DBNull.Value)
                return string.Empty;
            else
                return Conversions.ToString(objecValue); 
        }

        //int ActualSize { get; }
        //int Attributes { get; set; }
        //object DataFormat { get; set; }
        //int DefinedSize { get; set; }
        //byte NumericScale { get; set; }
        //object OriginalValue { get; }
        //byte Precision { get; set; }
        //Properties Properties { get; }
        //int Status { get; }
        //DataTypeEnum Type { get; set; }
        //object UnderlyingValue { get; }
        //void AppendChunk(object Data);
        //object GetChunk(int Length);

        /// <summary>
        /// Field オブジェクトを String 型に変換します。
        /// </summary>
        public static implicit operator string(Field field) {
            return field.ToString();
        }

        /// <summary>
        /// Field オブジェクトを decimal 型に変換します。
        /// </summary>
        public static implicit operator decimal(Field field) {
            return Conversions.ToDecimal(field.Value);
        }
    }
}
