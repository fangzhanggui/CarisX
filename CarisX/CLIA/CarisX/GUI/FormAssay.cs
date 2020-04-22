using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.CarisX.DB;
using Oelco.Common.Utility;
using Oelco.CarisX.Const;
using Oelco.CarisX.GUI.Controls;
using Infragistics.Win.Misc;
using Oelco.CarisX.Parameter;
using System.Collections;
using Oelco.CarisX.Common;
using Oelco.CarisX.Utility;
using Infragistics.Win.UltraWinGrid;
using Oelco.Common.DB;
using Infragistics.Win;
using Oelco.Common.Log;
using Oelco.CarisX.Log;
using Oelco.Common.Parameter;
using Infragistics.Win.UltraWinEditors;
using Infragistics.UltraGauge.Resources;
using Infragistics.Win.UltraWinGauge;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 分析ステータス画面クラス
    /// </summary>
    public partial class FormAssay : FormChildBase
    {
        #region [定数定義]
        /// <summary>
        /// 試薬グリッド分析項目名列キー
        /// </summary>
        private const String STRING_ANALYTES = "Analytes";
        /// <summary>
        /// 試薬グリッド試薬ロット番号列キー
        /// </summary>
        private const String STRING_LOTNO = "LotNo";
        /// <summary>
        /// 試薬グリッド残量列キー
        /// </summary>
        private const String STRING_REMAIN = "Remain";

        /// <summary>
        /// 一般検体タブ
        /// </summary>
        private const String STRING_SPECIMEN = "Specimen";
        /// <summary>
        /// キャリブレータタブ
        /// </summary>
        private const String STRING_CALIBRATOR = "Calibrator";
        /// <summary>
        /// 精度管理検体タブ
        /// </summary>
        private const String STRING_CONTROL = "Control";

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormAssay()
        {
            try
            {
                InitializeComponent();

                // 拡大率切替コントロール初期化
                this.zoomPanel.ZoomStep = CarisXConst.GRID_ZOOM_STEP;

                // 拡大率変更イベント登録
                this.zoomPanel.SetZoom += this.grdSpecimen.SetGridZoom;
                this.zoomPanel.SetZoom += this.grdCalibrator.SetGridZoom;
                this.zoomPanel.SetZoom += this.grdControl.SetGridZoom;

                // リアルタイムデータ更新イベント
                Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.RealtimeData, this.onRealTimeDataChanged);

                // 洗浄液タンク状態イベント
                Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.WashSolutionTankStatus, this.onWashSolutionTankStatusChanged);

                // サンプル分注チップと反応容器の表示コントロールの初期化
                this.tcvSamplingTipCell.Initialize();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 常に最新を表示するモード
        /// </summary>
        private Boolean alwaysNewest = true;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 廃液タンクの状態の取得、設定
        /// </summary>
        private WasteStatus WasteTankStatus
        {
            get
            {
                if (SubFunction.GetMD5HashFromImageData(this.pbxWasteTankRemainIcon.DefaultImage) ==
                    SubFunction.GetMD5HashFromImageData(Oelco.CarisX.Properties.Resources.Image_WasteTank_RedLarge))
                {
                    return WasteStatus.Full;
                }
                else if (SubFunction.GetMD5HashFromImageData(this.pbxWasteTankRemainIcon.DefaultImage)
                    == SubFunction.GetMD5HashFromImageData(Oelco.CarisX.Properties.Resources.Image_WasteTank_GreenLarge))
                {
                    return WasteStatus.NotFull;
                }
                else
                {
                    return WasteStatus.None;
                }
            }
            set
            {
                switch (value)
                {
                    case WasteStatus.None:
                        this.pbxWasteTankRemainIcon.DefaultImage = Oelco.CarisX.Properties.Resources.Image_WasteTank_WhiteLarge;
                        break;
                    case WasteStatus.NotFull:
                        this.pbxWasteTankRemainIcon.DefaultImage = Oelco.CarisX.Properties.Resources.Image_WasteTank_GreenLarge;
                        break;
                    case WasteStatus.Full:
                        this.pbxWasteTankRemainIcon.DefaultImage = Oelco.CarisX.Properties.Resources.Image_WasteTank_RedLarge;
                        break;
                }
            }
        }

        /// <summary>
        /// 廃棄ボックスの状態の取得、設定
        /// </summary>
        private WasteBoxViewStatus WasteBoxStatus
        {
            get
            {
                // 単体試験時、既存コード内にリソースの画像データを使用したインスタンス同士の比較処理が発見された為、
                // 対応を追加
                if (SubFunction.GetMD5HashFromImageData(this.pbxWasteBoxRemainIcon.DefaultImage) ==
                    SubFunction.GetMD5HashFromImageData(Oelco.CarisX.Properties.Resources.Image_WasteBox_RedLarge))
                {
                    return WasteBoxViewStatus.Full;
                }
                else if (SubFunction.GetMD5HashFromImageData(this.pbxWasteBoxRemainIcon.DefaultImage) ==
                    SubFunction.GetMD5HashFromImageData(Oelco.CarisX.Properties.Resources.Image_WasteBox_YellowLarge))
                {
                    return WasteBoxViewStatus.Warning;
                }
                else if (SubFunction.GetMD5HashFromImageData(this.pbxWasteBoxRemainIcon.DefaultImage) ==
                    SubFunction.GetMD5HashFromImageData(Oelco.CarisX.Properties.Resources.Image_WasteBox_GreenLarge))
                {
                    return WasteBoxViewStatus.NotFull;
                }
                else
                {
                    return WasteBoxViewStatus.None;
                }
            }
            set
            {
                switch (value)
                {
                    case WasteBoxViewStatus.None:
                        this.pbxWasteBoxRemainIcon.DefaultImage = Oelco.CarisX.Properties.Resources.Image_WasteBox_WhiteLarge;
                        break;
                    case WasteBoxViewStatus.NotFull:
                        this.pbxWasteBoxRemainIcon.DefaultImage = Oelco.CarisX.Properties.Resources.Image_WasteBox_GreenLarge;
                        break;
                    case WasteBoxViewStatus.Warning:
                        this.pbxWasteBoxRemainIcon.DefaultImage = Oelco.CarisX.Properties.Resources.Image_WasteBox_YellowLarge;
                        break;
                    case WasteBoxViewStatus.Full:
                        this.pbxWasteBoxRemainIcon.DefaultImage = Oelco.CarisX.Properties.Resources.Image_WasteBox_RedLarge;
                        break;
                }
            }
        }

        /// <summary>
        /// 洗浄液タンクの状態の取得、設定
        /// </summary>
        private RemainStatus WashSolutionTankStatus
        {
            get
            {
                // 単体試験時、既存コード内にリソースの画像データを使用したインスタンス同士の比較処理が発見された為、
                // 対応を追加
                if (SubFunction.GetMD5HashFromImageData(this.pbxWashSolutionTankRemainIcon.DefaultImage) ==
                    SubFunction.GetMD5HashFromImageData(Oelco.CarisX.Properties.Resources.Image_WashSolution_YellowLarge))
                {
                    return RemainStatus.Middle;
                }
                else if (SubFunction.GetMD5HashFromImageData(this.pbxWashSolutionTankRemainIcon.DefaultImage) ==
                    SubFunction.GetMD5HashFromImageData(Oelco.CarisX.Properties.Resources.Image_WashSolution_RedLarge))
                {
                    return RemainStatus.Low;
                }
                else if (SubFunction.GetMD5HashFromImageData(this.pbxWashSolutionTankRemainIcon.DefaultImage) ==
                    SubFunction.GetMD5HashFromImageData(Oelco.CarisX.Properties.Resources.Image_WashSolution_GreenLarge))
                {
                    return RemainStatus.Full;
                }
                else
                {
                    return RemainStatus.Empty;
                }
            }
            set
            {
                switch (value)
                {
                    case RemainStatus.Empty:
                        this.pbxWashSolutionTankRemainIcon.DefaultImage = Oelco.CarisX.Properties.Resources.Image_WashSolution_WhiteLarge;
                        break;
                    case RemainStatus.Full:
                        this.pbxWashSolutionTankRemainIcon.DefaultImage = Oelco.CarisX.Properties.Resources.Image_WashSolution_GreenLarge;
                        break;
                    case RemainStatus.Middle:
                        this.pbxWashSolutionTankRemainIcon.DefaultImage = Oelco.CarisX.Properties.Resources.Image_WashSolution_YellowLarge;
                        break;
                    case RemainStatus.Low:
                        this.pbxWashSolutionTankRemainIcon.DefaultImage = Oelco.CarisX.Properties.Resources.Image_WashSolution_RedLarge;
                        break;
                }
            }
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// リアルタイムデータ変更イベント
        /// </summary>
        /// <remarks>
        /// 画面種別により画面情報を更新します
        /// </remarks>
        /// <param name="kind"></param>
        protected void onRealTimeDataChanged(object kind)
        {
            // Assayデータ
            switch ((RealtimeDataKind)kind)
            {
                case RealtimeDataKind.AssayData:
                    // ステータス画面 分析情報更新
                    this.loadAssayData();
                    break;
                case RealtimeDataKind.ReagentData:
                    this.loadRemainInfo();
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// 洗浄液タンク状態更新イベント
        /// </summary>
        /// <remarks>
        /// 画面種別により画面情報を更新します
        /// </remarks>
        /// <param name="kind"></param>
        protected void onWashSolutionTankStatusChanged(object kind)
        {
            // 洗浄液タンクに設定
            this.WashSolutionTankStatus = Singleton<ReagentRemainStatusInfo>.Instance
                .GetRemainStatus(ReagentKind.WashSolutionTank, (Int32)Singleton<PublicMemory>.Instance.WashSolutionTankStatus);
        }

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
            this.cgpReagentBottle.ScrollProxy = this.grdReagentBottle.ScrollProxy;
            this.cgpSpecimen.ScrollProxy = this.grdSpecimen.ScrollProxy;
            this.cgpControl.ScrollProxy = this.grdControl.ScrollProxy;
            this.cgpCalibrator.ScrollProxy = this.grdCalibrator.ScrollProxy;

            // 拡大率切替コントロール初期化
            zoomPanel.Zoom = Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.AssaySettings.GridZoom;

            // グリッド表示順
            this.grdSpecimen.SetGridColumnOrder(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.AssaySettings.SpecimenGridColOrder);
            this.grdCalibrator.SetGridColumnOrder(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.AssaySettings.CalibGridColOrder);
            this.grdControl.SetGridColumnOrder(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.AssaySettings.ControlGridColOrder);

            // グリッド列幅
            this.grdSpecimen.SetGridColmnWidth(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.AssaySettings.SpecimenGridColWidth);
            this.grdCalibrator.SetGridColmnWidth(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.AssaySettings.CalibGridColWidth);
            this.grdControl.SetGridColmnWidth(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.AssaySettings.ControlGridColWidth);
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行っています
        /// </remarks>
        protected override void setCulture()
        {
            this.Text = Oelco.CarisX.Properties.Resources.STRING_ASSAY_057;

            this.lblTitleConsumables.Text = Oelco.CarisX.Properties.Resources.STRING_ASSAY_000;

            this.cbvPretrigger1.ReagentName = Oelco.CarisX.Properties.Resources.STRING_ASSAY_001;
            this.cbvPretrigger2.ReagentName = Oelco.CarisX.Properties.Resources.STRING_ASSAY_002;
            this.cbvTrigger1.ReagentName = Oelco.CarisX.Properties.Resources.STRING_ASSAY_003;
            this.cbvTrigger2.ReagentName = Oelco.CarisX.Properties.Resources.STRING_ASSAY_004;
            this.tcvSamplingTipCell.ReagentName = Oelco.CarisX.Properties.Resources.STRING_ASSAY_005;
            this.cbvDiluent.ReagentName = Oelco.CarisX.Properties.Resources.STRING_ASSAY_006;

            this.btnConsumableDetailed.Text = Oelco.CarisX.Properties.Resources.STRING_ASSAY_008;

            this.cbvPretrigger1.RemainUnit = Oelco.CarisX.Properties.Resources.STRING_ASSAY_009;
            this.cbvPretrigger2.RemainUnit = Oelco.CarisX.Properties.Resources.STRING_ASSAY_009;
            this.cbvTrigger1.RemainUnit = Oelco.CarisX.Properties.Resources.STRING_ASSAY_009;
            this.cbvTrigger2.RemainUnit = Oelco.CarisX.Properties.Resources.STRING_ASSAY_009;
            this.tcvSamplingTipCell.RemainUnit = Oelco.CarisX.Properties.Resources.STRING_ASSAY_009;
            this.cbvDiluent.RemainUnit = Oelco.CarisX.Properties.Resources.STRING_ASSAY_010;

            this.lblTitleReagentBottle.Text = Oelco.CarisX.Properties.Resources.STRING_ASSAY_011;

            this.grdReagentBottle.DisplayLayout.Bands[0].Columns[FormAssay.STRING_ANALYTES].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_012;
            this.grdReagentBottle.DisplayLayout.Bands[0].Columns[FormAssay.STRING_LOTNO].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_013;
            this.grdReagentBottle.DisplayLayout.Bands[0].Columns[FormAssay.STRING_REMAIN].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_014;

            this.btnReagentList.Text = Oelco.CarisX.Properties.Resources.STRING_ASSAY_015;

            this.lblTitleWasteTank.Text = Oelco.CarisX.Properties.Resources.STRING_ASSAY_016;
            this.lblTitleWasteBox.Text = Oelco.CarisX.Properties.Resources.STRING_ASSAY_017;
            this.lblTitleWashSolutionTank.Text = Oelco.CarisX.Properties.Resources.STRING_ASSAY_018;

            this.tabStatusSampleRack.Tabs[FormAssay.STRING_SPECIMEN].Text = Oelco.CarisX.Properties.Resources.STRING_ASSAY_020;
            this.tabStatusSampleRack.Tabs[FormAssay.STRING_CALIBRATOR].Text = Oelco.CarisX.Properties.Resources.STRING_ASSAY_021;
            this.tabStatusSampleRack.Tabs[FormAssay.STRING_CONTROL].Text = Oelco.CarisX.Properties.Resources.STRING_ASSAY_022;

            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.SequenceNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_032;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ReceiptNumber].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_033;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.RackId].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_034;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.RackPosition].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_035;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.PatientId].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_036;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ReplicationNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_042;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.StatusString].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_044;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.RemainTime].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_043;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.Analytes].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_041;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.SpecimenMaterialType].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_040;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ManualDilution].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_048;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.AutoDilution].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_049;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.Count].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_045;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.Concentration].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_046;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.Judgement].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_047;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.MeasureDateTime].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_050;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.CalibCurveDateTime].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_051;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ReagentLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_052;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.PretriggerLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_053;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.TriggerLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_054;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.Remark].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_055;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.Comment].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_056;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ConcentrationWithoutUnit].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_069;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.AnalysisMode].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_068;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.DarkCount].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_064;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.BGCount].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_065;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ResultCount].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_066;
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ModuleNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_070;

            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.SequenceNo].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ReceiptNumber].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.SpecimenMaterialType].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.AutoDilution].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.Count].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.MeasureDateTime].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.CalibCurveDateTime].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ReagentLotNo].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.PretriggerLotNo].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.TriggerLotNo].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ConcentrationWithoutUnit].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.AnalysisMode].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転

            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.DarkCount].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.BGCount].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
            this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ResultCount].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転


            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.SequenceNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_032;
            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.RackId].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_034;
            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.RackPosition].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_035;
            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.CalibLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_037;
            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.ReplicationNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_042;
            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.StatusString].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_044;
            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.RemainTime].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_043;
            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.Analytes].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_041;
            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.Count].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_045;
            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.Concentration].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_046;
            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.MeasureDateTime].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_050;
            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.ReagentLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_052;
            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.PretriggerLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_053;
            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.TriggerLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_054;
            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.Remark].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_055;
            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.DarkCount].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_064;
            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.BGCount].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_065;
            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.ResultCount].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_066;
            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.ModuleNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_070;

            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.DarkCount].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.BGCount].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
            this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.ResultCount].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転

            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.SequenceNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_032;
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.RackId].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_034;
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.RackPosition].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_035;
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.ControlLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_038;
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.ControlName].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_039;
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.ReplicationNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_042;
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.StatusString].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_044;
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.RemainTime].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_043;
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.Analytes].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_041;
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.Count].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_045;
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.Concentration].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_046;
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.MeasureDateTime].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_050;
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.CalibCurveDateTime].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_051;
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.ReagentLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_052;
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.PretriggerLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_053;
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.TriggerLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_054;
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.Remark].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_055;
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.Comment].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_056;
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.DarkCount].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_064;
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.BGCount].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_065;
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.ResultCount].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_066;
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.ModuleNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_ASSAY_070;

            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.DarkCount].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.BGCount].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
            this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.ResultCount].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転

            this.chkAlwaysNewest.Text = Oelco.CarisX.Properties.Resources.STRING_ASSAY_067;
        }

        /// <summary>
        /// ユーザレベル設定
        /// </summary>
        /// <remarks>
        /// ユーザレベル設定します
        /// </remarks>
        protected override void setUser(Object value)
        {
            base.setUser(value);
            if (this.grdSpecimen.DisplayLayout.Bands[0].Columns.Count != 0)
            {
                //起動時はまだ列の定義が存在しない為、分岐させておく
                this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.SequenceNo].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
                this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ReceiptNumber].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
                this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.SpecimenMaterialType].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
                this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.AutoDilution].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
                this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.Count].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
                this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.MeasureDateTime].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
                this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.CalibCurveDateTime].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
                this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ReagentLotNo].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
                this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.PretriggerLotNo].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
                this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.TriggerLotNo].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
                this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ConcentrationWithoutUnit].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転
                this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.AnalysisMode].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult);//Hiddenなので!で符号逆転

                this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.DarkCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
                this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.BGCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
                this.grdSpecimen.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ResultCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
            }

            if (this.grdCalibrator.DisplayLayout.Bands[0].Columns.Count != 0)
            {
                //起動時はまだ列の定義が存在しない為、分岐させておく
                this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.DarkCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
                this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.BGCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
                this.grdCalibrator.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.ResultCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
            }

            if (this.grdControl.DisplayLayout.Bands[0].Columns.Count != 0)
            {
                //起動時はまだ列の定義が存在しない為、分岐させておく
                this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.DarkCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
                this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.BGCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
                this.grdControl.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.ResultCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
            }
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// フォームの読み込みイベント
        /// </summary>
        /// <remarks>
        /// 画面情報の読込、初期化を行います
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void FormAssay_Load(object sender, EventArgs e)
        {
            Singleton<ReagentDB>.Instance.LoadDB();
            Singleton<ReagentHistoryDB>.Instance.LoadDB();
            Singleton<SpecimenAssayDB>.Instance.LoadDB();
            Singleton<CalibratorAssayDB>.Instance.LoadDB();
            Singleton<ControlAssayDB>.Instance.LoadDB();

            // 常に最新を表示するモード
            this.chkAlwaysNewest.Checked = this.alwaysNewest;

            this.loadRemainInfo();
            this.loadAssayData();
        }

        /// <summary>
        /// 消耗品詳細ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 操作履歴登録し、消耗品詳細表示ダイアログを表示します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnConsumableDetailed_Click(object sender, EventArgs e)
        {
            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_025 });

            DlgConsumableDetailed dlg = new DlgConsumableDetailed();
            dlg.ShowDialog();
        }


        /// <summary>
        /// 試薬リスト確認ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 操作履歴登録し、試薬（交換）テーブルダイアログを表示します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnReagentList_Click(object sender, EventArgs e)
        {
            // 操作履歴登録          
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_026 });

            DlgTurnTable dlg = new DlgTurnTable();
            dlg.DispMode = DlgTurnTable.TurnTableDispMode.Check;
            dlg.ShowDialog();
        }

        /// <summary>
        /// 残量情報の読み込み
        /// </summary>
        /// <remarks>
        /// 残量情報の読み込みを行います
        /// </remarks>
        public void loadRemainInfo()
        {
            Int32 selectModuleId = CarisXSubFunction.ModuleIndexToModuleId(Singleton<PublicMemory>.Instance.moduleIndex);

            var moduleReagentData = Singleton<ReagentDB>.Instance.GetData(null, selectModuleId);
            var rackReagentData = Singleton<ReagentDB>.Instance.GetData(null, (int)RackModuleIndex.RackTransfer);

            // プレトリガボトル表示
            this.cbvPretrigger1.setBottleRemain(selectModuleId);
            this.cbvPretrigger2.setBottleRemain(selectModuleId);

            // トリガボトル表示
            this.cbvTrigger1.setBottleRemain(selectModuleId);
            this.cbvTrigger2.setBottleRemain(selectModuleId);

            // 希釈液ボトル表示
            this.cbvDiluent.setBottleRemain(selectModuleId);

            // サンプル分注チップ/反応容器表示
            this.tcvSamplingTipCell.setTipCellRemain(selectModuleId);

            // 廃液タンク表示
            var wasteTankData = rackReagentData.FirstOrDefault(reagentDataItem => reagentDataItem.ReagentKind == (Int32)ReagentKind.WasteTank);
            if (wasteTankData != null)
            {
                this.WasteTankStatus = Singleton<ReagentRemainStatusInfo>.Instance
                    .GetWasteStatus(ReagentKind.WasteTank, (wasteTankData.Remain ?? 0), (wasteTankData.IsUse ?? false));
            }

            // 廃棄ボックス表示
            var wasteBoxData = moduleReagentData.FirstOrDefault((reagentDataItem) => reagentDataItem.ReagentKind == (Int32)ReagentKind.WasteBox);
            if (wasteBoxData != null)
            {
                this.WasteBoxStatus = Singleton<ReagentRemainStatusInfo>.Instance
                    .GetWasteBoxViewStatus(ReagentKind.WasteBox, (wasteBoxData.Remain ?? 0), (wasteBoxData.IsUse ?? false));
            }

            // 試薬ボトル表示
            BindingList<ReagentRemainData> reagentList;
            if (this.grdReagentBottle.DataSource == null)
            {
                //初回表示でまだデータソースが設定されていない場合
                //新たにデータソースのインスタンスを作成する
                reagentList = new BindingList<ReagentRemainData>();
            }
            else
            {
                //既に表示済みでデータソースが設定済みの場合
                //既に設定されているデータソースのインスタンスをコピーする
                //以降はコピーした変数の内容を変更していけば、自動で反映される
                reagentList = (BindingList<ReagentRemainData>)this.grdReagentBottle.DataSource;
                reagentList.Clear();    //一度中身をクリア
            }
            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList.ForEach((protocol) =>
            {
                IEnumerable<ReagentData> data = moduleReagentData.Where((remainData)
                    => remainData.ReagentKind == (Int32)ReagentKind.Reagent &&
                       remainData.ReagentCode == protocol.ReagentCode &&
                       (remainData.ReagentType == (Int32)ReagentType.M || remainData.ReagentType == (Int32)ReagentType.R1R2) &&
                       !String.IsNullOrEmpty(remainData.LotNo));
                if (data != null && data.Count() != 0)
                {
                    foreach (var lotData in data.GroupBy(lotDataSet => String.Format("{0,-8}{1,-2}", lotDataSet.LotNo, lotDataSet.MakerCode)))
                    {
                        // 試薬保冷庫1ポジション毎に全て(M、R1、R2)のロット番号が一致するものだけ抽出
                        var reagentRemainData = lotData.AsEnumerable()
                            .GroupBy(lotNameData => (lotNameData.PortNo - 1) / 3)
                            .Where(portData => portData.Count() == CarisXConst.REAGENT_BOTTLE_SETTING_POSITION_COUNT)
                            .SelectMany(grpData => grpData.AsEnumerable());

                        // 分析項目毎でロット番号が一致するもののみリストに追加
                        if (reagentRemainData.Count() > 0)
                        {
                            reagentList.Add(new ReagentRemainData(reagentRemainData, protocol.ProtocolName));
                        }
                    }
                }
            });
            if (this.grdReagentBottle.DataSource == null)
            {
                //初回表示でまだデータソースが設定されていない場合
                //編集した変数をデータソースに設定する
                this.grdReagentBottle.DataSource = reagentList;
            }

            // 試薬が有効期限切れの場合、試薬名の文字色を変更(赤)
            // 検量線がない場合、ロット番号の文字色を変更（赤）
            // 検量線が有効期限切れの場合、ロット番号の文字色を変更（橙）
            foreach (var row in this.grdReagentBottle.DisplayLayout.Rows)
            {
                // 試薬の有効期限切れチェック
                if (((ReagentRemainData)row.ListObject).IsExpired())
                {
                    // 有効期限切れ
                    row.Cells[ReagentRemainData.EXPIRATIONDATE_COLUMNNAME].Appearance.ForeColor = Color.Red;
                    row.Cells[ReagentRemainData.EXPIRATIONDATE_COLUMNNAME].ActiveAppearance.ForeColor = Color.Red;
                    row.Cells[ReagentRemainData.EXPIRATIONDATE_COLUMNNAME].SelectedAppearance.ForeColor = Color.Red;
                }
                else
                {
                    // 有効期限内
                    row.Cells[ReagentRemainData.EXPIRATIONDATE_COLUMNNAME].Appearance.ForeColor = Color.Black;
                    row.Cells[ReagentRemainData.EXPIRATIONDATE_COLUMNNAME].ActiveAppearance.ForeColor = Color.Black;
                    row.Cells[ReagentRemainData.EXPIRATIONDATE_COLUMNNAME].SelectedAppearance.ForeColor = Color.Black;
                }

                // 検量線の有無チェック
                ReagentRemainData.ResultCheckCalibCruve resultCheckCalibCurve = ((ReagentRemainData)row.ListObject ).CheckCalibCurve(selectModuleId);
                switch(resultCheckCalibCurve)
                {
                    // 検量線あり
                    case ReagentRemainData.ResultCheckCalibCruve.OK:
                        row.Cells[ReagentRemainData.NOCALIBCURVE_COLUMNNAME].Appearance.ForeColor = Color.Black;
                        row.Cells[ReagentRemainData.NOCALIBCURVE_COLUMNNAME].ActiveAppearance.ForeColor = Color.Black;
                        row.Cells[ReagentRemainData.NOCALIBCURVE_COLUMNNAME].SelectedAppearance.ForeColor = Color.Black;
                        break;
                    // 検量線なし
                    case ReagentRemainData.ResultCheckCalibCruve.NoCalibCurve:
                        row.Cells[ReagentRemainData.NOCALIBCURVE_COLUMNNAME].Appearance.ForeColor = Color.Red;
                        row.Cells[ReagentRemainData.NOCALIBCURVE_COLUMNNAME].ActiveAppearance.ForeColor = Color.Red;
                        row.Cells[ReagentRemainData.NOCALIBCURVE_COLUMNNAME].SelectedAppearance.ForeColor = Color.Red;
                        break;
                    // 検量線の有効期限切れ
                    case ReagentRemainData.ResultCheckCalibCruve.Expired:
                        row.Cells[ReagentRemainData.NOCALIBCURVE_COLUMNNAME].Appearance.ForeColor = Color.Orange;
                        row.Cells[ReagentRemainData.NOCALIBCURVE_COLUMNNAME].ActiveAppearance.ForeColor = Color.Orange;
                        row.Cells[ReagentRemainData.NOCALIBCURVE_COLUMNNAME].SelectedAppearance.ForeColor = Color.Orange;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 検定データ読込
        /// </summary>
        /// <remarks>
        /// 検定データを読込します
        /// </remarks>
        public void loadAssayData()
        {
            this.loadAndSetData((from v in Singleton<SpecimenAssayDB>.Instance.GetData()
                                 orderby v.GetUniqueNo() descending, v.ReplicationNo descending
                                 select v).ToList(), this.grdSpecimen);
            this.loadAndSetData((from v in Singleton<CalibratorAssayDB>.Instance.GetData()
                                 orderby v.GetUniqueNo() descending, v.ReplicationNo descending
                                 select v).ToList(), this.grdCalibrator);
            this.loadAndSetData((from v in Singleton<ControlAssayDB>.Instance.GetData()
                                 orderby v.GetUniqueNo() descending, v.ReplicationNo descending
                                 select v).ToList(), this.grdControl);
        }

        /// <summary>
        /// 検定データリスト読込
        /// </summary>
        /// <remarks>
        /// 検定データリストを読込します
        /// </remarks>
        /// <typeparam name="Data"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="grid"></param>
        private void loadAndSetData<Data>(List<Data> dataList, CustomGrid grid)
            where Data : DataRowWrapperBase
        {
            if (!grid.IsDisposed)
            {
                if (grid.DataSource == null)
                {
                    grid.DataSource = new BindingList<Data>(dataList);
                }
                else
                {
                    var bindList = grid.DataSource as BindingList<Data>;
                    if (bindList != null)
                    {
                        bindList.RaiseListChangedEvents = false;
                        bindList.Clear();
                        dataList.ForEach((data) => bindList.Add(data));
                        bindList.RaiseListChangedEvents = true;
                        bindList.ResetBindings();
                    }
                    else
                    {
                        grid.DataSource = new BindingList<Data>(dataList);
                    }

                    if (( this.alwaysNewest == true ) && ( grid.Rows.Count > 0 ))
                    {
                        // 選択状態の解除
                        grid.ActiveRow = null;
                        grid.Selected.Rows.Clear();

                        // 先頭行を設定
                        grid.Rows[0].Activate();
                        grid.Rows[0].Selected = true;
                    }
                }
            }
        }

        /// <summary>
        /// FormClosedイベント
        /// </summary>
        /// <remarks>
        /// 拡大率UI設定、グリッド表示順UI設定、グリッド列幅UI設定の保存します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormAssay_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 拡大率UI設定保存
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.AssaySettings.GridZoom = this.zoomPanel.Zoom;
            // グリッド表示順UI設定保存
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.AssaySettings.SpecimenGridColOrder = this.grdSpecimen.GetGridColumnOrder();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.AssaySettings.CalibGridColOrder = this.grdCalibrator.GetGridColumnOrder();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.AssaySettings.ControlGridColOrder = this.grdControl.GetGridColumnOrder();
            // グリッド列幅UI設定保存
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.AssaySettings.SpecimenGridColWidth = this.grdSpecimen.GetGridColmnWidth();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.AssaySettings.CalibGridColWidth = this.grdCalibrator.GetGridColmnWidth();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.AssaySettings.ControlGridColWidth = this.grdControl.GetGridColmnWidth();
        }

        /// <summary>
        /// 分析状態表示グリッドセルクリックイベント
        /// </summary>
        /// <remarks>
        /// リマークセルクリック時、リマーク詳細ダイアログ表示します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void grdAssay_ClickCell(object sender, ClickCellEventArgs e)
        {
            // リマークセルクリック時、リマーク詳細ダイアログ表示
            Type type = e.Cell.Row.ListObject.GetType();
            if (type == typeof(SpecimenAssayData))
            {
                if (e.Cell.Column.Key == SpecimenAssayData.DataKeys.Remark)
                {
                    DlgRemarkDetail.Show(((SpecimenAssayData)e.Cell.Row.ListObject).GetRemarkId());
                }
            }
            else if (type == typeof(CalibratorAssayData))
            {
                if (e.Cell.Column.Key == CalibratorAssayData.DataKeys.Remark)
                {
                    DlgRemarkDetail.Show(((CalibratorAssayData)e.Cell.Row.ListObject).GetRemarkId());
                }
            }
            else if (type == typeof(ControlAssayData))
            {
                if (e.Cell.Column.Key == ControlAssayData.DataKeys.Remark)
                {
                    DlgRemarkDetail.Show(((ControlAssayData)e.Cell.Row.ListObject).GetRemarkId());
                }
            }
        }

        private void grdReagentBottle_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            AvailableTestsEditor editor = new AvailableTestsEditor();
            e.Layout.Bands[0].Columns["Remain"].Editor = editor;
        }

        private void grdReagentBottle_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells[0].Text.Length >= 9)
            {
                e.Row.Cells[0].Appearance.FontData.SizeInPoints = 10;
            }
        }

        /// <summary>
        /// 常に最新を表示チェックボックス状態変化イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAlwaysNewest_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAlwaysNewest.Checked)
            {
                // 常に最新表示
                this.alwaysNewest = true;
            }
            else
            {
                // 手動
                this.alwaysNewest = false;
            }
        }

        #endregion
    }

    /// <summary>
    /// 試薬残量情報表示データ
    /// </summary>
    public class ReagentRemainData
    {
        /// <summary>
        /// 検量線チェック結果
        /// </summary>
        public enum ResultCheckCalibCruve
        {
            /// <summary>
            /// OK
            /// </summary>
            OK = 0,

            /// <summary>
            /// 検量線なし
            /// </summary>
            NoCalibCurve,

            /// <summary>
            /// 期限切れ
            /// </summary>
            Expired,
        }

        /// <summary>
        /// 試薬情報
        /// </summary>
        private IEnumerable<ReagentData> data;

        /// <summary>
        /// 有効期限切れ文字色スイッチ列
        /// </summary>
        public static String EXPIRATIONDATE_COLUMNNAME = "Analytes";

        /// <summary>
        /// 検量線無し文字色スイッチ列
        /// </summary>
        public static String NOCALIBCURVE_COLUMNNAME = "LotNo";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data">表示対象試薬情報</param>
        /// <param name="protocolName">試薬対応プロトコル名</param>
        public ReagentRemainData(IEnumerable<ReagentData> data, String protocolName)
        {
            this.data = data;
            this.Analytes = protocolName;
        }

        /// <summary>
        /// 分析項目名の取得、設定
        /// </summary>
        public String Analytes
        {
            get;
            set;
        }

        /// <summary>
        /// 試薬ロット番号の取得
        /// </summary>
        public String LotNo
        {
            get
            {
                return this.data.Select((data) => data.LotNo).Distinct().SingleOrDefault();
            }
        }

        /// <summary>
        /// 残量(残テスト数)の取得
        /// </summary>
        public Int32?[] Remain
        {
            get
            {
                Int32 remain = 0;
                Int32 capacity = 0;

                // 各R1,R2,Mの1セットに対して算出した最小テスト数の全ポート合計とする
                foreach (var reagPortSetGroupe in from dat in this.data group dat by (dat.PortNo - 1) / 3)
                {
                    if (reagPortSetGroupe.Count() > 0 && reagPortSetGroupe.First().ReagentCode.HasValue && reagPortSetGroupe.First().ReagentCode.Value > 0)
                    {
                        var proto = Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList.FirstOrDefault((protocol) => protocol.ReagentCode == reagPortSetGroupe.First().ReagentCode && data.Select((code) => code.ReagentCode).Contains(protocol.ReagentCode));
                        if (proto.R1DispenseVolume > 0)
                        {
                            //R1分注量が0でない場合
                            var reagentR1R2M = new[] { proto.R1DispenseVolume, proto.R2DispenseVolume, proto.MReagDispenseVolume };
                            var validData = reagPortSetGroupe.GroupBy(reagentData => (reagentData.PortNo - 1) / 3)
                                .Where(portData => portData.Count() == 3)
                                .SelectMany(grp => grp);
                            var lotGroupReagent = validData.GroupBy(reagentData => (reagentData.PortNo - 1) % 3).OrderBy((grp) => grp.Key);
                            var reagentRemainList = lotGroupReagent.Select(
                                group => group.Sum(
                                    reagData => (reagentR1R2M[group.Key.Value] > 0) ? (reagData.Remain.Value / reagentR1R2M[group.Key.Value]) : 0));
                            remain += reagentRemainList.Min();

                            //
                            var reagentRemainMaxList = lotGroupReagent.Select(
                                group => group.Sum(
                                    reagData => (reagentR1R2M[group.Key.Value] > 0) ? reagData.Capacity : 0));
                            capacity += reagentRemainMaxList.Max();

                        }
                        else
                        {
                            //R1分注量が0の場合
                            var reagentR1R2M = new[] { 1, proto.R2DispenseVolume, proto.MReagDispenseVolume };//R1の値は1に設定され、計算された値は最大値になりますが、結果には影響しません。
                            var validData = reagPortSetGroupe.GroupBy(reagentData => (reagentData.PortNo - 1) / 3)
                                .Where(portData => portData.Count() == 3)
                                .SelectMany(grp => grp);
                            var lotGroupReagent = validData.GroupBy(reagentData => (reagentData.PortNo - 1) % 3).OrderBy(grp => grp.Key);
                            var reagentRemainList = lotGroupReagent.Select(
                                group => group.Sum(
                                    reagData => (reagentR1R2M[group.Key.Value] > 0) ? (reagData.Remain.Value / reagentR1R2M[group.Key.Value]) : 0));
                            var reagentRemainMaxList = lotGroupReagent.Select(
                                group => group.Sum(
                                    reagData => (reagentR1R2M[group.Key.Value] > 0) ? reagData.Capacity : 0));

                            int[] nRestult = reagentRemainList.ToArray();
                            if (nRestult[1] > nRestult[2])//R2とRMのテスト数を比較する。最も小さいものが表示された効果値である.nRestult [1]はR2であり、nRestult [2]はMである。
                            {
                                remain += nRestult[2];
                            }
                            else
                            {
                                remain += nRestult[1];
                            }

                            int[] aryRemainMax = reagentRemainMaxList.ToArray();
                            if (nRestult[1] > nRestult[2])
                            {
                                capacity += aryRemainMax[1];
                            }
                            else
                            {
                                capacity += aryRemainMax[2];
                            }
                        }
                    }
                }

                return new Int32?[] { remain, capacity };
            }
        }

        /// <summary>
        /// 有効期限切れ
        /// </summary>
        /// <remarks>
        /// 有効期限切れかどうかを返します
        /// </remarks>
        public Boolean IsExpired()
        {
            return (data.Count() > 0) ? data.First().ExpirationDate.HasValue && data.First().ExpirationDate < DateTime.Now : false;
        }


        /// <summary>
        /// 検量線の確認
        /// </summary>
        /// <returns>検量線チェック結果</returns>
        public ResultCheckCalibCruve CheckCalibCurve(Int32 moduleNo)
        {
            // チェック結果を「検量線なし」で初期化
            ResultCheckCalibCruve result = ResultCheckCalibCruve.NoCalibCurve;

            // データ件数チェック
            if (data.Count() > 0)
            {
                // 分析項目情報取得
                MeasureProtocol measProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo( (int)data.First().ReagentCode );

                // 分析項目情報のnullチェック
                if (measProtocol != null)
                {
                    // 検量線データ抽出
                    var calibCurveList = Singleton<CalibrationCurveDB>.Instance.GetDataExcludeMasterCurve( measProtocol.ProtocolIndex, data.First().LotNo, moduleNo);

                    // 検量線データ件数チェック
                    if (calibCurveList.Count > 0)
                    {
                        // 成立日を文字列から日付に変換
                        DateTime approvalDate = DateTime.Now;
                        Boolean parseResult = DateTime.TryParse( calibCurveList.First().Key, out approvalDate );
                        if (parseResult)
                        {
                            // 変換成功

                            // 検量線の有効期限切れチェック
                            if (approvalDate.Date.AddDays( measProtocol.ValidityOfCurve ) < DateTime.Now.Date)
                            {
                                // 有効期限切れ
                                result = ResultCheckCalibCruve.Expired;
                            }
                            else
                            {
                                // 有効期限内
                                result = ResultCheckCalibCruve.OK;
                            }
                        }
                        else
                        {
                            // 変換失敗
                        }
                    }
                    else
                    {
                        // 検量線データがない
                    }
                }
            }

            return result;
        }
    }
}

public class AvailableTestsEditor : ControlContainerEditor
{
    #region Private Members
    private RemainBar remainBar;
    #endregion

    #region Constructor
    public AvailableTestsEditor()
    {
        this.remainBar = new RemainBar();
        this.remainBar.Initialize();
        this.remainBar.Value = 0;
        this.remainBar.Size = new System.Drawing.Size(220, 44);

        // コントロールをレンダリング コントロールとして設定します
        this.RenderingControl = remainBar;
    }
    #endregion

    #region override RendererValue
    protected override object RendererValue
    {
        get
        {
            return new Int32?[] { this.remainBar.Value, this.remainBar.Maximum };
        }
        set
        {
            Int32?[] renderValue = (Int32?[])value;

            this.remainBar.Maximum = (int)renderValue[1];
            this.remainBar.Value = (int)renderValue[0];
        }
    }
    #endregion


}
