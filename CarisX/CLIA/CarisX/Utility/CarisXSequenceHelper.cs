
using Oelco.CarisX.Comm;
using Oelco.CarisX.Maintenance;
using Oelco.Common.Comm;
using Oelco.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Oelco.CarisX.Utility
{
    using Oelco.CarisX.Common;
    using Oelco.CarisX.Const;
    using Oelco.CarisX.DB;
    using Oelco.CarisX.GUI;
    using Oelco.CarisX.Log;
    using Oelco.CarisX.Parameter;
    using Oelco.CarisX.Properties;
    using Oelco.CarisX.Status;
    using Oelco.Common.Log;
    using Oelco.Common.Parameter;
    // シーケンスデリゲート別名 引数、戻り値にシーケンス名称。
    using SequenceDelegate = Func<String, Object[], String>;

    /// <summary>
    /// CarisXシーケンス補助管理クラス
    /// </summary>
    class CarisXSequenceHelperManager
    {
        /// <summary>
        /// ラック搬送用
        /// </summary>
        public CarisXSequenceHelper RackTransfer = null;
        /// <summary>
        /// スレーブ用
        /// </summary>
        public Dictionary<int, CarisXSequenceHelper> Slave = new Dictionary<int, CarisXSequenceHelper>();
        /// <summary>
        /// ホスト用
        /// </summary>
        public CarisXSequenceHelper Host = null;
        /// <summary>
        /// メンテナンス用
        /// </summary>
        public CarisXSequenceHelper Maintenance = null;


        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CarisXSequenceHelperManager()
        {
        }

        #endregion

        #region [public]

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="NumOfSlave">スレーブ接続数</param>
        public void Intialize(int numOfConnectSlave)
        {
            // ラック搬送用シーケンスヘルパー生成
            this.RackTransfer = new CarisXSequenceHelper();
            this.RackTransfer.CommNo = Convert.ToInt16(MachineCode.RackTransfer);

            // スレーブ用シーケンスヘルパー生成
            this.Slave.Clear();
            for (int index = 0; index < numOfConnectSlave; index++)
            {
                this.Slave.Add(index, new CarisXSequenceHelper());
                this.Slave[index].ModuleIdx = index;
                this.Slave[index].CommNo = Convert.ToInt16(MachineCode.Slave + index);
            }

            // ホスト用シーケンスヘルパー生成
            this.Host = new CarisXSequenceHelper();
            this.Host.CommNo = Convert.ToInt16(MachineCode.Host);

            // メンテナンス用シーケンスヘルパー生成
            this.Maintenance = new CarisXSequenceHelper();
        }

        #endregion

    }


    /// <summary>
    /// CarisXシーケンス補助
    /// </summary>
    /// <remarks>
    /// 応答を持つ動作に対して、
    /// 呼び出し側からの動作は1関数のコールで行えるよう定義を行います。
    /// CarisXで使用されるシーケンスを定義します。
    /// </remarks>
    class CarisXSequenceHelper : SequenceHelper
    {
        /// <summary>
        /// モジュール番号
        /// </summary>
        private int moduleIdx = 0;

        /// <summary>
        /// 通信番号
        /// </summary>
        private int commNo = 0;

        /// <summary>
        /// モーターエラー発生フラグ
        /// </summary>
        private Boolean isMotorError = false;

        /// <summary>
        /// モジュール番号
        /// </summary>
        public int ModuleIdx
        {
            get { return this.moduleIdx; }
            set { this.moduleIdx = value; }
        }

        /// <summary>
        /// 通信番号
        /// </summary>
        public int CommNo
        {
            get { return this.commNo; }
            set { this.commNo = value; }
        }

        /// <summary>
        /// 初期シーケンス完了済フラグ
        /// ※初期シーケンスが完了しているかどうか。
        /// 　一部のコマンドで初期シーケンス実行中はOnCommand内でレスポンスを返したくないため、初期シーケンス実行中の判断に使用
        /// 　各接続先と一対一で値を保持しているので、スレーブ用・ラック用に分割はしない
        /// </summary>
        public bool flgInitializeSequenceCompleted { get; set; } = false;

        /// <summary>
        /// 全ての中断イベント
        /// </summary>
        protected AutoResetEvent abortEventAll = new AutoResetEvent(false);

        /// <summary>
        /// 重複シーケンス終了待ち時間_初期シーケンス用(5秒)
        /// </summary>
        protected const Int32 INITIALIZE_SEQUENCE_WAIT_TIME = 1000 * 5;

        /// <summary>
        /// 初期シーケンス用スレッド
        /// </summary>
        private Thread InitializeSequenceThread = null;

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CarisXSequenceHelper()
        {
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// スレーブ送信（単体）
        /// </summary>
        /// <param name="command">コマンドデータ</param>
        /// <param name="waitSend">送信待機フラグ</param>
        private void pushSendQueueSingleSlave(CommCommand command, bool waitSend = false)
        {
            // インスタンス生成時に指定したモジュール番号に対して、スレーブ送信を行う
            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((int)this.ModuleIdx, command, waitSend);
        }

        /// <summary>
        /// 温度問合せシーケンス
        /// </summary>
        /// <remarks>
        /// 温度の問合せをスレーブに対して実施します。
        /// </remarks>
        public void AskTemperature()
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceDelegate sequence = new SequenceDelegate(this.askTemperature);
            this.startSequence(methodName, null, sequence);
        }

        /// <summary>
        /// 温度設定シーケンス
        /// </summary>
        /// <remarks>
        /// 温度の設定をスレーブに対して実施します。
        /// </remarks>
        public void SetTemperature(TemperatureParameter setTemperature)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceDelegate sequence = new SequenceDelegate(this.setTemperature);
            Object[] args = new Object[] { setTemperature };
            this.startSequence(methodName, args, sequence);
        }

        /// <summary>
        /// モジュールとの初期シーケンス呼び出し（モジュールが先に起動）
        /// </summary>
        /// <remarks>
        /// 初期シーケンスを開始します。
        /// ユーザーより先にモジュールが起動している場合に実行
        /// </remarks>
        /// <param name="args">パラメータ（不使用）</param>
        public void InitializeSequenceModule(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name + ModuleIdx.ToString();
            SequenceDelegate sequence = new SequenceDelegate(this.initializeSequenceStart);
            this.startSequence(methodName, args, sequence);
        }

        /// <summary>
        /// ラック搬送との初期シーケンス呼び出し（ラック搬送が先に起動）
        /// </summary>
        /// <remarks>
        /// 初期シーケンスを開始します。
        /// ユーザーより先にラック搬送が起動している場合に実行
        /// </remarks>
        /// <param name="args">パラメータ（不使用）</param>
        public void InitializeSequenceRackTransfer(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceDelegate sequence = new SequenceDelegate(this.initializeSequenceStart);
            this.startSequence(methodName, args, sequence);
        }

        /// <summary>
        /// 初期シーケンス終了呼出
        /// </summary>
        /// <remarks>
        /// 初期シーケンススレッドの終了呼出を行います
        /// </remarks>
        /// <param name="args"></param>
        public void InitializeSequenceAbort(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceDelegate sequence = new SequenceDelegate(this.initializeSequenceAbort);
            this.startSequence(methodName, args, sequence);
        }

        /// <summary>
        /// 洗浄液供給開始シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 洗浄液供給開始シーケンスを呼び出します。
        /// </remarks>
        /// <param name="args">パラメータ（不使用)</param>
        /// <returns>シーケンス制御データ</returns>
        public SequenceSyncObject StartReplaceWashsolutionTankSequence(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceSyncObject syncObject = new SequenceSyncObject();

            // 引数の領域を拡張し、末尾に同期操作オブジェクトを加える
            this.pushSequenceSyncObject(ref args, syncObject);
            SequenceDelegate sequence = new SequenceDelegate(startReplaceWashsolutionTankSequence);
            this.startSequence(methodName, args, sequence);

            return syncObject;
        }

        /// <summary>
        /// 洗浄液供給停止シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 洗浄液供給停止シーケンスを呼び出します。
        /// </remarks>
        /// <param name="args">パラメータ（SequenceCommData)</param>
        /// <returns>シーケンス制御データ</returns>
        public SequenceSyncObject StopReplaceWashsolutionTankSequence(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceSyncObject syncObject = new SequenceSyncObject();

            // 引数の領域を拡張し、末尾に同期操作オブジェクトを加える
            this.pushSequenceSyncObject(ref args, syncObject);
            SequenceDelegate sequence = new SequenceDelegate(stopReplaceWashsolutionTankSequence);
            this.startSequence(methodName, args, sequence);

            return syncObject;
        }

        /// <summary>
        /// 試薬準備開始シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 試薬準備をシーケンスを開始します。
        /// </remarks>
        /// <param name="args">パラメータ（不使用)</param>
        /// <returns>シーケンス制御データ</returns>
        public SequenceCommData PrepareReagentBottleSequence(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);
            this.pushSequenceCommData(ref args, sequenceData);
            SequenceDelegate sequence = new SequenceDelegate(this.prepareReagentBottleSequence);
            this.startSequence(methodName, args, sequence);
            return sequenceData;
        }

        /// <summary>
        /// 試薬準備完了シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 試薬交換完了シーケンスを開始します。
        /// </remarks>
        /// <param name="args">パラメータ（不使用）</param>
        /// <returns>シーケンス制御データ</returns>
        public SequenceCommData PrepareCompleteReagentBottleSequence(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);
            this.pushSequenceCommData(ref args, sequenceData);
            SequenceDelegate sequence = new SequenceDelegate(this.prepareCompleteReagentBottleSequence);
            this.startSequence(methodName, args, sequence);
            return sequenceData;
        }

        /// <summary>
        /// 汎用準備開始シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 各準備開始シーケンス呼び出しします
        /// </remarks>
        /// <param name="args"></param>
        /// <returns></returns>
        public SequenceCommData PrepareSequence(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);
            this.pushSequenceCommData(ref args, sequenceData);
            SequenceDelegate sequence = new SequenceDelegate(this.prepareSequence);
            this.startSequence(methodName, args, sequence);
            return sequenceData;
        }

        /// <summary>
        /// 汎用準備完了シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 各準備完了シーケンス呼び出しします
        /// </remarks>
        /// <param name="args"></param>
        /// <returns></returns>
        public SequenceCommData PrepareCompleteSequence(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);
            this.pushSequenceCommData(ref args, sequenceData);
            SequenceDelegate sequence = new SequenceDelegate(this.prepareCompleteSequence);
            this.startSequence(methodName, args, sequence);
            return sequenceData;
        }

        /// <summary>
        /// 希釈液準備開始シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 希釈液準備開始シーケンス呼び出しします
        /// </remarks>
        /// <param name="args"></param>
        /// <returns></returns>
        public SequenceCommData PrepareDiluentSequence(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);
            this.pushSequenceCommData(ref args, sequenceData);
            SequenceDelegate sequence = new SequenceDelegate(this.prepareDiluentSequence);
            this.startSequence(methodName, args, sequence);
            return sequenceData;
        }

        /// <summary>
        /// 希釈液準備完了シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 希釈液準備完了シーケンス呼び出しします
        /// </remarks>
        /// <param name="args"></param>
        /// <returns></returns>
        public SequenceCommData PrepareCompleteDiluentSequence(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);
            this.pushSequenceCommData(ref args, sequenceData);
            SequenceDelegate sequence = new SequenceDelegate(this.prepareCompleteDiluentSequence);
            this.startSequence(methodName, args, sequence);
            return sequenceData;
        }

        /// <summary>
        /// 準備中断シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 準備中断シーケンス呼び出しします
        /// </remarks>
        /// <param name="args"></param>
        /// <returns></returns>
        public void ExchangeCancelSequence(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceDelegate sequence = new SequenceDelegate(this.exchangeCancelSequence);
            this.startSequence(methodName, args, sequence);
        }

        /// <summary>
        /// 試薬残量変更シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 試薬残量変更シーケンス呼び出しします
        /// </remarks>
        /// <param name="args"></param>
        /// <returns></returns>
        public SequenceCommData ChangeReagentRemainSequence(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);
            this.pushSequenceCommData(ref args, sequenceData);
            SequenceDelegate sequence = new SequenceDelegate(this.changeReagentRemainSequence);
            this.startSequence(methodName, args, sequence);
            return sequenceData;
        }

        /// <summary>
        /// 汎用残量変更シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 汎用残量変更シーケンス呼び出しします
        /// 対象：プレトリガ、トリガ、希釈液、ケース(反応容器・サンプル分注チップ)
        /// </remarks>
        /// <param name="args"></param>
        /// <returns></returns>
        public SequenceCommData ChangeCommonRemainSequence(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);
            this.pushSequenceCommData(ref args, sequenceData);
            SequenceDelegate sequence = new SequenceDelegate(this.changeCommonRemainSequence);
            this.startSequence(methodName, args, sequence);
            return sequenceData;
        }

        /// <summary>
        /// （ラック）残量チェックシーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// ラックの残量チェックシーケンスを呼び出しします
        /// </remarks>
        /// <param name="args">引数（未使用）</param>
        /// <returns>シーケンス同期オブジェクト</returns>
        public SequenceSyncObject AskRackReagentRemain(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceSyncObject syncObject = new SequenceSyncObject();

            // 引数の領域を拡張し、末尾に同期操作オブジェクトを加える
            this.pushSequenceSyncObject(ref args, syncObject);
            SequenceDelegate sequence = new SequenceDelegate(askRackReagentRemain);
            this.startSequence(methodName, args, sequence);

            return syncObject;
        }

        /// <summary>
        /// （スレーブ）残量チェックシーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// スレーブの残量チェックシーケンス呼び出しします
        /// </remarks>
        /// <param name="args">引数（未使用）</param>
        /// <returns>シーケンス同期オブジェクト</returns>
        public SequenceSyncObject AskReagentRemain(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceSyncObject syncObject = new SequenceSyncObject();

            // 引数の領域を拡張し、末尾に同期操作オブジェクトを加える
            this.pushSequenceSyncObject(ref args, syncObject);
            SequenceDelegate sequence = new SequenceDelegate(askReagentRemain);
            this.startSequence(methodName, args, sequence);

            return syncObject;
        }

        /// <summary>
        /// 自動起動シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 自動起動シーケンス呼び出しします
        /// </remarks>
        /// <param name="args"></param>
        public void AutoStartup(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceDelegate sequence = new SequenceDelegate(this.autoStartUp);
            this.startSequence(methodName, args, sequence);
        }

        /// <summary>
        /// （ラック）システムパラメータシーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// ラックのシステムパラメータシーケンスを呼び出しします
        /// </remarks>
        /// <param name="args">引数（未使用）</param>
        /// <returns>シーケンス同期オブジェクト</returns>
        public SequenceSyncObject SendRackSystemParameter(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceSyncObject syncObject = new SequenceSyncObject();

            // 引数の領域を拡張し、末尾に同期操作オブジェクトを加える
            this.pushSequenceSyncObject(ref args, syncObject);
            SequenceDelegate sequence = new SequenceDelegate(sendRackSystemParamter);
            this.startSequence(methodName, args, sequence);

            return syncObject;
        }

        /// <summary>
        /// （スレーブ）システムパラメータシーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// スレーブのシステムパラメータシーケンスを呼び出しします
        /// </remarks>
        /// <param name="args">引数（未使用）</param>
        /// <returns>シーケンス同期オブジェクト</returns>
        public SequenceSyncObject SendSlaveSystemParameter(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceSyncObject syncObject = new SequenceSyncObject();

            // 引数の領域を拡張し、末尾に同期操作オブジェクトを加える
            this.pushSequenceSyncObject(ref args, syncObject);
            SequenceDelegate sequence = new SequenceDelegate(sendSlaveSystemParamter);
            this.startSequence(methodName, args, sequence);

            return syncObject;
        }

        /// <summary>
        /// （スレーブ）センサー使用有無送信シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// スレーブのセンサー使用有無送信シーケンスを呼び出しします
        /// </remarks>
        /// <param name="args"></param>
        /// <returns></returns>
        public SequenceSyncObject SendSlaveSensorParameterUseNoUse( params Object[] args )
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceSyncObject syncObject = new SequenceSyncObject();

            // 引数の領域を拡張し、末尾に同期操作オブジェクトを加える
            this.pushSequenceSyncObject(ref args, syncObject);
            SequenceDelegate sequence = new SequenceDelegate(sendSlaveSensorParameterUseNoUse);
            this.startSequence(methodName, args, sequence);

            return syncObject;
        }

        /// <summary>
        /// （ラック）分析開始シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// ラックの分析開始シーケンスを呼び出しします
        /// </remarks>
        /// <param name="args">引数（未使用）</param>
        /// <returns>シーケンス同期オブジェクト</returns>
        public SequenceSyncObject StartRackAssay(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceSyncObject syncObject = new SequenceSyncObject();

            // 引数の領域を拡張し、末尾に同期操作オブジェクトを加える
            this.pushSequenceSyncObject(ref args, syncObject);
            SequenceDelegate sequence = new SequenceDelegate(startRackAssay);
            this.startSequence(methodName, args, sequence);

            return syncObject;
        }

        /// <summary>
        /// （スレーブ）分析開始シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// スレーブの分析開始シーケンスを呼び出しします
        /// </remarks>
        /// <param name="args">引数（未使用）</param>
        /// <returns>シーケンス同期オブジェクト</returns>
        public SequenceSyncObject StartSlaveAssay(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceSyncObject syncObject = new SequenceSyncObject();

            // 引数の領域を拡張し、末尾に同期操作オブジェクトを加える
            this.pushSequenceSyncObject(ref args, syncObject);
            SequenceDelegate sequence = new SequenceDelegate(startSlaveAssay);
            this.startSequence(methodName, args, sequence);

            return syncObject;
        }

        /// <summary>
        /// 消耗品問い合わせシーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 消耗品問い合わせシーケンス呼び出しします
        /// </remarks>
        /// <param name="args"></param>
        /// <returns></returns>
        public SequenceSyncObject AskSupplieParameter(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceSyncObject syncObject = new SequenceSyncObject();


            // 引数の領域を拡張し、末尾に同期操作オブジェクトを加える
            this.pushSequenceSyncObject(ref args, syncObject);
            SequenceDelegate sequence = new SequenceDelegate(askSupplieParameter);
            this.startSequence(methodName, args, sequence);


            return syncObject;
        }

        /// <summary>
        /// 消耗品設定シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 消耗品設定シーケンス呼び出しします
        /// </remarks>
        /// <param name="args"></param>
        /// <returns></returns>
        public SequenceCommData SetStateConsumables(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);
            this.pushSequenceCommData(ref args, sequenceData);
            SequenceDelegate sequence = new SequenceDelegate(this.setStateConsumables);
            this.startSequence(methodName, args, sequence);
            return sequenceData;
        }

        /// <summary>
        /// プローブ交換シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// プローブ交換シーケンス呼び出しします
        /// </remarks>
        /// <param name="args"></param>
        /// <returns></returns>
        public SequenceSyncObject SetStartProbeSeq(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceSyncObject syncObject = new SequenceSyncObject();
            this.pushSequenceSyncObject(ref args, syncObject);
            SequenceDelegate sequence = new SequenceDelegate(this.setStartProbeSeq);
            this.startSequence(methodName, args, sequence);
            return syncObject;
        }


        /// <summary>
        /// リンス処理シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// リンス処理シーケンス呼び出しします
        /// </remarks>
        /// <param name="args"></param>
        /// <returns></returns>
        public SequenceSyncObject RinsingSequence(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceSyncObject syncObject = new SequenceSyncObject();
            this.pushSequenceSyncObject(ref args, syncObject);
            SequenceDelegate sequence = new SequenceDelegate(this.rinsingSequence);
            this.startSequence(methodName, args, sequence);
            return syncObject;
        }

        /// <summary>
        /// シリンジエージング
        /// </summary>
        /// <remarks>
        /// シリンジエージングします
        /// </remarks>
        /// <param name="args"></param>
        /// <returns></returns>
        public SequenceSyncObject SyringeAgingSequence(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceSyncObject syncObject = new SequenceSyncObject();

            // 引数の領域を拡張し、末尾に同期操作オブジェクトを加える
            this.pushSequenceSyncObject(ref args, syncObject);
            SequenceDelegate sequence = new SequenceDelegate(syringeAgingSequence);
            this.startSequence(methodName, args, sequence);

            return syncObject;
        }


        /// <summary>
        /// 試薬ロット切替わりシーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 試薬ロット切替わりシーケンス呼び出しします
        /// </remarks>
        /// <param name="args">試薬ロット切替わりコマンド</param>
        /// <returns></returns>
        public void ChangeReagentLot(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            //SequenceSyncObject syncObject = new SequenceSyncObject();


            // 引数の領域を拡張し、末尾に同期操作オブジェクトを加える
            //this.pushSequenceSyncObject( ref args, syncObject );
            SequenceDelegate sequence = new SequenceDelegate(this.changeReagentLot);
            this.startSequence(methodName, args, sequence);


            //return syncObject;
        }
        /// <summary>
        /// 終了シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 終了シーケンスの呼び出しを行います。
        /// </remarks>
        /// <param name="args">引数（未使用）</param>
        /// <returns>シーケンス同期オブジェクト</returns>
        public SequenceSyncObject EndSequence(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceSyncObject syncObject = new SequenceSyncObject();

            // 引数の領域を拡張し、末尾に同期操作オブジェクトを加える
            this.pushSequenceSyncObject(ref args, syncObject);
            SequenceDelegate sequence = new SequenceDelegate(endSequence);
            this.startSequence(methodName, args, sequence);

            return syncObject;
        }
        /// <summary>
        /// ラック搬送用終了シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// ラック搬送用終了シーケンスの呼び出しを行います。
        /// </remarks>
        /// <param name="args">引数（未使用）</param>
        /// <returns>シーケンス同期オブジェクト</returns>
        public SequenceSyncObject EndSequenceRackTransfer(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceSyncObject syncObject = new SequenceSyncObject();

            // 引数の領域を拡張し、末尾に同期操作オブジェクトを加える
            this.pushSequenceSyncObject(ref args, syncObject);
            SequenceDelegate sequence = new SequenceDelegate(endSequenceRackTransfer);
            this.startSequence(methodName, args, sequence);

            return syncObject;
        }

        //ここで保冷庫移動コマンドを送るようにする。
        /// <summary>
        /// 保冷庫移動コマンド呼び出し
        /// </summary>
        /// <remarks>
        /// 保冷庫移動コマンドの呼び出しを行います。
        /// </remarks>
        /// <param name="args">引数（未使用）</param>
        /// <returns>シーケンス同期オブジェクト</returns>
        public SequenceSyncObject ReagentCoolerMoveSequence(DlgTurnTable TurnTableInstance, Int32 dispMode, Int32 PortNumber)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceSyncObject syncObject = new SequenceSyncObject();
            Object[] args = new Object[] { PortNumber, TurnTableInstance, dispMode };
            // 引数の領域を拡張し、末尾に同期操作オブジェクトを加える
            this.pushSequenceSyncObject(ref args, syncObject);
            SequenceDelegate sequence = new SequenceDelegate(ReagentCoolerMoveSequence);
            this.startSequence(methodName, args, sequence);

            return syncObject;
        }

        /// <summary>
        /// メンテナンス用パラメータ設定コマンド送信シーケンス
        /// </summary>
        /// <remarks>
        /// メンテナンス用パラメータ設定コマンド送信シーケンスを開始します。
        /// </remarks>
        /// <param name="args">パラメータ：0=List<CarisXCommCommand>、1=ModuleKind</param>
        public void MaintenanceSetParamSequence(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceDelegate sequence = new SequenceDelegate(this.maintenanceSetParamSequence);
            this.startSequence(methodName, args, sequence);
        }

        /// <summary>
        /// メンテナンス用PID設定コマンド送信シーケンス
        /// </summary>
        /// <remarks>
        /// メンテナンス用PID設定コマンド送信シーケンスを開始します。
        /// </remarks>
        /// <param name="args">パラメータ：0=List<CarisXCommCommand>、1=ModuleKind</param>
        public void MaintenanceSetPIDSequence(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceDelegate sequence = new SequenceDelegate(this.maintenanceSetPIDSequence);
            this.startSequence(methodName, args, sequence);
        }

        /// <summary>
        /// メンテナンス用Abortコマンド送信シーケンス
        /// </summary>
        /// <remarks>
        /// メンテナンス用Abortコマンド送信シーケンスを開始します。
        /// </remarks>
        /// <param name="args">パラメータ：0=List<CarisXCommCommand>、1=ModuleKind</param>
        public void MaintenanceAbortSequence(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceDelegate sequence = new SequenceDelegate(this.maintenanceAbortSequence);
            this.startSequence(methodName, args, sequence);
        }

        /// <summary>
        /// メンテナンス用Abort設定コマンド送信
        /// </summary>
        /// <remarks>
        /// メンテナンス用Abort設定コマンド送信します
        /// </remarks>
        /// <param name="sequenceName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected String maintenanceAbortSequence(String sequenceName, params Object[] param)
        {

            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 待機時間超過
                return sequenceName;
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            ModuleKind moduleKind = (ModuleKind)param[0];
            CarisXCommCommand cmd = param[1] as CarisXCommCommand;
            CommandKind waitcmd = (CommandKind)param[2];

            // 待機するレスポンスコマンドの指定
            sequenceData.WaitCommandKind = waitcmd;

            //コマンドを送信
            if (moduleKind == ModuleKind.RackTransfer)
            {
                //パラメータ送信コマンド
                Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd);
            }
            else
            {
                //パラメータ送信コマンド
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd);          //SelectedSlaveIndexに従って処理する
            }

            sequenceData.Wait(Timeout.Infinite);
            if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
            {
                // エラーログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, String.Format(Resources_Maintenance.STRING_MAINTENANCE_MESSAGE_000));
                //レスポンスの受信がタイムアウトした場合はRcvCommandをNullにしておく
                sequenceData.RcvCommand = null;
            }

            // 完了通知
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.MaintenanceAbort, sequenceData.RcvCommand);

            //free the Event
            sequenceData.Dispose();
            sequenceData = null;

            return sequenceName;
        }

        /// <summary>
        /// メンテナンス用パラメータ設定コマンド送信
        /// </summary>
        /// <remarks>
        /// メンテナンス用パラメータ設定コマンド送信します
        /// </remarks>
        /// <param name="sequenceName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected String maintenanceSetParamSequence(String sequenceName, params Object[] param)
        {

            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 待機時間超過
                return sequenceName;
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            List<CarisXCommCommand> cmdList = param[0] as List<CarisXCommCommand>;
            ModuleKind moduleKind = (ModuleKind)param[1];

            Boolean ResponseRuslt = true;

            foreach (var cmd in cmdList)
            {
                // 何かしらのコマンド応答待ち
                sequenceData.WaitCommandKind = CommandKind.Unknown;

                //コマンドを送信
                if (moduleKind == ModuleKind.RackTransfer)
                {
                    //パラメータ送信コマンド
                    Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd);
                }
                else
                {
                    //パラメータ送信コマンド
                    Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd);          //SelectedSlaveIndexに従って処理する
                }

                sequenceData.Wait(PARAM_RESP_WAIT_TIME);
                if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                {
                    // エラーログ出力
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                        , CarisXLogInfoBaseExtention.Empty, String.Format(Resources_Maintenance.STRING_MAINTENANCE_MESSAGE_000));
                    ResponseRuslt = false;
                    break;
                }
            }

            // 完了通知
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ParameterResponse, ResponseRuslt);

            //free the Event
            sequenceData.Dispose();
            sequenceData = null;

            return sequenceName;
        }

        /// <summary>
        /// PID設定コマンド送信
        /// </summary>
        /// <remarks>
        /// PID設定コマンド送信します
        /// </remarks>
        /// <param name="sequenceName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected String maintenanceSetPIDSequence(String sequenceName, params Object[] param)
        {

            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 待機時間超過
                return sequenceName;
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            List<CarisXCommCommand> cmdList = param[0] as List<CarisXCommCommand>;

            foreach (var cmd in cmdList)
            {
                // 何かしらのコマンド応答待ち
                sequenceData.WaitCommandKind = CommandKind.Unknown;

                //パラメータ送信コマンド
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd);          //SelectedSlaveIndexに従って処理する

                sequenceData.Wait(PARAM_RESP_WAIT_TIME);
                if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                {
                    // エラーログ出力
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                        , CarisXLogInfoBaseExtention.Empty, String.Format(Resources_Maintenance.STRING_MAINTENANCE_MESSAGE_000));
                    break;
                }
            }

            // 完了通知
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.TempPIDResponse, sequenceData.RcvCommand);

            //free the Event
            sequenceData.Dispose();
            sequenceData = null;

            return sequenceName;
        }

        /// <summary>
        /// 試薬ロット番号変更
        /// </summary>
        /// <remarks>
        /// 試薬ロット番号変更します
        /// </remarks>
        /// <param name="sequenceName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected String changeReagentLot(String sequenceName, params Object[] param)
        {
            SlaveCommCommand_0512 command0512 = param.First() as SlaveCommCommand_0512;

            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 待機時間超過
                return sequenceName;
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            int moduleId = CarisXSubFunction.ModuleIndexToModuleId( (ModuleIndex)this.ModuleIdx );

            // 検量線情報リスト作成
            List<KeyValuePair<String, List<CalibrationCurveData>>> selectedCurve = new List<KeyValuePair<string, List<CalibrationCurveData>>>();

            // 切替わった試薬ロットの検量線が存在するかどうか確認する。
            MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(command0512.ProtocolNo);
            if (protocol != null)
            {
                // 検量線データリスト取得
                var calibCurveDataList = Singleton<CalibrationCurveDB>.Instance.GetData(protocol.ProtocolIndex, command0512.ReagentLotNumber, moduleId);

                selectedCurve = (from v in calibCurveDataList
                                 select v).ToList();
            }

            SlaveCommCommand_1512 command1512 = new SlaveCommCommand_1512();

            // 確認結果を応答コマンドで返す
            // 検量線有無設定
            command1512.UsableCallibration = Convert.ToByte(selectedCurve.Count() != 0);
            this.pushSendQueueSingleSlave(command1512);

            // スレーブからDPRに存在しないプロトコル番号が送信されている場合
            if (protocol == null)
            {
                //スレーブから送信されたコマンドに、無効なプロトコル番号が含まれています。
                Singleton<CarisXLogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("[changeReagentLot] to commands sent from the slave, invalid protocol number is included.。ProtocolNo = {0}", command0512.ProtocolNo));

                SlaveCommCommand_0479 command0479 = new SlaveCommCommand_0479();
                this.pushSendQueueSingleSlave(command0479);

                sequenceData.Dispose();
                sequenceData = null;
                return sequenceName;
            }

            // 切替わった試薬ロットがキャリブレーションの分析で使用されている場合エラー発生させる。
            var useReagentLots = HybridDataMediator.GetAllRegisteredProtocolReagentLot(protocol, CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)ModuleIdx));

            // キャリブレータ分析に登録されている項目が該当し、切り替わった試薬のロットの異なる場合、エラーを発生させる。
            if (useReagentLots.Item2.Count != 0)
            {
                Boolean existCalibRegist = useReagentLots.Item2.Exists((value) => value == command0512.ReagentLotNumber);
                if (!existCalibRegist)
                {
                    // 変更された試薬ロットはキャリブレータ登録に存在しないのでエラー発生させる。
                    var errorCode = CarisXDPRErrorCode.CalibReagentLotChangeError;
                    errorCode.Arg = protocol.ProtocolNo; // 分析項目番号を可変にする。
                    CarisXSubFunction.WriteDPRErrorHist(errorCode);
                }
            }

            // 検量線が存在しない場合
            if (selectedCurve.Count() == 0)
            {
                // キャリブレーション確認コマンドが来るので、受けたタイミングでそのまま続行するかキャリブレーションを行うか確認画面を表示する
                sequenceData.WaitCommandKind = CommandKind.Command0513;
                sequenceData.Wait(RESP_WAIT_TIME);
                if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Success)
                {
                    // 確認画面表示内容生成
                    // 使用分析項目全種取得
                    // 試薬ロット切り替わり時確認画面を表示・再検査リストへ登録
                    // 再検査リストへの登録は別途イベント通知でメインスレッドから、分析中画面への更新・InProcessデータへの更新（スレーブから測定データコマンドがすぐ来るのであれば問題無い、確認する。）再検査への登録。
                    // TODO:2013.01リリース分では対応見送り
                    Boolean needSendStopCancel = true;

                    // 検体・精度管理検体から指定したロットの登録がある場合、
                    // 検量線が無いデータの為設定に従い確認ダイアログを表示する。
                    var useLotsList = useReagentLots.Item1.Union(useReagentLots.Item3).ToList();

                    // ロットを指定している場合、検量線が無かったロットであればキャリブレーション確認。
                    // ロットを指定していない場合があれば、キャリブレーション確認。
                    Boolean needCalibrationConfirm = useLotsList.Exists((usingLot) => usingLot == command0512.ReagentLotNumber)
                        || useLotsList.Exists((usingLot) => usingLot == String.Empty);
                    if (needCalibrationConfirm)
                    {
                        String protocolNames = protocol.ProtocolName;// String.Join( CarisX.Properties.Resources.STRING_COMMON_006, allUseProtocols.Select( ( v ) => v.ProtocolName ) );

                        // システム構成の試薬ロット切替わり時の処理が続行でなければ、キャリブレーション測定確認画面を表示する。
                        if (!Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ProcessAtReagentLotChange.ReagLotChangeProc)
                        {
                            // 停止要因解除コマンド送信する
                            needSendStopCancel = true;
                        }
                        else
                        {
                            Int32 moduleIndex = CarisXSubFunction.ModuleIDToModuleIndex(CarisXSubFunction.MachineCodeToRackModuleIndex((MachineCode)command0512.CommNo));
                            // 確認画面を表示する
                            using (var dlg = new DlgCalibrationConfirm(protocolNames, command0512.ReagentLotNumber, moduleIndex))
                            {
                                var result = dlg.ShowDialog();
                                if (result == System.Windows.Forms.DialogResult.OK)
                                {
                                    // キャリブレーション有りで続行の場合、ラック排出コマンドを送信する（WS問合せが行われた検体であれば測定データコマンド（リマーク付き）を受信する）
                                    RackTransferCommCommand_0069 command0069 = new RackTransferCommCommand_0069();
                                    Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(command0069);
                                    sequenceData.WaitCommandKind = CommandKind.RackTransferCommand1069;
                                    sequenceData.Wait(RESP_WAIT_TIME);

                                    // 停止要因解除コマンドを続けて送信
                                    needSendStopCancel = true;
                                }
                                else
                                {
                                    // キャリブレーション無しで続行の場合、停止要因解除コマンド
                                    needSendStopCancel = true;
                                }
                            }
                        }
                    }

                    // サンプル停止要因解除コマンド送信
                    if (needSendStopCancel)
                    {
                        SlaveCommCommand_0479 command0479 = new SlaveCommCommand_0479();
                        this.pushSendQueueSingleSlave(command0479);

                    }
                }
            }
            //free the Event
            sequenceData.Dispose();
            sequenceData = null;

            return sequenceName;
        }

        /// <summary>
        /// 寿命部品使用回数設定問い合わせ
        /// </summary>
        /// <remarks>
        /// 寿命部品使用回数設定問い合わせします
        /// </remarks>
        /// <param name="sequenceName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected String askSupplieParameter(String sequenceName, params Object[] param)
        {
            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                return sequenceName;
            }

            using (SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo))
            {

                SlaveCommCommand_0444 cmd0444 = new SlaveCommCommand_0444();
                CommandDataMediator.SetSupplieParamToCmd(this.ModuleIdx, CommandControlParameter.Ask, Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param, ref cmd0444);
                // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
                sequenceData.WaitCommandKind = CommandKind.Command1444;

                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("【Life use parts count setting query timeout verification】 command before sending"));

                // コマンド送信
                this.pushSendQueueSingleSlave(cmd0444);

                // スレーブから応答を待機
                sequenceData.Wait(RESP_WAIT_TIME);

                if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Success)
                {
                    SlaveCommCommand_1444 cmd1444 = sequenceData.RcvCommand as SlaveCommCommand_1444;
                    SupplieParameter supplie = Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param;
                    CommandDataMediator.SetSupplieCmdToParam(cmd1444, ref supplie);

                    Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Save();


                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SupplieResponced);
                }

            }

            //syncObject.SetEnd();
            //            SlaveCommCommand_1444 

            return sequenceName;
        }

        /// <summary>
        /// 消耗品設定
        /// </summary>
        /// <remarks>
        /// 消耗品設定します
        /// </remarks>
        /// <param name="sequenceName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected String setStateConsumables(String sequenceName, params Object[] param)
        {
            SequenceCommData sequenceData = (SequenceCommData)param.Last();

            SlaveCommCommand_0444 cmd0444 = new SlaveCommCommand_0444();
            CommandDataMediator.SetSupplieParamToCmd(this.ModuleIdx, CommandControlParameter.Set, Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param, ref cmd0444);
            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.Command1444;
            // コマンド送信
            this.pushSendQueueSingleSlave(cmd0444);
            // スレーブから応答を待機
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
            {
                System.Diagnostics.Debug.WriteLine("消耗品setting time-out。");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, "消耗品setting time-out。");
            }
            // 通知
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.StateConsumables, sequenceData);
            sequenceData.Dispose();
            sequenceData = null;
            return sequenceName;
        }

        /// <summary>
        /// 調整　スタート実行
        /// </summary>
        /// <remarks>
        /// 調整　スタート実行します
        /// </remarks>
        /// <param name="seq"></param>
        /// <param name="args"></param>
        public void ExecuteSeqence(SequenceDelegate seq, Object[] args = null)
        {
            // メンテナンスで使用
            String methodName = seq.Method.ToString();
            this.startSequence(methodName, args, seq);
        }

        /// <summary>
        /// probe交換シーケンス開始
        /// </summary>
        /// <remarks>
        /// probe交換シーケンス開始します。
        /// </remarks>
        /// <param name="sequenceName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected String setStartProbeSeq(String sequenceName, params Object[] param)
        {
            SequenceSyncObject syncObject = (SequenceSyncObject)param.Last();
            syncObject.Status = SequenceSyncObject.SequenceStatus.Wait;

            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 待機時間超過
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                syncObject.SetEnd();
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            SlaveCommCommand_0497 cmd0497 = new SlaveCommCommand_0497();
            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.Command1497;

            if ((Int32)param[0] == 0)
                cmd0497.Status = SlaveCommCommand_0497.probeUnit.R1Unit;
            else
                cmd0497.Status = SlaveCommCommand_0497.probeUnit.R2Unit;

            // コマンド送信
            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd0497);
            // スレーブから応答を待機
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
            {
                System.Diagnostics.Debug.WriteLine("試薬プローブ位置調整タイムアウト。");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, "There was no response to replacing reagent probe command.");
            }
            else
            {
                syncObject.Status = SequenceSyncObject.SequenceStatus.Success;
                syncObject.SequenceResultData = sequenceData.RcvCommand;
            }

            // 通知
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ProbeChangeResponse, sequenceData);
            syncObject.SetEnd();
            return sequenceName;
        }



        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// 終了シーケンス
        /// </summary>
        /// <remarks>
        /// 終了シーケンス動作を行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称（内部利用）</param>
        /// <param name="param">パラメータ</param>
        /// <returns>シーケンス名称（内部利用）</returns>
        protected String endSequence(String sequenceName, Object[] param)
        {
            SequenceSyncObject syncObject = (SequenceSyncObject)param.Last();
            syncObject.Status = SequenceSyncObject.SequenceStatus.Wait;

            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 待機時間超過
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                syncObject.SetEnd();
                return sequenceName;
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            // 装置との接続を確認する
            if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(this.ModuleIdx) == ConnectionStatus.Online)
            {
                // リンス処理
                SlaveCommCommand_0410 cmd0410 = new SlaveCommCommand_0410();

                // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
                sequenceData.WaitCommandKind = CommandKind.Command1410;

                // コマンド送信
                this.pushSendQueueSingleSlave(cmd0410);
                SlaveCommCommand_1410 cmd1410;

                // スレーブから応答を待機
                sequenceData.Wait(Timeout.Infinite);//miyoushi is Request for rinse timeout.

                if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Success)
                {
                    cmd1410 = ((SlaveCommCommand_1410)sequenceData.RcvCommand);
                    if (cmd1410.Result == 0)
                    {
                        // シャットダウンコマンドを投げて待機
                        SlaveCommCommand_0403 cmd0403 = new SlaveCommCommand_0403();
                        sequenceData.WaitCommandKind = CommandKind.Command1403;
                        this.pushSendQueueSingleSlave(cmd0403);
                        sequenceData.Wait(Timeout.Infinite);
                        if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Success)
                            syncObject.Status = SequenceSyncObject.SequenceStatus.Success;
                        else
                        {
                            syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                                , CarisXLogInfoBaseExtention.Empty, "シャットダウン処理タイムアウト。");
                        }
                    }
                    else
                    {
                        syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                    }
                }
                else
                {
                    syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                        , CarisXLogInfoBaseExtention.Empty, "リンス処理タイムアウト。");
                }
            }
            else
            {
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                // 失敗(未接続 )
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, "スレーブ未接続。");
            }

            // イベント通知
            SequenceHelperMessage sequenceHelperMessage = new SequenceHelperMessage();
            sequenceHelperMessage.ModuleIndex = this.ModuleIdx;
            sequenceHelperMessage.MessageParameter = (syncObject.Status == SequenceSyncObject.SequenceStatus.Success);

            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ShutDownExecuteSlave, sequenceHelperMessage);

            sequenceData.Dispose();
            sequenceData = null;

            syncObject.SetEnd();
            return sequenceName;
        }

        /// <summary>
        /// ラック搬送用終了シーケンス
        /// </summary>
        /// <remarks>
        /// ラック搬送用終了シーケンス動作を行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称（内部利用）</param>
        /// <param name="param">パラメータ</param>
        /// <returns>シーケンス名称（内部利用）</returns>
        protected String endSequenceRackTransfer(String sequenceName, Object[] param)
        {
            SequenceSyncObject syncObject = (SequenceSyncObject)param.Last();
            syncObject.Status = SequenceSyncObject.SequenceStatus.Wait;

            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 待機時間超過
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                syncObject.SetEnd();
                return sequenceName;
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            // 装置との接続を確認する
            if (Singleton<CarisXCommManager>.Instance.GetRackTransferCommStatus() == ConnectionStatus.Online)
            {
                // シャットダウンコマンドを投げて待機
                RackTransferCommCommand_0003 cmd0003 = new RackTransferCommCommand_0003();
                sequenceData.WaitCommandKind = CommandKind.RackTransferCommand1003;
                Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0003);
                sequenceData.Wait(Timeout.Infinite);
                if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Success)
                    syncObject.Status = SequenceSyncObject.SequenceStatus.Success;
                else
                    syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
            }
            else
            {
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                // 失敗(未接続 )
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, "ラック搬送未接続。");
            }

            // イベント通知
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ShutDownExecuteRack, syncObject.Status == SequenceSyncObject.SequenceStatus.Success);

            sequenceData.Dispose();
            sequenceData = null;

            syncObject.SetEnd();
            return sequenceName;
        }

        /// <summary>
        /// （ラック）残量チェックシーケンス
        /// </summary>
        /// <remarks>
        /// ラックの残量チェックシーケンス動作を行います。
        /// このシーケンスは完了時、SequencerSyncObjectのSequenceResultDataに残量チェックコマンド応答をセットします。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称（内部利用）</param>
        /// <param name="param">パラメータ</param>
        /// <returns>シーケンス名称（内部利用）</returns>
        protected String askRackReagentRemain(String sequenceName, Object[] param)
        {
            SequenceSyncObject syncObject = (SequenceSyncObject)param.Last();
            syncObject.Status = SequenceSyncObject.SequenceStatus.Wait;

            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 待機時間超過
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                syncObject.SetEnd();
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            // 残量チェックコマンド
            RackTransferCommCommand_0014 cmd0014 = new RackTransferCommCommand_0014();
            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.RackTransferCommand1014;
            // コマンド送信
            Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0014);
            // ラックから応答を待機
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Success)
            {
                syncObject.Status = SequenceSyncObject.SequenceStatus.Success;
                syncObject.SequenceResultData = sequenceData.RcvCommand;
            }
            else
            {
                // 応答なし
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
            }
            //Free the event
            sequenceData.Dispose();
            sequenceData = null;

            syncObject.SetEnd();
            return sequenceName;
        }

        /// <summary>
        /// 残量確認シーケンス
        /// </summary>
        /// <remarks>
        /// 残量確認シーケンス動作を行います。
        /// このシーケンスは完了時、SequencerSyncObjectのSequenceResultDataに残量チェックコマンド応答をセットします。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称（内部利用）</param>
        /// <param name="param">パラメータ</param>
        /// <returns>シーケンス名称（内部利用）</returns>
        protected String askReagentRemain(String sequenceName, Object[] param)
        {
            SequenceSyncObject syncObject = (SequenceSyncObject)param.Last();
            syncObject.Status = SequenceSyncObject.SequenceStatus.Wait;

            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 待機時間超過
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                syncObject.SetEnd();
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);


            // 残量チェックコマンド
            SlaveCommCommand_0414 cmd0414 = new SlaveCommCommand_0414();
            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.Command1414;
            // コマンド送信
            this.pushSendQueueSingleSlave(cmd0414);
            // スレーブから応答を待機
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Success)
            {
                syncObject.Status = SequenceSyncObject.SequenceStatus.Success;
                syncObject.SequenceResultData = sequenceData.RcvCommand;
            }
            else
            {
                // 応答なし
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
            }
            //Free the event
            sequenceData.Dispose();
            sequenceData = null;

            syncObject.SetEnd();
            return sequenceName;
        }

        /// <summary>
        /// （ラック）システムパラメータ送信シーケンス
        /// </summary>
        /// <remarks>
        /// ラックのシステムパラメータシーケンス動作を行います。
        /// このシーケンスは完了時、SequencerSyncObjectのSequenceResultDataにシステムパラメータコマンド応答をセットします。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称（内部利用）</param>
        /// <param name="param">パラメータ</param>
        /// <returns>シーケンス名称（内部利用）</returns>
        protected String sendRackSystemParamter(String sequenceName, Object[] param)
        {
            SequenceSyncObject syncObject = (SequenceSyncObject)param.Last();
            syncObject.Status = SequenceSyncObject.SequenceStatus.Wait;

            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 待機時間超過
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                syncObject.SetEnd();
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            // システムパラメータコマンド
            RackTransferCommCommand_0004 cmd0004 = new RackTransferCommCommand_0004();
            cmd0004.SetSystemParameter(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param);

            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.RackTransferCommand1004;

            // コマンド送信
            Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0004);
            // ラックから応答を待機
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Success)
            {
                syncObject.Status = SequenceSyncObject.SequenceStatus.Success;
                syncObject.SequenceResultData = sequenceData.RcvCommand;
            }
            else
            {
                // 応答なし
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
            }
            //Free the event
            sequenceData.Dispose();
            sequenceData = null;

            syncObject.SetEnd();
            return sequenceName;
        }

        /// <summary>
        /// （スレーブ）システムパラメータ送信シーケンス
        /// </summary>
        /// <remarks>
        /// スレーブのシステムパラメータシーケンス動作を行います。
        /// このシーケンスは完了時、SequencerSyncObjectのSequenceResultDataにシステムパラメータコマンド応答をセットします。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称（内部利用）</param>
        /// <param name="param">パラメータ</param>
        /// <returns>シーケンス名称（内部利用）</returns>
        protected String sendSlaveSystemParamter(String sequenceName, Object[] param)
        {
            SequenceSyncObject syncObject = (SequenceSyncObject)param.Last();
            syncObject.Status = SequenceSyncObject.SequenceStatus.Wait;

            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 待機時間超過
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                syncObject.SetEnd();
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            // システムパラメータコマンド
            SlaveCommCommand_0404 cmd0404 = new SlaveCommCommand_0404();
            cmd0404.SetSystemParameter(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param, this.ModuleIdx);

            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.Command1404;
            // コマンド送信
            this.pushSendQueueSingleSlave(cmd0404);
            // ラックから応答を待機
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Success)
            {
                syncObject.Status = SequenceSyncObject.SequenceStatus.Success;
                syncObject.SequenceResultData = sequenceData.RcvCommand;
            }
            else
            {
                // 応答なし
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
            }
            //Free the event
            sequenceData.Dispose();
            sequenceData = null;

            syncObject.SetEnd();
            return sequenceName;
        }

        /// <summary>
        /// （スレーブ）センサー使用有無送信シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// スレーブのセンサー使用有無送信シーケンス動作を行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称（内部利用）</param>
        /// <param name="param">パラメータ</param>
        /// <returns>シーケンス名称（内部利用）</returns>
        protected String sendSlaveSensorParameterUseNoUse( String sequenceName, Object[] param )
        {
            CarisXSensorParameter.SensorParameterUseNoUseSlave sensorParam = (CarisXSensorParameter.SensorParameterUseNoUseSlave)param.First();
            SequenceSyncObject syncObject = (SequenceSyncObject)param.Last();
            syncObject.Status = SequenceSyncObject.SequenceStatus.Wait;
            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 待機時間超過
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                syncObject.SetEnd();
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.Command1441;

            // センサー無効コマンド送信
            this.pushSendQueueSingleSlave(sensorParam);

            // スレーブからの応答を待機
            sequenceData.Wait(RESP_WAIT_TIME);

            // 送信成功した場合
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Success)
            {
                // ファイル保存パラメータにセンサー使用有無情報を渡す
                Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance.Param.SlaveList[this.ModuleIdx].sensorParameterUseNoUse = sensorParam;

                // ファイル保存パラメータを保存
                Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance.SaveRaw();
            }
            // 失敗した場合
            else
            {
                // モジュールIDの取得
                RackModuleIndex moduleIndex = (RackModuleIndex)CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)this.ModuleIdx);

                // モジュールIDからモジュール名を取得
                String moduleName = CarisXSubFunction.ModuleIdToModuleName((int)moduleIndex);

                // エラー履歴に登録
                CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.CommunicationErrorBetweenMasterAndSlave, 0, moduleName);
            }

            //Free the event
            sequenceData.Dispose();
            sequenceData = null;

            syncObject.SetEnd();

            return sequenceName;
        }

        /// <summary>
        /// （ラック）分析開始コマンド送信シーケンス
        /// </summary>
        /// <remarks>
        /// ラックの分析開始コマンド送信シーケンス動作を行います。
        /// このシーケンスは完了時、SequencerSyncObjectのSequenceResultDataにシステムパラメータコマンド応答をセットします。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称（内部利用）</param>
        /// <param name="param">パラメータ</param>
        /// <returns>シーケンス名称（内部利用）</returns>
        protected String startRackAssay(String sequenceName, Object[] param)
        {
            SequenceSyncObject syncObject = (SequenceSyncObject)param.Last();
            syncObject.Status = SequenceSyncObject.SequenceStatus.Wait;

            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 待機時間超過
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                syncObject.SetEnd();
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            // 分析開始コマンド
            RackTransferCommCommand_0011 cmd0011 = new RackTransferCommCommand_0011();

            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.RackTransferCommand1011;

            // コマンド送信
            Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0011);
            // ラックから応答を待機
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Success)
            {
                syncObject.Status = SequenceSyncObject.SequenceStatus.Success;
                syncObject.SequenceResultData = sequenceData.RcvCommand;
            }
            else
            {
                // 応答なし
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
            }
            //Free the event
            sequenceData.Dispose();
            sequenceData = null;

            syncObject.SetEnd();
            return sequenceName;
        }

        /// <summary>
        /// （スレーブ）システムパラメータ送信シーケンス
        /// </summary>
        /// <remarks>
        /// スレーブのシステムパラメータシーケンス動作を行います。
        /// このシーケンスは完了時、SequencerSyncObjectのSequenceResultDataにシステムパラメータコマンド応答をセットします。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称（内部利用）</param>
        /// <param name="param">パラメータ</param>
        /// <returns>シーケンス名称（内部利用）</returns>
        protected String startSlaveAssay(String sequenceName, Object[] param)
        {
            SequenceSyncObject syncObject = (SequenceSyncObject)param.Last();
            syncObject.Status = SequenceSyncObject.SequenceStatus.Wait;

            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 待機時間超過
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                syncObject.SetEnd();
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            // 分析開始コマンド
            SlaveCommCommand_0411 cmd0411 = new SlaveCommCommand_0411();

            //リンス実行の有無をシスパラから取得
            cmd0411.RinseExecution = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RinseExecutionBeforeAssay.Enable;

            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.Command1411;

            // コマンド送信
            this.pushSendQueueSingleSlave(cmd0411);
            
            // スレーブからの応答を待機
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Success)
            {
                syncObject.Status = SequenceSyncObject.SequenceStatus.Success;
                syncObject.SequenceResultData = sequenceData.RcvCommand;
            }
            else
            {
                // 応答なし
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
            }
            //Free the event
            sequenceData.Dispose();
            sequenceData = null;

            syncObject.SetEnd();
            return sequenceName;
        }

        /// <summary>
        /// コマンド応答待ち時間(1000秒)
        /// </summary>
        protected const Int32 RESP_WAIT_TIME = 1000000; // コマンド応答待ち時間(1000秒)

        /// <summary>
        /// パラメータコマンド応答待ち時間(10秒)
        /// </summary>
        protected const Int32 PARAM_RESP_WAIT_TIME = 10000;

        /// <summary>
        /// ソフトウェア識別コマンド応答待ち時間(5秒)
        /// </summary>
        protected const Int32 SOFTWARE_IDENTIFICATION_RESP_WAIT_TIME = 5000;

        /// <summary>
        /// 接続確認コマンド応答待ち時間(5秒)
        /// </summary>
        protected const Int32 CHECK_CONNECTION_RESP_WAIT_TIME = 5000;

        /// <summary>
        /// 自動起動
        /// </summary>
        /// <remarks>
        /// 自動起動します
        /// </remarks>
        /// <param name="sequenceName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected String autoStartUp(String sequenceName, Object[] param)
        {
            Boolean seqStatus = false;

            // TODO:END処理する場合のリンス液残量確認 -->「END処理する場合」のif文追加する
            using (SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo))
            {
                // 正常終了フラグON
                if (!Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SystemCondition)
                {
                    Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SystemCondition = true;
                    Singleton<ParameterFilePreserve<AppSettings>>.Instance.Save();
                }

                //リンスコマンド（清洗灌注命令）
                SlaveCommCommand_0410 cmd0410 = new SlaveCommCommand_0410();
                // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
                sequenceData.WaitCommandKind = CommandKind.Command1410;
                // コマンド送信
                this.pushSendQueueSingleSlave(cmd0410);
                // スレーブから応答を待機
                sequenceData.Wait(Timeout.Infinite);

                if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Success)
                {
                    //リンス処理成功

                    // シャットダウンコマンドを投げて待機
                    SlaveCommCommand_0403 cmd0403 = new SlaveCommCommand_0403();
                    sequenceData.WaitCommandKind = CommandKind.Command1403;
                    this.pushSendQueueSingleSlave(cmd0403);
                    sequenceData.Wait(Timeout.Infinite);
                    if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Success)
                        seqStatus = true;
                    else
                    {
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
                            , "AutoStartWait：Shutdown processing time-out");
                    }
                }
                else
                {
                    //リンス処理失敗
                    System.Diagnostics.Debug.WriteLine("リンス処理タイムアウト。");
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                        , "AutoStartWait：Rinse processing time-out");
                }

                // イベント通知
                SequenceHelperMessage sequenceHelperMessage = new SequenceHelperMessage();
                sequenceHelperMessage.ModuleIndex = this.ModuleIdx;
                sequenceHelperMessage.MessageParameter = seqStatus;

                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.AutoStartupStartModule, sequenceHelperMessage);
            }

            return sequenceName;
        }

        /// <summary>
        /// ホストへの測定データ送信
        /// </summary>
        /// <remarks>
        /// ホストへ測定データを送信します。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <returns>シーケンス名称</returns>
        protected String sendResultSpecimenToHost(String sequenceName, Object[] param)
        {
            // リアルタイム問い合わせ中はバッチ問合せを行わない。
            List<OutPutSpecimenResultData> sendDatas = (List<OutPutSpecimenResultData>)param[param.Length - 3];
            ManualResetEvent canceller = (ManualResetEvent)param[param.Length - 2];


            HostCommCommand_0003 cmd0003 = null;

            ManualResetEvent waitCommand = new ManualResetEvent(false);
            CommSendResult dlg = (CommSendResult)delegate (Int32 ret, CommCommand cmd)
            {
                if (cmd == cmd0003)
                {
                    waitCommand.Set();
                }
            };
            Singleton<CarisXCommManager>.Instance.SetHostSendNotify(dlg);
            try
            {
                foreach (var data in sendDatas)
                {
                    cmd0003 = new HostCommCommand_0003();
                    cmd0003.AutoDilution = CarisXSubFunction.GetHostAutoDil(data.AutoDilution);
                    cmd0003.Comment = data.Comment;
                    cmd0003.Conc = data.GetConcentration();
                    cmd0003.DispCount = data.Count;
                    cmd0003.Judge = data.Judgement;
                    cmd0003.ManualDil = data.ManualDilution;
                    cmd0003.MeasDateTime = data.MeasureDateTime;
                    cmd0003.ProtocolNumber = Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.GetHostProtocolNumber(Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(data.GetMeasureProtocolIndex()).ProtocolName);
                    if (((CarisXIDString)data.RackId).GetSampleKind() == SampleKind.Priority)
                    {
                        cmd0003.RackID = String.Empty;
                        cmd0003.RackPos = 0;
                    }
                    else
                    {
                        cmd0003.RackID = data.RackId;
                        cmd0003.RackPos = data.RackPosition;
                    }
                    cmd0003.ReagLotNo = data.ReagentLotNo;
                    cmd0003.ReceiptNumber = data.GetReceiptNo();
                    if (data.GetReplicationRemarkId() != null)
                    {
                        cmd0003.Remark = data.GetReplicationRemarkId().Value;
                    }
                    if (data.GetRemarkId() != null)
                    {
                        cmd0003.Remark = data.GetRemarkId().Value;
                    }
                    if (data.GetRemarkId() == null && data.GetReplicationRemarkId() == null)
                    {
                        cmd0003.Remark = 0;
                    }
                    cmd0003.SampleId = data.PatientId;
                    cmd0003.SampleType = HostSampleType.N;
                    cmd0003.SeqNo = data.SequenceNo;
                    cmd0003.HostSampleKind = data.GetSpecimenMaterialType().GetHostSampleKind();
                    cmd0003.SampleLot = String.Empty;

                    // 1件単位で送信完了を待つ。
                    waitCommand.Reset();
                    Singleton<CarisXCommManager>.Instance.PushSendQueueHost(cmd0003);
                    waitCommand.WaitOne();

                    // キャンセル通知された場合ここで終了する
                    if (canceller.WaitOne(0) == true)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("{0} {1}", ex.Message, ex.StackTrace));
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("{0} {1}", ex.Message, ex.StackTrace));
            }
            Singleton<CarisXCommManager>.Instance.ResetHostSendNotify();

            // メインへ通知
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)(NotifyKind.SendSpecimenDataHostComplete));

            return sequenceName;
        }

        /// <summary>
        /// ホストへの測定データ送信
        /// </summary>
        /// <remarks>
        /// ホストへ測定データを送信します。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <returns>シーケンス名称</returns>
        protected String sendResultControlToHost(String sequenceName, Object[] param)
        {
            List<OutPutControlResultData> sendDatas = (List<OutPutControlResultData>)param[param.Length - 3];
            ManualResetEvent canceller = (ManualResetEvent)param[param.Length - 2];


            HostCommCommand_0003 cmd0003 = null;

            ManualResetEvent waitCommand = new ManualResetEvent(false);
            CommSendResult dlg = (CommSendResult)delegate (Int32 ret, CommCommand cmd)
            {
                if (cmd == cmd0003)
                {
                    waitCommand.Set();
                }
            };
            Singleton<CarisXCommManager>.Instance.SetHostSendNotify(dlg);
            try
            {
                foreach (var data in sendDatas)
                {
                    cmd0003 = new HostCommCommand_0003();
                    cmd0003.AutoDilution = HostAutoDil.NoDil;//                CarisXSubFunction.GetHostAutoDil( data.AutoDilution );
                    cmd0003.Comment = data.Comment;
                    cmd0003.Conc = data.GetConcentration();
                    cmd0003.DispCount = data.Count;
                    cmd0003.Judge = String.Empty;
                    cmd0003.ManualDil = 1;// data.ManualDilution;
                    cmd0003.MeasDateTime = data.MeasureDateTime;
                    cmd0003.ProtocolNumber = Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.GetHostProtocolNumber(Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(data.GetMeasureProtocolIndex()).ProtocolName);
                    cmd0003.RackID = data.RackId.DispPreCharString;
                    cmd0003.RackPos = data.RackPosition;
                    cmd0003.ReagLotNo = data.ReagentLotNo;
                    cmd0003.ReceiptNumber = 0;//data.GetReceiptNo();
                    if (data.GetReplicationRemarkId() != null)
                    {
                        cmd0003.Remark = data.GetReplicationRemarkId().Value;
                    }
                    if (data.GetRemarkId() != null)
                    {
                        cmd0003.Remark = data.GetRemarkId().Value;
                    }
                    if (data.GetRemarkId() == null && data.GetReplicationRemarkId() == null)
                    {
                        cmd0003.Remark = 0;
                    }
                    cmd0003.SampleId = String.Empty;//data.PatientId;
                    cmd0003.SampleType = HostSampleType.C;
                    cmd0003.SeqNo = data.SequenceNo;
                    cmd0003.HostSampleKind = HostSampleKind.SerumBloodPlasma;//data.GetSpecimenMaterialType().GetHostSampleKind();
                    cmd0003.SampleLot = data.ControlLotNo;

                    // 1件単位で送信完了を待つ。
                    waitCommand.Reset();
                    Singleton<CarisXCommManager>.Instance.PushSendQueueHost(cmd0003);
                    waitCommand.WaitOne();

                    // キャンセル通知された場合ここで終了する
                    if (canceller.WaitOne(0) == true)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("{0} {1}", ex.Message, ex.StackTrace));
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("{0} {1}", ex.Message, ex.StackTrace));
            }
            Singleton<CarisXCommManager>.Instance.ResetHostSendNotify();

            // メインへ通知
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)(NotifyKind.SendControlDataHostComplete));

            return sequenceName;
        }

        /// <summary>
        /// ホストへの測定データ送信（リアルタイム）
        /// </summary>
        /// <remarks>
        /// ホストへ測定データを送信します。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <returns>シーケンス名称</returns>
        protected String sendResultToHost(String sequenceName, Object[] param)
        {
            HostCommCommand_0003 sendData = (HostCommCommand_0003)param[param.Length - 2];

            ManualResetEvent waitCommand = new ManualResetEvent(false);
            CommSendResult dlg = (CommSendResult)delegate (Int32 ret, CommCommand cmd)
            {
                if (cmd == sendData)
                {
                    waitCommand.Set();
                }
            };

            Singleton<CarisXCommManager>.Instance.SetHostSendNotify(dlg);

            try
            {
                // 1件単位で送信完了を待つ。
                waitCommand.Reset();
                Singleton<CarisXCommManager>.Instance.PushSendQueueHost(sendData);
                waitCommand.WaitOne();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("{0} {1}", ex.Message, ex.StackTrace));
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("{0} {1}", ex.Message, ex.StackTrace));
            }

            Singleton<CarisXCommManager>.Instance.ResetHostSendNotify();

            return sequenceName;
        }

        /// <summary>
        /// ホストへのワークシート問合せ（リアルタイム）
        /// </summary>
        /// <remarks>
        /// ホストへのリアルタイムワークシート問合せを行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <returns>シーケンス名称</returns>
        protected String askWorkSheetToHost(String sequenceName, Object[] param)
        {
            // リアルタイム問い合わせ中はバッチ問合せを行わない。
            Singleton<SystemStatus>.Instance.SubStatus[SubStatusKind.InAskHost] = true;

            // ホスト応答待機時間は20秒（タイムアウト時間を設定可能に変更します（修改超时时间为可设置））
            Int32 hostTimeout = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UseRealtimeSampleAskWaitTime * 1000;
            SequenceCommData sequenceData = (SequenceCommData)param[param.Length - 1];
            AskWorkSheetData askData = (AskWorkSheetData)param[param.Length - 2];
            IMeasureIndicate indicate = askData.AskData;

            // 問い合わせコマンド送信Query command transmission
            HostCommCommand_0001 cmd0001 = new HostCommCommand_0001();

            cmd0001.DeviceNo = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.DeviceNoParameter.DeviceNo;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.AssayMode == AssayModeParameter.AssayModeKind.RackID)
            {
                // ラックID問合せ
                cmd0001.RackId = askData.AskData.RackID;
                cmd0001.RackPos = askData.AskData.SamplePosition;
            }
            else
            {
                // サンプルID問合せ
                // IDがあればIDを使用、なければRackID-Posを使用
                if (askData.AskData.SampleID != String.Empty)
                {
                    cmd0001.SampleId = askData.AskData.SampleID;
                }
                else
                {
                    cmd0001.RackId = askData.AskData.RackID;
                    cmd0001.RackPos = askData.AskData.SamplePosition;
                }

            }

            //Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
            //    , String.Format("【チェック】ホストへのワークシート問合せ（リアル）を開始                    RackID={0} Pos={1}", cmd0001.RackId, cmd0001.RackPos));

            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.HostCommand0002;
            Singleton<CarisXCommManager>.Instance.PushSendQueueHost(cmd0001);
            sequenceData.Wait(hostTimeout);
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.TimeOut)
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", "CarisXDPRErrorCode.AskWorkSheetTimeOut;");
                // 応答なし
                String extStr = "RackID :" + askData.AskData.RackID + "RackPos :" + Convert.ToString(askData.AskData.SamplePosition);
                CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.AskWorkSheetTimeOut, 0, extStr);
                askData.FromHostCommand = new HostCommCommand_0002();
                askData.AskTimeOuted = true;
            }
            else if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Abort)
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", "SequenceCommData.WaitResult.Abort");
                Singleton<SystemStatus>.Instance.SubStatus[SubStatusKind.InAskHost] = false;
                // 中断なのでここで終了
                askData.FromHostCommand = new HostCommCommand_0002();
                askData.AskAborted = true;
            }
            else
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", "askData.FromHostCommand = sequenceData.RcvCommand as HostCommCommand_0002;");
                // 正常ケース
                askData.FromHostCommand = sequenceData.RcvCommand as HostCommCommand_0002;

                //【IssuesNo:3】增加仪器扫描的样本ID与LIS传输过来的样本ID一致性的判断
                if (!string.Equals(askData.FromHostCommand.SampleID, askData.AskData.SampleID))
                {
                    string extStr = "ModuleID :" + askData.AskData.ModuleID.ToString() + "RackID :" + askData.AskData.RackID + "RackPos :" + Convert.ToString(askData.AskData.SamplePosition);
                    CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.FromHostWorkSheetSampleIDIsDifferent, 0, extStr);
                    askData.FromHostCommand = new HostCommCommand_0002();
                    askData.AskSampleIDIsDifferent = true;
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("【Host SampleID is different from ask SampleID】 HostSampleID={0} AskSampleID={1}", askData.FromHostCommand.SampleID, askData.AskData.SampleID));
                }
            }

            // 応答を通知する。

            //Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
            //    , String.Format("【チェック】HostWorkSheetメッセージを送信                                   RackID={0} Pos={1} 受信内容 ReciptNo={2}", cmd0001.RackId, cmd0001.RackPos, askData.FromHostCommand.ReceiptNumber));

            // メイン画面へ通知
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)(NotifyKind.HostWorkSheet), askData);
            Singleton<SystemStatus>.Instance.SubStatus[SubStatusKind.InAskHost] = false;

            sequenceData.Dispose();
            sequenceData = null;
            return sequenceName;
        }

        /// <summary>
        /// ホストへのワークシート問合せ（バッチ）
        /// </summary>
        /// <remarks>
        /// ホストへのワークシート問合せ（バッチ）を行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <returns>シーケンス名称</returns>
        protected String askWorkSheetToHostBatch(String sequenceName, Object[] param)
        {
            // ホスト応答待機時間は20秒
            Int32 hostTimeout = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UseRealtimeSampleAskWaitTime * 1000;
            // リアルタイム問い合わせ中はバッチ問合せを行わない。
            SequenceCommData sequenceData = (SequenceCommData)param[param.Length - 1];
            AskWorkSheetData askData = (AskWorkSheetData)param[param.Length - 3];
            var AskID = param[param.Length - 2];
            HostCommunicationSequencePattern seqptn = (HostCommunicationSequencePattern)param[0];

            // 問い合わせコマンド送信
            HostCommCommand_0001 cmd0001 = new HostCommCommand_0001();

            cmd0001.DeviceNo = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.DeviceNoParameter.DeviceNo;

            // 一般検体の場合は受付番号、STATの場合は検体IDで問合せを行う
            if (seqptn.HasFlag(HostCommunicationSequencePattern.Specimen))
            {
                cmd0001.ReceiptNo = Int32.Parse(AskID.ToString());
            }
            else
            {
                cmd0001.SampleId = AskID.ToString();
            }

            //Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
            //    , String.Format("【チェック】ホストへのワークシート問合せ（バッチ）を開始                    ReceiptNo={0}", cmd0001.ReceiptNo));

            Singleton<CarisXCommManager>.Instance.PushSendQueueHost(cmd0001);

            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.HostCommand0002;
            // タイムアウト設定する
            sequenceData.Wait(hostTimeout);

            // 応答が無い場合等、空白WSをつめる
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Abort)
            {
                // 中断なのでここで終了
                askData.FromHostCommand = new HostCommCommand_0002();
                askData.AskAborted = true;
            }
            else if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.TimeOut)
            {
                // 応答なし
                CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.AskWorkSheetTimeOut);
                askData.FromHostCommand = new HostCommCommand_0002();
                askData.AskTimeOuted = true;
            }
            else
            {
                askData.FromHostCommand = sequenceData.RcvCommand as HostCommCommand_0002;

            }

            //Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
            //    , String.Format("【チェック】HostWorkSheetBatchメッセージを送信                              ReceiptNo={0} 受信内容 RackID={1} Pos={2}", cmd0001.ReceiptNo, askData.FromHostCommand.RackID, askData.FromHostCommand.RackPos));

            // 応答を通知する。
            // メイン画面へ通知
            if (seqptn.HasFlag(HostCommunicationSequencePattern.Specimen))
                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)(NotifyKind.HostWorkSheetBatch), askData);
            else
                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)(NotifyKind.HostWorkSheetBatchSTAT), askData);

            sequenceData.Dispose();
            sequenceData = null;

            return sequenceName;
        }

        /// <summary>
        /// 温度問合せシーケンス
        /// </summary>
        /// <remarks>
        /// スレーブに対して温度の問合せを行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <param name="param">不使用</param>
        /// <returns>シーケンス名称</returns>
        protected String askTemperature(String sequenceName, Object[] param)
        {
            // 待機中のみ動作する
            //if ( Singleton<SystemStatus>.Instance.Status == SystemStatusKind.Wait )
            if ((Singleton<SystemStatus>.Instance.Status == SystemStatusKind.Standby) || (Singleton<SystemStatus>.Instance.Status == SystemStatusKind.WaitSlaveResponce))
            {
                // インキュベータ温度問合せコマンド送信
                SlaveCommCommand_0437 cmd0437 = new SlaveCommCommand_0437();
                cmd0437.Ctrl = CommandControlParameter.Ask;
                SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

                // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
                sequenceData.WaitCommandKind = CommandKind.Command1437;

                // コマンド送信
                this.pushSendQueueSingleSlave(cmd0437);

                // スレーブからのレスポンス待ち
                sequenceData.Wait(RESP_WAIT_TIME);

                Boolean aborted = false;
                Boolean updated = false;
                switch (sequenceData.WaitSuccess)
                {
                    case SequenceCommData.WaitResult.Success:

                        // 問合正常終了
                        // 温度データを設定クラスへ保存
                        SlaveCommCommand_1437 command1437 = (SlaveCommCommand_1437)sequenceData.RcvCommand;

                        Singleton<PublicMemory>.Instance.moduleTemperature[this.ModuleIdx].SetSlaveTemperatureFromDPRTemperature(command1437.Temp);

                        updated = true;
                        break;
                    case SequenceCommData.WaitResult.TimeOut:

                        // 応答なし
                        System.Diagnostics.Debug.WriteLine("インキュベーター温度問合せタイムアウト。");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                            , CarisXLogInfoBaseExtention.Empty, "Incubator temperature Query Timeout.");

                        break;
                    case SequenceCommData.WaitResult.Abort:
                        // 中断
                        aborted = true;
                        break;
                    default:
                        break;
                }

                if (!aborted)
                {
                    SlaveCommCommand_0438 cmd0438 = new SlaveCommCommand_0438();
                    // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
                    sequenceData.WaitCommandKind = CommandKind.Command1438;
                    cmd0438.Ctrl = CommandControlParameter.Ask;         // 問合せ
                    // コマンド送信
                    this.pushSendQueueSingleSlave(cmd0438);
                    sequenceData.Wait(RESP_WAIT_TIME);

                    switch (sequenceData.WaitSuccess)
                    {
                        case SequenceCommData.WaitResult.Success:

                            // 問合正常終了
                            // 温度データを設定クラスへ保存
                            SlaveCommCommand_1438 command1438 = (SlaveCommCommand_1438)sequenceData.RcvCommand;

                            Singleton<PublicMemory>.Instance.moduleTemperature[this.ModuleIdx].SetDPRTemperatureFromSlaveTemperature(command1438);

                            updated = true;
                            break;
                        case SequenceCommData.WaitResult.TimeOut:

                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("試薬保冷庫温度取得timeout。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                                , CarisXLogInfoBaseExtention.Empty, "試薬保冷庫温度取得timeout。");

                            break;
                        case SequenceCommData.WaitResult.Abort:
                            // 中断
                            aborted = true;
                            break;
                        default:
                            break;
                    }
                }

                if (updated)
                {
                    // 温度更新通知
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.UpdateTemperature);
                }

                sequenceData.Dispose();
                sequenceData = null;

            }

            return sequenceName;
        }

        /// <summary>
        /// 温度問合せシーケンス
        /// </summary>
        /// <remarks>
        /// スレーブに対して温度の問合せを行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <param name="param">不使用</param>
        /// <returns>シーケンス名称</returns>
        protected String setTemperature(String sequenceName, Object[] param)
        {
            TemperatureParameter tempSetting = (TemperatureParameter)param.First();
            // 待機中のみ動作する
            // インキュベータ温度問合せコマンド送信
            SlaveCommCommand_0437 cmd0437 = new SlaveCommCommand_0437();
            cmd0437.Ctrl = CommandControlParameter.Set;
            cmd0437.Temp.SetSlaveTemperatureFromDPRTemperature(tempSetting);
            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.Command1437;

            // コマンド送信
            this.pushSendQueueSingleSlave(cmd0437);

            // スレーブからのレスポンス待ち
            sequenceData.Wait(RESP_WAIT_TIME);

            switch (sequenceData.WaitSuccess)
            {
                case SequenceCommData.WaitResult.Success:

                    // 問合正常終了
                    //// 温度データを設定クラスへ保存
                    //SlaveCommCommand_1437 command1437 = (SlaveCommCommand_1437)sequenceData.RcvCommand;

                    //SlaveCommCommand_1437 cmd1437 = (SlaveCommCommand_1437)sequenceData.RcvCommand;
                    //Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.SetDPRTemperatureFromSlaveTemperature( cmd1437.Temp );

                    //// 温度更新通知
                    //Singleton<NotifyManager>.Instance.PushSignalQueue( (Int32)NotifyKind.UpdateTemperature, sequenceData );
                    break;
                case SequenceCommData.WaitResult.TimeOut:

                    // 応答なし
                    System.Diagnostics.Debug.WriteLine("Incubator temperature setting time-out");
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                        , CarisXLogInfoBaseExtention.Empty, "Incubator temperature setting time-out");

                    break;
                case SequenceCommData.WaitResult.Abort:
                    // 中断
                    break;
                default:
                    break;
            }
            sequenceData.Dispose();
            sequenceData = null;
            return sequenceName;
        }

        /// <summary>
        /// 初期シーケンス終了用メソッド
        /// </summary>
        /// <param name="sequenceName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected String initializeSequenceAbort(String sequenceName, Object[] param)
        {
            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 他に動作中のシーケンスがある為実行失敗
                // この時にセマフォを解除しないよう、異なるシーケンス名を返す
                return "CANCEL";
            }

            //スレッドにインスタンスが設定されている場合、スレッドを停止する
            if (InitializeSequenceThread != null)
            {
                InitializeSequenceThread.Abort();
            }

            return sequenceName;
        }

        /// <summary>
        /// 初期シーケンス開始用メソッド
        /// </summary>
        /// <param name="sequenceName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected String initializeSequenceStart(String sequenceName, Object[] param)
        {
            InitializeSequencePattern initseqptn;

            //paramにInitializeSequencePatternが設定されていない場合は処理しない
            if (param.Length < 1 || !Enum.TryParse(param[0].ToString(), out initseqptn))
            {
                return "CANCEL";
            }


            //パラメータに設定されている初期シーケンス
            initseqptn = (InitializeSequencePattern)param[0];


            //ラックまたはモジュールが、ユーザーよりも後に起動している場合のみ
            //初期シーケンスを実行するかのチェックを行う
            if (initseqptn.HasFlag(InitializeSequencePattern.StartsAfterUser))
            {
                //接続がオンラインになるまで待機
                if (initseqptn.HasFlag(InitializeSequencePattern.RackTransfer))
                {
                    //ラックで動作中の場合
                    while (!(Singleton<CarisXCommManager>.Instance.GetRackTransferCommStatus() == ConnectionStatus.Online))
                        Thread.Sleep(100);
                }
                else
                {
                    //モジュールで動作中の場合
                    while (!(Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(this.ModuleIdx) == ConnectionStatus.Online))
                        Thread.Sleep(100);
                }
            }

            //スレッドにインスタンスが設定されている場合、スレッドを停止する
            if (InitializeSequenceThread != null)
            {
                InitializeSequenceThread.Abort();
            }


            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, INITIALIZE_SEQUENCE_WAIT_TIME))
            {
                // 他に動作中のシーケンスがある為実行失敗
                // この時にセマフォを解除しないよう、異なるシーケンス名を返す
                return "CANCEL";
            }

            //初期シーケンスを別スレッドで実行する
            if (initseqptn.HasFlag(InitializeSequencePattern.RackTransfer))
            {
                // ラック搬送
                InitializeSequenceThread = new Thread(new ParameterizedThreadStart(initializeSequenceRackTransfer));
            }
            else
            {
                // スレーブ１～４
                InitializeSequenceThread = new Thread(new ParameterizedThreadStart(initializeSequenceModule));
            }

            InitializeSequenceThread.Start(initseqptn);
            InitializeSequenceThread.Join();
            InitializeSequenceThread = null;

            return sequenceName;

        }

        /// <summary>
        /// モジュール初期シーケンスの実行
        /// </summary>
        /// <remarks>
        /// モジュールとの初期シーケンス動作を行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <returns>シーケンス名称</returns>
        protected void initializeSequenceModule(object param)
        {
            try
            {
                InitializeSequencePattern initseqptn = (InitializeSequencePattern)param;
                
                ProgressInfo progressInfo = new ProgressInfo();
                progressInfo.TargetModuleNo = (RackModuleIndex)CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)this.ModuleIdx);
                progressInfo.IsAbort = false;
                isMotorError = false;

                // ラムダ式をコマンド受信イベントに登録し、シーケンス終了時に登録解除する。
                // ラムダ式が呼び出されるスレッドはメインスレッドからになる。
                using (SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo))
                {
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.InitializeProgress, progressInfo.Clone());

                    //モジュールがユーザーよりも先に起動している場合のみ実行
                    if (initseqptn.HasFlag(InitializeSequencePattern.StartsBeforeUser))
                    {
                        #region [スレーブ接続確認(0467)]

                        // スレーブ接続確認コマンドの送信
                        Boolean connectFlag = this.checkInitializeConnection(sequenceData, progressInfo.TargetModuleNo);

                        #endregion

                        if (!connectFlag)
                        {
                            //ユーザーがモジュールと接続出来ない場合

                            System.Diagnostics.Debug.WriteLine("Initial sequence：サブレディ待機開始（スレーブが検出されなかった）");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                           "Initial sequence：Slave wait start(Slave has not detected).");
                            //初期シーケンス一時停止を通知
                            //ここを実行しておかないと、初回起動時の初期シーケンス画面が消えなくなる
                            progressInfo.IsAbort = true;
                            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.InitializePause, progressInfo.Clone());

                            //ここに来た場合はモジュールがまだ起動しておらず、待機する必要がないため終了する
                            return;
                        }
                    }

                    //初期シーケンスを始めるので、フラグを未完了の状態に戻す
                    flgInitializeSequenceCompleted = false;

                    // 進捗率更新（10%）
                    progressInfo.ProgressPos = 10;
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.InitializeProgress, progressInfo.Clone());

                    #region [ソフトウェア識別(0401)]

                    // ソフトウェア識別コマンド
                    SlaveCommCommand_0401 cmd0401 = new SlaveCommCommand_0401();
                    sequenceData.WaitCommandKind = CommandKind.Command1401;
                    cmd0401.SoftwareKind1 = SlaveCommCommand_0401.SoftwareKind.Running;
                    cmd0401.Control = CommandControlParameter.Start;

                    bool isSendCommand = false;
                    while (!isSendCommand)
                    {
                        // ソフトウェア識別コマンド送信
                        this.pushSendQueueSingleSlave(cmd0401);

                        // ソフトウェア識別コマンド（レスポンス）の受信待ち
                        sequenceData.Wait(SOFTWARE_IDENTIFICATION_RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: ソフトウェア識別コマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to software identification command");
                        }
                        else
                        {
                            // ソフトウェア識別コマンド送信成功
                            isSendCommand = true;
                        }
                    }

                    #endregion

                    #region [ユニットパラメータ(0448～0463)]
                    {
                        //コンフィグパラメータ読み込み
                        ParameterFilePreserve<CarisXConfigParameter> config = Singleton<ParameterFilePreserve<CarisXConfigParameter>>.Instance;
                        config.LoadRaw();

                        #region [ケース搬送ユニットパラメータ(0448)]

                        // ケース搬送ユニットパラメータコマンド
                        SlaveCommCommand_0448 cmd0448 = new SlaveCommCommand_0448();
                        sequenceData.WaitCommandKind = CommandKind.Command1448;
                        // パラメータなし
                        cmd0448 = config.Param.SlaveList[this.ModuleIdx].tipCellCaseTransferUnitConfigParam;
                        this.pushSendQueueSingleSlave(cmd0448);

                        // ケース搬送ユニットパラメータコマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: ケース搬送ユニットパラメータコマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to case transfer unit param command");
                        }

                        #endregion

                        #region [試薬保冷庫ユニットパラメータ(0449)]

                        // 試薬保冷庫パラメータコマンド
                        SlaveCommCommand_0449 cmd0449 = new SlaveCommCommand_0449();
                        sequenceData.WaitCommandKind = CommandKind.Command1449;
                        // パラメータなし
                        cmd0449 = config.Param.SlaveList[this.ModuleIdx].reagentStorageUnitConfigParam;
                        this.pushSendQueueSingleSlave(cmd0449);

                        // 試薬保冷庫パラメータコマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: 試薬保冷庫パラメータコマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to reagent storage unit param command");
                        }

                        #endregion

                        #region [STATユニットパラメータ(0450)]

                        // STATユニットパラメータコマンド
                        SlaveCommCommand_0450 cmd0450 = new SlaveCommCommand_0450();
                        sequenceData.WaitCommandKind = CommandKind.Command1450;
                        // パラメータなし
                        cmd0450 = config.Param.SlaveList[this.ModuleIdx].sTATUnitConfigParam;
                        this.pushSendQueueSingleSlave(cmd0450);

                        // STATユニットパラメータコマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: STATユニットパラメータコマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to stat unit param command");
                        }

                        #endregion

                        #region [サンプル分注ユニットパラメータ(0451)]

                        // サンプル分注ユニットパラメータコマンド
                        SlaveCommCommand_0451 cmd0451 = new SlaveCommCommand_0451();
                        sequenceData.WaitCommandKind = CommandKind.Command1451;
                        cmd0451 = config.Param.SlaveList[this.ModuleIdx].sampleDispenseConfigParam;
                        this.pushSendQueueSingleSlave(cmd0451);

                        // サンプル分注ユニットパラメータコマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: サンプル分注ユニットパラメータコマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to sample dispense unit param command");
                        }

                        #endregion

                        #region [反応容器搬送ユニットパラメータ(0452)]

                        // 反応容器搬送ユニットパラメータコマンド
                        SlaveCommCommand_0452 cmd0452 = new SlaveCommCommand_0452();
                        sequenceData.WaitCommandKind = CommandKind.Command1452;
                        cmd0452 = config.Param.SlaveList[this.ModuleIdx].reactionCellTransferUnitConfigParam;
                        this.pushSendQueueSingleSlave(cmd0452);

                        // 反応容器搬送ユニットパラメータコマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: 反応容器搬送ユニットパラメータコマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to reaction cell transfer unit param command");
                        }

                        #endregion

                        #region [反応テーブルユニットパラメータ(0453)]

                        // 反応テーブルユニットパラメータコマンド
                        SlaveCommCommand_0453 cmd0453 = new SlaveCommCommand_0453();
                        sequenceData.WaitCommandKind = CommandKind.Command1453;
                        cmd0453 = config.Param.SlaveList[this.ModuleIdx].reactionTableUnitConfigParam;
                        this.pushSendQueueSingleSlave(cmd0453);

                        // 反応テーブルユニットパラメータコマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: 反応テーブルユニットパラメータコマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to reaction table unit param command");
                        }

                        #endregion

                        #region [B/Fテーブルユニットパラメータ(0454)]

                        // B/Fテーブルユニットパラメータコマンド
                        SlaveCommCommand_0454 cmd0454 = new SlaveCommCommand_0454();
                        sequenceData.WaitCommandKind = CommandKind.Command1454;
                        cmd0454 = config.Param.SlaveList[this.ModuleIdx].bFTableUnitConfigParam;
                        this.pushSendQueueSingleSlave(cmd0454);

                        // B/Fテーブルユニットパラメータコマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: B/Fテーブルユニットパラメータコマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to bf table unit param command");
                        }

                        #endregion

                        #region [トラベラーユニットパラメータ(0455)]

                        // トラベラーユニットパラメータコマンド
                        SlaveCommCommand_0455 cmd0455 = new SlaveCommCommand_0455();
                        sequenceData.WaitCommandKind = CommandKind.Command1455;
                        cmd0455 = config.Param.SlaveList[this.ModuleIdx].travelerAndDisposalUnitConfigParam;
                        this.pushSendQueueSingleSlave(cmd0455);

                        // トラベラーユニットパラメータコマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: トラベラーユニットパラメータコマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to traveler and disposal unit param command");
                        }

                        #endregion

                        #region [試薬分注1部ユニットパラメータ(0456)]

                        // 試薬分注1部ユニットパラメータコマンド
                        SlaveCommCommand_0456 cmd0456 = new SlaveCommCommand_0456();
                        sequenceData.WaitCommandKind = CommandKind.Command1456;
                        cmd0456 = config.Param.SlaveList[this.ModuleIdx].r1DispenseUnitConfigParam;
                        this.pushSendQueueSingleSlave(cmd0456);

                        // 試薬分注1部ユニットパラメータコマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: 試薬分注1部ユニットパラメータコマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to r1 dispense unit param command");
                        }

                        #endregion

                        #region [試薬分注2部ユニットパラメータ(0457)]

                        // 試薬分注2部ユニットパラメータコマンド 
                        SlaveCommCommand_0457 cmd0457 = new SlaveCommCommand_0457();
                        sequenceData.WaitCommandKind = CommandKind.Command1457;
                        cmd0457 = config.Param.SlaveList[this.ModuleIdx].r2DispenseUnitConfigParam;
                        this.pushSendQueueSingleSlave(cmd0457);

                        // 試薬分注2部ユニットパラメータコマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: 試薬分注2部ユニットパラメータコマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to r2 dispense unit param command");
                        }

                        #endregion

                        #region [B/F1ユニットパラメータ(0458)]

                        // B/F1ユニットパラメータコマンド
                        SlaveCommCommand_0458 cmd0458 = new SlaveCommCommand_0458();
                        sequenceData.WaitCommandKind = CommandKind.Command1458;
                        cmd0458 = config.Param.SlaveList[this.ModuleIdx].bF1UnitConfigParam;
                        this.pushSendQueueSingleSlave(cmd0458);

                        // B/F1ユニットパラメータコマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: B/F1ユニットパラメータコマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to bf1 unit param command");
                        }

                        #endregion

                        #region [B/F2ユニットパラメータ(0459)]

                        // B/F2ユニットパラメータコマンド
                        SlaveCommCommand_0459 cmd0459 = new SlaveCommCommand_0459();
                        sequenceData.WaitCommandKind = CommandKind.Command1459;
                        cmd0459 = config.Param.SlaveList[this.ModuleIdx].bF2UnitConfigParam;
                        this.pushSendQueueSingleSlave(cmd0459);

                        // B/F2ユニットパラメータコマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: B/F2ユニットパラメータコマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to bf2 unit param command");
                        }

                        #endregion

                        #region [希釈分注ユニットパラメータ(0460)]

                        // 希釈分注ユニットパラメータコマンド
                        SlaveCommCommand_0460 cmd0460 = new SlaveCommCommand_0460();
                        sequenceData.WaitCommandKind = CommandKind.Command1460;
                        cmd0460 = config.Param.SlaveList[this.ModuleIdx].dilutionDispenseUnitConfigParam;
                        this.pushSendQueueSingleSlave(cmd0460);

                        // 希釈分注ユニットパラメータコマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: 希釈分注ユニットパラメータコマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to dilution dispense unit param command");
                        }

                        #endregion

                        #region [プレトリガユニットパラメータ(0461)]

                        // プレトリガユニットパラメータコマンド 
                        SlaveCommCommand_0461 cmd0461 = new SlaveCommCommand_0461();
                        sequenceData.WaitCommandKind = CommandKind.Command1461;
                        cmd0461 = config.Param.SlaveList[this.ModuleIdx].pretriggerUnitConfigParam;
                        this.pushSendQueueSingleSlave(cmd0461);

                        // プレトリガユニットパラメータコマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: プレトリガユニットパラメータコマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to pre-trigger unit param command");
                        }

                        #endregion

                        #region [トリガ分注測光ユニットパラメータ(0462)]

                        // トリガ分注測光ユニットパラメータコマンド 
                        SlaveCommCommand_0462 cmd0462 = new SlaveCommCommand_0462();
                        sequenceData.WaitCommandKind = CommandKind.Command1462;
                        cmd0462 = config.Param.SlaveList[this.ModuleIdx].triggerUnitConfigParam;
                        this.pushSendQueueSingleSlave(cmd0462);

                        // トリガ分注測光ユニットパラメータコマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: トリガ分注測光ユニットパラメータコマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to trigger unit param command");
                        }

                        #endregion

                        #region [流体配管ユニットパラメータ(0463)]

                        // 流体配管ユニットパラメータコマンド
                        SlaveCommCommand_0463 cmd0463 = new SlaveCommCommand_0463();
                        sequenceData.WaitCommandKind = CommandKind.Command1463;
                        cmd0463 = config.Param.SlaveList[this.ModuleIdx].fluidAndPipingUnitConfigParam;
                        this.pushSendQueueSingleSlave(cmd0463);

                        // 流体配管ユニットパラメータコマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: 流体配管ユニットパラメータコマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to fluid and piping unit param command");
                        }

                        #endregion
                    }
                    #endregion

                    #region [システム設定]
                    {
                        #region [システムパラメータ(0404)]

                        // システムパラメータコマンド
                        SlaveCommCommand_0404 cmd0404 = new SlaveCommCommand_0404();
                        sequenceData.WaitCommandKind = CommandKind.Command1404;
                        cmd0404.SetSystemParameter(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param, this.ModuleIdx);
                        this.pushSendQueueSingleSlave(cmd0404);

                        // システムパラメータコマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: システムパラメータコマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to system param command");
                        }

                        #endregion

                        #region [測定プロトコル(0405)]

                        // 測定プロトコルコマンド（プロトコルパラメータコマンド）
                        int totalMeasProtoCount = Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList.Count;
                        foreach (var protocol in Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList)
                        {
                            // 進捗率更新（10%）
                            progressInfo.ProgressPos = 10 + (int)( ( (double)protocol.ProtocolIndex / (double)totalMeasProtoCount ) * 10 );
                            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.InitializeProgress, progressInfo.Clone());

                            SlaveCommCommand_0405 cmd0405 = new SlaveCommCommand_0405();
                            sequenceData.WaitCommandKind = CommandKind.Command1405;
                            cmd0405.SetProtocolParameter(protocol);
                            this.pushSendQueueSingleSlave(cmd0405);

                            // 測定プロトコルコマンドコマンド（レスポンス）の受信待ち
                            sequenceData.Wait(RESP_WAIT_TIME);
                            if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                            {
                                // 応答なし
                                System.Diagnostics.Debug.WriteLine("Initial sequence: 測定プロトコルコマンドコマンドに応答がありませんでした。");
                                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                          "Initial sequence:   There was no response to meas protocol command");
                            }
                        }

                        #endregion
                    }
                    #endregion

                    #region [センサー無効(0441)]

                    // センサー無効コマンド
                    SlaveCommCommand_0441 cmd0441 = new SlaveCommCommand_0441();
                    sequenceData.WaitCommandKind = CommandKind.Command1441;
                    // センサー無効パラメータ読み込み
                    Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance.LoadRaw();
                    cmd0441 = Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance.Param.SlaveList[this.ModuleIdx].sensorParameterUseNoUse;
                    this.pushSendQueueSingleSlave(cmd0441);

                    // センサー無効コマンド（レスポンス）の受信待ち
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 応答なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence: センサー無効コマンドに応答がありませんでした。");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                  "Initial sequence:   There was no response to invalid sensor command");
                    }

                    #endregion

                    // システム初期化完了
                    progressInfo.EndSystemInit = ProgressInfoEndStatusKind.Completed;

                    // 進捗率更新（20%）
                    progressInfo.ProgressPos = 20;
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.InitializeProgress, progressInfo.Clone());

                    #region [カレンダー設定(0413)]

                    // カレンダー設定コマンド
                    SlaveCommCommand_0413 cmd0413 = new SlaveCommCommand_0413();
                    sequenceData.WaitCommandKind = CommandKind.Command1413;
                    // 当日日付設定
                    cmd0413.SetDateTime(DateTime.Now);
                    this.pushSendQueueSingleSlave(cmd0413);

                    // カレンダー設定コマンド（レスポンス）の受信待ち
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 応答なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence: カレンダー設定コマンドに応答がありませんでした。");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                  "Initial sequence:   There was no response to calendar settings command");
                    }

                    #endregion

                    #region [スタートアップ開始(0442)]

                    // スタートアップ開始コマンド
                    SlaveCommCommand_0442 cmd0442 = new SlaveCommCommand_0442();
                    sequenceData.WaitCommandKind = CommandKind.Command1442;
                    this.pushSendQueueSingleSlave(cmd0442);

                    // スタートアップ開始コマンド（レスポンス）の受信待ち
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 応答なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence: スタートアップ開始コマンドに応答がありませんでした。");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                  "Initial sequence:   There was no response to startup start command");
                    }

                    #endregion

                    #region [寿命部品使用回数設定問い合わせ(0444)]

                    // 寿命部品使用回数設定問い合わせコマンド
                    SlaveCommCommand_0444 cmd0444 = new SlaveCommCommand_0444();
                    // 使用回数/期限は設定クラスから読込む（SupplieParam.xml)
                    CommandDataMediator.SetSupplieParamToCmd(this.ModuleIdx, CommandControlParameter.Set, Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param, ref cmd0444);
                    // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
                    sequenceData.WaitCommandKind = CommandKind.Command1444;
                    // コマンド送信
                    this.pushSendQueueSingleSlave(cmd0444);
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 応答なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence: there was no response to aged parts use set number of times the query command.");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                            , CarisXLogInfoBaseExtention.Empty, "Initial sequence there was no response to aged parts use set number of times the query command.");
                    }
                    else
                    // 応答は設定クラスに設定する。
                    {
                        SupplieParameter supplie = Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param;
                        CommandDataMediator.SetSupplieCmdToParam(sequenceData.RcvCommand as SlaveCommCommand_1444, ref supplie);

                        Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Save();
                    }

                    #endregion

                    #region [総アッセイ数設定コマンド(0484)]

                    // 総アッセイ数設定コマンド
                    SlaveCommCommand_0484 cmd0484 = new SlaveCommCommand_0484();
                    // 設定クラスから読込む（SupplieParam.xml)
                    CommandDataMediator.SetTotalAssayTimesToCmd(this.ModuleIdx, Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param, ref cmd0484);
                    // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
                    sequenceData.WaitCommandKind = CommandKind.Command1484;
                    // コマンド送信
                    this.pushSendQueueSingleSlave(cmd0484);
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 応答なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence: there was no response to total assay number setting command.");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                            , CarisXLogInfoBaseExtention.Empty, "Initial sequence there was no response to total assay number setting command.");
                    }

                    #endregion

                    // 初期化処理無効モードがOFFか判定
                    if (!DlgInitialize.NoneInitializeMode)
                    {
                        #region [モーター初期化(0406)]

                        // 全モーター初期化コマンド
                        SlaveCommCommand_0406 cmd0406 = new SlaveCommCommand_0406();
                        sequenceData.WaitCommandKind = CommandKind.Command1406;
                        cmd0406.MotorNumber = 0; // モーター番号＝0で装置全体の初期化
                        this.pushSendQueueSingleSlave(cmd0406);

                        // 全モーター初期化コマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: 全モータ初期化コマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to all motor initialization command");
                        }

                        #endregion

                        #region [モーターセルフチェック(0407)]

                        // モーターエラーチェック
                        if (this.checkMotorErrorRackModule(progressInfo) == false)
                        {
                            // モーターエラーが発生していない場合のみ、全モーターセルフチェックを行う

                            // 全モーターセルフチェックコマンド
                            SlaveCommCommand_0407 cmd0407 = new SlaveCommCommand_0407();
                            sequenceData.WaitCommandKind = CommandKind.Command1407;
                            cmd0407.MotorNumber = 0; // モーター番号＝0で装置全体の初期化
                            this.pushSendQueueSingleSlave(cmd0407);

                            // 全モーターセルフチェックコマンド（レスポンス）の受信待ち
                            sequenceData.Wait(RESP_WAIT_TIME);
                            if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                            {
                                // 応答なし
                                System.Diagnostics.Debug.WriteLine("Initial sequence: 全モーターセルフチェックコマンドに応答がありませんでした。");
                                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                             "Initial sequence  There was no response to all motors self-check command.");
                            }
                        }
                        #endregion
                    }

                    // モーターエラーチェック
                    if (this.checkMotorErrorRackModule(progressInfo) == false)
                    {
                        // モーターエラーが発生していない場合のみ、モーター自己診断完了表示
                        progressInfo.EndMotorSelf = ProgressInfoEndStatusKind.Completed;
                    }

                    // 進捗率更新（30%）
                    progressInfo.ProgressPos = 30;
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.InitializeProgress, progressInfo.Clone());

                    #region [PID定数変更(0474)]
                    {
#if DEBUG
                        //PID定数設定ファイルから値を読み出し（デバッグ時のみ）
                        ParameterFilePreserve<TempPIDParameter> TempPID = Singleton<ParameterFilePreserve<TempPIDParameter>>.Instance;
                        TempPID.LoadRaw();
                        SlaveCommCommand_0474 TempPIDParam;
#endif

                        #region [反応テーブル温度]

                        // PID定数設定コマンド（反応テーブル温度）を送信
                        SlaveCommCommand_0474 StartComm1 = new SlaveCommCommand_0474();
                        sequenceData.WaitCommandKind = CommandKind.Command1474;
#if DEBUG
                        TempPIDParam = TempPID.Param.SlaveList[this.ModuleIdx].reactionTableTempPIDParam;
                        StartComm1.ProportionalConstValue = TempPIDParam.ProportionalConstValue;
                        StartComm1.IntegralConstvalue = TempPIDParam.IntegralConstvalue;
                        StartComm1.DifferentialConstValue = TempPIDParam.DifferentialConstValue;
#else
                        StartComm1.ProportionalConstValue = 2.0;
                        StartComm1.IntegralConstvalue = 400.0;
                        StartComm1.DifferentialConstValue = 1.0;
#endif
                        StartComm1.TempArea = PIDTempArea.ReactionTableTemp;
                        this.pushSendQueueSingleSlave(StartComm1);

                        // PID定数設定コマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine(String.Format("Initial sequence: PID定数変更コマンド[{0}]に応答がありませんでした。", PIDTempArea.ReactionTableTemp));
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                         "Initial sequence  There was no response to pid const setting command.");
                        }

                        #endregion

                        #region [B/Fテーブル温度]

                        // PID定数設定コマンド（B/Fテーブル温度）を送信
                        SlaveCommCommand_0474 StartComm2 = new SlaveCommCommand_0474();
                        sequenceData.WaitCommandKind = CommandKind.Command1474;
