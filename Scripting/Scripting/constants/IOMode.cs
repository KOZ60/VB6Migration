using System;

namespace Scripting
{
    /// <summary>
    /// 入出力モードを指定します。
    /// </summary>
	public enum IOMode
	{
        /// <summary>
        /// ファイルを読み取り専用として開きます。
        /// </summary>
		ForReading = 1,
        /// <summary>
        /// ファイルを書き込み用に開きます。同じ名前のファイルが存在した場合、以前の内容は上書きされます。
        /// </summary>
		ForWriting = 2,
        /// <summary>
        /// ファイルを開き、ファイルの最後から書き込みを行います。
        /// </summary>
		ForAppending = 8
	}
}