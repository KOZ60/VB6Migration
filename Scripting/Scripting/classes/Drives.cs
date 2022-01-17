using System;
using IO = System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Scripting
{
    /// <summary>
    /// ���p�\�Ȃ��ׂẴh���C�u�̃R���N�V�����ł��B���̃R���N�V�����́A�����o�̎擾�̂݉\�ł��B
    /// </summary>
    public class Drives : MarshalByRefObject, IEnumerable, IEnumerable<Drive>
	{
        Dictionary<string, Drive> m_DriveCollection;

        internal Drives(FileSystemObject fso)
        {
            m_DriveCollection = new Dictionary<string, Drive>();
            IO.DriveInfo[] infos = IO.DriveInfo.GetDrives();
            for (int i = 0; i < infos.Length; i++)
            {
                Drive d = new Drive(fso, infos[i].Name);
                m_DriveCollection.Add(d.DriveLetter, d);
            }
        }

        /// <summary>
        /// ���݂̃I�u�W�F�N�g��\���������Ԃ��܂��B
        /// </summary>
        /// <returns>�I�u�W�F�N�g��\���������Ԃ��܂��B</returns>
        public override string ToString()
        {
            return m_DriveCollection.ToString();
        }

        /// <summary>
        /// �R���N�V�����Ɋ܂܂�錏����Ԃ��܂��B
        /// </summary>
        /// <value>�R���N�V�����Ɋ܂܂�錏��</value>
        public int Count
        {
            get { return m_DriveCollection.Count; }
        }

        /// <summary>
        /// �w�肵���h���C�u�����g�p�ł��邩���擾���܂��B
        /// </summary>
        /// <param name="DriveLetter">�h���C�u��</param>
        /// <returns>�g�p�ł���ꍇ�� True�A�o���Ȃ��ꍇ�� False</returns>
        public bool Exists(string DriveLetter)
        {
            return m_DriveCollection.ContainsKey(DriveLetter);
        }

        /// <summary>
        /// �w�肵���h���C�u�������� Drive �I�u�W�F�N�g��Ԃ��܂��B
        /// </summary>
        /// <param name="name">Drive �I�u�W�F�N�g���擾����h���C�u�����w�肵�܂��B</param>
        /// <returns>Drive �I�u�W�F�N�g</returns>
        public Drive this[string name]
        {
            get { return m_DriveCollection[name]; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_DriveCollection.GetEnumerator();
        }

        IEnumerator<Drive> IEnumerable<Drive>.GetEnumerator()
        {
            return m_DriveCollection.Values.GetEnumerator();
        }
    }
}