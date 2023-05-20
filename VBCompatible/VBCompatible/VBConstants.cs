using System;
using System.ComponentModel;

namespace VBCompatible
{
    /// <summary>
    /// Align プロパティの定数です。
    /// </summary>
    public enum AlignConstants
    {
        /// <summary>
        /// 大きさと位置は、デザイン時、または実行時にコードで設定できます。
        /// </summary>
        vbAlignNone = 0,
        /// <summary>
        /// コントロールをフォームの上に合わせて配置します。
        /// </summary>
        vbAlignTop = 1,
        /// <summary>
        /// コントロールをフォームの下に合わせて配置します。
        /// </summary>
        vbAlignBottom = 2,
        /// <summary>
        /// コントロールをフォームの左に合わせて配置します。
        /// </summary>
        vbAlignLeft = 3,
        /// <summary>
        /// コントロールをフォームの右に合わせて配置します。
        /// </summary>
        vbAlignRight = 4,
    }
    /// <summary>
    /// Alignment プロパティの定数です。
    /// </summary>
    public enum AlignmentConstants
    {
        /// <summary>
        /// 左揃え
        /// </summary>
        vbLeftJustify = 0,
        /// <summary>
        /// 右揃え
        /// </summary>
        vbRightJustify = 1,
        /// <summary>
        /// 中央揃え
        /// </summary>
        vbCenter = 2,
    }
    /// <summary>
    ///  Appearance プロパティの定数です。
    /// </summary>
    public enum AppearanceConstants
    {
        /// <summary>
        /// オブジェクトを立体的に表示しません。
        /// </summary>
        vbAppearanceFlat = 0,
        /// <summary>
        /// オブジェクトを立体的に表示します。
        /// </summary>
        vbAppearance3D = 1,
    }
    /// <summary>
    /// アプリケーションの StartMode の定数です。
    /// </summary>
    public enum ApplicationStartConstants
    {
        /// <summary>
        /// アプリケーションを独立型のプロジェクトとして起動します。
        /// </summary>
        vbSModeStandalone = 0,
        /// <summary>
        /// アプリケーションをオートメーションサーバーとして起動します。
        /// </summary>
        vbSModeAutomation = 1,
    }
    /// <summary>
    /// AsyncRead メソッドの定数です。
    /// </summary>
    public enum AsyncReadConstants
    {
        /// <summary>
        /// AsyncReadComplete イベントが発生するまで AsyncRead メソッドは値を返しません。
        /// </summary>
        vbAsyncReadSynchronousDownload = 1,
        /// <summary>
        /// AsyncRead メソッドはローカルにキャッシュしたリソースのみを利用します。
        /// </summary>
        vbAsyncReadOfflineOperation = 8,
        /// <summary>
        /// AsyncRead キャッシュされたリソースを無視して強制的にサーバーから取得します。
        /// </summary>
        vbAsyncReadForceUpdate = 16,
        /// <summary>
        /// サーバーのバージョンが最新の場合、AsyncRead メソッドはローカルにキャッシュされたコピーのみを使用します。
        /// </summary>
        vbAsyncReadResynchronize = 512,
        /// <summary>
        /// サーバーとの接続ができない場合、AsyncRead メソッドはローカルにキャッシュされたコピーを使用します。
        /// </summary>
        vbAsyncReadGetFromCacheIfNetFail = 524288,
    }
    /// <summary>
    /// AsyncProperty オブジェクトの StatusCode プロパティで使用される定数です。
    /// </summary>
    public enum AsyncStatusCodeConstants
    {
        /// <summary>
        /// 非同期ダウンロード中にエラーが発生しました。
        /// </summary>
        vbAsyncStatusCodeError = 0,
        /// <summary>
        /// AsyncRead メソッドは、AsyncProperty.Status で指定されたダウンロード用の領域を探しています。
        /// </summary>
        vbAsyncStatusCodeFindingResource = 1,
        /// <summary>
        /// AsyncRead メソッドは、AsyncProperty.Status で指定されたダウンロード用の領域に接続しています。
        /// </summary>
        vbAsyncStatusCodeConnecting = 2,
        /// <summary>
        /// AsyncRead メソッドは、AsyncProperty.Status で指定された別の場所へリダイレクトされました。
        /// </summary>
        vbAsyncStatusCodeRedirecting = 3,
        /// <summary>
        /// AsyncRead メソッドは、AsyncProperty.Status で指定された記憶領域にデータを受け取りはじめました。
        /// </summary>
        vbAsyncStatusCodeBeginDownloadData = 4,
        /// <summary>
        /// AsyncRead メソッドは、AsyncProperty.Status で指定された記憶領域にデータを受け取っているところです。
        /// </summary>
        vbAsyncStatusCodeDownloadingData = 5,
        /// <summary>
        /// AsyncRead メソッドは、AsyncProperty.Status で指定された記憶領域へのデータの受け取りが完了しました。
        /// </summary>
        vbAsyncStatusCodeEndDownloadData = 6,
        /// <summary>
        /// AsyncRead メソッドは、要求された記憶領域をキャッシュから取得しています。AsyncProperty.Status は空です。
        /// </summary>
        vbAsyncStatusCodeUsingCachedCopy = 10,
        /// <summary>
        /// AsyncRead メソッドは、AsyncProperty.Status で指定された記憶領域を要求しています。
        /// </summary>
        vbAsyncStatusCodeSendingRequest = 11,
        /// <summary>
        /// 要求された保存領域の MIME タイプは AsyncProperty.Status で指定されます。
        /// </summary>
        vbAsyncStatusCodeMIMETypeAvailable = 13,
        /// <summary>
        /// 要求された記憶領域のためのローカルファイルキャッシュのファイル名が AsyncProperty.Status で指定されています。
        /// </summary>
        vbAsyncStatusCodeCacheFileNameAvailable = 14,
        /// <summary>
        /// AsyncRead メソッドは、同期操作を行います。
        /// </summary>
        vbAsyncStatusCodeBeginSyncOperation = 15,
        /// <summary>
        /// AsyncRead は同期命令を強制しています。
        /// </summary>
        vbAsyncStatusCodeEndSyncOperation = 16,
    }
    /// <summary>
    /// AsyncRead ﾒｿｯﾄﾞで使用される定数です。
    /// </summary>
    public enum AsyncTypeConstants
    {
        /// <summary>
        /// データは、Picture オブジェクトで提供されます。
        /// </summary>
        vbAsyncTypePicture = 0,
        /// <summary>
        /// データは、Visual Basic によって作成されるファイルで提供されます。
        /// </summary>
        vbAsyncTypeFile = 1,
        /// <summary>
        /// 取得されたデータを含むバイト配列として提供されるデータです。コントロールの作成側は、このデータの処理方法を把握している必要があります。
        /// </summary>
        vbAsyncTypeByteArray = 2,
    }
    /// <summary>
    /// BackStyle プロパティのVB6.0互換定数です。
    /// </summary>
    public enum BackStyleConstants
    {
        /// <summary>
        /// 背景を透過して描画します。
        /// </summary>
        vbBKTransparent = 0,
        /// <summary>
        /// 背景を BackColor で塗りつぶします。
        /// </summary>
        vbBKSolid = 1,
    }
    /// <summary>
    /// BorderStyle プロパティのVB6.0互換定数です。
    /// </summary>
    public enum BorderStyleConstants
    {
        /// <summary>
        /// 透過
        /// </summary>
        vbTransparent = 0,
        /// <summary>
        /// 塗りつぶし
        /// </summary>
        vbBSSolid = 1,
        /// <summary>
        /// 鎖線
        /// </summary>
        vbBSDash = 2,
        /// <summary>
        /// 点線
        /// </summary>
        vbBSDot = 3,
        /// <summary>
        /// 一点鎖線
        /// </summary>
        vbBSDashDot = 4,
        /// <summary>
        /// 二点鎖線
        /// </summary>
        vbBSDashDotDot = 5,
        /// <summary>
        /// 境界線の内側に実線が描かれる
        /// </summary>
        vbBSInsideSolid = 6,
    }
    /// <summary>
    /// コマンドボタン、チェックボックス、オプションボタンの Button 定数です。
    /// </summary>
    public enum ButtonConstants
    {
        /// <summary>
        /// 標準の Windows 形式での表示。
        /// </summary>
        vbButtonStandard = 0,
        /// <summary>
        /// グラフィックス形式での表示。
        /// </summary>
        vbButtonGraphical = 1,
    }
    /// <summary>
    /// チェックボックスの Value プロパティの定数です。
    /// </summary>
    public enum CheckBoxConstants
    {
        /// <summary>
        /// チェックボックスがオフの状態
        /// </summary>
        vbUnchecked = 0,
        /// <summary>
        /// チェックボックスがオンの状態
        /// </summary>
        vbChecked = 1,
        /// <summary>
        /// チェックボックスが無効の状態
        /// </summary>
        vbGrayed = 2,
    }
    /// <summary>
    /// クリップボードの形式の定数です。
    /// </summary>
    public enum ClipBoardConstants
    {
        /// <summary>
        /// DDE通信に関する情報です。
        /// </summary>
        vbCFLink = -16640,
        /// <summary>
        /// リッチテキストファイル形式
        /// </summary>
        vbCFRTF = -16639,
        /// <summary>
        /// テキストファイル形式
        /// </summary>
        vbCFText = 1,
        /// <summary>
        /// ビットマップファイル形式
        /// </summary>
        vbCFBitmap = 2,
        /// <summary>
        /// メタファイル形式
        /// </summary>
        vbCFMetafile = 3,
        /// <summary>
        /// デバイスに依存しないビットマップファイル形式
        /// </summary>
        vbCFDIB = 8,
        /// <summary>
        /// カラーパレット
        /// </summary>
        vbCFPalette = 9,
        /// <summary>
        /// 拡張メタファイル形式
        /// </summary>
        vbCFEMetafile = 14,
        /// <summary>
        /// ファイル名リスト
        /// </summary>
        vbCFFiles = 15,
    }
    /// <summary>
    /// Color 定数です。
    /// </summary>
    public enum ColorConstants
    {
        /// <summary>
        /// 黒
        /// </summary>
        vbBlack = 0,
        /// <summary>
        /// 赤
        /// </summary>
        vbRed = 255,
        /// <summary>
        /// 緑
        /// </summary>
        vbGreen = 65280,
        /// <summary>
        /// 黄
        /// </summary>
        vbYellow = 65535,
        /// <summary>
        /// 青
        /// </summary>
        vbBlue = 16711680,
        /// <summary>
        /// マゼンタ
        /// </summary>
        vbMagenta = 16711935,
        /// <summary>
        /// シアン
        /// </summary>
        vbCyan = 16776960,
        /// <summary>
        /// 白
        /// </summary>
        vbWhite = 16777215,
    }
    /// <summary>
    /// コンボボックスコントロールの Style プロパティの定数です。
    /// </summary>
    public enum ComboBoxConstants
    {
        /// <summary>
        /// テキストボックス部分に値を直接入力することも、ドロップダウンリストから項目を選択することもできます。
        /// </summary>
        vbComboDropdown = 0,
        /// <summary>
        /// テキストボックス部分に値を直接入力することも、（ドロップダウンリストでない）リストから項目を選択することもできます。
        /// </summary>
        vbComboSimple = 1,
        /// <summary>
        /// ドロップダウンリストから項目を選択だけができます。
        /// </summary>
        vbComboDropdownList = 2,
    }
    /// <summary>
    /// データコントロールの BOF 定数です。
    /// </summary>
    public enum DataBOFconstants
    {
        /// <summary>
        /// 先頭のレコードに移動します。
        /// </summary>
        vbMoveFirst = 0,
        /// <summary>
        /// ファイルの先頭に移動します。
        /// </summary>
        vbBOF = 1,
    }
    /// <summary>
    /// データコントロールの EOF 定数です。
    /// </summary>
    public enum DataEOFConstants
    {
        /// <summary>
        /// 最後のレコードに移動します。
        /// </summary>
        vbMoveLast = 0,
        /// <summary>
        /// ファイルの最後に移動します。
        /// </summary>
        vbEOF = 1,
        /// <summary>
        /// ファイルの末尾に新しいレコードを追加します。
        /// </summary>
        vbAddNew = 2,
    }
    /// <summary>
    /// データコントロールのエラーイベント定数です。
    /// </summary>
    public enum DataErrorConstants
    {
        /// <summary>
        /// 実行を継続します。
        /// </summary>
        vbDataErrContinue = 0,
        /// <summary>
        /// エラーメッセージを表示します。
        /// </summary>
        vbDataErrDisplay = 1,
    }
    /// <summary>
    /// データコントロールの Validate イベントの Action 定数です。
    /// </summary>
    public enum DataValidateConstants
    {
        /// <summary>
        /// 操作をキャンセルします。
        /// </summary>
        vbDataActionCancel = 0,
        /// <summary>
        /// MoveFirst メソッド
        /// </summary>
        vbDataActionMoveFirst = 1,
        /// <summary>
        /// MovePrevious メソッド
        /// </summary>
        vbDataActionMovePrevious = 2,
        /// <summary>
        /// MoveNext メソッド
        /// </summary>
        vbDataActionMoveNext = 3,
        /// <summary>
        /// MoveLast メソッド
        /// </summary>
        vbDataActionMoveLast = 4,
        /// <summary>
        /// AddNew メソッド
        /// </summary>
        vbDataActionAddNew = 5,
        /// <summary>
        /// Update メソッド
        /// </summary>
        vbDataActionUpdate = 6,
        /// <summary>
        /// Delete メソッド
        /// </summary>
        vbDataActionDelete = 7,
        /// <summary>
        /// Find メソッド
        /// </summary>
        vbDataActionFind = 8,
        /// <summary>
        /// Bookmark プロパティが設定されました。
        /// </summary>
        vbDataActionBookmark = 9,
        /// <summary>
        /// Close メソッド
        /// </summary>
        vbDataActionClose = 10,
        /// <summary>
        /// フォームがアンロードされています。
        /// </summary>
        vbDataActionUnload = 11,
    }
    /// <summary>
    /// カーソルドライバ型の定数です。
    /// </summary>
    public enum DefaultCursorTypeConstants
    {
        /// <summary>
        /// ODBC ドライバが適切なスタイルを選択します。
        /// </summary>
        vbUseDefaultCursor = 0,
        /// <summary>
        /// ODBC のカーソルライブラリを使用します。
        /// </summary>
        vbUseODBCCursor = 1,
        /// <summary>
        /// サーバーカーソルを使用します。
        /// </summary>
        vbUseServersideCursor = 2,
    }
    /// <summary>
    /// Drag メソッドの定数です。
    /// </summary>
    public enum DragConstants
    {
        /// <summary>
        /// ドラッグ操作をキャンセルします。
        /// </summary>
        vbCancel = 0,
        /// <summary>
        /// ドラッグ操作を開始します。
        /// </summary>
        vbBeginDrag = 1,
        /// <summary>
        /// ドラッグ操作を終了します。
        /// </summary>
        vbEndDrag = 2,
    }
    /// <summary>
    /// DragMode プロパティの定数です。
    /// </summary>
    public enum DragModeConstants
    {
        /// <summary>
        /// 手動ドラッグモード
        /// </summary>
        vbManual = 0,
        /// <summary>
        /// 自動ドラッグモード
        /// </summary>
        vbAutomatic = 1,
    }
    /// <summary>
    /// DragOver イベントおよび OLEDragOver イベントの状態変化の定数です。
    /// </summary>
    public enum DragOverConstants
    {
        /// <summary>
        /// ソースコントロールはターゲットの範囲内へ入ったところです。
        /// </summary>
        vbEnter = 0,
        /// <summary>
        /// ソースコントロールはターゲットの範囲外へ出るところです。
        /// </summary>
        vbLeave = 1,
        /// <summary>
        /// ソースコントロールはターゲット上を移動しています。
        /// </summary>
        vbOver = 2,
    }
    /// <summary>
    /// DrawMode プロパティの定数です。
    /// </summary>
    public enum DrawModeConstants
    {
        /// <summary>
        /// 黒いペンで出力します。
        /// </summary>
        vbBlackness = 1,
        /// <summary>
        /// 15 (Marge Pen) を反転した設定になります。
        /// </summary>
        vbNotMergePen = 2,
        /// <summary>
        /// 背景色とペンの反転色を組み合わせて出力します。
        /// </summary>
        vbMaskNotPen = 3,
        /// <summary>
        /// 13 (Copy Pen) を反転した設定になります。
        /// </summary>
        vbNotCopyPen = 4,
        /// <summary>
        /// ペンと表示の反転色に共通する色を組み合わせて出力します。
        /// </summary>
        vbMaskPenNot = 5,
        /// <summary>
        /// 表示色を反転して出力します。
        /// </summary>
        vbInvert = 6,
        /// <summary>
        /// ペンと表示色に使われている色でどちらか一方に含まれている色を組み合わせて出力します。
        /// </summary>
        vbXorPen = 7,
        /// <summary>
        /// 9 (Mask Pen) を反転した設定になります。
        /// </summary>
        vbNotMaskPen = 8,
        /// <summary>
        /// ペンと表示色に共通する色を組み合わせて出力します。
        /// </summary>
        vbMaskPen = 9,
        /// <summary>
        /// 7 (Xor Pen) を反転した設定になります。
        /// </summary>
        vbNotXorPen = 10,
        /// <summary>
        /// 色を加工せずそのまま出力します。この設定にすると、画面に描画されません。
        /// </summary>
        vbNop = 11,
        /// <summary>
        /// 表示色とペンの反転色を組み合わせて出力します。
        /// </summary>
        vbMergeNotPen = 12,
        /// <summary>
        /// (既定値) Forecolor プロパティで設定した色で出力します。
        /// </summary>
        vbCopyPen = 13,
        /// <summary>
        /// ペンの色と表示の反転色を組み合わせて出力します。
        /// </summary>
        vbMergePenNot = 14,
        /// <summary>
        /// ペンの色と表示色を組み合わせて出力します。
        /// </summary>
        vbMergePen = 15,
        /// <summary>
        /// 白いペンで出力します。
        /// </summary>
        vbWhiteness = 16,
    }
    /// <summary>
    /// DrawStyle プロパティの定数です。
    /// </summary>
    public enum DrawStyleConstants
    {
        /// <summary>
        /// 実線
        /// </summary>
        vbSolid = 0,
        /// <summary>
        /// 鎖線
        /// </summary>
        vbDash = 1,
        /// <summary>
        /// 点線
        /// </summary>
        vbDot = 2,
        /// <summary>
        /// 一点鎖線
        /// </summary>
        vbDashDot = 3,
        /// <summary>
        /// 二点鎖線
        /// </summary>
        vbDashDotDot = 4,
        /// <summary>
        /// 透明または非表示
        /// </summary>
        vbInvisible = 5,
        /// <summary>
        /// 塗りつぶし
        /// </summary>
        vbInsideSolid = 6,
    }
    /// <summary>
    /// FillStyle プロパティの定数。
    /// </summary>
    public enum FillStyleConstants
    {
        /// <summary>
        /// 塗りつぶし
        /// </summary>
        vbFSSolid = 0,
        /// <summary>
        /// 透明
        /// </summary>
        vbFSTransparent = 1,
        /// <summary>
        /// 横線
        /// </summary>
        vbHorizontalLine = 2,
        /// <summary>
        /// 縦線
        /// </summary>
        vbVerticalLine = 3,
        /// <summary>
        /// 斜線（左上から右下）
        /// </summary>
        vbUpwardDiagonal = 4,
        /// <summary>
        /// 斜線(左下から右上）
        /// </summary>
        vbDownwardDiagonal = 5,
        /// <summary>
        /// クロス
        /// </summary>
        vbCross = 6,
        /// <summary>
        /// 網掛け
        /// </summary>
        vbDiagonalCross = 7,
    }
    /// <summary>
    /// MDI フォームの Arrange メソッドの定数です。
    /// </summary>
    public enum FormArrangeConstants
    {
        /// <summary>
        /// 最小化していないすべての MDI 子フォームを重ねて表示します。
        /// </summary>
        vbCascade = 0,
        /// <summary>
        /// 最小化していないすべての MDI 子フォームを左右に並べて表示します。
        /// </summary>
        vbTileHorizontal = 1,
        /// <summary>
        /// 最小化していないすべての MDI 子フォームを上下に並べて表示します。
        /// </summary>
        vbTileVertical = 2,
        /// <summary>
        /// 最小化した MDI 子フォームのアイコンを整列します。
        /// </summary>
        vbArrangeIcons = 3,
    }
    /// <summary>
    /// BorderStyle プロパティの定数です。
    /// </summary>
    public enum FormBorderStyleConstants
    {
        /// <summary>
        /// 境界線なし
        /// </summary>
        vbBSNone = 0,
        /// <summary>
        /// 固定一重線
        /// </summary>
        vbFixedSingle = 1,
        /// <summary>
        /// 太線
        /// </summary>
        vbSizable = 2,
        /// <summary>
        /// 固定ダイアログ
        /// </summary>
        vbFixedDouble = 3,
        /// <summary>
        /// 固定ダイアログ
        /// </summary>
        vbFixedDialog = 3,
        /// <summary>
        /// 固定ツール ウィンドウ
        /// </summary>
        vbFixedToolWindow = 4,
        /// <summary>
        /// 可変ツール ウィンドウ
        /// </summary>
        vbSizableToolWindow = 5,
    }
    /// <summary>
    /// フォームの Show メソッドの定数です。
    /// </summary>
    public enum FormShowConstants
    {
        /// <summary>
        /// モードレス フォーム
        /// </summary>
        vbModeless = 0,
        /// <summary>
        /// モーダル フォーム
        /// </summary>
        vbModal = 1,
    }
    /// <summary>
    /// WindowState プロパティの定数です。
    /// </summary>
    public enum FormWindowStateConstants
    {
        /// <summary>
        /// 通常表示
        /// </summary>
        vbNormal = 0,
        /// <summary>
        /// 最小化
        /// </summary>
        vbMinimized = 1,
        /// <summary>
        /// 最大化
        /// </summary>
        vbMaximized = 2,
    }
    /// <summary>
    /// HitTest イベントの HitResult の定数です。
    /// </summary>
    public enum HitResultConstants
    {
        /// <summary>
        /// マウス ポインタがコントロールの可視領域の外側にあります。マウス メッセージが送信されます。
        /// </summary>
        vbHitResultOutside = 0,
        /// <summary>
        /// マウス ポインタがコントロールの透明領域にあります。マウス メッセージを送信できますが、実行時に設定が必要です。
        /// </summary>
        vbHitResultTransparent = 1,
        /// <summary>
        /// マウス ポインタがコントロールの可視領域の近くにあります。この隣接領域は、デザイン時に開発者が定義しておく必要があります。マウス メッセージを送信できますが、実行時に設定が必要です。
        /// </summary>
        vbHitResultClose = 2,
        /// <summary>
        /// マウス ポインタがコントロールの可視領域にあります。コントロールがマウス メッセージを受信します。
        /// </summary>
        vbHitResultHit = 3,
    }
    /// <summary>
    /// キーコード定数です。
    /// </summary>
    public enum KeyCodeConstants
    {
        /// <summary>
        /// マウスの左ボタン
        /// </summary>
        vbKeyLButton = 1,
        /// <summary>
        /// マウスの右ボタン
        /// </summary>
        vbKeyRButton = 2,
        /// <summary>
        /// Cancel キー
        /// </summary>
        vbKeyCancel = 3,
        /// <summary>
        /// マウスの中央ボタン
        /// </summary>
        vbKeyMButton = 4,
        /// <summary>
        /// BackSpace キー
        /// </summary>
        vbKeyBack = 8,
        /// <summary>
        /// Tab キー
        /// </summary>
        vbKeyTab = 9,
        /// <summary>
        /// Clear キー
        /// </summary>
        vbKeyClear = 12,
        /// <summary>
        /// Enter キー
        /// </summary>
        vbKeyReturn = 13,
        /// <summary>
        /// Shift キー
        /// </summary>
        vbKeyShift = 16,
        /// <summary>
        /// Ctrl キー
        /// </summary>
        vbKeyControl = 17,
        /// <summary>
        /// Menu キー
        /// </summary>
        vbKeyMenu = 18,
        /// <summary>
        /// Pause キー
        /// </summary>
        vbKeyPause = 19,
        /// <summary>
        /// CapsLock キー
        /// </summary>
        vbKeyCapital = 20,
        /// <summary>
        /// Esc キー
        /// </summary>
        vbKeyEscape = 27,
        /// <summary>
        /// Space キー
        /// </summary>
        vbKeySpace = 32,
        /// <summary>
        /// PageUp キー
        /// </summary>
        vbKeyPageUp = 33,
        /// <summary>
        /// PageDown キー
        /// </summary>
        vbKeyPageDown = 34,
        /// <summary>
        /// End キー
        /// </summary>
        vbKeyEnd = 35,
        /// <summary>
        /// Home キー
        /// </summary>
        vbKeyHome = 36,
        /// <summary>
        /// ← キー
        /// </summary>
        vbKeyLeft = 37,
        /// <summary>
        /// ↑ キー
        /// </summary>
        vbKeyUp = 38,
        /// <summary>
        /// → キー
        /// </summary>
        vbKeyRight = 39,
        /// <summary>
        /// ↓ キー
        /// </summary>
        vbKeyDown = 40,
        /// <summary>
        /// Select キー
        /// </summary>
        vbKeySelect = 41,
        /// <summary>
        /// PrintScreen キー
        /// </summary>
        vbKeyPrint = 42,
        /// <summary>
        /// Execute キー
        /// </summary>
        vbKeyExecute = 43,
        /// <summary>
        /// Snapshot キー
        /// </summary>
        vbKeySnapshot = 44,
        /// <summary>
        /// Ins キー
        /// </summary>
        vbKeyInsert = 45,
        /// <summary>
        /// Del キー
        /// </summary>
        vbKeyDelete = 46,
        /// <summary>
        /// Help キー
        /// </summary>
        vbKeyHelp = 47,
        /// <summary>
        /// 0 キー
        /// </summary>
        vbKey0 = 48,
        /// <summary>
        /// 1 キー
        /// </summary>
        vbKey1 = 49,
        /// <summary>
        /// 2 キー
        /// </summary>
        vbKey2 = 50,
        /// <summary>
        /// 3 キー
        /// </summary>
        vbKey3 = 51,
        /// <summary>
        /// 4 キー
        /// </summary>
        vbKey4 = 52,
        /// <summary>
        /// 5 キー
        /// </summary>
        vbKey5 = 53,
        /// <summary>
        /// 6 キー
        /// </summary>
        vbKey6 = 54,
        /// <summary>
        /// 7 キー
        /// </summary>
        vbKey7 = 55,
        /// <summary>
        /// 8 キー
        /// </summary>
        vbKey8 = 56,
        /// <summary>
        /// 9 キー
        /// </summary>
        vbKey9 = 57,
        /// <summary>
        /// A キー
        /// </summary>
        vbKeyA = 65,
        /// <summary>
        /// B キー
        /// </summary>
        vbKeyB = 66,
        /// <summary>
        /// C キー
        /// </summary>
        vbKeyC = 67,
        /// <summary>
        /// D キー
        /// </summary>
        vbKeyD = 68,
        /// <summary>
        /// E キー
        /// </summary>
        vbKeyE = 69,
        /// <summary>
        /// F キー
        /// </summary>
        vbKeyF = 70,
        /// <summary>
        /// G キー
        /// </summary>
        vbKeyG = 71,
        /// <summary>
        /// H キー
        /// </summary>
        vbKeyH = 72,
        /// <summary>
        /// I キー
        /// </summary>
        vbKeyI = 73,
        /// <summary>
        /// J キー
        /// </summary>
        vbKeyJ = 74,
        /// <summary>
        /// K キー
        /// </summary>
        vbKeyK = 75,
        /// <summary>
        /// L キー
        /// </summary>
        vbKeyL = 76,
        /// <summary>
        /// M キー
        /// </summary>
        vbKeyM = 77,
        /// <summary>
        /// N キー
        /// </summary>
        vbKeyN = 78,
        /// <summary>
        /// O キー
        /// </summary>
        vbKeyO = 79,
        /// <summary>
        /// P キー
        /// </summary>
        vbKeyP = 80,
        /// <summary>
        /// Q キー
        /// </summary>
        vbKeyQ = 81,
        /// <summary>
        /// R キー
        /// </summary>
        vbKeyR = 82,
        /// <summary>
        /// S キー
        /// </summary>
        vbKeyS = 83,
        /// <summary>
        /// T キー
        /// </summary>
        vbKeyT = 84,
        /// <summary>
        /// U キー
        /// </summary>
        vbKeyU = 85,
        /// <summary>
        /// V キー
        /// </summary>
        vbKeyV = 86,
        /// <summary>
        /// W キー
        /// </summary>
        vbKeyW = 87,
        /// <summary>
        /// X キー
        /// </summary>
        vbKeyX = 88,
        /// <summary>
        /// Y キー
        /// </summary>
        vbKeyY = 89,
        /// <summary>
        /// Z キー
        /// </summary>
        vbKeyZ = 90,
        /// <summary>
        /// テンキーの 0 キー
        /// </summary>
        vbKeyNumpad0 = 96,
        /// <summary>
        /// テンキーの 1 キー
        /// </summary>
        vbKeyNumpad1 = 97,
        /// <summary>
        /// テンキーの 2 キー
        /// </summary>
        vbKeyNumpad2 = 98,
        /// <summary>
        /// テンキーの 3 キー
        /// </summary>
        vbKeyNumpad3 = 99,
        /// <summary>
        /// テンキーの 4 キー
        /// </summary>
        vbKeyNumpad4 = 100,
        /// <summary>
        /// テンキーの 5 キー
        /// </summary>
        vbKeyNumpad5 = 101,
        /// <summary>
        /// テンキーの 6 キー
        /// </summary>
        vbKeyNumpad6 = 102,
        /// <summary>
        /// テンキーの 7 キー
        /// </summary>
        vbKeyNumpad7 = 103,
        /// <summary>
        /// テンキーの 8 キー
        /// </summary>
        vbKeyNumpad8 = 104,
        /// <summary>
        /// テンキーの 9 キー
        /// </summary>
        vbKeyNumpad9 = 105,
        /// <summary>
        /// 乗算記号 (*) キー
        /// </summary>
        vbKeyMultiply = 106,
        /// <summary>
        /// プラス記号 (+) キー
        /// </summary>
        vbKeyAdd = 107,
        /// <summary>
        /// Enter キー
        /// </summary>
        vbKeySeparator = 108,
        /// <summary>
        /// マイナス記号 (-) キー
        /// </summary>
        vbKeySubtract = 109,
        /// <summary>
        /// 小数点 (.) キー
        /// </summary>
        vbKeyDecimal = 110,
        /// <summary>
        /// 除算記号 (/) キー
        /// </summary>
        vbKeyDivide = 111,
        /// <summary>
        /// F1 キー
        /// </summary>
        vbKeyF1 = 112,
        /// <summary>
        /// F2 キー
        /// </summary>
        vbKeyF2 = 113,
        /// <summary>
        /// F3 キー
        /// </summary>
        vbKeyF3 = 114,
        /// <summary>
        /// F4 キー
        /// </summary>
        vbKeyF4 = 115,
        /// <summary>
        /// F5 キー
        /// </summary>
        vbKeyF5 = 116,
        /// <summary>
        /// F6 キー
        /// </summary>
        vbKeyF6 = 117,
        /// <summary>
        /// F7 キー
        /// </summary>
        vbKeyF7 = 118,
        /// <summary>
        /// F8 キー
        /// </summary>
        vbKeyF8 = 119,
        /// <summary>
        /// F9 キー
        /// </summary>
        vbKeyF9 = 120,
        /// <summary>
        /// F10 キー
        /// </summary>
        vbKeyF10 = 121,
        /// <summary>
        /// F11 キー
        /// </summary>
        vbKeyF11 = 122,
        /// <summary>
        /// F12 キー
        /// </summary>
        vbKeyF12 = 123,
        /// <summary>
        /// F13 キー
        /// </summary>
        vbKeyF13 = 124,
        /// <summary>
        /// F14 キー
        /// </summary>
        vbKeyF14 = 125,
        /// <summary>
        /// F15 キー
        /// </summary>
        vbKeyF15 = 126,
        /// <summary>
        /// F16 キー
        /// </summary>
        vbKeyF16 = 127,
        /// <summary>
        /// NumLock キー
        /// </summary>
        vbKeyNumlock = 144,
        /// <summary>
        /// ScrollLock キー
        /// </summary>
        vbKeyScrollLock = 145,
    }
    /// <summary>
    /// linkerr (LinkError イベント)の定数です。
    /// </summary>
    public enum LinkErrorConstants
    {
        /// <summary>
        /// ほかのアプリケーションから不正な形式のデータを要求されました。
        /// </summary>
        vbWrongFormat = 1,
        /// <summary>
        /// ソース アプリケーションが閉じた後で、デスティネーション アプリケーションが処理を続行しようとしました。
        /// </summary>
        vbDDESourceClosed = 6,
        /// <summary>
        /// すべてのソース リンクが使われています。
        /// </summary>
        vbTooManyLinks = 7,
        /// <summary>
        /// デスティネーション アプリケーションがデータを更新できません。
        /// </summary>
        vbDataTransferFailed = 8,
    }
    /// <summary>
    /// LinkMode プロパティの定数です。
    /// </summary>
    public enum LinkModeConstants
    {
        /// <summary>
        /// なし
        /// </summary>
        vbLinkNone = 0,
        /// <summary>
        /// DDE 通信のデータソースです。
        /// </summary>
        vbLinkSource = 1,
        /// <summary>
        /// 自動 (コントロールのみ)
        /// </summary>
        vbLinkAutomatic = 1,
        /// <summary>
        /// 手動 (コントロールのみ)
        /// </summary>
        vbLinkManual = 2,
        /// <summary>
        /// 通知 (コントロールのみ)
        /// </summary>
        vbLinkNotify = 3,
    }
    /// <summary>
    /// リスト ボックスコントロールの Style プロパティの定数です。
    /// </summary>
    public enum ListBoxConstants
    {
        /// <summary>
        /// 標準 Windows ListBox の外観
        /// </summary>
        vbListBoxStandard = 0,
        /// <summary>
        /// 選択可能な CheckBox 付きの項目を表示します。
        /// </summary>
        vbListBoxCheckbox = 1,
    }
    /// <summary>
    /// LoadPicture 関数の ColorDepth 引数に使用される定数です。
    /// </summary>
    public enum LoadPictureColorConstants
    {
        /// <summary>
        /// 指定ファイルを使用する際に用いられる最も近い値です。
        /// </summary>
        vbLPDefault = 0,
        /// <summary>
        /// 2 色。
        /// </summary>
        vbLPMonochrome = 1,
        /// <summary>
        /// 16 色。
        /// </summary>
        vbLPVGAColor = 2,
        /// <summary>
        /// 256 色。
        /// </summary>
        vbLPColor = 3,
    }
    /// <summary>
    /// LoadPicture 関数の Size 引数に使用される定数です。
    /// </summary>
    public enum LoadPictureSizeConstants
    {
        /// <summary>
        /// システムのスモール アイコンです。
        /// </summary>
        vbLPSmall = 0,
        /// <summary>
        /// システムのラージ アイコンのサイズです。ビデオ ドライバによって決まります。
        /// </summary>
        vbLPLarge = 1,
        /// <summary>
        /// シェルのスモール アイコンのサイズです。コントロール パネルの [画面のプロパティ] ダイアログ ボックスにある[デザイン] タブのキャプション ボタンの設定によって決まります。
        /// </summary>
        vbLPSmallShell = 2,
        /// <summary>
        /// シェルのラージ アイコンのサイズです。コントロール パネルの [画面のプロパティ] ダイアログ ボックスにある [デザイン] タブのアイコン サイズの設定によって決まります。
        /// </summary>
        vbLPLargeShell = 3,
        /// <summary>
        /// カスタム サイズです。引数 x と y で指定する値です。
        /// </summary>
        vbLPCustom = 4,
    }
    /// <summary>
    /// LoadResPicture メソッドで使用される定数です。
    /// </summary>
    public enum LoadResConstants
    {
        /// <summary>
        /// ビットマップ リソース
        /// </summary>
        vbResBitmap = 0,
        /// <summary>
        /// アイコン リソース
        /// </summary>
        vbResIcon = 1,
        /// <summary>
        /// カーソル リソース
        /// </summary>
        vbResCursor = 2,
    }
    /// <summary>
    /// LogEvent メソッドで使用される定数です。
    /// </summary>
    public enum LogEventTypeConstants
    {
        /// <summary>
        /// エラー イベントを記録します。
        /// </summary>
        vbLogEventTypeError = 1,
        /// <summary>
        /// 警告イベントを記録します。
        /// </summary>
        vbLogEventTypeWarning = 2,
        /// <summary>
        /// 情報イベントを記録します。
        /// </summary>
        vbLogEventTypeInformation = 4,
    }
    /// <summary>
    /// App オブジェクトの LogMode プロパティの定数です。
    /// </summary>
    public enum LogModeConstants
    {
        /// <summary>
        /// Windows 95 の場合は、LogPath プロパティで指定されたファイルにメッセージのログを記録します。
        /// Windows NT の場合は、Windows NT Application Event Log にメッセージのログを記録します。
        /// この場合、出力したアプリケーションとして "VBRunTime" が使われ、App.Title が記録されます。
        /// </summary>
        vbLogAuto = 0,
        /// <summary>
        /// すべてのログの記録を無効にします。
        /// UI から発生したメッセージも、LogEvent メソッドのメッセージと同様に破棄されます。
        /// </summary>
        vbLogOff = 1,
        /// <summary>
        /// 強制的にログをファイルに出力させます。
        /// LogPath に有効なファイル名が指定されていない場合、ログは記録されません。また、このプロパティの設定は vbLogOff に変更されます。
        /// </summary>
        vbLogToFile = 2,
        /// <summary>
        /// 強制的にログを NT イベント ログに出力させます。
        /// Windows NT 以外で実行している場合、またはイベント ログが使用不可の場合は、ログは記録されません。
        /// また、このプロパティの設定は vbLogOff に変更されます。
        /// </summary>
        vbLogToNT = 3,
        /// <summary>
        /// アプリケーションを起動するたびに、ログファイルを作成することを指定します。
        /// この値は、OR 演算子を使ってほかのモード オプションと組み合わせることができます。
        /// 特に指定しない限り、ログは既存のファイルに追加されます。
        /// NT のイベント ログでは、この指定は意味を持ちません。
        /// </summary>
        vbLogOverwrite = 16,
        /// <summary>
        /// 現在のスレッド ID を "[T:0nnn]" の形式でメッセージの先頭に挿入します。
        /// この値は、OR 演算子を使ってほかのモード オプションと組み合わせることができます。
        /// 特に指定しない限り、スレッド ID はアプリケーションがマルチスレッド化されているときだけ出力されます。
        /// つまり、明示的にスレッドセーフが指定されている場合、または instancing プロパティに Single-Use および multithreaded が指定されたローカル サーバーなどの暗黙的なマルチスレッド アプリケーションの場合です。
        /// </summary>
        vbLogThreadID = 32,
    }
    /// <summary>
    /// メニューのアクセス キー定数です。
    /// </summary>
    public enum MenuAccelConstants
    {
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + A )
        /// </summary>
        vbMenuAccelCtrlA = 1,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + B )
        /// </summary>
        vbMenuAccelCtrlB = 2,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + C )
        /// </summary>
        vbMenuAccelCtrlC = 3,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + D )
        /// </summary>
        vbMenuAccelCtrlD = 4,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + E )
        /// </summary>
        vbMenuAccelCtrlE = 5,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + F )
        /// </summary>
        vbMenuAccelCtrlF = 6,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + G )
        /// </summary>
        vbMenuAccelCtrlG = 7,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + H )
        /// </summary>
        vbMenuAccelCtrlH = 8,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + I )
        /// </summary>
        vbMenuAccelCtrlI = 9,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + J )
        /// </summary>
        vbMenuAccelCtrlJ = 10,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + K )
        /// </summary>
        vbMenuAccelCtrlK = 11,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + L )
        /// </summary>
        vbMenuAccelCtrlL = 12,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + M )
        /// </summary>
        vbMenuAccelCtrlM = 13,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + N )
        /// </summary>
        vbMenuAccelCtrlN = 14,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + O )
        /// </summary>
        vbMenuAccelCtrlO = 15,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + P )
        /// </summary>
        vbMenuAccelCtrlP = 16,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + Q )
        /// </summary>
        vbMenuAccelCtrlQ = 17,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + R )
        /// </summary>
        vbMenuAccelCtrlR = 18,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + S )
        /// </summary>
        vbMenuAccelCtrlS = 19,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + T )
        /// </summary>
        vbMenuAccelCtrlT = 20,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + U )
        /// </summary>
        vbMenuAccelCtrlU = 21,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + V )
        /// </summary>
        vbMenuAccelCtrlV = 22,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + W )
        /// </summary>
        vbMenuAccelCtrlW = 23,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + X )
        /// </summary>
        vbMenuAccelCtrlX = 24,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + Y )
        /// </summary>
        vbMenuAccelCtrlY = 25,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + Z )
        /// </summary>
        vbMenuAccelCtrlZ = 26,
        /// <summary>
        /// ユーザー定義のショートカット キー(F1)
        /// </summary>
        vbMenuAccelF1 = 27,
        /// <summary>
        /// ユーザー定義のショートカット キー(F2)
        /// </summary>
        vbMenuAccelF2 = 28,
        /// <summary>
        /// ユーザー定義のショートカット キー(F3)
        /// </summary>
        vbMenuAccelF3 = 29,
        /// <summary>
        /// ユーザー定義のショートカット キー(F4)
        /// </summary>
        vbMenuAccelF4 = 30,
        /// <summary>
        /// ユーザー定義のショートカット キー(F5)
        /// </summary>
        vbMenuAccelF5 = 31,
        /// <summary>
        /// ユーザー定義のショートカット キー(F6)
        /// </summary>
        vbMenuAccelF6 = 32,
        /// <summary>
        /// ユーザー定義のショートカット キー(F7)
        /// </summary>
        vbMenuAccelF7 = 33,
        /// <summary>
        /// ユーザー定義のショートカット キー(F8)
        /// </summary>
        vbMenuAccelF8 = 34,
        /// <summary>
        /// ユーザー定義のショートカット キー(F9)
        /// </summary>
        vbMenuAccelF9 = 35,
        /// <summary>
        /// ユーザー定義のショートカット キー(F11)
        /// </summary>
        vbMenuAccelF11 = 36,
        /// <summary>
        /// ユーザー定義のショートカット キー(F12)
        /// </summary>
        vbMenuAccelF12 = 37,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + F1)
        /// </summary>
        vbMenuAccelCtrlF1 = 38,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + F2)
        /// </summary>
        vbMenuAccelCtrlF2 = 39,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + F3)
        /// </summary>
        vbMenuAccelCtrlF3 = 40,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + F4)
        /// </summary>
        vbMenuAccelCtrlF4 = 41,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + F5)
        /// </summary>
        vbMenuAccelCtrlF5 = 42,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + F6)
        /// </summary>
        vbMenuAccelCtrlF6 = 43,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + F7)
        /// </summary>
        vbMenuAccelCtrlF7 = 44,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + F8)
        /// </summary>
        vbMenuAccelCtrlF8 = 45,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + F9)
        /// </summary>
        vbMenuAccelCtrlF9 = 46,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + F11)
        /// </summary>
        vbMenuAccelCtrlF11 = 47,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + F12)
        /// </summary>
        vbMenuAccelCtrlF12 = 48,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F1)
        /// </summary>
        vbMenuAccelShiftF1 = 49,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F2)
        /// </summary>
        vbMenuAccelShfitF2 = 50,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F3)
        /// </summary>
        vbMenuAccelShiftF3 = 51,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F4)
        /// </summary>
        vbMenuAccelShiftF4 = 52,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F5)
        /// </summary>
        vbMenuAccelShiftF5 = 53,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F6)
        /// </summary>
        vbMenuAccelShiftF6 = 54,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F7)
        /// </summary>
        vbMenuAccelShiftF7 = 55,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F8)
        /// </summary>
        vbMenuAccelShiftF8 = 56,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F9)
        /// </summary>
        vbMenuAccelShiftF9 = 57,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F11)
        /// </summary>
        vbMenuAccelShiftF11 = 58,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F12)
        /// </summary>
        vbMenuAccelShiftF12 = 59,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F1)
        /// </summary>
        vbMenuAccelShiftCtrlF1 = 60,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F2)
        /// </summary>
        vbMenuAccelShiftCtrlF2 = 61,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F3)
        /// </summary>
        vbMenuAccelShiftCtrlF3 = 62,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F4)
        /// </summary>
        vbMenuAccelShiftCtrlF4 = 63,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F5)
        /// </summary>
        vbMenuAccelShiftCtrlF5 = 64,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F6)
        /// </summary>
        vbMenuAccelShiftCtrlF6 = 65,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F7)
        /// </summary>
        vbMenuAccelShiftCtrlF7 = 66,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F8)
        /// </summary>
        vbMenuAccelShiftCtrlF8 = 67,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F9)
        /// </summary>
        vbMenuAccelShiftCtrlF9 = 68,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F11)
        /// </summary>
        vbMenuAccelShiftCtrlF11 = 69,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + F12)
        /// </summary>
        vbMenuAccelShiftCtrlF12 = 70,
        /// <summary>
        /// ユーザー定義のショートカット キー(Ctrl + Ins)
        /// </summary>
        vbMenuAccelCtrlIns = 71,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + Ins)
        /// </summary>
        vbMenuAccelShiftIns = 72,
        /// <summary>
        /// ユーザー定義のショートカット キー(Del)
        /// </summary>
        vbMenuAccelDel = 73,
        /// <summary>
        /// ユーザー定義のショートカット キー(Shift + Del)
        /// </summary>
        vbMenuAccelShiftDel = 74,
        /// <summary>
        /// ユーザー定義のショートカット キー(Alt + Del)
        /// </summary>
        vbMenuAccelAltBksp = 75,
    }
    /// <summary>
    /// PopupMenu メソッドの配置揃え
    /// </summary>
    public enum MenuControlConstants
    {
        /// <summary>
        /// ショートカット メニューを左揃えにします。
        /// </summary>
        vbPopupMenuLeftAlign = 0,
        /// <summary>
        /// ショートカット メニューを中央揃えにします。
        /// </summary>
        vbPopupMenuCenterAlign = 4,
        /// <summary>
        /// ショートカット メニューを右揃えにします。
        /// </summary>
        vbPopupMenuRightAlign = 8,
        /// <summary>
        /// ショートカット メニューは、マウスの左ボタンだけを認識します。
        /// </summary>
        vbPopupMenuLeftButton = 0,
        /// <summary>
        /// ショートカット メニューは、マウスの左ボタンと右ボタンを認識します。
        /// </summary>
        vbPopupMenuRightButton = 2,
    }
    /// <summary>
    /// マウス ボタン パラメータ マスク
    /// </summary>
    public enum MouseButtonConstants
    {
        /// <summary>
        /// 左マウス ボタン
        /// </summary>
        vbLeftButton = 1,
        /// <summary>
        /// 右マウス ボタン
        /// </summary>
        vbRightButton = 2,
        /// <summary>
        /// マウスの中央のボタン
        /// </summary>
        vbMiddleButton = 4,
    }
    /// <summary>
    /// マウス ポインタ定数
    /// </summary>
    public enum MousePointerConstants
    {
        /// <summary>
        /// (既定値) 形状はコントロールによって決定されます。
        /// </summary>
        vbDefault = 0,
        /// <summary>
        /// 矢印。
        /// </summary>
        vbArrow = 1,
        /// <summary>
        /// クロス。十文字のポインタです。
        /// </summary>
        vbCrosshair = 2,
        /// <summary>
        /// I 型。
        /// </summary>
        vbIbeam = 3,
        /// <summary>
        /// 四角形。
        /// </summary>
        vbIconPointer = 4,
        /// <summary>
        /// サイズ変更 (上下左右)。
        /// </summary>
        vbSizePointer = 5,
        /// <summary>
        /// サイズ変更 (右上 - 左下)。
        /// </summary>
        vbSizeNESW = 6,
        /// <summary>
        /// サイズ変更 (上下)。
        /// </summary>
        vbSizeNS = 7,
        /// <summary>
        /// サイズ変更 (左上 - 右下)。
        /// </summary>
        vbSizeNWSE = 8,
        /// <summary>
        /// サイズ変更 (左右)。
        /// </summary>
        vbSizeWE = 9,
        /// <summary>
        /// 上矢印。
        /// </summary>
        vbUpArrow = 10,
        /// <summary>
        /// 砂時計。
        /// </summary>
        vbHourglass = 11,
        /// <summary>
        /// 禁止。
        /// </summary>
        vbNoDrop = 12,
        /// <summary>
        /// 矢印と砂時計
        /// </summary>
        vbArrowHourglass = 13,
        /// <summary>
        /// 矢印と疑問符。
        /// </summary>
        vbArrowQuestion = 14,
        /// <summary>
        /// サイズ (すべて)。
        /// </summary>
        vbSizeAll = 15,
        /// <summary>
        /// MouseIcon プロパティで設定したユーザー定義のアイコン。
        /// </summary>
        vbCustom = 99,
    }
    /// <summary>
    /// リストコントロールの MultiSelect　プロパティの定数です。
    /// </summary>
    public enum MultiSelectConstants
    {
        /// <summary>
        /// 複数選択なし
        /// </summary>
        vbMultiSelectNone = 0,
        /// <summary>
        /// 標準複数選択
        /// </summary>
        vbMultiSelectSimple = 1,
        /// <summary>
        /// 拡張複数選択
        /// </summary>
        vbMultiSelectExtended = 2,
    }
    /// <summary>
    /// LinkMode プロパティの以前の定数値です。(VB1.0との互換用)
    /// </summary>
    public enum OldLinkModeConstants
    {
        /// <summary>
        /// LinkMode は Hot です
        /// </summary>
        vbHot = 1,
        /// <summary>
        /// LinkMode は サーバーです
        /// </summary>
        vbServer = 1,
        /// <summary>
        /// LinkMode は Cold です
        /// </summary>
        vbCold = 2,
    }
    /// <summary>
    /// OLE コンテナ (OLE) コントロールの定数です。
    /// </summary>
    public enum OLEContainerConstants
    {
        /// <summary>
        /// オブジェクト アプリケーションで元に戻すことができる、すべてのレコードの変更を無効にします。
        /// </summary>
        vbOLEDiscardUndoState = -6,
        /// <summary>
        /// オブジェクト内をクリックして、オブジェクトで作業できます。
        /// </summary>
        vbOLEInPlaceActivate = -5,
        /// <summary>
        /// オブジェクトに関連付けられているすべてのユーザー インターフェイスを表示し、使うことができるようにします。
        /// </summary>
        vbOLEUIActivate = -4,
        /// <summary>
        /// 埋め込みオブジェクトのために、その埋め込みオブジェクトを作成したアプリケーションを表示しません。
        /// </summary>
        vbOLEHide = -3,
        /// <summary>
        /// 別のアプリケーション ウィンドウでオブジェクトを開きます。
        /// </summary>
        vbOLEOpen = -2,
        /// <summary>
        /// 編集するためにオブジェクトをアクティブ化します。
        /// </summary>
        vbOLEShow = -1,
        /// <summary>
        /// OLE オブジェクトは自動的にアクティブ化されません。
        /// </summary>
        vbOLEActivateManual = 0,
        /// <summary>
        /// オブジェクトのデータを OLE コンテナ コントロール内に表示します。
        /// </summary>
        vbOLEDisplayContent = 0,
        /// <summary>
        /// OLE コンテナ コントロールの境界線で、オブジェクト イメージがクリップされます。
        /// </summary>
        vbOLESizeClip = 0,
        /// <summary>
        /// そのオブジェクトの既定値のアクションです。
        /// </summary>
        vbOLEPrimary = 0,
        /// <summary>
        /// オブジェクトのデータが変更されました。
        /// </summary>
        vbOLEChanged = 0,
        /// <summary>
        /// リンクされたデータを変更すると、オブジェクトは更新されます。
        /// </summary>
        vbOLEAutomatic = 0,
        /// <summary>
        /// OLE コンテナ コントロールにリンクされたオブジェクトが含まれています。
        /// </summary>
        vbOLELinked = 0,
        /// <summary>
        /// オブジェクトのロード中に、そのオブジェクトを保管するメモリが、コントロールに使われます。
        /// </summary>
        vbOLEMiscFlagMemStorage = 1,
        /// <summary>
        /// 淡色表示したメニュー項目。
        /// </summary>
        vbOLEFlagGrayed = 1,
        /// <summary>
        /// OLE コンテナ コントロールに埋め込みオブジェクトが含まれています。
        /// </summary>
        vbOLEEmbedded = 1,
        /// <summary>
        /// リンクされたドキュメントを、そのドキュメントを作成したアプリケーション内から保存すると、オブジェクトが更新されます。
        /// </summary>
        vbOLEFrozen = 1,
        /// <summary>
        /// OLE コンテナ コントロールがフォーカスを持つと、オブジェクトがアクティブ化されます。
        /// </summary>
        vbOLEActivateGetFocus = 1,
        /// <summary>
        /// オブジェクトのアイコンを OLE コンテナ コントロール内に表示します。
        /// </summary>
        vbOLEDisplayIcon = 1,
        /// <summary>
        /// オブジェクト イメージを OLE コンテナ コントロールと同じサイズにします。
        /// </summary>
        vbOLESizeStretch = 1,
        /// <summary>
        /// オブジェクトを作成したアプリケーションで、そのオブジェクトのデータが保存されました。
        /// </summary>
        vbOLESaved = 1,
        /// <summary>
        /// 使用不可能なメニュー項目。
        /// </summary>
        vbOLEFlagDisabled = 2,
        /// <summary>
        /// OLE コンテナ コントロールをダブルクリックすると、オブジェクトがアクティブ化されます。
        /// </summary>
        vbOLEActivateDoubleclick = 2,
        /// <summary>
        /// オブジェクト全体を表示できるように、OLE コンテナ コントロールのサイズを自動的に変更します。
        /// </summary>
        vbOLESizeAutoSize = 2,
        /// <summary>
        /// OLE コンテナ コントロールは、リンクされたオブジェクトまたは埋め込みオブジェクトを含むことができます。
        /// </summary>
        vbOLEEither = 2,
        /// <summary>
        /// Action プロパティの値が 6 (更新) に設定されているときにのみ、オブジェクトは更新されます。
        /// </summary>
        vbOLEManual = 2,
        /// <summary>
        /// OLE コンテナ コントロールに別のウィンドウのオブジェクトをアクティブ化します。
        /// </summary>
        vbOLEMiscFlagDisableInPlace = 2,
        /// <summary>
        /// リンクしたオブジェクトのデータが入っているアプリケーション ファイルが閉じられました。
        /// </summary>
        vbOLEClosed = 2,
        /// <summary>
        /// オブジェクト イメージを均等に伸縮します。
        /// </summary>
        vbOLESizeZoom = 3,
        /// <summary>
        /// OLE コンテナ コントロールにオブジェクトは含まれていません。
        /// </summary>
        vbOLENone = 3,
        /// <summary>
        /// リンクしたオブジェクトのデータが入っているアプリケーション ファイルの名前が変更されました。
        /// </summary>
        vbOLERenamed = 3,
        /// <summary>
        /// そのオブジェクトの既定値のアクティブ化方式に基づいて、オブジェクトがアクティブ化されます。
        /// </summary>
        vbOLEActivateAuto = 3,
        /// <summary>
        /// チェックしたメニュー項目。
        /// </summary>
        vbOLEFlagChecked = 8,
        /// <summary>
        /// メニュー項目リストの区分線。
        /// </summary>
        vbOLEFlagSeparator = 2048,
    }
    /// <summary>
    /// OLEDragMode プロパティの定数です。
    /// </summary>
    public enum OLEDragConstants
    {
        /// <summary>
        /// (既定値) 手動。プログラマがすべての OLE ドラッグ アンド ドロップ操作を処理します。
        /// </summary>
        vbOLEDragManual = 0,
        /// <summary>
        /// 自動。コンポーネントですべての OLE ドラッグ アンド ドロップ操作を処理します。
        /// </summary>
        vbOLEDragAutomatic = 1,
    }
    /// <summary>
    /// OLEDropMode プロパティの定数です。
    /// </summary>
    public enum OLEDropConstants
    {
        /// <summary>
        /// (既定値) なし。ターゲット コンポーネントは OLE ドロップを受け付けず、ドロップできないカーソルを表示します。
        /// </summary>
        vbOLEDropNone = 0,
        /// <summary>
        /// 手動。ターゲット コンポーネントは OLE ドロップ イベントを発生させます。
        /// プログラマは、プログラム内ですべての OLE ドロップ操作を処理できます。
        /// </summary>
        vbOLEDropManual = 1,
        /// <summary>
        /// 自動。ターゲット コンポーネントでは、DataObject オブジェクトに認識可能な形式のデータが入っている場合は、自動的に OLE ドロップを受け付けます。
        /// OLEDropMode プロパティが vbOLEDropAutomatic に設定されている場合、ターゲットではマウス イベントおよび OLE ドラッグ アンド ドロップ イベントは発生しません。
        /// </summary>
        vbOLEDropAutomatic = 2,
    }
    /// <summary>
    /// OLEDropMode プロパティの定数です。
    /// </summary>
    public enum OLEDropEffectConstants
    {
        /// <summary>
        /// ドロップターゲットのウインドウがスクロールした状態になります。
        /// </summary>
        vbDropEffectScroll = -2147483648,
        /// <summary>
        /// (既定値) なし。ターゲット コンポーネントは OLE ドロップを受け付けず、ドロップできないカーソルを表示します。
        /// </summary>
        vbDropEffectNone = 0,
        /// <summary>
        /// 手動。ターゲット コンポーネントは OLE ドロップ イベントを発生させます。プログラマは、プログラム内ですべての OLE ドロップ操作を処理できます。
        /// </summary>
        vbDropEffectCopy = 1,
        /// <summary>
        /// 自動。ターゲット コンポーネントでは、DataObject オブジェクトに認識可能な形式のデータが入っている場合は、自動的に OLE ドロップを受け付けます。
        /// OLEDropMode プロパティが vbOLEDropAutomatic に設定されている場合、ターゲットではマウス イベントおよび OLE ドラッグ アンド ドロップ イベントは発生しません。
        /// </summary>
        vbDropEffectMove = 2,
    }
    /// <summary>
    /// PaletteMode プロパティの定数です。
    /// </summary>
    public enum PaletteModeConstants
    {
        /// <summary>
        /// 既定の設定です。ハーフトーン パレットを使います。
        /// </summary>
        vbPaletteModeHalftone = 0,
        /// <summary>
        /// パレットを持つ最も手前に表示されているフォームと同じパレットを使います。
        /// </summary>
        vbPaletteModeUseZOrder = 1,
        /// <summary>
        /// Palette プロパティで指定されたパレットを使います。
        /// </summary>
        vbPaletteModeCustom = 2,
        /// <summary>
        /// アンビエント Palette プロパティがサポートされているコンテナのパレットを使います。この設定値は、ユーザー コントロールの場合に有効です。
        /// </summary>
        vbPaletteModeContainer = 3,
        /// <summary>
        /// パレットを使いません。この設定値は、ユーザー コントロールに場合に有効です。
        /// </summary>
        vbPaletteModeNone = 4,
        /// <summary>
        /// ActiveX デザイナのパレットを使います。この設定値は、パレットを持つ ActiveX デザイナの場合に有効です。
        /// </summary>
        vbPaletteModeObject = 5,
    }
    /// <summary>
    /// ParentControls プロパティの定数です。
    /// </summary>
    public enum ParentControlsType
    {
        /// <summary>
        /// ParentControls コレクションはコントロールそのものを返します。
        /// </summary>
        vbNoExtender = 0,
        /// <summary>
        /// ParentControls コレクションはコントロールのエクステンダを返します。
        /// </summary>
        vbExtender = 1,
    }
    /// <summary>
    /// Picture オブジェクトの定数です。
    /// </summary>
    public enum PictureTypeConstants
    {
        /// <summary>
        /// なし
        /// </summary>
        vbPicTypeNone = 0,
        /// <summary>
        /// ビットマップ タイプの Picture オブジェクト
        /// </summary>
        vbPicTypeBitmap = 1,
        /// <summary>
        /// メタファイル タイプの Picture オブジェクト
        /// </summary>
        vbPicTypeMetafile = 2,
        /// <summary>
        /// アイコン タイプの Picture オブジェクト
        /// </summary>
        vbPicTypeIcon = 3,
        /// <summary>
        /// 拡張メタファイル タイプの Picture オブジェクト
        /// </summary>
        vbPicTypeEMetafile = 4,
    }

    /// <summary>
    /// 用紙の向き
    /// </summary>
    public enum PrinterOrientationConstants
    {
        /// <summary>
        /// プリンタのデフォルトの用紙方向
        /// </summary>
        vbPRORDefault = 0,
        /// <summary>
        /// 用紙の短い辺を上にして印刷します。
        /// </summary>
        vbPRORPortrait = 1,
        /// <summary>
        /// 用紙の長い辺を上にして印刷します。
        /// </summary>
        vbPRORLandscape = 2,
    }

    /// <summary>
    /// Printer オブジェクトの定数
    /// </summary>
    public enum PrinterObjectConstants
    {
        //=============================================
        //プリンタのカラー モード
        //=============================================
        /// <summary>
        /// モノクロ出力
        /// </summary>
        vbPRCMMonochrome = 1,
        /// <summary>
        /// カラー出力
        /// </summary>
        vbPRCMColor = 2,

        //=============================================
        // 両面印刷
        //=============================================
        /// <summary>
        /// 片面印刷
        /// </summary>
        vbPRDPSimplex = 1,
        /// <summary>
        /// 両面水平印刷
        /// </summary>
        vbPRDPHorizontal = 2,
        /// <summary>
        /// 両面垂直印刷
        /// </summary>
        vbPRDPVertical = 3,

        //=============================================
        // 印字の品質
        //=============================================
        /// <summary>
        /// 高品位
        /// </summary>
        vbPRPQHigh = -4,
        /// <summary>
        /// 中品位
        /// </summary>
        vbPRPQMedium = -3,
        /// <summary>
        /// 低品位
        /// </summary>
        vbPRPQLow = -2,
        /// <summary>
        /// 簡易印刷
        /// </summary>
        vbPRPQDraft = -1,

        //=============================================
        // PaperBin プロパティ
        //=============================================
        /// <summary>
        /// 上部トレイの用紙を使います。
        /// </summary>
        vbPRBNUpper = 1,
        /// <summary>
        /// 下部トレイの用紙を使います。
        /// </summary>
        vbPRBNLower = 2,
        /// <summary>
        /// 中央トレイの用紙を使います。
        /// </summary>
        vbPRBNMiddle = 3,
        /// <summary>
        /// 用紙を 1 枚ずつ手差しにします。
        /// </summary>
        vbPRBNManual = 4,
        /// <summary>
        /// 封筒フィーダの封筒を使います。
        /// </summary>
        vbPRBNEnvelope = 5,
        /// <summary>
        /// 封筒フィーダの封筒を使いますが、手差しにも対応します。
        /// </summary>
        vbPRBNEnvManual = 6,
        /// <summary>
        /// (既定値) 通常使うトレイの用紙を使います。
        /// </summary>
        vbPRBNAuto = 7,
        /// <summary>
        /// トラクタ フィーダの用紙を使います。
        /// </summary>
        vbPRBNTractor = 8,
        /// <summary>
        /// 小型フィーダの用紙を使います。
        /// </summary>
        vbPRBNSmallFmt = 9,
        /// <summary>
        /// 大型トレイの用紙を使います。
        /// </summary>
        vbPRBNLargeFmt = 10,
        /// <summary>
        /// 大容量フィーダの用紙を使います。
        /// </summary>
        vbPRBNLargeCapacity = 11,
        /// <summary>
        /// 付属のカセット カートリッジの用紙を使います。
        /// </summary>
        vbPRBNCassette = 14,

        //=============================================
        // PaperSize プロパティ
        //=============================================
        /// <summary>
        /// レター、8 1/2 x 11 インチ
        /// </summary>
        vbPRPSLetter = 1,
        /// <summary>
        /// +A611 レター スモール、8 1/2 x 11 インチ
        /// </summary>
        vbPRPSLetterSmall = 2,
        /// <summary>
        /// タブロイド、11 x 17 インチ
        /// </summary>
        vbPRPSTabloid = 3,
        /// <summary>
        /// レジャー、17 x 11 インチ
        /// </summary>
        vbPRPSLedger = 4,
        /// <summary>
        /// リーガル、8 1/2 x 14 インチ
        /// </summary>
        vbPRPSLegal = 5,
        /// <summary>
        /// ステートメント、5 1/2 x 8 1/2 インチ
        /// </summary>
        vbPRPSStatement = 6,
        /// <summary>
        /// エグゼクティブ、7 1/2 x 10 1/2 インチ
        /// </summary>
        vbPRPSExecutive = 7,
        /// <summary>
        /// A3、297 x 420 mm
        /// </summary>
        vbPRPSA3 = 8,
        /// <summary>
        /// A4、210 x 297 mm
        /// </summary>
        vbPRPSA4 = 9,
        /// <summary>
        /// A4 Small、210 x 297 mm
        /// </summary>
        vbPRPSA4Small = 10,
        /// <summary>
        /// A5、148 x 210 mm
        /// </summary>
        vbPRPSA5 = 11,
        /// <summary>
        /// B4、250 x 354 mm
        /// </summary>
        vbPRPSB4 = 12,
        /// <summary>
        /// B5、182 x 257 mm
        /// </summary>
        vbPRPSB5 = 13,
        /// <summary>
        /// フォリオ、8 1/2 x 13 インチ
        /// </summary>
        vbPRPSFolio = 14,
        /// <summary>
        /// クォート、215 x 275 mm
        /// </summary>
        vbPRPSQuarto = 15,
        /// <summary>
        /// 10 x 14 インチ
        /// </summary>
        vbPRPS10x14 = 16,
        /// <summary>
        /// 11 x 17 インチ
        /// </summary>
        vbPRPS11x17 = 17,
        /// <summary>
        /// ノート、8 1/2 x 11 インチ
        /// </summary>
        vbPRPSNote = 18,
        /// <summary>
        /// 封筒 #9、3 7/8 x 8 7/8 インチ
        /// </summary>
        vbPRPSEnv9 = 19,
        /// <summary>
        /// 封筒 #10、4 1/8 x 9 1/2 インチ
        /// </summary>
        vbPRPSEnv10 = 20,
        /// <summary>
        /// 封筒 #11、4 1/2 x 10 3/8 インチ
        /// </summary>
        vbPRPSEnv11 = 21,
        /// <summary>
        /// 封筒 #12、4 1/2 x 11 インチ
        /// </summary>
        vbPRPSEnv12 = 22,
        /// <summary>
        /// 封筒 #14、5 x 11 1/2 インチ
        /// </summary>
        vbPRPSEnv14 = 23,
        /// <summary>
        /// C サイズ シート
        /// </summary>
        vbPRPSCSheet = 24,
        /// <summary>
        /// D サイズ シート
        /// </summary>
        vbPRPSDSheet = 25,
        /// <summary>
        /// E サイズ シート
        /// </summary>
        vbPRPSESheet = 26,
        /// <summary>
        /// 封筒 DL、110 x 220 mm
        /// </summary>
        vbPRPSEnvDL = 27,
        /// <summary>
        /// 封筒 C3、324 x 458 mm
        /// </summary>
        vbPRPSEnvC5 = 28,
        /// <summary>
        /// 封筒 C4、229 x 324 mm
        /// </summary>
        vbPRPSEnvC3 = 29,
        /// <summary>
        /// 封筒 C5、162 x 229 mm
        /// </summary>
        vbPRPSEnvC4 = 30,
        /// <summary>
        /// 封筒 C6、114 x 162 mm
        /// </summary>
        vbPRPSEnvC6 = 31,
        /// <summary>
        /// 封筒 C65、114 x 229 mm
        /// </summary>
        vbPRPSEnvC65 = 32,
        /// <summary>
        /// 封筒 B4、250 x 353 mm
        /// </summary>
        vbPRPSEnvB4 = 33,
        /// <summary>
        /// 封筒 B5、176 x 250 mm
        /// </summary>
        vbPRPSEnvB5 = 34,
        /// <summary>
        /// 封筒 B6、176 x 125 mm
        /// </summary>
        vbPRPSEnvB6 = 35,
        /// <summary>
        /// 封筒、110 x 230 mm
        /// </summary>
        vbPRPSEnvItaly = 36,
        /// <summary>
        /// 封筒 Monarch、3 7/8 x 7 1/2 インチ
        /// </summary>
        vbPRPSEnvMonarch = 37,
        /// <summary>
        /// 封筒、3 5/8 x 6 1/2 インチ
        /// </summary>
        vbPRPSEnvPersonal = 38,
        /// <summary>
        /// US スタンダード ファンフォールド、14 7/8 x 11 インチ
        /// </summary>
        vbPRPSFanfoldUS = 39,
        /// <summary>
        /// ドイツ スタンダード ファンフォールド、8 1/2 x 12 インチ
        /// </summary>
        vbPRPSFanfoldStdGerman = 40,
        /// <summary>
        /// ドイツ リーガル ファンフォールド、8 1/2 x 13 インチ
        /// </summary>
        vbPRPSFanfoldLglGerman = 41,
        /// <summary>
        /// ユーザー定義のサイズ
        /// </summary>
        vbPRPSUser = 256,
    }
    /// <summary>
    /// QueryUnload イベントの定数です。
    /// </summary>
    public enum QueryUnloadConstants
    {
        /// <summary>
        /// フォームのコントロール メニューから [閉じる] コマンドが選択されました。
        /// </summary>
        vbFormControlMenu = 0,
        /// <summary>
        /// コードから Unload メソッドが起動されました。
        /// </summary>
        vbFormCode = 1,
        /// <summary>
        /// 現在の Windows のセッションが終了しようとしています。
        /// </summary>
        vbAppWindows = 2,
        /// <summary>
        /// Windows のタスク マネージャによって、アプリケーションが閉じられました。
        /// </summary>
        vbAppTaskManager = 3,
        /// <summary>
        /// MDI フォームが閉じられたために、MDI 子フォームが閉じられました。
        /// </summary>
        vbFormMDIForm = 4,
        /// <summary>
        /// このフォームのオーナーフォームが終了します。
        /// </summary>
        vbFormOwner = 5,
    }
    /// <summary>
    /// ラスタ オペレーション定数です。
    /// </summary>
    public enum RasterOpConstants
    {
        /// <summary>
        /// Or 演算子を使って、結合したデスティネーション ビットマップとソース ビットマップを反転します。
        /// </summary>
        vbNotSrcErase = 1114278,
        /// <summary>
        /// 反転したソース ビットマップをデスティネーションにコピーします。
        /// </summary>
        vbNotSrcCopy = 3342344,
        /// <summary>
        /// デスティネーション ビットマップを反転し、And 演算子を使って、ソース ビットマップと結合します。
        /// </summary>
        vbSrcErase = 4457256,
        /// <summary>
        /// デスティネーション ビットマップを反転します。
        /// </summary>
        vbDstInvert = 5570569,
        /// <summary>
        /// Xor 演算子を使って、デスティネーション ビットマップとパターンを結合します。
        /// </summary>
        vbPatInvert = 5898313,
        /// <summary>
        /// Xor 演算子を使って、デスティネーション ビットマップとソース ビットマップのピクセルを結合します。
        /// </summary>
        vbSrcInvert = 6684742,
        /// <summary>
        /// And 演算子を使って、デスティネーション ビットマップとソース ビットマップのピクセルを結合します。
        /// </summary>
        vbSrcAnd = 8913094,
        /// <summary>
        /// Or 演算子を使って、反転したソース ビットマップとデスティネーション ビットマップを結合します。
        /// </summary>
        vbMergePaint = 12255782,
        /// <summary>
        /// パターンとソース ビットマップを結合します。
        /// </summary>
        vbMergeCopy = 12583114,
        /// <summary>
        /// ソース ビットマップをデスティネーション ビットマップにコピーします。
        /// </summary>
        vbSrcCopy = 13369376,
        /// <summary>
        /// Or 演算子を使って、デスティネーション ビットマップとソース ビットマップのピクセルを結合します。
        /// </summary>
        vbSrcPaint = 15597702,
        /// <summary>
        /// デスティネーション ビットマップにパターンをコピーします。
        /// </summary>
        vbPatCopy = 15728673,
        /// <summary>
        /// Or 演算子を使って、反転したソース ビットマップとパターンを結合します。
        /// さらに、Or 演算子を使って、結合結果とデスティネーション ビットマップを結合します。
        /// </summary>
        vbPatPaint = 16452105,
    }
    /// <summary>
    /// ScaleMode プロパティの定数です。
    /// </summary>
    public enum ScaleModeConstants
    {
        /// <summary>
        /// ScaleHeight、ScaleWidth、ScaleLeft、ScaleTop の各プロパティのうち 1 つ以上が独自の値に設定されていることを示します。
        /// </summary>
        vbUser = 0,
        /// <summary>
        /// (既定値)twip (論理インチあたり 1,440 twip、論理センチあたり 567 twip)。
        /// </summary>
        vbTwips = 1,
        /// <summary>
        /// ポイント (論理インチあたり 72 ポイント)。
        /// </summary>
        vbPoints = 2,
        /// <summary>
        /// ピクセル (画面またはプリンタの解像度の最小単位)。
        /// </summary>
        vbPixels = 3,
        /// <summary>
        /// キャラクタ (水平 = 1 単位あたり 120 twip、垂直 = 1 単位あたり 240 twip)。
        /// </summary>
        vbCharacters = 4,
        /// <summary>
        /// インチ。
        /// </summary>
        vbInches = 5,
        /// <summary>
        /// ミリメートル。
        /// </summary>
        vbMillimeters = 6,
        /// <summary>
        /// センチメートル。
        /// </summary>
        vbCentimeters = 7,
        /// <summary>
        /// HiMetric
        /// </summary>
        vbHimetric = 8,
        /// <summary>
        /// コントロールの位置を決定するためにコントロールのコンテナで使用する単位。
        /// </summary>
        vbContainerPosition = 9,
        /// <summary>
        /// コントロールのサイズを決定するためにコントロールのコンテナで使用する単位。
        /// </summary>
        vbContainerSize = 10,
    }
    /// <summary>
    /// ScrollBar 定数
    /// </summary>
    public enum ScrollBarConstants
    {
        /// <summary>
        /// なし
        /// </summary>
        vbSBNone = 0,
        /// <summary>
        /// 水平方向
        /// </summary>
        vbHorizontal = 1,
        /// <summary>
        /// 垂直方向
        /// </summary>
        vbVertical = 2,
        /// <summary>
        /// 両方向
        /// </summary>
        vbBoth = 3,
    }
    /// <summary>
    /// シェイプ (Shape) コントロール
    /// </summary>
    public enum ShapeConstants
    {
        /// <summary>
        /// 長方形
        /// </summary>
        vbShapeRectangle = 0,
        /// <summary>
        /// 正方形
        /// </summary>
        vbShapeSquare = 1,
        /// <summary>
        /// 楕円
        /// </summary>
        vbShapeOval = 2,
        /// <summary>
        /// 円
        /// </summary>
        vbShapeCircle = 3,
        /// <summary>
        /// 角の丸い長方形
        /// </summary>
        vbShapeRoundedRectangle = 4,
        /// <summary>
        /// 角の丸い正方形
        /// </summary>
        vbShapeRoundedSquare = 5,
    }
    /// <summary>
    /// Shift パラメータ マスク
    /// </summary>
    public enum ShiftConstants
    {
        /// <summary>
        /// Shift キー ビット マスク
        /// </summary>
        vbShiftMask = 1,
        /// <summary>
        /// Ctrl キー ビット マスク
        /// </summary>
        vbCtrlMask = 2,
        /// <summary>
        /// Alt キー ビット マスク
        /// </summary>
        vbAltMask = 4,
    }
    /// <summary>
    /// StartUpPosition プロパティの定数です。 
    /// </summary>
    public enum StartUpPositionConstants
    {
        /// <summary>
        /// 初期値が指定されていません。
        /// </summary>
        vbStartUpManual = 0,
        /// <summary>
        /// UserForm が属する項目の中央に指定します。
        /// </summary>
        vbStartUpOwner = 1,
        /// <summary>
        /// 画面全体の中央に指定します。
        /// </summary>
        vbStartUpScreen = 2,
        /// <summary>
        /// 画面の左上隅に指定します。
        /// </summary>
        vbStartUpWindowsDefault = 3,
    }
    /// <summary>
    /// システムカラー定数
    /// </summary>
    public enum SystemColorConstants
    {
        /// <summary>
        /// スクロール バーの色
        /// </summary>
        vbScrollBars = -2147483648,
        /// <summary>
        /// デスクトップの色
        /// </summary>
        vbDesktop = -2147483647,
        /// <summary>
        /// アクティブ タイトル バーの色
        /// </summary>
        vbActiveTitleBar = -2147483646,
        /// <summary>
        /// 非アクティブ タイトル バーの色
        /// </summary>
        vbInactiveTitleBar = -2147483645,
        /// <summary>
        /// メニューの背景色
        /// </summary>
        vbMenuBar = -2147483644,
        /// <summary>
        /// ウィンドウの背景色
        /// </summary>
        vbWindowBackground = -2147483643,
        /// <summary>
        /// ウィンドウの枠の色
        /// </summary>
        vbWindowFrame = -2147483642,
        /// <summary>
        /// メニューの文字の色
        /// </summary>
        vbMenuText = -2147483641,
        /// <summary>
        /// ウィンドウ内の文字の色
        /// </summary>
        vbWindowText = -2147483640,
        /// <summary>
        /// アクティブ タイトル バーの文字の色
        /// </summary>
        vbActiveTitleBarText = -2147483639,
        /// <summary>
        /// キャプション、サイズ ボックス、および矢印ボタン内の色
        /// </summary>
        vbTitleBarText = -2147483639,
        /// <summary>
        /// アクティブ ウィンドウの境界の色
        /// </summary>
        vbActiveBorder = -2147483638,
        /// <summary>
        /// 非アクティブ ウィンドウの境界の色
        /// </summary>
        vbInactiveBorder = -2147483637,
        /// <summary>
        /// MDI アプリケーションの背景色
        /// </summary>
        vbApplicationWorkspace = -2147483636,
        /// <summary>
        /// コントロール内で選択した項目の背景色
        /// </summary>
        vbHighlight = -2147483635,
        /// <summary>
        /// コントロール内で選択した項目の文字の色
        /// </summary>
        vbHighlightText = -2147483634,
        /// <summary>
        /// コマンド ボタンの表面の色
        /// </summary>
        vbButtonFace = -2147483633,
        /// <summary>
        /// 文字の表面の色
        /// </summary>
        vb3DFace = -2147483633,
        /// <summary>
        /// 文字の影の色
        /// </summary>
        vb3DShadow = -2147483632,
        /// <summary>
        /// コマンド ボタンの端の影の色
        /// </summary>
        vbButtonShadow = -2147483632,
        /// <summary>
        /// 淡色表示 (使用不可) の文字の色
        /// </summary>
        vbGrayText = -2147483631,
        /// <summary>
        /// プッシュ ボタンの文字の色
        /// </summary>
        vbButtonText = -2147483630,
        /// <summary>
        /// 非アクティブなウインドウタイトル、サイズ変更ボックス、およびスクロールバーの文字の色
        /// </summary>
        vbInactiveTitleBarText = -2147483629,
        /// <summary>
        /// 非アクティブなキャプションの文字色
        /// </summary>
        vbInactiveCaptionText = -2147483629,
        /// <summary>
        /// 3-D 表示要素の強調表示の色
        /// </summary>
        vb3DHighlight = -2147483628,
        /// <summary>
        /// 3-D 表示要素の濃い影の色
        /// </summary>
        vb3DDKShadow = -2147483627,
        /// <summary>
        /// 定数 vb3Dhighlight の次に明るい 3-D 表示要素の色
        /// </summary>
        vb3DLight = -2147483626,
        /// <summary>
        /// ツール ヒントのテキストの色
        /// </summary>
        vbInfoText = -2147483625,
        /// <summary>
        /// ツール ヒントの背景色
        /// </summary>
        vbInfoBackground = -2147483624,
    }
    /// <summary>
    /// ZOrder メソッドの定数
    /// </summary>
    public enum ZOrderConstants
    {
        /// <summary>
        /// 最前面に移動
        /// </summary>
        vbBringToFront = 0,
        /// <summary>
        /// 最背面へ移動
        /// </summary>
        vbSendToBack = 1,
    }

    /// <summary>
    /// TreeRelationship 定数
    /// </summary>
    public enum TreeRelationshipConstants
    {
        /// <summary>
        /// 先頭の項目。
        /// </summary>
        tvwFirst = 0,
        /// <summary>
        /// 末尾の項目。
        /// </summary>
        tvwLast = 1,
        /// <summary>
        /// 次の項目。
        /// </summary>
        tvwNext = 2,
        /// <summary>
        /// 前の項目。
        /// </summary>
        tvwPrevious = 3,
        /// <summary>
        /// 子。
        /// </summary>
        tvwChild = 4,
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
    /// ListItem オブジェクトの並べ替え順序。
    /// </summary>
    [Description("ListItem オブジェクトの並べ替え順序。")]
    public enum ListSortOrderConstants
    {
        /// <summary>昇順。</summary>
        lvwAscending,
        /// <summary>降順。</summary>
        lvwDescending
    }

    /// <summary>
    /// テキスト入力パターンの名前を定義します。 
    /// </summary>
    public enum InputScope
    {
        /// <summary>XML のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        Xml = -4,
        /// <summary>Speech Recognition Grammar Specification (SRGS) のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        Srgs = -3,
        /// <summary>正規表現のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        RegularExpression = -2,
        /// <summary>語句一覧のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        PhraseList = -1,
        /// <summary>入力コマンドの既定の処理。</summary>
        Default = 0,
        /// <summary>URL (Uniform Resource Locator) のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        Url = 1,
        /// <summary>ファイルの完全パスのテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        FullFilePath = 2,
        /// <summary>ファイル名のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        FileName = 3,
        /// <summary>電子メール ユーザー名のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        EmailUserName = 4,
        /// <summary>簡易メール転送プロトコル (SMTP) 電子メール アドレスのテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        EmailSmtpAddress = 5,
        /// <summary>ログオン名のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        LogOnName = 6,
        /// <summary>人名 (フル ネーム) のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        PersonalFullName = 7,
        /// <summary>人名のプレフィックスのテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        PersonalNamePrefix = 8,
        /// <summary>人名 (名) のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        PersonalGivenName = 9,
        /// <summary>人名 (ミドル ネーム) のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        PersonalMiddleName = 10,
        /// <summary>人名 (姓) のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        PersonalSurname = 11,
        /// <summary>人名のサフィックスのテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        PersonalNameSuffix = 12,
        /// <summary>住所のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        PostalAddress = 13,
        /// <summary>郵便番号のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        PostalCode = 14,
        /// <summary>番地のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        AddressStreet = 15,
        /// <summary>都道府県のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        AddressStateOrProvince = 16,
        /// <summary>都市名のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        AddressCity = 17,
        /// <summary>国名のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        AddressCountryName = 18,
        /// <summary>国の省略名のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        AddressCountryShortName = 19,
        /// <summary>通貨金額および通貨記号のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        CurrencyAmountAndSymbol = 20,
        /// <summary>通貨金額のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        CurrencyAmount = 21,
        /// <summary>カレンダー日付のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        Date = 22,
        /// <summary>カレンダー日付の月 (数字) のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        DateMonth = 23,
        /// <summary>カレンダー日付の日 (数字) のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        DateDay = 24,
        /// <summary>カレンダー日付の年のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        DateYear = 25,
        /// <summary>カレンダー日付の月 (名前) のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        DateMonthName = 26,
        /// <summary>カレンダー日付の日 (名前) のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        DateDayName = 27,
        /// <summary>数字のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        Digits = 28,
        /// <summary>数字のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        Number = 29,
        /// <summary>1 文字のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        OneChar = 30,
        /// <summary>パスワードのテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        Password = 31,
        /// <summary>電話番号のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        TelephoneNumber = 32,
        /// <summary>電話の国番号のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        TelephoneCountryCode = 33,
        /// <summary>電話の市外局番のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        TelephoneAreaCode = 34,
        /// <summary>電話の市内局番のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        TelephoneLocalNumber = 35,
        /// <summary>時刻のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        Time = 36,
        /// <summary>時刻 (時) のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        TimeHour = 37,
        /// <summary>時刻 (分または秒) のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        TimeMinorSec = 38,
        /// <summary>全角数字のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        NumberFullWidth = 39,
        /// <summary>半角英数字のテキスト入力パターン。</summary>
        AlphanumericHalfWidth = 40,
        /// <summary>全角英数字のテキスト入力パターン。</summary>
        AlphanumericFullWidth = 41,
        /// <summary>中国通貨のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        CurrencyChinese = 42,
        /// <summary>ボポモフォ標準中国語発音表記システムのテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        Bopomofo = 43,
        /// <summary>ひらがな書記体系のテキスト入力パターン。</summary>
        Hiragana = 44,
        /// <summary>半角カタカナ文字のテキスト入力パターン。</summary>
        KatakanaHalfWidth = 45,
        /// <summary>全角カタカナ文字のテキスト入力パターン。</summary>
        KatakanaFullWidth = 46,
        /// <summary>Hanja 文字のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        Hanja = 47,

        // 以下は GrapeCity INPUTMAN 7.0J SP1 より

        /// <summary>半角 Hanja 文字のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        HanjaHalfWidth = 48,
        /// <summary>全角 Hanja 文字のテキスト入力パターン。</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        HanjaFullWidth = 49,
    }

    #region CommonDialog

    /// <summary>
    /// Action プロパティ
    /// </summary>
    public enum cdlActionConstans
    {
        /// <summary>
        /// [ファイルを開く] ダイアログ ボックスを表示します。
        /// </summary>
        ACT_SHOWOPEN = 1,
        /// <summary>
        /// [ファイル名を付けて保存] ダイアログ ボックスを表示します。
        /// </summary>
        ACT_SHOWSAVE = 2,
        /// <summary>
        /// [色の設定] ダイアログ ボックスを表示します。
        /// </summary>
        ACT_SHOWCOLOR = 3,
        /// <summary>
        /// [フォントの指定] ダイアログ ボックスを表示します。
        /// </summary>
        ACT_SHOWFONT = 4,
        /// <summary>
        /// [印刷] ダイアログ ボックスを表示します。
        /// </summary>
        ACT_SHOWPRINT = 5,
        /// <summary>
        /// WINHLP32.EXE を起動します｡
        /// </summary>
        ACT_SHOWHELP = 6,
    }

    /// <summary>
    /// CommonDialog で呼び出されるオンライン ヘルプの種類を設定します。
    /// </summary>
    [Flags]
    public enum cdlHelpConstants
    {
        /// <summary>初期値です。</summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        cdlUnknown = 0,
        /// <summary>ヘルプ マクロを実行します。マクロ文字列を HelpKey プロパティに設定する必要があります。</summary>
        cdlHelpCommandHelp = 0x102,
        /// <summary>.hpj ファイルの [OPTION] セクションにある [Contents] オプションの定義にしたがって、ヘルプの目次を表示します。</summary>
        cdlHelpContents = 0x3,
        /// <summary>状況依存のヘルプを表示します。この設定を使用する場合は、HelpContext プロパティを使ってコンテキストを指定する必要があります。</summary>
        cdlHelpContext = 0x1,
        /// <summary>.hpj ファイルの [MAP] セクションで定義されたコンテキスト番号によって識別される特定のヘルプ トピックをポップアップ ウィンドウに表示します。</summary>
        cdlHelpContextPopup = 0x8,
        /// <summary>Winhlp32.exe が正しいヘルプ ファイルを表示するようにします。その時点で、正しいヘルプ ファイルが表示されている場合は、何も行われません。別のヘルプ ファイルが表示されている場合は、Winhlp32.exe によって正しいファイルが開かれます。</summary>
        cdlHelpForceFile = 0x9,
        /// <summary>ヘルプの使い方についてのヘルプを表示します。</summary>
        cdlHelpHelpOnHelp = 0x4,
        ///// <summary>指定されたヘルプ ファイルのインデックスを表示します。この設定は、インデックスが 1 つしかないヘルプ ファイルに対してだけ有効です。</summary>
        //cdlHelpIndex = 0x3,
        /// <summary>特定のキーワードに関するヘルプを表示します。この設定を使用する場合は、HelpKey プロパティを使ってキーワードを指定する必要があります。</summary>
        cdlHelpKey = 0x101,
        /// <summary>
        /// キーワード リストで、HelpKey プロパティによって指定されたキーワードと正確に一致するトピックが 1 つだけ見つかった場合に、そのトピックを表示します。
        /// 一致するトピックが複数ある場合は、[トピックの検索] ダイアログ ボックスの [ジャンプ] リスト ボックスに見つかったトピックが表示されます。
        /// 一致するトピックが見つからなかった場合は、[トピックの検索] ダイアログ ボックスが表示されます。
        /// [トピックの検索] ダイアログ ボックスを表示するには、HelpKey プロパティに空文字列 ("") を設定します。
        /// </summary>
        cdlHelpPartialKey = 0x105,
        /// <summary>指定されたヘルプ ファイルがもう使われていないことをヘルプ エンジンに通知します。</summary>
        cdlHelpQuit = 0x2,
        /// <summary>HelpContext プロパティによって指定されたコンテキストを、HelpFile プロパティによって指定されたヘルプ ファイルの現在のインデックスとして設定します。このインデックスは、ユーザーが別のヘルプ ファイルにアクセスするまで、現在のインデックスとして有効です。このインデックスは、複数のインデックスを持つヘルプ ファイルにだけ設定できます。</summary>
        cdlHelpSetIndex = 0x5,
    }

    /// <summary>
    /// 「色の設定」ダイアログボックスの定数
    /// </summary>
    [Flags]
    public enum cdlColorConstants
    {
        /// <summary>
        /// カスタム カラーを表示した状態にします。
        /// </summary>
        cdlCCRGBInit = 0x0001,
        /// <summary>
        /// ダイアログボックスを表示するときに、色の作成を行う部分も同時に表示します。
        /// </summary>
        cdlCCFullOpen = 0x0002,
        /// <summary>
        /// ダイアログボックスの「色の作成」ボタンを無効にします。
        /// </summary>
        cdlCCPreventFullOpen = 0x0004,
        /// <summary>
        /// ダイアログボックスにヘルプボタンが表示されます。
        /// </summary>
        cdlCCHelpButton = 0x0008,
        /// <summary>
        /// ダイアログ ボックスでユーザーが選択できる色を純色だけに制限するかどうかを示す値を取得または設定します。
        /// </summary>
        cdlCCSolidColorOnly = 0x0080,
        /// <summary>
        /// 使用可能なすべての色を基本色セットとしてダイアログ ボックスに表示します。
        /// </summary>
        cdlCCAnyColor = 0x0100,
    }

    /// <summary>
    /// 「ファイルを開く」または「ファイル名をつけて保存」ダイアログボックスの定数
    /// </summary>
    [Flags]
    public enum cdlFileOpenConstants
    {
        /// <summary>
        /// 「読み取り専用] のチェックボックスをオンにします。
        /// </summary>
        cdlOFNReadOnly = 0x00000001,
        /// <summary>
        /// 選択したファイルが既に存在する場合はメッセージボックスを表示します。
        /// </summary>
        cdlOFNOverwritePrompt = 0x00000002,
        /// <summary>
        /// 「読み取り専用] のチェックボックスを非表示にします。
        /// </summary>
        cdlOFNHideReadOnly = 0x00000004,
        /// <summary>
        /// ダイアログボックスを閉じる前に現在のディレクトリを復元します。
        /// </summary>
        cdlOFNNoChangeDir = 0x00000008,
        /// <summary>
        /// ダイアログボックスにヘルプボタンを表示します。
        /// </summary>
        cdlOFNHelpButton = 0x00000010,
        /// <summary>
        /// ファイル名に不正な文字が含まれていた場合にもファイル名が返されます。
        /// </summary>
        cdlOFNNoValidate = 0x00000100,
        /// <summary>
        /// 複数選択が可能になります。
        /// </summary>
        cdlOFNAllowMultiselect = 0x00000200,
        /// <summary>
        /// 選択されたファイルの拡張子が DefaultExt プロパティによって指定された拡張子と異なることを示します。
        /// </summary>
        cdlOFNExtensionDifferent = 0x00000400,
        /// <summary>
        /// 存在しないパスを指定した場合、警告を表示します。
        /// </summary>
        cdlOFNPathMustExist = 0x00000800,
        /// <summary>
        /// 存在しないファイルを指定した場合、警告を表示します。
        /// </summary>
        cdlOFNFileMustExist = 0x00001000,
        /// <summary>
        /// 存在しないファイルを指定した場合、新規に作成するかどうか確認します。
        /// </summary>
        cdlOFNCreatePrompt = 0x00002000,
        /// <summary>
        /// 共有違反エラーを無視するようにします。
        /// </summary>
        cdlOFNShareAware = 0x00004000,
        /// <summary>
        /// 「名前を付けて保存」 ダイアログ ボックスで、読み取り専用属性を持つファイル、または読み取り専用ディレクトリに存在するファイルを選択できないようにします。 このフラグが設定され、該当ファイルを選択した場合、警告が表示されます。
        /// </summary>
        cdlOFNNoReadOnlyReturn = 0x00008000,
        /// <summary>
        /// 長いファイル名を使用しません。
        /// </summary>
        cdlOFNNoLongNames = 0x00040000,
        /// <summary>
        /// エクスプローラ形式の [ファイルを開く] ダイアログ ボックスを使用します。
        /// </summary>
        cdlOFNExplorer = 0x00080000,
        /// <summary>
        /// ショートカットファイルの参照を禁止します。（ショートカットファイルを選択できるようにします） このフラグを設定しない場合、ショートカットファイルを指定すると、参照先のファイルが設定されます。
        /// </summary>
        cdlOFNNoDereferenceLinks = 0x00100000,
        /// <summary>
        /// 長いファイル名を使用します。
        /// </summary>
        cdlOFNLongNames = 0x00200000,
        /// <summary>
        /// ユーザーが拡張子を指定しない場合、ダイアログ ボックスがファイル名に自動的に拡張子を付けるかどうかを示す値を取得または設定します。
        /// </summary>
        cdlOFNAddExtension = unchecked(unchecked((int)0x80000000))
    }

    /// <summary>
    /// [フォントの指定] ダイアログ ボックスのフラグ
    /// </summary>
    [Flags]
    public enum cdlFontsConstants
    {
        /// <summary>
        /// ダイアログ ボックスには、システムでサポートされているスクリーン フォントのみが表示されます。
        /// </summary>
        cdlCFScreenFonts = 1,
        /// <summary>
        /// ダイアログ ボックスには、hDC プロパティで指定したプリンタがサポートするフォントのみが表示されます。
        /// </summary>
        cdlCFPrinterFonts = 2,
        /// <summary>
        /// ダイアログ ボックスには利用可能なスクリーン フォントとプリンタ フォントが表示されます。hDC プロパティは、プリンタと関連付けられたデバイス コンテキスト を示します。
        /// </summary>
        cdlCFBoth = 3,
        /// <summary>
        /// ダイアログ ボックスに [ヘルプ] ボタンを表示します。
        /// </summary>
        cdlCFHelpButton = 4,
        /// <summary>
        /// 取り消し線、下線、および色の設定を可能にします。
        /// </summary>
        cdlCFEffects = 256,
        /// <summary>
        /// ダイアログ ボックスで [適用] ボタンが使用できるようになります。
        /// </summary>
        cdlCFApply = 512,
        /// <summary>
        /// Windows の文字セットを使うフォントだけを選択できるようにします。このフラグが設定されている場合、ユーザーは記号だけを含むフォントを選択することはできません。
        /// </summary>
        cdlCFANSIOnly = 1024,
        /// <summary>
        /// Windows の文字セットを使うフォントだけを選択できるようにします。このフラグが設定されている場合、ユーザーは記号だけを含むフォントを選択することはできません。
        /// </summary>
        cdlCFScriptOnly = cdlCFANSIOnly,
        /// <summary>
        /// ダイアログ ボックスでベクタ フォントを選択できないようにします。
        /// </summary>
        cdlCFNoVectorFonts = 2048,
        /// <summary>
        /// ダイアログ ボックスでグラフィック デバイス インターフェイス (GDI) フォントのシミュレーションを行わないようにします。
        /// </summary>
        cdlCFNoSimulations = 4096,
        /// <summary>
        /// Min プロパティと Max プロパティで指定された値の範囲でフォントのサイズを選択します。
        /// </summary>
        cdlCFLimitSize = 8192,
        /// <summary>
        /// ダイアログ ボックスでは固定ピッチのフォントのみが選択できます。
        /// </summary>
        cdlCFFixedPitchOnly = 16384,
        /// <summary>
        /// プリンタと画面の両方で使用可能なフォントだけを選択できるようにします。このフラグにより、cdlCFBoth および cdlCFScalableOnly フラグも設定されます。
        /// </summary>
        cdlCFWYSIWYG = 32768,
        /// <summary>
        /// ユーザーが存在しないフォントやスタイルを指定した場合に、エラーを表示します。
        /// </summary>
        cdlCFForceFontExist = 65536,
        /// <summary>
        /// ダイアログ ボックスではスケーラブル フォントのみが選択できます。
        /// </summary>
        cdlCFScalableOnly = 131072,
        /// <summary>
        /// ダイアログ ボックスでは True Type フォントのみが選択できます。
        /// </summary>
        cdlCFTTOnly = 262144,
        /// <summary>
        /// フォント名が選択されていません。
        /// </summary>
        cdlCFNoFaceSel = 524288,
        /// <summary>
        /// フォントのスタイルが選択されていません。
        /// </summary>
        cdlCFNoStyleSel = 1048576,
        /// <summary>
        /// フォントのサイズが選択されていません。
        /// </summary>
        cdlCFNoSizeSel = 2097152,
        /// <summary>
        /// ダイアログ ボックスに縦書きと横書きのフォントを両方とも表示するのか、横書きフォントだけを表示するのかを示す値を取得または表示します。
        /// </summary>
        cdlCFNoVerticalFonts = 0x01000000,
        /// <summary>
        /// [スクリプト] ボックスに指定されている文字セットをユーザーが変更し、現在表示されている文字セットとは異なる文字セットを表示できるかどうかを示す値を取得または設定します。
        /// </summary>
        cdlCFSelectScript = 0x00400000,
    }

    /// <summary>
    /// [印刷] ダイアログ ボックスのフラグ
    /// </summary>
    [Flags]
    public enum cdlPrinterConstants : int
    {
        /// <summary>
        /// すべてのページのオプション ボタンの状態を設定します。値の取得も可能です。
        /// </summary>
        cdlPDAllPages = 0x0000,
        /// <summary>
        /// [選択した部分] オプション ボタンの状態を設定します。値の取得も可能です。cdlPDPageNums と cdlPDSelection の両方が指定されていない場合、オプション ボタンはすべて選択された状態になります。
        /// </summary>
        cdlPDSelection = 0x0001,
        /// <summary>
        /// ページ範囲を設定するオプション ボタンの状態を設定します。値の取得も可能です。
        /// </summary>
        cdlPDPageNums = 0x0002,
        /// <summary>
        /// [選択した部分] オプション ボタンを無効にします。
        /// </summary>
        cdlPDNoSelection = 0x0004,
        /// <summary>
        /// ページ範囲を設定するオプション ボタンおよび関連するエディット コントロールを無効にします。
        /// </summary>
        cdlPDNoPageNums = 0x0008,
        /// <summary>
        /// [部単位で印刷]チェック ボックスの状態を設定します。値の取得も可能です。
        /// </summary>
        cdlPDCollate = 0x0010,
        /// <summary>
        /// [ファイルへ出力] チェック ボックスの状態を設定します。値の取得も可能です。
        /// </summary>
        cdlPDPrintToFile = 0x0020,
        /// <summary>
        /// [印刷] ダイアログ ボックスの替わりに、[プリンタの設定] ダイアログ ボックスを表示します。
        /// </summary>
        cdlPDPrintSetup = 0x0040,
        /// <summary>
        /// 通常使うプリンタがないときでも、警告メッセージを表示しないようにします。
        /// </summary>
        cdlPDNoWarning = 0x0080,
        /// <summary>
        /// 選択したプリンタのデバイス コンテキストを返します。デバイス コンテキストは、ダイアログ ボックスの hDC プロパティに返されます。
        /// </summary>
        cdlPDReturnDC = 0x0100,
        /// <summary>
        /// 選択したプリンタに関する情報を返します。デバイス コンテキストは、ダイアログ ボックスの hDC プロパティに返されます。
        /// </summary>
        cdlPDReturnIC = 0x0200,
        /// <summary>
        /// 通常使うプリンタの名前を返します。
        /// </summary>
        cdlPDReturnDefault = 0x0400,
        /// <summary>
        /// ダイアログ ボックスにヘルプ ボタンが表示されます。
        /// </summary>
        cdlPDHelpButton = 0x0800,
        /// <summary>
        /// プリンタ ドライバが複数印刷をサポートしない場合、このフラグを設定すると [部数] エディット コントロールを無効にします。プリンタ ドライバが複数印刷をサポートする場合、このフラグを設定すると Copies プロパティに指定された部数がダイアログ ボックスに設定されます。
        /// </summary>
        cdlPDUseDevModeCopies = 0x40000,
        /// <summary>
        /// [ファイルへ出力] チェック ボックスを無効にします。
        /// </summary>
        cdlPDDisablePrintToFile = 0x80000,
        /// <summary>
        /// [ファイルへ出力] チェック ボックスを非表示にします。
        /// </summary>
        cdlPDHidePrintToFile = 0x100000
    }

    /// <summary>
    /// StrConvEx の第２引数
    /// </summary>
    [Flags]
    public enum vb6Conversion
    {
        /// <summary>
        /// 文字列を大文字に変換します。
        /// </summary>
        UpperCase = 0x01,
        /// <summary>
        /// 文字列を小文字に変換します。
        /// </summary>
        LowerCase = 0x02,
        /// <summary>
        /// 文字列の各単語の先頭の文字を大文字に変換します。
        /// </summary>
        ProperCase = 0x03,
        /// <summary>
        /// 文字列内の半角文字 (1 バイト) を全角文字 (2 バイト) に変換します。
        /// </summary>
        Wide = 0x04,
        /// <summary>
        /// 文字列内の全角文字 (2 バイト) を半角文字 (1 バイト) に変換します。
        /// </summary>
        Narrow = 0x08,
        /// <summary>
        /// 文字列内のひらがなをカタカナに変換します。
        /// </summary>
        Katakana = 0x10,
        /// <summary>
        /// 文字列内のカタカナをひらがなに変換します。
        /// </summary>
        Hiragana = 0x20,
        /// <summary>
        /// システムの既定のコード ページを使って文字列をUnicode に変換します。
        /// </summary>
        vbUnicode = 0x40,
        /// <summary>
        /// 文字列を Unicode からシステムの既定のコード ページに変換します。
        /// </summary>
        vbFromUnicode = 0x80,
    }

    /// <summary>
    /// タブキーを押下したときのフォーカス遷移を VB6.0 互換にするかどうかを設定する列挙体。
    /// </summary>
    public enum TabIndexMode
    {
        /// <summary>
        /// VB.NET 互換モード
        /// </summary>
        NET = 0,
        /// <summary>
        /// VB6.0 互換モード
        /// </summary>
        VB6 = 1,
    }


    #endregion
}
