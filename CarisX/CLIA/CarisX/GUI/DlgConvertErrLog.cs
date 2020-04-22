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
    /// インポートエラーダイアログクラス
    /// </summary>
    public partial class DlgConvertErrLog : DlgCarisXBaseSys
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgConvertErrLog()
        {
            InitializeComponent();            
        } 
        #endregion

        #region [publicメソッド]
        /// <summary>
        /// エラーリスト設定
        /// </summary>
        /// <remarks>
        /// エラーリストを設定します
        /// </remarks>
        /// <param name="list"></param>
        public void setErrorList( List<String> list )
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_CONVERT_ERROR_LOG_000;
            // Listの列タイトル            
            this.lblColTitle1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_CONVERT_ERROR_LOG_001;
            this.lblColTitle2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_CONVERT_ERROR_LOG_002;
            this.lblColTitle3.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_CONVERT_ERROR_LOG_003;
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
