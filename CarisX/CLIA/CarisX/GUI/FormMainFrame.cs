using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Oelco.CarisX.Calculator;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Common;
using Oelco.CarisX.Const;
using Oelco.CarisX.DB;
using Oelco.CarisX.Log;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Status;
using Oelco.CarisX.Utility;
using Oelco.Common.Comm;
using Oelco.Common.DB;
using Oelco.Common.GUI;
using Oelco.Common.Log;
using Oelco.Common.Parameter;
using Oelco.Common.Utility;
using Oelco.CarisX.Maintenance;
using System.Reflection;
using System.Threading.Tasks;
using System.Text;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 共通メニュー画面クラス
    /// </summary>
    public partial class FormMainFrame : FormTransitionBase
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 画面釦による分析終了フラグ
        /// </summary>
        //private Boolean endAssayFromFormButton = false;

        /// <summary>
        /// 自動立ち上げ待機画面
        /// </summary>
        DlgAutoSetup dlgAutoSetup;

        /// <summary>
        /// 初期シーケンス進捗率管理変数
        /// </summary>
        /// <remarks>
        /// RackModuleIndexの総数分配列を確保する
        /// </remarks>
        ProgressInfo[] progressInfos = new ProgressInfo[Enum.GetValues(typeof(RackModuleIndex)).Length];

        /// <summary>
        /// 初期化画面
        /// </summary>
        private DlgInitialize dlgStartup = null;

        /// <summary>
        /// シャットダウンシーケンス完了フラグ（ラック）
        /// </summary>
        private Boolean compRackEndSequence = false;
        /// <summary>
        /// シャットダウンシーケンスステータス（ラック）
        /// </summary>
        private Boolean statusRackEndSequence = false;

        /// <summary>
        /// シャットダウンシーケンス完了フラグ（スレーブ）
        /// </summary>
        private Boolean[] compSlaveEndSequence;
        /// <summary>
        /// シャットダウンシーケンスステータス（スレーブ）
        /// </summary>
        private Boolean[] statusSlaveEndSequence;

        /// <summary>
        /// 自動起動シーケンス完了フラグ（ラック）
        /// </summary>
        private Boolean compRackAutoStartUpSequence = false;
        /// <summary>
        /// 自動起動シーケンスステータス（ラック）
        /// </summary>
        private Boolean statusRackAutoStartUpSequence = false;

        /// <summary>
        /// 自動起動シーケンス完了フラグ（モジュール）
        /// </summary>
        private Boolean[] compSlaveAutoStartUpSequence;
        /// <summary>
        /// 自動起動シーケンスステータス（モジュール）
        /// </summary>
        private Boolean[] statusSlaveAutoStartUpSequence;

        private Rectangle rectangle;

        /// <summary>
        /// 手動分析ポーズフラグ（画面でAssay Pauseを行ったかどうかのフラグ
        /// </summary>
        /// <remarks>
        /// ON （true） …ポーズボタンをクリック時
        /// OFF（false）…システムステータスがサンプリングポーズ以外になった時
        /// </remarks>
        private Boolean flgManualAssayPause = false;

        /// <summary>
        /// コマンド受信処理
        /// </summary>
        public CarisXReceiveCommandThread onRecieveCommandThread = new CarisXReceiveCommandThread();


        /// <summary>
        /// 装置操作の現在の日付記録
        /// </summary>
        private DateTime deviceCurrentDate = DateTime.Now;

        /// <summary>
        /// モジュール毎の試薬交換待ちラベル情報
        /// ラベル - 点滅有無
        /// </summary>
        private Dictionary<ModuleIndex, ReagentMenuBtnManager> lblReagentSetupWaitMinutesList = new Dictionary<ModuleIndex, ReagentMenuBtnManager>();

        /// <summary>
        /// 試薬メニューボタン管理
        /// </summary>
        public class ReagentMenuBtnManager
        {
            /// <summary>
            /// 試薬交換待ちラベル
            /// </summary>
            public CustomLabel ReagentSetupWaitMinutesLabel;

            /// <summary>
            /// 表示フラグ
            /// </summary>
            public Boolean IsVisibleLabel;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="labelControl"></param>
            /// <param name="isVisible"></param>
            public ReagentMenuBtnManager(CustomLabel labelControl, Boolean isVisible)
            {
                // ラベルコントロール設定
                this.ReagentSetupWaitMinutesLabel = labelControl;

                // 表示フラグ設定
                this.IsVisibleLabel = isVisible;
            }
        }

        // スレーブボタンリスト
        private Dictionary<ModuleIndex, SlaveBtnManager> btnSlaveList = new Dictionary<ModuleIndex, SlaveBtnManager>();

        /// <summary>
        /// スレーブボタンボタン管理
        /// </summary>
        /// <remarks>
        /// スレーブボタンは常に活性状態とし、文字色のみ非活性状態として見えるようするために本クラスを作成
        /// [!!!注意!!!]スレーブボタンのEnabledコントロールを直接変更しないようにする
        /// </remarks>
        public class SlaveBtnManager
        {
            // ボタンのEnable色（白色）
            private readonly Color COLOR_SLAVE_BTN_ENABLED = Color.FromArgb(0xFF, 0xFF, 0xFF);

            // ボタンのDisable色（灰色）
            private readonly Color COLOR_SLAVE_BTN_DISABLED = Color.FromArgb(0x6D, 0x6D, 0x6D);

            /// <summary>
            /// スレーブボタン
            /// </summary>
            public BlinkButton BtnSlave;

            /// <summary>
            /// 活性状態
            /// </summary>
            private Boolean enabled;

            /// <summary>
            /// 活性状態
            /// </summary>
            public Boolean Enabled
            {
                get
                {
                    // 活性状態を返す
                    return this.enabled;
                }

                set
                {
                    // 活性状態を設定
                    this.enabled = value;

                    // 変更する活性状態をチェック
                    if (this.enabled == true)
                    {
                        // 活性状態とする場合、文字色を活性時の色に変更
                        this.BtnSlave.NormalAppearance.ForeColor = COLOR_SLAVE_BTN_ENABLED;
                        this.BtnSlave.ToggleAppearance.ForeColor = COLOR_SLAVE_BTN_ENABLED;
                    }
                    else
                    {
                        // 非活性状態とする場合、文字色を非活性時の色に変更
                        this.BtnSlave.NormalAppearance.ForeColor = COLOR_SLAVE_BTN_DISABLED;
                        this.BtnSlave.ToggleAppearance.ForeColor = COLOR_SLAVE_BTN_DISABLED;
                    }
                }
            }

            /// <summary>
            /// 表示状態
            /// </summary>
            public Boolean Visible
            {
                get
                {
                    // ボタンの表示状態を返す
                    return this.BtnSlave.Visible;
                }

                set
                {
                    // ボタンの表示状態を切り替え
                    this.BtnSlave.Visible = value;
                }
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="btnControl"></param>
            public SlaveBtnManager(BlinkButton btnControl)
            {
                // ボタンコントロール設定
                this.BtnSlave = btnControl;

                // 活性状態を非活性に変更
                this.Enabled = false;

                // 表示状態を非表示に設定
                this.Visible = false;
            }
        }

        #endregion

        /// <summary>
        /// 初期シーケンス進捗通知
        /// </summary>
        /// <remarks>
        /// 初期シーケンスの進捗をダイアログに通知します
        /// </remarks>
        /// <param name="value">進捗情報</param>
        protected void initializeProgressNotify(Object value)
        {
            String dbgMsg = String.Format("[[Investigation log]]FormMainFrame::{0} ", MethodBase.GetCurrentMethod().Name);
            
            ProgressInfo info = value as ProgressInfo;

            progressInfos[(Int32)info.TargetModuleNo] = info;

            if ((info != null) && (this.dlgStartup != null))
            {
                //初期シーケンス画面が起動している場合
                dbgMsg = dbgMsg + String.Format("RackModuleIndex = {0} progressPos = {1}, ", info.TargetModuleNo, info.ProgressPos);

                //初期シーケンス画面に受信した進捗状況を渡す
                this.dlgStartup.SetProgressInfo(info);

                //初期シーケンスが完了した時、対象のモジュールのステータスを設定する
                if (info.ProgressPos == this.dlgStartup.ProgressEndPos)
                {
                    Singleton<SystemStatus>.Instance.setModuleStatus(info.TargetModuleNo, SystemStatusKind.Standby);

                    dbgMsg = dbgMsg + String.Format("Progress Standby");

                    //メイン画面の
                    switch (info.TargetModuleNo)
                    {
                        case RackModuleIndex.Module1:
                            this.btnSlave1.Enabled = true;
                            break;
                        case RackModuleIndex.Module2:
                            this.btnSlave2.Enabled = true;
                            break;
                        case RackModuleIndex.Module3:
                            this.btnSlave3.Enabled = true;
                            break;
                        case RackModuleIndex.Module4:
                            this.btnSlave4.Enabled = true;
                            break;
                    }
                    this.btnSlaveTotal.Enabled = true;
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ModuleConnect, info.TargetModuleNo);
                }

                dbgMsg = dbgMsg + String.Format("ProgressPos = {0} || Status = {1}", info.ProgressPos, Singleton<SystemStatus>.Instance.ModuleStatus[(Int32)info.TargetModuleNo]);

                if (((info.ProgressPos == this.dlgStartup.ProgressEndPos)
                      || (Singleton<SystemStatus>.Instance.ModuleStatus[(Int32)info.TargetModuleNo] == SystemStatusKind.MotorError))
                    && this.dlgStartup.ChkCanClose(progressInfos))
                {
                    // モーラーエラー発生中の場合、その旨のダイアログボックスを表示します。
                    if ((Singleton<Status.SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.RackTransfer] == Status.SystemStatusKind.MotorError)
                        || (Singleton<Status.SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.Module1] == Status.SystemStatusKind.MotorError)
                        || (Singleton<Status.SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.Module2] == Status.SystemStatusKind.MotorError)
                        || (Singleton<Status.SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.Module3] == Status.SystemStatusKind.MotorError)
                        || (Singleton<Status.SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.Module4] == Status.SystemStatusKind.MotorError))
                    {
                        dbgMsg = dbgMsg + String.Format("MotorError");
                        // モーターエラーのラック、モジュールを取得
                        string rackModuleIndices = CarisXSubFunction.GetMotorErrorRackModule();

                        this.dlgStartup.Close();
                        this.dlgStartup = null;

                        // モーターエラーダイアログボックスの表示
                        DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_267, rackModuleIndices, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.Confirm);
                    }
                    else
                    {
                        this.dlgStartup.Close();
                        this.dlgStartup = null;
                    }
                    dbgMsg = dbgMsg + String.Format("Startup Close");
                }
            }

            if (info.ProgressPos == 0)
            {
                // 無接続状態に戻す
                Singleton<SystemStatus>.Instance.setModuleStatus(info.TargetModuleNo, SystemStatusKind.NoLink);

                dbgMsg = dbgMsg + String.Format("Progress noLink");

                if(this.dlgStartup == null)
                {
                    if (this.dlgStartup != null)
                    {
                        this.dlgStartup.Close();
                        this.dlgStartup = null;
                    }

                    dbgMsg = dbgMsg + String.Format("Startup Show");

                    this.dlgStartup = new DlgInitialize(progressInfos);
                    this.dlgStartup.ShowDialog();
                }
            }

            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
        }

        /// <summary>
        /// 初期シーケンス一時停止
        /// </summary>
        /// <remarks>
        /// スレーブがDPR起動時点で起動していない場合に、サブレディコマンド待機状態となる際呼び出されます。
        /// </remarks>
        /// <param name="value">不使用</param>
        protected void initializePause(Object value)
        {
            ProgressInfo info = value as ProgressInfo;

            progressInfos[(Int32)info.TargetModuleNo] = info;

            if (this.dlgStartup != null)
            {
                this.dlgStartup.SetProgressInfo(info);
                if (this.dlgStartup.ChkCanClose(progressInfos))
                {
                    this.dlgStartup.Close();
                    this.dlgStartup = null;
                }
            }
        }

        /// <summary>
        /// 初期シーケンス再開
        /// </summary>
        /// <remarks>
        /// サブレディコマンド待機状態が完了した際に呼び出されます。
        /// </remarks>
        /// <param name="value">不使用</param>
        protected void initializeReStart(Object value)
        {
            if (this.dlgStartup != null)
            {
                this.dlgStartup.Close();
                this.dlgStartup = null;
            }
            this.dlgStartup = new DlgInitialize(progressInfos);
            this.dlgStartup.ShowDialog();
        }


        /// <summary>
        /// ファイル保持設定一括取得
        /// </summary>
        /// <remarks>
        /// 外部ファイルに保持されているデータの全取得を行います。
        /// </remarks>
        private void readAllStaticParameter()
        {
            // 分析項目ファイル読込
            Singleton<MeasureProtocolManager>.Instance.LoadAllMeasureProtocol();
            //Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Load();
            //change with the method of Encrypiton
            Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.LoadEncryption();
            Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.SyncMeasProtocolManager(Singleton<MeasureProtocolManager>.Instance);

            //parameterList.ForEach((param)=>param.Load

            // アプリケーション設定読込
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Load();

            // UI設定読込
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Load();

            // システムパラメータ読込
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Load();
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleSequenceNumberHistory.Distinct();

            // モータパラメータ読込
            Singleton<ParameterFilePreserve<CarisXMotorParameter>>.Instance.LoadRaw();

            // サンサーパラメータ読込
            Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance.LoadRaw();

            // 消耗品パラメータ読込
            Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Load();

            // メンテナンス日誌（ユーザー用）パラメータファイル読み出し
            Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.LoadRaw();

            // メンテナンス日誌（サービスマン用）パラメータファイル読み出し
            Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.LoadRaw();

            // TODO:必要パラメータ読込随時追加
        }

        /// <summary>
        /// 画面リストの取得
        /// </summary>
        private List<FormBase> childFormList
        {
            get
            {
                List<FormBase> childs = new List<FormBase>()
                {
                    Singleton<FormAssay>.Instance,
                    Singleton<FormCalibAnalysis>.Instance,
                    Singleton<FormCalibRegistration>.Instance,
                    Singleton<FormCalibResult>.Instance,
                    Singleton<FormCalibStatus>.Instance,
                    Singleton<FormControlQC>.Instance,
                    Singleton<FormControlRegistration>.Instance,
                    Singleton<FormControlResult>.Instance,
                    Singleton<FormSetReagent>.Instance,
                    Singleton<FormSpecimenRegistration>.Instance,
                    Singleton<FormSpecimenStatRegistration>.Instance,
                    Singleton<FormSpecimenResult>.Instance,
                    Singleton<FormSpecimenRetest>.Instance,
                    Singleton<FormSystemAnalytes>.Instance,
                    Singleton<FormSystemLog>.Instance,
                    Singleton<FormSystemOption>.Instance,
                    Singleton<FormSystemUserControl>.Instance,
                    Singleton<FormSystemModuleOption>.Instance
                };

                return childs;
            }
        }

        /// <summary>
        /// 使用DBアクセスクラスリスト 取得
        /// </summary>
        private List<DBAccessControl> dbAccessList
        {
            get
            {
                // シングルトンのインスタンスなので、メンバに持たずに都度一時的な領域を取る。
                // 利用箇所でも、短期の参照のみとすること。
                List<DBAccessControl> targetDbAccessList = new List<DBAccessControl>()
                {
                    Singleton<DBAccessControl>.Instance,
                    Singleton<SpecimenGeneralDB>.Instance,
                    Singleton<SpecimenStatDB>.Instance,
                    Singleton<SpecimenAssayDB>.Instance,
                    Singleton<SpecimenResultDB>.Instance,
                    Singleton<SpecimenReMeasureDB>.Instance,
                    Singleton<SpecimenStatReMeasureDB>.Instance,
                    Singleton<CalibrationCurveDB>.Instance,
                    Singleton<CalibratorAssayDB>.Instance,
                    Singleton<CalibratorRegistDB>.Instance,
                    Singleton<CalibratorResultDB>.Instance,
                    Singleton<ControlRegistDB>.Instance,
                    Singleton<ControlAssayDB>.Instance,
                    Singleton<ControlResultDB>.Instance,
                    Singleton<ReagentDB>.Instance,
                    Singleton<ReagentHistoryDB>.Instance,
                    Singleton<UserInfoDB>.Instance,
                    Singleton<ErrorLogDB>.Instance,
                    Singleton<OperationLogDB>.Instance,
                    Singleton<ParameterChangeLogDB>.Instance,
                    Singleton<AnalyzeLogDB>.Instance,
                    Singleton<MasterErrorLogDB>.Instance,
                };
                return targetDbAccessList;
            }
        }

        /// <summary>
        /// シーケンス番号リストの取得
        /// </summary>
        private List<NumberingBase> sequencialNumberList
        {
            get
            {
                List<NumberingBase> targetNumberlingList = new List<NumberingBase>()
                {
                    Singleton<IndividuallyNo>.Instance,
                    Singleton<ReceiptNo>.Instance,
                    Singleton<SequencialCalibNo>.Instance,
                    Singleton<SequencialControlNo>.Instance,
                    Singleton<SequencialPrioritySampleNo>.Instance,
                    Singleton<SequencialSampleNo>.Instance,
                    Singleton<UniqueNo>.Instance
                };
                return targetNumberlingList;
            }
        }
        //private List<IPreserveParameter> parameterList
        //{
        //    get
        //    {
        //        List<IPreserveParameter> targetParameterList = new List<IPreserveParameter>()
        //        {
        //            Singleton<ParameterFilePreserve<AppSettings>>.Instance,
        //            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance,
        //            Singleton<ParameterFilePreserve<ErrorCodeDataManager>>.Instance,
        //            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance,
        //            Singleton<ParameterFilePreserve<UnitConfig>>.Instance,
        //            Singleton<ParameterFilePreserve<CarisXConfiguParameter>>.Instance,
        //            Singleton<ParameterFilePreserve<CarisXMotorParameter>>.Instance,
        //            Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance,
        //            Singleton<ParameterFilePreserve<ControlQC>>.Instance,
        //            Singleton<ParameterFilePreserve<SampleRequireAmount>>.Instance,
        //            Singleton<ParameterFilePreserve<SupplieParameter>>.Instance
        //        };
        //        return targetParameterList;
        //    }
        //}

        /// <summary>
        /// DBインスタンスの初期化を行います。
        /// </summary>
        /// <remarks>
        /// DBオープンを行います
        /// </remarks>
        private void initializeDBInstance()
        {
            // TODO:DBオープン 接続設定（Windows認証）
            // TODO:
            //Singleton<DBAccessControl>.Instance.Initialize( "0120B", "CLIA", 1000, 1000 );
            Boolean dbInitialized = Singleton<SQLServerDBAccess>.Instance.Initialize(Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.DbAccess.InstanceName, Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.DbAccess.Databasename, 1000, 1000, CarisXConst.PathData + @"\CALISX.mdf");

            //Singleton<SQLServerDBAccess>.Instance.Initialize( Environment.MachineName, "CLIA", 1000, 1000, CarisXConst.PathData + @"\CLIA.mdf" );
            //Singleton<SQLServerDBAccess>.Instance.Open();

            // シングルトンのインスタンスなので、メンバに持たずに一時的な領域を取る
            foreach (DBAccessControl dbInstance in this.dbAccessList)
            {
                if (!dbInstance.Open())
                {
                    MessageBox.Show(String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MSG_195));
                    System.Diagnostics.Debug.WriteLine(String.Format("{0}:Open failed", dbInstance.GetType().Name));
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, String.Format("{0}:Open失敗", dbInstance.GetType().Name));
                    //// 一つのDBがOpen失敗する場合（接続不能な場合）、全てのDBでタイムアウトまで待機して失敗する為ここで処理を抜ける。
                    //break;
                }
                if (dbInitialized)
                {
                    // DB接続初期化に失敗
                    // DBデータを取得する
                    dbInstance.LoadDB();
                }
            }
            System.Threading.Thread.Sleep(5000);
        }

        /// <summary>
        /// DBのカスタイマイズ
        /// </summary>
        /// <remarks>
        /// データ型の変更およびカラム追加など、DBの設定を調整する
        /// </remarks>
        private void customizeDB()
        {
            // DBのアクセスファイルパスをデバッグログへ出力
            String accessPath = Singleton<DBAccessControl>.Instance.GetDBAccessFilePath();
            Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog
                                                      , Singleton<CarisXUserLevelManager>.Instance.NowUserID
                                                      , CarisXLogInfoBaseExtention.Empty
                                                      , String.Format("CarisX DB access path = [{0}]", accessPath));

            // 同じくコンソールに出力
            Console.WriteLine(String.Format("CarisX DB access path = [{0}]", accessPath));

            // エラー履歴テーブルにカウンターカラムを追加
            Singleton<ErrorLogDB>.Instance.AddColumn();
            Singleton<MasterErrorLogDB>.Instance.AddColumn();

            // 試薬履歴テーブルの設置日のデータ型の変換
            Singleton<ReagentHistoryDB>.Instance.ChangeTableDataType();

            // キャリブレータ登録テーブルに登録分析モジュール番号カラムを追加
            Singleton<CalibratorRegistDB>.Instance.AddColumn();

            // 検量線テーブルにモジュール番号カラムを追加
            Singleton<CalibrationCurveDB>.Instance.AddColumn();
        }

        /// <summary>
        /// DBインスタンスの開放を行います。
        /// </summary>
        /// <remarks>
        /// DBインスタンスの開放を行います
        /// </remarks>
        private void disposeDBInstance()
        {
            foreach (DBAccessControl dbInstance in this.dbAccessList)
            {
                dbInstance.Close();
            }
        }

        /// <summary>
        /// ユニーク番号をリセットする
        /// </summary>
        /// <remarks>
        /// ユニーク番号をファイルで保持しているとバッティングすることが頻発したため、DBから値を直接取得するように変更
        /// </remarks>
        private void resetUniqueNo()
        {
            Int32 uniqueNo = 0;

            // 一般検体測定結果よりユニーク番号の最大値を取得
            Int32 specimenResultMaxUniqueNo = Singleton<SpecimenResultDB>.Instance.GetMaxUniqueNo();
            if (uniqueNo < specimenResultMaxUniqueNo)
            {
                uniqueNo = specimenResultMaxUniqueNo;
            }
            // キャリブレーター測定結果よりユニーク番号の最大値を取得
            Int32 calibratorResultMaxUniqueNo = Singleton<CalibratorResultDB>.Instance.GetMaxUniqueNo();
            if (uniqueNo < calibratorResultMaxUniqueNo)
            {
                uniqueNo = calibratorResultMaxUniqueNo;
            }
            // 精度管理検体測定結果よりユニーク番号の最大値を取得
            Int32 controlResultMaxUniqueNo = Singleton<ControlResultDB>.Instance.GetMaxUniqueNo();
            if (uniqueNo < controlResultMaxUniqueNo)
            {
                uniqueNo = controlResultMaxUniqueNo;
            }

            // ユニーク番号の設定をリセットする
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.UniqueNoInfo.CountNow = uniqueNo;
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Save();
        }

        /// <summary>
        /// エラー釦点滅
        /// </summary>
        /// <remarks>
        /// エラー釦点滅を行います
        /// </remarks>
        /// <param name="value"></param>
        private void blinkErrorButton(Object value)
        {
            // エラー釦点滅
            this.btnError.BlinkStart(Oelco.CarisX.Properties.Resources.Image_ErrButton_Yellow, Oelco.CarisX.Properties.Resources.Image_ErrButton_Red, 250, 1);
        }

        /// <summary>
        /// 試薬釦点滅
        /// </summary>
        /// <remarks>
        /// 試薬準備開始コマンドが応答（タイムアウト）されると試薬釦点滅します
        /// </remarks>
        /// <param name="value"></param>
        private void blinkReagentButton(Object value)
        {
            // 試薬交換用通知オブジェクトにキャスト可能か確認
            if (value is NotifyObjectSetReagent)
            {
                // 試薬交換用通知オブジェクトにキャスト
                NotifyObjectSetReagent notifyObjectSetReagent = (NotifyObjectSetReagent)value;

                // 試薬準備開始コマンドが応答されるまで（最大17.7分）の間、タイマーラベルが表示され、
                // タイムアウト、準備開始コマンド応答により釦が点滅します。
                // 試薬準備画面が開いていた場合、ボタンは点滅しない
                if (!Singleton<FormSetReagent>.Instance.IsVisible)
                {
                    // 試薬釦点滅
                    this.btnMenuLargeReagent.BlinkStart(Oelco.CarisX.Properties.Resources.Image_ReagentOFF
                                                      , Oelco.CarisX.Properties.Resources.Image_ReagentON
                                                      , 500
                                                      , 1);
                }

                // 試薬準備確認コマンドの応答イベント処理
                Singleton<FormSetReagent>.Instance.WaitRespComplete((ModuleIndex)notifyObjectSetReagent.ModuleIndex);
            }
            else
            {
                // キャスト失敗のため、処理なし
            }
        }

        /// <summary>
        /// 試薬準備確認応答
        /// </summary>
        /// <remarks>
        /// 試薬準備確認応答コマンド受信を試薬準備画面へ通知します
        /// </remarks>
        /// <param name="value"></param>
        private void reagentPrepareCheckResponse(Object value)
        {
            // 試薬交換用通知オブジェクトにキャスト可能か確認
            if (value is NotifyObjectSetReagent)
            {
                // 試薬交換用通知オブジェクトにキャスト
                NotifyObjectSetReagent notifyObjectSetReagent = (NotifyObjectSetReagent)value;

                // 試薬準備確認応答コマンド受信を試薬準備画面へ通知
                Singleton<FormSetReagent>.Instance.SetCmdResPrepareCheck((FormSetReagent.ReagentChangeTargetKind)notifyObjectSetReagent.ObjectValue
                                                                       , (ModuleIndex)notifyObjectSetReagent.ModuleIndex);
            }
            else
            {
                // キャスト失敗のため、処理なし
            }
        }

        /// <summary>
        /// 試薬準備開始応答
        /// </summary>
        /// <remarks>
        /// 試薬準備開始応答コマンド受信を試薬準備画面へ通知します
        /// </remarks>
        /// <param name="value"></param>
        private void commonPrepareStartResponse(Object value)
        {
            // 試薬交換用通知オブジェクトにキャスト可能か確認
            if (value is NotifyObjectSetReagent)
            {
                // 試薬交換用通知オブジェクトにキャスト
                NotifyObjectSetReagent notifyObjectSetReagent = (NotifyObjectSetReagent)value;

                // 試薬準備開始応答コマンド受信を試薬準備画面へ通知
                Singleton<FormSetReagent>.Instance.SetCmdResCommonPrepareStart((FormSetReagent.ReagentChangeTargetKind)notifyObjectSetReagent.ObjectValue
                                                                             , (ModuleIndex)notifyObjectSetReagent.ModuleIndex);
            }
            else
            {
                // キャスト失敗のため、処理なし
            }
        }

        /// <summary>
        /// 試薬準備完了
        /// </summary>
        /// <remarks>
        /// 試薬準備完了応答コマンド受信を試薬準備画面へ通知します
        /// </remarks>
        /// <param name="value"></param>
        private void reagentPrepareCompleteResponse(Object value)
        {
            // 試薬交換用通知オブジェクトにキャスト可能か確認
            if (value is NotifyObjectSetReagent)
            {
                // 試薬交換用通知オブジェクトにキャスト
                NotifyObjectSetReagent notifyObjectSetReagent = (NotifyObjectSetReagent)value;

                // 試薬準備完了したここでブリンク終了する。
                this.btnMenuLargeReagent.BlinkEnd();

                // 試薬準備完了応答コマンド受信を試薬準備画面へ通知
                Singleton<FormSetReagent>.Instance.SetCmdResPrepareComplete((FormSetReagent.ReagentChangeTargetKind)notifyObjectSetReagent.ObjectValue
                                                                          , (ModuleIndex)notifyObjectSetReagent.ModuleIndex);
            }
            else
            {
                // キャスト失敗のため、処理なし
            }
        }

        /// <summary>
        /// 試薬残量変更確認応答
        /// </summary>
        /// <remarks>
        /// 試薬残量変更確認応答コマンド受信を試薬準備画面へ通知します
        /// </remarks>
        /// <param name="value"></param>
        private void changeReagentRemainResponse(Object value)
        {
            // 試薬交換用通知オブジェクトにキャスト可能か確認
            if (value is NotifyObjectSetReagent)
            {
                // 試薬交換用通知オブジェクトにキャスト
                NotifyObjectSetReagent notifyObjectSetReagent = (NotifyObjectSetReagent)value;

                // 試薬残量変更確認応答コマンド受信を試薬準備画面へ通知
                Singleton<FormSetReagent>.Instance.SetCmdResChangeReagentRemain((Boolean)notifyObjectSetReagent.ObjectValue
                                                                              , (ModuleIndex)notifyObjectSetReagent.ModuleIndex);
            }
            else
            {
                // キャスト失敗のため、処理なし
            }
        }

        /// <summary>
        /// 汎用残量変更コマンド受信
        /// </summary>
        /// <remarks>
        /// 残量変更コマンド受信コマンド受信を試薬準備画面へ通知します
        /// </remarks>
        /// <param name="value"></param>
        private void changeCommonRemainResponse(Object value)
        {
            // 試薬交換用通知オブジェクトにキャスト可能か確認
            if (value is NotifyObjectSetReagent)
            {
                // 試薬交換用通知オブジェクトにキャスト
                NotifyObjectSetReagent notifyObjectSetReagent = (NotifyObjectSetReagent)value;

                // オブジェクト情報を文字列に変換し、カンマ区切りで分割
                String[] cmdtext = notifyObjectSetReagent.ObjectValue.ToString().Split(',');

                try
                {

                    // 試薬種別
                    ReagentKind reagKind = (ReagentKind)int.Parse(cmdtext[0]);
                    // ポート番号
                    int portNo = int.Parse(cmdtext[1]);
                    // 残量
                    int remain = int.Parse(cmdtext[2]);
                    // ロット番号
                    String lotNumber = cmdtext[3];
                    // シリアル番号
                    int serialNumber = int.Parse(cmdtext[4]);

                    // 残量変更コマンド受信を試薬準備画面へ通知
                    Singleton<FormSetReagent>.Instance.SetCmdResChangeCommonRemain(reagKind
                                                                                  , portNo
                                                                                  , remain
                                                                                  , lotNumber
                                                                                  , serialNumber
                                                                                  , (ModuleIndex)notifyObjectSetReagent.ModuleIndex);
                }
                catch(Exception ex)
                {
                    // 例外ログ出力
                    System.Diagnostics.Debug.WriteLine(String.Format("{0} {1}", ex.Message, ex.StackTrace));
                    Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("{0} {1}", ex.Message, ex.StackTrace));
                }
            }
            else
            {
                // キャスト失敗のため、処理なし
            }
        }

        /// <summary>
        /// 試薬交換開始
        /// </summary>
        /// <remarks>
        /// 試薬交換開始します
        /// </remarks>
        /// <param name="minutes"></param>
        private void startReagentTimer(Object value)
        {
            // 試薬交換用通知オブジェクトにキャスト可能か確認
            if (value is NotifyObjectSetReagent)
            {
                // 試薬交換用通知オブジェクトにキャスト
                NotifyObjectSetReagent notifyObjectSetReagent = (NotifyObjectSetReagent)value;

                // 待機時間
                Int32 waitMinute = Int32.Parse(notifyObjectSetReagent.ObjectValue.ToString());
                if (waitMinute != 0)
                {
                    // 対象モジュールIndexを取得
                    ModuleIndex targetModuleIndex = (ModuleIndex)notifyObjectSetReagent.ModuleIndex;

                    // 時間フォーマットを指定
                    this.lblReagentSetupWaitMinutesList[targetModuleIndex].ReagentSetupWaitMinutesLabel.TimeFormatString = "mm";

                    // カウントダウンタイマー開始
                    this.lblReagentSetupWaitMinutesList[targetModuleIndex].ReagentSetupWaitMinutesLabel.StartCountDown(TimeSpan.FromMinutes(waitMinute));

                    // ラベル表示フラグをON
                    this.lblReagentSetupWaitMinutesList[targetModuleIndex].IsVisibleLabel = true;

                    // 表示中のモジュールIndexと同じか確認
                    if (targetModuleIndex == Singleton<PublicMemory>.Instance.moduleIndex)
                    {
                        // 同じ場合は試薬交換待ちラベルを表示
                        this.lblReagentSetupWaitMinutesList[targetModuleIndex].ReagentSetupWaitMinutesLabel.Visible = true;
                    }
                }
                else
                {
                    // 待機時間取得失敗のため、処理なし
                }
            }
            else
            {
                // キャスト失敗のため、処理なし
            }
        }

        /// <summary>
        /// 試薬交換中断
        /// </summary>
        /// <remarks>
        /// 試薬交換中断します
        /// </remarks>
        /// <param name="minutes"></param>
        private void cancelReagentTimer(Object value)
        {
            // 試薬交換用通知オブジェクトにキャスト可能か確認
            if (value is NotifyObjectSetReagent)
            {
                // 試薬交換用通知オブジェクトにキャスト
                NotifyObjectSetReagent notifyObjectSetReagent = (NotifyObjectSetReagent)value;

                // 全モジュールが対象か確認
                if(notifyObjectSetReagent.ModuleIndex == CarisXConst.ALL_MODULEID)
                {
                    // 全モジュール対象
                    foreach (ModuleIndex targetModuleIndex in Enum.GetValues(typeof(ModuleIndex)))
                    {
                        // カウントダウンタイマー停止
                        this.lblReagentSetupWaitMinutesList[targetModuleIndex].ReagentSetupWaitMinutesLabel.AbortCountDown();
                    }
                }
                else
                {
                    // 対象モジュールのカウントダウンタイマー停止
                    this.lblReagentSetupWaitMinutesList[(ModuleIndex)notifyObjectSetReagent.ModuleIndex].ReagentSetupWaitMinutesLabel.AbortCountDown();
                }
            }
            else
            {
                // キャスト失敗のため、処理なし
            }
        }

        /// <summary>
        /// 試薬保冷庫移動応答
        /// </summary>
        /// <remarks>
        /// 試薬保冷庫移動応答コマンド受信を試薬準備画面へ通知します
        /// </remarks>
        /// <param name="value"></param>
        private void reagentCoolerMoveResponse(Object value)
        {
            // 試薬保冷庫移動応答コマンド受信を試薬準備画面へ通知
            Singleton<DlgTurnTable>.Instance.SetCmdResReagentCoolerComplete((DlgTurnTable)value);
        }

        /// <summary>
        /// 途中状態のデータ削除
        /// </summary>
        /// <remarks>
        /// 途中状態のデータを削除します
        /// </remarks>
        private void clearHalfwayAssayData()
        {
            // 強制終了するなどして、分析DBに残った分析中・待機の検体情報を削除する。
            // 分析DB全消去
            Singleton<SpecimenAssayDB>.Instance.ClearWaitAndInprocess();
            Singleton<SpecimenAssayDB>.Instance.CommitData();

            // 設定ファイルとあわせる
            // ファイルにあってDBに無い場合（DB内のデータが一定条件で削除されている場合）のデータが同期される。
            // DBにあってファイルにない場合（ファイル書き込み失敗等による異常）のデータは同期されない。
            // 後述のケースはシーケンス番号範囲設定時デバッグログに出力され、変更が失敗する。
            var individuallyNoListFile = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param
                .SampleSequenceNumberHistory.History.Select((v) => v.IndividuallyNumber).ToList();
            var individuallyListDB = Singleton<SpecimenAssayDB>.Instance.GetData().Select((v) => v.GetIndividuallyNo());

            foreach (var dbInd in individuallyListDB)
            {
                individuallyNoListFile.RemoveAll((value) => value == dbInd);
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleSequenceNumberHistory.SelectClearHistory(individuallyNoListFile);
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Save();

            Singleton<CalibratorAssayDB>.Instance.ClearWaitAndInprocess();
            Singleton<CalibratorAssayDB>.Instance.CommitData();

            Singleton<ControlAssayDB>.Instance.ClearWaitAndInprocess();
            Singleton<ControlAssayDB>.Instance.CommitData();

            // 再測定DBは、起動時・測定終了時にすべて削除する
            Singleton<SpecimenReMeasureDB>.Instance.DeleteAll();
            Singleton<SpecimenStatReMeasureDB>.Instance.DeleteAll();

            // 再検査画面の表示データを再読み込みする
            RealtimeDataAgent.LoadReMeasureSampleData();

        }

        /// <summary>
        /// 試薬設置ガイダンス表示
        /// </summary>
        /// <param name="value">null:全モジュール対象、null以外:モジュール指定</param>
        /// <remarks>パラメータ切り替え時は全モジュールが対象であり、試薬交換時はモジュール指定となる</remarks>
        private void showReagentSetGuidance(Object value)
        {
            // 1台構成の場合、処理なし
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected == 1)
            {
                return;
            }

            // ターンテーブル上の試薬情報を取得
            var reagentDatas = CarisXSubFunction.GetReagentNamesInTurnTable();

            // 試薬情報が無い場合、処理なし
            if (reagentDatas.Count == 0)
            {
                return;
            }

            // モジュール番号を設定（初期値=モジュール1）
            Int32 moduleIndex = 0;

            // 画面遷移フラグ
            // ※引数がnull以外の場合、試薬設定画面からの通知のため、画面遷移不要
            Boolean isChangeReagentSet = false;

            if (value != null)
            {
                // 指定のモジュールのみチェックするため、モジュール番号と最大カウントを調整
                Int32.TryParse(value.ToString(), out moduleIndex);
            }
            else
            {
                moduleIndex = CarisXConst.ALL_MODULEID;

                // 試薬設定画面へ遷移する
                isChangeReagentSet = true;
            }

            List<String> duplicationAnalytes = CarisXSubFunction.GetDuplicationAnalytes(moduleIndex, reagentDatas);

            // 重複している場合、ガイダンスを出力
            if (duplicationAnalytes.Count > 0)
            {
                // ラック移動方式の設定を文字列化
                String guidanceMessage = String.Empty;

                switch (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackMovementMethodParameter.RackMovementMethod)
                {
                    case RackMovementMethodKind.Performance:
                        guidanceMessage = CarisX.Properties.Resources.STRING_DLG_MSG_258;
                        break;
                    case RackMovementMethodKind.Cost:
                        guidanceMessage = CarisX.Properties.Resources.STRING_DLG_MSG_259;
                        break;
                }

                // 重複している項目名を列挙する
                String analytesNames = String.Empty;
                for (Int32 addIndex = 0; addIndex < duplicationAnalytes.Count; addIndex++)
                {
                    if (addIndex != 0)
                    {
                        analytesNames += ",";
                    }
                    analytesNames += duplicationAnalytes[addIndex];
                }

                // 試薬設置ガイダンスダイアログを表示
                DialogResult dlgResult = DlgMessage.Show(guidanceMessage, analytesNames, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel);

                // 試薬見直し＋画面遷移有りの場合
                if ((dlgResult == DialogResult.OK) && (isChangeReagentSet == true))
                {
                    // 試薬画面に遷移
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SmallMenuReagentPreparation, null);
                }
            }
        }

        /// <summary>
        /// スプラッシュ画面
        /// </summary>
        private DlgSplash splash = new DlgSplash();

        /// <summary>
        /// 汎用タイマ動作種別
        /// </summary>
        enum CommonTimerAction
        {
            /// <summary>
            /// 温度問合せ
            /// </summary>
            AskTemperature
        }
        /// <summary>
        /// 汎用タイマハンドラリスト
        /// </summary>
        private Dictionary<CommonTimerAction, Tuple<CycleCounter, Action>> commonTimerHandlerList = new Dictionary<CommonTimerAction, Tuple<CycleCounter, Action>>(); // 動作周期(ms)とハンドラのセット

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormMainFrame()
        {
            this.InitializeOnUserChange = false;

            //起動した旨のログを出力する
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
                , String.Format("CarisX has started. The version is {0}", CarisXConst.USER_PROGRAM_VERSION));

            SubFunction.ApplicationInitialize();

            this.splash = new DlgSplash();
            this.splash.StartPosition = FormStartPosition.CenterScreen;
            this.splash.Show();

            InitializeComponent();

            this.DateTimeFormatInitialize();

            // 全設定情報取得
            this.readAllStaticParameter();
            SubFunction.StartTablet(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.KeyBoardParameter.Enable);

            // 全フォーム基底クラス設定
            FormChildBase.BaseForm = this;
            FormChildBase.SubFormLocation = this.PointToScreen(this.pnlMdiChild.Location);

            this.lblTitleMdiChildName.Visible = false;

            // DB接続・アクセスクラス初期化
            this.initializeDBInstance();

            // DBのカスタマイズ
            this.customizeDB();

            // シーケンス番号初期化
            this.sequencialNumberList.ForEach((num) => num.Initialize());

            // シーケンス番号終了位置の設定
            CarisXSubFunction.SequenceEndCountChange();

            // ユニーク番号リセット
            this.resetUniqueNo();

            // TODO:初期化が完全に終了する位置への移動を考慮しておく
            // 通信・通知管理タイマ生成
            this.notifyWatchTimer.Start();

            // 送受信処理が100msで動く
            this.commWatchTimer.Interval = 100;
            this.commWatchTimer.Start();

            // 汎用タイマ生成
            var tempTimer = new Tuple<CycleCounter, Action>(new CycleCounter(CarisXConst.TEMP_WATCH_TIMER_SECOND), updeteTempData);
            tempTimer.Item1.Enable = false; // 初期値False
            this.commonTimerHandlerList.Add(CommonTimerAction.AskTemperature, tempTimer); // 温度更新
            this.commonTimer.Start();

            // 初期ステータス設定（NoLink状態を画面に反映する）
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SystemStatusChanged, CarisXConst.ALL_MODULEID);

            // 全フォーム生成(仮処理)
            this.childFormList.ForEach((f) => f.Tag = null);

            // コンストラクタでForm共通の編集中フラグがONになるため
            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;

            // イベントハンドラ関連付け
            this.bindEventHandler();

            // 値が変更された場合、通知を行う
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.UseOfHost, Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Enable);

            // 2020-02-27 CarisX IoT Add [START]
