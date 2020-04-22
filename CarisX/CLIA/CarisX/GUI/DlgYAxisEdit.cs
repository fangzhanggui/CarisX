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
    /// Y軸編集ダイアログクラス
    /// </summary>
    public partial class DlgYAxisEdit : DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgYAxisEdit()
        {
            InitializeComponent();
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 日差変動Y軸最大値の取得、設定
        /// </summary>
        public Double InterDayMax
        {
            get
            {
                return (Double)this.numInterDayYAxisMax.Value;
            }
            set
            {
                this.numInterDayYAxisMax.Value = value;
            }
        }

        /// <summary>
        /// 日差変動Y軸最小値の取得、設定
        /// </summary>
        public Double InterDayMin
        {
            get
            {
                return (Double)this.numInterDayYAxisMin.Value;
            }
            set
            {
                this.numInterDayYAxisMin.Value = value;
            }
        }

        /// <summary>
        /// R管理図Y軸最大値の取得、設定
        /// </summary>
        public Double RControlMax
        {
            get
            {
                return (Double)this.numRControlYAxisMax.Value;
            }
            set
            {
                this.numRControlYAxisMax.Value = value;
            }
        }

        /// <summary>
        /// R管理図Y軸最小値の取得、設定
        /// </summary>
        public Double RControlMin
        {
            get
            {
                return (Double)this.numRControlYAxisMin.Value;
            }
            set
            {
                this.numRControlYAxisMin.Value = value;
            }
        }

        /// <summary>
        /// 日内変動Y軸最大値の取得、設定
        /// </summary>
        public Double IntraDayMax
        {
            get
            {
                return (Double)this.numIntraDayYAxisMax.Value;
            }
            set
            {
                this.numIntraDayYAxisMax.Value = value;
            }
        }

        /// <summary>
        /// 日内変動Y軸最小値の取得、設定
        /// </summary>
        public Double IntraDayMin
        {
            get
            {
                return (Double)this.numIntraDayYAxisMin.Value;
            }
            set
            {
                this.numIntraDayYAxisMin.Value = value;
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

            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_YAXISEDIT_000;

            this.grpInterDay.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_YAXISEDIT_001;
            this.grpRControl.Text =Oelco.CarisX.Properties.Resources.STRING_DLG_YAXISEDIT_002;
            this.grpIntraDay.Text =Oelco.CarisX.Properties.Resources.STRING_DLG_YAXISEDIT_003;

            this.lblTitleInterDayYAxisMax.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_YAXISEDIT_004;
            this.lblTitleInterDayYAxisMin.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_YAXISEDIT_005;
            this.lblTitleRControlYAxisMax.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_YAXISEDIT_004;
            this.lblTitleRControlYAxisMin.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_YAXISEDIT_005;
            this.lblTitleIntraDayYAxisMax.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_YAXISEDIT_004;
            this.lblTitleIntraDayYAxisMin.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_YAXISEDIT_005;
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
