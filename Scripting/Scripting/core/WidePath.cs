using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Scripting
{
    /// <summary>
    /// ファイル名を 32000 文字対応のファイル名に変換するクラスです。
    /// </summary>
    /// <remarks>
    /// Extended Length Path が正しい名称のようですが、冗長なため WidePath という呼称を使います。
    /// </remarks>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class WidePath
    {
        // UNC パスプレフィックス
        private const string PREFIX_UNC = @"\\";

        // Win32 パスプレフィックス
        private const string PREFIX_WIN32_PATH = @"\\?\";
        private const string PREFIX_WIN32_PATH_UNC = @"\\?\UNC\";

        // NtApi パスプレフィックス
        private const string PREFIX_NTAPI_PATH = @"\??\";
        private const string PREFIX_NTAPI_PATH_UNC = @"\??\UNC\";

        internal static readonly char DirectorySeparatorChar = System.IO.Path.DirectorySeparatorChar;        // '\'
        internal static readonly char AltDirectorySeparatorChar = System.IO.Path.AltDirectorySeparatorChar;  // '/'
        internal static readonly char VolumeSeparatorChar = System.IO.Path.VolumeSeparatorChar;              // ':'
        internal static readonly char[] WildChars = new char[] { '?', '*' };                                 // ワイルドカードキャラクタ
        internal static readonly char[] DirectorySeparatorChars = new char[] { DirectorySeparatorChar, AltDirectorySeparatorChar };

        private readonly string m_Display;

        private WidePath(string target)
        {
            m_Display = ToDisplay(target);
        }

        #region メンバ

        /// <summary>
        /// 表示用のファイル名に変換します。
        /// </summary>
        /// <param name="value">ファイル名</param>
        /// <returns>表示用のファイル名</returns>
        public static string ToDisplay(string value)
        {
            // "/" を "\" に置換

            value = value.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

            // 先頭が  "\\?\" なら 32,000 文字対応ファイル名なので削る
            // 先頭が  "\\?\" なら 32,000 文字対応ファイル名なので削る

            if (value.StartsWith(PREFIX_WIN32_PATH))
            {
                // 先頭が  "\\?\UNC\" か？
                if (value.StartsWith(PREFIX_WIN32_PATH_UNC))
                {
                    // 例:\\?\UNC\Server\Path → \\Server\Path
                    value = PREFIX_UNC + value.Substring(PREFIX_WIN32_PATH_UNC.Length);
                }
                else
                {
                    // 例:\\?\C:\Windows → C:\Windows
                    value = value.Substring(PREFIX_WIN32_PATH.Length);
                }
            }

            // 先頭が  "\??\" なら NtApi 形式のファイル名なので削る

            if (value.StartsWith(PREFIX_NTAPI_PATH))
            {
                // 先頭が  "\??\UNC\" か？
                if (value.StartsWith(PREFIX_NTAPI_PATH_UNC))
                {
                    // 例:\??\UNC\Server\Path → \\Server\Path
                    value = PREFIX_UNC + value.Substring(PREFIX_NTAPI_PATH_UNC.Length);
                }
                else
                {
                    // 例:\??\C:\Windows → C:\Windows
                    value = value.Substring(PREFIX_NTAPI_PATH.Length);
                }
            }
            
            // 相対パスにカレントディレクトリを付与
            return BuildPath(System.IO.Directory.GetCurrentDirectory(), value);
        }

        /// <summary>
        /// 指定したファイル名が UNC パスを含むかどうかを取得します。
        /// </summary>
        /// <param name="target">UNC パスを含むかどうかを取得するファイル名を指定します。</param>
        /// <returns>UNC パスを含む場合は True、含まない場合は False を返します。</returns>
        public static bool IsUNC(string target)
        {
            return target.IndexOf(PREFIX_UNC) == 0;
        }

        /// <summary>
        /// 指定した WidePath が UNC パスを含むかどうかを取得します。
        /// </summary>
        /// <param name="target">UNC パスを含むかどうかを取得する WidePath を指定します。</param>
        /// <returns>UNC パスを含む場合は True、含まない場合は False を返します。</returns>
        public static bool IsUNC(WidePath target)
        {
            return IsUNC(target.Display);
        }

        /// <summary>
        /// 表示用ファイル名を取得します。
        /// </summary>
        /// <returns>表示用ファイル名。</returns>
        public override string ToString()
        {
            return Display;
        }

        /// <summary>
        /// 表示名
        /// </summary>
        public string Display
        {
            get { return m_Display; }
        }

        /// <summary>
        /// API に引き渡す、32,000 文字対応ファイル名を取得します。
        /// </summary>
        public string Win32
        {
            get
            {
                if (IsUNC(Display))
                {
                    // UNC
                    return PREFIX_WIN32_PATH_UNC + Display.Substring(PREFIX_UNC.Length);
                }
                else
                {
                    // ローカルファイル
                    return PREFIX_WIN32_PATH + Display;
                }
            }
        }

        /// <summary>
        /// NtCreateFile 等の NT API に引き渡す 32,000 文字対応ファイル名を取得します。
        /// </summary>
        public string NtApi
        {
            get
            {
                if (Display.StartsWith(PREFIX_UNC))
                {
                    // UNC
                    return PREFIX_NTAPI_PATH_UNC + Display.Substring(PREFIX_UNC.Length);
                }
                else
                {
                    // ローカルファイル
                    return PREFIX_NTAPI_PATH + Display;
                }
            }
        }

        #endregion

        #region 8.3形式ファイル名と長いファイル名

        /// <summary>
        /// 8.3 形式のファイル名を返します。
        /// </summary>
        public string ShortPathName
        {
            get
            {
                using (StringBuilderCache sb = new StringBuilderCache())
                {
                    uint ret = NativeMethods.GetShortPathName(Win32, sb, (uint)sb.Capacity);
                    if (ret == 0)
                        return string.Empty;
                    return ToDisplay(sb.ToString());
                }
            }
        }

        /// <summary>
        /// 長いファイル名を返します。
        /// </summary>
        public string LongPathName
        {
            get
            {
                using (StringBuilderCache sb = new StringBuilderCache())
                {
                    uint ret = NativeMethods.GetLongPathName(Win32, sb, (uint)sb.Capacity);
                    if (ret == 0)
                        return string.Empty;
                    return ToDisplay(sb.ToString());
                }
            }
        }

        #endregion

        #region パスとファイル名に分解(表示名)

        /// <summary>
        /// ルートパス名
        /// </summary>
        public string RootPathName
        {
            get
            {
                int nLen = GetRootLength(Display);
                if (nLen == 2)   // C: 
                    return Display.Substring(0, nLen) + DirectorySeparatorChar;
                else
                    // UNC or C:\
                    return Display.Substring(0, nLen);
            }
        }

        /// <summary>
        /// ドライブ名
        /// </summary>
        public string DriveName
        {
            get
            {
                int nLen = GetRootLength(Display);
                if (nLen == 2 || nLen == 3) // C: or C:\
                    // C:
                    return Display.Substring(0, 2);
                else
                    // UNC
                    return Display.Substring(0, nLen);
            }
        }

        /// <summary>
        /// Volume 情報を取得するための名前
        /// </summary>
        public string VolumeInfoName
        {
            get
            {
                return this.DriveName + DirectorySeparatorChar;
            }
        }

        #endregion

        #region パスとファイル名に分解

        /// <summary>
        /// 指定したファイル名のディレクトリ名を除いたファイル名を取得
        /// </summary>
        /// <param name="target">ディレクトリ名を含んだファイル名</param>
        /// <returns>ディレクトリ名を除いたファイル名</returns>
        public static string GetFileName(string target)
        {
            if (IsRootFolder(target)) return string.Empty;

            int nPos = target.LastIndexOf(DirectorySeparatorChar);
            if (nPos == -1)
                return target;
            else
                return target.Substring(nPos + 1);
        }

        /// <summary>
        /// 指定したファイル名の親のフォルダ名を取得
        /// </summary>
        /// <param name="target">ディレクトリ名を含んだファイル名</param>
        /// <returns>親のフォルダ名</returns>
        public static string GetParentFolderName(string target)
        {
            if (IsRootFolder(target)) return string.Empty;

            int nPos = target.LastIndexOf(DirectorySeparatorChar);
            if (nPos == -1)
                return string.Empty;
            else
                return target.Substring(0, nPos);
        }

        /// <summary>
        /// 拡張子を除いたファイル名を取得
        /// </summary>
        /// <param name="target">拡張子を含んだファイル名</param>
        /// <returns>拡張子を除いたファイル名</returns>
        public static string GetFileNameWithoutExtension(string target)
        {
            string fileName = GetFileName(target);
            int nPos = fileName.LastIndexOf('.');
            if (nPos == -1)
                return fileName;
            else
                return fileName.Substring(0, nPos);
        }

        /// <summary>
        /// 拡張子を取得
        /// </summary>
        /// <param name="target">拡張子を含んだファイル名</param>
        /// <returns>拡張子</returns>
        public static string GetExtensionName(string target)
        {
            string fileName = GetFileName(target);
            int nPos = fileName.LastIndexOf('.');
            if (nPos == -1)
                return string.Empty;
            else
                return fileName.Substring(nPos + 1);
        }

        #endregion

        #region 暗黙の型変換

        /// <summary>
        /// 指定したファイル名から WidePath を作成します。
        /// </summary>
        /// <param name="value">ファイル名。</param>
        /// <returns>WidePath クラスのインスタンス。</returns>
        public static implicit operator WidePath(string value)
        {
            return new WidePath(value);
        }

        #endregion

        #region 二項演算子

        /// <summary>
        /// == 演算子のオーバーロード
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(WidePath a, WidePath b)
        {
            return IsEqualInternal(a, b);
        }

        /// <summary>
        /// &lt;&gt; 演算子のオーバーロード
        /// </summary>
        public static bool operator !=(WidePath a, WidePath b)
        {
            return !IsEqualInternal(a, b);
        }

        internal static bool IsEqualInternal(WidePath a, WidePath b)
        {
            if (object.ReferenceEquals((object)a, (object)b))
                return true;

            if ((object)a == null || (object)b == null) return false;
            return string.Compare(a.Display, b.Display, true) == 0;
        }

        /// <summary>
        /// この文字列のハッシュ コードを返します。
        /// </summary>
        /// <returns>ハッシュ値</returns>
        public override int GetHashCode()
        {
            return Display.GetHashCode();
        }

        /// <summary>
        /// 指定のオブジェクトが現在のオブジェクトと等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">現在のオブジェクトと比較するオブジェクト。 </param>
        /// <returns>指定したオブジェクトが現在のオブジェクトと等しい場合は true。それ以外の場合は false。 </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return string.IsNullOrEmpty(Display);
            return string.Compare(Display, obj.ToString(), true) == 0;
        }

        #endregion

        internal static string BuildPath(string basePath, string relativePath)
        {
            // '/' を除去
            basePath = basePath.Replace(AltDirectorySeparatorChar, DirectorySeparatorChar);
            relativePath = relativePath.Replace(AltDirectorySeparatorChar, DirectorySeparatorChar);

            // relativePath に絶対パスが指定されていればそのまま
            // 相対パスが指定されていたら、basePath と結合

            string combinePath;
            if (IsPathRooted(relativePath))
            {
                combinePath = relativePath;
            }
            else
            {
                // ベースパスが指定されていなければ、カレントディレクトリを仮定する
                if (string.IsNullOrEmpty(basePath))
                    basePath = System.IO.Directory.GetCurrentDirectory();

                // 相対パスが指定されていなければ空文字
                if (string.IsNullOrEmpty(relativePath))
                    relativePath = string.Empty;

                combinePath = basePath.TrimEnd(DirectorySeparatorChar)
                            + DirectorySeparatorChar
                            + relativePath.TrimStart(DirectorySeparatorChar);
            }

            // ..\, .\ を削る

            List<string> elements = new List<string>();

            foreach (string v in combinePath.Split(DirectorySeparatorChar))
            {
                if (v == "..")
                    elements.RemoveAt(elements.Count - 1);
                else if (v != ".")
                    elements.Add(v);
            }
            return string.Join(DirectorySeparatorChar.ToString(), elements.ToArray());
        }

        /// <summary>
        /// ルートパスが含まれているかチェック
        /// </summary>
        internal static bool IsPathRooted(string target)
        {
            if (target == null) return false;

            // C:\ 等
            if (target.Length >= 2)
                if (target[1] == VolumeSeparatorChar) return true;

            // \\PC名
            if (target.Length >= 3)
                if (target[0] == DirectorySeparatorChar && target[1] == DirectorySeparatorChar) return true;

            return false;
        }

        /// <summary>
        /// ルートパスの長さ取得
        /// </summary>
        internal static int GetRootLength(string path)
        {
            int i = 0;
            int length = path.Length;
            if (length >= 1 && (IsDirectorySeparator(path[0])))
            {
                // UNC
                i = 1;
                if (length >= 2 && (IsDirectorySeparator(path[1])))
                {
                    i = 2;
                    int n = 2;
                    while (i < length && ((path[i] != DirectorySeparatorChar && path[i] != AltDirectorySeparatorChar) || --n > 0)) i++;
                }
            }
            else if (length >= 2 && path[1] == VolumeSeparatorChar)
            {
                // 通常のファイル名(C:\Hoge)
                i = 2;
                if (length >= 3 && (IsDirectorySeparator(path[2]))) i++;
            }
            return i;
        }

        internal static bool IsDirectorySeparator(char c)
        {
            return (c == DirectorySeparatorChar || c == AltDirectorySeparatorChar);
        }

        /// <summary>
        /// 指定したファイル名がルートフォルダかどうかを取得します。
        /// </summary>
        /// <param name="path">ファイル名。</param>
        /// <returns>指定したファイル名がルートフォルダなら True、そうでない場合は False を返します。</returns>
        public static bool IsRootFolder(string path)
        {
            return GetRootLength(TrimDirectorySeparator(path)) == path.Length;
        }

        internal static bool LastCharIsDirectorySeparatorChar(string value) {
            return (!string.IsNullOrEmpty(value) && IsDirectorySeparator(value[value.Length - 1]));
        }

        internal static bool ContainsWildChars(string value) {
            return value.IndexOfAny(WildChars) != -1;
        }

        internal static string TrimDirectorySeparator(string value)
        {
            return value.TrimEnd(DirectorySeparatorChars);
        }
    }

}