#if DEBUG
                        TempPIDParam = TempPID.Param.SlaveList[this.ModuleIdx].bFTableTempPIDParam;
                        StartComm2.ProportionalConstValue = TempPIDParam.ProportionalConstValue;
                        StartComm2.IntegralConstvalue = TempPIDParam.IntegralConstvalue;
                        StartComm2.DifferentialConstValue = TempPIDParam.DifferentialConstValue;
#else
                        StartComm2.ProportionalConstValue = 2.0;
                        StartComm2.IntegralConstvalue = 400.0;
                        StartComm2.DifferentialConstValue = 1.0;
#endif
                        StartComm2.TempArea = PIDTempArea.BFTableTemp;
                        this.pushSendQueueSingleSlave(StartComm2);

                        // PID定数設定コマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine(String.Format("Initial sequence: PID定数変更コマンド[{0}]に応答がありませんでした。", PIDTempArea.BFTableTemp));
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                         "Initial sequence  There was no response to pid const setting command.");
                        }

                        #endregion

                        #region [B/F1プレヒート温度]

                        // PID定数設定コマンド（B/F1プレヒート温度）を送信
                        SlaveCommCommand_0474 StartComm3 = new SlaveCommCommand_0474();
                        sequenceData.WaitCommandKind = CommandKind.Command1474;
