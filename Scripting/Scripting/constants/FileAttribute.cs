using System;

namespace Scripting
{
    /// <summary>
    /// �t�@�C���܂��̓t�H���_�̑�����ݒ肵�܂��B
    /// </summary>
	public enum FileAttribute
	{
        /// <summary>
        /// �W���t�@�C���B�ǂ̑������ݒ肳��܂���B
        /// </summary>
		Normal = 0,
        /// <summary>
        /// �ǂݎ���p�t�@�C���B
        /// </summary>
		ReadOnly = 1,
        /// <summary>
        /// �B���t�@�C���B
        /// </summary>
		Hidden = 2,
        /// <summary>
        /// �V�X�e�� �t�@�C���B
        /// </summary>
		System = 4,
        /// <summary>
        /// �f�B�X�N �h���C�u �{�����[�� ���x���B
        /// </summary>
		Volume = 8,
        /// <summary>
        /// �t�H���_�܂��̓f�B���N�g���B
        /// </summary>
		Directory = 16,
        /// <summary>
        /// �t�@�C�����O��̃o�b�N�A�b�v�ȍ~�ɕύX����Ă��邩�ǂ����B
        /// </summary>
		Archive = 32,
        /// <summary>
        /// �����N�܂��̓V���[�g�J�b�g�B
        /// </summary>
		Alias = 1024,
        /// <summary>
        /// ���k�t�@�C���B
        /// </summary>
		Compressed = 2048
	}
}