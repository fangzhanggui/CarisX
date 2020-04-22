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
    /// 反応テーブル部
    /// </summary>
    public partial class FormReactionTableUnit : FormUnitBase
    {
        IMaintenanceUnitStart unitStart = new UnitStart();
        IMaintenanceUnitStart unitStartAdjust = new UnitStarAdjust();
        IMaintenanceUnitStart unitAdjustAbort = new UnitStarAdjustAbort();
        volatile bool ResponseFlg;
        public CommonFunction ComFunc = new CommonFunction();

        public FormReactionTableUnit()
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
        }

        #region リソース設定
        private void setCulture()
        {
            //TAB
            tabUnit.Tabs[0].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_000;               //Test
            tabUnit.Tabs[1].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_001;               //Configuration
            tabUnit.Tabs[2].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_002;               //Motor Parameters
            tabUnit.Tabs[3].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_MOTORADJUSTMENT;    //Adjust

            //Testタブ
            gbxtestSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_003;           //Sequence
            object SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_004, No=(int)ReactionTableSequence.Init},       //1: Unit Initialization
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_005, No=(int)ReactionTableSequence.Step},       //2: 1 Step
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_006, No=(int)ReactionTableSequence.Mixing},     //3: Mixing
            };
            lbxtestSequenceListBox = ComFunc.setSequenceListBox(lbxtestSequenceListBox, SequenceList);

            gbxtestParameters.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_007;         //Parameters
            lbltestRepeatFrequency.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_008;    //Repeat Frequency
            lbltestNumber.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_009;             //Number
            lbltestRepeatInterval.Text = Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_029;
            lbltestRepeatIntervalUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_030;
            lbltestMoveStep.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_011;         //Position No.

            gbxtestResponce.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_012;           //Responce

            //Configurationタブ
            gbxconfParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_007;//Parameters

            lblconfStrringTime.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_016;//Mixing Time
            lblconfStrringTimeUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_017;//(sec)

            //MotorParameter
            gbxParamTableTheta.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_013;
            gbxParamTableThetaCommon.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_024;
            gbxParamTableThetaAxisOffset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_025;
            lblParamTableThetaInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_018;
            lblParamTableThetaTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_019;
            lblParamTableThetaAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_020;
            lblParamTableThetaConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_021;
            lblParamTableThetaAxisOffsetHomePosition.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_026;
            lblParamTableThetaAxisOffsetEncodeThresh.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_036;
            lblParamTableThetaInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_022;
            lblParamTableThetaTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_022;
            lblParamTableThetaAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_023;
            lblParamTableThetaConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_022;
            lblParamTableThetaAxisOffsetHomePositionUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_027;
            lblParamTableThetaAxisOffsetEncodeThreshUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_027;

            gbxParamMixingR1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_014;
            gbxParamMixingR1A1stUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_031;
            lblParamMixingR1A1stUpInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_018;
            lblParamMixingR1A1stUpTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_019;
            lblParamMixingR1A1stUpAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_020;
            lblParamMixingR1A1stUpConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_021;
            lblParamMixingR1A1stUpInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_022;
            lblParamMixingR1A1stUpTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_022;
            lblParamMixingR1A1stUpAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_023;
            lblParamMixingR1A1stUpConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_022;

            gbxParamMixingR1B12ndUp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_032;
            lblParamMixingR1B12ndUpInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_018;
            lblParamMixingR1B12ndUpTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_019;
            lblParamMixingR1B12ndUpAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_020;
            lblParamMixingR1B12ndUpConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_021;
            lblParamMixingR1B12ndUpInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_022;
            lblParamMixingR1B12ndUpTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_022;
            lblParamMixingR1B12ndUpAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_023;
            lblParamMixingR1B12ndUpConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_022;

            gbxParamMixingR1B2Down.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_033;
            lblParamMixingR1B2DownInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_018;
            lblParamMixingR1B2DownTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_019;
            lblParamMixingR1B2DownAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_020;
            lblParamMixingR1B2DownConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_021;
            lblParamMixingR1B2DownInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_022;
            lblParamMixingR1B2DownTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_022;
            lblParamMixingR1B2DownAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_023;
            lblParamMixingR1B2DownConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_022;

            gbxParamMixingR1Reverse.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_034;
            lblParamMixingR1ReverseInit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_018;
            lblParamMixingR1ReverseTop.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_019;
            lblParamMixingR1ReverseAccelerator.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_020;
            lblParamMixingR1ReverseConst.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_021;
            lblParamMixingR1ReverseInitUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_022;
            lblParamMixingR1ReverseTopUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_022;
            lblParamMixingR1ReverseAcceleratorUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_023;
            lblParamMixingR1ReverseConstUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_022;

            gbxParamMixingR1Offset.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_025;
            lblParamMixingR1OffsetAPos.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_035;
            lblParamMixingR1OffsetAPosUnit.Text = Properties.Resources_Maintenance.BLANK;

            //MotorAdjustment
            gbxAdjustSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_003;
            SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_028, No=(int)MotorAdjustStopPosition.ReactionTableInitialization},
            };
            lbxAdjustSequence = ComFunc.setSequenceListBox(lbxAdjustSequence, SequenceList);

            gbxAdjustParam.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_007;

            gbxAdjustTableThetaAxis.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_013;
            lblAdjustTableThetaAxisOffsetHomePosition.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_026;
            lblAdjustTableThetaAxisOffsetEncodeThresh.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_036;
            lblAdjustTableThetaAxisOffsetHomePositionUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_027;
            lblAdjustTableThetaAxisOffsetEncodeThreshUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_REACTIONTABLE_027;

            lblAdjustTableThetaAxisPitch.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_PITCH;

            btnAdjustTableThetaAxisCW.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_CW_L;
            btnAdjustTableThetaAxisCCW.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MOTOR_ADJUST_CCW_R;
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
                    StartComm.UnitNo = (int)UnitNoList.ReactionTable;
                    StartComm.FuncNo = FuncNo;

                    switch (FuncNo)
                    {
                        case (int)ReactionTableSequence.Step:
                            StartComm.Arg1 = (int)numtestMoveStep.Value;
                            break;
                    }

                    //レスポンスがある機能の場合のみ、レスポンスのログファイルを作成
                    if (ComFunc.chkExistsResponse(MaintenanceMainNavi.ReactionTableUnit, FuncNo))
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

                    if (ComFunc.chkExistsResponse(MaintenanceMainNavi.ReactionTableUnit, (int)lbxtestSequenceListBox.SelectedValue))
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
        /// パラメータをすべて非活性にする
        /// </summary>
        private void ParametersAllFalse()
        {
            numtestMoveStep.Enabled = false;
        }

        /// <summary>
        /// 選択された機能番号に応じてパラメータを活性化する
        /// </summary>
        private void lbxtestSequenceListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParametersAllFalse();
            switch (lbxtestSequenceListBox.SelectedValue)
            {
                case (int)ReactionTableSequence.Step:
                    numtestMoveStep.Enabled = true;
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

            CarisXConfigParameter.ReactionTableUnitConfigParam ConfigParam = new CarisXConfigParameter.ReactionTableUnitConfigParam();

            //反応テーブル部攪拌時間
            ConfigParam.StirringTime = (double)numconfStrringTime.Value;

            config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableUnitConfigParam = ConfigParam;

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

            //反応テーブル部攪拌時間
            numconfStrringTime.Value = config.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableUnitConfigParam.StirringTime;
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

            //反応テーブル部θ軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.ReactionTableThetaAxis;
            if (!adjustSave)
            {
                Mparam.MotorSpeed[0].InitSpeed = (int)numParamTableThetaInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamTableThetaTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamTableThetaAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamTableThetaConst.Value;

                Mparam.MotorOffset[0] = (double)numParamTableThetaAxisOffsetHomePosition.Value;
                Mparam.MotorOffset[1] = (double)numParamTableThetaAxisOffsetEncodeThresh.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableThetaAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableThetaAxisParam.MotorSpeed = Mparam.MotorSpeed;
            }
            else
            {
                //送信用アイテムにセット
                Mparam.MotorSpeed = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableThetaAxisParam.MotorSpeed;
                Mparam.MotorOffset[0] = (double)numAdjustTableThetaAxisOffsetHomePosition.Value;
                Mparam.MotorOffset[1] = (double)numAdjustTableThetaAxisOffsetEncodeThresh.Value;
            }
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableThetaAxisParam.OffsetHomePosition = Mparam.MotorOffset[0];
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableThetaAxisParam.OffsetEncodeThresh = Mparam.MotorOffset[1];

            //モータパラメータ送信内容スプール
            SpoolParam(Mparam);


            if (!adjustSave)
            {
                //撹拌部　R1撹拌Zθ
                Mparam = new SlaveCommCommand_0471();
                Mparam.MotorNo = (int)MotorNoList.ReactionTableR1MixingZThetaAxis;

                Mparam.MotorSpeed[0].InitSpeed = (int)numParamMixingR1A1stUpInit.Value;
                Mparam.MotorSpeed[0].TopSpeed = (int)numParamMixingR1A1stUpTop.Value;
                Mparam.MotorSpeed[0].Accel = (int)numParamMixingR1A1stUpAccelerator.Value;
                Mparam.MotorSpeed[0].ConstSpeed = (int)numParamMixingR1A1stUpConst.Value;

                Mparam.MotorSpeed[1].InitSpeed = Mparam.MotorSpeed[0].TopSpeed;                 //A Topの値をB1 Initに設定
                Mparam.MotorSpeed[1].TopSpeed = (int)numParamMixingR1B12ndUpTop.Value;
                Mparam.MotorSpeed[1].Accel = (int)numParamMixingR1B12ndUpAccelerator.Value;
                Mparam.MotorSpeed[1].ConstSpeed = (int)numParamMixingR1B12ndUpConst.Value;

                Mparam.MotorSpeed[2].InitSpeed = (int)numParamMixingR1B2DownInit.Value;
                Mparam.MotorSpeed[2].TopSpeed = Mparam.MotorSpeed[1].TopSpeed;                  //B1 TopをB2 Topに設定
                Mparam.MotorSpeed[2].Accel = (int)numParamMixingR1B2DownAccelerator.Value;
                Mparam.MotorSpeed[2].ConstSpeed = (int)numParamMixingR1B2DownConst.Value;

                Mparam.MotorSpeed[3].InitSpeed = (int)numParamMixingR1ReverseInit.Value;
                Mparam.MotorSpeed[3].TopSpeed = (int)numParamMixingR1ReverseTop.Value;
                Mparam.MotorSpeed[3].Accel = (int)numParamMixingR1ReverseAccelerator.Value;
                Mparam.MotorSpeed[3].ConstSpeed = (int)numParamMixingR1ReverseConst.Value;

                Mparam.MotorOffset[0] = (double)numParamMixingR1OffsetAPos.Value;

                //パラメータセット
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableR1MixingZThetaAxisParam.MotorNo = Mparam.MotorNo;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableR1MixingZThetaAxisParam.MotorSpeed = Mparam.MotorSpeed;
                motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableR1MixingZThetaAxisParam.OffsetAPos = Mparam.MotorOffset[0];

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

            //反応テーブル部θ軸
            numParamTableThetaInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableThetaAxisParam.MotorSpeed[0].InitSpeed;
            numParamTableThetaTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableThetaAxisParam.MotorSpeed[0].TopSpeed;
            numParamTableThetaAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableThetaAxisParam.MotorSpeed[0].Accel;
            numParamTableThetaConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableThetaAxisParam.MotorSpeed[0].ConstSpeed;
            //オフセット
            numParamTableThetaAxisOffsetHomePosition.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableThetaAxisParam.OffsetHomePosition;
            numParamTableThetaAxisOffsetEncodeThresh.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableThetaAxisParam.OffsetEncodeThresh;
            //オフセット（モーター調整用）
            numAdjustTableThetaAxisOffsetHomePosition.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableThetaAxisParam.OffsetHomePosition;
            numAdjustTableThetaAxisOffsetEncodeThresh.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableThetaAxisParam.OffsetEncodeThresh;

            //撹拌部　R1撹拌Zθ
            numParamMixingR1A1stUpInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableR1MixingZThetaAxisParam.MotorSpeed[0].InitSpeed;
            numParamMixingR1A1stUpTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableR1MixingZThetaAxisParam.MotorSpeed[0].TopSpeed;
            numParamMixingR1A1stUpAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableR1MixingZThetaAxisParam.MotorSpeed[0].Accel;
            numParamMixingR1A1stUpConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableR1MixingZThetaAxisParam.MotorSpeed[0].ConstSpeed;

            numParamMixingR1B12ndUpInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableR1MixingZThetaAxisParam.MotorSpeed[1].InitSpeed;
            numParamMixingR1B12ndUpTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableR1MixingZThetaAxisParam.MotorSpeed[1].TopSpeed;
            numParamMixingR1B12ndUpAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableR1MixingZThetaAxisParam.MotorSpeed[1].Accel;
            numParamMixingR1B12ndUpConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableR1MixingZThetaAxisParam.MotorSpeed[1].ConstSpeed;

            numParamMixingR1B2DownInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableR1MixingZThetaAxisParam.MotorSpeed[2].InitSpeed;
            numParamMixingR1B2DownTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableR1MixingZThetaAxisParam.MotorSpeed[2].TopSpeed;
            numParamMixingR1B2DownAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableR1MixingZThetaAxisParam.MotorSpeed[2].Accel;
            numParamMixingR1B2DownConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableR1MixingZThetaAxisParam.MotorSpeed[2].ConstSpeed;

            numParamMixingR1ReverseInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableR1MixingZThetaAxisParam.MotorSpeed[3].InitSpeed;
            numParamMixingR1ReverseTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableR1MixingZThetaAxisParam.MotorSpeed[3].TopSpeed;
            numParamMixingR1ReverseAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableR1MixingZThetaAxisParam.MotorSpeed[3].Accel;
            numParamMixingR1ReverseConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableR1MixingZThetaAxisParam.MotorSpeed[3].ConstSpeed;
            //オフセット
            numParamMixingR1OffsetAPos.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableR1MixingZThetaAxisParam.OffsetAPos;

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
            gbxAdjustTableThetaAxis.Enabled = false;
            numAdjustTableThetaAxisOffsetHomePosition.Enabled = false;
            numAdjustTableThetaAxisOffsetEncodeThresh.Enabled = false;

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
            btnAdjustTableThetaAxisCW.Enabled = enable;
            btnAdjustTableThetaAxisCCW.Enabled = enable;
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
                case (int)MotorAdjustStopPosition.ReactionTableInitialization:
                    gbxAdjustTableThetaAxis.Enabled = true;
                    numAdjustTableThetaAxisOffsetHomePosition.Enabled = true;
                    numAdjustTableThetaAxisOffsetHomePosition.ForeColor = System.Drawing.Color.OrangeRed;

                    break;
            }
        }

        #region Pitch値の調整
        private void btnAdjustTableThetaAxisPitchUp_Click(object sender, EventArgs e)
        {
            numAdjustTableThetaAxisPitch.Value = (double)numAdjustTableThetaAxisPitch.Value + (double)numAdjustTableThetaAxisPitch.SpinIncrement;
        }

        private void btnAdjustTableThetaAxisPitchDown_Click(object sender, EventArgs e)
        {
            numAdjustTableThetaAxisPitch.Value = (double)numAdjustTableThetaAxisPitch.Value - (double)numAdjustTableThetaAxisPitch.SpinIncrement;
        }
        #endregion

        private void btnAdjustTableThetaAxisCWCCW_Click(object sender, EventArgs e)
        {
            Boolean upFlg = ((Button)sender == btnAdjustTableThetaAxisCCW);
            Int32 motorNo = (int)MotorNoList.ReactionTableThetaAxis;
            Double pitchValue = (double)numAdjustTableThetaAxisPitch.Value;

            AdjustValue(upFlg, motorNo, numAdjustTableThetaAxisOffsetHomePosition, pitchValue);
        }
    }
}
