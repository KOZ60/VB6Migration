using System;
using IO = System.IO;
using System.Runtime.InteropServices;

namespace Scripting
{
    /// <summary>
    /// ファイルのあらゆるプロパティにアクセスする手段を提供します。
    /// </summary>
	public class File : ABSClass
	{
        internal File(FileSystemObject fso, FileNameClass fileName)
            : base(fso, fileName, FileAttribute.Normal)
        {
        }

        /// <summary>
        /// 指定されたファイルのバイト単位のサイズを返します。
        /// </summary>
        public override long Size
        {
            get
            {
                var findData = data;
                return NativeWrapper.MAKELONG(findData.nFileSizeLow, findData.nFileSizeHigh);
            }
        }

        /// <summary>
        /// 指定されたファイルを別の場所へコピーします。
        /// </summary>
        /// <param name="Destination">ファイルのコピー先を指定します。ワイルドカード文字は指定できません。</param>
        /// <param name="OverWriteFiles">既存ファイルや既存フォルダを上書きする場合は、既定値の真 (True) を指定します。上書きしない場合は、偽 (False) を指定します。</param>
        public override void Copy(string Destination, bool OverWriteFiles = false)
        {
            fso.CopyFileInternal(info.DisplayFileName, Destination, OverWriteFiles);
        }

        /// <summary>
        /// 指定されたファイルを削除します。
        /// </summary>
        /// <param name="Force">
        /// 真 (True) を指定すると読み取り専用のファイルやフォルダも削除されます。
        /// 既定値の偽 (False) を指定すると読み取り専用のファイルやフォルダを削除しません。
        /// </param>
        public override void Delete(bool Force = false)
        {
            fso.DeleteFileInternal(this, Force);
        }

        /// <summary>
        /// 指定されたファイルを移動します。
        /// </summary>
        /// <param name="Destination">ファイルの移動先を指定します。ワイルドカード文字は指定できません。</param>
        public override void Move(string Destination)
        {
            fso.MoveFileInternal(this, Destination);
            info = Destination;
        }

        /// <summary>
        /// 指定されたファイルを開き、開いたファイルの読み取り、書き込み、または追加書き込みに使用できる TextStream オブジェクトを返します。
        /// </summary>
        /// <param name="IOMode">入出力モードを指定します。</param>
        /// <param name="Format">開くファイルの形式を指定します。</param>
        /// <returns>TextStream オブジェクト</returns>
        public TextStream OpenAsTextStream(
                                IOMode IOMode = IOMode.ForReading,
                                Tristate Format = Tristate.TristateFalse)
        {
            return new TextStreamClass(this.Path, IOMode, false, Format);
        }
    }
}