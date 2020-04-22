using Oelco.CarisX.GUI;

namespace Oelco.CarisX.Maintenance
{
    /// <summary>
    /// モジュール切り替え時待機用ダイアログクラス
    /// </summary>
    public partial class DlgWaitModuleChange : DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgWaitModuleChange()
        {
            InitializeComponent();
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
            // ダイアログタイトル
            this.Caption = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_MODULECHANGE_000;

            // お待ちください
            this.lblPleaseWait.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_MODULECHANGE_001;
        }
        #endregion
    }

}
