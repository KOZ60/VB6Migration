using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System.Runtime.CompilerServices;

namespace VBCompatible
{
    /// <summary>
    /// VB6.0 Strings 互換クラス
    /// </summary>
    [StandardModule]
    public static class VBStrings
    {
        /// <summary>
        /// 式を指定した書式に変換し、その文字列の値を返します。
        /// </summary>
        /// <param name="Expression">
        /// 必ず指定します。任意の式を指定します。引数 expression に指定したデータは、引数 format の書式に従って変換されます。
        /// </param>
        /// <param name="Style"></param>
        /// <param name="DayOfWeek"></param>
        /// <param name="WeekOfYear"></param>
        /// <returns></returns>
        public static string VBFormat(object Expression,
                                string Style = "",
                                FirstDayOfWeek DayOfWeek = FirstDayOfWeek.Sunday,
                                FirstWeekOfYear WeekOfYear = FirstWeekOfYear.Jan1) {
            if (Expression is long) {
                Expression = new decimal(Convert.ToInt64(RuntimeHelpers.GetObjectValue(Expression)));
            } else if (Expression is char) {
                Expression = Expression.ToString();
            }
            int nResult;
            string str;
            IntPtr ptr = Marshal.AllocCoTaskMem(24);
            try {
                NativeMethods.VariantInit(ptr);
                try {
                    Marshal.GetNativeVariantForObject(Expression, ptr);
                    int dwFlags = (Thread.CurrentThread.CurrentCulture.Calendar is HijriCalendar)
                                    ? NativeMethods.VAR_CALENDAR_HIJRI : NativeMethods.VAR_FORMAT_NOSUBSTITUTE;
                    nResult = NativeMethods.VarFormat(ptr, ref Style, (int)DayOfWeek, (int)WeekOfYear, dwFlags, out str);
                } finally {
                    NativeMethods.VariantClear(ptr);
                }
            } finally {
                Marshal.FreeCoTaskMem(ptr);
            }
            if (nResult < 0) {
                throw new ArgumentException();
            }
            return str;
        }


        /// <summary>
        /// 指定した文字列の文字数または指定した変数を格納するのに必要なバイト数を返します。
        /// </summary>
        /// <param name="value">任意の文字列式を指定します。</param>
        /// <returns>
        /// 指定した文字列の文字数または指定した変数を格納するのに必要なバイト数を返します。
        /// </returns>
        public static int LenB(VBString value) {
            return value.LengthB;
        }

        /// <summary>
        /// 指定した構造体のアンマネージサイズをバイト数を返します。
        /// </summary>
        /// <typeparam name="T">構造体の型</typeparam>
        /// <param name="value">任意の構造体を指定します。</param>
        /// <returns>指定した構造体のサイズ。 </returns>
        public static int LenB<T>(T value)
            where T : struct {
            return Marshal.SizeOf(value);
        }

        /// <summary>
        /// 文字列の左端から指定したバイト数分の文字列を返します。
        /// </summary>
        /// <param name="value">任意の文字列式を指定します。</param>
        /// <param name="length">取り出す文字のバイト数を指定します。</param>
        /// <returns>
        /// 文字列の左端から指定したバイト数分の文字列を返します。
        /// </returns>
        public static VBString LeftB(VBString value, int length) {
            if (length > value.LengthB) length = value.LengthB;

            if (length <= 0) return string.Empty;

            byte[] b = new byte[length];

            for (int i = 0; i < length; i++) {
                b[i] = value.GetByte(i);
            }
            return b;
        }

        /// <summary>
        /// 文字列から指定したバイト数分の文字列を返します。
        /// </summary>
        /// <param name="value">任意の文字列式を指定します。</param>
        /// <param name="start">文字列式の先頭を１として、どの位置から文字列を取り出すかをバイト数単位で指定します。</param>
        /// <returns>
        /// 文字列から指定したバイト数分の文字列を返します。
        /// </returns>
        public static VBString MidB(VBString value, int start) {
            int length = value.LengthB - start + 1;
            return MidB(value, start, length);
        }

