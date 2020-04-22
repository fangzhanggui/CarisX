using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.Common.Comm
{
    /// <summary>
    /// 通信管理
    /// </summary>
    /// <remarks>
    /// 送受信スレッドとのやり取り及びを行います。
    /// </remarks>
    public class CommManager
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// ホスト通信管理
        /// </summary>
        protected HostProtocolManager protocolManagerHost = null;

        /// <summary>
        /// スレーブ通信管理
        /// </summary>
        protected SlaveProtocolManager protocolManagerSlave = null;

        /// <summary>
        /// ラック搬送通信管理
        /// </summary>
        protected RackTransferProtocolManager protocolManagerRackTransfer = null;

        /// <summary>
        /// ホスト受信スレッド
        /// </summary>
        protected DataReceiveThread receiveThreadHost = null;

        /// <summary>
        /// スレーブ受信スレッド
        /// </summary>
        protected DataReceiveThread receiveThreadSlave = null;

        /// <summary>
        /// ラック搬送受信スレッド
        /// </summary>
        protected DataReceiveThread receiveThreadRackTransfer = null;

        /// <summary>
        /// ホスト送信スレッド
        /// </summary>
        protected DataSendThread sendThreadHost = null;

        /// <summary>
        /// スレーブ送信スレッド
        /// </summary>
        protected DataSendThread sendThreadSlave = new DataSendThread();

        /// <summary>
        /// ラック搬送送信スレッド
        /// </summary>
        protected DataSendThread sendThreadRackTransfer = new DataSendThread();

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommManager()
        {
            this.initialize();

            // 送受信スレッドに通信管理クラスを設定
            this.ReceiveThreadHost.SetProtocolManager( this.ProtocolManagerHost );
            this.SendThreadHost.SetProtocolManager( this.ProtocolManagerHost );
            this.ReceiveThreadSlave.SetProtocolManager(this.ProtocolManagerSlave);
            this.SendThreadSlave.SetProtocolManager(this.ProtocolManagerSlave);
            this.ReceiveThreadRackTransfer.SetProtocolManager(this.ProtocolManagerRackTransfer);
            this.SendThreadRackTransfer.SetProtocolManager(this.ProtocolManagerRackTransfer);

        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// スレーブ接続状態取得
        /// </summary>
        public ConnectionStatus SlaveCommStatus
        {
            get
            {
                return this.ProtocolManagerSlave.CommStatus;
            }
        }
        /// <summary>
        /// ホスト接続状態取得
        /// </summary>
        public ConnectionStatus HostCommmStatus
        {
            get
            {
                return this.ProtocolManagerHost.CommStatus;
            }
        }
        /// <summary>
        /// ラック搬送接続状態取得
        /// </summary>
        public ConnectionStatus RackTransferCommStatus
        {
            get
            {
                return this.ProtocolManagerRackTransfer.CommStatus;
            }
        }

        /// <summary>
        /// オンラインログの取得
        /// </summary>
        public List<String> OnlineLog
        {
            get
            {
                return this.protocolManagerHost.Log;
            }
        }

        /// <summary>
        /// 同步后的在线日志
        /// </summary>
        public List<String> SycOnlineLog
        {
            get
            {
                return this.protocolManagerHost.SycLog;
            }
        }
        

        /// <summary>
        /// ホスト通信管理の取得
        /// </summary>
        protected HostProtocolManager ProtocolManagerHost
        {
            get
            {
                return this.protocolManagerHost;
            }
        }

        /// <summary>
        /// スレーブ通信管理の取得
        /// </summary>
        protected SlaveProtocolManager ProtocolManagerSlave
        {
            get
            {
                return this.protocolManagerSlave;
            }
            set
            {
                this.protocolManagerSlave = value;
            }
        }

        /// <summary>
        /// ラック搬送通信管理の取得
        /// </summary>
        protected RackTransferProtocolManager ProtocolManagerRackTransfer
        {
            get
            {
                return this.protocolManagerRackTransfer;
            }
            set
            {
                this.protocolManagerRackTransfer = value;
            }
        }

        /// <summary>
        /// ホスト受信スレッドの取得
        /// </summary>
        protected DataReceiveThread ReceiveThreadHost
        {
            get
            {
                return this.receiveThreadHost;
            }
        }

        /// <summary>
        /// スレーブ受信スレッドの取得
        /// </summary>
        protected DataReceiveThread ReceiveThreadSlave
        {
            get
            {
                return this.receiveThreadSlave;
            }
            set
            {
                this.receiveThreadSlave = value;
            }
        }

        /// <summary>
        /// ラック搬送受信スレッドの取得
        /// </summary>
        protected DataReceiveThread ReceiveThreadRackTransfer
        {
            get
            {
                return this.receiveThreadRackTransfer;
            }
            set
            {
                this.receiveThreadRackTransfer = value;
            }
        }

        /// <summary>
        /// ホスト送信スレッドの取得
        /// </summary>
        protected DataSendThread SendThreadHost
        {
            get
            {
                return this.sendThreadHost;
            }
        }

        /// <summary>
        /// スレーブ送信スレッドの取得
        /// </summary>
        protected DataSendThread SendThreadSlave
        {
            get
            {
                return this.sendThreadSlave;
            }
            set
            {
                this.sendThreadSlave = value;
            }
        }

        /// <summary>
        /// ラック搬送送信スレッドの取得
        /// </summary>
        protected DataSendThread SendThreadRackTransfer
        {
            get
            {
                return this.sendThreadRackTransfer;
            }
            set
            {
                this.sendThreadRackTransfer = value;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// ホスト送信
        /// </summary>
        /// <remarks>
        /// ホスト送信スレッドのキューへコマンドを追加します。
        /// </remarks>
        /// <param name="command">送信コマンド</param>
        public void PushSendQueueHost( CommCommand command )
        {
            if ( command == null )
            {
                return;
            }
            this.SendThreadHost.PushSendQueue( command );
        }

        /// <summary>
        /// スレーブ送信
        /// </summary>
        /// <remarks>
        /// スレーブ送信スレッドのキューへコマンドを追加します。
        /// </remarks>
        /// <param name="command">送信コマンド</param>
		/// <param name="waitSend">送信完了待機フラグ ( trueの場合指定したコマンドがキューから無くなるまでブロックする )</param>
        public  void PushSendQueueSlave(CommCommand command, Boolean waitSend = false)
        {
            if (command == null)
            {
                return;
            }
            this.SendThreadSlave.PushSendQueue(command, waitSend);
        }

        /// <summary>
        /// ラック搬送送信
        /// </summary>
        /// <remarks>
        /// ラック搬送送信スレッドのキューへコマンドを追加します。
        /// </remarks>
        /// <param name="command">送信コマンド</param>
		/// <param name="waitSend">送信完了待機フラグ ( trueの場合指定したコマンドがキューから無くなるまでブロックする )</param>
        public void PushSendQueueRackTransfer(CommCommand command, Boolean waitSend = false)
        {
            if (command == null)
            {
                return;
            }
            this.SendThreadRackTransfer.PushSendQueue(command, waitSend);
        }

        /// <summary>
        /// ホスト接続
        /// </summary>
        /// <remarks>
        /// ホストへの接続、ホスト送/受信スレッドの起動を行います。
        /// </remarks>
        /// <param name="parameter">接続パラメータ</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean ConnectHost( Object parameter )
        {
            Boolean connected = this.ProtocolManagerHost.Connect( parameter );

            // 接続時、送受信スレッドの開始
            if ( connected )
            {
                this.SendThreadHost.Start();
                this.ReceiveThreadHost.Start();
            }

            return connected;
        }

        /// <summary>
        /// ホスト接続パラメータ設定
        /// </summary>
        /// <remarks>
        /// ホスト接続へのパラメータ設定を行います。
        /// </remarks>
        /// <param name="parameter"></param>
        public void SetHostParameter( Object parameter )
        {
            // パラメータ設定
            this.ProtocolManagerHost.SetParameter( parameter );
        }

        /// <summary>
        /// スレーブ接続
        /// </summary>
        /// <remarks>
        /// スレーブへの接続、スレーブ送/受信スレッドの起動を行います。
        /// </remarks>
        /// <param name="parameter">接続パラメータ</param>
        /// <param name="saveFilePath">通信ログファイルパス</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean ConnectSlave( Object parameter, String saveFilePath )
        {
            Boolean connected = this.ProtocolManagerSlave.ConnectSlave( parameter, saveFilePath );

            // 接続時、送受信スレッドの開始
            if ( connected )
            {
                this.SendThreadSlave.Start();
                this.ReceiveThreadSlave.Start();
            }

            return connected;
        }

        /// <summary>
        /// ラック搬送接続
        /// </summary>
        /// <remarks>
        /// ラック搬送への接続、ラック搬送の送信/受信スレッドの起動を行います。
        /// </remarks>
        /// <param name="parameter">接続パラメータ</param>
        /// <param name="saveFilePath">通信ログファイルパス</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean ConnectRackTransfer(Object parameter, String saveFilePath)
        {
            Boolean connected = this.ProtocolManagerRackTransfer.ConnectRackTransfer(parameter, saveFilePath);

            // 接続時、送受信スレッドの開始
            if (connected)
            {
                this.SendThreadRackTransfer.Start();
                this.ReceiveThreadRackTransfer.Start();
            }

            return connected;
        }

        /// <summary>
        /// ホスト切断
        /// </summary>
        /// <remarks>
        /// ホストの送信/受信スレッドの終了、ホストの切断を行います。
        /// </remarks>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean DisConnectHost()
        {
            Boolean closeSuccess = this.ProtocolManagerHost.DisConnect();

            //↓↓↓アプリケーション終了時の例外発生対応 2019/2/19↓↓↓
            // 送信データを強制停止
            if (this.SendThreadHost.GetSendNum() > 0)
            {
                // 削除要求
                this.SendThreadHost.ForceClearSendData();

                // 削除要求が実行されるまで程度待つ
                System.Threading.Thread.Sleep(3000);  // 接続処理のタイムアウトが2sなので、それ以上待つ
            }
            //↑↑↑アプリケーション終了時の例外発生対応 2019/2/19↑↑↑

            // 送受信スレッドの終了後、切断
            this.SendThreadHost.EndJoin();
            this.ReceiveThreadHost.EndJoin();

            return closeSuccess;
        }

        /// <summary>
        /// スレーブ切断
        /// </summary>
        /// <remarks>
        /// スレーブの送信/受信スレッドの終了、ホストの切断を行います。
        /// </remarks>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean DisConnectSlave()
        {
            //↓↓↓アプリケーション終了時の例外発生対応 2019/2/19↓↓↓
            // 送信データを強制停止
            if (this.SendThreadSlave.GetSendNum() > 0)
            {
                // 削除要求
                this.SendThreadSlave.ForceClearSendData();

                // 削除要求が実行されるまで程度待つ
                System.Threading.Thread.Sleep(3000);  // 接続処理のタイムアウトが2sなので、それ以上待つ
            }
            //↑↑↑アプリケーション終了時の例外発生対応 2019/2/19↑↑↑


            // 通信クラスの終了順番について
            // １．送信スレッドの終了
            // ２．CommSocketクラスのCloseSessionのコール
            // ３．受信スレッドの終了
            // ４．CommSocketクラスのCloseのコール

            // 送受信スレッドの終了
            this.SendThreadSlave.EndJoin();
            this.ProtocolManagerSlave.CloseSession();
            this.ReceiveThreadSlave.EndJoin();
            // ※DisConnect()の中のClose()で通信部をnullにするので、必ず送受信スレッド終了してからDisConnect()する事！
            Boolean closeSuccess = this.ProtocolManagerSlave.DisConnect();

            return closeSuccess;
        }

        /// <summary>
        /// ラック搬送切断
        /// </summary>
        /// <remarks>
        /// ラック搬送の送信/受信スレッドの終了、ホストの切断を行います。
        /// </remarks>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean DisConnectRackTransfer()
        {
            //↓↓↓アプリケーション終了時の例外発生対応 2019/2/19↓↓↓
            // 送信データを強制停止
            if (this.SendThreadRackTransfer.GetSendNum() > 0)
            {
                // 削除要求
                this.SendThreadRackTransfer.ForceClearSendData();

                // 削除要求が実行されるまで程度待つ
                System.Threading.Thread.Sleep(3000);  // 接続処理のタイムアウトが2sなので、それ以上待つ
            }
            //↑↑↑アプリケーション終了時の例外発生対応 2019/2/19↑↑↑


            // 通信クラスの終了順番について
            // １．送信スレッドの終了
            // ２．CommSocketクラスのCloseSessionのコール
            // ３．受信スレッドの終了
            // ４．CommSocketクラスのCloseのコール

            // 送受信スレッドの終了
            this.SendThreadRackTransfer.EndJoin();
            this.ProtocolManagerRackTransfer.CloseSession();
            this.ReceiveThreadRackTransfer.EndJoin();
            // ※DisConnect()の中のClose()で通信部をnullにするので、必ず送受信スレッド終了してからDisConnect()する事！
            Boolean closeSuccess = this.ProtocolManagerRackTransfer.DisConnect();

            return closeSuccess;
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// 初期化
        /// </summary>
        /// <remarks>
        /// メンバの初期化処理を行います。
        /// </remarks>
        protected virtual void initialize()
        {
            this.protocolManagerHost = new HostProtocolManager();
            this.protocolManagerSlave = new SlaveProtocolManager();
            this.protocolManagerRackTransfer = new RackTransferProtocolManager();
            this.receiveThreadHost = new DataReceiveThread();
            this.receiveThreadSlave = new DataReceiveThread();
            this.receiveThreadRackTransfer = new DataReceiveThread();
            this.sendThreadHost = new DataSendThread();
            this.sendThreadSlave = new DataSendThread();
            this.sendThreadRackTransfer = new DataSendThread();
        }

        #endregion


    }
}
