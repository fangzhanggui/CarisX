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
    /// BF2部
    /// </summary>
    public partial class FormBF2Unit : FormUnitBase
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
        public FormBF2Unit()
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
            tabUnit.Tabs[0].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_013;                          //Test
            tabUnit.Tabs[1].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_054;                          //Configuration
            tabUnit.Tabs[2].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_055;                          //Motor Parameters
            tabUnit.Tabs[3].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_MOTORADJUSTMENT;     //Adjust

            //Testタブ
            gbxtestSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_000;                         //Sequence
            object SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_001, No=(int)BF2Sequence.Init},               //1: Unit Initialization
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_002, No=(int)BF2Sequence.BF2},                //2: BF 2
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_003, No=(int)BF2Sequence.WasteLiquid},        //3: Waste Liquid
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_004, No=(int)BF2Sequence.Prime},              //4: Prime
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_005, No=(int)BF2Sequence.Rinse},              //5: Rinse
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_052, No=(int)BF2Sequence.MovetoBF2Washing},   //6: Move to Adjust Position for BF 2 Washing
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_053, No=(int)BF2Sequence.BF2Dispense},        //7: BF 2 Dispense
            };
            lbxtestSequenceListBox = ComFunc.setSequenceListBox(lbxtestSequenceListBox, SequenceList);

            gbxtestParameters.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_006;                       //Parameters
            lbltestRepeatFrequency.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_007;                  //Repeat Frequency
            lbltestNumber.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_008;                           //Number
            lbltestRepeatInterval.Text = Resources_Maintenance.STRING_MAINTENANCE_BF2_062;
            lbltestRepeatIntervalUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_BF2_063;
            lbltestNozzle.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_056;                           //Nozzle
            rbttestAllNozzle.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_009;                        //All Nozzle
            rbttestNozzle1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_010;                          //Nozzle 1
            rbttestNozzle2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_011;                          //Nozzle 2
            rbttestNozzle3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_057;                          //Nozzle 3

            gbxtestResponce.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_012;                         //Responce

            //Configurationタブ
            gbxconfParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_006;                          //Parameters
            lblconfWaitTimeAfterSuckingUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_014;//The Delay Time after Wash Solution Aspiration
            lblconfWaitTimeAfterDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_015;//The Delay Time after Wash Solution Dispense
            lblconfInterval.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_016;//Washing Intervals
            lblconfNoOfWashTimes.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_017;//The Number of Times of Wash
            lblconfNoOfPrimeTimes.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_018;//The Number of Times of Prime
            lblconfNoOfRinseTimes.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_019;//The Number of Times of Rinse
            lblconfPrimeVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_020;//Prime Volume
            lblconfWashVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_021;//Washing Volume

            lblconfWaitTimeAfterSuckingUpUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_022;//(sec)
            lblconfWaitTimeAfterDispenseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_022;//(sec)
            lblconfIntervalUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_022;//(sec)

            lblconfNoOfWashTimesUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_023;//(times)
            lblconfNoOfPrimeTimesUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_023;//(times)
            lblconfNoOfRinseTimesUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_023;//(times)

            lblconfPrimeVolumeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_024;//(uL)
            lblconfWashVolumeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_024;//(uL)

            //MotorParameterタブ
            gbxParamNozzleZAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_025;
            gbxParamNozzleZAxisCommon.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_031;
            gbxParamNozzleZAxisDown.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_033;
            gbxParamNozzleZAxisUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_034;
            gbxParamNozzleZAxisOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_035;
            lblParamNozzleZAxisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_026;
            lblParamNozzleZAxisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_027;
            lblParamNozzleZAxisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_028;
            lblParamNozzleZAxisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_029;
            lblParamNozzleZAxisDownInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_026;
            lblParamNozzleZAxisDownTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_027;
            lblParamNozzleZAxisDownAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_028;
            lblParamNozzleZAxisDownConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_029;
            lblParamNozzleZAxisUpInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_026;
            lblParamNozzleZAxisUpTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_027;
            lblParamNozzleZAxisUpAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_028;
            lblParamNozzleZAxisUpConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_029;
            lblParamNozzleZAxisOffsetReactionCell.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_030;
            lblParamNozzleZAxisOffsetCuvette.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_036;
            lblParamNozzleZAxisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_047;
            lblParamNozzleZAxisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_047;
            lblParamNozzleZAxisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_048;
            lblParamNozzleZAxisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_047;
            lblParamNozzleZAxisDownInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_047;
            lblParamNozzleZAxisDownTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_047;
            lblParamNozzleZAxisDownAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_048;
            lblParamNozzleZAxisDownConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_047;
            lblParamNozzleZAxisUpInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_047;
            lblParamNozzleZAxisUpTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_047;
            lblParamNozzleZAxisUpAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_048;
            lblParamNozzleZAxisUpConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_047;
            lblParamNozzleZAxisOffsetReactionCellUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_049;
            lblParamNozzleZAxisOffsetCuvetteUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_049;

            //MotorAdjustタブ
            gbxAdjustSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_000;
            SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_058, No=(int)MotorAdjustStopPosition.Wash2Prime},
            };
            lbxAdjustSequence = ComFunc.setSequenceListBox(lbxAdjustSequence, SequenceList);

            gbxAdjustParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_006;
            lblAdjustCoarseFine.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_COARSEFINE;
            cmbValueCoarseFine = new Infragistics.Win.ValueListItem[]
            {
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_BF2_060, dataValue:MotorAdjustCoarseFine.Coarse),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_BF2_061, dataValue:MotorAdjustCoarseFine.Fine),
            };
            cmbAdjustCoarseFine.Items.AddRange(cmbValueCoarseFine);

            gbxAdjustNozzleZAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_025;
            lblAdjustNozzleZAxisOffsetReactionCell.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_030;
            lblAdjustNozzleZAxisOffsetReactionCellUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_BF2_049;

            //グループボックス
            //UP/Down
            btnAdjustNozzleZAxisDown.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_DOWN;
            btnAdjustNozzleZAxisUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_UP;

            //Pitch
            lblAdjustNozzleZAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;

        }
        #endregion

        /// <summary>
        /// ラジオボタンの選択値を設定
        /// </summary>
        private void setRadioButtonValue()
        {
            rbttestAllNozzle.Tag = (int)BF2RadioValue.AllNozzle;
            rbttestNozzle1.Tag = (int)BF2RadioValue.Nozzle1;
            rbttestNozzle2.Tag = (int)BF2RadioValue.Nozzle2;
            rbttestNozzle3.Tag = (int)BF2RadioValue.Nozzle3;
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

                    SlaveCommCommand_0439 StartComm = new SlaveCommCommand_0439();
                    StartComm.UnitNo = (int)UnitNoList.BF2;
                    StartComm.FuncNo = FuncNo;

                    switch (FuncNo)
                    {
                        case (int)BF2Sequence.BF2:
                        case (int)BF2Sequence.WasteLiquid:
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

                    // メイン画面のナビゲータバーをEnableコマンドバーをデフォルトに戻す
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
            rbttestNozzle3.Enabled = false;
        }

        /// <summary>
        /// 選択された機能番号に応じてパラメータを活性化する
        /// </summary>
        private void lbxtestSequenceListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParametersAllFalse();
            switch (lbxtestSequenceListBox.SelectedValue)
            {
                case (int)BF2Sequence.BF2:
                    gbxtestNozzle.Enabled = true;

                    rbttestAllNozzle.Checked = true;
                    break;
                case (int)BF2Sequence.WasteLiquid:
                    gbxtestNozzle.Enabled = true;
                    rbttestNozzle3.Enabled = true;

                    rbttestAllNozzle.Checked = true;
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

            //BF2部Z軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.BF2NozzleZAxis;
            if (!adjustSave)
            {
                //モーターパラメータの保存
                //通常
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamNozzleZAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamNozzleZAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamNozzleZAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamNozzleZAxisConst.Value;
                //洗浄降下(ｿﾌﾄ固定）   
                Mparam.MotorSpeed[1].InitSpeed = (int)numParamNozzleZAxisDownInit.Value;
                Mparam.MotorSpeed[1].TopSpeed = (int)numParamNozzleZAxisDownTop.Value;
                Mparam.MotorSpeed[1].Accel = (int)numParamNozzleZAxisDownAccelerator.Value;
                Mparam.MotorSpeed[1].ConstSpeed = (int)numParamNozzleZAxisDownConst.Value;
                //洗浄上昇(ｿﾌﾄ固定）
                Mparam.MotorSpeed[2].InitSpeed = (int)numParamNozzleZAxisUpInit.Value;
                Mparam.MotorSpeed[2].TopSpeed = (int)numParamNozzleZAxisUpTop.Value;
                Mparam.MotorSpeed[2].Accel = (int)numParamNozzleZAxisUpAccelerator.Value;
                Mparam.MotorSpeed[2].ConstSpeed = (int)numParamNozzleZAxisUpConst.Value;

                Mparam.MotorOffset[0] = (double)numParamNozzleZAxisOffsetReactionCell.Value;
                Mparam.MotorOffset[1] = (double)numParamNozzleZAxisOffsetCuvette.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2NozzleZAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2NozzleZAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2NozzleZAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustNozzleZAxisOffsetReactionCell.Value;
                Mparam.MotorOffset[1] = (double)numAdjustNozzleZAxisOffsetReactionCell.Value;       //Cuvetteの値も反応テーブル位置で更新する。（画面上にCuvetteは不要）
            }
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2NozzleZAxisParam.OffsetReactionCell = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2NozzleZAxisParam.OffsetCuvette = Mparam.MotorOffset[1];
            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);

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

            //BF2部Z軸
            //通常
            numParamNozzleZAxisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2NozzleZAxisParam.MotorSpeed[0].InitSpeed;
            numParamNozzleZAxisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2NozzleZAxisParam.MotorSpeed[0].TopSpeed;
            numParamNozzleZAxisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2NozzleZAxisParam.MotorSpeed[0].Accel;
            numParamNozzleZAxisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2NozzleZAxisParam.MotorSpeed[0].ConstSpeed;
            //洗浄降下(ｿﾌﾄ固定）
            numParamNozzleZAxisDownInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2NozzleZAxisParam.MotorSpeed[1].InitSpeed;
            numParamNozzleZAxisDownTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2NozzleZAxisParam.MotorSpeed[1].TopSpeed;
            numParamNozzleZAxisDownAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2NozzleZAxisParam.MotorSpeed[1].Accel;
            numParamNozzleZAxisDownConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2NozzleZAxisParam.MotorSpeed[1].ConstSpeed;
            //洗浄上昇(ｿﾌﾄ固定）
            numParamNozzleZAxisUpInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2NozzleZAxisParam.MotorSpeed[2].InitSpeed;
            numParamNozzleZAxisUpTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2NozzleZAxisParam.MotorSpeed[2].TopSpeed;
            numParamNozzleZAxisUpAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2NozzleZAxisParam.MotorSpeed[2].Accel;
            numParamNozzleZAxisUpConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2NozzleZAxisParam.MotorSpeed[2].ConstSpeed;

            // 反応テーブル位置オフセット
            numParamNozzleZAxisOffsetReactionCell.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2NozzleZAxisParam.OffsetReactionCell;
            // 洗浄層位置オフセット
            numParamNozzleZAxisOffsetCuvette.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2NozzleZAxisParam.OffsetCuvette;

            //モーター調整画面にも反映
            numAdjustNozzleZAxisOffsetReactionCell.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2NozzleZAxisParam.OffsetReactionCell;
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

            CarisXConfigParameter.BF2UnitConfigParam ConfigParam = new CarisXConfigParameter.BF2UnitConfigParam();
            //洗浄液吸引後遅延時間
            ConfigParam.WaitTimeAfterSuckingUp = (double)numconfWaitTimeAfterSuckingUp.Value;
            //洗浄液吐出後遅延時間
            ConfigParam.WaitTimeAfterDispense = (double)numconfWaitTimeAfterDispense.Value;
            //洗浄間隔
            ConfigParam.Interval = (double)numconfInterval.Value;
            //洗浄回数
            ConfigParam.NoOfWashTimes = (int)numconfNoOfWashTimes.Value;
            //プライム回数
            ConfigParam.NoOfPrimeTimes = (int)numconfNoOfPrimeTimes.Value;
            //リンス回数
            ConfigParam.NoOfRinseTimes = (int)numconfNoOfRinseTimes.Value;
            //プライム量
            ConfigParam.PrimeVolume = (int)numconfPrimeVolume.Value;
            //洗浄液量
            ConfigParam.WashVolume = (int)numconfWashVolume.Value;

            config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2UnitConfigParam = ConfigParam;

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

            //洗浄液吸引後遅延時間
            numconfWaitTimeAfterSuckingUp.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2UnitConfigParam.WaitTimeAfterSuckingUp;
            //洗浄液吐出後遅延時間
            numconfWaitTimeAfterDispense.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2UnitConfigParam.WaitTimeAfterDispense;
            //洗浄間隔
            numconfInterval.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2UnitConfigParam.Interval;
            //洗浄回数
            numconfNoOfWashTimes.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2UnitConfigParam.NoOfWashTimes;
            //プライム回数
            numconfNoOfPrimeTimes.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2UnitConfigParam.NoOfPrimeTimes;
            //リンス回数
            numconfNoOfRinseTimes.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2UnitConfigParam.NoOfRinseTimes;
            //プライム量
            numconfPrimeVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2UnitConfigParam.PrimeVolume;
            //洗浄液量
            numconfWashVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2UnitConfigParam.WashVolume;
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
            gbxAdjustNozzleZAxis.Enabled = false;
            numAdjustNozzleZAxisOffsetReactionCell.Enabled = false;

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
                case (int)MotorAdjustStopPosition.Wash2Prime:
                    gbxAdjustNozzleZAxis.Enabled = true;
                    numAdjustNozzleZAxisOffsetReactionCell.Enabled = true;
                    numAdjustNozzleZAxisOffsetReactionCell.ForeColor = System.Drawing.Color.OrangeRed;
                    break;
            }
        }

        /// <summary>
        /// Pitch値のUp（BF2ノズルZ軸）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdjustNozzleZAxisPitchUp_Click(object sender, EventArgs e)
        {
            numAdjustNozzleZAxisPitch.Value = (double)numAdjustNozzleZAxisPitch.Value + (double)numAdjustNozzleZAxisPitch.SpinIncrement;
        }

        /// <summary>
        /// Pitch値のDown（BF2ノズルZ軸）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdjustNozzleZAxisPitchDown_Click(object sender, EventArgs e)
        {
            numAdjustNozzleZAxisPitch.Value = (double)numAdjustNozzleZAxisPitch.Value - (double)numAdjustNozzleZAxisPitch.SpinIncrement;
        }

        /// <summary>
        /// Upボタンクリック（BF2ノズルZ軸）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdjustNozzleZAxisDownUp_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustNozzleZAxisDown);
            Int32 motorNo = (int)MotorNoList.BF2NozzleZAxis;
            Double pitchValue = (double)numAdjustNozzleZAxisPitch.Value;

            AdjustValue(upFlg, motorNo, numAdjustNozzleZAxisOffsetReactionCell, pitchValue);
        }
    }
}
