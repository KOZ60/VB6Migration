using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Scripting
{
    sealed internal class NativeWrapper
    {
        private NativeWrapper(){}

        // FD 等の場合、OS が勝手にダイアログを表示してしまうのでエラーモードを制御する

        private class ErrorMode : IDisposable
        {
            uint _oldMode;

            public ErrorMode()
            {
                _oldMode = NativeMethods.SetErrorMode(NativeMethods.SEM_FAILCRITICALERRORS);
            }

            public void Dispose()
            {
                NativeMethods.SetErrorMode(_oldMode);
            }
        }


        public static WidePath GetShortPathName(WidePath fullPathName)
        {
            using (StringBuilderCache sb = new StringBuilderCache())
            {
                using (ErrorMode mode = new ErrorMode())
                {
                    uint ret = NativeMethods.GetShortPathName(fullPathName.Win32, sb, (uint)sb.Capacity);
                    if (ret == 0)
                        return string.Empty;
                    else
                        return sb.ToString();
                }
            }
        }

        public static WidePath GetLongPathName(WidePath fullPathName)
        {
            using (StringBuilderCache sb = new StringBuilderCache())
            {
                using (ErrorMode mode = new ErrorMode())
                {
                    uint ret = NativeMethods.GetLongPathName(fullPathName.Win32, sb, (uint)sb.Capacity);
                    if (ret == 0)
                        return string.Empty;
                    else
                        return sb.ToString();
                }
            }
        }

        public class VolumeInformation
        {
            public string VolumeName;
            public int SerialNumber;
            public int MaxFileNameLen;
            public int FileSystemFlags;
            public string FileSystemName;

            internal VolumeInformation() {}
        }

        public static bool GetVolumeInformation(WidePath drive, out VolumeInformation volumeInfo)
        {
            using (StringBuilderCache volumeName = new StringBuilderCache())
            {
                using (StringBuilderCache fileSystemName = new StringBuilderCache())
                {
                    using (ErrorMode mode = new ErrorMode())
                    {
                        int serialNumber;
                        int maxFileNameLen;
                        int fileSystemFlags;

                        bool result = NativeMethods.GetVolumeInformation(
                                drive.VolumeInfoName,
                                volumeName,
                                volumeName.Capacity,
                                out serialNumber,
                                out maxFileNameLen,
                                out fileSystemFlags,
                                fileSystemName,
                                fileSystemName.Capacity
                                );

                        volumeInfo = new VolumeInformation();

                        if (result)
                        {
                            volumeInfo.VolumeName = volumeName.ToString();
                            volumeInfo.SerialNumber = serialNumber;
                            volumeInfo.MaxFileNameLen = maxFileNameLen;
                            volumeInfo.FileSystemFlags = fileSystemFlags;
                            volumeInfo.FileSystemName = fileSystemName.ToString();
                        }

                        return result;
                    }
                }
            }
        }

        public static bool SetVolumeLabel(WidePath driveLetter, string volumeName)
        {
            using (ErrorMode mode = new ErrorMode())
            {
                bool r = NativeMethods.SetVolumeLabel(driveLetter.Display, volumeName);
                if (!r)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw CreateIOException(errorCode);
                }
                return r;
            }
        }

        public static WidePath WNetGetConnection(WidePath lpLocalName)
        {
            using (ErrorMode mode = new ErrorMode())
            {
                using (StringBuilderCache shareName = new StringBuilderCache())
                {
                    int nSize = shareName.Capacity;
                    int errorCode = NativeMethods.WNetGetConnection(lpLocalName.Display, shareName, ref nSize);
                    if (errorCode != 0)
                        return string.Empty;
                    else
                        return shareName.ToString();
                }
            }
        }

        public static bool GetDiskFreeSpaceEx(
            WidePath directoryName,
            out long freeBytesAvailable,
            out long TotalNumberOfBytes,
            out long TotalNumberOfFreeBytes
            )
        {
            using (ErrorMode mode = new ErrorMode())
            {
                return NativeMethods.GetDiskFreeSpaceEx(
                    directoryName.Win32,
                    out freeBytesAvailable,
                    out TotalNumberOfBytes,
                    out TotalNumberOfFreeBytes
                );
            }
        }

        public static DriveType GetDriveType(WidePath drive)
        {
            using (ErrorMode mode = new ErrorMode())
            {
                return (DriveType)NativeMethods.GetDriveType(drive.VolumeInfoName);
            }
        }

        public static string GetWindowsDirectory()
        {
            using (ErrorMode mode = new ErrorMode())
            {
                using (StringBuilderCache sb = new StringBuilderCache())
                {
                    uint result = NativeMethods.GetWindowsDirectory(sb, (uint)sb.Capacity);
                    return sb.ToString();
                }
            }
        }

        public static long MAKELONG(uint low, uint high)
        {
            long longValue = ((long)high << 32) | low;
            return longValue;
        }

        public static IntPtr FindFirstFile(
                    WidePath fileName,
                    ref NativeMethods.WIN32_FIND_DATA data)
        {
            using (ErrorMode mode = new ErrorMode())
            {
                return NativeMethods.FindFirstFile(
                    fileName.Win32,
                    ref data);
            }
        }

        public static bool FindNextFile(
                    HandleRef hndFindFile,
                    ref NativeMethods.WIN32_FIND_DATA lpFindFileData)
        {
            using (ErrorMode mode = new ErrorMode())
            {
                return NativeMethods.FindNextFile(hndFindFile, ref lpFindFileData);
            }
        }

        public static bool FindClose(HandleRef handle)
        {
            using (ErrorMode mode = new ErrorMode())
            {
                return NativeMethods.FindClose(handle);
            }
        }

        public static long EnumFolderSize(WidePath folderName)
        {
            using (ErrorMode mode = new ErrorMode())
            {
                long nSize = 0;
                NativeMethods.WIN32_FIND_DATA fd = new NativeMethods.WIN32_FIND_DATA();

                IntPtr handle = NativeWrapper.FindFirstFile(WidePath.BuildPath(folderName.Display, "*"), ref fd);
                if (handle == (IntPtr)NativeMethods.INVALID_HANDLE_VALUE)
                    return 0;

                HandleRef handleRef = new HandleRef(new object(), handle);
                List<string> subFolders = new List<string>();

                do
                {
                    if (string.Compare(fd.cFileName, ".") != 0 && string.Compare(fd.cFileName, "..") != 0)
                    {
                        if (IsMask((uint)fd.dwFileAttributes, (uint)FileAttribute.Directory))
                            subFolders.Add(fd.cFileName);
                        else
                            nSize += NativeWrapper.MAKELONG(fd.nFileSizeLow, fd.nFileSizeHigh);
                    }

                } while (NativeWrapper.FindNextFile(handleRef, ref fd));

                bool result = NativeWrapper.FindClose(handleRef);

                foreach (string subFolder in subFolders)
                {
                    nSize += EnumFolderSize(WidePath.BuildPath(folderName.Display, subFolder));
                }
                return nSize;
            }
        }

        public static bool IsMask(int target, int mask)
        {
            return (target & mask) == mask;
        }

        public static bool IsMask(uint target, uint mask)
        {
            return (target & mask) == mask;
        }

        public static void CopyFile(WidePath source, WidePath destination, bool overWrite)
        {
            using (ErrorMode mode = new ErrorMode())
            {
                using (RenameFileForDuplication ren = new RenameFileForDuplication(destination))
                {
                    bool r = NativeMethods.CopyFile(
                                    source.Win32,
                                    destination.Win32,
                                    !overWrite);
                    if (!r)
                    {
                        int errorCode = Marshal.GetLastWin32Error();
                        throw CreateIOException(errorCode);
                    }
                }
            }
        }

        // 8.3形式ファイル名が被ったときのためファイル名を一時変更して戻すクラス

        private class RenameFileForDuplication : IDisposable
        {
            WidePath tempFile = null;
            WidePath restoreFileName = null;

            public RenameFileForDuplication(WidePath destination)
            {
                // コピー先に短いファイル名が存在し、
                // 短いファイル名と長いファイル名が違う場合
                // 上書きしないようコピー先のファイルをリネーム


                string fileName = WidePath.GetFileName(destination.Display);
                string shortFileName = WidePath.GetFileName(destination.ShortPathName);
                string longFileName = WidePath.GetFileName(destination.LongPathName);

                if (string.Compare(fileName, shortFileName, true) == 0 &&
                    string.Compare(longFileName, shortFileName, true) != 0)
                {
                    restoreFileName = destination.LongPathName;
                    tempFile = RenameToTempFile(restoreFileName);
                }
            }

            private static WidePath RenameToTempFile(WidePath target)
            {
                while (true)
                {
                    // 一時ファイル名を作成
                    WidePath tempFileName = CreateTempFileName(target);

                    // 作成された一時ファイル名に変更
                    bool r = NativeMethods.MoveFileEx(
                                        target.Win32,
                                        tempFileName.Win32,
                                        0);

                    // 成功したらテンポラリの名前を返す

                    if (r) return tempFileName;

                    // すでにファイルが存在している場合はリトライ
                    // それ以外は例外を Throw

                    int errorCode = Marshal.GetLastWin32Error();
                    if (errorCode != NativeMethods.ERROR_ALREADY_EXISTS)
                        throw CreateIOException(errorCode);
                }
            }

            // 一時ファイル名を作成

            private static WidePath CreateTempFileName(WidePath target)
            {
                WidePath targetPath = WidePath.GetParentFolderName(target.Display);
                WidePath tempFileName;
                uint uniqueId = 0;
                do
                {
                    using (StringBuilderCache sb = new StringBuilderCache())
                    {
                        uniqueId++;
                        uint result = NativeMethods.GetTempFileName(
                                            targetPath.Win32,
                                            "_",
                                            uniqueId,
                                            sb);
                        if (result == 0)
                        {
                            int errorCode = Marshal.GetLastWin32Error();
                            throw CreateIOException(errorCode);
                        }
                        tempFileName = sb.ToString();
                    }

                } while (string.IsNullOrEmpty(tempFileName.LongPathName)
                            && tempFileName.Win32 == target.Win32);

                return tempFileName;
            }

            public void Dispose()
            {
                if (tempFile != null)
                {
                    bool r = NativeMethods.MoveFileEx(
                                        tempFile.Win32,
                                        restoreFileName.Win32, 0);
                    if (!r)
                    {
                        int errorCode = Marshal.GetLastWin32Error();
                        throw CreateIOException(errorCode);
                    }
                }
                tempFile = null;
            }
        }

        // ファイル名の変更

        public static void RenameFile(WidePath source, WidePath destination)
        {
            using (ErrorMode mode = new ErrorMode())
            {
                bool r = NativeMethods.MoveFileEx(source.Win32, destination.Win32, 0);
                if (!r)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw CreateIOException(errorCode);
                }
            }
        }

        // ファイル移動

        public static void MoveFileEx(WidePath source, WidePath destination)
        {
            using (ErrorMode mode = new ErrorMode())
            {
                bool r = NativeMethods.MoveFileEx(
                                        source.Win32,
                                        destination.Win32,
                                        NativeMethods.MOVEFILE_COPY_ALLOWED);
                if (!r)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw CreateIOException(errorCode);
                }
            }
        }

        public static bool GetFileAttributesEx(
            WidePath fileName, 
            ref NativeMethods.WIN32_FILE_ATTRIBUTE_DATA data
        )
        {

            IntPtr pointer = Marshal.AllocHGlobal(Marshal.SizeOf(data));
            try
            {
                using (ErrorMode mode = new ErrorMode())
                {
                    bool r = NativeMethods.GetFileAttributesEx(
                                fileName.Win32,
                                NativeMethods.GET_FILEEX_INFO_LEVELS.GetFileExInfoStandard,
                                new HandleRef(new object(), pointer)
                            );

                    if (!r)
                    {
                        int errorCode = Marshal.GetLastWin32Error();
                        throw CreateIOException(errorCode);
                    }

                    data = (NativeMethods.WIN32_FILE_ATTRIBUTE_DATA)Marshal.PtrToStructure(
                                pointer,
                                typeof(NativeMethods.WIN32_FILE_ATTRIBUTE_DATA));


                    return r;
                }
            }
            finally
            {
                Marshal.FreeHGlobal(pointer);
            }
        }


        public static bool SetFileAttributes(
            WidePath fileName,
            FileAttribute dwAttributes
        )
        {
            using (ErrorMode mode = new ErrorMode())
            {
                bool r = NativeMethods.SetFileAttributes(fileName.Win32, (int)dwAttributes);
                if (!r)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw CreateIOException(errorCode);
                }
                return r;
            }
        }

        public static bool CreateDirectory(
            WidePath fileName
        )
        {
            using (ErrorMode mode = new ErrorMode())
            {
                bool r = NativeMethods.CreateDirectory(fileName.Win32, IntPtr.Zero);
                if (!r)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw CreateIOException(errorCode);
                }
                return r;
            }
        }

        public static bool RemoveDirectory(
            WidePath fileName
        )
        {
            using (ErrorMode mode = new ErrorMode())
            {
                bool r = NativeMethods.RemoveDirectory(fileName.Win32);
                if (!r)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw CreateIOException(errorCode);
                }
                return r;
            }
        }

        public static bool DeleteFile(
            WidePath fileName
        )
        {
            using (ErrorMode mode = new ErrorMode())
            {
                bool r = NativeMethods.DeleteFile(fileName.Win32);
                if (!r)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw CreateIOException(errorCode);
                }
                return r;
            }
        }

        public static Exception CreateIOException(int win32Error)
        {
            using (StringBuilderCache sb = new StringBuilderCache())
            {
                int result = NativeMethods.FormatMessage(
                                NativeMethods.FORMAT_MESSAGE_FROM_SYSTEM | NativeMethods.FORMAT_MESSAGE_IGNORE_INSERTS,
                                null,
                                win32Error,
                                NativeMethods.PROCESS_DEFAULT_LANGUAGE,
                                sb,
                                sb.Capacity,
                                IntPtr.Zero
                            );
                string errMsg;
                if (result > 0)
                    errMsg = sb.ToString();
                else
                    errMsg = string.Format("IO エラーが発生しました。エラーコード={0}", win32Error);
                return new IOException(errMsg);
            }
        }

        internal static int FillAttributeInfo(WidePath path, ref NativeMethods.WIN32_FILE_ATTRIBUTE_DATA data, bool tryagain, bool returnErrorOnNotFound)
        {
            int dataInitialised = 0;
            if (tryagain) 
            {
                NativeMethods.WIN32_FIND_DATA findData;
                findData = new NativeMethods.WIN32_FIND_DATA();

                using (ErrorMode mode = new ErrorMode())
                {
                    IntPtr handle = NativeMethods.FindFirstFile(path.Win32, ref findData);
                    if (handle == (IntPtr)NativeMethods.INVALID_HANDLE_VALUE)
                    {
                        dataInitialised = Marshal.GetLastWin32Error();
                        switch (dataInitialised)
                        {
                            case NativeMethods.ERROR_FILE_NOT_FOUND:
                            case NativeMethods.ERROR_PATH_NOT_FOUND:
                            case NativeMethods.ERROR_NOT_READY:          // floppy device not ready
                                if (!returnErrorOnNotFound)
                                {
                                    dataInitialised = 0;
                                    data.dwFileAttributes = (FileAttribute)(-1);
                                }
                                break;
                        }
                        return dataInitialised;
                    }
                    else
                    {
                        NativeMethods.FindClose(new HandleRef(new object(), handle));
                    }
                }

                data.dwFileAttributes = findData.dwFileAttributes;
                data.ftCreationTime = findData.ftCreationTime;
                data.ftLastAccessTime = findData.ftLastAccessTime;
                data.ftLastWriteTime = findData.ftLastWriteTime;
                data.nFileSizeHigh = findData.nFileSizeHigh;
                data.nFileSizeLow = findData.nFileSizeLow;
            }
            else
            {

                IntPtr pointer = Marshal.AllocHGlobal(Marshal.SizeOf(data));
                try
                {
                    using (ErrorMode mode = new ErrorMode())
                    {
                        bool r = NativeMethods.GetFileAttributesEx(
                                    path.Win32,
                                    NativeMethods.GET_FILEEX_INFO_LEVELS.GetFileExInfoStandard,
                                    new HandleRef(new object(), pointer)
                                );

                        if (!r)
                        {
                            dataInitialised = Marshal.GetLastWin32Error();
                            switch (dataInitialised)
                            {
                                case NativeMethods.ERROR_FILE_NOT_FOUND:
                                case NativeMethods.ERROR_PATH_NOT_FOUND:
                                case NativeMethods.ERROR_NOT_READY:
                                    return FillAttributeInfo(path, ref data, true, returnErrorOnNotFound);

                                default:
                                    if (!returnErrorOnNotFound)
                                    {
                                        dataInitialised = 0;
                                        data.dwFileAttributes = (FileAttribute)(-1);
                                    }
                                    break;
                            }
                            return dataInitialised;
                        }

                        data = (NativeMethods.WIN32_FILE_ATTRIBUTE_DATA)Marshal.PtrToStructure(
                                    pointer,
                                    typeof(NativeMethods.WIN32_FILE_ATTRIBUTE_DATA));
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(pointer);
                }
            }
            return dataInitialised;
        }
        public static string GetTypeName(WidePath name)
        {
            NativeMethods.SHFILEINFO shfi = new NativeMethods.SHFILEINFO();
            IntPtr hSuccess = NativeMethods.SHGetFileInfo(name.Display, 0, ref shfi,
                (uint)Marshal.SizeOf(shfi), NativeMethods.SHGFI_TYPENAME);

            if (hSuccess == IntPtr.Zero)
            {
                return string.Empty;
            }

            return shfi.szTypeName;
        }

        public static SafeFileHandle CreateFile(WidePath fileName, IOMode IOMode, bool OverWrite)
        {
            uint desiredAccess;
            uint creationDisposition;

            switch (IOMode)
            {
                case IOMode.ForReading:
                    desiredAccess = NativeMethods.GENERIC_READ | NativeMethods.GENERIC_WRITE;
                    creationDisposition = OverWrite ? NativeMethods.OPEN_ALWAYS
                                                    : NativeMethods.OPEN_EXISTING;
                    break;

                case IOMode.ForWriting:
                    desiredAccess = NativeMethods.GENERIC_WRITE;
                    creationDisposition = OverWrite ? NativeMethods.CREATE_ALWAYS
                                                    : NativeMethods.CREATE_NEW;
                    break;
                
                case IOMode.ForAppending:
                    desiredAccess = NativeMethods.GENERIC_WRITE;
                    creationDisposition = OverWrite ? NativeMethods.CREATE_ALWAYS
                                                    : NativeMethods.CREATE_NEW;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            using (ErrorMode mode = new ErrorMode())
            {
                IntPtr handle = NativeMethods.CreateFile(
                                                fileName.Win32,
                                                desiredAccess,
                                                NativeMethods.FILE_SHARE_READ | NativeMethods.FILE_SHARE_WRITE,
                                                IntPtr.Zero,
                                                creationDisposition,
                                                0,
                                                IntPtr.Zero
                                                );

                if (handle == (IntPtr)NativeMethods.INVALID_HANDLE_VALUE)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw CreateIOException(errorCode);
                }

                return new SafeFileHandle(handle, true);
            }
        }
    }
}
