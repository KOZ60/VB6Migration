using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace MSWinsockLib
{
    /// <summary>
    /// Winsock コンポーネントの配列をサポートします。
    /// </summary>
    [ProvideProperty("Index", typeof(Winsock))]
    public class WinsockArray : VBBaseComponentArray<Winsock>
    {
        /// <summary>
        /// WinsockArray のインスタンスを作成します。
        /// </summary>
        public WinsockArray() { }

        /// <summary>
        /// WinsockArray のインスタンスを作成します。
        /// </summary>
        /// <param name="Container">コンテナを指定します。</param>
        public WinsockArray(IContainer Container) : base(Container) { }

        /// <summary>
        /// コンポーネント配列内のコンポーネントのイベント ハンドラを追加します。
        /// </summary>
        /// <param name="o">対象のコンポーネント</param>
        protected override void HookUpControlEvents(object o)
        {
            base.HookUpControlEvents(o);

            Winsock target = (Winsock)o;
            if (this.CloseEvent != null)        target.CloseEvent += this.CloseEvent;
            if (this.ConnectEvent != null)      target.ConnectEvent += this.ConnectEvent;
            if (this.ConnectionRequest != null) target.ConnectionRequest += this.ConnectionRequest;
            if (this.DataArrival != null)       target.DataArrival += this.DataArrival;
            if (this.Error != null)             target.Error += this.Error;
            if (this.SendComplete != null)      target.SendComplete += this.SendComplete;
            if (this.SendProgress != null)      target.SendProgress += this.SendProgress;
        }

        /// <summary>
        /// ソケットが閉じられたときに発生します。
        /// </summary>
        public event EventHandler CloseEvent;

        /// <summary>
        /// 接続したときに発生します。
        /// </summary>
        public event EventHandler ConnectEvent;

        /// <summary>
        /// リモート マシンが接続を要求してきたときに発生します。 
        /// </summary>
        public event ConnectionRequestEventHandler ConnectionRequest;

        /// <summary>
        /// 新しいデータが送られてきたときに発生します。
        /// </summary>
        public event DataArrivalEventHandler DataArrival;

        /// <summary>
        /// バックグラウンド処理でエラーが発生したとき (たとえば、接続に失敗したとき、バックグラウンドで実行している送信や受信に失敗したときなど) に発生します。
        /// </summary>
        public event ErrorEventHandler Error;

        /// <summary>
        /// 送信処理が完了したときに発生します。
        /// </summary>
        public event EventHandler SendComplete;

        /// <summary>
        /// データの送信中に発生します。
        /// </summary>
        public event SendProgressEventHandler SendProgress;

    }
}
