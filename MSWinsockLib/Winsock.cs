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
    /// Winsock �R���g���[���݊��R���|�[�l���g
    /// </summary>
	public class Winsock : Component , ISupportInitialize
	{
        static Encoding enc = Encoding.GetEncoding("SHIFT_JIS");

        IntPtr m_SocketHandle;                  // �\�P�b�g�n���h��

        StateConstants m_State;                 // �R���g���[���̏��

        ByteBuffer m_SendBuffer;                // ���M�o�b�t�@

        string m_RemoteHost;
        WinsockAPI.sockaddr_in m_Remote;        // �ڑ���� sockaddr_in
        WinsockAPI.sockaddr_in m_Local;         // ������ sockaddr_in

        ControlInternal m_ControlInternal;      // �T�u�N���X���̂��߂̃R���g���[��

        /// <summary>
        /// Winsock �R���|�[�l���g�̃C���X�^���X���쐬���܂��B
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
        /// Winsock �R���|�[�l���g�̃C���X�^���X���쐬���܂��B
        /// </summary>
        /// <param name="container">�R���e�i���w�肵�܂��B</param>
        public Winsock(IContainer container) : this()
        {
            container.Add(this);
        }

        //------------------------------------------------------------------------------
        // �\�P�b�g�쐬����Ă��Ȃ���΍쐬����
        //------------------------------------------------------------------------------
        internal IntPtr SocketHandleInternal
        {
            get 
            {
                if (m_SocketHandle == WinsockAPI.INVALID_SOCKET)
                {
                    // Socket ���쐬
                    m_SocketHandle = WinsockAPI.socket(WinsockAPI.AF_INET, WinsockAPI.SOCK_STREAM, WinsockAPI.IPPROTO_TCP);
                    if (m_SocketHandle == WinsockAPI.INVALID_SOCKET)
                    {
                        SocketError();
                    }

                    // �\�P�b�g��������
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

            // ��u���b�L���O
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
        // �G���[�R�[�h���� VB �̃G���[�𔭐�����
        //------------------------------------------------------------------------------
        private void SocketError()
        {
            SocketError(WinsockAPI.WSAGetLastError());
        }
        private void SocketError(int nErrCode)
        {
            switch (nErrCode)
            {
                // 0 �Ȃɂ����Ȃ�
                case 0:
                case WinsockAPI.WSAEWOULDBLOCK:
                    break;

                // �G���[�𔭐�
                default:
                    NativeMethods.RaiseError(nErrCode);
                    break;
            }
        }
        //------------------------------------------------------------------------------
        // Dispose �����Ƃ��A�\�P�b�g��j��
        //------------------------------------------------------------------------------
        /// <summary>
        /// Component �ɂ���Ďg�p����Ă���A���}�l�[�W ���\�[�X��������A�I�v�V�����Ń}�l�[�W ���\�[�X��������܂��B
        /// </summary>
        /// <param name="disposing">�}�l�[�W ���\�[�X�ƃA���}�l�[�W ���\�[�X�̗������������ꍇ�� true�B�A���}�l�[�W ���\�[�X�������������ꍇ�� false�B </param>
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
        /// ��M�����f�[�^ (���ݎ�M�o�b�t�@���ɂ���f�[�^) �̃T�C�Y��Ԃ��܂��B�f�[�^���擾����ɂ́AGetData ���\�b�h���g�p���܂��B
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
        /// �R���g���[���̏�Ԃ�\���񋓌^�̒l��Ԃ��܂��B
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
        /// ���[�J�� �}�V���̖��O��Ԃ��܂��B�l�̎擾�̂݉\�ł��B
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
        /// ���[�J�� �}�V���� IP �A�h���X���A�s���I�h�ŋ�؂�ꂽ�`�� (xxx.xxx.xxx.xxx) �� IP �A�h���X�ŕԂ��܂��B
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
        /// �g�p���郍�[�J�� �|�[�g��ݒ肵�܂��B�l�̎擾���\�ł��B
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

        // Connect �������AListen �J�n���ɌĂ�
        private void GetLocalAddr()
        {
            int nRet = 0;
            WinsockAPI.sockaddr_in local = WinsockAPI.sockaddr_in.Empty;
            int nSize = Marshal.SizeOf(local);

            // �����̏����擾(�g�p����IP�A�h���X/�|�[�g)
            nRet = WinsockAPI.getsockname(SocketHandleInternal, ref local, ref nSize);
            if (nRet == WinsockAPI.SOCKET_ERROR)
            {
                SocketError();
            }
            m_Local.sin_addr = local.sin_addr;
            m_Local.sin_port = local.sin_port;
        }

        /// <summary>
        /// �\�P�b�g�̃v���g�R�����擾�܂��͐ݒ肵�܂��B
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
        /// �����[�g�z�X�g���擾�܂��͐ݒ肵�܂��B
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
        /// �����[�g�z�X�g�� IP �A�h���X���擾���܂��B
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
        /// �����[�g�z�X�g�̃|�[�g���擾�܂��͐ݒ肵�܂��B
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
        /// �\�P�b�g�n���h�����擾���܂��B
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
        /// TCP �T�[�o�[ �A�v���P�[�V�����݂̂Ŏg�p�ł��܂��B���̃��\�b�h�́AConnectionRequest �C�x���g���ŁA�����Ă����ڑ��v�����󂯓����Ƃ��Ɏg�p���܂��B
        /// </summary>
        /// <param name="requestID">ConnectionRequest �C�x���g�� requestID ���w�肵�܂��B</param>
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
        /// �\�P�b�g�����̃|�[�g����� IP �A�h���X�Ɋ��蓖�Ă܂��B
        /// </summary>
		public virtual void Bind()
		{
            m_Local.sin_addr = WinsockAPI.INADDR_ANY;
            BindInternal(m_Local);
        }

        /// <summary>
        /// �\�P�b�g�����̃|�[�g����� IP �A�h���X�Ɋ��蓖�Ă܂��B
        /// </summary>
        /// <param name="localPort">�ڑ��Ɏg�p���郍�[�J�� �|�[�g</param>
        /// <param name="localIP">�ڑ��Ɏg�p���� IP �A�h���X</param>
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
        /// ���݂̐ڑ�����܂��B
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
        /// �����[�g�R���s���[�^�ɐڑ����܂��B
        /// </summary>
		public virtual void Connect()
		{
            ConnectInternal(m_Remote);
		}

        /// <summary>
        /// �����[�g�R���s���[�^�ɐڑ����܂��B
        /// </summary>
        /// <param name="remoteHost">�ڑ����郊���[�g �R���s���[�^�̖��O���w�肵�܂��B</param>
        /// <param name="remotePort">�ڑ����郊���[�g �R���s���[�^�̃|�[�g���w�肵�܂��B</param>
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
        /// ���݂̃f�[�^ �u���b�N���擾���A����� string�^�̕ϐ��Ɋi�[���܂��B
        /// </summary>
        /// <param name="data">���\�b�h�̎��s������Ɋ��������Ƃ��ɁA�擾�����f�[�^���i�[�����ꏊ���w�肵�܂��B</param>
        public virtual void GetData(out string data)
		{
            GetData(out data, Constants.vbString);
		}

        /// <summary>
        /// ���݂̃f�[�^ �u���b�N���擾���A����� string�^�̕ϐ��Ɋi�[���܂��B
        /// </summary>
        /// <param name="data">���\�b�h�̎��s������Ɋ��������Ƃ��ɁA�擾�����f�[�^���i�[�����ꏊ���w�肵�܂��B</param>
        /// <param name="type">�擾����f�[�^�̌^���w�肵�܂��B</param>
        public virtual void GetData(out string data, VariantType type)
		{
            GetData(out data, type, this.BytesReceived);
        }

        /// <summary>
        /// ���݂̃f�[�^ �u���b�N���擾���A����� string�^�̕ϐ��Ɋi�[���܂��B
        /// </summary>
        /// <param name="data">���\�b�h�̎��s������Ɋ��������Ƃ��ɁA�擾�����f�[�^���i�[�����ꏊ���w�肵�܂��B</param>
        /// <param name="type">�擾����f�[�^�̌^���w�肵�܂��B</param>
        /// <param name="maxLen">�K�v�ȃf�[�^�̃T�C�Y���w�肵�܂��B</param>
        public virtual void GetData(out string data, VariantType type, int maxLen)
        {
            byte[] byteData;
            GetData(out byteData, Constants.vbByte | Constants.vbArray, maxLen);
            data = enc.GetString(byteData);
        }

        /// <summary>
        /// ���݂̃f�[�^ �u���b�N���擾���A����� byte�^�z��̕ϐ��Ɋi�[���܂��B
        /// </summary>
        /// <param name="data">���\�b�h�̎��s������Ɋ��������Ƃ��ɁA�擾�����f�[�^���i�[�����ꏊ���w�肵�܂��B</param>
        public virtual void GetData(out byte[] data)
        {
            GetData(out data, Constants.vbString);
        }

        /// <summary>
        /// ���݂̃f�[�^ �u���b�N���擾���A����� byte�^�z��̕ϐ��Ɋi�[���܂��B
        /// </summary>
        /// <param name="data">���\�b�h�̎��s������Ɋ��������Ƃ��ɁA�擾�����f�[�^���i�[�����ꏊ���w�肵�܂��B</param>
        /// <param name="type">�擾����f�[�^�̌^���w�肵�܂��B</param>
        public virtual void GetData(out byte[] data, VariantType type)
        {
            GetData(out data, type, this.BytesReceived);
        }

        /// <summary>
        /// ���݂̃f�[�^ �u���b�N���擾���A����� byte�^�z��̕ϐ��Ɋi�[���܂��B
        /// </summary>
        /// <param name="data">���\�b�h�̎��s������Ɋ��������Ƃ��ɁA�擾�����f�[�^���i�[�����ꏊ���w�肵�܂��B</param>
        /// <param name="type">�擾����f�[�^�̌^���w�肵�܂��B</param>
        /// <param name="maxLen">�K�v�ȃf�[�^�̃T�C�Y���w�肵�܂��B</param>
        public virtual void GetData(out byte[] data, VariantType type, int maxLen)
        {
            RecvInternal(out data, type, maxLen, false, true);
        }

        //------------------------------------------------------------------------------
        // GetData, PeekData ����
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
        /// �\�P�b�g���쐬���A�ڑ��v�����󂯕t���郂�[�h�Ɉڍs���܂��B���̃��\�b�h�́ATCP �ڑ��ł����@�\���܂���B
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
        /// ���݂̃f�[�^ �u���b�N���擾���A����� string�^�̕ϐ��Ɋi�[���܂��B
        /// ���̃��\�b�h�͓��̓L���[����f�[�^���폜���܂���B
        /// </summary>
        /// <param name="data">���\�b�h�̎��s������Ɋ��������Ƃ��ɁA�擾�����f�[�^���i�[�����ꏊ���w�肵�܂��B</param>
        public void PeekData(out string data)
        {
            PeekData(out data, Constants.vbString);
        }

        /// <summary>
        /// ���݂̃f�[�^ �u���b�N���擾���A����� string�^�̕ϐ��Ɋi�[���܂��B
        /// ���̃��\�b�h�͓��̓L���[����f�[�^���폜���܂���B
        /// </summary>
        /// <param name="data">���\�b�h�̎��s������Ɋ��������Ƃ��ɁA�擾�����f�[�^���i�[�����ꏊ���w�肵�܂��B</param>
        /// <param name="type">�擾����f�[�^�̌^���w�肵�܂��B</param>
        public void PeekData(out string data, VariantType type)
        {
            PeekData(out data, type, this.BytesReceived);
        }

        /// <summary>
        /// ���݂̃f�[�^ �u���b�N���擾���A����� string�^�̕ϐ��Ɋi�[���܂��B
        /// ���̃��\�b�h�͓��̓L���[����f�[�^���폜���܂���B
        /// </summary>
        /// <param name="data">���\�b�h�̎��s������Ɋ��������Ƃ��ɁA�擾�����f�[�^���i�[�����ꏊ���w�肵�܂��B</param>
        /// <param name="type">�擾����f�[�^�̌^���w�肵�܂��B</param>
        /// <param name="maxLen">�K�v�ȃf�[�^�̃T�C�Y���w�肵�܂��B</param>
        public void PeekData(out string data, VariantType type, int maxLen)
        {
            byte[] byteData;
            PeekData(out byteData, Constants.vbByte | Constants.vbArray, maxLen);
            data = enc.GetString(byteData);
        }

        /// <summary>
        /// ���݂̃f�[�^ �u���b�N���擾���A����� byte�^�z��̕ϐ��Ɋi�[���܂��B
        /// ���̃��\�b�h�͓��̓L���[����f�[�^���폜���܂���B
        /// </summary>
        /// <param name="data">���\�b�h�̎��s������Ɋ��������Ƃ��ɁA�擾�����f�[�^���i�[�����ꏊ���w�肵�܂��B</param>
        public virtual void PeekData(out byte[] data)
        {
            PeekData(out data, Constants.vbByte | Constants.vbArray);
        }

        /// <summary>
        /// ���݂̃f�[�^ �u���b�N���擾���A����� byte�^�z��̕ϐ��Ɋi�[���܂��B
        /// ���̃��\�b�h�͓��̓L���[����f�[�^���폜���܂���B
        /// </summary>
        /// <param name="data">���\�b�h�̎��s������Ɋ��������Ƃ��ɁA�擾�����f�[�^���i�[�����ꏊ���w�肵�܂��B</param>
        /// <param name="type">�擾����f�[�^�̌^���w�肵�܂��B</param>
        public virtual void PeekData(out byte[] data, VariantType type)
        {
            PeekData(out data, type, this.BytesReceived);
        }

        /// <summary>
        /// ���݂̃f�[�^ �u���b�N���擾���A����� byte�^�z��̕ϐ��Ɋi�[���܂��B
        /// ���̃��\�b�h�͓��̓L���[����f�[�^���폜���܂���B
        /// </summary>
        /// <param name="data">���\�b�h�̎��s������Ɋ��������Ƃ��ɁA�擾�����f�[�^���i�[�����ꏊ���w�肵�܂��B</param>
        /// <param name="type">�擾����f�[�^�̌^���w�肵�܂��B</param>
        /// <param name="maxLen">�K�v�ȃf�[�^�̃T�C�Y���w�肵�܂��B</param>
        public virtual void PeekData(out byte[] data, VariantType type, int maxLen)
        {
            RecvInternal(out data, type, maxLen, true, true);
        }

        //------------------------------------------------------------------------------
        // �f�[�^���M
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

            // �C�x���g�𔭐�

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
        /// �f�[�^�������[�g �R���s���[�^�֑��M���܂��B
        /// </summary>
        /// <param name="data">���M����f�[�^���w�肵�܂��B</param>
        public virtual void SendData(byte[] data)
        {
            m_SendBuffer.Clear();
            m_SendBuffer.Add(data);
            SendInternal();
        }

        /// <summary>
        /// �f�[�^�������[�g �R���s���[�^�֑��M���܂��B
        /// </summary>
        /// <param name="data">���M����f�[�^���w�肵�܂��B</param>
        public virtual void SendData(string data)
        {
            // SHIFT JIS �ɕϊ����đ��M
            SendData(enc.GetBytes(data));
        }

        //------------------------------------------------------------------------------
        // �C�x���g���N����
        //------------------------------------------------------------------------------
        /// <summary>
        /// CloseEvent �C�x���g�𔭐����܂��B
        /// </summary>
        /// <param name="e">�C�x���g�f�[�^���܂� EventArgs�B</param>
        protected virtual void OnCloseEvent(EventArgs e)
		{
            m_State = StateConstants.sckClosed;
            if (this.CloseEvent != null) this.CloseEvent(this, e);
        }

        /// <summary>
        /// ConnectEvent �C�x���g�𔭐����܂��B
        /// </summary>
        /// <param name="e">�C�x���g�f�[�^���܂� EventArgs�B</param>
        protected virtual void OnConnectEvent(EventArgs e)
		{
            m_State = StateConstants.sckConnected;
            if (this.ConnectEvent != null) this.ConnectEvent(this, e);
        }

        /// <summary>
        /// ConnectionRequest �C�x���g�𔭐����܂��B
        /// </summary>
        /// <param name="e">�C�x���g�f�[�^���܂� ConnectionRequestEvent�B</param>
        protected virtual void OnConnectionRequest(ConnectionRequestEvent e)
		{
            if (this.ConnectionRequest != null) this.ConnectionRequest(this, e);
		}

        private void OnDataArrivalCaller(DataArrivalEvent e)
        {
            if (e.bytesTotal > 0) OnDataArrival(e);
        }

        /// <summary>
        /// DataArrival �C�x���g�𔭐����܂��B
        /// </summary>
        /// <param name="e">�C�x���g�f�[�^���܂� DataArrivalEvent�B</param>
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
        /// Error �C�x���g�𔭐����܂��B
        /// </summary>
        /// <param name="e">�C�x���g�f�[�^���܂� ErrorEvent�B</param>
        protected virtual void OnError(ErrorEvent e)
		{
            if (this.Error != null) this.Error(this, e);
        }

        /// <summary>
        /// SendComplete �C�x���g�𔭐����܂��B
        /// </summary>
        /// <param name="e">�C�x���g�f�[�^���܂� EventArgs�B</param>
        protected virtual void OnSendComplete(EventArgs e)
		{
            if (this.SendComplete != null) this.SendComplete(this, e);
		}

        /// <summary>
        /// SendProgress �C�x���g�𔭐����܂��B
        /// </summary>
        /// <param name="e">�C�x���g�f�[�^���܂� SendProgressEvent�B</param>
        protected virtual void OnSendProgress(SendProgressEvent e)
		{
            if (this.SendProgress != null) this.SendProgress(this, e);
		}

        /// <summary>
        /// �����[�g �R���s���[�^���ڑ�������Ƃ��ɔ������܂��B
        /// </summary>
		public event EventHandler CloseEvent;

        /// <summary>
        /// �ڑ����������������Ƃ��ɔ������܂��B
        /// </summary>
		public event EventHandler ConnectEvent;

        /// <summary>
        /// �����[�g �}�V�����ڑ���v�����Ă����Ƃ��ɔ������܂��B 
        /// </summary>
		public event ConnectionRequestEventHandler ConnectionRequest;

        /// <summary>
        /// �V�����f�[�^�������Ă����Ƃ��ɔ������܂��B
        /// </summary>
		public event DataArrivalEventHandler DataArrival;

        /// <summary>
        /// �o�b�N�O���E���h�����ŃG���[�����������Ƃ� (���Ƃ��΁A�ڑ��Ɏ��s�����Ƃ��A�o�b�N�O���E���h�Ŏ��s���Ă��鑗�M���M�Ɏ��s�����Ƃ��Ȃ�) �ɔ������܂��B
        /// </summary>
		public event ErrorEventHandler Error;

        /// <summary>
        /// ���M���������������Ƃ��ɔ������܂��B
        /// </summary>
		public event EventHandler SendComplete;

        /// <summary>
        /// �f�[�^�̑��M���ɔ������܂��B
        /// </summary>
		public event SendProgressEventHandler SendProgress;

        internal void AsyncEvents(IntPtr socketHandle, IntPtr lParam)
        {
            // Winsock ���瑗�M����� Windows ���b�Z�[�W��
            // wParam : �\�P�b�g�n���h��
            // lParam : ��ʃ��[�h�ɂ̓G���[�R�[�h
            //          ���ʃ��[�h�ɂ̓C�x���g

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

        #region InitializeComponent �΍�
        
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