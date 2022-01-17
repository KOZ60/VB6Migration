using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace DAO
{
    /// <summary>
    /// DAO.Workspaces 互換クラス
    /// </summary>
    public class Workspaces : NameObjectCollectionBase
    {
        private DBEngine _DBEngine;
        private const string DefaultNamespace = "#0";

        internal Workspaces(DBEngine engine) {
            _DBEngine = engine;
            base.BaseAdd(DefaultNamespace, new Workspace(DBEngine, DefaultNamespace));
        }

        internal DBEngine DBEngine {
            get {
                return _DBEngine;
            }
        }

        /// <summary>
        /// インデックスを指定して Workspace オブジェクトを取り出します。
        /// </summary>
        /// <param name="index">取り出す Workspace オブジェクトのインデックス</param>
        /// <returns>指定したインデックスの Workspace オブジェクト</returns>
        public Workspace this[int index] {
            get {
                return (Workspace)base.BaseGet(index);
            }
        }

        /// <summary>
        /// キーを指定して Workspace オブジェクトを取り出します。
        /// </summary>
        /// <param name="name">取り出す Workspace オブジェクトのキー</param>
        /// <returns>指定したキーの Workspace オブジェクト</returns>
        public Workspace this[string name] {
            get {
                return (Workspace)base.BaseGet(name);
            }
        }

        /// <summary>
        /// Workspace オブジェクトを追加します。
        /// </summary>
        /// <param name="name">追加する WorkSpae オブジェクトのキー</param>
        public void Append(string name) {
            base.BaseAdd(name, new Workspace(DBEngine, name));
        }

        internal void Remove(Workspace item) {
            if (base.BaseGet(item.Name) != null) {
                base.BaseRemove(item.Name);
            }
        }
    }
}
