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
using Oelco.CarisX.Const;
using Oelco.CarisX.Properties;

namespace Oelco.CarisX.Maintenance
{
    /// <summary>
    /// 試薬分注1部
    /// </summary>
    public partial class FormR1DispenseUnit : FormUnitBase
    {
        IMaintenanceUnitStart unitStart = new UnitStart();
        IMaintenanceUnitStart unitStartAdjust = new UnitStarAdjust();
        IMaintenanceUnitStart unitAdjust = new UnitAdjust();
        IMaintenanceUnitStart unitAdjustAbort = new UnitStarAdjustAbort();
        volatile bool ResponseFlg;
        public CommonFunction ComFunc = new CommonFunction();

        //モーター調整コマンドチクチク
        SlaveCommCommand_0473 AdjustUpDownComm = new SlaveCommCommand_0473();
        public FormR1DispenseUnit()
        {
            InitializeComponent();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ParametersAllFalse();
            ConfigParamLoad();
            MotorParamDisp();
            setCulture();
            setRadioButtonValue();
            ComFunc.SetControlSettings(this);
            ConfigTabUseFlg = tabUnit.Tabs[(int)MaintenanceTabIndex.Config].Enabled;    //Configタブを利用有無を退避

            lbxtestSequenceListBox.SelectedIndex = 0;
            lbxAdjustSequence.SelectedIndex = 0;
            cmbAdjustCoarseFine.SelectedIndex = 0;

        }