#if DEBUG
                        TempPIDParam = TempPID.Param.SlaveList[this.ModuleIdx].bF1TempPIDParam;
                        StartComm3.ProportionalConstValue = TempPIDParam.ProportionalConstValue;
                        StartComm3.IntegralConstvalue = TempPIDParam.IntegralConstvalue;
                        StartComm3.DifferentialConstValue = TempPIDParam.DifferentialConstValue;
#else
                        StartComm3.ProportionalConstValue = 10.0;
                        StartComm3.IntegralConstvalue = 75.0;
                        StartComm3.DifferentialConstValue = 5.0;
#endif
                        StartComm3.TempArea = PIDTempArea.BF1PreheatTemp;
                        this.pushSendQueueSingleSlave(StartComm3);

                        // PID定数設定コマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine(String.Format("Initial sequence: PID定数変更コマンド[{0}]に応答がありませんでした。", PIDTempArea.BF1PreheatTemp));
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                         "Initial sequence  There was no response to pid const setting command.");
                        }

                        #endregion

                        #region [B/F2プレヒート温度]

                        // PID定数設定コマンド（B/F2プレヒート温度）を送信
                        SlaveCommCommand_0474 StartComm4 = new SlaveCommCommand_0474();
                        sequenceData.WaitCommandKind = CommandKind.Command1474;
