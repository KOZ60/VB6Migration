using System;
using System.Collections;
using System.ComponentModel;
using NET = System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Scripting
{
    /// <summary>
    /// �f�[�^ �L�[�Ƒg�݂ƂȂ鍀�ڂ��i�[����I�u�W�F�N�g�ł��B
    /// </summary>
    public class Dictionary : MarshalByRefObject
    {
        internal class NETDictionary : NET.Dictionary<string, object>
        {
            public NETDictionary(NET.IEqualityComparer<string> compare)
                : base(compare)
            {
            }
        }

        private NETDictionary m_Dictionary;
        private CompareMethod m_CompareMode;
        private static Exception OPERATION_EXCEPTION
            = new InvalidOperationException("��ۼ��ެ�̌Ăяo���A�܂��͈������s���ł��B");

        /// <summary>
        /// Dictionary �I�u�W�F�N�g�̃C���X�^���X���쐬���܂��B
        /// </summary>
        public Dictionary()
        {
            CreateDictionary(CompareMethod.BinaryCompare);
        }

        private void CreateDictionary(CompareMethod compareMode)
        {
            if (m_Dictionary != null && m_Dictionary.Count > 0)
                throw OPERATION_EXCEPTION;

            switch (compareMode)
            {
                case CompareMethod.BinaryCompare:
                    m_Dictionary = new NETDictionary(StringComparer.Ordinal);
                    break;

                case CompareMethod.TextCompare:
                    m_Dictionary = new NETDictionary(StringComparer.OrdinalIgnoreCase);
                    break;

                case CompareMethod.DatabaseCompare:
                    m_Dictionary = new NETDictionary(StringComparer.CurrentCulture);
                    break;

                default:
                    throw OPERATION_EXCEPTION;
            }
            m_CompareMode = compareMode;
        }

        /// <summary>
        /// Dictionary �I�u�W�F�N�g�ɕ������r�L�[�̔�r���[�h��ݒ肵�܂��B�l�̎擾���\�ł��B
        /// </summary>
        /// <value>CompareMethod</value>
        public CompareMethod CompareMode
        {
            get { return m_CompareMode; }
            set
            {
                if (m_CompareMode != value)
                {
                    CreateDictionary(value);
                }
            }
        }

        /// <summary>
        /// Dictionary �I�u�W�F�N�g�Ɋ܂܂�鍀�ڂ̐���Ԃ��܂��B�l�̎擾�̂݉\�ł��B
        /// </summary>
        /// <value>Dictionary �I�u�W�F�N�g�Ɋ܂܂�鍀�ڂ̐�</value>
        public int Count
        {
            get { return m_Dictionary.Count; }
        }

        /// <summary>
        /// �w�肵���L�[�̃n�b�V�� �R�[�h��Ԃ��܂��B
        /// </summary>
        /// <param name="Key">�n�b�V���R�[�h�𒲂ׂ�L�[</param>
        /// <returns>�n�b�V���R�[�h</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public object HashVal(string Key)
        {
            return Key.GetHashCode();
        }

        /// <summary>
        /// Dictionary �I�u�W�F�N�g�ɂ���w�肳�ꂽ�L�[�Ɗ֘A�t���鍀�ڂ�ݒ肵�܂��B
        /// </summary>
        /// <param name="Key">�擾�܂��͒ǉ����鍀�ڂƊ֘A�t����L�[���w�肵�܂��B</param>
        /// <returns></returns>
        public object this[string Key]
        {
            get { return m_Dictionary[Key]; }
            set { m_Dictionary[Key] = value; }
        }

        /// <summary>
        /// Dictionary �I�u�W�F�N�g�ɃL�[��ݒ肵�܂��B�l�̎擾���\�ł��B
        /// </summary>
        /// <remarks>object.Key(key) = newkey</remarks>
        public KeySetter Key
        {
            get { return new KeySetter(m_Dictionary); }
        }

        /// <summary>
        /// Dictionary �I�u�W�F�N�g�ɃL�[��ݒ肷�邽�߂̃N���X�ł��B
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public class KeySetter
        {
            NETDictionary m_Owner;
            internal KeySetter(NETDictionary owner)
            {
                m_Owner = owner;
            }
            /// <summary>
            /// Dictionary �I�u�W�F�N�g�ɃL�[��ݒ肵�܂��B
            /// </summary>
            /// <param name="Key">�ύX����L�[���w�肵�܂��B</param>
            /// <returns>���� Key �Ŏw�肵���l�ƒu��������V�����L�[���w�肵�܂��B</returns>
            public string this[string Key]
            {
                set
                {
                    object item = m_Owner[Key];
                    m_Owner.Add(value, item);
                    m_Owner.Remove(Key);
                }
            }
        }

        /// <summary>
        /// Dictionary �I�u�W�F�N�g�ɃL�[�Ƒ΂̍��ڂ�ǉ����܂��B
        /// </summary>
        /// <param name="Key">�ǉ�������� item �Ɗ֘A�t����ꂽ���� key ���w�肵�܂��B</param>
        /// <param name="Item">�ǉ�������� key �Ɗ֘A�t����ꂽ���� item ���w�肵�܂��B</param>
        public void Add(string Key, object Item)
        {
            m_Dictionary.Add(Key, Item);
        }

        /// <summary>
        /// �w�肳�ꂽ�L�[�� Dictionary �I�u�W�F�N�g�̒��ɑ��݂��邩�ǂ������擾���܂��B
        /// </summary>
        /// <param name="Key">Dictionary �I�u�W�F�N�g�̒����猟������L�[�̒l���w�肵�܂��B</param>
        /// <returns>�w�肳�ꂽ�L�[�� Dictionary �I�u�W�F�N�g�̒��ɑ��݂���ꍇ�́A�^ (True) ��Ԃ��܂��B���݂��Ȃ��ꍇ�́A�U (False) ��Ԃ��܂��B</returns>
        public bool Exists(string Key)
        {
            return m_Dictionary.ContainsKey(Key);
        }


        /// <summary>
        /// Dictionary �I�u�W�F�N�g�𔽕���������񋓎q��Ԃ��܂��B
        /// </summary>
        /// <returns>IEnumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return m_Dictionary.GetEnumerator();
        }

        /// <summary>
        /// Dictionary �I�u�W�F�N�g�̂��ׂĂ̍��ڂɊ܂܂��z���Ԃ��܂��B
        /// </summary>
        /// <returns>Dictionary �I�u�W�F�N�g�̂��ׂĂ̍��ڂɊ܂܂��z��B</returns>
        public object[] Items()
        {
            object[] valueArray = new object[this.Count];
            m_Dictionary.Values.CopyTo(valueArray, this.Count);
            return valueArray;
        }

        /// <summary>
        /// Dictionary �I�u�W�F�N�g�ɂ��邷�ׂẴL�[�Ɋ܂܂��z���Ԃ��܂��B
        /// </summary>
        /// <returns>Dictionary �I�u�W�F�N�g�ɂ��邷�ׂẴL�[�Ɋ܂܂��z��</returns>
        public object Keys()
        {
            string[] keyArray = new string[this.Count];
            m_Dictionary.Keys.CopyTo(keyArray, this.Count);
            return keyArray;
        }

        /// <summary>
        /// Dictionary �I�u�W�F�N�g����L�[�ƍ��ڂ̑g�݂��폜���܂��B
        /// </summary>
        /// <param name="Key">Dictionary �I�u�W�F�N�g����폜����L�[�ƍ��ڂ̑g�݂Ɗ֘A�t����ꂽ���� Key ���w�肵�܂��B</param>
        public void Remove(string Key)
        {
            m_Dictionary.Remove(Key);
        }

        /// <summary>
        /// Dictionary �I�u�W�F�N�g���炷�ׂẴL�[�ƍ��ڂ��폜���܂��B
        /// </summary>
        public void RemoveAll()
        {
            m_Dictionary.Clear();
        }
    }
}