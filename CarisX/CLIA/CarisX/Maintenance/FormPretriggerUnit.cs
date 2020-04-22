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
    /// プレトリガ
    /// </summary>
    public partial class FormPretriggerUnit : FormUnitBase
    {
        IMaintenanceUnitStart unitStart = new UnitStart();
        IMaintenanceUnitStart unitStartAdjust = new UnitStarAdjust();
        IMaintenanceUnitStart unitAdjust = new UnitAdjust();
        IMaintenanceUnitStart unitAdjustAbort = new UnitStarAdjustAbort();
        volatile bool ResponseFlg;
        public CommonFunction ComFunc = new CommonFunction();

        public FormPretriggerUnit()
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
        }

        #region リソース設定
        private void setCulture()
        {
            //Tab
            tabUnit.Tabs[0].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_010;        //Test
            tabUnit.Tabs[1].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_011;        //Configuration
            tabUnit.Tabs[2].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_012;        //Motor Parameters

            //Testタブ
            gbxtestSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_000;              //Sequence
            object SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_001, No=(int)PretriggerSequence.Init},                     //1: Unit Initialization
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_002, No=(int)PretriggerSequence.DispensePretrigger},       //2: Dispense Pretrigger
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_003, No=(int)PretriggerSequence.Prime},                    //3: Prime
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_004, No=(int)PretriggerSequence.Rinse},                    //4: Rinse
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_005, No=(int)PretriggerSequence.PrimeExchangeBottle},      //5: Prime to Exchange Bottle
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_045, No=(int)PretriggerSequence.MovePretriggerDispense},   //6: Move to Adjusted Position for Pre-trigger Dispensing
            };
            lbxtestSequenceListBox = ComFunc.setSequenceListBox(lbxtestSequenceListBox, SequenceList);

            lbltestRepeatFrequency.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_006;       //Repeat Frequency
            lbltestNumber.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_007;                //Number
            lbltestRepeatInterval.Text = Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_049;
            lbltestRepeatIntervalUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_050;
            lbltestBottle.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_046;                //Bottle
            rbttestBottle1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_047;               //1
            rbttestBottle2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_048;               //2

            gbxtestResponce.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_009;              //Response

            //Configurationタブ
            gbxconfParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_013;//Parameters

            lblconfWaitTimeAfterSuckingUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_014;//The Delay Time after Pretrigger Aspiration
            lblconfWaitTimeAfterDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_015;//The Delay Time after Pretrigger Dispense
            lblconfNoOfPrimeTimes.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_016;//The Number of Times of Prime
            lblconfNoOfRinseTimes.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_017;//The Number of Times of Rinse
            lblconfPrimeVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_018;//Prime Volume
            lblconfStandbyPosition.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_019;//Pretrigger Dispense Syringe Standby Position
            lblconfDispanseVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_020;//Pretrigger Dispense Volume

            lblconfWaitTimeAfterSuckingUpUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_021;//(sec)
            lblconfWaitTimeAfterDispenseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_021;//(sec)

            lblconfNoOfPrimeTimesUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_022;//(times)
            lblconfNoOfRinseTimesUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_022;//(times)

            lblconfPrimeVolumeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_023;//(uL)
            lblconfStandbyPositionUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_023;//(uL)
            lblconfDispanseVolumeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_023;//(uL)

            //MotorParameterタブ
            gbxParamSyringe.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_024;
            gbxParamSyringeAsp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_029;
            gbxParamSyringeDis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_030;
            gbxParamSyringeAir.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_031;
            gbxParamSyringePrime.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_032;
            lblParamSyringeAspInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_025;
            lblParamSyringeAspTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_026;
            lblParamSyringeAspAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_027;
            lblParamSyringeAspConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_028;
            lblParamSyringeDisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_025;
            lblParamSyringeDisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_026;
            lblParamSyringeDisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_027;
            lblParamSyringeDisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_028;
            lblParamSyringeAirInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_025;
            lblParamSyringeAirTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_026;
            lblParamSyringeAirAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_027;
            lblParamSyringeAirConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_028;
            lblParamSyringePrimeRinseInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_025;
            lblParamSyringePrimeRinseTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_026;
            lblParamSyringePrimeRinseAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_027;
            lblParamSyringePrimeRinseConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_028;
            lblParamSyringeAspInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_041;
            lblParamSyringeAspTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_041;
            lblParamSyringeAspAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_042;
            lblParamSyringeAspConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_041;
            lblParamSyringeDisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_041;
            lblParamSyringeDisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_041;
            lblParamSyringeDisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_042;
            lblParamSyringeDisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_041;
            lblParamSyringeAirInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_041;
            lblParamSyringeAirTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_041;
            lblParamSyringeAirAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_042;
            lblParamSyringeAirConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_041;
            lblParamSyringePrimeRinseInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_041;
            lblParamSyringePrimeRinseTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_041;
            lblParamSyringePrimeRinseAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_042;
            lblParamSyringePrimeRinseConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_041;
            lblParamSyringeGain.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_043;
            lblParamSyringeOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_PRETRIGGER_044;

        }
        #endregion

        /// <summary>
        /// ラジオボタンの選択値を設定
        /// </summary>
        private void setRadioButtonValue()
        {
            rbttestBottle1.Tag = (int)PretriggerRadioValue.Bottle1;
            rbttestBottle2.Tag = (int)PretriggerRadioValue.Bottle2;
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
            Func<object> getfuncno = () => { return lbxtestSequenceListBox.SelectedValue; };
            int FuncNo = (int)Invoke(getfuncno);

            SlaveCommCommand_0439 StartComm = new SlaveCommCommand_0439();
            StartComm.UnitNo = (int)UnitNoList.PreTrigger;
            StartComm.FuncNo = FuncNo;

            switch (FuncNo)
            {
                case (int)PretriggerSequence.DispensePretrigger:
                case (int)PretriggerSequence.Prime:
                case (int)PretriggerSequence.PrimeExchangeBottle:
                    StartComm.Arg1 = ComFunc.getSelectedRadioButtonValue(gbxtestBottle);
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
            }
        }

        /// <summary>
        /// パラメータをすべて非活性にする
        /// </summary>
        private void ParametersAllFalse()
        {
            gbxtestBottle.Enabled = false;
        }

        /// <summary>
        /// 選択された機能番号に応じてパラメータを活性化する
        /// </summary>
        private void lbxtestSequenceListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParametersAllFalse();
            switch (lbxtestSequenceListBox.SelectedValue)
            {
                case (int)PretriggerSequence.DispensePretrigger:
                case (int)PretriggerSequence.Prime:
                case (int)PretriggerSequence.PrimeExchangeBottle:
                    gbxtestBottle.Enabled = true;
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
        private void MotorParamSave()
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

            //プレトリガ液シリンジ
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.PreTriggerDispenseSyringe;
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

            Mparam.MotorSpeed[3].InitSpeed = (int)numParamSyringePrimeRinseInit.Value;
            Mparam.MotorSpeed[3].TopSpeed = (int)numParamSyringePrimeRinseTop.Value;
            Mparam.MotorSpeed[3].Accel = (int)numParamSyringePrimeRinseAccelerator.Value;
            Mparam.MotorSpeed[3].ConstSpeed = (int)numParamSyringePrimeRinseConst.Value;

            Mparam.MotorOffset[0] = (double)numParamSyringeGain.Value;
            Mparam.MotorOffset[1] = (double)numParamSyringeOffset.Value;

            //パラメータセット
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.MotorNo = Mparam.MotorNo;
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.MotorSpeed = Mparam.MotorSpeed;
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.Gain = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.Offset = Mparam.MotorOffset[1];

            //モータパラメータ送信
            SendParam(Mparam);
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
            Action Disp = () => { DispEnable(true); }; Invoke(Disp);

            ToolbarsControl();

        }

        /// <summary>
        /// モーターパラメータ画面表示
        /// </summary>
        public override void MotorParamDisp()
        {
            ParameterFilePreserve<CarisXMotorParameter> motor = Singleton<ParameterFilePreserve<CarisXMotorParameter>>.Instance;
            motorLoad(motor);

            //プレトリガ液ｼﾘﾝｼﾞ
            numParamSyringeAspInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.MotorSpeed[0].InitSpeed;
            numParamSyringeAspTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.MotorSpeed[0].TopSpeed;
            numParamSyringeAspAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.MotorSpeed[0].Accel;
            numParamSyringeAspConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.MotorSpeed[0].ConstSpeed;

            numParamSyringeDisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.MotorSpeed[1].InitSpeed;
            numParamSyringeDisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.MotorSpeed[1].TopSpeed;
            numParamSyringeDisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.MotorSpeed[1].Accel;
            numParamSyringeDisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.MotorSpeed[1].ConstSpeed;

            numParamSyringeAirInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.MotorSpeed[2].InitSpeed;
            numParamSyringeAirTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.MotorSpeed[2].TopSpeed;
            numParamSyringeAirAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.MotorSpeed[2].Accel;
            numParamSyringeAirConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.MotorSpeed[2].ConstSpeed;

            numParamSyringePrimeRinseInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.MotorSpeed[3].InitSpeed;
            numParamSyringePrimeRinseTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.MotorSpeed[3].TopSpeed;
            numParamSyringePrimeRinseAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.MotorSpeed[3].Accel;
            numParamSyringePrimeRinseConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.MotorSpeed[3].ConstSpeed;

            numParamSyringeGain.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.Gain;
            numParamSyringeOffset.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].preTriggerDispenseSyringeParam.Offset;

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

            CarisXConfigParameter.PretriggerUnitConfigParam ConfigParam = new CarisXConfigParameter.PretriggerUnitConfigParam();
            //pTr吸引後遅延時間
            ConfigParam.WaitTimeAfterSuckingUp = (double)numconfWaitTimeAfterSuckingUp.Value;
            //pTr吐出後遅延時間
            ConfigParam.WaitTimeAfterDispense = (double)numconfWaitTimeAfterDispense.Value;
            //プライム回数
            ConfigParam.NoOfPrimeTimes = (int)numconfNoOfPrimeTimes.Value;
            //リンス回数
            ConfigParam.NoOfRinseTimes = (int)numconfNoOfRinseTimes.Value;
            //プライム量
            ConfigParam.PrimeVolume = (int)numconfPrimeVolume.Value;
            //pTrシリンジ待機位置
            ConfigParam.StandbyPosition = (int)numconfStandbyPosition.Value;
            //pTr吐出量
            ConfigParam.DispanseVolume = (int)numconfDispanseVolume.Value;

            config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].pretriggerUnitConfigParam = ConfigParam;

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

            //pTr吸引後遅延時間
            numconfWaitTimeAfterSuckingUp.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].pretriggerUnitConfigParam.WaitTimeAfterSuckingUp;
            //pTr吐出後遅延時間
            numconfWaitTimeAfterDispense.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].pretriggerUnitConfigParam.WaitTimeAfterDispense;
            //プライム回数
            numconfNoOfPrimeTimes.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].pretriggerUnitConfigParam.NoOfPrimeTimes;
            //リンス回数
            numconfNoOfRinseTimes.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].pretriggerUnitConfigParam.NoOfRinseTimes;
            //プライム量
            numconfPrimeVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].pretriggerUnitConfigParam.PrimeVolume;
            //pTrシリンジ待機位置
            numconfStandbyPosition.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].pretriggerUnitConfigParam.StandbyPosition;
            //pTr吐出量
            numconfDispanseVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].pretriggerUnitConfigParam.DispanseVolume;
        }

        /// <summary>
        /// パラメータ保存
        /// </summary>
        public override void ParamSave()
        {
            if (tabUnit.SelectedTab.Index == 1)
            {
                //コンフィグパラメータセーブ
                ConfigParamSave();
            }
            else if (tabUnit.SelectedTab.Index == 2)
            {
                //モーターパラメータセーブ
                MotorParamSave();
            }
        }
    }
}
