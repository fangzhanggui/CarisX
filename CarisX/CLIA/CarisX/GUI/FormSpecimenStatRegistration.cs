using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Infragistics.Win.UltraWinDataSource;

using Oelco.Common.GUI;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.DB;
using Oelco.CarisX.Const;
using Infragistics.Win.UltraWinGrid;
using System.Text.RegularExpressions;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Common;
using Oelco.Common.DB;
using Oelco.CarisX.Print;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.CarisX.Comm;
using System.Threading;
using System.Threading.Tasks;
using Oelco.CarisX.Status;


namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 検体登録画面クラス
    /// </summary>
    public partial class FormSpecimenStatRegistration : FormChildBase
    {
        #region [定数定義]

        /// <summary>
        /// 保存
        /// </summary>
        private const String SAVE = "Save";

        /// <summary>
        /// グリッド列名 ラックID
        /// </summary>
        public const String STRING_GRD_RACKID = "RackID";
        public const int INDEX_GRD_RACKID = 0;
        /// <summary>
        /// グリッド列名 ラックポジション
        /// </summary>
        public const String STRING_GRD_RACKPOSITION = "Rack Position";
        public const int INDEX_GRD_RACKPOSITION = 1;
        /// <summary>
        /// グリッド列名 受付番号
        /// </summary>
        public const String STRING_GRD_RECEIPTNO = "Receipt No.";
        public const int INDEX_GRD_RECEIPTNO = 2;
        /// <summary>
        /// グリッド列名 検体ID
        /// </summary>
        public const String STRING_GRD_PATIENTID = "Patient ID";
        public const int INDEX_GRD_PATIENTID = 3;
        /// <summary>
        /// グリッド列名 検体種別
        /// </summary>
        public const String STRING_GRD_SPECIMENTYPE = "Specimen type";
        public const int INDEX_GRD_SPECIMENTYPE = 4;
        /// <summary>
        /// グリッド列名 分析項目
        /// </summary>
        public const String STRING_GRD_REGISTERED = "Registered";
        public const int INDEX_GRD_REGISTERED = 5;
        /// <summary>
        /// グリッド列名 手稀釈倍率
        /// </summary>
        public const String STRING_GRD_MANUALDILUTIONRATIO = "Manual dilution ratio";
        public const int INDEX_GRD_MANUALDILUTIONRATIO = 6;
        /// <summary>
        /// グリッド列名 コメント
        /// </summary>
        public const String STRING_GRD_COMMENT = "Coment";
        public const int INDEX_GRD_COMMENT = 7;
        /// <summary>
        /// グリッド列名 登録種別
        /// </summary>
        public const String STRING_GRD_REGISTTYPE = "Regist type";
        public const int INDEX_GRD_REGISTTYPE = 8;
        /// <summary>
        /// グリッド列名 容器種別
        /// </summary>
        public const String STRING_GRD_VESSELTYPE = "Vessel type";
        public const int INDEX_GRD_VESSELTYPE = 9;

        /// <summary>
        /// 削除
        /// </summary>
        private const String DELETE = "Delete";

        /// <summary>
        /// 全て削除
        /// </summary>
        private const String DELETE_ALL = "Delete all";

        /// <summary>
        /// 印刷
        /// </summary>
        private const String PRINT = "Print";

        /// <summary>
        /// ペースト
        /// </summary>
        private const String QUERY = "Query";

        #endregion

        #region [インスタンス変数定義]
        /// <summary>
        /// 検体登録画面UI設定
        /// </summary>
        private FormSpecimenResistrationSettings FormSpecimenStatRegistrationSettings;

        #endregion

        private bool isFirstDisplay = true;

        /// <summary>
        /// 画面表示
        /// </summary>
        /// <remarks>
        /// 画面表示します
        /// </remarks>
        /// <param name="captScreenRect"></param>
        public override void Show(Rectangle captScreenRect)
        {
            this.InitializeGridView();
            this.loadGridData();

            // stopwatch
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //sw.Start();

            isFirstDisplay = true;
            base.Show(captScreenRect);

            // stopwatch
            //sw.Stop();
            //Console.WriteLine("経過時間 captScreenRect　={0}", sw.Elapsed);
        }

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormSpecimenStatRegistration()
        {
            InitializeComponent();

            // 一時登録データのみ削除
            Singleton<SpecimenStatDB>.Instance.Delete(RegistType.Temporary);

            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.RealtimeData, this.onRealTimeDataChanged);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.AnalyteRoutineTableChanged, this.onAnalyteRoutineTableChanged);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UseOfPrint, this.onPrintParamChanged);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UseOfHost, this.onHostParamChanged);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.HostWorkSheetBatchSTAT, this.workSheetFromHostByBatch);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.AskBatchCompleteSTAT, this.askBatchComplete);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ChangeAnalyteGroup, this.onChangeAnalyteGroup);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.HostWorkSheetSingle, this.workSheetFromHostSingle);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ChangeProtocolSetting, this.onChangeProtocolSetting);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.AssayModeUseOfEmergencyMode, this.onAssayModeKindChanged);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.STATPatientIDformatError, this.chengePatientID);

            // グリッド背景色カラー設定
            this.getGridData(RegistType.Temporary).SetGridRowBackgroundColorRuleFromIndex(1, new List<Color>()
            {
                CarisXConst.GRID_ROWS_DEFAULT_COLOR,CarisXConst.GRID_ROWS_COLOR_PATTERN1
            });
            this.getGridData(RegistType.Fixed).SetGridRowBackgroundColorRuleFromIndex(1, new List<Color>()
            {
                CarisXConst.GRID_ROWS_DEFAULT_COLOR,CarisXConst.GRID_ROWS_COLOR_PATTERN1
            });

            // ツールバーの右ボタンを設定不可イベント登録
            this.tlbCommandBar.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.tlbCommandBar_BeforeToolbarListDropdown);
        }

        /// <summary>
        /// ツールバーの右ボタンを設定不可
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbCommandBar_BeforeToolbarListDropdown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        /// 編集データ
        /// </summary>
        private Dictionary<UltraDataRow, SpecimenStatRegistrationGridViewDataSet> editData = new Dictionary<UltraDataRow, SpecimenStatRegistrationGridViewDataSet>();

        /// <summary>
        /// DBレコードマッピングデータ(GridのDBのキー項目-Index)
        /// </summary>
        private Dictionary<Tuple<String, Int32, String>, Int32> dbLinkData = new Dictionary<Tuple<String, Int32, String>, Int32>();

        /// <summary>
        /// DBデータ（編集前）
        /// </summary>
        private List<SpecimenStatRegistrationGridViewDataSet> dispDataOriginal = new List<SpecimenStatRegistrationGridViewDataSet>();

        /// <summary>
        /// 急診モードの分析項目ボタンを非活性にするかの判断フラグ
        /// </summary>
        private bool enabledFlag;

        /// <summary>
        /// グリッドセルの編集可否設定
        /// </summary>
        /// <remarks>
        /// 登録状態や分析方式によりグリッドセルの編集可否設定します
        /// </remarks>
        /// <param name="registered"></param>
        /// <param name="row"></param>
        private void setItemEditable(Boolean registered, UltraGridRow row)
        {
            if (registered)
            {
                // 登録済みデータ編集可否設定
                row.Cells[STRING_GRD_PATIENTID].Activation = Activation.NoEdit; // 編集不可のスタイル
                row.Cells[STRING_GRD_SPECIMENTYPE].Activation = Activation.NoEdit; // 編集可のスタイル
                row.Cells[STRING_GRD_VESSELTYPE].Activation = Activation.NoEdit; // 編集可のスタイル
            }
            else
            {
                // 未登録データ編集可否設定          
                row.Cells[STRING_GRD_PATIENTID].Activation = Activation.AllowEdit; // 編集可のスタイル
                row.Cells[STRING_GRD_SPECIMENTYPE].Activation = Activation.AllowEdit; // 編集可のスタイル
                row.Cells[STRING_GRD_VESSELTYPE].Activation = Activation.AllowEdit; // 編集可のスタイル
            }

            // 分析方式による編集可否設定
            row.Cells[STRING_GRD_RACKID].Activation = Activation.NoEdit;
            row.Cells[STRING_GRD_RACKPOSITION].Activation = Activation.NoEdit;
        }

        /// <summary>
        /// グリッドデータ読込
        /// </summary>
        /// <remarks>
        /// グリッド表示データをDBから取得します。
        /// </remarks>
        private void loadGridData(Int32 currentIndex = 0, bool bRealTime = false, RegistType registType = RegistType.Temporary)
        {
            // DBからデータを取得
            Singleton<SpecimenStatDB>.Instance.LoadDB();
            this.dispDataOriginal = Singleton<SpecimenStatDB>.Instance.GetDispData();

            // グリッドにデータを設定
            foreach (SpecimenStatRegistrationGridViewDataSet val in dispDataOriginal)
            {
                Tuple<String, Int32, String> linkKey = new Tuple<String, Int32, String>(val.RackID.DispPreCharString, val.RackPosition, val.PatientID);
                if (this.dbLinkData.ContainsKey(linkKey))
                {
                    // この画面にデータをロードするのが2度目の場合
                    // いずれかの処理により、検体登録情報が存在し、DBのデータ行と関連付けがされている。
                    Int32 index = this.dbLinkData[linkKey];
                    Int32 rowIndex = (val.RegistType == RegistType.Temporary) ? 0 : (index - 1);
                    if (rowIndex < this.getDataSource(val.RegistType).Rows.Count)
                    {
                        UltraDataRow row = this.getDataSource(val.RegistType).Rows[rowIndex];
                        row[INDEX_GRD_RACKID] = val.RackID.DispPreCharString;
                        row[INDEX_GRD_RACKPOSITION] = val.RackPosition.ToString();
                        row[INDEX_GRD_RECEIPTNO] = val.ReceiptNumber;
                        row[INDEX_GRD_PATIENTID] = val.PatientID;
                        row[INDEX_GRD_SPECIMENTYPE] = CarisXSubFunction.GetSampleKindGridItemString(val.SpecimenType);
                        row[INDEX_GRD_REGISTERED] = this.joinProtocolNames(val.Registered);
                        row[INDEX_GRD_MANUALDILUTIONRATIO] = val.ManualDil.ToString();
                        row[INDEX_GRD_COMMENT] = val.Comment;
                        row[INDEX_GRD_REGISTTYPE] = val.RegistType;
                        row[INDEX_GRD_VESSELTYPE] = CarisXSubFunction.GetSpecimenCupKindGridItemString(val.VesselType);
                        this.setItemEditable(true, this.getGridData(val.RegistType).Rows[rowIndex]);

                        this.existData[index] = val;

                        // 手希釈編集設定
                        this.setAllowManualDilEdit(row, val);
                    }
                }
                else
                {
                    // 検体ID分析なら上から順に。ラックID分析なら位置合わせる。
                    // この画面にデータを最初にロードする場合,或はデータ全削除が行われた直後。
                    // 起動時に検体登録情報はDBからクリアされる為、必ずデータは存在しない状態となる。
                    Int32 index = 0;
                    // グリッドに表示しているラックIDとポジションと検体IDに応じて表示位置を検索する。
                    var searched = from v in this.getDataSource(val.RegistType).Rows.OfType<UltraDataRow>()
                                   where (String)v[STRING_GRD_RACKID] == linkKey.Item1 && Int32.Parse((String)v[STRING_GRD_RACKPOSITION]) == linkKey.Item2
                                   select v;
                    // 検索したデータは、ラックID分析の場合は必ず1つみつかる。（設定範囲外のデータはDB登録前にエラーで弾かれる。
                    // 複数見つかる場合は、このグリッドの初期化が検体ID分析で行われているので、ラックIDとラックポジションが空白になっているのと、
                    // 検索データが検体ID以外ない。複数見つかった場合も、一つ見つかった場合も、見つかった最初行が該当行になる。
                    if (searched.Count() != 0)
                    {
                        index = searched.First().Index;
                    }
                    else
                    {
                        // 無ければ、登録されている検体の一覧から最終インデックスを調べる
                        var indexList = from v in this.dbLinkData
                                        orderby v.Value descending
                                        select v.Value;
                        if (indexList.Count() != 0)
                        {
                            index = indexList.First() + 1;
                        }

                    }

                    Int32 rowIndex = index;
                    index = (val.RegistType == RegistType.Temporary) ? 0 : (index + 1);

                    // インデックス登録
                    this.dbLinkData[linkKey] = index;

                    if (rowIndex < this.getDataSource(val.RegistType).Rows.Count)
                    {
                        UltraDataRow row = this.getDataSource(val.RegistType).Rows[rowIndex];
                        row[INDEX_GRD_RACKID] = val.RackID.DispPreCharString;
                        row[INDEX_GRD_RACKPOSITION] = val.RackPosition.ToString();
                        row[INDEX_GRD_RECEIPTNO] = val.ReceiptNumber;
                        row[INDEX_GRD_PATIENTID] = val.PatientID;
                        row[INDEX_GRD_SPECIMENTYPE] = CarisXSubFunction.GetSampleKindGridItemString(val.SpecimenType);
                        row[INDEX_GRD_REGISTERED] = this.joinProtocolNames(val.Registered);
                        row[INDEX_GRD_MANUALDILUTIONRATIO] = val.ManualDil.ToString();
                        row[INDEX_GRD_COMMENT] = val.Comment;
                        row[INDEX_GRD_REGISTTYPE] = val.RegistType;
                        row[INDEX_GRD_VESSELTYPE] = CarisXSubFunction.GetSpecimenCupKindGridItemString(val.VesselType);
                        this.setItemEditable(true, this.getGridData(val.RegistType).Rows[row.Index]);

                        this.existData[index] = val;

                        // 手希釈編集設定
                        this.setAllowManualDilEdit(row, val);
                    }
                }
            }

            if (existData.Count != 0)
            {
                foreach (var dat in this.existData)
                {
                    // 編集データがある場合
                    if (dat.Value.ReceiptNumber == 0)
                    {
                        int rowIndex = dat.Value.RegistType == RegistType.Temporary ? 0 : dat.Value.RackPosition - 1;

                        UltraDataRow row = this.getDataSource(dat.Value.RegistType).Rows[rowIndex];
                        row[INDEX_GRD_RACKID] = dat.Value.RackID.DispPreCharString;
                        row[INDEX_GRD_RACKPOSITION] = dat.Value.RackPosition.ToString();
                        row[INDEX_GRD_RECEIPTNO] = dat.Value.ReceiptNumber;
                        row[INDEX_GRD_PATIENTID] = dat.Value.PatientID;
                        row[INDEX_GRD_SPECIMENTYPE] = CarisXSubFunction.GetSampleKindGridItemString(dat.Value.SpecimenType);
                        row[INDEX_GRD_REGISTERED] = this.joinProtocolNames(dat.Value.Registered);
                        row[INDEX_GRD_MANUALDILUTIONRATIO] = dat.Value.ManualDil.ToString();
                        row[INDEX_GRD_COMMENT] = dat.Value.Comment;
                        row[INDEX_GRD_REGISTTYPE] = dat.Value.RegistType;
                        row[INDEX_GRD_VESSELTYPE] = CarisXSubFunction.GetSpecimenCupKindGridItemString(dat.Value.VesselType);
                        this.setItemEditable(true, this.getGridData(dat.Value.RegistType).Rows[rowIndex]);
                        this.editData[row] = dat.Value;

                        // 手希釈編集設定
                        this.setAllowManualDilEdit(row, dat.Value);

                        // 現在行に編集データ行を設定
                        currentIndex = row.Index;
                        registType = dat.Value.RegistType;
                    }
                }
            }

            // 設定行を選択状態にする。
            // リアルタイム更新ではない場合、選択する
            grdSpecimenStatRegistration.Refresh();
            grdSpecimenStatRegistrationFixed.Refresh();
            if (bRealTime == false)
            {
                if (registType == RegistType.Temporary)
                {
                    this.grdSpecimenStatRegistration.Rows[currentIndex].Selected = true;
                    this.grdSpecimenStatRegistration.Rows[currentIndex].Activate();
                    this.clearGridSelected(RegistType.Fixed);
                }
                else
                {
                    this.grdSpecimenStatRegistrationFixed.Rows[currentIndex].Selected = true;
                    this.grdSpecimenStatRegistrationFixed.Rows[currentIndex].Activate();
                    this.clearGridSelected(RegistType.Temporary);
                }
            }
        }

        /// <summary>
        /// 分析項目表示文字列連結
        /// </summary>
        /// <remarks>
        /// 分析項目の設定情報から、グリッドに表示される分析項目文字列を作成します。
        /// </remarks>
        /// <param name="protocolRegistInfo">分析項目設定リスト</param>
        /// <returns>分析項目表示文字列</returns>
        protected String joinProtocolNames(List<Tuple<Int32?, Int32?, Int32?>> protocolRegistInfo)
        {
            String result = String.Empty;
            String protocolItem = String.Empty;
            enabledFlag = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.IsProtocolEnabledChangedInEmergencyMode();

            // プロトコル表示文字列を作成する。(AAA,BBB,CCC(100))
            if (protocolRegistInfo.Count != 0)
            {
                List<String> sa = new List<String>();
                for (Int32 i = 0; i < protocolRegistInfo.Count; i++)
                {
                    if (protocolRegistInfo[i].Item1 != null)
                    {
                        var protocal = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex((Int32)protocolRegistInfo [i].Item1);

                        // 急診使用によってグリッドから分析項目を非表示にする必要のない場合
                        if ((enabledFlag == false) || (protocal.UseEmergencyMode == false))
                        {
                            String protocolName = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(protocolRegistInfo [i].Item1.Value).ProtocolName;
                            protocolItem = protocolName;
                            if (protocolRegistInfo [i].Item2 != 1)
                            {
                                protocolItem += String.Format("(x{0})", protocolRegistInfo [i].Item2);
                            }
                            protocolItem += String.Format("[{0}]", protocolRegistInfo [i].Item3);

                            sa.Add(protocolItem);
                        }                       
                    }
                }
                result = String.Join(",", sa);
            }
            return result;
        }

        /// <summary>
        /// フォームコンポーネント初期化
        /// </summary>
        /// <remarks>
        /// 画面表示項目関連の初期化を行います。
        /// </remarks>
        protected override void initializeFormComponent()
        {
            // 拡大率変更イベント登録
            this.zoomPanel.SetZoom += getGridData(RegistType.Temporary).SetGridZoom;
            this.zoomPanel.SetZoom += getGridData(RegistType.Fixed).SetGridZoom;

            // 拡大率切替コントロール初期化
            zoomPanel.Zoom = Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenResistrationSettings.GridZoom;

            // グリッド初期化
            this.InitializeGridView();

            // グリッド表示データを取得
            this.loadGridData();

            // オンライン出力ボタン表示設定
            this.tlbCommandBar.Tools[QUERY].SharedProps.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Enable;

            // 印刷ボタン表示設定
            this.tlbCommandBar.Tools[PRINT].SharedProps.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable;

            // スクロール処理設定
            this.gesturePanel.ScrollProxy = this.getGridData(RegistType.Temporary).ScrollProxy;
            this.gesturePanelFixed.ScrollProxy = this.getGridData(RegistType.Fixed).ScrollProxy;

            // グリッド表示順
            this.getGridData(RegistType.Temporary).SetGridColumnOrder(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenResistrationSettings.GridColOrder);
            this.getGridData(RegistType.Fixed).SetGridColumnOrder(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenResistrationSettings.GridColOrder);

            // グリッド列幅
            this.getGridData(RegistType.Temporary).SetGridColmnWidth(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenResistrationSettings.GridColWidth);
            this.getGridData(RegistType.Fixed).SetGridColmnWidth(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenResistrationSettings.GridColWidth);
        }

        /// <summary>
        /// リアルタイムデータ更新
        /// </summary>
        /// <remarks>
        /// 登録データ表示更新します
        /// </remarks>
        /// <param name="kind"></param>
        protected void onRealTimeDataChanged(object kind)
        {
            // 登録データ表示更新
            if (((RealtimeDataKind)kind) == RealtimeDataKind.StatRegist)
            {
                int currentRowIndex = -1;
                var selectedTemporary = this.getGridData(RegistType.Temporary).SearchSelectRow();
                var selectedFixed = this.getGridData(RegistType.Fixed).SearchSelectRow();

                // 検体IDが空または0の場合、編集中のデータに対して、最後に選択されたレコードを選択する
                if ((selectedTemporary != null)
                    && (selectedTemporary.Count() > 0)
                    && ((selectedTemporary.Last().Cells[STRING_GRD_RECEIPTNO].Value.ToString() == string.Empty)
                        || (selectedTemporary.Last().Cells[STRING_GRD_RECEIPTNO].Value.ToString() == "0")))
                {
                    object[] noSaveData = new object[]{ selectedTemporary.Last().Cells[INDEX_GRD_RACKID].Value
                                                      , selectedTemporary.Last().Cells[INDEX_GRD_RACKPOSITION].Value
                                                      , selectedTemporary.Last().Cells[INDEX_GRD_RECEIPTNO].Value
                                                      , selectedTemporary.Last().Cells[INDEX_GRD_PATIENTID].Value
                                                      , selectedTemporary.Last().Cells[INDEX_GRD_SPECIMENTYPE].Value
                                                      , selectedTemporary.Last().Cells[INDEX_GRD_REGISTERED].Value
                                                      , selectedTemporary.Last().Cells[INDEX_GRD_MANUALDILUTIONRATIO].Value
                                                      , selectedTemporary.Last().Cells[INDEX_GRD_COMMENT].Value
                                                      , selectedTemporary.Last().Cells[INDEX_GRD_REGISTTYPE].Value
                                                      , selectedTemporary.Last().Cells[INDEX_GRD_VESSELTYPE].Value };

                    currentRowIndex = selectedTemporary.Last().Index;

                    this.InitializeGridView(true, currentRowIndex, noSaveData, RegistType.Temporary);
                    this.loadGridData(0, true);
                    UltraGridRow row = this.getGridData(RegistType.Temporary).Rows[currentRowIndex];
                    // 指定行を編集可能にする
                    this.setItemEditable(false, this.getGridData(RegistType.Temporary).Rows[currentRowIndex]);
                    // 指定行を選択する
                    this.getGridData(RegistType.Temporary).Rows[currentRowIndex].Selected = true;
                    this.getGridData(RegistType.Temporary).Rows[currentRowIndex].Activate();
                }
                else if ((selectedFixed != null)
                        && (selectedFixed.Count() > 0)
                        && ((selectedFixed.Last().Cells[STRING_GRD_RECEIPTNO].Value.ToString() == string.Empty)
                            || (selectedFixed.Last().Cells[STRING_GRD_RECEIPTNO].Value.ToString() == "0")))
                {
                    object[] noSaveData = new object[]{ selectedFixed.Last().Cells[INDEX_GRD_RACKID].Value
                                                      , selectedFixed.Last().Cells[INDEX_GRD_RACKPOSITION].Value
                                                      , selectedFixed.Last().Cells[INDEX_GRD_RECEIPTNO].Value
                                                      , selectedFixed.Last().Cells[INDEX_GRD_PATIENTID].Value
                                                      , selectedFixed.Last().Cells[INDEX_GRD_SPECIMENTYPE].Value
                                                      , selectedFixed.Last().Cells[INDEX_GRD_REGISTERED].Value
                                                      , selectedFixed.Last().Cells[INDEX_GRD_MANUALDILUTIONRATIO].Value
                                                      , selectedFixed.Last().Cells[INDEX_GRD_COMMENT].Value
                                                      , selectedFixed.Last().Cells[INDEX_GRD_REGISTTYPE].Value
                                                      , selectedFixed.Last().Cells[INDEX_GRD_VESSELTYPE].Value };

                    currentRowIndex = selectedFixed.Last().Index;

                    this.InitializeGridView(true, currentRowIndex, noSaveData, RegistType.Fixed);
                    this.loadGridData(0, true);
                    UltraGridRow row = this.getGridData(RegistType.Fixed).Rows[currentRowIndex];
                    // 指定行を編集可能にする
                    this.setItemEditable(false, this.getGridData(RegistType.Fixed).Rows[currentRowIndex]);
                    // 指定行を選択する
                    this.getGridData(RegistType.Fixed).Rows[currentRowIndex].Selected = true;
                    this.getGridData(RegistType.Fixed).Rows[currentRowIndex].Activate();
                }
                else
                {
                    // 登録データ表示更新
                    this.InitializeGridView(true);
                    this.loadGridData();
                }

            }
        }

        /// <summary>
        /// 検体ルーチンテーブル変更
        /// </summary>
        /// <remarks>
        /// 検体ルーチンテーブル変更します
        /// </remarks>
        /// <param name="value"></param>
        protected void onAnalyteRoutineTableChanged(Object value)
        {
            // 検体登録DB全消去
            Singleton<SpecimenStatDB>.Instance.DeleteAll();
            Singleton<SpecimenStatDB>.Instance.CommitSampleInfo();

            // グリッド初期化
            this.InitializeGridView();

            // マッピングデータをクリアする。
            this.dbLinkData.Clear();

            // 編集データをクリアする。
            this.editData.Clear();

            this.loadGridData();

            // 測定テーブル表示更新
            this.analysisSettingPanel.ReLoadAnalyteInformation();

            // 分析項目ボタンに活性/非活性の変更
            this.protocolIndexToButtonDicEnabled();
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
        /// AnalyteGroup変更時処理
        /// </summary>
        /// <param name="value"></param>
        private void onChangeAnalyteGroup(Object value)
        {
            // AnalyteGroupボタンの更新
            analysisSettingPanel.SetGroupButton();
        }

        /// <summary>
        /// グリッド表示初期化
        /// </summary>
        /// <remarks>
        /// グリッド表示内容の空状態を作成します。
        /// </remarks>
        public void InitializeGridView(Boolean bRealTime = false, int nCurrentIndex = -1, object[] noSaveData = null, RegistType type = RegistType.Temporary)
        {
            // グリッド初期化
            this.getDataSource(RegistType.Temporary).Rows.Clear();
            this.getDataSource(RegistType.Fixed).Rows.Clear();

            // グリッドデータが初期化される為、GridRowの関連する保持情報が無効になる。
            this.dbLinkData.Clear();

            // 非リアルタイム更新、編集データを削除しないでください
            this.editData.Clear();

            // 編集中データクリア
            this.clearExitData(bRealTime);

            // グリッド背景カラー設定
            this.getGridData(RegistType.Temporary).SetGridRowBackgroundColorRuleFromIndex(1, new List<Color>() { CarisXConst.GRID_ROWS_DEFAULT_COLOR, CarisXConst.GRID_ROWS_COLOR_PATTERN1 });
            this.getGridData(RegistType.Fixed).SetGridRowBackgroundColorRuleFromIndex(1, new List<Color>() { CarisXConst.GRID_ROWS_DEFAULT_COLOR, CarisXConst.GRID_ROWS_COLOR_PATTERN1 });

            String defaultSampleKind = CarisXSubFunction.GetSampleKindGridItemString(SpecimenMaterialType.BloodSerumAndPlasma);
            String defaultCupKind = CarisXSubFunction.GetSpecimenCupKindGridItemString(SpecimenCupKind.Cup);

            // STATラック
            StatRackID rackId = new StatRackID();

            // 一時登録グリッドを初期化
            if (bRealTime == true && 0 == nCurrentIndex && type == RegistType.Temporary)
            {
                this.getDataSource(RegistType.Temporary).Rows.Add(new Object[] { rackId.DispPreCharString                    // ラックID（E000固定）
                                                                                  , 0                                           // ラックポジション（0固定）
                                                                                  , noSaveData[INDEX_GRD_RECEIPTNO]             // レシピ番号
                                                                                  , noSaveData[INDEX_GRD_PATIENTID]             // 検体ID
                                                                                  , noSaveData[INDEX_GRD_SPECIMENTYPE]          // 検体種別
                                                                                  , noSaveData[INDEX_GRD_REGISTERED]            // 分析項目
                                                                                  , noSaveData[INDEX_GRD_MANUALDILUTIONRATIO]   // 手希釈倍率
                                                                                  , noSaveData[INDEX_GRD_COMMENT]               // コメント
                                                                                  , RegistType.Temporary                        // 登録種別（一時登録固定）
                                                                                  , noSaveData[INDEX_GRD_VESSELTYPE]});        // 容器種別
            }
            else
            {
                this.getDataSource(RegistType.Temporary).Rows.Add(new Object[] { rackId.DispPreCharString                    // ラックID（E000固定） 
                                                                                  , 0                                           // ラックポジション（0固定） 
                                                                                  , String.Empty                                // レシピ番号 
                                                                                  , String.Empty                                // 検体ID 
                                                                                  , defaultSampleKind                           // 検体種別 
                                                                                  , String.Empty                                // 分析項目 
                                                                                  , "1"                                         // 手希釈倍率 
                                                                                  , String.Empty                                // コメント 
                                                                                  , RegistType.Temporary                        // 登録種別（一時登録固定） 
                                                                                  , defaultCupKind });                         // 容器種別 
            }
            this.setItemEditable(false, this.getGridData(RegistType.Temporary).Rows.Last());

            // 固定登録グリッドを初期化
            for (Int32 rowIndex = 0; rowIndex < 4; rowIndex++)
            {
                if (bRealTime == true && rowIndex == nCurrentIndex && type == RegistType.Fixed)
                {
                    this.getDataSource(RegistType.Fixed).Rows.Add(new Object[] { rackId.DispPreCharString                    // ラックID（E000固定）
                                                                                  , noSaveData[INDEX_GRD_RACKPOSITION]          // ラックポジション
                                                                                  , noSaveData[INDEX_GRD_RECEIPTNO]             // レシピ番号
                                                                                  , noSaveData[INDEX_GRD_PATIENTID]             // 検体ID
                                                                                  , noSaveData[INDEX_GRD_SPECIMENTYPE]          // 検体種別
                                                                                  , noSaveData[INDEX_GRD_REGISTERED]            // 分析項目
                                                                                  , noSaveData[INDEX_GRD_MANUALDILUTIONRATIO]   // 手希釈倍率
                                                                                  , noSaveData[INDEX_GRD_COMMENT]               // コメント
                                                                                  , RegistType.Fixed                            // 登録種別（固定登録固定）
                                                                                  , noSaveData[INDEX_GRD_VESSELTYPE]            // 容器種別
                    });
                }
                else
                {
                    this.getDataSource(RegistType.Fixed).Rows.Add(new Object[] { rackId.DispPreCharString                    // ラックID（E000固定）
                                                                                  , rowIndex + 1                                // ラックポジション
                                                                                  , String.Empty                                // レシピ番号
                                                                                  , String.Empty                                // 検体ID
                                                                                  , defaultSampleKind                           // 検体種別
                                                                                  , String.Empty                                // 分析項目
                                                                                  , "1"                                         // 手希釈倍率
                                                                                  , String.Empty                                // コメント
                                                                                  , RegistType.Fixed                            // 登録種別（固定登録固定）
                                                                                  , defaultCupKind                              // 容器種別
                    });
                }
                this.setItemEditable(false, this.getGridData(RegistType.Fixed).Rows.Last());
            }
        }

        /// <summary>
        /// 編集中のデータクリア
        /// </summary>
        /// <param name="bRealTime"></param>
        protected void clearExitData(Boolean bRealTime = false)
        {
            if (bRealTime)
            {
                List<int> removeList = new List<int>();
                foreach (var item in existData)
                {
                    // 未保存のデータはReceiptNumberが0であるため、その他は削除対象
                    if (item.Value.ReceiptNumber != 0)
                    {
                        removeList.Add(item.Key);
                    }
                }

                foreach (int remove in removeList)
                {
                    this.existData.Remove(remove);
                }
            }
            else
            {
                this.existData.Clear();
            }
        }

        /// <summary>
        /// カルチャ関連情報設定
        /// </summary>
        /// <remarks>
        /// カルチャ情報に関連する設定を行います。
        /// </remarks>
        protected override void setCulture()
        {
            // 画面タイトル名設定
            this.Text = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_011;

            // 一時登録グリッド列名設定
            this.getGridData(RegistType.Temporary).DisplayLayout.Bands[0].Columns[INDEX_GRD_RACKID].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_000;
            this.getGridData(RegistType.Temporary).DisplayLayout.Bands[0].Columns[INDEX_GRD_RACKID].Hidden = true; // 非表示
            this.getGridData(RegistType.Temporary).DisplayLayout.Bands[0].Columns[INDEX_GRD_RACKPOSITION].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_016;
            this.getGridData(RegistType.Temporary).DisplayLayout.Bands[0].Columns[INDEX_GRD_RACKPOSITION].Hidden = true; // 非表示
            this.getGridData(RegistType.Temporary).DisplayLayout.Bands[0].Columns[INDEX_GRD_RECEIPTNO].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_002;
            this.getGridData(RegistType.Temporary).DisplayLayout.Bands[0].Columns[INDEX_GRD_PATIENTID].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_003;
            this.getGridData(RegistType.Temporary).DisplayLayout.Bands[0].Columns[INDEX_GRD_SPECIMENTYPE].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_004;
            this.getGridData(RegistType.Temporary).DisplayLayout.Bands[0].Columns[INDEX_GRD_REGISTERED].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_005;
            this.getGridData(RegistType.Temporary).DisplayLayout.Bands[0].Columns[INDEX_GRD_MANUALDILUTIONRATIO].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_006;
            this.getGridData(RegistType.Temporary).DisplayLayout.Bands[0].Columns[INDEX_GRD_COMMENT].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_007;
            this.getGridData(RegistType.Temporary).DisplayLayout.Bands[0].Columns[INDEX_GRD_REGISTTYPE].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_014;
            this.getGridData(RegistType.Temporary).DisplayLayout.Bands[0].Columns[INDEX_GRD_REGISTTYPE].Hidden = true; // 非表示
            this.getGridData(RegistType.Temporary).DisplayLayout.Bands[0].Columns[INDEX_GRD_VESSELTYPE].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_017;
            // 固定登録グリッド列名設定
            this.getGridData(RegistType.Fixed).DisplayLayout.Bands[0].Columns[INDEX_GRD_RACKID].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_000;
            this.getGridData(RegistType.Fixed).DisplayLayout.Bands[0].Columns[INDEX_GRD_RACKID].Hidden = true; // 非表示
            this.getGridData(RegistType.Fixed).DisplayLayout.Bands[0].Columns[INDEX_GRD_RACKPOSITION].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_016;
            this.getGridData(RegistType.Fixed).DisplayLayout.Bands[0].Columns[INDEX_GRD_RECEIPTNO].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_002;
            this.getGridData(RegistType.Fixed).DisplayLayout.Bands[0].Columns[INDEX_GRD_PATIENTID].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_003;
            this.getGridData(RegistType.Fixed).DisplayLayout.Bands[0].Columns[INDEX_GRD_SPECIMENTYPE].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_004;
            this.getGridData(RegistType.Fixed).DisplayLayout.Bands[0].Columns[INDEX_GRD_REGISTERED].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_005;
            this.getGridData(RegistType.Fixed).DisplayLayout.Bands[0].Columns[INDEX_GRD_MANUALDILUTIONRATIO].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_006;
            this.getGridData(RegistType.Fixed).DisplayLayout.Bands[0].Columns[INDEX_GRD_COMMENT].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_007;
            this.getGridData(RegistType.Fixed).DisplayLayout.Bands[0].Columns[INDEX_GRD_REGISTTYPE].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_014;
            this.getGridData(RegistType.Fixed).DisplayLayout.Bands[0].Columns[INDEX_GRD_REGISTTYPE].Hidden = true; // 非表示
            this.getGridData(RegistType.Fixed).DisplayLayout.Bands[0].Columns[INDEX_GRD_VESSELTYPE].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_017;

            // コマンドバーアイテム名設定
            this.tlbCommandBar.Tools[SAVE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_001;
            this.tlbCommandBar.Tools[DELETE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_002;
            this.tlbCommandBar.Tools[DELETE_ALL].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_003;
            this.tlbCommandBar.Tools[PRINT].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_004;
            this.tlbCommandBar.Tools[QUERY].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_007;

            // 一時登録グリッド検体種別コンボボックス設定
            this.getGridData(RegistType.Temporary).DisplayLayout.ValueLists[0].ValueListItems.Clear();
            this.getGridData(RegistType.Temporary).DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_008);
            this.getGridData(RegistType.Temporary).DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_009);
            // 固定登録グリッド検体種別コンボボックス設定
            this.getGridData(RegistType.Fixed).DisplayLayout.ValueLists[0].ValueListItems.Clear();
            this.getGridData(RegistType.Fixed).DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_008);
            this.getGridData(RegistType.Fixed).DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_009);

            // 一時登録グリッド容器種別コンボボックス設定
            this.getGridData(RegistType.Temporary).DisplayLayout.ValueLists[1].ValueListItems.Clear();
            this.getGridData(RegistType.Temporary).DisplayLayout.ValueLists[1].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_018);
            this.getGridData(RegistType.Temporary).DisplayLayout.ValueLists[1].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_019);
            this.getGridData(RegistType.Temporary).DisplayLayout.ValueLists[1].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_020);
            // 固定登録グリッド容器種別コンボボックス設定
            this.getGridData(RegistType.Fixed).DisplayLayout.ValueLists[1].ValueListItems.Clear();
            this.getGridData(RegistType.Fixed).DisplayLayout.ValueLists[1].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_018);
            this.getGridData(RegistType.Fixed).DisplayLayout.ValueLists[1].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_019);
            this.getGridData(RegistType.Fixed).DisplayLayout.ValueLists[1].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_020);

            // グリッド名ラベル設定
            this.lblTemporaryRegistration.Text = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_012;
            this.lblFixedRegistration.Text = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_013;
        }


        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 非エラー発生検体削除
        /// </summary>
        /// <remarks>
        /// 登録されている検体で、分析を行っているものの中からエラーが発生していないデータを削除します。
        /// </remarks>
        public void ClearAssayNoErrorData()
        {
            // 一般検体削除情報リスト作成,削除
            var noErrorListNormal = Singleton<InProcessSampleInfoManager>.Instance.GetNoErrorSampleInfo(SampleKind.Priority);
            var removeListNormal = from v in noErrorListNormal
                                   select new Tuple<String, Int32, String>(v.RackId.DispPreCharString, 0, v.SampleId);

            Singleton<SpecimenStatDB>.Instance.DeleteData(removeListNormal.ToList());

            // グリッド内容再取得
            this.loadGridData();
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
            //throw new NotImplementedException();
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// ワークシート問合せ検体ID
        /// </summary>
        private String askWorkSheatSampleID = String.Empty;
        /// <summary>
        /// ワークシート問合せ検体ID
        /// </summary>
        private SpecimenCupKind askWorkSheatCupKind = SpecimenCupKind.Cup;

        /// <summary>
        /// 待機問合せイベント
        /// </summary>
        private ManualResetEvent askWaitEvent = new ManualResetEvent(false);

        /// <summary>
        /// バッチ問合せ処理
        /// </summary>
        /// <remarks>
        /// バッチ問合せ処理を実行します
        /// </remarks>
        /// <param name="data"></param>
        private void askWorkSheetBatch(String SampleID)
        {
            Boolean endAsk = false;

            // ダイアログ表示
            // このダイアログは中断可能かつ、DoEventを含まず、メインスレッドを止めない事。
            Task showWaitDlg = new Task(() =>
           {
               this.waitDialog = DlgMessage.Create(Properties.Resources.STRING_DLG_MSG_208 + "\n" + Properties.Resources.STRING_DLG_MSG_209,
                           "", Properties.Resources.STRING_DLG_TITLE_007, MessageDialogButtons.Cancel);
               this.waitDialog.ShowDialog();

               // キャンセルしたか、問い合せが完了した。
               endAsk = true;

           });

            Action dialogCloser = null;
            dialogCloser = () =>
            {
                if (this.waitDialog != null)
                {
                    if (this.waitDialog.InvokeRequired)
                    {
                        this.waitDialog.Invoke(dialogCloser);
                    }
                    else
                    {
                        this.waitDialog.Close();
                    }

                }
            };

            Task askWorer = new Task(() =>
           {
               Boolean closeDlg = true;

               this.askWaitEvent.Reset();
               // 問合せを行う（別スレッド動作する）
               AskWorkSheetData askData = new AskWorkSheetData();
               Singleton<CarisXSequenceHelperManager>.Instance.Host.HostCommunicationSequence
                   (HostCommunicationSequencePattern.AskWorkSheetToHostBatch | HostCommunicationSequencePattern.STAT, askData, SampleID);

               // 問合せ完了待ち タイムアウトはシーケンス動作内部に設定されている為、無限待ちでよい。
               this.askWaitEvent.WaitOne();

               // キャンセル確認
               if (endAsk)
               {
                   closeDlg = false;
               }

               if (closeDlg)
               {
                   dialogCloser();
               }

               // ・実行完了時、画面へ通知を行う
               Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.AskBatchCompleteSTAT);
           });

            // 問合せシーケンス実行タスク
            showWaitDlg.Start();
            askWorer.Start();

        }

        /// <summary>
        /// ホストからのワークシート受信処理（IRGA用）
        /// </summary>
        /// <param name="obj"></param>
        protected void workSheetFromHostSingle(Object obj)
        {
            SpecimenStatRegistrationGridViewDataSet data = obj as SpecimenStatRegistrationGridViewDataSet;

            List<SpecimenStatRegistrationGridViewDataSet> dataList = new List<SpecimenStatRegistrationGridViewDataSet>();
            dataList.Add(data);
            Singleton<SpecimenStatDB>.Instance.SetIGRAHostDispData(dataList);
            Singleton<SpecimenStatDB>.Instance.CommitSampleInfo();

            if (CarisXSubFunction.AskReagentRemain() == false)
            {
                // デバッグログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog
                                                          , Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                                                          , CarisXLogInfoBaseExtention.Empty
                                                          , CarisX.Properties.Resources.STRING_LOG_MSG_069);
                //this.Cursor = Cursors.Default;
            }
            else
            {
                // 試薬残量確認処理
                CarisXSubFunction.ReagentRemainWarning();
            }
            this.InitializeGridView();
            this.loadGridData();
        }

        /// <summary>
        /// バッチ問合せ応答通知ハンドラ
        /// </summary>
        /// <remarks>
        /// バッチ問合せ応答通知処理を実行します
        /// </remarks>
        /// <param name="obj">ワークシート</param>
        protected void workSheetFromHostByBatch(Object obj)
        {

            try
            {
                AskWorkSheetData askData = obj as AskWorkSheetData;
                // バッチ問い合わせによるワークシート受信

                // タイムアウトしている場合は処理を抜ける
                if (askData.AskTimeOuted)
                {
                    return;
                }

                // 測定項目がない場合は処理しない
                if (askData.FromHostCommand.NumOfMeasItem == 0)
                {
                    return;
                }

                //RackID、ポジションを設定しておく
                StatRackID rackId = new StatRackID();
                askData.FromHostCommand.RackID = rackId.DispPreCharString;
                askData.FromHostCommand.RackPos = CarisXConst.TEMPORARY_STAT_POSITION;

                // ・サンプルIDがない場合エラー
                if (askData.FromHostCommand.SampleID == "")
                {
                    CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.FromHostWorkSheetFormatError
                        , extStr: String.Format(CarisX.Properties.Resources.STRING_LOG_MSG_081, askData.FromHostCommand.RackID, askData.FromHostCommand.RackPos));
                    return;
                }

                // データのチェック
                // ・装置番号が違うとエラー
                // ・自動希釈倍率が範囲外でエラー
                // ・手希釈倍率不可項目が登録されている上で手希釈倍率が1以外ならエラー
                // ・サンプル区分が異なる場合エラー
                if (!ValueChecker.IsValidSpecimenWorkSheet(askData.FromHostCommand, HostSampleType.N))
                {
                    return;
                }
                // ・一時登録にデータがあればエラー
                Int32 registeredReceiptNumber = Singleton<SpecimenStatDB>.Instance.SearchTemporaryData();
                if (registeredReceiptNumber != 0)
                {
                    CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.FromHostWorkSheetAlreadyExists
                        , extStr: String.Format(CarisX.Properties.Resources.STRING_LOG_MSG_076, askData.FromHostCommand.RackID, askData.FromHostCommand.RackPos));
                    return;
                }

                // 外部からの受付番号を保持する。
                if (Singleton<ReceiptNo>.Instance.ThroughExternalCreatedNumber(askData.FromHostCommand.ReceiptNumber))
                {
                    // DBへの登録
                    Singleton<SpecimenStatDB>.Instance.SetAskData(askData, askWorkSheatCupKind);
                    Singleton<SpecimenStatDB>.Instance.CommitSampleInfo();
                }
                else
                {
                    // 既に登録されたことのある受付番号がホストから指定された    
                    CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.FromHostWorkSheetFormatError, extStr: Properties.Resources.STRING_LOG_MSG_082);
                }

            }
            finally
            {
                this.askWaitEvent.Set();
            }

        }

        /// <summary>
        /// FormProtocolSetting変更時処理
        /// </summary>
        /// <param name="value"></param>
        private void onChangeProtocolSetting( Object value )
        {
            // 分析項目ボタンに活性/非活性の変更
            this.protocolIndexToButtonDicEnabled();
            this.loadGridData();
        }

        /// <summary>
        /// 分析方法(急診)変更時
        /// </summary>
        /// <param name="value"></param>
        private void onAssayModeKindChanged( Object value )
        {
            // 分析項目ボタンに活性/非活性の変更
            this.protocolIndexToButtonDicEnabled();
            this.loadGridData();
        }

        /// <summary>
        /// バッチ問合せ完了
        /// </summary>
        /// <remarks>
        /// バッチ問合せ完了による操作制限解除します
        /// </remarks>
        /// <param name="value"></param>
        private void askBatchComplete(Object value)
        {
            // バッチ問合せ完了による操作制限解除
            FormBase.AllFormItemEnableStatusRestore();

            // 残量問合せ処理
            if (CarisXSubFunction.AskReagentRemain() == false)
            {
                // デバッグログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, Properties.Resources.STRING_LOG_MSG_069);
            }
            else
            {
                // 試薬残量確認処理
                CarisXSubFunction.ReagentRemainWarning();
            }

            // グリッドへの表示
            this.InitializeGridView();
            this.loadGridData();

            // 一時登録データがある場合、かつ分析中の場合
            var temporaryData = dispDataOriginal.Where(v => v.RackPosition == CarisXConst.TEMPORARY_STAT_POSITION).FirstOrDefault();
            if (temporaryData != null && (Singleton<SystemStatus>.Instance.Status == SystemStatusKind.Assay))
            {
                // 一時登録データの検体IDを取得する
                String temporaryPatientId = temporaryData.PatientID;

                List<Int32> protocolList = new List<Int32>();

                foreach (Tuple<int?, int?, int?> regist in temporaryData.Registered)
                {
                    // 分析項目Indexをチェック
                    if (regist.Item1 == null)
                    {
                        // nullの場合、飛ばす
                        continue;
                    }
                    protocolList.Add((int)regist.Item1);
                }

                CarisXSubFunction.ShowCheckStatMeasurableModule(temporaryPatientId, protocolList);
            }
        }

        /// <summary>
        /// クエリー
        /// </summary>
        /// <remarks>
        /// バッチ問合せ処理を実行します
        /// </remarks>
        private void query()
        {
            //ワークシート問合せ画面を表示する
            using (var Dlg = new DlgAskWorkSheatSTAT())
            {
                if (Dlg.ShowDialog() != DialogResult.OK)
                {
                    // 設定しない場合ここで終了する
                    return;
                }
                askWorkSheatSampleID = Dlg.SampleID;
                askWorkSheatCupKind = Dlg.CupKind;
            }

            // 編集中のデータは削除する
            this.editData.Clear();

            // バッチ問合せを行う(処理は非同期で実行される。この関数ではブロックされない）
            this.askWorkSheetBatch(askWorkSheatSampleID);

            // バッチ問合せ完了まで画面を非活性
            FormBase.AllFormItemEnableChange(false);
        }

        /// <summary>
        /// 問合せ待ちダイアログ
        /// </summary>
        DlgMessage waitDialog = null;

        #region _コマンドバー_

        /// <summary>
        /// コマンドボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// コマンドボタンクリック処理を実行します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void tlbCommandBar_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            if (this.tlbCommandBar.ActiveTool == e.Tool)
            {
                switch (e.Tool.Key)
                {
                    case SAVE:                  // 保存
                        this.saveData();
                        break;
                    case DELETE:                // 削除
                        this.deleteData();
                        break;
                    case DELETE_ALL:            // 全て削除
                        this.deleteAllData();
                        break;
                    case PRINT:                 // 印刷
                        this.printData();
                        break;
                    case QUERY:                 // ホスト問合せ
                        this.query();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// すべての登録情報の保存
        /// </summary>
        /// <remarks>
        /// グリッドの編集によるデータ編集内容の、DB登録を行います。
        /// </remarks>
        private void saveData()
        {
            // 操作履歴登録：登録実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist
                                                      , Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                                                      , CarisXLogInfoBaseExtention.Empty
                                                      , new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_009 });

            // 一時登録が編集中状態における失敗ケース
            if ((this.getGridData(RegistType.Temporary).ActiveCell != null)
                && (this.getGridData(RegistType.Temporary).ActiveCell.IsInEditMode == true))
            {
                // 編集中セルからフォーカスを抜ける。
                // 変更中セル内容が確定できない場合、中断する。
                this.getGridData(RegistType.Temporary).ActiveCell.Activated = false;
                if (this.getGridData(RegistType.Temporary).ActiveCell != null)
                {
                    return;
                }
            }

            // 固定登録が編集中状態における失敗ケース
            if ((this.getGridData(RegistType.Fixed).ActiveCell != null)
                && (this.getGridData(RegistType.Fixed).ActiveCell.IsInEditMode == true))
            {
                // 編集中セルからフォーカスを抜ける。
                // 変更中セル内容が確定できない場合、中断する。
                this.getGridData(RegistType.Fixed).ActiveCell.Activated = false;
                if (this.getGridData(RegistType.Fixed).ActiveCell != null)
                {
                    return;
                }
            }

            // 編集データの有無チェック
            if (this.editData.Count != 0)
            {
                // 分析項目の登録されていないデータは消す。
                var removeList = from v in this.editData
                                 where v.Value.Registered.Count == 0
                                 || v.Value.Registered.All((vv) => vv.Item1 == null && vv.Item2 == null)
                                 select v.Key;

                foreach (UltraDataRow remove in removeList.ToArray())
                {
                    this.editData.Remove(remove);
                }

                // ここまでに編集データチェック
                Singleton<SpecimenStatDB>.Instance.SetDispData(this.editData.Values.ToList());
                Singleton<SpecimenStatDB>.Instance.CommitSampleInfo();

                // 試薬残量確認処理追加

                // 残量問合せ処理
                if (CarisXSubFunction.AskReagentRemain() == false)
                {
                    // デバッグログ出力
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                        , CarisXLogInfoBaseExtention.Empty, CarisX.Properties.Resources.STRING_LOG_MSG_069);
                }
                else
                {
                    // 試薬残量確認処理
                    CarisXSubFunction.ReagentRemainWarning();
                }

                // DBレコードマッピングデータ作成( データの保持は、アプリケーションが起動している間ずっと、なのでクリアはされない )
                foreach (KeyValuePair<UltraDataRow, SpecimenStatRegistrationGridViewDataSet> data in this.editData)
                {
                    Tuple<String, Int32, String> linkKey = new Tuple<String, Int32, String>(data.Value.RackID.DispPreCharString, data.Value.RackPosition, data.Value.PatientID);
                    this.dbLinkData[linkKey] = data.Value.RackPosition;
                }

                // 編集データ削除
                this.editData.Clear();

                // 保存データを読み直す
                RegistType selectedRegistType = RegistType.Temporary;
                var selected = this.getGridData(RegistType.Temporary).SearchSelectRow();
                Int32 currentRowIndex = 0;

                if (selected.Count != 0)
                {
                    currentRowIndex = selected.First().Index;
                    selectedRegistType = RegistType.Temporary;

                    //一時登録の場合、STAT状態通知コマンドはSTAT測定可能モジュールの確認ダイアログ呼出直前に送信する
                }
                else
                {
                    selected = this.getGridData(RegistType.Fixed).SearchSelectRow();
                    if (selected.Count != 0)
                    {
                        currentRowIndex = selected.First().Index;
                        selectedRegistType = RegistType.Fixed;

                        int moduleId = Int32.Parse(selected.First().Cells[INDEX_GRD_RACKPOSITION].Text);
                        int moduleIndex = CarisXSubFunction.ModuleIDToModuleIndex(moduleId);

                        // STAT状態が受付可の場合
                        if (Singleton<SystemStatus>.Instance.ModuleSTATStatus[moduleIndex] == STATStatus.Acceptable)
                        {
                            // STAT状態通知コマンドをスレーブへ送信
                            SlaveCommCommand_0491 cmd0491 = new SlaveCommCommand_0491();
                            cmd0491.Request = STATStatusRequest.WaitSWPress;                // SW押下待ち
                            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((int)moduleIndex, cmd0491);
                        }

                    }
                }

                this.InitializeGridView();
                this.loadGridData(currentRowIndex, false, selectedRegistType);

                // 一時登録データがある場合、かつ分析中の場合
                if ((selectedRegistType == RegistType.Temporary)
                    && (selected.Count != 0)
                    && (Singleton<SystemStatus>.Instance.Status == SystemStatusKind.Assay))
                {
                    if (this.existData.Count == 0)
                    {
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
                                                                  , String.Format("STAT測定可能モジュールの確認前に一時登録が削除されました"));
                        return;
                    }

                    // 一時登録データの検体IDを取得する
                    String registedTemporaryPatientId = this.existData[0].PatientID;

                    List<Int32> protocolList = new List<Int32>();

                    foreach (Tuple<int?, int?, int?> regist in this.existData[0].Registered)
                    {
                        // 分析項目Indexをチェック
                        if (regist.Item1 == null)
                        {
                            // nullの場合、飛ばす
                            continue;
                        }
                        protocolList.Add((int)regist.Item1);
                    }

                    CarisXSubFunction.ShowCheckStatMeasurableModule(registedTemporaryPatientId, protocolList);
                }

                // Form共通の編集中フラグOFF
                FormChildBase.IsEdit = false;
            }
        }

        /// <summary>
        /// 選択中登録情報の削除
        /// </summary>
        /// <remarks>
        /// 選択されたデータの編集状態及びDB登録情報を削除します。
        /// </remarks>
        private void deleteData()
        {
            CustomGrid deleteGrid = null;
            UltraDataSource deleteDataSource = null;
            RegistType selectedRegistType = RegistType.Temporary;

            if (this.getGridData(RegistType.Temporary).SearchSelectRow().Count != 0)
            {
                deleteGrid = this.getGridData(RegistType.Temporary);
                deleteDataSource = this.getDataSource(RegistType.Temporary);
                selectedRegistType = RegistType.Temporary;
            }
            else if (this.getGridData(RegistType.Fixed).SearchSelectRow().Count != 0)
            {
                deleteGrid = this.getGridData(RegistType.Fixed);
                deleteDataSource = this.getDataSource(RegistType.Fixed);
                selectedRegistType = RegistType.Fixed;
            }

            if (deleteGrid == null)
            {
                // 選択行がないため、削除処理なし
                return;
            }

            // 操作履歴：削除実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_003 });

            // 削除メッセージ作成
            string dlgMessage = CarisX.Properties.Resources.STRING_DLG_MSG_019;

            // 編集中かどうか判断して、表示メッセージを変える
            if (this.editData.Count != 0)
            {
                // 編集中のデータも消去されます。消去を行いますか？ 
                dlgMessage = CarisX.Properties.Resources.STRING_DLG_MSG_024;
            }

            DialogResult dlgResult = DlgMessage.Show(dlgMessage, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel);
            if (dlgResult != DialogResult.OK)
            {
                // 操作履歴：削除キャンセル
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_004 });
                return;
            }

            // 削除データリスト作成
            List<Tuple<String, Int32, String>> delList = new List<Tuple<String, Int32, String>>();
            foreach (UltraGridRow val in deleteGrid.SearchSelectRow())
            {
                UltraDataRow dataRow = deleteDataSource.Rows[val.Index];

                if ((String)(dataRow[STRING_GRD_RECEIPTNO]) != String.Empty)
                {
                    // 受付番号リストを削除リストとする。
                    Int32 rackPos = 0;
                    Int32.TryParse((String)dataRow[STRING_GRD_RACKPOSITION], out rackPos);
                    CarisXIDString rackID = (String)dataRow[STRING_GRD_RACKID];
                    delList.Add(new Tuple<String, Int32, String>(rackID.DispPreCharString, rackPos, (String)dataRow[STRING_GRD_PATIENTID]));
                }
            }

            // 編集データクリア
            this.editData.Clear();

            // 削除・コミット・グリッド表示内容再生成
            Singleton<SpecimenStatDB>.Instance.DeleteData(delList);
            Singleton<SpecimenStatDB>.Instance.CommitSampleInfo();

            // マッピングデータを同期する。
            foreach (var delDat in delList)
            {
                this.dbLinkData.Remove(delDat);
            }

            // グリッド初期化
            this.InitializeGridView();

            // グリッドデータ読込
            this.loadGridData(registType: selectedRegistType);

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// すべての登録情報の削除
        /// </summary>
        /// <remarks>
        /// 全ての編集状態・関連のDB登録情報を削除します。
        /// </remarks>
        private void deleteAllData()
        {
            // 操作履歴登録：全消去実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_010 });

            // 全消去します。よろしいですか？
            DialogResult dlgResult = DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_001, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel);
            if (dlgResult != DialogResult.OK)
            {
                // 操作履歴登録：全消去キャンセル
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_011 });
                return;
            }

            Singleton<SpecimenStatDB>.Instance.DeleteAll();
            Singleton<SpecimenStatDB>.Instance.CommitSampleInfo();

            // グリッド初期化
            this.InitializeGridView();

            // グリッドデータ読込
            this.loadGridData();

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// 登録情報の印刷出力
        /// </summary>
        /// <remarks>
        /// 登録情報の印刷出力処理を実行します
        /// </remarks>
        private void printData()
        {
            // 操作履歴登録：印刷実行
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_005 });

            // 確認ダイアログ表示
            TargetRange outputRange = DlgTargetSelectRange.Show();

            List<SpecimenStatRegistrationGridViewDataSet> prtData = new List<SpecimenStatRegistrationGridViewDataSet>();
            switch (outputRange)
            {
                case TargetRange.None:
                    // 操作履歴登録：印刷キャンセル                    
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_006 });
                    return;
                case TargetRange.All:
                    // 印刷範囲：全て                   
                    prtData = dispDataOriginal.ToList();
                    break;
                case TargetRange.Specification:
                    // 印刷範囲：範囲指定                　　
                    var selected = from v in getGridData(RegistType.Temporary).SearchSelectRow()
                                   select new Tuple<String, Int32, String>(
                                      (String)v.Cells[STRING_GRD_RACKID].Value,
                                      CarisXSubFunction.Int32InnerTryParse((String)v.Cells[STRING_GRD_RACKPOSITION].Value),
                                      (String)v.Cells[STRING_GRD_PATIENTID].Value
                                   );
                    var dispDic = this.dispDataOriginal.ToDictionary((v) => new Tuple<String, Int32, String>(v.RackID.DispPreCharString, v.RackPosition, v.PatientID));
                    foreach (var row in selected)
                    {
                        if (dispDic.ContainsKey(row))
                        {
                            prtData.Add(dispDic[row]);
                        }
                    }
                    break;
            }

            if (prtData.Count == 0)
            {
                // 印刷データなし・メッセージ表示
                DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_064, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_003, MessageDialogButtons.OK);
            }
            else
            {
                enabledFlag = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.IsProtocolEnabledChangedInEmergencyMode();

                // 対象データ取得
                List<SpecimenRegistrationReportData> rptData = new List<SpecimenRegistrationReportData>();
                SpecimenRegistrationPrint prt = new SpecimenRegistrationPrint();

                foreach (var row in prtData)
                {
                    foreach (var anlytes in row.Registered)
                    {
                        SpecimenRegistrationReportData dt = new SpecimenRegistrationReportData();
                        dt.RackID = row.RackID.ToString();
                        dt.RackPosition = row.RackPosition;
                        dt.ReceiptNumber = row.ReceiptNumber;
                        dt.PatientID = row.PatientID;
                        dt.SpecimenType = row.SpecimenType.ToTypeString();
                        dt.ManualDil = row.ManualDil;
                        dt.Comment = row.Comment;
                        dt.PrintDateTime = DateTime.Now.ToDispString();

                        if (anlytes.Item1 != null)
                        {
                            var protocal = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex((Int32)anlytes.Item1);

                            // 全スレーブで急診無しかつ急診使用ありの分析項目は印刷しない
                            if (( enabledFlag == true ) && ( protocal.UseEmergencyMode == true ))
                            {
                                continue;
                            }

                            dt.ProtoName = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex((int)anlytes.Item1).ProtocolName;
                            dt.AutoDil = (Int32)anlytes.Item2;
                            rptData.Add(dt);
                        }
                    }
                }

                // 印刷実行
                Boolean ret = prt.Print(rptData);
            }
        }

        #endregion

        /// <summary>
        /// 検体登録グリッド表示初期化
        /// </summary>
        /// <remarks>
        /// 検体登録グリッド表示を初期化します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void grdSpecimenStatRegistration_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            // 測定テーブルを拡大(表示)
            this.splAnalyteTable.Collapsed = false;
        }

        /// <summary>
        /// 一時登録グリッドのセル、行、列選択状態変更前イベント
        /// </summary>
        /// <remarks>
        /// 測定テーブルを拡大(表示)します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenStatRegistration_BeforeSelectChange(object sender, BeforeSelectChangeEventArgs e)
        {
            // 測定テーブルを拡大(表示)
            this.splAnalyteTable.Collapsed = false;

            System.Diagnostics.Debug.WriteLine(String.Format("{0}:grdSpecimenStatRegistration_BeforeSelectChange", DateTime.Now.ToLongTimeString()));
        }

        /// <summary>
        /// 測定テーブルの閉じるボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 測定テーブルを縮小(非表示)します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void analysisSettingPanel1_Closed()
        {
            // 測定テーブルを縮小(非表示)
            this.splAnalyteTable.Collapsed = true;
        }

        /// <summary>
        /// 許容サンプル比較
        /// </summary>
        /// <remarks>
        /// サンプル種別をプロトコル設定の許容サンプル種別に変換します
        /// </remarks>
        /// <param name="sampleType"></param>
        /// <param name="protocolAllowed"></param>
        /// <returns></returns>
        private Boolean isAllowSample(SpecimenMaterialType sampleType, MeasureProtocol.SampleTypeKind protocolAllowed)
        {
            Boolean contain = false;

            // サンプル種別をプロトコル設定の許容サンプル種別に変換
            MeasureProtocol.SampleTypeKind sampleKind;
            switch (sampleType)
            {
                case SpecimenMaterialType.BloodSerumAndPlasma:
                    sampleKind = MeasureProtocol.SampleTypeKind.SerumOrPlasma;
                    break;
                case SpecimenMaterialType.Urine:
                    sampleKind = MeasureProtocol.SampleTypeKind.Urine;
                    break;
                default:
                    // デフォルトは画面と同じにする。
                    sampleKind = MeasureProtocol.SampleTypeKind.SerumOrPlasma;
                    break;
            }

            contain = (protocolAllowed & sampleKind) != MeasureProtocol.SampleTypeKind.None;

            return contain;
        }

        /// <summary>
        /// 分析項目選択状態変更前イベント
        /// </summary>
        /// <remarks>
        /// 選択した分析項目が、現在の入力情報に違反する場合にキャンセルします
        /// </remarks>
        /// <param name="protocolIndex"></param>
        /// <param name="data"></param>
        private void analysisSettingPanel_ProtocolCheckChanging(int protocolIndex, Controls.AnalysisSettingPanel.AnalisisSettingPanelSelectChangingData data)
        {
            // 選択した分析項目が、現在の入力情報に違反する場合にキャンセルする。
            var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(protocolIndex);
            RegistType selectedRegistType = RegistType.Temporary;
            var selected = this.getGridData(RegistType.Temporary).SearchSelectRow();
            if (selected.Count == 0)
            {
                selected = this.getGridData(RegistType.Fixed).SearchSelectRow();
                selectedRegistType = RegistType.Fixed;
            }

            if (selected.Count != 0)
            {
                foreach (var selectedRow in selected)
                {

                    ///
                    /// 登録時にコメントに文字列「ROMA」がある場合、ROMA計算が実行される。
                    ///
                    if (data.IsCalcRoma == true)
                    {
                        this.getDataSource(selectedRegistType).Rows[selectedRow.Index][STRING_GRD_COMMENT] = "ROMA";
                    }
                    else
                    {
                        if (this.getDataSource(selectedRegistType).Rows[selectedRow.Index][STRING_GRD_COMMENT].ToString().Contains("ROMA") &&
                            protocolIndex == 15 || protocolIndex == 25)
                        {
                            this.getDataSource(selectedRegistType).Rows[selectedRow.Index][STRING_GRD_COMMENT] = this.getDataSource(selectedRegistType).Rows[selectedRow.Index][STRING_GRD_COMMENT].ToString().Replace("ROMA", "");
                        }
                    }

                    var viewData = this.createGridViewDataSetFromRow(this.getDataSource(selectedRegistType).Rows[selectedRow.Index]);

                    if (!this.isAllowSample(viewData.SpecimenType, protocol.SampleKind))
                    {
                        // エラーメッセージを表示してキャンセル
                        DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_159, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                        data.Cancel = true;
                        break;
                    }

                    // 手希釈倍率使用状況の確認をする
                    Int32 registCount = viewData.Registered.Count((v) => v.Item1.HasValue);

                    // 一つ目の分析項目設定の場合、手希釈が既に数値入力されているかどうか見て判定する。
                    if ((viewData.ManualDil != 1) && !protocol.UseManualDil)
                    {
                        // エラーメッセージ表示せずキャンセル
                        data.Cancel = true;
                        break;
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewData"></param>
        /// <returns></returns>
        private Boolean isNoUseManualDilRow(SpecimenStatRegistrationGridViewDataSet viewData)
        {
            // 登録内容に一つでも手希釈選択があればtrueを返す
            Boolean result = viewData.Registered.Any((v) => v.Item1.HasValue && !Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(v.Item1.Value).UseManualDil);
            return result;
        }

        /// <summary>
        /// 分析項目登録パネルProtocolCheckChangedイベント処理
        /// </summary>
        /// <param name="protocolIndex"></param>
        /// <param name="selected"></param>
        private void analysisSettingPanel_ProtocolCheckChanged(Int32 protocolIndex, Boolean selected)
        {
            this.analysisSettingPanel_ProtocolCheckChanged(protocolIndex, this.analysisSettingPanel.Dilution, this.analysisSettingPanel.MeasTimes, selected);
        }

        /// <summary>
        /// 分析項目選択状態変更時イベント
        /// </summary>
        /// <remarks>
        /// 選択した分析項目で編集状態を更新します
        /// </remarks>
        /// <param name="protocolIndex">分析項目インデックス</param>
        /// <param name="selected">選択状態</param>
		private void analysisSettingPanel_ProtocolCheckChanged(Int32 protocolIndex, Int32 dil, Int32 meastimes, Boolean selected)
        {
            CustomGrid checkGrid = null;
            UltraDataSource checkDataSource = null;

            if (this.getGridData(RegistType.Temporary).SearchSelectRow().Count != 0)
            {
                checkGrid = this.getGridData(RegistType.Temporary);
                checkDataSource = this.getDataSource(RegistType.Temporary);
            }
            else if (this.getGridData(RegistType.Fixed).SearchSelectRow().Count != 0)
            {
                checkGrid = this.getGridData(RegistType.Fixed);
                checkDataSource = this.getDataSource(RegistType.Fixed);
            }

            if (checkGrid != null)
            {
                foreach (var selectedRow in checkGrid.SearchSelectRow())
                {
                    UltraDataRow currentRow = checkDataSource.Rows[selectedRow.Index];

                    SpecimenStatRegistrationGridViewDataSet data = new SpecimenStatRegistrationGridViewDataSet();
                    if (this.editData.ContainsKey(currentRow))
                    {
                        data = this.editData[currentRow];
                    }
                    else if (this.existData.ContainsKey(Int32.Parse(currentRow[STRING_GRD_RACKPOSITION].ToString())))
                    {
                        data = this.existData[Int32.Parse(currentRow[STRING_GRD_RACKPOSITION].ToString())];
                    }
                    else
                    {
                        data.RackPosition = Int32.Parse(currentRow[STRING_GRD_RACKPOSITION].ToString());
                    }

                    List<Tuple<Int32?, Int32?, Int32?>> list = data.Registered;
                    Tuple<Int32?, Int32?, Int32?> searched = null;

                    Int32 listIndex = 0;
                    // 選択した項目が含まれているか検索
                    foreach (Tuple<Int32?, Int32?, Int32?> inf in list)
                    {
                        if (inf.Item1 == protocolIndex)
                        {
                            searched = inf;
                            break;
                        }
                        listIndex++;
                    }
                    // 選択解除であれば削除
                    if (!selected)
                    {
                        if (searched != null)
                        {
                            // 選択解除されたら空白を入れる。
                            list[list.FindIndex((v) => v == searched)] = new Tuple<Int32?, Int32?, Int32?>(null, null, null);
                            //                        list.Remove( searched );

                        }
                    }
                    else
                    {
                        if (searched == null)
                        {
                            // 新しく選択された
                            list.Add(new Tuple<Int32?, Int32?, Int32?>(protocolIndex, dil, meastimes));
                        }
                        else
                        {
                            // 変更された
                            list[listIndex] = new Tuple<Int32?, Int32?, Int32?>(protocolIndex, dil, meastimes);
                        }

                    }

                    this.editData[currentRow] = data;

                    // 編集状態を更新
                    this.updateGridEditData(currentRow);
                }

                // Form共通の編集中フラグON
                FormChildBase.IsEdit = true;
            }
        }

        /// <summary>
        /// DBのデータに対して、編集データを重ね合わせた表示用データ(グリッドRowインデックス->データ)
        /// </summary>
        /// <remarks>
        /// 編集状態更新します
        /// </remarks>
        Dictionary<Int32, SpecimenStatRegistrationGridViewDataSet> existData = new Dictionary<Int32, SpecimenStatRegistrationGridViewDataSet>();

        /// <summary>
        /// グリッドデータ更新
        /// </summary>
        /// <remarks>
        /// 編集状態に合わせてグリッドの関連データを更新します。
        /// </remarks>
        /// <param name="row">編集行</param>
        /// <returns>True:成功 False:失敗</returns>
        private Boolean updateGridEditData(UltraDataRow row)
        {
            Boolean result = true;
            SpecimenStatRegistrationGridViewDataSet setData = this.createGridViewDataSetFromRow(row);

            // 編集状態更新（処理が重い場合、setEditDataにて1レコードずつやる）
            foreach (KeyValuePair<UltraDataRow, SpecimenStatRegistrationGridViewDataSet> dat in this.editData)
            {
                this.existData[dat.Value.RackPosition] = dat.Value;
            }

            // 手希釈編集設定
            this.setAllowManualDilEdit(row, setData);

            // 検体チェック
            if (this.editData.ContainsKey(row))
            {
                var editedData = this.createGridViewDataSetFromRow(row);
                var beforeData = this.editData[row];
                if (beforeData.SpecimenType != 0 && editedData.SpecimenType != beforeData.SpecimenType)
                {
                    // もし種別を変更された場合、クリアを行う
                    // クリアする場合、分析項目を初期化して設定
                    editedData.Registered.Clear();
                    this.editData[row] = editedData;
                    this.existData[this.editData[row].RackPosition] = this.editData[row];
                    // 測定テーブル表示更新
                    this.analysisSettingPanel.ReLoadAnalyteInformation();
                }
                else
                {
                    this.editData[row] = this.createGridViewDataSetFromRow(row);
                    this.existData[this.editData[row].RackPosition] = this.editData[row];
                }

            }
            else
            {
                // IDが編集データ上に重複しない場合登録。
                this.editData[row] = this.createGridViewDataSetFromRow(row);
                this.existData[this.editData[row].RackPosition] = this.editData[row];
            }

            return result;
        }

        /// <summary>
        /// 手希釈編集設定
        /// </summary>
        /// <param name="row"></param>
        /// <param name="setData"></param>
        private void setAllowManualDilEdit(UltraDataRow row, SpecimenStatRegistrationGridViewDataSet setData)
        {
            int rackPosition = Int32.Parse(row[STRING_GRD_RACKPOSITION].ToString());

            RegistType registType = (RegistType)Enum.Parse(typeof(RegistType), row[STRING_GRD_REGISTTYPE].ToString(), true);
            int rowIndex = rackPosition == 0 ? 0 : rackPosition - 1;

            // 手希釈倍率使用による編集可否の設定
            Int32 setCount = setData.Registered.Count((v) => v.Item1.HasValue);
            if (setCount == 0)
            {
                // 何も設定されていなければ、編集可
                this.getGridData(registType).Rows[rowIndex].Cells[STRING_GRD_MANUALDILUTIONRATIO].Activation = Activation.AllowEdit;
            }
            else
            {
                // 手希釈不使用があれば編集不可
                if (this.isNoUseManualDilRow(setData))
                {
                    this.getGridData(registType).Rows[rowIndex].Cells[STRING_GRD_MANUALDILUTIONRATIO].Activation = Activation.NoEdit;
                }
                else
                {
                    this.getGridData(registType).Rows[rowIndex].Cells[STRING_GRD_MANUALDILUTIONRATIO].Activation = Activation.AllowEdit;
                }
            }
        }

        /// <summary>
        /// 選択行情報からグリッド表示データ作成
        /// </summary>
        /// <remarks>
        /// 選択行情報からグリッド表示データ作成します
        /// </remarks>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private SpecimenStatRegistrationGridViewDataSet createGridViewDataSetFromRow(UltraDataRow dataRow)
        {
            SpecimenStatRegistrationGridViewDataSet data = new SpecimenStatRegistrationGridViewDataSet();

            // ラックID
            data.RackID = dataRow[STRING_GRD_RACKID] == DBNull.Value ? String.Empty : (String)dataRow[STRING_GRD_RACKID];

            // ラックポジション
            if (dataRow[STRING_GRD_RACKPOSITION] == null)
            {
                data.RackPosition = 0;
            }
            else
            {
                Int32.TryParse(dataRow[STRING_GRD_RACKPOSITION] == DBNull.Value ? "0" : (String)dataRow[STRING_GRD_RACKPOSITION], out data.RackPosition);
            }

            // 受付番号：登録タイミングで発番する
            // this.editData[row].ReceiptNumber = row[STRING_GRD_RECEIPTNO] = val.ReceiptNumber;

            // 検体ID
            if (dataRow[STRING_GRD_PATIENTID] == null || dataRow[STRING_GRD_PATIENTID] is DBNull)
                data.PatientID = String.Empty;
            else
                data.PatientID = (String)dataRow[STRING_GRD_PATIENTID];

            // 検体種別：必ず何れかの値が入力されている（初期値で設定されている）
            data.SpecimenType = CarisXSubFunction.GetSampleKindFromGridItemString((String)dataRow[STRING_GRD_SPECIMENTYPE]);

            // 測定項目：編集データとしてRegisterdを扱わない（DBからの読込で存在する場合のみグリッドへ表示される）
            if (this.editData.ContainsKey(dataRow))
            {
                data.Registered = this.editData[dataRow].Registered;
            }
            else if (this.existData.ContainsKey(data.RackPosition))
            {
                data.Registered = this.existData[data.RackPosition].Registered;
            }

            // 手希釈倍率
            data.ManualDil = Convert.ToInt32(dataRow[STRING_GRD_MANUALDILUTIONRATIO]);

            // コメント
            if (dataRow[STRING_GRD_COMMENT] == null || dataRow[STRING_GRD_COMMENT] is DBNull)
                data.Comment = String.Empty;
            else
                data.Comment = (String)dataRow[STRING_GRD_COMMENT];

            // 登録種別
            data.RegistType = (RegistType)Enum.Parse(typeof(RegistType), dataRow[STRING_GRD_REGISTTYPE].ToString(), true);

            // 容器種別
            data.VesselType = CarisXSubFunction.GetSpecimenCupKindFromGridItemString((String)dataRow[STRING_GRD_VESSELTYPE]);

            return data;
        }

        #endregion

        #region IUISetting メンバー
        /// <summary>
        /// UI設定項目の型取得
        /// </summary>
        /// <remarks>
        /// UI設定項目の型取得します
        /// </remarks>
        /// <returns></returns>
        public Type GetSettingsType()
        {
            return typeof(FormSpecimenResistrationSettings);
        }
        /// <summary>
        /// UI設定項目の取得
        /// </summary>
        /// <remarks>
        /// UI設定項目を取得します
        /// </remarks>
        /// <returns></returns>
        public object GetSettings()
        {
            return this.FormSpecimenStatRegistrationSettings;
        }
        /// <summary>
        /// UI設定項目の設定
        /// </summary>
        /// <remarks>
        /// UI設定項目を設定します
        /// </remarks>
        /// <param name="setting"></param>
        public void SetSettings(object setting)
        {
            this.FormSpecimenStatRegistrationSettings = (FormSpecimenResistrationSettings)setting;
        }

        #endregion

        /// <summary>
        /// 検体登録グリッド編集モード終了前イベント
        /// </summary>
        /// <remarks>
        /// 検体登録グリッド編集確定します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenStatRegistration_BeforeExitEditMode(object sender, BeforeExitEditModeEventArgs e)
        {
            // 編集セルからのフォーカス抜けをキャンセルするエラー処理はここ
            if (!e.CancellingEditOperation)
            {
                var grd = sender as UltraGrid;
                if (grd.ActiveCell != null)
                {
                    this.updateGridEditData(this.getDataSource(RegistType.Temporary).Rows[grd.ActiveCell.Row.Index]);
                }
            }
            System.Diagnostics.Debug.WriteLine(String.Format("{0}:grdSpecimenStatRegistration_BeforeExitEditMode", DateTime.Now.ToLongTimeString()));
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, "grdSpecimenStatRegistration_BeforeExitEditMode");
        }

        /// <summary>
        /// 編集データ設定
        /// </summary>
        /// <remarks>
        /// 検体登録グリッドに編集データを反映します
        /// </remarks>
        /// <param name="columnName"></param>
        /// <param name="rowIndex"></param>
        /// <param name="editData"></param>
        private void setEditdata(String columnName, Int32 rowIndex, String editData, RegistType registType)
        {
            if (rowIndex < 0)
            {
                return;
            }

            // 編集行データ取得
            UltraDataRow editRow = this.getDataSource(registType).Rows[this.getGridData(registType).Rows[rowIndex].Index];
            switch (columnName)
            {
                // 編集不可能列
                case STRING_GRD_RECEIPTNO:
                case STRING_GRD_REGISTERED:
                case STRING_GRD_RACKID:
                case STRING_GRD_RACKPOSITION:
                case STRING_GRD_REGISTTYPE:
                    {
                        break;
                    }

                // 編集可能列
                case STRING_GRD_PATIENTID:
                case STRING_GRD_SPECIMENTYPE:
                case STRING_GRD_MANUALDILUTIONRATIO:
                case STRING_GRD_COMMENT:
                case STRING_GRD_VESSELTYPE:
                    {
                        // 編集内容を更新
                        this.updateGridEditData(editRow);
                        break;
                    }
                default:
                    break;
            }
        }

        /// <summary>
        /// 一時登録グリッド編集モード終了後イベント
        /// </summary>
        /// <remarks>
        /// 一時登録グリッド編集データ確定します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenStatRegistration_AfterExitEditMode(object sender, EventArgs e)
        {
            UltraGridCell currentCell = ((CustomGrid)sender).ActiveCell;
            if (currentCell != null)
            {
                String columnName = currentCell.Column.Key;
                Int32 rowIndex = currentCell.Row.Index;
                this.setEditdata(columnName, rowIndex, currentCell.Text, RegistType.Temporary);
            }
            System.Diagnostics.Debug.WriteLine(String.Format("{0}:grdSpecimenStatRegistration_AfterExitEditMode", DateTime.Now.ToLongTimeString()));
        }

        /// <summary>
        /// セルデータエラーイベント
        /// </summary>
        /// <remarks>
        /// エラーイベント設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void grdSpecimenStatRegistration_CellDataError(object sender, CellDataErrorEventArgs e)
        {
            e.RaiseErrorEvent = false;
            System.Diagnostics.Debug.WriteLine(String.Format("{0}:grdSpecimenStatRegistration_CellDataError", DateTime.Now.ToLongTimeString()));
        }

        /// <summary>
        /// 一時登録グリッド選択変更後イベント
        /// </summary>
        /// <remarks>
        /// 一時登録グリッド選択変更後デバッグログ出力します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenStatRegistration_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            if (typeof(UltraGridRow) == e.Type)
            {
                if (this.grdSpecimenStatRegistration.Selected.Rows.Count != 0)
                    // 固定登録グリッドの選択解除
                    this.clearGridSelected(RegistType.Fixed);
            }

            System.Diagnostics.Debug.WriteLine(String.Format("{0}:grdSpecimenStatRegistration_AfterSelectChange", DateTime.Now.ToLongTimeString()));
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, "grdSpecimenStatRegistration_AfterSelectChange");
        }

        /// <summary>
        /// 一時登録グリッド行アクティブ後イベント
        /// </summary>
        /// <remarks>
        /// 保持されている選択項目を分析項目パネルに設定します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenStatRegistration_AfterRowActivate(object sender, EventArgs e)
        {
            // 保持されている選択項目を分析項目パネルに設定する。
            this.setAnalytesPanel((CustomGrid)sender);

            System.Diagnostics.Debug.WriteLine(String.Format("{0}:grdSpecimenStatRegistration_AfterRowActivate", DateTime.Now.ToLongTimeString()));
        }

        /// <summary>
        /// 一時登録グリッドセルアクティブ後イベント
        /// </summary>
        /// <remarks>
        /// 分析項目パネル設定処理を実行します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenStatRegistration_AfterCellActivate(object sender, EventArgs e)
        {
            this.setAnalytesPanel((CustomGrid)sender);

            System.Diagnostics.Debug.WriteLine(String.Format("{0}:grdSpecimenStatRegistration_AfterCellActivate", DateTime.Now.ToLongTimeString()));
        }

        /// <summary>
        /// 分析項目パネル設定
        /// </summary>
        /// <remarks>
        /// 検体登録グリッドのアクティブ行のデータを分析項目パネルに設定します
        /// </remarks>
        /// <param name="sender"></param>
        private void setAnalytesPanel(CustomGrid sender)
        {
            try
            {
                if (sender.ActiveRow != null)
                {
                    //Regist Typeが設定されていない行の場合は処理を行わない
                    if (sender.ActiveRow.Cells[STRING_GRD_REGISTTYPE].Value.ToString() == String.Empty)
                    {
                        return;
                    }

                    RegistType registType = (RegistType)Enum.Parse(typeof(RegistType)
                                                                  , sender.ActiveRow.Cells[STRING_GRD_REGISTTYPE].Value.ToString()
                                                                  , true);

                    int rowIndex = sender.ActiveRow.Index;
                    if (registType == RegistType.Fixed)
                    {
                        // 固定登録の場合、カウントアップ
                        rowIndex = (sender.ActiveRow.Index + 1);
                    }

                    if (this.existData.ContainsKey(rowIndex))
                    {
                        var registerd = from v in this.existData[rowIndex].Registered
                                        where v.Item1.HasValue
                                        select new Tuple<Int32, Int32, Int32>(v.Item1.Value, v.Item2.Value, v.Item3.Value);

                        this.analysisSettingPanel.SetProtocolSettingState(registerd.ToList(), true);
                        // 選択行が登録済項目であれば、分析項目未設定を許可しない
                        var currentData = this.createGridViewDataSetFromRow(this.getDataSource(registType).Rows[sender.ActiveRow.Index]);
                        Tuple<String, Int32, String> linkKey = new Tuple<String, Int32, String>(currentData.RackID.DispPreCharString
                                                                                               , currentData.RackPosition
                                                                                               , currentData.PatientID);
                        this.analysisSettingPanel.SetMustSelection(0);
                        if (this.dbLinkData.ContainsKey(linkKey))
                        {
                            Int32 index = this.dbLinkData[linkKey];
                            if (index < this.getDataSource(registType).Rows.Count)
                            {
                                this.analysisSettingPanel.SetMustSelection(1);
                            }
                        }
                    }
                    else
                    {
                        this.analysisSettingPanel.SetProtocolSettingState(new List<Tuple<Int32, Int32, Int32>>());
                        this.analysisSettingPanel.SetMustSelection(0);
                    }
                }
            }
            catch (Exception ex)
            {
                // 不正データ
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog
                                                          , Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                                                          , CarisXLogInfoBaseExtention.Empty
                                                          , ex.StackTrace);
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
        private void FormSpecimenStatRegistration_FormClosed(object sender, FormClosedEventArgs e)
        {
            // UI設定保存
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenResistrationSettings.GridZoom = this.zoomPanel.Zoom;
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenResistrationSettings.GridColOrder = this.getGridData(RegistType.Temporary).GetGridColumnOrder();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenResistrationSettings.GridColWidth = this.getGridData(RegistType.Temporary).GetGridColmnWidth();
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
            this.tlbCommandBar.Tools[QUERY].SharedProps.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Enable;
        }

        /// <summary>
        /// 明細上のドロップダウンリストが閉じられたイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenStatRegistration_AfterCellListCloseUp(object sender, CellEventArgs e)
        {
            Int32 rowIdx = e.Cell.Row.Index;
            var value = e.Cell.Value;

            System.Diagnostics.Debug.WriteLine(String.Format("{0}:grdSpecimenStatRegistration_AfterCellListCloseUp", DateTime.Now.ToLongTimeString()));
        }

        /// <summary>
        /// 一時登録グリッドのセル、行、列選択状態変更前イベント
        /// </summary>
        /// <remarks>
        /// 測定テーブルを拡大(表示)します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenStatRegistrationFixed_BeforeSelectChange(object sender, BeforeSelectChangeEventArgs e)
        {
            // 測定テーブルを拡大(表示)
            this.splAnalyteTable.Collapsed = false;
            System.Diagnostics.Debug.WriteLine(String.Format("{0}:grdSpecimenStatRegistrationFixed_BeforeSelectChange", DateTime.Now.ToLongTimeString()));
        }

        /// <summary>
        /// セルデータエラーイベント
        /// </summary>
        /// <remarks>
        /// エラーイベント設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void grdSpecimenStatRegistrationFixed_CellDataError(object sender, CellDataErrorEventArgs e)
        {
            e.RaiseErrorEvent = false;
            System.Diagnostics.Debug.WriteLine(String.Format("{0}:grdSpecimenStatRegistrationFixed_CellDataError", DateTime.Now.ToLongTimeString()));
        }

        private void grdSpecimenStatRegistrationFixed_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            // 測定テーブルを拡大(表示)
            this.splAnalyteTable.Collapsed = false;
            System.Diagnostics.Debug.WriteLine(String.Format("{0}:grdSpecimenStatRegistrationFixed_InitializeLayout", DateTime.Now.ToLongTimeString()));
        }

        /// <summary>
        /// 固定登録グリッドセルアクティブ後イベント
        /// </summary>
        /// <remarks>
        /// 分析項目パネル設定処理を実行します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenStatRegistrationFixed_AfterCellActivate(object sender, EventArgs e)
        {
            this.setAnalytesPanel((CustomGrid)sender);
            System.Diagnostics.Debug.WriteLine(String.Format("{0}:grdSpecimenStatRegistrationFixed_AfterCellActivate", DateTime.Now.ToLongTimeString()));
        }

        /// <summary>
        /// 明細上のドロップダウンリストが閉じられたイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenStatRegistrationFixed_AfterCellListCloseUp(object sender, CellEventArgs e)
        {
            Int32 rowIdx = e.Cell.Row.Index;
            var value = e.Cell.Value;
            System.Diagnostics.Debug.WriteLine(String.Format("{0}:grdSpecimenStatRegistrationFixed_AfterCellListCloseUp", DateTime.Now.ToLongTimeString()));
        }

        /// <summary>
        /// 固定登録グリッド編集モード終了後イベント
        /// </summary>
        /// <remarks>
        /// 固定登録グリッド編集データ確定します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenStatRegistrationFixed_AfterExitEditMode(object sender, EventArgs e)
        {
            UltraGridCell currentCell = ((CustomGrid)sender).ActiveCell;
            if (currentCell != null)
            {
                String columnName = currentCell.Column.Key;
                Int32 rowIndex = currentCell.Row.Index;
                this.setEditdata(columnName, rowIndex, currentCell.Text, RegistType.Fixed);
            }
            System.Diagnostics.Debug.WriteLine(String.Format("{0}:grdSpecimenStatRegistrationFixed_AfterExitEditMode", DateTime.Now.ToLongTimeString()));
        }

        /// <summary>
        /// 固定登録グリッド行アクティブ後イベント
        /// </summary>
        /// <remarks>
        /// 保持されている選択項目を分析項目パネルに設定します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenStatRegistrationFixed_AfterRowActivate(object sender, EventArgs e)
        {
            if (isFirstDisplay)
            {
                //初回画面表示のタイミングでアクティブにならないよう制御
                grdSpecimenStatRegistrationFixed.ActiveRow.Activated = false;
            }

            // 保持されている選択項目を分析項目パネルに設定する。
            this.setAnalytesPanel((CustomGrid)sender);

            System.Diagnostics.Debug.WriteLine(String.Format("{0}:grdSpecimenStatRegistrationFixed_AfterRowActivate", DateTime.Now.ToLongTimeString()));
        }

        /// <summary>
        /// 固定登録グリッド選択変更後イベント
        /// </summary>
        /// <remarks>
        /// 固定登録グリッド選択変更後デバッグログ出力します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenStatRegistrationFixed_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            if (typeof(UltraGridRow) == e.Type)
            {
                if (this.grdSpecimenStatRegistrationFixed.Selected.Rows.Count != 0)
                    // 一時登録グリッドの選択解除
                    this.clearGridSelected(RegistType.Temporary);
            }

            System.Diagnostics.Debug.WriteLine(String.Format("{0}:grdSpecimenStatRegistrationFixed_AfterSelectChange", DateTime.Now.ToLongTimeString()));
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, "grdSpecimenStatRegistrationFixed_AfterSelectChange");
        }

        private void grdSpecimenStatRegistrationFixed_BeforeExitEditMode(object sender, BeforeExitEditModeEventArgs e)
        {
            // 編集セルからのフォーカス抜けをキャンセルするエラー処理はここ
            if (!e.CancellingEditOperation)
            {
                var grd = sender as UltraGrid;
                if (grd.ActiveCell != null)
                {
                    this.updateGridEditData(this.getDataSource(RegistType.Fixed).Rows[grd.ActiveCell.Row.Index]);
                }
            }
            System.Diagnostics.Debug.WriteLine(String.Format("{0}:grdSpecimenStatRegistrationFixed_BeforeExitEditMode", DateTime.Now.ToLongTimeString()));
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, "grdSpecimenStatRegistrationFixed_BeforeExitEditMode");
        }

        /// <summary>
        /// グリッド取得
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private CustomGrid getGridData(RegistType type)
        {
            if (RegistType.Temporary == type)
            {
                return this.grdSpecimenStatRegistration;
            }
            else
            {
                return this.grdSpecimenStatRegistrationFixed;
            }
        }

        /// <summary>
        /// データソース取得
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private UltraDataSource getDataSource(RegistType type)
        {
            if (RegistType.Temporary == type)
            {
                return this.dscSpecimenStatRegistration;
            }
            else
            {
                return this.dscSpecimenStatRegistrationFixed;
            }
        }

        /// <summary>
        /// 一時登録グリッドのセルクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenStatRegistration_ClickCell(object sender, ClickCellEventArgs e)
        {
            // 固定登録グリッドの選択解除
            this.clearGridSelected(RegistType.Fixed);
            e.Cell.Row.Selected = true;     //クリックしたセルが所属する行を選択状態にしておかないと制御が上手くいかない
            if (e.Cell.CanEnterEditMode)
            {
                //Selected = trueの処理で一度編集モードから抜けてしまう為、編集可能なセルだった場合は編集モードにする
                grdSpecimenStatRegistration.PerformAction(UltraGridAction.EnterEditMode);
            }
            System.Diagnostics.Debug.WriteLine(String.Format("{0}:grdSpecimenStatRegistration_ClickCell", DateTime.Now.ToLongTimeString()));
        }

        /// <summary>
        /// 固定登録グリッドのセルクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenStatRegistrationFixed_ClickCell(object sender, ClickCellEventArgs e)
        {
            // 一時登録グリッドの選択解除
            this.clearGridSelected(RegistType.Temporary);
            e.Cell.Row.Selected = true;     //クリックしたセルが所属する行を選択状態にしておかないと制御が上手くいかない
            if (e.Cell.CanEnterEditMode)
            {
                //Selected = trueの処理で一度編集モードから抜けてしまう為、編集可能なセルだった場合は編集モードにする
                grdSpecimenStatRegistrationFixed.PerformAction(UltraGridAction.EnterEditMode);
            }
            System.Diagnostics.Debug.WriteLine(String.Format("{0}:grdSpecimenStatRegistrationFixed_ClickCell", DateTime.Now.ToLongTimeString()));
        }

        /// <summary>
        /// グリッドの選択状態解除
        /// </summary>
        /// <param name="registType">登録種別</param>
        /// <remarks>
        /// パラメータで指定されたタイプのグリッドの選択状態を解除する。
        /// </remarks>
        protected void clearGridSelected(RegistType registType)
        {
            CustomGrid targetGrid = null;
            if (registType == RegistType.Temporary)
            {
                if (this.grdSpecimenStatRegistration != null)
                {
                    targetGrid = this.grdSpecimenStatRegistration;
                }
            }
            else
            {
                if (this.grdSpecimenStatRegistrationFixed != null)
                {
                    targetGrid = this.grdSpecimenStatRegistrationFixed;
                }
            }

            if (targetGrid != null)
            {
                //選択行の選択解除
                foreach (UltraGridRow row in targetGrid.SearchSelectRow())
                {
                    row.Selected = false;
                }

                //セルの選択状態を解除
                foreach (UltraGridCell cell in targetGrid.Selected.Cells)
                {
                    cell.Selected = false;
                }

                //アクティブ行の状態を解除
                var activerow = targetGrid.ActiveRow;
                if (activerow != null)
                {
                    activerow.Activated = false;
                }
            }
        }

        /// <summary>
        /// 画面のフェード表示完了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormSpecimenStatRegistration_OnFadeShown(object sender, EventArgs e)
        {
            //フェード表示が終わったタイミングで画面表示終了と判断する
            this.isFirstDisplay = false;
        }

        /// <summary>
        /// 分析項目選択ボタンの活性/非活性処理
        /// </summary>
        /// <remarks>
        /// 分析項目の急診と、モジュールの急診使用有無によって分析項目選択ボタンの活性/非活性を切り替える
        /// </remarks>
        private void protocolIndexToButtonDicEnabled()
        {
            // trueなら分析項目ボタンを非活性にする可能性あり
            // falseならすべての分析項目ボタンを活性にする
            enabledFlag = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.IsProtocolEnabledChangedInEmergencyMode();

            // 分析項目パネルに表示されている全ボタンの取得
            foreach (var btn in analysisSettingPanel.ProtocolIndexToButtonDic)
            {
                // ボタンのキーと対応する分析項目のプロトコルを取得
                var protocal = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(btn.Key);

                // 非活性にする必要がある場合
                if (enabledFlag)
                {
                    // 分析項目の急診使用がありの場合非活性にする
                    if (protocal.UseEmergencyMode == true)
                    {
                        btn.Value.Enabled = false;
                    }
                    else
                    {
                        btn.Value.Enabled = true;
                    }
                }
                // 非活性にする必要がない場合
                else
                {
                    btn.Value.Enabled = true;
                }
            }

        }

        /// <summary>
        /// 検体登録グリッドセル変更イベント
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenStatRegistration_CellChange( object sender, CellEventArgs e )
        {
            // アクティブセルの取得
            UltraGridCell currentCell = ( (CustomGrid)sender ).ActiveCell;

            // アクティブセルが検体IDの場合
            if (currentCell.Column.Key == STRING_GRD_PATIENTID)
            {

                String formatCheck = String.Empty;

                // セル内のテキストから使用できない文字を削除する
                formatCheck = CarisXSubFunction.IsValidForPatientID(currentCell.Text);

                // 検体IDに使用できない文字があった場合
                Boolean formatError = formatCheck.Equals(currentCell.Text);

                if (!formatError)
                {
                    // 検体IDフォーマットエラー通知を行う
                    Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.STATPatientIDformatError, currentCell);
                }
            }

            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        /// <summary>
        /// 一時登録グリッドセル変更イベント
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenStatRegistrationFixed_CellChange( object sender, CellEventArgs e )
        {
            // アクティブセルの取得
            UltraGridCell currentCell = ( (CustomGrid)sender ).ActiveCell;

            // アクティブセルが検体IDの場合
            if (currentCell.Column.Key == STRING_GRD_PATIENTID)
            {

                String formatCheck = String.Empty;

                // セル内のテキストから使用できない文字を削除する
                formatCheck = CarisXSubFunction.IsValidForPatientID(currentCell.Text);

                // 検体IDに使用できない文字があった場合
                Boolean formatError = formatCheck.Equals(currentCell.Text);

                if (!formatError)
                {
                    // 検体IDフォーマットエラー通知を行う
                    Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.STATPatientIDformatError, currentCell);
                }
            }

            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        /// <summary>
        /// 検体ID変更処理
        /// </summary>
        /// <param name="value"></param>
        private void chengePatientID( Object value )
        {
            // アクティブセルの取得
            UltraGridCell currentCell = (UltraGridCell)value;

            // 検体ID内の使用できない文字があれば検体IDから削除する
            String str = CarisXSubFunction.IsValidForPatientID(currentCell.Text);

            // セル内のカーソル位置を記憶
            Int32 selset = currentCell.SelStart;

            // アクティブセルの検体IDをチェック後の検体IDに変更
            currentCell.Value = str;

            // セル内のカーソル位置を変更
            currentCell.SelStart = selset - 1;
        }
    }
}
