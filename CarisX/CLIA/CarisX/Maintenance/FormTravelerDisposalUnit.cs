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
    /// トラベラー・廃棄部
    /// </summary>
    public partial class FormTravelerDisposalUnit : FormUnitBase
    {
        IMaintenanceUnitStart unitStart = new UnitStart();
        IMaintenanceUnitStart unitStartAdjust = new UnitStarAdjust();
        IMaintenanceUnitStart unitAdjustAbort = new UnitStarAdjustAbort();
        volatile bool ResponseFlg;
        public CommonFunction ComFunc = new CommonFunction();

        public FormTravelerDisposalUnit()
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
        }

        #region リソース設定
        private void setCulture()
        {
            //Tab
            tabUnit.Tabs[0].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_000;    //Test
            tabUnit.Tabs[1].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_001;    //Configuration
            tabUnit.Tabs[2].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_002;    //Motor Parameters

            //Testタブ
            gbxtestSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_003;//Sequence
            object SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_004, No=(int)TravelerDisposalSequence.Init},                         //1: Unit Initialization
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_005, No=(int)TravelerDisposalSequence.DisposalReactionCell},         //2: Disposal of Reaction Cell
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_006, No=(int)TravelerDisposalSequence.MoveBFTablefromReactionTable}, //3: Move to BF Table from Reaction Table
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_007, No=(int)TravelerDisposalSequence.MoveReactionTablefromBFTable}, //4: Move to Reaction Table (Inner) from BF Table (Inner)
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_008, No=(int)TravelerDisposalSequence.MoveBFTableOuterfromInner},    //5: Move to BF Table (Outer) from BF Table (Inner)
            };
            lbxtestSequenceListBox = ComFunc.setSequenceListBox(lbxtestSequenceListBox, SequenceList);

            gbxtestParameters.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_009;          //Parameters
            lbltestRepeatFrequency.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_010;     //Repeat Frequency
            lbltestNumber.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_011;              //Number
            lbltestRepeatInterval.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_036;
            lbltestRepeatIntervalUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_037;
            lbltestReactionCellPosition.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_038;
            rbttestReactionTableInner.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_021;
            rbttestReactionTableOuter.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_022;
            rbttestBFTableInner.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_023;
            rbttestBFTableOuter.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_024;
            lbltestTable.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_039;
            rbttestInnerOuter.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_040;
            rbttestOuterInner.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_041;

            gbxtestResponce.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_012;            //Responce

            //Configurationタブ
            gbxconfParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_009;

            lblconfWaitTimeAfterReactContainer.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_034;
            lblconfWaitTimeAfterReactContainerUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_035;

            //MotorParameterタブ
            gbxParamXAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_013;
            gbxParamXAxisCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_027;
            gbxParamXAxisOffset.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_028;
            lblParamXAxisInit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_015;
            lblParamXAxisTop.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_016;
            lblParamXAxisAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_017;
            lblParamXAxisConst.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_018;
            lblParamXAxisOffsetReactionTableInside.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_021;
            lblParamXAxisOffsetReactionTableOutside.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_022;
            lblParamXAxisOffsetReactionCellRemover.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_025;
            lblParamXAxisOffsetBFTableOutside.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_024;
            lblParamXAxisOffsetBFTableInside.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_023;
            lblParamXAxisInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_019;
            lblParamXAxisTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_019;
            lblParamXAxisAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_020;
            lblParamXAxisConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_019;
            lblParamXAxisOffsetReactionTableInsideUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_026;
            lblParamXAxisOffsetReactionTableOutsideUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_026;
            lblParamXAxisOffsetReactionCellRemoverUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_026;
            lblParamXAxisOffsetBFTableOutsideUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_026;
            lblParamXAxisOffsetBFTableInsideUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_026;

            gbxParamZAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_014;
            gbxParamZAxisCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_027;
            gbxParamZAxisOffset.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_028;
            lblParamZAxisInit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_015;
            lblParamZAxisTop.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_016;
            lblParamZAxisAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_017;
            lblParamZAxisConst.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_018;
            lblParamZAxisOffsetReactionTableInside.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_021;
            lblParamZAxisOffsetReactionTableOutside.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_022;
            lblParamZAxisOffsetReactionCellRemover.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_025;
            lblParamZAxisOffsetBFTableOutside.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_024;
            lblParamZAxisOffsetBFTableInside.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_023;
            lblParamZAxisInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_019;
            lblParamZAxisTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_019;
            lblParamZAxisAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_020;
            lblParamZAxisConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_019;
            lblParamZAxisOffsetReactionTableInsideUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_026;
            lblParamZAxisOffsetReactionTableOutsideUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_026;
            lblParamZAxisOffsetReactionCellRemoverUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_026;
            lblParamZAxisOffsetBFTableOutsideUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_026;
            lblParamZAxisOffsetBFTableInsideUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_026;

            //MotorAdjustタブ
            gbxAdjustSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_003;
            SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_029, No=(int)MotorAdjustStopPosition.ReactionTableInside},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_030, No=(int)MotorAdjustStopPosition.ReactionTableOutside},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_031, No=(int)MotorAdjustStopPosition.ReactionCellRemover},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_032, No=(int)MotorAdjustStopPosition.BFTableOutside},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_033, No=(int)MotorAdjustStopPosition.BFTableInside},
            };
            lbxAdjustSequence = ComFunc.setSequenceListBox(lbxAdjustSequence, SequenceList);

            gbxAdjustParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_009;

            gbxAdjustXAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_013;
            lblAdjustXAxisOffsetReactionTableInside.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_021;
            lblAdjustXAxisOffsetReactionTableOutside.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_022;
            lblAdjustXAxisOffsetReactionCellRemover.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_025;
            lblAdjustXAxisOffsetBFTableOutside.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_024;
            lblAdjustXAxisOffsetBFTableInside.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_023;
            lblAdjustXAxisOffsetReactionTableInsideUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_026;
            lblAdjustXAxisOffsetReactionTableOutsideUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_026;
            lblAdjustXAxisOffsetReactionCellRemoverUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_026;
            lblAdjustXAxisOffsetBFTableOutsideUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_026;
            lblAdjustXAxisOffsetBFTableInsideUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_026;

            gbxAdjustZAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_014;
            lblAdjustZAxisOffsetReactionTableInside.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_021;
            lblAdjustZAxisOffsetReactionTableOutside.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_022;
            lblAdjustZAxisOffsetReactionCellRemover.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_025;
            lblAdjustZAxisOffsetBFTableOutside.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_024;
            lblAdjustZAxisOffsetBFTableInside.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_023;
            lblAdjustZAxisOffsetReactionTableInsideUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_026;
            lblAdjustZAxisOffsetReactionTableOutsideUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_026;
            lblAdjustZAxisOffsetReactionCellRemoverUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_026;
            lblAdjustZAxisOffsetBFTableOutsideUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_026;
            lblAdjustZAxisOffsetBFTableInsideUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_TRAVELERDISPOSAL_026;

            lblAdjustXAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;
            lblAdjustZAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;

            btnAdjustXAxisLeft.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_LEFT;
            btnAdjustXAxisRight.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_RIGHT;

            btnAdjustZAxisDown.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_DOWN;
            btnAdjustZAxisUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_UP;

        }
        #endregion

        /// <summary>
        /// ラジオボタンの選択値を設定
        /// </summary>
        private void setRadioButtonValue()
        {
            rbttestReactionTableInner.Tag = (int)TravelerDisposalRadioValue.ReactionTableInner;
            rbttestReactionTableOuter.Tag = (int)TravelerDisposalRadioValue.ReactionTableOuter;
            rbttestBFTableInner.Tag = (int)TravelerDisposalRadioValue.BFTableInner;
            rbttestBFTableOuter.Tag = (int)TravelerDisposalRadioValue.BFTableOuter;
            rbttestInnerOuter.Tag = (int)TravelerDisposalRadioValue.InnerOuter;
            rbttestOuterInner.Tag = (int)TravelerDisposalRadioValue.OuterInner;
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
                    StartComm.UnitNo = (int)UnitNoList.TravelerDisposal;
                    StartComm.FuncNo = FuncNo;

                    switch (FuncNo)
                    {
                        case (int)TravelerDisposalSequence.DisposalReactionCell:
                            StartComm.Arg1 = ComFunc.getSelectedRadioButtonValue(gbxtestReactionCellPosition);
                            break;
                        case (int)TravelerDisposalSequence.MoveBFTablefromReactionTable:
                            StartComm.Arg1 = ComFunc.getSelectedRadioButtonValue(gbxtestTable);
                            break;
                    }

                    // レスポンスがある機能の場合のみ、レスポンスのログファイルを作成
                    if (ComFunc.chkExistsResponse(MaintenanceMainNavi.TravelerandDisposalUnit, FuncNo))
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
                    TextData resultText = new TextData(Result);
                    String tempString = String.Empty;

                    if (ComFunc.chkExistsResponse(MaintenanceMainNavi.TravelerandDisposalUnit, (int)lbxtestSequenceListBox.SelectedValue))
                    {
                        switch ((int)lbxtestSequenceListBox.SelectedValue)
                        {
                            case (int)TravelerDisposalSequence.DisposalReactionCell:
                                resultText.spoilString(out tempString, 1);
                                Result = "Reaction Cell Setting (Inner)=" + tempString + ",";
                                resultText.spoilString(out tempString, 1);
                                Result += "Reaction Cell Setting (Outer)=" + tempString + ",";
                                resultText.spoilString(out tempString, 1);
                                Result += "Reaction Cell Setting (BF1)=" + tempString + ",";
                                resultText.spoilString(out tempString, 1);
                                Result += "Reaction Cell Setting (BF2)=" + tempString;
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
            gbxtestReactionCellPosition.Enabled = false;
            gbxtestTable.Enabled = false;
        }

        /// <summary>
        /// 選択された機能番号に応じてパラメータを活性化する
        /// </summary>
        private void lbxtestSequenceListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParametersAllFalse();
            switch (lbxtestSequenceListBox.SelectedValue)
            {
                case (int)TravelerDisposalSequence.DisposalReactionCell:
                    gbxtestReactionCellPosition.Enabled = true;
                    break;
                case (int)TravelerDisposalSequence.MoveBFTablefromReactionTable:
                    gbxtestTable.Enabled = true;
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

            //トラベラー・廃棄部X軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.TravelerXAxis;
            if (!adjustSave)
            {
                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamXAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamXAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamXAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamXAxisConst.Value;

                Mparam.MotorOffset[0] = (double)numParamXAxisOffsetReactionTableInside.Value;
                Mparam.MotorOffset[1] = (double)numParamXAxisOffsetReactionTableOutside.Value;
                Mparam.MotorOffset[2] = (double)numParamXAxisOffsetReactionCellRemover.Value;
                Mparam.MotorOffset[3] = (double)numParamXAxisOffsetBFTableOutside.Value;
                Mparam.MotorOffset[4] = (double)numParamXAxisOffsetBFTableInside.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                //送信用アイテムにセット
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustXAxisOffsetReactionTableInside.Value;
                Mparam.MotorOffset[1] = (double)numAdjustXAxisOffsetReactionTableOutside.Value;
                Mparam.MotorOffset[2] = (double)numAdjustXAxisOffsetReactionCellRemover.Value;
                Mparam.MotorOffset[3] = (double)numAdjustXAxisOffsetBFTableOutside.Value;
                Mparam.MotorOffset[4] = (double)numAdjustXAxisOffsetBFTableInside.Value;
            }
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.OffsetReactionTableInside = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.OffsetReactionTableOutside = Mparam.MotorOffset[1];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.OffsetReactionCellRemover = Mparam.MotorOffset[2];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.OffsetBFTableOutside = Mparam.MotorOffset[3];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.OffsetBFTableInside = Mparam.MotorOffset[4];

            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);


            //トラベラー・廃棄部Z軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.TravelerZAxis;
            if (!adjustSave)
            {
                //モーターパラメータの保存
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamZAxisInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamZAxisTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamZAxisAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamZAxisConst.Value;

                Mparam.MotorOffset[0] = (double)numParamZAxisOffsetReactionTableInside.Value;
                Mparam.MotorOffset[1] = (double)numParamZAxisOffsetReactionTableOutside.Value;
                Mparam.MotorOffset[2] = (double)numParamZAxisOffsetReactionCellRemover.Value;
                Mparam.MotorOffset[3] = (double)numParamZAxisOffsetBFTableOutside.Value;
                Mparam.MotorOffset[4] = (double)numParamZAxisOffsetBFTableInside.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                //送信用アイテムにセット
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustZAxisOffsetReactionTableInside.Value;
                Mparam.MotorOffset[1] = (double)numAdjustZAxisOffsetReactionTableOutside.Value;
                Mparam.MotorOffset[2] = (double)numAdjustZAxisOffsetReactionCellRemover.Value;
                Mparam.MotorOffset[3] = (double)numAdjustZAxisOffsetBFTableOutside.Value;
                Mparam.MotorOffset[4] = (double)numAdjustZAxisOffsetBFTableInside.Value;
            }
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.OffsetReactionTableInside = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.OffsetReactionTableOutside = Mparam.MotorOffset[1];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.OffsetReactionCellRemover = Mparam.MotorOffset[2];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.OffsetBFTableOutside = Mparam.MotorOffset[3];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.OffsetBFTableInside = Mparam.MotorOffset[4];

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

            //トラベラー・廃棄部X軸
            numParamXAxisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.MotorSpeed[0].InitSpeed;
            numParamXAxisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.MotorSpeed[0].TopSpeed;
            numParamXAxisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.MotorSpeed[0].Accel;
            numParamXAxisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.MotorSpeed[0].ConstSpeed;
            //オフセット
            numParamXAxisOffsetReactionTableInside.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.OffsetReactionTableInside;
            numParamXAxisOffsetReactionTableOutside.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.OffsetReactionTableOutside;
            numParamXAxisOffsetReactionCellRemover.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.OffsetReactionCellRemover;
            numParamXAxisOffsetBFTableOutside.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.OffsetBFTableOutside;
            numParamXAxisOffsetBFTableInside.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.OffsetBFTableInside;
            //オフセット（モーター調整用）
            numAdjustXAxisOffsetReactionTableInside.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.OffsetReactionTableInside;
            numAdjustXAxisOffsetReactionTableOutside.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.OffsetReactionTableOutside;
            numAdjustXAxisOffsetReactionCellRemover.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.OffsetReactionCellRemover;
            numAdjustXAxisOffsetBFTableOutside.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.OffsetBFTableOutside;
            numAdjustXAxisOffsetBFTableInside.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerXAxisParam.OffsetBFTableInside;

            //トラベラー・廃棄部Z軸
            numParamZAxisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.MotorSpeed[0].InitSpeed;
            numParamZAxisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.MotorSpeed[0].TopSpeed;
            numParamZAxisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.MotorSpeed[0].Accel;
            numParamZAxisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.MotorSpeed[0].ConstSpeed;
            //オフセット
            numParamZAxisOffsetReactionTableInside.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.OffsetReactionTableInside;
            numParamZAxisOffsetReactionTableOutside.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.OffsetReactionTableOutside;
            numParamZAxisOffsetReactionCellRemover.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.OffsetReactionCellRemover;
            numParamZAxisOffsetBFTableOutside.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.OffsetBFTableOutside;
            numParamZAxisOffsetBFTableInside.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.OffsetBFTableInside;
            //オフセット（モーター調整用）
            numAdjustZAxisOffsetReactionTableInside.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.OffsetReactionTableInside;
            numAdjustZAxisOffsetReactionTableOutside.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.OffsetReactionTableOutside;
            numAdjustZAxisOffsetReactionCellRemover.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.OffsetReactionCellRemover;
            numAdjustZAxisOffsetBFTableOutside.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.OffsetBFTableOutside;
            numAdjustZAxisOffsetBFTableInside.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerZAxisParam.OffsetBFTableInside;

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

            CarisXConfigParameter.TravelerAndDisposalUnitConfigParam ConfigParam = new CarisXConfigParameter.TravelerAndDisposalUnitConfigParam();
            //反応容器搬送後遅延時間
            ConfigParam.WaitTimeAfterReactContainer = (double)numconfWaitTimeAfterReactContainer.Value;

            config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerAndDisposalUnitConfigParam = ConfigParam;

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

            //反応容器搬送後遅延時間
            numconfWaitTimeAfterReactContainer.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].travelerAndDisposalUnitConfigParam.WaitTimeAfterReactContainer;
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
            gbxAdjustXAxis.Enabled = false;
            numAdjustXAxisOffsetReactionTableInside.Enabled = false;
            numAdjustXAxisOffsetReactionTableOutside.Enabled = false;
            numAdjustXAxisOffsetReactionCellRemover.Enabled = false;
            numAdjustXAxisOffsetBFTableOutside.Enabled = false;
            numAdjustXAxisOffsetBFTableInside.Enabled = false;

            gbxAdjustZAxis.Enabled = false;
            numAdjustZAxisOffsetReactionTableInside.Enabled = false;
            numAdjustZAxisOffsetReactionTableOutside.Enabled = false;
            numAdjustZAxisOffsetReactionCellRemover.Enabled = false;
            numAdjustZAxisOffsetBFTableOutside.Enabled = false;
            numAdjustZAxisOffsetBFTableInside.Enabled = false;

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
            btnAdjustXAxisLeft.Enabled = enable;
            btnAdjustXAxisRight.Enabled = enable;

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
            adjustGbAllDisable();
            //モータ調整ボタンDisable
            UpDownButtonEnable(false);
            switch (lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.ReactionTableInside:
                    gbxAdjustXAxis.Enabled = true;
                    numAdjustXAxisOffsetReactionTableInside.Enabled = true;
                    numAdjustXAxisOffsetReactionTableInside.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetReactionTableInside.Enabled = true;
                    numAdjustZAxisOffsetReactionTableInside.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

                case (int)MotorAdjustStopPosition.ReactionTableOutside:
                    gbxAdjustXAxis.Enabled = true;
                    numAdjustXAxisOffsetReactionTableOutside.Enabled = true;
                    numAdjustXAxisOffsetReactionTableOutside.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetReactionTableOutside.Enabled = true;
                    numAdjustZAxisOffsetReactionTableOutside.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

                case (int)MotorAdjustStopPosition.ReactionCellRemover:
                    gbxAdjustXAxis.Enabled = true;
                    numAdjustXAxisOffsetReactionCellRemover.Enabled = true;
                    numAdjustXAxisOffsetReactionCellRemover.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetReactionCellRemover.Enabled = true;
                    numAdjustZAxisOffsetReactionCellRemover.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

                case (int)MotorAdjustStopPosition.BFTableOutside:
                    gbxAdjustXAxis.Enabled = true;
                    numAdjustXAxisOffsetBFTableOutside.Enabled = true;
                    numAdjustXAxisOffsetBFTableOutside.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetBFTableOutside.Enabled = true;
                    numAdjustZAxisOffsetBFTableOutside.ForeColor = System.Drawing.Color.OrangeRed;

                    break;

                case (int)MotorAdjustStopPosition.BFTableInside:
                    gbxAdjustXAxis.Enabled = true;
                    numAdjustXAxisOffsetBFTableInside.Enabled = true;
                    numAdjustXAxisOffsetBFTableInside.ForeColor = System.Drawing.Color.OrangeRed;

                    gbxAdjustZAxis.Enabled = true;
                    numAdjustZAxisOffsetBFTableInside.Enabled = true;
                    numAdjustZAxisOffsetBFTableInside.ForeColor = System.Drawing.Color.OrangeRed;

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

        #endregion

        private void btnAdjustXAxisLeftRight_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustXAxisRight);
            Int32 motorNo = (int)MotorNoList.TravelerXAxis;
            Double pitchValue = (double)numAdjustXAxisPitch.Value;

            switch ((int)lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.ReactionTableInside:
                    AdjustValue(upFlg, motorNo, numAdjustXAxisOffsetReactionTableInside, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.ReactionTableOutside:
                    AdjustValue(upFlg, motorNo, numAdjustXAxisOffsetReactionTableOutside, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.ReactionCellRemover:
                    AdjustValue(upFlg, motorNo, numAdjustXAxisOffsetReactionCellRemover, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.BFTableOutside:
                    AdjustValue(upFlg, motorNo, numAdjustXAxisOffsetBFTableOutside, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.BFTableInside:
                    AdjustValue(upFlg, motorNo, numAdjustXAxisOffsetBFTableInside, pitchValue);
                    break;
            }
        }

        private void btnAdjustZAxisDownUp_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustZAxisDown);
            Int32 motorNo = (int)MotorNoList.TravelerZAxis;
            Double pitchValue = (double)numAdjustZAxisPitch.Value;

            switch ((int)lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.ReactionTableInside:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetReactionTableInside, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.ReactionTableOutside:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetReactionTableOutside, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.ReactionCellRemover:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetReactionCellRemover, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.BFTableOutside:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetBFTableOutside, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.BFTableInside:
                    AdjustValue(upFlg, motorNo, numAdjustZAxisOffsetBFTableInside, pitchValue);
                    break;
            }
        }
    }
}
