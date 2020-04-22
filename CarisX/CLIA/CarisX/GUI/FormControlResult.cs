using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Oelco.CarisX.DB;
using Oelco.Common.GUI;
using Oelco.Common.Utility;
using Infragistics.Win.UltraWinGrid;
using System.ComponentModel;
using Oelco.CarisX.Calculator;
using Oelco.Common.DB;
using Oelco.CarisX.GUI.Controls;
using Oelco.CarisX.Print;
using Oelco.CarisX.Const;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.Common.Parameter;
using Oelco.CarisX.Utility;
using Oelco.Common.Const;
using Oelco.CarisX.Status;
using Oelco.CarisX.Common;
using System.Threading;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 精度管理検体測定データ画面クラス
    /// </summary>
    public partial class FormControlResult : FormChildBase, IFormMeasureResult
    {
        #region [定数定義]

        /// <summary>
        /// 削除
        /// </summary>
        public const String DELETE = "Delete";

        /// <summary>
        /// ファイル出力
        /// </summary>
        public const String EXPORT = "Export";

        /// <summary>
        /// 印刷
        /// </summary>
        public const String PRINT = "Print";

        /// <summary>
        /// オンライン出力
        /// </summary>
        public const String TRANSMIT = "Transmit";

        /// <summary>
        /// コントロール範囲変数
        /// </summary>
        private int pageCurrent = 1, pageCount = 1, pageSize = 10000;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 現在の精度管理検体測定結果情報
        /// </summary>
        private List<ControlResultData> currentControlResultInfo = new List<ControlResultData>();

        /// <summary>
        /// 現在の精度管理検体測定結果情報(バインド用)
        /// </summary>
        private BindingList<ControlResultData> bindCurrentControlResultInfo;

        /// <summary>
        /// データ更新
        /// </summary>
        private EventHandler updateData = null;

        /// <summary>
        /// グリッドの列情報
        /// </summary>
        Dictionary<String, String> gridColmunDic = new Dictionary<string, string>();

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormControlResult()
        {
            InitializeComponent();

            // 拡大率切替コントロール初期化
            this.zoomPanel.ZoomStep = CarisXConst.GRID_ZOOM_STEP;

            // 拡大率変更イベント登録
            this.zoomPanel.SetZoom += this.grdControlResult.SetGridZoom;

            // コマンドバーのイベント追加
            this.tlbCommandBar.Tools[DELETE].ToolClick += (sender, e) => this.deleteData();
            this.tlbCommandBar.Tools[PRINT].ToolClick += (sender, e) => this.printData();
            this.tlbCommandBar.Tools[EXPORT].ToolClick += (sender, e) => this.exportData();
            this.tlbCommandBar.Tools[TRANSMIT].ToolClick += (sender, e) => this.transmit();

            // リアルタイムデータ更新イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.RealtimeData, this.onRealTimeDataChanged);
            // システムステータス変更イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SystemStatusChanged, this.onSystemStatusChanged);
            // 印刷パラメータ変更イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UseOfPrint, this.onPrintParamChanged);
            // ホスト有無パラメータ変更イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UseOfHost, this.onHostParamChanged);
            // ホストへのデータ送信完了イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SendControlDataHostComplete, this.sendHostComplete);
            // ユーザレベル変更イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UserLevelChanged, this.setUser);

            this.grdControlResult.SetGridRowBackgroundColorRuleFromCellData(
                new[] { ControlResultData.DataKeys.SequenceNo, ControlResultData.DataKeys.Analytes }.ToList(),
                new[] { CarisXConst.GRID_ROWS_DEFAULT_COLOR, CarisXConst.GRID_ROWS_COLOR_PATTERN1 }.ToList());

            //设置ToolBar的右键功能不可用
            this.tlbCommandBar.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.tlbCommandBar_BeforeToolbarListDropdown);

            gridColmunDic = new Dictionary<String, String>()
                {
                    {ControlResultData.DataKeys.SequenceNo, Oelco.CarisX.Properties.Resources.STRING_CONTROLRESULT_000},
                    {ControlResultData.DataKeys.RackID, Oelco.CarisX.Properties.Resources.STRING_CONTROLRESULT_001},
                    {ControlResultData.DataKeys.RackPosition, Oelco.CarisX.Properties.Resources.STRING_CONTROLRESULT_002},
                    {ControlResultData.DataKeys.ControlName, Oelco.CarisX.Properties.Resources.STRING_CONTROLRESULT_003},
                    {ControlResultData.DataKeys.ControlLotNo, Oelco.CarisX.Properties.Resources.STRING_CONTROLRESULT_004},
                    {ControlResultData.DataKeys.Comment, Oelco.CarisX.Properties.Resources.STRING_CONTROLRESULT_005},
                    {ControlResultData.DataKeys.Analytes, Oelco.CarisX.Properties.Resources.STRING_CONTROLRESULT_006},
                    {ControlResultData.DataKeys.Count, Oelco.CarisX.Properties.Resources.STRING_CONTROLRESULT_007},
                    {ControlResultData.DataKeys.CountAve, Oelco.CarisX.Properties.Resources.STRING_CONTROLRESULT_008},
                    {ControlResultData.DataKeys.Concentration, Oelco.CarisX.Properties.Resources.STRING_CONTROLRESULT_009},
                    {ControlResultData.DataKeys.ConcentrationAve, Oelco.CarisX.Properties.Resources.STRING_CONTROLRESULT_010},
                    {ControlResultData.DataKeys.ReplicationNo, Oelco.CarisX.Properties.Resources.STRING_CONTROLRESULT_011},
                    {ControlResultData.DataKeys.Remark, Oelco.CarisX.Properties.Resources.STRING_CONTROLRESULT_013},
                    {ControlResultData.DataKeys.CalibCurveDateTime, Oelco.CarisX.Properties.Resources.STRING_CONTROLRESULT_014},
                    {ControlResultData.DataKeys.ReagentLotNo, Oelco.CarisX.Properties.Resources.STRING_CONTROLRESULT_015},
                    {ControlResultData.DataKeys.PretriggerLotNo, Oelco.CarisX.Properties.Resources.STRING_CONTROLRESULT_016},
                    {ControlResultData.DataKeys.TriggerLotNo, Oelco.CarisX.Properties.Resources.STRING_CONTROLRESULT_017},
                    {ControlResultData.DataKeys.MeasureDateTime, Oelco.CarisX.Properties.Resources.STRING_CONTROLRESULT_018},
                    {ControlResultData.DataKeys.ErrorCode, Oelco.CarisX.Properties.Resources.STRING_CONTROLRESULT_023},
                    {ControlResultData.DataKeys.DarkCount, Properties.Resources.STRING_CONTROLRESULT_024},
                    {ControlResultData.DataKeys.BGCount, Properties.Resources.STRING_CONTROLRESULT_025},
                    {ControlResultData.DataKeys.ResultCount, Properties.Resources.STRING_CONTROLRESULT_026},
                    {ControlResultData.DataKeys.ModuleNo, Properties.Resources.STRING_CONTROLRESULT_027},
                };
        }

        //设置ToolBar的右键功能不可用
        private void tlbCommandBar_BeforeToolbarListDropdown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventArgs e)
        {
            e.Cancel = true;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 絞り込みパネルの表示状態の取得、設定
        /// </summary>
        public Boolean DispSearchInfoPanel
        {
            get
            {
                return this.searchInfoPanelControlResult.Visible;
            }
            protected set
            {
                this.recalcInfoPanelControlResult.Visible = false;
                this.searchInfoPanelControlResult.Visible = value;
            }
        }

        /// <summary>
        /// 再計算パネルの表示状態の取得、設定
        /// </summary>
        public Boolean DispRecalcInfoPanel
        {
            get
            {
                return this.recalcInfoPanelControlResult.Visible;
            }
            protected set
            {
                this.searchInfoPanelControlResult.Visible = false;
                this.recalcInfoPanelControlResult.Visible = value;
            }
        }

        /// <summary>
        /// 測定結果列の取得
        /// </summary>
        public Dictionary<String, String> ResultGridColumns
        {
            get
            {
                Dictionary<String, String> rtnVal = new Dictionary<String, String>(gridColmunDic);

                if (!Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult))
                {
                    rtnVal.Remove(ControlResultData.DataKeys.DarkCount);
                    rtnVal.Remove(ControlResultData.DataKeys.BGCount);
                    rtnVal.Remove(ControlResultData.DataKeys.ResultCount);
                }

                return rtnVal;
            }
        }

        #endregion

        #region [publicメソッド]

        #region _Realtime印刷_

        ControlResultPrint prt = new ControlResultPrint();
        /// <summary>
        /// リアルタイム印刷
        /// </summary>
        /// <remarks>
        /// リアルタイム印刷を行います。
        /// 即実行フラグによってページ単位で印刷するか、全て印刷するか選択します。
        /// </remarks>
        /// <param name="immediatelyExecute">即実行フラグ</param>
        public void RealtimePrint(Boolean immediatelyExecute)
        {
            List<Tuple<int, int, int>> dataForCount = Singleton<InProcessSampleInfoManager>.Instance.AskRealTimePrintQueue(RealtimePrintDataType.Control);
            List<Tuple<int, int, int>> dataForPrint = null;

            Boolean printExecute = immediatelyExecute;

            if (!immediatelyExecute)
            {
                Int32 pageCount = prt.GetTotalPageCount(dataForCount);

                // 1ページを越えた場合、印刷を行う
                if (pageCount > 1)
                {
                    // datas.Countは前回本関数がよびだされてから1件単位でしか追加されず、今回呼び出しでページ数が2になる前は1ページの件数なので、
                    // 1件引いて取得する。
                    dataForPrint = Singleton<InProcessSampleInfoManager>.Instance.PopRalTimePrintQueueData(RealtimePrintDataType.Control, dataForCount.Count - 1);
                }
            }
            else
            {
                dataForPrint = Singleton<InProcessSampleInfoManager>.Instance.PopRalTimePrintQueueData(RealtimePrintDataType.Control);
            }

            if (dataForPrint != null && dataForPrint.Count != 0)
            {
                // データを生成する
                var searchDic = (from dbData in Singleton<ControlResultDB>.Instance.GetData()
                                 select new OutPutControlResultData(dbData))
                    .ToDictionary((v) => new Tuple<Int32, Int32>(v.GetUniqueNo(), v.ReplicationNo), (v) => v);

                var outputData = from v in dataForPrint
                                 let key = new Tuple<int, int>(v.Item2, v.Item3)
                                 where searchDic.ContainsKey(key)
                                 select searchDic[key];
                var printData = this.convertPrintData(outputData);

                // 印刷する
                Boolean ret = prt.Print(printData, Singleton<InProcessSampleInfoManager>.Instance.GetNextRealtimePrintPageNumber(RealtimePrintDataType.Control));

            }

        }

        /// <summary>
        /// 印刷用データ変換処理
        /// </summary>
        /// <remarks>
        /// DBから取得したデータを、印刷用データに変換します。
        /// </remarks>
        /// <param name="resultData">DBデータ</param>
        /// <returns>印刷用データ</returns>
        private List<ControlResultReportData> convertPrintData(IEnumerable<OutPutControlResultData> resultData)
        {
            List<ControlResultReportData> rptData = new List<ControlResultReportData>();

            foreach (var row in resultData)
            {
                ControlResultReportData rptDataRow = new ControlResultReportData();
                rptDataRow.SeqNo = row.SequenceNo.ToString();
                rptDataRow.RackID = row.RackId.ToString();
                rptDataRow.ControlName = row.ControlName;
                rptDataRow.ControlLot = row.ControlLotNo;
                rptDataRow.Comment = row.Comment;
                rptDataRow.ProtoName = row.Analytes;
                rptDataRow.Count = row.Count;
                rptDataRow.CountAvg = row.CountAve;
                rptDataRow.Conc = row.Concentration;
                rptDataRow.ConcAvg = row.ConcentrationAve;
                rptDataRow.MultiMeas = (Int32)row.ReplicationNo;
                rptDataRow.ReagentLotNo = row.ReagentLotNo;
                rptDataRow.PreTriggerLotNo = row.PretriggerLotNo;
                rptDataRow.TriggerLotNo = row.TriggerLotNo;
                rptDataRow.MeasTime = row.MeasureDateTime.ToDispString();
                rptDataRow.MeasDate = row.MeasureDateTime.ToString("yyyyMMdd");
                rptDataRow.PrintDateTime = DateTime.Now.ToDispString();

                String[] remarks = row.Remark.Split(',');

                if (remarks.Length > 3)
                {
                    String[] editRemarks = new String[3];
                    Array.Copy(remarks, editRemarks, 3);
                    rptDataRow.Remark = String.Join(",", editRemarks) + "...";
                }
                else
                {
                    rptDataRow.Remark = row.Remark;
                }

                rptData.Add(rptDataRow);
            }

            return rptData;
        }

        #endregion
        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// データ変更リアルタイム更新
        /// </summary>
        /// <remarks>
        /// データ変更をリアルタイム更新します
        /// </remarks>
        /// <param name="kind">種別</param>
        protected void onRealTimeDataChanged(object kind)
        {// 精度管理測定データ
            if (((RealtimeDataKind)kind) == RealtimeDataKind.ControlResult)
            {
                if (this.updateData == null)
                {
                    this.updateData = (obj, e) =>
                    {
                        // 精度管理測定データ画面 情報更新
                        this.loadData();
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
            // スクロール処理設定
            this.gesturePanel.ScrollProxy = this.grdControlResult.ScrollProxy;
            // ズーム率設定
            this.zoomPanel.Zoom = Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.ControlResultSettings.GridZoom;
            // グリッド列表示順設定
            this.grdControlResult.SetGridColumnOrder(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.ControlResultSettings.GridColOrder);
            // グリッド列幅設定
            this.grdControlResult.SetGridColmnWidth(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.ControlResultSettings.GridColWidth);
            // 印刷ボタン表示設定
            this.tlbCommandBar.Tools[PRINT].SharedProps.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable;
            // オンライン出力ボタン表示設定
            this.tlbCommandBar.Tools[TRANSMIT].SharedProps.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Enable;
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            this.Text = Properties.Resources.STRING_CONTROLRESULT_019;

            // ページめくりボタン
            this.btnPre.Text = Properties.Resources.STRING_CONTROLRESULT_028;
            this.btnNext.Text = Properties.Resources.STRING_CONTROLRESULT_029;

            // コマンドバーアイテム名設定
            this.tlbCommandBar.Tools[DELETE].SharedProps.Caption = Properties.Resources.STRING_COMMANDBARITEM_002;
            this.tlbCommandBar.Tools[EXPORT].SharedProps.Caption = Properties.Resources.STRING_COMMANDBARITEM_010;
            this.tlbCommandBar.Tools[PRINT].SharedProps.Caption = Properties.Resources.STRING_COMMANDBARITEM_004;
            this.tlbCommandBar.Tools[TRANSMIT].SharedProps.Caption = Properties.Resources.STRING_COMMANDBARITEM_011;

            // グリッドカラムヘッダー表示設定
            foreach (var gridColmun in gridColmunDic)
            {
                this.grdControlResult.DisplayLayout.Bands[0].Columns[gridColmun.Key].Header.Caption = gridColmun.Value;
            }

            if (this.grdControlResult.DisplayLayout.Bands[0].Columns.Count != 0)
            {
                //起動時はまだ列の定義が存在しない為、分岐させておく
                this.grdControlResult.DisplayLayout.Bands[0].Columns[ControlResultData.DataKeys.DarkCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
                this.grdControlResult.DisplayLayout.Bands[0].Columns[ControlResultData.DataKeys.BGCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
                this.grdControlResult.DisplayLayout.Bands[0].Columns[ControlResultData.DataKeys.ResultCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
            }

            // ボタン
            this.btnFilter.Text = Properties.Resources.STRING_CONTROLRESULT_021;
            this.btnRecalc.Text = Properties.Resources.STRING_CONTROLRESULT_022;
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

            // Deleteボタンの活性/非活性の設定
            this.tlbCommandBar.Tools[DELETE].SharedProps.Enabled = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.SampleDataEditDelete);
            //　Re-Calcボタンの表示/非表示の設定
            btnRecalc.Visible = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.CalibratorEditRecalc);
            if (!btnRecalc.Visible)
            {
                this.DispRecalcInfoPanel = false;
            }
            //add by marxsu  数据修改权限的设置
            this.searchInfoPanelControlResult.RemarkDataEditedEnable = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.SetRemarkDataEditedEnable);

            if (this.grdControlResult.DisplayLayout.Bands[0].Columns.Count != 0)
            {
                //起動時はまだ列の定義が存在しない為、分岐させておく
                this.grdControlResult.DisplayLayout.Bands[0].Columns[ControlResultData.DataKeys.DarkCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
                this.grdControlResult.DisplayLayout.Bands[0].Columns[ControlResultData.DataKeys.BGCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
                this.grdControlResult.DisplayLayout.Bands[0].Columns[ControlResultData.DataKeys.ResultCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
            }
        }
        #endregion

        #region [privateメソッド]

        #region _コマンドバー_

        /// <summary>
        /// 指定の測定結果情報の削除
        /// </summary>
        /// <remarks>
        /// 操作履歴に削除実行を登録し、精度管理検体測定結果データの削除実行して反映を行います
        /// </remarks>
        private void deleteData()
        {
            // 操作履歴：削除実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_003 });

            var selectRows = this.grdControlResult.SearchSelectRow().Select((row) => ((ControlResultData)row.ListObject).GetUniqueNo()).Distinct();
            var selectRowDatas = this.grdControlResult.Rows.Where((row) => selectRows.Contains(((ControlResultData)row.ListObject).GetUniqueNo())).Select((row) => (ControlResultData)row.ListObject).ToList();

            // 削除対象データ
            var deleteDatas = selectRowDatas.Where((data) => !Singleton<InProcessSampleInfoManager>.Instance.InProcessSampleList.Exists((info) => info.GetUniqueNumbers().Contains(data.GetUniqueNo())));

            if (DialogResult.OK == DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_038, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel))
            {
                deleteDatas.DeleteAllDataList();

                // 変更を反映
                Singleton<ControlResultDB>.Instance.SetData(deleteDatas.ToList());
                Singleton<ControlResultDB>.Instance.CommitData();
                this.loadData();

                // 削除対象に分析中の測定結果が含まれていた場合
                if (deleteDatas.Count() != selectRowDatas.Count())
                {
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_085, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.Confirm);
                }
            }
            else
            {
                // 操作履歴：削除キャンセル 
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_004 });
            }
        }

        /// <summary>
        /// 測定結果情報の印刷出力
        /// </summary>
        /// <remarks>
        /// 操作履歴に印刷実行を登録し、精度管理検体測定結果データの印刷を実行します
        /// </remarks>
        private void printData()
        {
            // 操作履歴登録：印刷実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_005 });

            // 印刷対象を取得
            List<OutPutControlResultData> printData;
            if (this.outputData(out printData))
            {
                if (printData.Count > 0)
                {
                    // 印刷用Listに取得データを格納
                    List<ControlResultReportData> rptData = this.convertPrintData(printData);

                    ControlResultPrint prt = new ControlResultPrint();
                    Boolean ret = prt.Print(rptData);

                }
                else
                {
                    // 印刷するデータがありません。
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_064, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_003, MessageDialogButtons.Confirm);
                }
            }
            else
            {
                // 操作履歴登録：印刷キャンセル
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_006 });
            }
        }

        /// <summary>
        /// 測定結果情報のファイル出力
        /// </summary>
        /// <remarks>
        /// 操作履歴にファイル出力実行を登録し、精度管理検体測定結果情報のファイル出力を行います
        /// </remarks>
        private void exportData()
        {
            // 操作履歴登録：ファイル出力実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_007 });

            // ファイル出力対象を取得
            List<OutPutControlResultData> exportData;
            if (this.outputData(out exportData))
            {
                if (exportData.Count > 0)
                {
                    String fileName;
                    if (CarisXSubFunction.ShowSaveCSVFileDialog(out fileName, OutputFileKind.CSV, CarisXConst.EXPORT_CSV_CONTROLRESULT, CarisX.Properties.Resources.STRING_CONTROLRESULT_020, Singleton<CarisXUISettingManager>.Instance.ControlResultSettings) == System.Windows.Forms.DialogResult.OK)
                    {
                        Singleton<DataHelper>.Instance.ExportCsv(exportData, this.ResultGridColumns, fileName);
                    }
                }
                else
                {
                    // UNDONE:ファイル出力するデータがありません。
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_169, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.Confirm);
                }
            }
            else
            {
                // 操作履歴登録：ファイル出力キャンセル
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_008 });

            }
        }

        /// <summary>
        /// オンライン出力
        /// </summary>
        /// <remarks>
        /// 操作履歴にオンライン出力実行を登録し、精度管理検体測定結果情報のオンライン出力を行います
        /// </remarks>
        private void transmit()
        {
            // 操作履歴登録：オンライン出力実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_012 });

            // ファイル出力対象を取得
            List<OutPutControlResultData> transmitData;
            if (this.outputData(out transmitData))
            {
                if (transmitData == null || transmitData.Count == 0)
                {
                    // オンライン出力するデータがありません。
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_170, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.Confirm);
                }
                else if (Singleton<SystemStatus>.Instance.Status == SystemStatusKind.Assay && transmitData.Count > CarisXConst.TRANSMIT_DATA_MAX)
                {
                    // データ数が200を超えています
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_211 + CarisXConst.TRANSMIT_DATA_MAX.ToString() + CarisX.Properties.Resources.STRING_DLG_MSG_212,
                        String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.Confirm);
                }
                else
                {
                    // オンライン出力
                    this.sendHost(transmitData);
                }
            }
            else
            {
                // 操作履歴登録：オンライン出力キャンセル
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_013 });
            }
        }
        /// <summary>
        /// 待機メッセージ
        /// </summary>
        DlgMessage waitMessage = null;

        /// <summary>
        /// ホスト送信
        /// </summary>
        /// <remarks>
        /// 精度管理検体測定結果情報のホスト送信を行います
        /// </remarks>
        /// <param name="data"></param>
        private void sendHost(List<OutPutControlResultData> data)
        {
            using (this.waitMessage = DlgMessage.Create(CarisX.Properties.Resources.STRING_DLG_MSG_209,
                                        "", CarisX.Properties.Resources.STRING_DLG_TITLE_008, MessageDialogButtons.Cancel))
            {
                ManualResetEvent canceller = new ManualResetEvent(false);
                Singleton<CarisXSequenceHelperManager>.Instance.Host.HostCommunicationSequence
                    (HostCommunicationSequencePattern.SendResultToHostBatch | HostCommunicationSequencePattern.Control, data, canceller);
                if (this.waitMessage.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                {
                    canceller.Set();
                }
            }
            this.waitMessage = null;
        }

        /// <summary>
        /// ホスト送信完了
        /// </summary>
        /// <remarks>
        /// ホスト送信完了時、メッセージをクローズします
        /// </remarks>
        /// <param name="value"></param>
        private void sendHostComplete(Object value)
        {
            if (this.waitMessage != null)
            {
                //ダイアログだけ先に消えて黒背景だけ残る現象を防ぐ
                while (Singleton<SystemStatus>.Instance.SubStatus[SubStatusKind.InAskHost])
                {
                    Application.DoEvents();
                }

                this.waitMessage.Close();
            }
        }

        /// <summary>
        /// 出力対象データの取得
        /// </summary>
        /// <remarks>
        /// 出力対象の精度管理検体測定結果情報データを取得します
        /// </remarks>
        /// <param name="outputData">出力対象データ</param>
        /// <returns>true:出力実行あり/false:出力実行なし</returns>
        private Boolean outputData(out List<OutPutControlResultData> outputData)
        {
            TargetRange outputRange = DlgTargetSelectRange.Show();
            outputData = new List<OutPutControlResultData>();
            switch (outputRange)
            {
                case TargetRange.All:
                    for (int i = 0; i < this.grdControlResult.Rows.Count; i++)
                    {
                        OutPutControlResultData item = new OutPutControlResultData(this.grdControlResult.Rows[i].ListObject as DataRowWrapperBase);
                        outputData.Add(item);
                    }
                    outputData.RollBackDataList();
                    break;
                case TargetRange.Specification:

                    // 選択した対象を取ってくる
                    // 選択された検体の全レプリケーションが対象となる。
                    var selectRows = this.grdControlResult.SearchSelectRow().Select((row) => ((ControlResultData)row.ListObject).GetUniqueNo()).Distinct();
                    var selectRowDatas = this.grdControlResult.Rows.Where((row) => selectRows.Contains(((ControlResultData)row.ListObject).GetUniqueNo())).Select((row) => (ControlResultData)row.ListObject);

                    // 転送対象データ
                    outputData = selectRowDatas.Where((data) => !Singleton<InProcessSampleInfoManager>.Instance.InProcessSampleList.Exists((info) => info.GetUniqueNumbers().Contains(data.GetUniqueNo()))).Select((data) => new OutPutControlResultData(data)).ToList();
                    outputData.RollBackDataList();

                    break;
                default:
                    outputData = new List<OutPutControlResultData>();
                    break;
            }

            return outputRange != TargetRange.None;
        }
        #endregion

        /// <summary>
        /// 絞込みボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 絞込みパネルを表示します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnFilter_Click(object sender, EventArgs e)
        {
            this.DispSearchInfoPanel = true;
        }

        /// <summary>
        /// 再計算ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 再計算パネルを表示します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnRecalc_Click(object sender, EventArgs e)
        {
            this.DispRecalcInfoPanel = true;
        }

        #region _絞り込みパネルイベント_

        /// <summary>
        /// 絞り込みパネルOKボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 精度管理検体測定結果情報の絞込みを実行します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void searchInfoPanelControlResult_OkClick(object sender, EventArgs e)
        {
            var searchDatas = Singleton<ControlResultDB>.Instance.GetSearchData(this.searchInfoPanelControlResult);
            // 修改内容：ページめくり処理追加
            if (IsAllowPageBreaks(this.currentControlResultInfo.Count))
                this.grdControlResult.DataSource = new BindingList<ControlResultData>(searchDatas.Take(this.pageSize).ToList());
            else
                this.grdControlResult.DataSource = new BindingList<ControlResultData>(searchDatas);

            //this.grdControlResult.DataSource = new BindingList<ControlResultData>(searchDatas);
            this.btnFilter.Appearance.ImageBackground = CarisX.Properties.Resources.Image_SelectButton_selected;
        }

        /// <summary>
        /// 絞り込みパネルキャンセルボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 精度管理検体測定結果情報の絞込みの実行をキャンセルします
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void searchInfoPanelControlResult_CancelClick(object sender, EventArgs e)
        {
            this.bindCurrentControlResultInfo = null;
            this.loadData();
            this.btnFilter.Appearance.ImageBackground = CarisX.Properties.Resources.Image_SelectButton;
        }

        /// <summary>
        /// 絞り込みパネル閉じるボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 絞り込みパネルを閉じます
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void searchInfoPanelControlResult_CloseClick(object sender, EventArgs e)
        {
            this.DispSearchInfoPanel = false;
        }

        #endregion

        #region _再計算パネルイベント_

        /// <summary>
        /// 再計算OKボタンクリック
        /// </summary>
        /// <remarks>
        /// 精度管理検体測定結果情報の再計算を実行します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void recalcInfoPanelControlResult_OkClick(object sender, EventArgs e)
        {
            RecalcInfoPanelControlResult recalcInfo = this.recalcInfoPanelControlResult;

            var recalcData = (from data in Singleton<ControlResultDB>.Instance.GetReCalcData(recalcInfo)
                              let calcData = new CalcData( data.GetModuleNo()
                                                         , data.GetMeasureProtocolIndex()
                                                         , data.ReagentLotNo
                                                         , data.GetIndividuallyNo()
                                                         , data.GetUniqueNo()
                                                         , data.ReplicationNo
                                                         , 1
                                                         , 1
                                                         , data.MeasureDateTime
                                                         , data.RackId
                                                         , data.RackPosition
                                                         , data.ControlLotNo )
                              select new
                              {
                                  data,
                                  calcData
                              }).ToDictionary((pair) => pair.calcData);

            List<CalcData> completeData;

            foreach (var data in recalcData)
            {
                data.Key.CalcInfoReplication = new CalcInfo(data.Value.data.GetCount());
                data.Key.CalcInfoReplication.Remark = data.Value.data.GetReplicationRemarkId();
                if (!String.IsNullOrEmpty(data.Value.data.CountAve) || data.Value.data.GetRemarkId() != null)
                {
                    data.Key.CalcInfoAverage = new CalcInfo(data.Value.data.GetCountAveValue());
                    data.Key.CalcInfoAverage.Remark = data.Value.data.GetRemarkId();
                }
            }

            if (CarisXCalculator.ReCalcControl(recalcInfo, recalcData.Keys.ToList(), out completeData) && completeData != null)
            {
                completeData.ForEach((calcData) =>
               {
                    // 測定結果設定
                    Int32 digits = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(calcData.ProtocolIndex).LengthAfterDemPoint;
                   var updateData = recalcData.Values.SingleOrDefault((data) => data.calcData.IndividuallyNo == calcData.IndividuallyNo && data.calcData.UniqueNo == calcData.UniqueNo && data.calcData.ReplicationNo == calcData.ReplicationNo);
                   if (updateData != null)
                   {

                       if (calcData.CalcInfoReplication != null)
                       {
                           updateData.data.SetCount(calcData.CalcInfoReplication.CountValue);

                           if (calcData.CalcInfoReplication.Concentration.HasValue)
                           {
                               updateData.data.SetConcentration(SubFunction.ToRoundOffParse(calcData.CalcInfoReplication.Concentration.Value, digits));
                           }
                           else
                           {
                               updateData.data.SetConcentration(null);
                           }

                           updateData.data.SetReplicationRemarkId(calcData.CalcInfoReplication.Remark);
                       }

                       if (calcData.CalcInfoAverage != null)
                       {
                           updateData.data.SetCountAve(calcData.CalcInfoAverage.CountValue);

                           if (calcData.CalcInfoAverage.Concentration.HasValue)
                           {
                               updateData.data.SetConcentrationAve(SubFunction.ToRoundOffParse(calcData.CalcInfoAverage.Concentration.Value, digits));
                           }
                           else
                           {
                               updateData.data.SetConcentrationAve(null);
                           }
                           updateData.data.SetRemarkId(calcData.CalcInfoAverage.Remark);
                       }

                       updateData.data.SetCalibCurveDateTime(calcData.UseCalcCalibCurveApprovalDate);
                   }
               });

                Singleton<ControlResultDB>.Instance.SetData(recalcData.Select((data) => data.Value.data).ToList());
                Singleton<ControlResultDB>.Instance.CommitData();

                this.loadData();
            }
        }

        /// <summary>
        /// 再計算閉じるボタンクリック
        /// </summary>
        /// <remarks>
        /// 再計算パネルを閉じます
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void recalcInfoPanelControlResult_CloseClick(object sender, EventArgs e)
        {
            this.DispRecalcInfoPanel = false;
        }

        #endregion

        /// <summary>
        /// フォーム読み込みイベント
        /// </summary>
        /// <remarks>
        /// 画面初期化します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void FormControlResult_Load(object sender, EventArgs e)
        {
            Singleton<ControlResultDB>.Instance.LoadDB();
            this.loadData();
        }

        /// <summary>
        /// データの読み込み
        /// </summary>
        /// <remarks>
        /// データの読み込みします
        /// </remarks>
        private void loadData()
        {
            var list = this.grdControlResult.DataSource as BindingList<ControlResultData>;
            if (this.bindCurrentControlResultInfo == null || list == null)
            {
                this.currentControlResultInfo = Singleton<ControlResultDB>.Instance.GetSearchData();
                this.bindCurrentControlResultInfo = new BindingList<ControlResultData>(this.currentControlResultInfo);
                // 修改内容：ページめくり処理追加
                //this.grdControlResult.DataSource = this.bindCurrentControlResultInfo;
                
                if (IsAllowPageBreaks(this.bindCurrentControlResultInfo.Count))
                    this.grdControlResult.DataSource = new BindingList<ControlResultData>(this.currentControlResultInfo.Take(this.pageSize).ToList());
                else
                    this.grdControlResult.DataSource = this.bindCurrentControlResultInfo;

            }
            else
            {
                list.RaiseListChangedEvents = false;
                list.Clear();
                // 修改内容：ページめくり処理追加
                //if (list == this.bindCurrentControlResultInfo)
                //{
                //    Singleton<ControlResultDB>.Instance.GetSearchData().ForEach((data) => list.Add(data));
                //}
                //else
                //{
                //    Singleton<ControlResultDB>.Instance.GetSearchData(this.searchInfoPanelControlResult).ForEach((data) => list.Add(data));
                //}
                int iPageIndex = this.pageCurrent;
                if (IsAllowPageBreaks(this.currentControlResultInfo.Count))
                {
                    if (iPageIndex > this.pageCount)
                    {
                        iPageIndex = this.pageCount;
                    }

                    this.pageCurrent = iPageIndex;
                    this.teCurrent.Value = this.pageCurrent;

                    if ((this.pageCurrent > 1 && this.pageCurrent < this.pageCount) || this.pageCurrent == this.pageCount)
                    {
                        int iSkipNum = (this.pageCurrent - 1) * this.pageSize;
                        this.currentControlResultInfo.Skip(iSkipNum).Take(this.pageSize).ToList().ForEach((data) => list.Add(data));
                    }
                    if (this.pageCurrent == 1)
                    {
                        this.currentControlResultInfo.Take(this.pageSize).ToList().ForEach((data) => list.Add(data));
                    }
                }
                else
                {
                    this.currentControlResultInfo.Take(this.pageSize).ToList().ForEach((data) => list.Add(data));
                }

                list.RaiseListChangedEvents = true;
                list.ResetBindings();
            }
        }

        /// <summary>
        /// グリッドセルクリックイベント
        /// </summary>
        /// <remarks>
        /// リマークセルクリック時、リマーク詳細ダイアログ表示します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void grdControlResult_ClickCell(object sender, ClickCellEventArgs e)
        {
            // リマークセルクリック時、リマーク詳細ダイアログ表示
            if (e.Cell.Column.Key == ControlResultData.DataKeys.Remark)
            {
                var controlResultData = ((ControlResultData)e.Cell.Row.ListObject);
                if (controlResultData.GetRemarkId() != null)
                {
                    DlgRemarkDetail.Show(controlResultData.GetRemarkId());
                }
                else
                {
                    DlgRemarkDetail.Show(controlResultData.GetReplicationRemarkId());
                }
            }
        }

        /// <summary>
        /// FormClosedイベント
        /// </summary>
        /// <remarks>
        /// UI設定保存します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormControlResult_FormClosed(object sender, FormClosedEventArgs e)
        {
            // UI設定保存
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.ControlResultSettings.GridZoom = this.zoomPanel.Zoom;
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.ControlResultSettings.GridColOrder = this.grdControlResult.GetGridColumnOrder();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.ControlResultSettings.GridColWidth = this.grdControlResult.GetGridColmnWidth();
        }

        /// <summary>
        /// システムステータス変化イベント
        /// </summary>
        /// <remarks>
        /// システムステータス変化により検索・再計算パネルを表示・非表示にします
        /// </remarks>
        /// <param name="value"></param>
        private void onSystemStatusChanged(Object value)
        {
            switch (Singleton<SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId(Singleton<PublicMemory>.Instance.moduleIndex)])
            {
                case SystemStatusKind.ReagentExchange:
                    // 検索・再計算ボタンを非活性
                    btnFilter.Enabled = false;
                    btnRecalc.Enabled = false;
                    // 検索・再計算パネルを非表示にする
                    this.DispSearchInfoPanel = false;
                    this.DispRecalcInfoPanel = false;
                    break;
                default:
                    // 検索・再計算ボタン活性化
                    btnFilter.Enabled = true;
                    btnRecalc.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// セルデータエラーイベント
        /// </summary>
        /// <remarks>
        /// 編集モードにとどまる設定にします
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void grdControlResult_CellDataError(object sender, CellDataErrorEventArgs e)
        {
            e.StayInEditMode = true;
        }

        /// <summary>
        /// 印刷パラメータ変更時処理
        /// </summary>
        /// <remarks>
        /// 印刷ボタン表示設定します
        /// </remarks>
        /// <param name="value"></param>
        private void onPrintParamChanged(Object value)
        {
            // 印刷ボタン表示設定
            this.tlbCommandBar.Tools[PRINT].SharedProps.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable;
        }

        /// <summary>
        /// ホストパラメータ変更時処理
        /// </summary>
        /// <remarks>
        /// オンライン出力ボタン表示設定します
        /// </remarks>
        /// <param name="value"></param>
        private void onHostParamChanged(Object value)
        {
            // オンライン出力ボタン表示設定
            this.tlbCommandBar.Tools[TRANSMIT].SharedProps.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Enable;
        }

        #endregion

        #region ページめくり処理

        /// <summary>
        /// ページめくりが必要かどうかを判断する
        /// </summary>
        /// <param name="rowCounts"></param>
        /// <returns></returns>
        private bool IsAllowPageBreaks(int rowCounts)
        {
            if (rowCounts > this.pageSize)
            {
                this.pageCount = rowCounts % this.pageSize > 0 ? rowCounts / this.pageSize + 1 : rowCounts / this.pageSize;
                this.pageCurrent = 1;
                this.btnPre.Enabled = false;
                this.btnNext.Enabled = true;
                this.teCurrent.MaxValue = this.pageCount;
                this.teCurrent.Value = this.pageCurrent;
                this.teTotal.Value = this.pageCount;
                return true;
            }
            else
            {
                this.btnPre.Enabled = false;
                this.btnNext.Enabled = false;
                this.teCurrent.Value = 1;
                this.teCurrent.MaxValue = 1;
                this.teTotal.Value = 1;
                return false;
            }
        }

        /// <summary>
        /// 戻るボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPre_Click(object sender, EventArgs e)
        {
            if (this.pageCurrent > 1)
            {
                this.pageCurrent--;
                this.teCurrent.Value = this.pageCurrent;
                this.btnNext.Enabled = true;

                if (this.pageCurrent == 1)
                    this.btnPre.Enabled = false;
            }
        }

        /// <summary>
        /// 次へボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (this.pageCurrent < this.pageCount)
            {
                this.pageCurrent++;
                this.teCurrent.Value = this.pageCurrent;
                this.btnPre.Enabled = true;

                if (this.pageCurrent == this.pageCount)
                    this.btnNext.Enabled = false;
            }
        }

        private void teCurrent_ValueChanged(object sender, EventArgs e)
        {
            int iPageIndex = 1;
            if (!int.TryParse(this.teCurrent.Value.ToString(), out iPageIndex))
                return;

            this.pageCurrent = iPageIndex;
            PageTurning();
        }

        /// <summary>
        /// ページめくり
        /// </summary>
        private void PageTurning()
        {
            if (this.currentControlResultInfo == null || this.currentControlResultInfo.Count == 0)
                return;

            if ((this.pageCurrent > 1 && this.pageCurrent < this.pageCount) || this.pageCurrent == this.pageCount)
            {
                int iSkipNum = (this.pageCurrent - 1) * this.pageSize;
                this.grdControlResult.DataSource = new BindingList<ControlResultData>(this.currentControlResultInfo.Skip(iSkipNum).Take(this.pageSize).ToList());
                this.btnPre.Enabled = true;
                if (this.pageCurrent == this.pageCount)
                    this.btnNext.Enabled = false;
                else
                    this.btnNext.Enabled = true;
                return;
            }

            if (this.pageCurrent == 1)
            {
                this.grdControlResult.DataSource = new BindingList<ControlResultData>(this.currentControlResultInfo.Take(this.pageSize).ToList());
                this.btnPre.Enabled = false;
                this.btnNext.Enabled = true;
            }

        }

        #endregion
    }
}
