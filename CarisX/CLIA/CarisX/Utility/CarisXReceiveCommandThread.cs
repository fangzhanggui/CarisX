using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

using Oelco.Common.Utility;
using Oelco.Common.Log;
using Oelco.Common.Comm;
using Oelco.Common.Parameter;

using Oelco.CarisX.Comm;
using Oelco.CarisX.Log;
using Oelco.CarisX.Const;
using Oelco.CarisX.Status;
using Oelco.CarisX.Common;
using Oelco.CarisX.DB;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Calculator;
using Oelco.CarisX.GUI;

namespace Oelco.CarisX.Utility
{
    /// <summary>
    /// 受信コマンド処理スレッド
    /// </summary>
    public class CarisXReceiveCommandThread
    {
        #region [インスタンス変数定義]
        
        /// <summary>
        /// ホスト問い合わせデータ
        /// </summary>
        static private List<AskWorkSheetData> askHostData = new List<AskWorkSheetData>();

        /// <summary>
        /// ホスト問合せ完了済フラグ
        /// </summary>
        /// <remarks>
        /// ON （true） …ホスト問合せ後、レスポンスを受信した
        /// OFF（false）…ホスト問合せ前、またはホスト問合せ後にレスポンスがまだ帰ってきていない
        /// </remarks>
        static private Boolean flgAskHost = false;

        /// <summary>
        /// 検体識別ID・プロトコル→多重単位のIndividuallyNumber.Protocol->多重单位
        /// </summary>
        private Dictionary<Tuple<Int32, Int32>, List<Tuple<SlaveCommCommand_0503, Boolean, Boolean, Boolean>>> assayEndList = new Dictionary<Tuple<int, int>, List<Tuple<SlaveCommCommand_0503, bool, bool, bool>>>();

        /// <summary>
        /// エラーコード：ラック、モジュールのモーターエラーコード番号
        /// </summary>
        private int[] motorErrorRackModuleCode = new int[3] { 1, 2, 3 };

        #endregion

        #region [コンストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CarisXReceiveCommandThread()
        {
        }

        #endregion

        #region [メソッド]

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            // 初期化処理があればここに記載
        }

        /// <summary>
        /// スレッド開始処理
        /// </summary>
        /// <param name="param"></param>
        public void Start(params Object[] param)
        {
            try
            {
                if (param[0] is CarisXCommCommand)
                {

                    CarisXCommCommand cmd = param[0] as CarisXCommCommand;

                    // コマンド種別チェック
                    switch (cmd.CommKind)
                    {
                        case CommandKind.RackTransferCommand0104:
                            // エラーコマンド
                            this.onErrorCommand(param);
                            break;
                        case CommandKind.RackTransferCommand0105:
                            // サブイベントコマンド
                            this.onRackSubEvent(param);
                            break;
                        case CommandKind.RackTransferCommand0106:
                            // ラック分析ステータスコマンド
                            this.onRackAssayStatus(param);
                            break;
                        case CommandKind.RackTransferCommand0108:
                            // 残量コマンド
                            this.onReagentRemainCommandFromRack(param);
                            break;
                        case CommandKind.RackTransferCommand0111:
                            // バージョンコマンド(ラック搬送用)
                            this.onVersionCommandFromRack(param);
                            break;
                        case CommandKind.RackTransferCommand0117:
                            // ラック情報通知コマンド
                            this.onNotifyRackInfomation(param);
                            break;
                        case CommandKind.RackTransferCommand0119:
                            // ラック移動位置問合せ（装置待機位置）コマンド
                            Task ReceiveCommand0119 = new Task(() =>
                            {
                                this.onRackMoveInquiryFromModuleWait(param);
                            });
                            ReceiveCommand0119.Start();
                            break;
                        case CommandKind.RackTransferCommand0120:
                            // ラック移動位置問合せ（BCR）コマンド
                            Task ReceiveCommand0120 = new Task(() =>
                            {
                                this.onRackMoveInquiryFromBCR(param);
                            });
                            ReceiveCommand0120.Start();
                            break;
                        case CommandKind.Command0502:
                            // 測定指示データ問い合わせコマンド
                            this.onAskAssayData(param);
                            break;
                        case CommandKind.Command0503:
                            // 測定データコマンド
                            Task ReceiveCommand0503Task = new Task(() =>
                            {
                                this.onAssayData(param);
                            });
                            ReceiveCommand0503Task.Start();
                            break;
                        case CommandKind.Command0504:
                            // エラーコマンド
                            this.onErrorCommand(param);
                            break;
                        case CommandKind.Command0505:
                            // サブイベントコマンド
                            this.onSlaveSubEvent(param);
                            break;
                        case CommandKind.Command0506:
                            // 分析ステータスコマンド
                            this.onAssayStatus(param);
                            break;
                        case CommandKind.Command0508:
                            // 残量コマンド(スレーブ用)
                            this.onReagentRemainCommandFromSlave(param);
                            break;
                        case CommandKind.Command0510:
                            // マスターカーブ情報コマンド
                            this.onMasterCurveCommand(param);
                            break;
                        case CommandKind.Command0511:
                            // バージョン通知コマンド(スレーブ用)
                            this.onVersionCommandFromSlave(param);
                            break;
                        case CommandKind.Command0512:
                            // 試薬ロット確認コマンド
                            this.onReagentLotChangeCommand(param);
                            break;
                        case CommandKind.Command0513:
                            // キャリブレーション測定確認コマンド
                            this.onCalibMeasureCommand(param);
                            break;
                        case CommandKind.Command0514:
                            // 総アッセイ数通知コマンド
                            this.onAssayTotalCount(param);
                            break;
                        case CommandKind.Command0515:
                            // ラック設定状況コマンド
                            this.onRackStatus(param);
                            break;
                        case CommandKind.Command0516:
                            // 試薬テーブル回転SW押下通知コマンド
                            this.onTableTurnCommand(param);
                            break;
                        case CommandKind.Command0520:
                            // 試薬設置状況通知コマンド
                            this.onReagentStatusCommand(param);
                            break;
                        case CommandKind.Command0521:
                            // 廃液タンク状態問合せコマンド
                            this.onWasteTankStatusCommand(param);
                            break;
                        case CommandKind.Command0522:
                            // キャリブレータ情報通知コマンド
                            this.onNotifyCalibratorInfoCommand(param);
                            break;
                        case CommandKind.Command0591:
                            // STAT状態通知コマンド
                            this.onNotifySTATStatus(param);
                            break;
                        case CommandKind.Command0596:
                            // 分取完了コマンド
                            this.onSampleAspirationCompleted(param);
                            break;
                        case CommandKind.HostCommand0004:
                            // 装置ステータス問合せコマンド
                            this.onAskSlaveStatus(param);
                            break;

                    }
                }
                else
                {
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                        CarisXLogInfoBaseExtention.Empty, String.Format("{0} The command could not be converted.", System.Reflection.MethodBase.GetCurrentMethod().Name));
                }
            }
            catch (Exception ex)
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        #endregion

        ///////////////////////////////////////
        // ラック搬送部関連コマンド
        ///////////////////////////////////////
        #region [ [0104][0504]エラー通知コマンド ]

        /// <summary>
        /// [0104][0504]エラー通知コマンド解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onErrorCommand(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    RackTransferCommCommand_0104 cmd0104 = arrayParam[0] as RackTransferCommCommand_0104;

