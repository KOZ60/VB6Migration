using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace VBCompatible
{
    /// <summary>
    /// VB の String 互換クラス
    /// </summary>
    /// <remarks>
    /// VBString クラスは Byte 型配列で値を保持しています。
    /// （String クラスは Char 型の配列で文字列を管理しています。）
    /// <para>【特徴】</para>
    /// <para>・バイト型配列で値を保持する String クラスです。</para>
    /// <para>・ByVal String 経由で API に値を渡すことはできますが、取得はできません。</para>
    /// <para>【代入可能な型】</para>
    /// <para>  String</para>
    /// <para>  Byte(配列)</para>
    /// <para>  Boolean</para>
    /// <para>  Byte</para>
    /// <para>  Char</para>
    /// <para>  Short</para>
    /// <para>  Integer</para>
    /// <para>  Long</para>
    /// <para>  SByte</para>
    /// <para>  UShort</para>
    /// <para>  UInteger</para>
    /// <para>  ULong</para>
    /// <para>  Decimal</para>
    /// <para>  Single</para>
    /// <para>  Double</para>
    /// <para>  Date</para>
    /// <para>【取得可能な型】</para>
    /// <para>  String</para>
    /// <para>  Byte(配列)</para>
    /// </remarks>
    [Serializable]
    public class VBString : IComparable, ICloneable, IConvertible, IComparable<VBString>, IEnumerable<byte>, IEnumerable, IEquatable<VBString>
    {
        /// <summary>
        /// 値を保持
        /// </summary>
        private byte[] m_Value;
        private VariantType m_VarType;

        #region コンストラクタ/デストラクタ

        internal VBString() {
            m_Value = new byte[0];
            m_VarType = VariantType.String;
        }

        internal VBString(string value) {
            if (string.IsNullOrEmpty(value)) {
                m_Value = new byte[0];
            } else {
                m_Value = Encoding.Unicode.GetBytes(value);
            }
            m_VarType = VariantType.String;
        }

        internal VBString(byte[] value)
            : this(value, VariantType.Byte) {
        }

        internal VBString(byte[] value, VariantType vartype) {
            m_Value = (byte[])value.Clone();
            m_VarType = vartype;
        }

        #endregion

        #region VBString メンバ

        internal byte GetByte(int index) {
            return m_Value[index];
        }

        internal VariantType VarType {
            get { return m_VarType; }
        }

        /// <summary>
        /// 格納されたデータのバイト数を取得します。
        /// </summary>
        public int LengthB {
            get { return m_Value.Length; }
        }

        /// <summary>
        /// 格納されたデータのキャラクタ数を取得します。
        /// </summary>
        public int Length {
            get { return ToString().Length; }
        }

        /// <summary>
        /// 格納されたデータを String 型で返します。
        /// </summary>
        /// <returns>格納されたデータを示す文字列式</returns>
        public override string ToString() {
            if (this.LengthB == 0) {
                return string.Empty;
            }
            return Encoding.Unicode.GetString(m_Value);
        }

        /// <summary>
        /// 格納されたデータを Byte 型配列で返します。
        /// </summary>
        /// <returns>格納されたデータのByte 型配列のコピー</returns>
        public byte[] ToByteArray() {
            return (byte[])m_Value.Clone();
        }

        /// <summary>
        /// 新しいインスタンスの VBString を作成し、内容をコピーします。
        /// </summary>
        /// <returns>新しいインスタンスの VBString</returns>
        public VBString ToVBString() {
            return new VBString(m_Value, m_VarType);
        }

        /// <summary>
        /// 新しいインスタンスの VBString を作成し、内容をコピーします。
        /// </summary>
        /// <returns>オブジェクトのコピー</returns>
        public object Clone() {
            return new VBString(m_Value, m_VarType);
        }

        /// <summary>
        /// 特定の型のハッシュ関数として機能します。
        /// </summary>
        /// <returns>ハッシュコード</returns>
        public override int GetHashCode() {
            return m_Value.GetHashCode();
        }

        /// <summary>
        /// 指定されたインスタンスの値が等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">比較するオブジェクト</param>
        /// <returns>等しい場合は true そうでないときは false を返します。</returns>
        public override bool Equals(object obj) {
            return Equals(this, obj);
        }

        /// <summary>
        /// 指定されたインスタンスの値が等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">比較する VBString オブジェクト</param>
        /// <returns>等しい場合は true そうでないときは false を返します。</returns>
        public bool Equals(VBString other) {
            return Equals(this, other);
        }

        #endregion

        #region 暗黙の型変換

        //******************************************************************************
        // String
        //******************************************************************************
        /// <summary>
        /// string 型を VBString 型に変換します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns>VBStringオブジェクト</returns>
        public static implicit operator VBString(string value) {
            return new VBString(value);
        }

        /// <summary>
        /// VBString 型を string 型に変換します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator string(VBString value) {
            return value.ToString();
        }

        //******************************************************************************
        // Byte()
        //******************************************************************************
        /// <summary>
        /// byte[] 型を VBString 型に変換します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator VBString(byte[] value) {
            return new VBString(value);
        }

        /// <summary>
        /// VBString 型を byte[] 型に変換します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator byte[](VBString value) {
            return (byte[])value.ToByteArray();
        }

        //******************************************************************************
        // Boolean
        //******************************************************************************
        /// <summary>
        /// bool 型を VBString 型に変換します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator VBString(bool value) {
            return new VBString(value.ToString());
        }

        //******************************************************************************
        // Char
        //******************************************************************************
        /// <summary>
        /// Char 型を VBString オブジェクトに変換します。
        /// </summary>
        public static implicit operator VBString(char value) {
            return new VBString(value.ToString());
        }

        //******************************************************************************
        // Byte
        //******************************************************************************
        /// <summary>
        /// Byte 型を VBString オブジェクトに変換します。
        /// </summary>
        public static implicit operator VBString(byte value) {
            return new VBString(value.ToString());
        }

        //******************************************************************************
        // Short
        //******************************************************************************
        /// <summary>
        /// Short 型を VBString オブジェクトに変換します。
        /// </summary>
        public static implicit operator VBString(short value) {
            return new VBString(value.ToString());
        }

        //******************************************************************************
        // Integer
        //******************************************************************************
        /// <summary>
        /// Integer 型を VBString オブジェクトに変換します。
        /// </summary>
        public static implicit operator VBString(int value) {
            return new VBString(value.ToString());
        }

        //******************************************************************************
        // Long
        //******************************************************************************
        /// <summary>
        /// Long 型を VBString オブジェクトに変換します。
        /// </summary>
        public static implicit operator VBString(long value) {
            return new VBString(value.ToString());
        }

        //******************************************************************************
        // SByte
        //******************************************************************************
        /// <summary>
        /// SByte 型を VBString オブジェクトに変換します。
        /// </summary>
        public static implicit operator VBString(sbyte value) {
            return new VBString(value.ToString());
        }

        //******************************************************************************
        // UShort
        //******************************************************************************
        /// <summary>
        /// UShort 型を VBString オブジェクトに変換します。
        /// </summary>
        public static implicit operator VBString(ushort value) {
            return new VBString(value.ToString());
        }

        //******************************************************************************
        // UInteger
        //******************************************************************************
        /// <summary>
        /// UInteger 型を VBString オブジェクトに変換します。
        /// </summary>
        public static implicit operator VBString(uint value) {
            return new VBString(value.ToString());
        }

        //******************************************************************************
        // ULong
        //******************************************************************************
        /// <summary>
        /// ULong 型を VBString オブジェクトに変換します。
        /// </summary>
        public static implicit operator VBString(ulong value) {
            return new VBString(value.ToString());
        }

        //******************************************************************************
        // Single
        //******************************************************************************
        /// <summary>
        /// Single 型を VBString オブジェクトに変換します。
        /// </summary>
        public static implicit operator VBString(float value) {
            return new VBString(value.ToString());
        }

        //******************************************************************************
        // Double
        //******************************************************************************
        /// <summary>
        /// Double 型を VBString オブジェクトに変換します。
        /// </summary>
        public static implicit operator VBString(double value) {
            return new VBString(value.ToString());
        }

        //******************************************************************************
        // Decimal
        //******************************************************************************
        /// <summary>
        /// Decimal 型を VBString オブジェクトに変換します。
        /// </summary>
        public static implicit operator VBString(decimal value) {
            return new VBString(value.ToString());
        }

        //******************************************************************************
        // Date
        //******************************************************************************
        /// <summary>
        /// Date 型を VBString オブジェクトに変換します。
        /// </summary>
        public static implicit operator VBString(System.DateTime value) {
            return new VBString(value.ToString("YYYY/MM/DD hh:nn:ss"));
        }

        #endregion

        #region ２項演算子

        /// <summary>
        /// VBString オブジェクト同士の + 演算を行います。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [Description("VBString オブジェクト同士の + 演算を行います。")]
        [SpecialName]
        public static VBString operator +(VBString a, VBString b) {
            return Concat(a, b);
        }

        /// <summary>
        /// VBString オブジェクトとの &amp; 演算を行います。
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Description("VBString オブジェクト同士の &amp; 演算を行います。")]
        [SpecialName]
        public static VBString op_Concatenate(VBString a, VBString b) {
            return Concat(a, b);
        }

        /// <summary>
        /// 指定した VBString が等しいかどうかを判断します。
        /// </summary>
        /// <param name="a">第１の引数</param>
        /// <param name="b">第２の引数</param>
        /// <returns>等しい場合は true そうでないときは false を返します。</returns>
        public static bool Equals(VBString a, VBString b) {
            return (Compare(a, b, false) == 0);
        }

        #endregion

        #region 比較演算子


        /// <summary>
        /// 指定した 2 つの VBString オブジェクトを比較します。
        /// </summary>
        /// <param name="a">第１の引数</param>
        /// <param name="b">第２の引数</param>
        /// <returns>
        /// 0            : a = b
        /// 0 より大きい : a &gt; b
        /// 0 より小さい : a &lt; b
        /// </returns>
        /// <remarks>Nothing は String.Empty と同値として扱います。</remarks>
        public static int Compare(VBString a, VBString b) {
            return Compare(a, b, false);
        }

        /// <summary>
        /// 指定した 2 つの VBString オブジェクトを比較します。比較時に大文字小文字を区別するかどうかを設定できます。 
        /// </summary>
        /// <param name="a">第１の引数</param>
        /// <param name="b">第２の引数</param>
        /// <param name="ignoreCase">
        /// 大文字と小文字を区別して比較するか、区別せずに比較するかを示す Boolean。
        /// (true は、大文字と小文字を区別せずに比較することを示します。
        /// </param>
        /// <returns>
        /// 2 つの比較対照値の構文上の関係を示す 32 ビット符号付き整数。 
        /// 0 : 2 つの文字列は等価。
        /// 0 より小さい値 : a が b より小さい。
        /// 0 より大きい値 : a が b より大きくなっています。
        /// </returns>
        public static int Compare(VBString a, VBString b, bool ignoreCase) {

            // 両方 Null かインスタンスが同一ならゼロ
            if (object.ReferenceEquals((object)a, (object)b)) {
                return 0;
            }

            // a が null なら b のほうが大きい
            if ((object)a == null) {
                return -1;
            }

            // b が null なら a のほうが大きい
            if ((object)b == null) {
                return 1;
            }

            if (ignoreCase) {
                return CultureInfo.CurrentCulture.CompareInfo.Compare(a.ToString(), b.ToString(), CompareOptions.IgnoreCase);
            }

            return CultureInfo.CurrentCulture.CompareInfo.Compare(a.ToString(), b.ToString(), CompareOptions.None);

        }

        //=============================================================================
        // = 演算子
        // String クラスに == があるので String との比較も記述する必要がある
        //=============================================================================

        /// <summary>= 演算子</summary>
        public static bool operator ==(VBString a, VBString b) {
            return Compare(a, b) == 0;
        }
        /// <summary>= 演算子</summary>
        public static bool operator ==(string a, VBString b) {
            return Compare(a, b) == 0;
        }
        /// <summary>= 演算子</summary>
        public static bool operator ==(VBString a, string b) {
            return Compare(a, b) == 0;
        }
        /// <summary>= 演算子</summary>
        public static bool operator ==(object a, VBString b) {
            if (object.ReferenceEquals(a, (object)b)) return true;
            if (a == null) return false;
            VBString va = a as VBString;
            if ((object)va != null) return Compare(va, b) == 0;
            return Compare(Conversions.ToString(a), b) == 0;
        }
        /// <summary>= 演算子</summary>
        public static bool operator ==(VBString a, object b) {
            if (object.ReferenceEquals((object)a, b)) return true;
            if (b == null) return false;
            VBString vb = b as VBString;
            if ((object)vb != null) return Compare(a, vb) == 0;
            return Compare(a, Conversions.ToString(b)) == 0;
        }

        //=============================================================================
        // <> 演算子
        // String クラスに <> があるので String との比較も記述する必要がある
        //=============================================================================
        /// <summary>&lt;&gt; 演算子</summary>
        public static bool operator !=(VBString a, VBString b) {
            return Compare(a, b) != 0;
        }
        /// <summary>&lt;&gt; 演算子</summary>
        public static bool operator !=(string a, VBString b) {
            return Compare(a, b) != 0;
        }
        /// <summary>&lt;&gt; 演算子</summary>
        public static bool operator !=(VBString a, string b) {
            return Compare(a, b) != 0;
        }
        /// <summary>&lt;&gt; 演算子</summary>
        public static bool operator !=(object a, VBString b) {
            if (object.ReferenceEquals(a, (object)b)) return false;
            if (a == null) return true;
            VBString va = a as VBString;
            if ((object)va != null) return Compare(va, b) != 0;
            return Compare(Conversions.ToString(a), b) != 0;
        }
        /// <summary>&lt;&gt; 演算子</summary>
        public static bool operator !=(VBString a, object b) {
            if (object.ReferenceEquals((object)a, b)) return false;
            if (b == null) return true;
            VBString vb = b as VBString;
            if ((object)vb != null) return Compare(a, vb) != 0;
            return Compare(a, b.ToString()) != 0;
        }

        //=============================================================================
        // > 演算子
        //=============================================================================
        /// <summary>
        /// &gt; 演算子
        /// </summary>
        public static bool operator >(VBString a, VBString b) {
            return Compare(a, b) > 0;
        }

        //=============================================================================
        // < 演算子
        //=============================================================================
        /// <summary>
        /// &lt; 演算子
        /// </summary>
        public static bool operator <(VBString a, VBString b) {
            return Compare(a, b) < 0;
        }

        //=============================================================================
        // >= 演算子
        //=============================================================================
        /// <summary>
        /// &gt;= 演算子
        /// </summary>
        public static bool operator >=(VBString a, VBString b) {
            return Compare(a, b) >= 0;
        }

        //=============================================================================
        // <= 演算子
        //=============================================================================
        /// <summary>
        /// &lt;= 演算子
        /// </summary>
        public static bool operator <=(VBString a, VBString b) {
            return Compare(a, b) <= 0;
        }

        //=============================================================================
        // Like 演算子
        //=============================================================================
        /// <summary>
        /// Like 演算子
        /// </summary>
        [SpecialName]
        public static bool op_Like(VBString a, VBString b) {
            if ((object)a == null || (object)b == null)
                return false;

            return LikeOperator.LikeString(a.ToString(), b.ToString(), CompareMethod.Binary);
        }


        #endregion

        #region 部品

        /// <summary>
        /// ２つの VBString オブジェクトを結合します。
        /// </summary>
        /// <param name="a">連結する最初の文字列。</param>
        /// <param name="b">連結する 2 番目の文字列。</param>
        /// <returns></returns>
        public static VBString Concat(VBString a, VBString b) {
            if ((object)a == null) a = string.Empty;
            if ((object)b == null) b = string.Empty;

            // どちらも String なら String で結合
            if (a.VarType == VariantType.String & b.VarType == VariantType.String) {
                return string.Concat(a, b);
            }

            // どちらかが Byte() なら配列で結合

            int nLengthB = a.LengthB + b.LengthB;
            if (nLengthB == 0) {
                return string.Empty;
            }
            byte[] newValue = new byte[nLengthB];
            if (a.LengthB > 0) Array.Copy(a.ToByteArray(), newValue, a.LengthB);
            if (b.LengthB > 0) Array.Copy(b.ToByteArray(), 0, newValue, a.LengthB, b.LengthB);
            return newValue;

        }

        #endregion

        #region IComparable

        static VBString ConvertToVBString(object value) {
            if (value == null) return string.Empty;

            string stringValue = value as string;
            if (stringValue != null) return stringValue;

            byte[] byteValue = value as byte[];
            if (byteValue != null) return byteValue;

            if (value is bool) return (bool)value;
            if (value is byte) return (byte)value;
            if (value is char) return (char)value;
            if (value is short) return (short)value;
            if (value is int) return (int)value;
            if (value is long) return (long)value;
            if (value is sbyte) return (sbyte)value;
            if (value is ushort) return (ushort)value;
            if (value is uint) return (uint)value;
            if (value is ulong) return (ulong)value;
            if (value is decimal) return (decimal)value;
            if (value is float) return (float)value;
            if (value is double) return (double)value;
            if (value is DateTime) return (DateTime)value;

            throw new InvalidCastException();
        }

        int IComparable.CompareTo(object obj) {
            return Compare(this, ConvertToVBString(obj));
        }

        int IComparable<VBString>.CompareTo(VBString other) {
            return Compare(this, other);
        }

        #endregion

        #region IEnumerable

        /// <summary>
        /// VBString の指定した位置に対する要素を返します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public byte this[int index] {
            get { return m_Value[index]; }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return new VBStringEnumerator(this);
        }

        IEnumerator<byte> IEnumerable<byte>.GetEnumerator() {
            return new VBStringEnumerator(this);
        }

        /// <summary>
        /// VBString の IEnumerator です。
        /// </summary>
        public class VBStringEnumerator : IEnumerator, IEnumerator<byte>
        {
            private VBString m_VBString;
            private int m_CurrentIndex;

            internal VBStringEnumerator(VBString value) {
                m_VBString = value;
                m_CurrentIndex = -1;
            }

            object IEnumerator.Current {
                get { return m_VBString[m_CurrentIndex]; }
            }

            byte IEnumerator<byte>.Current {
                get { return m_VBString[m_CurrentIndex]; }
            }

            bool IEnumerator.MoveNext() {
                m_CurrentIndex++;
                return (m_CurrentIndex < m_VBString.LengthB);
            }

            void IEnumerator.Reset() {
                m_CurrentIndex = -1;
            }

            void IDisposable.Dispose() {
                m_VBString = null;
            }

        }

        #endregion

        #region IConvertible

        TypeCode IConvertible.GetTypeCode() {
            return TypeCode.String;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider) {
            return Convert.ToBoolean(this.ToString(), provider);
        }

        byte IConvertible.ToByte(IFormatProvider provider) {
            return Convert.ToByte(this.ToString(), provider);
        }

        char IConvertible.ToChar(IFormatProvider provider) {
            return Convert.ToChar(this.ToString(), provider);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) {
            return Convert.ToDateTime(this.ToString(), provider);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider) {
            return Convert.ToDecimal(this.ToString(), provider);
        }

        double IConvertible.ToDouble(IFormatProvider provider) {
            return Convert.ToDouble(this.ToString(), provider);
        }

        short IConvertible.ToInt16(IFormatProvider provider) {
            return Convert.ToInt16(this.ToString(), provider);
        }

        int IConvertible.ToInt32(IFormatProvider provider) {
            return Convert.ToInt32(this.ToString(), provider);
        }

        long IConvertible.ToInt64(IFormatProvider provider) {
            return Convert.ToInt64(this.ToString(), provider);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider) {
            return Convert.ToSByte(this.ToString(), provider);
        }

        float IConvertible.ToSingle(IFormatProvider provider) {
            return Convert.ToSingle(this.ToString(), provider);
        }

        string IConvertible.ToString(IFormatProvider provider) {
            return Convert.ToString(this.ToString(), provider);
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) {
            if (conversionType == typeof(byte[])) return (byte[])m_Value.Clone();

            if (conversionType == typeof(string)) return Convert.ToString(ToString(), provider);
            if (conversionType == typeof(bool)) return Convert.ToBoolean(ToString(), provider);
            if (conversionType == typeof(byte)) return Convert.ToByte(ToString(), provider);
            if (conversionType == typeof(char)) return Convert.ToChar(ToString(), provider);
            if (conversionType == typeof(short)) return Convert.ToInt16(ToString(), provider);
            if (conversionType == typeof(int)) return Convert.ToInt32(ToString(), provider);
            if (conversionType == typeof(long)) return Convert.ToInt64(ToString(), provider);
            if (conversionType == typeof(sbyte)) return Convert.ToSByte(ToString(), provider);
            if (conversionType == typeof(ushort)) return Convert.ToUInt16(ToString(), provider);
            if (conversionType == typeof(uint)) return Convert.ToUInt32(ToString(), provider);
            if (conversionType == typeof(ulong)) return Convert.ToUInt64(ToString(), provider);
            if (conversionType == typeof(decimal)) return Convert.ToDecimal(ToString(), provider);
            if (conversionType == typeof(float)) return Convert.ToSingle(ToString(), provider);
            if (conversionType == typeof(double)) return Convert.ToDouble(ToString(), provider);
            if (conversionType == typeof(DateTime)) return Convert.ToDateTime(ToString(), provider);

            return new InvalidCastException();
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider) {
            return Convert.ToUInt16(this.ToString(), provider);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider) {
            return Convert.ToUInt32(this.ToString(), provider);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider) {
            return Convert.ToUInt64(this.ToString(), provider);
        }

        #endregion
    }
}
