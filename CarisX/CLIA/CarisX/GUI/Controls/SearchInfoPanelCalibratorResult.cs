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
    /// キャリブレータ測定データ画面絞込みパネル
    /// </summary>
    public partial class SearchInfoPanelCalibratorResult : SearchInfoPanelBase, ISearchInfoCalibratorResult
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SearchInfoPanelCalibratorResult()
            : base()
        {
            InitializeComponent();

            // キャリブレータラックID接頭辞
            this.lblRackIdPrefix1.Text = CarisXConst.CALIB_RACK_ID_PRECHAR;
            this.lblRackIdPrefix2.Text = CarisXConst.CALIB_RACK_ID_PRECHAR;

            // キャリブレータロット
            this.chkCalibratorLot.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_012;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 選択中のキャリブレータロットの取得、設定
        /// </summary>
        Tuple<Boolean, String> ISearchInfoCalibratorResult.CalibratorLotSelect
        {
            get
            {
                return new Tuple<Boolean, String>( this.chkCalibratorLot.Checked, this.txtCalibratorLot.Text );
            }
            set
            {
                 this.chkCalibratorLot.Checked= value.Item1;
                 this.txtCalibratorLot.Text = value.Item2;
            }
        }
        
        #endregion

    }
}
