using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualBasic;

namespace MSWinsockLib
{
    internal static class NativeMethods
    {
		private const int FORMAT_MESSAGE_IGNORE_INSERTS = 0x200;
		private const int FORMAT_MESSAGE_FROM_SYSTEM = 0x1000;
		private const int PROCESS_DEFAULT_LANGUAGE = 0x400;
		private const int FORMAT_MESSAGE_MAX_WIDTH_MASK = 0xff;

        [DllImport("kernel32.dll")]
        public static extern int FormatMessage(int Flags, IntPtr Source, int MessageID, int LanguageID, StringBuilder Buffer, int Size, IntPtr Args);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        //------------------------------------------------------------------------------
        // 指定されたエラー番号のVBエラーを発生する
        //------------------------------------------------------------------------------
        public static void RaiseError(int iErrNumber)
		{
            Information.Err().Clear();
            Information.Err().Raise(iErrNumber, "Winsock", GetErrorMsg(iErrNumber));
		}
        //------------------------------------------------------------------------------
        // 指定されたエラー番号のメッセージをシステムより取得
        //------------------------------------------------------------------------------
        public static string GetErrorMsg(int ErrNumber)
        {
            StringBuilder builder = new StringBuilder(1024);
            string strBuffer;
            int nRet = 0;

            nRet = FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
                                 IntPtr.Zero,
                                 ErrNumber, 
                                 PROCESS_DEFAULT_LANGUAGE,
                                 builder,
                                 builder.Capacity,
                                 IntPtr.Zero);
            if (nRet > 0)
            {
                strBuffer = builder.ToString();
            }
            else
            {
                // 古いOSでは Winsock のエラーメッセージが取り出せなかったので、その名残
                strBuffer = WSAErrorString(ErrNumber);
            }

            strBuffer = strBuffer.Replace("\r", "");
            strBuffer = strBuffer.Replace("\n", "");

