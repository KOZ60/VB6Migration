using System;
using IO = System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Scripting
{
    /// <summary>
    /// �t�H���_�̂�����v���p�e�B�ɃA�N�Z�X�����i��񋟂��܂��B
    /// </summary>
	public class Folder : ABSClass
	{
        internal Folder(FileSystemObject fso, FileNameClass fileName)
            : base(fso, fileName, FileAttribute.Directory)
        {
            Drive drv = new Drive(fso, fileName);
            if (!drv.IsReady)
                throw new IO.IOException(string.Format("�h���C�u�̏������ł��Ă��܂���B{0}", drv.Path));
        }

        /// <summary>
        /// �w�肳�ꂽ�t�H���_���ɒu����Ă��邷�ׂĂ� File �I�u�W�F�N�g�̓����� Files �R���N�V������Ԃ��܂��B���̃R���N�V�����ɂ́A�B���t�@�C����V�X�e�� �t�@�C���̑��������� File �I�u�W�F�N�g���܂܂�܂��B
        /// </summary>
        /// <value>Files �R���N�V����</value>
        public Files Files
        {
            get { return new Files(fso, info.DisplayFileName); }
        }

        /// <summary>
        /// �w�肳�ꂽ�t�H���_�����[�g �t�H���_���ǂ������擾���܂��B
        /// </summary>
        /// <value>�w�肳�ꂽ�t�H���_�����[�g �t�H���_�̏ꍇ�́A�^ (True) ��Ԃ��܂��B���[�g �t�H���_�łȂ���΁A�U (False) ��Ԃ��܂��B</value>
        public bool IsRootFolder
        {
            get { return FileNameClass.IsRootFolder(info.DisplayFileName); }
        }

        /// <summary>
        /// �w�肳�ꂽ�t�@�C���܂��̓t�H���_���i�[����Ă���t�H���_��\�� Folder �I�u�W�F�N�g��Ԃ��܂��B�擾�̂݉\�ł��B
        /// </summary>
        /// <value>
        /// �e�t�H���_������ Folder �I�u�W�F�N�g�B���[�g�t�H���_�̏ꍇ�� null ��Ԃ��܂��B
        /// </value>
        public override Folder ParentFolder
        {
            get 
            {
                if (!IsRootFolder)
                {
                    return base.ParentFolder;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// �w�肳�ꂽ�ʒu�̃t�H���_���̃t�@�C������Ԃ��܂��B
        /// </summary>
        /// <param name="index">�ʒu���w�肷�鐮���l���w�肵�܂��B</param>
        /// <returns>�t�@�C����</returns>
        public string this[int index]
        {
            get { return this.Files[index].Name; }
        }

        /// <summary>
        /// �w�肳�ꂽ�t�H���_���̂��ׂẴt�@�C������уt�H���_�̍��v�T�C�Y���o�C�g�P�ʂŕԂ��܂��B
        /// </summary>
        /// <value>�t�H���_�̍��v�T�C�Y�B</value>
        public override long Size
        {
            get { return NativeWrapper.EnumFolderSize(info.DisplayFileName); }
        }

        /// <summary>
        /// �w�肳�ꂽ�t�H���_���ɒu����Ă��邷�ׂẴt�H���_�̓����� Folders �R���N�V������Ԃ��܂��B���̃R���N�V�����ɂ́A�B���t�@�C����V�X�e�� �t�@�C���̑��������t�H���_���܂܂�܂��B
        /// </summary>
        /// <value>Folder �R���N�V����</value>
        public Folders SubFolders
        {
            get { return new Folders(fso, info.DisplayFileName); }
        }

        /// <summary>
        /// �w�肳�ꂽ�t�H���_��ʂ̏ꏊ�փR�s�[���܂��B
        /// </summary>
        /// <param name="Destination">�t�H���_�̃R�s�[����w�肵�܂��B</param>
        /// <param name="OverWriteFiles">�����t�@�C��������t�H���_���㏑������ꍇ�́A�^ (True) ���w�肵�܂��B�㏑�����Ȃ��ꍇ�́A�U (False) ���w�肵�܂��B</param>
        public override void Copy(string Destination, bool OverWriteFiles = false)
        {
            fso.CopyFolderInternal(this, Destination, OverWriteFiles);
        }

        /// <summary>
        /// �w�肳�ꂽ�t�H���_���폜���܂��B
        /// </summary>
        /// <param name="Force">�^ (True) ���w�肷��Ɠǂݎ���p�̃t�@�C����t�H���_���폜����܂��B</param>
        public override void Delete(bool Force = false)
        {
            fso.DeleteFolderInternal(this, Force);
        }

        /// <summary>
        /// �w�肳�ꂽ�t�H���_��ʂ̏ꏊ�ֈړ����܂��B
        /// </summary>
        /// <param name="Destination">�t�@�C���܂��̓t�H���_�̈ړ�����w�肵�܂��B���C���h�J�[�h�����͎w��ł��܂���B</param>
        public override void Move(string Destination)
        {
            fso.MoveFolderInternal(this, Destination);
        }

        /// <summary>
        /// �w�肳�ꂽ�t�H���_�Ƀt�@�C�����쐬���A�t�@�C����ǂݍ��񂾂�A�������ނƂ��Ɏg�p���� TextStream �I�u�W�F�N�g��Ԃ��܂��B
        /// </summary>
        /// <param name="FileName">�쐬����t�@�C�������������񎮂��w�肵�܂��B</param>
        /// <param name="Overwrite">
        /// ���ɂ���t�@�C�����㏑�����邩�ǂ����������u�[���l���w�肵�܂��B
        /// �t�@�C�����㏑������ꍇ�́A�^ (True) ���w�肵�A�㏑�����Ȃ��ꍇ�́A�U (False) ���w�肵�܂��B
        /// �ȗ�����ƁA�t�@�C���͏㏑������܂���B</param>
        /// <param name="Unicode">Unicode �܂��� ASCII �t�@�C���̂����ꂩ�ō쐬���ꂽ�t�@�C�����������u�[���l���w�肵�܂��B
        /// Unicode �ō쐬���ꂽ�t�@�C���ł́A�^ (True) ���w�肵�܂��BASCII �t�@�C���ō쐬���ꂽ�t�@�C���ł́A�U (False) ���w�肵�܂��B
        /// �ȗ������ꍇ�AASCII �t�@�C���ō쐬���ꂽ�t�@�C���Ƃ��܂��B</param>
        /// <returns>TextStream �I�u�W�F�N�g</returns>
        public TextStream CreateTextFile(string FileName, bool Overwrite = false, bool Unicode = false)
        {
            string newPath = fso.BuildPath(this.Path, FileName);
            return fso.CreateTextFile(newPath, Overwrite, Unicode);
        }

    }
}