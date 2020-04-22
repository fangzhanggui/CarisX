using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Infragistics.Win.Misc;
using Oelco.Common.Utility;
using Oelco.CarisX.Const;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Common;
using Oelco.CarisX.DB;
using Oelco.CarisX.GUI.Controls;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Status;
using Infragistics.Win.UltraWinToolbars;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 試薬準備画面クラス
    /// </summary>
    public partial class FormSetReagent : FormChildBase
    {
        #region [一部ややこしいので少し説明]
        ///交換開始した際、画面上でオレンジ色になっているボタンを対象に交換準備開始処理を行っていく。
        ///「runNextTarget」メソッドを使用して対象となるボタンに該当する処理を進めていく。
        ///処理の流れとしては下記のようになる。
        ///exchangeStart
        ///　runNextTarget
        ///　　sendCommonPrepareStart
        ///　　　PrepareSequence（CarisXSequenceHelper。）
        ///　　　　commonPrepareStartResponse（MainFrame。SequenceHelperからCommonPrepareStartResponseメッセージで呼び出し）
        ///　　　　　SetCmdResCommonPrepareStart
        ///　runNextTarget
        ///　:
        ///　:
        #endregion

        #region [定数定義]

        /// <summary>
        /// 交換開始
        /// </summary>
        public const String EXCHANGE_START = "Exchange start";

        /// <summary>
        /// 交換完了
        /// </summary>
        public const String EXCHANGE_COMPLETE = "Exchange complete";

        /// <summary>
        /// 交換キャンセル
        /// </summary>
        public const String EXCHANGE_CANCEL = "Exchange cancel";

        /// <summary>
        /// 編集
        /// </summary>
        public const String EDIT = "Edit";

        /// <summary>
        /// 洗浄液供給
        /// </summary>
        public const String REPLACE_TANK = "Replace Tank";

        /// <summary>
        /// ボタン点滅間隔
        /// </summary>
        public const Int32 BTN_BLINK_INTERVAL = 200;

        /// <summary>
        /// 交換対象
        /// </summary>
        [Flags]
        public enum ReagentChangeTargetKind : int
        {
            /// <summary>
            /// 開始
            /// </summary>
            Start = 0x0001,
            /// <summary>
            /// ポート１
            /// </summary>
            Port1 = 0x0002,
            /// <summary>
            /// ポート２
            /// </summary>
            Port2 = 0x0004,
            /// <summary>
            /// 試薬ボトル
            /// </summary>
            ReagentBottle = 0x0008,
            /// <summary>
            /// 廃液バッファ
            /// </summary>
            WasteBuffer = 0x0010,
            /// <summary>
            /// 洗浄液バッファ
            /// </summary>
            WashSolutionBuffer = 0x0020,
            /// <summary>
            /// プレトリガ
            /// </summary>
            Pretrigger = 0x0040,
            /// <summary>
            /// トリガ
            /// </summary>
            Trigger = 0x0080,
            /// <summary>
            /// 希釈液
            /// </summary>
            Diluent = 0x0100,
            /// <summary>
            /// サンプル分注チップ
            /// </summary>
            SamplingTip = 0x0200,
            /// <summary>
            /// 全て
            /// </summary>
            All = 0x1000,
        }

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// プレトリガ、トリガ、希釈液ボタン
        /// </summary>
        private List<BlinkButton> pretriggerTriggerDiluentButtons = new List<BlinkButton>();

        /// <summary>
        /// 分注チップボタン
        /// </summary>
        private List<BlinkButton> samplingTipCellButtons = new List<BlinkButton>();

        /// <summary>
        /// 試薬ボトル残量確認ダイアログ
        /// </summary>
        private DlgTurnTable dlgTurnTable;

        /// <summary>
        /// 中断用オブジェクト
        /// </summary>
        private List<CarisXSequenceHelper.SequenceCommData> sequenceCommData = new List<CarisXSequenceHelper.SequenceCommData>();

        /// <summary>
        /// セルチップケースのアクト状況オブジェクリスト
        /// </summary>
        private List<Infragistics.Win.UltraWinEditors.UltraPictureBox> pbxTipCellList;

        /// <summary>
        /// プレトリガ、トリガ、希釈液のコマンド送信用の情報を保存する構造体
        /// </summary>
        private struct SendRemainInfo
        {
            /// <summary>
            /// ポート番号
            /// </summary>
            public int portNo;
            /// <summary>
            /// 残量
            /// </summary>
            public int remain;
            /// <summary>
            /// ロット番号
            /// </summary>
            public int lotNo;
            /// <summary>
            /// 使用期限
            /// </summary>
            public DateTime termOfUse;
        }

        /// <summary>
        /// プレトリガ残量情報
        /// </summary>
        private SendRemainInfo[] inpPretriggerVal = new SendRemainInfo[2];
        /// <summary>
        /// トリガ残量情報
        /// </summary>
        private SendRemainInfo[] inpTriggerVal = new SendRemainInfo[2];
        /// <summary>
        /// 希釈液残量情報
        /// </summary>
        private SendRemainInfo inpDilVal;

        /// <summary>
        /// 残量取得処理
        /// </summary>
        private Func<ReagentKind, Int32, Int32> getRemain = (kind, portNo) =>
        {
            Int32 moduleId = CarisXSubFunction.ModuleIndexToModuleId(Singleton<PublicMemory>.Instance.moduleIndex);
            int result = 0;
            var reagDataList = Singleton<ReagentDB>.Instance.GetData(kind, moduleId: moduleId);
            if (reagDataList.Count > 0)
            {
                var portData = reagDataList.First((data) => data.PortNo == portNo);
                if (portData != null && portData.Remain.HasValue)
                {
                    result = portData.Remain.Value;
                }
            }

            return result;
        };

        /// <summary>
        /// データ更新
        /// </summary>
        private EventHandler updateData = null;

        /// <summary>
        /// ボタンとラベルの対応リスト
        /// </summary>
        private Dictionary<BlinkButton, List<UltraLabel>> buttonLabelInfo = new Dictionary<BlinkButton, List<UltraLabel>>();

        /// <summary>
        /// ボタンとピクチャーボックスの対応リスト
        /// </summary>
        private Dictionary<BlinkButton, Dictionary<Infragistics.Win.UltraWinEditors.UltraPictureBox, String>> buttonPbxInfo = new Dictionary<BlinkButton, Dictionary<Infragistics.Win.UltraWinEditors.UltraPictureBox, String>>();

        /// <summary>
        /// モジュール毎の画面状態リスト
        /// </summary>
        private Dictionary<ModuleIndex, SetReagentStateInfo> setReagentStateManager = new Dictionary<ModuleIndex, SetReagentStateInfo>();

        /// <summary>
        /// 試薬交換操作禁止状態の有無
        /// </summary>
        private Boolean isReagentChangeRefused = false;

        /// <summary>
        /// 【IssuesNo:12】当前条码信息
        /// </summary>
        private string barCodeResult;  

        /// <summary>
        /// 試薬準備画面におけるボタン状態クラス
        /// </summary>
        private class SetReagentStateInfo
        {
            /// <summary>
            /// 試薬交換・編集操作フラグ
            /// </summary>
            public DlgTurnTable.TurnTableDispMode TurnTableDispMode = DlgTurnTable.TurnTableDispMode.Change;


            /// <summary>
            /// 直近システムステータス
            /// </summary>
            public SystemStatusKind BeforeStatusKind = SystemStatusKind.NoLink;

            /// <summary>
            /// ターンテーブル表示フラグ
            /// </summary>
            public Boolean IsShowTurnTable = false;

            /// <summary>
            /// バーコード読取表示フラグ
            /// </summary>
            public Boolean IsShowReadBCBottle = false;

            /// <summary>
            /// 分注終了待機時点滅対象ボタンリスト
            /// </summary>
            public List<BlinkButton> BlinkListOnDispenceEnd = new List<BlinkButton>();

            /// <summary>
            /// 準備確認レスポンス受信にて待機
            /// </summary>
            public ReagentChangeTargetKind WaitSetCmdResPrepareCheck = 0;

            /// <summary>
            /// 準備開始コマンド送信にて待機
            /// </summary>
            public ReagentChangeTargetKind WaitSetCmdResCommonPrepareStart = 0;

            /// <summary>
            /// 待機中のターンテーブル表示にて待機
            /// </summary>
            public Boolean WaitShowTurnTableIfWaitDispence = false;

            /// <summary>
            /// 試薬残量変更完了受信にて待機
            /// </summary>
            public Boolean WaitSetCmdResChangeReagentRemain = false;

            #region _分析中試薬交換関連フラグ_

            /// <summary>
            /// 試薬準備確認コマンド応答受信フラグ
            /// </summary>
            public Boolean WaitExchangeResp = false;

            /// <summary>
            /// 試薬交換時分注待機時間完了フラグ
            /// </summary>
            public Boolean WaitDispenseCompleted = false;

            /// <summary>
            /// 分注待ち状態フラグ
            /// </summary>
            public Boolean WaitDispense = false;

            #endregion

            /// <summary>
            /// 全て選択
            /// </summary>
            public Dictionary<UltraButton, ButtonState> AllSwitch = new Dictionary<UltraButton, ButtonState>();

            /// <summary>
            /// 点滅
            /// </summary>
            public Dictionary<BlinkButton, ButtonState> Blink = new Dictionary<BlinkButton, ButtonState>();

            /// <summary>
            /// ツールバー
            /// </summary>
            public Dictionary<String, ButtonState> ToolBar = new Dictionary<String, ButtonState>();
        }

        /// <summary>
        /// ボタン状態
        /// </summary>
        private class ButtonState
        {
            /// <summary>
            /// 選択状態
            /// </summary>
            public Boolean CurrentState;

            /// <summary>
            /// 活性状態
            /// </summary>
            public Boolean Enabled;

            /// <summary>
            /// 点滅状態
            /// </summary>
            public Boolean IsBlink;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="currentState"></param>
            /// <param name="enabled"></param>
            /// <param name="isBlink"></param>
            public ButtonState(Boolean currentState, Boolean enabled, Boolean isBlink)
            {
                this.CurrentState = currentState;
                this.Enabled = enabled;
                this.IsBlink = isBlink;
            }
        }

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormSetReagent()
        {
            InitializeComponent();

            // プレトリガ、トリガ、希釈液ボタンリスト追加
            this.pretriggerTriggerDiluentButtons.AddRange(new BlinkButton[] { this.btnPretrigger1, this.btnPretrigger2, this.btnTrigger1, this.btnTrigger2, this.btnDiluent });

            // 分注チップボタンリスト追加
            this.samplingTipCellButtons.AddRange(new BlinkButton[] { this.btnSamplingTipCell1, this.btnSamplingTipCell2, this.btnSamplingTipCell3, this.btnSamplingTipCell4, this.btnSamplingTipCell5, this.btnSamplingTipCell6, this.btnSamplingTipCell7, this.btnSamplingTipCell8 });

            // コマンドバーのイベント追加
            this.tlbCommandBar.Tools[EXCHANGE_START].ToolClick += (sender, e) => this.exchangeStart();
            this.tlbCommandBar.Tools[EXCHANGE_COMPLETE].ToolClick += (sender, e) => this.exchangeComplete();
            this.tlbCommandBar.Tools[EXCHANGE_CANCEL].ToolClick += (sender, e) => this.exchangeCancel();
            this.tlbCommandBar.Tools[EDIT].ToolClick += (sender, e) => this.editData();
            this.tlbCommandBar.Tools[REPLACE_TANK].ToolClick += (sender, e) => this.washSolutionExterior();

            //ボタンとラベルの関連付け用変数に設定
            this.buttonLabelInfo.Add(this.btnPretrigger1, new List<UltraLabel> { this.lblPretrigger1 });
            this.buttonLabelInfo.Add(this.btnPretrigger2, new List<UltraLabel> { this.lblPretrigger2 });
            this.buttonLabelInfo.Add(this.btnTrigger1, new List<UltraLabel> { this.lblTrigger1 });
            this.buttonLabelInfo.Add(this.btnTrigger2, new List<UltraLabel> { this.lblTrigger2 });
            this.buttonLabelInfo.Add(this.btnSamplingTipCell1, new List<UltraLabel> { this.lblSamplingTipCellNo1, this.lblSamplingTipCell1 });
            this.buttonLabelInfo.Add(this.btnSamplingTipCell2, new List<UltraLabel> { this.lblSamplingTipCellNo2, this.lblSamplingTipCell2 });
            this.buttonLabelInfo.Add(this.btnSamplingTipCell3, new List<UltraLabel> { this.lblSamplingTipCellNo3, this.lblSamplingTipCell3 });
            this.buttonLabelInfo.Add(this.btnSamplingTipCell4, new List<UltraLabel> { this.lblSamplingTipCellNo4, this.lblSamplingTipCell4 });
            this.buttonLabelInfo.Add(this.btnSamplingTipCell5, new List<UltraLabel> { this.lblSamplingTipCellNo5, this.lblSamplingTipCell5 });
            this.buttonLabelInfo.Add(this.btnSamplingTipCell6, new List<UltraLabel> { this.lblSamplingTipCellNo6, this.lblSamplingTipCell6 });
            this.buttonLabelInfo.Add(this.btnSamplingTipCell7, new List<UltraLabel> { this.lblSamplingTipCellNo7, this.lblSamplingTipCell7 });
            this.buttonLabelInfo.Add(this.btnSamplingTipCell8, new List<UltraLabel> { this.lblSamplingTipCellNo8, this.lblSamplingTipCell8 });

            //ボタンとピクチャーボックスの関連付け用変数に設定
            this.buttonPbxInfo.Add(this.btnPretrigger1,
                new Dictionary<Infragistics.Win.UltraWinEditors.UltraPictureBox, string> { { this.pbxPreTrigger1IsUse, Oelco.CarisX.Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_001 } });
            this.buttonPbxInfo.Add(this.btnPretrigger2,
                new Dictionary<Infragistics.Win.UltraWinEditors.UltraPictureBox, string> { { this.pbxPreTrigger2IsUse, Oelco.CarisX.Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_002 } });
            this.buttonPbxInfo.Add(this.btnTrigger1, 
                new Dictionary<Infragistics.Win.UltraWinEditors.UltraPictureBox, string> { { this.pbxTrigger1IsUse, Oelco.CarisX.Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_003 } });
            this.buttonPbxInfo.Add(this.btnTrigger2,
                new Dictionary<Infragistics.Win.UltraWinEditors.UltraPictureBox, string> { { this.pbxTrigger2IsUse, Oelco.CarisX.Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_004 } });
            this.buttonPbxInfo.Add(this.btnSamplingTipCell1,
                new Dictionary<Infragistics.Win.UltraWinEditors.UltraPictureBox, string> { { this.pbxTipCell1IsUse, Oelco.CarisX.Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_012 } });
            this.buttonPbxInfo.Add(this.btnSamplingTipCell2,
                new Dictionary<Infragistics.Win.UltraWinEditors.UltraPictureBox, string> { { this.pbxTipCell2IsUse, Oelco.CarisX.Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_013 } });
            this.buttonPbxInfo.Add(this.btnSamplingTipCell3,
                new Dictionary<Infragistics.Win.UltraWinEditors.UltraPictureBox, string> { { this.pbxTipCell3IsUse, Oelco.CarisX.Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_014 } });
            this.buttonPbxInfo.Add(this.btnSamplingTipCell4,
                new Dictionary<Infragistics.Win.UltraWinEditors.UltraPictureBox, string> { { this.pbxTipCell4IsUse, Oelco.CarisX.Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_015 } });
            this.buttonPbxInfo.Add(this.btnSamplingTipCell5,
                new Dictionary<Infragistics.Win.UltraWinEditors.UltraPictureBox, string> { { this.pbxTipCell5IsUse, Oelco.CarisX.Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_016 } });
            this.buttonPbxInfo.Add(this.btnSamplingTipCell6,
                new Dictionary<Infragistics.Win.UltraWinEditors.UltraPictureBox, string> { { this.pbxTipCell6IsUse, Oelco.CarisX.Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_017 } });
            this.buttonPbxInfo.Add(this.btnSamplingTipCell7,
                new Dictionary<Infragistics.Win.UltraWinEditors.UltraPictureBox, string> { { this.pbxTipCell7IsUse, Oelco.CarisX.Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_018 } });
            this.buttonPbxInfo.Add(this.btnSamplingTipCell8,
                new Dictionary<Infragistics.Win.UltraWinEditors.UltraPictureBox, string> { { this.pbxTipCell8IsUse, Oelco.CarisX.Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_019 } });

            //ボタンとラベルの関連付け
            buttonLabelParentSet();

            //begin fix bug when Assay start ,but the ReagentChangeIsRefused ,ReagntChangeIsAllowed Notice was not Add to NoticeMamager
            // 試薬交換禁止状態通知ハンドラ登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ReagentChangeIsRefused, this.onReagentChangeRefused);
            // 試薬交換許可状態通知ハンドラ登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ReagentChangeIsAllowed, this.onReagentChangeAllowed);
            // リアルタイムデータ更新イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.RealtimeData, this.onRealTimeDataChanged);
            // システム状態変化通知
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SystemStatusChanged, this.onSystemStatusChanged);
            //【IssuesNo:2】使用外部供给液变化通知
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.WashSolutionExteriorUsable, OnWashSolutionFromExteriorChange);

            // 交換開始、編集コマンドバーEnable
            this.tlbCommandBar.Tools[EXCHANGE_START].SharedProps.Enabled = true;
            this.tlbCommandBar.Tools[EDIT].SharedProps.Enabled = true;
            this.tlbCommandBar.Tools[REPLACE_TANK].SharedProps.Enabled = true;

            // 交換完了、交換キャンセルコマンドバーDisable
            this.tlbCommandBar.Tools[EXCHANGE_COMPLETE].SharedProps.Enabled = false;
            this.tlbCommandBar.Tools[EXCHANGE_CANCEL].SharedProps.Enabled = false;
            //end fix bug when Assay start ,but the ReagentChangeIsRefused ,ReagntChangeIsAllowed Notice was not Add to NoticeMamager


            //ツールバーの右ボタンの設定は利用できません
            this.tlbCommandBar.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.tlbCommandBar_BeforeToolbarListDropdown);

            // リストの追加
            pbxTipCellList = new List<Infragistics.Win.UltraWinEditors.UltraPictureBox>()
            {
                this.pbxTipCell1IsUse,
                this.pbxTipCell2IsUse,
                this.pbxTipCell3IsUse,
                this.pbxTipCell4IsUse,
                this.pbxTipCell5IsUse,
                this.pbxTipCell6IsUse,
                this.pbxTipCell7IsUse,
                this.pbxTipCell8IsUse
            };

            // モジュール毎のボタンステータス管理リストを追加
            foreach (ModuleIndex moduleIndex in Enum.GetValues(typeof(ModuleIndex)))
            {
                setReagentStateManager.Add(moduleIndex, new SetReagentStateInfo());

                // 全て選択
                setReagentStateManager[moduleIndex].AllSwitch = new Dictionary<UltraButton, ButtonState>
                {
                    { this.btnPretriggerTriggerDiluentSelectAllSwitch, new ButtonState( false, this.btnPretriggerTriggerDiluentSelectAllSwitch.Enabled, false ) }
                  , { this.btnSamplingTipSelectAllSwitch             , new ButtonState( false, this.btnSamplingTipSelectAllSwitch.Enabled, false ) }
                };

                // 点滅ボタン
                setReagentStateManager[moduleIndex].Blink = new Dictionary<BlinkButton, ButtonState>
                {
                    { this.btnReagentBottle,      new ButtonState( this.btnReagentBottle.CurrentState, this.btnReagentBottle.Enabled, this.btnReagentBottle.IsBlink ) }
                  , { this.btnWasteBuffer,        new ButtonState( this.btnWasteBuffer.CurrentState, this.btnWasteBuffer.Enabled, this.btnWasteBuffer.IsBlink ) }
                  , { this.btnWashSolutionBuffer, new ButtonState( this.btnWashSolutionBuffer.CurrentState, this.btnWashSolutionBuffer.Enabled, this.btnWashSolutionBuffer.IsBlink ) }
                  , { this.btnPretrigger1,        new ButtonState( this.btnPretrigger1.CurrentState, this.btnPretrigger1.Enabled, this.btnPretrigger1.IsBlink ) }
                  , { this.btnPretrigger2,        new ButtonState( this.btnPretrigger2.CurrentState, this.btnPretrigger2.Enabled, this.btnPretrigger2.IsBlink ) }
                  , { this.btnTrigger1,           new ButtonState( this.btnTrigger1.CurrentState, this.btnTrigger1.Enabled, this.btnTrigger1.IsBlink ) }
                  , { this.btnTrigger2,           new ButtonState( this.btnTrigger2.CurrentState, this.btnTrigger2.Enabled, this.btnTrigger2.IsBlink ) }
                  , { this.btnDiluent,            new ButtonState( this.btnDiluent.CurrentState, this.btnDiluent.Enabled, this.btnDiluent.IsBlink ) }
                  , { this.btnSamplingTipCell1,   new ButtonState( this.btnSamplingTipCell1.CurrentState, this.btnSamplingTipCell1.Enabled, this.btnSamplingTipCell1.IsBlink ) }
                  , { this.btnSamplingTipCell2,   new ButtonState( this.btnSamplingTipCell2.CurrentState, this.btnSamplingTipCell2.Enabled, this.btnSamplingTipCell2.IsBlink ) }
                  , { this.btnSamplingTipCell3,   new ButtonState( this.btnSamplingTipCell3.CurrentState, this.btnSamplingTipCell3.Enabled, this.btnSamplingTipCell3.IsBlink ) }
                  , { this.btnSamplingTipCell4,   new ButtonState( this.btnSamplingTipCell4.CurrentState, this.btnSamplingTipCell4.Enabled, this.btnSamplingTipCell4.IsBlink ) }
                  , { this.btnSamplingTipCell5,   new ButtonState( this.btnSamplingTipCell5.CurrentState, this.btnSamplingTipCell5.Enabled, this.btnSamplingTipCell5.IsBlink ) }
                  , { this.btnSamplingTipCell6,   new ButtonState( this.btnSamplingTipCell6.CurrentState, this.btnSamplingTipCell6.Enabled, this.btnSamplingTipCell6.IsBlink ) }
                  , { this.btnSamplingTipCell7,   new ButtonState( this.btnSamplingTipCell7.CurrentState, this.btnSamplingTipCell7.Enabled, this.btnSamplingTipCell7.IsBlink ) }
                  , { this.btnSamplingTipCell8,   new ButtonState( this.btnSamplingTipCell8.CurrentState, this.btnSamplingTipCell8.Enabled, this.btnSamplingTipCell8.IsBlink ) }
                };


                setReagentStateManager[moduleIndex].ToolBar = new Dictionary<String, ButtonState>
                {
                    { EXCHANGE_START,     new ButtonState( false, this.tlbCommandBar.Tools[EXCHANGE_START].SharedProps.Enabled, false ) }
                  , { EDIT,               new ButtonState( false, this.tlbCommandBar.Tools[EDIT].SharedProps.Enabled, false ) }
                  , { REPLACE_TANK,       new ButtonState( false, this.tlbCommandBar.Tools[REPLACE_TANK].SharedProps.Enabled, false ) }
                  , { EXCHANGE_COMPLETE,  new ButtonState( false, this.tlbCommandBar.Tools[EXCHANGE_COMPLETE].SharedProps.Enabled, false ) }
                  , { EXCHANGE_CANCEL,    new ButtonState( false, this.tlbCommandBar.Tools[EXCHANGE_CANCEL].SharedProps.Enabled, false ) }
                };
            }
        }

        /// <summary>
        /// ツールバーの右ボタンの設定は利用不可制御
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbCommandBar_BeforeToolbarListDropdown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        /// ボタンの上にラベルを配置しているものの親子関係を作成する
        /// </summary>
        private void buttonLabelParentSet()
        {
            //キーに持ってるボタンコントロール分繰り返し処理を行う
            foreach (BlinkButton btn in this.buttonLabelInfo.Keys)
            {
                //ボタンに関連付けさせたいラベルコントロール分繰り返し処理を行う
                foreach (UltraLabel lbl in this.buttonLabelInfo[btn])
                {
                    lbl.Parent = btn;
                    lbl.Location -= (Size)btn.Location;
                }
            }
        }
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 試薬テーブル回転位置取得
        /// </summary>
        /// <param name="moduleIndex">対象となるモジュールインデックス（モジュール１は０）</param>
        /// <remarks>
        /// 試薬ボトル交換ダイアログが表示されている時以外は0、
        /// 表示されているときは、次の移動位置を返します。
        /// この関数は試薬テーブル回転コマンドがスレーブから送信された時呼び出されます。
        /// </remarks>
        /// <returns>試薬保冷庫テーブル移動位置</returns>
        public void TurnTable(Int32 moduleIndex)
        {
            // 試薬保冷庫テーブル移動
            if (this.dlgTurnTable != null)
            {
                //試薬ボトル残量確認ダイアログが表示されている（＝試薬交換操作中）場合、
                //選択中のモジュールから来た0516コマンド以外反応させない（試薬吸引中に回転させてしまう可能性があるため）
                if (moduleIndex == (int)Singleton<PublicMemory>.Instance.moduleIndex)
                {
                    // Turnボタンによる動作が可能な場合にのみ回転コマンド送信する。
                    // この画面が開いていて、Turn不可であれば回転コマンドは送信しない。
                    if (this.dlgTurnTable.CanTurn)
                    {
                        //次のポジションへ回転
                        this.dlgTurnTable.TurnTable(moduleIndex);
                    }
                }
            }
            else
            {
                // 回転コマンド
                SlaveCommCommand_0487 turnCommand = new SlaveCommCommand_0487();
                // 試薬ボトル交換ダイアログが表示されてないので0(1/10回転)で回転させる
                turnCommand.PortNumber = 0;
                // コマンド送信
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(moduleIndex, turnCommand);
            }
        }

        /// <summary>
        /// 準備確認レスポンス受信
        /// </summary>
        /// <remarks>
        /// ボタン点滅、交換完了、交換キャンセルボタン有効化などを行います。
        /// </remarks>
        /// <returns>なし</returns>
        public void SetCmdResPrepareCheck(ReagentChangeTargetKind target, ModuleIndex targetModuleIndex)
        {
            // 画面反映フラグ
            Boolean isReflectDisplay = false;
            if(targetModuleIndex == Singleton<PublicMemory>.Instance.moduleIndex)
            {
                // 選択中のモジュールと同じ場合、画面反映
                isReflectDisplay = true;
            }

            // 試薬準備確認コマンド応答フラグON
            this.setReagentStateManager[targetModuleIndex].WaitExchangeResp = true;

            // 分注待ち状態か確認
            if (this.setReagentStateManager[targetModuleIndex].WaitDispense == false)
            {
                // 分注待ち状態ではないため、活性状態変更
                if( isReflectDisplay )
                {
                    // 交換完了ボタン有効化
                    this.tlbCommandBar.Tools[EXCHANGE_COMPLETE].SharedProps.Enabled = true;
                }
                this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_COMPLETE].Enabled = true;
            }

            // 試薬ボトルが交換対象の場合
            if (target.HasFlag(ReagentChangeTargetKind.ReagentBottle))
            {
                // 試薬交換中ステータスに切替
                this.setReagentExchangeStatus(targetModuleIndex);

                // 分注待ち状態ではないか確認
                if (this.setReagentStateManager[targetModuleIndex].WaitDispense == false)
                {
                    // 試薬交換可能を示すダイアログの表示は行わないこと

                    // ボタン点滅開始
                    this.setButtonBlink(true, this.btnReagentBottle, targetModuleIndex);

                    // 画面反映有無
                    if ((this.Visible == true)
                     && (isReflectDisplay == true))
                    {
                        // ターンテーブル表示
                        this.showTurnTable();

                        // 次回モジュール切り替え時にターンテーブル表示フラグOFF
                        this.setReagentStateManager[targetModuleIndex].IsShowTurnTable = false;
                        this.setReagentStateManager[targetModuleIndex].WaitSetCmdResPrepareCheck = 0;

                        // 次の準備開始対象を処理
                        this.runNextTarget(target, targetModuleIndex);
                    }
                    else
                    {
                        // 次回モジュール切り替え時にターンテーブル表示フラグON
                        this.setReagentStateManager[targetModuleIndex].IsShowTurnTable = true;
                        this.setReagentStateManager[targetModuleIndex].WaitSetCmdResPrepareCheck = target;
                    }
                }

            }

            // 廃液バッファに準備コマンドが無いのでここには来ません
            // 洗浄液バッファに準備コマンドが無いのでここには来ません
            // プレトリガは準備コマンドのレスポンスが無いのでここには来ません
            // トリガは準備コマンドのレスポンスが無いのでここには来ません

            // 希釈液が交換対象の場合
            if (target.HasFlag(ReagentChangeTargetKind.Diluent))
            {
                // 分注待ち状態か確認
                if (this.setReagentStateManager[targetModuleIndex].WaitDispense == true)
                {
                    // 分注終了待機時点滅ボタンリストへ追加
                    this.setReagentStateManager[targetModuleIndex].BlinkListOnDispenceEnd.Add(this.btnDiluent);
                }
                else
                {
                    // ボタン点滅開始
                    this.setButtonBlink(true, this.btnDiluent, targetModuleIndex);
                }
            }

            // 分注チップは準備コマンドのレスポンスが無いのでここには来ません
        }

        /// <summary>
        /// 準備開始レスポンス受信
        /// </summary>
        /// <remarks>
        /// ボタン点滅、交換完了、交換キャンセルボタン有効化などを行います。
        /// </remarks>
        /// <returns>なし</returns>
        public void SetCmdResCommonPrepareStart(ReagentChangeTargetKind target, ModuleIndex targetModuleIndex)
        {
            // 画面反映フラグ
            Boolean isReflectDisplay = false;
            if (targetModuleIndex == Singleton<PublicMemory>.Instance.moduleIndex)
            {
                // 選択中のモジュールと同じ場合、画面反映
                isReflectDisplay = true;
            }
            
            // 廃液バッファが交換対象の場合
            if (target.HasFlag(ReagentChangeTargetKind.WasteBuffer))
            {
                // 分注待ち状態か確認
                if (this.setReagentStateManager[targetModuleIndex].WaitDispense == true)
                {
                    // 分注終了待機時点滅ボタンリストへ追加
                    this.setReagentStateManager[targetModuleIndex].BlinkListOnDispenceEnd.Add(this.btnWasteBuffer);
                }
                else
                {
                    // ボタン点滅開始
                    this.setButtonBlink(true, this.btnWasteBuffer, targetModuleIndex);

                    if (isReflectDisplay == true)
                    {
                        // 交換完了ボタン有効化
                        this.tlbCommandBar.Tools[EXCHANGE_COMPLETE].SharedProps.Enabled = true;
                    }
                    this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_COMPLETE].Enabled = true;
                }
            }

            // 洗浄液バッファが交換対象の場合
            if (target.HasFlag(ReagentChangeTargetKind.WashSolutionBuffer))
            {
                // 分注待ち状態か確認
                if (this.setReagentStateManager[targetModuleIndex].WaitDispense == true)
                {
                    // 分注終了待機時点滅ボタンリストへ追加
                    this.setReagentStateManager[targetModuleIndex].BlinkListOnDispenceEnd.Add(this.btnWashSolutionBuffer);
                }
                else
                {
                    // ボタン点滅開始
                    this.setButtonBlink(true, this.btnWashSolutionBuffer, targetModuleIndex);

                    if (isReflectDisplay == true)
                    {
                        // 交換完了ボタン有効化
                        this.tlbCommandBar.Tools[EXCHANGE_COMPLETE].SharedProps.Enabled = true;
                    }
                    this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_COMPLETE].Enabled = true;
                }
            }

            // プレトリガが交換対象の場合
            if (target.HasFlag(ReagentChangeTargetKind.Pretrigger))
            {
                BlinkButton btnPretrigger;

                if (target.HasFlag(ReagentChangeTargetKind.Port1))
                {
                    btnPretrigger = this.btnPretrigger1;
                }
                else
                {
                    btnPretrigger = this.btnPretrigger2;
                }

                // 分注待ち状態か確認
                if (this.setReagentStateManager[targetModuleIndex].WaitDispense == true)
                {
                    // 分注終了待機時点滅ボタンリストへ追加
                    this.setReagentStateManager[targetModuleIndex].BlinkListOnDispenceEnd.Add(btnPretrigger);
                }
                else
                {
                    // ボタン点滅開始
                    this.setButtonBlink(true, btnPretrigger, targetModuleIndex);

                    if (isReflectDisplay == true)
                    {
                        // 交換完了ボタン有効化
                        this.tlbCommandBar.Tools[EXCHANGE_COMPLETE].SharedProps.Enabled = true;
                    }
                    this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_COMPLETE].Enabled = true;
                }
            }

            // トリガが交換対象の場合
            if (target.HasFlag(ReagentChangeTargetKind.Trigger))
            {
                BlinkButton btnTrigger;

                if (target.HasFlag(ReagentChangeTargetKind.Port1))
                {
                    btnTrigger = this.btnTrigger1;
                }
                else
                {
                    btnTrigger = this.btnTrigger2;
                }

                // 分注待ち状態か確認
                if (this.setReagentStateManager[targetModuleIndex].WaitDispense == true)
                {
                    // 分注終了待機時点滅ボタンリストへ追加
                    this.setReagentStateManager[targetModuleIndex].BlinkListOnDispenceEnd.Add(btnTrigger);
                }
                else
                {
                    // ボタン点滅開始
                    this.setButtonBlink(true, btnTrigger, targetModuleIndex);

                    if (isReflectDisplay == true)
                    {
                        // 交換完了ボタン有効化
                        this.tlbCommandBar.Tools[EXCHANGE_COMPLETE].SharedProps.Enabled = true;
                    }
                    this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_COMPLETE].Enabled = true;
                }
            }

            // 希釈液が交換対象の場合
            if (target.HasFlag(ReagentChangeTargetKind.Diluent))
            {
                // 分注待ち状態か確認
                if (this.setReagentStateManager[targetModuleIndex].WaitDispense == true)
                {
                    // 分注終了待機時点滅ボタンリストへ追加
                    this.setReagentStateManager[targetModuleIndex].BlinkListOnDispenceEnd.Add(this.btnDiluent);
                }
                else
                {
                    // ボタンブリンク開始
                    this.setButtonBlink(true, this.btnDiluent, targetModuleIndex);
                }
            }

            // 分注チップが交換対象の場合
            if (target.HasFlag(ReagentChangeTargetKind.SamplingTip))
            {
                // 分注待ち状態でないか確認
                if (this.setReagentStateManager[targetModuleIndex].WaitDispense == false)
                {
                    if (isReflectDisplay == true)
                    {
                        // 交換完了ボタン有効化
                        this.tlbCommandBar.Tools[EXCHANGE_COMPLETE].SharedProps.Enabled = true;
                    }
                    this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_COMPLETE].Enabled = true;
                }
            }

            // 次の準備開始対象を処理
            this.runNextTarget(target, targetModuleIndex);
        }

        /// <summary>
        /// 準備完了レスポンス受信
        /// </summary>
        /// <remarks>
        /// ボタン点滅終了、交換開始ボタン有効化など
        /// </remarks>
        /// <returns>なし</returns>
        public void SetCmdResPrepareComplete(ReagentChangeTargetKind target, ModuleIndex targetModuleIndex)
        {
            // 交換完了・キャンセル共通処理
            this.finishReagentChange(target, targetModuleIndex);
        }

        /// <summary>
        /// 試薬残量変更の完了受信
        /// </summary>
        /// <remarks>
        /// 本ダイアログを閉じる
        /// </remarks>
        /// <returns>なし</returns>
        public void SetCmdResChangeReagentRemain(Boolean success, ModuleIndex targetModuleIndex)
        {
            if ( (success == true)
              && (this.setReagentStateManager[targetModuleIndex].WaitDispense == false) )
            {
                if ((this.Visible == true)
                 && (targetModuleIndex == Singleton<PublicMemory>.Instance.moduleIndex))
                {
                    // ターンテーブルダイアログ表示
                    this.showTurnTable();

                    // 次回モジュール切り替え時にターンテーブル表示フラグOFF
                    this.setReagentStateManager[targetModuleIndex].IsShowTurnTable = false;
                    this.setReagentStateManager[targetModuleIndex].WaitSetCmdResChangeReagentRemain = false;
                }
                else
                {
                    // 次回モジュール切り替え時にターンテーブル表示フラグON
                    this.setReagentStateManager[targetModuleIndex].IsShowTurnTable = true;
                    this.setReagentStateManager[targetModuleIndex].WaitSetCmdResChangeReagentRemain = success;
                }
            }
        }

        /// <summary>
        /// 残量コマンド受信
        /// </summary>
        /// <remarks>
        /// 残量情報更新
        /// </remarks>
        /// <returns>なし</returns>
        public void SetReagentRemain()
        {
            // 残量コマンド受信によりボタンのステータス（ボタンイメージ）を更新
            this.changeRemainStatus();
        }

        /// <summary>
        /// 残量変更コマンド受信
        /// </summary>
        /// <remarks>
        /// 残量変更コマンド受信処理を実行します
        /// </remarks>
        /// <param name="reagentKind">試薬種別</param>
        /// <param name="portNo">ポート番号</param>
        /// <param name="remain">残量</param>
        /// <param name="lotNumber">ロット番号</param>
        /// <param name="serialNumber">シリアル番号</param>
        public void SetCmdResChangeCommonRemain( ReagentKind reagentKind
                                               , Int32 portNo
                                               , Int32 remain
                                               , String lotNumber
                                               , Int32 serialNumber
                                               , ModuleIndex targetModuleIndex )
        {
            if (targetModuleIndex == Singleton<PublicMemory>.Instance.moduleIndex)
            {
                switch (reagentKind)
                {
                    case ReagentKind.Pretrigger:
                        btnPretrigger1.CurrentState = false;
                        btnPretrigger2.CurrentState = false;
                        break;
                    case ReagentKind.Trigger:
                        btnTrigger1.CurrentState = false;
                        btnTrigger2.CurrentState = false;
                        break;
                    case ReagentKind.Diluent:
                        btnDiluent.CurrentState = false;
                        break;
                    case ReagentKind.SamplingTip:
                        btnSamplingTipCell1.CurrentState = false;
                        btnSamplingTipCell2.CurrentState = false;
                        btnSamplingTipCell3.CurrentState = false;
                        btnSamplingTipCell4.CurrentState = false;
                        btnSamplingTipCell5.CurrentState = false;
                        btnSamplingTipCell6.CurrentState = false;
                        btnSamplingTipCell7.CurrentState = false;
                        btnSamplingTipCell8.CurrentState = false;
                        break;
                    default:
                        return;
                }
            }

            // 状態を記憶
            switch (reagentKind)
            {
                case ReagentKind.Pretrigger:
                    this.setReagentStateManager[targetModuleIndex].Blink[this.btnPretrigger1].CurrentState = false;
                    this.setReagentStateManager[targetModuleIndex].Blink[this.btnPretrigger2].CurrentState = false;
                    break;
                case ReagentKind.Trigger:
                    this.setReagentStateManager[targetModuleIndex].Blink[this.btnTrigger1].CurrentState = false;
                    this.setReagentStateManager[targetModuleIndex].Blink[this.btnTrigger2].CurrentState = false;
                    break;
                case ReagentKind.Diluent:
                    this.setReagentStateManager[targetModuleIndex].Blink[this.btnDiluent].CurrentState = false;
                    break;
                case ReagentKind.SamplingTip:
                    this.setReagentStateManager[targetModuleIndex].Blink[this.btnSamplingTipCell1].CurrentState = false;
                    this.setReagentStateManager[targetModuleIndex].Blink[this.btnSamplingTipCell2].CurrentState = false;
                    this.setReagentStateManager[targetModuleIndex].Blink[this.btnSamplingTipCell3].CurrentState = false;
                    this.setReagentStateManager[targetModuleIndex].Blink[this.btnSamplingTipCell4].CurrentState = false;
                    this.setReagentStateManager[targetModuleIndex].Blink[this.btnSamplingTipCell5].CurrentState = false;
                    this.setReagentStateManager[targetModuleIndex].Blink[this.btnSamplingTipCell6].CurrentState = false;
                    this.setReagentStateManager[targetModuleIndex].Blink[this.btnSamplingTipCell7].CurrentState = false;
                    this.setReagentStateManager[targetModuleIndex].Blink[this.btnSamplingTipCell8].CurrentState = false;
                    break;
                default:
                    return;
            }

            // コントロールから情報取得しDBへ登録
            Singleton<ReagentDB>.Instance.SetReagentRemainLotSerial(reagentKind, portNo, remain, lotNumber, serialNumber);
            Singleton<ReagentDB>.Instance.CommitData();

            // ボタンのステータス（ボタンイメージ）を更新
            this.changeRemainStatus();
        }

        /// <summary>
        /// 画面表示
        /// </summary>
        /// <remarks>
        /// 画面表示及び、分析中試薬交換時に他画面からの遷移の際に試薬交換ダイアログの表示を行います。
        /// </remarks>
        /// <param name="captScreenRect">表示領域</param>
        public override void Show(Rectangle captScreenRect)
        {
            base.Show(captScreenRect);

            // アクト状態更新
            this.changeActStatus();

            // 試薬ボタン状態を反映
            this.reflectReagentBtnStatus(Singleton<PublicMemory>.Instance.moduleIndex);

            // 待機時ターンテーブル表示
            this.showTurnTableIfWaitDispence(Singleton<PublicMemory>.Instance.moduleIndex);

        }

        /// <summary>
        /// 試薬準備確認コマンド応答受信処理
        /// </summary>
        public void WaitRespComplete(ModuleIndex targetModuleIndex)
        {
            // 試薬準備確認コマンド応答受信フラグON
            this.setReagentStateManager[targetModuleIndex].WaitExchangeResp = true;

            // この画面が表示されている場合、試薬テーブルダイアログを表示する。
            if (this.IsVisible)
            {
                // ターンテーブルダイアログを表示
                this.showTurnTableIfWaitDispence(targetModuleIndex);
            }
        }
        /// <summary>
        /// 分注待ち完了処理
        /// </summary>
        /// <remarks>
        /// 分析中試薬交換の際に、試薬交換確認コマンドに対して応答があった際に動作します。
        /// </remarks>
        public void WaitDispenceComplete(ModuleIndex targetModuleIndex)
        {
            // 待機時間経過フラグON
            this.setReagentStateManager[targetModuleIndex].WaitDispenseCompleted = true;


            // この画面が表示されている場合、試薬テーブルダイアログを表示する。
            if (this.IsVisible)
            {
                this.showTurnTableIfWaitDispence(targetModuleIndex);
            }
        }

        /// <summary>
        /// 試薬準備画面が保持している前回モジュールステータス取得
        /// </summary>
        /// <param name="targetModuleIndex">対象モジュールIndex</param>
        /// <returns></returns>
        public SystemStatusKind GetBeforeModuleStatus(ModuleIndex targetModuleIndex)
        {
            SystemStatusKind result = SystemStatusKind.NoLink;

            if (this.setReagentStateManager.ContainsKey(targetModuleIndex) == true)
            {
                result = this.setReagentStateManager[targetModuleIndex].BeforeStatusKind;
            }

            return result;
        }
        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// データ変更リアルタイム更新
        /// </summary>
        /// <remarks>
        /// データ変更リアルタイム更新します
        /// </remarks>
        /// <param name="kind">種別</param>
        protected void onRealTimeDataChanged(object kind)
        {
            // 試薬情報
            if (((RealtimeDataKind)kind) == RealtimeDataKind.ReagentData)
            {
                if (this.updateData == null)
                {
                    this.updateData = (obj, e) =>
                    {
                        // 試薬情報表示更新
                        this.SetReagentRemain();
                        this.Activated -= this.updateData;
                    };
                }

                if (this.updateData != null)
                {
                    this.Activated -= this.updateData;

                    // 表示中のみ更新
                    if (this.IsVisible)
                    {
                        this.updateData(this, null);
                    }
                    else
                    {
                        // 非表示中は次回フォームアクティブ時に更新
                        this.Activated += this.updateData;
                    }
                }

                // 対象となるモジュールIndexを取得
                ModuleIndex targetModuleIndex = Singleton<PublicMemory>.Instance.moduleIndex;

                // アクト状態を更新
                this.changeActStatus();

                // 試薬ボタン状態を反映
                this.reflectReagentBtnStatus(targetModuleIndex);

                // ターンテーブル表示待ちの場合
                if (this.setReagentStateManager[targetModuleIndex].IsShowTurnTable == true)
                {
                    // 準備確認レスポンス受信にて待機の場合
                    if (this.setReagentStateManager[targetModuleIndex].WaitSetCmdResPrepareCheck != 0)
                    {
                        // 準備確認レスポンス受信処理を再開
                        this.SetCmdResPrepareCheck(this.setReagentStateManager[targetModuleIndex].WaitSetCmdResPrepareCheck, targetModuleIndex);
                    }
                    // 試薬残量変更完了受信にて待機の場合
                    else if (this.setReagentStateManager[targetModuleIndex].WaitSetCmdResChangeReagentRemain == true)
                    {
                        // 試薬残量変更の完了受信処理を再開
                        this.SetCmdResChangeReagentRemain(this.setReagentStateManager[targetModuleIndex].WaitSetCmdResChangeReagentRemain, targetModuleIndex);
                    }
                    // 待機中のターンテーブル表示にて待機の場合
                    else if (this.setReagentStateManager[targetModuleIndex].WaitShowTurnTableIfWaitDispence == true)
                    {
                        // 待機時ターンテーブル表示を再開
                        this.showTurnTableIfWaitDispence(targetModuleIndex);
                    }
                }

                // ボトルバーコード読取ダイアログ表示待ちの場合
                if (this.setReagentStateManager[targetModuleIndex].IsShowReadBCBottle == true)
                {
                    // 準備開始コマンド送信にて待機の場合
                    if (this.setReagentStateManager[targetModuleIndex].WaitSetCmdResCommonPrepareStart != 0)
                    {
                        // 準備開始コマンド送信処理を再開
                        this.SetCmdResCommonPrepareStart(this.setReagentStateManager[targetModuleIndex].WaitSetCmdResCommonPrepareStart, targetModuleIndex);
                    }
                }
            }
        }

        /// <summary>
        /// システムステータス変化イベント
        /// </summary>
        /// <remarks>
        /// システムステータス変化により試薬ボトルなどを非選択状態にします
        /// </remarks>
        /// <param name="value"></param>
        protected void onSystemStatusChanged(Object value)
        {
            // スレーブ応答待ち状態の場合(スレーブ応答待ち状態になるのはAssay移行中のみ)
            if (Singleton<SystemStatus>.Instance.Status == SystemStatusKind.WaitSlaveResponce)
            {
                // モジュール毎のボタンステータス管理リストをチェック
                foreach (ModuleIndex moduleIndex in Enum.GetValues(typeof(ModuleIndex)))
                {
                    // 全ての点滅ボタンをチェック
                    foreach (var blinkBtn in this.setReagentStateManager[moduleIndex].Blink)
                    {
                        // 選択状態を解除
                        blinkBtn.Key.CurrentState = false;
                    }
                }
            }
            // それ以外の場合
            else
            {
                // 何もしない
            }
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
            // ボタンのステータス（ボタンイメージ）を更新
            this.changeRemainStatus();

            // 洗浄液供給ボタン表示設定
            this.tlbCommandBar.Tools[REPLACE_TANK].SharedProps.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashSolutionFromExterior.Enable;
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
            this.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_032;

            // コマンドバー
            this.tlbCommandBar.Tools[EXCHANGE_START].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_014;
            this.tlbCommandBar.Tools[EXCHANGE_COMPLETE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_015;
            this.tlbCommandBar.Tools[EXCHANGE_CANCEL].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_016;
            this.tlbCommandBar.Tools[EDIT].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_017;
            this.tlbCommandBar.Tools[REPLACE_TANK].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_024;

            // 子ダイアログタイトル
            this.lblTitleReagentBottle.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_000;
            this.lblTitlePretriggerTriggerDiluent.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_001;
            this.lblTitleSamplingTip.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_002;
            this.lblTitleBuffer.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_004;

            // ラベル
            this.lblPretrigger1.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_008;
            this.lblPretrigger2.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_009;
            this.lblTrigger1.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_008;
            this.lblTrigger2.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_009;
            this.lblSamplingTipCellNo1.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_008;
            this.lblSamplingTipCellNo2.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_009;
            this.lblSamplingTipCellNo3.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_010;
            this.lblSamplingTipCellNo4.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_011;
            this.lblSamplingTipCellNo5.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_012;
            this.lblSamplingTipCellNo6.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_013;
            this.lblSamplingTipCellNo7.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_014;
            this.lblSamplingTipCellNo8.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_015;

            // ボタン
            this.btnReagentBottle.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_005;
            this.btnWasteBuffer.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_006;
            this.btnWashSolutionBuffer.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_034;
            this.btnPretriggerTriggerDiluentSelectAllSwitch.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_007;
            this.btnSamplingTipSelectAllSwitch.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_007;
            this.btnPretrigger1.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_018;
            this.btnPretrigger2.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_018;
            this.btnTrigger1.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_019;
            this.btnTrigger2.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_019;
            this.btnDiluent.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_033;
            this.lblSamplingTipCell1.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_020;
            this.lblSamplingTipCell2.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_020;
            this.lblSamplingTipCell3.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_020;
            this.lblSamplingTipCell4.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_020;
            this.lblSamplingTipCell5.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_020;
            this.lblSamplingTipCell6.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_020;
            this.lblSamplingTipCell7.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_020;
            this.lblSamplingTipCell8.Text = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_020;

        }

        /// <summary>
        /// 試薬交換操作禁止
        /// </summary>
        /// <remarks>
        /// 試薬の交換を行えないタイミングで通知されます。
        /// </remarks>
        /// <param name="obj">不使用</param>
        protected void onReagentChangeRefused(Object obj)
        {
            this.isReagentChangeRefused = true;

            // 交換開始、交換完了、交換キャンセル、編集コマンドバーDisable
            this.tlbCommandBar.Tools[EXCHANGE_START].SharedProps.Enabled = false;
            this.tlbCommandBar.Tools[EXCHANGE_COMPLETE].SharedProps.Enabled = false;
            this.tlbCommandBar.Tools[EDIT].SharedProps.Enabled = false;
            this.tlbCommandBar.Tools[EXCHANGE_CANCEL].SharedProps.Enabled = false;
            this.tlbCommandBar.Tools[REPLACE_TANK].SharedProps.Enabled = false;

            // 各ボタンは操作不可にする
            this.setButtonEnable(false);

            // デバッグログ出力
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog
                                                     , Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                                                     , CarisXLogInfoBaseExtention.Empty
                                                     , "done onReagentChangeRefused function");
        }

        /// <summary>
        /// 試薬交換操作許可
        /// </summary>
        /// <remarks>
        /// 試薬の交換が可能となったタイミングで通知されます。
        /// </remarks>
        /// <param name="obj">不使用</param>
        protected void onReagentChangeAllowed(Object obj)
        {
            this.isReagentChangeRefused = false;

            ModuleIndex targetModuleIndex = Singleton<PublicMemory>.Instance.moduleIndex;

            // 交換開始、編集コマンドバーEnable
            this.tlbCommandBar.Tools[EXCHANGE_START].SharedProps.Enabled = this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_START].Enabled;
            this.tlbCommandBar.Tools[EDIT].SharedProps.Enabled = this.setReagentStateManager[targetModuleIndex].ToolBar[EDIT].Enabled;
            // 交換完了、交換キャンセルコマンドバーDisable
            this.tlbCommandBar.Tools[EXCHANGE_COMPLETE].SharedProps.Enabled = this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_COMPLETE].Enabled;
            this.tlbCommandBar.Tools[EXCHANGE_CANCEL].SharedProps.Enabled = this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_CANCEL].Enabled;
            this.tlbCommandBar.Tools[REPLACE_TANK].SharedProps.Enabled = this.setReagentStateManager[targetModuleIndex].ToolBar[REPLACE_TANK].Enabled;

            // 各ボタンは操作可にする
            this.setButtonEnable(true);
        }

        /// <summary>
        /// 【IssuesNo:2】修复系统设置在使用外部供给液时，试剂界面按钮没有联动刷新问题 
        /// </summary>
        /// <param name="obj"></param>
        protected void OnWashSolutionFromExteriorChange(object obj)
        {
            // 洗浄液供給ボタン表示設定
            this.tlbCommandBar.Tools[REPLACE_TANK].SharedProps.Visible = (Boolean)obj;
        }

        /// <summary>
        /// 待機時ターンテーブル表示
        /// </summary>
        /// <remarks>
        /// 待機時ターンテーブル表示します
        /// </remarks>
        protected void showTurnTableIfWaitDispence(ModuleIndex targetModuleIndex)
        {
            // 分注待ち状態、かつ分注待ちが完了していて、試薬準備確認コマンドに応答がされている場合に
            // 試薬交換ダイアログを表示する。
            if ( (this.setReagentStateManager[targetModuleIndex].WaitDispense == true)
              && (this.setReagentStateManager[targetModuleIndex].WaitDispenseCompleted == true)
              && (this.setReagentStateManager[targetModuleIndex].WaitExchangeResp == true) )
            {
                // 画面表示されているか確認
                // 対象モジュールと表示中モジュールが一致しているか確認
                if ((this.Visible == true)
                    && (targetModuleIndex == Singleton<PublicMemory>.Instance.moduleIndex))
                {
                    this.setReagentStateManager[targetModuleIndex].WaitDispense = false;
                    this.setReagentStateManager[targetModuleIndex].WaitDispenseCompleted = false;
                    this.setReagentStateManager[targetModuleIndex].WaitExchangeResp = false;

                    // 交換完了ボタン有効化
                    this.tlbCommandBar.Tools[EXCHANGE_COMPLETE].SharedProps.Enabled = true;

                    // 状態を記録
                    this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_COMPLETE].Enabled = true;

                    // ボタン点滅
                    this.setButtonBlink(true, this.setReagentStateManager[targetModuleIndex].BlinkListOnDispenceEnd, targetModuleIndex);

                    // ターンテーブル表示
                    this.showTurnTable();

                    // 待機中のターンテーブル表示フラグをOFF
                    this.setReagentStateManager[targetModuleIndex].IsShowTurnTable = false;
                    this.setReagentStateManager[targetModuleIndex].WaitShowTurnTableIfWaitDispence = false;

                    // 次の準備開始対象を処理
                    this.runNextTarget(ReagentChangeTargetKind.ReagentBottle, targetModuleIndex);
                }
                else
                {
                    // 待機中のターンテーブル表示フラグをON
                    this.setReagentStateManager[targetModuleIndex].IsShowTurnTable = true;
                    this.setReagentStateManager[targetModuleIndex].WaitShowTurnTableIfWaitDispence = true;
                }
            }
        }

        /// <summary>
        /// ターンテーブル表示
        /// </summary>
        /// <remarks>
        /// ターンテーブル表示します
        /// </remarks>
        protected void showTurnTable()
        {
            // 試薬ボトル残量確認ダイアログ表示
            if (this.dlgTurnTable == null)
            {
                using (this.dlgTurnTable = new DlgTurnTable())
                {
                    // 対象モジュールIndexを取得
                    ModuleIndex targetModuleIndex = Singleton<PublicMemory>.Instance.moduleIndex;

                    this.dlgTurnTable.IsChangeFirst = true;// 初回に試薬準備開始コマンドを送信する為のフラグ
                    this.dlgTurnTable.DispMode = this.setReagentStateManager[targetModuleIndex].TurnTableDispMode;
                    this.dlgTurnTable.ShowDialog();

                    // 交換・編集かで処理を変更
                    if (this.setReagentStateManager[targetModuleIndex].TurnTableDispMode == DlgTurnTable.TurnTableDispMode.Edit)
                    {
                        // 試薬ボトルボタンOFF
                        this.btnReagentBottle.CurrentState = false;
                        this.setReagentStateManager[targetModuleIndex].Blink[this.btnReagentBottle].CurrentState = false;
                    }
                    else if (this.setReagentStateManager[targetModuleIndex].TurnTableDispMode == DlgTurnTable.TurnTableDispMode.Change)
                    {
                        if (this.dlgTurnTable.DialogResult == DialogResult.Cancel)
                        {
                            // キャンセル操作した場合は、交換完了・キャンセル共通処理（対象関係無いのでAll指定）
                            // 全ての項目キャンセルから試薬ボトル交換のみを対象に変更
                            this.finishReagentChange(ReagentChangeTargetKind.ReagentBottle, targetModuleIndex);
                        }
                    }
                }
                this.dlgTurnTable = null;
            }
        }
        #endregion

        #region [privateメソッド]

        #region _コマンドバー_

        /// <summary>
        /// 洗浄液外部供給
        /// </summary>
        private void washSolutionExterior()
        {
            DlgReplaceTank dlg = new DlgReplaceTank();
            dlg.ShowDialog();
        }

        /// <summary>
        /// 交換開始
        /// </summary>
        /// <remarks>
        /// 交換開始処理を実行します
        /// </remarks>
        private void exchangeStart()
        {
            // 未選択の為、交換開始不可
            Int32 nSelectCount = this.getSelectBtnCount();
            if (nSelectCount == 0)
            {
                // 交換対象がない旨の警告メッセージ表示
                DlgMessage.Show( CarisX.Properties.Resources.STRING_DLG_MSG_174
                               , String.Empty
                               , CarisX.Properties.Resources.STRING_DLG_TITLE_001
                               , MessageDialogButtons.Confirm );
                return;
            }

            // TODO:ホストにワークシート問い合わせ時は交換処理を行えないようにする
            // TODO:編集 無接続時は交換不可

            // 交換開始共通処理
            this.startReagentChange(Singleton<PublicMemory>.Instance.moduleIndex);

            // 交換準備処理開始
            this.runNextTarget(ReagentChangeTargetKind.Start, Singleton<PublicMemory>.Instance.moduleIndex);

        }

        /// <summary>
        /// 大項目試薬釦点滅タイマー解除
        /// </summary>
        /// <remarks>
        /// 大項目試薬釦点滅タイマー解除します
        /// </remarks>
        private void cancelWaitBlinkTimer(ModuleIndex targetModuleIndex)
        {
            // 試薬交換用通知オブジェクト生成
            NotifyObjectSetReagent notifyObjectForCancelReagentTimer = new NotifyObjectSetReagent((int)targetModuleIndex, null);

            // 画面通知
            Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)(NotifyKind.CancelReagentTimer), notifyObjectForCancelReagentTimer);
        }


        /// <summary>
        /// 希釈液交換用の交換待機時間の取得
        /// </summary>
        /// <returns></returns>
        private Int32 getWaitBlinkTimerForDilution(ModuleIndex targetModuleIndex)
        {
            // 待機時間
            Int32 waitTime_min = 0;

            // 分析中検体の存在を確認
            if (Singleton<InProcessSampleInfoManager>.Instance.InProcessSampleList.Count > 0)
            {
                // 分析中検体の中から最小ポジションを取得する
                var inProcessSamples = from v in Singleton<InProcessSampleInfoManager>.Instance.InProcessSampleList
                                       where (v.IsAllWaiting() != true)
                                          && (v.ReactorPosition < CarisXConst.REAGENT_CHANGE_POSITION_MAX) // 59ポジション以降は無視する
                                          && (v.ModuleID == CarisXSubFunction.ModuleIndexToModuleId(targetModuleIndex))
                                       orderby v.ReactorPosition
                                       select v;
                if (inProcessSamples.Count() != 0)
                {
                    // 最小ポジション
                    Int32 minPos = inProcessSamples.First().ReactorPosition;

                    // サイクル時間
                    Int32 cycleTime = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CycleTimeParameter.CycleTime;

                    // 待機時間を算出
                    waitTime_min = ((CarisXConst.REAGENT_CHANGE_POSITION_MAX - (minPos + 1)) * cycleTime + 59) / 60;// 59秒足して60で割る事で分で切上げ
                }
            }

            return waitTime_min;
        }

        /// <summary>
        /// 大項目試薬釦点滅タイマー設定
        /// </summary>
        /// <remarks>
        /// 大項目試薬釦点滅タイマー設定します
        /// </remarks>
        /// <returns></returns>
        private Int32 setWaitBlinkTimer(ModuleIndex targetModuleIndex)
        {
            // 試薬ボトル設置可能になるまでの間、メニューの試薬ボタンに待ち時間（分）を表示する(メイン画面へ通知)
            Int32 waitTime_min = 0;

            // 分析中検体が存在するか確認
            if (Singleton<InProcessSampleInfoManager>.Instance.InProcessSampleList.Count > 0)
            {
                // 分析中検体の中から最小ポジションを取得する
                var inProcessSamples = from v in Singleton<InProcessSampleInfoManager>.Instance.InProcessSampleList
                                       where (v.IsAllWaiting() != true)
                                          && (v.ReactorPosition < CarisXConst.REAGENT_CHANGE_POSITION_MAX) // 59ポジション以降は無視する
                                          && (v.ModuleID == CarisXSubFunction.ModuleIndexToModuleId(targetModuleIndex))
                                       orderby v.ReactorPosition
                                       select v;

                if (inProcessSamples.Count() > 0)
                {
                    // 最小ポジション
                    Int32 minPos = inProcessSamples.First().ReactorPosition;

                    // サイクル時間
                    Int32 cycleTime = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CycleTimeParameter.CycleTime;

                    // 待機時間を算出
                    waitTime_min = ((CarisXConst.REAGENT_CHANGE_POSITION_MAX - (minPos + 1)) * cycleTime + 59) / 60;// 59秒足して60で割る事で分で切上げ

                    if (waitTime_min > 0)
                    {
                        // 待ちうけフラグOn
                        this.setReagentStateManager[targetModuleIndex].WaitDispense = true;
                        this.setReagentStateManager[targetModuleIndex].WaitDispenseCompleted = false;
                        this.setReagentStateManager[targetModuleIndex].WaitExchangeResp = false;
                    }
                }
            }

            // 試薬交換用オブジェクト生成
            NotifyObjectSetReagent notifyObjectStartReagentTimer = new NotifyObjectSetReagent((int)targetModuleIndex, waitTime_min);

            // 画面通知
            Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)(NotifyKind.StartReagentTimer), notifyObjectStartReagentTimer);

            return waitTime_min;
        }

        /// <summary>
        /// 交換完了
        /// </summary>
        /// <remarks>
        /// 交換完了処理を実行します
        /// </remarks>
        private void exchangeComplete()
        {
            // TODO:ホストにワークシート問い合わせ時は交換処理を行えないようにする

            // 対象モジュールIndexを取得
            ModuleIndex targetModuleIndex = Singleton<PublicMemory>.Instance.moduleIndex;

            // 対象モジュールのシーケンスヘルパー取得
            CarisXSequenceHelper targetModuleSequenceHelper = Singleton<CarisXSequenceHelperManager>.Instance.Slave[(int)targetModuleIndex];

            #region _試薬ボトル_

            // 試薬ボトルが交換対象の場合
            if (this.setReagentStateManager[targetModuleIndex].Blink[this.btnReagentBottle].CurrentState == true)
            {
                // 試薬準備完了シーケンス完了するまでコマンドバーをDisableにする
                this.tlbCommandBar.Tools[EXCHANGE_START].SharedProps.Enabled = false;
                this.tlbCommandBar.Tools[EXCHANGE_COMPLETE].SharedProps.Enabled = false;
                this.tlbCommandBar.Tools[EXCHANGE_CANCEL].SharedProps.Enabled = false;
                this.tlbCommandBar.Tools[EDIT].SharedProps.Enabled = false;

                // 状態を取得
                this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_START].Enabled = false;
                this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_COMPLETE].Enabled = false;
                this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_CANCEL].Enabled = false;
                this.setReagentStateManager[targetModuleIndex].ToolBar[EDIT].Enabled = false;

                // 試薬準備完了シーケンス開始
                this.sequenceCommData.Add(targetModuleSequenceHelper.PrepareCompleteReagentBottleSequence());
            }

            #endregion

            #region _廃液バッファ_

            // 廃液バッファが交換対象の場合
            if (this.setReagentStateManager[targetModuleIndex].Blink[this.btnWasteBuffer].CurrentState == true)
            {
                // 廃液ボトルセット完了コマンド送信
                SlaveCommCommand_0436 cmd0436 = new SlaveCommCommand_0436();
                cmd0436.tankBufferKind = TankBufferKind.Buffer;
                this.sequenceCommData.Add(targetModuleSequenceHelper.PrepareCompleteSequence(cmd0436
                                                                                           , CommandKind.Command1436
                                                                                           , ReagentChangeTargetKind.WasteBuffer));
            }

            #endregion

            #region _洗浄液バッファ_

            // 洗浄液バッファが交換対象の場合
            if (this.setReagentStateManager[targetModuleIndex].Blink[this.btnWashSolutionBuffer].CurrentState == true)
            {
                // 洗浄液供給コマンド送信
                SlaveCommCommand_0495 cmd0495 = new SlaveCommCommand_0495();
                cmd0495.Status = SlaveCommCommand_0495.WashSolutionSupplyStatus.Stop;
                cmd0495.tankBufferKind = TankBufferKind.Buffer;
                this.sequenceCommData.Add(targetModuleSequenceHelper.PrepareCompleteSequence(cmd0495
                                                                                           , CommandKind.Command1495
                                                                                           , ReagentChangeTargetKind.WashSolutionBuffer));
            }

            #endregion

            #region _プレトリガ_

            // プレトリガ1が交換対象の場合
            if (this.setReagentStateManager[targetModuleIndex].Blink[this.btnPretrigger1].CurrentState == true)
            {
                // プレトリガ準備完了コマンド送信
                SlaveCommCommand_0422 cmd0422 = new SlaveCommCommand_0422();
                // ポート番号
                cmd0422.BottleNo = inpPretriggerVal[0].portNo;
                // 残量
                cmd0422.Remain = inpPretriggerVal[0].remain;
                // ロット番号
                cmd0422.LotNumber = inpPretriggerVal[0].lotNo.ToString("00000000");
                // 有効期限
                cmd0422.TermOfUse = inpPretriggerVal[0].termOfUse;
                this.sequenceCommData.Add(targetModuleSequenceHelper.PrepareCompleteSequence(cmd0422
                                                                                           , CommandKind.Command1422
                                                                                           , (ReagentChangeTargetKind.Pretrigger | ReagentChangeTargetKind.Port1)));
            }

            // プレトリガ2が交換対象の場合
            if (this.setReagentStateManager[targetModuleIndex].Blink[this.btnPretrigger2].CurrentState == true)
            {
                // プレトリガ準備完了コマンド送信
                SlaveCommCommand_0422 cmd0422 = new SlaveCommCommand_0422();
                // ポート番号
                cmd0422.BottleNo = inpPretriggerVal[1].portNo;
                // 残量
                cmd0422.Remain = inpPretriggerVal[1].remain;
                // ロット番号
                cmd0422.LotNumber = inpPretriggerVal[1].lotNo.ToString("00000000");
                // 有効期限
                cmd0422.TermOfUse = inpPretriggerVal[1].termOfUse;
                this.sequenceCommData.Add(targetModuleSequenceHelper.PrepareCompleteSequence(cmd0422
                                                                                           , CommandKind.Command1422
                                                                                           , (ReagentChangeTargetKind.Pretrigger | ReagentChangeTargetKind.Port2)));
            }

            #endregion

            #region _トリガ_

            // トリガ1が交換対象の場合
            if (this.setReagentStateManager[targetModuleIndex].Blink[this.btnTrigger1].CurrentState == true)
            {
                // トリガ準備完了コマンド送信
                SlaveCommCommand_0424 cmd0424 = new SlaveCommCommand_0424();
                // ポート番号
                cmd0424.BottleNo = inpTriggerVal[0].portNo;
                // 残量
                cmd0424.Remain = inpTriggerVal[0].remain;
                // ロット番号
                cmd0424.LotNumber = inpTriggerVal[0].lotNo.ToString("00000000");
                // 有効期限
                cmd0424.TermOfUse = inpTriggerVal[0].termOfUse;
                this.sequenceCommData.Add(targetModuleSequenceHelper.PrepareCompleteSequence(cmd0424
                                                                                           , CommandKind.Command1424
                                                                                           , (ReagentChangeTargetKind.Trigger | ReagentChangeTargetKind.Port1)));
            }

            // トリガ2が交換対象の場合
            if (this.setReagentStateManager[targetModuleIndex].Blink[this.btnTrigger2].CurrentState == true)
            {
                // トリガ準備完了コマンド送信
                SlaveCommCommand_0424 cmd0424 = new SlaveCommCommand_0424();
                // ポート番号
                cmd0424.BottleNo = inpTriggerVal[1].portNo;
                // 残量
                cmd0424.Remain = inpTriggerVal[1].remain;
                // ロット番号
                cmd0424.LotNumber = inpTriggerVal[1].lotNo.ToString("00000000");
                // 有効期限
                cmd0424.TermOfUse = inpTriggerVal[1].termOfUse;
                this.sequenceCommData.Add(targetModuleSequenceHelper.PrepareCompleteSequence(cmd0424
                                                                                           , CommandKind.Command1424
                                                                                           , (ReagentChangeTargetKind.Trigger | ReagentChangeTargetKind.Port2)));
            }

            #endregion

            #region _希釈液_

            // 希釈液が交換対象の場合
            if (this.setReagentStateManager[targetModuleIndex].Blink[this.btnDiluent].CurrentState == true)
            {
                // 希釈液準備完了シーケンス完了するまでコマンドバーをDisableにする
                this.tlbCommandBar.Tools[EXCHANGE_START].SharedProps.Enabled = false;
                this.tlbCommandBar.Tools[EXCHANGE_COMPLETE].SharedProps.Enabled = false;
                this.tlbCommandBar.Tools[EXCHANGE_CANCEL].SharedProps.Enabled = false;
                this.tlbCommandBar.Tools[EDIT].SharedProps.Enabled = false;

                // 状態を取得
                this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_START].Enabled = false;
                this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_COMPLETE].Enabled = false;
                this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_CANCEL].Enabled = false;
                this.setReagentStateManager[targetModuleIndex].ToolBar[EDIT].Enabled = false;

                // 希釈液準備完了シーケンス開始
                this.sequenceCommData.Add(targetModuleSequenceHelper.PrepareCompleteDiluentSequence(this.inpDilVal.remain
                                                                                                  , this.inpDilVal.lotNo
                                                                                                  , this.inpDilVal.termOfUse));
            }

            #endregion

            #region _分注チップ、反応容器_

            foreach (BlinkButton btn in this.samplingTipCellButtons)
            {
                // 分注チップが１つでも交換対象の場合
                if (this.setReagentStateManager[targetModuleIndex].Blink[btn].CurrentState == true)
                {
                    // サンプル分注チップ準備完了コマンド送信
                    SlaveCommCommand_0426 cmd0426 = new SlaveCommCommand_0426();
                    this.sequenceCommData.Add(targetModuleSequenceHelper.PrepareCompleteSequence(cmd0426
                                                                                               , CommandKind.Command1426
                                                                                               , ReagentChangeTargetKind.SamplingTip));

                    //コマンドを送信したらループを抜ける
                    break;
                }
            }

            #endregion
        }

        /// <summary>
        /// 交換中断
        /// </summary>
        /// <remarks>
        /// 交換中断処理を実行します
        /// </remarks>
        private void exchangeCancel()
        {
            // CarisXSequenceHelperでレスポンス待機中の処理の中断。
            foreach (CarisXSequenceHelper.SequenceCommData SequenceData in this.sequenceCommData)
            {
                SequenceData.AbortWait();
            }
            this.sequenceCommData = null;
            this.sequenceCommData = new List<CarisXSequenceHelper.SequenceCommData>();

            // 準備中断コマンド送信
            Singleton<CarisXSequenceHelperManager>.Instance.Slave[(int)Singleton<PublicMemory>.Instance.moduleIndex].ExchangeCancelSequence();
        }

        /// <summary>
        /// 編集
        /// </summary>
        /// <remarks>
        /// 操作履歴に編集実行を登録し、データ編集処理を実行します
        /// </remarks>
        private void editData()
        {
            DialogResult dlgRtn = DialogResult.OK;

            // 複数のボタンが選択されている場合は、注意を促すダイアログを表示し、編集ダイアログは表示しない。
            Int32 nSelectCount = this.getSelectBtnCount();
            if (nSelectCount > 1)
            {
                // 2つ以上選択してるので注意を促すダイアログを表示する
                DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_175
                              , String.Empty
                              , CarisX.Properties.Resources.STRING_DLG_TITLE_001
                              , MessageDialogButtons.Confirm);
                return;
            }
            else if (nSelectCount == 0)
            {
                // 未選択なので注意を促すダイアログを表示する
                DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_176
                              , String.Empty
                              , CarisX.Properties.Resources.STRING_DLG_TITLE_001
                              , MessageDialogButtons.Confirm);
                return;
            }

            // 対象モジュールのシーケンスヘルパーを取得
            CarisXSequenceHelper targetModuleSequenceHelper = Singleton<CarisXSequenceHelperManager>.Instance.Slave[(int)Singleton<PublicMemory>.Instance.moduleIndex];

            // 編集機能の利用可否問い合わせ結果
            Boolean askEnableAction = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.ReagentRemainModify);

            // 試薬ボトル
            if (this.btnReagentBottle.CurrentState == true)
            {
                // 編集機能の利用可否問い合わせ結果がNGの場合
                if (askEnableAction == false)
                {
                    // 編集不可の旨のメッセージ表示
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_240, String.Empty, String.Empty, MessageDialogButtons.OK);
                    return;
                }


                // 試薬編集操作中にする
                this.setReagentStateManager[Singleton<PublicMemory>.Instance.moduleIndex].TurnTableDispMode = DlgTurnTable.TurnTableDispMode.Edit;

                // 試薬残量変更シーケンスを開始する
                this.sequenceCommData.Add(targetModuleSequenceHelper.ChangeReagentRemainSequence());
            }

            //プレトリガー
            if ( (this.btnPretrigger1.CurrentState == true)
              || (this.btnPretrigger2.CurrentState == true) )
            {
                // 編集機能の利用可否問い合わせ結果がNGの場合
                if (askEnableAction == false)
                {
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_240, String.Empty, String.Empty, MessageDialogButtons.OK);
                    return;
                }

            }

            //トリガー
            if ( (this.btnTrigger1.CurrentState == true)
              || (this.btnTrigger2.CurrentState == true) )
            {
                // 編集機能の利用可否問い合わせ結果がNGの場合
                if (askEnableAction == false)
                {
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_240, String.Empty, String.Empty, MessageDialogButtons.OK);
                    return;
                }
            }

            //希釈液
            if (this.btnDiluent.CurrentState == true)
            {
                // 編集機能の利用可否問い合わせ結果がNGの場合
                if (askEnableAction == false)
                {
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_240, String.Empty, String.Empty, MessageDialogButtons.OK);
                    return;
                }
            }

            // 廃液バッファ
            if (this.btnWasteBuffer.CurrentState == true)
            {
                // 廃液バッファは編集不可
                DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_177
                              , String.Empty
                              , CarisX.Properties.Resources.STRING_DLG_TITLE_001
                              , MessageDialogButtons.Confirm);

                // 廃液バッファボタンをOFF
                this.btnWasteBuffer.CurrentState = false;
            }

            // 洗浄液バッファ
            if (this.btnWashSolutionBuffer.CurrentState == true)
            {
                // 洗浄液バッファは編集不可
                DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_246
                              , String.Empty
                              , CarisX.Properties.Resources.STRING_DLG_TITLE_001
                              , MessageDialogButtons.Confirm);

                // 洗浄液バッファボタンをOFF
                this.btnWashSolutionBuffer.CurrentState = false;
            }

            // 操作履歴登録：編集実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist
                                                     , Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                                                     , CarisXLogInfoBaseExtention.Empty
                                                     , new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_042 });

            // ReagentDBのデータ更新(DBから取得)
            Singleton<ReagentDB>.Instance.LoadDB();

            // 更新後の試薬情報を再取得
            var reagentData = Singleton<ReagentDB>.Instance.GetData();
            Action<BlinkButton, ReagentKind, Int32> reagentEdit = (btn, kind, portNo) =>
                {
                    var editTarget = reagentData.FirstOrDefault((data) => (ReagentKind)data.ReagentKind == kind && data.PortNo == portNo);
                    if (btn.CurrentState && editTarget != null)
                    {
                        // 種別がサンプリングチップセル以外の場合
                        if (kind != ReagentKind.SamplingTip)
                        {
                            // ロット番号が設定されているか確認
                            if (String.IsNullOrEmpty(editTarget.LotNo))
                            {
                                // 設置されていないので編集できない旨をダイアログ表示する
                                DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_178
                                              , String.Empty
                                              , CarisX.Properties.Resources.STRING_DLG_TITLE_001
                                              , MessageDialogButtons.Confirm);
                                return;
                            }
                        }

                        // 編集ダイアログ表示
                        DlgEditReagent dlg = new DlgEditReagent();

                        // 種別がサンプリングチップセルの場合
                        if (kind == ReagentKind.SamplingTip)
                        {
                            // 固定で設定
                            dlg.Caption = Properties.Resources.STRING_SETREAGENT_020;
                        }
                        else
                        {
                            // ボタン名を設定
                            dlg.Caption = btn.Text;
                        }

                        // 種別設定
                        dlg.Kind = kind;

                        // 種別が希釈液以外の場合
                        if (kind != ReagentKind.Diluent)
                        {
                            // ポート番号設定
                            dlg.PortNo = portNo;
                        }

                        // 種別がサンプリングチップセル以外の場合
                        if (kind != ReagentKind.SamplingTip)
                        {
                            // ロット番号設定
                            dlg.LotNumber = editTarget.LotNo;

                            // 種別が希釈液以外の場合
                            if (kind != ReagentKind.Diluent)
                            {
                                // シリアル番号設定
                                dlg.SerialNumber = editTarget.SerialNo.Value;
                            }
                        }
                        // 残量設定
                        dlg.Remain = CarisXSubFunction.GetDispRemainCount(kind, this.getRemain(kind, portNo));

                        // プレトリガーまたはトリガーまたは希釈液の場合
                        if ((kind == ReagentKind.Pretrigger)
                         || (kind == ReagentKind.Trigger)
                         || (kind == ReagentKind.Diluent))
                        {
                            // プレトリガ、トリガ、希釈液の場合、容量も必要
                            dlg.Capacity = editTarget.Capacity;
                        }

                        // 編集ダイアログを表示
                        dlg.ShowDialog();

                        // 結果取得
                        dlgRtn = dlg.DialogResult;
                        if (dlgRtn == DialogResult.OK)
                        {
                            // 残量変更コマンド送信

                            CarisXCommCommand cmd = null;
                            switch (kind)
                            {
                                case ReagentKind.Diluent:
                                    cmd = new SlaveCommCommand_0430();
                                    ((SlaveCommCommand_0430)cmd).LotNumber = dlg.LotNumber;                                         // ロット番号
                                    ((SlaveCommCommand_0430)cmd).Remain = CarisXSubFunction.GetRealRemainCount(kind, dlg.Remain);   // 残量
                                    break;
                                case ReagentKind.Pretrigger:
                                    cmd = new SlaveCommCommand_0431();
                                    ((SlaveCommCommand_0431)cmd).PortNumber = dlg.PortNo;
                                    ((SlaveCommCommand_0431)cmd).LotNumber = dlg.LotNumber;                                         // ロット番号
                                    ((SlaveCommCommand_0431)cmd).Remain = CarisXSubFunction.GetRealRemainCount(kind, dlg.Remain);   // 残量
                                    break;
                                case ReagentKind.Trigger:
                                    cmd = new SlaveCommCommand_0432();
                                    ((SlaveCommCommand_0432)cmd).PortNumber = dlg.PortNo;
                                    ((SlaveCommCommand_0432)cmd).LotNumber = dlg.LotNumber;                                         // ロット番号
                                    ((SlaveCommCommand_0432)cmd).Remain = CarisXSubFunction.GetRealRemainCount(kind, dlg.Remain);   // 残量
                                    break;
                                case ReagentKind.SamplingTip:
                                    cmd = new SlaveCommCommand_0433();
                                    ((SlaveCommCommand_0433)cmd).PortNumber = dlg.PortNo;
                                    ((SlaveCommCommand_0433)cmd).Remain = CarisXSubFunction.GetRealRemainCount(kind, dlg.Remain);   // 残量
                                    break;
                            }

                            // コマンド送信
                            this.sequenceCommData.Add(Singleton<CarisXSequenceHelperManager>.Instance.Slave[(int)Singleton<PublicMemory>.Instance.moduleIndex]
                                .ChangeCommonRemainSequence(cmd, dlg.Kind, dlg.PortNo, CarisXSubFunction.GetRealRemainCount(dlg.Kind, dlg.Remain), dlg.LotNumber, dlg.SerialNumber));
                        }
                        else
                        {
                            btn.CurrentState = false;
                            // 操作履歴登録：編集キャンセル 
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_044 });
                        }
                    }
                };

            //下記の内、選択されているいずれか１つが動作する（reagentEdit内で分岐）
            // プレトリガー1
            reagentEdit(this.btnPretrigger1, ReagentKind.Pretrigger, 1);
            // プレトリガー2
            reagentEdit(this.btnPretrigger2, ReagentKind.Pretrigger, 2);
            // トリガー1
            reagentEdit(this.btnTrigger1, ReagentKind.Trigger, 1);
            // トリガー2
            reagentEdit(this.btnTrigger2, ReagentKind.Trigger, 2);
            // 希釈液
            reagentEdit(this.btnDiluent, ReagentKind.Diluent, 1);
            // サンプリングチップセル
            Int32 editPortNo = 1;
            foreach (BlinkButton btn in this.samplingTipCellButtons)
            {
                reagentEdit(btn, ReagentKind.SamplingTip, editPortNo++);
            }
        }

        #endregion

        /// <summary>
        /// 試薬交換中ステータス設定
        /// </summary>
        /// <remarks>
        /// 試薬交換中ステータス設定します
        /// </remarks>
        private void setReagentExchangeStatus(ModuleIndex targetModuleIndex)
        {
            // モジュールIndexを取得
            Int32 moduleId = CarisXSubFunction.ModuleIndexToModuleId(targetModuleIndex);

            // 試薬交換中状態以外の場合
            if (Singleton<SystemStatus>.Instance.ModuleStatus[moduleId] != SystemStatusKind.ReagentExchange)
            {
                // 交換前のステータスを退避しておく
                this.setReagentStateManager[targetModuleIndex].BeforeStatusKind = Singleton<SystemStatus>.Instance.ModuleStatus[moduleId];

                // モジュールのステータスを交換開始中にする
                Singleton<SystemStatus>.Instance.setModuleStatus((RackModuleIndex)moduleId, SystemStatusKind.ReagentExchange);
            }
        }

        /// <summary>
        /// 交換準備の処理を順番に実施する
        /// </summary>
        /// <remarks>
        /// 廃液バッファ→洗浄液バッファ→プレトリガ→トリガ→希釈液→チップ１～８の順に処理
        /// 処理対象が決定したら一度処理を抜け、レスポンスが来たら再度呼び出す
        /// </remarks>
        /// <param name="target">直前に処理したボトル</param>
        private void runNextTarget(ReagentChangeTargetKind target, ModuleIndex targetModuleIndex)
        {
            bool runstate = false;

            // 試薬ボトルが交換対象の場合
            if ((this.setReagentStateManager[targetModuleIndex].Blink[this.btnReagentBottle].CurrentState == true)
              && (target < ReagentChangeTargetKind.ReagentBottle))
            {
                this.sendCommonPrepareStart(ReagentChangeTargetKind.ReagentBottle, targetModuleIndex);
                runstate = true;
            }

            //廃液バッファ
            if ((runstate == false)
              && (this.setReagentStateManager[targetModuleIndex].Blink[this.btnWasteBuffer].CurrentState == true)
              && (target < ReagentChangeTargetKind.WasteBuffer))
            {
                this.sendCommonPrepareStart(ReagentChangeTargetKind.WasteBuffer, targetModuleIndex);
                runstate = true;
            }

            //洗浄液バッファ
            if ((runstate == false)
              && (this.setReagentStateManager[targetModuleIndex].Blink[this.btnWashSolutionBuffer].CurrentState == true)
              && (target < ReagentChangeTargetKind.WashSolutionBuffer))
            {
                this.sendCommonPrepareStart(ReagentChangeTargetKind.WashSolutionBuffer, targetModuleIndex);
                runstate = true;
            }

            //プレトリガ
            if ((runstate == false)
              && ((target < ReagentChangeTargetKind.Pretrigger)
                || (target.HasFlag(ReagentChangeTargetKind.Pretrigger) == true)))
            {
                if ((this.setReagentStateManager[targetModuleIndex].Blink[this.btnPretrigger1].CurrentState == true)
                  && (this.setReagentStateManager[targetModuleIndex].Blink[this.btnPretrigger1].IsBlink == false))
                {
                    this.sendCommonPrepareStart((ReagentChangeTargetKind.Pretrigger | ReagentChangeTargetKind.Port1), targetModuleIndex);
                    runstate = true;
                }
                else if ((this.setReagentStateManager[targetModuleIndex].Blink[this.btnPretrigger2].CurrentState == true)
                       && (this.setReagentStateManager[targetModuleIndex].Blink[this.btnPretrigger2].IsBlink == false))
                {
                    this.sendCommonPrepareStart((ReagentChangeTargetKind.Pretrigger | ReagentChangeTargetKind.Port2), targetModuleIndex);
                    runstate = true;
                }
            }

            //トリガ
            if ((runstate == false)
              && ((target < ReagentChangeTargetKind.Trigger)
                || (target.HasFlag(ReagentChangeTargetKind.Trigger) == true)))
            {
                if ((this.setReagentStateManager[targetModuleIndex].Blink[this.btnTrigger1].CurrentState == true)
                  && (this.setReagentStateManager[targetModuleIndex].Blink[this.btnTrigger1].IsBlink == false))
                {
                    this.sendCommonPrepareStart((ReagentChangeTargetKind.Trigger | ReagentChangeTargetKind.Port1), targetModuleIndex);
                    runstate = true;
                }
                else if (btnTrigger2.CurrentState && !btnTrigger2.IsBlink)
                {
                    this.sendCommonPrepareStart((ReagentChangeTargetKind.Trigger | ReagentChangeTargetKind.Port2), targetModuleIndex);
                    runstate = true;
                }
            }

            //希釈液
            if ((runstate == false)
              && (this.setReagentStateManager[targetModuleIndex].Blink[this.btnDiluent].CurrentState == true)
              && (target < ReagentChangeTargetKind.Diluent))
            {
                this.sendCommonPrepareStart(ReagentChangeTargetKind.Diluent, targetModuleIndex);
                runstate = true;
            }

            //チップ
            if ((runstate == false)
              && (target < ReagentChangeTargetKind.SamplingTip))
            {
                if ((this.setReagentStateManager[targetModuleIndex].Blink[this.btnSamplingTipCell1].CurrentState == true)
                  || (this.setReagentStateManager[targetModuleIndex].Blink[this.btnSamplingTipCell2].CurrentState == true)
                  || (this.setReagentStateManager[targetModuleIndex].Blink[this.btnSamplingTipCell3].CurrentState == true)
                  || (this.setReagentStateManager[targetModuleIndex].Blink[this.btnSamplingTipCell4].CurrentState == true)
                  || (this.setReagentStateManager[targetModuleIndex].Blink[this.btnSamplingTipCell5].CurrentState == true)
                  || (this.setReagentStateManager[targetModuleIndex].Blink[this.btnSamplingTipCell6].CurrentState == true)
                  || (this.setReagentStateManager[targetModuleIndex].Blink[this.btnSamplingTipCell7].CurrentState == true)
                  || (this.setReagentStateManager[targetModuleIndex].Blink[this.btnSamplingTipCell8].CurrentState == true))
                {
                    this.sendCommonPrepareStart(ReagentChangeTargetKind.SamplingTip, targetModuleIndex);
                    runstate = true;
                }
            }
        }

        /// <summary>
        /// 準備開始コマンド送信
        /// </summary>
        /// <remarks>
        /// 準備開始コマンドの送信を行います。
        /// </remarks>
        /// <param name="target">交換対象フラグ</param>
        /// <param name="targetModuleIndex">対象モジュールIndex</param>
        /// <returns>なし</returns>
        private void sendCommonPrepareStart(ReagentChangeTargetKind target, ModuleIndex targetModuleIndex)
        {
            // 画面反映フラグ
            Boolean isReflectDisplay = false;
            if (targetModuleIndex == Singleton<PublicMemory>.Instance.moduleIndex)
            {
                isReflectDisplay = true;
            }

            // 試薬が交換対象の場合
            if (target.HasFlag(ReagentChangeTargetKind.ReagentBottle))
            {
                // 試薬交換中ステータスに切替
                this.setReagentExchangeStatus(targetModuleIndex);

                // 試薬交換操作中にする
                this.setReagentStateManager[targetModuleIndex].TurnTableDispMode = DlgTurnTable.TurnTableDispMode.Change;

                // 画面左メニューボタンの試薬ボタンの点滅タイマー時間取得
                Int32 waitTime_min = this.setWaitBlinkTimer(targetModuleIndex);

                // 試薬準備開始シーケンス開始
                this.sequenceCommData.Add(
                    Singleton<CarisXSequenceHelperManager>.Instance.Slave[(int)targetModuleIndex].PrepareReagentBottleSequence());

                // 分注待ち状態か確認
                if (this.setReagentStateManager[targetModuleIndex].WaitDispense == true)
                {
                    // 分注終了時点滅ボタンリストへ追加
                    this.setReagentStateManager[targetModuleIndex].BlinkListOnDispenceEnd.Add(this.btnReagentBottle);
                }
            }

            // 廃液バッファが交換対象の場合
            if (target.HasFlag(ReagentChangeTargetKind.WasteBuffer))
            {
                // 試薬交換中ステータスに切替
                this.setReagentExchangeStatus(targetModuleIndex);

                // 廃液ボトルセット開始コマンド送信
                SlaveCommCommand_0435 cmd0435 = new SlaveCommCommand_0435();
                cmd0435.tankBufferKind = TankBufferKind.Buffer;
                this.sequenceCommData.Add(
                    Singleton<CarisXSequenceHelperManager>.Instance.Slave[(int)targetModuleIndex].PrepareSequence(cmd0435, CommandKind.Command1435, target));
            }

            // 洗浄液バッファが交換対象の場合
            if (target.HasFlag(ReagentChangeTargetKind.WashSolutionBuffer))
            {
                // 試薬交換中ステータスに切替
                this.setReagentExchangeStatus(targetModuleIndex);

                // 洗浄液供給コマンド送信
                SlaveCommCommand_0495 cmd0495 = new SlaveCommCommand_0495();
                cmd0495.Status = SlaveCommCommand_0495.WashSolutionSupplyStatus.Start;
                cmd0495.tankBufferKind = TankBufferKind.Buffer;
                this.sequenceCommData.Add(
                    Singleton<CarisXSequenceHelperManager>.Instance.Slave[(int)targetModuleIndex].PrepareSequence(cmd0495, CommandKind.Command1495, target));
            }

            // プレトリガが交換対象の場合
            if (target.HasFlag(ReagentChangeTargetKind.Pretrigger))
            {
                // 試薬交換中ステータスに切替
                this.setReagentExchangeStatus(targetModuleIndex);

                if (isReflectDisplay == true)
                {
                    // ボトルバーコード読取ダイアログを表示
                    DlgReadBCBottle dlg = new DlgReadBCBottle();

                    // プレトリガー用のタイトル設定
                    dlg.Caption = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_022;

                    // ボトル1かボトル2か判定
                    if (target.HasFlag(ReagentChangeTargetKind.Port1))
                    {
                        // プレトリガー1を設定
                        dlg.TargetName = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_023;
                    }
                    else
                    {
                        // プレトリガー2を設定
                        dlg.TargetName = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_026;
                    }

                    // 種別にプレトリガーを設定
                    dlg.ReagentKind = ReagentKind.Pretrigger;
                    dlg.ShowDialog();

                    // ボトルバーコード読取待ちを解除
                    this.setReagentStateManager[targetModuleIndex].IsShowReadBCBottle = false;
                    this.setReagentStateManager[targetModuleIndex].WaitSetCmdResCommonPrepareStart = 0;

                    // バーコード読取完了
                    if (dlg.DialogResult == DialogResult.OK)
                    {
                        //【IssuesNo:12】记录预激发液更换后的条码信息
                        this.barCodeResult = dlg.BarcodeResult;
                        // ボトル番号取得
                        Int32 portNo = target.HasFlag(ReagentChangeTargetKind.Port1) ? 1 : 2; //Port1のフラグが含まれている場合は1、含まれていない場合は2

                        // プレトリガ準備完了コマンド用のデータを準備
                        // プレトリガバーコードフォーマット参照
                        this.inpPretriggerVal[portNo - 1].portNo = portNo;
                        this.inpPretriggerVal[portNo - 1].remain = CarisXConst.REMAIN_PRETRIGGER_TRIGGER_MAX_1; //200000μL （300000μLはCarisXにはない）
                        this.inpPretriggerVal[portNo - 1].lotNo = int.Parse(dlg.BarcodeResult.Substring(1, 8));
                        this.inpPretriggerVal[portNo - 1].termOfUse = dlg.BarcodeResultDateTime;

                        // バーコード記載の満杯量にボトル容量を更新
                        var pretriggerData = Singleton<ReagentDB>.Instance.GetData(ReagentKind.Pretrigger).First((data) => data.PortNo == portNo);
                        pretriggerData.Capacity = dlg.BarcodeResultCapacity;
                        Singleton<ReagentDB>.Instance.SetData(new List<ReagentData>() { pretriggerData });

                        // プレトリガ準備開始コマンド送信
                        SlaveCommCommand_0421 cmd0421 = new SlaveCommCommand_0421();
                        cmd0421.BottleNo = portNo;
                        this.sequenceCommData.Add(
                            Singleton<CarisXSequenceHelperManager>.Instance.Slave[(int)targetModuleIndex].PrepareSequence(cmd0421, CommandKind.Command1421, target));
                    }
                    else
                    {
                        // 対象を交換完了またはキャンセルとする
                        this.finishReagentChange(target, targetModuleIndex);

                        //次の準備開始対象を処理
                        this.runNextTarget(target, targetModuleIndex);
                    }
                }
                else
                {
                    // ボトルバーコード読取待ちを設定
                    this.setReagentStateManager[targetModuleIndex].IsShowReadBCBottle = true;
                    this.setReagentStateManager[targetModuleIndex].WaitSetCmdResCommonPrepareStart = target;
                }
            }

            // トリガーが対象の場合
            if (target.HasFlag(ReagentChangeTargetKind.Trigger))
            {
                // 試薬交換中ステータスに切替
                this.setReagentExchangeStatus(targetModuleIndex);

                if (isReflectDisplay == true)
                {
                    // ボトルバーコード読取ダイアログを表示
                    DlgReadBCBottle dlg = new DlgReadBCBottle();

                    // トリガー用のタイトル設定
                    dlg.Caption = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_027;

                    // ボトル1かボトル2か判定
                    if (target.HasFlag(ReagentChangeTargetKind.Port1))
                    {
                        // トリガー1をラベルに設定
                        dlg.TargetName = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_028;
                    }
                    else
                    {
                        // トリガー2をラベルに設定
                        dlg.TargetName = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_029;
                    }

                    // 種別をトリガーに設定
                    dlg.ReagentKind = ReagentKind.Trigger;
                    dlg.ShowDialog();

                    // バーコード読取完了
                    if (dlg.DialogResult == DialogResult.OK)
                    {
                        //【IssuesNo:12】记录激发液更换后的条码信息
                        this.barCodeResult = dlg.BarcodeResult;
                        // ボトル番号を取得
                        Int32 portNo = target.HasFlag(ReagentChangeTargetKind.Port1) ? 1 : 2; //Port1のフラグが含まれている場合は1、含まれていない場合は2

                        // トリガ準備完了コマンド用のデータを準備
                        //トリガバーコードフォーマット参照
                        this.inpTriggerVal[portNo - 1].portNo = portNo;
                        this.inpTriggerVal[portNo - 1].remain = CarisXConst.REMAIN_PRETRIGGER_TRIGGER_MAX_1; //200000μL（300000μLはCarisXにはない）
                        this.inpTriggerVal[portNo - 1].lotNo = int.Parse(dlg.BarcodeResult.Substring(1, 8));
                        this.inpTriggerVal[portNo - 1].termOfUse = dlg.BarcodeResultDateTime;

                        // バーコード記載の満杯量にボトル容量を更新
                        var triggerData = Singleton<ReagentDB>.Instance.GetData(ReagentKind.Trigger).First((data) => data.PortNo == portNo);
                        triggerData.Capacity = dlg.BarcodeResultCapacity;
                        Singleton<ReagentDB>.Instance.SetData(new List<ReagentData>() { triggerData });

                        // トリガ準備開始コマンド送信
                        SlaveCommCommand_0423 cmd0423 = new SlaveCommCommand_0423();
                        cmd0423.BottleNo = portNo;
                        this.sequenceCommData.Add(Singleton<CarisXSequenceHelperManager>.Instance.Slave[(int)Singleton<PublicMemory>.Instance.moduleIndex]
                            .PrepareSequence(cmd0423, CommandKind.Command1423, target));
                    }
                    else
                    {
                        // 対象を交換完了またはキャンセルとする
                        this.finishReagentChange(target, targetModuleIndex);

                        //次の準備開始対象を処理
                        this.runNextTarget(target, targetModuleIndex);
                    }
                }
                else
                {
                    // ボトルバーコード読取待ちを設定
                    this.setReagentStateManager[targetModuleIndex].IsShowReadBCBottle = true;
                    this.setReagentStateManager[targetModuleIndex].WaitSetCmdResCommonPrepareStart = target;
                }
            }

            // 希釈液が交換対象の場合
            if (target.HasFlag(ReagentChangeTargetKind.Diluent))
            {
                // 試薬交換中ステータスに切替
                this.setReagentExchangeStatus(targetModuleIndex);

                // 画面左メニューボタンの試薬ボタンの点滅タイマー時間取得
                Int32 waitTime_min = this.getWaitBlinkTimerForDilution(targetModuleIndex);

                if (waitTime_min > 0)
                {
                    // 画面表示有無
                    if (isReflectDisplay)
                    {
                        // 現時点では希釈剤を交換することはできません。試薬の分注が完了するまでお待ちください。
                        DlgMessage.Show(Properties.Resources.STRING_DLG_MSG_245
                                      , String.Empty
                                      , Properties.Resources.STRING_DLG_TITLE_001
                                      , MessageDialogButtons.Confirm);

                        // ボトルバーコード読取待ちを解除
                        this.setReagentStateManager[targetModuleIndex].IsShowReadBCBottle = false;
                        this.setReagentStateManager[targetModuleIndex].WaitSetCmdResCommonPrepareStart = 0;

                        // 対象を交換完了またはキャンセルとする
                        this.finishReagentChange(target, targetModuleIndex);

                        //次の準備開始対象を処理
                        this.runNextTarget(target, targetModuleIndex);
                    }
                    else
                    {
                        // ボトルバーコード読取待ちを設定
                        this.setReagentStateManager[targetModuleIndex].IsShowReadBCBottle = true;
                        this.setReagentStateManager[targetModuleIndex].WaitSetCmdResCommonPrepareStart = target;
                    }

                    return;
                }


                // 画面表示有無
                if (isReflectDisplay)
                {
                    // ボトルバーコード読取ダイアログ表示
                    DlgReadBCBottle dlg = new DlgReadBCBottle();

                    // 希釈液用のタイトル設定
                    dlg.Caption = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_030;

                    // 希釈液をラベルに設定
                    dlg.TargetName = Oelco.CarisX.Properties.Resources.STRING_SETREAGENT_031;

                    // 種別を希釈液に設定
                    dlg.ReagentKind = ReagentKind.Diluent;
                    dlg.ShowDialog();

                    // ボトルバーコード読取待ちを解除
                    this.setReagentStateManager[targetModuleIndex].IsShowReadBCBottle = false;
                    this.setReagentStateManager[targetModuleIndex].WaitSetCmdResCommonPrepareStart = 0;

                    // バーコード読取完了
                    if (dlg.DialogResult == DialogResult.OK)
                    {
                        //【IssuesNo:12】记录稀释液更换后的条码信息
                        this.barCodeResult = dlg.BarcodeResult;
                        // 希釈液準備完了コマンド用のデータを準備
                        // 希釈液コードフォーマット参照
                        this.inpDilVal.portNo = 1;
                        this.inpDilVal.remain = CarisXConst.REMAIN_DILUENT_MAX_1; //200000μL（300000μLはCarisXにはない）
                        this.inpDilVal.lotNo = int.Parse(dlg.BarcodeResult.Substring(1, 8));
                        this.inpDilVal.termOfUse = dlg.BarcodeResultDateTime;

                        // バーコード記載の満杯量にボトル容量を更新
                        var diluentData = Singleton<ReagentDB>.Instance.GetData(ReagentKind.Diluent).First();
                        diluentData.Capacity = dlg.BarcodeResultCapacity;
                        Singleton<ReagentDB>.Instance.SetData(new List<ReagentData>() { diluentData });

                        // 希釈液準備開始シーケンス開始
                        this.sequenceCommData.Add(
                            Singleton<CarisXSequenceHelperManager>.Instance.Slave[(int)targetModuleIndex].PrepareDiluentSequence());
                    }
                    else
                    {
                        // 対象を交換完了またはキャンセルとする
                        this.finishReagentChange(target, targetModuleIndex);

                        // 次の準備開始対象を処理
                        this.runNextTarget(target, targetModuleIndex);
                    }
                }
                else
                {
                    // ボトルバーコード読取待ちを設定
                    this.setReagentStateManager[targetModuleIndex].IsShowReadBCBottle = true;
                    this.setReagentStateManager[targetModuleIndex].WaitSetCmdResCommonPrepareStart = target;
                }
            }

            // サンプリングチップが交換対象の場合
            if (target.HasFlag(ReagentChangeTargetKind.SamplingTip))
            {
                // サンプリングチップ交換準備開始フラグ
                Boolean isSamplingTipStart = false;
                SlaveCommCommand_0425 cmd0425 = new SlaveCommCommand_0425();
                Int32 nSamplingTipNo = 0;

                // 全てのサンプリングチップセルを確認
                foreach (BlinkButton btn in this.samplingTipCellButtons)
                {
                    // 指定のサンプリングチップセルが交換対象の場合
                    if (this.setReagentStateManager[targetModuleIndex].Blink[btn].CurrentState)
                    {
                        // 試薬交換中ステータスを設定
                        this.setReagentExchangeStatus(targetModuleIndex);

                        // 分注待ち状態か確認
                        if (this.setReagentStateManager[targetModuleIndex].WaitDispense)
                        {
                            // 分注終了時点滅対象ボタンリストに追加
                            this.setReagentStateManager[targetModuleIndex].BlinkListOnDispenceEnd.Add(btn);
                        }
                        else
                        {
                            // ボタンブリンク開始
                            this.setButtonBlink(true, btn, targetModuleIndex);
                        }

                        // サンプリングチップ交換準備開始フラグをONにする
                        isSamplingTipStart = true;

                        cmd0425.BottleNo[nSamplingTipNo] = SlaveCommCommand_0425.EXCHANGE_ON;
                    }
                    else
                    {
                        cmd0425.BottleNo[nSamplingTipNo] = SlaveCommCommand_0425.EXCHANGE_OFF;
                    }

                    nSamplingTipNo++;
                }

                // サンプリングチップ交換準備開始フラグを確認
                if (isSamplingTipStart)
                {
                    // サンプル分注チップ準備開始コマンド送信
                    this.sequenceCommData.Add(
                        Singleton<CarisXSequenceHelperManager>.Instance.Slave[(int)targetModuleIndex].PrepareSequence(cmd0425, CommandKind.Command1425, target));
                }
            }
        }

        /// <summary>
        /// 交換開始共通処理（コマンドバーEnable変更など）
        /// </summary>
        /// <remarks>
        /// 交換開始処理を実行します
        /// </remarks>
        private void startReagentChange(ModuleIndex targetModuleIndex)
        {
            // 交換開始したら、完了かキャンセルするまで各ボタンは操作不可にする
            this.setButtonEnable(false, (int)targetModuleIndex);

            if (targetModuleIndex == Singleton<PublicMemory>.Instance.moduleIndex)
            {
                // 交換開始、交換完了、編集コマンドバーDisable
                this.tlbCommandBar.Tools[EXCHANGE_START].SharedProps.Enabled = false;
                this.tlbCommandBar.Tools[EXCHANGE_COMPLETE].SharedProps.Enabled = false;
                this.tlbCommandBar.Tools[EDIT].SharedProps.Enabled = false;
                this.tlbCommandBar.Tools[REPLACE_TANK].SharedProps.Enabled = false;

                // 交換キャンセルコマンドバーEnable
                this.tlbCommandBar.Tools[EXCHANGE_CANCEL].SharedProps.Enabled = true;
            }

            // 状態記憶
            this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_START].Enabled = false;
            this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_COMPLETE].Enabled = false;
            this.setReagentStateManager[targetModuleIndex].ToolBar[EDIT].Enabled = false;
            this.setReagentStateManager[targetModuleIndex].ToolBar[REPLACE_TANK].Enabled = false;
            this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_CANCEL].Enabled = true;

            // 分注待機時点滅ボタンリスト初期化
            this.setReagentStateManager[targetModuleIndex].BlinkListOnDispenceEnd.Clear();

        }

        /// <summary>
        /// 交換完了・キャンセル共通処理（コマンドバーEnable変更など）
        /// </summary>
        /// <remarks>
        /// 交換完了・キャンセル共通処理を実行します
        /// </remarks>
        /// <param name="target">交換対象フラグ</param>
        /// <param name="targetModuleIndex">対象モジュールIndex</param>
        private void finishReagentChange(ReagentChangeTargetKind target, ModuleIndex targetModuleIndex)
        {
            Boolean isStatusAllOk = true;

            // 画面反映フラグ
            Boolean isReflectDisplay = false;
            if (targetModuleIndex == Singleton<PublicMemory>.Instance.moduleIndex)
            {
                isReflectDisplay = true;
            }

            // 指定の対象ボタンの点滅を終了し、ステータスを戻す。(Allの場合は全てが対象)

            // 試薬ボトルが交換対象の場合
            if (target == ReagentChangeTargetKind.ReagentBottle || target == ReagentChangeTargetKind.All)
            {
                this.setReagentStateManager[targetModuleIndex].WaitDispense = false;
                this.setReagentStateManager[targetModuleIndex].WaitDispenseCompleted = false;
                this.setReagentStateManager[targetModuleIndex].WaitExchangeResp = false;

                // ボタン点滅停止
                this.setButtonBlink(false, this.btnReagentBottle, targetModuleIndex);

                // ステータスを戻す
                if (isReflectDisplay == true)
                {
                    this.btnReagentBottle.CurrentState = false;
                }
                this.setReagentStateManager[targetModuleIndex].Blink[this.btnReagentBottle].CurrentState = false;

                // 大項目試薬釦点滅タイマ中断通知を送信する
                this.cancelWaitBlinkTimer(targetModuleIndex);

                // 複数台構成の場合、ガイダンス表示を通知する
                if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected > 1)
                {
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ShowReagentSetGuidance, targetModuleIndex);
                }
            }

            // 廃液バッファが交換対象の場合
            if (target == ReagentChangeTargetKind.WasteBuffer || target == ReagentChangeTargetKind.All)
            {
                // ボタン点滅停止
                this.setButtonBlink(false, this.btnWasteBuffer, targetModuleIndex);

                // ステータスを戻す
                if (isReflectDisplay == true)
                {
                    this.btnWasteBuffer.CurrentState = false;
                }
                this.setReagentStateManager[targetModuleIndex].Blink[this.btnWasteBuffer].CurrentState = false;
            }

            // 洗浄液バッファが交換対象の場合
            if (target == ReagentChangeTargetKind.WashSolutionBuffer || target == ReagentChangeTargetKind.All)
            {
                // ボタン点滅停止
                this.setButtonBlink(false, this.btnWashSolutionBuffer, targetModuleIndex);

                // ステータスを戻す
                if (isReflectDisplay == true)
                {
                    this.btnWashSolutionBuffer.CurrentState = false;
                }
                this.setReagentStateManager[targetModuleIndex].Blink[this.btnWashSolutionBuffer].CurrentState = false;
            }

            //【IssuesNo:12】预激发液、激发液和稀释液更换完成后，条码写入注册表中
            if(target.HasFlag(ReagentChangeTargetKind.Pretrigger)|| target.HasFlag(ReagentChangeTargetKind.Trigger)|| target.HasFlag(ReagentChangeTargetKind.Diluent))
            {
                CarisXSubFunction.RegistBarCodeInfo(this.barCodeResult);
                //重置
                this.barCodeResult = string.Empty;
            }

            // プレトリガが交換対象の場合
            if (target.HasFlag(ReagentChangeTargetKind.Pretrigger) || target.HasFlag(ReagentChangeTargetKind.All))
            {
                if (target.HasFlag(ReagentChangeTargetKind.Port1) || target.HasFlag(ReagentChangeTargetKind.All))
                {
                    // ボタン点滅停止
                    this.setButtonBlink(false, this.btnPretrigger1, targetModuleIndex);

                    // ステータスを戻す
                    if (isReflectDisplay == true)
                    {
                        this.btnPretrigger1.CurrentState = false;
                    }
                    this.setReagentStateManager[targetModuleIndex].Blink[this.btnPretrigger1].CurrentState = false;
                }

                if (target.HasFlag(ReagentChangeTargetKind.Port2) || target.HasFlag(ReagentChangeTargetKind.All))
                {
                    // ボタン点滅停止
                    this.setButtonBlink(false, this.btnPretrigger2, targetModuleIndex);

                    // ステータスを戻す
                    if (isReflectDisplay == true)
                    {
                        this.btnPretrigger2.CurrentState = false;
                    }
                    this.setReagentStateManager[targetModuleIndex].Blink[this.btnPretrigger2].CurrentState = false;
                }
            }

            // トリガが交換対象の場合
            if (target.HasFlag(ReagentChangeTargetKind.Trigger) || target.HasFlag(ReagentChangeTargetKind.All))
            {
                if (target.HasFlag(ReagentChangeTargetKind.Port1) || target.HasFlag(ReagentChangeTargetKind.All))
                {
                    // ボタン点滅停止
                    this.setButtonBlink(false, this.btnTrigger1, targetModuleIndex);

                    // ステータスを戻す
                    if (isReflectDisplay == true)
                    {
                        this.btnTrigger1.CurrentState = false;
                    }
                    this.setReagentStateManager[targetModuleIndex].Blink[this.btnTrigger1].CurrentState = false;
                }

                if (target.HasFlag(ReagentChangeTargetKind.Port2) || target.HasFlag(ReagentChangeTargetKind.All))
                {
                    // ボタン点滅停止
                    this.setButtonBlink(false, this.btnTrigger2, targetModuleIndex);

                    // ステータスを戻す
                    if (isReflectDisplay == true)
                    {
                        this.btnTrigger2.CurrentState = false;
                    }
                    this.setReagentStateManager[targetModuleIndex].Blink[this.btnTrigger2].CurrentState = false;
                }
            }

            // 希釈液が交換対象の場合
            if (target == ReagentChangeTargetKind.Diluent || target == ReagentChangeTargetKind.All)
            {
                // ボタン点滅停止
                this.setButtonBlink(false, this.btnDiluent, targetModuleIndex);

                // ステータスを戻す
                if (isReflectDisplay == true)
                {
                    this.btnDiluent.CurrentState = false;

                    // ボタンイメージ更新
                    var objImg = this.getButtonImageFromState(ReagentKind.Diluent, this.getRemain(ReagentKind.Diluent, 1));
                    this.btnDiluent.Appearance.Image = objImg;
                    this.btnDiluent.ToggleAppearance.Image = objImg;
                }
                this.setReagentStateManager[targetModuleIndex].Blink[this.btnDiluent].CurrentState = false;
            }

            // 分注チップ、反応容器が交換対象の場合
            if (target == ReagentChangeTargetKind.SamplingTip || target == ReagentChangeTargetKind.All)
            {
                // ボタン点滅停止
                this.setButtonBlink(false, this.samplingTipCellButtons, targetModuleIndex);

                // ステータスを戻す
                foreach (BlinkButton btn in this.samplingTipCellButtons)
                {
                    if (isReflectDisplay == true)
                    {
                        btn.CurrentState = false;
                    }
                    this.setReagentStateManager[targetModuleIndex].Blink[btn].CurrentState = false;
                }
            }

            // 試薬ボトル
            if (this.setReagentStateManager[targetModuleIndex].Blink[this.btnReagentBottle].CurrentState == true)
            {
                isStatusAllOk = false;
            }
            // 廃液バッファ
            if (this.setReagentStateManager[targetModuleIndex].Blink[this.btnWasteBuffer].CurrentState == true)
            {
                isStatusAllOk = false;
            }
            // 洗浄液バッファ
            if (this.setReagentStateManager[targetModuleIndex].Blink[this.btnWashSolutionBuffer].CurrentState == true)
            {
                isStatusAllOk = false;
            }
            // プレトリガ、トリガ、希釈液
            foreach (BlinkButton btn in this.pretriggerTriggerDiluentButtons)
            {
                if (this.setReagentStateManager[targetModuleIndex].Blink[btn].CurrentState == true)
                {
                    isStatusAllOk = false;
                    break;
                }
            }
            // 分注チップ、反応容器
            foreach (BlinkButton btn in this.samplingTipCellButtons)
            {
                if (this.setReagentStateManager[targetModuleIndex].Blink[btn].CurrentState == true)
                {
                    isStatusAllOk = false;
                    break;
                }
            }

            // 全てのステータスが戻っていたら、各ボタン操作可、コマンドバーのEnable変更
            if (isStatusAllOk)
            {
                this.sequenceCommData = null;
                this.sequenceCommData = new List<CarisXSequenceHelper.SequenceCommData>();

                // 交換完了、又はキャンセルしたら各ボタンは操作可にする
                this.setButtonEnable(true, (int)targetModuleIndex);

                if (isReflectDisplay)
                {
                    // 交換開始、編集コマンドバーEnable
                    this.tlbCommandBar.Tools[EXCHANGE_START].SharedProps.Enabled = true;
                    this.tlbCommandBar.Tools[EDIT].SharedProps.Enabled = true;
                    this.tlbCommandBar.Tools[REPLACE_TANK].SharedProps.Enabled = true;

                    // 交換完了、交換キャンセルコマンドバーDisable
                    this.tlbCommandBar.Tools[EXCHANGE_COMPLETE].SharedProps.Enabled = false;
                    this.tlbCommandBar.Tools[EXCHANGE_CANCEL].SharedProps.Enabled = false;
                }

                // 交換開始、編集コマンドバーEnable
                this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_START].Enabled = true;
                this.setReagentStateManager[targetModuleIndex].ToolBar[EDIT].Enabled = true;
                this.setReagentStateManager[targetModuleIndex].ToolBar[REPLACE_TANK].Enabled = true;

                // 交換完了、交換キャンセルコマンドバーDisable
                this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_COMPLETE].Enabled = false;
                this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_CANCEL].Enabled = false;

                // ステータスの復元は全ての交換状態が解除された時
                // システムステータスを元に戻す
                RackModuleIndex moduleId = (RackModuleIndex)CarisXSubFunction.ModuleIndexToModuleId(targetModuleIndex);
                Singleton<SystemStatus>.Instance.setModuleStatus(moduleId, this.setReagentStateManager[targetModuleIndex].BeforeStatusKind);
            }
        }

        /// <summary>
        /// 全ボタンのEnable変更
        /// </summary>
        /// <remarks>
        ///  全ボタンのEnable変更処理を実行します
        /// </remarks>
        private void setButtonEnable(Boolean enable, int targetModuleIndex = CarisXConst.ALL_MODULEID)
        {
            // モジュールIndexの指定がある場合
            if (targetModuleIndex != CarisXConst.ALL_MODULEID)
            {
                // 画面反映の有無を確認
                if (targetModuleIndex == (int)Singleton<PublicMemory>.Instance.moduleIndex)
                {
                    //全選択/解除ボタン
                    this.btnPretriggerTriggerDiluentSelectAllSwitch.Enabled = enable;
                    this.btnSamplingTipSelectAllSwitch.Enabled = enable;

                    // 試薬ボトル
                    this.btnReagentBottle.Enabled = enable;
                    // 廃液バッファ
                    this.btnWasteBuffer.Enabled = enable;
                    // 洗浄液バッファ
                    this.btnWashSolutionBuffer.Enabled = enable;
                    // プレトリガ、トリガ、希釈液
                    foreach (BlinkButton btn in this.pretriggerTriggerDiluentButtons)
                    {
                        btn.Enabled = enable;
                    }
                    // 分注チップ、反応容器
                    foreach (BlinkButton btn in this.samplingTipCellButtons)
                    {
                        btn.Enabled = enable;
                    }
                }

                // ボタンの活性状態を記憶する

                // 全選択/解除ボタン
                this.setReagentStateManager[(ModuleIndex)targetModuleIndex].AllSwitch[this.btnPretriggerTriggerDiluentSelectAllSwitch].Enabled = enable;
                this.setReagentStateManager[(ModuleIndex)targetModuleIndex].AllSwitch[this.btnSamplingTipSelectAllSwitch].Enabled = enable;

                // 試薬ボトル
                this.setReagentStateManager[(ModuleIndex)targetModuleIndex].Blink[this.btnReagentBottle].Enabled = enable;

                // 廃液バッファ
                this.setReagentStateManager[(ModuleIndex)targetModuleIndex].Blink[this.btnWasteBuffer].Enabled = enable;
                
                // 洗浄液バッファ
                this.setReagentStateManager[(ModuleIndex)targetModuleIndex].Blink[this.btnWashSolutionBuffer].Enabled = enable;
                
                // プレトリガ、トリガ、希釈液
                foreach (BlinkButton btn in this.pretriggerTriggerDiluentButtons)
                {
                    this.setReagentStateManager[(ModuleIndex)targetModuleIndex].Blink[btn].Enabled = enable;
                }
                
                // サンプリングチップセル
                foreach (BlinkButton btn in this.samplingTipCellButtons)
                {
                    this.setReagentStateManager[(ModuleIndex)targetModuleIndex].Blink[btn].Enabled = enable;
                }
            }
            else
            {
                // モジュールIndexの指定がない場合、ボタン状態を記憶しない

                // ONに戻す場合は前回状態を反映する
                if (enable== true)
                {
                    // 現在選択中のモジュールのボタン状態を取得
                    SetReagentStateInfo currentModuleButtonState = this.setReagentStateManager[Singleton<PublicMemory>.Instance.moduleIndex];

                    //全選択/解除ボタン
                    this.btnPretriggerTriggerDiluentSelectAllSwitch.Enabled = currentModuleButtonState.AllSwitch[this.btnPretriggerTriggerDiluentSelectAllSwitch].Enabled;
                    this.btnSamplingTipSelectAllSwitch.Enabled = currentModuleButtonState.AllSwitch[this.btnSamplingTipSelectAllSwitch].Enabled;

                    // 試薬ボトル
                    this.btnReagentBottle.Enabled = currentModuleButtonState.Blink[this.btnReagentBottle].Enabled;
                    
                    // 廃液バッファ
                    this.btnWasteBuffer.Enabled = currentModuleButtonState.Blink[this.btnWasteBuffer].Enabled;
                    
                    // 洗浄液バッファ
                    this.btnWashSolutionBuffer.Enabled = currentModuleButtonState.Blink[this.btnWashSolutionBuffer].Enabled;
                    
                    // プレトリガ、トリガ、希釈液
                    foreach (BlinkButton btn in this.pretriggerTriggerDiluentButtons)
                    {
                        btn.Enabled = currentModuleButtonState.Blink[btn].Enabled;
                    }
                    
                    // サンプリングチップセル
                    foreach (BlinkButton btn in this.samplingTipCellButtons)
                    {
                        btn.Enabled = currentModuleButtonState.Blink[btn].Enabled;
                    }
                }
                // OFFの場合はOFF
                else
                {
                    //全選択/解除ボタン
                    this.btnPretriggerTriggerDiluentSelectAllSwitch.Enabled = enable;
                    this.btnSamplingTipSelectAllSwitch.Enabled = enable;

                    // 試薬ボトル
                    this.btnReagentBottle.Enabled = enable;
                    
                    // 廃液バッファ
                    this.btnWasteBuffer.Enabled = enable;
                   
                    // 洗浄液バッファ
                    this.btnWashSolutionBuffer.Enabled = enable;
                    
                    // プレトリガ、トリガ、希釈液
                    foreach (BlinkButton btn in this.pretriggerTriggerDiluentButtons)
                    {
                        btn.Enabled = enable;
                    }

                    // サンプリングチップセル
                    foreach (BlinkButton btn in this.samplingTipCellButtons)
                    {
                        btn.Enabled = enable;
                    }
                }
            }
        }

        /// <summary>
        /// ボタンブリンク開始・停止（リスト一括）
        /// </summary>
        /// <remarks>
        ///  ボタンブリンク開始・停止（リスト一括）処理を実行します
        /// </remarks>
        /// <param name="isBlinkStart">ブリンク開始・停止種別</param>
        /// <param name="buttons">ボタンリスト</param>
        private void setButtonBlink(Boolean isBlinkStart, List<BlinkButton> buttons, ModuleIndex targetModuleIndex)
        {
            foreach (BlinkButton btn in buttons)
            {
                this.setButtonBlink(isBlinkStart, btn, targetModuleIndex);
            }
        }

        /// <summary>
        /// ボタンブリンク開始・停止
        /// </summary>
        /// <remarks>
        /// ボタンブリンク開始・停止処理を実行します
        /// </remarks>
        /// <param name="isBlinkStart">ブリンク開始・停止種別</param>
        /// <param name="buttons">ボタンリスト</param>
        private void setButtonBlink(Boolean isBlinkStart, BlinkButton btn, ModuleIndex targetModuleIndex)
        {
            // ボタンのON状態を確認
            if (this.setReagentStateManager[targetModuleIndex].Blink[btn].CurrentState == true)
            {
                // 点滅状態設定
                this.setReagentStateManager[targetModuleIndex].Blink[btn].IsBlink = isBlinkStart;
            }
            else
            {
                // 点滅状態設定
                this.setReagentStateManager[targetModuleIndex].Blink[btn].IsBlink = false;
            }

            // 画面反映有無
            if (targetModuleIndex == Singleton<PublicMemory>.Instance.moduleIndex)
            {
                if (isBlinkStart)
                {
                    // 点滅開始
                    btn.BlinkStart(btn.NormalAppearance.ImageBackground
                                    , btn.ToggleAppearance.ImageBackground
                                    , BTN_BLINK_INTERVAL
                                    , 1);
                }
                else
                {
                    // 点滅停止
                    btn.BlinkEnd();
                }
            }
            else
            {
                // 対象モジュールと異なる場合、何もしない
            }
        }

        /// <summary>
        /// ボタンの選択状態の一括変更(全選択の切り替え)
        /// </summary>
        /// <remarks>
        /// ボタンの選択状態の一括変更(全選択の切り替え)処理を実行します
        /// </remarks>
        /// <param name="buttons">選択状態を一括変更するボタンリスト</param>
        private void setButtonToggle(List<BlinkButton> buttons)
        {
            Boolean allSelect = false;

            List<BlinkButton> targetButtonList = new List<BlinkButton>();
            Dictionary<Infragistics.Win.UltraWinEditors.UltraPictureBox, String> pbxDic = new Dictionary<Infragistics.Win.UltraWinEditors.UltraPictureBox, string>();

            // 分析中の場合
            if (Singleton<SystemStatus>.Instance.Status == SystemStatusKind.Assay)
            {
                foreach (BlinkButton btn in buttons)
                {
                    Infragistics.Win.UltraWinEditors.UltraPictureBox pbx = new Infragistics.Win.UltraWinEditors.UltraPictureBox();

                    // キーとして存在するかの確認
                    if (buttonPbxInfo.ContainsKey(btn))
                    {
                        pbxDic = buttonPbxInfo[btn];
                        // ディクショナリー内の最初のkeyを取得する
                        pbx = pbxDic.FirstOrDefault().Key;
                    }
                    else
                    {
                        // ターゲットボタンリストに追加
                        targetButtonList.Add(btn);
                    }

                    // 使用中じゃない場合
                    if (pbx.Visible == false)
                    {
                        // ターゲットボタンリストに追加
                        targetButtonList.Add(btn);
                    }


                }
            }
            else
            {
                targetButtonList = buttons;
            }

            // ボタンリストに未選択のボタンがあるかどうか
            foreach (BlinkButton btn in targetButtonList)
            {
                if (!btn.CurrentState)
                {
                    allSelect = true;
                    break;
                }
            }

            // 選択状態の切り替え
            if (allSelect)
            {
                // ひとつ以上未選択のボタンがある場合
                // 全てを選択状態にする
                foreach (BlinkButton btn in targetButtonList)
                {
                    btn.CurrentState = true;
                }
            }
            else
            {
                // 全て選択状態の場合
                // 全てを非選択状態にする
                foreach (BlinkButton btn in targetButtonList)
                {
                    btn.CurrentState = false;
                }
            }
        }

        /// <summary>
        /// プレトリガ、トリガ、希釈液一括選択切り替えボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ボタンの選択状態の一括変更(全選択の切り替え)処理を実行します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnPretriggerTriggerDiluentSelectAllSwitch_Click(object sender, EventArgs e)
        {
            this.setButtonToggle(this.pretriggerTriggerDiluentButtons);
        }

        /// <summary>
        /// 分注チップ、反応容器一括選択切り替えボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ボタンの選択状態の一括変更(全選択の切り替え)処理を実行します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnSamplingTipSelectAllSwitch_Click(object sender, EventArgs e)
        {
            this.setButtonToggle(this.samplingTipCellButtons);
        }

        /// <summary>
        /// ボタンの選択数を取得
        /// </summary>
        /// <remarks>
        /// ボタンの選択数を取得します
        /// </remarks>
        private Int32 getSelectBtnCount()
        {
            Int32 nSelectCount = 0;
            // 試薬ボトル
            if (this.btnReagentBottle.CurrentState)
            {
                nSelectCount++;
            }
            // 廃液バッファ
            if (this.btnWasteBuffer.CurrentState)
            {
                nSelectCount++;
            }
            // 洗浄液バッファ
            if (this.btnWashSolutionBuffer.CurrentState)
            {
                nSelectCount++;
            }
            // プレトリガ、トリガ、希釈液
            foreach (BlinkButton btn in this.pretriggerTriggerDiluentButtons)
            {
                if (btn.CurrentState)
                {
                    nSelectCount++;
                }
            }
            // 分注チップ
            foreach (BlinkButton btn in this.samplingTipCellButtons)
            {
                if (btn.CurrentState)
                {
                    nSelectCount++;
                }
            }
            return nSelectCount;
        }

        /// <summary>
        /// ボタンのステータス（ボタンイメージ）を更新
        /// </summary>
        /// <remarks>
        /// ボタンのステータス（ボタンイメージ）を更新します
        /// </remarks>
        private void changeRemainStatus()
        {
            //
            // 閾値などからステータス更新する
            //

            // DBから残量情報取得
            Int32 remain = 0;
            object objImg;
            Singleton<ReagentDB>.Instance.LoadDB();

            // 試薬ボトルは無し

            // 廃液バッファ
            remain = this.getRemain(ReagentKind.WasteBuffer, 1);
            objImg = this.getButtonImageFromState(ReagentKind.WasteBuffer, remain);
            this.btnWasteBuffer.Appearance.Image = objImg;
            this.btnWasteBuffer.NormalAppearance.Image = objImg;
            this.btnWasteBuffer.ToggleAppearance.Image = objImg;

            // 洗浄液バッファ
            remain = this.getRemain(ReagentKind.WashSolutionBuffer, 1);
            objImg = this.getButtonImageFromState(ReagentKind.WashSolutionBuffer, remain);
            this.btnWashSolutionBuffer.Appearance.Image = objImg;
            this.btnWashSolutionBuffer.NormalAppearance.Image = objImg;
            this.btnWashSolutionBuffer.ToggleAppearance.Image = objImg;

            // プレトリガ
            remain = this.getRemain(ReagentKind.Pretrigger, 1);
            objImg = this.getButtonImageFromState(ReagentKind.Pretrigger, remain);
            this.btnPretrigger1.Appearance.Image = objImg;
            this.btnPretrigger1.NormalAppearance.Image = objImg;
            this.btnPretrigger1.ToggleAppearance.Image = objImg;
            remain = this.getRemain(ReagentKind.Pretrigger, 2);
            objImg = this.getButtonImageFromState(ReagentKind.Pretrigger, remain);
            this.btnPretrigger2.Appearance.Image = objImg;
            this.btnPretrigger2.NormalAppearance.Image = objImg;
            this.btnPretrigger2.ToggleAppearance.Image = objImg;

            // トリガ
            remain = this.getRemain(ReagentKind.Trigger, 1);
            objImg = this.getButtonImageFromState(ReagentKind.Trigger, remain);
            this.btnTrigger1.Appearance.Image = objImg;
            this.btnTrigger1.NormalAppearance.Image = objImg;
            this.btnTrigger1.ToggleAppearance.Image = objImg;
            remain = this.getRemain(ReagentKind.Trigger, 2);
            objImg = this.getButtonImageFromState(ReagentKind.Trigger, remain);
            this.btnTrigger2.Appearance.Image = objImg;
            this.btnTrigger2.NormalAppearance.Image = objImg;
            this.btnTrigger2.ToggleAppearance.Image = objImg;

            // 希釈液
            if (!this.btnDiluent.IsBlink)
            {
                remain = this.getRemain(ReagentKind.Diluent, 1);
                objImg = this.getButtonImageFromState(ReagentKind.Diluent, remain);
                this.btnDiluent.Appearance.Image = objImg;
                this.btnDiluent.NormalAppearance.Image = objImg;
                this.btnDiluent.ToggleAppearance.Image = objImg;
            }

            // 分注チップ、反応容器
            Int32 portNo = 0;
            foreach (BlinkButton btn in this.samplingTipCellButtons)
            {
                portNo++;
                remain = this.getRemain(ReagentKind.SamplingTip, portNo);
                objImg = this.getButtonImageFromState(ReagentKind.SamplingTip, remain);
                btn.Appearance.Image = objImg;
                btn.NormalAppearance.Image = objImg;
                btn.ToggleAppearance.Image = objImg;
            }

        }

        /// <summary>
        /// ボタンのイメージを取得
        /// </summary>
        /// <remarks>
        /// ボタンのイメージを取得します
        /// </remarks>
        /// <param name="reagentKind">試薬種別</param>
        /// <param name="remain">残量</param>
        private object getButtonImageFromState(ReagentKind reagentKind, Int32 remain)
        {
            object objRet = null;
            try
            {
                // プレトリガボトル、トリガボトル、希釈液ボトル
                if ( (reagentKind == ReagentKind.Pretrigger)
                  || (reagentKind == ReagentKind.Trigger)
                  || (reagentKind == ReagentKind.Diluent))
                {
                    // 表示残量取得
                    Int32 dispRemainCount = CarisXSubFunction.GetDispRemainCount(reagentKind, remain);

                    // 閾値情報(残量降順)
                    RemainStatus bottleStatus = Singleton<ReagentRemainStatusInfo>.Instance.GetRemainStatus(reagentKind, dispRemainCount);
                    switch (bottleStatus)
                    {
                        case RemainStatus.Empty:    // 白：なし
                            objRet = Oelco.CarisX.Properties.Resources.Image_BottleWhiteLarge;
                            break;
                        case RemainStatus.Low:      // 黄：残りわずか
                            objRet = Oelco.CarisX.Properties.Resources.Image_BottleYellowLarge;
                            break;
                        case RemainStatus.Full:     // 緑：十分にある
                            objRet = Oelco.CarisX.Properties.Resources.Image_BottleGreenLarge;
                            break;
                    }

                }
                // サンプリングチップセル
                else if (reagentKind == ReagentKind.SamplingTip)
                {
                    // 閾値情報(残量降順)
                    RemainStatus tipStatus = Singleton<ReagentRemainStatusInfo>.Instance.GetRemainStatus(reagentKind, remain);

                    switch (tipStatus)
                    {
                        case RemainStatus.Empty:    // 白：なし
                            objRet = Oelco.CarisX.Properties.Resources.Image_TipCaseWhite;
                            break;
                        case RemainStatus.Low:      // 黄：残りわずか
                            objRet = Oelco.CarisX.Properties.Resources.Image_TipCaseYellow;
                            break;
                        case RemainStatus.Full:     // 緑：十分にある
                            objRet = Oelco.CarisX.Properties.Resources.Image_TipCaseGreen;
                            break;
                    }

                }
                //廃液バッファ
                else if (reagentKind == ReagentKind.WasteBuffer)
                {
                    // 閾値と比較してステータスを取得
                    WasteStatus wasteBufferStatus = Singleton<ReagentRemainStatusInfo>.Instance.GetWasteStatus(reagentKind, remain, false);
                    switch (wasteBufferStatus)
                    {
                        case WasteStatus.None:      // 白：タンクなし
                            //CarisXでは発生しない
                            objRet = Oelco.CarisX.Properties.Resources.Image_WasteTank_WhiteLarge;
                            break;
                        case WasteStatus.NotFull:   // 緑：廃液注入可
                            objRet = Oelco.CarisX.Properties.Resources.Image_WasteTank_GreenLarge;
                            break;
                        case WasteStatus.Full:      // 赤：満杯
                            objRet = Oelco.CarisX.Properties.Resources.Image_WasteTank_RedLarge;
                            break;
                    }
                }

                //洗浄液バッファ
                else if (reagentKind == ReagentKind.WashSolutionBuffer)
                {
                    // 閾値と比較してステータスを取得
                    RemainStatus washSolutionStatus = Singleton<ReagentRemainStatusInfo>.Instance.GetRemainStatus(reagentKind, remain);
                    switch (washSolutionStatus)
                    {
                        case RemainStatus.Empty:
                            objRet = Oelco.CarisX.Properties.Resources.Image_WashSolution_WhiteLarge;
                            break;
                        case RemainStatus.Full:
                            objRet = Oelco.CarisX.Properties.Resources.Image_WashSolution_GreenLarge;
                            break;
                        case RemainStatus.Middle:
                            objRet = Oelco.CarisX.Properties.Resources.Image_WashSolution_YellowLarge;
                            break;
                        case RemainStatus.Low:
                            objRet = Oelco.CarisX.Properties.Resources.Image_WashSolution_RedLarge;
                            break;
                    }
                }
            }
            catch (Exception)
            {
                //残量の閾値設定がありません
                System.Diagnostics.Debug.WriteLine(String.Format("There is no threshold setting of the remaining amount:{0}", Enum.GetName(typeof(ReagentKind), reagentKind)));
                objRet = null;
            }

            return objRet;
        }

        /// <summary>
        /// ボタンの選択状態が変更された時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void blinkButton_StateChanged(object sender, CustomUStateButton.ChangeStateEventArgs e)
        {
            // 点滅ボタンか判定
            var targetButton = sender as BlinkButton;
            if (targetButton == null)
            {
                // 点滅ボタンではないため、処理なし
                return;
            }

            // ボタンとラベルの対応リストが生成されている場合
            if ((this.buttonLabelInfo != null) && (this.buttonLabelInfo.ContainsKey(targetButton) == true))
            {
                Dictionary<Infragistics.Win.UltraWinEditors.UltraPictureBox, String> pbxDic = new Dictionary<Infragistics.Win.UltraWinEditors.UltraPictureBox, string>();

                foreach (UltraLabel lbl in this.buttonLabelInfo[targetButton])
                {
                    if (e.AfterState)
                    {
                        // 分析中の場合
                        if (Singleton<SystemStatus>.Instance.Status == SystemStatusKind.Assay)
                        {
                            pbxDic = buttonPbxInfo[targetButton];

                            // ディクショナリー内の最初のkeyを取得する
                            Infragistics.Win.UltraWinEditors.UltraPictureBox pbx = pbxDic.FirstOrDefault().Key;

                            // 使用中の場合
                            if (pbx.Visible == true)
                            {
                                // ステータスを非選択状態に変更
                                e.AfterState = false;

                                // ピクチャーボックスに対応する文字を取得
                                String name = pbxDic[pbx];
                                String str = String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MSG_266, name);

                                // 警告ダイアログの表示
                                DlgMessage.Show(str, "", CarisX.Properties.Resources.STRING_DLG_TITLE_005, MessageDialogButtons.OK);
                                break;
                            }
                        }

                        //選択状態
                        lbl.Appearance.ForeColor = Color.White;
                        lbl.Appearance.BackColor = Color.Transparent;
                    }
                    else
                    {
                        //非選択状態
                        lbl.Appearance.ForeColor = Color.Black;
                        lbl.Appearance.BackColor = Color.Transparent;
                    }
                }
            }

            // 選択されているモジュールIndex取得
            ModuleIndex selectModuleIndex = Singleton<PublicMemory>.Instance.moduleIndex;

            // 選択されているモジュールIndexがボタンステータスリストに存在しているかチェック
            if (this.setReagentStateManager.ContainsKey(selectModuleIndex) == true)
            {
                // イベント通知のボタンがボタンステータスリストに存在しているかチェック
                if (this.setReagentStateManager[selectModuleIndex].Blink.ContainsKey(targetButton) == true)
                {
                    // モジュール毎のボタンステータスを変更
                    this.setReagentStateManager[selectModuleIndex].Blink[targetButton].CurrentState = e.AfterState;
                }
            }
        }

        /// <summary>
        /// ラベルがクリックされた時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_Click(object sender, EventArgs e)
        {
            UltraLabel lbl = (UltraLabel)sender;

            //クリックされたラベルをValueにもつbuttonLabelInfoのキー（＝ボタン）を取得する
            foreach (BlinkButton btn in this.buttonLabelInfo.Where(a => a.Value.Exists(x => x == lbl)).Select(b => b.Key))
            {
                btn.PerformClick();
            }
        }

        /// <summary>
        /// 残量によるアクト状況の変更
        /// </summary>
        private void changeActStatus()
        {
            // モジュールIDの取得
            Int32 moduleId = CarisXSubFunction.ModuleIndexToModuleId(Singleton<PublicMemory>.Instance.moduleIndex);

            // プレトリガのアクト状況の変更
            pbxPreTrigger1IsUse.Visible = this.isRemainUse(moduleId, ReagentKind.Pretrigger, 1);
            pbxPreTrigger2IsUse.Visible = this.isRemainUse(moduleId, ReagentKind.Pretrigger, 2);

            // トリガ液のアクト状況の変更
            pbxTrigger1IsUse.Visible = this.isRemainUse(moduleId, ReagentKind.Trigger, 1);
            pbxTrigger2IsUse.Visible = this.isRemainUse(moduleId, ReagentKind.Trigger, 2);

            // 使用中のチップセルケースのポート番号取得
            Int32 useTipPos = this.isUseTipPos(moduleId);

            // すべてのチップセルケースの使用を無しにする
            foreach (var tipCell in pbxTipCellList)
            {
                tipCell.Visible = false;
            }

            // 使用中のチップセルケースがあるか確認
            if (useTipPos != 0)
            {
                // チップセルケースのアクト状況を「使用中」にする
                pbxTipCellList[useTipPos - 1].Visible = true;
            }

        }

        /// <summary>
        /// ボトルの使用状況の取得
        /// </summary>
        /// <param name="moduleId">モジュール番号</param>
        /// <param name="reagentKind">ボトル種別</param>
        /// <param name="PortNo">ポート番号</param>
        /// <returns>ボトル使用状況</returns>
        private Boolean isRemainUse(Int32 moduleId, ReagentKind reagentKind, Int32 PortNo)
        {
            Boolean isUse = false;

            // ボトルの使用状況取得
            ReagentData data = Singleton<ReagentDB>.Instance.GetData(moduleId: moduleId)
               .FirstOrDefault((reagentDataItem) => reagentDataItem.ReagentKind == (Int32)reagentKind && (reagentDataItem.PortNo == PortNo));

            // 情報が取得できた場合
            if (data != null)
            {
                isUse = data.IsUse ?? false;
            }

            return isUse;
        }

        /// <summary>
        /// 使用チップケースアクト番号の取得
        /// </summary>
        /// <param name="moduleId">モジュール番号</param>
        /// <returns>使用チップケース番号</returns>
        private int isUseTipPos(Int32 moduleId)
        {
            int useTipPos = 0;

            // 使用チップケースアクト番号を取得
            var temp = Singleton<ReagentDB>.Instance.GetData(moduleId: moduleId)
                .Where((reagentDataItem) => reagentDataItem.ReagentKind == (Int32)ReagentKind.SamplingTip
                && (bool)reagentDataItem.IsUse).Select(data => (Int32)data.PortNo);

            // リストが存在しかつ、使用チップケースが一つだけの場合
            if (temp != null && temp.Count() > 0)
            {
                // 使用チップケースのアクト番号を設定
                useTipPos = temp.FirstOrDefault();
            }

            return useTipPos;
        }

        /// <summary>
        /// 試薬ボタン状態を反映
        /// </summary>
        /// <param name="targetModuleIndex"></param>
        private void reflectReagentBtnStatus(ModuleIndex targetModuleIndex)
        {
            // モジュールIDを取得
            int moduleId = CarisXSubFunction.ModuleIndexToModuleId(targetModuleIndex);

            // 未接続またはモーターエラー時
            SystemStatusKind targetModuleStatus = Singleton<SystemStatus>.Instance.ModuleStatus[moduleId];

            // ボタン操作可否を確認
            if ( (this.isReagentChangeRefused == true)
              || ( (targetModuleStatus == SystemStatusKind.NoLink)
                || (targetModuleStatus == SystemStatusKind.MotorError) ) )
            {
                // 交換禁止状態になっている
                // または、モジュールステータスが未接続またはモーターエラー時 => 操作不可

                // ボタンの状態を初期状態に戻す
                foreach (var blinkBtn in this.setReagentStateManager[targetModuleIndex].Blink)
                {
                    // ボタン点滅状態をOFFにする
                    this.setButtonBlink(false, blinkBtn.Key, targetModuleIndex);

                    // ボタンをOFF状態に戻す
                    blinkBtn.Key.CurrentState = false;
                }

                // 交換開始、交換完了、交換キャンセル、編集コマンドバーを操作不可にする
                this.tlbCommandBar.Tools[EXCHANGE_START].SharedProps.Enabled = false;
                this.tlbCommandBar.Tools[EXCHANGE_COMPLETE].SharedProps.Enabled = false;
                this.tlbCommandBar.Tools[EXCHANGE_CANCEL].SharedProps.Enabled = false;
                this.tlbCommandBar.Tools[EDIT].SharedProps.Enabled = false;
                this.tlbCommandBar.Tools[REPLACE_TANK].SharedProps.Enabled = false;

                // 各ボタンは操作不可にする
                this.setButtonEnable(false);
            }
            else
            {
                // 前回状態を反映

                // ボタンの状態を設定する
                foreach (var blinkBtn in this.setReagentStateManager[targetModuleIndex].Blink)
                {
                    // ボタン点滅状態を設定する
                    this.setButtonBlink(blinkBtn.Value.IsBlink, blinkBtn.Key, targetModuleIndex);

                    // ボタン押下状態を設定する
                    blinkBtn.Key.CurrentState = blinkBtn.Value.CurrentState;
                }

                // 交換開始、交換完了、交換キャンセル、編集コマンドバーを操作可にする
                this.tlbCommandBar.Tools[EXCHANGE_START].SharedProps.Enabled = this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_START].Enabled;
                this.tlbCommandBar.Tools[EXCHANGE_COMPLETE].SharedProps.Enabled = this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_COMPLETE].Enabled;
                this.tlbCommandBar.Tools[EXCHANGE_CANCEL].SharedProps.Enabled = this.setReagentStateManager[targetModuleIndex].ToolBar[EXCHANGE_CANCEL].Enabled;
                this.tlbCommandBar.Tools[EDIT].SharedProps.Enabled = this.setReagentStateManager[targetModuleIndex].ToolBar[EDIT].Enabled;
                this.tlbCommandBar.Tools[REPLACE_TANK].SharedProps.Enabled = this.setReagentStateManager[targetModuleIndex].ToolBar[REPLACE_TANK].Enabled;

                // 各ボタンは操作可にする
                this.setButtonEnable(true);
            }
        }

        #endregion
    }
}