        #region リソース設定
        private void setCulture()
        {
            //Tab
            tabUnit.Tabs[0].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_016;                        //Test
            tabUnit.Tabs[1].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_017;                        //Configuration
            tabUnit.Tabs[2].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_018;                        //Motor Parameters
            tabUnit.Tabs[3].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_MOTORADJUSTMENT;  //Adjust

            //Testタブ
            gbxtestSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_000;                              //Sequence
            object SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_001, No=(int)R1DispenceSequence.Init},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_002, No=(int)R1DispenceSequence.R1DispenseandProbeWashing},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_003, No=(int)R1DispenceSequence.R2DispenseandProbeWashing},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_004, No=(int)R1DispenceSequence.MDispenseandProbeWashing},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_005, No=(int)R1DispenceSequence.Prime},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_006, No=(int)R1DispenceSequence.Rinse},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_065, No=(int)R1DispenceSequence.MovetoR1washing},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_066, No=(int)R1DispenceSequence.MovetoR1aspirating},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_067, No=(int)R1DispenceSequence.MovetoR1dispensing},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_081, No=(int)R1DispenceSequence.DetectionSensor},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_090, No=(int)R1DispenceSequence.NozzleWash},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_099, No=(int)R1DispenceSequence.ProbeReplacement},
            };
            lbxtestSequenceListBox = ComFunc.setSequenceListBox(lbxtestSequenceListBox, SequenceList);

            gbxtestParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_008;
            lbltestRepeatFrequency.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_009;
            lbltestNumber.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_010;
            lbltestRepeatInterval.Text = Resources_Maintenance.STRING_MAINTENANCE_R1_096;
            lbltestRepeatIntervalUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_R1_097;
            lbltestPortNo.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_011;
            lbltestWashONOFF.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_087;
            rbttestWashON.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_088;
            rbttestWashOFF.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_089;

            gbxtestResponce.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_015;

            //Configurationタブ
            gbxconfParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_008;//Parameters

            lblconfWaitTimeAfterSuckingUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_019;//The Delay Time after R1 Aspiration
            lblconfWaitTimeAfterDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_020;//The Delay Time after R1 Dispense
            lblconfNoOfPrimeTimes.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_021;//The Number of Times of Prime
            lblconfNoOfRinseTimes.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_022;//The Number of Times of Rinse
            lblconfPrimeVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_023;//Prime Volume
            lblconfDispenseVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_024;//R1 Dispense Volume
            lblconfStandbyPosition.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_025;//R1Dispense Syringe Standby Position
            lblconfUsableElecCapaSensor.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_028;//Capacity Sensor
            rbCapacityON.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_030;//ON
            rbCapacityOFF.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_031;//OFF
            lblconfVomitVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_068;
            lblconfEjectorAirVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_069;
            lblconfProbeSeparationAir.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_070;
            lblconfAirLiftingAfterSuckingUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_071;
            lblconfLiquidSurfaceOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_072;
            lblconfNozzleWashTime.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_073;
            lblconfBubbleMixingValveONTime.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_074;
            lblconfBubbleMixingValveOFFTime.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_075;
            lblconfBubbleMixingValveFrequency.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_076;
            lblconfWashVomitVolume1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_077;
            lblconfWashVomitVolume2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_078;
            lblconfPrimeTime.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_091;
            lblconfDispenseMode.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_092;
            rbtconfDispenseModeEjector.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_094;
            rbtconfDispenseModeVomit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_095;
            lblconfWasteTime.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_093;

            //(sec)
            lblconfWaitTimeAfterSuckingUpUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_032;
            lblconfWaitTimeAfterDispenseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_032;
            lblconfNozzleWashTimeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_032;
            lblconfBubbleMixingValveONTimeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_032;
            lblconfBubbleMixingValveOFFTimeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_032;
            //(times)
            lblconfNoOfPrimeTimesUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_033;
            lblconfNoOfRinseTimesUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_033;
            lblconfBubbleMixingValveFrequencyUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_033;
            //(uL)
            lblconfPrimeVolumeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_034;
            lblconfDispenseVolumeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_034;
            lblconfStandbyPositionUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_034;
            lblconfVomitVolumeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_034;
            lblconfEjectorAirVolumeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_034;
            lblconfProbeSeparationAirUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_034;
            lblconfAirLiftingAfterSuckingUpUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_034;
            lblconfWashVomitVolume1Unit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_034;
            lblconfWashVomitVolume2Unit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_034;
            //(mm)
            lblconfLiquidSurfaceOffsetUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_079;

            //MotorParameterタブ
            gbxParamThetaAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_035;
            gbxParamThetaAxisCommon.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_053;
            gbxParamThetaAxisOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_054;
            lblParamThetaAxisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_036;
            lblParamThetaAxisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_037;
            lblParamThetaAxisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_038;
            lblParamThetaAxisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_039;
            lblParamThetaAxisOffsetR1Aspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_042;
            lblParamThetaAxisOffsetR2Aspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_043;
            lblParamThetaAxisOffsetMReagentAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_048;
            lblParamThetaAxisOffsetCuvette.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_041;
            lblParamThetaAxisOffsetReactionCellDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_040;
            lblParamThetaAxisOffsetEncodeThresh.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_098;
            lblParamThetaAxisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_044;
            lblParamThetaAxisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_044;
            lblParamThetaAxisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_045;
            lblParamThetaAxisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_044;
            lblParamThetaAxisOffsetR1AspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_046;
            lblParamThetaAxisOffsetR2AspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_046;
            lblParamThetaAxisOffsetMReagentAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_046;
            lblParamThetaAxisOffsetCuvetteUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_046;
            lblParamThetaAxisOffsetReactionCellDispenseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_046;
            lblParamThetaAxisOffsetEncodeThreshUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_046;

            gbxParamZAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_047;
            gbxParamZAxisCommon.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_053;
            gbxParamZAxisLiq.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_057;
            gbxParamZAxisBubble.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_058;
            gbxParamZAxisOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_054;
            lblParamZAxisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_036;
            lblParamZAxisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_037;
            lblParamZAxisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_038;
            lblParamZAxisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_039;
            lblParamZAxisLiqInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_036;
            lblParamZAxisLiqTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_037;
            lblParamZAxisLiqAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_038;
            lblParamZAxisLiqConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_039;
            lblParamZAxisBubbleInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_036;
            lblParamZAxisBubbleTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_037;
            lblParamZAxisBubbleAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_038;
            lblParamZAxisBubbleConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_039;
            lblParamZAxisOffsetR1R2Aspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_080;
            lblParamZAxisOffsetMReagentAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_048;
            lblParamZAxisOffsetCuvette.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_041;
            lblParamZAxisOffsetReactionCellDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_040;
            lblParamZAxisOffsetPositioningProbe.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_100;
            lblParamZAxisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_044;
            lblParamZAxisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_044;
            lblParamZAxisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_045;
            lblParamZAxisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_044;
            lblParamZAxisLiqInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_044;
            lblParamZAxisLiqTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_044;
            lblParamZAxisLiqAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_045;
            lblParamZAxisLiqConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_044;
            lblParamZAxisBubbleInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_044;
            lblParamZAxisBubbleTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_044;
            lblParamZAxisBubbleAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_045;
            lblParamZAxisBubbleConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_044;
            lblParamZAxisOffsetR1R2AspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_055;
            lblParamZAxisOffsetMReagentAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_055;
            lblParamZAxisOffsetCuvetteUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_055;
            lblParamZAxisOffsetReactionCellDispenseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_055;
            lblParamZAxisOffsetPositioningProbeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_055;

            gbxParamSyringe.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_056;
            gbxParamSyringeAsp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_059;
            gbxParamSyringeDis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_060;
            gbxParamSyringeAir.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_061;
            lblParamSyringeAspInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_036;
            lblParamSyringeAspTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_037;
            lblParamSyringeAspAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_038;
            lblParamSyringeAspConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_039;
            lblParamSyringeDisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_036;
            lblParamSyringeDisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_037;
            lblParamSyringeDisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_038;
            lblParamSyringeDisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_039;
            lblParamSyringeAirInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_036;
            lblParamSyringeAirTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_037;
            lblParamSyringeAirAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_038;
            lblParamSyringeAirConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_039;
            lblParamSyringeGain.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_062;
            lblParamSyringeOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_063;
            lblParamSyringeAspInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_044;
            lblParamSyringeAspTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_044;
            lblParamSyringeAspAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_045;
            lblParamSyringeAspConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_044;
            lblParamSyringeDisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_044;
            lblParamSyringeDisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_044;
            lblParamSyringeDisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_045;
            lblParamSyringeDisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_044;
            lblParamSyringeAirInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_044;
            lblParamSyringeAirTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_044;
            lblParamSyringeAirAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_045;
            lblParamSyringeAirConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_044;

            //MotorAdjustタブ
            gbxAdjustSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_000;
            SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_082, No=(int)MotorAdjustStopPosition.R1R1Aspiration},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_083, No=(int)MotorAdjustStopPosition.R1R2Aspiration},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_084, No=(int)MotorAdjustStopPosition.R1MAspiration},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_085, No=(int)MotorAdjustStopPosition.R1ReactionCell},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_086, No=(int)MotorAdjustStopPosition.R1Cuvette},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_101, No=(int)MotorAdjustStopPosition.R1PositioningProbe},
            };
            lbxAdjustSequence = ComFunc.setSequenceListBox(lbxAdjustSequence, SequenceList);

            gbxAdjustParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_008;
            lblAdjustPortNo.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_011;
            lblAdjustCoarseFine.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_COARSEFINE;

            Infragistics.Win.ValueListItem[] cmbValueCoarseFine = new Infragistics.Win.ValueListItem[]
            {
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_COARSE, dataValue:MotorAdjustCoarseFine.Coarse),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_FINE, dataValue:MotorAdjustCoarseFine.Fine),
            };
            cmbAdjustCoarseFine.Items.AddRange(cmbValueCoarseFine);

            gbxAdjustThetaAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_035;
            lblAdjustThetaAxisOffsetR1Aspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_042;
            lblAdjustThetaAxisOffsetR2Aspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_043;
            lblAdjustThetaAxisOffsetMReagentAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_048;
            lblAdjustThetaAxisOffsetCuvette.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_041;
            lblAdjustThetaAxisOffsetReactionCellDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_040;
            lblAdjustThetaAxisOffsetEncodeThresh.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_098;
            lblAdjustThetaAxisOffsetR1AspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_046;
            lblAdjustThetaAxisOffsetR2AspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_046;
            lblAdjustThetaAxisOffsetMReagentAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_046;
            lblAdjustThetaAxisOffsetCuvetteUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_046;
            lblAdjustThetaAxisOffsetReactionCellDispenseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_046;
            lblAdjustThetaAxisOffsetEncodeThreshUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_046;

            gbxAdjustZAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_047;
            lblAdjustZAxisOffsetR1R2Aspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_080;
            lblAdjustZAxisOffsetMReagentAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_048;
            lblAdjustZAxisOffsetCuvette.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_041;
            lblAdjustZAxisOffsetReactionCellDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_040;
            lblAdjustZAxisOffsetPositioningProbe.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_100;
            lblAdjustZAxisOffsetR1R2AspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_055;
            lblAdjustZAxisOffsetMReagentAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_055;
            lblAdjustZAxisOffsetCuvetteUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_055;
            lblAdjustZAxisOffsetReactionCellDispenseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_055;
            lblAdjustZAxisOffsetPositioningProbeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_055;

            gbxAdjustTable.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_028;
            lblAdjustTableOffsetMRead.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_060;
            lblAdjustTableOffsetRRead.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_061;
            lblAdjustTableOffsetR1UnitR1Asp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_062;
            lblAdjustTableOffsetR1UnitR2Asp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_063;
            lblAdjustTableOffsetR1UnitMAsp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_064;
            lblAdjustTableOffsetMRBottleCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_065;
            lblAdjustTableOffsetR2UnitMAsp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_066;
            lblAdjustTableOffsetR2UnitR2Asp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_067;
            lblAdjustTableOffsetEncodeThresh.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_074;
            lblAdjustTableOffsetMReadUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblAdjustTableOffsetRReadUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblAdjustTableOffsetR1UnitR1AspUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblAdjustTableOffsetR1UnitR2AspUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblAdjustTableOffsetR1UnitMAspUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblAdjustTableOffsetMRBottleCheckUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblAdjustTableOffsetR2UnitMAspUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblAdjustTableOffsetR2UnitR2AspUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblAdjustTableOffsetEncodeThreshUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;

            lblAdjustThetaAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;
            lblAdjustZAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;
            lblAdjustTablePitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;

            btnAdjustThetaAxisCCW.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_CCW;
            btnAdjustThetaAxisCW.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_CW;

            btnAdjustZAxisDown.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_DOWN;
            btnAdjustZAxisUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_UP;

            btnAdjustTableCCW.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_CCW;
            btnAdjustTableCW.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_CW;

        }
        #endregion

        /// <summary>
        /// ラジオボタンの選択値を設定
        /// </summary>
        private void setRadioButtonValue()
        {
            rbttestWashOFF.Tag = (int)R1DispenceRadioValue.WashOFF;
            rbttestWashON.Tag = (int)R1DispenceRadioValue.WashON;
        }

        /// <summary>
        /// ユニットをスタートします。
        /// </summary>
        public override void UnitStart()
        {
            //入力値チェック
            if (!CheckControls(tabUnit.Tabs[0].TabPage))
            {
                DlgMaintenance msg = new DlgMaintenance(Properties.Resources_Maintenance.DlgSeqChkMsg, false);
                msg.ShowDialog();
                DispEnable(true);
                return;
            }
            txttestResponce.Text = "";
            Singleton<CarisXSequenceHelperManager>.Instance.Maintenance.ExecuteSeqence(UnitStartPrc);
        }

        /// <summary>
        /// ユニットスタート処理をおこないます。
        /// </summary>
        private String UnitStartPrc(String str, object[] param)
        {
            switch (tabUnit.SelectedTab.Index)
            {
                case (int)MaintenanceTabIndex.Test:
                    Func<object> getfuncno = () => { return lbxtestSequenceListBox.SelectedValue; };
                    int FuncNo = (int)Invoke(getfuncno);

                    Func<string> getfuncname = () => { return lbxtestSequenceListBox.Text; };
                    string FuncName = (string)Invoke(getfuncname);

                    SlaveCommCommand_0439 StartComm = new SlaveCommCommand_0439();
                    StartComm.UnitNo = (int)UnitNoList.ReagentDispense1;
                    StartComm.FuncNo = FuncNo;

                    switch (FuncNo)
                    {
                        case (int)R1DispenceSequence.R1DispenseandProbeWashing:
                        case (int)R1DispenceSequence.R2DispenseandProbeWashing:
                        case (int)R1DispenceSequence.MDispenseandProbeWashing:
                            StartComm.Arg1 = (int)numtestPortNo.Value;
                            break;
                        case (int)R1DispenceSequence.DetectionSensor:
                            StartComm.Arg1 = (int)numtestPortNo.Value;
                            StartComm.Arg2 = ComFunc.getSelectedRadioButtonValue(gbxtestWashONOFF);
                            break;
                    }

                    // レスポンスがある機能の場合のみ、レスポンスのログファイルを作成
                    if (ComFunc.chkExistsResponse(MaintenanceMainNavi.ReagentDispense1Unit, FuncNo))
                    {
                        Singleton<ResponseLog>.Instance.CreateLog();
                        Singleton<ResponseLog>.Instance.SequenceNo = FuncNo;
                        Singleton<ResponseLog>.Instance.SequenceName = FuncName;
                    }

                    Func<int> getRepeatInterval = () => { return (int)numtestRepeatInterval.Value; };
                    int repeatInteerval = (int)Invoke(getRepeatInterval);

                    Func<int> repeatFrequency = () => { return (int)numtestRepeatFrequency.Value; };
                    for (int i = 0; i < (int)Invoke(repeatFrequency); i++)
                    {
                        // 繰り返しインターバルに設定されているミリ秒分待機（初回以外かつ中断フラグオフ）
                        if (i != 0 && !AbortFlg) System.Threading.Thread.Sleep(repeatInteerval);

                        // ポーズ処理
                        while (PauseFlg)
                        {
                            if (AbortFlg) break;
                        }

                        // 中断処理
                        if (AbortFlg)
                        {
                            AbortFlg = false;
                            break;
                        }
                        // 現在の実行回数を画面に反映
                        Action getNumber = () => { lbltestNumberDsp.Text = (i + 1).ToString(); }; Invoke(getNumber);

                        // 受信待ちフラグを設定
                        ResponseFlg = true;

                        //コマンド送信
                        unitStart.Start(StartComm);

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
                    
                    //メイン画面のナビゲータバーをEnableコマンドバーをデフォルトに戻す。
                    Action Disp = () => { DispEnable(true); }; Invoke(Disp);

                    break;

                case (int)MaintenanceTabIndex.MAdjust:
                    AdjustUnitStartPrc();
                    break;
            }
            return str;
        }

        /// <summary>
        /// 受信データ処理をおこないます。
        /// </summary>
        public override void SetResponse(CommCommandEventArgs comm)
        {
            switch (comm.Command.CommandId)
            {
                case (int)CommandKind.Command1439:
                    //ユニットテスト
                    string Result = comm.Command.CommandText.Remove(0, 20).Trim();

                    if (ComFunc.chkExistsResponse(MaintenanceMainNavi.ReagentDispense1Unit, (int)lbxtestSequenceListBox.SelectedValue))
                    {
                        Singleton<ResponseLog>.Instance.RepeatNo = int.Parse(lbltestNumberDsp.Text);
                        Singleton<ResponseLog>.Instance.WriteLog(Result);
                        Result = (txttestResponce.Text + Result + System.Environment.NewLine);     //画面の内容にレスポンスの内容を追記する形にする
                    }

                    txttestResponce.Text = Result;
                    txttestResponce.SelectionStart = txttestResponce.Text.Length;
                    txttestResponce.Focus();
                    txttestResponce.ScrollToCaret();

                    ResponseFlg = false;
                    break;
                case (int)CommandKind.Command1473:
                    //モーター微調整時
                    UpDownButtonEnable(true);
                    break;
                case (int)CommandKind.Command1480:
                    //モーター微調整開始
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

                            UnitTabEnable(true);

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
                case (int)CommandKind.Command1481:
                    //モーター微調整終了
                    UnitTabEnable(true);

                    this.Enabled = true;
                    break;
                case (int)CommandKind.Command1497:
                    // プローブ位置調整終了
                    ResponseFlg = false;

                    UnitTabEnable(true);

                    this.Enabled = true;

                    //プローブ交換調整
                    SlaveCommCommand_1497 respCommand = (SlaveCommCommand_1497)comm.Command;
                    if (respCommand.Result == 1 && respCommand.Offset != 0)
                    {
                        // OKの場合、オフセットを加算
                        numAdjustZAxisOffsetPositioningProbe.Value = (double)respCommand.Offset;
                        numAdjustZAxisOffsetPositioningProbe.Update();
                    }

                    // モーター調整ボタン非活性
                    UpDownButtonEnable(false);

                    this.ToolbarsControl((int)ToolBarEnable.Abort);

                    break;
            }
        }

        /// <summary>
        /// パラメータをすべて非活性にする
        /// </summary>
        private void ParametersAllFalse()
        {
            numtestPortNo.Enabled = false;
            gbxtestWashONOFF.Enabled = false;
        }

        /// <summary>
        /// 選択された機能番号に応じてパラメータを活性化する
        /// </summary>
        private void lbxtestSequenceListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParametersAllFalse();
            switch (lbxtestSequenceListBox.SelectedValue)
            {
                case (int)R1DispenceSequence.R1DispenseandProbeWashing:
                case (int)R1DispenceSequence.R2DispenseandProbeWashing:
                case (int)R1DispenceSequence.MDispenseandProbeWashing:
                    numtestPortNo.Enabled = true;
                    break;
                case (int)R1DispenceSequence.DetectionSensor:
                    numtestPortNo.Enabled = true;
                    gbxtestWashONOFF.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// タブが切り替えられたときにメンテナンスメイン画面のコマンドバーの表示切り替えをおこないます。
        /// </summary>
        private void TabControl_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            ToolbarsControl();
            SetUnitMode(e.Tab.Index);
        }

        /// <summary>
        /// メンテナンスメイン画面のコマンドバーの表示切り替えをおこないます。
        /// </summary>
        public override void ToolbarsControl(int toolBarEnablekind = -1)
        {
            if (tabUnit != null && tabUnit.SelectedTab != null)
            {
                //デリゲートでCall
                ToolbarsDisp(tabUnit.SelectedTab.Index, toolBarEnablekind);
            }
        }

        /// <summary>
        /// Maintenanc画面のEnable/Disableを切り替えます。
        /// </summary>
        private void DispEnable(bool enable)
        {
            this.Enabled = enable;
            MainToolbarsDispEnable(enable);
        }


        /// <summary>
        /// モーターパラメータ保存
        /// </summary>
        private void MotorParamSave(bool adjustSave)
        {
            //入力値チェック
            if (!CheckControls(tabUnit.Tabs[2].TabPage))
            {
                DlgMaintenance msg = new DlgMaintenance(Properties.Resources_Maintenance.DlgChkMsg, false);
                msg.ShowDialog();
                return;
            }

            SlaveCommCommand_0471 Mparam;

            ParameterFilePreserve<CarisXMotorParameter> motor = Singleton<ParameterFilePreserve<CarisXMotorParameter>>.Instance;
            motorLoad(motor);

            //試薬分注1部θ軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.R1DispenseArmThetaAxis;
            if (!adjustSave)
            {
                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamThetaAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamThetaAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamThetaAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamThetaAxisConst.Value;

                Mparam.MotorOffset[0] = (double)numParamThetaAxisOffsetR1Aspiration.Value;
                Mparam.MotorOffset[1] = (double)numParamThetaAxisOffsetR2Aspiration.Value;
                Mparam.MotorOffset[2] = (double)numParamThetaAxisOffsetMReagentAspiration.Value;
                Mparam.MotorOffset[3] = (double)numParamThetaAxisOffsetCuvette.Value;
                Mparam.MotorOffset[4] = (double)numParamThetaAxisOffsetReactionCellDispense.Value;
                Mparam.MotorOffset[5] = (double)numParamThetaAxisOffsetEncodeThresh.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.MotorSpeed;

                Mparam.MotorOffset[0] = (double)numAdjustThetaAxisOffsetR1Aspiration.Value;
                Mparam.MotorOffset[1] = (double)numAdjustThetaAxisOffsetR2Aspiration.Value;
                Mparam.MotorOffset[2] = (double)numAdjustThetaAxisOffsetMReagentAspiration.Value;
                Mparam.MotorOffset[3] = (double)numAdjustThetaAxisOffsetCuvette.Value;
                Mparam.MotorOffset[4] = (double)numAdjustThetaAxisOffsetReactionCellDispense.Value;
                Mparam.MotorOffset[5] = (double)numAdjustThetaAxisOffsetEncodeThresh.Value;
            }
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.OffsetR1Aspiration = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.OffsetR2Aspiration = Mparam.MotorOffset[1];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.OffsetMReagentAspiration = Mparam.MotorOffset[2];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.OffsetCuvette = Mparam.MotorOffset[3];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.OffsetReactionCellDispense = Mparam.MotorOffset[4];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.OffsetEncodeThresh = Mparam.MotorOffset[5];

            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);


            //試薬分注1部Z軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.R1DispenseArmZAxis;
            if (!adjustSave)
            {
                //モーターパラメータの保存
                //通常
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamZAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamZAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamZAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamZAxisConst.Value;
                //液面検知(ｿﾌﾄ固定）
                Mparam.MotorSpeed[1].InitSpeed = (int)numParamZAxisLiqInit.Value;
                Mparam.MotorSpeed[1].TopSpeed = (int)numParamZAxisLiqTop.Value;
                Mparam.MotorSpeed[1].Accel = (int)numParamZAxisLiqAccelerator.Value;
                Mparam.MotorSpeed[1].ConstSpeed = (int)numParamZAxisLiqConst.Value;
                //泡検知動作(ｿﾌﾄ固定）
                Mparam.MotorSpeed[2].InitSpeed = (int)numParamZAxisBubbleInit.Value;
                Mparam.MotorSpeed[2].TopSpeed = (int)numParamZAxisBubbleTop.Value;
                Mparam.MotorSpeed[2].Accel = (int)numParamZAxisBubbleAccelerator.Value;
                Mparam.MotorSpeed[2].ConstSpeed = (int)numParamZAxisBubbleConst.Value;

                Mparam.MotorOffset[0] = (double)numParamZAxisOffsetR1R2Aspiration.Value;
                Mparam.MotorOffset[1] = (double)numParamZAxisOffsetMReagentAspiration.Value;
                Mparam.MotorOffset[2] = (double)numParamZAxisOffsetCuvette.Value;
                Mparam.MotorOffset[3] = (double)numParamZAxisOffsetReactionCellDispense.Value;
                Mparam.MotorOffset[4] = (double)numParamZAxisOffsetPositioningProbe.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.MotorSpeed;

                Mparam.MotorOffset[0] = (double)numAdjustZAxisOffsetR1R2Aspiration.Value;
                Mparam.MotorOffset[1] = (double)numAdjustZAxisOffsetMReagentAspiration.Value;
                Mparam.MotorOffset[2] = (double)numAdjustZAxisOffsetCuvette.Value;
                Mparam.MotorOffset[3] = (double)numAdjustZAxisOffsetReactionCellDispense.Value;
                Mparam.MotorOffset[4] = (double)numAdjustZAxisOffsetPositioningProbe.Value;
            }
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.OffsetR1R2Aspiration = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.OffsetMReagentAspiration = Mparam.MotorOffset[1];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.OffsetCuvette = Mparam.MotorOffset[2];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.OffsetReactionCellDispense = Mparam.MotorOffset[3];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.OffsetPositioningProbe = Mparam.MotorOffset[4];

            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);


            if (!adjustSave)
            {
                //試薬分注1ｼﾘﾝｼﾞ
                Mparam = new SlaveCommCommand_0471();
                Mparam.MotorNo = (int)MotorNoList.R1DispenseSyringe;
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamSyringeAspInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamSyringeAspTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamSyringeAspAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamSyringeAspConst.Value;

                Mparam.MotorSpeed[1].InitSpeed = (int)numParamSyringeDisInit.Value;
                Mparam.MotorSpeed[1].TopSpeed = (int)numParamSyringeDisTop.Value;
                Mparam.MotorSpeed[1].Accel = (int)numParamSyringeDisAccelerator.Value;
                Mparam.MotorSpeed[1].ConstSpeed = (int)numParamSyringeDisConst.Value;

                Mparam.MotorSpeed[2].InitSpeed = (int)numParamSyringeAirInit.Value;
                Mparam.MotorSpeed[2].TopSpeed = (int)numParamSyringeAirTop.Value;
                Mparam.MotorSpeed[2].Accel = (int)numParamSyringeAirAccelerator.Value;
                Mparam.MotorSpeed[2].ConstSpeed = (int)numParamSyringeAirConst.Value;

                Mparam.MotorOffset[0] = (double)numParamSyringeGain.Value;
                Mparam.MotorOffset[1] = (double)numParamSyringeOffset.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseSyringeParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseSyringeParam.MotorSpeed = Mparam.MotorSpeed;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseSyringeParam.Gain = Mparam.MotorOffset[0];
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseSyringeParam.Offset = Mparam.MotorOffset[1];

                //モータパラメータ送信内容スプール
                SpoolParam(Mparam);
            }

            if (adjustSave)
            {
                //試薬保冷庫テーブルθ軸
                Mparam = new SlaveCommCommand_0471();
                Mparam.MotorNo = (int)MotorNoList.ReagentStorageTableThetaAxis;

                //送信用アイテムにセット
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustTableOffsetMRead.Value;
                Mparam.MotorOffset[1] = (double)numAdjustTableOffsetRRead.Value;
                Mparam.MotorOffset[2] = (double)numAdjustTableOffsetR1UnitR1Asp.Value;
                Mparam.MotorOffset[3] = (double)numAdjustTableOffsetR1UnitR2Asp.Value;
                Mparam.MotorOffset[4] = (double)numAdjustTableOffsetR1UnitMAsp.Value;
                Mparam.MotorOffset[5] = (double)numAdjustTableOffsetMRBottleCheck.Value;
                Mparam.MotorOffset[6] = (double)numAdjustTableOffsetR2UnitMAsp.Value;
                Mparam.MotorOffset[7] = (double)numAdjustTableOffsetR2UnitR2Asp.Value;
                Mparam.MotorOffset[8] = (double)numAdjustTableOffsetEncodeThresh.Value;

                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetMReagentIDReading = Mparam.MotorOffset[0];
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetRReagentIDReading = Mparam.MotorOffset[1];
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR1UnitR1Aspiration = Mparam.MotorOffset[2];
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR1UnitR2Aspiration = Mparam.MotorOffset[3];
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR1UnitMReagentAspiration = Mparam.MotorOffset[4];
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetMRBottleCheck = Mparam.MotorOffset[5];
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR2UnitMReagentAspiration = Mparam.MotorOffset[6];
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR2UnitR2Aspiration = Mparam.MotorOffset[7];
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetEncodeThresh = Mparam.MotorOffset[8];

                //モータパラメータ送信内容スプール
                SpoolParam(Mparam);
            }

            //モータパラメータ送信
            SendParam();

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
                        ParameterFilePreserve<CarisXConfigParameter> config = Singleton<ParameterFilePreserve<CarisXConfigParameter>>.Instance;
                        configSave(config);
                        break;
                    case (int)MaintenanceTabIndex.MParam:
                    case (int)MaintenanceTabIndex.MAdjust:
                        //モータパラメータ保存
                        ParameterFilePreserve<CarisXMotorParameter> motor = Singleton<ParameterFilePreserve<CarisXMotorParameter>>.Instance;
                        motorSave(motor);
                        MotorParamDisp();
                        if (tabUnit.SelectedTab.Index == (int)MaintenanceTabIndex.MAdjust)
                        {
                            try
                            {
                                //モーター調整タブの時は、関連する他の画面のモーターパラメータタブの再表示を行う
                                FormMaintenanceMain formMaintenanceMain = (FormMaintenanceMain)Application.OpenForms[typeof(FormMaintenanceMain).Name];
                                formMaintenanceMain.subFormReagentStorage[formMaintenanceMain.moduleIndex].MotorParamDisp();
                                formMaintenanceMain.subFormR2Dispense[formMaintenanceMain.moduleIndex].MotorParamDisp();
                            }
                            catch (Exception)
                            {
                                System.Diagnostics.Debug.WriteLine("FormMaintenanceMain instance does not exist");
                                Singleton<Log.CarisXLogManager>.Instance.Write(Oelco.Common.Log.LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                                    Log.CarisXLogInfoBaseExtention.Empty, "FormMaintenanceMain instance does not exist");
                            }
                        }
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

            //メイン画面のナビゲータバーをEnableコマンドバーをデフォルトに戻す。
            switch (tabUnit.SelectedTab.Index)
            {
                case (int)MaintenanceTabIndex.Config:
                case (int)MaintenanceTabIndex.MParam:
                    Action Disp = () => { DispEnable(true); }; Invoke(Disp);
                    ToolbarsControl();
                    break;

                case (int)MaintenanceTabIndex.MAdjust:
                    this.Enabled = true;
                    if ((int)lbxAdjustSequence.SelectedValue == (int)MotorAdjustStopPosition.R1PositioningProbe)
                    {
                        this.ToolbarsControl((int)ToolBarEnable.AdjustReplacingProbe);
                    }
                    else
                    {
                        this.ToolbarsControl((int)ToolBarEnable.Adjust);
                    }
                    break;
            }

        }

        /// <summary>
        /// モーターパラメータ画面表示
        /// </summary>
        public override void MotorParamDisp()
        {
            ParameterFilePreserve<CarisXMotorParameter> motor = Singleton<ParameterFilePreserve<CarisXMotorParameter>>.Instance;
            motorLoad(motor);

            //試薬分注1部θ軸
            numParamThetaAxisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.MotorSpeed[0].InitSpeed;
            numParamThetaAxisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.MotorSpeed[0].TopSpeed;
            numParamThetaAxisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.MotorSpeed[0].Accel;
            numParamThetaAxisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.MotorSpeed[0].ConstSpeed;
            //オフセット
            numParamThetaAxisOffsetR1Aspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.OffsetR1Aspiration;
            numParamThetaAxisOffsetR2Aspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.OffsetR2Aspiration;
            numParamThetaAxisOffsetMReagentAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.OffsetMReagentAspiration;
            numParamThetaAxisOffsetCuvette.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.OffsetCuvette;
            numParamThetaAxisOffsetReactionCellDispense.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.OffsetReactionCellDispense;
            numParamThetaAxisOffsetEncodeThresh.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.OffsetEncodeThresh;
            //オフセット（モーター調整用）
            numAdjustThetaAxisOffsetR1Aspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.OffsetR1Aspiration;
            numAdjustThetaAxisOffsetR2Aspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.OffsetR2Aspiration;
            numAdjustThetaAxisOffsetMReagentAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.OffsetMReagentAspiration;
            numAdjustThetaAxisOffsetCuvette.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.OffsetCuvette;
            numAdjustThetaAxisOffsetReactionCellDispense.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.OffsetReactionCellDispense;
            numAdjustThetaAxisOffsetEncodeThresh.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmThetaAxisParam.OffsetEncodeThresh;

            //試薬分注1部Z軸
            //通常
            numParamZAxisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.MotorSpeed[0].InitSpeed;
            numParamZAxisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.MotorSpeed[0].TopSpeed;
            numParamZAxisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.MotorSpeed[0].Accel;
            numParamZAxisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.MotorSpeed[0].ConstSpeed;
            //液面検知(ｿﾌﾄ固定）
            numParamZAxisLiqInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.MotorSpeed[1].InitSpeed;
            numParamZAxisLiqTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.MotorSpeed[1].TopSpeed;
            numParamZAxisLiqAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.MotorSpeed[1].Accel;
            numParamZAxisLiqConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.MotorSpeed[1].ConstSpeed;
            //泡検知動作(ｿﾌﾄ固定）
            numParamZAxisBubbleInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.MotorSpeed[2].InitSpeed;
            numParamZAxisBubbleTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.MotorSpeed[2].TopSpeed;
            numParamZAxisBubbleAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.MotorSpeed[2].Accel;
            numParamZAxisBubbleConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.MotorSpeed[2].ConstSpeed;
            //オフセット
            numParamZAxisOffsetR1R2Aspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.OffsetR1R2Aspiration;
            numParamZAxisOffsetMReagentAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.OffsetMReagentAspiration;
            numParamZAxisOffsetReactionCellDispense.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.OffsetReactionCellDispense;
            numParamZAxisOffsetCuvette.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.OffsetCuvette;
            numParamZAxisOffsetPositioningProbe.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.OffsetPositioningProbe;
            //オフセット（モーター調整用）
            numAdjustZAxisOffsetR1R2Aspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.OffsetR1R2Aspiration;
            numAdjustZAxisOffsetMReagentAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.OffsetMReagentAspiration;
            numAdjustZAxisOffsetReactionCellDispense.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.OffsetReactionCellDispense;
            numAdjustZAxisOffsetCuvette.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.OffsetCuvette;
            numAdjustZAxisOffsetPositioningProbe.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseArmZAxisParam.OffsetPositioningProbe;

            //R1分注シリンジ
            numParamSyringeAspInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseSyringeParam.MotorSpeed[0].InitSpeed;
            numParamSyringeAspTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseSyringeParam.MotorSpeed[0].TopSpeed;
            numParamSyringeAspAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseSyringeParam.MotorSpeed[0].Accel;
            numParamSyringeAspConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseSyringeParam.MotorSpeed[0].ConstSpeed;

            numParamSyringeDisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseSyringeParam.MotorSpeed[1].InitSpeed;
            numParamSyringeDisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseSyringeParam.MotorSpeed[1].TopSpeed;
            numParamSyringeDisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseSyringeParam.MotorSpeed[1].Accel;
            numParamSyringeDisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseSyringeParam.MotorSpeed[1].ConstSpeed;

            numParamSyringeAirInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseSyringeParam.MotorSpeed[2].InitSpeed;
            numParamSyringeAirTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseSyringeParam.MotorSpeed[2].TopSpeed;
            numParamSyringeAirAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseSyringeParam.MotorSpeed[2].Accel;
            numParamSyringeAirConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseSyringeParam.MotorSpeed[2].ConstSpeed;

            numParamSyringeGain.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseSyringeParam.Gain;
            numParamSyringeOffset.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseSyringeParam.Offset;

            //試薬保冷庫テーブルθ軸
            numAdjustTableOffsetMRead.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetMReagentIDReading;
            numAdjustTableOffsetRRead.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetRReagentIDReading;
            numAdjustTableOffsetR1UnitR1Asp.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR1UnitR1Aspiration;
            numAdjustTableOffsetR1UnitR2Asp.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR1UnitR2Aspiration;
            numAdjustTableOffsetR1UnitMAsp.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR1UnitMReagentAspiration;
            numAdjustTableOffsetMRBottleCheck.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetMRBottleCheck;
            numAdjustTableOffsetR2UnitMAsp.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR2UnitMReagentAspiration;
            numAdjustTableOffsetR2UnitR2Asp.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR2UnitR2Aspiration;
            numAdjustTableOffsetEncodeThresh.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetEncodeThresh;

        }

        /// <summary>
        /// コンフィグパラメータ保存
        /// </summary>
        public void ConfigParamSave()
        {
            //入力値チェック
            if (!CheckControls(tabUnit.Tabs[1].TabPage))
            {
                DlgMaintenance msg = new DlgMaintenance(Properties.Resources_Maintenance.DlgChkMsg, false);
                msg.ShowDialog();
                return;
            }

            ParameterFilePreserve<CarisXConfigParameter> config = Singleton<ParameterFilePreserve<CarisXConfigParameter>>.Instance;
            configLoad(config);

            CarisXConfigParameter.R1DispenseUnitConfigParam ConfigParam = new CarisXConfigParameter.R1DispenseUnitConfigParam();
            //試薬吸引後遅延時間
            ConfigParam.WaitTimeAfterSuckingUp = (double)numconfWaitTimeAfterSuckingUp.Value;
            //試薬吐出後遅延時間
            ConfigParam.WaitTimeAfterDispense = (double)numconfWaitTimeAfterDispense.Value;
            //プライム回数
            ConfigParam.NoOfPrimeTimes = (int)numconfNoOfPrimeTimes.Value;
            //リンス回数
            ConfigParam.NoOfRinseTimes = (int)numconfNoOfRinseTimes.Value;
            //プライム量
            ConfigParam.PrimeVolume = (int)numconfPrimeVolume.Value;
            //試薬分注量
            ConfigParam.DispenseVolume = (int)numconfDispenseVolume.Value;
            //試薬シリンジ待機位置
            ConfigParam.StandbyPosition = (int)numconfStandbyPosition.Value;

            //静電容量センサー使用有無
            if (rbCapacityON.Checked)
                ConfigParam.UsableElecCapaSensor = 1;
            else
                ConfigParam.UsableElecCapaSensor = 0;

            //試薬吐き残し量
            ConfigParam.VomitVolume = (int)numconfVomitVolume.Value;
            //試薬追い出しエアー量
            ConfigParam.EjectorAirVolume = (int)numconfEjectorAirVolume.Value;
            //試薬プローブ分離エアー量
            ConfigParam.ProbeSeparationAir = (int)numconfProbeSeparationAir.Value;
            //試薬吸引後エアー引き上げ量
            ConfigParam.AirLiftingAfterSuckingUp = (int)numconfAirLiftingAfterSuckingUp.Value;
            //試薬液面オフセット
            ConfigParam.LiquidSurfaceOffset = (double)numconfLiquidSurfaceOffset.Value;
            //ノズル洗浄時間
            ConfigParam.NozzleWashTime = (double)numconfNozzleWashTime.Value;
            //気泡混入バルブON時間
            ConfigParam.BubbleMixingValveONTime = (double)numconfBubbleMixingValveONTime.Value;
            //気泡混入バルブOFF時間
            ConfigParam.BubbleMixingValveOFFTime = (double)numconfBubbleMixingValveOFFTime.Value;
            //気泡混入バルブ動作回数
            ConfigParam.BubbleMixingValveFrequency = (int)numconfBubbleMixingValveFrequency.Value;
            //洗浄後洗浄液吐出量1
            ConfigParam.WashVomitVolume1 = (int)numconfWashVomitVolume1.Value;
            //洗浄後洗浄液吐出量2
            ConfigParam.WashVomitVolume2 = (int)numconfWashVomitVolume2.Value;
            //プライム時間
            ConfigParam.PrimeTime = (double)numconfPrimeTime.Value;

            //吐出方法
            if (rbtconfDispenseModeVomit.Checked)
                ConfigParam.DispenseMode = 1;
            else
                ConfigParam.DispenseMode = 0;

            //廃液時間
            ConfigParam.WasteTime = (double)numconfWasteTime.Value;

            config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam = ConfigParam;

            //パラメータ送信
            SendParam(ConfigParam);
        }

        /// <summary>
        /// コンフィグパラメータ読み込み
        /// </summary>
        public override void ConfigParamLoad()
        {
            ParameterFilePreserve<CarisXConfigParameter> config = Singleton<ParameterFilePreserve<CarisXConfigParameter>>.Instance;
            configLoad(config);

            //試薬吸引後遅延時間
            numconfWaitTimeAfterSuckingUp.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.WaitTimeAfterSuckingUp;
            //試薬吐出後遅延時間
            numconfWaitTimeAfterDispense.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.WaitTimeAfterDispense;
            //プライム回数
            numconfNoOfPrimeTimes.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.NoOfPrimeTimes;
            //リンス回数
            numconfNoOfRinseTimes.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.NoOfRinseTimes;
            //プライム量
            numconfPrimeVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.PrimeVolume;
            //試薬分注量
            numconfDispenseVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.DispenseVolume;
            //試薬シリンジ待機位置
            numconfStandbyPosition.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.StandbyPosition;

            //静電容量センサー使用有無
            if (config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.UsableElecCapaSensor == 1)
                rbCapacityON.Checked = true;
            else
                rbCapacityOFF.Checked = true;

            //試薬吐き残し量
            numconfVomitVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.VomitVolume;
            //試薬追い出しエアー量
            numconfEjectorAirVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.EjectorAirVolume;
            //試薬プローブ分離エアー量
            numconfProbeSeparationAir.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.ProbeSeparationAir;
            //試薬吸引後エアー引き上げ量
            numconfAirLiftingAfterSuckingUp.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.AirLiftingAfterSuckingUp;
            //試薬液面オフセット
            numconfLiquidSurfaceOffset.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.LiquidSurfaceOffset;
            //ノズル洗浄時間
            numconfNozzleWashTime.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.NozzleWashTime;
            //気泡混入バルブON時間
            numconfBubbleMixingValveONTime.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.BubbleMixingValveONTime;
            //気泡混入バルブOFF時間
            numconfBubbleMixingValveOFFTime.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.BubbleMixingValveOFFTime;
            //気泡混入バルブ動作回数
            numconfBubbleMixingValveFrequency.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.BubbleMixingValveFrequency;
            //洗浄後洗浄液吐出量1
            numconfWashVomitVolume1.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.WashVomitVolume1;
            //洗浄後洗浄液吐出量2
            numconfWashVomitVolume2.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.WashVomitVolume2;
            //プライム時間
            numconfPrimeTime.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.PrimeTime;

            //吐出方法
            if (config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.DispenseMode == 1)
                rbtconfDispenseModeVomit.Checked = true;
            else
                rbtconfDispenseModeEjector.Checked = true;

            //廃液時間
            numconfWasteTime.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1DispenseUnitConfigParam.WasteTime;

        }

        /// <summary>
        /// パラメータ保存
        /// </summary>
        public override void ParamSave()
        {
            switch (tabUnit.SelectedTab.Index)
            {
                case (int)MaintenanceTabIndex.Config:
                    //コンフィグパラメータセーブ
                    ConfigParamSave();
                    break;
                case (int)MaintenanceTabIndex.MParam:
                    //モーターパラメータセーブ
                    MotorParamSave(false);
                    break;
                case (int)MaintenanceTabIndex.MAdjust:
                    //モータ調整パラメータセーブ
                    MotorParamSave(true);
                    break;
            }
        }

        /// <summary>
        /// Adjustグループ全Disable
        /// </summary>
        private void adjustGbAllDisable()
        {
            numAdjustPortNo.Enabled = false;
            cmbAdjustCoarseFine.Enabled = false;

            gbxAdjustThetaAxis.Enabled = false;
            numAdjustThetaAxisOffsetR1Aspiration.Enabled = false;
            numAdjustThetaAxisOffsetR2Aspiration.Enabled = false;
            numAdjustThetaAxisOffsetMReagentAspiration.Enabled = false;
            numAdjustThetaAxisOffsetCuvette.Enabled = false;
            numAdjustThetaAxisOffsetReactionCellDispense.Enabled = false;
            numAdjustThetaAxisOffsetEncodeThresh.Enabled = false;

            gbxAdjustZAxis.Enabled = false;
            numAdjustZAxisOffsetR1R2Aspiration.Enabled = false;
            numAdjustZAxisOffsetMReagentAspiration.Enabled = false;
            numAdjustZAxisOffsetCuvette.Enabled = false;
            numAdjustZAxisOffsetReactionCellDispense.Enabled = false;
            numAdjustZAxisOffsetPositioningProbe.Enabled = false;

            gbxAdjustTable.Enabled = false;
            numAdjustTableOffsetMRead.Enabled = false;
            numAdjustTableOffsetRRead.Enabled = false;
            numAdjustTableOffsetR1UnitR1Asp.Enabled = false;
            numAdjustTableOffsetR1UnitR2Asp.Enabled = false;
            numAdjustTableOffsetR1UnitMAsp.Enabled = false;
            numAdjustTableOffsetMRBottleCheck.Enabled = false;
            numAdjustTableOffsetR2UnitMAsp.Enabled = false;
            numAdjustTableOffsetR2UnitR2Asp.Enabled = false;
            numAdjustTableOffsetEncodeThresh.Enabled = false;

            //EditBoxの文字色をすべて黒にする。
            EditBoxControlsBlack(tabUnit.Tabs[3].TabPage);
        }

        /// <summary>
        /// モーター調整機能スタート時処理
        /// </summary>
        private void AdjustUnitStartPrc()
        {
            //モータ調整ボタンEnable
            Action DispUpDownButton = () =>
            {
                UpDownButtonEnable(true);
                gbxAdjustParam.Enabled = false;
                lbxAdjustSequence.Enabled = false;
                tabUnit.Tabs[0].Enabled = false;
                tabUnit.Tabs[1].Enabled = false;
                tabUnit.Tabs[2].Enabled = false;
            }; Invoke(DispUpDownButton);

            Func<int> getStopPos = () => { return (int)lbxAdjustSequence.SelectedValue; };
            Func<int> portno = () => { return (int)numAdjustPortNo.Value; };
            Func<int> coarsefine = () => { return (int)cmbAdjustCoarseFine.Value; };

            int stopPos = (int)Invoke(getStopPos);

            // 受信待ちフラグを設定
            ResponseFlg = true;

            if (stopPos != (int)MotorAdjustStopPosition.R1PositioningProbe)
            {
                // 試薬プローブ位置調整以外 =>

                // 調整位置停止コマンド
                SlaveCommCommand_0480 AdjustStartComm = new SlaveCommCommand_0480();
                AdjustStartComm.Pos = stopPos;

                switch (AdjustStartComm.Pos)
                {
                    case (int)MotorAdjustStopPosition.R1R1Aspiration:
                    case (int)MotorAdjustStopPosition.R1R2Aspiration:
                    case (int)MotorAdjustStopPosition.R1MAspiration:
                        AdjustStartComm.Arg1 = (int)Invoke(portno);
                        AdjustStartComm.Arg2 = (int)Invoke(coarsefine);
                        break;
                }

                // コマンド送信
                unitStartAdjust.Start(AdjustStartComm);
            }
            else
            {
                // 試薬プローブ位置調整

                // プローブ交換コマンド
                SlaveCommCommand_0497 PositioningProbeComm = new SlaveCommCommand_0497();
                PositioningProbeComm.Status = SlaveCommCommand_0497.probeUnit.R1Unit;

                // コマンド送信
                unitStartAdjust.Start(PositioningProbeComm);
            }

            // 応答コマンド受信待ち
            while (ResponseFlg)
            {
                if (AbortFlg)
                {
                    // 中断要求により受信待ち解除
                    break;
                }
            }

            // メイン画面のナビゲータバーをEnableコマンドバーをデフォルトに戻す
            Action Disp = () => { this.Enabled = true; }; Invoke(Disp);

            AbortFlg = false;
        }

        /// <summary>
        /// モーター移動終了時処理
        /// </summary>
        public override bool UnitAbortexe()
        {
            switch (tabUnit.SelectedTab.Index)
            {
                case (int)MaintenanceTabIndex.Test:
                    UnitAbort();
                    break;
                case (int)MaintenanceTabIndex.MAdjust:
                    if (DialogResult.OK != GUI.DlgMessage.Show(Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_SAVEMASSAGE
                        , String.Empty, Properties.Resources.STRING_DLG_TITLE_001, GUI.MessageDialogButtons.OKCancel))
                    {
                        return (false);
                    }
                    UpDownButtonEnable(false);
                    MotorParamDisp();

                    // プローブ位置調整の場合、開始状態に戻す
                    if ((int)lbxAdjustSequence.SelectedValue == (int)MotorAdjustStopPosition.R1PositioningProbe)
                    {
                        // プローブ位置調整終了
                        ResponseFlg = false;

                        UnitTabEnable(true);
                        this.Enabled = true;
                        this.ToolbarsControl((int)ToolBarEnable.AdjustReplacingProbe);
                    }
                    else
                    {
                        SlaveCommCommand_0481 AdjustStartComm = new SlaveCommCommand_0481();
                        Func<int> getStopPos = () => { return (int)lbxAdjustSequence.SelectedValue; };
                        AdjustStartComm.Pos = (int)Invoke(getStopPos);

                        unitAdjustAbort.Start(AdjustStartComm);
                    }
                    break;
            }

            return (true);
        }

        /// <summary>
        /// UP/DownボタンをEnable/Disableにする
        /// </summary>
        public override void UpDownButtonEnable(bool enable)
        {
            btnAdjustThetaAxisCCW.Enabled = enable;
            btnAdjustThetaAxisCW.Enabled = enable;

            btnAdjustZAxisDown.Enabled = enable;
            btnAdjustZAxisUp.Enabled = enable;

            btnAdjustTableCCW.Enabled = enable;
            btnAdjustTableCW.Enabled = enable;
        }

        /// <summary>
        /// TabとAdjustタブの一部コントロールをEnable/Disableにする
        /// </summary>
        public void UnitTabEnable(bool enable)
        {
            gbxAdjustParam.Enabled = enable;
            lbxAdjustSequence.Enabled = enable;
            tabUnit.Tabs[(int)MaintenanceTabIndex.Test].Enabled = enable;
            tabUnit.Tabs[(int)MaintenanceTabIndex.Config].Enabled = enable && ConfigTabUseFlg;
            tabUnit.Tabs[(int)MaintenanceTabIndex.MParam].Enabled = enable;
        }

        /// <summary>
        /// モータ調整シーケンスリスト変更時の処理
        /// </summary>
        private void lbxAdjustSequence_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            adjustGbAllDisable();
            //モータ調整ボタンDisable
            UpDownButtonEnable(false);

            this.ToolbarsControl((int)ToolBarEnable.Default);

            switch (lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.R1R1Aspiration:
                    numAdjustPortNo.Enabled = true;
                    cmbAdjustCoarseFine.Enabled = true;

                    gbxAdjustThetaAxis.Enabled = true;
                    numAdjustThetaAxisOffsetR1Aspiration.Enabled = true;
                    numAdjustThetaAxisOffsetR1Aspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetR1R2Aspiration.Enabled = true;
                    numAdjustZAxisOffsetR1R2Aspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustTable.Enabled = true;
                    numAdjustTableOffsetR1UnitR1Asp.Enabled = true;
                    numAdjustTableOffsetR1UnitR1Asp.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

                case (int)MotorAdjustStopPosition.R1R2Aspiration:
                    numAdjustPortNo.Enabled = true;
                    cmbAdjustCoarseFine.Enabled = true;

                    gbxAdjustThetaAxis.Enabled = true;
                    numAdjustThetaAxisOffsetR2Aspiration.Enabled = true;
                    numAdjustThetaAxisOffsetR2Aspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetR1R2Aspiration.Enabled = true;
                    numAdjustZAxisOffsetR1R2Aspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustTable.Enabled = true;
                    numAdjustTableOffsetR1UnitR2Asp.Enabled = true;
                    numAdjustTableOffsetR1UnitR2Asp.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

                case (int)MotorAdjustStopPosition.R1MAspiration:
                    numAdjustPortNo.Enabled = true;
                    cmbAdjustCoarseFine.Enabled = true;

                    gbxAdjustThetaAxis.Enabled = true;
                    numAdjustThetaAxisOffsetMReagentAspiration.Enabled = true;
                    numAdjustThetaAxisOffsetMReagentAspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetMReagentAspiration.Enabled = true;
                    numAdjustZAxisOffsetMReagentAspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustTable.Enabled = true;
                    numAdjustTableOffsetR1UnitMAsp.Enabled = true;
                    numAdjustTableOffsetR1UnitMAsp.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

                case (int)MotorAdjustStopPosition.R1ReactionCell:
                    gbxAdjustThetaAxis.Enabled = true;
                    numAdjustThetaAxisOffsetReactionCellDispense.Enabled = true;
                    numAdjustThetaAxisOffsetReactionCellDispense.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetReactionCellDispense.Enabled = true;
                    numAdjustZAxisOffsetReactionCellDispense.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

                case (int)MotorAdjustStopPosition.R1Cuvette:
                    gbxAdjustThetaAxis.Enabled = true;
                    numAdjustThetaAxisOffsetCuvette.Enabled = true;
                    numAdjustThetaAxisOffsetCuvette.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetCuvette.Enabled = true;
                    numAdjustZAxisOffsetCuvette.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

                case (int)MotorAdjustStopPosition.R1PositioningProbe:
                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetPositioningProbe.Enabled = true;
                    numAdjustZAxisOffsetPositioningProbe.ForeColor = System.Drawing.Color.OrangeRed;
                   
                    this.ToolbarsControl((int)ToolBarEnable.AdjustReplacingProbe);
                    break;
            }
        }

        #region Pitch値の調整
        private void btnAdjustThetaAxisPitchUp_Click(object sender, EventArgs e)
        {
            numAdjustThetaAxisPitch.Value = (double)numAdjustThetaAxisPitch.Value + (double)numAdjustThetaAxisPitch.SpinIncrement;
        }

        private void btnAdjustThetaAxisPitchDown_Click(object sender, EventArgs e)
        {
            numAdjustThetaAxisPitch.Value = (double)numAdjustThetaAxisPitch.Value - (double)numAdjustThetaAxisPitch.SpinIncrement;
        }

        private void btnAdjustZAxisPitchUp_Click(object sender, EventArgs e)
        {
            numAdjustZAxisPitch.Value = (double)numAdjustZAxisPitch.Value + (double)numAdjustZAxisPitch.SpinIncrement;
        }

        private void btnAdjustZAxisPitchDown_Click(object sender, EventArgs e)
        {
            numAdjustZAxisPitch.Value = (double)numAdjustZAxisPitch.Value - (double)numAdjustZAxisPitch.SpinIncrement;
        }

        private void btnAdjustTablePitchUp_Click(object sender, EventArgs e)
        {
            numAdjustTablePitch.Value = (double)numAdjustTablePitch.Value + (double)numAdjustTablePitch.SpinIncrement;
        }

        private void btnAdjustTablePitchDown_Click(object sender, EventArgs e)
        {
            numAdjustTablePitch.Value = (double)numAdjustTablePitch.Value - (double)numAdjustTablePitch.SpinIncrement;
        }
        #endregion

        private void btnAdjustThetaAxisCCWCW_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustThetaAxisCCW);
            Int32 motorNo = (int)MotorNoList.R1DispenseArmThetaAxis;
            Double pitchValue = (double)numAdjustThetaAxisPitch.Value;

            switch (lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.R1R1Aspiration:
                    AdjustValue(upFlg, motorNo, numAdjustThetaAxisOffsetR1Aspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R1R2Aspiration:
                    AdjustValue(upFlg, motorNo, numAdjustThetaAxisOffsetR2Aspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R1MAspiration:
                    AdjustValue(upFlg, motorNo, numAdjustThetaAxisOffsetMReagentAspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R1ReactionCell:
                    AdjustValue(upFlg, motorNo, numAdjustThetaAxisOffsetReactionCellDispense, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R1Cuvette:
                    AdjustValue(upFlg, motorNo, numAdjustThetaAxisOffsetCuvette, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R1PositioningProbe:
                    break;
            }
        }

        private void btnAdjustZAxisDownUp_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustZAxisDown);
            Int32 motorNo = (int)MotorNoList.R1DispenseArmZAxis;
            Double pitchValue = (double)numAdjustZAxisPitch.Value;

            switch (lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.R1R1Aspiration:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetR1R2Aspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R1R2Aspiration:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetR1R2Aspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R1MAspiration:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetMReagentAspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R1ReactionCell:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetReactionCellDispense, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R1Cuvette:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetCuvette, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R1PositioningProbe:
                    break;
            }
        }

        private void btnAdjustTableCCWCW_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustTableCW);
            Int32 motorNo = (int)MotorNoList.ReagentStorageTableThetaAxis;
            Double pitchValue = (double)numAdjustTablePitch.Value;

            switch (lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.R1R1Aspiration:
                    AdjustValue(upFlg, motorNo, numAdjustTableOffsetR1UnitR1Asp, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R1R2Aspiration:
                    AdjustValue(upFlg, motorNo, numAdjustTableOffsetR1UnitR2Asp, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R1MAspiration:
                    AdjustValue(upFlg, motorNo, numAdjustTableOffsetR1UnitMAsp, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R1ReactionCell:
                case (int)MotorAdjustStopPosition.R1Cuvette:
                case (int)MotorAdjustStopPosition.R1PositioningProbe:
                    break;
            }
        }
    }
}
