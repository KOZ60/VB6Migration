using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Types;

namespace Managed.OracleInProcServer.Core
{
    internal class OracleTypes
    {
        static OracleTypes() {
            ColumnTypes = new HashSet<Type>();
            ColumnTypes.Add(typeof(OracleBFile));
            ColumnTypes.Add(typeof(OracleBinary));
            ColumnTypes.Add(typeof(OracleBlob));
            ColumnTypes.Add(typeof(OracleClob));
            ColumnTypes.Add(typeof(OracleDate));
            ColumnTypes.Add(typeof(OracleDecimal));
            ColumnTypes.Add(typeof(OracleIntervalDS));
            ColumnTypes.Add(typeof(OracleIntervalYM));
            //ColumnTypes.Add(typeof(OracleRef));
            ColumnTypes.Add(typeof(OracleRefCursor));
            ColumnTypes.Add(typeof(OracleString));
            ColumnTypes.Add(typeof(OracleTimeStamp));
            ColumnTypes.Add(typeof(OracleTimeStampLTZ));
            ColumnTypes.Add(typeof(OracleTimeStampTZ));
        }

        public static HashSet<Type> ColumnTypes;
    }
}
