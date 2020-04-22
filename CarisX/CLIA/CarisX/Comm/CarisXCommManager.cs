using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Comm;
using Oelco.Common.Utility;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Const;
using Oelco.CarisX.Log;
using Oelco.Common.Log;

namespace Oelco.CarisX.Comm
{
#if DEBUG_CARIS_OLD_SOCKET
    /// <summary>
    /// CarisX通信管理
    /// </summary>
    /// <remarks>
    /// CarisXで使用される通信の管理を行います。
    /// </remarks>
    public class CarisXCommManager : CommManager
    {
    #region [インスタンス変数定義]

        /// <summary>
        /// スレーブ通信管理（複数管理用）
        /// </summary>
        protected List<SlaveProtocolManager> protocolManagerSlaveList = null;

        /// <summary>
        /// ラック搬送通信管理
        /// </summary>
        protected List<RackTransferProtocolManager> protocolManagerRackTransferList = null;

        /// <summary>
        /// スレーブ受信スレッド
        /// </summary>
        protected List<DataReceiveThread> receiveThreadSlaveList = null;

        /// <summary>
        /// ラック搬送受信スレッド
        /// </summary>
        protected List<DataReceiveThread> receiveThreadRackTransferList = null;

        /// <summary>
        /// スレーブ送信スレッド
        /// </summary>
        protected List<DataSendThread> sendThreadSlaveList = new List<DataSendThread>();

        /// <summary>
        /// ラック搬送送信スレッド
        /// </summary>
        protected List<DataSendThread> sendThreadRackTransferList = new List<DataSendThread>();

        /// <summary>
        /// 選択されてるスレーブ
        /// </summary>
        protected Int32 selectedSlaveIndex = 0;

        /// <summary>
        /// 選択されてるラック搬送
        /// </summary>
        protected Int32 selectedRackTransferIndex = 0;

    #endregion

    #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CarisXCommManager()
        {
            // コマンドアナライザにCarisXコマンド解析を設定
            this.ProtocolManagerHost.CommandAnalyser = new CarisXCommandAnalyserHost();

            // 送受信スレッドに通信管理クラスを設定
            this.ReceiveThreadSlave[0].SetProtocolManager(this.ProtocolManagerSlave[0]);
            this.SendThreadSlave[0].SetProtocolManager(this.ProtocolManagerSlave[0]);
            this.ProtocolManagerSlave[0].CommandAnalyser = new CarisXCommandAnalyserSlave();
            this.ReceiveThreadRackTransfer[0].SetProtocolManager(this.ProtocolManagerRackTransfer[0]);
            this.SendThreadRackTransfer[0].SetProtocolManager(this.ProtocolManagerRackTransfer[0]);
            this.ProtocolManagerRackTransfer[0].CommandAnalyser = new CarisXCommandAnalyserRackTransfer();

        }

    #endregion

    #region [プロパティ]

        /// <summary>
        /// スレーブ通信管理の取得
        /// </summary>
        protected new List<SlaveProtocolManager> ProtocolManagerSlave
        {
            get
            {
                return this.protocolManagerSlaveList;
            }
        }

        /// <summary>
        /// ラック搬送通信管理の取得
        /// </summary>
        protected new List<RackTransferProtocolManager> ProtocolManagerRackTransfer
        {
            get
            {
                return this.protocolManagerRackTransferList;
            }
        }

        /// <summary>
        /// スレーブ受信スレッドの取得
        /// </summary>
        protected new List<DataReceiveThread> ReceiveThreadSlave
        {
            get
            {
                return this.receiveThreadSlaveList;
            }
        }

        /// <summary>
        /// ラック搬送受信スレッドの取得
        /// </summary>
        protected new List<DataReceiveThread> ReceiveThreadRackTransfer
        {
            get
            {
                return this.receiveThreadRackTransferList;
            }
        }

        /// <summary>
        /// スレーブ送信スレッドの取得
        /// </summary>
        protected new List<DataSendThread> SendThreadSlave
        {
            get
            {
                return this.sendThreadSlaveList;
            }
        }

        /// <summary>
        /// ラック搬送送信スレッドの取得
        /// </summary>
        protected new List<DataSendThread> SendThreadRackTransfer
        {
            get
            {
                return this.sendThreadRackTransferList;
            }
        }

        /// <summary>
        /// 選択中のスレーブ番号の取得
        /// </summary>
        public Int32 SelectedSlaveIndex
        {
            get
            {
                return this.selectedSlaveIndex;
            }
            set
            {
                this.selectedSlaveIndex = value;
            }
        }

        /// <summary>
        /// 選択中のラック搬送の機器番号の取得
        /// </summary>
        public Int32 SelectedRackTransferIndex
        {
            get
            {
                return this.selectedRackTransferIndex;
            }
            set
            {
                this.selectedRackTransferIndex = value;
            }
        }

    #endregion

    #region [privateメソッド]

