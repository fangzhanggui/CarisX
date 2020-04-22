using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.GUI.Controls
{
    /// <summary>
    /// 精度管理検体測定データ画面用再計算パネル
    /// </summary>
    public partial class RecalcInfoPanelControlResult : RecalcInfoPanelBase, IRecalcInfoControlResult
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RecalcInfoPanelControlResult()
        {
            InitializeComponent();

            this.chkControlLot.Text = Oelco.CarisX.Properties.Resources.STRING_RECALCINFOPANELCONTROLRESULT_000;
            this.chkControlName.Text = Oelco.CarisX.Properties.Resources.STRING_RECALCINFOPANELCONTROLRESULT_001;

            // コントロールラックID接頭辞
            this.lblRackIdPrefix1.Text = CarisXConst.CONTROL_RACK_ID_PRECHAR;
            this.lblRackIdPrefix2.Text = CarisXConst.CONTROL_RACK_ID_PRECHAR;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 選択中の精度管理検体ロットの取得、設定
        /// </summary>
        public Tuple<Boolean, String> ControlLotNoSelect
        {
            get
            {
                return new Tuple<Boolean, String>( this.chkControlLot.Checked, this.txtControlLot.Text );
            }
            set
            {
                this.chkControlLot.Checked=value.Item1;
                this.txtControlLot.Text = value.Item2;
            }
        }

        /// <summary>
        /// 選択中の精度管理検体名の取得、設定
        /// </summary>
        public Tuple<Boolean, String> ControlNameSelect
        {
            get
            {
                return new Tuple<Boolean, String>( this.chkControlName.Checked, this.txtControlName.Text );
            }
            set
            {
                this.chkControlName.Checked = value.Item1;
                this.txtControlName.Text = value.Item2;
            }
        }
        
        #endregion


    }
}
