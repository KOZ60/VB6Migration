using System;

namespace Scripting
{
    /// <summary>
    /// �J���t�@�C���̌`�����w�肵�܂��B
    /// </summary>
	public enum Tristate
	{
        /// <summary>
        /// �V�X�e�� �f�t�H���g���g���ăt�@�C�����J���܂��B
        /// </summary>
		TristateMixed = -2,
        /// <summary>
        /// �V�X�e�� �f�t�H���g���g���ăt�@�C�����J���܂��B
        /// </summary>
		TristateUseDefault = -2,
        /// <summary>
        /// �t�@�C���� Unicode �t�@�C���Ƃ��ĊJ���܂��B
        /// </summary>
		TristateTrue = -1,
        /// <summary>
        /// �t�@�C���� ASCII �t�@�C���Ƃ��ĊJ���܂��B
        /// </summary>
		TristateFalse = 0
	}
}