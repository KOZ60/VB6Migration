using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OracleInProcServer.Core;

namespace OracleInProcServer
{
    public class OraParameter : OraObject
    {
        DbParameter m_Parameter;

        protected OraParameter() {
            // OracleParameter 作成
            m_Parameter = Factory.CreateParameter();
            // 自動バインドする
            AutoBindEnable();
        }

        public OraParameter(string name, paramMode IOtype, serverType serverType)
            : this() {
            this.Name = name;
            this.IOtype = IOtype;
            this.serverType = serverType;

            // string の場合、初期値は 127
            if (serverType.IsString()) {
                MinimumSize = 127;
            }
        }

        public virtual DbParameter Parameter {
            get {
                return m_Parameter;
            }
        }

        protected virtual bool IsArray {
            get {
                return false;
            }
        }

        protected override void Dispose(bool disposing) {
            // OracleParameter は IDisposable
            if (m_Parameter != null) {
                m_Parameter.Dispose();
                m_Parameter = null;
            }
        }

        internal DbParameter CreateDbParameter() {
            // DbParameter を Clone する
            var p = (DbParameter)m_Parameter.Invoke("Clone");

            // Clone できないプロパティをコピー
            p.SetProperty("ArrayBindSize", m_Parameter.GetProperty<object>("ArrayBindSize"));
            p.SetProperty("ArrayBindStatus", m_Parameter.GetProperty<object>("ArrayBindStatus"));

            // 元の DbParameter を破棄
            m_Parameter.Dispose();

            // 新しい DbParameter をセット
            m_Parameter = p;
            return m_Parameter;
        }

        public bool AutoBind { get; private set; }

        public void AutoBindDisable() {
            AutoBind = false;
        }

        public void AutoBindEnable() {
            AutoBind = true;
        }

        public virtual int MinimumSize {
            get {
                return Parameter.Size;
            }
            set {
                Parameter.Size = value;
            }
        }

        public virtual string Name {
            get {
                return Parameter.ParameterName;
            }
            private set {
                Parameter.ParameterName = value;
            }
        }

        public virtual serverType serverType {
            get {
                var oracleDbType = (OracleDbType)Parameter.GetProperty<int>("OracleDbType");
                return oracleDbType.ToServerType();
            }
            set {
                var oracleDbType = value.ToOracleDbType();
                Parameter.SetProperty("OracleDbType", (int)oracleDbType);
            }
        }

        protected virtual paramMode IOtype {
            get {
                return Parameter.Direction.ToParamMode();
            }
            set {
                Parameter.Direction = value.ToParameterDirection();
            }
        }

        public virtual object Value {
            get {
                return Parameter.Value.ToBlankStrip(true);
            }
            set {
                Parameter.Value = UTL.ConvertTo(value, this.serverType).ToBlankStrip(true);
            }
        }

        internal virtual int ArraySize {
            get {
                return 0;
            }
        }

        public virtual object get_Value(int index) {
            throw new NotSupportedException("The method or operation is not implemented.");
        }

        public virtual void put_Value(object newval, int index) {
            throw new NotSupportedException("The method or operation is not implemented.");
        }

        public virtual object this[int index] {
            get { throw new NotSupportedException("The method or operation is not implemented."); }
            set { throw new NotSupportedException("The method or operation is not implemented."); }
        }

    }
}
