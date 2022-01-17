using System;

namespace Scripting
{
    /// <summary>
    /// 取得する特別なフォルダの種類を指定します。
    /// </summary>
	public enum SpecialFolderConst
	{
        /// <summary>
        /// Windows オペレーティング システムによりセットアップされたファイルの置かれている Windows フォルダが返されます。
        /// </summary>
		WindowsFolder,
        /// <summary>
        /// ライブラリ、フォント、デバイス ドライバなどの置かれている System フォルダが返されます。
        /// </summary>
		SystemFolder,
        /// <summary>
        /// 一時ファイルの格納に使用される Temp フォルダが返されます。このパスは、環境変数 TMP より取得します。
        /// </summary>
		TemporaryFolder
	}
}