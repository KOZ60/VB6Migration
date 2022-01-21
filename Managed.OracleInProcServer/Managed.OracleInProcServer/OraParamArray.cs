using System;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Managed.OracleInProcServer.Core;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Managed.OracleInProcServer
{
    public class OraParamArray : OraParameter
    {
        int _MinimumSize;

        internal OraParamArray(string name, paramMode IOtype, serverType serverType, int dimension)
            : base(name, IOtype, serverType) {

            // 配列の場合、OracleParameter.Size は配列サイズになり、項目のサイズは ArrayBindSize になる。
            Parameter.Size = dimension;

            // 配列の値を初期化
            this.ArrayValue = new object[dimension];

            // string の場合、初期値は 127
            if (serverType.IsString()) {
                MinimumSize = 127;
            }

            // ArrayBindStatus を初期化
            this.ArrayBindStatus = new OracleParameterStatus[dimension];
        }

        protected override bool IsArray {
            get {
                return true;
            }
        }

        public override int MinimumSize {
            get {
                return _MinimumSize;
            }
            set {
                _MinimumSize = value;
                var arrayBindSize = new int[ArraySize];
                for (int i = 0; i < ArraySize; i++) {
                    arrayBindSize[i] = value;
                }
                this.ArrayBindSize = arrayBindSize;
            }
        }

        internal override int ArraySize {
            get {
                return Parameter.Size;
            }
        }

        protected int[] ArrayBindSize {
            get {
                return Parameter.ArrayBindSize;
            }
            set {
                Parameter.ArrayBindSize = value;
            }
        }

        protected OracleParameterStatus[] ArrayBindStatus {
            get {
                return Parameter.ArrayBindStatus;
            }
            set {
                Parameter.ArrayBindStatus = value;
            }
        }

        protected Array ArrayValue {
            get {
                return (Array)Parameter.Value;
            }
            set {
                Parameter.Value = value;
            }
        }

        public override object get_Value(int index) {
            return ArrayValue.GetValue(index);
        }

        public override void put_Value(object newval, int index) {
            object obj = UTL.ConvertTo(newval, this.serverType);
            ArrayValue.SetValue(obj.ToBlankStrip(true), index);
        }

        public override object this[int index] {
            get {
                return get_Value(index);
            }
            set {
                put_Value(value, index);
            }
        }
    }
}
