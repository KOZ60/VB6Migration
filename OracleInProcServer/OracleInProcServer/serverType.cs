using System.ComponentModel;

namespace OracleInProcServer
{
    /// <summary>
    /// バインド変数のOracle外部型
    /// </summary>
    /// <remarks>
    /// 定義はあるがマニュアルに記載されていないもの多数
    /// </remarks>
    public enum serverType
    {
        /// <summary>
        /// BOOL
        /// </summary>
        ORATYPE_BOOL = 0,
        /// <summary>
        /// VARCHAR2
        /// </summary>
        ORATYPE_VARCHAR2 = 1,
        /// <summary>
        /// NUMBER
        /// </summary>
        ORATYPE_NUMBER = 2,
        /// <summary>
        /// SIGNED INTEGER
        /// </summary>
        ORATYPE_SINT = 3,
        /// <summary>
        /// FLOAT
        /// </summary>
        ORATYPE_FLOAT = 4,
        /// <summary>
        /// NULL文字で終了するSTRING
        /// </summary>
        ORATYPE_STRING = 5,
        /// <summary></summary>
        ORATYPE_DECIMAL = 7,
        /// <summary>
        /// LONG
        /// </summary>
        ORATYPE_LONG = 8,
        /// <summary>
        /// VARCHAR
        /// </summary>
        ORATYPE_VARCHAR = 9,
        /// <summary>
        /// DATE
        /// </summary>
        ORATYPE_DATE = 12,
        /// <summary></summary>
        ORATYPE_REAL = 21,
        /// <summary></summary>
        ORATYPE_DOUBLE = 22,
        /// <summary>
        /// RAW
        /// </summary>
        ORATYPE_RAW = 23,
        /// <summary>
        /// LONG RAW
        /// </summary>
        ORATYPE_LONGRAW = 24,
        /// <summary></summary>
        ORATYPE_UNSIGNED16 = 25,
        /// <summary></summary>
        ORATYPE_UNSIGNED32 = 26,
        /// <summary></summary>
        ORATYPE_SIGNED8 = 27,
        /// <summary></summary>
        ORATYPE_SIGNED16 = 28,
        /// <summary></summary>
        ORATYPE_SIGNED32 = 29,
        /// <summary></summary>
        ORATYPE_UNSIGNED64 = 30,
        /// <summary></summary>
        ORATYPE_SIGNED64 = 31,
        /// <summary></summary>
        ORATYPE_PTR = 32,
        /// <summary></summary>
        ORATYPE_OPAQUE = 58,
        /// <summary>
        /// UNSIGNED INTEGER
        /// </summary>
        ORATYPE_UINT = 68,
        /// <summary>
        /// CHAR
        /// </summary>
        ORATYPE_CHAR = 96,
        /// <summary>
        /// NULL文字で終了するCHAR
        /// </summary>
        ORATYPE_CHARZ = 97,
        /// <summary>
        /// BINARY_FLOAT
        /// </summary>
        ORATYPE_BFLOAT = 100,
        /// <summary>
        /// BINARY_DOUBLE
        /// </summary>
        ORATYPE_BDOUBLE = 101,
        /// <summary>
        /// PLSQL CURSOR
        /// </summary>
        ORATYPE_CURSOR = 102,
        /// <summary></summary>
        ORATYPE_ROWID = 104,
        /// <summary>
        /// MLSLABEL
        /// </summary>
        ORATYPE_MLSLABEL = 105,
        /// <summary>
        /// OBJECT
        /// </summary>
        ORATYPE_OBJECT = 108,
        /// <summary>
        /// REF
        /// </summary>
        ORATYPE_REF = 110,
        /// <summary>
        /// CLOB
        /// </summary>
        ORATYPE_CLOB = 112,
        /// <summary>
        /// BLOB
        /// </summary>
        ORATYPE_BLOB = 113,
        /// <summary>
        /// BFILE
        /// </summary>
        ORATYPE_BFILE = 114,
        /// <summary>
        /// CFILE
        /// </summary>
        ORATYPE_CFILE = 115,
        /// <summary></summary>
        ORATYPE_RSLT = 116,
        /// <summary></summary>
        ORATYPE_XML = 117,
        /// <summary></summary>
        ORATYPE_NAMEDCOLLECTION = 122,
        /// <summary>
        /// TIMESTAMP
        /// </summary>
        ORATYPE_TIMESTAMP = 187,
        /// <summary>
        /// TIMESTAMP WITH TIMEZONE
        /// </summary>
        ORATYPE_TIMESTAMPTZ = 188,
        /// <summary>
        /// INTERVAL YEAR TO MONTH
        /// </summary>
        ORATYPE_INTERVALYM = 189,
        /// <summary>
        /// INTERVAL DAY TO SECOND
        /// </summary>
        ORATYPE_INTERVALDS = 190,
        /// <summary></summary>
        ORATYPE_SYSFIRST = 228,
        /// <summary>
        /// TIMESTAMP WITH LOCAL TIME ZONE
        /// </summary>
        ORATYPE_TIMESTAMPLTZ = 232,
        /// <summary></summary>
        ORATYPE_SYSLAST = 235,
        /// <summary></summary>
        ORATYPE_OCTET = 245,
        /// <summary></summary>
        ORATYPE_SMALLINT = 246,
        /// <summary>
        /// VARRAY
        /// </summary>
        ORATYPE_VARRAY = 247,
        /// <summary>
        /// NESTED TABLE
        /// </summary>
        ORATYPE_TABLE = 248,
        /// <summary></summary>
        ORATYPE_OTMLAST = 320,
        /// <summary>
        /// RAW
        /// </summary>
        ORATYPE_RAW_BIN = 2000,
        /// <summary>
        /// NVARCHAR2
        /// </summary>
        ORATYPE_NVARCHAR2 = 10001,
        /// <summary>
        /// NVARCHAR
        /// </summary>
        ORATYPE_NVARCHAR = 10009,
        /// <summary>
        /// NCHAR
        /// </summary>
        ORATYPE_NCHAR = 10096,
        /// <summary>
        /// NULL文字で終了するCHAR
        /// </summary>
        ORATYPE_NCHARZ = 10097,
        /// <summary>
        /// CLOB
        /// </summary>
        ORATYPE_NCLOB = 10112,
    }
}
