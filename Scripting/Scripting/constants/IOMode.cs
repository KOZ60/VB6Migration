using System;

namespace Scripting
{
    /// <summary>
    /// ���o�̓��[�h���w�肵�܂��B
    /// </summary>
	public enum IOMode
	{
        /// <summary>
        /// �t�@�C����ǂݎ���p�Ƃ��ĊJ���܂��B
        /// </summary>
		ForReading = 1,
        /// <summary>
        /// �t�@�C�����������ݗp�ɊJ���܂��B�������O�̃t�@�C�������݂����ꍇ�A�ȑO�̓��e�͏㏑������܂��B
        /// </summary>
		ForWriting = 2,
        /// <summary>
        /// �t�@�C�����J���A�t�@�C���̍Ōォ�珑�����݂��s���܂��B
        /// </summary>
		ForAppending = 8
	}
}