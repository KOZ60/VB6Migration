using System;
using System.Collections.Generic;
using System.Data;

namespace ADODB
{
    /// <summary>
    /// ベース テーブルのレコード セット全体、またはコマンドの実行によって返された結果を表します。
    /// Recordset オブジェクトでは、常にレコードセット内の 1 つのレコードのみをカレント レコードとして参照します。
    /// </summary>
    public class Recordset : IDisposable
    {
        private List<RecordsetData> _DataSet;
        private int _DataSetIndex;
        private DataTable _DataTable;
        private Fields _Fields;
        private int _RowPosition;
        private string _Sort;
        private DataRow _CurrentRow;

        /// <summary>
        /// Recordset オブジェクトのインスタンスを作成します。
        /// </summary>
        public Recordset()
        {
            Init(null, 0);
        }

        /// <summary>
        /// DataTable からRecordset オブジェクトのインスタンスを作成します。
        /// </summary>
        /// <param name="dt"></param>
        public Recordset(DataTable dt) {
            var lst = new List<RecordsetData>();
            lst.Add(new RecordsetData(dt));
            Init(lst, 0);
        }

        internal Recordset(List<RecordsetData> lst) {
            Init(lst, 0);
        }

        internal Recordset(List<RecordsetData> lst, int index) {
            Init(lst, index);
        }

        private void Init(List<RecordsetData> lst, int index) {
            if (lst == null) {
                lst = new List<RecordsetData>();
                lst.Add(new RecordsetData());
            }
            _DataSet = lst;
            _DataSetIndex = index;
            _DataTable = lst[index].DataTable;
            _Fields = new Fields(this);
            _Sort = string.Empty;
            _CurrentRow = null;
            AbsolutePosition = PositionEnum.adPosUnknown;
            if (_DataTable.Rows.Count > 0) {
                MoveFirst();
            }
        }

        /// <summary>
        /// カーソルを開きます。
        /// </summary>
        public void Open()
        {
            AbsolutePosition = PositionEnum.adPosUnknown;
        }
        /// <summary>
        /// カーソルを閉じます。
        /// </summary>
        public void Close()
        {
            this.Dispose();
        }

        /// <summary>
        /// 開いているオブジェクトおよびそれに従属するすべてのオブジェクトを閉じます。
        /// </summary>
        public void Dispose()
        {
            if (_DataTable != null) {
                _DataTable.Dispose();
                _DataTable = null;
            }
        }

        internal DataTable Table
        {
            get { return _DataTable; }
        }

        internal int RowPosition
        {
            get { return _RowPosition; }
            set { _RowPosition = value; }
        }

        internal DataRow CurrentRow
        {
            get 
            { 
                switch (AbsolutePosition)
                {
                    case PositionEnum.adPosUnknown:
                    case PositionEnum.adPosBOF:
                    case PositionEnum.adPosEOF:
                        return _CurrentRow;
                    default:
                        return _DataTable.Rows[RowPosition];
                }
            }
        }

        /// <summary>
        /// Recordset オブジェクト内のカレント レコードの位置を示します。
        /// </summary>
        public PositionEnum AbsolutePosition
        {
            get 
            {
                int pos = RowPosition + 1;

                if (pos >= 1 && pos <= RecordCount)
                    return (PositionEnum)pos;

                if (RowPosition == -1)
                    return PositionEnum.adPosBOF;
                else if (RowPosition == RecordCount)
                    return PositionEnum.adPosEOF;
                else
                    return PositionEnum.adPosUnknown;
            }
            set 
            {
                if ((int)value >= 1 && (int)value <= RecordCount)
                    RowPosition = (int)value - 1;
                else
                    switch (value)
                    {
                        case PositionEnum.adPosBOF:
                            RowPosition = -1;
                            break;

                        case PositionEnum.adPosEOF:
                            RowPosition = RecordCount;
                            break;

                        case PositionEnum.adPosUnknown:
                            RowPosition = -2;
                            break;
                    }
            }
        }

        /// <summary>
        /// カレント レコードの位置が Recordset オブジェクトの最初のレコードより前にあることを示します。
        /// </summary>
        /// <value>カレント レコードの位置が Recordset オブジェクトの最初のレコードより前にある場合は True,そうでない場合は False</value>
        public bool BOF
        {
            get
            {
                if (this.RecordCount == 0) {
                    return true;
                }
                return (AbsolutePosition == PositionEnum.adPosBOF) ||
                       (AbsolutePosition == PositionEnum.adPosUnknown);
            }
        }
        /// <summary>
        /// カレント レコードの位置が Recordset オブジェクトの最後のレコードより後にあることを示します。
        /// </summary>
        /// <value>カレント レコードの位置が Recordset オブジェクトの最後のレコードより後にある場合は True,そうでない場合は False</value>
        public bool EOF
        {
            get
            {
                if (this.RecordCount == 0) {
                    return true;
                }
                return (AbsolutePosition == PositionEnum.adPosEOF) ||
                       (AbsolutePosition == PositionEnum.adPosUnknown);
            }
        }

