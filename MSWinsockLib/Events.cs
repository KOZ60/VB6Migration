using System;
using System.Collections.Generic;
using System.Text;
using AxMSWinsockLib;

namespace MSWinsockLib
{
    /// <summary>
    /// リモート マシンが接続を要求してきたときに発生します。 
    /// </summary>
    public delegate void ConnectionRequestEventHandler(object sender, ConnectionRequestEvent e);

    /// <summary>
    /// 新しいデータが送られてきたときに発生します。
    /// </summary>
    public delegate void DataArrivalEventHandler(object sender, DataArrivalEvent e);

    /// <summary>
    /// バックグラウンド処理でエラーが発生したとき (たとえば、接続に失敗したとき、バックグラウンドで実行している送信や受信に失敗したときなど) に発生します。
    /// </summary>
    public delegate void ErrorEventHandler(object sender, ErrorEvent e);

    /// <summary>
    /// データの送信中に発生します。
    /// </summary>
    public delegate void SendProgressEventHandler(object sender, SendProgressEvent e);

    /// <summary>
    /// ConnectionRequestEvent イベントのイベントデータ
    /// </summary>
    public class ConnectionRequestEvent : EventArgs, DMSWinsockControlEvents_ConnectionRequestEvent
    {
        /// <summary>
        /// 送られてきた接続要求の識別子
        /// </summary>
        public IntPtr requestID { get; set; }

        /// <summary>
        /// このクラスのインスタンスを作成します。
        /// </summary>
        /// <param name="requestID">送られてきた接続要求の識別子</param>
        public ConnectionRequestEvent(IntPtr requestID)
        {
            this.requestID = requestID;
        }
    }

    /// <summary>
    /// DataArrival イベントのイベントデータ
    /// </summary>
    public class DataArrivalEvent : EventArgs, DMSWinsockControlEvents_DataArrivalEvent
    {
        /// <summary>
        /// 取得できるデータ全体のサイズが渡されます。
        /// </summary>
        public int bytesTotal { get; set; }

        /// <summary>
        /// このクラスのインスタンスを作成します。
        /// </summary>
        /// <param name="bytesTotal">取得できるデータ全体のサイズが渡されます。</param>
        public DataArrivalEvent(int bytesTotal)
        {
            this.bytesTotal = bytesTotal;
        }
    }

    /// <summary>
    /// Error イベントのイベントデータ
    /// </summary>
    public class ErrorEvent : EventArgs, DMSWinsockControlEvents_ErrorEvent
    {
        /// <summary>
        /// エラー コード
        /// </summary>
        public int number { get; set; }

        /// <summary>
        /// エラーの内容を説明する文字列
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 長整数の SCODE が渡されます。
        /// </summary>
        public int scode { get; set; }

        /// <summary>
        /// エラーの発生元を説明する文字列が渡されます。
        /// </summary>
        public string source { get; set; }

        /// <summary>
        /// ヘルプ ファイルの名前の入った文字列が渡されます。
        /// </summary>
        public string helpFile { get; set; }

        /// <summary>
        /// ヘルプ ファイルのコンテキストが渡されます。
        /// </summary>
        public int helpContext { get; set; }

        /// <summary>
        /// 表示をキャンセルするかどうかを指定します。既定値の偽 (False) にした場合は、既定のエラー メッセージ ボックスが表示されます。既定のメッセージ ボックスが表示されないようにするには、CancelDisplay を真 (True) に設定します。
        /// </summary>
        public bool cancelDisplay { get; set; }

        /// <summary>
        /// このクラスのインスタンスを作成します。
        /// </summary>
        /// <param name="number">エラー コード</param>
        /// <param name="description">エラーの内容を説明する文字列</param>
        /// <param name="scode">長整数の SCODE</param>
        /// <param name="source">ラーの発生元を説明する文字列</param>
        /// <param name="helpFile">ヘルプ ファイルの名前の入った文字列</param>
        /// <param name="helpContext">ヘルプ ファイルのコンテキスト</param>
        /// <param name="cancelDisplay">表示をキャンセルするかどうかを指定します。</param>
        public ErrorEvent(int number, string description, int scode, string source, string helpFile, int helpContext, bool cancelDisplay)
        {
            this.number = number;
            this.description = description;
            this.scode = scode;
            this.source = source;
            this.helpFile = helpFile;
            this.helpContext = helpContext;
            this.cancelDisplay = cancelDisplay;
        }
    }

    /// <summary>
    /// SendProgress イベントのイベントデータ
    /// </summary>
    public class SendProgressEvent : EventArgs, DMSWinsockControlEvents_SendProgressEvent
    {
        /// <summary>
        /// 前回このイベントが発生した以降に送信されたデータの量がバイト単位で渡されます。
        /// </summary>
        public int bytesSent { get; set; }

        /// <summary>
        /// 送信バッファ内で送信待ちとなっているデータの量がバイト単位で渡されます。
        /// </summary>
        public int bytesRemaining { get; set; }

        /// <summary>
        /// このクラスのインスタンスを作成します。
        /// </summary>
        /// <param name="bytesSent">前回このイベントが発生した以降に送信されたデータの量</param>
        /// <param name="bytesRemaining">送信バッファ内で送信待ちとなっているデータの量</param>
        public SendProgressEvent(int bytesSent, int bytesRemaining)
        {
            this.bytesSent = bytesSent;
            this.bytesRemaining = bytesRemaining;
        }
    }
}
