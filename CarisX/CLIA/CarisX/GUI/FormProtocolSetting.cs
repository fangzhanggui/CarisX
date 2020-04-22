using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Oelco.Common.GUI;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.DB;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Comm;
using Infragistics.Win.UltraWinEditors;
using Oelco.CarisX.Status;
using Oelco.CarisX.Const;
using Infragistics.Win;
using Oelco.CarisX.Parameter.AnalyteGroup;
using Oelco.Common.Comm;
using Oelco.CarisX.Common;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 分析項目設定画面クラス
    /// </summary>
    public partial class FormProtocolSetting : FormChildBase
    {
        #region [定数定義]

        /// <summary>
        /// 閉じる
        /// </summary>
        public const String CLOSE = "Close";

        /// <summary>
        /// 保存
        /// </summary>
        public const String SAVE = "Save";

        /// <summary>
        /// 初期化
        /// </summary>
        public const String INITIALIZE = "Initialize";

        #endregion

        #region [クラス変数定義]

        //private static FormProtocolSetting instance;

        #endregion


        private Dictionary<Int32, String> diluCalibOrControlKindList = new Dictionary<Int32, String>()
        {
            {0, Oelco.CarisX.Properties.Resources.STRING_NODILU},
            {1, Oelco.CarisX.Properties.Resources.STRING_DILUCALIB},
            {2, Oelco.CarisX.Properties.Resources.STRING_DILUCONTROL},
            {3, Oelco.CarisX.Properties.Resources.STRING_DILUALL}
        };


        #region [インスタンス変数定義]

        /// <summary>
        /// 分析シーケンス
        /// </summary>
        private Dictionary<MeasureProtocol.AssaySequenceKind, String> assaySequenceKindList = new Dictionary<MeasureProtocol.AssaySequenceKind, String>()
        {
            {MeasureProtocol.AssaySequenceKind.OneStep,                     Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_075},
            {MeasureProtocol.AssaySequenceKind.TwoStep,                     Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_076},
            {MeasureProtocol.AssaySequenceKind.TwoStepMinus,                Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_077},
            {MeasureProtocol.AssaySequenceKind.OnePointFiveStep,            Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_078},
            {MeasureProtocol.AssaySequenceKind.OnePointFiveStepRM,          Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_113},
        };

        /// <summary>
        /// 前処理シーケンス
        /// </summary>
        private Dictionary<MeasureProtocol.PreProcessSequenceKind, String> preProcessSequenceList = new Dictionary<MeasureProtocol.PreProcessSequenceKind, String>()
        {
            {MeasureProtocol.PreProcessSequenceKind.None,   Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_079},
            {MeasureProtocol.PreProcessSequenceKind.SR1,    Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_080},
            {MeasureProtocol.PreProcessSequenceKind.ST1,    Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_081},
            {MeasureProtocol.PreProcessSequenceKind.ST1T2,  Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_082},
            {MeasureProtocol.PreProcessSequenceKind.ST1SR1, Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_083},
            {MeasureProtocol.PreProcessSequenceKind.ST1ST2, Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_084}
        };

        /// <summary>
        /// 自動希釈再検条件
        /// </summary>
        private Dictionary<MeasureProtocol.AutoDilutionReTestRatioKind, String> autoDilutionRetestConditionDilutionRatioList = new Dictionary<MeasureProtocol.AutoDilutionReTestRatioKind, String>()
        {
            {MeasureProtocol.AutoDilutionReTestRatioKind.x10, Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_120},
            {MeasureProtocol.AutoDilutionReTestRatioKind.x20, Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_121},
            {MeasureProtocol.AutoDilutionReTestRatioKind.x100, Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_122},
            {MeasureProtocol.AutoDilutionReTestRatioKind.x200, Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_123},
            {MeasureProtocol.AutoDilutionReTestRatioKind.x400, Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_124},
            {MeasureProtocol.AutoDilutionReTestRatioKind.x1000, Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_125},
            {MeasureProtocol.AutoDilutionReTestRatioKind.x2000, Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_126},
            {MeasureProtocol.AutoDilutionReTestRatioKind.x4000, Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_127},
            {MeasureProtocol.AutoDilutionReTestRatioKind.x8000, Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_128},
        };

        /// <summary>
        /// 後希釈倍率
        /// </summary>
        private Dictionary<MeasureProtocol.DilutionRatioKind, String> DilutionRatioList = new Dictionary<MeasureProtocol.DilutionRatioKind, String>()
        {
            {MeasureProtocol.DilutionRatioKind.x1,   Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_119},
            {MeasureProtocol.DilutionRatioKind.x10,   Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_120},
            {MeasureProtocol.DilutionRatioKind.x20,   Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_121},
            {MeasureProtocol.DilutionRatioKind.x100,  Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_122},
            {MeasureProtocol.DilutionRatioKind.x200,  Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_123},
            {MeasureProtocol.DilutionRatioKind.x400,  Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_124},
            {MeasureProtocol.DilutionRatioKind.x1000, Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_125},
            {MeasureProtocol.DilutionRatioKind.x2000,  Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_126},
            {MeasureProtocol.DilutionRatioKind.x4000,  Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_127},
            {MeasureProtocol.DilutionRatioKind.x8000,  Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_128},
        };

        /// <summary>
        /// キャリブレーションタイプ
        /// </summary>
        private Dictionary<MeasureProtocol.CalibrationType, String> calibrationTypeList = new Dictionary<MeasureProtocol.CalibrationType, String>()
        {
            {MeasureProtocol.CalibrationType.Spline,                Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_089},
            {MeasureProtocol.CalibrationType.LogitLog,              Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_090},
            {MeasureProtocol.CalibrationType.FourParameters,        Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_091},
            {MeasureProtocol.CalibrationType.CutOff,                Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_092},
            {MeasureProtocol.CalibrationType.INH,                   Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_093},
            {MeasureProtocol.CalibrationType.DoubleLogarithmic1,    Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_114},
            {MeasureProtocol.CalibrationType.DoubleLogarithmic2,    Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_115}
        };

        /// <summary>
        /// フルキャリブレーションポイント数
        /// </summary>
        private Dictionary<Int32, String> numberOfCalibrationPointsList = new Dictionary<Int32, String>()
        {
                {2,Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_094},
                {3,Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_095},
                {4,Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_096},
                {5,Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_097},
                {6,Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_098},
                {7,Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_099},
                {8,Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_100}
        };

        /// <summary>
        /// マスターキャリブレーションポイント数
        /// </summary>
        private Dictionary<Int32, String> masterCalibrationPointsDic = new Dictionary<int, string>();

        /// <summary>
        /// キャリブレーション方法
        /// </summary>
        private Dictionary<MeasureProtocol.CalibrationMethod, String> calibrationMethodList = new Dictionary<MeasureProtocol.CalibrationMethod, String>()
        {
                {MeasureProtocol.CalibrationMethod.FullCalibration,     Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_101},
                {MeasureProtocol.CalibrationMethod.MasterCalibration,   Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_102}
        };

        /// <summary>
        /// 分析項目番号
        /// </summary>
        private Int32 protocolNo = -1;

        /// <summary>
        /// 編集中プロトコル情報
        /// </summary>
        /// <remarks>
        /// 初期化等で現在と違う元ファイルの情報を使用する際等を考慮する為、
        /// この画面で適用されているプロトコル情報として保持する。
        /// </remarks>
        MeasureProtocol protocolNowEdit = null;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="protocolNo">設定対象の分析項目番号</param>
        public FormProtocolSetting(Int32 protocolNo)
        {
            InitializeComponent();

            // 分析項目番号の保持
            this.protocolNo = protocolNo;

            // コマンドバーのイベント追加
            this.tlbCommandBar.Tools[SAVE].ToolClick += (sender, e) => this.saveParameter();
            this.tlbCommandBar.Tools[CLOSE].ToolClick += (sender, e) => this.closeParameter();
            this.tlbCommandBar.Tools[INITIALIZE].ToolClick += (sender, e) => this.InitializeParameter();

            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SystemStatusChanged, this.onSystemStatusChanged);
            this.onSystemStatusChanged(null);
            this.setUser(null);

            //ツールバーの右ボタンを設定することはできません。（设置ToolBar的右键功能不可用）
            this.tlbCommandBar.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.tlbCommandBar_BeforeToolbarListDropdown);

        }

        //ツールバーの右ボタンを設定することはできません。（设置ToolBar的右键功能不可用）
        private void tlbCommandBar_BeforeToolbarListDropdown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventArgs e)
        {
            e.Cancel = true;
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
            // 分析項目情報の読み込み
            this.loadProtocolData();

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            this.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_107;

            this.tlbCommandBar.Tools[SAVE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_001;
            this.tlbCommandBar.Tools[CLOSE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_012;
            this.tlbCommandBar.Tools[INITIALIZE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_013;

            //
            // コンボボックス設定
            //

            // キャリブレータと品質管理が希釈されているかどうか（校准品、质控品是否稀释）
            this.cmbCalibrationControlDilu.Items.Clear();
            this.cmbCalibrationControlDilu.DataSource = this.diluCalibOrControlKindList.ToList();
            this.cmbCalibrationControlDilu.ValueMember = "Key";
            this.cmbCalibrationControlDilu.DisplayMember = "Value";
            this.cmbCalibrationControlDilu.SelectedIndex = 0;

            // アッセイシーケンス
            this.cmbAssaySequence.Items.Clear();
            this.cmbAssaySequence.DataSource = this.assaySequenceKindList.ToList();
            this.cmbAssaySequence.ValueMember = "Key";
            this.cmbAssaySequence.DisplayMember = "Value";
            this.cmbAssaySequence.SelectedIndex = 0;

            // 前処理シーケンス
            this.cmbPreProcessSequence.Items.Clear();
            this.cmbPreProcessSequence.DataSource = this.preProcessSequenceList.ToList();
            this.cmbPreProcessSequence.ValueMember = "Key";
            this.cmbPreProcessSequence.DisplayMember = "Value";
            this.cmbPreProcessSequence.SelectedIndex = 0;

            // 自動希釈再検条件
            this.cmbAutoDilutionRetestConditionDilutionRatio.Items.Clear();
            this.cmbAutoDilutionRetestConditionDilutionRatio.DataSource = this.autoDilutionRetestConditionDilutionRatioList.ToList();
            this.cmbAutoDilutionRetestConditionDilutionRatio.ValueMember = "Key";
            this.cmbAutoDilutionRetestConditionDilutionRatio.DisplayMember = "Value";
            this.cmbAutoDilutionRetestConditionDilutionRatio.SelectedIndex = 0;

            // 後希釈倍率
            this.cmbDilutionRatio.Items.Clear();
            this.cmbDilutionRatio.DataSource = this.DilutionRatioList.ToList();
            this.cmbDilutionRatio.ValueMember = "Key";
            this.cmbDilutionRatio.DisplayMember = "Value";
            this.cmbDilutionRatio.SelectedIndex = 0;

            // キャリブレーションタイプ
            this.cmbCalibrationType.Items.Clear();
            this.cmbCalibrationType.DataSource = this.calibrationTypeList.ToList();
            this.cmbCalibrationType.ValueMember = "Key";
            this.cmbCalibrationType.DisplayMember = "Value";
            this.cmbCalibrationType.SelectedIndex = 0;

            // フルキャリブレーションポイント数
            this.cmbNumberOfCalibrationPoints.Items.Clear();
            this.cmbNumberOfCalibrationPoints.DataSource = this.numberOfCalibrationPointsList.ToList();
            this.cmbNumberOfCalibrationPoints.ValueMember = "Key";
            this.cmbNumberOfCalibrationPoints.DisplayMember = "Value";
            this.cmbNumberOfCalibrationPoints.SelectedIndex = 0;

            // キャリブレーション方法
            this.cmbCalibrationMethod.Items.Clear();
            this.cmbCalibrationMethod.DataSource = this.calibrationMethodList.ToList();
            this.cmbCalibrationMethod.ValueMember = "Key";
            this.cmbCalibrationMethod.DisplayMember = "Value";
            this.cmbCalibrationMethod.SelectedIndex = 0;

            // ラベル
            this.lblTitleCalibrationDilu.Text = Oelco.CarisX.Properties.Resources.STRING_DILUCALIBANDCONTROLTITLE;
            this.lblTitleAnalyteNo.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_000;
            this.lblTitleAnalytesName.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_001;
            this.lblTitleReagentName.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_002;
            this.lblTitleReagentCode.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_003;
            this.tbpProtocolRegular.Tab.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_004;
            this.tbpProtocolDetailed.Tab.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_005;
            this.lblMeasurementTimes.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_006;
            this.lblCalibAssayCondition.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_007;
            this.lblNegativePositiveValue.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_008;
            this.lblTitleSpecimenMultiMeasure.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_009;
            this.lblTitleControlMeasureTimes.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_010;
            this.lblTitleCalibratorMeasureTimes.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_011;
            this.lblUnitSpecimentMultiMeasure.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_012;
            this.lblUnittControlMeasureTimes.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_012;
            this.lblUnitCalibratorMeasureTimes.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_012;
            this.lblLimitSpecimenMultiMeasure.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_013;
            this.lblLimitControlMeasureTimes.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_014;
            this.lblLimitCalibratorMeasureTimes.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_015;
            this.lblTitleExpirationDateOfTheCalibCurve.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_016;
            this.lblUnitExpirationDateOfTheCalibCurve.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_017;
            this.lblTitlePositiveValue.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_018;
            this.lblNegativeValue.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_019;

            this.lblAssayCondition1.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_020;
            this.lblAutoDilutionRetestCondition.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_021;
            this.lblAutoRetestCondition.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_022;
            this.lblAssayCondition2.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_023;
            this.lblCalibAssayConditionDetailed.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_024;
            this.lblDispensingCondtion.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_025;
            this.lblTitleAssaySequence.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_026;
            this.lblTitleDilutionRatio.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_037;
            this.lblTitlePreProcessSequence.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_027;
            this.lblTitleSpecimenType.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_028;
            this.chkSpecimenTypeSerumOrPlasma.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_029;
            this.chkSpecimenTypeUrine.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_030;
            this.lblTitleRetestRange.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_129;
            this.chkRetestLowLimit.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_130;
            this.chkRetestMiddleLimit.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_131;
            this.chkRetestUpperLimit.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_132;
            this.lblTitleAutoDilutionRetestUse.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_031;
            this.optAutoDilutionRetestUse.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_032;
            this.optAutoDilutionRetestUse.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_033;
            this.lblTitleAutoRetestUse.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_034;
            this.optAutoRetestUse.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_032;
            this.optAutoRetestUse.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_033;
            this.lblTitleAutoDilutionRetestConditionLowerLimit.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_035;
            this.lblTitleAutoDilutionRetestConditionUpperLimit.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_036;
            this.lblTitleAutoDilutionRetestConditionDilutionRatio.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_037;
            this.lblTitleAutoRetestConditionLowerLimit.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_038;
            this.lblTitleAutoRetestConditionUpperLimit.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_039;
            this.lblTitleManualDilutionUse.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_040;
            this.optManualDilutionUse.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_032;
            this.optManualDilutionUse.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_033;
            this.lblTitleAnalysisMode.Text = CarisX.Properties.Resources.STRING_PROTOCOLSETTING_133;
            this.optEmergencyModeUse.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_032;
            this.optEmergencyModeUse.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_033;
            this.lblTitleReverseDispensingOrderR1.Text = CarisX.Properties.Resources.STRING_PROTOCOLSETTING_134;
            this.optReverseDispensingOrderR1.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_032;
            this.optReverseDispensingOrderR1.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_033;

            String[] TitleDynamicRangeArray = new string[] { Properties.Resources.STRING_PROTOCOLSETTING_041
                , Properties.Resources.STRING_PROTOCOLSETTING_042, Properties.Resources.STRING_PROTOCOLSETTING_044, Properties.Resources.STRING_PROTOCOLSETTING_045
                ,Properties.Resources.STRING_PROTOCOLSETTING_046, Properties.Resources.STRING_PROTOCOLSETTING_043};
            this.lblTitleDynamicRange.Text = String.Join(" ", TitleDynamicRangeArray);
            this.lblHyphen1.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_047;
            this.lblTitleCorrelationCoeffcient.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_048;
            this.lblTitleCofficientA.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_049;
            this.lblTitleCofficientB.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_050;
            this.lblTilteMultiReplicationLimitCV.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_051;
            this.lblTitleUnit.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_052;
            this.lblTitleConcentrationDecimalPlaces.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_053;
            this.lblTitleCalibrationType.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_054;
            this.lblTitleNumberOfCalibrationPoints.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_055;
            this.lblTitleCalibrationMethod.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_056;
            this.lblTitleConcentrationColumn.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_057;
            this.lblTitlteCountrangeColumn.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_058;
            this.lblTitleConcentrationCountrangeRow1.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_059;
            this.lblTitleConcentrationCountrangeRow2.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_060;
            this.lblTitleConcentrationCountrangeRow3.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_061;
            this.lblTitleConcentrationCountrangeRow4.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_062;
            this.lblTitleConcentrationCountrangeRow5.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_063;
            this.lblTitleConcentrationCountrangeRow6.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_064;
            this.lblTitleConcentrationCountrangeRow7.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_065;
            this.lblTitleConcentrationCountrangeRow8.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_066;
            this.lblHyphen2.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_067;
            this.lblHyphen3.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_067;
            this.lblHyphen4.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_067;
            this.lblHyphen5.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_067;
            this.lblHyphen6.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_067;
            this.lblHyphen7.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_067;
            this.lblHyphen8.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_067;
            this.lblHyphen9.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_067;
            this.lblTitleSampleDispenseVolume.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_068;
            this.lblTitleMReagentDispenseVolume.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_069;
            this.lblTitleR1ReagentDispenseVolume.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_070;
            this.lblTitleR2ReagentDispenseVolume.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_071;
            this.lblTitlePreProcessLiquid1DispenseVolume.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_072;
            this.lblTitlePreProcessLiquid2DispenseVolume.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_073;
            this.lblUnitSampleDispenseVolume.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_074;
            this.lblUnitMReagentDispenseVolume.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_074;
            this.lblUnitR1ReagentDispenseVolume.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_074;
            this.lblUnitR2ReagentDispenseVolume.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_074;
            this.lblUnitPreProcessLiquid1DispenseVolume.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_074;
            this.lblUnitPreProcessLiquid2DispenseVolume.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_074;
            this.lblAssayConditionReagent.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_110;
            this.lblTitleDayOfReagentValid.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_111;
            this.lblUnitDayOfReagentValid.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_112;
            this.lblAssayCondition.Text = Oelco.CarisX.Properties.Resources.STRING_PROTOCOLSETTING_118;
        }

        /// <summary>
        /// ユーザレベル設定
        /// </summary>
        /// <remarks>
        /// ユーザレベル設定します
        /// </remarks>
        protected override void setUser(Object value)
        {
            this.tabProtocolSetting.Tabs[1].Visible = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.MeasureProtocolSettingDetail);
            chgProtocolSettingShadowInside();
            this.numDayOfReagentValid.ReadOnly = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.SetDayOfReagentValid);

            this.numExpirationDateOfTheCalibCurve.ReadOnly = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.SetExpirationDateOfTheCalibCurve);

            this.numCalibratorMeasureTimes.ReadOnly = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.CalibratorMeasureTimesModify);

            ///<summary>
            ///add by marxsu 正/負のしきい値許可設定（阴性/阳性阀值权限设置）
            ///</summary>
            ///

            this.numPositiveValue.ReadOnly = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.NegativeAndPositiveVaild);
            this.numNegativeValue.ReadOnly = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.NegativeAndPositiveVaild);

            this.gbxAssayCondition.Enabled = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AssayCondition);
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// FormClosing処理
        /// </summary>
        /// <remarks>
        /// 通知管理削除します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormProtocolSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            Singleton<NotifyManager>.Instance.RemoveNotifyTarget((Int32)NotifyKind.SystemStatusChanged, this.onSystemStatusChanged);
        }

        #region _コマンドバー_

        /// <summary>
        /// Closeを押下時、Form共通の編集中フラグチェックを実施
        /// </summary>
        /// <remarks>
        /// Hide()をする前にチェックを実施します。
        /// </remarks>
        private void closeParameter()
        {
            // 編集中かどうか、また編集中でも画面遷移するかどうか
            if (CarisXSubFunction.IsEditsMessageShow())
            {   
                this.Hide();

                // Form共通の編集中フラグOFF
                FormChildBase.IsEdit = false;
            }
        }

        /// <summary>
        /// 分析パラメータの保存
        /// </summary>
        /// <remarks>
        /// 操作履歴に登録実行を登録し、分析パラメータを保存します
        /// </remarks>
        private void saveParameter()
        {
            // キャリブレータ濃度値の昇順チェック追加
            // チェック対象コントロール配列
            var numConcentrationCtrls = new[]{
                this.numConcentration1,
                this.numConcentration2,
                this.numConcentration3,
                this.numConcentration4,
                this.numConcentration5,
                this.numConcentration6,
                this.numConcentration7,
                this.numConcentration8,
            };

            // キャリブレータ濃度値で活性の項目の値をリストに格納する。
            List<double> checkConcentrations = new List<double>();
            if ((MeasureProtocol.CalibrationMethod)this.cmbCalibrationMethod.SelectedItem.DataValue == MeasureProtocol.CalibrationMethod.FullCalibration)
            {
                // フルキャリブレーションのときは、活性項目全部valueをリストに積む
                for (int i = 0; i < (int)cmbNumberOfCalibrationPoints.Value; i++)
                {
                    checkConcentrations.Add((double)numConcentrationCtrls[i].Value);
                }
            }
            else
            {
                // マスタキャリブレーションのときはCalibMeasPointOfEachがtrueになっている項目だけリストに積む
                var measureProtocol = protocolNowEdit;
                for (int i = 0; i < measureProtocol.CalibMeasPointOfEach.Count(); i++)
                {
                    if (measureProtocol.CalibMeasPointOfEach[i])
                    {
                        checkConcentrations.Add((double)numConcentrationCtrls[i].Value);
                    }
                }
            }


            // 定量法は検量線シーケンスを確認する必要がある（定量的方法需要确认校准曲线顺序）
            MeasureProtocol.CalibrationType Ctype = (MeasureProtocol.CalibrationType)this.cmbCalibrationType.SelectedItem.DataValue;
            if (Ctype != MeasureProtocol.CalibrationType.INH
                && Ctype != MeasureProtocol.CalibrationType.CutOff)
            {
                bool askCheckErr = false;
                // 重複値が存在する場合はエラー
                if (checkConcentrations.Count() != checkConcentrations.Distinct().Count())
                {
                    askCheckErr = true;
                }
                else
                {
                    // キャリブレータ濃度値を昇順で並び替えた比較用のリストを作成する。
                    List<double> compareConcentrations = checkConcentrations.OrderBy(x => x).ToList();

                    // 昇順チェック処理実施
                    // 濃度値をソートしたリストと、比較対象の順番が違っていればエラーにする
                    for (int i = 0; i < checkConcentrations.Count(); i++)
                    {
                        if (checkConcentrations[i] != compareConcentrations[i])
                        {
                            askCheckErr = true;
                            break;
                        }
                    }
                }

                // キャリブレータ濃度値昇順チェックでエラーの場合、ダイアログ表示して処理終了
                if (askCheckErr)
                {
                    DlgMessage.Show(Oelco.CarisX.Properties.Resources.STRING_DLG_MSG_226, "", Oelco.CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                    return;
                }
            }

            // 入力チェック
            if (!CheckControls(tabProtocolSetting))
            {
                DlgMessage.Show(Oelco.CarisX.Properties.Resources.STRING_DLG_TITLE_002, "", Oelco.CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                return;
            }

            // 操作履歴 
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_009 });

            // 保存確認
            DialogResult dlgRet = DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_182, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel);
            if (dlgRet != DialogResult.OK)
            {
                return;
            }

            // サンプル種別を修正した場合、且つ「血」「尿」のどちらかしかチェックONでない場合は、AnalyteGroupの整合性チェックをする必要有。			

            // チェックボックスどちらかしかONでない場合は、AnalyteGroupのチェックをする
            if (!this.chkSpecimenTypeSerumOrPlasma.Checked || !this.chkSpecimenTypeUrine.Checked)
            {
                Oelco.CarisX.Parameter.MeasureProtocol.SampleTypeKind checkSampleKind;
                Int32 protocolIndex = protocolNowEdit.ProtocolIndex;
                if (this.chkSpecimenTypeSerumOrPlasma.Checked)
                {
                    checkSampleKind = MeasureProtocol.SampleTypeKind.SerumOrPlasma;
                }
                else
                {
                    checkSampleKind = MeasureProtocol.SampleTypeKind.Urine;
                }

                // 保存対象分析項目を使用しているAnalyteGroupを検索する
                List<Oelco.CarisX.Parameter.AnalyteGroup.AnalyteGroupInfo> deleteGroupList
                    = Oelco.CarisX.Parameter.AnalyteGroup.AnalyteGroupManager.GetSumpleKindCheckErrorGroupNames(
                        new List<Tuple<Int32, Oelco.CarisX.Parameter.MeasureProtocol.SampleTypeKind>> { new Tuple<Int32, Oelco.CarisX.Parameter.MeasureProtocol.SampleTypeKind>(protocolIndex, checkSampleKind) });

                if (deleteGroupList.Count > 0)
                {
                    if (DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_228, "", "", MessageDialogButtons.OKCancel) == DialogResult.OK)
                    {
                        // 削除前のAnalyteGroup情報を取得
                        Singleton<ParameterFilePreserve<AnalyteGroupInfoManager>>.Instance.Load();
                        AnalyteGroupInfoManager analyteGroupInfoManager = Singleton<ParameterFilePreserve<AnalyteGroupInfoManager>>.Instance.Param;

                        foreach (AnalyteGroupInfo delGroup in deleteGroupList)
                        {
                            analyteGroupInfoManager.AnalyteGroupInfos.Remove(analyteGroupInfoManager.AnalyteGroupInfos.Where(x => x.GroupName == delGroup.GroupName).First());
                        }
                        // AnalyteGroup削除処理をXMLに反映
                        AnalyteGroupManager.SaveAnalyteGroup(analyteGroupInfoManager.AnalyteGroupInfos);
                    }
                    else
                    {
                        return;
                    }
                }
            }


            // 分析項目の情報を設定する
            // MeasureProtocolManagerのGetMeasureProtocolFromXXXで参照MeasureProtocolが返ってくるのでそこに設定する。
            MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(this.protocolNo);

            // ParameterChangeLogテーブルデータ読込
            Singleton<ParameterChangeLogDB>.Instance.LoadDB();


            //
            // Regular(一般設定)
            //

            //
            // Measurement times(測定回数)
            //


            // 検体多重測定回数
            if (protocol.RepNoForSample != (Int32)this.numSpecimenMultiMeasure.Value)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblMeasurementTimes.Text, lblTitleSpecimenMultiMeasure.Text
                                   , this.numSpecimenMultiMeasure.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.RepNoForSample = (Int32)this.numSpecimenMultiMeasure.Value;

            // 精度管理検体多重測定回数
            if (protocol.RepNoForControl != (Int32)this.numControlMeasureTimes.Value)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblMeasurementTimes.Text, lblTitleControlMeasureTimes.Text
                                   , this.numControlMeasureTimes.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.RepNoForControl = (Int32)this.numControlMeasureTimes.Value;

            // キャリブレータ多重測定回数
            if (protocol.RepNoForCalib != (Int32)this.numCalibratorMeasureTimes.Value)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblMeasurementTimes.Text, lblTitleCalibratorMeasureTimes.Text
                                   , this.numCalibratorMeasureTimes.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.RepNoForCalib = (Int32)this.numCalibratorMeasureTimes.Value;

            //
            // Calibration assay condition(キャリブレーション分析条件)
            //

            // 検量線有効期間
            // TODO:パラメータ登録必要有無確認
            if (protocol.ValidityOfCurve != Double.Parse(this.numExpirationDateOfTheCalibCurve.Value.ToString()))
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblCalibAssayCondition.Text, lblTitleExpirationDateOfTheCalibCurve.Text
                                   , this.numExpirationDateOfTheCalibCurve.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.ValidityOfCurve = Double.Parse(this.numExpirationDateOfTheCalibCurve.Value.ToString());

            //
            // Negative/Positive value(陰性/陽性判定閾値)
            //

            // 陽性判定閾値
            if (protocol.PosiLine != Double.Parse(this.numPositiveValue.Value.ToString()))
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblNegativePositiveValue.Text, lblTitlePositiveValue.Text
                                   , this.numPositiveValue.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.PosiLine = Double.Parse(this.numPositiveValue.Value.ToString());

            // 陰性判定閾値
            if (protocol.NegaLine != Double.Parse(this.numNegativeValue.Value.ToString()))
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblNegativePositiveValue.Text, lblNegativeValue.Text
                                   , this.numNegativeValue.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.NegaLine = Double.Parse(this.numNegativeValue.Value.ToString());

            //
            // Detailed(詳細)
            //

            //
            // Assay condition 1(分析条件1)
            //

            // アッセイシーケンス
            if (protocol.AssaySequence != (MeasureProtocol.AssaySequenceKind)this.cmbAssaySequence.Value)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblAssayCondition1.Text, lblTitleAssaySequence.Text
                                   , this.cmbAssaySequence.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.AssaySequence = (MeasureProtocol.AssaySequenceKind)this.cmbAssaySequence.Value;

            // プロトコル希釈倍率
            if (protocol.ProtocolDilutionRatio != (MeasureProtocol.DilutionRatioKind)this.cmbDilutionRatio.Value)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblAssayCondition1.Text, lblTitleDilutionRatio.Text
                                   , this.cmbDilutionRatio.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.ProtocolDilutionRatio = (MeasureProtocol.DilutionRatioKind)this.cmbDilutionRatio.Value;

            // 前処理シーケンス
            // TODO:パラメータ変更履歴登録必要有無確認
            if (protocol.PreProcessSequence != (MeasureProtocol.PreProcessSequenceKind)this.cmbPreProcessSequence.Value)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblAssayCondition1.Text, lblTitlePreProcessSequence.Text
                                   , this.cmbPreProcessSequence.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.PreProcessSequence = (MeasureProtocol.PreProcessSequenceKind)this.cmbPreProcessSequence.Value;

            // サンプル種別
            MeasureProtocol.SampleTypeKind sampleKind = MeasureProtocol.SampleTypeKind.None;

            if (this.chkSpecimenTypeSerumOrPlasma.Checked == true)
            {
                sampleKind |= MeasureProtocol.SampleTypeKind.SerumOrPlasma;
            }
            if (this.chkSpecimenTypeUrine.Checked == true)
            {
                sampleKind |= MeasureProtocol.SampleTypeKind.Urine;
            }

            if (protocol.SampleKind != sampleKind)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblAssayCondition1.Text, lblTitleSpecimenType.Text
                                    , sampleKind.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.SampleKind = sampleKind;

            // 自動希釈再検使用有無
            Boolean useAfterDil;
            if (this.optAutoDilutionRetestUse.CheckedIndex == 0)
            {
                useAfterDil = true;
            }
            else
            {
                useAfterDil = false;
            }
            if (protocol.UseAfterDil != useAfterDil)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblAssayCondition1.Text, lblTitleAutoDilutionRetestUse.Text
                                   , useAfterDil.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.UseAfterDil = useAfterDil;

            // 自動再検使用有無
            Boolean useAutoReTest;
            if (this.optAutoRetestUse.CheckedIndex == 0)
            {
                useAutoReTest = true;
            }
            else
            {
                useAutoReTest = false;
            }
            if (protocol.UseAutoReTest != useAutoReTest)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblAssayCondition1.Text, lblTitleAutoRetestUse.Text
                                   , useAutoReTest.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.UseAutoReTest = useAutoReTest;

            // 自動希釈再検条件
            // TODO:パラメータ変更履歴登録必要有無確認
            if (protocol.AutoDilutionReTest.Min != Double.Parse(this.numAutoDilutionRetestConditionLowerLimit.Value.ToString()))
            {
                // パラメータ変更履歴登録 下限
                AddParamChangeLogData(lblAssayCondition1.Text, lblAutoDilutionRetestCondition.Text + CarisX.Properties.Resources.STRING_COMMON_013 + lblTitleAutoDilutionRetestConditionLowerLimit.Text
                                   , numAutoDilutionRetestConditionLowerLimit.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            if (protocol.AutoDilutionReTest.Max != Double.Parse(this.numAutoDilutionRetestConditionUpperLimit.Value.ToString()))
            {
                // パラメータ変更履歴登録 上限
                AddParamChangeLogData(lblAssayCondition1.Text, lblAutoDilutionRetestCondition.Text + CarisX.Properties.Resources.STRING_COMMON_013 + lblTitleAutoDilutionRetestConditionUpperLimit.Text
                                   , numAutoDilutionRetestConditionUpperLimit.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            if (protocol.AutoDilutionReTestRatio != (MeasureProtocol.AutoDilutionReTestRatioKind)this.cmbAutoDilutionRetestConditionDilutionRatio.Value)
            {
                // パラメータ変更履歴登録 
                AddParamChangeLogData(lblAssayCondition1.Text, lblAutoDilutionRetestCondition.Text + CarisX.Properties.Resources.STRING_COMMON_013 + lblTitleAutoDilutionRetestConditionDilutionRatio.Text
                                   , cmbAutoDilutionRetestConditionDilutionRatio.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.AutoDilutionReTest.Min = Double.Parse(this.numAutoDilutionRetestConditionLowerLimit.Value.ToString());  //下限      
            protocol.AutoDilutionReTest.Max = Double.Parse(this.numAutoDilutionRetestConditionUpperLimit.Value.ToString());  //上限
            protocol.AutoDilutionReTestRatio = (MeasureProtocol.AutoDilutionReTestRatioKind)this.cmbAutoDilutionRetestConditionDilutionRatio.Value;

            // 自動再検条件
            // TODO:パラメータ変更履歴登録必要有無確認
            if (protocol.AutoReTest.Min != Double.Parse(this.numAutoRetestConditionLowerLimit.Value.ToString()))
            {
                // パラメータ変更履歴登録 
                AddParamChangeLogData(lblAssayCondition1.Text, lblAutoRetestCondition.Text + CarisX.Properties.Resources.STRING_COMMON_013 + lblTitleAutoRetestConditionLowerLimit.Text
                                   , numAutoRetestConditionLowerLimit.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            if (protocol.AutoReTest.Max != Double.Parse(this.numAutoRetestConditionUpperLimit.Value.ToString()))
            {
                // パラメータ変更履歴登録 
                AddParamChangeLogData(lblAssayCondition1.Text, lblAutoRetestCondition.Text + CarisX.Properties.Resources.STRING_COMMON_013 + lblTitleAutoRetestConditionUpperLimit.Text
                                   , numAutoRetestConditionUpperLimit.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.AutoReTest.Min = Double.Parse(this.numAutoRetestConditionLowerLimit.Value.ToString());  //下限
            protocol.AutoReTest.Max = Double.Parse(this.numAutoRetestConditionUpperLimit.Value.ToString());  //上限

            // 手希釈使用有無
            Boolean manualDil;
            if (this.optManualDilutionUse.CheckedIndex == 0)
            {
                manualDil = true;
            }
            else
            {
                manualDil = false;
            }
            if (protocol.UseManualDil != manualDil)
            {
                // パラメータ変更履歴登録 
                AddParamChangeLogData(lblAssayCondition1.Text, lblTitleManualDilutionUse.Text
                                   , manualDil.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.UseManualDil = manualDil;
            //再試験の境界条件を判断する（判断重测的边界条件）
            if (protocol.RetestRange.UseLow != chkRetestLowLimit.Checked)
            {
                AddParamChangeLogData(lblAssayCondition1.Text, chkRetestLowLimit.Text
                                   , chkRetestLowLimit.Checked.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.RetestRange.UseLow = chkRetestLowLimit.Checked;

            if (protocol.RetestRange.UseMiddle != chkRetestMiddleLimit.Checked)
            {
                AddParamChangeLogData(lblAssayCondition1.Text, chkRetestMiddleLimit.Text
                                   , chkRetestMiddleLimit.Checked.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.RetestRange.UseMiddle = chkRetestMiddleLimit.Checked;

            if (protocol.RetestRange.UseHigh != chkRetestUpperLimit.Checked)
            {
                AddParamChangeLogData(lblAssayCondition1.Text, chkRetestUpperLimit.Text
                                   , chkRetestUpperLimit.Checked.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.RetestRange.UseHigh = chkRetestUpperLimit.Checked;
            //

            // Assay condition 2(分析条件2)
            //

            //キャリブレータと品質管理が希釈されているかどうか（校准品、质控品是否稀释）
            if (protocol.DiluCalibOrControl != (Int32)this.cmbCalibrationControlDilu.Value)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblAssayCondition2.Text, lblTitleCalibrationDilu.Text
                                   , this.cmbCalibrationControlDilu.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.DiluCalibOrControl = (Int32)this.cmbCalibrationControlDilu.Value;

            // 濃度ダイナミックレンジ
            if (protocol.ConcDynamicRange.Min != Double.Parse(this.numDynamicRangeLower.Value.ToString()))
            {
                // パラメータ変更履歴登録 下限
                AddParamChangeLogData(lblAssayCondition2.Text
                    , Properties.Resources.STRING_PROTOCOLSETTING_041 + Properties.Resources.STRING_COMMON_013 + Properties.Resources.STRING_PROTOCOLSETTING_044
                    , this.numDynamicRangeLower.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            if (protocol.ConcDynamicRange.Max != Double.Parse(this.numDynamicRangeUpper.Value.ToString()))
            {
                // パラメータ変更履歴登録 上限
                AddParamChangeLogData(lblAssayCondition2.Text
                    , Properties.Resources.STRING_PROTOCOLSETTING_041 + Properties.Resources.STRING_COMMON_013 + Properties.Resources.STRING_PROTOCOLSETTING_046
                    , this.numDynamicRangeUpper.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.ConcDynamicRange.Min = Double.Parse(this.numDynamicRangeLower.Value.ToString());   //下限
            protocol.ConcDynamicRange.Max = Double.Parse(this.numDynamicRangeUpper.Value.ToString());   //上限

            // 相関係数
            if (protocol.GainOfCorrelation != Double.Parse(this.numCofficientA.Value.ToString()))
            {
                // パラメータ変更履歴登録 相関係数A
                AddParamChangeLogData(lblAssayCondition2.Text, lblTitleCorrelationCoeffcient.Text + CarisX.Properties.Resources.STRING_COMMON_013 + lblTitleCofficientA.Text
                                   , this.numCofficientA.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            if (protocol.OffsetOfCorrelation != Double.Parse(this.numCofficientB.Value.ToString()))
            {
                // パラメータ変更履歴登録 相関係数B
                AddParamChangeLogData(lblAssayCondition2.Text, lblTitleCorrelationCoeffcient.Text + CarisX.Properties.Resources.STRING_COMMON_013 + lblTitleCofficientB.Text
                                   , this.numCofficientB.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.GainOfCorrelation = Double.Parse(this.numCofficientA.Value.ToString());     //相関係数A
            protocol.OffsetOfCorrelation = Double.Parse(this.numCofficientB.Value.ToString());   //相関係数B

            // 多重測定内乖離限界 CV%
            if (protocol.MulMeasDevLimitCV != Double.Parse(this.numMultiReplicationLimitCV.Value.ToString()))
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblAssayCondition2.Text, lblTilteMultiReplicationLimitCV.Text
                                   , this.numMultiReplicationLimitCV.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.MulMeasDevLimitCV = Double.Parse(this.numMultiReplicationLimitCV.Value.ToString());

            // 濃度単位
            if (protocol.ConcUnit != this.txtUnit.Text)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblAssayCondition2.Text, lblTitleUnit.Text
                                   , this.txtUnit.Text + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.ConcUnit = this.txtUnit.Text;

            // 濃度値小数点以下桁数
            if (protocol.LengthAfterDemPoint != (Int32)this.numConcentrationDecimalPlaces.Value)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblAssayCondition2.Text, lblTitleConcentrationDecimalPlaces.Text
                                   , this.numConcentrationDecimalPlaces.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.LengthAfterDemPoint = (Int32)this.numConcentrationDecimalPlaces.Value;

            //
            // Calibration assay condition(キャリブレーション分析条件)
            //

            // キャリブレーションタイプ
            if (protocol.CalibType != (MeasureProtocol.CalibrationType)this.cmbCalibrationType.Value)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblCalibAssayConditionDetailed.Text, lblTitleCalibrationType.Text
                                  , this.cmbCalibrationType.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.CalibType = (MeasureProtocol.CalibrationType)this.cmbCalibrationType.Value;

            // フルキャリブレーションポイント数
            if (protocol.NumOfMeasPointInCalib != (Int32)this.cmbNumberOfCalibrationPoints.Value)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblCalibAssayConditionDetailed.Text, lblTitleNumberOfCalibrationPoints.Text
                                   , this.cmbNumberOfCalibrationPoints.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.NumOfMeasPointInCalib = (Int32)this.cmbNumberOfCalibrationPoints.Value;

            // キャリブレーション方法
            if (protocol.CalibMethod != (MeasureProtocol.CalibrationMethod)this.cmbCalibrationMethod.Value)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblCalibAssayConditionDetailed.Text, lblTitleCalibrationMethod.Text
                                   , this.cmbCalibrationMethod.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.CalibMethod = (MeasureProtocol.CalibrationMethod)this.cmbCalibrationMethod.Value;

            // 急診有無
            Boolean useEmergencyMode;
            if (this.optEmergencyModeUse.CheckedIndex == 0)
            {
                useEmergencyMode = true;
            }
            else
            {
                useEmergencyMode = false;
            }
            if (protocol.UseEmergencyMode != useEmergencyMode)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData( lblAssayCondition1.Text, lblTitleAnalysisMode.Text
                                   , useEmergencyMode.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001 );
            }
            protocol.UseEmergencyMode = useEmergencyMode;

            // R1ユニットの分注順逆転
            Boolean reverseDispensingOrderR1;
            if (this.optReverseDispensingOrderR1.CheckedIndex == 0)
            {
                reverseDispensingOrderR1 = true;
            }
            else
            {
                reverseDispensingOrderR1 = false;
            }
            if (protocol.ReverseDispensingOrderR1 != reverseDispensingOrderR1)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblAssayCondition1.Text, lblTitleReverseDispensingOrderR1.Text
                                   , reverseDispensingOrderR1.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.ReverseDispensingOrderR1 = reverseDispensingOrderR1;

            // 濃度桁数フォーマット設定
            ConcMaskFotmat();

            protocol.CalibMeasPointOfEach = this.protocolNowEdit.CalibMeasPointOfEach;

            var concOfEach = new[]{
                Math.Round((double)this.numConcentration1.Value, (Int32)this.numConcentrationDecimalPlaces.Value, MidpointRounding.AwayFromZero).ToString(),
                Math.Round((double)this.numConcentration2.Value, (Int32)this.numConcentrationDecimalPlaces.Value, MidpointRounding.AwayFromZero).ToString(),
                Math.Round((double)this.numConcentration3.Value, (Int32)this.numConcentrationDecimalPlaces.Value, MidpointRounding.AwayFromZero).ToString(),
                Math.Round((double)this.numConcentration4.Value, (Int32)this.numConcentrationDecimalPlaces.Value, MidpointRounding.AwayFromZero).ToString(),
                Math.Round((double)this.numConcentration5.Value, (Int32)this.numConcentrationDecimalPlaces.Value, MidpointRounding.AwayFromZero).ToString(),
                Math.Round((double)this.numConcentration6.Value, (Int32)this.numConcentrationDecimalPlaces.Value, MidpointRounding.AwayFromZero).ToString(),
                Math.Round((double)this.numConcentration7.Value, (Int32)this.numConcentrationDecimalPlaces.Value, MidpointRounding.AwayFromZero).ToString(),
                Math.Round((double)this.numConcentration8.Value, (Int32)this.numConcentrationDecimalPlaces.Value, MidpointRounding.AwayFromZero).ToString(),
            };
            //---------------------------------------------------------
            for (var i = 0; i < concOfEach.Count(); i++)
            {
                var val = Double.Parse(concOfEach[i]);
                if (protocol.ConcsOfEach[i] != val)
                {
                    // パラメータ変更履歴登録
                    AddParamChangeLogData(lblCalibAssayConditionDetailed.Text, lblTitleConcentrationColumn.Text + i.ToString()
                                       , val.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
                }
                protocol.ConcsOfEach[i] = val;
            }

            // カウント範囲
            Action<UltraNumericEditor, UltraNumericEditor, MeasureProtocol.ItemRange> setCountCheckRange = (from, to, range) =>
            {
                Double fromValue = Double.Parse(from.Value.ToString());
                if (range.Min != fromValue)
                {
                    range.Min = fromValue;
                    // パラメータ変更履歴登録
                    AddParamChangeLogData(lblCalibAssayConditionDetailed.Text
                        , lblTitlteCountrangeColumn.Text + Properties.Resources.STRING_COMMON_013 + Properties.Resources.STRING_PROTOCOLSETTING_044
                        , fromValue.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
                }

                Double toValue = Double.Parse(to.Value.ToString());
                if (range.Max != toValue)
                {
                    range.Max = toValue;
                    // パラメータ変更履歴登録
                    AddParamChangeLogData(lblCalibAssayConditionDetailed.Text
                        , lblTitlteCountrangeColumn.Text + Properties.Resources.STRING_COMMON_013 + Properties.Resources.STRING_PROTOCOLSETTING_046
                        , toValue.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
                }
            };
            setCountCheckRange(this.numCoutnrange1From, this.numCoutnrange1To, protocol.CountRangesOfEach[0]);
            setCountCheckRange(this.numCoutnrange2From, this.numCoutnrange2To, protocol.CountRangesOfEach[1]);
            setCountCheckRange(this.numCoutnrange3From, this.numCoutnrange3To, protocol.CountRangesOfEach[2]);
            setCountCheckRange(this.numCoutnrange4From, this.numCoutnrange4To, protocol.CountRangesOfEach[3]);
            setCountCheckRange(this.numCoutnrange5From, this.numCoutnrange5To, protocol.CountRangesOfEach[4]);
            setCountCheckRange(this.numCoutnrange6From, this.numCoutnrange6To, protocol.CountRangesOfEach[5]);
            setCountCheckRange(this.numCoutnrange7From, this.numCoutnrange7To, protocol.CountRangesOfEach[6]);
            setCountCheckRange(this.numCoutnrange8From, this.numCoutnrange8To, protocol.CountRangesOfEach[7]);

            // サンプル分注量
            if (protocol.SmpDispenseVolume != (Int32)this.numSampleDispenseVolume.Value)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblDispensingCondtion.Text, lblTitleSampleDispenseVolume.Text
                                   , this.numSampleDispenseVolume.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.SmpDispenseVolume = (Int32)this.numSampleDispenseVolume.Value;

            // M試薬分注量
            if (protocol.MReagDispenseVolume != (Int32)this.numMReagentDispenseVolume.Value)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblDispensingCondtion.Text, lblTitleMReagentDispenseVolume.Text
                                   , this.numMReagentDispenseVolume.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.MReagDispenseVolume = (Int32)this.numMReagentDispenseVolume.Value;

            // R1試薬分注量
            if (protocol.R1DispenseVolume != (Int32)this.numR1ReagentDispenseVolume.Value)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblDispensingCondtion.Text, lblTitleR1ReagentDispenseVolume.Text
                                   , this.numR1ReagentDispenseVolume.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.R1DispenseVolume = (Int32)this.numR1ReagentDispenseVolume.Value;

            // R2試薬分注量
            if (protocol.R2DispenseVolume != (Int32)this.numR2ReagentDispenseVolume.Value)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblDispensingCondtion.Text, lblTitleR2ReagentDispenseVolume.Text
                                   , this.numR2ReagentDispenseVolume.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.R2DispenseVolume = (Int32)this.numR2ReagentDispenseVolume.Value;

            // 前処理液１分注量
            if (protocol.PreProsess1DispenseVolume != (Int32)this.numPreProcessLiquid1DispenseVolume.Value)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblDispensingCondtion.Text, lblTitlePreProcessLiquid1DispenseVolume.Text
                                  , this.numPreProcessLiquid1DispenseVolume.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.PreProsess1DispenseVolume = (Int32)this.numPreProcessLiquid1DispenseVolume.Value;

            // 前処理液２分注量
            if (protocol.PreProsess2DispenseVolume != (Int32)this.numPreProcessLiquid2DispenseVolume.Value)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(lblDispensingCondtion.Text, lblTitlePreProcessLiquid2DispenseVolume.Text
                                   , this.numPreProcessLiquid2DispenseVolume.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.PreProsess2DispenseVolume = (Int32)this.numPreProcessLiquid2DispenseVolume.Value;

            // 開封後有効期限
            if (protocol.DayOfReagentValid != (Int32)this.numDayOfReagentValid.Value)
            {
                // パラメータ変更履歴登録
                AddParamChangeLogData(this.lblAssayConditionReagent.Text, this.lblTitleDayOfReagentValid.Text
                                   , this.numDayOfReagentValid.Value.ToString() + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            protocol.DayOfReagentValid = (Int32)this.numDayOfReagentValid.Value;

            // 全てファイルへ保存する
            Singleton<MeasureProtocolManager>.Instance.SaveMeasureProtocol(protocol.ProtocolNo);

            // 各モジュールへプロトコルパラメータコマンドを送信する
            foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                // オンラインとなっているモジュールへ送信
                if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                {
                    SlaveCommCommand_0405 cmd0405 = new SlaveCommCommand_0405();
                    cmd0405.SetProtocolParameter(protocol);
                    Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((int)moduleindex, cmd0405);
                }
            }

            // ログ更新
            Singleton<ParameterChangeLogDB>.Instance.CommitParameterChangeLog();

            // プロトコル変更通知
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ChangeProtocolSetting, protocol.ProtocolIndex);

            // 試薬テーブルの更新
            // ※検量線の有効期限が変わった場合に試薬テーブル上のロット番号の色を変えるため
            RealtimeDataAgent.LoadReagentRemainData();

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// パラメータ初期化
        /// </summary>
        /// <remarks>
        /// 操作履歴にパラメータ初期化実行を登録し、分析項目情報の読込を行います
        /// </remarks>
        private void InitializeParameter()
        {
            // 操作履歴：パラメータ初期化実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_014 });

            // 画面表示更新
            loadProtocolData(true);

            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        #endregion

        /// <summary>
        /// 分析項目情報の読み込み
        /// </summary>
        /// <remarks>
        /// 分析項目情報の読込を行います
        /// </remarks>
        private void loadProtocolData(Boolean isInit = false)
        {

            MeasureProtocol protocol;

            if (isInit)
            {
                //初期化用分析項目の情報を取得(this.protocolNo 分析項目番号により分析項目の情報を取得)
                protocol = Singleton<MeasureProtocolManager>.Instance.GetDefaultMeasureProtocolFromProtocolNo(this.protocolNo);
            }
            else
            {
                //分析項目の情報を取得(this.protocolNo 分析項目番号により分析項目の情報を取得)
                protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(this.protocolNo);
            }

            // 分析項目が取得出来なかったら処理終了
            if (protocol == null)
            {
                DlgMessage.Show(Oelco.CarisX.Properties.Resources.STRING_DLG_TITLE_002, "", Oelco.CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                return;
            }

            //
            // タイトル
            //

            // 分析項目番号
            this.lblAnalyteNo.Text = protocol.ProtocolNo.ToString();



            // 分析項目名
            this.lblAnalytesName.Text = protocol.ProtocolName;

            // 試薬名
            this.lblReagentName.Text = protocol.ReagentName;

            // 試薬コード
            this.lblReagentCode.Text = protocol.ReagentCode.ToString();

            //
            // Regular(一般設定)
            //

            //
            // Measurement times(測定回数)
            //


            // 検体多重測定回数
            this.numSpecimenMultiMeasure.Value = protocol.RepNoForSample;

            // 精度管理検体多重測定回数
            this.numControlMeasureTimes.Value = protocol.RepNoForControl;
            // キャリブレータ多重測定回数
            this.numCalibratorMeasureTimes.Value = protocol.RepNoForCalib;

            //
            // Calibration assay condition(キャリブレーション分析条件)
            //

            // 検量線有効期間
            this.numExpirationDateOfTheCalibCurve.Value = protocol.ValidityOfCurve;

            //
            // Negative/Positive value(陰性/陽性判定閾値)
            //

            // 陽性判定閾値
            this.numPositiveValue.Value = protocol.PosiLine;

            // 陰性判定閾値
            this.numNegativeValue.Value = protocol.NegaLine;

            //
            // Detailed(詳細)
            //

            //
            // Assay condition 1(分析条件1)
            //

            // アッセイシーケンス
            this.cmbAssaySequence.Value = protocol.AssaySequence;

            // 後希釈倍率
            this.cmbDilutionRatio.Value = protocol.ProtocolDilutionRatio;

            // 前処理シーケンス
            this.cmbPreProcessSequence.Value = protocol.PreProcessSequence;

            // サンプル種別
            this.chkSpecimenTypeSerumOrPlasma.Checked = protocol.SampleKind.HasFlag(MeasureProtocol.SampleTypeKind.SerumOrPlasma);
            this.chkSpecimenTypeUrine.Checked = protocol.SampleKind.HasFlag(MeasureProtocol.SampleTypeKind.Urine);

            // 自動希釈再検使用有無
            if (protocol.UseAfterDil == true)
            {
                this.optAutoDilutionRetestUse.CheckedIndex = 0;
            }
            else
            {
                this.optAutoDilutionRetestUse.CheckedIndex = 1;
            }

            // 自動再検使用有無
            if (protocol.UseAutoReTest == true)
            {
                this.optAutoRetestUse.CheckedIndex = 0;
            }
            else
            {
                this.optAutoRetestUse.CheckedIndex = 1;
            }

            // 自動希釈再検条件
            this.numAutoDilutionRetestConditionLowerLimit.Value = protocol.AutoDilutionReTest.Min;  //下限      
            this.numAutoDilutionRetestConditionUpperLimit.Value = protocol.AutoDilutionReTest.Max;  //上限
            this.cmbAutoDilutionRetestConditionDilutionRatio.Value = protocol.AutoDilutionReTestRatio;

            // 自動再検条件
            this.numAutoRetestConditionLowerLimit.Value = protocol.AutoReTest.Min;  //下限
            this.numAutoRetestConditionUpperLimit.Value = protocol.AutoReTest.Max;  //上限

            // 手希釈使用有無
            if (protocol.UseManualDil == true)
            {
                this.optManualDilutionUse.CheckedIndex = 0;
            }
            else
            {
                this.optManualDilutionUse.CheckedIndex = 1;
            }
            this.chkRetestLowLimit.Checked = protocol.RetestRange.UseLow;
            this.chkRetestMiddleLimit.Checked = protocol.RetestRange.UseMiddle;
            this.chkRetestUpperLimit.Checked = protocol.RetestRange.UseHigh;

            // 急診有無
            if (protocol.UseEmergencyMode == true)
            {
                this.optEmergencyModeUse.CheckedIndex = 0;
            }
            else
            {
                this.optEmergencyModeUse.CheckedIndex = 1;
            }

            // R1ユニットの分注順逆転
            if (protocol.ReverseDispensingOrderR1 == true)
            {
                this.optReverseDispensingOrderR1.CheckedIndex = 0;
            }
            else
            {
                this.optReverseDispensingOrderR1.CheckedIndex = 1;
            }

            //
            // Assay condition 2(分析条件2)
            //

            //设置校准品、质控品是否稀释
            this.cmbCalibrationControlDilu.Value = protocol.DiluCalibOrControl;

            // 濃度ダイナミックレンジ
            this.numDynamicRangeLower.Value = protocol.ConcDynamicRange.Min;    //下限
            this.numDynamicRangeUpper.Value = protocol.ConcDynamicRange.Max;    //上限

            // 相関係数
            this.numCofficientA.Value = protocol.GainOfCorrelation;     //相関係数A
            this.numCofficientB.Value = protocol.OffsetOfCorrelation;   //相関係数B

            // 多重測定内乖離限界 CV%
            this.numMultiReplicationLimitCV.Value = protocol.MulMeasDevLimitCV;

            // 濃度単位
            this.txtUnit.Value = protocol.ConcUnit;

            // 濃度値小数点以下桁数
            this.numConcentrationDecimalPlaces.Value = protocol.LengthAfterDemPoint;

            //
            // Calibration assay condition(キャリブレーション分析条件)
            //

            // キャリブレーションタイプ
            this.cmbCalibrationType.Value = protocol.CalibType;

            // キャリブレーションポイント数
            this.cmbNumberOfCalibrationPoints.Value = protocol.NumOfMeasPointInCalib;

            // キャリブレーション方法
            this.cmbCalibrationMethod.Value = protocol.CalibMethod;

            // キャリブレーション方法がマスターキャリブレーションの場合
            if ((MeasureProtocol.CalibrationMethod)this.cmbCalibrationMethod.Value == MeasureProtocol.CalibrationMethod.MasterCalibration)
            {
                // キャリブレーションポイントを取得
                Int32 masterCalibrationPoint = protocol.CalibMeasPointOfEach.Where(v => v == true).Count();

                // マスターキャリブレーションポイントディクショナリー作成成否 
                Boolean createResult = false;

                // マスターキャリブレーションポイントディクショナリー作成
                createResult = this.createNumberOfMasterCalibrationPointsDic(masterCalibrationPoint);

                // マスターキャリブレーションポイントディクショナリー作成に成功した場合
                if (createResult == true)
                {
                    // コンボボックスのデータソースにマスターキャリブレーションポイントディクショナリーを設定
                    this.cmbNumberOfCalibrationPoints.DataSource = this.masterCalibrationPointsDic.ToList();
                }
                // 失敗した場合
                else
                {
                    // キャリブレーション方法コンボボックスの選択をフルキャリブレーションに変更
                    this.cmbCalibrationMethod.SelectedItem = this.cmbCalibrationMethod.Items.All.First(( item ) =>
                                                              (MeasureProtocol.CalibrationMethod)( (ValueListItem)item ).DataValue == MeasureProtocol.CalibrationMethod.FullCalibration) as ValueListItem;

                    // マスターキャリブへの変更に失敗したため、フルキャリブレーションに変更します。
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_270, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.Confirm);

                    // マスターキャリブへの変更に失敗したため、フルキャリブレーションに変更します。
                    String dbgMsg = String.Format("[[Investigation log]]FormProtocolSetting::{0} " +
                                     "Since the change to the Master Calibration failed, change to Full - calibration. ", MethodBase.GetCurrentMethod().Name);

                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                }
               
            }
            else
            {
                // フルキャリブレーションの場合はsetCulture()で設定しているため不要
            }

            //濃度フォーマット設定
            ConcMaskFotmat();

            // 濃度
            this.numConcentration1.Value = Math.Round(protocol.ConcsOfEach[0], (Int32)this.numConcentrationDecimalPlaces.Value, MidpointRounding.AwayFromZero);
            this.numConcentration2.Value = Math.Round(protocol.ConcsOfEach[1], (Int32)this.numConcentrationDecimalPlaces.Value, MidpointRounding.AwayFromZero);
            this.numConcentration3.Value = Math.Round(protocol.ConcsOfEach[2], (Int32)this.numConcentrationDecimalPlaces.Value, MidpointRounding.AwayFromZero);
            this.numConcentration4.Value = Math.Round(protocol.ConcsOfEach[3], (Int32)this.numConcentrationDecimalPlaces.Value, MidpointRounding.AwayFromZero);
            this.numConcentration5.Value = Math.Round(protocol.ConcsOfEach[4], (Int32)this.numConcentrationDecimalPlaces.Value, MidpointRounding.AwayFromZero);
            this.numConcentration6.Value = Math.Round(protocol.ConcsOfEach[5], (Int32)this.numConcentrationDecimalPlaces.Value, MidpointRounding.AwayFromZero);
            this.numConcentration7.Value = Math.Round(protocol.ConcsOfEach[6], (Int32)this.numConcentrationDecimalPlaces.Value, MidpointRounding.AwayFromZero);
            this.numConcentration8.Value = Math.Round(protocol.ConcsOfEach[7], (Int32)this.numConcentrationDecimalPlaces.Value, MidpointRounding.AwayFromZero);

            // カウント範囲
            this.numCoutnrange1From.Value = protocol.CountRangesOfEach[0].Min;
            this.numCoutnrange2From.Value = protocol.CountRangesOfEach[1].Min;
            this.numCoutnrange3From.Value = protocol.CountRangesOfEach[2].Min;
            this.numCoutnrange4From.Value = protocol.CountRangesOfEach[3].Min;
            this.numCoutnrange5From.Value = protocol.CountRangesOfEach[4].Min;
            this.numCoutnrange6From.Value = protocol.CountRangesOfEach[5].Min;
            this.numCoutnrange7From.Value = protocol.CountRangesOfEach[6].Min;
            this.numCoutnrange8From.Value = protocol.CountRangesOfEach[7].Min;
            this.numCoutnrange1To.Value = protocol.CountRangesOfEach[0].Max;
            this.numCoutnrange2To.Value = protocol.CountRangesOfEach[1].Max;
            this.numCoutnrange3To.Value = protocol.CountRangesOfEach[2].Max;
            this.numCoutnrange4To.Value = protocol.CountRangesOfEach[3].Max;
            this.numCoutnrange5To.Value = protocol.CountRangesOfEach[4].Max;
            this.numCoutnrange6To.Value = protocol.CountRangesOfEach[5].Max;
            this.numCoutnrange7To.Value = protocol.CountRangesOfEach[6].Max;
            this.numCoutnrange8To.Value = protocol.CountRangesOfEach[7].Max;

            //
            // Dispensing condition(分注条件)
            //

            // サンプル分注量
            this.numSampleDispenseVolume.Value = protocol.SmpDispenseVolume;

            // M試薬分注量
            this.numMReagentDispenseVolume.Value = protocol.MReagDispenseVolume;

            // R1試薬分注量
            this.numR1ReagentDispenseVolume.Value = protocol.R1DispenseVolume;

            // R2試薬分注量
            try
            {
                this.numR2ReagentDispenseVolume.Value = protocol.R2DispenseVolume;
            }
            catch (Exception ex)
            {
                // numR2ReagentDispenseVolumeのMinValueを1→25に変更したことにより、
                // 登録済み分析項目が25以下の場合、例外が発生する事象の予防措置
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                                CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
            }

            // 前処理液１分注量
            this.numPreProcessLiquid1DispenseVolume.Value = protocol.PreProsess1DispenseVolume;

            // 前処理液２分注量
            this.numPreProcessLiquid2DispenseVolume.Value = protocol.PreProsess2DispenseVolume;

            try
            {
                this.numDayOfReagentValid.Value = protocol.DayOfReagentValid;
            }
            catch
            {
            }

            protocolNowEdit = protocol;

            this.changeCalibPointEnable();

            // 希釈倍率に関するコントロールの活性非活性切り替え
            this.changedDilutionEnable();
        }

        /// <summary>
        /// 濃度項目マスクフォーマット設定
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="param"></param>
        /// <param name="paramDetail"></param>
        /// <param name="ChangeValue"></param>
        private void ConcMaskFotmat()
        {
            string maskStr = "";
            switch ((Int32)this.numConcentrationDecimalPlaces.Value)
            {
                case 0:
                    maskStr = "nnnnnnnnn";
                    break;
                case 1:
                    maskStr = "nnnnnnnnn.n";
                    break;
                case 2:
                    maskStr = "nnnnnnnnn.nn";
                    break;
                case 3:
                    maskStr = "nnnnnnnnn.nnn";
                    break;
            }

            this.numConcentration1.MaskInput = maskStr;
            this.numConcentration2.MaskInput = maskStr;
            this.numConcentration3.MaskInput = maskStr;
            this.numConcentration4.MaskInput = maskStr;
            this.numConcentration5.MaskInput = maskStr;
            this.numConcentration6.MaskInput = maskStr;
            this.numConcentration7.MaskInput = maskStr;
            this.numConcentration8.MaskInput = maskStr;

        }

        /// <summary>
        /// パラメータ変更履歴追加
        /// </summary>
        /// <remarks>
        /// パラメータ変更履歴を追加します
        /// </remarks>
        /// <param name="param"></param>
        /// <param name="paramDetail"></param>
        /// <param name="ChangeValue"></param>
        private void AddParamChangeLogData(string param, string paramDetail, string ChangeValue)
        {

            String[] contents = new String[4];
            contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_002 + CarisX.Properties.Resources.STRING_COMMON_014
                            + lblAnalytesName.Text + CarisX.Properties.Resources.STRING_COMMON_015;
            contents[1] = param;
            contents[2] = paramDetail;
            contents[3] = ChangeValue;
            Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);
        }

        /// <summary>
        /// システムステータス変化イベント
        /// </summary>
        /// <remarks>
        /// システムステータス変化によりコマンドバーの有効無効状態が変化します
        /// </remarks>
        /// <param name="value"></param>
        private void onSystemStatusChanged(Object value)
        {
            bool enabled = true;

            if (Singleton<SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId(Singleton<PublicMemory>.Instance.moduleIndex)] == SystemStatusKind.ReagentExchange)
            {
                //選択中のモジュールが試薬交換中の場合は非活性
                enabled = false;
            }
            else
            {
                switch (Singleton<SystemStatus>.Instance.Status)
                {
                    case SystemStatusKind.WaitSlaveResponce:
                    case SystemStatusKind.Assay:
                    case SystemStatusKind.SamplingPause:
                    case SystemStatusKind.ToEndAssay:
                        enabled = false;
                        break;
                }
            }

            this.tlbCommandBar.Tools[SAVE].SharedProps.Enabled = enabled;
            this.tlbCommandBar.Tools[INITIALIZE].SharedProps.Enabled = enabled;
        }

        /// <summary>
        /// キャリブポイント数変更
        /// </summary>
        /// <remarks>
        /// キャリブレーションポイント有効状態が切替します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void cmbNumberOfCalibrationPoints_ValueChanged(object sender, EventArgs e)
        {
            changeCalibPointEnable();

            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        /// <summary>
        /// キャリブレーション方法変更
        /// </summary>
        /// <remarks>
        /// キャリブレーション方法を変更します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCalibrationMethod_ValueChanged(object sender, EventArgs e)
        {
            // キャリブレーション方法がマスターキャリブレーションの場合
            if ((MeasureProtocol.CalibrationMethod)this.cmbCalibrationMethod.SelectedItem.DataValue == MeasureProtocol.CalibrationMethod.MasterCalibration)
            {
                if (protocolNowEdit == null)
                {
                    // load前である為抜ける。
                    return;
                }
                var measureProtocol = protocolNowEdit;

                // 測定ポイント数と、コンボボックスの情報から設定可能な最大測定ポイント数のアイテムを取得
                var selectedItem = this.cmbNumberOfCalibrationPoints.Items.All.FirstOrDefault((item) => 
                                    (Int32)(item as ValueListItem).DataValue == measureProtocol.CalibMeasPointOfEach.Count((point) => point)) as ValueListItem;

                if (selectedItem != null)
                {
                    // マスターキャリブレーションポイント数の取得
                    Int32 masterCalibrationPoint = Int32.Parse(selectedItem.DisplayText);

                    // マスターキャリブレーションポイントディクショナリー作成成否 
                    Boolean createResult = false;

                    // マスターキャリブレーションポイントディクショナリー作成
                    createResult = this.createNumberOfMasterCalibrationPointsDic(masterCalibrationPoint);

                    // マスターキャリブレーションポイントディクショナリー作成に成功した場合
                    if (createResult == true)
                    {
                        // コンボボックスのデータソースにマスターキャリブレーションポイントディクショナリーを設定
                        this.cmbNumberOfCalibrationPoints.DataSource = this.masterCalibrationPointsDic.ToList();

                        // 設定後のコンボボックス内に設定前と同じアイテムがあるかチェック
                        Boolean exists = this.cmbNumberOfCalibrationPoints.Items.Contains(selectedItem);

                        // 設定前のアイテムがある場合
                        if (exists == true)
                        {
                            // 選択項目を変更
                            this.cmbNumberOfCalibrationPoints.SelectedItem = selectedItem;
                        }
                        // 設定前のアイテムがない場合
                        else
                        {
                            // 先頭のアイテムを選択状態とする
                            this.cmbNumberOfCalibrationPoints.SelectedItem = this.cmbNumberOfCalibrationPoints.Items[0];
                        }
                    }
                    // マスターキャリブレーションポイントディクショナリー作成に失敗した場合
                    else
                    {
                        // キャリブレーション方法コンボボックスの選択をフルキャリブレーションに変更
                        this.cmbCalibrationMethod.SelectedItem = this.cmbCalibrationMethod.Items.All.First(( item ) => 
                                                                  (MeasureProtocol.CalibrationMethod)( (ValueListItem)item ).DataValue ==  MeasureProtocol.CalibrationMethod.FullCalibration) as ValueListItem;

                        // マスターキャリブへの変更に失敗したため、フルキャリブレーションに変更します。
                        DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_270, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.Confirm);

                        // マスターキャリブへの変更に失敗したため、フルキャリブレーションに変更します。
                        String dbgMsg = String.Format("[[Investigation log]]FormProtocolSetting::{0} " +
                        "Since the change to the Master Calibration failed, change to Full - calibration. ", MethodBase.GetCurrentMethod().Name);

                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                    }
                }
                else
                {
                    // 先頭のアイテムを選択状態とする
                    this.cmbNumberOfCalibrationPoints.SelectedItem = this.cmbNumberOfCalibrationPoints.Items[0];
                }
            }
            // キャリブレーション方法がフルキャリブレーションの場合
            else
            {
                // コンボボックスのデータソースにフルキャリブレーションポイントリストを設定
                this.cmbNumberOfCalibrationPoints.DataSource = this.numberOfCalibrationPointsList.ToList();

                //// todo:大塚電子様に使用確認が必要
                //// コンボボックスの選択をインデックス0にする
                //this.cmbNumberOfCalibrationPoints.SelectedIndex = 0;
            }
            this.changeCalibPointEnable();

            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        /// <summary>
        /// キャリブレーションポイント有効状態切替
        /// </summary>
        /// <remarks>
        /// キャリブレーションポイント有効状態切替します
        /// </remarks>
        private void changeCalibPointEnable()
        {
            UltraNumericEditor[][] calibPointConcCount = { new[]{ this.numConcentration1, this.numCoutnrange1From, this.numCoutnrange1To },
                                                         new[]{ this.numConcentration2, this.numCoutnrange2From, this.numCoutnrange2To },
                                                         new[]{ this.numConcentration3, this.numCoutnrange3From, this.numCoutnrange3To },
                                                         new[]{ this.numConcentration4, this.numCoutnrange4From, this.numCoutnrange4To },
                                                         new[]{ this.numConcentration5, this.numCoutnrange5From, this.numCoutnrange5To },
                                                         new[]{ this.numConcentration6, this.numCoutnrange6From, this.numCoutnrange6To },
                                                         new[]{ this.numConcentration7, this.numCoutnrange7From, this.numCoutnrange7To },
                                                         new[]{ this.numConcentration8, this.numCoutnrange8From, this.numCoutnrange8To } };


            var limitPointNo = (Int32)this.cmbNumberOfCalibrationPoints.Items[0].DataValue;
            var measureProtocol = this.protocolNowEdit;
            if (this.cmbCalibrationMethod.SelectedItem != null)
            {
                if ((MeasureProtocol.CalibrationMethod)this.cmbCalibrationMethod.SelectedItem.DataValue == MeasureProtocol.CalibrationMethod.MasterCalibration)
                {
                    limitPointNo = measureProtocol.CalibMeasPointOfEach.ToList().FindLastIndex((item) => item == true) + 1;
                }
                else
                {
                    limitPointNo = (Int32)this.cmbNumberOfCalibrationPoints.SelectedItem.DataValue;
                }
            }

            //選択により、各キャリブポイントの濃度、カウント上下限のEnableを切り替える
            Int32 pointNo = 1;
            foreach (var item in calibPointConcCount)
            {
                foreach (var control in item)
                {
                    control.Enabled = pointNo <= limitPointNo;
                }
                pointNo++;
            }
        }

        /// <summary>
        /// キャリブレーションタイプの切り替え
        /// </summary>
        /// <remarks>
        /// キャリブレーションタイプを切替します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void cmbCalibrationType_ValueChanged(object sender, EventArgs e)
        {
            // 定性項目の場合
            if (((MeasureProtocol.CalibrationType)this.cmbCalibrationType.SelectedItem.DataValue).IsQualitative())
            {
                // キャリブレーションポイント数コンボボックス内を探索
                foreach (var item in this.cmbNumberOfCalibrationPoints.Items)
                {
                    // アイテム値と定性項目ポイント数が等しい場合
                    if ((Int32)item.DataValue == CarisXConst.QUALITATIVE_POINT_COUNT)
                    {
                        // コンボボックスの選択項目を定性項目ポイント数と等しいアイテムに変更する
                        this.cmbNumberOfCalibrationPoints.SelectedItem = item;
                    }
                    // アイテム値と定性項目ポイント数が等しくない場合
                    else
                    {
                        // 何もしない
                    }
                }

                // キャリブレーション方法コンボボックスの選択をフルキャリブレーションに変更
                this.cmbCalibrationMethod.SelectedItem = this.cmbCalibrationMethod.Items.All.First((item) => (MeasureProtocol.CalibrationMethod)((ValueListItem)item).DataValue == MeasureProtocol.CalibrationMethod.FullCalibration) as ValueListItem;
            }
            else
            {
                // 定性項目でない場合は何もしない
            }

            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        /// <summary>
        /// 指定されたコントロール上に存在するすべてのコントロールを取得します。
        /// </summary>
        /// <param name="top_ctrl"></param>
        /// <returns></returns>
        protected bool CheckControls(Control top_ctrl)
        {

            List<Control> controls = GetAllControls(top_ctrl);

            foreach (Control ctrl in controls)
            {
                Infragistics.Win.UltraWinEditors.UltraNumericEditor target = ctrl as Infragistics.Win.UltraWinEditors.UltraNumericEditor;
                if (target != null && target.ReadOnly == false)
                {
                    if (!IsNumeric(target.Value.ToString()))
                        return false;
                }
            }
            return true;
        }


        protected bool IsNumeric(string stTarget)
        {
            double dNullable;

            return double.TryParse(
                stTarget,
                System.Globalization.NumberStyles.Any,
                null,
                out dNullable
            );
        }


        protected List<Control> GetAllControls(Control top_ctrl)
        {
            List<Control> buf = new List<Control>();
            foreach (Control c in top_ctrl.Controls)
            {
                buf.Add(c);
                buf.AddRange(GetAllControls(c));
            }
            return buf;
        }

        //-------------------------------------------------------------

        /// <summary>
        /// ProtocolSettingのタブの上部にある影の位置、幅を変更する
        /// </summary>
        private void chgProtocolSettingShadowInside()
        {
            Point baseShadowLocation = new Point(311, 173); //影の基準表示場所
            Size baseShadowSize = new Size(1116, 14);       //影の基準表示サイズ

            //タブの数に応じて影の表示位置を変更する
            if (this.tabProtocolSetting.Tabs[1].Visible)
            {
                //Detailsタグが表示されている場合
                pnlProtocolSettingShadowInside.Location = new Point(311, 173);
                pnlProtocolSettingShadowInside.Size = new Size(1116, 14);
            }
            else
            {
                //Detailsタグが表示されていない場合
                pnlProtocolSettingShadowInside.Location = new Point(161, 173);
                pnlProtocolSettingShadowInside.Size = new Size(1266, 14);
            }
        }

        /// <summary>
        /// タブページ「Regular」の値切り替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpProtocolRegular_ValueChanged(object sender, EventArgs e)
        {
            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        /// <summary>
        /// タブページ「Detailed」の値切り替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpProtocolDetailed_ValueChanged(object sender, EventArgs e)
        {
            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        /// <summary>
        /// タブページ「Detailed」のチェック状態切り替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbpProtocolDetailed_CheckedValueChanged(object sender, EventArgs e)
        {
            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        /// <summary>
        /// マスターキャリブレーションポイントディクショナリー作成
        /// </summary>
        /// <param name="maxCalibrationPoint">マスターキャリブレーションポイント数</param>
        /// <returns>ディクショナリー作成の正否　成功:true 失敗:false</returns>
        private Boolean createNumberOfMasterCalibrationPointsDic( Int32 masterCalibrationPoint )
        {
            String dbgMsg = String.Format("[[Investigation log]]FormProtocolSetting::{0} ", MethodBase.GetCurrentMethod().Name);

            // ディクショナリー作成の正否
            Boolean result = true;

            // マスターキャリブレーションポイント数をクリアする
            this.masterCalibrationPointsDic.Clear();

            // フルキャリブレーションポイント数がnullではない場合
            if (this.numberOfCalibrationPointsList != null)
            {
                dbgMsg = dbgMsg + String.Format(" masterCalibrationPoint = {0} ", masterCalibrationPoint);

                Int32 minItem = numberOfCalibrationPointsList.Select(v => v.Key).FirstOrDefault();

                // マスターキャリブレーションポイント数がフルキャリブレーションポイント数より大きい場合
                if (masterCalibrationPoint >= minItem)
                {
                    dbgMsg = dbgMsg + String.Format(" create masterCalibrationPointsDic start ");

                    // フルキャリブレーションポイント数リストを探索
                    foreach (var numberOfCalibrationPoints in this.numberOfCalibrationPointsList)
                    {
                        // マスターキャリブレーションポイント数がフルキャリブレーションポイント数より小さい場合
                        if (masterCalibrationPoint < numberOfCalibrationPoints.Key)
                        {
                            dbgMsg = dbgMsg + String.Format(" Finished create with {0} ", numberOfCalibrationPoints.Key);
                            // 探索を抜ける
                            break;
                        }
                        // マスターキャリブレーションポイント数がフルキャリブレーションポイント数以上の場合
                        else
                        {
                            dbgMsg = dbgMsg + String.Format(" Add {0} add to Dic ", numberOfCalibrationPoints.Key);

                            // マスターキャリブレーションポイント数にフルキャリブレーションポイント数を追加
                            this.masterCalibrationPointsDic.Add(numberOfCalibrationPoints.Key, numberOfCalibrationPoints.Value);
                        }
                    }
                }
                // マスターキャリブレーションポイント数がフルキャリブレーションポイント数より小さい場合
                else
                {
                    // 作成失敗
                    result = false;

                    dbgMsg = dbgMsg + String.Format(" < minItem:{0} ", minItem);
                }
            }
            // フルキャリブレーションポイント数がnullの場合
            else
            {
                // 作成失敗
                result = false;

                dbgMsg = dbgMsg + String.Format(" numberOfCalibrationPointsList is null ");
            }
            

            // マスターキャリブレーションポイント数がnullまたは、要素がない場合
            if (( this.masterCalibrationPointsDic == null) || ( this.masterCalibrationPointsDic.Count == 0))
            {
                dbgMsg = dbgMsg + String.Format(" masterCalibrationPointsDic is null or masterCalibrationPointsDic.Count == 0 ");
                // 作成失敗
                result = false;
            }
            // マスターキャリブレーションポイント数の要素がある場合
            else
            {
                // 作成成功
                result = true;
            }

            dbgMsg = dbgMsg + String.Format(" result = {0} ", result);

            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);

            return result;
        }

        /// <summary>
        /// 急診使用有無変更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optEmergencyModeUse_ValueChanged( object sender, EventArgs e )
        {
            // Form共通の編集中フラグONにする
            this.tbpProtocolDetailed_ValueChanged(sender, e);

            // 希釈倍率に関するコントロールの活性非活性切り替え
            this.changedDilutionEnable();
        }

        /// <summary>
        /// 希釈倍率に関するコントロールの活性/非活性の変更
        /// </summary>
        /// <remarks>
        /// 試薬、スレーブの急診使用有無によって希釈倍率に関するコントロールの活性/非活性を変更する
        /// </remarks>
        private void changedDilutionEnable()
        {
            // 急診使用有にチェックがあるの場合
            if (this.optEmergencyModeUse.CheckedIndex == 0)
            {
                // 自動希釈再検使用有無ボタンを非活性にする
                this.optAutoDilutionRetestUse.Enabled = false;

                // 自動希釈再検グループボックスを非活性にする
                this.gbxAutoDilutionRetestCondition.Enabled = false;
                               
                // プロトコル希釈倍率コンボボックスを非活性にする
                this.cmbDilutionRatio.Enabled = false;
            }
            // 急診使用無にチェックがあるの場合
            else
            {
                // 自動希釈再検使用有無ボタンを活性にする
                this.optAutoDilutionRetestUse.Enabled = true;

                // 自動希釈再検グループボックス活性にする
                this.gbxAutoDilutionRetestCondition.Enabled = true;

                // プロトコル希釈倍率コンボボックスを活性にする
                this.cmbDilutionRatio.Enabled = true;
            }
        }

        #endregion
    }
}