        /// <summary>
        /// カレント レコードの編集ステータスを示します。
        /// </summary>
        /// <value>EditModeEnum 値を返します。</value>
        public EditModeEnum EditMode
        {
            get 
            {
                if (CurrentRow == null)
                    return EditModeEnum.adEditNone;

                switch (CurrentRow.RowState)
                {
                    case DataRowState.Detached:
                    case DataRowState.Added:
                        return EditModeEnum.adEditAdd;
                    case DataRowState.Deleted:
                        return EditModeEnum.adEditDelete;
                    case DataRowState.Modified:
                        return EditModeEnum.adEditInProgress;
                    default:
                        return EditModeEnum.adEditNone;
                }
            }
        }

        /// <summary>
        /// Recordset オブジェクトのすべての Field オブジェクトを格納します。
        /// </summary>
        public Fields Fields
        {
            get { return _Fields; }
        }

        /// <summary>
        /// Fields コレクションの特定のメンバをインデックスで示します。
        /// </summary>
        /// <param name="index">コレクション内のインデックスを指定します。</param>
        /// <returns>Field オブジェクトが返ります。</returns>
        public Field this[int index]
        {
            get { return _Fields[index]; }
        }

        /// <summary>
        /// Fields コレクションの特定のメンバを名前で示します。
        /// </summary>
        /// <param name="name">コレクション内の名前を指定します。</param>
        /// <returns>Field オブジェクトが返ります。</returns>
        public Field this[string name] {
            get { return _Fields[name]; }
        }

        /// <summary>
        /// 更新可能な Recordset オブジェクトの新規レコードを作成します。
        /// </summary>
        public void AddNew()
        {
            if (EditMode != EditModeEnum.adEditNone)
            {
                Update();
            }

            _CurrentRow = _DataTable.NewRow();
            _CurrentRow.BeginEdit();
            AbsolutePosition = PositionEnum.adPosUnknown;
        }

        /// <summary>
        /// Recordset オブジェクトのカレント行、または Record オブジェクトの Fields コレクションに加えた変更を保存します。
        /// </summary>
        public void Update()
        {
            switch (EditMode)
            {
                case EditModeEnum.adEditAdd:
                    _CurrentRow.EndEdit();
                    _DataTable.Rows.Add(_CurrentRow);
                    _CurrentRow = null;
                    break;
                case EditModeEnum.adEditDelete:
                    CurrentRow.EndEdit();
                    break;
                case EditModeEnum.adEditInProgress:
                    break;
            }
            _DataTable.AcceptChanges();
        }

        /// <summary>
        /// Recordset オブジェクトでカレント レコードの位置を移動します。
        /// </summary>
        /// <param name="NumRecords">カレント レコードの位置を移動するレコード数を指定する数値を指定します。</param>
        public void Move(int NumRecords)
        {
            RowPosition = RowPosition + NumRecords;
        }

        /// <summary>
        /// カレント レコードの位置を Recordset の最初のレコードに移動します。
        /// </summary>
        public void MoveFirst()
        {
            RowPosition = 0;
        }

        /// <summary>
        /// カレント レコードの位置を Recordset の最後のレコードに移動します。
        /// </summary>
        public void MoveLast()
        {
            RowPosition = RecordCount - 1;
        }

        /// <summary>
        /// カレント レコードの位置を 1 レコード前方 (Recordset の終端方向) に移動します。
        /// </summary>
        public void MoveNext()
        {
            RowPosition = RowPosition + 1;
        }

        /// <summary>
        /// カレント レコードの位置を 1 レコード後方 (Recordset の始端方向) に移動します。
        /// </summary>
        public void MovePrevious()
        {
            RowPosition = RowPosition - 1;
        }

        /// <summary>
        /// Recordset オブジェクト内のレコード数を示します。
        /// </summary>
        public int RecordCount
        {
            get { return _DataTable.Rows.Count; }
        }

        /// <summary>
        /// Recordset をソートする 1 つ以上のフィールド名、および各フィールドのソート順序が昇順か降順かを示します。
        /// </summary>
        public string Sort
        {
            get { return _Sort; }
            set
            {
                _Sort = value;
                DataTable dt = _DataTable.Copy();
                DataView dv = new DataView(dt);
                dv.Sort = value;
                _DataTable.Rows.Clear();
                foreach (DataRowView drv in dv)
                {
                    _DataTable.ImportRow(drv.Row);
                }
            }
        }

