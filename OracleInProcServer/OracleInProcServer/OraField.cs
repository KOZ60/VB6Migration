using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Runtime.CompilerServices;
using OracleInProcServer.Core;

namespace OracleInProcServer
{
    public class OraField : IConvertible
    {
        OraDynaset _Dynaset;
        DataColumn _Column;
        SchemaInformation _SchemaInformation;

        internal OraField(OraDynaset dyn, DataColumn dc, SchemaInformation sc) {
            _Dynaset = dyn;
            _Column = dc;
            _SchemaInformation = sc;
        }

        public OraVariant Value {
            get {
                DataRow row = _Dynaset.CurrentRow;
                if (row != null) {
                    if (_Column.DataType.IsOdp()) {
                        return new OraVariant(row[_Column], _SchemaInformation.ProviderType);
                    } else {
                        return new OraVariant(row[_Column].ToBlankStrip(_Dynaset.BlankStripOption));
                    }
                } else {
                    return OraVariant.Null;
                }
            }
            set {
                DataRow row = _Dynaset.CurrentRow;
                if (row != null) {
                    row[_Column] = value.ToBlankStrip(_Dynaset.BlankStripOption);
                }
            }
        }

        public string Name {
            get {
                return _SchemaInformation.ColumnName;
            }
        }

        public Type Type {
            get {
                return _Column.DataType;
            }
        }

        public serverType serverType {
            get {
                return _SchemaInformation.ProviderType.ToServerType();
            }
        }

        public override string ToString() {
            DataRow row = _Dynaset.CurrentRow;
            if (row != null) {
                return row[_Column].ToString();
            }
            return null;
        }

        /// <summary>
        /// OraField オブジェクトを String 型に変換します。
        /// </summary>
        public static implicit operator string(OraField field) {
            // 比較で呼び出されたとき、インスタンス化されていなければ null が入っている
            if ((object)field == null) {
                return string.Empty;
            }
            return field.Value;
        }

        /// <summary>
        /// 文字列の連結演算を行います。
        /// </summary>
        [SpecialName]
        public static string op_Concatenate(OraField a, OraField b) {
            return string.Concat(a, b);
        }

        /// <summary>
        /// 文字列の連結演算を行います。
        /// </summary>
        [SpecialName]
        public static string op_Concatenate(string a, OraField b) {
            return string.Concat(a, b);
        }

        /// <summary>
        /// 文字列の連結演算を行います。
        /// </summary>
        [SpecialName]
        public static string op_Concatenate(OraField a, string b) {
            return string.Concat(a, b);
        }

        #region IConvertible

        TypeCode IConvertible.GetTypeCode() {
            return ((IConvertible)this.Value).GetTypeCode();
        }

        bool IConvertible.ToBoolean(IFormatProvider provider) {
            return ((IConvertible)this.Value).ToBoolean(provider);
        }

        char IConvertible.ToChar(IFormatProvider provider) {
            return ((IConvertible)this.Value).ToChar(provider);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider) {
            return ((IConvertible)this.Value).ToSByte(provider);
        }

        byte IConvertible.ToByte(IFormatProvider provider) {
            return ((IConvertible)this.Value).ToByte(provider);
        }

        short IConvertible.ToInt16(IFormatProvider provider) {
            return ((IConvertible)this.Value).ToInt16(provider);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider) {
            return ((IConvertible)this.Value).ToUInt16(provider);
        }

        int IConvertible.ToInt32(IFormatProvider provider) {
            return ((IConvertible)this.Value).ToInt32(provider);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider) {
            return ((IConvertible)this.Value).ToUInt32(provider);
        }

        long IConvertible.ToInt64(IFormatProvider provider) {
            return ((IConvertible)this.Value).ToInt64(provider);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider) {
            return ((IConvertible)this.Value).ToUInt64(provider);
        }

        float IConvertible.ToSingle(IFormatProvider provider) {
            return ((IConvertible)this.Value).ToSingle(provider);
        }

        double IConvertible.ToDouble(IFormatProvider provider) {
            return ((IConvertible)this.Value).ToDouble(provider);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider) {
            return ((IConvertible)this.Value).ToDecimal(provider);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) {
            return ((IConvertible)this.Value).ToDateTime(provider);
        }

        string IConvertible.ToString(IFormatProvider provider) {
            return ((IConvertible)this.Value).ToString(provider);
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) {
            return ((IConvertible)this.Value).ToType(conversionType, provider);
        }

        #endregion

    }
}
