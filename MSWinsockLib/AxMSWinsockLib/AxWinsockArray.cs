using System;
using System.ComponentModel;
using MSWinsockLib;

namespace AxMSWinsockLib
{
    /// <summary>
    /// AxWinsock コンポーネントの配列をサポートします。
    /// </summary>
    [ProvideProperty("Index", typeof(Winsock))]
    public class AxWinsockArray : VBBaseComponentArray<AxWinsock>
    {
        /// <summary>
        /// AxWinsockArray のインスタンスを作成します。
        /// </summary>
        public AxWinsockArray() : base() { }

        /// <summary>
        /// コンテナを指定して AxWinsockArray のインスタンスを作成します。
        /// </summary>
        /// <param name="Container">コンテナを指定します。</param>
        public AxWinsockArray(IContainer Container) : base(Container) { }

        /// <summary>
        /// コントロールのイベントをフックします。
        /// </summary>
        /// <param name="o">対象のコントロール</param>
        protected override void HookUpControlEvents(object o)
        {
            base.HookUpControlEvents(o);

            AxWinsock target = (AxWinsock)o;
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
        public event DMSWinsockControlEvents_ConnectionRequestEventHandler ConnectionRequest;

        /// <summary>
        /// 新しいデータが送られてきたときに発生します。
        /// </summary>
        public event DMSWinsockControlEvents_DataArrivalEventHandler DataArrival;

        /// <summary>
        /// バックグラウンド処理でエラーが発生したとき (たとえば、接続に失敗したとき、バックグラウンドで実行している送信や受信に失敗したときなど) に発生します。
        /// </summary>
        public event DMSWinsockControlEvents_ErrorEventHandler Error;

        /// <summary>
        /// 送信処理が完了したときに発生します。
        /// </summary>
        public event EventHandler SendComplete;

        /// <summary>
        /// データの送信中に発生します。
        /// </summary>
        public event DMSWinsockControlEvents_SendProgressEventHandler SendProgress;
    }
}
