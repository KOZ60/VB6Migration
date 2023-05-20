using System;
using System.Collections.Generic;
using System.Text;

namespace VBCompatible
{
    /// <summary>
    /// 文字エンコーディングを表します。
    /// </summary>
    public class VBEncoding : ICloneable
    {
        #region static メンバ

        [ThreadStatic]
        private static Dictionary<Encoding, VBEncoding> _EncodingDictionary;

        private static VBEncoding GetVBEncoding(Encoding enc) {
            if (_EncodingDictionary == null) {
                _EncodingDictionary = new Dictionary<Encoding, VBEncoding>();
            }
            VBEncoding vbenc;
            if (!_EncodingDictionary.TryGetValue(enc, out vbenc)) {
                vbenc = new VBEncoding(enc);
                _EncodingDictionary.Add(enc, vbenc);
            }
            return vbenc;
        }

        /// <summary>
        /// VBEncoding を Encoding に変換します。
        /// </summary>
        /// <param name="value">変換する VBEncoding</param>
        /// <returns>変換された Encoding</returns>
        public static implicit operator Encoding(VBEncoding value) {
            return value._enc;
        }

        /// <summary>
        /// ASCII (7 ビット) 文字セットのエンコーディングを取得します。
        /// </summary>
        public static VBEncoding ASCII {
            get {
                return GetVBEncoding(Encoding.ASCII);
            }
        }

        /// <summary>
        /// ビッグ エンディアンのバイト順を使用する UTF-16 形式のエンコーディングを取得します。
        /// </summary>
        public static VBEncoding BigEndianUnicode {
            get {
                return GetVBEncoding(Encoding.BigEndianUnicode);
            }
        }

        /// <summary>
        /// オペレーティング システムの現在の ANSI コード ページのエンコーディングを取得します。
        /// </summary>
        public static VBEncoding Default {
            get {
                return GetVBEncoding(Encoding.Default);
            }
        }

        /// <summary>
        /// リトル エンディアン バイト順を使用する UTF-16 形式のエンコーディングを取得します。
        /// </summary>
        public static VBEncoding Unicode {
            get {
                return GetVBEncoding(Encoding.Unicode);
            }
        }

        /// <summary>
        /// リトル エンディアン バイト順を使用する UTF-32 形式のエンコーディングを取得します。
        /// </summary>
        public static VBEncoding UTF32 {
            get {
                return GetVBEncoding(Encoding.UTF32);
            }
        }

        /// <summary>
        /// UTF-7 形式のエンコーディングを取得します。
        /// </summary>
        public static VBEncoding UTF7 {
            get {
                return GetVBEncoding(Encoding.UTF7);
            }
        }

        /// <summary>
        /// UTF-8 形式のエンコーディングを取得します。
        /// </summary>
        public static VBEncoding UTF8 {
            get {
                return GetVBEncoding(Encoding.UTF8);
            }
        }

        /// <summary>
        /// バイト配列全体を、あるエンコーディングから別のエンコーディングに変換します。
        /// </summary>
        /// <param name="srcEncoding">bytes のエンコーディング形式。 </param>
        /// <param name="dstEncoding">変換後のエンコーディング形式。 </param>
        /// <param name="bytes">変換対象のバイト。 </param>
        /// <returns>bytes を srcEncoding から dstEncoding へ変換した結果を格納する Byte 型の配列。 </returns>
        public static byte[] Convert(VBEncoding srcEncoding, VBEncoding dstEncoding, byte[] bytes) {
            return Encoding.Convert(srcEncoding._enc, dstEncoding._enc, bytes);
        }

        /// <summary>
        /// バイト配列内のバイトの範囲を、あるエンコーディングから別のエンコーディングに変換します。
        /// </summary>
        /// <param name="srcEncoding">変換前の配列 bytes のエンコーディング。 </param>
        /// <param name="dstEncoding">変換後の配列のエンコーディング。 </param>
        /// <param name="bytes">変換対象のバイト配列。</param>
        /// <param name="index">変換対象の bytes の最初の要素を示すインデックス。</param>
        /// <param name="count">変換するバイト数。 </param>
        /// <returns>bytes に含まれる特定の範囲のバイトを srcEncoding から dstEncoding へ変換した結果が格納されている Byte 型の配列。</returns>
        public static byte[] Convert(VBEncoding srcEncoding, VBEncoding dstEncoding, byte[] bytes, int index, int count) {
            return Encoding.Convert(srcEncoding._enc, dstEncoding._enc, bytes, index, count);
        }