        /// <summary>
        /// スレーブの追加
        /// </summary>
        /// <remarks>
        /// 接続するスレーブを増やします。
        /// </remarks>
        private void addSlave()
        {
            this.protocolManagerSlaveList.Add(new SlaveProtocolManager());
            this.receiveThreadSlaveList.Add(new DataReceiveThread());
            this.sendThreadSlaveList.Add(new DataSendThread());

            Int32 addedIndex = this.protocolManagerSlaveList.Count() - 1;

            this.ReceiveThreadSlave[addedIndex].SetProtocolManager(this.ProtocolManagerSlave[addedIndex]);
            this.SendThreadSlave[addedIndex].SetProtocolManager(this.ProtocolManagerSlave[addedIndex]);
            this.ProtocolManagerSlave[addedIndex].CommandAnalyser = new CarisXCommandAnalyserSlave();
        }

        /// <summary>
        /// ラック搬送の追加
        /// </summary>
        /// <remarks>
        /// 接続するラック搬送を増やします。
        /// </remarks>
        private void addRackTransfer()
        {
            this.protocolManagerRackTransferList.Add(new RackTransferProtocolManager());
            this.receiveThreadRackTransferList.Add(new DataReceiveThread());
            this.sendThreadRackTransferList.Add(new DataSendThread());

            Int32 addedIndex = this.protocolManagerRackTransferList.Count() - 1;

            this.ReceiveThreadRackTransfer[addedIndex].SetProtocolManager(this.ProtocolManagerRackTransfer[addedIndex]);
            this.SendThreadRackTransfer[addedIndex].SetProtocolManager(this.ProtocolManagerRackTransfer[addedIndex]);
            this.ProtocolManagerRackTransfer[addedIndex].CommandAnalyser = new CarisXCommandAnalyserRackTransfer();
        }

    #endregion


    #region [publicメソッド]

        /// <summary>
        /// スレーブ送信
        /// </summary>
        /// <remarks>
        /// スレーブ送信スレッドのキューへコマンドを追加します。
        /// </remarks>
        /// <param name="command">送信コマンド</param>
        /// <param name="waitSend">送信完了待機フラグ ( trueの場合指定したコマンドがキューから無くなるまでブロックする )</param>
        public new void PushSendQueueSlave(CommCommand command, Boolean waitSend = false)
        {
            if (command == null)
            {
                return;
            }
            this.PushSendQueueSlave((int)this.SelectedSlaveIndex, command, waitSend);
        }

        /// <summary>
        /// スレーブ送信
        /// </summary>
        /// <remarks>
        /// スレーブ送信スレッドのキューへコマンドを追加します。
        /// </remarks>
        /// <param name="command">送信コマンド</param>
        /// <param name="waitSend">送信完了待機フラグ ( trueの場合指定したコマンドがキューから無くなるまでブロックする )</param>
        public void PushSendQueueSlave(MachineCode machineCode, CommCommand command, Boolean waitSend = false)
        {
            if (command == null)
            {
                return;
            }
            this.PushSendQueueSlave(CarisXSubFunction.ModuleIDToModuleIndex(CarisXSubFunction.MachineCodeToRackModuleIndex(machineCode)), command, waitSend);
        }

        /// <summary>
        /// スレーブ送信
        /// </summary>
        /// <remarks>
        /// スレーブ送信スレッドのキューへコマンドを追加します。
        /// </remarks>
        /// <param name="moduleIndex">送信対象のモジュール</param>
        /// <param name="command">送信コマンド</param>
		/// <param name="waitSend">送信完了待機フラグ ( trueの場合指定したコマンドがキューから無くなるまでブロックする )</param>
        public void PushSendQueueSlave(int moduleIdx, CommCommand command, Boolean waitSend = false)
        {
            if (command == null)
            {
                return;
            }
            this.SendThreadSlave[moduleIdx].PushSendQueue(command, waitSend);
        }

        /// <summary>
        /// ラック搬送送信
        /// </summary>
        /// <remarks>
        /// ラック搬送送信スレッドのキューへコマンドを追加します。
        /// </remarks>
        /// <param name="command">送信コマンド</param>
		/// <param name="waitSend">送信完了待機フラグ ( trueの場合指定したコマンドがキューから無くなるまでブロックする )</param>
        public new void PushSendQueueRackTransfer(CommCommand command, Boolean waitSend = false)
        {
            if (command == null)
            {
                return;
            }
            this.SendThreadRackTransfer[this.SelectedRackTransferIndex].PushSendQueue(command, waitSend);
        }