#if DEBUG
                        TempPIDParam = TempPID.Param.SlaveList[this.ModuleIdx].bF2TempPIDParam;
                        StartComm4.ProportionalConstValue = TempPIDParam.ProportionalConstValue;
                        StartComm4.IntegralConstvalue = TempPIDParam.IntegralConstvalue;
                        StartComm4.DifferentialConstValue = TempPIDParam.DifferentialConstValue;
#else
                        StartComm4.ProportionalConstValue = 10.0;
                        StartComm4.IntegralConstvalue = 75.0;
                        StartComm4.DifferentialConstValue = 5.0;
#endif
                        StartComm4.TempArea = PIDTempArea.BF2PreheatTemp;
                        this.pushSendQueueSingleSlave(StartComm4);

                        // PID定数設定コマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine(String.Format("Initial sequence: PID定数変更コマンド[{0}]に応答がありませんでした。", PIDTempArea.BF2PreheatTemp));
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                         "Initial sequence  There was no response to pid const setting command.");
                        }

                        #endregion

                        #region [R1プローブプレヒート温度]

                        // PID定数設定コマンド（R1プローブプレヒート温度）を送信
                        SlaveCommCommand_0474 StartComm5 = new SlaveCommCommand_0474();
                        sequenceData.WaitCommandKind = CommandKind.Command1474;
