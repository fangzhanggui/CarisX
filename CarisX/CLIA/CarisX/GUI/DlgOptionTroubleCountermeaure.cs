using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.Utility;
using Oelco.CarisX.Common;
using Oelco.CarisX.Parameter;
using Oelco.Common.Parameter;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Utility;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 障害対策ダイアログクラス
    /// </summary>
    public partial class DlgOptionTroubleCountermeaure : DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgOptionTroubleCountermeaure()
        {
            InitializeComponent();
            SensorUnitLoad();
        }

        #endregion



        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            // パネル既定ボタン
            this.btnOk.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;

            // ダイアログタイトル
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONTROUBLECOUNTERMEASURE_000;

            // ケース搬送
            gbxUsableCaseTransfer.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_082;
            gbxUsableTipCellCaseTransfer.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_043;
            optUsableTipCellCaseTransfer.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optUsableTipCellCaseTransfer.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxUsableTipCellCase.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_044;
            optUsableTipCellCase.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optUsableTipCellCase.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;

            // 試薬保冷庫
            gbxUsableReagentStorage.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_083;
            gbxReagStorageCoverDetective.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_045;
            optReagStorageCoverDetective.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optReagStorageCoverDetective.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxUsableRReagBottle.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_046;
            optUsableRReagBottle.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optUsableRReagBottle.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxUsableMReagBottle.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_047;
            optUsableMReagBottle.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optUsableMReagBottle.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxR1MReagentOpenClose.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_114;
            optR1MReagentOpenClose.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optR1MReagentOpenClose.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxR2MReagentOpenClose.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_115;
            optR2MReagentOpenClose.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optR2MReagentOpenClose.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxReagentTableTurnSwitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_118;
            optReagentTableTurnSwitch.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optReagentTableTurnSwitch.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;

            // STAT部
            gbxUsableSTAT.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_112;
            gbxSTATTubeCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_113;
            optSTATTubeCheck.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optSTATTubeCheck.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxSTATSwitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_117;
            optSTATSwitch.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optSTATSwitch.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;

            // サンプル分注移送部
            gbxUsableSampleDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_084;
            gbxUsableDispenceTipCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_048;
            optUsableDispenceTipCatch.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optUsableDispenceTipCatch.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;

            // 反応容器搬送部
            gbxUsableReactionCellTransfer.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_085;
            gbxUsableReactionCellCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_049;
            optUsableReactionCellCatch.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optUsableReactionCellCatch.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;

            // 反応テーブル部
            gbxUsableReactionTable.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_086;
            gbxReactionCellSettingCheckOuter.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_050;
            optReactionCellSettingCheckOuter.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optReactionCellSettingCheckOuter.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxReactionCellSettingCheckInner.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_051;
            optReactionCellSettingCheckInner.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optReactionCellSettingCheckInner.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxReactionCellSettingCheckSettingPosition.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_052;
            optReactionCellSettingCheckSettingPosition.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optReactionCellSettingCheckSettingPosition.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxR1MixingZThetaCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_017;
            optR1MixingZThetaCheck.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optR1MixingZThetaCheck.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;

            // BFテーブル部
            gbxUsableBFTable.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_087;
            gbxReactionCellSettingCheckBF1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_053;
            optReactionCellSettingCheckBF1.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optReactionCellSettingCheckBF1.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxReactionCellSettingCheckBF2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_054;
            optReactionCellSettingCheckBF2.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optReactionCellSettingCheckBF2.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxR2MixingZThetaCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_056;
            optR2MixingZThetaCheck.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optR2MixingZThetaCheck.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxBF1MixingZThetaCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_057;
            optBF1MixingZThetaCheck.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optBF1MixingZThetaCheck.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxBF2MixingZThetaCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_058;
            optBF2MixingZThetaCheck.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optBF2MixingZThetaCheck.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxPTrMixingZThetaCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_059;
            optPTrMixingZThetaCheck.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optPTrMixingZThetaCheck.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;

            // トリガ分注
            gbxUsableTriggerDisupense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_088;
            gbxPhotometryShutterSolenoidCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_060;
            optPhotometryShutterSolenoidCheck.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optPhotometryShutterSolenoidCheck.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;

            // 試薬分注1部
            gbxUsableR1Dispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_089;
            gbxReagDispense1NozzleClashDetective.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_061;
            optReagDispense1NozzleClashDetective.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optReagDispense1NozzleClashDetective.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;

            // 試薬分注1部
            gbxUsableR2Dispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_090;
            gbxReagDispense2NozzleClashDetective.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_062;
            optReagDispense2NozzleClashDetective.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optReagDispense2NozzleClashDetective.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;

            // BF1部
            gbxUsableBF1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_093;
            gbxBF1Nozzle1DrainCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_100;
            optBF1Nozzle1DrainCheck.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optBF1Nozzle1DrainCheck.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxBF1Nozzle2DrainCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_101;
            optBF1Nozzle2DrainCheck.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optBF1Nozzle2DrainCheck.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;

            // BF2部
            gbxUsableBF2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_094;
            gbxBF2Nozzle1DrainCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_102;
            optBF2Nozzle1DrainCheck.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optBF2Nozzle1DrainCheck.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxBF2Nozzle2DrainCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_103;
            optBF2Nozzle2DrainCheck.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optBF2Nozzle2DrainCheck.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxBF2Nozzle3DrainCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_104;
            optBF2Nozzle3DrainCheck.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optBF2Nozzle3DrainCheck.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;

            // 流体配管部
            gbxUsableFluidandPiping.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_091;
            gbxUsableWashBuffer.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_063;
            optUsableWashBuffer.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optUsableWashBuffer.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxWashBufferFull.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_064;
            optWashBufferFull.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optWashBufferFull.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxDrainBufferFull.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_065;
            optDrainBufferFull.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optDrainBufferFull.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxUsablePreTrigger1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_066;
            optUsablePreTrigger1.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optUsablePreTrigger1.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxUsablePreTrigger2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_067;
            optUsablePreTrigger2.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optUsablePreTrigger2.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxUsableTrigger1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_068;
            optUsableTrigger1.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optUsableTrigger1.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxUsableTrigger2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_069;
            optUsableTrigger2.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optUsableTrigger2.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxUsableDiluent.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_070;
            optUsableDiluent.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optUsableDiluent.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxUsablePurifiedWater.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_071;
            optUsablePurifiedWater.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optUsablePurifiedWater.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxPressWashPullInPump.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_072;
            optPressWashPullInPump.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optPressWashPullInPump.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxPressDrainPump1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_073;
            optPressDrainPump1.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optPressDrainPump1.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxPressDrainPump2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_074;
            optPressDrainPump2.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optPressDrainPump2.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxPressDrainPump3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_075;
            optPressDrainPump3.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optPressDrainPump3.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxPressDrainPump4.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_076;
            optPressDrainPump4.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optPressDrainPump4.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxPressExtracorporealDrainPump.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_077;
            optPressExtracorporealDrainPump.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optPressExtracorporealDrainPump.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;

            // 本部フレーム部
            gbxUsableMainFlame.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_092;
            gbxCaseDoorDetective.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_040;
            optCaseDoorDetective.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optCaseDoorDetective.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxDrainBoxFull.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_041;
            optDrainBoxFull.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optDrainBoxFull.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxUsableDrainBox.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_042;
            optUsableDrainBox.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optUsableDrainBox.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;
            gbxCellDisposeCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_116;
            optCellDisposeCheck.Items[0].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_078;
            optCellDisposeCheck.Items[1].DisplayText = Properties.Resources_Maintenance.STRING_MAINTENANCE_SENSOR_079;

        }

        #region private

        /// <summary>
        /// センサーパラメータの読み込み
        /// </summary>
        private void SensorUnitLoad()
        {
            // ファイル保存されているセンサパラメータを取得
            CarisXSensorParameter.SensorParameterUseNoUseSlave sensorParam = Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance.Param.
                                                                              SlaveList[(Int32)Singleton<PublicMemory>.Instance.moduleIndex].sensorParameterUseNoUse;
            // ケース搬送
            SetSenserUseNoUse(sensorParam.UsableTipCellCaseTransfer, optUsableTipCellCaseTransfer);
            SetSenserUseNoUse(sensorParam.UsableTipCellCase, optUsableTipCellCase);

            // 試薬保冷庫
            SetSenserUseNoUse(sensorParam.ReagStorageCoverDetective, optReagStorageCoverDetective);
            SetSenserUseNoUse(sensorParam.UsableRReagBottle, optUsableRReagBottle);
            SetSenserUseNoUse(sensorParam.UsableMReagBottle, optUsableMReagBottle);
            SetSenserUseNoUse(sensorParam.R1MReagentOpenClose, optR1MReagentOpenClose);
            SetSenserUseNoUse(sensorParam.R2MReagentOpenClose, optR2MReagentOpenClose);
            SetSenserUseNoUse(sensorParam.ReagentTableTurnSwitch, optReagentTableTurnSwitch);

            //STAT部
            SetSenserUseNoUse(sensorParam.STATTubeCheck, optSTATTubeCheck);
            SetSenserUseNoUse(sensorParam.STATSwitch, optSTATSwitch);

            // サンプル分注移送部
            SetSenserUseNoUse(sensorParam.UsableDispenceTipCatch, optUsableDispenceTipCatch);

            // 反応容器搬送部
            SetSenserUseNoUse(sensorParam.UsableReactionCellCatch, optUsableReactionCellCatch);

            // 反応テーブル部
            SetSenserUseNoUse(sensorParam.ReactionCellSettingCheckOuter, optReactionCellSettingCheckOuter);
            SetSenserUseNoUse(sensorParam.ReactionCellSettingCheckInner, optReactionCellSettingCheckInner);
            SetSenserUseNoUse(sensorParam.ReactionCellSettingCheckSettingPosition, optReactionCellSettingCheckSettingPosition);
            SetSenserUseNoUse(sensorParam.R1MixingZThetaCheck, optR1MixingZThetaCheck);

            // BFテーブル部
            SetSenserUseNoUse(sensorParam.ReactionCellSettingCheckBF1, optReactionCellSettingCheckBF1);
            SetSenserUseNoUse(sensorParam.ReactionCellSettingCheckBF2, optReactionCellSettingCheckBF2);
            SetSenserUseNoUse(sensorParam.R2MixingZThetaCheck, optR2MixingZThetaCheck);
            SetSenserUseNoUse(sensorParam.BF1MixingZThetaCheck, optBF1MixingZThetaCheck);
            SetSenserUseNoUse(sensorParam.BF2MixingZThetaCheck, optBF2MixingZThetaCheck);
            SetSenserUseNoUse(sensorParam.PTrMixingZThetaCheck, optPTrMixingZThetaCheck);

            // トリガ分注
            SetSenserUseNoUse(sensorParam.PhotometryShutterSolenoidCheck, optPhotometryShutterSolenoidCheck);

            // 試薬分注1部
            SetSenserUseNoUse(sensorParam.ReagDispense1NozzleClashDetective, optReagDispense1NozzleClashDetective);

            // 試薬分注2部
            SetSenserUseNoUse(sensorParam.ReagDispense2NozzleClashDetective, optReagDispense2NozzleClashDetective);

            // BF1部
            SetSenserUseNoUse(sensorParam.BF1Nozzle1DrainCheck, optBF1Nozzle1DrainCheck);
            SetSenserUseNoUse(sensorParam.BF1Nozzle2DrainCheck, optBF1Nozzle2DrainCheck);

            // BF1部
            SetSenserUseNoUse(sensorParam.BF2Nozzle1DrainCheck, optBF2Nozzle1DrainCheck);
            SetSenserUseNoUse(sensorParam.BF2Nozzle2DrainCheck, optBF2Nozzle2DrainCheck);
            SetSenserUseNoUse(sensorParam.BF2Nozzle3DrainCheck, optBF2Nozzle3DrainCheck);

            // 流体配管部
            SetSenserUseNoUse(sensorParam.UsableWashBuffer, optUsableWashBuffer);
            SetSenserUseNoUse(sensorParam.WashBufferFull, optWashBufferFull);
            SetSenserUseNoUse(sensorParam.DrainBufferFull, optDrainBufferFull);
            SetSenserUseNoUse(sensorParam.UsablePreTrigger1, optUsablePreTrigger1);
            SetSenserUseNoUse(sensorParam.UsablePreTrigger2, optUsablePreTrigger2);
            SetSenserUseNoUse(sensorParam.UsableTrigger1, optUsableTrigger1);
            SetSenserUseNoUse(sensorParam.UsableTrigger2, optUsableTrigger2);
            SetSenserUseNoUse(sensorParam.UsableDiluent, optUsableDiluent);
            SetSenserUseNoUse(sensorParam.UsablePurifiedWater, optUsablePurifiedWater);
            SetSenserUseNoUse(sensorParam.PressWashPullInPump, optPressWashPullInPump);
            SetSenserUseNoUse(sensorParam.PressDrainPump1, optPressDrainPump1);
            SetSenserUseNoUse(sensorParam.PressDrainPump2, optPressDrainPump2);
            SetSenserUseNoUse(sensorParam.PressDrainPump3, optPressDrainPump3);
            SetSenserUseNoUse(sensorParam.PressDrainPump4, optPressDrainPump4);
            SetSenserUseNoUse(sensorParam.PressExtracorporealDrainPump, optPressExtracorporealDrainPump);

            // 本体フレーム部
            SetSenserUseNoUse(sensorParam.CaseDoorDetective, optCaseDoorDetective);
            SetSenserUseNoUse(sensorParam.DrainBoxFull, optDrainBoxFull);
            SetSenserUseNoUse(sensorParam.UsableDrainBox, optUsableDrainBox);
            SetSenserUseNoUse(sensorParam.CellDisposeCheck, optCellDisposeCheck);
        }

        /// <summary>
        /// センサー使用/未使用設定
        /// </summary>
        /// <param name="SenserUseNoUse">使用/未使用の値</param>
        /// <param name="optSet">使用/未使用を表すラジオボタン</param>
        private void SetSenserUseNoUse( byte SenserUseNoUse, Oelco.Common.GUI.CustomUOptionSet rBtn)
        {
            // 使用ありの場合
            if (SenserUseNoUse == (byte)UseStatus.Use)
            {
                // 使用ありのボタンにチェックを入れる
                rBtn.CheckedIndex = 0;
            }
            // 使用無しの場合
            else
            {
                // 使用無しのボタンにチェックを入れる
                rBtn.CheckedIndex = 1;
            }
        }

        /// <summary>
        /// センサーパラメータの送信
        /// </summary>
        private void SendSensorParameter()
        {
            CarisXSensorParameter.SensorParameterUseNoUseSlave sensorParam = new CarisXSensorParameter.SensorParameterUseNoUseSlave();

            // ケース搬送
            sensorParam.UsableTipCellCaseTransfer = (byte)optUsableTipCellCaseTransfer.CheckedIndex;
            sensorParam.UsableTipCellCase = (byte)optUsableTipCellCase.CheckedIndex;

            // 試薬保冷庫
            sensorParam.ReagStorageCoverDetective = (byte)optReagStorageCoverDetective.CheckedIndex;
            sensorParam.UsableRReagBottle = (byte)optUsableRReagBottle.CheckedIndex;
            sensorParam.UsableMReagBottle = (byte)optUsableMReagBottle.CheckedIndex;
            sensorParam.R1MReagentOpenClose = (byte)optR1MReagentOpenClose.CheckedIndex;
            sensorParam.R2MReagentOpenClose = (byte)optR2MReagentOpenClose.CheckedIndex;
            sensorParam.ReagentTableTurnSwitch = (byte)optReagentTableTurnSwitch.CheckedIndex;

            //STAT部
            sensorParam.STATTubeCheck = (byte)optSTATTubeCheck.CheckedIndex;
            sensorParam.STATSwitch = (byte)optSTATSwitch.CheckedIndex;

            // サンプル分注移送部
            sensorParam.UsableDispenceTipCatch = (byte)optUsableDispenceTipCatch.CheckedIndex;

            // 反応容器搬送部
            sensorParam.UsableReactionCellCatch = (byte)optUsableReactionCellCatch.CheckedIndex;

            // 反応テーブル部
            sensorParam.ReactionCellSettingCheckOuter = (byte)optReactionCellSettingCheckOuter.CheckedIndex;
            sensorParam.ReactionCellSettingCheckInner = (byte)optReactionCellSettingCheckInner.CheckedIndex;
            sensorParam.ReactionCellSettingCheckSettingPosition = (byte)optReactionCellSettingCheckSettingPosition.CheckedIndex;
            sensorParam.R1MixingZThetaCheck = (byte)optR1MixingZThetaCheck.CheckedIndex;

            // BFテーブル部
            sensorParam.ReactionCellSettingCheckBF1 = (byte)optReactionCellSettingCheckBF1.CheckedIndex;
            sensorParam.ReactionCellSettingCheckBF2 = (byte)optReactionCellSettingCheckBF2.CheckedIndex;
            sensorParam.R2MixingZThetaCheck = (byte)optR2MixingZThetaCheck.CheckedIndex;
            sensorParam.BF1MixingZThetaCheck = (byte)optBF1MixingZThetaCheck.CheckedIndex;
            sensorParam.BF2MixingZThetaCheck = (byte)optBF2MixingZThetaCheck.CheckedIndex;
            sensorParam.PTrMixingZThetaCheck = (byte)optPTrMixingZThetaCheck.CheckedIndex;

            // トリガ分注
            sensorParam.PhotometryShutterSolenoidCheck = (byte)optPhotometryShutterSolenoidCheck.CheckedIndex;

            // 試薬分注1部
            sensorParam.ReagDispense1NozzleClashDetective = (byte)optReagDispense1NozzleClashDetective.CheckedIndex;

            // 試薬分注2部
            sensorParam.ReagDispense2NozzleClashDetective = (byte)optReagDispense2NozzleClashDetective.CheckedIndex;

            // BF1部
            sensorParam.BF1Nozzle1DrainCheck = (byte)optBF1Nozzle1DrainCheck.CheckedIndex;
            sensorParam.BF1Nozzle2DrainCheck = (byte)optBF1Nozzle2DrainCheck.CheckedIndex;

            // BF2部
            sensorParam.BF2Nozzle1DrainCheck = (byte)optBF2Nozzle1DrainCheck.CheckedIndex;
            sensorParam.BF2Nozzle2DrainCheck = (byte)optBF2Nozzle2DrainCheck.CheckedIndex;
            sensorParam.BF2Nozzle3DrainCheck = (byte)optBF2Nozzle3DrainCheck.CheckedIndex;

            // 流体配管部
            sensorParam.UsableWashBuffer = (byte)optUsableWashBuffer.CheckedIndex;
            sensorParam.WashBufferFull = (byte)optWashBufferFull.CheckedIndex;
            sensorParam.DrainBufferFull = (byte)optDrainBufferFull.CheckedIndex;
            sensorParam.UsablePreTrigger1 = (byte)optUsablePreTrigger1.CheckedIndex;
            sensorParam.UsablePreTrigger2 = (byte)optUsablePreTrigger2.CheckedIndex;
            sensorParam.UsableTrigger1 = (byte)optUsableTrigger1.CheckedIndex;
            sensorParam.UsableTrigger2 = (byte)optUsableTrigger2.CheckedIndex;
            sensorParam.UsableDiluent = (byte)optUsableDiluent.CheckedIndex;
            sensorParam.UsablePurifiedWater = (byte)optUsablePurifiedWater.CheckedIndex;
            sensorParam.PressWashPullInPump = (byte)optPressWashPullInPump.CheckedIndex;
            sensorParam.PressDrainPump1 = (byte)optPressDrainPump1.CheckedIndex;
            sensorParam.PressDrainPump2 = (byte)optPressDrainPump2.CheckedIndex;
            sensorParam.PressDrainPump3 = (byte)optPressDrainPump3.CheckedIndex;
            sensorParam.PressDrainPump4 = (byte)optPressDrainPump4.CheckedIndex;
            sensorParam.PressExtracorporealDrainPump = (byte)optPressExtracorporealDrainPump.CheckedIndex;

            // 本体フレーム部
            sensorParam.CaseDoorDetective = (byte)optCaseDoorDetective.CheckedIndex;
            sensorParam.DrainBoxFull = (byte)optDrainBoxFull.CheckedIndex;
            sensorParam.UsableDrainBox = (byte)optUsableDrainBox.CheckedIndex;
            sensorParam.CellDisposeCheck = (byte)optCellDisposeCheck.CheckedIndex;

            // スレーブへのセンサー無効コマンド送信
            Singleton<CarisXSequenceHelperManager>.Instance.Slave[(Int32)Singleton<PublicMemory>.Instance.moduleIndex].SendSlaveSensorParameterUseNoUse(sensorParam);
        }

        /// <summary>
        /// Cancelボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にキャンセルを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnCancel_Click( object sender, EventArgs e )
        {
            this.Close();
        }

        #endregion

        /// <summary>
        /// OKボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にキャンセルを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnOk_Click( object sender, EventArgs e )
        {
            SendSensorParameter();
            this.Close();
        }
    }
}