        /// <summary>
        /// スレーブ接続
        /// </summary>
        /// <remarks>
        /// スレーブへの接続、スレーブ送/受信スレッドの起動を行います。
        /// </remarks>
        /// <param name="parameter">接続パラメータ（複数）</param>
        /// <param name="saveFilePath">通信ログファイルパス</param>
        /// <returns>True:成功 False:失敗</returns>
        public new Boolean ConnectSlave(Object parameter, String saveFilePath)
        {
            Boolean connected = true;
            List<SocketParameter> slaveCommSettings = parameter as List<SocketParameter>;

            for (Int32 i = 0; i < slaveCommSettings.Count(); i++)
            {
                // 装置コード生成
                short slaveId = Convert.ToInt16(MachineCode.Slave + slaveCommSettings[i].ModuleId);
                short userSoftId = Convert.ToInt16(MachineCode.PC);

                // スレーブ通信スレッド追加
                if (i > this.protocolManagerSlaveList.Count() - 1)
                {
                    this.addSlave();
                }

                // 通信ログファイルを別ファイルで管理
                String saveFilePathModuleNo = String.Format("{0}Module{1}", saveFilePath, (slaveCommSettings[i].ModuleId + 1)) + @"\";

                // 接続
                if (this.ProtocolManagerSlave[i].ConnectSlave(slaveCommSettings[i], saveFilePathModuleNo))
                {
                    // 接続時、送受信スレッドの開始

                    // 装置コード設定
                    this.ProtocolManagerSlave[i].CommObject.SetMachineCode(userSoftId, slaveId);

                    // 装置コードを通信番号として記録
                    this.ProtocolManagerSlave[i].CommNo = slaveId;

                    this.SendThreadSlave[i].Start();
                    this.ReceiveThreadSlave[i].Start();
                }
                else
                {
                    connected = false;
                }
            }

            return connected;
        }

        /// <summary>
        /// ラック搬送接続
        /// </summary>
        /// <remarks>
        /// ラック搬送への接続、ラック搬送送/受信スレッドの起動を行います。
        /// </remarks>
        /// <param name="parameter">接続パラメータ（複数）</param>
        /// <param name="saveFilePath">通信ログファイルパス</param>
        /// <returns>True:成功 False:失敗</returns>
        public new Boolean ConnectRackTransfer(Object parameter, String saveFilePath)
        {
            Boolean connected = true;
            List<SocketParameter> rackTransferCommSettings = parameter as List<SocketParameter>;

            for (Int32 i = 0; i < rackTransferCommSettings.Count(); i++)
            {
                // 装置コード生成
                short rackTransferId = Convert.ToInt16(MachineCode.RackTransfer + rackTransferCommSettings[i].ModuleId);
                short userSoftId = Convert.ToInt16(MachineCode.PC);

                // ラック搬送部通信スレッド追加
                // ※CarisXでは運用上は基本1台のみ
                if (i > this.protocolManagerRackTransferList.Count() - 1)
                {
                    this.addRackTransfer();
                }

                // 通信ログファイルを別ファイルで管理
                String saveFilePathModuleNo = String.Format("{0}RackTransfer", saveFilePath) + @"\";

                // 接続
                if (this.ProtocolManagerRackTransfer[i].ConnectRackTransfer(rackTransferCommSettings[i], saveFilePathModuleNo))
                {
                    // 接続時、送受信スレッドの開始

                    // 装置コード設定
                    this.ProtocolManagerRackTransfer[i].CommObject.SetMachineCode(userSoftId, rackTransferId);

                    // 装置コードを通信番号として記録
                    this.ProtocolManagerRackTransfer[i].CommNo = rackTransferId;

                    this.SendThreadRackTransfer[i].Start();
                    this.ReceiveThreadRackTransfer[i].Start();
                }
                else
                {
                    connected = false;
                }
            }

            return connected;
        }


        /// <summary>
        /// スレーブ・ラック搬送切断
        /// </summary>
        /// <remarks>
        /// スレーブとラック搬送の送信/受信スレッドの終了、ホストの切断を行います。
        /// </remarks>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean DisConnect()
        {
            Boolean closeSuccess = true;

            closeSuccess = DisConnectSlave();
            if (!closeSuccess)
            {
                return closeSuccess;
            }

            closeSuccess = DisConnectRackTransfer();
            if (!closeSuccess)
            {
                return closeSuccess;
            }

            return closeSuccess;
        }

        /// <summary>
        /// スレーブ切断
        /// </summary>
        /// <remarks>
        /// スレーブの送信/受信スレッドの終了、ホストの切断を行います。
        /// </remarks>
        /// <returns>True:成功 False:失敗</returns>
        public new Boolean DisConnectSlave()
        {
            Boolean closeSuccess = true;

            // 送受信スレッドの終了
            for (int i = this.SendThreadSlave.Count() - 1; i >= 0; i--)
            {
                base.SendThreadSlave = this.SendThreadSlave[i];
                base.ProtocolManagerSlave = this.ProtocolManagerSlave[i];
                base.ReceiveThreadSlave = this.ReceiveThreadSlave[i];
                if (!(base.DisConnectSlave()))
                {
                    closeSuccess = false;
                }
                this.SendThreadSlave[i] = base.SendThreadSlave;
                this.ProtocolManagerSlave[i] = base.ProtocolManagerSlave;
                this.ReceiveThreadSlave[i] = base.ReceiveThreadSlave;
                this.ReceiveThreadSlave.RemoveAt(i);
                this.ProtocolManagerSlave.RemoveAt(i);
                this.SendThreadSlave.RemoveAt(i);
            }

            return closeSuccess;
        }