#if DEBUG
                        TempPIDParam = TempPID.Param.SlaveList[this.ModuleIdx].r1TempPIDParam;
                        StartComm5.ProportionalConstValue = TempPIDParam.ProportionalConstValue;
                        StartComm5.IntegralConstvalue = TempPIDParam.IntegralConstvalue;
                        StartComm5.DifferentialConstValue = TempPIDParam.DifferentialConstValue;
#else
                        StartComm5.ProportionalConstValue = 10.0;
                        StartComm5.IntegralConstvalue = 75.0;
                        StartComm5.DifferentialConstValue = 5.0;
#endif
                        StartComm5.TempArea = PIDTempArea.R1PreheatTemp;
                        this.pushSendQueueSingleSlave(StartComm5);

                        // PID定数設定コマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine(String.Format("Initial sequence: PID定数変更コマンド[{0}]に応答がありませんでした。", PIDTempArea.R1PreheatTemp));
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                         "Initial sequence  There was no response to pid const setting command.");
                        }

                        #endregion

                        #region [R2プローブプレヒート温度]

                        // PID定数設定コマンド（R2プローブプレヒート温度）を送信
                        SlaveCommCommand_0474 StartComm6 = new SlaveCommCommand_0474();
                        sequenceData.WaitCommandKind = CommandKind.Command1474;
#if DEBUG
                        TempPIDParam = TempPID.Param.SlaveList[this.ModuleIdx].r2TempPIDParam;
                        StartComm6.ProportionalConstValue = TempPIDParam.ProportionalConstValue;
                        StartComm6.IntegralConstvalue = TempPIDParam.IntegralConstvalue;
                        StartComm6.DifferentialConstValue = TempPIDParam.DifferentialConstValue;
#else
                        StartComm6.ProportionalConstValue = 10.0;
                        StartComm6.IntegralConstvalue = 75.0;
                        StartComm6.DifferentialConstValue = 5.0;
#endif
                        StartComm6.TempArea = PIDTempArea.R2PreheatTemp;
                        this.pushSendQueueSingleSlave(StartComm6);

                        // PID定数設定コマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine(String.Format("Initial sequence: PID定数変更コマンド[{0}]に応答がありませんでした。", PIDTempArea.R2PreheatTemp));
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                         "Initial sequence  There was no response to pid const setting command.");
                        }

                        #endregion

                        #region [測光部温度]

                        // PID定数設定コマンド（測光部温度）を送信
                        SlaveCommCommand_0474 StartComm7 = new SlaveCommCommand_0474();
                        sequenceData.WaitCommandKind = CommandKind.Command1474;
#if DEBUG
                        TempPIDParam = TempPID.Param.SlaveList[this.ModuleIdx].ptotometryTempPIDParam;
                        StartComm7.ProportionalConstValue = TempPIDParam.ProportionalConstValue;
                        StartComm7.IntegralConstvalue = TempPIDParam.IntegralConstvalue;
                        StartComm7.DifferentialConstValue = TempPIDParam.DifferentialConstValue;
#else
                        StartComm7.ProportionalConstValue = 3.0;
                        StartComm7.IntegralConstvalue = 100.0;
                        StartComm7.DifferentialConstValue = 1.0;
