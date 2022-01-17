using System;
using System.Data;
using Microsoft.VisualBasic.CompilerServices;

namespace DAO
{
    /// <summary>
    /// 共通のデータ型を持つデータの列を表します。
    /// </summary>
    public class Field 
    {

        private Recordset _Recordset;
        private DataColumn _DataColumn;

        internal Field(Recordset owner, DataColumn dc) {
            _Recordset = owner;
            _DataColumn = dc;
        }

        internal Recordset Recordset {
            get {
                return _Recordset;
            }
        }

        internal DataColumn DataColumn {
            get {
                return _DataColumn;
            }
        }

        internal DataTable DataTable {
            get {
                return Recordset.DataTable;
            }
        }

        /// <summary>
        /// Field オブジェクトの名前を示す文字列型 (String) の値を取得します。
        /// </summary>
        public string Name {
            get { return DataColumn.ColumnName; }
        }

        /// <summary>
        /// フィールド内のデータを取得または設定します。
        /// </summary>
        public object Value {
            get { 
                return Recordset.CurrentRow[DataColumn.Ordinal]; 
            }
            set { 
                Recordset.CurrentRow[DataColumn.Ordinal] = value; 
            }
        }

        /// <summary>
        /// フィールド内のデータを文字列に変換します。
        /// </summary>
        /// <returns>現在のオブジェクトを表す文字列。</returns>
        public override string ToString() {
            object objecValue = this.Value;
            if (objecValue == null || objecValue == DBNull.Value) {
                return string.Empty;
            } else {
                return Conversions.ToString(objecValue);
            }
        }
    }
}
