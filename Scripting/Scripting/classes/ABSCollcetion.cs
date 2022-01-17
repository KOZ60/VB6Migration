using System;
using IO = System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Scripting
{

    /// <summary>
    /// コレクションの基底クラス
    /// </summary>
    /// <typeparam name="TItem">コレクションに格納する型を指定します。</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class ABSCollcetion<TItem> : MarshalByRefObject, IEnumerable, IEnumerable<TItem>
        where TItem: ABSClass
    {
        private FileNameClass m_ParentFolder;
        private FileSystemObject m_FileSystemObject;
        private Dictionary<string, string> m_Dictionary;
        private List<string> m_List;

        internal ABSCollcetion(FileSystemObject fso, FileNameClass parentFolder, FileAttribute attr)
        {
            m_ParentFolder = parentFolder;
            m_FileSystemObject = fso;
            m_Dictionary = new Dictionary<string, string>();
            m_List = new List<string>();

            string tempPath = fso.BuildPath(parentFolder.DisplayFileName, "*");
            NativeMethods.WIN32_FIND_DATA fd = new NativeMethods.WIN32_FIND_DATA();
            IntPtr handle = NativeWrapper.FindFirstFile(tempPath, ref fd);
            if (handle == (IntPtr)NativeMethods.INVALID_HANDLE_VALUE)
            {
                int errorCode = Marshal.GetLastWin32Error();
                throw NativeWrapper.CreateIOException(errorCode);
            }

            HandleRef handleRef = new HandleRef(new object(), handle);

            do
            {
                if (string.Compare(fd.cFileName, ".") != 0 && string.Compare(fd.cFileName, "..") != 0)
                {
                    if ((fd.dwFileAttributes &  FileAttribute.Directory) == attr)
                    {
                        m_Dictionary.Add(fd.cFileName, fd.cFileName);
                        m_List.Add(fd.cFileName);
                    }
                }
            } while (NativeWrapper.FindNextFile(handleRef, ref fd));

            bool result = NativeWrapper.FindClose(handleRef);
        }

        internal FileSystemObject fso
        {
            get { return m_FileSystemObject; }
        }

        internal FileNameClass ParentFolder
        {
            get { return m_ParentFolder; }
        }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>ディクショナリのファイル名あるいはフォルダ名</returns>
        public override string ToString()
        {
            return string.Format("{0}[{1}]", typeof(TItem).Name, m_Dictionary.Count);
        }

        /// <summary>
        /// 継承クラスに内部コレクションへのアクセスを提供します。
        /// </summary>
        protected Dictionary<string, string> Collection
        {
            get { return m_Dictionary; }
        }

        /// <summary>
        /// 格納されているキー/値ペアの数を取得します。
        /// </summary>
        public int Count
        {
            get { return m_Dictionary.Count; }
        }

        /// <summary>
        /// 指定したインデックスから TItem  オブジェクトを作成します。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>TItem  オブジェクト</returns>
        public TItem this[int index]
        {
            get { return GetItem(m_List[index]); }
        }

        /// <summary>
        /// 指定したファイル名から TItem  オブジェクトを作成します。
        /// </summary>
        /// <param name="name">ファイル名</param>
        /// <returns>TItem オブジェクト</returns>
        public TItem this[string name]
        {
            get { return GetItem(Collection[name]); }
        }

        /// <summary>
        /// 指定したファイル名から TItem  オブジェクトを作成します。
        /// </summary>
        /// <param name="name">ファイル名</param>
        /// <returns>TItem オブジェクト</returns>
        protected abstract TItem GetItem(string name);

        /// <summary>
        /// ジェネリック コレクションに対する単純な反復処理をサポートします。
        /// </summary>
        /// <returns>IEnumerator&lt;TItem&gt;</returns>
        public IEnumerator<TItem> GetEnumerator()
        {
            return new ABSEnumurator<TItem>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ABSEnumurator<TItem>(this);
        }
    }
}
