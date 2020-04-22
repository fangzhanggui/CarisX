using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Oelco.CarisX.Calculator;
using Oelco.CarisX.DB;
using Oelco.CarisX.GUI.Controls;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Utility;
using Oelco.Common.GUI;
using Oelco.Common.Utility;
using Oelco.Common.DB;
using Oelco.CarisX.Const;
using Oelco.CarisX.Print;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.Common.Parameter;
using Oelco.Common.Const;
using Oelco.CarisX.Status;
using Oelco.CarisX.Common;
using System.Threading;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 検体測定データ画面クラス
    /// </summary>
    public partial class FormSpecimenResult : FormChildBase, IFormMeasureResult
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
        /// 手希釈倍率保存
        /// </summary>
        public const String SAVE_MANUAL_DILUTION = "Save manual dilution";

        /// <summary>
        /// 再検査に追加
        /// </summary>
        public const String RETEST = "Re-test";

        /// <summary>
        /// オンライン出力
        /// </summary>
        public const String TRANSMIT = "Transmit";

        /// <summary>
        /// スペシメン範囲変数
        /// </summary>
        private int pageCurrent = 1, pageCount = 1, pageSize = 10000;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 現在の検体測定結果情報
        /// </summary>
        private List<SpecimenResultData> currentSampleResultInfo = new List<SpecimenResultData>();

        /// <summary>
        /// 現在の検体測定結果情報(バインド用)
        /// </summary>
        private BindingList<SpecimenResultData> bindCurrentSampleResultInfo;

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
        public FormSpecimenResult()
        {
            InitializeComponent();

            // 拡大率切替コントロール初期化
            this.zoomPanel.ZoomStep = CarisXConst.GRID_ZOOM_STEP;

            // 拡大率変更イベント登録
            this.zoomPanel.SetZoom += this.grdSpecimenResult.SetGridZoom;

            // コマンドバーのイベント追加
            this.tlbCommandBar.Tools[DELETE].ToolClick += (sender, e) => this.deleteData();
            this.tlbCommandBar.Tools[EXPORT].ToolClick += (sender, e) => this.exportData();
            this.tlbCommandBar.Tools[PRINT].ToolClick += (sender, e) => this.printData();
            this.tlbCommandBar.Tools[SAVE_MANUAL_DILUTION].ToolClick += (sender, e) => this.saveMenuDilutionData();
            //this.tlbCommandBar.Tools[RETEST].ToolClick += ( sender, e ) => this.reTest();
            this.tlbCommandBar.Tools[TRANSMIT].ToolClick += (sender, e) => this.transmit();

            // リアルタイムデータ更新イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.RealtimeData, this.onRealTimeDataChanged);
            // システムステータス変更イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SystemStatusChanged, this.onSystemsStatusChanged);
            // 印刷パラメータ変更イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UseOfPrint, this.onPrintParamChanged);
            // ホスト有無パラメータ変更イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UseOfHost, this.onHostParamChanged);
            // ユーザレベル変更イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UserLevelChanged, this.setUser);

            // ホストへのデータ送信完了イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SendSpecimenDataHostComplete, this.sendHostComplete);
            // 
            //Singleton<NotifyManager>.Instance.AddNotifyTarget( (Int32)NotifyKind.HostWorkSheetBatch, this.workSheetFromHostByBatch );
            //Singleton<NotifyManager>.Instance.AddNotifyTarget( (Int32)NotifyKind.AskBatchComplete, this.askBatchComplete );

            this.grdSpecimenResult.SetGridRowBackgroundColorRuleFromRowData(
                (rowData1, rowData2) =>
                {
                    if (((SpecimenResultData)rowData1).GetUniqueNo() > ((SpecimenResultData)rowData2).GetUniqueNo())
                    {
                        return 1;
                    }
                    else if (((SpecimenResultData)rowData1).GetUniqueNo() < ((SpecimenResultData)rowData2).GetUniqueNo())
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                },
                new[] { CarisXConst.GRID_ROWS_DEFAULT_COLOR, CarisXConst.GRID_ROWS_COLOR_PATTERN1 }.ToList());

            // 分析項目手希釈編集不可
            Action rowEditChange = () =>
            {
                foreach (var row in this.grdSpecimenResult.Rows)
                {
                    if (!Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(((SpecimenResultData)row.ListObject).GetMeasureProtocolIndex()).UseManualDil)
                    {
                        row.Cells[SpecimenResultData.DataKeys.ManualDilution].Activation = Activation.NoEdit;
                        row.Cells[SpecimenResultData.DataKeys.ManualDilution].Appearance.BackColor = row.Appearance.BackColor;
                    }
                }
            };

            this.grdSpecimenResult.InitializeLayout += (sender, e) =>
            {
                rowEditChange();
                ((BindingList<SpecimenResultData>)this.grdSpecimenResult.DataSource).ListChanged += (sndr, ed) => rowEditChange();
            };

            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ChangeProtocolSetting, (obj) => this.loadData());

            //设置ToolBar的右键功能不可用
            this.tlbCommandBar.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.tlbCommandBar_BeforeToolbarListDropdown);

            gridColmunDic = new Dictionary<String, String>()
                {
                    {SpecimenResultData.DataKeys.SequenceNo, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_000},
                    {SpecimenResultData.DataKeys.RackId, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_001},
                    {SpecimenResultData.DataKeys.RackPosition, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_002},
                    {SpecimenResultData.DataKeys.PatientId, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_003},
                    {SpecimenResultData.DataKeys.ReplicationNo, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_004},
                    {SpecimenResultData.DataKeys.Analytes,Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_005},
                    {SpecimenResultData.DataKeys.SpecimenMaterialType, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_006},
                    {SpecimenResultData.DataKeys.ManualDilution, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_007},
                    {SpecimenResultData.DataKeys.AutoDilution, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_008},
                    {SpecimenResultData.DataKeys.Count, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_009},
                    {SpecimenResultData.DataKeys.Concentration, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_010},
                    {SpecimenResultData.DataKeys.CountAve, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_012},
                    {SpecimenResultData.DataKeys.ConcentrationAve ,Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_013},
                    {SpecimenResultData.DataKeys.Remark, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_014},
                    {SpecimenResultData.DataKeys.Judgement, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_015},
                    {SpecimenResultData.DataKeys.ReagentLotNo, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_016},
                    {SpecimenResultData.DataKeys.PretriggerLotNo, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_017},
                    {SpecimenResultData.DataKeys.TriggerLotNo ,Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_018},
                    {SpecimenResultData.DataKeys.CalibCurveDateTime, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_019},
                    {SpecimenResultData.DataKeys.MeasureDateTime, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_020},
                    {SpecimenResultData.DataKeys.Comment, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_021},
                    {SpecimenResultData.DataKeys.ErrorCode, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_026},
                    {SpecimenResultData.DataKeys.ReceiptNo, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_027},
                    {SpecimenResultData.DataKeys.AnalysisMode, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_031},
                    {SpecimenResultData.DataKeys.DarkCount, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_028},
                    {SpecimenResultData.DataKeys.BGCount, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_029},
                    {SpecimenResultData.DataKeys.ResultCount, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_030},
                    {SpecimenResultData.DataKeys.ModuleNo, Oelco.CarisX.Properties.Resources.STRING_SPECIMENRESULT_032},
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
                return this.searchInfoPanelSpecimenResult.Visible;
            }
            protected set
            {
                this.recalcInfoPanelSpecimenResult.Visible = false;
                this.searchInfoPanelSpecimenResult.Visible = value;
            }
        }

        /// <summary>
        /// 再計算パネルの表示状態の取得、設定
        /// </summary>
        public Boolean DispRecalcInfoPanel
        {
            get
            {
                return this.recalcInfoPanelSpecimenResult.Visible;
            }
            protected set
            {
                this.searchInfoPanelSpecimenResult.Visible = false;
                this.recalcInfoPanelSpecimenResult.Visible = value;
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
                    rtnVal.Remove(SpecimenResultData.DataKeys.DarkCount);
                    rtnVal.Remove(SpecimenResultData.DataKeys.BGCount);
                    rtnVal.Remove(SpecimenResultData.DataKeys.ResultCount);
                }

                return rtnVal;
            }
        }
        #endregion

        #region [publicメソッド]

        #region _Realtime印刷_

        SpecimenResultPrint prt = new SpecimenResultPrint();
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
            List<Tuple<int, int, int>> dataForCount = Singleton<InProcessSampleInfoManager>.Instance.AskRealTimePrintQueue(RealtimePrintDataType.Specimen);
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
                    dataForPrint = Singleton<InProcessSampleInfoManager>.Instance.PopRalTimePrintQueueData(RealtimePrintDataType.Specimen, dataForCount.Count - 1);
                }
            }
            else
            {
                dataForPrint = Singleton<InProcessSampleInfoManager>.Instance.PopRalTimePrintQueueData(RealtimePrintDataType.Specimen);
            }

            if (dataForPrint != null && dataForPrint.Count != 0)
            {
                // データを生成する
                var searchDic = (from dbData in Singleton<SpecimenResultDB>.Instance.GetSearchData()
                                 select new OutPutSpecimenResultData(dbData))
                    .ToDictionary((v) => new Tuple<Int32, Int32>(v.GetUniqueNo(), v.ReplicationNo), (v) => v);

                var outputData = from v in dataForPrint
                                 let key = new Tuple<int, int>(v.Item2, v.Item3)
                                 where searchDic.ContainsKey(key)
                                 select searchDic[key];
                var printData = this.convertPrintData(outputData);

                // 印刷する
                Boolean ret = prt.Print(printData, Singleton<InProcessSampleInfoManager>.Instance.GetNextRealtimePrintPageNumber(RealtimePrintDataType.Specimen));

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
        private List<SpecimenResultReportData> convertPrintData(IEnumerable<OutPutSpecimenResultData> resultData)
        {
            List<SpecimenResultReportData> rptData = new List<SpecimenResultReportData>();

            foreach (var row in resultData)
            {
                SpecimenResultReportData rptDataRow = new SpecimenResultReportData();
                rptDataRow.SeqNo = row.SequenceNo;
                rptDataRow.RackID = ((CarisXIDString)row.RackId).ToString() + " - " + row.RackPosition;
                rptDataRow.SpecimenType = row.SpecimenMaterialType;
                rptDataRow.SampleNo = row.GetIndividuallyNo();
                rptDataRow.SampleID = row.PatientId;
                rptDataRow.ProtoName = row.Analytes;
                rptDataRow.ManualDil = row.ManualDilution;
                rptDataRow.AutoDil = row.AutoDilution.ToString();
                rptDataRow.MultiMeas = row.ReplicationNo;
                rptDataRow.Count = row.Count;
                rptDataRow.CountAvg = row.CountAve;
                rptDataRow.Conc = row.Concentration;
                rptDataRow.ConcAvg = row.ConcentrationAve;
                rptDataRow.Conc = rptDataRow.Conc ?? String.Empty;
                rptDataRow.ConcAvg = rptDataRow.ConcAvg ?? String.Empty;
                rptDataRow.Judge = row.Judgement;
                rptDataRow.ReagentLotNo = row.ReagentLotNo;
                rptDataRow.PreTriggerLotNo = row.PretriggerLotNo;
                rptDataRow.TriggerLotNo = row.TriggerLotNo;
                rptDataRow.MeasTime = row.MeasureDateTime.ToString();
                rptDataRow.MeasDate = row.MeasureDateTime.ToString("yyyyMMdd");
                rptDataRow.Comment = row.Comment;
                rptDataRow.PrintDateTime = DateTime.Now.ToDispString();
                rptDataRow.ReceiptNo = row.ReceiptNo;

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

        /// <summary>
        /// フォーム表示
        /// </summary>
        /// <remarks>
        /// 画面をフェードイン表示します
        /// </remarks>
        /// <param name="captScreenRect"></param>
        public override void Show(Rectangle captScreenRect)
        {
            base.Show(captScreenRect);
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
            // 一般検体測定データ
            if (((RealtimeDataKind)kind) == RealtimeDataKind.SampleResult)
            {
                if (this.updateData == null)
                {
                    this.updateData = (obj, e) =>
                    {
                        // 一般検体測定データ画面 分析情報更新
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
            this.grdSpecimenResult.DisplayLayout.Bands[0].Columns[SpecimenResultData.DataKeys.ManualDilution].CellActivation = Activation.AllowEdit;
            this.grdSpecimenResult.DisplayLayout.Bands[0].Columns[SpecimenResultData.DataKeys.ManualDilution].CellAppearance.BackColor = CarisXConst.GRID_CELL_CAN_EDIT_COLOR;

            // スクロール処理設定
            this.gesturePanel.ScrollProxy = this.grdSpecimenResult.ScrollProxy;
            // 拡大率切替コントロール初期化
            this.zoomPanel.Zoom = Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenResultSettings.GridZoom;
            // グリッド表示順
            this.grdSpecimenResult.SetGridColumnOrder(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenResultSettings.GridColOrder);
            // グリッド列幅
            this.grdSpecimenResult.SetGridColmnWidth(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenResultSettings.GridColWidth);
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
            this.Text = Properties.Resources.STRING_SPECIMENRESULT_025;

            // ページめくりボタン
            this.btnPre.Text = Properties.Resources.STRING_SPECIMENRESULT_033;
            this.btnNext.Text = Properties.Resources.STRING_SPECIMENRESULT_034;

            // 絞込み、再計算パネル表示ボタン表示設定
            this.btnFilter.Text = Properties.Resources.STRING_SPECIMENRESULT_022;
            this.btnRecalc.Text = Properties.Resources.STRING_SPECIMENRESULT_023;

            // グリッドカラムヘッダー表示設定
            foreach (var gridColmun in gridColmunDic)
            {
                this.grdSpecimenResult.DisplayLayout.Bands[0].Columns[gridColmun.Key].Header.Caption = gridColmun.Value;
            }
            this.grdSpecimenResult.DisplayLayout.Bands[0].Columns[SpecimenResultData.DataKeys.ReceiptNo].CellActivation = Activation.NoEdit;

            this.grdSpecimenResult.DisplayLayout.Bands[0].Columns[SpecimenResultData.DataKeys.DarkCount].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
            this.grdSpecimenResult.DisplayLayout.Bands[0].Columns[SpecimenResultData.DataKeys.BGCount].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
            this.grdSpecimenResult.DisplayLayout.Bands[0].Columns[SpecimenResultData.DataKeys.ResultCount].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転

            // コマンドバー
            this.tlbCommandBar.Tools[DELETE].SharedProps.Caption = Properties.Resources.STRING_COMMANDBARITEM_002;
            this.tlbCommandBar.Tools[EXPORT].SharedProps.Caption = Properties.Resources.STRING_COMMANDBARITEM_010;
            this.tlbCommandBar.Tools[PRINT].SharedProps.Caption = Properties.Resources.STRING_COMMANDBARITEM_004;
            this.tlbCommandBar.Tools[SAVE_MANUAL_DILUTION].SharedProps.Caption = Properties.Resources.STRING_COMMANDBARITEM_018;
            this.tlbCommandBar.Tools[RETEST].SharedProps.Caption = Properties.Resources.STRING_COMMANDBARITEM_019;
            this.tlbCommandBar.Tools[TRANSMIT].SharedProps.Caption = Properties.Resources.STRING_COMMANDBARITEM_011;
        }

        /// <summary>
        /// ユーザレベル設定
        /// </summary>
        /// <remarks>
        /// ユーザレベル設定します
        /// </remarks>
        protected override void setUser(Object value)
        {
            base.setUser(value);

            //　Re-Calcボタンの表示/非表示の設定
            //【IssuesNo:18】样本重新计算功能移至移至（检测结果删除功能）权限下进行管理
            btnRecalc.Visible = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.SampleDataEditDelete);
            if (!btnRecalc.Visible)
            {
                this.DispRecalcInfoPanel = false;
            }
            // Deleteボタンの活性/非活性の設定
            this.tlbCommandBar.Tools[DELETE].SharedProps.Enabled = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.SampleDataEditDelete);
            // Save manual dilutionボタンの活性/非活性の設定
            this.tlbCommandBar.Tools[SAVE_MANUAL_DILUTION].SharedProps.Enabled = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.SampleDataEditDelete);
            //add by marxsu 数据修改权限的设置
            this.searchInfoPanelSpecimenResult.RemarkDataEditedEnable = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.SetRemarkDataEditedEnable); ;

            if (this.grdSpecimenResult.DisplayLayout.Bands[0].Columns.Count != 0)
            {
                //起動時はまだ列の定義が存在しない為、分岐させておく
                this.grdSpecimenResult.DisplayLayout.Bands[0].Columns[SpecimenResultData.DataKeys.DarkCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
                this.grdSpecimenResult.DisplayLayout.Bands[0].Columns[SpecimenResultData.DataKeys.BGCount].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);     //Hiddenなので!で符号逆転
                this.grdSpecimenResult.DisplayLayout.Bands[0].Columns[SpecimenResultData.DataKeys.ResultCount].Hidden
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
        /// 指定の測定結果情報を削除します
        /// </remarks>
        private void deleteData()
        {
            // 操作履歴：削除実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_003 });

            var selectRows = this.grdSpecimenResult.SearchSelectRow().Select((row) => ((SpecimenResultData)row.ListObject).GetUniqueNo()).Distinct();
            var selectRowDatas = this.grdSpecimenResult.Rows.Where((row) => selectRows.Contains(((SpecimenResultData)row.ListObject).GetUniqueNo())).Select((row) => (SpecimenResultData)row.ListObject).ToList();

            // 削除対象データ
            var deleteDatas = selectRowDatas.Where((data) => !Singleton<InProcessSampleInfoManager>.Instance.InProcessSampleList.Exists((info) => info.GetUniqueNumbers().Contains(data.GetUniqueNo())));

            if (DialogResult.OK == DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_038, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel))
            {
                deleteDatas.DeleteAllDataList();

                // 変更を反映
                Singleton<SpecimenResultDB>.Instance.SetData(deleteDatas.ToList());
                Singleton<SpecimenResultDB>.Instance.CommitData();
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
        /// 測定結果情報のファイル出力
        /// </summary>
        /// <remarks>
        /// 測定結果情報のファイル出力します
        /// </remarks>
        private void exportData()
        {
            // 操作履歴登録：ファイル出力実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_007 });

            // ファイル出力対象を取得
            List<OutPutSpecimenResultData> exportData;
            if (this.outputData(out exportData))
            {
                if (exportData.Count > 0)
                {
                    String fileName;
                    if (CarisXSubFunction.ShowSaveCSVFileDialog(out fileName, OutputFileKind.CSV, CarisXConst.EXPORT_CSV_SPECIMENRESULT, CarisX.Properties.Resources.STRING_SPECIMENRESULT_024, Singleton<CarisXUISettingManager>.Instance.SpecimenResultSettings) == System.Windows.Forms.DialogResult.OK)
                    {
                        Singleton<DataHelper>.Instance.ExportCsv(exportData, this.ResultGridColumns, fileName);
                    }
                }
                else
                {
                    // ファイル出力するデータがありません。
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_169, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.Confirm);
                }
            }
            else
            {
                // 操作履歴登録：ファイル出力キャンセル
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_008 });
            }
        }

        /// <summary>
        /// 測定結果情報の印刷出力
        /// </summary>
        /// <remarks>
        /// 測定結果情報の印刷出力します
        /// </remarks>
        private void printData()
        {
            // 操作履歴登録：印刷実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_005 });

            // 印刷対象を取得
            List<OutPutSpecimenResultData> printData;
            if (this.outputData(out printData))
            {
                if (printData.Count > 0)
                {
                    // 印刷用Listに取得データを格納
                    List<SpecimenResultReportData> rptData = new List<SpecimenResultReportData>();
                    rptData = this.convertPrintData(printData);

                    SpecimenResultPrint prt = new SpecimenResultPrint();
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
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_006 });
            }
        }

        /// <summary>
        /// 出力対象データの取得
        /// </summary>
        /// <remarks>
        /// 出力対象データの取得します
        /// </remarks>
        /// <param name="outputData">出力対象データ</param>
        /// <returns>true:出力実施あり/false:出力実施なし</returns>
        private Boolean outputData(out List<OutPutSpecimenResultData> outputData)
        {
            TargetRange outputRange = DlgTargetSelectRange.Show();
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //sw.Start();
            outputData = new List<OutPutSpecimenResultData>();
            switch (outputRange)
            {
                case TargetRange.All:

                    for (int i = 0; i < this.grdSpecimenResult.Rows.Count; i++)
                    {
                        OutPutSpecimenResultData item = new OutPutSpecimenResultData(this.grdSpecimenResult.Rows[i].ListObject as DataRowWrapperBase);
                        outputData.Add(item);
                    }
                    outputData.RollBackDataList();
                    break;
                case TargetRange.Specification:
                    List<UltraGridRow> searchRows = this.grdSpecimenResult.SearchSelectRow();
                    for (int i = 0; i < searchRows.Count; i++)
                    {
                        OutPutSpecimenResultData item = new OutPutSpecimenResultData(searchRows[i].ListObject as DataRowWrapperBase);
                        outputData.Add(item);
                    }
                    outputData.RollBackDataList();
                    break;
                default:
                    outputData = new List<OutPutSpecimenResultData>();
                    break;
            }

            return outputRange != TargetRange.None;
        }

        /// <summary>
        /// 手希釈率の保存
        /// </summary>
        /// <remarks>
        /// 手希釈率を保存します
        /// </remarks>
        private void saveMenuDilutionData()
        {
            if (this.grdSpecimenResult.ActiveCell != null && this.grdSpecimenResult.ActiveCell.IsInEditMode)
            {
                this.grdSpecimenResult.ActiveCell.Activated = false;
                if (this.grdSpecimenResult.ActiveCell != null)
                {
                    return;
                }
            }

            // 操作履歴登録：手希釈保存実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_018 });

            // 編集データ抽出
            IEnumerable<SpecimenResultData> dataList = this.grdSpecimenResult.Rows.OfType<UltraGridRow>().Select((row) => (SpecimenResultData)row.ListObject);
            var editList = dataList.Where((data) => data.IsModifyData()).ToList();

            SpecimenResultData modifyBeforeData;
            foreach (var editData in editList)
            {
                modifyBeforeData = new SpecimenResultData(editData.Copy());
                modifyBeforeData.RollbackData();

                var datas = dataList.OrderByDescending((data) => data.ReplicationNo).Where(
                    (data) => data.GetIndividuallyNo() == editData.GetIndividuallyNo()
                        && data.GetMeasureProtocolIndex() == editData.GetMeasureProtocolIndex()
                        && data.GetUniqueNo() == editData.GetUniqueNo());

                // 濃度値の小数点以下桁数の取得
                MeasureProtocol measureProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(editData.GetMeasureProtocolIndex());
                Int32 numOfDecimalPlaces = measureProtocol.LengthAfterDemPoint;

                Action<Boolean> concentrationChange = new Action<Boolean>((average) =>
               {
                   String concentration = String.Empty;
                   if (average)
                   {
                       concentration = editData.GetConcentrationAve();
                   }
                   else
                   {
                       concentration = editData.GetConcentration();
                   }

                   Double conc;
                   if (Double.TryParse(concentration, out conc))
                   {
                       // 濃度値を手希釈倍率の変更に連動させ保存
                       conc = (Double)(((Decimal)conc) * ((Decimal)editData.ManualDilution / (Decimal)modifyBeforeData.ManualDilution));
                       concentration = SubFunction.TruncateParse(conc, numOfDecimalPlaces);

                       // リマーク追加[データ編集]
                       Remark editRemark = editData.GetReplicationRemarkId();
                       editRemark.AddRemark(Remark.RemarkBit.EditOfManualDil);
                       editData.SetReplicationRemarkId(editRemark);

                       // レプリケーション(降順)の最初の要素(レプリケーション最大)と一致
                       // かつ、平均またはレプリケーションが単独かつ1の場合
                       editData.SetJudgement(String.Empty);
                       if (datas.First().ReplicationNo == editData.ReplicationNo && (average || (datas.Count() == 1 && editData.ReplicationNo == 1)))
                       {
                           editData.SetJudgement(CarisXCalculator.GetJudgementString(measureProtocol, conc, Const.SampleKind.Sample));
                       }
                   }
                   else if (editData.Concentration == null)
                   {
                       concentration = String.Empty;
                   }

                   if (average)
                   {
                       editData.SetConcentrationAve(concentration);
                   }
                   else
                   {
                       editData.SetConcentration(concentration);
                   }
               });

                concentrationChange(false);   // 濃度値
                concentrationChange(true);    // 濃度値平均
            }

            // 変更
            Singleton<SpecimenResultDB>.Instance.SetData(editList);
            Singleton<SpecimenResultDB>.Instance.CommitData();

            this.loadData();

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// オンライン出力
        /// </summary>
        /// <remarks>
        /// 一般検体測定結果データをオンライン出力します
        /// </remarks>
        private void transmit()
        {
            // 操作履歴登録：オンライン出力実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_012 });

            TargetRange outputRange = DlgTargetSelectRange.Show();

            // 出力対象を取得
            List<OutPutSpecimenResultData> transmitData = null;
            switch (outputRange)
            {
                case TargetRange.All:
                    Singleton<SpecimenResultDB>.Instance.LoadDB();
                    transmitData = this.grdSpecimenResult.Rows.Select((row) => new OutPutSpecimenResultData((row.ListObject as SpecimenResultData).Copy())).ToList();
                    break;
                case TargetRange.Specification:
                    Singleton<SpecimenResultDB>.Instance.LoadDB();

                    // 選択した対象を取ってくる
                    // 選択された検体の全レプリケーションが対象となる。
                    var selectRows = this.grdSpecimenResult.SearchSelectRow().Select((row) => ((SpecimenResultData)row.ListObject).GetUniqueNo()).Distinct();
                    var selectRowDatas = this.grdSpecimenResult.Rows.Where((row) => selectRows.Contains(((SpecimenResultData)row.ListObject).GetUniqueNo())).Select((row) => (SpecimenResultData)row.ListObject);

                    // 転送対象データ
                    transmitData = selectRowDatas.Where((data) => !Singleton<InProcessSampleInfoManager>.Instance.InProcessSampleList.Exists((info) => info.GetUniqueNumbers().Contains(data.GetUniqueNo()))).Select((data) => new OutPutSpecimenResultData(data)).ToList();
                    break;
                case TargetRange.None:
                    // 操作履歴登録：オンライン出力キャンセル
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_013 });
                    return;
            }

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

        #endregion

        /// <summary>
        /// 待機メッセージ
        /// </summary>
        DlgMessage waitMessage = null;
        /// <summary>
        /// ホストへ測定データを送信
        /// </summary>
        /// <remarks>
        /// ホストへ測定データを送信します
        /// </remarks>
        /// <param name="data"></param>
        private void sendHost(List<OutPutSpecimenResultData> data)
        {
            using (this.waitMessage = DlgMessage.Create(Properties.Resources.STRING_DLG_MSG_209, ""
                , Properties.Resources.STRING_DLG_TITLE_008, MessageDialogButtons.Cancel))
            {
                ManualResetEvent canceller = new ManualResetEvent(false);
                Singleton<CarisXSequenceHelperManager>.Instance.Host.HostCommunicationSequence
                    (HostCommunicationSequencePattern.SendResultToHostBatch | HostCommunicationSequencePattern.Specimen, data, canceller);
                if (this.waitMessage.ShowDialog() == DialogResult.Cancel)
                {
                    canceller.Set();
                }
            }
            this.waitMessage = null;
        }
        /// <summary>
        /// ホストへ測定データを送信完了
        /// </summary>
        /// <remarks>
        /// 待機メッセージを終了します
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
        /// 絞込みボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 絞り込みパネルを表示します
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
        /// 検体測定結果から絞り込みを行います
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void searchInfoPanelSpecimenResult_OkClick(object sender, EventArgs e)
        {
            var searchDatas = Singleton<SpecimenResultDB>.Instance.GetSearchData(this.searchInfoPanelSpecimenResult);

            // 修改内容：ページめくり処理追加
            if (IsAllowPageBreaks(this.currentSampleResultInfo.Count))
                this.grdSpecimenResult.DataSource = new BindingList<SpecimenResultData>(searchDatas.Take(this.pageSize).ToList());
            else
                this.grdSpecimenResult.DataSource = new BindingList<SpecimenResultData>(searchDatas);
            
            //this.grdSpecimenResult.DataSource = new BindingList<SpecimenResultData>(searchDatas);
            this.btnFilter.Appearance.ImageBackground = CarisX.Properties.Resources.Image_SelectButton_selected;
        }

        /// <summary>
        /// 絞り込みパネルキャンセルボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 検体測定結果から絞り込みをキャンセルします
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void searchInfoPanelSpecimenResult_CancelClick(object sender, EventArgs e)
        {
            this.bindCurrentSampleResultInfo = null;
            this.loadData();
            this.btnFilter.Appearance.ImageBackground = CarisX.Properties.Resources.Image_SelectButton;
        }

        /// <summary>
        /// 絞り込みパネル閉じるボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 絞り込みパネルが非表示となります
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void searchInfoPanelSpecimenResult_CloseClick(object sender, EventArgs e)
        {
            this.DispSearchInfoPanel = false;
        }

        #endregion

        #region _再計算パネルイベント_

        /// <summary>
        /// 再計算OKボタンクリック
        /// </summary>
        /// <remarks>
        /// 検体測定結果の再計算を実行します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void recalcInfoPanelSpecimenResult_OkClick(object sender, EventArgs e)
        {
            lock (this)
            {
                IRecalcInfoSpecimenResult recalcInfo = this.recalcInfoPanelSpecimenResult;

                //【IssuesNo:9】在样本结果中增加ReceiptNo,用于IGRA项目再计算
                var recalcData = (from data in Singleton<SpecimenResultDB>.Instance.GetReCalcData(recalcInfo)
                                  let calcData = new CalcData( data.GetModuleNo()
                                                             , data.GetMeasureProtocolIndex()
                                                             , data.ReagentLotNo
                                                             , data.GetIndividuallyNo()
                                                             , data.GetUniqueNo()
                                                             , data.ReplicationNo
                                                             , data.ManualDilution
                                                             , data.AutoDilution
                                                             , data.MeasureDateTime
                                                             , data.GetReceiptNo()
                                                             , data.RackId
                                                             , data.RackPosition
                                                             , data.PatientId )
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
                        data.Key.CalcInfoAverage = new CalcInfo(data.Value.data.GetCountAve());
                        data.Key.CalcInfoAverage.Remark = data.Value.data.GetRemarkId();
                    }
                }

                if (CarisXCalculator.ReCalcSpecimen(recalcInfoPanelSpecimenResult, recalcData.Keys.ToList(), out completeData) && completeData != null)
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

                           if (!String.IsNullOrEmpty(calcData.Judgement))
                           {
                               updateData.data.SetJudgement(calcData.Judgement);
                           }
                           else
                           {
                               updateData.data.SetJudgement(null);
                           }
                       }
                   });

                    Singleton<SpecimenResultDB>.Instance.SetData(recalcData.Select((data) => data.Value.data).ToList());
                    Singleton<SpecimenResultDB>.Instance.CommitData();

                    this.loadData();
                }
            }
        }

        /// <summary>
        /// 再計算閉じるボタンクリック
        /// </summary>
        /// <remarks>
        /// 検体測定結果再計算パネルを非表示にします
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void recalcInfoPanelSpecimenResult_CloseClick(object sender, EventArgs e)
        {
            recalcInfoPanelSpecimenResult.Visible = false;
        }

        #endregion

        /// <summary>
        /// フォーム読み込みイベント
        /// </summary>
        /// <remarks>
        /// データ読込を行います
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void FormSpecimenResult_Load(object sender, EventArgs e)
        {
            this.loadData();
        }

        /// <summary>
        /// データの読み込み
        /// </summary>
        /// <remarks>
        /// 検体測定結果データを取得します
        /// </remarks>
        private void loadData()
        {
            var list = this.grdSpecimenResult.DataSource as BindingList<SpecimenResultData>;
            if (this.bindCurrentSampleResultInfo == null || list == null)
            {
                this.currentSampleResultInfo = Singleton<SpecimenResultDB>.Instance.GetSearchData();
                this.bindCurrentSampleResultInfo = new BindingList<SpecimenResultData>(this.currentSampleResultInfo);

                // 修改内容：ページめくり処理追加
                //this.grdSpecimenResult.DataSource = this.bindCurrentSampleResultInfo;

                if (IsAllowPageBreaks(this.bindCurrentSampleResultInfo.Count))
                    this.grdSpecimenResult.DataSource = new BindingList<SpecimenResultData>(this.currentSampleResultInfo.Take(this.pageSize).ToList());
                else
                    this.grdSpecimenResult.DataSource = this.bindCurrentSampleResultInfo;
            }
            else
            {
                list.RaiseListChangedEvents = false;
                list.Clear();
                // 修改内容：ページめくり処理追加
                this.currentSampleResultInfo = Singleton<SpecimenResultDB>.Instance.GetSearchData();
                //if (list == this.bindCurrentSampleResultInfo)
                //{
                //    Singleton<SpecimenResultDB>.Instance.GetSearchData().ForEach((data) => list.Add(data));
                //}
                //else
                //{
                //    Singleton<SpecimenResultDB>.Instance.GetSearchData(this.searchInfoPanelSpecimenResult).ForEach((data) => list.Add(data));
                //}
                int iPageIndex = this.pageCurrent;
                if (IsAllowPageBreaks(this.currentSampleResultInfo.Count))
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
                        this.currentSampleResultInfo.Skip(iSkipNum).Take(this.pageSize).ToList().ForEach((data) => list.Add(data));
                    }
                    if (this.pageCurrent == 1)
                    {
                        this.currentSampleResultInfo.Take(this.pageSize).ToList().ForEach((data) => list.Add(data));
                    }
                }
                else
                {
                    this.currentSampleResultInfo.Take(this.pageSize).ToList().ForEach((data) => list.Add(data));
                }

                list.RaiseListChangedEvents = true;
                list.ResetBindings();
            }
        }

        /// <summary>
        /// FormClosedイベント
        /// </summary>
        /// <remarks>
        /// UI設定を保存します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void FormSpecimenResult_FormClosed(object sender, FormClosedEventArgs e)
        {
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenResultSettings.GridZoom = this.zoomPanel.Zoom;
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenResultSettings.GridColOrder = this.grdSpecimenResult.GetGridColumnOrder();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenResultSettings.GridColWidth = this.grdSpecimenResult.GetGridColmnWidth();
        }

        /// <summary>
        /// グリッドセルクリックイベント
        /// </summary>
        /// <remarks>
        /// リマークセルクリック時、リマーク詳細ダイアログ表示します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void grdSpecimenResult_ClickCell(object sender, ClickCellEventArgs e)
        {
            // リマークセルクリック時、リマーク詳細ダイアログ表示
            if (e.Cell.Column.Key == SpecimenResultData.DataKeys.Remark)
            {
                var SpecimenResultData = ((SpecimenResultData)e.Cell.Row.ListObject);
                if (SpecimenResultData.GetRemarkId() != null)
                {
                    DlgRemarkDetail.Show(SpecimenResultData.GetRemarkId());
                }
                else
                {
                    DlgRemarkDetail.Show(SpecimenResultData.GetReplicationRemarkId());
                }
            }
        }

        /// <summary>
        /// セルデータエラーイベント
        /// </summary>
        /// <remarks>
        /// エラーイベント設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void grdSpecimenResult_CellDataError(object sender, CellDataErrorEventArgs e)
        {
            e.RaiseErrorEvent = false;
        }

        /// <summary>
        /// システムステータス変化イベント
        /// </summary>
        /// <remarks>
        /// システムステータス変化により検索・再計算パネルボタンの有効無効状態設定します
        /// </remarks>
        /// <param name="value"></param>
        private void onSystemsStatusChanged(Object value)
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
            if (this.currentSampleResultInfo == null || this.currentSampleResultInfo.Count == 0)
                return;

            if ((this.pageCurrent > 1 && this.pageCurrent < this.pageCount) || this.pageCurrent == this.pageCount)
            {
                int iSkipNum = (this.pageCurrent - 1) * this.pageSize;
                this.grdSpecimenResult.DataSource = new BindingList<SpecimenResultData>(this.currentSampleResultInfo.Skip(iSkipNum).Take(this.pageSize).ToList());
                this.btnPre.Enabled = true;
                if (this.pageCurrent == this.pageCount)
                    this.btnNext.Enabled = false;
                else
                    this.btnNext.Enabled = true;
                return;
            }

            if (this.pageCurrent == 1)
            {
                this.grdSpecimenResult.DataSource = new BindingList<SpecimenResultData>(this.currentSampleResultInfo.Take(this.pageSize).ToList());
                this.btnPre.Enabled = false;
                this.btnNext.Enabled = true;
            }

        }

        /// <summary>
        /// 検体結果グリッドセル変更イベント
        /// </summary>
        /// <remarks>
        /// 値変更時、Form共通の編集中フラグをONにします。
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenResult_CellChange(object sender, CellEventArgs e)
        {
            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        #endregion
    }
}
