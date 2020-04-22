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
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.CarisX.Common;

namespace Oelco.CarisX.GUI.Controls
{
    /// <summary>
    /// 残量表示コントロール
    /// </summary>
    public partial class RemainBar : UserControl
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RemainBar()
        {
            InitializeComponent();
        }

        #endregion

        #region [プロパティ]

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
                if (value > this.prgRemain.Maximum)
                {
                    //設定したい値がMaximumを超えている場合はログを表示する
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("【調査用デバッグログ】prgRemain.Maximum over remain. Maximum:{0} , remain :{1}", this.prgRemain.Maximum, value));
                }
                this.prgRemain.Value = value;
            }
        }

        /// <summary>
        /// 最大数
        /// </summary>
        public Int32 Maximum
        {
            get
            {
                return this.prgRemain.Maximum;
            }
            set
            {
                this.prgRemain.Maximum = value;
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
            Singleton<ReagentRemainStatusInfo>.Instance.Reagent.ForEach((item) =>
            {
                this.prgRemain.BarImageSetting.AddImageRangePair(item.Remain, item.Status.ToImage());
            });
        }

        #endregion

    }
}