using System;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OracleInProcServer.Core;

namespace OracleInProcServer
{
    public static class OraClient
    {
        private static OracleProviderNameDictionary _ProviderNames;
        private static OracleProviderTypes _ProviderType;
        private static string _ProviderName;
        private static DbProviderFactory factory;

        static OraClient() {
            ProviderType = OracleProviderTypes.Unmanaged;
        }

        private class OracleProviderNameDictionary : ConvertDictionary<OracleProviderTypes, string>
        {
            public OracleProviderNameDictionary() {
                Add(OracleProviderTypes.Unmanaged, "Oracle.DataAccess.Client");
                Add(OracleProviderTypes.Managed, "Oracle.ManagedDataAccess.Client");
            }

            protected override string CashOut(OracleProviderTypes key) {
                throw new NotSupportedException(key.ToString() + " はサポート外です。");
            }
        }

        public static IReadOnlyDictionary<OracleProviderTypes, string>  ProviderNames {
            get {
                if (_ProviderNames == null) {
                    _ProviderNames = new OracleProviderNameDictionary();
                }
                return _ProviderNames;
            }
        }

        public static OracleProviderTypes ProviderType {
            get {
                foreach (var kp in ProviderNames) {
                    if (string.Compare(ProviderName, kp.Value, true) == 0) {
                        return kp.Key;
                    }
                }
                return OracleProviderTypes.Unknown;
            }
            set {
                ProviderName = ProviderNames[value];
                _ProviderType = value;
            }
        }

        public static string ProviderName {
            get {
                return _ProviderName;
            }
            set {
                if (factory != null) {
                    throw new InvalidOperationException("データベースを開いた後は変更できません。");
                }
                _ProviderName = value;
            }
        }

        public static DbProviderFactory Factory {
            get {
                if (factory == null) {
                    factory = DbProviderFactories.GetFactory(ProviderName);
                }
                return factory;
            }
        }
    }
}
