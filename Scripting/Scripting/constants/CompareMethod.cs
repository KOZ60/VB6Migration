using System;

namespace Scripting
{
    /// <summary>
    /// Dictionary �I�u�W�F�N�g�̕������r�L�[�̔�r���[�h�B
    /// </summary>
	public enum CompareMethod
	{
        /// <summary>
        /// �o�C�i���Ŕ�r���܂��B
        /// </summary>
		BinaryCompare,
        /// <summary>
        /// �啶���������𖳎����Ĕ�r���܂��B
        /// </summary>
		TextCompare,
        /// <summary>
        /// Microsoft Access �̏ꍇ�̂ݗL���B�f�[�^�x�[�X�Ɋi�[����Ă���ݒ�Ɋ�Â��Ĕ�r���s���܂��B
        /// </summary>
		DatabaseCompare
	}
}