using System;
using IO = System.IO;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Scripting
{
    /// <summary>
    /// TextStream �I�u�W�F�N�g�̊��N���X�ł��B
    /// </summary>
    public class TextStreamClass : MarshalByRefObject, TextStream
	{

        private object m_Stream;
        private bool m_UnDispose;
        private static Encoding m_ShiftJisEncoding = System.Text.Encoding.GetEncoding("shift_jis");
        private static Encoding m_UnicodeEncoding = System.Text.Encoding.Unicode;
        private int m_Line = 1;
        private int m_Column = 1;

        internal TextStreamClass(StandardStreamTypes StandardStreamType, bool Unicode)
        {
            m_UnDispose = true;
            switch (StandardStreamType)
            {
                case StandardStreamTypes.StdIn:
                    m_Stream = Console.In;
                    break;

                case StandardStreamTypes.StdOut:
                    m_Stream = Console.Out;
                    break;
                case StandardStreamTypes.StdErr:
                    m_Stream = Console.Error;
                    break;
                default:
                    throw new IO.IOException();
            }
        }

        internal TextStreamClass(FileNameClass fileName, IOMode IOMode, bool OverWrite, Tristate Format)
        {
            m_UnDispose = false;
            Encoding enc;
            switch (Format)
            {
                case Tristate.TristateFalse:
                    enc = m_ShiftJisEncoding;
                    break;
                case Tristate.TristateTrue:
                    enc = m_UnicodeEncoding;
                    break;
                case Tristate.TristateUseDefault:
                    enc = Encoding.Default;
                    break;
                default:
                    throw new IO.IOException();
            }


            SafeFileHandle handle = NativeWrapper.CreateFile(fileName, IOMode, OverWrite);

            switch (IOMode)
            {
                case Scripting.IOMode.ForReading:
                    m_Stream = new IO.StreamReader(new IO.FileStream(handle, IO.FileAccess.Read), enc);
                    break;

                case Scripting.IOMode.ForWriting:
                    m_Stream = new IO.StreamWriter(new IO.FileStream(handle, IO.FileAccess.Write), enc);
                    break;

                case Scripting.IOMode.ForAppending:
                    IO.FileStream stream = new IO.FileStream(handle, IO.FileAccess.ReadWrite);
                    stream.Position = stream.Length;
                    m_Stream = new IO.StreamWriter(stream, enc);
                    break;
            }
        }

        internal IO.StreamReader Reader
        {
            get { return (IO.StreamReader)m_Stream; }
        }

        internal IO.StreamWriter Writer
        {
            get { return (IO.StreamWriter)m_Stream; }
        }

        /// <summary>
        /// TextStream �t�@�C���̒��ŁA�t�@�C�� �|�C���^���t�@�C���̖��� (EOL) �̒��O�ɂ���ꍇ�́A�^ (True) ��Ԃ��܂��B����ȊO�̏ꍇ�́A�U (False) ��Ԃ��܂��B�l�̎擾�̂݉\�ł��B
        /// </summary>
        /// <value>�|�C���^���t�@�C���̖��� (EOL) �̒��O�ɂ���ꍇ�́A�^ (True) ��Ԃ��܂��B����ȊO�̏ꍇ�́A�U (False) </value>
        public bool AtEndOfLine
        {
            get { return Reader.EndOfStream; }
        }

        /// <summary>
        /// �t�@�C���̃|�C���^�� TextStream �t�@�C���̖����̈ʒu�ɂ���ꍇ�́A�^ (True) ��Ԃ��܂��B����ȊO�̏ꍇ�́A�U (False) ��Ԃ��܂��B�l�̎擾�̂݉\�ł��B
        /// </summary>
        /// <value>�|�C���^�� TextStream �t�@�C���̖����̈ʒu�ɂ���ꍇ�́A�^ (True) ��Ԃ��܂��B����ȊO�̏ꍇ�́A�U (False) </value>
        public bool AtEndOfStream
        {
            get { return Reader.EndOfStream; }
        }

        /// <summary>
        /// ���݂̍s�̃J�����ʒu (�s������̕�����) ��Ԃ��܂��B�l�̎擾�̂݉\�ł��B
        /// </summary>
        public int Column
        {
            get { return m_Column; }
        }

        /// <summary>
        /// TextStream �t�@�C���̌��݂̃t�@�C�� �|�C���^�̈ʒu���s�ԍ��ŕԂ��܂��B�l�̎擾�̂݉\�ł��B
        /// </summary>
        public int Line
        {
            get { return m_Line; }
        }

        /// <summary>
        /// �J���Ă���TextStream �t�@�C������܂��B
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// TextStream �t�@�C������w�肳�ꂽ��������ǂݍ��݁A���̌��ʂ̕������Ԃ��܂��B
        /// </summary>
        /// <param name="Characters">�t�@�C������ǂݍ��ޕ��������w�肵�܂��B</param>
        /// <returns>�t�@�C������ǂݍ��񂾕���</returns>
        public string Read(int Characters)
        {
            char[] buffer = new char[Characters];
            int num = Reader.Read(buffer, 0, Characters);
            if (num == 0)
                return string.Empty;
            else if (num == Characters)
                return buffer.ToString();
            else 
            {
                char[] newBuffer = new char[num];
                buffer.CopyTo(newBuffer, 0);
                return newBuffer.ToString();
            }
        }

        /// <summary>
        /// TextStream �t�@�C���S�̂�ǂݍ��݁A���̌��ʂ̕������Ԃ��܂��B
        /// </summary>
        /// <returns>TextStream �t�@�C���S�̂̕�����</returns>
        public string ReadAll()
        {
            return Reader.ReadToEnd();
        }

        /// <summary>
        /// TextStream �t�@�C������ 1 �s (���s����������) ��ǂݍ��݁A���̌��ʂ̕������Ԃ��܂��B
        /// </summary>
        /// <returns>�t�@�C������P�s���ǂݍ��񂾌���</returns>
        public string ReadLine()
        {
            return Reader.ReadLine();
        }

        /// <summary>
        /// TextStream �t�@�C����ǂݍ��ނƂ��Ɏw�肳�ꂽ���̕������X�L�b�v���܂��B
        /// </summary>
        /// <param name="Characters">�t�@�C����ǂݍ��ނƂ��ɃX�L�b�v�����镶�������w�肵�܂��B</param>
        public void Skip(int Characters)
        {
            Read(Characters);
        }

        /// <summary>
        /// TextStream �t�@�C����ǂݍ��ނƂ��Ɏ��̍s���X�L�b�v���܂��B
        /// </summary>
        public void SkipLine()
        {
            Reader.ReadLine(); 
        }

        /// <summary>
        /// TextStream �t�@�C���Ɏw�肳�ꂽ��������������݂܂��B
        /// </summary>
        /// <param name="Text">�t�@�C���ɏ������ޕ�������w�肵�܂��B</param>
        public void Write(string Text)
        {
            Writer.Write(Text);
        }

        /// <summary>
        /// TextStream �t�@�C���Ɏw�肳�ꂽ���̉��s�������������݂܂��B
        /// </summary>
        /// <param name="Lines">�t�@�C���ɏ������މ��s�����̐����w�肵�܂��B</param>
        public void WriteBlankLines(int Lines)
        {
            WriteLine(string.Empty);
        }

        /// <summary>
        /// TextStream �t�@�C���Ɏw�肳�ꂽ������Ɖ��s�������������݂܂��B
        /// </summary>
        /// <param name="Text">�t�@�C���ɏ������ޕ�������w�肵�܂��B�ȗ�����ƁA���s�������t�@�C���ɏ������܂�܂��B</param>
        public void WriteLine(string Text = "")
        {
            Writer.WriteLine(Text);
        }

        bool _disposedValue = false;
        
        /// <summary>
        /// �g�p���Ă���A���}�l�[�W ���\�[�X��������܂��B�I�v�V�����ŁA�}�l�[�W ���\�[�X��������܂��B
        /// </summary>
        /// <param name="disposing">�}�l�[�W ���\�[�X�ƃA���}�l�[�W ���\�[�X�̗������������ꍇ�� true�B�A���}�l�[�W ���\�[�X�������������ꍇ�� false�B </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!(this._disposedValue))
            {
                this._disposedValue = true;
                if (disposing && !m_UnDispose)
                {
                    IDisposable idisp = m_Stream as IDisposable;
                    if (idisp != null)
                        idisp.Dispose();
                    m_Stream = null;
                }
            }
        }

        /// <summary>
        /// �g�p����Ă��邷�ׂẴ��\�[�X��������܂��B
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// �K�x�[�W �R���N�V�����ɂ���ăN���A�����O�ɁA�A���}�l�[�W ���\�[�X��������A���̑��̃N���[���A�b�v��������s���܂��B 
        /// </summary>
        ~TextStreamClass()
        {
            Dispose(false);
        }
    }
}