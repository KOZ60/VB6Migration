using System;
using IO = System.IO;

namespace Scripting
{
    /// <summary>
    /// �t�H���_���̂��ׂĂ� File �I�u�W�F�N�g�̃R���N�V�����ł��B
    /// </summary>
    public class Files : ABSCollcetion<File>
	{
        internal Files(FileSystemObject fso, FileNameClass fileName)
            : base(fso, fileName, FileAttribute.Normal)
        {
        }

        /// <summary>
        /// ����̌^�̃n�b�V���֐��Ƃ��ċ@�\���܂��B
        /// </summary>
        /// <returns>���݂� Object �̃n�b�V�� �R�[�h�B </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// �w�肵�����O�� File �I�u�W�F�N�g���쐬���܂��B
        /// </summary>
        /// <param name="name">�t�@�C����</param>
        /// <returns>File �I�u�W�F�N�g</returns>
        protected override File GetItem(string name)
        {
            string path = fso.BuildPath(ParentFolder.DisplayFileName, name);
            return new File(fso, path);
        }
    }
}