        //public PositionEnum AbsolutePage
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        //public object ActiveCommand
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //public object ActiveConnection
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        //public object Bookmark
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        //public int CacheSize
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        //public CursorLocationEnum CursorLocation
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        //public CursorTypeEnum CursorType
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        //public string DataMember
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        //public object DataSource
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        //public object Filter
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        //public string Index
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        //public LockTypeEnum LockType
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        //public MarshalOptionsEnum MarshalOptions
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        //public int MaxRecords
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        //public int PageCount
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //public int PageSize
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        //public Properties Properties
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //public object Source
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        //public ObjectStateEnum State
        //{
        //    get { return m_State; }
        //    internal set { m_State = value; }
        //}

        //public RecordStatusEnum Status
        //{
        //    get { return m_Status; }
        //    internal set { m_Status = value; }
        //}

        //public bool StayInSync
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        //public Recordset _xClone()
        //{
        //    throw new NotImplementedException();
        //}

        //public void _xResync(AffectEnum AffectRecords = AffectEnum.adAffectAll)
        //{
        //    throw new NotImplementedException();
        //}

        //public void _xSave(string FileName = "", PersistFormatEnum PersistFormat = PersistFormatEnum.adPersistADTG)
        //{
        //    throw new NotImplementedException();
        //}


        ////public void AddNew(object FieldList = null, object Values = null)
        ////{
        ////    throw new NotImplementedException();
        ////}

        //public void Cancel()
        //{
        //    throw new NotImplementedException();
        //}

        //public void CancelBatch(AffectEnum AffectRecords = AffectEnum.adAffectAll)
        //{
        //    throw new NotImplementedException();
        //}

        //public void CancelUpdate()
        //{
        //    throw new NotImplementedException();
        //}

        //public Recordset Clone(LockTypeEnum LockType = LockTypeEnum.adLockUnspecified)
        //{
        //    throw new NotImplementedException();
        //}

        //public CompareEnum CompareBookmarks(object Bookmark1, object Bookmark2)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Delete(AffectEnum AffectRecords = AffectEnum.adAffectCurrent)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Find(string Criteria, int SkipRecords = 0, SearchDirectionEnum SearchDirection = SearchDirectionEnum.adSearchForward, object Start = null)
        //{
        //    throw new NotImplementedException();
        //}

        //public object get_Collect(object Index)
        //{
        //    throw new NotImplementedException();
        //}

        //public object GetRows(int Rows = -1, object Start = null, object Fields = null)
        //{
        //    throw new NotImplementedException();
        //}

        //public string GetString(StringFormatEnum StringFormat = StringFormatEnum.adClipString, int NumRows = -1, string ColumnDelimeter = "", string RowDelimeter = "", string NullExpr = "")
        //{
        //    throw new NotImplementedException();
        //}

        //public void let_ActiveConnection(object pvar)
        //{
        //    throw new NotImplementedException();
        //}

        //public void let_Source(string pvSource)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// コマンドを実行して得られた次のレコードセットを返します。
        /// </summary>
        /// <returns>次の Recordset オブジェクト。</returns>
        public Recordset NextRecordset() {
            int recordsAffected;
            return NextRecordset(out recordsAffected);
        }

        /// <summary>
        /// コマンドを実行して得られた次のレコードセットを返します。
        /// </summary>
        /// <param name="recordsAffected">実行操作によって作用を受けたレコードの数を返します。</param>
        /// <returns>次の Recordset オブジェクト。</returns>
        public Recordset NextRecordset(out int recordsAffected) {
            int index = _DataSetIndex + 1;
            if (index < _DataSet.Count) {
                recordsAffected = _DataSet[index].RecordsAffected;
                return new Recordset(_DataSet, index);
            } else {
                recordsAffected = 0;
                return null;
            }
        }

        //public void Requery(int Options = -1)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Resync(AffectEnum AffectRecords = AffectEnum.adAffectAll, ResyncEnum ResyncValues = ResyncEnum.adResyncAllValues)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Save(object Destination = null, PersistFormatEnum PersistFormat = PersistFormatEnum.adPersistADTG)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Seek(object KeyValues, SeekEnum SeekOption = SeekEnum.adSeekFirstEQ)
        //{
        //    throw new NotImplementedException();
        //}

        //public void set_Collect(object Index, object pvar)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool Supports(CursorOptionEnum CursorOptions)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Update(object Fields = null, object Values = null)
        //{
        //    throw new NotImplementedException();
        //}

        //public void UpdateBatch(AffectEnum AffectRecords = AffectEnum.adAffectAll)
        //{
        //    throw new NotImplementedException();
        //}

    }


}
