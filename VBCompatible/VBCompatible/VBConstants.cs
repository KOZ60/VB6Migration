namespace VBCompatible
{
    using System.ComponentModel;

    /// <summary>
    /// タブキーを押下したときのフォーカス遷移を VB6.0 互換にするかどうかを設定する列挙体。
    /// </summary>
    public enum TabIndexMode
    {
        /// <summary>VB.NET 互換モード</summary>
        NET = 0,
        /// <summary>VB6.0 互換モード</summary>
        VB6 = 1,
    }

    /// <summary>
    /// コントロールの外観を指定します。
    /// </summary>
    public enum VBFlatStyle
    {
        /// <summary>VB6.0 互換スタイル</summary>
        ClassicStyle,
        /// <summary>ビジュアルスタイル</summary>
        VisualStyle,
        /// <summary>フラットスタイル</summary>
        FlatStyle
    }

    /// <summary>
    /// Flat Style の枠の太さを設定します。
    /// </summary>
    public enum FlatBorderSize
    {
        /// <summary>枠なし</summary>
        None,
        /// <summary>通常の太さ(1 dot)</summary>
        Normal,
        /// <summary>太い枠線(2 dot)</summary>
        Bold,
    }

    /// <summary>
    /// 文字列の均等割り付けのスタイル
    /// </summary>
    public enum TextJustifyStyles
    {
        /// <summary>均等割り付けしない</summary>
        None,
        /// <summary>文字列を均等に割付ける。 </summary>
        Justify,
        /// <summary>両端の空白も均等にする。</summary>
        FullJustify,
    }

    /// <summary>
    /// VisualStyle で描画する DateTimePicker のビジュアル状態を指定します。
    /// </summary>
    public enum CalenderButtonState
    {
        /// <summary>既定の外観</summary>
        Normal = 1,
        /// <summary>ホットな状態</summary>
        Hot = 2,
        /// <summary>押された状態</summary>
        Pressed = 3,
        /// <summary>無効な状態</summary>
        Disabled = 4,
    }

    /// <summary>
    /// テキスト入力パターンの名前を定義します。 
    /// </summary>
    public enum InputScope
    {
        /// <summary>XML のテキスト入力パターン。</summary>
        Xml = -4,
        /// <summary>Speech Recognition Grammar Specification (SRGS) のテキスト入力パターン。</summary>
        Srgs = -3,
        /// <summary>正規表現のテキスト入力パターン。</summary>
        RegularExpression = -2,
        /// <summary>語句一覧のテキスト入力パターン。</summary>
        PhraseList = -1,
        /// <summary>入力コマンドの既定の処理。</summary>
        Default = 0,
        /// <summary>URL (Uniform Resource Locator) のテキスト入力パターン。</summary>
        Url = 1,
        /// <summary>ファイルの完全パスのテキスト入力パターン。</summary>
        FullFilePath = 2,
        /// <summary>ファイル名のテキスト入力パターン。</summary>
        FileName = 3,
        /// <summary>電子メール ユーザー名のテキスト入力パターン。</summary>
        EmailUserName = 4,
        /// <summary>簡易メール転送プロトコル (SMTP) 電子メール アドレスのテキスト入力パターン。</summary>
        EmailSmtpAddress = 5,
        /// <summary>ログオン名のテキスト入力パターン。</summary>
        LogOnName = 6,
        /// <summary>人名 (フル ネーム) のテキスト入力パターン。</summary>
        PersonalFullName = 7,
        /// <summary>人名のプレフィックスのテキスト入力パターン。</summary>
        PersonalNamePrefix = 8,
        /// <summary>人名 (名) のテキスト入力パターン。</summary>
        PersonalGivenName = 9,
        /// <summary>人名 (ミドル ネーム) のテキスト入力パターン。</summary>
        PersonalMiddleName = 10,
        /// <summary>人名 (姓) のテキスト入力パターン。</summary>
        PersonalSurname = 11,
        /// <summary>人名のサフィックスのテキスト入力パターン。</summary>
        PersonalNameSuffix = 12,
        /// <summary>住所のテキスト入力パターン。</summary>
        PostalAddress = 13,
        /// <summary>郵便番号のテキスト入力パターン。</summary>
        PostalCode = 14,
        /// <summary>番地のテキスト入力パターン。</summary>
        AddressStreet = 15,
        /// <summary>都道府県のテキスト入力パターン。</summary>
        AddressStateOrProvince = 16,
        /// <summary>都市名のテキスト入力パターン。</summary>
        AddressCity = 17,
        /// <summary>国名のテキスト入力パターン。</summary>
        AddressCountryName = 18,
        /// <summary>国の省略名のテキスト入力パターン。</summary>
        AddressCountryShortName = 19,
        /// <summary>通貨金額および通貨記号のテキスト入力パターン。</summary>
        CurrencyAmountAndSymbol = 20,
        /// <summary>通貨金額のテキスト入力パターン。</summary>
        CurrencyAmount = 21,
        /// <summary>カレンダー日付のテキスト入力パターン。</summary>
        Date = 22,
        /// <summary>カレンダー日付の月 (数字) のテキスト入力パターン。</summary>
        DateMonth = 23,
        /// <summary>カレンダー日付の日 (数字) のテキスト入力パターン。</summary>
        DateDay = 24,
        /// <summary>カレンダー日付の年のテキスト入力パターン。</summary>
        DateYear = 25,
        /// <summary>カレンダー日付の月 (名前) のテキスト入力パターン。</summary>
        DateMonthName = 26,
        /// <summary>カレンダー日付の日 (名前) のテキスト入力パターン。</summary>
        DateDayName = 27,
        /// <summary>数字のテキスト入力パターン。</summary>
        Digits = 28,
        /// <summary>数字のテキスト入力パターン。</summary>
        Number = 29,
        /// <summary>1 文字のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        OneChar = 30,
        /// <summary>パスワードのテキスト入力パターン。</summary>
        Password = 31,
        /// <summary>電話番号のテキスト入力パターン。</summary>
        TelephoneNumber = 32,
        /// <summary>電話の国番号のテキスト入力パターン。</summary>
        TelephoneCountryCode = 33,
        /// <summary>電話の市外局番のテキスト入力パターン。</summary>
        TelephoneAreaCode = 34,
        /// <summary>電話の市内局番のテキスト入力パターン。</summary>
        TelephoneLocalNumber = 35,
        /// <summary>時刻のテキスト入力パターン。</summary>
        Time = 36,
        /// <summary>時刻 (時) のテキスト入力パターン。</summary>
        TimeHour = 37,
        /// <summary>時刻 (分または秒) のテキスト入力パターン。</summary>
        TimeMinorSec = 38,
        /// <summary>全角数字のテキスト入力パターン。</summary>
        NumberFullWidth = 39,
        /// <summary>半角英数字のテキスト入力パターン。</summary>
        AlphanumericHalfWidth = 40,
        /// <summary>全角英数字のテキスト入力パターン。</summary>
        AlphanumericFullWidth = 41,
        /// <summary>中国通貨のテキスト入力パターン。</summary>
        CurrencyChinese = 42,
        /// <summary>ボポモフォ標準中国語発音表記システムのテキスト入力パターン。</summary>
        Bopomofo = 43,
        /// <summary>ひらがな書記体系のテキスト入力パターン。</summary>
        Hiragana = 44,
        /// <summary>半角カタカナ文字のテキスト入力パターン。</summary>
        KatakanaHalfWidth = 45,
        /// <summary>全角カタカナ文字のテキスト入力パターン。</summary>
        KatakanaFullWidth = 46,
        /// <summary>Hanja 文字のテキスト入力パターン。</summary>
        Hanja = 47,

        // 以下は GrapeCity INPUTMAN 7.0J SP1 より

        /// <summary>半角 Hanja 文字のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        HanjaHalfWidth = 48,
        /// <summary>全角 Hanja 文字のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        HanjaFullWidth = 49,
    }

}