        /// <summary>
        /// 指定したコード ページ ID に関連付けられたエンコーディングを返します。
        /// </summary>
        /// <param name="codepage">使用するエンコーディングのコード ページ ID。 使用可能な値は、 Encoding クラスのトピックに記載されている表の、コード ページの列にリストされています。 既定のエンコーディングを使用する場合は 0。 </param>
        /// <returns>指定したコード ページに関連付けられたエンコーディング。 </returns>
        public static VBEncoding GetEncoding(int codepage) {
            return new VBEncoding(Encoding.GetEncoding(codepage));
        }

        /// <summary>
        /// 指定したコード ページ名に関連付けられたエンコーディングを返します。
        /// </summary>
        /// <param name="name">使用するエンコーディングのコード ページ名。 WebName プロパティが返す値はすべて有効です。 使用可能な値は、 Encoding クラスのトピックに記載されている表の、名前の列にリストされています。 </param>
        /// <returns>指定したコード ページに関連付けられたエンコード。 </returns>
        public static VBEncoding GetEncoding(string name) {
            return new VBEncoding(Encoding.GetEncoding(name));
        }

        /// <summary>
        /// 指定したコード ページ ID に関連付けられたエンコーディングを返します。 パラメーターには、エンコードできない文字とデコードできないバイト シーケンスのためのエラー ハンドラーを指定します。
        /// </summary>
        /// <param name="codepage"></param>
        /// <param name="encoderFallback">現在のエンコーディングで文字をエンコードできない場合にエラー処理プロシージャを提供するオブジェクト。 </param>
        /// <param name="decoderFallback">現在のエンコーディングでバイト シーケンスをデコードできない場合にエラー処理プロシージャを提供するオブジェクト。 </param>
        /// <returns>指定したコード ページに関連付けられたエンコーディング。 </returns>
        public static VBEncoding GetEncoding(int codepage, EncoderFallback encoderFallback, DecoderFallback decoderFallback) {
            Encoding enc = Encoding.GetEncoding(codepage);
            return new VBEncoding(enc, encoderFallback, decoderFallback);
        }

        /// <summary>
        /// 指定したコード ページ名に関連付けられたエンコーディングを返します。 パラメーターには、エンコードできない文字とデコードできないバイト シーケンスのためのエラー ハンドラーを指定します。
        /// </summary>
        /// <param name="name">使用するエンコーディングのコード ページ名。 WebName プロパティが返す値はすべて有効です。 使用可能な値は、 Encoding クラスのトピックに記載されている表の、名前の列にリストされています。</param>
        /// <param name="encoderFallback">現在のエンコーディングで文字をエンコードできない場合にエラー処理プロシージャを提供するオブジェクト。</param>
        /// <param name="decoderFallback">現在のエンコーディングでバイト シーケンスをデコードできない場合にエラー処理プロシージャを提供するオブジェクト。</param>
        /// <returns>指定したコード ページに関連付けられたエンコーディング。</returns>
        public static VBEncoding GetEncoding(string name, EncoderFallback encoderFallback, DecoderFallback decoderFallback) {
            Encoding enc = Encoding.GetEncoding(name);
            return new VBEncoding(enc, encoderFallback, decoderFallback);
        }

        #endregion

        #region インスタンスメンバ

        private Encoding _enc;

        private static EncoderFallback _DefaultEncoderFallback;
        private static DecoderFallback _DefaultDecoderFallback;

        private static EncoderFallback DefaultEncoderFallback {
            get {
                if (_DefaultEncoderFallback == null) {
                    _DefaultEncoderFallback = new EncoderReplacementFallback("■");
                }
                return _DefaultEncoderFallback;
            }
        }

        private static DecoderFallback DefaultDecoderFallback {
            get {
                if (_DefaultDecoderFallback == null) {
                    _DefaultDecoderFallback = new DecoderReplacementFallback("■");
                }
                return _DefaultDecoderFallback;
            }
        }

        internal VBEncoding(Encoding enc)
            : this(enc, DefaultEncoderFallback, DefaultDecoderFallback) {
        }

        internal VBEncoding(Encoding enc, EncoderFallback encoderFallback, DecoderFallback decoderFallback) {
            _enc = (Encoding)enc.Clone();
            _enc.EncoderFallback = encoderFallback;
            _enc.DecoderFallback = decoderFallback;
        }

