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
using Oelco.CarisX.DB;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Status;
using Oelco.CarisX.Const;
using System.IO;
using System.Threading;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.Common.Comm;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// バージョンアップダイアログクラス
    /// </summary>
    public partial class DlgOptionVersionUp : DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgOptionVersionUp()
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
            this.btnOk.Enabled = true;
            this.btnCancel.Enabled = true;

            //チェックボックスの活性状態を制御する
            SetEnableRackSlave();

            // okボタンの活性状態を変更
            this.btnOkEnabledChange();
        }

        /// <summary>
        /// RackTransfer、Module1～4タブの活性状態制御を行う
        /// 接続されていないものは画面上操作できないようにする
        /// </summary>
        private void SetEnableRackSlave()
        {
            chkRackTransfer.Enabled = !(Singleton<CarisXCommManager>.Instance.GetRackTransferCommStatus() == ConnectionStatus.Offline);
            chkModule1.Enabled = !(Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus((int)ModuleIndex.Module1) == ConnectionStatus.Offline);
            chkModule2.Enabled = !(Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus((int)ModuleIndex.Module2) == ConnectionStatus.Offline);
            chkModule3.Enabled = !(Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus((int)ModuleIndex.Module3) == ConnectionStatus.Offline);
            chkModule4.Enabled = !(Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus((int)ModuleIndex.Module4) == ConnectionStatus.Offline);
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
            this.btnOk.Text = Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Properties.Resources.STRING_COMMON_003;

            // ダイアログタイトル
            this.Caption = Properties.Resources.STRING_DLG_OPTIONVERSIONUP_000;

            // 終了処理確認文
            this.lblMessage.Text = Properties.Resources.STRING_DLG_OPTIONVERSIONUP_001;

            //選択項目
            this.gbxSelect.Text = Properties.Resources.STRING_DLG_OPTIONVERSIONUP_002;
            this.chkRackTransfer.Text = Properties.Resources.STRING_DLG_OPTIONVERSIONUP_003;
            this.chkModule1.Text = Properties.Resources.STRING_DLG_OPTIONVERSIONUP_004;
            this.chkModule2.Text = Properties.Resources.STRING_DLG_OPTIONVERSIONUP_005;
            this.chkModule3.Text = Properties.Resources.STRING_DLG_OPTIONVERSIONUP_006;
            this.chkModule4.Text = Properties.Resources.STRING_DLG_OPTIONVERSIONUP_007;

            this.lblProgramVersion.Text = Properties.Resources.STRING_DLG_OPTIONVERSIONUP_008;
            this.lblRackTransferProgramVersion.Text = Singleton<FormSystemOption>.Instance.RackTransferVersionText; //System Option画面に表示されている内容を取得
            this.lblSlave1ProgramVersion.Text = Singleton<FormSystemOption>.Instance.Slave1VersionText;             //System Option画面に表示されている内容を取得
            this.lblSlave2ProgramVersion.Text = Singleton<FormSystemOption>.Instance.Slave2VersionText;             //System Option画面に表示されている内容を取得
            this.lblSlave3ProgramVersion.Text = Singleton<FormSystemOption>.Instance.Slave3VersionText;             //System Option画面に表示されている内容を取得
            this.lblSlave4ProgramVersion.Text = Singleton<FormSystemOption>.Instance.Slave4VersionText;             //System Option画面に表示されている内容を取得

        }

        #endregion

        #region [privateメソッド]

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
            // ボタン、チェックボックスを操作不可にする。
            this.btnOk.Enabled = false;
            this.btnCancel.Enabled = false;
            this.chkRackTransfer.Enabled = false;
            this.chkModule1.Enabled = false;
            this.chkModule2.Enabled = false;
            this.chkModule3.Enabled = false;
            this.chkModule4.Enabled = false;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            try
            {
                // インストーラのパスを生成
                string install_path = SubFunction.GetApplicationDirectory() + @"\Wan200Updater.exe";

                // スレーブ切断処理
                Singleton<SystemStatus>.Instance.setAllModuleStatus(SystemStatusKind.NoLink);
                Singleton<CarisXCommManager>.Instance.DisConnect();

                // スレーブインストーラを起動
                if (File.Exists(install_path))
                {
                    //画面上でチェックされているものに対応パラメータでインストーラーを起動する

                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(install_path);

                    startInfo.Arguments = getInstallerArgument();
                    System.Diagnostics.Process.Start(startInfo);
                }

                // スレーブシャットダウン後にアプリケーション終了。G1200より
                for (int i = 0; i < 20; i++)
                {
                    Thread.Sleep(100);
                    Application.DoEvents();
                }

                // ソフト終了イベント通知
                Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.SystemEnd);
            }
            catch (Exception ex)
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                // TODO:例外処理
                //throw new System.Data.StrongTypingException(Properties.Resources.SlaveInstallerNotFound);   // "Slave Installer was not found.");
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
        private void btnCancel_Click( object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// パラメータで指定された内容に対応するインストーラー起動用パラメータを返す
        /// </summary>
        private String getInstallerArgument()
        {
            String argument = String.Empty;

            SocketParameter SocketParameter = new SocketParameter();
            int accessPoint = 0x00;                     //接続先
            String noChoice = String.Format("0x{0}", accessPoint.ToString("X2")); //未選択

            List<String> ArgumentsList = new List<String>() //各要素の引数リスト
            {
                noChoice,
                noChoice,
                noChoice,
                noChoice,
                noChoice
            };

            // ラック搬送にチェックが入っている場合
            if (chkRackTransfer.Checked)
            {
                accessPoint = (int)MachineCode.RackTransfer;
                ArgumentsList[0] = String.Format("0x{0}", accessPoint.ToString("X2"));
            }
            
            // モジュール1にチェックが入っている場合
            if (chkModule1.Checked)
            {
                accessPoint = (int)MachineCode.Slave + (int)ModuleIndex.Module1;
                ArgumentsList[1] = String.Format("0x{0}", accessPoint.ToString("X2"));
            }

            // モジュール2にチェックが入っている場合
            if (chkModule2.Checked)
            {
                accessPoint = (int)MachineCode.Slave + (int)ModuleIndex.Module2;
                ArgumentsList[2] = String.Format("0x{0}", accessPoint.ToString("X2"));
            }

            // モジュール3にチェックが入っている場合
            if (chkModule3.Checked)
            {
                accessPoint = (int)MachineCode.Slave + (int)ModuleIndex.Module3;
                ArgumentsList[3] = String.Format("0x{0}", accessPoint.ToString("X2"));
            }

            // モジュール4にチェックが入っている場合
            if (chkModule4.Checked)
            {
                accessPoint = (int)MachineCode.Slave + (int)ModuleIndex.Module4;
                ArgumentsList[4] = String.Format("0x{0}", accessPoint.ToString("X2"));
            }

            // リストに格納した内容を空白区切りで文字列に変換
            argument = String.Join(" ", ArgumentsList);

            return argument;
        }

        /// <summary>
        /// ラック搬送チェックボックスのチェック状態変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkRackTransfer_CheckedChanged( object sender, EventArgs e )
        {
            // okボタンの活性状態を変更
            this.btnOkEnabledChange();
        }

        /// <summary>
        /// モジュール1チェックボックスのチェック状態変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkModule1_CheckedChanged( object sender, EventArgs e )
        {
            // okボタンの活性状態を変更
            this.btnOkEnabledChange();
        }

        /// <summary>
        /// モジュール2チェックボックスのチェック状態変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkModule2_CheckedChanged( object sender, EventArgs e )
        {
            // okボタンの活性状態を変更
            this.btnOkEnabledChange();
        }

        /// <summary>
        /// モジュール3チェックボックスのチェック状態変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkModule3_CheckedChanged( object sender, EventArgs e )
        {
            // okボタンの活性状態を変更
            this.btnOkEnabledChange();
        }

        /// <summary>
        /// モジュール4チェックボックスのチェック状態変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkModule4_CheckedChanged( object sender, EventArgs e )
        {
            // okボタンの活性状態を変更
            this.btnOkEnabledChange();
        }

        /// <summary>
        /// okボタン活性状態の変更をおこなう
        /// </summary>
        private void btnOkEnabledChange()
        {
            // すべてのチェックボックスのチェック状態がfalseの場合
            if ( ( this.chkRackTransfer.Checked == false)
                && ( this.chkModule1.Checked == false )
                && ( this.chkModule2.Checked == false )
                && ( this.chkModule3.Checked == false )
                && ( this.chkModule4.Checked == false ))
            {
                // okボタンを非活性化
                btnOk.Enabled = false;
            }
            else
            {
                // okボタンを活性化
                btnOk.Enabled = true;
            }
        }
        #endregion
    }
}
