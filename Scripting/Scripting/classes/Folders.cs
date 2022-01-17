using System;

namespace Scripting
{
    /// <summary>
    /// 1 �� Folder �I�u�W�F�N�g�Ɋ܂܂�邷�ׂĂ� Folder �I�u�W�F�N�g�̃R���N�V�����ł��B
    /// </summary>
    public class Folders : ABSCollcetion<Folder>
    {
        internal Folders(FileSystemObject fso, string parentFolder)
            : base(fso, parentFolder, FileAttribute.Directory)
        {
        }

        /// <summary>
        /// �w�肵�����O����t�H���_�[�I�u�W�F�N�g���쐬���܂��B
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override Folder GetItem(string name)
        {
            string path = fso.BuildPath(ParentFolder.DisplayFileName, name);
            return new Folder(fso, path);
        }

        /// <summary>
        /// �t�H���_�[��ǉ����܂��B
        /// </summary>
        /// <param name="Name">�ǉ�������� item �Ɗ֘A�t����ꂽ���� key ���w�肵�܂��B</param>
        /// <returns>�ǉ������t�H���_�[������ Folder �I�u�W�F�N�g���Ԃ���܂��B</returns>
        public Folder Add(string Name)
        {
            return fso.CreateFolder(fso.BuildPath(ParentFolder.DisplayFileName, Name));
        }
    }
}