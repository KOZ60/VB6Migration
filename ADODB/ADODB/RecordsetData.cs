using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADODB
{
    internal class RecordsetData
    {
        private int _RecordsAffected;
        private DataTable _DataTable;

        public int RecordsAffected {
            get {
                return _RecordsAffected;
            }
        }
        public DataTable DataTable {
            get {
                return _DataTable;
            }
        }

        public RecordsetData() : this(new DataTable()) { }

        public RecordsetData(DataTable dt) {
            _RecordsAffected = 0;
            _DataTable = dt;
        }

        public RecordsetData(DbDataReader reader) {
            _RecordsAffected = reader.RecordsAffected;
            int fieldCount = reader.FieldCount;
            _DataTable = new DataTable();
            DataTable.BeginInit();
            for (int i = 0; i < fieldCount; i++) {
                var dc = new DataColumn();
                dc.ColumnName = reader.GetName(i);
                dc.DataType = reader.GetFieldType(i);
                DataTable.Columns.Add(dc);
            }
            DataTable.EndInit();
            DataTable.BeginLoadData();
            while (reader.Read()) {
                object[] values = new object[fieldCount];
                reader.GetValues(values);
                DataTable.Rows.Add(values);
            }
            DataTable.EndLoadData();
        }
    }
}