                    // エラー通知コマンド解析処理コール
                    this.OnErrorCommand(cmd0104);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0104][0504]エラー通知コマンド解析
        /// </summary>
        /// <param name="cmd0104"></param>
        protected void OnErrorCommand(RackTransferCommCommand_0104 cmd0104)
        {
            try
            {
                CarisXLogInfoErrorLogExtention errorExt = new CarisXLogInfoErrorLogExtention();
                errorExt.ErrorCode = cmd0104.ErrorCode;
                errorExt.ErrorArg = cmd0104.Arg;

                // モジュールID取得
                Int32 moduleID = CarisXSubFunction.MachineCodeToRackModuleIndex((MachineCode)cmd0104.CommNo);

                // エラー時の警告灯およびブザー鳴動用にエラーコードを変換
                DPRErrorCode errCode = new DPRErrorCode(cmd0104.ErrorCode, cmd0104.Arg, moduleID);

                // レスポンスの送信処理
                if (cmd0104.CommandId == (Int32)CommandKind.RackTransferCommand0104)
                {
                    // ラック搬送にレスポンスを返す
                    Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(new RackTransferCommCommand_1104());
                }
                else
                {
                    // モジュールIndex取得（ラック搬送の場合、スレーブ1(=0)になる）
                    Int32 moduleIndex = CarisXSubFunction.ModuleIDToModuleIndex(moduleID);

                    // スレーブにレスポンスを返す
                    Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((int)moduleIndex, new SlaveCommCommand_1504());
                }

                // エラー履歴に登録
                CarisXSubFunction.WriteDPRErrorHist(errCode, 0, cmd0104.Str);

                if (motorErrorRackModuleCode.Contains(errCode.ErrorCode))
                {
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.CheckErrorRackModule, moduleID);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        #endregion

        #region [ [0105]ラックサブイベントコマンド ]

        /// <summary>
        /// [0105]サブイベントコマンド解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onRackSubEvent(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    RackTransferCommCommand_0105 cmd0105 = arrayParam[0] as RackTransferCommCommand_0105;

                    // サブイベントコマンド解析処理コール
                    this.OnRackSubEvent(cmd0105);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0105]サブイベントコマンド解析
        /// </summary>
        /// <param name="cmd0105"></param>
        protected void OnRackSubEvent(RackTransferCommCommand_0105 cmd0105)
        {
            try
            {
                Singleton<SystemStatus>.Instance.PauseReason[(Int32)RackModuleIndex.RackTransfer] = SamplingPauseReason.SAMPLINGPAUSEREASON_DEFAULT;

                switch (cmd0105.SubEvent)
                {
                    case RackTransferSubEvent.Wait://1
                                                   // 待機状態
                        Singleton<SystemStatus>.Instance.setModuleStatus(RackModuleIndex.RackTransfer, SystemStatusKind.Standby);
                        break;
                    case RackTransferSubEvent.Running://2
                                                      // TODO:稼働状態（処理が不明。いったんSlaveのAssayのパターンのまま）
                        Singleton<SystemStatus>.Instance.setModuleStatus(RackModuleIndex.RackTransfer, SystemStatusKind.Assay);
                        break;
                    case RackTransferSubEvent.SamplingStop://3
                                                           // サンプリング停止
                        switch (cmd0105.SubEventArg1)
                        {
                            case (int)SamplingPauseReason.SamplingPauseReasonBit.PreparationReagent:
                            case (int)SamplingPauseReason.SamplingPauseReasonBit.PreparationDiluent:
                            case (int)(SamplingPauseReason.SamplingPauseReasonBit.PreparationReagent | SamplingPauseReason.SamplingPauseReasonBit.PreparationDiluent):
                                //M、R1、R2試薬または希釈液の準備の為にサンプリング停止した場合、モジュールステータスは変更しない
                                //※他のサンプリング停止要因がある場合はこの限りではない為、switchで分岐させる
                                break;
                            default:
                                //試薬・希釈液の準備以外の場合はモジュールステータスをサンプリング停止状態にする
                                Singleton<SystemStatus>.Instance.setModuleStatus(RackModuleIndex.RackTransfer, SystemStatusKind.SamplingPause);
                                Singleton<SystemStatus>.Instance.PauseReason[(Int32)RackModuleIndex.RackTransfer] = cmd0105.SubEventArg1;
                                break;
                        }

                        break;
                }

                // システムステータスアイコン設定通知
                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SetSystemStatusIcon);

                if (Singleton<CarisXSequenceHelperManager>.Instance.RackTransfer.flgInitializeSequenceCompleted)
                {
                    //初期シーケンスが終わっている場合は、ここでレスポンスを返す
                    Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(new RackTransferCommCommand_1105());
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        #endregion

        #region [ [0106]ラック分析ステータスコマンド ]

        /// <summary>
        /// [0106]ラック分析ステータスコマンド解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onRackAssayStatus(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    RackTransferCommCommand_0106 cmd0106 = arrayParam[0] as RackTransferCommCommand_0106;

                    // ラック分析ステータスコマンド解析処理コール
                    this.OnRackAssayStatus(cmd0106);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0106]ラック分析ステータスコマンド解析
        /// </summary>
        /// <param name="cmd0106"></param>
        protected void OnRackAssayStatus(RackTransferCommCommand_0106 cmd0106)
        {
            try
            {
                //待機レーン→回収レーンへの移動を反映する

                //回収レーン以外の場所にいるラックの情報を取得
                var rackPosition = Singleton<RackPositionManager>.Instance.RackPosition.Where(v => v.Value != RackPositionKind.CollectRack).ToDictionary(rackpos => rackpos.Key);
                foreach (var rackPos in rackPosition)
                {
                    if (((CarisXIDString)rackPos.Key).GetSampleKind() == SampleKind.Priority)
                    {
                        //STAT検体はラック分析ステータスでは絶対に来ないが位置を変えたくないので飛ばす。
                        continue;
                    }

                    //受信したラック分析ステータスにラックIDが存在しないかチェック
                    if (cmd0106.RackID.Contains(rackPos.Key) == false)
                    {
                        //ラックIDが存在しない場合、該当ラックはラック回収レーン（または待機レーン）に居るので、ラック位置を変更する
                        Singleton<RackPositionManager>.Instance.RackPosition[rackPos.Key] = RackPositionKind.CollectRack;
                    }
                }

                //レスポンスを返す
                Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(new RackTransferCommCommand_1106());
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        #endregion

        #region [ [0108]残量コマンド(ラック搬送用) ]

        /// <summary>
        /// [0108]残量コマンド解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onReagentRemainCommandFromRack(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    RackTransferCommCommand_0108 cmd0108 = arrayParam[0] as RackTransferCommCommand_0108;

                    // 残量コマンド解析処理コール
                    this.OnReagentRemainCommandFromRack(cmd0108);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0108]残量コマンド解析
        /// </summary>
        /// <param name="cmd0108"></param>
        protected void OnReagentRemainCommandFromRack(RackTransferCommCommand_0108 cmd0108)
        {
            try
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                                CarisXLogInfoBaseExtention.Empty, String.Format("OnReagentRemainCommandFromRack"));

                CarisXSubFunction.SetRackReagentRemain(cmd0108);

                // 廃液タンク状態を取得
                Boolean existWasteTank = cmd0108.ExistWasteTank;
                Boolean isFullWasteTank = cmd0108.IsFullWasteTank;

                cmd0108 = null;

                //レスポンスを返す
                Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(new RackTransferCommCommand_1108());

                // 各モジュールへ廃液タンク状態通知コマンドを送信
                foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
                {
                    // オンラインとなっているモジュールへ送信
                    if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                    {
                        SlaveCommCommand_0447 cmd0447 = new SlaveCommCommand_0447();
                        cmd0447.ExistWasteTank = existWasteTank;
                        cmd0447.IsFullWasteTank = isFullWasteTank;
                        Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((int)moduleindex, cmd0447);
                    }
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        #endregion

        #region [ [0111]バージョンコマンド(ラック搬送用) ]

        /// <summary>
        /// [0111]バージョンコマンド(ラック搬送用) 解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onVersionCommandFromRack(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    RackTransferCommCommand_0111 cmd0111 = arrayParam[0] as RackTransferCommCommand_0111;

                    // バージョンコマンド(ラック搬送用)解析処理コール
                    this.OnVersionCommandFromRack(cmd0111);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0111]バージョンコマンド(ラック搬送用)解析
        /// </summary>
        /// <param name="cmd0111"></param>
        protected void OnVersionCommandFromRack(RackTransferCommCommand_0111 cmd0111)
        {
            try
            {
                // バージョンコマンド通知
                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.RackTransferVersion, cmd0111);
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        #endregion

        #region [ [0117]ラック情報通知コマンド ]

        /// <summary>
        /// [0117]ラック情報通知コマンド解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onNotifyRackInfomation(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    RackTransferCommCommand_0117 cmd0117 = arrayParam[0] as RackTransferCommCommand_0117;

                    // ラック情報通知コマンド解析処理コール
                    this.OnNotifyRackInfomation(cmd0117);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0117]ラック情報通知コマンド解析
        /// </summary>
        /// <param name="cmd0117"></param>
        protected void OnNotifyRackInfomation(RackTransferCommCommand_0117 cmd0117)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(String.Format("OnNotifyRackInfomation"));
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                                CarisXLogInfoBaseExtention.Empty, String.Format("OnNotifyRackInfomation"));

                Singleton<RackInfoManager>.Instance.RackInfoInitialize();

                // ラック情報に受信した内容を記録する
                RackInfo RackInfo = new RackInfo();
                RackInfo.RackId = cmd0117.RackID;

                for (int i = 0; i < CarisXConst.RACK_POS_COUNT; i++)
                {
                    RackInfo.RackPosition.Add(new RackInfoDetail());
                    RackInfo.RackPosition.Last().PositionNo = i + 1;
                    RackInfo.RackPosition.Last().SampleID = cmd0117.SampleId[i];
                    RackInfo.RackPosition.Last().CupKind = cmd0117.CupKind[i];
                }

                Singleton<RackInfoManager>.Instance.SetRackInfo(RackInfo);
                askHostData.RemoveAll(v => v.AskData.RackID == cmd0117.RackID);

                RackTransferCommCommand_1117 respCommand = new RackTransferCommCommand_1117();

                Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(respCommand);
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        #endregion

        #region [ [0119]ラック移動位置問合せ（装置待機位置）コマンド ]

        /// <summary>
        /// [0119]ラック移動位置問合せ（装置待機位置）コマンド解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onRackMoveInquiryFromModuleWait(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    RackTransferCommCommand_0119 cmd0119 = arrayParam[0] as RackTransferCommCommand_0119;

                    // ラック移動位置問合せ（装置待機位置）コマンド解析処理コール
                    this.OnRackMoveInquiryForModuleWait(cmd0119);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0119]ラック移動位置問合せ（装置待機位置）コマンド解析
        /// </summary>
        /// <param name="cmd0119"></param>
        protected void OnRackMoveInquiryForModuleWait(RackTransferCommCommand_0119 cmd0119)
        {
            try
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, String.Format("OnRackMoveInquiryForModuleWait"));

                int errorArg = 0;
                RackTransferCommCommand_1119 respCommand = new RackTransferCommCommand_1119();
                respCommand.StopPosition = Singleton<RackMoveManager>.Instance.GetRackMoveStopPosition(cmd0119);

                // 回収レーンに戻る場合
                if ((respCommand.StopPosition == RackPositionKind.CollectRack) && (errorArg != (Int32)ErrorCollectKind.NoError))
                {
                    // エラー付与文字列の生成 " RackID:XXXX"
                    String errStr = Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_000 + Oelco.CarisX.Properties.Resources.STRING_COMMON_019 + cmd0119.RackID;

                    // エラー履歴に登録
                    CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.RackTransferError, errorArg, errStr);
                }

                respCommand.MoveBetweenDevice = RackTransferCommCommand_1119.MoveBetweenDeviceKind.No;
                respCommand.RackID = cmd0119.RackID;

                //ラックがモジュール１～４→モジュール１～４のいずれかに移動する場合
                if ((respCommand.StopPosition != RackPositionKind.CollectRack) && (respCommand.StopPosition != RackPositionKind.Rack))
                {
                    // モジュール間移動フラグ
                    Boolean isMoveBetweenDevice = false;
                    isMoveBetweenDevice = Singleton<RackMoveManager>.Instance.IsMoveBetweenDevice(cmd0119.RackID, cmd0119.StartPosition, respCommand.StopPosition);

                    // モジュール間の移動がある場合
                    if (isMoveBetweenDevice == true)
                    {
                        // 装置間移動有無をYesに設定する
                        respCommand.MoveBetweenDevice = RackTransferCommCommand_1119.MoveBetweenDeviceKind.Yes;
                    }
                }

                //ラックがモジュール１～４→待機レーンに移動する場合
                //該当ラックIDで一般検体の登録情報が存在する場合は再検リストへ移動する
                if (respCommand.StopPosition == RackPositionKind.Rack)
                {
                    this.setRemeasureDBfromSpecimenGeneral(cmd0119.RackID);
                }

                Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(respCommand);

                Singleton<RackPositionManager>.Instance.RackPositionInitialize();
                Singleton<RackPositionManager>.Instance.SetRackPosition(cmd0119.RackID, respCommand.StopPosition);

                //ラック待機に移動する際、自動希釈再検・自動再検を行わないものの場合は測定完了通知を行う
                if (respCommand.StopPosition == RackPositionKind.Rack)
                {
                    //自動再検が存在する？
                    var rackInfo = Singleton<RackInfoManager>.Instance.RackInfo.Find(v => v.RackId.DispPreCharString == cmd0119.RackID);
                    if ((rackInfo == null) || (rackInfo.IsAutoReMeasure == false))
                    {
                        //自動再検が存在しない場合は、このタイミングで測定完了通知を行う
                        RackTransferCommCommand_0097 cmd0097 = new RackTransferCommCommand_0097();
                        cmd0097.RackId = cmd0119.RackID;
                        cmd0097.ReTestWith = RackTransferCommCommand_0097.ReTestKind.No;
                        Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0097);

                    }
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        #endregion

        #region [ [0120]ラック移動位置問合せ（BCR）コマンド ]

        /// <summary>
        /// [0120]ラック移動位置問合せ（BCR）コマンド解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onRackMoveInquiryFromBCR(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    RackTransferCommCommand_0120 cmd0120 = arrayParam[0] as RackTransferCommCommand_0120;

                    // ラック移動位置問合せ（BCR）コマンド解析処理コール
                    this.OnRackMoveInquiryFromBCR(cmd0120);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0120]ラック移動位置問合せ（BCR）コマンド解析
        /// </summary>
        /// <param name="cmd0120"></param>
        protected void OnRackMoveInquiryFromBCR(RackTransferCommCommand_0120 cmd0120)
        {
            try
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, String.Format("OnRackMoveInquiryFromBCR"));

                // エラー引数
                int errorArg = 0;
                if (cmd0120.StartPosition == RackTransferCommCommand_0119.PositionKind.BCR)
                {

/* @@@検討した結果、対応不足となったため、一旦コメントアウト
                    // ラック情報を確認する
                    var rackinfos = Singleton<RackInfoManager>.Instance.RackInfo.Where(v => v.RackId.DispPreCharString == cmd0120.RackID);
                    if ((rackinfos != null) && (rackinfos.Count() > 0))
                    {
                        RackInfo rackInfo = rackinfos.FirstOrDefault();

                        // キャリブレータ時にラック情報の問合せダイアログを表示する
                        if (rackInfo.RackId.PreChar == CarisXConst.CALIB_RACK_ID_PRECHAR)
                        {
                            Boolean isShowDlgAskRackMoveSelection = false;

                            foreach (var rackInfoDetail in rackInfo.RackPosition)
                            {
                                // キャリブレータかつサンプルIDが10桁以上の場合
                                if (rackInfoDetail.SampleID.Length >= 10)
                                {
                                    // 自動登録指定のラックのため、サンプルIDを分解
                                    String sampleId = rackInfoDetail.SampleID;

                                    // 検体IDを分解する（000:試薬コード(プロトコル番号) 000000:CalibratorLot 0:ConcIndex）
                                    int reagentCode = int.Parse(sampleId.Substring(0, 3));
                                    String calibLot = sampleId.Substring(3, 6).Trim();
                                    int concIdx = int.Parse(sampleId.Substring(9, 1));

                                    // キャリブレータロットが5桁以上の場合
                                    if (calibLot.Length >= 5)
                                    {
                                        // 接続台数が複数台構成か確認
                                        if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected > 1)
                                        {
                                            isShowDlgAskRackMoveSelection = true;
                                            break;
                                        }
                                        else
                                        {
                                            // 1台構成のため、これまで通りモジュール1への移動を設定
                                            rackInfoDetail.RegisteredModulesForAutoCalib.Add(RackModuleIndex.Module1);
                                        }
                                    }
                                }
                            }

                            // ラック移動先選択ダイアログを表示する必要がある場合
                            if (isShowDlgAskRackMoveSelection == true)
                            {
                                // ラック移動先選択ダイアログを表示
                                DlgAskRackMoveSelection dlgAskRackMoveSelection = new DlgAskRackMoveSelection(rackInfo);
                                dlgAskRackMoveSelection.ShowDialog();
                            }
                        }
                    }
*/

                    //BCR（＝初回問合せ）の場合、分析予定の内、分析項目を設定する
                    this.setMeasItemToAssaySchedule(cmd0120.RackID, ref errorArg);
                }

                RackTransferCommCommand_1120 respCommand = new RackTransferCommCommand_1120();
                respCommand.StopPosition = Singleton<RackMoveManager>.Instance.GetRackMoveStopPosition(cmd0120);

                // 回収レーンに戻る場合
                if ((respCommand.StopPosition == RackPositionKind.CollectRack) && (errorArg != (Int32)ErrorCollectKind.NoError))
                {
                    // エラー付与文字列の生成 " RackID:XXXX"
                    String errStr = Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_000 + Oelco.CarisX.Properties.Resources.STRING_COMMON_019 + cmd0120.RackID;

                    // エラー履歴に登録
                    CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.RackTransferError, errorArg, errStr);
                }

                respCommand.MoveBetweenDevice = RackTransferCommCommand_1119.MoveBetweenDeviceKind.No;
                respCommand.RackID = cmd0120.RackID;

                //ラックがBCR→モジュール１～４のいずれかに移動する場合
                //このラックの分析予定を作成し、どこのモジュールに移動する事があるかがわかるようにする
                if (respCommand.StopPosition != RackPositionKind.CollectRack)
                {
                    // モジュール間移動フラグ
                    Boolean isMoveBetweenDevice = false;
                    isMoveBetweenDevice = Singleton<RackMoveManager>.Instance.SetModuleIdToAssaySchedule(cmd0120.RackID, respCommand.StopPosition);

                    // モジュール間の移動がある場合
                    if (isMoveBetweenDevice == true)
                    {
                        // 装置間移動有無をYesに設定する
                        respCommand.MoveBetweenDevice = RackTransferCommCommand_1119.MoveBetweenDeviceKind.Yes;
                    }
                }

                //ラックがモジュール１～４→待機レーンに移動する場合
                //該当ラックIDで一般検体の登録情報が存在する場合は再検リストへ移動する
                if (respCommand.StopPosition == RackPositionKind.Rack)
                {
                    this.setRemeasureDBfromSpecimenGeneral(cmd0120.RackID);
                }

                Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(respCommand);

                Singleton<RackPositionManager>.Instance.RackPositionInitialize();
                Singleton<RackPositionManager>.Instance.SetRackPosition(cmd0120.RackID, respCommand.StopPosition);

                //ラック待機に移動する際、自動希釈再検・自動再検を行わないものの場合は測定完了通知を行う
                if (respCommand.StopPosition == RackPositionKind.Rack)
                {
                    //自動再検が存在する？
                    var rackInfo = Singleton<RackInfoManager>.Instance.RackInfo.Find(v => v.RackId.DispPreCharString == cmd0120.RackID);
                    if ((rackInfo == null) || (rackInfo.IsAutoReMeasure == false))
                    {
                        //自動再検が存在しない場合は、このタイミングで測定完了通知を行う
                        RackTransferCommCommand_0097 cmd0097 = new RackTransferCommCommand_0097();
                        cmd0097.RackId = cmd0120.RackID;
                        cmd0097.ReTestWith = RackTransferCommCommand_0097.ReTestKind.No;
                        Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0097);

                    }
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }


        /// <summary>
        /// 検体登録情報から再検査DB設定
        /// </summary>
        /// <remarks>
        /// 検体情報登録から再検査リストへの設定を行います。
        /// </remarks>
        protected void setRemeasureDBfromSpecimenGeneral(String rackId)
        {
            String dbgMsgHead = String.Format("[[Investigation log]]setRemeasureDBfromSpecimenGeneral ");
            String dbgMsg = dbgMsgHead;

            //ラック情報を取得
            var rackinfos = Singleton<RackInfoManager>.Instance.RackInfo.Where(v => v.RackId.DispPreCharString == rackId);
            if ((rackinfos != null) && (rackinfos.Count() > 0))
            {
                //登録データ存在チェック用
                SlaveCommCommand_1502 respCommand = new SlaveCommCommand_1502();
                AskWorkSheetData workSheetData = new AskWorkSheetData();
                AskTypeKind asktype = AskTypeKind.RackID;
                switch (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.AssayMode)
                {
                    case AssayModeParameter.AssayModeKind.SampleID:
                        asktype = AskTypeKind.SampleID;
                        break;
                }
                String comment = String.Empty;
                Int32 remeasReceiptNumber = 0;
                Boolean targetFind = false;

                //ラック情報があれば、各ポジションごとにデータが存在するかチェックする
                var rackinfo = rackinfos.FirstOrDefault();
                foreach (var rackinfoDetail in rackinfo.RackPosition)
                {
                    dbgMsg = dbgMsgHead + String.Format("{0}-{1} ", rackId, rackinfoDetail.PositionNo);

                    respCommand = new SlaveCommCommand_1502();
                    workSheetData = new AskWorkSheetData();
                    workSheetData.AskData = respCommand;
                    workSheetData.ToDprCommand = respCommand;

                    List<SpecimenReMeasureGridViewDataSet> setList = new List<SpecimenReMeasureGridViewDataSet>();
                    SpecimenReMeasureGridViewDataSet newReMeasureData = new SpecimenReMeasureGridViewDataSet();

                    workSheetData.AskData.RackID = rackId;
                    workSheetData.AskData.SampleID = rackinfoDetail.SampleID;
                    workSheetData.AskData.SamplePosition = rackinfoDetail.PositionNo;

                    switch (rackinfo.RackId.PreChar)
                    {
                        case CarisXConst.CALIB_RACK_ID_PRECHAR:
                        case CarisXConst.CONTROL_RACK_ID_PRECHAR:
                            //接頭辞がキャリブレータ、コントロールの場合
                            //何もしない
                            break;
                        default:
                            //上記以外の場合

                            int errorArf = 0;

                            //一般検体登録情報に登録が残っているか
                            targetFind = Singleton<SpecimenGeneralDB>.Instance.MeasureIndicate( asktype
                                                                                              , CarisXConst.ALL_MODULEID
                                                                                              , null
                                                                                              , false
                                                                                              , ref workSheetData.AskData
                                                                                              , out remeasReceiptNumber
                                                                                              , out comment
                                                                                              , ref errorArf
                                                                                              , rackinfoDetail.CupKind );
                            if (!targetFind)
                            {
                                //ホストからの受信データから問合せ結果を取得する
                                var data = askHostData.Where(v => v.AskData.RackID == workSheetData.AskData.RackID && v.AskData.SamplePosition == workSheetData.AskData.SamplePosition
                                    && v.AskData.SampleID == workSheetData.AskData.SampleID).FirstOrDefault();
                                if (data != null)
                                {
                                    targetFind = true;
                                    workSheetData = data;
                                    remeasReceiptNumber = data.FromHostCommand.ReceiptNumber;
                                    comment = data.FromHostCommand.Comment;
                                    askHostData.Remove(data);
                                }
                            }

                            break;
                    }

                    if (targetFind)
                    {
                        /* 検体識別番号生成 */
                        respCommand.IndividuallyNumber = Singleton<IndividuallyNo>.Instance.CreateNumber();

                        newReMeasureData.Comment = comment;
                        newReMeasureData.Concentration = null;
                        newReMeasureData.Count = 0;
                        newReMeasureData.IndividuallyNo = respCommand.IndividuallyNumber;
                        newReMeasureData.IsAutoReMeasure = false;
                        newReMeasureData.ManualDilution = workSheetData.AskData.PreDil;

                        // 測定時刻取得を変更
                        newReMeasureData.MeasureDateTime = DateTime.Now;

                        newReMeasureData.ModuleNo = CarisXConst.ALL_MODULEID;
                        newReMeasureData.SampleID = workSheetData.AskData.SampleID;
                        newReMeasureData.RackID = workSheetData.AskData.RackID;
                        newReMeasureData.RackPosition = workSheetData.AskData.SamplePosition;
                        newReMeasureData.ReceiptNumber = remeasReceiptNumber;
                        newReMeasureData.Remark = new Remark();
                        newReMeasureData.SequenceNumber = Singleton<SpecimenAssayDB>.Instance.getSequenceNumber<SequencialSampleNo>(false, 0);
                        newReMeasureData.SpecimenMaterialType = workSheetData.AskData.SpecimenMaterial;
                        newReMeasureData.WaitMeasureIndicate = false; // 再検査コマンドを送信した際trueになる
                        newReMeasureData.ReplicationNo = 1;

                        foreach (var measItem in workSheetData.AskData.MeasItemArray)
                        {
                            // 自動再検履歴を設定
                            var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(measItem.ProtoNo);

                            newReMeasureData.AutoDilution = measItem.AutoDil;
                            newReMeasureData.MeasprotocolNo = measItem.ProtoNo;
                            setList.Add(newReMeasureData);

                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("isOnlyReMeasure8 IsAutoReMeasure={0}", newReMeasureData.IsAutoReMeasure));
                            Boolean bAddRemesureData = Singleton<SpecimenReMeasureDB>.Instance.AddRemesureData(setList);
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("isOnlyReMeasure8 bAddRemesureData={0}", bAddRemesureData));
                            Singleton<SpecimenReMeasureDB>.Instance.CommitSampleReMeasureInfo();
                        }

                        // SpecimenGeneralDBからは削除する。
                        Singleton<SpecimenGeneralDB>.Instance.DeleteData(workSheetData.AskData);
                        Singleton<SpecimenGeneralDB>.Instance.CommitSampleInfo();

                        RealtimeDataAgent.LoadReMeasureSampleData();
                        RealtimeDataAgent.LoadSampleData();
                    }
                    else
                    {
                        dbgMsg = dbgMsg + String.Format("No target data");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                    }
                }
            }
            else
            {
                dbgMsg = dbgMsg + String.Format("No rack information");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
            }
        }

        /// <summary>
        /// 分析予定の設定（分析項目）
        /// </summary>
        /// <param name="rackId">対象のラックID</param>
        /// <param name="errorCollectionArg">回収エラー引数</param>
        private void setMeasItemToAssaySchedule(String rackId, ref int errorCollectionArg)
        {
            String dbgMsgHead = String.Format("[[Investigation log]]setMeasItemToAssaySchedule ");
            String dbgMsg = dbgMsgHead;

            //ラック情報を取得
            var rackinfos = Singleton<RackInfoManager>.Instance.RackInfo.Where(v => v.RackId.DispPreCharString == rackId);
            if ((rackinfos != null) && (rackinfos.Count() > 0))
            {
                //登録データ存在チェック用
                SlaveCommCommand_1502 respCommand = new SlaveCommCommand_1502();
                AskWorkSheetData workSheetData = new AskWorkSheetData();
                AskTypeKind asktype = AskTypeKind.RackID;
                switch (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.AssayMode)
                {
                    case AssayModeParameter.AssayModeKind.SampleID:
                        asktype = AskTypeKind.SampleID;
                        break;
                }
                Boolean targetFind = false;
                Int32 remeasSequenceNo;
                String comment = String.Empty;
                Int32 remeasReceiptNumber;
                Boolean askHost = false;
                int errorCode = 0;
                int errorArg = 0;
                Boolean posCheck = false;

                //ラック情報があれば、各ポジションごとにデータが存在するかチェックする
                var rackinfo = rackinfos.FirstOrDefault();
                foreach (var rackinfoDetail in rackinfo.RackPosition)
                {
                    dbgMsg = dbgMsgHead + String.Format("{0}-{1} ", rackId, rackinfoDetail.PositionNo);

                    posCheck = true;

                    respCommand = new SlaveCommCommand_1502();
                    workSheetData = new AskWorkSheetData();
                    workSheetData.AskData = respCommand;
                    workSheetData.ToDprCommand = respCommand;
                    askHost = false;

                    workSheetData.AskData.RackID = rackId;
                    workSheetData.AskData.SampleID = rackinfoDetail.SampleID;
                    workSheetData.AskData.SamplePosition = rackinfoDetail.PositionNo;
                    List<RackModuleIndex> preAssayModuleIdListForCalibrator = new List<RackModuleIndex>();

                    switch (rackinfo.RackId.PreChar)
                    {
                        //接頭辞がキャリブレータの場合
                        case CarisXConst.CALIB_RACK_ID_PRECHAR:

                            // キャリブレータおよびコントロール時はカップ無し時は無効
                            if(rackinfoDetail.CupKind == SpecimenCupKind.None)
                            {
                                continue;
                            }

                            //キャリブレータ登録情報に登録されているかチェック
                            targetFind = Singleton<CalibratorRegistDB>.Instance.MeasureIndicate( CarisXConst.ALL_MODULEID
                                                                                               , null
                                                                                               , ref workSheetData.AskData
                                                                                               , ref errorCollectionArg
                                                                                               , ref preAssayModuleIdListForCalibrator );

                            if (!targetFind)
                            {
                                //キャリブレータ情報に存在しない場合

                                //検体IDの内容からキャリブレータ測定可能か
                                Double conc = 0;
                                targetFind = this.measureIndicateForSampleId( CarisXConst.ALL_MODULEID
                                                                            , null
                                                                            , ref workSheetData.AskData
                                                                            , ref conc
                                                                            , ref errorCode
                                                                            , ref errorArg
                                                                            , ref errorCollectionArg );
/* @@@検討した結果、対応不足となったため、一旦コメントアウト
                                // ラック詳細情報からモジュール搬送先情報を取得
                                preAssayModuleIdListForCalibrator = rackinfoDetail.RegisteredModulesForAutoCalib;
*/
                            }

                            if (errorCode != 0 && errorArg != 0)
                            {
                                //エラーに履歴に登録する

                                DPRErrorCode errCode = new DPRErrorCode(errorCode, errorArg);

                                // エラー履歴に登録
                                CarisXSubFunction.WriteDPRErrorHist(errCode);
                            }
                            break;

                        //接頭辞がコントロールの場合
                        case CarisXConst.CONTROL_RACK_ID_PRECHAR:

                            // キャリブレータおよびコントロール時はカップ無し時は無効
                            if (rackinfoDetail.CupKind == SpecimenCupKind.None)
                            {
                                continue;
                            }

                            //コントロール登録情報に登録されているかチェック
                            targetFind = Singleton<ControlRegistDB>.Instance.MeasureIndicate( CarisXConst.ALL_MODULEID
                                                                                            , null
                                                                                            , ref workSheetData.AskData
                                                                                            , ref errorCode
                                                                                            , ref errorArg
                                                                                            , ref errorCollectionArg );
                            if (errorCode != 0 && errorArg != 0)
                            {
                                //エラーに履歴に登録する

                                DPRErrorCode errCode = new DPRErrorCode(errorCode, errorArg);

                                // エラー履歴に登録
                                CarisXSubFunction.WriteDPRErrorHist(errCode);
                            }
                            break;

                        //上記以外（一般検体）の場合
                        default:

                            //再検データがあるか
                            targetFind = Singleton<SpecimenReMeasureDB>.Instance.MeasureIndicate( asktype
                                                                                                , CarisXConst.ALL_MODULEID
                                                                                                , null
                                                                                                , ref workSheetData.AskData
                                                                                                , out remeasSequenceNo
                                                                                                , out remeasReceiptNumber
                                                                                                , out comment
                                                                                                , ref errorCollectionArg );
                            if (!targetFind)
                            {
                                //再検データがない場合

                                //Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
                                //    , String.Format("【チェック】一般検体登録情報に登録されているか                              RackID={0} Pos={1}", workSheetData.AskData.RackID, workSheetData.AskData.SamplePosition));

                                //一般検体登録情報に登録されているかチェック
                                targetFind = Singleton<SpecimenGeneralDB>.Instance.MeasureIndicate( asktype
                                                                                                  , CarisXConst.ALL_MODULEID
                                                                                                  , null
                                                                                                  , true
                                                                                                  , ref workSheetData.AskData
                                                                                                  , out remeasReceiptNumber
                                                                                                  , out comment
                                                                                                  , ref errorCollectionArg
                                                                                                  , rackinfoDetail.CupKind );

                                //Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
                                //    , String.Format("【チェック】一般検体登録情報の確認結果＝{2}                                 RackID={0} Pos={1}", workSheetData.AskData.RackID, workSheetData.AskData.SamplePosition, targetFind));

                                if (!targetFind)
                                {
                                    HostParameter hostParam = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter;

                                    if (hostParam.Enable && hostParam.UseRealtimeSampleAsk)
                                    {
                                        //ホストが有効で検体問合せも行う場合

                                        askHost = true;

                                        bool bdd = asktype == AskTypeKind.SampleID;

                                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
                                            , String.Format("Sample askHost==={0},command.AskType == AskTypeKind.SampleID==={1}", askHost.ToString(), bdd.ToString()));
                                    }
                                }
                            }

                            break;
                    }

                    if (askHost)
                    {
                        //ホスト問合せを行う場合

                        if (workSheetData.AskData.SampleID.Contains("ERROR") || workSheetData.AskData.SampleID.Contains("???"))
                        {
                            //検体IDが異常な場合は問合せしない
                            dbgMsg = dbgMsg + String.Format("askHost = true, but SampleID is {0} ", workSheetData.AskData.SampleID);
                        }
                        else
                        {
                            //検体IDが正常な場合はホスト問い合わせを行う
                            dbgMsg = dbgMsg + String.Format("askHost = true ");

                            flgAskHost = false;
                            this.askHost(respCommand.SampleType, workSheetData);

                            while (!flgAskHost)
                            {
                                Application.DoEvents();     //ホスト通信の結果が帰ってくるまで待機
                            }
                            flgAskHost = false;             //結果が帰ってきたので事故がないようにフラグを折っておく

                            //Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
                            //    , String.Format("【チェック】一般検体登録情報に登録されているか再確認                        RackID={0} Pos={1}", workSheetData.AskData.RackID, workSheetData.AskData.SamplePosition));

                            //リアルタイム問合せ中にバッチで受信出来ているかもしれないので、改めて一般検体登録情報に登録されているかチェック
                            targetFind = Singleton<SpecimenGeneralDB>.Instance.MeasureIndicate( asktype
                                                                                              , CarisXConst.ALL_MODULEID
                                                                                              , null
                                                                                              , true
                                                                                              , ref workSheetData.AskData
                                                                                              , out remeasReceiptNumber
                                                                                              , out comment
                                                                                              , ref errorCollectionArg
                                                                                              , workSheetData.AskData.SpecimenCup );

                            //Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
                            //    , String.Format("【チェック】一般検体登録情報の再確認結果＝{2}                               RackID={0} Pos={1}", workSheetData.AskData.RackID, workSheetData.AskData.SamplePosition, targetFind));

                            if (!targetFind)
                            {
                                //ホストからの受信データから問合せ結果を取得する
                                var data = askHostData.Where(v => v.AskData.RackID == workSheetData.AskData.RackID && v.AskData.SamplePosition == workSheetData.AskData.SamplePosition
                                    && v.AskData.SampleID == workSheetData.AskData.SampleID).FirstOrDefault();
                                if (data != null)
                                {
                                    dbgMsg = dbgMsg + String.Format("Found target data");
                                    targetFind = true;
                                    workSheetData.AskData.MeasItemArray = data.AskData.MeasItemArray;
                                }
                            }
                            else
                            {
                                //一般検体登録情報に増えてたので、ホスト受信データからは削除しておく
                                askHostData.RemoveAll(v => v.AskData.RackID == workSheetData.AskData.RackID && v.AskData.SamplePosition == workSheetData.AskData.SamplePosition
                                    && v.AskData.SampleID == workSheetData.AskData.SampleID);
                            }
                        }
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                    }

                    if (targetFind)
                    {
                        //分析予定を書き込みする場合

                        dbgMsg = dbgMsg + String.Format("Write to assay schedule ProtocolNo-ReagentLotNo:{0}"
                            , String.Join(",", workSheetData.AskData.MeasItemArray.Select(v => v.ProtoNo.ToString() + "-" + v.ReagentLotNo).ToArray()));
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);

                        Boolean setSchOK = false;
                        setSchOK = rackinfoDetail.SetMeasItemInAssaySchedule(rackinfo.RackId, workSheetData.AskData.MeasItemArray, preAssayModuleIdListForCalibrator);
                    }
                    else
                    {
                        //対象データが無かった場合
                        dbgMsg = dbgMsg + String.Format("No target data");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                    }
                }
                // 容器が設定されていない
                if (!posCheck)
                {

                    dbgMsg = dbgMsg + String.Format("[90-4]No rack position");
                    errorCollectionArg = (Int32)ErrorCollectKind.ContainerError;
                }
            }
            else
            {
                dbgMsg = dbgMsg + String.Format("No rack information");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
            }

            return;
        }

        /// <summary>
        /// ホストへのワークシート問い合わせ
        /// </summary>
        /// <remarks>
        /// ホストへのワークシート問い合わせを別スレッドから行います
        /// </remarks>
        /// <param name="kind"></param>
        /// <param name="indicate"></param>
        protected void askHost(SampleKind kind, AskWorkSheetData indicate)
        {
            // ホストへのワークシート問い合わせを別スレッドから行う。do from another thread worksheet inquiry to the host.
            // ホストからの応答はNotifyManagerを経由してメインスレッドにて処理する。Is treated by the main thread via the NotifyManager response from the host
            //Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
            //    , String.Format("【チェック】ホストへのワークシート問合せ（リアル）シーケンスを呼び出し      RackID={0} Pos={1}", indicate.AskData.RackID, indicate.AskData.SamplePosition));

            Singleton<CarisXSequenceHelperManager>.Instance.Host.HostCommunicationSequence
                (HostCommunicationSequencePattern.AskWorkSheetToHost, indicate);

            //Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
            //    , String.Format("【チェック】ホストへのワークシート問合せ（リアル）シーケンスを呼び出し完了  RackID={0} Pos={1}", indicate.AskData.RackID, indicate.AskData.SamplePosition));
        }


        #endregion


        ///////////////////////////////////////
        // スレーブ関連コマンド
        ///////////////////////////////////////
        #region [ [0502]測定指示データ問い合わせコマンド ]

        /// <summary>
        /// [0502]測定指示データ問い合わせコマンド解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onAskAssayData(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    SlaveCommCommand_0502 cmd0502 = arrayParam[0] as SlaveCommCommand_0502;

                    // 測定指示データ問い合わせコマンド解析処理コール
                    this.OnAskAssayData(cmd0502);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0502]測定指示データ問い合わせコマンド解析
        /// </summary>
        /// <param name="cmd0502"></param>
        protected void OnAskAssayData(SlaveCommCommand_0502 cmd0502)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(String.Format("OnAskAssayData"));
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                                CarisXLogInfoBaseExtention.Empty, String.Format("OnAskAssayData"));
                SlaveCommCommand_1502 respCommand = new SlaveCommCommand_1502();

                respCommand.RackID = cmd0502.RackID;
                respCommand.SamplePosition = cmd0502.SamplePosition;
                respCommand.ModuleID = CarisXSubFunction.MachineCodeToRackModuleIndex((MachineCode)cmd0502.CommNo);
                List<AssaySchedule> assaySchedules = null;

                if (respCommand.RackID.Substring(0, 1) == "E")
                {
                    //検体区分のみ設定（他項目は登録情報から取得・設定する）
                    respCommand.SampleType = SampleKind.Priority;
                }
                else
                {
                    //ラック情報、ラック情報詳細を取得する
                    var rackInfoList = Singleton<RackInfoManager>.Instance.RackInfo.Where(x => x.RackId.DispPreCharString == respCommand.RackID).Select(x => x);
                    if ((rackInfoList != null) && (rackInfoList.Count() > 0))
                    {
                        // ラック情報リストの先頭データ取得
                        RackInfo rackInfo = rackInfoList.First();

                        rackInfo.SampleMoveSource = cmd0502.SampleMoveSource;

                        var rackInfoDetailList = rackInfo.RackPosition.Where(x => x.PositionNo == (respCommand.SamplePosition)).Select(x => x);
                        if ((rackInfoDetailList != null) && (rackInfoDetailList.Count() > 0))
                        {
                            // ラック情報詳細リストの先頭データ取得
                            RackInfoDetail rackInfoDetail = rackInfoDetailList.First();

                            //検体区分、検体IDはラック情報、ラック情報詳細から取得・設定する
                            respCommand.SampleType = rackInfo.SampleKind;
                            respCommand.SampleID = rackInfoDetail.SampleID;
                            respCommand.SpecimenCup = rackInfoDetail.CupKind;

                            assaySchedules = rackInfoDetail.AssaySchedules;
                        }
                    }
                }

                //何をしているのか不明
                //// 一般検体･優先検体は分析方式による切替必要
                //if (respCommand.SampleType == SampleKind.Sample
                //    || respCommand.SampleType == SampleKind.Priority)
                //{
                //    respCommand.SampleID = command.SampleID;
                //}
                //else
                //{
                //    // キャリブレータ、分析項目は分析方式による切替不要
                //}

                //Caris200にあった問合せ方式を内部変数として持っておく。内容はシステムパラメータと同じとする
                AskTypeKind asktype = AskTypeKind.RackID;
                switch (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.AssayMode)
                {
                    case AssayModeParameter.AssayModeKind.SampleID:
                        asktype = AskTypeKind.SampleID;
                        break;
                }

                List<MeasItem> itemList = new List<MeasItem>();

                // DBから該当データを検索する。
                AskWorkSheetData workSheetData = new AskWorkSheetData();
                workSheetData.AskData = respCommand;
                workSheetData.ToDprCommand = respCommand;
                Boolean isSearcheable = true;

                HostParameter hostParam = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter;
                AssayModeParameter assayModeParam = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter;

                // システムパラメータ設定が、検体ID分析設定の場合、検索元情報のチェックを行う。
                if ((assayModeParam.AssayMode == AssayModeParameter.AssayModeKind.SampleID) && ((respCommand.SampleType == SampleKind.Sample)))
                {
                    isSearcheable = ValueChecker.IsCorrectMeasIndicateAskWhenSampleIDAssay(asktype, workSheetData.AskData.RackID);
                }

                Boolean find = false;
                Int32 remeasSequenceNo;
                String comment = String.Empty;
                Int32 remeasReceiptNumber;
                Int32 errorCode = 0;
                Int32 errorArg = 0;
                Int32 errorCollectionArg = 0;

                if (isSearcheable)
                {
                    switch (respCommand.SampleType)
                    {
                        case SampleKind.Sample: // 一般検体

                            // 再検査データ検索
                            // 当分の間検討されていない再テストリスト（重测列表，暂时不考虑）
                            find = Singleton<SpecimenReMeasureDB>.Instance.MeasureIndicate(asktype
                                                                                         , respCommand.ModuleID
                                                                                         , assaySchedules
                                                                                         , ref workSheetData.AskData
                                                                                         , out remeasSequenceNo
                                                                                         , out remeasReceiptNumber
                                                                                         , out comment
                                                                                         , ref errorCollectionArg);
                            if (find)
                            {
                                // 再検査データが見つかった場合、DBからは削除。
                                for (Int32 i = 0; i < workSheetData.AskData.MeasItemCount; i++)
                                {
                                    Singleton<SpecimenReMeasureDB>.Instance.DeleteData(workSheetData.AskData
                                                                                      .IndividuallyNumber
                                                                                      , workSheetData.AskData.MeasItemArray[i].ProtoNo);
                                }
                                Singleton<SpecimenReMeasureDB>.Instance.CommitSampleReMeasureInfo();

                                //関連インターフェースの更新（刷新相关的界面）
                                RealtimeDataAgent.LoadReMeasureSampleData();

                                // SpecimenAssayDBへ書き込む。
                                Singleton<SpecimenAssayDB>.Instance.AddAssayData(workSheetData, false, true, remeasSequenceNo, remeasReceiptNumber);
                                Singleton<SpecimenAssayDB>.Instance.CommitData();

                                // 分析中情報に追加
                                this.addInprocessSample(respCommand, remeasSequenceNo, remeasReceiptNumber, comment);
                                RealtimeDataAgent.LoadAssayData();
                            }
                            else
                            {
                                // 登録DB検索
                                // 測定指示データ問合せ&ユニーク番号発行
                                find = Singleton<SpecimenGeneralDB>.Instance.MeasureIndicate(asktype
                                                                                           , respCommand.ModuleID
                                                                                           , assaySchedules
                                                                                           , true
                                                                                           , ref workSheetData.AskData
                                                                                           , out remeasReceiptNumber
                                                                                           , out comment
                                                                                           , ref errorCollectionArg
                                                                                           , respCommand.SpecimenCup);
                                if (find)
                                {
                                    /* 検体識別番号生成 */
                                    respCommand.IndividuallyNumber = Singleton<IndividuallyNo>.Instance.CreateNumber();

                                    // SpecimenAssayDBへ書き込む。
                                    Singleton<SpecimenAssayDB>.Instance.AddAssayData(workSheetData, false, false);
                                    Singleton<SpecimenAssayDB>.Instance.CommitData();

                                    // TODO：複数モジュール時に分析可能な試薬情報のみ削除する？
                                    //        すべての試薬情報が分析となった時点で消す？
                                    // SpecimenGeneralDBからは削除する。
                                    Singleton<SpecimenGeneralDB>.Instance.DeleteData(workSheetData.AskData);
                                    Singleton<SpecimenGeneralDB>.Instance.CommitSampleInfo();

                                    RealtimeDataAgent.LoadSampleData();

                                    // SpecimenGeneralDBでは分析中状態とする。
                                    // 分析中情報に追加
                                    this.addInprocessSample(respCommand, Singleton<SequencialSampleNo>.Instance.Number, remeasReceiptNumber, comment);
                                    RealtimeDataAgent.LoadAssayData();
                                }
                                else if (hostParam.Enable && hostParam.UseRealtimeSampleAsk)
                                {
                                    //ホスト問合せを行う場合

                                    //ホストからの受信データから問合せ結果を取得する
                                    var data = askHostData.Where(v => v.AskData.RackID == workSheetData.AskData.RackID && v.AskData.SamplePosition == workSheetData.AskData.SamplePosition
                                        && v.AskData.SampleID == workSheetData.AskData.SampleID).FirstOrDefault();
                                    if (data != null)
                                    {
                                        //分析予定にあるもののみ設定
                                        List<MeasItem> measItemList = new List<MeasItem>();
                                        List<MeasItem> measItemOutList = new List<MeasItem>();
                                        foreach (var measItem in data.AskData.MeasItemArray)
                                        {
                                            MeasItem item = new MeasItem();
                                            item.AutoDil = measItem.AutoDil; // 後希釈倍率
                                            item.ProtoNo = measItem.ProtoNo;
                                            item.RepCount = measItem.RepCount;
                                            item.TurnNo = measItem.TurnNo;
                                            item.UniqNo = measItem.UniqNo;
                                            item.ReagentLotNo = Singleton<ReagentDB>.Instance.GetNowReagentLotNo(item.ProtoNo, moduleId: respCommand.ModuleID);// 試薬ロット番号                         

                                            if (item.ReagentLotNo != String.Empty)
                                            {
                                                if (assaySchedules == null
                                                    || (assaySchedules != null && assaySchedules.Count != 0 && assaySchedules.Exists(v => v.ExistsSchedule(item.ProtoNo, String.Empty))))
                                                {
                                                    //Rack No Use（分析予定がNull）、または分析予定がある場合は登録する

                                                    item.UniqNo = Singleton<UniqueNo>.Instance.CreateNumber(); /* ユニーク番号生成 */
                                                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, ""
                                                        , String.Format("【検体追加】 Unique number = {0} Rack = {1}-{2} ID = {3}", item.UniqNo, data.AskData.RackID, data.AskData.SamplePosition, data.AskData.SampleID));

                                                    measItemList.Add(item);
                                                }
                                                else
                                                {
                                                    //分析予定にない場合
                                                    measItemOutList.Add(item);
                                                }
                                            }
                                            else
                                            {
                                                //試薬がない場合
                                                measItemOutList.Add(item);
                                            }
                                        }
                                        workSheetData.AskData.MeasItemCount = measItemList.Count;
                                        workSheetData.AskData.MeasItemArray = measItemList.ToArray();

                                        if (workSheetData.AskData.MeasItemCount != 0)
                                        {
                                            workSheetData.AskData.PreDil = data.AskData.PreDil;
                                            workSheetData.AskData.SpecimenMaterial = data.AskData.SpecimenMaterial;
                                            workSheetData.FromHostCommand = data.FromHostCommand;

                                            /* 検体識別番号生成 */
                                            workSheetData.AskData.IndividuallyNumber = Singleton<IndividuallyNo>.Instance.CreateNumber();

                                            // SpecimenAssayDBへ書き込む。
                                            Singleton<SpecimenAssayDB>.Instance.AddAssayData(workSheetData, true, false, 0, data.FromHostCommand.ReceiptNumber);
                                            Singleton<SpecimenAssayDB>.Instance.CommitData();

                                            // 分析中情報に追加
                                            this.addInprocessSample(workSheetData.AskData, Singleton<SequencialSampleNo>.Instance.Number, data.FromHostCommand.ReceiptNumber, data.FromHostCommand.Comment);
                                            RealtimeDataAgent.LoadAssayData();
                                        }

                                        if (measItemOutList.Count == 0)
                                        {
                                            askHostData.Remove(data);
                                        }
                                        else
                                        {
                                            askHostData.Find(v => v == data).AskData.MeasItemCount = measItemOutList.Count;
                                            askHostData.Find(v => v == data).AskData.MeasItemArray = measItemOutList.ToArray();
                                        }
                                    }
                                }
                            }
                            break;

                        case SampleKind.Priority: // STAT検体

                            // 再検査データ検索
                            find = Singleton<SpecimenStatReMeasureDB>.Instance.MeasureIndicate
                                (asktype, respCommand.ModuleID, ref workSheetData.AskData, out remeasSequenceNo, out remeasReceiptNumber, out comment);
                            if (find)
                            {
                                // 再検査データが見つかった場合、DBからは削除。
                                for (Int32 i = 0; i < workSheetData.AskData.MeasItemCount; i++)
                                {
                                    Singleton<SpecimenStatReMeasureDB>.Instance.DeleteData(workSheetData.AskData
                                                                                      .IndividuallyNumber
                                                                                      , workSheetData.AskData.MeasItemArray[i].ProtoNo);
                                }
                                Singleton<SpecimenStatReMeasureDB>.Instance.CommitSampleReMeasureInfo();

                                //関連インターフェースの更新（刷新相关的界面）
                                RealtimeDataAgent.LoadReMeasureSampleData();

                                // SpecimenAssayDBへ書き込む。
                                Singleton<SpecimenAssayDB>.Instance.AddAssayData(workSheetData, false, true, remeasSequenceNo, remeasReceiptNumber);
                                Singleton<SpecimenAssayDB>.Instance.CommitData();

                                // 分析中情報に追加
                                this.addInprocessSample(respCommand, remeasSequenceNo, remeasReceiptNumber, comment);
                                RealtimeDataAgent.LoadAssayData();
                            }
                            else
                            {
                                // STAT登録データ検索
                                RegistType registType = RegistType.Temporary;

                                // 測定指示データ問合せ&ユニーク番号発行
                                find = Singleton<SpecimenStatDB>.Instance.MeasureIndicate( asktype
                                                                                         , ref workSheetData.AskData
                                                                                         , out remeasReceiptNumber
                                                                                         , out comment
                                                                                         , respCommand.ModuleID
                                                                                         , out registType);
                                if (find)
                                {
                                    /* 検体識別番号生成 */
                                    respCommand.IndividuallyNumber = Singleton<IndividuallyNo>.Instance.CreateNumber();
                                    respCommand.SpecimenCup = workSheetData.AskData.SpecimenCup;
                                    respCommand.SampleID = workSheetData.AskData.SampleID;

                                    // SpecimenAssayDBへ書き込む。
                                    Singleton<SpecimenAssayDB>.Instance.AddPriorityAssayData(workSheetData, false, false);
                                    Singleton<SpecimenAssayDB>.Instance.CommitData();

                                    // SpecimenStatDBから一時登録の場合のみ削除する。
                                    if (registType == RegistType.Temporary)
                                    {
                                        Singleton<SpecimenStatDB>.Instance.DeleteData(new List<Tuple<string, int, string>>() {
                                                                    new Tuple<string, int, string>(respCommand.RackID, 0, respCommand.SampleID) });
                                        Singleton<SpecimenStatDB>.Instance.CommitSampleInfo();
                                    }
                                    else if(registType == RegistType.Fixed)
                                    {
                                        //【IssuesNo:14】修复STAT固定注册信息中的ReceiptNo在使用过后没有自动更新的问题
                                        Singleton<SpecimenStatDB>.Instance.UpdateReceiptNo(new List<Tuple<string, int, string>>() {
                                                                    new Tuple<string, int, string>(respCommand.RackID, 0, respCommand.SampleID) });
                                        Singleton<SpecimenStatDB>.Instance.CommitSampleInfo();

                                    }

                                    RealtimeDataAgent.LoadStatData();

                                    // 分析中情報に追加
                                    this.addInprocessSample(respCommand, Singleton<SequencialPrioritySampleNo>.Instance.Number, remeasReceiptNumber, comment);
                                    RealtimeDataAgent.LoadAssayData();
                                }
                                //STATの場合、リアルタイム問合せは出来ない
                            }

                            break;

                        case SampleKind.Calibrator: // キャリブレータ

                            // 仮入れ物（ここでは使用することはない）
                            List<RackModuleIndex> tempModuleNoList = new List<RackModuleIndex>();

                            // キャリブレータ登録DB検索
                            find = Singleton<CalibratorRegistDB>.Instance.MeasureIndicate( respCommand.ModuleID
                                                                                         , assaySchedules
                                                                                         , ref workSheetData.AskData
                                                                                         , ref errorArg
                                                                                         , ref tempModuleNoList );

                            if (find)
                            {
                                /* 検体識別番号生成 */
                                respCommand.IndividuallyNumber = Singleton<IndividuallyNo>.Instance.CreateNumber();

                                // 測定指示データ問合せ&ユニーク番号発行
                                Int32 calibSequenceNo = 0; // キャリブレータのシーケンス番号は、同一項目に対してラック投入順不動で統一されるようにする
                                string conc = HybridDataMediator.SearchConcentrationFromCalibratorRegistDB(respCommand.ModuleID, workSheetData.AskData.RackID, workSheetData.AskData.SamplePosition);

                                // SpecimenAssayDBへ書き込む。
                                Singleton<CalibratorAssayDB>.Instance.AddAssayData(respCommand, out calibSequenceNo, conc);
                                Singleton<CalibratorAssayDB>.Instance.CommitData();

                                // 分析中情報に追加
                                this.addInprocessSample(respCommand, calibSequenceNo);

                                RealtimeDataAgent.LoadAssayData();
                            }
                            else//キャリブレーションレジストリに見つからない場合は、まず登録してから関連データを送信してください（没有在校准注册表里找到，先注册，然后再发送相关的数据）
                            {
                                Double conc = 0;

                                find = this.measureIndicateForSampleId(respCommand.ModuleID, assaySchedules, ref workSheetData.AskData, ref conc, ref errorCode, ref errorArg, ref errorCollectionArg);
                                if (find)
                                {
                                    respCommand.IndividuallyNumber = Singleton<IndividuallyNo>.Instance.CreateNumber();

                                    // 測定指示データ問合せ&ユニーク番号発行
                                    Int32 calibSequenceNo = 0; // キャリブレータのシーケンス番号は、同一項目に対してラック投入順不動で統一されるようにする

                                    // SpecimenAssayDBへ書き込む。
                                    Singleton<CalibratorAssayDB>.Instance.AddAssayData(respCommand, out calibSequenceNo, conc.ToString());
                                    Singleton<CalibratorAssayDB>.Instance.CommitData();

                                    // 分析中情報に追加
                                    this.addInprocessSample(respCommand, calibSequenceNo);

                                    RealtimeDataAgent.LoadAssayData();
                                }
                                else
                                {
                                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog
                                                                              , Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                                                                              , CarisXLogInfoBaseExtention.Empty
                                                                              , String.Format("Calibarator Lot number is empty"));
                                }
                            }
                            break;

                        case SampleKind.Control: // 精度管理検体

                            // コントロール登録DB検索
                            // 測定指示データ問合せ&ユニーク番号発行
                            find = Singleton<ControlRegistDB>.Instance.MeasureIndicate(respCommand.ModuleID, assaySchedules, ref workSheetData.AskData, ref errorCode, ref errorArg, ref errorCollectionArg);
                            if (find)
                            {
                                // ControlAssayDBへ書き込む。
                                // ControlDBでは分析中状態とする。
                                /* 検体識別番号生成 */
                                respCommand.IndividuallyNumber = Singleton<IndividuallyNo>.Instance.CreateNumber();

                                // ControlAssayDBへ書き込む。
                                Singleton<ControlAssayDB>.Instance.AddAssayData(respCommand);
                                Singleton<ControlAssayDB>.Instance.CommitData();

                                // 分析中情報に追加
                                this.addInprocessSample(respCommand, Singleton<SequencialControlNo>.Instance.Number);

                                RealtimeDataAgent.LoadAssayData();
                            }
                            else
                            {
                                //エラーはラック移動問合せ時のみ出力する
                            }
                            break;
                        default:
                            break;
                    }
                }

                //ラック情報に検査項目の情報を保持する
                if (respCommand.SampleType != SampleKind.Priority)
                {
                    Singleton<RackInfoManager>.Instance.SetAssayStarted(workSheetData.AskData);
                }

                // 応答コマンド送信
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((MachineCode)cmd0502.CommNo, respCommand);
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// 分析中検体情報追加
        /// </summary>
        /// <remarks>
        /// 測定指示データ問合せコマンド応答のタイミングで、検体の情報が確定した時に呼び出されます。
        /// </remarks>
        /// <param name="measIndcate">検体情報</param>
        /// <param name="receiptNumber">受付番号</param>
        /// <param name="sequenceNo">シーケンス番号(一般、優先、キャリブレータ、制度管理)</param>
        /// <param name="comment">コメントデータ</param>
        private void addInprocessSample(IMeasureIndicate measIndcate, Int32 sequenceNo, Int32 receiptNumber = 0, String comment = "")
        {
            if (measIndcate.SampleType == SampleKind.Sample || (measIndcate.SampleType == SampleKind.Priority))
            {
                foreach (MeasItem measItem in measIndcate.MeasItemArray)
                {
                    SampleInfo sampleInfo = Singleton<InProcessSampleInfoManager>.Instance.CreateSampleInfo(measIndcate.SampleType);
                    sampleInfo.IndividuallyNumber = measIndcate.IndividuallyNumber;
                    sampleInfo.SequenceNumber = sequenceNo;
                    sampleInfo.RackId = measIndcate.RackID;
                    sampleInfo.RackPos = measIndcate.SamplePosition;
                    sampleInfo.SampleId = measIndcate.SampleID;
                    sampleInfo.SampleKind = measIndcate.SampleType;
                    sampleInfo.ModuleID = measIndcate.ModuleID;

                    // 検体番号とコメントは、一般・優先検体でのみ値が設定される
                    sampleInfo.ReceiptNumber = receiptNumber;
                    sampleInfo.Comment = comment;

                    var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(measItem.ProtoNo);
                    Int32 repCount = 1;
                    repCount = measItem.RepCount;
                    sampleInfo.AddMeasureProtocol(protocol.ProtocolIndex, measItem.UniqNo, repCount);
                    sampleInfo.SetMeasureProtocolStatus(SampleInfo.SampleMeasureStatus.Wait);
                    Singleton<InProcessSampleInfoManager>.Instance.AddSampleInfo(sampleInfo);
                }
            }
            else
            {
                // 分析項目集合をReagentLotで分割する            
                var measGroupedFromReagentLot = from v in measIndcate.MeasItemArray
                                                group v by v.ReagentLotNo;
                foreach (var measGroup in measGroupedFromReagentLot)
                {
                    SampleInfo sampleInfo = Singleton<InProcessSampleInfoManager>.Instance.CreateSampleInfo(measIndcate.SampleType);
                    sampleInfo.IndividuallyNumber = measIndcate.IndividuallyNumber;
                    sampleInfo.SequenceNumber = sequenceNo;
                    sampleInfo.RackId = measIndcate.RackID;
                    sampleInfo.RackPos = measIndcate.SamplePosition;
                    sampleInfo.SampleId = measIndcate.SampleID;
                    sampleInfo.SampleKind = measIndcate.SampleType;
                    sampleInfo.ModuleID = measIndcate.ModuleID;

                    // 検体番号とコメントは、一般・優先検体でのみ値が設定される
                    sampleInfo.ReceiptNumber = receiptNumber;
                    sampleInfo.Comment = comment;

                    foreach (var meas in measGroup)
                    {
                        var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(meas.ProtoNo);
                        Int32 repCount = 1;
                        switch (measIndcate.SampleType)
                        {
                            case SampleKind.Sample:
                            case SampleKind.Priority:
                                repCount = meas.RepCount;
                                break;
                            case SampleKind.Calibrator:
                                repCount = protocol.RepNoForCalib;
                                break;
                            case SampleKind.Control:
                                repCount = protocol.RepNoForControl;
                                break;
                            default:
                                break;
                        }
                        sampleInfo.AddMeasureProtocol(protocol.ProtocolIndex, meas.UniqNo, repCount);
                    }
                    sampleInfo.SetMeasureProtocolStatus(SampleInfo.SampleMeasureStatus.Wait);
                    Singleton<InProcessSampleInfoManager>.Instance.AddSampleInfo(sampleInfo);
                }
            }

            // 発番履歴
            switch (measIndcate.SampleType)
            {
                case SampleKind.Sample:
                    Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleSequenceNumberHistory.AddHistory(measIndcate.IndividuallyNumber, false);
                    break;
                case SampleKind.Priority:
                    Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleSequenceNumberHistory.AddHistory(measIndcate.IndividuallyNumber, true);
                    break;
                default:
                    break;
            }

            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleSequenceNumberHistory.Distinct();
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Save();

        }

        /// <summary>
        /// 測定指示の取得（キャリブレータ情報から）
        /// </summary>
        /// <param name="moduleId">モジュールID（全モジュールも対象）</param>
        /// <param name="assaySchedules">分析予定</param>
        /// <param name="data">測定指示データ</param>
        /// <param name="conc">濃度</param>
        /// <param name="errorCode">エラーコード</param>
        /// <param name="errorArg">エラー引数</param>
        /// <param name="errorCollectionArg">回収エラー引数</param>
        /// <param name="preModuleNoList">仮モジュール番号リスト</param>
        /// <returns></returns>
        private Boolean measureIndicateForSampleId(int moduleId
                                                 , List<AssaySchedule> assaySchedules
                                                 , ref IMeasureIndicate data
                                                 , ref Double conc
                                                 , ref int errorCode
                                                 , ref int errorArg
                                                 , ref int errorCollectionArg)
        {
            String dbgMsgHead = String.Format("[[Investigation log]]measureIndicateForSampleId ");
            String dbgMsg = dbgMsgHead;

            Boolean rtnVal = false;
            conc = -1;
            Int32 reagentCode = 0;
            String calibLot = String.Empty;
            String reagentLot = String.Empty;
            Double concentration = -1;
            Int32 concIdx = 0;

            if (data.SampleID.Length >= 10)
            {
                //検体IDを分解する（000:試薬コード(プロトロコル番号) 000000:CalibratorLot 0:ConcIndex）
                reagentCode = int.Parse(data.SampleID.Substring(0, 3));
                calibLot = data.SampleID.Substring(3, 6).Trim();
                concIdx = int.Parse(data.SampleID.Substring(9, 1));
            }

            if (calibLot.Length >= 5)
            {
                //CalibratorLotが5文字以上の場合

                //キャリブレータ情報の内、キャリブレータロットNoがCalibratorLotと一致する内容をモジュールID→ポート番号順に参照
                var calibinfos = Singleton<CalibratorInfoManager>.Instance.CalibratorLot
                    .Where(v => (v.ModuleId == moduleId || moduleId == CarisXConst.ALL_MODULEID)
                        && v.ReagentCode == reagentCode
                        && v.CalibratorLot.Exists(vv => vv.CalibratorLotNo == calibLot))
                    .OrderBy(v => v.ModuleId).ThenBy(v => v.PortNo);
                foreach (var calibinfo in calibinfos)
                {
                    //試薬情報からキャリブレータ情報のポート番号、試薬コードと合致するM試薬を取得
                    var reagent = Singleton<ReagentDB>.Instance.GetReagentData(calibinfo.PortNo, calibinfo.ReagentCode, ReagentTypeDetail.M, calibinfo.ModuleId);
                    if (reagent == null)
                    {
                        //M試薬が参照できない場合は次のポートを処理
                        continue;
                    }

                    //M試薬の残量が300uL以上あるかチェック
                    if (reagent.Remain < 300)
                    {
                        //300uL未満の場合、次のポートを処理
                        continue;
                    }

                    //条件に合致する内容の内、一番大きいM試薬のロット番号の場合だけ適用
                    if (String.Compare(reagent.LotNo, reagentLot) > 0)
                    {
                        data.SampleID = calibLot;
                        reagentLot = reagent.LotNo;

                        //キャリブレータ情報の補正ポイントの内、ConcIndex-1と一致する位置にある値を取得
                        var concList = calibinfo.CalibratorLot.Where(v => v.CalibratorLotNo == calibLot && v.ConcCount >= (concIdx - 1))
                            .Select(v => v.Concentration).FirstOrDefault();
                        if (concList != null)
                        {
                            concentration = concList[concIdx - 1];
                        }
                    }
                }

                if (reagentLot != String.Empty && concentration != -1)
                {
                    //M試薬の試薬ロット番号を取得、補正ポイントも取得出来た場合
                    conc = concentration;

                    //ワークシートデータを作成
                    data.PreDil = 1; // 手希釈倍率 キャリブレータは固定で1(定数定義へ移動する)
                    data.SpecimenMaterial = SpecimenMaterialType.BloodSerumAndPlasma; // サンプル種別 精度管理検体は固定で血

                    // 問い合わせコマンド応答用情報
                    List<MeasItem> measItemList = new List<MeasItem>();
                    MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(reagentCode);    //Caris200の時からなぜかコマンドで受けた試薬コードをプロトコルインデックスに当てている
                    MeasItem item = new MeasItem();
                    item.AutoDil = 1; // 後希釈倍率 精度管理検体は固定で1（定数定義へ移動する）
                    item.ProtoNo = protocol.ProtocolNo;
                    item.RepCount = protocol.RepNoForCalib;
                    item.TurnNo = Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.GetTurnOrder(protocol.ProtocolName);
                    item.UniqNo = 1;
                    item.ReagentLotNo = reagentLot;// 指定の試薬ロット番号

                    if (moduleId == CarisXConst.ALL_MODULEID)
                    {
                        //全モジュールを対象に処理した場合

                        item.ReagentLotNo = String.Empty;   //ロット番号が正しいかわからないので空白扱いにする
                        measItemList.Add(item);
                    }
                    else
                    {
                        //特定のモジュールを対象に処理した場合

                        if (assaySchedules == null
                            || (assaySchedules != null && assaySchedules.Count != 0 && assaySchedules.Exists(v => v.ExistsSchedule(item.ProtoNo, String.Empty))))
                        {
                            //Rack No Use（分析予定がNull）、または分析予定がある場合は登録する

                            item.UniqNo = Singleton<UniqueNo>.Instance.CreateNumber(); /* ユニーク番号生成 */
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("【検体追加】 UniqNo = {0} Rack = {1}-{2} ID = {3}", item.UniqNo, data.RackID, data.SamplePosition, data.SampleID));

                            measItemList.Add(item);
                        }
                    }

                    if (measItemList.Count != 0)
                    {
                        // 測定項目を応答データに設定
                        data.MeasItemCount = measItemList.Count;
                        data.MeasItemArray = measItemList.ToArray();
                        rtnVal = true;  // 検索結果あり

                        dbgMsg = dbgMsg + String.Format("Found matching data reagentCode=>{0} reagentLot=>{1} concentration=>{2}", reagentCode, reagentLot, concentration);
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                    }
                    else
                    {
                        dbgMsg = dbgMsg + String.Format("No matching data reagentCode=>{0} reagentLot=>{1} concentration=>{2}", reagentCode, reagentLot, concentration);
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                    }
                }
                else
                {
                    //M試薬の試薬ロット番号が取得出来ていない、補正ポイントが取得出来ていない場合はエラー202-1

                    dbgMsg = dbgMsg + String.Format("No matching data reagentCode=>{0} calibrationLot=>{1}", reagentCode, calibLot);
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);

                    errorCode = 202;
                    errorArg = 1;

                    // 試薬情報がない場合場合
                    if (reagentLot != String.Empty)
                    {
                        errorCollectionArg = (Int32)ErrorCollectKind.ReagentError;
                        dbgMsg = dbgMsg + String.Format("[90-2]Insufficient reagent or no reagent information.");
                    }
                    // 登録がない場合
                    else
                    {
                        errorCollectionArg = (Int32)ErrorCollectKind.RegisterError;
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", "[90-3]There is no registration information.");
                    }
                }
            }
            else
            {
                dbgMsg = dbgMsg + String.Format("No matching data sampleID=>{0}", data.SampleID);
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
            }

            return rtnVal;
        }

        /// <summary>
        /// ホストからのワークシート受信イベント
        /// </summary>
        /// <remarks>
        /// ホストからのワークシート受信イベントを実行します
        /// </remarks>
        /// <param name="data"></param>
        public void WorkSheetFromHost(AskWorkSheetData data)
        {
            String dbgMsgHead = String.Format("[[Investigation log]]workSheetFromHost ");
            String dbgMsg = dbgMsgHead;

            AskWorkSheetData askData = data as AskWorkSheetData;

            //Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
            //    , String.Format("【チェック】HostWorkSheetメッセージを処理                                   RackID={0} Pos={1} ReciptNo={2}", askData.FromHostCommand.RackID, askData.FromHostCommand.RackPos, askData.FromHostCommand.ReceiptNumber));

            // タイムアウトしている場合は処理しない
            if (askData.AskTimeOuted)
            {
                dbgMsg = dbgMsg + "AskTimeOuted ";
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                flgAskHost = true;
                return;
            }

            //【IssuesNo:3】如果样本ID不一致不处理
            if(askData.AskSampleIDIsDifferent)
            {
                dbgMsg = dbgMsg + "AskSampleIDIsDifferernt";
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                flgAskHost = true;
                return;
            }

            // 測定項目がない場合は処理しない
            if (askData.FromHostCommand.NumOfMeasItem == 0)
            {
                dbgMsg = dbgMsg + "NumOfMeasItem == 0 ";
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                flgAskHost = true;
                return;
            }

            // 装置番号エラー
            Boolean isValid = ValueChecker.IsValidSpecimenWorkSheet(askData.FromHostCommand, HostSampleType.N);

            // ・サンプルID、ラックID、ラックPosどれも無い場合エラー
            if (askData.FromHostCommand.SampleID == ""
                && askData.FromHostCommand.RackID == ""
                && (askData.FromHostCommand.RackPos < 1 || askData.FromHostCommand.RackPos > 5))
            {
                CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.FromHostWorkSheetFormatError
                    , extStr: String.Format(Properties.Resources.STRING_LOG_MSG_081, askData.FromHostCommand.RackID, askData.FromHostCommand.RackPos));

                dbgMsg = dbgMsg + "Sample ID, rack ID, and rack position are not all set. ";
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                flgAskHost = true;
                return;
            }

            // ホストから受信したWSを退避する
            if (isValid && askData.FromHostCommand.NumOfMeasItem != 0)
            {
                // データあり
                // データを分析DBに登録し、スレーブに送信

                // 外部からの受付番号を保持する。
                if (Singleton<ReceiptNo>.Instance.ThroughExternalCreatedNumber(askData.FromHostCommand.ReceiptNumber))
                {
                    // ホストからのデータを移す。
                    if (this.setWorkSheetFromHost(askData.FromHostCommand, ref askData.AskData))
                    {
                        dbgMsg = dbgMsg + "Move data from the host. ";
                        askHostData.Add(askData);
                    }
                    else
                    {
                        dbgMsg = dbgMsg + "Data from the host could not be transferred. ";
                    }
                }
                else
                {
                    // 既に登録されたことのある受付番号がホストから指定された    
                    CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.FromHostWorkSheetFormatError, extStr: Properties.Resources.STRING_LOG_MSG_082);
                    dbgMsg = dbgMsg + "The host specified a receipt number that has already been registered. ";
                }
            }
            else
            {
                // データなし
                dbgMsg = dbgMsg + "No data found";
            }

            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
            flgAskHost = true;
        }

        /// <summary>
        /// ホストから受信したワークシートを設定
        /// </summary>
        /// <remarks>
        /// ホストから受信したワークシートを設定します
        /// </remarks>
        /// <param name="command"></param>
        /// <param name="indicate"></param>
        protected bool setWorkSheetFromHost(HostCommCommand_0002 command, ref IMeasureIndicate indicate)
        {
            indicate.SpecimenMaterial = command.SampleMaterialType.GetDPRSpecimemMaterialType();
            indicate.PreDil = command.ManualDil;
            List<MeasItem> measItemList = new List<MeasItem>();
            for (int i = 0; i < command.NumOfMeasItem; i++)
            {
                MeasItem item = new MeasItem();

                // ここでのプロトコル番号不一致による検索Nullは発生しない。
                MeasureProtocol protocol = Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.GetDPRProtocolFromHostProtocolNumber(command.MeasItems[i].protoNo, Singleton<MeasureProtocolManager>.Instance);
                if (protocol == null)
                {
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("【运行不能Error】 Host protocol number = {0} has not been detected。", command.MeasItems[i].protoNo));
                    continue;
                }

                // 試薬の急診使用有の場合
                if (protocol.UseEmergencyMode == true)
                {
                    // 急診使用ありの場合は希釈倍率を1に固定する
                    item.AutoDil = 1;
                }
                // 試薬の急診使用無の場合
                else
                {
                    // 後希釈倍率はHOSTから指定される。
                    item.AutoDil = command.MeasItems[i].afterDil.GetDPRAutoDilution();
                }

                if (protocol.IsIGRA)
                {
                    if (indicate.SamplePosition < 5)
                    {
                        SpecimenRegistrationGridViewDataSet dataset = new SpecimenRegistrationGridViewDataSet();
                        dataset.PatientID = indicate.SampleID;
                        dataset.RackID = indicate.RackID;//是从样本架拿到的 架子号
                                                         // dataset.RackPosition = indicate.RackPos + 1;
                        dataset.RackPosition = 2;//第二个位置为起始位置。
                        //【IssuesNo:13】
                        if (command.ReceiptNumber == 0 || (Singleton<ReceiptNo>.Instance.StartCount <= command.ReceiptNumber && command.ReceiptNumber <= Singleton<ReceiptNo>.Instance.Number))
                        {
                            dataset.ReceiptNumber = Singleton<ReceiptNo>.Instance.CreateNumber();

                        }
                        else
                        {
                            dataset.ReceiptNumber = command.ReceiptNumber;
                        }

                        dataset.SpecimenType = command.SampleMaterialType.GetDPRSpecimemMaterialType();
                        dataset.Registered.Add(new Tuple<Int32?, Int32?, Int32?>(protocol.ProtocolNo, item.AutoDil, protocol.RepNoForSample));
                        dataset.ManualDil = command.ManualDil;
                        dataset.Comment = command.Comment;
                        Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)(NotifyKind.HostWorkSheetSingle), dataset);
                    }
                    measItemList.Clear();
                    // 測定項目を応答データに設定
                    indicate.MeasItemCount = measItemList.Count;
                    indicate.MeasItemArray = measItemList.ToArray();
                    return false;
                }
                else
                {
                    item.ProtoNo = protocol.ProtocolNo;
                    item.RepCount = protocol.RepNoForSample;
                    item.TurnNo = Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.GetTurnOrder(protocol.ProtocolName);
                    item.UniqNo = 0;    // この段階の情報は分析予定に登録するのでユニーク番号は振れない
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("【検体追加】 Unique number = {0} Rack = {1}-{2} ID = {3}", item.UniqNo, indicate.RackID, indicate.SamplePosition, indicate.SampleID));

                    // この段階の情報は分析予定に登録するので、まだ試薬ロットは取れない。
                    item.ReagentLotNo = String.Empty;
                    measItemList.Add(item);
                }

            }

            // 手希釈倍率が設定されている場合、登録されている測定項目の中に手希釈倍率不使用のものがあればエラーとなる。
            Boolean allowPreDil = true;
            if (indicate.PreDil > 1)
            {
                // 登録されている分析項目全体に対して手希釈倍率不使用が見つからなかった場合、true
                allowPreDil = ((from v in measItemList
                                select Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(v.ProtoNo))
                                    .FirstOrDefault((protocol) => protocol.UseManualDil == false) == null);
                if (!allowPreDil)
                {
                    // TODO:エラーメッセージ

                    // 空にする。
                    measItemList.Clear();
                }
            }

            // 測定項目を応答データに設定
            indicate.MeasItemCount = measItemList.Count;
            indicate.MeasItemArray = measItemList.ToArray();

            return true;
        }

        #endregion

        #region [ [0503]測定データコマンド ]

        /// <summary>
        /// [0503]測定データコマンド解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onAssayData(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    SlaveCommCommand_0503 cmd0503 = arrayParam[0] as SlaveCommCommand_0503;

                    // 測定データコマンド解析処理コール
                    this.OnAssayData(cmd0503);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0503]測定データコマンド解析
        /// </summary>
        /// <param name="cmd0503"></param>
        protected void OnAssayData(SlaveCommCommand_0503 cmd0503)
        {
            try
            {
                //モジュールIDを設定
                cmd0503.ModuleID = CarisXSubFunction.MachineCodeToRackModuleIndex((MachineCode)cmd0503.CommNo);

                // TODO:測定データコマンド処理
                // 分析項目の測定完了時に送信される。
                SampleInfo sampleInfo = Singleton<InProcessSampleInfoManager>.Instance.SearchInProcessSampleFromUniqueNo(cmd0503.UniqueNo);
                if (sampleInfo == null)
                {
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, ""
                        , String.Format("Measurement data command、invalid sample number :Individually={0} Rack-Pos={1}-{2} Uniq = {3} rep = {4}"
                        , cmd0503.IndividuallyNumber, cmd0503.RackID, cmd0503.SamplePos, cmd0503.UniqueNo, cmd0503.RepNo));
                    return;
                }

                // 調査用ログ
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, ""
                    , String.Format("【Measurement data command】Individually={0} Rack-Pos={1}-{2} Uniq = {3} rep = {4}"
                    , cmd0503.IndividuallyNumber, cmd0503.RackID, cmd0503.SamplePos, cmd0503.UniqueNo, cmd0503.RepNo));

                // 分析残り時間0を設定
                this.setAssayRemainTime(cmd0503.UniqueNo, cmd0503.RepNo, sampleInfo, TimeSpan.FromSeconds(0));

                List<AnalysisErrorInfo> errorList;
                Int32 repCount = sampleInfo.ProtocolStatusDictionary[cmd0503.MeasProtocolNumber].Count();
                var data = ((IMeasureResultData)cmd0503).Convert(true);
                CalcData aveData;
                CarisXCalculator.Calc(CarisXSubFunction.MachineCodeToModuleIndex((MachineCode)cmd0503.CommNo), cmd0503.DarkCount, cmd0503.BGCount, cmd0503.ResultCount
                    , cmd0503.SampleKind, repCount, ref data, out errorList, out aveData);

                // エラー履歴追加
                foreach (AnalysisErrorInfo error in errorList)
                {
                    if (error.ErrorCode != 0)
                    {
                        DPRErrorCode errCode = new DPRErrorCode(error.ErrorCode, error.ErrorArg, cmd0503.ModuleID);

                        // エラー履歴に登録
                        CarisXSubFunction.WriteDPRErrorHist(errCode, 0, (error.ErrorDetailInfoParam.Count > 0 ? error.ErrorDetailInfoParam[0] : String.Empty));
                    }
                }

                MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(cmd0503.MeasProtocolNumber);
                sampleInfo.SetMeasureProtocolStatus(protocol.ProtocolIndex, cmd0503.RepNo, SampleInfo.SampleMeasureStatus.End);

                // 測定時刻保持を追加
                sampleInfo.SetMeasureEndTime(protocol.ProtocolIndex, cmd0503.RepNo, data.MeasureDateTime);

                Boolean isAutoRemeasure = false;
                Boolean isAutoDilRemeasure = false;
                Boolean isOnlyRemeasureAdd = false;
                Tuple<Int32, Int32> dataKey = new Tuple<int, int>(cmd0503.IndividuallyNumber, cmd0503.MeasProtocolNumber);

                // 分析残り時間0を設定(ラック表示更新の為、計算後にも呼び出す（計算前の呼び出しも必要）)
                this.setAssayRemainTime(cmd0503.UniqueNo, cmd0503.RepNo, sampleInfo, TimeSpan.FromSeconds(0));

                // 測定結果テーブル・分析ステータステーブルに測定データを反映する。
                switch (cmd0503.SampleKind)
                {
                    case SampleKind.Sample:
                    case SampleKind.Priority:

                        // 再検査対象確認
                        // 同一プロトコルの全多重が終了するときまで待機する。
                        Boolean isRemeased = this.selectReMeasureData(cmd0503, false, ref isAutoRemeasure, ref isAutoDilRemeasure, ref isOnlyRemeasureAdd, data);
                        if (!this.assayEndList.ContainsKey(dataKey))
                        {
                            this.assayEndList[dataKey] = new List<Tuple<SlaveCommCommand_0503, bool, bool, bool>>();
                        }
                        this.assayEndList[dataKey].Add(new Tuple<SlaveCommCommand_0503, bool, bool, bool>(cmd0503, isAutoRemeasure, isAutoDilRemeasure, isOnlyRemeasureAdd));

                        // ステータス設定
                        if (isRemeased)
                        {
                            sampleInfo.SetMeasureProtocolStatus(protocol.ProtocolIndex, cmd0503.RepNo, SampleInfo.SampleMeasureStatus.Error);
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, ""
                                , String.Format("isRemeased1】Individually={0} Rack-Pos={1}-{2} Uniq = {3} rep = {4}"
                                , cmd0503.IndividuallyNumber, cmd0503.RackID, cmd0503.SamplePos, cmd0503.UniqueNo, cmd0503.RepNo));
                        }

                        var SpecimenAssayData = Singleton<SpecimenAssayDB>.Instance.GetData(cmd0503.RackID).Find(
                            (assay) => assay.GetIndividuallyNo() == cmd0503.IndividuallyNumber && assay.GetUniqueNo() == cmd0503.UniqueNo);

                        if (protocol.IsIGRA)
                        {
                            List<SpecimenAssayData> listAssayData = Singleton<SpecimenAssayDB>.Instance.GetData(sampleInfo.ReceiptNumber, protocol.ProtocolIndex);
                            List<SpecimenAssayData> resultAssayData = listAssayData.OrderBy(((measitem) => (measitem.GetUniqueNo()))).ToList();

                            int nNotFinishedAssay = 0;//分析されていないデータ（还未分析完的数据。）
                            for (int i = 0; i < resultAssayData.Count; i++)
                            {
                                if (resultAssayData[i].GetStatus() == SampleInfo.SampleMeasureStatus.Wait
                                    || resultAssayData[i].GetStatus() == SampleInfo.SampleMeasureStatus.InProcess)
                                {
                                    nNotFinishedAssay++;
                                }
                            }
                            if (nNotFinishedAssay == 1)//1つの分析結果にのみ価値がありません。（只剩一个分析结果没有出值。）
                            {
                                List<int> UniqueNoList = new List<int>();
                                for (int i = 0; i < resultAssayData.Count; i++)
                                {
                                    if (!UniqueNoList.Contains(resultAssayData[i].GetUniqueNo()))
                                    {
                                        UniqueNoList.Add(resultAssayData[i].GetUniqueNo());
                                    }
                                }

                                {
                                    int nIndex = 2;//失敗の結果、最後の結果が最初に出る可能性があります。（有可能最后一个结果由于故障结果先出）
                                    for (int i = 0; i < UniqueNoList.Count; i++)
                                    {
                                        if (data.UniqueNo == UniqueNoList[i])
                                        {
                                            nIndex = i;
                                        }
                                    }

                                    if (repCount == 1 && resultAssayData.Count == 3)//重複なし（没有重复的情况）
                                    {
                                        IGRAMethod igraMethod = null;
                                        if (nIndex == 0)
                                        {
                                            igraMethod = new IGRAMethod(data.CalcInfoReplication.Concentration
                                                , resultAssayData[1].ConcentrationWithoutUnit, resultAssayData[2].ConcentrationWithoutUnit);
                                        }
                                        else if (nIndex == 1)
                                        {
                                            igraMethod = new IGRAMethod(resultAssayData[0].ConcentrationWithoutUnit
                                                , data.CalcInfoReplication.Concentration, resultAssayData[2].ConcentrationWithoutUnit);
                                        }
                                        else if (nIndex == 2)
                                        {
                                            igraMethod = new IGRAMethod(resultAssayData[0].ConcentrationWithoutUnit
                                                , resultAssayData[1].ConcentrationWithoutUnit, data.CalcInfoReplication.Concentration);
                                        }

                                        JudgementType judgeType = igraMethod.CaculateJudge();
                                        //Pチューブの破損による値がPチューブ（サンプルホルダーの4番目の位置）にまだ配置されている場合は、
                                        //正の陰が表示されます。（如果P管失败导致的值，还是放在P管（样本架第四个位置）显示阴阳性）
                                        if (nIndex != 2)
                                        {
                                            if (resultAssayData.Count == 3)
                                            {
                                                resultAssayData[2].SetJudgement(judgeType.ToTypeString());
                                                Singleton<SpecimenAssayDB>.Instance.SetData(new List<SpecimenAssayData>() { resultAssayData[2] });

                                                Singleton<SpecimenResultDB>.Instance.UpdateIGRAJudge(resultAssayData[2].GetUniqueNo(), judgeType);
                                            }

                                        }
                                        else
                                        {
                                            data.Judgement = judgeType.ToTypeString();
                                        }
                                    }
                                    else//複数の繰り返し（多个重复）
                                    {
                                        if (resultAssayData.Count == repCount * 3)
                                        {
                                            Boolean bCanCaculateAVE = true;
                                            for (int i = 0; i < repCount * 2; i++)
                                            {
                                                if (resultAssayData[i].ConcentrationWithoutUnit == null)
                                                {
                                                    bCanCaculateAVE = false;
                                                    break;
                                                }
                                            }
                                            if (data.CalcInfoAverage.Concentration == null)
                                            {
                                                bCanCaculateAVE = false;
                                            }
                                            if (bCanCaculateAVE)
                                            {
                                                //濃度値1計算（浓度值1计算）
                                                double sum1 = 0;
                                                int i = 0;
                                                for (i = 0; i < repCount; i++)
                                                {
                                                    sum1 += resultAssayData[i].ConcentrationWithoutUnit.Value;
                                                }
                                                double aveCon1 = sum1 / repCount;

                                                //濃度値2計算（浓度值2计算）
                                                double sum2 = 0;
                                                for (; i < repCount * 2; i++)
                                                {
                                                    sum2 += resultAssayData[i].ConcentrationWithoutUnit.Value;
                                                }
                                                double aveCon2 = sum2 / repCount;

                                                //濃度値3計算（浓度值3计算）
                                                double aveCon3 = data.CalcInfoAverage.Concentration.Value;

                                                IGRAMethod igraMethod = new IGRAMethod(aveCon1, aveCon2, aveCon3);
                                                JudgementType judgeType = igraMethod.CaculateJudge();
                                                data.Judgement = judgeType.ToTypeString();
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        int ca125ProtocolNumber = (int)BetweenItemsCalc.CA125;
                        int he4ProtocolNumber = (int)BetweenItemsCalc.HE4;

                        if (sampleInfo.Comment.Contains(Oelco.CarisX.Properties.Resources.STRING_ROMA))
                        {
                            if ((cmd0503.MeasProtocolNumber == ca125ProtocolNumber) || (cmd0503.MeasProtocolNumber == he4ProtocolNumber))
                            {
                                List<SpecimenAssayData> listAssayData = Singleton<SpecimenAssayDB>.Instance.GetData(cmd0503.RackID).FindAll((assay) =>
                                assay.GetIndividuallyNo() == cmd0503.IndividuallyNumber && (assay.GetMeasureProtocolIndex() == ca125ProtocolNumber || assay.GetMeasureProtocolIndex() == he4ProtocolNumber));

                                int nNotFinishedAssay = 0;//未解析データ（还未分析完的数据。）
                                for (int i = 0; i < listAssayData.Count; i++)
                                {
                                    SampleInfo.SampleMeasureStatus sampleMeasureStatus = listAssayData[i].GetStatus();
                                    if ((sampleMeasureStatus == SampleInfo.SampleMeasureStatus.Wait)
                                          || (sampleMeasureStatus == SampleInfo.SampleMeasureStatus.InProcess))
                                    {
                                        nNotFinishedAssay++;
                                    }
                                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("SpecimenAssayData  protocol={0}  count ={1} concent = {2} "
                                        , listAssayData[i].GetMeasureProtocolIndex().ToString(), listAssayData[i].Count, listAssayData[i].Concentration));
                                }

                                if (nNotFinishedAssay == 1) //最後の分析結果は残った（还剩最后一个分析结果 ）
                                {
                                    Double ca125Concent = 0; Double he4Concent = 0;
                                    if (cmd0503.MeasProtocolNumber == ca125ProtocolNumber) //最後の分析結果はCA125です（最后一个分析结果是CA125）
                                    {
                                        if (data.CalcInfoAverage != null)
                                        {
                                            if (data.CalcInfoAverage.Concentration.HasValue)
                                            {
                                                ca125Concent = data.CalcInfoAverage.Concentration.Value;
                                            }
                                        }
                                        if (data.CalcInfoReplication != null)
                                        {
                                            if (data.CalcInfoReplication.Concentration.HasValue)
                                            {
                                                ca125Concent = data.CalcInfoReplication.Concentration.Value;
                                            }
                                        }

                                        List<SpecimenAssayData> he4ListAssayData = listAssayData.FindAll((assay) => assay.GetMeasureProtocolIndex() == he4ProtocolNumber);
                                        if (he4ListAssayData.Count > 1)
                                        {
                                            Double sum = 0;
                                            for (int i = 0; i < he4ListAssayData.Count; i++)
                                            {
                                                sum += he4ListAssayData[i].ConcentrationWithoutUnit.Value;
                                            }
                                            he4Concent = sum / he4ListAssayData.Count;

                                        }
                                        else if (he4ListAssayData.Count > 0)
                                        {
                                            he4Concent = he4ListAssayData[0].ConcentrationWithoutUnit.Value;
                                        }

                                    }
                                    if (cmd0503.MeasProtocolNumber == he4ProtocolNumber)//最後の分析結果はHE4です（最后一个分析结果是HE4）
                                    {
                                        if (data.CalcInfoAverage != null)
                                        {
                                            if (data.CalcInfoAverage.Concentration.HasValue)
                                            {
                                                he4Concent = data.CalcInfoAverage.Concentration.Value;
                                            }
                                        }
                                        if (data.CalcInfoReplication != null)
                                        {
                                            if (data.CalcInfoReplication.Concentration.HasValue)
                                            {
                                                he4Concent = data.CalcInfoReplication.Concentration.Value;
                                            }
                                        }


                                        List<SpecimenAssayData> ca125ListAssayData = listAssayData.FindAll((assay) => assay.GetMeasureProtocolIndex() == ca125ProtocolNumber);
                                        if (ca125ListAssayData.Count > 1)
                                        {
                                            Double sum = 0;
                                            for (int i = 0; i < ca125ListAssayData.Count; i++)
                                            {
                                                sum += ca125ListAssayData[i].ConcentrationWithoutUnit.Value;
                                            }
                                            ca125Concent = sum / ca125ListAssayData.Count;

                                        }
                                        else if (ca125ListAssayData.Count > 0)
                                        {
                                            ca125Concent = ca125ListAssayData[0].ConcentrationWithoutUnit.Value;
                                        }

                                    }

                                    if ((ca125Concent > 0) && (he4Concent > 0))
                                    {
                                        RomaMethod romaMethod = new RomaMethod();

                                        double preRoma = romaMethod.preRoma(ca125Concent, he4Concent);
                                        double postRoma = romaMethod.postRoma(ca125Concent, he4Concent);

                                        SpecimenAssayData.SetComment(SpecimenAssayData.Comment + Oelco.CarisX.Properties.Resources.STRING_PREMENOPAUSEROMA + Math.Round(preRoma, 1).ToString()
                                            + Oelco.CarisX.Properties.Resources.STRING_POST_MENOPAUSEROMA + Math.Round(postRoma, 1).ToString());
                                    }
                                    else
                                    {
                                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, ""
                                            , String.Format("ROMA  ca125Concent={0}  he4Concent ={1} ", ca125Concent, he4Concent));
                                    }
                                }
                            }

                        }



                        // 検体情報のコメントデータが「PGI/PGII」がある場合
                        if (sampleInfo.Comment.Contains(Oelco.CarisX.Properties.Resources.STRING_PG1_DIVISION_PG2))
                        {
                            int pg1ProtocolNumber = (int)BetweenItemsCalc.PG1;
                            int pg2ProtocolNumber = (int)BetweenItemsCalc.PG2;

                            // 分析項目が「PG1」または「PG2」の場合
                            if ((cmd0503.MeasProtocolNumber == pg1ProtocolNumber) || (cmd0503.MeasProtocolNumber == pg2ProtocolNumber))
                            {
                                // アッセイデータからPG1またはPG2のデータを取得
                                List<SpecimenAssayData> listAssayData = Singleton<SpecimenAssayDB>.Instance.GetData(cmd0503.RackID).FindAll((assay) =>
                                    assay.GetIndividuallyNo() == cmd0503.IndividuallyNumber && (assay.GetMeasureProtocolIndex() == pg1ProtocolNumber || assay.GetMeasureProtocolIndex() == pg2ProtocolNumber));

                                // アッセイデータがnillではない場合
                                if (listAssayData != null)
                                {
                                    // 未解析データ
                                    int nNotFinishedAssay = 0;

                                    // 分析終了していないアッセイデータ件数を取得
                                    for (int i = 0; i < listAssayData.Count; i++)
                                    {
                                        SampleInfo.SampleMeasureStatus sampleMeasureStatus = listAssayData[i].GetStatus();
                                        if ((sampleMeasureStatus == SampleInfo.SampleMeasureStatus.Wait)
                                            || (sampleMeasureStatus == SampleInfo.SampleMeasureStatus.InProcess))
                                        {
                                            nNotFinishedAssay++;
                                        }
                                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("SpecimenAssayData  protocol={0}  count ={1} concent = {2} "
                                            , listAssayData[i].GetMeasureProtocolIndex().ToString(), listAssayData[i].Count, listAssayData[i].Concentration));
                                    }

                                    // 最後のアッセイデータである場合
                                    if (nNotFinishedAssay == 1)
                                    {
                                        Double pg1Concent = 0;
                                        Double pg2Concent = 0;

                                        // 最後のアッセイデータがPG1の場合 
                                        if (cmd0503.MeasProtocolNumber == pg1ProtocolNumber)
                                        {
                                            this.calcAssayDataBetweenItem(data, listAssayData, ref pg1Concent, ref pg2Concent, pg2ProtocolNumber);
                                        }
                                        // 最後のアッセイデータがPG2の場合
                                        if (cmd0503.MeasProtocolNumber == pg2ProtocolNumber)
                                        {
                                            this.calcAssayDataBetweenItem(data, listAssayData, ref pg2Concent, ref pg1Concent, pg1ProtocolNumber);
                                        }

                                        // pg1とpg2の濃度値が取得できている場合
                                        if ((pg1Concent > 0) && (pg2Concent > 0))
                                        {
                                            PgMethod pgMethod = new PgMethod();

                                            double pg = pgMethod.Pg(pg1Concent, pg2Concent);

                                            // アッセイデータデータのコメントに胃がんスクリーニングの計算結果を付与
                                            SpecimenAssayData.SetComment(SpecimenAssayData.Comment + ":" + pg.ToString("f1"));
                                        }
                                        else
                                        {
                                            // デバックログに出力
                                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, ""
                                                , String.Format("PG1/PG2  pg1Concent={0}  pg2Concent ={1} ", pg1Concent, pg2Concent));
                                        }
                                    }
                                }
                            }

                        }

                        // 検体情報のコメントデータが「f-PSA/T-PSA」がある場合
                        if (sampleInfo.Comment.Contains(Oelco.CarisX.Properties.Resources.STRING_FPSA_DIVISION_TPSA))
                        {
                            int f_psaProtocolNumber = (int)BetweenItemsCalc.f_PAS;
                            int t_psaProtocolNumber = (int)BetweenItemsCalc.T_PSA;

                            // 分析項目が「f-PSA」または「T-PSA」の場合
                            if ((cmd0503.MeasProtocolNumber == t_psaProtocolNumber) || (cmd0503.MeasProtocolNumber == f_psaProtocolNumber))
                            {
                                // アッセイデータからPG1またはPG2のデータを取得
                                List<SpecimenAssayData> listAssayData = Singleton<SpecimenAssayDB>.Instance.GetData(cmd0503.RackID).FindAll((assay) =>
                                    assay.GetIndividuallyNo() == cmd0503.IndividuallyNumber && (assay.GetMeasureProtocolIndex() == t_psaProtocolNumber || assay.GetMeasureProtocolIndex() == f_psaProtocolNumber));

                                // アッセイデータがnillではない場合
                                if (listAssayData != null)
                                {
                                    // 未解析データ
                                    int nNotFinishedAssay = 0;

                                    // 分析終了していないアッセイデータ件数を取得
                                    for (int i = 0; i < listAssayData.Count; i++)
                                    {
                                        SampleInfo.SampleMeasureStatus sampleMeasureStatus = listAssayData[i].GetStatus();
                                        if ((sampleMeasureStatus == SampleInfo.SampleMeasureStatus.Wait)
                                            || (sampleMeasureStatus == SampleInfo.SampleMeasureStatus.InProcess))
                                        {
                                            nNotFinishedAssay++;
                                        }
                                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("SpecimenAssayData  protocol={0}  count ={1} concent = {2} "
                                            , listAssayData[i].GetMeasureProtocolIndex().ToString(), listAssayData[i].Count, listAssayData[i].Concentration));
                                    }

                                    // 最後のアッセイデータである場合
                                    if (nNotFinishedAssay == 1)
                                    {
                                        Double f_psaConcent = 0;
                                        Double t_psaConcent = 0;

                                        // 最後のアッセイデータがT-PSAの場合
                                        if (cmd0503.MeasProtocolNumber == t_psaProtocolNumber)
                                        {
                                            this.calcAssayDataBetweenItem(data, listAssayData, ref t_psaConcent, ref f_psaConcent, f_psaProtocolNumber);
                                        }
                                        // 最後のアッセイデータがf-PSAの場合
                                        if (cmd0503.MeasProtocolNumber == f_psaProtocolNumber)
                                        {
                                            this.calcAssayDataBetweenItem(data, listAssayData, ref f_psaConcent, ref t_psaConcent, t_psaProtocolNumber);
                                        }

                                        // pg1とpg2の濃度値が取得できている場合
                                        if ((f_psaConcent) > 0 && (t_psaConcent > 0))
                                        {
                                            PsaMethod psaMethod = new PsaMethod();

                                            double psg = psaMethod.Psa(f_psaConcent, t_psaConcent);

                                            // アッセイデータデータのコメントに前立腺がんスクリーニングの計算結果を付与
                                            SpecimenAssayData.SetComment(SpecimenAssayData.Comment + ":" + psg.ToString("f1"));
                                        }
                                        else
                                        {
                                            // デバックログに出力
                                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, ""
                                                , String.Format("f-PSA/T-PSA  f_psaConcent={0}  t_psaConcent ={1} ", f_psaConcent, t_psaConcent));
                                        }
                                    }
                                }
                            }
                        }

                        Singleton<SpecimenResultDB>.Instance.AddResultData(data, cmd0503, SpecimenAssayData.GetReMeasure(), SpecimenAssayData.Comment);
                        
                        // ここでコミットしない場合直後のUpdateが追加扱いとなりエラーが出る。
                        Singleton<SpecimenResultDB>.Instance.CommitData();

                        Singleton<SpecimenResultDB>.Instance.UpdateAverageData(aveData);
                        Singleton<SpecimenResultDB>.Instance.CommitData();
                        Singleton<SpecimenAssayDB>.Instance.SetResultData(data, cmd0503);
                        Singleton<SpecimenAssayDB>.Instance.CommitData();

                        RealtimeDataAgent.LoadSpecimenResultData();
                        break;
                    case SampleKind.Calibrator:
                        Singleton<CalibratorResultDB>.Instance.AddResultData(data, cmd0503);

                        // ここでコミットしない場合直後のUpdateが追加扱いとなりエラーが出る。
                        Singleton<CalibratorResultDB>.Instance.CommitData();

                        Singleton<CalibratorResultDB>.Instance.UpdateAverageData(aveData);
                        Singleton<CalibratorResultDB>.Instance.CommitData();

                        var calibAssayData = Singleton<CalibratorAssayDB>.Instance.GetData(cmd0503.RackID).Find((assay) =>
                            assay.GetIndividuallyNo() == cmd0503.IndividuallyNumber && assay.GetUniqueNo() == cmd0503.UniqueNo);
                        CarisXCalculator.CreateCalibCurveData(data.ProtocolIndex, data.RackID, data.RackPosition.Value, calibAssayData.SequenceNo, out errorList);
                        errorList.ForEach((error) =>
                        {
                            if (error.ErrorCode != 0)
                            {
                                DPRErrorCode errCode = new DPRErrorCode(error.ErrorCode, error.ErrorArg, cmd0503.ModuleID);

                                // エラー履歴に登録（アラート発生無し）
                                CarisXSubFunction.WriteDPRErrorHist(errCode, 0, (error.ErrorDetailInfoParam.Count > 0 ? error.ErrorDetailInfoParam[0] : String.Empty), false);
                            }
                        });
                        Singleton<CalibratorResultDB>.Instance.UpdateRemark(data);
                        Singleton<CalibratorResultDB>.Instance.CommitData();
                        Singleton<CalibratorAssayDB>.Instance.SetResultData(data, cmd0503);
                        Singleton<CalibratorAssayDB>.Instance.CommitData();
                        RealtimeDataAgent.LoadCalibratoResultData();
                        // 試薬テーブルの更新
                        // ※検量線データが追加された場合に試薬テーブル上のロット番号の色を変えるため
                        RealtimeDataAgent.LoadReagentRemainData();
                        break;
                    case SampleKind.Control:
                        // QCデータを追加する最初の時間を設定
                        if (DateTime.Compare(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Delivery_date, DateTime.MaxValue) == 0)
                        {
                            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Delivery_date = DateTime.Now;
                            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Save();
                            CarisXSubFunction.SendIoTDueDate();
                        }
                        var controlAssayData = Singleton<ControlAssayDB>.Instance.GetData(cmd0503.RackID).Find((assay) =>
                            assay.GetIndividuallyNo() == cmd0503.IndividuallyNumber && assay.GetUniqueNo() == cmd0503.UniqueNo);
                        Singleton<ControlResultDB>.Instance.AddResultData(data, cmd0503, controlAssayData.ControlName, controlAssayData.Comment);

                        // ここでコミットしない場合直後のUpdateが追加扱いとなりエラーが出る。
                        Singleton<ControlResultDB>.Instance.CommitData();

                        Singleton<ControlResultDB>.Instance.UpdateAverageData(aveData);
                        Singleton<ControlResultDB>.Instance.CommitData();
                        Singleton<ControlAssayDB>.Instance.SetResultData(data, cmd0503);
                        Singleton<ControlAssayDB>.Instance.CommitData();
                        RealtimeDataAgent.LoadControlResultData();
                        break;
                    default:
                        break;
                }

                // ステータス画面 分析情報更新
                RealtimeDataAgent.LoadAssayData();

                // ホストへ送信
                this.sendHostResult(cmd0503, data);


                // 測定データをクラウドIoTへ送信
                CarisXSubFunction.SendIoTMeasureResult(cmd0503, data);

                if (sampleInfo.ProtocolStatusDictionary[protocol.ProtocolIndex].All((v) => v.Value == SampleInfo.SampleMeasureStatus.End || v.Value == SampleInfo.SampleMeasureStatus.Error))
                {
                    // 測定結果ファイルへ保存
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.RealtimeOutputFileData, data);
                }

                // リアルタイム印刷(ページ単位)
                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.RealtimePrintData, false);

                foreach (var item in sampleInfo.ProtocolStatusDictionary)
                {
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("sampleInfo.ProtocolStatusDictionary ProtoclIndex ={0}  Uniq = {1} ", item.Key, cmd0503.UniqueNo));
                    foreach (var item1 in item.Value)
                    {
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("sampleInfo.ProtocolStatusDictionary repet ={0}  SampleMeasureStatus ={1}Uniq = {2} ", item1.Key, item1.Value.ToString(), cmd0503.UniqueNo));
                    }
                }

                //ユニークID（同一ラックID＋ポジション）内の全プロトコル番号のAssayが終了またはエラーになっているか。
                if (sampleInfo.ProtocolStatusDictionary.All((v) => v.Value.All((vv) => vv.Value == SampleInfo.SampleMeasureStatus.End || vv.Value == SampleInfo.SampleMeasureStatus.Error)))
                {

                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("sampleInfo.ProtocolStatusDictionary endList.count() ={0}  Uniq = {1} ", this.assayEndList.Count, cmd0503.UniqueNo));

                    // 再検査登録
                    foreach (var endData in this.assayEndList)
                    {
                        // 再検査登録の優先順序による登録を行う
                        var endInfo = endData.Value;

                        // 自動希釈再検→自動再検→再検リスト記載の優先順位でどれか一つを実施する。
                        var autoDilRemeasure = endInfo.Find((v) => v.Item3 == true);
                        var autoRemeasure = endInfo.Find((v) => v.Item2 == true);
                        var onlyRemeasureAdd = endInfo.FindAll((v) => v.Item4 == true);

                        // 自動希釈再検・自動再検はGUIに表示せず、そのまま再検されるので見つかったら一つだけをDBに登録しておく。
                        // 再検リスト追加の場合、再検画面に表示される為、各レプリ分全てをDBに登録する。
                        if (!protocol.IsIGRA && autoDilRemeasure != null)//IGRA项目不允许自动重测
                        {
                            isAutoRemeasure = false;
                            isAutoDilRemeasure = true;
                            isOnlyRemeasureAdd = false;
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("autoDilRemeasure != null: Individually={0} ", cmd0503.IndividuallyNumber));
                            this.selectReMeasureData(autoDilRemeasure.Item1, true, ref isAutoRemeasure, ref isAutoDilRemeasure, ref isOnlyRemeasureAdd, data);
                        }
                        else if (!protocol.IsIGRA && autoRemeasure != null)//IGRA项目不允许自动重测
                        {
                            isAutoRemeasure = true;
                            isAutoDilRemeasure = false;
                            isOnlyRemeasureAdd = false;
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("autoRemeasure != null: Individually={0} ", cmd0503.IndividuallyNumber));
                            this.selectReMeasureData(autoRemeasure.Item1, true, ref isAutoRemeasure, ref isAutoDilRemeasure, ref isOnlyRemeasureAdd, data);
                        }
                        else if (!protocol.IsIGRA && onlyRemeasureAdd != null && onlyRemeasureAdd.Count != 0)//IGRA项目不允许自动重测
                        {
                            isAutoRemeasure = false;
                            isAutoDilRemeasure = false;
                            isOnlyRemeasureAdd = true;
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("onlyRemeasureAdd != null && onlyRemeasureAdd.Count != 0 Individually={0} Rack-Pos={1}-{2} Uniq = {3} rep = {4}"
                                , cmd0503.IndividuallyNumber, cmd0503.RackID, cmd0503.SamplePos, cmd0503.UniqueNo, cmd0503.RepNo));
                            onlyRemeasureAdd.ForEach((v) =>
                                this.selectReMeasureData(v.Item1, true, ref isAutoRemeasure, ref isAutoDilRemeasure, ref isOnlyRemeasureAdd, data));

                        }
                        //just for trace
                        else if (onlyRemeasureAdd != null && onlyRemeasureAdd.Count == 0)
                        {
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("onlyRemeasureAdd != null && onlyRemeasureAdd.Count == 0 Individually={0} Rack-Pos={1}-{2} Uniq = {3} rep = {4}"
                                , cmd0503.IndividuallyNumber, cmd0503.RackID, cmd0503.SamplePos, cmd0503.UniqueNo, cmd0503.RepNo));
                        }
                        else if (onlyRemeasureAdd == null)
                        {
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("onlyRemeasureAdd == null Individually={0} Rack-Pos={1}-{2} Uniq = {3} rep = {4}"
                                , cmd0503.IndividuallyNumber, cmd0503.RackID, cmd0503.SamplePos, cmd0503.UniqueNo, cmd0503.RepNo));
                        }
                        //trace end

                        //                    this.endList.Remove( dataKey );
                    }
                    this.assayEndList.Clear();
                    this.sendMeasureCompleteAfterMeasure(cmd0503.RackID);
                    this.sendAutoRetestData(cmd0503);

                    // 調査用ログ
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("【Measurement data command】SampleInfo deleted: Individually={0} Rack-Pos={1}-{2} Uniq = {3} rep = {4}"
                        , cmd0503.IndividuallyNumber, cmd0503.RackID, cmd0503.SamplePos, cmd0503.UniqueNo, cmd0503.RepNo));

                    // 分析中情報削除
                    Singleton<InProcessSampleInfoManager>.Instance.RemoveSampleInfo(sampleInfo);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));

                //如果此项目出现计算异常，删除相关的进行信息。
                SampleInfo sampleInfo = Singleton<InProcessSampleInfoManager>.Instance.SearchInProcessSampleFromUniqueNo(cmd0503.UniqueNo);
                Singleton<InProcessSampleInfoManager>.Instance.RemoveSampleInfo(sampleInfo);
            }

            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((MachineCode)cmd0503.CommNo, new SlaveCommCommand_1503());
        }

        /// <summary>
        /// 分析残り時間設定
        /// </summary>
        /// <remarks>
        /// 分析残り時間を設定します
        /// </remarks>
        /// <param name="uniqNo"></param>
        /// <param name="repNo"></param>
        /// <param name="sampleInfo"></param>
        /// <param name="span"></param>
        private void setAssayRemainTime(Int32 uniqNo, Int32 repNo, SampleInfo sampleInfo, TimeSpan span)
        {
            switch (sampleInfo.SampleKind)
            {
                case SampleKind.Sample:
                case SampleKind.Priority:
                    Singleton<SpecimenAssayDB>.Instance.AssayStatusUpdate(uniqNo, repNo, span);
                    Singleton<SpecimenAssayDB>.Instance.CommitData();
                    break;
                case SampleKind.Control:
                    Singleton<ControlAssayDB>.Instance.AssayStatusUpdate(uniqNo, repNo, span);
                    Singleton<ControlAssayDB>.Instance.CommitData();
                    break;
                case SampleKind.Calibrator:
                    Singleton<CalibratorAssayDB>.Instance.AssayStatusUpdate(uniqNo, repNo, span);
                    Singleton<CalibratorAssayDB>.Instance.CommitData();
                    break;
            }
        }

        /// <summary>
        /// 再検査データ確認
        /// </summary>
        /// <remarks>
        /// 測定結果データから、再検査リスト記載・自動再検・自動希釈再検の3パターンの登録を行います。
        /// </remarks>
        /// <param name="measureResult">測定結果データ</param>
        /// <returns>True:再検査DB追加 False:再検査非対象</returns>
        protected Boolean selectReMeasureData(SlaveCommCommand_0503 measureResult, Boolean allowRegist, ref Boolean isAutoReMeasure, ref Boolean isAutoDilRemeasure, ref Boolean isOnlyReMeasureAdd, CalcData calcData = null)
        {
            Boolean result = true;

            // 使用分析項目
            MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(measureResult.MeasProtocolNumber);

            if (allowRegist == false)
            {
                isAutoReMeasure = false;
                isAutoDilRemeasure = false;

                if (calcData.CalcInfoReplication.Remark.CanCalcConcentration == true && calcData.CalcInfoReplication.Concentration.HasValue)
                {
                    // 自動再検フラグ
                    // 自動再検使用あり設定＆測定カウントが指定範囲内
                    if (protocol.UseAutoReTest)
                    {
                        if (protocol.RetestRange.UseLow && calcData.CalcInfoReplication.Concentration <= protocol.AutoReTest.Min)
                        {
                            isAutoReMeasure = true;

                        }
                        else if (protocol.RetestRange.UseMiddle && (calcData.CalcInfoReplication.Concentration >= protocol.AutoReTest.Min && calcData.CalcInfoReplication.Concentration <= protocol.AutoReTest.Max))
                        {
                            isAutoReMeasure = true;
                        }
                        else if (protocol.RetestRange.UseHigh && calcData.CalcInfoReplication.Concentration >= protocol.AutoReTest.Max)
                        {
                            isAutoReMeasure = true;
                        }
                    }

                    // 自動希釈再検フラグ
                    // 自動再検使用あり設定＆測定カウントが指定範囲内
                    if (protocol.UseAfterDil)
                    {
                        // 急診使用有の場合
                        if (protocol.UseEmergencyMode == true)
                        {
                            isAutoDilRemeasure = false;
                        }
                        // 急診使用無の場合
                        else
                        {
                            if (protocol.RetestRange.UseLow && calcData.CalcInfoReplication.Concentration <= protocol.AutoDilutionReTest.Min)
                            {
                                isAutoDilRemeasure = true;

                            }
                            else if (protocol.RetestRange.UseMiddle && ( calcData.CalcInfoReplication.Concentration >= protocol.AutoDilutionReTest.Min && calcData.CalcInfoReplication.Concentration <= protocol.AutoDilutionReTest.Max ))
                            {
                                isAutoDilRemeasure = true;
                            }
                            else if (protocol.RetestRange.UseHigh && calcData.CalcInfoReplication.Concentration >= protocol.AutoDilutionReTest.Max)
                            {
                                isAutoDilRemeasure = true;
                            }
                        }
                    }
                }

                // 再検査リスト記載のみフラグ (再検査リスト要記載のリマークがついており、いずれの自動再検条件にも合致しない)
                isOnlyReMeasureAdd = (measureResult.Remark.IsNeedReMeasure && !isAutoReMeasure && !isAutoDilRemeasure);

                //STATで自動再検又は希釈再検の場合は手動再検扱いにする
                if (((CarisXIDString)measureResult.RackID).GetSampleKind() == SampleKind.Priority && (isAutoReMeasure || isAutoDilRemeasure))
                {
                    isOnlyReMeasureAdd = true;
                    isAutoReMeasure = false;
                    isAutoDilRemeasure = false;
                }

                if (isOnlyReMeasureAdd)
                {
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("ReMarsure0-isOnlyReMeasureAdd Individually={0} Rack-Pos={1}-{2} Uniq = {3} rep = {4},measureResult.Remark.IsNeedReMeasure{5}", measureResult.IndividuallyNumber, measureResult.RackID, measureResult.SamplePos, measureResult.UniqueNo, measureResult.RepNo, measureResult.Remark.IsNeedReMeasure));
                }
                if (isAutoReMeasure)
                {
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("ReMarsure0-isAutoReMeasure Individually={0} Rack-Pos={1}-{2} Uniq = {3} rep = {4},measureResult.Remark.IsNeedReMeasure{5}", measureResult.IndividuallyNumber, measureResult.RackID, measureResult.SamplePos, measureResult.UniqueNo, measureResult.RepNo, measureResult.Remark.IsNeedReMeasure));
                }
                if (isAutoDilRemeasure)
                {
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("ReMarsure0-isAutoDilRemeasure Individually={0} Rack-Pos={1}-{2} Uniq = {3} rep = {4},measureResult.Remark.IsNeedReMeasure{5}", measureResult.IndividuallyNumber, measureResult.RackID, measureResult.SamplePos, measureResult.UniqueNo, measureResult.RepNo, measureResult.Remark.IsNeedReMeasure));
                }

            }
            else
            {
                //  Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("ReMarsure1--start test! Individually={0} Rack-Pos={1}-{2} Uniq = {3} rep = {4},measureResult.Remark.IsNeedReMeasure{5}", measureResult.IndividuallyNumber, measureResult.RackID, measureResult.RackPos, measureResult.UniqueNo, measureResult.RepNo, measureResult.Remark.IsNeedReMeasure));
            }


            // isOnlyReMeasureAddのみの場合はそのまま再検査DBに追加するのみ、
            // isAutoReMeasureとisAutoDilRemeasureが競合した場合、isAutoDilReMeasureを先に判定している。

            // 再検査非対称確認
            result = isOnlyReMeasureAdd || isAutoDilRemeasure || isAutoReMeasure;

            if (allowRegist)
            {
                if (isOnlyReMeasureAdd)
                {
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("isOnlyReMeasure5  Individually={0} Rack-Pos={1}-{2} Uniq = {3} rep = {4},measureResult.Remark.IsNeedReMeasure{5}", measureResult.IndividuallyNumber, measureResult.RackID, measureResult.SamplePos, measureResult.UniqueNo, measureResult.RepNo, measureResult.Remark.IsNeedReMeasure));

                    // 後希釈倍率 * プロトコル倍率
                    int afterDilution = (measureResult.AfterDilution * (Int32)protocol.ProtocolDilutionRatio);

                    // 再検査リスト記載のみ
                    this.setRemeasureDB(measureResult, false, afterDilution);
                }
                else if (isAutoDilRemeasure)
                {
                    // Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("isAutoDilRemeasure5  Individually={0} Rack-Pos={1}-{2} Uniq = {3} rep = {4},measureResult.Remark.IsNeedReMeasure{5}", measureResult.IndividuallyNumber, measureResult.RackID, measureResult.RackPos, measureResult.UniqueNo, measureResult.RepNo, measureResult.Remark.IsNeedReMeasure));
                    // 自動再検は1度限り

                    /* 中国対応にて2000倍を除外する理由が不明なことや自動希釈倍率が前回と同じ倍率で希釈するケースがあるため、削除
                     * 参考として一旦コメントアウトで残す
                    // 自動希釈再検
                    int nProtocolReTestRatio = (Int32)protocol.AutoDilutionReTestRatio;
                    int afterDilution = measureResult.AfterDilution;

                    if (afterDilution <= CarisXConst.MaxDILUTION && afterDilution != 2000)//如果设置的倍数超过了仪器能够稀释的倍数且稀释倍数不为2000
                    {
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("isAutoDilRemeasure5  Individually={0} Rack-Pos={1}-{2} Uniq = {3} rep = {4},measureResult.Remark.IsNeedReMeasure{5}", measureResult.IndividuallyNumber, measureResult.RackID, measureResult.SamplePos, measureResult.UniqueNo, measureResult.RepNo, measureResult.Remark.IsNeedReMeasure));
                        this.setRemeasureDB(measureResult, true, afterDilution);
                    }
                    else
                    {
                        //如果两次的稀释倍数超过8000倍且第一次稀释倍数小于项目重测稀释倍数，则稀释倍数设置为项目重测稀释倍数。
                        if (measureResult.AfterDilution < nProtocolReTestRatio)
                        {
                            afterDilution = nProtocolReTestRatio;
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("isAutoDilRemeasure6  Individually={0} Rack-Pos={1}-{2} Uniq = {3} rep = {4},measureResult.Remark.IsNeedReMeasure{5}", measureResult.IndividuallyNumber, measureResult.RackID, measureResult.SamplePos, measureResult.UniqueNo, measureResult.RepNo, measureResult.Remark.IsNeedReMeasure));
                            this.setRemeasureDB(measureResult, true, afterDilution);
                        }
                    }
                    */

                    // 自動希釈倍率
                    int afterDilution = measureResult.AfterDilution;

                    // 自動希釈再検倍率
                    int nProtocolReTestRatio = (Int32)protocol.AutoDilutionReTestRatio;

                    // プロトコル希釈倍率
                    int protocolDilutionRatio = (Int32)protocol.ProtocolDilutionRatio;

                    // 8000倍を超える希釈倍率設定になっていないかチェック
                    if ((afterDilution * nProtocolReTestRatio * protocolDilutionRatio) <= CarisXConst.MaxDILUTION)
                    {
                        // 「自動希釈倍率×自動希釈再検倍率」を希釈倍率として採用

                        // 前回の倍率を超える希釈倍率設定となっているか確認
                        if (afterDilution < (afterDilution * nProtocolReTestRatio))
                        {
                            // プロトコル倍率を乗算して設定（再検データ登録にて除算するため）
                            afterDilution = afterDilution * nProtocolReTestRatio * protocolDilutionRatio;

                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("isAutoDilRemeasure5  Individually={0} Rack-Pos={1}-{2} Uniq = {3} rep = {4},measureResult.Remark.IsNeedReMeasure{5}", measureResult.IndividuallyNumber, measureResult.RackID, measureResult.SamplePos, measureResult.UniqueNo, measureResult.RepNo, measureResult.Remark.IsNeedReMeasure));
                            this.setRemeasureDB(measureResult, true, afterDilution);
                        }
                        else
                        {
                            // 前回の倍率以下のため、自動希釈再検不要
                        }
                    }
                    // 自動希釈再検倍率が自動希釈倍率を超えているかチェック
                    else if (afterDilution < nProtocolReTestRatio)
                    {
                        // 「自動希釈再検倍率」を希釈倍率として採用

                        // 「自動希釈再検倍率×プロトコル倍率」が8000倍以下であれば、再検可能
                        if ((nProtocolReTestRatio * protocolDilutionRatio) <= CarisXConst.MaxDILUTION)
                        {
                            // プロトコル倍率を乗算して設定（再検データ登録にて除算するため）
                            afterDilution = nProtocolReTestRatio * protocolDilutionRatio;

                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("isAutoDilRemeasure6  Individually={0} Rack-Pos={1}-{2} Uniq = {3} rep = {4},measureResult.Remark.IsNeedReMeasure{5}", measureResult.IndividuallyNumber, measureResult.RackID, measureResult.SamplePos, measureResult.UniqueNo, measureResult.RepNo, measureResult.Remark.IsNeedReMeasure));
                            this.setRemeasureDB(measureResult, true, afterDilution);
                        }
                    }
                    else
                    {
                        // 前回の倍率以下のため、自動希釈再検不要
                    }

                }
                else if (isAutoReMeasure)
                {
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("iisAutoReMeasure5  Individually={0} Rack-Pos={1}-{2} Uniq = {3} rep = {4},measureResult.Remark.IsNeedReMeasure{5}", measureResult.IndividuallyNumber, measureResult.RackID, measureResult.SamplePos, measureResult.UniqueNo, measureResult.RepNo, measureResult.Remark.IsNeedReMeasure));
                    // 自動再検は1度限り

                    // 後希釈倍率 * プロトコル倍率
                    int afterDilution = (measureResult.AfterDilution * (Int32)protocol.ProtocolDilutionRatio);

                    // 自動再検
                    this.setRemeasureDB(measureResult, true, afterDilution);
                }
            }

            return result;
        }


        /// <summary>
        /// 再検査DB設定
        /// </summary>
        /// <remarks>
        /// 測定結果から再検査リストへの設定を行います。
        /// </remarks>
        /// <param name="measureResult">測定結果</param>
        /// <param name="isAutoRemeasure">再検査フラグRe-examination flag</param>
        /// <param name="autoDilRatio">自動希釈倍率</param>
        protected void setRemeasureDB(IMeasureResultData measureResult, Boolean isAutoRemeasure, Int32 autoDilRatio)
        {
            List<SpecimenReMeasureGridViewDataSet> setList = new List<SpecimenReMeasureGridViewDataSet>();
            SpecimenReMeasureGridViewDataSet newReMeasureData = new SpecimenReMeasureGridViewDataSet();
            var inProcessData = Singleton<InProcessSampleInfoManager>.Instance.SearchInProcessSampleFromUniqueNo(measureResult.UniqueNo); // Singleton<InProcessSampleInfoManager>.Instance.SearchInProcessSampleFromIndividuallyNumber( measureResult.IndividuallyNumber );
            if (inProcessData == null)
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("isOnlyReMeasure6 Individually={0} Rack-Pos={1}-{2} Uniq = {3} rep = {4},measureResult.Remark.IsNeedReMeasure{5}", measureResult.IndividuallyNumber, measureResult.RackID, measureResult.SamplePos, measureResult.UniqueNo, measureResult.RepNo, measureResult.Remark.IsNeedReMeasure));
                return;
            }

            // 自動再検履歴を設定
            MeasDetailKey measKey = new MeasDetailKey(measureResult.IndividuallyNumber, Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(measureResult.MeasProtocolNumber).ProtocolIndex);
            Boolean isFirstReMeasure = Singleton<InProcessSampleInfoManager>.Instance.SetAddedReMeasList(measKey);
            // 自動再検に登録されるのは初回のみ（Statの場合は自動再検を手動再検扱いにするのでここでは除外しない）
            if (!isFirstReMeasure && isAutoRemeasure && measureResult.SampleKind != SampleKind.Priority)
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("isOnlyReMeasure7 Individually={0} Rack-Pos={1}-{2} Uniq = {3} rep = {4},measureResult.Remark.IsNeedReMeasure{5}", measureResult.IndividuallyNumber, measureResult.RackID, measureResult.SamplePos, measureResult.UniqueNo, measureResult.RepNo, measureResult.Remark.IsNeedReMeasure));
                return;
            }

            var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(measureResult.MeasProtocolNumber);

            autoDilRatio = autoDilRatio / (int)protocol.ProtocolDilutionRatio;

            newReMeasureData.AutoDilution = autoDilRatio;
            newReMeasureData.Comment = inProcessData.Comment;
            newReMeasureData.Concentration = null;
            newReMeasureData.Count = measureResult.ResultCount;
            newReMeasureData.IndividuallyNo = measureResult.IndividuallyNumber;
            if (measureResult.SampleKind == SampleKind.Priority)
                newReMeasureData.IsAutoReMeasure = false;               //STATの場合は自動再検が出来ない
            else
                newReMeasureData.IsAutoReMeasure = isAutoRemeasure;
            newReMeasureData.ManualDilution = measureResult.PreDilution;
            newReMeasureData.MeasprotocolNo = measureResult.MeasProtocolNumber;


            // 測定時刻取得を変更
            newReMeasureData.MeasureDateTime = inProcessData.GetMeasureEndTime(protocol.ProtocolIndex, measureResult.RepNo);

            newReMeasureData.ModuleNo = measureResult.ModuleID;
            newReMeasureData.SampleID = measureResult.SampleId;
            newReMeasureData.RackID = measureResult.RackID;
            newReMeasureData.RackPosition = measureResult.SamplePos;
            newReMeasureData.ReceiptNumber = inProcessData.ReceiptNumber;
            newReMeasureData.Remark = measureResult.Remark;
            newReMeasureData.SequenceNumber = inProcessData.SequenceNumber;
            newReMeasureData.SpecimenMaterialType = measureResult.SpecimenMaterialType;
            newReMeasureData.WaitMeasureIndicate = false; // 再検査コマンドを送信した際trueになる
            newReMeasureData.ReplicationNo = measureResult.RepNo;
            setList.Add(newReMeasureData);

            if (measureResult.SampleKind == SampleKind.Priority)
            {
                //STATの場合
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("isOnlyReMeasure8 IsAutoReMeasure={0}", newReMeasureData.IsAutoReMeasure));
                Boolean bAddRemesureData = Singleton<SpecimenStatReMeasureDB>.Instance.AddRemesureData(setList, measureResult.CupKind);
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("isOnlyReMeasure8 bAddRemesureData={0}", bAddRemesureData));
                Singleton<SpecimenStatReMeasureDB>.Instance.CommitSampleReMeasureInfo();
            }
            else
            {
                //一般検体の場合
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("isOnlyReMeasure8 IsAutoReMeasure={0}", newReMeasureData.IsAutoReMeasure));
                Boolean bAddRemesureData = Singleton<SpecimenReMeasureDB>.Instance.AddRemesureData(setList);
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("isOnlyReMeasure8 bAddRemesureData={0}", bAddRemesureData));
                Singleton<SpecimenReMeasureDB>.Instance.CommitSampleReMeasureInfo();
            }
            RealtimeDataAgent.LoadReMeasureSampleData();
        }

        /// <summary>
        /// ホストへ検体の測定結果送信
        /// </summary>
        /// <remarks>
        /// ホストへ検体の測定結果送信します
        /// </remarks>
        /// <param name="command"></param>
        /// <param name="calcData"></param>
        private void sendHostResult(SlaveCommCommand_0503 command, CalcData calcData)
        {
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Enable)
            {
                HostCommCommand_0003 sendData = new HostCommCommand_0003();
                var dat = Singleton<InProcessSampleInfoManager>.Instance.SearchInProcessSampleFromIndividuallyNumber(calcData.IndividuallyNo).First();


                // Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("*{0}:UsableRealtimeOutputSamp = {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UsableRealtimeOutputSamp.ToString()));
                // 検体の測定結果を送信
                if ((command.SampleKind == SampleKind.Sample || command.SampleKind == SampleKind.Priority) &&
                    Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UsableRealtimeOutputSamp)
                {
                    sendData.AutoDilution = CarisXSubFunction.GetHostAutoDil(command.AfterDilution);
                    // カウント・濃度は計算済のものを取ってくる
                    //command03.Conc = calcData.CalcInfoReplication.
                    //command03.DispCount = 
                    sendData.DispCount = calcData.CalcInfoReplication.CountValue.ToString();
                    sendData.Conc = String.IsNullOrEmpty(calcData.CalcInfoReplication.Concentration.ToString()) ? CarisXConst.COUNT_CONCENTRATION_NOTHING : calcData.CalcInfoReplication.Concentration.ToString();
                    sendData.Judge = calcData.Judgement;
                    sendData.ManualDil = calcData.ManualDilution;
                    sendData.MeasDateTime = calcData.MeasureDateTime;
                    MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(calcData.ProtocolIndex);
                    sendData.ProtocolNumber = Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.GetHostProtocolNumber(protocol.ProtocolName);
                    if (calcData.RackID.GetSampleKind() == SampleKind.Priority)
                    {
                        sendData.RackID = String.Empty;
                        sendData.RackPos = 0;
                    }
                    else
                    {
                        sendData.RackID = calcData.RackID.DispPreCharString;
                        sendData.RackPos = calcData.RackPosition.HasValue ? calcData.RackPosition.Value : 0;
                    }
                    sendData.ReagLotNo = calcData.ReagentLotNo;
                    sendData.ReceiptNumber = dat.ReceiptNumber;
                    sendData.Remark = calcData.CalcInfoReplication.Remark.Value;
                    sendData.SampleId = calcData.SampleID;
                    sendData.SampleType = HostSampleType.N;
                    sendData.SeqNo = dat.SequenceNumber;
                    sendData.HostSampleKind = command.SpecimenMaterialType.GetHostSampleKind();
                    sendData.SampleLot = ""; // サンプルロットは精度管理以外で空白

                    Singleton<CarisXSequenceHelperManager>.Instance.Host.HostCommunicationSequence
                        (HostCommunicationSequencePattern.SendResultToHost | HostCommunicationSequencePattern.Specimen, sendData);
                }
                else if (command.SampleKind == SampleKind.Control &&
                    Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UsableRealtimeOutputCtrl)
                {

                    sendData.AutoDilution = CarisXSubFunction.GetHostAutoDil(command.AfterDilution);
                    // カウント・濃度は計算済のものを取ってくる
                    //command03.Conc = calcData.CalcInfoReplication.
                    //command03.DispCount = 
                    sendData.DispCount = calcData.CalcInfoReplication.CountValue.ToString();
                    sendData.Conc = String.IsNullOrEmpty(calcData.CalcInfoReplication.Concentration.ToString()) ? CarisXConst.COUNT_CONCENTRATION_NOTHING : calcData.CalcInfoReplication.Concentration.ToString();
                    sendData.Judge = calcData.Judgement;
                    sendData.ManualDil = calcData.ManualDilution;
                    sendData.MeasDateTime = calcData.MeasureDateTime;
                    MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(calcData.ProtocolIndex);
                    sendData.ProtocolNumber = Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.GetHostProtocolNumber(protocol.ProtocolName);
                    sendData.RackID = calcData.RackID.DispPreCharString;
                    sendData.RackPos = calcData.RackPosition.HasValue ? calcData.RackPosition.Value : 0;
                    sendData.ReagLotNo = calcData.ReagentLotNo;
                    sendData.ReceiptNumber = dat.ReceiptNumber;
                    sendData.Remark = calcData.CalcInfoReplication.Remark.Value;
                    sendData.SampleId = calcData.SampleID;
                    sendData.SampleType = HostSampleType.C;
                    sendData.SeqNo = dat.SequenceNumber;
                    sendData.HostSampleKind = command.SpecimenMaterialType.GetHostSampleKind();

                    var record = from v in Singleton<ControlResultDB>.Instance.GetData()
                                 where v.GetIndividuallyNo() == calcData.IndividuallyNo
                                 select v;
                    sendData.SampleLot = record.First().ControlLotNo;

                    Singleton<CarisXSequenceHelperManager>.Instance.Host.HostCommunicationSequence
                        (HostCommunicationSequencePattern.SendResultToHost | HostCommunicationSequencePattern.Control, sendData);
                }

            }
        }


        /// <summary>
        /// リアルタイム印刷処理
        /// </summary>
        /// <param name="immediatelyExecute">即実行フラグ</param>
        /// <remarks>
        /// リアルタイム印刷を実施します。
        /// 即実行フラグがFalseの場合、1ページ分のデータが溜まってから印刷されます。
        /// Falseであれば、現在未印刷のデータ全てを印刷します。
        /// </remarks>
        private void realtimePrint(Boolean immediatelyExecute)
        {
            // リアルタイム印刷が設定されていれば実行する。
            if ((Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable) &&
                (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.UsableRealtimePrint))
            {

                // A.1ページ分測定データが溜まったら印刷実施
                // B.分析終了タイミングで印刷実施

                try
                {
                    // 検体・優先検体印刷
                    Singleton<FormSpecimenResult>.Instance.RealtimePrint(immediatelyExecute);

                    // キャリブレータ印刷
                    Singleton<FormCalibResult>.Instance.RealtimePrint(immediatelyExecute);

                    // 精度管理検体印刷
                    Singleton<FormControlResult>.Instance.RealtimePrint(immediatelyExecute);
                }
                catch (Exception ex)
                {
                    Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("An exception occurred in real-time printing : {0}", ex.StackTrace));
                }
            }
        }

        /// <summary>
        /// 自動再検査データ送信
        /// </summary>
        /// <remarks>
        /// 再検査コマンドを送信可能な自動再検査データがあれば送信を行います。
        /// 送信されたデータは検体測定指示問合せの際、DBから削除されます。
        /// </remarks>
        private void sendAutoRetestData(SlaveCommCommand_0503 command)
        {

            // 再検データ送信可否確認
            // 自動再検対象データがあるか
            // v.IsAutoReMeasure…自動再検または自動希釈再検
            // !v.WaitMeasureIndicate…再検査コマンドを送信されていない
            Boolean flgExistsRetestData = false;
            IEnumerable<SpecimenReMeasureGridViewDataSet> autoReMeasure = from v in Singleton<SpecimenReMeasureDB>.Instance.GetDispData(GetAutoRemeasKind.All)
                                                                          where v.IsAutoReMeasure && !v.WaitMeasureIndicate
                                                                          select v;
            if (autoReMeasure.Count() != 0)
            {
                var autoRemeasureList = autoReMeasure.ToList(); // リストを保持しておく（Enumerableだと中身が変更されたときにリスト化結果が変わる）
                // 自動再検対象データのラックが待機レーンにあるか
                foreach (var dat in autoRemeasureList)
                {
                    RackFindResult searchResult = CarisXSubFunction.SearchRack(dat.RackID);
                    if (searchResult == RackFindResult.FindOnWaitingRack)
                    {
                        // 自動再検対象データのラックにある検体は全て分析終了ないしエラーとなっているか
                        var inprocessInfo = Singleton<InProcessSampleInfoManager>.Instance.SearchInProcessSampleFromRackId(dat.RackID);
                        if (inprocessInfo.Count != 0)
                        {
                            var inprocessSamples = inprocessInfo.Find((v) => (v.isWaitingOrInProcess()));// no Error include StatusEnd, Processing，Waiting.
                            if (inprocessSamples == null)
                            {
                                // ラックにある検体は全て分析終了している為、再検査対象となる
                                flgExistsRetestData = true;

                                // 測定指示待機状態
                                dat.WaitMeasureIndicate = true;
                            }
                        }
                    }
                }

                // 再検査コマンドが送信可能であれば送信する
                if (flgExistsRetestData)
                {
                    // 再検査DBでの状態を測定支持問合せ待機状態に更新する。To update the status of the re-inspection DB to measure support inquiry standby state.
                    Singleton<SpecimenReMeasureDB>.Instance.SetDispData(autoRemeasureList);
                    Singleton<SpecimenReMeasureDB>.Instance.CommitSampleReMeasureInfo();
                }
            }
        }

        /// <summary>
        /// 測定完了通知データ送信（測定完了後）
        /// </summary>
        /// <remarks>
        /// 再検査コマンドを送信可能な自動再検査データがあれば送信を行います。
        /// 送信されたデータは検体測定指示問合せの際、DBから削除されます。
        /// </remarks>
        private void sendMeasureCompleteAfterMeasure(CarisXIDString rackID)
        {
            //自動再検が存在する？
            var rackInfo = Singleton<RackInfoManager>.Instance.RackInfo.Find(v => v.RackId.DispPreCharString == rackID.DispPreCharString);
            if ((rackInfo == null) || (rackInfo.IsAutoReMeasure == false))
            {
                //自動再検対象ではない場合、以降の処理は行わない（ラック移動問合せ時に行っている為）
                return;
            }

            //ラックにある検体が全て分析終了ないしエラーとなっているか
            var inprocessInfo = Singleton<InProcessSampleInfoManager>.Instance.SearchInProcessSampleFromRackId(rackID);
            if (inprocessInfo.Count != 0)
            {
                var inprocessSamples = inprocessInfo.Find((v) => (v.isWaitingOrInProcess()));// no Error include StatusEnd, Processing，Waiting.
                if (inprocessSamples != null)
                {
                    //ステータスがWaiting又はInProcessのデータが存在する為、測定完了通知は送信しない
                    return;
                }
            }

            //測定完了通知の送信
            RackTransferCommCommand_0097 cmd0097 = new RackTransferCommCommand_0097();
            cmd0097.RackId = rackID.DispPreCharString;

            //再検有無の判定
            IEnumerable<SpecimenReMeasureGridViewDataSet> autoReMeasure = from v in Singleton<SpecimenReMeasureDB>.Instance.GetDispData(GetAutoRemeasKind.Auto)
                                                                          where v.RackID.DispPreCharString == rackID.DispPreCharString
                                                                          select v;
            if (autoReMeasure.Count() != 0)
                //ReMeasureDBにデータが存在する場合
                cmd0097.ReTestWith = RackTransferCommCommand_0097.ReTestKind.Yes;
            else
                //ReMeasureDBにデータが存在しない場合
                cmd0097.ReTestWith = RackTransferCommCommand_0097.ReTestKind.No;

            Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0097);
        }

        /// <summary>
        /// 分析項目の濃度値平均の算出
        /// </summary>
        /// <remarks>
        /// 項目間演算に用いる分析項目の濃度値平均を算出する
        /// </remarks>
        /// <param name="data">計算データ</param>
        /// <param name="listAssayData">アッセイデータ</param>
        /// <param name="assayData1Concent">濃度値</param>
        /// <param name="assayData2Concent">濃度値</param>
        /// <param name="protocolIndex">項目間演算を行う分析項目</param>
        private void calcAssayDataBetweenItem(CalcData data, List<SpecimenAssayData> listAssayData, ref double assayData1Concent, ref double assayData2Concent, int protocolIndex)
        {
            // アッセイデータの平均計算情報がnullではない場合
            if (data.CalcInfoAverage != null)
            {
                // アッセイデータの濃度平均が有効値である場合
                if (data.CalcInfoAverage.Concentration.HasValue)
                {
                    // アッセイデータの濃度を取得
                    assayData1Concent = data.CalcInfoAverage.Concentration.Value;
                }
            }
            if (data.CalcInfoReplication != null)
            {
                // アッセイデータの濃度平均が有効値である場合
                if (data.CalcInfoReplication.Concentration.HasValue)
                {
                    // アッセイデータの濃度を取得
                    assayData1Concent = data.CalcInfoReplication.Concentration.Value;
                }
            }

            // 項目間演算を行う際に使用するアッセイデータ
            List<SpecimenAssayData> calcPartnerListAssayData = listAssayData.FindAll((assay) => assay.GetMeasureProtocolIndex() == protocolIndex);

            if (calcPartnerListAssayData != null)
            {
                // アッセイデータが複数存在する場合
                if (calcPartnerListAssayData.Count > 1)
                {
                    Double sum = 0;
                    // アッセイデータの濃度を取得
                    for (int i = 0; i < calcPartnerListAssayData.Count; i++)
                    {
                        sum += calcPartnerListAssayData[i].ConcentrationWithoutUnit.Value;
                    }
                    assayData2Concent = sum / calcPartnerListAssayData.Count;

                }
                // アッセイデータが1個だけの場合
                else if (calcPartnerListAssayData.Count == 1)
                {
                    // アッセイデータの濃度を取得
                    assayData2Concent = calcPartnerListAssayData[0].ConcentrationWithoutUnit.Value;
                }
            }
        }

        #endregion

        #region [ [0505]スレーブサブイベントコマンド ]

        /// <summary>
        /// [0505]サブイベントコマンド解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onSlaveSubEvent(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    SlaveCommCommand_0505 cmd0505 = arrayParam[0] as SlaveCommCommand_0505;

                    // サブイベントコマンド解析処理コール
                    this.OnSlaveSubEvent(cmd0505);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0505]サブイベントコマンド解析
        /// </summary>
        /// <param name="cmd0505"></param>
        protected void OnSlaveSubEvent(SlaveCommCommand_0505 cmd0505)
        {
            try
            {
                int moduleId = CarisXSubFunction.MachineCodeToRackModuleIndex((MachineCode)cmd0505.CommNo);
                Singleton<SystemStatus>.Instance.PauseReason[moduleId] = SamplingPauseReason.SAMPLINGPAUSEREASON_DEFAULT;   //一旦エラーを初期化

                switch (cmd0505.SubEvent)
                {
                    case SlaveSubEvent.Wait://1
                                            // 待機状態
                        Singleton<SystemStatus>.Instance.setModuleStatus((RackModuleIndex)moduleId, SystemStatusKind.Standby);
                        Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ReagentChangeIsAllowed, null);
                        break;
                    case SlaveSubEvent.Assay://3
                                             // 分析状態
                        Singleton<SystemStatus>.Instance.setModuleStatus((RackModuleIndex)moduleId, SystemStatusKind.Assay);
                        Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ReagentChangeIsAllowed, null);

                        // 全スレーブが分析状態か否か確認
                        Boolean isAllModuleStatusAssay = Singleton<SystemStatus>.Instance.chkModule1to4Status(SystemStatusKind.Assay, false);

                        // 全モジュールが分析状態の場合かつ、前回のシステムステータスがスタンバイ状態だった場合
                        if ((isAllModuleStatusAssay == true)
                            && ( Singleton<SystemStatus>.Instance.PrevSystemStatus == SystemStatusKind.Standby ))
                        {
                            // ラックの分析開始コマンド送信イベント通知を行う
                            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.RackTransferStartAssay, true);
                        }

                        break;
                    case SlaveSubEvent.SamplingStop://6
                                                    // サンプリング停止
                                                    // not set reagent,can change the assay startOK,Cause when Change Reagent was not Ready ,the "start Assay "button will be not enable
                        switch (cmd0505.SubEventArg1)
                        {
                            case (int)SamplingPauseReason.SamplingPauseReasonBit.PreparationReagent:
                            case (int)SamplingPauseReason.SamplingPauseReasonBit.PreparationDiluent:
                            case (int)(SamplingPauseReason.SamplingPauseReasonBit.PreparationReagent | SamplingPauseReason.SamplingPauseReasonBit.PreparationDiluent):
                                //M、R1、R2試薬または希釈液の準備の為にサンプリング停止した場合、モジュールステータスは変更しない
                                //※他のサンプリング停止要因がある場合はこの限りではない為、switchで分岐させる
                                break;
                            default:
                                //試薬・希釈液の準備以外の場合はモジュールステータスをサンプリング停止状態にする
                                Singleton<SystemStatus>.Instance.setModuleStatus((RackModuleIndex)moduleId, SystemStatusKind.SamplingPause);
                                Singleton<SystemStatus>.Instance.PauseReason[moduleId] = cmd0505.SubEventArg1;
                                break;
                        }

                        break;
                    case SlaveSubEvent.ToAbortAssay://8
                        // システムステータス変更通知
                        Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ChangeAbortAssay);

                        // 試薬の追加・編集は禁止とする。
                        Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ReagentChangeIsRefused, null);
                        break;

                    case SlaveSubEvent.SamplingRestart://7
                        // 分析状態
                        //Singleton<SystemStatus>.Instance.Status = SystemStatusKind.Assay;
                        Singleton<SystemStatus>.Instance.setModuleStatus((RackModuleIndex)moduleId, SystemStatusKind.Assay);
                        Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ReagentChangeIsAllowed, null);
                        break;

                    case SlaveSubEvent.ToAssay://2
                    case SlaveSubEvent.ToSamplingStop://5
                        // 状態移行中

                        // 試薬の追加・編集は禁止とする。
                        Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ReagentChangeIsRefused, null);

                        break;
                }

                // システムステータスアイコン設定通知
                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SetSystemStatusIcon);

                if (Singleton<CarisXSequenceHelperManager>.Instance.Slave[CarisXSubFunction.ModuleIDToModuleIndex(moduleId)].flgInitializeSequenceCompleted)
                {
                    //初期シーケンスが終わっている場合は、ここでレスポンスを返す
                    Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((MachineCode)cmd0505.CommNo, new SlaveCommCommand_1505());
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        #endregion

        #region [ [0506]分析ステータスコマンド ]

        /// <summary>
        /// [0506]分析ステータスコマンド 解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onAssayStatus(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    SlaveCommCommand_0506 cmd0506 = arrayParam[0] as SlaveCommCommand_0506;

                    // 分析ステータスコマンド解析処理コール
                    this.OnAssayStatus(cmd0506);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0506]分析ステータスコマンド解析
        /// </summary>
        /// <remarks>
        /// スレーブより送信された、分析ステータスコマンドに対する処理を行います。
        /// このコマンドは検体分析中に1サイクル毎のタイミングで送信され続けます。
        /// </remarks>
        /// <param name="cmd0506"></param>
        protected void OnAssayStatus(SlaveCommCommand_0506 cmd0506)
        {
            try
            {
                List<SampleKind> commitSampleKindDB = new List<SampleKind>();

                // 分析ステータスコマンド処理
                foreach (Tuple<Int32, Int32, Int32> uniqNoAndRepAndPos in cmd0506.AssayStatus.UniqueNoAndRepAndPosition)
                {
                    SampleInfo sampleInfo = Singleton<InProcessSampleInfoManager>.Instance.SearchInProcessSampleFromUniqueNo(uniqNoAndRepAndPos.Item1);

                    if (sampleInfo != null)
                    {
                        var statusDetail = sampleInfo.GetMeasureProtocolStatusFromUniqueRep(uniqNoAndRepAndPos.Item1, uniqNoAndRepAndPos.Item2);
                        if (statusDetail == SampleInfo.SampleMeasureStatus.Error || statusDetail == SampleInfo.SampleMeasureStatus.End)
                        {
                            // 分析中検体の特定項目・特定多重に対して、エラー・終了状態であればステータス更新しない。
                            String logMsg = String.Format("An error or a completed analysis status has been detected. Unique number = {0} RepIdx = {1} Position = {2}"
                                , uniqNoAndRepAndPos.Item1, uniqNoAndRepAndPos.Item2, uniqNoAndRepAndPos.Item3);
                            System.Diagnostics.Debug.WriteLine(logMsg);
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, logMsg);
                            continue;
                        }

                        sampleInfo.ReactorPosition = uniqNoAndRepAndPos.Item3;
                        if (sampleInfo.IsWaiting())
                        {
                            // 分析ステータス検知初回、分析中検体の全測定項目・全多重に対してInProcessを設定する。
                            sampleInfo.SetMeasureProtocolStatus(SampleInfo.SampleMeasureStatus.InProcess);
                        }

                        // ステータス更新
                        // 各種Assayのテーブルの該当データに対して、残り時間更新（分析ステータスコマンドの位置により時間計算（1サイクルの時間*インデックス）
                        TimeSpan span = CarisXSubFunction.GetTimeSpanFromAssayStatusPosition(uniqNoAndRepAndPos.Item3);

                        if (!commitSampleKindDB.Contains(sampleInfo.SampleKind))
                        {
                            commitSampleKindDB.Add(sampleInfo.SampleKind);
                        }

                        switch (sampleInfo.SampleKind)
                        {
                            case SampleKind.Sample:
                            case SampleKind.Priority:
                                Singleton<SpecimenAssayDB>.Instance.AssayStatusUpdate(uniqNoAndRepAndPos.Item1, uniqNoAndRepAndPos.Item2, span);
                                break;
                            case SampleKind.Control:
                                Singleton<ControlAssayDB>.Instance.AssayStatusUpdate(uniqNoAndRepAndPos.Item1, uniqNoAndRepAndPos.Item2, span);
                                break;
                            case SampleKind.Calibrator:
                                Singleton<CalibratorAssayDB>.Instance.AssayStatusUpdate(uniqNoAndRepAndPos.Item1, uniqNoAndRepAndPos.Item2, span);
                                break;
                        }
                    }
                    else
                    {
                        // 分析中でないユニーク番号が検出された。
                        String logMsg = String.Format("Unique number that is not being analyzed has been detected. Unique number = {0} RepIdx = {1} Position = {2}"
                            , uniqNoAndRepAndPos.Item1, uniqNoAndRepAndPos.Item2, uniqNoAndRepAndPos.Item3);
                        System.Diagnostics.Debug.WriteLine(logMsg);
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, logMsg);
                    }
                }

                foreach (var kindDB in commitSampleKindDB)
                {
                    switch (kindDB)
                    {
                        case SampleKind.Sample:
                        case SampleKind.Priority:
                            Singleton<SpecimenAssayDB>.Instance.CommitData();
                            break;
                        case SampleKind.Control:
                            Singleton<ControlAssayDB>.Instance.CommitData();
                            break;
                        case SampleKind.Calibrator:
                            Singleton<CalibratorAssayDB>.Instance.CommitData();
                            break;
                    }
                }
                // ステータス画面 分析情報更新
                RealtimeDataAgent.LoadAssayData();

                // モジュールIDの取得
                int moduleIndex = CarisXSubFunction.MachineCodeToModuleIndex((MachineCode)cmd0506.CommNo);

                // 温度情報更新
                Singleton<PublicMemory>.Instance.moduleTemperature[moduleIndex]
                    .SetDPRTemperatureFromSlaveTemperature(cmd0506.AssayStatus.TemperatureTable);

                // 温度更新通知
                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.UpdateTemperature);

                // 連続発生しなくなったエラー蓄積データの削除と比較時間の取得
                Singleton<ErrorDataStorageListManeger>.Instance.ErrorDataStorageList[moduleIndex].DeleteFilteringData();
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }

            // レスポンスを返す
            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((MachineCode)cmd0506.CommNo, new SlaveCommCommand_1506());
        }

        #endregion

        #region [ [0508]残量コマンド(スレーブ用) ]

        /// <summary>
        /// [0508]残量コマンド(スレーブ用) 解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onReagentRemainCommandFromSlave(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    SlaveCommCommand_0508 cmd0508 = arrayParam[0] as SlaveCommCommand_0508;

                    // 残量コマンド(スレーブ用)解析処理コール
                    this.OnReagentRemainCommandFromSlave(cmd0508);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0508]残量コマンド(スレーブ用)解析
        /// </summary>
        /// <param name="cmd0508"></param>
        protected void OnReagentRemainCommandFromSlave(SlaveCommCommand_0508 cmd0508)
        {
            try
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, String.Format("OnReagentRemainCommandFromSlave"));

                CarisXSubFunction.SetReagentRemain(cmd0508, true, CarisXSubFunction.MachineCodeToRackModuleIndex((MachineCode)cmd0508.CommNo), false);

                // 表示更新
                RealtimeDataAgent.LoadReagentRemainData();

                SlaveCommCommand_1508 cmd1508 = new SlaveCommCommand_1508();
                IRackRemainAmountInfoSet rackRemainAmountSet = cmd1508; // インターフェースの実装クラスをrefで渡せない為、ここで作業用にインターフェース型へ移し変える。内容の設定される実体はコマンドクラス。
                Singleton<ReagentDB>.Instance.GetRackReagentRemain(ref rackRemainAmountSet);
                if (Singleton<CarisXCommManager>.Instance.GetRackTransferCommStatus() != ConnectionStatus.Online)
                {
                    //ラックと接続されていない場合
                    cmd1508.ExistWasteTank = false; //タンクなし
                    cmd1508.IsFullWasteTank = true; //タンク満杯
                }

                //レスポンスを返す
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((MachineCode)cmd0508.CommNo, cmd1508);
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }

        }

        #endregion

        #region [ [0510]マスターカーブ情報コマンド ]

        /// <summary>
        /// [0510]マスターカーブ情報コマンド 解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onMasterCurveCommand(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    SlaveCommCommand_0510 cmd0510 = arrayParam[0] as SlaveCommCommand_0510;

                    // マスターカーブ情報コマンド解析処理コール
                    this.OnMasterCurveCommand(cmd0510);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0510]マスターカーブ情報コマンド解析
        /// </summary>
        /// <remarks>
        /// スレーブより送信された、マスターカーブコマンドに対する処理を行います。
        /// このコマンドは、試薬ボトル準備時、試薬バーコードが読込まれた際に送信されます。
        /// </remarks>
        /// <param name="cmd0510"></param>
        protected void OnMasterCurveCommand(SlaveCommCommand_0510 cmd0510)
        {
            try
            {
                // マスターカーブ使用可否取得
                // プロトコルNoが有効でなければ終了
                if (cmd0510.MasterCurve.Length != 0)
                {
                    var curveInfo = cmd0510.MasterCurve.First();
                    var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(curveInfo.ProtoNo);
                    if ( (protocol == null)
                        || (protocol.CalibType.IsQualitative() == true) )
                    {
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                            String.Format("The master curve was discarded because it was set to not use it or IsQualitative  ProtoNo = {0}", curveInfo.ProtoNo));
                        return;
                    }

                    // プロトコルNo,ReagLotからマスターカーブを消す
                    Singleton<CalibrationCurveDB>.Instance.LoadDB();

                    Int32 moduleNo = CarisXSubFunction.MachineCodeToRackModuleIndex((MachineCode)cmd0510.CommNo);

                    foreach (var masterCurve in cmd0510.MasterCurve)
                    {
                        protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(masterCurve.ProtoNo);
                        if (protocol != null)
                        {
                            // 同一ロットのマスターカーブを削除
                            this.deleteMasterCurve(protocol.ProtocolIndex, cmd0510.LotNumber);

                            for (int i = 0; i < masterCurve.PointCount; i++)
                            {
                                Singleton<CalibrationCurveDB>.Instance.AddCalibData( moduleNo
                                    , cmd0510.ReagCode
                                    , cmd0510.LotNumber
                                    , null               // ラックID固定でnull
                                    , null               // ラックPos固定でnull
                                    , protocol.ProtocolIndex
                                    , 1                  // 多重測定回数ID固定で1
                                    , masterCurve.ConcAry[i].ToString()
                                    , masterCurve.CountAry[i]
                                    , i + 1
                                    , null               // カウント平均固定でnull
                                    , CarisXConst.MASTER_CURVE_UNIQUE_NO
                                    , CarisXConst.MASTER_CURVE_DATE );
                            }

                        }
                    }

                    Singleton<CalibrationCurveDB>.Instance.CommitData();
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }

            // レスポンスを返す
            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((MachineCode)cmd0510.CommNo, new SlaveCommCommand_1510());
        }

        /// <summary>
        /// マスターカーブ削除
        /// </summary>
        /// <remarks>
        /// マスターカーブを削除します
        /// </remarks>
        /// <param name="protocolNo"></param>
        /// <param name="reagentLotNo"></param>
        protected void deleteMasterCurve(Int32 protocolIndex, String reagentLotNo)
        {
            var curveData = Singleton<CalibrationCurveDB>.Instance.GetData(protocolIndex, reagentLotNo, CarisXConst.MASTER_CURVE_DATE);
            curveData.ForEach((v) =>
            {
                v.DeleteData();
            });

            // 削除を適用
            Singleton<CalibrationCurveDB>.Instance.SetCalibData(curveData);
            Singleton<CalibrationCurveDB>.Instance.CommitData();
        }

        #endregion

        #region [ [0511]バージョン通知コマンド(スレーブ用) ]

        /// <summary>
        /// [0511]バージョン通知コマンド(スレーブ用) 解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onVersionCommandFromSlave(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    SlaveCommCommand_0511 cmd0511 = arrayParam[0] as SlaveCommCommand_0511;

                    // バージョン通知コマンド(スレーブ用)解析処理コール
                    this.OnVersionCommandFromSlave(cmd0511);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0511]バージョン通知コマンド(スレーブ用)解析
        /// </summary>
        /// <param name="cmd0511"></param>
        protected void OnVersionCommandFromSlave(SlaveCommCommand_0511 cmd0511)
        {
            try
            {
                // バージョンコマンド通知
                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SlaveVersion, cmd0511);
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        #endregion

        #region [ [0512]試薬ロット確認コマンド ]

        /// <summary>
        /// [0512]試薬ロット確認コマンド 解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onReagentLotChangeCommand(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    SlaveCommCommand_0512 cmd0512 = arrayParam[0] as SlaveCommCommand_0512;

                    // 試薬ロット確認コマンド解析処理コール
                    this.OnReagentLotChangeCommand(cmd0512);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0512]試薬ロット確認コマンド解析
        /// </summary>
        /// <param name="cmd0512"></param>
        protected void OnReagentLotChangeCommand(SlaveCommCommand_0512 cmd0512)
        {
            try
            {
                Int32 moduleIndex = CarisXSubFunction.MachineCodeToModuleIndex((MachineCode)cmd0512.CommNo);

                // 試薬ロット切替わり処理
                Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleIndex].ChangeReagentLot(cmd0512);
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        #endregion

        #region [ [0513]キャリブレーション測定確認コマンド ]

        /// <summary>
        /// [0513]キャリブレーション測定確認コマンド 解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onCalibMeasureCommand(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    SlaveCommCommand_0513 cmd0513 = arrayParam[0] as SlaveCommCommand_0513;

                    // キャリブレーション測定確認コマンド解析処理コール
                    this.OnCalibMeasureCommand(cmd0513);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0513]キャリブレーション測定確認コマンド解析
        /// </summary>
        /// <param name="cmd0513"></param>
        protected void OnCalibMeasureCommand(SlaveCommCommand_0513 cmd0513)
        {
            try
            {
                //レスポンスを返す
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((MachineCode)cmd0513.CommNo, new SlaveCommCommand_1513());
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        #endregion

        #region [ [0514]総アッセイ数通知コマンド ]

        /// <summary>
        /// [0514]総アッセイ数通知コマンド 解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onAssayTotalCount(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    SlaveCommCommand_0514 cmd0514 = arrayParam[0] as SlaveCommCommand_0514;

                    // 総アッセイ数通知コマンド解析処理コール
                    this.OnAssayTotalCount(cmd0514);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0514]総アッセイ数通知コマンド解析
        /// </summary>
        /// <param name="cmd0514"></param>
        protected void OnAssayTotalCount(SlaveCommCommand_0514 cmd0514)
        {
            try
            {
                Int32 moduleIndex = CarisXSubFunction.MachineCodeToModuleIndex((MachineCode)cmd0514.CommNo);

                // 数を記録しておく（表示用）
                SupplieParameter supplie = Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param;

                supplie.SlaveList[moduleIndex].TotalAssay = cmd0514.AssayTotalCount;

                Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Save();
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }

            //レスポンスを返す
            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((MachineCode)cmd0514.CommNo, new SlaveCommCommand_1514());
        }

        #endregion

        #region [ [0515]ラック設置状況コマンド ]

        /// <summary>
        /// [0515]ラック設置状況コマンド 解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onRackStatus(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    SlaveCommCommand_0515 cmd0515 = arrayParam[0] as SlaveCommCommand_0515;

                    // ラック設置状況コマンド解析処理コール
                    this.OnRackStatus(cmd0515);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0515]ラック設置状況コマンド解析
        /// </summary>
        /// <remarks>
        /// スレーブより送信された、ラック設置状況コマンドに対する処理を行います。
        /// このコマンドは、ラックの設置状況が変化した際に送信されます。
        /// </remarks>
        /// <param name="cmd0515"></param>
        protected void OnRackStatus(SlaveCommCommand_0515 cmd0515)
        {
            try
            {
                Int32 moduleID = CarisXSubFunction.MachineCodeToRackModuleIndex((MachineCode)cmd0515.CommNo);
                int moduleIndex = CarisXSubFunction.ModuleIDToModuleIndex(moduleID);


                // ラック設置状況初期化
                Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[moduleIndex].RackSettingStatusInitialize();
                // ラック引込奥ステータス設定
                Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[moduleIndex].RackPullBackStatus = cmd0515.RackRetractionBack;
                // ラック引込手前ステータス設定
                Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[moduleIndex].RackPullFrontStatus = cmd0515.RackRetractionFront;
                // STATステータス設定
                Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[moduleIndex].STATStatus = cmd0515.STATInstallation;
                // 外部搬送ステータス設定
                Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[moduleIndex].OutsideTransfer = cmd0515.OutsideTransfer;
                // 順番設定
                Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[moduleIndex].TurnOrder = cmd0515.TurnOrder;
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }

            //レスポンスを返す
            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((MachineCode)cmd0515.CommNo, new SlaveCommCommand_1515());
        }

        #endregion

        #region [ [0516]試薬テーブル回転SW押下通知コマンド ]

        /// <summary>
        /// [0516]試薬テーブル回転SW押下通知コマンド 解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onTableTurnCommand(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    SlaveCommCommand_0516 cmd0516 = arrayParam[0] as SlaveCommCommand_0516;

                    // 試薬テーブル回転SW押下通知コマンド解析処理コール
                    this.OnTableTurnCommand(cmd0516);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0516]試薬テーブル回転SW押下通知コマンド解析
        /// </summary>
        /// <param name="cmd0516"></param>
        protected void OnTableTurnCommand(SlaveCommCommand_0516 cmd0516)
        {
            try
            {
                Int32 moduleIndex = CarisXSubFunction.MachineCodeToModuleIndex((MachineCode)cmd0516.CommNo);

                // ポート番号指定、又は0(1/10回転)で試薬テーブル回転
                Singleton<FormSetReagent>.Instance.TurnTable(moduleIndex);
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }

            //レスポンスを返す
            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((MachineCode)cmd0516.CommNo, new SlaveCommCommand_1516());
        }

        #endregion

        #region [ [0520]試薬設置状況通知コマンド ]

        /// <summary>
        /// [0520]試薬設置状況通知コマンド 解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onReagentStatusCommand(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    SlaveCommCommand_0520 cmd0520 = arrayParam[0] as SlaveCommCommand_0520;

                    // 試薬設置状況通知コマンド解析処理コール
                    this.OnReagentStatusCommand(cmd0520);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0520]試薬設置状況通知コマンド解析
        /// </summary>
        /// <param name="cmd0520"></param>
        protected void OnReagentStatusCommand(SlaveCommCommand_0520 cmd0520)
        {
            try
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, String.Format("OnReagentStatusCommand"));

                //モジュールIDを取得
                Int32 moduleId = CarisXSubFunction.MachineCodeToRackModuleIndex((MachineCode)cmd0520.CommNo);

                //受信した試薬情報の内容をチェック
                IRemainAmountInfoSet remainAmountInfo = cmd0520; //インターフェースの実装クラスをrefで渡せない為、ここで作業用にインターフェース型へ移し変える。
                this.chkReagentRemain(ref remainAmountInfo, moduleId);

                //残量情報に設定する（残量は上書きしない）
                CarisXSubFunction.SetReagentRemain(remainAmountInfo, false, moduleId, true);

                //レスポンス用
                SlaveCommCommand_1520 cmd1520 = new SlaveCommCommand_1520();
                IRemainAmountInfoSet rspRemainAmountInfo = cmd1520; // インターフェースの実装クラスをrefで渡せない為、ここで作業用にインターフェース型へ移し変える。内容の設定される実体はコマンドクラス。
                Singleton<ReagentDB>.Instance.GetReagentRemain(ref rspRemainAmountInfo, moduleId);
                Singleton<ReagentHistoryDB>.Instance.GetReagentHistory(ref rspRemainAmountInfo);

                //試薬準備完了のレスポンスの情報がある場合、結果が0：成功以外のポートの残量をゼロにする
                if (!(Singleton<PublicMemory>.Instance.prepareResult == null))
                {
                    //試薬準備完了結果による残量のクリア処理を実施
                    this.clearReagentRemainByPrepareResult(ref rspRemainAmountInfo, moduleId);
                }

                //レスポンスを返す
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((MachineCode)cmd0520.CommNo, cmd1520);

            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// 試薬情報をチェックする
        /// </summary>
        /// <param name="remain">残量情報</param>
        protected void chkReagentRemain(ref IRemainAmountInfoSet remain, Int32 moduleID)
        {
            // 残量情報のマッチングチェック

            //残量情報を初期化する為の値を設定
            ReagentRemainTable brankReagentRemainTable = new ReagentRemainTable();
            brankReagentRemainTable.ReagType = 0;
            brankReagentRemainTable.ReagCode = 0;
            brankReagentRemainTable.RemainingAmount.Remain = 0;
            brankReagentRemainTable.RemainingAmount.LotNumber = "";
            brankReagentRemainTable.RemainingAmount.SerialNumber = 0;
            brankReagentRemainTable.RemainingAmount.TermOfUse = DateTime.MinValue;
            brankReagentRemainTable.MakerCode = "";
            brankReagentRemainTable.Capacity = 0;

            //残量履歴情報からパラメータの残量情報に対応するグループ番号のリストを取得する
            List<Int64> groupNoList = Singleton<ReagentHistoryDB>.Instance.GetGroupNoList(remain);

            //試薬ポート毎に処理を行う（試薬ポート数はREAGENT_PORT_MAXで取得）
            for (int i = 0; i < CarisXConst.REAGENT_PORT_MAX; i++)
            {
                String strPortNo = String.Format("PortNo = {0} ", i + 1);

                //前処理液が設定されている場合は処理を行わない
                if (remain.ReagentRemainTable[i * 3].ReagTypeDetail == ReagentTypeDetail.T1)
                {
                    continue;
                }

                //M、R2試薬のいずれかがセットされていない場合は実施しない
                if (remain.ReagentRemainTable[i * 3 + 1].ReagCode == 0 || remain.ReagentRemainTable[i * 3 + 2].ReagCode == 0)
                {
                    continue;
                }

                //グループ番号のリストから、今処理中のポートの内容（M、R1orT1、R2orT2の３要素分）を抽出する＝＞対象グループ番号リスト
                List<Int64> targetGroupNoList = groupNoList.GetRange(i * 3, 3);

                //取得した対象グループ番号リストでグループ番号が一致しているか？
                if (targetGroupNoList.Distinct().Count() != 1)
                {
                    //一致していない場合
                    if (targetGroupNoList[0] == 0)
                    {
                        //Rが新規（履歴情報なし）の場合
                        //200-1をエラー履歴に登録
                        CarisXSubFunction.WriteDPRErrorHist(new DPRErrorCode(200, 1, moduleID), 0, strPortNo);
                    }
                    else
                    {
                        //Mが新規、またはMとRがどちらも別の組合せで使用中の場合
                        //200-2をエラー履歴に登録
                        CarisXSubFunction.WriteDPRErrorHist(new DPRErrorCode(200, 2, moduleID), 0, strPortNo);
                    }

                    //該当ポートの情報をクリアする
                    remain.ReagentRemainTable[(i * 3) + 0] = brankReagentRemainTable;
                    remain.ReagentRemainTable[(i * 3) + 1] = brankReagentRemainTable;
                    remain.ReagentRemainTable[(i * 3) + 2] = brankReagentRemainTable;
                }
            }
        }

        /// <summary>
        /// 試薬準備完了結果による残量のクリア処理
        /// </summary>
        /// <remarks>
        /// 試薬準備完了の結果が特定の値となっているポートは残量情報、残量履歴情報で管理している残量をゼロにする
        /// </remarks>
        /// <param name="RemainAmountSet">試薬情報</param>
        /// <param name="moduleID">対象のモジュールID</param>
        protected void clearReagentRemainByPrepareResult(ref IRemainAmountInfoSet RemainAmountSet, Int32 moduleID)
        {
            String dbgMsgHead = String.Format("[[Investigation log]]clearReagentRemainByPrepareResult ");
            String dbgMsg = dbgMsgHead;

            if (Singleton<PublicMemory>.Instance.prepareResult == null)
            {
                dbgMsg += String.Format("prepareResult is null.");

                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);

                return;
            }

            //試薬準備完了コマンドのレスポンスのデータ分繰り返す
            for (int i = 0; i < Singleton<PublicMemory>.Instance.prepareResult.Length; i++)
            {
                if (Singleton<PublicMemory>.Instance.prepareResult[i] == 0)
                {
                    //結果が成功の場合は次のデータを処理
                    continue;
                }

                dbgMsg = dbgMsgHead + String.Format("port:{0} clear remain {1} ", i + 1, Singleton<PublicMemory>.Instance.prepareResult[i]);

                //各残量をクリアする際のデバッグメッセージ出力用匿名メソッドを定義
                Action<int, IRemainAmountInfoSet> outputDebugMsgClearReagent = (int index, IRemainAmountInfoSet remain) =>
                {
                    dbgMsg = dbgMsg + String.Format("index:{0} TypeDetail:{1} Lot:{2} Serial:{3} ", index
                                                                    , remain.ReagentRemainTable[index].ReagTypeDetail
                                                                    , remain.ReagentRemainTable[index].RemainingAmount.LotNumber
                                                                    , remain.ReagentRemainTable[index].RemainingAmount.SerialNumber);

                };

                //結果に不備がある試薬の残量をクリアする
                if (Singleton<PublicMemory>.Instance.prepareResult[i].HasFlag(ReagentPreparationErrorTarget.R1orT1))
                {
                    //R1orT1の試薬情報をクリア
                    RemainAmountSet.ReagentRemainTable[(i * 3) + 0].RemainingAmount.Remain = 0;
                    outputDebugMsgClearReagent((i * 3) + 0, RemainAmountSet);
                }

                if (Singleton<PublicMemory>.Instance.prepareResult[i].HasFlag(ReagentPreparationErrorTarget.R2orT2))
                {
                    //R2orT2の試薬情報をクリア
                    RemainAmountSet.ReagentRemainTable[(i * 3) + 1].RemainingAmount.Remain = 0;
                    outputDebugMsgClearReagent((i * 3) + 1, RemainAmountSet);
                }

                if (Singleton<PublicMemory>.Instance.prepareResult[i].HasFlag(ReagentPreparationErrorTarget.M))
                {
                    //Mの試薬情報をクリア
                    RemainAmountSet.ReagentRemainTable[(i * 3) + 2].RemainingAmount.Remain = 0;
                    outputDebugMsgClearReagent((i * 3) + 2, RemainAmountSet);
                }

                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
            }

            //残量情報に設定する（残量は上書きする）
            RemainAmountSet.TimeStamp = DateTime.Now;
            CarisXSubFunction.SetReagentRemain(RemainAmountSet, true, moduleID, false);

            //試薬準備完了のレスポンスの情報をクリア
            for (Int32 i = 0; i < Singleton<PublicMemory>.Instance.prepareResult.Count(); i++)
            {
                Singleton<PublicMemory>.Instance.prepareResult[i] = 0;
            }

            Singleton<PublicMemory>.Instance.prepareResult = null;

        }

        #endregion

        #region [ [0521]廃液タンク状態問合せコマンド ]

        /// <summary>
        /// [0521]廃液タンク状態問合せコマンド 解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onWasteTankStatusCommand(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    SlaveCommCommand_0521 cmd0521 = arrayParam[0] as SlaveCommCommand_0521;

                    // 廃液タンク状態問合せコマンド解析処理コール
                    this.OnWasteTankStatusCommand(cmd0521);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0521]廃液タンク状態問合せコマンド解析
        /// </summary>
        /// <param name="cmd0521"></param>
        protected void OnWasteTankStatusCommand(SlaveCommCommand_0521 cmd0521)
        {
            try
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, String.Format("OnWasteTankStatusCommand"));

                SlaveCommCommand_1521 cmd1521 = new SlaveCommCommand_1521();
                IRackRemainAmountInfoSet rackRemainAmountSet = cmd1521; // インターフェースの実装クラスをrefで渡せない為、ここで作業用にインターフェース型へ移し変える。内容の設定される実体はコマンドクラス。
                Singleton<ReagentDB>.Instance.GetRackReagentRemain(ref rackRemainAmountSet);
                if (Singleton<CarisXCommManager>.Instance.GetRackTransferCommStatus() != ConnectionStatus.Online)
                {
                    //ラックと接続されていない場合
                    cmd1521.ExistWasteTank = false; //タンクなし
                    cmd1521.IsFullWasteTank = true; //タンク満杯
                }

                //レスポンスを返す
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((MachineCode)cmd0521.CommNo, cmd1521);
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        #endregion

        #region [ [0522]キャリブレータ情報通知コマンド ]

        /// <summary>
        /// [0522]キャリブレータ情報通知コマンド 解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onNotifyCalibratorInfoCommand(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    SlaveCommCommand_0522 cmd0522 = arrayParam[0] as SlaveCommCommand_0522;

                    // キャリブレータ情報通知コマンド解析処理コール
                    this.OnNotifyCalibratorInfoCommand(cmd0522);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0522]キャリブレータ情報通知コマンド解析
        /// </summary>
        /// <param name="cmd0522"></param>
        protected void OnNotifyCalibratorInfoCommand(SlaveCommCommand_0522 cmd0522)
        {
            try
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, String.Format("OnNotifyCalibratorInfoCommand"));

                // キャリブレータ情報初期化
                Singleton<CalibratorInfoManager>.Instance.CalibratorInfoInitialize();

                // キャリブレータ情報に受信した内容を記録する
                // 参照渡しにならないよう個別に値を設定する
                CalibratorInfo CalibratorInfo = new CalibratorInfo();
                CalibratorInfo.ModuleId = CarisXSubFunction.MachineCodeToRackModuleIndex((MachineCode)cmd0522.CommNo);
                CalibratorInfo.PortNo = cmd0522.PortNo;
                CalibratorInfo.ReagentCode = cmd0522.ReagCode;
                CalibratorInfo.CalibratorLotCount = cmd0522.CalibratorLotCount;
                CalibratorInfo.CalibratorLot = new List<CalibratorLot>();
                for (int i = 0; i < CalibratorInfo.CalibratorLotCount; i++)
                {
                    // 補正ポイントリスト生成
                    List<Double> conc = new List<Double>();
                    for (int j = 0; j < cmd0522.CalibratorLot[i].ConcCount; j++)
                    {
                        conc.Add(cmd0522.CalibratorLot[i].Concentration[j]);
                    }

                    // キャリブレータロット情報設定（ロット番号、補正ポイント数、補正ポイントリスト）
                    CalibratorInfo.CalibratorLot.Add(new CalibratorLot(cmd0522.CalibratorLot[i].CalibratorLotNo, cmd0522.CalibratorLot[i].ConcCount, conc));
                }

                // キャリブレータ情報設定
                Singleton<CalibratorInfoManager>.Instance.SetCalibratorInfo(CalibratorInfo);
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }

            //レスポンスを返す
            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((MachineCode)cmd0522.CommNo, new SlaveCommCommand_1522());
        }

        #endregion

        #region [ [0591]STAT状態通知コマンド ]

        /// <summary>
        /// [0591]STAT状態通知コマンド 解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onNotifySTATStatus(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    SlaveCommCommand_0591 cmd0591 = arrayParam[0] as SlaveCommCommand_0591;

                    // STAT状態通知コマンド解析処理コール
                    this.OnNotifySTATStatus(cmd0591);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0591]STAT状態通知コマンド解析
        /// </summary>
        /// <param name="cmd0591"></param>
        protected void OnNotifySTATStatus(SlaveCommCommand_0591 cmd0591)
        {
            try
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                                CarisXLogInfoBaseExtention.Empty, String.Format("OnNotifySTATStatus"));

                // 状態に応じてSTAT状態通知(0491)を送る必要があるため、先にスレーブへレスポンスを返す
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((MachineCode)cmd0591.CommNo, new SlaveCommCommand_1591());

                // STAT状態取得
                STATStatus recvStatus = cmd0591.Status;

                // モジュール番号およびIndexを取得
                int moduleId = CarisXSubFunction.MachineCodeToRackModuleIndex((MachineCode)cmd0591.CommNo);
                int moduleIndex = CarisXSubFunction.ModuleIDToModuleIndex(moduleId);

                // モジュールステータスを取得
                SystemStatusKind moduleStatusKind = Singleton<SystemStatus>.Instance.ModuleStatus[moduleId];

                // 接続台数を取得
                int connectMax = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;

                // STAT状態設定
                Singleton<SystemStatus>.Instance.SetSTATStatus(moduleIndex, recvStatus);

                // 状態に応じてSTAT状態通知コマンドを送信する
                SlaveCommCommand_0491 cmd0491 = new SlaveCommCommand_0491();

                //ラックIDを作成（ラックIDがバッティングすると上書きしてしまうので、E001～E004の形になるようにする）
                StatRackID statRackID = new StatRackID();
                statRackID.Value = moduleId;

                Dictionary<Int32, SlaveCommCommand_0491> sendList = new Dictionary<Int32, SlaveCommCommand_0491>();
                List<SpecimenStatRegistrationGridViewDataSet> statRegisteredList = new List<SpecimenStatRegistrationGridViewDataSet>();

                switch (moduleStatusKind)
                {
                    // 待機中
                    case SystemStatusKind.Standby:
                        // 何もしない
                        break;

                    // 分析中
                    case SystemStatusKind.Assay:
                        switch (recvStatus)
                        {
                            // 受付可
                            case STATStatus.Acceptable:
                                // STAT登録状態を確認
                                Boolean existsSTAT = false;

                                statRegisteredList = Singleton<SpecimenStatDB>.Instance.GetDispData();
                                if ((statRegisteredList != null) && (statRegisteredList.Count() > 0))
                                {
                                    // 一時登録の有無
                                    var statTemporaryList = statRegisteredList.Where(x => (x.RegistType == RegistType.Temporary)).ToList();

                                    if (statTemporaryList != null && statTemporaryList.Count() > 0)
                                    {
                                        foreach (Tuple<int?, int?, int?> regist in statTemporaryList.First().Registered)
                                        {
                                            // 分析項目Indexをチェック
                                            if (regist.Item1 == null)
                                            {
                                                // nullの場合、飛ばす
                                                continue;
                                            }
                                            int protocolIndex = (int)regist.Item1;

                                            // 分析項目情報取得
                                            var measProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(protocolIndex);

                                            // 試薬が残っているか
                                            String reagentLotNo = Singleton<ReagentDB>.Instance.GetNowReagentLotNo(measProtocol.ReagentCode, moduleId: moduleId);
                                            if (reagentLotNo != String.Empty)
                                            {
                                                //対象となるSTATの登録情報有り
                                                existsSTAT = true;
                                                break;
                                            }
                                        }

                                        //STATがいる場合、STAT測定可能モジュール画面更新用の通知を行う
                                        if (existsSTAT)
                                        {
                                            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.UpdateStatMeasurableModule, sendList.Keys.ToList());
                                        }
                                    }

                                    if (!existsSTAT)
                                    {
                                        // 一時登録が無かった場合、固定登録の有無を確認
                                        var statFixedList = statRegisteredList.Where(x => (x.RegistType == RegistType.Fixed)
                                                                                       && (x.RackPosition == moduleId)).ToList();
                                        if (statFixedList != null && statFixedList.Count() > 0)
                                        {
                                            // 固定登録がある場合 => SW押下待ち
                                            existsSTAT = true;
                                        }
                                    }
                                }

                                if (existsSTAT)
                                    // STAT登録がある => SW押下待ち
                                    cmd0491.Request = STATStatusRequest.WaitSWPress;
                                else
                                    // STAT登録がない => 待機
                                    cmd0491.Request = STATStatusRequest.Wait;

                                sendList.Add(moduleIndex, cmd0491);

                                break;

                            // SW押下
                            case STATStatus.SWPressed:
                                // 検体取り込み
                                cmd0491.Request = STATStatusRequest.SampleUptake;
                                sendList.Add(moduleIndex, cmd0491);

                                //ラックポジションにSTATのラックIDを追加（ラックIDがバッティングすると上書きしてしまうので、E001～E004の形になるようにする）
                                Singleton<RackPositionManager>.Instance.RackPositionInitialize();
                                Singleton<RackPositionManager>.Instance.SetRackPosition(statRackID, (RackPositionKind)moduleId);
                                break;

                            // 検体無し
                            case STATStatus.NoSample:
                                // SW押下待ち
                                cmd0491.Request = STATStatusRequest.WaitSWPress;
                                sendList.Add(moduleIndex, cmd0491);
                                break;

                            // 検体有り
                            case STATStatus.OkSample:
                                // 待機
                                statRegisteredList = Singleton<SpecimenStatDB>.Instance.GetDispData();
                                for (int otherModuleIndex = 0; otherModuleIndex < connectMax; otherModuleIndex++)
                                {
                                    Int32 otherModuleId = CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)otherModuleIndex);

                                    // 検体有りを返してきたモジュール以外のSW押下待ちのモジュールに待機またはSW押下待ちを送信
                                    if (otherModuleIndex != moduleIndex && Singleton<SystemStatus>.Instance.ModuleStatus[otherModuleId] == SystemStatusKind.Assay)
                                    {
                                        cmd0491 = new SlaveCommCommand_0491();

                                        // 固定登録の有無
                                        var statFixedList = statRegisteredList.Where(x => (x.RegistType == RegistType.Fixed)
                                                                                       && (x.RackPosition == otherModuleId)).ToList();
                                        if (statFixedList != null && statFixedList.Count() > 0)
                                            cmd0491.Request = STATStatusRequest.WaitSWPress;    //固定登録有りならSW押下待ち
                                        else
                                            cmd0491.Request = STATStatusRequest.Wait;           //固定登録無しなら待機

                                        sendList.Add(otherModuleIndex, cmd0491);
                                    }
                                }
                                break;

                            // 吸引完了
                            case STATStatus.Aspiration:
                                // 排出
                                cmd0491.Request = STATStatusRequest.Emission;
                                sendList.Add(moduleIndex, cmd0491);
                                //ラックポジションからSTATのラックIDを削除（ラックIDがバッティングすると上書きしてしまうので、E001～E004の形になるようにする）
                                Singleton<RackPositionManager>.Instance.RackPositionInitialize();
                                Singleton<RackPositionManager>.Instance.SetRackPosition(statRackID, RackPositionKind.Rack);
                                break;

                            default:
                                break;
                        }

                        break;

                    // サンプリング停止中
                    case SystemStatusKind.SamplingPause:
                        // 何もしない
                        break;
                }

                // STAT状態通知コマンドの送信が必要な場合
                if (sendList.Count() > 0)
                {
                    foreach (var send in sendList)
                    {
                        // 接続状態チェック
                        if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(send.Key) == ConnectionStatus.Online)
                        {
                            // STAT状態通知コマンド送信
                            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(send.Key, send.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        #endregion

        #region [ [0596]分取完了コマンド ]

        /// <summary>
        /// [0596]分取完了コマンド 解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onSampleAspirationCompleted(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    SlaveCommCommand_0596 cmd0596 = arrayParam[0] as SlaveCommCommand_0596;

                    // 分取完了コマンド解析処理コール
                    this.OnSampleAspirationCompleted(cmd0596);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0596]分取完了コマンド解析
        /// </summary>
        /// <remarks>
        /// スレーブより送信された、分取完了コマンドに対する応答を行います。
        /// ラックへ分取完了コマンドの送信を行います。
        /// </remarks>
        /// <param name="cmd0596"></param>
        protected void OnSampleAspirationCompleted(SlaveCommCommand_0596 cmd0596)
        {
            try
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                                CarisXLogInfoBaseExtention.Empty, String.Format("OnSampleAspirationCompleted"));

                //スレーブへレスポンスを返す
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((MachineCode)cmd0596.CommNo, new SlaveCommCommand_1596());

                //ラックへ分取完了コマンドを送信する
                RackTransferCommCommand_0096 respCommand = new RackTransferCommCommand_0096();
                respCommand.RackId = cmd0596.RackID;
                respCommand.DeviceNo = CarisXSubFunction.MachineCodeToRackModuleIndex((MachineCode)cmd0596.CommNo);

                Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(respCommand);
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        #endregion

        ///////////////////////////////////////
        // ホスト関連コマンド
        ///////////////////////////////////////
        #region [ [0004]装置ステータス問合せコマンド ]

        /// <summary>
        /// [0004]装置ステータス問合せコマンド 解析-引数変換処理
        /// </summary>
        /// <param name="param"></param>
        private void onAskSlaveStatus(Object param)
        {
            try
            {
                if (param is Object[])
                {
                    Object[] arrayParam = param as Object[];

                    // 引数変換
                    HostCommCommand_0004 cmd0004 = arrayParam[0] as HostCommCommand_0004;

                    // 装置ステータス問合せコマンド解析処理コール
                    this.OnAskSlaveStatus(cmd0004);
                }
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// [0004]装置ステータス問合せコマンド解析
        /// </summary>
        /// <param name="cmd0515"></param>
        protected void OnAskSlaveStatus(HostCommCommand_0004 cmd0004)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(String.Format("FormMainFrame::rspAskSlaveStatus"));
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                                CarisXLogInfoBaseExtention.Empty, String.Format("FormMainFrame::rspAskSlaveStatus"));
                HostCommCommand_0005 respCommand = new HostCommCommand_0005();

                switch (Singleton<SystemStatus>.Instance.Status)
                {
                    case SystemStatusKind.Standby:
                    case SystemStatusKind.WaitSlaveResponce:
                    case SystemStatusKind.ToEndAssay:
                    case SystemStatusKind.ReagentExchange:
                        respCommand.Status = HostCommCommand_0005.InspectionAcceptStatus.WaitingInspectionAccept;
                        break;
                    case SystemStatusKind.Assay:
                    case SystemStatusKind.SamplingPause:
                        respCommand.Status = HostCommCommand_0005.InspectionAcceptStatus.AnalysingInspectionAccept;
                        break;
                    case SystemStatusKind.NoLink:
                    case SystemStatusKind.Shutdown:
                        respCommand.Status = HostCommCommand_0005.InspectionAcceptStatus.NotInspectionAccept;
                        break;
                }

                // 応答コマンド送信
                Singleton<CarisXCommManager>.Instance.PushSendQueueHost(respCommand);
            }
            catch (Exception ex)
            {
                // Exceptionログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty, String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));
            }
        }

        #endregion

    }
}
