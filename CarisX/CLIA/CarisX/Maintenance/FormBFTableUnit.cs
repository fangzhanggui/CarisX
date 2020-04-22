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
    /// BFテーブル部
    /// </summary>
    public partial class FormBFTableUnit : FormUnitBase
    {
        IMaintenanceUnitStart unitStart = new UnitStart();
        IMaintenanceUnitStart unitStartAdjust = new UnitStarAdjust();
        IMaintenanceUnitStart unitAdjustAbort = new UnitStarAdjustAbort();
        volatile bool ResponseFlg;
        public CommonFunction ComFunc = new CommonFunction();

        public FormBFTableUnit()
        {
            InitializeComponent();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ParametersAllFalse();
            ConfigParamLoad();
            MotorParamDisp();
            setCulture();
            ComFunc.SetControlSettings(this);
            ConfigTabUseFlg = tabUnit.Tabs[(int)MaintenanceTabIndex.Config].Enabled;    //Configタブを利用有無を退避

            lbxtestSequenceListBox.SelectedIndex = 0;
        }

        #region リソース設定
        private void setCulture()
        {
            //タブ
            tabUnit.Tabs[0].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_000;                     //Test
            tabUnit.Tabs[1].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_001;                     //Configuration
            tabUnit.Tabs[2].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_002;                     //Motor Parameters
            tabUnit.Tabs[3].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_MOTORADJUSTMENT;    //Adjust

            //Testタブ
            gbxtestSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_003;                 //Sequence
            object SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_004, No=(int)BFTableSequence.Init},       //1: Unit Initialization
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_005, No=(int)BFTableSequence.Step},       //2: 1 Step
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_006, No=(int)BFTableSequence.MixingR2},   //3: Mixing (R2)
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_007, No=(int)BFTableSequence.MixingBF1},  //4: Mixing (BF 1)
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_008, No=(int)BFTableSequence.MixingPTr},  //5: Mixing (PTr)
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_009, No=(int)BFTableSequence.MixingBF2},  //6: Mixing (BF 2)
            };
            lbxtestSequenceListBox = ComFunc.setSequenceListBox(lbxtestSequenceListBox, SequenceList);

            gbxtestParameters.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_010;
            lbltestRepeatFrequency.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_011;
            lbltestNumber.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_012;
            lbltestRepeatInterval.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_033;
            lbltestRepeatIntervalUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_034;
            lbltestMoveStep.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_032;

            gbxtestResponce.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_013;

            //Configurationタブ
            gbxconfParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_010;
            lblconfStrringTimeR2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_025;
            lblconfStrringTimeBF1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_041;
            lblconfStrringTimeBF2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_042;
            lblconfStrringTimePretrigger.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_043;
            lblconfStrringTimeR2Unit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_026;
            lblconfStrringTimeBF1Unit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_026;
            lblconfStrringTimeBF2Unit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_026;
            lblconfStrringTimePretriggerUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_026;

            //MotorParameterタブ
            //BFテーブル部θ軸
            gbxParamTableTheta.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_014;
            gbxParamTableThetaCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_027;
            gbxParamTableThetaAxisOffset.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_028;
            lblParamTableThetaInit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_019;
            lblParamTableThetaTop.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_020;
            lblParamTableThetaAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_021;
            lblParamTableThetaConst.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_022;
            lblParamTableThetaAxisOffsetHomePosition.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_029;
            lblParamTableThetaAxisOffsetEncodeThresh.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_040;
            lblParamTableThetaInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamTableThetaTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamTableThetaAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_024;
            lblParamTableThetaConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamTableThetaAxisOffsetHomePositionUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_030;
            lblParamTableThetaAxisOffsetEncodeThreshUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_030;

            //撹拌部　R2撹拌Zθ
            gbxParamMixingR2.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_015;
            gbxParamMixingR2A1stUp.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_035;
            lblParamMixingR2A1stUpInit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_019;
            lblParamMixingR2A1stUpTop.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_020;
            lblParamMixingR2A1stUpAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_021;
            lblParamMixingR2A1stUpConst.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_022;
            lblParamMixingR2A1stUpInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingR2A1stUpTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingR2A1stUpAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_024;
            lblParamMixingR2A1stUpConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;

            gbxParamMixingR2B12ndUp.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_036;
            lblParamMixingR2B12ndUpInit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_019;
            lblParamMixingR2B12ndUpTop.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_020;
            lblParamMixingR2B12ndUpAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_021;
            lblParamMixingR2B12ndUpConst.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_022;
            lblParamMixingR2B12ndUpInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingR2B12ndUpTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingR2B12ndUpAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_024;
            lblParamMixingR2B12ndUpConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;

            gbxParamMixingR2B2Down.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_037;
            lblParamMixingR2B2DownInit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_019;
            lblParamMixingR2B2DownTop.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_020;
            lblParamMixingR2B2DownAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_021;
            lblParamMixingR2B2DownConst.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_022;
            lblParamMixingR2B2DownInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingR2B2DownTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingR2B2DownAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_024;
            lblParamMixingR2B2DownConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;

            gbxParamMixingR2Reverse.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_038;
            lblParamMixingR2ReverseInit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_019;
            lblParamMixingR2ReverseTop.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_020;
            lblParamMixingR2ReverseAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_021;
            lblParamMixingR2ReverseConst.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_022;
            lblParamMixingR2ReverseInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingR2ReverseTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingR2ReverseAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_024;
            lblParamMixingR2ReverseConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;

            gbxParamMixingR2Offset.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_028;
            lblParamMixingR2OffsetAPos.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_039;
            lblParamMixingR2OffsetAPosUnit.Text = Resources_Maintenance.BLANK;

            //撹拌部　BF1撹拌Zθ
            gbxParamMixingBF1.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_016;
            gbxParamMixingBF1A1stUp.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_035;
            lblParamMixingBF1A1stUpInit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_019;
            lblParamMixingBF1A1stUpTop.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_020;
            lblParamMixingBF1A1stUpAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_021;
            lblParamMixingBF1A1stUpConst.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_022;
            lblParamMixingBF1A1stUpInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingBF1A1stUpTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingBF1A1stUpAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_024;
            lblParamMixingBF1A1stUpConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;

            gbxParamMixingBF1B12ndUp.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_036;
            lblParamMixingBF1B12ndUpInit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_019;
            lblParamMixingBF1B12ndUpTop.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_020;
            lblParamMixingBF1B12ndUpAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_021;
            lblParamMixingBF1B12ndUpConst.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_022;
            lblParamMixingBF1B12ndUpInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingBF1B12ndUpTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingBF1B12ndUpAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_024;
            lblParamMixingBF1B12ndUpConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;

            gbxParamMixingBF1B2Down.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_037;
            lblParamMixingBF1B2DownInit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_019;
            lblParamMixingBF1B2DownTop.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_020;
            lblParamMixingBF1B2DownAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_021;
            lblParamMixingBF1B2DownConst.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_022;
            lblParamMixingBF1B2DownInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingBF1B2DownTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingBF1B2DownAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_024;
            lblParamMixingBF1B2DownConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;

            gbxParamMixingBF1Reverse.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_038;
            lblParamMixingBF1ReverseInit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_019;
            lblParamMixingBF1ReverseTop.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_020;
            lblParamMixingBF1ReverseAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_021;
            lblParamMixingBF1ReverseConst.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_022;
            lblParamMixingBF1ReverseInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingBF1ReverseTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingBF1ReverseAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_024;
            lblParamMixingBF1ReverseConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;

            gbxParamMixingBF1Offset.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_028;
            lblParamMixingBF1OffsetAPos.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_039;
            lblParamMixingBF1OffsetAPosUnit.Text = Resources_Maintenance.BLANK;

            //撹拌部　BF2撹拌Zθ
            gbxParamMixingBF2.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_017;
            gbxParamMixingBF2A1stUp.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_035;
            lblParamMixingBF2A1stUpInit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_019;
            lblParamMixingBF2A1stUpTop.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_020;
            lblParamMixingBF2A1stUpAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_021;
            lblParamMixingBF2A1stUpConst.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_022;
            lblParamMixingBF2A1stUpInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingBF2A1stUpTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingBF2A1stUpAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_024;
            lblParamMixingBF2A1stUpConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;

            gbxParamMixingBF2B12ndUp.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_036;
            lblParamMixingBF2B12ndUpInit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_019;
            lblParamMixingBF2B12ndUpTop.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_020;
            lblParamMixingBF2B12ndUpAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_021;
            lblParamMixingBF2B12ndUpConst.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_022;
            lblParamMixingBF2B12ndUpInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingBF2B12ndUpTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingBF2B12ndUpAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_024;
            lblParamMixingBF2B12ndUpConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;

            gbxParamMixingBF2B2Down.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_037;
            lblParamMixingBF2B2DownInit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_019;
            lblParamMixingBF2B2DownTop.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_020;
            lblParamMixingBF2B2DownAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_021;
            lblParamMixingBF2B2DownConst.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_022;
            lblParamMixingBF2B2DownInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingBF2B2DownTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingBF2B2DownAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_024;
            lblParamMixingBF2B2DownConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;

            gbxParamMixingBF2Reverse.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_038;
            lblParamMixingBF2ReverseInit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_019;
            lblParamMixingBF2ReverseTop.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_020;
            lblParamMixingBF2ReverseAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_021;
            lblParamMixingBF2ReverseConst.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_022;
            lblParamMixingBF2ReverseInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingBF2ReverseTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingBF2ReverseAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_024;
            lblParamMixingBF2ReverseConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;

            gbxParamMixingBF2Offset.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_028;
            lblParamMixingBF2OffsetAPos.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_039;
            lblParamMixingBF2OffsetAPosUnit.Text = Resources_Maintenance.BLANK;

            //撹拌部　Pre-Trigger撹拌Zθ
            gbxParamMixingPTr.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_018;
            gbxParamMixingPTrA1stUp.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_035;
            lblParamMixingPTrA1stUpInit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_019;
            lblParamMixingPTrA1stUpTop.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_020;
            lblParamMixingPTrA1stUpAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_021;
            lblParamMixingPTrA1stUpConst.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_022;
            lblParamMixingPTrA1stUpInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingPTrA1stUpTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingPTrA1stUpAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_024;
            lblParamMixingPTrA1stUpConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;

            gbxParamMixingPTrB12ndUp.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_036;
            lblParamMixingPTrB12ndUpInit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_019;
            lblParamMixingPTrB12ndUpTop.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_020;
            lblParamMixingPTrB12ndUpAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_021;
            lblParamMixingPTrB12ndUpConst.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_022;
            lblParamMixingPTrB12ndUpInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingPTrB12ndUpTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingPTrB12ndUpAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_024;
            lblParamMixingPTrB12ndUpConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;

            gbxParamMixingPTrB2Down.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_037;
            lblParamMixingPTrB2DownInit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_019;
            lblParamMixingPTrB2DownTop.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_020;
            lblParamMixingPTrB2DownAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_021;
            lblParamMixingPTrB2DownConst.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_022;
            lblParamMixingPTrB2DownInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingPTrB2DownTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingPTrB2DownAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_024;
            lblParamMixingPTrB2DownConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;

            gbxParamMixingPTrReverse.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_038;
            lblParamMixingPTrReverseInit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_019;
            lblParamMixingPTrReverseTop.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_020;
            lblParamMixingPTrReverseAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_021;
            lblParamMixingPTrReverseConst.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_022;
            lblParamMixingPTrReverseInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingPTrReverseTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;
            lblParamMixingPTrReverseAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_024;
            lblParamMixingPTrReverseConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_023;

            gbxParamMixingPTrOffset.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_028;
            lblParamMixingPTrOffsetAPos.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_039;
            lblParamMixingPTrOffsetAPosUnit.Text = Resources_Maintenance.BLANK;

            //MotorAdjustment
            gbxAdjustSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_003;
            SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_031, No=(int)MotorAdjustStopPosition.BFTableInitialization},
            };
            lbxAdjustSequence = ComFunc.setSequenceListBox(lbxAdjustSequence, SequenceList);

            gbxAdjustParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_010;

            gbxAdjustTableThetaAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_014;
            lblAdjustTableThetaAxisOffsetHomePosition.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_029;
            lblAdjustTableThetaAxisOffsetEncodeThresh.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_040;
            lblAdjustTableThetaAxisOffsetHomePositionUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_030;
            lblAdjustTableThetaAxisOffsetEncodeThreshUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BFTABLE_030;

            lblAdjustTableThetaAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;

            btnAdjustTableThetaAxisCW.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_CW_L;
            btnAdjustTableThetaAxisCCW.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_CCW_R;
        }
        #endregion

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
                    StartComm.UnitNo = (int)UnitNoList.BFTable;
                    StartComm.FuncNo = FuncNo;

                    switch (FuncNo)
                    {
                        case (int)BFTableSequence.Step:
                            StartComm.Arg1 = (int)numtestMoveStep.Value;
                            break;
                    }

                    // レスポンスがある機能の場合のみ、レスポンスのログファイルを作成
                    if (ComFunc.chkExistsResponse(MaintenanceMainNavi.BFTableUnit, FuncNo))
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

                    if (ComFunc.chkExistsResponse(MaintenanceMainNavi.BFTableUnit, (int)lbxtestSequenceListBox.SelectedValue))
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
            }
        }

        /// <summary>
        /// パラメータをすべて非活性にする
        /// </summary>
        private void ParametersAllFalse()
        {
            numtestMoveStep.Enabled = false;
        }

        /// <summary>
        /// 選択された機能番号に応じてパラメータを活性化する
        /// </summary>
        private void lbxtestSequenceListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParametersAllFalse();
            switch (lbxtestSequenceListBox.SelectedValue)
            {
                case (int)ReactionTableSequence.Step:
                    numtestMoveStep.Enabled = true;
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

            CarisXConfigParameter.BFTableUnitConfigParam ConfigParam = new CarisXConfigParameter.BFTableUnitConfigParam();

            //R2攪拌時間
            ConfigParam.StirringTimeR2 = (double)numconfStrringTimeR2.Value;
            //BF1攪拌時間
            ConfigParam.StirringTimeBF1 = (double)numconfStrringTimeBF1.Value;
            //BF2攪拌時間
            ConfigParam.StirringTimeBF2 = (double)numconfStrringTimeBF2.Value;
            //Pre-trigger攪拌時間
            ConfigParam.StirringTimePretrigger = (double)numconfStrringTimePretrigger.Value;

            config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableUnitConfigParam = ConfigParam;

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

            //R2攪拌時間
            numconfStrringTimeR2.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableUnitConfigParam.StirringTimeR2;
            //BF1攪拌時間
            numconfStrringTimeBF1.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableUnitConfigParam.StirringTimeBF1;
            //BF2攪拌時間
            numconfStrringTimeBF2.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableUnitConfigParam.StirringTimeBF2;
            //Pre-trigger攪拌時間
            numconfStrringTimePretrigger.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableUnitConfigParam.StirringTimePretrigger;
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

            //BFテーブル部θ軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.BFTableThetaAxis;
            if (!adjustSave)
            {
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamTableThetaInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamTableThetaTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamTableThetaAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamTableThetaConst.Value;

                Mparam.MotorOffset[0] = (double)numParamTableThetaAxisOffsetHomePosition.Value;
                Mparam.MotorOffset[1] = (double)numParamTableThetaAxisOffsetEncodeThresh.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableThetaAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableThetaAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                //送信用アイテムにセット
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableThetaAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustTableThetaAxisOffsetHomePosition.Value;
                Mparam.MotorOffset[1] = (double)numAdjustTableThetaAxisOffsetEncodeThresh.Value;
            }
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableThetaAxisParam.OffsetHomePosition = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableThetaAxisParam.OffsetEncodeThresh = Mparam.MotorOffset[1];

            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);


            if (!adjustSave)
            {
                //撹拌部　R2撹拌Zθ
                Mparam = new SlaveCommCommand_0471();
                Mparam.MotorNo = (int)MotorNoList.BFTableR2MixingZThetaAxis;

                Mparam.MotorSpeed[0].InitSpeed = (int)numParamMixingR2A1stUpInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamMixingR2A1stUpTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamMixingR2A1stUpAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamMixingR2A1stUpConst.Value;

                Mparam.MotorSpeed[1].InitSpeed = Mparam.MotorSpeed[0].TopSpeed;                 //A Topの値をB1 Initに設定
                Mparam.MotorSpeed[1].TopSpeed = (int)numParamMixingR2B12ndUpTop.Value;
                Mparam.MotorSpeed[1].Accel = (int)numParamMixingR2B12ndUpAccelerator.Value;
                Mparam.MotorSpeed[1].ConstSpeed = (int)numParamMixingR2B12ndUpConst.Value;

                Mparam.MotorSpeed[2].InitSpeed = (int)numParamMixingR2B2DownInit.Value;
                Mparam.MotorSpeed[2].TopSpeed = Mparam.MotorSpeed[1].TopSpeed;                  //B1 TopをB2 Topに設定
                Mparam.MotorSpeed[2].Accel = (int)numParamMixingR2B2DownAccelerator.Value;
                Mparam.MotorSpeed[2].ConstSpeed = (int)numParamMixingR2B2DownConst.Value;

                Mparam.MotorSpeed[3].InitSpeed = (int)numParamMixingR2ReverseInit.Value;
                Mparam.MotorSpeed[3].TopSpeed = (int)numParamMixingR2ReverseTop.Value;
                Mparam.MotorSpeed[3].Accel = (int)numParamMixingR2ReverseAccelerator.Value;
                Mparam.MotorSpeed[3].ConstSpeed = (int)numParamMixingR2ReverseConst.Value;

                Mparam.MotorOffset[0] = (double)numParamMixingR2OffsetAPos.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableR2MixingZThetaAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableR2MixingZThetaAxisParam.MotorSpeed = Mparam.MotorSpeed;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableR2MixingZThetaAxisParam.OffsetAPos = Mparam.MotorOffset[0];

                //モータパラメータ送信内容スプール
                SpoolParam(Mparam);


                //撹拌部　BF1撹拌Zθ
                Mparam = new SlaveCommCommand_0471();
                Mparam.MotorNo = (int)MotorNoList.BFTableBF1MixingZThetaAxis;

                Mparam.MotorSpeed[0].InitSpeed = (int)numParamMixingBF1A1stUpInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamMixingBF1A1stUpTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamMixingBF1A1stUpAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamMixingBF1A1stUpConst.Value;

                Mparam.MotorSpeed[1].InitSpeed = Mparam.MotorSpeed[0].TopSpeed;                 //A Topの値をB1 Initに設定
                Mparam.MotorSpeed[1].TopSpeed = (int)numParamMixingBF1B12ndUpTop.Value;
                Mparam.MotorSpeed[1].Accel = (int)numParamMixingBF1B12ndUpAccelerator.Value;
                Mparam.MotorSpeed[1].ConstSpeed = (int)numParamMixingBF1B12ndUpConst.Value;

                Mparam.MotorSpeed[2].InitSpeed = (int)numParamMixingBF1B2DownInit.Value;
                Mparam.MotorSpeed[2].TopSpeed = Mparam.MotorSpeed[1].TopSpeed;                  //B1 TopをB2 Topに設定
                Mparam.MotorSpeed[2].Accel = (int)numParamMixingBF1B2DownAccelerator.Value;
                Mparam.MotorSpeed[2].ConstSpeed = (int)numParamMixingBF1B2DownConst.Value;

                Mparam.MotorSpeed[3].InitSpeed = (int)numParamMixingBF1ReverseInit.Value;
                Mparam.MotorSpeed[3].TopSpeed = (int)numParamMixingBF1ReverseTop.Value;
                Mparam.MotorSpeed[3].Accel = (int)numParamMixingBF1ReverseAccelerator.Value;
                Mparam.MotorSpeed[3].ConstSpeed = (int)numParamMixingBF1ReverseConst.Value;

                Mparam.MotorOffset[0] = (double)numParamMixingBF1OffsetAPos.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF1MixingZThetaAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF1MixingZThetaAxisParam.MotorSpeed = Mparam.MotorSpeed;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF1MixingZThetaAxisParam.OffsetAPos = Mparam.MotorOffset[0];

                //モータパラメータ送信内容スプール
                SpoolParam(Mparam);


                //撹拌部　BF2撹拌Zθ
                Mparam = new SlaveCommCommand_0471();
                Mparam.MotorNo = (int)MotorNoList.BFTableBF2MixingZThetaAxis;

                Mparam.MotorSpeed[0].InitSpeed = (int)numParamMixingBF2A1stUpInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamMixingBF2A1stUpTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamMixingBF2A1stUpAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamMixingBF2A1stUpConst.Value;

                Mparam.MotorSpeed[1].InitSpeed = Mparam.MotorSpeed[0].TopSpeed;                 //A Topの値をB1 Initに設定
                Mparam.MotorSpeed[1].TopSpeed = (int)numParamMixingBF2B12ndUpTop.Value;
                Mparam.MotorSpeed[1].Accel = (int)numParamMixingBF2B12ndUpAccelerator.Value;
                Mparam.MotorSpeed[1].ConstSpeed = (int)numParamMixingBF2B12ndUpConst.Value;

                Mparam.MotorSpeed[2].InitSpeed = (int)numParamMixingBF2B2DownInit.Value;
                Mparam.MotorSpeed[2].TopSpeed = Mparam.MotorSpeed[1].TopSpeed;                  //B1 TopをB2 Topに設定
                Mparam.MotorSpeed[2].Accel = (int)numParamMixingBF2B2DownAccelerator.Value;
                Mparam.MotorSpeed[2].ConstSpeed = (int)numParamMixingBF2B2DownConst.Value;

                Mparam.MotorSpeed[3].InitSpeed = (int)numParamMixingBF2ReverseInit.Value;
                Mparam.MotorSpeed[3].TopSpeed = (int)numParamMixingBF2ReverseTop.Value;
                Mparam.MotorSpeed[3].Accel = (int)numParamMixingBF2ReverseAccelerator.Value;
                Mparam.MotorSpeed[3].ConstSpeed = (int)numParamMixingBF2ReverseConst.Value;

                Mparam.MotorOffset[0] = (double)numParamMixingBF2OffsetAPos.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF2MixingZThetaAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF2MixingZThetaAxisParam.MotorSpeed = Mparam.MotorSpeed;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF2MixingZThetaAxisParam.OffsetAPos = Mparam.MotorOffset[0];

                //モータパラメータ送信内容スプール
                SpoolParam(Mparam);


                //撹拌部　ｐTr撹拌Zθ
                Mparam = new SlaveCommCommand_0471();
                Mparam.MotorNo = (int)MotorNoList.BFTablePreTriggerMixingZThetaAxis;

                Mparam.MotorSpeed[0].InitSpeed = (int)numParamMixingPTrA1stUpInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamMixingPTrA1stUpTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamMixingPTrA1stUpAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamMixingPTrA1stUpConst.Value;

                Mparam.MotorSpeed[1].InitSpeed = Mparam.MotorSpeed[0].TopSpeed;                 //A Topの値をB1 Initに設定
                Mparam.MotorSpeed[1].TopSpeed = (int)numParamMixingPTrB12ndUpTop.Value;
                Mparam.MotorSpeed[1].Accel = (int)numParamMixingPTrB12ndUpAccelerator.Value;
                Mparam.MotorSpeed[1].ConstSpeed = (int)numParamMixingPTrB12ndUpConst.Value;

                Mparam.MotorSpeed[2].InitSpeed = (int)numParamMixingPTrB2DownInit.Value;
                Mparam.MotorSpeed[2].TopSpeed = Mparam.MotorSpeed[1].TopSpeed;                  //B1 TopをB2 Topに設定
                Mparam.MotorSpeed[2].Accel = (int)numParamMixingPTrB2DownAccelerator.Value;
                Mparam.MotorSpeed[2].ConstSpeed = (int)numParamMixingPTrB2DownConst.Value;

                Mparam.MotorSpeed[3].InitSpeed = (int)numParamMixingPTrReverseInit.Value;
                Mparam.MotorSpeed[3].TopSpeed = (int)numParamMixingPTrReverseTop.Value;
                Mparam.MotorSpeed[3].Accel = (int)numParamMixingPTrReverseAccelerator.Value;
                Mparam.MotorSpeed[3].ConstSpeed = (int)numParamMixingPTrReverseConst.Value;

                Mparam.MotorOffset[0] = (double)numParamMixingPTrOffsetAPos.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTablePreTriggerMixingZThetaAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTablePreTriggerMixingZThetaAxisParam.MotorSpeed = Mparam.MotorSpeed;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTablePreTriggerMixingZThetaAxisParam.OffsetAPos = Mparam.MotorOffset[0];

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

            //BFテーブル部θ軸
            numParamTableThetaInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableThetaAxisParam.MotorSpeed[0].InitSpeed;
            numParamTableThetaTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableThetaAxisParam.MotorSpeed[0].TopSpeed;
            numParamTableThetaAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableThetaAxisParam.MotorSpeed[0].Accel;
            numParamTableThetaConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableThetaAxisParam.MotorSpeed[0].ConstSpeed;
            //オフセット
            numParamTableThetaAxisOffsetHomePosition.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableThetaAxisParam.OffsetHomePosition;
            numParamTableThetaAxisOffsetEncodeThresh.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableThetaAxisParam.OffsetEncodeThresh;
            //オフセット（モーター調整用）
            numAdjustTableThetaAxisOffsetHomePosition.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableThetaAxisParam.OffsetHomePosition;
            numAdjustTableThetaAxisOffsetEncodeThresh.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableThetaAxisParam.OffsetEncodeThresh;

            //撹拌部 R2撹拌Zθ
            numParamMixingR2A1stUpInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableR2MixingZThetaAxisParam.MotorSpeed[0].InitSpeed;
            numParamMixingR2A1stUpTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableR2MixingZThetaAxisParam.MotorSpeed[0].TopSpeed;
            numParamMixingR2A1stUpAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableR2MixingZThetaAxisParam.MotorSpeed[0].Accel;
            numParamMixingR2A1stUpConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableR2MixingZThetaAxisParam.MotorSpeed[0].ConstSpeed;

            numParamMixingR2B12ndUpInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableR2MixingZThetaAxisParam.MotorSpeed[1].InitSpeed;
            numParamMixingR2B12ndUpTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableR2MixingZThetaAxisParam.MotorSpeed[1].TopSpeed;
            numParamMixingR2B12ndUpAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableR2MixingZThetaAxisParam.MotorSpeed[1].Accel;
            numParamMixingR2B12ndUpConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableR2MixingZThetaAxisParam.MotorSpeed[1].ConstSpeed;

            numParamMixingR2B2DownInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableR2MixingZThetaAxisParam.MotorSpeed[2].InitSpeed;
            numParamMixingR2B2DownTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableR2MixingZThetaAxisParam.MotorSpeed[2].TopSpeed;
            numParamMixingR2B2DownAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableR2MixingZThetaAxisParam.MotorSpeed[2].Accel;
            numParamMixingR2B2DownConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableR2MixingZThetaAxisParam.MotorSpeed[2].ConstSpeed;

            numParamMixingR2ReverseInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableR2MixingZThetaAxisParam.MotorSpeed[3].InitSpeed;
            numParamMixingR2ReverseTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableR2MixingZThetaAxisParam.MotorSpeed[3].TopSpeed;
            numParamMixingR2ReverseAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableR2MixingZThetaAxisParam.MotorSpeed[3].Accel;
            numParamMixingR2ReverseConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableR2MixingZThetaAxisParam.MotorSpeed[3].ConstSpeed;
            //オフセット
            numParamMixingR2OffsetAPos.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableR2MixingZThetaAxisParam.OffsetAPos;

            //撹拌部 BF1撹拌Zθ
            numParamMixingBF1A1stUpInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF1MixingZThetaAxisParam.MotorSpeed[0].InitSpeed;
            numParamMixingBF1A1stUpTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF1MixingZThetaAxisParam.MotorSpeed[0].TopSpeed;
            numParamMixingBF1A1stUpAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF1MixingZThetaAxisParam.MotorSpeed[0].Accel;
            numParamMixingBF1A1stUpConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF1MixingZThetaAxisParam.MotorSpeed[0].ConstSpeed;

            numParamMixingBF1B12ndUpInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF1MixingZThetaAxisParam.MotorSpeed[1].InitSpeed;
            numParamMixingBF1B12ndUpTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF1MixingZThetaAxisParam.MotorSpeed[1].TopSpeed;
            numParamMixingBF1B12ndUpAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF1MixingZThetaAxisParam.MotorSpeed[1].Accel;
            numParamMixingBF1B12ndUpConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF1MixingZThetaAxisParam.MotorSpeed[1].ConstSpeed;

            numParamMixingBF1B2DownInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF1MixingZThetaAxisParam.MotorSpeed[2].InitSpeed;
            numParamMixingBF1B2DownTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF1MixingZThetaAxisParam.MotorSpeed[2].TopSpeed;
            numParamMixingBF1B2DownAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF1MixingZThetaAxisParam.MotorSpeed[2].Accel;
            numParamMixingBF1B2DownConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF1MixingZThetaAxisParam.MotorSpeed[2].ConstSpeed;

            numParamMixingBF1ReverseInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF1MixingZThetaAxisParam.MotorSpeed[3].InitSpeed;
            numParamMixingBF1ReverseTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF1MixingZThetaAxisParam.MotorSpeed[3].TopSpeed;
            numParamMixingBF1ReverseAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF1MixingZThetaAxisParam.MotorSpeed[3].Accel;
            numParamMixingBF1ReverseConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF1MixingZThetaAxisParam.MotorSpeed[3].ConstSpeed;
            //オフセット
            numParamMixingBF1OffsetAPos.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF1MixingZThetaAxisParam.OffsetAPos;

            //撹拌部 BF2撹拌Zθ
            numParamMixingBF2A1stUpInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF2MixingZThetaAxisParam.MotorSpeed[0].InitSpeed;
            numParamMixingBF2A1stUpTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF2MixingZThetaAxisParam.MotorSpeed[0].TopSpeed;
            numParamMixingBF2A1stUpAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF2MixingZThetaAxisParam.MotorSpeed[0].Accel;
            numParamMixingBF2A1stUpConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF2MixingZThetaAxisParam.MotorSpeed[0].ConstSpeed;

            numParamMixingBF2B12ndUpInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF2MixingZThetaAxisParam.MotorSpeed[1].InitSpeed;
            numParamMixingBF2B12ndUpTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF2MixingZThetaAxisParam.MotorSpeed[1].TopSpeed;
            numParamMixingBF2B12ndUpAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF2MixingZThetaAxisParam.MotorSpeed[1].Accel;
            numParamMixingBF2B12ndUpConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF2MixingZThetaAxisParam.MotorSpeed[1].ConstSpeed;

            numParamMixingBF2B2DownInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF2MixingZThetaAxisParam.MotorSpeed[2].InitSpeed;
            numParamMixingBF2B2DownTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF2MixingZThetaAxisParam.MotorSpeed[2].TopSpeed;
            numParamMixingBF2B2DownAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF2MixingZThetaAxisParam.MotorSpeed[2].Accel;
            numParamMixingBF2B2DownConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF2MixingZThetaAxisParam.MotorSpeed[2].ConstSpeed;

            numParamMixingBF2ReverseInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF2MixingZThetaAxisParam.MotorSpeed[3].InitSpeed;
            numParamMixingBF2ReverseTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF2MixingZThetaAxisParam.MotorSpeed[3].TopSpeed;
            numParamMixingBF2ReverseAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF2MixingZThetaAxisParam.MotorSpeed[3].Accel;
            numParamMixingBF2ReverseConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF2MixingZThetaAxisParam.MotorSpeed[3].ConstSpeed;
            //オフセット
            numParamMixingBF2OffsetAPos.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableBF2MixingZThetaAxisParam.OffsetAPos;

            //撹拌部 pTr撹拌Zθ
            numParamMixingPTrA1stUpInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTablePreTriggerMixingZThetaAxisParam.MotorSpeed[0].InitSpeed;
            numParamMixingPTrA1stUpTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTablePreTriggerMixingZThetaAxisParam.MotorSpeed[0].TopSpeed;
            numParamMixingPTrA1stUpAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTablePreTriggerMixingZThetaAxisParam.MotorSpeed[0].Accel;
            numParamMixingPTrA1stUpConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTablePreTriggerMixingZThetaAxisParam.MotorSpeed[0].ConstSpeed;

            numParamMixingPTrB12ndUpInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTablePreTriggerMixingZThetaAxisParam.MotorSpeed[1].InitSpeed;
            numParamMixingPTrB12ndUpTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTablePreTriggerMixingZThetaAxisParam.MotorSpeed[1].TopSpeed;
            numParamMixingPTrB12ndUpAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTablePreTriggerMixingZThetaAxisParam.MotorSpeed[1].Accel;
            numParamMixingPTrB12ndUpConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTablePreTriggerMixingZThetaAxisParam.MotorSpeed[1].ConstSpeed;

            numParamMixingPTrB2DownInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTablePreTriggerMixingZThetaAxisParam.MotorSpeed[2].InitSpeed;
            numParamMixingPTrB2DownTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTablePreTriggerMixingZThetaAxisParam.MotorSpeed[2].TopSpeed;
            numParamMixingPTrB2DownAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTablePreTriggerMixingZThetaAxisParam.MotorSpeed[2].Accel;
            numParamMixingPTrB2DownConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTablePreTriggerMixingZThetaAxisParam.MotorSpeed[2].ConstSpeed;

            numParamMixingPTrReverseInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTablePreTriggerMixingZThetaAxisParam.MotorSpeed[3].InitSpeed;
            numParamMixingPTrReverseTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTablePreTriggerMixingZThetaAxisParam.MotorSpeed[3].TopSpeed;
            numParamMixingPTrReverseAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTablePreTriggerMixingZThetaAxisParam.MotorSpeed[3].Accel;
            numParamMixingPTrReverseConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTablePreTriggerMixingZThetaAxisParam.MotorSpeed[3].ConstSpeed;
            //オフセット
            numParamMixingPTrOffsetAPos.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTablePreTriggerMixingZThetaAxisParam.OffsetAPos;

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
            gbxAdjustTableThetaAxis.Enabled = false;
            numAdjustTableThetaAxisOffsetHomePosition.Enabled = false;
            numAdjustTableThetaAxisOffsetEncodeThresh.Enabled = false;

            //EditBoxの文字色をすべて黒にする。
            EditBoxControlsBlack(tabUnit.Tabs[3].TabPage);
        }

        /// <summary>
        /// モーター調整機能スタート時処理
        /// </summary>
        private void AdjustUnitStartPrc()
        {
            // モータ調整ボタンEnable
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
            AdjustStartComm.Pos = (int)Invoke(getStopPos);

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
            btnAdjustTableThetaAxisCW.Enabled = enable;
            btnAdjustTableThetaAxisCCW.Enabled = enable;
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
                case (int)MotorAdjustStopPosition.BFTableInitialization:
                    gbxAdjustTableThetaAxis.Enabled = true;
                    numAdjustTableThetaAxisOffsetHomePosition.Enabled = true;
                    numAdjustTableThetaAxisOffsetHomePosition.ForeColor = System.Drawing.Color.OrangeRed;

                    break;
            }
        }

        #region Pitch値の調整
        private void btnAdjustTableThetaAxisPitchUp_Click(object sender, EventArgs e)
        {
            numAdjustTableThetaAxisPitch.Value = (double)numAdjustTableThetaAxisPitch.Value + (double)numAdjustTableThetaAxisPitch.SpinIncrement;
        }

        private void btnAdjustTableThetaAxisPitchDown_Click(object sender, EventArgs e)
        {
            numAdjustTableThetaAxisPitch.Value = (double)numAdjustTableThetaAxisPitch.Value - (double)numAdjustTableThetaAxisPitch.SpinIncrement;
        }
        #endregion

        private void btnAdjustTableThetaAxisCWCCW_Click(object sender, EventArgs e)
        {
            Boolean upFlg = ((Button)sender == btnAdjustTableThetaAxisCCW);
            Int32 motorNo = (int)MotorNoList.BFTableThetaAxis;
            Double pitchValue = (double)numAdjustTableThetaAxisPitch.Value;

            AdjustValue(upFlg, motorNo, numAdjustTableThetaAxisOffsetHomePosition, pitchValue);
        }
    }
}
