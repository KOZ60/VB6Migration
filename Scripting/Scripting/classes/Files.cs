using System;
using IO = System.IO;

namespace Scripting
{
    /// <summary>
    /// フォルダ内のすべての File オブジェクトのコレクションです。
    /// </summary>
    public class Files : ABSCollcetion<File>
	{
        internal Files(FileSystemObject fso, WidePath fileName)
            : base(fso, fileName, FileAttribute.Normal)
        {
        }

        /// <summary>
        /// 特定の型のハッシュ関数として機能します。
        /// </summary>
        /// <returns>現在の Object のハッシュ コード。 </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 指定した名前の File オブジェクトを作成します。
        /// </summary>
        /// <param name="name">ファイル名</param>
        /// <returns>File オブジェクト</returns>
        protected override File GetItem(string name)
        {
            string path = fso.BuildPath(ParentFolder.Display, name);
            return new File(fso, path);
        }
    }
}