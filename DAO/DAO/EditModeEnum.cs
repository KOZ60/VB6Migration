using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    /// <summary>
    /// カレントレコードの編集ステータスを表します。
    /// </summary>
    public enum EditModeEnum
    {
        /// <summary>
        /// 編集の操作は行われていません。
        /// </summary>
        dbEditNone = 0,
        /// <summary>
        /// Edit メソッドが呼び出されています。カレント レコードはコピー バッファにあります。
        /// </summary>
        dbEditInProgress = 1,
        /// <summary>
        /// AddNew メソッドが呼び出されています。コピー バッファのカレント レコードはデータベースに保存されていない新しいレコードです。
        /// </summary>
        dbEditAdd = 2,
    }
}
