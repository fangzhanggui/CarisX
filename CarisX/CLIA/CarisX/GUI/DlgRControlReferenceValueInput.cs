using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 管理値R入力ダイアログ
    /// </summary>
    public partial class DlgRControlReferenceValueInput : DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgRControlReferenceValueInput()
        {
            InitializeComponent();
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 管理値Rの取得、設定
        /// </summary>
        public Double ControlR
        {
            get
            {
                return (Double)this.numControlR.Value;
            }
            set
            {
                this.numControlR.Value = value;
            }
        }

        /// <summary>
        /// 管理値Rの入力コントロールMaskInputの設定
        /// </summary>
        public String MaskInput
        {
            set
            {
                this.numControlR.MaskInput = value;
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
            // タイトル
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RCONTROLREFERENCEVALUEINPUT_000;

            // ラベル
            this.lblTitleControlR.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_RCONTROLREFERENCEVALUEINPUT_001;

            // ボタン
            this.btnOk.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;
        }
   
        #endregion

        #region [privateメソッド]

        /// <summary>
        /// OKボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にOKを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnOk_Click( object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Cancelボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にキャンセルを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnCancel_Click( object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        #endregion

    }
}
