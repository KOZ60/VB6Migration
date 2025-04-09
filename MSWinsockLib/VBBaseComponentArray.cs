using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace MSWinsockLib
{

    /// <summary>
    /// VB6.0 互換コンポーネントの配列に共通の基本的な機能を実装するジェネリッククラスです。
    /// </summary>
    public abstract class VBBaseComponentArray<T> : BaseComponentArray, IExtenderProvider
        where T : Component
    {
        /// <summary>
        /// コンポーネントがコンポーネント配列のメンバかどうかを示す値を取得します。
        /// </summary>
        /// <param name="target">テスト対象のコンポーネント</param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool CanExtend(object target)
        {
            bool flag = false;
            if (!(target is T))
            {
                return flag;
            }
            return this.BaseCanExtend(RuntimeHelpers.GetObjectValue(target));
        }

        /// <summary>
        /// コンポーネント配列内のコンポーネントの型を返します。
        /// </summary>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override Type GetControlInstanceType()
        {
            return typeof(T);
        }

        /// <summary>
        /// コンポーネント配列内のコンポーネントを取得します。
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        public T this[short Index]
        {
            get { return (T)this.BaseGetItem(Index); }
        }

        /// <summary>
        /// VBBaseComponentArray のインスタンスを作成します。
        /// </summary>
        public VBBaseComponentArray()
        {
        }

        /// <summary>
        /// IContainer を指定して VBBaseComponentArray のインスタンスを作成します。
        /// </summary>
        /// <param name="Container">コンポーネントを格納しているオブジェクト</param>
        public VBBaseComponentArray(IContainer Container)
            : base(Container)
		{
		}

        /// <summary>
        /// コンポーネントのインデックスを取得します。
        /// </summary>
        /// <param name="o">対象のコンポーネント</param>
        /// <returns></returns>
        public short GetIndex(T o)
        {
            return this.BaseGetIndex(o);
        }

        /// <summary>
        /// このメソッドはサポートされていません。
        /// </summary>
        /// <param name="o"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetIndex(T o)
        {
            this.BaseResetIndex(o);
        }

        /// <summary>
        /// コンポーネント配列内のコンポーネントのインデックスを設定します。
        /// </summary>
        /// <param name="o">インデックスを設定する対象の Control</param>
        /// <param name="Index">コンポーネントのインデックスを表す Short 整数。 </param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetIndex(T o, short Index)
        {
            this.BaseSetIndex(o, Index, false);
        }

        /// <summary>
        /// コンポーネントがコンポーネント配列のメンバーかどうかを示す値を返します。
        /// </summary>
        /// <param name="o">コントローrを指定します。</param>
        /// <returns>そのコンポーネントがコンポーネント配列のメンバーである場合は true。それ以外の場合は false。 </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeIndex(T o)
        {
            return this.BaseShouldSerializeIndex(o);
        }

        /// <summary>
        /// コンポーネント配列内のコンポーネントのイベント ハンドラを追加します。
        /// </summary>
        /// <param name="o">対象のコンポーネント</param>
        protected override void HookUpControlEvents(object o)
        {
            T target = (T)o;
            if (this.Disposed != null) target.Disposed += this.Disposed;
        }

        /// <summary>
        /// コンポーネントが破棄されたときに発生します。
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public new event EventHandler Disposed;
    }
}
