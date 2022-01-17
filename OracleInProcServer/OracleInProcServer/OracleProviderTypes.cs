using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleInProcServer
{
    /// <summary>
    /// 使用する ODP.NET を指定します。
    /// </summary>
    public enum OracleProviderTypes
    {
        /// <summary>
        /// 管理対象外(Unmanaged)ドライバ
        /// </summary>
        Unmanaged,
        /// <summary>
        /// 管理対象(Managed)ドライバ
        /// </summary>
        Managed,
        /// <summary>
        /// 未知のドライバ
        /// </summary>
        Unknown,
        
    }
}
