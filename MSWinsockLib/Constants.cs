using System;
using System.Collections.Generic;
using System.Text;

namespace MSWinsockLib
{
    /// <summary>
    /// Winsock エラーの定数
    /// </summary>
    public enum ErrorConstants
    {
        /// <summary>メモリが不足しています。</summary>
        sckOutOfMemory = 7,

        /// <summary>プロパティの値が不正です。</summary>
        sckInvalidPropertyValue = 380,

        /// <summary>値の取得のみが可能なプロパティです。</summary>
        sckSetNotSupported = 383,

        /// <summary>値の設定のみが可能なプロパティです。</summary>
        sckGetNotSupported = 394,

        /// <summary>処理がキャンセルされました。</summary>
        sckOpCanceled = 10004,

        /// <summary>不正な引数です。</summary>
        sckInvalidArgument = 10014,

        /// <summary>ソケットはブロッキングを行いません。指定した処理がブロッキングを行います。</summary>
        sckWouldBlock = 10035,

        /// <summary>ブロッキング処理が実行中です。</summary>
        sckInProgress = 10036,

        /// <summary>処理が完了しました。ブロッキング処理は実行されていません。</summary>
        sckAlreadyComplete = 10037,

        /// <summary>記述子はソケットではありません。</summary>
        sckNotSocket = 10038,

        /// <summary>データグラムはバッファより大きいため切り詰められます。</summary>
        sckMsgTooBig = 10040,

        /// <summary>指定したポートは使用できません。</summary>
        sckPortNotSupported = 10043,

        /// <summary>アドレスが使用されています。</summary>
        sckAddressInUse = 10048,

        /// <summary>アドレスが使用できません。</summary>
        sckAddressNotAvailable = 10049,

        /// <summary>サブシステムで異常が発生しました。</summary>
        sckNetworkSubsystemFailed = 10050,

        /// <summary>現在、このホストからネットワークに接続できません。</summary>
        sckNetworkUnreachable = 10051,

        /// <summary>SO_KEEPALIVE が設定され、接続がタイムアウトしました。</summary>
        sckNetReset = 10052,

        /// <summary>タイムアウトまたは他の問題によって接続が中断されます。</summary>
        sckConnectAborted = 10053,

        /// <summary>接続が受け付けられませんでした。</summary>
        sckConnectionReset = 10054,

        /// <summary>利用可能なバッファスペースがありません。</summary>
        sckNoBufferSpace = 10055,

        /// <summary>ソケットは既に接続されています。</summary>
        sckAlreadyConnected = 10056,

        /// <summary>ソケットは接続されていません。</summary>
        sckNotConnected = 10057,

        /// <summary>ソケットはシャットダウンされました。</summary>
        sckSocketShutdown = 10058,

        /// <summary>タイムアウトのため接続できませんでした。</summary>
        sckTimedout = 10060,

        /// <summary>接続が受け付けられませんでした。</summary>
        sckConnectionRefused = 10061,

        /// <summary>Winsock の初期化処理が実行されていません。</summary>
        sckNotInitialized = 10093,

        /// <summary>ホストが見つかりません。</summary>
        sckHostNotFound = 11001,

        /// <summary>ホストが見つかりません。(サーバー側に問題がある可能性があります。)</summary>
        sckHostNotFoundTryAgain = 11002,

        /// <summary>回復不可能なエラーです。</summary>
        sckNonRecoverableError = 11003,

        /// <summary>ホスト名は適切ですが、要求された種類のデータレコードはありません。</summary>
        sckNoData = 11004,

        /// <summary>接続状態に問題があります。</summary>
        sckBadState = 40006,

        /// <summary>引数が適切でないか、指定した範囲外です。</summary>
        sckInvalidArg = 40014,

        /// <summary>正常に完了しました。</summary>
        sckSuccess = 40017,

        /// <summary>サポートされないバリアント型です。</summary>
        sckUnsupported = 40018,
        
        /// <summary>現在の状態では不正な処理です。</summary>
        sckInvalidOp = 40020,
        
        /// <summary>引数の値が範囲外です。</summary>
        sckOutOfRange = 40021,

        /// <summary>要求のプロトコルに問題があります。</summary>
        sckWrongProtocol = 40026
    }

    /// <summary>
    /// Protocol プロパティの定数です。
    /// </summary>
    public enum ProtocolConstants
    {
        /// <summary>TCP プロトコルを使用します。</summary>
        sckTCPProtocol,

        /// <summary>UDP プロトコルを使用します。(未サポート)</summary>
        sckUDPProtocol
    }

    /// <summary>
    /// State プロパティの定数です。
    /// </summary>
    public enum StateConstants
    {

        /// <summary>ソケットは現在閉じられています。</summary>
        sckClosed,
        /// <summary>ソケットは現在開かれています。</summary>
        sckOpen,
        /// <summary>ソケットは要求を受付可能です。</summary>
        sckListening,
        /// <summary>ソケットによって要求が保留されました。</summary>
        sckConnectionPending,
        /// <summary>ソケットはリモートコンピュータ名を特定しています。</summary>
        sckResolvingHost,
        /// <summary>ソケットはリモートコンピュータ名を特定しました。</summary>
        sckHostResolved,
        /// <summary>ソケットはリモートコンピュータに接続しています。</summary>
        sckConnecting,
        /// <summary>ソケットはリモートコンピュータに接続されました。</summary>
        sckConnected,
        /// <summary>ソケットはリモートコンピュータへの接続を閉じています。</summary>
        sckClosing,
        /// <summary>ソケットでエラーが発生しました。</summary>
        sckError
    }
}
