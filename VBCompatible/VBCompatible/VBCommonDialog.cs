using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VBCompatible.VB6;

namespace VBCompatible
{
    /// <summary>
    /// VB6.0 CommonDialog 互換コントロール
    /// </summary>
    [DesignerCategory("Code")]
    public class VBCommonDialog : Component
    {
        const int CDERR_DIALOGFAILURE = 0x7FF3;

        /// <summary>
        /// VBCommonDialog のインスタンスを作成します。
        /// </summary>
        public VBCommonDialog() {
            // ShowHelp
            this.HelpCommand = cdlHelpConstants.cdlUnknown;
            this.HelpContext = 0;
            this.HelpFile = string.Empty;
            this.HelpKey = string.Empty;
            this.Font = VBForm.DefaultFont;
            // ShowPrinter
            this.Min = 0;
            this.Max = 9999;
        }

        private IntPtr hWndOwner {
            get {
                IntPtr focusWindow = NativeMethods.GetFocus();
                if (focusWindow == IntPtr.Zero) {
                    return NativeMethods.GetDesktopWindow();
                } else {
                    return focusWindow;
                }
            }
        }

        /// <summary>
        /// Windows Vista で実行する場合に、外観と動作を自動的にアップグレードするかどうかを示す値を取得または設定します。
        /// </summary>
        public bool AutoUpgradeEnabled { get; set; }

        /// <summary>
        /// ユーザーによる [キャンセル] ボタンのクリックをエラーとして扱うかどうかを設定または取得します。
        /// </summary>
        public bool CancelError { get; set; }

        /// <summary>
        /// 「色の設定]または「フォントの設定」ダイアログボックスで選択される色を設定または取得します。
        /// </summary>
        public OLE_COLOR Color { get; set; }

        /// <summary>
        /// 印刷部数を設定します。値の取得も可能です。
        /// </summary>
        public short Copies {
            get {
                return PrinterSettings.Copies;
            }
            set {
                PrinterSettings.Copies = value;
            }
        }

        /// <summary>
        /// そのダイアログ ボックスで使用する既定のファイル拡張子を設定または取得します。
        /// </summary>
        public string DefaultExt { get; set; }

        /// <summary>
        /// ダイアログ ボックスのタイトル バーに表示される文字列を設定または取得します。
        /// </summary>
        public string DialogTitle { get; set; }

        /// <summary>
        /// 選択されているファイルのパスおよびファイル名を設定します
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// ダイアログ ボックスの [ファイルの種類]リスト ボックスに表示されるフィルタを設定または取得します。
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// [ファイルを開く] ダイアログ ボックスまたは [ファイル名を付けて保存] ダイアログ ボックスに表示される、既定のフィルタを設定または取得します。
        /// </summary>
        public int FilterIndex { get; set; }

        /// <summary>
        /// ダイアログ ボックスのオプションを設定または取得します。
        /// </summary>
        public int Flags { get; set; }


        /// <summary>
        /// フォントが太字かどうかを設定または取得します。
        /// </summary>
        public bool FontBold {
            get {
                return this.Font.Bold;
            }
            set {
                if (FontBold != value) {
                    ChangeFontStyle(FontStyle.Bold, value);
                }
            }
        }

        /// <summary>
        /// フォントが斜体かどうかを設定または取得します。
        /// </summary>
        public bool FontItalic {
            get {
                return this.Font.Italic;
            }
            set {
                if (FontItalic != value) {
                    ChangeFontStyle(FontStyle.Italic, value);
                }
            }
        }

        /// <summary>
        /// フォント名を設定または取得します。
        /// </summary>
        public string FontName {
            get {
                return this.Font.Name;
            }
            set {
                if (FontName != value) {
                    this.Font = new Font(value, this.FontSize, this.Font.Style);
                }
            }
        }

        /// <summary>
        /// フォントサイズを設定または取得します。
        /// </summary>
        public float FontSize {
            get {
                return this.Font.SizeInPoints;
            }
            set {
                if (FontSize != value) {
                    this.Font = new Font(this.FontName, value, this.Font.Style);
                }
            }
        }