#if !NOT_USE_IOT
            // IoT使用有無切替通知
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.UseOfIoT, Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Enable);
#endif
            // 2020-02-27 CarisX IoT Add [END]

            this.Enabled = false; // 表示完了まで操作不能とする。
            this.formShownTimer.Start();

            // 途中状態のデータを削除する
            this.clearHalfwayAssayData();

            //项目版本号设定
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ProtocolVersion, Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ProtocolVersionParameter.ProtocolVersion);


            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.CalibrationModeKindChanged, Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CalibrationModeParameter.CalibrationMode);

            //启动自动保存在线日志线程
            Singleton<CarisXOnlineLogWriter>.Instance.Start();

            //左メニューのラベル透過処理
            this.SetMenuLargeLabelTransparent();

            //Slaveのボタン表示・非表示
            this.btnSlaveList.Add(ModuleIndex.Module1, new SlaveBtnManager(this.btnSlave1));
            this.btnSlaveList.Add(ModuleIndex.Module2, new SlaveBtnManager(this.btnSlave2));
            this.btnSlaveList.Add(ModuleIndex.Module3, new SlaveBtnManager(this.btnSlave3));
            this.btnSlaveList.Add(ModuleIndex.Module4, new SlaveBtnManager(this.btnSlave4));

            this.cmbAssayModuleNo.Items.Clear();
            Dictionary<String, Int32> assayModuleNoList = new Dictionary<String, Int32>();

            //TODO:接続するスレーブのマックス件数に合わせてタブを非表示にする
            switch (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected)
            {
                case 1:
                    //スレーブが１台しか接続されない場合は何も表示しない
                    this.btnSlaveList[ModuleIndex.Module1].Visible = false;
                    this.btnSlaveList[ModuleIndex.Module2].Visible = false;
                    this.btnSlaveList[ModuleIndex.Module3].Visible = false;
                    this.btnSlaveList[ModuleIndex.Module4].Visible = false;
                    btnSlaveTotal.Visible = false;
                    assayModuleNoList.Add("1", (Int32)RackPositionKind.Module1);
                    break;
                case 2:
                    //スレーブの台数分ボタンを表示
                    this.btnSlaveList[ModuleIndex.Module1].Visible = true;
                    this.btnSlaveList[ModuleIndex.Module2].Visible = true;
                    this.btnSlaveList[ModuleIndex.Module3].Visible = false;
                    this.btnSlaveList[ModuleIndex.Module4].Visible = false;
                    btnSlaveTotal.Visible = true;
                    //右寄せで位置を調整
                    this.btnSlaveList[ModuleIndex.Module1].BtnSlave.Location = new Point(1093, 0);
                    this.btnSlaveList[ModuleIndex.Module2].BtnSlave.Location = new Point(1293, 0);
                    //一番右のボタンの背景画像を変更
                    this.btnSlaveList[ModuleIndex.Module2].BtnSlave.Appearance.ImageBackground = Properties.Resources.Image_MenuBarRightOFF;
                    this.btnSlaveList[ModuleIndex.Module2].BtnSlave.ToggleAppearance.ImageBackground = Properties.Resources.Image_MenuBarRightON;
                    assayModuleNoList.Add("ALL", CarisXConst.ALL_MODULEID);
                    assayModuleNoList.Add("1", (Int32)RackPositionKind.Module1);
                    assayModuleNoList.Add("2", (Int32)RackPositionKind.Module2);
                    break;
                case 3:
                    //スレーブの台数分ボタンを表示
                    this.btnSlaveList[ModuleIndex.Module1].Visible = true;
                    this.btnSlaveList[ModuleIndex.Module2].Visible = true;
                    this.btnSlaveList[ModuleIndex.Module3].Visible = true;
                    this.btnSlaveList[ModuleIndex.Module4].Visible = false;
                    btnSlaveTotal.Visible = true;
                    //右寄せで位置を調整
                    this.btnSlaveList[ModuleIndex.Module1].BtnSlave.Location = new Point(893, 0);
                    this.btnSlaveList[ModuleIndex.Module2].BtnSlave.Location = new Point(1093, 0);
                    this.btnSlaveList[ModuleIndex.Module3].BtnSlave.Location = new Point(1293, 0);
                    //一番右のボタンの背景画像を変更
                    this.btnSlaveList[ModuleIndex.Module3].BtnSlave.Appearance.ImageBackground = Properties.Resources.Image_MenuBarRightOFF;
                    this.btnSlaveList[ModuleIndex.Module3].BtnSlave.ToggleAppearance.ImageBackground = Properties.Resources.Image_MenuBarRightON;
                    assayModuleNoList.Add("ALL", CarisXConst.ALL_MODULEID);
                    assayModuleNoList.Add("1", (Int32)RackPositionKind.Module1);
                    assayModuleNoList.Add("2", (Int32)RackPositionKind.Module2);
                    assayModuleNoList.Add("3", (Int32)RackPositionKind.Module3);
                    break;
                case 4:
                    //スレーブの台数分ボタンを表示
                    this.btnSlaveList[ModuleIndex.Module1].Visible = true;
                    this.btnSlaveList[ModuleIndex.Module2].Visible = true;
                    this.btnSlaveList[ModuleIndex.Module3].Visible = true;
                    this.btnSlaveList[ModuleIndex.Module4].Visible = true;
                    btnSlaveTotal.Visible = true;
                    //右寄せで位置を調整
                    this.btnSlaveList[ModuleIndex.Module1].BtnSlave.Location = new Point(693, 0);
                    this.btnSlaveList[ModuleIndex.Module2].BtnSlave.Location = new Point(893, 0);
                    this.btnSlaveList[ModuleIndex.Module3].BtnSlave.Location = new Point(1093, 0);
                    this.btnSlaveList[ModuleIndex.Module4].BtnSlave.Location = new Point(1293, 0);
                    //一番右のボタンの背景画像を変更
                    this.btnSlaveList[ModuleIndex.Module4].BtnSlave.Appearance.ImageBackground = Properties.Resources.Image_MenuBarRightOFF;
                    this.btnSlaveList[ModuleIndex.Module4].BtnSlave.ToggleAppearance.ImageBackground = Properties.Resources.Image_MenuBarRightON;
                    assayModuleNoList.Add("ALL", CarisXConst.ALL_MODULEID);
                    assayModuleNoList.Add("1", (Int32)RackPositionKind.Module1);
                    assayModuleNoList.Add("2", (Int32)RackPositionKind.Module2);
                    assayModuleNoList.Add("3", (Int32)RackPositionKind.Module3);
                    assayModuleNoList.Add("4", (Int32)RackPositionKind.Module4);
                    break;
                default:
                    this.btnSlaveList[ModuleIndex.Module1].Visible = false;
                    this.btnSlaveList[ModuleIndex.Module2].Visible = false;
                    this.btnSlaveList[ModuleIndex.Module3].Visible = false;
                    this.btnSlaveList[ModuleIndex.Module4].Visible = false;
                    btnSlaveTotal.Visible = false;
                    assayModuleNoList.Add("1", (Int32)RackPositionKind.Module1);
                    break;
            }

            this.cmbAssayModuleNo.DataSource = new BindingSource(assayModuleNoList, null);
            this.cmbAssayModuleNo.DisplayMember = "key";
            this.cmbAssayModuleNo.ValueMember = "value";
            this.cmbAssayModuleNo.SelectedIndex = 0;


            // 試薬交換待ちラベルリストを生成
            this.lblReagentSetupWaitMinutesList.Add(ModuleIndex.Module1, new ReagentMenuBtnManager(this.lblReagentSetupWaitMinutesForModule1, false));
            this.lblReagentSetupWaitMinutesList.Add(ModuleIndex.Module2, new ReagentMenuBtnManager(this.lblReagentSetupWaitMinutesForModule2, false));
            this.lblReagentSetupWaitMinutesList.Add(ModuleIndex.Module3, new ReagentMenuBtnManager(this.lblReagentSetupWaitMinutesForModule3, false));
            this.lblReagentSetupWaitMinutesList.Add(ModuleIndex.Module4, new ReagentMenuBtnManager(this.lblReagentSetupWaitMinutesForModule4, false));
        }


        #endregion

        #region [プロパティ]
        /// <summary>
        /// ウィンドウパラメータ（.Netフレームワーク内部からの呼び出しのみ）
        /// </summary>
        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                const Int32 WS_EX_TOOLWINDOW = 0x00000080;

                // ExStyle に WS_EX_TOOLWINDOW ビットを立てる事で、
                // ALT+TABのメニューで非表示となる。
                // 基底クラスでは非表示設定をデフォルトとしているが、FormMainFrameでは表示を行うようにする。
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle & ~WS_EX_TOOLWINDOW;

                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED

                return cp;
            }
        }
        #endregion

        #region [publicメソッド]
        //public void SystemAnalysisOpen()
        //{
        //    //systemanalyte.Show();;
        //    pnlMenuSmallSystem.Visible = false;
        //}

        #endregion

        #region [protectedメソッド]
        /// <summary>
        /// 画面終了
        /// </summary>
        /// <remarks>
        /// 画面終了します
        /// </remarks>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            //↓↓↓アプリケーション終了時の例外発生対応 2019/2/19↓↓↓
            // この時点で未送信データがある場合はクリアする
            Singleton<CarisXCommManager>.Instance.ClearAllSendData();
            //↑↑↑アプリケーション終了時の例外発生対応 2019/2/19↑↑↑

            // スレーブ切断
            Singleton<CarisXCommManager>.Instance.DisConnect();
            // ホスト切断
            Singleton<CarisXCommManager>.Instance.DisConnectHost();

            // 2020-02-27 CarisX IoT Add [START]
#if !NOT_USE_IOT
            // IoT切断
            Singleton<CarisXCommIoTManager>.Instance.DisConnectIoT();
