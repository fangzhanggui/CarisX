using Oelco.CarisX.Const;
using Oelco.Common.Utility;
using System;
using System.Collections.Generic;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// キャリブレーション確認ダイアログ
    /// </summary>
    public partial class DlgCalibrationConfirm : DlgCarisXBase
    {
        #region [変数]
        private Int32 ModuleIndex;
        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgCalibrationConfirm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 内部コンストラクタ
        /// </summary>
        /// <param name="protocols"></param>
        /// <param name="lotNo"></param>
        public DlgCalibrationConfirm( String protocols, String lotNo, Int32 moduleindex)
        {

            InitializeComponent();

            this.lblProtocols.Text = protocols;
            this.lblLotNumber.Text = lotNo;
            ModuleIndex = moduleindex;

        }

        #endregion

        #region [protectedメソッド]


        /// <summary>
        /// ロードイベントオーバーライド
        /// </summary>
        /// <remarks>
        /// ロード時に警告灯コマンド送信を行います。
        /// </remarks>
        /// <param name="e">イベントデータ（不使用）</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            // スレーブに警告灯・ブザー送信
            Singleton<NotifyManager>.Instance.PushSignalQueue( (Int32)NotifyKind.SendCaution, new DPRErrorCode(1, 0) );
        }


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
            this.lblDialogTitle.Text = CarisX.Properties.Resources.STRING_DLG_CALIBRATIONCONFIRM000;
            this.lblProtocolTitle.Text = CarisX.Properties.Resources.STRING_DLG_CALIBRATIONCONFIRM001;
            this.lblLotTitle.Text = CarisX.Properties.Resources.STRING_DLG_CALIBRATIONCONFIRM002;
            this.lblConfirmMessage.Text = CarisX.Properties.Resources.STRING_DLG_CALIBRATIONCONFIRM003;

            this.btnYes.Text = CarisX.Properties.Resources.STRING_COMMON_020;
            this.btnNo.Text = CarisX.Properties.Resources.STRING_COMMON_021;
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// ブザー消去
        /// </summary>
        /// <remarks>
        /// ブザーを消去します
        /// </remarks>
        void clearBussor()
        {
            // スレーブにブザー消去送信
            Singleton<NotifyManager>.Instance.PushSignalQueue( (Int32)NotifyKind.SendBuzzerCancel, ModuleIndex);
        }

        /// <summary>
        /// Yesボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にOKを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void Yes_Click( object sender, EventArgs e )
        {
            this.clearBussor();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Noボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にキャンセルを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void No_Click( object sender, EventArgs e )
        {
            this.clearBussor();

            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        #endregion
       
    }
}
