using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    /// <summary>
    /// トランザクションをサポートするクラスの抽象クラス
    /// </summary>
    public abstract class ConnectionBase : DaoObjectBase
    {
        /// <summary>
        /// トランザクションを開始します。
        /// </summary>
        public abstract void BeginTrans();

        /// <summary>
        /// トランザクションをコミットします。
        /// </summary>
        public abstract void CommitTrans();

        /// <summary>
        /// トランザクションをロールバックします。
        /// </summary>
        public abstract void Rollback();

    }
}