        /// <summary>
        /// フォントが取消線を含むかどうかを設定または取得します。
        /// </summary>
        public bool FontStrikeThru {
            get {
                return this.Font.Strikeout;
            }
            set {
                if (FontStrikeThru != value) {
                    ChangeFontStyle(FontStyle.Strikeout, value);
                }
            }
        }

        /// <summary>
        /// フォントが下線を含むかどうかを設定または取得します。
        /// </summary>
        public bool FontUnderLine {
            get {
                return this.Font.Underline;
            }
            set {
                if (FontUnderLine != value) {
                    ChangeFontStyle(FontStyle.Underline, value);
                }
            }
        }

        private void ChangeFontStyle(FontStyle style, bool value) {
            FontStyle newStyle = this.Font.Style;
            if (value) {
                newStyle = newStyle | style;
            } else {
                newStyle = newStyle & ~style;
            }
            this.Font = new Font(this.Font, newStyle);
        }

        private Font _Font;

        /// <summary>
        /// フォントオブジェクトを設定または取得します。
        /// </summary>
        public Font Font {
            get {
                if (_Font == null) {
                    return VBForm.DefaultFont;
                }
                return _Font;
            }
            set {
                _Font = value;
            }
        }

        internal bool ShouldSerializeFont() {
            return (this.Font != VBForm.DefaultFont);
        }

        /// <summary>
        /// [印刷] ダイアログ ボックスの、[ページから] ボックスの値を設定または取得します。
        /// </summary>
        public short FromPage {
            get {
                return (short)PrinterSettings.FromPage;
            }
            set {
                PrinterSettings.FromPage = value;
            }
        }

        /// <summary>
        /// オブジェクトのデバイス コンテキストに設定されているハンドルを返します。
        /// </summary>
        public SafeHandle hDC { get; set; }

        /// <summary>
        /// 呼び出されるオンライン ヘルプの種類を設定または取得します。
        /// </summary>
        public cdlHelpConstants HelpCommand { get; set; }

        /// <summary>
        /// ヘルプに対応するコンテキスト番号を設定または取得します。
        /// </summary>
        public int HelpContext { get; set; }

        /// <summary>
        /// ヘルプファイル名を設定または取得します。
        /// </summary>
        public string HelpFile { get; set; }

        /// <summary>
        /// 呼び出されるヘルプ トピックを識別するキーワードを設定または取得します。
        /// </summary>
        public string HelpKey { get; set; }

        /// <summary>
        /// ダイアログ ボックスで、ファイルの場所として最初に開かれるディレクトリを設定または取得します。
        /// </summary>
        public string InitDir { get; set; }

        /// <summary>
        /// [フォントの指定] ダイアログ ボックスで、[サイズ]リスト ボックスに表示されるフォント サイズの最大値を設定または取得します。
        /// [印刷] ダイアログ ボックスで、印刷範囲の最大ページ番号を設定または取得します。
        /// </summary>
        public short Max { get; set; }

        /// <summary>
        /// ファイル名の最大サイズを設定または取得します。
        /// </summary>
        public int MaxFileSize { get; set; }

        /// <summary>
        /// [フォントの指定] ダイアログ ボックスで、[サイズ]リスト ボックスに表示されるフォント サイズの最小値を設定または取得します。
        /// [印刷] ダイアログ ボックスで、印刷範囲の最小ページ番号を設定または取得します。
        /// </summary>
        public short Min { get; set; }

        /// <summary>
        /// 文書の印刷を縦向きモードで行うか横向きモードで行うかを示す値を設定します。値の取得も可能です。デザイン時には使用できません。
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PrinterOrientationConstants Orientation {
            get {
                if (this.PrinterSettings.DefaultPageSettings.Landscape) {
                    return PrinterOrientationConstants.vbPRORLandscape;
                }
                return PrinterOrientationConstants.vbPRORPortrait;
            }
            set {
                switch (value) {
                    case PrinterOrientationConstants.vbPRORLandscape:
                        this.PrinterSettings.DefaultPageSettings.Landscape = true;
                        break;
                    case PrinterOrientationConstants.vbPRORPortrait:
                        this.PrinterSettings.DefaultPageSettings.Landscape = false;
                        break;
                }
            }
        }

        /// <summary>
        /// ユーザーが [印刷] ダイアログ ボックスで行った選択にしたがって、システム既定のプリンタの設定を変更するかどうかを指定します。値の取得も可能です。
        /// </summary>
        public bool PrinterDefault { get; set; }

