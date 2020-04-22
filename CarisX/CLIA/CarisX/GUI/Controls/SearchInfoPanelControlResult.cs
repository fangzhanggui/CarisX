using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Oelco.CarisX.GUI.Controls
{
    /// <summary>
    /// 精度管理検体測定データ画面用絞込みパネル
    /// </summary>
    public partial class SearchInfoPanelControlResult : SearchInfoPanelBase, ISearchInfoControlResult
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SearchInfoPanelControlResult()
        {
            InitializeComponent();

            this.chkComment.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_016;
            this.chkControlLot.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_017;
            this.chkControlName.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_018;
        }

        #endregion

        #region [プロパティ]

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

        /// <summary>
        /// 選択中の精度管理検体ロットの取得、設定
        /// </summary>
        public Tuple<Boolean, String> ControlLotSelect
        {
            get
            {
                return new Tuple<Boolean, String>( this.chkControlLot.Checked, this.txtControlLot.Text );
            }
            set
            {
                this.chkControlLot.Checked = value.Item1;
                this.txtControlLot.Text = value.Item2;
            }
        }

        /// <summary>
        /// 選択中のコメントの取得、設定
        /// </summary>
        public Tuple<Boolean, String> CommentSelect
        {
            get
            {
                return new Tuple<Boolean, String>( this.chkComment.Checked, this.txtComment.Text );
            }
            set
            {
                this.chkComment.Checked = value.Item1;
                this.txtComment.Text = value.Item2;
            }
        }

        #endregion

    }
}
