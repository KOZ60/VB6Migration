using System;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using IO = System.IO;

namespace Scripting
{
    /// <summary>
    /// ディスク ドライブまたはネットワーク共有の各種プロパティへアクセスする手段を提供します。
    /// </summary>
    public class Drive : MarshalByRefObject
    {
        private FileSystemObject m_FileSystemObject;
        private WidePath m_RootPathName;

        internal class Information
        {
            public int SerialNumber = 0;
            public string VolumeName = string.Empty;
            public string FileSystem = string.Empty;
            public long AvailableSpace = 0;
            public long TotalSize = 0;
            public long FreeSpace = 0;
        }

        internal Drive(FileSystemObject fso, WidePath fileName)
        {
            m_FileSystemObject = fso;
            m_RootPathName = fileName.RootPathName;
            Information info = Info;
        }

        internal FileSystemObject fso
        {
            get { return m_FileSystemObject; }
        }

        internal Information Info
        {
            get
            {
                Information info = new Information();

                if (GetVolumeInformation(info))
                    GetDiskFreeSpaceEx(info);

                return info;
            }
        }

        /// <summary>
        /// 現在のオブジェクトを示す文字列を返します。
        /// </summary>
        /// <returns>ドライブのパス名が返ります。</returns>
        public override string ToString()
        {
            return this.Path;
        }

        /// <summary>
        /// 指定されたドライブまたはネットワーク共有でユーザーが使用できるディスク容量を返します。
        /// </summary>
        /// <value>ユーザーが使用できるディスク容量をバイト単位で返します。</value>
        public long AvailableSpace
        {
            get { return Info.AvailableSpace; }
        }

        /// <summary>
        /// 物理ローカル ドライブまたはネットワーク共有のドライブ名を返します。値の取得のみ可能です
        /// </summary>
        /// <value>ドライブ名を返します。</value>
        public string DriveLetter
        {
            get 
            {
                if (WidePath.IsUNC(m_RootPathName))
                    return string.Empty;
                else
                    return m_RootPathName.Display[0].ToString();
            }
        }

        /// <summary>
        /// 指定されたドライブの種類を示す値を返します。
        /// </summary>
        /// <value>DriveTypeConst</value>
        public DriveTypeConst DriveType
        {
            get 
            {
                switch (NativeWrapper.GetDriveType(m_RootPathName))
                {
                    case IO.DriveType.Unknown:
                        return DriveTypeConst.UnknownType;
                    
                    case IO.DriveType.NoRootDirectory:
                        return DriveTypeConst.Removable;

                    case IO.DriveType.Removable:
                        return DriveTypeConst.Removable;

                    case IO.DriveType.Fixed:
                        return DriveTypeConst.Fixed;

                    case IO.DriveType.Network:
                        return DriveTypeConst.Remote;

                    case IO.DriveType.CDRom:
                        return DriveTypeConst.CDRom;

                    case IO.DriveType.Ram:
                        return DriveTypeConst.RamDisk;

                    default:
                        return DriveTypeConst.UnknownType;
                }
            }
        }

        /// <summary>
        /// 指定されたドライブが使用しているファイル システムの種類を返します。
        /// </summary>
        /// <value>返される可能性のある種類には、FAT、NTFS、CDFS があります。</value>
        public string FileSystem
        {
            get { return Info.FileSystem; }
        }

        /// <summary>
        /// 指定されたドライブまたはネットワーク共有でユーザーが使用できるディスク空き容量を返します。値の取得のみ可能です。
        /// </summary>
        /// <value>空き容量をバイト単位で返します。</value>
        public long FreeSpace
        {
            get { return Info.FreeSpace; }
        }

        /// <summary>
        /// 指定されたドライブの準備ができているかどうかを取得します。
        /// </summary>
        /// <value>指定されたドライブの準備ができている場合は真 (True) を返します。準備ができれいなければ偽 (False) を返します。</value>
        public bool IsReady
        {
            get
            {
                Information info = new Information();
                return GetVolumeInformation(info); 
            }
        }

        internal string RootPathName
        {
            get { return m_RootPathName.RootPathName; }
        }

        /// <summary>
        /// ドライブのパスを返します。
        /// </summary>
        /// <value>ドライブ名の場合は、返される文字列にルート ディレクトリは含まれません。たとえば、C ドライブの場合なら、C:\ ではなく C: が返されます。</value>
        public string Path
        {
            // \ を外した状態
            get { return m_RootPathName.RootPathName.TrimEnd(WidePath.DirectorySeparatorChar); }
        }

        /// <summary>
        /// 指定されたドライブのルート フォルダを表す Folder オブジェクトを返します。取得のみ可能です。
        /// </summary>
        /// <value>Folder オブジェクト</value>
        public Folder RootFolder
        {
            get { return new Folder(fso, m_RootPathName.RootPathName); }
        }

        /// <summary>
        /// ディスク ボリュームを一意に識別するための十進数のシリアル番号を返します。
        /// </summary>
        /// <value>十進数のシリアル番号を返します。</value>
        /// <remarks>
        /// SerialNumber プロパティを使うと、リムーバブル メディアを使用している場合に、正しいディスクがドライブにセットされているかどうかを確認することができます。
        /// </remarks>
        public int SerialNumber
        {
            get { return Info.SerialNumber; }
        }

        /// <summary>
        /// 指定されたドライブのネットワーク共有名を返します。
        /// </summary>
        /// <value>
        /// ネットワーク ドライブでないドライブの場合は、長さ 0 の文字列 ("") が返されます。
        /// </value>
        public string ShareName
        {
            get
            {
                switch (this.DriveType)
                {
                    case DriveTypeConst.Remote:

                        // UNC の場合は、表示名を返す
                        if (this.RootPathName[0] == WidePath.DirectorySeparatorChar)
                            return m_RootPathName.Display;

                        return NativeWrapper.WNetGetConnection(this.Path).Display;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// ドライブまたはネットワーク共有の総容量をバイト単位で返します。
        /// </summary>
        /// <value>総容量をバイト単位で返します。</value>
        public long TotalSize
        {
            get { return Info.TotalSize; }
        }

        /// <summary>
        /// 指定されたドライブのボリューム名を設定します。値の取得も可能です。
        /// </summary>
        public string VolumeName
        {
            get { return Info.VolumeName; }
            set 
            { 
                bool r = NativeWrapper.SetVolumeLabel(this.RootPathName, value);
                if (!r)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw NativeWrapper.CreateIOException(errorCode);
                }
            }
        }

        internal bool GetVolumeInformation(Information info)
        {
            NativeWrapper.VolumeInformation volumeInfo;
            bool result = NativeWrapper.GetVolumeInformation(m_RootPathName, out volumeInfo);

            if (result)
            {
                info.SerialNumber = volumeInfo.SerialNumber;
                info.VolumeName = volumeInfo.VolumeName;
                info.FileSystem = volumeInfo.FileSystemName;
            }
            return result;
        }

        internal void GetDiskFreeSpaceEx(Information info)
        {
            long availableSpace;
            long totalSize;
            long freeSpace;

            bool result = NativeWrapper.GetDiskFreeSpaceEx(
                                            this.RootPathName,
                                            out availableSpace,
                                            out totalSize,
                                            out freeSpace);
            if (result)
            {
                info.AvailableSpace = availableSpace;
                info.TotalSize = totalSize;
                info.FreeSpace = freeSpace;
            }
        }

    }
}