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
using Infragistics.Win.UltraWinEditors;

namespace Oelco.CarisX.Maintenance
{
    /// <summary>
    /// サンプル分注移送部
    /// </summary>
    public partial class FormSampleDispenseUnit : FormUnitBase
    {
        IMaintenanceUnitStart unitStart = new UnitStart();
        IMaintenanceUnitStart unitStartAdjust = new UnitStarAdjust();
        IMaintenanceUnitStart unitAdjustAbort = new UnitStarAdjustAbort();
        volatile bool ResponseFlg;
        public CommonFunction ComFunc = new CommonFunction();

        public FormSampleDispenseUnit()
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
            cmbAdjustTipNo.SelectedIndex = 0;
            cmbAdjustCoarseFine.SelectedIndex = 0;
            cmbAdjustSampleTubeType.SelectedIndex = 0;
            cmbAdjustPortNo.SelectedIndex = 0;
            cmbAdjustSamplePosition.SelectedIndex = 0;

        }

        #region リソース設定
        private void setCulture()
        {
            //Tab
            tabUnit.Tabs[0].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_038;                //Test
            tabUnit.Tabs[1].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_039;                //Configuration
            tabUnit.Tabs[2].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_040;                //Motor Parameters
            tabUnit.Tabs[3].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_MOTORADJUSTMENT;      //Adjust

            //Testタブ
            gbxtestSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_000;                          //Sequence
            object SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_001, No=(int)SampleDispenseSequence.Init},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_002, No=(int)SampleDispenseSequence.RackDispensing},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_003, No=(int)SampleDispenseSequence.STATorExternalTransferDispensing},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_004, No=(int)SampleDispenseSequence.ReactionTableDispensing},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_005, No=(int)SampleDispenseSequence.RemovesSampleTip},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_116, No=(int)SampleDispenseSequence.DetectionSensor},
            };
            lbxtestSequenceListBox = ComFunc.setSequenceListBox(lbxtestSequenceListBox, SequenceList);

            gbxtestParameters.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_007;                        //Parameters
            lbltestRepeatFrequency.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_008;                   //Repeat Frequency
            lbltestNumber.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_009;                            //Number
            lbltestTipNumber.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_010;                         //Tip Number
            lbltestRepeatInterval.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_142;
            lbltestRepeatIntervalUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_143;
            lbltestFractionationPosition.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_088;             //Fractionation Position
            rbttestSTAT.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_089;                              //STAT
            rbttestExternalTransport.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_090;                 //External Transport
            rbttestDilutionPosition.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_091;                  //Dilution Position
            rbttestPretreatmentPosition.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_092;              //Pretreatment Position
            lbltestFront.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_093;                             //Front
            rbttestFront1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_094;                            //1
            rbttestFront2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_095;                            //2
            rbttestFront3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_096;                            //3
            rbttestFront4.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_097;                            //4
            rbttestFront5.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_098;                            //5
            lbltestBack.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_099;                              //Back
            rbttestBack1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_100;                             //1
            rbttestBack2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_101;                             //2
            rbttestBack3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_102;                             //3
            rbttestBack4.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_103;                             //4
            rbttestBack5.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_104;                             //5
            lbltestReactionCellVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_105;                //Reaction Cell Volume (Preset)
            lbltestUnitReactionCellVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_106;            //uL
            lbltestTestNo.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_135;

            gbxtestResponce.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_011;                          //Response

            //Configurationタブ
            gbxConfParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_007;                           //Parameters

            lblconfDispenceCyringeStandbyPosition.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_012;
            lblconfSmpSurplusVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_013;
            lblconfSmpDispenseVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_014;
            lblconfWaitTimeAfterSuckingUpForSample.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_015;
            lblconfContainerKind.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_016;
            lbxconfContainerKind.Items[0] = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_017;
            lbxconfContainerKind.Items[1] = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_018;
            lbxconfContainerKind.Items[2] = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_019;

            lblconfSampleKind.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_020;//Sample Type
            lbxconfSampleKind.Items[0] = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_021;//Serum
            lbxconfSampleKind.Items[1] = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_022;//Urine
            lbxconfSampleKind.Items[2] = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_107;

            lblconfLiquidLevelSensor.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_023;//Use Surface Detection Sesnor
            rbtLiquidLevelSensorUSE.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_027;//ON
            rbtLiquidLevelSensorNOUSE.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_028;//OFF

            lblconfBubbleCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_024;//Use Bubble Check On Sample
            rbtBubbleCheckUSE.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_027;//ON
            rbtBubbleCheckNOUSE.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_028;//OFF

            lblconfUpperOfPresa1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_025;//Upper Pressure Value1
            lblconfUpperOfPresa2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_026;//Upper Pressure Value2
            lblconfUpperOfSuckingUpErr1ForSerum.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_029;//Aspiration Error Upper Limit1 (For Serum)
            lblconfLowerOfSuckingUpErr1ForSerum.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_030;//Aspiration Error Lower Limit1 (For Serum)
            lblconfUpperOfSuckingUpErr1ForUrine.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_031;//Aspiration Error Upper Limit1 (For Urine)
            lblconfLowerOfSuckingUpErr1ForUrine.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_032;//Aspiration Error Lower Limit1 (For Urine)
            lblconfLowerOfLeakErrFluidVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_033;//Lower limit volume of liquid for leakage error
            lblconfLowerOfLeakErrPress.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_034;//Lower limit pressure for leakage error
            lblconfThresholdDefectiveDischarge.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_035;//Dispensing error threshold
            lblconfPreSampleDispenseVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_108;

            lblconfUpperOfPresa1Over100.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_144;                  //Upper Pressure Value1
            lblconfUpperOfPresa2Over100.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_145;                  //Upper Pressure Value2
            lblconfUpperOfSuckingUpErr1ForSerumOver100.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_146;   //Aspiration Error Upper Limit1 (For Serum)
            lblconfLowerOfSuckingUpErr1ForSerumOver100.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_147;   //Aspiration Error Lower Limit1 (For Serum)
            lblconfUpperOfSuckingUpErr1ForUrineOver100.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_148;   //Aspiration Error Upper Limit1 (For Urine)
            lblconfLowerOfSuckingUpErr1ForUrineOver100.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_149;   //Aspiration Error Lower Limit1 (For Urine)
            lblconfLowerOfLeakErrFluidVolumeOver100.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_150;      //Lower limit volume of liquid for leakage error
            lblconfLowerOfLeakErrPressOver100.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_151;            //Lower limit pressure for leakage error
            lblconfThresholdDefectiveDischargeOver100.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_152;    //Dispensing error threshold

            lblconfDispenceCyringeStandbyPositionUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_036;//(uL)
            lblconfSmpSurplusVolumeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_036;//(uL)
            lblconfSmpDispenseVolumeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_036;//(uL)

            lblconfWaitTimeAfterSuckingUpForSampleUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_037;//(sec)

            //MotorParameter
            gbxParamYAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_041;
            lblParamYAxisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_042;
            lblParamYAxisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_043;
            lblParamYAxisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_044;
            lblParamYAxisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_045;
            lblParamYAxisOffsetRack1Aspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_117;
            lblParamYAxisOffsetRack5Aspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_118;
            lblParamYAxisOffsetSTATAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_048;
            lblParamYAxisOffsetDiluentAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_049;
            lblParamYAxisOffsetPretreatAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_050;
            lblParamYAxisOffsetLineAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_109;
            lblParamYAxisOffsetSampleDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_110;
            lblParamYAxisOffsetTipRemove.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_111;
            lblParamYAxisOffsetTip1Catch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_119;
            lblParamYAxisOffsetTip6Catch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_120;
            lblParamYAxisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamYAxisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamYAxisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_053;
            lblParamYAxisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamYAxisOffsetRack1AspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblParamYAxisOffsetRack5AspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblParamYAxisOffsetSTATAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblParamYAxisOffsetDiluentAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblParamYAxisOffsetPretreatAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblParamYAxisOffsetLineAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblParamYAxisOffsetSampleDispenseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblParamYAxisOffsetTipRemoveUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblParamYAxisOffsetTip1CatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblParamYAxisOffsetTip6CatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;

            gbxParamZAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_062;
            lblParamZAxisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_042;
            lblParamZAxisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_043;
            lblParamZAxisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_044;
            lblParamZAxisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_045;
            lblParamZAxisLiqInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_042;
            lblParamZAxisLiqTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_043;
            lblParamZAxisLiqAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_044;
            lblParamZAxisLiqConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_045;
            lblParamZAxisOffsetRackAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_047;
            lblParamZAxisOffsetSTATAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_048;
            lblParamZAxisOffsetLineAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_109;
            lblParamZAxisOffsetDiluentAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_121;
            lblParamZAxisOffsetTipRemove.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_111;
            lblParamZAxisOffsetTipCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_046;
            lblParamZAxisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamZAxisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamZAxisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_053;
            lblParamZAxisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamZAxisLiqInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamZAxisLiqTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamZAxisLiqAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_053;
            lblParamZAxisLiqConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamZAxisOffsetRackAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblParamZAxisOffsetSTATAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblParamZAxisOffsetLineAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblParamZAxisOffsetDiluentAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblParamZAxisOffsetTipRemoveUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblParamZAxisOffsetTipCatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;

            gbxParamThetaAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_054;
            lblParamThetaAxisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_042;
            lblParamThetaAxisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_043;
            lblParamThetaAxisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_044;
            lblParamThetaAxisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_045;
            lblParamThetaAxisOffsetRack1Aspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_117;
            lblParamThetaAxisOffsetRack5Aspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_118;
            lblParamThetaAxisOffsetSTATAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_048;
            lblParamThetaAxisOffsetDiluentAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_049;
            lblParamThetaAxisOffsetPretreatAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_050;
            lblParamThetaAxisOffsetLineAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_109;
            lblParamThetaAxisOffsetSampleDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_110;
            lblParamThetaAxisOffsetTipRemove.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_111;
            lblParamThetaAxisOffsetTip1Catch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_119;
            lblParamThetaAxisOffsetTip6Catch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_120;
            lblParamThetaAxisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamThetaAxisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamThetaAxisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_053;
            lblParamThetaAxisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamThetaAxisOffsetRack1AspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_115;
            lblParamThetaAxisOffsetRack5AspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_115;
            lblParamThetaAxisOffsetSTATAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_115;
            lblParamThetaAxisOffsetDiluentAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_115;
            lblParamThetaAxisOffsetPretreatAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_115;
            lblParamThetaAxisOffsetLineAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_115;
            lblParamThetaAxisOffsetSampleDispenseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_115;
            lblParamThetaAxisOffsetTipRemoveUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_115;
            lblParamThetaAxisOffsetTip1CatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_115;
            lblParamThetaAxisOffsetTip6CatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_115;

            gbxParamSyringe.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_071;
            lblParamSyringeLiqInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_042;
            lblParamSyringeLiqTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_043;
            lblParamSyringeLiqAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_044;
            lblParamSyringeLiqConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_045;
            lblParamSyringeAspInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_042;
            lblParamSyringeAspTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_043;
            lblParamSyringeAspAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_044;
            lblParamSyringeAspConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_045;
            lblParamSyringeDisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_042;
            lblParamSyringeDisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_043;
            lblParamSyringeDisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_044;
            lblParamSyringeDisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_045;
            lblParamSyringeAirInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_042;
            lblParamSyringeAirTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_043;
            lblParamSyringeAirAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_044;
            lblParamSyringeAirConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_045;
            lblParamSyringeAspUp50Init.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_042;
            lblParamSyringeAspUp50Top.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_043;
            lblParamSyringeAspUp50Accelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_044;
            lblParamSyringeAspUp50Const.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_045;
            lblParamSyringeDisUp50Init.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_042;
            lblParamSyringeDisUp50Top.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_043;
            lblParamSyringeDisUp50Accelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_044;
            lblParamSyringeDisUp50Const.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_045;
            lblParamSyringeGain.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_084;
            lblParamSyringeOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_085;
            lblParamSyringeGainOver100.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_153;
            lblParamSyringeOffsetOver100.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_154;
            lblParamSyringeLiqInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamSyringeLiqTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamSyringeLiqAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_053;
            lblParamSyringeLiqConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamSyringeAspInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamSyringeAspTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamSyringeAspAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_053;
            lblParamSyringeAspConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamSyringeDisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamSyringeDisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamSyringeDisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_053;
            lblParamSyringeDisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamSyringeAirInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamSyringeAirTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamSyringeAirAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_053;
            lblParamSyringeAirConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamSyringeAspUp50InitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamSyringeAspUp50TopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamSyringeAspUp50AcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_053;
            lblParamSyringeAspUp50ConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamSyringeDisUp50InitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamSyringeDisUp50TopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;
            lblParamSyringeDisUp50AcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_053;
            lblParamSyringeDisUp50ConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_052;

            gbxParamYAxisCommon.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_059;
            gbxParamZAxisCommon.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_059;
            gbxParamThetaAxisCommon.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_059;

            gbxParamYAxisOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_060;
            gbxParamZAxisOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_060;
            gbxParamThetaAxisOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_060;

            gbxParamZAxisLiq.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_112;
            gbxParamSyringeLiq.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_112;

            gbxParamSyringeAsp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_072;
            gbxParamSyringeDis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_076;
            gbxParamSyringeAir.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_080;
            gbxParamSyringeAspUp50.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_113;
            gbxParamSyringeDisUp50.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_114;


            //MotorAdjustタブ
            gbxAdjustSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_000;
            SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_122, No=(int)MotorAdjustStopPosition.SampleTipCatch},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_123, No=(int)MotorAdjustStopPosition.RackAspiration},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_124, No=(int)MotorAdjustStopPosition.STATAspiration},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_125, No=(int)MotorAdjustStopPosition.DiluentAspiration },
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_126, No=(int)MotorAdjustStopPosition.PretreatAspiration},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_127, No=(int)MotorAdjustStopPosition.SampleDispense},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_128, No=(int)MotorAdjustStopPosition.SampleTipRemover},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_129, No=(int)MotorAdjustStopPosition.LineSampleAspiration},
            };
            lbxAdjustSequence = ComFunc.setSequenceListBox(lbxAdjustSequence, SequenceList);

            gbxAdjustParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_007;
            lblAdjustTipNumber.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_010;
            lblAdjustCoarseFine.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_COARSEFINE;
            lblAdjustSampleTubeType.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_016;
            lblAdjustPortNo.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_134;
            lblAdjustSamplePosition.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_133;

            Infragistics.Win.ValueListItem[] cmbValueTipNumber = new Infragistics.Win.ValueListItem[]
            {
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_136, dataValue:SampleDispenseCmbValue.TipNumber1),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_137, dataValue:SampleDispenseCmbValue.TipNumber6),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_138, dataValue:SampleDispenseCmbValue.TipNumber91),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_139, dataValue:SampleDispenseCmbValue.TipNumber96),
            };
            Infragistics.Win.ValueListItem[] cmbValueCoarseFine = new Infragistics.Win.ValueListItem[]
            {
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_COARSE, dataValue:MotorAdjustCoarseFine.Coarse),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_FINE, dataValue:MotorAdjustCoarseFine.Fine),
            };
            Infragistics.Win.ValueListItem[] cmbValueSampleTubeType = new Infragistics.Win.ValueListItem[]
            {
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_017, dataValue:SpecimenCupKind.Cup),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_018, dataValue:SpecimenCupKind.Tube),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_019, dataValue:SpecimenCupKind.CupOnTube),
            };
            Infragistics.Win.ValueListItem[] cmbValuePortNo = new Infragistics.Win.ValueListItem[]
            {
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_099, dataValue:SampleDispensePortNo.Back),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_093, dataValue:SampleDispensePortNo.Front),
            };
            Infragistics.Win.ValueListItem[] cmbValueSamplePosition = new Infragistics.Win.ValueListItem[]
            {
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_140, dataValue:SampleDispenseCmbValue.SamplePosition1),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_141, dataValue:SampleDispenseCmbValue.SamplePosition5),
            };
            cmbAdjustTipNo.Items.AddRange(cmbValueTipNumber);
            cmbAdjustCoarseFine.Items.AddRange(cmbValueCoarseFine);
            cmbAdjustSampleTubeType.Items.AddRange(cmbValueSampleTubeType);
            cmbAdjustPortNo.Items.AddRange(cmbValuePortNo);
            cmbAdjustSamplePosition.Items.AddRange(cmbValueSamplePosition);

            gbxAdjustYAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_041;
            lblAdjustYAxisOffsetRack1Aspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_117;
            lblAdjustYAxisOffsetRack5Aspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_118;
            lblAdjustYAxisOffsetSTATAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_048;
            lblAdjustYAxisOffsetDiluentAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_049;
            lblAdjustYAxisOffsetPretreatAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_050;
            lblAdjustYAxisOffsetLineAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_109;
            lblAdjustYAxisOffsetSampleDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_110;
            lblAdjustYAxisOffsetTipRemove.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_111;
            lblAdjustYAxisOffsetTip1Catch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_119;
            lblAdjustYAxisOffsetTip6Catch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_120;
            lblAdjustYAxisOffsetRack1AspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblAdjustYAxisOffsetRack5AspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblAdjustYAxisOffsetSTATAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblAdjustYAxisOffsetDiluentAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblAdjustYAxisOffsetPretreatAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblAdjustYAxisOffsetLineAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblAdjustYAxisOffsetSampleDispenseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblAdjustYAxisOffsetTipRemoveUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblAdjustYAxisOffsetTip1CatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblAdjustYAxisOffsetTip6CatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;

            gbxAdjustZAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_062;
            lblAdjustZAxisOffsetRackAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_047;
            lblAdjustZAxisOffsetSTATAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_048;
            lblAdjustZAxisOffsetLineAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_109;
            lblAdjustZAxisOffsetDiluentAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_121;
            lblAdjustZAxisOffsetTipRemove.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_111;
            lblAdjustZAxisOffsetTipCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_046;
            lblAdjustZAxisOffsetRackAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblAdjustZAxisOffsetSTATAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblAdjustZAxisOffsetLineAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblAdjustZAxisOffsetDiluentAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblAdjustZAxisOffsetTipRemoveUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;
            lblAdjustZAxisOffsetTipCatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_051;

            gbxAdjustThetaAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_054;
            lblAdjustThetaAxisOffsetRack1Aspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_117;
            lblAdjustThetaAxisOffsetRack5Aspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_118;
            lblAdjustThetaAxisOffsetSTATAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_048;
            lblAdjustThetaAxisOffsetDiluentAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_049;
            lblAdjustThetaAxisOffsetPretreatAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_050;
            lblAdjustThetaAxisOffsetLineAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_109;
            lblAdjustThetaAxisOffsetSampleDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_110;
            lblAdjustThetaAxisOffsetTipRemove.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_111;
            lblAdjustThetaAxisOffsetTip1Catch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_119;
            lblAdjustThetaAxisOffsetTip6Catch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_120;
            lblAdjustThetaAxisOffsetRack1AspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_115;
            lblAdjustThetaAxisOffsetRack5AspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_115;
            lblAdjustThetaAxisOffsetSTATAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_115;
            lblAdjustThetaAxisOffsetDiluentAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_115;
            lblAdjustThetaAxisOffsetPretreatAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_115;
            lblAdjustThetaAxisOffsetLineAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_115;
            lblAdjustThetaAxisOffsetSampleDispenseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_115;
            lblAdjustThetaAxisOffsetTipRemoveUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_115;
            lblAdjustThetaAxisOffsetTip1CatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_115;
            lblAdjustThetaAxisOffsetTip6CatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLEDISPENSE_115;

            gbxAdjustCaseYAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_015;
            lblAdjustCaseYAxisOffsetCaseCatchRelease.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_020;
            lblAdjustCaseYAxisOffsetReactionCellCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_021;
            lblAdjustCaseYAxisOffsetTipCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_031;
            lblAdjustCaseYAxisOffsetCaseCatchReleaseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_025;
            lblAdjustCaseYAxisOffsetReactionCellCatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_025;
            lblAdjustCaseYAxisOffsetTipCatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_025;

            gbxAdjustSTATYAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_011;
            lblAdjustSTATYAxisOffsetSTATAspiration.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_020;
            lblAdjustSTATYAxisOffsetSTATAspirationUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_021;

            lblAdjustYAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;
            lblAdjustZAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;
            lblAdjustThetaAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;
            lblAdjustCaseYAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;
            lblAdjustSTATYAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;

            btnAdjustYAxisForward.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_FORWARD;
            btnAdjustCaseYAxisForward.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_FORWARD;
            btnAdjustSTATYAxisForward.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_FORWARD;
            btnAdjustYAxisBack.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_BACK;
            btnAdjustCaseYAxisBack.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_BACK;
            btnAdjustSTATYAxisBack.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_BACK;

            btnAdjustZAxisDown.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_DOWN;
            btnAdjustZAxisUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_UP;

            btnAdjustThetaAxisCW.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_CW_L;
            btnAdjustThetaAxisCCW.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_CCW_R;

        }
        #endregion

        /// <summary>
        /// ラジオボタンの選択値を設定
        /// </summary>
        private void setRadioButtonValue()
        {
            rbttestSTAT.Tag = (int)SampleDispenseRadioValue.STAT;
            rbttestExternalTransport.Tag = (int)SampleDispenseRadioValue.ExternalTransport;
            rbttestDilutionPosition.Tag = (int)SampleDispenseRadioValue.DilutionPosition;
            rbttestPretreatmentPosition.Tag = (int)SampleDispenseRadioValue.PretreatmentPosition;
            rbttestFront1.Tag = (int)SampleDispenseRadioValue.Front1;
            rbttestFront2.Tag = (int)SampleDispenseRadioValue.Front2;
            rbttestFront3.Tag = (int)SampleDispenseRadioValue.Front3;
            rbttestFront4.Tag = (int)SampleDispenseRadioValue.Front4;
            rbttestFront5.Tag = (int)SampleDispenseRadioValue.Front5;
            rbttestBack1.Tag = (int)SampleDispenseRadioValue.Back1;
            rbttestBack2.Tag = (int)SampleDispenseRadioValue.Back2;
            rbttestBack3.Tag = (int)SampleDispenseRadioValue.Back3;
            rbttestBack4.Tag = (int)SampleDispenseRadioValue.Back4;
            rbttestBack5.Tag = (int)SampleDispenseRadioValue.Back5;
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
                    StartComm.UnitNo = (int)UnitNoList.SampleDispense;
                    StartComm.FuncNo = FuncNo;

                    switch (FuncNo)
                    {
                        case (int)SampleDispenseSequence.RackDispensing:
                        case (int)SampleDispenseSequence.STATorExternalTransferDispensing:
                        case (int)SampleDispenseSequence.ReactionTableDispensing:
                            StartComm.Arg1 = ComFunc.getSelectedRadioButtonValue(gbxtestFractionationPosition);
                            StartComm.Arg2 = (int)numtestTipNumber.Value;
                            StartComm.Arg3 = (int)numtestReactionCellVolume.Value;
                            break;
                        case (int)SampleDispenseSequence.RemovesSampleTip:
                            StartComm.Arg1 = (int)numtestTipNumber.Value;
                            break;
                        case (int)SampleDispenseSequence.DetectionSensor:
                            StartComm.Arg1 = ComFunc.getSelectedRadioButtonValue(gbxtestFractionationPosition);
                            StartComm.Arg2 = (int)numtestTipNumber.Value;
                            StartComm.Arg3 = (int)numtestTestNo.Value;
                            break;
                    }

                    // レスポンスがある機能の場合のみ、レスポンスのログファイルを作成
                    if (ComFunc.chkExistsResponse(MaintenanceMainNavi.SampleDispenseUnit, FuncNo))
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
                    TextData resultText = new TextData(Result);
                    String tempString = String.Empty;

                    if (ComFunc.chkExistsResponse(MaintenanceMainNavi.SampleDispenseUnit, (int)lbxtestSequenceListBox.SelectedValue))
                    {
                        switch ((int)lbxtestSequenceListBox.SelectedValue)
                        {
                            case (int)SampleDispenseSequence.RackDispensing:
                            case (int)SampleDispenseSequence.STATorExternalTransferDispensing:
                            case (int)SampleDispenseSequence.ReactionTableDispensing:
                            case (int)SampleDispenseSequence.DetectionSensor:
                                resultText.spoilString(out tempString, 6); 
                                Result =  "Reference=" + tempString;
                                resultText.spoilString(out tempString, 6);
                                Result += ",Aspiration point 1=" + tempString;
                                resultText.spoilString(out tempString, 6);
                                Result += ",Aspiration point 2=" + tempString;
                                resultText.spoilString(out tempString, 1);
                                Result += ",Test result=" + tempString;
                                resultText.spoilString(out tempString, 1);
                                Result += ",Sample type=" + tempString;
                                resultText.spoilString(out tempString, 5);
                                Result += ",Aspiration delta=" + tempString;
                                resultText.spoilString(out tempString, 6);
                                Result += ",Descent Amount=" + tempString;
                                break;
                        }

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
                    switch ((int)lbxAdjustSequence.SelectedValue)
                    {
                        case (int)MotorAdjustStopPosition.SampleTipCatch:
                            switch ((int)cmbAdjustTipNo.Value)
                            {
                                case (int)SampleDispenseCmbValue.TipNumber1:
                                case (int)SampleDispenseCmbValue.TipNumber91:
                                    numAdjustYAxisOffsetTip1Catch.Enabled = true;
                                    numAdjustYAxisOffsetTip1Catch.ForeColor = System.Drawing.Color.OrangeRed;
                                    numAdjustThetaAxisOffsetTip1Catch.Enabled = true;
                                    numAdjustThetaAxisOffsetTip1Catch.ForeColor = System.Drawing.Color.OrangeRed;
                                    numAdjustYAxisOffsetTip6Catch.Enabled = false;
                                    numAdjustYAxisOffsetTip6Catch.ForeColor = System.Drawing.Color.Black;
                                    numAdjustThetaAxisOffsetTip6Catch.Enabled = false;
                                    numAdjustThetaAxisOffsetTip6Catch.ForeColor = System.Drawing.Color.Black;
                                    break;
                                case (int)SampleDispenseCmbValue.TipNumber6:
                                case (int)SampleDispenseCmbValue.TipNumber96:
                                    numAdjustYAxisOffsetTip1Catch.Enabled = false;
                                    numAdjustYAxisOffsetTip1Catch.ForeColor = System.Drawing.Color.Black;
                                    numAdjustThetaAxisOffsetTip1Catch.Enabled = false;
                                    numAdjustThetaAxisOffsetTip1Catch.ForeColor = System.Drawing.Color.Black;
                                    numAdjustYAxisOffsetTip6Catch.Enabled = true;
                                    numAdjustYAxisOffsetTip6Catch.ForeColor = System.Drawing.Color.OrangeRed;
                                    numAdjustThetaAxisOffsetTip6Catch.Enabled = true;
                                    numAdjustThetaAxisOffsetTip6Catch.ForeColor = System.Drawing.Color.OrangeRed;
                                    break;
                            }
                            break;
                        case (int)MotorAdjustStopPosition.RackAspiration:
                            switch ((int)cmbAdjustSamplePosition.Value)
                            {
                                case (int)SampleDispenseCmbValue.SamplePosition1:
                                    numAdjustYAxisOffsetRack1Aspiration.Enabled = true;
                                    numAdjustYAxisOffsetRack1Aspiration.ForeColor = System.Drawing.Color.OrangeRed;
                                    numAdjustThetaAxisOffsetRack1Aspiration.Enabled = true;
                                    numAdjustThetaAxisOffsetRack1Aspiration.ForeColor = System.Drawing.Color.OrangeRed;
                                    numAdjustYAxisOffsetRack5Aspiration.Enabled = false;
                                    numAdjustYAxisOffsetRack5Aspiration.ForeColor = System.Drawing.Color.Black;
                                    numAdjustThetaAxisOffsetRack5Aspiration.Enabled = false;
                                    numAdjustThetaAxisOffsetRack5Aspiration.ForeColor = System.Drawing.Color.Black;
                                    break;
                                case (int)SampleDispenseCmbValue.SamplePosition5:
                                    numAdjustYAxisOffsetRack1Aspiration.Enabled = false;
                                    numAdjustYAxisOffsetRack1Aspiration.ForeColor = System.Drawing.Color.Black;
                                    numAdjustThetaAxisOffsetRack1Aspiration.Enabled = false;
                                    numAdjustThetaAxisOffsetRack1Aspiration.ForeColor = System.Drawing.Color.Black;
                                    numAdjustYAxisOffsetRack5Aspiration.Enabled = true;
                                    numAdjustYAxisOffsetRack5Aspiration.ForeColor = System.Drawing.Color.OrangeRed;
                                    numAdjustThetaAxisOffsetRack5Aspiration.Enabled = true;
                                    numAdjustThetaAxisOffsetRack5Aspiration.ForeColor = System.Drawing.Color.OrangeRed;
                                    break;
                            }

                            break;
                    }
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
            }
        }

        /// <summary>
        /// パラメータをすべて非活性にする
        /// </summary>
        private void ParametersAllFalse()
        {
            numtestTipNumber.Enabled = false;
            gbxtestFractionationPosition.Enabled = false;
            rbttestSTAT.Enabled = false;
            rbttestExternalTransport.Enabled = false;
            rbttestDilutionPosition.Enabled = false;
            rbttestPretreatmentPosition.Enabled = false;
            rbttestFront1.Enabled = false;
            rbttestFront2.Enabled = false;
            rbttestFront3.Enabled = false;
            rbttestFront4.Enabled = false;
            rbttestFront5.Enabled = false;
            rbttestBack1.Enabled = false;
            rbttestBack2.Enabled = false;
            rbttestBack3.Enabled = false;
            rbttestBack4.Enabled = false;
            rbttestBack5.Enabled = false;
            numtestReactionCellVolume.Enabled = false;
            numtestTestNo.Enabled = false;
        }

        /// <summary>
        /// 選択された機能番号に応じてパラメータを活性化する
        /// </summary>
        private void lbxtestSequenceListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParametersAllFalse();
            switch (lbxtestSequenceListBox.SelectedValue)
            {
                case (int)SampleDispenseSequence.RackDispensing:
                    numtestTipNumber.Enabled = true;
                    gbxtestFractionationPosition.Enabled = true;
                    rbttestFront1.Enabled = true;
                    rbttestFront2.Enabled = true;
                    rbttestFront3.Enabled = true;
                    rbttestFront4.Enabled = true;
                    rbttestFront5.Enabled = true;
                    rbttestBack1.Enabled = true;
                    rbttestBack2.Enabled = true;
                    rbttestBack3.Enabled = true;
                    rbttestBack4.Enabled = true;
                    rbttestBack5.Enabled = true;
                    numtestReactionCellVolume.Enabled = true;

                    rbttestFront1.Checked = true;
                    break;
                case (int)SampleDispenseSequence.STATorExternalTransferDispensing:
                    numtestTipNumber.Enabled = true;
                    gbxtestFractionationPosition.Enabled = true;
                    rbttestSTAT.Enabled = true;
                    rbttestExternalTransport.Enabled = true;
                    numtestReactionCellVolume.Enabled = true;

                    rbttestSTAT.Checked = true;
                    break;
                case (int)SampleDispenseSequence.ReactionTableDispensing:
                    numtestTipNumber.Enabled = true;
                    gbxtestFractionationPosition.Enabled = true;
                    rbttestDilutionPosition.Enabled = true;
                    rbttestPretreatmentPosition.Enabled = true;
                    numtestReactionCellVolume.Enabled = true;

                    rbttestDilutionPosition.Checked = true;
                    break;
                case (int)SampleDispenseSequence.RemovesSampleTip:
                    numtestTipNumber.Enabled = true;
                    break;
                case (int)SampleDispenseSequence.DetectionSensor:
                    numtestTipNumber.Enabled = true;
                    gbxtestFractionationPosition.Enabled = true;
                    rbttestSTAT.Enabled = true;
                    rbttestExternalTransport.Enabled = true;
                    rbttestDilutionPosition.Enabled = true;
                    rbttestPretreatmentPosition.Enabled = true;
                    rbttestFront1.Enabled = true;
                    rbttestFront2.Enabled = true;
                    rbttestFront3.Enabled = true;
                    rbttestFront4.Enabled = true;
                    rbttestFront5.Enabled = true;
                    rbttestBack1.Enabled = true;
                    rbttestBack2.Enabled = true;
                    rbttestBack3.Enabled = true;
                    rbttestBack4.Enabled = true;
                    rbttestBack5.Enabled = true;

                    rbttestFront1.Checked = true;

                    numtestTestNo.Enabled = true;
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
            //デリゲートでCall
            ToolbarsDisp(tabUnit.SelectedTab.Index, toolBarEnablekind);
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

            CarisXConfigParameter.SampleDispenseConfigParam ConfigParam = new CarisXConfigParameter.SampleDispenseConfigParam();
            //サンプル分注シリンジ待機位置
            ConfigParam.DispenceCyringeStandbyPosition = (int)numconfDespenseCyStandbyPosition.Value;
            //サンプル余剰量
            ConfigParam.SmpSurplusVolume = (int)numconfSmpSurplusVolume.Value;
            //サンプル分注量
            ConfigParam.SmpDispenseVolume = (int)numconfSmpDispenseVolume.Value;
            //サンプル吸引後待ち時間
            ConfigParam.WaitTimeAfterSuckingUpForSample = (double)numconfWaitTimeAfterSuckingUpForSample.Value;
            //検体容器の種類
            ConfigParam.ContainerKind = (SlaveCommCommand_0451.SampleContainerKind)lbxconfContainerKind.SelectedIndex + 1;
            //サンプルの種類
            ConfigParam.SampleKind = (SlaveCommCommand_0451.SmpKind)lbxconfSampleKind.SelectedIndex + 1;

            //液面センサー使用有無
            if (rbtLiquidLevelSensorUSE.Checked)
                ConfigParam.LiquidLevelSensor = 1;
            else
                ConfigParam.LiquidLevelSensor = 0;

            //泡がみチェック使用有無
            if (rbtBubbleCheckUSE.Checked)
                ConfigParam.BubbleCheck = 1;
            else
                ConfigParam.BubbleCheck = 0;
            //圧力差上限値1
            ConfigParam.UpperOfPress1 = (int)numconfUpperOfPresa1.Value;
            //圧力差上限値2
            ConfigParam.UpperOfPress2 = (int)numconfUpperOfPresa2.Value;
            //吸引エラー閾値１上限（血清用）
            ConfigParam.UpperOfSuckingUpErr1ForSerum = (int)numconfUpperOfSuckingUpErr1ForSerum.Value;
            //吸引エラー閾値１下限（血清用）
            ConfigParam.LowerOfSuckingUpErr1ForSerum = (int)numconfLowerOfSuckingUpErr1ForSerum.Value;
            //吸引エラー閾値１上限（尿用）
            ConfigParam.UpperOfSuckingUpErr1ForUrine = (int)numconfUpperOfSuckingUpErr1ForUrine.Value;
            //吸引エラー閾値１下限（尿用）
            ConfigParam.LowerOfSuckingUpErr1ForUrine = (int)numconfLowerOfSuckingUpErr1ForUrine.Value;

            //リークエラー液量下限値(5)		= /* リークエラー検出時の液量下限値 */
            ConfigParam.LowerOfLeakErrFluidVolume = (int)numconfLowerOfLeakErrFluidVolume.Value;
            //リークエラー圧力下限値(5)		= /* リークエラーを検出する圧力下限値 */
            ConfigParam.LowerOfLeakErrPress = (int)numconfLowerOfLeakErrPress.Value;
            //吐出不良閾値(5)			=／＊吐出不良閾値　＊／
            ConfigParam.ThresholdDefectiveDischarge = (int)numconfThresholdDefectiveDischarge.Value;
            //前処理検体分注量
            ConfigParam.PreSampleDispenseVolume = (int)numconfPreSampleDispenseVolume.Value;

            //圧力差上限値1（>100uL）
            ConfigParam.UpperOfPress1Over100 = (int)numconfUpperOfPresa1Over100.Value;
            //圧力差上限値2（>100uL）
            ConfigParam.UpperOfPress2Over100 = (int)numconfUpperOfPresa2Over100.Value;
            //吸引エラー閾値１上限（血清用）（>100uL）
            ConfigParam.UpperOfSuckingUpErr1ForSerumOver100 = (int)numconfUpperOfSuckingUpErr1ForSerumOver100.Value;
            //吸引エラー閾値１下限（血清用）（>100uL）
            ConfigParam.LowerOfSuckingUpErr1ForSerumOver100 = (int)numconfLowerOfSuckingUpErr1ForSerumOver100.Value;
            //吸引エラー閾値１上限（尿用）（>100uL）
            ConfigParam.UpperOfSuckingUpErr1ForUrineOver100 = (int)numconfUpperOfSuckingUpErr1ForUrineOver100.Value;
            //吸引エラー閾値１下限（尿用）（>100uL）
            ConfigParam.LowerOfSuckingUpErr1ForUrineOver100 = (int)numconfLowerOfSuckingUpErr1ForUrineOver100.Value;
            //リークエラー液量下限値(5)（>100uL）
            ConfigParam.LowerOfLeakErrFluidVolumeOver100 = (int)numconfLowerOfLeakErrFluidVolumeOver100.Value;
            //リークエラー圧力下限値(5)（>100uL）
            ConfigParam.LowerOfLeakErrPressOver100 = (int)numconfLowerOfLeakErrPressOver100.Value;
            //吐出不良閾値(5)（>100uL）
            ConfigParam.ThresholdDefectiveDischargeOver100 = (int)numconfThresholdDefectiveDischargeOver100.Value;

            config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam = ConfigParam;

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

            //サンプル分注シリンジ待機位置
            numconfDespenseCyStandbyPosition.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.DispenceCyringeStandbyPosition;
            //サンプル余剰量
            numconfSmpSurplusVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.SmpSurplusVolume;
            //サンプル分注量
            numconfSmpDispenseVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.SmpDispenseVolume;
            //サンプル吸引後待ち時間
            numconfWaitTimeAfterSuckingUpForSample.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.WaitTimeAfterSuckingUpForSample;
            //検体容器の種類
            if ((int)config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.ContainerKind != 0)
                lbxconfContainerKind.SelectedIndex = (int)config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.ContainerKind - 1;
            //サンプルの種類
            if ((int)config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.SampleKind != 0)
                lbxconfSampleKind.SelectedIndex = (int)config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.SampleKind - 1;

            //液面センサー使用有無
            if (config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.LiquidLevelSensor == 1)
                rbtLiquidLevelSensorUSE.Checked = true;
            else
                rbtLiquidLevelSensorNOUSE.Checked = true;

            //泡がみチェック使用有無
            if (config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.BubbleCheck == 1)
                rbtBubbleCheckUSE.Checked = true;
            else
                rbtBubbleCheckNOUSE.Checked = true;

            //圧力差上限値1
            numconfUpperOfPresa1.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.UpperOfPress1;
            //圧力差上限値2
            numconfUpperOfPresa2.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.UpperOfPress2;
            //吸引エラー閾値１上限（血清用）
            numconfUpperOfSuckingUpErr1ForSerum.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.UpperOfSuckingUpErr1ForSerum;
            //吸引エラー閾値１下限（血清用）
            numconfLowerOfSuckingUpErr1ForSerum.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.LowerOfSuckingUpErr1ForSerum;
            //吸引エラー閾値１上限（尿用）
            numconfUpperOfSuckingUpErr1ForUrine.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.UpperOfSuckingUpErr1ForUrine;
            //吸引エラー閾値１下限（尿用）
            numconfLowerOfSuckingUpErr1ForUrine.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.LowerOfSuckingUpErr1ForUrine;

            //リークエラー液量下限値(5)		= /* リークエラー検出時の液量下限値 */
            numconfLowerOfLeakErrFluidVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.LowerOfLeakErrFluidVolume;
            //リークエラー圧力下限値(5)		= /* リークエラーを検出する圧力下限値 */
            numconfLowerOfLeakErrPress.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.LowerOfLeakErrPress;
            //吐出不良閾値(5)			=／＊吐出不良閾値　＊／
            numconfThresholdDefectiveDischarge.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.ThresholdDefectiveDischarge;
            //前処理検体分注量
            numconfPreSampleDispenseVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.PreSampleDispenseVolume;

            //圧力差上限値1（>100uL）
            numconfUpperOfPresa1Over100.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.UpperOfPress1Over100;
            //圧力差上限値2（>100uL）
            numconfUpperOfPresa2Over100.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.UpperOfPress2Over100;
            //吸引エラー閾値１上限（血清用）（>100uL）
            numconfUpperOfSuckingUpErr1ForSerumOver100.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.UpperOfSuckingUpErr1ForSerumOver100;
            //吸引エラー閾値１下限（血清用）（>100uL）
            numconfLowerOfSuckingUpErr1ForSerumOver100.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.LowerOfSuckingUpErr1ForSerumOver100;
            //吸引エラー閾値１上限（尿用）（>100uL）
            numconfUpperOfSuckingUpErr1ForUrineOver100.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.UpperOfSuckingUpErr1ForUrineOver100;
            //吸引エラー閾値１下限（尿用）（>100uL）
            numconfLowerOfSuckingUpErr1ForUrineOver100.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.LowerOfSuckingUpErr1ForUrineOver100;
            //リークエラー液量下限値(5)（>100uL）
            numconfLowerOfLeakErrFluidVolumeOver100.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.LowerOfLeakErrFluidVolumeOver100;
            //リークエラー圧力下限値(5)（>100uL）
            numconfLowerOfLeakErrPressOver100.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.LowerOfLeakErrPressOver100;
            //吐出不良閾値(5)（>100uL）
            numconfThresholdDefectiveDischargeOver100.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseConfigParam.ThresholdDefectiveDischargeOver100;

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

            //サンプル分注移送部Z軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.SampleDispenseArmYAxis;
            if (!adjustSave)
            {
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamYAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamYAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamYAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamYAxisConst.Value;

                Mparam.MotorOffset[0] = (double)numParamYAxisOffsetRack1Aspiration.Value;
                Mparam.MotorOffset[1] = (double)numParamYAxisOffsetRack5Aspiration.Value;
                Mparam.MotorOffset[2] = (double)numParamYAxisOffsetSTATAspiration.Value;
                Mparam.MotorOffset[3] = (double)numParamYAxisOffsetDiluentAspiration.Value;
                Mparam.MotorOffset[4] = (double)numParamYAxisOffsetPretreatAspiration.Value;
                Mparam.MotorOffset[5] = (double)numParamYAxisOffsetLineAspiration.Value;
                Mparam.MotorOffset[6] = (double)numParamYAxisOffsetSampleDispense.Value;
                Mparam.MotorOffset[7] = (double)numParamYAxisOffsetTipRemove.Value;
                Mparam.MotorOffset[8] = (double)numParamYAxisOffsetTip1Catch.Value;
                Mparam.MotorOffset[9] = (double)numParamYAxisOffsetTip6Catch.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                //送信用アイテムにセット
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustYAxisOffsetRack1Aspiration.Value;
                Mparam.MotorOffset[1] = (double)numAdjustYAxisOffsetRack5Aspiration.Value;
                Mparam.MotorOffset[2] = (double)numAdjustYAxisOffsetSTATAspiration.Value;
                Mparam.MotorOffset[3] = (double)numAdjustYAxisOffsetDiluentAspiration.Value;
                Mparam.MotorOffset[4] = (double)numAdjustYAxisOffsetPretreatAspiration.Value;
                Mparam.MotorOffset[5] = (double)numAdjustYAxisOffsetLineAspiration.Value;
                Mparam.MotorOffset[6] = (double)numAdjustYAxisOffsetSampleDispense.Value;
                Mparam.MotorOffset[7] = (double)numAdjustYAxisOffsetTipRemove.Value;
                Mparam.MotorOffset[8] = (double)numAdjustYAxisOffsetTip1Catch.Value;
                Mparam.MotorOffset[9] = (double)numAdjustYAxisOffsetTip6Catch.Value;
            }
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetRackSample1Aspiration = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetRackSample5Aspiration = Mparam.MotorOffset[1];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetSTATSampleAspiration = Mparam.MotorOffset[2];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetDiluentSampleAspiration = Mparam.MotorOffset[3];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetPretreatSampleAspiration = Mparam.MotorOffset[4];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetLineSampleAspiration = Mparam.MotorOffset[5];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetSampleDispense = Mparam.MotorOffset[6];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetSampleTipRemover = Mparam.MotorOffset[7];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetSampleTip1Catch = Mparam.MotorOffset[8];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetSampleTip6Catch = Mparam.MotorOffset[9];
            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);


            //サンプル分注アームZ軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.SampleDispenseArmZAxis;
            if (!adjustSave)
            {
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamZAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamZAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamZAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamZAxisConst.Value;

                Mparam.MotorSpeed[1].InitSpeed = (int)numParamZAxisLiqInit.Value;
                Mparam.MotorSpeed[1].TopSpeed = (int)numParamZAxisLiqTop.Value;
                Mparam.MotorSpeed[1].Accel = (int)numParamZAxisLiqAccelerator.Value;
                Mparam.MotorSpeed[1].ConstSpeed = (int)numParamZAxisLiqConst.Value;

                Mparam.MotorOffset[0] = (double)numParamZAxisOffsetRackAspiration.Value;
                Mparam.MotorOffset[1] = (double)numParamZAxisOffsetSTATAspiration.Value;
                Mparam.MotorOffset[2] = (double)numParamZAxisOffsetLineAspiration.Value;
                Mparam.MotorOffset[3] = (double)numParamZAxisOffsetDiluentAspiration.Value;
                Mparam.MotorOffset[4] = (double)numParamZAxisOffsetTipRemove.Value;
                Mparam.MotorOffset[5] = (double)numParamZAxisOffsetTipCatch.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                //送信用アイテムにセット
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustZAxisOffsetRackAspiration.Value;
                Mparam.MotorOffset[1] = (double)numAdjustZAxisOffsetSTATAspiration.Value;
                Mparam.MotorOffset[2] = (double)numAdjustZAxisOffsetLineAspiration.Value;
                Mparam.MotorOffset[3] = (double)numAdjustZAxisOffsetDiluentAspiration.Value;
                Mparam.MotorOffset[4] = (double)numAdjustZAxisOffsetTipRemove.Value;
                Mparam.MotorOffset[5] = (double)numAdjustZAxisOffsetTipCatch.Value;
            }
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.OffsetRackSampleAspiration = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.OffsetSTATSampleAspiration = Mparam.MotorOffset[1];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.OffsetLineSampleAspiration = Mparam.MotorOffset[2];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.OffsetSampleDispenseDilutePretreatAspiration = Mparam.MotorOffset[3];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.OffsetSampleTipRemover = Mparam.MotorOffset[4];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.OffsetSampleTipCatch = Mparam.MotorOffset[5];
            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);


            //サンプル分注アームθ軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.SampleDispenseArmThetaAxis;
            if (!adjustSave)
            {
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamThetaAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamThetaAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamThetaAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamThetaAxisConst.Value;

                Mparam.MotorOffset[0] = (double)numParamThetaAxisOffsetRack1Aspiration.Value;
                Mparam.MotorOffset[1] = (double)numParamThetaAxisOffsetRack5Aspiration.Value;
                Mparam.MotorOffset[2] = (double)numParamThetaAxisOffsetSTATAspiration.Value;
                Mparam.MotorOffset[3] = (double)numParamThetaAxisOffsetDiluentAspiration.Value;
                Mparam.MotorOffset[4] = (double)numParamThetaAxisOffsetPretreatAspiration.Value;
                Mparam.MotorOffset[5] = (double)numParamThetaAxisOffsetLineAspiration.Value;
                Mparam.MotorOffset[6] = (double)numParamThetaAxisOffsetSampleDispense.Value;
                Mparam.MotorOffset[7] = (double)numParamThetaAxisOffsetTipRemove.Value;
                Mparam.MotorOffset[8] = (double)numParamThetaAxisOffsetTip1Catch.Value;
                Mparam.MotorOffset[9] = (double)numParamThetaAxisOffsetTip6Catch.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                //送信用アイテムにセット
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustThetaAxisOffsetRack1Aspiration.Value;
                Mparam.MotorOffset[1] = (double)numAdjustThetaAxisOffsetRack5Aspiration.Value;
                Mparam.MotorOffset[2] = (double)numAdjustThetaAxisOffsetSTATAspiration.Value;
                Mparam.MotorOffset[3] = (double)numAdjustThetaAxisOffsetDiluentAspiration.Value;
                Mparam.MotorOffset[4] = (double)numAdjustThetaAxisOffsetPretreatAspiration.Value;
                Mparam.MotorOffset[5] = (double)numAdjustThetaAxisOffsetLineAspiration.Value;
                Mparam.MotorOffset[6] = (double)numAdjustThetaAxisOffsetSampleDispense.Value;
                Mparam.MotorOffset[7] = (double)numAdjustThetaAxisOffsetTipRemove.Value;
                Mparam.MotorOffset[8] = (double)numAdjustThetaAxisOffsetTip1Catch.Value;
                Mparam.MotorOffset[9] = (double)numAdjustThetaAxisOffsetTip6Catch.Value;
            }
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetRackSample1Aspiration = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetRackSample5Aspiration = Mparam.MotorOffset[1];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetSTATSampleAspiration = Mparam.MotorOffset[2];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetDiluentSampleAspiration = Mparam.MotorOffset[3];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetPretreatSampleAspiration = Mparam.MotorOffset[4];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetLineSampleAspiration = Mparam.MotorOffset[5];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetSampleDispense = Mparam.MotorOffset[6];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetSampleTipRemover = Mparam.MotorOffset[7];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetSampleTip1Catch = Mparam.MotorOffset[8];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetSampleTip6Catch = Mparam.MotorOffset[9];

            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);


            if (!adjustSave)
            {
                //サンプル分注シリンジ
                Mparam = new SlaveCommCommand_0471();
                Mparam.MotorNo = (int)MotorNoList.SampleDispenseSyringe;

                Mparam.MotorSpeed[0].InitSpeed = (int)numParamSyringeLiqInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamSyringeLiqTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamSyringeLiqAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamSyringeLiqConst.Value;

                Mparam.MotorSpeed[1].InitSpeed = (int)numParamSyringeAspInit.Value;
                Mparam.MotorSpeed[1].TopSpeed = (int)numParamSyringeAspTop.Value;
                Mparam.MotorSpeed[1].Accel = (int)numParamSyringeAspAccelerator.Value;
                Mparam.MotorSpeed[1].ConstSpeed = (int)numParamSyringeAspConst.Value;

                Mparam.MotorSpeed[2].InitSpeed = (int)numParamSyringeDisInit.Value;
                Mparam.MotorSpeed[2].TopSpeed = (int)numParamSyringeDisTop.Value;
                Mparam.MotorSpeed[2].Accel = (int)numParamSyringeDisAccelerator.Value;
                Mparam.MotorSpeed[2].ConstSpeed = (int)numParamSyringeDisConst.Value;

                Mparam.MotorSpeed[3].InitSpeed = (int)numParamSyringeAirInit.Value;
                Mparam.MotorSpeed[3].TopSpeed = (int)numParamSyringeAirTop.Value;
                Mparam.MotorSpeed[3].Accel = (int)numParamSyringeAirAccelerator.Value;
                Mparam.MotorSpeed[3].ConstSpeed = (int)numParamSyringeAirConst.Value;

                Mparam.MotorSpeed[4].InitSpeed = (int)numParamSyringeAspUp50Init.Value;
                Mparam.MotorSpeed[4].TopSpeed = (int)numParamSyringeAspUp50Top.Value;
                Mparam.MotorSpeed[4].Accel = (int)numParamSyringeAspUp50Accelerator.Value;
                Mparam.MotorSpeed[4].ConstSpeed = (int)numParamSyringeAspUp50Const.Value;

                Mparam.MotorSpeed[5].InitSpeed = (int)numParamSyringeDisUp50Init.Value;
                Mparam.MotorSpeed[5].TopSpeed = (int)numParamSyringeDisUp50Top.Value;
                Mparam.MotorSpeed[5].Accel = (int)numParamSyringeDisUp50Accelerator.Value;
                Mparam.MotorSpeed[5].ConstSpeed = (int)numParamSyringeDisUp50Const.Value;

                Mparam.MotorOffset[0] = (double)numParamSyringeGain.Value;
                Mparam.MotorOffset[1] = (double)numParamSyringeOffset.Value;
                Mparam.MotorOffset[2] = (double)numParamSyringeGainOver100.Value;
                Mparam.MotorOffset[3] = (double)numParamSyringeOffsetOver100.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed = Mparam.MotorSpeed;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.Gain = Mparam.MotorOffset[0];
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.Offset = Mparam.MotorOffset[1];
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.GainOver100 = Mparam.MotorOffset[2];
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.OffsetOver100 = Mparam.MotorOffset[3];

                //モータパラメータ送信内容スプール
                SpoolParam(Mparam);
            }

            if (adjustSave)
            {

                //ケース搬送部Y軸
                Mparam = new SlaveCommCommand_0471();
                Mparam.MotorNo = (int)MotorNoList.CaseTransferYAxis;
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustCaseYAxisOffsetCaseCatchRelease.Value;
                Mparam.MotorOffset[1] = (double)numAdjustCaseYAxisOffsetReactionCellCatch.Value;
                Mparam.MotorOffset[2] = (double)numAdjustCaseYAxisOffsetTipCatch.Value;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.OffsetCaseCatchRelease = Mparam.MotorOffset[0];
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.OffsetReactionCellCatch = Mparam.MotorOffset[1];
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.OffsetSamplingTipCatch = Mparam.MotorOffset[2];
                //モータパラメータ送信内容スプール
                SpoolParam(Mparam);

                //STAT部Y軸
                Mparam = new SlaveCommCommand_0471();
                Mparam.MotorNo = (int)MotorNoList.STATYAxis;
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sTATYAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustSTATYAxisOffsetSTATAspiration.Value;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sTATYAxisParam.OffsetSTATSampleAspiration = Mparam.MotorOffset[0];
                //モータパラメータ送信内容スプール
                SpoolParam(Mparam);
            }

            //モーターパラメータ送信
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
                                formMaintenanceMain.subFormCaseTransfer[formMaintenanceMain.moduleIndex].MotorParamDisp();
                                formMaintenanceMain.subFormSTAT[formMaintenanceMain.moduleIndex].MotorParamDisp();
                                formMaintenanceMain.subFormReactionCellTransfer[formMaintenanceMain.moduleIndex].MotorParamDisp();
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
                    ToolbarsControl((int)ToolBarEnable.Adjust);
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

            //サンプル分注移送部Y軸
            numParamYAxisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.MotorSpeed[0].InitSpeed;
            numParamYAxisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.MotorSpeed[0].TopSpeed;
            numParamYAxisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.MotorSpeed[0].Accel;
            numParamYAxisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.MotorSpeed[0].ConstSpeed;
            numParamYAxisOffsetRack1Aspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetRackSample1Aspiration;
            numParamYAxisOffsetRack5Aspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetRackSample5Aspiration;
            numParamYAxisOffsetSTATAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetSTATSampleAspiration;
            numParamYAxisOffsetDiluentAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetDiluentSampleAspiration;
            numParamYAxisOffsetPretreatAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetPretreatSampleAspiration;
            numParamYAxisOffsetLineAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetLineSampleAspiration;
            numParamYAxisOffsetSampleDispense.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetSampleDispense;
            numParamYAxisOffsetTipRemove.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetSampleTipRemover;
            numParamYAxisOffsetTip1Catch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetSampleTip1Catch;
            numParamYAxisOffsetTip6Catch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetSampleTip6Catch;
            //オフセット（モーター調整用）
            numAdjustYAxisOffsetRack1Aspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetRackSample1Aspiration;
            numAdjustYAxisOffsetRack5Aspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetRackSample5Aspiration;
            numAdjustYAxisOffsetSTATAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetSTATSampleAspiration;
            numAdjustYAxisOffsetDiluentAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetDiluentSampleAspiration;
            numAdjustYAxisOffsetPretreatAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetPretreatSampleAspiration;
            numAdjustYAxisOffsetLineAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetLineSampleAspiration;
            numAdjustYAxisOffsetSampleDispense.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetSampleDispense;
            numAdjustYAxisOffsetTipRemove.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetSampleTipRemover;
            numAdjustYAxisOffsetTip1Catch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetSampleTip1Catch;
            numAdjustYAxisOffsetTip6Catch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmYAxisParam.OffsetSampleTip6Catch;

            //サンプル分注移送部Z軸
            numParamZAxisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.MotorSpeed[0].InitSpeed;
            numParamZAxisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.MotorSpeed[0].TopSpeed;
            numParamZAxisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.MotorSpeed[0].Accel;
            numParamZAxisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.MotorSpeed[0].ConstSpeed;
            numParamZAxisLiqInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.MotorSpeed[1].InitSpeed;
            numParamZAxisLiqTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.MotorSpeed[1].TopSpeed;
            numParamZAxisLiqAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.MotorSpeed[1].Accel;
            numParamZAxisLiqConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.MotorSpeed[1].ConstSpeed;
            numParamZAxisOffsetRackAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.OffsetRackSampleAspiration;
            numParamZAxisOffsetSTATAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.OffsetSTATSampleAspiration;
            numParamZAxisOffsetLineAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.OffsetLineSampleAspiration;
            numParamZAxisOffsetDiluentAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.OffsetSampleDispenseDilutePretreatAspiration;
            numParamZAxisOffsetTipRemove.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.OffsetSampleTipRemover;
            numParamZAxisOffsetTipCatch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.OffsetSampleTipCatch;
            //オフセット（モーター調整用）
            numAdjustZAxisOffsetRackAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.OffsetRackSampleAspiration;
            numAdjustZAxisOffsetSTATAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.OffsetSTATSampleAspiration;
            numAdjustZAxisOffsetLineAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.OffsetLineSampleAspiration;
            numAdjustZAxisOffsetDiluentAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.OffsetSampleDispenseDilutePretreatAspiration;
            numAdjustZAxisOffsetTipRemove.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.OffsetSampleTipRemover;
            numAdjustZAxisOffsetTipCatch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmZAxisParam.OffsetSampleTipCatch;

            //サンプル分注移送部θ軸
            numParamThetaAxisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.MotorSpeed[0].InitSpeed;
            numParamThetaAxisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.MotorSpeed[0].TopSpeed;
            numParamThetaAxisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.MotorSpeed[0].Accel;
            numParamThetaAxisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.MotorSpeed[0].ConstSpeed;
            numParamThetaAxisOffsetRack1Aspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetRackSample1Aspiration;
            numParamThetaAxisOffsetRack5Aspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetRackSample5Aspiration;
            numParamThetaAxisOffsetSTATAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetSTATSampleAspiration;
            numParamThetaAxisOffsetDiluentAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetDiluentSampleAspiration;
            numParamThetaAxisOffsetPretreatAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetPretreatSampleAspiration;
            numParamThetaAxisOffsetLineAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetLineSampleAspiration;
            numParamThetaAxisOffsetSampleDispense.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetSampleDispense;
            numParamThetaAxisOffsetTipRemove.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetSampleTipRemover;
            numParamThetaAxisOffsetTip1Catch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetSampleTip1Catch;
            numParamThetaAxisOffsetTip6Catch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetSampleTip6Catch;
            //オフセット（モーター調整用）
            numAdjustThetaAxisOffsetRack1Aspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetRackSample1Aspiration;
            numAdjustThetaAxisOffsetRack5Aspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetRackSample5Aspiration;
            numAdjustThetaAxisOffsetSTATAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetSTATSampleAspiration;
            numAdjustThetaAxisOffsetDiluentAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetDiluentSampleAspiration;
            numAdjustThetaAxisOffsetPretreatAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetPretreatSampleAspiration;
            numAdjustThetaAxisOffsetLineAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetLineSampleAspiration;
            numAdjustThetaAxisOffsetSampleDispense.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetSampleDispense;
            numAdjustThetaAxisOffsetTipRemove.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetSampleTipRemover;
            numAdjustThetaAxisOffsetTip1Catch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetSampleTip1Catch;
            numAdjustThetaAxisOffsetTip6Catch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseArmThetaAxisParam.OffsetSampleTip6Catch;

            //サンプル分注シリンジ
            numParamSyringeLiqInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[0].InitSpeed;
            numParamSyringeLiqTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[0].TopSpeed;
            numParamSyringeLiqAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[0].Accel;
            numParamSyringeLiqConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[0].ConstSpeed;

            numParamSyringeAspInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[1].InitSpeed;
            numParamSyringeAspTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[1].TopSpeed;
            numParamSyringeAspAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[1].Accel;
            numParamSyringeAspConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[1].ConstSpeed;

            numParamSyringeDisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[2].InitSpeed;
            numParamSyringeDisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[2].TopSpeed;
            numParamSyringeDisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[2].Accel;
            numParamSyringeDisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[2].ConstSpeed;

            numParamSyringeAirInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[3].InitSpeed;
            numParamSyringeAirTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[3].TopSpeed;
            numParamSyringeAirAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[3].Accel;
            numParamSyringeAirConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[3].ConstSpeed;

            numParamSyringeAspUp50Init.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[4].InitSpeed;
            numParamSyringeAspUp50Top.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[4].TopSpeed;
            numParamSyringeAspUp50Accelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[4].Accel;
            numParamSyringeAspUp50Const.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[4].ConstSpeed;

            numParamSyringeDisUp50Init.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[5].InitSpeed;
            numParamSyringeDisUp50Top.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[5].TopSpeed;
            numParamSyringeDisUp50Accelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[5].Accel;
            numParamSyringeDisUp50Const.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.MotorSpeed[5].ConstSpeed;

            numParamSyringeGain.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.Gain;
            numParamSyringeOffset.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.Offset;
            numParamSyringeGainOver100.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.GainOver100;
            numParamSyringeOffsetOver100.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sampleDispenseSyringeParam.OffsetOver100;

            //ケース搬送
            numAdjustCaseYAxisOffsetCaseCatchRelease.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.OffsetCaseCatchRelease;
            numAdjustCaseYAxisOffsetReactionCellCatch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.OffsetReactionCellCatch;
            numAdjustCaseYAxisOffsetTipCatch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.OffsetSamplingTipCatch;

            //STAT
            numAdjustSTATYAxisOffsetSTATAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sTATYAxisParam.OffsetSTATSampleAspiration;

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

            cmbAdjustTipNo.Enabled = false;
            cmbAdjustCoarseFine.Enabled = false;
            cmbAdjustSampleTubeType.Enabled = false;
            cmbAdjustPortNo.Enabled = false;
            cmbAdjustSamplePosition.Enabled = false;

            gbxAdjustYAxis.Enabled = false;
            numAdjustYAxisOffsetRack1Aspiration.Enabled = false;
            numAdjustYAxisOffsetRack5Aspiration.Enabled = false;
            numAdjustYAxisOffsetSTATAspiration.Enabled = false;
            numAdjustYAxisOffsetDiluentAspiration.Enabled = false;
            numAdjustYAxisOffsetPretreatAspiration.Enabled = false;
            numAdjustYAxisOffsetLineAspiration.Enabled = false;
            numAdjustYAxisOffsetSampleDispense.Enabled = false;
            numAdjustYAxisOffsetTipRemove.Enabled = false;
            numAdjustYAxisOffsetTip1Catch.Enabled = false;
            numAdjustYAxisOffsetTip6Catch.Enabled = false;

            gbxAdjustZAxis.Enabled = false;
            numAdjustZAxisOffsetRackAspiration.Enabled = false;
            numAdjustZAxisOffsetSTATAspiration.Enabled = false;
            numAdjustZAxisOffsetLineAspiration.Enabled = false;
            numAdjustZAxisOffsetDiluentAspiration.Enabled = false;
            numAdjustZAxisOffsetTipRemove.Enabled = false;
            numAdjustZAxisOffsetTipCatch.Enabled = false;

            gbxAdjustThetaAxis.Enabled = false;
            numAdjustThetaAxisOffsetRack1Aspiration.Enabled = false;
            numAdjustThetaAxisOffsetRack5Aspiration.Enabled = false;
            numAdjustThetaAxisOffsetSTATAspiration.Enabled = false;
            numAdjustThetaAxisOffsetDiluentAspiration.Enabled = false;
            numAdjustThetaAxisOffsetPretreatAspiration.Enabled = false;
            numAdjustThetaAxisOffsetLineAspiration.Enabled = false;
            numAdjustThetaAxisOffsetSampleDispense.Enabled = false;
            numAdjustThetaAxisOffsetTipRemove.Enabled = false;
            numAdjustThetaAxisOffsetTip1Catch.Enabled = false;
            numAdjustThetaAxisOffsetTip6Catch.Enabled = false;

            gbxAdjustCaseYAxis.Enabled = false;
            numAdjustCaseYAxisOffsetCaseCatchRelease.Enabled = false;
            numAdjustCaseYAxisOffsetReactionCellCatch.Enabled = false;
            numAdjustCaseYAxisOffsetTipCatch.Enabled = false;

            gbxAdjustSTATYAxis.Enabled = false;
            numAdjustSTATYAxisOffsetSTATAspiration.Enabled = false;

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

            SlaveCommCommand_0480 AdjustStartComm = new SlaveCommCommand_0480();

            Func<int> getStopPos = () => { return (int)lbxAdjustSequence.SelectedValue; };
            Func<int> tipno = () => { return (int)cmbAdjustTipNo.Value; };
            Func<int> coarsefine = () => { return (int)cmbAdjustCoarseFine.Value; };
            Func<int> sampletubetype = () => { return (int)cmbAdjustSampleTubeType.Value; };
            Func<int> portno = () => { return (int)cmbAdjustPortNo.Value; };
            Func<int> rackpos = () => { return (int)cmbAdjustSamplePosition.Value; };

            AdjustStartComm.Pos = (int)Invoke(getStopPos);
            AdjustStartComm.Arg1 = (int)Invoke(tipno);
            switch (AdjustStartComm.Pos)
            {
                case (int)MotorAdjustStopPosition.SampleTipCatch:
                case (int)MotorAdjustStopPosition.SampleTipRemover:
                    AdjustStartComm.Arg2 = (int)Invoke(coarsefine);
                    break;
                case (int)MotorAdjustStopPosition.RackAspiration:
                    AdjustStartComm.Arg2 = (int)Invoke(sampletubetype);
                    AdjustStartComm.Arg3 = ((int)Invoke(portno) * 10) + (int)Invoke(rackpos);
                    break;
                case (int)MotorAdjustStopPosition.STATAspiration:
                case (int)MotorAdjustStopPosition.LineSampleAspiration:
                    AdjustStartComm.Arg2 = (int)Invoke(sampletubetype);
                    break;
                case (int)MotorAdjustStopPosition.DiluentAspiration:
                case (int)MotorAdjustStopPosition.PretreatAspiration:
                case (int)MotorAdjustStopPosition.SampleDispense:
                    break;
            }

            // 受信待ちフラグを設定
            ResponseFlg = true;

            // コマンド送信
            unitStartAdjust.Start(AdjustStartComm);

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

                    SlaveCommCommand_0481 AdjustStartComm = new SlaveCommCommand_0481();
                    Func<int> getStopPos = () => { return (int)lbxAdjustSequence.SelectedValue; };
                    AdjustStartComm.Pos = (int)Invoke(getStopPos);

                    unitAdjustAbort.Start(AdjustStartComm);
                    break;
            }

            return (true);
        }

        /// <summary>
        /// UP/DownボタンをEnable/Disableにする
        /// </summary>
        public override void UpDownButtonEnable(bool enable)
        {
            btnAdjustYAxisForward.Enabled = enable;
            btnAdjustYAxisBack.Enabled = enable;

            btnAdjustThetaAxisCW.Enabled = enable;
            btnAdjustThetaAxisCCW.Enabled = enable;

            btnAdjustZAxisDown.Enabled = enable;
            btnAdjustZAxisUp.Enabled = enable;

            btnAdjustCaseYAxisForward.Enabled = enable;
            btnAdjustCaseYAxisBack.Enabled = enable;

            btnAdjustSTATYAxisForward.Enabled = enable;
            btnAdjustSTATYAxisBack.Enabled = enable;
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
            switch (lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.SampleTipCatch:
                    cmbAdjustTipNo.Enabled = true;
                    cmbAdjustCoarseFine.Enabled = true;

                    gbxAdjustYAxis.Enabled = true;
                    numAdjustYAxisOffsetTip1Catch.Enabled = true;
                    numAdjustYAxisOffsetTip1Catch.ForeColor = System.Drawing.Color.OrangeRed;
                    numAdjustYAxisOffsetTip6Catch.Enabled = true;
                    numAdjustYAxisOffsetTip6Catch.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetTipCatch.Enabled = true;
                    numAdjustZAxisOffsetTipCatch.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustThetaAxis.Enabled = true;
                    numAdjustThetaAxisOffsetTip1Catch.Enabled = true;
                    numAdjustThetaAxisOffsetTip1Catch.ForeColor = System.Drawing.Color.OrangeRed;
                    numAdjustThetaAxisOffsetTip6Catch.Enabled = true;
                    numAdjustThetaAxisOffsetTip6Catch.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustCaseYAxis.Enabled = true;
                    numAdjustCaseYAxisOffsetTipCatch.Enabled = true;
                    numAdjustCaseYAxisOffsetTipCatch.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

                case (int)MotorAdjustStopPosition.RackAspiration:
                    cmbAdjustTipNo.SelectedIndex = 0;               //操作不可で1を選択した状態に変更する
                    cmbAdjustSampleTubeType.Enabled = true;
                    cmbAdjustPortNo.Enabled = true;
                    cmbAdjustSamplePosition.Enabled = true;

                    gbxAdjustYAxis.Enabled = true;
                    numAdjustYAxisOffsetRack1Aspiration.Enabled = true;
                    numAdjustYAxisOffsetRack1Aspiration.ForeColor = System.Drawing.Color.OrangeRed;
                    numAdjustYAxisOffsetRack5Aspiration.Enabled = true;
                    numAdjustYAxisOffsetRack5Aspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetRackAspiration.Enabled = true;
                    numAdjustZAxisOffsetRackAspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustThetaAxis.Enabled = true;
                    numAdjustThetaAxisOffsetRack1Aspiration.Enabled = true;
                    numAdjustThetaAxisOffsetRack1Aspiration.ForeColor = System.Drawing.Color.OrangeRed;
                    numAdjustThetaAxisOffsetRack5Aspiration.Enabled = true;
                    numAdjustThetaAxisOffsetRack5Aspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

                case (int)MotorAdjustStopPosition.STATAspiration:
                    cmbAdjustTipNo.SelectedIndex = 0;               //操作不可で1を選択した状態に変更する
                    cmbAdjustSampleTubeType.Enabled = true;

                    gbxAdjustYAxis.Enabled = true;
                    numAdjustYAxisOffsetSTATAspiration.Enabled = true;
                    numAdjustYAxisOffsetSTATAspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetSTATAspiration.Enabled = true;
                    numAdjustZAxisOffsetSTATAspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustThetaAxis.Enabled = true;
                    numAdjustThetaAxisOffsetSTATAspiration.Enabled = true;
                    numAdjustThetaAxisOffsetSTATAspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustSTATYAxis.Enabled = true;
                    numAdjustSTATYAxisOffsetSTATAspiration.Enabled = true;
                    numAdjustSTATYAxisOffsetSTATAspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

                case (int)MotorAdjustStopPosition.DiluentAspiration:
                    cmbAdjustTipNo.SelectedIndex = 0;               //操作不可で1を選択した状態に変更する

                    gbxAdjustYAxis.Enabled = true;
                    numAdjustYAxisOffsetDiluentAspiration.Enabled = true;
                    numAdjustYAxisOffsetDiluentAspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetDiluentAspiration.Enabled = true;
                    numAdjustZAxisOffsetDiluentAspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustThetaAxis.Enabled = true;
                    numAdjustThetaAxisOffsetDiluentAspiration.Enabled = true;
                    numAdjustThetaAxisOffsetDiluentAspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

                case (int)MotorAdjustStopPosition.PretreatAspiration:
                    cmbAdjustTipNo.SelectedIndex = 0;               //操作不可で1を選択した状態に変更する

                    gbxAdjustYAxis.Enabled = true;
                    numAdjustYAxisOffsetPretreatAspiration.Enabled = true;
                    numAdjustYAxisOffsetPretreatAspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetDiluentAspiration.Enabled = true;
                    numAdjustZAxisOffsetDiluentAspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustThetaAxis.Enabled = true;
                    numAdjustThetaAxisOffsetPretreatAspiration.Enabled = true;
                    numAdjustThetaAxisOffsetPretreatAspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

                case (int)MotorAdjustStopPosition.SampleDispense:
                    cmbAdjustTipNo.SelectedIndex = 0;               //操作不可で1を選択した状態に変更する

                    gbxAdjustYAxis.Enabled = true;
                    numAdjustYAxisOffsetSampleDispense.Enabled = true;
                    numAdjustYAxisOffsetSampleDispense.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetDiluentAspiration.Enabled = true;
                    numAdjustZAxisOffsetDiluentAspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustThetaAxis.Enabled = true;
                    numAdjustThetaAxisOffsetSampleDispense.Enabled = true;
                    numAdjustThetaAxisOffsetSampleDispense.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

                case (int)MotorAdjustStopPosition.SampleTipRemover:
                    cmbAdjustTipNo.SelectedIndex = 0;               //操作不可で1を選択した状態に変更する
                    cmbAdjustCoarseFine.Enabled = true;

                    gbxAdjustYAxis.Enabled = true;
                    numAdjustYAxisOffsetTipRemove.Enabled = true;
                    numAdjustYAxisOffsetTipRemove.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetTipRemove.Enabled = true;
                    numAdjustZAxisOffsetTipRemove.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustThetaAxis.Enabled = true;
                    numAdjustThetaAxisOffsetTipRemove.Enabled = true;
                    numAdjustThetaAxisOffsetTipRemove.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

                case (int)MotorAdjustStopPosition.LineSampleAspiration:
                    cmbAdjustTipNo.SelectedIndex = 0;               //操作不可で1を選択した状態に変更する
                    cmbAdjustSampleTubeType.Enabled = true;

                    gbxAdjustYAxis.Enabled = true;
                    numAdjustYAxisOffsetLineAspiration.Enabled = true;
                    numAdjustYAxisOffsetLineAspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetLineAspiration.Enabled = true;
                    numAdjustZAxisOffsetLineAspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustThetaAxis.Enabled = true;
                    numAdjustThetaAxisOffsetLineAspiration.Enabled = true;
                    numAdjustThetaAxisOffsetLineAspiration.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

            }
        }

        #region Pitch値の調整
        private void btnAdjustYAxisPitchUp_Click(object sender, EventArgs e)
        {
            numAdjustYAxisPitch.Value = (double)numAdjustYAxisPitch.Value + (double)numAdjustYAxisPitch.SpinIncrement;
        }

        private void btnAdjustYAxisPitchDown_Click(object sender, EventArgs e)
        {
            numAdjustYAxisPitch.Value = (double)numAdjustYAxisPitch.Value - (double)numAdjustYAxisPitch.SpinIncrement;
        }

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

        private void btnAdjustCaseYAxisPitchUp_Click(object sender, EventArgs e)
        {
            numAdjustCaseYAxisPitch.Value = (double)numAdjustCaseYAxisPitch.Value + (double)numAdjustCaseYAxisPitch.SpinIncrement;
        }

        private void btnAdjustCaseYAxisPitchDown_Click(object sender, EventArgs e)
        {
            numAdjustCaseYAxisPitch.Value = (double)numAdjustCaseYAxisPitch.Value - (double)numAdjustCaseYAxisPitch.SpinIncrement;
        }

        private void btnAdjustSTATYAxisPitchUp_Click(object sender, EventArgs e)
        {
            numAdjustSTATYAxisPitch.Value = (double)numAdjustSTATYAxisPitch.Value + (double)numAdjustSTATYAxisPitch.SpinIncrement;
        }

        private void btnAdjustSTATYAxisPitchDown_Click(object sender, EventArgs e)
        {
            numAdjustSTATYAxisPitch.Value = (double)numAdjustSTATYAxisPitch.Value - (double)numAdjustSTATYAxisPitch.SpinIncrement;
        }
        #endregion

        private void btnAdjustYAxisForwardBack_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustYAxisForward);
            Int32 motorNo = (int)MotorNoList.SampleDispenseArmYAxis;
            Double pitchValue = (double)numAdjustYAxisPitch.Value;

            switch ((int)lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.SampleTipCatch:
                    if (numAdjustYAxisOffsetTip1Catch.Enabled)
                        AdjustValue(upFlg, motorNo, numAdjustYAxisOffsetTip1Catch, pitchValue);
                    else
                        AdjustValue(upFlg, motorNo, numAdjustYAxisOffsetTip6Catch, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.RackAspiration:
                    if (numAdjustYAxisOffsetRack1Aspiration.Enabled)
                        AdjustValue(upFlg, motorNo, numAdjustYAxisOffsetRack1Aspiration, pitchValue);
                    else
                        AdjustValue(upFlg, motorNo, numAdjustYAxisOffsetRack5Aspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.STATAspiration:
                    AdjustValue(upFlg, motorNo, numAdjustYAxisOffsetSTATAspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.DiluentAspiration:
                    AdjustValue(upFlg, motorNo, numAdjustYAxisOffsetDiluentAspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.PretreatAspiration:
                    AdjustValue(upFlg, motorNo, numAdjustYAxisOffsetPretreatAspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.SampleDispense:
                    AdjustValue(upFlg, motorNo, numAdjustYAxisOffsetSampleDispense, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.SampleTipRemover:
                    AdjustValue(upFlg, motorNo, numAdjustYAxisOffsetTipRemove, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.LineSampleAspiration:
                    AdjustValue(upFlg, motorNo, numAdjustYAxisOffsetLineAspiration, pitchValue);
                    break;
            }
        }

        private void btnAdjustZAxisDownUp_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustZAxisDown);
            Int32 motorNo = (int)MotorNoList.SampleDispenseArmZAxis;
            Double pitchValue = (double)numAdjustZAxisPitch.Value;

            switch ((int)lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.SampleTipCatch:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetTipCatch, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.RackAspiration:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetRackAspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.STATAspiration:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetSTATAspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.DiluentAspiration:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetDiluentAspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.PretreatAspiration:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetDiluentAspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.SampleDispense:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetDiluentAspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.SampleTipRemover:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetTipRemove, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.LineSampleAspiration:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetLineAspiration, pitchValue);
                    break;
            }
        }

        private void btnAdjustThetaAxisCWCCW_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustThetaAxisCW);
            Int32 motorNo = (int)MotorNoList.SampleDispenseArmThetaAxis;
            Double pitchValue = (double)numAdjustThetaAxisPitch.Value;

            switch ((int)lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.SampleTipCatch:
                    if (numAdjustThetaAxisOffsetTip1Catch.Enabled)
                        AdjustValue(upFlg, motorNo, numAdjustThetaAxisOffsetTip1Catch, pitchValue);
                    else
                        AdjustValue(upFlg, motorNo, numAdjustThetaAxisOffsetTip6Catch, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.RackAspiration:
                    if (numAdjustThetaAxisOffsetRack1Aspiration.Enabled)
                        AdjustValue(upFlg, motorNo, numAdjustThetaAxisOffsetRack1Aspiration, pitchValue);
                    else
                        AdjustValue(upFlg, motorNo, numAdjustThetaAxisOffsetRack5Aspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.STATAspiration:
                    AdjustValue(upFlg, motorNo, numAdjustThetaAxisOffsetSTATAspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.DiluentAspiration:
                    AdjustValue(upFlg, motorNo, numAdjustThetaAxisOffsetDiluentAspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.PretreatAspiration:
                    AdjustValue(upFlg, motorNo, numAdjustThetaAxisOffsetPretreatAspiration, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.SampleDispense:
                    AdjustValue(upFlg, motorNo, numAdjustThetaAxisOffsetSampleDispense, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.SampleTipRemover:
                    AdjustValue(upFlg, motorNo, numAdjustThetaAxisOffsetTipRemove, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.LineSampleAspiration:
                    AdjustValue(upFlg, motorNo, numAdjustThetaAxisOffsetLineAspiration, pitchValue);
                    break;
            }
        }

        private void btnAdjustCaseYAxisForwardBack_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustCaseYAxisForward);
            Int32 motorNo = (int)MotorNoList.CaseTransferYAxis;
            Double pitchValue = (double)numAdjustCaseYAxisPitch.Value;

            AdjustValue(upFlg, motorNo, numAdjustCaseYAxisOffsetTipCatch, pitchValue);
        }

        private void btnAdjustSTATYAxisForwardBack_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustSTATYAxisBack);
            Int32 motorNo = (int)MotorNoList.STATYAxis;
            Double pitchValue = (double)numAdjustSTATYAxisPitch.Value;

            AdjustValue(upFlg, motorNo, numAdjustSTATYAxisOffsetSTATAspiration, pitchValue);
        }
    }

}
