using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{

    /// <summary>
    /// Recordset オブジェクトのオプション
    /// </summary>
    [Flags]
    public enum RecordsetOptionEnum
    {
        /// <summary>ほかのユーザーがレコードを修正したり、追加できないようにします。</summary>
        dbDenyWrite = 1,

        /// <summary>ほかのユーザーがテーブルのデータの読み取りをできないようにします。</summary>
        dbDenyRead = 2,

        /// <summary>ほかのユーザーが Recordset を変更できないようにします。</summary>
        dbReadOnly = 4,
        
        /// <summary>ユーザーは Recordset に新しいレコードを追加することができますが、既存のレコードの編集および削除を行うことはできません。</summary>
        dbAppendOnly = 8,

        /// <summary>矛盾を許す更新を可能にします。</summary>
        dbInconsistent = 16,

        /// <summary>一貫性のある更新のみを可能にします。</summary>
        dbConsistent = 32,

        /// <summary>SQL ステートメントを Jet 接続型 ODBC データ ソースに渡して処理します。</summary>
        dbSQLPassThrough = 64,

        /// <summary>エラーが発生すると更新をロールバックします。</summary>
        dbFailOnError = 128,

        /// <summary>前方スクロール タイプの Recordset オブジェクト</summary>
        dbForwardOnly = 256,

        /// <summary>編集中のデータをほかのユーザーが変更しようとした場合、実行時エラーが発生します。</summary>
        dbSeeChanges = 512,

        /// <summary>非同期クエリーを実行します。</summary>
        dbRunAsync = 1024,

        /// <summary>SQLPrepare をスキップし SQLExecDirect を直接呼び出して、クエリーを実行します。</summary>
        dbExecDirect = 2048
    }
}
