using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADODB
{
    /// <summary>
    /// パラメータ指定クエリーやストアド プロシージャに基づいて Command オブジェクトに関連付けられたパラメータや引数を表すオブジェクト
    /// </summary>
    public class Parameter
    {
        private IDbDataParameter _DbParameter = null;
        private string _Name;
        private object _Value;
        private ParameterDirectionEnum _Direction;
        private ParameterAttributesEnum _Attributes;
        private byte _NumericScale;
        private byte _Precision;
        private int _Size;
        private DataTypeEnum _Type;

        internal Parameter() { }

        internal bool IsCreated {
            get {
                return _DbParameter != null;
            }
        }

        internal IDbDataParameter GetDbParameter(IDbCommand cmd) {
            if (_DbParameter == null) {
                IDbDataParameter p = cmd.CreateParameter();
                p.ParameterName = _Name;
                p.Direction = UTL.ToDirection(_Direction);
                p.DbType = UTL.ToDbType(_Type);
                p.Value = _Value;
                p.Size = _Size;
                p.Scale = _NumericScale;
                p.Precision = _Precision;
                _DbParameter = p;
            }
            return _DbParameter;
        }

        /// <summary>
        /// 名前を取得または設定します。
        /// </summary>
        public string Name {
            get {
                return _Name;
            }
            set {
                _Name = value;
                if (IsCreated) {
                    _DbParameter.ParameterName = value;
                }
            }
        }

        /// <summary>
        /// パラメータの値を取得または設定します。
        /// </summary>
        public object Value {
            get {
                if (IsCreated) {
                    return _DbParameter.Value;
                }
                return _Value;
            }
            set {
                _Value = value;
                if (IsCreated) {
                    _DbParameter.Value = value;
                }
            }
        }

        /// <summary>
        /// 入力パラメータ、出力パラメータ、入出力パラメータ、あるいはストアド プロシージャからの戻り値のどれであるかを示す値を取得または設定します。
        /// </summary>
        public ParameterDirectionEnum Direction {
            get {
                return _Direction;
            }
            set {
                _Direction = value;
                if (IsCreated) {
                    _DbParameter.Direction = UTL.ToDirection(value);
                }
            }
        }

        /// <summary>
        /// パラメータの属性を表す値を取得または設定します。
        /// </summary>
        public ParameterAttributesEnum Attributes {
            get {
                return _Attributes;
            }
            set {
                _Attributes = value;
            }
        }

        /// <summary>
        /// 数値パラメータの小数部桁数を取得または設定します。
        /// </summary>
        public byte NumericScale {
            get {
                return _NumericScale;
            }
            set {
                _NumericScale = value;
                if (IsCreated) {
                    _DbParameter.Scale = value;
                }
            }
        }

        /// <summary>
        /// 数値パラメータの有効桁数を取得または設定します。
        /// </summary>
        public byte Precision {
            get {
                return _Precision;
            }
            set {
                _Precision = value;
                if (IsCreated) {
                    _DbParameter.Precision = value;
                }
            }
        }

        /// <summary>
        /// パラメータのサイズを取得または設定します。
        /// </summary>
        public int Size {
            get {
                return _Size;
            }
            set {
                _Size = value;
                if (IsCreated) {
                    _DbParameter.Size = value;
                }
            }
        }

        /// <summary>
        /// パラメータの型を取得または設定します。
        /// </summary>
        public DataTypeEnum Type {
            get {
                return _Type;
            }
            set {
                _Type = value;
                if (IsCreated) {
                    _DbParameter.DbType = UTL.ToDbType(value);
                }
            }
        }

    }
}
