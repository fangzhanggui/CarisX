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
    /// 反応容器搬送部
    /// </summary>
    public partial class FormReactionCellTransferUnit : FormUnitBase
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

        public FormReactionCellTransferUnit()
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
            lbxAdjustSequence.SelectedIndex = 0;
            cmbAdjustCoarseFine.SelectedIndex = 0;

        }

        #region リソース設定
        private void setCulture()
        {
            //Tab
            tabUnit.Tabs[0].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_013;        //Test
            tabUnit.Tabs[1].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_014;        //Configuration
            tabUnit.Tabs[2].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_015;        //Motor Parameters
            tabUnit.Tabs[3].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_MOTORADJUSTMENT;    //Adjust

            //testタブ
            gbxtestSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_000;                        //Sequence
            object SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_001, No=(int)ReactionCellTransferSequence.Init},                             //1: Unit Initialization
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_002, No=(int)ReactionCellTransferSequence.CatchesfromStorage},               //2：Catches the Reaction Cell from Storage
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_003, No=(int)ReactionCellTransferSequence.ReleasestoSettingPosition},        //3: Releases the Reaction Cell to the Setting Position
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_004, No=(int)ReactionCellTransferSequence.CatchesfromStorageandRelease},     //4: Catches the Reaction Cell from the Storage and Release It.
            };
            lbxtestSequenceListBox = ComFunc.setSequenceListBox(lbxtestSequenceListBox, SequenceList);

            gbxtestParameters.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_008;                      //Parameters
            lbltestRepeatFrequency.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_009;                 //Repeat Frequency
            lbltestNumber.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_010;                          //Number
            lbltestRepeatInterval.Text = Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_056;
            lbltestRepeatIntervalUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_057;
            lbltestReactionCellNo.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_011;                  //Reaction Cell Number

            gbxtestResponce.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_012;                        //Response

            //Configurationタブ

            //MotorParameterタブ
            gbxParamXAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_018;
            gbxParamXAxisCommon.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_048;
            gbxParamXAxisOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_049;
            lblParamXAxisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_019;
            lblParamXAxisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_020;
            lblParamXAxisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_021;
            lblParamXAxisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_022;
            lblParamXAxisOffsetCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_023;
            lblParamXAxisOffsetRelease.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_024;
            lblParamXAxisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_025;
            lblParamXAxisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_025;
            lblParamXAxisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_026;
            lblParamXAxisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_025;
            lblParamXAxisOffsetCatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_027;
            lblParamXAxisOffsetReleaseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_027;

            gbxParamZAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_028;
            gbxParamZAxisCommon.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_048;
            gbxParamZAxisOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_049;
            lblParamZAxisInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_019;
            lblParamZAxisTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_020;
            lblParamZAxisAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_021;
            lblParamZAxisConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_022;
            lblParamZAxisOffsetCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_023;
            lblParamZAxisOffsetRelease.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_024;
            lblParamZAxisInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_025;
            lblParamZAxisTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_025;
            lblParamZAxisAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_026;
            lblParamZAxisConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_025;
            lblParamZAxisOffsetCatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_027;
            lblParamZAxisOffsetReleaseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_027;


            //MotorAdjustタブ
            gbxAdjustSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_000;
            SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_054, No=(int)MotorAdjustStopPosition.ReactionCellCatch},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_055, No=(int)MotorAdjustStopPosition.ReactionCellRelease},
            };
            lbxAdjustSequence = ComFunc.setSequenceListBox(lbxAdjustSequence, SequenceList);

            gbxAdjustParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_008;
            lblAdjustReactionCellNo.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_011;
            lblAdjustCoarseFine.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_COARSEFINE;

            cmbValueCoarseFine = new Infragistics.Win.ValueListItem[]
            {
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_COARSE, dataValue:MotorAdjustCoarseFine.Coarse),
                new Infragistics.Win.ValueListItem(displayText:Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_FINE, dataValue:MotorAdjustCoarseFine.Fine),
            };
            cmbAdjustCoarseFine.Items.AddRange(cmbValueCoarseFine);

            gbxAdjustXAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_018;
            lblAdjustXAxisOffsetCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_023;
            lblAdjustXAxisOffsetRelease.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_024;
            lblAdjustXAxisOffsetCatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_027;
            lblAdjustXAxisOffsetReleaseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_027;

            gbxAdjustZAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_028;
            lblAdjustZAxisOffsetCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_023;
            lblAdjustZAxisOffsetRelease.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_024;
            lblAdjustZAxisOffsetCatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_027;
            lblAdjustZAxisOffsetReleaseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONCELLTRANSFER_027;

            gbxAdjustCaseYAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_015;
            lblAdjustCaseYAxisOffsetCaseCatchRelease.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_020;
            lblAdjustCaseYAxisOffsetReactionCellCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_021;
            lblAdjustCaseYAxisOffsetTipCatch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_031;
            lblAdjustCaseYAxisOffsetCaseCatchReleaseUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_025;
            lblAdjustCaseYAxisOffsetReactionCellCatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_025;
            lblAdjustCaseYAxisOffsetTipCatchUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_CASETRANSFER_025;

            lblAdjustXAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;
            lblAdjustZAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;
            lblAdjustCaseYAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;

            btnAdjustXAxisLeft.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_LEFT;
            btnAdjustXAxisRight.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_RIGHT;

            btnAdjustZAxisDown.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_DOWN;
            btnAdjustZAxisUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_UP;

            btnAdjustCaseYAxisForward.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_FORWARD;
            btnAdjustCaseYAxisBack.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_BACK;
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
                    StartComm.UnitNo = (int)UnitNoList.ReactionCellTransfer;
                    StartComm.FuncNo = FuncNo;

                    switch (FuncNo)
                    {
                        case (int)ReactionCellTransferSequence.CatchesfromStorage:
                        case (int)ReactionCellTransferSequence.CatchesfromStorageandRelease:
                            StartComm.Arg1 = (int)numtestReactionCellNo.Value;
                            break;
                    }

                    //レスポンスがある機能の場合のみ、レスポンスのログファイルを作成
                    if (ComFunc.chkExistsResponse(MaintenanceMainNavi.ReactionCellTransferUnit, FuncNo))
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
                        //レスポンス待ち
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

                    if (ComFunc.chkExistsResponse(MaintenanceMainNavi.ReactionCellTransferUnit, (int)lbxtestSequenceListBox.SelectedValue))
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
            numtestReactionCellNo.Enabled = false;
        }

        /// <summary>
        /// パラメータ入力部すべてDisable
        /// </summary>
        private void lbxtestSequenceListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParametersAllFalse();
            switch (lbxtestSequenceListBox.SelectedValue)
            {
                case (int)ReactionCellTransferSequence.CatchesfromStorage:
                case (int)ReactionCellTransferSequence.CatchesfromStorageandRelease:
                    numtestReactionCellNo.Enabled = true;
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

            //反応容器搬送部X軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.ReactionCellTransferXAxis;
            if (!adjustSave)
            {
                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamXAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamXAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamXAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamXAxisConst.Value;

                Mparam.MotorOffset[0] = (double)numParamXAxisOffsetCatch.Value;
                Mparam.MotorOffset[1] = (double)numParamXAxisOffsetRelease.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferXAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferXAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferXAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustXAxisOffsetCatch.Value;
                Mparam.MotorOffset[1] = (double)numAdjustXAxisOffsetRelease.Value;
            }
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferXAxisParam.OffsetReactionCellCatch = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferXAxisParam.OffsetReactionCellRelease = Mparam.MotorOffset[1];

            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);


            //反応容器搬送部Z軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.ReactionCellTransferZAxis;
            if (!adjustSave)
            {
                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamZAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamZAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamZAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamZAxisConst.Value;

                Mparam.MotorOffset[0] = (double)numParamZAxisOffsetCatch.Value;
                Mparam.MotorOffset[1] = (double)numParamZAxisOffsetRelease.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferZAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferZAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferZAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustZAxisOffsetCatch.Value;
                Mparam.MotorOffset[1] = (double)numAdjustZAxisOffsetRelease.Value;
            }
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferZAxisParam.OffsetReactionCellCatch = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferZAxisParam.OffsetReactionCellRelease = Mparam.MotorOffset[1];
            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);

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

            //反応容器搬送部X軸
            numParamXAxisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferXAxisParam.MotorSpeed[0].InitSpeed;
            numParamXAxisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferXAxisParam.MotorSpeed[0].TopSpeed;
            numParamXAxisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferXAxisParam.MotorSpeed[0].Accel;
            numParamXAxisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferXAxisParam.MotorSpeed[0].ConstSpeed;
            //オフセット
            numParamXAxisOffsetCatch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferXAxisParam.OffsetReactionCellCatch;
            numParamXAxisOffsetRelease.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferXAxisParam.OffsetReactionCellRelease;
            //オフセット（モーター調整用）
            numAdjustXAxisOffsetCatch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferXAxisParam.OffsetReactionCellCatch;
            numAdjustXAxisOffsetRelease.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferXAxisParam.OffsetReactionCellRelease;

            //反応容器搬送部Z軸
            numParamZAxisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferZAxisParam.MotorSpeed[0].InitSpeed;
            numParamZAxisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferZAxisParam.MotorSpeed[0].TopSpeed;
            numParamZAxisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferZAxisParam.MotorSpeed[0].Accel;
            numParamZAxisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferZAxisParam.MotorSpeed[0].ConstSpeed;
            //オフセット
            numParamZAxisOffsetCatch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferZAxisParam.OffsetReactionCellCatch;
            numParamZAxisOffsetRelease.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferZAxisParam.OffsetReactionCellRelease;
            //オフセット（モーター調整用）
            numAdjustZAxisOffsetCatch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferZAxisParam.OffsetReactionCellCatch;
            numAdjustZAxisOffsetRelease.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionCellTransferZAxisParam.OffsetReactionCellRelease;

            //ケース搬送
            numAdjustCaseYAxisOffsetCaseCatchRelease.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.OffsetCaseCatchRelease;
            numAdjustCaseYAxisOffsetReactionCellCatch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.OffsetReactionCellCatch;
            numAdjustCaseYAxisOffsetTipCatch.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].caseTransferYAxisParam.OffsetSamplingTipCatch;
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
        /// Adjustグループ全Disable
        /// </summary>
        private void adjustGbAllDisable()
        {

            numAdjustReactionCellNo.Enabled = false;
            cmbAdjustCoarseFine.Enabled = false;

            gbxAdjustXAxis.Enabled = false;
            numAdjustXAxisOffsetCatch.Enabled = false;
            numAdjustXAxisOffsetRelease.Enabled = false;

            gbxAdjustZAxis.Enabled = false;
            numAdjustZAxisOffsetCatch.Enabled = false;
            numAdjustZAxisOffsetRelease.Enabled = false;

            gbxAdjustCaseYAxis.Enabled = false;
            numAdjustCaseYAxisOffsetCaseCatchRelease.Enabled = false;
            numAdjustCaseYAxisOffsetReactionCellCatch.Enabled = false;
            numAdjustCaseYAxisOffsetTipCatch.Enabled = false;

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
            Func<int> reactioncell = () => { return (int)numAdjustReactionCellNo.Value; };
            Func<int> coarsefine = () => { return (int)cmbAdjustCoarseFine.Value; };

            AdjustStartComm.Pos = (int)Invoke(getStopPos);
            switch (AdjustStartComm.Pos)
            {
                case (int)MotorAdjustStopPosition.ReactionCellCatch:
                case (int)MotorAdjustStopPosition.ReactionCellRelease:
                    AdjustStartComm.Arg1 = (int)Invoke(reactioncell);
                    AdjustStartComm.Arg2 = (int)Invoke(coarsefine);
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
                    if (DialogResult.OK != GUI.DlgMessage.Show(Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_SAVEMASSAGE
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
            btnAdjustXAxisLeft.Enabled = enable;
            btnAdjustXAxisRight.Enabled = enable;

            btnAdjustZAxisDown.Enabled = enable;
            btnAdjustZAxisUp.Enabled = enable;

            btnAdjustCaseYAxisForward.Enabled = enable;
            btnAdjustCaseYAxisBack.Enabled = enable;
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
                case (int)MotorAdjustStopPosition.ReactionCellCatch:
                    numAdjustReactionCellNo.Enabled = true;
                    cmbAdjustCoarseFine.Enabled = true;

                    gbxAdjustXAxis.Enabled = true;
                    numAdjustXAxisOffsetCatch.Enabled = true;
                    numAdjustXAxisOffsetCatch.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetCatch.Enabled = true;
                    numAdjustZAxisOffsetCatch.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustCaseYAxis.Enabled = true;
                    numAdjustCaseYAxisOffsetReactionCellCatch.Enabled = true;
                    numAdjustCaseYAxisOffsetReactionCellCatch.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

                case (int)MotorAdjustStopPosition.ReactionCellRelease:
                    numAdjustReactionCellNo.Value = 1;                      //操作不可で1を設定した状態に変更する
                    cmbAdjustCoarseFine.Enabled = true;

                    gbxAdjustXAxis.Enabled = true;
                    numAdjustXAxisOffsetRelease.Enabled = true;
                    numAdjustXAxisOffsetRelease.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetRelease.Enabled = true;
                    numAdjustZAxisOffsetRelease.ForeColor = System.Drawing.Color.OrangeRed;

                    break;
            }
        }

        #region Pitch値の調整
        private void btnAdjustXAxisPitchUp_Click(object sender, EventArgs e)
        {
            numAdjustXAxisPitch.Value = (double)numAdjustXAxisPitch.Value + (double)numAdjustXAxisPitch.SpinIncrement;
        }

        private void btnAdjustXAxisPitchDown_Click(object sender, EventArgs e)
        {
            numAdjustXAxisPitch.Value = (double)numAdjustXAxisPitch.Value - (double)numAdjustXAxisPitch.SpinIncrement;
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
        #endregion

        private void btnAdjustXAxisLeftRight_Click(object sender, EventArgs e)
        {
            Boolean upFlg = ((Button)sender == btnAdjustXAxisRight);
            Int32 motorNo = (int)MotorNoList.ReactionCellTransferXAxis;
            Double pitchValue = (double)numAdjustXAxisPitch.Value;

            switch ((int)lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.ReactionCellCatch:
                    AdjustValue(upFlg, motorNo, numAdjustXAxisOffsetCatch, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.ReactionCellRelease:
                    AdjustValue(upFlg, motorNo, numAdjustXAxisOffsetRelease, pitchValue);
                    break;
            }
        }

        private void btnAdjustZAxisDownUp_Click(object sender, EventArgs e)
        {
            Boolean upFlg = ((Button)sender == btnAdjustZAxisDown);
            Int32 motorNo = (int)MotorNoList.ReactionCellTransferZAxis;
            Double pitchValue = (double)numAdjustZAxisPitch.Value;

            switch ((int)lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.ReactionCellCatch:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetCatch, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.ReactionCellRelease:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetRelease, pitchValue);
                    break;
            }
        }

        private void btnAdjustCaseYAxisForwardBack_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustCaseYAxisForward);
            Int32 motorNo = (int)MotorNoList.CaseTransferYAxis;
            Double pitchValue = (double)numAdjustCaseYAxisPitch.Value;

            AdjustValue(upFlg, motorNo, numAdjustCaseYAxisOffsetReactionCellCatch, pitchValue);
        }

    }
}
