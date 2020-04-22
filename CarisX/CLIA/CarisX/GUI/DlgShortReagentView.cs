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
    /// 試薬不足情報表示ダイアログ
    /// </summary>
    public partial class DlgShortReagentView : DlgCarisXBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="remark"></param>
        private DlgShortReagentView( IEnumerable<String> viewList )
        {
            InitializeComponent();

            this.lvwRemarkDetail.Items.AddRange( viewList.Select( ( name ) => new UltraListViewItem( name )
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
        public static void Show( IEnumerable<String> viewList )
        {
            using ( var dlg = new DlgShortReagentView( viewList ) )
            {
                dlg.ShowDialog();
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
            this.Caption = CarisX.Properties.Resources.STRING_DLG_TITLE_004;

            // ボタン
            this.btnConfirm.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
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