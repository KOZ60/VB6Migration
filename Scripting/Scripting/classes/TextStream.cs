using System;
namespace Scripting
{
    /// <summary>
    /// ファイルへのシーケンシャル アクセスを行うオブジェクトです。
    /// </summary>
    public interface TextStream : IDisposable
    {
        /// <summary>
        /// TextStream ファイルの中で、ファイル ポインタがファイルの末尾 (EOL) の直前にある場合は、真 (True) を返します。それ以外の場合は、偽 (False) を返します。値の取得のみ可能です。
        /// </summary>
        /// <value>ポインタがファイルの末尾 (EOL) の直前にある場合は、真 (True) を返します。それ以外の場合は、偽 (False) </value>
        bool AtEndOfLine { get; }

        /// <summary>
        /// ファイルのポインタが TextStream ファイルの末尾の位置にある場合は、真 (True) を返します。それ以外の場合は、偽 (False) を返します。値の取得のみ可能です。
        /// </summary>
        /// <value>ポインタが TextStream ファイルの末尾の位置にある場合は、真 (True) を返します。それ以外の場合は、偽 (False) </value>
        bool AtEndOfStream { get; }
        
        /// <summary>
        /// オブジェクトを閉じます。
        /// </summary>
        void Close();

        /// <summary>
        /// 現在の行のカラム位置 (行頭からの文字数) を返します。値の取得のみ可能です。
        /// </summary>
        int Column { get; }

        /// <summary>
        /// TextStream ファイルの現在のファイル ポインタの位置を行番号で返します。値の取得のみ可能です。
        /// </summary>
        int Line { get; }

        /// <summary>
        /// TextStream ファイルから指定された文字数を読み込み、その結果の文字列を返します。
        /// </summary>
        /// <param name="Characters">ファイルから読み込む文字数を指定します。</param>
        /// <returns>ファイルから読み込んだ文字</returns>
        string Read(int Characters);

        /// <summary>
        /// TextStream ファイル全体を読み込み、その結果の文字列を返します。
        /// </summary>
        /// <returns>TextStream ファイル全体の文字列</returns>
        string ReadAll();

        /// <summary>
        /// TextStream ファイルから 1 行 (改行文字を除く) を読み込み、その結果の文字列を返します。
        /// </summary>
        /// <returns>ファイルから１行う読み込んだ結果</returns>
        string ReadLine();


        /// <summary>
        /// TextStream ファイルを読み込むときに指定された数の文字をスキップします。
        /// </summary>
        /// <param name="Characters">ファイルを読み込むときにスキップさせる文字数を指定します。</param>
        void Skip(int Characters);

        /// <summary>
        /// TextStream ファイルを読み込むときに次の行をスキップします。
        /// </summary>
        void SkipLine();

        /// <summary>
        /// TextStream ファイルに指定された文字列を書き込みます。
        /// </summary>
        /// <param name="Text">ファイルに書き込む文字列を指定します。</param>
        void Write(string Text);

        /// <summary>
        /// TextStream ファイルに指定された数の改行文字を書き込みます。
        /// </summary>
        /// <param name="Lines">ファイルに書き込む改行文字の数を指定します。</param>
        void WriteBlankLines(int Lines);

        /// <summary>
        /// TextStream ファイルに指定された文字列と改行文字を書き込みます。
        /// </summary>
        /// <param name="Text">ファイルに書き込む文字列を指定します。省略すると、改行文字がファイルに書き込まれます。</param>
        void WriteLine(string Text = "");
    }
}
