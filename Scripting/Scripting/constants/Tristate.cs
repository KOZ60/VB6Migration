using System;

namespace Scripting
{
    /// <summary>
    /// 開くファイルの形式を指定します。
    /// </summary>
	public enum Tristate
	{
        /// <summary>
        /// システム デフォルトを使ってファイルを開きます。
        /// </summary>
		TristateMixed = -2,
        /// <summary>
        /// システム デフォルトを使ってファイルを開きます。
        /// </summary>
		TristateUseDefault = -2,
        /// <summary>
        /// ファイルを Unicode ファイルとして開きます。
        /// </summary>
		TristateTrue = -1,
        /// <summary>
        /// ファイルを ASCII ファイルとして開きます。
        /// </summary>
		TristateFalse = 0
	}
}