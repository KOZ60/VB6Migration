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
    /// コンピュータのファイル システムへのアクセスを提供します。
    /// </summary>
    public class FileSystemObject : MarshalByRefObject
	{
        /// <summary>
        /// ローカル マシンで利用できるすべての Drive オブジェクトが入った Drives コレクションを返します。
        /// </summary>
        /// <value>Dirave オブジェクトのコレクション</value>
        public Drives Drives
        {
            get { return new Drives(this); }
        }

        /// <summary>
        /// 既存パスの末尾に名前を追加します。
        /// </summary>
        /// <param name="basePath">引数 relativePath で指定した名前を末尾に追加する既存パスを指定します。絶対パスまたは相対パスのどちらかを指定します。また、存在しないフォルダを指定することができます。</param>
        /// <param name="relativePath">引数 basePath で指定した既存パスの末尾に追加する名前を指定します。</param>
        /// <returns>既存パスに追加した後の名前。</returns>
        /// <remarks>BuildPath メソッドは、必要な場合だけ、既存パスと指定した名前の間にパスの区切り文字を挿入します。</remarks>
        public string BuildPath(string basePath, string relativePath)
        {
            return WidePath.BuildPath(basePath, relativePath);
        }

        internal void RemoveReadOnlyAttribute(ABSClass target, bool Force)
        {
            if (Force && (NativeWrapper.IsMask((int)target.Attributes, (int)FileAttribute.ReadOnly)))
            {
                target.Attributes = target.Attributes & ~FileAttribute.ReadOnly;
            }
        }

        /// <summary>
        /// ファイルを別の場所へコピーします。
        /// </summary>
        /// <param name="Source">コピーするファイルを表す文字列を指定します。1 つまたは複数のファイルを指定するためにワイルドカード文字を使用できます。</param>
        /// <param name="Destination">引数 source で指定したファイルのコピー先を表す文字列を指定します。ワイルドカード文字は使用できません。</param>
        /// <param name="OverWriteFiles">
        /// 既存ファイルを上書きするかどうかを示すブール値を指定します。
        /// 真 (True) を指定すると既存ファイルは上書きされ、偽 (False) を指定すると上書きされません。
        /// 既定値は、真 (false) です。
        /// 引数 destination に指定したコピー先が読み取り専用の属性を持っていた場合は、引数 overwrite に指定した値とは関係なく CopyFile メソッドの処理は失敗するので、注意してください。
        /// </param>
        public void CopyFile(string Source, string Destination, bool OverWriteFiles = false)
        {
            // Source の最後の文字が \ なら CopyFolder を実行する
            if (WidePath.LastCharIsDirectorySeparatorChar(Source)) {

                CopyFolder(Source, Destination, OverWriteFiles);

            } else {
                
                // Source にワイルドカードが含まれるなら、ファイルを列挙してコピーする
                if (WidePath.ContainsWildChars(Source)) {
                    Folder folder = this.GetFolder(GetParentFolderName(Source));
                    foreach (File f in folder.Files) {
                        if (LikeOperator.LikeString(f.Path, Source, Microsoft.VisualBasic.CompareMethod.Text)) {
                            CopyFileInternal(f.Path, Destination, OverWriteFiles);
                        }
                    }
                } else {
                    // 1対1 のコピー
                    CopyFileInternal(Source, Destination, OverWriteFiles);
                }
            }
        }

        internal void CopyFileInternal(string Source, string Destination, bool OverWriteFiles = false)
        {
            // Destination の最後の文字が \ ならディレクトリとして Source からファイル名を取り出し、コピー先とする
            if (WidePath.LastCharIsDirectorySeparatorChar(Destination)) {
                Destination = WidePath.BuildPath(Destination, WidePath.GetFileName(Source));
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
        /// フォルダを再帰的に別の場所へコピーします。
        /// </summary>
        /// <param name="Source">コピーするフォルダを表す文字列を指定します。1 つまたは複数のフォルダを指定するためにワイルドカード文字を使用できます。</param>
        /// <param name="Destination">引数 source で指定したフォルダおよびサブフォルダのコピー先を表す文字列を指定します。ワイルドカード文字は使用できません。</param>
        /// <param name="OverWriteFiles">既存フォルダを上書きするかどうかを示すブール値を指定します。真 (True) を指定すると既存フォルダ内のファイルを上書きし、偽 (False) を指定すると上書きしません。既定値は、偽 (False)です。</param>
        public void CopyFolder(string Source, string Destination, bool OverWriteFiles = false)
        {
            Source = WidePath.TrimDirectorySeparator(Source);
            Destination = WidePath.TrimDirectorySeparator(Destination);
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
        /// フォルダを作成します。
        /// </summary>
        /// <param name="Path">作成するフォルダを表す文字列式を指定します。</param>
        /// <returns>作成したフォルダに対する Folder オブジェクト。</returns>
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
        /// 指定されたファイルを削除します。
        /// </summary>
        /// <param name="FileSpec">削除するファイルの名前を指定します。</param>
        /// <param name="Force">真 (True) を指定すると、読み取り専用のファイルも削除されます。</param>
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
        /// 指定したフォルダおよびそのフォルダ内のフォルダとファイルを削除します。
        /// </summary>
        /// <param name="FolderSpec">削除するフォルダの名前を指定します。</param>
        /// <param name="Force">真 (True) を指定すると、読み取り専用のフォルダも削除されます。</param>
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
        /// 指定したドライブが存在するかどうかを取得します。
        /// </summary>
        /// <param name="DriveSpec">存在するかどうかを調べるドライブの名前を指定します。</param>
        /// <returns>指定したドライブが存在する場合は、真 (True) を返します。存在しない場合は、偽 (False) を返します。</returns>
        public bool DriveExists(string DriveSpec)
        {
            if (DriveSpec.Length > 1)
                DriveSpec = DriveSpec[0].ToString();
            return this.Drives.Exists(DriveSpec);
        }

        /// <summary>
        /// 指定したファイルが存在するかどうかを取得します。
        /// </summary>
        /// <param name="FileSpec">存在するかどうかを調べるファイルの名前を指定します。</param>
        /// <returns>指定したファイルが存在する場合は、真 (True) を返します。存在しない場合は、偽 (False) を返します。</returns>
        public bool FileExists(string FileSpec)
        {
            return ExistsInternal(FileSpec, FileAttribute.Normal);
        }

        /// <summary>
        /// 指定したフォルダが存在するかどうかを取得します。
        /// </summary>
        /// <param name="FolderSpec">存在するかどうかを調べるフォルダの名前を指定します。</param>
        /// <returns>指定したフォルダが存在する場合は、真 (True) を返します。存在しない場合は、偽 (False) を返します。</returns>
        public bool FolderExists(string FolderSpec)
        {
            return ExistsInternal(FolderSpec, FileAttribute.Directory);
        }

        internal static bool ExistsInternal(WidePath path, FileAttribute attr)
        {
            NativeMethods.WIN32_FILE_ATTRIBUTE_DATA data = new NativeMethods.WIN32_FILE_ATTRIBUTE_DATA();
            int dataInitialised = NativeWrapper.FillAttributeInfo(path, ref data, false, true);
            return (dataInitialised == 0)
                && (data.dwFileAttributes != (FileAttribute)(-1))
                && ((data.dwFileAttributes & FileAttribute.Directory) == attr);
        }

        /// <summary>
        /// 指定したパスに対して、完全な一意のパス名を返します。
        /// </summary>
        /// <param name="Path">完全で一意的なパス名を取得するパスを指定します。</param>
        /// <returns>
        /// 完全な一意のパス名。
        /// </returns>
        public string GetAbsolutePathName(string Path)
        {
            WidePath f = Path;
            return f.Display;
        }

        /// <summary>
        /// 指定されたパスのベース名を表す文字列を返します。ファイル拡張子は含まれません。
        /// </summary>
        /// <param name="Path">ベース名を取得する構成要素のパスを指定します。</param>
        /// <returns>定されたパスのベース名を表す文字列。</returns>
        public string GetBaseName(string Path)
        {
            return WidePath.GetFileNameWithoutExtension(Path);
        }

        /// <summary>
        /// 指定されたパスに含まれるドライブに対応する Drive オブジェクトを返します。
        /// </summary>
        /// <param name="DriveSpec">
        /// ドライブ名 (c)、コロン付きのドライブ名 (c:)、
        /// コロンとパスの区切り文字の付いたドライブ名 (c:\)、
        /// 任意のネットワーク共有名 (\\computer2\share1) のいずれかを指定します。
        /// </param>
        /// <returns>指定されたパスに含まれるドライブに対応する Drive オブジェクト。</returns>
        public Drive GetDrive(string DriveSpec)
        {
            if (!WidePath.IsUNC(DriveSpec))
                DriveSpec = new string(
                                new char[] { 
                                    DriveSpec[0],
                                    WidePath.VolumeSeparatorChar,
                                    WidePath.DirectorySeparatorChar
                                }
                            );

            return new Drive(this, DriveSpec);
        }

        /// <summary>
        /// 指定されたパスが置かれているドライブの名前の入った文字列を返します。
        /// </summary>
        /// <param name="Path">ドライブ名を取り出すパスを指定します。</param>
        /// <returns>ドライブ名。</returns>
        public string GetDriveName(string Path)
        {
            WidePath f = Path;
            return f.DriveName;
        }

        /// <summary>
        /// 指定されたパスの拡張子を表す文字列を返します。
        /// </summary>
        /// <param name="Path">拡張子を取り出す構成要素のパスを指定します。</param>
        /// <returns>指定されたパスの拡張子を表す文字列。</returns>
        public string GetExtensionName(string Path)
        {
            return WidePath.GetExtensionName(Path);
        }

        /// <summary>
        /// 指定されたパスに置かれているファイルに対応する File オブジェクトを返します。
        /// </summary>
        /// <param name="FilePath">目的のファイルのパスを指定します。絶対パス、または相対パスのどちらかを指定できます。</param>
        /// <returns>指定されたパスに置かれているファイルに対応する File オブジェクト。</returns>
        public File GetFile(string FilePath)
        {
            return new File(this, FilePath);
        }

        /// <summary>
        /// 指定されたパスの最後の構成要素を返します。
        /// </summary>
        /// <param name="Path">目的のファイルのパスを指定します。絶対パス、または相対パスのどちらかを指定できます。</param>
        /// <returns>指定されたパスの最後の構成要素。</returns>
        public string GetFileName(string Path)
        {
            return WidePath.GetFileName(Path);
        }

        /// <summary>
        /// 指定されたパスに置かれているフォルダに対応する Folder オブジェクトを返します。
        /// </summary>
        /// <param name="FolderPath">目的のフォルダのパスを指定します。</param>
        /// <returns>指定されたパスに置かれているフォルダに対応する Folder オブジェクト</returns>
        public Folder GetFolder(string FolderPath)
        {
            return new Folder(this, FolderPath);
        }

        /// <summary>
        /// 指定されたパスの最後の構成要素の親フォルダの名前が入った文字列を返します。
        /// </summary>
        /// <param name="Path">親フォルダの名前を取得する構成要素のパスを指定します。</param>
        /// <returns>親フォルダの名前。</returns>
        public string GetParentFolderName(string Path)
        {
            return WidePath.GetParentFolderName(Path);
        }

        /// <summary>
        /// 指定された特別なフォルダを返します。
        /// </summary>
        /// <param name="SpecialFolder">取得する特別なフォルダの種類を指定します。</param>
        /// <returns>特別なフォルダを示す Folder オブジェクト</returns>
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
        /// 一時ファイルまたは一時フォルダの名前をランダムに生成し、それを返します。
        /// </summary>
        /// <returns>一時ファイルまたは一時フォルダの名前。</returns>
        public string GetTempName()
        {
            return IO.Path.GetTempFileName();
        }

        /// <summary>
        /// ファイルを別の場所へ移動します。
        /// </summary>
        /// <param name="Source">移動するファイルを表す文字列を指定します。パスの最後の構成要素内ではワイルドカード文字を使用できます。</param>
        /// <param name="Destination">引数 source で指定したファイルの移動先を表す文字列を指定します。ワイルドカード文字は使用できません。</param>
        public void MoveFile(string Source, string Destination)
        {
            MoveFileInternal(new File(this, Source), Destination);
        }

        internal void MoveFileInternal(File file, string Destination)
        {
            NativeWrapper.MoveFileEx(file.Path, Destination);
        }

        /// <summary>
        /// フォルダを別の場所へ移動します。
        /// </summary>
        /// <param name="Source">移動するフォルダを表す文字列を指定します。パスの最後の構成要素内ではワイルドカード文字を使用できます。</param>
        /// <param name="Destination">引数 source で指定したフォルダの移動先を表す文字列を指定します。ワイルドカード文字は使用できません。</param>
        public void MoveFolder(string Source, string Destination)
        {
            MoveFolderInternal(new Folder(this, Source), Destination);
        }

        internal void MoveFolderInternal(Folder folder, string Destination)
        {
            NativeWrapper.MoveFileEx(folder.Path, Destination);
        }

        /// <summary>
        /// ファイルのバージョン情報を取得します。
        /// </summary>
        /// <param name="FileName">バージョン情報を取得するファイル名を示す文字列式を指定します。</param>
        /// <returns>ファイルのバージョン情報。</returns>
        public string GetFileVersion(string FileName)
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(FileName);
            return fvi.ProductVersion;
        }

        /// <summary>
        /// 指定されたファイル名のファイルを作成し、ファイルを読み込んだり、書き込むときに使用する TextStream オブジェクトを返します。
        /// </summary>
        /// <param name="FileName">作成するファイルを示す文字列式を指定します。</param>
        /// <param name="Overwrite">
        /// 既にあるファイルを上書きするかどうかを示すブール値を指定します。
        /// ファイルを上書きする場合は、真 (True) を指定し、上書きしない場合は、偽 (False) を指定します。
        /// 省略すると、ファイルは上書きされません。</param>
        /// <param name="Unicode">
        /// Unicode または ASCII ファイルのいずれかで作成されたファイルかを示すブール値を指定します。
        /// Unicode で作成されたファイルでは、真 (True) を指定します。
        /// ASCII ファイルで作成されたファイルでは、偽 (False) を指定します。
        /// 省略した場合、ASCII ファイルで作成されたファイルとします。
        /// </param>
        /// <returns>TextStream オブジェクトを返します。</returns>
        public TextStream CreateTextFile(string FileName, bool Overwrite = false, bool Unicode = false)
        {
            Tristate Format = Unicode ? Tristate.TristateTrue : Tristate.TristateFalse;
            return new TextStreamClass(FileName, IOMode.ForWriting, Overwrite, Format);
        }

        /// <summary>
        /// 指定されたファイルを開き、ファイルを読み込んだり、文字列を追加するときに使用する TextStream オブジェクトを返します。
        /// </summary>
        /// <param name="FileName">開くファイルを示す文字列式を指定します。</param>
        /// <param name="IOMode">読み込み、または書き込みのいずれのためにファイルを開くのかを示す、IOMode 定数を指定します。</param>
        /// <param name="Create">
        /// 引数 filename で指定されたファイルが存在しない場合に新しいファイルを作成するかどうかを示すブール値を指定します。
        /// 新しいファイルを作成する場合は、真 (True) を指定し、作成しない場合は、偽 (False) を指定します。
        /// 省略すると、新しいファイルは作成されません。
        /// </param>
        /// <param name="Format">開くファイルの形式を示す値を指定します。省略すると、ASCII ファイルとして開かれます。</param>
        /// <returns>TextStream オブジェクトを返します。</returns>
        public TextStream OpenTextFile(string FileName, IOMode IOMode = IOMode.ForReading, bool Create = false, Tristate Format = Tristate.TristateFalse)
        {
            return new TextStreamClass(FileName, IOMode, Create, Format);
        }

        /// <summary>
        /// 標準入出力の TextStream を返します。
        /// </summary>
        /// <param name="StandardStreamType">作成する標準入出力の種類を指定します。</param>
        /// <param name="Unicode">unicode を含むかどうかを指定します。</param>
        /// <returns>TextStream オブジェクトを返します。</returns>
        public TextStream GetStandardStream(StandardStreamTypes StandardStreamType, bool Unicode = false)
        {
            return new TextStreamClass(StandardStreamType, Unicode);
        }
    }
}