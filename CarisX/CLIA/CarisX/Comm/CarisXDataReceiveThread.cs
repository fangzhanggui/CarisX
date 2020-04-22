using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;
using System.Threading;

namespace Oelco.Common.Comm
{
    /// <summary>
    /// データ受信スレッド
    /// </summary>
    /// <remarks>
    /// 通信データの受信機能を提供します。
    /// </remarks>
    public class CarisXDataReceiveThread : ThreadBase
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// データ受信キュー
        /// </summary>
        private LockObject<Queue<Tuple<CommCommand, DateTime>>> receiveQueue = new LockObject<Queue<Tuple<CommCommand, DateTime>>>();

        /// <summary>
        /// 通信マネージャ
        /// </summary>
        private CarisXCommProtocolManager protocolManager = null;

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 通信マネージャ設定
        /// </summary>
        /// <remarks>
        /// 通信マネージャの設定を行います
        /// </remarks>
        public void SetProtocolManager( CarisXCommProtocolManager manager )
        {
            this.protocolManager = manager;
        }

        /// <summary>
        /// 受信キュー追加
        /// </summary>
        /// <remarks>受信スレッドの監視しているキューに対してコマンド追加を行います。</remarks>
        /// <param name="command">受信コマンド</param>
        /// <param name="receiveTime">受信日時</param>
        public Boolean PopReceiveQueue( out CommCommand command, out DateTime receiveTime )
        {
            Boolean result = false;
            command = null;
            receiveTime = DateTime.MinValue;

            // キューの追加
            this.receiveQueue.Lock();
            if ( this.receiveQueue.Get.Instance.Count != 0 )
            {
                var queue = this.receiveQueue.Get.Instance.Dequeue();
                command = queue.Item1;
                receiveTime = queue.Item2;
                result = true;
            }
            this.receiveQueue.UnLock();

            return result;
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// スレッド関数
        /// </summary>
        /// <remarks>
        /// 受信データの監視と受信キューへの追加を行います。
        /// </remarks>
        protected override void threadFunction()
        {
            // スレッド優先順位高
            this.Thread.Priority = ThreadPriority.AboveNormal;

            while ( !this.EndEvent.WaitOne( this.Interval ) )
            {
//Lock()をEnqueueのとこに持って行きました。
//                this.receiveQueue.Lock();

                CommCommand receiveCmd = null;

                while ( this.dataReceive( out receiveCmd ) )
                {
					// 現在のWaitEventEnd動作は、送信側で待機設定を行った時点で受信中であったコマンドが、
					// 送信側でのWaitEventEnd待機中に対して受信側WaitEventEnd待機までの間に受信されます。
					// その後、受信側で待機が行われ、待機が解除された場合は送信側WaitEventEnd中に受信したコマンドを含め、順次コマンドが処理されます。
					this.protocolManager.WaitEventEnd();

                    this.receiveQueue.Lock();
                    // 受信成功する限り受信を続ける
                    this.receiveQueue.Get.Instance.Enqueue( new Tuple<CommCommand,DateTime>( receiveCmd, DateTime.Now )  );
                    this.receiveQueue.UnLock();

                    // TODO:受信失敗時 ？
                }

//                this.receiveQueue.UnLock();
            }
        }

        /// <summary>
        /// データ受信
        /// </summary>
        /// <remarks>
        /// プロトコルマネージャを利用して、コマンドデータの受信を行います。
        /// </remarks>
        /// <param name="command">コマンドデータ</param>
        /// <returns>True:成功 False:失敗</returns>
        protected Boolean dataReceive( out CommCommand command )
        {
            Boolean received = false;
            command = null;

            if ( this.protocolManager != null )
            {
                received = this.protocolManager.Receive( out command );
            }

            return received;
        }
        #endregion
    }
}
