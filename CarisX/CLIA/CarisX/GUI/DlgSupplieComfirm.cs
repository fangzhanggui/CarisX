using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 消耗品警告画面クラス
    /// </summary>
    public partial class DlgSupplieComfirm : DlgCarisXBaseSys
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgSupplieComfirm( )
        {
            InitializeComponent();            
        } 
        #endregion

        #region [publicメソッド]
        /// <summary>
        /// 消耗品リスト設定
        /// </summary>
        /// <remarks>
        /// 消耗品リストを設定します
        /// </remarks>
        public void setPartsList(List<String> list)
        {
            foreach ( String name in list )
            {
                this.listPartsName.Items.Add( name ); 
            }
        }
        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            // ダイアログタイトル
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_SUPPLIECOMFIRM_000;
            // メッセージラベル            
            this.lblMessage.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SUPPLIECOMFIRM_001;            
            // ボタン
            this.btnOK.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;           
        }

        #endregion

        #region [privateメソッド]
        
        /// <summary>
        /// OKボタン押下時処理
        /// </summary>
        /// <remarks>
        /// 画面を終了します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click( object sender, EventArgs e )
        {
            // 画面を閉じる。
            this.Close();
        }
        
        #endregion
    }
}
