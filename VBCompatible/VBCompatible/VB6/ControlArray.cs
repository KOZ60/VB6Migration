namespace VBCompatible.VB6
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;

    public abstract class ControlArray<T> : BaseControlArray,
                                            IExtenderProvider, IEnumerable<T>
                                            where T : Control, new()
    {

        protected ControlArray() : base() { }

        protected ControlArray(IContainer Container) : base(Container) { }

        protected sealed override Type GetControlInstanceType => typeof(T);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool CanExtend(object extendee) {
            return BaseCanExtend(extendee);
        }

        public T this[int Index] {
            get {
                return (T)BaseGet(Index);
            }
        }

        public int GetIndex(object o) {
            return BaseGetIndex(o as T);
        }

        public void SetIndex(object o, int Index) {
            BaseSetIndex(o as T, Index);
        }

        public void ResetIndex(object o) {
            BaseResetIndex(o as T);
        }

        public bool ShouldSerializeIndex(object o) {
            return BaseShouldSerializeIndex(o as T);
        }

        public T Load(int Index) {
            return (T)BaseLoad(Index);
        }

        public void Unload(int Index) {
            BaseUnload(Index);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            foreach (var kp in controls) {
                yield return (T)kp.Value;
            }
        }
    }
}
