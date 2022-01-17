using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace ADODB
{
    /// <summary>
    /// データ ソースに対して実行する特定のコマンドを定義します。
    /// </summary>
    public class Command : IDisposable
    {
        private DbCommand _DbCommand;
        private Parameters _Parameters;

        /// <summary>
        /// Command オブジェクトのインスタンスを初期化します。
        /// </summary>
        public Command() {
            _Parameters = new Parameters();
            CommandType = CommandTypeEnum.adCmdText;
            CommandTimeout = 30;
        }

        /// <summary>
        /// コマンドを実行する接続を表す Connection オブジェクトを取得または設定します。
        /// </summary>
        public Connection ActiveConnection { get; set; }

        /// <summary>
        /// SQL ステートメント、テーブル名、またはストアド プロシージャ呼び出しからなる文字列型 (String) の値を取得または設定します。
        /// </summary>
        public string CommandText { get; set; }

        /// <summary>
        /// コマンドが実行されるまで待機する秒数を示す値を取得または設定します。デフォルトは 30 です。
        /// </summary>
        public int CommandTimeout { get; set; }

        /// <summary>
        /// Command オブジェクトの種類を示す値を取得または設定します。
        /// </summary>
        public CommandTypeEnum CommandType { get; set; }

        /// <summary>
        /// Command オブジェクトのすべての Parameter オブジェクトが格納されたコレクションを取得します。
        /// </summary>
        public Parameters Parameters {
            get {
                return _Parameters;
            }
        }

        /// <summary>
        /// Parameter オブジェクトを作成します。
        /// </summary>
        /// <param name="name">名前を指定します。</param>
        /// <param name="type">データ型を指定します。</param>
        /// <param name="direction">種類を指定します。</param>
        /// <param name="size">サイズを指定します。</param>
        /// <param name="value">値を指定します。</param>
        /// <returns>Parameter オブジェクト。</returns>
        public Parameter CreateParameter(string name = "", DataTypeEnum type = DataTypeEnum.adEmpty, ParameterDirectionEnum direction = ParameterDirectionEnum.adParamInput, int size = 0, object value = null) {
            var p = new Parameter();
            p.Name = name;
            p.Type = type;
            p.Direction = direction;
            if (size != 0) {
                p.Size = size;
            }
            p.Value = value;
            return p;
        }

        /// <summary>
        /// CommandText プロパティに設定された SQL を実行しクエリ―の結果を Recrodset で返します。
        /// </summary>
        /// <returns>クエリーの結果が格納される Recordset オブジェクトを表すオブジェクト変数。</returns>
        public Recordset Execute() {
            int recordsAffected;
            return Execute(out recordsAffected, null, ExecuteOptionEnum.adOptionUnspecified);
        }

        /// <summary>
        /// CommandText プロパティに設定された SQL を実行しクエリ―の結果を Recrodset で返します。
        /// </summary>
        /// <param name="parameters">SQL ステートメントとともに渡されるパラメータ値を示す配列。</param>
        /// <returns>クエリーの結果が格納される Recordset オブジェクトを表すオブジェクト変数。</returns>
        public Recordset Execute(object[] parameters) {
            int recordsAffected;
            return Execute(out recordsAffected, parameters, ExecuteOptionEnum.adOptionUnspecified);
        }

        /// <summary>
        /// CommandText プロパティに設定された SQL を実行しクエリ―の結果を Recrodset で返します。
        /// </summary>
        /// <param name="options">コマンドを実行する方法を示す ExecuteOptionEnum。</param>
        /// <returns>クエリーの結果が格納される Recordset オブジェクトを表すオブジェクト変数。</returns>
        public Recordset Execute(ExecuteOptionEnum options) {
            int recordsAffected;
            return Execute(out recordsAffected, null, options);
        }

        /// <summary>
        /// CommandText プロパティに設定された SQL を実行しクエリ―の結果を Recrodset で返します。
        /// </summary>
        /// <param name="parameters">SQL ステートメントとともに渡されるパラメータ値を示す配列。</param>
        /// <param name="options">コマンドを実行する方法を示す ExecuteOptionEnum。</param>
        /// <returns>クエリーの結果が格納される Recordset オブジェクトを表すオブジェクト変数。</returns>
        public Recordset Execute(object[] parameters, ExecuteOptionEnum options) {
            int recordsAffected;
            return Execute(out recordsAffected, parameters, options);
        }

        /// <summary>
        /// CommandText プロパティに設定された SQL を実行しクエリ―の結果を Recrodset で返します。
        /// </summary>
        /// <param name="recordsAffected">実行操作によって作用を受けたレコードの数をこの変数に返します。</param>
        /// <param name="parameters">SQL ステートメントとともに渡されるパラメータ値を示す配列。</param>
        /// <param name="options">コマンドを実行する方法を示す ExecuteOptionEnum。</param>
        /// <returns>クエリーの結果が格納される Recordset オブジェクトを表すオブジェクト変数。</returns>
        public Recordset Execute(out int recordsAffected, object[] parameters = null, ExecuteOptionEnum options = ExecuteOptionEnum.adOptionUnspecified) {
            if (_DbCommand == null) {
                _DbCommand = CreateDbCommand();
            }

            if (options != ExecuteOptionEnum.adOptionUnspecified) {
                throw new NotImplementedException("");
            }

            if (parameters != null) {
                for (int i = 0; i < parameters.Length; i++) {
                    _DbCommand.Parameters[i].Value = parameters[i];
                }
            }
            var lst = new List<RecordsetData>();
            using (var reader = _DbCommand.ExecuteReader()) {
                recordsAffected = reader.RecordsAffected;
                lst.Add(new RecordsetData(reader));
                while (reader.NextResult()) {
                    lst.Add(new RecordsetData(reader));
                }
                return new Recordset(lst);
            }
        }

        private DbCommand CreateDbCommand() {
            Connection con = ActiveConnection;
            if (con == null) {
                throw new ArgumentNullException("ActiveConnection が指定されていません。");
            }
            DbCommand cmd = con.DbProviderFactory.CreateCommand();
            cmd.CommandText = this.CommandText;
            cmd.CommandTimeout = this.CommandTimeout;
            cmd.CommandType = UTL.ToCommandType(this.CommandType);
            cmd.Connection = ActiveConnection.DbConnection;
            cmd.Transaction = ActiveConnection.DbTransaction;
            foreach (Parameter p in this.Parameters) {
                cmd.Parameters.Add(p.GetDbParameter(cmd));
            }
            return cmd;
        }

        /// <summary>
        /// このオブジェクトで使用しているリソースを破棄します。
        /// </summary>
        public void Dispose() {
            if (_DbCommand != null) {
                _DbCommand.Dispose();
                _DbCommand = null;
            }
        }
    }
}
