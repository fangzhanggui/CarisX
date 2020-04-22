using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;
using System.Threading;

namespace Oelco.Common.Comm
{
    /// <summary>
    /// データ送信スレッド
    /// </summary>
    /// <remarks>
    /// 通信データの送信機能を提供します。
    /// </remarks>
    public class CarisXDataSendThread : ThreadBase
    {
        #region [クラス変数定義]
        
        /// <summary>
        /// キュー追加通知イベント
        /// </summary>
        private ManualResetEvent queueAdded = new ManualResetEvent(false);

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// データ送信キュー
        /// </summary>
        private LockObject<Queue<CommCommand>> sendQueue = new LockObject<Queue<CommCommand>>();

        /// <summary>
        /// 通信マネージャ
        /// </summary>
        private CarisXCommProtocolManager protocolManager = null;

        //↓↓↓アプリケーション終了時の例外発生対応 2019/2/19↓↓↓
        /// <summary>
        /// 送信データを強制クリアするかどうか
        /// </summary>
        private bool isForceClearSendData = false;
        //↑↑↑アプリケーション終了時の例外発生対応 2019/2/19↑↑↑

        /// <summary>
        /// 装置コード
        /// </summary>
        private short machineCode = 0;

        /// <summary>
        /// 送信処理ロック
        /// </summary>
        static LockObject<int> sendLock = new LockObject<int>();

        #endregion

        #region [コンストラクタ]

        /// <summary>
        /// 
        /// </summary>
        /// <param name="machineCode"></param>
        public CarisXDataSendThread(int machineCode)
        {
            this.machineCode = (short)machineCode;
        }

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
        /// 送信キュー追加
        /// </summary>
        /// <remarks>
        /// 送信キューを追加します。
        /// </remarks>
        /// <remarks>送信スレッドの監視しているキューに対してコマンド追加を行います。</remarks>
		/// <param name="command">送信コマンド</param>
		/// <param name="waitSend">送信完了待機フラグ ( trueの場合指定したコマンドがキューから無くなるまでブロックする )</param>
        public void PushSendQueue( CommCommand command, Boolean waitSend = false )
        {

            // キューの追加
            this.sendQueue.Lock();
            this.sendQueue.Get.Instance.Enqueue( command );
            this.sendQueue.UnLock();
            
            // キュー追加イベントをセット
            this.queueAdded.Set();

			// 完了待機を行う
			if ( waitSend )
			{
				Boolean find = true;
				while ( find )
				{
					System.Threading.Thread.Sleep( 10 );
					this.sendQueue.Lock();
					find = this.sendQueue.Get.Instance.FirstOrDefault( ( queueItem ) => queueItem == command ) != null;
					this.sendQueue.UnLock();
				}
			}
        }

        //↓↓↓アプリケーション終了時の例外発生対応 2019/2/19↓↓↓
        /// <summary>
        /// 送信データの強制クリアを設定
        /// </summary>
        public void ForceClearSendData()
        {
            isForceClearSendData = true;
        }

        /// <summary>
        /// 送信データ数の取得
        /// </summary>
        /// <returns>送信データ数</returns>
        public int GetSendNum()
        {
            return this.sendQueue.Get.Instance.Count;
        }
        //↑↑↑アプリケーション終了時の例外発生対応 2019/2/19↑↑↑

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// スレッド関数
        /// </summary>
        /// <remarks>
        /// データの送信キュー監視と送信を行います。
        /// </remarks>
        protected override void threadFunction()
        {
            //↓↓↓アプリケーション終了時の例外発生対応 2019/2/19↓↓↓
            // インスタンスの再生成なく再度このスレッドが開始された場合、
            // 前回、強制停止を行っている場合、メンバーを初期化しておかないと問題になる
            isForceClearSendData = false;
            //↑↑↑アプリケーション終了時の例外発生対応 2019/2/19↑↑↑

            // スレッド優先順位高
            this.Thread.Priority = System.Threading.ThreadPriority.AboveNormal;

            WaitHandle[] waitHandles = new WaitHandle[] { this.queueAdded, this.EndEvent };
            Int32 waitIndex = WaitHandle.WaitAny( waitHandles );//, this.Interval );
            while ( waitIndex == 0 )
            {
                // キュー追加イベントをリセット
                this.queueAdded.Reset();


                // キュー追加イベント
                while ( this.sendQueue.Get.Instance.Count != 0 )
                {
                    //↓↓↓アプリケーション終了時の例外発生対応 2019/2/19↓↓↓
                    // 強制停止が有効な場合を確認
                    if (isForceClearSendData)
                    {
                        // キューをクリアする
                        this.sendQueue.Lock();
                        this.sendQueue.Get.Instance.Clear();
                        this.sendQueue.UnLock();

                        // 処理終了
                        break;
                    }
                    //↑↑↑アプリケーション終了時の例外発生対応 2019/2/19↑↑↑
                           
                    System.Threading.Thread.Sleep( 1 );
                    this.sendQueue.Lock();
                    CommCommand sendCmd = this.sendQueue.Get.Instance.Dequeue();
                    this.sendQueue.UnLock();

                    if ( !this.dataSend( sendCmd ) )
                    {
                        // 送信失敗
                        // TODO:送信失敗時 ログ出力
                    }

                    // TODO:取りあえず連続送信するとテスト用ダミーがうまく処理できないのでウェイト→テスト用Libから正規の通信Libになったので解除しても問題ない？
                    System.Threading.Thread.Sleep(10);

					if ( sendCmd.WaitEvent != null )
					{
						this.protocolManager.SleepEvent = sendCmd.WaitEvent;
						this.protocolManager.WaitEventEnd();
					}
                }

                waitIndex = WaitHandle.WaitAny( waitHandles );//, this.Interval );
            }
        }

        /// <summary>
        /// データ送信
        /// </summary>
        /// <remarks>
        /// プロトコルマネージャを利用して、コマンドデータの送信を行います。
        /// </remarks>
        /// <param name="command">コマンドデータ</param>
        /// <returns>True:成功 False:失敗</returns>
        protected Boolean dataSend( CommCommand command )
        {
            Boolean sended = false;

            if ( this.protocolManager != null )
            {
                /***********************
                 * 上位でコマンドデータをnewせずに使いまわしている場合、異なる送信スレッド間でもCommNoのメモリアドレスが同じになる。
                 * このため、装置番号を上書きしてしまうため、複数台にコマンドを送るはずが、特定の装置に2回送ってしまう不具合があった。
                 ***********************/

                // protocolManagerが送受信スレッドで同じインスタンスを使用しているため、送信処理の排他制御を追加
                sendLock.Lock();

                command.CommNo = machineCode;
                sended = this.protocolManager.Send(command);

                sendLock.UnLock();
            }

            return sended;
        }
        #endregion
    }
}
