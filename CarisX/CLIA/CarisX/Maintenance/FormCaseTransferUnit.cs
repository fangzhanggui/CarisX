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
    /// ケース搬送
    /// </summary>
    public partial class FormCaseTransferUnit : FormUnitBase
    {
        IMaintenanceUnitStart unitStart = new UnitStart();
        IMaintenanceUnitStart unitStartAdjust = new UnitStarAdjust();
        IMaintenanceUnitStart unitAdjustAbort = new UnitStarAdjustAbort();
        volatile bool ResponseFlg;
        public CommonFunction ComFunc = new CommonFunction();

        public FormCaseTransferUnit()
        {
            InitializeComponent();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ParametersAllFalse();
            MotorParamDisp();
            setCulture();
            setRadioButtonValue();
            ComFunc.SetControlSettings(this);
            ConfigTabUseFlg = tabUnit.Tabs[(int)MaintenanceTabIndex.Config].Enabled;    //Configタブを利用有無を退避

            lbxtestSequenceListBox.SelectedIndex = 0;
            lbxAdjustSequence.SelectedIndex = 0;
        }

        #region リソース設定
        private void setCulture()
        {
            //Tab
            tabUnit.Tabs[0].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_012;                //Test
            tabUnit.Tabs[1].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_013;                //Configuration
            tabUnit.Tabs[2].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_014;                //Motor Parameters
            tabUnit.Tabs[3].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_MOTORADJUSTMENT;    //Adjust

            //Testタブ
            gbxtestSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_000;                        //Sequence
            object SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_001, No=(int)CaseTransferSequence.Init},                     //1: Unit Initialization
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_002, No=(int)CaseTransferSequence.AlltheCasesChecking},      //2: All the Cases Checking
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_003, No=(int)CaseTransferSequence.CaseLoading},              //3: Case Loading
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_004, No=(int)CaseTransferSequence.CaseUnloading},            //4: Case Unloading
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_005, No=(int)CaseTransferSequence.CaseDoorLockOperation},    //5: Case Door Lock Operation
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_006, No=(int)CaseTransferSequence.MoveTiptoCatchPosition},   //6: Move Tip to Catch Position
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_041, No=(int)CaseTransferSequence.MoveCelltoCatchPosition},
            };
            lbxtestSequenceListBox = ComFunc.setSequenceListBox(lbxtestSequenceListBox, SequenceList);

            gbxtestParameters.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_007;                      //Parameters
            lbltestRepeatFrequency.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_008;                 //Repeat Frequency
            lbltestNumber.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_009;                          //Number
            lbltestRepeatInterval.Text = Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_042;
            lbltestRepeatIntervalUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_043;
            lbltestCheckPortNo.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_037;                     //Check Port No.
            lbltestPortNo.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_010;                          //Port No.
            lbltestLockOperation.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_033;                   //Lock Operation
            rbttestLockON.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_034;                          //ON
            rbttestLockOFF.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_035;                         //OFF
            lbltestCatchPosition.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_036;                   //Catch Position

            gbxtestResponce.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_011;                        //Response

            //Configuration


            //MotorParameter
            gbxParamYAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_015;
            lblParamYAxisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_016;
            lblParamYAxisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_017;
            lblParamYAxisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_018;
            lblParamYAxisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_019;
            lblParamYAxisOffsetCaseCatchRelease.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_020;
            lblParamYAxisOffsetReactionCellCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_021;
            lblParamYAxisOffsetTipCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_031;
            lblParamYAxisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_023;
            lblParamYAxisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_023;
            lblParamYAxisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_024;
            lblParamYAxisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_023;
            lblParamYAxisOffsetCaseCatchReleaseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_025;
            lblParamYAxisOffsetReactionCellCatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_025;
            lblParamYAxisOffsetTipCatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_025;

            gbxParamZAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_026;
            lblParamZAxisinit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_016;
            lblParamZAxisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_017;
            lblParamZAxisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_018;
            lblParamZAxisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_019;
            lblParamZAxisOffsetCaseCatchRelease.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_020;
            lblParamZAxisOffsetTipCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_032;
            lblParamZAxisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_023;
            lblParamZAxisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_023;
            lblParamZAxisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_024;
            lblParamZAxisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_023;
            lblParamZAxisOffsetCaseCatchReleaseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_025;
            lblParamZAxisOffsetTipCatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_025;

            gbxParamYAxisCommon.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_022;
            gbxParamZAxisCommon.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_022;

            gbxParamYAxisOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_038;
            gbxParamZAxisOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_038;


            //MotorAdjustタブ
            gbxAdjustSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_000;
            SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_040, No=(int)MotorAdjustStopPosition.CaseCatch},
            };
            lbxAdjustSequence = ComFunc.setSequenceListBox(lbxAdjustSequence, SequenceList);

            gbxAdjustParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_007;
            lblAdjustPortNo.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_039;

            gbxAdjustYAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_015;
            lblAdjustYAxisOffsetCaseCatchRelease.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_020;
            lblAdjustYAxisOffsetReactionCellCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_021;
            lblAdjustYAxisOffsetTipCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_031;
            lblAdjustYAxisOffsetCaseCatchReleaseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_025;
            lblAdjustYAxisOffsetReactionCellCatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_025;
            lblAdjustYAxisOffsetTipCatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_025;
            lblAdjustYAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;
            btnAdjustYAxisForward.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_FORWARD;
            btnAdjustYAxisBack.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_BACK;

            gbxAdjustZAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_026;
            lblAdjustZAxisOffsetCaseCatchRelease.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_020;
            lblAdjustZAxisOffsetTipCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_032;
            lblAdjustZAxisOffsetCaseCatchReleaseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_025;
            lblAdjustZAxisOffsetTipCatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_025;
            lblAdjustZAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;
            btnAdjustZAxisDown.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_DOWN;
            btnAdjustZAxisUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_UP;
        }
        #endregion

        /// <summary>
        /// ラジオボタンの選択値を設定
        /// </summary>
        private void setRadioButtonValue()
        {
            rbttestLockON.Tag = (int)CaseTransferRadioValue.LockON;
            rbttestLockOFF.Tag = (int)CaseTransferRadioValue.LockOFF;
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
                    StartComm.UnitNo = (int)UnitNoList.CaseTransfer;
                    StartComm.FuncNo = FuncNo;

                    switch (FuncNo)
                    {
                        case (int)CaseTransferSequence.AlltheCasesChecking:
                            StartComm.Arg1 = (int)numtestCheckPortNo.Value;
                            break;
                        case (int)CaseTransferSequence.CaseLoading:
                        case (int)CaseTransferSequence.CaseUnloading:
                            StartComm.Arg1 = (int)numtestPortNo.Value;
                            break;
                        case (int)CaseTransferSequence.CaseDoorLockOperation:
                            StartComm.Arg1 = ComFunc.getSelectedRadioButtonValue(gbxtestLockOperation);
                            break;
                        case (int)CaseTransferSequence.MoveTiptoCatchPosition:
                        case (int)CaseTransferSequence.MoveCelltoCatchPosition:
                            StartComm.Arg1 = (int)numtestCatchPosition.Value;
                            break;
                    }

                    //レスポンスがある機能の場合のみ、レスポンスのログファイルを作成
                    if (ComFunc.chkExistsResponse(MaintenanceMainNavi.TipCellCaseTransferUnit, FuncNo))
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

                    //メイン画面のナビゲータバーをEnableコマンドバーをデフォルトに戻す。
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

                    if (ComFunc.chkExistsResponse(MaintenanceMainNavi.TipCellCaseTransferUnit, (int)lbxtestSequenceListBox.SelectedValue))
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
        /// パラメータ入力部すべてをDisableにします。
        /// </summary>
        private void ParametersAllFalse()
        {
            numtestCheckPortNo.Enabled = false;
            numtestPortNo.Enabled = false;
            gbxtestLockOperation.Enabled = false;
            numtestCatchPosition.Enabled = false;
        }

        /// <summary>
        /// パラメータ入力部すべてDisable
        /// </summary>
        private void lbxtestSequenceListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParametersAllFalse();
            switch (lbxtestSequenceListBox.SelectedValue)
            {
                case (int)CaseTransferSequence.AlltheCasesChecking:
                    numtestCheckPortNo.Enabled = true;
                    break;
                case (int)CaseTransferSequence.CaseLoading:
                case (int)CaseTransferSequence.CaseUnloading:
                    numtestPortNo.Enabled = true;
                    break;
                case (int)CaseTransferSequence.CaseDoorLockOperation:
                    gbxtestLockOperation.Enabled = true;
                    break;
                case (int)CaseTransferSequence.MoveTiptoCatchPosition:
                case (int)CaseTransferSequence.MoveCelltoCatchPosition:
                    numtestCatchPosition.Enabled = true;
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
        /// モーターパラメータ保存
        /// </summary>
        public void MotorParamSave(bool adjustSave)
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

            //ケース搬送部Y軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.CaseTransferYAxis;
            if (!adjustSave)
            {
                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamYAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamYAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamYAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamYAxisConst.Value;

                //オフセット
                Mparam.MotorOffset[0] = (double)numParamYAxisOffsetCaseCatchRelease.Value;
                Mparam.MotorOffset[1] = (double)numParamYAxisOffsetReactionCellCatch.Value;
                Mparam.MotorOffset[2] = (double)numParamYAxisOffsetTipCatch.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustYAxisOffsetCaseCatchRelease.Value;
                Mparam.MotorOffset[1] = (double)numAdjustYAxisOffsetReactionCellCatch.Value;
                Mparam.MotorOffset[2] = (double)numAdjustYAxisOffsetTipCatch.Value;
            }
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.OffsetCaseCatchRelease = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.OffsetReactionCellCatch = Mparam.MotorOffset[1];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.OffsetSamplingTipCatch = Mparam.MotorOffset[2];
            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);


            //ケース搬送部Z軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.CaseTransferZAxis;
            if (!adjustSave)
            {
                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamZAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamZAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamZAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamZAxisConst.Value;

                //オフセット
                Mparam.MotorOffset[0] = (double)numParamZAxisOffsetCaseCatchRelease.Value;
                Mparam.MotorOffset[1] = (double)numParamZAxisOffsetTipCatch.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferZAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferZAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferZAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustZAxisOffsetCaseCatchRelease.Value;
                Mparam.MotorOffset[1] = (double)numAdjustZAxisOffsetTipCatch.Value;
            }
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferZAxisParam.OffsetCaseCatchRelease = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferZAxisParam.OffsetReactionCellSamplingTipCatch = Mparam.MotorOffset[1];
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
                    case (int)MaintenanceTabIndex.MParam:
                    case (int)MaintenanceTabIndex.MAdjust:
                        //モータパラメータ保存
                        ParameterFilePreserve<CarisXMotorParameter> motor = Singleton<ParameterFilePreserve<CarisXMotorParameter>>.Instance;
                        motorSave(motor);
                        MotorParamDisp();
                        try
                        {
                            //関連する他の画面のモーターパラメータの再表示を行う
                            FormMaintenanceMain formMaintenanceMain = (FormMaintenanceMain)Application.OpenForms[typeof(FormMaintenanceMain).Name];
                            formMaintenanceMain.subFormSampleDispense[formMaintenanceMain.moduleIndex].MotorParamDisp();
                            formMaintenanceMain.subFormReactionCellTransfer[formMaintenanceMain.moduleIndex].MotorParamDisp();
                        }
                        catch (Exception)
                        {
                            System.Diagnostics.Debug.WriteLine("FormMaintenanceMain instance does not exist");
                            Singleton<Log.CarisXLogManager>.Instance.Write(Oelco.Common.Log.LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                                Log.CarisXLogInfoBaseExtention.Empty, "FormMaintenanceMain instance does not exist");
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

            //ケース搬送部Y軸
            numParamYAxisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.MotorSpeed[0].InitSpeed;
            numParamYAxisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.MotorSpeed[0].TopSpeed;
            numParamYAxisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.MotorSpeed[0].Accel;
            numParamYAxisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.MotorSpeed[0].ConstSpeed;
            //オフセット
            numParamYAxisOffsetCaseCatchRelease.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.OffsetCaseCatchRelease;
            numParamYAxisOffsetReactionCellCatch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.OffsetReactionCellCatch;
            numParamYAxisOffsetTipCatch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.OffsetSamplingTipCatch;
            //オフセット（モーター調整用）
            numAdjustYAxisOffsetCaseCatchRelease.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.OffsetCaseCatchRelease;
            numAdjustYAxisOffsetReactionCellCatch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.OffsetReactionCellCatch;
            numAdjustYAxisOffsetTipCatch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.OffsetSamplingTipCatch;

            //ケース搬送部Z軸
            numParamZAxisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferZAxisParam.MotorSpeed[0].InitSpeed;
            numParamZAxisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferZAxisParam.MotorSpeed[0].TopSpeed;
            numParamZAxisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferZAxisParam.MotorSpeed[0].Accel;
            numParamZAxisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferZAxisParam.MotorSpeed[0].ConstSpeed;
            //オフセット
            numParamZAxisOffsetCaseCatchRelease.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferZAxisParam.OffsetCaseCatchRelease;
            numParamZAxisOffsetTipCatch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferZAxisParam.OffsetReactionCellSamplingTipCatch;
            //オフセット（モーター調整用）
            numAdjustZAxisOffsetCaseCatchRelease.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferZAxisParam.OffsetCaseCatchRelease;
            numAdjustZAxisOffsetTipCatch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferZAxisParam.OffsetReactionCellSamplingTipCatch;
        }

        /// <summary>
        /// パラメータ保存
        /// </summary>
        public override void ParamSave()
        {
            switch (tabUnit.SelectedTab.Index)
            {
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
        /// Adjustグループ全非活性
        /// </summary>
        private void adjustGbAllDisabled()
        {
            gbxAdjustYAxis.Enabled = false;
            gbxAdjustZAxis.Enabled = false;
            numAdjustPortNo.Enabled = false;

            //Y
            numAdjustYAxisOffsetCaseCatchRelease.Enabled = false;
            numAdjustYAxisOffsetReactionCellCatch.Enabled = false;
            numAdjustYAxisOffsetTipCatch.Enabled = false;

            //Z
            numAdjustZAxisOffsetCaseCatchRelease.Enabled = false;
            numAdjustZAxisOffsetTipCatch.Enabled = false;

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
            Func<int> portno = () => { return (int)numAdjustPortNo.Value; };

            Func<int> getindex = () => { return (int)lbxAdjustSequence.SelectedValue; };
            int SelectedIndex = (int)Invoke(getindex);


            AdjustStartComm.Pos = (int)Invoke(getStopPos);

            switch (AdjustStartComm.Pos)
            {
                case (int)MotorAdjustStopPosition.CaseCatch:
                    AdjustStartComm.Arg1 = (int)Invoke(portno);
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
                    if (DialogResult.OK != Oelco.CarisX.GUI.DlgMessage.Show(Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_SAVEMASSAGE
                        , String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, Oelco.CarisX.GUI.MessageDialogButtons.OKCancel))
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

            btnAdjustZAxisDown.Enabled = enable;
            btnAdjustZAxisUp.Enabled = enable;
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
            adjustGbAllDisabled();
            //モータ調整ボタンDisable
            UpDownButtonEnable(false);
            switch (lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.CaseCatch:
                    gbxAdjustYAxis.Enabled = true;
                    gbxAdjustZAxis.Enabled = true;
                    numAdjustPortNo.Enabled = true;

                    //Y
                    numAdjustYAxisOffsetCaseCatchRelease.Enabled = true;
                    numAdjustYAxisOffsetCaseCatchRelease.Appearance.ForeColor = System.Drawing.Color.OrangeRed;

                    //Z
                    numAdjustZAxisOffsetCaseCatchRelease.Enabled = true;
                    numAdjustZAxisOffsetCaseCatchRelease.Appearance.ForeColor = System.Drawing.Color.OrangeRed;

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

        private void btnAdjustZAxisPitchUp_Click(object sender, EventArgs e)
        {
            numAdjustZAxisPitch.Value = (double)numAdjustZAxisPitch.Value + (double)numAdjustZAxisPitch.SpinIncrement;
        }

        private void btnAdjustZAxisPitchDown_Click(object sender, EventArgs e)
        {
            numAdjustZAxisPitch.Value = (double)numAdjustZAxisPitch.Value - (double)numAdjustZAxisPitch.SpinIncrement;
        }
        #endregion

        private void btnAdjustYAxisForwardBack_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustYAxisForward);
            Int32 motorNo = (int)MotorNoList.CaseTransferYAxis;
            Double pitchValue = (double)numAdjustYAxisPitch.Value;

            AdjustValue(upFlg, motorNo, numAdjustYAxisOffsetCaseCatchRelease, pitchValue);
        }

        private void btnAdjustZAxisDownUp_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustZAxisDown);
            Int32 motorNo = (int)MotorNoList.CaseTransferZAxis;
            Double pitchValue = (double)numAdjustZAxisPitch.Value;

            AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetCaseCatchRelease, pitchValue);
        }
    }
}
