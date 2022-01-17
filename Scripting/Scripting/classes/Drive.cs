using System;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using IO = System.IO;

namespace Scripting
{
    /// <summary>
    /// �f�B�X�N �h���C�u�܂��̓l�b�g���[�N���L�̊e��v���p�e�B�փA�N�Z�X�����i��񋟂��܂��B
    /// </summary>
    public class Drive : MarshalByRefObject
    {
        private FileSystemObject m_FileSystemObject;
        private FileNameClass m_RootPathName;

        internal class Information
        {
            public int SerialNumber = 0;
            public string VolumeName = string.Empty;
            public string FileSystem = string.Empty;
            public long AvailableSpace = 0;
            public long TotalSize = 0;
            public long FreeSpace = 0;
        }

        internal Drive(FileSystemObject fso, FileNameClass fileName)
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
        /// ���݂̃I�u�W�F�N�g�������������Ԃ��܂��B
        /// </summary>
        /// <returns>�h���C�u�̃p�X�����Ԃ�܂��B</returns>
        public override string ToString()
        {
            return this.Path;
        }

        /// <summary>
        /// �w�肳�ꂽ�h���C�u�܂��̓l�b�g���[�N���L�Ń��[�U�[���g�p�ł���f�B�X�N�e�ʂ�Ԃ��܂��B
        /// </summary>
        /// <value>���[�U�[���g�p�ł���f�B�X�N�e�ʂ��o�C�g�P�ʂŕԂ��܂��B</value>
        public long AvailableSpace
        {
            get { return Info.AvailableSpace; }
        }

        /// <summary>
        /// �������[�J�� �h���C�u�܂��̓l�b�g���[�N���L�̃h���C�u����Ԃ��܂��B�l�̎擾�̂݉\�ł�
        /// </summary>
        /// <value>�h���C�u����Ԃ��܂��B</value>
        public string DriveLetter
        {
            get 
            {
                if (FileNameClass.IsUNC(m_RootPathName))
                    return string.Empty;
                else
                    return m_RootPathName.DisplayFileName[0].ToString();
            }
        }

        /// <summary>
        /// �w�肳�ꂽ�h���C�u�̎�ނ������l��Ԃ��܂��B
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
        /// �w�肳�ꂽ�h���C�u���g�p���Ă���t�@�C�� �V�X�e���̎�ނ�Ԃ��܂��B
        /// </summary>
        /// <value>�Ԃ����\���̂����ނɂ́AFAT�ANTFS�ACDFS ������܂��B</value>
        public string FileSystem
        {
            get { return Info.FileSystem; }
        }

        /// <summary>
        /// �w�肳�ꂽ�h���C�u�܂��̓l�b�g���[�N���L�Ń��[�U�[���g�p�ł���f�B�X�N�󂫗e�ʂ�Ԃ��܂��B�l�̎擾�̂݉\�ł��B
        /// </summary>
        /// <value>�󂫗e�ʂ��o�C�g�P�ʂŕԂ��܂��B</value>
        public long FreeSpace
        {
            get { return Info.FreeSpace; }
        }

        /// <summary>
        /// �w�肳�ꂽ�h���C�u�̏������ł��Ă��邩�ǂ������擾���܂��B
        /// </summary>
        /// <value>�w�肳�ꂽ�h���C�u�̏������ł��Ă���ꍇ�͐^ (True) ��Ԃ��܂��B�������ł��ꂢ�Ȃ���΋U (False) ��Ԃ��܂��B</value>
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
        /// �h���C�u�̃p�X��Ԃ��܂��B
        /// </summary>
        /// <value>�h���C�u���̏ꍇ�́A�Ԃ���镶����Ƀ��[�g �f�B���N�g���͊܂܂�܂���B���Ƃ��΁AC �h���C�u�̏ꍇ�Ȃ�AC:\ �ł͂Ȃ� C: ���Ԃ���܂��B</value>
        public string Path
        {
            // \ ���O�������
            get { return m_RootPathName.RootPathName.TrimEnd(FileNameClass.DirectorySeparatorChar); }
        }

        /// <summary>
        /// �w�肳�ꂽ�h���C�u�̃��[�g �t�H���_��\�� Folder �I�u�W�F�N�g��Ԃ��܂��B�擾�̂݉\�ł��B
        /// </summary>
        /// <value>Folder �I�u�W�F�N�g</value>
        public Folder RootFolder
        {
            get { return new Folder(fso, m_RootPathName.RootPathName); }
        }

        /// <summary>
        /// �f�B�X�N �{�����[������ӂɎ��ʂ��邽�߂̏\�i���̃V���A���ԍ���Ԃ��܂��B
        /// </summary>
        /// <value>�\�i���̃V���A���ԍ���Ԃ��܂��B</value>
        /// <remarks>
        /// SerialNumber �v���p�e�B���g���ƁA�����[�o�u�� ���f�B�A���g�p���Ă���ꍇ�ɁA�������f�B�X�N���h���C�u�ɃZ�b�g����Ă��邩�ǂ������m�F���邱�Ƃ��ł��܂��B
        /// </remarks>
        public int SerialNumber
        {
            get { return Info.SerialNumber; }
        }

        /// <summary>
        /// �w�肳�ꂽ�h���C�u�̃l�b�g���[�N���L����Ԃ��܂��B
        /// </summary>
        /// <value>
        /// �l�b�g���[�N �h���C�u�łȂ��h���C�u�̏ꍇ�́A���� 0 �̕����� ("") ���Ԃ���܂��B
        /// </value>
        public string ShareName
        {
            get
            {
                switch (this.DriveType)
                {
                    case DriveTypeConst.Remote:

                        // UNC �̏ꍇ�́A�\������Ԃ�
                        if (this.RootPathName[0] == FileNameClass.DirectorySeparatorChar)
                            return m_RootPathName.DisplayFileName;

                        return NativeWrapper.WNetGetConnection(this.Path).DisplayFileName;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// �h���C�u�܂��̓l�b�g���[�N���L�̑��e�ʂ��o�C�g�P�ʂŕԂ��܂��B
        /// </summary>
        /// <value>���e�ʂ��o�C�g�P�ʂŕԂ��܂��B</value>
        public long TotalSize
        {
            get { return Info.TotalSize; }
        }

        /// <summary>
        /// �w�肳�ꂽ�h���C�u�̃{�����[������ݒ肵�܂��B�l�̎擾���\�ł��B
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