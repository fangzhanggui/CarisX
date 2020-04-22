using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.DB;
using Oelco.Common.Utility;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Const;
using Oelco.CarisX.Status;
using Infragistics.Win.UltraWinExplorerBar;
using Oelco.Common.Comm;
using Oelco.CarisX.Common;
using System.Diagnostics;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// システム構成画面クラス
    /// </summary>
    public partial class FormSystemConfiguration : FormChildBase
    {
        #region [定数定義]

        /// <summary>
        /// 削除
        /// </summary>
        public const String DELETE = "Delete";

        /// <summary>
        /// 追加
        /// </summary>
        public const String ADD = "Add";

        /// <summary>
        /// パラメータ変更
        /// </summary>
        public const String EDIT_PARAMETERS = "Edit parameters";

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormSystemConfiguration()
        {
            InitializeComponent();

            // コマンドバーのイベント追加
            this.tlbCommandBar.Tools[EDIT_PARAMETERS].ToolClick += (sender, e) => this.editParameter();
            this.tlbCommandBar.Tools[ADD].ToolClick += (sender, e) => this.addOption();
            this.tlbCommandBar.Tools[DELETE].ToolClick += (sender, e) => this.deleteConfig();

            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SystemStatusChanged, this.onSystemStatusChanged);
            // ユーザレベル変更イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UserLevelChanged, this.setUser);
            this.onSystemStatusChanged(null);

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
            // 画面左側の設定
            Int32 groupIdx;
            Int32 itemIdx;

            //////////////////////////////////////////////////////////
            // 装置に関する設定
            //////////////////////////////////////////////////////////
            groupIdx = 0;
            itemIdx = 0;
            // 検体バーコードリーダー使用有無
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysSampleBC).FullName;
            itemIdx++;
            // 自動起動タイマー使用有無
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysAutoSetUpTimer).FullName;
            itemIdx++;
            // 自動シャットダウン使用有無
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysAutoShutDown).FullName;
            itemIdx++;
            // 自動プライム使用有無
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysAutoPrime).FullName;
            itemIdx++;
            // 警告灯使用有無
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysWarninglight).FullName;
            itemIdx++;
            // フラッシュプライム使用有無
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysFlashPrime).FullName;
            itemIdx++;
            // 装置No.
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysDeviceNo).FullName;
            itemIdx++;
            // ソフトウエアーキーボード
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysKeyBoard).FullName;
            itemIdx++;
            // エラー音、警告音
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysWarningSound).FullName;
            itemIdx++;
            // 起動時の残量チェック
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysCheckRemainAtStartUp).FullName;
            itemIdx++;
            // サンプルラック架設部カバーオープンエラー通知時間
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysRackCoverNotificationTime).FullName;
            itemIdx++;
            // 洗浄液の外部からの供給
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysWashSolutionFromExterior).FullName;
            itemIdx++;
            // 泡検知
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysBubble).FullName;
            itemIdx++;
            // 搬送ライン使用有無
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysTransferSystem).FullName;
            itemIdx++;
            // 分析モジュール接続台数
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysAssayModuleConnect).FullName;
            itemIdx++;

            //////////////////////////////////////////////////////////
            // 登録に関する設定
            //////////////////////////////////////////////////////////
            groupIdx++;
            itemIdx = 0;
            // ラックID割り当て
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysRackAssign).FullName;
            itemIdx++;
            // シーケンス番号発番方法
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysSequenceNo).FullName;
            itemIdx++;

            //////////////////////////////////////////////////////////
            // 外部出力に関する設定
            //////////////////////////////////////////////////////////
            groupIdx++;
            itemIdx = 0;
            // ホスト使用有無
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysHost).FullName;
            itemIdx++;
            // 測定結果ファイル作成有無
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysMeasurementResultFile).FullName;
            itemIdx++;
            // プリンタ使用有無
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysPrinter).FullName;
            itemIdx++;
            // 2020-02-27 CarisX IoT Add [START]
            // IoT使用有無
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysIoT).FullName;
#if !NOT_USE_IOT
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Visible = true;
#else
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Visible = false;
#endif
            itemIdx++;
            // 2020-02-27 CarisX IoT Add [END]


            //////////////////////////////////////////////////////////
            // 分析に関する設定（詳細設定）
            //////////////////////////////////////////////////////////
            groupIdx++;
            itemIdx = 0;
            // プライム
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysAnaPrime).FullName;
            itemIdx++;
            // サイクル時間
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysCycleTime).FullName;
            itemIdx++;
            // 温度
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysTemp).FullName;
            itemIdx++;
            // 洗浄、分注
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysWashDispense).FullName;
            itemIdx++;
            // 測光
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysPhotometry).FullName;
            itemIdx++;

            //////////////////////////////////////////////////////////
            // 分析に関する設定（一般設定）
            //////////////////////////////////////////////////////////
            groupIdx++;
            itemIdx = 0;
            // 試薬不足時の分析の状態
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysReagentShortSupply).FullName;
            itemIdx++;
            // 希釈液不足時の分析の状態
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysDilutedLiquidShortSupply).FullName;
            itemIdx++;
            // 分析方式
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysAnalysisMethod).FullName;
            itemIdx++;
            // 検体吸引エラー後の処理
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysSmplSuckingUpErr).FullName;
            itemIdx++;
            // 試薬ロット切替わり時の処理
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysReagentLotChange).FullName;
            itemIdx++;
            // 校准方式选择 add by marxsu
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgCheckCalibrationMode).FullName;
            itemIdx++;
            // ラック移動方式
            exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key = typeof(DlgSysRackMovementMethod).FullName;
            itemIdx++;

            //////////////////////////////////////////////////////////
            // オプションカタログ（画面右側）の設定
            //////////////////////////////////////////////////////////
            ////////////////////////////
            // 装置に関する設定
            ////////////////////////////
            Int32 groupIdxOpList;
            Int32 itemIdxOpList;
            groupIdxOpList = 0;
            itemIdxOpList = 0;
            // 検体バーコードリーダー使用有無
            exbOptionList.Groups[groupIdxOpList].Items[itemIdxOpList].Key = typeof(DlgSysSampleBC).FullName;
            itemIdxOpList++;
            // 自動起動タイマー使用有無
            exbOptionList.Groups[groupIdxOpList].Items[itemIdxOpList].Key = typeof(DlgSysAutoSetUpTimer).FullName;
            itemIdxOpList++;
            // 自動シャットダウン使用有無
            exbOptionList.Groups[groupIdxOpList].Items[itemIdxOpList].Key = typeof(DlgSysAutoShutDown).FullName;
            itemIdxOpList++;
            // 自動プライム使用有無
            exbOptionList.Groups[groupIdxOpList].Items[itemIdxOpList].Key = typeof(DlgSysAutoPrime).FullName;
            itemIdxOpList++;
            // 警告灯使用有無
            exbOptionList.Groups[groupIdxOpList].Items[itemIdxOpList].Key = typeof(DlgSysWarninglight).FullName;
            itemIdxOpList++;
            // ソフトウエアーキーボード
            exbOptionList.Groups[groupIdxOpList].Items[itemIdxOpList].Key = typeof(DlgSysKeyBoard).FullName;
            itemIdxOpList++;
            // 洗浄液の外部からの供給
            exbOptionList.Groups[groupIdxOpList].Items[itemIdxOpList].Key = typeof(DlgSysWashSolutionFromExterior).FullName;
            itemIdxOpList++;
            //搬送ライン使用有無
            exbOptionList.Groups[groupIdxOpList].Items[itemIdxOpList].Key = typeof(DlgSysTransferSystem).FullName;
            itemIdxOpList++;

            ////////////////////////////
            // 外部出力に関する設定
            ////////////////////////////
            groupIdxOpList++;
            itemIdxOpList = 0;
            // ホスト使用有無
            exbOptionList.Groups[groupIdxOpList].Items[itemIdxOpList].Key = typeof(DlgSysHost).FullName;
            itemIdxOpList++;
            // 測定結果ファイル作成有無
            exbOptionList.Groups[groupIdxOpList].Items[itemIdxOpList].Key = typeof(DlgSysMeasurementResultFile).FullName;
            itemIdxOpList++;
            // プリンタ使用有無
            exbOptionList.Groups[groupIdxOpList].Items[itemIdxOpList].Key = typeof(DlgSysPrinter).FullName;
            itemIdxOpList++;
            // 2020-02-27 CarisX IoT Add [START]
            // IoT使用有無
            exbOptionList.Groups[groupIdxOpList].Items[itemIdxOpList].Key = typeof(DlgSysIoT).FullName;
