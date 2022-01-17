using Microsoft.VisualBasic.CompilerServices;

namespace VBCompatible
{
    /// <summary>
    /// VB6.0 _HiddenModule
    /// </summary>
    [StandardModule]
    public class VBHiddenModule
    {
        /// <summary>
        /// オブジェクトのアドレスを取得します。
        /// </summary>
        /// <typeparam name="T">オブジェクトの型。</typeparam>
        /// <param name="obj">対象のオブジェクトを指定します。</param>
        /// <returns>指定したオブジェクトのアドレスを VBSafePointer で返します。</returns>
        public static VBSafePointer ObjPtr<T>(T obj)
            where T : class {
            return new VBSafePointer(obj);
        }

        /// <summary>
        /// 構造体をボックス化し、そのアドレスを取得します。
        /// </summary>
        /// <typeparam name="T">構造体の型。</typeparam>
        /// <param name="obj">対象となる構造体を指定します。</param>
        /// <returns>ボックス化された変数のアドレスを VBSafePointer で返します。</returns>
        /// <remarks>VBSafePointer.Target プロパティを参照すると API が変更した値を知ることができます。</remarks>
        public static VBSafePointer VarPtr<T>(ref T obj)
                where T : struct {
            return new VBSafePointer(obj);
        }

        /// <summary>
        /// 文字列のアドレスを取得します。
        /// </summary>
        /// <param name="str">アドレスを取得する文字列。</param>
        /// <returns>指定した文字列のアドレスを VBSafePointer で返します。</returns>
        /// <remarks>
        /// 文字列には新しいインスタンスが設定されます。
        /// </remarks>
        public static VBSafePointer StrPtr(ref string str) {
            str = new string(str.ToCharArray());
            return new VBSafePointer(str);
        }
    }
}