        /// <summary>
        /// [印刷] ダイアログ ボックスの、[ページまで] ボックスの値を設定または取得します。
        /// </summary>
        public short ToPage {
            get {
                return (short)PrinterSettings.ToPage;
            }
            set {
                PrinterSettings.ToPage = value;
            }
        }

        /// <summary>
        /// 表示するダイアログボックスの種類を設定または取得します。
        /// </summary>
        public cdlActionConstans Action {
            set {
                switch (value) {
                    case cdlActionConstans.ACT_SHOWOPEN:
                        ShowOpenInternal();
                        break;
                    case cdlActionConstans.ACT_SHOWSAVE:
                        ShowSaveInternal();
                        break;
                    case cdlActionConstans.ACT_SHOWCOLOR:
                        ShowColorInternal();
                        break;
                    case cdlActionConstans.ACT_SHOWFONT:
                        ShowFontInternal();
                        break;
                    case cdlActionConstans.ACT_SHOWPRINT:
                        ShowPrintInternal();
                        break;
                    case cdlActionConstans.ACT_SHOWHELP:
                        ShowHelpInternal();
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
        }

        /// <summary>
        /// 「色の設定」ダイアログボックスを表示します。
        /// </summary>
        public void ShowColor() {
            this.Action = cdlActionConstans.ACT_SHOWCOLOR;
        }

        /// <summary>
        /// 「フォントの指定」ダイアログボックスを表示します。
        /// </summary>
        public void ShowFont() {
            this.Action = cdlActionConstans.ACT_SHOWFONT;
        }

        /// <summary>
        /// Winhlp32.exe を起動し、指定されたヘルプ ファイルを表示します。
        /// </summary>
        public void ShowHelp() {
            this.Action = cdlActionConstans.ACT_SHOWHELP;
        }

        /// <summary>
        /// 「ファイルを開く」ダイアログボックスを表示します。
        /// </summary>
        public void ShowOpen() {
            this.Action = cdlActionConstans.ACT_SHOWOPEN;
        }

        /// <summary>
        /// 「印刷」ダイアログボックスを表示します。
        /// </summary>
        public void ShowPrinter() {
            this.Action = cdlActionConstans.ACT_SHOWPRINT;
        }

        /// <summary>
        /// 「ファイル名を付けて保存」ダイアログ ボックスを表示します。
        /// </summary>
        public void ShowSave() {
            this.Action = cdlActionConstans.ACT_SHOWSAVE;
        }

        //--------------------------------------------------
        // 共通
        //--------------------------------------------------
        private IntPtr hInstance {
            get {
                return NativeMethods.GetModuleHandle(null);
            }
        }

        private bool IsFlagOn(int flag) {
            return (this.Flags & flag) == flag;
        }

        private bool ShowDialogInternal(CommonDialog dlg) {
            if (dlg.ShowDialog() == DialogResult.Cancel) {
                RaiseCancelError();
                return false;
            } else {
                return true;
            }
        }

        private void RaiseCancelError() {
            if (this.CancelError) {
                Microsoft.VisualBasic.Information.Err().Raise(CDERR_DIALOGFAILURE, "CommonDialog", "[ｷｬﾝｾﾙ] ﾎﾞﾀﾝが選択されました。");
            }
        }

        //--------------------------------------------------
        // ShowOpen/ShowSave
        //--------------------------------------------------

        private bool IsFlagOn(cdlFileOpenConstants flag) {
            return IsFlagOn((int)flag);
        }

        private void ShowOpenInternal() {
            using (var dlg = new OpenFileDialog()) {
                dlg.CheckFileExists = IsFlagOn(cdlFileOpenConstants.cdlOFNFileMustExist);
                dlg.Multiselect = IsFlagOn(cdlFileOpenConstants.cdlOFNAllowMultiselect);
                dlg.ReadOnlyChecked = IsFlagOn(cdlFileOpenConstants.cdlOFNReadOnly);
                dlg.ShowReadOnly = !IsFlagOn(cdlFileOpenConstants.cdlOFNHideReadOnly);
                if (ShowFileDialog(dlg)) {
                }
            }
        }

        private void ShowSaveInternal() {
            using (var dlg = new SaveFileDialog()) {
                dlg.CreatePrompt = IsFlagOn(cdlFileOpenConstants.cdlOFNCreatePrompt);
                dlg.OverwritePrompt = IsFlagOn(cdlFileOpenConstants.cdlOFNOverwritePrompt);
                if (ShowFileDialog(dlg)) {
                }
            }
        }

        private bool ShowFileDialog(FileDialog dlg) {
            dlg.AddExtension = IsFlagOn(cdlFileOpenConstants.cdlOFNAddExtension);
            dlg.AutoUpgradeEnabled = this.AutoUpgradeEnabled;
            dlg.CheckPathExists = IsFlagOn(cdlFileOpenConstants.cdlOFNPathMustExist);
            dlg.DefaultExt = this.DefaultExt;
            dlg.DereferenceLinks = IsFlagOn(cdlFileOpenConstants.cdlOFNNoDereferenceLinks);
            dlg.FileName = this.FileName;
            dlg.Filter = this.Filter;
            dlg.FilterIndex = this.FilterIndex;
            dlg.InitialDirectory = this.InitDir;
            dlg.RestoreDirectory = IsFlagOn(cdlFileOpenConstants.cdlOFNNoChangeDir);
            dlg.ShowHelp = IsFlagOn(cdlFileOpenConstants.cdlOFNHelpButton);
            dlg.SupportMultiDottedExtensions = IsFlagOn(cdlFileOpenConstants.cdlOFNExtensionDifferent);
            dlg.Title = this.DialogTitle;
            dlg.ValidateNames = !IsFlagOn(cdlFileOpenConstants.cdlOFNNoValidate);
            if (ShowDialogInternal(dlg)) {
                this.FileName = dlg.FileName;
                return true;
            } else {
                return false;
            }
        }

        //--------------------------------------------------
        // ShowColor
        //--------------------------------------------------
        private int[] _CustomColors = InitCustomColors();

        private static int[] InitCustomColors() {
            var tmp = new int[16];
            for (int i = 0; i < tmp.Length; i++) {
                tmp[i] = 0x00FFFFFF;
            }
            return tmp;
        }

        /// <summary>
        /// ダイアログ ボックスに表示されるカスタム カラー セットを取得または設定します。
        /// </summary>
        public int[] CustomColors {
            get {
                return (int[])_CustomColors.Clone();
            }
            set {
                int length = value == null ? 0 : Math.Min(value.Length, 16);
                if (length > 0) Array.Copy(value, 0, _CustomColors, 0, length);
                for (int i = length; i < 16; i++) _CustomColors[i] = 0x00FFFFFF;
            }
        }

        private void ShowColorInternal() {
            using (var ptr = VBHiddenModule.ObjPtr(CustomColors)) {
                var cc = new NativeMethods.CHOOSECOLOR();
                cc.hwndOwner = this.hWndOwner;
                cc.hInstance = this.hInstance;
                cc.rgbResult = ColorTranslator.ToWin32(this.Color);
                cc.lpCustColors = ptr.DangerousGetHandle();
                cc.Flags = this.Flags;
                if (NativeMethods.ChooseColor(cc)) {
                    this.Flags = cc.Flags;
                    this.Color = ColorTranslator.FromOle(cc.rgbResult);
                    Marshal.Copy(cc.lpCustColors, _CustomColors, 0, 16);
                } else {
                    RaiseCancelError();
                }
            }
        }

        //--------------------------------------------------
        // ShowFont
        //--------------------------------------------------

        private bool IsFlagOn(cdlFontsConstants flag) {
            return IsFlagOn((int)flag);
        }

        private void ShowFontInternal() {
            using (var dlg = new FontDialog()) {
                dlg.AllowScriptChange = IsFlagOn(cdlFontsConstants.cdlCFSelectScript);
                dlg.AllowSimulations = !IsFlagOn(cdlFontsConstants.cdlCFNoSimulations);
                dlg.AllowVectorFonts = !IsFlagOn(cdlFontsConstants.cdlCFNoVectorFonts);
                dlg.AllowVerticalFonts = !IsFlagOn(cdlFontsConstants.cdlCFNoVerticalFonts);
                dlg.Color = this.Color;
                dlg.FixedPitchOnly = IsFlagOn(cdlFontsConstants.cdlCFFixedPitchOnly);
                dlg.Font = this.Font;
                dlg.FontMustExist = IsFlagOn(cdlFontsConstants.cdlCFForceFontExist);
                dlg.MaxSize = this.Max;
                dlg.MinSize = this.Min;
                dlg.ScriptsOnly = IsFlagOn(cdlFontsConstants.cdlCFScriptOnly);
                dlg.ShowApply = IsFlagOn(cdlFontsConstants.cdlCFApply);
                dlg.ShowEffects = IsFlagOn(cdlFontsConstants.cdlCFEffects);
                dlg.ShowColor = dlg.ShowEffects;
                dlg.ShowHelp = IsFlagOn(cdlFontsConstants.cdlCFHelpButton);
                if (ShowDialogInternal(dlg)) {
                    this.Font = dlg.Font;
                    this.Color = dlg.Color;
                }
            }
        }

        //--------------------------------------------------
        // ShowHelp
        //--------------------------------------------------
        private void ShowHelpInternal() {

            switch (HelpCommand) {
                case cdlHelpConstants.cdlHelpCommandHelp:
                    NativeMethods.WinHelp(this.hWndOwner, this.HelpFile, (int)this.HelpCommand, this.HelpKey);
                    break;

                case cdlHelpConstants.cdlHelpContents:
                    NativeMethods.WinHelp(this.hWndOwner, this.HelpFile, (int)this.HelpCommand, 0);
                    break;

                case cdlHelpConstants.cdlHelpContext:
                    NativeMethods.WinHelp(this.hWndOwner, this.HelpFile, (int)this.HelpCommand, this.HelpContext);
                    break;

                case cdlHelpConstants.cdlHelpContextPopup:
                    NativeMethods.WinHelp(this.hWndOwner, this.HelpFile, (int)this.HelpCommand, this.HelpContext);
                    break;

                case cdlHelpConstants.cdlHelpForceFile:
                    NativeMethods.WinHelp(this.hWndOwner, this.HelpFile, (int)this.HelpCommand, 0);
                    break;

                case cdlHelpConstants.cdlHelpHelpOnHelp:
                    NativeMethods.WinHelp(this.hWndOwner, this.HelpFile, (int)this.HelpCommand, 0);
                    break;

                //case HelpConstants.cdlHelpIndex:
                case cdlHelpConstants.cdlHelpKey:
                    NativeMethods.WinHelp(this.hWndOwner, this.HelpFile, (int)this.HelpCommand, this.HelpKey);
                    break;

                case cdlHelpConstants.cdlHelpPartialKey:
                    NativeMethods.WinHelp(this.hWndOwner, this.HelpFile, (int)this.HelpCommand, this.HelpKey);
                    break;

                case cdlHelpConstants.cdlHelpQuit:
                    NativeMethods.WinHelp(this.hWndOwner, this.HelpFile, (int)this.HelpCommand, 0);
                    break;

                case cdlHelpConstants.cdlHelpSetIndex:
                    NativeMethods.WinHelp(this.hWndOwner, this.HelpFile, (int)this.HelpCommand, this.HelpContext);
                    break;
            }
        }

        //--------------------------------------------------
        // ShowPrint
        //--------------------------------------------------

        private bool IsFlagOn(cdlPrinterConstants flag) {
            return IsFlagOn((int)flag);
        }

        private PrinterSettings _PrinterSettings;

        /// <summary>
        /// Windows フォーム アプリケーションから印刷する場合のドキュメントの印刷方法に関する情報 (印刷に使用するプリンタなど) を取得します。
        /// </summary>
        public PrinterSettings PrinterSettings {
            get {
                if (_PrinterSettings == null) {
                    _PrinterSettings = new PrinterSettings();
                }
                return _PrinterSettings;
            }
        }

        private NativeMethods.PRINTDLG CreatePRINTDLG() {
            NativeMethods.PRINTDLG pd = NativeMethods.CreatePRINTDLG();
            pd.hwndOwner = this.hWndOwner;
            pd.hDevMode = this.PrinterSettings.GetHdevmode();
            pd.hDevNames = this.PrinterSettings.GetHdevnames();
            pd.Flags = this.Flags;
            pd.hDC = IntPtr.Zero;
            pd.nFromPage = this.FromPage;
            pd.nToPage = this.ToPage;
            pd.nCopies = this.Copies;
            pd.nMinPage = this.Min;
            pd.nMaxPage = this.Max;
            return pd;
        }

        private void ApplyPRINTDLG(NativeMethods.PRINTDLG pd) {

            // PrintDlg が書き換えた Flags を反映
            this.Flags = pd.Flags;

            // DevMode および DevNames を PrinterSettings に反映
            this.PrinterSettings.SetHdevmode(pd.hDevMode);
            this.PrinterSettings.SetHdevnames(pd.hDevNames);
            //this.PrinterSettings.DefaultPageSettings.SetHdevmode(pd.hDevMode);

            // Flags より反映
            this.PrinterSettings.Collate = IsFlagOn(cdlPrinterConstants.cdlPDCollate);
            this.PrinterSettings.PrintToFile = IsFlagOn(cdlPrinterConstants.cdlPDPrintToFile);

            // その他を反映
            this.FromPage = pd.nFromPage;
            this.ToPage = pd.nToPage;
            this.Copies = pd.nCopies;
            this.Min = pd.nMinPage;
            this.Max = pd.nMaxPage;
            // hdc または hIc 
            if (pd.hDC != IntPtr.Zero) {
                this.hDC = new VBSafeDCHandle(pd.hDC);
            } else {
                this.hDC = null;
            }

            // PrinterDefault = true ならデフォルトプリンタを変更
            if (this.PrinterDefault) {
                ChangeDefaultPrinter();
            }
        }

        private void ShowPrintInternal() {

            using (var pd = CreatePRINTDLG()) {
                if (NativeMethods.PrintDlg(pd)) {
                    // 正常ならば内容をプロパティに反映
                    ApplyPRINTDLG(pd);
                } else {
                    // キャンセルボタンが押された
                    RaiseCancelError();
                }
            }
        }

        static UIntPtr lpResult;

        private void ChangeDefaultPrinter() {

            var printerName = this.PrinterSettings.PrinterName;

            // デフォルトの用紙設定を変更する

            IntPtr hPrinter;
            var def = new NativeMethods.PRINTER_DEFAULTS();
            def.DesiredAccess = NativeMethods.PRINTER_ACCESS_USE;
            var nRet = NativeMethods.OpenPrinter(printerName, out hPrinter, ref def);
            if (nRet) {
                try {
                    // PRINTER_INFO_2 構造体のサイズを取得
                    int pcbNeeded;
                    NativeMethods.GetPrinter(hPrinter, 2, IntPtr.Zero, 0, out pcbNeeded);

                    // バッファを確保して PRINTER_INFO_2 を取得
                    using (var printerInfo2 = VBSafeGlobalMemory.FromSize(pcbNeeded)) {
                        IntPtr lpPrinterInfo2 = printerInfo2.Lock();

                        var b = NativeMethods.GetPrinter(hPrinter, 2, lpPrinterInfo2, pcbNeeded, out pcbNeeded);

                        // DevMode を PRINTER_INFO_2.pDevMode にセットする
                        using (var devmode = VBSafeGlobalMemory.FromHGlobal(this.PrinterSettings.GetHdevmode())) {
                            IntPtr lpDevMode = devmode.Lock();
                            unsafe {
                                ((NativeMethods.PRINTER_INFO_2*)lpPrinterInfo2)->pDevMode = lpDevMode;
                            }

                            // 更新した PRINTER_INFO_2 を適用
                            NativeMethods.SetPrinter(hPrinter, 2, lpPrinterInfo2, 0);

                            devmode.Unlock();
                        }

                        printerInfo2.Unlock();
                    }

                } finally {
                    NativeMethods.ClosePrinter(hPrinter);
                }
            }

            // デフォルトプリンタを変更
            NativeMethods.SetDefaultPrinter(printerName);

            // 全ウインドウに通知
            NativeMethods.SendMessageTimeout(
                                NativeMethods.HWND_BROADCAST,
                                NativeMethods.WM_WININICHANGE,
                                IntPtr.Zero,
                                IntPtr.Zero,
                                NativeMethods.SendMessageTimeoutFlags.SMTO_NORMAL,
                                1000,
                                out lpResult);
        }
    }
}
