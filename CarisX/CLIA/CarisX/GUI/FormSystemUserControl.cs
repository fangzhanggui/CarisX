using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.Common.Utility;
using Oelco.CarisX.DB;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Const;
using Oelco.CarisX.Status;
using Oelco.CarisX.Common;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// ユーザー管理画面クラス
    /// </summary>
    public partial class FormSystemUserControl : FormChildBase
    {
        #region [定数定義]

        /// <summary>
        /// 登録
        /// </summary>
        private const String REGISTER = "Register";

        /// <summary>
        /// 編集
        /// </summary>
        private const String EDIT = "Edit";

        /// <summary>
        /// 削除
        /// </summary>
        private const String DELETE = "Delete";

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// ユーザーレベル.
        /// </summary>
        private Dictionary<UserLevel, String> authorityLevelList = new Dictionary<UserLevel, String>()
        {
                {UserLevel.Level1,Oelco.CarisX.Properties.Resources.STRING_SYSTEMUSERCONTROL_007},
                {UserLevel.Level2,Oelco.CarisX.Properties.Resources.STRING_SYSTEMUSERCONTROL_008},
                {UserLevel.Level3,Oelco.CarisX.Properties.Resources.STRING_SYSTEMUSERCONTROL_009}
        };

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormSystemUserControl()
        {
            InitializeComponent();

            // コマンドバーのイベント追加
            this.tlbCommandBar.Tools[REGISTER].ToolClick += (sender, e) => this.registerData();
            this.tlbCommandBar.Tools[EDIT].ToolClick += (sender, e) => this.editData();
            this.tlbCommandBar.Tools[DELETE].ToolClick += (sender, e) => this.deleteData();

            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SystemStatusChanged, this.onSystemStatusChanged);

            //设置ToolBar的右键功能不可用
            this.tlbCommandBar.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.tlbCommandBar_BeforeToolbarListDropdown);

        }

        //设置ToolBar的右键功能不可用
        private void tlbCommandBar_BeforeToolbarListDropdown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventArgs e)
        {
            e.Cancel = true;
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
            // 権限レベルをコンボボックスへ設定
            this.cmbAuthorityLevel.Items.Clear();
            this.cmbAuthorityLevel.DataSource = this.authorityLevelList.ToList();// UNDONE:本実装(ユーザーレベル3以下をすべて表示)
            this.cmbAuthorityLevel.ValueMember = "Key";
            this.cmbAuthorityLevel.DisplayMember = "Value";
            this.cmbAuthorityLevel.SelectedIndex = 0;

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;

            this.txtPassword.PasswordChar = Convert.ToChar(Oelco.CarisX.Properties.Resources.STRING_COMMON_005);
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
            this.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMUSERCONTROL_000;

            // コマンドバー
            this.tlbCommandBar.Tools[REGISTER].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_023;
            this.tlbCommandBar.Tools[EDIT].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_017;
            this.tlbCommandBar.Tools[DELETE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_002;

            // ラベル
            this.lblSelectUserID.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMUSERCONTROL_001;
            this.lblRegisterEdit.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMUSERCONTROL_002;
            this.lblUserID.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMUSERCONTROL_003;
            this.lblPassword.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMUSERCONTROL_004;
            this.lblAuthorityLevel.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMUSERCONTROL_005;

            // ボタン
            this.btnRegister.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMUSERCONTROL_006;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;
        }

        #endregion

        /// <summary>
        /// フォーム表示
        /// </summary>
        /// <remarks>
        /// ユーザー情報(UserID)を設定して画面を表示します
        /// </remarks>
        public override void Show()
        {
            // ユーザー情報(UserID)をコンボボックスへ設定
            setUserIDToCombo();

            base.Show();
        }
        #region [privateメソッド]

        #region _コマンドバー_

        /// <summary>
        /// 登録開始
        /// </summary>
        /// <remarks>
        /// ユーザー情報の登録開始します
        /// </remarks>
        private void registerData()
        {
            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_009 });

            this.txtUserID.Text = String.Empty;
            this.txtPassword.Text = String.Empty;
            if (this.cmbAuthorityLevel.Items.Count > 0)
            {
                this.cmbAuthorityLevel.SelectedIndex = 0;
            }
            this.startRegistEdit(false);
        }

        /// <summary>
        /// 編集開始
        /// </summary>
        /// <remarks>
        /// ユーザー情報の編集開始します
        /// </remarks>
        private void editData()
        {
            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_042 });

            if (this.cmbUserID.SelectedIndex >= 0)
            {
                this.txtUserID.Text = this.cmbUserID.Value.ToString();
                List<Tuple<String, String, UserLevel>> userInfos = Singleton<UserInfoDB>.Instance.GetUserInformation();
                foreach (Tuple<String, String, UserLevel> userInfo in userInfos)
                {
                    if (userInfo.Item1 == this.txtUserID.Text)
                    {
                        this.txtPassword.Text = userInfo.Item2;
                        this.cmbAuthorityLevel.Value = userInfo.Item3;
                        break;
                    }
                }
                this.startRegistEdit(true);
            }

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// 削除開始
        /// </summary>
        /// <remarks>
        /// ユーザー情報の削除開始します
        /// </remarks>
        private void deleteData()
        {

            this.removeUser();
        }

        #endregion

        /// <summary>
        /// フォーム読み込みイベント
        /// </summary>
        /// <remarks>
        /// ユーザー情報(UserID)をコンボボックスへ設定します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormSystemUserControl_Load(object sender, EventArgs e)
        {
            // ユーザー情報(UserID)をコンボボックスへ設定
            setUserIDToCombo();
        }

        /// <summary>
        /// 登録ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ユーザー情報を登録します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnRegister_Click(object sender, EventArgs e)
        {
            // 新規登録かどうかを判定
            Boolean isAddUser = this.txtUserID.Enabled;
            // 入力欄全体をEnabled=falseにする
            this.gbxRegisterEdit.Enabled = false;
            if (isAddUser)
            {
                // ユーザー情報を登録
                this.addUser();
            }
            else
            {
                // ユーザー情報を編集
                this.editUser();
            }

            this.clearForm();
            this.endRegistEdit();

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// キャンセルボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 画面クリアしてユーザー情報の登録をキャンセルします
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_043 });

            this.clearForm();

            this.endRegistEdit();

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// 登録/編集の開始
        /// </summary>
        /// <remarks>
        /// 登録/編集を開始します
        /// </remarks>
        /// <param name="edit">編集:true、登録:false</param>
        private void startRegistEdit(Boolean edit)
        {
            this.tlbCommandBar.Enabled = false;
            this.cmbUserID.Enabled = false;
            this.gbxRegisterEdit.Enabled = true;
            this.txtUserID.Enabled = !edit;
            this.txtPassword.Enabled = true;
            this.cmbAuthorityLevel.Enabled = true;
        }

        /// <summary>
        /// 登録/編集の完了
        /// </summary>
        /// <remarks>
        /// 登録/編集を完了します
        /// </remarks>
        private void endRegistEdit()
        {
            this.tlbCommandBar.Enabled = true;
            this.cmbUserID.Enabled = true;
        }

        /// <summary>
        /// 画面クリア
        /// </summary>
        /// <remarks>
        /// 画面クリアします
        /// </remarks>
        private void clearForm()
        {
            this.gbxRegisterEdit.Enabled = false;
            this.txtUserID.Text = String.Empty;
            this.txtPassword.Text = String.Empty;

            if (this.cmbAuthorityLevel.Items.Count > 0)
            {
                this.cmbAuthorityLevel.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// ユーザー情報の登録
        /// </summary>
        /// <remarks>
        /// ユーザー情報の登録します
        /// </remarks>
        private void addUser()
        {
            if (Singleton<CarisXUserLevelManager>.Instance.AddAccount(this.txtUserID.Text, this.txtPassword.Text, (UserLevel)this.cmbAuthorityLevel.Value))
            {
                // 操作履歴：登録実行
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_009 });

                // ユーザー情報(UserID)をコンボボックスへ再設定
                setUserIDToCombo();
            }
            else
            {
                // 登録不可メッセージ
                DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_159, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
            }
        }

        /// <summary>
        /// ユーザー情報の編集
        /// </summary>
        /// <remarks>
        /// ユーザー情報(UserID)をコンボボックスへ再設定します
        /// </remarks>
        private void editUser()
        {
            Singleton<CarisXUserLevelManager>.Instance.UpdateAccount(this.txtUserID.Text, this.txtPassword.Text, (UserLevel)this.cmbAuthorityLevel.Value);

            // ユーザー情報(UserID)をコンボボックスへ再設定（UserID変わらないはずだがいちおう）
            setUserIDToCombo();
        }

        /// <summary>
        /// ユーザー情報の削除
        /// </summary>
        /// <remarks>
        /// ユーザー情報の削除します
        /// </remarks>
        private void removeUser()
        {
            if (this.cmbUserID.SelectedIndex >= 0)
            {
                // 現在ログイン中のユーザであれば、削除不可
                if (Singleton<CarisXUserLevelManager>.Instance.NowUserID == this.cmbUserID.Value.ToString())
                {
                    // 削除不可メッセージ
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_086, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                }
                else
                {
                    // 削除確認メッセージ
                    if (DialogResult.OK == DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_038, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel))
                    {
                        Singleton<CarisXUserLevelManager>.Instance.RemoveAccount(this.cmbUserID.Value.ToString());

                        // 操作履歴：削除実行   
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_003 });

                        // ユーザー情報(UserID)をコンボボックスへ再設定
                        setUserIDToCombo();
                    }
                    else
                    {
                        // 操作履歴：削除キャンセル
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_004 });
                    }
                }
            }
        }

        /// <summary>
        /// ユーザー情報(UserID)をコンボボックスへ設定
        /// </summary>
        /// <remarks>
        /// ユーザー情報(UserID)をコンボボックスへ設定します
        /// </remarks>
        private void setUserIDToCombo()
        {
            // ユーザ情報をDBから読込み
            Singleton<UserInfoDB>.Instance.LoadDB();
            // ユーザー情報(UserID)をコンボボックスへ設定
            this.cmbUserID.Items.Clear();

            // レベル3以下のユーザを取得する。
            var userUnder3 = from v in Singleton<UserInfoDB>.Instance.GetUserInformation()
                             where (Int32)v.Item3 <= (Int32)UserLevel.Level3
                             select v;
            foreach (Tuple<String, String, UserLevel> userInfo in userUnder3)
            {
                this.cmbUserID.Items.Add(userInfo.Item1);
            }
        }

        /// <summary>
        /// システムステータス変化イベント
        /// </summary>
        /// <remarks>
        /// システムステータスにより画面項目の有効/無効変化します
        /// </remarks>
        /// <param name="value"></param>
        private void onSystemStatusChanged(Object value)
        {
            if (Singleton<SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId(Singleton<PublicMemory>.Instance.moduleIndex)] == SystemStatusKind.ReagentExchange)
            {
                // 画面クリア
                this.clearForm();
                // コマンドバー非活性
                this.tlbCommandBar.Enabled = false;
                // ユーザ選択コンボ活性化
                this.cmbUserID.Enabled = true;
            }
            else
            {
                switch (Singleton<SystemStatus>.Instance.Status)
                {
                    case SystemStatusKind.NoLink:
                    case SystemStatusKind.Standby:
                        // ボタン活性
                        this.endRegistEdit();
                        break;
                    case SystemStatusKind.WaitSlaveResponce:
                    case SystemStatusKind.Assay:
                    case SystemStatusKind.SamplingPause:
                    case SystemStatusKind.ToEndAssay:
                        // 画面クリア
                        this.clearForm();
                        // コマンドバー非活性
                        this.tlbCommandBar.Enabled = false;
                        // ユーザ選択コンボ活性化
                        this.cmbUserID.Enabled = true;
                        break;
                }

            }
        }

        /// <summary>
        /// UserID切り替えドイベント
        /// </summary>
        /// <remarks>
        /// 値変更時、Form共通の編集中フラグをONにします。
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUserID_ValueChanged(object sender, EventArgs e)
        {
            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        /// <summary>
        /// Password切り替えイベント
        /// </summary>
        /// <remarks>
        /// 値変更時、Form共通の編集中フラグをONにします。
        /// </remarks>
        private void txtPassword_ValueChanged(object sender, EventArgs e)
        {
            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        /// <summary>
        /// AuthorityLevel切り替えイベント
        /// </summary>
        /// <remarks>
        /// 値変更時、Form共通の編集中フラグをONにします。
        /// </remarks>
        private void cmbAuthorityLevel_ValueChanged(object sender, EventArgs e)
        {
            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        #endregion

    }
}
