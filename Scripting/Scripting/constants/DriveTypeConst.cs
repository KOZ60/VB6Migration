using System;

namespace Scripting
{
    /// <summary>
    /// �w�肳�ꂽ�h���C�u�̎�ނ������l��Ԃ��܂��B
    /// </summary>
	public enum DriveTypeConst
	{
        /// <summary>
        /// �s���ȃh���C�u�^�C�v�B
        /// </summary>
		UnknownType,
        /// <summary>
        /// �����[�o�u�����f�B�A
        /// </summary>
		Removable,
        /// <summary>
        /// �Œ胁�f�B�A
        /// </summary>
		Fixed,
        /// <summary>
        /// �����[�g�t�@�C��
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