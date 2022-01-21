using System;
using System.Linq;
using System.Data;
using System.Runtime.InteropServices;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Managed.OracleInProcServer.Core
{
    internal struct SchemaInformation
    {
        public String ColumnName;
        public String RealColumnName;
        public Int32 ColumnOrdinal;
        public Int64 ColumnSize;
        public Int16 NumericPrecision;
        public Int16 NumericScale;
        public Boolean IsUnique;
        public Boolean IsKey;
        public Boolean IsRowID;
        public String BaseColumnName;
        public String BaseSchemaName;
        public String BaseTableName;
        public Type DataType;
        public OracleDbType ProviderType;
        public Boolean AllowDBNull;
        public Boolean IsAliased;
        public Boolean IsByteSemantic;
        public Boolean IsExpression;
        public Boolean IsHidden;
        public Boolean IsReadOnly;
        public Boolean IsLong;

        public SchemaInformation(DataRow sc) {
            ColumnName = ParseString(sc["ColumnName"]);
            RealColumnName = ColumnName;
            ColumnOrdinal = ParseInteger(sc["ColumnOrdinal"]);
            ColumnSize = ParseLong(sc["ColumnSize"]);
            NumericPrecision = ParseShort(sc["NumericPrecision"]);
            NumericScale = ParseShort(sc["NumericScale"]);
            IsUnique = ParseBoolean(sc["IsUnique"]);
            IsKey = ParseBoolean(sc["IsKey"]);
            IsRowID = ParseBoolean(sc["IsRowID"]);
            BaseColumnName = ParseString(sc["BaseColumnName"]);
            BaseSchemaName = ParseString(sc["BaseSchemaName"]);
            BaseTableName = ParseString(sc["BaseTableName"]);
            DataType = ParseType(sc["DataType"]);
            ProviderType = ParseOracleDbType(sc["ProviderType"]);
            DataType = ProviderType.ToType();
            AllowDBNull = ParseBoolean(sc["AllowDBNull"]);
            IsAliased = ParseBoolean(sc["IsAliased"]);
            IsByteSemantic = ParseBoolean(sc["IsByteSemantic"]);
            IsExpression = ParseBoolean(sc["IsExpression"]);
            IsHidden = ParseBoolean(sc["IsHidden"]);
            IsReadOnly = ParseBoolean(sc["IsReadOnly"]);
            IsLong = ParseBoolean(sc["IsLong"]);
        }

        public SchemaInformation(DataColumn dc) {
            ColumnName = dc.ColumnName;
            RealColumnName = dc.ColumnName;
            ColumnOrdinal = dc.Ordinal;
            ColumnSize = 0;
            NumericPrecision = 0;
            NumericScale = 0;
            IsUnique = false;
            IsKey = dc.Table.PrimaryKey.Contains(dc);
            IsRowID = false;
            BaseColumnName = null;
            BaseSchemaName = null;
            BaseTableName = null;
            DataType = dc.DataType;
            ProviderType = dc.DataType.ToOracleDbType();
            AllowDBNull = dc.AllowDBNull;
            IsAliased = false;
            IsByteSemantic = false;
            IsExpression = false;
            IsHidden = false;
            IsReadOnly = dc.ReadOnly;
            IsLong = false;
        }


        static string ParseString(object value) {
            if (value == null) return string.Empty;
            if (value == DBNull.Value) return string.Empty;
            return value.ToString();
        }
        static int ParseInteger(object value) {
            if (value == null) return 0;
            if (value == DBNull.Value) return 0;
            int result = 0;
            if (int.TryParse(value.ToString(), out result))
                return result;
            return 0;
        }
        static long ParseLong(object value) {
            if (value == null) return 0;
            if (value == DBNull.Value) return 0;
            long result = 0;
            if (long.TryParse(value.ToString(), out result))
                return result;
            return 0;
        }
        static short ParseShort(object value) {
            if (value == null) return 0;
            if (value == DBNull.Value) return 0;
            short result = 0;
            if (short.TryParse(value.ToString(), out result))
                return result;
            return 0;
        }
        static bool ParseBoolean(object value) {
            if (value == null) return false;
            if (value == DBNull.Value) return false;
            bool result = false;
            if (bool.TryParse(value.ToString(), out result))
                return result;
            return false;
        }
        static Type ParseType(object value) {
            return Type.GetType(value.ToString());
        }
        static OracleDbType ParseOracleDbType(object value) {
            return (OracleDbType)ParseInteger(value);
        }
    }
}
