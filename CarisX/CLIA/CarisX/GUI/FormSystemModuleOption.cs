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
    /// システムオプション画面クラス
    /// </summary>
    public partial class FormSystemModuleOption : FormChildBase
    {
        #region [定数定義]

        /// <summary>
        /// ファイル出力
        /// </summary>
        private const String EXECUTE = "Execute";

        /// <summary>
        /// 消耗品の確認
        /// </summary>
        private const String CHECK_CONSUMABLES = "Check consumables";

        /// <summary>
        /// 障害対策
        /// </summary>
        private const String TROUBLE_COUNTERMEASURE = "Trouble countermeasure";

        /// <summary>
        /// 分注器エージング
        /// </summary>
        private const String SYRINGE_UNIT_AGING = "Syringe unit aging";

        /// <summary>
        /// END処理
        /// </summary>
        private const String EOP = "EOP";

        /// <summary>
        /// アナライザ温度
        /// </summary>
        private const String ANALYZER_TEMPERATURE = "Analyzer temperature";

        /// <summary>
        /// 試薬バーコード読込切り替え
        /// </summary>
        private const String CHANGE_READ_REAGENT_BC = "Change read reagent BC";

        /// <summary>
        /// 試薬バーコード手入力
        /// </summary>
        private const String INPUT_REAGENT_BC = "Input reagent BC";

        #endregion

        #region [クラス変数定義]

        #endregion

        #region [インスタンス変数定義]


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
        public FormSystemModuleOption()
        {
            InitializeComponent();

            // コマンドバーのイベント追加
            this.tlbCommandBar.Tools[EXECUTE].ToolClick += (sender, e) => this.execute();

            //システムステータス変更イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SystemStatusChanged, this.onSystemStatusChanged);

            // ユーザレベル変更イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UserLevelChanged, this.setUser);

            // スレーブバージョン通知イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SlaveVersion, this.slaveVersionCommand);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.RackTransferVersion, this.rackTransferVersionCommand);

            // モジュール変更通知イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ModuleChange, this.SelectedModuleChanged);

            // システムステータスにより操作の有効状態が切り替わる要素の一覧を追加
            // 無接続状態
            this.dicStatusItemKeysNoLink.Add(CHECK_CONSUMABLES, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysNoLink.Add(TROUBLE_COUNTERMEASURE, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysNoLink.Add(SYRINGE_UNIT_AGING, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysNoLink.Add(EOP, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysNoLink.Add(ANALYZER_TEMPERATURE, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysNoLink.Add(CHANGE_READ_REAGENT_BC, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysNoLink.Add(INPUT_REAGENT_BC, Infragistics.Win.DefaultableBoolean.True);
            // 待機中状態
            this.dicStatusItemKeysWait.Add(CHECK_CONSUMABLES, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysWait.Add(TROUBLE_COUNTERMEASURE, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysWait.Add(SYRINGE_UNIT_AGING, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysWait.Add(EOP, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysWait.Add(ANALYZER_TEMPERATURE, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysWait.Add(CHANGE_READ_REAGENT_BC, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysWait.Add(INPUT_REAGENT_BC, Infragistics.Win.DefaultableBoolean.True);
            // 分析中状態
            this.dicStatusItemKeysAssay.Add(CHECK_CONSUMABLES, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysAssay.Add(TROUBLE_COUNTERMEASURE, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysAssay.Add(SYRINGE_UNIT_AGING, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysAssay.Add(EOP, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysAssay.Add(ANALYZER_TEMPERATURE, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysAssay.Add(CHANGE_READ_REAGENT_BC, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysAssay.Add(INPUT_REAGENT_BC, Infragistics.Win.DefaultableBoolean.False);
            // サンプリング停止中状態
            this.dicStatusItemKeysStopAssay.Add(CHECK_CONSUMABLES, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysStopAssay.Add(TROUBLE_COUNTERMEASURE, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysStopAssay.Add(SYRINGE_UNIT_AGING, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysStopAssay.Add(EOP, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysStopAssay.Add(ANALYZER_TEMPERATURE, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysStopAssay.Add(CHANGE_READ_REAGENT_BC, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysStopAssay.Add(INPUT_REAGENT_BC, Infragistics.Win.DefaultableBoolean.True);
            // 試薬交換開始中
            this.dicStatusItemKeysReagentExchange.Add(CHECK_CONSUMABLES, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysReagentExchange.Add(TROUBLE_COUNTERMEASURE, Infragistics.Win.DefaultableBoolean.True);
            this.dicStatusItemKeysReagentExchange.Add(SYRINGE_UNIT_AGING, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysReagentExchange.Add(EOP, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysReagentExchange.Add(ANALYZER_TEMPERATURE, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysReagentExchange.Add(CHANGE_READ_REAGENT_BC, Infragistics.Win.DefaultableBoolean.False);
            this.dicStatusItemKeysReagentExchange.Add(INPUT_REAGENT_BC, Infragistics.Win.DefaultableBoolean.False);

            // ツールバーの右ボタン使用不可
            this.tlbCommandBar.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.tlbCommandBar_BeforeToolbarListDropdown);

        }

        /// <summary> 
        /// ツールバーの右ボタン使用不可
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// スレーブバージョンテキストの取得、設定
        /// </summary>
        public String SlaveVersionText
        {
            get
            {
                return this.lblSlaveProgramVersion.Text;
            }
            set
            {
                this.lblSlaveProgramVersion.Text = value;
            }
        }


        /// <summary>
        /// スレーブ1バージョンテキストの取得、設定
        /// </summary>
        private String Slave1VersionText = String.Empty;

        /// <summary>
        /// スレーブ２バージョンテキストの取得、設定
        /// </summary>
        private String Slave2VersionText = String.Empty;

        /// <summary>
        /// スレーブ３バージョンテキストの取得、設定
        /// </summary>
        private String Slave3VersionText = String.Empty;

        /// <summary>
        /// スレーブ４バージョンテキストの取得、設定
        /// </summary>
        private String Slave4VersionText = String.Empty;

        /// <summary>
        /// 分析項目パラメータのバージョンの設定と取得
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

            // コマンドバー
            this.tlbCommandBar.Tools[EXECUTE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_022;

            // ラベル
            this.lblTitleOption.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_001;
            this.lblTitleInformation.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_002;
            this.gbxVersion.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_003;
            this.lblTitleUserProgramVersion.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_004;
            this.lblUserProgramVersion.Text = CarisXConst.USER_PROGRAM_VERSION;
            this.lblTitleRackTransferProgramVersion.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_020;
            this.lblRackTransferProgramVersion.Text = Singleton<FormSystemOption>.Instance.RackTransferVersionText;
            this.lblTitleSlaveProgramVersion.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_005;
            this.lblSlaveProgramVersion.Text = Singleton<FormSystemOption>.Instance.Slave1VersionText;
            this.lblTitleProtocolVersion.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_019;
            this.lblProtocolVersion.Text = Singleton<FormSystemOption>.Instance.ProtocolVersionText;

            // エクスプローラーバー
            this.exbOption.Groups[0].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_006;
            this.exbOption.Groups[0].Items[0].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_009;
            this.exbOption.Groups[0].Items[1].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_010;
            this.exbOption.Groups[0].Items[2].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_011;
            this.exbOption.Groups[0].Items[3].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_013;
            this.exbOption.Groups[0].Items[4].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_014;
            this.exbOption.Groups[0].Items[5].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_025;
            this.exbOption.Groups[0].Items[6].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMOPTION_024;
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

            this.exbOption.Groups[0].Items[1].Visible = flg;    // 障害対策
            this.exbOption.Groups[0].Items[2].Visible = flg;    // シリンジエージング
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
                    case FormSystemModuleOption.CHECK_CONSUMABLES:            // 消耗品の確認
                        this.checkConsumables();
                        break;
                    case FormSystemModuleOption.TROUBLE_COUNTERMEASURE:       // 障害対策
                        this.troubleCountermeasure();
                        break;
                    case FormSystemModuleOption.SYRINGE_UNIT_AGING:           // 分注器エージング
                        this.syringeUnitAging();
                        break;
                    case FormSystemModuleOption.EOP:                          // END処理
                        this.eop();
                        break;
                    case FormSystemModuleOption.ANALYZER_TEMPERATURE:         // アナライザ温度
                        this.analyzerTemperature();
                        break;
                    case FormSystemModuleOption.CHANGE_READ_REAGENT_BC:       // 試薬バーコード読込切り替え
                        this.changeReadReagentBC();
                        break;
                    case FormSystemModuleOption.INPUT_REAGENT_BC:             // 試薬バーコード手入力
                        this.inputReagentBC();
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        /// <summary>
        /// 消耗品の確認
        /// </summary>
        /// <remarks>
        /// 消耗品の確認ダイアログを表示します
        /// </remarks>
        private void checkConsumables()
        {
            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_032 });

            // ダイアログ表示
            DlgOptionCheckConsumables dlg = new DlgOptionCheckConsumables();
            dlg.ShowDialog();
        }

        /// <summary>
        /// 障害対策
        /// </summary>
        /// <remarks>
        /// 障害対策画面表示します
        /// </remarks>
        private void troubleCountermeasure()
        {
            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_033 });

            // ダイアログ表示
            DlgOptionTroubleCountermeaure dlg = new DlgOptionTroubleCountermeaure();
            dlg.ShowDialog();

            // Maintenance.FormSensorUnitで保存されたパラメータを再読込みする。
            Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance.LoadRaw();
        }

        /// <summary>
        /// 分注器エージング
        /// </summary>
        /// <remarks>
        /// 分注器エージングダイアログ表示します
        /// </remarks>
        private void syringeUnitAging()
        {
            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_034 });

            // TODO:実装
            // ダイアログ表示
            DlgOptionSyringeAging dlg = new DlgOptionSyringeAging();
            dlg.ShowDialog();
        }

        /// <summary>
        /// EOP処理
        /// </summary>
        /// <remarks>
        /// EOP処理ダイアログを表示します
        /// </remarks>
        private void eop()
        {
            // 操作履歴登録

            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_036 });

            // 装置の配管をリンス液で充填する
            // ダイアログ表示
            DlgOptionEOP dlg = new DlgOptionEOP();

            dlg.ShowDialog();
        }

        /// <summary>
        /// アナライザ温度
        /// </summary>
        /// <remarks>
        /// アナライザ温度ダイアログ表示します
        /// </remarks>
        private void analyzerTemperature()
        {
            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_037 });

            // ダイアログ表示
            DlgOptionAnalyzerTemperature dlg = new DlgOptionAnalyzerTemperature();

            dlg.ShowDialog();
        }

        /// <summary>
        /// 試薬バーコード読込切り替え
        /// </summary>
        /// <remarks>
        /// 試薬バーコード読込切り替えダイアログ表示します
        /// </remarks>
        private void changeReadReagentBC()
        {
            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_031 });

            DlgOptionChangeReadReagentBC dlg = new DlgOptionChangeReadReagentBC();
            dlg.ShowDialog();
        }

        /// <summary>
        /// 試薬バーコード手入力
        /// </summary>
        /// <remarks>
        /// 試薬バーコード手入力ダイアログ表示します
        /// </remarks>
        private void inputReagentBC()
        {
            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_031 });

            DlgOptionInputReagentBC dlg = new DlgOptionInputReagentBC();
            dlg.ShowDialog();
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

        /// <summary>
        /// 画面読込時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormSystemModuleOption_Load(object sender, EventArgs e)
        {
            SetProgramVersion();
        }

        /// <summary>
        /// 選択されているモジュールが変更された際に実行
        /// </summary>
        /// <param name="value"></param>
        private void SelectedModuleChanged(Object value)
        {
            SetProgramVersion();
        }

        /// <summary>
        /// スレーブバージョン通知イベント
        /// </summary>
        /// <remarks>
        /// スレーブバージョンをシステムオプション画面に設定します
        /// </remarks>
        /// <param name="value"></param>
        private void slaveVersionCommand(Object value)
        {
            // スレーブバージョン設定
            if (value is SlaveCommCommand_0511)
            {
                SlaveCommCommand_0511 cmd0511 = (SlaveCommCommand_0511)value;
                switch (CarisXSubFunction.MachineCodeToRackModuleIndex((MachineCode)cmd0511.CommNo))
                {
                    case (int)RackModuleIndex.Module1:
                        Slave1VersionText = cmd0511.Version;
                        break;
                    case (int)RackModuleIndex.Module2:
                        Slave2VersionText = cmd0511.Version;
                        break;
                    case (int)RackModuleIndex.Module3:
                        Slave3VersionText = cmd0511.Version;
                        break;
                    case (int)RackModuleIndex.Module4:
                        Slave4VersionText = cmd0511.Version;
                        break;
                }
            }
            else
            {
                Slave1VersionText = (String)value;
                Slave2VersionText = (String)value;
                Slave3VersionText = (String)value;
                Slave4VersionText = (String)value;
            }

            SetProgramVersion();
        }

        /// <summary>
        /// ラック搬送バージョン通知イベント
        /// </summary>
        /// <remarks>
        /// ラック搬送バージョンをシステムオプション画面に設定します
        /// </remarks>
        /// <param name="value"></param>
        private void rackTransferVersionCommand(Object value)
        {
            // ラック搬送バージョン設定
            if (value is RackTransferCommCommand_0111)
            {
                RackTransferCommCommand_0111 cmd0111 = (RackTransferCommCommand_0111)value;
                RackTransferVersionText = cmd0111.Version;
            }
            else
            {
                RackTransferVersionText = (String)value;
            }
        }

        /// <summary>
        /// プログラムバージョンを設定
        /// </summary>
        private void SetProgramVersion()
        {
            // 現在選択されているモジュールによって表示切り替え
            switch (Singleton<PublicMemory>.Instance.moduleIndex)
            {
                case ModuleIndex.Module1:
                    this.SlaveVersionText = this.Slave1VersionText;
                    this.lblTitleSlaveProgramVersion.Text = Properties.Resources.STRING_SYSTEMOPTION_005;
                    break;

                case ModuleIndex.Module2:
                    this.SlaveVersionText = this.Slave2VersionText;
                    this.lblTitleSlaveProgramVersion.Text = Properties.Resources.STRING_SYSTEMOPTION_021;
                    break;

                case ModuleIndex.Module3:
                    this.SlaveVersionText = this.Slave3VersionText;
                    this.lblTitleSlaveProgramVersion.Text = Properties.Resources.STRING_SYSTEMOPTION_022;
                    break;

                case ModuleIndex.Module4:
                    this.SlaveVersionText = this.Slave4VersionText;
                    this.lblTitleSlaveProgramVersion.Text = Properties.Resources.STRING_SYSTEMOPTION_023;
                    break;

                default:
                    // Slave1を設定する
                    this.SlaveVersionText = this.Slave1VersionText;
                    this.lblTitleSlaveProgramVersion.Text = Properties.Resources.STRING_SYSTEMOPTION_005;
                    break;
            }
        }

        #endregion
    }
}
