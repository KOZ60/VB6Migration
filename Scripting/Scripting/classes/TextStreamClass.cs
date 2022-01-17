using System;
using IO = System.IO;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Scripting
{
    /// <summary>
    /// TextStream オブジェクトの基底クラスです。
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
        /// TextStream ファイルの中で、ファイル ポインタがファイルの末尾 (EOL) の直前にある場合は、真 (True) を返します。それ以外の場合は、偽 (False) を返します。値の取得のみ可能です。
        /// </summary>
        /// <value>ポインタがファイルの末尾 (EOL) の直前にある場合は、真 (True) を返します。それ以外の場合は、偽 (False) </value>
        public bool AtEndOfLine
        {
            get { return Reader.EndOfStream; }
        }

        /// <summary>
        /// ファイルのポインタが TextStream ファイルの末尾の位置にある場合は、真 (True) を返します。それ以外の場合は、偽 (False) を返します。値の取得のみ可能です。
        /// </summary>
        /// <value>ポインタが TextStream ファイルの末尾の位置にある場合は、真 (True) を返します。それ以外の場合は、偽 (False) </value>
        public bool AtEndOfStream
        {
            get { return Reader.EndOfStream; }
        }

        /// <summary>
        /// 現在の行のカラム位置 (行頭からの文字数) を返します。値の取得のみ可能です。
        /// </summary>
        public int Column
        {
            get { return m_Column; }
        }

        /// <summary>
        /// TextStream ファイルの現在のファイル ポインタの位置を行番号で返します。値の取得のみ可能です。
        /// </summary>
        public int Line
        {
            get { return m_Line; }
        }

        /// <summary>
        /// 開いているTextStream ファイルを閉じます。
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// TextStream ファイルから指定された文字数を読み込み、その結果の文字列を返します。
        /// </summary>
        /// <param name="Characters">ファイルから読み込む文字数を指定します。</param>
        /// <returns>ファイルから読み込んだ文字</returns>
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
        /// TextStream ファイル全体を読み込み、その結果の文字列を返します。
        /// </summary>
        /// <returns>TextStream ファイル全体の文字列</returns>
        public string ReadAll()
        {
            return Reader.ReadToEnd();
        }

        /// <summary>
        /// TextStream ファイルから 1 行 (改行文字を除く) を読み込み、その結果の文字列を返します。
        /// </summary>
        /// <returns>ファイルから１行う読み込んだ結果</returns>
        public string ReadLine()
        {
            return Reader.ReadLine();
        }

        /// <summary>
        /// TextStream ファイルを読み込むときに指定された数の文字をスキップします。
        /// </summary>
        /// <param name="Characters">ファイルを読み込むときにスキップさせる文字数を指定します。</param>
        public void Skip(int Characters)
        {
            Read(Characters);
        }

        /// <summary>
        /// TextStream ファイルを読み込むときに次の行をスキップします。
        /// </summary>
        public void SkipLine()
        {
            Reader.ReadLine(); 
        }

        /// <summary>
        /// TextStream ファイルに指定された文字列を書き込みます。
        /// </summary>
        /// <param name="Text">ファイルに書き込む文字列を指定します。</param>
        public void Write(string Text)
        {
            Writer.Write(Text);
        }

        /// <summary>
        /// TextStream ファイルに指定された数の改行文字を書き込みます。
        /// </summary>
        /// <param name="Lines">ファイルに書き込む改行文字の数を指定します。</param>
        public void WriteBlankLines(int Lines)
        {
            WriteLine(string.Empty);
        }

        /// <summary>
        /// TextStream ファイルに指定された文字列と改行文字を書き込みます。
        /// </summary>
        /// <param name="Text">ファイルに書き込む文字列を指定します。省略すると、改行文字がファイルに書き込まれます。</param>
        public void WriteLine(string Text = "")
        {
            Writer.WriteLine(Text);
        }

        bool _disposedValue = false;
        
        /// <summary>
        /// 使用しているアンマネージ リソースを解放します。オプションで、マネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースとアンマネージ リソースの両方を解放する場合は true。アンマネージ リソースだけを解放する場合は false。 </param>
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
        /// 使用されているすべてのリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// ガベージ コレクションによってクリアされる前に、アンマネージ リソースを解放し、その他のクリーンアップ操作を実行します。 
        /// </summary>
        ~TextStreamClass()
        {
            Dispose(false);
        }
    }
}