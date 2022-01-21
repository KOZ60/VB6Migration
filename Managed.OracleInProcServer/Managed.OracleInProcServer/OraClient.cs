using Oracle.ManagedDataAccess.Client;

namespace Managed.OracleInProcServer
{
    public static class OraClient
    {
        public static OracleProviderTypes ProviderType {
            get {
                return OracleProviderTypes.Unmanaged;
            }
        }

        public static OracleClientFactory Factory {
            get {
                return OracleClientFactory.Instance;
            }
        }
    }
}
