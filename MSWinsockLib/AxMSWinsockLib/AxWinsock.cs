using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using MSWinsockLib;

namespace AxMSWinsockLib
{
    /// <summary>
    /// Winsock コントロールの RCW 互換コンポーネント
    /// </summary>
    public partial class AxWinsock : Winsock
    {
        /// <summary>
        /// AxWinsock のインスタンスを作成します。
        /// </summary>
        public AxWinsock() : base() { }

        /// <summary>
        /// コンテナを指定して AxWinsock のインスタンスを作成します。
        /// </summary>
        /// <param name="container">コンテナ</param>
        public AxWinsock(IContainer container) : base(container) { }

        /// <summary>
        /// ソケットが閉じられたときに発生します。
        /// </summary>
        public new event EventHandler CloseEvent;

        /// <summary>
        /// 接続したときに発生します。
        /// </summary>
        public new event EventHandler ConnectEvent;

        /// <summary>
        /// リモート マシンが接続を要求してきたときに発生します。 
        /// </summary>
        public new event DMSWinsockControlEvents_ConnectionRequestEventHandler ConnectionRequest;

        /// <summary>
        /// 新しいデータが送られてきたときに発生します。
        /// </summary>
        public new event DMSWinsockControlEvents_DataArrivalEventHandler DataArrival;

        /// <summary>
        /// バックグラウンド処理でエラーが発生したとき (たとえば、接続に失敗したとき、バックグラウンドで実行している送信や受信に失敗したときなど) に発生します。
        /// </summary>
        public new event DMSWinsockControlEvents_ErrorEventHandler Error;

        /// <summary>
        /// 送信処理が完了したときに発生します。
        /// </summary>
        public new event EventHandler SendComplete;

        /// <summary>
        /// データの送信中に発生します。
        /// </summary>
        public new event DMSWinsockControlEvents_SendProgressEventHandler SendProgress;

        /// <summary>
        /// CloseEvent イベントを発生します。
        /// </summary>
        /// <param name="e">イベントデータを含む EventArgs。</param>
        protected override void OnCloseEvent(EventArgs e)
        {
            base.OnCloseEvent(e);
            if (this.CloseEvent != null) this.CloseEvent(this, e);
        }

        /// <summary>
        /// ConnectEvent イベントを発生します。
        /// </summary>
        /// <param name="e">イベントデータを含む EventArgs。</param>
        protected override void OnConnectEvent(EventArgs e)
        {
            base.OnConnectEvent(e);
            if (this.ConnectEvent != null) this.ConnectEvent(this, e);
        }

        /// <summary>
        /// ConnectionRequest イベントを発生します。
        /// </summary>
        /// <param name="e">イベントデータを含む ConnectionRequestEvent。</param>
        protected override void OnConnectionRequest(ConnectionRequestEvent e)
        {
            base.OnConnectionRequest(e);
            if (this.ConnectionRequest != null) this.ConnectionRequest(this, e);
        }

        /// <summary>
        /// DataArrival イベントを発生します。
        /// </summary>
        /// <param name="e">イベントデータを含む DataArrivalEvent。</param>
        protected override void OnDataArrival(DataArrivalEvent e)
        {
            base.OnDataArrival(e);
            if (this.DataArrival != null) this.DataArrival(this, e);
        }

        /// <summary>
        /// Error イベントを発生します。
        /// </summary>
        /// <param name="e">イベントデータを含む ErrorEvent。</param>
        protected override void OnError(ErrorEvent e)
        {
            base.OnError(e);
            if (this.Error != null) this.Error(this, e);
        }

        /// <summary>
        /// SendComplete イベントを発生します。
        /// </summary>
        /// <param name="e">イベントデータを含む EventArgs。</param>
        protected override void OnSendComplete(EventArgs e)
        {
            base.OnSendComplete(e);
            if (this.SendComplete != null) this.SendComplete(this, e);
        }

        /// <summary>
        /// SendProgress イベントを発生します。
        /// </summary>
        /// <param name="e">イベントデータを含む SendProgressEvent。</param>
        protected override void OnSendProgress(SendProgressEvent e)
        {
            base.OnSendProgress(e);
            if (this.SendProgress != null) this.SendProgress(this, e);
        }
    }
}