        /// <summary>
        /// 文字列から指定したバイト数分の文字列を返します。
        /// </summary>
        /// <param name="value">任意の文字列式を指定します。</param>
        /// <param name="start">文字列式の先頭を１として、どの位置から文字列を取り出すかをバイト数単位で指定します。</param>
        /// <param name="length">取り出す文字のバイト数を指定します。</param>
        /// <returns>
        /// 文字列から指定したバイト数分の文字列を返します。
        /// </returns>
        public static VBString MidB(VBString value, int start, int length) {
            int pos = start - 1;

            if (pos + length > value.LengthB)
                length = value.LengthB - start + 1;

            if (length <= 0) return string.Empty;

            byte[] b = new byte[length];

            for (int i = 0; i < length; i++) {
                b[i] = value.GetByte(i + pos);
            }
            return b;
        }

        /// <summary>
        /// 文字列の右端から指定したバイト数分の文字列を返します。
        /// </summary>
        /// <param name="value">任意の文字列式を指定します。</param>
        /// <param name="length">取り出す文字のバイト数を指定します。</param>
        /// <returns>
        /// 文字列の右端から指定したバイト数分の文字列を返します。
        /// </returns>
        public static VBString RightB(VBString value, int length) {
            if (length > value.LengthB) length = value.LengthB;

            int pos = value.LengthB - length;

            if (length <= 0) return string.Empty;

            byte[] b = new byte[length];

            for (int i = 0; i < length; i++) {
                b[i] = value.GetByte(i + pos);
            }
            return b;
        }

        /// <summary>
        /// ある文字列 (StringCheck) の中から指定した文字列 (StringMatch) を検索し、最初に見つかったバイト位置 (先頭からその位置までのバイト数) を返す文字列処理関数です。
        /// </summary>
        /// <param name="StringCheck">検索対象となる文字列式を指定します。</param>
        /// <param name="StringMatch">引数 string1 内で検索する文字列式を指定します。</param>
        /// <param name="Compare">文字列比較の比較モードを指定する番号を設定しますが、値は無視され、常にバイナリモードで比較します。</param>
        /// <returns></returns>
        public static int InStrB(VBString StringCheck, VBString StringMatch, CompareMethod Compare = CompareMethod.Binary) {
            return InStrB(1, StringCheck, StringMatch, Compare);
        }

        /// <summary>
        /// ある文字列 (StringCheck) の中から指定した文字列 (StringMatch) を検索し、最初に見つかったバイト位置 (先頭からその位置までのバイト数) を返す文字列処理関数です。
        /// </summary>
        /// <param name="Start">検索の開始位置を表す数式を指定します。</param>
        /// <param name="StringCheck">検索対象となる文字列式を指定します。</param>
        /// <param name="StringMatch">引数 string1 内で検索する文字列式を指定します。</param>
        /// <param name="Compare">文字列比較の比較モードを指定する番号を設定しますが、値は無視され、常にバイナリモードで比較します。</param>
        /// <returns></returns>
        public static int InStrB(int Start, VBString StringCheck, VBString StringMatch, CompareMethod Compare = CompareMethod.Binary) {
            // InStrB では、Compare に何を指定してもバイナリ比較になる

            byte[] b1 = StringCheck;
            byte[] b2 = StringMatch;

            if (Start < 0) Start = 0;
            int nEnd = b1.Length - b2.Length + 1;

            for (int pos = Start - 1; pos < nEnd; pos++) {
                bool sw = true;
                for (int i = 0; i < b2.Length; i++) {
                    if (b1[pos + i] != b2[i]) {
                        sw = false;
                        break;
                    }
                }
                if (sw) return pos + 1;
            }

            return 0;
        }
        /// <summary>
        /// 文字列の右側から開始して、1 つの文字列が別の文字列内に最初に出現する位置を返します。
        /// サロゲートペアは１文字としてカウントされます。
        /// </summary>
        /// <param name="StringCheck">検索対象となる文字列式を指定します。</param>
        /// <param name="StringMatch">引数 StringCheck 内で検索する文字列式を指定します。</param>
        /// <param name="Start">検索の開始位置を表す数式を指定します。</param>
        /// <param name="Compare">文字列比較の比較モードを指定する番号を設定しますが、値は無視され、常にバイナリモードで比較します。</param>
        /// <returns></returns>
        public static int InStrRevB(VBString StringCheck, VBString StringMatch, int Start = 0, CompareMethod Compare = CompareMethod.Binary) {
            // InStrRevB では、Compare に何を指定してもバイナリ比較になる

            byte[] b1 = StringCheck;
            byte[] b2 = StringMatch;

            Start--;
            if (Start < 0 | Start + b2.Length > b1.Length) Start = b1.Length - b2.Length;

            for (int pos = Start; pos >= 0; pos--) {
                bool sw = true;
                for (int i = 0; i < b2.Length; i++) {
                    if (b1[pos + i] != b2[i]) {
                        sw = false;
                        break;
                    }
                }
                if (sw) return pos + 1;
            }

            return 0;
        }

