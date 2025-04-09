using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.VisualBasic;

namespace MSWinsockLib
{
    /// <summary>
    /// Winsock コントロール互換コンポーネント
    /// </summary>
	public class Winsock : Component , ISupportInitialize
	{
        static Encoding enc = Encoding.GetEncoding("SHIFT_JIS");

        IntPtr m_SocketHandle;                  // ソケットハンドル

        StateConstants m_State;                 // コントロールの状態

        ByteBuffer m_SendBuffer;                // 送信バッファ

        string m_RemoteHost;
        WinsockAPI.sockaddr_in m_Remote;        // 接続先の sockaddr_in
        WinsockAPI.sockaddr_in m_Local;         // 自分の sockaddr_in

        ControlInternal m_ControlInternal;      // サブクラス化のためのコントロール

        /// <summary>
        /// Winsock コンポーネントのインスタンスを作成します。
        /// </summary>
        public Winsock()
        {
            m_SocketHandle = WinsockAPI.INVALID_SOCKET;
            m_State = StateConstants.sckClosed;

            m_SendBuffer = new ByteBuffer();

            m_RemoteHost = string.Empty;
            m_Remote = WinsockAPI.sockaddr_in.Empty;
            m_Local = WinsockAPI.sockaddr_in.Empty;

            m_ControlInternal = new ControlInternal(this);
        }

        /// <summary>
        /// Winsock コンポーネントのインスタンスを作成します。
        /// </summary>
        /// <param name="container">コンテナを指定します。</param>
        public Winsock(IContainer container) : this()
        {
            container.Add(this);
        }

        //------------------------------------------------------------------------------
        // ソケット作成されていなければ作成する
        //------------------------------------------------------------------------------
        internal IntPtr SocketHandleInternal
        {
            get 
            {
                if (m_SocketHandle == WinsockAPI.INVALID_SOCKET)
                {
                    // Socket を作成
                    m_SocketHandle = WinsockAPI.socket(WinsockAPI.AF_INET, WinsockAPI.SOCK_STREAM, WinsockAPI.IPPROTO_TCP);
                    if (m_SocketHandle == WinsockAPI.INVALID_SOCKET)
                    {
                        SocketError();
                    }

                    // ソケットを初期化
                    InitializeSocket();
                }
                return m_SocketHandle;                   
            }
        }

        internal void InitializeSocket()
        {

            int nRet;

            // SO_REUSEADDR

            int nOption = 1;
            int nSize = Marshal.SizeOf(nOption);
            nRet = WinsockAPI.setsockopt(SocketHandleInternal, WinsockAPI.SOL_SOCKET, WinsockAPI.SO_REUSEADDR, ref nOption, nSize);
            if (nRet == WinsockAPI.SOCKET_ERROR) SocketError();

            // 非ブロッキング
            int nEvents = WinsockAPI.FD_CONNECT | WinsockAPI.FD_WRITE | WinsockAPI.FD_ACCEPT | WinsockAPI.FD_CLOSE | WinsockAPI.FD_READ;
            nRet = WinsockAPI.WSAAsyncSelect(SocketHandleInternal, m_ControlInternal.Handle, WinsockAPI.WM_WINSOCK_MESSAGE, nEvents);
            if (nRet == WinsockAPI.SOCKET_ERROR) SocketError();
        }

        internal void CloseSocketInternal()
        {

            if (m_SocketHandle != WinsockAPI.INVALID_SOCKET)
            {
                int nRet = WinsockAPI.closesocket(m_SocketHandle);
            }
            m_SocketHandle = WinsockAPI.INVALID_SOCKET;
            m_State = StateConstants.sckClosed;
        }

