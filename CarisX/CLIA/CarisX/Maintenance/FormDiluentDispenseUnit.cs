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
    /// 希釈液分注部
    /// </summary>
    public partial class FormDiluentDispenseUnit : FormUnitBase
    {
        IMaintenanceUnitStart unitStart = new UnitStart();
        IMaintenanceUnitStart unitStartAdjust = new UnitStarAdjust();
        IMaintenanceUnitStart unitAdjust = new UnitAdjust();
        IMaintenanceUnitStart unitAdjustAbort = new UnitStarAdjustAbort();
        volatile bool ResponseFlg;
        public CommonFunction ComFunc = new CommonFunction();

        //モーター調整コマンドチクチク
        SlaveCommCommand_0473 AdjustUpDownComm = new SlaveCommCommand_0473();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormDiluentDispenseUnit()
        {

            InitializeComponent();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ConfigParamLoad();
            MotorParamDisp();
            setCulture();
            ComFunc.SetControlSettings(this);
            ConfigTabUseFlg = tabUnit.Tabs[(int)MaintenanceTabIndex.Config].Enabled;    //Configタブを利用有無を退避

            lbxtestSequenceListBox.SelectedIndex = 0;
            lbxAdjustSequence.SelectedIndex = 0;
        }

        #region リソース設定

        /// <summary>
        /// リソース設定
        /// </summary>
        private void setCulture()
        {
            //Tab
            tabUnit.Tabs[0].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_009;         //Test
            tabUnit.Tabs[1].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_010;         //Configuration
            tabUnit.Tabs[2].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_011;         //Motor Parameters
            tabUnit.Tabs[3].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_MOTORADJUSTMENT; //Adjust

            //Testタブ
            gbxtestSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_000; //Sequence
            object SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_001, No=(int)DiluentDispenseSequence.Init},                      //1: Unit Initialization
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_002, No=(int)DiluentDispenseSequence.DiluentDispense},           //2: Diluent Dispense
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_003, No=(int)DiluentDispenseSequence.Prime},                     //3: Prime
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_004, No=(int)DiluentDispenseSequence.Rinse},                     //4: Rinse
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_039, No=(int)DiluentDispenseSequence.MovetoDiluentDispense},     //5: Move to Adjusted Position for Diluent Dispensing
            };
            lbxtestSequenceListBox = ComFunc.setSequenceListBox(lbxtestSequenceListBox, SequenceList);

            gbxtestParameters.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_005;                  //Parameters
            lbltestRepeatFrequency.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_006;             //Repeat Frequency
            lbltestNumber.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_007;                      //Number
            lbltestRepeatInterval.Text = Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_042;
            lbltestRepeatIntervalUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_043;

            gbxtestResponce.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_008;                    //Responce

            //Configurationタブ
            gbxconfParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_005;//Parameters
            lblconfWaitTimeAfterSuckingUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_012;//The Delay Time after Diluent Aspiration
            lblconfWaitTimeAfterDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_013;//The Delay Time after Diluent Dispense
            lblconfStandbyPosition.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_014;//Diluent Dispense Syringe Standby Position
            lblconfNoOfDilutiePrimeTimes.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_015;//The Number of Times of Prime
            lblconfNoOfDilutieRinseTimes.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_016;//The Number of Times of Rinse
            lblconfDilutiePrimeVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_017;//Diluent Prime Volume
            lblconfDispenseVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_018;//Diluent Dispense Volume

            lblconfWaitTimeAfterSuckingUpUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_019;//(sec)
            lblconfWaitTimeAfterDispenseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_019;//(sec)

            lblconfStandbyPositionUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_020;//(uL)
            lblconfDilutiePrimeVolumeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_020;//(uL)
            lblconfDispenseVolumeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_020;//(uL)

            lblconfNoOfDilutiePrimeTimesUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_021;//(times)
            lblconfNoOfDilutieRinseTimesUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_021;//(times)

            //MotorParameter
            gbxParamZAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_022;
            gbxParamZAxisCommon.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_028;
            gbxParamZAxisOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_033;
            lblParamZAxisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_023;
            lblParamZAxisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_024;
            lblParamZAxisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_025;
            lblParamZAxisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_026;
            lblParamZAxisOffsetReactionCell.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_027;
            lblParamZAxisOffsetCuvette.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_040;
            lblParamZAxisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_029;
            lblParamZAxisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_029;
            lblParamZAxisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_030;
            lblParamZAxisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_029;
            lblParamZAxisOffsetReactionCellUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_031;
            lblParamZAxisOffsetCuvetteUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_031;

            gbxParamSyringe.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_032;
            gbxParamSyringeAsp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_034;
            gbxParamSyringeDis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_035;
            gbxParamSyringeAir.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_036;
            lblParamSyringeAspInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_023;
            lblParamSyringeAspTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_024;
            lblParamSyringeAspAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_025;
            lblParamSyringeAspConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_026;
            lblParamSyringeDisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_023;
            lblParamSyringeDisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_024;
            lblParamSyringeDisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_025;
            lblParamSyringeDisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_026;
            lblParamSyringeAirInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_023;
            lblParamSyringeAirTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_024;
            lblParamSyringeAirAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_025;
            lblParamSyringeAirConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_026;
            lblParamSyringeGain.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_037;
            lblParamSyringeOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_038;
            lblParamSyringeAspInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_029;
            lblParamSyringeAspTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_029;
            lblParamSyringeAspAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_030;
            lblParamSyringeAspConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_029;
            lblParamSyringeDisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_029;
            lblParamSyringeDisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_029;
            lblParamSyringeDisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_030;
            lblParamSyringeDisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_029;
            lblParamSyringeAirInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_029;
            lblParamSyringeAirTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_029;
            lblParamSyringeAirAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_030;
            lblParamSyringeAirConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_029;

            //MotorAdjustタブ
            gbxAdjustSeq.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_000;
            SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_041
                                , No=(int)MotorAdjustStopPosition.DiluentDispensePrime}
            };
            lbxAdjustSequence = ComFunc.setSequenceListBox(lbxAdjustSequence, SequenceList);

            gbxAdjustParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_005;

            gbxAdjustZAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_022;
            lblAdjustZAxisOffsetReactionCell.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_DILUTIONDISPENSE_027;

            //グループボックス
            //UP/Down
            btnAdjustZAxisDown.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_DOWN;
            btnAdjustZAxisUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_UP;

            //Pitch
            lblAdjustZAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;
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

                    SlaveCommCommand_0439 StartComm = new SlaveCommCommand_0439();
                    StartComm.UnitNo = (int)UnitNoList.DiluentDispense;
                    StartComm.FuncNo = FuncNo;

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

            //希釈液分注部Z軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.DiluentDispenseArmZAxis;
            if (!adjustSave)
            {
                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamZAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamZAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamZAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamZAxisConst.Value;

                Mparam.MotorOffset[0] = (double)numParamZAxisOffsetReactionCell.Value;
                Mparam.MotorOffset[1] = (double)numParamZAxisOffsetCuvette.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseArmZAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseArmZAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseArmZAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustZAxisOffsetReactionCell.Value;
                Mparam.MotorOffset[1] = (double)numAdjustZAxisOffsetReactionCell.Value;       //Cuvetteの値も反応テーブル位置で更新する。（画面上にCuvetteは不要）
            }

            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseArmZAxisParam.OffsetReactionCell = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseArmZAxisParam.OffsetCuvette = Mparam.MotorOffset[1];

            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);

            if (!adjustSave)
            {
                //希釈分注シリンジ
                Mparam = new SlaveCommCommand_0471();
                Mparam.MotorNo = (int)MotorNoList.DiluentDispenseSyringe;
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
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseSyringeParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseSyringeParam.MotorSpeed = Mparam.MotorSpeed;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseSyringeParam.Gain = Mparam.MotorOffset[0];
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseSyringeParam.Offset = Mparam.MotorOffset[1];

                //モータパラメータ送信内容スプール
                SpoolParam(Mparam);
            }

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

            //希釈液分注部Z軸
            numParamZAxisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseArmZAxisParam.MotorSpeed[0].InitSpeed;
            numParamZAxisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseArmZAxisParam.MotorSpeed[0].TopSpeed;
            numParamZAxisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseArmZAxisParam.MotorSpeed[0].Accel;
            numParamZAxisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseArmZAxisParam.MotorSpeed[0].ConstSpeed;

            //希釈液吐出オフセット
            numParamZAxisOffsetReactionCell.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseArmZAxisParam.OffsetReactionCell;
            //プライム位置オフセット
            numParamZAxisOffsetCuvette.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseArmZAxisParam.OffsetCuvette;

            //モーター調整画面にも反映
            numAdjustZAxisOffsetReactionCell.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseArmZAxisParam.OffsetReactionCell;

            //希釈分注シリンジ
            numParamSyringeAspInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseSyringeParam.MotorSpeed[0].InitSpeed;
            numParamSyringeAspTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseSyringeParam.MotorSpeed[0].TopSpeed;
            numParamSyringeAspAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseSyringeParam.MotorSpeed[0].Accel;
            numParamSyringeAspConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseSyringeParam.MotorSpeed[0].ConstSpeed;

            numParamSyringeDisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseSyringeParam.MotorSpeed[1].InitSpeed;
            numParamSyringeDisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseSyringeParam.MotorSpeed[1].TopSpeed;
            numParamSyringeDisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseSyringeParam.MotorSpeed[1].Accel;
            numParamSyringeDisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseSyringeParam.MotorSpeed[1].ConstSpeed;

            numParamSyringeAirInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseSyringeParam.MotorSpeed[2].InitSpeed;
            numParamSyringeAirTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseSyringeParam.MotorSpeed[2].TopSpeed;
            numParamSyringeAirAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseSyringeParam.MotorSpeed[2].Accel;
            numParamSyringeAirConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseSyringeParam.MotorSpeed[2].ConstSpeed;

            numParamSyringeGain.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseSyringeParam.Gain;
            numParamSyringeOffset.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].diluentDispenseSyringeParam.Offset;
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

            CarisXConfigParameter.DilutionDispenseUnitConfigParam ConfigParam = new CarisXConfigParameter.DilutionDispenseUnitConfigParam();

            //希釈液吸引後遅延時間
            ConfigParam.WaitTimeAfterSuckingUp = (double)numconfWaitTimeAfterSuckingUp.Value;
            //希釈液吐出後遅延時間
            ConfigParam.WaitTimeAfterDispense = (double)numconfWaitTimeAfterDispense.Value;
            //希釈液シリンジ待機位置
            ConfigParam.StandbyPosition = (int)numconfStandbyPosition.Value;
            //プライム回数
            ConfigParam.NoOfDilutiePrimeTimes = (int)numconfNoOfDilutiePrimeTimes.Value;
            //リンス回数
            ConfigParam.NoOfDilutieRinseTimes = (int)numconfNoOfDilutieRinseTimes.Value;
            //プライム量
            ConfigParam.DilutiePrimeVolume = (int)numconfDilutiePrimeVolume.Value;
            //希釈液吐出量
            ConfigParam.DispenseVolume = (int)numconfDispenseVolume.Value;

            config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].dilutionDispenseUnitConfigParam = ConfigParam;

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

            //希釈液吸引後遅延時間
            numconfWaitTimeAfterSuckingUp.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].dilutionDispenseUnitConfigParam.WaitTimeAfterSuckingUp;
            //希釈液吐出後遅延時間
            numconfWaitTimeAfterDispense.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].dilutionDispenseUnitConfigParam.WaitTimeAfterDispense;
            //希釈液シリンジ待機位置
            numconfStandbyPosition.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].dilutionDispenseUnitConfigParam.StandbyPosition;
            //プライム回数
            numconfNoOfDilutiePrimeTimes.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].dilutionDispenseUnitConfigParam.NoOfDilutiePrimeTimes;
            //リンス回数
            numconfNoOfDilutieRinseTimes.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].dilutionDispenseUnitConfigParam.NoOfDilutieRinseTimes;
            //プライム量
            numconfDilutiePrimeVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].dilutionDispenseUnitConfigParam.DilutiePrimeVolume;
            //希釈液吐出量
            numconfDispenseVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].dilutionDispenseUnitConfigParam.DispenseVolume;
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
            gbxAdjustZAxis.Enabled = false;
            numAdjustZAxisOffsetReactionCell.Enabled = false;

            //EditBoxの文字色をすべて黒にする。
            EditBoxControlsBlack(tabUnit.Tabs[3].TabPage);
        }

        /// <summary>
        /// モーター調整機能スタート時処理
        /// </summary>
        private void AdjustUnitStartPrc()
        {
            Func<int> getindex = () => { return lbxAdjustSequence.SelectedIndex; };
            int SelectedIndex = (int)Invoke(getindex);

            SlaveCommCommand_0480 AdjustStartComm = new SlaveCommCommand_0480();

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
            btnAdjustZAxisUp.Enabled = enable;
            btnAdjustZAxisDown.Enabled = enable;
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
            switch (lbxAdjustSequence.SelectedIndex + 1)
            {
                case 1:
                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetReactionCell.Enabled = true;
                    numAdjustZAxisOffsetReactionCell.ForeColor = System.Drawing.Color.OrangeRed;
                    break;
            }
        }

        #region Pitch値の調整
        private void btnAdjustZAxisPitchUp_Click(object sender, EventArgs e)
        {
            numAdjustZAxisPitch.Value = (double)numAdjustZAxisPitch.Value + (double)numAdjustZAxisPitch.SpinIncrement;
        }

        private void btnAdjustZAxisPitchDown_Click(object sender, EventArgs e)
        {
            numAdjustZAxisPitch.Value = (double)numAdjustZAxisPitch.Value - (double)numAdjustZAxisPitch.SpinIncrement;
        }
        #endregion

        private void btnAdjustZAxisDownUp_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustZAxisDown);
            Int32 motorNo = (int)MotorNoList.DiluentDispenseArmZAxis;
            Double pitchValue = (double)numAdjustZAxisPitch.Value;

            AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetReactionCell, pitchValue);
        }
    }
}
