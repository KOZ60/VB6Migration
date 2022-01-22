using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace VBCompatible
{
    public static class VBUtils
    {
        public const int STATE_CREATED = 0x00000001;
        public const int STATE_VISIBLE = 0x00000002;
        public const int STATE_ENABLED = 0x00000004;
        public const int STATE_TABSTOP = 0x00000008;
        public const int STATE_RECREATE = 0x00000010;
        public const int STATE_MODAL = 0x00000020;
        public const int STATE_ALLOWDROP = 0x00000040;
        public const int STATE_DROPTARGET = 0x00000080;
        public const int STATE_NOZORDER = 0x00000100;
        public const int STATE_LAYOUTDEFERRED = 0x00000200;
        public const int STATE_USEWAITCURSOR = 0x00000400;
        public const int STATE_DISPOSED = 0x00000800;
        public const int STATE_DISPOSING = 0x00001000;
        public const int STATE_MOUSEENTERPENDING = 0x00002000;
        public const int STATE_TRACKINGMOUSEEVENT = 0x00004000;
        public const int STATE_THREADMARSHALLPENDING = 0x00008000;
        public const int STATE_SIZELOCKEDBYOS = 0x00010000;
        public const int STATE_CAUSESVALIDATION = 0x00020000;
        public const int STATE_CREATINGHANDLE = 0x00040000;
        public const int STATE_TOPLEVEL = 0x00080000;
        public const int STATE_ISACCESSIBLE = 0x00100000;
        public const int STATE_OWNCTLBRUSH = 0x00200000;
        public const int STATE_EXCEPTIONWHILEPAINTING = 0x00400000;
        public const int STATE_LAYOUTISDIRTY = 0x00800000;
        public const int STATE_CHECKEDHOST = 0x01000000;
        public const int STATE_HOSTEDINDIALOG = 0x02000000;
        public const int STATE_DOUBLECLICKFIRED = 0x04000000;
        public const int STATE_MOUSEPRESSED = 0x08000000;
        public const int STATE_VALIDATIONCANCELLED = 0x10000000;
        public const int STATE_PARENTRECREATING = 0x20000000;
        public const int STATE_MIRRORED = 0x40000000;

        private static readonly MethodInfo GetStyleMethodInfo =
                    typeof(Control).GetMethod("GetStyle", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly MethodInfo SetStyleMethodInfo =
                    typeof(Control).GetMethod("SetStyle", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly MethodInfo GetStateMethodInfo =
                    typeof(Control).GetMethod("GetState", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly MethodInfo SetStateMethodInfo =
                    typeof(Control).GetMethod("SetState", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly FieldInfo EventEnabledFieldInfo =
                    typeof(Control).GetField("EventEnabled", BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly PropertyInfo EventsPropertyInfo =
                    typeof(Component).GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly object EventEnabled = EventEnabledFieldInfo.GetValue(null);

        /// <summary>
        /// 指定したコントロールの指定したコントロール スタイル ビットの値を取得します。
        /// </summary>
        /// <param name="con">コントロール スタイル ビットの値を取得するコントロール。</param>
        /// <param name="styles">値を返す ControlStyles ビット。</param>
        /// <returns></returns>
        public static bool GetStyle(this Control con, ControlStyles styles) {
            return (bool)GetStyleMethodInfo.Invoke(con, new object[] { styles });
        }

        /// <summary>
        /// 指定したコントロールの指定したコントロール スタイル ビットの値を設定します。
        /// </summary>
        /// <param name="con">コントロール スタイル ビットの値を設定するコントロール</param>
        /// <param name="styles">設定する ControlStyles ビット。</param>
        /// <param name="value">指定したスタイルをコントロールに適用する場合は true。それ以外の場合は false。</param>
        public static void SetStyle(this Control con, ControlStyles styles, bool value) {
            SetStyleMethodInfo.Invoke(con, new object[] { styles, value });
        }

        /// <summary>
        /// 指定したコントロールの状態から指定したビットの値を取得します。
        /// </summary>
        /// <param name="con">ビットの値を取得するコントロール</param>
        /// <param name="flags">ビット位置を指定します。</param>
        public static bool GetState(this Control con, int flags) {
            return (bool)GetStateMethodInfo.Invoke(con, new object[] { flags });
        }

        /// <summary>
        /// 指定したコントロールの状態から指定したビットの値を設定します。
        /// </summary>
        /// <param name="con">ビットの値を取得するコントロール</param>
        /// <param name="flags">ビット位置を指定します。</param>
        /// <param name="value">設定する値。</param>
        public static void SetState(this Control con, int flags, bool value) {
            SetStateMethodInfo.Invoke(con, new object[] { flags, value });
        }

        /// <summary>
        /// コントロール自身が管理している Enabled プロパティの値を取得します。
        /// </summary>
        /// <remarks>
        /// コントロールの Enabled プロパティは親が false なら false を返します。
        /// このメソッドはコントロール自身が管理している値を取得します。
        /// </remarks>
        /// <param name="con">取得するコントロール。</param>
        public static bool IsEnabled(this Control con) {
            return GetState(con, STATE_ENABLED);
        }

        /// <summary>
        /// 指定したコントロールが選択可能かどうかを取得します。
        /// CanSelect プロパティと違い、Visible/Enabled プロパティの影響を受けません。
        /// </summary>
        /// <param name="con">選択可能かどうかを取得するコントロール</param>
        /// <returns>選択可能なら true、そうでなければ false が返ります。</returns>
        public static bool IsSelectable(this Control con) {
            // Form や TabControl 等のコンテナは 選択可能だが便宜上選択不可として扱う
            if (con is Form || con is TabControl || con is Panel) {
                return false;
            }
            return GetStyle(con, ControlStyles.Selectable);
        }

        /// <summary>
        /// 指定したコントロールのフォントハンドルを取得します。
        /// </summary>
        /// <param name="con">フォントハンドルを取得するコントロール。</param>
        public static IntPtr GetFontHandle(this Control con) {
            return NativeMethods.SendMessage(con.Handle,
                                    NativeMethods.WM_GETFONT, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// オブジェクトをディープコピーします。
        /// </summary>
        /// <typeparam name="T">オブジェクトの型。</typeparam>
        /// <param name="target">ディープコピーするオブジェクト</param>
        /// <returns>ディープコピーされたオブジェクト</returns>
        /// <remarks>
        /// ディープコピーするオブジェクトは、SerializableAttribute 属性でマークされている必要があります。
        /// </remarks>
        /// <example>
        /// <code>
        /// &lt;Serializable&gt;
        /// Public Structure S1
        ///     Public Value As String
        /// End Structure
        /// 
        /// &lt;Serializable&gt;
        /// Public Structure S2
        ///     Public Values() As S1
        ///     
        ///     Public Sub Initialize()
        ///         ReDim Values(1)
        ///     End Sub    
        /// End Structure
        /// 
        /// Public Sub Main()
        ///     Dim d1(1) As S2
        ///     For i As Integer = 0 To d1.Length - 1
        ///         Dim v2 As New S2
        ///         v2.Initialize()
        ///         v2.Values(0).Value = "0"
        ///         v2.Values(1).Value = "1"
        ///         d1(i) = v2
        ///     Next
        ///     
        ///     Dim d2() As S2 = d1.DeepCopy()
        ///     
        ///     For i As Integer = 0 To d2.Length - 1
        ///         d2(i).Values(0).Value = "2"
        ///         d2(i).Values(1).Value = "3"
        ///     Next
        /// End Sub
        /// </code>
        /// </example>
        public static T DeepCopy<T>(this T target) {
            object result;
            BinaryFormatter b = new BinaryFormatter();
            MemoryStream mem = new MemoryStream();
            try {
                b.Serialize(mem, target);
                mem.Position = 0;
                result = b.Deserialize(mem);
            } catch {
                ICloneable cloneValue = target as ICloneable;
                if (cloneValue != null)
                    result = cloneValue.Clone();
                else
                    result = cloneValue;
            } finally {
                if (mem != null) mem.Dispose();
            }
            return (T)result;
        }

        public static void GetSelect(this TextBox box, out int start, out int end) {
            GetSelect(box.Handle, out start, out end);
        }

        public static void GetSelect(IntPtr editBoxHandle, out int start, out int end) {
            NativeMethods.SendMessage(
                             editBoxHandle,
                             NativeMethods.EM_GETSEL,
                             out start, out end);
        }

        public static Color ToArgbColor(this Color color) {
            int win32Color = ColorTranslator.ToWin32(color);
            return ColorTranslator.FromWin32(win32Color);
        }

        public static bool IsCtlColor(ref Message m) {
            switch (m.Msg) {
                case NativeMethods.WM_CTLCOLOR:
                case NativeMethods.WM_CTLCOLORBTN:
                case NativeMethods.WM_CTLCOLORDLG:
                case NativeMethods.WM_CTLCOLORMSGBOX:
                case NativeMethods.WM_CTLCOLORSCROLLBAR:
                case NativeMethods.WM_CTLCOLOREDIT:
                case NativeMethods.WM_CTLCOLORLISTBOX:
                case NativeMethods.WM_CTLCOLORSTATIC:

                case NativeMethods.WM_REFLECT + NativeMethods.WM_CTLCOLOR:
                case NativeMethods.WM_REFLECT + NativeMethods.WM_CTLCOLORBTN:
                case NativeMethods.WM_REFLECT + NativeMethods.WM_CTLCOLORDLG:
                case NativeMethods.WM_REFLECT + NativeMethods.WM_CTLCOLORMSGBOX:
                case NativeMethods.WM_REFLECT + NativeMethods.WM_CTLCOLORSCROLLBAR:
                case NativeMethods.WM_REFLECT + NativeMethods.WM_CTLCOLOREDIT:
                case NativeMethods.WM_REFLECT + NativeMethods.WM_CTLCOLORLISTBOX:
                case NativeMethods.WM_REFLECT + NativeMethods.WM_CTLCOLORSTATIC:
                    return true;
            }
            return false;
        }

        public static void SetCtlColor(Control con, ref Message m) {
            if (!con.GetStyle(ControlStyles.UserPaint)) {
                IntPtr hDC = m.WParam;

                Color foreColor = con.IsEnabled() ? con.ForeColor : SystemColors.GrayText;
                Color backColor = con.BackColor;

                int intForeColor = ColorTranslator.ToWin32(foreColor);
                int intBackColor = ColorTranslator.ToWin32(backColor);

                NativeMethods.SetTextColor(hDC, intForeColor);
                NativeMethods.SetBkColor(hDC, intBackColor);

                m.Result = VBGraphicsCache.GetNativeBrush(backColor);
            } else {
                m.Result = NativeMethods.GetStockObject(NativeMethods.StockObjects.HOLLOW_BRUSH);
            }
        }

        // コントロールの親を含めて Enabled プロパティをチェック
        // 階層上にひとつでも Enabled プロパティ = False のものがあれば、そのコントロールは Enabled = False
        public static bool GetControlEnabledHierarchy(Control con) {
            if (con == null) return true;
            if (!GetControlEnabled(con)) return false;
            return GetControlEnabledHierarchy(con.Parent);
        }

        // Control にキャストしたときの値と違う場合があるため
        // Reflection を使って Enabled プロパティを取得
        private static bool GetControlEnabled(Control con) {
            PropertyInfo pi = con.GetType().GetProperty("Enabled");
            if (pi != null)
                return (bool)pi.GetValue(con);
            else
                return con.Enabled;
        }

        public static bool GetAnyDisposingInHierarchy(this Control control) {
            Control up = control;
            bool isDisposing = false;
            while (up != null) {
                if (up.Disposing) {
                    isDisposing = true;
                    break;
                }
                up = up.Parent;
            }
            return isDisposing;
        }

        public static EventHandlerList Events(Component component) {
            return (EventHandlerList)EventsPropertyInfo.GetValue(component);
        }

        public static void OnEnabledChanged(Control control, EventArgs e) {
            if (GetAnyDisposingInHierarchy(control)) {
                return;
            }
            if (control.IsHandleCreated) {
                NativeMethods.EnableWindow(control.Handle, control.IsEnabled());
                control.Invalidate();
                control.Update();
            }
            EventHandler eh = Events(control)[EventEnabled] as EventHandler;
            if (eh != null) {
                eh(control, e);
            }
        }

        private const BindingFlags BindingFlag = BindingFlags.Instance | BindingFlags.Static
                                                | BindingFlags.Public | BindingFlags.NonPublic;

        public static MethodInfo GetMethodInfo(this Type type, string name, params Type[] args) {
            return type.GetMethod(name, BindingFlag, null, args, null);
        }

        public static PropertyInfo GetPropertyInfo(this Type type, string name) {
            return type.GetProperty(name, BindingFlag);
        }

        public static T CreateDelegate<T>(this MethodInfo mi, object instance) where T : Delegate {
            return (T)Delegate.CreateDelegate(typeof(T), instance, mi);
        }
    }
}
