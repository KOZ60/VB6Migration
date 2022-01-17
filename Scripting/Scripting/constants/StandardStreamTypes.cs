using System;

namespace Scripting
{
    /// <summary>
    /// 標準入出力の種類を指定します。
    /// </summary>
	public enum StandardStreamTypes
	{
        /// <summary>
        /// 標準入力
        /// </summary>
		StdIn,
        /// <summary>
        /// 標準出力
        /// </summary>
		StdOut,
        /// <summary>
        /// 標準エラー出力
        /// </summary>
		StdErr
	}
}