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
    /// 試薬保冷庫ユニット
    /// </summary>
    public partial class FormReagentStorageUnit : FormUnitBase
    {
        IMaintenanceUnitStart unitStart = new UnitStart();
        IMaintenanceUnitStart unitStartAdjust = new UnitStarAdjust();
        IMaintenanceUnitStart unitAdjustAbort = new UnitStarAdjustAbort();
        volatile bool ResponseFlg;
        public CommonFunction ComFunc = new CommonFunction();
        SlaveCommCommand_0473 AdjustUpDownComm = new SlaveCommCommand_0473();

        public FormReagentStorageUnit()
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
            tabUnit.Tabs[0].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_025;                        //Test
            tabUnit.Tabs[1].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_026;                        //Configuration
            tabUnit.Tabs[2].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_027;                        //Motor Parameters
            tabUnit.Tabs[3].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_MOTORADJUSTMENT;              //Adjust

            //Testタブ
            gbxtestSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_000;                                  //Sequence
            object SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_001, No=(int)ReagentStorageSequence.Init},                 //1: Unit Initialization
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_002, No=(int)ReagentStorageSequence.ReagentTableBottle},   //2: Reagent Table Turn to the Bottle Setting Position
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_003, No=(int)ReagentStorageSequence.ReagentTableR1R1},     //3: Reagent Table Turn to the R1Aspiration Position(R1 Dispense Unit)
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_004, No=(int)ReagentStorageSequence.ReagentTableR2R1},     //4: Reagent Table Turn to the R2Aspiration Position(R1 Dispense Unit)
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_005, No=(int)ReagentStorageSequence.ReagentTableMR1},      //5: Reagent Table Turn to the M Aspiration Position(R1 Dispense Unit)
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_006, No=(int)ReagentStorageSequence.ReagentTableR1R2},     //6: Reagent Table Turn to the R1Aspiration Position(R2 Dispense Unit)
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_007, No=(int)ReagentStorageSequence.ReagentTableR2R2},     //7: Reagent Table Turn to the R2Aspiration Position(R2 Dispense Unit)
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_008, No=(int)ReagentStorageSequence.ReagentTableMR2},      //8: Reagent Table Turn to the M Aspiration Position(R2 Dispense Unit)
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_009, No=(int)ReagentStorageSequence.RBottleCheck},         //9: R Bottle Check
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_010, No=(int)ReagentStorageSequence.MBottlesCheck},        //10: M Bottles Check
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_011, No=(int)ReagentStorageSequence.RBottleBCID},          //11: R Bottle BC ID Reading
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_012, No=(int)ReagentStorageSequence.MBottlesBCID},         //12: M Bottles BC ID Reading
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_013, No=(int)ReagentStorageSequence.Mixing},               //13: Reagent Bottle Mixing
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_014, No=(int)ReagentStorageSequence.LocksUnlocksCover},    //14: Locks/Unlocks Cover
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_015, No=(int)ReagentStorageSequence.TurnsReagentTable},    //15: Turns Reagent Table
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_049, No=(int)ReagentStorageSequence.TurnsReagentCover},    //16: Turns Reagent Cover
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_052, No=(int)ReagentStorageSequence.RBottleCheckEX1},      //17: R Bottle Check(No action→No table rotation→Current position)
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_053, No=(int)ReagentStorageSequence.MBottlesCheckEX1},     //18: M Bottles Check(No action→No table rotation→Current position)
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_054, No=(int)ReagentStorageSequence.RBottleBCIDEX1},       //19: R Bottle BC ID Reading(No action→No table rotation→Current position)
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_055, No=(int)ReagentStorageSequence.MBottlesBCIDEX1},      //20: M Bottles BC ID Reading(No action→No table rotation→Current position)
            };
            lbxtestSequenceListBox = ComFunc.setSequenceListBox(lbxtestSequenceListBox, SequenceList);

            gbxtestParameters.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_017;                                //Parameters
            lbltestRepeatFrequency.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_018;                           //Repeat Frequency
            lbltestNumber.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_019;                                    //Number
            lbltestRepeatInterval.Text = Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_072;
            lbltestRepeatIntervalUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_073;
            lbltestPositionNo.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_056;                                //Position No
            lbltestLockOperation.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_021;                             //Lock Operation
            rbttestLockON.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_022;                                    //ON
            rbttestLockOFF.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_023;                                   //OFF
            rbttestLockONtoOFF.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_057;                               //ON→OFF
            lbltestStepNo.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_020;                                    //Step No.

            gbxtestResponce.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_024;                                  //Response

            //Configuration

            //MotorParameterタブ
            gbxParamTable.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_028;
            gbxParamTableCommon.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_033;
            gbxParamTableOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_034;
            lblParamTableInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_029;
            lblParamTableTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_030;
            lblParamTableAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_031;
            lblParamTableConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_032;
            lblParamTableOffsetMRead.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_060;
            lblParamTableOffsetRRead.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_061;
            lblParamTableOffsetR1UnitR1Asp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_062;
            lblParamTableOffsetR1UnitR2Asp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_063;
            lblParamTableOffsetR1UnitMAsp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_064;
            lblParamTableOffsetMRBottleCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_065;
            lblParamTableOffsetR2UnitMAsp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_066;
            lblParamTableOffsetR2UnitR2Asp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_067;
            lblParamTableOffsetEncodeThresh.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_074;
            lblParamTableInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_036;
            lblParamTableTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_036;
            lblParamTableAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_037;
            lblParamTableConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_036;
            lblParamTableOffsetMReadUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblParamTableOffsetRReadUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblParamTableOffsetR1UnitR1AspUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblParamTableOffsetR1UnitR2AspUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblParamTableOffsetR1UnitMAspUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblParamTableOffsetMRBottleCheckUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblParamTableOffsetR2UnitMAspUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblParamTableOffsetR2UnitR2AspUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblParamTableOffsetEncodeThreshUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;

            gbxParamMixing.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_039;
            gbxParamMixingCommon.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_033;
            lblParamMixingInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_029;
            lblParamMixingTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_030;
            lblParamMixingAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_031;
            lblParamMixingConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_032;
            lblParamMixingInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_036;
            lblParamMixingTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_036;
            lblParamMixingAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_037;
            lblParamMixingConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_036;


            //MotorAdjustタブ
            gbxAdjustSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_000;
            SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_069, No=(int)MotorAdjustStopPosition.MReagentIDReading},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_070, No=(int)MotorAdjustStopPosition.RReagentIDReading},
            };
            lbxAdjustSequence = ComFunc.setSequenceListBox(lbxAdjustSequence, SequenceList);

            gbxAdjustParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_017;
            lblAdjustPortNumber.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_071;
            lblAdjustBCRread.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_BCR_SENSOR;

            gbxAdjustBC.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_BCR_SENSOR_READ;
            btnAdjustBCRead.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_BCREAD;
            btnAdjustNoneSensor.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_BOTTLE_CHECK;
            btnAdjustBCReadR.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_R_BCREAD;
            btnAdjustNoneSensorR.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_R_BOTTLE_CHECK;

            gbxAdjustTable.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_028;
            lblAdjustTableOffsetMRead.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_060;
            lblAdjustTableOffsetRRead.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_061;
            lblAdjustTableOffsetR1UnitR1Asp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_062;
            lblAdjustTableOffsetR1UnitR2Asp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_063;
            lblAdjustTableOffsetR1UnitMAsp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_064;
            lblAdjustTableOffsetMRBottleCheck.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_065;
            lblAdjustTableOffsetR2UnitMAsp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_066;
            lblAdjustTableOffsetR2UnitR2Asp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_067;
            lblAdjustTableOffsetEncodeThresh.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_074;
            lblAdjustTableOffsetMReadUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblAdjustTableOffsetRReadUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblAdjustTableOffsetR1UnitR1AspUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblAdjustTableOffsetR1UnitR2AspUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblAdjustTableOffsetR1UnitMAspUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblAdjustTableOffsetMRBottleCheckUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblAdjustTableOffsetR2UnitMAspUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblAdjustTableOffsetR2UnitR2AspUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblAdjustTableOffsetEncodeThreshUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REAGENTSTORAGE_068;
            lblAdjustTablePitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;
            btnAdjustTableCCW.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_CCW;
            btnAdjustTableCW.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_CW;

        }
        #endregion

        /// <summary>
        /// ラジオボタンの選択値を設定
        /// </summary>
        private void setRadioButtonValue()
        {
            rbttestLockON.Tag = (int)ReagentStorageRadioValue.LockON;
            rbttestLockOFF.Tag = (int)ReagentStorageRadioValue.LockOFF;
            rbttestLockONtoOFF.Tag = (int)ReagentStorageRadioValue.LockONtoOFF;
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
                    StartComm.UnitNo = (int)UnitNoList.ReagentStorage;
                    StartComm.FuncNo = FuncNo;

                    switch (FuncNo)
                    {
                        case (int)ReagentStorageSequence.ReagentTableBottle:
                        case (int)ReagentStorageSequence.ReagentTableR1R1:
                        case (int)ReagentStorageSequence.ReagentTableR2R1:
                        case (int)ReagentStorageSequence.ReagentTableMR1:
                        case (int)ReagentStorageSequence.ReagentTableR1R2:
                        case (int)ReagentStorageSequence.ReagentTableR2R2:
                        case (int)ReagentStorageSequence.ReagentTableMR2:
                        case (int)ReagentStorageSequence.RBottleCheck:
                        case (int)ReagentStorageSequence.MBottlesCheck:
                        case (int)ReagentStorageSequence.RBottleBCID:
                        case (int)ReagentStorageSequence.MBottlesBCID:
                            StartComm.Arg1 = (int)numtestPositionNo.Value;
                            break;

                        case (int)ReagentStorageSequence.LocksUnlocksCover:
                            StartComm.Arg1 = ComFunc.getSelectedRadioButtonValue(gbxtestLockOperation);
                            break;

                        case (int)ReagentStorageSequence.TurnsReagentTable:
                            StartComm.Arg1 = (int)numtestStepNo.Value;
                            break;

                        case (int)ReagentStorageSequence.TurnsReagentCover:
                            StartComm.Arg1 = (int)numtestPositionNo.Value;
                            StartComm.Arg2 = ComFunc.getSelectedRadioButtonValue(gbxtestLockOperation);
                            break;
                    }

                    //レスポンスがある機能の場合のみ、レスポンスのログファイルを作成
                    if (ComFunc.chkExistsResponse(MaintenanceMainNavi.ReagentStorageUnit, FuncNo))
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

                        //コマンド送信
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
                    switch (tabUnit.SelectedTab.Index)
                    {
                        case (int)MaintenanceTabIndex.Test:
                            string Result = comm.Command.CommandText.Remove(0, 20).Trim();

                            if (ComFunc.chkExistsResponse(MaintenanceMainNavi.ReagentStorageUnit, (int)lbxtestSequenceListBox.SelectedValue))
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
                            txtAdjustBC.Text += comm.Command.CommandText.Remove(0, 20) + System.Environment.NewLine;
                            WriteLog(comm.Command.CommandText.Remove(0, 20));
                            txtAdjustBC.SelectionStart = txtAdjustBC.Text.Length;
                            txtAdjustBC.Focus();
                            txtAdjustBC.ScrollToCaret();

                            break;
                    }

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
            numtestPositionNo.Enabled = false;
            gbxtestLockOperation.Enabled = false;
            rbttestLockONtoOFF.Enabled = false;
            numtestStepNo.Enabled = false;
        }

        /// <summary>
        /// 選択された機能番号に応じてパラメータを活性化する
        /// </summary>
        private void lbxtestSequenceListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParametersAllFalse();
            switch (lbxtestSequenceListBox.SelectedValue)
            {
                case (int)ReagentStorageSequence.ReagentTableBottle:
                case (int)ReagentStorageSequence.ReagentTableR1R1:
                case (int)ReagentStorageSequence.ReagentTableR2R1:
                case (int)ReagentStorageSequence.ReagentTableMR1:
                case (int)ReagentStorageSequence.ReagentTableR1R2:
                case (int)ReagentStorageSequence.ReagentTableR2R2:
                case (int)ReagentStorageSequence.ReagentTableMR2:
                case (int)ReagentStorageSequence.RBottleCheck:
                case (int)ReagentStorageSequence.MBottlesCheck:
                case (int)ReagentStorageSequence.RBottleBCID:
                case (int)ReagentStorageSequence.MBottlesBCID:
                    numtestPositionNo.Enabled = true;
                    break;

                case (int)ReagentStorageSequence.LocksUnlocksCover:
                    gbxtestLockOperation.Enabled = true;

                    rbttestLockON.Checked = true;
                    break;

                case (int)ReagentStorageSequence.TurnsReagentTable:
                    numtestStepNo.Enabled = true;
                    break;

                case (int)ReagentStorageSequence.TurnsReagentCover:
                    numtestPositionNo.Enabled = true;
                    gbxtestLockOperation.Enabled = true;
                    rbttestLockONtoOFF.Enabled = true;

                    rbttestLockON.Checked = true;
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

            //試薬保冷庫テーブルθ軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.ReagentStorageTableThetaAxis;
            if (!adjustSave)
            {
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamTableInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamTableTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamTableAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamTableConst.Value;

                //オフセット
                Mparam.MotorOffset[0] = (double)numParamTableOffsetMRead.Value;
                Mparam.MotorOffset[1] = (double)numParamTableOffsetRRead.Value;
                Mparam.MotorOffset[2] = (double)numParamTableOffsetR1UnitR1Asp.Value;
                Mparam.MotorOffset[3] = (double)numParamTableOffsetR1UnitR2Asp.Value;
                Mparam.MotorOffset[4] = (double)numParamTableOffsetR1UnitMAsp.Value;
                Mparam.MotorOffset[5] = (double)numParamTableOffsetMRBottleCheck.Value;
                Mparam.MotorOffset[6] = (double)numParamTableOffsetR2UnitMAsp.Value;
                Mparam.MotorOffset[7] = (double)numParamTableOffsetR2UnitR2Asp.Value;
                Mparam.MotorOffset[8] = (double)numParamTableOffsetEncodeThresh.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                //送信用アイテムにセット
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustTableOffsetMRead.Value;
                Mparam.MotorOffset[1] = (double)numAdjustTableOffsetRRead.Value;
                Mparam.MotorOffset[2] = (double)numAdjustTableOffsetR1UnitR1Asp.Value;
                Mparam.MotorOffset[3] = (double)numAdjustTableOffsetR1UnitR2Asp.Value;
                Mparam.MotorOffset[4] = (double)numAdjustTableOffsetR1UnitMAsp.Value;
                Mparam.MotorOffset[5] = (double)numAdjustTableOffsetMRBottleCheck.Value;
                Mparam.MotorOffset[6] = (double)numAdjustTableOffsetR2UnitMAsp.Value;
                Mparam.MotorOffset[7] = (double)numAdjustTableOffsetR2UnitR2Asp.Value;
                Mparam.MotorOffset[8] = (double)numAdjustTableOffsetEncodeThresh.Value;
            }
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetMReagentIDReading = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetRReagentIDReading = Mparam.MotorOffset[1];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR1UnitR1Aspiration = Mparam.MotorOffset[2];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR1UnitR2Aspiration = Mparam.MotorOffset[3];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR1UnitMReagentAspiration = Mparam.MotorOffset[4];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetMRBottleCheck = Mparam.MotorOffset[5];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR2UnitMReagentAspiration = Mparam.MotorOffset[6];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR2UnitR2Aspiration = Mparam.MotorOffset[7];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetEncodeThresh = Mparam.MotorOffset[8];
            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);

            if (!adjustSave)
            {
                //試薬保冷庫部撹拌θ軸
                Mparam = new SlaveCommCommand_0471();
                Mparam.MotorNo = (int)MotorNoList.ReagentStorageMixingThetaAxis;

                Mparam.MotorSpeed[0].InitSpeed = (int)numParamMixingInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamMixingTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamMixingAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamMixingConst.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageMixingThetaAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageMixingThetaAxisParam.MotorSpeed = Mparam.MotorSpeed;

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
                        try
                        {
                            //関連する他の画面のモーターパラメータの再表示を行う
                            FormMaintenanceMain formMaintenanceMain = (FormMaintenanceMain)Application.OpenForms[typeof(FormMaintenanceMain).Name];
                            formMaintenanceMain.subFormR1Dispense[formMaintenanceMain.moduleIndex].MotorParamDisp();
                            formMaintenanceMain.subFormR2Dispense[formMaintenanceMain.moduleIndex].MotorParamDisp();
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

            //試薬保冷庫テーブルθ軸
            numParamTableInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.MotorSpeed[0].InitSpeed;
            numParamTableTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.MotorSpeed[0].TopSpeed;
            numParamTableAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.MotorSpeed[0].Accel;
            numParamTableConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.MotorSpeed[0].ConstSpeed;
            //オフセット
            numParamTableOffsetMRead.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetMReagentIDReading;
            numParamTableOffsetRRead.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetRReagentIDReading;
            numParamTableOffsetR1UnitR1Asp.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR1UnitR1Aspiration;
            numParamTableOffsetR1UnitR2Asp.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR1UnitR2Aspiration;
            numParamTableOffsetR1UnitMAsp.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR1UnitMReagentAspiration;
            numParamTableOffsetMRBottleCheck.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetMRBottleCheck;
            numParamTableOffsetR2UnitMAsp.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR2UnitMReagentAspiration;
            numParamTableOffsetR2UnitR2Asp.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR2UnitR2Aspiration;
            numParamTableOffsetEncodeThresh.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetEncodeThresh;
            //オフセット（モーター調整用）
            numAdjustTableOffsetMRead.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetMReagentIDReading;
            numAdjustTableOffsetRRead.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetRReagentIDReading;
            numAdjustTableOffsetR1UnitR1Asp.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR1UnitR1Aspiration;
            numAdjustTableOffsetR1UnitR2Asp.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR1UnitR2Aspiration;
            numAdjustTableOffsetR1UnitMAsp.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR1UnitMReagentAspiration;
            numAdjustTableOffsetMRBottleCheck.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetMRBottleCheck;
            numAdjustTableOffsetR2UnitMAsp.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR2UnitMReagentAspiration;
            numAdjustTableOffsetR2UnitR2Asp.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetR2UnitR2Aspiration;
            numAdjustTableOffsetEncodeThresh.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageTableThetaAxisParam.OffsetEncodeThresh;

            //試薬保冷庫撹拌θ軸
            numParamMixingInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageMixingThetaAxisParam.MotorSpeed[0].InitSpeed;
            numParamMixingTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageMixingThetaAxisParam.MotorSpeed[0].TopSpeed;
            numParamMixingAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageMixingThetaAxisParam.MotorSpeed[0].Accel;
            numParamMixingConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reagentStorageMixingThetaAxisParam.MotorSpeed[0].ConstSpeed;

        }

        /// <summary>
        /// パラメータ保存
        /// </summary>
        public override void ParamSave()
        {
            switch (tabUnit.SelectedTab.Index)
            {
                case (int)MaintenanceTabIndex.Config:
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
            gbxAdjustTable.Enabled = false;

            numAdjustPortNumber.Enabled = false;
            numAdjustBCRread.Enabled = false;

            //Table Theta Axis
            numAdjustTableOffsetMRead.Enabled = false;
            numAdjustTableOffsetRRead.Enabled = false;
            numAdjustTableOffsetR1UnitR1Asp.Enabled = false;
            numAdjustTableOffsetR1UnitR2Asp.Enabled = false;
            numAdjustTableOffsetR1UnitMAsp.Enabled = false;
            numAdjustTableOffsetMRBottleCheck.Enabled = false;
            numAdjustTableOffsetR2UnitMAsp.Enabled = false;
            numAdjustTableOffsetR2UnitR2Asp.Enabled = false;
            numAdjustTableOffsetEncodeThresh.Enabled = false;

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
            Func<int> portno = () => { return (int)numAdjustPortNumber.Value; };

            AdjustStartComm.Pos = (int)Invoke(getStopPos);
            switch (AdjustStartComm.Pos)
            {
                case (int)MotorAdjustStopPosition.MReagentIDReading:
                case (int)MotorAdjustStopPosition.RReagentIDReading:
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
                case (int)MaintenanceTabIndex.Config:
                    break;
                case (int)MaintenanceTabIndex.MParam:
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
            btnAdjustTableCCW.Enabled = enable;
            btnAdjustTableCW.Enabled = enable;
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
            switch ((int)lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.MReagentIDReading:
                    numAdjustPortNumber.Enabled = true;
                    numAdjustBCRread.Enabled = true;
                    gbxAdjustBC.Enabled = true;
                    btnAdjustBCRead.Enabled = true;
                    btnAdjustNoneSensor.Enabled = true;
                    gbxAdjustTable.Enabled = true;
                    numAdjustTableOffsetMRead.Enabled = true;
                    numAdjustTableOffsetMRead.Appearance.ForeColor = System.Drawing.Color.OrangeRed;
                    break;
                case (int)MotorAdjustStopPosition.RReagentIDReading:
                    numAdjustPortNumber.Enabled = true;
                    numAdjustBCRread.Enabled = true;
                    gbxAdjustBC.Enabled = true;
                    btnAdjustBCReadR.Enabled = true;
                    btnAdjustNoneSensorR.Enabled = true;
                    gbxAdjustTable.Enabled = true;
                    numAdjustTableOffsetRRead.Enabled = true;
                    numAdjustTableOffsetRRead.Appearance.ForeColor = System.Drawing.Color.OrangeRed;
                    break;
            }
        }

        /// <summary>
        /// M試薬バーコード読取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdjustBCRead_Click(object sender, EventArgs e)
        {
            btnAdjustBCRead.Enabled = false;

            SlaveCommCommand_0439 StartComm = new SlaveCommCommand_0439();
            StartComm.UnitNo = (int)UnitNoList.ReagentStorage;
            StartComm.FuncNo = (int)ReagentStorageSequence.MBottlesBCIDEX1;
            StartComm.Arg1 = 1;

            for (int i = 0; i < (int)numAdjustBCRread.Value; i++)
            {
                // 受信待ちフラグを設定
                ResponseFlg = true;

                // コマンド送信
                unitStart.Start(StartComm);

                // 応答コマンド受信待ち
                while (ResponseFlg)
                {
                    Application.DoEvents();
                }
            }

            btnAdjustBCRead.Enabled = true;
        }

        /// <summary>
        /// R試薬バーコード読取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdjustBCReadR_Click(object sender, EventArgs e)
        {
            btnAdjustBCReadR.Enabled = false;

            SlaveCommCommand_0439 StartComm = new SlaveCommCommand_0439();
            StartComm.UnitNo = (int)UnitNoList.ReagentStorage;
            StartComm.FuncNo = (int)ReagentStorageSequence.RBottleBCIDEX1;
            StartComm.Arg1 = 1;

            for (int i = 0; i < (int)numAdjustBCRread.Value; i++)
            {
                // 受信待ちフラグを設定
                ResponseFlg = true;

                // コマンド送信
                unitStart.Start(StartComm);

                // 応答コマンド受信待ち
                while (ResponseFlg)
                {
                    Application.DoEvents();
                }
            }

            btnAdjustBCReadR.Enabled = true;
        }

        /// <summary>
        /// M試薬ボトル有無センサー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdjustNoneSensor_Click(object sender, EventArgs e)
        {
            btnAdjustNoneSensor.Enabled = false;

            SlaveCommCommand_0439 StartComm = new SlaveCommCommand_0439();
            StartComm.UnitNo = (int)UnitNoList.ReagentStorage;
            StartComm.FuncNo = (int)ReagentStorageSequence.MBottlesCheckEX1;
            StartComm.Arg1 = 1;

            for (int i = 0; i < (int)numAdjustBCRread.Value; i++)
            {
                // 受信待ちフラグを設定
                ResponseFlg = true;

                // コマンド送信
                unitStart.Start(StartComm);

                // 応答コマンド受信待ち
                while (ResponseFlg)
                {
                    // 受信待ちフラグを設定
                    ResponseFlg = true;

                    // コマンド送信
                    unitStart.Start(StartComm);

                    // 応答コマンド受信待ち
                    while (ResponseFlg)
                    {
                        Application.DoEvents();
                    }
                }
            }

            btnAdjustNoneSensor.Enabled = true;
        }

        /// <summary>
        /// R試薬ボトル有無センサー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdjustNoneSensorR_Click(object sender, EventArgs e)
        {
            btnAdjustNoneSensorR.Enabled = false;

            SlaveCommCommand_0439 StartComm = new SlaveCommCommand_0439();
            StartComm.UnitNo = (int)UnitNoList.ReagentStorage;
            StartComm.FuncNo = (int)ReagentStorageSequence.RBottleCheckEX1;
            StartComm.Arg1 = 1;

            for (int i = 0; i < (int)numAdjustBCRread.Value; i++)
            {
                // 受信待ちフラグを設定
                ResponseFlg = true;

                // コマンド送信
                unitStart.Start(StartComm);

                // 応答コマンド受信待ち
                while (ResponseFlg)
                {
                    // 受信待ちフラグを設定
                    ResponseFlg = true;

                    // コマンド送信
                    unitStart.Start(StartComm);

                    // 応答コマンド受信待ち
                    while (ResponseFlg)
                    {
                        Application.DoEvents();
                    }
                }
            }

            btnAdjustNoneSensorR.Enabled = true;
        }

        #region Pitch値の調整
        private void btnAdjustTablePitchUp_Click(object sender, EventArgs e)
        {
            numAdjustTablePitch.Value = (double)numAdjustTablePitch.Value + (double)numAdjustTablePitch.SpinIncrement;
        }

        private void btnAdjustTablePitchDown_Click(object sender, EventArgs e)
        {
            numAdjustTablePitch.Value = (double)numAdjustTablePitch.Value - (double)numAdjustTablePitch.SpinIncrement;
        }
        #endregion

        private void btnAdjustTableCCWCW_Click(object sender, EventArgs e)
        {
            //Senderが加算対象のボタンならTrue
            Boolean upFlg = ((Button)sender == btnAdjustTableCW);
            Int32 motorNo = (int)MotorNoList.ReagentStorageTableThetaAxis;
            Double pitchValue = (double)numAdjustTablePitch.Value;

            switch ((int)lbxAdjustSequence.SelectedValue)
            {
                case (int)MotorAdjustStopPosition.MReagentIDReading:
                    AdjustValue(upFlg, motorNo, numAdjustTableOffsetMRead, pitchValue);
                    break;
                case (int)MotorAdjustStopPosition.RReagentIDReading:
                    AdjustValue(upFlg, motorNo, numAdjustTableOffsetRRead, pitchValue);
                    break;
            }
        }
    }
}
