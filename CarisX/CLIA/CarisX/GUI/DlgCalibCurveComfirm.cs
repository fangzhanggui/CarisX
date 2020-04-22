using Oelco.CarisX.Const;
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
    /// 分析項目・ロット警告画面クラス
    /// </summary>
    public partial class DlgCalibCurveConfirm : DlgCarisXBaseSys
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgCalibCurveConfirm()
        {
            InitializeComponent();            
        } 
        #endregion

        #region [publicメソッド]
        /// <summary>
        /// 分析項目・ロットリスト設定
        /// </summary>
        /// <remarks>
        /// 分析項目・ロットを設定します
        /// </remarks>
        public void setPartsList(List<Tuple<String, String, String>> list)
        {
            foreach ( var nameLot in list )
            {
                // 試薬ロット不明の場合、()を付けない
                String setString = String.Empty;

                if ( !String.IsNullOrEmpty( nameLot.Item3 ) )
                {
                    setString = String.Format( "{0}:{1}({2})", nameLot.Item1, nameLot.Item2, nameLot.Item3 );
                }
                else
                {
                    setString = String.Format( "{0}:{1}", nameLot.Item1, nameLot.Item2);
                }

                this.listPartsName.Items.Add( setString ); 
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_CALIBCURVECONFIRM_000;
            // メッセージラベル            
            this.lblMessage.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_CALIBCURVECONFIRM_001;            
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
