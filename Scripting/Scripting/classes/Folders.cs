using System;

namespace Scripting
{
    /// <summary>
    /// 1 つの Folder オブジェクトに含まれるすべての Folder オブジェクトのコレクションです。
    /// </summary>
    public class Folders : ABSCollcetion<Folder>
    {
        internal Folders(FileSystemObject fso, string parentFolder)
            : base(fso, parentFolder, FileAttribute.Directory)
        {
        }

        /// <summary>
        /// 指定した名前からフォルダーオブジェクトを作成します。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override Folder GetItem(string name)
        {
            string path = fso.BuildPath(ParentFolder.DisplayFileName, name);
            return new Folder(fso, path);
        }

        /// <summary>
        /// フォルダーを追加します。
        /// </summary>
        /// <param name="Name">追加する引数 item と関連付けられた引数 key を指定します。</param>
        /// <returns>追加したフォルダーを示す Folder オブジェクトが返されます。</returns>
        public Folder Add(string Name)
        {
            return fso.CreateFolder(fso.BuildPath(ParentFolder.DisplayFileName, Name));
        }
    }
}