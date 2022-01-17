using System;
using IO = System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Scripting
{
    /// <summary>
    /// File オブジェクトと Folder オブジェクトの基底クラスです。
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class ABSClass : MarshalByRefObject
    {
        private FileSystemObject m_FileSystemObject;
        private FileNameClass m_FileNameClass;
        private FileAttribute m_FileAttribute;  // Directory or Normal
        private NativeMethods.WIN32_FILE_ATTRIBUTE_DATA m_Data;

        internal ABSClass(FileSystemObject fso, FileNameClass fileName, FileAttribute fileAttribute)
        {
            m_FileSystemObject = fso;
            m_FileNameClass = fileName;
            m_FileAttribute = fileAttribute;
            Refresh();
        }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>現在のオブジェクトを表す文字列。</returns>
        public override string ToString()
        {
            return info.DisplayFileName;
        }

        internal void Refresh()
        {
            m_Data = new NativeMethods.WIN32_FILE_ATTRIBUTE_DATA();
            NativeWrapper.GetFileAttributesEx(info, ref m_Data);

            // 見つかったが属性が違う(ディレクトリまたはファイルの判定)
            if ((m_Data.dwFileAttributes & FileAttribute.Directory) != m_FileAttribute)
            {
                if (m_FileAttribute == FileAttribute.Directory)
                    throw NativeWrapper.CreateIOException(NativeMethods.ERROR_PATH_NOT_FOUND);
                else
                    throw NativeWrapper.CreateIOException(NativeMethods.ERROR_FILE_NOT_FOUND);
            }
        }

        internal FileSystemObject fso
        {
            get { return m_FileSystemObject; }
        }

        internal FileNameClass info
        {
            get { return m_FileNameClass; }
            set { m_FileNameClass = value; }
        }

        internal NativeMethods.WIN32_FILE_ATTRIBUTE_DATA data
        {
            get
            {
                Refresh();
                return m_Data;
            }
        }

        /// <summary>
        /// ファイルまたはフォルダの属性を設定します。値の取得も可能です。属性によっては、値の取得のみ可能な場合もあります。
        /// </summary>
        /// <value>FileAttribute 値</value>
        public FileAttribute Attributes
        {
            get { return data.dwFileAttributes; }
            set
            {
                NativeWrapper.SetFileAttributes(info.Win32FileName, value);
            }
        }

        /// <summary>
        /// 指定されたファイルまたはフォルダが作成された日付と時刻を返します。値の取得のみ可能です。
        /// </summary>
        /// <value>作成された日付と時刻</value>
        public DateTime DateCreated
        {
            get { return data.ftCreationTime.ToDate(); }
        }

        /// <summary>
        /// 指定されたファイルまたはフォルダが最後にアクセスされたときの日付と時刻を返します。値の取得のみ可能です。
        /// </summary>
        /// <value>最後にアクセスされたときの日付と時刻</value>
        public DateTime DateLastAccessed
        {
            get { return data.ftLastAccessTime.ToDate(); }
        }

        /// <summary>
        /// 指定されたファイルまたはフォルダが最後に更新されたときの日付と時刻を返します。値の取得のみ可能です。
        /// </summary>
        /// <value>最後に更新されたときの日付と時刻</value>
        public DateTime DateLastModified
        {
            get { return data.ftLastWriteTime.ToDate(); }
        }

        /// <summary>
        /// 指定されたファイルまたはフォルダが格納されているドライブの名前を返します。値の取得のみ可能です。
        /// </summary>
        public Drive Drive
        {
            get { return fso.GetDrive(info.DriveName); }
        }

        /// <summary>
        /// 指定されたファイルまたはフォルダの名前を設定します。値の取得も可能です。
        /// </summary>
        /// <value>指定されたファイルまたはフォルダの名前</value>
        public string Name
        {
            get { return FileNameClass.GetFileName(info.DisplayFileName); }
            set
            {
                string newFileName = fso.BuildPath(FileNameClass.GetParentFolderName(info.DisplayFileName), value);
                NativeWrapper.RenameFile(info, newFileName);
                info = newFileName;
            }
        }

        /// <summary>
        /// 指定されたファイルまたはフォルダが格納されているフォルダを表す Folder オブジェクトを返します。取得のみ可能です。
        /// </summary>
        /// <value>指定されたファイルまたはフォルダが格納されているフォルダを表す Folder オブジェクト</value>
        public virtual Folder ParentFolder
        {
            get 
            { 
                string parentFolderName = FileNameClass.GetParentFolderName(info.DisplayFileName);
                if (string.IsNullOrEmpty(parentFolderName))
                    return null;
                else
                    return new Folder(fso, parentFolderName); 
            }
        }

        /// <summary>
        /// ファイルまたはフォルダの種類に関する情報を返します。たとえば、名前が .TXT で終わるファイルの場合なら、"Text Document" という文字列が返されます。
        /// </summary>
        /// <value>ファイルまたはフォルダの種類に関する情報</value>
        public string Type
        {
            get { return NativeWrapper.GetTypeName(info); }
        }

        /// <summary>
        /// 指定されたファイル、フォルダ、またはドライブのパスを返します。
        /// </summary>
        public string Path
        {
            get { return info.DisplayFileName; }
        }

        /// <summary>
        /// 8.3 名前付け規則に従った名前を必要とするプログラムで使用できる短いパスを返します。
        /// </summary>
        public string ShortPath
        {
            get { return info.ShortPathName; }
        }

        /// <summary>
        /// 8.3 名前付け規則に従った名前を必要とするプログラムで使用できる短い名前を返します。
        /// </summary>
        public string ShortName
        {
            get { return FileNameClass.GetFileName(info.ShortPathName); }
        }

        /// <summary>
        /// 対象がファイルの場合、指定されたファイルのバイト単位のサイズを返します。対象がフォルダの場合、指定されたフォルダ内のすべてのファイルおよびフォルダの合計サイズをバイト単位で返します。
        /// </summary>
        public abstract long Size { get; }

        /// <summary>
        /// 指定されたファイルまたはフォルダを別の場所へコピーします。
        /// </summary>
        /// <param name="Destination">ファイルまたはフォルダのコピー先を指定します。ワイルドカード文字は指定できません。</param>
        /// <param name="OverWriteFiles">既存ファイルや既存フォルダを上書きする場合は、真 (True) を指定します。上書きしない場合は、既定値の偽 (False) を指定します。</param>
        public abstract void Copy(string Destination, bool OverWriteFiles = false);

        /// <summary>
        /// 指定されたファイルまたはフォルダを削除します。
        /// </summary>
        /// <param name="Force">
        /// 真 (True) を指定すると読み取り専用のファイルやフォルダも削除されます。
        /// 既定値の偽 (False) を指定すると読み取り専用のファイルやフォルダを削除しません。
        /// </param>
        public abstract void Delete(bool Force = false);

        /// <summary>
        /// 指定されたファイルまたはフォルダを別の場所へ移動します。
        /// </summary>
        /// <param name="Destination">移動先を指定します。</param>
        public abstract void Move(string Destination);
    }
}
