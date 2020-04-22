using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Oelco.CarisX.GUI;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Properties;
using Oelco.CarisX.Const;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.Common.Comm;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Utility;

namespace Oelco.CarisX.Maintenance
{
    public partial class FormTempPID : DlgCarisXBase
    {
        private PIDControl ImmediatelyBeforePID = PIDControl.Stop;

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            this.lblDialogTitle.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_000;
            this.lblReactionTable.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_001;
            this.lblBFTable.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_002;
            this.lblBF1PreHeat.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_003;
            this.lblBF2PreHeat.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_004;
            this.lblR1PreHeat.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_005;
            this.lblR2PreHeat.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_006;
            this.lblPtotometry.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_007;

            this.lblReactionTableP.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_008;
            this.lblBFTableP.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_008;
            this.lblBF1PreHeatP.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_008;
            this.lblBF2PreHeatP.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_008;
            this.lblR1PreHeatP.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_008;
            this.lblR2PreHeatP.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_008;
            this.lblPtotometryP.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_008;

            this.lblReactionTableI.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_009;
            this.lblBFTableI.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_009;
            this.lblBF1PreHeatI.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_009;
            this.lblBF2PreHeatI.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_009;
            this.lblR1PreHeatI.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_009;
            this.lblR2PreHeatI.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_009;
            this.lblPtotometryI.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_009;

            this.lblReactionTableD.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_010;
            this.lblBFTableD.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_010;
            this.lblBF1PreHeatD.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_010;
            this.lblBF2PreHeatD.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_010;
            this.lblR1PreHeatD.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_010;
            this.lblR2PreHeatD.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_010;
            this.lblPtotometryD.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_010;

            this.lblRemark.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_011;

            this.btnPIDON.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_012;
            this.btnPIDOFF.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_013;

            this.btnSAVE.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_014;
            this.btnCLOSE.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_015;

        }