        /// <summary>
        /// 文字が半角か全角かを判定する拡張メソッド
        /// </summary>
        /// <param name="charValue">チェックするキャラクタ</param>
        /// <returns>半角文字なら True, 全角文字なら False</returns>
        public static bool IsHalfChar(this char charValue) {
            return OneByteChars.Contains(charValue);
        }

        /// <summary>
        /// 文字が ShiftJis に変換可能かを判定する拡張メソッド
        /// </summary>
        /// <param name="value">チェックするキャラクタ</param>
        /// <returns>可能なら True, 不可能なら False</returns>
        public static bool IsShiftJis(this char value) {
            byte[] buffer = new byte[4];
            char defaultChar = char.MinValue;
            bool useDefaultChar;
            int result = NativeMethods.WideCharToMultiByte(
                                NativeMethods.CP_932,
                                NativeMethods.WC_NO_BEST_FIT_CHARS,
                                ref value,
                                1,
                                buffer,
                                buffer.Length,
                                ref defaultChar,
                                out useDefaultChar
                                );

            return !useDefaultChar;
        }

        /// <summary>
        /// VB の Chr関数と同等機能
        /// </summary>
        /// <param name="CharCode">対象のキャラクタコード</param>
        /// <returns>変換後キャラクタ</returns>
        public static char VBChr(int CharCode) {
            char chr;
            int chars;
            if (CharCode >= 0 && CharCode <= 127) {
                return Convert.ToChar(CharCode);
            }
            VBEncoding encoding = VBEncoding.GetEncoding(Thread.CurrentThread.CurrentCulture.TextInfo.ANSICodePage);
            char[] chrArray = new char[2];
            byte[] charCode = new byte[2];
            Decoder decoder = encoding.GetDecoder();
            if (CharCode < 0 || CharCode > 255) {
                charCode[0] = checked((byte)((CharCode & 65280) >> 8));
                charCode[1] = checked((byte)(CharCode & 255));
                chars = decoder.GetChars(charCode, 0, 2, chrArray, 0);
            } else {
                charCode[0] = checked((byte)(CharCode & 255));
                chars = decoder.GetChars(charCode, 0, 1, chrArray, 0);
            }
            chr = chrArray[0];
            return chr;
        }

        // 半角とみなすキャラクタの HashSet
        internal static HashSet<char> OneByteChars = InitialOnByteChars();

        // 半角とみなすキャラクタの HashSet を作成
        private static HashSet<char> InitialOnByteChars() {
            HashSet<char> hash = new HashSet<char>();
            for (int i = 0; i < 256; i++) {
                hash.Add(VBChr(i));
            }
            return hash;
        }