        /// <summary>
        /// 指定した文字配列に格納されている文字のセットをエンコードすることによって生成されるバイト数を計算します。
        /// </summary>
        /// <param name="chars">エンコード対象の文字のセットを格納している文字配列。</param>
        /// <param name="index">エンコードする最初の文字のインデックス。 </param>
        /// <param name="count">エンコードする文字数。</param>
        /// <returns>指定した文字をエンコードすることによって生成されるバイト数。</returns>
        public virtual int GetByteCount(char[] chars, int index, int count) {
            return _enc.GetByteCount(chars, index, count);
        }

        /// <summary>
        /// 指定した文字配列に格納されている文字のセットを、指定したバイト配列にエンコードします。
        /// </summary>
        /// <param name="chars">エンコード対象の文字のセットを格納している文字配列。 </param>
        /// <param name="charIndex">エンコードする最初の文字のインデックス。 </param>
        /// <param name="charCount">エンコードする文字数。</param>
        /// <param name="bytes">結果のバイト シーケンスを格納するバイト配列。</param>
        /// <param name="byteIndex">結果のバイト シーケンスを書き込む開始位置のインデックス。</param>
        /// <returns>bytes に書き込まれた実際のバイト数。</returns>
        public virtual int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) {
            return _enc.GetBytes(chars, charIndex, charCount, bytes, byteIndex);
        }

        /// <summary>
        /// 指定したバイト配列に格納されているバイト シーケンスをデコードすることによって生成される文字数を計算します。
        /// </summary>
        /// <param name="bytes">デコード対象のバイト シーケンスが格納されたバイト配列。</param>
        /// <param name="index">デコードする最初のバイトのインデックス。</param>
        /// <param name="count">デコードするバイト数。 </param>
        /// <returns>指定したバイト シーケンスをデコードすることによって生成される文字数。</returns>
        public virtual int GetCharCount(byte[] bytes, int index, int count) {
            return _enc.GetCharCount(bytes, index, count);
        }

