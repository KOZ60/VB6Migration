using System;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Managed.OracleInProcServer
{
    public class OraDatabase : OraObject
    {
        public OraDatabase() {
            Connection = new OracleConnection();
        }

        public OracleConnection Connection { get; set; }

        public OracleTransaction Transaction { get; set; }

        public string ConnectionString {
            get {
                return Connection.ConnectionString;
            }
            set {
                Connection.ConnectionString = value;
            }
        }

        public void Open() {
            Connection.Open();
        }

        public void Open(string connectionString) {
            Connection.ConnectionString = connectionString;
            Connection.Open();
        }

        protected override void Dispose(bool disposing) {
            ResetTrans();
            if (Connection != null) {
                Connection.Dispose();
                Connection = null;
            }
        }

        public void BeginTrans() {
            Transaction = Connection.BeginTransaction();
        }

        public void CommitTrans() {
            Transaction.Commit();
            ResetTrans();
        }

        public void Rollback() {
            Transaction.Rollback();
            ResetTrans();
        }

        public void ResetTrans() {
            if (Transaction != null) {
                Transaction.Dispose();
                Transaction = null;
            }
        }

        public void Close() {
            Dispose();
        }

        public int CommandTimeout { get; set; } = 0;

        private OraParameters _Parameters;

        public OraParameters Parameters {
            get {
                if (_Parameters == null) {
                    _Parameters = new OraParameters();
                }
                return _Parameters;
            }
        }

        public OraParamArrays ParamArrays {
            get {
                return this.Parameters;
            }
        }

        static char[] sepChars = "\t\r\n();,=|' 　".ToCharArray(); // sql をワードに分解する単位

        private OracleCommand CreateDbCommand(string sqlStmt, bool autoBind) {
            var cmd = new OracleCommand();
            cmd.Connection = this.Connection;
            cmd.Transaction = this.Transaction;
            cmd.CommandText = sqlStmt;
            cmd.CommandTimeout = CommandTimeout;

            // ODP.NET 固有
            cmd.BindByName = true;
            cmd.InitialLOBFetchSize =  -1;
            cmd.InitialLONGFetchSize =  -1;
            cmd.FetchSize = 1024 * 512;

            if (autoBind) {
                // SQL をワード単位に分解
                string[] sqlWords = sqlStmt.Split(
                                                sepChars,
                                                StringSplitOptions.RemoveEmptyEntries);
                int nArrayBindCount = 0;  // 配列の最大数を調べる                          
                foreach (var p in Parameters) {
                    // AutoBind かつパラメータ名が SQL 中に存在する場合、バインドする
                    if (p.AutoBind && IsExistsParameter(sqlWords, p)) {
                        cmd.Parameters.Add(p.CreateDbParameter());
                        if (p.ArraySize > nArrayBindCount) {
                            nArrayBindCount = p.ArraySize;
                        }
                    }
                }
                if (nArrayBindCount != 0) {
                    cmd.ArrayBindCount = nArrayBindCount;
                }
            }
            return cmd;
        }

        private static bool IsExistsParameter(string[] sqlWords, OraParameter parameter) {
            string searchName = ":" + parameter.Name;
            for (int i = 0; i < sqlWords.Length; i++) {
                if (string.Compare(sqlWords[i], searchName, true) == 0) return true;
            }
            return false;
        }

        public int ExecuteSQL(string sqlStmt) {
            using (var cmd = CreateDbCommand(sqlStmt, true)) {
                return cmd.ExecuteNonQuery();
            }
        }

        public OraDynaset CreateDynaset(string sqlStmt, dynOption options) {
            using (var cmd = CreateDbCommand(sqlStmt, !options.HasFlag(dynOption.ORADYN_NO_AUTOBIND))) {
                var dyn = new OraDynaset(cmd, options);
                return dyn;
            }
        }
    }
}