#endif
            // 2020-02-27 CarisX IoT Add [END]

            // DB接続クローズ
            this.disposeDBInstance();

            // UI設定保存
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Save();

            Singleton<CarisXLogManager>.Instance.Dispose();

            //Figu:release
            Singleton<CarisXOnlineLogWriter>.Instance.Dispose();
        }

        /// <summary>
        /// 分析ステータス画面表示
        /// </summary>
        /// <remarks>
        /// 分析ステータス画面表示します
        /// </remarks>
        protected void showAssay()
        {
            Singleton<HistoryManager>.Instance.AddNew(CarisXHistoryActionKind.GetKind(CarisXHistoryKind.ShowAssay), () =>
            {
                this.showChildForm<FormAssay>();
                this.largeMenuButtonStateChange(this.btnMenuLargeAssay);
            });
            Singleton<HistoryManager>.Instance.ExecRecent();

        }
        /// <summary>
        /// 一般検体登録画面表示
        /// </summary>
        /// <remarks>
        /// 一般検体登録画面表示します
        /// </remarks>
        protected void showSpecimenRegistration()
        {
            Singleton<HistoryManager>.Instance.AddNew(CarisXHistoryActionKind.GetKind(CarisXHistoryKind.ShowSpecimenRegistration), () =>
         {
             this.showChildForm<FormSpecimenRegistration>();
             this.largeMenuButtonStateChange(this.btnMenuLargeSpecimen);
         });
            Singleton<HistoryManager>.Instance.ExecRecent();
        }
        /// <summary>
        /// STAT検体登録画面表示
        /// </summary>
        /// <remarks>
        /// STAT検体登録画面表示します
        /// </remarks>
        protected void showSpecimenStatRegistration()
        {
            Singleton<HistoryManager>.Instance.AddNew(CarisXHistoryActionKind.GetKind(CarisXHistoryKind.ShowSpecimenStatRegistration), () =>
         {
             this.showChildForm<FormSpecimenStatRegistration>();
             this.largeMenuButtonStateChange(this.btnMenuLargeSpecimen);
         });
            Singleton<HistoryManager>.Instance.ExecRecent();
        }
        /// <summary>
        /// 検体測定データ画面表示
        /// </summary>
        /// <remarks>
        /// 検体測定データ画面表示します
        /// </remarks>
        protected void showSpecimenResult()
        {
            Singleton<HistoryManager>.Instance.AddNew(CarisXHistoryActionKind.GetKind(CarisXHistoryKind.ShowSpecimenResult), () =>
            {
                this.showChildForm<FormSpecimenResult>();
                this.largeMenuButtonStateChange(this.btnMenuLargeSpecimen);
            });
            Singleton<HistoryManager>.Instance.ExecRecent();
        }
        /// <summary>
        /// 検体再検査画面表示
        /// </summary>
        /// <remarks>
        /// 検体再検査画面表示します
        /// </remarks>
        protected void showSpecimenRetest()
        {
            Singleton<HistoryManager>.Instance.AddNew(CarisXHistoryActionKind.GetKind(CarisXHistoryKind.ShowSpecimenRetest), () =>
            {
                this.showChildForm<FormSpecimenRetest>();
                this.largeMenuButtonStateChange(this.btnMenuLargeSpecimen);
            });
            Singleton<HistoryManager>.Instance.ExecRecent();
        }
        /// <summary>
        /// 試薬準備画面表示
        /// </summary>
        /// <remarks>
        /// 試薬準備画面表示します
        /// </remarks>
        protected void showSetReagent()
        {
            Singleton<HistoryManager>.Instance.AddNew(CarisXHistoryActionKind.GetKind(CarisXHistoryKind.ShowSetReagent), () =>
            {
                this.showChildForm<FormSetReagent>();
                this.largeMenuButtonStateChange(this.btnMenuLargeReagent);
            });
            Singleton<HistoryManager>.Instance.ExecRecent();
        }
        /// <summary>
        /// キャリブレータ解析画面表示
        /// </summary>
        /// <remarks>
        /// キャリブレータ解析画面表示します
        /// </remarks>
        protected void showCalibAnalysis()
        {
            Singleton<HistoryManager>.Instance.AddNew(CarisXHistoryActionKind.GetKind(CarisXHistoryKind.ShowCalibAnalysis), () =>
            {
                this.showChildForm<FormCalibAnalysis>();
                this.largeMenuButtonStateChange(this.btnMenuLargeCalibrator);
            });
            Singleton<HistoryManager>.Instance.ExecRecent();
        }
        /// <summary>
        /// キャリブレータステータス画面表示
        /// </summary>
        /// <remarks>
        /// キャリブレータステータス画面表示します
        /// </remarks>
        protected void showCalibStatus()
        {
            Singleton<HistoryManager>.Instance.AddNew(CarisXHistoryActionKind.GetKind(CarisXHistoryKind.ShowCalibStatus), () =>
            {
                this.showChildForm<FormCalibStatus>();
                this.largeMenuButtonStateChange(this.btnMenuLargeCalibrator);
            });
            Singleton<HistoryManager>.Instance.ExecRecent();
        }
        /// <summary>
        /// キャリブレータ登録画面表示
        /// </summary>
        /// <remarks>
        /// キャリブレータ登録画面表示します
        /// </remarks>
        protected void showCalibRegistration()
        {
            Singleton<HistoryManager>.Instance.AddNew(CarisXHistoryActionKind.GetKind(CarisXHistoryKind.ShowCalibRegistration), () =>
            {
                this.showChildForm<FormCalibRegistration>();
                this.largeMenuButtonStateChange(this.btnMenuLargeCalibrator);
            });
            Singleton<HistoryManager>.Instance.ExecRecent();
        }
        /// <summary>
        /// キャリブレータ測定データ画面表示
        /// </summary>
        /// <remarks>
        /// キャリブレータ測定データ画面表示します
        /// </remarks>
        protected void showCalibResult()
        {
            Singleton<HistoryManager>.Instance.AddNew(CarisXHistoryActionKind.GetKind(CarisXHistoryKind.ShowCalibResult), () =>
            {
                this.showChildForm<FormCalibResult>();
                this.largeMenuButtonStateChange(this.btnMenuLargeCalibrator);
            });
            Singleton<HistoryManager>.Instance.ExecRecent();
        }
        /// <summary>
        /// 精度管理検体登録画面表示
        /// </summary>
        /// <remarks>
        /// 精度管理検体登録画面表示します
        /// </remarks>
        protected void showControlRegistration()
        {
            Singleton<HistoryManager>.Instance.AddNew(CarisXHistoryActionKind.GetKind(CarisXHistoryKind.ShowControlRegistration), () =>
            {
                this.showChildForm<FormControlRegistration>();
                this.largeMenuButtonStateChange(this.btnMenuLargeControl);
            });
            Singleton<HistoryManager>.Instance.ExecRecent();
        }
        /// <summary>
        /// 精度管理検体測定履歴データ画面表示
        /// </summary>
        /// <remarks>
        /// 精度管理検体測定履歴データ画面表示します
        /// </remarks>
        protected void showControlResult()
        {
            Singleton<HistoryManager>.Instance.AddNew(CarisXHistoryActionKind.GetKind(CarisXHistoryKind.ShowControlResult), () =>
            {
                this.showChildForm<FormControlResult>();
                this.largeMenuButtonStateChange(this.btnMenuLargeControl);
            });
            Singleton<HistoryManager>.Instance.ExecRecent();
        }

        /// <summary>
        /// 精度管理検体精度管理画面表示
        /// </summary>
        /// <remarks>
        /// 精度管理検体精度管理画面表示します
        /// </remarks>
        protected void showQualityControl()
        {
            Singleton<HistoryManager>.Instance.AddNew(CarisXHistoryActionKind.GetKind(CarisXHistoryKind.ShowControlQC), () =>
            {
                this.showChildForm<FormControlQC>();
                this.largeMenuButtonStateChange(this.btnMenuLargeControl);
            });
            Singleton<HistoryManager>.Instance.ExecRecent();
        }


        /// <summary>
        /// システム構成画面表示
        /// </summary>
        /// <remarks>
        /// システム構成画面表示します
        /// </remarks>
        protected void showSystem()
        {
            Singleton<HistoryManager>.Instance.AddNew(CarisXHistoryActionKind.GetKind(CarisXHistoryKind.ShowSystemConfigration), () =>
            {
                this.showChildForm<FormSystemConfiguration>();
                this.largeMenuButtonStateChange(this.btnMenuLargeSystem);
            });
            Singleton<HistoryManager>.Instance.ExecRecent();
        }
        /// <summary>
        /// システムオプション画面表示
        /// </summary>
        /// <remarks>
        /// システムオプション画面表示します
        /// </remarks>
        protected void showSystemOption()
        {
            Singleton<HistoryManager>.Instance.AddNew(CarisXHistoryActionKind.GetKind(CarisXHistoryKind.ShowSystemOption), () =>
            {
                this.showChildForm<FormSystemOption>();
                this.largeMenuButtonStateChange(this.btnMenuLargeSystem);
            });
            FormSystemOption formSystemOption = Singleton<FormSystemOption>.Instance;
            formSystemOption.ShutdownEvent -= new ShutdownEventHandler(this.WindowsShutdown);

            formSystemOption.ShutdownEvent += new ShutdownEventHandler(this.WindowsShutdown);
            Singleton<HistoryManager>.Instance.ExecRecent();
        }
        /// <summary>
        /// システム履歴画面表示
        /// </summary>
        /// <remarks>
        /// システム履歴画面表示します
        /// </remarks>
        protected void showLog()
        {
            Singleton<HistoryManager>.Instance.AddNew(CarisXHistoryActionKind.GetKind(CarisXHistoryKind.ShowSystemLog), () =>
            {
                if (Singleton<SystemStatus>.Instance.Status == SystemStatusKind.ReagentExchange)
                {
                    return;
                }
                this.showChildForm<FormSystemLog>();
                this.largeMenuButtonStateChange(this.btnMenuLargeSystem);
            });
            Singleton<HistoryManager>.Instance.ExecRecent();
        }
        /// <summary>
        /// システム分析項目画面表示
        /// </summary>
        /// <remarks>
        /// システム分析項目画面表示します
        /// </remarks>
        protected void showAnalytes()
        {
            Singleton<HistoryManager>.Instance.AddNew(CarisXHistoryActionKind.GetKind(CarisXHistoryKind.ShowSystemAnalytes), () =>
            {
                this.showChildForm<FormSystemAnalytes>();
                this.largeMenuButtonStateChange(this.btnMenuLargeSystem);
            });
            Singleton<HistoryManager>.Instance.ExecRecent();
        }
        /// <summary>
        /// ユーザー管理画面表示
        /// </summary>
        /// <remarks>
        /// ユーザー管理画面表示します
        /// </remarks>
        protected void showUser()
        {
            Singleton<HistoryManager>.Instance.AddNew(CarisXHistoryActionKind.GetKind(CarisXHistoryKind.ShowSystemUserControl), () =>
            {
                this.showChildForm<FormSystemUserControl>();
                this.largeMenuButtonStateChange(this.btnMenuLargeSystem);
            });
            Singleton<HistoryManager>.Instance.ExecRecent();
        }
        /// <summary>
        /// システムオプション画面(モジュール毎)表示
        /// </summary>
        /// <remarks>
        /// システムオプション画面(モジュール毎)表示します
        /// </remarks>
        protected void showSystemModuleOption()
        {
            Singleton<HistoryManager>.Instance.AddNew(CarisXHistoryActionKind.GetKind(CarisXHistoryKind.ShowSystemModuleOption), () =>
         {
             this.showChildForm<FormSystemModuleOption>();
             this.largeMenuButtonStateChange(this.btnMenuLargeOption);
         });
            Singleton<HistoryManager>.Instance.ExecRecent();
        }
        /// <summary>
        /// 大項目メニューボタン状態変更
        /// </summary>
        /// <remarks>
        /// 大項目メニューボタン状態を変更します
        /// </remarks>
        /// <param name="enableButton"></param>
        protected void largeMenuButtonStateChange(CustomUStateButton enableButton)
        {


            // 連動リスト
            CustomUStateButton[] linkGroup = {    this.btnMenuLargeAssay
                                                 , this.btnMenuLargeCalibrator
                                                 , this.btnMenuLargeControl
                                                 , this.btnMenuLargeReagent
                                                 , this.btnMenuLargeSpecimen
                                                 , this.btnMenuLargeOption
                                                 , this.btnMenuLargeSystem };

            // 指定ボタンを選択状態とし、他を非選択状態にする。
            foreach (CustomUStateButton btn in linkGroup)
            {
                btn.CurrentState = (btn == enableButton);
            }

        }

        /// <summary>
        /// コンポーネントの初期化
        /// </summary>
        /// <remarks>
        /// コンポーネントを初期化します
        /// </remarks>
        protected override void initializeFormComponent()
        {

            Singleton<HistoryManager>.Instance.HistChanged += this.histryChanged;
            this.refleshNavigationButton();

            // TODO:2012/04/26 Debug用の為、最終的には削除
            //this.button1.Click += (sender, e) => new DebugForm_SpecifyCountValue().Show();


            //设置企业LOGO
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo == CompanyLogoParameter.CompanyLogoKind.LogoDefault)
            {
                this.BackgroundImage = global::Oelco.CarisX.Properties.Resources.Image_MainFrameBackgound;
            }
            else if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo == CompanyLogoParameter.CompanyLogoKind.LogoOne)
            {
                this.BackgroundImage = global::Oelco.CarisX.Properties.Resources.Image_MainFrameBackgound_01;
            }
            else if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
            {
                this.BackgroundImage = global::Oelco.CarisX.Properties.Resources.Image_MainFrameBackgound_02;
            }
        }
        /// <summary>
        /// リソースの初期化
        /// </summary>
        /// <remarks>
        /// リソースを初期化します
        /// </remarks>
        protected override void initializeResource()
        {
            // TODO:
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            this.lblMenuLargeAssay.Text = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_000;
            this.lblMenuLargeReagent.Text = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_001;
            this.lblMenuLargeSpecimen.Text = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_002;
            this.lblMenuLargeCalibrator.Text = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_003;
            this.lblMenuLargeControl.Text = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_004;
            this.lblMenuLargeOption.Text = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_029;
            this.lblMenuLargeSystem.Text = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_005;
            this.btnError.Text = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_006;
            this.lblAnalyzerStatus.Text = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_007;
            //this.lblHostOnlineStatus.Text = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_008;
            this.lblAssayStart.Text = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_009;
            this.lblAssayPause.Text = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_010;
            this.lblAssayAbort.Text = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_011;
            this.btnHelp.Text = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_012;
            this.btnExit.Text = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_013;
            this.btnSlaveTotal.Text = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_038;
            this.btnSlave1.Text = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_039;
            this.btnSlave2.Text = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_040;
            this.btnSlave3.Text = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_041;
            this.btnSlave4.Text = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_042;
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// ラベルの透過処理設定
        /// </summary>
        /// <remarks>
        /// backcolorの設定だけではボタンを通り抜けてフォームの色を取得してしまうので、
        /// 親コントロールをボタンに設定する
        /// </remarks>
        private void SetMenuLargeLabelTransparent()
        {
            //Specimen
            this.lblMenuLargeSpecimen.Parent = this.btnMenuLargeSpecimen;
            this.lblMenuLargeSpecimen.Location -= (Size)this.btnMenuLargeSpecimen.Location;

            //Assay
            this.lblMenuLargeAssay.Parent = this.btnMenuLargeAssay;
            this.lblMenuLargeAssay.Location -= (Size)this.btnMenuLargeAssay.Location;

            //Reagent
            this.lblMenuLargeReagent.Parent = this.btnMenuLargeReagent;
            this.lblMenuLargeReagent.Location -= (Size)this.btnMenuLargeReagent.Location;

            //Calibrator
            this.lblMenuLargeCalibrator.Parent = this.btnMenuLargeCalibrator;
            this.lblMenuLargeCalibrator.Location -= (Size)this.btnMenuLargeCalibrator.Location;

            //Control
            this.lblMenuLargeControl.Parent = this.btnMenuLargeControl;
            this.lblMenuLargeControl.Location -= (Size)this.btnMenuLargeControl.Location;

            //Option
            this.lblMenuLargeOption.Parent = this.btnMenuLargeOption;
            this.lblMenuLargeOption.Location -= (Size)this.btnMenuLargeOption.Location;

            //System
            this.lblMenuLargeSystem.Parent = this.btnMenuLargeSystem;
            this.lblMenuLargeSystem.Location -= (Size)this.btnMenuLargeSystem.Location;

        }

        /// <summary>
        /// 日付フォーマット初期化
        /// </summary>
        /// <remarks>
        /// 日付フォーマット初期化します
        /// </remarks>
        private void DateTimeFormatInitialize()
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();

            // DateTime型ToString()メソッドにおける既定フォーマットで使用されるフォーマットを設定
            // "G"パターン(短い形式の日付 (d) パターンと長い形式の時刻 (T) パターンを空白で区切って組み合わせ)の短い日付の"d"パターンを設定)
            String shortDatePattern = String.Empty;

            switch (culture.Name)
            {
                case "zh-TW":
                case "zh-CN":
                    shortDatePattern = @"yyyy/MM/dd";
                    break;
                case "en-US":
                default:
                    shortDatePattern = @"MM/dd/yyyy";
                    culture = (CultureInfo)CultureInfo.GetCultureInfo("en-US").Clone();
                    break;
            }

            if (!String.IsNullOrEmpty(shortDatePattern))
            {
                culture.DateTimeFormat.ShortDatePattern = shortDatePattern;

            }
            culture.DateTimeFormat.LongTimePattern = @"HH:mm:ss";

            culture.DateTimeFormat.YearMonthPattern = culture.DateTimeFormat.ShortDatePattern.Replace("/dd", "").Replace("dd/", "");

            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// 履歴変更
        /// </summary>
        /// <remarks>
        /// ナビゲーションボタンリフレッシュします
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void histryChanged(Object sender, EventArgs args)
        {
            this.refleshNavigationButton();
        }

        /// <summary>
        /// ナビゲーションボタンリフレッシュ
        /// </summary>
        /// <remarks>
        /// ナビゲーションボタンリフレッシュします
        /// </remarks>
        void refleshNavigationButton()
        {
            // ボタンの押下可不可を設定
            this.btnNaviBack.Enabled = Singleton<HistoryManager>.Instance.ExistPrev();      // ←
            this.btnNaviBack.Appearance.ImageBackground = this.btnNaviBack.Enabled ? Oelco.CarisX.Properties.Resources.Image_NavButton_Return : Oelco.CarisX.Properties.Resources.Image_NavButton_ReturnDisable;
            this.btnNaviForward.Enabled = Singleton<HistoryManager>.Instance.ExistNext();   // →
            this.btnNaviForward.Appearance.ImageBackground = this.btnNaviForward.Enabled ? Oelco.CarisX.Properties.Resources.Image_NavButton_Next : Oelco.CarisX.Properties.Resources.Image_NavButton_NextDisable;
        }

        #region _大項目メニューボタン_
        /// <summary>
        /// 小項目メニュー直近選択実施
        /// </summary>
        /// <remarks>
        /// 小項目メニュー直近選択実施します
        /// </remarks>
        /// <param name="kind"></param>
        private void executeRecentSelectSmallManu(SelectedLargeMenu kind)
        {
            // 直近選択実施
            if (this.dicSelectedRecent.ContainsKey(kind))
            {
                this.dicSelectedRecent[kind]();
            }
        }

        /// <summary>
        /// スモールメニューフォームの表示
        /// </summary>
        private void ShowSmallMenu(SmallMenuKind kind)
        {
            FormMainFrameSmallMenu SmallMenu = new FormMainFrameSmallMenu();
            SmallMenu.SmallMenuType = kind;
            SmallMenu.Location = this.Location + (Size)SmallMenu.AdjustLocation;
            this.AddOwnedForm(SmallMenu); //メイン画面の上にスモールメニューが来るように、スモールメニュー画面をメイン画面に所属させる
            SmallMenu.Show(rectangle);
        }


        /// <summary>
        /// 検体(大項目メニュー)クリックイベント
        /// </summary>
        /// <remarks>
        /// 検体(大項目メニュー)クリックイベント実行します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnMenuLargeSpecimen_Click(object sender, EventArgs e)
        {
            // 編集中かどうか、また編集中でも画面遷移するかどうか
            if (CarisXSubFunction.IsEditsMessageShow())
            {
                this.largeMenuButtonStateChange(this.btnMenuLargeSpecimen);
                this.executeRecentSelectSmallManu(SelectedLargeMenu.Specimen);

                //スモールメニューフォームを表示する
                ShowSmallMenu(SmallMenuKind.Specimen);

                // 直近選択実施
                this.nowSelectedMenu = SelectedLargeMenu.Specimen;

                // 編集中フラグOFF
                FormChildBase.IsEdit = false;
            }
            else
            {
                // 画面を遷移しない時、ボタンが選択状態のままになってしまう場合があるためボタン状態を非選択にする
                if (this.btnMenuLargeSpecimen.CurrentState)
                {
                    // Specimen画面からボタン押下した場合は、選択状態のままにする
                    if ((this.btnMenuLargeAssay.CurrentState == false)
                        && (this.btnMenuLargeReagent.CurrentState == false)
                        && (this.btnMenuLargeCalibrator.CurrentState == false)
                        && (this.btnMenuLargeControl.CurrentState == false)
                        && (this.btnMenuLargeOption.CurrentState == false)
                        && (this.btnMenuLargeSystem.CurrentState == false))
                    {

                    }
                    else
                    {
                        this.btnMenuLargeSpecimen.CurrentState = false;
                    }
                }
            }
        }
        /// <summary>
        /// 分析ボタン(大項目メニュー)クリックイベント
        /// </summary>
        /// <remarks>
        /// 分析ボタン(大項目メニュー)クリックイベントを実行します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnMenuLargeAssay_Click(object sender, EventArgs e)
        {
            // 編集中かどうか、また編集中でも画面遷移するかどうか
            if (CarisXSubFunction.IsEditsMessageShow())
            {
                this.dicSelectedRecent[this.nowSelectedMenu] = this.showAssay; // 直近選択記録
                this.showAssay();

                // 編集中フラグOFF
                FormChildBase.IsEdit = false;
            }
            else
            {
                // 画面を遷移しない時、ボタンが選択状態のままになってしまう場合があるためボタン状態を非選択にする
                if(this.btnMenuLargeAssay.CurrentState)
                {
                    // Assay画面からボタン押下した場合は、選択状態のままにする
                    if ((this.btnMenuLargeSpecimen.CurrentState == false)
                        && (this.btnMenuLargeReagent.CurrentState == false)
                        && (this.btnMenuLargeCalibrator.CurrentState == false)
                        && (this.btnMenuLargeControl.CurrentState == false)
                        && (this.btnMenuLargeOption.CurrentState == false)
                        && (this.btnMenuLargeSystem.CurrentState == false))
                    {

                    }
                    else
                    {
                        this.btnMenuLargeAssay.CurrentState = false;
                    }
                }
            }
        }

        /// <summary>
        /// 試薬(大項目メニュー)クリックイベント
        /// </summary>
        /// <remarks>
        /// 試薬(大項目メニュー)クリックイベント実行します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnMenuLargeReagent_Click(object sender, EventArgs e)
        {
            // 編集中かどうか、また編集中でも画面遷移するかどうか
            if (CarisXSubFunction.IsEditsMessageShow())
            {
                /// サブメニューが一つしかなく、サブメニューを表示せずに画面遷移するように変更となった
                /// 今後、サブメニューを表示する際に、サブメニュー非表示処理の漏れがないようにコメントアウトで残しておく
                /*
                Boolean flgblink = this.btnMenuLargeReagent.IsBlink;    //ブリンクしている場合（＝試薬テーブルの画面が表示されている）時はスモールメニューを表示しない

                this.executeRecentSelectSmallManu(SelectedLargeMenu.Reagent);
                this.btnMenuLargeReagent.BlinkEnd();
                this.largeMenuButtonStateChange(this.btnMenuLargeReagent);

                if (!flgblink)
                {                
                    //スモールメニューフォームを表示する
                    ShowSmallMenu(SmallMenuKind.Reagent);
                }

                // 直近選択実施
                this.nowSelectedMenu = SelectedLargeMenu.Reagent;
                */

                this.dicSelectedRecent[this.nowSelectedMenu] = this.showSetReagent; // 直近選択記録
                this.showSetReagent();

                // 編集中フラグOFF
                FormChildBase.IsEdit = false;
            }
            else
            {
                // 画面を遷移しない時、ボタンが選択状態のままになってしまう場合があるためボタン状態を非選択にする
                if (this.btnMenuLargeReagent.CurrentState)
                {
                    // Reagent画面からボタン押下した場合は、選択状態のままにする
                    if ((this.btnMenuLargeSpecimen.CurrentState == false)
                        && (this.btnMenuLargeAssay.CurrentState == false)
                        && (this.btnMenuLargeCalibrator.CurrentState == false)
                        && (this.btnMenuLargeControl.CurrentState == false)
                        && (this.btnMenuLargeOption.CurrentState == false)
                        && (this.btnMenuLargeSystem.CurrentState == false))
                    {

                    }
                    else
                    {
                        this.btnMenuLargeReagent.CurrentState = false;
                    }
                }
            }
        }

        /// <summary>
        /// キャリブレータ(大項目メニュー)クリックイベント
        /// </summary>
        /// <remarks>
        /// キャリブレータ(大項目メニュー)クリックイベント実行します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnMenuLargeCalibrator_Click(object sender, EventArgs e)
        {
            // 編集中かどうか、また編集中でも画面遷移するかどうか
            if (CarisXSubFunction.IsEditsMessageShow())
            {
                this.largeMenuButtonStateChange(this.btnMenuLargeCalibrator);
                this.executeRecentSelectSmallManu(SelectedLargeMenu.Calibrator);

                //スモールメニューフォームを表示する
                ShowSmallMenu(SmallMenuKind.Calibration);

                // 直近選択実施
                this.nowSelectedMenu = SelectedLargeMenu.Calibrator;

                // 編集中フラグOFF
                FormChildBase.IsEdit = false;
            }
            else
            {
                // 画面を遷移しない時、ボタンが選択状態のままになってしまう場合があるためボタン状態を非選択にする
                if (this.btnMenuLargeCalibrator.CurrentState)
                {
                    // Calibrator画面からボタン押下した場合は、選択状態のままにする
                    if ((this.btnMenuLargeSpecimen.CurrentState == false)
                        && (this.btnMenuLargeAssay.CurrentState == false)
                        && (this.btnMenuLargeReagent.CurrentState == false)
                        && (this.btnMenuLargeControl.CurrentState == false)
                        && (this.btnMenuLargeOption.CurrentState == false)
                        && (this.btnMenuLargeSystem.CurrentState == false))
                    {

                    }
                    else
                    {
                        this.btnMenuLargeCalibrator.CurrentState = false;
                    }
                }
            }
        }

        /// <summary>
        /// 精度管理検体(大項目メニュー)クリックイベント
        /// </summary>
        /// <remarks>
        /// 精度管理検体(大項目メニュー)クリックイベント実行します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnMenuLargeControl_Click(object sender, EventArgs e)
        {
            // 編集中かどうか、また編集中でも画面遷移するかどうか
            if (CarisXSubFunction.IsEditsMessageShow())
            {
                this.largeMenuButtonStateChange(this.btnMenuLargeControl);
                this.executeRecentSelectSmallManu(SelectedLargeMenu.Control);

                //スモールメニューフォームを表示する
                ShowSmallMenu(SmallMenuKind.Control);

                // 直近選択実施
                this.nowSelectedMenu = SelectedLargeMenu.Control;

                // 編集中フラグOFF
                FormChildBase.IsEdit = false;
            }
            else
            {
                // 画面を遷移しない時、ボタンが選択状態のままになってしまう場合があるためボタン状態を非選択にする
                if (this.btnMenuLargeControl.CurrentState)
                {
                    // Control画面からボタン押下した場合は、選択状態のままにする
                    if((this.btnMenuLargeSpecimen.CurrentState == false)
                        && (this.btnMenuLargeAssay.CurrentState == false)
                        && (this.btnMenuLargeReagent.CurrentState == false)
                        && (this.btnMenuLargeCalibrator.CurrentState == false)
                        && (this.btnMenuLargeOption.CurrentState == false)
                        && (this.btnMenuLargeSystem.CurrentState == false))
                    {
                        
                    }
                    else
                    {
                        this.btnMenuLargeControl.CurrentState = false;
                    }
                }
            }
        }

        /// <summary>
        /// オプション(大項目メニュー)クリックイベント
        /// </summary>
        /// <remarks>
        /// オプション(大項目メニュー)クリックイベント実行します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnMenuLargeOption_Click(object sender, EventArgs e)
        {
            // 編集中かどうか、また編集中でも画面遷移するかどうか
            if (CarisXSubFunction.IsEditsMessageShow())
            {
                this.largeMenuButtonStateChange(this.btnMenuLargeOption);
                this.executeRecentSelectSmallManu(SelectedLargeMenu.Option);

                // 直接、オプション画面（モジュール毎）を表示する
                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SmallMenuSystemModuleOption, null);

                // 直近選択実施
                this.nowSelectedMenu = SelectedLargeMenu.Option;

                // 編集中フラグOFF
                FormChildBase.IsEdit = false;
            }
            else
            {
                // 画面を遷移しない時、ボタンが選択状態のままになってしまう場合があるためボタン状態を非選択にする
                if (this.btnMenuLargeOption.CurrentState)
                {
                    // Option画面からボタン押下した場合は、選択状態のままにする
                    if ((this.btnMenuLargeSpecimen.CurrentState == false)
                        && (this.btnMenuLargeAssay.CurrentState == false)
                        && (this.btnMenuLargeReagent.CurrentState == false)
                        && (this.btnMenuLargeCalibrator.CurrentState == false)
                        && (this.btnMenuLargeControl.CurrentState == false)
                        && (this.btnMenuLargeSystem.CurrentState == false))
                    {

                    }
                    else
                    {
                        this.btnMenuLargeOption.CurrentState = false;
                    }
                }
            }
        }

        /// <summary>
        /// システム(大項目メニュー)クリックイベント
        /// </summary>
        /// <remarks>
        /// システム(大項目メニュー)クリックイベント実行します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnMenuLargeSystem_Click(object sender, EventArgs e)
        {
            // 編集中かどうか、また編集中でも画面遷移するかどうか
            if (CarisXSubFunction.IsEditsMessageShow())
            {
                this.largeMenuButtonStateChange(this.btnMenuLargeSystem);
                this.executeRecentSelectSmallManu(SelectedLargeMenu.System);

                //スモールメニューフォームを表示する
                ShowSmallMenu(SmallMenuKind.System);

                // 直近選択実施
                this.nowSelectedMenu = SelectedLargeMenu.System;

                // 編集中フラグOFF
                FormChildBase.IsEdit = false;
            }
            else
            {
                // 画面を遷移しない時、ボタンが選択状態のままになってしまう場合があるためボタン状態を非選択にする
                if (this.btnMenuLargeSystem.CurrentState)
                {
                    // Option画面からボタン押下した場合は、選択状態のままにする
                    if ((this.btnMenuLargeSpecimen.CurrentState == false)
                        && (this.btnMenuLargeAssay.CurrentState == false)
                        && (this.btnMenuLargeReagent.CurrentState == false)
                        && (this.btnMenuLargeCalibrator.CurrentState == false)
                        && (this.btnMenuLargeControl.CurrentState == false)
                        && (this.btnMenuLargeOption.CurrentState == false))
                    {

                    }
                    else
                    {
                        this.btnMenuLargeSystem.CurrentState = false;
                    }
                }
            }
        }
        #endregion

        #region _分析関連_

        //        private AmountTable getAmountFromDB()
        //        {
        //            // TODO:IRemainAmountInfoSetの実体を臨時で残量コマンドデータクラスで代用している。
        //            SlaveCommCommand_0508 remainAmount = new SlaveCommCommand_0508();
        //            IRemainAmountInfoSet remain = remainAmount;
        //            Singleton<ReagentDB>.Instance.GetReagentRemain(ref remain);
        //            AmountTable amount = new AmountTable();
        //            amount.PreTriggerAmountInfo.Amount = remain.PreTriggerRemainTable.RemainingAmount.Sum( ( v ) => v.Remain );
        //            amount.TriggerAmountInfo.Amount = remain.TriggerRemainTable.RemainingAmount.Sum( ( v ) => v.Remain );
        //            amount.RinceContainerAmountInfo.Amount = remain.RinceContainerRemain;
        //            amount.SampleTipAmountInfo.Amount = remain.SampleTipRemainTable.tipRemainTable.Sum();
        //            amount.CellAmountInfo.Amount = remain.CellRemainTable.reactContainerRemainTable.Sum();
        //            amount.DilutionAmountInfo.Amount = remain.DilutionRemainTable.RemainingAmount.Remain;
        //            var reags = from v in remain.ReagentRemainTable
        //                        orderby v.ReagCode
        //                        select v.RemainingAmount;
        //            AmountInfo info = new AmountInfo();

        ////            amount.ReagentAmountInfo

        ////            var amountInfo = from v in 
        //        }

        /// <summary>
        /// 検体登録されている内容の検量線存在確認
        /// </summary>
        /// <remarks>
        /// 一般・優先検体が使用する分析項目の検量線存在確認を行い、
        /// 無ければダイアログにその旨表示を行う。
        /// </remarks>
        /// <returns>True:検量線なし項目無し False:検量線無し項目あり [戻り値現在不使用]</returns>
        private Boolean checkUseProtocolCalibCurve()
        {
            Boolean result = true;

            List<Tuple<String, String, String>> curveInvalidProtoLot = new List<Tuple<String, String, String>>();

            // 検量線なし項目リスト作成
            for(int moduleId = (int)RackModuleIndex.Module1; moduleId < Singleton<SystemStatus>.Instance.ModuleStatus.Count(); moduleId++)
            {
                // 未接続状態のモジュールは対象外
                if (Singleton<SystemStatus>.Instance.ModuleStatus[moduleId] != SystemStatusKind.NoLink )
                {
                    // モーターエラーのスレーブは処理を行わない
                    if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[moduleId] == Status.SystemStatusKind.MotorError)
                    {
                        continue;
                    }

                    var allRegist = HybridDataMediator.GetAllDBRegisteredProtocolAndNowLotNo(moduleId);
                    foreach (var protocolAndLot in allRegist)
                    {
                        if (!this.checkUseCalibCurve(moduleId, protocolAndLot.Item1, protocolAndLot.Item2))
                        {
                            String moduleIndexString = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_039;
                            switch (moduleId)
                            {
                                case (int)RackModuleIndex.Module1:
                                    moduleIndexString = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_039;
                                    break;
                                case (int)RackModuleIndex.Module2:
                                    moduleIndexString = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_040;
                                    break;
                                case (int)RackModuleIndex.Module3:
                                    moduleIndexString = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_041;
                                    break;
                                case (int)RackModuleIndex.Module4:
                                    moduleIndexString = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_042;
                                    break;
                            }

                            curveInvalidProtoLot.Add(new Tuple<String, String, String>(moduleIndexString, protocolAndLot.Item1.ProtocolName, protocolAndLot.Item2));
                        }
                    }
                }
            }

            // 検量線なし項目リスト表示
            if (curveInvalidProtoLot.Count != 0)
            {
                using (var confirm = new DlgCalibCurveConfirm())
                {
                    confirm.setPartsList(curveInvalidProtoLot);
                    confirm.ShowDialog();
                }
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 検量線有効確認
        /// </summary>
        /// <remarks>
        /// 指定された分析項目・試薬ロット番号で検量線の存在を確認し、
        /// 存在しない場合に検量線作成確認を行います。
        /// この確認で検量線作成を選んだ場合、ラックは強制排出されます。
        /// </remarks>
        /// <param name="protocol">分析項目</param>
        /// <param name="reagentLotNo">試薬ロット番号</param>
        /// <returns>検量線作成選択</returns>
        private Boolean checkUseCalibCurve(int moduleId, MeasureProtocol protocol, String reagentLotNo)
        {
            // 検量線チェック
            Boolean askCalib = true;
            var curveData = Singleton<CalibrationCurveDB>.Instance.GetData(protocol.ProtocolIndex, reagentLotNo, moduleId);
            if (curveData.Count == 0)
            {
                // データなし
                askCalib = false;
            }
            else
            {
                var dat = curveData.First().Value;
                if (dat == null || dat.Count <= 0)
                {
                    // データなし
                    askCalib = false;
                }
                else
                {
                    if (dat.First().GetApprovalDateTime().AddDays(protocol.ValidityOfCurve) < DateTime.Now)
                    {
                        // データなし
                        askCalib = false;
                    }
                }
            }

            return askCalib;
        }

        /// <summary>
        /// サブレディコマンド（モジュール再接続時）
        /// </summary>
        /// <remarks>
        /// モジュールの再接続時、初期シーケンス起動を実行します
        /// </remarks>
        protected void onSubreadyCommand(SlaveCommCommand_0501 command)
        {
            Debug.WriteLine(String.Format("FormMainFrame::{0}", MethodBase.GetCurrentMethod().Name));
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                , CarisXLogInfoBaseExtention.Empty, String.Format("FormMainFrame::{0} commNo = {1} moduleIndex = {2}", 
                MethodBase.GetCurrentMethod().Name, command.CommNo, CarisXSubFunction.MachineCodeToModuleIndex((MachineCode)command.CommNo)));

            //自動立ち上げ待機画面が表示されていたら、閉じる。
            if (dlgAutoSetup != null)
            {
                dlgAutoSetup.Close();
                dlgAutoSetup.Dispose();
            }

            // モジュールIndexを取得
            Int32 moduleIndex = CarisXSubFunction.MachineCodeToModuleIndex((MachineCode)command.CommNo);

            Int32 rackModuleIndex = CarisXSubFunction.MachineCodeToRackModuleIndex((MachineCode)command.CommNo);

            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                , CarisXLogInfoBaseExtention.Empty, String.Format("||||||||||||||||||||command.CommNo = {0} rackModuleIndex = {1}", command.CommNo, rackModuleIndex));

            // 無接続状態に戻す
            Singleton<SystemStatus>.Instance.setModuleStatus((RackModuleIndex)rackModuleIndex, SystemStatusKind.NoLink);


            //初期シーケンス中にモジュールが再起動された場合に初期シーケンスが再実行されるように、条件なしに呼び出しする
            Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleIndex].InitializeSequenceModule(InitializeSequencePattern.Module | InitializeSequencePattern.StartsAfterUser);
        }

        /// <summary>
        /// ラックレディコマンド（ラック搬送再接続時）
        /// </summary>
        /// <remarks>
        /// ラック搬送の再接続時、初期シーケンス起動を実行します
        /// </remarks>
        protected void onRackreadyCommand()
        {
            Debug.WriteLine(String.Format("FormMainFrame::{0}", MethodBase.GetCurrentMethod().Name));
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                , CarisXLogInfoBaseExtention.Empty, String.Format("FormMainFrame::{0}", MethodBase.GetCurrentMethod().Name));

            //自動立ち上げ待機画面が表示されていたら、閉じる。
            if (dlgAutoSetup != null)
            {
                dlgAutoSetup.Close();
                dlgAutoSetup.Dispose();
            }

            // 無接続状態に戻す
            Singleton<SystemStatus>.Instance.setModuleStatus(RackModuleIndex.RackTransfer, SystemStatusKind.NoLink);

            //初期シーケンス中にラック搬送が再起動された場合に初期シーケンスが再実行されるように、条件なしに呼び出しする
            Singleton<CarisXSequenceHelperManager>.Instance.RackTransfer.InitializeSequenceRackTransfer(
                InitializeSequencePattern.RackTransfer | InitializeSequencePattern.StartsAfterUser);
        }

        /// <summary>
        /// コマンド受信処理
        /// </summary>
        /// <remarks>
        /// メイン画面経由で処理を行うコマンドの受信を行います。
        /// </remarks>
        /// <param name="sender">不使用</param>
        /// <param name="command">コマンドデータ</param>
        protected void onCommand(Object sender, CommCommandEventArgs command)
        {
            // 数が多くなる場合、CommManagerクラスに個別のイベントを定義し、関連付けを整理する。
            System.Diagnostics.Debug.WriteLine(String.Format("FormMainFrame::OnCommand commandId = {0}", command.Command.CommandId));
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, String.Format("FormMainFrame::OnCommand commandId = {0}", command.Command.CommandId));

            CarisXCommCommand cmd = command.Command as CarisXCommCommand;

            switch (cmd.CommKind)
            {
                ////////////////////////////////////
                // ※画面との連結が強いため、切り離し不可
                ////////////////////////////////////
                case CommandKind.RackTransferCommand0101:
                    // ラックレディーコマンド
                    this.onRackreadyCommand();
                    break;
                case CommandKind.Command0501:
                    // サブレディコマンド（スレーブ再接続時）
                    this.onSubreadyCommand(cmd as SlaveCommCommand_0501);
                    break;
                case CommandKind.Command0507:
                    // 分析終了コマンド
                    this.onAssayEnd(cmd as SlaveCommCommand_0507);
                    break;

                ////////////////////////////////////
                // ラック搬送関連コマンド
                ////////////////////////////////////
                case CommandKind.RackTransferCommand0104:   // エラー通知コマンド(ラック搬送用)
                case CommandKind.RackTransferCommand0105:   // サブイベントコマンド
                case CommandKind.RackTransferCommand0106:   // ラック分析ステータスコマンド
                case CommandKind.RackTransferCommand0108:   // 残量コマンド(ラック搬送用)
                case CommandKind.RackTransferCommand0111:   // バージョン通知コマンド(ラック搬送用)
                case CommandKind.RackTransferCommand0117:   // ラック情報通知コマンド
                case CommandKind.RackTransferCommand0119:   // ラック移動位置問合せ（装置待機位置）コマンド
                case CommandKind.RackTransferCommand0120:   // ラック移動位置問合せ（BCR）コマンド
                
                ////////////////////////////////////
                // スレーブ関連コマンド
                ////////////////////////////////////
                case CommandKind.Command0502:               // 測定指示データ問い合わせコマンド
                case CommandKind.Command0503:               // 測定データコマンド
                case CommandKind.Command0504:               // エラー通知コマンド(スレーブ用)
                case CommandKind.Command0505:               // サブイベントコマンド
                case CommandKind.Command0506:               // 分析ステータスコマンド
                case CommandKind.Command0508:               // 残量コマンド(スレーブ用)
                case CommandKind.Command0510:               // マスターカーブ情報コマンド
                case CommandKind.Command0511:               // バージョン通知コマンド(スレーブ用)
                case CommandKind.Command0512:               // 試薬ロット確認コマンド
                case CommandKind.Command0513:               // キャリブレーション測定確認コマンド
                case CommandKind.Command0514:               // 総アッセイ数通知コマンド
                case CommandKind.Command0515:               // ラック設定状況コマンド
                case CommandKind.Command0516:               // 試薬テーブル回転SW押下通知コマンド
                case CommandKind.Command0520:               // 試薬設置状況通知コマンド
                case CommandKind.Command0521:               // 廃液タンク状態問合せコマンド
                case CommandKind.Command0522:               // キャリブレータ情報通知コマンド
                case CommandKind.Command0591:               // STAT状態通知コマンド
                case CommandKind.Command0596:               // 分取完了コマンド

                ////////////////////////////////////
                // ホスト関連コマンド
                ////////////////////////////////////
                case CommandKind.HostCommand0004:           // 装置ステータス問合せコマンド
                    this.onRecieveCommandThread.Start(cmd);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// システムステータスのアイコンを設定する
        /// </summary>
        private void setSystemStatusIcon(Object value)
        {
            if (Singleton<SystemStatus>.Instance.PauseReason.All(v => v.Value == SamplingPauseReason.SAMPLINGPAUSEREASON_DEFAULT))
            {
                //サンプリング停止理由が何もない場合
                this.pnlAnalyzerStatus.Appearance.ImageBackground = global::Oelco.CarisX.Properties.Resources.image_AnalyzerStatus;
            }
            else
            {
                //サンプリング停止理由がある場合
                this.pnlAnalyzerStatus.Appearance.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Exclamation;
            }
        }

        /// <summary>
        /// システムステータス変更通知
        /// </summary>
        /// <param name="value"></param>
        private void changeAbortAssay(Object value)
        {
            this.lblAnalyzerStatus.Text = Oelco.CarisX.Properties.Resources.STRING_MAIN_FRAME_011;
        }

        /// <summary>
        /// 日替わり処理
        /// </summary>
        /// <remarks>
        /// DB内容初期化し、各画面の検索関連UIの初期表示日付を更新します
        /// </remarks>
        /// <param name="value">不使用</param>
        private void onDateChange(Object value)
        {

            // 前回の日替わり処理発生日と異なる場合処理を行う。
            // 日替わり処理確認日時は設定ファイルに保存される。
            DateTime lastCheckedTime = Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.DateChange;
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.DateChange = DateTime.Now;

            //// HACK:デバッグ用、一時的に日替わり処理に入らないようにする
            //{
            //    lastCheckedTime = DateTime.Now;
            //}

            if (lastCheckedTime.Date != DateTime.Now.Date)
            {
                // 分析開始時・起動時・システム初期化時に動作
                System.Diagnostics.Debug.WriteLine("Daily processing occurred:清除AssayDB,検体登録DB,Sequence number initialization,Numbering history initialization.");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                                CarisXLogInfoBaseExtention.Empty, "Daily processing occurred:清除AssayDB,検体登録DB,Sequence number initialization,Numbering history initialization.");
                //
                // DB内容初期化
                //

                // 分析DB全消去,template
                Singleton<SpecimenAssayDB>.Instance.ClearAssayData();
                Singleton<SpecimenAssayDB>.Instance.CommitData();

                Singleton<CalibratorAssayDB>.Instance.ClearAssayData();
                Singleton<CalibratorAssayDB>.Instance.CommitData();

                Singleton<ControlAssayDB>.Instance.ClearAssayData();
                Singleton<ControlAssayDB>.Instance.CommitData();

                // 検体登録DB全消去
                Singleton<SpecimenGeneralDB>.Instance.DeleteAll();
                Singleton<SpecimenGeneralDB>.Instance.CommitSampleInfo();

                // STAT登録DB一時登録データ消去
                Singleton<SpecimenStatDB>.Instance.Delete(RegistType.Temporary);
                Singleton<SpecimenStatDB>.Instance.CommitSampleInfo();

                // シーケンス番号初期化
                Singleton<SequencialSampleNo>.Instance.ResetNumber();
                Singleton<SequencialPrioritySampleNo>.Instance.ResetNumber();
                Singleton<SequencialCalibNo>.Instance.ResetNumber();
                Singleton<SequencialControlNo>.Instance.ResetNumber();
                Singleton<ReceiptNo>.Instance.ResetNumber();
                //int nNumber = Singleton<ReceiptNo>.Instance.Number;

                //保存相关的配置设置。
                // Singleton<ParameterFilePreserve<AppSettings>>.Instance.Save();

                // 発番履歴初期化
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleSequenceNumberHistory.ClearHistory();
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Save();
                // 各画面の検索関連UIの初期表示日付を更新

                // 表示更新
                RealtimeDataAgent.LoadAssayData();
                RealtimeDataAgent.LoadSampleData();
                RealtimeDataAgent.LoadStatData();
            }
        }

        /// <summary>
        /// 分析終了コマンド受信
        /// </summary>
        /// <remarks>
        /// スレーブより送信された、分析終了コマンドに対する処理を行います。
        /// このコマンドは分析終了時に送信されます。
        /// </remarks>
        /// <param name="command">分析終了コマンド</param>
        protected void onAssayEnd(SlaveCommCommand_0507 command)
        {
            //TODO:分析(強制)終了コマンド処理

            //モジュールインデックスを取得
            RackModuleIndex moduleId = (RackModuleIndex)CarisXSubFunction.MachineCodeToRackModuleIndex((MachineCode)command.CommNo);

            //レスポンスを返す
            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((MachineCode)command.CommNo, new SlaveCommCommand_1507());

            // 状態遷移
            Singleton<SystemStatus>.Instance.setModuleStatus(moduleId, SystemStatusKind.Standby);

            // 消耗品問い合わせ
            Int32 moduleIndex = CarisXSubFunction.MachineCodeToModuleIndex((MachineCode)command.CommNo);
            CarisXSequenceHelper.SequenceSyncObject seqData = Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleIndex].AskSupplieParameter();

            // モジュール１～４のステータスがWaitになったら行う
            if (Singleton<SystemStatus>.Instance.chkModule1to4Status(SystemStatusKind.Standby, false))
            {
                if (!chkRackNoUse.Checked)
                {
                    // リアルタイム印刷(全て実行)
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.RealtimePrintData, true);

                    // 分析中のみスレーブからの分析終了コマンドを受付
                    
                    // 分析中検体情報削除
                    Singleton<InProcessSampleInfoManager>.Instance.Clear();

                    //キャリブレータ情報削除
                    Singleton<CalibratorInfoManager>.Instance.Clear();

                    // 登録・再検DBからエラー状態以外の検体情報を削除する。
                    
                    // 登録DBから非エラー検体削除
                    Singleton<FormSpecimenRegistration>.Instance.ClearAssayNoErrorData();

                    // STAT検体登録DBから非エラー検体削除
                    Singleton<FormSpecimenStatRegistration>.Instance.ClearAssayNoErrorData();

                    // 途中状態のデータを削除する
                    this.clearHalfwayAssayData();

                    //ポーズコマンド（中断）を送信
                    RackTransferCommCommand_0012 cmd0012 = new RackTransferCommCommand_0012();
                    cmd0012.Control = CommandControlParameter.Abort;
                    cmd0012.Stop = RackTransferCommCommand_0012.StopParameter.AllStop;
                    Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0012);

                    //ラックへラック排出コマンドを送信
                    Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(new RackTransferCommCommand_0069());

                    //ラックデータ削除
                    Singleton<RackInfoManager>.Instance.Clear();
                }

                // 操作履歴 分析が強制終了されました。
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_064 });
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
        private void realtimePrint(Object data)
        {
            if (data is Boolean)
            {
                Boolean immediatelyExecute = (Boolean)data;

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
        }

        /// <summary>
        /// 測定結果ファイル保存
        /// </summary>
        /// <remarks>
        /// 測定結果ファイルへ保存します
        /// </remarks>
        /// <param name="calcData"></param>
        private void saveMeasureResultFile(Object data)
        {
            CalcData calcData = data as CalcData;

            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.MeasurementResultFileParameter.Enable)
            {
                String filePath = String.Empty;
                if (String.IsNullOrEmpty(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.MeasurementResultFileParameter.FolderName))
                {
                    filePath = CarisXConst.PathExport;
                }
                else
                {
                    filePath = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.MeasurementResultFileParameter.FolderName;
                }

                filePath += "\\" + calcData.RackID.GetSampleKind().ToString() + DateTime.Today.ToString("yyMMdd") + ".csv ";

                switch (calcData.RackID.GetSampleKind())
                {
                    case SampleKind.Sample:
                    case SampleKind.Priority:
                        Singleton<DataHelper>.Instance.ExportCsv(
                            Singleton<SpecimenResultDB>.Instance.GetSearchData().Where((resultData) => resultData.GetIndividuallyNo() == calcData.IndividuallyNo && resultData.GetUniqueNo() == calcData.UniqueNo),
                            ((FormSpecimenResult)this.childFormList.Single((form) => form is FormSpecimenResult)).ResultGridColumns,
                            filePath,
                            null);
                        break;
                    case SampleKind.Control:
                        Singleton<DataHelper>.Instance.ExportCsv(
                            Singleton<ControlResultDB>.Instance.GetSearchData().Where((resultData) => resultData.GetIndividuallyNo() == calcData.IndividuallyNo && resultData.GetUniqueNo() == calcData.UniqueNo),
                            ((FormControlResult)this.childFormList.Single((form) => form is FormControlResult)).ResultGridColumns,
                            filePath,
                            null);
                        break;
                    case SampleKind.Calibrator:
                        Singleton<DataHelper>.Instance.ExportCsv(
                            Singleton<CalibratorResultDB>.Instance.GetData().Where((resultData) => resultData.GetIndividuallyNo() == calcData.IndividuallyNo && resultData.GetUniqueNo() == calcData.UniqueNo),
                            ((FormCalibResult)this.childFormList.Single((form) => form is FormCalibResult)).ResultGridColumns,
                            filePath,
                            null);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// サンプリングステージ中のラックの指定ラックポジションのステータス取得
        /// </summary>
        /// <remarks>
        /// サンプリングステージ中のラックの指定ラックポジションのステータス取得します
        /// </remarks>
        /// <param name="individuallyNoList">ラック内の全ての検体識別番号</param>
        /// <param name="individuallyNo">検体識別番号</param>
        /// <param name="rackPos">ラックポジション</param>
        /// <param name="sampleInfo">分析中情報</param>
        /// <returns></returns>
        private static RackPosStatus getStatusSamplingStage(Int32[] individuallyNoList, Int32 individuallyNo, Int32 rackPos, List<SampleInfo> sampleInfo)
        {
            if (sampleInfo != null && sampleInfo.Any((v) => v.IsError))
            {
                return RackPosStatus.Error;
            }

            if (individuallyNo == 0)
            {
                // *サンプリングステージラックのみの判定
                // 現在のラックポジション以降に検体番号0以上があれば、現在のポジションはEmpty
                // なければ現在のポジションはUnknown
                if (individuallyNoList.Skip(rackPos).Sum() > 0)
                {
                    return RackPosStatus.Empty;
                }

                return RackPosStatus.Unknown;
            }

            return RackPosStatus.Normal;
        }

        /// <summary>
        /// 優先ラック中のラックの指定ラックポジションのステータス取得
        /// </summary>
        /// <remarks>
        /// 優先ラック中のラックの指定ラックポジションのステータス取得します
        /// </remarks>
        /// <param name="rackInfo">ラックステータス</param>
        /// <param name="individuallyNo">検体識別番号</param>
        /// <param name="sampleInfo">分析中情報</param>
        /// <returns></returns>
        private static RackPosStatus getStatusPriorityNormal(RackStatus rackStatus, Int32 individuallyNo, List<SampleInfo> sampleInfo)
        {
            if (sampleInfo != null && sampleInfo.Any((v) => v.IsError))
            {
                return RackPosStatus.Error;
            }

            if (rackStatus == RackStatus.Waiting)
            {
                return RackPosStatus.Unknown;
            }

            if (individuallyNo > 0)
            {
                return RackPosStatus.Normal;
            }

            return RackPosStatus.Empty;
        }

        /// <summary>
        /// 警告灯・ブザーコマンド送信
        /// </summary>
        /// <remarks>
        /// エラーコードに沿って、警告灯・ブザーコマンドの送信を行います。
        /// </remarks>
        /// <param name="val">エラーコード</param>
        protected void sendCaution(Object val)
        {
            DPRErrorCode errorCode = val as DPRErrorCode;

            // エラーコードを見て、以下の分岐を行う
            // <100  : エラー
            // >=100 : 警告

            // ブザー制御コマンド生成
            SlaveCommCommand_0465 cmd0465 = new SlaveCommCommand_0465();
            {
                // ブザーON/OFF
                if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.BeepVolume != ErrWarningBeepParameter.BeepVolumeKind.None)
                {   // ON
                    cmd0465.Ctrl = SlaveCommCommand_0465.ControlKind.ON;
                }
                else
                {   // OFF
                    cmd0465.Ctrl = SlaveCommCommand_0465.ControlKind.OFF;
                }

                // ブザー音（音色の設定はシステムパラメータコマンドで行っている）
                if (errorCode.ErrorCode < CarisXConst.ERROR_CODE_THRESHOLD)
                {   // エラー
                    cmd0465.Sound = (SlaveCommCommand_0465.SoundKind)Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.BeepError;
                }
                else
                {   // 警告
                    cmd0465.Sound = (SlaveCommCommand_0465.SoundKind)Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.BeepWarning;
                }
            }

            // コマンド送信先モジュールIndex取得（ラック搬送の場合、スレーブ(=0)になる）
            Int32 moduleIndex = CarisXSubFunction.ModuleIDToModuleIndex(errorCode.ModuleId);

            // エラーコード付きの場合
            if (errorCode.ErrorCode > 0)
            {
                // ブザー鳴動制御コマンド送信
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((int)moduleIndex, cmd0465);
            }
        }

        /// <summary>
        /// ブザー消去コマンド送信
        /// </summary>
        /// <remarks>
        /// ブザー消去コマンド送信します
        /// </remarks>
        /// <param name="val"></param>
        protected void sendBuzzerCancel(Object val)
        {
            Int32 moduleIndex = (Int32)val;
            if (moduleIndex == CarisXConst.ALL_MODULEID)
            {
                // 各モジュールへブザー制御コマンド（消去）を送信
                foreach (int moduleIdx in Enum.GetValues(typeof(ModuleIndex)))
                {
                    // オンラインとなっているモジュールへ送信
                    if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleIdx) == ConnectionStatus.Online)
                    {
                        SlaveCommCommand_0465 cmd0465 = new SlaveCommCommand_0465();
                        cmd0465.Ctrl = SlaveCommCommand_0465.ControlKind.OFF;
                        cmd0465.Sound = SlaveCommCommand_0465.SoundKind.Other;
                        Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((int)moduleIdx, cmd0465);
                    }
                }
            }
            else
            {
                // 対象モジュール１つにブザー制御コマンド（消去）を送信
                SlaveCommCommand_0465 cmd0465 = new SlaveCommCommand_0465();
                cmd0465.Ctrl = SlaveCommCommand_0465.ControlKind.OFF;
                cmd0465.Sound = SlaveCommCommand_0465.SoundKind.Other;
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((int)moduleIndex, cmd0465);
            }
        }

        /// <summary>
        /// ホストから受信したワークシートを設定
        /// </summary>
        /// <remarks>
        /// ホストから受信したワークシートを設定します
        /// </remarks>
        /// <param name="command"></param>
        /// <param name="indicate"></param>
        protected void workSheetFromHost(Object data)
        {
            if( data is AskWorkSheetData)
            {
                // ワークシートデータ設定
                AskWorkSheetData askData = data as AskWorkSheetData;

                Singleton<CarisXReceiveCommandThread>.Instance.WorkSheetFromHost(askData);
            }

            return;
        }

        /// <summary>
        /// ユーザレベル設定
        /// </summary>
        /// <remarks>
        /// ユーザーレベルの設定を行います
        /// </remarks>
        protected override void setUser(Object value)
        {
            // 表示中の画面を消す
            FormChildBase lastForm = ((FormChildBase)this.OwnedForms.FirstOrDefault((form) => form is FormChildBase && (form as FormChildBase).IsVisible));
            if (lastForm != null)
            {
                lastForm.Hide();
            }
            this.lblTitleMdiChildName.Visible = false;

            // 画面表示履歴のクリア
            Singleton<HistoryManager>.Instance.ClearHistory();
            this.dicSelectedRecent.Clear();

        }

        /// <summary>
        /// 残量チェック
        /// </summary>
        /// <remarks>
        /// ラック、スレーブに残量チェックコマンドの送信を行います。
        /// </remarks>
        /// <returns>True:問合せ成功 False:問合せ失敗</returns>
        private Boolean askReagentRemain()
        {
            Boolean askSuccess = false;

            CarisXSequenceHelper.SequenceSyncObject syncData;
            Boolean SkipRack = false;

            if (chkRackNoUse.Checked)
            {
                syncData = new CarisXSequenceHelper.SequenceSyncObject();
                SkipRack = true;
            }
            else
            {
                //ラックへの残量チェックコマンド送信
                syncData = Singleton<CarisXSequenceHelperManager>.Instance.RackTransfer.AskRackReagentRemain();
                while (!syncData.EndSequence.WaitOne(10))
                {
                    // ここをDoEventsでの待ちにしない場合、上位の処理をブロック単位に切り分けて複数段階での実行を行う事になり
                    // コード全体の見通しが悪くなる為使用する。メインスレッドをブロックして構わない場合この限りではない。
                    Application.DoEvents();
                }
                if (syncData.Status == CarisXSequenceHelper.SequenceSyncObject.SequenceStatus.Success)
                {
                    CarisXSubFunction.SetRackReagentRemain(syncData.SequenceResultData as IRackRemainAmountInfoSet);
                    askSuccess = true;
                }
            }

            if (syncData.Status == CarisXSequenceHelper.SequenceSyncObject.SequenceStatus.Success || SkipRack)
            {
                //スレーブへの残量チェックコマンド送信
                CarisXSequenceHelper.SequenceSyncObject[] syncDataWasteList = new CarisXSequenceHelper.SequenceSyncObject[Enum.GetValues(typeof(ModuleIndex)).Length];
                foreach (Int32 moduleindex in Enum.GetValues(typeof(ModuleIndex)))
                {
                    // モーターエラーのスレーブは処理を行わない
                    if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)moduleindex)] == Status.SystemStatusKind.MotorError)
                    {
                        continue;
                    }

                    syncDataWasteList[moduleindex] = new CarisXSequenceHelper.SequenceSyncObject();
                }

                foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
                {
                    if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                    {
                        // モーターエラーのスレーブは処理を行わない
                        if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)moduleindex)] == Status.SystemStatusKind.MotorError)
                        {
                            continue;
                        }

                        //接続されているモジュールそれぞれに残量チェックコマンドを送信する
                        syncDataWasteList[moduleindex] = Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleindex].AskReagentRemain();
                    }
                }

                foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
                {
                    if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                    {
                        // モーターエラーのスレーブは処理を行わない
                        if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)moduleindex)] == Status.SystemStatusKind.MotorError)
                        {
                            continue;
                        }

                        while (!syncDataWasteList[moduleindex].EndSequence.WaitOne(10))
                        {
                            // ここをDoEventsでの待ちにしない場合、上位の処理をブロック単位に切り分けて複数段階での実行を行う事になり
                            // コード全体の見通しが悪くなる為使用する。メインスレッドをブロックして構わない場合この限りではない。
                            Application.DoEvents();
                        }
                        if (syncDataWasteList[moduleindex].Status == CarisXSequenceHelper.SequenceSyncObject.SequenceStatus.Success)
                        {
                            CarisXSubFunction.SetReagentRemain(syncDataWasteList[moduleindex].SequenceResultData as IRemainAmountInfoSet, false
                                , CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)moduleindex), false); //残量チェックの場合は履歴情報の残量を上書きしない
                            askSuccess = true;
                        }
                    }
                }
            }

            return askSuccess;
        }

        /// <summary>
        /// システムパラメータ送信
        /// </summary>
        /// <remarks>
        /// ラック、スレーブにシステムパラメータ送信コマンドの送信を行います。
        /// </remarks>
        /// <returns>True:問合せ成功 False:問合せ失敗</returns>
        private Boolean sendSystemParameter()
        {
            Boolean askSuccess = false;

            CarisXSequenceHelper.SequenceSyncObject syncData;
            Boolean SkipRack = false;

            if (chkRackNoUse.Checked)
            {
                syncData = new CarisXSequenceHelper.SequenceSyncObject();
                SkipRack = true;
            }
            else
            {
                //ラックへのシステムパラメータコマンド送信
                syncData = Singleton<CarisXSequenceHelperManager>.Instance.RackTransfer.SendRackSystemParameter();
                while (!syncData.EndSequence.WaitOne(10))
                {
                    // ここをDoEventsでの待ちにしない場合、上位の処理をブロック単位に切り分けて複数段階での実行を行う事になり
                    // コード全体の見通しが悪くなる為使用する。メインスレッドをブロックして構わない場合この限りではない。
                    Application.DoEvents();
                }

                if (syncData.Status != CarisXSequenceHelper.SequenceSyncObject.SequenceStatus.Success)
                {
                    //エラーの場合
                    CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.CommunicationErrorBetweenMasterAndRackTransfer);
                }
            }

            if (syncData.Status == CarisXSequenceHelper.SequenceSyncObject.SequenceStatus.Success || SkipRack)
            {
                //スレーブへのシステムパラメータコマンド送信
                foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
                {
                    if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                    {
                        // モーターエラーのスレーブは処理を行わない
                        if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)moduleindex)] == Status.SystemStatusKind.MotorError)
                        {
                            continue;
                        }

                        syncData = Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleindex].SendSlaveSystemParameter();
                        while (!syncData.EndSequence.WaitOne(10))
                        {
                            // ここをDoEventsでの待ちにしない場合、上位の処理をブロック単位に切り分けて複数段階での実行を行う事になり
                            // コード全体の見通しが悪くなる為使用する。メインスレッドをブロックして構わない場合この限りではない。
                            Application.DoEvents();
                        }

                        if (syncData.Status == CarisXSequenceHelper.SequenceSyncObject.SequenceStatus.Success)
                        {
                            askSuccess = true;
                        }
                        else
                        {
                            //エラーの場合
                            CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.CommunicationErrorBetweenMasterAndSlave);
                        }
                    }
                }
            }

            return askSuccess;
        }

        /// <summary>
        /// スレーブの分析開始送信
        /// </summary>
        /// <remarks>
        /// スレーブに分析開始送信コマンドの送信を行います。
        /// </remarks>
        /// <returns>True:問合せ成功 False:問合せ失敗</returns>
        private Boolean startSlaveAssay()
        {
            // 分析開始シーケンス成功フラグ
            Boolean askSuccess = false;

            CarisXSequenceHelper.SequenceSyncObject syncData;

            //スレーブへの分析開始コマンド送信
            foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                // スレーブがオンラインだった場合
                if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                {
                    // モーターエラーのスレーブは処理を行わない
                    if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)moduleindex)] == Status.SystemStatusKind.MotorError)
                    {
                        continue;
                    }

                    // スレーブへの分析開始コマンド送信
                    syncData = Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleindex].StartSlaveAssay();
                    while (!syncData.EndSequence.WaitOne(10))
                    {
                        // ここをDoEventsでの待ちにしない場合、上位の処理をブロック単位に切り分けて複数段階での実行を行う事になり
                        // コード全体の見通しが悪くなる為使用する。メインスレッドをブロックして構わない場合この限りではない。
                        Application.DoEvents();
                    }

                    if (syncData.Status == CarisXSequenceHelper.SequenceSyncObject.SequenceStatus.Success)
                    {
                        askSuccess = true;
                    }
                    else
                    {
                        //エラーの場合
                        CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.CommunicationErrorBetweenMasterAndSlave);
                    }
                }
            }

            return askSuccess;
        }

        /// <summary>
        /// ラックの分析開始送信
        /// </summary>
        /// <remarks>
        /// ラックに分析開始送信コマンドの送信を行います。
        /// </remarks>
        /// <param name="data"></param>
        private void startRackAssay( Object data )
        {
            CarisXSequenceHelper.SequenceSyncObject syncData;

            if (chkRackNoUse.Checked)
            {
                syncData = new CarisXSequenceHelper.SequenceSyncObject();
            }
            else
            {
                // ラック搬送接続ステータスがオンラインの場合
                if (Singleton<CarisXCommManager>.Instance.GetRackTransferCommStatus() == ConnectionStatus.Online)
                {
                    // ラックがモーターエラーではない場合
                    if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.RackTransfer] != Status.SystemStatusKind.MotorError)
                    {
                        //ラックへの分析開始コマンド送信
                        syncData = Singleton<CarisXSequenceHelperManager>.Instance.RackTransfer.StartRackAssay();
                        while (!syncData.EndSequence.WaitOne(10))
                        {
                            // ここをDoEventsでの待ちにしない場合、上位の処理をブロック単位に切り分けて複数段階での実行を行う事になり
                            // コード全体の見通しが悪くなる為使用する。メインスレッドをブロックして構わない場合この限りではない。
                            Application.DoEvents();
                        }

                        // シーケンス状態が失敗の場合
                        if (syncData.Status != CarisXSequenceHelper.SequenceSyncObject.SequenceStatus.Success)
                        {
                            //エラーの場合
                            CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.CommunicationErrorBetweenMasterAndRackTransfer);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 分析開始
        /// </summary>
        /// <remarks>
        /// 検体分析を開始します。
        /// </remarks>
        private void assayStart()
        {
            // add start 修正内容 : モーターエラー処理追加
            // 処理を行うか判定。下記の場合、分析を行わない
            // ・ラックがモーターエラーの場合  ・全てのモジュールがモーターエラーの場合
            if (CarisXSubFunction.CheckMotorErrorAllRackModule())
            {
                // ラックまたは全てのスレーブにモーターエラーがあるため分析開始できませんでした。
                System.Diagnostics.Debug.WriteLine("Initial sequence: ラックまたは全てのスレーブにモーターエラーがあるため分析開始できませんでした。");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                             "Initial sequence  Analysis could not be started due to a motor error in the rack or all slaves.");
                return;
            }
            // add end 修正内容 : モーターエラー処理追加

            // 待機中・一時停止中であれば分析開始可能
            if ((Singleton<SystemStatus>.Instance.Status == SystemStatusKind.Standby)
                || (Singleton<SystemStatus>.Instance.Status == SystemStatusKind.SamplingPause))
            {
                //ラック、モジュールのどちらかがすべて繋がっていない場合はエラー（Assayが出来ない為）
                var module1to4Status = Singleton<SystemStatus>.Instance.getModule1to4Status(false);
                if ((!chkRackNoUse.Checked && Singleton<SystemStatus>.Instance.ModuleStatus[(int)RackModuleIndex.RackTransfer] == SystemStatusKind.NoLink)
                    || (Singleton<SystemStatus>.Instance.chkModule1to4Status(SystemStatusKind.NoLink, false)))
                {
                    DlgMessage.Show(Properties.Resources.STRING_DLG_MSG_248, "", CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OK);
                    return;
                }

                // 交換開始ボタンDisable
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                             CarisXLogInfoBaseExtention.Empty, "assayStart to ReagentChangeIsRefused");
                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ReagentChangeIsRefused, null);
                this.Cursor = Cursors.WaitCursor;

                // 試薬ボトル残量確認
                // トリガ、プレトリガ残量確認
                // 洗浄液有無確認
                // サンプル分注チップ、反応容器の残量確認
                // 廃液タンク確認
                // 廃棄ボックス確認
                // 検量線確認
                // 試薬テーブル（ルーチンテーブル）に設置されている項目のロットの検量線が存在するか確認。
                Singleton<CalibrationCurveDB>.Instance.LoadDB();

                //スレーブ応答待ち状態にする
                Singleton<SystemStatus>.Instance.setAllModuleStatus(SystemStatusKind.WaitSlaveResponce);

                // 残量チェックを行う
                if (this.askReagentRemain() == false)
                {
                    // 操作履歴登録
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                        , CarisXLogInfoBaseExtention.Empty, CarisX.Properties.Resources.STRING_LOG_MSG_069);
                    this.Cursor = Cursors.Default;
                    Singleton<SystemStatus>.Instance.RestoreAllModulePrevStatus();
                    return;
                }
                // 残量警告によらず分析開始を行えるよう変更
                Boolean reag = CarisXSubFunction.ReagentRemainWarning();
                if (reag)
                {
                    // 交換開始ボタンEnable
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                        , CarisXLogInfoBaseExtention.Empty, " start the assay cancel to ReagntChangeIsAllowed");
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ReagentChangeIsAllowed, null);
                    // 操作履歴登録
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                        , CarisXLogInfoBaseExtention.Empty, Properties.Resources.STRING_LOG_MSG_069);
                    Singleton<SystemStatus>.Instance.RestoreAllModulePrevStatus();
                    this.Cursor = Cursors.Default;
                    return;
                }

                // 開始ならコマンド0411,再開ならコマンド0412 
                if (Singleton<SystemStatus>.Instance.PrevSystemStatus == SystemStatusKind.Standby) // 応答待機ステータスにする前のステータスと比較する
                {
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                        , CarisXLogInfoBaseExtention.Empty, "Assay Start");

                    // 分析項目の検量線確認は警告のみ表示する。
                    this.checkUseProtocolCalibCurve();

                    DlgAssayStartConfirm dlgAssayStartConfirm = new DlgAssayStartConfirm();
                    if (DialogResult.OK == dlgAssayStartConfirm.ShowDialog())
                    {
                        Boolean userCancel = false; // ユーザー意思によるキャンセルが行われたか

                        // 加温待ちを行う
                        using (var dlgWaitTemperature = new DlgWaitAnalyzerTemperature())
                        {
                            if (dlgWaitTemperature.IsNeedShow())
                            {
                                var dlgResult = dlgWaitTemperature.ShowDialog();
                                userCancel = (dlgResult == System.Windows.Forms.DialogResult.Cancel);
                            }
                        }
                        if (!userCancel)
                        {
                            // システムパラメータを送信
                            // メンテナンスで温度設定を変えている場合があるので、分析前に再セットする。
                            if (this.sendSystemParameter() == false)
                            {
                                // 操作履歴登録
                                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                                    , CarisXLogInfoBaseExtention.Empty, CarisX.Properties.Resources.STRING_LOG_MSG_069);
                                this.Cursor = Cursors.Default;
                                Singleton<SystemStatus>.Instance.RestoreAllModulePrevStatus();
                                return;
                            }

                            // 分析開始
                            if (this.startSlaveAssay() == false)
                            {
                                // 操作履歴登録
                                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                                    , CarisXLogInfoBaseExtention.Empty, CarisX.Properties.Resources.STRING_LOG_MSG_069);
                                this.Cursor = Cursors.Default;
                                Singleton<SystemStatus>.Instance.RestoreAllModulePrevStatus();
                                return;
                            }

                            // 日替わりチェック(UIPackageが記録していた前回アクセス日付を元にする)
                            Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.DateChanged, null); // 日替わり処理

                            // 分析中検体情報をクリアする（GのCalibInfoManager）