        /// <summary>
        /// 画面や帳票で文字が占める桁数を取得します。
        /// </summary>
        /// <param name="charValue">対象の文字。</param>
        /// <returns>
        /// 上位サロゲートペアのとき２、下位サロゲートペアのときゼロ
        /// 半角のときは１、全角のときは２
        /// </returns>
        public static int GetPrintWidth(this char charValue) {
            if (char.IsHighSurrogate(charValue))
                return 2;

            if (char.IsLowSurrogate(charValue))
                return 0;

            if (IsHalfChar(charValue))
                return 1;
            else
                return 2;
        }

        /// <summary>
        /// 画面や帳票で文字列が占める桁数を取得します。
        /// </summary>
        /// <param name="value">対象の文字列。</param>
        /// <returns>画面や帳票で文字列が占める桁数。</returns>
        public static int GetPrintWidth(this string value) {
            int length = 0;
            if (string.IsNullOrEmpty(value)) {
                foreach (char c in value) {
                    length += GetPrintWidth(c);
                }
            }
            return length;
        }

        #region サロゲートペアを意識した文字列操作関数

        /// <summary>
        /// 文字列の文字数を返します。
        /// サロゲートペアは１文字としてカウントされます。
        /// </summary>
        /// <param name="str">任意の有効な String 型の式または変数名。</param>
        /// <returns>
        /// 文字列内の文字数を返します。
        /// サロゲートペアは１文字としてカウントされます。
        /// </returns>
        public static int LenU(string str) {
            if (string.IsNullOrEmpty(str)) return 0;

            int lengthC = str.Length;
            int lengthU = 0;
            int i = 0;
            while (i < lengthC) {
                char c = str[i];
                if (i + 1 < lengthC && char.IsSurrogatePair(c, str[i + 1])) {
                    lengthU++;
                    i += 2;
                } else {
                    lengthU++;
                    i += 1;
                }
            }
            return lengthU;
        }

        /// <summary>
        /// 文字列の左側から指定された文字数を含む文字列を返します。
        /// サロゲートペアは１文字としてカウントされます。
        /// </summary>
        /// <param name="str">左端の文字が返される String 型の式。 </param>
        /// <param name="length">
        /// 返す文字の数を示す値。 
        /// 0 を指定すると、長さ 0 の文字列 ("") が返されます。
        /// str 内の文字数以上を指定すると、文字列全体が返されます。 </param>
        /// <returns>
        /// 指定された文字数を含む文字列を返します。
        /// サロゲートペアは１文字としてカウントされます。
        /// </returns>
        public static string LeftU(string str, int length) {
            return MidU(str, 1, length);
        }

        /// <summary>
        /// 文字列から指定された文字数分の文字列を返します。
        /// サロゲートペアは１文字としてカウントされます。
        /// </summary>
        /// <param name="str">文字が返される String 型の式。 </param>
        /// <param name="start">
        /// 返す文字の開始位置。
        /// Start の値が str 内の文字数よりも大きい場合、Mid 関数は長さ 0 の文字列 ("") を返します。
        /// Start は 1 から始まります。 
        /// </param>
        /// <returns>
        /// 文字列から指定された文字数分の文字列を返します。
        /// サロゲートペアは１文字としてカウントされます。
        /// </returns>
        public static string MidU(string str, int start) {
            int length = LenU(str) - start + 1;
            return MidU(str, start, length);
        }

