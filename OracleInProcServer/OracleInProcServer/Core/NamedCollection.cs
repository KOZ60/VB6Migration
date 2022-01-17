using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleInProcServer.Core
{
    public class NamedCollection<T> : NameObjectCollectionBase, IEnumerable<T>, IEnumerable
    {
        public NamedCollection() : base(StringComparer.CurrentCultureIgnoreCase) { }

        public void Clear() {
            base.BaseClear();
        }

        public void  Add(string name, T value) {
            base.BaseAdd(name, value);
        }

        public void Remove(string name) {
            base.BaseRemove(name);
        }

        public void Remove(int index) {
            base.BaseRemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            foreach (var item in base.BaseGetAllValues()) {
                yield return item;
            }
        }

        public new IEnumerator<T> GetEnumerator() {
            foreach (var item in base.BaseGetAllValues()) {
                yield return (T)item;
            }
        }

        public T this[int index] {
            get {
                return (T)base.BaseGet(index);
            }
            set {
                base.BaseSet(index, value);
            }
        }

        public T this[string name] {
            get {
                return (T)base.BaseGet(name);
            }
            set {
                base.BaseSet(name, value);
            }
        }

    }
}
