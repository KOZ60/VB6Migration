using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace DAO
{
    /// <summary>
    /// DAO.Database 互換クラス
    /// </summary>
    public class Database : ConnectionBase
    {
        private Workspace _Workspace;
        private OleDbConnection _Connection;
        private OleDbTransaction _Transaction;
        private Recordsets _Recordsets;

        internal Database(Workspace workspace, OleDbConnection con) {
            _Workspace = workspace;
            _Connection = con;
        }

        internal Workspace Workspace {
            get {
                return _Workspace;
            }
        }

        internal DBEngine DBEngine {
            get {
                return Workspace.DBEngine;
            }
        }

        /// <summary>
        /// Recordset オブジェクトのコレクションを返します。
        /// </summary>
        public Recordsets Recordsets {
            get {
                if (_Recordsets == null) {
                    _Recordsets = new Recordsets();
                }
                return _Recordsets;
            }
        }

        /// <summary>
        /// 新しい Recordset オブジェクトを作成し、Recordsets コレクションに追加します。
        /// </summary>
        /// <param name="SQL">新しい Recordset のレコードの取得元です。</param>
        /// <returns>Recordset</returns>
        public Recordset OpenRecordset(string SQL) {
            return OpenRecordset(SQL, RecordsetTypeEnum.dbOpenTable);
        }

        /// <summary>
        /// 新しい Recordset オブジェクトを作成し、Recordsets コレクションに追加します。
        /// </summary>
        /// <param name="SQL">新しい Recordset のレコードの取得元です。</param>
        /// <param name="Type">開く Recordset の型を示す RecordsetTypeEnum 定数。</param>
        /// <returns>Recordset</returns>
        public Recordset OpenRecordset(string SQL, RecordsetTypeEnum Type) {
            return OpenRecordset(SQL, Type, 0);
        }

        /// <summary>
        /// 新しい Recordset オブジェクトを作成し、Recordsets コレクションに追加します。
        /// </summary>
        /// <param name="SQL">新しい Recordset のレコードの取得元です。</param>
        /// <param name="Type">開く Recordset の型を示す RecordsetTypeEnum 定数。</param>
        /// <param name="Options">新しい Recordset の特性を指定する RecordsetOptionEnum 定数の組み合わせ。</param>
        /// <returns>Recordset</returns>
        public Recordset OpenRecordset(string SQL, RecordsetTypeEnum Type, RecordsetOptionEnum Options) {
            return OpenRecordset(SQL, Type, Options, LockTypeEnum.dbPessimistic);
        }

        /// <summary>
        /// 新しい Recordset オブジェクトを作成し、Recordsets コレクションに追加します。
        /// </summary>
        /// <param name="SQL">新しい Recordset のレコードの取得元です。</param>
        /// <param name="Type">開く Recordset の型を示す RecordsetTypeEnum 定数。</param>
        /// <param name="Options">新しい Recordset の特性を指定する RecordsetOptionEnum 定数の組み合わせ。</param>
        /// <param name="LockEdit">Recordset のロックを決定する LockTypeEnum 定数。</param>
        /// <returns>Recordset</returns>
        public Recordset OpenRecordset(string SQL, RecordsetTypeEnum Type, RecordsetOptionEnum Options, LockTypeEnum LockEdit) {
            var cmd = new OleDbCommand(SQL, _Connection);
            try {
                using (var adapter = new OleDbDataAdapter(cmd)) {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    var rec = new Recordset(this, cmd, dt, Type, Options, LockEdit);
                    Recordsets.Add(rec);
                    return rec;
                }
            } catch {
                // 異常終了したら OleDbCommand を Dispose
                if (cmd == null) {
                    cmd.Dispose();
                }
                throw;
            }
        }

        /// <summary>
        /// 指定したオブジェクトのアクション クエリまたは SQL ステートメントを実行します。
        /// </summary>
        /// <param name="Query">アクション クエリまたは SQL ステートメント</param>
        public void Execute(string Query) {
            Execute(Query, RecordsetOptionEnum.dbInconsistent);
        }

        /// <summary>
        /// 指定したオブジェクトのアクション クエリまたは SQL ステートメントを実行します。
        /// </summary>
        /// <param name="Query">アクション クエリまたは SQL ステートメント</param>
        /// <param name="Options">実行時のオプションを指定します。</param>
        public void Execute(string Query, RecordsetOptionEnum Options) {
            using (var cmd = _Connection.CreateCommand()) {
                cmd.CommandText = Query;
                cmd.Transaction = _Transaction;
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 指定したオブジェクトのアクション クエリまたは SQL ステートメントを実行します。
        /// </summary>
        /// <param name="SQL">アクション クエリまたは SQL ステートメント</param>
        /// <returns>影響を受けた行数。</returns>
        public int ExecuteSQL(string SQL) {
            using (var cmd = _Connection.CreateCommand()) {
                cmd.CommandText = SQL;
                cmd.Transaction = _Transaction;
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// このクラスによって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースとアンマネージ リソースの両方を解放する場合は true。アンマネージ リソースだけを解放する場合は false。</param>
        protected override void Dispose(bool disposing) {
            Workspace.RemoveDatabase(this);
            if (_Transaction != null) {
                _Transaction.Dispose();
                _Transaction = null;
            }
            if (_Connection != null) {
                _Connection.Dispose();
                _Connection = null;
            }
        }

        /// <summary>
        /// トランザクションを開始します。
        /// </summary>
        public override void BeginTrans() {
            if (_Transaction != null) {
                throw new InvalidOperationException("トランザクションは開始されています。");
            }
            _Transaction = _Connection.BeginTransaction();
        }

        /// <summary>
        /// トランザクションをコミットします。
        /// </summary>
        public override void CommitTrans() {
            if (_Transaction == null) {
                throw new InvalidOperationException("トランザクションは開始されていません。");
            }
            _Transaction.Commit();
            _Transaction.Dispose();
            _Transaction = null;
        }

        /// <summary>
        /// トランザクションをロールバックします。
        /// </summary>
        public override void Rollback() {
            if (_Transaction == null) {
                throw new InvalidOperationException("トランザクションは開始されていません。");
            }
            _Transaction.Rollback();
            _Transaction.Dispose();
            _Transaction = null;
        }
    }
}
