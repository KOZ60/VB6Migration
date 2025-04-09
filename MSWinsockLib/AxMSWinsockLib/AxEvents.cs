using System;
using MSWinsockLib;

namespace AxMSWinsockLib
{
    /// <summary>
    /// リモート マシンが接続を要求してきたときに発生します。 
    /// </summary>
    public delegate void DMSWinsockControlEvents_ConnectionRequestEventHandler(object sender, DMSWinsockControlEvents_ConnectionRequestEvent e);

    /// <summary>
    /// 新しいデータが送られてきたときに発生します。
    /// </summary>
    public delegate void DMSWinsockControlEvents_DataArrivalEventHandler(object sender, DMSWinsockControlEvents_DataArrivalEvent e);

    /// <summary>
    /// バックグラウンド処理でエラーが発生したとき (たとえば、接続に失敗したとき、バックグラウンドで実行している送信や受信に失敗したときなど) に発生します。
    /// </summary>
    public delegate void DMSWinsockControlEvents_ErrorEventHandler(object sender, DMSWinsockControlEvents_ErrorEvent e);

    /// <summary>
    /// データの送信中に発生します。
    /// </summary>
    public delegate void DMSWinsockControlEvents_SendProgressEventHandler(object sender, DMSWinsockControlEvents_SendProgressEvent e);

    /// <summary>
    /// ConnectionRequestEvent イベントのイベントデータ
    /// </summary>
    public interface DMSWinsockControlEvents_ConnectionRequestEvent
    {
        /// <summary>
        /// 送られてきた接続要求の識別子
        /// </summary>
        IntPtr requestID { get; set; }
    }

    /// <summary>
    /// DataArrival イベントのイベントデータ
    /// </summary>
    public interface DMSWinsockControlEvents_DataArrivalEvent
    {
        /// <summary>
        /// 取得できるデータ全体のサイズが渡されます。
        /// </summary>
        int bytesTotal { get; set; }
    }

    /// <summary>
    /// Error イベントのイベントデータ
    /// </summary>
    public interface DMSWinsockControlEvents_ErrorEvent
    {
        /// <summary>
        /// エラー コード
        /// </summary>
        int number { get; set; }
        /// <summary>
        /// エラーの内容を説明する文字列
        /// </summary>
        string description { get; set; }
        /// <summary>
        /// 長整数の SCODE が渡されます。
        /// </summary>
        int scode { get; set; }
        /// <summary>
        /// エラーの発生元を説明する文字列が渡されます。
        /// </summary>
        string source { get; set; }
        /// <summary>
        /// ヘルプ ファイルの名前の入った文字列が渡されます。
        /// </summary>
        string helpFile { get; set; }
        /// <summary>
        /// ヘルプ ファイルのコンテキストが渡されます。
        /// </summary>
        int helpContext { get; set; }
        /// <summary>
        /// 表示をキャンセルするかどうかを指定します。既定値の偽 (False) にした場合は、既定のエラー メッセージ ボックスが表示されます。既定のメッセージ ボックスが表示されないようにするには、CancelDisplay を真 (True) に設定します。
        /// </summary>
        bool cancelDisplay { get; set; }

    }

    /// <summary>
    /// SendProgress イベントのイベントデータ
    /// </summary>
    public interface DMSWinsockControlEvents_SendProgressEvent
    {
        /// <summary>
        /// 前回このイベントが発生した以降に送信されたデータの量がバイト単位で渡されます。
        /// </summary>
        int bytesSent { get; set; }

        /// <summary>
        /// 送信バッファ内で送信待ちとなっているデータの量がバイト単位で渡されます。
        /// </summary>
        int bytesRemaining { get; set; }
    }
}
