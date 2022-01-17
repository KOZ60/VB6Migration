using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Scripting
{
    /// <summary>
    /// 未サポートです。使えません。
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("System.Text.Encoding を使用してください。",true)]
    public class Encoder
    {
        /// <summary>
        /// 未サポートです。使えません。
        /// </summary>
        public Encoder()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 未サポートです。使えません。
        /// </summary>
        /// <param name="szExt"></param>
        /// <param name="bstrStreamIn"></param>
        /// <param name="cFlags"></param>
        /// <param name="bstrDefaultLang"></param>
        /// <returns></returns>
        public string EncodeScriptFile([In] string szExt, [In] string bstrStreamIn, [In] int cFlags, [In] string bstrDefaultLang)
        {
            throw new System.NotImplementedException();
        }
    }
}
