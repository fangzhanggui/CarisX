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
    /// ペースト回数入力ダイアログクラス
    /// </summary>
    public partial class DlgPasteCountInput : DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgPasteCountInput()
        {
            InitializeComponent();
        }
    
        #endregion

        #region [プロパティ]

        /// <summary>
        /// ペースト回数の取得
        /// </summary>
        public Int32 PasteCount
        {
            get
            {
                return (Int32)this.numPasteCount.Value;
            }
        }

        /// <summary>
        /// ペースト回数の最大値取得、設定
        /// </summary>
        public Int32 MaxValue
        {
            get
            {
                return (Int32)this.numPasteCount.MaxValue;
            }
            set
            {
                this.numPasteCount.MaxValue = value;
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_PASTECOUNT_004;            
            
            this.lblMessage1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_PASTECOUNT_000;
            this.grpMessage1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_PASTECOUNT_001;
            this.lblMessage2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_PASTECOUNT_002;
            this.btnOk.Text =  Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;
        }

        #endregion

        /// <summary>
        /// OKボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にOKを設定して画面を終了します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click( object sender, EventArgs e )
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Cancelボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にキャンセルを設定して画面を終了します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click( object sender, EventArgs e )
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }
}
