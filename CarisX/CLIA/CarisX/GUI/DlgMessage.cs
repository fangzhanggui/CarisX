using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Oelco.CarisX.GUI
{
    // TODO:文字サイズなど要調整
    // TODO:アイコンが必要かどうか要調査
    // TODO:ボタン配置検討
    /// <summary>
    /// 汎用メッセージダイアログ
    /// </summary>
    public partial class DlgMessage : DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgMessage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 内部コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="warningMessage">警告メッセージ</param>
        /// <param name="caption"></param>
        /// <param name="buttons"></param>
        protected DlgMessage( String message, String warningMessage, String caption, MessageDialogButtons buttons )
        {
            InitializeComponent();

            // ダイアログタイトル表示
            this.Caption = caption;
            this.ultraLabel1.Text = message;
            this.ultraLabel2.Text = warningMessage;

            switch ( buttons )
            {
            case MessageDialogButtons.Confirm:
                // 確認ボタン設定
                this.btnButton2.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_004;
                this.btnButton2.Click += new EventHandler( this.Confirm_Click );
                this.btnButton1.Visible = false;
                break;
            case MessageDialogButtons.OK:
                // OKボタン設定
                this.btnButton2.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
                this.btnButton2.Click += new EventHandler( this.OK_Click );
                this.btnButton1.Visible = false;
                break;
            case MessageDialogButtons.Cancel:
                // Cancelボタン設定
                this.btnButton1.Visible = false;

                this.btnButton2.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;
                this.btnButton2.Click += new EventHandler( this.Cancel_Click );
                this.btnButton2.Appearance.Image = Oelco.Common.Properties.Resources.Image_Exit;
                break;
            case MessageDialogButtons.OKCancel:
                // OK/Cancelボタン設定
                this.btnButton1.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
                this.btnButton1.Click += new EventHandler( this.OK_Click );
                this.btnButton2.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;
                this.btnButton2.Click += new EventHandler( this.Cancel_Click );
                this.btnButton2.Appearance.Image = Oelco.Common.Properties.Resources.Image_Exit;
                break;
            default:
                break;
            }

        }

        #endregion

        #region [publicメソッド]
        
        /// <summary>
        /// メッセージ表示ダイアログの表示
        /// </summary>
        /// <remarks>
        /// メッセージ表示ダイアログを表示します
        /// </remarks>
        /// <param name="message">メッセージ</param>
        /// <param name="warningMessage">警告、注意メッセージ(赤字)</param>
        /// <param name="caption">ダイアログタイトル</param>
        /// <param name="buttons">ボタン</param>
        /// <returns>ダイアログ結果</returns>
        public static DialogResult Show( String message, String warningMessage, String caption, MessageDialogButtons buttons )
        {
            return new DlgMessage( message, warningMessage, caption, buttons ).showDialog();
        }

        /// <summary>
        /// メッセージ表示ダイアログの生成
        /// </summary>
        /// <remarks>
        /// メッセージ表示ダイアログを生成します
        /// </remarks>
        /// <param name="message">メッセージ</param>
        /// <param name="warningMessage">警告、注意メッセージ(赤字)</param>
        /// <param name="caption">ダイアログタイトル</param>
        /// <param name="buttons">ボタン</param>
        /// <returns>ダイアログ結果</returns>
        public static DlgMessage Create( String message, String warningMessage, String caption, MessageDialogButtons buttons )
        {
            return new DlgMessage( message, warningMessage, caption, buttons );
        }

        #endregion

        #region [protectedメソッド]
        /// <summary>
        /// ダイアログ呼び出し
        /// </summary>
        /// <remarks>
        /// ダイアログ呼び出しを行います
        /// </remarks>
        /// <returns></returns>
        protected DialogResult showDialog()
        {
           return base.ShowDialog();
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
        }

        #endregion

        #region [privateメソッド]
        
        /// <summary>
        /// 確認ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にYesを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void Confirm_Click( object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Close();
        }

        /// <summary>
        /// OKボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にOKを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void OK_Click( object sender, EventArgs e )
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
        private void Cancel_Click( object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        #endregion

       
    }

    /// <summary>
    /// メッセージボタン種別
    /// </summary>
    public enum MessageDialogButtons
    {
        /// <summary>
        /// 確認
        /// </summary>
        Confirm,
        /// <summary>
        /// OK
        /// </summary>
        OK,
        /// <summary>
        /// Cancel
        /// </summary>
        Cancel,
        /// <summary>
        /// OKとCancel
        /// </summary>
        OKCancel,

    }
}
