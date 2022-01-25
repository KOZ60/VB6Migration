using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using Managed.OracleInProcServer.Core;
using Microsoft.VisualBasic.CompilerServices;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Managed.OracleInProcServer
{
    static class UTL
    {

        public static ParameterDirection ToParameterDirection(this paramMode value) {
            switch (value) {
                case paramMode.ORAPARM_BOTH:
                    return ParameterDirection.InputOutput;
                case paramMode.ORAPARM_INPUT:
                    return ParameterDirection.Input;
                case paramMode.ORAPARM_OUTPUT:
                    return ParameterDirection.Output;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static paramMode ToParamMode(this ParameterDirection value) {
            switch (value) {
                case ParameterDirection.InputOutput:
                    return paramMode.ORAPARM_BOTH;
                case ParameterDirection.Input:
                    return paramMode.ORAPARM_INPUT;
                case ParameterDirection.Output:
                    return paramMode.ORAPARM_OUTPUT;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // oo4o の serverType を OracleDbType に変換
        public class ServerTypeToOracleDbTypeDictionary : ConvertDictionary<serverType, OracleDbType>
        {
            public ServerTypeToOracleDbTypeDictionary() {
                Add(serverType.ORATYPE_VARCHAR2, OracleDbType.Varchar2);
                Add(serverType.ORATYPE_VARCHAR, OracleDbType.Varchar2);

                Add(serverType.ORATYPE_CHAR, OracleDbType.Char);
                Add(serverType.ORATYPE_CHARZ, OracleDbType.Varchar2);

                Add(serverType.ORATYPE_NVARCHAR2, OracleDbType.NVarchar2);
                Add(serverType.ORATYPE_NVARCHAR, OracleDbType.NVarchar2);

                Add(serverType.ORATYPE_NCHAR, OracleDbType.NChar);
                Add(serverType.ORATYPE_NCHARZ, OracleDbType.NVarchar2);

                Add(serverType.ORATYPE_NUMBER, OracleDbType.Decimal);
                Add(serverType.ORATYPE_DATE, OracleDbType.Date);

                //Add(serverType.ORATYPE_BOOL, OracleDbType.Boolean);
                Add(serverType.ORATYPE_SMALLINT, OracleDbType.Byte);
                Add(serverType.ORATYPE_SIGNED16, OracleDbType.Int16);
                Add(serverType.ORATYPE_SIGNED32, OracleDbType.Int32);
                Add(serverType.ORATYPE_SIGNED64, OracleDbType.Int64);
                Add(serverType.ORATYPE_FLOAT, OracleDbType.Single);
                Add(serverType.ORATYPE_DOUBLE, OracleDbType.Double);

                Add(serverType.ORATYPE_RAW, OracleDbType.Raw);
                Add(serverType.ORATYPE_LONG, OracleDbType.Long);
                Add(serverType.ORATYPE_LONGRAW, OracleDbType.LongRaw);

                Add(serverType.ORATYPE_BFILE, OracleDbType.BFile);
                Add(serverType.ORATYPE_BLOB, OracleDbType.Blob);
                Add(serverType.ORATYPE_CLOB, OracleDbType.Clob);
                Add(serverType.ORATYPE_NCLOB, OracleDbType.NClob);

                Add(serverType.ORATYPE_INTERVALDS, OracleDbType.IntervalDS);
                Add(serverType.ORATYPE_INTERVALYM, OracleDbType.IntervalYM);

                //Add(serverType.ORATYPE_REF, OracleDbType.Ref);
                Add(serverType.ORATYPE_CURSOR, OracleDbType.RefCursor);

                Add(serverType.ORATYPE_TIMESTAMP, OracleDbType.TimeStamp);
                Add(serverType.ORATYPE_TIMESTAMPLTZ, OracleDbType.TimeStampLTZ);
                Add(serverType.ORATYPE_TIMESTAMPTZ, OracleDbType.TimeStampTZ);

                Add(serverType.ORATYPE_BFLOAT, OracleDbType.BinaryFloat);
                Add(serverType.ORATYPE_BDOUBLE, OracleDbType.BinaryDouble);

                Add(serverType.ORATYPE_XML, OracleDbType.XmlType);
                //Add(serverType.ORATYPE_OBJECT, OracleDbType.Object);
            }

            protected override OracleDbType CashOut(serverType key) {
                throw new NotSupportedException(key.ToString() + " はサポート外です。");
            }
        }

        public static ServerTypeToOracleDbTypeDictionary ServerTypeToOracleDbType = new ServerTypeToOracleDbTypeDictionary();

        public static OracleDbType ToOracleDbType(this serverType serverType) {
            return ServerTypeToOracleDbType[serverType];
        }

        public static serverType ToServerType(this OracleDbType oracleDbType) {
            return ServerTypeToOracleDbType[oracleDbType];
        }

        public class OracleDbTypeToTypeDictionary : ConvertDictionary<OracleDbType, Type>
        {
            public OracleDbTypeToTypeDictionary() {
                Add(OracleDbType.Varchar2, typeof(string));
                Add(OracleDbType.Char, typeof(string));
                Add(OracleDbType.NChar, typeof(string));
                Add(OracleDbType.NVarchar2, typeof(string));
                Add(OracleDbType.Decimal, typeof(decimal));
                Add(OracleDbType.Date, typeof(DateTime));

                //Add(OracleDbType.Boolean, typeof(bool));
                Add(OracleDbType.Byte, typeof(byte));
                Add(OracleDbType.Int16, typeof(Int16));
                Add(OracleDbType.Int32, typeof(Int32));
                Add(OracleDbType.Int64, typeof(Int64));
                Add(OracleDbType.Single, typeof(float));
                Add(OracleDbType.Double, typeof(double));

                Add(OracleDbType.Raw, typeof(OracleBinary));
                Add(OracleDbType.Long, typeof(string));
                Add(OracleDbType.LongRaw, typeof(byte[]));

                Add(OracleDbType.BFile, typeof(OracleBFile));
                Add(OracleDbType.Blob, typeof(OracleBlob));
                Add(OracleDbType.Clob, typeof(OracleClob));
                Add(OracleDbType.NClob, typeof(OracleClob));

                Add(OracleDbType.IntervalDS, typeof(OracleIntervalDS));
                Add(OracleDbType.IntervalYM, typeof(OracleIntervalYM));

                //Add(OracleDbType.Ref, OracleTypes.OracleRef);
                Add(OracleDbType.RefCursor, typeof(OracleRefCursor));

                Add(OracleDbType.TimeStamp, typeof(OracleTimeStamp));
                Add(OracleDbType.TimeStampLTZ, typeof(OracleTimeStampLTZ));
                Add(OracleDbType.TimeStampTZ, typeof(OracleTimeStampTZ));

                Add(OracleDbType.BinaryFloat, typeof(float));
                Add(OracleDbType.BinaryDouble, typeof(double));

                Add(OracleDbType.XmlType, typeof(string));
                //Add(OracleDbType.Object, typeof(Object));
            }

            protected override Type CashOut(OracleDbType key) {
                return typeof(object);
            }
        }

        public static OracleDbTypeToTypeDictionary OracleDbTypeToType = new OracleDbTypeToTypeDictionary();

        /// <summary>
        /// Type を OracleDbType に変換します。
        /// </summary>
        public static OracleDbType ToOracleDbType(this Type value) {
            return OracleDbTypeToType[value];
        }

        public static bool IsNull(this object value) {
            if (value == null) {
                return true;
            }
            if (value == DBNull.Value) {
                return true;
            }
            var nullable = value as INullable;
            if (nullable != null) {
                return nullable.IsNull;
            }
            Type type = value.GetType();
            PropertyInfo pi = type.GetProperty("IsNull");
            if (pi != null) {
                return (bool)pi.GetValue(value);
            }
            return false;
        }

        public static object ToBlankStrip(this object value, bool option) {
            if (IsNull(value)) {
                return DBNull.Value;
            }
            var str = value as string;
            if (str != null) {
                return ToBlankStrip(str, option);
            }
            return value;
        }

        public static string ToBlankStrip(this string value, bool blankStrip) {
            // 単純に空白を除去するわけではない。
            // 除去した結果、空文字になった時は元の値を返す
            if (blankStrip) {
                var trimValue = value.TrimEnd(' ');
                if (string.IsNullOrEmpty(trimValue)) {
                    return value;
                }
                return trimValue;
            } else {
                return value;
            }
        }

        public static Object DefaultToType(IConvertible value, Type targetType, IFormatProvider provider) {
            if (value is null) {
                return null;
            }
            Type valueType = value.GetType();
            if (valueType == targetType) {
                return value;
            }
            if (targetType == typeof(bool)) {
                return value.ToBoolean(provider);
            }
            if (targetType == typeof(char)) {
                return value.ToChar(provider);
            }
            if (targetType == typeof(sbyte)) {
                return value.ToSByte(provider);
            }
            if (targetType == typeof(byte)) {
                return value.ToByte(provider);
            }
            if (targetType == typeof(Int16)) {
                return value.ToInt16(provider);
            }
            if (targetType == typeof(UInt16)) {
                return value.ToUInt16(provider);
            }
            if (targetType == typeof(Int32)) {
                return value.ToInt32(provider);
            }
            if (targetType == typeof(UInt32)) {
                return value.ToUInt32(provider);
            }
            if (targetType == typeof(Int64)) {
                return value.ToInt64(provider);
            }
            if (targetType == typeof(UInt64)) {
                return value.ToUInt64(provider);
            }
            if (targetType == typeof(Single)) {
                return value.ToSingle(provider);
            }
            if (targetType == typeof(Double)) {
                return value.ToDouble(provider);
            }
            if (targetType == typeof(Decimal)) {
                return value.ToDecimal(provider);
            }
            if (targetType == typeof(DateTime)) {
                return value.ToDateTime(provider);
            }
            if (targetType == typeof(String)) {
                return value.ToString(provider);
            }
            if (targetType == typeof(Object)) {
                return (Object)value;
            }
            if (targetType.IsEnum) {
                return (Enum)value;
            }
            var cnv = TypeDescriptor.GetConverter(valueType);
            if (cnv != null && cnv.CanConvertTo(targetType)) {
                return cnv.ConvertTo(value, targetType);
            }
            throw new InvalidCastException(string.Format("{0} から {1} にキャストできませんでした。", value.GetType().FullName, targetType.FullName));
        }

        public static Type ToType(this OracleDbType oracleDbType) {
            return OracleDbTypeToType[oracleDbType];
        }

        public static Type ToType(this serverType serverType) {
            return ToType(serverType.ToOracleDbType());
        }

        public static bool IsString(this serverType value) {
            return IsString(ToType(value));
        }

        public static bool IsString(this Type value) {
            return value == typeof(string);
        }

        public static bool IsBoolean(this serverType value) {
            return IsBoolean(ToType(value));
        }

        public static bool IsBoolean(this Type value) {
            return value == typeof(Boolean);
        }

        public static bool IsBinary(this serverType value) {
            return IsBinary(ToType(value));
        }

        public static bool IsBinary(this Type value) {
            return value == typeof(byte[]);
        }

        public static bool IsDateTime(this serverType value) {
            return IsDateTime(ToType(value));
        }

        public static bool IsDateTime(this Type value) {
            return value == typeof(DateTime);
        }

        public static bool IsDecimal(this serverType value) {
            return IsDecimal(ToType(value));
        }

        public static bool IsDecimal(this Type value) {
            return value == typeof(decimal);
        }

        public static bool IsDouble(this serverType value) {
            return IsDouble(ToType(value));
        }

        public static bool IsDouble(this Type value) {
            return value == typeof(double);
        }

        public static bool IsSingle(this serverType value) {
            return IsSingle(ToType(value));
        }

        public static bool IsSingle(this Type value) {
            return value == typeof(float);
        }

        public static object ConvertTo(object value, serverType serverType) {
            return ConvertTo(value, ToType(serverType));
        }

        public static object ConvertTo(object value, Type type) {
            if (value.IsNull()) {
                return DBNull.Value;
            }
            var valueType = value.GetType();
            if (valueType == type) {
                return value;
            }
            if (type == typeof(string)) {
                return Conversions.ToString(value);
            };
            if (type == typeof(decimal)) {
                return Conversions.ToDecimal(value);
            };
            if (type == typeof(bool)) {
                return Conversions.ToBoolean(value);
            };
            if (type == typeof(DateTime)) {
                return Conversions.ToDate(value);
            };
            if (type == typeof(double)) {
                return Conversions.ToDouble(value);
            };

            var converter = TypeDescriptor.GetConverter(type);
            if (converter != null && converter.CanConvertFrom(valueType)) {
                return converter.ConvertFrom(value);
            }

            IConvertible convertible = value as IConvertible;
            if (convertible != null) {
                return convertible.ToType(type, null);
            }

            // 変換できなければそのまま
            return value;
        }

        public static SchemaInformation[] CreateSchema(OracleDataReader reader) {
            // フィールド数分の SchemaInformation を確保
            var fieldCount = reader.FieldCount;
            var sc = new SchemaInformation[fieldCount];

            using (DataTable dt = reader.GetSchemaTable()) {

                // 重複するフィールドがある場合、後ろ側が優先されるので後ろから名前をつける

                HashSet<string> hash = new HashSet<string>(StringComparer.CurrentCultureIgnoreCase);
                for (int i = fieldCount - 1; i >= 0; i--) {
                    sc[i] = new SchemaInformation(dt.Rows[i]);

                    // 同一項目名には連番を振る
                    int index = 0;
                    string name = sc[i].ColumnName;
                    while (hash.Contains(name)) {
                        index++;
                        name = "#" + sc[i].ColumnName + "_" + index.ToString();
                    }
                    hash.Add(name);
                    sc[i].RealColumnName = name;
                }
            }
            return sc;
        }

        public static DataTable CreateDataTable(SchemaInformation[] sc) {
            var dt = new DataTable();
            dt.BeginInit();
            for (int i = 0; i < sc.Length; i++) {
                DataColumn dc = new DataColumn(sc[i].RealColumnName, sc[i].DataType);
                dt.Columns.Add(dc);
            }
            dt.EndInit();
            return dt;
        }

        public static void FillDataTable(OracleDataReader reader, DataTable dt, SchemaInformation[] sc, bool blankStrip) {
            dt.BeginLoadData();
            while (reader.Read()) {
                var row = dt.NewRow();
                row.BeginEdit();
                for (int i = 0; i < sc.Length; i++) {
                    switch (sc[i].ProviderType) {
                        case OracleDbType.Decimal:
                            if (reader.IsDBNull(i)) {
                                row[i] = DBNull.Value;
                            } else {
                                // G29 で編集しないと ToString() したときに "1.00" となることがある
                                var providerValue = reader.GetProviderSpecificValue(i);
                                var dec = decimal.Parse(providerValue.ToString());
                                row[i] = decimal.Parse(dec.ToString("G29"));
                            }
                            break;
                        default:
                            if (sc[i].DataType == typeof(string)) {
                                // String の場合は BlankStrp 
                                if (reader.IsDBNull(i)) {
                                    row[i] = DBNull.Value;
                                } else {
                                    row[i] = reader.GetString(i).ToBlankStrip(blankStrip);
                                }
                            } else if (OracleTypes.ColumnTypes.Contains(sc[i].DataType)) {
                                // ODP.NET のタイプの場合はプロバイダ型を取得
                                row[i] = reader.GetProviderSpecificValue(i);
                            } else {
                                // .NET 型
                                if (reader.IsDBNull(i)) {
                                    row[i] = DBNull.Value;
                                } else {
                                    row[i] = reader.GetValue(i);
                                }
                            }
                            break;
                    }
                }
                row.EndEdit();
                dt.Rows.Add(row);
            }
            dt.EndLoadData();
        }

        public static bool IsOdp(this Type type) {
            return OracleTypes.ColumnTypes.Contains(type);
        }
    }
}

