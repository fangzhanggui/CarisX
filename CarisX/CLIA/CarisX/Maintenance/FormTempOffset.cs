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
    /// <summary>
    /// 【IssuesNo:4】温度补偿调整界面
    /// </summary>
    public partial class FormTempOffset : DlgCarisXBase
    {
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

            this.btnSAVE.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_014;
            this.btnCLOSE.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_TEMPPID_015;

        }

        public FormTempOffset()
        {
            InitializeComponent();
        }

        /// <summary>
        /// コンフィグパラメータ読み込み
        /// </summary>
        private void TempPIDParamLoad()
        {
            ParameterFilePreserve<TempOffsetParameter> TempOffset = Singleton<ParameterFilePreserve<TempOffsetParameter>>.Instance;
            TempOffset.LoadRaw();

            //反応テーブル温度
            numReactionTableOffsetTemp.Value = TempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableTempOffsetParam;

            //BFテーブル温度
            numBFTableOffsetTemp.Value = TempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableTempOffsetParam;

            //B/F1温度
            numBF1OffsetTemp.Value = TempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1TempOffsetParam;

            //B/F2温度       
            numBF2OffsetTemp.Value = TempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2TempOffsetParam;

            //R1温度
            numR1OffsetTemp.Value = TempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1TempOffsetParam;

            //R2温度
            numR2OffsetTemp.Value = TempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2TempOffsetParam;

            //化学発光測定部温度
            numChemilumiOffSetTemp.Value = TempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].ptotometryTempOffsetParam;
        }

        private void btnSAVE_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            ParameterFilePreserve<TempOffsetParameter> tempOffset = Singleton<ParameterFilePreserve<TempOffsetParameter>>.Instance;
            tempOffset.LoadRaw();

            //反応テーブル
            tempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableTempOffsetParam = (double)numReactionTableOffsetTemp.Value;

            //BFテーブル
            tempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableTempOffsetParam = (double)numBFTableOffsetTemp.Value;

            //BF1
            tempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1TempOffsetParam = (double)numBF1OffsetTemp.Value;

            //BF2
            tempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2TempOffsetParam = (double)numBF2OffsetTemp.Value;

            //R1
            tempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1TempOffsetParam = (double)numR1OffsetTemp.Value;

            //R2
            tempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2TempOffsetParam = (double)numR2OffsetTemp.Value;

            //化学発光測定部温度
            tempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].ptotometryTempOffsetParam = (double)numChemilumiOffSetTemp.Value;

            tempOffset.SaveRaw();

            MessageBox.Show("Complete!");

            this.Enabled = true;
        }

        private void btnCLOSE_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
