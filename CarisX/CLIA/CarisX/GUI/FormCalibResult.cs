using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.CarisX.DB;
using Oelco.Common.Utility;
using Oelco.CarisX.GUI.Controls;
using Infragistics.Win.UltraWinGrid;
using Oelco.Common.DB;
using Oelco.CarisX.Print;
using Oelco.CarisX.Const;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Utility;
using Oelco.Common.Const;
using Oelco.CarisX.Status;
using Oelco.CarisX.Common;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// キャリブレータ測定データ画面クラス
    /// </summary>
    public partial class FormCalibResult : FormChildBase, IFormMeasureResult
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
        /// キャリブレータ範囲変数
        /// </summary>
        private int pageCurrent = 1, pageCount = 1, pageSize = 10000;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 現在のキャリブレータ測定結果情報
        /// </summary>
        private List<CalibratorResultData> currentCalibResultInfo = new List<CalibratorResultData>();

        /// <summary>
        /// 現在のキャリブレータ測定結果情報(バインド用)
        /// </summary>
        private BindingList<CalibratorResultData> bindCurrentCalibResultInfo;

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
        public FormCalibResult()
        {
            InitializeComponent();


            // 拡大率切替コントロール初期化
            this.zoomPanel.ZoomStep = CarisXConst.GRID_ZOOM_STEP;

            // 拡大率変更イベント登録
            this.zoomPanel.SetZoom += this.grdCalibResult.SetGridZoom;

            // コマンドバーのイベント追加
            this.tlbCommandBar.Tools[DELETE].ToolClick += (sender, e) => this.deleteData();
            this.tlbCommandBar.Tools[EXPORT].ToolClick += (sender, e) => this.exportData();
            this.tlbCommandBar.Tools[PRINT].ToolClick += (sender, e) => this.printData();

            // リアルタイムデータ更新イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.RealtimeData, this.onRealTimeDataChanged);
            // システムステータス変更イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SystemStatusChanged, this.onSystemStatusChanged);
            // 印刷パラメータ変更イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UseOfPrint, this.onPrintParamChanged);
            // ユーザレベル変更時イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UserLevelChanged, this.setUser);

            this.grdCalibResult.SetGridRowBackgroundColorRuleFromCellData(
                new[] { CalibratorResultData.DataKeys.SequenceNo, CalibratorResultData.DataKeys.Analytes }.ToList(),
                new[] { CarisXConst.GRID_ROWS_DEFAULT_COLOR, CarisXConst.GRID_ROWS_COLOR_PATTERN1 }.ToList());

            //设置ToolBar的右键功能不可用
            this.tlbCommandBar.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.tlbCommandBar_BeforeToolbarListDropdown);

            gridColmunDic = new Dictionary<String, String>()
                {
                    {CalibratorResultData.DataKeys.SequenceNo, Oelco.CarisX.Properties.Resources.STRING_CALIBRESULT_000},
                    {CalibratorResultData.DataKeys.CalibLotNo, Oelco.CarisX.Properties.Resources.STRING_CALIBRESULT_001},
                    {CalibratorResultData.DataKeys.RackId, Oelco.CarisX.Properties.Resources.STRING_CALIBRESULT_002},
                    {CalibratorResultData.DataKeys.RackPosition, Oelco.CarisX.Properties.Resources.STRING_CALIBRESULT_003},
                    {CalibratorResultData.DataKeys.Analytes, Oelco.CarisX.Properties.Resources.STRING_CALIBRESULT_004},
                    {CalibratorResultData.DataKeys.Count, Oelco.CarisX.Properties.Resources.STRING_CALIBRESULT_005},
                    {CalibratorResultData.DataKeys.Concentration, Oelco.CarisX.Properties.Resources.STRING_CALIBRESULT_006},
                    {CalibratorResultData.DataKeys.ReplicationNo, Oelco.CarisX.Properties.Resources.STRING_CALIBRESULT_007},
                    {CalibratorResultData.DataKeys.CountAve, Oelco.CarisX.Properties.Resources.STRING_CALIBRESULT_008},
                    {CalibratorResultData.DataKeys.ConcentrationAve, Oelco.CarisX.Properties.Resources.STRING_CALIBRESULT_009},
                    {CalibratorResultData.DataKeys.MeasureDateTime, Oelco.CarisX.Properties.Resources.STRING_CALIBRESULT_010},
                    {CalibratorResultData.DataKeys.ReagentLotNo, Oelco.CarisX.Properties.Resources.STRING_CALIBRESULT_012},
                    {CalibratorResultData.DataKeys.PretriggerLotNo, Oelco.CarisX.Properties.Resources.STRING_CALIBRESULT_013},
                    {CalibratorResultData.DataKeys.TriggerLotNo, Oelco.CarisX.Properties.Resources.STRING_CALIBRESULT_014},
                    {CalibratorResultData.DataKeys.Remark, Oelco.CarisX.Properties.Resources.STRING_CALIBRESULT_011},
                    {CalibratorResultData.DataKeys.ErrorCode, Oelco.CarisX.Properties.Resources.STRING_CALIBRESULT_019},
                    {CalibratorResultData.DataKeys.DarkCount, Properties.Resources.STRING_CALIBRESULT_020},
                    {CalibratorResultData.DataKeys.BGCount, Properties.Resources.STRING_CALIBRESULT_021},
                    {CalibratorResultData.DataKeys.ResultCount, Properties.Resources.STRING_CALIBRESULT_022},
                    {CalibratorResultData.DataKeys.ModuleNo, Properties.Resources.STRING_CALIBRESULT_023},
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
        /// キャリブレーション結果グリッドの取得
        /// </summary>
        public CustomGrid Grid
        {
            get
            {
                return this.grdCalibResult;
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
                    rtnVal.Remove(CalibratorResultData.DataKeys.DarkCount);
                    rtnVal.Remove(CalibratorResultData.DataKeys.BGCount);
                    rtnVal.Remove(CalibratorResultData.DataKeys.ResultCount);
                }

                return rtnVal;
            }
        }

        #endregion

        #region [publicメソッド]


        #region _Realtime印刷_

        CalibResultPrint prt = new CalibResultPrint();
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
            List<Tuple<int, int, int>> dataForCount = Singleton<InProcessSampleInfoManager>.Instance.AskRealTimePrintQueue(RealtimePrintDataType.Calibrator);
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
                    dataForPrint = Singleton<InProcessSampleInfoManager>.Instance.PopRalTimePrintQueueData(RealtimePrintDataType.Calibrator, dataForCount.Count - 1);
                }
            }
            else
            {
                dataForPrint = Singleton<InProcessSampleInfoManager>.Instance.PopRalTimePrintQueueData(RealtimePrintDataType.Calibrator);
            }

            if (dataForPrint != null && dataForPrint.Count != 0)
            {
                // データを生成する
                var searchDic = (from dbData in Singleton<CalibratorResultDB>.Instance.GetData() select new OutPutCalibratorResultData(dbData))
                    .ToDictionary((v) => new Tuple<Int32, Int32>(v.GetUniqueNo(), v.ReplicationNo), (v) => v);
                var outputData = from v in dataForPrint
                                 let key = new Tuple<int, int>(v.Item2, v.Item3)
                                 where searchDic.ContainsKey(key)
                                 select searchDic[key];
                var printData = this.convertPrintData(outputData);

                // 印刷する
                Boolean ret = prt.Print(printData, Singleton<InProcessSampleInfoManager>.Instance.GetNextRealtimePrintPageNumber(RealtimePrintDataType.Calibrator));

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
        private List<CalibResultReportData> convertPrintData(IEnumerable<OutPutCalibratorResultData> resultData)
        {
            List<CalibResultReportData> rptData = new List<CalibResultReportData>();

            foreach (var row in resultData)
            {
                CalibResultReportData rptDataRow = new CalibResultReportData();
                // データ格納元確認
                rptDataRow.SeqNo = row.SequenceNo.ToString();
                rptDataRow.CalibratorLot = row.CalibLotNo;
                rptDataRow.ReagentLot = row.ReagentLotNo;
                rptDataRow.PreTriggerLotNo = row.PretriggerLotNo;
                rptDataRow.TriggerLotNo = row.TriggerLotNo;
                rptDataRow.RackID = row.RackId.ToString();
                rptDataRow.RackPosition = row.RackPosition;
                rptDataRow.ProtoName = row.Analytes;
                rptDataRow.Count = row.Count;
                rptDataRow.CountAvg = row.CountAve;
                rptDataRow.Conc = row.Concentration;
                rptDataRow.ConcAvg = row.ConcentrationAve;
                rptDataRow.MultiMeas = row.ReplicationNo;
                rptDataRow.MeasTime = row.MeasureDateTime.ToString();
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
        /// データ変更リアルタイム更新します
        /// </remarks>
        /// <param name="kind">種別</param>
        protected void onRealTimeDataChanged(object kind)
        {
            // キャリブレータ測定データ
            if (((RealtimeDataKind)kind) == RealtimeDataKind.CalibResult)
            {
                if (this.updateData == null)
                {
                    this.updateData = (obj, e) =>
                    {
                        // キャリブレータ測定データ画面 分析情報更新
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
            this.gesturePanel.ScrollProxy = this.grdCalibResult.ScrollProxy;

            // 拡大率切替コントロール初期化
            zoomPanel.Zoom = Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.CalibResultSettings.GridZoom;
            // グリッド表示順
            this.grdCalibResult.SetGridColumnOrder(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.CalibResultSettings.GridColOrder);
            // グリッド列幅
            this.grdCalibResult.SetGridColmnWidth(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.CalibResultSettings.GridColWidth);

            // 印刷ボタン表示設定
            this.tlbCommandBar.Tools[PRINT].SharedProps.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable;

            //this.grdCalibResult.DisplayLayout.Bands[0].Columns[CalibratorResultData.DataKeys.Count].NullText = CarisXConst.COUNT_CONCENTRATION_NOTHING;
            //this.grdCalibResult.DisplayLayout.Bands[0].Columns[CalibratorResultData.DataKeys.Concentration].NullText = CarisXConst.COUNT_CONCENTRATION_NOTHING;
            //this.grdCalibResult.DisplayLayout.Bands[0].Columns[CalibratorResultData.DataKeys.CountAve].NullText = CarisXConst.COUNT_CONCENTRATION_NOTHING;
            //this.grdCalibResult.DisplayLayout.Bands[0].Columns[CalibratorResultData.DataKeys.ConcentrationAve].NullText = CarisXConst.COUNT_CONCENTRATION_NOTHING;
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            this.Text = Properties.Resources.STRING_CALIBRESULT_016;

            // ページめくりボタン
            this.btnPre.Text = Properties.Resources.STRING_CALIBRESULT_024;
            this.btnNext.Text = Properties.Resources.STRING_CALIBRESULT_025;

            // コマンドバーアイテム名設定
            this.tlbCommandBar.Tools[DELETE].SharedProps.Caption = Properties.Resources.STRING_COMMANDBARITEM_002;
            this.tlbCommandBar.Tools[EXPORT].SharedProps.Caption = Properties.Resources.STRING_COMMANDBARITEM_010;
            this.tlbCommandBar.Tools[PRINT].SharedProps.Caption = Properties.Resources.STRING_COMMANDBARITEM_004;

            // グリッドカラムヘッダー表示設定
            foreach (var gridColmun in gridColmunDic)
            {
                this.grdCalibResult.DisplayLayout.Bands[0].Columns[gridColmun.Key].Header.Caption = gridColmun.Value;
            }

            if (this.grdCalibResult.DisplayLayout.Bands[0].Columns.Count != 0)
            {
                //起動時はまだ列の定義が存在しない為、分岐させておく
                this.grdCalibResult.DisplayLayout.Bands[0].Columns[CalibratorResultData.DataKeys.DarkCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
                this.grdCalibResult.DisplayLayout.Bands[0].Columns[CalibratorResultData.DataKeys.BGCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
                this.grdCalibResult.DisplayLayout.Bands[0].Columns[CalibratorResultData.DataKeys.ResultCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
            }

            // ボタン
            this.btnFilter.Text = Properties.Resources.STRING_CALIBRESULT_018;
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

            //add by marxsu 数据修改权限的设置
            this.searchInfoPanelCalibratorResult.RemarkDataEditedEnable = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.SetRemarkDataEditedEnable); ;

            if (this.grdCalibResult.DisplayLayout.Bands[0].Columns.Count != 0)
            {
                //起動時はまだ列の定義が存在しない為、分岐させておく
                this.grdCalibResult.DisplayLayout.Bands[0].Columns[CalibratorResultData.DataKeys.DarkCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
                this.grdCalibResult.DisplayLayout.Bands[0].Columns[CalibratorResultData.DataKeys.BGCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
                this.grdCalibResult.DisplayLayout.Bands[0].Columns[CalibratorResultData.DataKeys.ResultCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
            }
        }

        #endregion

        #region [privateメソッド]

        #region _コマンドバー_

        /// <summary>
        /// 削除
        /// </summary>
        /// <remarks>
        /// 操作履歴に削除実行を登録し、キャリブレータ測定結果データの削除実行して反映を行います
        /// </remarks>
        private void deleteData()
        {
            // 操作履歴：削除実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_003 });

            var selectRows = this.grdCalibResult.SearchSelectRow().Select((row) => ((CalibratorResultData)row.ListObject).GetUniqueNo()).Distinct();
            var selectRowDatas = this.grdCalibResult.Rows.Where((row) => selectRows.Contains(((CalibratorResultData)row.ListObject).GetUniqueNo())).Select((row) => (CalibratorResultData)row.ListObject).ToList();

            // 削除対象データ
            var deleteDatas = selectRowDatas.Where((data) => !Singleton<InProcessSampleInfoManager>.Instance.InProcessSampleList.Exists((info) => info.GetUniqueNumbers().Contains(data.GetUniqueNo())));

            if (DialogResult.OK == DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_038, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel))
            {
                deleteDatas.DeleteAllDataList();

                // 変更を反映
                Singleton<CalibratorResultDB>.Instance.SetData(deleteDatas.ToList());
                Singleton<CalibratorResultDB>.Instance.CommitData();
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
        /// 印刷
        /// </summary>
        /// <remarks>
        /// 操作履歴に印刷実行を登録し、キャリブレータ測定結果データの印刷を行います
        /// </remarks>
        private void printData()
        {
            // 操作履歴登録：印刷実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_005 });

            TargetRange outputRange = DlgTargetSelectRange.Show();

            List<OutPutCalibratorResultData> printData = new List<OutPutCalibratorResultData>();
            switch (outputRange)
            {
                case TargetRange.All:
                    for (int i = 0; i < this.grdCalibResult.Rows.Count; i++)
                    {
                        OutPutCalibratorResultData item = new OutPutCalibratorResultData(this.grdCalibResult.Rows[i].ListObject as DataRowWrapperBase);
                        printData.Add(item);
                    }
                    printData.RollBackDataList();
                    break;
                case TargetRange.Specification:
                    List<UltraGridRow> searchRows = this.grdCalibResult.SearchSelectRow();
                    for (int i = 0; i < searchRows.Count; i++)
                    {
                        OutPutCalibratorResultData item = new OutPutCalibratorResultData(searchRows[i].ListObject as DataRowWrapperBase);
                        printData.Add(item);
                    }
                    printData.RollBackDataList();
                    break;
                case TargetRange.None:
                    // 操作履歴登録：印刷キャンセル
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_006 });
                    return;
            }

            if (printData.Count == 0)
            {
                // 印刷するデータがありません。
                DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_064, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_003, MessageDialogButtons.Confirm);
                return;
            }

            // 印刷の実装
            // 印刷用Listに取得データを格納
            List<CalibResultReportData> rptData = this.convertPrintData(printData);

            CalibResultPrint prt = new CalibResultPrint();
            Boolean ret = prt.Print(rptData);

        }

        /// <summary>
        /// ファイル出力
        /// </summary>
        /// <remarks>
        /// 操作履歴にファイル出力実行を登録し、キャリブレータ測定結果データのファイル保存を行います
        /// </remarks>
        private void exportData()
        {
            // 操作履歴登録：ファイル出力実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_007 });

            TargetRange outputRange = DlgTargetSelectRange.Show();

            // ファイル出力対象を取得
            List<OutPutCalibratorResultData> exportData = new List<OutPutCalibratorResultData>();
            switch (outputRange)
            {
                case TargetRange.All:
                    for (int i = 0; i < this.grdCalibResult.Rows.Count; i++)
                    {
                        OutPutCalibratorResultData item = new OutPutCalibratorResultData(this.grdCalibResult.Rows[i].ListObject as DataRowWrapperBase);
                        exportData.Add(item);
                    }
                    exportData.RollBackDataList();
                    break;
                case TargetRange.Specification:
                    List<UltraGridRow> searchRows = this.grdCalibResult.SearchSelectRow();
                    for (int i = 0; i < searchRows.Count; i++)
                    {
                        OutPutCalibratorResultData item = new OutPutCalibratorResultData(searchRows[i].ListObject as DataRowWrapperBase);
                        exportData.Add(item);
                    }
                    exportData.RollBackDataList();
                    break;
                case TargetRange.None:
                    // 操作履歴登録：ファイル出力キャンセル
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_008 });
                    return;
            }

            if (exportData.Count == 0)
            {
                // UNDONE:ファイル出力するデータがありません。
                DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_169, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.Confirm);
                return;
            }

            // ユーザー指定出力先へ
            String fileName;
            if (CarisXSubFunction.ShowSaveCSVFileDialog(out fileName, OutputFileKind.CSV, CarisXConst.EXPORT_CSV_CALIBRATORRESULT, CarisX.Properties.Resources.STRING_CALIBRESULT_017, Singleton<CarisXUISettingManager>.Instance.CalibResultSettings) == System.Windows.Forms.DialogResult.OK)
            {
                Singleton<DataHelper>.Instance.ExportCsv(exportData, this.ResultGridColumns, fileName);
            }

        }

        #endregion

        /// <summary>
        /// フォーム読み込み時のイベント
        /// </summary>
        /// <remarks>
        /// データ読込を行います
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void FormCalibResult_Load(object sender, EventArgs e)
        {
            this.searchInfoPanelCalibratorResult.Visible = false;
            this.loadData();
        }

        /// <summary>
        /// 絞込みボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// キャリブレータ測定結果の絞り込みパネルの表示を行います
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnFilter_Click(object sender, EventArgs e)
        {
            this.searchInfoPanelCalibratorResult.Visible = true;
        }

        #region _絞り込みパネルイベント_

        /// <summary>
        /// 絞り込みパネルOKボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// キャリブレータ測定結果の絞込みを実行します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void searchInfoPanelCalibratorResult_OkClick(object sender, EventArgs e)
        {
            var searchDatas = Singleton<CalibratorResultDB>.Instance.GetData(this.searchInfoPanelCalibratorResult);
            // 修改内容：ページめくり処理追加
            if (IsAllowPageBreaks(this.currentCalibResultInfo.Count))
                this.grdCalibResult.DataSource = new BindingList<CalibratorResultData>(searchDatas.Take(this.pageSize).ToList());
            else
                this.grdCalibResult.DataSource = new BindingList<CalibratorResultData>(searchDatas);

            //this.grdCalibResult.DataSource = new BindingList<CalibratorResultData>(searchDatas);
            this.btnFilter.Appearance.ImageBackground = CarisX.Properties.Resources.Image_SelectButton_selected;
        }

        /// <summary>
        /// 絞り込みパネルキャンセルボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// キャリブレータ測定結果の絞込みの実行をキャンセルします
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void searchInfoPanelCalibratorResult_CancelClick(object sender, EventArgs e)
        {
            this.bindCurrentCalibResultInfo = null;
            this.loadData();
            this.btnFilter.Appearance.ImageBackground = CarisX.Properties.Resources.Image_SelectButton;
        }

        /// <summary>
        /// 絞り込みパネル閉じるボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// キャリブレータ測定結果の絞り込みパネルを非表示します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void searchInfoPanelCalibratorResult_CloseClick(object sender, EventArgs e)
        {
            this.searchInfoPanelCalibratorResult.Visible = false;
        }

        #endregion

        /// <summary>
        /// データの読み込み
        /// </summary>
        /// <remarks>
        /// キャリブレータ測定結果を読込します
        /// </remarks>
        private void loadData()
        {
            var list = this.grdCalibResult.DataSource as BindingList<CalibratorResultData>;
            if (this.bindCurrentCalibResultInfo == null || list == null)
            {
                this.currentCalibResultInfo = Singleton<CalibratorResultDB>.Instance.GetData();
                this.bindCurrentCalibResultInfo = new BindingList<CalibratorResultData>(this.currentCalibResultInfo);
                // 修改内容：ページめくり処理追加
                //this.grdCalibResult.DataSource = this.bindCurrentCalibResultInfo;

                if (IsAllowPageBreaks(this.bindCurrentCalibResultInfo.Count))
                    this.grdCalibResult.DataSource = new BindingList<CalibratorResultData>(this.currentCalibResultInfo.Take(this.pageSize).ToList());
                else
                    this.grdCalibResult.DataSource = this.bindCurrentCalibResultInfo;
            }
            else
            {
                list.RaiseListChangedEvents = false;
                list.Clear();
                // 修改内容：ページめくり処理追加
                this.currentCalibResultInfo = Singleton<CalibratorResultDB>.Instance.GetData();
                //if (list == this.bindCurrentCalibResultInfo)
                //{
                //    Singleton<CalibratorResultDB>.Instance.GetData().ForEach((data) => list.Add(data));
                //}
                //else
                //{
                //    Singleton<CalibratorResultDB>.Instance.GetData(this.searchInfoPanelCalibratorResult).ForEach((data) => list.Add(data));
                //}
                int iPageIndex = this.pageCurrent;
                if (IsAllowPageBreaks(this.currentCalibResultInfo.Count))
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
                        this.currentCalibResultInfo.Skip(iSkipNum).Take(this.pageSize).ToList().ForEach((data) => list.Add(data));
                    }
                    if (this.pageCurrent == 1)
                    {
                        this.currentCalibResultInfo.Take(this.pageSize).ToList().ForEach((data) => list.Add(data));
                    }
                }
                else
                {
                    this.currentCalibResultInfo.Take(this.pageSize).ToList().ForEach((data) => list.Add(data));
                }

                list.RaiseListChangedEvents = true;
                list.ResetBindings();
            }
        }

        /// <summary>
        /// グリッドセルクリックイベント
        /// </summary>
        /// <remarks>
        /// リマークセルのクリック時リマーク詳細画面表示します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void grdCalibResult_ClickCell(object sender, Infragistics.Win.UltraWinGrid.ClickCellEventArgs e)
        {
            // リマークセルのクリック時のダイアログ表示
            if (e.Cell.Column.Key == CalibratorResultData.DataKeys.Remark)
            {
                var calibrationResultData = ((CalibratorResultData)e.Cell.Row.ListObject);
                if (calibrationResultData.GetRemarkId() != null)
                {
                    DlgRemarkDetail.Show(calibrationResultData.GetRemarkId());
                }
                else
                {
                    DlgRemarkDetail.Show(calibrationResultData.GetReplicationRemarkId());
                }
            }
        }

        /// <summary>
        /// FormClosedイベント
        /// </summary>
        /// <remarks>
        /// UI設定保存します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void FormCalibResult_FormClosed(object sender, FormClosedEventArgs e)
        {
            // UI設定保存
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.CalibResultSettings.GridZoom = this.zoomPanel.Zoom;
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.CalibResultSettings.GridColOrder = this.grdCalibResult.GetGridColumnOrder();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.CalibResultSettings.GridColWidth = this.grdCalibResult.GetGridColmnWidth();
        }

        /// <summary>
        /// システムステータス変化イベント
        /// </summary>
        /// <remarks>
        /// システムステータス変化により検索ボタン、パネルの状態を変更します
        /// </remarks>
        /// <param name="value"></param>
        private void onSystemStatusChanged(Object value)
        {
            switch (Singleton<SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId(Singleton<PublicMemory>.Instance.moduleIndex)])
            {
                case SystemStatusKind.ReagentExchange:
                    // 検索ボタンを非活性    
                    btnFilter.Enabled = false;
                    // 検索パネルを非表示にする
                    this.searchInfoPanelCalibratorResult.Visible = false;
                    break;
                default:
                    // 検索ボタン活性化
                    btnFilter.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// 印刷パラメータ変更時処理
        /// </summary>
        /// <remarks>
        /// 印刷ボタン表示設定を変更します
        /// </remarks>
        /// <param name="value"></param>
        private void onPrintParamChanged(Object value)
        {
            // 印刷ボタン表示設定
            this.tlbCommandBar.Tools[PRINT].SharedProps.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable;
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
            if (this.currentCalibResultInfo == null || this.currentCalibResultInfo.Count == 0)
                return;

            if ((this.pageCurrent > 1 && this.pageCurrent < this.pageCount) || this.pageCurrent == this.pageCount)
            {
                int iSkipNum = (this.pageCurrent - 1) * this.pageSize;
                this.grdCalibResult.DataSource = new BindingList<CalibratorResultData>(this.currentCalibResultInfo.Skip(iSkipNum).Take(this.pageSize).ToList());
                this.btnPre.Enabled = true;
                if (this.pageCurrent == this.pageCount)
                    this.btnNext.Enabled = false;
                else
                    this.btnNext.Enabled = true;
                return;
            }

            if (this.pageCurrent == 1)
            {
                this.grdCalibResult.DataSource = new BindingList<CalibratorResultData>(this.currentCalibResultInfo.Take(this.pageSize).ToList());
                this.btnPre.Enabled = false;
                this.btnNext.Enabled = true;
            }

        }

        #endregion
    }
}