        /// <summary>
        /// 文字列から指定された文字数分の文字列を返します。
        /// サロゲートペアは１文字としてカウントされます。
        /// </summary>
        /// <param name="str">文字が返される String 型の式。 </param>
        /// <param name="start">
        /// 返す文字の開始位置。 
        /// Start の値が str 内の文字数よりも大きい場合、Mid 関数は長さ 0 の文字列 ("") を返します。
        /// Start は 1 から始まります。 
        /// </param>
        /// <param name="length">
        /// 返される文字数。 省略した場合、またはテキスト内の Length の文字数 (位置 Start の文字を含む) よりも少なかった場合は、開始位置から文字列の末尾までのすべての文字が返されます。 
        /// </param>
        /// <returns>
        /// 文字列から指定された文字数分の文字列を返します。
        /// サロゲートペアは１文字としてカウントされます。
        /// </returns>
        public static string MidU(string str, int start, int length) {
            if (string.IsNullOrEmpty(str)) return string.Empty;

            // 開始位置を検索

            int pos = GetStartPositionU(str, start);

            // 開始位置以降の文字を取得

            StringBuilder builder = new StringBuilder(str.Length);
            int lengthU = 0;
            int lengthC = str.Length;

            while (pos < lengthC && lengthU < length) {
                char c = str[pos];
                if (pos + 1 < lengthC && char.IsSurrogatePair(c, str[pos + 1])) {
                    builder.Append(c);
                    builder.Append(str[pos + 1]);
                    lengthU++;
                    pos += 2;
                } else {
                    builder.Append(c);
                    lengthU++;
                    pos += 1;
                }
            }
            return builder.ToString();
        }

        private static int GetStartPositionU(string str, int start) {
            int lengthC = str.Length;
            int skipLength = 0;
            int pos = 0;
            start = start - 1;
            while (pos < lengthC && skipLength < start) {
                char c = str[pos];
                if (pos + 1 < lengthC && char.IsSurrogatePair(c, str[pos + 1])) {
                    skipLength++;
                    pos += 2;
                } else {
                    skipLength++;
                    pos += 1;
                }
            }
            return pos;
        }

        /// <summary>
        /// 文字列の右端から指定された文字数分の文字列を返します。
        /// サロゲートペアは１文字としてカウントされます。
        /// </summary>
        /// <param name="str">文字が返される String 型の式。 </param>
        /// <param name="length">
        /// 返す文字の数を示す数式。 
        /// 0 を指定すると、長さ 0 の文字列 ("") が返されます。
        /// str 内の文字数以上を指定すると、文字列全体が返されます。 
        /// </param>
        /// <returns>
        /// 文字列の右端から指定された文字数分の文字列を返します。 
        /// サロゲートペアは１文字としてカウントされます。
        /// </returns>
        public static string RightU(string str, int length) {
            if (length <= 0) return string.Empty;
            int lengthU = LenU(str);
            if (length > lengthU) length = lengthU;

            int pos = lengthU - length + 1;
            return MidU(str, pos, length);
        }

        /// <summary>
        /// ある文字列 (StringCheck) の中から指定した文字列 (StringMatch) を検索し、最初に見つかった位置 (先頭からその位置までの文字数) を返す文字列処理関数です。
        /// サロゲートペアは１文字としてカウントされます。
        /// </summary>
        /// <param name="StringCheck">検索対象となる文字列式を指定します。</param>
        /// <param name="StringMatch">引数 StringCheck 内で検索する文字列式を指定します。</param>
        /// <param name="Compare">文字列比較の比較モードを指定する番号を設定します</param>
        /// <returns>最初に見つかった位置</returns>
        public static int InStrU(string StringCheck, string StringMatch, CompareMethod Compare = CompareMethod.Binary) {
            return InStrU(1, StringCheck, StringMatch, Compare);
        }