        /// <summary>
        /// ラック搬送切断
        /// </summary>
        /// <remarks>
        /// ラック搬送の送信/受信スレッドの終了、ホストの切断を行います。
        /// </remarks>
        /// <returns>True:成功 False:失敗</returns>
        public new Boolean DisConnectRackTransfer()
        {
            Boolean closeSuccess = true;

            // 送受信スレッドの終了
            for (int i = this.SendThreadRackTransfer.Count() - 1; i >= 0; i--)
            {
                base.SendThreadRackTransfer = this.SendThreadRackTransfer[i];
                base.ProtocolManagerRackTransfer = this.ProtocolManagerRackTransfer[i];
                base.ReceiveThreadRackTransfer = this.ReceiveThreadRackTransfer[i];
                if (!(base.DisConnectRackTransfer()))
                {
                    closeSuccess = false;
                }
                this.SendThreadRackTransfer[i] = base.SendThreadRackTransfer;
                this.ProtocolManagerRackTransfer[i] = base.ProtocolManagerRackTransfer;
                this.ReceiveThreadRackTransfer[i] = base.ReceiveThreadRackTransfer;
                this.ReceiveThreadRackTransfer.RemoveAt(i);
                this.ProtocolManagerRackTransfer.RemoveAt(i);
                this.SendThreadRackTransfer.RemoveAt(i);
            }

            return closeSuccess;
        }

        /// <summary>
        /// コマンド送信結果コールバック登録
        /// </summary>
        /// <remarks>
        /// コマンド送信結果コールバックを登録します。
        /// </remarks>
        public void SetHostSendNotify(CommSendResult sendResult)
        {
            this.protocolManagerHost.SetCommSendResultCallBack(sendResult);
        }

        /// <summary>
        /// コマンド送信結果コールバック解除
        /// </summary>
        /// <remarks>
        /// コマンド送信結果コールバックを解除します。
        /// </remarks>
        public void ResetHostSendNotify()
        {
            this.protocolManagerHost.ClearCommSendResultCallBack();
        }

        /// <summary>
        /// 受信キュー移動
        /// </summary>
        /// <remarks>
        /// ホスト/スレーブの受信スレッドが保持する受信キュー内容を取り出し、
        /// メッセージマネージャへ移動します。
        /// </remarks>
        public void TransferReceiveQueueToMessageManager()
        {
            // MainFrameがこの関数を呼び、次にCarisXCommMesasgeManagerのイベント発生関数を呼び出す。

            CommCommand command = null;
            DateTime time = DateTime.MinValue;
            SortedList<DateTime, CommCommand> soretedUnite = new SortedList<DateTime, CommCommand>();

            // Hostからの受信キューとSlaveからの受信キュー、ラック搬送からの受信キューを時系列で統合。
            while (this.ReceiveThreadHost.PopReceiveQueue(out command, out time))
            {
                // 受信時間かぶると落ちる為、Ticksレベルで微調整を行う。
                while (soretedUnite.ContainsKey(time))
                {
                    time = new DateTime(time.Ticks + 1);
                }

                // コマンドに通信番号付与
                command.CommNo = Convert.ToInt16(MachineCode.Host);

                // メッセージキューに追加
                soretedUnite.Add(time, command);
            }
            for (int i = 0; i < this.ReceiveThreadSlave.Count(); i++)
            {
                while (this.ReceiveThreadSlave[i].PopReceiveQueue(out command, out time))
                {
                    // 受信時間かぶると落ちる為、Ticksレベルで微調整を行う。
                    while (soretedUnite.ContainsKey(time))
                    {
                        time = new DateTime(time.Ticks + 1);
                    }

                    // コマンドに通信番号付与
                    command.CommNo = this.ProtocolManagerSlave[i].CommNo;

                    // メッセージキューに追加
                    soretedUnite.Add(time, command);
                }
            }
            for (int i = 0; i < this.ReceiveThreadRackTransfer.Count(); i++)
            {
                while (this.ReceiveThreadRackTransfer[i].PopReceiveQueue(out command, out time))
                {
                    // 受信時間かぶると落ちる為、Ticksレベルで微調整を行う。
                    while (soretedUnite.ContainsKey(time))
                    {
                        time = new DateTime(time.Ticks + 1);
                    }

                    // コマンドに通信番号付与
                    command.CommNo = this.ProtocolManagerRackTransfer[i].CommNo;

                    // メッセージキューに追加
                    soretedUnite.Add(time, command);
                }
            }

            // メッセージマネージャに移し替える。
            foreach (CommCommand uniteCommand in soretedUnite.Values)
            {
                Singleton<CarisXCommMessageManager>.Instance.PushCommand(uniteCommand);
            }
        }