#if DEBUG
                            if (!chkRackNoUse.Checked)
                            {
                                Singleton<InProcessSampleInfoManager>.Instance.Clear();
                            }
#else
                            Singleton<InProcessSampleInfoManager>.Instance.Clear();
#endif
                            // ラック設置状況リストの全削除
                            Singleton<RackSettingStatusManagerList>.Instance.RackSettingDataAllClear();
                        }
                        else
                        {
                            // 操作履歴登録
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                                , CarisXLogInfoBaseExtention.Empty, CarisX.Properties.Resources.STRING_LOG_MSG_071);

                            // 加温キャンセル時の処理
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                                , CarisXLogInfoBaseExtention.Empty, "SdlgWaitTemperature cancel to ReagntChangeIsAllowed");
                            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ReagentChangeIsAllowed, null);

                            //ここでステータスを戻す
                            Singleton<SystemStatus>.Instance.RestoreAllModulePrevStatus();
                            this.Cursor = Cursors.Default;
                            return;
                        }
                    }
                    else
                    {
                        // 交換開始ボタンEnable
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                            , CarisXLogInfoBaseExtention.Empty, " start the assay cancel to ReagntChangeIsAllowed");
                        Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ReagentChangeIsAllowed, null);
                        // 操作履歴登録
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                            , CarisXLogInfoBaseExtention.Empty, Properties.Resources.STRING_LOG_MSG_071);
                        Singleton<SystemStatus>.Instance.RestoreAllModulePrevStatus();
                        this.Cursor = Cursors.Default;
                        return;
                    }
                }
                else
                {
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                        , CarisXLogInfoBaseExtention.Empty, "Assay ReStart");

                    // ラック搬送へポーズコマンド(分析再開)を送信
                    RackTransferCommCommand_0012 cmd0012 = new RackTransferCommCommand_0012();
                    cmd0012.Control = CommandControlParameter.Restart;
                    cmd0012.Stop = RackTransferCommCommand_0012.StopParameter.SamplingStop;
                    Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0012);

                    // 各モジュールへポーズコマンド(分析再開)を送信
                    foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
                    {
                        // オンラインとなっているモジュールへ送信
                        if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                        {
                            // モーターエラーのスレーブは処理を行わない
                            if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)moduleindex)] == Status.SystemStatusKind.MotorError)
                            {
                                continue;
                            }

                            // ポーズコマンド(分析再開)を送信
                            SlaveCommCommand_0412 cmd0412 = new SlaveCommCommand_0412();
                            cmd0412.Control = CommandControlParameter.Restart;
                            cmd0412.Stop = SlaveCommCommand_0412.StopParameter.SamplingStop;
                            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(moduleindex, cmd0412);
                        }
                    }                    

                    Singleton<SystemStatus>.Instance.setAllModuleStatus(SystemStatusKind.Assay);
                }

                // エラー蓄積データリストの全削除
                Singleton<ErrorDataStorageListManeger>.Instance.DeleteAllList();

                this.Cursor = Cursors.Default;

                // 操作履歴登録
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, CarisX.Properties.Resources.STRING_LOG_MSG_070);

            }
        }

        /// <summary>
        /// 分析一時停止
        /// </summary>
        /// <remarks>
        /// 検体分析を一時停止します。
        /// </remarks>
        private void assayPause()
        {
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                , CarisXLogInfoBaseExtention.Empty, "Assay Pause");

            flgManualAssayPause = true;

            // 分析一時停止

            // ラック搬送へポーズコマンド(サンプリング停止)を送信
            RackTransferCommCommand_0012 cmd0012 = new RackTransferCommCommand_0012();
            cmd0012.Control = CommandControlParameter.Pause;
            cmd0012.Stop = RackTransferCommCommand_0012.StopParameter.SamplingStop;
            Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0012);

            // 各モジュールへポーズコマンド(サンプリング停止)を送信
            foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                // オンラインとなっているモジュールへ送信
                if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                {
                    // モーターエラーのスレーブは処理を行わない
                    if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)moduleindex)] == Status.SystemStatusKind.MotorError)
                    {
                        continue;
                    }

                    // ポーズコマンド(サンプリング停止)を送信
                    SlaveCommCommand_0412 cmd0412 = new SlaveCommCommand_0412();
                    cmd0412.Control = CommandControlParameter.Pause;
                    cmd0412.Stop = SlaveCommCommand_0412.StopParameter.SamplingStop;
                    Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(moduleindex, cmd0412);
                }
            }

            Singleton<SystemStatus>.Instance.setAllModuleStatus(SystemStatusKind.SamplingPause);

            //スレーブ・ラックの１サイクル中にポーズ→再開とした時にアイコンを元に戻せないのでやめる
            ////ボタンを押下した段階でアイコンを「！」に変更する
            //Singleton<SystemStatus>.Instance.setAllModulePauseReason(SamplingPauseReason.SamplingPauseReasonBit.SamplingStopKeyPressed);
            //setSystemStatusIcon();

            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, CarisX.Properties.Resources.STRING_LOG_MSG_072);
        }


        /// <summary>
        /// 消耗品確認応答
        /// </summary>
        /// <remarks>
        /// スレーブからの消耗品確認コマンド応答を取得します。
        /// </remarks>
        /// <param name="value">応答コマンド</param>
        private void showDialogSupplie(Object value)
        {
            //if ( seqData.Status == CarisXSequenceHelper.SequenceSyncObject.SequenceStatus.Success )
            //{
            //    CommandDataMediator.SetSupplieCmdToParam( (SlaveCommCommand_1444)seqData.SequenceResultData, ref supplie );
            //}
            // 寿命確認
            //            supplie.OverList; // 寿命切れ項目リスト
            // TODO:消耗品警告Dlg表示

        }

        /// <summary>
        /// 分析中断
        /// </summary>
        /// <remarks>
        /// 検体分析を中断します。
        /// </remarks>
        private void assayAbort()
        {
            // リアルタイム印刷(全て実行)
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.RealtimePrintData, true);
            
            // 試薬交換中はメッセージ表示して失敗とする。
            // 登録画面・再検査画面のエラー発生以外の情報を削除する。
            // 分析中断

            // ラックへのポーズコマンド送信はラック排出コマンド送信の直前に行うため、ここでは行わない

            // 各モジュールへポーズコマンド(中断)を送信
            foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                // オンラインとなっているモジュールへ送信
                if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                {
                    // モーターエラーのスレーブは処理を行わない
                if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)moduleindex)] == Status.SystemStatusKind.MotorError)
                {
                    continue;
                }

                    // ポーズコマンド(中断)を送信
                    SlaveCommCommand_0412 cmd0412 = new SlaveCommCommand_0412();
                    cmd0412.Control = CommandControlParameter.Abort;
                    cmd0412.Stop = SlaveCommCommand_0412.StopParameter.AllStop;
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(moduleindex, cmd0412);
                }
            }

            // ステータスを分析終了移行中に変更
            Singleton<SystemStatus>.Instance.setAllModuleStatus(SystemStatusKind.ToEndAssay);

            // 分析中検体情報削除
            Singleton<InProcessSampleInfoManager>.Instance.Clear();

            //ラックデータ削除
            Singleton<RackInfoManager>.Instance.Clear();

            //キャリブレータ情報削除
            Singleton<CalibratorInfoManager>.Instance.Clear();

            // 登録DBから非エラー検体削除
            Singleton<FormSpecimenRegistration>.Instance.ClearAssayNoErrorData();

            // 登録DBから非エラーSTAT検体削除
            Singleton<FormSpecimenStatRegistration>.Instance.ClearAssayNoErrorData();

            // 途中状態のデータを削除する
            this.clearHalfwayAssayData();

            // エラー蓄積データリストの全削除
            Singleton<ErrorDataStorageListManeger>.Instance.DeleteAllList();

            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, CarisX.Properties.Resources.STRING_LOG_MSG_064);

            // 2020-02-27 CarisX IoT Add [START]
