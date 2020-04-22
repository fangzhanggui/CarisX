using Oelco.CarisX.Const;
using Oelco.CarisX.Maintenance;
using Oelco.CarisX.Parameter;
using Oelco.Common.GUI;
using Oelco.Common.Utility;
using System;
using System.Windows.Forms;
using Oelco.CarisX.Utility;
using Oelco.Common.Parameter;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 分析項目の詳細指定ダイアログ
    /// </summary>
    public partial class DlgEditRegistInfo : Oelco.CarisX.GUI.DlgCarisXBaseSys
    {
        CommonFunction ComFunc = new CommonFunction();
        Boolean debugControl = false;

        public DlgEditRegistInfo(Int32 ProtoNo, Int32 DilutionRatio, Int32 MeasTimes)
        {
            
            debugControl = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.DebugControlVisibled); ;
            InitializeComponent();

            //プロトコル情報の取得
            MeasureProtocol measureProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(ProtoNo);

            //分析項目名の設定
            this.Caption = measureProtocol.ProtocolName;

            // スレーブの急診モード使用フラグ
            bool enabledFlag = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.IsProtocolEnabledChangedInEmergencyMode();

            // 試薬、スレーブ両方で急診の使用がありの場合
            if ((measureProtocol.UseEmergencyMode == true) && (enabledFlag == false))
            {
                // 希釈倍率変更グループボックスを非活性にする
                gbxAutoDilutionRatio.Enabled = false;
            }
            // 試薬の急診使用がなしの場合、または、全スレーブで急診の仕様がなしになっている場合
            else
            {
                // 希釈倍率変更グループボックスを活性にする
                gbxAutoDilutionRatio.Enabled = true;
            }
            
            //画面に表示する希釈倍率の内容を制御
            foreach (CustomURadioButton rbt in gbxAutoDilutionRatio.Controls)
            {
                if (((Int32)measureProtocol.ProtocolDilutionRatio * int.Parse(rbt.Tag.ToString())) > CarisXConst.MaxDILUTION)
                {
                    rbt.Visible = false;
                }
            }
            //既に設定されていた希釈倍率が、画面に表示できる希釈倍率を超えている場合は、最大の希釈倍率を設定する
            this.DilutionRatio = DilutionRatio;
            if (((Int32)measureProtocol.ProtocolDilutionRatio * DilutionRatio) > CarisXConst.MaxDILUTION)
            {
                rbtDilutionRatioX1.Checked = true;
            }

            this.MeasTimes = MeasTimes;
        }

        #region [プロパティ]
        public Int32 DilutionRatio
        {
            get
            {
                return ComFunc.getSelectedCustomURadioButtonValue(gbxAutoDilutionRatio);
            }
            set
            {
                ComFunc.setSelectedCustomURadioButtonCheck(gbxAutoDilutionRatio, value);
            }
        }

        public Int32 MeasTimes
        {
            get
            {
                // ユーザレベル5の場合
                if (debugControl)
                {
                    return (int)this.numSpecimenMultiMeasure.Value; 
                }
                // それ以外のユーザーレベルの場合
                else
                {
                    return (int)this.optMeasTimes.Value;
                }
               
            }
            set
            {
                // ユーザレベル5の場合
                if (debugControl)
                {
                    this.numSpecimenMultiMeasure.Value = value;
                }
                // それ以外のユーザーレベルの場合
                else
                {
                    this.optMeasTimes.Value = value;
                }
            }
        }
        #endregion

        protected override void initializeFormComponent()
        {
            // ユーザレベル5の場合
            if (debugControl)
            {
                this.numSpecimenMultiMeasure.Visible = true;
                this.numSpecimenMultiMeasure.Enabled = true;
                this.optMeasTimes.Enabled = false;
            }
            // それ以外のユーザーレベルの場合
            else
            {
                this.numSpecimenMultiMeasure.Visible = false;
                this.numSpecimenMultiMeasure.Enabled = false;
                this.optMeasTimes.Enabled = true;
            }
        }

        protected override void setCulture()
        {
            this.gbxAutoDilutionRatio.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_EDITREGISTINFO_000;
            this.rbtDilutionRatioX1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_EDITREGISTINFO_001;
            this.rbtDilutionRatioX10.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_EDITREGISTINFO_003;
            this.rbtDilutionRatioX20.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_EDITREGISTINFO_005;
            this.rbtDilutionRatioX100.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_EDITREGISTINFO_007;
            this.rbtDilutionRatioX200.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_EDITREGISTINFO_009;
            this.rbtDilutionRatioX400.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_EDITREGISTINFO_002;
            this.rbtDilutionRatioX1000.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_EDITREGISTINFO_004;
            this.rbtDilutionRatioX2000.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_EDITREGISTINFO_006;
            this.rbtDilutionRatioX4000.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_EDITREGISTINFO_008;
            this.rbtDilutionRatioX8000.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_EDITREGISTINFO_010;

            this.gbxMeasTimes.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_EDITREGISTINFO_011;
            this.optMeasTimes.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_EDITREGISTINFO_012;
            this.optMeasTimes.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_EDITREGISTINFO_013;
            this.optMeasTimes.Items[2].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_EDITREGISTINFO_014;

            this.btnOK.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_020;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_021;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