#if !NOT_USE_IOT
            exbOptionList.Groups[groupIdxOpList].Items[itemIdxOpList].Visible = true;
#else
            exbOptionList.Groups[groupIdxOpList].Items[itemIdxOpList].Visible = false;
#endif
            itemIdxOpList++;
            // 2020-02-27 CarisX IoT Add [END]

            // 項目の表示更新
            refreshConfigDisplay();
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
            this.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_044;

            // コマンドバー
            this.tlbCommandBar.Tools[EDIT_PARAMETERS].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_020;
            this.tlbCommandBar.Tools[ADD].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_021;
            this.tlbCommandBar.Tools[DELETE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_002;

            // ラベル
            this.lblTitleCurrentConfiguration.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_000;
            this.lblTitleOptionList.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_001;

            // エクスプローラーバー
            this.exbCurrentConfiguration.Groups[0].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_002;
            this.exbCurrentConfiguration.Groups[1].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_003;
            this.exbCurrentConfiguration.Groups[2].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_004;
            this.exbCurrentConfiguration.Groups[3].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_005;
            this.exbCurrentConfiguration.Groups[4].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_006;
            this.exbCurrentConfiguration.Groups[0].Items[0].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_007;
            this.exbCurrentConfiguration.Groups[0].Items[1].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_008;
            this.exbCurrentConfiguration.Groups[0].Items[2].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_009;
            this.exbCurrentConfiguration.Groups[0].Items[3].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_010;
            this.exbCurrentConfiguration.Groups[0].Items[4].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_011;
            this.exbCurrentConfiguration.Groups[0].Items[5].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_012;
            this.exbCurrentConfiguration.Groups[0].Items[6].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_013;
            this.exbCurrentConfiguration.Groups[0].Items[7].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_014;
            this.exbCurrentConfiguration.Groups[0].Items[8].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_015;
            this.exbCurrentConfiguration.Groups[0].Items[9].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_016;
            this.exbCurrentConfiguration.Groups[0].Items[10].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_017;
            this.exbCurrentConfiguration.Groups[0].Items[11].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_047;
            this.exbCurrentConfiguration.Groups[0].Items[12].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_048;
            this.exbCurrentConfiguration.Groups[0].Items[13].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_050;
            this.exbCurrentConfiguration.Groups[0].Items[14].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_051;

            this.exbCurrentConfiguration.Groups[1].Items[0].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_018;
            this.exbCurrentConfiguration.Groups[1].Items[1].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_019;
            this.exbCurrentConfiguration.Groups[2].Items[0].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_020;
            this.exbCurrentConfiguration.Groups[2].Items[1].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_021;
            this.exbCurrentConfiguration.Groups[2].Items[2].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_022;
            this.exbCurrentConfiguration.Groups[2].Items[3].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_053;
            this.exbCurrentConfiguration.Groups[3].Items[0].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_023;
            this.exbCurrentConfiguration.Groups[3].Items[1].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_024;
            this.exbCurrentConfiguration.Groups[3].Items[2].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_025;
            this.exbCurrentConfiguration.Groups[3].Items[3].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_026;
            this.exbCurrentConfiguration.Groups[3].Items[4].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_027;
            this.exbCurrentConfiguration.Groups[4].Items[0].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_028;
            this.exbCurrentConfiguration.Groups[4].Items[1].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_029;
            this.exbCurrentConfiguration.Groups[4].Items[2].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_030;
            this.exbCurrentConfiguration.Groups[4].Items[3].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_031;
            this.exbCurrentConfiguration.Groups[4].Items[4].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_032;
            this.exbCurrentConfiguration.Groups[4].Items[5].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_049;
            this.exbCurrentConfiguration.Groups[4].Items[6].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_052;

            this.exbOptionList.Groups[0].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_033;
            this.exbOptionList.Groups[1].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_034;

            this.exbOptionList.Groups[0].Items[0].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_035;
            this.exbOptionList.Groups[0].Items[1].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_036;
            this.exbOptionList.Groups[0].Items[2].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_037;
            this.exbOptionList.Groups[0].Items[3].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_038;
            this.exbOptionList.Groups[0].Items[4].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_039;
            this.exbOptionList.Groups[0].Items[5].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_040;
            this.exbOptionList.Groups[0].Items[6].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_047;
            this.exbOptionList.Groups[0].Items[7].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_050;

            this.exbOptionList.Groups[1].Items[0].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_041;
            this.exbOptionList.Groups[1].Items[1].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_042;
            this.exbOptionList.Groups[1].Items[2].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_043;
            this.exbOptionList.Groups[1].Items[3].Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_053;

        }

        #endregion

        #region [privateメソッド]

        #region _コマンドバー_

        /// <summary>
        /// 項目の削除
        /// </summary>
        /// <remarks>
        /// 項目を削除します
        /// </remarks>
        private void deleteConfig()
        {
            if (exbCurrentConfiguration.ActiveItem != null)
            {
                // 操作履歴：削除実行
                String activeItem = Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_045
                                    + Type.GetType(exbCurrentConfiguration.ActiveItem.Key).Name
                                    + Oelco.CarisX.Properties.Resources.STRING_SYSTEMCONFIGURATION_046;

                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_051 + activeItem });

                // 未使用に設定
                DlgCarisXBaseSys dlg = (DlgCarisXBaseSys)Activator.CreateInstance(Type.GetType(exbCurrentConfiguration.ActiveItem.Key));
                dlg.UseConfig = false;

                //スレーブへのシステムパラメータコマンド送信
                sendSystemParameter();

                // 項目の表示更新
                refreshConfigDisplay();
            }
        }

        /// <summary>
        /// 項目の追加
        /// </summary>
        /// <remarks>
        /// 項目を追加します
        /// </remarks>
        private void addOption()
        {
            Singleton<OperationLogDB>.Instance.CommitOperationLog();

            if (exbOptionList.ActiveItem != null)
            {
                addOption(Type.GetType(exbOptionList.ActiveItem.Key));
            }
        }
        /// <summary>
        /// 項目の追加
        /// </summary>
        /// <remarks>
        /// 項目を追加します
        /// </remarks>
        /// <param name="key"></param>
        private void addOption(Type key)
        {
            // 操作履歴
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_028 + String.Format("({0})", key.Name) });

            // 使用に設定
            DlgCarisXBaseSys dlg = (DlgCarisXBaseSys)Activator.CreateInstance(key);

            dlg.UseConfig = true;

            //スレーブへのシステムパラメータコマンド送信
            sendSystemParameter();

            // 項目の表示更新
            refreshConfigDisplay();
        }

        /// <summary>
        /// パラメータ編集
        /// </summary>
        /// <remarks>
        /// パラメータ編集します
        /// </remarks>
        private void editParameter()
        {
            if (exbCurrentConfiguration.ActiveItem != null)
            {
                // 操作履歴
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { CarisX.Properties.Resources.STRING_LOG_MSG_019 + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + Type.GetType(exbCurrentConfiguration.ActiveItem.Key).Name });

                // ユーザレベルによる表示可否判断
                if (exbCurrentConfiguration.ActiveItem.Group.Index == 3)
                {
                    // 画面表示権限がない場合、エラーメッセージ表示
                    if (!Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.SystemParameterSetupDetail))
                    {
                        // 画面を表示する権限がありません。
                        DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_189, "", CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OK);
                        return;
                    }
                }
                // ダイアログ表示
                DlgCarisXBaseSys dlg = (DlgCarisXBaseSys)Activator.CreateInstance(Type.GetType(exbCurrentConfiguration.ActiveItem.Key));

                dlg.ShowDialog();

                // 項目の表示更新
                refreshConfigDisplay();
                if (dlg.DialogResult == DialogResult.OK)
                {
                    string configItemKey = exbCurrentConfiguration.ActiveItem.Key;

                    //スレーブへのシステムパラメータコマンド送信
                    sendSystemParameter();

                    // 操作履歴
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
                        , new String[] { this.Text + Properties.Resources.STRING_COMMON_013 + Type.GetType(configItemKey).Name 
                        + Properties.Resources.STRING_COMMON_013 + Properties.Resources.STRING_LOG_MSG_050 });

                    //分析モジュールの接続台数を変更された場合、ユーザーアプリを終了する
                    if (configItemKey == typeof(DlgSysAssayModuleConnect).FullName)
                    {
                        // アプリ終了
                        DialogResult msgRet = DlgMessage.Show(String.Empty,
                                                    Properties.Resources.STRING_DLG_MSG_186 + "\n" + Properties.Resources.STRING_DLG_MSG_187,
                                                    Properties.Resources.STRING_DLG_TITLE_001,
                                                    MessageDialogButtons.OKCancel
                                                );
                        if (msgRet == DialogResult.OK)
                        {
                            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SystemCondition = true;

                            // 正常終了状態保存
                            if (!Singleton<ParameterFilePreserve<AppSettings>>.Instance.Save())
                            {
                                // 失敗時
                                Debug.WriteLine(String.Format("AppSettings保存に失敗しました。 {0}", Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SavePath));
                                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                              String.Format("AppSettings保存に失敗しました。 {0}", Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SavePath));
                            }

                            Singleton<SystemStatus>.Instance.setAllModuleStatus(SystemStatusKind.NoLink);

                            Singleton<CarisXCommManager>.Instance.DisConnect();

                            Environment.Exit(0);
                        }
                    }
                }
                else
                {
                    // 操作履歴 
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + Type.GetType(exbCurrentConfiguration.ActiveItem.Key).Name + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_049 });
                }
            }
        }

        #endregion

        /// <summary>
        /// 項目の表示更新
        /// </summary>
        /// <remarks>
        /// 項目の表示更新します
        /// </remarks>
        private void refreshConfigDisplay()
        {
            // 使用、未使用の状態で表示、非表示を決める（オプションリストに無い項目は対象外）
            for (Int32 groupIdx = 0; groupIdx < exbCurrentConfiguration.Groups.Count; groupIdx++)
            {
                for (Int32 itemIdx = 0; itemIdx < exbCurrentConfiguration.Groups[groupIdx].Items.Count; itemIdx++)
                {
                    for (Int32 groupIdxOpList = 0; groupIdxOpList < exbOptionList.Groups.Count; groupIdxOpList++)
                    {
                        if (exbOptionList.Groups[groupIdxOpList].GetItemsByKey(exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key) != null)
                        {
                            DlgCarisXBaseSys dlg = (DlgCarisXBaseSys)Activator.CreateInstance(Type.GetType(exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Key));
                            if (dlg.UseConfig == true)
                            {
                                if (exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Visible == false)
                                {
                                    // 追加になった場合は、展開表示する
                                    exbCurrentConfiguration.Groups[groupIdx].Expanded = true;
                                    exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Visible = true;
                                }
                            }
                            else
                            {
                                exbCurrentConfiguration.Groups[groupIdx].Items[itemIdx].Visible = false;
                            }
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ドラッグオーバーイベント
        /// </summary>
        /// <remarks>
        /// ドラッグされているデータがstring型であれば操作受け入れします
        /// </remarks>
        private void exbCurrentConfiguration_DragOver(object sender, DragEventArgs e)
        {
            //ドラッグされているデータがstring型か調べる
            if (e.Data.GetDataPresent(typeof(String)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                //Stringでなければ受け入れない
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// ドラッグ＆ドロップイベント
        /// </summary>
        /// <remarks>
        /// ドラッグ＆ドロップされたデータがstring型であれば項目追加します
        /// </remarks>
        private void exbCurrentConfiguration_DragDrop(object sender, DragEventArgs e)
        {
            // システムステータスが画面操作不可状態の場合は処理を終了する
            if (!GetEnabledStatus())
            {
                return;
            }

            //ドロップされたデータがStringか調べる
            if (e.Data.GetDataPresent(typeof(String)))
            {
                //ドロップされたデータ(string型)を取得
                String itemText = (String)e.Data.GetData(typeof(String));
                //ドロップされたデータから
                // 左側の対応する項目を表示する
                for (Int32 groupIdx = 0; groupIdx < exbCurrentConfiguration.Groups.Count; groupIdx++)
                {
                    if (exbCurrentConfiguration.Groups[groupIdx].GetItemsByKey(itemText) != null)
                    {
                        addOption(Type.GetType(itemText));
                    }
                }
            }
        }
        /// <summary>
        /// マウスダウン座標
        /// </summary>
        private Point mouseDownPoint = Point.Empty;
        /// <summary>
        /// オプションリストマウスダウンイベント
        /// </summary>
        /// <remarks>
        /// マウスの左ボタンだけが押されている時ドラッグ操作の準備をします
        /// </remarks>
        private void exbOptionList_MouseDown(object sender, MouseEventArgs e)
        {
            // マウスの左ボタンだけが押されている時のみドラッグできるようにする
            if (e.Button == MouseButtons.Left)
            {
                // ドラッグの準備
                if (exbOptionList.ActiveItem != null)
                {
                    mouseDownPoint = new Point(e.X, e.Y);
                }
                else
                {
                    mouseDownPoint = Point.Empty;
                }
            }
        }

        /// <summary>
        /// オプションリストマウスアップイベント
        /// </summary>
        /// <remarks>
        /// マウスダウン座標クリアします
        /// </remarks>
        private void exbOptionList_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDownPoint = Point.Empty;
        }

        /// <summary>
        /// オプションリストマウスムーブイベント
        /// </summary>
        /// <remarks>
        /// ドラッグ＆ドロップの条件が成立していればドラッグ＆ドロップ操作をします
        /// </remarks>
        private void exbOptionList_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDownPoint != Point.Empty)
            {
                //ドラッグとしないマウスの移動範囲を取得する
                Rectangle moveRect = new Rectangle(
                    mouseDownPoint.X - SystemInformation.DragSize.Width / 2,
                    mouseDownPoint.Y - SystemInformation.DragSize.Height / 2,
                    SystemInformation.DragSize.Width,
                    SystemInformation.DragSize.Height);
                //ドラッグとする移動範囲を超えたか調べる
                if (!moveRect.Contains(e.X, e.Y))
                {
                    //ドラッグの準備
                    if (exbOptionList.ActiveItem != null)
                    {
                        //ドラッグするアイテムの内容を取得する
                        String itemText = exbOptionList.ActiveItem.Key;
                        //ドラッグ&ドロップ処理を開始する
                        DragDropEffects dde = exbOptionList.DoDragDrop(itemText, DragDropEffects.All);
                    }
                    mouseDownPoint = Point.Empty;
                }
            }
        }

        /// <summary>
        /// オプションリストドラッグ中に発生するイベント
        /// </summary>
        /// <remarks>
        /// ドラッグ＆ドロップ中にカーソル表示を変更します
        /// </remarks>
        private void exbOptionList_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            //ドロップ効果にあわせてカーソルを指定する
            if ((e.Effect & DragDropEffects.Move) == DragDropEffects.Move)
            {
                e.UseDefaultCursors = false;
                Cursor.Current = Cursors.Hand;
            }
            else
            {
                e.UseDefaultCursors = true;
            }
        }

        /// <summary>
        /// ダブルクリックで発生するイベント
        /// </summary>
        /// <remarks>
        /// パラメータ編集します
        /// </remarks>
        private void exbCurrentConfiguration_ItemDoubleClick(object sender, Infragistics.Win.UltraWinExplorerBar.ItemEventArgs e)
        {
            this.editParameter();
        }

        /// <summary>
        /// システムステータス変更時処理
        /// </summary>
        /// <remarks>
        /// システムステータスにより追加、削除ボタンの動作可否を切替します
        /// </remarks>
        /// <param name="value"></param>
        private void onSystemStatusChanged(Object value)
        {
            this.tlbCommandBar.Tools[ADD].SharedProps.Enabled = GetEnabledStatus();
            this.tlbCommandBar.Tools[DELETE].SharedProps.Enabled = GetEnabledStatus();
        }

        /// <summary>
        /// システムステータスによるEnableを返す
        /// </summary>
        /// <remarks>
        /// システムステータスによるEnableを返します
        /// </remarks>
        /// <returns></returns>
        private bool GetEnabledStatus()
        {
            bool enabled = true;
            if (Singleton<SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId(Singleton<PublicMemory>.Instance.moduleIndex)] == SystemStatusKind.ReagentExchange)
            {
                enabled = false;
            }
            else
            {
                switch (Singleton<SystemStatus>.Instance.Status)
                {
                    case SystemStatusKind.WaitSlaveResponce:
                    case SystemStatusKind.Assay:
                    case SystemStatusKind.SamplingPause:
                    case SystemStatusKind.ToEndAssay:
                        enabled = false;
                        break;
                }
            }

            return enabled;
        }

        /// <summary>
        /// グループを縮小した際に、ヘッダ部の画像を「＋」型のものに変更する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exbCurrentConfiguration_GroupCollapsed(object sender, Infragistics.Win.UltraWinExplorerBar.GroupEventArgs e)
        {
            e.Group.Settings.AppearancesSmall.HeaderAppearance.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_GroupExpandLong;
        }

        /// <summary>
        /// グループを展開した際に、ヘッダ部の画像を「－」型のものに変更する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exbCurrentConfiguration_GroupExpanded(object sender, Infragistics.Win.UltraWinExplorerBar.GroupEventArgs e)
        {
            e.Group.Settings.AppearancesSmall.HeaderAppearance.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_GroupCollapseLong;
        }

        /// <summary>
        /// グループを縮小した際に、ヘッダ部の画像を「＋」型のものに変更する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exbOptionList_GroupCollapsed(object sender, GroupEventArgs e)
        {
            e.Group.Settings.AppearancesSmall.HeaderAppearance.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_GroupExpandShort;
        }

        /// <summary>
        /// グループを展開した際に、ヘッダ部の画像を「－」型のものに変更する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exbOptionList_GroupExpanded(object sender, GroupEventArgs e)
        {
            e.Group.Settings.AppearancesSmall.HeaderAppearance.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_GroupCollapseShort;
        }

        /// <summary>
        /// システムパラメータコマンド送信
        /// </summary>
        private void sendSystemParameter()
        {
            CarisXSequenceHelper.SequenceSyncObject syncData;

            //ラック搬送へのシステムパラメータコマンド送信
            if (Singleton<CarisXCommManager>.Instance.GetRackTransferCommStatus() == ConnectionStatus.Online)
            {
                syncData = Singleton<CarisXSequenceHelperManager>.Instance.RackTransfer.SendRackSystemParameter();
                while (!syncData.EndSequence.WaitOne(10))
                {
                    // ここをDoEventsでの待ちにしない場合、上位の処理をブロック単位に切り分けて複数段階での実行を行う事になり
                    // コード全体の見通しが悪くなる為使用する。メインスレッドをブロックして構わない場合この限りではない。
                    Application.DoEvents();
                }

                if (syncData.Status != CarisXSequenceHelper.SequenceSyncObject.SequenceStatus.Success)
                {
                    //エラーの場合
                    CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.CommunicationErrorBetweenMasterAndRackTransfer);
                }
            }

            //スレーブへのシステムパラメータコマンド送信
            foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                {
                    syncData = Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleindex].SendSlaveSystemParameter();
                    while (!syncData.EndSequence.WaitOne(10))
                    {
                        // ここをDoEventsでの待ちにしない場合、上位の処理をブロック単位に切り分けて複数段階での実行を行う事になり
                        // コード全体の見通しが悪くなる為使用する。メインスレッドをブロックして構わない場合この限りではない。
                        Application.DoEvents();
                    }

                    if (syncData.Status != CarisXSequenceHelper.SequenceSyncObject.SequenceStatus.Success)
                    {
                        //エラーの場合
                        CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.CommunicationErrorBetweenMasterAndSlave);
                    }
                }
            }
        }

        #endregion
    }
}