#if !NOT_USE_IOT
            // 配信情報をクラウドにアップロードする
            CarisXSubFunction.SendIoTDueDate();

            // システムファイルをIoTクラウドへアップロードする
            CarisXSubFunction.SendIoTSystemFiles(DateTime.Now);
#endif
            // 2020-02-27 CarisX IoT Add [END]
        }

        // 2020-02-27 CarisX IoT Add [START]
#if !NOT_USE_IOT
        /// <summary>
        /// IoT障害情報コマンド送信通知
        /// </summary>
        /// <param name="value"></param>
        private void sendIoTErrorCommand(Object value)
        {
            // IoT障害情報コマンド送信
            CarisXSubFunction.SendIoTErrorCommand(value);
        }
#endif
        // 2020-02-27 CarisX IoT Add [END]

        #endregion

        #region _分析ボタンクリックイベント_
        /// <summary>
        /// 分析開始ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 分析開始します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnAssayStart_Click(object sender, EventArgs e)
        {
            this.assayStart();
        }
        /// <summary>
        /// 分析一時停止ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 分析一時停止します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnAssayPause_Click(object sender, EventArgs e)
        {
            this.assayPause();
        }

        /// <summary>
        /// 分析中断ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 分析中断します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnAssayAbort_Click(object sender, EventArgs e)
        {
            // 分析終了確認ダイアログ表示
            if (DialogResult.OK == DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_222, "", CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel))
            {
                this.assayAbort();
            }
        }

        #endregion

        #region _その他ボタンクリックイベント_
        /// <summary>
        /// エラーボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// エラーをクリアします
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnError_Click(object sender, EventArgs e)
        {
            // 編集中かどうか、また編集中でも画面遷移するかどうか
            if (CarisXSubFunction.IsEditsMessageShow())
            {
                // 点滅していた場合、点滅を終了する。
                this.btnError.BlinkEnd();

                // スレーブにブザー消去送信
                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SendBuzzerCancel, CarisXConst.ALL_MODULEID);

                Singleton<HistoryManager>.Instance.AddNew(CarisXHistoryActionKind.GetKind(CarisXHistoryKind.ShowSystemLog), () =>
                {
                    this.showChildForm<FormSystemLog>();
                    this.largeMenuButtonStateChange(this.btnMenuLargeSystem);
                });
                Singleton<HistoryManager>.Instance.ExecRecent();

                // 編集中フラグOFF
                FormChildBase.IsEdit = false;
            }
        }

        /// <summary>
        /// ヘルプボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ヘルプファイル起動します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnHelp_Click(object sender, EventArgs e)
        {
                // 操作履歴登録
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { CarisX.Properties.Resources.STRING_LOG_MSG_065 });

            // start DataAdd
            #if DEBUG_CARIS_OLD_SOCKET
            int iUniqueNo = 0;

            // 判定代わり
            int iItem = 2;

            if (iItem == 0)
            {
                iUniqueNo = Singleton<Oelco.CarisX.DB.SpecimenResultDB>.Instance.GetMaxUniqueNo();
                for (int i = 1; i <= 10000; i++)
                {
                    for (int j = 1; j <= 50; j++)
                    {
                        // SQL作成
                        StringBuilder strSql = new StringBuilder();
                        //-----------------------------------------------
                        // Specimen
                        //-----------------------------------------------
                        strSql.Append(" INSERT INTO ");
                        strSql.Append(" dbo.specimenResult ");
                        strSql.Append(" (moduleNo, uniqueNo, replicationNo, individuallyNo, sequenceNo, rackId, rackPosition, sampleId, measureProtocolIndex, count,");
                        strSql.Append(" concentration, replicationRemarkId, countAve, concentrationAve, remarkId, measureDateTime, reagentLotNo, pretriggerLotNo, triggerLotNo,");
                        strSql.Append(" calibCurveDateTime, comment, errorCode, judgement, manualDilution, autoDilution, remeasure, specimenMaterialType,receiptNo, ");
                        strSql.Append(" analysisMode, bGCount, darkCount, resultCount");
                        strSql.Append(" ) VALUES ( ");
                        strSql.Append(" 1, ");
                        strSql.AppendFormat("{0}, ", iUniqueNo + i);
                        strSql.AppendFormat("{0}, ", j);
                        strSql.AppendFormat("{0}, ", iUniqueNo + i);
                        strSql.Append(" 1, ");
                        strSql.Append(" '0001', ");
                        strSql.Append(" 1, ");
                        strSql.Append(" '', ");
                        strSql.Append(" 1, ");
                        strSql.Append(" '30000', ");
                        strSql.Append(" 0.22, ");
                        strSql.Append(" '137438953472', ");
                        strSql.Append(" null ,null ,null, ");
                        strSql.AppendFormat("'{0}', ", DateTime.Now);
                        strSql.Append(" '00000001', '00000001', '00000001', ");
                        strSql.Append(" '2019-10-25 16:19:28.6776278', ");
                        strSql.AppendFormat("'{0}', ", iUniqueNo + i);
                        strSql.Append(" '',null, ");
                        strSql.Append(" '1', '1', '0', '1', '1', '0', ");
                        strSql.Append(" '3200', '90', '33200'");
                        strSql.Append(" ) ");
                        Singleton<DBAccessControl>.Instance.ExecuteSql(strSql.ToString(), String.Empty);
                    }
                }
            }
            else if (iItem == 1)
            {
                iUniqueNo = Singleton<Oelco.CarisX.DB.CalibratorResultDB>.Instance.GetMaxUniqueNo();
                for (int i = 1; i <= 30000; i++)
                {
                    // SQL作成
                    StringBuilder strSql = new StringBuilder();
                    //-----------------------------------------------
                    // Calibrator
                    //-----------------------------------------------
                    strSql.Append(" INSERT INTO ");
                    strSql.Append(" dbo.calibratorResult ");
                    strSql.Append(" (moduleNo, uniqueNo, replicationNo, individuallyNo, sequenceNo, rackId, rackPosition, calibLotNo, measureProtocolIndex, count,");
                    strSql.Append(" concentration, replicationRemarkId, countAve, concentrationAve, remarkId, measureDateTime, reagentLotNo, pretriggerLotNo, triggerLotNo,");
                    strSql.Append(" errorCode, isCreatedCalibCurve, bGCount, darkCount, resultCount");
                    strSql.Append(" ) VALUES ( ");
                    strSql.Append(" 1, ");
                    strSql.AppendFormat("{0}, ", iUniqueNo + i);
                    strSql.Append(" 1, ");
                    strSql.AppendFormat("{0}, ", iUniqueNo + i);
                    strSql.Append(" '9001', ");
                    strSql.Append(" 'S001', ");
                    strSql.Append(" '1', ");
                    strSql.Append(" '', ");
                    strSql.Append(" '1', ");
                    strSql.Append(" '60000', ");
                    strSql.Append(" '100.00', ");
                    strSql.Append(" '0', ");
                    strSql.Append(" null ,null ,null, ");
                    strSql.AppendFormat("'{0}', ", DateTime.Now); //.Ticks
                    strSql.AppendFormat("'{0}', ", iUniqueNo + i);
                    strSql.Append(" '', '', ");
                    strSql.Append(" '', ");
                    strSql.Append(" '1', ");
                    strSql.Append(" '10000', '90', '20000'");
                    strSql.Append(" ) ");
                    Singleton<DBAccessControl>.Instance.ExecuteSql(strSql.ToString(), String.Empty);
                }
            }
            else if (iItem == 2)
            {
                iUniqueNo = Singleton<Oelco.CarisX.DB.ControlResultDB>.Instance.GetMaxUniqueNo();
                for (int i = 1; i <= 30000; i++)
                {
                    // SQL作成
                    StringBuilder strSql = new StringBuilder();
                    //-----------------------------------------------
                    // Control
                    //-----------------------------------------------
                    strSql.Append(" INSERT INTO ");
                    strSql.Append(" dbo.controlResult ");
                    strSql.Append(" (moduleNo, uniqueNo, replicationNo, individuallyNo, sequenceNo, rackId, rackPosition, controlLotNo, controlName, measureProtocolIndex, count,");
                    strSql.Append(" concentration, replicationRemarkId, countAve, concentrationAve, remarkId, measureDateTime, reagentLotNo, pretriggerLotNo, triggerLotNo,");
                    strSql.Append(" calibCurveDateTime, comment, errorCode, bGCount, darkCount, resultCount");
                    strSql.Append(" ) VALUES ( ");
                    strSql.Append(" 1, ");
                    strSql.AppendFormat("{0}, ", iUniqueNo + i);
                    strSql.Append(" 1, ");
                    strSql.AppendFormat("{0}, ", iUniqueNo + i);
                    strSql.Append(" '8001', ");
                    strSql.Append(" 'C001', ");
                    strSql.Append(" '1', ");
                    strSql.Append(" 'test', ");
                    strSql.Append(" 'test', ");
                    strSql.Append(" '1', ");
                    strSql.Append(" '1752', ");
                    strSql.Append(" '0.10', ");
                    strSql.Append(" '4398046511104', ");
                    strSql.Append(" null ,null ,null, ");
                    strSql.AppendFormat("'{0}', ", DateTime.Now);
                    strSql.AppendFormat("'{0}', ", iUniqueNo + i);
                    strSql.Append(" '', '', ");
                    strSql.Append(" '2020-01-23 16:32:34.0266156', ");
                    strSql.Append(" null, ");
                    strSql.Append(" '', ");
                    strSql.Append(" '1000', '90', '1292'");
                    strSql.Append(" ) ");
                    Singleton<DBAccessControl>.Instance.ExecuteSql(strSql.ToString(), String.Empty);
                }
            }
            #endif
                // end DataAdd

                // HELPファイル起動
                // ファイルまでのディレクトリのパスをセット
                CarisX.Const.HelpDocument.openHelpDocumentPage();
        }

        /// <summary>
        /// 終了ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 終了メニュー処理実行します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            // 編集中かどうか、また編集中でも画面遷移するかどうか
            if (CarisXSubFunction.IsEditsMessageShow())
            {
                this.exit();

                // 編集中フラグOFF
                FormChildBase.IsEdit = false;
            }
        }

        /// <summary>
        /// 終了シーケンス完了（スレーブ）
        /// </summary>
        /// <remarks>
        /// （スレーブ）終了シーケンスが完了した際に呼び出されます。
        /// </remarks>
        /// <param name="param"></param>
        private void endSequenceCompleteSlave(Object param)
        {
            SequenceHelperMessage sequenceHelperMessage = (SequenceHelperMessage)param;

            //シャットダウンシーケンスを完了扱いにする
            compSlaveEndSequence[sequenceHelperMessage.ModuleIndex] = true;

            //シャットダウンシーケンスが正常に終了したかどうかを取得
            statusSlaveEndSequence[sequenceHelperMessage.ModuleIndex] = (Boolean)sequenceHelperMessage.MessageParameter;

            endSequenceComplete();
        }

        /// <summary>
        /// 終了シーケンス完了（ラック）
        /// </summary>
        /// <remarks>
        /// ラックの終了シーケンスが完了した際に呼び出されます。
        /// </remarks>
        /// <param name="param"></param>
        private void endSequenceCompleteRack(Object param)
        {
            //シャットダウンシーケンスを完了扱いにする
            compRackEndSequence = true;

            //シャットダウンシーケンスが正常に終了したかどうかを取得
            statusRackEndSequence = (Boolean)param;

            endSequenceComplete();
        }

        /// <summary>
        /// 終了シーケンス完了
        /// </summary>
        /// <remarks>
        /// 終了シーケンスが完了した際に呼び出されます。
        /// </remarks>
        /// <param name="param"></param>
        private void endSequenceComplete()
        {
            if (compSlaveEndSequence.All(v => v == true) && compRackEndSequence)
            {
                // ラックとスレーブの全てのレスポンスを受信出来ている場合
                if (statusSlaveEndSequence.All(v => v == true) && statusRackEndSequence)
                {
                    // ラックもスレーブもどちらも正常に終了出来ている場合
                    // 終了処理＋Windowsをシャットダウン
                    this.WindowsShutdown();
                    this.Close();
                }
                else
                {
                    // ラックとスレーブのどちらか又は両方が正常に終了出来ていない場合
                    FormBase.AllFormItemEnableStatusRestore();
                    Singleton<SystemStatus>.Instance.RestoreAllModulePrevStatus();
                }
            }
        }

        /// <summary>
        /// 終了メニュー生成
        /// </summary>
        /// <remarks>
        /// 終了メニューダイアログの生成を行います。
        /// </remarks>
        /// <returns>終了メニューダイアログ</returns>
        private DlgShutdown createShutdownDlg()
        {
            DlgShutdown dlg = new DlgShutdown((o, args) =>
            {
                //Shutdown

                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { CarisX.Properties.Resources.STRING_LOG_MSG_066 });
                Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SystemCondition = true; // 終了時はTrueにして保存する。
                Singleton<ParameterFilePreserve<AppSettings>>.Instance.Save(); // パラメータ保存

                // 終了シーケンス
                if (this.allShutdown())
                {
                    //シャットダウン中に以降
                    Singleton<SystemStatus>.Instance.setAllModuleStatus(SystemStatusKind.Shutdown);
                }

            }, (o, args) =>
            {
                //Exit

                // システム終了
                this.SystemEnd(o);
            }, (o, args) =>
            {
                //自動立ち上げ（Auto-StartUp）

                // 操作履歴
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { CarisX.Properties.Resources.STRING_LOG_MSG_067 });

                // 自動立ち上げ待機
                if (this.autoStartWait())
                {
                    // 操作履歴(自動立ち上げを実行しました。)   
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { CarisX.Properties.Resources.STRING_LOG_MSG_022 });

                }

            }, (o, args) =>
            {
                //ログアウト（Logout）

                // ログアウト
                Singleton<CarisXUserLevelManager>.Instance.Logout();

                // 操作履歴
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { CarisX.Properties.Resources.STRING_LOG_MSG_068 });
            });

            return dlg;
        }

        /// <summary>
        /// 終了メニュー処理
        /// </summary>
        /// <remarks>
        /// 終了メニューの表示を行います。
        /// </remarks>
        protected void exit()
        {
            using (DlgShutdown dlg = this.createShutdownDlg())
            {
                dlg.ShowDialog();
                if (DlgShutdown.SelectedMethod.LogOut == dlg.SelectResult)
                {
                    this.showLogin();
                }
            }
        }

        #endregion

        /// <summary>
        /// フォームの表示
        /// </summary>
        /// <remarks>
        /// 子画面を表示します
        /// </remarks>
        /// <typeparam name="classT">子画面クラス</typeparam>
        private void showChildForm<classT>()
            where classT : FormChildBase, new()
        {
            // stopwatch
            //Stopwatch sw = new Stopwatch();
            //sw.Start();

            Rectangle rect = new Rectangle(this.PointToScreen(this.pnlMdiChild.Location), new Size(this.pnlMdiChild.Width, this.pnlMdiChild.Height));

            foreach (Form form in this.OwnedForms)
            {
                //asキャスト
                FormChildBase formBase = form as FormChildBase;
                if (this != form && formBase != null)
                {
                    if (formBase.IsVisible)
                    {
                        formBase.BringToFront();
                        formBase.Hide();
                        formBase.Hide(rect);
                        break;  //追加
                    }
                }
            }

            Singleton<classT>.Instance.Show(rect);

            this.lblTitleMdiChildName.Visible = true;
            this.lblTitleMdiChildName.Text = Singleton<classT>.Instance.Text;
            Singleton<classT>.Instance.Visible = true;

            // 操作履歴：画面表示   
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { CarisX.Properties.Resources.STRING_LOG_MSG_019 + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + this.lblTitleMdiChildName.Text });

            // stopwatch
            //sw.Stop();
            //Console.WriteLine("経過時間 子画面クラス　={0}", sw.Elapsed);
        }

        /// <summary>
        /// 小画面非表示化
        /// </summary>
        /// <remarks>
        /// 小画面非表示化します
        /// </remarks>
        /// <typeparam name="classT"></typeparam>
        private void hideChildForm<classT>()
           where classT : FormChildBase, new()
        {
            Singleton<classT>.Instance.Hide();
        }

        /// <summary>
        /// 大項目選択状態
        /// </summary>
        /// <remarks>
        /// 大項目に属する小項目で直近に選択されたものを記録する為に使用されます。
        /// </remarks>
        public enum SelectedLargeMenu
        {
            /// <summary>
            /// 分析
            /// </summary>
            Assay,
            /// <summary>
            /// 試薬
            /// </summary>
            Reagent,
            /// <summary>
            /// 検体
            /// </summary>
            Specimen,
            /// <summary>
            /// キャリブレータ
            /// </summary>
            Calibrator,
            /// <summary>
            /// 精度管理
            /// </summary>
            Control,
            /// <summary>
            /// オプション
            /// </summary>
            Option,
            /// <summary>
            /// システム
            /// </summary>
            System
        }
        /// <summary>
        /// 大項目選択状態
        /// </summary>
        private SelectedLargeMenu nowSelectedMenu;
        /// <summary>
        /// 直近選択項目記録
        /// </summary>
        private Dictionary<SelectedLargeMenu, Action> dicSelectedRecent = new Dictionary<SelectedLargeMenu, Action>();

        #region SmallMenuSpecimen
        /// <summary>
        /// 一般検体登録ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 一般検体登録画面表示します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSpecimenRegistration_Click(object value)
        {
            this.dicSelectedRecent[this.nowSelectedMenu] = this.showSpecimenRegistration; // 直近選択記録
            this.showSpecimenRegistration();
        }

        /// <summary>
        /// STAT検体登録ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// STAT検体登録画面表示します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSpecimenStatRegistration_Click(object value)
        {
            this.dicSelectedRecent[this.nowSelectedMenu] = this.showSpecimenStatRegistration; // 直近選択記録
            this.showSpecimenStatRegistration();
        }

        /// <summary>
        /// 検体測定ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 検体測定データ画面表示します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSpecimenResult_Click(object value)
        {
            this.dicSelectedRecent[this.nowSelectedMenu] = this.showSpecimenResult; // 直近選択記録
            this.showSpecimenResult();
        }

        /// <summary>
        /// 検体再検査ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 検体再検査画面表示します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSpecimenRetest_Click(object value)
        {
            this.dicSelectedRecent[this.nowSelectedMenu] = this.showSpecimenRetest; // 直近選択記録
            this.showSpecimenRetest();
        }
        #endregion

        #region SmallMenuAssay
        /// <summary>
        /// 分析項目ステータスボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 分析項目ステータス画面表示します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAssayStatus_Click(Object value)
        {
            this.dicSelectedRecent[this.nowSelectedMenu] = this.showAssay; // 直近選択記録
            this.showAssay();
        }
        #endregion

        #region SmallMenuReagent
        /// <summary>
        /// 試薬準備ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 試薬準備画面を表示します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReagentPreparation_Click(object value)
        {
            this.dicSelectedRecent[this.nowSelectedMenu] = this.showSetReagent; // 直近選択記録
            this.showSetReagent();
        }
        #endregion

        #region SmallMenuCalibration
        /// <summary>
        /// キャリブレータ登録ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// キャリブレータ登録画面表示します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalibRegistration_Click(object value)
        {
            this.dicSelectedRecent[this.nowSelectedMenu] = this.showCalibRegistration; // 直近選択記録
            this.showCalibRegistration();
        }

        /// <summary>
        /// キャリブレータステータスボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// キャリブレータステータス画面表示します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalibrationStatus_Click(object value)
        {
            this.dicSelectedRecent[this.nowSelectedMenu] = this.showCalibStatus; // 直近選択記録
            this.showCalibStatus();
        }

        /// <summary>
        /// キャリブレータ解析ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// キャリブレータ解析画面表示します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalibAnalysis_Click(object value)
        {
            this.dicSelectedRecent[this.nowSelectedMenu] = this.showCalibAnalysis; // 直近選択記録
            this.showCalibAnalysis();
        }

        /// <summary>
        /// キャリブレータ測定結果ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// キャリブレータ測定結果画面表示します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalibResult_Click(object value)
        {
            this.dicSelectedRecent[this.nowSelectedMenu] = this.showCalibResult; // 直近選択記録
            this.showCalibResult();
        }
        #endregion

        #region SmallMenuControl
        /// <summary>
        /// 精度管理検体登録ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 精度管理検体登録画面表示します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnControlRegist_Click(object value)
        {
            this.dicSelectedRecent[this.nowSelectedMenu] = this.showControlRegistration; // 直近選択記録
            this.showControlRegistration();
        }

        /// <summary>
        /// 精度管理検体精度管理ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 精度管理検体精度管理画面表示します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnControlQC_Click(object value)
        {
            this.dicSelectedRecent[this.nowSelectedMenu] = this.showQualityControl; // 直近選択記録
            this.showQualityControl();
        }

        /// <summary>
        /// 精度管理検体測定履歴データボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 精度管理検体測定履歴データ画面表示します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnControlResult_Click(object value)
        {
            this.dicSelectedRecent[this.nowSelectedMenu] = this.showControlResult; // 直近選択記録
            this.showControlResult();
        }
        #endregion

        #region SmallMenuOption

        #endregion

        #region SmallMenuSystem
        /// <summary>
        /// システム構成ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// システム構成画面表示します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSystemStatus_Click(object value)
        {
            this.dicSelectedRecent[this.nowSelectedMenu] = this.showSystem; // 直近選択記録
            this.showSystem();
        }

        /// <summary>
        /// システム分析項目ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// システム分析項目画面表示します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSystemAnalytes_Click(object value)
        {
            this.dicSelectedRecent[this.nowSelectedMenu] = this.showAnalytes; // 直近選択記録
            this.showAnalytes();
        }

        /// <summary>
        /// システム履歴ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// システム履歴画面表示します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSystemLog_Click(object value)
        {
            this.dicSelectedRecent[this.nowSelectedMenu] = this.showLog; // 直近選択記録
            this.showLog();
        }

        /// <summary>
        /// システムオプションボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// システムオプション画面表示します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSystemOption_Click(object value)
        {
            this.dicSelectedRecent[this.nowSelectedMenu] = this.showSystemOption; // 直近選択記録
            this.showSystemOption();
        }

        /// <summary>
        /// メンテナンス調整ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// メンテナンス調整画面表示します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSystemAdjustment_Click(object value)
        {
            //メンテナンス画面へのログイン画面を表示する
            DialogResult result;
            using (DlgMaintenanceLogin maintenanceLoginDlg = new DlgMaintenanceLogin())
            {
                result = maintenanceLoginDlg.ShowDialog();
            }
            //キャンセルされた場合は以降の処理を行わない
            if (result == DialogResult.Cancel)
            {
                return;
            }

            //メンテナンス開始処理
            if (this.MeintenanceStartProcess() == false)
            {
                //メンテナンス開始処理が上手くいかなかった場合は画面起動しない
                return;
            }

            //メンテナンス画面の起動
            using (Maintenance.FormMaintenanceMain Maintenance = new Maintenance.FormMaintenanceMain())
            {
                Maintenance.ShowDialog();
            }

            //選択されているモジュール番号をメイン画面の内容に戻す
            Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex = (int)Singleton<PublicMemory>.Instance.moduleIndex;
        }

        /// <summary>
        /// メンテナンス開始ダイアログの表示
        /// </summary>
        /// <remarks>
        /// ラック、スレーブにコンフィグパラメータコマンドの送信を行います。
        /// </remarks>
        /// <returns>True:問合せ成功 False:問合せ失敗</returns>
        private Boolean MeintenanceStartProcess()
        {
            //処理中に画面の操作が行われないよう非活性状態にする
            this.Cursor = Cursors.WaitCursor;
            this.Enabled = false;
            foreach (Form form in this.OwnedForms)
            {
                form.Enabled = false;
            }

            //メンテナンス開始ダイアログを起動
            DialogResult askSuccess;
            using (DlgMaintenanceStart dlgMaintenanceStart = new DlgMaintenanceStart())
            {
                askSuccess = dlgMaintenanceStart.ShowDialog();
            }

            //活性状態に戻す
            foreach (Form form in this.OwnedForms)
            {
                form.Enabled = true;
            }
            this.Enabled = true;
            this.Cursor = Cursors.Default;

            return (askSuccess == DialogResult.OK);
        }

        /// <summary>
        /// ユーザー管理ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ユーザー管理画面表示します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSystemUser_Click(object value)
        {
            this.dicSelectedRecent[this.nowSelectedMenu] = this.showUser; // 直近選択記録
            this.showUser();
        }

        /// <summary>
        /// システムオプションボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// システムオプション画面表示します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSystemModuleOption_Click(object value)
        {
            this.dicSelectedRecent[this.nowSelectedMenu] = this.showSystemModuleOption; // 直近選択記録
            this.showSystemModuleOption();
        }

        #endregion

        /// <summary>
        /// 待機ラベル待機時間経過イベントハンドラ
        /// </summary>
        /// <remarks>
        /// 待機ラベルで、設定された時間が経過された際に非表示化します。
        /// </remarks>
        /// <param name="sender">送信元（不使用）</param>
        /// <param name="e">イベントデータ（不使用）</param>
        private void lblReagentSetupWaitMinutes_TimeOver(object sender, EventArgs e)
        {
            CustomLabel reagentSetupWaitMinutes = sender as CustomLabel;

            if (reagentSetupWaitMinutes != null)
            {
                // 対象のモジュールIndexを取得
                ModuleIndex targetModuleIndex = (ModuleIndex)int.Parse(reagentSetupWaitMinutes.Tag.ToString());

                // 表示されている画面と一致する場合
                if (targetModuleIndex == Singleton<PublicMemory>.Instance.moduleIndex)
                {
                    // ラベル表示をOFF
                    reagentSetupWaitMinutes.Visible = false;
                }

                // ラベル表示フラグをOFF
                this.lblReagentSetupWaitMinutesList[targetModuleIndex].IsVisibleLabel = false;

                // 試薬準備開始コマンドが応答されるまで（最大17.7分）の間、タイマーラベルが表示され、
                // タイムアウト、準備開始コマンド応答により釦が点滅します。
                // 試薬準備画面が開いていた場合、ボタンは点滅しない
                if (Singleton<FormSetReagent>.Instance.IsVisible == false)
                {
                    // 試薬釦点滅開始
                    this.btnMenuLargeReagent.BlinkStart(Oelco.CarisX.Properties.Resources.Image_ReagentOFF
                                                      , Oelco.CarisX.Properties.Resources.Image_ReagentON
                                                      , 500
                                                      , 1);
                }

                // 試薬準備画面へ通知
                Singleton<FormSetReagent>.Instance.WaitDispenceComplete(targetModuleIndex);
            }
        }

        #endregion

        /// <summary>
        /// ナビゲーションの戻るボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ナビゲーションの戻る処理を実行します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNaviBack_Click(object sender, EventArgs e)
        {
            // 編集中かどうか、また編集中でも画面遷移するかどうか
            if (CarisXSubFunction.IsEditsMessageShow())
            {
                // 操作履歴登録
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { CarisX.Properties.Resources.STRING_LOG_MSG_023 });

                // 履歴処理 戻る、実行
                Singleton<HistoryManager>.Instance.MovePrev();
                Singleton<HistoryManager>.Instance.ExecCurrent();

                // 編集中フラグOFF
                FormChildBase.IsEdit = false;
            }
        }

        /// <summary>
        /// ナビゲーションの進むボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ナビゲーションの進む処理を実行します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNaviForward_Click(object sender, EventArgs e)
        {
            // 編集中かどうか、また編集中でも画面遷移するかどうか
            if (CarisXSubFunction.IsEditsMessageShow())
            {
                // 操作履歴登録
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { CarisX.Properties.Resources.STRING_LOG_MSG_024 });

                // 履歴処理 進む、実行
                Singleton<HistoryManager>.Instance.MoveNext();
                Singleton<HistoryManager>.Instance.ExecCurrent();

                // 編集中フラグOFF
                FormChildBase.IsEdit = false;
            }
        }


        // 通信通知・イベント通知を行うタイマ処理
        /// <summary>
        /// 通知管理タイマ
        /// </summary>
        /// <remarks>
        /// 通知管理によるイベントを発生させます。
        /// 動作周期は100msです。
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyWatchTimer_Tick(object sender, EventArgs e)
        {

#if !NOT_USE_IOT
            // シャットダウンせずに日付変更にアクションを追加
            if (this.deviceCurrentDate.Date != DateTime.Today.Date)
            {
                // 履歴ファイルのバックアップ
                // ネットワーク障害などによるIoT送信失敗時、バッチ再送信
                CarisXSubFunction.SendIoTSystemFiles(this.deviceCurrentDate);
                CarisXSubFunction.SendIoTSystemFilesBatch();

                // レシピ番号をリセット
                Singleton<ReceiptNo>.Instance.ResetNumber();

                // 日付更新
                this.deviceCurrentDate = DateTime.Now;
            }
#endif

            // 通知イベント発生
            Singleton<NotifyManager>.Instance.ExecuteAllQueue();
        }

        /// <summary>
        /// 通信管理通知タイマ
        /// </summary>
        /// <remarks>
        /// 通信管理による通知を発生させます。
        /// 動作周期は1000msです。
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void commWatchTimer_Tick(object sender, EventArgs e)
        {
            this.commEventExecute();
        }

        /// <summary>
        /// 通信イベント実行
        /// </summary>
        /// <remarks>
        /// 通信イベント実行します
        /// </remarks>
        private void commEventExecute()
        {
            // 受信キュー移動（送受信スレッドの保持するキュー内容をCommManagerに移動させる）
            Singleton<CarisXCommManager>.Instance.TransferReceiveQueueToMessageManager();

            // イベント発生
            Singleton<CarisXCommMessageManager>.Instance.RaiseEvent();
        }

        /// <summary>
        /// 画面表示イベント
        /// </summary>
        /// <remarks>
        /// 画面表示イベント実行します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMainFrame_Shown(object sender, EventArgs e)
        {
            foreach (ModuleIndex moduleIndex in Enum.GetValues(typeof(ModuleIndex)))
            {
                this.btnSlaveList[moduleIndex].BtnSlave.CurrentState = false;
                this.btnSlaveList[moduleIndex].Enabled = false;
            }
            //モジュール１を選択状態にする
            this.btnSlaveList[ModuleIndex.Module1].BtnSlave.CurrentState = true;

            btnSlaveTotal.Enabled = false;
            Singleton<PublicMemory>.Instance.moduleIndex = ModuleIndex.Module1;
            Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex = (int)Singleton<PublicMemory>.Instance.moduleIndex;

            rectangle = new Rectangle(this.Location, this.Size);
        }

        /// <summary>
        /// 分析方式種別変更
        /// </summary>
        /// <remarks>
        /// システムパラメータ設定の分析方式種別が変化した際に呼び出されます。
        /// 検体登録DBの初期化や、状態のクリアが行われます。
        /// </remarks>
        /// <param name="value">分析方式種別</param>
        private void onAssayModeKindChanged(Object value)
        {
            AssayModeParameter.AssayModeKind kind = (AssayModeParameter.AssayModeKind)value;

            // 検体登録クリア
            Singleton<SpecimenGeneralDB>.Instance.DeleteAll();
            Singleton<SpecimenGeneralDB>.Instance.CommitSampleInfo();
            // 一般検体登録画面グリッド再生成
            Singleton<FormSpecimenRegistration>.Instance.InitializeGridView();
        }

        private void onCalibrtionModeKindChanged(Object value)
        {
            CalibrationModeParameter.CalibrationModeKind kind = (CalibrationModeParameter.CalibrationModeKind)value;
            Singleton<CalibrationModeParameter>.Instance.CalibrationMode = kind;
        }

        /// <summary>
        /// ユーザーレベル変更イベントハンドラ
        /// </summary>
        /// <remarks>
        /// ユーザーレベルが変化したときに呼び出されます。
        /// FormBase継承クラスインスタンス全体に対して通知を行います。
        /// </remarks>
        /// <param name="value">ユーザーレベル</param>
        private void onUserLevelChanged(Object value)
        {
            // 現在生成されているFormBase継承クラスインスタンス全体に対してsetUserが呼び出される。
            // TODO:起動時呼び出すと落ちるので対応する
            //   FormBase.UserLevelChanged();
            FormBase.UserLevelChanged();

            this.chkRackNoUse.Visible = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.DebugControlVisibled)
                && Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.DebugMode;
            this.cmbAssayModuleNo.Visible = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.DebugControlVisibled)
                && Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.DebugMode;
            if (!Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.DebugControlVisibled))
            {
                //developerじゃない場合は値を初期化
                this.chkRackNoUse.Checked = false;
                this.cmbAssayModuleNo.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// ラック配置差異イベント
        /// </summary>
        /// <remarks>
        /// 架設ラックが取られた場合、そのラックIDで再検査が登録されていれば削除します
        /// </remarks>
        /// <param name="value"></param>
        private void onBeforeRackLaneContentChange(Object value)
        {
            LaneContentChangeInfo info = value as LaneContentChangeInfo;
            // ラックの設置状況変化イベント
            // 架設ラックが取られた場合、そのラックIDで再検査が登録されていれば削除をおこなう。
            // このイベントは、ラックID読み取り以後動作する。
            if (info.Status == LaneContentChangeInfo.ChangingStatus.Remove)
            {
                //ラック設置状況変化:{0}が{1}されました
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("Rack Setting status change: {0} has been {1}", info.RackId, info.Status.ToString()));
                //var searchedData = from v in Singleton<SpecimenReMeasureDB>.Instance.GetDispData( true )
                //                   where v.RackID.DispPreCharString == info.RackId.DispPreCharString
                //                   select v;
                //                foreach ( var data in searchedData )
                //                {
                //#warning 一時的削除（ラック架設状況変化通知修正したら戻す）
                //                    //Singleton<SpecimenReMeasureDB>.Instance.DeleteData(data.IndividuallyNo);
                //                }
                Singleton<SpecimenReMeasureDB>.Instance.CommitSampleReMeasureInfo();
            }

        }

        /// <summary>
        /// イベントハンドラ関連付け
        /// </summary>
        /// <remarks>
        /// イベントハンドラ関連付けを行います
        /// </remarks>
        private void bindEventHandler()
        {

            // 通信イベント登録
            Singleton<CarisXCommMessageManager>.Instance.ReceiveCommCommand += this.onCommand;

            //  初期シーケンス進捗通知イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.InitializeProgress, this.initializeProgressNotify);

            //  初期シーケンス中断通知イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.InitializePause, this.initializePause);

            //  初期シーケンス再開通知イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.InitializeStart, this.initializeReStart);

            //  試薬項目釦点滅イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.BlinkReagentButton, this.blinkReagentButton);

            //  エラー釦点滅イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.BlinkErrorButton, this.blinkErrorButton);

            // システムステータス変更イベント通知
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SystemStatusChanged, this.onSystemStatusChanged);

            // 日替わり発生通知
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.DateChanged, this.onDateChange);

            // 分析方式変更通知
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.AssayModeKindChanged, this.onAssayModeKindChanged);

            // 試薬準備確認応答イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ReagentPrepareCheckResponse, this.reagentPrepareCheckResponse);

            // 汎用準備開始応答イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.CommonPrepareStartResponse, this.commonPrepareStartResponse);

            // 試薬準備完了応答イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ReagentPrepareCompleteResponse, this.reagentPrepareCompleteResponse);

            // 試薬残量変更確認応答イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ChangeReagentRemainResponse, this.changeReagentRemainResponse);
            
            // 試薬交換開始イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.StartReagentTimer, this.startReagentTimer);
            
            // 試薬交換中断イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.CancelReagentTimer, this.cancelReagentTimer);

            // 汎用残量変更イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ChangeCommonRemainResponse, this.changeCommonRemainResponse);

            // ユーザレベル変更通知イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UserLevelChanged, this.onUserLevelChanged);

            // ラック配置差異イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.BeforeRackLaneChange, this.onBeforeRackLaneContentChange);

            // 消耗品確認応答イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SupplieResponced, this.showDialogSupplie);

            // バージョン通知(スレーブ用)イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SlaveVersion, this.slaveVersionCommand);

            // バージョン通知(ラック搬送用)イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.RackTransferVersion, this.rackTransferVersionCommand);

            // 自動立ち上げ開始(スレーブ用)イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.AutoStartupStartModule, this.autoStartupStartSlave);

            // 自動立ち上げ開始(ラック搬送用)イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.AutoStartupStartRack, this.autoStartupStartRack);

            // 警告灯・ブザー設定イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SendCaution, this.sendCaution);

            // 警告灯・ブザーキャンセルイベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SendBuzzerCancel, this.sendBuzzerCancel);

            // システム終了イベント登録　
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SystemEnd, this.SystemEnd);

            // ホストWS受信イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.HostWorkSheet, this.workSheetFromHost);

            // ホスト使用有無イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UseOfHost, this.setHostStatus);

            // 終了シーケンス(スレーブ用)イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ShutDownExecuteSlave, this.endSequenceCompleteSlave);

            // 終了シーケンス(ラック搬送用)イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ShutDownExecuteRack, this.endSequenceCompleteRack);

            // 温度問合せタイマ設定イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SetAskTemperatureTimer, this.onSetAskTemperatureTimer);

            // 試薬保冷庫移動確認応答イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ReagentCoolerMoveResponse, this.reagentCoolerMoveResponse);

            // プロトコルバージョン通知
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ProtocolVersion, this.protocolVersionCommand);

            // 校准方式変更通知
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.CalibrationModeKindChanged, this.onCalibrtionModeKindChanged);

            //メインフォームのスモールメニューSpecimen
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SmallMenuSpecimenRegistration, this.btnSpecimenRegistration_Click);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SmallMenuSpecimenStatRegistration, this.btnSpecimenStatRegistration_Click);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SmallMenuSpecimenResult, this.btnSpecimenResult_Click);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SmallMenuSpecimenRetest, this.btnSpecimenRetest_Click);

            //メインフォームのスモールメニューAssay
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SmallMenuAssayStatus, this.btnAssayStatus_Click);
            
            //メインフォームのスモールメニューReagent
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SmallMenuReagentPreparation, this.btnReagentPreparation_Click);
            
            //メインフォームのスモールメニューCalibration
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SmallMenuCalibRegistration, this.btnCalibRegistration_Click);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SmallMenuCalibStatus, this.btnCalibrationStatus_Click);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SmallMenuCalibAnalysis, this.btnCalibAnalysis_Click);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SmallMenuCalibResult, this.btnCalibResult_Click);
            
            //メインフォームのスモールメニューControl
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SmallMenuControlRegistration, this.btnControlRegist_Click);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SmallMenuControlQC, this.btnControlQC_Click);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SmallMenuControlResult, this.btnControlResult_Click);
            
            //メインフォームのスモールメニューOption
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SmallMenuSystemModuleOption, this.btnSystemModuleOption_Click);
            
            //メインフォームのスモールメニューSystem
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SmallMenuSystemStatus, this.btnSystemStatus_Click);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SmallMenuSystemAnalytes, this.btnSystemAnalytes_Click);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SmallMenuSystemLog, this.btnSystemLog_Click);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SmallMenuSystemOption, this.btnSystemOption_Click);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SmallMenuSystemAdjustment, this.btnSystemAdjustment_Click);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SmallMenuSystemUser, this.btnSystemUser_Click);
            
            // 試薬設置ガイダンス表示通知
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ShowReagentSetGuidance, this.showReagentSetGuidance);
            
            // システムステータスアイコン設定通知
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SetSystemStatusIcon, this.setSystemStatusIcon);
            
            // システムステータス変更通知
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ChangeAbortAssay, this.changeAbortAssay);
            
            // リアルタイム測定結果ファイル出力通知
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.RealtimeOutputFileData, this.saveMeasureResultFile);
            
            // リアルタイム印刷通知
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.RealtimePrintData, this.realtimePrint);

            // ラック分析開始コマンド送信通知
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.RackTransferStartAssay, this.startRackAssay);

            // 2020-02-27 CarisX IoT Add [START]
