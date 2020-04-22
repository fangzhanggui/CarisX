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
    /// トリガ分注
    /// </summary>
    public partial class FormTriggerDispenseUnit : FormUnitBase
    {
        IMaintenanceUnitStart unitStart = new UnitStart();
        IMaintenanceUnitStart unitStartAdjust = new UnitStarAdjust();
        IMaintenanceUnitStart unitAdjust = new UnitAdjust();
        IMaintenanceUnitStart unitAdjustAbort = new UnitStarAdjustAbort();
        volatile bool ResponseFlg;
        public CommonFunction ComFunc = new CommonFunction();

        //モーター調整コマンドチクチク
        SlaveCommCommand_0473 AdjustUpDownComm = new SlaveCommCommand_0473();
        double TempInstrumentCoef;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormTriggerDispenseUnit()
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
            lbxconfPhotometryMode.SelectedIndex = 0;
        }

        #region リソース設定
        
        /// <summary>
        /// リソース設定
        /// </summary>
        private void setCulture()
        {
            //Tab
            tabUnit.Tabs[0].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_065;              //Test
            tabUnit.Tabs[1].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_066;              //Configuration
            tabUnit.Tabs[2].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_067;              //Motor Parameters
            tabUnit.Tabs[3].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_MOTORADJUSTMENT; //tabu

            //Testタブ
            gbxtestSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_000;      //Sequence
            object SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_001, No=(int)TriggerDispenseSequence.Init},                   //1: Unit Initialization
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_002, No=(int)TriggerDispenseSequence.TriggerDispense},        //2: Trigger Dispense
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_003, No=(int)TriggerDispenseSequence.Prime},                  //3: Prime
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_004, No=(int)TriggerDispenseSequence.Rinse},                  //4: Rinse
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_005, No=(int)TriggerDispenseSequence.PrimeExchangeBottle},    //5: Prime to Exchange Bottle
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_006, No=(int)TriggerDispenseSequence.Measurement},            //6: Measurement
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_007, No=(int)TriggerDispenseSequence.DetectorOutput},         //7: Detector Output
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_063, No=(int)TriggerDispenseSequence.MoveTriggerDispense},    //8: Move to Adjusted Position for Trigger Dispensing
            };
            lbxtestSequenceListBox = ComFunc.setSequenceListBox(lbxtestSequenceListBox, SequenceList);

            gbxtestParameters.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_008;                       //Parameters
            lbltestRepeatFrequency.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_009;                  //Repeat Frequency
            lbltestNumber.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_010;                           //Number
            lbltestRepeatInterval.Text = Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_072;
            lbltestRepeatIntervalUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_073;
            lbltestBottle.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_068;                           //Bottle
            rbttestBottle1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_069;                          //1
            rbttestBottle2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_070;                          //2
            lbltestMeas.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_012;                             //Measurement
            rbttestMeasDark.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_013;                         //Dark
            rbttestMeasSample.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_014;                       //Sample
            rbttestMeasBackground.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_064;                   //Background
            rbttestMeasLED.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_015;                          //LED
            lbltestDetect.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_016;                           //Detector Output
            rbttestDetectON.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_017;                         //ON
            rbttestDetectOFF.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_018;                        //OFF

            gbxtestInstrumentParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_019;                  //Instrument Parameter Set
            lbltestInstCoef.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_020;                         //InstrumentCoef
            btntestInstCoef.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_021;                         //Save

            gbxtestResponce.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_022;                         //Response

            //Configurationタブ
            gbxconfParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_008;//Parameters

            lblconfWaitTimeAfterSuckingUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_023;//The Delay Time after Trigger Aspiration
            lblconfWaitTimeAfterDispense.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_024;//The Delay Time after Trigger Dispense
            lblconfNoOfPrimeTimes.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_025;//The Number of Times of Prime
            lblconfNoOfRinseTimes.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_026;//The Number of Times of Rinse
            lblconfPrimeVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_027;//Trigger Prime Volume
            lblconfStandbyPosition.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_028;//Trigger Dispense Syringe Standby Position
            lblconfDispanseVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_029;//Trigger Dispense Volume
            lblconfPhotometryMode.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_030;//Measurement Mode
            lbxconfPhotometryMode.Items[0] = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_031;//Area Method
            lbxconfPhotometryMode.Items[1] = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_032;//Peak Method
            lblconfExposureTime.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_033;//Gate time

            lblconfWaitTimeAfterSuckingUpUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_034;//(sec)
            lblconfWaitTimeAfterDispenseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_034;//(sec)

            lblconfNoOfPrimeTimesUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_035;//(times)
            lblconfNoOfRinseTimesUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_035;//(times)

            lblconfPrimeVolumeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_036;//(uL)
            lblconfStandbyPositionUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_036;//(uL)
            lblconfDispanseVolumeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_036;//(uL)

            lblconfExposureTimeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_037;//(msec)

            //MotorParameter
            gbxParamZAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_038;
            gbxParamZAxisCommon.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_044;
            gbxParamZAxisOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_049;
            gbxParamZAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_038;
            lblParamZAxisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_039;
            lblParamZAxisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_040;
            lblParamZAxisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_041;
            lblParamZAxisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_042;
            lblParamZAxisOffsetReactionCell.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_043;
            lblParamZAxisOffsetCuvette.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_054;
            lblParamZAxisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_045;
            lblParamZAxisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_045;
            lblParamZAxisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_046;
            lblParamZAxisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_045;
            lblParamZAxisOffsetReactionCellUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_047;
            lblParamZAxisOffsetCuvetteUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_047;

            gbxParamSyringe.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_048;
            gbxParamSyringeAsp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_050;
            gbxParamSyringeDis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_051;
            gbxParamSyringeAir.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_052;
            gbxParamSyringePrime.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_053;
            lblParamSyringeAspInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_039;
            lblParamSyringeAspTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_040;
            lblParamSyringeAspAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_041;
            lblParamSyringeAspConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_042;
            lblParamSyringeDisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_039;
            lblParamSyringeDisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_040;
            lblParamSyringeDisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_041;
            lblParamSyringeDisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_042;
            lblParamSyringeAirInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_039;
            lblParamSyringeAirTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_040;
            lblParamSyringeAirAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_041;
            lblParamSyringeAirConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_042;
            lblParamSyringePrimeRinseInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_039;
            lblParamSyringePrimeRinseTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_040;
            lblParamSyringePrimeRinseAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_041;
            lblParamSyringePrimeRinseConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_042;
            lblParamSyringeGain.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_061;
            lblParamSyringeOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_062;
            lblParamSyringeAspInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_045;
            lblParamSyringeAspTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_045;
            lblParamSyringeAspAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_046;
            lblParamSyringeAspConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_045;
            lblParamSyringeDisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_045;
            lblParamSyringeDisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_045;
            lblParamSyringeDisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_046;
            lblParamSyringeDisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_045;
            lblParamSyringeAirInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_045;
            lblParamSyringeAirTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_045;
            lblParamSyringeAirAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_046;
            lblParamSyringeAirConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_045;
            lblParamSyringePrimeRinseInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_045;
            lblParamSyringePrimeRinseTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_045;
            lblParamSyringePrimeRinseAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_046;
            lblParamSyringePrimeRinseConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_045;

            //MotorAdjustタブ
            gbxAdjustSeq.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_000;
            SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_071
                                , No=(int)MotorAdjustStopPosition.TriggerPreTriggerDispensePrime}
            };
            lbxAdjustSequence = ComFunc.setSequenceListBox(lbxAdjustSequence, SequenceList);

            gbxAdjustParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_008;

            gbxAdjustZAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_038;
            lblAdjustZAxisOffsetReactionCell.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_043;

            //グループボックス
            //UP/Down
            btnAdjustZAxisDown.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_DOWN;
            btnAdjustZAxisUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_UP; 

            //Pitch
            lblAdjustZAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;

        }
        #endregion

        /// <summary>
        /// ラジオボタンの選択値を設定
        /// </summary>
        private void setRadioButtonValue()
        {
            rbttestBottle1.Tag = (int)TriggerDispenseRadioValue.Bottle1;
            rbttestBottle2.Tag = (int)TriggerDispenseRadioValue.Bottle2;
            rbttestMeasDark.Tag = (int)TriggerDispenseRadioValue.MeasDark;
            rbttestMeasSample.Tag = (int)TriggerDispenseRadioValue.MeasSample;
            rbttestMeasBackground.Tag = (int)TriggerDispenseRadioValue.MeasBackground;
            rbttestMeasLED.Tag = (int)TriggerDispenseRadioValue.MeasLED;
            rbttestDetectON.Tag = (int)TriggerDispenseRadioValue.DetectON;
            rbttestDetectOFF.Tag = (int)TriggerDispenseRadioValue.DetectOFF;
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
                    StartComm.UnitNo = (int)UnitNoList.TriggerDispense;
                    StartComm.FuncNo = FuncNo;

                    switch (FuncNo)
                    {
                        case (int)TriggerDispenseSequence.TriggerDispense:
                        case (int)TriggerDispenseSequence.Prime:
                        case (int)TriggerDispenseSequence.PrimeExchangeBottle:
                            StartComm.Arg1 = ComFunc.getSelectedRadioButtonValue(gbxtestBottle);
                            break;
                        case (int)TriggerDispenseSequence.Measurement:
                            StartComm.Arg1 = ComFunc.getSelectedRadioButtonValue(gbxtestMeas);
                            break;
                        case (int)TriggerDispenseSequence.DetectorOutput:
                            StartComm.Arg1 = ComFunc.getSelectedRadioButtonValue(gbxtestDetect);
                            break;
                    }

                    // レスポンスがある機能の場合のみ、レスポンスのログファイルを作成
                    if (ComFunc.chkExistsResponse(MaintenanceMainNavi.TriggerDispensingUnitandChemiluminescenceMeasUnit, FuncNo))
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

                    if (ComFunc.chkExistsResponse(MaintenanceMainNavi.TriggerDispensingUnitandChemiluminescenceMeasUnit, (int)lbxtestSequenceListBox.SelectedValue))
                    {
                        switch ((int)lbxtestSequenceListBox.SelectedValue)
                        {
                            case (int)TriggerDispenseSequence.Measurement:
                                if (!rbttestMeasDark.Checked)
                                {
                                    Result = SubFunction.ToRoundOffParse(int.Parse(Result) * TempInstrumentCoef, 0);
                                }
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
            gbxtestBottle.Enabled = false;
            gbxtestMeas.Enabled = false;
            gbxtestDetect.Enabled = false;
        }

        /// <summary>
        /// 選択された機能番号に応じてパラメータを活性化する
        /// </summary>
        private void lbxtestSequenceListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParametersAllFalse();
            switch (lbxtestSequenceListBox.SelectedValue)
            {
                case (int)TriggerDispenseSequence.TriggerDispense:
                case (int)TriggerDispenseSequence.Prime:
                case (int)TriggerDispenseSequence.PrimeExchangeBottle:
                    gbxtestBottle.Enabled = true;
                    break;

                case (int)TriggerDispenseSequence.Measurement:
                    gbxtestMeas.Enabled = true;
                    break;

                case (int)TriggerDispenseSequence.DetectorOutput:
                    gbxtestDetect.Enabled = true;
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

            //プレトリガ・トリガノズルZ軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.TriggerAndPreTriggerDispenseNozzleZAxis;
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
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerAndPreTriggerDispenseNozzleZAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerAndPreTriggerDispenseNozzleZAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                //送信用アイテムにセット
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerAndPreTriggerDispenseNozzleZAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustZAxisOffsetReactionCell.Value;
                Mparam.MotorOffset[1] = (double)numAdjustZAxisOffsetReactionCell.Value;       //Cuvetteの値も反応テーブル位置で更新する。（画面上にCuvetteは不要）
            }
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerAndPreTriggerDispenseNozzleZAxisParam.OffsetReactionCell = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerAndPreTriggerDispenseNozzleZAxisParam.OffsetCuvette = Mparam.MotorOffset[1];

            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);


            if (!adjustSave)
            {
                //トリガ液ｼﾘﾝｼﾞ
                Mparam = new SlaveCommCommand_0471();
                Mparam.MotorNo = (int)MotorNoList.TriggerDispenseSyringe;
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
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.MotorSpeed = Mparam.MotorSpeed;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.Gain = Mparam.MotorOffset[0];
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.Offset = Mparam.MotorOffset[1];

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

            //プレトリガ・トリガノズルZ軸
            numParamZAxisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerAndPreTriggerDispenseNozzleZAxisParam.MotorSpeed[0].InitSpeed;
            numParamZAxisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerAndPreTriggerDispenseNozzleZAxisParam.MotorSpeed[0].TopSpeed;
            numParamZAxisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerAndPreTriggerDispenseNozzleZAxisParam.MotorSpeed[0].Accel;
            numParamZAxisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerAndPreTriggerDispenseNozzleZAxisParam.MotorSpeed[0].ConstSpeed;
            //反応テーブル位置オフセット
            numParamZAxisOffsetReactionCell.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerAndPreTriggerDispenseNozzleZAxisParam.OffsetReactionCell;
            //洗浄層位置オフセット
            numParamZAxisOffsetCuvette.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerAndPreTriggerDispenseNozzleZAxisParam.OffsetCuvette;

            //モーター調整画面にも反映
            numAdjustZAxisOffsetReactionCell.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerAndPreTriggerDispenseNozzleZAxisParam.OffsetReactionCell;

            //トリガ液ｼﾘﾝｼﾞ
            numParamSyringeAspInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.MotorSpeed[0].InitSpeed;
            numParamSyringeAspTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.MotorSpeed[0].TopSpeed;
            numParamSyringeAspAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.MotorSpeed[0].Accel;
            numParamSyringeAspConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.MotorSpeed[0].ConstSpeed;

            numParamSyringeDisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.MotorSpeed[1].InitSpeed;
            numParamSyringeDisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.MotorSpeed[1].TopSpeed;
            numParamSyringeDisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.MotorSpeed[1].Accel;
            numParamSyringeDisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.MotorSpeed[1].ConstSpeed;

            numParamSyringeAirInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.MotorSpeed[2].InitSpeed;
            numParamSyringeAirTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.MotorSpeed[2].TopSpeed;
            numParamSyringeAirAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.MotorSpeed[2].Accel;
            numParamSyringeAirConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.MotorSpeed[2].ConstSpeed;

            numParamSyringePrimeRinseInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.MotorSpeed[3].InitSpeed;
            numParamSyringePrimeRinseTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.MotorSpeed[3].TopSpeed;
            numParamSyringePrimeRinseAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.MotorSpeed[3].Accel;
            numParamSyringePrimeRinseConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.MotorSpeed[3].ConstSpeed;

            numParamSyringeGain.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.Gain;
            numParamSyringeOffset.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerDispenseSyringeParam.Offset;

            //装置補正係数
            numtestInstCoef.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].instrumentCoef.InstrumentCoefficient;
            TempInstrumentCoef = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].instrumentCoef.InstrumentCoefficient;
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
            CarisXConfigParameter.TriggerUnitConfigParam ConfigParam = new CarisXConfigParameter.TriggerUnitConfigParam();

            //分注シリンジ吸引後待ち時間
            ConfigParam.WaitTimeAfterSuckingUp = (double)numconfWaitTimeAfterSuckingUp.Value;
            //分注シリンジ吐出後待ち時間
            ConfigParam.WaitTimeAfterDispense = (double)numconfWaitTimeAfterDispense.Value;
            //プライム回数
            ConfigParam.NoOfPrimeTimes = (int)numconfNoOfPrimeTimes.Value;
            //リンス回数
            ConfigParam.NoOfRinseTimes = (int)numconfNoOfRinseTimes.Value;
            //プライム量
            ConfigParam.PrimeVolume = (int)numconfPrimeVolume.Value;
            //シリンジ待機位置
            ConfigParam.StandbyPosition = (int)numconfStandbyPosition.Value;
            //プレトリガ分注量
            ConfigParam.DispanseVolume = (int)numconfDispanseVolume.Value;
            //測光モード
            ConfigParam.PhotometryMode = (SlaveCommCommand_0462.PhotometryModeKind)lbxconfPhotometryMode.SelectedIndex + 1;
            //露光時間
            ConfigParam.ExposureTime = (int)numconfExposureTime.Value;

            config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerUnitConfigParam = ConfigParam;

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

            //分注シリンジ吸引後待ち時間
            numconfWaitTimeAfterSuckingUp.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerUnitConfigParam.WaitTimeAfterSuckingUp;
            //分注シリンジ吐出後待ち時間
            numconfWaitTimeAfterDispense.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerUnitConfigParam.WaitTimeAfterDispense;
            //プライム回数
            numconfNoOfPrimeTimes.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerUnitConfigParam.NoOfPrimeTimes;
            //リンス回数
            numconfNoOfRinseTimes.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerUnitConfigParam.NoOfRinseTimes;
            //プライム量
            numconfPrimeVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerUnitConfigParam.PrimeVolume;
            //シリンジ待機位置
            numconfStandbyPosition.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerUnitConfigParam.StandbyPosition;
            //プレトリガ分注量
            numconfDispanseVolume.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerUnitConfigParam.DispanseVolume;
            //測光モード
            lbxconfPhotometryMode.SelectedIndex = (int)config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerUnitConfigParam.PhotometryMode - 1;
            //露光時間
            numconfExposureTime.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].triggerUnitConfigParam.ExposureTime;
        }

        //装置定数入力
        private void BtInstrumentCoef_Click(object sender, EventArgs e)
        {
            //入力値チェック
            if (!IsNumeric(numtestInstCoef.Value.ToString()))
            {
                DlgMaintenance msg = new DlgMaintenance(Properties.Resources_Maintenance.DlgChkMsg, false);
                msg.ShowDialog();
                return;
            }

            DlgMaintenance msgSave = new DlgMaintenance(Properties.Resources_Maintenance.DlgSaveMsg, true);
            if (DialogResult.OK != msgSave.ShowDialog())
                return;

            ParameterFilePreserve<CarisXMotorParameter> motor = Singleton<ParameterFilePreserve<CarisXMotorParameter>>.Instance;
            motorLoad(motor);
            CarisXMotorParameter.InstrumentCoef instrumentCoef = new CarisXMotorParameter.InstrumentCoef();
            instrumentCoef.InstrumentCoefficient = (double)numtestInstCoef.Value;
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].instrumentCoef = instrumentCoef;
            TempInstrumentCoef = instrumentCoef.InstrumentCoefficient;
            //モータパラメータ保存
            motorSave(motor);

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
                // 中断要求により受信待ち解除
                if (AbortFlg)
                {
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
                case (int) MaintenanceTabIndex.Test:
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
        private void lbxAdjustSequence_SelectedIndexChanged(object sender, EventArgs e)
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
            Int32 motorNo = (int)MotorNoList.TriggerAndPreTriggerDispenseNozzleZAxis;
            Double pitchValue = (double)numAdjustZAxisPitch.Value;

            AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetReactionCell, pitchValue);
        }
    }
}
