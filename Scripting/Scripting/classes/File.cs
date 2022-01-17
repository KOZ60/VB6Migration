using System;
using IO = System.IO;
using System.Runtime.InteropServices;

namespace Scripting
{
    /// <summary>
    /// �t�@�C���̂�����v���p�e�B�ɃA�N�Z�X�����i��񋟂��܂��B
    /// </summary>
	public class File : ABSClass
	{
        internal File(FileSystemObject fso, FileNameClass fileName)
            : base(fso, fileName, FileAttribute.Normal)
        {
        }

        /// <summary>
        /// �w�肳�ꂽ�t�@�C���̃o�C�g�P�ʂ̃T�C�Y��Ԃ��܂��B
        /// </summary>
        public override long Size
        {
            get
            {
                var findData = data;
                return NativeWrapper.MAKELONG(findData.nFileSizeLow, findData.nFileSizeHigh);
            }
        }

        /// <summary>
        /// �w�肳�ꂽ�t�@�C����ʂ̏ꏊ�փR�s�[���܂��B
        /// </summary>
        /// <param name="Destination">�t�@�C���̃R�s�[����w�肵�܂��B���C���h�J�[�h�����͎w��ł��܂���B</param>
        /// <param name="OverWriteFiles">�����t�@�C��������t�H���_���㏑������ꍇ�́A����l�̐^ (True) ���w�肵�܂��B�㏑�����Ȃ��ꍇ�́A�U (False) ���w�肵�܂��B</param>
        public override void Copy(string Destination, bool OverWriteFiles = false)
        {
            fso.CopyFileInternal(info.DisplayFileName, Destination, OverWriteFiles);
        }

        /// <summary>
        /// �w�肳�ꂽ�t�@�C�����폜���܂��B
        /// </summary>
        /// <param name="Force">
        /// �^ (True) ���w�肷��Ɠǂݎ���p�̃t�@�C����t�H���_���폜����܂��B
        /// ����l�̋U (False) ���w�肷��Ɠǂݎ���p�̃t�@�C����t�H���_���폜���܂���B
        /// </param>
        public override void Delete(bool Force = false)
        {
            fso.DeleteFileInternal(this, Force);
        }

        /// <summary>
        /// �w�肳�ꂽ�t�@�C�����ړ����܂��B
        /// </summary>
        /// <param name="Destination">�t�@�C���̈ړ�����w�肵�܂��B���C���h�J�[�h�����͎w��ł��܂���B</param>
        public override void Move(string Destination)
        {
            fso.MoveFileInternal(this, Destination);
            info = Destination;
        }

        /// <summary>
        /// �w�肳�ꂽ�t�@�C�����J���A�J�����t�@�C���̓ǂݎ��A�������݁A�܂��͒ǉ��������݂Ɏg�p�ł��� TextStream �I�u�W�F�N�g��Ԃ��܂��B
        /// </summary>
        /// <param name="IOMode">���o�̓��[�h���w�肵�܂��B</param>
        /// <param name="Format">�J���t�@�C���̌`�����w�肵�܂��B</param>
        /// <returns>TextStream �I�u�W�F�N�g</returns>
        public TextStream OpenAsTextStream(
                                IOMode IOMode = IOMode.ForReading,
                                Tristate Format = Tristate.TristateFalse)
        {
            return new TextStreamClass(this.Path, IOMode, false, Format);
        }
    }
}