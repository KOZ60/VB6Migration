using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleInProcServer.Core
{
    abstract class ConvertDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        Dictionary<TKey, TValue> dic;
        Dictionary<TValue, TKey> reverse;

        public ConvertDictionary() {
            dic = new Dictionary<TKey, TValue>();
            reverse = new Dictionary<TValue, TKey>();
        }

        public ConvertDictionary(IEqualityComparer<TKey> comparer) {
            dic = new Dictionary<TKey, TValue>(comparer);
            reverse = new Dictionary<TValue, TKey>();
        }

        protected void Add(TKey key, TValue value) {
            dic[key] = value;
            if (!reverse.ContainsKey(value)) {
                reverse.Add(value, key);
            }
        }

        public TValue this[TKey key] {
            get {
                TValue value;
                if (!dic.TryGetValue(key, out value)) {
                    value = CashOut(key);
                    dic.Add(key, value);
                }
                return value;
            }
        }

        public TKey this[TValue key] {
            get {
                return reverse[key];
            }
        }

        protected abstract TValue CashOut(TKey key);

        public IEnumerable<TKey> Keys {
            get {
                return dic.Keys;
            }
        }

        public IEnumerable<TValue> Values {
            get {
                return dic.Values;
            }
        }

        public int Count {
            get {
                return dic.Count;
            }
        }

        public bool ContainsKey(TKey key) {
            return dic.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value) {
            return dic.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            return dic.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return dic.GetEnumerator();
        }
    }
}
