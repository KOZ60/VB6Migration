using System;
using System.Data;
using Managed.OracleInProcServer.Core;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Managed.OracleInProcServer
{
    public class OraDynaset : IDisposable
    {
        DataTable _Table;
        dynOption _Options;
        int _RowPosition;
        OraFields _Fields;

        private OraDynaset(dynOption options) {
            _Options = options;
            _RowPosition = -1;

        }

        public OraDynaset(DataTable dt)
            : this(dynOption.ORADYN_READONLY) {
            var sc = new SchemaInformation[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++) {
                sc[i] = new SchemaInformation(dt.Columns[i]);
            }
            Init(dt, sc);
        }

        internal OraDynaset(OracleCommand cmd, dynOption options)
            : this(options) {
            using (var reader = cmd.ExecuteReader()) {
                var sc = UTL.CreateSchema(reader);
                var dt = UTL.CreateDataTable(sc);
                if (reader.HasRows) {
                    UTL.FillDataTable(reader, dt, sc, BlankStripOption);
                }
                this.SQL = cmd.CommandText;
                Init(dt, sc);
            }
        }

        private void Init(DataTable dt, SchemaInformation[] sc) {
            _Table = dt;
            _Fields = new OraFields(this, sc);
            if (dt.Rows.Count > 0 && MoveFirstOption) {
                MoveFirst();
            }
        }

        public void Dispose() {
            if (_Table != null) {
                _Table.Dispose();
                _Table = null;
            }
        }

        public DataTable Table {
            get {
                return _Table;
            }
        }

        public dynOption Options {
            get {
                return _Options;
            }
        }

        internal bool BlankStripOption {
            get {
                return !Options.HasFlag(dynOption.ORADYN_NO_BLANKSTRIP);
            }
        }

        internal bool MoveFirstOption {
            get {
                return !Options.HasFlag(dynOption.ORADYN_NO_MOVEFIRST);
            }
        }

        public string SQL { get; internal set; }

        protected virtual int RowPosition {
            get {
                return _RowPosition;
            }
            set {
                if (_RowPosition >= -1 && _RowPosition <= Table.Rows.Count) {
                    _RowPosition = value;
                }
            }
        }

        internal DataRow CurrentRow {
            get {
                int rowPosition = RowPosition;
                if (rowPosition >= 0 && rowPosition < Table.Rows.Count) {
                    return Table.Rows[rowPosition];
                }
                return null;
            }
        }

        public virtual int RecordCount {
            get {
                return _Table.Rows.Count;
            }
        }

        public void MoveFirst() {
            RowPosition = 0;
        }

        public void MoveLast() {
            RowPosition = RecordCount - 1;
        }

        public void MovePrevious() {
            RowPosition--;
        }

        public void MoveNext() {
            RowPosition++;
        }

        public bool BOF {
            get {
                return Table.Rows.Count == 0 || RowPosition < 0;
            }
        }

        public bool EOF {
            get {
                // 0 件なら true
                if (Table.Rows.Count == 0) {
                    return true;
                }
                // 読み込んだ件数以下なら False
                if (RowPosition < _Table.Rows.Count) {
                    return false;
                }
                // レコード件数以下なら false
                return RowPosition >= RecordCount;
            }
        }

        public OraFields Fields {
            get {
                return _Fields;
            }
        }

        public OraField this[int index] {
            get {
                return _Fields[index];
            }
        }

        public OraField this[string name] {
            get {
                return _Fields[name];
            }
        }

    }

}