        public FormTempPID()
        {
            InitializeComponent();
            TempPIDParamLoad();
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.TempPIDResponse, RecvTempPID);
        }

        /// <summary>
        /// コンフィグパラメータ読み込み
        /// </summary>
        private void TempPIDParamLoad()
        {
            ParameterFilePreserve<TempPIDParameter> TempPID = Singleton<ParameterFilePreserve<TempPIDParameter>>.Instance;
            TempPID.LoadRaw();

            //反応テーブル温度
            numReactionTableP.Value = TempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableTempPIDParam.ProportionalConstValue;
            numReactionTableI.Value = TempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableTempPIDParam.IntegralConstvalue;
            numReactionTableD.Value = TempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableTempPIDParam.DifferentialConstValue;

            //BFテーブル温度
            numBFTableP.Value = TempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableTempPIDParam.ProportionalConstValue;
            numBFTableI.Value = TempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableTempPIDParam.IntegralConstvalue;
            numBFTableD.Value = TempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableTempPIDParam.DifferentialConstValue;

            //B/F1温度
            numBF1PreHeatP.Value = TempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1TempPIDParam.ProportionalConstValue;
            numBF1PreHeatI.Value = TempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1TempPIDParam.IntegralConstvalue;
            numBF1PreHeatD.Value = TempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1TempPIDParam.DifferentialConstValue;

            //B/F2温度
            numBF2PreHeatP.Value = TempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2TempPIDParam.ProportionalConstValue;
            numBF2PreHeatI.Value = TempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2TempPIDParam.IntegralConstvalue;
            numBF2PreHeatD.Value = TempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2TempPIDParam.DifferentialConstValue;

            //R1温度
            numR1PreHeatP.Value = TempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1TempPIDParam.ProportionalConstValue;
            numR1PreHeatI.Value = TempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1TempPIDParam.IntegralConstvalue;
            numR1PreHeatD.Value = TempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1TempPIDParam.DifferentialConstValue;

            //R2温度
            numR2PreHeatP.Value = TempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2TempPIDParam.ProportionalConstValue;
            numR2PreHeatI.Value = TempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2TempPIDParam.IntegralConstvalue;
            numR2PreHeatD.Value = TempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2TempPIDParam.DifferentialConstValue;

            //化学発光測定部温度
            numPtotometryP.Value = TempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].ptotometryTempPIDParam.ProportionalConstValue;
            numPtotometryI.Value = TempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].ptotometryTempPIDParam.IntegralConstvalue;
            numPtotometryD.Value = TempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].ptotometryTempPIDParam.DifferentialConstValue;
        }

        private void btnPIDON_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            // PID制御開始コマンド（すべて）
            SlaveCommCommand_0472 cmd0472 = new SlaveCommCommand_0472();
            cmd0472.Control = PIDControl.Start;
            cmd0472.TempArea = PIDTempArea.All;
            //送信内容スプール
            SpoolParam(cmd0472);

            ImmediatelyBeforePID = PIDControl.Start;

            //送信
            SendParam();
        }

        private void btnPIDOFF_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            // PID制御開始コマンド（すべて）
            SlaveCommCommand_0472 cmd0472 = new SlaveCommCommand_0472();
            cmd0472.Control = PIDControl.Stop;
            cmd0472.TempArea = PIDTempArea.All;
            //送信内容スプール
            SpoolParam(cmd0472);

            ImmediatelyBeforePID = PIDControl.Stop;

            //送信
            SendParam();
        }

        private void btnSAVE_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            SlaveCommCommand_0474 cmd0474;

            ParameterFilePreserve<TempPIDParameter> tempPID = Singleton<ParameterFilePreserve<TempPIDParameter>>.Instance;
            tempPID.LoadRaw();

            //反応テーブル
            cmd0474 = new SlaveCommCommand_0474();
            cmd0474.ProportionalConstValue = (double)numReactionTableP.Value;
            cmd0474.IntegralConstvalue = (double)numReactionTableI.Value;
            cmd0474.DifferentialConstValue = (double)numReactionTableD.Value;
            cmd0474.TempArea = PIDTempArea.ReactionTableTemp;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableTempPIDParam.ProportionalConstValue = cmd0474.ProportionalConstValue;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableTempPIDParam.IntegralConstvalue = cmd0474.IntegralConstvalue;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableTempPIDParam.DifferentialConstValue = cmd0474.DifferentialConstValue;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableTempPIDParam.TempArea = cmd0474.TempArea;
            //送信内容スプール
            SpoolParam(cmd0474);

            //BFテーブル
            cmd0474 = new SlaveCommCommand_0474();
            cmd0474.ProportionalConstValue = (double)numBFTableP.Value;
            cmd0474.IntegralConstvalue = (double)numBFTableI.Value;
            cmd0474.DifferentialConstValue = (double)numBFTableD.Value;
            cmd0474.TempArea = PIDTempArea.BFTableTemp;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableTempPIDParam.ProportionalConstValue = cmd0474.ProportionalConstValue;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableTempPIDParam.IntegralConstvalue = cmd0474.IntegralConstvalue;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableTempPIDParam.DifferentialConstValue = cmd0474.DifferentialConstValue;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableTempPIDParam.TempArea = cmd0474.TempArea;
            //送信内容スプール
            SpoolParam(cmd0474);

            //BF1
            cmd0474 = new SlaveCommCommand_0474();
            cmd0474.ProportionalConstValue = (double)numBF1PreHeatP.Value;
            cmd0474.IntegralConstvalue = (double)numBF1PreHeatI.Value;
            cmd0474.DifferentialConstValue = (double)numBF1PreHeatD.Value;
            cmd0474.TempArea = PIDTempArea.BF1PreheatTemp;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1TempPIDParam.ProportionalConstValue = cmd0474.ProportionalConstValue;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1TempPIDParam.IntegralConstvalue = cmd0474.IntegralConstvalue;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1TempPIDParam.DifferentialConstValue = cmd0474.DifferentialConstValue;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1TempPIDParam.TempArea = cmd0474.TempArea;
            //送信内容スプール
            SpoolParam(cmd0474);

            //BF2
            cmd0474 = new SlaveCommCommand_0474();
            cmd0474.ProportionalConstValue = (double)numBF2PreHeatP.Value;
            cmd0474.IntegralConstvalue = (double)numBF2PreHeatI.Value;
            cmd0474.DifferentialConstValue = (double)numBF2PreHeatD.Value;
            cmd0474.TempArea = PIDTempArea.BF2PreheatTemp;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2TempPIDParam.ProportionalConstValue = cmd0474.ProportionalConstValue;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2TempPIDParam.IntegralConstvalue = cmd0474.IntegralConstvalue;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2TempPIDParam.DifferentialConstValue = cmd0474.DifferentialConstValue;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2TempPIDParam.TempArea = cmd0474.TempArea;
            //送信内容スプール
            SpoolParam(cmd0474);

            //R1
            cmd0474 = new SlaveCommCommand_0474();
            cmd0474.ProportionalConstValue = (double)numR1PreHeatP.Value;
            cmd0474.IntegralConstvalue = (double)numR1PreHeatI.Value;
            cmd0474.DifferentialConstValue = (double)numR1PreHeatD.Value;
            cmd0474.TempArea = PIDTempArea.R1PreheatTemp;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1TempPIDParam.ProportionalConstValue = cmd0474.ProportionalConstValue;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1TempPIDParam.IntegralConstvalue = cmd0474.IntegralConstvalue;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1TempPIDParam.DifferentialConstValue = cmd0474.DifferentialConstValue;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1TempPIDParam.TempArea = cmd0474.TempArea;
            //送信内容スプール
            SpoolParam(cmd0474);

            //R2
            cmd0474 = new SlaveCommCommand_0474();
            cmd0474.ProportionalConstValue = (double)numR2PreHeatP.Value;
            cmd0474.IntegralConstvalue = (double)numR2PreHeatI.Value;
            cmd0474.DifferentialConstValue = (double)numR2PreHeatD.Value;
            cmd0474.TempArea = PIDTempArea.R2PreheatTemp;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2TempPIDParam.ProportionalConstValue = cmd0474.ProportionalConstValue;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2TempPIDParam.IntegralConstvalue = cmd0474.IntegralConstvalue;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2TempPIDParam.DifferentialConstValue = cmd0474.DifferentialConstValue;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2TempPIDParam.TempArea = cmd0474.TempArea;
            //送信内容スプール
            SpoolParam(cmd0474);

            //化学発光測定部温度
            cmd0474 = new SlaveCommCommand_0474();
            cmd0474.ProportionalConstValue = (double)numPtotometryP.Value;
            cmd0474.IntegralConstvalue = (double)numPtotometryI.Value;
            cmd0474.DifferentialConstValue = (double)numPtotometryD.Value;
            cmd0474.TempArea = PIDTempArea.PtotometryTemp;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].ptotometryTempPIDParam.ProportionalConstValue = cmd0474.ProportionalConstValue;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].ptotometryTempPIDParam.IntegralConstvalue = cmd0474.IntegralConstvalue;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].ptotometryTempPIDParam.DifferentialConstValue = cmd0474.DifferentialConstValue;
            tempPID.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].ptotometryTempPIDParam.TempArea = cmd0474.TempArea;
            //送信内容スプール
            SpoolParam(cmd0474);

            //送信
            SendParam();

        }

        /// <summary>
        /// PID保存コマンドに対するレスポンス受信時の処理
        /// </summary>
        /// <param name="Comm"></param>
        private void RecvTempPID(object Comm)
        {
            if (Comm != null)
            {
                //レスポンスがあった場合
                CarisXCommCommand carisXCommCommand = (CarisXCommCommand)Comm;
                switch (carisXCommCommand.CommandId)
                {
                    case (int)CommandKind.Command1474:
                        //コマンドのやり取りが正しく終了
                        ParameterFilePreserve<TempPIDParameter> tempPID = Singleton<ParameterFilePreserve<TempPIDParameter>>.Instance;
                        tempPID.SaveRaw();

                        MessageBox.Show("PID SAVE");
                        break;
                    case (int)CommandKind.Command1472:
                        //コマンドのやり取りが正しく終了
                        if (ImmediatelyBeforePID == PIDControl.Start)
                        {
                            MessageBox.Show("PID ON");
                        }
                        else
                        {
                            MessageBox.Show("PID OFF");
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

            this.Enabled = true;
        }

        private void btnCLOSE_Click(object sender, EventArgs e)
        {
            Singleton<NotifyManager>.Instance.RemoveNotifyTarget((Int32)NotifyKind.TempPIDResponse, RecvTempPID);
            this.Close();
        }

        public List<CarisXCommCommand> sendParamList = new List<CarisXCommCommand>();

        /// <summary>
        /// 送信するパラメータコマンドを貯めておきます。
        /// </summary>
        protected void SpoolParam(CarisXCommCommand param)
        {
            sendParamList.Add(param);
        }

        /// <summary>
        /// パラメータコマンドを送信します。
        /// </summary>
        protected void SendParam()
        {
            List<CarisXCommCommand> temp = new List<CarisXCommCommand>();
            temp.AddRange(sendParamList);

            // データ送信
            Singleton<CarisXSequenceHelperManager>.Instance.Maintenance.MaintenanceSetPIDSequence(temp);

            // リストクリア
            sendParamList.Clear();
        }
    }
}
