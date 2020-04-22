using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinEditors;
using Oelco.CarisX.Const;
using Oelco.CarisX.DB;
using Oelco.CarisX.Parameter;
using Oelco.Common.Parameter;
using Oelco.Common.Utility;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Common;

namespace Oelco.CarisX.GUI.Controls
{
    /// <summary>
    /// サンプルチップ＆セル表示コントロール
    /// </summary>
    public partial class TipCellView : UserControl
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TipCellView()
        {
            InitializeComponent();
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 試薬種別名
        /// </summary>
        public String ReagentName
        {
            get
            {
                return this.lblReagentName.Text;
            }
            set
            {
                this.lblReagentName.Text = value;
            }
        }

        /// <summary>
        /// 残数
        /// </summary>
        public Int32 Value
        {
            get
            {
                return this.prgRemain.Value;
            }
            set
            {
                this.lblRemain.Text = value.ToString();
                this.prgRemain.Value = value;
            }
        }

        /// <summary>
        /// 残数単位
        /// </summary>
        public String RemainUnit
        {
            get
            {
                return this.lblRemainUnit.Text;
            }
            set
            {
                this.lblRemainUnit.Text = value;
            }
        }

        /// <summary>
        /// 使用チップケース
        /// </summary>
        public String UseTipPos
        {
            set
            {
                this.lblUse.Text = value;
            }
        }

        /// <summary>
        /// 現在、使用チップケーズがあるかどうかを取得、設定
        /// </summary>
        public Boolean IsSelected
        {
            get
            {
                return this.pbxUseTipCell.Visible;
            }
            set
            {
                this.pbxUseTipCell.Visible = value;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コントロールを初期化
        /// </summary>
        public void Initialize()
        {
            //プログレスバーの最大値を設定
            this.prgRemain.Maximum = CarisXConst.REMAIN_SAMPLINGTIP_CELL * CarisXConst.REMAIN_SAMPLINGTIP_CELL_CASE;

            //使用する画像イメージの設定を取得
            Singleton<ReagentRemainStatusInfo>.Instance.SamplingTipTotal
                .ForEach(samplingTip => this.prgRemain.BarImageSetting.AddImageRangePair(samplingTip.Remain, samplingTip.Status.ToImage()));
        }

        /// <summary>
        /// 残量を取得・設定
        /// </summary>
        public void setTipCellRemain(Int32 moduleId)
        {
            // プログレスバーに取得した残数を設定
            this.Value = Singleton<ReagentDB>.Instance.GetData(moduleId: moduleId)
                .Where((reagentDataItem) => reagentDataItem.ReagentKind == (Int32)ReagentKind.SamplingTip).Sum((data) => data.Remain ?? 0);

            // プログレスバーに取得した使用チップケースアクト番号を取得
            var temp = Singleton<ReagentDB>.Instance.GetData(moduleId: moduleId)
                .Where(( reagentDataItem ) => reagentDataItem.ReagentKind == (Int32)ReagentKind.SamplingTip
                && (bool)reagentDataItem.IsUse).Select(data => (Int32)data.PortNo);

            // リストが存在しかつ、使用チップケースが一つだけの場合
            if(temp != null && temp.Count() > 0 )
            {
                // 使用チップケース有にする
                this.IsSelected = true;

                // 使用チップケースのアクト番号を設定
                this.UseTipPos = temp.FirstOrDefault().ToString();
            }
            else
            {
                // チップケース番号を空にする
                this.UseTipPos = String.Empty;

                // 使用チップケース無しにする
                this.IsSelected = false;
            }
        }
        #endregion

    }
}