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
    /// 試薬分注2部
    /// </summary>
    public partial class FormR2DispenseUnit : FormUnitBase
    {
        IMaintenanceUnitStart unitStart = new UnitStart();
        IMaintenanceUnitStart unitStartAdjust = new UnitStarAdjust();
        IMaintenanceUnitStart unitAdjust = new UnitAdjust();
        IMaintenanceUnitStart unitAdjustAbort = new UnitStarAdjustAbort();
        volatile bool ResponseFlg;
        public CommonFunction ComFunc = new CommonFunction();

        //モーター調整コマンドチクチク
        SlaveCommCommand_0473 AdjustUpDownComm = new SlaveCommCommand_0473();
        public FormR2DispenseUnit()
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
            tabUnit.Tabs[0].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_016;                        //Test
            tabUnit.Tabs[1].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_017;                        //Configuration
            tabUnit.Tabs[2].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_018;                        //Motor Parameters
            tabUnit.Tabs[3].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_MOTORADJUSTMENT;  //Adjust

            //Testタブ
            gbxtestSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_000;                              //Sequence
            object SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_001, No=(int)R2DispenceSequence.Init},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_002, No=(int)R2DispenceSequence.R2DispenseandProbeWashing},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_074, No=(int)R2DispenceSequence.MDispenseandProbeWashing},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_005, No=(int)R2DispenceSequence.Prime},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_006, No=(int)R2DispenceSequence.Rinse},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_071, No=(int)R2DispenceSequence.MovetoR2washing},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_072, No=(int)R2DispenceSequence.MovetoR2aspirating},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_073, No=(int)R2DispenceSequence.MovetoR2dispensing},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_095, No=(int)R2DispenceSequence.DetectionSensor},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_103, No=(int)R2DispenceSequence.NozzleWash},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_112, No=(int)R2DispenceSequence.ProbeReplacement},
            };
            lbxtestSequenceListBox = ComFunc.setSequenceListBox(lbxtestSequenceListBox, SequenceList);

            gbxtestParameters.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_008;
            lbltestRepeatFrequency.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_009;
            lbltestNumber.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_010;
            lbltestRepeatInterval.Text = Resources_Maintenance.STRING_MAINTENANCE_R2_109;
            lbltestRepeatIntervalUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_R2_110;
            lbltestPortNo.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_011;
            lbltestWashONOFF.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_100;
            rbttestWashON.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_101;
            rbttestWashOFF.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_102;

            gbxtestResponce.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_015;

            //Configurationタブ
            gbxconfParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_008;//Parameters

            lblconfWaitTimeAfterSuckingUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_019;//The Delay Time after R2 Aspiration
            lblconfWaitTimeAfterDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_020;//The Delay Time after R2 Dispense
            lblconfNoOfPrimeTimes.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_021;//The Number of Times of Prime
            lblconfNoOfRinseTimes.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_022;//The Number of Times of Rinse
            lblconfPrimeVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_023;//Prime Volume
            lblconfDispenseVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_024;//R2 Dispense Volume
            lblconfStandbyPosition.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_025;//R2Dispense Syringe Standby Position
            lblconfUsableElecCapaSensor.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_028;//Capacity Sensor
            rbCapacityON.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_030;//ON
            rbCapacityOFF.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_031;//OFF
            lblconfVomitVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_075;
            lblconfEjectorAirVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_076;
            lblconfProbeSeparationAir.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_077;
            lblconfAirLiftingAfterSuckingUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_078;
            lblconfLiquidSurfaceOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_079;
            lblconfNozzleWashTime.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_080;
            lblconfBubbleMixingValveONTime.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_081;
            lblconfBubbleMixingValveOFFTime.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_082;
            lblconfBubbleMixingValveFrequency.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_083;
            lblconfWashVomitVolume1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_084;
            lblconfWashVomitVolume2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_085;
            lblconfPrimeTime.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_091;
            lblconfDispenseMode.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_092;
            rbtconfDispenseModeEjector.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_094;
            rbtconfDispenseModeVomit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_095;
            lblconfWasteTime.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R1_093;

            //(sec)
            lblconfWaitTimeAfterSuckingUpUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_032;
            lblconfWaitTimeAfterDispenseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_032;
            lblconfNozzleWashTimeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_032;
            lblconfBubbleMixingValveONTimeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_032;
            lblconfBubbleMixingValveOFFTimeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_032;
            //(times)
            lblconfNoOfPrimeTimesUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_033;
            lblconfNoOfRinseTimesUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_033;
            lblconfBubbleMixingValveFrequencyUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_033;
            //(uL)
            lblconfPrimeVolumeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_034;
            lblconfDispenseVolumeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_034;
            lblconfStandbyPositionUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_034;
            lblconfVomitVolumeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_034;
            lblconfEjectorAirVolumeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_034;
            lblconfProbeSeparationAirUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_034;
            lblconfAirLiftingAfterSuckingUpUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_034;
            lblconfWashVomitVolume1Unit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_034;
            lblconfWashVomitVolume2Unit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_034;
            //(mm)
            lblconfLiquidSurfaceOffsetUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_086;


            //MotorParameter
            gbxParamThetaAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_035;
            gbxParamThetaAxisCommon.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_040;
            gbxParamThetaAxisOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_041;
            lblParamThetaAxisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_036;
            lblParamThetaAxisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_037;
            lblParamThetaAxisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_038;
            lblParamThetaAxisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_039;
            lblParamThetaAxisOffsetR2Aspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_093;
            lblParamThetaAxisOffsetMReagentAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_094;
            lblParamThetaAxisOffsetCuvette.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_092;
            lblParamThetaAxisOffsetReactionCellDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_091;
            lblParamThetaAxisOffsetEncodeThresh.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_111;
            lblParamThetaAxisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_044;
            lblParamThetaAxisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_044;
            lblParamThetaAxisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_045;
            lblParamThetaAxisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_044;
            lblParamThetaAxisOffsetR2AspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_046;
            lblParamThetaAxisOffsetMReagentAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_046;
            lblParamThetaAxisOffsetCuvetteUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_046;
            lblParamThetaAxisOffsetReactionCellDispenseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_046;
            lblParamThetaAxisOffsetEncodeThreshUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_046;

            gbxParamZAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_047;
            gbxParamZAxisCommon.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_040;
            gbxParamZAxisLiq.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_042;
            gbxParamZAxisBubble.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_043;
            gbxParamZAxisOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_041;
            lblParamZAxisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_036;
            lblParamZAxisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_037;
            lblParamZAxisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_038;
            lblParamZAxisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_039;
            lblParamZAxisLiqInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_036;
            lblParamZAxisLiqTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_037;
            lblParamZAxisLiqAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_038;
            lblParamZAxisLiqConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_039;
            lblParamZAxisBubbleInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_036;
            lblParamZAxisBubbleTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_037;
            lblParamZAxisBubbleAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_038;
            lblParamZAxisBubbleConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_039;
            lblParamZAxisOffsetR2Aspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_093;
            lblParamZAxisOffsetMReagentAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_094;
            lblParamZAxisOffsetCuvette.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_092;
            lblParamZAxisOffsetReactionCellDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_091;
            lblParamZAxisOffsetPositioningProbe.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_113;
            lblParamZAxisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_044;
            lblParamZAxisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_044;
            lblParamZAxisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_045;
            lblParamZAxisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_044;
            lblParamZAxisLiqInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_044;
            lblParamZAxisLiqTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_044;
            lblParamZAxisLiqAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_045;
            lblParamZAxisLiqConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_044;
            lblParamZAxisBubbleInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_044;
            lblParamZAxisBubbleTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_044;
            lblParamZAxisBubbleAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_045;
            lblParamZAxisBubbleConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_044;
            lblParamZAxisOffsetR2AspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_086;
            lblParamZAxisOffsetMReagentAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_086;
            lblParamZAxisOffsetCuvetteUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_086;
            lblParamZAxisOffsetReactionCellDispenseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_086;
            lblParamZAxisOffsetPositioningProbeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_086;

            gbxParamSyringe.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_056;
            gbxParamSyringeAsp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_048;
            gbxParamSyringeDis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_049;
            gbxParamSyringeAir.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_050;
            lblParamSyringeAspInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_036;
            lblParamSyringeAspTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_037;
            lblParamSyringeAspAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_038;
            lblParamSyringeAspConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_039;
            lblParamSyringeDisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_036;
            lblParamSyringeDisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_037;
            lblParamSyringeDisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_038;
            lblParamSyringeDisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_039;
            lblParamSyringeAirInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_036;
            lblParamSyringeAirTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_037;
            lblParamSyringeAirAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_038;
            lblParamSyringeAirConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_039;
            lblParamSyringeGain.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_069;
            lblParamSyringeOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_070;
            lblParamSyringeAspInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_044;
            lblParamSyringeAspTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_044;
            lblParamSyringeAspAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_045;
            lblParamSyringeAspConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_044;
            lblParamSyringeDisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_044;
            lblParamSyringeDisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_044;
            lblParamSyringeDisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_045;
            lblParamSyringeDisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_044;
            lblParamSyringeAirInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_044;
            lblParamSyringeAirTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_044;
            lblParamSyringeAirAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_045;
            lblParamSyringeAirConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_044;

            //MotorAdjustタブ
            gbxAdjustSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_000;
            SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_096, No=(int)MotorAdjustStopPosition.R2R2Aspiration},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_097, No=(int)MotorAdjustStopPosition.R2MAspiration},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_098, No=(int)MotorAdjustStopPosition.R2ReactionCell},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_099, No=(int)MotorAdjustStopPosition.R2Cuvette},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_114, No=(int)MotorAdjustStopPosition.R2PositioningProbe},
            };
            lbxAdjustSequence = ComFunc.setSequenceListBox(lbxAdjustSequence, SequenceList);

            gbxAdjustParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_008;
            lblAdjustPortNo.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_011;
            lblAdjustCoarseFine.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_COARSEFINE;

            Infragistics.Win.ValueListItem[] cmbValueCoarseFine = new Infragistics.Win.ValueListItem[]
            {
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_COARSE, dataValue:MotorAdjustCoarseFine.Coarse),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_FINE, dataValue:MotorAdjustCoarseFine.Fine),
            };
            cmbAdjustCoarseFine.Items.AddRange(cmbValueCoarseFine);

            gbxAdjustThetaAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_035;
            lblAdjustThetaAxisOffsetR2Aspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_093;
            lblAdjustThetaAxisOffsetMReagentAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_094;
            lblAdjustThetaAxisOffsetCuvette.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_092;
            lblAdjustThetaAxisOffsetReactionCellDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_091;
            lblAdjustThetaAxisOffsetEncodeThresh.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_111;
            lblAdjustThetaAxisOffsetR2AspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_046;
            lblAdjustThetaAxisOffsetMReagentAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_046;
            lblAdjustThetaAxisOffsetCuvetteUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_046;
            lblAdjustThetaAxisOffsetReactionCellDispenseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_046;
            lblAdjustThetaAxisOffsetEncodeThreshUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_046;

            gbxAdjustZAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_047;
            lblAdjustZAxisOffsetR2Aspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_093;
            lblAdjustZAxisOffsetMReagentAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_094;
            lblAdjustZAxisOffsetCuvette.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_092;
            lblAdjustZAxisOffsetReactionCellDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_091;
            lblAdjustZAxisOffsetPositioningProbe.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_113;
            lblAdjustZAxisOffsetMReagentAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_086;
            lblAdjustZAxisOffsetCuvetteUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_086;
            lblAdjustZAxisOffsetReactionCellDispenseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_086;
            lblAdjustZAxisOffsetPositioningProbeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_R2_086;

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
            rbttestWashOFF.Tag = (int)R2DispenceRadioValue.WashOFF;
            rbttestWashON.Tag = (int)R2DispenceRadioValue.WashON;
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
                    StartComm.UnitNo = (int)UnitNoList.ReagentDispense2;
                    StartComm.FuncNo = FuncNo;

                    switch (FuncNo)
                    {
                        case (int)R2DispenceSequence.R2DispenseandProbeWashing:
                        case (int)R2DispenceSequence.MDispenseandProbeWashing:
                            StartComm.Arg1 = (int)numtestPortNo.Value;
                            break;
                        case (int)R2DispenceSequence.DetectionSensor:
                            StartComm.Arg1 = (int)numtestPortNo.Value;
                            StartComm.Arg2 = ComFunc.getSelectedRadioButtonValue(gbxtestWashONOFF);
                            break;
                    }

                    //レスポンスがある機能の場合のみ、レスポンスのログファイルを作成
                    if (ComFunc.chkExistsResponse(MaintenanceMainNavi.ReagentDispense2Unit, FuncNo))
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

                        // コマンド送信
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

                    // メイン画面のナビゲータバーをEnableコマンドバーをデフォルトに戻す。
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

                    if (ComFunc.chkExistsResponse(MaintenanceMainNavi.ReagentDispense2Unit, (int)lbxtestSequenceListBox.SelectedValue))
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
                case (int)R2DispenceSequence.R2DispenseandProbeWashing:
                case (int)R2DispenceSequence.MDispenseandProbeWashing:
                    numtestPortNo.Enabled = true;
                    break;
                case (int)R2DispenceSequence.DetectionSensor:
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

            //試薬分注2部θ軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.R2DispenseArmThetaAxis;
            if (!adjustSave)
            {
                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamThetaAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamThetaAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamThetaAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamThetaAxisConst.Value;

                Mparam.MotorOffset[0] = (double)numParamThetaAxisOffsetR2Aspiration.Value;
                Mparam.MotorOffset[1] = (double)numParamThetaAxisOffsetMReagentAspiration.Value;
                Mparam.MotorOffset[2] = (double)numParamThetaAxisOffsetCuvette.Value;
                Mparam.MotorOffset[3] = (double)numParamThetaAxisOffsetReactionCellDispense.Value;
                Mparam.MotorOffset[4] = (double)numParamThetaAxisOffsetEncodeThresh.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.MotorSpeed;

                Mparam.MotorOffset[0] = (double)numAdjustThetaAxisOffsetR2Aspiration.Value;
                Mparam.MotorOffset[1] = (double)numAdjustThetaAxisOffsetMReagentAspiration.Value;
                Mparam.MotorOffset[2] = (double)numAdjustThetaAxisOffsetCuvette.Value;
                Mparam.MotorOffset[3] = (double)numAdjustThetaAxisOffsetReactionCellDispense.Value;
                Mparam.MotorOffset[4] = (double)numAdjustThetaAxisOffsetEncodeThresh.Value;
            }
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.OffsetR2Aspiration = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.OffsetMReagentAspiration = Mparam.MotorOffset[1];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.OffsetCuvette = Mparam.MotorOffset[2];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.OffsetReactionCellDispense = Mparam.MotorOffset[3];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.OffsetEncodeThresh = Mparam.MotorOffset[4];

            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);


            //試薬分注2部Z軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.R2DispenseArmZAxis;
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

                Mparam.MotorOffset[0] = (double)numParamZAxisOffsetR2Aspiration.Value;
                Mparam.MotorOffset[1] = (double)numParamZAxisOffsetMReagentAspiration.Value;
                Mparam.MotorOffset[2] = (double)numParamZAxisOffsetCuvette.Value;
                Mparam.MotorOffset[3] = (double)numParamZAxisOffsetReactionCellDispense.Value;
                Mparam.MotorOffset[4] = (double)numParamZAxisOffsetPositioningProbe.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.MotorSpeed;

                Mparam.MotorOffset[0] = (double)numAdjustZAxisOffsetR2Aspiration.Value;
                Mparam.MotorOffset[1] = (double)numAdjustZAxisOffsetMReagentAspiration.Value;
                Mparam.MotorOffset[2] = (double)numAdjustZAxisOffsetCuvette.Value;
                Mparam.MotorOffset[3] = (double)numAdjustZAxisOffsetReactionCellDispense.Value;
                Mparam.MotorOffset[4] = (double)numAdjustZAxisOffsetPositioningProbe.Value;
            }
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.OffsetR2Aspiration = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.OffsetMReagentAspiration = Mparam.MotorOffset[1];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.OffsetCuvette = Mparam.MotorOffset[2];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.OffsetReactionCellDispense = Mparam.MotorOffset[3];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.OffsetPositioningProbe = Mparam.MotorOffset[4];

            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);


            if (!adjustSave)
            {
                //試薬分注2ｼﾘﾝｼﾞ
                Mparam = new SlaveCommCommand_0471();
                Mparam.MotorNo = (int)MotorNoList.R2DispenseSyringe;
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
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseSyringeParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseSyringeParam.MotorSpeed = Mparam.MotorSpeed;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseSyringeParam.Gain = Mparam.MotorOffset[0];
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseSyringeParam.Offset = Mparam.MotorOffset[1];

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
                                formMaintenanceMain.subFormR1Dispense[formMaintenanceMain.moduleIndex].MotorParamDisp();
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
                    // プローブ位置調整の場合、開始状態に戻す
                    if ((int)lbxAdjustSequence.SelectedValue == (int)MotorAdjustStopPosition.R2PositioningProbe)
                    {
                        this.ToolbarsControl((int)ToolBarEnable.AdjustReplacingProbe);
                    }
                    else
                    {
                        ToolbarsControl((int)ToolBarEnable.Adjust);
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


            //試薬分注2部θ軸
            numParamThetaAxisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.MotorSpeed[0].InitSpeed;
            numParamThetaAxisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.MotorSpeed[0].TopSpeed;
            numParamThetaAxisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.MotorSpeed[0].Accel;
            numParamThetaAxisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.MotorSpeed[0].ConstSpeed;
            //オフセット
            numParamThetaAxisOffsetR2Aspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.OffsetR2Aspiration;
            numParamThetaAxisOffsetMReagentAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.OffsetMReagentAspiration;
            numParamThetaAxisOffsetCuvette.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.OffsetCuvette;
            numParamThetaAxisOffsetReactionCellDispense.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.OffsetReactionCellDispense;
            numParamThetaAxisOffsetEncodeThresh.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.OffsetEncodeThresh;
            //オフセット（モーター調整用）
            numAdjustThetaAxisOffsetR2Aspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.OffsetR2Aspiration;
            numAdjustThetaAxisOffsetMReagentAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.OffsetMReagentAspiration;
            numAdjustThetaAxisOffsetCuvette.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.OffsetCuvette;
            numAdjustThetaAxisOffsetReactionCellDispense.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.OffsetReactionCellDispense;
            numAdjustThetaAxisOffsetEncodeThresh.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmThetaAxisParam.OffsetEncodeThresh;

            //試薬分注2部Z軸
            //通常
            numParamZAxisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.MotorSpeed[0].InitSpeed;
            numParamZAxisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.MotorSpeed[0].TopSpeed;
            numParamZAxisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.MotorSpeed[0].Accel;
            numParamZAxisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.MotorSpeed[0].ConstSpeed;
            //液面検知（ｿﾌﾄ固定）
            numParamZAxisLiqInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.MotorSpeed[1].InitSpeed;
            numParamZAxisLiqTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.MotorSpeed[1].TopSpeed;
            numParamZAxisLiqAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.MotorSpeed[1].Accel;
            numParamZAxisLiqConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.MotorSpeed[1].ConstSpeed;
            //泡検知動作（ｿﾌﾄ固定）
            numParamZAxisBubbleInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.MotorSpeed[2].InitSpeed;
            numParamZAxisBubbleTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.MotorSpeed[2].TopSpeed;
            numParamZAxisBubbleAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.MotorSpeed[2].Accel;
            numParamZAxisBubbleConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.MotorSpeed[2].ConstSpeed;
            //オフセット
            numParamZAxisOffsetR2Aspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.OffsetR2Aspiration;
            numParamZAxisOffsetMReagentAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.OffsetMReagentAspiration;
            numParamZAxisOffsetCuvette.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.OffsetCuvette;
            numParamZAxisOffsetReactionCellDispense.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.OffsetReactionCellDispense;
            numParamZAxisOffsetPositioningProbe.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.OffsetPositioningProbe;
            //オフセット（モーター調整用）
            numAdjustZAxisOffsetR2Aspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.OffsetR2Aspiration;
            numAdjustZAxisOffsetMReagentAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.OffsetMReagentAspiration;
            numAdjustZAxisOffsetCuvette.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.OffsetCuvette;
            numAdjustZAxisOffsetReactionCellDispense.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.OffsetReactionCellDispense;
            numAdjustZAxisOffsetPositioningProbe.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseArmZAxisParam.OffsetPositioningProbe;

            //R2分注シリンジ
            numParamSyringeAspInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseSyringeParam.MotorSpeed[0].InitSpeed;
            numParamSyringeAspTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseSyringeParam.MotorSpeed[0].TopSpeed;
            numParamSyringeAspAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseSyringeParam.MotorSpeed[0].Accel;
            numParamSyringeAspConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseSyringeParam.MotorSpeed[0].ConstSpeed;

            numParamSyringeDisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseSyringeParam.MotorSpeed[1].InitSpeed;
            numParamSyringeDisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseSyringeParam.MotorSpeed[1].TopSpeed;
            numParamSyringeDisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseSyringeParam.MotorSpeed[1].Accel;
            numParamSyringeDisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseSyringeParam.MotorSpeed[1].ConstSpeed;

            numParamSyringeAirInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseSyringeParam.MotorSpeed[2].InitSpeed;
            numParamSyringeAirTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseSyringeParam.MotorSpeed[2].TopSpeed;
            numParamSyringeAirAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseSyringeParam.MotorSpeed[2].Accel;
            numParamSyringeAirConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseSyringeParam.MotorSpeed[2].ConstSpeed;

            numParamSyringeGain.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseSyringeParam.Gain;
            numParamSyringeOffset.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseSyringeParam.Offset;

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
        private void ConfigParamSave()
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

            CarisXConfigParameter.R2DispenseUnitConfigParam ConfigParam = new CarisXConfigParameter.R2DispenseUnitConfigParam();
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

            config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam = ConfigParam;

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
            numconfWaitTimeAfterSuckingUp.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.WaitTimeAfterSuckingUp;
            //試薬吐出後遅延時間
            numconfWaitTimeAfterDispense.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.WaitTimeAfterDispense;
            //プライム回数
            numconfNoOfPrimeTimes.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.NoOfPrimeTimes;
            //リンス回数
            numconfNoOfRinseTimes.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.NoOfRinseTimes;
            //プライム量
            numconfPrimeVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.PrimeVolume;
            //試薬分注量
            numconfDispenseVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.DispenseVolume;
            //試薬シリンジ待機位置
            numconfStandbyPosition.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.StandbyPosition;

            //静電容量センサー使用有無
            if (config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.UsableElecCapaSensor == 1)
                rbCapacityON.Checked = true;
            else
                rbCapacityOFF.Checked = true;

            //試薬吐き残し量
            numconfVomitVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.VomitVolume;
            //試薬追い出しエアー量
            numconfEjectorAirVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.EjectorAirVolume;
            //試薬プローブ分離エアー量
            numconfProbeSeparationAir.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.ProbeSeparationAir;
            //試薬吸引後エアー引き上げ量
            numconfAirLiftingAfterSuckingUp.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.AirLiftingAfterSuckingUp;
            //試薬液面オフセット
            numconfLiquidSurfaceOffset.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.LiquidSurfaceOffset;
            //ノズル洗浄時間
            numconfNozzleWashTime.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.NozzleWashTime;
            //気泡混入バルブON時間
            numconfBubbleMixingValveONTime.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.BubbleMixingValveONTime;
            //気泡混入バルブOFF時間
            numconfBubbleMixingValveOFFTime.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.BubbleMixingValveOFFTime;
            //気泡混入バルブ動作回数
            numconfBubbleMixingValveFrequency.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.BubbleMixingValveFrequency;
            //洗浄後洗浄液吐出量1
            numconfWashVomitVolume1.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.WashVomitVolume1;
            //洗浄後洗浄液吐出量2
            numconfWashVomitVolume2.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.WashVomitVolume2;
            //プライム時間
            numconfPrimeTime.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.PrimeTime;

            //吐出方法
            if (config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.DispenseMode == 1)
                rbtconfDispenseModeVomit.Checked = true;
            else
                rbtconfDispenseModeEjector.Checked = true;

            //廃液時間
            numconfWasteTime.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2DispenseUnitConfigParam.WasteTime;

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
            numAdjustThetaAxisOffsetR2Aspiration.Enabled = false;
            numAdjustThetaAxisOffsetMReagentAspiration.Enabled = false;
            numAdjustThetaAxisOffsetCuvette.Enabled = false;
            numAdjustThetaAxisOffsetReactionCellDispense.Enabled = false;
            numAdjustThetaAxisOffsetEncodeThresh.Enabled = false;

            gbxAdjustZAxis.Enabled = false;
            numAdjustZAxisOffsetR2Aspiration.Enabled = false;
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

            if (stopPos != (int)MotorAdjustStopPosition.R2PositioningProbe)
            {
                // 試薬プローブ位置調整以外 =>

                // 調整位置停止コマンド
                SlaveCommCommand_0480 AdjustStartComm = new SlaveCommCommand_0480();
                AdjustStartComm.Pos = stopPos;
                switch (AdjustStartComm.Pos)
                {
                    case (int)MotorAdjustStopPosition.R2R2Aspiration:
                    case (int)MotorAdjustStopPosition.R2MAspiration:
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
                PositioningProbeComm.Status = SlaveCommCommand_0497.probeUnit.R2Unit;

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
                    if ((int)lbxAdjustSequence.SelectedValue == (int)MotorAdjustStopPosition.R2PositioningProbe)
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
        private void lbxAdjustSequence_SelectedIndexChanged(object sender, EventArgs e)
        {
            adjustGbAllDisable();
            //モータ調整ボタンDisable
            UpDownButtonEnable(false);

            this.ToolbarsControl((int)ToolBarEnable.Default);

            switch (lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.R2R2Aspiration:
                    numAdjustPortNo.Enabled = true;
                    cmbAdjustCoarseFine.Enabled = true;

                    gbxAdjustThetaAxis.Enabled = true;
                    numAdjustThetaAxisOffsetR2Aspiration.Enabled = true;
                    numAdjustThetaAxisOffsetR2Aspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetR2Aspiration.Enabled = true;
                    numAdjustZAxisOffsetR2Aspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustTable.Enabled = true;
                    numAdjustTableOffsetR2UnitR2Asp.Enabled = true;
                    numAdjustTableOffsetR2UnitR2Asp.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

                case (int)MotorAdjustStopPosition.R2MAspiration:
                    numAdjustPortNo.Enabled = true;
                    cmbAdjustCoarseFine.Enabled = true;

                    gbxAdjustThetaAxis.Enabled = true;
                    numAdjustThetaAxisOffsetMReagentAspiration.Enabled = true;
                    numAdjustThetaAxisOffsetMReagentAspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetMReagentAspiration.Enabled = true;
                    numAdjustZAxisOffsetMReagentAspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustTable.Enabled = true;
                    numAdjustTableOffsetR2UnitMAsp.Enabled = true;
                    numAdjustTableOffsetR2UnitMAsp.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

                case (int)MotorAdjustStopPosition.R2ReactionCell:
                    gbxAdjustThetaAxis.Enabled = true;
                    numAdjustThetaAxisOffsetReactionCellDispense.Enabled = true;
                    numAdjustThetaAxisOffsetReactionCellDispense.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetReactionCellDispense.Enabled = true;
                    numAdjustZAxisOffsetReactionCellDispense.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

                case (int)MotorAdjustStopPosition.R2Cuvette:
                    gbxAdjustThetaAxis.Enabled = true;
                    numAdjustThetaAxisOffsetCuvette.Enabled = true;
                    numAdjustThetaAxisOffsetCuvette.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetCuvette.Enabled = true;
                    numAdjustZAxisOffsetCuvette.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

                case (int)MotorAdjustStopPosition.R2PositioningProbe:
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
            Boolean upFlg = ((Button)sender == btnAdjustThetaAxisCW);
            Int32 motorNo = (int)MotorNoList.R2DispenseArmThetaAxis;
            Double pitchValue = (double)numAdjustThetaAxisPitch.Value;

            switch (lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.R2R2Aspiration:
                    AdjustValue(upFlg, motorNo, numAdjustThetaAxisOffsetR2Aspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R2MAspiration:
                    AdjustValue(upFlg, motorNo, numAdjustThetaAxisOffsetMReagentAspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R2ReactionCell:
                    AdjustValue(upFlg, motorNo, numAdjustThetaAxisOffsetReactionCellDispense, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R2Cuvette:
                    AdjustValue(upFlg, motorNo, numAdjustThetaAxisOffsetCuvette, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R2PositioningProbe:
                    break;
            }
        }

        private void btnAdjustZAxisDownUp_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustZAxisDown);
            Int32 motorNo = (int)MotorNoList.R2DispenseArmZAxis;
            Double pitchValue = (double)numAdjustZAxisPitch.Value;

            switch (lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.R2R2Aspiration:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetR2Aspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R2MAspiration:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetMReagentAspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R2ReactionCell:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetReactionCellDispense, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R2Cuvette:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetCuvette, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R2PositioningProbe:
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
                case (int)MotorAdjustStopPosition.R2R2Aspiration:
                    AdjustValue(upFlg, motorNo, numAdjustTableOffsetR2UnitR2Asp, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R2MAspiration:
                    AdjustValue(upFlg, motorNo, numAdjustTableOffsetR2UnitMAsp, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.R2ReactionCell:
                case (int)MotorAdjustStopPosition.R2Cuvette:
                case (int)MotorAdjustStopPosition.R2PositioningProbe:
                    break;
            }
        }
    }
}
