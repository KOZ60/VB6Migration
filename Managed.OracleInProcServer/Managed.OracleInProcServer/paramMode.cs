using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Managed.OracleInProcServer
{
    /// <summary>
    /// SQL文とPL/SQLブロック内でのパラメータの使用方法を指定する整数コード
    /// </summary>
    [Flags]
    public enum paramMode
    {
        /// <summary>
        /// 入力変数にのみ使用
        /// </summary>
        ORAPARM_INPUT = 1,
        /// <summary>
        /// 出力変数にのみ使用
        /// </summary>
        ORAPARM_OUTPUT = 2,
        /// <summary>
        /// 入力変数と出力変数の両方に使用
        /// </summary>
        ORAPARM_BOTH = 3,
    }
}
