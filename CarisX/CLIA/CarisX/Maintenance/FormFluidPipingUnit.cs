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
    /// 流体配管部
    /// </summary>
    public partial class FormFluidPipingUnit : FormUnitBase
    {
        IMaintenanceUnitStart unitStart = new UnitStart();
        volatile bool ResponseFlg;
        public CommonFunction ComFunc = new CommonFunction();

        public FormFluidPipingUnit()
        {
            InitializeComponent();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            setCulture();
            ComFunc.SetControlSettings(this);
            ConfigTabUseFlg = tabUnit.Tabs[(int)MaintenanceTabIndex.Config].Enabled;    //Configタブを利用有無を退避

            lbxtestSequenceListBox.SelectedIndex = 0;
        }

        #region リソース設定
        private void setCulture()
        {
            //Tab
            tabUnit.Tabs[0].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_FLUIDPIPING_013;                 //Test
            tabUnit.Tabs[1].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_FLUIDPIPING_014;                 //Configuration
            tabUnit.Tabs[2].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_FLUIDPIPING_015;                 //Motor Parameters

            //Testタブ
            gbxtestSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_FLUIDPIPING_000;                 //Sequence
            object SequenceList = new SequenceItem[]
             {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_FLUIDPIPING_001, No=(int)FluidPipingSequence.WasteWaterPumpON},               //1: Waste Water Disposal Pump ON
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_FLUIDPIPING_002, No=(int)FluidPipingSequence.WasteWaterPumpOFF},              //2: Waste Water Disposal Pump OFF
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_FLUIDPIPING_003, No=(int)FluidPipingSequence.WasteLiquidEnforcementEffluent}, //3: Waste Liquid Buffer Enforcement Effluent
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_FLUIDPIPING_004, No=(int)FluidPipingSequence.WasteLiquidReactionCell},        //4: M Reagent Dispense and Probe Washing
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_FLUIDPIPING_005, No=(int)FluidPipingSequence.WashSolutionTankPumpON},         //5: Wash Solution Tank Pump ON
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_FLUIDPIPING_010, No=(int)FluidPipingSequence.WashSolutionTankPumpOFF},        //6: Wash Solution Tank Pump OFF
             };
            lbxtestSequenceListBox = ComFunc.setSequenceListBox(lbxtestSequenceListBox, SequenceList);

            gbxtestParameters.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_FLUIDPIPING_006;                 //Parameters
            lbltestRepeatFrequency.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_FLUIDPIPING_007;            //Repeat Frequency
            lbltestNumber.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_FLUIDPIPING_008;                     //Number
            lbltestRepeatInterval.Text = Resources_Maintenance.STRING_MAINTENANCE_FLUIDPIPING_011;
            lbltestRepeatIntervalUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_FLUIDPIPING_012;

            gbxtestResponce.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_FLUIDPIPING_009;                   //Responce

            //Configurationタブ

            //MotorParameterタブ

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
            Func<object> getfuncno = () => { return lbxtestSequenceListBox.SelectedValue; };
            int FuncNo = (int)Invoke(getfuncno);

            SlaveCommCommand_0439 StartComm = new SlaveCommCommand_0439();
            StartComm.UnitNo = (int)UnitNoList.FluidPiping;
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
    }
}
