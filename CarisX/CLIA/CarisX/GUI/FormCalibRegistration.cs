using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.CarisX.Parameter;
using Oelco.Common.Utility;
using Oelco.CarisX.DB;
using Infragistics.Win.UltraWinGrid;
using Oelco.CarisX.Print;
using Oelco.CarisX.Const;
using Oelco.CarisX.Utility;
using Oelco.Common.DB;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.Common.Parameter;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Common;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// キャリブレータ登録画面クラス
    /// </summary>
    public partial class FormCalibRegistration : FormChildBase
    {
        #region [定数定義]

        /// <summary>
        /// ラック最大数
        /// </summary>
        public const Int32 CALIB_MAX_RACK_COUNT = 100;

        /// <summary>
        /// 保存
        /// </summary>
        public const String SAVE = "Save";

        /// <summary>
        /// 削除
        /// </summary>
        public const String DELETE = "Delete";

        /// <summary>
        /// 全て削除
        /// </summary>
        public const String DELETE_ALL = "Delete all";

        /// 印刷
        /// </summary>
        public const String PRINT = "Print";

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 現在の分析項目
        /// </summary>
        private MeasureProtocol currentProtocol = null;

        /// <summary>
        /// 現在のキャリブレータ登録情報
        /// </summary>
        private List<CalibratorRegistData> currentCalibRegistInfo = new List<CalibratorRegistData>();


        /// <summary>
        /// 現在の試薬コードのロット番号リスト
        /// </summary>
        private Dictionary<String, String> reagentLotInfos;

        /// <summary>
        /// 指定ラックID
        /// </summary>
        private CalibRackID selectRackId;

        /// <summary>
        /// 現在の指定ラックIDでの登録可否
        /// </summary>
        public Boolean canRegist;

        /// <summary>
        /// 登録開始ラックポジション
        /// </summary>
        private Int32 registStartPos;

        /// <summary>
        /// Gridバインド用データリスト
        /// </summary>
        private BindingList<CalibratorRegistData> grdBindgngList;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormCalibRegistration()
        {
            InitializeComponent();

            // コマンドバーのイベント追加
            this.tlbCommandBar.Tools[SAVE].ToolClick += (sender, e) => this.saveData();
            this.tlbCommandBar.Tools[DELETE].ToolClick += (sender, e) => this.deleteData();
            this.tlbCommandBar.Tools[DELETE_ALL].ToolClick += (sender, e) => this.deleteAllData();
            this.tlbCommandBar.Tools[PRINT].ToolClick += (sender, e) => this.printData();

            // 分析項目測定テーブル変更後通知登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.AnalyteRoutineTableChanged, this.onAnalyteRoutineTableChanged);
            // 印刷機能有無切替通知
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UseOfPrint, this.onPrintParamChanged);
            // ラックID割り当て変更後通知
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.RackIdDefinitionChanged, this.onRackIdDefinitionChanged);
            // 分析項目変更後通知
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ChangeProtocolSetting, this.onChangeProtocolSetting);

            this.grdCalibRegistration.SetGridRowBackgroundColorRuleFromRowData(
                (rowData1, rowData2) =>
                {
                    if (((CalibratorRegistData)rowData1).GetStartRackID().Value > ((CalibratorRegistData)rowData2).GetStartRackID().Value || ((CalibratorRegistData)rowData1).GetStartRackPosition() > ((CalibratorRegistData)rowData2).GetStartRackPosition())
                    {
                        return 1;
                    }
                    else if (((CalibratorRegistData)rowData1).GetStartRackID().Value < ((CalibratorRegistData)rowData2).GetStartRackID().Value || ((CalibratorRegistData)rowData1).GetStartRackPosition() < ((CalibratorRegistData)rowData2).GetStartRackPosition())
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                },
                new[] { CarisXConst.GRID_ROWS_DEFAULT_COLOR, CarisXConst.GRID_ROWS_COLOR_PATTERN1 }.ToList());

            //Dictionary<string, string> dicPoints = new Dictionary<string, string>();
            //dicPoints.Add("2", "2");
            //dicPoints.Add("3", "3");
            //this.cmbManualPoints.Text = string.Empty;
            //this.cmbManualPoints.DataSource = dicPoints.ToList();
            //this.cmbManualPoints.DisplayMember = "Key";
            //this.cmbManualPoints.ValueMember = "Value";
            //this.cmbManualPoints.SelectedIndex = 0;

            this.gbxCalConPoints.Visible = false;

            // 登録分析モジュールはモジュール1を選択状態とする
            this.chkModule1.Checked = true;
            
            // 搬送先モジュールのチェック状態の表示制御
            this.dispControlForCheckModule();

            //设置ToolBar的右键功能不可用
            this.tlbCommandBar.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.tlbCommandBar_BeforeToolbarListDropdown);
        }

        //设置ToolBar的右键功能不可用
        private void tlbCommandBar_BeforeToolbarListDropdown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventArgs e)
        {
            e.Cancel = true;
        }
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// フォーム表示
        /// </summary>
        /// <remarks>
        /// 画面初期化して、表示します
        /// </remarks>
        /// <param name="captScreenRect">表示対象領域</param>
        public override void Show(Rectangle captScreenRect)
        {
            this.rackCalibRegistStatusView.Clear();
            this.btnSelectRackId.Enabled = ((this.numRackId.Value ?? "").ToString().Length > 0) && (this.cmbReagentLotNo.Items.Count > 0);
            this.loadData();
            base.Show(captScreenRect);
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// リソースの初期化
        /// </summary>
        /// <remarks>
        /// リソースを初期化します
        /// </remarks>
        protected override void initializeResource()
        {
        }

        /// <summary>
        /// コンポーネントの初期化
        /// </summary>
        /// <remarks>
        /// コンポーネントを初期化します
        /// </remarks>
        protected override void initializeFormComponent()
        {
            // スクロール処理設定
            this.gesturePanel.ScrollProxy = this.grdCalibRegistration.ScrollProxy;

            // グリッド表示順
            this.grdCalibRegistration.SetGridColumnOrder(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.CalibResistrationSettings.GridColOrder);
            // グリッド列幅
            this.grdCalibRegistration.SetGridColmnWidth(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.CalibResistrationSettings.GridColWidth);

            // 印刷ボタン表示設定
            this.tlbCommandBar.Tools[PRINT].SharedProps.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable;
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            this.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_019;

            // コマンドバーアイテム名設定
            this.tlbCommandBar.Tools[SAVE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_001;
            this.tlbCommandBar.Tools[DELETE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_002;
            this.tlbCommandBar.Tools[DELETE_ALL].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_003;
            this.tlbCommandBar.Tools[PRINT].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_004;

            // 登録パネルアイテム名設定
            this.lblRegistrationTitle.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_010;
            this.lblAnalytes.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_011;
            this.btnSelectAnalyte.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_012;
            this.lblReagentLotNoSelection.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_013;
            this.lblRackId.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_014;
            this.lblCalibRackIDPrefix.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_015;
            this.btnSelectRackId.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_016;
            this.lblCalibratorLotNo.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_017;
            this.lblCalConPoints.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_021;
            this.chkManual.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_022;
            this.lblConc1.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_023;
            this.lblConc2.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_024;
            this.lblConc3.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_025;
            this.lblModuleNoSelection.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_026;
            this.chkModule1.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_027;
            this.chkModule2.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_028;
            this.chkModule3.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_029;
            this.chkModule4.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_030;

            // グリッドカラムヘッダー表示設定
            this.grdCalibRegistration.DisplayLayout.Bands[0].Columns[CalibratorRegistData.DataKeys.RackID].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_001;
            this.grdCalibRegistration.DisplayLayout.Bands[0].Columns[CalibratorRegistData.DataKeys.RackPosition].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_002;
            this.grdCalibRegistration.DisplayLayout.Bands[0].Columns[CalibratorRegistData.DataKeys.Analytes].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_003;
            this.grdCalibRegistration.DisplayLayout.Bands[0].Columns[CalibratorRegistData.DataKeys.Concentration].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_004;
            this.grdCalibRegistration.DisplayLayout.Bands[0].Columns[CalibratorRegistData.DataKeys.LotSelection].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_005;
            this.grdCalibRegistration.DisplayLayout.Bands[0].Columns[CalibratorRegistData.DataKeys.ReagentLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_006;
            this.grdCalibRegistration.DisplayLayout.Bands[0].Columns[CalibratorRegistData.DataKeys.ConcentrationUnit].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_007;
            this.grdCalibRegistration.DisplayLayout.Bands[0].Columns[CalibratorRegistData.DataKeys.CalibLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_020;

            // 登録分析モジュール番号を追加
            this.grdCalibRegistration.DisplayLayout.Bands[0].Columns[CalibratorRegistData.DataKeys.RegisteredModules].Hidden = true;
            this.grdCalibRegistration.DisplayLayout.Bands[0].Columns[CalibratorRegistData.DataKeys.RegisteredModulesString].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_031;

            // ラック登録状態表示ラックタイトル設定
            this.rackCalibRegistStatusView.RackIDTitle = Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_018;
        }

        #endregion

        #region [privateメソッド]

        #region _コマンドバー_

        /// <summary>
        /// すべての登録対象情報の保存
        /// </summary>
        /// <remarks>
        /// 操作履歴に登録実行を登録し、キャリブレータ情報の抽出して反映を行います
        /// </remarks>
        private void saveData()
        {
            if (!this.canRegist)
            {
                // TODO:(メッセージ文言不確定)登録されるキャリブレータがありません。
                DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_071, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.Confirm);
                return;
            }

            // マスターキャリブ時に濃度値が空の場合、登録できないようにする
            if (chkManual.Checked)
            {
                if (textCon1.Value == null || textCon2.Value == null ||
                    (textCon1.Value != null && string.IsNullOrEmpty(textCon1.Value.ToString())) ||
                    (textCon2.Value != null && string.IsNullOrEmpty(textCon2.Value.ToString())))
                {
                    DlgMessage.Show(Properties.Resources.STRING_DLG_MSG_250, String.Empty, Properties.Resources.STRING_DLG_MSG_251, MessageDialogButtons.OK);
                    return;
                }
                // 濃度3が空データの場合
                if (this.currentProtocol.NumOfMeasPointInCalib > 2 && (textCon3.Value == null ||
                    (textCon3.Value != null && string.IsNullOrEmpty(textCon3.Value.ToString()))))
                {
                    DlgMessage.Show(Properties.Resources.STRING_DLG_MSG_250, String.Empty, Properties.Resources.STRING_DLG_MSG_252, MessageDialogButtons.OK);
                    return;
                }
            }

                // 操作履歴登録：登録実行
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_009 });

            // 登録対象情報の追加
            CalibRackID rackId = new CalibRackID();
            DateTime registDateTime = DateTime.Now;
            Int32 posCount = (this.currentProtocol.CalibType.IsQualitative()) ? CarisXConst.QUALITATIVE_POINT_COUNT : this.currentProtocol.NumOfMeasPointInCalib;

            // 登録分析モジュール番号を取得
            Int32 registeredModules = this.getRegisteredModules();

            if (this.currentProtocol.CalibMethod == MeasureProtocol.CalibrationMethod.MasterCalibration
                    && chkManual.Checked && this.currentProtocol.CalibType.IsQuantitative())
            {
                for (Int32 pos = this.registStartPos, i = 0; pos < this.registStartPos + posCount && i < posCount; i++, pos++)
                {
                    rackId.Value = this.selectRackId.Value + pos / CarisXConst.RACK_POS_COUNT;
                    if (i == 0)//校准浓度1
                    {
                        Singleton<CalibratorRegistDB>.Instance.AddData( rackId
                                                                      , (pos % (CarisXConst.RACK_POS_COUNT) + 1)
                                                                      , this.selectRackId
                                                                      , (this.registStartPos + 1)
                                                                      , this.currentProtocol.ProtocolIndex
                                                                      , this.cmbReagentLotNo.Value.ToString()
                                                                      , SubFunction.TruncateParse(double.Parse(textCon1.Value.ToString()), this.currentProtocol.LengthAfterDemPoint)
                                                                      , registDateTime
                                                                      , this.txtCalibratorLotNo.Text
                                                                      , registeredModules);
                    }
                    if (i == 1)//校准浓度2
                    {
                        Singleton<CalibratorRegistDB>.Instance.AddData( rackId
                                                                      , (pos % (CarisXConst.RACK_POS_COUNT) + 1)
                                                                      , this.selectRackId
                                                                      , (this.registStartPos + 1)
                                                                      , this.currentProtocol.ProtocolIndex
                                                                      , this.cmbReagentLotNo.Value.ToString()
                                                                      , SubFunction.TruncateParse(double.Parse(textCon2.Value.ToString()), this.currentProtocol.LengthAfterDemPoint)
                                                                      , registDateTime
                                                                      , this.txtCalibratorLotNo.Text
                                                                      , registeredModules);
                    }
                    if (i == 2)//校准浓度3
                    {
                        Singleton<CalibratorRegistDB>.Instance.AddData( rackId
                                                                      , (pos % (CarisXConst.RACK_POS_COUNT) + 1)
                                                                      , this.selectRackId
                                                                      , (this.registStartPos + 1)
                                                                      , this.currentProtocol.ProtocolIndex
                                                                      , this.cmbReagentLotNo.Value.ToString()
                                                                      , SubFunction.TruncateParse(double.Parse(textCon3.Value.ToString()), this.currentProtocol.LengthAfterDemPoint)
                                                                      , registDateTime
                                                                      , this.txtCalibratorLotNo.Text
                                                                      , registeredModules);
                    }

                }
            }
            else
            {
                // 登録濃度の取得
                var concs = new[] { "" };
                if (this.currentProtocol.CalibType.IsQuantitative())
                {
                    string strLotNo = this.cmbReagentLotNo.Value.ToString();
                    List<CalibrationCurveData> list = Singleton<CalibrationCurveDB>.Instance.GetMasterCurveData(this.currentProtocol.ProtocolIndex, strLotNo);
                    if (list.Count != 0)
                    {
                        concs = new string[list.Count];
                        for (int i = 0; i < list.Count; i++)
                        {
                            concs[i] = list[i].Concentration;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    concs = new[] { SubFunction.TruncateParse(this.currentProtocol.NegaLine, this.currentProtocol.LengthAfterDemPoint), SubFunction.TruncateParse(this.currentProtocol.PosiLine, this.currentProtocol.LengthAfterDemPoint) };
                }

                //------------------------------------------------------------------------------------------
                // 測定ポイントの抽出
                var measConcs = concs.Zip(this.currentProtocol.CalibMeasPointOfEach
                    , (conc, measPoint) => new
                    {
                        measPoint,
                        conc
                    }).Where((point) => this.currentProtocol.CalibMethod == MeasureProtocol.CalibrationMethod.FullCalibration || point.measPoint).ToArray();

                // 測定ポイントが抽出された場合
                if (measConcs.Count() != 0)
                {
                    // 登録開始ポジションから登録回数分繰り返す
                    for (Int32 pos = this.registStartPos; pos < this.registStartPos + posCount; pos++)
                    {
                        // ラックID値の取得
                        rackId.Value = this.selectRackId.Value + pos / CarisXConst.RACK_POS_COUNT;

                        // 各ポジションごとにデータを追加
                        Singleton<CalibratorRegistDB>.Instance.AddData(rackId
                                                                      , (pos % (CarisXConst.RACK_POS_COUNT) + 1)
                                                                      , this.selectRackId
                                                                      , (this.registStartPos + 1)
                                                                      , this.currentProtocol.ProtocolIndex
                                                                      , this.cmbReagentLotNo.Value.ToString()
                                                                      , measConcs[pos - this.registStartPos].conc
                                                                      , registDateTime
                                                                      , this.txtCalibratorLotNo.Text
                                                                      , registeredModules);
                    }
                }
                // 測定ポイントが抽出されなかった場合
                else
                {
                    // 分析パラメータに不整合があるため、登録されません。
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_269, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.Confirm);

                    // 測定方法と測定タイプに不整合が発生する場合がある
                    String dbgMsg = "[[Investigation log]]FormCalibRegistration::saveData " +
                                     "measConcs Array Count is = 0, Calibration type and Calibration method are mismatch";

                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                }
            }

            // 変更を反映
            Singleton<CalibratorRegistDB>.Instance.CommitData();

            this.loadData();
            this.canRegist = false;
            this.setRegistView(this.selectRackId);

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// 選択中登録情報の削除
        /// </summary>
        /// <remarks>
        /// 操作履歴に削除実行を登録し、キャリブレータ情報データの削除実行して反映を行います
        /// </remarks>
        private void deleteData()
        {
            // 操作履歴登録：消去実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_003 });

            // UNDONE:消去します。よろしいですか？
            if (DialogResult.OK != DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_019, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel))
            {
                // 操作履歴登録：消去キャンセル
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_004 });
                return;
            }

            // 選択中のラックポジションのラックID、ラックポジションのキャリブレータ登録情報データを抽出
            var selectRows = this.grdCalibRegistration.SearchSelectRow();
            var selectRowDatas = selectRows.Select((row) => ((CalibratorRegistData)row.ListObject)).ToList();

            // 選択中ラックポジションのキャリブレータ登録情報データより
            // 削除対象データ(同一開始ラックID、同一開始ラックポジション)の抽出
            var targets = from v in this.currentCalibRegistInfo
                          where selectRowDatas.Exists((data) => data.GetStartRackID().DispPreCharString == v.GetStartRackID().DispPreCharString && data.GetStartRackPosition() == v.GetStartRackPosition())
                          select v;

            // 削除対象の削除実施
            foreach (var target in targets)
            {
                target.DeleteData();
            }

            // 変更を反映
            Singleton<CalibratorRegistDB>.Instance.SetData(this.currentCalibRegistInfo);
            Singleton<CalibratorRegistDB>.Instance.CommitData();
            this.loadData();
            this.canRegist = false;
            this.setRegistView(new CalibRackID()
            {
                Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDCalib
            });
        }

        /// <summary>
        /// すべての登録情報の削除
        /// </summary>
        /// <remarks>
        /// 操作履歴に全消去実行を登録し、キャリブレータ情報データの全削除実行して反映を行います
        /// </remarks>
        private void deleteAllData()
        {
            // 操作履歴登録：全消去実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_010 });

            // UNDONE:全消去します。よろしいですか？
            if (DialogResult.OK != DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_001, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel))
            {
                // 操作履歴登録：全消去キャンセル
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_011 });
                return;
            }
            this.currentCalibRegistInfo.DeleteAllDataList();

            // 変更を反映
            Singleton<CalibratorRegistDB>.Instance.SetData(this.currentCalibRegistInfo);
            Singleton<CalibratorRegistDB>.Instance.CommitData();
            this.loadData();
            this.canRegist = false;
            this.setRegistView(new CalibRackID()
            {
                Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDCalib
            });
        }

        /// <summary>
        /// 登録情報の印刷出力
        /// </summary>
        /// <remarks>
        /// 操作履歴に印刷実行を登録し、キャリブレータ情報データの印刷を行います
        /// </remarks>
        private void printData()
        {
            // 操作履歴登録：印刷実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_005 });

            TargetRange outputRange = DlgTargetSelectRange.Show();

            // 印刷対象を取得
            List<CalibratorRegistData> printData = null;
            switch (outputRange)
            {
                case TargetRange.All:
                    Singleton<CalibratorRegistDB>.Instance.LoadDB();
                    printData = Singleton<CalibratorRegistDB>.Instance.GetData();
                    break;
                case TargetRange.Specification:
                    Singleton<CalibratorRegistDB>.Instance.LoadDB();
                    var selectRows = this.grdCalibRegistration.SearchSelectRow();
                    printData = selectRows.Select((row) => ((CalibratorRegistData)row.ListObject)).ToList();

                    break;
                case TargetRange.None:
                    // 操作履歴登録：印刷キャンセル
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_006 });
                    return;
            }

            if (printData == null || printData.Count == 0)
            {
                // 印刷するデータがありません。
                DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_064, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_003, MessageDialogButtons.Confirm);
                return;
            }

            // 印刷用Listに取得データを格納
            List<CalibRegistrationReportData> rptData = new List<CalibRegistrationReportData>();
            foreach (var row in printData)
            {
                CalibRegistrationReportData rptDataRow = new CalibRegistrationReportData();
                rptDataRow.RackID = row.RackID.ToString();
                rptDataRow.ProtoName = row.Analytes;
                rptDataRow.Conc = row.Concentration;
                rptDataRow.LotSelect = row.LotSelection;
                rptDataRow.LotNo = row.ReagentLotNo;
                rptDataRow.CalibratorLot = row.CalibLotNo;
                rptDataRow.PrintDateTime = DateTime.Now.ToDispString();

                rptData.Add(rptDataRow);
            }

            CalibRegistrationPrint prt = new CalibRegistrationPrint();
            Boolean ret = prt.Print(rptData);

        }

        #endregion

        /// <summary>
        /// ラック登録候補状態表示設定
        /// </summary>
        /// <remarks>
        /// 指定ラックIDを開始ラックIDとするキャリブレータ登録情報を取得します
        /// </remarks>
        private void setRegistView(CarisXIDString rackId)
        {
            // ラック設定フラグ
            Boolean setRackFlag = true;

            // 指定ラックIDを開始ラックIDとするキャリブレータ登録情報を取得
            IEnumerable<CalibratorRegistData> firstRackDatas = from v in this.currentCalibRegistInfo
                                                               where v.RackID.Value == rackId.Value
                                                               orderby v.RackPosition
                                                               select v;

            IEnumerable<CalibratorRegistData> secondRackDatas = null;
            IEnumerable<CalibratorRegistData> datas = (from v in firstRackDatas
                                                       orderby v.RackID.Value, v.RackPosition
                                                       select v).ToList();

            if (firstRackDatas.Count() != 0)
            {
                var firstRackFirstPosData = firstRackDatas.FirstOrDefault();
                var measureProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(firstRackFirstPosData.GetMeasureProtocolIndex());

                if (firstRackFirstPosData.RackID.DispPreCharString == firstRackFirstPosData.GetStartRackID().DispPreCharString &&
                    measureProtocol.NumOfMeasPointInCalib > CarisXConst.RACK_POS_COUNT)
                {
                    secondRackDatas = from v in this.currentCalibRegistInfo
                                      where (v.RackID.Value == (rackId.Value + 1)) //&& ( v.GetMeasureProtocolIndex() == protoNumber )
                                      select v;                                  //orderby v.RackID.Value, v.RackPosition

                    datas = (from v in datas.Union(secondRackDatas)
                             orderby v.RackID.Value, v.RackPosition
                             select v).ToList();
                }
            }

            //var datas = from v in this.currentCalibRegistInfo
            //            let idx = ( from vv in this.currentCalibRegistInfo where vv.RackID.Value == rackId.Value select v.GetMeasureProtocolIndex() ).SingleOrDefault()
            //            where v.RackID.Value == rackId.Value || ( v.RackID.Value == rackId.Value + 1 && v.GetMeasureProtocolIndex() == idx )
            //            orderby v.RackID.Value, v.RackPosition
            //            select v;
            //var datas = this.currentCalibRegistInfo.OrderBy( ( data ) => data.RackID.Value ).ThenBy( ( data ) => data.RackPosition ).
            //    Where( ( data ) => data.RackID.DispPreCharString == rackId.DispPreCharString );

            // ラックポジションステータスインスタンス生成
            List<Tuple<String, ProtocolRegistStatus>>[] rackInfo = new List<Tuple<String, ProtocolRegistStatus>>[] { new List<Tuple<String, ProtocolRegistStatus>>(), new List<Tuple<String, ProtocolRegistStatus>>() };
            foreach (var list in rackInfo)
            {
                while (list.Count < CarisXConst.RACK_POS_COUNT)
                {
                    list.Add(new Tuple<String, ProtocolRegistStatus>(null, ProtocolRegistStatus.Empty));
                }
            }

            // 登録済み情報の追加
            foreach (var data in datas)
            {
                var rackIndex = (data.RackID.DispPreCharString == rackId.DispPreCharString) ? 0 : 1;
                var rackPosIndex = data.RackPosition - 1;
                var calibType = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(data.GetMeasureProtocolIndex()).CalibType;

                String dispPos = String.Empty;
                // 定量項目
                if (calibType.IsQuantitative())
                {
                    dispPos = data.Concentration;
                }
                // 定性項目
                else if (calibType.IsQualitative())
                {
                    dispPos = (data.RackPosition == data.GetStartRackPosition()) ? CarisXConst.NEGATIVE_POSITION : CarisXConst.POSITIVE_POSITION;
                }

                rackInfo[rackIndex][rackPosIndex] = new Tuple<String, ProtocolRegistStatus>(dispPos, ProtocolRegistStatus.Registerd);
            }

            // 登録候補情報の追加
            if (this.canRegist)
            {
                // 指定のプロトコル、試薬ロット、ラックID、キャリブレータロット番号
                var posLimit = this.registStartPos;
                if (this.currentProtocol.CalibType.IsQuantitative())
                {
                    posLimit += this.currentProtocol.NumOfMeasPointInCalib;
                }
                else if (this.currentProtocol.CalibType.IsQualitative())
                {
                    posLimit += CarisXConst.QUALITATIVE_POINT_COUNT;
                }


                var concs = new[] { "" };
                //若是定量的项目，获得Master曲线
                if (this.currentProtocol.CalibType.IsQuantitative())
                {
                    string strLotNo = this.cmbReagentLotNo.Value.ToString();
                    List<CalibrationCurveData> list = Singleton<CalibrationCurveDB>.Instance.GetMasterCurveData(this.currentProtocol.ProtocolIndex, strLotNo);
                    if (list.Count != 0)
                    {
                        concs = new string[list.Count];
                        for (int i = 0; i < list.Count; i++)
                        {
                            concs[i] = list[i].Concentration;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else//定性的项目，确定Positive和Negative值
                {
                    concs = new[] { SubFunction.TruncateParse(this.currentProtocol.NegaLine, this.currentProtocol.LengthAfterDemPoint), SubFunction.TruncateParse(this.currentProtocol.PosiLine, this.currentProtocol.LengthAfterDemPoint) };
                }

                // 測定ポイントのみ抽出
                var measPointConc = concs.Zip(this.currentProtocol.CalibMeasPointOfEach, (conc, meas) => new
                {
                    conc,
                    meas
                }).Where((data) => this.currentProtocol.CalibMethod == MeasureProtocol.CalibrationMethod.FullCalibration || data.meas);

                if (this.currentProtocol.CalibMethod == MeasureProtocol.CalibrationMethod.MasterCalibration
                    && chkManual.Checked && this.currentProtocol.CalibType.IsQuantitative())
                {
                    for (Int32 i = this.registStartPos, j = 0; i < posLimit && j < this.currentProtocol.NumOfMeasPointInCalib; i++, j++)
                    {
                        String dispPos = String.Empty;

                        // 定量項目
                        if (j == 0)
                        {
                                dispPos = SubFunction.TruncateParse(double.Parse(textCon1.Value.ToString()), this.currentProtocol.LengthAfterDemPoint);
                            }
                        if (j == 1)
                        {
                                dispPos = SubFunction.TruncateParse(double.Parse(textCon2.Value.ToString()), this.currentProtocol.LengthAfterDemPoint);
                            }
                        if (j == 2)
                        {
                                dispPos = SubFunction.TruncateParse(double.Parse(textCon3.Value.ToString()), this.currentProtocol.LengthAfterDemPoint);
                            }
                        rackInfo[i / CarisXConst.RACK_POS_COUNT][i % CarisXConst.RACK_POS_COUNT] = new Tuple<String, ProtocolRegistStatus>(dispPos, ProtocolRegistStatus.Uncertain);
                    }
                }
                else
                {
                    // 抽出された測定ポイント数が測定ポイント数より大きい場合
                    if (measPointConc.Count() < this.currentProtocol.NumOfMeasPointInCalib)
                    {
                        // ラック設定フラグをfalseにする
                        setRackFlag = false;

                        // データに不整合があるため、登録されません。
                        DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_269, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.Confirm);

                        // マスターカーブのポイント数と測定ポイント数によっては不整合が発生する場合がある
                        String dbgMsg = String.Format("[[Investigation log]]FormCalibRegistration::setRegistView " +
                                         "measConcs Array Count is = {0}, posLimit = {1}, " +
                                         "measPointConc and posLimit are mismatch", measPointConc.Count(), posLimit);

                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                    }
                    // 抽出された測定ポイント数が測定ポイント数以下の場合
                    else
                    {
                        for (Int32 i = this.registStartPos; i < posLimit; i++)
                        {
                            String dispPos = String.Empty;

                            // 定量項目
                            if (this.currentProtocol.CalibType.IsQuantitative())
                            {
                                dispPos = measPointConc.ElementAt(i - ( this.registStartPos )).conc;
                            }
                            // 定性項目
                            else if (this.currentProtocol.CalibType.IsQualitative())
                            {
                                dispPos = ( this.registStartPos == i ) ? CarisXConst.NEGATIVE_POSITION : CarisXConst.POSITIVE_POSITION;
                            }

                            rackInfo[i / CarisXConst.RACK_POS_COUNT][i % CarisXConst.RACK_POS_COUNT] = new Tuple<String, ProtocolRegistStatus>(dispPos, ProtocolRegistStatus.Uncertain);
                        }
                    }
                }

            }

            // 登録状態(候補状態)の表示
            //this.rackCalibRegistStatusView.Clear();
            //this.rackCalibRegistStatusView.AddRackInfo( rackId.DispPreCharString, rackInfo[0].ToArray() );
            //if ( rackInfo.Select( ( list ) => list.Where( ( data ) => data.Item2 != ProtocolRegistStatus.Empty ).Count() ).Sum() > CarisXConst.RACK_POS_COUNT )
            //{
            //    this.rackCalibRegistStatusView.AddRackInfo( new CalibRackID()
            //    {
            //        Value = rackId.Value + 1
            //    }.DispPreCharString, rackInfo[1].ToArray() );
            //}

            // ラック設定フラグがtrueの場合
            if (setRackFlag)
            {
                this.rackCalibRegistStatusView.SetRackInfo(rackId, rackInfo);
            }
            // ラック設定フラグがfalseの場合
            else
            {
                // 何もしない
            }
            
        }

        /// <summary>
        /// ラックポジション毎登録候補状態表示設定
        /// </summary>
        /// <remarks>
        /// textConで変更した値を取得してRegistViewに反映します
        /// </remarks>
        /// <param name="numEdiTextConc">textCon番号</param>
        private void setTextConRegistView(Infragistics.Win.UltraWinEditors.UltraNumericEditor numEdiTextConc)
        {
            // RackIDが選択された状態
            // キャリブメソッドがMasterCalibrationを選択中
            // manual calibration pointのyesにチェック状態
            // キャリブレーションタイプが定量項目の時
            if ((this.canRegist)
                && (this.currentProtocol.CalibMethod == MeasureProtocol.CalibrationMethod.MasterCalibration)
                && (chkManual.Checked)
                && (this.currentProtocol.CalibType.IsQuantitative()))
            {
                // tagに設定したtextConの番号を設定します
                int textConcNo = int.Parse(numEdiTextConc.Tag.ToString());
                // textConの値を設定します
                string strTextConc = String.Empty;
                // ラックポジションの濃度値を設定します
                String dispPos = String.Empty;
                // double.TryParse判定用の一時的な変数
                double result = -1;

                // textConが空の状態でManual Calibration Pointを切り替えた時、nullになっているため
                strTextConc = numEdiTextConc.Value == null ? String.Empty : numEdiTextConc.Value.ToString();

                // double.Parseで例外が発生するため、double.TryParseを実施します
                if (double.TryParse(strTextConc, out result))
                {
                    // 小数点以下第2位までを表示するため
                    dispPos = SubFunction.TruncateParse(double.Parse(strTextConc), this.currentProtocol.LengthAfterDemPoint);
                }
                else
                {
                    // 空文字の場合、doble string変換処理で例外が発生してしまうため
                    dispPos = strTextConc;
                }

                // ラックポジションを設定します
                Int32 rackPosition = this.registStartPos + textConcNo;

                // 濃度値をRegistViewに反映します
                this.rackCalibRegistStatusView.SetTextConRackInfo(rackPosition, dispPos);
            }
        }

        /// <summary>
        /// 分析項目選択ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 分析項目選択を更新します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnSelectAnalyte_Click(object sender, EventArgs e)
        {
            using (DlgProtocolSelect dlg = new DlgProtocolSelect(true, 1, 1))
            {
                MeasureProtocol measureProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromName(this.lblAnalyte.Text);
                if (measureProtocol != null)
                {
                    dlg.SelectedProtocolIndexs.Add(measureProtocol.ProtocolIndex);
                }
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (dlg.SelectedProtocolIndexs.Count > 0)
                    {
                        // 分析項目の取得
                        this.currentProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(dlg.SelectedProtocolIndexs[0]);

                        // 分析項目名を表示
                        this.lblAnalyte.Text = this.currentProtocol.ProtocolName;

                        // 試薬ロット番号の取得
                        Singleton<ReagentDB>.Instance.LoadDB();
                        List<String> reagentLotNames = Singleton<ReagentDB>.Instance.GetReagentLotNo(this.currentProtocol.ReagentCode).ToList();

                        // ロット番号並び替え(昇順)
                        reagentLotNames.Sort((x, y) =>
                       {
                           return Int32.Parse(x) - Int32.Parse(y);
                       });

                        // 最小試薬ロット番号の表示を"現ロット"へ切り替え
                        this.reagentLotInfos = new Dictionary<String, String>();
                        //this.reagentLotInfos.Add( Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_000, String.Empty );
                        if (reagentLotNames.Count > 0)
                        {
                            reagentLotNames.ForEach((name) =>
                            {
                                if (!this.reagentLotInfos.ContainsKey(name))
                                {
                                    this.reagentLotInfos.Add(name, name);
                                }
                            });
                        }

                        // 試薬ロット選択へデータを設定
                        this.cmbReagentLotNo.Text = string.Empty;
                        this.cmbReagentLotNo.DataSource = this.reagentLotInfos.ToList();
                        this.cmbReagentLotNo.DisplayMember = "Key";
                        this.cmbReagentLotNo.ValueMember = "Value";
                        this.cmbReagentLotNo.SelectedIndex = 0;

                        // 試薬ロット番号が複数個ある場合のみロット番号選択可変有効
                        this.cmbReagentLotNo.Enabled = (this.cmbReagentLotNo.Items.Count >= 1);

                        // 試薬ロット番号が単数個以上ある場合のみラックID指定可能
                        this.btnSelectRackId.Enabled = (this.cmbReagentLotNo.Items.Count > 0);
                    }
                    else
                    {
                        // 分析項目名を表示(選択なし)
                        this.lblAnalyte.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;
                    }
                    this.numRackId.Value = null;
                    this.txtCalibratorLotNo.Text = String.Empty;
                    this.rackCalibRegistStatusView.Clear();
                    this.canRegist = false;
                    this.chkManual.Checked = false;

                    //add method to  show or not show  anyPoints calibaration
                    if (this.currentProtocol.CalibMethod == Oelco.CarisX.Parameter.MeasureProtocol.CalibrationMethod.FullCalibration)
                    {
                        //gbxCalConPoints.Enabled = false;
                        this.gbxCalConPoints.Visible = false;
                        //手動校正ポイントの表示・非表示に応じて、Rackグループの表示位置を調整する
                        this.rackCalibRegistStatusView.Location = new Point(this.rackCalibRegistStatusView.Location.X, 448);
                        
                    }
                    else
                    {
                        this.gbxCalConPoints.Visible = true;
                        //手動校正ポイントの表示・非表示に応じて、Rackグループの表示位置を調整する
                        this.rackCalibRegistStatusView.Location = new Point(this.rackCalibRegistStatusView.Location.X, 561);
                        gbxCalConPoints.Enabled = true;
                        clearConcPointsText();
                        if (this.currentProtocol.NumOfMeasPointInCalib > 2)
                        {
                            showCons3(true);
                        }
                        else
                        {
                            showCons3(false);
                        }
                        enableConcPoints(false);
                    }

                    // Form共通の編集中フラグOFF
                    FormChildBase.IsEdit = false;
                }
            }
        }

        /// <summary>
        /// データの再取得
        /// </summary>
        /// <remarks>
        /// キャリブ登録情報を取得します
        /// </remarks>
        private void loadData()
        {
            // 各種コントロールの初期化
            this.numRackId.MaxValue = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDCalib;
            this.numRackId.MinValue = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDCalib;

            if (this.currentProtocol != null)
            {
                // 分析項目を設定
                this.lblAnalyte.Text = this.currentProtocol.ProtocolName;

                // 分析項目が設定しているため、ラックIDの選択ボタンは活性状態にする
                this.btnSelectRackId.Enabled = true;
            }
            else
            {
                this.lblAnalyte.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;
                this.txtCalibratorLotNo.Clear();
                this.cmbReagentLotNo.Items.Clear();
                this.rackCalibRegistStatusView.Clear();
            }

            // キャリブ登録情報の取得
            Singleton<CalibratorRegistDB>.Instance.LoadDB();
            if (this.currentCalibRegistInfo == null)
            {
                this.currentCalibRegistInfo = new List<CalibratorRegistData>();
            }
            if (this.grdBindgngList == null)
            {
                this.grdBindgngList = new BindingList<CalibratorRegistData>(this.currentCalibRegistInfo);
            }

            this.currentCalibRegistInfo.Clear();
            this.currentCalibRegistInfo.AddRange(Singleton<CalibratorRegistDB>.Instance.GetData());

            if (this.grdCalibRegistration.DataSource != null)
            {
                this.grdBindgngList.ResetBindings();
            }
            else
            {
                this.grdCalibRegistration.DataSource = this.grdBindgngList;
            }

            bool enabledFlag = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.IsProtocolEnabledChangedInEmergencyMode();

            foreach (UltraGridRow row in this.grdCalibRegistration.Rows)
            {
                CalibratorRegistData data = (CalibratorRegistData)row.ListObject;
                var protocal = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(data.GetMeasureProtocolIndex());

                // 全スレーブで急診無しかつ急診使用ありの分析項目のグリッドの背景色を変える
                if ((enabledFlag == true) && (protocal.UseEmergencyMode == true))
                {
                    row.Appearance.BackColor = Color.LightGray;
                }
               
            }


        }

        /// <summary>
        /// ラックID確定ボタンクリック
        /// </summary>
        /// <remarks>
        /// ラックIDを確定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnSelectRackId_Click(object sender, EventArgs e)
        {
            //定量项目浓度点限制条件。
            if (this.currentProtocol.CalibType.IsQuantitative())
            {
                // 手動校正を行うか確認
                if (chkManual.Checked)
                {
                    if (textCon1.Value == null || textCon2.Value == null ||
                        (textCon1.Value != null && string.IsNullOrEmpty(textCon1.Value.ToString())) ||
                        (textCon2.Value != null && string.IsNullOrEmpty(textCon2.Value.ToString())))
                    {
                        DlgMessage.Show(Properties.Resources.STRING_DLG_MSG_250, String.Empty, Properties.Resources.STRING_DLG_MSG_251, MessageDialogButtons.OK);
                        return;
                    }
                    // 濃度3が空データの場合
                    if (this.currentProtocol.NumOfMeasPointInCalib > 2 && (textCon3.Value == null ||
                        (textCon3.Value != null && string.IsNullOrEmpty(textCon3.Value.ToString()))))
                    {
                        DlgMessage.Show(Properties.Resources.STRING_DLG_MSG_250, String.Empty, Properties.Resources.STRING_DLG_MSG_252, MessageDialogButtons.OK);
                        return;
                    }
                }
                else
                {
                    string strLotNo = this.cmbReagentLotNo.Value.ToString();
                    List<CalibrationCurveData> list = Singleton<CalibrationCurveDB>.Instance.GetMasterCurveData(this.currentProtocol.ProtocolIndex, strLotNo);
                    if (list.Count == 0)
                    {
                        this.canRegist = false;
                        this.rackCalibRegistStatusView.Clear();
                        DlgMessage.Show(Properties.Resources.STRING_DLG_MSG_249, String.Empty, Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel);
                        return;
                    }
                }
            }

            // 入力ラックIDチェック
            Int32? rackId = this.numRackId.Value as Int32?;
            if (rackId.HasValue)
            {
                this.selectRackId = new CalibRackID();
                this.selectRackId.Value = rackId.Value;
            }
            else
            {
                DlgMessage.Show(Properties.Resources.STRING_DLG_MSG_117, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                return;
            }

            // 再設定フラグ
            DialogResult reset = DialogResult.None;
            do
            {
                CalibRackID nowRack = new CalibRackID();
                Int32 sequencialRackCount = (this.currentProtocol.CalibType.IsQualitative()) ? 1 : (Int32)(Math.Ceiling((Double)this.currentProtocol.NumOfMeasPointInCalib / (Double)CarisXConst.RACK_POS_COUNT)); // 必要連続空きラック数
                // 登録開始位置は、1ラックに収まるポジション数であれば、1ポジション目以外からそのラックへの登録を行うことができるが。
                // 2ラック以上に跨る場合、必ず1ポジションから開始する。
                //                 Int32 nowResitStartPos = sequencialRackCount > 1 ? 0 : this.registStartPos; // 登録開始位置
                nowRack.Value = this.selectRackId.Value;
                this.canRegist = true;
                for (Int32 rackSeqIndex = 0; (this.canRegist) && rackSeqIndex < sequencialRackCount; rackSeqIndex++)
                {
                    if (nowRack.Value > Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDCalib ||
                        nowRack.Value < Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDCalib)
                    {
                        this.selectRackId = null;
                        this.canRegist = false;
                        // ラックIDが指定範囲を超えました。
                        DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_119, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.Confirm);
                        return;
                    }

                    // 設置済みラックポジション取得
                    var nonVacantPosData = this.currentCalibRegistInfo.FindAll((data) =>
                   {
                       return data.RackID.Value == nowRack.Value;
                   }).Select((data) => data.RackPosition);

                    // ラックポジションの空きが必要数以上で連続している場合、登録可能
                    //this.canRegist = true;
                    this.registStartPos = 0;

                    Int32 needEmptyPosCount = (this.currentProtocol.CalibType.IsQualitative()) ? 2 : this.currentProtocol.NumOfMeasPointInCalib - (rackSeqIndex * CarisXConst.RACK_POS_COUNT);

                    for (Int32 i = 1; i <= needEmptyPosCount; i++)
                    {
                        // 連続空きラックポジションカウント位置が空きポジション以外の場合
                        if (nonVacantPosData.Contains(this.registStartPos + i))
                        {
                            if (sequencialRackCount != 1)
                            {
                                // 2ラック以上に跨る場合、途中ポジションからの登録は不可。
                                this.canRegist = false;
                                break;
                            }
                            else
                            {
                                // 開始ラックポジションをシフト
                                this.registStartPos += i;
                            }
                            // 連続ラックポジションのカウント初期化
                            i = 0;
                        }

                        // 連続空きラックポジションカウント中にラック数上限に達した場合、登録不可
                        if (this.registStartPos + i > CarisXConst.RACK_POS_COUNT)
                        {
                            if (sequencialRackCount == (rackSeqIndex + 1))
                            {
                                this.canRegist = false;
                            }
                            break;
                        }
                    }

                    nowRack.Value++;
                }


                // 登録可否
                if (this.canRegist)
                {
                    this.numRackId.Value = this.selectRackId.Value;
                    this.setRegistView(this.selectRackId);
                    break;
                }
                else
                {
                    if (reset == DialogResult.None)
                    {
                        // 指定ラック使用中の空きラック検索実施確認
                        reset = DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_127, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel);
                        if (reset == DialogResult.OK)
                        {
                            this.selectRackId.Value++;
                        }
                        else// if ( reset == DialogResult.None )
                        {
                            this.setRegistView(this.selectRackId);
                        }
                    }
                    else
                    {
                        this.selectRackId.Value++;
                    }
                }
            } while (reset == DialogResult.OK);

            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        ///// <summary>
        ///// グリッドセルのアクティブ後イベント
        ///// </summary>
        ///// <param name="sender">呼び出し元オブジェクト</param>
        ///// <param name="e">イベントデータ</param>
        //private void grdCalibRegistration_AfterCellActivate( object sender, EventArgs e )
        //{
        //    this.canRegist = false;
        //    this.setRegistView( ( (CalibratorRegistData)this.grdCalibRegistration.ActiveCell.Row.ListObject).GetStartRackID());
        //}

        ///// <summary>
        ///// グリッド行のアクティブ後イベント
        ///// </summary>
        ///// <param name="sender">呼び出し元オブジェクト</param>
        ///// <param name="e">イベントデータ</param>
        //private void grdCalibRegistration_AfterRowActivate( object sender, EventArgs e )
        //{
        //    this.canRegist = false;
        //    this.setRegistView( ( (CalibratorRegistData)this.grdCalibRegistration.ActiveRow.ListObject ).GetStartRackID() );
        //}

        /// <summary>
        /// FormClosedイベント
        /// </summary>
        /// <remarks>
        /// UI設定保存します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormCalibRegistration_FormClosed(object sender, FormClosedEventArgs e)
        {
            // UI設定保存
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.CalibResistrationSettings.GridColOrder = this.grdCalibRegistration.GetGridColumnOrder();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.CalibResistrationSettings.GridColWidth = this.grdCalibRegistration.GetGridColmnWidth();
        }

        /// <summary>
        /// キャリブレータ登録グリッド選択変更イベント
        /// </summary>
        /// <remarks>
        /// ラック登録候補状態表示設定を更新します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdCalibRegistration_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            this.canRegist = false;
            this.setRegistView(((CalibratorRegistData)this.grdCalibRegistration.SearchSelectRow()[0].ListObject).GetStartRackID());
        }

        /// <summary>
        /// 分析項目測定テーブル変更後処理
        /// </summary>
        /// <remarks>
        /// 登録情報全削除します
        /// </remarks>
        /// <param name="value"></param>
        private void onAnalyteRoutineTableChanged(object value)
        {
            // 登録情報全削除
            this.currentCalibRegistInfo.DeleteAllDataList();
            Singleton<CalibratorRegistDB>.Instance.SetData(this.currentCalibRegistInfo);
            Singleton<CalibratorRegistDB>.Instance.CommitData();
            this.currentProtocol = null;

            this.loadData();
        }

        /// <summary>
        /// 印刷パラメータ変更時処理
        /// </summary>
        /// <remarks>
        /// 印刷ボタン表示設定します
        /// </remarks>
        /// <param name="value"></param>
        private void onPrintParamChanged(Object value)
        {
            // 印刷ボタン表示設定
            this.tlbCommandBar.Tools[PRINT].SharedProps.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable;
        }

        /// <summary>
        /// ラックID割り当て変更時
        /// </summary>
        /// <remarks>
        /// ラックID割り当て変更します
        /// </remarks>
        /// <param name="value"></param>
        private void onRackIdDefinitionChanged(Object value)
        {
            // 一般(優先)ラックID割り当て変更時
            var changeSampleKind = value as IEnumerable<SampleKind>;
            if ((changeSampleKind ?? new SampleKind[] { }).Contains(SampleKind.Calibrator))
            {
                // 登録情報全削除
                this.currentCalibRegistInfo.DeleteAllDataList();

                Singleton<CalibratorRegistDB>.Instance.SetData(this.currentCalibRegistInfo);
                Singleton<CalibratorRegistDB>.Instance.CommitData();

                this.loadData();
            }
        }

        /// <summary>
        /// 分析項目変更時
        /// </summary>
        /// <remarks>
        /// 分析項目変更時に手動校正の表示切替、校正項目3の表示切替えを行う
        /// </remarks>
        /// <param name="value"></param>
        private void onChangeProtocolSetting( Object value )
        {
            // 現在の分析項目がnullではない場合
            if (this.currentProtocol != null)
            {
                // 分析方式がフルキャリブレーションの場合
                if (this.currentProtocol.CalibMethod == Oelco.CarisX.Parameter.MeasureProtocol.CalibrationMethod.FullCalibration)
                {
                    // 手動校正を非表示にする
                    this.gbxCalConPoints.Visible = false;
                }
                // それ以外の場合
                else
                {
                    // 手動校正を表示する
                    this.gbxCalConPoints.Visible = true;

                    // キャリブレーションポイント数が2より大きい場合
                    if (this.currentProtocol.NumOfMeasPointInCalib > 2)
                    {
                        // 校正項目3を表示する
                        this.showCons3(true);
                    }
                    else
                    {
                        // 校正項目3を非表示にする
                        this.showCons3(false);
                    }
                }
            }
            // 現在の分析項目がnullの場合
            else
            {
                // 何もしない
            }
           
        }

        /// <summary>
        /// 手動校正設定切替イベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkManual_CheckedValueChanged(object sender, EventArgs e)
        {
            if (this.currentProtocol == null)
            {
                return;
            }
            if (this.currentProtocol.CalibMethod == Oelco.CarisX.Parameter.MeasureProtocol.CalibrationMethod.FullCalibration)
            {
                return;
            }

            // double.TryParse判定用の一時的な変数
            double result = -1;

            this.enableConcPoints(true);
            if (!chkManual.Checked)
            {
                this.enableConcPoints(false);

                // RegistViewに値を設定します
                this.setRegistView(this.selectRackId);

            }
            // textConかRegistViewの値を設定します
            else if (this.canRegist && this.currentProtocol.CalibType.IsQuantitative())
            {
                // RackIDに入っている値をtextCon1に表示させます
                if ((textCon1.Value == null)
                    || !(double.TryParse(textCon1.Value.ToString(), out result)))
                {
                    // 濃度1にRegistViewの値を設定します
                    textCon1.Value = this.rackCalibRegistStatusView.GetTextConRackInfo(this.registStartPos + int.Parse(textCon1.Tag.ToString()));
                }

                // RackIDに入っている値をtextCon2に表示させます
                if ((textCon2.Value == null)
                    || !(double.TryParse(textCon2.Value.ToString(), out result)))
                {
                    // 濃度2にRegistViewの値を設定します
                    textCon2.Value = this.rackCalibRegistStatusView.GetTextConRackInfo(this.registStartPos + int.Parse(textCon2.Tag.ToString()));
                }

                if (this.currentProtocol.NumOfMeasPointInCalib > 2)
                {
                    // RackIDに入っている値をtextCon3に表示させます
                    if ((textCon3.Value == null)
                        || !(double.TryParse(textCon3.Value.ToString(), out result)))
                    {
                        // 濃度3が必要な場合
                        textCon3.Value = this.rackCalibRegistStatusView.GetTextConRackInfo(this.registStartPos + int.Parse(textCon3.Tag.ToString()));
                    }
                }

                // RegistViewに値を設定します
                this.setRegistView(this.selectRackId);
            }

            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        /// <summary>
        /// 校正項目有効無効切替処理
        /// </summary>
        /// <param name="bEnable"></param>
        private void enableConcPoints(bool bEnable)
        {
            textCon1.Enabled = bEnable;
            textCon2.Enabled = bEnable;
            textCon3.Enabled = bEnable;
        }

        /// <summary>
        /// 校正項目3表示切替処理
        /// </summary>
        /// <param name="bShow"></param>
        private void showCons3(bool bShow)
        {
            lblConc3.Visible = bShow;
            textCon3.Visible = bShow;
        }

        /// <summary>
        /// 校正項目入力値クリア処理
        /// </summary>
        private void clearConcPointsText()
        {
            textCon1.ResetText();
            textCon2.ResetText();
            textCon3.ResetText();
        }

        /// <summary>
        /// 登録分析モジュール番号
        /// </summary>
        /// <returns></returns>
        private Int32 getRegisteredModules()
        {
            Int32 category = 0;

            // モジュール1
            if (this.chkModule1.Checked)
            {
                category |= (int)ModuleCategory.Module1;
            }
            // モジュール2
            if (this.chkModule2.Checked)
            {
                category |= (int)ModuleCategory.Module2;
            }
            // モジュール3
            if (this.chkModule3.Checked)
            {
                category |= (int)ModuleCategory.Module3;
            }
            // モジュール4
            if (this.chkModule4.Checked)
            {
                category |= (int)ModuleCategory.Module4;
            }

            return category;
        }

        /// <summary>
        /// 登録モジュール1チェック状態切り替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkModule1_CheckedChanged(object sender, EventArgs e)
        {
            // 対象のチェック状態がOFFになる場合
            if (this.chkModule1.Checked == false)
            {
                // 他のチェック状態が全てOFFの場合
                if ((this.chkModule2.Checked == false)
                    && (this.chkModule3.Checked == false)
                    && (this.chkModule4.Checked == false))
                {
                    // チェックを外さないようにする
                    this.chkModule1.Checked = true;
                }
                else
                {
                    // Form共通の編集中フラグON
                    FormChildBase.IsEdit = true;
                }
            }
            else
            {
                // Form共通の編集中フラグON
                FormChildBase.IsEdit = true;
            }
        }

        /// <summary>
        /// 登録モジュール2チェック状態切り替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkModule2_CheckedChanged(object sender, EventArgs e)
        {
            // 対象のチェック状態がOFFになる場合
            if (this.chkModule2.Checked == false)
            {
                // 他のチェック状態が全てOFFの場合
                if ((this.chkModule1.Checked == false)
                    && (this.chkModule3.Checked == false)
                    && (this.chkModule4.Checked == false))
                {
                    // チェックを外さないようにする
                    this.chkModule2.Checked = true;
                }
                else
                {
                    // Form共通の編集中フラグON
                    FormChildBase.IsEdit = true;
                }
            }
            else
            {
                // Form共通の編集中フラグON
                FormChildBase.IsEdit = true;
            }
        }

        /// <summary>
        /// 登録モジュール3チェック状態切り替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkModule3_CheckedChanged(object sender, EventArgs e)
        {
            // 対象のチェック状態がOFFになる場合
            if (this.chkModule3.Checked == false)
            {
                // 他のチェック状態が全てOFFの場合
                if ((this.chkModule1.Checked == false)
                    && (this.chkModule2.Checked == false)
                    && (this.chkModule4.Checked == false))
                {
                    // チェックを外さないようにする
                    this.chkModule3.Checked = true;
                }
                else
                {
                    // Form共通の編集中フラグON
                    FormChildBase.IsEdit = true;
                }
            }
            else
            {
                // Form共通の編集中フラグON
                FormChildBase.IsEdit = true;
            }
        }

        /// <summary>
        /// 登録モジュール4チェック状態切り替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkModule4_CheckedChanged(object sender, EventArgs e)
        {
            // 対象のチェック状態がOFFになる場合
            if (this.chkModule4.Checked == false)
            {
                // 他のチェック状態が全てOFFの場合
                if ((this.chkModule1.Checked == false)
                    && (this.chkModule2.Checked == false)
                    && (this.chkModule3.Checked == false))
                {
                    // チェックを外さないようにする
                    this.chkModule4.Checked = true;
                }
                else
                {
                    // Form共通の編集中フラグON
                    FormChildBase.IsEdit = true;
                }
            }
            else
            {
                // Form共通の編集中フラグON
                FormChildBase.IsEdit = true;
            }
        }

        /// <summary>
        /// LotNo切り替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCalibratorLotNo_ValueChanged(object sender, EventArgs e)
        {
            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        /// <summary>
        /// RackID切り替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numRackId_ValueChanged(object sender, EventArgs e)
        {
            // Form共通の編集中フラグON

            FormChildBase.IsEdit = true;
        }

        /// <summary>
        /// ManualCalibrationPoint conc1, conc2, conc3切り替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textCon_ValueChanged(object sender, EventArgs e)
        {
            // textCon1～3のうち、値変更されたtextConをUltraNumericEditor型で設定します
            Infragistics.Win.UltraWinEditors.UltraNumericEditor numEdiTextConc = sender as Infragistics.Win.UltraWinEditors.UltraNumericEditor;

            // UltraNumericEditor型で取得できない場合はnullになるので処理しないようにします
            if (numEdiTextConc != null)
            {
                // RegistViewの値をリアルタイム変更
                setTextConRegistView(numEdiTextConc);
            }

            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        /// <summary>
        /// 搬送先モジュールのチェック状態の表示制御
        /// </summary>
        private void dispControlForCheckModule()
        {
            // 接続台数取得
            int numOfConnect = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;

            switch( numOfConnect )
            {
                // 1台構成の場合
                case 1:
                    // グループボックス自体を非表示
                    this.gbxModuleNoSelection.Enabled = false;
                    this.gbxModuleNoSelection.Visible = false;

                    // モジュール選択のグループボックスが非表示になった場合、
                    // 手動補正ポイントのグループボックスが見えると画面に穴ができるため、
                    // 手動補正ポイントのグループボックスのY座標位置を移動する
                    Point movePoint = this.gbxCalConPoints.Location;
                    movePoint.Y = this.gbxModuleNoSelection.Location.Y;
                    this.gbxCalConPoints.Location = movePoint;

                    break;
                
                // 2台構成の場合
                case 2:
                    // モジュール3,4を非表示・非活性
                    this.chkModule3.Visible = false;
                    this.chkModule4.Visible = false;
                    this.chkModule3.Enabled = false;
                    this.chkModule4.Enabled = false;
                    break;

                // 3台構成時
                case 3:
                    // モジュール4を非表示・非活性
                    this.chkModule4.Visible = false;
                    this.chkModule4.Enabled = false;
                    break;

                // 4台構成時
                case 4:
                default:
                    // 全て表示
                    break;
            }
        }

        /// <summary>
        /// 現在の試薬No.情報の選択変更後イベント
        /// </summary>
        /// <remarks>
        /// 現在の試薬No.情報の選択を変更しデータを設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void cmbReagentLotNo_SelectionChanged(object sender, EventArgs e)
        {
            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        #endregion
    }
}
