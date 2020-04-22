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
using Microsoft.VisualBasic.ApplicationServices;
using System.Threading;
using Oelco.CarisX.DB;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Status;
using Oelco.CarisX.Comm;
using System.IO;
using Oelco.CarisX.Common;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// シャットダウンイベント用デリゲート
    /// </summary>
    public delegate void ShutdownEventHandler();
    /// <summary>
    /// システムオプション画面クラス
    /// </summary>
    public partial class FormSystemOption : FormChildBase
    {
        #region [定数定義]

        /// <summary>
        /// ファイル出力
        /// </summary>
        private const String EXECUTE = "Execute";

        /// <summary>
        /// システム初期化
        /// </summary>
        private const String SYSTEM_INITIALIZING = "System Initializing";

        /// <summary>
        /// スレーブプログラムのバージョンアップ
        /// </summary>
        private const String INSTAL_SLAVE_PROGRAM = "Install slave program";

        /// <summary>
        /// 全てのパラメータのバックアップ
        /// </summary>
        private const String BACKUP_ALL_PARAMETERS = "Backup all parameters";

        /// <summary>
        /// 全てのパラメータの呼び出し
        /// </summary>
        private const String RETRIEVE_ALL_PARAMETERS = "Retrieve all parameters";

        /// <summary>
        /// ユーザープログラムのバックアップ
        /// </summary>
        private const String USER_PARAMETER_BACKUP = "User parameters backup";

        /// <summary>
        /// ユーザープログラムの呼び出し
        /// </summary>
        private const String RETRIEVAL_OF_USER_PARAMETERS = "Retrieval of user parameters";

        /// <summary>
        /// メンテナンス日誌
        /// </summary>
        private const String MAINTENANCE_JOURNAL = "Maintenance Journal";

        #endregion

        #region [クラス変数定義]

        /// <summary>
        /// シャットダウンイベントハンドラ
        /// </summary>
        public event ShutdownEventHandler ShutdownEvent;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// システムステータスにより操作の有効状態が切り替わる要素の一覧　　
        /// </summary>
        //private String[] changeStatusItemKeys = new String[] { SYSTEM_INITIALIZING, TROUBLE_COUNTERMEASURE, SYRINGE_UNIT_AGING, INSTAL_SLAVE_PROGRAM, EOP, BACKUP_ALL_PARAMETERS, USER_PARAMETER_BACKUP, };
        /// <summary>
        /// 無接続状態の有効無効設定
        /// </summary>
        private Dictionary<string, Infragistics.Win.DefaultableBoolean> dicStatusItemKeysNoLink = new Dictionary<string, Infragistics.Win.DefaultableBoolean>();
        /// <summary>
        /// 待機中状態の有効無効設定
        /// </summary>
        private Dictionary<string, Infragistics.Win.DefaultableBoolean> dicStatusItemKeysWait = new Dictionary<string, Infragistics.Win.DefaultableBoolean>();
        /// <summary>
        /// 分析中状態の有効無効設定
        /// </summary>
        private Dictionary<string, Infragistics.Win.DefaultableBoolean> dicStatusItemKeysAssay = new Dictionary<string, Infragistics.Win.DefaultableBoolean>();
        /// <summary>
        /// サンプリング停止中状態の有効無効設定
        /// </summary>
        private Dictionary<string, Infragistics.Win.DefaultableBoolean> dicStatusItemKeysStopAssay = new Dictionary<string, Infragistics.Win.DefaultableBoolean>();
        /// <summary>
        /// 試薬交換開始中
        /// </summary>
        private Dictionary<string, Infragistics.Win.DefaultableBoolean> dicStatusItemKeysReagentExchange = new Dictionary<string, Infragistics.Win.DefaultableBoolean>();

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormSystemOption()
        {
            InitializeComponent();

            // コマンドバーのイベント追加
            this.tlbCommandBar.Tools[EXECUTE].ToolClick += (sender, e) => this.execute();

            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SystemStatusChanged, this.onSystemStatusChanged);
            // ユーザレベル変更イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UserLevelChanged, this.setUser);

            // システムステータスにより操作の有効状態が切り替わる要素の一覧を追加
            // 無接続状態
            this.dicStatusItemKeysNoLink.Add(SYSTEM_INITIALIZING, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysNoLink.Add(INSTAL_SLAVE_PROGRAM, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysNoLink.Add(MAINTENANCE_JOURNAL, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysNoLink.Add(BACKUP_ALL_PARAMETERS, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysNoLink.Add(RETRIEVE_ALL_PARAMETERS, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysNoLink.Add(USER_PARAMETER_BACKUP, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysNoLink.Add(RETRIEVAL_OF_USER_PARAMETERS, Infragistics.Win.DefaultableBoolean.True);
            // 待機中状態
            this.dicStatusItemKeysWait.Add(SYSTEM_INITIALIZING, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysWait.Add(INSTAL_SLAVE_PROGRAM, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysWait.Add(MAINTENANCE_JOURNAL, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysWait.Add(BACKUP_ALL_PARAMETERS, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysWait.Add(RETRIEVE_ALL_PARAMETERS, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysWait.Add(USER_PARAMETER_BACKUP, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysWait.Add(RETRIEVAL_OF_USER_PARAMETERS, Infragistics.Win.DefaultableBoolean.True);
            // 分析中状態
            this.dicStatusItemKeysAssay.Add(SYSTEM_INITIALIZING, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysAssay.Add(INSTAL_SLAVE_PROGRAM, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysAssay.Add(MAINTENANCE_JOURNAL, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysAssay.Add(BACKUP_ALL_PARAMETERS, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysAssay.Add(RETRIEVE_ALL_PARAMETERS, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysAssay.Add(USER_PARAMETER_BACKUP, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysAssay.Add(RETRIEVAL_OF_USER_PARAMETERS, Infragistics.Win.DefaultableBoolean.False);
            // サンプリング停止中状態
            this.dicStatusItemKeysStopAssay.Add(SYSTEM_INITIALIZING, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysStopAssay.Add(INSTAL_SLAVE_PROGRAM, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysStopAssay.Add(MAINTENANCE_JOURNAL, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysStopAssay.Add(BACKUP_ALL_PARAMETERS, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysStopAssay.Add(RETRIEVE_ALL_PARAMETERS, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysStopAssay.Add(USER_PARAMETER_BACKUP, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysStopAssay.Add(RETRIEVAL_OF_USER_PARAMETERS, Infragistics.Win.DefaultableBoolean.False);
            // 試薬交換開始中
            this.dicStatusItemKeysReagentExchange.Add(SYSTEM_INITIALIZING, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysReagentExchange.Add(INSTAL_SLAVE_PROGRAM, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysReagentExchange.Add(MAINTENANCE_JOURNAL, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysReagentExchange.Add(BACKUP_ALL_PARAMETERS, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysReagentExchange.Add(RETRIEVE_ALL_PARAMETERS, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysReagentExchange.Add(USER_PARAMETER_BACKUP, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysReagentExchange.Add(RETRIEVAL_OF_USER_PARAMETERS, Infragistics.Win.DefaultableBoolean.False);

            //设置ToolBar的右键功能不可用
            this.tlbCommandBar.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.tlbCommandBar_BeforeToolbarListDropdown);

        }

        //设置ToolBar的右键功能不可用
        private void tlbCommandBar_BeforeToolbarListDropdown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventArgs e)
        {
            e.Cancel = true;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラック搬送バージョンテキストの取得、設定
        /// </summary>
        public String RackTransferVersionText
        {
            get
            {
                return this.lblRackTransferProgramVersion.Text;
            }
            set
            {
                this.lblRackTransferProgramVersion.Text = value;
            }
        }

        /// <summary>
        /// スレーブ１バージョンテキストの取得、設定
        /// </summary>
        public String Slave1VersionText
        {
            get
            {
                return this.lblSlave1ProgramVersion.Text;
            }
            set
            {
                this.lblSlave1ProgramVersion.Text = value;
            }
        }

        /// <summary>
        /// スレーブ２バージョンテキストの取得、設定
        /// </summary>
        public String Slave2VersionText
        {
            get
            {
                return this.lblSlave2ProgramVersion.Text;
            }
            set
            {
                this.lblSlave2ProgramVersion.Text = value;
            }
        }

        /// <summary>
        /// スレーブ３バージョンテキストの取得、設定
        /// </summary>
        public String Slave3VersionText
        {
            get
            {
                return this.lblSlave3ProgramVersion.Text;
            }
            set
            {
                this.lblSlave3ProgramVersion.Text = value;
            }
        }

        /// <summary>
        /// スレーブ４バージョンテキストの取得、設定
        /// </summary>
        public String Slave4VersionText
        {
            get
            {
                return this.lblSlave4ProgramVersion.Text;
            }
            set
            {
                this.lblSlave4ProgramVersion.Text = value;
            }
        }

        /// <summary>
        /// アイテムのバージョンの設定と取得（项目版本号的设定和取得）
        /// </summary>
        public String ProtocolVersionText
        {
            get
            {
                return this.lblProtocolVersion.Text;
            }
            set
            {
                this.lblProtocolVersion.Text = value;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// ファイルまたはディレクトリ、およびその内容を新しい場所にコピーします。
        /// </summary>
        /// <remarks>
        /// ファイルまたはディレクトリ、およびその内容を新しい場所にコピーします。
        /// </remarks>
        /// <param name="stSourcePath">コピー元のディレクトリのパス。</param>
        /// <param name="stDestPath">コピー先のディレクトリのパス。</param>
        public static void CopyDirectory(string stSourcePath, string stDestPath)
        {
            // コピー先のディレクトリがなければ作成する
            if (!System.IO.Directory.Exists(stDestPath))
            {
                System.IO.Directory.CreateDirectory(stDestPath);
                System.IO.File.SetAttributes(stDestPath, System.IO.File.GetAttributes(stSourcePath));
            }

            // コピー元のディレクトリにあるすべてのファイルをコピーする
            foreach (string stCopyFrom in System.IO.Directory.GetFiles(stSourcePath))
            {
                string stCopyTo = System.IO.Path.Combine(stDestPath, System.IO.Path.GetFileName(stCopyFrom));
                try
                {
                    System.IO.File.Copy(stCopyFrom, stCopyTo, true);
                }
                catch (Exception)
                {
                    Singleton<CarisXLogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "Failed[ FileCopy ]:From(" + stCopyFrom + ")/To(" + stCopyTo + ")");
                }
            }

            // コピー元のディレクトリをすべてコピーする (再帰)
            foreach (string stCopyFrom in System.IO.Directory.GetDirectories(stSourcePath))
            {
                string stCopyTo = System.IO.Path.Combine(stDestPath, System.IO.Path.GetFileName(stCopyFrom));
                CopyDirectory(stCopyFrom, stCopyTo);
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
            // タイトル
            this.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_000;

            // ラベル
            this.lblTitleOption.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_001;
            this.lblTitleInformation.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_002;
            this.gbxVersion.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_003;
            this.lblTitleUserProgramVersion.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_004;
            this.lblUserProgramVersion.Text = CarisXConst.USER_PROGRAM_VERSION;
            this.lblTitleRackTransferProgramVersion.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_020;
            this.lblTitleSlave1ProgramVersion.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_005;
            this.lblTitleSlave2ProgramVersion.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_021;
            this.lblTitleSlave3ProgramVersion.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_022;
            this.lblTitleSlave4ProgramVersion.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_023;
            this.lblTitleProtocolVersion.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_019;

            // エクスプローラーバー
            this.exbOption.Groups[0].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_006;
            this.exbOption.Groups[1].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_007;
            this.exbOption.Groups[0].Items[0].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_008;
            this.exbOption.Groups[0].Items[1].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_012;
            this.exbOption.Groups[0].Items[2].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_026;
            this.exbOption.Groups[1].Items[0].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_015;
            this.exbOption.Groups[1].Items[1].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_016;
            this.exbOption.Groups[1].Items[2].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_017;
            this.exbOption.Groups[1].Items[3].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_018;
        }
        /// <summary>
        /// ユーザレベル設定
        /// </summary>
        /// <remarks>
        /// ユーザレベルを設定します
        /// </remarks>
        protected override void setUser(Object value)
        {
            base.setUser(value);

            Boolean flg = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AnalyserParameterSetting);

            this.exbOption.Groups[0].Items[1].Visible = flg;        // Slave Install Program

        }

        #endregion

        #region [privateメソッド]

        #region _コマンドバー_

        /// <summary>
        /// 実行
        /// </summary>
        /// <remarks>
        /// 項目に設定された処理を実行します
        /// </remarks>
        private void execute()
        {
            if (this.exbOption.ActiveItem != null)
            {
                if (this.exbOption.ActiveItem.Settings.Enabled == Infragistics.Win.DefaultableBoolean.False)
                {
                    return;
                }

                switch (this.exbOption.ActiveItem.Key)
                {
                    case FormSystemOption.SYSTEM_INITIALIZING:          // システム初期化
                        this.systemInitializing();
                        break;
                    case FormSystemOption.INSTAL_SLAVE_PROGRAM:         // スレーブバージョンアップ
                        this.installSlaveProgram();
                        break;
                    case FormSystemOption.MAINTENANCE_JOURNAL:          // メンテナンス日誌
                        this.maintenanceJournal();
                        break;
                    case FormSystemOption.BACKUP_ALL_PARAMETERS:        // 全てのパラメータのバックアップ
                        this.backupAllParameters();
                        break;
                    case FormSystemOption.RETRIEVE_ALL_PARAMETERS:      // 全てのパラメータの呼び出し
                        this.retrieveAllParameters();
                        break;
                    case FormSystemOption.USER_PARAMETER_BACKUP:        // ユーザーパラメータのバックアップ
                        this.backupUserParameter();
                        break;
                    case FormSystemOption.RETRIEVAL_OF_USER_PARAMETERS: // ユーザーパラメータの呼び出し
                        this.retrievalOfUserParameters();
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        /// <summary>
        /// システム初期化
        /// </summary>
        /// <remarks>
        /// システム初期化ダイアログ表示します
        /// </remarks>
        private void systemInitializing()
        {
            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_031 });

            // TODO:システム全体の初期化、自己診断、プライムを実行
            // G1200でダイアログ出してるので合わせたが、起動時の処理呼ぶだけでよい？
            DlgOptionSystemInitializing dlg = new DlgOptionSystemInitializing();
            dlg.ShowDialog();
        }

        /// <summary>
        /// USBポート設定
        /// </summary>
        /// <remarks>
        /// USBポート設定します
        /// </remarks>
        private void settingUSB()
        {
            // TODO:仕様書に記載無いが不要？
        }

        /// <summary>
        /// スレーブバージョンアップ
        /// </summary>
        /// <remarks>
        /// スレーブインストーラを起動し、アプリケーションを終了します
        /// </remarks>
        private void installSlaveProgram()
        {
            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_035 });

            DlgOptionVersionUp dlg = new DlgOptionVersionUp();

            dlg.ShowDialog();
        }

        /// <summary>
        /// メンテナンス日誌
        /// </summary>
        /// <remarks>
        /// メンテナンス日誌ダイアログ表示します
        /// </remarks>
        private void maintenanceJournal()
        {
            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist
                                                     , Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                                                     , CarisXLogInfoBaseExtention.Empty
                                                     , new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_092 });

            Singleton<Oelco.CarisX.Common.MaintenanceJournalInfoManager>.Instance.SetMaintenanceJournalType(MaintenanceJournalType.User);
            // メンテナンス日誌開くかチェックします
            string userParamPath = Singleton<CarisXMaintenanceUserParameter>.Instance.SavePath;
            if (Singleton<Oelco.CarisX.Common.MaintenanceJournalInfoManager>.Instance.IsShowSelect())
            {
                // ファイルが開いている場合、値の変更ができないため選択画面に遷移させません
                if (Singleton<DataHelper>.Instance.CheckFileOpen(userParamPath))
                {
                    // メンテナンス日誌選択画面を表示
                    Oelco.CarisX.GUI.DlgOptionReconfirmMaintenanceJournal dlgMaintenanceSelect = new Oelco.CarisX.GUI.DlgOptionReconfirmMaintenanceJournal(MaintenanceJournalType.User);
                    if (DialogResult.OK == dlgMaintenanceSelect.ShowDialog())
                    {
                        // クリア対象なしの状態で全てのチェックが完了していない場合、前回状態を表示させる
                        Oelco.CarisX.GUI.DlgMaintenanceList dlgMaintenanceList = new Oelco.CarisX.GUI.DlgMaintenanceList(MaintenanceJournalType.User);
                        dlgMaintenanceList.ShowDialog();
                    }
                }
                else
                {
                    // メンテナンス日誌画面のロードに失敗しました。パラメータファイルを閉じてください。
                    Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_019));
                }
            }
        }

        /// <summary>
        /// 全てのパラメータのバックアップ
        /// </summary>
        /// <remarks>
        /// 全てのパラメータのバックアップします
        /// </remarks>
        private void backupAllParameters()
        {
            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_012 + CarisX.Properties.Resources.STRING_LOG_MSG_038 });

            // TODO:実装

            // フォルダ選択ダイアログを表示して保存先フォルダを選択する(G1200はParam,Protocol,System)
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult folderRet = folderBrowserDialog.ShowDialog();
            if (folderRet != DialogResult.OK)
            {
                return;
            }
            // 選択したフォルダへパラメータを保存する（保存先に同名ファイルがある場合、上書きする）
            DialogResult msgRet = DlgMessage.Show(String.Empty,
                                                    CarisX.Properties.Resources.STRING_DLG_MSG_183 + "\n" + CarisX.Properties.Resources.STRING_DLG_MSG_184,
                                                    CarisX.Properties.Resources.STRING_DLG_TITLE_001,
                                                    MessageDialogButtons.OKCancel
                                                );
            if (msgRet != DialogResult.OK)
            {
                return;
            }
            // 保存
            String selectedPath = folderBrowserDialog.SelectedPath + "\\";
            String appPath = SubFunction.GetApplicationDirectory();

            String path = CarisXConst.PathParam.Replace(appPath, selectedPath);
            CopyDirectory(CarisXConst.PathParam, path);

            path = CarisXConst.PathSystem.Replace(appPath, selectedPath);
            CopyDirectory(CarisXConst.PathSystem, path);

            path = CarisXConst.PathProtocol.Replace(appPath, selectedPath);
            CopyDirectory(CarisXConst.PathProtocol, path);
        }

        /// <summary>
        /// 全てのパラメータの呼び出し
        /// </summary>
        /// <remarks>
        /// 全てのパラメータの呼び出しします
        /// </remarks>
        private void retrieveAllParameters()
        {
            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_039 });

            // フォルダ選択ダイアログを表示して読込元先フォルダを選択する(G1200はParam,Protocol,System)
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult folderRet = folderBrowserDialog.ShowDialog();
            if (folderRet != DialogResult.OK)
            {
                return;
            }
            String selectedPath = folderBrowserDialog.SelectedPath + "\\";
            String appPath = SubFunction.GetApplicationDirectory();
            Boolean isPathParam = false;
            String pathParam = CarisXConst.PathParam.Replace(appPath, selectedPath);
            Boolean isPathSystem = false;
            String pathSystem = CarisXConst.PathSystem.Replace(appPath, selectedPath);
            Boolean isPathProtocol = false;
            String pathProtocol = CarisXConst.PathProtocol.Replace(appPath, selectedPath);
            String[] subFolders = System.IO.Directory.GetDirectories(folderBrowserDialog.SelectedPath, "*", System.IO.SearchOption.AllDirectories);
            foreach (String strDir in subFolders)
            {
                if (strDir == pathParam)
                {
                    isPathParam = true;
                }
                if (strDir == pathSystem)
                {
                    isPathSystem = true;
                }
                if (strDir == pathProtocol)
                {
                    isPathProtocol = true;
                }
            }

            if (isPathParam && isPathSystem && isPathProtocol)
            {
                // 選択したフォルダからパラメータを読込、保存する（保存先に同名ファイルがある場合、上書きする）
                DialogResult msgRet = DlgMessage.Show(String.Empty,
                                                        CarisX.Properties.Resources.STRING_DLG_MSG_185,
                                                        CarisX.Properties.Resources.STRING_DLG_TITLE_001,
                                                        MessageDialogButtons.OKCancel
                                                    );
                if (msgRet == DialogResult.OK)
                {
                    CopyDirectory(pathParam, CarisXConst.PathParam);
                    CopyDirectory(pathSystem, CarisXConst.PathSystem);
                    CopyDirectory(pathProtocol, CarisXConst.PathProtocol);

                    // ファイル更新後、アプリ終了
                    msgRet = DlgMessage.Show(String.Empty,
                                                CarisX.Properties.Resources.STRING_DLG_MSG_186 + "\n" + CarisX.Properties.Resources.STRING_DLG_MSG_187,
                                                CarisX.Properties.Resources.STRING_DLG_TITLE_001,
                                                MessageDialogButtons.OKCancel
                                            );
                    if (msgRet == DialogResult.OK)
                    {
                        Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SystemCondition = true;

                        // 正常終了状態保存
                        if (!Singleton<ParameterFilePreserve<AppSettings>>.Instance.Save())
                        {
                            // 失敗時
                            System.Diagnostics.Debug.WriteLine(String.Format("AppSettings保存に失敗しました。 {0}", Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SavePath));
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                          String.Format("AppSettings保存に失敗しました。 {0}", Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SavePath));
                        }

                        //Singleton<SystemStatus>.Instance.Status = SystemStatusKind.NoLink;
                        Singleton<SystemStatus>.Instance.setAllModuleStatus(SystemStatusKind.NoLink);

                        //ファイルを更新後はユーザーアプリを終了する
                        //※ユーザーパラメータの呼び出し終了時の処理と異なる。理由が不明な為、Caris200のままとしておく。
                        Singleton<CarisXCommManager>.Instance.DisConnect();

                        Environment.Exit(0);
                    }
                }
            }
            else
            {
                DialogResult msgRet = DlgMessage.Show(String.Empty,
                                                        CarisX.Properties.Resources.STRING_DLG_MSG_188,
                                                        CarisX.Properties.Resources.STRING_DLG_TITLE_001,
                                                        MessageDialogButtons.OK
                                                    );
            }
        }

        /// <summary>
        /// ユーザーパラメータのバックアップ
        /// </summary>
        /// <remarks>
        /// ユーザーパラメータのバックアップします
        /// </remarks>
        private void backupUserParameter()
        {
            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_040 });

            // フォルダ選択ダイアログを表示して保存先フォルダを選択する(G1200はProtocol,System)
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult folderRet = folderBrowserDialog.ShowDialog();
            if (folderRet != DialogResult.OK)
            {
                return;
            }

            // 選択したフォルダへパラメータを保存する（保存先に同名ファイルがある場合、上書きする）
            DialogResult msgRet = DlgMessage.Show(String.Empty,
                                                    CarisX.Properties.Resources.STRING_DLG_MSG_183 + "\n" + CarisX.Properties.Resources.STRING_DLG_MSG_184,
                                                    CarisX.Properties.Resources.STRING_DLG_TITLE_001,
                                                    MessageDialogButtons.OKCancel
                                                );
            if (msgRet != DialogResult.OK)
            {
                return;
            }

            // systemフォルダ作成
            String selectedPath = folderBrowserDialog.SelectedPath + "\\";
            String appPath = SubFunction.GetApplicationDirectory();
            String path = CarisXConst.PathSystem.Replace(appPath, selectedPath);
            System.IO.Directory.CreateDirectory(path);
            // ファイルがないときコピーしない
            String filepath = Singleton<ParameterFilePreserve<ControlQC>>.Instance.Param.SavePath;
            String filename = System.IO.Path.GetFileName(filepath);
            if (System.IO.File.Exists(filepath))
            {
                // 既存ファイルの上書きを許可
                System.IO.File.Copy(filepath, path + "\\" + filename, true);
            }
            filepath = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SavePath;
            filename = System.IO.Path.GetFileName(filepath);
            if (System.IO.File.Exists(filepath))
            {
                // 既存ファイルの上書きを許可
                System.IO.File.Copy(filepath, path + "\\" + filename, true);
            }

            // Protocolフォルダコピー
            path = CarisXConst.PathProtocol.Replace(appPath, selectedPath);
            CopyDirectory(CarisXConst.PathProtocol, path);
        }

        /// <summary>
        /// ユーザーパラメータの呼び出し
        /// </summary>
        /// <remarks>
        /// ユーザーパラメータの呼び出しします
        /// </remarks>
        private void retrievalOfUserParameters()
        {
            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_041 });

            // TODO:実装

            // フォルダ選択ダイアログを表示して読込元先フォルダを選択する(G1200はProtocol,System)
            // フォルダ選択ダイアログを表示して読込元先フォルダを選択する(G1200はParam,Protocol,System)
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult folderRet = folderBrowserDialog.ShowDialog();
            if (folderRet != DialogResult.OK)
            {
                return;
            }
            String selectedPath = folderBrowserDialog.SelectedPath + "\\";
            String appPath = SubFunction.GetApplicationDirectory();
            Boolean isPathSystem = false;
            String pathSystem = CarisXConst.PathSystem.Replace(appPath, selectedPath);
            Boolean isPathProtocol = false;
            String pathProtocol = CarisXConst.PathProtocol.Replace(appPath, selectedPath);
            String[] subFolders = System.IO.Directory.GetDirectories(folderBrowserDialog.SelectedPath, "*", System.IO.SearchOption.AllDirectories);
            foreach (String strDir in subFolders)
            {
                if (strDir == pathSystem)
                {
                    isPathSystem = true;
                }
                if (strDir == pathProtocol)
                {
                    isPathProtocol = true;
                }
            }
            if (isPathSystem && isPathProtocol)
            {
                // 選択したフォルダからパラメータを読込、保存する
                // ユーザレベル1-3までで変更可能な項目取得などして、各項目を個別の取得、設定する?G1200より
                // 選択したフォルダからパラメータを読込、保存する（保存先に同名ファイルがある場合、上書きする）
                DialogResult msgRet = DlgMessage.Show(String.Empty,
                                                        CarisX.Properties.Resources.STRING_DLG_MSG_185,
                                                        CarisX.Properties.Resources.STRING_DLG_TITLE_001,
                                                        MessageDialogButtons.OKCancel
                                                    );
                if (msgRet == DialogResult.OK)
                {
                    // XMLからのデータ読み込み
                    String path = CarisXConst.PathSystem.Replace(appPath, selectedPath);
                    String filepath = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SavePath;
                    String filename = System.IO.Path.GetFileName(filepath);
                    String srcpath = path + "\\" + filename;
                    if (System.IO.File.Exists(srcpath))
                    {
                        // SystemParameter.xmlの読み込み
                        ParameterFilePreserve<CarisXSystemParameter> systemParam = new ParameterFilePreserve<CarisXSystemParameter>();
                        systemParam.Param.SavePath = srcpath;
                        systemParam.Load();

                        foreach (CarisXUserLevelManagedAction action in Enum.GetValues(typeof(CarisXUserLevelManagedAction)))
                        {
                            // 現ユーザーレベルでの各機能利用可能別読み込み判定
                            if (Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(action))
                            {
                                switch (action)
                                {
                                    case CarisXUserLevelManagedAction.ReagentPreparation:
                                        // 起動時の残量チェック設定 取得/設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ReagentCheckAtStartUpParameter =
                                            systemParam.Param.ReagentCheckAtStartUpParameter;
                                        break;
                                    case CarisXUserLevelManagedAction.RegistSample:
                                        // ホスト設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter =
                                            systemParam.Param.HostParameter;
                                        break;
                                    //case CarisXUserLevelManagedAction.Assay:
                                    //    break;
                                    //case CarisXUserLevelManagedAction.DataReOutput:
                                    //break;
                                    case CarisXUserLevelManagedAction.SystemParameterSetup:
                                        // 自動プライミング設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoPrimeParameter =
                                            systemParam.Param.AutoPrimeParameter;
                                        // 自動シャットダウン
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoShutdownParameter =
                                            systemParam.Param.AutoShutdownParameter;
                                        // // 自動起動設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter =
                                            systemParam.Param.AutoStartupTimerParameter;
                                        // 装置No.設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.DeviceNoParameter =
                                            systemParam.Param.DeviceNoParameter;
                                        // エラー音、警告音設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter =
                                            systemParam.Param.ErrWarningBeepParameter;
                                        // フラッシュプライミング設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.FlushParameter =
                                            systemParam.Param.FlushParameter;
                                        // シーケンス番号発番方法
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HowToCreateSequenceNoParameter =
                                            systemParam.Param.HowToCreateSequenceNoParameter;
                                        // ソフトウエアキーボード設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.KeyBoardParameter =
                                            systemParam.Param.KeyBoardParameter;
                                        // 測定結果ファイル作成設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.MeasurementResultFileParameter =
                                            systemParam.Param.MeasurementResultFileParameter;
                                        // プリンタ設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter =
                                            systemParam.Param.PrinterParameter;
                                        // 検体吸引エラー後の処理設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ProcessAfterSampleAspiratingErrorParameter =
                                            systemParam.Param.ProcessAfterSampleAspiratingErrorParameter;
                                        // 希釈液不足時の分析状態設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ProcessAtDiluentShortageParameter =
                                            systemParam.Param.ProcessAtDiluentShortageParameter;
                                        // 試薬ロット切替わり時の処理設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ProcessAtReagentLotChange =
                                            systemParam.Param.ProcessAtReagentLotChange;
                                        // 試薬不足時の分析の状況設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ProcessAtReagentShortageParameter =
                                            systemParam.Param.ProcessAtReagentShortageParameter;
                                        // ラックID割り当て設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter =
                                            systemParam.Param.RackIDDefinitionParameter;
                                        // 検体バーコードリーダー設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter =
                                            systemParam.Param.SampleBCRParameter;
                                        // サンプルラック架設部カバーオープンエラー通知時間設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleLoaderCoverOpenErrorNotificationTimeParameter =
                                            systemParam.Param.SampleLoaderCoverOpenErrorNotificationTimeParameter;
                                        // 警告灯使用有無設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WarningLightParameter =
                                            systemParam.Param.WarningLightParameter;
                                        // 泡検知有無
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.BubbleCheckParameter =
                                            systemParam.Param.BubbleCheckParameter;
                                        break;
                                    case CarisXUserLevelManagedAction.MeasureProtocolSetting:
                                        // 分析方式設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter =
                                            systemParam.Param.AssayModeParameter;
                                        break;
                                    //case CarisXUserLevelManagedAction.QualityControl:
                                    //    break;
                                    //case CarisXUserLevelManagedAction.UserMaintenance:
                                    //    break;
                                    //case CarisXUserLevelManagedAction.CalibratorEditRecalc:
                                    //    break;
                                    case CarisXUserLevelManagedAction.SystemParameterSetupDetail:
                                        // サイクルタイム設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CycleTimeParameter =
                                            systemParam.Param.CycleTimeParameter;
                                        // 測光設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PhotometryParameter =
                                            systemParam.Param.PhotometryParameter;
                                        // プライム設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter =
                                            systemParam.Param.PrimeParameter;
                                        // 温度設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter =
                                            systemParam.Param.TemperatureParameter;
                                        // 洗浄、分注設定
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashDispVolParameter =
                                            systemParam.Param.WashDispVolParameter;
                                        break;
                                    //case CarisXUserLevelManagedAction.SampleDataEditDelete:
                                    //    break;
                                    //case CarisXUserLevelManagedAction.UserManage:
                                    //    break;
                                    //case CarisXUserLevelManagedAction.AnalyserParameterSetting:
                                    //    break;
                                    //case CarisXUserLevelManagedAction.Maintenance:
                                    //    break;
                                    //case CarisXUserLevelManagedAction.MeasureProtocolSettingDetail:
                                    //    break;
                                    //case CarisXUserLevelManagedAction.MeasureProtocolAdd:
                                    //    break;
                                    default:
                                        break;
                                }
                            }
                        }
                        // XMLへ保存
                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Save();
                    }

                    MeasureProtocolManager measureProtocolManager = new MeasureProtocolManager();
                    foreach (var protocolList in measureProtocolManager.MeasureProtocolList)
                    {
                        // 読み込み先のパスを変更
                        protocolList.SetSaveProtocolPath(pathProtocol);
                    }
                    if ((measureProtocolManager.LoadAllMeasureProtocol()) &&
                        (measureProtocolManager.MeasureProtocolList.Count == Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList.Count))
                    {
                        for (Int32 cnt = 0; cnt < measureProtocolManager.MeasureProtocolList.Count; ++cnt)
                        {

                            foreach (CarisXUserLevelManagedAction action in Enum.GetValues(typeof(CarisXUserLevelManagedAction)))
                            {
                                // 現ユーザーレベルでの各機能利用可能別読み込み判定
                                if (Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(action))
                                {
                                    switch (action)
                                    {
                                        //case CarisXUserLevelManagedAction.ReagentPreparation:
                                        //    break;
                                        //case CarisXUserLevelManagedAction.RegistSample:
                                        //    break;
                                        //case CarisXUserLevelManagedAction.Assay:
                                        //    break;
                                        //case CarisXUserLevelManagedAction.DataReOutput:
                                        //    break;
                                        //case CarisXUserLevelManagedAction.SystemParameterSetup:
                                        //    break;
                                        case CarisXUserLevelManagedAction.MeasureProtocolSetting:
                                            // 分析項目番号
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].ProtocolNo =
                                                measureProtocolManager.MeasureProtocolList[cnt].ProtocolNo;
                                            // 検体多重測定回数
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].RepNoForSample =
                                                measureProtocolManager.MeasureProtocolList[cnt].RepNoForSample;
                                            // 試薬開封後有効期間
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].DayOfReagentValid =
                                                measureProtocolManager.MeasureProtocolList[cnt].DayOfReagentValid;
                                            // 精度管理検体多重測定回数
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].RepNoForControl =
                                                measureProtocolManager.MeasureProtocolList[cnt].RepNoForControl;
                                            // キャリブレータ多重測定回数
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].RepNoForCalib =
                                                measureProtocolManager.MeasureProtocolList[cnt].RepNoForCalib;
                                            // 検量線有効期間
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].ValidityOfCurve =
                                                measureProtocolManager.MeasureProtocolList[cnt].ValidityOfCurve;
                                            // 陽性判定閾値
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].PosiLine =
                                                measureProtocolManager.MeasureProtocolList[cnt].PosiLine;
                                            // 陰性判定閾値
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].NegaLine =
                                                measureProtocolManager.MeasureProtocolList[cnt].NegaLine;
                                            break;
                                        //case CarisXUserLevelManagedAction.QualityControl:
                                        //    break;
                                        //case CarisXUserLevelManagedAction.UserMaintenance:
                                        //    break;
                                        //case CarisXUserLevelManagedAction.CalibratorEditRecalc:
                                        //    break;
                                        //case CarisXUserLevelManagedAction.SystemParameterSetupDetail:
                                        //    break;
                                        //case CarisXUserLevelManagedAction.SampleDataEditDelete:
                                        //    break;
                                        //case CarisXUserLevelManagedAction.UserManage:
                                        //    break;
                                        //case CarisXUserLevelManagedAction.AnalyserParameterSetting:
                                        //    break;
                                        //case CarisXUserLevelManagedAction.Maintenance:
                                        //    break;
                                        case CarisXUserLevelManagedAction.MeasureProtocolSettingDetail:
                                            // アッセイシーケンス
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].AssaySequence =
                                                measureProtocolManager.MeasureProtocolList[cnt].AssaySequence;
                                            // 前処理シーケンス
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].PreProcessSequence =
                                                measureProtocolManager.MeasureProtocolList[cnt].PreProcessSequence;
                                            // サンプル種別
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].SampleKind =
                                                measureProtocolManager.MeasureProtocolList[cnt].SampleKind;
                                            // 自動希釈再検使用有無
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].UseAfterDil =
                                                measureProtocolManager.MeasureProtocolList[cnt].UseAfterDil;
                                            // 自動再検使用有無
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].UseAutoReTest =
                                                measureProtocolManager.MeasureProtocolList[cnt].UseAutoReTest;
                                            // 自動希釈再検条件
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].AutoDilutionReTest =
                                                measureProtocolManager.MeasureProtocolList[cnt].AutoDilutionReTest;
                                            // 自動希釈再検条件(希釈倍率)
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].AutoDilutionReTestRatio =
                                                measureProtocolManager.MeasureProtocolList[cnt].AutoDilutionReTestRatio;
                                            // 自動再検条件
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].AutoReTest =
                                                measureProtocolManager.MeasureProtocolList[cnt].AutoReTest;
                                            // 手希釈使用有無
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].UseManualDil =
                                                measureProtocolManager.MeasureProtocolList[cnt].UseManualDil;
                                            // 濃度ダイナミックレンジ
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].ConcDynamicRange =
                                                measureProtocolManager.MeasureProtocolList[cnt].ConcDynamicRange;
                                            // 相関係数A
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].GainOfCorrelation =
                                                measureProtocolManager.MeasureProtocolList[cnt].GainOfCorrelation;
                                            // 相関係数B
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].OffsetOfCorrelation =
                                                measureProtocolManager.MeasureProtocolList[cnt].OffsetOfCorrelation;
                                            // 多重測定内乖離限界 CV%
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].MulMeasDevLimitCV =
                                                measureProtocolManager.MeasureProtocolList[cnt].MulMeasDevLimitCV;
                                            // 濃度単位
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].ConcUnit =
                                                measureProtocolManager.MeasureProtocolList[cnt].ConcUnit;
                                            // 濃度値小数点以下桁数
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].LengthAfterDemPoint =
                                                measureProtocolManager.MeasureProtocolList[cnt].LengthAfterDemPoint;
                                            // キャリブレーションタイプ
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].CalibType =
                                                measureProtocolManager.MeasureProtocolList[cnt].CalibType;
                                            // フルキャリブレーションポイント数
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].NumOfMeasPointInCalib =
                                                measureProtocolManager.MeasureProtocolList[cnt].NumOfMeasPointInCalib;
                                            // キャリブレーション方法
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].CalibMethod =
                                                measureProtocolManager.MeasureProtocolList[cnt].CalibMethod;
                                            // 濃度
                                            for (Int32 i = 0; i < measureProtocolManager.MeasureProtocolList[cnt].ConcsOfEach.Length; ++i)
                                            {
                                                Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].ConcsOfEach[i] =
                                                    measureProtocolManager.MeasureProtocolList[cnt].ConcsOfEach[i];
                                            }
                                            // 測定ポイント
                                            for (Int32 i = 0; i < measureProtocolManager.MeasureProtocolList[cnt].CalibMeasPointOfEach.Length; ++i)
                                            {
                                                Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].CalibMeasPointOfEach[i] =
                                                    measureProtocolManager.MeasureProtocolList[cnt].CalibMeasPointOfEach[i];
                                            }
                                            // カウント範囲
                                            for (Int32 i = 0; i < measureProtocolManager.MeasureProtocolList[cnt].CountRangesOfEach.Length; ++i)
                                            {
                                                Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].CountRangesOfEach[i] =
                                                    measureProtocolManager.MeasureProtocolList[cnt].CountRangesOfEach[i];
                                            }
                                            // サンプル分注量
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].SmpDispenseVolume =
                                                measureProtocolManager.MeasureProtocolList[cnt].SmpDispenseVolume;
                                            // M試薬分注量
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].MReagDispenseVolume =
                                                measureProtocolManager.MeasureProtocolList[cnt].MReagDispenseVolume;
                                            // R1試薬分注量
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].R1DispenseVolume =
                                                measureProtocolManager.MeasureProtocolList[cnt].R1DispenseVolume;
                                            // R2試薬分注量
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].R2DispenseVolume =
                                                measureProtocolManager.MeasureProtocolList[cnt].R2DispenseVolume;
                                            // 前処理液１分注量
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].PreProsess1DispenseVolume =
                                                measureProtocolManager.MeasureProtocolList[cnt].PreProsess1DispenseVolume;
                                            // 前処理液２分注量
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].PreProsess2DispenseVolume =
                                                measureProtocolManager.MeasureProtocolList[cnt].PreProsess2DispenseVolume;

                                            ///////////////////

                                            // 分析項目番号
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].ProtocolNo =
                                                measureProtocolManager.MeasureProtocolList[cnt].ProtocolNo;
                                            // 分析項目インデックス
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].ProtocolIndex =
                                                measureProtocolManager.MeasureProtocolList[cnt].ProtocolIndex;
                                            // 分析項目名称
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].ProtocolName =
                                                measureProtocolManager.MeasureProtocolList[cnt].ProtocolName;
                                            // 試薬名称
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].ReagentName =
                                                measureProtocolManager.MeasureProtocolList[cnt].ReagentName;
                                            // 試薬コード
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].ReagentCode =
                                                measureProtocolManager.MeasureProtocolList[cnt].ReagentCode;
                                            //// 項目パラメータA
                                            //Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].CoefAOfLog =
                                            //    measureProtocolManager.MeasureProtocolList[cnt].CoefAOfLog;
                                            //// 項目パラメータB
                                            //Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].CoefBOfLog =
                                            //    measureProtocolManager.MeasureProtocolList[cnt].CoefBOfLog;
                                            //// サンプル表示容量
                                            //Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].SmpDispVol =
                                            //    measureProtocolManager.MeasureProtocolList[cnt].SmpDispVol;
                                            // 自動希釈倍率演算可否
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].UseAfterDilAtCalcu =
                                                measureProtocolManager.MeasureProtocolList[cnt].UseAfterDilAtCalcu;
                                            // 手希釈倍率演算可否
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].UseManualDilAtCalcu =
                                                measureProtocolManager.MeasureProtocolList[cnt].UseManualDilAtCalcu;
                                            //// 抑制率上限値
                                            //Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].INHCutOffForPos =
                                            //    measureProtocolManager.MeasureProtocolList[cnt].INHCutOffForPos;
                                            //// 抑制率下限値
                                            //Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].INHCutOffForNega =
                                            //    measureProtocolManager.MeasureProtocolList[cnt].INHCutOffForNega;
                                            // 係数A
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].Coef_A =
                                                measureProtocolManager.MeasureProtocolList[cnt].Coef_A;
                                            // 係数B
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].Coef_B =
                                                measureProtocolManager.MeasureProtocolList[cnt].Coef_B;
                                            // 係数C
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].Coef_C =
                                                measureProtocolManager.MeasureProtocolList[cnt].Coef_C;
                                            // 係数D
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].Coef_D =
                                                measureProtocolManager.MeasureProtocolList[cnt].Coef_D;
                                            // 係数E
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].Coef_E =
                                                measureProtocolManager.MeasureProtocolList[cnt].Coef_E;
                                            // 校准品、质控品是否稀释
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].DiluCalibOrControl =
                                                measureProtocolManager.MeasureProtocolList[cnt].DiluCalibOrControl;

                                            break;
                                        case CarisXUserLevelManagedAction.MeasureProtocolAdd:
                                            // 分析項目使用フラグ
                                            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList[cnt].DisplayProtocol =
                                                measureProtocolManager.MeasureProtocolList[cnt].DisplayProtocol;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                        Singleton<MeasureProtocolManager>.Instance.SaveAllMeasureProtocol();
                    }

                    path = CarisXConst.PathSystem.Replace(appPath, selectedPath);
                    filepath = Singleton<ParameterFilePreserve<ControlQC>>.Instance.Param.SavePath;
                    filename = System.IO.Path.GetFileName(filepath);
                    srcpath = path + "\\" + filename;
                    if (System.IO.File.Exists(srcpath) && Singleton<ParameterFilePreserve<ControlQC>>.Instance.Load())
                    {
                        ParameterFilePreserve<ControlQC> controlQC = new ParameterFilePreserve<ControlQC>();
                        controlQC.Param.SavePath = srcpath;
                        controlQC.Load();
                        // 精度管理
                        if (Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.QualityControl))
                        {
                            Singleton<ParameterFilePreserve<ControlQC>>.Instance.Param.ControlQCList.Clear();
                            foreach (var QCList in controlQC.Param.ControlQCList)
                            {
                                Singleton<ParameterFilePreserve<ControlQC>>.Instance.Param.ControlQCList.Add(QCList);
                            }
                        }
                        // 保存
                        Singleton<ParameterFilePreserve<ControlQC>>.Instance.Save();
                    }

                    // ファイル更新後、アプリ終了
                    msgRet = DlgMessage.Show(String.Empty,
                                                CarisX.Properties.Resources.STRING_DLG_MSG_186 + "\n" + CarisX.Properties.Resources.STRING_DLG_MSG_187,
                                                CarisX.Properties.Resources.STRING_DLG_TITLE_001,
                                                MessageDialogButtons.OKCancel
                                            );
                    if (msgRet == DialogResult.OK)
                    {
                        // ファイル更新後、PCをシャットダウン?G1200より
                        Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SystemCondition = true;
                        // 正常終了状態保存
                        if (!Singleton<ParameterFilePreserve<AppSettings>>.Instance.Save())
                        {
                            // 失敗時
                            System.Diagnostics.Debug.WriteLine(String.Format("AppSettings保存に失敗しました。 {0}", Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SavePath));
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                          String.Format("AppSettings保存に失敗しました。 {0}", Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SavePath));
                        }

                        //ファイルを更新後はＰＣをシャットダウンする
                        //※全パラメータの呼び出し終了時の処理と異なる。理由が不明な為、Caris200のままとしておく。
                        if (ShutdownEvent != null)
                        {
                            ShutdownEvent();
                        }
                    }
                }

            }
            else
            {
                DialogResult msgRet = DlgMessage.Show(String.Empty,
                                                        CarisX.Properties.Resources.STRING_DLG_MSG_188,
                                                        CarisX.Properties.Resources.STRING_DLG_TITLE_001,
                                                        MessageDialogButtons.OK
                                                    );
            }
        }

        /// <summary>
        /// ダブルクリックイベント
        /// </summary>
        /// <remarks>
        /// 選択された項目の処理を実行します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exbOption_ItemDoubleClick(object sender, Infragistics.Win.UltraWinExplorerBar.ItemEventArgs e)
        {
            // 実行
            this.execute();
        }

        /// <summary>
        /// システムステータス変化イベント
        /// </summary>
        /// <remarks>
        /// システムステータスにより項目の動作可否を切替します
        /// </remarks>
        /// <param name="value"></param>
        private void onSystemStatusChanged(Object value)
        {
            foreach (var group in this.exbOption.Groups)
            {
                foreach (var item in group.Items)
                {
                    if (Singleton<SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId(Singleton<PublicMemory>.Instance.moduleIndex)] == SystemStatusKind.ReagentExchange)
                    {
                        if (this.dicStatusItemKeysReagentExchange.ContainsKey(item.Key))
                        {
                            item.Settings.Enabled = this.dicStatusItemKeysReagentExchange[item.Key];
                        }
                    }
                    else
                    {
                        switch (Singleton<SystemStatus>.Instance.Status)
                        {
                            case SystemStatusKind.NoLink:  //無接続状態
                                if (this.dicStatusItemKeysNoLink.ContainsKey(item.Key))
                                {
                                    item.Settings.Enabled = this.dicStatusItemKeysNoLink[item.Key];
                                }
                                break;
                            case SystemStatusKind.Standby:   // 待機中状態
                                if (this.dicStatusItemKeysWait.ContainsKey(item.Key))
                                {
                                    item.Settings.Enabled = this.dicStatusItemKeysWait[item.Key];
                                }
                                break;
                            case SystemStatusKind.Assay:  // 分析中状態
                            case SystemStatusKind.WaitSlaveResponce:
                            case SystemStatusKind.ToEndAssay: // 分析終了移行中
                                if (this.dicStatusItemKeysAssay.ContainsKey(item.Key))
                                {
                                    item.Settings.Enabled = this.dicStatusItemKeysAssay[item.Key];
                                }
                                break;
                            case SystemStatusKind.SamplingPause:  // サンプリング停止中状態
                                if (this.dicStatusItemKeysStopAssay.ContainsKey(item.Key))
                                {
                                    item.Settings.Enabled = this.dicStatusItemKeysStopAssay[item.Key];
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// グループを縮小した際に、ヘッダ部の画像を「＋」型のものに変更する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exbOption_GroupCollapsed(object sender, Infragistics.Win.UltraWinExplorerBar.GroupEventArgs e)
        {
            e.Group.Settings.AppearancesSmall.HeaderAppearance.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_GroupExpandLong;
        }

        /// <summary>
        /// グループを展開した際に、ヘッダ部の画像を「－」型のものに変更する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exbOption_GroupExpanded(object sender, Infragistics.Win.UltraWinExplorerBar.GroupEventArgs e)
        {
            e.Group.Settings.AppearancesSmall.HeaderAppearance.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_GroupCollapseLong;
        }

        #endregion
    }
}
