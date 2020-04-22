using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Oelco.CarisX.GUI;
using Oelco.CarisX.Utility;
using Oelco.Common.Utility;
using Oelco.CarisX.DB;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// ログイン画面クラス
    /// </summary>
    public partial class DlgLogin : DlgCarisXBase
    {
        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            this.btnOk.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;
            this.lblDialogTitle.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_LOGIN_000;
            this.lblUserID.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_LOGIN_001;
            this.lblPassword.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_LOGIN_002;
            this.lblInputErrorMessage.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_LOGIN_003;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgLogin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// OKボタンクリックイベントハンドラ
        /// </summary>
        /// <remarks>
        /// ログイン処理を行います。
        /// </remarks>
        /// <param name="sender">不使用</param>
        /// <param name="e">不使用</param>
        private void btnOk_Click( object sender, EventArgs e )
        {
            // ログイン処理
            if ( Singleton<CarisXUserLevelManager>.Instance.Login( this.txtUserID.Text, this.txtPassword.Text ) )
            {
                Singleton<NotifyManager>.Instance.RaiseSignalQueue( (Int32)CarisX.Const.NotifyKind.UserLevelChanged, Singleton<CarisXUserLevelManager>.Instance.NowUserLevel );                
                this.DialogResult = System.Windows.Forms.DialogResult.OK;

                this.Close();
            }
            else
            {
                // ユーザーID入力部にフォーカスを当てる
                this.txtUserID.Focus();

                // エラーメッセージ表示
                this.lblInputErrorMessage.Visible = true;

            }
        }

        /// <summary>
        /// キャンセルボタンクリックイベントハンドラ
        /// </summary>
        /// <remarks>
        /// ログイン画面のキャンセル処理を行います。
        /// </remarks>
        /// <param name="sender">不使用</param>
        /// <param name="e">不使用</param>
        private void btnCancel_Click( object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// パスワードフォーカスイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPassword_Enter(object sender, EventArgs e)
        {
            // エラーメッセージ非表示
            this.lblInputErrorMessage.Visible = false;
        }

        /// <summary>
        /// ユーザーIDフォーカスイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUserID_Enter(object sender, EventArgs e)
        {
            // エラーメッセージ非表示
            this.lblInputErrorMessage.Visible = false;
        }

        /// <summary>
        /// 表示イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DlgLogin_Shown(object sender, EventArgs e)
        {
            // ユーザーID入力部にフォーカスを当てる
            this.txtUserID.Focus();
        }
    }
}
