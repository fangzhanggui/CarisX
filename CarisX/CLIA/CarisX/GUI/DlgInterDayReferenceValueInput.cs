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
    /// 日差変動データ入力ダイアログクラス
    /// </summary>
    public partial class DlgInterDayReferenceValueInput : DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgInterDayReferenceValueInput()
        {
            InitializeComponent();
        }
    
        #endregion

        #region [プロパティ]
        /// <summary>
        /// 管理平均値の取得、設定
        /// </summary>
        public Double Mean
        {
            get
            {
                return (Double)this.numInterDayMean.Value;
            }
            set
            {
                this.numInterDayMean.Value = value;
            }
        }

        /// <summary>
        /// 管理濃度幅(標準偏差)の取得、設定
        /// </summary>
        public Double ConcentrationWidth
        {
            get
            {
                return (Double)this.numInterDayConcentrationWidth.Value;
            }
            set
            {
                this.numInterDayConcentrationWidth.Value = value;
            }
        }

        /// <summary>
        /// 管理平均値、管理濃度幅(標準偏差)入力コントロールのMaskInputの設定
        /// </summary>
        public string MaskInput
        {
            set
            {
                this.numInterDayMean.MaskInput = value;
                this.numInterDayConcentrationWidth.MaskInput = value;
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
            // パネル既定ボタン
            this.btnOk.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;

            // ダイアログタイトル
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_INTERDAYREFERENCEVALUEINPUT_000;

            // 項目タイトル
            this.lblTitleInterDayMean.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_INTERDAYREFERENCEVALUEINPUT_001;
            this.lblTitleInterDayConcentrationWidth.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_INTERDAYREFERENCEVALUEINPUT_002;
        }

        #endregion

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

    }
}
