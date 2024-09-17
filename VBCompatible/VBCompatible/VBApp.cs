namespace VBCompatible
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;
    using System.Security.Principal;
    using Microsoft.VisualBasic.ApplicationServices;
    using Microsoft.VisualBasic.CompilerServices;
    using VBCompatible.VB6;

    /// <summary>
    /// VB 互換 App オブジェクト
    /// </summary>
    /// <remarks>
    /// <para>【重要】必ずメインプログラムから呼び出してください。</para>
    /// <para>以下のプロパティ/メソッドはサポートしません。</para>
    /// <para>HelpFile</para>
    /// <para>RetainedProject</para>
    /// <para>StartMode</para>
    /// <para>TaskVisible</para>
    /// <para>UnattendedApp</para>
    /// <para>FileDescription</para>
    /// <para>OleRequestPendingMsgText</para>
    /// <para>OleRequestPendingMsgTitle</para>
    /// <para>OleRequestPendingTimeout</para>
    /// <para>OleServerBusyMsgText</para>
    /// <para>OleServerBusyMsgTitle</para>
    /// <para>OleServerBusyRaiseError</para>
    /// <para>OleServerBusyTimeout</para>
    /// </remarks>
    public class VBApp : IDisposable
    {
        //
        //   プロパティ
        //
        string m_Comments;
        string m_CompanyName;
        string m_EXEName;
        HandleRef m_hInstance;
        string m_LegalCopyright;
        string m_LegalTrademarks;
        LogModeConstants m_LogMode;
        string m_LogPath;
        int m_Major;
        int m_Minor;
        string m_Path;
        bool m_PrevInstance;
        string m_ProductName;
        int m_Revision;

        //Dim m_FileDescription As String
        //Dim m_HelpFile As String
        //Dim m_RetainedProject As Boolean
        //Dim m_StartMode As ApplicationStartConstants
        //Dim m_TaskVisible As Boolean
        //Dim m_UnattendedApp As Boolean
        //
        //   LogMode プロパティの属性フラグ
        //
        bool m_overWrite;
        bool m_logToNT;
        bool m_logToFile;
        bool m_logThreadID;

        static VBHostImpl m_VBHostImpl;

        static VBApp() {
            m_VBHostImpl = new VBHostImpl();
            HostServices.VBHost = m_VBHostImpl;
        }

        private class VBHostImpl : IVbHost
        {
            public IWin32Window GetParentWindow() {
                return null;
            }

            public string GetWindowTitle() {
                return Title;
            }

            public string Title { get; set; }
        }


        private VBApp(Assembly asm) {
            m_PrevInstance = PrevInstanceInternal;
            m_EXEName = System.IO.Path.GetFileNameWithoutExtension(asm.Location);
            m_Path = System.IO.Path.GetDirectoryName(asm.Location);
            m_hInstance = new HandleRef(this, NativeMethods.GetModuleHandle(null));
            Version ver = asm.GetName().Version;
            m_Major = ver.Major;
            m_Minor = ver.Minor;
            m_Revision = ver.Revision;

            foreach (object v in asm.GetCustomAttributes(false)) {
                Type typ = v.GetType();
                if (typ == typeof(AssemblyCompanyAttribute)) {
                    AssemblyCompanyAttribute comp = (AssemblyCompanyAttribute)v;
                    m_CompanyName = comp.Company;
                } else if (typ == typeof(AssemblyDescriptionAttribute)) {
                    AssemblyDescriptionAttribute desc = (AssemblyDescriptionAttribute)v;
                    m_Comments = desc.Description;
                } else if (typ == typeof(AssemblyCopyrightAttribute)) {
                    AssemblyCopyrightAttribute cpy = (AssemblyCopyrightAttribute)v;
                    m_LegalCopyright = cpy.Copyright;
                } else if (typ == typeof(AssemblyTrademarkAttribute)) {
                    AssemblyTrademarkAttribute tmark = (AssemblyTrademarkAttribute)v;
                    m_LegalTrademarks = tmark.Trademark;
                } else if (typ == typeof(AssemblyTitleAttribute)) {
                    AssemblyTitleAttribute title = (AssemblyTitleAttribute)v;
                    m_VBHostImpl.Title = title.Title;
                } else if (typ == typeof(AssemblyProductAttribute)) {
                    AssemblyProductAttribute product = (AssemblyProductAttribute)v;
                    m_ProductName = product.Product;
                }
            }

            this.LogMode = LogModeConstants.vbLogAuto;
            this.LogPath = string.Empty;
        }

        private static Dictionary<Assembly, VBApp> AppCollection = new Dictionary<Assembly, VBApp>();

        /// <summary>
        /// VBApp クラスのインスタンスを取得します。
        /// </summary>
        /// <param name="asm">対象となるアセンブリ</param>
        /// <returns>VBApp オブジェクト</returns>
        public static VBApp GetDefaultInstance(Assembly asm) {
            VBApp item;
            if (!AppCollection.TryGetValue(asm, out item)) {
                item = new VBApp(asm);
                AppCollection.Add(asm, item);
            }
            return item;
        }

        /// <summary>
        /// 実行可能ファイルについてのコメントを含む文字列を取得します。
        /// </summary>
        public string Comments {
            get { return m_Comments; }
        }

        /// <summary>
        /// 会社名を返します。
        /// </summary>
        public string CompanyName {
            get { return m_CompanyName; }
        }

        /// <summary>
        /// 現在実行中の実行可能ファイルの、拡張子を除いたファイル名を取得します。
        /// </summary>
        public string EXEName {
            get { return m_EXEName; }
        }

        //Public ReadOnly Property FileDescription() As String
        //    Get
        //        Return m_FileDescription
        //    End Get
        //End Property

        //Public ReadOnly Property HelpFile() As String
        //    Get
        //        Return m_HelpFile
        //    End Get
        //End Property

        /// <summary>
        /// アプリケーションのインスタンスを参照するハンドルを返します。
        /// </summary>
        public HandleRef hInstance {
            get { return m_hInstance; }
        }

        /// <summary>
        /// 実行可能ファイルについての著作権情報を返します。
        /// </summary>
        public string LegalCopyright {
            get { return m_LegalCopyright; }
        }

        /// <summary>
        /// 実行可能ファイルについての商標情報を返します。
        /// </summary>
        public string LegalTrademarks {
            get { return m_LegalTrademarks; }
        }

        /// <summary>
        /// ログの記録方法を表す値を返します。ログの記録は、LogEvent メソッドにより行われます。
        /// </summary>
        public LogModeConstants LogMode {
            get { return m_LogMode; }

            private set {
                m_LogMode = value;
                if (m_LogMode == LogModeConstants.vbLogAuto) {
                    m_overWrite = false;
                    m_logToFile = false;
                    m_logToNT = true;
                    m_logThreadID = false;
                } else {
                    m_overWrite = (LogMode & LogModeConstants.vbLogOverwrite) == LogModeConstants.vbLogOverwrite;
                    m_logToNT = (LogMode & LogModeConstants.vbLogToNT) == LogModeConstants.vbLogToNT;
                    m_logToFile = (LogMode & LogModeConstants.vbLogToFile) == LogModeConstants.vbLogToFile;
                    m_logThreadID = (LogMode & LogModeConstants.vbLogThreadID) == LogModeConstants.vbLogThreadID;
                }
                if (m_logToFile & m_overWrite) {
                    using (StreamWriter logFile = new StreamWriter(m_LogPath, false)) {

                    }
                }
            }
        }

        /// <summary>
        /// LogEvent メソッドの出力を記録するファイルのパスおよびファイル名を返します。
        /// </summary>
        public string LogPath {
            get { return m_LogPath; }
            private set {
                if (string.IsNullOrEmpty(value)) {
                    m_LogPath = System.IO.Path.Combine(this.Path, "vbevents.log");
                } else {
                    m_LogPath = value;
                }
            }
        }

        /// <summary>
        /// プロジェクトのメジャー リリース番号を設定します。
        /// </summary>
        public int Major {
            get { return m_Major; }
        }

        /// <summary>
        /// プロジェクトのマイナ リリース番号を設定します。
        /// </summary>
        public int Minor {
            get { return m_Minor; }
        }

        /// <summary>
        /// フォームをモードレスに表示可能かを示す値を返します。
        /// </summary>
        public bool NonModalAllowed {
            get {
                foreach (Form f in Application.OpenForms) {
                    if (f.Modal) {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// アプリケーションの存在するパスを返します。
        /// </summary>
        public string Path {
            get { return m_Path; }
        }

        /// <summary>
        /// アプリケーションのインスタンスが既に実行されているかどうかを示す値を返します。
        /// </summary>
        public bool PrevInstance {
            get { return m_PrevInstance; }
        }

        /// <summary>
        /// 実行可能ファイルの製品名を返します。
        /// </summary>
        public string ProductName {
            get { return m_ProductName; }
        }

        //Public ReadOnly Property RetainedProject() As Boolean
        //    Get
        //        Return m_RetainedProject
        //    End Get
        //End Property

        /// <summary>
        /// プロジェクトのリビジョン番号を返します。
        /// </summary>
        public int Revision {
            get { return m_Revision; }
        }

        //Public ReadOnly Property StartMode() As ApplicationStartConstants
        //    Get
        //        Return m_StartMode
        //    End Get
        //End Property

        //Public ReadOnly Property TaskVisible() As Boolean
        //    Get
        //        Return m_TaskVisible
        //    End Get
        //End Property

        /// <summary>
        /// 実行中のスレッドの Win32 ID を取得するために使います。
        /// </summary>
        public int ThreadID {
            get { return (int)NativeMethods.GetCurrentThreadId(); }
        }

        /// <summary>
        /// タスクリストに表示されるアプリケーションタイトルを設定します。
        /// </summary>
        public string Title {
            get { return m_VBHostImpl.Title; }
            set { m_VBHostImpl.Title = value; }
        }

        //Public ReadOnly Property UnattendedApp() As Boolean
        //    Get
        //        Return m_UnattendedApp
        //    End Get
        //End Property

        /// <summary>
        /// ログ出力開始
        /// </summary>
        public void StartLogging() {
            this.StartLogging(string.Empty, LogModeConstants.vbLogAuto);
        }

        /// <summary>
        /// 操作のログ ターゲットおよびログ モードを設定するために使います。
        /// </summary>
        /// <param name="logTarget">LogEvent メソッドの出力を記録するファイルのパスおよびファイル名を指定します。</param>
        public void StartLogging(string logTarget) {
            this.StartLogging(logTarget, LogModeConstants.vbLogAuto);
        }

        /// <summary>
        /// 操作のログ ターゲットおよびログ モードを設定するために使います。
        /// </summary>
        /// <param name="logMode">LogEvent メソッドによるログの記録方法を指定します。</param>
        public void StartLogging(LogModeConstants logMode) {
            this.StartLogging(string.Empty, logMode);
        }

        /// <summary>
        /// 操作のログ ターゲットおよびログ モードを設定するために使います。
        /// </summary>
        /// <param name="logTarget">LogEvent メソッドの出力を記録するファイルのパスおよびファイル名を指定します。</param>
        /// <param name="logMode">LogEvent メソッドによるログの記録方法を指定します。</param>
        public void StartLogging(string logTarget, LogModeConstants logMode) {
            // LogPath を先に設定
            this.LogPath = logTarget;
            this.LogMode = logMode;

        }

        /// <summary>
        /// アプリケーションのログ ターゲットでのイベントを記録します。
        /// </summary>
        /// <param name="logBuffer">ログに書き込む文字列を指定します。</param>
        /// <param name="eventType">イベントの種類を表す値を指定します。</param>
        public void LogEvent(string logBuffer, LogEventTypeConstants eventType) {
            string Header = null;

            // ﾍｯﾀﾞに ThreadID を付加

            if (m_logThreadID) {
                Header = string.Format(CultureInfo.CurrentCulture, "[T:{0:0000}] ", this.ThreadID);
            } else {
                Header = string.Empty;
            }

            if (m_logToFile) {
                // ﾌｧｲﾙに書き込み
                using (StreamWriter logFile = new StreamWriter(m_LogPath, true)) {
                    logFile.WriteLine(Header + logBuffer);
                }
            }

            if (m_logToNT) {
                // Source が無ければ作成
                if (!EventLog.SourceExists(this.Title)) {
                    EventLog.CreateEventSource(this.Title, "Application");
                }

                // VB6.0 の EventType を変換
                EventLogEntryType logEventType = default(EventLogEntryType);
                switch (eventType) {
                    case LogEventTypeConstants.vbLogEventTypeError:
                        logEventType = EventLogEntryType.Error;
                        break;
                    case LogEventTypeConstants.vbLogEventTypeInformation:
                        logEventType = EventLogEntryType.Information;
                        break;
                    case LogEventTypeConstants.vbLogEventTypeWarning:
                        logEventType = EventLogEntryType.Warning;
                        break;
                }

                // ｲﾍﾞﾝﾄﾛｸﾞに書き込み
                EventLog.WriteEntry(this.Title, Header + logBuffer, logEventType);
            }
        }

        private static bool PrevInstanceInternal {
            get {
                bool result = false;
                using (Process current = Process.GetCurrentProcess()) {
                    // 自分のファイル名とユーザーの SID を取得
                    string currentFileName = current.MainModule.FileName;
                    var currentSid = GetTokenUserSid(current);

                    foreach (Process p in Process.GetProcessesByName(current.ProcessName)) {
                        // result = true になったら処理しない。Dispose だけ行う
                        if (!result) {
                            // 自分と違うプロセスIDでファイル名とアカウントが同一なら PrevInstance は True
                            if (p.Id != current.Id) {
                                // プロセスが終了していると Process.MainModule や Process.Handle がこけるので try catch
                                try {
                                    // ファイル名を比較
                                    if (string.Compare(p.MainModule.FileName, currentFileName, true) == 0) {
                                        // ユーザの SID を比較
                                        var otherSid = GetTokenUserSid(p);
                                        if (otherSid != null && otherSid.Equals(currentSid)) {
                                            result = true;
                                        }
                                    }
                                } catch {
                                } finally {
                                }
                            }
                        }
                        p.Dispose();
                    }
                }
                return result;
            }
        }

        private static SecurityIdentifier GetTokenUserSid(Process process) {
            SecurityIdentifier result = null;
            using (var hToken = NativeMethods.OpenProcessToken(process.Handle, NativeMethods.TOKEN_QUERY)) {
                using (var tokenUserBuffer = NativeMethods.GetTokenInformation(
                                                        hToken,
                                                        NativeMethods.TOKEN_INFORMATION_CLASS.TokenUser)) {
                    unsafe {
                        var ts = (NativeMethods.TOKEN_USER*)(IntPtr)tokenUserBuffer;
                        result = new SecurityIdentifier(ts->User.Sid);
                    }
                }
            }
            return result;
        }

        private static bool GetAccountFromPID(int pid, out string domainName, out string accountName) {
            domainName = string.Empty;
            accountName = string.Empty;
            bool result = false;
            IntPtr hProcess = NativeMethods.OpenProcess(
                                    NativeMethods.PROCESS_QUERY_INFORMATION,
                                    false,
                                    pid);
            if (hProcess != IntPtr.Zero) {
                if (GetAccountFromHProcess(hProcess, out domainName, out accountName)) {
                    result = true;
                }
                NativeMethods.CloseHandle(hProcess);
            }
            return result;
        }

        private static bool GetAccountFromHProcess(IntPtr hProcess, out string domainName, out string accountName) {
            bool result = false;
            domainName = string.Empty;
            accountName = string.Empty;

            using (var token = NativeMethods.OpenProcessToken(hProcess, NativeMethods.TOKEN_QUERY)) {
                using (var tokenUserBuffer = NativeMethods.GetTokenInformation(
                                                        token,
                                                        NativeMethods.TOKEN_INFORMATION_CLASS.TokenUser)) {
                    unsafe {
                        var ts = (NativeMethods.TOKEN_USER*)(IntPtr)tokenUserBuffer;
                        StringBuilder szAccountName = new StringBuilder(1024);
                        StringBuilder szDomainName = new StringBuilder(1024);
                        int dwAccountSize = szAccountName.Capacity;
                        int dwDomainSize = szDomainName.Capacity;
                        NativeMethods.SID_NAME_USE snu;
                        if (NativeMethods.LookupAccountSid(
                                                    null,
                                                    ts->User.Sid,
                                                    szAccountName,
                                                    ref dwAccountSize,
                                                    szDomainName,
                                                    ref dwDomainSize,
                                                    out snu)) {
                            domainName = szDomainName.ToString();
                            accountName = szAccountName.ToString();
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        #region Dispose 処理

        bool _disposedValue = false;

        /// <summary>
        /// Dispose 処理で呼ばれるプロテクトメソッド
        /// </summary>
        /// <param name="disposing">
        /// Dispose メソッドから呼び出されたときは True。
        /// デストラクタから呼び出されたときは False。
        /// </param>
        protected virtual void Dispose(bool disposing) {
            if (!(this._disposedValue)) {
                this._disposedValue = true;
                if (disposing) {
                    // マネージオブジェクトを解放する
                }
                // 共有オブジェクトを解放する
            }
        }

        /// <summary>
        /// アンマネージ リソースの解放およびリセットに関連付けられているアプリケーション定義のタスクを実行します。
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this); // Finalize 抑止
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~VBApp() {
            //Debug.WriteLine("Finalize " + this.GetType().Name);
            Dispose(false);
        }

        #endregion

        /// <summary>
        /// このインスタンスの値を System.String に変換します。
        /// </summary>
        /// <returns>このインスタンスと同じ値の文字列。</returns>
        public override string ToString() {
            return EXEName;
        }

        /// <summary>
        /// アプリケーションをVB6.0 互換モードで開始します。
        /// すべてのウインドウが閉じたとき終了します。
        /// </summary>
        public static void Run() {
            Run(MainForm);
        }

        /// <summary>
        /// アプリケーションをVB6.0 互換モードで開始します。
        /// すべてのウインドウが閉じたとき終了します。
        /// </summary>
        /// <param name="mainForm">メインフォームのインスタンスを指定します。</param>
        public static void Run(Form mainForm) {
            var myApp = new MyApplication(mainForm);
            myApp.Run(Environment.GetCommandLineArgs());
        }

        private static Form _MainForm;

        /// <summary>
        /// アプリケーションのメインウインドウのインスタンスを指定します。
        /// </summary>
        public static Form MainForm {
            get {
                return _MainForm;
            }
            set {
                _MainForm = value;
            }
        }

        class MyApplication : WindowsFormsApplicationBase
        {
            public MyApplication(Form mainForm)
                : base(AuthenticationMode.Windows) {
                base.ShutdownStyle = ShutdownMode.AfterAllFormsClose;
                if (mainForm != null) {
                    base.MainForm = mainForm;
                }
            }

            protected override void OnCreateMainForm() {
                if (base.MainForm == null) {
                    IntPtr hwnd = NativeMethods.GetActiveWindow();
                    Form activeForm = Control.FromHandle(hwnd) as Form;
                    if (activeForm != null) {
                        base.MainForm = activeForm;
                    } else {
                        foreach (Form f in OpenForms) {
                            if (!f.InvokeRequired) {
                                base.MainForm = f;
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// アプリケーションで visual スタイルを有効にします。
        /// </summary>
        public static void EnableVisualStyles() {
            Application.EnableVisualStyles();
        }

        /// <summary>
        /// 特定のコントロールで定義された UseCompatibleTextRendering プロパティにアプリケーション全体で有効な既定値を設定します。
        /// </summary>
        /// <param name="defaultValue">
        /// 新しいコントロールに適用する既定値。
        /// true の場合、UseCompatibleTextRendering をサポートする新しいコントロールは、テキストレンダリングに GDI+ ベースの System.Drawing.Graphics クラスを使用します。
        /// false の場合、新しいコントロールは GDI ベースの System.Windows.Forms.TextRenderer クラスを使用します
        /// </param>
        /// <exception cref="System.InvalidOperationException">
        /// このメソッドは、Windows フォーム アプリケーションによって最初のウィンドウが作成される前にしか呼び出すことができません。
        /// </exception>
        public static void SetCompatibleTextRenderingDefault(bool defaultValue) {
            Application.SetCompatibleTextRenderingDefault(defaultValue);
        }
    }
}
