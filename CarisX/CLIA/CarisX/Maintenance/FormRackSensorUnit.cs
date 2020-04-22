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
    public partial class FormRackSensorUnit : FormUnitBase
    {
        IMaintenanceUnitStart unitStart = new UnitSensorStart();
        volatile bool ResponseFlg;
        private bool MaintenanceMode;
        public CommonFunction ComFunc = new CommonFunction();


        public FormRackSensorUnit()
        {
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            InitializeComponent();
            SensorUnitLoad();
            MaintenanceMode = true;
            setCulture();
            ComFunc.SetControlSettings(this);
        }


        #region リソース設定
        private void setCulture()
        {
            tabUnit.Tabs[0].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_000;
            tabUnit.Tabs[1].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_001;

            //センサーステータス
            gbxStatusSampleRackSet.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_048;
            gbxStatusTankSetup.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_049;
            gbxStatusSampleRackTransfer1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_046;
            gbxStatusSampleRackPullIn1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_047;
            gbxStatusSampleRackTransfer2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_050;
            gbxStatusSampleRackPullIn2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_051;
            gbxStatusSampleRackTransfer3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_052;
            gbxStatusSampleRackPullIn3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_053;
            gbxStatusSampleRackTransfer4.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_054;
            gbxStatusSampleRackPullIn4.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_055;

            lblSampleContainerTypeIdentify1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_006;
            lblSampleContainerTypeIdentify2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_007;
            lblSampleContainerTypeIdentify3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_008;
            lblRackCollectionLaneFront.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_009;
            lblRackCollectionLaneBack.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_010;
            lblRackInstallationLaneFront.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_011;
            lblRackInstallationLaneBack.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_012;
            lblRackStandbyLaneFront.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_013;
            lblRackStandbyLaneBack.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_014;
            lblRackFeederCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_015;
            lblReturnRackFeederCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_016;
            lblRackSettingDetectiveLightReception.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_018;
            lblCollectionAndRetestRackCover.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_019;
            lblDrainTankFull.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_020;
            lblUsableDrainTank.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_021;
            lblRackSendStandbyPosition1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_002;
            lblRackBackStandbyPosition1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_003;
            lblRackPullInForkCatch11.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_004;
            lblRackPullInForkCatch21.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_005;
            lblRackSendStandbyPosition2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_002;
            lblRackBackStandbyPosition2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_003;
            lblRackPullInForkCatch12.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_004;
            lblRackPullInForkCatch22.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_005;
            lblRackSendStandbyPosition3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_002;
            lblRackBackStandbyPosition3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_003;
            lblRackPullInForkCatch13.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_004;
            lblRackPullInForkCatch23.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_005;
            lblRackSendStandbyPosition4.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_002;
            lblRackBackStandbyPosition4.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_003;
            lblRackPullInForkCatch14.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_004;
            lblRackPullInForkCatch24.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_005;

            //Senser Use/NoUse
            gbxSampleContainerTypeIdentify1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_026;
            gbxSampleContainerTypeIdentify2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_027;
            gbxSampleContainerTypeIdentify3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_028;
            gbxRackCollectionLaneFront.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_029;
            gbxRackCollectionLaneBack.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_030;
            gbxRackInstallationLaneFront.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_031;
            gbxRackInstallationLaneBack.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_032;
            gbxRackStandbyLaneFront.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_033;
            gbxRackStandbyLaneBack.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_034;
            gbxRackFeederCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_035;
            gbxReturnRackFeederCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_036;
            gbxRackSettingDetectiveLightReception.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_038;
            gbxCollectionAndRetestRackCover.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_039;
            gbxDrainTankFull.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_040;
            gbxUsableDrainTank.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_041;
            gbxRackSendStandbyPosition1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_022;
            gbxRackBackStandbyPosition1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_023;
            gbxRackPullInForkCatch11.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_024;
            gbxRackPullInForkCatch21.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_025;
            gbxRackSendStandbyPosition2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_022;
            gbxRackBackStandbyPosition2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_023;
            gbxRackPullInForkCatch12.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_024;
            gbxRackPullInForkCatch22.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_025;
            gbxRackSendStandbyPosition3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_022;
            gbxRackBackStandbyPosition3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_023;
            gbxRackPullInForkCatch13.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_024;
            gbxRackPullInForkCatch23.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_025;
            gbxRackSendStandbyPosition4.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_022;
            gbxRackBackStandbyPosition4.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_023;
            gbxRackPullInForkCatch14.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_024;
            gbxRackPullInForkCatch24.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_025;

            gbxUsableSampleRackSet.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_048;
            gbxUsableTankSetup.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_049;
            gbxUsableSampleRackTransfer1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_046;
            gbxUsableSampleRackPullIn1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_047;
            gbxUsableSampleRackTransfer2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_050;
            gbxUsableSampleRackPullIn2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_051;
            gbxUsableSampleRackTransfer3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_052;
            gbxUsableSampleRackPullIn3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_053;
            gbxUsableSampleRackTransfer4.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_054;
            gbxUsableSampleRackPullIn4.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_055;

            //Use
            rbtSampleContainerTypeIdentify1Use.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtSampleContainerTypeIdentify2Use.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtSampleContainerTypeIdentify3Use.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackCollectionLaneFrontUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackCollectionLaneBackUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackInstallationLaneFrontUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackInstallationLaneBackUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackStandbyLaneFrontUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackStandbyLaneBackUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackFeederCatchUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtReturnRackFeederCatchUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackSettingDetectiveLightReceptionUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtCollectionAndRetestRackCoverUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtDrainTankFullUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtUsableDrainTankUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackSendStandbyPositionUse1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackBackStandbyPositionUse1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackPullInForkCatch1Use1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackPullInForkCatch2Use1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackSendStandbyPositionUse2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackBackStandbyPositionUse2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackPullInForkCatch1Use2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackPullInForkCatch2Use2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackSendStandbyPositionUse3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackBackStandbyPositionUse3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackPullInForkCatch1Use3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackPullInForkCatch2Use3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackSendStandbyPositionUse4.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackBackStandbyPositionUse4.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackPullInForkCatch1Use4.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            rbtRackPullInForkCatch2Use4.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_042;
            //NoUse
            rbtSampleContainerTypeIdentify1NoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtSampleContainerTypeIdentify2NoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtSampleContainerTypeIdentify3NoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackCollectionLaneFrontNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackCollectionLaneBackNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackInstallationLaneFrontNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackInstallationLaneBackNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackStandbyLaneFrontNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackStandbyLaneBackNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackFeederCatchNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtReturnRackFeederCatchNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackSettingDetectiveLightReceptionNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtCollectionAndRetestRackCoverNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtDrainTankFullNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtUsableDrainTankNoUse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackSendStandbyPositionNoUse1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackBackStandbyPositionNoUse1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackPullInForkCatch1NoUse1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackPullInForkCatch2NoUse1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackSendStandbyPositionNoUse2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackBackStandbyPositionNoUse2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackPullInForkCatch1NoUse2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackPullInForkCatch2NoUse2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackSendStandbyPositionNoUse3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackBackStandbyPositionNoUse3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackPullInForkCatch1NoUse3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackPullInForkCatch2NoUse3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackSendStandbyPositionNoUse4.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackBackStandbyPositionNoUse4.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackPullInForkCatch1NoUse4.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;
            rbtRackPullInForkCatch2NoUse4.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_043;

            btnOk.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_044;
            btnCancel.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_RACKSENSOR_045;

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
            RackTransferCommCommand_0040 StartComm = new RackTransferCommCommand_0040();

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

                System.Threading.Thread.Sleep(300);
            }

            AbortFlg = false;

            //メイン画面のナビゲータバーをEnableコマンドバーをデフォルトに戻す。
            Action Disp = () => { DispEnable(true); }; Invoke(Disp);

            return str;
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public override bool UnitAbortexe()
        {
            UnitAbort(ModuleKind.RackTransfer);
            return true;
        }

        /// <summary>
        /// 受信データ処理をおこないます。
        /// </summary>
        public override void SetResponse(CommCommandEventArgs comm)
        {
            switch (comm.Command.CommandId)
            {
                case (int)CommandKind.RackTransferCommand1040:
                    //センサーステータス
                    RackTransferCommCommand_1040 ds1040 = new RackTransferCommCommand_1040();
                    ds1040 = (RackTransferCommCommand_1040)comm.Command;

                    //サンプル容器種識別センサ1
                    SetSenserStatusONOFF(ds1040.SampleContainerTypeIdentify1, lblSampleContainerTypeIdentify1Dsp);
                    //サンプル容器種識別センサ2
                    SetSenserStatusONOFF(ds1040.SampleContainerTypeIdentify2, lblSampleContainerTypeIdentify2Dsp);
                    //サンプル容器種識別センサ3
                    SetSenserStatusONOFF(ds1040.SampleContainerTypeIdentify3, lblSampleContainerTypeIdentify3Dsp);

                    //ラック回収レーン前センサ
                    SetSenserStatusONOFF(ds1040.RackCollectionLaneFront, lblRackCollectionLaneFrontDsp);
                    //ラック回収レーン奥センサ
                    SetSenserStatusONOFF(ds1040.RackCollectionLaneBack, lblRackCollectionLaneBackDsp);

                    //ラック設置レーン前センサ
                    SetSenserStatusONOFF(ds1040.RackInstallationLaneFront, lblRackInstallationLaneFrontDsp);
                    //ラック設置レーン奥センサ
                    SetSenserStatusONOFF(ds1040.RackInstallationLaneBack, lblRackInstallationLaneBackDsp);

                    //ラック待機レーン前センサ
                    SetSenserStatusONOFF(ds1040.RackStandbyLaneFront, lblRackStandbyLaneFrontDsp);
                    //ラック待機レーン奥センサ
                    SetSenserStatusONOFF(ds1040.RackStandbyLaneBack, lblRackStandbyLaneBackDsp);

                    //ラックフィーダ受取センサ
                    SetSenserStatusONOFF(ds1040.RackFeederCatch, lblRackFeederCatchDsp);

                    //返却ラックフィーダ受取センサ
                    SetSenserStatusONOFF(ds1040.ReturnRackFeederCatch, lblReturnRackFeederCatchDsp);

                    //ラック設置検知センサ(受光)
                    SetSenserStatusONOFF(ds1040.RackSettingDetectiveLightReception, lblRackSettingDetectiveLightReceptionDsp);

                    //回収・再検ラック蓋センサ
                    SetSenserStatusONOFF(ds1040.CollectionAndRetestRackCover, lblCollectionAndRetestRackCoverDsp);

                    //廃液満杯センサ
                    SetSenserStatusONOFF(ds1040.DrainTankFull, lblDrainTankFullDsp);

                    //廃液タンク有無センサ
                    SetSenserStatusONOFF(ds1040.UsableDrainTank, lblUsableDrainTankDsp);

                    //装置１
                    //ラック送り待機位置センサ
                    SetSenserStatusONOFF(ds1040.RackSendStandbyPosition1, lblRackSendStandbyPositionDsp1);
                    //ラック戻し待機位置センサ
                    SetSenserStatusONOFF(ds1040.RackBackStandbyPosition1, lblRackBackStandbyPositionDsp1);

                    //ラック引込フォーク受取りセンサ1
                    SetSenserStatusONOFF(ds1040.RackPullInForkCatch11, lblRackPullInForkCatch1Dsp1);
                    //ラック引込フォーク受取りセンサ2
                    SetSenserStatusONOFF(ds1040.RackPullInForkCatch21, lblRackPullInForkCatch2Dsp1);

                    //装置２
                    //ラック送り待機位置センサ
                    SetSenserStatusONOFF(ds1040.RackSendStandbyPosition2, lblRackSendStandbyPositionDsp2);
                    //ラック戻し待機位置センサ
                    SetSenserStatusONOFF(ds1040.RackBackStandbyPosition2, lblRackBackStandbyPositionDsp2);

                    //ラック引込フォーク受取りセンサ1
                    SetSenserStatusONOFF(ds1040.RackPullInForkCatch12, lblRackPullInForkCatch1Dsp2);
                    //ラック引込フォーク受取りセンサ2
                    SetSenserStatusONOFF(ds1040.RackPullInForkCatch22, lblRackPullInForkCatch2Dsp2);

                    //装置３
                    //ラック送り待機位置センサ
                    SetSenserStatusONOFF(ds1040.RackSendStandbyPosition3, lblRackSendStandbyPositionDsp3);
                    //ラック戻し待機位置センサ
                    SetSenserStatusONOFF(ds1040.RackBackStandbyPosition3, lblRackBackStandbyPositionDsp3);

                    //ラック引込フォーク受取りセンサ1
                    SetSenserStatusONOFF(ds1040.RackPullInForkCatch13, lblRackPullInForkCatch1Dsp3);
                    //ラック引込フォーク受取りセンサ2
                    SetSenserStatusONOFF(ds1040.RackPullInForkCatch23, lblRackPullInForkCatch2Dsp3);

                    //装置４
                    //ラック送り待機位置センサ
                    SetSenserStatusONOFF(ds1040.RackSendStandbyPosition4, lblRackSendStandbyPositionDsp4);
                    //ラック戻し待機位置センサ
                    SetSenserStatusONOFF(ds1040.RackBackStandbyPosition4, lblRackBackStandbyPositionDsp4);

                    //ラック引込フォーク受取りセンサ1
                    SetSenserStatusONOFF(ds1040.RackPullInForkCatch14, lblRackPullInForkCatch1Dsp4);
                    //ラック引込フォーク受取りセンサ2
                    SetSenserStatusONOFF(ds1040.RackPullInForkCatch24, lblRackPullInForkCatch2Dsp4);

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
            }
        }

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

            CarisXSensorParameter.SensorParameterUseNoUseRack sensorParam = new CarisXSensorParameter.SensorParameterUseNoUseRack();

            sensorParam.SampleContainerTypeIdentify1 = (byte)ComFunc.getSelectedRadioButtonValue(gbxSampleContainerTypeIdentify1);
            sensorParam.SampleContainerTypeIdentify2 = (byte)ComFunc.getSelectedRadioButtonValue(gbxSampleContainerTypeIdentify2);
            sensorParam.SampleContainerTypeIdentify3 = (byte)ComFunc.getSelectedRadioButtonValue(gbxSampleContainerTypeIdentify3);
            sensorParam.RackCollectionLaneFront = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackCollectionLaneFront);
            sensorParam.RackCollectionLaneBack = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackCollectionLaneBack);
            sensorParam.RackInstallationLaneFront = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackInstallationLaneFront);
            sensorParam.RackInstallationLaneBack = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackInstallationLaneBack);
            sensorParam.RackStandbyLaneFront = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackStandbyLaneFront);
            sensorParam.RackStandbyLaneBack = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackStandbyLaneBack);
            sensorParam.RackFeederCatch = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackFeederCatch);
            sensorParam.ReturnRackFeederCatch = (byte)ComFunc.getSelectedRadioButtonValue(gbxReturnRackFeederCatch);
            sensorParam.RackSettingDetectiveLightReception = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackSettingDetectiveLightReception);
            sensorParam.CollectionAndRetestRackCover = (byte)ComFunc.getSelectedRadioButtonValue(gbxCollectionAndRetestRackCover);
            sensorParam.DrainTankFull = (byte)ComFunc.getSelectedRadioButtonValue(gbxDrainTankFull);
            sensorParam.UsableDrainTank = (byte)ComFunc.getSelectedRadioButtonValue(gbxUsableDrainTank);
            sensorParam.RackSendStandbyPosition1 = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackSendStandbyPosition1);
            sensorParam.RackBackStandbyPosition1 = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackBackStandbyPosition1);
            sensorParam.RackPullInForkCatch11 = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackPullInForkCatch11);
            sensorParam.RackPullInForkCatch21 = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackPullInForkCatch21);
            sensorParam.RackSendStandbyPosition2 = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackSendStandbyPosition2);
            sensorParam.RackBackStandbyPosition2 = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackBackStandbyPosition2);
            sensorParam.RackPullInForkCatch12 = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackPullInForkCatch12);
            sensorParam.RackPullInForkCatch22 = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackPullInForkCatch22);
            sensorParam.RackSendStandbyPosition3 = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackSendStandbyPosition3);
            sensorParam.RackBackStandbyPosition3 = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackBackStandbyPosition3);
            sensorParam.RackPullInForkCatch13 = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackPullInForkCatch13);
            sensorParam.RackPullInForkCatch23 = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackPullInForkCatch23);
            sensorParam.RackSendStandbyPosition4 = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackSendStandbyPosition4);
            sensorParam.RackBackStandbyPosition4 = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackBackStandbyPosition4);
            sensorParam.RackPullInForkCatch14 = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackPullInForkCatch14);
            sensorParam.RackPullInForkCatch24 = (byte)ComFunc.getSelectedRadioButtonValue(gbxRackPullInForkCatch24);

            config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse = sensorParam;

            //パラメータ送信
            SendParam(sensorParam, ModuleKind.RackTransfer);
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

            //メイン画面のナビゲータバーをEnableコマンドバーをデフォルトに戻す。
            Action Disp = () => { DispEnable(true); }; Invoke(Disp);

            ToolbarsControl();

        }

        /// <summary>
        /// センサーパラメータを読み込みます。
        /// </summary>
        public void SensorUnitLoad()
        {
            ParameterFilePreserve<CarisXSensorParameter> config = Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance;
            SensorParamLoad(config);

            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.SampleContainerTypeIdentify1, rbtSampleContainerTypeIdentify1Use, rbtSampleContainerTypeIdentify1NoUse);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.SampleContainerTypeIdentify2, rbtSampleContainerTypeIdentify2Use, rbtSampleContainerTypeIdentify2NoUse);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.SampleContainerTypeIdentify3, rbtSampleContainerTypeIdentify3Use, rbtSampleContainerTypeIdentify3NoUse);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackCollectionLaneFront, rbtRackCollectionLaneFrontUse, rbtRackCollectionLaneFrontNoUse);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackCollectionLaneBack, rbtRackCollectionLaneBackUse, rbtRackCollectionLaneBackNoUse);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackInstallationLaneFront, rbtRackInstallationLaneFrontUse, rbtRackInstallationLaneFrontNoUse);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackInstallationLaneBack, rbtRackInstallationLaneBackUse, rbtRackInstallationLaneBackNoUse);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackStandbyLaneFront, rbtRackStandbyLaneFrontUse, rbtRackStandbyLaneFrontNoUse);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackStandbyLaneBack, rbtRackStandbyLaneBackUse, rbtRackStandbyLaneBackNoUse);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackFeederCatch, rbtRackFeederCatchUse, rbtRackFeederCatchNoUse);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.ReturnRackFeederCatch, rbtReturnRackFeederCatchUse, rbtReturnRackFeederCatchNoUse);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackSettingDetectiveLightReception, rbtRackSettingDetectiveLightReceptionUse, rbtRackSettingDetectiveLightReceptionNoUse);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.CollectionAndRetestRackCover, rbtCollectionAndRetestRackCoverUse, rbtCollectionAndRetestRackCoverNoUse);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.DrainTankFull, rbtDrainTankFullUse, rbtDrainTankFullNoUse);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.UsableDrainTank, rbtUsableDrainTankUse, rbtUsableDrainTankNoUse);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackSendStandbyPosition1, rbtRackSendStandbyPositionUse1, rbtRackSendStandbyPositionNoUse1);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackBackStandbyPosition1, rbtRackBackStandbyPositionUse1, rbtRackBackStandbyPositionNoUse1);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackPullInForkCatch11, rbtRackPullInForkCatch1Use1, rbtRackPullInForkCatch1NoUse1);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackPullInForkCatch21, rbtRackPullInForkCatch2Use1, rbtRackPullInForkCatch2NoUse1);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackSendStandbyPosition2, rbtRackSendStandbyPositionUse2, rbtRackSendStandbyPositionNoUse2);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackBackStandbyPosition2, rbtRackBackStandbyPositionUse2, rbtRackBackStandbyPositionNoUse2);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackPullInForkCatch12, rbtRackPullInForkCatch1Use2, rbtRackPullInForkCatch1NoUse2);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackPullInForkCatch22, rbtRackPullInForkCatch2Use2, rbtRackPullInForkCatch2NoUse2);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackSendStandbyPosition3, rbtRackSendStandbyPositionUse3, rbtRackSendStandbyPositionNoUse3);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackBackStandbyPosition3, rbtRackBackStandbyPositionUse3, rbtRackBackStandbyPositionNoUse3);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackPullInForkCatch13, rbtRackPullInForkCatch1Use3, rbtRackPullInForkCatch1NoUse3);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackPullInForkCatch23, rbtRackPullInForkCatch2Use3, rbtRackPullInForkCatch2NoUse3);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackSendStandbyPosition4, rbtRackSendStandbyPositionUse4, rbtRackSendStandbyPositionNoUse4);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackBackStandbyPosition4, rbtRackBackStandbyPositionUse4, rbtRackBackStandbyPositionNoUse4);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackPullInForkCatch14, rbtRackPullInForkCatch1Use4, rbtRackPullInForkCatch1NoUse4);
            SetSenserUseNoUse(config.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].sensorParameterUseNoUse.RackPullInForkCatch24, rbtRackPullInForkCatch2Use4, rbtRackPullInForkCatch2NoUse4);

        }

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

        private void btnOk_Click(object sender, EventArgs e)
        {
            ParamSave();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

