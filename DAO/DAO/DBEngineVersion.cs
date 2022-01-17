using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    /// <summary>
    /// DAO のバージョンを指定します。
    /// </summary>
    public enum DBEngineVersion
    {
        /// <summary>
        /// Microsoft.Jet.OLEDB.3.51 を使用してデータベースの操作を行います。
        /// </summary>
        VER351,
        /// <summary>
        /// Microsoft.Jet.OLEDB.4.00 を使用してデータベースの操作を行います。
        /// </summary>
        VER400
    }
}
