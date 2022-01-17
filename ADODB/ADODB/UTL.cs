using System;
using System.Data;
using System.Linq;
using System.Text;

namespace ADODB
{
    internal static class UTL
    {
        internal static DataColumn CreateDataColumn(string name, DataTypeEnum dataType, int definedSize, FieldAttributeEnum attrib, object fieldValue) {
            DataColumn dc = new DataColumn();
            dc.ColumnName = name;
            dc.DataType = UTL.ToDataType(dataType);
            if (dc.DataType == typeof(string)) {
                dc.MaxLength = definedSize;
            }
            dc.AllowDBNull = UTL.GetMask(attrib, FieldAttributeEnum.adFldIsNullable);
            dc.DefaultValue = fieldValue;
            return dc;
        }

        internal static bool GetMask(FieldAttributeEnum value, FieldAttributeEnum mask) {
            return (value & mask) == mask;
        }

        internal static bool GetMask(ParameterAttributesEnum value, ParameterAttributesEnum mask) {
            return (value & mask) == mask;
        }

        internal static bool GetMask(int value, int mask) {
            return (value & mask) == mask;
        }

        internal static Type ToDataType(DataTypeEnum type) {
            switch (type) {
                case DataTypeEnum.adDate:
                case DataTypeEnum.adDBDate:
                case DataTypeEnum.adDBTime:
                case DataTypeEnum.adDBTimeStamp:
                case DataTypeEnum.adFileTime:
                    return typeof(DateTime);

                case DataTypeEnum.adBinary:
                case DataTypeEnum.adLongVarBinary:
                case DataTypeEnum.adVarBinary:
                    return typeof(byte[]);

                case DataTypeEnum.adBSTR:
                case DataTypeEnum.adChar:
                case DataTypeEnum.adLongVarChar:
                case DataTypeEnum.adLongVarWChar:
                case DataTypeEnum.adVarChar:
                case DataTypeEnum.adVarWChar:
                case DataTypeEnum.adWChar:
                    return typeof(string);

                case DataTypeEnum.adBoolean:
                    return typeof(bool);

                case DataTypeEnum.adBigInt:
                case DataTypeEnum.adCurrency:
                case DataTypeEnum.adDecimal:
                case DataTypeEnum.adNumeric:
                    return typeof(decimal);

                case DataTypeEnum.adInteger:
                    return typeof(int);

                case DataTypeEnum.adTinyInt:
                    return typeof(sbyte);

                case DataTypeEnum.adSmallInt:
                    return typeof(short);

                case DataTypeEnum.adSingle:
                    return typeof(float);

                case DataTypeEnum.adDouble:
                    return typeof(double);

                case DataTypeEnum.adUnsignedBigInt:
                    return typeof(long);

                case DataTypeEnum.adUnsignedTinyInt:
                    return typeof(byte);

                case DataTypeEnum.adUnsignedSmallInt:
                    return typeof(ushort);

                case DataTypeEnum.adUnsignedInt:
                    return typeof(uint);
            }

            return typeof(object);
        }

        public static ParameterDirection ToDirection(ParameterDirectionEnum direction) {
            switch (direction) {
                case ParameterDirectionEnum.adParamInput:
                    return ParameterDirection.Input;

                case ParameterDirectionEnum.adParamInputOutput:
                    return ParameterDirection.InputOutput;

                case ParameterDirectionEnum.adParamOutput:
                    return ParameterDirection.Output;

                case ParameterDirectionEnum.adParamReturnValue:
                    return ParameterDirection.ReturnValue;

                default:
                    throw new ArgumentException();
            }
        }

        public static DbType ToDbType(DataTypeEnum type) {
            switch (type) {
                case DataTypeEnum.adDate:
                case DataTypeEnum.adDBDate:
                case DataTypeEnum.adDBTime:
                case DataTypeEnum.adDBTimeStamp:
                case DataTypeEnum.adFileTime:
                    return DbType.DateTime;

                case DataTypeEnum.adBinary:
                case DataTypeEnum.adLongVarBinary:
                case DataTypeEnum.adVarBinary:
                    return DbType.Binary;

                case DataTypeEnum.adBSTR:
                case DataTypeEnum.adChar:
                case DataTypeEnum.adLongVarChar:
                case DataTypeEnum.adLongVarWChar:
                case DataTypeEnum.adVarChar:
                case DataTypeEnum.adVarWChar:
                case DataTypeEnum.adWChar:
                    return DbType.String;

                case DataTypeEnum.adBoolean:
                    return DbType.Boolean;

                case DataTypeEnum.adBigInt:
                case DataTypeEnum.adCurrency:
                case DataTypeEnum.adDecimal:
                case DataTypeEnum.adNumeric:
                    return DbType.Decimal;

                case DataTypeEnum.adInteger:
                    return DbType.Int32;

                case DataTypeEnum.adTinyInt:
                    return DbType.SByte;

                case DataTypeEnum.adSmallInt:
                    return DbType.Int16;

                case DataTypeEnum.adSingle:
                    return DbType.Single;

                case DataTypeEnum.adDouble:
                    return DbType.Double;

                case DataTypeEnum.adUnsignedBigInt:
                    return DbType.Int64;

                case DataTypeEnum.adUnsignedTinyInt:
                    return DbType.Byte;

                case DataTypeEnum.adUnsignedSmallInt:
                    return DbType.UInt16;

                case DataTypeEnum.adUnsignedInt:
                    return DbType.UInt32;
            }
            throw new ArgumentException();
        }

        public static CommandType ToCommandType(CommandTypeEnum value) {
            switch (value) {
                case CommandTypeEnum.adCmdText:
                    return CommandType.Text;
                case CommandTypeEnum.adCmdTable:
                    return CommandType.TableDirect;
                case CommandTypeEnum.adCmdStoredProc:
                    return CommandType.StoredProcedure;
            }
            throw new ArgumentException();
        }

    }
}
