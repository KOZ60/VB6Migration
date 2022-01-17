using System;
using System.Diagnostics;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace OracleInProcServer.Core
{
    internal class OracleTypes
    {
        private class GetOracleTypeHelper
        {
            Assembly asm = Assembly.GetAssembly(OraClient.Factory.GetType());

            public GetOracleTypeHelper() {
                asm = Assembly.GetAssembly(OraClient.Factory.GetType());
            }

            public Type GetOracleType(string key) {
                return GetOracleType(key, OracleNamespace.Types);
            }

            public Type GetOracleType(string key, OracleNamespace oracleNamespace) {
                Type result = null;

                switch (OraClient.ProviderType) {
                    case OracleProviderTypes.Managed:
                        result = asm.GetType($"Oracle.ManagedDataAccess.{oracleNamespace}.{key}", false, true);
                        break;

                    case OracleProviderTypes.Unmanaged:
                        result = asm.GetType($"Oracle.DataAccess.{oracleNamespace}.{key}", false, true);
                        break;
                    default:
                        result = null;
                        break;
                }
                return result;
            }
        }

        private enum OracleNamespace
        {
            Client,
            Types,
        }


        static OracleTypes() {
            var helper = new GetOracleTypeHelper();

            OracleParameterStatus = helper.GetOracleType("OracleParameterStatus", OracleNamespace.Client);

            OracleBFile = helper.GetOracleType("OracleBFile");
            OracleBinary = helper.GetOracleType("OracleBinary");
            OracleBlob = helper.GetOracleType("OracleBlob");
            OracleClob = helper.GetOracleType("OracleClob");
            OracleDate = helper.GetOracleType("OracleDate");
            OracleDecimal = helper.GetOracleType("OracleDecimal");
            OracleIntervalDS = helper.GetOracleType("OracleIntervalDS");
            OracleIntervalYM = helper.GetOracleType("OracleIntervalYM");
            OracleRef = helper.GetOracleType("OracleRef");
            OracleRefCursor = helper.GetOracleType("OracleRefCursor");
            OracleString = helper.GetOracleType("OracleString");
            OracleTimeStamp = helper.GetOracleType("OracleTimeStamp");
            OracleTimeStampLTZ = helper.GetOracleType("OracleTimeStampLTZ");
            OracleTimeStampTZ = helper.GetOracleType("OracleTimeStampTZ");

            ColumnTypes = new HashSet<Type>();
            ColumnTypes.Add(OracleBFile);
            ColumnTypes.Add(OracleBinary);
            ColumnTypes.Add(OracleBlob);
            ColumnTypes.Add(OracleClob);
            ColumnTypes.Add(OracleDate);
            ColumnTypes.Add(OracleDecimal);
            ColumnTypes.Add(OracleIntervalDS);
            ColumnTypes.Add(OracleIntervalYM);
            ColumnTypes.Add(OracleRef);
            ColumnTypes.Add(OracleRefCursor);
            ColumnTypes.Add(OracleString);
            ColumnTypes.Add(OracleTimeStamp);
            ColumnTypes.Add(OracleTimeStampLTZ);
            ColumnTypes.Add(OracleTimeStampTZ);
        }

        public static HashSet<Type> ColumnTypes;

        public static Type OracleParameterStatus { get; set; }
        public static Type OracleBFile { get; set; }
        public static Type OracleBinary { get; set; }
        public static Type OracleBlob { get; set; }
        public static Type OracleClob { get; set; }
        public static Type OracleDate { get; set; }
        public static Type OracleDecimal { get; set; }
        public static Type OracleIntervalDS { get; set; }
        public static Type OracleIntervalYM { get; set; }
        public static Type OracleRef { get; set; }
        public static Type OracleRefCursor { get; set; }
        public static Type OracleString { get; set; }
        public static Type OracleTimeStamp { get; set; }
        public static Type OracleTimeStampLTZ { get; set; }
        public static Type OracleTimeStampTZ { get; set; }
    }
}
