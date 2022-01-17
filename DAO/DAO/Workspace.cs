using System;
using System.ComponentModel;
using System.Data.OleDb;
using System.Collections.Generic;

namespace DAO
{
    /// <summary>
    /// DAO.Workspace 互換クラス
    /// </summary>
    public class Workspace : ConnectionBase
    {
        private DBEngine _DBEngine;
        private string _Name;
        private List<Database> _Databases;
        private Database _Proxy;
        internal Workspace(DBEngine engine) : this(engine, null) { }

        internal Workspace(DBEngine engine, string name) {
            _DBEngine = engine;
            _Name = name;
            _Databases = new List<Database>();
            _Proxy = MultiCastProxy.CreateTransparentProxy(_Databases);
        }

        internal DBEngine DBEngine {
            get {
                return _DBEngine;
            }
        }

        /// <summary>
        /// このオブジェクトに関連付けられた名前を取得します。
        /// </summary>
        public string Name {
            get {
                return _Name;
            }
        }

        #region OpenDatabase

        // OpenDatabase の Options について
        // Jet 
        //      True : 排他モードでデータベースを開きます。
        //      False: (既定値)共有モードでデータベースを開きます。
        // ODBCDirect
        //      DriverPromptEnum の値をとります。

        #region Jet

        /// <summary>
        /// 指定したデータベースを開き、そのデータベースを表す Database オブジェクトへの参照を返します。
        /// </summary>
        /// <param name="Name">Jet データベースのファイル名</param>
        /// <returns>Database オブジェクト</returns>
        public Database OpenDatabase(string Name) {
            return OpenDatabase(Name, false);
        }

        /// <summary>
        /// 指定したデータベースを開き、そのデータベースを表す Database オブジェクトへの参照を返します。
        /// </summary>
        /// <param name="Name">Jet データベースのファイル名</param>
        /// <param name="Options">排他モードでデータベースを開くかどうかを指定します。</param>
        /// <returns>Database オブジェクト</returns>
        public Database OpenDatabase(string Name, bool Options) {
            return OpenDatabase(Name, Options, false);
        }

        /// <summary>
        /// 指定したデータベースを開き、そのデータベースを表す Database オブジェクトへの参照を返します。
        /// </summary>
        /// <param name="Name">Jet データベースのファイル名</param>
        /// <param name="Options">排他モードでデータベースを開くかどうかを指定します。</param>
        /// <param name="ReadOnly">データベースを読み取り専用で開くかどうかを指定します。</param>
        /// <returns>Database オブジェクト</returns>
        public Database OpenDatabase(string Name, bool Options, bool ReadOnly) {
            var builder = new OleDbConnectionStringBuilder();
            builder.Provider = DBEngine.Provider;
            builder.DataSource = Name;
            OleDbConnection con = new OleDbConnection();
            con.ConnectionString = builder.ToString();
            con.Open();
            var db = new Database(this, con);
            _Databases.Add(db);
            return db;
        }
        
        #endregion

        #region ODBC Direct(未実装)

        /// <summary>
        /// 指定したデータベースを開き、そのデータベースを表す Database オブジェクトへの参照を返します。
        /// </summary>
        /// <param name="Name"> ODBC データ ソースのデータ ソース名 （DSN） を表す文字列型 (String) を指定します。</param>
        /// <param name="Options">接続を確立するか、またはいつ確立するかをユーザーに表示するかを決定します。</param>
        /// <returns>Database オブジェクト</returns>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), Obsolete("未実装です。", true)]
        public Database OpenDatabase(string Name, DriverPromptEnum Options) {
            return OpenDatabase(Name, Options, false);
        }

        /// <summary>
        /// 指定したデータベースを開き、そのデータベースを表す Database オブジェクトへの参照を返します。
        /// </summary>
        /// <param name="Name"> ODBC データ ソースのデータ ソース名 （DSN） を表す文字列型 (String) を指定します。</param>
        /// <param name="Options">接続を確立するか、またはいつ確立するかをユーザーに表示するかを指定します。</param>
        /// <param name="ReadOnly">データベースを読み取り専用で開くかどうかを指定します。</param>
        /// <returns>Database オブジェクト</returns>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), Obsolete("未実装です。", true)]
        public Database OpenDatabase(string Name, DriverPromptEnum Options, bool ReadOnly) {
            return OpenDatabase(Name, Options, ReadOnly, null);
        }

        /// <summary>
        /// 指定したデータベースを開き、そのデータベースを表す Database オブジェクトへの参照を返します。
        /// </summary>
        /// <param name="Name"> ODBC データ ソースのデータ ソース名 （DSN） を表す文字列型 (String) を指定します。</param>
        /// <param name="Options">接続を確立するか、またはいつ確立するかをユーザーに表示するかを指定します。</param>
        /// <param name="ReadOnly">データベースを読み取り専用で開くかどうかを指定します。</param>
        /// <param name="Connect">パスワードを含むさまざまな接続情報を示すサブタイプ</param>
        /// <returns>Database オブジェクト</returns>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), Obsolete("未実装です。", true)]
        public Database OpenDatabase(string Name, DriverPromptEnum Options, bool ReadOnly, string Connect) {
            throw new NotImplementedException();
        }


        #endregion

        #endregion


        /// <summary>
        /// このクラスによって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースとアンマネージ リソースの両方を解放する場合は true。アンマネージ リソースだけを解放する場合は false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (_Proxy != null) {
                    _Proxy.Dispose();
                    _Proxy = null;
                }
                DBEngine.Workspaces.Remove(this);
            }
        }

        /// <summary>
        /// トランザクションを開始します。
        /// </summary>
        public override void BeginTrans() {
            _Proxy.BeginTrans();
        }

        /// <summary>
        /// トランザクションをコミットします。
        /// </summary>
        public override void CommitTrans() {
            _Proxy.CommitTrans();
        }

        /// <summary>
        /// トランザクションをロールバックします。
        /// </summary>
        public override void Rollback() {
            _Proxy.Rollback();
        }

        internal void RemoveDatabase(Database db) {
            if (_Databases.Contains(db)) {
                _Databases.Remove(db);
            }
        }
    }
}