            return strBuffer;

        }
        //------------------------------------------------------------------------------
        // 指定されたエラー番号の Winsock メッセージを設定
        //------------------------------------------------------------------------------
        private static string WSAErrorString(int ErrNumber)
        {
            string functionReturnValue = null;

            switch (ErrNumber)
            {
                case WinsockAPI.WSAEINTR:
                    functionReturnValue = "呼び出しが取り消された。";
                    break;
                case WinsockAPI.WSAEACCES:
                    functionReturnValue = "指定されたアドレスは、ブロードキャストパケットの送信をサポートしていない。";
                    break;
                case WinsockAPI.WSAEFAULT:
                    functionReturnValue = "32ビットインターネットアドレス指定に誤りがあります。";
                    break;
                case WinsockAPI.WSAEINVAL:
                    functionReturnValue = "関数の呼び出し順番に誤りがある。";
                    break;
                case WinsockAPI.WSAEMFILE:
                    functionReturnValue = "利用可能なファイル番号が存在しない。";
                    break;
                case WinsockAPI.WSAEWOULDBLOCK:
                    functionReturnValue = "リソースが一時的に利用できなくなっています｡ ";
                    break;
                case WinsockAPI.WSAEINPROGRESS:
                    functionReturnValue = "ブロッキング呼び出し進行中。";
                    break;
                case WinsockAPI.WSAEALREADY:
                    functionReturnValue = "ノンブロッキング型の処理を処理中。";
                    break;
                case WinsockAPI.WSAENOTSOCK:
                    functionReturnValue = "ソケット以外を指定した。";
                    break;
                case WinsockAPI.WSAEDESTADDRREQ:
                    functionReturnValue = "リモートアドレスが指定されていない。";
                    break;
                case WinsockAPI.WSAEMSGSIZE:
                    functionReturnValue = "データがバッファより大きいので切りつめられた。";
                    break;
                case WinsockAPI.WSAEPROTOTYPE:
                    functionReturnValue = "ソケットは指定されたプロトコルをサポートしていない。";
                    break;
                case WinsockAPI.WSAENOPROTOOPT:
                    functionReturnValue = "プロトコルのオプションに誤りがある。";
                    break;
                case WinsockAPI.WSAEPROTONOSUPPORT:
                    functionReturnValue = "指定されたプロトコルはサポートされていない。";
                    break;
                case WinsockAPI.WSAESOCKTNOSUPPORT:
                    functionReturnValue = "指定されたソケットタイプはサポートされていない。";
                    break;
                case WinsockAPI.WSAEOPNOTSUPP:
                    functionReturnValue = "指定されたソケットがサポートしていない操作である。";
                    break;
                case WinsockAPI.WSAEPFNOSUPPORT:
                    functionReturnValue = "指定されたアドレスファミリがサポートされていない。";
                    break;
                case WinsockAPI.WSAEAFNOSUPPORT:
                    functionReturnValue = "指定されたプロトコルでは、指定されたアドレスファミリがサポートされていない。";
                    break;
                case WinsockAPI.WSAEADDRINUSE:
                    functionReturnValue = "指定されたアドレスは使用中。";
                    break;
                case WinsockAPI.WSAEADDRNOTAVAIL:
                    functionReturnValue = "指定されたアドレスは使用できない。";
                    break;
                case WinsockAPI.WSAENETDOWN:
                    functionReturnValue = "ネットワークがダウンしている。";
                    break;
                case WinsockAPI.WSAENETUNREACH:
                    functionReturnValue = "リモートホストのネットワークに接続できない。";
                    break;
                case WinsockAPI.WSAENETRESET:
                    functionReturnValue = "ネットワークから切断された。";
                    break;
                case WinsockAPI.WSAECONNABORTED:
                    functionReturnValue = "ネットワーク不良で接続が中止された。";
                    break;
                case WinsockAPI.WSAECONNRESET:
                    functionReturnValue = "リモートホストから切断された。";
                    break;
                case WinsockAPI.WSAENOBUFS:
                    functionReturnValue = "バッファ領域不足。";
                    break;
                case WinsockAPI.WSAEISCONN:
                    functionReturnValue = "ソケットはすでに接続されている。";
                    break;
                case WinsockAPI.WSAENOTCONN:
                    functionReturnValue = "ソケットが接続されていない。";
                    break;
                case WinsockAPI.WSAESHUTDOWN:
                    functionReturnValue = "ソケットがシャットダウンされている。";
                    break;
                case WinsockAPI.WSAETIMEDOUT:
                    functionReturnValue = "接続処理タイムアウト。";
                    break;
                case WinsockAPI.WSAECONNREFUSED:
                    functionReturnValue = "接続が拒否された。";
                    break;
                case WinsockAPI.WSAEHOSTDOWN:
                    functionReturnValue = "リモートホストがダウンしている。";
                    break;
                case WinsockAPI.WSAEHOSTUNREACH:
                    functionReturnValue = "ホストへのルートが存在しない。";
                    break;
                case WinsockAPI.WSAEPROCLIM:
                    functionReturnValue = "プロセスが多すぎます。";
                    break;
                case WinsockAPI.WSASYSNOTREADY:
                    functionReturnValue = "ネットワークサブシステムに通信するための準備ができていない。";
                    break;
                case WinsockAPI.WSAVERNOTSUPPORTED:
                    functionReturnValue = "指定されたバージョンがサポートされていない。";
                    break;
                case WinsockAPI.WSANOTINITIALISED:
                    functionReturnValue = "WSAStartup関数を呼び出していないか、失敗しています。";
                    break;
                case WinsockAPI.WSAHOST_NOT_FOUND:
                    functionReturnValue = "ホストが見つからない。";
                    break;
                case WinsockAPI.WSATRY_AGAIN:
                    functionReturnValue = "権限があるホストが見つからない。";
                    break;
                case WinsockAPI.WSANO_RECOVERY:
                    functionReturnValue = "修復不可能なエラーが発生した。";
                    break;
                case WinsockAPI.WSANO_DATA:
                    functionReturnValue = "データレコードの型が要求されていない。";
                    break;
                default:
                    functionReturnValue = "不明なエラーが返された。エラーコード = (" + ErrNumber + ")";
                    break;
            }
            return functionReturnValue;
        }

        public static int HIWORD(int n)
        {
            return (n >> 16) & 0xffff;
        }

        public static int HIWORD(IntPtr n)
        {
            return HIWORD(unchecked((int)(long)n));
        }

        public static int LOWORD(int n)
        {
            return n & 0xffff;
        }

        public static int LOWORD(IntPtr n)
        {
            return LOWORD(unchecked((int)(long)n));
        }

        public static void VBRaiseError(int nError, string strMessage)
        {
            Information.Err().Clear();
            if (string.IsNullOrEmpty(strMessage))
            {
                strMessage = Conversion.ErrorToString(nError);
            }
            Information.Err().Raise(nError, "Microsoft.VisualBasic.Compatibility.dll", strMessage, null, null);
        }

        [DllImport("Kernel32.dll", SetLastError = false)]
        public static extern void RtlMoveMemory(IntPtr dest, IntPtr src, int size);
    }
}