#if !NOT_USE_IOT
            // IoT障害情報コマンド送信通知
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SendErrorToIoT, this.sendIoTErrorCommand);
#endif
            // 2020-02-27 CarisX IoT Add [END]
        }

        /// <summary>
        /// ホスト使用有無イベント
        /// </summary>
        /// <remarks>
        /// ホスト使用有無イベント実行します
        /// </remarks>
        /// <param name="obj"></param>
        private void setHostStatus(Object obj)
        {
            Boolean use = (Boolean)obj;
            if (use)
            {
                this.pnlHostOnlineStatus.Appearance.ImageBackground = Properties.Resources.Image_Online;
                this.lblHostOnlineStatus.Text = Properties.Resources.STRING_MAIN_FRAME_036;

            }
            else
            {
                this.pnlHostOnlineStatus.Appearance.ImageBackground = Properties.Resources.Image_OffLine;
                this.lblHostOnlineStatus.Text = Properties.Resources.STRING_MAIN_FRAME_008;
            }
        }

        /// <summary>
        /// スレーブバージョン通知イベント
        /// </summary>
        /// <remarks>
        /// スレーブバージョンをシステムオプション画面に設定します
        /// </remarks>
        /// <param name="value"></param>
        private void slaveVersionCommand(Object value)
        {
            // スレーブバージョン設定
            if (value is SlaveCommCommand_0511)
            {
                SlaveCommCommand_0511 cmd0511 = (SlaveCommCommand_0511)value;
                switch (CarisXSubFunction.MachineCodeToRackModuleIndex((MachineCode)cmd0511.CommNo))
                {
                    case (int)RackModuleIndex.Module1:
                        Singleton<FormSystemOption>.Instance.Slave1VersionText = cmd0511.Version;
                        break;
                    case (int)RackModuleIndex.Module2:
                        Singleton<FormSystemOption>.Instance.Slave2VersionText = cmd0511.Version;
                        break;
                    case (int)RackModuleIndex.Module3:
                        Singleton<FormSystemOption>.Instance.Slave3VersionText = cmd0511.Version;
                        break;
                    case (int)RackModuleIndex.Module4:
                        Singleton<FormSystemOption>.Instance.Slave4VersionText = cmd0511.Version;
                        break;
                }
            }
            else
            {
                Singleton<FormSystemOption>.Instance.Slave1VersionText = (String)value;
                Singleton<FormSystemOption>.Instance.Slave2VersionText = (String)value;
                Singleton<FormSystemOption>.Instance.Slave3VersionText = (String)value;
                Singleton<FormSystemOption>.Instance.Slave4VersionText = (String)value;
            }
        }

        /// <summary>
        /// ラック搬送バージョン通知イベント
        /// </summary>
        /// <remarks>
        /// ラック搬送バージョンをシステムオプション画面に設定します
        /// </remarks>
        /// <param name="value"></param>
        private void rackTransferVersionCommand(Object value)
        {
            // ラック搬送バージョン設定
            if (value is RackTransferCommCommand_0111)
            {
                RackTransferCommCommand_0111 cmd0111 = (RackTransferCommCommand_0111)value;
                Singleton<FormSystemOption>.Instance.RackTransferVersionText = cmd0111.Version;
            }
            else
            {
                Singleton<FormSystemOption>.Instance.RackTransferVersionText = (String)value;
            }
        }

        private void protocolVersionCommand(object value)
        {
            String protocolVersion = (String)value;
            Singleton<FormSystemOption>.Instance.ProtocolVersionText = protocolVersion;
            Singleton<FormSystemModuleOption>.Instance.ProtocolVersionText = protocolVersion;
        }

        /// <summary>
        /// システム状態変化イベントハンドラ
        /// </summary>
        /// <remarks>
        /// システムのステータスに変化があった時呼び出されます。
        /// </remarks>
        /// <param name="parameter"></param>
        private void onSystemStatusChanged(Object parameter)
        {
            if (Singleton<SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId(Singleton<PublicMemory>.Instance.moduleIndex)] == SystemStatusKind.ReagentExchange)
            {
                // 試薬交換中は分析関連のボタンを非活性
                this.lblAssayStart.Enabled = false;
                this.btnAssayStart.Enabled = false;
                this.btnAssayPause.Enabled = false;
                this.lblAssayPause.Enabled = false;
                this.btnAssayAbort.Enabled = false;
                this.lblAssayAbort.Enabled = false;
                this.btnHelp.Enabled = false;
                this.btnExit.Enabled = false;
            }
            else
            {
                // トータルボタンの活性状態
                this.btnSlaveTotal.Enabled = false;
                foreach (ModuleIndex moduleIndex in Enum.GetValues(typeof(ModuleIndex)))
                {
                    // モジュールIDを取得
                    Int32 moduleId = CarisXSubFunction.ModuleIndexToModuleId(moduleIndex);

                    // 活性状態を切り替え（未接続時は非活性）
                    this.btnSlaveList[moduleIndex].Enabled = (Singleton<SystemStatus>.Instance.ModuleStatus[moduleId] != SystemStatusKind.NoLink);

                    // スレーブ１～４のいずれかが起動している場合のみTotalを活性化
                    if (this.btnSlaveList[moduleIndex].Enabled == true)
                    {
                        this.btnSlaveTotal.Enabled = true;
                    }
                }

                switch (Singleton<SystemStatus>.Instance.Status)
                {
                    case SystemStatusKind.SamplingPause:
                        //何もしない
                        break;
                    default:
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                            , CarisXLogInfoBaseExtention.Empty, "set flgManualAssayPause = false");

                        //sampling pause以外のステータスに変更された時、フラグをリセットする
                        flgManualAssayPause = false;
                        break;
                }

                switch (Singleton<SystemStatus>.Instance.Status)
                {
                    case SystemStatusKind.NoLink:
                        this.lblAssayStart.Enabled = false;
                        this.btnAssayStart.Enabled = false;
                        this.btnAssayPause.Enabled = false;
                        this.lblAssayPause.Enabled = false;
                        this.btnAssayAbort.Enabled = false;
                        this.lblAssayAbort.Enabled = false;
                        this.btnHelp.Enabled = true;
                        this.btnExit.Enabled = true;

                        // スレーブバージョン表示
                        Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SlaveVersion, Properties.Resources.STRING_SYSTEMOPTION_0019);
                        Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.RackTransferVersion, Properties.Resources.STRING_SYSTEMOPTION_0019);

                        break;
                    case SystemStatusKind.Standby:
                        // 分析ステータスDB内容のクリアを行う。
                        Singleton<InProcessSampleInfoManager>.Instance.Clear();
                        //ラックデータ削除
                        Singleton<RackInfoManager>.Instance.Clear();
                        //キャリブレータ情報削除
                        Singleton<CalibratorInfoManager>.Instance.Clear();
                        RealtimeDataAgent.LoadAssayData();

                        this.lblAssayStart.Enabled = true;
                        this.btnAssayStart.Enabled = true;
                        this.btnAssayPause.Enabled = false;
                        this.lblAssayPause.Enabled = false;
                        this.btnAssayAbort.Enabled = false;
                        this.lblAssayAbort.Enabled = false;
                        this.btnHelp.Enabled = true;
                        this.btnExit.Enabled = true;

                        // 試薬交換用オブジェクト生成
                        NotifyObjectSetReagent notifyObjectForCancelReagentTimer = new NotifyObjectSetReagent(CarisXConst.ALL_MODULEID, null);

                        // 画面通知
                        Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)(NotifyKind.CancelReagentTimer), notifyObjectForCancelReagentTimer);

                        // ログ出力
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                        CarisXLogInfoBaseExtention.Empty, "SystemStatusKind.Standby to ReagentChangeIsAllowed");

                        // 試薬交換許可通知
                        Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ReagentChangeIsAllowed, null);
                        break;

                    case SystemStatusKind.WaitSlaveResponce:
                        this.lblAssayStart.Enabled = false;
                        this.btnAssayStart.Enabled = false;
                        this.btnAssayPause.Enabled = false;
                        this.lblAssayPause.Enabled = false;
                        this.btnAssayAbort.Enabled = true;
                        this.lblAssayAbort.Enabled = true;
                        this.btnHelp.Enabled = false;
                        this.btnExit.Enabled = false;

                        break;

                    case SystemStatusKind.Assay:
                        this.lblAssayStart.Enabled = false;
                        this.btnAssayStart.Enabled = false;
                        this.btnAssayPause.Enabled = true;
                        this.lblAssayPause.Enabled = true;
                        this.btnAssayAbort.Enabled = true;
                        this.lblAssayAbort.Enabled = true;
                        this.btnHelp.Enabled = true;
                        this.btnExit.Enabled = false;

                        break;

                    case SystemStatusKind.ToEndAssay:
                        this.lblAssayStart.Enabled = false;
                        this.btnAssayStart.Enabled = false;
                        this.btnAssayPause.Enabled = false;
                        this.lblAssayPause.Enabled = false;
                        this.btnAssayAbort.Enabled = false;
                        this.lblAssayAbort.Enabled = false;
                        this.btnHelp.Enabled = true;
                        this.btnExit.Enabled = false;

                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                                CarisXLogInfoBaseExtention.Empty, "SystemStatusKind.ToEndAssay to ReagentChangeIsRefused");
                        Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ReagentChangeIsRefused, null);
                        break;

                    case SystemStatusKind.SamplingPause:

                        if (flgManualAssayPause)
                        {
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                                , CarisXLogInfoBaseExtention.Empty, "Assay restart is possible");

                            //手動でサンプリング停止した場合は手動で再開できる
                            this.lblAssayStart.Enabled = true;
                            this.btnAssayStart.Enabled = true;
                        }
                        else
                        {
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                                , CarisXLogInfoBaseExtention.Empty, "Assay restart is impossible");

                            //手動以外でサンプリング停止した場合は手動で再開できない
                            this.lblAssayStart.Enabled = false;
                            this.btnAssayStart.Enabled = false;
                        }
                        this.btnAssayPause.Enabled = false;
                        this.lblAssayPause.Enabled = false;
                        this.btnAssayAbort.Enabled = true;
                        this.lblAssayAbort.Enabled = true;
                        this.btnHelp.Enabled = true;
                        this.btnExit.Enabled = false;

                        break;

                    case SystemStatusKind.Shutdown:
                        this.lblAssayStart.Enabled = false;
                        this.btnAssayStart.Enabled = false;
                        this.btnAssayPause.Enabled = false;
                        this.lblAssayPause.Enabled = false;
                        this.btnAssayAbort.Enabled = false;
                        this.lblAssayAbort.Enabled = false;
                        this.btnHelp.Enabled = false;
                        this.btnExit.Enabled = false;

                        break;
                    default:
                        break;
                }
            }


            switch (Singleton<SystemStatus>.Instance.Status)
            {
                case SystemStatusKind.NoLink:
                    this.lblAnalyzerStatus.Text = Properties.Resources.STRING_MAIN_FRAME_033;
                    break;
                case SystemStatusKind.Standby:
                    this.lblAnalyzerStatus.Text = Properties.Resources.STRING_MAIN_FRAME_032;
                    break;
                case SystemStatusKind.Assay:
                    this.lblAnalyzerStatus.Text = Properties.Resources.STRING_MAIN_FRAME_034;
                    break;
                case SystemStatusKind.SamplingPause:
                    this.lblAnalyzerStatus.Text = Properties.Resources.STRING_MAIN_FRAME_035;
                    break;
                case SystemStatusKind.Shutdown:
                    this.lblAnalyzerStatus.Text = Properties.Resources.STRING_MAIN_FRAME_037;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 再ログイン確認
        /// </summary>
        /// <remarks>
        /// 終了メニュー画面表示し、再ログインの実行確認します
        /// </remarks>
        /// <returns></returns>
        private Boolean confirmRetryLogin()
        {
            Boolean needRetry = false;
            using (DlgShutdown dlg = this.createShutdownDlg())
            {
                dlg.EnableCancel = false;
                dlg.EnableAutomaticStartupWait = false;
                dlg.EnableShutdown = false;

                dlg.ShowDialog();
                // needRetry = dlg.SelectResult == DlgShutdown.SelectedMethod.LogOut;
                if (dlg.SelectResult == DlgShutdown.SelectedMethod.AllShutDown
                    || dlg.SelectResult == DlgShutdown.SelectedMethod.LogOut)
                {
                    needRetry = true;
                }
            }
            return needRetry;
        }

        /// <summary>
        /// ログイン画面表示
        /// </summary>
        /// <remarks>
        /// ログイン画面表示します
        /// </remarks>
        private void showLogin()
        {
            // ログイン画面表示
            Singleton<CarisXUserLevelManager>.Instance.SyncUserInfoDB(); // ユーザ情報更新
            Boolean isComplate = false;
            while (!isComplate)
            {
                using (DlgLogin loginDlg = new DlgLogin())
                {
                    DialogResult result = loginDlg.ShowDialog();
                    // ログインされた場合は処理なし、キャンセルされた場合は終了メニュー表示
                    if (result == System.Windows.Forms.DialogResult.Cancel)
                    {
                        Boolean needRetry = this.confirmRetryLogin();
                        isComplate = !needRetry;
                    }
                    else
                    {
                        isComplate = true;
                    }

                    // 操作履歴登録     
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
                        , new String[] { String.Format(CarisX.Properties.Resources.STRING_LOG_MSG_047 + "{0} ({1})", Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserLevel) });
                }
            }
        }
        /// <summary>
        /// 初期シーケンス起動
        /// </summary>
        /// <remarks>
        /// 初期シーケンスを起動します
        /// </remarks>
        private void startUp()
        {
            // 日替わりチェック(UIPackageが記録していた前回アクセス日付を元にする)
            Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.DateChanged, null); // 日替わり処理

            // 初期シーケンス起動
            foreach (Int32 moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                if (Singleton<CarisXSequenceHelperManager>.Instance.Slave.ContainsKey(moduleindex))
                {
                    Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleindex].InitializeSequenceModule(
                        InitializeSequencePattern.Module | InitializeSequencePattern.StartsBeforeUser);
                }
            }
            Singleton<CarisXSequenceHelperManager>.Instance.RackTransfer.InitializeSequenceRackTransfer(
                InitializeSequencePattern.RackTransfer | InitializeSequencePattern.StartsBeforeUser);

            if (this.dlgStartup == null)
            {
                this.dlgStartup = new DlgInitialize(progressInfos);
                this.dlgStartup.ShowDialog();
            }

            // ログイン画面表示
            this.showLogin();

            Singleton<Oelco.CarisX.Common.MaintenanceJournalInfoManager>.Instance.SetMaintenanceJournalType(MaintenanceJournalType.User);
            // メンテナンス日誌開くかチェックします
            if (Singleton<Oelco.CarisX.Common.MaintenanceJournalInfoManager>.Instance.IsShow())
            {
                // メンテナンス日誌（ユーザー用）を表示
                Oelco.CarisX.GUI.DlgMaintenanceList dlgMaintenanceList = new Oelco.CarisX.GUI.DlgMaintenanceList(MaintenanceJournalType.User);
                dlgMaintenanceList.ShowDialog();
            }

            // 消耗品警告ダイアログ表示
            this.showSupplieWarning();
        }

        /// <summary>
        /// 消耗品警告ダイアログ表示
        /// </summary>
        /// <remarks>
        /// 消耗品警告ダイアログ表示します
        /// </remarks>
        private void showSupplieWarning()
        {
            List<String> warningList = new List<string>();
            foreach (Int32 moduleIndex in Enum.GetValues(typeof(ModuleIndex)))
            {
                Int32 moduleId = CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)moduleIndex);

                if (!(Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param.SlaveList.Count > moduleIndex))
                {
                    //SlaveListに存在しないインデックスの場合は処理しない
                    continue;
                }

                // 消耗品警告Dlg表示
                if (Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param.SlaveList[moduleIndex].DiluentDispensingSyringePackin.IsOver)
                {
                    // 希釈液分注シリンジパッキン
                    warningList.Add(String.Format(Properties.Resources.STRING_SUPPLIE_WARNING_001, moduleId));
                }
                if (Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param.SlaveList[moduleIndex].OutDrainPump.IsOver)
                {
                    // 体外廃液ポンプ
                    warningList.Add(String.Format(Properties.Resources.STRING_SUPPLIE_WARNING_002, moduleId));
                }
                if (Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param.SlaveList[moduleIndex].OutDrainPumpTube.IsOver)
                {
                    // 体外廃液ポンプチューブ
                    warningList.Add(String.Format(Properties.Resources.STRING_SUPPLIE_WARNING_003, moduleId));
                }
                if (Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param.SlaveList[moduleIndex].PreTriggerDispensingSyringePackin.IsOver)
                {
                    // プレトリガ分注シリンジパッキン
                    warningList.Add(String.Format(Properties.Resources.STRING_SUPPLIE_WARNING_004, moduleId));
                }
                if (Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param.SlaveList[moduleIndex].R1DispensingSyringePackin.IsOver)
                {
                    // R1分注シリンジパッキン
                    warningList.Add(String.Format(Properties.Resources.STRING_SUPPLIE_WARNING_005, moduleId));
                }
                if (Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param.SlaveList[moduleIndex].R2DispensingSyringePackin.IsOver)
                {
                    // R2分注シリンジパッキン
                    warningList.Add(String.Format(Properties.Resources.STRING_SUPPLIE_WARNING_006, moduleId));
                }
                if (Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param.SlaveList[moduleIndex].ReagentDispensingSyringePackin.IsOver)
                {
                    // 試薬分注洗浄液シリンジパッキン
                    warningList.Add(String.Format(Properties.Resources.STRING_SUPPLIE_WARNING_007, moduleId));
                }
                if (Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param.SlaveList[moduleIndex].SampleDispensingSyringePackin.IsOver)
                {
                    // 検体分注シリンジパッキン
                    warningList.Add(String.Format(Properties.Resources.STRING_SUPPLIE_WARNING_008, moduleId));
                }
                if (Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param.SlaveList[moduleIndex].TriggerDispensingSyringePackin.IsOver)
                {
                    // トリガ分注シリンジパッキン
                    warningList.Add(String.Format(Properties.Resources.STRING_SUPPLIE_WARNING_009, moduleId));
                }
                if (Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param.SlaveList[moduleIndex].Wash1DispensingSyringePackin.IsOver)
                {
                    // 洗浄1シリンジパッキン
                    warningList.Add(String.Format(Properties.Resources.STRING_SUPPLIE_WARNING_010, moduleId));
                }
                if (Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param.SlaveList[moduleIndex].Wash2DispensingSyringePackin.IsOver)
                {
                    // 洗浄2シリンジパッキン
                    warningList.Add(String.Format(Properties.Resources.STRING_SUPPLIE_WARNING_011, moduleId));
                }

            }

            //消耗品警告画面表示
            if (warningList.Count > 0)
            {
                DlgSupplieComfirm supplieconf = new DlgSupplieComfirm();
                supplieconf.setPartsList(warningList);
                supplieconf.ShowDialog();
            }

        }

        /// <summary>
        /// フェード表示完了イベント
        /// </summary>
        /// <remarks>
        /// フェード表示完了処理を実行します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMainFrame_OnFadeShown(object sender, EventArgs e)
        {
            //#if DEBUG_CARIS
            //            // マスターカーブコマンド検証

            //            // かり
            //            SlaveCommCommand_0510 com = new SlaveCommCommand_0510();
            //            // 110
            //            com.SetCommandString( "  12  16     188   29149   38987  172359 1183544 2469584       0       0  00       0       0       0       0       0       0       0       00001" );
            //            this.onMasterCurveCommand( com );
            //#endif

            // 初期化要求通知
            // Shown時点のタイミングでダイアログ類の表示を行うと、実際の描画が行われるよりも先にダイアログ類の表示が行われてしまう為、
            // タイマにより行う。
            //Singleton<NotifyManager>.Instance.PushSignalQueue( (Int32)NotifyKind.InitializeRequire, null );
        }

        /// <summary>
        /// 表示完了待機タイマタイムアップイベント
        /// </summary>
        /// <remarks>
        /// 表示完了待機タイマタイムアップ処理実行します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void formShownTimer_Tick(object sender, EventArgs e)
        {
            // Main画面の表示完了（実際に可視状態となるタイミング）が取れない為、
            // Showが完了したタイミングから一定時間待機を行う。
            // この待機を行わない場合、Main画面の表示より先に初期シーケンスのダイアログが表示されてしまう。
            this.splash.Close(); // スプラッシュ終了
            this.splash.Dispose();
            this.splash = null;

            this.Enabled = true;

            // タイマの動作は初回の一度のみ
            this.formShownTimer.Stop();

            // 前回正常終了確認
            if (Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SystemCondition == false)
            {
                // メッセージ表示
                // "確認", "前回、正常に終了していない可能性が有る為、\nチップもしくはカートリッジの登録が消去されている可能性があります。\n再度登録を確認してください。");
                // TODO:Captionが未指定
                DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_028, "", CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.Confirm);
            }
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SystemCondition = false; // 終了時はTrueにして保存する。
                                                                                                  // 正常終了状態保存
            if (!Singleton<ParameterFilePreserve<AppSettings>>.Instance.Save())
            {
                // 失敗時
                System.Diagnostics.Debug.WriteLine(String.Format("AppSettings save failed {0}", Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SavePath));
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                     CarisXLogInfoBaseExtention.Empty, String.Format("AppSettings save failed {0}", Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SavePath));
            }

            // 通信管理初期化

            // スレーブ接続
            // 通信ログファイルパスを設定する(パスは、出力先フォルダ + "\")
            // ※　通信クラス仕様　受信ログ：保存パス+（年月日）+revfile.txt
            // 　　　　　　　　　　送信ログ：保存パス+（年月日）+sendfile.txt
            // 　　　　　　　　　　引数に設定するパスの最後は"\"で終えてください。
            string saveFilePath = CarisXConst.PathLog + @"\";
