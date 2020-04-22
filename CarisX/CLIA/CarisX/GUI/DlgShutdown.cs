using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.CarisX.Utility;
using Oelco.Common.Utility;
using Oelco.CarisX.Status;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 終了処理(シャットダウン)ダイアログクラス
    /// </summary>
    public partial class DlgShutdown : DlgCarisXBase
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 全て終了イベントハンドラ
        /// </summary>
        private event FormClosedEventHandler AllShutdown;

        /// <summary>
        /// 終了イベントハンドラ
        /// </summary>
        private event FormClosedEventHandler Shutdown;

        /// <summary>
        /// 自動立ち上げ待機イベントハンドラ
        /// </summary>
        private event FormClosedEventHandler AutomaticStartupWait;

        /// <summary>
        /// ログアウトイベントハンドラ
        /// </summary>
        private event FormClosedEventHandler LogOut;

        #endregion


        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgShutdown( FormClosedEventHandler allShutdown, FormClosedEventHandler shutdown, FormClosedEventHandler automaticStartupWait, FormClosedEventHandler logOut )
        {
            InitializeComponent();

            // 各ボタンイベント追加
            this.AllShutdown += allShutdown;
            this.Shutdown += shutdown;
            this.AutomaticStartupWait += automaticStartupWait;
            this.LogOut += logOut;

            if (this.AllShutdown == null)
            {
                this.btnAllShutdown.Enabled = false;
            }
            if (this.Shutdown == null)
            {
                this.btnShutdown.Enabled = false;
            }
            if (this.AutomaticStartupWait == null)
            {
                this.btnAutomaticStartupWait.Enabled = false;
            }
            if (this.LogOut == null)
            {
                this.btnLogOut.Enabled = false;
            }
        }

        #endregion
        /// <summary>
        /// 選択ボタン
        /// </summary>
        public enum SelectedMethod
        {
            AllShutDown,
            ShutDown,
            AutomaticStartupWait,
            LogOut,
            Cancel
        }
        /// <summary>
        /// 選択ボタン取得、設定
        /// </summary>
        public SelectedMethod SelectResult
        {
            get;
            set;
        }
        #region [プロパティ]
        /// <summary>
        /// キャンセルボタン有効無効の取得、設定
        /// </summary>
        public Boolean EnableCancel
        {
            get
            {
                return this.btnCancel.Enabled;
            }
            set
            {
                this.btnCancel.Enabled = value;
            }
        }
        /// <summary>
        /// 全シャットダウンボタン有効無効の取得、設定
        /// </summary>
        public Boolean EnableAllShutdown
        {
            get
            {
                return this.btnAllShutdown.Enabled;
            }
            set
            {
                if ( this.AllShutdown != null )
                {
                    this.btnAllShutdown.Enabled = value;
                }
            }
        }
        /// <summary>
        /// シャットダウンボタン有効無効の取得、設定
        /// </summary>
        public Boolean EnableShutdown
        {
            get
            {
                return this.btnShutdown.Enabled;
            }
            set
            {
                if ( this.Shutdown != null )
                {
                    this.btnShutdown.Enabled = value;
                }
            }
        }
        /// <summary>
        /// 自動立ち上げ待機ボタン有効無効の取得、設定
        /// </summary>
        public Boolean EnableAutomaticStartupWait
        {
            get
            {
                return this.btnAutomaticStartupWait.Enabled;
            }
            set
            {
                if ( this.AutomaticStartupWait != null )
                {
                    this.btnAutomaticStartupWait.Enabled = value;
                }
            }
        }
        /// <summary>
        /// ログアウトボタン有効無効の取得、設定
        /// </summary>
        public Boolean EnableLogOut
        {
            get
            {
                return this.btnLogOut.Enabled;
            }
            set
            {
                if ( this.LogOut != null )
                {
                    this.btnLogOut.Enabled = value;
                }
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
            if ( Singleton<SystemStatus>.Instance.Status == SystemStatusKind.NoLink )
            {
                this.btnAutomaticStartupWait.Enabled = false;
            }
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_SHUTDOWN_000;

            // ラベル
            this.lblMessage.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SHUTDOWN_001;

            // ボタン
            this.btnLogOut.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SHUTDOWN_002;
            this.btnAutomaticStartupWait.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SHUTDOWN_003;
            this.btnShutdown.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SHUTDOWN_004;
            this.btnAllShutdown.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SHUTDOWN_005;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;
        }
        /// <summary>
        /// ユーザーレベル設定
        /// </summary>
        /// <remarks>
        /// ユーザーレベルの設定を行います
        /// </remarks>
        protected override void setUser( Object value )
        {
            base.setUser(value);
            this.btnShutdown.Enabled = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction( CarisXUserLevelManagedAction.AnalyserParameterSetting );
            
        }


        #endregion

        #region [privateメソッド]

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
            this.SelectResult = DlgShutdown.SelectedMethod.Cancel;
            this.Close();
        }

        /// <summary>
        /// 全て終了ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果に全シャットダウンを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnAllShutdown_Click( object sender, EventArgs e )
        {
            this.SelectResult = DlgShutdown.SelectedMethod.AllShutDown;
            //if ( this.AllShutdown != null )
            //{
            //    this.AllShutdown( this, new FormClosedEventArgs( CloseReason.None ) );
            //}
            //else
            //{
            //    this.Close();
            //}
            this.FormClosed += this.AllShutdown;
            this.Close();
        }
        /// <summary>
        /// 終了ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にシャットダウンを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnShutdown_Click( object sender, EventArgs e )
        {
            this.SelectResult = DlgShutdown.SelectedMethod.ShutDown;
            this.FormClosed += this.Shutdown;
            this.Close();
        }

        /// <summary>
        /// 自動立ち上げ待機ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果に自動立ち上げ待機を設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnAutomaticStartupWait_Click( object sender, EventArgs e )
        {
            this.SelectResult = DlgShutdown.SelectedMethod.AutomaticStartupWait;
            this.FormClosed += this.AutomaticStartupWait;
            this.Close();
        }

        /// <summary>
        /// ログアウトボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にログアウトを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnLogOut_Click( object sender, EventArgs e )
        {
            this.SelectResult = DlgShutdown.SelectedMethod.LogOut;
            this.FormClosed += this.LogOut;
            this.Close();
        }

        #endregion

    }
}
