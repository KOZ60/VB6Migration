using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;
using Managed.OracleInProcServer.Core;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Managed.OracleInProcServer
{
    [Serializable]
    public struct OraVariant : IComparable, IComparable<OraVariant>, IConvertible
    {
        object _Value;
        bool _IsNull;
        OracleDbType _OracleDbType;

        public static OraVariant Null = new OraVariant(DBNull.Value);

        public OraVariant(object value) {
            if (value == null || value == DBNull.Value) {
                _Value = DBNull.Value;
                _IsNull = true;
                _OracleDbType = (OracleDbType)0;
            }
            else if (value is OraVariant) {
                var ov = (OraVariant)value;
                while (ov.Value is OraVariant) {
                    ov = (OraVariant)ov.Value;
                }
                _Value = ov.Value;
                _IsNull = ov._IsNull;
                _OracleDbType = ov._OracleDbType;
            }
            else {
                _Value = value;
                _IsNull = false;
                _OracleDbType = value.GetType().ToOracleDbType();
            }
        }

        internal OraVariant(object value, OracleDbType oracleDbType) {
            _Value = value;
            _IsNull = value.IsNull();
            _OracleDbType = oracleDbType;
        }

        public object Value {
            get {
                return _Value;
            }
        }

        public bool IsNull {
            get {
                return _IsNull;
            }
        }

        public override string ToString() {
            if (IsNull) {
                return string.Empty;
            }
            return Conversions.ToString(_Value);
        }

        /// <summary>
        /// OraVariant を String に変換します。
        /// </summary>
        public static implicit operator string(OraVariant value) {
            if ((object)value == null || value.IsNull) {
                return string.Empty;
            }
            return ((IConvertible)value).ToString(null);
        }

        public static implicit operator OraBFile(OraVariant value) {
            return new OraBFile((OracleBFile)value.Value);
        }

        public static implicit operator OraVariant(OraBFile value) {
            return new OraVariant(value.oraclebFile, OracleDbType.BFile);
        }

        /// <summary>
        /// String を OraVariant に変換します。
        /// </summary>
        public static implicit operator OraVariant(string value) {
            return new OraVariant(value, OracleDbType.Varchar2);
        }

        /// <summary>
        /// Decimal を OraVariant に変換します。
        /// </summary>
        public static implicit operator OraVariant(decimal value) {
            return new OraVariant(value, OracleDbType.Decimal);
        }

        /// <summary>
        /// DateTime を OraVariant に変換します。
        /// </summary>
        public static implicit operator OraVariant(DateTime value) {
            return new OraVariant(value, OracleDbType.Date);
        }

        /// <summary>
        /// 文字列の連結演算を行います。
        /// </summary>
        [SpecialName]
        public static string op_Concatenate(OraVariant a, OraVariant b) {
            return string.Concat(a, b);
        }

        /// <summary>
        /// 文字列の連結演算を行います。
        /// </summary>
        [SpecialName]
        public static string op_Concatenate(string a, OraVariant b) {
            return string.Concat(a, b);
        }

        /// <summary>
        /// 文字列の連結演算を行います。
        /// </summary>
        [SpecialName]
        public static string op_Concatenate(OraVariant a, string b) {
            return string.Concat(a, b);
        }

        public int CompareTo(object obj) {

            // インスタンスが等しいならゼロ
            if (object.ReferenceEquals(this, obj)) {
                return 0;
            }

            // 相手が null なら、1 を返す
            if (obj is null) {
                return 1;
            }

            // OraVariant なら CompareTo(OraVariant other)を呼び出す
            if (obj is OraVariant) {
                return CompareTo((OraVariant)obj);
            }

            // OraVariant を作成して CompareTo(OraVariant other) を呼び出す
            return CompareTo(new OraVariant(obj));
        }

        public int CompareTo(OraVariant other) {
            return Compare(this, other);
        }

        public static int Compare(OraVariant a, OraVariant b) {
            // 両者 null か、インスタンスが同一
            if (object.ReferenceEquals((object)a, (object)b)) {
                return 0;
            }

            return string.Compare(a, b);
        }

        #region IConvertible

        TypeCode IConvertible.GetTypeCode() {
            return Convert.GetTypeCode(this.Value);
        }

        bool IConvertible.ToBoolean(IFormatProvider provider) {
            return Convert.ToBoolean(this.Value, provider);
        }

        char IConvertible.ToChar(IFormatProvider provider) {
            return Convert.ToChar(this.Value, provider);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider) {
            return Convert.ToSByte(this.Value, provider);
        }

        byte IConvertible.ToByte(IFormatProvider provider) {
            return Convert.ToByte(this.Value, provider);
        }

        short IConvertible.ToInt16(IFormatProvider provider) {
            return Convert.ToInt16(this.Value, provider);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider) {
            return Convert.ToUInt16(this.Value, provider);
        }

        int IConvertible.ToInt32(IFormatProvider provider) {
            return Convert.ToInt32(this.Value, provider);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider) {
            return Convert.ToUInt32(this.Value, provider);
        }

        long IConvertible.ToInt64(IFormatProvider provider) {
            return Convert.ToInt64(this.Value, provider);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider) {
            return Convert.ToUInt64(this.Value, provider);
        }

        float IConvertible.ToSingle(IFormatProvider provider) {
            return Convert.ToSingle(this.Value, provider);
        }

        double IConvertible.ToDouble(IFormatProvider provider) {
            return Convert.ToDouble(this.Value, provider);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider) {
            var str = Conversions.ToString(this.Value);
            if (string.IsNullOrWhiteSpace(str)) {
                return decimal.Zero;
            }
            return decimal.Parse(str);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) {
            return Convert.ToDateTime(this.Value, provider);
        }

        string IConvertible.ToString(IFormatProvider provider) {
            if (IsNull) {
                return string.Empty;
            }
            return Conversions.ToString(_Value);
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) {
            return UTL.DefaultToType((IConvertible)this, conversionType, provider);
        }


        #endregion


    }
}