        /// <summary>
        /// 指定したバイト配列に格納されているバイト シーケンスを、指定した文字配列にデコードします。
        /// </summary>
        /// <param name="bytes">デコード対象のバイト シーケンスが格納されたバイト配列。</param>
        /// <param name="byteIndex">デコードする最初のバイトのインデックス。</param>
        /// <param name="byteCount">デコードするバイト数。</param>
        /// <param name="chars">結果の文字のセットを格納する文字配列。</param>
        /// <param name="charIndex">結果の文字のセットを書き込む開始位置のインデックス。</param>
        /// <returns>chars に書き込まれた実際の文字数。 </returns>
        public virtual int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) {
            return _enc.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
        }

        /// <summary>
        /// 指定した文字数をエンコードすることによって生成される最大バイト数を計算します。
        /// </summary>
        /// <param name="charCount">エンコードする文字数。</param>
        /// <returns>指定した文字数をエンコードすることによって生成される最大バイト数。</returns>
        public virtual int GetMaxByteCount(int charCount) {
            return _enc.GetMaxByteCount(charCount);
        }

        /// <summary>
        /// 指定したバイト数をデコードすることによって生成される最大文字数を計算します。
        /// </summary>
        /// <param name="byteCount">デコードするバイト数。</param>
        /// <returns>指定したバイト数をデコードすることによって生成される最大文字数。</returns>
        public virtual int GetMaxCharCount(int byteCount) {
            return _enc.GetMaxCharCount(byteCount);
        }

        /// <summary>
        /// メール エージェントの Body タグと共に使用できる現在のエンコーディングの名前を取得します。
        /// </summary>
        public virtual string BodyName {
            get {
                return _enc.BodyName;
            }
        }

        /// <summary>
        /// 現在の Encoding のコード ページ ID を取得します。
        /// </summary>
        public virtual int CodePage {
            get {
                return _enc.CodePage;
            }
        }

        /// <summary>
        /// 現在の VBEncoding オブジェクトの簡易コピーを作成します。
        /// </summary>
        /// <returns></returns>
        public virtual object Clone() {
            return new VBEncoding((Encoding)_enc.Clone());
        }

        /// <summary>
        /// 現在のエンコーディングについての記述を、ユーザーが判読できる形式で取得します。
        /// </summary>
        public virtual string EncodingName {
            get {
                return _enc.EncodingName;
            }
        }

        /// <summary>
        /// 指定した Object が、現在のインスタンスと等しいかどうかを判断します。
        /// </summary>
        /// <param name="value">現在のインスタンスと比較する Object。</param>
        /// <returns>value が Encoding のインスタンスで、現在のインスタンスと等しい場合は true。それ以外の場合は false。 </returns>
        public override bool Equals(object value) {
            VBEncoding enc = value as VBEncoding;
            if (enc != null) {
                return _enc.Equals(enc._enc);
            }
            return false;
        }

        /// <summary>
        /// 指定した文字ポインターから始まる文字のセットをエンコードすることによって生成されるバイト数を計算します。
        /// </summary>
        /// <param name="chars">エンコードする最初の文字へのポインター。 </param>
        /// <param name="count">エンコードする文字数。 </param>
        /// <returns>指定した文字をエンコードすることによって生成されるバイト数。</returns>
        public unsafe virtual int GetByteCount(char* chars, int count) {
            return _enc.GetByteCount(chars, count);
        }

        /// <summary>
        /// 指定した文字配列に格納されているすべての文字をエンコードすることによって生成されるバイト数を計算します。
        /// </summary>
        /// <param name="chars">エンコード対象の文字を格納している文字配列。</param>
        /// <returns>指定した文字配列に格納されているすべての文字をエンコードすることによって生成されるバイト数。 </returns>
        public virtual int GetByteCount(char[] chars) {
            return _enc.GetByteCount(chars);
        }

        /// <summary>
        /// 指定した文字列に含まれる文字をエンコードすることによって生成されるバイト数を計算します。
        /// </summary>
        /// <param name="s">エンコード対象の文字のセットを格納している文字列。</param>
        /// <returns>指定した文字をエンコードすることによって生成されるバイト数。 </returns>
        public virtual int GetByteCount(string s) {
            return _enc.GetByteCount(s);
        }

        /// <summary>
        /// 派生クラスでオーバーライドされた場合、指定した文字ポインターで始まる文字のセットを、指定したバイト ポインターを開始位置として格納されるバイト シーケンスにエンコードします。
        /// </summary>
        /// <param name="chars">エンコードする最初の文字へのポインター。 </param>
        /// <param name="charCount">エンコードする文字数。</param>
        /// <param name="bytes">結果のバイト シーケンスの書き込みを開始する位置へのポインター。 </param>
        /// <param name="byteCount">書き込む最大バイト数。</param>
        /// <returns>bytes パラメーターによって示される位置に書き込まれた実際のバイト数。</returns>
        public unsafe virtual int GetBytes(char* chars, int charCount, byte* bytes, int byteCount) {
            return _enc.GetBytes(chars, charCount, bytes, byteCount);
        }

        /// <summary>
        /// 指定した文字配列に格納されているすべての文字をバイト シーケンスにエンコードします。
        /// </summary>
        /// <param name="chars">エンコード対象の文字を格納している文字配列。 </param>
        /// <returns>指定した文字のセットをエンコードした結果を格納しているバイト配列。</returns>
        public virtual byte[] GetBytes(char[] chars) {
            return _enc.GetBytes(chars);
        }

        /// <summary>
        /// 指定した文字配列に格納されている文字のセットをバイト シーケンスにエンコードします。
        /// </summary>
        /// <param name="chars">エンコード対象の文字のセットを格納している文字配列。</param>
        /// <param name="index">エンコードする最初の文字のインデックス。 </param>
        /// <param name="count">エンコードする文字数。</param>
        /// <returns>指定した文字のセットをエンコードした結果を格納しているバイト配列。 </returns>
        public virtual byte[] GetBytes(char[] chars, int index, int count) {
            return _enc.GetBytes(chars, index, count);
        }

        /// <summary>
        /// 指定した文字列に含まれるすべての文字をバイト シーケンスにエンコードします。
        /// </summary>
        /// <param name="s">エンコードする文字を含む文字列。 </param>
        /// <returns>指定した文字のセットをエンコードした結果を格納しているバイト配列。</returns>
        public virtual byte[] GetBytes(string s) {
            return _enc.GetBytes(s);
        }

        /// <summary>
        /// 指定した文字列に含まれる文字のセットを、指定したバイト配列にエンコードします。
        /// </summary>
        /// <param name="s">エンコード対象の文字のセットを格納している文字列。</param>
        /// <param name="charIndex">エンコードする最初の文字のインデックス。</param>
        /// <param name="charCount">エンコードする文字数。 </param>
        /// <param name="bytes">結果のバイト シーケンスを格納するバイト配列。</param>
        /// <param name="byteIndex">結果のバイト シーケンスを書き込む開始位置のインデックス。 </param>
        /// <returns>bytes に書き込まれた実際のバイト数。</returns>
        public virtual int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex) {
            return _enc.GetBytes(s, charIndex, charCount, bytes, byteIndex);
        }

        /// <summary>
        /// 指定したバイト ポインターから始まるバイト シーケンスをデコードすることによって生成される文字数を計算します。
        /// </summary>
        /// <param name="bytes">デコードする最初のバイトへのポインター。 </param>
        /// <param name="count">デコードするバイト数。 </param>
        /// <returns>指定したバイト シーケンスをデコードすることによって生成される文字数。</returns>
        public unsafe virtual int GetCharCount(byte* bytes, int count) {
            return _enc.GetCharCount(bytes, count);
        }

        /// <summary>
        /// 指定したバイト配列に格納されているすべてのバイトをデコードすることによって生成される文字数を計算します。
        /// </summary>
        /// <param name="bytes">デコード対象のバイト シーケンスが格納されたバイト配列。 </param>
        /// <returns>指定したバイト シーケンスをデコードすることによって生成される文字数。</returns>
        public virtual int GetCharCount(byte[] bytes) {
            return _enc.GetCharCount(bytes);
        }

        /// <summary>
        /// 指定したバイト ポインターで始まるバイト シーケンスを、指定した文字ポインターを開始位置として格納される文字のセットにエンコードします。
        /// </summary>
        /// <param name="bytes">デコードする最初のバイトへのポインター。 </param>
        /// <param name="byteCount">デコードするバイト数。 </param>
        /// <param name="chars">結果の文字セットの書き込みを開始する位置へのポインター。</param>
        /// <param name="charCount">書き込む文字の最大数。 </param>
        /// <returns>chars パラメーターによって示される位置に書き込まれた実際の文字数。</returns>
        public unsafe virtual int GetChars(byte* bytes, int byteCount, char* chars, int charCount) {
            return _enc.GetChars(bytes, byteCount, chars, charCount);
        }

        /// <summary>
        /// 指定したバイト配列に格納されているすべてのバイトを文字のセットにデコードします。
        /// </summary>
        /// <param name="bytes">デコード対象のバイト シーケンスが格納されたバイト配列。</param>
        /// <returns>指定したバイト シーケンスのデコード結果が格納された文字配列。 </returns>
        public virtual char[] GetChars(byte[] bytes) {
            return _enc.GetChars(bytes);
        }

        /// <summary>
        /// 指定したバイト配列に格納されているバイト シーケンスを文字のセットにデコードします。
        /// </summary>
        /// <param name="bytes">デコード対象のバイト シーケンスが格納されたバイト配列。</param>
        /// <param name="index">デコードする最初のバイトのインデックス。</param>
        /// <param name="count">デコードするバイト数。 </param>
        /// <returns>指定したバイト シーケンスのデコード結果が格納された文字配列。</returns>
        public virtual char[] GetChars(byte[] bytes, int index, int count) {
            return _enc.GetChars(bytes, index, count);
        }

        /// <summary>
        /// エンコード済みバイト シーケンスを文字シーケンスに変換するデコーダーを取得します。
        /// </summary>
        /// <returns>エンコード済みバイト シーケンスを文字シーケンスに変換する Decoder。</returns>
        public virtual Decoder GetDecoder() {
            return _enc.GetDecoder();
        }

        /// <summary>
        /// Unicode 文字のシーケンスをエンコード済みバイト シーケンスに変換するエンコーダーを取得します。
        /// </summary>
        /// <returns>Unicode 文字のシーケンスをエンコード済みバイト シーケンスに変換する Encoder。 </returns>
        public virtual Encoder GetEncoder() {
            return _enc.GetEncoder();
        }

        /// <summary>
        /// 現在のインスタンスのハッシュ コードを返します。
        /// </summary>
        /// <returns>現在のインスタンスのハッシュ コード。 </returns>
        public override int GetHashCode() {
            return _enc.GetHashCode();
        }

        /// <summary>
        /// 使用するエンコーディングを指定するバイト シーケンスを返します。
        /// </summary>
        /// <returns>使用するエンコーディングを指定するバイト シーケンスを格納するバイト配列。 </returns>
        public virtual byte[] GetPreamble() {
            return _enc.GetPreamble();
        }

        /// <summary>
        /// 指定したバイト配列に格納されているすべてのバイトを文字列にデコードします。
        /// </summary>
        /// <param name="bytes">デコード対象のバイト シーケンスが格納されたバイト配列。 </param>
        /// <returns>指定したバイト シーケンスのデコード結果が格納されている文字列。</returns>
        public virtual string GetString(byte[] bytes) {
            return _enc.GetString(bytes);
        }

        /// <summary>
        /// 指定したバイト配列に格納されているバイト シーケンスを文字列にデコードします。
        /// </summary>
        /// <param name="bytes">デコード対象のバイト シーケンスが格納されたバイト配列。 </param>
        /// <param name="index">デコードする最初のバイトのインデックス。 </param>
        /// <param name="count">デコードするバイト数。</param>
        /// <returns>指定したバイト シーケンスのデコード結果が格納されている文字列。 </returns>
        public virtual string GetString(byte[] bytes, int index, int count) {
            return _enc.GetString(bytes, index, count);
        }

        /// <summary>
        /// メール エージェント ヘッダー タグと共に使用できる現在のエンコーディングの名前を取得します。
        /// </summary>
        public virtual string HeaderName {
            get {
                return _enc.HeaderName;
            }
        }

        /// <summary>
        /// 現在のエンコーディングが、既定の正規化方式を使用して常に正規化されるかどうかを示す値。
        /// </summary>
        /// <returns>現在の Encoding が常に正規化される場合は true。それ以外の場合は false。 既定値は、false です。 </returns>
        public virtual bool IsAlwaysNormalized() {
            return _enc.IsAlwaysNormalized();
        }

        /// <summary>
        /// 現在のエンコーディングが、指定した正規化方式を使用して常に正規化されるかどうかを示す値を取得します。
        /// </summary>
        /// <param name="form">System.Text.NormalizationForm 値の 1 つ。</param>
        /// <returns>現在の Encoding オブジェクトが、指定した NormalizationForm 値を使用して常に正規化される場合は true。それ以外の場合は false。 既定値は、false です。 </returns>
        public virtual bool IsAlwaysNormalized(NormalizationForm form) {
            return _enc.IsAlwaysNormalized(form);
        }

        /// <summary>
        /// ブラウザー クライアントが現在のエンコーディングを使用してコンテンツを表示できるかどうかを示す値を取得します。
        /// </summary>
        public virtual bool IsBrowserDisplay {
            get {
                return _enc.IsBrowserDisplay;
            }
        }

        /// <summary>
        /// ブラウザー クライアントが現在のエンコーディングを使用してコンテンツを保存できるかどうかを示す値を取得します。
        /// </summary>
        public virtual bool IsBrowserSave {
            get {
                return _enc.IsBrowserSave;
            }
        }

        /// <summary>
        /// メール クライアントおよびニュース クライアントが現在のエンコーディングを使用してコンテンツを表示できるかどうかを示す値を取得します。
        /// </summary>
        public virtual bool IsMailNewsDisplay {
            get {
                return _enc.IsMailNewsDisplay;
            }
        }

        /// <summary>
        /// メール クライアントおよびニュース クライアントが現在のエンコーディングを使用してコンテンツを保存できるかどうかを示す値を取得します。
        /// </summary>
        public virtual bool IsMailNewsSave {
            get {
                return _enc.IsMailNewsSave;
            }
        }

        /// <summary>
        /// 現在のエンコーディングが読み取り専用かどうかを示す値を取得します。
        /// </summary>
        public virtual bool IsReadOnly {
            get {
                return _enc.IsReadOnly;
            }
        }

        /// <summary>
        /// 現在のエンコーディングが 1 バイトのコード ポイントを使用するかどうかを示す値を取得します。
        /// </summary>
        public virtual bool IsSingleByte {
            get {
                return _enc.IsSingleByte;
            }
        }

        /// <summary>
        /// 現在の System.Object を表す System.String を返します。
        /// </summary>
        /// <returns>現在の System.Object を表す System.String。</returns>
        public override string ToString() {
            return _enc.ToString();
        }

        /// <summary>
        /// 現在のエンコーディングの IANA (Internet Assigned Numbers Authority) に登録されている名前を取得します。
        /// </summary>
        public virtual string WebName {
            get {
                return _enc.WebName;
            }
        }

        /// <summary>
        /// 現在のエンコーディングに最も厳密に対応する Windows オペレーティング システムのコード ページを取得します。
        /// </summary>
        public virtual int WindowsCodePage {
            get {
                return _enc.WindowsCodePage;
            }
        }

        #endregion

    }
}