        /// <summary>
        /// 接続ステータス取得
        /// </summary>
        /// <param name="moduleIndex"></param>
        /// <returns></returns>
        public ConnectionStatus GetSlaveCommStatus(int moduleIndex)
        {
            ConnectionStatus result = ConnectionStatus.Offline;

            if (this.protocolManagerSlaveList.Count > moduleIndex)
            {
                result = this.protocolManagerSlaveList[moduleIndex].CommStatus;
            }

            return result;
        }

        /// <summary>
        /// ラック搬送接続ステータス取得
        /// </summary>
        /// <returns></returns>
        public ConnectionStatus GetRackTransferCommStatus()
        {
            ConnectionStatus result = ConnectionStatus.Offline;

            result = this.protocolManagerRackTransferList[0].CommStatus;

            return result;
        }

        //↓↓↓アプリケーション終了時の例外発生対応 2019/2/19↓↓↓
        /// <summary>
        /// 全ての未送信データのクリア
        /// </summary>
        /// <remarks>
        /// アプリケーション終了時用の処理です。
        /// 
        /// 接続エラーなどで送信エラーが大量に残っていると、スレッドの終了処理が
        /// タイムアウト時間内に終了せず、スレッド外部からAbortすることになるため、
        /// スレッド内でウェイトしている処理でThreadAbortException例外が発生します。
        /// 
        /// アプリケーション終了時にデータ再送と送信クラスの削除が同時に行われる
        /// こと自体、おかしな状況なので送信クラスの削除前に残っている送信データ
        /// をクリアするようにします。
        /// </remarks>
        public void ClearAllSendData()
        {
            // スレーブ
            for (int i = this.SendThreadSlave.Count() - 1; i >= 0; i--)
            {
                if (this.SendThreadSlave[i].GetSendNum() > 0)
                {
                    this.SendThreadSlave[i].ForceClearSendData();
                }
            }

            // ラック
            for (int i = this.SendThreadRackTransfer.Count() - 1; i >= 0; i--)
            {
                if (this.SendThreadRackTransfer[i].GetSendNum() > 0)
                {
                    this.SendThreadRackTransfer[i].ForceClearSendData();
                }
            }

            // ホスト
            if (this.SendThreadHost.GetSendNum() > 0)
            {
                // 削除要求
                this.SendThreadHost.ForceClearSendData();
            }
        }
        //↑↑↑アプリケーション終了時の例外発生対応 2019/2/19↑↑↑

    #endregion

    #region [protectedメソッド]

        /// <summary>
        /// 初期化
        /// </summary>
        /// <remarks>
        /// メンバの初期化処理を行います。
        /// </remarks>
        protected override void initialize()
        {
            base.initialize();

            // プロトコル管理リスト生成
            this.protocolManagerSlaveList = new List<SlaveProtocolManager>();
            this.protocolManagerRackTransferList = new List<RackTransferProtocolManager>();

            // 受信スレッドリスト生成
            this.receiveThreadSlaveList = new List<DataReceiveThread>();
            this.receiveThreadRackTransferList = new List<DataReceiveThread>();

            // 送信スレッドリスト生成
            this.sendThreadSlaveList = new List<DataSendThread>();
            this.sendThreadRackTransferList = new List<DataSendThread>();


            // baseで生成したプロトコル管理を追加
            this.protocolManagerSlaveList.Add(base.protocolManagerSlave);
            this.protocolManagerRackTransferList.Add(base.protocolManagerRackTransfer);

            // baseで生成した受信スレッドを追加
            this.receiveThreadSlaveList.Add(base.receiveThreadSlave);
            this.receiveThreadRackTransferList.Add(base.receiveThreadRackTransfer);

            // baseで生成した送信スレッドを追加
            this.sendThreadSlaveList.Add(base.sendThreadSlave);
            this.sendThreadRackTransferList.Add(base.sendThreadRackTransfer);
        }

        #endregion
#else
    /// <summary>
    /// CarisX通信管理
    /// </summary>
    /// <remarks>
    /// CarisXで使用される通信の管理を行います。
    /// </remarks>
    public class CarisXCommManager
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// ホスト通信管理
        /// </summary>
        protected HostProtocolManager protocolManagerHost = null;

        /// <summary>
        /// スレーブ通信管理
        /// </summary>
        protected CarisXCommProtocolManager protocolManagerSlaveRack = null;

        /// <summary>
        /// ホスト受信スレッド
        /// </summary>
        protected DataReceiveThread receiveThreadHost = null;

        /// <summary>
        /// スレーブ受信スレッド
        /// </summary>
        protected CarisXDataReceiveThread receiveThreadSlaveRack = null;

        /// <summary>
        /// ホスト送信スレッド
        /// </summary>
        protected DataSendThread sendThreadHost = null;

        /// <summary>
        /// スレーブ送信スレッド
        /// </summary>
        protected List<CarisXDataSendThread> sendThreadSlave = new List<CarisXDataSendThread>();

