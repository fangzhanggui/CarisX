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
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// メンテンナンス日誌選択画面ダイアログクラス
    /// </summary>
    public partial class DlgOptionReconfirmMaintenanceJournal : DlgCarisXBaseSys
    {
        #region [定数定義]

        /// <summary>
        ///  メンテナンス日誌種別
        /// </summary>
        private MaintenanceJournalType mainteJournalType = MaintenanceJournalType.User;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgOptionReconfirmMaintenanceJournal(MaintenanceJournalType type)
        {
            // 種別の設定
            mainteJournalType = type;

            InitializeComponent();
        }

        #endregion

        #region [プロパティ]

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
            chkKind1.Checked = false;
            chkKind2.Checked = false;
            chkKind3.Checked = false;
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTION_RECONFIRMMAINTENANCEJOURNAL_000;

            // チェックボックス
            if (this.mainteJournalType == MaintenanceJournalType.User)
            {
                this.chkKind1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTION_RECONFIRMMAINTENANCEJOURNAL_001;
                this.chkKind2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTION_RECONFIRMMAINTENANCEJOURNAL_002;
                this.chkKind3.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTION_RECONFIRMMAINTENANCEJOURNAL_003;
            }
            else if (this.mainteJournalType == MaintenanceJournalType.Serviceman)
            {
                this.chkKind1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTION_RECONFIRMMAINTENANCEJOURNAL_006;
                this.chkKind2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTION_RECONFIRMMAINTENANCEJOURNAL_007;
                this.chkKind3.Visible = false;
            }

            // ラベル
            this.lblDescription.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTION_RECONFIRMMAINTENANCEJOURNAL_004;
            this.lblComment.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTION_RECONFIRMMAINTENANCEJOURNAL_005;
            this.lblComment.Visible = false;

            // ボタン
            this.btnOK.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// OKボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 設定パラメータをファイルに保存し、
        /// ダイアログ結果にOKを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            bool blOpenFlag = false;

            if (this.mainteJournalType == MaintenanceJournalType.User)
            {
                if (chkKind1.Checked)
                {
                    // User - Dailyのパラメータを全てfalseにします
                    Singleton<Oelco.CarisX.Common.MaintenanceJournalInfoManager>.Instance.FlagChange(Kind.U_Daily);
                    // User - Dailyのクリアデータを作成します
                    blOpenFlag = true;
                }
                // Weekly
                if (chkKind2.Checked)
                {
                    Singleton<Oelco.CarisX.Common.MaintenanceJournalInfoManager>.Instance.FlagChange(Kind.U_Weekly);
                    blOpenFlag = true;
                }
                // Monthly
                if (chkKind3.Checked)
                {
                    Singleton<Oelco.CarisX.Common.MaintenanceJournalInfoManager>.Instance.FlagChange(Kind.U_Monthly);
                    blOpenFlag = true;
                }

                // メンテナンス日誌開くかチェックします
                if (blOpenFlag)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    CarisXMaintenanceUserParameter AllCheckUserFlag = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param;

                    // メンテナンス期限切れのチェック
                    DateTime nowDateTime = DateTime.Now;
                    if (((AllCheckUserFlag.AllFinishDaily.AddDays(1) < nowDateTime)
                      || (AllCheckUserFlag.AllFinishWeekly.AddDays(7) < nowDateTime)
                      || (AllCheckUserFlag.AllFinishMonthly.AddMonths(1) < nowDateTime))
                      || ((AllCheckUserFlag.AllCheckDaily == false)
                      || (AllCheckUserFlag.AllCheckWeekly == false)
                      || (AllCheckUserFlag.AllCheckMonthly == false)))
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        // クリア対象なしの状態で全てのチェックが完了している場合、チェックボックスの下に赤字メッセージを表示する。
                        this.lblComment.Visible = true;
                    }
                }
            }
            else if (this.mainteJournalType == MaintenanceJournalType.Serviceman)
            {
                // Monthly
                if (chkKind1.Checked)
                {
                    // Serviceman - Monthlyのパラメータを全てfalseにします
                    Singleton<Oelco.CarisX.Common.MaintenanceJournalInfoManager>.Instance.FlagChange(Kind.S_Monthly);
                    // Serviceman - Monthlyのクリアデータを作成します
                    blOpenFlag = true;
                }
                // Yearly
                if (chkKind2.Checked)
                {
                    Singleton<Oelco.CarisX.Common.MaintenanceJournalInfoManager>.Instance.FlagChange(Kind.S_Yearly);
                    blOpenFlag = true;
                }
                // メンテナンス日誌開くかチェックします
                if (blOpenFlag)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    CarisXMaintenanceServicemanParameter AllCheckServicemanFlag = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param;

                    // メンテナンス期限切れのチェック
                    DateTime nowDateTime = DateTime.Now;
                    if (((AllCheckServicemanFlag.AllFinishMonthly.AddMonths(1) < nowDateTime)
                        || (AllCheckServicemanFlag.AllFinishYearly.AddYears(1) < nowDateTime))
                      || ((AllCheckServicemanFlag.AllCheckMonthly == false)
                      || (AllCheckServicemanFlag.AllCheckYearly == false)))
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        // クリア対象なしの状態で全てのチェックが完了している場合、チェックボックスの下に赤字メッセージを表示する。
                        this.lblComment.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// Cancelボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にキャンセルを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        #endregion

    }
}