#if DEBUG_CARIS_OLD_SOCKET
            //Singleton<CarisXCommManager>.Instance.ConnectSlave(Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SlaveCommSettings, saveFilePath);
            Singleton<CarisXCommManager>.Instance.ConnectSlave(Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.ConnectSettings.GetSlaveCommSettings(), saveFilePath);

            // ラック搬送接続
            //Singleton<CarisXCommManager>.Instance.ConnectRackTransfer(Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.RackTransferCommSettings, saveFilePath);
            Singleton<CarisXCommManager>.Instance.ConnectRackTransfer(Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.ConnectSettings.RackTransferCommSettings, saveFilePath);
#else
            Singleton<CarisXCommManager>.Instance.Connect(Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.ConnectSettings.RackTransferCommSettings,
                                                        Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.ConnectSettings.GetSlaveCommSettings(),
                                                        saveFilePath);
#endif //[DEBUG_CARIS_OLD_SOCKET]


            // ホスト接続
            SerialParameter hostCommSetting = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.ConvertSerialParameter();
            if (!Singleton<CarisXCommManager>.Instance.ConnectHost(hostCommSetting))
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                     CarisXLogInfoBaseExtention.Empty, String.Format("HOST通信用failed to COM port connection。 Port:{0}", hostCommSetting.CommPort));
            }

#if !NOT_USE_IOT
            // IoT設定パラメータを取得
            IoTParameter iotParam = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter;

            // IoTへの接続処理確認
            if (iotParam.Enable)
            {
                // ログ出力
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, string.Format("Connect to IoT; Date = {0}", DateTime.Now.ToString()));

                // IoT接続
                Singleton<CarisXCommIoTManager>.Instance.ConnectIoT(iotParam.IoTConnectionStr, iotParam.MachineSerialNumber.ToString());
            }
