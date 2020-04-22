using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using Oelco.Common.Utility;
using Oelco.CarisX.DB;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Const;
using Oelco.CarisX.Comm;
using Oelco.Common.Parameter;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Common;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 試薬(交換)テーブルダイアログクラス
    /// </summary>
    public partial class DlgTurnTable : DlgCarisXBase
    {
        #region [定数定義]

        /// <summary>
        /// 試薬テーブル交換可能範囲
        /// </summary>
        public const Int32 VIEW_RANGE = 5;

        /// <summary>
        /// 試薬設置ポート番号列キー
        /// </summary>
        private const String STRING_PORTNO = "PortNo";
        /// <summary>
        /// 分析項目名列キー
        /// </summary>
        private const String STRING_ANALYTES = "Analytes";
        /// <summary>
        /// 試薬ロット番号列キー
        /// </summary>
        private const String STRING_LOTNO = "LotNo";
        /// <summary>
        /// 有効期限列キー
        /// </summary>
        private const String STRING_EXPIRATIONDATE = "ExpirationDate";
        /// <summary>
        /// 使用期限列キー
        /// </summary>
        private const String STRING_STABILITYDATE = "StabilityDate";
        /// <summary>
        /// 残量列キー
        /// </summary>
        private const String STRING_REMAIN = "Remain";
        /// <summary>
        /// 重複チェック列キー
        /// </summary>
        private const String STRING_CHECK = "Check";

        #endregion

        #region [クラス変数定義]

        /// <summary>
        /// カレント交換対象位置(0:未移動)
        /// </summary>
        private static Int32 currentTurnPosition = 0;

        // 試薬保冷庫テーブルSW移動許可コマンド送信
        SlaveCommCommand_0494 cmd0494 = new SlaveCommCommand_0494();

        // 最終判定
        Boolean isLast;

        /// <summary>
        /// 表示モード
        /// </summary>
        public enum TurnTableDispMode
        {
            /// <summary>
            /// 試薬確認モード
            /// </summary>
            Check = 0,
            /// <summary>
            /// 試薬交換モード
            /// </summary>
            Change = 1,
            /// <summary>
            /// 試薬編集モード
            /// </summary>
            Edit = 2
        }

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 選択中のインデックス
        /// </summary>
        private List<Int32> selectedIndex = new List<Int32>();

        /// <summary>
        /// 表示モード
        /// </summary>
        private TurnTableDispMode dispMode = TurnTableDispMode.Check;

        /// <summary>
        /// 試薬交換初回フラグ
        /// </summary>
        private Boolean isChangeFirst = false;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgTurnTable()
        {
            InitializeComponent();
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 表示モードの取得、設定
        /// </summary>
        public TurnTableDispMode DispMode
        {
            get
            {
                return this.dispMode;
            }
            set
            {
                // 全選択ボタン、キャンセルボタンの表示状態切り替え
                this.dispMode = value;
                switch (this.dispMode)
                {
                    case TurnTableDispMode.Change:
                        // 試薬交換モード
                        this.btnTurn.Visible = true;
                        this.btnSelectAll.Visible = true;
                        this.btnCancel.Visible = true;
                        this.btnOK.Visible = true;
                        break;
                    case TurnTableDispMode.Edit:
                        // 試薬編集モード
                        this.btnTurn.Visible = false;
                        this.btnSelectAll.Visible = false;
                        this.btnCancel.Visible = true;
                        this.btnOK.Visible = true;
                        break;
                    case TurnTableDispMode.Check:
                        // 試薬確認モード
                        this.btnTurn.Visible = false;
                        this.btnSelectAll.Visible = false;
                        this.btnCancel.Visible = false;
                        this.btnOK.Visible = true;
                        break;
                }
            }
        }

        /// <summary>
        /// カレント交換対象位置(0:未移動)の取得、設定
        /// </summary>
        static public Int32 CurrentTurnPosition
        {
            get
            {
                return currentTurnPosition;
            }
            set
            {
                currentTurnPosition = value;
            }
        }

        /// <summary>
        /// 試薬交換初回フラグの取得、設定
        /// </summary>
        public Boolean IsChangeFirst
        {
            get
            {
                return isChangeFirst;
            }
            set
            {
                isChangeFirst = value;
            }
        }

        public Boolean CanTurn
        {
            get
            {
                return this.btnTurn.Enabled;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// Turnボタンクリック、及び試薬テーブル回転SW押下通知コマンド受信時の処理
        /// </summary>
        /// <remarks>
        /// 選択行をカレントの交換指定対象に順次切り替えします
        /// </remarks>
        public void TurnTable(Int32 moduleIndex)
        {
            // 試薬交換モードの場合、初回にTurnボタンをクリックした際、試薬準備開始コマンドを送信する
            if ((this.dispMode == TurnTableDispMode.Change) && (this.isChangeFirst == true))
            {
                this.isChangeFirst = false;// フラグ戻す

                this.btnOK.Enabled = true;// 初回にTurnボタンをクリックした時、OKボタンを有効にする

                SlaveCommCommand_0416 cmd0416 = new SlaveCommCommand_0416();
                // 試薬準備テーブル設定。選択されている設置ポート番号(1～50)を設定する
                // 全選択状態取得
                Int32 index;

                foreach (UltraGridRow row in this.grdReagentList.Rows)
                {
                    UltraGridCell cell = row.Cells[STRING_PORTNO];
                    index = Int32.Parse(cell.Text);
                    index--;// 0オリジンにする
                    if (cell.Appearance.BackColor == CarisXConst.BUTTON_SELECT_COLOR)
                    {
                        cmd0416.PrepareFlag[index] = 1;
                    }
                    else
                    {
                        cmd0416.PrepareFlag[index] = 0;
                    }
                }

                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(moduleIndex, cmd0416);
            }

            // 試薬交換モードか判定
            else if (this.dispMode != TurnTableDispMode.Change)
            {
                // 回転コマンド
                SlaveCommCommand_0487 turnCommandEdit = new SlaveCommCommand_0487();
                // 試薬ボトル交換ダイアログが表示されてないので0(1/10回転)で回転させる
                turnCommandEdit.PortNumber = 0;
                // コマンド送信
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(moduleIndex, turnCommandEdit);
                return;
            }

            // 選択行をカレントの交換指定対象に順次切り替え
            Int32 oldcurrent = DlgTurnTable.currentTurnPosition;
            Action retrunCurrent = null;

            foreach (UltraGridRow row in this.grdReagentList.Rows)
            {
                // UNDONE:列の指定方法
                UltraGridCell selectCell = row.Cells[STRING_PORTNO];

                // 選択の切り替え
                if (selectCell.Appearance.BackColor == CarisXConst.BUTTON_SELECT_COLOR)
                {
                    if (oldcurrent == (Int32)selectCell.Value)
                    {
                        // ボタンの選択状態を解除
                        selectCell.Appearance.BackColor = Color.Empty;
                        selectCell.ButtonAppearance.BackColor = Color.Empty;

                        // 通常表示
                        this.rowChangeCurrent(row, false);
                    }
                    else if (DlgTurnTable.currentTurnPosition < (Int32)selectCell.Value)
                    {
                        // カレントを更新
                        DlgTurnTable.currentTurnPosition = (Int32)selectCell.Value;

                        // カレント表示
                        this.rowChangeCurrent(row, true);
                        break;

                    }
                    else if (oldcurrent > (Int32)selectCell.Value && retrunCurrent == null)
                    {
                        UltraGridRow nextRow = row;

                        retrunCurrent += () =>
                        {
                            // 交換対象の位置をカレントに指定 
                            DlgTurnTable.currentTurnPosition = (Int32)nextRow.Cells[STRING_PORTNO].Value;

                            // 交換対象がカレント表示
                            this.rowChangeCurrent(nextRow, true);
                        };
                    }
                }
            }

            if (DlgTurnTable.currentTurnPosition == oldcurrent)
            {
                // カレント位置以降に交換対象候補がない場合
                if (retrunCurrent != null)
                {
                    // カレント位置以降の最上位の候補がある場合はその候補へ
                    retrunCurrent();
                }
                else
                {
                    // 全て交換対象候補がない場合は、カレント
                    DlgTurnTable.currentTurnPosition = 0;
                }
            }

            // 回転コマンド
            SlaveCommCommand_0487 turnCommand = new SlaveCommCommand_0487();
            // ポート番号指定、又は0設定
            turnCommand.PortNumber = DlgTurnTable.currentTurnPosition;
            // コマンド送信後、送信したポート番号以外選択されているポート番号が無くなったらTurnボタンをDisableにする

            isLast = true;
            foreach (UltraGridRow row in this.grdReagentList.Rows)
            {
                // UNDONE:列の指定方法
                UltraGridCell selectCell = row.Cells[STRING_PORTNO];
                if (selectCell.Appearance.BackColor == CarisXConst.BUTTON_SELECT_COLOR)
                {
                    if (DlgTurnTable.currentTurnPosition != (Int32)selectCell.Value)
                    {
                        isLast = false;
                    }
                }
            }
            if (isLast == true)
            {
                this.btnTurn.Enabled = false;
            }
            cmd0494.SwParam = Convert.ToByte(false);
            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(moduleIndex, cmd0494);

            this.btnOK.Enabled = false;
            this.btnTurn.Enabled = false;
            this.btnCancel.Enabled = false;
            Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleIndex]
                .ReagentCoolerMoveSequence(this, (Int32)this.dispMode, turnCommand.PortNumber);
        }

        /// <summary>
        /// 試薬保冷庫レスポンス受信
        /// </summary>
        /// <remarks>
        /// 試薬保冷庫テーブルレスポンス受信後処理
        /// </remarks>
        /// <returns>なし</returns>
        public void SetCmdResReagentCoolerComplete(DlgTurnTable param)
        {
            if (param.isLast == true)
            {
                //試薬保冷庫テーブルSW移動許可コマンド送信
                cmd0494.SwParam = Convert.ToByte(false);
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd0494);
            }
            else
            {
                //試薬保冷庫テーブルSW移動許可コマンド送信
                cmd0494.SwParam = Convert.ToByte(true);
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd0494);
                param.btnTurn.Enabled = true;
            }
            param.btnOK.Enabled = true;
            param.btnCancel.Enabled = true;
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
            // グリッド表示順
            this.grdReagentList.SetGridColumnOrder(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.TurnTableSettings.ReagentList1GridColOrder);
            // グリッド列幅
            this.grdReagentList.SetGridColmnWidth(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.TurnTableSettings.ReagentList1GridColWidth);
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_TURNTABLE_000;

            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;
            this.btnOK.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnSelectAll.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_008;
            this.btnTurn.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_TURNTABLE_007;

            this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_PORTNO].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_TURNTABLE_001;
            this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_ANALYTES].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_TURNTABLE_002;
            this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_LOTNO].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_TURNTABLE_003;
            this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_EXPIRATIONDATE].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_TURNTABLE_004;
            this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_STABILITYDATE].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_TURNTABLE_005;
            this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_REMAIN].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_TURNTABLE_006;
            this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_CHECK].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_TURNTABLE_008;
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// セルデータエラーイベント
        /// </summary>
        /// <remarks>
        /// エラーイベント設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void grdReagentList_CellDataError(object sender, CellDataErrorEventArgs e)
        {
            e.RaiseErrorEvent = false;
        }

        /// <summary>
        /// グリッドセルクリックイベントハンドラ
        /// </summary>
        /// <param name="sender">対象グリッド</param>
        /// <param name="e">クリックセル情報</param>
        private void grdReagentList_ClickCell(object sender, ClickCellEventArgs e)
        {
            // 満杯容量の単位はμl
            // 編集モードかつ残量が空白となっていないセルについて処理する
            if ((this.dispMode == TurnTableDispMode.Edit) && (!String.IsNullOrEmpty(e.Cell.Text)))
            {
                // CellからRowを取得してポート番号列を参照する。
                // ポート番号計算ではゼロオリジンにて扱う
                Int32 portNo = (Int32)e.Cell.Row.Cells[STRING_PORTNO].Value - 1;
                CustomGrid grid = sender as CustomGrid;

                // ReagentDBからポート番号をキーにしてCapacity検索
                // PortNo  :r1試薬
                // PortNo+1:r2試薬
                // PortNo+3:M試薬
                var selectPortInfo = from v in Singleton<ReagentDB>.Instance.GetData( ReagentKind.Reagent
                                                                                    , CarisXSubFunction.ModuleIndexToModuleId(Singleton<PublicMemory>.Instance.moduleIndex))
                                     where (v.PortNo.HasValue)
                                        && (v.ReagentCode != 0)
                                        && (
                                            (((portNo * 3) + 1) == v.PortNo) ||
                                            (((portNo * 3) + 2) == v.PortNo) ||
                                            (((portNo * 3) + 3) == v.PortNo)
                                           )
                                     orderby v.PortNo
                                     select v;

                if (selectPortInfo.Count() == 3)
                {
                    // 分析項目名から分析項目取得する
                    String analyteName = e.Cell.Row.Cells[STRING_ANALYTES].Value.ToString();
                    MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromName(analyteName);

                    if (protocol != null)
                    {
                        Int32[] testCountAry = new Int32[]
                        {
                            ( selectPortInfo.ElementAt( 0 ).Capacity ),
                            ( selectPortInfo.ElementAt( 1 ).Capacity ),
                            ( selectPortInfo.ElementAt( 2 ).Capacity )
                        };

                        Int32 testCount = testCountAry.Min();

                        // 最大入力値適用
                        // 列単位に対しての設定となる（この設定により他の行の残量が最大値オーバーとなっても、入力状態にしない限り最大値には掛からない、セルクリックで該当行の最大値で更新される為問題ない）
                        grid.DisplayLayout.Bands[0].Columns[STRING_REMAIN].MinValue = 0;
                        grid.DisplayLayout.Bands[0].Columns[STRING_REMAIN].MaxValue = testCount;

                        System.Diagnostics.Debug.WriteLine(" 列 Port = {0},{1},{2} のデータを使用して、最大テスト数 = {3} を設定しました。", (portNo * 3) + 1, (portNo * 3) + 2, (portNo * 3) + 3, testCount);
                    }
                    else
                    {
                        Singleton<Oelco.CarisX.Log.CarisXLogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog,
                            String.Format("定義されていない分析項目名 {0} が画面に表示されています。", analyteName));

                        // 入力最大0設定
                        grid.DisplayLayout.Bands[0].Columns[STRING_REMAIN].MinValue = 0;
                        grid.DisplayLayout.Bands[0].Columns[STRING_REMAIN].MaxValue = 0;
                    }
                }
                else
                {
                    Singleton<Oelco.CarisX.Log.CarisXLogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog,
                        String.Format("grdReagentList_ClickCell PortNo = {0}, {1}, {2} が揃わなかった為最大入力値算出不能です。", (portNo * 3) + 1, (portNo * 3) + 2, (portNo * 3) + 3));

                    // 入力最大0設定
                    grid.DisplayLayout.Bands[0].Columns[STRING_REMAIN].MinValue = 0;
                    grid.DisplayLayout.Bands[0].Columns[STRING_REMAIN].MaxValue = 0;
                }
            }
        }

        /// <summary>
        /// フォーム読み込みイベント
        /// </summary>
        /// <remarks>
        /// 画面の表示状態を初期化して編集可否、表示色を設定します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DlgTurnTable_Load(object sender, EventArgs e)
        {
            // カレント交換対象位置初期化(0:未移動)
            DlgTurnTable.currentTurnPosition = 0;

            // ボタン有効無効設定
            if (this.dispMode == TurnTableDispMode.Change)
            {
                // 試薬交換の場合、初期表示時はTurnボタン、OKボタン無効
                btnTurn.Enabled = false;
                btnOK.Enabled = false;

                //試薬保冷庫テーブルSW移動許可コマンド送信
                cmd0494.SwParam = Convert.ToByte(false);
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd0494);
            }

            // 編集可否初期設定
            this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_PORTNO].CellActivation = Activation.NoEdit;
            this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_ANALYTES].CellActivation = Activation.NoEdit;
            if (this.dispMode == TurnTableDispMode.Edit)
            {
                this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_LOTNO].CellActivation = Activation.AllowEdit;
                this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_LOTNO].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_REMAIN].CellActivation = Activation.AllowEdit;
                this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_REMAIN].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Integer;
            }
            else
            {
                this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_LOTNO].CellActivation = Activation.NoEdit;
                this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_REMAIN].CellActivation = Activation.NoEdit;
            }
            this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_EXPIRATIONDATE].CellActivation = Activation.NoEdit;
            this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_STABILITYDATE].CellActivation = Activation.NoEdit;

            if ((this.dispMode == TurnTableDispMode.Change) || (this.dispMode == TurnTableDispMode.Edit))
            {
                // No.列の表示スタイルをボタンスタイルに設定
                this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_PORTNO].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;

                this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_PORTNO].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            }
            else
            {
                // No.列の表示スタイルを既定スタイルに設定
                this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_PORTNO].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Default;
            }

            ColorStatus colorStatus = new ColorStatus();
            Singleton<ReagentRemainStatusInfo>.Instance.Reagent.ForEach(item =>
            {
               colorStatus.AddColorRangePair(item.Remain, item.Status.ToColor());
            });

            // データ表示
            var singletonInstanceGetData = Singleton<ReagentDB>.Instance.GetData(moduleId: CarisXSubFunction.ModuleIndexToModuleId(Singleton<PublicMemory>.Instance.moduleIndex));
            Func<int, String> reagentCodeToProtocolName = (reagentCode) =>
            {
                MeasureProtocol measureProtocol = Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList.FirstOrDefault((protocol) => protocol.ReagentCode == reagentCode);
                if (measureProtocol != null)
                {
                    return measureProtocol.ProtocolName;
                }
                else
                {
                    return String.Empty;
                }
            };

            var reagentDatas = (from grp in
                                    (from data in singletonInstanceGetData
                                     where data.ReagentKind == (Int32)ReagentKind.Reagent
                                     orderby data.PortNo
                                     group data by (Int32)((data.PortNo - 1) / 3))
                                let analytes = grp.All((reagentdata) => reagentdata.ReagentCode == grp.First().ReagentCode) ? reagentCodeToProtocolName(grp.First().ReagentCode.Value) : null
                                let protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromName(analytes)
                                let MultiLotNo = !grp.All((regDat) => regDat.LotNo == grp.First().LotNo)
                                let MultiMakerCd = !grp.All((regDat) => regDat.MakerCode == grp.First().MakerCode)
                                let CombinationMandR = grp.All((regDat) => (regDat.ReagentType == (Int32)ReagentType.M || regDat.ReagentType == (Int32)ReagentType.R1R2))
                                select new
                                {
                                    Data = new
                                    {
                                        PortNo = grp.Key + 1,
                                        Analytes = (!MultiLotNo && !MultiMakerCd && CombinationMandR) ? analytes : string.Empty,
                                        LotNo = (!MultiLotNo && !MultiMakerCd && CombinationMandR && !String.IsNullOrEmpty(analytes) && grp.First().LotNo != null) ? grp.First().LotNo : String.Empty,
                                        ExpirationDate = (!MultiLotNo && !MultiMakerCd && CombinationMandR && !String.IsNullOrEmpty(analytes) && grp.First().ExpirationDate.HasValue) ? grp.First().ExpirationDate.Value.ToShortDateString() : String.Empty,
                                        StabilityDate = (!MultiLotNo && !MultiMakerCd && CombinationMandR && !String.IsNullOrEmpty(analytes) && grp.First().StabilityDate.HasValue) ? grp.First().StabilityDate.Value.ToShortDateString() : String.Empty,
                                        // 残量（ul→テストに変換する。分注量＝0の場合は計算できないので空文字とする）
                                        Remain = (!MultiLotNo && !MultiMakerCd && CombinationMandR && !String.IsNullOrEmpty(analytes)
                                            && protocol != null && protocol.R2DispenseVolume > 0 && protocol.MReagDispenseVolume > 0)
                                            ? (protocol.R1DispenseVolume > 0
                                                ? new[] { grp.First((data) => data.PortNo - 1 == grp.Key * 3).Remain / protocol.R1DispenseVolume
                                                    , grp.First((data) => data.PortNo - 1 == grp.Key * 3 + 1).Remain / protocol.R2DispenseVolume
                                                    , grp.First((data) => data.PortNo - 1 == grp.Key * 3 + 2).Remain / protocol.MReagDispenseVolume }.Min().ToString()
                                                : new[] { grp.First((data) => data.PortNo - 1 == grp.Key * 3 + 1).Remain / protocol.R2DispenseVolume
                                                    , grp.First((data) => data.PortNo - 1 == grp.Key * 3 + 2).Remain / protocol.MReagDispenseVolume }.Min().ToString())
                                                : String.Empty,
                                        Check = 0,  //後から設定
                                    },
                                    ExirationData = grp.First().ExpirationDate
                                }).ToList();

            // 試薬編集の場合は編集可能
            List<ReagentRemainDataForTurnTable> reagentList = new List<ReagentRemainDataForTurnTable>();
            foreach (var data in reagentDatas.Select((value) => value.Data).ToList())
            {
                reagentList.Add(new ReagentRemainDataForTurnTable(data.PortNo, data.Analytes, data.LotNo, data.ExpirationDate, data.StabilityDate, data.Remain, data.Check));
            }

            // 複数台構成の場合、試薬の重複チェックを行う
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected != 1)
            {
                //全モジュールに載っている試薬情報を取得
                var reagentNamesInTurnTable = CarisXSubFunction.GetReagentNamesInTurnTable();
                if (reagentNamesInTurnTable.Count != 0)
                {
                    //重複している試薬の情報を取得
                    List<String> duplicationAnalytes = CarisXSubFunction.GetDuplicationAnalytes((Int32)Singleton<PublicMemory>.Instance.moduleIndex, reagentNamesInTurnTable);
                    foreach (var duplicationAnalyte in duplicationAnalytes)
                    {
                        for (int i = 0; i < reagentList.Count; i++)
                        {
                            if (reagentList[i].Analytes == duplicationAnalyte)
                            {
                                //画面表示する内容の内、重複判定となっている試薬にフラグを立てる
                                reagentList[i].Check = 1;
                            }
                        }
                    }
                }
            }

            this.grdReagentList.DataSource = reagentList.ToList();

            // 列の色設定
            switch (dispMode)
            {
                case TurnTableDispMode.Change:
                    // 試薬交換モード
                    this.grdReagentList.SetColorStatusForColumn(this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_REMAIN].Index, colorStatus);
                    break;

                case TurnTableDispMode.Edit:
                    // 試薬編集モード (試薬ボトル残量確認ダイアログのロットＮｏ.と試薬残量列を黄色にする（編集可能列とわかるように）)
                    foreach (UltraGridRow row in this.grdReagentList.Rows)
                    {
                        row.Cells[STRING_LOTNO].Appearance.BackColor = Color.Yellow;
                        row.Cells[STRING_REMAIN].Appearance.BackColor = Color.Yellow;
                        row.Cells[STRING_LOTNO].Activation = Activation.NoEdit;// 画面表示時は編集不可（ポート番号選択で編集可）
                        row.Cells[STRING_REMAIN].Activation = Activation.NoEdit;// 画面表示時は編集不可（ポート番号選択で編集可）
                    }
                    break;

                case TurnTableDispMode.Check:

                    Int32 selectModuleId = CarisXSubFunction.ModuleIndexToModuleId(Singleton<PublicMemory>.Instance.moduleIndex);

                    // 試薬確認モード
                    this.grdReagentList.SetColorStatusForColumn(this.grdReagentList.DisplayLayout.Bands[0].Columns[STRING_REMAIN].Index, colorStatus);

                    // 試薬が有効期限切れの場合、試薬名の文字色を変更(赤)
                    // 検量線がない場合、ロット番号の文字色を変更（赤）
                    // 検量線が有効期限切れの場合、ロット番号の文字色を変更（橙）
                    var setColorExirationData = new Action<RowsCollection>((rows) =>
                    {
                        foreach (var row in rows)
                        {
                            var reagentData = reagentDatas.FirstOrDefault((data) => data.Data.Equals(row.ListObject));
                            if (reagentData != null)
                            {
                                // 試薬の有効期限切れチェック
                                if (reagentData.ExirationData.HasValue && reagentData.ExirationData.Value.AddDays(1) <= DateTime.Today)
                                {
                                    // 試薬の有効期限切れ
                                    row.Cells[STRING_ANALYTES].Appearance.ForeColor = Color.Red;
                                }

                                // 検量線の有無チェック

                                // 分析項目情報取得
                                MeasureProtocol measProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromName( reagentData.Data.Analytes );
                                if (measProtocol != null)
                                {
                                    // 検量線データ抽出
                                    var calibCurveList = Singleton<CalibrationCurveDB>.Instance.GetDataExcludeMasterCurve( measProtocol.ProtocolIndex, reagentData.Data.LotNo, selectModuleId);

                                    // 検量線データ件数チェック
                                    if (calibCurveList.Count > 0)
                                    {
                                        // 成立日を文字列から日付に変換
                                        DateTime approvalDate = DateTime.Now;
                                        Boolean parseResult = DateTime.TryParse( calibCurveList.First().Key, out approvalDate );
                                        if (parseResult)
                                        {
                                            // 変換成功

                                            // 検量線の有効期限切れチェック
                                            if (approvalDate.Date.AddDays( measProtocol.ValidityOfCurve ) < DateTime.Now.Date)
                                            {
                                                // 試薬の有効期限切れ
                                                row.Cells[STRING_LOTNO].Appearance.ForeColor = Color.Orange;
                                            }
                                            else
                                            {
                                                // 有効期限内
                                                row.Cells[STRING_LOTNO].Appearance.ForeColor = Color.Black;
                                            }
                                        }
                                        else
                                        {
                                            // 変換失敗 => データ無しと判定
                                            row.Cells[STRING_LOTNO].Appearance.ForeColor = Color.Red;
                                        }
                                    }
                                    else
                                    {
                                        // 検量線データ無し
                                        row.Cells[STRING_LOTNO].Appearance.ForeColor = Color.Red;
                                    }
                                }
                                else
                                {
                                    // 該当する分析項目無し
                                }
                            }
                        }
                    });
                    setColorExirationData(grdReagentList.Rows);
                    break;
            }
        }

        /// <summary>
        /// Cancelボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 試薬交換中、試薬編集中の場合、準備中断コマンドを送信し
        /// ダイアログ結果にキャンセルを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 試薬交換中、試薬編集中の場合、準備中断コマンドを送信する
            if ((this.dispMode == TurnTableDispMode.Change) || (this.dispMode == TurnTableDispMode.Edit))
            {
                // 準備中断コマンド送信
                SlaveCommCommand_0476 cmd0476 = new SlaveCommCommand_0476();
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd0476);
            }

            // 試薬交換モードか判定
            if (this.dispMode == TurnTableDispMode.Change)
            {
                SlaveCommCommand_0494 cmd0494 = new SlaveCommCommand_0494();

                // 前回のモジュールステータスが待機状態か確認
                if (Singleton<FormSetReagent>.Instance.GetBeforeModuleStatus(Singleton<PublicMemory>.Instance.moduleIndex) == Status.SystemStatusKind.Standby)
                {
                    //試薬保冷庫テーブルSW移動許可コマンド送信
                    cmd0494.SwParam = Convert.ToByte(true);
                }
                else
                {
                    //試薬保冷庫テーブルSW移動許可コマンド送信
                    cmd0494.SwParam = Convert.ToByte(false);
                }
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd0494);
            }
            //--------------------------------------------
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 全選択切り替えボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 全選択切り替えを行います
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            // 試薬交換モードの場合は、Turnボタン押した後は選択変更不可
            if ((this.dispMode != TurnTableDispMode.Change) || (this.isChangeFirst == true))
            {
                Color changeColor = Color.Empty;

                // 全選択状態チェック
                if (changeColor != CarisXConst.BUTTON_SELECT_COLOR)
                {
                    foreach (UltraGridRow row in this.grdReagentList.Rows)
                    {
                        // UNDONE:列の指定方法
                        UltraGridCell cell = row.Cells[STRING_PORTNO];
                        if (cell.Appearance.BackColor == Color.Empty)
                        {
                            changeColor = CarisXConst.BUTTON_SELECT_COLOR;
                            break;
                        }
                    }
                }

                // 全選択切り替え
                foreach (UltraGridRow row in this.grdReagentList.Rows)
                {
                    // UNDONE:列の指定方法
                    UltraGridCell cell = row.Cells[STRING_PORTNO];
                    cell.Appearance.BackColor = changeColor;
                    cell.ButtonAppearance.BackColor = changeColor;
                }

                if (changeColor == CarisXConst.BUTTON_SELECT_COLOR)
                {
                    // 全選択になったのでTurnボタン選択可にする
                    this.btnTurn.Enabled = true;

                    //試薬保冷庫テーブルSW移動許可コマンド送信
                    cmd0494.SwParam = Convert.ToByte(true);
                    Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd0494);
                }
                else
                {
                    // 全非選択になったのでTurnボタン選択不可にする
                    this.btnTurn.Enabled = false;

                    //試薬保冷庫テーブルSW移動許可コマンド送信
                    cmd0494.SwParam = Convert.ToByte(false);
                    Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd0494);
                }

            }
        }

        /// <summary>
        /// Turnボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 試薬交換モードの場合、初回にTurnボタンをクリックした際、
        /// 試薬準備開始コマンドを送信し次のテーブルへ移動します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnTurn_Click(object sender, EventArgs e)
        {
            // 次のテーブルへ移動処理
            TurnTable((int)Singleton<PublicMemory>.Instance.moduleIndex);
        }

        /// <summary>
        /// OKボタンクリックイベント
        /// </summary>
        /// 試薬交換モードの場合、試薬残量変更コマンド、試薬残量変更終了コマンドを送信し、
        /// ダイアログ結果にOKを設定して画面を終了します
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            // 試薬編集モードの場合、試薬残量変更コマンド、試薬残量変更終了コマンドを送信する
            if (this.dispMode == TurnTableDispMode.Edit)
            {
                SlaveCommCommand_0428 cmd0428 = new SlaveCommCommand_0428();

                // 選択中のロット番号、残量の未入力チェック
                foreach (UltraGridRow row in this.grdReagentList.Rows)
                {
                    UltraGridCell cell = row.Cells[STRING_PORTNO];
                    if (cell.Appearance.BackColor == CarisXConst.BUTTON_SELECT_COLOR)
                    {
                        // ロット番号
                        if (row.Cells[STRING_LOTNO].Text == "")
                        {
                            DlgMessage.Show(String.Format(CarisX.Properties.Resources.STRING_DLG_MSG_100, Oelco.CarisX.Properties.Resources.STRING_DLG_TURNTABLE_003), String.Empty, String.Empty, MessageDialogButtons.Confirm);
                            // 試薬残量変更終了コマンド送信
                            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd0428);
                            return;
                        }
                        // 残量
                        if (row.Cells[STRING_REMAIN].Text == "")
                        {
                            DlgMessage.Show(String.Format(CarisX.Properties.Resources.STRING_DLG_MSG_100, Oelco.CarisX.Properties.Resources.STRING_DLG_TURNTABLE_006), String.Empty, String.Empty, MessageDialogButtons.Confirm);
                            // 試薬残量変更終了コマンド送信
                            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd0428);
                            return;
                        }
                    }
                }

                // 選択中（複数可）のポート番号の試薬残量変更コマンドを送信する。
                // 全選択状態取得
                foreach (UltraGridRow row in this.grdReagentList.Rows)
                {
                    UltraGridCell cell = row.Cells[STRING_PORTNO];
                    if (cell.Appearance.BackColor == CarisXConst.BUTTON_SELECT_COLOR)
                    {
                        // 試薬残量変更コマンド送信
                        SlaveCommCommand_0429 cmd0429 = new SlaveCommCommand_0429();
                        // ポート番号
                        cmd0429.PortNumber = Int32.Parse(cell.Text);
                        // ロット番号
                        cmd0429.LotNumber = row.Cells[STRING_LOTNO].Text;
                        // 残量（テスト→ulに変換する）
                        Int32 dispenseVolume = getDispenseVolume(row.Cells[STRING_ANALYTES].Text);
                        if (dispenseVolume > 0)
                        {
                            cmd0429.Remain = (Int32.Parse(row.Cells[STRING_REMAIN].Text) * dispenseVolume);
                        }
                        else
                        {
                            cmd0429.Remain = 0;
                        }
                        // コマンド送信
                        Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd0429);
                    }
                }

                // 試薬残量変更終了コマンド送信
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd0428);
            }

            // 試薬交換モードか判定
            if (this.dispMode == TurnTableDispMode.Change)
            {
                SlaveCommCommand_0494 cmd0494 = new SlaveCommCommand_0494();

                // 前回のモジュールステータスが待機状態か確認
                if (Singleton<FormSetReagent>.Instance.GetBeforeModuleStatus(Singleton<PublicMemory>.Instance.moduleIndex) == Status.SystemStatusKind.Standby)
                {
                    //試薬保冷庫テーブルSW移動許可コマンド送信
                    cmd0494.SwParam = Convert.ToByte(true);
                }
                else
                {
                    //試薬保冷庫テーブルSW移動許可コマンド送信
                    cmd0494.SwParam = Convert.ToByte(false);
                }
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd0494);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 行の選択状態
        /// </summary>
        /// <remarks>
        /// 行の選択状態を設定します
        /// </remarks>
        /// <param name="row">対象行</param>
        private void rowChangeCurrent(UltraGridRow row, Boolean current)
        {
            Color color;
            GradientStyle style;
            if (current)
            {
                color = Color.Orange;
                style = GradientStyle.Vertical;
            }
            else
            {
                color = Color.Empty;
                style = GradientStyle.None;
            }

            // UNDONE:カレント表示方法
            foreach (UltraGridCell cell in row.Cells)
            {
                cell.Appearance.BackColor2 = color;
                cell.Appearance.BackGradientStyle = style;
            }
        }

        /// <summary>
        /// グリッドセルボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// グリッドセルボタンの選択状態を設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void grdReagentList_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {

            // 試薬交換モードの場合は、Turnボタン押した後は選択変更不可
            if ((this.dispMode != TurnTableDispMode.Change) || (this.isChangeFirst == true))
            {
                // 試薬交換モードの場合、未選択はTurnボタンはDisableとする
                if (this.dispMode == TurnTableDispMode.Change)
                {
                    // セルボタン選択状態色切り替え
                    if (e.Cell.Appearance.BackColor == CarisXConst.BUTTON_SELECT_COLOR)
                    {
                        e.Cell.Appearance.BackColor = Color.Empty;
                        e.Cell.ButtonAppearance.BackColor = Color.Empty;
                    }
                    else
                    {
                        e.Cell.Appearance.BackColor = CarisXConst.BUTTON_SELECT_COLOR;
                        e.Cell.ButtonAppearance.BackColor = CarisXConst.BUTTON_SELECT_COLOR;
                    }
                    if (this.getGrdSelectCount() > 0)
                    {
                        this.btnTurn.Enabled = true;

                        if (this.getGrdSelectCount() == 1)
                        {
                            //試薬保冷庫テーブルSW移動許可コマンド送信
                            cmd0494.SwParam = Convert.ToByte(true);
                            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd0494);
                        }
                    }
                    else
                    {
                        this.btnTurn.Enabled = false;

                        //試薬保冷庫テーブルSW移動許可コマンド送信
                        cmd0494.SwParam = Convert.ToByte(false);
                        Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd0494);
                    }
                }

                // 試薬編集モードの場合、編集する際は編集を行うポートＮｏ.のボタンを押下することにより編集可能となる
                if (this.dispMode == TurnTableDispMode.Edit)
                {
                    foreach (UltraGridRow row in this.grdReagentList.Rows)
                    {
                        // 値が設定されていない（設置されていないポート）に対しては、編集可としない
                        if ((e.Cell.Text == row.Cells[STRING_PORTNO].Text) && (!String.IsNullOrEmpty(row.Cells[STRING_REMAIN].Text)))

                        {
                            // セルボタン選択状態色切り替え
                            if (e.Cell.Appearance.BackColor == CarisXConst.BUTTON_SELECT_COLOR)
                            {
                                e.Cell.Appearance.BackColor = Color.Empty;
                                e.Cell.ButtonAppearance.BackColor = Color.Empty;
                            }
                            else
                            {
                                e.Cell.Appearance.BackColor = CarisXConst.BUTTON_SELECT_COLOR;
                                e.Cell.ButtonAppearance.BackColor = CarisXConst.BUTTON_SELECT_COLOR;
                            }

                            if (e.Cell.Appearance.BackColor == CarisXConst.BUTTON_SELECT_COLOR)
                            {
                                // 編集可能にする
                                row.Cells[STRING_LOTNO].Activation = Activation.AllowEdit;
                                row.Cells[STRING_REMAIN].Activation = Activation.AllowEdit;
                            }
                            else
                            {
                                // 編集不可にする
                                row.Cells[STRING_LOTNO].Activation = Activation.NoEdit;
                                row.Cells[STRING_REMAIN].Activation = Activation.NoEdit;
                            }
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 選択ポジション数取得
        /// </summary>
        /// <remarks>
        /// 現在この画面で選択されている反応槽ポジション数を返します。
        /// </remarks>
        /// <returns>反応槽ポジション数</returns>
        private Int32 getGrdSelectCount()
        {
            Int32 retSelectCount = 0;
            foreach (UltraGridRow row in this.grdReagentList.Rows)
            {
                UltraGridCell cell = row.Cells[STRING_PORTNO];
                if (cell.Appearance.BackColor == CarisXConst.BUTTON_SELECT_COLOR)
                {
                    retSelectCount++;
                }
            }
            return retSelectCount;
        }

        /// <summary>
        /// 各試薬の1テストあたりの分注量取得
        /// </summary>
        /// <remarks>
        /// 各試薬の1テストあたりの分注量を取得します
        /// </remarks>
        private Int32 getDispenseVolume(String protocolName)
        {
            Int32 retDispenseVolume = 0;

            // プロトコル名から分析パラメータ取得し、M試薬分注量、R1試薬分注量、R2試薬分注量の中で一番大きい分注量を取得する
            MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromName(protocolName);
            if (protocol != null)
            {
                retDispenseVolume = protocol.MReagDispenseVolume;
                if (retDispenseVolume < protocol.R1DispenseVolume)
                {
                    retDispenseVolume = protocol.R1DispenseVolume;
                }
                if (retDispenseVolume < protocol.R2DispenseVolume)
                {
                    retDispenseVolume = protocol.R2DispenseVolume;
                }
            }

            return retDispenseVolume;
        }

        /// <summary>
        /// フォームClosingイベント
        /// </summary>
        /// <remarks>
        /// グリッド表示順UI設定、グリッド列幅UI設定を保存します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DlgTurnTable_FormClosing(object sender, FormClosingEventArgs e)
        {
            // グリッド表示順UI設定保存
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.TurnTableSettings.ReagentList1GridColOrder = this.grdReagentList.GetGridColumnOrder();
            // グリッド列幅UI設定保存
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.TurnTableSettings.ReagentList1GridColWidth = this.grdReagentList.GetGridColmnWidth();
        }

        #endregion

        private void grdReagentList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            DuplicationCheckEditor editor = new DuplicationCheckEditor();
            e.Layout.Bands[0].Columns[STRING_CHECK].Editor = editor;
        }
    }

    /// <summary>
    /// 試薬残量情報表示データ
    /// </summary>
    /// <remarks>
    /// 試薬残量情報表示用のデータクラスです。
    /// このデータクラスへのアクセスは、全てグリッド内部から行われる為、
    /// コード上には利用箇所が登場しません。
    /// このクラスのメンバ名称はグリッドのColumn名称と連動する為、変更を行う場合は
    /// 使用グリッドのデザイナ上での変更も動作に影響します。
    /// </remarks>
    public class ReagentRemainDataForTurnTable
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data">表示対象試薬情報</param>
        /// <param name="portNo">設置ポート番号</param>
        /// <param name="analytes">分析項目名</param>
        /// <param name="lotNo">試薬ロット番号</param>
        /// <param name="expirationDate">有効期限</param>
        /// <param name="stabilityDate">使用期限</param>
        /// <param name="remain">残量(残テスト数)</param>
        /// <param name="check">重複フラグ</param>
        public ReagentRemainDataForTurnTable(Int32? portNo, String analytes, String lotNo, String expirationDate, String stabilityDate, String remain, Int32 check)
        {
            this.PortNo = portNo;
            this.Analytes = analytes;
            this.LotNo = lotNo;
            this.ExpirationDate = expirationDate;
            this.StabilityDate = stabilityDate;
            this.Remain = remain;
            this.Check = check;
        }

        /// <summary>
        /// 設置ポート番号
        /// </summary>
        public Int32? PortNo
        {
            get;
            set;
        }

        /// <summary>
        /// 分析項目名
        /// </summary>
        public String Analytes
        {
            get;
            set;
        }

        /// <summary>
        /// 試薬ロット番号
        /// </summary>
        public String LotNo
        {
            get;
            set;
        }

        /// <summary>
        /// 有効期限
        /// </summary>
        public String ExpirationDate
        {
            get;
            set;
        }

        /// <summary>
        /// 使用期限
        /// </summary>
        public String StabilityDate
        {
            get;
            set;
        }

        /// <summary>
        /// 残量(残テスト数)
        /// </summary>
        String remain = String.Empty;

        /// <summary>
        /// 残量(残テスト数)
        /// </summary>
        public String Remain
        {
            // 00000などの入力時、データ反映後0の表示を行う対応
            get
            {
                return this.remain;
            }
            set
            {
                Int32 intValue = 0;
                if (Int32.TryParse(value, out intValue))
                {
                    this.remain = intValue.ToString();
                }
                else
                {
                    this.remain = String.Empty;
                }
            }
        }

        /// <summary>
        /// 重複フラグ
        /// </summary>
        public Int32 Check
        {
            get;
            set;
        }
    }
}

public class DuplicationCheckEditor : ControlContainerEditor
{
    #region Private Members
    private Infragistics.Win.UltraWinEditors.UltraPictureBox pbxDuplicationCheck;
    #endregion

    #region Constructor
    public DuplicationCheckEditor()
    {
        this.pbxDuplicationCheck = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
        this.pbxDuplicationCheck.AutoSize = false;
        this.pbxDuplicationCheck.Dock = DockStyle.Fill;
        this.pbxDuplicationCheck.BackColor = Color.Transparent;

        // コントロールをレンダリング コントロールとして設定します
        this.RenderingControl = pbxDuplicationCheck;
        this.RenderingControl.BackColor = Color.Transparent;
    }
    #endregion

    #region override RendererValue
    protected override object RendererValue
    {
        get
        {
            if (this.pbxDuplicationCheck.DefaultImage == Oelco.CarisX.Properties.Resources.Image_TotalBufferStatus_Cross)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if (Int32.Parse(value.ToString()) == 1)
            {
                this.pbxDuplicationCheck.DefaultImage = Oelco.CarisX.Properties.Resources.Image_TotalBufferStatus_Cross;
            }
            else
            {
                this.pbxDuplicationCheck.DefaultImage = null;
            }

        }
    }
    #endregion

}
