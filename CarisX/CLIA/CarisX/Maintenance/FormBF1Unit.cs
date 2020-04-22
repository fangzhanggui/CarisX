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
    /// BF1部、BF1廃液部
    /// </summary>
    public partial class FormBF1Unit : FormUnitBase
    {
        IMaintenanceUnitStart unitStart = new UnitStart();
        IMaintenanceUnitStart unitStartAdjust = new UnitStarAdjust();
        IMaintenanceUnitStart unitAdjust = new UnitAdjust();
        IMaintenanceUnitStart unitAdjustAbort = new UnitStarAdjustAbort();
        volatile bool ResponseFlg;
        public CommonFunction ComFunc = new CommonFunction();
        Infragistics.Win.ValueListItem[] cmbValueCoarseFine;

        //モーター調整コマンドチクチク
        SlaveCommCommand_0473 AdjustUpDownComm = new SlaveCommCommand_0473();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormBF1Unit()
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

            lbxtestSequence.SelectedIndex = 0;
            lbxAdjustSequence.SelectedIndex = 0;
            cmbAdjustCoarseFine.SelectedIndex = 0;

        }

        #region リソース設定

        /// <summary>
        /// リソース設定
        /// </summary>
        private void setCulture()
        {
            //Tab
            tabUnit.Tabs[0].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_013;                          //Test
            tabUnit.Tabs[1].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_052;                          //Configuration
            tabUnit.Tabs[2].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_053;                          //Motor Parameters
            tabUnit.Tabs[3].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_MOTORADJUSTMENT;     //Adjust

            //Testタブ
            gbxtestSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_000;      //Sequence
            object SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_001, No=(int)BF1Sequence.Init},               //1: Unit Initialization
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_002, No=(int)BF1Sequence.BF1},                //2: BF 1
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_003, No=(int)BF1Sequence.WasteLiquid},        //3: Waste Liquid
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_004, No=(int)BF1Sequence.Prime},              //4: Prime
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_005, No=(int)BF1Sequence.Rinse},              //5: Rinse
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_050, No=(int)BF1Sequence.MovetoBF1Washing},   //6: Move to Adjust Position for BF 1 Washing
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_051, No=(int)BF1Sequence.BF1Dispense},        //7: BF 1 Dispense
            };
            lbxtestSequence = ComFunc.setSequenceListBox(lbxtestSequence, SequenceList);

            gbxtestParameters.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_006;                       //Parameters
            lbltestRepeatFrequency.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_007;                  //Repeat Frequency
            lbltestNumber.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_008;                           //Number
            lbltestRepeatInterval.Text = Resources_Maintenance.STRING_MAINTENANCE_BF1_061;
            lbltestRepeatIntervalUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BF1_062;
            lbltestNozzle.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_054;                           //Nozzle
            rbttestAllNozzle.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_009;                        //All nozzle
            rbttestNozzle1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_010;                          //Nozzle 1
            rbttestNozzle2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_011;                          //Nozzle 2

            gbxtestResponce.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_012;//Responce

            //Configurationタブ
            gbxconfParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_006;//Parameters

            lblconfWaitTimeAfterSuckingUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_014;//The Delay Time after Wash Solution Aspiration
            lblconfWaitTimeAfterDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_015;//The Delay Time after Wash Solution Dispense
            lblconfInterval.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_016;//Washing Intervals
            lblconfNoOfWashTimes.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_017;//The Number of Times of Wash
            lblconfNoOfPrimeTimes.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_018;//The Number of Times of Prime
            lblconfNoOfRinseTimes.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_019;//The Number of Times of Rinse
            lblconfPrimeVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_020;//Prime Volume
            lblconfWashVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_021;//Washing Volume
            lblconfWaitTimeWaste.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_057;

            lblconfWaitTimeAfterSuckingUpUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_022;//(sec)
            lblconfWaitTimeAfterDispenseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_022;//(sec)
            lblconfIntervalUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_022;//(sec)
            lblconfWaitTimeWasteUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_022;//(sec)

            lblconfNoOfWashTimesUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_023;//(times)
            lblconfNoOfPrimeTimesUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_023;//(times)
            lblconfNoOfRinseTimesUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_023;//(times)

            lblconfPrimeVolumeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_024;//(uL)
            lblconfWashVolumeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_024;//(uL)

            // BF2から移動
            lblconfDelayTimeCycleStartTo1stAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_014;//The Delay Time From Cycle Start to 1st-Aspiration
            lblconfDelayTime1stDispenseTo2ndAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_015;//The Delay Time From 1st-Dispense to 2nd-Aspiration
            lblconfDelayTime2ndDispenseTo3rdAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_016;//The Delay Time From 2nd-Dispense to 3rd-Aspiration
            lblconfVolumeAfter3rdAspiration.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_021;//Volume After the 3rd-Aspiration

            lblconfDelayTimeCycleStartTo1stAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_022;//(sec)
            lblconfDelayTime1stDispenseTo2ndAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_022;//(sec)
            lblconfDelayTime2ndDispenseTo3rdAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_022;//(sec)

            lblconfVolumeAfter3rdAspirationUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_024;//(uL)

            //MotorParameter
            gbxParamNozzleZAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_025;
            gbxParamNozzleZAxisCommon.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_032;
            gbxParamNozzleZAxisDown.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_063;
            gbxParamNozzleZAxisUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_064;
            gbxParamNozzleZAxisOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_039;
            lblParamNozzleZAxisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_033;
            lblParamNozzleZAxisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_034;
            lblParamNozzleZAxisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_035;
            lblParamNozzleZAxisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_036;
            lblParamNozzleZAxisDownInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_033;
            lblParamNozzleZAxisDownTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_034;
            lblParamNozzleZAxisDownAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_035;
            lblParamNozzleZAxisDownConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_036;
            lblParamNozzleZAxisUpInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_033;
            lblParamNozzleZAxisUpTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_034;
            lblParamNozzleZAxisUpAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_035;
            lblParamNozzleZAxisUpConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_036;
            lblParamNozzleZAxisOffsetReactionCell.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_030;
            lblParamNozzleZAxisOffsetCuvette.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_043;
            lblParamNozzleZAxisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamNozzleZAxisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamNozzleZAxisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_048;
            lblParamNozzleZAxisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamNozzleZAxisDownInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamNozzleZAxisDownTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamNozzleZAxisDownAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_048;
            lblParamNozzleZAxisDownConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamNozzleZAxisUpInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamNozzleZAxisUpTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamNozzleZAxisUpAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_048;
            lblParamNozzleZAxisUpConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamNozzleZAxisOffsetReactionCellUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_049;
            lblParamNozzleZAxisOffsetCuvetteUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_049;

            gbxParamWasteZAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_031;
            gbxParamWasteZAxisCommon.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_065;
            gbxParamWasteZAxisDown.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_037;
            gbxParamWasteZAxisUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_038;
            gbxParamWasteZAxisOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_039;
            lblParamWasteZAxisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_033;
            lblParamWasteZAxisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_034;
            lblParamWasteZAxisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_035;
            lblParamWasteZAxisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_036;
            lblParamWasteZAxisDownInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_033;
            lblParamWasteZAxisDownTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_034;
            lblParamWasteZAxisDownAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_035;
            lblParamWasteZAxisDownConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_036;
            lblParamWasteZAxisUpInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_033;
            lblParamWasteZAxisUpTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_034;
            lblParamWasteZAxisUpAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_035;
            lblParamWasteZAxisUpConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_036;
            lblParamWasteZAxisOffsetReactionCell.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_030;
            lblParamWasteZAxisOffsetCuvette.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_043;
            lblParamWasteZAxisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamWasteZAxisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamWasteZAxisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_048;
            lblParamWasteZAxisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamWasteZAxisDownInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamWasteZAxisDownTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamWasteZAxisDownAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_048;
            lblParamWasteZAxisDownConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamWasteZAxisUpInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamWasteZAxisUpTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamWasteZAxisUpAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_048;
            lblParamWasteZAxisUpConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamWasteZAxisOffsetReactionCellUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_049;
            lblParamWasteZAxisOffsetCuvetteUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_049;

            gbxParamSyringe.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_056;
            gbxParamSyringeAsp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_040;
            gbxParamSyringeDis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_041;
            gbxParamSyringePrime.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_042;
            lblParamSyringeAspInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_033;
            lblParamSyringeAspTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_034;
            lblParamSyringeAspAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_035;
            lblParamSyringeAspConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_036;
            lblParamSyringeDisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_033;
            lblParamSyringeDisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_034;
            lblParamSyringeDisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_035;
            lblParamSyringeDisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_036;
            lblParamSyringePrimeInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_033;
            lblParamSyringePrimeTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_034;
            lblParamSyringePrimeAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_035;
            lblParamSyringePrimeConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_036;
            lblParamSyringeGain.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_045;
            lblParamSyringeOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_046;
            lblParamSyringeAspInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamSyringeAspTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamSyringeAspAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_048;
            lblParamSyringeAspConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamSyringeDisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamSyringeDisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamSyringeDisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_048;
            lblParamSyringeDisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamSyringePrimeInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamSyringePrimeTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;
            lblParamSyringePrimeAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_048;
            lblParamSyringePrimeConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_047;

            //MotorAdjust
            gbxAdjustSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_000;
            SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_058, No=(int)MotorAdjustStopPosition.Wash1Prime},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_059, No=(int)MotorAdjustStopPosition.WasteWash1Prime},
            };
            lbxAdjustSequence = ComFunc.setSequenceListBox(lbxAdjustSequence, SequenceList);

            gbxAdjustParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_006;
            lblAdjustCoarseFine.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_COARSEFINE;
            cmbValueCoarseFine = new Infragistics.Win.ValueListItem[]
            {
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_COARSE, dataValue:MotorAdjustCoarseFine.Coarse),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_FINE, dataValue:MotorAdjustCoarseFine.Fine),
            };
            cmbAdjustCoarseFine.Items.AddRange(cmbValueCoarseFine);

            gbxAdjustNozzleZAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_025;
            lblAdjustNozzleZAxisOffsetReactionCell.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_030;
            lblAdjustNozzleZAxisOffsetReactionCellUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_049;

            gbxAdjustWasteNozzleZAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_031;
            lblAdjustWasteNozzleZAxisOffsetReactionCell.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_030;
            lblAdjustWasteNozzleZAxisOffsetReactionCellUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF1_049;

            //UP/Down
            btnAdjustNozzleZAxisDown.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_DOWN;
            btnAdjustNozzleZAxisUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_UP;
            btnAdjustWasteNozzleZAxisDown.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_DOWN;
            btnAdjustWasteNozzleZAxisUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_UP;

            //Pitch
            lblAdjustNozzleZAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;
            lblAdjustWasteNozzleZAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;

        }
        #endregion

        /// <summary>
        /// ラジオボタンの選択値を設定
        /// </summary>
        private void setRadioButtonValue()
        {
            rbttestAllNozzle.Tag = (int)BF1RadioValue.AllNozzle;
            rbttestNozzle1.Tag = (int)BF1RadioValue.Nozzle1;
            rbttestNozzle2.Tag = (int)BF1RadioValue.Nozzle2;
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
                    Func<object> getfuncno = () => { return lbxtestSequence.SelectedValue; };
                    int FuncNo = (int)Invoke(getfuncno);

                    SlaveCommCommand_0439 StartComm = new SlaveCommCommand_0439();
                    StartComm.UnitNo = (int)UnitNoList.BF1;
                    StartComm.FuncNo = FuncNo;

                    switch (FuncNo)
                    {
                        case (int)BF1Sequence.WasteLiquid:
                        case (int)BF1Sequence.BF1Dispense:
                            StartComm.Arg1 = ComFunc.getSelectedRadioButtonValue(gbxtestNozzle);
                            break;
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
                    txttestResponce.Text = comm.Command.CommandText.Remove(0, 20);

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
            gbxtestNozzle.Enabled = false;
        }

        /// <summary>
        /// 選択された機能番号に応じてパラメータを活性化する
        /// </summary>
        private void lbxtestSequenceListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParametersAllFalse();
            switch (lbxtestSequence.SelectedValue)
            {
                case (int)BF1Sequence.WasteLiquid:
                case (int)BF1Sequence.BF1Dispense:
                    gbxtestNozzle.Enabled = true;
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

            //BF1部Z軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.BF1NozzleZAxis;
            if (!adjustSave)
            {
                //モーターパラメータの保存
                //通常
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamNozzleZAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamNozzleZAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamNozzleZAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamNozzleZAxisConst.Value;
                //洗浄降下   
                Mparam.MotorSpeed[1].InitSpeed = (int)numParamNozzleZAxisDownInit.Value;
                Mparam.MotorSpeed[1].TopSpeed = (int)numParamNozzleZAxisDownTop.Value;
                Mparam.MotorSpeed[1].Accel = (int)numParamNozzleZAxisDownAccelerator.Value;
                Mparam.MotorSpeed[1].ConstSpeed = (int)numParamNozzleZAxisDownConst.Value;
                //洗浄上昇
                Mparam.MotorSpeed[2].InitSpeed = (int)numParamNozzleZAxisUpInit.Value;
                Mparam.MotorSpeed[2].TopSpeed = (int)numParamNozzleZAxisUpTop.Value;
                Mparam.MotorSpeed[2].Accel = (int)numParamNozzleZAxisUpAccelerator.Value;
                Mparam.MotorSpeed[2].ConstSpeed = (int)numParamNozzleZAxisUpConst.Value;

                Mparam.MotorOffset[0] = (double)numParamNozzleZAxisOffsetReactionCell.Value;
                Mparam.MotorOffset[1] = (double)numParamNozzleZAxisOffsetCuvette.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustNozzleZAxisOffsetReactionCell.Value;
                Mparam.MotorOffset[1] = (double)numAdjustNozzleZAxisOffsetReactionCell.Value;       //Cuvetteの値も反応テーブル位置で更新する。（画面上にCuvetteは不要）
            }
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.OffsetReactionCell = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.OffsetCuvette = Mparam.MotorOffset[1];
            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);


            //BF1廃液部Z軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.BF1WasteNozzleZAxis;
            if (!adjustSave)
            {
                //モーターパラメータの保存
                //通常(ｿﾌﾄ固定）(※BF1 Nozzle Z軸と連動する）
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamNozzleZAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamNozzleZAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamNozzleZAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamNozzleZAxisConst.Value;
                //洗浄降下(ｿﾌﾄ固定）(※BF1 Nozzle Z軸と連動する）
                Mparam.MotorSpeed[1].InitSpeed = (int)numParamNozzleZAxisDownInit.Value;
                Mparam.MotorSpeed[1].TopSpeed = (int)numParamNozzleZAxisDownTop.Value;
                Mparam.MotorSpeed[1].Accel = (int)numParamNozzleZAxisDownAccelerator.Value;
                Mparam.MotorSpeed[1].ConstSpeed = (int)numParamNozzleZAxisDownConst.Value;
                //洗浄上昇(ｿﾌﾄ固定）(※BF1 Nozzle Z軸と連動する）
                Mparam.MotorSpeed[2].InitSpeed = (int)numParamNozzleZAxisUpInit.Value;
                Mparam.MotorSpeed[2].TopSpeed = (int)numParamNozzleZAxisUpTop.Value;
                Mparam.MotorSpeed[2].Accel = (int)numParamNozzleZAxisUpAccelerator.Value;
                Mparam.MotorSpeed[2].ConstSpeed = (int)numParamNozzleZAxisUpConst.Value;

                Mparam.MotorOffset[0] = (double)numParamWasteZAxisOffsetReactionCell.Value;
                Mparam.MotorOffset[1] = (double)numParamWasteZAxisOffsetCuvette.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1WasteNozzleZAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1WasteNozzleZAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1WasteNozzleZAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustWasteNozzleZAxisOffsetReactionCell.Value;
                Mparam.MotorOffset[1] = (double)numAdjustWasteNozzleZAxisOffsetReactionCell.Value;       //Cuvetteの値も反応テーブル位置で更新する。（画面上にCuvetteは不要）
            }
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1WasteNozzleZAxisParam.OffsetReactionCell = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1WasteNozzleZAxisParam.OffsetCuvette = Mparam.MotorOffset[1];
            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);

            if (!adjustSave)
            {
                //洗浄液ｼﾘﾝｼﾞ
                Mparam = new SlaveCommCommand_0471();
                Mparam.MotorNo = (int)MotorNoList.BFWashSyringe;
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamSyringeAspInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamSyringeAspTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamSyringeAspAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamSyringeAspConst.Value;

                Mparam.MotorSpeed[1].InitSpeed = (int)numParamSyringeDisInit.Value;
                Mparam.MotorSpeed[1].TopSpeed = (int)numParamSyringeDisTop.Value;
                Mparam.MotorSpeed[1].Accel = (int)numParamSyringeDisAccelerator.Value;
                Mparam.MotorSpeed[1].ConstSpeed = (int)numParamSyringeDisConst.Value;

                Mparam.MotorSpeed[2].InitSpeed = (int)numParamSyringePrimeInit.Value;
                Mparam.MotorSpeed[2].TopSpeed = (int)numParamSyringePrimeTop.Value;
                Mparam.MotorSpeed[2].Accel = (int)numParamSyringePrimeAccelerator.Value;
                Mparam.MotorSpeed[2].ConstSpeed = (int)numParamSyringePrimeConst.Value;

                Mparam.MotorOffset[0] = (double)numParamSyringeGain.Value;
                Mparam.MotorOffset[1] = (double)numParamSyringeOffset.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFWashSyringeParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFWashSyringeParam.MotorSpeed = Mparam.MotorSpeed;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFWashSyringeParam.Gain = Mparam.MotorOffset[0];
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFWashSyringeParam.Offset = Mparam.MotorOffset[1];

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

            switch (tabUnit.SelectedTab.Index)
            {
                case (int)MaintenanceTabIndex.Config:
                case (int)MaintenanceTabIndex.MParam:
                    Action Disp = () => { DispEnable(true); }; Invoke(Disp);
                    ToolbarsControl();
                    break;

                case (int)MaintenanceTabIndex.MAdjust:
                    ToolbarsControl((int)ToolBarEnable.Adjust);
                    break;
            }
        }

        /// <summary>
        /// モーターパラメータ画面表示
        /// </summary>
        public override void MotorParamDisp()
        {
            ParameterFilePreserve<CarisXMotorParameter> motor = new ParameterFilePreserve<CarisXMotorParameter>();
            motorLoad(motor);

            //BF1部Z軸
            //通常
            numParamNozzleZAxisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[0].InitSpeed;
            numParamNozzleZAxisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[0].TopSpeed;
            numParamNozzleZAxisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[0].Accel;
            numParamNozzleZAxisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[0].ConstSpeed;
            //洗浄降下
            numParamNozzleZAxisDownInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[1].InitSpeed;
            numParamNozzleZAxisDownTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[1].TopSpeed;
            numParamNozzleZAxisDownAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[1].Accel;
            numParamNozzleZAxisDownConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[1].ConstSpeed;
            //洗浄上昇
            numParamNozzleZAxisUpInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[2].InitSpeed;
            numParamNozzleZAxisUpTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[2].TopSpeed;
            numParamNozzleZAxisUpAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[2].Accel;
            numParamNozzleZAxisUpConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[2].ConstSpeed;

            // 反応テーブル位置オフセット
            numParamNozzleZAxisOffsetReactionCell.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.OffsetReactionCell;
            // 洗浄層位置オフセット
            numParamNozzleZAxisOffsetCuvette.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.OffsetCuvette;

            //モーター調整画面にも反映
            numAdjustNozzleZAxisOffsetReactionCell.Value = numParamNozzleZAxisOffsetReactionCell.Value;

            //BF1廃液部Z軸
            //通常(ｿﾌﾄ固定)
            numParamWasteZAxisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[0].InitSpeed;
            numParamWasteZAxisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[0].TopSpeed;
            numParamWasteZAxisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[0].Accel;
            numParamWasteZAxisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[0].ConstSpeed;
            //洗浄降下(ｿﾌﾄ固定)
            numParamWasteZAxisDownInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[1].InitSpeed;
            numParamWasteZAxisDownTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[1].TopSpeed;
            numParamWasteZAxisDownAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[1].Accel;
            numParamWasteZAxisDownConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[1].ConstSpeed;
            //洗浄上昇(ｿﾌﾄ固定)
            numParamWasteZAxisUpInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[2].InitSpeed;
            numParamWasteZAxisUpTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[2].TopSpeed;
            numParamWasteZAxisUpAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[2].Accel;
            numParamWasteZAxisUpConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1NozzleZAxisParam.MotorSpeed[2].ConstSpeed;

            // 反応テーブル位置オフセット
            numParamWasteZAxisOffsetReactionCell.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1WasteNozzleZAxisParam.OffsetReactionCell;
            // 洗浄層位置オフセット
            numParamWasteZAxisOffsetCuvette.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1WasteNozzleZAxisParam.OffsetCuvette;

            //モーター調整画面にも反映
            numAdjustWasteNozzleZAxisOffsetReactionCell.Value = numParamWasteZAxisOffsetReactionCell.Value;


            //洗浄液ｼﾘﾝｼﾞ
            numParamSyringeAspInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFWashSyringeParam.MotorSpeed[0].InitSpeed;
            numParamSyringeAspTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFWashSyringeParam.MotorSpeed[0].TopSpeed;
            numParamSyringeAspAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFWashSyringeParam.MotorSpeed[0].Accel;
            numParamSyringeAspConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFWashSyringeParam.MotorSpeed[0].ConstSpeed;

            numParamSyringeDisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFWashSyringeParam.MotorSpeed[1].InitSpeed;
            numParamSyringeDisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFWashSyringeParam.MotorSpeed[1].TopSpeed;
            numParamSyringeDisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFWashSyringeParam.MotorSpeed[1].Accel;
            numParamSyringeDisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFWashSyringeParam.MotorSpeed[1].ConstSpeed;

            numParamSyringePrimeInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFWashSyringeParam.MotorSpeed[2].InitSpeed;
            numParamSyringePrimeTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFWashSyringeParam.MotorSpeed[2].TopSpeed;
            numParamSyringePrimeAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFWashSyringeParam.MotorSpeed[2].Accel;
            numParamSyringePrimeConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFWashSyringeParam.MotorSpeed[2].ConstSpeed;

            numParamSyringeGain.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFWashSyringeParam.Gain;
            numParamSyringeOffset.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFWashSyringeParam.Offset;

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

            // BF1コンフィグ設定
            CarisXConfigParameter.BF1UnitConfigParam BF1ConfigParam = new CarisXConfigParameter.BF1UnitConfigParam();
            //洗浄液吸引後遅延時間
            BF1ConfigParam.WaitTimeAfterSuckingUp = (Double)numconfWaitTimeAfterSuckingUp.Value;
            //洗浄液吐出後遅延時間
            BF1ConfigParam.WaitTimeAfterDispense = (Double)numconfWaitTimeAfterDispense.Value;
            //洗浄間隔
            BF1ConfigParam.Interval = (double)numconfInterval.Value;
            //洗浄回数
            BF1ConfigParam.NoOfWashTimes = (int)numconfNoOfWashTimes.Value;
            //プライム回数
            BF1ConfigParam.NoOfPrimeTimes = (int)numconfNoOfPrimeTimes.Value;
            //リンス回数
            BF1ConfigParam.NoOfRinseTimes = (int)numconfNoOfRinseTimes.Value;
            //プライム量
            BF1ConfigParam.PrimeVolume = (int)numconfPrimeVolume.Value;
            //洗浄液量
            BF1ConfigParam.WashVolume = (int)numconfWashVolume.Value;
            //廃液　洗浄液吸引後遅延時間
            BF1ConfigParam.WaitTimeWaste = (double)numconfWaitTimeWaste.Value;

            config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1UnitConfigParam = BF1ConfigParam;

            //パラメータ送信
            SendParam(BF1ConfigParam);

            // BF2コンフィグ設定
            CarisXConfigParameter.BF2UnitConfigParam BF2ConfigParam = new CarisXConfigParameter.BF2UnitConfigParam();
            //BFユニット開始時間（サイクル）
            BF2ConfigParam.WaitTimeAfterSuckingUp = (double)numconfDelayTimeCycleStartTo1stAspiration.Value;
            //BF一回目吐出後の吸引までの待ち時間
            BF2ConfigParam.WaitTimeAfterDispense = (double)numconfDelayTime1stDispenseTo2ndAspiration.Value;
            //BF二回目吐出後の吸引までの待ち時間
            BF2ConfigParam.Interval = (double)numconfDelayTime2ndDispenseTo3rdAspiration.Value;
            //BF三回目吸引後の残液量
            BF2ConfigParam.WashVolume = (int)numconfVolumeAfter3rdAspiration.Value;

            config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2UnitConfigParam = BF2ConfigParam;

            //パラメータ送信
            SendParam(BF2ConfigParam);

        }

        /// <summary>
        /// コンフィグパラメータ読み込み
        /// </summary>
        public override void ConfigParamLoad()
        {
            ParameterFilePreserve<CarisXConfigParameter> config = Singleton<ParameterFilePreserve<CarisXConfigParameter>>.Instance;
            configLoad(config);

            //洗浄液吸引後遅延時間
            numconfWaitTimeAfterSuckingUp.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1UnitConfigParam.WaitTimeAfterSuckingUp;
            //洗浄液吐出後遅延時間
            numconfWaitTimeAfterDispense.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1UnitConfigParam.WaitTimeAfterDispense;

            //洗浄間隔
            numconfInterval.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1UnitConfigParam.Interval;
            //洗浄回数
            numconfNoOfWashTimes.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1UnitConfigParam.NoOfWashTimes;
            //プライム回数
            numconfNoOfPrimeTimes.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1UnitConfigParam.NoOfPrimeTimes;
            //リンス回数
            numconfNoOfRinseTimes.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1UnitConfigParam.NoOfRinseTimes;
            //プライム量
            numconfPrimeVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1UnitConfigParam.PrimeVolume;
            //洗浄液量
            numconfWashVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1UnitConfigParam.WashVolume;
            //廃液　洗浄液吸引後遅延時間
            numconfWaitTimeWaste.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1UnitConfigParam.WaitTimeWaste;

            //BFユニット開始時間（サイクル）
            numconfDelayTimeCycleStartTo1stAspiration.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2UnitConfigParam.WaitTimeAfterSuckingUp;
            //BF一回目吐出後の吸引までの待ち時間
            numconfDelayTime1stDispenseTo2ndAspiration.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2UnitConfigParam.WaitTimeAfterDispense;
            //BF二回目吐出後の吸引までの待ち時間
            numconfDelayTime2ndDispenseTo3rdAspiration.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2UnitConfigParam.Interval;
            //BF三回目吸引後の残液量
            numconfVolumeAfter3rdAspiration.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2UnitConfigParam.WashVolume;
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
            cmbAdjustCoarseFine.Enabled = false;

            gbxAdjustNozzleZAxis.Enabled = false;
            numAdjustNozzleZAxisOffsetReactionCell.Enabled = false;
            gbxAdjustWasteNozzleZAxis.Enabled = false;
            numAdjustWasteNozzleZAxisOffsetReactionCell.Enabled = false;

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
            AdjustStartComm.Arg1 = (int)cmbAdjustCoarseFine.Value;

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
            btnAdjustNozzleZAxisDown.Enabled = enable;
            btnAdjustNozzleZAxisUp.Enabled = enable;
            btnAdjustWasteNozzleZAxisDown.Enabled = enable;
            btnAdjustWasteNozzleZAxisUp.Enabled = enable;
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
                case (int)MotorAdjustStopPosition.Wash1Prime:
                    cmbAdjustCoarseFine.Enabled = true;
                    gbxAdjustNozzleZAxis.Enabled = true;
                    numAdjustNozzleZAxisOffsetReactionCell.Enabled = true;
                    numAdjustNozzleZAxisOffsetReactionCell.ForeColor = System.Drawing.Color.OrangeRed;
                    break;

                case (int)MotorAdjustStopPosition.WasteWash1Prime:
                    cmbAdjustCoarseFine.Enabled = true;
                    gbxAdjustWasteNozzleZAxis.Enabled = true;
                    numAdjustWasteNozzleZAxisOffsetReactionCell.Enabled = true;
                    numAdjustWasteNozzleZAxisOffsetReactionCell.ForeColor = System.Drawing.Color.OrangeRed;
                    break;
            }
        }

        /// <summary>
        /// Pitch値のUp（BF1ノズルZ軸）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPitchUpNozzle_Click(object sender, EventArgs e)
        {
            numAdjustNozzleZAxisPitch.Value = (double)numAdjustNozzleZAxisPitch.Value + (double)numAdjustNozzleZAxisPitch.SpinIncrement;
        }

        /// <summary>
        /// Pitch値のDown（BF1ノズルZ軸）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPitchDownNozzle_Click(object sender, EventArgs e)
        {
            numAdjustNozzleZAxisPitch.Value = (double)numAdjustNozzleZAxisPitch.Value - (double)numAdjustNozzleZAxisPitch.SpinIncrement;
        }

        /// <summary>
        /// Pitch値のUp（BF1廃液ノズルZ軸）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPitchUpWasteNozzle_Click(object sender, EventArgs e)
        {
            numAdjustWasteNozzleZAxisPitch.Value = (double)numAdjustWasteNozzleZAxisPitch.Value + (double)numAdjustNozzleZAxisPitch.SpinIncrement;
        }

        /// <summary>
        /// Pitch値のDown（BF1廃液ノズルZ軸）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPitchDownWasteNozzle_Click(object sender, EventArgs e)
        {
            numAdjustWasteNozzleZAxisPitch.Value = (double)numAdjustWasteNozzleZAxisPitch.Value - (double)numAdjustNozzleZAxisPitch.SpinIncrement;
        }

        private void btnAdjustNozzleZAxisDownUp_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustNozzleZAxisDown);
            Int32 motorNo = (int)MotorNoList.BF1NozzleZAxis;
            Double pitchValue = (double)numAdjustNozzleZAxisPitch.Value;

            AdjustValue(upFlg, motorNo, numAdjustNozzleZAxisOffsetReactionCell, pitchValue);
        }

        private void btnAdjustWasteNozzleZAxisDownUp_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustWasteNozzleZAxisDown);
            Int32 motorNo = (int)MotorNoList.BF1WasteNozzleZAxis;
            Double pitchValue = (double)numAdjustWasteNozzleZAxisPitch.Value;

            AdjustValue(upFlg, motorNo, numAdjustWasteNozzleZAxisOffsetReactionCell, pitchValue);
        }
    }
}