#endif
                        StartComm7.TempArea = PIDTempArea.PtotometryTemp;
                        this.pushSendQueueSingleSlave(StartComm7);

                        // PID定数設定コマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine(String.Format("Initial sequence: PID定数変更コマンド[{0}]に応答がありませんでした。", PIDTempArea.PtotometryTemp));
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                         "Initial sequence  There was no response to pid const setting command.");
                        }

                        #endregion
                    }
                    #endregion

                    #region [PID制御開始(0472)]

                    // PID制御開始コマンド（すべて）
                    SlaveCommCommand_0472 cmd0472 = new SlaveCommCommand_0472();
                    sequenceData.WaitCommandKind = CommandKind.Command1472;
                    cmd0472.Control = PIDControl.Start;
                    cmd0472.TempArea = PIDTempArea.All;
                    this.pushSendQueueSingleSlave(cmd0472);

                    // PID制御開始コマンド（レスポンス）の受信待ち
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 応答なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence: PID制御開始コマンドに応答がありませんでした。");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                     "Initial sequence  There was no response to pid control start command.");
                    }

                    #endregion

                    // 初期化処理無効モードがOFFか判定
                    if (!DlgInitialize.NoneInitializeMode)
                    {
                        #region [光学系セルフチェック(0408)]

                        // モーターエラーチェック
                        if (this.checkMotorErrorRackModule(progressInfo) == false)
                        {
                            // モーターエラーが発生していない場合のみ、光学系セルフチェックコマンド送信

                            // 光学系セルフチェックコマンド
                            SlaveCommCommand_0408 cmd0408 = new SlaveCommCommand_0408();
                            sequenceData.WaitCommandKind = CommandKind.Command1408;
                            this.pushSendQueueSingleSlave(cmd0408);

                            // 光学系セルフチェックコマンド（レスポンス）の受信待ち
                            sequenceData.Wait(RESP_WAIT_TIME);
                            if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                            {
                                // 応答なし
                                System.Diagnostics.Debug.WriteLine("Initial sequence: 光学系セルフチェックコマンドに応答がありませんでした。");
                                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                          "Initial sequence  There was no response to the optical system self-check command.");
                            }
                            SlaveCommCommand_1408 cmd1408 = ( (SlaveCommCommand_1408)sequenceData.RcvCommand );

                            //ダークが100を超えるのとき
                            if (( cmd1408.ExecuteCheck == true ) && ( cmd1408.Dark > 100 ))
                            {
                                // ダークエラーメッセージ                            
                                DPRErrorCode dPRErrorCode = new DPRErrorCode(CarisXDPRErrorCode.DarkError.ErrorCode, CarisXDPRErrorCode.DarkError.Arg
                                    , CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)ModuleIdx));
                                CarisXSubFunction.WriteDPRErrorHist(dPRErrorCode);
                                // そのまま次の処理へ。
                                progressInfo.EndOpticalSelf = ProgressInfoEndStatusKind.Error;
                            }
                            else
                            {
                                progressInfo.EndOpticalSelf = ProgressInfoEndStatusKind.Completed;
                            }
                        }

                        #endregion
                    }

                    // 進捗率更新（30%）
                    progressInfo.ProgressPos = 30;
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.InitializeProgress, progressInfo.Clone());

                    #region [試薬保冷庫BC読み込み無効(0493)]

                    // 試薬保冷庫BC読み込み無効コマンド送信
                    SlaveCommCommand_0493 cmd0493 = new SlaveCommCommand_0493();
                    sequenceData.WaitCommandKind = CommandKind.Command1493;
                    cmd0493.ReadReagBC = Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.ReadReagentBC.GetReadReagentBC(this.ModuleIdx);
                    this.pushSendQueueSingleSlave(cmd0493);

                    // 試薬保冷庫BC読み込み無効コマンド（レスポンス）の受信待ち
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 応答なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence: 試薬保冷庫BC読み込み無効コマンドに応答がありませんでした。");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                  "Initial sequence  There was no response to reagent storage invalid read bc command.");
                    }

                    #endregion

                    #region [残量セット(0434)]

                    SlaveCommCommand_0434 cmd0434 = new SlaveCommCommand_0434();
                    sequenceData.WaitCommandKind = CommandKind.Command1434;
                    IRemainAmountInfoSet remainAmountSet = cmd0434; // インターフェースの実装クラスをrefで渡せない為、ここで作業用にインターフェース型へ移し変える。内容の設定される実体はコマンドクラス。
                    Singleton<ReagentDB>.Instance.GetReagentRemain(ref remainAmountSet, CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)this.ModuleIdx));
                    Singleton<ReagentHistoryDB>.Instance.GetReagentHistory(ref remainAmountSet);
                    this.pushSendQueueSingleSlave(cmd0434);

                    // 残量セットコマンド（レスポンス）の受信待ち
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 応答なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence: 残量セットコマンドに応答がありませんでした。");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                  "Initial sequence  There was no response to remain set command.");
                    }

                    #endregion

                    #region [残量チェック(0414)]

                    // 残量チェックコマンド(Type2)
                    SlaveCommCommand_0414 cmd0414 = new SlaveCommCommand_0414();

                    // 初期化処理無効モードがOFFか判定
                    if (!DlgInitialize.NoneInitializeMode)
                    {
                        // システム設定によりチェック種別判定
                        if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ReagentCheckAtStartUpParameter.Enable)
                        {
                            cmd0414.KindRemainCheck = SlaveCommCommand_0414.RetCheckRemainCom.AllCheck;
                        }
                        else
                        {
                            cmd0414.KindRemainCheck = SlaveCommCommand_0414.RetCheckRemainCom.Info;
                        }
                    }
                    else
                    {
                        // 常に「Type1:情報のみ」を設定
                        cmd0414.KindRemainCheck = SlaveCommCommand_0414.RetCheckRemainCom.Info;
                    }

                    // モーターエラーチェック
                    if ((this.checkMotorErrorRackModule(progressInfo) == false)
                     || (cmd0414.KindRemainCheck == SlaveCommCommand_0414.RetCheckRemainCom.Info))
                    {
                        // モーターエラーが発生していない場合、
                        // または残量チェック種別がInfoの場合、残量チェックコマンド送信
                        sequenceData.WaitCommandKind = CommandKind.Command1414;

                        this.pushSendQueueSingleSlave(cmd0414);

                        // 残量チェックコマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: 残量チェックコマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                "Initial sequence: There was no response on the remaining amount check command");
                        }
                        else
                        {
                            // 残量チェック完了
                            progressInfo.EndReagentCheck = ProgressInfoEndStatusKind.Completed;
                            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.InitializeProgress, progressInfo.Clone());
                        }
                    }

                    #endregion

                    // 初期化処理無効モードがOFFか判定
                    if (!DlgInitialize.NoneInitializeMode)
                    {
                        #region [全プライム(0409)]
                        
                        // モーターエラーチェック
                        if (this.checkMotorErrorRackModule(progressInfo) == false)
                        {
                            // モーターエラーが発生していない場合のみ、プライムコマンド送信

                            // 自動プライム有効設定確認
                            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoPrimeParameter.Enable)
                            {
                                // 全プライムコマンド
                                SlaveCommCommand_0409 cmd0409 = new SlaveCommCommand_0409();
                                cmd0409.ItemPrime = SlaveCommCommand_0409.ItemPrimeKind.All;
                                sequenceData.WaitCommandKind = CommandKind.Command1409;
                                this.pushSendQueueSingleSlave(cmd0409);

                                // 全プライムコマンド（レスポンス）の受信待ち
                                sequenceData.Wait(RESP_WAIT_TIME);

                                if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                                {
                                    // 応答なし 全プライムコマンドに応答がありませんでした。
                                    System.Diagnostics.Debug.WriteLine("Initial sequence: 全プライムコマンドに応答がありませんでした。");
                                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                        "Initial sequence: There was no response to all prime command.");
                                }
                                else
                                {
                                    // 全プライム完了
                                    progressInfo.EndPrime = ProgressInfoEndStatusKind.Completed;
                                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.InitializeProgress, progressInfo.Clone());
                                }
                            }
                        }

                        #endregion
                    }

                    #region [寿命部品使用回数設定問い合わせ(0444)]

                    // 寿命部品使用回数設定問い合わせコマンド
                    cmd0444 = new SlaveCommCommand_0444();
                    // 使用回数/期限は設定クラスから読込む（SupplieParam.xml)
                    CommandDataMediator.SetSupplieParamToCmd(this.ModuleIdx, CommandControlParameter.Set, Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param, ref cmd0444);
                    // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
                    sequenceData.WaitCommandKind = CommandKind.Command1444;
                    // コマンド送信
                    this.pushSendQueueSingleSlave(cmd0444);
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 応答なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence: there was no response to aged parts use set number of times the query command.");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence there was no response to aged parts use set number of times the query command.");
                    }
                    else
                    // 応答は設定クラスに設定する。
                    {
                        SupplieParameter supplie = Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param;
                        CommandDataMediator.SetSupplieCmdToParam(sequenceData.RcvCommand as SlaveCommCommand_1444, ref supplie);

                        Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Save();
                    }

                    #endregion

                    #region [サンプル必要量設定(0485)]

                    // サンプル必要量設定コマンド
                    SlaveCommCommand_0485 cmd0485 = new SlaveCommCommand_0485();
                    sequenceData.WaitCommandKind = CommandKind.Command1485;
                    // パラメータ読込
                    Singleton<ParameterFilePreserve<SampleRequireAmount>>.Instance.LoadRaw();
                    cmd0485.Set(Singleton<ParameterFilePreserve<SampleRequireAmount>>.Instance.Param);
                    this.pushSendQueueSingleSlave(cmd0485);

                    // サンプル必要量設定コマンド（レスポンス）の受信待ち
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 応答なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence: サンプル必要量設定コマンドに応答がありませんでした。");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                  "Initial sequence  There was no response to need sample volume command.");
                    }

                    #endregion

                    #region [スタートアップ終了(0443)]

                    // スタートアップ終了コマンド
                    SlaveCommCommand_0443 cmd0043 = new SlaveCommCommand_0443();
                    sequenceData.WaitCommandKind = CommandKind.Command1443;
                    this.pushSendQueueSingleSlave(cmd0043);

                    // スタートアップ終了コマンド（レスポンス）の受信待ち
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 応答なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence: スタートアップ終了コマンドに応答がありませんでした。");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                  "Initial sequence  There was no response to startup end command.");
                    }

                    #endregion

                    // 進捗率更新（40%）
                    progressInfo.ProgressPos = 40;
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.InitializeProgress, progressInfo.Clone());

                    //
                    // スレーブからの受信コマンド
                    //

                    #region [モーターパラメータ設定(0509)]

                    SlaveCommCommand_0509 cmd0509 = new SlaveCommCommand_0509();
                    SlaveCommCommand_1509 cmd1509 = new SlaveCommCommand_1509();
                    MotorParameterSetUp motorParameterSetUp = Singleton<MotorParameterSetUp>.Instance;

                    // モーター番号リスト取得
                    List<MotorNoList> motorNoList = CarisXSubFunction.GetMotorNoListForModule();
                    int totalMotorCount = motorNoList.Count;
                    while (motorNoList.Count != 0)
                    {
                        // プログレスバー
                        progressInfo.ProgressPos = 50 + ( totalMotorCount - motorNoList.Count );
                        Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.InitializeProgress, progressInfo.Clone());

                        // スレーブからの受信コマンド
                        cmd0509 = new SlaveCommCommand_0509();
                        sequenceData.WaitCommandKind = CommandKind.Command0509;

                        // モーターパラメータ設定コマンドの受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: モーターパラメータ設定コマンドの送信がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence There was no response to the motor parameter setting command.");
                        }

                        // モータパラメータ設定コマンドの受信データ取得
                        cmd0509 = (SlaveCommCommand_0509)sequenceData.RcvCommand;

                        // モーター番号リストから受信済み番号を削除
                        motorNoList.Remove((MotorNoList)cmd0509.MotorNo);

                        //XMLに書き込み
                        motorParameterSetUp.SetUpMotorParam(cmd0509, this.ModuleIdx);
                        //System.Diagnostics.Debug.WriteLine(String.Format("MNO=[{0}]",cmd0109.MotorNo)); ちゃんと受信できてるか確認用

                        // モーターパラメータ設定コマンド（レスポンス）を送信
                        cmd1509 = new SlaveCommCommand_1509();
                        this.pushSendQueueSingleSlave(cmd1509);
                    }

                    //モーターパラメータ保存
                    motorParameterSetUp.MotorparamSave();

                    #endregion

                    #region [バージョン通知(0511)]

                    // バージョン通知コマンドの受信待ち
                    sequenceData.WaitCommandKind = CommandKind.Command0511;
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 送信なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence: バージョン通知コマンドの送信がありませんでした。");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                       "Initial sequence There was no version notification command");
                    }

                    // スレーブバージョンコマンド（レスポンス）を送信
                    SlaveCommCommand_1511 cmd1511 = new SlaveCommCommand_1511();
                    this.pushSendQueueSingleSlave(cmd1511);

                    #endregion

                    #region [サブイベント(0505)]

                    // サブイベントコマンドの受信待ち
                    sequenceData.WaitCommandKind = CommandKind.Command0505;
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 応答なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence: サブイベントコマンドの送信がありませんでした。");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                       "Initial sequence: There was no sub-event command.");
                    }

                    // サブイベントコマンド（レスポンス）を送信
                    SlaveCommCommand_1505 cmd1505 = new SlaveCommCommand_1505();
                    this.pushSendQueueSingleSlave(cmd1505);

                    #endregion

                    #region [総アッセイ数通知(0514)]

                    // 総アッセイ数通知コマンドの受信待ち
                    sequenceData.WaitCommandKind = CommandKind.Command0514;
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 送信なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence: 総アッセイ数通知コマンドの送信がありませんでした。");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                       "Initial sequence: There was no notification of total num of assays command.");
                    }

                    /* FormMainFrameにあるonCommandで応答を返すため、ここでは不要
                    // 総アッセイ数通知コマンド（レスポンス）を送信
                    SlaveCommCommand_1514 cmd1514 = new SlaveCommCommand_1514();
                    this.pushSendQueueSingleSlave(cmd1514);
                    */

                    #endregion

                    // 進捗率更新（100%）
                    progressInfo.ProgressPos = 100;
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.InitializeProgress, progressInfo.Clone());

                    FormMaintenanceMain formMaintenanceMain = (FormMaintenanceMain)System.Windows.Forms.Application.OpenForms[typeof(FormMaintenanceMain).Name];
                    if (!( formMaintenanceMain is null ))
                    {
                        //メンテナンス画面が起動している場合、メンテナンスモードの旨をスレーブに通知する
                        cmd0401 = new SlaveCommCommand_0401();
                        cmd0401.SoftwareKind1 = SlaveCommCommand_0401.SoftwareKind.Maintenance;
                        cmd0401.Control = CommandControlParameter.Start;
                        sequenceData.WaitCommandKind = CommandKind.Command1401;
                        this.pushSendQueueSingleSlave(cmd0401);

                        // ソフトウェア識別コマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: ソフトウェア識別コマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to software identification command");
                        }
                    }

                    //初期シーケンス完了済フラグを立てる
                    flgInitializeSequenceCompleted = true;
                }
            }
            catch (ThreadAbortException ThreadAbortEx)
            {
                //初期シーケンスを実行中にスレーブが再起動した際に、再度初期シーケンス実行できるようにするためにスレッドを停止させる
                //（初期シーケンスを実行中にスレーブを再起動させても、初期シーケンスが再実行するようにしてほしい、という要望の対応）
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("initial sequence is Restarted."));
                System.Diagnostics.Debug.WriteLine(ThreadAbortEx.Message);
            }
            catch (Exception ex)
            {
                // 初期シーケンスでの例外エラー調査用Try-Catch(仮)
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, String.Format("An exception occurred in the initial sequence. message = {0} stack={1}", ex.Message, ex.StackTrace));
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return;
        }

        /// <summary>
        /// ラック搬送初期シーケンスの実行
        /// </summary>
        /// <remarks>
        /// ラック搬送との初期シーケンス動作を行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <returns>シーケンス名称</returns>
        protected void initializeSequenceRackTransfer(object param)
        {
            try
            {
                InitializeSequencePattern initseqptn = (InitializeSequencePattern)param;

                ProgressInfo progressInfo = new ProgressInfo();
                progressInfo.TargetModuleNo = RackModuleIndex.RackTransfer;
                progressInfo.IsAbort = false;
                isMotorError = false;

                // ラムダ式をコマンド受信イベントに登録し、シーケンス終了時に登録解除する。
                // ラムダ式が呼び出されるスレッドはメインスレッドからになる。
                using (SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo))
                {

                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.InitializeProgress, progressInfo.Clone());

                    //ラック搬送がユーザーよりも先に起動している場合のみ実行
                    if (initseqptn.HasFlag(InitializeSequencePattern.StartsBeforeUser))
                    {
                        #region [ラック接続確認(0067)]

                        // ラック接続確認コマンドの送信
                        Boolean connectFlag = this.checkInitializeConnection(sequenceData, progressInfo.TargetModuleNo);

                        #endregion

                        if (!connectFlag)
                        {
                            //ユーザーがラック搬送と接続出来ない場合

                            System.Diagnostics.Debug.WriteLine("Initial sequence：ラックレディ待機開始（ラック搬送が検出されなかった）");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                                , CarisXLogInfoBaseExtention.Empty, "Initial sequence：RackTransfer Wait Start（RackTransfer has not detected）");

                            //初期シーケンス一時停止を通知
                            //ここを実行しておかないと、初回起動時の初期シーケンス画面が消えなくなる
                            progressInfo.IsAbort = true;
                            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.InitializePause, progressInfo.Clone());

                            //ここに来た場合はラック搬送がまだ起動しておらず、待機する必要がないため終了する
                            return;
                        }
                    }

                    //初期シーケンスを始めるので、フラグを未完了の状態に戻す
                    flgInitializeSequenceCompleted = false;

                    progressInfo.ProgressPos = 10;
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.InitializeProgress, progressInfo.Clone());

                    #region [ソフトウェア識別(0001)]

                    // ソフトウェア識別コマンドを送信
                    RackTransferCommCommand_0001 cmd0001 = new RackTransferCommCommand_0001();
                    cmd0001.SoftwareKind1 = RackTransferCommCommand_0001.SoftwareKind.Running;
                    cmd0001.Control = CommandControlParameter.Start;
                    sequenceData.WaitCommandKind = CommandKind.RackTransferCommand1001;

                    bool isSendCommand = false;
                    while (!isSendCommand)
                    {
                        // ソフトウェア識別コマンド送信
                        Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0001);

                        // ソフトウェア識別コマンド（レスポンス）の受信待ち
                        sequenceData.Wait(SOFTWARE_IDENTIFICATION_RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: ソフトウェア識別コマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to software identification initialization command");
                        }
                        else
                        {
                            // ソフトウェア識別コマンド送信成功
                            isSendCommand = true;
                        }
                    }

                    #endregion

                    #region [ユニットコンフィグ]
                    {
                        //コンフィグパラメータ読み込み
                        ParameterFilePreserve<CarisXConfigParameter> config = Singleton<ParameterFilePreserve<CarisXConfigParameter>>.Instance;
                        config.LoadRaw();

                        #region [ラック搬送ユニットパラメータ(0047)]

                        // ラック搬送ユニットパラメータコマンド
                        RackTransferCommCommand_0047 cmd0047 = new RackTransferCommCommand_0047();
                        cmd0047 = config.Param.RackList[0].rackTransferUnitConfigParam;
                        sequenceData.WaitCommandKind = CommandKind.RackTransferCommand1047;
                        Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0047);
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: ラック搬送ユニットパラメータコマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to rack transfer unit parameter command");
                        }

                        #endregion
                    }
                    #endregion

                    #region [システムパラメータ(0004)]

                    // システムパラメータコマンドを送信
                    RackTransferCommCommand_0004 cmd0004 = new RackTransferCommCommand_0004();
                    cmd0004.SetSystemParameter(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param);
                    sequenceData.WaitCommandKind = CommandKind.RackTransferCommand1004;
                    Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0004);

                    // システムパラメータコマンド（レスポンス）の受信待ち
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 応答なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence: システムパラメータコマンドに応答がありませんでした。");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                    "Initial sequence:   There was no response to system parameter command");
                    }

                    #endregion

                    #region [検体バーコード設定(0082)]

                    // 検体バーコード設定コマンドを送信
                    RackTransferCommCommand_0082 cmd0082 = new RackTransferCommCommand_0082();
                    // バーコード種別
                    switch (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.SelectBCKind)
                    {
                        case SampleBCRParameter.BarcodeKind.Type1:
                            cmd0082.Kind1 = RackTransferCommCommand_0082.Kind.NW7ITFCODE39CODE128;
                            break;
                        case SampleBCRParameter.BarcodeKind.Type2:
                            cmd0082.Kind1 = RackTransferCommCommand_0082.Kind.ITF2of5CODE39CODE128;
                            break;
                        case SampleBCRParameter.BarcodeKind.Type3:
                            cmd0082.Kind1 = RackTransferCommCommand_0082.Kind.NW7ITF2of5CODE128;
                            break;
                        case SampleBCRParameter.BarcodeKind.Type4:
                            cmd0082.Kind1 = RackTransferCommCommand_0082.Kind.NW7ITFCODE392of5;
                            break;
                    }
                    // チェックデジット検査有無
                    cmd0082.CDCheck = Convert.ToByte(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableCDCheck);
                    // チェックデジット転送
                    cmd0082.CDTrans = Convert.ToByte(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableCDCharTrans);
                    // ST/SPコード転送
                    cmd0082.STSPTrans = Convert.ToByte(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableSTSPTrans);
                    sequenceData.WaitCommandKind = CommandKind.RackTransferCommand1082;
                    Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0082);

                    // 検体バーコードコマンド（レスポンス）の受信待ち
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 応答なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence: 検体バーコード設定コマンドに応答がありませんでした。");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                    "Initial sequence:   There was no response to setting sample barcode command");
                    }

                    #endregion

                    #region [センサー無効(0041)]

                    // センサー無効パラメータ読み込み
                    Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance.LoadRaw();

                    // センサー無効コマンドを送信
                    RackTransferCommCommand_0041 cmd0041 = new RackTransferCommCommand_0041();
                    // パラメータファイル設定を反映
                    cmd0041 = Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse;
                    sequenceData.WaitCommandKind = CommandKind.RackTransferCommand1041;
                    Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0041);

                    // センサー無効コマンド（レスポンス）の受信待ち
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 応答なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence: センサー無効コマンドに応答がありませんでした。");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                  "Initial sequence:   There was no response to invalid sensor command");
                    }

                    #endregion

                    // システム初期化完了
                    progressInfo.EndSystemInit = ProgressInfoEndStatusKind.Completed;
                    progressInfo.ProgressPos = 20;
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.InitializeProgress, progressInfo.Clone());

                    #region [カレンダー設定(0013)]

                    // カレンダー設定コマンドを送信
                    RackTransferCommCommand_0013 cmd0013 = new RackTransferCommCommand_0013();
                    // 現在日時を設定
                    cmd0013.SetDateTime(DateTime.Now);
                    sequenceData.WaitCommandKind = CommandKind.RackTransferCommand1013;
                    Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0013);

                    // カレンダー設定コマンド（レスポンス）の受信待ち
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 応答なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence: カレンダー設定コマンドに応答がありませんでした。");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                  "Initial sequence:   There was no response to set calendar command");
                    }

                    #endregion

                    #region [スタートアップ開始(0042)]

                    // スタートアップ開始コマンド
                    RackTransferCommCommand_0042 cmd0042 = new RackTransferCommCommand_0042();
                    sequenceData.WaitCommandKind = CommandKind.RackTransferCommand1042;
                    Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0042);

                    // スタートアップ開始コマンド（レスポンス）の受信待ち
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 応答なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence: スタートアップ開始コマンドに応答がありませんでした。");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                  "Initial sequence:   There was no response to startup start command");
                    }

                    #endregion

                    // 2秒待つ?
                    Thread.Sleep(2000);

                    // 初期化処理無効モードを判定
                    if (!DlgInitialize.NoneInitializeMode)
                    {
                        #region [モーター初期化(0006)]

                        // 全モーター初期化コマンドを送信
                        RackTransferCommCommand_0006 cmd0006 = new RackTransferCommCommand_0006();
                        // モーター番号＝0で装置全体の初期化
                        cmd0006.MotorNumber = 0;
                        sequenceData.WaitCommandKind = CommandKind.RackTransferCommand1006;
                        Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0006);

                        // モーター初期化コマンド(レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: 全モータ初期化コマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to all motor initialization command");
                        }

                        #endregion

                        #region [モーターセルフチェック(0007)]

                        // モーターエラーチェック
                        if (this.checkMotorErrorRackModule(progressInfo) == false)
                        {
                            // モーターエラーが発生していない場合のみ、全モーターセルフチェックコマンドを送信

                            // 全モーターセルフチェックコマンドを送信
                            RackTransferCommCommand_0007 cmd0007 = new RackTransferCommCommand_0007();
                            // モーター番号＝0で装置全体の初期化
                            cmd0007.MotorNumber = 0;
                            sequenceData.WaitCommandKind = CommandKind.RackTransferCommand1007;
                            Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0007);

                            // モーターセルフチェックコマンド（レスポンス）の受信待ち
                            sequenceData.Wait(RESP_WAIT_TIME);
                            if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                            {
                                // 応答なし
                                System.Diagnostics.Debug.WriteLine("Initial sequence: 全モーターセルフチェックコマンドに応答がありませんでした。");
                                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                             "Initial sequence  There was no response to all motors self-check command.");
                            }
                        }

                        #endregion
                    }

                    // モーターエラーチェック
                    if (this.checkMotorErrorRackModule(progressInfo) == false)
                    {
                        // モーターエラーが発生していない場合のみ、モーター自己診断完了表示
                        progressInfo.EndMotorSelf = ProgressInfoEndStatusKind.Completed;
                    }

                    progressInfo.ProgressPos = 30;
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.InitializeProgress, progressInfo.Clone());

                    #region [スタートアップ終了(0043)]

                    // スタートアップ終了コマンドを送信
                    RackTransferCommCommand_0043 cmd0043 = new RackTransferCommCommand_0043();
                    sequenceData.WaitCommandKind = CommandKind.RackTransferCommand1043;
                    Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0043);

                    // スタートアップ終了（レスポンス）の受信待ち
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 応答なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence: スタートアップ終了コマンドに応答がありませんでした。");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                  "Initial sequence:   There was no response to startup end command");
                    }

                    #endregion

                    progressInfo.ProgressPos = 40;
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.InitializeProgress, progressInfo.Clone());

                    //
                    // ラック搬送からの受信コマンド
                    //

                    #region [モーターパラメータ設定(0109)]

                    // 全モーターパラメータ設定を送信
                    RackTransferCommCommand_0109 cmd0109 = new RackTransferCommCommand_0109();
                    RackTransferCommCommand_1109 cmd1109 = new RackTransferCommCommand_1109();
                    MotorParameterSetUp motorParameterSetUp = Singleton<MotorParameterSetUp>.Instance;

                    // ラック搬送用モーター番号リストを取得
                    List<MotorNoList> motorNoList = CarisXSubFunction.GetMotorNoListForRackTransfer();

                    // モーター最大数
                    int maxMotorNoList = motorNoList.Count;

                    while (motorNoList.Count != 0)
                    {
                        //プログレスバー
                        progressInfo.ProgressPos = 50 + (maxMotorNoList - motorNoList.Count);
                        Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.InitializeProgress, progressInfo.Clone());

                        // ラック搬送からの受信コマンド
                        cmd0109 = new RackTransferCommCommand_0109();
                        sequenceData.WaitCommandKind = CommandKind.RackTransferCommand0109;

                        // モーターパラメータ設定コマンドの受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence There was no response to the 0109 command.");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence There was no response to the 0109 command(Motor Parameter Setting) .");
                        }

                        // モータパラメータ設定コマンドの受信データを取得
                        cmd0109 = (RackTransferCommCommand_0109)sequenceData.RcvCommand;

                        // モーター番号リストから受信済み番号を削除
                        motorNoList.Remove((MotorNoList)cmd0109.MotorNo);

                        // XMLに書き込み
                        motorParameterSetUp.SetUpMotorParam(cmd0109);

                        // モーターパラメータ設定コマンド（レスポンス）を送信
                        cmd1109 = new RackTransferCommCommand_1109();
                        Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd1109);
                    }

                    //モーターパラメータ保存        
                    motorParameterSetUp.MotorparamSave();

                    #endregion

                    #region [バージョン通知(0111)]

                    // バージョン通知コマンドの受信待ち
                    sequenceData.WaitCommandKind = CommandKind.RackTransferCommand0111;
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 応答なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence There was no version command");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                       "Initial sequence There was no version command");
                    }
                    RackTransferCommCommand_1111 cmd1111 = new RackTransferCommCommand_1111();
                    Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd1111);

                    #endregion

                    #region [ラックイベント通知(0105)]

                    // ラックイベント通知
                    sequenceData.WaitCommandKind = CommandKind.RackTransferCommand0105;
                    sequenceData.Wait(RESP_WAIT_TIME);
                    if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                    {
                        // 応答なし
                        System.Diagnostics.Debug.WriteLine("Initial sequence not have a sub-event command.");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                       "Initial sequence: not have a sub-event command.");
                    }
                    RackTransferCommCommand_1105 cmd1105 = new RackTransferCommCommand_1105();
                    Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd1105);

                    #endregion

                    progressInfo.ProgressPos = 100;
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.InitializeProgress, progressInfo.Clone());

                    FormMaintenanceMain formMaintenanceMain = (FormMaintenanceMain)System.Windows.Forms.Application.OpenForms[typeof(FormMaintenanceMain).Name];
                    if (!(formMaintenanceMain is null))
                    {
                        //メンテナンス画面が起動している場合、メンテナンスモードの旨をラックに通知する
                        cmd0001 = new RackTransferCommCommand_0001();
                        cmd0001.SoftwareKind1 = RackTransferCommCommand_0001.SoftwareKind.Maintenance;
                        cmd0001.Control = CommandControlParameter.Start;
                        sequenceData.WaitCommandKind = CommandKind.RackTransferCommand1001;
                        Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0001);

                        // ソフトウェア識別コマンド（レスポンス）の受信待ち
                        sequenceData.Wait(RESP_WAIT_TIME);
                        if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                        {
                            // 応答なし
                            System.Diagnostics.Debug.WriteLine("Initial sequence: ソフトウェア識別コマンドに応答がありませんでした。");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                      "Initial sequence:   There was no response to software identification initialization command");
                        }
                    }

                    //初期シーケンス完了済フラグを立てる
                    flgInitializeSequenceCompleted = true;
                }
            }
            catch (ThreadAbortException ThreadAbortEx)
            {
                //初期シーケンスを実行中にスレーブが再起動した際に、再度初期シーケンス実行できるようにするためにスレッドを停止させる
                //（初期シーケンスを実行中にスレーブを再起動させても、初期シーケンスが再実行するようにしてほしい、という要望の対応）
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("initial sequence is Restarted."));
                System.Diagnostics.Debug.WriteLine(ThreadAbortEx.Message);
            }
            catch (Exception ex)
            {
                // 初期シーケンスでの例外エラー調査用Try-Catch(仮)
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, String.Format("An exception occurred in the initial sequence. message = {0} {1}", ex.Message, ex.StackTrace));
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return;
        }

        /// <summary>
        /// 初回接続確認
        /// </summary>
        /// <param name="sequenceData"></param>
        /// <returns></returns>
        private Boolean checkInitializeConnection(SequenceCommData sequenceData, RackModuleIndex index)
        {
            Boolean result = false;

            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）

            sequenceData.WaitCommandKind = CommandKind.Unknown;

            // コマンド送信
            if (index == RackModuleIndex.RackTransfer)
            {
                Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(new RackTransferCommCommand_0067());
            }
            else
            {
                this.pushSendQueueSingleSlave(new SlaveCommCommand_0467());
            }
            
            // 通信状態を取得
            ConnectionStatus connectStatus = new ConnectionStatus();

            for( int count = 0; count < 50; count++ )
            {
                if (index == RackModuleIndex.RackTransfer)
                {
                    // ラック搬送の接続状態を確認
                    connectStatus = Singleton<CarisXCommManager>.Instance.GetRackTransferCommStatus();
                }
                else
                {
                    // スレーブの接続状態を確認
                    int moduleIndex = CarisXSubFunction.ModuleIDToModuleIndex((int)index);
                    connectStatus = Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleIndex);
                }
                
                // 通信状態がオンラインの場合、初期シーケンス処理開始
                if(connectStatus == ConnectionStatus.Online)
                {
                    result = true;
                    break;
                }

                System.Threading.Thread.Sleep(100);
            }

            return result;
        }

        /// <summary>
        /// モーターエラーチェック
        /// </summary>
        /// <param name="progressInfo"></param>
        /// <returns>判定結果：true=エラー有り / false=エラー無し</returns>
        private Boolean checkMotorErrorRackModule(ProgressInfo progressInfo)
        {
            // 判定結果（初期値=エラー無し）
            Boolean result = false;

            // モーターエラー発生フラグがOFFの場合
            if ( this.isMotorError == false )
            {
                // モジュールステータスがモーターエラーの場合
                if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[(Int32)progressInfo.TargetModuleNo] == Status.SystemStatusKind.MotorError)
                {
                    // モーターエラー発生フラグをONにする
                    this.isMotorError = true;

                    // ログ出力
                    System.Diagnostics.Debug.WriteLine(String.Format("Initial sequence: モーターエラーがありました。TargetModuleNo={0}", (Int32)progressInfo.TargetModuleNo));
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                String.Format("Initial sequence: There was motor error. TargetModuleNo={0}", (Int32)progressInfo.TargetModuleNo));

                    // モーター自己診断エラー表示
                    progressInfo.EndMotorSelf = ProgressInfoEndStatusKind.Error;

                    // 進捗率更新通知
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.InitializeProgress, progressInfo.Clone());

                    // 判定結果にエラー有りを設定
                    result = true;
                }
            }
            else
            {
                // 既にモーターエラー発生フラグがONになっている場合

                // 判定結果にエラー有りを設定
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 洗浄液供給開始シーケンス
        /// </summary>
        /// <remarks>
        /// 洗浄液供給シーケンス動作を行います。
        /// </remarks>
        /// <param name="args">パラメータ（開始/停止)</param>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <returns>シーケンス名称</returns>
        protected String startReplaceWashsolutionTankSequence(String sequenceName, Object[] param)
        {
            SequenceSyncObject syncObject = (SequenceSyncObject)param.Last();
            syncObject.Status = SequenceSyncObject.SequenceStatus.Wait;

            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 待機時間超過
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                syncObject.SetEnd();
                return sequenceName;
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);


            // 洗浄液供給コマンド
            SlaveCommCommand_0495 cmd0495 = new SlaveCommCommand_0495();
            cmd0495.Status = SlaveCommCommand_0495.WashSolutionSupplyStatus.Start;
            cmd0495.tankBufferKind = TankBufferKind.Tank;

            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.Command1495;

            // コマンド送信
            this.pushSendQueueSingleSlave(cmd0495);

            // スレーブから応答を待機
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Success)
            {
                syncObject.Status = SequenceSyncObject.SequenceStatus.Success;
                syncObject.SequenceResultData = sequenceData.RcvCommand;
            }
            else
            {
                // 応答なし
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                System.Diagnostics.Debug.WriteLine("洗浄液供給コマンドのレスポンスがありませんでした。");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, "洗浄液供給コマンドのレスポンスがありませんでした。");
            }
            //Free the event
            sequenceData.Dispose();
            sequenceData = null;

            syncObject.SetEnd();
            return sequenceName;
        }

        /// <summary>
        /// 洗浄液供給停止シーケンス
        /// </summary>
        /// <remarks>
        /// 洗浄液供給シーケンス動作を行います。
        /// </remarks>
        /// <param name="args">パラメータ（開始/停止)</param>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <returns>シーケンス名称</returns>
        protected String stopReplaceWashsolutionTankSequence(String sequenceName, Object[] param)
        {
            SequenceSyncObject syncObject = (SequenceSyncObject)param.Last();
            syncObject.Status = SequenceSyncObject.SequenceStatus.Wait;

            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 待機時間超過
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                syncObject.SetEnd();
                return sequenceName;
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);


            // 残量チェックコマンド
            SlaveCommCommand_0495 cmd0495 = new SlaveCommCommand_0495();
            cmd0495.Status = SlaveCommCommand_0495.WashSolutionSupplyStatus.Stop;
            cmd0495.tankBufferKind = TankBufferKind.Tank;

            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.Command1495;

            // コマンド送信
            this.pushSendQueueSingleSlave(cmd0495);

            // スレーブから応答を待機
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Success)
            {
                syncObject.Status = SequenceSyncObject.SequenceStatus.Success;
                syncObject.SequenceResultData = sequenceData.RcvCommand;
            }
            else
            {
                // 応答なし
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                System.Diagnostics.Debug.WriteLine("洗浄液供給コマンドのレスポンスがありませんでした。");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, "洗浄液供給コマンドのレスポンスがありませんでした。");
            }
            //Free the event
            sequenceData.Dispose();
            sequenceData = null;

            syncObject.SetEnd();
            return sequenceName;
        }

        /// <summary>
        /// 試薬準備開始シーケンス
        /// </summary>
        /// <remarks>
        /// 試薬準備開始シーケンス動作を行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <returns>シーケンス名称</returns>
        protected String prepareReagentBottleSequence(String sequenceName, Object[] param)
        {
            SequenceCommData sequenceData = (SequenceCommData)param.Last();

            // 試薬準備確認コマンド送信
            SlaveCommCommand_0415 cmd0415 = new SlaveCommCommand_0415();
            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.Command1415;
            // コマンド送信
            this.pushSendQueueSingleSlave(cmd0415);
            // スレーブからのレスポンス待ち
            //sequenceData.Wait(((Int32)param[0]) * 60 * 1000);// 試薬ボトル設置可能になるまでの待ち時間(分をミリ秒に変換)
            // →レスポンスより先にタイムアウトしたら問題なので無限待ちにしました 2012.4.26 KUSU
            sequenceData.Wait(Timeout.Infinite);
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.TimeOut)
            {
                // 応答なし
                System.Diagnostics.Debug.WriteLine("試薬準備確認コマンドのレスポンスがありませんでした。");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                              "試薬準備確認コマンドのレスポンスがありませんでした。");
                // 17分のタイムアウトの場合、処理を続ける
            }
            else if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Abort)
            {
                // 中断なのでここで終了
                sequenceData.Dispose();
                sequenceData = null;
                return sequenceName;
            }

            sequenceData.Dispose();
            sequenceData = null;

            // 試薬交換用通知オブジェクト生成
            NotifyObjectSetReagent notifyObjectForBlinkButton = new NotifyObjectSetReagent(this.ModuleIdx, null);

            // メイン画面へ通知（試薬釦点滅開始）
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)(NotifyKind.BlinkReagentButton), notifyObjectForBlinkButton);

            // 試薬交換用通知オブジェクト生成
            NotifyObjectSetReagent notifyObjectForPrepareCheckResponse = new NotifyObjectSetReagent(this.ModuleIdx
                                                                                                  , FormSetReagent.ReagentChangeTargetKind.ReagentBottle);

            // メイン画面へ通知（試薬準備画面へ通知）
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)(NotifyKind.ReagentPrepareCheckResponse), notifyObjectForPrepareCheckResponse);

            return sequenceName;
        }

        /// <summary>
        /// 試薬準備完了シーケンス
        /// </summary>
        /// <remarks>
        /// 試薬準備完了シーケンス動作を行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <returns>シーケンス名称</returns>
        protected String prepareCompleteReagentBottleSequence(String sequenceName, Object[] param)
        {
            SequenceCommData sequenceData = (SequenceCommData)param.Last();

            // 試薬準備完了コマンド送信
            SlaveCommCommand_0417 cmd0417 = new SlaveCommCommand_0417();
            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.Command1417;
            // コマンド送信
            this.pushSendQueueSingleSlave(cmd0417);
            // スレーブからのレスポンス待ち
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.TimeOut)
            {
                // 応答なし
                System.Diagnostics.Debug.WriteLine("試薬準備完了コマンドのレスポンスがありませんでした。");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                              "試薬準備完了コマンドのレスポンスがありませんでした。");
                // タイムアウトしても試薬準備画面へ通知は必要。
            }
            else if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Abort)
            {
                // 中断なのでここで終了
                sequenceData.Dispose();
                sequenceData = null;
                return sequenceName;
            }

            // 受信結果を保持（0520コマンドで使用）
            Singleton<PublicMemory>.Instance.prepareResult = ((SlaveCommCommand_1417)sequenceData.RcvCommand).PrepareResultFlag;

            // 試薬交換用通知オブジェクト生成
            NotifyObjectSetReagent notifyObjectForPrepareCompleteResponse = new NotifyObjectSetReagent(this.ModuleIdx
                                                                                                     , FormSetReagent.ReagentChangeTargetKind.ReagentBottle);

            // メイン画面へ通知（試薬準備画面へ通知）
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)(NotifyKind.ReagentPrepareCompleteResponse), notifyObjectForPrepareCompleteResponse);

            sequenceData.Dispose();
            sequenceData = null;

            return sequenceName;
        }

        /// <summary>
        /// 準備開始シーケンス
        /// </summary>
        /// <remarks>
        /// 各準備開始シーケンス動作を行います。
        /// 対象：プレトリガ、トリガ、ケース(反応容器・サンプル分注チップ)
        /// paramの配列０…送信する準備開始コマンド
        /// paramの配列１…受信待ちするコマンド
        /// paramの配列２…交換対象
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <returns>シーケンス名称</returns>
        protected String prepareSequence(String sequenceName, Object[] param)
        {
            SequenceCommData sequenceData = (SequenceCommData)param.Last();

            // 準備開始コマンド送信
            CommCommand cmdPrepare = param[0] as CommCommand;
            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            if (param[1] != null)
            {
                sequenceData.WaitCommandKind = (CommandKind)param[1];
            }
            // コマンド送信
            this.pushSendQueueSingleSlave(cmdPrepare);

            if (param[1] != null)
            {
                // 待機コマンドがある場合のみレスポンス待ちを行う

                // スレーブからのレスポンス待ち
                sequenceData.Wait(RESP_WAIT_TIME);
                if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.TimeOut)
                {
                    // 応答なし
                    System.Diagnostics.Debug.WriteLine("準備開始コマンドのレスポンスがありませんでした。");
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                  "準備開始コマンドのレスポンスがありませんでした。");
                    sequenceData.Dispose();
                    sequenceData = null;
                    return sequenceName;
                }
                else if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Abort)
                {
                    // 中断なのでここで終了
                    sequenceData.Dispose();
                    sequenceData = null;
                    return sequenceName;
                }
            }

            // 試薬交換用通知オブジェクト生成
            NotifyObjectSetReagent notifyObjectForCommonPrepareStartResponse = new NotifyObjectSetReagent(this.ModuleIdx
                                                                                                        , (FormSetReagent.ReagentChangeTargetKind)param[2]);

            // メイン画面へ通知
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)(NotifyKind.CommonPrepareStartResponse), notifyObjectForCommonPrepareStartResponse);

            sequenceData.Dispose();
            sequenceData = null;
            return sequenceName;
        }

        /// <summary>
        /// 各準備完了シーケンス
        /// </summary>
        /// <remarks>
        /// 各準備完了シーケンス動作を行います。
        /// 対象：プレトリガ、トリガ、ケース(反応容器・サンプル分注チップ)
        /// paramの配列０…送信する準備完了コマンド
        /// paramの配列１…受信待ちするコマンド
        /// paramの配列２…メイン画面へ通知を送るかどうかのフラグ
        /// paramの配列３…交換対象の種類
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <returns>シーケンス名称</returns>
        protected String prepareCompleteSequence(String sequenceName, Object[] param)
        {
            SequenceCommData sequenceData = (SequenceCommData)param.Last();

            // 準備完了コマンド送信
            CommCommand cmdPrepareComplete = param[0] as CommCommand;
            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = (CommandKind)param[1];
            // コマンド送信
            this.pushSendQueueSingleSlave(cmdPrepareComplete);
            // スレーブからのレスポンス待ち
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.TimeOut)
            {
                // 応答なし
                System.Diagnostics.Debug.WriteLine("準備完了コマンドのレスポンスがありませんでした。");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, "準備完了コマンドのレスポンスがありませんでした。");
                // タイムアウトしても試薬準備画面へ通知は必要。
            }
            else if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Abort)
            {
                // 中断なのでここで終了
                sequenceData.Dispose();
                sequenceData = null;

                return sequenceName;
            }

            // 試薬交換用通知オブジェクト生成
            NotifyObjectSetReagent notifyObjectForPrepareCompleteResponse = new NotifyObjectSetReagent(this.ModuleIdx
                                                                                                     , (FormSetReagent.ReagentChangeTargetKind)param[2]);

            // メイン画面へ通知（試薬準備画面へ通知）
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)(NotifyKind.ReagentPrepareCompleteResponse), notifyObjectForPrepareCompleteResponse);
            sequenceData.Dispose();
            sequenceData = null;

            return sequenceName;
        }

        /// <summary>
        /// 希釈液準備開始シーケンス
        /// </summary>
        /// <remarks>
        /// 希釈液準備開始シーケンス動作を行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <returns>シーケンス名称</returns>
        protected String prepareDiluentSequence(String sequenceName, Object[] param)
        {
            SequenceCommData sequenceData = (SequenceCommData)param.Last();

            // 希釈液準備確認コマンド送信
            SlaveCommCommand_0418 cmd0418 = new SlaveCommCommand_0418();
            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.Command1418;
            // コマンド送信
            this.pushSendQueueSingleSlave(cmd0418);
            // スレーブからのレスポンス待ち
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.TimeOut)
            {
                // 応答なし
                System.Diagnostics.Debug.WriteLine("希釈液準備確認コマンドのレスポンスがありませんでした。");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, "希釈液準備確認コマンドのレスポンスがありませんでした。");
                // タイムアウトしても試薬準備画面へ通知は必要。
            }
            else if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Abort)
            {
                // 中断なのでここで終了
                sequenceData.Dispose();
                sequenceData = null;
                return sequenceName;
            }

            // 試薬交換用通知オブジェクト生成
            NotifyObjectSetReagent notifyObjectForPrepareCheckResponse = new NotifyObjectSetReagent(this.ModuleIdx
                                                                                                  , FormSetReagent.ReagentChangeTargetKind.Diluent);

            // メイン画面へ通知（試薬準備画面へ通知）
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)(NotifyKind.ReagentPrepareCheckResponse), notifyObjectForPrepareCheckResponse);

            sequenceData.Dispose();
            sequenceData = null;

            // 希釈液準備開始コマンド送信
            sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            SlaveCommCommand_0419 cmd0419 = new SlaveCommCommand_0419();
            sequenceData.WaitCommandKind = CommandKind.Command1419;
            this.pushSendQueueSingleSlave(cmd0419);
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.TimeOut)
            {
                // 応答なし
                System.Diagnostics.Debug.WriteLine("希釈液準備開始コマンドのレスポンスがありませんでした。");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                              "希釈液準備開始コマンドのレスポンスがありませんでした。");
                // タイムアウトしても試薬準備画面へ通知は必要。
            }
            else if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Abort)
            {
                // 中断なのでここで終了
                sequenceData.Dispose();
                sequenceData = null;
                return sequenceName;
            }

            // 試薬交換用通知オブジェクト生成
            NotifyObjectSetReagent notifyObjectForCommonPrepareStartResponse = new NotifyObjectSetReagent(this.ModuleIdx
                                                                                                        , FormSetReagent.ReagentChangeTargetKind.Diluent);

            // メイン画面へ通知
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)(NotifyKind.CommonPrepareStartResponse), notifyObjectForCommonPrepareStartResponse);

            sequenceData.Dispose();
            sequenceData = null;
            return sequenceName;
        }

        /// <summary>
        /// 交換中断シーケンス
        /// </summary>
        /// <remarks>
        /// 交換中断シーケンス動作を行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <returns>シーケンス名称</returns>
        protected String exchangeCancelSequence(String sequenceName, Object[] param)
        {
            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            // 準備中断コマンド送信
            SlaveCommCommand_0476 cmd0476 = new SlaveCommCommand_0476();
            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.Command1476;
            // コマンド送信
            this.pushSendQueueSingleSlave(cmd0476);
            // スレーブからのレスポンス待ち
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.TimeOut)
            {
                // 応答なし
                System.Diagnostics.Debug.WriteLine("準備中断コマンドのレスポンスがありませんでした。");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, "準備中断コマンドのレスポンスがありませんでした。");
                // タイムアウトしても試薬準備画面へ通知は必要。
            }
            else if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Abort)
            {
                // 中断なのでここで終了
                sequenceData.Dispose();
                sequenceData = null;
                return sequenceName;
            }

            sequenceData.Dispose();
            sequenceData = null;

            // 試薬交換用通知オブジェクト生成
            NotifyObjectSetReagent notifyObjectForPrepareCompleteResponse = new NotifyObjectSetReagent(this.ModuleIdx
                                                                                                     , FormSetReagent.ReagentChangeTargetKind.All);

            // メイン画面へ通知（試薬準備画面へ通知）
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)(NotifyKind.ReagentPrepareCompleteResponse), notifyObjectForPrepareCompleteResponse);

            return sequenceName;
        }

        /// <summary>
        /// 希釈液準備完了シーケンス
        /// </summary>
        /// <remarks>
        /// 希釈液準備完了シーケンス動作を行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <returns>シーケンス名称</returns>
        protected String prepareCompleteDiluentSequence(String sequenceName, Object[] param)
        {
            SequenceCommData sequenceData = (SequenceCommData)param.Last();

            // 希釈液準備完了コマンド送信
            SlaveCommCommand_0420 cmd0420 = new SlaveCommCommand_0420();
            // 残量
            cmd0420.Remain = (Int32)param[0];
            // ロット番号
            cmd0420.LotNumber = ((Int32)param[1]).ToString("00000000");
            // 有効期限
            cmd0420.TermOfUse = (DateTime)param[2];
            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.Command1420;
            // コマンド送信
            this.pushSendQueueSingleSlave(cmd0420);
            // スレーブからのレスポンス待ち
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.TimeOut)
            {
                // 応答なし
                System.Diagnostics.Debug.WriteLine("希釈液準備完了コマンドのレスポンスがありませんでした。");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, "希釈液準備完了コマンドのレスポンスがありませんでした。");
                // タイムアウトしても試薬準備画面へ通知は必要。
            }
            else if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Abort)
            {
                // 中断なのでここで終了
                sequenceData.Dispose();
                sequenceData = null;

                return sequenceName;
            }

            // 試薬交換用通知オブジェクト生成
            NotifyObjectSetReagent notifyObjectForPrepareCompleteResponse = new NotifyObjectSetReagent(this.ModuleIdx
                                                                                                     , FormSetReagent.ReagentChangeTargetKind.Diluent);

            // メイン画面へ通知（試薬準備画面へ通知）
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)(NotifyKind.ReagentPrepareCompleteResponse), notifyObjectForPrepareCompleteResponse);
            sequenceData.Dispose();
            sequenceData = null;

            return sequenceName;
        }

        /// <summary>
        /// 試薬残量変更シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 試薬残量変更シーケンス動作を行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <returns>シーケンス名称</returns>
        protected String changeReagentRemainSequence(String sequenceName, Object[] param)
        {
            SequenceCommData sequenceData = (SequenceCommData)param.Last();

            // 試薬交換用通知オブジェクト生成
            NotifyObjectSetReagent notifyObjectForRemainResponse = notifyObjectForRemainResponse = new NotifyObjectSetReagent(this.ModuleIdx, true);

            // 試薬残量変更確認コマンド送信
            SlaveCommCommand_0427 cmd0427 = new SlaveCommCommand_0427();
            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.Command1427;
            // コマンド送信
            this.pushSendQueueSingleSlave(cmd0427);
            // スレーブからのレスポンス待ち
            //sequenceData.Wait(RESP_WAIT_TIME);
            // →レスポンスより先にタイムアウトしたら問題なので無限待ちにしました 2012.4.26 KUSU
            sequenceData.Wait(Timeout.Infinite);
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.TimeOut)
            {
                // 応答なし
                System.Diagnostics.Debug.WriteLine("試薬残量変更確認コマンドのレスポンスがありませんでした。");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog
                                                         , Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                                                         , CarisXLogInfoBaseExtention.Empty
                                                         , "There was no response to the reagent remaining amount change confirmation command.");

                // 試薬交換用通知オブジェクト情報をfalseに変更
                notifyObjectForRemainResponse.ObjectValue = false;

                // タイムアウトしても試薬準備画面へ通知は必要。
                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)(NotifyKind.ChangeReagentRemainResponse), notifyObjectForRemainResponse);
                sequenceData.Dispose();
                sequenceData = null;
                return sequenceName;
            }
            else if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Abort)
            {
                // 中断なのでここで終了
                sequenceData.Dispose();
                sequenceData = null;
                return sequenceName;
            }

            // 試薬交換用通知オブジェクト生成
            NotifyObjectSetReagent notifyObjectForBlinkButton = new NotifyObjectSetReagent(this.ModuleIdx, null);

            // メイン画面へ通知（試薬釦点滅開始）
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)(NotifyKind.BlinkReagentButton), notifyObjectForBlinkButton);

            // メイン画面へ通知（試薬準備画面へ通知）
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)(NotifyKind.ChangeReagentRemainResponse), notifyObjectForRemainResponse);
            sequenceData.Dispose();
            sequenceData = null;

            return sequenceName;
        }

        /// <summary>
        /// 汎用残量変更シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 汎用残量変更シーケンス動作を行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <returns>シーケンス名称</returns>
        protected String changeCommonRemainSequence(String sequenceName, Object[] param)
        {
            SequenceCommData sequenceData = (SequenceCommData)param.Last();

            // 残量変更コマンド送信
            CommCommand cmdchangeCommonRemain = param[0] as CommCommand;
            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = (CommandKind)param[1];
            // コマンド送信
            this.pushSendQueueSingleSlave(cmdchangeCommonRemain);
            // スレーブからのレスポンス待ち（試薬残量変更コマンドに合わせて無限待ち）
            sequenceData.Wait(Timeout.Infinite);
            //タイムアウトが発生しないので、タイムアウト関連の処理は削除
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Abort)
            {
                // 中断なのでここで終了
                sequenceData.Dispose();
                sequenceData = null;

                return sequenceName;
            }

            // 試薬残量情報を文字列にカンマ区切りで連結
            // （連結情報 = 試薬種別, ポート番号, 残量, ロット番号, シリアル番号）
            String cmdText = String.Join(",", (int)((ReagentKind)param[2]), param[3], param[4], param[5], param[6]);

            // 試薬交換用通知オブジェクト生成
            NotifyObjectSetReagent notifyObjectForCommonRemainResponse = new NotifyObjectSetReagent(this.ModuleIdx, cmdText);

            // メイン画面へ通知（試薬準備画面へ通知）
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)(NotifyKind.ChangeCommonRemainResponse), notifyObjectForCommonRemainResponse);
            sequenceData.Dispose();
            sequenceData = null;

            return sequenceName;
        }

        /// <summary>
        /// リンス処理シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// リンス処理シーケンス呼び出しします
        /// </remarks>
        /// <param name="sequenceName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected String rinsingSequence(String sequenceName, Object[] param)
        {
            SequenceSyncObject sequenceSync = (SequenceSyncObject)param.Last();
            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            SlaveCommCommand_0410 cmd0410 = new SlaveCommCommand_0410();
            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.Command1410;
            // コマンド送信
            this.pushSendQueueSingleSlave(cmd0410);
            // スレーブから応答を待機
            sequenceData.Wait(Timeout.Infinite);
            if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
            {
                sequenceSync.Status = SequenceSyncObject.SequenceStatus.Error;
                System.Diagnostics.Debug.WriteLine("リンス処理タイムアウト。");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, "リンス処理タイムアウト。");
            }
            else
            {
                sequenceSync.Status = SequenceSyncObject.SequenceStatus.Success;
                sequenceSync.SequenceResultData = sequenceData.RcvCommand;
            }

            sequenceSync.SetEnd();
            //free the event
            sequenceData.Dispose();
            sequenceData = null;
            //// 通知
            //Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.RinsingSequence, sequenceData);

            return sequenceName;
        }

        /// <summary>
        /// シリンジエージング
        /// </summary>
        /// <remarks>
        /// シリンジエージングします
        /// </remarks>
        /// <param name="sequenceName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected String syringeAgingSequence(String sequenceName, Object[] param)
        {
            SequenceSyncObject syncObject = (SequenceSyncObject)param.Last();
            syncObject.Status = SequenceSyncObject.SequenceStatus.Wait;

            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 待機時間超過
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                syncObject.SetEnd();
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            SlaveCommCommand_0483 cmd0483 = new SlaveCommCommand_0483();
            cmd0483.NoCyMotor = (Int32)param[0];
            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.Command1483;
            // コマンド送信
            this.pushSendQueueSingleSlave(cmd0483);
            // スレーブから応答を待機
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Success)
            {
                syncObject.Status = SequenceSyncObject.SequenceStatus.Success;
                syncObject.SequenceResultData = sequenceData.RcvCommand;
            }
            else
            {
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
            }

            syncObject.SetEnd();
            //free the Event
            sequenceData.Dispose();
            sequenceData = null;

            return sequenceName;
        }

        /// <summary>
        /// 保冷庫移動コマンドシーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 保冷庫移動コマンドシーケンス呼び出しします
        /// </remarks>
        /// <param name="sequenceName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected String ReagentCoolerMoveSequence(String sequenceName, Object[] param)
        {
            SequenceSyncObject syncObject = (SequenceSyncObject)param.Last();
            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            SlaveCommCommand_0487 cmd0487 = new SlaveCommCommand_0487();
            cmd0487.PortNumber = (Int32)param[0];
            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.Command1487;
            // コマンド送信
            this.pushSendQueueSingleSlave(cmd0487);
            // スレーブから応答を待機
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
            {
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                System.Diagnostics.Debug.WriteLine("试剂仓move command time-out.");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, "试剂仓move command time-out.");
            }
            else
            {
                syncObject.Status = SequenceSyncObject.SequenceStatus.Success;
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("试剂仓move command OK."));

            }
            if ((Int32)param[2] == 1)
            {
                // メイン画面へ通知（試薬準備画面へ通知）
                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)(NotifyKind.ReagentCoolerMoveResponse), param[1]);
            }
            //syncObject.SetEnd();

            //free the Event
            sequenceData.Dispose();
            sequenceData = null;

            return sequenceName;
        }

        #region 廃液タンク交換開始
        /// <summary>
        /// 廃液タンク交換開始シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 廃液タンク交換開始シーケンス呼び出しします
        /// </remarks>
        /// <param name="args"></param>
        /// <returns></returns>
        public SequenceSyncObject StartReplaceWasteTankSequence(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceSyncObject syncObject = new SequenceSyncObject();

            // 引数の領域を拡張し、末尾に同期操作オブジェクトを加える
            this.pushSequenceSyncObject(ref args, syncObject);
            SequenceDelegate sequence = new SequenceDelegate(startReplaceWasteTankSequence);
            this.startSequence(methodName, args, sequence);

            return syncObject;
        }

        /// <summary>
        /// 廃液タンク交換開始シーケンス
        /// </summary>
        /// <remarks>
        /// 廃液タンク交換開始シーケンス動作を行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <returns>シーケンス名称</returns>
        protected String startReplaceWasteTankSequence(String sequenceName, Object[] param)
        {
            SequenceSyncObject syncObject = (SequenceSyncObject)param.Last();
            syncObject.Status = SequenceSyncObject.SequenceStatus.Wait;

            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 待機時間超過
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                syncObject.SetEnd();
                return sequenceName;
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            // 廃液ボトルセット開始コマンド送信
            SlaveCommCommand_0435 cmd0435 = new SlaveCommCommand_0435();
            cmd0435.tankBufferKind = TankBufferKind.Tank;

            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.Command1435;

            // コマンド送信
            this.pushSendQueueSingleSlave(cmd0435);

            // スレーブからのレスポンス待ち
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Success)
            {
                syncObject.Status = SequenceSyncObject.SequenceStatus.Success;
                syncObject.SequenceResultData = sequenceData.RcvCommand;
            }
            else
            {
                // 応答なし
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                System.Diagnostics.Debug.WriteLine("廃液セット開始コマンドのレスポンスがありませんでした。");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, "廃液セット開始コマンドのレスポンスがありませんでした。");
            }

            sequenceData.Dispose();
            sequenceData = null;

            syncObject.SetEnd();
            return sequenceName;
        }
        #endregion

        #region 廃液タンク交換中止
        /// <summary>
        /// 廃液タンク交換中止シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 廃液タンク交換中止シーケンス呼び出しします
        /// </remarks>
        /// <param name="args"></param>
        /// <returns></returns>
        public SequenceSyncObject StopReplaceWasteTankSequence(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceSyncObject syncObject = new SequenceSyncObject();

            // 引数の領域を拡張し、末尾に同期操作オブジェクトを加える
            this.pushSequenceSyncObject(ref args, syncObject);
            SequenceDelegate sequence = new SequenceDelegate(endReplaceWasteTankSequence);
            this.startSequence(methodName, args, sequence);

            return syncObject;
        }

        /// <summary>
        /// 廃液タンク交換中止シーケンス
        /// </summary>
        /// <remarks>
        /// 廃液タンク交換中止シーケンス動作を行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <returns>シーケンス名称</returns>
        protected String endReplaceWasteTankSequence(String sequenceName, Object[] param)
        {
            SequenceSyncObject syncObject = (SequenceSyncObject)param.Last();
            syncObject.Status = SequenceSyncObject.SequenceStatus.Wait;

            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 待機時間超過
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                syncObject.SetEnd();
                return sequenceName;
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            // 廃液ボトルセット完了コマンド送信
            SlaveCommCommand_0436 cmd0436 = new SlaveCommCommand_0436();
            cmd0436.tankBufferKind = TankBufferKind.Tank;

            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.Command1436;

            // コマンド送信
            this.pushSendQueueSingleSlave(cmd0436);

            // スレーブからのレスポンス待ち
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Success)
            {
                syncObject.Status = SequenceSyncObject.SequenceStatus.Success;
                syncObject.SequenceResultData = sequenceData.RcvCommand;
            }
            else
            {
                // 応答なし
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                System.Diagnostics.Debug.WriteLine("廃液セット完了コマンドのレスポンスがありませんでした。");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, "廃液セット完了コマンドのレスポンスがありませんでした。");
            }

            sequenceData.Dispose();
            sequenceData = null;

            syncObject.SetEnd();
            return sequenceName;
        }
        #endregion

        #region モーター初期化（モジュール）

        /// <summary>
        /// モーター初期化シーケンス（モジュール）
        /// </summary>
        /// <remarks>
        /// モーター初期化の問合せをモジュールに対して実施します。
        /// </remarks>
        public void MotorInitializeModuleSequence()
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceDelegate sequence = new SequenceDelegate(this.motorInitializeModuleSequence);
            this.startSequence(methodName, null, sequence);
        }

        /// <summary>
        /// モーター初期化シーケンス（モジュール）
        /// </summary>
        /// <remarks>
        /// モジュールに対してモーター初期化の問合せを行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <param name="param">不使用</param>
        /// <returns>シーケンス名称</returns>
        protected String motorInitializeModuleSequence(String sequenceName, Object[] param)
        {
            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            // モーター初期化コマンド送信
            SlaveCommCommand_0406 cmd0406 = new SlaveCommCommand_0406();
            cmd0406.MotorNumber = 0;

            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.Command1406;

            // コマンド送信
            this.pushSendQueueSingleSlave(cmd0406);

            // レスポンス待ち
            sequenceData.Wait(RESP_WAIT_TIME);

            MotorInitCompStatusKind motorInitCompStatusKind = MotorInitCompStatusKind.Init;

            switch (this.ModuleIdx)
            {
                case (int)ModuleIndex.Module1:
                    motorInitCompStatusKind = MotorInitCompStatusKind.Module1;
                    break;
                case (int)ModuleIndex.Module2:
                    motorInitCompStatusKind = MotorInitCompStatusKind.Module2;
                    break;
                case (int)ModuleIndex.Module3:
                    motorInitCompStatusKind = MotorInitCompStatusKind.Module3;
                    break;
                case (int)ModuleIndex.Module4:
                    motorInitCompStatusKind = MotorInitCompStatusKind.Module4;
                    break;
            }

            switch (sequenceData.WaitSuccess)
            {
                case SequenceCommData.WaitResult.Success:
                    // 正常終了
                    motorInitCompStatusKind = motorInitCompStatusKind | MotorInitCompStatusKind.Completed;
                    break;
                default:
                    // 正常終了以外
                    motorInitCompStatusKind = motorInitCompStatusKind | MotorInitCompStatusKind.TimeOut;
                    break;
            }

            // モーター初期化完了通知を送信
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.MotorInitializeCompleted, motorInitCompStatusKind);

            sequenceData.Dispose();
            sequenceData = null;

            return sequenceName;
        }

        #endregion

        #region モーター初期化（ラック搬送）

        /// <summary>
        /// モーター初期化シーケンス（ラック搬送）
        /// </summary>
        /// <remarks>
        /// モーター初期化の問合せをラック搬送に対して実施します。
        /// </remarks>
        public void MotorInitializeRackSequence()
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceDelegate sequence = new SequenceDelegate(this.motorInitializeRackSequence);
            this.startSequence(methodName, null, sequence);
        }

        /// <summary>
        /// モーター初期化シーケンス（モジュール）
        /// </summary>
        /// <remarks>
        /// モジュールに対してモーター初期化の問合せを行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <param name="param">不使用</param>
        /// <returns>シーケンス名称</returns>
        protected String motorInitializeRackSequence(String sequenceName, Object[] param)
        {
            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            // モーター初期化コマンド送信
            RackTransferCommCommand_0006 cmd0006 = new RackTransferCommCommand_0006();
            cmd0006.MotorNumber = 0;

            // 待機コマンドの設定（コマンド送信後の応答を待つ場合は、必ず送信前に待機コマンドを設定する）
            sequenceData.WaitCommandKind = CommandKind.RackTransferCommand1006;

            // コマンド送信
            Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0006);

            // レスポンス待ち
            sequenceData.Wait(RESP_WAIT_TIME);

            MotorInitCompStatusKind motorInitCompStatusKind = MotorInitCompStatusKind.Rack;

            switch (sequenceData.WaitSuccess)
            {
                case SequenceCommData.WaitResult.Success:
                    // 正常終了
                    motorInitCompStatusKind = motorInitCompStatusKind | MotorInitCompStatusKind.Completed;
                    break;
                default:
                    // 正常終了以外
                    motorInitCompStatusKind = motorInitCompStatusKind | MotorInitCompStatusKind.TimeOut;
                    break;
            }

            // モーター初期化完了通知を送信
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.MotorInitializeCompleted, motorInitCompStatusKind);

            sequenceData.Dispose();
            sequenceData = null;

            return sequenceName;
        }

        #endregion

        #region コンフィグパラメータ送信（モジュール）

        /// <summary>
        /// コンフィグパラメータ送信シーケンス呼び出し（モジュール）
        /// </summary>
        /// <remarks>
        /// コンフィグパラメータ送信シーケンスを呼び出します（モジュール）
        /// </remarks>
        /// <param name="args">引数（未使用）</param>
        /// <returns>シーケンス同期オブジェクト</returns>
        public SequenceSyncObject SendModuleConfigParameter(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceSyncObject syncObject = new SequenceSyncObject();

            // 引数の領域を拡張し、末尾に同期操作オブジェクトを加える
            this.pushSequenceSyncObject(ref args, syncObject);
            SequenceDelegate sequence = new SequenceDelegate(sendModuleConfigParameter);
            this.startSequence(methodName, args, sequence);

            return syncObject;
        }

        /// <summary>
        /// コンフィグパラメータ送信シーケンス（モジュール）
        /// </summary>
        /// <remarks>
        /// モジュールのコンフィグパラメータ送信シーケンス動作を行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称（内部利用）</param>
        /// <param name="param">パラメータ</param>
        /// <returns>シーケンス名称（内部利用）</returns>
        protected String sendModuleConfigParameter(String sequenceName, Object[] param)
        {
            CarisXConfigParameter.ModuleConfig config = (CarisXConfigParameter.ModuleConfig)param.First();
            SequenceSyncObject syncObject = (SequenceSyncObject)param.Last();
            syncObject.Status = SequenceSyncObject.SequenceStatus.Wait;

            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 待機時間超過
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                syncObject.SetEnd();
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            // ケース搬送ユニットパラメータコマンド
            SlaveCommCommand_0448 cmd0448 = new SlaveCommCommand_0448();
            cmd0448 = config.tipCellCaseTransferUnitConfigParam;
            sequenceData.WaitCommandKind = CommandKind.Command1448;
            this.pushSendQueueSingleSlave(cmd0448);
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
            {
                // 応答なし
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;

            }

            if (syncObject.Status != SequenceSyncObject.SequenceStatus.Error)
            {
                // 試薬保冷庫ユニットパラメータコマンド
                SlaveCommCommand_0449 cmd0449 = new SlaveCommCommand_0449();
                cmd0449 = config.reagentStorageUnitConfigParam;
                sequenceData.WaitCommandKind = CommandKind.Command1449;
                this.pushSendQueueSingleSlave(cmd0449);
                sequenceData.Wait(RESP_WAIT_TIME);
                if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                {
                    // 応答なし
                    syncObject.Status = SequenceSyncObject.SequenceStatus.Error;

                }
            }

            if (syncObject.Status != SequenceSyncObject.SequenceStatus.Error)
            {
                // スタットユニットパラメータコマンド
                SlaveCommCommand_0450 cmd0450 = new SlaveCommCommand_0450();
                cmd0450 = config.sTATUnitConfigParam;
                sequenceData.WaitCommandKind = CommandKind.Command1450;
                this.pushSendQueueSingleSlave(cmd0450);
                sequenceData.Wait(RESP_WAIT_TIME);
                if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                {
                    // 応答なし
                    syncObject.Status = SequenceSyncObject.SequenceStatus.Error;

                }
            }

            if (syncObject.Status != SequenceSyncObject.SequenceStatus.Error)
            {
                // サンプル分注ユニットパラメータコマンド
                SlaveCommCommand_0451 cmd0451 = new SlaveCommCommand_0451();
                cmd0451 = config.sampleDispenseConfigParam;
                sequenceData.WaitCommandKind = CommandKind.Command1451;
                this.pushSendQueueSingleSlave(cmd0451);
                sequenceData.Wait(RESP_WAIT_TIME);
                if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                {
                    // 応答なし
                    syncObject.Status = SequenceSyncObject.SequenceStatus.Error;

                }
            }

            if (syncObject.Status != SequenceSyncObject.SequenceStatus.Error)
            {
                // 反応容器搬送ユニットパラメータコマンド
                SlaveCommCommand_0452 cmd0452 = new SlaveCommCommand_0452();
                cmd0452 = config.reactionCellTransferUnitConfigParam;
                sequenceData.WaitCommandKind = CommandKind.Command1452;
                this.pushSendQueueSingleSlave(cmd0452);
                sequenceData.Wait(RESP_WAIT_TIME);
                if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                {
                    // 応答なし
                    syncObject.Status = SequenceSyncObject.SequenceStatus.Error;

                }
            }

            if (syncObject.Status != SequenceSyncObject.SequenceStatus.Error)
            {
                // 反応テーブルユニットパラメータコマンド
                SlaveCommCommand_0453 cmd0453 = new SlaveCommCommand_0453();
                cmd0453 = config.reactionTableUnitConfigParam;
                sequenceData.WaitCommandKind = CommandKind.Command1453;
                this.pushSendQueueSingleSlave(cmd0453);
                sequenceData.Wait(RESP_WAIT_TIME);
                if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                {
                    // 応答なし
                    syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                }
            }

            if (syncObject.Status != SequenceSyncObject.SequenceStatus.Error)
            {
                // BFテーブルユニットパラメータコマンド
                SlaveCommCommand_0454 cmd0454 = new SlaveCommCommand_0454();
                cmd0454 = config.bFTableUnitConfigParam;
                sequenceData.WaitCommandKind = CommandKind.Command1454;
                this.pushSendQueueSingleSlave(cmd0454);
                sequenceData.Wait(RESP_WAIT_TIME);
                if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                {
                    // 応答なし
                    syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                }
            }

            if (syncObject.Status != SequenceSyncObject.SequenceStatus.Error)
            {
                // トラベラーユニットパラメータコマンド
                SlaveCommCommand_0455 cmd0455 = new SlaveCommCommand_0455();
                cmd0455 = config.travelerAndDisposalUnitConfigParam;
                sequenceData.WaitCommandKind = CommandKind.Command1455;
                this.pushSendQueueSingleSlave(cmd0455);
                sequenceData.Wait(RESP_WAIT_TIME);
                if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                {
                    // 応答なし
                    syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                }
            }

            if (syncObject.Status != SequenceSyncObject.SequenceStatus.Error)
            {
                // R1分注ユニットパラメータコマンド
                SlaveCommCommand_0456 cmd0456 = new SlaveCommCommand_0456();
                cmd0456 = config.r1DispenseUnitConfigParam;
                sequenceData.WaitCommandKind = CommandKind.Command1456;
                this.pushSendQueueSingleSlave(cmd0456);
                sequenceData.Wait(RESP_WAIT_TIME);
                if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                {
                    // 応答なし
                    syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                }
            }

            if (syncObject.Status != SequenceSyncObject.SequenceStatus.Error)
            {
                // R2分注ユニットパラメータコマンド 
                SlaveCommCommand_0457 cmd0457 = new SlaveCommCommand_0457();
                cmd0457 = config.r2DispenseUnitConfigParam;
                sequenceData.WaitCommandKind = CommandKind.Command1457;
                this.pushSendQueueSingleSlave(cmd0457);
                sequenceData.Wait(RESP_WAIT_TIME);
                if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                {
                    // 応答なし
                    syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                }
            }

            if (syncObject.Status != SequenceSyncObject.SequenceStatus.Error)
            {
                // B/F1ユニットパラメータコマンド
                SlaveCommCommand_0458 cmd0458 = new SlaveCommCommand_0458();
                cmd0458 = config.bF1UnitConfigParam;
                sequenceData.WaitCommandKind = CommandKind.Command1458;
                this.pushSendQueueSingleSlave(cmd0458);
                sequenceData.Wait(RESP_WAIT_TIME);
                if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                {
                    // 応答なし
                    syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                }
            }

            if (syncObject.Status != SequenceSyncObject.SequenceStatus.Error)
            {
                // B/F2ユニットパラメータコマンド
                SlaveCommCommand_0459 cmd0459 = new SlaveCommCommand_0459();
                cmd0459 = config.bF2UnitConfigParam;
                sequenceData.WaitCommandKind = CommandKind.Command1459;
                this.pushSendQueueSingleSlave(cmd0459);
                sequenceData.Wait(RESP_WAIT_TIME);
                if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                {
                    // 応答なし
                    syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                }
            }

            if (syncObject.Status != SequenceSyncObject.SequenceStatus.Error)
            {
                // 希釈分注ユニットパラメータコマンド
                SlaveCommCommand_0460 cmd0460 = new SlaveCommCommand_0460();
                cmd0460 = config.dilutionDispenseUnitConfigParam;
                sequenceData.WaitCommandKind = CommandKind.Command1460;
                this.pushSendQueueSingleSlave(cmd0460);
                sequenceData.Wait(RESP_WAIT_TIME);
                if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                {
                    // 応答なし
                    syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                }
            }

            if (syncObject.Status != SequenceSyncObject.SequenceStatus.Error)
            {
                // プレトリガ分注ユニットパラメータコマンド 
                SlaveCommCommand_0461 cmd0461 = new SlaveCommCommand_0461();
                cmd0461 = config.pretriggerUnitConfigParam;
                sequenceData.WaitCommandKind = CommandKind.Command1461;
                this.pushSendQueueSingleSlave(cmd0461);
                sequenceData.Wait(RESP_WAIT_TIME);
                if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                {
                    // 応答なし
                    syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                }
            }

            if (syncObject.Status != SequenceSyncObject.SequenceStatus.Error)
            {
                // トリガ分注測光ユニットパラメータコマンド 
                SlaveCommCommand_0462 cmd0462 = new SlaveCommCommand_0462();
                cmd0462 = config.triggerUnitConfigParam;
                sequenceData.WaitCommandKind = CommandKind.Command1462;
                this.pushSendQueueSingleSlave(cmd0462);
                sequenceData.Wait(RESP_WAIT_TIME);
                if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                {
                    // 応答なし
                    syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                }
            }

            if (syncObject.Status != SequenceSyncObject.SequenceStatus.Error)
            {
                // 流体配管部ユニットパラメータコマンド 
                SlaveCommCommand_0463 cmd0463 = new SlaveCommCommand_0463();
                cmd0463 = config.fluidAndPipingUnitConfigParam;
                sequenceData.WaitCommandKind = CommandKind.Command1463;
                this.pushSendQueueSingleSlave(cmd0463);
                sequenceData.Wait(RESP_WAIT_TIME);
                if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
                {
                    // 応答なし
                    syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                }
            }

            if (syncObject.Status != SequenceSyncObject.SequenceStatus.Error)
            {
                syncObject.Status = SequenceSyncObject.SequenceStatus.Success;
            }

            //Free the event
            sequenceData.Dispose();
            sequenceData = null;

            syncObject.SetEnd();
            return sequenceName;
        }

        #endregion

        #region コンフィグパラメータ送信（ラック搬送）

        /// <summary>
        /// コンフィグパラメータ送信シーケンス呼び出し（ラック搬送）
        /// </summary>
        /// <remarks>
        /// コンフィグパラメータ送信シーケンスを呼び出します（ラック搬送）
        /// </remarks>
        /// <param name="args">引数（未使用）</param>
        /// <returns>シーケンス同期オブジェクト</returns>
        public SequenceSyncObject SendRackConfigParameter(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceSyncObject syncObject = new SequenceSyncObject();

            // 引数の領域を拡張し、末尾に同期操作オブジェクトを加える
            this.pushSequenceSyncObject(ref args, syncObject);
            SequenceDelegate sequence = new SequenceDelegate(sendRackConfigParameter);
            this.startSequence(methodName, args, sequence);

            return syncObject;
        }

        /// <summary>
        /// コンフィグパラメータ送信シーケンス（ラック搬送）
        /// </summary>
        /// <remarks>
        /// ラック搬送のコンフィグパラメータ送信シーケンス動作を行います。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称（内部利用）</param>
        /// <param name="param">パラメータ</param>
        /// <returns>シーケンス名称（内部利用）</returns>
        protected String sendRackConfigParameter(String sequenceName, Object[] param)
        {
            CarisXConfigParameter.RackConfig config = (CarisXConfigParameter.RackConfig)param.First();
            SequenceSyncObject syncObject = (SequenceSyncObject)param.Last();
            syncObject.Status = SequenceSyncObject.SequenceStatus.Wait;

            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 待機時間超過
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
                syncObject.SetEnd();
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);

            // ラックユニットパラメータコマンド
            RackTransferCommCommand_0047 cmd0047 = new RackTransferCommCommand_0047();
            cmd0047 = config.rackTransferUnitConfigParam;
            sequenceData.WaitCommandKind = CommandKind.RackTransferCommand1047;
            Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0047);
            sequenceData.Wait(RESP_WAIT_TIME);
            if (sequenceData.WaitSuccess != SequenceCommData.WaitResult.Success)
            {
                // 応答なし
                syncObject.Status = SequenceSyncObject.SequenceStatus.Error;
            }

            if (syncObject.Status != SequenceSyncObject.SequenceStatus.Error)
            {
                syncObject.Status = SequenceSyncObject.SequenceStatus.Success;
            }

            //Free the event
            sequenceData.Dispose();
            sequenceData = null;

            syncObject.SetEnd();
            return sequenceName;
        }

        #endregion

        #region 自動スタートアップ（ラック搬送）
        /// <summary>
        /// 自動起動（ラック搬送）シーケンス呼び出し
        /// </summary>
        /// <remarks>
        /// 自動起動（ラック搬送）シーケンス呼び出しします
        /// </remarks>
        /// <param name="args"></param>
        public void AutoStartupRackTransfer(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceDelegate sequence = new SequenceDelegate(this.autoStartUpRackTransfer);
            this.startSequence(methodName, args, sequence);
        }

        /// <summary>
        /// 自動起動（ラック搬送）
        /// </summary>
        /// <remarks>
        /// 自動起動（ラック搬送）します
        /// </remarks>
        /// <param name="sequenceName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected String autoStartUpRackTransfer(String sequenceName, Object[] param)
        {
            Boolean seqStatus = false;

            using (SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo))
            {
                // シャットダウンコマンドを投げて待機
                RackTransferCommCommand_0003 cmd0003 = new RackTransferCommCommand_0003();
                sequenceData.WaitCommandKind = CommandKind.RackTransferCommand1003;
                Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0003);
                sequenceData.Wait(Timeout.Infinite);
                if (sequenceData.WaitSuccess == SequenceCommData.WaitResult.Success)
                    seqStatus = true;

                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.AutoStartupStartRack, seqStatus);
            }

            return sequenceName;
        }

        #endregion

        #region ホスト通信用シーケンス
        /// <summary>
        /// ホスト通信用シーケンス
        /// </summary>
        /// <remarks>
        /// ホスト通信を開始します。
        /// </remarks>
        /// <param name="args">パラメータ（不使用）</param>
        public void HostCommunicationSequence(params Object[] args)
        {
            String methodName = MethodBase.GetCurrentMethod().Name;
            SequenceDelegate sequence = new SequenceDelegate(this.hostCommunicationSequence);
            this.startSequence(methodName, args, sequence);
        }

        /// <summary>
        /// ホスト通信用シーケンス用メソッド
        /// </summary>
        /// <param name="sequenceName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected String hostCommunicationSequence(String sequenceName, Object[] param)
        {
            HostCommunicationSequencePattern seqptn;

            //paramにHostCommunicationSequencePatternが設定されていない場合は処理しない
            if (param.Length < 1 || !Enum.TryParse(param[0].ToString(), out seqptn))
            {
                return "CANCEL";
            }

            //パラメータに設定されている初期シーケンス
            seqptn = (HostCommunicationSequencePattern)param[0];

            // シーケンス動作排他制御
            if (!this.setActiveSequence(sequenceName, DEFAULT_SEQUENCE_WAIT_TIME))
            {
                // 他に動作中のシーケンスがある為実行失敗
                // この時にセマフォを解除しないよう、異なるシーケンス名を返す
                return "CANCEL";
            }

            SequenceCommData sequenceData = new SequenceCommData(this.abortEventAll, this.CommNo);
            this.pushSequenceCommData(ref param, sequenceData);

            //各ホスト通信処理を呼び出す
            if (seqptn.HasFlag(HostCommunicationSequencePattern.AskWorkSheetToHost))
            {
                //ワークシート問合せ（リアルタイム）
                askWorkSheetToHost(sequenceName, param);
            }
            else if (seqptn.HasFlag(HostCommunicationSequencePattern.AskWorkSheetToHostBatch))
            {
                //ワークシート問合せ（バッチ）
                askWorkSheetToHostBatch(sequenceName, param);
            }
            else if (seqptn.HasFlag(HostCommunicationSequencePattern.SendResultToHost))
            {
                //測定データ送信（リアルタイム）
                sendResultToHost(sequenceName, param);
            }
            else if (seqptn.HasFlag(HostCommunicationSequencePattern.SendResultToHostBatch))
            {
                if (seqptn.HasFlag(HostCommunicationSequencePattern.Specimen))
                {
                    //測定データ送信（検体）
                    sendResultSpecimenToHost(sequenceName, param);
                }
                else
                {
                    //測定データ送信（コントロール）
                    sendResultControlToHost(sequenceName, param);
                }
            }

            return sequenceName;

        }
        #endregion

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// シーケンス同期操作オブジェクト追加
        /// </summary>
        /// <remarks>
        /// シーケンス同期操作オブジェクト追加します
        /// </remarks>
        /// <param name="args"></param>
        /// <param name="syncObj"></param>
        private void pushSequenceSyncObject(ref Object[] args, SequenceSyncObject syncObj)
        {

            // 引数の領域を拡張し、末尾に同期操作オブジェクトを加える
            Array.Resize<Object>(ref args, args.Length + 1);
            args[args.Length - 1] = syncObj;

        }

        /// <summary>
        /// 中断用オブジェクト追加
        /// </summary>
        /// <remarks>
        /// 中断用オブジェクト追加します
        /// </remarks>
        /// <param name="args"></param>
        /// <param name="syncObj"></param>
        private void pushSequenceCommData(ref Object[] args, SequenceCommData sequenceData)
        {
            // 引数の領域を拡張し、末尾に中断用オブジェクトを加える
            Array.Resize<Object>(ref args, args.Length + 1);
            args[args.Length - 1] = sequenceData;
        }

        #endregion

        #region 内部クラス

        /// <summary>
        /// シーケンス同期操作オブジェクト
        /// </summary>
        /// <remarks>
        /// シーケンスとの同期制御に必要な情報を定義します。
        /// </remarks>
        public class SequenceSyncObject
        {
            /// <summary>
            /// シーケンス完了イベント
            /// </summary>
            private ManualResetEvent endSequence = new ManualResetEvent(false);

            /// <summary>
            /// シーケンス完了イベント 取得
            /// </summary>
            public ManualResetEvent EndSequence
            {
                get
                {
                    return this.endSequence;
                }
            }
            /// <summary>
            /// シーケンス完了設定
            /// </summary>
            /// <remarks>
            /// シーケンス完了設定します
            /// </remarks>
            public void SetEnd()
            {
                this.endSequence.Set();
                if (this.OnEndSequence != null)
                {
                    this.OnEndSequence(this);
                }
            }
            /// <summary>
            /// シーケンス完了イベント用デリゲート
            /// </summary>
            /// <param name="obj"></param>
            public delegate void DlgOnEndSequence(SequenceSyncObject obj);
            /// <remarks>
            /// シーケンス完了イベント
            /// </remarks>
            public event DlgOnEndSequence OnEndSequence;

            /// <summary>
            /// シーケンス状態
            /// </summary>
            public enum SequenceStatus
            {
                /// <summary>
                /// 待機中
                /// </summary>
                Wait,
                /// <summary>
                /// 動作中
                /// </summary>
                Running,
                /// <summary>
                /// 成功終了
                /// </summary>
                Success,
                /// <summary>
                /// 失敗終了
                /// </summary>
                Error
            }

            /// <summary>
            /// シーケンス状態
            /// </summary>
            private SequenceStatus status;

            /// <summary>
            /// シーケンス状態 取得/設定
            /// </summary>
            /// <remarks>
            /// シーケンス状態の設定はシーケンス動作している関数からのみ行います。
            /// </remarks>
            public SequenceStatus Status
            {
                get
                {
                    return status;
                }
                set
                {
                    status = value;
                }
            }


            /// <summary>
            /// シーケンス戻り値
            /// </summary>
            private Object sequenceResultData = null;

            /// <summary>
            /// シーケンス戻り値 取得/設定
            /// </summary>
            public Object SequenceResultData
            {
                get
                {
                    return sequenceResultData;
                }
                set
                {
                    sequenceResultData = value;
                }
            }


        }
        /// <summary>
        /// シーケンスデータクラス
        /// </summary>
        /// <remarks>
        /// シーケンスのコマンド待ちうけや、データ受け渡で使用されます。
        /// </remarks>
        public class SequenceCommData : IDisposable
        {
            /// <summary>
            /// 全ての中断イベント
            /// </summary>
            protected AutoResetEvent abortEventAll = new AutoResetEvent(false);

            /// <summary>
            /// 通信番号
            /// </summary>
            private Int32 commNo = 0;

            /// <summary>
            /// 全ての中断
            /// </summary>
            /// <remarks>
            /// このシーケンスデータクラスの全インスタンスで、待機を中断します。
            /// </remarks>
            public void AbortWaitAll()
            {
                // 全体の待ちうけを中断する。
                this.abortEventAll.Set();
            }

            /// <summary>
            /// 中断イベント
            /// </summary>
            protected ManualResetEvent abortEvent = new ManualResetEvent(false);

            /// <summary>
            /// 中断
            /// </summary>
            /// <remarks>
            /// このインスタンスのみで、待機を中断します。
            /// </remarks>
            public void AbortWait()
            {
                // 待ちうけを中断する。
                abortEvent.Set();
            }

            /// <summary>
            /// 待機結果
            /// </summary>
            public enum WaitResult
            {
                /// <summary>
                /// 成功
                /// </summary>
                Success = 0,
                /// <summary>
                /// タイムアウト
                /// </summary>
                TimeOut = 1,
                /// <summary>
                /// 中断
                /// </summary>
                Abort = 2
            }

            /// <summary>
            /// 待機成功
            /// </summary>
            private WaitResult waitSuccess;

            /// <summary>
            /// 待機成功 設定/取得
            /// </summary>
            public WaitResult WaitSuccess
            {
                get
                {
                    return waitSuccess;
                }
                set
                {
                    waitSuccess = value;
                }
            }

            /// <summary>
            /// 受信イベント
            /// </summary>
            private ManualResetEvent rcvEvent = new ManualResetEvent(false);

            /// <summary>
            /// コマンドデータ
            /// </summary>
            private CarisXCommCommand rcvCommand = null;

            /// <summary>
            /// コマンドデータ 設定/取得
            /// </summary>
            public CarisXCommCommand RcvCommand
            {
                get
                {
                    return rcvCommand;
                }
                set
                {
                    rcvCommand = value;
                }
            }

            /// <summary>
            /// 待ちうけコマンド種別
            /// </summary>
            private CommandKind waitCommandKind = CommandKind.Unknown;

            /// <summary>
            /// 待ちうけコマンド種別　設定/取得
            /// </summary>
            public CommandKind WaitCommandKind
            {
                get
                {
                    return waitCommandKind;
                }
                set
                {
                    waitCommandKind = value;
                }
            }
            /// <summary>
            /// 受信ハンドラ
            /// </summary>
            EventHandler<CommCommandEventArgs> receiver;

            /// <summary>
            /// 受信ハンドラ　設定/取得
            /// </summary>
            public EventHandler<CommCommandEventArgs> Receiver
            {
                get
                {
                    return receiver;
                }
                set
                {
                    receiver = value;
                }
            }

            /// <summary>
            /// 受信コマンド用キュー
            /// </summary>
            private LockObject<List<Tuple<CarisXCommCommand, DateTime>>> receiveCommandList =
                new LockObject<List<Tuple<CarisXCommCommand, DateTime>>>();

            /// <summary>
            /// ベースとなる待機時間
            /// </summary>
            protected const Int32 BASE_WAIT_TIME = 1000;

            /// <summary>
            /// 待機時間取得
            /// </summary>
            /// <remarks>
            /// 待機時間取得します
            /// </remarks>
            /// <param name="baseTime"></param>
            /// <param name="setTime"></param>
            /// <param name="offset"></param>
            /// <param name="lastWait"></param>
            /// <returns></returns>
            protected Int32 getWaitTime(Int32 baseTime, Int32 setTime, ref Int32 offset, out Boolean lastWait)
            {
                if (setTime == -1) // 無限待ちなら
                {
                    lastWait = false;
                    return baseTime;
                }

                Int32 waitTime = 0;
                Int32 waitSum = baseTime * (offset + 1);
                waitTime = waitSum <= setTime ? baseTime : setTime % baseTime;
                lastWait = waitSum >= setTime;
                offset++;
                return waitTime;
            }

            /// <summary>
            /// 受信待機
            /// </summary>
            /// <remarks>
            /// 設定したコマンドの受信を待機します。この関数の動作にはReceiverが通信イベントに関連付けられている必要があります。
            /// </remarks>
            /// <param name="milliSecondsTimeout">待機時間</param>
            public void Wait(Int32 milliSecondsTimeout)
            {
                Boolean endFlg = false;
                Boolean lastWaitFlg = false;
                Int32 waitTimeOffset = 0;
                Int32 waitTime = 0;
                System.Diagnostics.Debug.WriteLine(String.Format("【Receiver確認】{0} Wait開始({1})", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), this.waitCommandKind.ToString()));
                while (true)
                {
                    // 受信コマンド用リストチェック
                    this.receiveCommandList.Lock();
                    for (Int32 index = 0; index < this.receiveCommandList.Get.Instance.Count(); index++)
                    {
                        var data = this.receiveCommandList.Get.Instance.ElementAt(index);
                        if ((this.WaitCommandKind == CommandKind.Unknown) || (this.WaitCommandKind == data.Item1.CommKind))
                        {
                            System.Diagnostics.Debug.WriteLine(String.Format("【Receiver確認】{0} WaitOK({1})", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), this.waitCommandKind.ToString()));
                            // コマンド受信成功
                            this.RcvCommand = data.Item1;
                            this.receiveCommandList.Get.Instance.RemoveAt(index);
                            this.waitSuccess = WaitResult.Success;
                            endFlg = true;
                            break;
                        }
                    }
                    this.rcvEvent.Reset();
                    this.receiveCommandList.UnLock();

                    if (endFlg == true)
                        break;// 期待するデータが見つかったのでここで関数終了

                    // 中断・コマンド受信イベントを待機する。waitResultは待機オブジェクト配列のシグナルが発生したインデックス                            System.Diagnostics.Debug.WriteLine( String.Format( "【Receiver確認】WaitOK({0})", this.waitCommandKind.ToString() ) );
                    //System.Diagnostics.Debug.WriteLine( String.Format( "【Receiver確認】WaitAny開始" ) );
                    waitTime = this.getWaitTime(BASE_WAIT_TIME, milliSecondsTimeout, ref waitTimeOffset, out lastWaitFlg);
                    Int32 waitResult = WaitHandle.WaitAny(new WaitHandle[] { this.rcvEvent, abortEvent, this.abortEventAll }, waitTime);
                    //System.Diagnostics.Debug.WriteLine( String.Format( "【Receiver確認】WaitAny終了 Result={0}", waitResult ) );
                    if ((waitResult == WaitHandle.WaitTimeout) && (lastWaitFlg))
                    {
                        // タイムアウト発生
                        this.waitSuccess = WaitResult.TimeOut;
                        break;// タイムアウト発生したのでここで関数終了
                    }
                    else if ((waitResult == 1) || (waitResult == 2))
                    {
                        // 中断（全ての中断も含む）
                        this.waitSuccess = WaitResult.Abort;
                        break;// 中断されたのでここで関数終了
                    }
                    else
                    {
                        // ここでの処理は無し。ループに戻って受信コマンド用リストをチェックする
                    }
                }
            }

            /// <summary>
            /// 解放
            /// </summary>
            /// <remarks>
            /// コマンド受信イベント解除します
            /// </remarks>
            public void Dispose()
            {
                Singleton<CarisXCommMessageManager>.Instance.ReceiveCommCommand -= this.Receiver;
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("【Command Receiver】id:{1} Remove Receiver Waiting(wait)command Id={0}", this.waitCommandKind.ToString(), this.GetHashCode()));
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="commNo">通信番号</param>
            /// <param name="resetEvent">リセット要求</param>
            public SequenceCommData(AutoResetEvent resetEvent, Int32 commNo = 0)
            {
                this.commNo = commNo;
                this.abortEventAll = resetEvent;
                this.receiver = this.onReceive;

                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("【Command Receiver】id:{1} Add Receiver Waiting(wait)command Id={0}", this.waitCommandKind.ToString(), this.GetHashCode()));
                Singleton<CarisXCommMessageManager>.Instance.ReceiveCommCommand += this.Receiver;
            }

            /// <summary>
            /// 受信イベントハンドラ
            /// </summary>
            /// <remarks>
            /// 待ちうけコマンドの受信を行います。
            /// </remarks>
            /// <param name="sender">不使用</param>
            /// <param name="command">コマンドデータ</param>
            protected void onReceive(Object sender, CommCommandEventArgs command)
            {
                // 受信コマンドをリストに突っ込んでイベントをセットする(この処理はメインスレッドにて動作する)
                CarisXCommCommand cmd = command.Command as CarisXCommCommand;
                if (cmd != null)
                {
                    // 通信番号が一致しない場合、スルー
                    // ※メンテナンス用シーケンス(=0)の場合、チェック不要（基本1:1で動作するものであり、commNoが限定できないことも踏まえて）
                    if ((cmd.CommNo != this.commNo) && (this.commNo != 0))
                    {
                        return;
                    }

                    try
                    {
                        //System.Diagnostics.Debug.WriteLine( String.Format( "【Receiver確認】Command{0:0000}を追加", command.Command.CommandId ) );
                        // Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("【Command Receiver】id:{2} Reception Command Id ={0} 待機(wait)command Id={1}", command.Command.CommandId, this.waitCommandKind.ToString(), this.GetHashCode()));
                        this.receiveCommandList.Lock();
                        this.receiveCommandList.Get.Instance.Add(new Tuple<CarisXCommCommand, DateTime>(cmd, DateTime.Now));
#if DEBUG
                        System.Diagnostics.Debug.WriteLine(String.Format("【Receiver確認】受信イベントをSignal", new Tuple<CarisXCommCommand, DateTime>(cmd, DateTime.Now)));
                        for (Int32 index = 0; index < this.receiveCommandList.Get.Instance.Count(); index++)
                        {
                            var data = this.receiveCommandList.Get.Instance.ElementAt(index);
                            System.Diagnostics.Debug.WriteLine(String.Format("【Receiver確認】[{0}]receiveCommandList({1}:{2})", this.commNo, index, data.Item1.ToString()));
                        }
#endif
                        this.rcvEvent.Set();
                        this.receiveCommandList.UnLock();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(String.Format("{0} {1}", ex.Message, ex.StackTrace));
                        Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("{0} {1}", ex.Message, ex.StackTrace));
                    }
                }
            }
        }
        #endregion
    }
}
