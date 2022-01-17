using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace Scripting
{
    sealed internal class NativeMethods
    {
        private NativeMethods() { }

        [DllImport("mpr.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int WNetGetConnection(
            string lpLocalName,
            [Out]StringBuilder lpRemoteName,
            ref int lpnLength
        );

        public const uint SEM_FAILCRITICALERRORS = 0x1;

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern uint SetErrorMode(
            uint uMode
        );

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern uint GetShortPathName(
            string lpszLongPath,
            StringBuilder lpszShortPath,
            uint cchBuffer
        );

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern uint GetLongPathName(
            string lpszLongPath,
            StringBuilder lpszShortPath,
            uint cchBuffer
        );

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool GetVolumeInformation(
            string drive,
            [Out]StringBuilder volumeName,
            int volumeNameBufLen,
            out int volSerialNumber,
            out int maxFileNameLen,
            out int fileSystemFlags,
            [Out]StringBuilder fileSystemName,
            int fileSystemNameBufLen
        );

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool SetVolumeLabel(
            string driveLetter,
            string volumeName
        );

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool GetDiskFreeSpaceEx(
            string directoryName,
            out long freeBytesAvailable,
            out long TotalNumberOfBytes,
            out long TotalNumberOfFreeBytes
        );

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int GetDriveType(string drive);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern uint GetWindowsDirectory(
            [Out]StringBuilder buffer,
            uint length
        );

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WIN32_FILE_ATTRIBUTE_DATA
        {
            public FileAttribute dwFileAttributes;
            public FILETIME ftCreationTime;
            public FILETIME ftLastAccessTime;
            public FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
        }

        public enum GET_FILEEX_INFO_LEVELS
        {
            GetFileExInfoStandard,
            GetFileExMaxInfoLevel
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct FILETIME
        {
            public uint dwLowDateTime;
            public uint dwHighDateTime;

            public DateTime ToDate()
            {
                return ToDateUTC().ToLocalTime();
            }
            public DateTime ToDateUTC()
            {
                long fileTime = NativeWrapper.MAKELONG(dwLowDateTime, dwHighDateTime);
                return DateTime.FromFileTimeUtc(fileTime);
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WIN32_FIND_DATA
        {
            public FileAttribute dwFileAttributes;
            public FILETIME ftCreationTime;
            public FILETIME ftLastAccessTime;
            public FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            public uint dwReserved0;
            public uint dwReserved1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName;
        }

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr FindFirstFile(
            String fileName,
            ref WIN32_FIND_DATA data
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool FindNextFile(
            HandleRef hndFindFile,
            ref WIN32_FIND_DATA lpFindFileData
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool FindClose(HandleRef handle);

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern uint GetTempFileName(
            string tmpPath, 
            string prefix, 
            uint uniqueIdOrZero,
            StringBuilder tmpFileName
        );

        internal const int MOVEFILE_REPLACE_EXISTING = 0x1;
        internal const int MOVEFILE_DELAY_UNTIL_REBOOT = 0x4;
        internal const int MOVEFILE_COPY_ALLOWED = 0x2;

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool MoveFileEx(
            string lpExistingFileName,
            string lpNewFileName,
            int dwFlags
        );

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool CopyFile(
            string source,
            string destination,
            bool failIfExists
        );

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool CreateDirectory(
            string fileName,
            IntPtr lpSecurityAttributes
        );

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool GetFileAttributesEx(
            string fileName,
            GET_FILEEX_INFO_LEVELS  fInfoLevelId,
            HandleRef lpFileInformation
        );

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool SetFileAttributes(
            string fileName,
            int dwFileAttributes
        );

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool RemoveDirectory(string fileName);

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool DeleteFile(string fileName);

        public const int FORMAT_MESSAGE_IGNORE_INSERTS  = 0x0200;
        public const int FORMAT_MESSAGE_FROM_SYSTEM     = 0x1000;
        public const int FORMAT_MESSAGE_MAX_WIDTH_MASK  = 0x00FF;
        public const int PROCESS_DEFAULT_LANGUAGE = 0x400;

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int FormatMessage(
              int dwFlags,
              string lpSource,
              int dwMessageId,
              int dwLanguageId,
              StringBuilder lpBuffer,
              int nSize,
              IntPtr Arguments
        );

        public const int INVALID_HANDLE_VALUE = -1;
        public const int NO_ERROR = 0x0;
        public const int ERROR_INVALID_FUNCTION = 0x00000001;
        public const int ERROR_FILE_NOT_FOUND   = 0x00000002;
        public const int ERROR_PATH_NOT_FOUND   = 0x00000003;
        public const int ERROR_NOT_READY        = 0x00000015;
        public const int ERROR_BAD_PATHNAME     = 0x000000A1;
        public const int ERROR_ALREADY_EXISTS   = 0x000000B7;

        // SHGetFileInfo関数で使用するフラグ
        public const uint SHGFI_ICON = 0x100; // アイコン・リソースの取得
        public const uint SHGFI_LARGEICON = 0x0; // 大きいアイコン
        public const uint SHGFI_SMALLICON = 0x1; // 小さいアイコン
        public const uint SHGFI_TYPENAME = 0x400;//ファイルの種類

        // SHGetFileInfo関数で使用する構造体
        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        [DllImport("shell32.dll")]
        internal static extern IntPtr SHGetFileInfo(
            string pszPath, uint dwFileAttributes,
            ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, EntryPoint="#195", SetLastError = true)]
        internal static extern void SHFree(IntPtr pointer);

        /*---------------------------------------------------------------------------------
            CreateFile
        ---------------------------------------------------------------------------------*/
        public const uint
            // 
            GENERIC_READ = 0x80000000,
            GENERIC_WRITE = 0x40000000,


            FILE_SHARE_READ = 0x00000001,
            FILE_SHARE_WRITE = 0x00000002,

            CREATE_NEW = 1,
            CREATE_ALWAYS = 2,
            OPEN_EXISTING = 3,
            OPEN_ALWAYS = 4,
            TRUNCATE_EXISTING = 5
        ;

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr CreateFile
        (
            [MarshalAs(UnmanagedType.LPWStr)] string FileName,
            [MarshalAs(UnmanagedType.U4)] uint DesiredAccess,
            [MarshalAs(UnmanagedType.U4)] uint ShareMode,
            IntPtr SecurityAttributes,
            [MarshalAs(UnmanagedType.U4)] uint CreationDisposition,
            [MarshalAs(UnmanagedType.U4)] uint FlagsAndAttributes,
            IntPtr hTemplateFile
        );

    }
}