        /// <summary>
        /// ラック搬送送信スレッド
        /// </summary>
        protected CarisXDataSendThread sendThreadRackTransfer = null;

        /// <summary>
        /// 選択されてるスレーブ
        /// </summary>
        protected Int32 selectedSlaveIndex = 0;

        /// <summary>
        /// 選択されてるラック搬送
        /// </summary>
        protected Int32 selectedRackTransferIndex = 0;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CarisXCommManager()
        {
            this.initialize();

            // 送受信スレッドに通信管理クラスを設定
            this.ProtocolManagerHost.CommandAnalyser = new CarisXCommandAnalyserHost();
            this.ReceiveThreadHost.SetProtocolManager(this.ProtocolManagerHost);
            this.SendThreadHost.SetProtocolManager(this.ProtocolManagerHost);
            
            this.ProtocolManagerSlaveRack.CommandAnalyser = new CarisXCommandAnalyserSlaveRack();
            this.ReceiveThreadSlaveRack.SetProtocolManager(this.ProtocolManagerSlaveRack);
            this.SendThreadSlave[0].SetProtocolManager(this.ProtocolManagerSlaveRack);
            this.sendThreadRackTransfer.SetProtocolManager(this.ProtocolManagerSlaveRack);
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
                return this.ProtocolManagerSlaveRack.CommStatus;
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
        /// スレーブ接続状態取得
        /// </summary>
        public ConnectionStatus RackTransferCommStatus
        {
            get
            {
                return this.ProtocolManagerSlaveRack.CommStatus;
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
        protected CarisXCommProtocolManager ProtocolManagerSlaveRack
        {
            get
            {
                return this.protocolManagerSlaveRack;
            }
            set
            {
                this.protocolManagerSlaveRack = value;
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
        protected CarisXDataReceiveThread ReceiveThreadSlaveRack
        {
            get
            {
                return this.receiveThreadSlaveRack;
            }
            set
            {
                this.receiveThreadSlaveRack = value;
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
        protected List<CarisXDataSendThread> SendThreadSlave
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
        /// スレーブ送信スレッドの取得
        /// </summary>
        protected CarisXDataSendThread SendThreadRackTransfer
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

        /// <summary>
        /// 選択中のスレーブ番号の取得
        /// </summary>
        public Int32 SelectedSlaveIndex
        {
            get
            {
                return this.selectedSlaveIndex;
            }
            set
            {
                this.selectedSlaveIndex = value;
            }
        }

        /// <summary>
        /// 選択中のラック搬送の機器番号の取得
        /// </summary>
        public Int32 SelectedRackTransferIndex
        {
            get
            {
                return this.selectedRackTransferIndex;
            }
            set
            {
                this.selectedRackTransferIndex = value;
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
        public void PushSendQueueHost(CommCommand command)
        {
            if (command == null)
            {
                return;
            }
            this.SendThreadHost.PushSendQueue(command);
        }


        /// <summary>
        /// スレーブ送信
        /// </summary>
        /// <remarks>
        /// スレーブ送信スレッドのキューへコマンドを追加します。
        /// </remarks>
        /// <param name="command">送信コマンド</param>
        /// <param name="waitSend">送信完了待機フラグ ( trueの場合指定したコマンドがキューから無くなるまでブロックする )</param>
        public void PushSendQueueSlave(CommCommand command, Boolean waitSend = false)
        {
            this.PushSendQueueSlave((int)this.SelectedSlaveIndex, command, waitSend);
        }

        /// <summary>
        /// スレーブ送信
        /// </summary>
        /// <remarks>
        /// スレーブ送信スレッドのキューへコマンドを追加します。
        /// </remarks>
        /// <param name="command">送信コマンド</param>
        /// <param name="waitSend">送信完了待機フラグ ( trueの場合指定したコマンドがキューから無くなるまでブロックする )</param>
        public void PushSendQueueSlave(MachineCode machineCode, CommCommand command, Boolean waitSend = false)
        {
            if (command == null)
            {
                return;
            }
            this.PushSendQueueSlave(CarisXSubFunction.ModuleIDToModuleIndex(CarisXSubFunction.MachineCodeToRackModuleIndex(machineCode)) , command, waitSend);
        }
        
        /// <summary>
        /// スレーブ送信
        /// </summary>
        /// <remarks>
        /// スレーブ送信スレッドのキューへコマンドを追加します。
        /// </remarks>
        /// <param name="command">送信コマンド</param>
        /// <param name="waitSend">送信完了待機フラグ ( trueの場合指定したコマンドがキューから無くなるまでブロックする )</param>
        public void PushSendQueueSlave(int moduleIdx, CommCommand command, Boolean waitSend = false)
        {
            if (command == null)
            {
                return;
            }
            this.SendThreadSlave[moduleIdx].PushSendQueue(command, waitSend);
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
        public Boolean ConnectHost(Object parameter)
        {
            Boolean connected = this.ProtocolManagerHost.Connect(parameter);

            // 接続時、送受信スレッドの開始
            if (connected)
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
        public void SetHostParameter(Object parameter)
        {
            // パラメータ設定
            this.ProtocolManagerHost.SetParameter(parameter);
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
        public Boolean Connect(Object rackParam, Object slaveParam, String saveFilePath)
        {
            List<SocketParameter> setParam = new List<SocketParameter>();
            List<String> commLogFilePath = new List<String>();

            List<SocketParameter> rackCommSettings = rackParam as List<SocketParameter>;
            for (Int32 rackIdx = 0; rackIdx < rackCommSettings.Count(); rackIdx++)
            {
                setParam.Add(new SocketParameter());
                setParam[setParam.Count - 1].IpAddress = rackCommSettings[rackIdx].IpAddress;
                setParam[setParam.Count - 1].ModuleId = rackCommSettings[rackIdx].ModuleId + (int)MachineCode.RackTransfer;
                setParam[setParam.Count - 1].R_Port = rackCommSettings[rackIdx].R_Port;
                setParam[setParam.Count - 1].S_Port = rackCommSettings[rackIdx].S_Port;
                setParam[setParam.Count - 1].TimeOut = rackCommSettings[rackIdx].TimeOut;
                commLogFilePath.Add(String.Format("{0}\\RackTransfer\\", saveFilePath));
            }

            List<SocketParameter> slaveCommSettings = slaveParam as List<SocketParameter>;
            for (Int32 slaveIdx = 0; slaveIdx < slaveCommSettings.Count; slaveIdx++)
            {
                setParam.Add(new SocketParameter());
                setParam[setParam.Count - 1].IpAddress = slaveCommSettings[slaveIdx].IpAddress;
                setParam[setParam.Count - 1].ModuleId = slaveCommSettings[slaveIdx].ModuleId + (int)MachineCode.Slave;
                setParam[setParam.Count - 1].R_Port = slaveCommSettings[slaveIdx].R_Port;
                setParam[setParam.Count - 1].S_Port = slaveCommSettings[slaveIdx].S_Port;
                setParam[setParam.Count - 1].TimeOut = slaveCommSettings[slaveIdx].TimeOut;
                commLogFilePath.Add(String.Format("{0}\\Module{1}\\", saveFilePath, (slaveIdx + 1)));

                // スレーブが複数台構成の場合
                if (slaveIdx > 0)
                {
                    // 送信スレッド追加
                    this.SendThreadSlave.Add(new CarisXDataSendThread(setParam[setParam.Count - 1].ModuleId));
                    this.SendThreadSlave[this.SendThreadSlave.Count - 1].SetProtocolManager(this.ProtocolManagerSlaveRack);
                }
            }

            // 接続
            Boolean connected = this.ProtocolManagerSlaveRack.Connect(setParam, commLogFilePath);

            // 接続時、送受信スレッドの開始
            if (connected)
            {
                this.SendThreadRackTransfer.Start();
                foreach ( CarisXDataSendThread sendThread in this.SendThreadSlave)
                {
                    sendThread.Start();
                }
                this.ReceiveThreadSlaveRack.Start();
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
        public Boolean DisConnect()
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

            foreach (CarisXDataSendThread sendThread in this.SendThreadSlave)
            {
                if (sendThread.GetSendNum() > 0)
                {
                    // 削除要求
                    sendThread.ForceClearSendData();

                    // 削除要求が実行されるまで程度待つ
                    System.Threading.Thread.Sleep(3000);  // 接続処理のタイムアウトが2sなので、それ以上待つ
                }
            }
            //↑↑↑アプリケーション終了時の例外発生対応 2019/2/19↑↑↑


            // 通信クラスの終了順番について
            // １．送信スレッドの終了
            // ２．CommSocketクラスのCloseSessionのコール
            // ３．受信スレッドの終了
            // ４．CommSocketクラスのCloseのコール

            // 送受信スレッドの終了
            this.SendThreadRackTransfer.EndJoin();
            foreach (CarisXDataSendThread sendThread in this.SendThreadSlave)
            {
                sendThread.EndJoin();
            }
            this.ProtocolManagerSlaveRack.CloseSession();
            this.ReceiveThreadSlaveRack.EndJoin();
            // ※DisConnect()の中のClose()で通信部をnullにするので、必ず送受信スレッド終了してからDisConnect()する事！
            Boolean closeSuccess = this.ProtocolManagerSlaveRack.DisConnect();

            return closeSuccess;
        }

        //↓↓↓アプリケーション終了時の例外発生対応 2019/2/19↓↓↓
        /// <summary>
        /// 全ての未送信データのクリア
        /// </summary>
        /// <remarks>
        /// アプリケーション終了時用の処理です。
        /// 
        /// 接続エラーなどで送信エラーが大量に残っていると、スレッドの終了処理が
        /// タイムアウト時間内に終了せず、スレッド外部からAbortすることになるため、
        /// スレッド内でウェイトしている処理でThreadAbortException例外が発生します。
        /// 
        /// アプリケーション終了時にデータ再送と送信クラスの削除が同時に行われる
        /// こと自体、おかしな状況なので送信クラスの削除前に残っている送信データ
        /// をクリアするようにします。
        /// </remarks>
        public void ClearAllSendData()
        {
            // スレーブ
            for (int i = this.SendThreadSlave.Count() - 1; i >= 0; i--)
            {
                if (this.SendThreadSlave[i].GetSendNum() > 0)
                {
                    this.SendThreadSlave[i].ForceClearSendData();
                }
            }

            // ホスト
            if (this.SendThreadHost.GetSendNum() > 0)
            {
                // 削除要求
                this.SendThreadHost.ForceClearSendData();
            }
        }
        //↑↑↑アプリケーション終了時の例外発生対応 2019/2/19↑↑↑

        /// <summary>
        /// スレーブ接続ステータス取得
        /// </summary>
        /// <param name="moduleIndex"></param>
        /// <returns></returns>
        public ConnectionStatus GetSlaveCommStatus(int moduleIndex)
        {
            ConnectionStatus result = ConnectionStatus.Offline;

            int id = moduleIndex + (int)MachineCode.Slave;
            result = this.protocolManagerSlaveRack.GetCommStatus((short)id);

            return result;
        }

        /// <summary>
        /// ラック搬送接続ステータス取得
        /// </summary>
        /// <returns></returns>
        public ConnectionStatus GetRackTransferCommStatus()
        {
            ConnectionStatus result = ConnectionStatus.Offline;

            int id = (int)MachineCode.RackTransfer;
            result = this.protocolManagerSlaveRack.GetCommStatus((short)id);

            return result;
        }

        /// <summary>
        /// コマンド送信結果コールバック登録
        /// </summary>
        /// <remarks>
        /// コマンド送信結果コールバックを登録します。
        /// </remarks>
        public void SetHostSendNotify(CommSendResult sendResult)
        {
            this.protocolManagerHost.SetCommSendResultCallBack(sendResult);
        }

        /// <summary>
        /// コマンド送信結果コールバック解除
        /// </summary>
        /// <remarks>
        /// コマンド送信結果コールバックを解除します。
        /// </remarks>
        public void ResetHostSendNotify()
        {
            this.protocolManagerHost.ClearCommSendResultCallBack();
        }

        /// <summary>
        /// 受信キュー移動
        /// </summary>
        /// <remarks>
        /// ホスト/スレーブの受信スレッドが保持する受信キュー内容を取り出し、
        /// メッセージマネージャへ移動します。
        /// </remarks>
        public void TransferReceiveQueueToMessageManager()
        {
            // MainFrameがこの関数を呼び、次にCarisXCommMesasgeManagerのイベント発生関数を呼び出す。

            CommCommand command = null;
            DateTime time = DateTime.MinValue;
            SortedList<DateTime, CommCommand> soretedUnite = new SortedList<DateTime, CommCommand>();

            // Hostからの受信キューとSlaveからの受信キュー、ラック搬送からの受信キューを時系列で統合。
            while (this.ReceiveThreadHost.PopReceiveQueue(out command, out time))
            {
                // 受信時間かぶると落ちる為、Ticksレベルで微調整を行う。
                while (soretedUnite.ContainsKey(time))
                {
                    time = new DateTime(time.Ticks + 1);
                }

                // コマンドに通信番号付与
                command.CommNo = Convert.ToInt16(MachineCode.Host);

                // メッセージキューに追加
                soretedUnite.Add(time, command);
            }

            while (this.ReceiveThreadSlaveRack.PopReceiveQueue(out command, out time))
            {
                // 受信時間かぶると落ちる為、Ticksレベルで微調整を行う。
                while (soretedUnite.ContainsKey(time))
                {
                    time = new DateTime(time.Ticks + 1);
                }

                // メッセージキューに追加
                soretedUnite.Add(time, command);
            }

            // メッセージマネージャに移し替える。
            foreach (CommCommand uniteCommand in soretedUnite.Values)
            {
                Singleton<CarisXCommMessageManager>.Instance.PushCommand(uniteCommand);
            }
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
            this.protocolManagerSlaveRack = new CarisXCommProtocolManager();
            this.receiveThreadHost = new DataReceiveThread();
            this.receiveThreadSlaveRack = new CarisXDataReceiveThread();
            this.sendThreadHost = new DataSendThread();
            this.sendThreadRackTransfer = new CarisXDataSendThread((int)MachineCode.RackTransfer);
            this.sendThreadSlave = new List<CarisXDataSendThread>();
            this.sendThreadSlave.Add(new CarisXDataSendThread((int)MachineCode.Slave));
        }

        #endregion
#endif //[DEBUG_CARIS_OLD_SOCKET]
    }
}
