using System.Reflection;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace VBCompatible
{
    /// <summary>
    ///  VB6.0互換 Global モジュール
    /// </summary>
    [StandardModule]
    public static class VBGlobal
    {
        /// <summary>
        /// VB6.0 互換 App オブジェクト
        /// </summary>
        public static VBApp App {
            get {
                return VBApp.GetDefaultInstance(Assembly.GetCallingAssembly());
            }
        }

        /// <summary>
        /// VB6.0 互換 Forms コレクション
        /// </summary>
        public static FormCollection Forms {
            get {
                return Application.OpenForms;
            }
        }

        /// <summary>
        /// デザイン中かどうかを返します。
        /// </summary>
        public static bool DesignMode {
            get {
                return (Assembly.GetEntryAssembly() == null);
            }
        }
    }
}