#endif

            // シーケンスヘルパー管理生成
            Singleton<CarisXSequenceHelperManager>.Instance.Intialize(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected);

            // フラグ初期化
            compSlaveEndSequence = new Boolean[Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected];
            statusSlaveEndSequence = new Boolean[Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected];
            compSlaveAutoStartUpSequence = new Boolean[Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected];
            statusSlaveAutoStartUpSequence = new Boolean[Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected];
            for (int i = 0; i < Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected; i++)
            {
                compSlaveEndSequence[i] = false;
                statusSlaveEndSequence[i] = false;
                compSlaveAutoStartUpSequence[i] = false;
                statusSlaveAutoStartUpSequence[i] = false;
            }

            Singleton<PublicMemory>.Instance.lastRackMovePosition.Clear();
            Singleton<PublicMemory>.Instance.lastRackMovePosition.Add(0, 0);

            // 初期シーケンス起動
            this.startUp();
        }

        /// <summary>
        /// Windwosシャットダウン
        /// </summary>
        /// <remarks>
        /// Windwosシャットダウン実行します
        /// </remarks>
        private void WindowsShutdown()
        {
            // ユーザー特権を有効にするための設定を作成
            System.Management.ConnectionOptions co = new System.Management.ConnectionOptions();
            co.Impersonation = System.Management.ImpersonationLevel.Impersonate;
            co.EnablePrivileges = true;

            // ManagementScopeを作成
            System.Management.ManagementScope sc = new System.Management.ManagementScope("\\ROOT\\CIMV2", co);

            // 接続
            sc.Connect();

            // 管理クエリを生成
            System.Management.ObjectQuery oq = new System.Management.ObjectQuery("select * from Win32_OperatingSystem");

            // 管理クエリを実行
            System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(sc, oq);

            // Shutdownメソッドを呼び出す
            foreach (System.Management.ManagementObject mo in mos.Get())
            {
                //パラメータを指定
                System.Management.ManagementBaseObject inParams = mo.GetMethodParameters("Win32Shutdown");
                inParams["Flags"] = 1 + 4;
                inParams["Reserved"] = 0;

                //Win32Shutdownメソッドを呼び出す
                System.Management.ManagementBaseObject outParams = mo.InvokeMethod("Win32Shutdown", inParams, null);
                mo.Dispose();
            }

            mos.Dispose();
        }

        /// <summary>
        /// システム終了
        /// </summary>
        /// <remarks>
        /// 履歴を出力し、パラメータ保存して画面を終了します
        /// </remarks>
        private void SystemEnd(Object value)
        {
            // 操作履歴：終了実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                , CarisXLogInfoBaseExtention.Empty, new String[] { CarisX.Properties.Resources.STRING_LOG_MSG_020 });

            //初期シーケンスが待機中の場合は終了する
            foreach (Int32 moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                if (Singleton<CarisXSequenceHelperManager>.Instance.Slave.Count > moduleindex)
                {
                    Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleindex].InitializeSequenceAbort();
                }
            }
            Singleton<CarisXSequenceHelperManager>.Instance.RackTransfer.InitializeSequenceAbort();

            // 前回終了フラグをtrueに設定
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SystemCondition = true;

            // ユーザー設定パラメータを保存
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Save();

            this.Close();
        }

        /// <summary>
        /// 自動起動シーケンス完了（スレーブ）
        /// </summary>
        /// <remarks>
        /// （スレーブ）自動起動シーケンスが完了した際に呼び出されます。
        /// 自動起動後、ラックのみ起動した時にモジュール１～４のステータスがNoLink以外になっていると分析が出来てしまう為、
        /// 自動起動シーケンスでシャットダウンに成功している場合はモジュールステータスをNoLinkに変更しておく。
        /// </remarks>
        /// <param name="param"></param>
        private void autoStartupStartSlave(Object param)
        {
            SequenceHelperMessage sequenceHelperMessage = (SequenceHelperMessage)param;

            compSlaveAutoStartUpSequence[sequenceHelperMessage.ModuleIndex] = true;

            statusSlaveAutoStartUpSequence[sequenceHelperMessage.ModuleIndex] = (Boolean)sequenceHelperMessage.MessageParameter;

            // 正常にシャットダウン指示が行えたかチェック
            if (compSlaveAutoStartUpSequence[sequenceHelperMessage.ModuleIndex])
            {
                // モジュールIDを取得
                Int32 moduleId = CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)sequenceHelperMessage.ModuleIndex);

                // モジュールステータスをNoLinkに変更する
                Singleton<SystemStatus>.Instance.setModuleStatus((RackModuleIndex)moduleId, SystemStatusKind.NoLink);

                // 進捗率をリセット
                progressInfos[moduleId].Reset();
            }

            // 自動立ち上げ待機を開始
            this.autoStartupStart();
        }

        /// <summary>
        /// 自動起動シーケンス完了（ラック）
        /// </summary>
        /// <remarks>
        /// （ラック）自動起動シーケンスが完了した際に呼び出されます。
        /// 自動起動後、モジュールのみ起動した時にラックのステータスがNoLink以外になっていると分析が出来てしまう為、
        /// 自動起動シーケンスでシャットダウンに成功している場合はモジュールステータスをNoLinkに変更しておく。
        /// </remarks>
        /// <param name="param"></param>
        private void autoStartupStartRack(Object param)
        {
            compRackAutoStartUpSequence = true;
            statusRackAutoStartUpSequence = (Boolean)param;

            // 正常にシャットダウン指示が行えたか確認
            if (statusRackAutoStartUpSequence)
            {
                //モジュールステータスをNoLinkに変更する
                Singleton<SystemStatus>.Instance.setModuleStatus(RackModuleIndex.RackTransfer, SystemStatusKind.NoLink);

                // 進捗率をリセット
                progressInfos[(Int32)RackModuleIndex.RackTransfer].Reset();
            }

            // 自動立ち上げ待機を開始
            this.autoStartupStart();
        }

        /// <summary>
        /// 自動立ち上げ待機
        /// </summary>
        /// <remarks>
        /// 自動立上げの待機処理を行います。
        /// 指示を出したラック・スレーブのすべてからレスポンスが返ってきた時のみ処理を行う
        /// 自動立上げ画面はサブレディコマンドorラックレディコマンドを受信時に閉じる
        /// </remarks>
        /// <param name="value">待機実施フラグ</param>
        private void autoStartupStart()
        {
            if (compSlaveAutoStartUpSequence.All(v => v == true) && compRackAutoStartUpSequence)
            {
                // ラックとスレーブの全てのレスポンスを受信出来ている場合
                if (statusSlaveAutoStartUpSequence.All(v => v == true) && statusRackAutoStartUpSequence)
                {
                    // ラックもスレーブもどちらも正常に終了出来ている場合

                    // 自動立ち上げダイアログ表示
                    using (dlgAutoSetup = new DlgAutoSetup())
                    {
                        if (DialogResult.Cancel == dlgAutoSetup.ShowDialog())
                        {
                            Singleton<SystemStatus>.Instance.setAllModuleStatus(SystemStatusKind.NoLink);

                            // 操作履歴(自動立ち上げ待機をキャンセルしました。)   
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
                                , new String[] { Properties.Resources.STRING_COMMON_013 + Properties.Resources.STRING_LOG_MSG_021 });
                        }
                    }

                    dlgAutoSetup = null;

                    FormBase.AllFormItemEnableStatusRestore();
                }
                else
                {
                    // ラックとスレーブのどちらか又は両方が正常に終了出来ていない場合
                    FormBase.AllFormItemEnableStatusRestore();
                }
            }
        }

        /// <summary>
        /// 自動立ち上げ待機
        /// </summary>
        /// <remarks>
        /// 自動立ち上げ待機します
        /// </remarks>
        private bool autoStartWait()
        {
            FormBase.AllFormItemEnableChange(false);

            //リンス液の残量チェック
            if (!checkRinseRemain())
            {
                FormBase.AllFormItemEnableStatusRestore();
                return false;
            }

            // 正常終了フラグON
            if (!Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SystemCondition)
            {
                Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SystemCondition = true;
                Singleton<ParameterFilePreserve<AppSettings>>.Instance.Save();
            }

            //リンス液がすべてあるので、リンス→シャットダウンの指示を行う
            //ラック搬送（リンスはない）
            if (Singleton<CarisXCommManager>.Instance.GetRackTransferCommStatus() == ConnectionStatus.Online)
            {
                compRackAutoStartUpSequence = false;     //自動起動シーケンスを未完扱いとする
                Singleton<CarisXSequenceHelperManager>.Instance.RackTransfer.AutoStartupRackTransfer();
            }
            else
            {
                compRackAutoStartUpSequence = true;     //自動起動シーケンスを完了扱いとする
                statusRackAutoStartUpSequence = true;   //自動起動シーケンスを正常終了扱いとする
            }

            //モジュール
            foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                {
                    compSlaveAutoStartUpSequence[moduleindex] = false;      //終了シーケンスを未完扱いとする
                    Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleindex].AutoStartup();
                }
                else
                {
                    if (compSlaveAutoStartUpSequence.Length > moduleindex)
                    {
                        compSlaveAutoStartUpSequence[moduleindex] = true;   //終了シーケンスを未完扱いとする
                        statusSlaveAutoStartUpSequence[moduleindex] = true; //終了シーケンスを正常終了扱いとする
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// シャットダウン
        /// </summary>
        /// <remarks>
        /// 自動立ち上げ待機します
        /// </remarks>
        private bool allShutdown()
        {
            FormBase.AllFormItemEnableChange(false);

            //リンス液の残量チェック
            if (!checkRinseRemain())
            {
                FormBase.AllFormItemEnableStatusRestore();
                return false;
            }

            //ラック、スレーブのいずれにもつながっていない場合、画面に復帰する
            if (Singleton<SystemStatus>.Instance.ModuleStatus.All(v => v == SystemStatusKind.NoLink))
            {
                FormBase.AllFormItemEnableStatusRestore();
                return false;
            }

            //リンス液がすべてあるので、リンス→シャットダウンの指示を行う
            if (Singleton<CarisXCommManager>.Instance.GetRackTransferCommStatus() == ConnectionStatus.Online)
            {
                compRackEndSequence = false;     //終了シーケンスを未完扱いとする
                Singleton<CarisXSequenceHelperManager>.Instance.RackTransfer.EndSequenceRackTransfer();
            }
            else
            {
                compRackEndSequence = true;     //終了シーケンスを完了扱いとする
                statusRackEndSequence = true;   //終了シーケンスを正常終了扱いとする
            }


            foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                {
                    compSlaveEndSequence[moduleindex] = false;      //終了シーケンスを未完扱いとする
                    Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleindex].EndSequence();
                }
                else
                {
                    //接続台数の設定を変更された可能性があるので、接続台数ではなく起動時に作成した配列の要素数で判断する
                    if (compSlaveEndSequence.Length > moduleindex)
                    {
                        compSlaveEndSequence[moduleindex] = true;   //終了シーケンスを未完扱いとする
                        statusSlaveEndSequence[moduleindex] = true; //終了シーケンスを正常終了扱いとする
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// リンス液残量チェック
        /// </summary>
        /// <remarks>
        /// リンス液の残量があるかチェックします
        /// </remarks>
        /// <returns>True:残量あり False:残量なし</returns>
        private Boolean checkRinseRemain()
        {
            Boolean[] SkipList = new Boolean[] { true, true, true, true };
            CarisXSequenceHelper.SequenceSyncObject[] syncDataList = new CarisXSequenceHelper.SequenceSyncObject[Enum.GetValues(typeof(ModuleIndex)).Length];
            foreach (Int32 moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                syncDataList[moduleindex] = new CarisXSequenceHelper.SequenceSyncObject();
            }

            //各モジュールに対して残量チェックコマンドを送信する
            foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                //モジュールが接続されているか
                if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                {
                    SkipList[moduleindex] = false;

                    //接続されている場合は残量チェックコマンドの送信シーケンスを呼び出しする
                    syncDataList[moduleindex] = Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleindex].AskReagentRemain();
                }
            }

            // ファイルからデータ読み込み
            Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance.Load();
            CarisXSensorParameter setting = Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance.Param;

            //送信対象となったそれぞれのシーケンスが完了出来ている事を順番にチェックする。
            foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                if (!SkipList[moduleindex])
                {
                    //送信対象になっている場合
                    while (!syncDataList[moduleindex].EndSequence.WaitOne(10))
                    {
                        Application.DoEvents();
                    }
                    if (syncDataList[moduleindex].Status != CarisXSequenceHelper.SequenceSyncObject.SequenceStatus.Success)
                    {
                        //success以外が設定された場合は処理終了
                        return false;
                    }

                    // リンス液のセンサーチェック
                    // →センサー使用しているなら、残量チェック
                    if (setting.SlaveList[moduleindex].sensorParameterUseNoUse.UsableWashBuffer == (byte)UseStatus.Use)     //パイプラインを掃除する場合（如果清洗管路）
                    {
                        if (((SlaveCommCommand_1414)syncDataList[moduleindex].SequenceResultData).RinceContainerRemain == 0)//リンス液なし（没有去离子水了）
                        {
                            // リンス液なし
                            DlgMessage.Show(String.Empty, CarisX.Properties.Resources.STRING_DLG_MSG_172,
                                CarisX.Properties.Resources.STRING_DLG_TITLE_004, MessageDialogButtons.OK);

                            // リンス液がなかったので処理終了
                            return false;
                        }
                    }
                }
            }

            //ここまで辿り着いた場合はリンス液の残量がある
            return true;
        }

        /// <summary>
        /// 温度設定
        /// </summary>
        /// <param name="value"></param>
        private void onSetAskTemperatureTimer(Object value)
        {
            // タイマ動作を設定
            Boolean enable = (Boolean)value;
            var timerItem = this.commonTimerHandlerList[CommonTimerAction.AskTemperature].Item1;
            timerItem.Enable = enable;

            // タイマ内容を最初はすぐ実行されるよう設定する。
            if (enable)
            {
                timerItem.SetCycle(timerItem.Cycle);
            }
        }

        /// <summary>
        /// 汎用定周期タイマ
        /// </summary>
        /// <param name="sender">タイマーオブジェクト</param>
        /// <param name="e">不使用</param>
        private void commonTimer_Tick(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer timerObj = sender as System.Windows.Forms.Timer;

            // 処理動作中はタイマを止める。処理後再開する。
            timerObj.Stop();

            // 定周期処理の実施を行う。
            foreach (var handler in this.commonTimerHandlerList)
            {
                handler.Value.Item1.AddValue(timerObj.Interval);
                while (handler.Value.Item1.NextCycle())
                {
                    handler.Value.Item2();
                }
            }

            timerObj.Start();
        }

        /// <summary>
        /// 温度更新関数
        /// </summary>
        /// <remarks>
        /// システムの状態が待機中の場合、スレーブへ温度問い合わせを実施します。
        /// </remarks>
        private void updeteTempData()
        {
            // 温度問合せ実施
            foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                {
                    // モーターエラーのスレーブは処理を行わない
                    if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)moduleindex)] == Status.SystemStatusKind.MotorError)
                    {
                        continue;
                    }

                    Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleindex].AskTemperature();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //List<AnalysisErrorInfo> errorList = new List<AnalysisErrorInfo>();
            //AnalysisErrorInfo errorInfo = AddErrorInfo(AnalysisError.PhotometryError, errorList,null,null);
            //errorInfo.EditInfo(null, null, null, null, 2, new String[]{"发光值：1000，背景值：1000"}.ToList());

            //errorList.ForEach((error) =>
            //{
            //    Singleton<CarisXLogManager>.Instance.Write(
            //        Oelco.Common.Log.LogKind.ErrorHist,
            //        Singleton<CarisXUserLevelManager>.Instance.NowUserID,
            //        new CarisXLogInfoErrorLogExtention()
            //        {
            //            ErrorCode = error.ErrorCode,
            //            ErrorArg = error.ErrorArg
            //        },
            //        errorInfo.ErrorDetailInfoParam);
            //});
            //HostCommCommand_0005 respCommand = new HostCommCommand_0005();

            //respCommand.Status = HostCommCommand_0005.InspectionAcceptStatus.AnalysingInspectionAccept;

            //// 応答コマンド送信
            //Singleton<CarisXCommManager>.Instance.PushSendQueueHost(respCommand);


            //test1begin
            // List<AnalysisErrorInfo> errorList;


            // SlaveCommCommand_0503 command = new SlaveCommCommand_0503();
            // command.AfterDilution = 1;
            // command.DarkCount = 0;
            // command.BlankCount = 3000;
            // command.CupKind = SpecimenCupKind.Cup;
            // command.IndividuallyNumber =33814;
            // command.UniqueNo = 41314;
            // command.ReagentLotNumber = "0003";
            // command.RepNo = 1;
            // command.ResultCount = 4000;
            // command.SampleId = "tstdd";
            // command.SampleKind = SampleKind.Calibrator;
            // command.SpecimenMaterialType = SpecimenMaterialType.BloodSerumAndPlasma;
            // command.TriggerLotNo = "0001";
            // command.PreDilution = 1;
            // command.PreTriggerLotNo = "0001";
            // command.RackID = "S006";
            // command.RackPos = 1;
            // command.Remark = null;
            // command.TurnOrder = 1;
            // command.MeasProtocolNumber = 1;

            // CalcData data = new CalcData(command.MeasProtocolNumber, command.ReagentLotNumber, command.IndividuallyNumber, command.UniqueNo,
            //                      1, 1, 1,DateTime.Now, command.RackID, command.RackPos, command.SampleId);
            // data.CalcInfoReplication = new CalcInfo(62824);
            // data.CalcInfoReplication.Concentration = 1.100;
            // data.CalcInfoReplication.CountValue = 62824;


            // //var data1 = ((IMeasureResultData)command).Convert(true);
            // //CalcData aveData;
            // //CarisXCalculator.Calc(command.DarkCount, command.BlankCount, command.ResultCount, command.SampleKind, ref data, out errorList, out aveData);





            //// Singleton<CalibratorResultDB>.Instance.AddResultData(data, command);

            //// Singleton<CalibratorResultDB>.Instance.UpdateAverageData(aveData);
            //// Singleton<CalibratorResultDB>.Instance.CommitData();

            // CarisXCalculator.CreateCalibCurveData(data.ProtocolIndex, data.RackID, data.RackPosition.Value,9009, out errorList);
            // errorList.ForEach((error) =>
            // {
            //     if (error.ErrorCode != 0)
            //     {
            //         Singleton<CarisXLogManager>.Instance.Write(
            //             Oelco.Common.Log.LogKind.ErrorHist,
            //             Singleton<CarisXUserLevelManager>.Instance.NowUserID,
            //             new CarisXLogInfoErrorLogExtention()
            //             {
            //                 ErrorCode = error.ErrorCode,
            //                 ErrorArg = error.ErrorArg
            //             },
            //             error.ErrorDetailInfoParam);
            //     }
            // });
            // Singleton<CalibratorResultDB>.Instance.UpdateRemark(data);
            // Singleton<CalibratorResultDB>.Instance.CommitData();
            //// Singleton<CalibratorAssayDB>.Instance.SetResultData(data);
            //// Singleton<CalibratorAssayDB>.Instance.CommitData();
            // RealtimeDataAgent.LoadCalibratoResultData();
            //test1 end

            //SlaveCommCommand_0503 command = new SlaveCommCommand_0503();
            //command.SetCommandString("4   4710            0001S00111   598816 22 1       0      43   24050         0001000010001   1   125286444664467360");
            ////onAssayData(cmd1);
            //List<AnalysisErrorInfo> errorList;
            //var data = ((IMeasureResultData)command).Convert(true);
            //CalcData aveData;
            //CarisXCalculator.Calc(command.DarkCount, command.BlankCount, command.ResultCount, command.SampleKind, ref data, out errorList, out aveData);

            //SlaveCommCommand_0508 command = new SlaveCommCommand_0508();
            //command.SetCommandString("2 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 5  100000001   1415123001    2002 5  100000001   1415123001    2001 5  100000001   1415123001    2002 6  100000001   1415123001    2002 6   90000001   1415123001    2001 6   90000001   1415123001    2002 7  100000001   1415123001    2002 7  100000001   1415123001    2001 7  100000001   1415123001    200248   38801501   1015062201    100248   38801501   1015062201    100148   37001501   1015062201    100234   64001501   1015062201    100234   64001501   1015062201    100134   91001501   1015062201    100245   57001501   1015062301    100245   57001501   1015062301    100145   87501501   1015062301    100239   50001501   1015062501    100239  100001501   1015062501    100139   50001501   1015062501    1002 4      00001  10115062501    1502 4   75000001  10115062501    1501 4   75000001  10115062501    1502 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 9      08888    315062501    2002 9  100008888    315062501    2001 9  100008888    115062501    2002 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        0258   65001512   1015062301    150258   65001512   1015062301    150158   65001512   1015062301    1502 0      0        0010101        02 0      0        0010101        01 0      0        0010101        0223      01512   1015062501    150223   75001512   1015062501    150123   75001512   1015062501    1502 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        0228      01512   1015062501    150228   75001512   1015062501    150128   75001512   1015062501    150221  141001512   1015061901    150221  141001512   1015061901    150136   50001501  15415061901    100230  150001512   1015062501    150230   65001512   1015062501    150130   60001512   1015062501    1500 0      0        0              00 0      0        0              01 8   75000002  10315062501    1500 0      0        0              00 0      0        0              00 0      0        0              00 0      0        0              00 0      0        0              00 0      0        0              02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        02 0      0        0010101        02 0      0        0010101        01 0      0        0010101        0 136000   1151230 3000000001151230 194600   11512302 300000   1151230 194600   11512302 46  0  0  0  0  0  0  0  0  01 46  0  0  0  0  0  0  0  0  0120000  50010100");
            //string strPause = "ddd";
            //List<AnalysisErrorInfo> errorList;
            //CarisXCalculator.CreateCalibCurveData(37, "S012", 3, 9011, out errorList);
            //  CarisXCalculator.CreateCalibCurveData(1, "S002", 2, 9008, out errorList);


            //   SlaveCommCommand_0503 command = new SlaveCommCommand_0503();
            //    command.SetCommandString("1   7496                004611   947631 37 3       0      45   48148   1048576200707141500150315001503   1   115293835704422390");
            //    //onAssayData(cmd1);
            //    List<AnalysisErrorInfo> errorList;
            //    var data = ((IMeasureResultData)command).Convert(true);
            //    CalcData aveData;
            //    CarisXCalculator.Calc(command.DarkCount, command.BlankCount, command.ResultCount, command.SampleKind, ref data, out errorList, out aveData);

            ////    CarisXCalculator.CreateCalibCurveData(5, "S002", 3, 9005, out errorList);

            //    HostCommCommand_0002 dd = new HostCommCommand_0002();
            //    dd.SetCommandString("W1N    1484436900           S        1                                                                                   02  21  11");

            //    dd.SetCommandString("W1N1111222222222222222233334S555555556666abcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghijabcdefghij 2 1141123");

            //MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(6);
            //if (protocol != null)
            //{
            //    //设置校准点的个数
            //    protocol.NumOfMeasPointInCalib = 6;
            //    for (int i = 0; i < 6; i++)
            //    {
            //        //写入浓度值
            //        protocol.ConcsOfEach[i] = 0.9;

            //    }
            //    Singleton<MeasureProtocolManager>.Instance.SaveMeasureProtocol(protocol.ProtocolNo);
            //}
            // string strConc =  HybridDataMediator.SearchConcentrationFromCalibRegistDB(10, "S008", 1);




            //SlaveCommCommand_0503 command = new SlaveCommCommand_0503();
            //command.SetCommandString("2   4825                002611   7802 0  1 1       0      78  259358         0201507031500150415001505   1   114794338934415020                  ");
            //onAssayData(command);
            //List<AnalysisErrorInfo> errorList;
            //var data = ((IMeasureResultData)command).Convert(true);
            //CalcData aveData;
            //CarisXCalculator.Calc(command.DarkCount, command.BlankCount, command.ResultCount, command.SampleKind, ref data, out errorList, out aveData);

            //SlaveCommCommand_0502 commnnd = new SlaveCommCommand_0502();
            //commnnd.SetCommandString("13C008-4                          ERROR                             ");
            //this.rspAskAssayData(commnnd);

            //List<AnalysisErrorInfo> errorList;
            // CarisXCalculator.CreateCalibCurveData(2, "S002", 1, 9001, out errorList);  

            //SlaveCommCommand_0503 command = new SlaveCommCommand_0503();

            //command.SetCommandString("4  24275                S00111  4829621 37 2       0      61    6691   1048576201507011500150315001503   1   124774827532375410                  ");           
            //onAssayData(command);
            //List<AnalysisErrorInfo> errorList;
            //var data = ((IMeasureResultData)command).Convert(true);
            //CalcData aveData;
            //CarisXCalculator.Calc(command.DarkCount, command.BlankCount, command.ResultCount, command.SampleKind, ref data, out errorList, out aveData);

            // List<double> list = new List<double>();
            // list.Add(402);
            // list.Add(9945);
            // double dAve =0;
            //double dd=  SubFunction.GetSD(list, out dAve,true);
            //double dAve1 = dd;
            //SlaveCommCommand_0502 command = new SlaveCommCommand_0502();
            //command.SetCommandString("14S002-1                          ý@ø=                         ");
            //this.rspAskAssayData(command);

            // List<AnalysisErrorInfo> errorList;
            //  CarisXCalculator.CreateCalibCurveData(15, "S013", 2, 9015, out errorList);

            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //sw.Start();
            //RealtimeDataAgent.LoadSampleData();
            //RealtimeDataAgent.LoadSpecimenResultData();
            //RealtimeDataAgent.LoadAssayData();
            //sw.Stop();
            //Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
            //                                                                               CarisXLogInfoBaseExtention.Empty, String.Format("加载数据库gg花了{0}毫秒", sw.ElapsedMilliseconds));


            //SlaveCommCommand_0503 command = new SlaveCommCommand_0503();
            //command.SetCommandString("4    526      2016031901S00221    52649 67 3       0      81 1011800         0201509012016010120151106   1   114555530733362360                  ");
            //onAssayData(command);
            //List<AnalysisErrorInfo> errorList;
            //var data = ((IMeasureResultData)command).Convert(true);
            //CalcData aveData;
            //CarisXCalculator.Calc(command.DarkCount, command.BlankCount, command.ResultCount, command.SampleKind, ref data, out errorList, out aveData);

            //  List<AnalysisErrorInfo> errorList;
            //   CarisXCalculator.CreateCalibCurveData(1, "S001", 2,9008, out errorList);

            //test Log
            //while (true)
            //{
            //    Thread.Sleep(2);
            //    string ss = "adddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd";
            //    Boolean dd = true;
            //    Boolean ddd = true;
            //    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("【Rack ID添加或删除】{0} Rack ID identification status of the state has changed。 isRemove = {1} isAdd = {2}", ss, dd, ddd));
            //}

            //List<AnalysisErrorInfo> errorList;
            //CarisXCalculator.CreateCalibCurveData(5, "S002", 2, 9010, out errorList);


            // SlaveCommCommand_0503 command = new SlaveCommCommand_0503();
            //// command.SetCommandString("1   7496                004611   947631 37 3       0      45   48148   1048576200707141500150315001503   1   115293835704422390");
            //   command.SetCommandString("4    646          160701S00911    66436 38 3       0    1343   84555         0201607012016000120160001   1   124760635699409130");
            // //onAssayData(cmd1);
            // List<AnalysisErrorInfo> errorList;
            // var data = ((IMeasureResultData)command).Convert(true);
            // CalcData aveData;
            // CarisXCalculator.Calc(command.DarkCount, command.BlankCount, command.ResultCount, command.SampleKind, ref data, out errorList, out aveData);

            // SlaveCommCommand_0503 command = new SlaveCommCommand_0503();
            // command.SetCommandString("1    258           ERROR000111    298 0 10 1       0     446    2835         0160100212016000120160704   1   124701021680363250                  ");
            //command.SetCommandString("1   1110      1607250187000111   3030 0 37 1       0     26614854281         0201603012016020220160201   1   124526026117362440");
            // onAssayData(cmd1);
            //command.SetCommandString("1   1113      1607261708000141   3047 3 36 1       0     238 1781308         0201603012016020220160201   1   124498625859359270");
            // command.SetCommandString("1   1115      1607272264000211   3060 0 37 1       0     276    1118         0201603012016020220160201   1   124514026558360100");
            //   List<AnalysisErrorInfo> errorList;
            //  var data = ((IMeasureResultData)command).Convert(true);
            //  CalcData aveData;
            //  CarisXCalculator.Calc(command.DarkCount, command.BlankCount, command.ResultCount, command.SampleKind, ref data, out errorList, out aveData);

            //刷新实验
            //  RealtimeDataAgent.LoadSampleData();


            // List<AnalysisErrorInfo> errorList;
            //   CarisXCalculator.CreateCalibCurveData(10, "S001", 3, 9001, out errorList);


            //Boolean userCancel = false; // ユーザー意思によるキャンセルが行われたか

            //// 加温待ちを行う
            //using (var dlgWaitTemperature = new DlgWaitAnalyzerTemperature())
            //{
            //    if (dlgWaitTemperature.IsNeedShow())
            //    {
            //        var dlgResult = dlgWaitTemperature.ShowDialog();
            //        userCancel = (dlgResult == System.Windows.Forms.DialogResult.Cancel);
            //    }
            //    //release all the resource!
            //   // dlgWaitTemperature.Close();
            //    // dlgWaitTemperature.Dispose();
            //    MessageBox.Show("hell00");
            //}
            //MessageBox.Show("hell0");

            //ThreadStart threadStart = new ThreadStart(ThreadProc);
            //Thread thread = new Thread(threadStart);
            //thread.Start();   


            //Sample Retest test begin
            // HostCommCommand_0002 hComand = new HostCommCommand_0002();
            // hComand.SetCommandString("W1N    611080483147         S        0001                                                                                05 341 351 361 371 461");
            // hComand.RackID = "0021";
            // hComand.RackPos = 1;
            // hComand.ReceiptNumber = 22;
            // AskWorkSheetData askData = new AskWorkSheetData();
            // askData.FromHostCommand = hComand;
            // SlaveCommCommand_1502 respCommand = new SlaveCommCommand_1502();

            // respCommand.SampleType = SampleKind.Sample;               

            // // 一般検体･優先検体は分析方式による切替必要

            // respCommand.RackID = hComand.RackID;
            // respCommand.RackPos = hComand.RackPos;
            // respCommand.SampleID = hComand.SampleID;
            // respCommand.IndividuallyNumber = 2000;

            // askData.ToDprCommand = respCommand;
            // askData.AskData = respCommand;            

            //// List<MeasItem> itemList = new List<MeasItem>();


            // this.setWorkSheetFromHost(hComand, ref askData.AskData);
            // this.addInprocessSample(askData.AskData, Singleton<SequencialSampleNo>.Instance.Number, askData.FromHostCommand.ReceiptNumber, askData.FromHostCommand.Comment);


            // SampleInfo sampleInfo = Singleton<InProcessSampleInfoManager>.Instance.SearchInProcessSampleFromUniqueNo(askData.AskData.MeasItemArray[0].UniqNo);

            // foreach (var item in sampleInfo.ProtocolStatusDictionary)
            // {
            //     Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("sampleInfo.ProtocolStatusDictionary ProtoclIndex ={0}  Uniq = {1} ", item.Key, askData.AskData.MeasItemArray[0].UniqNo));
            //     foreach (var item1 in item.Value)
            //     {
            //         Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("sampleInfo.ProtocolStatusDictionary repet ={0}  SampleMeasureStatus ={1}Uniq = {2} ", item1.Key, item1.Value.ToString(), askData.AskData.MeasItemArray[0].UniqNo));
            //     }
            // }

            //sample retest end



            //SlaveCommCommand_0503 command = new SlaveCommCommand_0503();
            // command.SetCommandString("1   7496                004611   947631 37 3       0      45   48148   1048576200707141500150315001503   1   115293835704422390");
            //command.SetCommandString("3     22               1C00111     22 0  1 1       0       1       1         0201709072016121620170303   1   114569538072396680                    ");
            //onAssayData(command);
            //List<AnalysisErrorInfo> errorList;
            //var data = ((IMeasureResultData)command).Convert(true);
            //CalcData aveData;
            //CarisXCalculator.Calc(command.DarkCount, command.BlankCount, command.ResultCount, command.SampleKind, ref data, out errorList, out aveData);

            // Boolean isAutoRemeasure = false;
            //Boolean isAutoDilRemeasure = false;
            //Boolean isOnlyRemeasureAdd = true;

            //SlaveCommCommand_0503 command = new SlaveCommCommand_0503();
            //command.SetCommandString("2  62721               3002231  8538044 37 1       1       1       1        32                           1   1245218    0    01 74- 3            ");

            //this.selectReMeasureData(command, true, ref isAutoRemeasure, ref isAutoDilRemeasure, ref isOnlyRemeasureAdd);

            // SlaveCommCommand_0503 command2 = new SlaveCommCommand_0503();
            //command2.SetCommandString("2  62721               3002231  8538044 37 2       1       1       1        32                           1   1245244    0    01 74- 3            ");

            //this.selectReMeasureData(command2, true, ref isAutoRemeasure, ref isAutoDilRemeasure, ref isOnlyRemeasureAdd);

            //List<AnalysisErrorInfo> errorList;
            //SlaveCommCommand_0503 command = new SlaveCommCommand_0503();

            //command.SetCommandString("4   7475        20160903S00421  42115 0 37 2       0     268    4138         0201612042016091020160806   1   124695227327377970                  ");
            //var data = ((IMeasureResultData)command).Convert(true);
            //CalcData aveData;
            //CarisXCalculator.Calc(command.DarkCount, command.BlankCount, command.ResultCount, command.SampleKind, ref data, out errorList, out aveData);

            //bool isAutoRemeasure = false;
            //bool isAutoDilRemeasure = false;
            //bool isOnlyRemeasureAdd = false;


            //Boolean isRemeased = this.selectReMeasureData(command, false, ref isAutoRemeasure, ref isAutoDilRemeasure, ref isOnlyRemeasureAdd, data);

            //this.selectReMeasureData(command, true, ref isAutoRemeasure, ref isAutoDilRemeasure, ref isOnlyRemeasureAdd, data);

            ////            

            //            command.SetCommandString("2   3018    612140165973001021  10392 1 37 1       0     320 8549475         0201609032016091220160909 200   124662727420351880                  ");


            //            var data = ((IMeasureResultData)command).Convert(true);
            //           // CalcData aveData;
            //            CarisXCalculator.Calc(command.DarkCount, command.BlankCount, command.ResultCount, command.SampleKind, ref data, out errorList, out aveData);

            //SlaveCommCommand_0462 cmd0062 = new SlaveCommCommand_0462();
            //IRemainAmountInfoSet remainAmountSet = cmd0062; // インターフェースの実装クラスをrefで渡せない為、ここで作業用にインターフェース型へ移し変える。内容の設定される実体はコマンドクラス。
            //Singleton<ReagentDB>.Instance.GetReagentRemain(ref remainAmountSet);
            //Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd0062);



            //SlaveCommCommand_0502 command = new SlaveCommCommand_0502();
            //command.SetCommandString("14S006-1                          00916080210.540020160801        ");
            //rspAskAssayData(command);
            //command.SetCommandString("14S006-2                          00916080225.940020160801        ");
            //rspAskAssayData(command);
            //List<AnalysisErrorInfo> errorList;
            //CarisXCalculator.CreateCalibCurveData(9, "S006", 2, 9008, out errorList);


            //LIS双通测试
            //SlaveCommCommand_0502 command = new SlaveCommCommand_0502();
            //command.SetCommandString("110071-1                          11111                           ");
            //SlaveCommCommand_1502 respCommand = new SlaveCommCommand_1502();

            //respCommand.SampleType = command.SampleType;

            //// 一般検体･優先検体は分析方式による切替必要
            //if (command.SampleType == SampleKind.Sample
            //    || command.SampleType == SampleKind.Priority)
            //{
            //    respCommand.RackID = command.RackID;
            //    respCommand.RackPos = command.RackPos;
            //    respCommand.SampleID = command.SampleID;
            //}
            //else
            //{
            //    // キャリブレータ、分析項目は分析方式による切替不要
            //    respCommand.RackID = command.RackID;
            //    respCommand.RackPos = command.RackPos;
            //}
            //AskWorkSheetData workSheetData = new AskWorkSheetData();
            //workSheetData.AskData = respCommand;
            //workSheetData.ToDprCommand = respCommand;
            //workSheetData.AskTimeOuted = false;
            //HostCommCommand_0002 hostCommand = new HostCommCommand_0002();
            //hostCommand.SetCommandString("W1N    11111                S        0001010281");
            ////hostCommand.DeviceNo = 1;
            ////hostCommand.RackID = 72;
            ////hostCommand.SampleType
            //workSheetData.FromHostCommand = hostCommand;

            //workSheetFromHost(workSheetData);

        }

        public static void ThreadProc()
        {
            while (true)
            {
                Thread.Sleep(5000);

                // 登録データ更新
                RealtimeDataAgent.LoadSampleData();
                RealtimeDataAgent.LoadStatData();
            }
        }

        /// <summary>
        /// デバッグ用マスターカーブ作成処理
        /// </summary>
        /// <remarks>
        /// デバッグ用にマスターカーブを作製するための処理
        /// </remarks>
        protected void debugMasterCurveCreate()
        {
            Action<SlaveCommCommand_0510> action = (data) =>
            {
            // マスターカーブ使用可否取得
            // プロトコルNoが有効でなければ終了
            if (data.MasterCurve.Length != 0)
                {
                    var curveInfo = data.MasterCurve.First();
                    var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(curveInfo.ProtoNo);
                    if (protocol == null || protocol.CalibType.IsQualitative())
                    {
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                            String.Format("The master curve was discarded because it was set to not use it or IsQualitative  ProtoNo = {0}", curveInfo.ProtoNo));
                        return;
                    }

                    // プロトコルNo,ReagLotでマスターカーブが存在しない場合のみ処理
                    Singleton<CalibrationCurveDB>.Instance.LoadDB();

                    foreach (var masterCurve in data.MasterCurve)
                    {
                        protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(masterCurve.ProtoNo);
                        if (protocol != null)
                        {
                            // 同一ロットのマスターカーブを削除
                            var deleteCurveData = Singleton<CalibrationCurveDB>.Instance.GetData(protocol.ProtocolIndex, data.LotNumber, CarisXConst.MASTER_CURVE_DATE);
                            deleteCurveData.ForEach((v) =>
                            {
                                v.DeleteData();
                            });

                            // 削除を適用
                            Singleton<CalibrationCurveDB>.Instance.SetCalibData(deleteCurveData);
                            Singleton<CalibrationCurveDB>.Instance.CommitData();

                            var curveData = Singleton<CalibrationCurveDB>.Instance.GetData(protocol.ProtocolIndex, data.LotNumber, CarisXConst.MASTER_CURVE_DATE);
                            if (curveData.Count == 0)
                            {
                                for (int i = 0; i < masterCurve.PointCount; i++)
                                {
                                    Singleton<CalibrationCurveDB>.Instance.AddCalibData( 0
                                        , data.ReagCode
                                        , data.LotNumber
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
                    }

                // 結果にコミット
                Singleton<CalibrationCurveDB>.Instance.CommitData();
                }
            };

            SlaveCommCommand_0510[] commandArray = new SlaveCommCommand_0510[3];
            //AFP
            commandArray[0] = new SlaveCommCommand_0510();
            commandArray[0].ReagCode = 1;
            commandArray[0].ProtocolCount = 1;
            commandArray[0].MasterCurve = new MasterCurveInfo[1];
            commandArray[0].LotNumber = "00000001";

            //HBsAb
            commandArray[1] = new SlaveCommCommand_0510();
            commandArray[1].ReagCode = 36;
            commandArray[1].ProtocolCount = 1;
            commandArray[1].MasterCurve = new MasterCurveInfo[1];
            commandArray[1].LotNumber = "00000001";

            //HBsAg
            commandArray[2] = new SlaveCommCommand_0510();
            commandArray[2].ReagCode = 37;
            commandArray[2].ProtocolCount = 1;
            commandArray[2].MasterCurve = new MasterCurveInfo[1];
            commandArray[2].LotNumber = "00000001";

            foreach (SlaveCommCommand_0510 command in commandArray)
            {
                command.MasterCurve[0] = new MasterCurveInfo();
                command.MasterCurve[0].ProtoNo = command.ReagCode;
                command.MasterCurve[0].PointCount = 8;

                switch (command.MasterCurve[0].ProtoNo)
                {
                    case 1:
                        //濃度１～８
                        command.MasterCurve[0].ConcAry = new double[] { 0.01, 0.05, 0.2, 1, 10, 100, 400, 2000 };

                        //カウント１～８
                        command.MasterCurve[0].CountAry = new int[] { 2102, 3496, 8894, 38644, 379796, 3068471, 7045205, 10590632 };
                        break;

                    case 36:
                        //濃度１～８
                        command.MasterCurve[0].ConcAry = new Double[] { 0, 5, 10, 20, 50, 100, 398, 1000 };

                        //カウント１～８
                        command.MasterCurve[0].CountAry = new int[] { 1732, 9442, 17791, 32554, 88436, 202851, 843802, 1194541 };
                        break;

                    case 37:
                        //濃度１～８
                        command.MasterCurve[0].ConcAry = new Double[] { 0, 0.05, 0.1, 0.5, 1, 5, 50, 250 };

                        //カウント１～８
                        command.MasterCurve[0].CountAry = new int[] { 851, 6779, 12884, 63038, 126786, 639107, 5361541, 14389120 };
                        break;
                }

                action(command);
            }
        }

        /// <summary>
        /// SLAVE1クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSlave_Click(object sender, EventArgs e)
        {
            BlinkButton btnSlave = sender as BlinkButton;
            if (btnSlave != null)
            {
                ModuleIndex targetModuleIndex = (ModuleIndex)int.Parse(btnSlave.Tag.ToString());

                //CurrentStateがtrueの時にクリックした際に、falseにしない。（画面上、いずれか１つは必ず選択状態）
                foreach (ModuleIndex moduleIndex in Enum.GetValues(typeof(ModuleIndex)))
                {
                    // 対象モジュールIndexと画面表示モジュールIndexが同じか確認
                    if ( targetModuleIndex == moduleIndex)
                    {
                        // スレーブボタンをON
                        this.btnSlaveList[moduleIndex].BtnSlave.CurrentState = true;

                        // 試薬交換待ちラベルを表示するか確認
                        if (this.lblReagentSetupWaitMinutesList[moduleIndex].IsVisibleLabel == true)
                        {
                            // 試薬交換待ちラベルを表示
                            this.lblReagentSetupWaitMinutesList[moduleIndex].ReagentSetupWaitMinutesLabel.Visible = true;
                        }
                        else
                        {
                            // 試薬交換待ちラベルを非表示
                            this.lblReagentSetupWaitMinutesList[moduleIndex].ReagentSetupWaitMinutesLabel.Visible = false;
                        }
                    }
                    else
                    {
                        // スレーブボタンをOFF
                        this.btnSlaveList[moduleIndex].BtnSlave.CurrentState = false;

                        // 試薬交換待ちラベルを非表示
                        this.lblReagentSetupWaitMinutesList[moduleIndex].ReagentSetupWaitMinutesLabel.Visible = false;
                    }
                }


                Singleton<PublicMemory>.Instance.moduleIndex = targetModuleIndex;
                Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex = (int)targetModuleIndex;

                RealtimeDataAgent.LoadReagentRemainData();
                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ModuleChange);
            }
        }

        /// <summary>
        /// 分析ステータス表示ラベルクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblAnalyzerStatus_Click(object sender, EventArgs e)
        {
            Int32 moduleId = CarisXSubFunction.ModuleIndexToModuleId(Singleton<PublicMemory>.Instance.moduleIndex);

            if ((Singleton<SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.RackTransfer] == SystemStatusKind.SamplingPause)
                || (Singleton<SystemStatus>.Instance.ModuleStatus[moduleId] == SystemStatusKind.SamplingPause))
            {
                //サンプリング停止状態の場合、サンプリング停止理由をダイアログで表示
                DlgSamplingPauseReasonDetail.Show((int)moduleId, Singleton<SystemStatus>.Instance.PauseReason[(int)RackModuleIndex.RackTransfer]
                    , Singleton<SystemStatus>.Instance.PauseReason[(int)moduleId]);
            }
        }

        /// <summary>
        /// 画面の日時表示を更新する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nowTimer_Tick(object sender, EventArgs e)
        {
            // 現在時を取得
            DateTime datetime = DateTime.Now;

            lblNowDate.Text = datetime.ToString("yyyy.MM.dd");      //2019.09.03
            lblNowTime.Text = datetime.ToString("HH:mm");           //15:30
        }

        /// <summary>
        /// 分析ステータスパネルクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pnlAnalyzerStatus_ClickClient(object sender, EventArgs e)
        {
            Int32 moduleId = CarisXSubFunction.ModuleIndexToModuleId(Singleton<PublicMemory>.Instance.moduleIndex);

            if ((Singleton<SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.RackTransfer] == SystemStatusKind.SamplingPause)
                || (Singleton<SystemStatus>.Instance.ModuleStatus[moduleId] == SystemStatusKind.SamplingPause))
            {
                //サンプリング停止状態の場合、サンプリング停止理由をダイアログで表示
                DlgSamplingPauseReasonDetail.Show((int)moduleId, Singleton<SystemStatus>.Instance.PauseReason[(int)RackModuleIndex.RackTransfer]
                    , Singleton<SystemStatus>.Instance.PauseReason[(int)moduleId]);
            }
        }

        /// <summary>
        /// トータルボタンクリック
        /// </summary>
        /// <remarks>
        /// 複数台接続時のみボタン表示
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSlaveTotal_Click(object sender, EventArgs e)
        {
            FormTotal Total = new FormTotal();
            Total.Location = this.Location + (Size)Total.AdjustLocation;
            this.AddOwnedForm(Total); //メイン画面の上にスモールメニューが来るように、スモールメニュー画面をメイン画面に所属させる
            Total.Show(rectangle);
        }

        /// <summary>
        /// デバッグボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDebug_Click(object sender, EventArgs e)
        {
            Remark remark = new Remark();
            remark.AddRemark(Remark.RemarkBit.MReagentSuckingUpError);
            remark.AddRemark(Remark.RemarkBit.R1ReagentSuckingUpError);
            remark.AddRemark(Remark.RemarkBit.R2ReagentSuckingUpError);
            remark.AddRemark(Remark.RemarkBit.PreProcessLiquidSuckingUpError);
            remark.AddRemark(Remark.RemarkBit.NoSampleError);
            remark.AddRemark(Remark.RemarkBit.SampleStoppedUpError);
            remark.AddRemark(Remark.RemarkBit.NotSampleSuckingUpError);
            remark.AddRemark(Remark.RemarkBit.SampleDispenseError);
            remark.AddRemark(Remark.RemarkBit.NoDilutionError);
            remark.AddRemark(Remark.RemarkBit.WashingFailureError);
            remark.AddRemark(Remark.RemarkBit.DetectorError);
            remark.AddRemark(Remark.RemarkBit.SampleDispenseTipSetError);
            remark.AddRemark(Remark.RemarkBit.SampleDispenseTipDisposalError);
            remark.AddRemark(Remark.RemarkBit.DarkError);
            remark.AddRemark(Remark.RemarkBit.PhotometryError);
            remark.AddRemark(Remark.RemarkBit.TempOfImmunoreactionError);
            remark.AddRemark(Remark.RemarkBit.TempOfBF1PreHeatError);
            remark.AddRemark(Remark.RemarkBit.TempOfBF2PreHeatError);
            remark.AddRemark(Remark.RemarkBit.TempOfR1ProbePreHeatError);
            remark.AddRemark(Remark.RemarkBit.TempOfR2ProbePreHeatError);
            remark.AddRemark(Remark.RemarkBit.ReagentExpirationDateError);
            remark.AddRemark(Remark.RemarkBit.DilutionExpirationDateError);
            remark.AddRemark(Remark.RemarkBit.TempOfDetectorError);
            remark.AddRemark(Remark.RemarkBit.TempOfReagentCoolerError);
            remark.AddRemark(Remark.RemarkBit.TempOfDilutionCoolerError);
            remark.AddRemark(Remark.RemarkBit.CycleTimeOverError);
            remark.AddRemark(Remark.RemarkBit.NoMeasuredError);
            remark.AddRemark(Remark.RemarkBit.ReactionVesselCarryError);
            remark.AddRemark(Remark.RemarkBit.PreTriggerExpirationDateError);
            remark.AddRemark(Remark.RemarkBit.TempOfPreTriggertemperatureError);
            remark.AddRemark(Remark.RemarkBit.TriggerExpirationDateError);
            remark.AddRemark(Remark.RemarkBit.CalibrationCurveError);
            remark.AddRemark(Remark.RemarkBit.CalibExpirationDateError);
            remark.AddRemark(Remark.RemarkBit.DiffError);
            remark.AddRemark(Remark.RemarkBit.CalcConcError);
            remark.AddRemark(Remark.RemarkBit.CalibError);
            remark.AddRemark(Remark.RemarkBit.DynamicrangeUpperError);
            remark.AddRemark(Remark.RemarkBit.DynamicrangeLowerError);
            remark.AddRemark(Remark.RemarkBit.ControlRangeError);
            remark.AddRemark(Remark.RemarkBit.ControlError);
            remark.AddRemark(Remark.RemarkBit.EditOfManualDil);
            remark.AddRemark(Remark.RemarkBit.EditOfReCalcu);
            remark.AddRemark(Remark.RemarkBit.EditOfCalibCount);
            remark.AddRemark(Remark.RemarkBit.EditOfReCalcuByEditCurve);
            remark.AddRemark(Remark.RemarkBit.EditOfControlConc);
            DlgRemarkDetail.Show(remark);

            DlgCheckCompanyLogo checkCompanyLogo = new DlgCheckCompanyLogo();
            checkCompanyLogo.ShowDialog();

            ProtocolConverterLogInfo convLog = new ProtocolConverterLogInfo();
            convLog.ErrorList.Add("Test Error 1");
            convLog.ErrorList.Add("Test Error 2");

            DlgConvertErrLog dlg = new DlgConvertErrLog();
            dlg.setErrorList(convLog.ErrorList);
            dlg.ShowDialog();

            DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_085, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.Confirm);

            DlgMessage.Show(String.Format(CarisX.Properties.Resources.STRING_DLG_MSG_141, CarisXConst.INASSAY_CALCDATA_LIMIT_MAX.ToString()), "", CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OK);

            DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_211 + CarisXConst.TRANSMIT_DATA_MAX.ToString() + CarisX.Properties.Resources.STRING_DLG_MSG_212
                , String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.Confirm);

            DlgMessage.Show(Properties.Resources.STRING_DLG_MSG_245, String.Empty, Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.Confirm);

            List<AnalysisErrorInfo> errorList = new List<AnalysisErrorInfo>();

            var failed = (Action<String>)((detailInfoMessage) =>
            {
                AnalysisErrorInfo analysisErrorInfo = new AnalysisErrorInfo();
                analysisErrorInfo.EditInfo(errorCode: 34, errorArg: 1, errorDetailInfoParam: new List<String>() { detailInfoMessage });
                errorList.Add(analysisErrorInfo);
            });

            failed(Properties.Resources.STRING_ERROR_MESSAGE_000 + Properties.Resources.STRING_ERROR_MESSAGE_001 + Properties.Resources.STRING_ERROR_MESSAGE_005);
            failed(Properties.Resources.STRING_ERROR_MESSAGE_000 + Properties.Resources.STRING_ERROR_MESSAGE_002 + Properties.Resources.STRING_ERROR_MESSAGE_005);
            failed(Properties.Resources.STRING_ERROR_MESSAGE_000 + Properties.Resources.STRING_ERROR_MESSAGE_003 + Properties.Resources.STRING_ERROR_MESSAGE_005);
            failed(Properties.Resources.STRING_ERROR_MESSAGE_000 + Properties.Resources.STRING_ERROR_MESSAGE_006 + Properties.Resources.STRING_ERROR_MESSAGE_005);

            errorList.ForEach((error) =>
            {
                if (error.ErrorCode != 0)
                {
                    DPRErrorCode errCode = new DPRErrorCode(error.ErrorCode, error.ErrorArg, 1);

                // エラー履歴に登録（アラート発生無し）
                CarisXSubFunction.WriteDPRErrorHist(errCode, 0, (error.ErrorDetailInfoParam.Count > 0 ? error.ErrorDetailInfoParam[0] : String.Empty), false);
                }
            });

            DlgMessage.Show(Properties.Resources.STRING_DLG_MSG_253, "", "", MessageDialogButtons.OK);

            DlgMessage.Show(Properties.Resources.STRING_DLG_MSG_254, "", "", MessageDialogButtons.OK);

            String[] contents = new String[4];
            contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_002 + CarisX.Properties.Resources.STRING_COMMON_014
                            + "AFP" + CarisX.Properties.Resources.STRING_COMMON_015;
            contents[1] = "param";
            contents[2] = "paramDetail";
            contents[3] = "ChangeValue";
            Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);

            var userid = Singleton<CarisXUserLevelManager>.Instance.NowUserID;

            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_012 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_013 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_014 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_018 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_019 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_020 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_021 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_022 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_023 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_024 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_028 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_035 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_038 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_039 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_040 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_041 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_043 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_044 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_047 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_049 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_050 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_051 });

            contents = new String[4];
            contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
            contents[1] = "lblDialogTitle.Text";
            contents[2] = Properties.Resources.STRING_LOG_MSG_053;
            contents[3] = "valueStr";
            Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);

            contents = new String[4];
            contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
            contents[1] = "lblDialogTitle.Text";
            contents[2] = Properties.Resources.STRING_LOG_MSG_054;
            contents[3] = "valueStr";
            Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);

            contents = new String[4];
            contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
            contents[1] = "lblDialogTitle.Text";
            contents[2] = Properties.Resources.STRING_LOG_MSG_055;
            contents[3] = "valueStr";
            Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);

            contents = new String[4];
            contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
            contents[1] = "lblDialogTitle.Text";
            contents[2] = Properties.Resources.STRING_LOG_MSG_056;
            contents[3] = "valueStr";
            Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);

            contents = new String[4];
            contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
            contents[1] = "lblDialogTitle.Text";
            contents[2] = Properties.Resources.STRING_LOG_MSG_057;
            contents[3] = "valueStr";
            Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);

            contents = new String[4];
            contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
            contents[1] = "lblDialogTitle.Text";
            contents[2] = Properties.Resources.STRING_LOG_MSG_058;
            contents[3] = "valueStr";
            Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);

            contents = new String[4];
            contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
            contents[1] = "lblDialogTitle.Text";
            contents[2] = Properties.Resources.STRING_LOG_MSG_059;
            contents[3] = "valueStr";
            Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);

            contents = new String[4];
            contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
            contents[1] = "lblDialogTitle.Text";
            contents[2] = Properties.Resources.STRING_LOG_MSG_060;
            contents[3] = "valueStr";
            Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);

            contents = new String[4];
            contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
            contents[1] = "lblDialogTitle.Text";
            contents[2] = Properties.Resources.STRING_LOG_MSG_063;
            contents[3] = "valueStr";
            Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);

            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_064 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_065 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_066 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_067 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_068 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                , CarisXLogInfoBaseExtention.Empty, Properties.Resources.STRING_LOG_MSG_069);
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_070 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_071 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_072 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_073 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_074 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_075 });
            CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.FromHostWorkSheetAlreadyExists, extStr: String.Format(CarisX.Properties.Resources.STRING_LOG_MSG_076, 1, 1));
            CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.FromHostWorkSheetFormatError, extStr: String.Format(CarisX.Properties.Resources.STRING_LOG_MSG_078, CarisXConst.REGIST_MEAS_ITEM_MAX_UPPER));
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_079 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_080 });
            CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.FromHostWorkSheetFormatError, extStr: String.Format(CarisX.Properties.Resources.STRING_LOG_MSG_081, 1, 1));
            CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.FromHostWorkSheetFormatError, extStr: CarisX.Properties.Resources.STRING_LOG_MSG_082);
            CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.FromHostWorkSheetFormatError, extStr: CarisX.Properties.Resources.STRING_LOG_MSG_083);
            CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.FromHostWorkSheetFormatError, extStr: String.Format(CarisX.Properties.Resources.STRING_LOG_MSG_084, 1, 1));
            CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.FromHostWorkSheetFormatError, extStr: CarisX.Properties.Resources.STRING_LOG_MSG_085);
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { string.Format(CarisX.Properties.Resources.STRING_LOG_MSG_087, "txtGroupName") });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_088 });
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, userid, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources.STRING_LOG_MSG_089 });
        }
    }
}
