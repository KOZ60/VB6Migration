using System;
using IO = System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Scripting
{
    /// <summary>
    /// フォルダのあらゆるプロパティにアクセスする手段を提供します。
    /// </summary>
	public class Folder : ABSClass
	{
        internal Folder(FileSystemObject fso, WidePath fileName)
            : base(fso, fileName, FileAttribute.Directory)
        {
            Drive drv = new Drive(fso, fileName);
            if (!drv.IsReady)
                throw new IO.IOException(string.Format("ドライブの準備ができていません。{0}", drv.Path));
        }

        /// <summary>
        /// 指定されたフォルダ内に置かれているすべての File オブジェクトの入った Files コレクションを返します。このコレクションには、隠しファイルやシステム ファイルの属性を持つ File オブジェクトも含まれます。
        /// </summary>
        /// <value>Files コレクション</value>
        public Files Files
        {
            get { return new Files(fso, info.Display); }
        }

        /// <summary>
        /// 指定されたフォルダがルート フォルダかどうかを取得します。
        /// </summary>
        /// <value>指定されたフォルダがルート フォルダの場合は、真 (True) を返します。ルート フォルダでなければ、偽 (False) を返します。</value>
        public bool IsRootFolder
        {
            get { return WidePath.IsRootFolder(info.Display); }
        }

        /// <summary>
        /// 指定されたファイルまたはフォルダが格納されているフォルダを表す Folder オブジェクトを返します。取得のみ可能です。
        /// </summary>
        /// <value>
        /// 親フォルダを示す Folder オブジェクト。ルートフォルダの場合は null を返します。
        /// </value>
        public override Folder ParentFolder
        {
            get 
            {
                if (!IsRootFolder)
                {
                    return base.ParentFolder;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// 指定された位置のフォルダ内のファイル名を返します。
        /// </summary>
        /// <param name="index">位置を指定する整数値を指定します。</param>
        /// <returns>ファイル名</returns>
        public string this[int index]
        {
            get { return this.Files[index].Name; }
        }

        /// <summary>
        /// 指定されたフォルダ内のすべてのファイルおよびフォルダの合計サイズをバイト単位で返します。
        /// </summary>
        /// <value>フォルダの合計サイズ。</value>
        public override long Size
        {
            get { return NativeWrapper.EnumFolderSize(info.Display); }
        }

        /// <summary>
        /// 指定されたフォルダ内に置かれているすべてのフォルダの入った Folders コレクションを返します。このコレクションには、隠しファイルやシステム ファイルの属性を持つフォルダも含まれます。
        /// </summary>
        /// <value>Folder コレクション</value>
        public Folders SubFolders
        {
            get { return new Folders(fso, info.Display); }
        }

        /// <summary>
        /// 指定されたフォルダを別の場所へコピーします。
        /// </summary>
        /// <param name="Destination">フォルダのコピー先を指定します。</param>
        /// <param name="OverWriteFiles">既存ファイルや既存フォルダを上書きする場合は、真 (True) を指定します。上書きしない場合は、偽 (False) を指定します。</param>
        public override void Copy(string Destination, bool OverWriteFiles = false)
        {
            fso.CopyFolderInternal(this, Destination, OverWriteFiles);
        }

        /// <summary>
        /// 指定されたフォルダを削除します。
        /// </summary>
        /// <param name="Force">真 (True) を指定すると読み取り専用のファイルやフォルダも削除されます。</param>
        public override void Delete(bool Force = false)
        {
            fso.DeleteFolderInternal(this, Force);
        }

        /// <summary>
        /// 指定されたフォルダを別の場所へ移動します。
        /// </summary>
        /// <param name="Destination">ファイルまたはフォルダの移動先を指定します。ワイルドカード文字は指定できません。</param>
        public override void Move(string Destination)
        {
            fso.MoveFolderInternal(this, Destination);
        }

        /// <summary>
        /// 指定されたフォルダにファイルを作成し、ファイルを読み込んだり、書き込むときに使用する TextStream オブジェクトを返します。
        /// </summary>
        /// <param name="FileName">作成するファイルを示す文字列式を指定します。</param>
        /// <param name="Overwrite">
        /// 既にあるファイルを上書きするかどうかを示すブール値を指定します。
        /// ファイルを上書きする場合は、真 (True) を指定し、上書きしない場合は、偽 (False) を指定します。
        /// 省略すると、ファイルは上書きされません。</param>
        /// <param name="Unicode">Unicode または ASCII ファイルのいずれかで作成されたファイルかを示すブール値を指定します。
        /// Unicode で作成されたファイルでは、真 (True) を指定します。ASCII ファイルで作成されたファイルでは、偽 (False) を指定します。
        /// 省略した場合、ASCII ファイルで作成されたファイルとします。</param>
        /// <returns>TextStream オブジェクト</returns>
        public TextStream CreateTextFile(string FileName, bool Overwrite = false, bool Unicode = false)
        {
            string newPath = fso.BuildPath(this.Path, FileName);
            return fso.CreateTextFile(newPath, Overwrite, Unicode);
        }

    }
}