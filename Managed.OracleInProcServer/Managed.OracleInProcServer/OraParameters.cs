using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Managed.OracleInProcServer.Core;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Managed.OracleInProcServer
{
    public class OraParameters : OraObject, IEnumerable<OraParameter>, OraParamArrays
    {
        NamedCollection<OraParameter> _Parameters;

        public OraParameters() {
            _Parameters = new NamedCollection<OraParameter>();
        }

        public int Count {
            get {
                return _Parameters.Count;
            }
        }

        public void Clear() {
            if (_Parameters != null) {
                foreach (var p in _Parameters) {
                    p.Dispose();
                }
                _Parameters.Clear();
            }
        }

        public OraParamArray AddTable(string name, paramMode IOtype, serverType varType, int dimension) {
            var p = new OraParamArray(name, IOtype, varType, dimension);
            _Parameters.Add(name, p);
            return p;
        }

        public OraParamArray AddTable(string name, paramMode IOtype, serverType varType, int dimension, int size) {
            var p = AddTable(name, IOtype, varType, dimension);
            p.MinimumSize = size;
            return p;
        }

        public OraParameter Add(string name, object initval, paramMode IOtype, serverType varType) {
            // OracleParameter.Value を設定すると DbType が決定され
            // DbType を変更する場合は Value プロパティを再設定しなければならず
            // 面倒くさくなるという事情で serverType は省略不可としました。
            var p = new OraParameter(name, IOtype, varType);
            OraBFile bfile = initval as OraBFile;
            if (bfile == null) {
                p.Value = initval;
            } else {
                p.Value = bfile.oraclebFile;
            }
            _Parameters.Add(name, p);
            return p;
        }

        public OraParameter this[int index] {
            get {
                return _Parameters[index];
            }
        }

        public OraParameter this[string name] {
            get {
                return _Parameters[name];
            }
        }

        public void Remove(string name) {
            _Parameters.Remove(name);
        }

        public void Remove(int index) {
            _Parameters.Remove(index);
        }

        protected override void Dispose(bool disposing) {
            Clear();
            _Parameters = null;
        }

        public IEnumerator<OraParameter> GetEnumerator() {
            return _Parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _Parameters.GetEnumerator();
        }

        OraParamArray OraParamArrays.this[string name] {
            get {
                return _Parameters[name] as OraParamArray;
            }
        }
        OraParamArray OraParamArrays.this[int index] {
            get {
                return _Parameters[index] as OraParamArray; 
            }
        }

        IEnumerator<OraParamArray> IEnumerable<OraParamArray>.GetEnumerator() {
            foreach (var parameter in _Parameters) {
                yield return (OraParamArray)parameter;
            }
        }

    }
}
