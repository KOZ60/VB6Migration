using System;
using System.Runtime.InteropServices;
using System.Net;

namespace MSWinsockLib
{
    internal class WinsockAPI
    {

        static WinsockAPI()
        {
            WSAData udtWSAdata;
            WSAStartup(0x2, out udtWSAdata);
        }

        private WinsockAPI() { }

        //
        //   WinSock 関連
        //
        public const int SOL_SOCKET = 0xffff;
        public const int SO_REUSEADDR = 0x4;
        public const int IPPROTO_TCP = 6;
        public const int IPPROTO_UDP = 17;
        public const int INADDR_NONE = -1;
        public const int INADDR_ANY = 0x0;
        public const int MSG_PEEK = 2;
        public const int WM_APP = 0x8000;
        public const int WM_WINSOCK_MESSAGE = WM_APP + 1;
        public const int WM_WINSOCK_MESSAGE_ASYNC = WM_APP + 2;
        public const int WM_WINSOCK_SENDCOMPLETE = WM_APP + 3;

        [StructLayout(LayoutKind.Sequential)]
        public struct HostEnt
        {
            public IntPtr h_name;
            public IntPtr h_aliases;
            public short h_addrtype;
            public short h_length;
            public IntPtr h_addr_list;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct sockaddr_in
        {
            public short sin_family;
            public ushort sin_port;
            public int sin_addr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] sin_zero;

            public sockaddr_in(string Host, int Port)
            {
                sin_family = AF_INET;
                sin_port = htons(unchecked((ushort)Port));
                sin_addr = WinsockAPI.GetHostByNameAlias(Host);
                sin_zero = new byte[8];
            }

            public static sockaddr_in Empty
            {
                get
                {
                    sockaddr_in target = new sockaddr_in();
                    target.sin_family = AF_INET;
                    target.sin_port = 0;
                    target.sin_addr = 0;
                    target.sin_zero = new byte[8];
                    return target;
                }
            }

        }

        public const int WSADESCRIPTION_LEN = 256;
        public const int WSASYSSTATUS_LEN = 128;