        /// <summary>
        /// ある文字列 (StringCheck) の中から指定した文字列 (StringMatch) を検索し、最初に見つかった位置 (先頭からその位置までの文字数) を返す文字列処理関数です。
        /// サロゲートペアは１文字としてカウントされます。
        /// </summary>
        /// <param name="Start">検索の開始位置を表す数式を指定します。</param>
        /// <param name="StringCheck">検索対象となる文字列式を指定します。</param>
        /// <param name="StringMatch">引数 StringCheck 内で検索する文字列式を指定します。</param>
        /// <param name="Compare">文字列比較の比較モードを指定する番号を設定します。</param>
        /// <returns>最初に見つかった位置</returns>
        public static int InStrU(int Start, string StringCheck, string StringMatch, CompareMethod Compare = CompareMethod.Binary) {
            int nPos = Strings.InStr(GetStartPositionU(StringCheck, Start) + 1, StringCheck, StringMatch, Compare);
            if (nPos == 0) return 0;
            return LenU(Strings.Mid(StringCheck, 1, nPos));
        }

        /// <summary>
        /// 文字列の右側から開始して、1 つの文字列が別の文字列内に最初に出現する位置を返します。
        /// サロゲートペアは１文字としてカウントされます。
        /// </summary>
        /// <param name="StringCheck">検索対象となる文字列式を指定します。</param>
        /// <param name="StringMatch">引数 StringCheck 内で検索する文字列式を指定します。</param>
        /// <param name="Start">検索の開始位置を表す数式を指定します。</param>
        /// <param name="Compare">文字列比較の比較モードを指定する番号を設定します。</param>
        /// <returns>最初に出現する位置</returns>
        public static int InStrRevU(string StringCheck, string StringMatch, int Start = -1, CompareMethod Compare = CompareMethod.Binary) {
            if (Start != -1)
                StringCheck = LeftU(StringCheck, Start);
            int nPos = Strings.InStrRev(StringCheck, StringMatch, -1, Compare);
            if (nPos == 0) return 0;
            return LenU(Strings.Mid(StringCheck, 1, nPos));
        }

        /// <summary>
        /// 指定どおりに変換された VBString オブジェクトを返します。Unicode を文字化けさせません。
        /// </summary>
        /// <param name="Expression">変換する VBString 型の式。</param>
        /// <param name="Conversion">vb6Conversion 列挙型 メンバ。実行する比較の種類を指定する列挙値。</param>
        /// <returns>変換後の VBString オブジェクト</returns>
        public static VBString VB6StrConv(VBString Expression, vb6Conversion Conversion) {
            return VB6StrConv(Expression, Conversion, 0);
        }

        /// <summary>
        /// 指定どおりに変換された VBString オブジェクトを返します。Unicode を文字化けさせません。
        /// </summary>
        /// <param name="Expression">変換する VBString 型の式。</param>
        /// <param name="Conversion">VbStrConvEx 列挙型 メンバ。実行する比較の種類を指定する列挙値。</param>
        /// <param name="LocaleID">システムとは異なる国別情報識別子 (LCID) を指定できます。</param>
        /// <returns>変換後の VBString オブジェクト</returns>
        public static VBString VB6StrConv(VBString Expression, vb6Conversion Conversion, int LocaleID) {
            if ((object)Expression == null) return string.Empty;
            if (Expression.LengthB == 0) return string.Empty;

            // vbUnicode は先に変換する

            if ((Conversion & vb6Conversion.vbUnicode) == vb6Conversion.vbUnicode) {
                Expression = VBEncoding.Default.GetString(Expression.ToByteArray());
                Conversion = Conversion & ~vb6Conversion.vbUnicode;
            }

            // vbFromUnicode は後で変換するので、フラグとして保存する

            bool fromUnicode = (Conversion & vb6Conversion.vbFromUnicode) == vb6Conversion.vbFromUnicode;
            Conversion = Conversion & ~vb6Conversion.vbFromUnicode;

            // LCMapString を実行
            if (Conversion != 0)
                Expression = LCMapStringCaller(Expression, (VbStrConv)Conversion, LocaleID);

            // シフト JIS に変換
            if (fromUnicode)
                return VBEncoding.Default.GetBytes(Expression);
            else
                return Expression;
        }

        // LCMapString を実行する

