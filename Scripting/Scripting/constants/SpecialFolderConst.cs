using System;

namespace Scripting
{
    /// <summary>
    /// �擾������ʂȃt�H���_�̎�ނ��w�肵�܂��B
    /// </summary>
	public enum SpecialFolderConst
	{
        /// <summary>
        /// Windows �I�y���[�e�B���O �V�X�e���ɂ��Z�b�g�A�b�v���ꂽ�t�@�C���̒u����Ă��� Windows �t�H���_���Ԃ���܂��B
        /// </summary>
		WindowsFolder,
        /// <summary>
        /// ���C�u�����A�t�H���g�A�f�o�C�X �h���C�o�Ȃǂ̒u����Ă��� System �t�H���_���Ԃ���܂��B
        /// </summary>
		SystemFolder,
        /// <summary>
        /// �ꎞ�t�@�C���̊i�[�Ɏg�p����� Temp �t�H���_���Ԃ���܂��B���̃p�X�́A���ϐ� TMP ���擾���܂��B
        /// </summary>
		TemporaryFolder
	}
}