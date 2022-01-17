using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    /// <summary>
    /// Recordset 内のレコード ポインタの現在の位置を表します。
    /// </summary>
    public enum PositionEnum
    {
        /// <summary>
        /// カレント レコードのポインタが EOF にあることを表します (EOF プロパティが True です)。
        /// </summary>
        dbPosEOF = -3,
        /// <summary>
        /// カレント レコードのポインタが BOF にあることを表します (BOF プロパティが True です)。
        /// </summary>
        dbPosBOF = -2,
        /// <summary>
        /// Recordset が空である、現在の位置が不明、あるいはプロバイダが AbsolutePage プロパティまたは AbsolutePosition プロパティをサポートしていません。
        /// </summary>
        dbPosUnknown = -1
    }
}
