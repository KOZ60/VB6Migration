using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    /// <summary>
    /// レコードセットを開くときに使用されるレコード ロックの種類を指定します。
    /// </summary>
    public enum LockTypeEnum
    {
        /// <summary>
        /// レコード値に基づく共有的同時ロック。カーソルは古いレコードと新しいレコードのデータ値を比較し、そのレコードへのアクセスが最後に行われてから変更が加えられたかどうか判断します (ODBCDirect ワークスペースのみ)。
        /// </summary>
        dbOptimisticValue = 1,
        /// <summary>
        /// 排他的同時ロック。カーソルは、レコードが更新可能であることを保証するために必要な最低限のロックを使用します。
        /// </summary>
        dbPessimistic = 2,
        /// <summary>
        /// レコード ID に基づく共有的同時ロック。カーソルは古いレコードと新しいレコードのレコード ID を比較し、そのレコードへのアクセスが最後に行われてから変更が加えられたかどうか判断します。
        /// </summary>
        dbOptimistic = 3,
        /// <summary>
        /// 読み取り専用
        /// </summary>
        dbReadOnly = 4,
        /// <summary>
        /// 共有的バッチ更新を可能にします (ODBCDirect ワークスペースのみ)。
        /// </summary>
        dbOptimisticBatch = 5
    }
}
