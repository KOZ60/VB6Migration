using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    /// <summary>
    /// DAO.DBEngine 互換クラス
    /// </summary>
    public class DBEngine
    {

        private DBEngineVersion _Version = DBEngineVersion.VER351;
        private Workspaces _Workspaces;

        /// <summary>
        /// 内部的に使用するプロバイダを設定および取得します。
        /// </summary>
        public DBEngineVersion Version {
            get {
                return _Version;
            }
            set {
                _Version = value;
            }
        }

        internal String Provider {
            get {
                switch (Version) {
                    case DBEngineVersion.VER351:
                        return @"Microsoft.Jet.OLEDB.3.51";
                    case DBEngineVersion.VER400:
                        return @"Microsoft.Jet.OLEDB.4.0";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Workspace オブジェクトのコレクションを返します。
        /// </summary>
        public Workspaces Workspaces {
            get {
                if (_Workspaces == null) {
                    _Workspaces = new Workspaces(this);
                }
                return _Workspaces;
            }
        }

    }
}
