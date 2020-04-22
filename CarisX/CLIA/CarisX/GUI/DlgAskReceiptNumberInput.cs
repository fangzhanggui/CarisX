using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 受付番号入力問い合わせガイアログクラス
    /// </summary>
    public partial class DlgAskReceiptNumberInput : DlgCarisXBase
    {
         #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgAskReceiptNumberInput()
        {
            InitializeComponent();
        }
    
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 領収書番号最大値の取得、設定
        /// </summary>
        public Int32 MaxValue
        {
            get
            {
                return (Int32)this.numReceiptNumberMax.Value;
            }
            set
            {
                this.numReceiptNumberMax.Value = value;
            }
        }
        /// <summary>
        /// 領収書番号最小値の取得、設定
        /// </summary>
        public Int32 MinValue
        {
            get
            {
                return (Int32)this.numReceiptNumberMin.Value;
            }
            set
            {
                this.numReceiptNumberMin.Value = value;
            }
        }
        
        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// リソースの初期化
        /// </summary>
        /// <remarks>
        /// リソースを初期化します
        /// </remarks>
        protected override void initializeResource()
        {
        }

        /// <summary>
        /// コンポーネントの初期化
        /// </summary>
        /// <remarks>
        /// コンポーネントを初期化します
        /// </remarks>
        protected override void initializeFormComponent()
        {
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_ASK_RECEIPTNUMBER_INPUT_000;            
            this.lblSubTitle.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_ASK_RECEIPTNUMBER_INPUT_001;
            this.ultraLabel1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_ASK_RECEIPTNUMBER_INPUT_002;
            this.lblReceiptNumberTitle.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_ASK_RECEIPTNUMBER_INPUT_003;
            this.btnOk.Text =  Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;
        }

        #endregion

        /// <summary>
        /// OKボタン押下イベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にOKを設定して、画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnOk_Click( object sender, EventArgs e )
        {
            // 大小反転チェック

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// キャンセルボタン押下イベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にキャンセルを設定して、画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnCancel_Click( object sender, EventArgs e )
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
