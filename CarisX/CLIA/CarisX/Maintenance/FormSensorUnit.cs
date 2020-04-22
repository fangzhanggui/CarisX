using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.CarisX.Parameter;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.Common.Comm;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Properties;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.Maintenance
{
    public partial class FormSensorUnit : FormUnitBase
    {
        IMaintenanceUnitStart unitStart = new UnitSensorStart();
        volatile bool ResponseFlg;
        private bool MaintenanceMode;
        public CommonFunction ComFunc = new CommonFunction();

        /// <summary>
        /// コンストラクタ（メンテナンス版）
        /// </summary>
        public FormSensorUnit()
        {
            AutoScaleMode = AutoScaleMode.None;
            InitializeComponent();
            SensorUnitLoad();
            MaintenanceMode = true;
            setCulture();
            ComFunc.SetControlSettings(this);
        }

        /// <summary>
        /// コンストラクタ（障害対策版）
        /// </summary>
        /// <param name="MaintenanceMode"></param>
        public FormSensorUnit(bool MaintenanceMode)
        {
            AutoScaleMode = AutoScaleMode.None;

            InitializeComponent();
            SensorUnitLoad();

            MaintenanceMode = false;
            btnOk.Visible = true;
            btnCancel.Visible = true;
            tabUnit.Tabs[0].Visible = false;

            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ParameterResponse, RecvParameter);  //OKをクリックした時用

            setCulture();
            ComFunc.SetControlSettings(this);
        }

        #region リソース設定
        private void setCulture()
        {
            tabUnit.Tabs[0].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_000;
            tabUnit.Tabs[1].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_001;

            //センサーステータスタブ
            lblCaseDoorDetective.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_002;
            lblDrainBoxFull.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_003;
            lblUsableDrainBox.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_004;
            lblUsableTipCellCaseTransfer.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_005;
            lblUsableTipCellCase.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_006;
            lblReagStorageCoverDetective.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_007;
            lblUsableRReagBottle.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_008;
            lblUsableMReagBottle.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_009;
            lblUsableDispenceTipCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_010;
            lblUsableReactionCellCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_011;
            lblReactionCellSettingCheckOuter.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_012;
            lblReactionCellSettingCheckInner.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_013;
            lblReactionCellSettingCheckSettingPosition.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_014;
            lblReactionCellSettingCheckBF1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_015;
            lblReactionCellSettingCheckBF2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_016;
            lblR1MixingZThetaCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_017;
            lblR2MixingZThetaCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_018;
            lblBF1MixingZThetaCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_019;
            lblBF2MixingZThetaCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_020;
            lblPTrMixingZThetaCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_021;
            lblPhotometryShutterSolenoidCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_022;
            lblReagDispense1NozzleClashDetective.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_023;
            lblReagDispense2NozzleClashDetective.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_024;
            lblBF1Nozzle1DrainCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_095;
            lblBF1Nozzle2DrainCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_096;
            lblBF2Nozzle1DrainCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_097;
            lblBF2Nozzle2DrainCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_098;
            lblBF2Nozzle3DrainCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_099;
            lblUsableWashBuffer.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_025;
            lblWashBufferFull.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_026;
            lblDrainBufferFull.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_027;
            lblUsablePreTrigger1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_028;
            lblUsablePreTrigger2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_029;
            lblUsableTrigger1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_030;
            lblUsableTrigger2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_031;
            lblUsableDiluent.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_032;
            lblUsablePurifiedWater.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_033;
            lblPressWashPullInPump.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_034;
            lblPressDrainPump1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_035;
            lblPressDrainPump2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_036;
            lblPressDrainPump3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_037;
            lblPressDrainPump4.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_038;
            lblPressExtracorporealDrainPump.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_039;
            lblSTATTubeCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_105;
            lblR1MReagentOpenClose.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_106;
            lblR2MReagentOpenClose.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_107;
            lblCellDisposeCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_108;
            lblSTATSwitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_109;
            lblReagentTableTurnSwitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_110;
            lblSampleAspirationData.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_111;
            lblSampleAspirationDataDsp.Text = "";   //値を受信するまでは何も表示しない

            gbxStatusCaseTransfer.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_082;
            gbxStatusReagentStorage.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_083;
            gbxStatusSampleDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_084;
            gbxStatusReactionCellTransfer.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_085;
            gbxStatusReactionTable.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_086;
            gbxStatusBFTable.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_087;
            gbxStatusTriggerDisupense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_088;
            gbxStatusR1Dispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_089;
            gbxStatusR2Dispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_090;
            gbxStatusBF1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_093;
            gbxStatusBF2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_094;
            gbxStatusFluidandPiping.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_091;
            gbxStatusMainFlame.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_092;
            gbxStatusSTAT.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_112;

            //Senser Use/NoUse
            gbxCaseDoorDetective.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_040;
            gbxDrainBoxFull.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_041;
            gbxUsableDrainBox.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_042;
            gbxUsableTipCellCaseTransfer.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_043;
            gbxUsableTipCellCase.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_044;
            gbxReagStorageCoverDetective.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_045;
            gbxUsableRReagBottle.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_046;
            gbxUsableMReagBottle.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_047;
            gbxUsableDispenceTipCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_048;
            gbxUsableReactionCellCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_049;
            gbxReactionCellSettingCheckOuter.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_050;
            gbxReactionCellSettingCheckInner.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_051;
            gbxReactionCellSettingCheckSettingPosition.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_052;
            gbxReactionCellSettingCheckBF1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_053;
            gbxReactionCellSettingCheckBF2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_054;
            gbxR1MixingZThetaCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_055;
            gbxR2MixingZThetaCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_056;
            gbxBF1MixingZThetaCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_057;
            gbxBF2MixingZThetaCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_058;
            gbxPTrMixingZThetaCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_059;
            gbxPhotometryShutterSolenoidCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_060;
            gbxReagDispense1NozzleClashDetective.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_061;
            gbxReagDispense2NozzleClashDetective.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_062;
            gbxBF1Nozzle1DrainCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_100;
            gbxBF1Nozzle2DrainCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_101;
            gbxBF2Nozzle1DrainCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_102;
            gbxBF2Nozzle2DrainCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_103;
            gbxBF2Nozzle3DrainCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_104;
            gbxUsableWashBuffer.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_063;
            gbxWashBufferFull.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_064;
            gbxDrainBufferFull.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_065;
            gbxUsablePreTrigger1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_066;
            gbxUsablePreTrigger2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_067;
            gbxUsableTrigger1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_068;
            gbxUsableTrigger2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_069;
            gbxUsableDiluent.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_070;
            gbxUsablePurifiedWater.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_071;
            gbxPressWashPullInPump.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_072;
            gbxPressDrainPump1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_073;
            gbxPressDrainPump2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_074;
            gbxPressDrainPump3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_075;
            gbxPressDrainPump4.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_076;
            gbxPressExtracorporealDrainPump.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_077;
            gbxSTATTubeCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_113;
            gbxR1MReagentOpenClose.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_114;
            gbxR2MReagentOpenClose.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_115;
            gbxCellDisposeCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_116;
            gbxSTATSwitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_117;
            gbxReagentTableTurnSwitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_118;

            gbxUsableCaseTransfer.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_082;
            gbxUsableReagentStorage.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_083;
            gbxUsableSampleDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_084;
            gbxUsableReactionCellTransfer.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_085;
            gbxUsableReactionTable.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_086;
            gbxUsableBFTable.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_087;
            gbxUsableTriggerDisupense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_088;
            gbxUsableR1Dispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_089;
            gbxUsableR2Dispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_090;
            gbxUsableBF1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_093;
            gbxUsableBF2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_094;
            gbxUsableFluidandPiping.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_091;
            gbxUsableMainFlame.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_092;
            gbxUsableSTAT.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_112;

            //use
            rbtCaseDoorDetectiveUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtDrainBoxFullUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtUsableDrainBoxUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtUsableTipCellCaseTransferUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtUsableTipCellCaseUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtReagStorageCoverDetectiveUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtUsableRReagBottleUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtUsableMReagBottleUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtUsableDispenceTipCatchUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtUsableReactionCellCatchUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtReactionCellSettingCheckOuterUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtReactionCellSettingCheckInnerUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtReactionCellSettingCheckSettingPositionUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtReactionCellSettingCheckBF1Use.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtReactionCellSettingCheckBF2Use.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtR1MixingZThetaCheckUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtR2MixingZThetaCheckUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtBF1MixingZThetaCheckUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtBF2MixingZThetaCheckUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtPTrMixingZThetaCheckUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtPhotometryShutterSolenoidCheckUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtReagDispense1NozzleClashDetectiveUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtReagDispense2NozzleClashDetectiveUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtBF1Nozzle1DrainCheckUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtBF1Nozzle2DrainCheckUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtBF2Nozzle1DrainCheckUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtBF2Nozzle2DrainCheckUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtBF2Nozzle3DrainCheckUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtUsableWashBufferUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtWashBufferFullUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtDrainBufferFullUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtUsablePreTrigger1Use.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtUsablePreTrigger2Use.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtUsableTrigger1Use.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtUsableTrigger2Use.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtUsableDiluentUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtUsablePurifiedWaterUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtPressWashPullInPumpUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtPressDrainPump1Use.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtPressDrainPump2Use.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtPressDrainPump3Use.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtPressDrainPump4Use.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtPressExtracorporealDrainPumpUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtSTATTubeCheckUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtR1MReagentOpenCloseUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtR2MReagentOpenCloseUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtCellDisposeCheckUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtSTATSwitchUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            rbtReagentTableTurnSwitchUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;

            //NoUse
            rbtCaseDoorDetectiveNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtDrainBoxFullNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtUsableDrainBoxNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtUsableTipCellCaseTransferNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtUsableTipCellCaseNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtReagStorageCoverDetectiveNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtUsableRReagBottleNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtUsableMReagBottleNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtUsableDispenceTipCatchNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtUsableReactionCellCatchNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtReactionCellSettingCheckOuterNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtReactionCellSettingCheckInnerNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtReactionCellSettingCheckSettingPositionNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtReactionCellSettingCheckBF1NoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtReactionCellSettingCheckBF2NoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtR1MixingZThetaCheckNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtR2MixingZThetaCheckNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtBF1MixingZThetaCheckNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtBF2MixingZThetaCheckNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtPTrMixingZThetaCheckNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtPhotometryShutterSolenoidCheckNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtReagDispense1NozzleClashDetectiveNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtReagDispense2NozzleClashDetectiveNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtBF1Nozzle1DrainCheckNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtBF1Nozzle2DrainCheckNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtBF2Nozzle1DrainCheckNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtBF2Nozzle2DrainCheckNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtBF2Nozzle3DrainCheckNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtUsableWashBufferNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtWashBufferFullNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtDrainBufferFullNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtUsablePreTrigger1NoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtUsablePreTrigger2NoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtUsableTrigger1NoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtUsableTrigger2NoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtUsableDiluentNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtUsablePurifiedWaterNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtPressWashPullInPumpNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtPressDrainPump1NoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtPressDrainPump2NoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtPressDrainPump3NoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtPressDrainPump4NoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtPressExtracorporealDrainPumpNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtSTATTubeCheckNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtR1MReagentOpenCloseNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtR2MReagentOpenCloseNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtCellDisposeCheckNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtSTATSwitchNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            rbtReagentTableTurnSwitchNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;

            btnOk.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_080;
            btnCancel.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_081;

        }
        #endregion

        /// <summary>
        /// ユニットをスタートします。
        /// </summary>
        public override void UnitStart()
        {
            DispEnable(false);
            Singleton<CarisXSequenceHelperManager>.Instance.Maintenance.ExecuteSeqence(UnitStartPrc);
        }

        /// <summary>
        /// ユニットスタート処理をおこないます。
        /// </summary>
        private String UnitStartPrc(String str, object[] param)
        {
            SlaveCommCommand_0440 StartComm = new SlaveCommCommand_0440();

            while (true)
            {
                // ポーズ処理
                while (PauseFlg) break;

                // 中断処理
                if (AbortFlg)
                {
                    AbortFlg = false;
                    break;
                }

                // 受信待ちフラグを設定
                ResponseFlg = true;

                // コマンド送信
                unitStart.Start(StartComm);

                // 送信後少し待つ
                System.Threading.Thread.Sleep(300);

                // 応答コマンド受信待ち
                while (ResponseFlg)
                {
                    if (AbortFlg)
                    {
                        // 中断要求により受信待ち解除
                        break;
                    }
                }
            }

            AbortFlg = false;

            // メイン画面のナビゲータバーをEnableコマンドバーをデフォルトに戻す。
            Action Disp = () => { DispEnable(true); }; Invoke(Disp);

            return str;
        }

        /// <summary>
        /// 受信データ処理をおこないます。
        /// </summary>
        public override void SetResponse(CommCommandEventArgs comm)
        {
            switch (comm.Command.CommandId)
            {
                case (int)CommandKind.Command1440:
                    //モーター微調整開始
                    SlaveCommCommand_1440 ds1440 = new SlaveCommCommand_1440();
                    ds1440 = (SlaveCommCommand_1440)comm.Command;

                    //ケース扉検知センサ
                    SetSenserStatusONOFF(ds1440.CaseDoorDetective, lblCaseDoorDetectiveDsp);
                    //廃棄ボックス満杯センサ
                    SetSenserStatusONOFF(ds1440.DrainBoxFull, lblDrainBoxFullDsp);
                    //廃棄ボックス有無センサ
                    SetSenserStatusONOFF(ds1440.UsableDrainBox, lblUsableDrainBoxDsp);

                    //ケース搬送有無センサ
                    SetSenserStatusONOFF(ds1440.UsableTipCellCaseTransfer, lblUsableTipCellCaseTransferDsp);
                    //ケース有無センサ
                    SetSenserStatusONOFF(ds1440.UsableTipCellCase, lblUsableTipCellCaseDsp);

                    //試薬保冷庫カバー検知センサ
                    SetSenserStatusONOFF(ds1440.ReagStorageCoverDetective, lblReagStorageCoverDetectiveDsp);
                    //R試薬ボトル有無センサ
                    SetSenserStatusONOFF(ds1440.UsableRReagBottle, lblUsableRReagBottleDsp);
                    //M試薬ボトル有無センサ
                    SetSenserStatusONOFF(ds1440.UsableMReagBottle, lblUsableMReagBottleDsp);

                    //分注チップキャッチ有無センサ
                    SetSenserStatusONOFF(ds1440.UsableDispenceTipCatch, lblUsableDispenceTipCatchDsp);

                    //反応容器キャッチ有無センサ
                    SetSenserStatusONOFF(ds1440.UsableReactionCellCatch, lblUsableReactionCellCatchDsp);

                    //反応容器設置確認(外側)センサ
                    SetSenserStatusONOFF(ds1440.ReactionCellSettingCheckOuter, lblReactionCellSettingCheckOuterDsp);
                    //反応容器設置確認(内側)センサ
                    SetSenserStatusONOFF(ds1440.ReactionCellSettingCheckInner, lblReactionCellSettingCheckInnerDsp);
                    //反応容器設置確認(設置部)センサ
                    SetSenserStatusONOFF(ds1440.ReactionCellSettingCheckSettingPosition, lblReactionCellSettingCheckSettingPositionDsp);

                    //反応容器設置確認(BF1)センサ
                    SetSenserStatusONOFF(ds1440.ReactionCellSettingCheckBF1, lblReactionCellSettingCheckBF1Dsp);
                    //反応容器設置確認(BF2)センサ
                    SetSenserStatusONOFF(ds1440.ReactionCellSettingCheckBF2, lblReactionCellSettingCheckBF2Dsp);

                    //R1撹拌Zθ確認センサ
                    SetSenserStatusONOFF(ds1440.R1MixingZThetaCheck, lblR1MixingZThetaCheckDsp);
                    //R2撹拌Zθ確認センサ
                    SetSenserStatusONOFF(ds1440.R2MixingZThetaCheck, lblR2MixingZThetaCheckDsp);
                    //BF1撹拌Zθ確認センサ
                    SetSenserStatusONOFF(ds1440.BF1MixingZThetaCheck, lblBF1MixingZThetaCheckDsp);
                    //BF2撹拌Zθ確認センサ
                    SetSenserStatusONOFF(ds1440.BF2MixingZThetaCheck, lblBF2MixingZThetaCheckDsp);
                    //PTr撹拌Zθ確認センサ
                    SetSenserStatusONOFF(ds1440.PTrMixingZThetaCheck, lblPTrMixingZThetaCheckDsp);

                    //測光部シャッターソレノイド確認センサ
                    SetSenserStatusONOFF(ds1440.PhotometryShutterSolenoidCheck, lblPhotometryShutterSolenoidCheckDsp);

                    //試薬分注1ノズル衝突検知センサ
                    SetSenserStatusONOFF(ds1440.ReagDispense1NozzleClashDetective, lblReagDispense1NozzleClashDetectiveDsp);

                    //試薬分注2ノズル衝突検知センサ
                    SetSenserStatusONOFF(ds1440.ReagDispense2NozzleClashDetective, lblReagDispense2NozzleClashDetectiveDsp);

                    //BF1ノズル1 廃液確認センサ
                    SetSenserStatusONOFF(ds1440.BF1Nozzle1DrainCheck, lblBF1Nozzle1DrainCheckDsp);
                    //BF1ノズル2 廃液確認センサ
                    SetSenserStatusONOFF(ds1440.BF1Nozzle2DrainCheck, lblBF1Nozzle2DrainCheckDsp);

                    //BF2ノズル1 廃液確認センサ
                    SetSenserStatusONOFF(ds1440.BF2Nozzle1DrainCheck, lblBF2Nozzle1DrainCheckDsp);
                    //BF2ノズル2 廃液確認センサ
                    SetSenserStatusONOFF(ds1440.BF2Nozzle2DrainCheck, lblBF2Nozzle2DrainCheckDsp);
                    //BF2ノズル3 廃液確認センサ
                    SetSenserStatusONOFF(ds1440.BF2Nozzle3DrainCheck, lblBF2Nozzle3DrainCheckDsp);

                    //洗浄液バッファ有無センサ(残量分検知できる位置へ変更)
                    SetSenserStatusONOFF(ds1440.UsableWashBuffer, lblUsableWashBufferDsp);
                    //洗浄液バッファ満杯センサ
                    SetSenserStatusONOFF(ds1440.WashBufferFull, lblWashBufferFullDsp);
                    //廃液バッファ満杯センサ(導通基板へ)
                    SetSenserStatusONOFF(ds1440.DrainBufferFull, lblDrainBufferFullDsp);
                    //プレトリガ1液有無センサ
                    SetSenserStatusONOFF(ds1440.UsablePreTrigger1, lblUsablePreTrigger1Dsp);
                    //プレトリガ2液有無センサ
                    SetSenserStatusONOFF(ds1440.UsablePreTrigger2, lblUsablePreTrigger2Dsp);
                    //トリガ1液有無センサ
                    SetSenserStatusONOFF(ds1440.UsableTrigger1, lblUsableTrigger1Dsp);
                    //トリガ2液有無センサ
                    SetSenserStatusONOFF(ds1440.UsableTrigger2, lblUsableTrigger2Dsp);
                    //希釈液有無センサ(導通基板へ)
                    SetSenserStatusONOFF(ds1440.UsableDiluent, lblUsableDiluentDsp);
                    //精製水有無センサ
                    SetSenserStatusONOFF(ds1440.UsablePurifiedWater, lblUsablePurifiedWaterDsp);
                    //洗浄液引込ポンプ用圧力センサ
                    SetSenserStatusONOFF(ds1440.PressWashPullInPump, lblPressWashPullInPumpDsp);
                    //廃液ポンプ1用圧力センサ
                    SetSenserStatusONOFF(ds1440.PressDrainPump1, lblPressDrainPump1Dsp);
                    //廃液ポンプ2用圧力センサ
                    SetSenserStatusONOFF(ds1440.PressDrainPump2, lblPressDrainPump2Dsp);
                    //廃液ポンプ3用圧力センサ
                    SetSenserStatusONOFF(ds1440.PressDrainPump3, lblPressDrainPump3Dsp);
                    //廃液ポンプ4用圧力センサ
                    SetSenserStatusONOFF(ds1440.PressDrainPump4, lblPressDrainPump4Dsp);
                    //体外廃液ポンプ用圧力センサ
                    SetSenserStatusONOFF(ds1440.PressExtracorporealDrainPump, lblPressExtracorporealDrainPumpDsp);
                    //スタット容器有無センサ
                    SetSenserStatusONOFF(ds1440.STATTubeCheck, lblSTATTubeCheckDsp);
                    //試薬蓋開閉センサ1
                    SetSenserStatusONOFF(ds1440.R1MReagentOpenClose, lblR1MReagentOpenCloseDsp);
                    //試薬蓋開閉センサ2
                    SetSenserStatusONOFF(ds1440.R2MReagentOpenClose, lblR2MReagentOpenCloseDsp);
                    //反応容器廃棄確認センサ
                    SetSenserStatusONOFF(ds1440.CellDisposeCheck, lblCellDisposeCheckDSP);
                    //STAT設置スイッチ
                    SetSenserStatusONOFF(ds1440.STATSwitch, lblSTATSwitchDsp);
                    //試薬保冷庫回転スイッチ
                    SetSenserStatusONOFF(ds1440.ReagentTableTurnSwitch, lblReagentTableTurnSwitchDsp);
                    //検体液面検知センサ基板
                    lblSampleAspirationDataDsp.Text = ds1440.SampleAspirationData.ToString();


                    ResponseFlg = false;
                    break;

                case (int)CommandKind.Command1412:
                    //ポーズ、再開、停止のいずれか
                    switch (LastestPauseCtl)
                    {
                        case CommandControlParameter.Abort:
                            //停止処理時
                            PauseFlg = false;
                            AbortFlg = true;

                            this.Enabled = true;
                            break;
                        case CommandControlParameter.Pause:
                            //ポーズ処理時
                            //レスポンスを受けてする処理なし
                            break;
                        case CommandControlParameter.Restart:
                            //再開処理時
                            PauseFlg = false;
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// センサーステータスON/OFF反映処理
        /// </summary>
        /// <param name="SenserStatus">センサーステータス値</param>
        /// <param name="lbl">ON/OFFの表示を行うラベル</param>
        public void SetSenserStatusONOFF(byte SenserStatus, Infragistics.Win.Misc.UltraLabel lbl)
        {
            if (SenserStatus == (byte)SenserStatusKind.ON)
            {
                lbl.Appearance.ForeColor = System.Drawing.Color.OrangeRed;
                lbl.Text = Properties.Resources_Maintenance.ON;
            }
            else
            {
                lbl.Appearance.ForeColor = System.Drawing.Color.Black;
                lbl.Text = Properties.Resources_Maintenance.OFF;
            }
        }

        /// <summary>
        /// センサーパラメータを保存します。
        /// </summary>
        public override void ParamSave()
        {
            ParameterFilePreserve<CarisXSensorParameter> config = Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance;
            SensorParamLoad(config);

            CarisXSensorParameter.SensorParameterUseNoUseSlave sensorParam = new CarisXSensorParameter.SensorParameterUseNoUseSlave();

            sensorParam.CaseDoorDetective = (byte)ComFunc.getSelectedRadioButtonValue(gbxCaseDoorDetective);
            sensorParam.DrainBoxFull = (byte)ComFunc.getSelectedRadioButtonValue(gbxDrainBoxFull);
            sensorParam.UsableDrainBox = (byte)ComFunc.getSelectedRadioButtonValue(gbxUsableDrainBox);
            sensorParam.UsableTipCellCaseTransfer = (byte)ComFunc.getSelectedRadioButtonValue(gbxUsableTipCellCaseTransfer);
            sensorParam.UsableTipCellCase = (byte)ComFunc.getSelectedRadioButtonValue(gbxUsableTipCellCase);
            sensorParam.ReagStorageCoverDetective = (byte)ComFunc.getSelectedRadioButtonValue(gbxReagStorageCoverDetective);
            sensorParam.UsableRReagBottle = (byte)ComFunc.getSelectedRadioButtonValue(gbxUsableRReagBottle);
            sensorParam.UsableMReagBottle = (byte)ComFunc.getSelectedRadioButtonValue(gbxUsableMReagBottle);
            sensorParam.UsableDispenceTipCatch = (byte)ComFunc.getSelectedRadioButtonValue(gbxUsableDispenceTipCatch);
            sensorParam.UsableReactionCellCatch = (byte)ComFunc.getSelectedRadioButtonValue(gbxUsableReactionCellCatch);
            sensorParam.ReactionCellSettingCheckOuter = (byte)ComFunc.getSelectedRadioButtonValue(gbxReactionCellSettingCheckOuter);
            sensorParam.ReactionCellSettingCheckInner = (byte)ComFunc.getSelectedRadioButtonValue(gbxReactionCellSettingCheckInner);
            sensorParam.ReactionCellSettingCheckSettingPosition = (byte)ComFunc.getSelectedRadioButtonValue(gbxReactionCellSettingCheckSettingPosition);
            sensorParam.ReactionCellSettingCheckBF1 = (byte)ComFunc.getSelectedRadioButtonValue(gbxReactionCellSettingCheckBF1);
            sensorParam.ReactionCellSettingCheckBF2 = (byte)ComFunc.getSelectedRadioButtonValue(gbxReactionCellSettingCheckBF2);
            sensorParam.R1MixingZThetaCheck = (byte)ComFunc.getSelectedRadioButtonValue(gbxR1MixingZThetaCheck);
            sensorParam.R2MixingZThetaCheck = (byte)ComFunc.getSelectedRadioButtonValue(gbxR2MixingZThetaCheck);
            sensorParam.BF1MixingZThetaCheck = (byte)ComFunc.getSelectedRadioButtonValue(gbxBF1MixingZThetaCheck);
            sensorParam.BF2MixingZThetaCheck = (byte)ComFunc.getSelectedRadioButtonValue(gbxBF2MixingZThetaCheck);
            sensorParam.PTrMixingZThetaCheck = (byte)ComFunc.getSelectedRadioButtonValue(gbxPTrMixingZThetaCheck);
            sensorParam.PhotometryShutterSolenoidCheck = (byte)ComFunc.getSelectedRadioButtonValue(gbxPhotometryShutterSolenoidCheck);
            sensorParam.ReagDispense1NozzleClashDetective = (byte)ComFunc.getSelectedRadioButtonValue(gbxReagDispense1NozzleClashDetective);
            sensorParam.ReagDispense2NozzleClashDetective = (byte)ComFunc.getSelectedRadioButtonValue(gbxReagDispense2NozzleClashDetective);
            sensorParam.BF1Nozzle1DrainCheck = (byte)ComFunc.getSelectedRadioButtonValue(gbxBF1Nozzle1DrainCheck);
            sensorParam.BF1Nozzle2DrainCheck = (byte)ComFunc.getSelectedRadioButtonValue(gbxBF1Nozzle2DrainCheck);
            sensorParam.BF2Nozzle1DrainCheck = (byte)ComFunc.getSelectedRadioButtonValue(gbxBF2Nozzle1DrainCheck);
            sensorParam.BF2Nozzle2DrainCheck = (byte)ComFunc.getSelectedRadioButtonValue(gbxBF2Nozzle2DrainCheck);
            sensorParam.BF2Nozzle3DrainCheck = (byte)ComFunc.getSelectedRadioButtonValue(gbxBF2Nozzle3DrainCheck);
            sensorParam.UsableWashBuffer = (byte)ComFunc.getSelectedRadioButtonValue(gbxUsableWashBuffer);
            sensorParam.WashBufferFull = (byte)ComFunc.getSelectedRadioButtonValue(gbxWashBufferFull);
            sensorParam.DrainBufferFull = (byte)ComFunc.getSelectedRadioButtonValue(gbxDrainBufferFull);
            sensorParam.UsablePreTrigger1 = (byte)ComFunc.getSelectedRadioButtonValue(gbxUsablePreTrigger1);
            sensorParam.UsablePreTrigger2 = (byte)ComFunc.getSelectedRadioButtonValue(gbxUsablePreTrigger2);
            sensorParam.UsableTrigger1 = (byte)ComFunc.getSelectedRadioButtonValue(gbxUsableTrigger1);
            sensorParam.UsableTrigger2 = (byte)ComFunc.getSelectedRadioButtonValue(gbxUsableTrigger2);
            sensorParam.UsableDiluent = (byte)ComFunc.getSelectedRadioButtonValue(gbxUsableDiluent);
            sensorParam.UsablePurifiedWater = (byte)ComFunc.getSelectedRadioButtonValue(gbxUsablePurifiedWater);
            sensorParam.PressWashPullInPump = (byte)ComFunc.getSelectedRadioButtonValue(gbxPressWashPullInPump);
            sensorParam.PressDrainPump1 = (byte)ComFunc.getSelectedRadioButtonValue(gbxPressDrainPump1);
            sensorParam.PressDrainPump2 = (byte)ComFunc.getSelectedRadioButtonValue(gbxPressDrainPump2);
            sensorParam.PressDrainPump3 = (byte)ComFunc.getSelectedRadioButtonValue(gbxPressDrainPump3);
            sensorParam.PressDrainPump4 = (byte)ComFunc.getSelectedRadioButtonValue(gbxPressDrainPump4);
            sensorParam.PressExtracorporealDrainPump = (byte)ComFunc.getSelectedRadioButtonValue(gbxPressExtracorporealDrainPump);
            sensorParam.STATTubeCheck = (byte)ComFunc.getSelectedRadioButtonValue(gbxSTATTubeCheck);
            sensorParam.R1MReagentOpenClose = (byte)ComFunc.getSelectedRadioButtonValue(gbxR1MReagentOpenClose);
            sensorParam.R2MReagentOpenClose = (byte)ComFunc.getSelectedRadioButtonValue(gbxR2MReagentOpenClose);
            sensorParam.CellDisposeCheck = (byte)ComFunc.getSelectedRadioButtonValue(gbxCellDisposeCheck);
            sensorParam.STATSwitch = (byte)ComFunc.getSelectedRadioButtonValue(gbxSTATSwitch);
            sensorParam.ReagentTableTurnSwitch = (byte)ComFunc.getSelectedRadioButtonValue(gbxReagentTableTurnSwitch);

            config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse = sensorParam;

            //パラメータ送信
            SendParam(sensorParam);

        }

        /// <summary>
        /// パラメータ保存コマンドに対するレスポンスが返ってきた際の処理
        /// </summary>
        /// <param name="SendResult">レスポンスが返ってきたかどうか</param>
        public override void SetParameterResponse(bool SendResult)
        {
            if (SendResult)
            {
                switch (tabUnit.SelectedTab.Index)
                {
                    case (int)MaintenanceTabIndex.Config:
                        ParameterFilePreserve<CarisXSensorParameter> config = Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance;
                        sensorParamSave(config);
                        break;
                }
            }
            else
            {
                // 送信失敗時
                // レスポンスが無いため、パラメータを保存しませんでした
                Oelco.CarisX.GUI.DlgMessage.Show(Oelco.CarisX.Properties.Resources_Maintenance.STRING_MAINTENANCE_MESSAGE_000
                                                , String.Empty
                                                , Oelco.CarisX.Properties.Resources_Maintenance.STRING_DLG_TITLE_000
                                                , Oelco.CarisX.GUI.MessageDialogButtons.OK);
            }

            if (MaintenanceMode)
            {
                //メイン画面のナビゲータバーをEnableコマンドバーをデフォルトに戻す。
                Action Disp = () => { DispEnable(true); }; Invoke(Disp);

                ToolbarsControl();
            }
            else
            {
                //障害対策時は画面を閉じる
                //Action formClose = () => { this.Close(); }; Invoke(formClose);
                this.Close();
            }

        }

        /// <summary>
        /// センサーパラメータを読み込みます。
        /// </summary>
        public void SensorUnitLoad()
        {
            ParameterFilePreserve<CarisXSensorParameter> config = Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance;
            SensorParamLoad(config);

            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.CaseDoorDetective, rbtCaseDoorDetectiveUse, rbtCaseDoorDetectiveNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.DrainBoxFull, rbtDrainBoxFullUse, rbtDrainBoxFullNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.UsableDrainBox, rbtUsableDrainBoxUse, rbtUsableDrainBoxNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.UsableTipCellCaseTransfer, rbtUsableTipCellCaseTransferUse, rbtUsableTipCellCaseTransferNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.UsableTipCellCase, rbtUsableTipCellCaseUse, rbtUsableTipCellCaseNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.ReagStorageCoverDetective, rbtReagStorageCoverDetectiveUse, rbtReagStorageCoverDetectiveNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.UsableRReagBottle, rbtUsableRReagBottleUse, rbtUsableRReagBottleNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.UsableMReagBottle, rbtUsableMReagBottleUse, rbtUsableMReagBottleNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.UsableDispenceTipCatch, rbtUsableDispenceTipCatchUse, rbtUsableDispenceTipCatchNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.UsableReactionCellCatch, rbtUsableReactionCellCatchUse, rbtUsableReactionCellCatchNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.ReactionCellSettingCheckOuter, rbtReactionCellSettingCheckOuterUse, rbtReactionCellSettingCheckOuterNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.ReactionCellSettingCheckInner, rbtReactionCellSettingCheckInnerUse, rbtReactionCellSettingCheckInnerNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.ReactionCellSettingCheckSettingPosition, rbtReactionCellSettingCheckSettingPositionUse, rbtReactionCellSettingCheckSettingPositionNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.ReactionCellSettingCheckBF1, rbtReactionCellSettingCheckBF1Use, rbtReactionCellSettingCheckBF1NoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.ReactionCellSettingCheckBF2, rbtReactionCellSettingCheckBF2Use, rbtReactionCellSettingCheckBF2NoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.R1MixingZThetaCheck, rbtR1MixingZThetaCheckUse, rbtR1MixingZThetaCheckNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.R2MixingZThetaCheck, rbtR2MixingZThetaCheckUse, rbtR2MixingZThetaCheckNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.BF1MixingZThetaCheck, rbtBF1MixingZThetaCheckUse, rbtBF1MixingZThetaCheckNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.BF2MixingZThetaCheck, rbtBF2MixingZThetaCheckUse, rbtBF2MixingZThetaCheckNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.PTrMixingZThetaCheck, rbtPTrMixingZThetaCheckUse, rbtPTrMixingZThetaCheckNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.PhotometryShutterSolenoidCheck, rbtPhotometryShutterSolenoidCheckUse, rbtPhotometryShutterSolenoidCheckNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.ReagDispense1NozzleClashDetective, rbtReagDispense1NozzleClashDetectiveUse, rbtReagDispense1NozzleClashDetectiveNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.ReagDispense2NozzleClashDetective, rbtReagDispense2NozzleClashDetectiveUse, rbtReagDispense2NozzleClashDetectiveNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.BF1Nozzle1DrainCheck, rbtBF1Nozzle1DrainCheckUse, rbtBF1Nozzle1DrainCheckNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.BF1Nozzle2DrainCheck, rbtBF1Nozzle2DrainCheckUse, rbtBF1Nozzle2DrainCheckNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.BF2Nozzle1DrainCheck, rbtBF2Nozzle1DrainCheckUse, rbtBF2Nozzle1DrainCheckNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.BF2Nozzle2DrainCheck, rbtBF2Nozzle2DrainCheckUse, rbtBF2Nozzle2DrainCheckNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.BF2Nozzle3DrainCheck, rbtBF2Nozzle3DrainCheckUse, rbtBF2Nozzle3DrainCheckNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.UsableWashBuffer, rbtUsableWashBufferUse, rbtUsableWashBufferNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.WashBufferFull, rbtWashBufferFullUse, rbtWashBufferFullNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.DrainBufferFull, rbtDrainBufferFullUse, rbtDrainBufferFullNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.UsablePreTrigger1, rbtUsablePreTrigger1Use, rbtUsablePreTrigger1NoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.UsablePreTrigger2, rbtUsablePreTrigger2Use, rbtUsablePreTrigger2NoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.UsableTrigger1, rbtUsableTrigger1Use, rbtUsableTrigger1NoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.UsableTrigger2, rbtUsableTrigger2Use, rbtUsableTrigger2NoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.UsableDiluent, rbtUsableDiluentUse, rbtUsableDiluentNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.UsablePurifiedWater, rbtUsablePurifiedWaterUse, rbtUsablePurifiedWaterNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.PressWashPullInPump, rbtPressWashPullInPumpUse, rbtPressWashPullInPumpNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.PressDrainPump1, rbtPressDrainPump1Use, rbtPressDrainPump1NoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.PressDrainPump2, rbtPressDrainPump2Use, rbtPressDrainPump2NoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.PressDrainPump3, rbtPressDrainPump3Use, rbtPressDrainPump3NoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.PressDrainPump4, rbtPressDrainPump4Use, rbtPressDrainPump4NoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.PressExtracorporealDrainPump, rbtPressExtracorporealDrainPumpUse, rbtPressExtracorporealDrainPumpNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.STATTubeCheck, rbtSTATTubeCheckUse, rbtSTATTubeCheckNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.R1MReagentOpenClose, rbtR1MReagentOpenCloseUse, rbtR1MReagentOpenCloseNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.R2MReagentOpenClose, rbtR2MReagentOpenCloseUse, rbtR2MReagentOpenCloseNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.CellDisposeCheck, rbtCellDisposeCheckUse, rbtCellDisposeCheckNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.STATSwitch, rbtSTATSwitchUse, rbtSTATSwitchNoUse);
            SetSenserUseNoUse(config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sensorParameterUseNoUse.ReagentTableTurnSwitch, rbtReagentTableTurnSwitchUse, rbtReagentTableTurnSwitchNoUse);
        }

        /// <summary>
        /// センサー使用/未使用設定
        /// </summary>
        /// <param name="SenserUseNoUse">使用/未使用の値</param>
        /// <param name="UseRbt">使用を表すラジオボタン</param>
        /// <param name="NoUseRbt">未使用を表すラジオボタン</param>
        public void SetSenserUseNoUse(byte SenserUseNoUse, RadioButton UseRbt, RadioButton NoUseRbt)
        {
            if (SenserUseNoUse == (byte)UseStatus.Use)
            {
                UseRbt.Checked = true;
            }
            else
            {
                NoUseRbt.Checked = true;
            }
        }

        /// <summary>
        /// Maintenanc画面のEnable/Disableを切り替えます。
        /// </summary>
        private void DispEnable(bool enable)
        {
            if (tabUnit.SelectedTab.Index == 0)
            {
                tabUnit.Controls[2].Enabled = enable;
            }
            else if (tabUnit.SelectedTab.Index == 1)
            {
                this.Enabled = enable;
            }
            MainToolbarsDispEnable(enable);
        }

        /// <summary>
        /// タブが切り替えられたときにメンテナンスメイン画面のコマンドバーの表示切り替えをおこないます。
        /// </summary>
        private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (MaintenanceMode) ToolbarsControl();
        }

        public override void ToolbarsControl(int toolBarEnablekind = -1)
        {
            //デリゲートでCall
            ToolbarsDisp(tabUnit.SelectedTab.Index, toolBarEnablekind);
        }

        /// <summary>
        /// OKボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            ParamSave();
        }

        /// <summary>
        /// キャンセルボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 各パラメータ保存コマンドに対するレスポンス受信時の処理
        /// </summary>
        /// <param name="Comm"></param>
        /// <remarks>障害対策用に表示した場合のみ使用</remarks>
        private void RecvParameter(object Comm)
        {
            Boolean SendResult;
            if (Comm is bool)
            {
                SendResult = (bool)Comm;
            }
            else
            {
                SendResult = false;
            }

            this.SetParameterResponse(SendResult);
        }

        /// <summary>
        /// フォームを閉じた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormSensorUnit_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!MaintenanceMode)
            {
                //障害対策版の場合、通知管理から削除する
                Singleton<NotifyManager>.Instance.RemoveNotifyTarget((Int32)NotifyKind.ParameterResponse, RecvParameter);
            }
        }
    }
}

