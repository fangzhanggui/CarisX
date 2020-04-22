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
using Oelco.CarisX.Log;
using Oelco.Common.Utility;
using Oelco.Common.Log;

namespace Oelco.CarisX.Maintenance
{
    /// <summary>
    /// メンテナンスログイン画面クラス
    /// </summary>
    public partial class DlgMaintenanceLogin : DlgCarisXBase
    {
        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            this.btnOk.Text = Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Properties.Resources.STRING_COMMON_003;
            this.lblDialogTitle.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_LOGIN_000;
            this.lblPassword.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_LOGIN_001;
            this.lblLoginReason.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_LOGIN_002;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgMaintenanceLogin()
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
#if DEBUG
            if ( this.txtLoginReason.Text == this.txtPassword.Text && this.txtLoginReason.Text == String.Empty )
            {
                this.txtPassword.Text = "123456";
                this.txtLoginReason.Text = "DEBUG";
            }
#endif
            if (this.txtLoginReason.Text == String.Empty )
            {
                //ログイン理由を未入力は許さない
                return;
            }

            if ( Singleton<CarisXUserLevelManager>.Instance.PasswordCheck( this.txtPassword.Text ) )
            {
                //ログイン理由をＤＢへ書き込みする
                Singleton<CarisXLogManager>.Instance.Write(LogKind.MaintenanceLogin, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { txtLoginReason.Text });
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_LOGIN_004 + Properties.Resources.STRING_COMMON_013 + Properties.Resources.STRING_LOG_MSG_047 });

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
            }
        }

        /// <summary>
        /// キャンセルボタンクリックイベントハンドラ
        /// </summary>
        /// <remarks>
        /// メンテナンスログイン画面のキャンセル処理を行います。
        /// </remarks>
        /// <param name="sender">不使用</param>
        /// <param name="e">不使用</param>
        private void btnCancel_Click( object sender, EventArgs e )
        {
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_LOGIN_004 + Properties.Resources.STRING_COMMON_013 + Properties.Resources.STRING_LOG_MSG_043 });
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
