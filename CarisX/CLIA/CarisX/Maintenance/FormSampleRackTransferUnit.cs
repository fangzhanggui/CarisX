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
    /// ラック搬送
    /// </summary>
    public partial class FormSampleRackTransferUnit : FormUnitBase
    {
        IMaintenanceUnitStart unitStart = new UnitStart();
        IMaintenanceUnitStart unitStartAdjust = new UnitStarAdjust();
        IMaintenanceUnitStart unitAdjust = new UnitAdjust();
        IMaintenanceUnitStart unitAdjustAbort = new UnitStarAdjustAbort();
        volatile bool ResponseFlg;
        public CommonFunction ComFunc = new CommonFunction();
        Infragistics.Win.ValueListItem[] cmbtestFeederTypeValue;
        Infragistics.Win.ValueListItem[] cmbtestPositionValue;
        Infragistics.Win.ValueListItem[] cmbtestLoaderTypeValue;
        Infragistics.Win.ValueListItem[] cmbtestModuleValue;
        Infragistics.Win.ValueListItem[] cmbtestForkOpePosValue;
        Infragistics.Win.ValueListItem[] cmbtestPositionAValue;
        Infragistics.Win.ValueListItem[] cmbtestPositionBValue;
        Infragistics.Win.ValueListItem[] cmbAdjustModuleValue;

        //モーター調整コマンドチクチク
        RackTransferCommCommand_0073 AdjustUpDownComm = new RackTransferCommCommand_0073();
        public FormSampleRackTransferUnit()
        {
            InitializeComponent();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ParametersAllFalse();
            ConfigParamLoad();
            MotorParamDisp();
            setCulture();
            setRadioButtonValue();
            setCmbValue();
            ComFunc.SetControlSettings(this);
            ConfigTabUseFlg = tabUnit.Tabs[(int)MaintenanceTabIndex.Config].Enabled;    //Configタブを利用有無を退避

            lbxtestSequence.SelectedIndex = 0;
            cmbtestFeederType.SelectedIndex = 0;
            cmbtestPosition.SelectedIndex = 0;
            cmbtestLoaderType.SelectedIndex = 0;
            cmbtestModule.SelectedIndex = 0;
            cmbtestForkOperationPosition.SelectedIndex = 0;
            cmbtestPositionA.SelectedIndex = 0;
            cmbtestPositionB.SelectedIndex = 0;
            lbxAdjustSequence.SelectedIndex = 0;
            cmbAdjustModule.SelectedIndex = 0;
            cmbAdjustSamplePosition.SelectedIndex = 0;

            //これがないと、初回起動時のToolbarsDispでSelectedTab.Indexが取得できずに落ちる
            tabUnit.Tabs[0].Selected = true;
        }

        #region リソース設定
        private void setCulture()
        {
            //Tab
            tabUnit.Tabs[0].Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_017;             //Test
            tabUnit.Tabs[1].Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_018;             //Configuration
            tabUnit.Tabs[2].Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_019;             //Motor Parameters
            tabUnit.Tabs[3].Text = Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_MOTORADJUSTMENT;    //Adjust

            //Testタブ
            gbxtestSequence.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_000;  //Sequence
            object SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_001, No=(int)RackTransferSequence.Init},
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_002, No=(int)RackTransferSequence.FeederOperation},
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_003, No=(int)RackTransferSequence.LoaderOperation},
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_004, No=(int)RackTransferSequence.ForkOperation},
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_005, No=(int)RackTransferSequence.RackSetting},
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_006, No=(int)RackTransferSequence.RackIDReading},
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_007, No=(int)RackTransferSequence.SampleIDReading},
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_008, No=(int)RackTransferSequence.SampleCupTubeCheck},
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_009, No=(int)RackTransferSequence.SendMoveStandby},
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_064, No=(int)RackTransferSequence.BackMoveStandby},
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_061, No=(int)RackTransferSequence.SendRackCatch},
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_065, No=(int)RackTransferSequence.BackRackCatch},
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_062, No=(int)RackTransferSequence.SendRackRelease},
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_063, No=(int)RackTransferSequence.BackRackRelease},
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_066, No=(int)RackTransferSequence.RackUnload},
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_067, No=(int)RackTransferSequence.RackRetest},
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_068, No=(int)RackTransferSequence.RackTakeOut },
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_174, No=(int)RackTransferSequence.RackReturn },
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_175, No=(int)RackTransferSequence.RackCoverLED },
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_185, No=(int)RackTransferSequence.AllRackOperation },
            };
            lbxtestSequence = ComFunc.setSequenceListBox(lbxtestSequence, SequenceList);

            cmbtestFeederTypeValue = new Infragistics.Win.ValueListItem[]
            {
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_084, dataValue:RackTransferCmbValue.FeederTypeLoad),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_085, dataValue:RackTransferCmbValue.FeederTypeUnload),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_086, dataValue:RackTransferCmbValue.FeederTypeSlider),
            };

            cmbtestPositionValue = new Infragistics.Win.ValueListItem[]
            {
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_177, dataValue:RackTransferCmbValue.PositionLeft),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_178, dataValue:RackTransferCmbValue.PositionMiddle),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_179, dataValue:RackTransferCmbValue.PositionRight),
            };

            cmbtestLoaderTypeValue = new Infragistics.Win.ValueListItem[]
            {
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_090, dataValue:RackTransferCmbValue.LoaderTypeLoadY),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_091, dataValue:RackTransferCmbValue.LoaderTypeUnLoadY),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_092, dataValue:RackTransferCmbValue.LoaderTypeTakeOutY),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_093, dataValue:RackTransferCmbValue.LoaderTypeSendX1),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_094, dataValue:RackTransferCmbValue.LoaderTypeBackX1),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_095, dataValue:RackTransferCmbValue.LoaderTypeSendX2),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_096, dataValue:RackTransferCmbValue.LoaderTypeBackX2),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_097, dataValue:RackTransferCmbValue.LoaderTypeSendX3),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_098, dataValue:RackTransferCmbValue.LoaderTypeBackX3),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_099, dataValue:RackTransferCmbValue.LoaderTypeSendX4),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_100, dataValue:RackTransferCmbValue.LoaderTypeBackX4),
            };

            cmbtestModuleValue = new Infragistics.Win.ValueListItem[]
            {
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_101, dataValue:RackTransferCmbValue.Module1),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_102, dataValue:RackTransferCmbValue.Module2),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_103, dataValue:RackTransferCmbValue.Module3),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_104, dataValue:RackTransferCmbValue.Module4),
            };

            cmbtestForkOpePosValue = new Infragistics.Win.ValueListItem[]
            {
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_180, dataValue:RackTransferCmbValue.ForkOperationPositionBack),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_181, dataValue:RackTransferCmbValue.ForkOperationPosition2ndfromBack),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_182, dataValue:RackTransferCmbValue.ForkOperationPosition2ndfromFront),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_183, dataValue:RackTransferCmbValue.ForkOperationPositionFront),
            };

            cmbtestPositionAValue = new Infragistics.Win.ValueListItem[]
            {
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_114, dataValue:RackTransferCmbValue.PositionABCR),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_115, dataValue:RackTransferCmbValue.PositionAModule1),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_116, dataValue:RackTransferCmbValue.PositionAModule2),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_117, dataValue:RackTransferCmbValue.PositionAModule3),
            };

            cmbtestPositionBValue = new Infragistics.Win.ValueListItem[]
            {
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_118, dataValue:RackTransferCmbValue.PositionBModule1),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_119, dataValue:RackTransferCmbValue.PositionBModule2),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_120, dataValue:RackTransferCmbValue.PositionBModule3),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_121, dataValue:RackTransferCmbValue.PositionBModule4),
            };

            gbxtestParameters.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_010;
            lbltestRepeatFrequency.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_011;
            lbltestNumber.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_012;
            lbltestRepeatInterval.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_171;
            lbltestRepeatIntervalUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_172;
            lbltestFeederType.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_069;
            lbltestPosition.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_070;
            lbltestLoaderType.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_071;
            lbltestRotation.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_072;
            rbttestRotationStop.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_073;
            rbttestRotationBackRight.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_074;
            rbttestRotationFrontLeft.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_075;
            lbltestModule.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_076;
            lbltestForkOperationPosition.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_077;
            lbltestSamplePosition.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_078;
            lbltestPositionA.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_079;
            lbltestPositionB.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_080;
            lbltestForkPosition.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_081;
            rbttestForkPositionBack.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_082;
            rbttestForkPositionFront.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_083;
            lbltestInterval.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_176;
            lbltestIntervalUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_172;

            gbxtestResponce.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_016;

            //Configuratioonタブ
            gbxconfParam.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_127;
            lblconfInstallationLoaderTime.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_128;
            lblconfInstallationLoaderTimeUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_129;
            lblconfRetestWaitLoaderTime.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_130;
            lblconfRetestWaitLoaderTimeUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_131;
            lblconfCollectionLoaderTime.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_132;
            lblconfCollectionLoaderTimeUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_133;
            lblconfSendLoaderTime.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_134;
            lblconfSendLoaderTimeUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_135;
            lblconfBackLoaderTime.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_136;
            lblconfBackLoaderTimeUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_137;

            //MotorParameterタブ
            gbxParamM1SendXAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_020;
            lblParamM1SendXAxisInit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_021;
            lblParamM1SendXAxisTop.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_022;
            lblParamM1SendXAxisAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_023;
            lblParamM1SendXAxisConst.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_024;
            lblParamM1SendXAxisInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM1SendXAxisTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM1SendXAxisAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_032;
            lblParamM1SendXAxisConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;

            gbxParamM1BackXAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_034;
            lblParamM1BackXAxisInit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_021;
            lblParamM1BackXAxisTop.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_022;
            lblParamM1BackXAxisAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_023;
            lblParamM1BackXAxisConst.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_024;
            lblParamM1BackXAxisInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM1BackXAxisTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM1BackXAxisAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_032;
            lblParamM1BackXAxisConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;

            gbxParamM1ForkYAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_040;
            lblParamM1ForkYAxisInit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_021;
            lblParamM1ForkYAxisTop.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_022;
            lblParamM1ForkYAxisAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_023;
            lblParamM1ForkYAxisConst.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_024;
            lblParamM1ForkYAxisOffsetHome.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_045;
            lblParamM1ForkYAxisInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM1ForkYAxisTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM1ForkYAxisAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_032;
            lblParamM1ForkYAxisConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM1ForkYAxisOffsetHomeUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;

            gbxParamM2SendXAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_157;
            lblParamM2SendXAxisInit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_021;
            lblParamM2SendXAxisTop.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_022;
            lblParamM2SendXAxisAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_023;
            lblParamM2SendXAxisConst.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_024;
            lblParamM2SendXAxisInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM2SendXAxisTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM2SendXAxisAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_032;
            lblParamM2SendXAxisConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;

            gbxParamM2BackXAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_158;
            lblParamM2BackXAxisInit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_021;
            lblParamM2BackXAxisTop.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_022;
            lblParamM2BackXAxisAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_023;
            lblParamM2BackXAxisConst.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_024;
            lblParamM2BackXAxisInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM2BackXAxisTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM2BackXAxisAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_032;
            lblParamM2BackXAxisConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;

            gbxParamM2ForkYAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_159;
            lblParamM2ForkYAxisInit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_021;
            lblParamM2ForkYAxisTop.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_022;
            lblParamM2ForkYAxisAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_023;
            lblParamM2ForkYAxisConst.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_024;
            lblParamM2ForkYAxisOffsetHome.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_045;
            lblParamM2ForkYAxisInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM2ForkYAxisTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM2ForkYAxisAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_032;
            lblParamM2ForkYAxisConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM2ForkYAxisOffsetHomeUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;

            gbxParamM3SendXAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_160;
            lblParamM3SendXAxisInit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_021;
            lblParamM3SendXAxisTop.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_022;
            lblParamM3SendXAxisAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_023;
            lblParamM3SendXAxisConst.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_024;
            lblParamM3SendXAxisInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM3SendXAxisTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM3SendXAxisAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_032;
            lblParamM3SendXAxisConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;

            gbxParamM3BackXAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_161;
            lblParamM3BackXAxisInit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_021;
            lblParamM3BackXAxisTop.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_022;
            lblParamM3BackXAxisAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_023;
            lblParamM3BackXAxisConst.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_024;
            lblParamM3BackXAxisInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM3BackXAxisTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM3BackXAxisAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_032;
            lblParamM3BackXAxisConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;

            gbxParamM3ForkYAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_162;
            lblParamM3ForkYAxisInit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_021;
            lblParamM3ForkYAxisTop.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_022;
            lblParamM3ForkYAxisAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_023;
            lblParamM3ForkYAxisConst.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_024;
            lblParamM3ForkYAxisOffsetHome.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_045;
            lblParamM3ForkYAxisInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM3ForkYAxisTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM3ForkYAxisAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_032;
            lblParamM3ForkYAxisConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM3ForkYAxisOffsetHomeUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;

            gbxParamM4SendXAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_163;
            lblParamM4SendXAxisInit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_021;
            lblParamM4SendXAxisTop.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_022;
            lblParamM4SendXAxisAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_023;
            lblParamM4SendXAxisConst.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_024;
            lblParamM4SendXAxisInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM4SendXAxisTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM4SendXAxisAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_032;
            lblParamM4SendXAxisConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;

            gbxParamM4BackXAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_164;
            lblParamM4BackXAxisInit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_021;
            lblParamM4BackXAxisTop.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_022;
            lblParamM4BackXAxisAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_023;
            lblParamM4BackXAxisConst.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_024;
            lblParamM4BackXAxisInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM4BackXAxisTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM4BackXAxisAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_032;
            lblParamM4BackXAxisConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;

            gbxParamM4ForkYAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_165;
            lblParamM4ForkYAxisInit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_021;
            lblParamM4ForkYAxisTop.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_022;
            lblParamM4ForkYAxisAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_023;
            lblParamM4ForkYAxisConst.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_024;
            lblParamM4ForkYAxisOffsetHome.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_045;
            lblParamM4ForkYAxisInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM4ForkYAxisTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM4ForkYAxisAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_032;
            lblParamM4ForkYAxisConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamM4ForkYAxisOffsetHomeUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;

            gbxParamLoadYAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_139;
            lblParamLoadYAxisInit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_021;
            lblParamLoadYAxisTop.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_022;
            lblParamLoadYAxisAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_023;
            lblParamLoadYAxisConst.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_024;
            lblParamLoadYAxisInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamLoadYAxisTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamLoadYAxisAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_032;
            lblParamLoadYAxisConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;

            gbxParamUnLoadYAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_140;
            lblParamUnLoadYAxisInit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_021;
            lblParamUnLoadYAxisTop.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_022;
            lblParamUnLoadYAxisAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_023;
            lblParamUnLoadYAxisConst.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_024;
            lblParamUnLoadYAxisInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamUnLoadYAxisTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamUnLoadYAxisAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_032;
            lblParamUnLoadYAxisConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;

            gbxParamTakeOutYAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_141;
            lblParamTakeOutYAxisInit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_021;
            lblParamTakeOutYAxisTop.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_022;
            lblParamTakeOutYAxisAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_023;
            lblParamTakeOutYAxisConst.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_024;
            lblParamTakeOutYAxisInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamTakeOutYAxisTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamTakeOutYAxisAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_032;
            lblParamTakeOutYAxisConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;

            gbxParamLoadFeederXAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_142;
            lblParamLoadFeederXAxisInit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_021;
            lblParamLoadFeederXAxisTop.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_022;
            lblParamLoadFeederXAxisAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_023;
            lblParamLoadFeederXAxisConst.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_024;
            lblParamLoadFeederXAxisOffsetTakeOut.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_053;
            lblParamLoadFeederXAxisOffsetLoad.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_145;
            lblParamLoadFeederXAxisOffsetTubeSensor.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_060;
            lblParamLoadFeederXAxisOffsetSampleIDRead.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_059;
            lblParamLoadFeederXAxisOffsetRackIDRead.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_184;
            lblParamLoadFeederXAxisInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamLoadFeederXAxisTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamLoadFeederXAxisAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_032;
            lblParamLoadFeederXAxisConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamLoadFeederXAxisOffsetTakeOutUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;
            lblParamLoadFeederXAxisOffsetLoadUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;
            lblParamLoadFeederXAxisOffsetTubeSensorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;
            lblParamLoadFeederXAxisOffsetSampleIDReadUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;
            lblParamLoadFeederXAxisOffsetRackIDReadUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;

            gbxParamUnLoadFeederXAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_143;
            lblParamUnLoadFeederXAxisInit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_021;
            lblParamUnLoadFeederXAxisTop.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_022;
            lblParamUnLoadFeederXAxisAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_023;
            lblParamUnLoadFeederXAxisConst.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_024;
            lblParamUnLoadFeederXAxisOffsetTakeOut.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_053;
            lblParamUnLoadFeederXAxisOffsetRetest.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_166;
            lblParamUnLoadFeederXAxisOffsetUnLoad.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_138;
            lblParamUnLoadFeederXAxisInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamUnLoadFeederXAxisTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamUnLoadFeederXAxisAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_032;
            lblParamUnLoadFeederXAxisConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamUnLoadFeederXAxisOffsetTakeOutUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;
            lblParamUnLoadFeederXAxisOffsetRetestUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;
            lblParamUnLoadFeederXAxisOffsetUnLoadUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;

            gbxParamSliderXAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_144;
            lblParamSliderXAxisInit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_123;
            lblParamSliderXAxisTop.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_124;
            lblParamSliderXAxisAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_125;
            lblParamSliderXAxisConst.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_126;
            lblParamSliderXAxisOffsetLoad.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_167;
            lblParamSliderXAxisOffsetUnLoad.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_173;
            lblParamSliderXAxisInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamSliderXAxisTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamSliderXAxisAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_032;
            lblParamSliderXAxisConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_031;
            lblParamSliderXAxisOffsetLoadUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;
            lblParamSliderXAxisOffsetUnLoadUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;

            gbxParamM1SendXAxisCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_025;
            gbxParamM1BackXAxisCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_025;
            gbxParamM1ForkYAxisCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_025;
            gbxParamM2SendXAxisCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_025;
            gbxParamM2BackXAxisCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_025;
            gbxParamM2ForkYAxisCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_025;
            gbxParamM3SendXAxisCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_025;
            gbxParamM3BackXAxisCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_025;
            gbxParamM3ForkYAxisCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_025;
            gbxParamM4SendXAxisCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_025;
            gbxParamM4BackXAxisCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_025;
            gbxParamM4ForkYAxisCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_025;
            gbxParamLoadYAxisCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_025;
            gbxParamUnLoadYAxisCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_025;
            gbxParamTakeOutYAxisCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_025;
            gbxParamLoadFeederXAxisCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_025;
            gbxParamUnLoadFeederXAxisCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_025;
            gbxParamSliderXAxisCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_025;

            gbxParamM1ForkYAxisOffset.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_026;
            gbxParamM2ForkYAxisOffset.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_026;
            gbxParamM3ForkYAxisOffset.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_026;
            gbxParamM4ForkYAxisOffset.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_026;
            gbxParamLoadFeederXAxisOffset.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_026;
            gbxParamUnLoadFeederXAxisOffset.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_026;
            gbxParamSliderXAxisOffset.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_026;

            //MotorAdjustタブ
            gbxAdjustSequence.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_000;
            SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_146, No=(int)MotorAdjustStopPosition.RackForkPosition},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_147, No=(int)MotorAdjustStopPosition.LoadFeederRackLoad},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_148, No=(int)MotorAdjustStopPosition.LoadFeederRackReturnFeeder},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_149, No=(int)MotorAdjustStopPosition.LoadFeederRackIDReading},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_150, No=(int)MotorAdjustStopPosition.LoadFeederSampleIDReading},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_151, No=(int)MotorAdjustStopPosition.LoadFeederTubeSensorReading},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_152, No=(int)MotorAdjustStopPosition.SliderRackLoad},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_153, No=(int)MotorAdjustStopPosition.SliderRackUnLoad},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_154, No=(int)MotorAdjustStopPosition.UnLoadFeederRackUnLoad},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_155, No=(int)MotorAdjustStopPosition.UnLoadFeederRackRetest},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_156, No=(int)MotorAdjustStopPosition.UnLoadFeederRackTakeout},
            };
            lbxAdjustSequence = ComFunc.setSequenceListBox(lbxAdjustSequence, SequenceList);

            gbxAdjustParam.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_010;
            lblAdjustModule.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_015;
            lblAdjustSamplePosition.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_014;
            lblAdjustBCRread.Text = Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_BCR_SENSOR;

            cmbAdjustModuleValue = new Infragistics.Win.ValueListItem[]
            {
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_118, dataValue:RackTransferCmbValue.Module1),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_119, dataValue:RackTransferCmbValue.Module2),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_120, dataValue:RackTransferCmbValue.Module3),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_121, dataValue:RackTransferCmbValue.Module4),
            };

            gbxAdjustBC.Text = Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_BCR_SENSOR_READ;
            btnAdjustRackIDRead.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_168;
            btnAdjustBCRead.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_169;
            btnAdjustTubeSensorRead.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_170;

            gbxAdjustM1ForkYAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_040;
            lblAdjustM1ForkYAxisOffsetHome.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_045;
            lblAdjustM1ForkYAxisOffsetHomeUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;

            gbxAdjustM2ForkYAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_159;
            lblAdjustM2ForkYAxisOffsetHome.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_045;
            lblAdjustM2ForkYAxisOffsetHomeUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;

            gbxAdjustM3ForkYAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_162;
            lblAdjustM3ForkYAxisOffsetHome.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_045;
            lblAdjustM3ForkYAxisOffsetHomeUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;

            gbxAdjustM4ForkYAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_165;
            lblAdjustM4ForkYAxisOffsetHome.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_045;
            lblAdjustM4ForkYAxisOffsetHomeUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;

            gbxAdjustLoadFeederXAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_142;
            lblAdjustLoadFeederXAxisOffsetTakeOut.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_053;
            lblAdjustLoadFeederXAxisOffsetLoad.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_145;
            lblAdjustLoadFeederXAxisOffsetTubeSensor.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_060;
            lblAdjustLoadFeederXAxisOffsetSampleIDRead.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_059;
            lblAdjustLoadFeederXAxisOffsetRackIDRead.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_184;
            lblAdjustLoadFeederXAxisOffsetTakeOutUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;
            lblAdjustLoadFeederXAxisOffsetLoadUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;
            lblAdjustLoadFeederXAxisOffsetTubeSensorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;
            lblAdjustLoadFeederXAxisOffsetSampleIDReadUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;
            lblAdjustLoadFeederXAxisOffsetRackIDReadUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;

            gbxAdjustUnLoadFeederXAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_143;
            lblAdjustUnLoadFeederXAxisOffsetTakeOut.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_053;
            lblAdjustUnLoadFeederXAxisOffsetRemeas.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_166;
            lblAdjustUnLoadFeederXAxisOffsetUnLoad.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_138;
            lblAdjustUnLoadFeederXAxisOffsetTakeOutUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;
            lblAdjustUnLoadFeederXAxisOffsetRemeasUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;
            lblAdjustUnLoadFeederXAxisOffsetUnLoadUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;

            gbxAdjustSliderXAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_144;
            lblAdjustSliderXAxisOffsetLoad.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_167;
            lblAdjustSliderXAxisOffsetUnLoad.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_173;
            lblAdjustSliderXAxisOffsetLoadUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;
            lblAdjustSliderXAxisOffsetUnLoadUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_SAMPLERACKTRANS_033;

            //Pitch
            lblAdjustM1ForkYAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;
            lblAdjustM2ForkYAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;
            lblAdjustM3ForkYAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;
            lblAdjustM4ForkYAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;
            lblAdjustLoadFeederXAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;
            lblAdjustUnLoadFeederXAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;
            lblAdjustSliderXAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;

            //Pitch Adjust
            btnAdjustM1ForkYAxisForward.Text = Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_FORWARD;
            btnAdjustM1ForkYAxisBack.Text = Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_BACK;
            btnAdjustM2ForkYAxisForward.Text = Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_FORWARD;
            btnAdjustM2ForkYAxisBack.Text = Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_BACK;
            btnAdjustM3ForkYAxisForward.Text = Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_FORWARD;
            btnAdjustM3ForkYAxisBack.Text = Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_BACK;
            btnAdjustM4ForkYAxisForward.Text = Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_FORWARD;
            btnAdjustM4ForkYAxisBack.Text = Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_BACK;
            btnAdjustLoadFeederXAxisLeft.Text = Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_LEFT;
            btnAdjustLoadFeederXAxisRight.Text = Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_RIGHT;
            btnAdjustUnLoadFeederXAxisLeft.Text = Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_LEFT;
            btnAdjustUnLoadFeederXAxisRight.Text = Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_RIGHT;
            btnAdjustSliderXAxisLeft.Text = Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_LEFT;
            btnAdjustSliderXAxisRight.Text = Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_RIGHT;

        }
        #endregion

        /// <summary>
        /// ラジオボタンの選択値を設定
        /// </summary>
        private void setRadioButtonValue()
        {
            rbttestRotationStop.Tag = (int)RackTransferRadioValue.RotationStop;
            rbttestRotationBackRight.Tag = (int)RackTransferRadioValue.RotationBackRight;
            rbttestRotationFrontLeft.Tag = (int)RackTransferRadioValue.RotationFrontLeft;
            rbttestForkPositionBack.Tag = (int)RackTransferRadioValue.ForkPositionBack;
            rbttestForkPositionFront.Tag = (int)RackTransferRadioValue.ForkPositionFront;
        }

        /// <summary>
        /// 機能番号のドロップダウンの設定
        /// </summary>
        private void setCmbValue()
        {
            cmbtestFeederType.Items.AddRange(cmbtestFeederTypeValue);
            cmbtestPosition.Items.AddRange(cmbtestPositionValue);
            cmbtestLoaderType.Items.AddRange(cmbtestLoaderTypeValue);
            cmbtestModule.Items.AddRange(cmbtestModuleValue);
            cmbtestForkOperationPosition.Items.AddRange(cmbtestForkOpePosValue);
            cmbtestPositionA.Items.AddRange(cmbtestPositionAValue);
            cmbtestPositionB.Items.AddRange(cmbtestPositionBValue);
            cmbAdjustModule.Items.AddRange(cmbAdjustModuleValue);
        }

        /// <summary>
        /// PositionA、Bの大小チェックを行う
        /// </summary>
        private bool chkPositionAB()
        {
            bool chkReslut = true;
            int PositionA;
            int PositionB;

            if (tabUnit.SelectedTab.Index == 0)
            {
                Func<object> getfuncno = () => { return lbxtestSequence.SelectedValue; };
                int FuncNo = (int)Invoke(getfuncno);

                switch (FuncNo)
                {
                    case (int)RackTransferSequence.SendMoveStandby:
                    case (int)RackTransferSequence.BackMoveStandby:
                        PositionA = (int)cmbtestPositionA.SelectedItem.DataValue;
                        PositionB = (int)cmbtestPositionB.SelectedItem.DataValue;
                        if (PositionA >= PositionB)
                            chkReslut = false;
                        break;
                }
            }

            return chkReslut;
        }

        /// <summary>
        /// ユニットをスタートします。
        /// </summary>
        public override void UnitStart()
        {
            //入力値チェック
            if (!CheckControls(tabUnit.Tabs[0].TabPage))
            {
                DlgMaintenance msg = new DlgMaintenance(Resources_Maintenance.DlgSeqChkMsg, false);
                msg.ShowDialog();
                DispEnable(true);
                return;
            }
            //フォームのバリデーションを行う
            if (!chkPositionAB())
            {
                DlgMaintenance msg = new DlgMaintenance(Resources_Maintenance.DlgSeqChkMsg, false);
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
                    Func<object> getfuncno = () => { return lbxtestSequence.SelectedValue; };
                    int FuncNo = (int)Invoke(getfuncno);

                    Func<string> getfuncname = () => { return lbxtestSequence.Text; };
                    string FuncName = (string)Invoke(getfuncname);

                    RackTransferCommCommand_0039 StartComm = new RackTransferCommCommand_0039();
                    StartComm.UnitNo = getUnitNo(FuncNo);
                    StartComm.FuncNo = FuncNo;

                    switch (FuncNo)
                    {
                        case (int)RackTransferSequence.FeederOperation:
                            StartComm.Arg1 = (int)cmbtestFeederType.SelectedItem.DataValue;             //フィーダー種別
                            StartComm.Arg2 = (int)cmbtestPosition.SelectedItem.DataValue;               //ポジション
                            break;
                        case (int)RackTransferSequence.LoaderOperation:
                            StartComm.Arg1 = int.Parse(((int)cmbtestLoaderType.SelectedItem.DataValue).ToString().Substring(0, 1));     //ローダー種別
                            StartComm.Arg2 = int.Parse(((int)cmbtestLoaderType.SelectedItem.DataValue).ToString().Substring(1, 1));     //装置番号
                            StartComm.Arg3 = ComFunc.getSelectedRadioButtonValue(gbxtestRotation);                                      //モーター回転方向
                            break;
                        case (int)RackTransferSequence.ForkOperation:
                            StartComm.Arg1 = (int)cmbtestModule.SelectedItem.DataValue;                 //装置番号
                            StartComm.Arg2 = (int)cmbtestForkOperationPosition.SelectedItem.DataValue;  //ポジション
                            break;
                        case (int)RackTransferSequence.RackIDReading:
                            StartComm.Arg1 = 0;                                                         //「0：移動動作なし」を固定で設定
                            break;
                        case (int)RackTransferSequence.SampleIDReading:
                        case (int)RackTransferSequence.SampleCupTubeCheck:
                            StartComm.Arg1 = (int)numtestSamplePosition.Value;                          //ラックポジション
                            break;
                        case (int)RackTransferSequence.SendMoveStandby:
                            StartComm.Arg1 = (int)cmbtestPositionA.SelectedItem.DataValue;              //開始位置
                            StartComm.Arg2 = (int)cmbtestPositionB.SelectedItem.DataValue;              //停止位置
                            break;
                        case (int)RackTransferSequence.BackMoveStandby:
                            StartComm.Arg1 = (int)cmbtestPositionB.SelectedItem.DataValue;              //開始位置
                            StartComm.Arg2 = (int)cmbtestPositionA.SelectedItem.DataValue;              //停止位置
                            break;
                        case (int)RackTransferSequence.SendRackCatch:
                        case (int)RackTransferSequence.BackRackCatch:
                        case (int)RackTransferSequence.SendRackRelease:
                        case (int)RackTransferSequence.BackRackRelease:
                            StartComm.Arg1 = (int)cmbtestModule.SelectedItem.DataValue;                 //装置
                            StartComm.Arg2 = ComFunc.getSelectedRadioButtonValue(gbxtestForkPosition);  //ラック引き込みユニット位置
                            break;
                        case (int)RackTransferSequence.RackCoverLED:
                            StartComm.Arg1 = (int)numtestInterval.Value;                                //インターバル
                            break;
                        case (int)RackTransferSequence.AllRackOperation:
                            StartComm.Arg1 = (int)cmbtestModule.SelectedItem.DataValue;                 //装置
                            break;
                    }

                    //レスポンスがある機能の場合のみ、レスポンスのログファイルを作成
                    if (ComFunc.chkExistsResponse(MaintenanceMainNavi.RackAllUnits, FuncNo))
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
                        unitStart.Start(StartComm, ModuleKind.RackTransfer);

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
        /// ユニット番号を取得します。
        /// </summary>
        private int getUnitNo(int FuncNo)
        {
            int UnitNo = 0;
            switch (FuncNo)
            {
                case (int)RackTransferSequence.ForkOperation:
                case (int)RackTransferSequence.SendRackCatch:
                case (int)RackTransferSequence.BackRackCatch:
                case (int)RackTransferSequence.SendRackRelease:
                case (int)RackTransferSequence.BackRackRelease:
                    UnitNo = (int)UnitNoList.RackPullin;
                    break;
                default:
                    UnitNo = (int)UnitNoList.RackFrame;
                    break;
            }

            return UnitNo;
        }

        /// <summary>
        /// 受信データ処理をおこないます。
        /// </summary>
        public override void SetResponse(CommCommandEventArgs comm)
        {
            switch (comm.Command.CommandId)
            {
                case (int)CommandKind.RackTransferCommand1039:
                    //ユニットテスト
                    switch (tabUnit.SelectedTab.Index)
                    {
                        case (int)MaintenanceTabIndex.Test:
                            string Result = comm.Command.CommandText.Remove(0, 20).Trim();

                            if (ComFunc.chkExistsResponse(MaintenanceMainNavi.RackAllUnits, (int)lbxtestSequence.SelectedValue))
                            {
                                Singleton<ResponseLog>.Instance.RepeatNo = int.Parse(lbltestNumberDsp.Text);
                                Singleton<ResponseLog>.Instance.WriteLog(Result);
                                Result = (txttestResponce.Text + Result + System.Environment.NewLine);     //画面の内容にレスポンスの内容を追記する形にする
                            }

                            txttestResponce.Text = Result;
                            txttestResponce.SelectionStart = txttestResponce.Text.Length;
                            txttestResponce.Focus();
                            txttestResponce.ScrollToCaret();
                            break;
                        case (int)MaintenanceTabIndex.MAdjust:
                            txtAdjustBC.Text += comm.Command.CommandText.Remove(0, 20).Trim() + System.Environment.NewLine;
                            WriteLog(comm.Command.CommandText.Remove(0, 20));
                            txtAdjustBC.SelectionStart = txtAdjustBC.Text.Length;
                            txtAdjustBC.Focus();
                            txtAdjustBC.ScrollToCaret();

                            break;
                    }
                    ResponseFlg = false;
                    break;
                case (int)CommandKind.RackTransferCommand1073:
                    //モーター微調整時
                    UpDownButtonEnable(true);
                    break;
                case (int)CommandKind.RackTransferCommand1080:
                    //モーター微調整開始
                    //パラメータに設定されている内容に対応するオフセットのみ変更対象とする
                    switch ((int)lbxAdjustSequence.SelectedValue)
                    {
                        case (int)MotorAdjustStopPosition.RackForkPosition:
                            gbxAdjustM1ForkYAxis.Enabled = false;
                            numAdjustM1ForkYAxisOffsetHome.Enabled = false;
                            numAdjustM1ForkYAxisOffsetHome.ForeColor = System.Drawing.Color.Black;
                            gbxAdjustM2ForkYAxis.Enabled = false;
                            numAdjustM2ForkYAxisOffsetHome.Enabled = false;
                            numAdjustM2ForkYAxisOffsetHome.ForeColor = System.Drawing.Color.Black;
                            gbxAdjustM3ForkYAxis.Enabled = false;
                            numAdjustM3ForkYAxisOffsetHome.Enabled = false;
                            numAdjustM3ForkYAxisOffsetHome.ForeColor = System.Drawing.Color.Black;
                            gbxAdjustM4ForkYAxis.Enabled = false;
                            numAdjustM4ForkYAxisOffsetHome.Enabled = false;
                            numAdjustM4ForkYAxisOffsetHome.ForeColor = System.Drawing.Color.Black;

                            switch ((int)cmbAdjustModule.Value)
                            {
                                case (int)RackTransferCmbValue.Module1:
                                    gbxAdjustM1ForkYAxis.Enabled = true;
                                    numAdjustM1ForkYAxisOffsetHome.Enabled = true;
                                    numAdjustM1ForkYAxisOffsetHome.ForeColor = System.Drawing.Color.OrangeRed;
                                    break;
                                case (int)RackTransferCmbValue.Module2:
                                    gbxAdjustM2ForkYAxis.Enabled = true;
                                    numAdjustM2ForkYAxisOffsetHome.Enabled = true;
                                    numAdjustM2ForkYAxisOffsetHome.ForeColor = System.Drawing.Color.OrangeRed;
                                    break;
                                case (int)RackTransferCmbValue.Module3:
                                    gbxAdjustM3ForkYAxis.Enabled = true;
                                    numAdjustM3ForkYAxisOffsetHome.Enabled = true;
                                    numAdjustM3ForkYAxisOffsetHome.ForeColor = System.Drawing.Color.OrangeRed;
                                    break;
                                case (int)RackTransferCmbValue.Module4:
                                    gbxAdjustM4ForkYAxis.Enabled = true;
                                    numAdjustM4ForkYAxisOffsetHome.Enabled = true;
                                    numAdjustM4ForkYAxisOffsetHome.ForeColor = System.Drawing.Color.OrangeRed;
                                    break;
                            }
                            break;
                    }

                    ResponseFlg = false;
                    break;
                case (int)CommandKind.RackTransferCommand1012:
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
                case (int)CommandKind.RackTransferCommand1081:
                    //モーター微調整終了
                    UnitTabEnable(true);

                    //モーター調整を終了するタイミングで、色を元に戻す
                    switch ((int)lbxAdjustSequence.SelectedValue)
                    {
                        case (int)MotorAdjustStopPosition.RackForkPosition:
                            gbxAdjustM1ForkYAxis.Enabled = true;
                            numAdjustM1ForkYAxisOffsetHome.Enabled = true;
                            numAdjustM1ForkYAxisOffsetHome.ForeColor = System.Drawing.Color.OrangeRed;
                            gbxAdjustM2ForkYAxis.Enabled = true;
                            numAdjustM2ForkYAxisOffsetHome.Enabled = true;
                            numAdjustM2ForkYAxisOffsetHome.ForeColor = System.Drawing.Color.OrangeRed;
                            gbxAdjustM3ForkYAxis.Enabled = true;
                            numAdjustM3ForkYAxisOffsetHome.Enabled = true;
                            numAdjustM3ForkYAxisOffsetHome.ForeColor = System.Drawing.Color.OrangeRed;
                            gbxAdjustM4ForkYAxis.Enabled = true;
                            numAdjustM4ForkYAxisOffsetHome.Enabled = true;
                            numAdjustM4ForkYAxisOffsetHome.ForeColor = System.Drawing.Color.OrangeRed;
                            break;
                    }

                    this.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// パラメータをすべて非活性にする
        /// </summary>
        private void ParametersAllFalse()
        {
            cmbtestFeederType.Enabled = false;
            cmbtestPosition.Enabled = false;
            cmbtestLoaderType.Enabled = false;
            gbxtestRotation.Enabled = false;
            cmbtestModule.Enabled = false;
            cmbtestForkOperationPosition.Enabled = false;
            numtestSamplePosition.Enabled = false;
            cmbtestPositionA.Enabled = false;
            cmbtestPositionB.Enabled = false;
            gbxtestForkPosition.Enabled = false;
            numtestInterval.Enabled = false;
        }

        /// <summary>
        /// 選択された機能番号に応じてパラメータを活性化する
        /// </summary>
        private void lbxtestSequence_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParametersAllFalse();
            switch (lbxtestSequence.SelectedValue)
            {
                case (int)RackTransferSequence.FeederOperation:
                    cmbtestFeederType.Enabled = true;
                    cmbtestPosition.Enabled = true;
                    break;
                case (int)RackTransferSequence.LoaderOperation:
                    cmbtestLoaderType.Enabled = true;
                    gbxtestRotation.Enabled = true;
                    break;
                case (int)RackTransferSequence.ForkOperation:
                    cmbtestModule.Enabled = true;
                    cmbtestForkOperationPosition.Enabled = true;
                    break;
                case (int)RackTransferSequence.SampleIDReading:
                case (int)RackTransferSequence.SampleCupTubeCheck:
                    numtestSamplePosition.Enabled = true;
                    break;
                case (int)RackTransferSequence.SendMoveStandby:
                case (int)RackTransferSequence.BackMoveStandby:
                    cmbtestPositionA.Enabled = true;
                    cmbtestPositionB.Enabled = true;
                    break;
                case (int)RackTransferSequence.SendRackCatch:
                case (int)RackTransferSequence.BackRackCatch:
                case (int)RackTransferSequence.SendRackRelease:
                case (int)RackTransferSequence.BackRackRelease:
                    cmbtestModule.Enabled = true;
                    gbxtestForkPosition.Enabled = true;
                    break;
                case (int)RackTransferSequence.RackCoverLED:
                    numtestInterval.Enabled = true;
                    break;
                case (int)RackTransferSequence.AllRackOperation:
                    cmbtestModule.Enabled = true;
                    break;
            }
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
        /// タブが切り替えられたときにメンテナンスメイン画面のコマンドバーの表示切り替えをおこないます。
        /// </summary>
        private void TabControl_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            ToolbarsControl();
            SetUnitMode(e.Tab.Index);
        }

        /// <summary>
        /// コンフィグパラメータ読み込み
        /// </summary>
        public override void ConfigParamLoad()
        {
            ParameterFilePreserve<CarisXConfigParameter> config = Singleton<ParameterFilePreserve<CarisXConfigParameter>>.Instance;
            configLoad(config);

            //ラック設置ローダー時間
            numconfInstallationLoaderTime.Value = config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferUnitConfigParam.InstallationLoaderTime;
            //再検ラック待機ローダー時間
            numconfRetestWaitLoaderTime.Value = config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferUnitConfigParam.RetestWaitLoaderTime;
            //ラック回収ローダー時間
            numconfCollectionLoaderTime.Value = config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferUnitConfigParam.CollectionLoaderTime;
            //ラック送りローダー時間
            numconfSendLoaderTime.Value = config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferUnitConfigParam.SendLoaderTime;
            //ラック戻りローダー時間
            numconfBackLoaderTime.Value = config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferUnitConfigParam.BackLoaderTime;
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

            CarisXConfigParameter.RackTransferUnitConfigParam ConfigParam = new CarisXConfigParameter.RackTransferUnitConfigParam();
            //ラック設置ローダー時間
            ConfigParam.InstallationLoaderTime = (Int32)numconfInstallationLoaderTime.Value;
            //再検ラック待機ローダー時間
            ConfigParam.RetestWaitLoaderTime = (Int32)numconfRetestWaitLoaderTime.Value;
            //ラック回収ローダー時間
            ConfigParam.CollectionLoaderTime = (Int32)numconfCollectionLoaderTime.Value;
            //ラック送りローダー時間
            ConfigParam.SendLoaderTime = (Int32)numconfSendLoaderTime.Value;
            //ラック戻りローダー時間
            ConfigParam.BackLoaderTime = (Int32)numconfBackLoaderTime.Value;

            config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferUnitConfigParam = ConfigParam;

            //ラックユニットパラメータコマンド
            SendParam(ConfigParam, ModuleKind.RackTransfer);

        }

        /// <summary>
        /// モーターパラメータ保存
        /// </summary>
        private void MotorParamSave(bool adjustSave)
        {
            //入力値チェック
            if (!CheckControls(tabUnit.Tabs[2].TabPage))
            {
                DlgMaintenance msg = new DlgMaintenance(Resources_Maintenance.DlgChkMsg, false);
                msg.ShowDialog();
                return;
            }

            RackTransferCommCommand_0071 Mparam;
            ParameterFilePreserve<CarisXMotorParameter> motor = Singleton<ParameterFilePreserve<CarisXMotorParameter>>.Instance;
            motorLoad(motor);

            if (!adjustSave)
            {
                //ラック搬送部送りX軸（モジュール１）
                Mparam = new RackTransferCommCommand_0071();
                Mparam.MotorNo = (int)MotorNoList.RackTransferSendingXAxisM1;

                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamM1SendXAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamM1SendXAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamM1SendXAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamM1SendXAxisConst.Value;

                //パラメータセット
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM1Param.MotorNo = Mparam.MotorNo;
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM1Param.MotorSpeed = Mparam.MotorSpeed;

                //モータパラメータ送信内容スプール
                SpoolParam(Mparam);


                //ラック搬送部戻りX軸（モジュール１）
                Mparam = new RackTransferCommCommand_0071();
                Mparam.MotorNo = (int)MotorNoList.RackTransferBackXAxisM1;

                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamM1BackXAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamM1BackXAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamM1BackXAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamM1BackXAxisConst.Value;

                //パラメータセット
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM1Param.MotorNo = Mparam.MotorNo;
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM1Param.MotorSpeed = Mparam.MotorSpeed;

                //モータパラメータ送信内容スプール
                SpoolParam(Mparam);
            }

            //ラック引込部Y軸（モジュール１）
            Mparam = new RackTransferCommCommand_0071();
            Mparam.MotorNo = (int)MotorNoList.RackPullinYAxisM1;
            if (!adjustSave)
            {
                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamM1ForkYAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamM1ForkYAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamM1ForkYAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamM1ForkYAxisConst.Value;

                Mparam.MotorOffset[0] = (double)numParamM1ForkYAxisOffsetHome.Value;

                //パラメータセット
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM1Param.MotorNo = Mparam.MotorNo;
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM1Param.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                Mparam.MotorSpeed = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM1Param.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustM1ForkYAxisOffsetHome.Value;
            }
            motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM1Param.OffsetHomePosition = Mparam.MotorOffset[0];
            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);


            if (!adjustSave)
            {
                //ラック搬送部送りX軸（モジュール２）
                Mparam = new RackTransferCommCommand_0071();
                Mparam.MotorNo = (int)MotorNoList.RackTransferSendingXAxisM2;

                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamM2SendXAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamM2SendXAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamM2SendXAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamM2SendXAxisConst.Value;

                //パラメータセット
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM2Param.MotorNo = Mparam.MotorNo;
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM2Param.MotorSpeed = Mparam.MotorSpeed;

                //モータパラメータ送信内容スプール
                SpoolParam(Mparam);


                //ラック搬送部戻りX軸（モジュール２）
                Mparam = new RackTransferCommCommand_0071();
                Mparam.MotorNo = (int)MotorNoList.RackTransferBackXAxisM2;

                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamM2BackXAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamM2BackXAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamM2BackXAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamM2BackXAxisConst.Value;

                //パラメータセット
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM2Param.MotorNo = Mparam.MotorNo;
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM2Param.MotorSpeed = Mparam.MotorSpeed;

                //モータパラメータ送信内容スプール
                SpoolParam(Mparam);
            }


            //ラック引込部Y軸（モジュール２）
            Mparam = new RackTransferCommCommand_0071();
            Mparam.MotorNo = (int)MotorNoList.RackPullinYAxisM2;
            if (!adjustSave)
            {
                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamM2ForkYAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamM2ForkYAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamM2ForkYAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamM2ForkYAxisConst.Value;

                Mparam.MotorOffset[0] = (double)numParamM2ForkYAxisOffsetHome.Value;

                //パラメータセット
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM2Param.MotorNo = Mparam.MotorNo;
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM2Param.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                Mparam.MotorSpeed = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM2Param.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustM2ForkYAxisOffsetHome.Value;
            }
            motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM2Param.OffsetHomePosition = Mparam.MotorOffset[0];
            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);


            if (!adjustSave)
            {
                //ラック搬送部送りX軸（モジュール３）
                Mparam = new RackTransferCommCommand_0071();
                Mparam.MotorNo = (int)MotorNoList.RackTransferSendingXAxisM3;

                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamM3SendXAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamM3SendXAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamM3SendXAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamM3SendXAxisConst.Value;

                //パラメータセット
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM3Param.MotorNo = Mparam.MotorNo;
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM3Param.MotorSpeed = Mparam.MotorSpeed;

                //モータパラメータ送信内容スプール
                SpoolParam(Mparam);


                //ラック搬送部戻りX軸（モジュール３）
                Mparam = new RackTransferCommCommand_0071();
                Mparam.MotorNo = (int)MotorNoList.RackTransferBackXAxisM3;

                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamM3BackXAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamM3BackXAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamM3BackXAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamM3BackXAxisConst.Value;

                //パラメータセット
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM3Param.MotorNo = Mparam.MotorNo;
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM3Param.MotorSpeed = Mparam.MotorSpeed;

                //モータパラメータ送信内容スプール
                SpoolParam(Mparam);
            }


            //ラック引込部Y軸（モジュール３）
            Mparam = new RackTransferCommCommand_0071();
            Mparam.MotorNo = (int)MotorNoList.RackPullinYAxisM3;
            if (!adjustSave)
            {
                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamM3ForkYAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamM3ForkYAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamM3ForkYAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamM3ForkYAxisConst.Value;

                Mparam.MotorOffset[0] = (double)numParamM3ForkYAxisOffsetHome.Value;

                //パラメータセット
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM3Param.MotorNo = Mparam.MotorNo;
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM3Param.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                Mparam.MotorSpeed = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM3Param.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustM3ForkYAxisOffsetHome.Value;
            }
            motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM3Param.OffsetHomePosition = Mparam.MotorOffset[0];
            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);


            if (!adjustSave)
            {
                //ラック搬送部送りX軸（モジュール４）
                Mparam = new RackTransferCommCommand_0071();
                Mparam.MotorNo = (int)MotorNoList.RackTransferSendingXAxisM4;

                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamM4SendXAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamM4SendXAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamM4SendXAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamM4SendXAxisConst.Value;

                //パラメータセット
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM4Param.MotorNo = Mparam.MotorNo;
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM4Param.MotorSpeed = Mparam.MotorSpeed;

                //モータパラメータ送信内容スプール
                SpoolParam(Mparam);


                //ラック搬送部戻りX軸（モジュール４）
                Mparam = new RackTransferCommCommand_0071();
                Mparam.MotorNo = (int)MotorNoList.RackTransferBackXAxisM4;

                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamM4BackXAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamM4BackXAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamM4BackXAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamM4BackXAxisConst.Value;

                //パラメータセット
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM4Param.MotorNo = Mparam.MotorNo;
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM4Param.MotorSpeed = Mparam.MotorSpeed;

                //モータパラメータ送信内容スプール
                SpoolParam(Mparam);
            }


            //ラック引込部Y軸（モジュール４）
            Mparam = new RackTransferCommCommand_0071();
            Mparam.MotorNo = (int)MotorNoList.RackPullinYAxisM4;
            if (!adjustSave)
            {
                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamM4ForkYAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamM4ForkYAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamM4ForkYAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamM4ForkYAxisConst.Value;

                Mparam.MotorOffset[0] = (double)numParamM4ForkYAxisOffsetHome.Value;

                //パラメータセット
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM4Param.MotorNo = Mparam.MotorNo;
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM4Param.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                Mparam.MotorSpeed = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM4Param.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustM4ForkYAxisOffsetHome.Value;
            }
            motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM4Param.OffsetHomePosition = Mparam.MotorOffset[0];
            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);


            if (!adjustSave)
            {
                //ラック架設部　ラック設置Y軸
                Mparam = new RackTransferCommCommand_0071();
                Mparam.MotorNo = (int)MotorNoList.RackSetLoadYAxis;

                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamLoadYAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamLoadYAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamLoadYAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamLoadYAxisConst.Value;

                //パラメータセット
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadYAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadYAxisParam.MotorSpeed = Mparam.MotorSpeed;

                //モータパラメータ送信内容スプール
                SpoolParam(Mparam);


                //ラック架設部　再検ラック待機Y軸
                Mparam = new RackTransferCommCommand_0071();
                Mparam.MotorNo = (int)MotorNoList.RackSetUnLoadYAxis;

                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamUnLoadYAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamUnLoadYAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamUnLoadYAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamUnLoadYAxisConst.Value;

                //パラメータセット
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadYAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadYAxisParam.MotorSpeed = Mparam.MotorSpeed;

                //モータパラメータ送信内容スプール
                SpoolParam(Mparam);


                //ラック架設部　ラック回収Y軸
                Mparam = new RackTransferCommCommand_0071();
                Mparam.MotorNo = (int)MotorNoList.RackSetTakeOutYAxis;

                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamTakeOutYAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamTakeOutYAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamTakeOutYAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamTakeOutYAxisConst.Value;

                //パラメータセット
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetTakeOutYAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetTakeOutYAxisParam.MotorSpeed = Mparam.MotorSpeed;

                //モータパラメータ送信内容スプール
                SpoolParam(Mparam);
            }


            //ラック架設部　ラックフィーダX軸
            Mparam = new RackTransferCommCommand_0071();
            Mparam.MotorNo = (int)MotorNoList.RackSetLoadFeederXAxis;
            if (!adjustSave)
            {
                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamLoadFeederXAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamLoadFeederXAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamLoadFeederXAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamLoadFeederXAxisConst.Value;

                Mparam.MotorOffset[0] = (double)numParamLoadFeederXAxisOffsetTakeOut.Value;
                Mparam.MotorOffset[1] = (double)numParamLoadFeederXAxisOffsetLoad.Value;
                Mparam.MotorOffset[2] = (double)numParamLoadFeederXAxisOffsetTubeSensor.Value;
                Mparam.MotorOffset[3] = (double)numParamLoadFeederXAxisOffsetSampleIDRead.Value;
                Mparam.MotorOffset[4] = (double)numParamLoadFeederXAxisOffsetRackIDRead.Value;

                //パラメータセット
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                Mparam.MotorSpeed = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustLoadFeederXAxisOffsetTakeOut.Value;
                Mparam.MotorOffset[1] = (double)numAdjustLoadFeederXAxisOffsetLoad.Value;
                Mparam.MotorOffset[2] = (double)numAdjustLoadFeederXAxisOffsetTubeSensor.Value;
                Mparam.MotorOffset[3] = (double)numAdjustLoadFeederXAxisOffsetSampleIDRead.Value;
                Mparam.MotorOffset[4] = (double)numAdjustLoadFeederXAxisOffsetRackIDRead.Value;
            }
            motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.OffsetRackTakeout = Mparam.MotorOffset[0];
            motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.OffsetRackLoad = Mparam.MotorOffset[1];
            motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.OffsetTubeSensorReading = Mparam.MotorOffset[2];
            motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.OffsetSampleIDReading = Mparam.MotorOffset[3];
            motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.OffsetRackIDReading = Mparam.MotorOffset[4];
            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);


            //ラック架設部　再検ラックフィーダX軸
            Mparam = new RackTransferCommCommand_0071();
            Mparam.MotorNo = (int)MotorNoList.RackSetUnLoadFeederXAxis;
            if (!adjustSave)
            {
                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamUnLoadFeederXAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamUnLoadFeederXAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamUnLoadFeederXAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamUnLoadFeederXAxisConst.Value;

                Mparam.MotorOffset[0] = (double)numParamUnLoadFeederXAxisOffsetTakeOut.Value;
                Mparam.MotorOffset[1] = (double)numParamUnLoadFeederXAxisOffsetRetest.Value;
                Mparam.MotorOffset[2] = (double)numParamUnLoadFeederXAxisOffsetUnLoad.Value;

                //パラメータセット
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadFeederXAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadFeederXAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                Mparam.MotorSpeed = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadFeederXAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustUnLoadFeederXAxisOffsetTakeOut.Value;
                Mparam.MotorOffset[1] = (double)numAdjustUnLoadFeederXAxisOffsetRetest.Value;
                Mparam.MotorOffset[2] = (double)numAdjustUnLoadFeederXAxisOffsetUnLoad.Value;
            }
            motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadFeederXAxisParam.OffsetRackTakeout = Mparam.MotorOffset[0];
            motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadFeederXAxisParam.OffsetRackRetest = Mparam.MotorOffset[1];
            motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadFeederXAxisParam.OffsetRackUnLoad = Mparam.MotorOffset[2];
            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);


            //ラック架設部　ラックスライダーX軸
            Mparam = new RackTransferCommCommand_0071();
            Mparam.MotorNo = (int)MotorNoList.RackSetSliderXAxis;
            if (!adjustSave)
            {
                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamSliderXAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamSliderXAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamSliderXAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamSliderXAxisConst.Value;

                Mparam.MotorOffset[0] = (double)numParamSliderXAxisOffsetLoad.Value;
                Mparam.MotorOffset[1] = (double)numParamSliderXAxisOffsetUnLoad.Value;

                //パラメータセット
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetSliderXAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetSliderXAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                Mparam.MotorSpeed = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetSliderXAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustSliderXAxisOffsetLoad.Value;
                Mparam.MotorOffset[1] = (double)numAdjustSliderXAxisOffsetUnLoad.Value;
            }
            motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetSliderXAxisParam.OffsetRackLoad = Mparam.MotorOffset[0];
            motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetSliderXAxisParam.OffsetRackUnLoad = Mparam.MotorOffset[1];
            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);


            //モータパラメータ送信
            SendParam(ModuleKind.RackTransfer);

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

            //ラック搬送部送りX軸（モジュール１）
            numParamM1SendXAxisInit.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM1Param.MotorSpeed[0].InitSpeed;
            numParamM1SendXAxisTop.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM1Param.MotorSpeed[0].TopSpeed;
            numParamM1SendXAxisAccelerator.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM1Param.MotorSpeed[0].Accel;
            numParamM1SendXAxisConst.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM1Param.MotorSpeed[0].ConstSpeed;

            //ラック搬送部戻りX軸（モジュール１）
            numParamM1BackXAxisInit.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM1Param.MotorSpeed[0].InitSpeed;
            numParamM1BackXAxisTop.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM1Param.MotorSpeed[0].TopSpeed;
            numParamM1BackXAxisAccelerator.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM1Param.MotorSpeed[0].Accel;
            numParamM1BackXAxisConst.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM1Param.MotorSpeed[0].ConstSpeed;

            //ラック引込部Y軸（モジュール１）
            numParamM1ForkYAxisInit.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM1Param.MotorSpeed[0].InitSpeed;
            numParamM1ForkYAxisTop.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM1Param.MotorSpeed[0].TopSpeed;
            numParamM1ForkYAxisAccelerator.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM1Param.MotorSpeed[0].Accel;
            numParamM1ForkYAxisConst.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM1Param.MotorSpeed[0].ConstSpeed;
            numParamM1ForkYAxisOffsetHome.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM1Param.OffsetHomePosition;

            numAdjustM1ForkYAxisOffsetHome.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM1Param.OffsetHomePosition;


            //ラック搬送部送りX軸（モジュール２）
            numParamM2SendXAxisInit.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM2Param.MotorSpeed[0].InitSpeed;
            numParamM2SendXAxisTop.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM2Param.MotorSpeed[0].TopSpeed;
            numParamM2SendXAxisAccelerator.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM2Param.MotorSpeed[0].Accel;
            numParamM2SendXAxisConst.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM2Param.MotorSpeed[0].ConstSpeed;

            //ラック搬送部戻りX軸（モジュール２）
            numParamM2BackXAxisInit.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM2Param.MotorSpeed[0].InitSpeed;
            numParamM2BackXAxisTop.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM2Param.MotorSpeed[0].TopSpeed;
            numParamM2BackXAxisAccelerator.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM2Param.MotorSpeed[0].Accel;
            numParamM2BackXAxisConst.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM2Param.MotorSpeed[0].ConstSpeed;

            //ラック引込部Y軸（モジュール２）
            numParamM2ForkYAxisInit.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM2Param.MotorSpeed[0].InitSpeed;
            numParamM2ForkYAxisTop.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM2Param.MotorSpeed[0].TopSpeed;
            numParamM2ForkYAxisAccelerator.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM2Param.MotorSpeed[0].Accel;
            numParamM2ForkYAxisConst.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM2Param.MotorSpeed[0].ConstSpeed;
            numParamM2ForkYAxisOffsetHome.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM2Param.OffsetHomePosition;

            numAdjustM2ForkYAxisOffsetHome.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM2Param.OffsetHomePosition;


            //ラック搬送部送りX軸（モジュール３）
            numParamM3SendXAxisInit.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM3Param.MotorSpeed[0].InitSpeed;
            numParamM3SendXAxisTop.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM3Param.MotorSpeed[0].TopSpeed;
            numParamM3SendXAxisAccelerator.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM3Param.MotorSpeed[0].Accel;
            numParamM3SendXAxisConst.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM3Param.MotorSpeed[0].ConstSpeed;

            //ラック搬送部戻りX軸（モジュール３）
            numParamM3BackXAxisInit.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM3Param.MotorSpeed[0].InitSpeed;
            numParamM3BackXAxisTop.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM3Param.MotorSpeed[0].TopSpeed;
            numParamM3BackXAxisAccelerator.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM3Param.MotorSpeed[0].Accel;
            numParamM3BackXAxisConst.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM3Param.MotorSpeed[0].ConstSpeed;

            //ラック引込部Y軸（モジュール３）
            numParamM3ForkYAxisInit.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM3Param.MotorSpeed[0].InitSpeed;
            numParamM3ForkYAxisTop.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM3Param.MotorSpeed[0].TopSpeed;
            numParamM3ForkYAxisAccelerator.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM3Param.MotorSpeed[0].Accel;
            numParamM3ForkYAxisConst.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM3Param.MotorSpeed[0].ConstSpeed;
            numParamM3ForkYAxisOffsetHome.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM3Param.OffsetHomePosition;

            numAdjustM3ForkYAxisOffsetHome.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM3Param.OffsetHomePosition;


            //ラック搬送部送りX軸（モジュール４）
            numParamM4SendXAxisInit.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM4Param.MotorSpeed[0].InitSpeed;
            numParamM4SendXAxisTop.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM4Param.MotorSpeed[0].TopSpeed;
            numParamM4SendXAxisAccelerator.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM4Param.MotorSpeed[0].Accel;
            numParamM4SendXAxisConst.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM4Param.MotorSpeed[0].ConstSpeed;

            //ラック搬送部戻りX軸（モジュール４）
            numParamM4BackXAxisInit.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM4Param.MotorSpeed[0].InitSpeed;
            numParamM4BackXAxisTop.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM4Param.MotorSpeed[0].TopSpeed;
            numParamM4BackXAxisAccelerator.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM4Param.MotorSpeed[0].Accel;
            numParamM4BackXAxisConst.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM4Param.MotorSpeed[0].ConstSpeed;

            //ラック引込部Y軸（モジュール４）
            numParamM4ForkYAxisInit.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM4Param.MotorSpeed[0].InitSpeed;
            numParamM4ForkYAxisTop.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM4Param.MotorSpeed[0].TopSpeed;
            numParamM4ForkYAxisAccelerator.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM4Param.MotorSpeed[0].Accel;
            numParamM4ForkYAxisConst.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM4Param.MotorSpeed[0].ConstSpeed;
            numParamM4ForkYAxisOffsetHome.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM4Param.OffsetHomePosition;

            numAdjustM4ForkYAxisOffsetHome.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM4Param.OffsetHomePosition;


            //ラック架設部　ラック設置Y軸
            numParamLoadYAxisInit.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadYAxisParam.MotorSpeed[0].InitSpeed;
            numParamLoadYAxisTop.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadYAxisParam.MotorSpeed[0].TopSpeed;
            numParamLoadYAxisAccelerator.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadYAxisParam.MotorSpeed[0].Accel;
            numParamLoadYAxisConst.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadYAxisParam.MotorSpeed[0].ConstSpeed;


            //ラック架設部　再検ラック待機Y軸
            numParamUnLoadYAxisInit.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadYAxisParam.MotorSpeed[0].InitSpeed;
            numParamUnLoadYAxisTop.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadYAxisParam.MotorSpeed[0].TopSpeed;
            numParamUnLoadYAxisAccelerator.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadYAxisParam.MotorSpeed[0].Accel;
            numParamUnLoadYAxisConst.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadYAxisParam.MotorSpeed[0].ConstSpeed;


            //ラック架設部　ラック回収Y軸
            numParamTakeOutYAxisInit.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetTakeOutYAxisParam.MotorSpeed[0].InitSpeed;
            numParamTakeOutYAxisTop.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetTakeOutYAxisParam.MotorSpeed[0].TopSpeed;
            numParamTakeOutYAxisAccelerator.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetTakeOutYAxisParam.MotorSpeed[0].Accel;
            numParamTakeOutYAxisConst.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetTakeOutYAxisParam.MotorSpeed[0].ConstSpeed;


            //ラック架設部　ラックフィーダX軸
            numParamLoadFeederXAxisInit.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.MotorSpeed[0].InitSpeed;
            numParamLoadFeederXAxisTop.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.MotorSpeed[0].TopSpeed;
            numParamLoadFeederXAxisAccelerator.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.MotorSpeed[0].Accel;
            numParamLoadFeederXAxisConst.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.MotorSpeed[0].ConstSpeed;
            numParamLoadFeederXAxisOffsetTakeOut.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.OffsetRackTakeout;
            numParamLoadFeederXAxisOffsetLoad.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.OffsetRackLoad;
            numParamLoadFeederXAxisOffsetTubeSensor.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.OffsetTubeSensorReading;
            numParamLoadFeederXAxisOffsetSampleIDRead.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.OffsetSampleIDReading;
            numParamLoadFeederXAxisOffsetRackIDRead.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.OffsetRackIDReading;

            numAdjustLoadFeederXAxisOffsetTakeOut.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.OffsetRackTakeout;
            numAdjustLoadFeederXAxisOffsetLoad.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.OffsetRackLoad;
            numAdjustLoadFeederXAxisOffsetTubeSensor.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.OffsetTubeSensorReading;
            numAdjustLoadFeederXAxisOffsetSampleIDRead.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.OffsetSampleIDReading;
            numAdjustLoadFeederXAxisOffsetRackIDRead.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.OffsetRackIDReading;


            //ラック架設部　再検ラックフィーダX軸
            numParamUnLoadFeederXAxisInit.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadFeederXAxisParam.MotorSpeed[0].InitSpeed;
            numParamUnLoadFeederXAxisTop.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadFeederXAxisParam.MotorSpeed[0].TopSpeed;
            numParamUnLoadFeederXAxisAccelerator.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadFeederXAxisParam.MotorSpeed[0].Accel;
            numParamUnLoadFeederXAxisConst.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadFeederXAxisParam.MotorSpeed[0].ConstSpeed;
            numParamUnLoadFeederXAxisOffsetTakeOut.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadFeederXAxisParam.OffsetRackTakeout;
            numParamUnLoadFeederXAxisOffsetRetest.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadFeederXAxisParam.OffsetRackRetest;
            numParamUnLoadFeederXAxisOffsetUnLoad.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadFeederXAxisParam.OffsetRackUnLoad;

            numAdjustUnLoadFeederXAxisOffsetTakeOut.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadFeederXAxisParam.OffsetRackTakeout;
            numAdjustUnLoadFeederXAxisOffsetRetest.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadFeederXAxisParam.OffsetRackRetest;
            numAdjustUnLoadFeederXAxisOffsetUnLoad.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadFeederXAxisParam.OffsetRackUnLoad;


            //ラック架設部　ラックスライダーX軸
            numParamSliderXAxisInit.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetSliderXAxisParam.MotorSpeed[0].InitSpeed;
            numParamSliderXAxisTop.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetSliderXAxisParam.MotorSpeed[0].TopSpeed;
            numParamSliderXAxisAccelerator.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetSliderXAxisParam.MotorSpeed[0].Accel;
            numParamSliderXAxisConst.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetSliderXAxisParam.MotorSpeed[0].ConstSpeed;
            numParamSliderXAxisOffsetLoad.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetSliderXAxisParam.OffsetRackLoad;
            numParamSliderXAxisOffsetUnLoad.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetSliderXAxisParam.OffsetRackUnLoad;

            numAdjustSliderXAxisOffsetLoad.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetSliderXAxisParam.OffsetRackLoad;
            numAdjustSliderXAxisOffsetUnLoad.Value = motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetSliderXAxisParam.OffsetRackUnLoad;
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
            gbxAdjustBC.Enabled = false;
            gbxAdjustM1ForkYAxis.Enabled = false;
            gbxAdjustM2ForkYAxis.Enabled = false;
            gbxAdjustM3ForkYAxis.Enabled = false;
            gbxAdjustM4ForkYAxis.Enabled = false;
            gbxAdjustLoadFeederXAxis.Enabled = false;
            gbxAdjustUnLoadFeederXAxis.Enabled = false;
            gbxAdjustSliderXAxis.Enabled = false;

            cmbAdjustModule.Enabled = false;
            cmbAdjustSamplePosition.Enabled = false;
            numAdjustBCRread.Enabled = false;

            //PullInYAxis
            numAdjustM1ForkYAxisOffsetHome.Enabled = false;
            numAdjustM2ForkYAxisOffsetHome.Enabled = false;
            numAdjustM3ForkYAxisOffsetHome.Enabled = false;
            numAdjustM4ForkYAxisOffsetHome.Enabled = false;

            //LoadFeederXAxis
            numAdjustLoadFeederXAxisOffsetTakeOut.Enabled = false;
            numAdjustLoadFeederXAxisOffsetLoad.Enabled = false;
            numAdjustLoadFeederXAxisOffsetTubeSensor.Enabled = false;
            numAdjustLoadFeederXAxisOffsetSampleIDRead.Enabled = false;
            numAdjustLoadFeederXAxisOffsetRackIDRead.Enabled = false;

            //UnLoadFeederXAxis
            numAdjustUnLoadFeederXAxisOffsetTakeOut.Enabled = false;
            numAdjustUnLoadFeederXAxisOffsetRetest.Enabled = false;
            numAdjustUnLoadFeederXAxisOffsetUnLoad.Enabled = false;

            //SliderXAxis
            numAdjustSliderXAxisOffsetLoad.Enabled = false;
            numAdjustSliderXAxisOffsetUnLoad.Enabled = false;

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

            RackTransferCommCommand_0080 AdjustStartComm = new RackTransferCommCommand_0080();

            Func<int> getStopPos = () => { return (int)lbxAdjustSequence.SelectedValue; };
            Func<int> module = () => { return (int)cmbAdjustModule.Value; };
            Func<int> samplepos = () => { return (int)cmbAdjustSamplePosition.Value; };

            AdjustStartComm.Pos = (int)Invoke(getStopPos);
            switch (AdjustStartComm.Pos)
            {
                case (int)MotorAdjustStopPosition.RackForkPosition:
                case (int)MotorAdjustStopPosition.RackForkPosition2:
                    AdjustStartComm.Arg1 = (int)Invoke(module);
                    break;
                case (int)MotorAdjustStopPosition.LoadFeederSampleIDReading:
                case (int)MotorAdjustStopPosition.LoadFeederTubeSensorReading:
                    AdjustStartComm.Arg1 = (int)Invoke(samplepos);
                    break;
                case (int)MotorAdjustStopPosition.LoadFeederRackLoad:
                case (int)MotorAdjustStopPosition.LoadFeederRackReturnFeeder:
                case (int)MotorAdjustStopPosition.LoadFeederRackIDReading:
                case (int)MotorAdjustStopPosition.SliderRackLoad:
                case (int)MotorAdjustStopPosition.SliderRackUnLoad:
                case (int)MotorAdjustStopPosition.UnLoadFeederRackUnLoad:
                case (int)MotorAdjustStopPosition.UnLoadFeederRackRetest:
                case (int)MotorAdjustStopPosition.UnLoadFeederRackTakeout:
                    break;
            }

            // 受信待ちフラグを設定
            ResponseFlg = true;

            // コマンド送信
            unitStartAdjust.Start(AdjustStartComm, ModuleKind.RackTransfer);

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
                    UnitAbort(ModuleKind.RackTransfer);
                    break;
                case (int)MaintenanceTabIndex.MAdjust:
                    if (DialogResult.OK != GUI.DlgMessage.Show(Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_SAVEMASSAGE
                        , String.Empty, Properties.Resources.STRING_DLG_TITLE_001, GUI.MessageDialogButtons.OKCancel))
                    {
                        return (false);
                    }
                    UpDownButtonEnable(false);
                    MotorParamDisp();

                    RackTransferCommCommand_0081 AdjustStartComm = new RackTransferCommCommand_0081();
                    Func<int> getStopPos = () => { return (int)lbxAdjustSequence.SelectedValue; };
                    AdjustStartComm.Pos = (int)Invoke(getStopPos);

                    unitAdjustAbort.Start(AdjustStartComm, ModuleKind.RackTransfer);
                    break;
            }

            return (true);
        }

        /// <summary>
        /// UP/DownボタンをEnable/Disableにする
        /// </summary>
        public override void UpDownButtonEnable(bool enable)
        {
            btnAdjustM1ForkYAxisForward.Enabled = enable;
            btnAdjustM1ForkYAxisBack.Enabled = enable;
            btnAdjustM2ForkYAxisForward.Enabled = enable;
            btnAdjustM2ForkYAxisBack.Enabled = enable;
            btnAdjustM3ForkYAxisForward.Enabled = enable;
            btnAdjustM3ForkYAxisBack.Enabled = enable;
            btnAdjustM4ForkYAxisForward.Enabled = enable;
            btnAdjustM4ForkYAxisBack.Enabled = enable;
            btnAdjustLoadFeederXAxisLeft.Enabled = enable;
            btnAdjustLoadFeederXAxisRight.Enabled = enable;
            btnAdjustUnLoadFeederXAxisLeft.Enabled = enable;
            btnAdjustUnLoadFeederXAxisRight.Enabled = enable;
            btnAdjustSliderXAxisLeft.Enabled = enable;
            btnAdjustSliderXAxisRight.Enabled = enable;
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
        /// 微調整シーケンスセレクトイベント
        /// </summary>
        private void lbxAdjustSequence_SelectedIndexChanged(object sender, EventArgs e)
        {
            adjustGbAllDisable();
            //モータ調整ボタンDisable
            UpDownButtonEnable(false);
            switch ((int)lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.RackForkPosition:
                case (int)MotorAdjustStopPosition.RackForkPosition2:
                    cmbAdjustModule.Enabled = true;
                    gbxAdjustM1ForkYAxis.Enabled = true;
                    numAdjustM1ForkYAxisOffsetHome.Enabled = true;
                    numAdjustM1ForkYAxisOffsetHome.ForeColor = System.Drawing.Color.OrangeRed;
                    gbxAdjustM2ForkYAxis.Enabled = true;
                    numAdjustM2ForkYAxisOffsetHome.Enabled = true;
                    numAdjustM2ForkYAxisOffsetHome.ForeColor = System.Drawing.Color.OrangeRed;
                    gbxAdjustM3ForkYAxis.Enabled = true;
                    numAdjustM3ForkYAxisOffsetHome.Enabled = true;
                    numAdjustM3ForkYAxisOffsetHome.ForeColor = System.Drawing.Color.OrangeRed;
                    gbxAdjustM4ForkYAxis.Enabled = true;
                    numAdjustM4ForkYAxisOffsetHome.Enabled = true;
                    numAdjustM4ForkYAxisOffsetHome.ForeColor = System.Drawing.Color.OrangeRed;
                    break;
                case (int)MotorAdjustStopPosition.LoadFeederRackLoad:
                    gbxAdjustLoadFeederXAxis.Enabled = true;
                    numAdjustLoadFeederXAxisOffsetLoad.Enabled = true;
                    numAdjustLoadFeederXAxisOffsetLoad.ForeColor = System.Drawing.Color.OrangeRed;
                    break;
                case (int)MotorAdjustStopPosition.LoadFeederRackReturnFeeder:
                    gbxAdjustLoadFeederXAxis.Enabled = true;
                    numAdjustLoadFeederXAxisOffsetTakeOut.Enabled = true;
                    numAdjustLoadFeederXAxisOffsetTakeOut.ForeColor = System.Drawing.Color.OrangeRed;
                    break;
                case (int)MotorAdjustStopPosition.LoadFeederRackIDReading:
                    numAdjustBCRread.Enabled = true;
                    gbxAdjustLoadFeederXAxis.Enabled = true;
                    numAdjustLoadFeederXAxisOffsetRackIDRead.Enabled = true;
                    numAdjustLoadFeederXAxisOffsetRackIDRead.ForeColor = System.Drawing.Color.OrangeRed;
                    gbxAdjustBC.Enabled = true;
                    btnAdjustRackIDRead.Enabled = true;
                    btnAdjustBCRead.Enabled = true;
                    btnAdjustTubeSensorRead.Enabled = true;
                    break;
                case (int)MotorAdjustStopPosition.LoadFeederSampleIDReading:
                    cmbAdjustSamplePosition.Enabled = true;
                    numAdjustBCRread.Enabled = true;
                    gbxAdjustLoadFeederXAxis.Enabled = true;
                    numAdjustLoadFeederXAxisOffsetSampleIDRead.Enabled = true;
                    numAdjustLoadFeederXAxisOffsetSampleIDRead.ForeColor = System.Drawing.Color.OrangeRed;
                    gbxAdjustBC.Enabled = true;
                    btnAdjustRackIDRead.Enabled = true;
                    btnAdjustBCRead.Enabled = true;
                    btnAdjustTubeSensorRead.Enabled = true;
                    break;
                case (int)MotorAdjustStopPosition.LoadFeederTubeSensorReading:
                    cmbAdjustSamplePosition.Enabled = true;
                    numAdjustBCRread.Enabled = true;
                    gbxAdjustLoadFeederXAxis.Enabled = true;
                    numAdjustLoadFeederXAxisOffsetTubeSensor.Enabled = true;
                    numAdjustLoadFeederXAxisOffsetTubeSensor.ForeColor = System.Drawing.Color.OrangeRed;
                    gbxAdjustBC.Enabled = true;
                    btnAdjustRackIDRead.Enabled = true;
                    btnAdjustBCRead.Enabled = true;
                    btnAdjustTubeSensorRead.Enabled = true;
                    break;
                case (int)MotorAdjustStopPosition.SliderRackLoad:
                    gbxAdjustSliderXAxis.Enabled = true;
                    numAdjustSliderXAxisOffsetLoad.Enabled = true;
                    numAdjustSliderXAxisOffsetLoad.ForeColor = System.Drawing.Color.OrangeRed;
                    break;
                case (int)MotorAdjustStopPosition.SliderRackUnLoad:
                    gbxAdjustSliderXAxis.Enabled = true;
                    numAdjustSliderXAxisOffsetUnLoad.Enabled = true;
                    numAdjustSliderXAxisOffsetUnLoad.ForeColor = System.Drawing.Color.OrangeRed;
                    break;
                case (int)MotorAdjustStopPosition.UnLoadFeederRackUnLoad:
                    gbxAdjustUnLoadFeederXAxis.Enabled = true;
                    numAdjustUnLoadFeederXAxisOffsetUnLoad.Enabled = true;
                    numAdjustUnLoadFeederXAxisOffsetUnLoad.ForeColor = System.Drawing.Color.OrangeRed;
                    break;
                case (int)MotorAdjustStopPosition.UnLoadFeederRackRetest:
                    gbxAdjustUnLoadFeederXAxis.Enabled = true;
                    numAdjustUnLoadFeederXAxisOffsetRetest.Enabled = true;
                    numAdjustUnLoadFeederXAxisOffsetRetest.ForeColor = System.Drawing.Color.OrangeRed;
                    break;
                case (int)MotorAdjustStopPosition.UnLoadFeederRackTakeout:
                    gbxAdjustUnLoadFeederXAxis.Enabled = true;
                    numAdjustUnLoadFeederXAxisOffsetTakeOut.Enabled = true;
                    numAdjustUnLoadFeederXAxisOffsetTakeOut.ForeColor = System.Drawing.Color.OrangeRed;
                    break;
            }
        }

        /// <summary>
        /// ラックID読込
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdjustRackIDRead_Click(object sender, EventArgs e)
        {
            btnAdjustRackIDRead.Enabled = false;

            RackTransferCommCommand_0039 StartComm = new RackTransferCommCommand_0039();
            StartComm.UnitNo = (int)UnitNoList.RackFrame;
            StartComm.FuncNo = (int)RackTransferSequence.RackIDReading;
            StartComm.Arg1 = 1;                                         //「1：移動動作なし(BCRのみ読み込み)」を設定

            for (int i = 0; i < (int)numAdjustBCRread.Value; i++)
            {
                // 受信待ちフラグを設定
                ResponseFlg = true;

                // コマンド送信
                unitStart.Start(StartComm, ModuleKind.RackTransfer);

                // 応答コマンド受信待ち
                while (ResponseFlg)
                {
                    Application.DoEvents();
                }
            }

            btnAdjustRackIDRead.Enabled = true;
        }

        /// <summary>
        /// 検体ID読込
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdjustBCRead_Click(object sender, EventArgs e)
        {
            btnAdjustBCRead.Enabled = false;

            RackTransferCommCommand_0039 StartComm = new RackTransferCommCommand_0039();
            StartComm.UnitNo = (int)UnitNoList.RackFrame;
            StartComm.FuncNo = (int)RackTransferSequence.SampleIDReading;
            StartComm.Arg1 = 0;                                         //「0:移動動作なし(BCRのみ読み込み)」を設定

            for (int i = 0; i < (int)numAdjustBCRread.Value; i++)
            {
                // 受信待ちフラグを設定
                ResponseFlg = true;

                // コマンド送信
                unitStart.Start(StartComm, ModuleKind.RackTransfer);

                // 応答コマンド受信待ち
                while (ResponseFlg)
                {
                    Application.DoEvents();
                }
            }

            btnAdjustBCRead.Enabled = true;
        }

        /// <summary>
        /// チューブタイプ検出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdjustTubeSensorRead_Click(object sender, EventArgs e)
        {
            btnAdjustTubeSensorRead.Enabled = false;

            RackTransferCommCommand_0039 StartComm = new RackTransferCommCommand_0039();
            StartComm.UnitNo = (int)UnitNoList.RackFrame;
            StartComm.FuncNo = (int)RackTransferSequence.SampleCupTubeCheck;
            StartComm.Arg1 = 0;                                         //「0:移動動作なし(チューブタイプ検出のみ)」を設定

            for (int i = 0; i < (int)numAdjustBCRread.Value; i++)
            {
                // 受信待ちフラグを設定
                ResponseFlg = true;

                // コマンド送信
                unitStart.Start(StartComm, ModuleKind.RackTransfer);

                // 応答コマンド受信待ち
                while (ResponseFlg)
                {
                    Application.DoEvents();
                }
            }

            btnAdjustTubeSensorRead.Enabled = true;
        }

        #region Pitch値の調整
        private void btnAdjustM1PullInYAxisPitchUp_Click(object sender, EventArgs e)
        {
            numAdjustM1ForkYAxisPitch.Value = (double)numAdjustM1ForkYAxisPitch.Value + (double)numAdjustM1ForkYAxisPitch.SpinIncrement;
        }

        private void btnAdjustM1PullInYAxisPitchDown_Click(object sender, EventArgs e)
        {
            numAdjustM1ForkYAxisPitch.Value = (double)numAdjustM1ForkYAxisPitch.Value - (double)numAdjustM1ForkYAxisPitch.SpinIncrement;
        }

        private void btnAdjustM2PullInYAxisPitchUp_Click(object sender, EventArgs e)
        {
            numAdjustM2ForkYAxisPitch.Value = (double)numAdjustM2ForkYAxisPitch.Value + (double)numAdjustM2ForkYAxisPitch.SpinIncrement;
        }

        private void btnAdjustM2PullInYAxisPitchDown_Click(object sender, EventArgs e)
        {
            numAdjustM2ForkYAxisPitch.Value = (double)numAdjustM2ForkYAxisPitch.Value - (double)numAdjustM2ForkYAxisPitch.SpinIncrement;
        }

        private void btnAdjustM3PullInYAxisPitchUp_Click(object sender, EventArgs e)
        {
            numAdjustM3ForkYAxisPitch.Value = (double)numAdjustM3ForkYAxisPitch.Value + (double)numAdjustM3ForkYAxisPitch.SpinIncrement;
        }

        private void btnAdjustM3PullInYAxisPitchDown_Click(object sender, EventArgs e)
        {
            numAdjustM3ForkYAxisPitch.Value = (double)numAdjustM3ForkYAxisPitch.Value - (double)numAdjustM3ForkYAxisPitch.SpinIncrement;
        }

        private void btnAdjustM4PullInYAxisPitchUp_Click(object sender, EventArgs e)
        {
            numAdjustM4ForkYAxisPitch.Value = (double)numAdjustM4ForkYAxisPitch.Value + (double)numAdjustM4ForkYAxisPitch.SpinIncrement;
        }

        private void btnAdjustM4PullInYAxisPitchDown_Click(object sender, EventArgs e)
        {
            numAdjustM4ForkYAxisPitch.Value = (double)numAdjustM4ForkYAxisPitch.Value - (double)numAdjustM4ForkYAxisPitch.SpinIncrement;
        }

        private void btnAdjustLoadFeederXAxisPitchUp_Click(object sender, EventArgs e)
        {
            numAdjustLoadFeederXAxisPitch.Value = (double)numAdjustLoadFeederXAxisPitch.Value + (double)numAdjustLoadFeederXAxisPitch.SpinIncrement;
        }

        private void btnAdjustLoadFeederXAxisPitchDown_Click(object sender, EventArgs e)
        {
            numAdjustLoadFeederXAxisPitch.Value = (double)numAdjustLoadFeederXAxisPitch.Value - (double)numAdjustLoadFeederXAxisPitch.SpinIncrement;
        }

        private void btnAdjustUnLoadFeederXAxisPitchUp_Click(object sender, EventArgs e)
        {
            numAdjustUnLoadFeederXAxisPitch.Value = (double)numAdjustUnLoadFeederXAxisPitch.Value + (double)numAdjustUnLoadFeederXAxisPitch.SpinIncrement;
        }

        private void btnAdjustUnLoadFeederXAxisPitchDown_Click(object sender, EventArgs e)
        {
            numAdjustUnLoadFeederXAxisPitch.Value = (double)numAdjustUnLoadFeederXAxisPitch.Value - (double)numAdjustUnLoadFeederXAxisPitch.SpinIncrement;
        }

        private void btnAdjustSliderXAxisPitchUp_Click(object sender, EventArgs e)
        {
            numAdjustSliderXAxisPitch.Value = (double)numAdjustSliderXAxisPitch.Value + (double)numAdjustSliderXAxisPitch.SpinIncrement;
        }

        private void btnAdjustSliderXAxisPitchDown_Click(object sender, EventArgs e)
        {
            numAdjustSliderXAxisPitch.Value = (double)numAdjustSliderXAxisPitch.Value - (double)numAdjustSliderXAxisPitch.SpinIncrement;
        }
        #endregion

        #region Offset値の微調整
        /// <summary>
        /// モジュール１　ラック引込部　Y軸のモーター調整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdjustM1PullInYAxisForwardBack_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustM1ForkYAxisForward);
            Int32 motorNo = (int)MotorNoList.RackPullinYAxisM1;
            Double pitchValue = (double)numAdjustM1ForkYAxisPitch.Value;

            AdjustValue(upFlg, motorNo, numAdjustM1ForkYAxisOffsetHome, pitchValue);
        }

        /// <summary>
        /// モジュール２　ラック引込部　Y軸のモーター調整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdjustM2PullInYAxisForwardBack_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustM2ForkYAxisForward);
            Int32 motorNo = (int)MotorNoList.RackPullinYAxisM2;
            Double pitchValue = (double)numAdjustM2ForkYAxisPitch.Value;

            AdjustValue(upFlg, motorNo, numAdjustM2ForkYAxisOffsetHome, pitchValue);
        }

        /// <summary>
        /// モジュール３　ラック引込部　Y軸のモーター調整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdjustM3PullInYAxisForwardBack_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustM3ForkYAxisForward);
            Int32 motorNo = (int)MotorNoList.RackPullinYAxisM3;
            Double pitchValue = (double)numAdjustM3ForkYAxisPitch.Value;

            AdjustValue(upFlg, motorNo, numAdjustM3ForkYAxisOffsetHome, pitchValue);
        }

        /// <summary>
        /// モジュール４　ラック引込部　Y軸のモーター調整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdjustM4PullInYAxisForwardBack_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustM4ForkYAxisForward);
            Int32 motorNo = (int)MotorNoList.RackPullinYAxisM4;
            Double pitchValue = (double)numAdjustM4ForkYAxisPitch.Value;

            AdjustValue(upFlg, motorNo, numAdjustM4ForkYAxisOffsetHome, pitchValue);
        }

        /// <summary>
        /// ラックフィーダ　X軸のモーター調整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdjustLoadFeederXAxisLeftRight_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustLoadFeederXAxisRight);
            Int32 motorNo = (int)MotorNoList.RackSetLoadFeederXAxis;
            Double pitchValue = (double)numAdjustLoadFeederXAxisPitch.Value;

            switch ((int)lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.LoadFeederRackLoad:
                    AdjustValue(upFlg, motorNo, numAdjustLoadFeederXAxisOffsetLoad, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.LoadFeederRackReturnFeeder:
                    AdjustValue(upFlg, motorNo, numAdjustLoadFeederXAxisOffsetTakeOut, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.LoadFeederRackIDReading:
                    AdjustValue(upFlg, motorNo, numAdjustLoadFeederXAxisOffsetRackIDRead, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.LoadFeederSampleIDReading:
                    AdjustValue(upFlg, motorNo, numAdjustLoadFeederXAxisOffsetSampleIDRead, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.LoadFeederTubeSensorReading:
                    AdjustValue(upFlg, motorNo, numAdjustLoadFeederXAxisOffsetTubeSensor, pitchValue);
                    break;
            }
        }

        /// <summary>
        /// 再検ラックフィーダ　X軸のモーター調整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdjustUnLoadFeederXAxisLeftRight_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustUnLoadFeederXAxisLeft);
            Int32 motorNo = (int)MotorNoList.RackSetUnLoadFeederXAxis;
            Double pitchValue = (double)numAdjustUnLoadFeederXAxisPitch.Value;

            switch ((int)lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.UnLoadFeederRackUnLoad:
                    AdjustValue(upFlg, motorNo, numAdjustUnLoadFeederXAxisOffsetUnLoad, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.UnLoadFeederRackRetest:
                    AdjustValue(upFlg, motorNo, numAdjustUnLoadFeederXAxisOffsetRetest, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.UnLoadFeederRackTakeout:
                    AdjustValue(upFlg, motorNo, numAdjustUnLoadFeederXAxisOffsetTakeOut, pitchValue);
                    break;
            }
        }

        /// <summary>
        /// ラックスライダー　X軸のモーター調整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdjustSliderXAxisLeftRight_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustSliderXAxisRight);
            Int32 motorNo = (int)MotorNoList.RackSetSliderXAxis;
            Double pitchValue = (double)numAdjustSliderXAxisPitch.Value;

            switch ((int)lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.SliderRackLoad:
                    AdjustValue(upFlg, motorNo, numAdjustSliderXAxisOffsetLoad, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.SliderRackUnLoad:
                    AdjustValue(upFlg, motorNo, numAdjustSliderXAxisOffsetUnLoad, pitchValue);
                    break;
            }
        }

        /// <summary>
        /// モーター移動 
        /// </summary>
        /// <param name="Up">調整値を加算するかどうか</param>
        /// <param name="MotorNo">コマンドで送信するモーター番号</param>
        /// <param name="numOffset">調整対象のオフセットのコントロール</param>
        /// <param name="Pitch">調整値</param>
        new private void AdjustValue(bool Up, int MotorNo, UltraNumericEditor numOffset, double Pitch)
        {
            UpDownButtonEnable(false);

            //モーター調整コマンドチクチク
            AdjustUpDownComm.MotorNo = MotorNo;
            if (Up)
                AdjustUpDownComm.Distance = (double)Pitch;
            else
                AdjustUpDownComm.Distance = -(double)Pitch;

            //画面の調整対象のオフセットに調整値を加味する
            //Downの場合はDistanceにマイナス値が入っているので減算になる
            numOffset.Value = (double)numOffset.Value + AdjustUpDownComm.Distance;

            unitAdjust.Start(AdjustUpDownComm, ModuleKind.RackTransfer);

            //レスポンスはSetResponseで待機
        }
        #endregion
    }
}
