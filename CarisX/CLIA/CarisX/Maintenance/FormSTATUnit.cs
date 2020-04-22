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
    /// スタット部
    /// </summary>
    public partial class FormSTATUnit : FormUnitBase
    {
        IMaintenanceUnitStart unitStart = new UnitStart();
        IMaintenanceUnitStart unitStartAdjust = new UnitStarAdjust();
        IMaintenanceUnitStart unitAdjust = new UnitAdjust();
        IMaintenanceUnitStart unitAdjustAbort = new UnitStarAdjustAbort();
        volatile bool ResponseFlg;
        public CommonFunction ComFunc = new CommonFunction();

        public FormSTATUnit()
        {
            InitializeComponent();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ConfigParamLoad();
            MotorParamDisp();
            setCulture();
            ComFunc.SetControlSettings(this);
            ConfigTabUseFlg = tabUnit.Tabs[(int)MaintenanceTabIndex.Config].Enabled;    //Configタブを利用有無を退避

            lbxtestSequenceListBox.SelectedIndex = 0;
        }

        #region [リソース設定]
        private void setCulture()
        {
            //Tab
            tabUnit.Tabs[0].Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_008;           //Test
            tabUnit.Tabs[1].Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_009;           //Configuration
            tabUnit.Tabs[2].Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_010;           //Motor Parameters

            //Testタブ
            gbxtestSequence.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_000;           //Sequence
            object SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_STAT_001, No=(int)ReagentStorageSequence.Init},                  //1: Unit Initialization
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_STAT_002, No=(int)ReagentStorageSequence.ReagentTableBottle},    //2: Reagent Table Turn to the Bottle Setting Position
                new SequenceItem{Name=Resources_Maintenance.STRING_MAINTENANCE_STAT_003, No=(int)ReagentStorageSequence.ReagentTableR1R1},      //3: Reagent Table Turn to the R1Aspiration Position(R1 Dispense Unit)
            };
            lbxtestSequenceListBox = ComFunc.setSequenceListBox(lbxtestSequenceListBox, SequenceList);

            gbxtestParameters.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_004;         //Parameters
            lbltestRepeatFrequency.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_005;    //Repeat Frequency
            lbltestNumber.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_006;             //Number
            lbltestRepeatInterval.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_022;
            lbltestRepeatIntervalUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_023;

            gbxtestResponce.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_007;           //Response

            //Configurationタブ

            //MotorParameterタブ
            gbxParamYAxis.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_011;
            gbxParamYAxisCommon.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_016;
            gbxParamYAxisOffset.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_019;

            lblParamYAxisInit.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_012;
            lblParamYAxisTop.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_013;
            lblParamYAxisAccelerator.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_014;
            lblParamYAxisConst.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_015;
            lblParamYAxisOffsetSTATAspiration.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_020;
            lblParamYAxisInitUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_017;
            lblParamYAxisTopUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_017;
            lblParamYAxisAcceleratorUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_018;
            lblParamYAxisConstUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_017;
            lblParamYAxisOffsetSTATAspirationUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_STAT_021;

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
                DlgMaintenance msg = new DlgMaintenance(Resources_Maintenance.DlgSeqChkMsg, false);
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
            StartComm.UnitNo = (int)UnitNoList.STAT;
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
                //レスポンス待ち
            }

            AbortFlg = false;

            //メイン画面のナビゲータバーをEnableコマンドバーをデフォルトに戻す。
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
        /// パラメータ保存
        /// </summary>
        public override void ParamSave()
        {
            if (tabUnit.SelectedTab.Index == 2)
            {
                //モーターパラメータセーブ
                MotorParamSave();
            }
        }

        /// <summary>
        /// モーターパラメータ保存
        /// </summary>
        private void MotorParamSave()
        {
            //入力値チェック
            if (!CheckControls(tabUnit.Tabs[2].TabPage))
            {
                DlgMaintenance msg = new DlgMaintenance(Resources_Maintenance.DlgChkMsg, false);
                msg.ShowDialog();
                return;
            }

            SlaveCommCommand_0471 Mparam;
            ParameterFilePreserve<CarisXMotorParameter> motor = Singleton<ParameterFilePreserve<CarisXMotorParameter>>.Instance;
            motorLoad(motor);

            //スタット部Y軸
            Mparam = new SlaveCommCommand_0471();
            Mparam.MotorNo = (int)MotorNoList.STATYAxis;
            Mparam.MotorSpeed[0].InitSpeed = (int)numParamYAxisInit.Value;
            Mparam.MotorSpeed[0].TopSpeed = (int)numParamYAxisTop.Value;
            Mparam.MotorSpeed[0].Accel = (int)numParamYAxisAccelerator.Value;
            Mparam.MotorSpeed[0].ConstSpeed = (int)numParamYAxisConst.Value;

            Mparam.MotorOffset[0] = (double)numParamYAxisOffsetSTATAspiration.Value;

            //パラメータセット
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sTATYAxisParam.MotorNo = Mparam.MotorNo;
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sTATYAxisParam.MotorSpeed = Mparam.MotorSpeed;
            motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sTATYAxisParam.OffsetSTATSampleAspiration = Mparam.MotorOffset[0];
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
                    case (int)MaintenanceTabIndex.MParam:
                        //モータパラメータ保存
                        ParameterFilePreserve<CarisXMotorParameter> motor = Singleton<ParameterFilePreserve<CarisXMotorParameter>>.Instance;
                        motorSave(motor);
                        MotorParamDisp();
                        try
                        {
                            //関連する他の画面のモーターパラメータの再表示を行う
                            FormMaintenanceMain formMaintenanceMain = (FormMaintenanceMain)Application.OpenForms[typeof(FormMaintenanceMain).Name];
                            formMaintenanceMain.subFormSampleDispense[formMaintenanceMain.moduleIndex].MotorParamDisp();
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

            //スタット部Y軸
            numParamYAxisInit.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sTATYAxisParam.MotorSpeed[0].InitSpeed;
            numParamYAxisTop.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sTATYAxisParam.MotorSpeed[0].TopSpeed;
            numParamYAxisAccelerator.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sTATYAxisParam.MotorSpeed[0].Accel;
            numParamYAxisConst.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sTATYAxisParam.MotorSpeed[0].ConstSpeed;
            numParamYAxisOffsetSTATAspiration.Value = motor.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].sTATYAxisParam.OffsetSTATSampleAspiration;

        }
    }
}