        private static string LCMapStringCaller(string value, VbStrConv Conversion, int LocaleID = 0) {
            // LocaleID から cultureInfo を取得

            CultureInfo cultureInfo;
            if (LocaleID == 0 || LocaleID == 1) {
                cultureInfo = Thread.CurrentThread.CurrentCulture;
                LocaleID = cultureInfo.LCID;
            } else {
                cultureInfo = new CultureInfo(LocaleID & 65535);
            }

            int dwMapFlags = 0;
            bool properCase = (Conversion & VbStrConv.ProperCase) == VbStrConv.ProperCase;

            // ProperCase(3) が指定されていなければ、Uppercase(1) と Lowercase(2) が有効

            if (!properCase) {
                if ((Conversion & VbStrConv.Uppercase) == VbStrConv.Uppercase) dwMapFlags |= NativeMethods.LCMAP_UPPERCASE;
                if ((Conversion & VbStrConv.Lowercase) == VbStrConv.Lowercase) dwMapFlags |= NativeMethods.LCMAP_LOWERCASE;
            }

            // その他のフラグを設定

            if ((Conversion & VbStrConv.Wide) == VbStrConv.Wide) dwMapFlags |= NativeMethods.LCMAP_FULLWIDTH;
            if ((Conversion & VbStrConv.Narrow) == VbStrConv.Narrow) dwMapFlags |= NativeMethods.LCMAP_HALFWIDTH;

            if ((Conversion & VbStrConv.Hiragana) == VbStrConv.Hiragana) dwMapFlags |= NativeMethods.LCMAP_HIRAGANA;
            if ((Conversion & VbStrConv.Katakana) == VbStrConv.Katakana) dwMapFlags |= NativeMethods.LCMAP_KATAKANA;

            if ((Conversion & VbStrConv.SimplifiedChinese) == VbStrConv.SimplifiedChinese) dwMapFlags |= NativeMethods.LCMAP_SIMPLIFIED_CHINESE;
            if ((Conversion & VbStrConv.TraditionalChinese) == VbStrConv.TraditionalChinese) dwMapFlags |= NativeMethods.LCMAP_TRADITIONAL_CHINESE;
            if ((Conversion & VbStrConv.LinguisticCasing) == VbStrConv.LinguisticCasing) dwMapFlags |= NativeMethods.LCMAP_LINGUISTIC_CASING;

            // 必要なバッファ長を調べ、アンマネージメモリを確保する

            int stringLength = NativeMethods.LCMapString(cultureInfo.LCID, dwMapFlags, value, value.Length, IntPtr.Zero, 0);
            if (stringLength == 0) {
                throw new Exception(string.Format("LCMapString がエラー値を返しました。{0}", Marshal.GetLastWin32Error()));
            }

            // 文字数が返るので２倍のバイト数が必要

            int bufferLength = stringLength * 2;
            IntPtr buffer = Marshal.AllocHGlobal(bufferLength);

            try {
                // 変換
                int result = NativeMethods.LCMapString(cultureInfo.LCID, dwMapFlags, value, value.Length, buffer, bufferLength);
                if (result == 0) {
                    throw new Exception(string.Format("LCMapString がエラー値を返しました。{0}", Marshal.GetLastWin32Error()));
                }

                // 文字数分切り出し
                string returnString = Marshal.PtrToStringUni(buffer, result);

                // ProperCase が指定されていれば変換(全角アルファベットもOK)
                if (properCase)
                    return cultureInfo.TextInfo.ToTitleCase(returnString);

                return returnString;
            } finally {
                // アンマネージメモリを解放
                Marshal.FreeHGlobal(buffer);
            }
        }

        /// <summary>
        /// シフトJIS に変換できないときに置き換えるキャラクタ
        /// </summary>
        static readonly byte[] notConvertible = VBEncoding.Default.GetBytes("■");

        #endregion
    }
}
