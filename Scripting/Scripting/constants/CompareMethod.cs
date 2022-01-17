using System;

namespace Scripting
{
    /// <summary>
    /// Dictionary オブジェクトの文字列比較キーの比較モード。
    /// </summary>
	public enum CompareMethod
	{
        /// <summary>
        /// バイナリで比較します。
        /// </summary>
		BinaryCompare,
        /// <summary>
        /// 大文字小文字を無視して比較します。
        /// </summary>
		TextCompare,
        /// <summary>
        /// Microsoft Access の場合のみ有効。データベースに格納されている設定に基づいて比較を行います。
        /// </summary>
		DatabaseCompare
	}
}