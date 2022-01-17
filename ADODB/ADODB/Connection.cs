using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace ADODB
{
    /// <summary>
    /// データ ソースへの開いている接続を表すオブジェクトです。
    /// </summary>
    public class Connection : IDisposable
    {
        private const string DEFAULT_PROVIDER_NAME = "System.Data.SqlClient";

        private DbProviderFactory _DbProviderFactory;
        private DbConnection _DbConnection;
        private List<DbTransaction> _DbTransactions;

        internal DbProviderFactory DbProviderFactory {
            get {
                return _DbProviderFactory;
            }
        }

        internal DbConnection DbConnection {
            get {
                return _DbConnection;
            }
        }

        internal DbTransaction DbTransaction {
            get {
                if (_DbTransactions == null || _DbTransactions.Count == 0) {
                    return null;
                }
                return _DbTransactions[_DbTransactions.Count - 1];
            }
        }

        /// <summary>
        /// Connection オブジェクトのインスタンスを初期化します。
        /// </summary>
        public Connection() {
            Provider = DEFAULT_PROVIDER_NAME;
        }

        /// <summary>
        /// ADO.NET のプロバイダ名を取得または設定します。
        /// </summary>
        public string Provider { get; set;}

        /// <summary>
        /// データソースに接続するための文字列を取得または設定します。
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// データソースへの接続を開きます。
        /// </summary>
        public void Open() {
            this.Open(this.ConnectionString);
        }

        /// <summary>
        /// データソースへの接続を開きます。
        /// </summary>
        /// <param name="connectionString">データソースに接続するための文字列。</param>
        public void Open(string connectionString) {
            _DbProviderFactory = DbProviderFactories.GetFactory(Provider);
            _DbConnection = _DbProviderFactory.CreateConnection();
            _DbConnection.ConnectionString = connectionString;
            _DbConnection.Open();
        }

        /// <summary>
        /// データソースへの接続を閉じます。
        /// </summary>
        public void Close() {
            this.Dispose();
        }

        /// <summary>
        /// トランザクションを開始します。
        /// </summary>
        public void BeginTrans() {
            var transaction = _DbConnection.BeginTransaction();
            if (transaction != null) {
                PushTransaction(transaction);
            }
        }

        /// <summary>
        /// トランザクションをコミットします。
        /// </summary>
        public void CommitTrans() {
            var transaction = PopTransaction();
            transaction.Commit();
            transaction.Dispose();
        }

        /// <summary>
        /// トランザクションをロールバックします。
        /// </summary>
        public void RollbackTrans() {
            var transaction = PopTransaction();
            transaction.Rollback();
            transaction.Dispose();
        }

        /// <summary>
        /// トランザクションをリセットします。
        /// </summary>
        public void ResetTrans() {
            if (_DbTransactions == null || _DbTransactions.Count == 0) {
                return;
            }
            while (_DbTransactions.Count > 0) {
                var transaction = PopTransaction();
                transaction.Dispose();
            }
        }

        internal void PushTransaction(DbTransaction transaction) {
            if (_DbTransactions == null) {
                _DbTransactions = new List<DbTransaction>();
            }
            _DbTransactions.Add(transaction);
        }

        internal IDbTransaction PopTransaction() {
            if (_DbTransactions == null || _DbTransactions.Count == 0) {
                return null;
            }
            int index = _DbTransactions.Count - 1;
            var transaction = _DbTransactions[index];
            _DbTransactions.RemoveAt(index);
            return transaction;
        }

        /// <summary>
        /// 使用しているリソースを解放します。
        /// </summary>
        public void Dispose() {
            // トランザクションがなくなるまで Rollback
            var transaction = PopTransaction();
            while (transaction != null) {
                transaction.Rollback();
                transaction.Dispose();
                transaction = PopTransaction();
            }
            if (_DbConnection != null) {
                _DbConnection.Dispose();
                _DbConnection = null;
            }
        }

        /// <summary>
        /// commandText に指定した SQL を実行しクエリ―の結果を Recrodset で返します。
        /// </summary>
        /// <param name="commandText">実行する SQL。</param>
        /// <returns>クエリ―の結果を示す Recrodset オブジェクト。</returns>
        public Recordset Execute(string commandText) {
            return Execute(commandText, ExecuteOptionEnum.adOptionUnspecified);
        }

        /// <summary>
        /// commandText に指定した SQL を実行しクエリ―の結果を Recrodset で返します。
        /// </summary>
        /// <param name="commandText">実行する SQL。</param>
        /// <param name="options">コマンドを実行する方法を示す ExecuteOptionEnum。</param>
        /// <returns>クエリ―の結果を示す Recrodset オブジェクト。</returns>
        public Recordset Execute(string commandText, ExecuteOptionEnum options) {
            int recordsAffected;
            return Execute(commandText, out recordsAffected, options);
        }

        /// <summary>
        /// commandText に指定した SQL を実行しクエリ―の結果を Recrodset で返します。
        /// </summary>
        /// <param name="commandText">実行する SQL。</param>
        /// <param name="recordsAffected"></param>
        /// <returns>クエリ―の結果を示す Recrodset オブジェクト。</returns>
        public Recordset Execute(string commandText, out int recordsAffected) {
            return Execute(commandText, out recordsAffected, ExecuteOptionEnum.adOptionUnspecified);
        }

        /// <summary>
        /// commandText に指定した SQL を実行しクエリ―の結果を Recrodset で返します。
        /// </summary>
        /// <param name="commandText">実行する SQL。</param>
        /// <param name="recordsAffected"></param>
        /// <param name="options">コマンドを実行する方法を示す ExecuteOptionEnum。</param>
        /// <returns>クエリ―の結果を示す Recrodset オブジェクト。</returns>
        public Recordset Execute(string commandText, out int recordsAffected, ExecuteOptionEnum options) {
            var cmd = new Command();
            cmd.ActiveConnection = this;
            cmd.CommandText = commandText;
            return cmd.Execute(out recordsAffected, null, options);
        }
    }
}
