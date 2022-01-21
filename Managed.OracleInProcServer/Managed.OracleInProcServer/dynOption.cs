using System;
using System.ComponentModel;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Managed.OracleInProcServer
{
    /// <summary>
    /// ダイナセットのオプション状態を示すビット・フラグ。
    /// </summary>
    [Flags]
    public enum dynOption
    {
        /// <summary>
        /// デフォルトの動作を受け入れる。
        /// </summary>
        /// <remarks>
        /// AddNewまたはEditメソッドの使用時に、明示的に設定されていないフィールド(列)の値はNULLに設定されます。
        /// NULL値は、データベースの列値のデフォルトを上書きします。Edit("SELECT...FOR UPDATE")の使用時に、行のロック解除を待機します。
        /// SQLの非ブロック機能は、使用できなくなります。
        /// </remarks>
        ORADYN_DEFAULT = 0,
        /// <summary>
        /// データベース・パラメータの自動的なバインドを実行しない。
        /// </summary>
        ORADYN_NO_AUTOBIND = 1,
        /// <summary>
        /// データベースから取り出された文字列データから、後続するブランクを削除しない。
        /// </summary>
        ORADYN_NO_BLANKSTRIP = 2,
        /// <summary>
        /// ダイナセットを読取り専用にする。
        /// </summary>
        ORADYN_READONLY = 4,
        /// <summary>
        /// ローカル・データ・キャッシュの DataTable を作成しない。
        /// </summary>
        /// <remarks>
        /// このフラグを指定すると読み取り用スレッドを作成しません。
        /// </remarks>
        ORADYN_NOCACHE = 8,
        /// <summary>
        /// (無効) OraDatabase の Oracle モードと同じように動作するが、作成中のダイナセットにのみ影響を与える。
        /// </summary>
        /// <remarks>
        /// AddNewメソッドの使用時に、Oracle データベースにより、デフォルトのフィールド(列)値が設定されます。
        /// 挿入または追加操作の後、Oracle のデフォルトの列値はただちにデータベースから再度フェッチされます。
        /// OraDatabase オブジェクトが ORADB_ORAMODE で作成されている場合、ダイナセットは(互換性を保つために)そのプロパティを継承します。
        /// </remarks>
        ORADYN_ORAMODE = 16,
        /// <summary>
        /// (無効) OraDatabase の ORADB_NO_REFETCH モードと同じように動作するが、作成中のダイナセットにのみ影響を与える。
        /// </summary>
        /// <remarks>
        /// Oracleモードと同様に実行されますが、データはローカル・キャッシュに再フェッチされません。これによってパフォーマンスが向上します。
        /// OraDatabase オブジェクトが ORADB_NO_REFETCH で作成されている場合、ダイナセットは(互換性を保つために)そのプロパティを継承します。
        /// </remarks>
        ORADYN_NO_REFETCH = 32,
        /// <summary>
        /// ダイナセットの作成時に MoveFirst を強制的に実行しない。
        /// </summary>
        /// <remarks>
        /// ダイナセット作成時、BOF と EOF は両方ともTRUEです。
        /// </remarks> 
        ORADYN_NO_MOVEFIRST = 64,
        /// <summary>
        /// (無効)UpdateおよびDeleteメソッドで、読取り一貫性をチェックしない。
        /// </summary>
        /// <remarks>
        /// OracleInProcServer.NET では、読取り一貫性をチェックしません。
        /// </remarks>
        ORADYN_DIRTY_WRITE = 128,
    }
}
