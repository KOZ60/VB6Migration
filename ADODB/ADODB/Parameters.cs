using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADODB
{
    /// <summary>
    /// Parameter オブジェクトのコレクション
    /// </summary>
    public class Parameters : NameObjectCollectionBase, IEnumerable<Parameter>
    {
        internal Parameters() : base(StringComparer.InvariantCultureIgnoreCase) { }

        /// <summary>
        /// コレクションの特定のメンバをインデックスで取得します。
        /// </summary>
        /// <param name="index">取得するメンバを表すインデックス。</param>
        /// <returns>Parameter オブジェクト。</returns>
        public Parameter this[int index] {
            get {
                return (Parameter)base.BaseGet(index);
            }
        }

        /// <summary>
        /// コレクションの特定のメンバを名前で取得します。
        /// </summary>
        /// <param name="name">取得するメンバを表す名前。</param>
        /// <returns>Parameter オブジェクト。</returns>
        public Parameter this[string name] {
            get {
                return (Parameter)base.BaseGet(name);
            }
        }

        /// <summary>
        /// Parameter をコレクションに追加します。
        /// </summary>
        /// <param name="parameter">追加する Parameter オブジェクト。</param>
        public void Append(Parameter parameter) {
            base.BaseAdd(parameter.Name, parameter);
        }

        /// <summary>
        /// インデックスを指定してコレクションから Parameter を削除します。
        /// </summary>
        /// <param name="index">削除するパラメータのインデックス。</param>
        public void Delete(int index) {
            base.BaseRemoveAt(index);
        }

        /// <summary>
        /// 名前を指定してコレクションから Parameter を削除します。
        /// </summary>
        /// <param name="name">削除するパラメータの名前。</param>
        public void Delete(string name) {
            base.BaseRemove(name);
        }

        /// <summary>
        /// Parameter オブジェクトを列挙する IEnumerator オブジェクトを返します。
        /// </summary>
        /// <returns></returns>
        public override IEnumerator GetEnumerator() {
            foreach (Parameter p in base.BaseGetAllValues()) {
                yield return p;
            }
        }

        IEnumerator<Parameter> IEnumerable<Parameter>.GetEnumerator() {
            foreach (Parameter p in base.BaseGetAllValues()) {
                yield return p;
            }
        }

    }
}
