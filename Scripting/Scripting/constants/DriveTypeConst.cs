using System;

namespace Scripting
{
    /// <summary>
    /// 指定されたドライブの種類を示す値を返します。
    /// </summary>
	public enum DriveTypeConst
	{
        /// <summary>
        /// 不明なドライブタイプ。
        /// </summary>
		UnknownType,
        /// <summary>
        /// リムーバブルメディア
        /// </summary>
		Removable,
        /// <summary>
        /// 固定メディア
        /// </summary>
		Fixed,
        /// <summary>
        /// リモートファイル
        /// </summary>
		Remote,
        /// <summary>
        /// CDROM
        /// </summary>
		CDRom,
        /// <summary>
        /// RAMDISK
        /// </summary>
		RamDisk
	}
}