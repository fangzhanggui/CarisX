using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Oelco.CarisX.Utility;
using Infragistics.Win.UltraWinListView;
using System.Linq;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// リマーク詳細ダイアログクラス
    /// </summary>
    public partial class DlgRemarkDetail : DlgCarisXBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="remark"></param>
        private DlgRemarkDetail( Remark remark )
        {
            InitializeComponent();

            this.lvwRemarkDetail.Items.AddRange( remark.GetRemarkNameStrings().Select( ( name ) => new UltraListViewItem( name )
            {
                Value = name
            } ).ToArray() );
        }

        /// <summary>
        /// 画面表示
        /// </summary>
        /// <remarks>
        /// 画面表示します
        /// </remarks>
        /// <param name="remark"></param>
        public static void Show( Remark remark )
        {
            if ( remark != null && remark != Remark.REMARK_DEFAULT )
            {
				// RemarkのEnum値に定義されていない値が渡されたときは、Remark == 0 と同様の扱い（エラーなし）とする
				if (remark.GetRemarkStrings().Length == 0)
				{
					return;
				}

                new DlgRemarkDetail( remark ).ShowDialog();
            }
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            // タイトル
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_REMARKDETAIL_000;

            // ボタン
            this.btnConfirm.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_004;
        }

        /// <summary>
        /// 確認ボタン押下イベント
        /// </summary>
        /// <remarks>
        /// 画面終了します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click( object sender, EventArgs e )
        {
            this.Close();
        }

    }
}
