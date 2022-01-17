using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;

namespace DAO
{

    internal static class MultiCastProxy
    {
        public static T CreateTransparentProxy<T>(IList<T> targets)
            where T : MarshalByRefObject {
            var proxy = new InternalProxy<T>(targets);
            return proxy.GetTransparentProxy();
        }

        /// <summary>
        /// 複数のオブジェクトに対し、メソッドを実行するプロキシ
        /// </summary>
        internal class InternalProxy<T> : RealProxy
           where T : MarshalByRefObject
        {
            IList<T> _Targets;

            /// <summary>
            /// 指定したオブジェクトに対するプロキシを作成します。
            /// </summary>
            /// <param name="targets">対象のオブジェクト</param>
            public InternalProxy(IList<T> targets)
                : base(typeof(T)) {
                _Targets = targets;
            }

            /// <summary>
            /// メソッドを実行します。
            /// </summary>
            /// <param name="msg">派生クラスでオーバーライドされた場合は、提供された IMessage で指定されたメソッドを、現在のインスタンスが表すリモート オブジェクトで呼び出します。</param>
            /// <returns>呼び出されたメソッドが返すメッセージで、out パラメーターまたは ref パラメーターのどちらかと戻り値を格納しているメッセージ。</returns>
            public sealed override IMessage Invoke(IMessage msg) {
                try {
                    IMethodMessage mm = msg as IMethodMessage;
                    object[] args = mm.Args;
                    MethodInfo method = (MethodInfo)mm.MethodBase;
                    object ret = Invoke(method, args);
                    return new ReturnMessage(
                        ret, args, args.Length, mm.LogicalCallContext, (IMethodCallMessage)msg);

                } catch (Exception ex) {
                    if (ex.InnerException != null)
                        return new ReturnMessage(ex.InnerException, (IMethodCallMessage)msg);
                    return new ReturnMessage(ex, (IMethodCallMessage)msg);
                }
            }

            /// <summary>
            /// ターゲットのインスタンス
            /// </summary>
            public IList<T> Targets {
                get {
                    return _Targets;
                }
            }

            /// <summary>
            /// メソッドを実行し、値を取得します。
            /// </summary>
            /// <param name="mi">実行するメソッドの MethodInfo</param>
            /// <param name="args">実行するメソッドの引数</param>
            /// <returns>メソッドを実行した結果</returns>
            public virtual object Invoke(MethodInfo mi, object[] args) {
                object result = null;
                for (int i = 0; i < this.Targets.Count; i++) {
                    result = mi.Invoke(this.Targets[i], args);
                }
                return result;
            }

            /// <summary>
            /// 透過プロキシを返します。
            /// </summary>
            /// <returns>透過プロキシ</returns>
            public new T GetTransparentProxy() {
                return (T)base.GetTransparentProxy();
            }
        }
    }

}
