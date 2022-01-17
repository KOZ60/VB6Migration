using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    /// <summary>
    /// ベース テーブルのレコード セット全体、またはコマンドの実行によって返された結果を表します。
    /// Recordset オブジェクトでは、常にレコードセット内の 1 つのレコードのみをカレント レコードとして参照します。
    /// </summary>
    public class Recordset : DaoObjectBase
    {
        private Database _Database;
        private OleDbCommand _Command;
        private DataTable _DataTable;
        private Fields _Fields;
        private int _RowPosition;
        private string _Sort;
        private DataRow _CurrentRow;

        internal Recordset(Database db, OleDbCommand cmd, DataTable dt, RecordsetTypeEnum Type, RecordsetOptionEnum Options, LockTypeEnum LockEdit) {
            _Database = db;
            _Command = cmd;
            _DataTable = dt;
            _Fields = new Fields(this);
            _Sort = string.Empty;
            _CurrentRow = null;
            AbsolutePosition = PositionEnum.dbPosUnknown;
            if (dt.Rows.Count > 0) {
                MoveFirst();
            }
        }

        /// <summary>
        /// このクラスによって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースとアンマネージ リソースの両方を解放する場合は true。アンマネージ リソースだけを解放する場合は false。</param>
        protected override void Dispose(bool disposing) {
            _Database.Recordsets.Remove(this);
            if (_DataTable != null) {
                _DataTable.Dispose();
                _DataTable = null;
            }
        }

        internal Database Database {
            get {
                return _Database;
            }
        }

        internal DataTable DataTable {
            get { return _DataTable; }
        }

        internal int RowPosition {
            get { return _RowPosition; }
            set { _RowPosition = value; }
        }

        internal DataRow CurrentRow {
            get {
                switch (AbsolutePosition) {
                    case PositionEnum.dbPosUnknown:
                    case PositionEnum.dbPosBOF:
                    case PositionEnum.dbPosEOF:
                        return _CurrentRow;
                    default:
                        return _DataTable.Rows[RowPosition];
                }
            }
        }

        /// <summary>
        /// Recordset オブジェクト内のカレント レコードの位置を示します。
        /// </summary>
        public PositionEnum AbsolutePosition {
            get {
                int pos = RowPosition + 1;

                if (pos >= 1 && pos <= RecordCount)
                    return (PositionEnum)pos;

                if (RowPosition == -1)
                    return PositionEnum.dbPosBOF;
                else if (RowPosition == RecordCount)
                    return PositionEnum.dbPosEOF;
                else
                    return PositionEnum.dbPosUnknown;
            }
            set {
                if ((int)value >= 1 && (int)value <= RecordCount)
                    RowPosition = (int)value - 1;
                else
                    switch (value) {
                        case PositionEnum.dbPosBOF:
                            RowPosition = -1;
                            break;

                        case PositionEnum.dbPosEOF:
                            RowPosition = RecordCount;
                            break;

                        case PositionEnum.dbPosUnknown:
                            RowPosition = -2;
                            break;
                    }
            }
        }

        /// <summary>
        /// カレント レコードの位置が Recordset オブジェクトの最初のレコードより前にあることを示します。
        /// </summary>
        /// <value>カレント レコードの位置が Recordset オブジェクトの最初のレコードより前にある場合は True,そうでない場合は False</value>
        public bool BOF {
            get {
                return (AbsolutePosition == PositionEnum.dbPosBOF) ||
                       (AbsolutePosition == PositionEnum.dbPosUnknown);
            }
        }
        /// <summary>
        /// カレント レコードの位置が Recordset オブジェクトの最後のレコードより後にあることを示します。
        /// </summary>
        /// <value>カレント レコードの位置が Recordset オブジェクトの最後のレコードより後にある場合は True,そうでない場合は False</value>
        public bool EOF {
            get {
                return (AbsolutePosition == PositionEnum.dbPosEOF) ||
                       (AbsolutePosition == PositionEnum.dbPosUnknown);
            }
        }

        /// <summary>
        /// カレント レコードの編集ステータスを示します。
        /// </summary>
        /// <value>EditModeEnum 値を返します。</value>
        public EditModeEnum EditMode {
            get {
                if (CurrentRow == null)
                    return EditModeEnum.dbEditNone;

                switch (CurrentRow.RowState) {
                    case DataRowState.Detached:
                    case DataRowState.Added:
                        return EditModeEnum.dbEditAdd;
                    case DataRowState.Modified:
                        return EditModeEnum.dbEditInProgress;
                    default:
                        return EditModeEnum.dbEditNone;
                }
            }
        }

        /// <summary>
        /// Recordset オブジェクトのすべての Field オブジェクトを格納します。
        /// </summary>
        public Fields Fields {
            get { return _Fields; }
        }

        /// <summary>
        /// Fields コレクションの特定のメンバをインデックスで示します。
        /// </summary>
        /// <param name="index">コレクション内のインデックスを指定します。</param>
        /// <returns>Field オブジェクトが返ります。</returns>
        public Field this[int index] {
            get { return _Fields[index]; }
        }

        /// <summary>
        /// Fields コレクションの特定のメンバを名前で示します。
        /// </summary>
        /// <param name="name">コレクション内のオブジェクト名を指定します。</param>
        /// <returns>Field オブジェクトが返ります。</returns>
        public Field this[string name] {
            get { return _Fields[name]; }
        }

        /// <summary>
        /// 更新可能な Recordset オブジェクトの新規レコードを作成します。
        /// </summary>
        public void AddNew() {
            if (EditMode != EditModeEnum.dbEditNone) {
                CancelUpdate();
            }

            _CurrentRow = _DataTable.NewRow();
            _DataTable.Rows.Add(_CurrentRow);
            AbsolutePosition = PositionEnum.dbPosUnknown;
        }

        /// <summary>
        /// Recordset オブジェクトのカレント行、または Record オブジェクトの Fields コレクションに加えた変更を保存します。
        /// </summary>
        public void Update() {
            // OleDbCommandBuilder による更新
            using (var adapter = new OleDbDataAdapter(_Command)) {
                using (var builder = new OleDbCommandBuilder(adapter)) {
                    adapter.Update(new DataRow[] { _CurrentRow });
                    _CurrentRow.AcceptChanges();
                }
            }
        }

        /// <summary>
        /// Recordset オブジェクトのカレント行を削除します。
        /// </summary>
        public void Delete() {
            // OleDbCommandBuilder による更新
            using (var adapter = new OleDbDataAdapter(_Command)) {
                using (var builder = new OleDbCommandBuilder(adapter)) {
                    _CurrentRow.Delete();
                    adapter.Update(new DataRow[] { _CurrentRow });
                    _CurrentRow.AcceptChanges();
                }
            }
        }

        /// <summary>
        /// Recordset オブジェクトに加えられた変更をキャンセルします。
        /// </summary>
        public void CancelUpdate() {
            if (_CurrentRow != null) {
                switch (_CurrentRow.RowState) {
                    case DataRowState.Added:
                    case DataRowState.Modified:
                        _CurrentRow.RejectChanges();
                        _CurrentRow.AcceptChanges();
                        break;

                }
            }
        }

        /// <summary>
        /// Recordset オブジェクトでカレント レコードの位置を移動します。
        /// </summary>
        /// <param name="NumRecords">カレント レコードの位置を移動するレコード数を指定する数値を指定します。</param>
        public void Move(int NumRecords) {
            RowPosition = RowPosition + NumRecords;
        }

        /// <summary>
        /// カレント レコードの位置を Recordset の最初のレコードに移動します。
        /// </summary>
        public void MoveFirst() {
            RowPosition = 0;
        }

        /// <summary>
        /// カレント レコードの位置を Recordset の最後のレコードに移動します。
        /// </summary>
        public void MoveLast() {
            RowPosition = RecordCount - 1;
        }

        /// <summary>
        /// カレント レコードの位置を 1 レコード前方 (Recordset の終端方向) に移動します。
        /// </summary>
        public void MoveNext() {
            RowPosition = RowPosition + 1;
        }

        /// <summary>
        /// カレント レコードの位置を 1 レコード後方 (Recordset の始端方向) に移動します。
        /// </summary>
        public void MovePrevious() {
            RowPosition = RowPosition - 1;
        }

        /// <summary>
        /// Recordset オブジェクト内のレコード数を示します。
        /// </summary>
        public int RecordCount {
            get { return _DataTable.Rows.Count; }
        }


        // Find 系メソッド 

        private bool _NoMatch = false;

        /// <summary>
        /// 指定された条件を満たす最初のレコードを検索し、そのレコードをカレント レコードにします。
        /// </summary>
        /// <param name="clause">レコードの検索に使用する文字列。</param>
        public virtual void FindFirst(string clause) {
            _NoMatch = !Select(clause, SelectPosition.Fisrt);
        }

        /// <summary>
        /// 指定された条件を満たす最後のレコードを検索し、そのレコードをカレント レコードにします。
        /// </summary>
        /// <param name="clause">レコードの検索に使用する文字列。</param>
        public virtual void FindLast(string clause) {
            _NoMatch = !Select(clause, SelectPosition.Last);
        }

        /// <summary>
        /// 指定された条件を満たす次のレコードを検索し、そのレコードをカレント レコードにします。
        /// </summary>
        /// <param name="clause">レコードの検索に使用する文字列。</param>
        public virtual void FindNext(string clause) {
            _NoMatch = !Select(clause, SelectPosition.Next);
        }

        /// <summary>
        /// 指定された条件を満たす前のレコードを検索し、そのレコードをカレント レコードにします。
        /// </summary>
        /// <param name="clause">レコードの検索に使用する文字列。</param>
        public virtual void FindPrevious(string clause) {
            _NoMatch = !Select(clause, SelectPosition.Previous);
        }

        /// <summary>
        /// Find メソッドの 1 つを使用して、特定のレコードが見つかったかどうかを示します。
        /// </summary>
        public virtual bool NoMatch {
            get { return _NoMatch; }
        }

        private enum SelectPosition
        {
            Fisrt,
            Last,
            Next,
            Previous,
        }

        private bool Select(string clause, SelectPosition selectPosition) {

            // 編集中断
            CancelUpdate();

            // 検索開始位置
            switch (selectPosition) {
                case SelectPosition.Fisrt:
                    RowPosition = 0;
                    break;
                case SelectPosition.Last:
                    RowPosition = this.RecordCount - 1;
                    break;
                case SelectPosition.Next:
                    RowPosition = RowPosition + 1;
                    break;
                case SelectPosition.Previous:
                    RowPosition = RowPosition - 1;
                    break;
                default:
                    RowPosition = RowPosition;
                    break;
            }

            // 検索方向で処理を分ける
            switch (selectPosition) {
                case SelectPosition.Fisrt:
                case SelectPosition.Next:
                    while (!this.EOF) {
                        if (IsMatch(clause)) {
                            return true;
                        }
                        RowPosition++;
                    }
                    break;

                case SelectPosition.Last:
                case SelectPosition.Previous:
                    while (!this.BOF) {
                        if (IsMatch(clause)) {
                            return true;
                        }
                        RowPosition--;
                    }
                    break;
            }
            return false;
        }

        // DataTable に格納して Select した結果で判断
        private bool IsMatch(string clause) {
            // もっとスマートな方法はないかと、System.Data.Select を解析したが、
            // 有用なクラス（ExpressionParser）が internal 宣言されている。
            // コピペして持ってきても良いがメンテナンスが面倒になるので断念。

            using (DataTable dt = DataTable.Clone()) {
                dt.Rows.Add(CurrentRow.ItemArray);
                DataRow[] rows = dt.Select(clause);
                return (rows.Length > 0);
            }
        }
    }

}