        [StructLayout(LayoutKind.Sequential)]
        public struct WSAData
        {
            public Int16 version;
            public Int16 highVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = WSADESCRIPTION_LEN + 1)]
            public String description;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = WSASYSSTATUS_LEN + 1)]
            public String systemStatus;
            public Int16 maxSockets;
            public Int16 maxUdpDg;
            public IntPtr vendorInfo;
        }


        public static IntPtr INVALID_SOCKET = (IntPtr)(-1);
        public const int SOCKET_ERROR = -1;
        public const int SOCK_STREAM = 1;
        public const int SOCK_DGRAM = 2;
        public const int MAXGETHOSTSTRUCT = 1024;
        public const int AF_INET = 2;
        public const int PF_INET = 2;
        public const int SOMAXCONN = 0x7fffffff;

        // Winsock エラー
        public const int WSABASEERR = 10000;
        public const int WSAEINTR = WSABASEERR + 4;
        public const int WSAEBADF = WSABASEERR + 9;
        public const int WSAEACCES = WSABASEERR + 13;
        public const int WSAEFAULT = WSABASEERR + 14;
        public const int WSAEINVAL = WSABASEERR + 22;
        public const int WSAEMFILE = WSABASEERR + 24;
        ///*
        // * Windows Sockets definitions of regular Berkeley error constants
        // */
        public const int WSAEWOULDBLOCK = WSABASEERR + 35;
        public const int WSAEINPROGRESS = WSABASEERR + 36;
        public const int WSAEALREADY = WSABASEERR + 37;
        public const int WSAENOTSOCK = WSABASEERR + 38;
        public const int WSAEDESTADDRREQ = WSABASEERR + 39;
        public const int WSAEMSGSIZE = WSABASEERR + 40;
        public const int WSAEPROTOTYPE = WSABASEERR + 41;
        public const int WSAENOPROTOOPT = WSABASEERR + 42;
        public const int WSAEPROTONOSUPPORT = WSABASEERR + 43;
        public const int WSAESOCKTNOSUPPORT = WSABASEERR + 44;
        public const int WSAEOPNOTSUPP = WSABASEERR + 45;
        public const int WSAEPFNOSUPPORT = WSABASEERR + 46;
        public const int WSAEAFNOSUPPORT = WSABASEERR + 47;
        public const int WSAEADDRINUSE = WSABASEERR + 48;
        public const int WSAEADDRNOTAVAIL = WSABASEERR + 49;
        public const int WSAENETDOWN = WSABASEERR + 50;
        public const int WSAENETUNREACH = WSABASEERR + 51;
        public const int WSAENETRESET = WSABASEERR + 52;
        public const int WSAECONNABORTED = WSABASEERR + 53;
        public const int WSAECONNRESET = WSABASEERR + 54;
        public const int WSAENOBUFS = WSABASEERR + 55;
        public const int WSAEISCONN = WSABASEERR + 56;
        public const int WSAENOTCONN = WSABASEERR + 57;
        public const int WSAESHUTDOWN = WSABASEERR + 58;
        public const int WSAETOOMANYREFS = WSABASEERR + 59;
        public const int WSAETIMEDOUT = WSABASEERR + 60;
        public const int WSAECONNREFUSED = WSABASEERR + 61;
        public const int WSAELOOP = WSABASEERR + 62;
        public const int WSAENAMETOOLONG = WSABASEERR + 63;
        public const int WSAEHOSTDOWN = WSABASEERR + 64;
        public const int WSAEHOSTUNREACH = WSABASEERR + 65;
        public const int WSAENOTEMPTY = WSABASEERR + 66;
        public const int WSAEPROCLIM = WSABASEERR + 67;
        public const int WSAEUSERS = WSABASEERR + 68;
        public const int WSAEDQUOT = WSABASEERR + 69;
        public const int WSAESTALE = WSABASEERR + 70;
        public const int WSAEREMOTE = WSABASEERR + 71;
        public const int WSAEDISCON = WSABASEERR + 101;
        ///*
        // * Extended Windows Sockets error constant definitions
        // */
        public const int WSASYSNOTREADY = WSABASEERR + 91;
        public const int WSAVERNOTSUPPORTED = WSABASEERR + 92;
        public const int WSANOTINITIALISED = WSABASEERR + 93;
        ///* Authoritative Answer: Host not found */
        public const int WSAHOST_NOT_FOUND = WSABASEERR + 1001;
        public const int HOST_NOT_FOUND = WSAHOST_NOT_FOUND;
        ///* Non-Authoritative: Host not found, or SERVERFAIL */
        public const int WSATRY_AGAIN = WSABASEERR + 1002;
        public const int TRY_AGAIN = WSATRY_AGAIN;
        ///* Non recoverable errors, FORMERR, REFUSED, NOTIMP */
        public const int WSANO_RECOVERY = WSABASEERR + 1003;
        public const int NO_RECOVERY = WSANO_RECOVERY;
        ///* Valid name, no data record of requested type */
        public const int WSANO_DATA = WSABASEERR + 1004;
        public const int NO_DATA = WSANO_DATA;
        ///* no address, look for MX record */
        public const int WSANO_ADDRESS = WSANO_DATA;
        public const int NO_ADDRESS = WSANO_ADDRESS;

        public const int WSA_IO_INCOMPLETE = 996;
        public const int WSA_IO_PENDING = 997;
        //ioctl
        public const int FIONREAD = unchecked((int)0x8004667f);
        public const int FIONBIO = unchecked((int)0x8004667e);
        public const int FIOASYNC = unchecked((int)0x8004667d);

        //非同期通知メッセージ
        public const int FD_ACCEPT = 0x8;
        public const int FD_CLOSE = 0x20;
        public const int FD_CONNECT = 0x10;
        public const int FD_READ = 0x1;
        public const int FD_WRITE = 0x2;

        [StructLayout(LayoutKind.Sequential)]
        public struct Inet_Address
        {
            public byte Byte4;
            public byte Byte3;
            public byte Byte2;
            public byte Byte1;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WSAOVERLAPPED
        {
            public int Internal;
            public int InternalHigh;
            public int Offset;
            public int OffsetHigh;
            public int hEvent;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WSABUF
        {
            public int Length;
            public IntPtr buf;
        }

        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int WSAStartup(int wVR, out WSAData lpWSAD);
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int WSAAsyncSelect(IntPtr s, IntPtr hwnd, int wMsg, long lngEvent);
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int WSACleanup();
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int WSAGetLastError();

        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int WSACreateEvent();
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool WSASetEvent(IntPtr hEvent);
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool WSAResetEvent(IntPtr hEvent);
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool WSACloseEvent(IntPtr hEvent);

        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int WSARecv(IntPtr s, ref WSABUF lpBuffers, int dwBufferCount, ref int lpNumberOfBytesRecvd, ref int lpFlags, ref WSAOVERLAPPED over, int lpCompletionRoutine);
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int WSACancelBlockingCall();
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int WSAGetOverlappedResult(IntPtr s, ref WSAOVERLAPPED over, ref int lpcbTransfer, int fwait, ref int dwFlags);

        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int gethostbyaddr(ref int addr, int addr_len, int addr_type);
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr gethostbyname(string host_name);
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int gethostname(string host_name, int namelen);
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int getservbyname(string serv_name, string proto);
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int getprotobynumber(int proto);
        [DllImport("ws2_32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int getprotobyname(string proto_name);
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int getservbyport(short Port, int proto);

        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern uint inet_addr(string cp);
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int inet_ntoa(int inn);
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int htonl(int hostlong);
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int ntohl(int netlong);
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern ushort htons(ushort hostshort);

        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern ushort ntohs(ushort netshort);
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr socket(int af, int s_type, int Protocol);

        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int closesocket(IntPtr s);
        
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int connect(IntPtr s, ref sockaddr_in Name, int namelen);

        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int bind(IntPtr s, ref sockaddr_in Name, ref int namelen);
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int recv(IntPtr s, IntPtr buf, int buflen, int flags);
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int send(IntPtr s, IntPtr buf, int buflen, int flags);
        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int listen(IntPtr s, int backlog);
        [DllImport("ws2_32.dll",CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr accept(IntPtr s, ref sockaddr_in addr, ref int addrlen);

        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int getsockname(IntPtr s, ref sockaddr_in Name, ref int namelen);

        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int getpeername(IntPtr s, ref sockaddr_in Name, ref int namelen);

        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int setsockopt(IntPtr s, int level, int optname, ref int optval, int optlen);

        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int getsockopt(IntPtr s, int level, int optname, ref int optval, ref int optlen);

        [DllImport("Ws2_32.dll")]
        public static extern int ioctlsocket(IntPtr s, int cmd, ref int argp);

        public const int SD_RECEIVE = 0x0;
        public const int SD_SEND = 0x1;
        public const int SD_BOTH = 0x2;

        [DllImport("ws2_32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int shutdown(IntPtr s, int how);

        public static int GetHostByNameAlias(string HostName)
        {
            int ipAddr = unchecked((int)inet_addr(HostName));
            if (ipAddr == INADDR_NONE)
            {
                IntPtr lngRet = WinsockAPI.gethostbyname(HostName);
                if (lngRet != IntPtr.Zero)
                {
                    HostEnt host = (HostEnt)Marshal.PtrToStructure(lngRet, typeof(HostEnt));
                    IntPtr addr = Marshal.ReadIntPtr(host.h_addr_list);
                    return Marshal.ReadInt32(addr);
                }
                else
                {
                    return INADDR_NONE;
                }
            }
            return ipAddr;
        }

        public static string InetAddrToString(int lngAddress)
        {
            IntPtr buffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Inet_Address)));
            Marshal.WriteInt32(buffer, lngAddress);

            Inet_Address udtIPAddress = (Inet_Address)Marshal.PtrToStructure(buffer, typeof(Inet_Address));

            return Convert.ToString(udtIPAddress.Byte4) + "."
                 + Convert.ToString(udtIPAddress.Byte3) + "."
                 + Convert.ToString(udtIPAddress.Byte2) + "."
                 + Convert.ToString(udtIPAddress.Byte1);
        }

    }
}
