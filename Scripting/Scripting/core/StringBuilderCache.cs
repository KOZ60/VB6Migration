using System;
using System.Collections.Generic;
using System.Text;

namespace Scripting
{

    // 32,000 文字対応の StringBuilder をキャッシュする

    [Serializable]
    internal class StringBuilderCache : IDisposable
    {
        private StringBuilder _Instance;

        public StringBuilderCache() {
            _Instance = Dispenser.Acquire();
        }

        public int Length {
            get { return _Instance.Length; }
        }

        public void Append(char value) {
            _Instance.Append(value);
        }

        public void Append(string value) {
            _Instance.Append(value);
        }

        public static implicit operator StringBuilder(StringBuilderCache value) {
            return value._Instance;
        }

        public override string ToString() {
            return _Instance.ToString();
        }

        public int Capacity {
            get { return _Instance.Capacity; }
        }

        public void Dispose() {
            if (_Instance != null) {
                Dispenser.Release(_Instance);
                _Instance = null;
            }
            GC.SuppressFinalize(this);
        }

        internal static class Dispenser
        {
            private const int MAX_BUILDER_SIZE = 32000;

            [ThreadStatic]
            private static StringBuilder CachedInstance;

            public static StringBuilder Acquire() {
                StringBuilder sb = Dispenser.CachedInstance;
                if (sb != null) {
                    Dispenser.CachedInstance = null;
                    sb.Length = 0;
                    return sb;
                }
                return new StringBuilder(MAX_BUILDER_SIZE);
            }

            public static void Release(StringBuilder sb) {
                Dispenser.CachedInstance = sb;
            }
        }
    }

}
