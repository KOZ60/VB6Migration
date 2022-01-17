using System;

namespace Scripting
{
    /// <summary>
    /// ファイルまたはフォルダの属性を設定します。
    /// </summary>
	public enum FileAttribute
	{
        /// <summary>
        /// 標準ファイル。どの属性も設定されません。
        /// </summary>
		Normal = 0,
        /// <summary>
        /// 読み取り専用ファイル。
        /// </summary>
		ReadOnly = 1,
        /// <summary>
        /// 隠しファイル。
        /// </summary>
		Hidden = 2,
        /// <summary>
        /// システム ファイル。
        /// </summary>
		System = 4,
        /// <summary>
        /// ディスク ドライブ ボリューム ラベル。
        /// </summary>
		Volume = 8,
        /// <summary>
        /// フォルダまたはディレクトリ。
        /// </summary>
		Directory = 16,
        /// <summary>
        /// ファイルが前回のバックアップ以降に変更されているかどうか。
        /// </summary>
		Archive = 32,
        /// <summary>
        /// リンクまたはショートカット。
        /// </summary>
		Alias = 1024,
        /// <summary>
        /// 圧縮ファイル。
        /// </summary>
		Compressed = 2048
	}
}