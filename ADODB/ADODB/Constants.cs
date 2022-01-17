using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADODB
{
    /// <summary>
    /// Recordset 内のレコード ポインタの現在の位置を表します。
    /// </summary>
    public enum PositionEnum
    {
        /// <summary>
        /// カレント レコードのポインタが EOF にあることを表します (EOF プロパティが True です)。
        /// </summary>
        adPosEOF = -3,
        /// <summary>
        /// カレント レコードのポインタが BOF にあることを表します (BOF プロパティが True です)。
        /// </summary>
        adPosBOF = -2,
        /// <summary>
        /// Recordset が空である、現在の位置が不明、あるいはプロバイダが AbsolutePage プロパティまたは AbsolutePosition プロパティをサポートしていません。
        /// </summary>
        adPosUnknown = -1
    }

    /// <summary>
    /// カーソル サービスの場所を表します。
    /// </summary>
    public enum CursorLocationEnum
    {
        /// <summary>
        /// カーソル サービスを使いません。(この定数は現行バージョン用ではなく、以前のバージョンとの互換性を保つために装備されています。)
        /// </summary>
        adUseNone = 1,
        /// <summary>
        /// データ プロバイダ カーソルまたはドライバによって供給されるカーソルを使用します。
        /// </summary>
        adUseServer = 2,
        /// <summary>
        /// ローカル カーソル ライブラリより提供されたクライアント側カーソルを使います。
        /// </summary>
        adUseClient = 3,
        /// <summary>
        /// ローカル カーソル ライブラリより提供されたクライアント側カーソルを使います。
        /// </summary>
        adUseClientBatch = 3
    }

    /// <summary>
    /// Recordset オブジェクトが使うカーソルの種類を表します。
    /// </summary>
    public enum CursorTypeEnum
    {
        /// <summary>
        /// カーソルの種類を指定しません。
        /// </summary>
        adOpenUnspecified = -1,
        /// <summary>
        /// 前方専用カーソルを使います。
        /// </summary>
        adOpenForwardOnly = 0,
        /// <summary>
        /// キーセット カーソルを使います。
        /// </summary>
        adOpenKeyset = 1,
        /// <summary>
        /// 動的カーソルを使います。
        /// </summary>
        adOpenDynamic = 2,
        /// <summary>
        /// キーセット カーソルを開きます。
        /// </summary>
        adOpenStatic = 3,
    }

    /// <summary>
    /// レコードの編集ステータスを表します。
    /// </summary>
    public enum EditModeEnum
    {
        /// <summary>
        /// 進行中の編集操作がないことを示します。
        /// </summary>
        adEditNone = 0,
        /// <summary>
        /// カレント レコードのデータが変更されましたが、保存されていないことを示します。
        /// </summary>
        adEditInProgress = 1,
        /// <summary>
        /// AddNew メソッドが呼び出されていて、コピー バッファのカレント レコードが、データベースに保存されていない新規レコードであることを示します。
        /// </summary>
        adEditAdd = 2,
        /// <summary>
        /// カレント レコードが削除されたことを示します。
        /// </summary>
        adEditDelete = 4,
    }

    /// <summary>
    /// データ型を表します。
    /// </summary>
    public enum DataTypeEnum
    {
        /// <summary>
        /// 値を指定しません。
        /// </summary>
        adEmpty = 0,
        /// <summary>
        /// 2 バイトの符号付き整数を示します (DBTYPE_I2)。
        /// </summary>
        adSmallInt = 2,
        /// <summary>
        /// 4 バイトの符号付き整数を示します (DBTYPE_I4)。
        /// </summary>
        adInteger = 3,
        /// <summary>
        /// 単精度浮動小数点値を示します (DBTYPE_R4)。
        /// </summary>
        adSingle = 4,
        /// <summary>
        /// 倍精度浮動小数点値を示します (DBTYPE_R8)。
        /// </summary>
        adDouble = 5,
        /// <summary>
        /// 通貨値を示します (DBTYPE_CY)。
        /// </summary>
        adCurrency = 6,
        /// <summary>
        /// 日付値を示します (DBTYPE_DATE)。
        /// </summary>
        adDate = 7,
        /// <summary>
        /// Null で終了する Unicode 文字列を示します (DBTYPE_BSTR)。
        /// </summary>
        adBSTR = 8,
        /// <summary>
        /// COM オブジェクトの IDispatch インターフェイスへのポインタを示します。
        /// </summary>
        adIDispatch = 9,
        /// <summary>
        /// 32 ビット エラー コードを示します (DBTYPE_ERROR)。
        /// </summary>
        adError = 10,
        /// <summary>
        /// ブール値を示します (DBTYPE_BYTES)。
        /// </summary>
        adBoolean = 11,
        /// <summary>
        /// オートメーション バリアント型 (Variant) を示します。
        /// </summary>
        adVariant = 12,
        /// <summary>
        /// COM オブジェクトの IUnknown インターフェイスへのポインタを示します。
        /// </summary>
        adIUnknown = 13,
        /// <summary>
        /// 固定精度およびスケールの正確な数値を示します (DBTYPE_DECIMAL)。
        /// </summary>
        adDecimal = 14,
        /// <summary>
        /// 1 バイトの符号付き整数を示します (DBTYPE_I1)。
        /// </summary>
        adTinyInt = 16,
        /// <summary>
        /// 1 バイトの符号なし整数を示します (DBTYPE_I1)。
        /// </summary>
        adUnsignedTinyInt = 17,
        /// <summary>
        /// 2 バイトの符号なし整数を示します (DBTYPE_I2)。
        /// </summary>
        adUnsignedSmallInt = 18,
        /// <summary>
        /// 4 バイトの符号なし整数を示します (DBTYPE_I4)。
        /// </summary>
        adUnsignedInt = 19,
        /// <summary>
        /// 8 バイトの符号付き整数を示します (DBTYPE_I8)。
        /// </summary>
        adBigInt = 20,
        /// <summary>
        /// 8 バイトの符号なし整数を示します (DBTYPE_I8)。
        /// </summary>
        adUnsignedBigInt = 21,
        /// <summary>
        /// 1601 年 1 月 1 日からの時間を示す 64 ビット値を 100 ナノ秒単位で示します (DBTYPE_FILETIME)。
        /// </summary>
        adFileTime = 64,
        /// <summary>
        /// 固有のグローバル識別子 (GUID) を示します (DBTYPE_GUID)。
        /// </summary>
        adGUID = 72,
        /// <summary>
        /// バイナリ値を示します (DBTYPE_BYTES)。
        /// </summary>
        adBinary = 128,
        /// <summary>
        /// 文字列値を示します (DBTYPE_STR)。
        /// </summary>
        adChar = 129,
        /// <summary>
        /// Null で終了する Unicode 文字列を示します (DBTYPE_WSTR)。
        /// </summary>
        adWChar = 130,
        /// <summary>
        /// 固定精度およびスケールの正確な数値を示します (DBTYPE_NUMERIC)。
        /// </summary>
        adNumeric = 131,
        /// <summary>
        /// ユーザー定義の変数を示します (DBTYPE_UDT)。
        /// </summary>
        adUserDefined = 132,
        /// <summary>
        /// 日付値 (yyyymmdd) を示します (DBTYPE_DBDATE)。
        /// </summary>
        adDBDate = 133,
        /// <summary>
        /// 時刻値 (hhmmss) を示します (DBTYPE_DBTIME)。
        /// </summary>
        adDBTime = 134,
        /// <summary>
        /// 日付/時刻スタンプ (yyyymmddhhmmss および 10 億分の 1 桁までの分数) を示します (DBTYPE_DBTIMESTAMP)。
        /// </summary>
        adDBTimeStamp = 135,
        /// <summary>
        /// 子行セットの行を識別する 4 バイト チャプタ値を示します (DBTYPE_HCHAPTER)。
        /// </summary>
        adChapter = 136,
        /// <summary>
        /// オートメーション PROPVARIANT を示します (DBTYPE_PROP_VARIANT)。
        /// </summary>
        adPropVariant = 138,
        /// <summary>
        /// 数値を示します (Parameter オブジェクトのみ)。
        /// </summary>
        adVarNumeric = 139,
        /// <summary>
        /// 文字列値を示します (Parameter オブジェクトのみ)。
        /// </summary>
        adVarChar = 200,
        /// <summary>
        /// 長文字列の値を示します (Parameter オブジェクトのみ)。
        /// </summary>
        adLongVarChar = 201,
        /// <summary>
        /// Null で終了する Unicode 文字列を示します (Parameter オブジェクトのみ)。
        /// </summary>
        adVarWChar = 202,
        /// <summary>
        /// 長文字列の値を示します (Parameter オブジェクトのみ)。
        /// </summary>
        adLongVarWChar = 203,
        /// <summary>
        /// バイナリ値を示します (Parameter オブジェクトのみ)。
        /// </summary>
        adVarBinary = 204,
        /// <summary>
        /// 長バイナリ値を示します (Parameter オブジェクトのみ)。
        /// </summary>
        adLongVarBinary = 205,
        /// <summary>
        /// 常に別のデータ型定数と組み合わされ、そのデータ型の配列を示すフラグ値です。 
        /// </summary>
        adArray = 8192,
    }

    /// <summary>
    /// 1 つ以上の Field オブジェクトの属性を表します。
    /// </summary>
    public enum FieldAttributeEnum
    {
        /// <summary>
        /// プロバイダがフィールド属性を指定しないことを示します。
        /// </summary>
        adFldUnspecified = -1,
        /// <summary>
        /// フィールドが従属であることを示します。フィールド値は、レコード全体のデータ ソースから取得されず、明示的にアクセスした場合にのみ取得されます。
        /// </summary>
        adFldMayDefer = 2,
        /// <summary>
        /// フィールドに書き込めることを示します。
        /// </summary>
        adFldUpdatable = 4,
        /// <summary>
        /// プロバイダがフィールドへの書き込み権限を決定できないことを示します。
        /// </summary>
        adFldUnknownUpdatable = 8,
        /// <summary>
        /// フィールドが固定長データを含むことを示します。
        /// </summary>
        adFldFixed = 16,
        /// <summary>
        /// フィールドに Null 値を指定できることを示します。
        /// </summary>
        adFldIsNullable = 32,
        /// <summary>
        /// フィールドから Null 値を読み取れることを示します。
        /// </summary>
        adFldMayBeNull = 64,
        /// <summary>
        /// フィールドが長バイナリ型のフィールドであることを示します。また、AppendChunk および GetChunk メソッドが使用できることを示します。
        /// </summary>
        adFldLong = 128,
        /// <summary>
        /// フィールドが書き込み禁止の永続化された行識別子を含み、行 (レコード番号、一意識別子など) を識別する以外に有効な値は持たないことを示します。
        /// </summary>
        adFldRowID = 256,
        /// <summary>
        /// フィールドが更新を記録するための時刻または日付スタンプを含むことを示します。
        /// </summary>
        adFldRowVersion = 512,
        /// <summary>
        /// プロバイダでフィールド値がキャッシュされ、その後の読み取りはキャッシュから行われることを示します。
        /// </summary>
        adFldCacheDeferred = 4096,
        /// <summary>
        /// フィールドがチャプタ値を含み、親フィールドに関連付けられた特定の子レコードセットを指定していることを示します。通常、チャプタ フィールドはデータ シェイプやフィルタ用に使います。
        /// </summary>
        adFldIsChapter = 8192,
        /// <summary>
        /// 負のスケール値をサポートする列の数値を、フィールドが表していることを示します。スケールは、NumericScale プロパティで指定します。
        /// </summary>
        adFldNegativeScale = 16384,
        /// <summary>
        /// フィールドは、列の主キーの全部または一部を指定することを示しています。
        /// </summary>
        adFldKeyColumn = 32768,
        /// <summary>
        /// フィールドが、レコードが示すデータ ストアのリソースを指定する URL を含むことを示します。 
        /// </summary>
        adFldIsRowURL = 65536,
        /// <summary>
        /// フィールドは、レコードが示すリソースのデフォルトのストリームが含まれていることを示しています。
        /// </summary>
        adFldIsDefaultStream = 131072,
        /// <summary>
        /// レコードが示すリソースが、テキスト ファイルなどの単純なリソースではなく、フォルダなどのようにほかのリソースのコレクションであることを、このフィールドが指定していることを示します。
        /// </summary>
        adFldIsCollection = 262144,
    }

    /// <summary>
    /// Resync の呼び出しによって基になる値が上書きされるかどうかを表します。
    /// </summary>
    public enum ResyncEnum
    {
        /// <summary>
        /// データは上書きされず、保留中の更新は取り消されません。
        /// </summary>
        adResyncUnderlyingValues = 1,
        /// <summary>
        /// 既定値です。データは上書きされ、保留中の更新は取り消されます。
        /// </summary>
        adResyncAllValues = 2,
    }

    /// <summary>
    /// 編集時にレコードに適用されるロックの種類を表します。
    /// </summary>
    public enum LockTypeEnum
    {
        /// <summary>
        /// ロックの種類を指定しません。クローンの場合、複製元と同じロックの種類が適用されます。
        /// </summary>
        adLockUnspecified = -1,
        /// <summary>
        /// 読み取り専用のレコードを示します。データの変更はできません。
        /// </summary>
        adLockReadOnly = 1,
        /// <summary>
        /// レコード単位の排他的ロックを示します。プロバイダは、レコードを確実に編集するための措置を行います。通常は、編集直後のデータ ソースでレコードをロックします。
        /// </summary>
        adLockPessimistic = 2,
        /// <summary>
        /// レコード単位の共有的ロックを示します。Update メソッドを呼び出した場合にのみ、プロバイダは共有的ロックを使ってレコードをロックします。
        /// </summary>
        adLockOptimistic = 3,
        /// <summary>
        /// 共有的バッチ更新を示します。バッチ更新モードの場合にのみ指定できます。
        /// </summary>
        adLockBatchOptimistic = 4,
    }

    /// <summary>
    /// サーバーにどのレコードが返されるかを表します。
    /// </summary>
    public enum MarshalOptionsEnum
    {
        /// <summary>
        /// 既定値です。すべての行をサーバーに返します。
        /// </summary>
        adMarshalAll = 0,
        /// <summary>
        /// 変更した行のみサーバーに返します。
        /// </summary>
        adMarshalModifiedOnly = 1,
    }

    /// <summary>
    /// 操作の対象になるレコードを表します。
    /// </summary>
    public enum AffectEnum
    {
        /// <summary>
        /// カレント レコードにだけ反映されます。
        /// </summary>
        adAffectCurrent = 1,
        /// <summary>
        /// 現在の Filter プロパティ設定を満たすレコードにだけ反映されます。
        /// </summary>
        adAffectGroup = 2,
        /// <summary>
        /// Recordset に適用される Filter がない場合、すべてのレコードが対象です。 
        /// </summary>
        adAffectAll = 3,
        /// <summary>
        /// 現在適用されている Filter で非表示になっているものを含めて、Recordset のすべての兄弟チャプタに全レコードが反映されます。
        /// </summary>
        adAffectAllChapters = 4,
    }

    /// <summary>
    /// Recordset を保存するときの形式を表します。
    /// </summary>
    public enum PersistFormatEnum
    {
        /// <summary>
        /// Microsoft Advanced Data TableGram (ADTG) フォーマットであることを表します。
        /// </summary>
        adPersistADTG = 0,
        /// <summary>
        /// Extensible Markup Language (XML) フォーマットであることを表します。
        /// </summary>
        adPersistXML = 1,
    }

    /// <summary>
    /// ブックマークで表された 2 つのレコードの相対位置を表します。
    /// </summary>
    public enum CompareEnum
    {
        /// <summary>
        /// 最初のブックマークが 2 番目のブックマークの前になることを表します。
        /// </summary>
        adCompareLessThan = 0,
        /// <summary>
        /// ブックマークが等しいことを表します。
        /// </summary>
        adCompareEqual = 1,
        /// <summary>
        /// 最初のブックマークが 2 番目のブックマークの後になることを表します。
        /// </summary>
        adCompareGreaterThan = 2,
        /// <summary>
        /// これらのブックマークは異なっており、順位がないことを表します。
        /// </summary>
        adCompareNotEqual = 3,
        /// <summary>
        /// ブックマークを比較できないことを表します。
        /// </summary>
        adCompareNotComparable = 4,
    }

    /// <summary>
    /// Recordset 内のレコードの検索方向を表します。
    /// </summary>
    public enum SearchDirectionEnum
    {
        /// <summary>
        /// 後方検索をし、Recordset の先頭で終了します。一致するレコードが見つからない場合、レコード ポインタは BOF に移動します。
        /// </summary>
        adSearchBackward = -1,
        /// <summary>
        /// 前方検索をし、Recordset の末尾で終了します。一致するレコードが見つからない場合、レコード ポインタは EOF に移動します。
        /// </summary>
        adSearchForward = 1,
    }

    /// <summary>
    /// 文字列として Recordset を取得するときの形式を表します。
    /// </summary>
    public enum StringFormatEnum
    {
        /// <summary>
        /// 行が RowDelimiter によって、列が ColumnDelimiter によって、Null 値が NullExpr によって区切られます。
        /// GetString メソッドのこれら 3 つのパラメータは、adClipString の StringFormat とのみ併用できます。
        /// </summary>
        adClipString = 2,
    }

    /// <summary>
    /// 実行する Seek オブジェクトの種類を表します。
    /// </summary>
    public enum SeekEnum
    {
        /// <summary>
        /// KeyValues と一致する最初のキーを検索します。
        /// </summary>
        adSeekFirstEQ = 1,
        /// <summary>
        /// KeyValues と一致する最後のキーを検索します。
        /// </summary>
        adSeekLastEQ = 2,
        /// <summary>
        /// KeyValues と一致するキー、またはその直後のキーのいずれかを検索します。
        /// </summary>
        adSeekAfterEQ = 4,
        /// <summary>
        /// KeyValues と一致するキーの直後のキーを検索します。
        /// </summary>
        adSeekAfter = 8,
        /// <summary>
        /// KeyValues と一致するキー、またはその直前のキーのいずれかを検索します。
        /// </summary>
        adSeekBeforeEQ = 16,
        /// <summary>
        /// KeyValues と一致するキーの直前のキーを検索します。
        /// </summary>
        adSeekBefore = 32,
    }

    /// <summary>
    /// Supports メソッドがテストする機能を表します。
    /// </summary>
    public enum CursorOptionEnum
    {
        /// <summary>
        /// 保留中のすべての変更を実行せずに、新たなレコードを格納するか、または次の格納位置を変更します。
        /// </summary>
        adHoldRecords = 256,
        /// <summary>
        /// ブックマークを使用せずにカレント レコードの位置を後方に移動する MoveFirst、MovePrevious メソッドおよび Move、または GetRows メソッドをサポートします。
        /// </summary>
        adMovePrevious = 512,
        /// <summary>
        /// 特定のレコードへのアクセスを確保する Bookmark プロパティをサポートします。
        /// </summary>
        adBookmark = 8192,
        /// <summary>
        /// AbsolutePosition プロパティと AbsolutePage プロパティをサポートします。
        /// </summary>
        adApproxPosition = 16384,
        /// <summary>
        /// 複数の変更をグループとしてプロバイダに送信するバッチ更新 (UpdateBatch メソッドと CancelBatch メソッド) をサポートします。
        /// </summary>
        adUpdateBatch = 65536,
        /// <summary>
        /// 基になるデータベースのカーソルにある可視データを更新する Resync メソッドをサポートします。
        /// </summary>
        adResync = 131072,
        /// <summary>
        /// 基になるデータ プロバイダが通知をサポートしていることを表します (これにより Recordset イベントのサポートの有無が決まります)。
        /// </summary>
        adNotify = 262144,
        /// <summary>
        /// Recordset 内の行の位置を確認する Find メソッドをサポートします。
        /// </summary>
        adFind = 524288,
        /// <summary>
        /// Recordset 内の行に割り当てる Seek メソッドをサポートします。
        /// </summary>
        adSeek = 4194304,
        /// <summary>
        /// インデックスに名前を付ける Index プロパティをサポートします。
        /// </summary>
        adIndex = 8388608,
        /// <summary>
        /// 新規レコードを追加する AddNew メソッドをサポートします。
        /// </summary>
        adAddNew = 16778240,
        /// <summary>
        /// レコードを削除する Delete メソッドをサポートします。
        /// </summary>
        adDelete = 16779264,
        /// <summary>
        /// 既存のデータを変更する Update メソッドをサポートします。
        /// </summary>
        adUpdate = 16809984,
    }

    /// <summary>
    /// オブジェクトを開いているか閉じているか、データ ソースに接続中か、コマンドを実行中か、またはデータを取得中かどうかを表します。
    /// </summary>
    public enum ObjectStateEnum
    {
        /// <summary>
        /// オブジェクトが閉じていることを示します。
        /// </summary>
        adStateClosed = 0,	    
        /// <summary>
        /// オブジェクトが開いていることを示します。
        /// </summary>
        adStateOpen = 1,
        /// <summary>
        /// オブジェクトが接続していることを示します。
        /// </summary>
        adStateConnecting = 2,
        /// <summary>
        /// オブジェクトがコマンドを実行中であることを示します。
        /// </summary>
        adStateExecuting = 4,
        /// <summary>
        /// オブジェクトの行が取得されていることを示します。
        /// </summary>
        adStateFetching = 8,
    }

    /// <summary>
    /// バッチ更新または別の一括操作に関するレコードのステータスを表します。
    /// </summary>
    public enum RecordStatusEnum
    {
        /// <summary>
        /// 操作が取り消されたため、レコードが保存されなかったことを表します。
        /// </summary>
        adRecCanceled = 0x100,
        /// <summary>
        /// 既存のレコードがロックされたため、新しいレコードが保存されなかったことを表します。
        /// </summary>
        adRecCantRelease = 0x400,
        /// <summary>
        /// 共有的ロックの同時実行が使用中のため、レコードが保存されなかったことを表します。
        /// </summary>
        adRecConcurrencyViolation = 0x800,
        /// <summary>
        /// レコードは既にデータ ソースから削除されていることを表します。
        /// </summary>
        adRecDBDeleted = 0x40000,
        /// <summary>
        /// レコードが削除されたことを表します。
        /// </summary>
        adRecDeleted = 0x4,
        /// <summary>
        /// ユーザーが整合性の制約に違反したため、レコードが保存されなかったことを表します。
        /// </summary>
        adRecIntegrityViolation = 0x1000,
        /// <summary>
        /// ブックマークが無効なため、レコードが保存されなかったことを表します。
        /// </summary>
        adRecInvalid = 0x10,
        /// <summary>
        /// 保留中の変更が多すぎたため、レコードが保存されなかったことを表します。
        /// </summary>
        adRecMaxChangesExceeded = 0x2000,
        /// <summary>
        /// レコードが変更されたことを表します。
        /// </summary>
        adRecModified = 0x2,
        /// <summary>
        /// 複数のレコードに影響があったので、レコードが保存されなかったことを表します。
        /// </summary>
        adRecMultipleChanges = 0x40,
        /// <summary>
        /// レコードが新しいことを表します。
        /// </summary>
        adRecNew = 0x1,
        /// <summary>
        /// 開いているストレージ オブジェクトとの競合のため、レコードが保存されなかったことを表します。
        /// </summary>
        adRecObjectOpen = 0x4000,
        /// <summary>
        /// レコードの更新が成功したことを表します。
        /// </summary>
        adRecOK = 0,
        /// <summary>
        /// メモリ不足のためレコードが保存されなかったことを表します。
        /// </summary>
        adRecOutOfMemory = 0x8000,
        /// <summary>
        /// 保留中の挿入を参照するので、レコードが保存されなかったことを表します。
        /// </summary>
        adRecPendingChanges = 0x80,
        /// <summary>
        /// ユーザーの権限不足により、レコードが保存されなかったことを表します。
        /// </summary>
        adRecPermissionDenied = 0x10000,
        /// <summary>
        /// 基になるデータベースの構造に合わないので、レコードが保存されなかったことを表します。
        /// </summary>
        adRecSchemaViolation = 0x20000,
        /// <summary>
        /// レコードが変更されなかったことを表します。
        /// </summary>
        adRecUnmodified = 0x8,
    }

    /// <summary>
    /// コマンド引数を解釈する方法を指定します。
    /// </summary>
    [Flags]
    public enum CommandTypeEnum
    {
        /// <summary>
        /// コマンドの型引数を指定しません。
        /// </summary>
        adCmdUnspecified = -1,
        /// <summary>
        /// 通常のクエリ。
        /// </summary>
        adCmdText = 1,
        /// <summary>
        /// テーブル名。
        /// </summary>
        adCmdTable = 2,
        /// <summary>
        /// ストアド プロシージャ。
        /// </summary>
        adCmdStoredProc = 4,
        /// <summary>
        /// 不明
        /// </summary>
        adCmdUnknown = 8,
        /// <summary>
        /// ファイル名
        /// </summary>
        adCmdFile = 256,
        /// <summary>
        /// 
        /// </summary>
        adCmdTableDirect = 512
    }

    /// <summary>
    /// プロバイダーがコマンドを実行する方法を指定します。
    /// </summary>
    [Flags]
    public enum ExecuteOptionEnum
    {
        /// <summary>
        /// コマンドが指定されていないことを示します。
        /// </summary>
        adOptionUnspecified = -1,
        /// <summary>
        /// コマンドを非同期的に実行することを示します。
        /// </summary>
        adAsyncExecute = 16,
        /// <summary>
        /// CacheSize プロパティで指定した初期量の残りの行を非同期に取得することを示します。
        /// </summary>
        adAsyncFetch = 32,
        /// <summary>
        /// 取得中にメイン スレッドをブロックしないことを示します。
        /// </summary>
        adAsyncFetchNonBlocking = 64,
        /// <summary>
        /// コマンド テキストが、行を返さないコマンドまたはストアド プロシージャ (たとえば、データの挿入のみを行うコマンド) であることを示します。
        /// </summary>
        adExecuteNoRecords = 128,
        /// <summary>
        /// コマンドの実行結果がストリームとして返されることを示します。
        /// </summary>
        adExecuteStream = 1024,
        /// <summary>
        /// CommandText が、Record オブジェクトとして返される単一の行を返すコマンドまたはストアド プロシージャであることを示します。
        /// </summary>
        adExecuteRecord = 2048
    }

    /// <summary>
    /// パラメータの種別を指定します。
    /// </summary>
    public enum ParameterDirectionEnum
    {
        /// <summary>
        /// パラメーターの方向が既知であることを示します。
        /// </summary>
        adParamUnknown,
        /// <summary>
        /// 既定値です。 パラメーターが入力パラメーターを表すことを示します。
        /// </summary>
        adParamInput,
        /// <summary>
        /// パラメーターが出力パラメーターを表すことを示します。
        /// </summary>
        adParamOutput,
        /// <summary>
        /// パラメーターが入力と出力の両方のパラメーターを表すことを示します。
        /// </summary>
        adParamInputOutput,
        /// <summary>
        /// パラメーターが戻り値を表すことを示します。
        /// </summary>
        adParamReturnValue
    }

    /// <summary>
    /// パラメータの属性を指定します。
    /// </summary>
    [Flags]
    public enum ParameterAttributesEnum
    {
        /// <summary>
        /// パラメーターが符号付きの値を受け取ることを示します。
        /// </summary>
        adParamSigned = 16,
        /// <summary>
        /// パラメーターが null 値を受け入れることを示します。
        /// </summary>
        adParamNullable = 64,
        /// <summary>
        /// パラメーターがバイナリ データを受け入れることを示します。
        /// </summary>
        adParamLong = 128
    }
}
