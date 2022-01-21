using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Managed.OracleInProcServer.Core;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Managed.OracleInProcServer
{
    public class OraFields : IEnumerable<OraField>
    {
        OraDynaset _Dynaset;
        DataColumnCollection _Columns;
        Dictionary<DataColumn, OraField> _dic;

        internal OraFields(OraDynaset dyn, SchemaInformation[] sc) {
            _Dynaset = dyn;
            _Columns = dyn.Table.Columns;
            _dic = new Dictionary<DataColumn, OraField>();
            for (int i = 0; i < sc.Length; i++) {
                var dc = dyn.Table.Columns[i];
                _dic.Add(dc, new OraField(dyn, dc, sc[i]));
            }
        }

        public int Count {
            get {
                return _dic.Count;
            }
        }

        public OraField this[int index] {
            get {
                DataColumn dc = _Columns[index];
                return _dic[dc];
            }
        }

        public OraField this[string name] {
            get {
                DataColumn dc = _Columns[name];
                return _dic[dc];
            }
        }

        public IEnumerator<OraField> GetEnumerator() {
            foreach (var item in _dic.Values) {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            foreach (var item in _dic.Values) {
                yield return item;
            }
        }
    }

}
