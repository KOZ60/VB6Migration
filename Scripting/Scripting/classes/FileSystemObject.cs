using System;
using IO = System.IO;
using Env = System.Environment;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Scripting
{
    /// <summary>
    /// �R���s���[�^�̃t�@�C�� �V�X�e���ւ̃A�N�Z�X��񋟂��܂��B
    /// </summary>
    public class FileSystemObject : MarshalByRefObject
	{
        /// <summary>
        /// ���[�J�� �}�V���ŗ��p�ł��邷�ׂĂ� Drive �I�u�W�F�N�g�������� Drives �R���N�V������Ԃ��܂��B
        /// </summary>
        /// <value>Dirave �I�u�W�F�N�g�̃R���N�V����</value>
        public Drives Drives
        {
            get { return new Drives(this); }
        }

        /// <summary>
        /// �����p�X�̖����ɖ��O��ǉ����܂��B
        /// </summary>
        /// <param name="basePath">���� relativePath �Ŏw�肵�����O�𖖔��ɒǉ���������p�X���w�肵�܂��B��΃p�X�܂��͑��΃p�X�̂ǂ��炩���w�肵�܂��B�܂��A���݂��Ȃ��t�H���_���w�肷�邱�Ƃ��ł��܂��B</param>
        /// <param name="relativePath">���� basePath �Ŏw�肵�������p�X�̖����ɒǉ����閼�O���w�肵�܂��B</param>
        /// <returns>�����p�X�ɒǉ�������̖��O�B</returns>
        /// <remarks>BuildPath ���\�b�h�́A�K�v�ȏꍇ�����A�����p�X�Ǝw�肵�����O�̊ԂɃp�X�̋�؂蕶����}�����܂��B</remarks>
        public string BuildPath(string basePath, string relativePath)
        {
            return FileNameClass.BuildPath(basePath, relativePath);
        }

        internal void RemoveReadOnlyAttribute(ABSClass target, bool Force)
        {
            if (Force && (NativeWrapper.IsMask((int)target.Attributes, (int)FileAttribute.ReadOnly)))
            {
                target.Attributes = target.Attributes & ~FileAttribute.ReadOnly;
            }
        }

        /// <summary>
        /// �t�@�C����ʂ̏ꏊ�փR�s�[���܂��B
        /// </summary>
        /// <param name="Source">�R�s�[����t�@�C����\����������w�肵�܂��B1 �܂��͕����̃t�@�C�����w�肷�邽�߂Ƀ��C���h�J�[�h�������g�p�ł��܂��B</param>
        /// <param name="Destination">���� source �Ŏw�肵���t�@�C���̃R�s�[���\����������w�肵�܂��B���C���h�J�[�h�����͎g�p�ł��܂���B</param>
        /// <param name="OverWriteFiles">
        /// �����t�@�C�����㏑�����邩�ǂ����������u�[���l���w�肵�܂��B
        /// �^ (True) ���w�肷��Ɗ����t�@�C���͏㏑������A�U (False) ���w�肷��Ə㏑������܂���B
        /// ����l�́A�^ (false) �ł��B
        /// ���� destination �Ɏw�肵���R�s�[�悪�ǂݎ���p�̑����������Ă����ꍇ�́A���� overwrite �Ɏw�肵���l�Ƃ͊֌W�Ȃ� CopyFile ���\�b�h�̏����͎��s����̂ŁA���ӂ��Ă��������B
        /// </param>
        public void CopyFile(string Source, string Destination, bool OverWriteFiles = false)
        {
            // Source �̍Ō�̕����� \ �Ȃ� CopyFolder �����s����
            if (FileNameClass.LastCharIsDirectorySeparatorChar(Source)) {

                CopyFolder(Source, Destination, OverWriteFiles);

            } else {
                
                // Source �Ƀ��C���h�J�[�h���܂܂��Ȃ�A�t�@�C����񋓂��ăR�s�[����
                if (FileNameClass.ContainsWildChars(Source)) {
                    Folder folder = this.GetFolder(GetParentFolderName(Source));
                    foreach (File f in folder.Files) {
                        if (LikeOperator.LikeString(f.Path, Source, Microsoft.VisualBasic.CompareMethod.Text)) {
                            CopyFileInternal(f.Path, Destination, OverWriteFiles);
                        }
                    }
                } else {
                    // 1��1 �̃R�s�[
                    CopyFileInternal(Source, Destination, OverWriteFiles);
                }
            }
        }

        internal void CopyFileInternal(string Source, string Destination, bool OverWriteFiles = false)
        {
            // Destination �̍Ō�̕����� \ �Ȃ�f�B���N�g���Ƃ��� Source ����t�@�C���������o���A�R�s�[��Ƃ���
            if (FileNameClass.LastCharIsDirectorySeparatorChar(Destination)) {
                Destination = FileNameClass.BuildPath(Destination, FileNameClass.GetFileName(Source));
            }
            if (FileExists(Destination))
            {
                if (OverWriteFiles)
                {
                    RemoveReadOnlyAttribute(new File(this, Destination), OverWriteFiles);
                }
                else
                    throw NativeWrapper.CreateIOException(NativeMethods.ERROR_ALREADY_EXISTS);
            }

            NativeWrapper.CopyFile(Source, Destination, OverWriteFiles);
        }

        /// <summary>
        /// �t�H���_���ċA�I�ɕʂ̏ꏊ�փR�s�[���܂��B
        /// </summary>
        /// <param name="Source">�R�s�[����t�H���_��\����������w�肵�܂��B1 �܂��͕����̃t�H���_���w�肷�邽�߂Ƀ��C���h�J�[�h�������g�p�ł��܂��B</param>
        /// <param name="Destination">���� source �Ŏw�肵���t�H���_����уT�u�t�H���_�̃R�s�[���\����������w�肵�܂��B���C���h�J�[�h�����͎g�p�ł��܂���B</param>
        /// <param name="OverWriteFiles">�����t�H���_���㏑�����邩�ǂ����������u�[���l���w�肵�܂��B�^ (True) ���w�肷��Ɗ����t�H���_���̃t�@�C�����㏑�����A�U (False) ���w�肷��Ə㏑�����܂���B����l�́A�U (False)�ł��B</param>
        public void CopyFolder(string Source, string Destination, bool OverWriteFiles = false)
        {
            Source = FileNameClass.TrimDirectorySeparator(Source);
            Destination = FileNameClass.TrimDirectorySeparator(Destination);
            CopyFolderInternal(new Folder(this, Source), Destination, OverWriteFiles);
        }

        internal void CopyFolderInternal(Folder folder, string Destination, bool OverWriteFiles)
        {
            if (!FolderExists(Destination))
            {
                CreateFolder(Destination);
            }
            else if (!OverWriteFiles)
            {
                throw NativeWrapper.CreateIOException(NativeMethods.ERROR_ALREADY_EXISTS);
            }

            foreach (File fileInfo in folder.Files)
            {
                string tempPath = BuildPath(Destination, fileInfo.Name);
                CopyFileInternal(fileInfo.Path, tempPath, OverWriteFiles);
            }

            foreach (Folder dir in folder.SubFolders)
            {
                string tempPath = BuildPath(Destination, dir.Name);
                CopyFolderInternal(dir, tempPath, OverWriteFiles);
            }
        }

        /// <summary>
        /// �t�H���_���쐬���܂��B
        /// </summary>
        /// <param name="Path">�쐬����t�H���_��\�������񎮂��w�肵�܂��B</param>
        /// <returns>�쐬�����t�H���_�ɑ΂��� Folder �I�u�W�F�N�g�B</returns>
        public Folder CreateFolder(string Path)
        {
            if (Path.Length > 3) {
                if (Path[Path.Length - 1] == '\\') {
                    Path = Path.Substring(0, Path.Length - 1);
                }
            }
            CreateFolderInternal(Path);
            return new Folder(this, Path);
        }

        internal void CreateFolderInternal(string Path)
        {
            string parentPath = GetParentFolderName(Path);
            
            if (string.IsNullOrEmpty(parentPath))
                throw NativeWrapper.CreateIOException(NativeMethods.ERROR_BAD_PATHNAME);

            if (!FolderExists(parentPath))
                CreateFolderInternal(parentPath);

            NativeWrapper.CreateDirectory(Path);
        }

        /// <summary>
        /// �w�肳�ꂽ�t�@�C�����폜���܂��B
        /// </summary>
        /// <param name="FileSpec">�폜����t�@�C���̖��O���w�肵�܂��B</param>
        /// <param name="Force">�^ (True) ���w�肷��ƁA�ǂݎ���p�̃t�@�C�����폜����܂��B</param>
        public void DeleteFile(string FileSpec, bool Force = false)
        {
            DeleteFileInternal(new File(this, FileSpec), Force);
        }

        internal void DeleteFileInternal(File file, bool Force)
        {
            RemoveReadOnlyAttribute(file, Force);
            NativeWrapper.DeleteFile(file.Path);
        }

        /// <summary>
        /// �w�肵���t�H���_����т��̃t�H���_���̃t�H���_�ƃt�@�C�����폜���܂��B
        /// </summary>
        /// <param name="FolderSpec">�폜����t�H���_�̖��O���w�肵�܂��B</param>
        /// <param name="Force">�^ (True) ���w�肷��ƁA�ǂݎ���p�̃t�H���_���폜����܂��B</param>
        public void DeleteFolder(string FolderSpec, bool Force = false)
        {
            DeleteFolderInternal(new Folder(this, FolderSpec), Force);
        }

        internal void DeleteFolderInternal(Folder folder, bool Force)
        {
            RemoveReadOnlyAttribute(folder, Force);
            foreach (Folder subDir in folder.SubFolders)
            {
                DeleteFolderInternal(subDir, Force);
            }
            foreach (File file in folder.Files)
            {
                DeleteFileInternal(file, Force);
            }
            NativeWrapper.RemoveDirectory(folder.Path);
        }

        /// <summary>
        /// �w�肵���h���C�u�����݂��邩�ǂ������擾���܂��B
        /// </summary>
        /// <param name="DriveSpec">���݂��邩�ǂ����𒲂ׂ�h���C�u�̖��O���w�肵�܂��B</param>
        /// <returns>�w�肵���h���C�u�����݂���ꍇ�́A�^ (True) ��Ԃ��܂��B���݂��Ȃ��ꍇ�́A�U (False) ��Ԃ��܂��B</returns>
        public bool DriveExists(string DriveSpec)
        {
            if (DriveSpec.Length > 1)
                DriveSpec = DriveSpec[0].ToString();
            return this.Drives.Exists(DriveSpec);
        }

        /// <summary>
        /// �w�肵���t�@�C�������݂��邩�ǂ������擾���܂��B
        /// </summary>
        /// <param name="FileSpec">���݂��邩�ǂ����𒲂ׂ�t�@�C���̖��O���w�肵�܂��B</param>
        /// <returns>�w�肵���t�@�C�������݂���ꍇ�́A�^ (True) ��Ԃ��܂��B���݂��Ȃ��ꍇ�́A�U (False) ��Ԃ��܂��B</returns>
        public bool FileExists(string FileSpec)
        {
            return ExistsInternal(FileSpec, FileAttribute.Normal);
        }

        /// <summary>
        /// �w�肵���t�H���_�����݂��邩�ǂ������擾���܂��B
        /// </summary>
        /// <param name="FolderSpec">���݂��邩�ǂ����𒲂ׂ�t�H���_�̖��O���w�肵�܂��B</param>
        /// <returns>�w�肵���t�H���_�����݂���ꍇ�́A�^ (True) ��Ԃ��܂��B���݂��Ȃ��ꍇ�́A�U (False) ��Ԃ��܂��B</returns>
        public bool FolderExists(string FolderSpec)
        {
            return ExistsInternal(FolderSpec, FileAttribute.Directory);
        }

        internal static bool ExistsInternal(FileNameClass path, FileAttribute attr)
        {
            NativeMethods.WIN32_FILE_ATTRIBUTE_DATA data = new NativeMethods.WIN32_FILE_ATTRIBUTE_DATA();
            int dataInitialised = NativeWrapper.FillAttributeInfo(path, ref data, false, true);
            return (dataInitialised == 0)
                && (data.dwFileAttributes != (FileAttribute)(-1))
                && ((data.dwFileAttributes & FileAttribute.Directory) == attr);
        }

        /// <summary>
        /// �w�肵���p�X�ɑ΂��āA���S�Ȉ�ӂ̃p�X����Ԃ��܂��B
        /// </summary>
        /// <param name="Path">���S�ň�ӓI�ȃp�X�����擾����p�X���w�肵�܂��B</param>
        /// <returns>
        /// ���S�Ȉ�ӂ̃p�X���B
        /// </returns>
        public string GetAbsolutePathName(string Path)
        {
            FileNameClass f = Path;
            return f.DisplayFileName;
        }

        /// <summary>
        /// �w�肳�ꂽ�p�X�̃x�[�X����\���������Ԃ��܂��B�t�@�C���g���q�͊܂܂�܂���B
        /// </summary>
        /// <param name="Path">�x�[�X�����擾����\���v�f�̃p�X���w�肵�܂��B</param>
        /// <returns>�肳�ꂽ�p�X�̃x�[�X����\��������B</returns>
        public string GetBaseName(string Path)
        {
            return FileNameClass.GetFileNameWithoutExtension(Path);
        }

        /// <summary>
        /// �w�肳�ꂽ�p�X�Ɋ܂܂��h���C�u�ɑΉ����� Drive �I�u�W�F�N�g��Ԃ��܂��B
        /// </summary>
        /// <param name="DriveSpec">
        /// �h���C�u�� (c)�A�R�����t���̃h���C�u�� (c:)�A
        /// �R�����ƃp�X�̋�؂蕶���̕t�����h���C�u�� (c:\)�A
        /// �C�ӂ̃l�b�g���[�N���L�� (\\computer2\share1) �̂����ꂩ���w�肵�܂��B
        /// </param>
        /// <returns>�w�肳�ꂽ�p�X�Ɋ܂܂��h���C�u�ɑΉ����� Drive �I�u�W�F�N�g�B</returns>
        public Drive GetDrive(string DriveSpec)
        {
            if (!FileNameClass.IsUNC(DriveSpec))
                DriveSpec = new string(
                                new char[] { 
                                    DriveSpec[0],
                                    FileNameClass.VolumeSeparatorChar,
                                    FileNameClass.DirectorySeparatorChar
                                }
                            );

            return new Drive(this, DriveSpec);
        }

        /// <summary>
        /// �w�肳�ꂽ�p�X���u����Ă���h���C�u�̖��O�̓������������Ԃ��܂��B
        /// </summary>
        /// <param name="Path">�h���C�u�������o���p�X���w�肵�܂��B</param>
        /// <returns>�h���C�u���B</returns>
        public string GetDriveName(string Path)
        {
            FileNameClass f = Path;
            return f.DriveName;
        }

        /// <summary>
        /// �w�肳�ꂽ�p�X�̊g���q��\���������Ԃ��܂��B
        /// </summary>
        /// <param name="Path">�g���q�����o���\���v�f�̃p�X���w�肵�܂��B</param>
        /// <returns>�w�肳�ꂽ�p�X�̊g���q��\��������B</returns>
        public string GetExtensionName(string Path)
        {
            return FileNameClass.GetExtensionName(Path);
        }

        /// <summary>
        /// �w�肳�ꂽ�p�X�ɒu����Ă���t�@�C���ɑΉ����� File �I�u�W�F�N�g��Ԃ��܂��B
        /// </summary>
        /// <param name="FilePath">�ړI�̃t�@�C���̃p�X���w�肵�܂��B��΃p�X�A�܂��͑��΃p�X�̂ǂ��炩���w��ł��܂��B</param>
        /// <returns>�w�肳�ꂽ�p�X�ɒu����Ă���t�@�C���ɑΉ����� File �I�u�W�F�N�g�B</returns>
        public File GetFile(string FilePath)
        {
            return new File(this, FilePath);
        }

        /// <summary>
        /// �w�肳�ꂽ�p�X�̍Ō�̍\���v�f��Ԃ��܂��B
        /// </summary>
        /// <param name="Path">�ړI�̃t�@�C���̃p�X���w�肵�܂��B��΃p�X�A�܂��͑��΃p�X�̂ǂ��炩���w��ł��܂��B</param>
        /// <returns>�w�肳�ꂽ�p�X�̍Ō�̍\���v�f�B</returns>
        public string GetFileName(string Path)
        {
            return FileNameClass.GetFileName(Path);
        }

        /// <summary>
        /// �w�肳�ꂽ�p�X�ɒu����Ă���t�H���_�ɑΉ����� Folder �I�u�W�F�N�g��Ԃ��܂��B
        /// </summary>
        /// <param name="FolderPath">�ړI�̃t�H���_�̃p�X���w�肵�܂��B</param>
        /// <returns>�w�肳�ꂽ�p�X�ɒu����Ă���t�H���_�ɑΉ����� Folder �I�u�W�F�N�g</returns>
        public Folder GetFolder(string FolderPath)
        {
            return new Folder(this, FolderPath);
        }

        /// <summary>
        /// �w�肳�ꂽ�p�X�̍Ō�̍\���v�f�̐e�t�H���_�̖��O���������������Ԃ��܂��B
        /// </summary>
        /// <param name="Path">�e�t�H���_�̖��O���擾����\���v�f�̃p�X���w�肵�܂��B</param>
        /// <returns>�e�t�H���_�̖��O�B</returns>
        public string GetParentFolderName(string Path)
        {
            return FileNameClass.GetParentFolderName(Path);
        }

        /// <summary>
        /// �w�肳�ꂽ���ʂȃt�H���_��Ԃ��܂��B
        /// </summary>
        /// <param name="SpecialFolder">�擾������ʂȃt�H���_�̎�ނ��w�肵�܂��B</param>
        /// <returns>���ʂȃt�H���_������ Folder �I�u�W�F�N�g</returns>
        public Folder GetSpecialFolder(SpecialFolderConst SpecialFolder)
        {
            string folderName = null;

            switch (SpecialFolder)
            {
                case SpecialFolderConst.WindowsFolder:
                    folderName = NativeWrapper.GetWindowsDirectory();
                    break;

                case SpecialFolderConst.SystemFolder:
                    folderName = Env.SystemDirectory;
                    break;

                case SpecialFolderConst.TemporaryFolder:
                    folderName = IO.Path.GetTempPath();
                    break;
            }

            return new Folder(this, folderName);
        }

        /// <summary>
        /// �ꎞ�t�@�C���܂��͈ꎞ�t�H���_�̖��O�������_���ɐ������A�����Ԃ��܂��B
        /// </summary>
        /// <returns>�ꎞ�t�@�C���܂��͈ꎞ�t�H���_�̖��O�B</returns>
        public string GetTempName()
        {
            return IO.Path.GetTempFileName();
        }

        /// <summary>
        /// �t�@�C����ʂ̏ꏊ�ֈړ����܂��B
        /// </summary>
        /// <param name="Source">�ړ�����t�@�C����\����������w�肵�܂��B�p�X�̍Ō�̍\���v�f���ł̓��C���h�J�[�h�������g�p�ł��܂��B</param>
        /// <param name="Destination">���� source �Ŏw�肵���t�@�C���̈ړ����\����������w�肵�܂��B���C���h�J�[�h�����͎g�p�ł��܂���B</param>
        public void MoveFile(string Source, string Destination)
        {
            MoveFileInternal(new File(this, Source), Destination);
        }

        internal void MoveFileInternal(File file, string Destination)
        {
            NativeWrapper.MoveFileEx(file.Path, Destination);
        }

        /// <summary>
        /// �t�H���_��ʂ̏ꏊ�ֈړ����܂��B
        /// </summary>
        /// <param name="Source">�ړ�����t�H���_��\����������w�肵�܂��B�p�X�̍Ō�̍\���v�f���ł̓��C���h�J�[�h�������g�p�ł��܂��B</param>
        /// <param name="Destination">���� source �Ŏw�肵���t�H���_�̈ړ����\����������w�肵�܂��B���C���h�J�[�h�����͎g�p�ł��܂���B</param>
        public void MoveFolder(string Source, string Destination)
        {
            MoveFolderInternal(new Folder(this, Source), Destination);
        }

        internal void MoveFolderInternal(Folder folder, string Destination)
        {
            NativeWrapper.MoveFileEx(folder.Path, Destination);
        }

        /// <summary>
        /// �t�@�C���̃o�[�W���������擾���܂��B
        /// </summary>
        /// <param name="FileName">�o�[�W���������擾����t�@�C���������������񎮂��w�肵�܂��B</param>
        /// <returns>�t�@�C���̃o�[�W�������B</returns>
        public string GetFileVersion(string FileName)
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(FileName);
            return fvi.ProductVersion;
        }

        /// <summary>
        /// �w�肳�ꂽ�t�@�C�����̃t�@�C�����쐬���A�t�@�C����ǂݍ��񂾂�A�������ނƂ��Ɏg�p���� TextStream �I�u�W�F�N�g��Ԃ��܂��B
        /// </summary>
        /// <param name="FileName">�쐬����t�@�C�������������񎮂��w�肵�܂��B</param>
        /// <param name="Overwrite">
        /// ���ɂ���t�@�C�����㏑�����邩�ǂ����������u�[���l���w�肵�܂��B
        /// �t�@�C�����㏑������ꍇ�́A�^ (True) ���w�肵�A�㏑�����Ȃ��ꍇ�́A�U (False) ���w�肵�܂��B
        /// �ȗ�����ƁA�t�@�C���͏㏑������܂���B</param>
        /// <param name="Unicode">
        /// Unicode �܂��� ASCII �t�@�C���̂����ꂩ�ō쐬���ꂽ�t�@�C�����������u�[���l���w�肵�܂��B
        /// Unicode �ō쐬���ꂽ�t�@�C���ł́A�^ (True) ���w�肵�܂��B
        /// ASCII �t�@�C���ō쐬���ꂽ�t�@�C���ł́A�U (False) ���w�肵�܂��B
        /// �ȗ������ꍇ�AASCII �t�@�C���ō쐬���ꂽ�t�@�C���Ƃ��܂��B
        /// </param>
        /// <returns>TextStream �I�u�W�F�N�g��Ԃ��܂��B</returns>
        public TextStream CreateTextFile(string FileName, bool Overwrite = false, bool Unicode = false)
        {
            Tristate Format = Unicode ? Tristate.TristateTrue : Tristate.TristateFalse;
            return new TextStreamClass(FileName, IOMode.ForWriting, Overwrite, Format);
        }

        /// <summary>
        /// �w�肳�ꂽ�t�@�C�����J���A�t�@�C����ǂݍ��񂾂�A�������ǉ�����Ƃ��Ɏg�p���� TextStream �I�u�W�F�N�g��Ԃ��܂��B
        /// </summary>
        /// <param name="FileName">�J���t�@�C�������������񎮂��w�肵�܂��B</param>
        /// <param name="IOMode">�ǂݍ��݁A�܂��͏������݂̂�����̂��߂Ƀt�@�C�����J���̂��������AIOMode �萔���w�肵�܂��B</param>
        /// <param name="Create">
        /// ���� filename �Ŏw�肳�ꂽ�t�@�C�������݂��Ȃ��ꍇ�ɐV�����t�@�C�����쐬���邩�ǂ����������u�[���l���w�肵�܂��B
        /// �V�����t�@�C�����쐬����ꍇ�́A�^ (True) ���w�肵�A�쐬���Ȃ��ꍇ�́A�U (False) ���w�肵�܂��B
        /// �ȗ�����ƁA�V�����t�@�C���͍쐬����܂���B
        /// </param>
        /// <param name="Format">�J���t�@�C���̌`���������l���w�肵�܂��B�ȗ�����ƁAASCII �t�@�C���Ƃ��ĊJ����܂��B</param>
        /// <returns>TextStream �I�u�W�F�N�g��Ԃ��܂��B</returns>
        public TextStream OpenTextFile(string FileName, IOMode IOMode = IOMode.ForReading, bool Create = false, Tristate Format = Tristate.TristateFalse)
        {
            return new TextStreamClass(FileName, IOMode, Create, Format);
        }

        /// <summary>
        /// �W�����o�͂� TextStream ��Ԃ��܂��B
        /// </summary>
        /// <param name="StandardStreamType">�쐬����W�����o�͂̎�ނ��w�肵�܂��B</param>
        /// <param name="Unicode">unicode ���܂ނ��ǂ������w�肵�܂��B</param>
        /// <returns>TextStream �I�u�W�F�N�g��Ԃ��܂��B</returns>
        public TextStream GetStandardStream(StandardStreamTypes StandardStreamType, bool Unicode = false)
        {
            return new TextStreamClass(StandardStreamType, Unicode);
        }
    }
}