        //------------------------------------------------------------------------------
        // エラーコードから VB のエラーを発生する
        //------------------------------------------------------------------------------
        private void SocketError()
        {
            SocketError(WinsockAPI.WSAGetLastError());
        }
        private void SocketError(int nErrCode)
        {
            switch (nErrCode)
            {
                // 0 なにもしない
                case 0:
                case WinsockAPI.WSAEWOULDBLOCK:
                    break;

                // エラーを発生
                default:
                    NativeMethods.RaiseError(nErrCode);
                    break;
            }
        }
        //------------------------------------------------------------------------------
        // Dispose されるとき、ソケットを破棄
        //------------------------------------------------------------------------------
        /// <summary>
        /// Component によって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースとアンマネージ リソースの両方を解放する場合は true。アンマネージ リソースだけを解放する場合は false。 </param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            CloseSocketInternal();
            if (m_ControlInternal != null)
            {
                m_ControlInternal.Dispose();
                m_ControlInternal = null;
            }
            if (m_SendBuffer != null)
            {
                m_SendBuffer.Dispose();
                m_SendBuffer = null;
            }
        }
        
        /// <summary>
        /// 受信したデータ (現在受信バッファ内にあるデータ) のサイズを返します。データを取得するには、GetData メソッドを使用します。
        /// </summary>
        [Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual int BytesReceived
		{
			get
			{
                byte[] buffer;
                return RecvInternal(out buffer, Constants.vbByte | Constants.vbArray, 5120, true, false);
            }
		}

        /// <summary>
        /// コントロールの状態を表す列挙型の値を返します。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual StateConstants CtlState
		{
			get
			{
                return m_State;
			}
		}

        /// <summary>
        /// ローカル マシンの名前を返します。値の取得のみ可能です。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual string LocalHostName
		{
			get
			{
                return Dns.GetHostName();
			}
		}

        /// <summary>
        /// ローカル マシンの IP アドレスを、ピリオドで区切られた形式 (xxx.xxx.xxx.xxx) の IP アドレスで返します。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual string LocalIP
		{
			get
			{
                IPHostEntry entry = Dns.GetHostEntry(LocalHostName);
                IPAddress[] ipAddrs = entry.AddressList;
                if (ipAddrs.Length > 0)
                    return ipAddrs[0].ToString();
                else
                    return "0.0.0.0";
            }
		}

        /// <summary>
        /// 使用するローカル ポートを設定します。値の取得も可能です。
        /// </summary>
        [DefaultValue(0)]
        public virtual int LocalPort
		{
			get
			{
                return WinsockAPI.ntohs(m_Local.sin_port);
            }
			set
			{
                m_Local.sin_port = WinsockAPI.htons(unchecked((ushort)value));
			}
		}

        // Connect 成功時、Listen 開始時に呼ぶ
        private void GetLocalAddr()
        {
            int nRet = 0;
            WinsockAPI.sockaddr_in local = WinsockAPI.sockaddr_in.Empty;
            int nSize = Marshal.SizeOf(local);

            // 自分の情報を取得(使用したIPアドレス/ポート)
            nRet = WinsockAPI.getsockname(SocketHandleInternal, ref local, ref nSize);
            if (nRet == WinsockAPI.SOCKET_ERROR)
            {
                SocketError();
            }
            m_Local.sin_addr = local.sin_addr;
            m_Local.sin_port = local.sin_port;
        }

        /// <summary>
        /// ソケットのプロトコルを取得または設定します。
        /// </summary>
        [DefaultValue(typeof(ProtocolConstants), "sckTCPProtocol")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ProtocolConstants Protocol
		{
			get
			{
                return ProtocolConstants.sckTCPProtocol;
			}
			set
			{
                if (value != ProtocolConstants.sckTCPProtocol)
                    throw new NotImplementedException();
			}
		}

        /// <summary>
        /// リモートホストを取得または設定します。
        /// </summary>
        [DefaultValue("")]
        public virtual string RemoteHost
		{
			get
			{
                return m_RemoteHost;
			}
			set
			{
                m_RemoteHost = value;
                m_Remote.sin_addr = WinsockAPI.GetHostByNameAlias(value);
			}
		}

        /// <summary>
        /// リモートホストの IP アドレスを取得します。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual string RemoteHostIP
		{
			get
			{
                return WinsockAPI.InetAddrToString(m_Remote.sin_addr);
			}
		}

        /// <summary>
        /// リモートホストのポートを取得または設定します。
        /// </summary>
        [DefaultValue(0)]
        public virtual int RemotePort
		{
            get 
            { 
                return WinsockAPI.ntohs(m_Remote.sin_port); 
            }
            set 
            { 
                m_Remote.sin_port = WinsockAPI.htons(unchecked((ushort)value)); 
            }
        }

        /// <summary>
        /// ソケットハンドルを取得します。
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual IntPtr SocketHandle
		{
			get
			{
                return SocketHandleInternal;
			}
		}

        /// <summary>
        /// TCP サーバー アプリケーションのみで使用できます。このメソッドは、ConnectionRequest イベント内で、送られてきた接続要求を受け入れるときに使用します。
        /// </summary>
        /// <param name="requestID">ConnectionRequest イベントの requestID を指定します。</param>
		public virtual void Accept(IntPtr requestID)
		{
            WinsockAPI.sockaddr_in sockaddr = WinsockAPI.sockaddr_in.Empty;
            int nSize = Marshal.SizeOf(sockaddr);
            IntPtr hSocket = WinsockAPI.accept(requestID, ref sockaddr, ref nSize);
            if (hSocket == WinsockAPI.INVALID_SOCKET) SocketError();
            CloseSocketInternal();

            m_SocketHandle = hSocket;
            m_Remote = sockaddr;
            m_RemoteHost = WinsockAPI.InetAddrToString(sockaddr.sin_addr);
            InitializeSocket();
            m_State = StateConstants.sckConnected;
        }

        /// <summary>
        /// ソケットを特定のポートおよび IP アドレスに割り当てます。
        /// </summary>
		public virtual void Bind()
		{
            m_Local.sin_addr = WinsockAPI.INADDR_ANY;
            BindInternal(m_Local);
        }

        /// <summary>
        /// ソケットを特定のポートおよび IP アドレスに割り当てます。
        /// </summary>
        /// <param name="localPort">接続に使用するローカル ポート</param>
        /// <param name="localIP">接続に使用する IP アドレス</param>
		public virtual void Bind(int localPort, string localIP)
		{
            BindInternal(new WinsockAPI.sockaddr_in(localIP, localPort));
		}

        internal void BindInternal(WinsockAPI.sockaddr_in sockaddr)
        {
            int nSize = Marshal.SizeOf(sockaddr);
            int nRet = WinsockAPI.bind(SocketHandleInternal, ref sockaddr, ref nSize);
            if (nRet == WinsockAPI.SOCKET_ERROR) SocketError();
            m_Local = sockaddr;
        }

        /// <summary>
        /// 現在の接続を閉じます。
        /// </summary>
		public virtual void Close()
		{
            if (this.CtlState == StateConstants.sckConnected)
            {
                WinsockAPI.shutdown(SocketHandleInternal, WinsockAPI.SD_SEND);
                
                DataArrivalEvent e = new DataArrivalEvent(BytesReceived);
                OnDataArrivalCaller(e);
            }
            CloseSocketInternal();
        }

        /// <summary>
        /// リモートコンピュータに接続します。
        /// </summary>
		public virtual void Connect()
		{
            ConnectInternal(m_Remote);
		}

        /// <summary>
        /// リモートコンピュータに接続します。
        /// </summary>
        /// <param name="remoteHost">接続するリモート コンピュータの名前を指定します。</param>
        /// <param name="remotePort">接続するリモート コンピュータのポートを指定します。</param>
		public virtual void Connect(string remoteHost, int remotePort)
		{
            ConnectInternal(new WinsockAPI.sockaddr_in(remoteHost, remotePort));
		}

        internal void ConnectInternal(WinsockAPI.sockaddr_in sockaddr)
        {
            int nRet = WinsockAPI.connect(SocketHandleInternal, ref sockaddr, Marshal.SizeOf(sockaddr));
            if (nRet == WinsockAPI.SOCKET_ERROR) SocketError();
            m_Remote = sockaddr;
            GetLocalAddr();
            m_State = StateConstants.sckConnecting;
        }
        //------------------------------------------------------------------------------
        // GetData
        //------------------------------------------------------------------------------

        /// <summary>
        /// 現在のデータ ブロックを取得し、それを string型の変数に格納します。
        /// </summary>
        /// <param name="data">メソッドの実行が正常に完了したときに、取得したデータが格納される場所を指定します。</param>
        public virtual void GetData(out string data)
		{
            GetData(out data, Constants.vbString);
		}

        /// <summary>
        /// 現在のデータ ブロックを取得し、それを string型の変数に格納します。
        /// </summary>
        /// <param name="data">メソッドの実行が正常に完了したときに、取得したデータが格納される場所を指定します。</param>
        /// <param name="type">取得するデータの型を指定します。</param>
        public virtual void GetData(out string data, VariantType type)
		{
            GetData(out data, type, this.BytesReceived);
        }

        /// <summary>
        /// 現在のデータ ブロックを取得し、それを string型の変数に格納します。
        /// </summary>
        /// <param name="data">メソッドの実行が正常に完了したときに、取得したデータが格納される場所を指定します。</param>
        /// <param name="type">取得するデータの型を指定します。</param>
        /// <param name="maxLen">必要なデータのサイズを指定します。</param>
        public virtual void GetData(out string data, VariantType type, int maxLen)
        {
            byte[] byteData;
            GetData(out byteData, Constants.vbByte | Constants.vbArray, maxLen);
            data = enc.GetString(byteData);
        }

        /// <summary>
        /// 現在のデータ ブロックを取得し、それを byte型配列の変数に格納します。
        /// </summary>
        /// <param name="data">メソッドの実行が正常に完了したときに、取得したデータが格納される場所を指定します。</param>
        public virtual void GetData(out byte[] data)
        {
            GetData(out data, Constants.vbString);
        }

        /// <summary>
        /// 現在のデータ ブロックを取得し、それを byte型配列の変数に格納します。
        /// </summary>
        /// <param name="data">メソッドの実行が正常に完了したときに、取得したデータが格納される場所を指定します。</param>
        /// <param name="type">取得するデータの型を指定します。</param>
        public virtual void GetData(out byte[] data, VariantType type)
        {
            GetData(out data, type, this.BytesReceived);
        }

        /// <summary>
        /// 現在のデータ ブロックを取得し、それを byte型配列の変数に格納します。
        /// </summary>
        /// <param name="data">メソッドの実行が正常に完了したときに、取得したデータが格納される場所を指定します。</param>
        /// <param name="type">取得するデータの型を指定します。</param>
        /// <param name="maxLen">必要なデータのサイズを指定します。</param>
        public virtual void GetData(out byte[] data, VariantType type, int maxLen)
        {
            RecvInternal(out data, type, maxLen, false, true);
        }

        //------------------------------------------------------------------------------
        // GetData, PeekData 共通
        //------------------------------------------------------------------------------
        internal int RecvInternal(out byte[] data, VariantType type, int maxLen, bool peek, bool errHandle)
        {
            if (maxLen < 0) maxLen = 5120;

            IntPtr buffer = Marshal.AllocHGlobal(maxLen + 1);
            int flags = peek ? WinsockAPI.MSG_PEEK : 0;
            try
            {
                int nLength = WinsockAPI.recv(SocketHandleInternal, buffer, maxLen, flags);
                if (nLength == WinsockAPI.SOCKET_ERROR)
                {
                    int errorCode = WinsockAPI.WSAGetLastError();
                    if (errHandle && (errorCode != WinsockAPI.WSAEWOULDBLOCK)) SocketError(errorCode);
                    data = new byte[0];
                    return 0;
                }
                data = new byte[nLength];
                if (nLength > 0)
                {
                    Marshal.Copy(buffer, data, 0, nLength);
                }
                return nLength;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        /// <summary>
        /// ソケットを作成し、接続要求を受け付けるモードに移行します。このメソッドは、TCP 接続でしか機能しません。
        /// </summary>
        public virtual void Listen()
		{
            Bind();

            int nRet = WinsockAPI.listen(SocketHandleInternal, WinsockAPI.SOMAXCONN);
            if (nRet == WinsockAPI.SOCKET_ERROR)
            {
                SocketError();
            }
            m_State = StateConstants.sckListening;
        }

        //------------------------------------------------------------------------------
        // PeekData
        //------------------------------------------------------------------------------

        /// <summary>
        /// 現在のデータ ブロックを取得し、それを string型の変数に格納します。
        /// このメソッドは入力キューからデータを削除しません。
        /// </summary>
        /// <param name="data">メソッドの実行が正常に完了したときに、取得したデータが格納される場所を指定します。</param>
        public void PeekData(out string data)
        {
            PeekData(out data, Constants.vbString);
        }

        /// <summary>
        /// 現在のデータ ブロックを取得し、それを string型の変数に格納します。
        /// このメソッドは入力キューからデータを削除しません。
        /// </summary>
        /// <param name="data">メソッドの実行が正常に完了したときに、取得したデータが格納される場所を指定します。</param>
        /// <param name="type">取得するデータの型を指定します。</param>
        public void PeekData(out string data, VariantType type)
        {
            PeekData(out data, type, this.BytesReceived);
        }

        /// <summary>
        /// 現在のデータ ブロックを取得し、それを string型の変数に格納します。
        /// このメソッドは入力キューからデータを削除しません。
        /// </summary>
        /// <param name="data">メソッドの実行が正常に完了したときに、取得したデータが格納される場所を指定します。</param>
        /// <param name="type">取得するデータの型を指定します。</param>
        /// <param name="maxLen">必要なデータのサイズを指定します。</param>
        public void PeekData(out string data, VariantType type, int maxLen)
        {
            byte[] byteData;
            PeekData(out byteData, Constants.vbByte | Constants.vbArray, maxLen);
            data = enc.GetString(byteData);
        }

        /// <summary>
        /// 現在のデータ ブロックを取得し、それを byte型配列の変数に格納します。
        /// このメソッドは入力キューからデータを削除しません。
        /// </summary>
        /// <param name="data">メソッドの実行が正常に完了したときに、取得したデータが格納される場所を指定します。</param>
        public virtual void PeekData(out byte[] data)
        {
            PeekData(out data, Constants.vbByte | Constants.vbArray);
        }

        /// <summary>
        /// 現在のデータ ブロックを取得し、それを byte型配列の変数に格納します。
        /// このメソッドは入力キューからデータを削除しません。
        /// </summary>
        /// <param name="data">メソッドの実行が正常に完了したときに、取得したデータが格納される場所を指定します。</param>
        /// <param name="type">取得するデータの型を指定します。</param>
        public virtual void PeekData(out byte[] data, VariantType type)
        {
            PeekData(out data, type, this.BytesReceived);
        }

        /// <summary>
        /// 現在のデータ ブロックを取得し、それを byte型配列の変数に格納します。
        /// このメソッドは入力キューからデータを削除しません。
        /// </summary>
        /// <param name="data">メソッドの実行が正常に完了したときに、取得したデータが格納される場所を指定します。</param>
        /// <param name="type">取得するデータの型を指定します。</param>
        /// <param name="maxLen">必要なデータのサイズを指定します。</param>
        public virtual void PeekData(out byte[] data, VariantType type, int maxLen)
        {
            RecvInternal(out data, type, maxLen, true, true);
        }

        //------------------------------------------------------------------------------
        // データ送信
        //------------------------------------------------------------------------------
        internal void SendInternal()
        {
            int nLength = m_SendBuffer.Length;
            if (nLength == 0) return;

            int nRet = WinsockAPI.send(SocketHandleInternal, m_SendBuffer.Pointer, nLength, 0);
            if (nRet == WinsockAPI.SOCKET_ERROR)
            {
                int errorCode = WinsockAPI.WSAGetLastError();
                if (errorCode != WinsockAPI.WSAEWOULDBLOCK) SocketError(errorCode);
                nRet = 0;
            }
            m_SendBuffer.Remove(nRet);

            // イベントを発生

            int bytesSent = nRet;
            int bytesRemaining = m_SendBuffer.Length;

            if (bytesRemaining == 0)
            {
                NativeMethods.PostMessage(new HandleRef(this, m_ControlInternal.Handle),
                                          WinsockAPI.WM_WINSOCK_SENDCOMPLETE,
                                          IntPtr.Zero, IntPtr.Zero);
            }
            else
            {
                SendProgressEvent e = new SendProgressEvent(bytesSent, bytesRemaining);
                OnSendProgress(e);
            }
        }

        /// <summary>
        /// データをリモート コンピュータへ送信します。
        /// </summary>
        /// <param name="data">送信するデータを指定します。</param>
        public virtual void SendData(byte[] data)
        {
            m_SendBuffer.Clear();
            m_SendBuffer.Add(data);
            SendInternal();
        }

        /// <summary>
        /// データをリモート コンピュータへ送信します。
        /// </summary>
        /// <param name="data">送信するデータを指定します。</param>
        public virtual void SendData(string data)
        {
            // SHIFT JIS に変換して送信
            SendData(enc.GetBytes(data));
        }

        //------------------------------------------------------------------------------
        // イベントを起こす
        //------------------------------------------------------------------------------
        /// <summary>
        /// CloseEvent イベントを発生します。
        /// </summary>
        /// <param name="e">イベントデータを含む EventArgs。</param>
        protected virtual void OnCloseEvent(EventArgs e)
		{
            m_State = StateConstants.sckClosed;
            if (this.CloseEvent != null) this.CloseEvent(this, e);
        }

        /// <summary>
        /// ConnectEvent イベントを発生します。
        /// </summary>
        /// <param name="e">イベントデータを含む EventArgs。</param>
        protected virtual void OnConnectEvent(EventArgs e)
		{
            m_State = StateConstants.sckConnected;
            if (this.ConnectEvent != null) this.ConnectEvent(this, e);
        }

        /// <summary>
        /// ConnectionRequest イベントを発生します。
        /// </summary>
        /// <param name="e">イベントデータを含む ConnectionRequestEvent。</param>
        protected virtual void OnConnectionRequest(ConnectionRequestEvent e)
		{
            if (this.ConnectionRequest != null) this.ConnectionRequest(this, e);
		}

        private void OnDataArrivalCaller(DataArrivalEvent e)
        {
            if (e.bytesTotal > 0) OnDataArrival(e);
        }

        /// <summary>
        /// DataArrival イベントを発生します。
        /// </summary>
        /// <param name="e">イベントデータを含む DataArrivalEvent。</param>
        protected virtual void OnDataArrival(DataArrivalEvent e)
		{
            if (this.DataArrival != null) this.DataArrival(this, e);
		}

        private void OnErrorCaller(ErrorEvent e)
        {
            StateConstants backupState = m_State;
            try
            {
                m_State = StateConstants.sckError;
                OnError(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                m_State = backupState;
            }
        }

        /// <summary>
        /// Error イベントを発生します。
        /// </summary>
        /// <param name="e">イベントデータを含む ErrorEvent。</param>
        protected virtual void OnError(ErrorEvent e)
		{
            if (this.Error != null) this.Error(this, e);
        }

        /// <summary>
        /// SendComplete イベントを発生します。
        /// </summary>
        /// <param name="e">イベントデータを含む EventArgs。</param>
        protected virtual void OnSendComplete(EventArgs e)
		{
            if (this.SendComplete != null) this.SendComplete(this, e);
		}

        /// <summary>
        /// SendProgress イベントを発生します。
        /// </summary>
        /// <param name="e">イベントデータを含む SendProgressEvent。</param>
        protected virtual void OnSendProgress(SendProgressEvent e)
		{
            if (this.SendProgress != null) this.SendProgress(this, e);
		}

        /// <summary>
        /// リモート コンピュータが接続を閉じたときに発生します。
        /// </summary>
		public event EventHandler CloseEvent;

        /// <summary>
        /// 接続処理が完了したときに発生します。
        /// </summary>
		public event EventHandler ConnectEvent;

        /// <summary>
        /// リモート マシンが接続を要求してきたときに発生します。 
        /// </summary>
		public event ConnectionRequestEventHandler ConnectionRequest;

        /// <summary>
        /// 新しいデータが送られてきたときに発生します。
        /// </summary>
		public event DataArrivalEventHandler DataArrival;

        /// <summary>
        /// バックグラウンド処理でエラーが発生したとき (たとえば、接続に失敗したとき、バックグラウンドで実行している送信や受信に失敗したときなど) に発生します。
        /// </summary>
		public event ErrorEventHandler Error;

        /// <summary>
        /// 送信処理が完了したときに発生します。
        /// </summary>
		public event EventHandler SendComplete;

        /// <summary>
        /// データの送信中に発生します。
        /// </summary>
		public event SendProgressEventHandler SendProgress;

        internal void AsyncEvents(IntPtr socketHandle, IntPtr lParam)
        {
            // Winsock から送信される Windows メッセージは
            // wParam : ソケットハンドル
            // lParam : 上位ワードにはエラーコード
            //          下位ワードにはイベント

            if (socketHandle != SocketHandleInternal) return;

            int nEvent = NativeMethods.LOWORD(lParam);
            int errorCode = NativeMethods.HIWORD(lParam);

            if (errorCode == 0)
                switch (nEvent)
                {
                    case WinsockAPI.FD_ACCEPT:
                        {
                            ConnectionRequestEvent e = new ConnectionRequestEvent(socketHandle);
                            OnConnectionRequest(e);
                            break;
                        }

                    case WinsockAPI.FD_CLOSE:
                        {
                            OnCloseEvent(EventArgs.Empty);
                            break;
                        }

                    case WinsockAPI.FD_CONNECT:
                        {
                            OnConnectEvent(EventArgs.Empty);
                            break;
                        }

                    case WinsockAPI.FD_READ:
                        {
                            DataArrivalEvent e = new DataArrivalEvent(this.BytesReceived);
                            OnDataArrivalCaller(e);
                            break;
                        }

                    case WinsockAPI.FD_WRITE:
                        {
                            SendInternal();
                            break;
                        }

                    default:
                        break;
                }
            else
            {
                ErrorEvent e = new ErrorEvent(
                                        errorCode, 
                                        NativeMethods.GetErrorMsg(errorCode),
                                        errorCode,
                                        "MSWinsock",
                                        string.Empty,
                                        0,
                                        true);
                OnErrorCaller(e);
                if (!e.cancelDisplay)
                {
                    MessageBox.Show(
                            e.description,
                            e.source,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation,
                            MessageBoxDefaultButton.Button1);
                }
            }
        }

        internal class ControlInternal : Control
        {
            Winsock m_Owner;

            internal ControlInternal(Winsock owner)
            {
                m_Owner = owner;
            }

            [DebuggerStepThrough]
            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case WinsockAPI.WM_WINSOCK_MESSAGE:
                        NativeMethods.PostMessage(new HandleRef(this, this.Handle),
                                                  WinsockAPI.WM_WINSOCK_MESSAGE_ASYNC,
                                                  m.WParam, m.LParam);
                        break;

                    case WinsockAPI.WM_WINSOCK_MESSAGE_ASYNC:
                        m_Owner.AsyncEvents(m.WParam, m.LParam);
                        break;

                    case WinsockAPI.WM_WINSOCK_SENDCOMPLETE:
                        m_Owner.OnSendComplete(EventArgs.Empty);
                        break;

                    default:
                        base.WndProc(ref m);
                        break;
                }
            }
        }

        #region InitializeComponent 対策
        
        void ISupportInitialize.BeginInit()
        {
        }

        void ISupportInitialize.EndInit()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AxHost.State OcxState
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Point Location
        {
            get { return default(Point); }
            set { }
        }

        #endregion

    }
}