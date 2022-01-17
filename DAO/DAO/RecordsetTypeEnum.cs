using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    /// <summary>
    /// Recordset の種類を示す定数
    /// </summary>
    public enum RecordsetTypeEnum
    {
        /// <summary>テーブル タイプの Recordset オブジェクト</summary>
        dbOpenTable = 1,
        /// <summary>ダイナセット タイプの Recordset オブジェクト</summary>
        dbOpenDynaset = 2,
        /// <summary>スナップショット タイプの Recordset オブジェクト</summary>
        dbOpenSnapshot = 4,
        /// <summary>前方スクロール タイプの Recordset</summary>
        dbOpenForwardOnly = 8,
        /// <summary>動的 タイプの Recordset オブジェクト</summary>
        dbOpenDynamic = 16
    }
}
