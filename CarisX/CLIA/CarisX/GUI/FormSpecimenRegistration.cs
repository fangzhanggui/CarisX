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

//using UISettings = Oelco.Common.Utility.Singleton<Oelco.Common.Parameter.ParameterFilePreserve<Oelco.CarisX.Parameter.CarisXUISettingManager>>;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 検体登録画面クラス
    /// </summary>
    public partial class FormSpecimenRegistration : FormChildBase
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
        /// <summary>
        /// グリッド列名 ラックポジション
        /// </summary>
        public const String STRING_GRD_RACKPOSITION = "Rack Position";
        /// <summary>
        /// グリッド列名 受付番号
        /// </summary>
        public const String STRING_GRD_RECEIPTNO = "Receipt No.";
        /// <summary>
        /// グリッド列名 検体ID
        /// </summary>
        public const String STRING_GRD_PATIENTID = "Patient ID";
        /// <summary>
        /// グリッド列名 検体種別
        /// </summary>
        public const String STRING_GRD_SPECIMENTYPE = "Specimen type";
        /// <summary>
        /// グリッド列名 分析項目
        /// </summary>
        public const String STRING_GRD_REGISTERED = "Registered";
        /// <summary>
        /// グリッド列名 手稀釈倍率
        /// </summary>
        public const String STRING_GRD_MANUALDILUTIONRATIO = "Manual dilution ratio";
        /// <summary>
        /// グリッド列名 コメント
        /// </summary>
        public const String STRING_GRD_COMMENT = "Coment";

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
        /// コピー
        /// </summary>
        private const String COPY = "Copy";

        /// <summary>
        /// ペースト
        /// </summary>
        private const String PASTE = "Paste";

        /// <summary>
        /// ペースト
        /// </summary>
        private const String QUERY = "Query";

        #endregion

        #region [インスタンス変数定義]
        /// <summary>
        /// 検体登録画面UI設定
        /// </summary>
        private FormSpecimenResistrationSettings formSpecimenRegistrationSettings;

        /// <summary>
        /// 現在この画面が動作している分析モード
        /// </summary>
        private AssayModeParameter.AssayModeKind usingAssayModeParameter;

        /// <summary>
        /// 急診モードの分析項目ボタンを非活性にするかの判断フラグ
        /// </summary>
        private bool enabledFlag;

        #endregion

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

            base.Show(captScreenRect);

            // stopwatch
            //sw.Stop();
            //Console.WriteLine("経過時間 captScreenRect　={0}", sw.Elapsed);
        }

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormSpecimenRegistration()
        {
            InitializeComponent();

            // TODO:仮クリア
#if !Debug_caris
            Singleton<SpecimenGeneralDB>.Instance.DeleteAll();
#endif
            //this.initializeFormComponent();
            //this.setCulture();
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.RealtimeData, this.onRealTimeDataChanged);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.AnalyteRoutineTableChanged, this.onAnalyteRoutineTableChanged);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UseOfPrint, this.onPrintParamChanged);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.RackIdDefinitionChanged, this.onRackIdDefinitionChanged);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UseOfHost, this.onHostParamChanged);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.HostWorkSheetBatch, this.workSheetFromHostByBatch);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.AskBatchComplete, this.askBatchComplete);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ChangeAnalyteGroup, this.onChangeAnalyteGroup);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.HostWorkSheetSingle, this.workSheetFromHostSingle);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ChangeProtocolSetting, this.onChangeProtocolSetting);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.AssayModeUseOfEmergencyMode, this.onAssayModeKindChanged);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SpecimenPatientIDformatError, this.chengePatientID);

            this.grdSpecimenRegistration.SetGridRowBackgroundColorRuleFromIndex(5, new List<Color>()
            {
                CarisXConst.GRID_ROWS_DEFAULT_COLOR,CarisXConst.GRID_ROWS_COLOR_PATTERN1
            });

            //设置ToolBar的右键功能不可用
            this.tlbCommandBar.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.tlbCommandBar_BeforeToolbarListDropdown);


        }
        //设置ToolBar的右键功能不可用
        private void tlbCommandBar_BeforeToolbarListDropdown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventArgs e)
        {
            e.Cancel = true;
        }


        /// <summary>
        /// 編集データ
        /// </summary>
        private Dictionary<UltraDataRow, SpecimenRegistrationGridViewDataSet> editData = new Dictionary<UltraDataRow, SpecimenRegistrationGridViewDataSet>();

        /// <summary>
        /// DBレコードマッピングデータ(GridのDBのキー項目-Index)
        /// </summary>
        private Dictionary<Tuple<String, Int32, String>, Int32> dbLinkData = new Dictionary<Tuple<String, Int32, String>, Int32>();
        /// <summary>
        /// DBデータ（編集前）
        /// </summary>
        private List<SpecimenRegistrationGridViewDataSet> dispDataOriginal = new List<SpecimenRegistrationGridViewDataSet>();

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
                row.Cells[STRING_GRD_PATIENTID].Activation = Activation.NoEdit; //= Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText; // 編集不可のスタイル
                row.Cells[STRING_GRD_SPECIMENTYPE].Activation = Activation.NoEdit; // 編集可のスタイル
            }
            else
            {
                // 未登録データ編集可否設定          
                row.Cells[STRING_GRD_PATIENTID].Activation = Activation.AllowEdit; // 編集可のスタイル
                row.Cells[STRING_GRD_SPECIMENTYPE].Activation = Activation.AllowEdit; // 編集可のスタイル
            }


            // 分析方式による編集可否設定
            switch (this.usingAssayModeParameter)
            {
                case AssayModeParameter.AssayModeKind.RackID:
                    row.Cells[STRING_GRD_RACKID].Activation = Activation.NoEdit;
                    row.Cells[STRING_GRD_RACKPOSITION].Activation = Activation.NoEdit;
                    break;

                case AssayModeParameter.AssayModeKind.SampleID:

                    //by marxsu  
                    if (registered)
                    {
                        row.Cells[STRING_GRD_RACKID].Activation = Activation.NoEdit;
                        row.Cells[STRING_GRD_RACKPOSITION].Activation = Activation.NoEdit;
                    }
                    else
                    {
                        row.Cells[STRING_GRD_RACKID].Activation = Activation.AllowEdit;
                        row.Cells[STRING_GRD_RACKPOSITION].Activation = Activation.AllowEdit;
                    }

                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// グリッドデータ読込
        /// </summary>
        /// <remarks>
        /// グリッド表示データをDBから取得します。
        /// </remarks>
        private void loadGridData(Int32 currentIndex = 0, bool bRealTime = false)
        {
            // DBからデータを取得
            Singleton<SpecimenGeneralDB>.Instance.LoadDB();
            this.dispDataOriginal = Singleton<SpecimenGeneralDB>.Instance.GetDispData();

            // グリッドにデータを設定
            foreach (SpecimenRegistrationGridViewDataSet val in dispDataOriginal)
            {
                Tuple<String, Int32, String> linkKey = new Tuple<String, Int32, String>(val.RackID.DispPreCharString, val.RackPosition, val.PatientID);
                if (this.dbLinkData.ContainsKey(linkKey))
                {
                    // この画面にデータをロードするのが2度目の場合
                    // いずれかの処理により、検体登録情報が存在し、DBのデータ行と関連付けがされている。
                    Int32 index = this.dbLinkData[linkKey];
                    if (index < this.dscSpecimenRegistration.Rows.Count)
                    {
                        UltraDataRow row = this.dscSpecimenRegistration.Rows[index];
                        row[STRING_GRD_RACKID] = val.RackID.Value == 0 ? String.Empty : val.RackID.DispPreCharString;
                        row[STRING_GRD_RACKPOSITION] = val.RackPosition == 0 ? String.Empty : val.RackPosition.ToString();
                        row[STRING_GRD_RECEIPTNO] = val.ReceiptNumber;
                        row[STRING_GRD_PATIENTID] = val.PatientID;
                        row[STRING_GRD_SPECIMENTYPE] = CarisXSubFunction.GetSampleKindGridItemString(val.SpecimenType);
                        row[STRING_GRD_REGISTERED] = this.joinProtocolNames(val.Registered);
                        row[STRING_GRD_MANUALDILUTIONRATIO] = val.ManualDil.ToString();
                        row[STRING_GRD_COMMENT] = val.Comment;
                        this.setItemEditable(true, this.grdSpecimenRegistration.Rows[row.Index]);

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
                    var searched = from v in this.dscSpecimenRegistration.Rows.OfType<UltraDataRow>()
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

                    // インデックス登録
                    this.dbLinkData[linkKey] = index;



                    if (index < this.dscSpecimenRegistration.Rows.Count)
                    {
                        UltraDataRow row = this.dscSpecimenRegistration.Rows[index];
                        row[STRING_GRD_RACKID] = val.RackID.Value == 0 ? String.Empty : val.RackID.DispPreCharString;
                        row[STRING_GRD_RACKPOSITION] = val.RackPosition == 0 ? String.Empty : val.RackPosition.ToString();
                        row[STRING_GRD_RECEIPTNO] = val.ReceiptNumber;
                        row[STRING_GRD_PATIENTID] = val.PatientID;
                        row[STRING_GRD_SPECIMENTYPE] = CarisXSubFunction.GetSampleKindGridItemString(val.SpecimenType);
                        row[STRING_GRD_REGISTERED] = this.joinProtocolNames(val.Registered);
                        row[STRING_GRD_MANUALDILUTIONRATIO] = val.ManualDil.ToString();
                        row[STRING_GRD_COMMENT] = val.Comment;
                        this.setItemEditable(true, this.grdSpecimenRegistration.Rows[row.Index]);

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
                    if (dat.Value.ReceiptNumber == 0)//表明是编辑的数据
                    {
                        UltraDataRow row = this.dscSpecimenRegistration.Rows[dat.Key];
                        row[STRING_GRD_RACKID] = dat.Value.RackID.Value == 0 ? String.Empty : dat.Value.RackID.DispPreCharString;
                        row[STRING_GRD_RACKPOSITION] = dat.Value.RackPosition == 0 ? String.Empty : dat.Value.RackPosition.ToString();
                        row[STRING_GRD_RECEIPTNO] = dat.Value.ReceiptNumber;
                        row[STRING_GRD_PATIENTID] = dat.Value.PatientID;
                        row[STRING_GRD_SPECIMENTYPE] = CarisXSubFunction.GetSampleKindGridItemString(dat.Value.SpecimenType);
                        row[STRING_GRD_REGISTERED] = this.joinProtocolNames(dat.Value.Registered);
                        row[STRING_GRD_MANUALDILUTIONRATIO] = dat.Value.ManualDil.ToString();
                        row[STRING_GRD_COMMENT] = dat.Value.Comment;
                        this.setItemEditable(true, this.grdSpecimenRegistration.Rows[row.Index]);
                        this.editData[row] = dat.Value;
                        // 手希釈編集設定
                        this.setAllowManualDilEdit(row, dat.Value);
                        //对编辑的数据设置当前行选中
                        currentIndex = row.Index;
                    }
                }
            }

            // 設定行を選択状態にする。如果是实时刷新，则在这里不选中当前项。
            if (bRealTime == false)
            {
                this.grdSpecimenRegistration.Rows[currentIndex].Selected = true;
                this.grdSpecimenRegistration.Rows[currentIndex].Activate();
            }


            //this.grdSpecimenRegistration.DisplayLayout.Bands[0].Columns[3].MaxLength = 16;
            //this.grdSpecimenRegistration.DisplayLayout.Bands[0].Columns[3].InvalidValueBehavior = InvalidValueBehavior.RevertValueAndRetainFocus;
            //// 先頭行を選択状態にする。
            //this.grdSpecimenRegistration.Rows.First().Selected = true;
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
                        // ボタンのキーと対応する分析項目のプロトコルを取得
                        var protocal = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex((Int32)protocolRegistInfo[i].Item1);

                        // 急診使用によってグリッドから分析項目を非表示にする必要のない場合
                        if ((enabledFlag == false) || (protocal.UseEmergencyMode == false))
                        {
                            String protocolName = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(protocolRegistInfo[i].Item1.Value).ProtocolName;
                            protocolItem = protocolName;
                            if (protocolRegistInfo[i].Item2 != 1)
                            {
                                protocolItem += String.Format("(x{0})", protocolRegistInfo[i].Item2);
                            }
                            protocolItem += String.Format("[{0}]", protocolRegistInfo[i].Item3);

                            sa.Add(protocolItem);
                        }

                    }
                }
                result = String.Join(",", sa);
            }
            return result;
        }

        /// <summary>
        /// 分析項目取得
        /// </summary>
        /// <remarks>
        /// グリッドの分析項目表示文字列から、分析項目設定情報を作成します。
        /// </remarks>
        /// <param name="joinedProtocolNames">分析項目表示文字列</param>
        /// <returns>分析項目設定情報</returns>
        [Obsolete("この関数はグリッドのRegisterd項目から編集データへの連携が無くなった為、現在使用予定はありません。", true)]
        protected List<Tuple<Int32?, Int32?>> splitProtocolNames(String joinedProtocolNames)
        {
            List<Tuple<Int32?, Int32?>> names = new List<Tuple<Int32?, Int32?>>();
            String[] namesAry = joinedProtocolNames.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            foreach (String name in namesAry)
            {
                // AAA(x10)の数値部分を正規表現で切り出す
                Regex regex = new Regex(".*(x(?<value>[0-9]+))");
                Match match = regex.Match(name);
                Int32? dilRatio = 1;
                String name2 = name;

                if (match.Success)
                {
                    dilRatio = Int32.Parse(match.Groups["value"].Value); // コード上からのみ操作される文字列なので変換例外発生しない。
                    name2 = name.Substring(0, name.IndexOf('('));
                }

                Int32 protoIndex = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromName(name2).ProtocolIndex;
                names.Add(new Tuple<Int32?, Int32?>(protoIndex, dilRatio));
            }

            return names;
        }
        ///// <summary>
        ///// 手稀釈倍率取得
        ///// </summary>
        ///// <remarks>
        ///// グリッド表示の手稀釈倍率から、倍率値を取得します。
        ///// </remarks>
        ///// <param name="gridDispStr">手稀釈倍率表示文字列</param>
        ///// <returns>手稀釈倍率</returns>
        //protected Int32 getManualDilutionRatio( String gridDispStr )
        //{
        //    // X10 等の数値部分を正規表現で切り出す
        //    Regex regex = new Regex( "X(?<value>[0-9]+)" );
        //    Match match = regex.Match( gridDispStr );
        //    Int32 handDilRatio = 1; // TODO:手稀釈倍率初期値(?設定)

        //    if ( match.Success )
        //    {
        //        handDilRatio = Int32.Parse( match.Groups["value"].Value ); // この時点ではコード上からのみ操作される文字列なので変換例外発生しない。
        //    }

        //    return handDilRatio;
        //}


        //DataTable tb = new DataTable();

        /// <summary>
        /// フォームコンポーネント初期化
        /// </summary>
        /// <remarks>
        /// 画面表示項目関連の初期化を行います。
        /// </remarks>
        protected override void initializeFormComponent()
        {
            // 拡大率変更イベント登録
            this.zoomPanel.SetZoom += grdSpecimenRegistration.SetGridZoom;

            // 拡大率切替コントロール初期化
            //            this.zoomPanel.ZoomStep = this.formSpecimenRegistrationSettings.GridZoom;
            //this.zoomPanel.Zoom = this.formSpecimenRegistrationSettings.GridZoom;
            zoomPanel.Zoom = Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenResistrationSettings.GridZoom;

            //// 分析項目選択パネルへ項目設定（この時点でファイルのデシリアライズはされている事）
            //this.analysisSettingPanel.
            //Singleton<ParameterFilePreserve<MeasureProtocolManager>>.Instance.Param.MeasureProtocol

            // グリッド初期化
            this.InitializeGridView();

            // グリッド表示データを取得
            this.loadGridData();

            // ペーストボタンを利用不可設定
            this.tlbCommandBar.Tools[PASTE].SharedProps.Enabled = false;

            // オンライン出力ボタン表示設定
            this.tlbCommandBar.Tools[QUERY].SharedProps.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Enable;

            // 印刷ボタン表示設定
            this.tlbCommandBar.Tools[PRINT].SharedProps.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable;

            // スクロール処理設定
            this.gesturePanel.ScrollProxy = this.grdSpecimenRegistration.ScrollProxy;

            // グリッド表示順
            this.grdSpecimenRegistration.SetGridColumnOrder(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenResistrationSettings.GridColOrder);

            // グリッド列幅
            this.grdSpecimenRegistration.SetGridColmnWidth(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenResistrationSettings.GridColWidth);
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
            if (((RealtimeDataKind)kind) == RealtimeDataKind.SampleRegist)
            {

                int currentRowIndex = -1;
                var selected = this.grdSpecimenRegistration.SearchSelectRow();
                //在接收ID为空或为0时，为正在编辑的数据，选出选中的最后一条记录
                if (selected != null && selected.Count() > 0 && (selected.Last().Cells[2].Value.ToString() == string.Empty || selected.Last().Cells[2].Value.ToString() == "0"))
                {
                    object[] noSaveData = new object[]{selected.Last().Cells[0].Value,selected.Last().Cells[1].Value,selected.Last().Cells[2].Value,selected.Last().Cells[3].Value
                                                       ,selected.Last().Cells[4].Value,selected.Last().Cells[5].Value,selected.Last().Cells[6].Value,selected.Last().Cells[7].Value};

                    currentRowIndex = selected.Last().Index;
                    //try
                    //{
                    //    string strTest = String.Format("rackID={1} reckPositon={2} recefID={3}sampleid={4}specietype={5}protocl={6}manudilo={7}comment={8},currentRowIndex={9}", noSaveData[0].ToString(), noSaveData[1].ToString(), noSaveData[2].ToString(), noSaveData[3].ToString(), noSaveData[4], noSaveData[5], noSaveData[6].ToString(), noSaveData[7].ToString(), currentRowIndex.ToString());
                    //    Console.WriteLine(strTest);
                    //}
                    //catch (System.Exception ex)
                    //{
                    //    MessageBox.Show(ex.ToString());
                    //}

                    this.InitializeGridView(true, currentRowIndex, noSaveData);
                    this.loadGridData(0, true);
                    UltraGridRow row = this.grdSpecimenRegistration.Rows[currentRowIndex];
                    //让这条记录处于可编辑状态
                    this.setItemEditable(false, this.grdSpecimenRegistration.Rows[currentRowIndex]);
                    //设置这条记录为选中状态
                    this.grdSpecimenRegistration.Rows[currentRowIndex].Selected = true;
                    this.grdSpecimenRegistration.Rows[currentRowIndex].Activate();

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
            Singleton<SpecimenGeneralDB>.Instance.DeleteAll();
            Singleton<SpecimenGeneralDB>.Instance.CommitSampleInfo();

            // グリッド初期化
            this.InitializeGridView();

            // マッピングデータをクリアする。
            this.dbLinkData.Clear();

            // 編集データをクリアする。
            this.editData.Clear();

            this.loadGridData();

            // 測定テーブル表示更新
            this.analysisSettingPanel.ReLoadAnalyteInformation();

            // ペーストボタンを利用不可設定
            this.tlbCommandBar.Tools[PASTE].SharedProps.Enabled = false;
            this.copyDataTemp = null;

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
        /// ラックID割り当て変更時
        /// </summary>
        /// <remarks>
        /// ラックID割り当て変更します
        /// </remarks>
        /// <param name="value"></param>
        private void onRackIdDefinitionChanged(Object value)
        {
            // 一般(優先)ラックID割り当て変更時
            var changeSampleKind = value as IEnumerable<SampleKind>;
            if ((changeSampleKind ?? new SampleKind[] { }).Contains(SampleKind.Sample))
            {
                // 登録DBの全削除
                Singleton<SpecimenGeneralDB>.Instance.DeleteAll();
                Singleton<SpecimenGeneralDB>.Instance.CommitSampleInfo();

                // グリッド初期化
                this.InitializeGridView();

                // マッピングデータをクリアする。
                this.dbLinkData.Clear();

                // 編集データをクリアする。
                this.editData.Clear();


                this.loadGridData();
            }
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
        /// FormProtocolSetting変更時処理
        /// </summary>
        /// <param name="value"></param>
        private void onChangeProtocolSetting(Object value)
        {
            // 分析項目ボタンに活性/非活性の変更
            this.protocolIndexToButtonDicEnabled();
            this.loadGridData();
        }

        /// <summary>
        /// 分析方法(急診)変更時
        /// </summary>
        /// <param name="value"></param>
        private void onAssayModeKindChanged(Object value)
        {
            // 分析項目ボタンに活性/非活性の変更
            this.protocolIndexToButtonDicEnabled();
        }

        /// <summary>
        /// グリッド表示用検定モードラックID初期化
        /// </summary>
        /// <remarks>
        /// グリッド表示用検定モードラックIDを初期化します
        /// </remarks>
        protected void initializeGridViewForAssayModeRackID(Boolean bRealTime = false, int nCurrentIndex = -1, object[] noSaveData = null)
        {
            this.grdSpecimenRegistration.SetGridRowBackgroundColorRuleFromIndex(5, new List<Color>()
                  {
                      CarisXConst.GRID_ROWS_DEFAULT_COLOR,CarisXConst.GRID_ROWS_COLOR_PATTERN1
                  });

            // ラック数取得(システム設定)
            Int32 rackMax = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDSamp;
            Int32 rackMin = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDSamp;

            String defaultSampleKind = CarisXSubFunction.GetSampleKindGridItemString(SpecimenMaterialType.BloodSerumAndPlasma);

            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //sw.Start();
            // 一般ラック
            CarisXIDString generalRack = new GeneralRackID();
            //var rowTemplate = new Object[] { generalRack.DispPreCharString, j + 1, String.Empty, String.Empty, defaultSampleKind, String.Empty, "1", String.Empty };
            //var rowTemplate = new Object[] { generalRack.DispPreCharString, 1, String.Empty, String.Empty, defaultSampleKind, String.Empty, "1", String.Empty };

            // TODO:3999ラックだと非常に処理が重い為、高速化が必要
            //var objObj = new Object[(rackMax - rackMin + 1) * CarisXConst.RACK_POS_COUNT,8];
            //var obob = (Object[]) of objObj[0,0];
            int nCount = 0;
            for (Int32 i = rackMin; i <= rackMax; i++)
            {
                for (Int32 j = 0; j < CarisXConst.RACK_POS_COUNT; j++)
                {
                    generalRack.Value = i;
                    //objObj[(i*CarisXConst.RACK_POS_COUNT) +j,0] =  generalRack.DispPreCharString; 
                    //objObj[(i*CarisXConst.RACK_POS_COUNT) +j,1] = j + 1;
                    //objObj[(i*CarisXConst.RACK_POS_COUNT) +j,2] = String.Empty;
                    //objObj[(i*CarisXConst.RACK_POS_COUNT) +j,3] = String.Empty;
                    //objObj[(i*CarisXConst.RACK_POS_COUNT) +j,4] = defaultSampleKind;
                    //objObj[(i*CarisXConst.RACK_POS_COUNT) +j,5] = String.Empty;
                    //objObj[( i * CarisXConst.RACK_POS_COUNT ) + j, 6] = "1";
                    //objObj[( i * CarisXConst.RACK_POS_COUNT ) + j, 7] = String.Empty;
                    //this.dscSpecimenRegistration.Rows.Add(objObj[( i * CarisXConst.RACK_POS_COUNT )]);
                    if (bRealTime == true && nCount == nCurrentIndex)//是实时刷新且，找到正在编辑的这一列
                    {
                        this.dscSpecimenRegistration.Rows.Add(new Object[] { noSaveData[0], noSaveData[1], noSaveData[2], noSaveData[3], noSaveData[4], noSaveData[5], noSaveData[6], noSaveData[7] });
                    }
                    else
                    {
                        this.dscSpecimenRegistration.Rows.Add(new Object[] { generalRack.DispPreCharString, j + 1, String.Empty, String.Empty, defaultSampleKind, String.Empty, "1", String.Empty });
                    }
                    nCount++;
                    this.setItemEditable(false, this.grdSpecimenRegistration.Rows.Last());
                }
            }
            //sw.Stop();
            //System.Diagnostics.Debug.WriteLine( "Row作るのに{0}かかりました。", sw.Elapsed );

            //// 優先ラック
            //Int32 priorityRackCnt = 2;// TODO:ラック数取得(システム設定)
            //CarisXIDString priorityRack = new PriorityRackID();
            //for ( Int32 i = 0; i < priorityRackCnt; i++ )
            //{
            //    for ( Int32 j = 0; j < CarisXConst.RACK_POS_COUNT; j++ )
            //    {
            //        priorityRack.Value = i + 1;
            //        this.dscSpecimenRegistration.Rows.Add( new Object[] { priorityRack.DispPreCharString, j + 1, String.Empty, String.Empty, defaultSampleKind, String.Empty, "X1", String.Empty } );
            //        this.setItemEditable( false, this.grdSpecimenRegistration.Rows.Last() );
            //    }
            //}

            // this.existData.Clear();
            //   ClearExitData(bRealTime);
        }

        /// <summary>
        /// グリッド表示用検定モードサンプルID初期化
        /// </summary>
        /// <remarks>
        /// グリッド表示用検定モードサンプルIDを初期化します
        /// </remarks>
        protected void initializeGridViewForAssayModeSampleID(Boolean bRealTime = false, int nCurrentIndex = -1, object[] noSaveData = null)
        {
            // ラック数取得(システム設定：検体ID用ラック範囲）
            Int32 rackCnt = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDSampModeSample -
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDSampModeSample + 1;
            String defaultSampleKind = CarisXSubFunction.GetSampleKindGridItemString(SpecimenMaterialType.BloodSerumAndPlasma);

            this.grdSpecimenRegistration.SetGridRowBackgroundColorRuleFromIndex(1, new List<Color>()
                  {
                      CarisXConst.GRID_ROWS_DEFAULT_COLOR,CarisXConst.GRID_ROWS_COLOR_PATTERN1
                  });

            int nCount = 0;
            // 一般ラック
            for (Int32 i = 0; i < rackCnt; i++)
            {
                for (Int32 j = 0; j < CarisXConst.RACK_POS_COUNT; j++)
                {
                    //是实时刷新且，找到正在编辑的这一列
                    if (bRealTime == true && nCount == nCurrentIndex)
                    {
                        this.dscSpecimenRegistration.Rows.Add(new Object[] { noSaveData[0], noSaveData[1], noSaveData[2], noSaveData[3], noSaveData[4], noSaveData[5], noSaveData[6], noSaveData[7] });
                    }
                    else
                    {
                        this.dscSpecimenRegistration.Rows.Add(new Object[] { String.Empty, String.Empty, String.Empty, String.Empty, defaultSampleKind, String.Empty, "1", String.Empty });
                    }
                    nCount++;
                    this.setItemEditable(false, this.grdSpecimenRegistration.Rows.Last());
                }
            }

            //Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.
            //// 優先ラック
            //Int32 priorityRackCnt = 2;// TODO:ラック数取得(システム設定)
            //CarisXIDString priorityRack = new PriorityRackID() ;
            //for ( Int32 i = 0; i < priorityRackCnt; i++ )
            //{
            //    for ( Int32 j = 0; j < CarisXConst.RACK_POS_COUNT; j++ )
            //    {
            //        priorityRack.Value = i + 1;
            //        this.dscSpecimenRegistration.Rows.Add( new Object[] { String.Empty, String.Empty, String.Empty, String.Empty, defaultSampleKind, String.Empty, "X1", String.Empty } );
            //        this.setItemEditable( false, this.grdSpecimenRegistration.Rows.Last() );
            //    }
            //}

            // this.existData.Clear();
            // ClearExitData(bRealTime);
        }

        /// <summary>
        /// グリッド表示初期化
        /// </summary>
        /// <remarks>
        /// グリッド表示内容の空状態を作成します。
        /// </remarks>
        public void InitializeGridView(Boolean bRealTime = false, int nCurrentIndex = -1, object[] noSaveData = null)
        {
            // グリッド初期化
            this.dscSpecimenRegistration.Rows.Clear();

            // グリッドデータが初期化される為、GridRowの関連する保持情報が無効になる。
            this.dbLinkData.Clear();
            //非实时更新，不删除编辑数据           
            this.editData.Clear();

            ClearExitData(bRealTime);

            //        this.dscSpecimenRegistration.Band.ove
            //            this.grdSpecimenRegistration.DisplayLayout.Bands[0].Override.ActiveRowAppearance.BackColor = CarisXConst.GRID_ROWS_SELECT_COLOR;
            //this.grdSpecimenRegistration.DisplayLayout.Bands[0].Override.SelectedRowAppearance.BackColor = CarisXConst.GRID_ROWS_SELECT_COLOR;


            // *現在の分析方式を設定から取得する*
            this.usingAssayModeParameter = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.AssayMode;
            // 分析方式によるグリッド初期表示の設定
            switch (this.usingAssayModeParameter)
            {
                case AssayModeParameter.AssayModeKind.RackID:
                    this.initializeGridViewForAssayModeRackID(bRealTime, nCurrentIndex, noSaveData);
                    break;
                case AssayModeParameter.AssayModeKind.SampleID:
                    this.initializeGridViewForAssayModeSampleID(bRealTime, nCurrentIndex, noSaveData);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 在实时刷新时，对正在编辑的数据进行保留
        /// </summary>
        /// <param name="bRealTime"></param>
        protected void ClearExitData(Boolean bRealTime = false)
        {
            if (bRealTime)
            {
                List<int> removeList = new List<int>();
                foreach (var item in existData)
                {
                    // if (item.Value.ReceiptNumber != 0 || (item.Value.RackPosition==0 && string.IsNullOrEmpty(item.Value.PatientID)))
                    //没有保存的数据为ReceiptNumber =0，删除其他条件
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
            this.Text = Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_011;

            // グリッド列名設定
            this.grdSpecimenRegistration.DisplayLayout.Bands[0].Columns[STRING_GRD_RACKID].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_000;
            this.grdSpecimenRegistration.DisplayLayout.Bands[0].Columns[STRING_GRD_RACKPOSITION].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_001;
            this.grdSpecimenRegistration.DisplayLayout.Bands[0].Columns[STRING_GRD_RECEIPTNO].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_002;
            this.grdSpecimenRegistration.DisplayLayout.Bands[0].Columns[STRING_GRD_PATIENTID].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_003;
            this.grdSpecimenRegistration.DisplayLayout.Bands[0].Columns[STRING_GRD_SPECIMENTYPE].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_004;
            this.grdSpecimenRegistration.DisplayLayout.Bands[0].Columns[STRING_GRD_REGISTERED].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_005;
            this.grdSpecimenRegistration.DisplayLayout.Bands[0].Columns[STRING_GRD_MANUALDILUTIONRATIO].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_006;
            this.grdSpecimenRegistration.DisplayLayout.Bands[0].Columns[STRING_GRD_COMMENT].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_007;

            // コマンドバーアイテム名設定
            this.tlbCommandBar.Tools[SAVE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_001;
            this.tlbCommandBar.Tools[DELETE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_002;
            this.tlbCommandBar.Tools[DELETE_ALL].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_003;
            this.tlbCommandBar.Tools[PRINT].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_004;
            this.tlbCommandBar.Tools[COPY].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_005;
            this.tlbCommandBar.Tools[PASTE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_006;
            this.tlbCommandBar.Tools[QUERY].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_007;

            // 検体種別コンボボックス設定
            this.grdSpecimenRegistration.DisplayLayout.ValueLists[0].ValueListItems.Clear();
            this.grdSpecimenRegistration.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_008);
            this.grdSpecimenRegistration.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_009);
            //this.grdSpecimenRegistration.DisplayLayout.ValueLists[0].ValueListItems.Add( Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_010 );
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
            var noErrorListNormal = Singleton<InProcessSampleInfoManager>.Instance.GetNoErrorSampleInfo(SampleKind.Sample);
            var removeListNormal = from v in noErrorListNormal
                                   select new Tuple<String, Int32, String>(v.RackId.DispPreCharString, v.RackPos, v.SampleId);
            Singleton<SpecimenGeneralDB>.Instance.DeleteData(removeListNormal.ToList());

            // グリッド内容再取得
            this.loadGridData();
        }

        /// <summary>
        /// テンポラリコピーデータ
        /// </summary>
        SpecimenRegistrationGridViewDataSet copyDataTemp = null;

        /// <summary>
        /// 検体情報のコピー
        /// </summary>
        /// <remarks>
        /// 検体情報をコピーします
        /// </remarks>
        public void copyData()
        {
            if ((this.grdSpecimenRegistration.ActiveCell != null) && (this.grdSpecimenRegistration.ActiveCell.IsInEditMode))
            {
                // 編集中セルからフォーカスを抜ける。
                // 変更中セル内容が確定できない場合、中断する。
                this.grdSpecimenRegistration.ActiveCell.Activated = false;
                if (this.grdSpecimenRegistration.ActiveCell != null)
                {
                    return;
                }
                //Boolean noError = this.grdSpecimenRegistration.ActiveCell.EditorResolved.ExitEditMode( false, true );
                //if ( !noError )
                //{
                //    return;
                //}
            }
            // TODO:メッセージ
            if (this.grdSpecimenRegistration.ActiveRow != null)
            {
                this.copyDataTemp = this.createGridViewDataSetFromRow(this.dscSpecimenRegistration.Rows[this.grdSpecimenRegistration.ActiveRow.Index]);

                // 操作履歴登録：コピー実行
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_016 });

                // ペーストボタンを利用可設定
                this.tlbCommandBar.Tools[PASTE].SharedProps.Enabled = true;

            }
        }

        /// <summary>
        /// 検体情報の貼り付け
        /// </summary>
        /// <remarks>
        /// 検体情報を貼り付けします
        /// </remarks>
        public void pasteData()
        {
            // TODO:実装＆XMLコメント作成
            if (this.grdSpecimenRegistration.ActiveRow == null)
            {
                DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_242, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                return;
            }
            Int32 selectedIndex = this.grdSpecimenRegistration.ActiveRow.Index;
            using (DlgPasteCountInput pasteDlg = new DlgPasteCountInput())
            {
                pasteDlg.MaxValue = CarisXConst.SAMPLE_REGIST_MAX;
                pasteDlg.ShowDialog();

                if (pasteDlg.DialogResult != DialogResult.OK)
                    return;

                if (pasteDlg.PasteCount > (this.dscSpecimenRegistration.Rows.Count - selectedIndex))
                {
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_241, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                    return;
                }

                //UltraDataRow copyRow = this.dscSpecimenRegistration.Rows[this.copySelectIndex];
                Int32 pasteCount = pasteDlg.PasteCount;

                bool isIGRA = false;
                foreach (var data in this.copyDataTemp.Registered)
                {
                    if (data.Item1.HasValue)
                    {
                        isIGRA = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(data.Item1.Value).IsIGRA;
                        if (isIGRA)
                        {
                            break;
                        }

                    }
                }

                if (this.usingAssayModeParameter == AssayModeParameter.AssayModeKind.RackID)
                {
                    this.pasteDataByRackIdMode(selectedIndex, pasteCount, isIGRA);
                }
                // by marxsu
                if (this.usingAssayModeParameter == AssayModeParameter.AssayModeKind.SampleID)
                {
                    this.pasteDataBySampleIdMode(selectedIndex, pasteCount, isIGRA);
                }

                // 操作履歴登録：ペースト実行  
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_017 });

                // Form共通の編集中フラグON
                FormChildBase.IsEdit = true;
            }
        }

        public void pasteDataByRackIdMode(Int32 selectedIndex, Int32 pasteCount, bool isIGRA)
        {
            string strPatientID = this.copyDataTemp.PatientID;
            string destStrPatientID = ergodicPatientID(strPatientID);
            for (Int32 i = 0; i < pasteCount; i++)
            {
                Int32 destIndex = selectedIndex + i;

                //TB项目可复制粘贴
                if (isIGRA)
                {
                    destIndex = selectedIndex;
                    if (destIndex != 2)
                    {
                        while (true)
                        {
                            if (destIndex % 5 == 2)
                            {
                                destIndex = destIndex - 1;
                                break;
                            }
                            destIndex++;
                        }
                    }
                    selectedIndex = destIndex + 3;

                }



                if (destIndex >= this.dscSpecimenRegistration.Rows.Count)
                {
                    // グリッド最後尾オーバー
                    break;
                }

                if (this.dbLinkData.ContainsValue(destIndex))
                {
                    // 登録済データに対してはペーストしない
                    continue;
                }

                // ペースト処理
                UltraDataRow destRow = this.dscSpecimenRegistration.Rows[destIndex];
                destRow[STRING_GRD_SPECIMENTYPE] = CarisXSubFunction.GetSampleKindGridItemString(this.copyDataTemp.SpecimenType);
                destRow[STRING_GRD_MANUALDILUTIONRATIO] = this.copyDataTemp.ManualDil.ToString();
                destRow[STRING_GRD_COMMENT] = this.copyDataTemp.Comment;

                if (!string.IsNullOrEmpty(this.copyDataTemp.PatientID))
                {
                    String firstPattern = "^[a-zA-Z0-9]+$";
                    if (System.Text.RegularExpressions.Regex.IsMatch(destStrPatientID, firstPattern))
                    {
                        String secondPattern = "([A-Za-z])(0)\\d+$";
                        if (!System.Text.RegularExpressions.Regex.IsMatch(destStrPatientID, secondPattern))
                        {
                            String firstRegExpIsNumPattern = "([A-Za-z])\\d+$";
                            String secondRegExpIsNumPattern = "^[0-9]+$";
                            String thirdRegExpIsNumPattern = "[A-Za-z]";
                            String regExpResult = System.Text.RegularExpressions.Regex.Match(destStrPatientID, firstRegExpIsNumPattern).ToString();
                            if (regExpResult.Length != 0)
                            {
                                String oldString = regExpResult;

                                long num = long.Parse(regExpResult.Substring(1)) + 1;
                                regExpResult = regExpResult.Replace(regExpResult.Substring(1), Convert.ToString(num));
                                destStrPatientID = destStrPatientID.Replace(oldString, regExpResult);

                            }
                            regExpResult = System.Text.RegularExpressions.Regex.Match(destStrPatientID, secondRegExpIsNumPattern).ToString();
                            if (regExpResult.Length != 0)
                            {
                                long num = long.Parse(regExpResult) + 1;
                                destStrPatientID = Convert.ToString(num);
                            }
                            String lastStr = destStrPatientID.Substring(destStrPatientID.Length - 1);
                            regExpResult = System.Text.RegularExpressions.Regex.Match(lastStr, thirdRegExpIsNumPattern).ToString();
                            if (regExpResult.Length != 0)
                            {
                                destStrPatientID = destStrPatientID + "1";
                            }
                            destRow[STRING_GRD_PATIENTID] = destStrPatientID;
                        }
                    }

                }
                this.editData[destRow] = this.createGridViewDataSetFromRow(destRow);
                this.editData[destRow].Registered = new List<Tuple<Int32?, Int32?, Int32?>>(this.copyDataTemp.Registered);
                this.existData[destRow.Index] = this.editData[destRow];

                this.setAnalytesPanel(this.grdSpecimenRegistration);
            }
        }

        public void pasteDataBySampleIdMode(Int32 selectedIndex, Int32 pasteCount, bool isIGRA)
        {
            Int32 intRackID = this.copyDataTemp.RackID;
            Int32 intRackPosition = this.copyDataTemp.RackPosition;
            string strPatientID = this.copyDataTemp.PatientID;
            string destStrPatientID = ergodicPatientID(strPatientID);
            Int32[] result = ergodicID(intRackID, intRackPosition);
            intRackID = result[0];
            intRackPosition = result[1];
            for (Int32 i = 0; i < pasteCount;)
            {
                if (intRackPosition >= CarisXConst.RACK_POS_COUNT)
                {
                    intRackPosition = 0;
                    intRackID++;
                }

                for (; intRackPosition < CarisXConst.RACK_POS_COUNT && i < pasteCount;)
                {
                    Int32 destIndex = selectedIndex + i;

                    if (destIndex >= this.dscSpecimenRegistration.Rows.Count)
                    {
                        i = pasteCount;
                        break;
                    }

                    if (this.dbLinkData.ContainsValue(destIndex))
                    {
                        i++;
                        continue;
                    }
                    intRackPosition++;

                    //IGRA可复制黏贴
                    if (isIGRA)
                    {
                        if (intRackPosition != 2)
                        {
                            continue;
                        }
                    }
                    UltraDataRow destRow = this.dscSpecimenRegistration.Rows[destIndex];
                    destRow[STRING_GRD_SPECIMENTYPE] = CarisXSubFunction.GetSampleKindGridItemString(this.copyDataTemp.SpecimenType);
                    destRow[STRING_GRD_MANUALDILUTIONRATIO] = this.copyDataTemp.ManualDil.ToString();
                    destRow[STRING_GRD_COMMENT] = this.copyDataTemp.Comment;

                    if (intRackID > 0 && intRackPosition > 0)
                    {
                        destRow[STRING_GRD_RACKID] = intRackID;
                        destRow[STRING_GRD_RACKPOSITION] = intRackPosition;


                    }


                    if (!string.IsNullOrEmpty(this.copyDataTemp.PatientID))
                    {
                        String firstPattern = "^[a-zA-Z0-9]+$";
                        if (System.Text.RegularExpressions.Regex.IsMatch(destStrPatientID, firstPattern))
                        {
                            String secondPattern = "([A-Za-z])(0)\\d+$";
                            if (!System.Text.RegularExpressions.Regex.IsMatch(destStrPatientID, secondPattern))
                            {
                                String firstRegExpIsNumPattern = "([A-Za-z])\\d+$";
                                String secondRegExpIsNumPattern = "^[0-9]+$";
                                String thirdRegExpIsNumPattern = "[A-Za-z]";
                                String regExpResult = System.Text.RegularExpressions.Regex.Match(destStrPatientID, firstRegExpIsNumPattern).ToString();
                                if (regExpResult.Length != 0)
                                {
                                    String oldString = regExpResult;

                                    long num = long.Parse(regExpResult.Substring(1)) + 1;
                                    regExpResult = regExpResult.Replace(regExpResult.Substring(1), Convert.ToString(num));
                                    destStrPatientID = destStrPatientID.Replace(oldString, regExpResult);

                                }
                                regExpResult = System.Text.RegularExpressions.Regex.Match(destStrPatientID, secondRegExpIsNumPattern).ToString();
                                if (regExpResult.Length != 0)
                                {
                                    long num = long.Parse(regExpResult) + 1;
                                    destStrPatientID = Convert.ToString(num);
                                }
                                String lastStr = destStrPatientID.Substring(destStrPatientID.Length - 1);
                                regExpResult = System.Text.RegularExpressions.Regex.Match(lastStr, thirdRegExpIsNumPattern).ToString();
                                if (regExpResult.Length != 0)
                                {
                                    destStrPatientID = destStrPatientID + "1";
                                }
                                destRow[STRING_GRD_PATIENTID] = destStrPatientID;
                            }
                        }
                    }
                    this.editData[destRow] = this.createGridViewDataSetFromRow(destRow);
                    this.editData[destRow].Registered = new List<Tuple<Int32?, Int32?, Int32?>>(this.copyDataTemp.Registered);
                    this.existData[destRow.Index] = this.editData[destRow];
                    this.setAnalytesPanel(this.grdSpecimenRegistration);

                    i++;
                }
            }
        }

        public Int32[] ergodicID(Int32 intRackID, Int32 intRackPosition)
        {
            Int32[] result = { 0, 0 };
            List<Int32> destRackIdList = new List<Int32>();
            List<Int32> destRackPositionList = new List<Int32>();
            foreach (KeyValuePair<Int32, SpecimenRegistrationGridViewDataSet> dat in this.existData)
            {
                destRackIdList.Add(dat.Value.RackID.Value);
            }
            destRackIdList.Sort();
            intRackID = destRackIdList.Max();
            foreach (KeyValuePair<Int32, SpecimenRegistrationGridViewDataSet> dat in this.existData)
            {
                if (intRackID == dat.Value.RackID.Value)
                {
                    destRackPositionList.Add(dat.Value.RackPosition);
                }
            }
            destRackPositionList.Sort();
            intRackPosition = destRackPositionList.Max();
            result[0] = intRackID;
            result[1] = intRackPosition;
            return result;
        }

        public String ergodicPatientID(String strPatientID)
        {
            List<long> destList = new List<long>();
            String destStrPatientId = strPatientID;
            String firstRegExpIsNumPattern = "([A-Za-z])\\d+$";
            String secondRegExpIsNumPattern = "^[0-9]+$";
            String thirdRegExpIsNumPattern = "[A-Za-z]";
            for (int i = 0; i < this.grdSpecimenRegistration.Rows.Count; i++)
            {
                UltraGridRow destRow = this.grdSpecimenRegistration.Rows[i];
                String destString = destRow.Cells[STRING_GRD_PATIENTID].Value.ToString();
                if (destString != String.Empty)
                {
                    String regExpResult = System.Text.RegularExpressions.Regex.Match(destString, firstRegExpIsNumPattern).ToString();
                    if (regExpResult.Length != 0)
                    {
                        String oldString = regExpResult;
                        long num = long.Parse(regExpResult.Substring(1));
                        destList.Add(num);
                    }
                    regExpResult = System.Text.RegularExpressions.Regex.Match(destString, secondRegExpIsNumPattern).ToString();
                    if (regExpResult.Length != 0)
                    {
                        long num = long.Parse(regExpResult);
                        destList.Add(num);
                    }
                    String lastStr = destString.Substring(destString.Length - 1);
                    regExpResult = System.Text.RegularExpressions.Regex.Match(lastStr, thirdRegExpIsNumPattern).ToString();
                    if (regExpResult.Length != 0)
                    {
                        destList.Add(0);
                    }
                }
            }

            destList.Sort();

            if (strPatientID != String.Empty && destList.Count > 0)
            {
                String destRegExpResult = System.Text.RegularExpressions.Regex.Match(strPatientID, firstRegExpIsNumPattern).ToString();
                if (destRegExpResult.Length != 0)
                {
                    String oldString = destRegExpResult;
                    destRegExpResult = destRegExpResult.Replace(destRegExpResult.Substring(1), Convert.ToString(destList.Max()));
                    destStrPatientId = destStrPatientId.Replace(oldString, destRegExpResult);

                }
                destRegExpResult = System.Text.RegularExpressions.Regex.Match(strPatientID, secondRegExpIsNumPattern).ToString();
                if (destRegExpResult.Length != 0)
                {
                    destStrPatientId = Convert.ToString(destList.Max());
                }
                String destLastStr = strPatientID.Substring(strPatientID.Length - 1);
                destRegExpResult = System.Text.RegularExpressions.Regex.Match(destLastStr, thirdRegExpIsNumPattern).ToString();
                if (destRegExpResult.Length != 0)
                {
                    destStrPatientId = strPatientID + Convert.ToString(destList.Max());
                }
            }
            return destStrPatientId;

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
        /// バッチ問合せデータ
        /// </summary>
        private class AskBatchData
        {
            /// <summary>
            /// 最小領収書番号
            /// </summary>
            public Int32 minReceiptNumber = 0;
            /// <summary>
            /// 最大領収書番号
            /// </summary>
            public Int32 maxReceiptNumber = 0;
        }

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
        private void askWorkSheetBatch(AskBatchData data)
        {
            Boolean endAsk = false;

            #region __バッチ処理__

            // ダイアログ表示
            // このダイアログは中断可能かつ、DoEventを含まず、メインスレッドを止めない事。
            Task showWaitDlg = new Task(() =>
           {
               this.waitDialog = DlgMessage.Create(CarisX.Properties.Resources.STRING_DLG_MSG_208 + "\n" + CarisX.Properties.Resources.STRING_DLG_MSG_209,
                           "", CarisX.Properties.Resources.STRING_DLG_TITLE_007, MessageDialogButtons.Cancel);
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
               for (int receiptNumber = data.minReceiptNumber; receiptNumber <= data.maxReceiptNumber; receiptNumber++)
               {
                   this.askWaitEvent.Reset();
                   // 問合せを行う（別スレッド動作する）
                   AskWorkSheetData askData = new AskWorkSheetData();

                   //Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
                   //    , String.Format("【チェック】ホストへのワークシート問合せ（バッチ）シーケンスを呼び出し      ReciptNo={0}", receiptNumber));

                   Singleton<CarisXSequenceHelperManager>.Instance.Host.HostCommunicationSequence
                       (HostCommunicationSequencePattern.AskWorkSheetToHostBatch | HostCommunicationSequencePattern.Specimen, askData, receiptNumber);

                   //Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
                   //    , String.Format("【チェック】ホストへのワークシート問合せ（バッチ）シーケンスを呼び出し完了  ReciptNo={0}", receiptNumber));

                   // 問合せ完了待ち タイムアウトはシーケンス動作内部に設定されている為、無限待ちでよい。
                   this.askWaitEvent.WaitOne();

                   // キャンセル確認
                   if (endAsk)
                   {
                       closeDlg = false;
                       break;
                   }
               }
               if (closeDlg)
               {
                   dialogCloser();
               }

               // ・実行完了時、画面へ通知を行う
               Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.AskBatchComplete);
           });
            #endregion

            // 問合せシーケンス実行タスク
            showWaitDlg.Start();
            askWorer.Start();

        }

        /// <summary>
        /// 从LIS上接收到的IGRA项目的数据
        /// </summary>
        /// <param name="obj"></param>
        protected void workSheetFromHostSingle(Object obj)
        {
            SpecimenRegistrationGridViewDataSet data = obj as SpecimenRegistrationGridViewDataSet;

            List<SpecimenRegistrationGridViewDataSet> dataList = new List<SpecimenRegistrationGridViewDataSet>();
            dataList.Add(data);
            Singleton<SpecimenGeneralDB>.Instance.SetIGRAHostDispData(dataList);
            Singleton<SpecimenGeneralDB>.Instance.CommitSampleInfo();
            if (CarisXSubFunction.AskReagentRemain() == false)
            {
                // デバッグログ出力
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, CarisX.Properties.Resources.STRING_LOG_MSG_069);
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

                //Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
                //    , String.Format("【チェック】HostWorkSheetBatchメッセージを処理                              RackID={0} Pos={1} ReciptNo={2}", askData.FromHostCommand.RackID, askData.FromHostCommand.RackPos, askData.FromHostCommand.ReceiptNumber));

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

                // ・サンプルID、ラックID、ラックPosどれも無い場合エラー
                if (askData.FromHostCommand.SampleID == ""
                    && askData.FromHostCommand.RackID == ""
                    && (askData.FromHostCommand.RackPos < 1 || askData.FromHostCommand.RackPos > 5))
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

                // ・サンプルID、ラックID、ラックPosで登録時既存データがあればエラー
                //Sample ID, rack ID, error if there is a registration when the existing data in the rack Pos
                Int32 registeredReceiptNumber = Singleton<SpecimenGeneralDB>.Instance.SearchReceiptNumber(askData.FromHostCommand.RackID, askData.FromHostCommand.RackPos, askData.FromHostCommand.SampleID);
                if (registeredReceiptNumber != 0)
                {
                    CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.FromHostWorkSheetAlreadyExists
                        , extStr: String.Format(CarisX.Properties.Resources.STRING_LOG_MSG_076, askData.FromHostCommand.RackID, askData.FromHostCommand.RackPos));
                    return;
                }

                // ・登録件数が限界ならエラー
                if (Singleton<SpecimenGeneralDB>.Instance.IsRecordMax)
                {
                    CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.FromHostWorkSheetFormatError
                        , extStr: String.Format(CarisX.Properties.Resources.STRING_LOG_MSG_084, askData.FromHostCommand.RackID, askData.FromHostCommand.RackPos));
                    return;
                }

                // 外部からの受付番号を保持する。
                if (Singleton<ReceiptNo>.Instance.ThroughExternalCreatedNumber(askData.FromHostCommand.ReceiptNumber))
                {
                    //Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
                    //    , String.Format("【チェック】検体情報へ登録                                                  RackID={0} Pos={1} ReciptNo={2}", askData.FromHostCommand.RackID, askData.FromHostCommand.RackPos, askData.FromHostCommand.ReceiptNumber));

                    // DBへの登録
                    Singleton<SpecimenGeneralDB>.Instance.SetAskData(askData);
                    Singleton<SpecimenGeneralDB>.Instance.CommitSampleInfo();
                }
                else
                {
                    // 既に登録されたことのある受付番号がホストから指定された    
                    CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.FromHostWorkSheetFormatError, extStr: Properties.Resources.STRING_LOG_MSG_082);
                }

                // グリッドへの表示
                this.InitializeGridView();
                this.loadGridData();
            }
            finally
            {
                this.askWaitEvent.Set();
            }

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
        }

        /// <summary>
        /// クエリー
        /// </summary>
        /// <remarks>
        /// バッチ問合せ処理を実行します
        /// </remarks>
        private void query()
        {
            var data = new AskBatchData();

            Task showWaitDlg = new Task(() =>
            {
                // 番号を設定する
                using (var setReceiptNumberDlg = new DlgAskReceiptNumberInput())
                {
                    // 初期値は次に発行される受付番号
                    setReceiptNumberDlg.MinValue = Singleton<ReceiptNo>.Instance.AskNextNumber();
                    setReceiptNumberDlg.MaxValue = setReceiptNumberDlg.MinValue;
                    if (setReceiptNumberDlg.ShowDialog() != DialogResult.OK)
                    {
                        // 設定しない場合ここで終了する
                        return;
                    }
                    data.minReceiptNumber = setReceiptNumberDlg.MinValue;
                    data.maxReceiptNumber = setReceiptNumberDlg.MaxValue;
                }

                this.editData.Clear();

                // バッチ問合せを行う(処理は非同期で実行される。この関数ではブロックされない）
                this.askWorkSheetBatch(data);

                // バッチ問合せ完了まで
                FormBase.AllFormItemEnableChange(false);
            });

            //showWaitDlg.Start();

            // 番号を設定する
            using (var setReceiptNumberDlg = new DlgAskReceiptNumberInput())
            {
                // 初期値は次に発行される受付番号
                setReceiptNumberDlg.MinValue = Singleton<ReceiptNo>.Instance.AskNextNumber();
                setReceiptNumberDlg.MaxValue = setReceiptNumberDlg.MinValue;
                if (setReceiptNumberDlg.ShowDialog() != DialogResult.OK)
                {
                    // 設定しない場合ここで終了する
                    return;
                }
                data.minReceiptNumber = setReceiptNumberDlg.MinValue;
                data.maxReceiptNumber = setReceiptNumberDlg.MaxValue;
            }

            this.editData.Clear();

            // バッチ問合せを行う(処理は非同期で実行される。この関数ではブロックされない）
            this.askWorkSheetBatch(data);

            // バッチ問合せ完了まで
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
                    case COPY:                  // コピー
                        this.copyData();
                        break;
                    case PASTE:                 // ペースト
                        this.pasteData();
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
        /// coa
        /// </summary>
        /// <param name="data"></param>
        private void saveHostData(SpecimenRegistrationGridViewDataSet data)
        {
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
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_009 });

            // TODO:失敗ケース
            if (this.grdSpecimenRegistration.ActiveCell != null && this.grdSpecimenRegistration.ActiveCell.IsInEditMode)
            {
                // 編集中セルからフォーカスを抜ける。
                // 変更中セル内容が確定できない場合、中断する。
                this.grdSpecimenRegistration.ActiveCell.Activated = false;
                if (this.grdSpecimenRegistration.ActiveCell != null)
                {
                    return;
                }
            }

            if (this.editData.Count != 0)
            {
                // DB登録に必須のデータが含まれるものを抽出
                if (this.usingAssayModeParameter == AssayModeParameter.AssayModeKind.SampleID)
                {
                    // 検体ID登録の場合、検体IDが入力されている、かつラックIDかラックポジションどちらか片方のみ入力されている場合、
                    // ラックIDとラックポジションは消す
                    // 範囲外のラックIDも削除する
                    foreach (var dat in this.editData)
                    {
                        if (dat.Value.PatientID != String.Empty)
                        {
                            if ((dat.Value.RackID.Value == 0) || (dat.Value.RackPosition == 0))
                            {
                                dat.Value.RackID.Value = 0;
                                dat.Value.RackPosition = 0;
                            }
                        }

                        // 範囲外削除
                        if ((dat.Value.RackID.Value < Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDSampModeSample)
                            || (dat.Value.RackID.Value > Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDSampModeSample))
                        {
                            dat.Value.RackID.Value = 0;
                            dat.Value.RackPosition = 0;
                        }
                        // TODO:必須入力項目を未入力のまま保存しようとした時に、「dat.Value.RackID == null」となる場合がある。
                        // 　　 本来ならnull設定している個所を探してString.Emptyを設定するようにすべきだが、該当箇所が見当たらないので
                        //      応急処置として以下の処理を追加する。
                        if (dat.Value.RackID == null)
                        {
                            CarisXIDString.TryParse("0", out dat.Value.RackID);
                        }
                    }
                }

                // ラックID＆ラックポジションのどちらも入力されていない、かつ検体IDが入力されていない編集データは消す。
                var removeListNoIdPos = from v in this.editData
                                        where (v.Value.RackID.Value == 0 || v.Value.RackPosition == 0) && v.Value.PatientID == String.Empty
                                               || (v.Key.Index < 0) // 編集データを持っている際に該当行がクリアされていると無効行になる。
                                        select v.Key;
                // 分析項目の登録されていないデータは消す。
                var removeListNoMeas = from v in this.editData
                                       where v.Value.Registered.Count == 0
                                       || v.Value.Registered.All((vv) => vv.Item1 == null && vv.Item2 == null)
                                       select v.Key;
                var removeList = removeListNoIdPos.Union(removeListNoMeas).Distinct();
                foreach (UltraDataRow remove in removeList.ToArray())
                {
                    this.editData.Remove(remove);
                }

                // ここまでに編集データチェック
                ////计算正在编辑的项目和已经注册了的项目
                String setErrorMessage = String.Empty;
                Boolean result = Singleton<SpecimenGeneralDB>.Instance.SetDispData(this.editData.Values.ToList(), out setErrorMessage);
                Singleton<SpecimenGeneralDB>.Instance.CommitSampleInfo();

                // 結果が失敗かつエラーメッセージがある場合
                if (!result && !String.IsNullOrEmpty(setErrorMessage))
                {
                    // 失敗メッセージを表示
                    DlgMessage.Show(setErrorMessage, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_005, MessageDialogButtons.Confirm);
                }

                // 試薬残量確認処理追加

                // 残量問合せ処理
                if (CarisXSubFunction.AskReagentRemain() == false)
                {
                    // デバッグログ出力
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                                CarisXLogInfoBaseExtention.Empty, CarisX.Properties.Resources.STRING_LOG_MSG_069);
                }
                else
                {
                    // 試薬残量確認処理
                    CarisXSubFunction.ReagentRemainWarning();
                }

                // DBレコードマッピングデータ作成( データの保持は、アプリケーションが起動している間ずっと、なのでクリアはされない )
                foreach (KeyValuePair<UltraDataRow, SpecimenRegistrationGridViewDataSet> data in this.editData)
                {
                    Tuple<String, Int32, String> linkKey = new Tuple<String, Int32, String>(data.Value.RackID.DispPreCharString, data.Value.RackPosition, data.Value.PatientID);
                    this.dbLinkData[linkKey] = data.Key.Index;
                }

                // 編集データをクリアする。
                this.editData.Clear();


                // 保存データを読み直す
                var selected = this.grdSpecimenRegistration.SearchSelectRow();
                Int32 currentRowIndex = 0;
                if (selected.Count != 0)
                {
                    currentRowIndex = selected.First().Index;
                }

                this.InitializeGridView();
                this.loadGridData(currentRowIndex);

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
            if (this.grdSpecimenRegistration.SearchSelectRow().Count != 0)
            {
                // 操作履歴：削除実行
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_003 });

                // TODO:編集中かどうか判断して、表示メッセージを変える
                DialogResult dlgResult;

                if (this.editData.Count != 0)
                {
                    // 編集中のデータも消去されます。消去を行いますか？ 
                    dlgResult = DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_024, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel);
                }
                else
                {
                    // 消去します。よろしいですか？ 
                    dlgResult = DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_019, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel);
                }

                if (dlgResult != DialogResult.OK)
                {
                    // 操作履歴：削除キャンセル
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_004 });
                    return;
                }

                List<Tuple<String, Int32, String>> delList = new List<Tuple<String, Int32, String>>();

                foreach (UltraGridRow val in this.grdSpecimenRegistration.SearchSelectRow())
                {
                    UltraDataRow dataRow = this.dscSpecimenRegistration.Rows[val.Index];

                    if ((String)(dataRow[STRING_GRD_RECEIPTNO]) != String.Empty)
                    {
                        // 受付番号リストを削除リストとする。
                        Int32 rackPos = 0;
                        Int32.TryParse((String)dataRow[STRING_GRD_RACKPOSITION], out rackPos);
                        CarisXIDString rackID = (String)dataRow[STRING_GRD_RACKID];
                        delList.Add(new Tuple<String, Int32, String>(rackID.DispPreCharString, rackPos, (String)dataRow[STRING_GRD_PATIENTID]));
                    }
                }
                this.editData.Clear(); // 編集データ削除

                // 削除・コミット・グリッド表示内容再生成
                Singleton<SpecimenGeneralDB>.Instance.DeleteData(delList);
                Singleton<SpecimenGeneralDB>.Instance.CommitSampleInfo();

                // マッピングデータを同期する。
                foreach (var delDat in delList)
                {
                    this.dbLinkData.Remove(delDat);
                }

                this.InitializeGridView();
                this.loadGridData();

                // Form共通の編集中フラグOFF
                FormChildBase.IsEdit = false;
            }
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

            // UNDONE:全消去します。よろしいですか？
            if (DialogResult.OK != DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_001, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel))
            {
                // 操作履歴登録：全消去キャンセル
                Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_011 });
                return;
            }
            // TODO:メッセージ
            Singleton<SpecimenGeneralDB>.Instance.DeleteAll();
            Singleton<SpecimenGeneralDB>.Instance.CommitSampleInfo();

            // グリッド初期化
            this.InitializeGridView();

            // マッピングデータをクリアする。
            this.dbLinkData.Clear();

            // 編集データをクリアする。
            this.editData.Clear();

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

            List<SpecimenRegistrationGridViewDataSet> prtData = new List<SpecimenRegistrationGridViewDataSet>();
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
                    var selected = from v in grdSpecimenRegistration.SearchSelectRow()
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
                            if ((enabledFlag == true) && (protocal.UseEmergencyMode == true))
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
        private void grdSpecimenRegistration_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            // 測定テーブルを拡大(表示)
            this.splAnalyteTable.Collapsed = false;
        }

        /// <summary>
        /// 検体登録グリッドのセル選択変更前イベント
        /// </summary>
        /// <remarks>
        /// 測定テーブルを拡大(表示)します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenRegistration_BeforeSelectChange(object sender, BeforeSelectChangeEventArgs e)
        {
            // 測定テーブルを拡大(表示)
            this.splAnalyteTable.Collapsed = false;


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
            var selected = this.grdSpecimenRegistration.SearchSelectRow();
            if (selected.Count != 0)
            {
                foreach (var selectedRow in selected)
                {

                    ///
                    /// 注册时当注释中有“ROMA”这个字符串时表示进行ROMA计算
                    ///
                    if (data.IsCalcRoma == true)
                    {
                        this.dscSpecimenRegistration.Rows[selectedRow.Index][STRING_GRD_COMMENT] = "ROMA";
                    }
                    else
                    {
                        if (this.dscSpecimenRegistration.Rows[selectedRow.Index][STRING_GRD_COMMENT].ToString().Contains("ROMA") &&
                            protocolIndex == 15 || protocolIndex == 25)
                        {
                            this.dscSpecimenRegistration.Rows[selectedRow.Index][STRING_GRD_COMMENT] = this.dscSpecimenRegistration.Rows[selectedRow.Index][STRING_GRD_COMMENT].ToString().Replace("ROMA", "");
                        }
                    }

                    var viewData = this.createGridViewDataSetFromRow(this.dscSpecimenRegistration.Rows[selectedRow.Index]);

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
                    //if ( registCount == 0 )
                    //{
                    //}
                    //else
                    //{
                    //    // 二つ目移行の分析項目設定の場合、手希釈が
                    //    if ( protocol.UseManualDil != this.isUseManualDilRow( viewData ) )
                    //    {
                    //        // エラーメッセージ表示せずキャンセル
                    //        data.Cancel = true;
                    //        break;
                    //    }
                    //}
                }
            }

        }

        private Boolean isNoUseManualDilRow(SpecimenRegistrationGridViewDataSet viewData)
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
            if (this.grdSpecimenRegistration.SearchSelectRow().Count != 0)
            {
                foreach (var selectedRow in this.grdSpecimenRegistration.SearchSelectRow())
                {
                    UltraDataRow currentRow = this.dscSpecimenRegistration.Rows[selectedRow.Index];

                    SpecimenRegistrationGridViewDataSet data = new SpecimenRegistrationGridViewDataSet();
                    if (this.editData.ContainsKey(currentRow))
                    {
                        data = this.editData[currentRow];
                    }
                    else if (this.existData.ContainsKey(currentRow.Index))
                    {
                        data = this.existData[currentRow.Index];
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
        /// DBのデータに対して、編集データを重ね合わせた作業用データ(グリッドRowインデックス->データ)
        /// </summary>
        /// <remarks>
        /// 編集状態更新します
        /// </remarks>
        Dictionary<Int32, SpecimenRegistrationGridViewDataSet> existData = new Dictionary<Int32, SpecimenRegistrationGridViewDataSet>();

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
            SpecimenRegistrationGridViewDataSet setData = this.createGridViewDataSetFromRow(row);

            // 編集状態更新（処理が重い場合、setEditDataにて1レコードずつやる）
            foreach (KeyValuePair<UltraDataRow, SpecimenRegistrationGridViewDataSet> dat in this.editData)
            {
                this.existData[dat.Key.Index] = dat.Value;
            }

            // 手希釈編集設定
            this.setAllowManualDilEdit(row, setData);

            // グリッド内容空の状態からの最初の編集はexistDataを使うチェックができない。

            Boolean dataUnique = true;
            String inputValue = string.Empty;
            foreach (KeyValuePair<Int32, SpecimenRegistrationGridViewDataSet> dat in this.existData)
            {
                if (dat.Key == row.Index)  // 自身は除外
                {
                    continue;
                }
                switch (this.usingAssayModeParameter)
                {
                    case AssayModeParameter.AssayModeKind.RackID:
                        // ラックID分析での入力チェック
                        // 空白でなければチェックする。
                        if (setData.PatientID != String.Empty)
                        {
                            // 検体IDユニークチェック
                            if (setData.PatientID == dat.Value.PatientID)
                            {
                                // TODO:検体ID重複Msg
                                // TODO:検体ID重複時動作は仮
                                dataUnique = false;
                                inputValue = setData.PatientID;
                                this.grdSpecimenRegistration.Rows[row.Index].Cells[STRING_GRD_PATIENTID].Value = String.Empty; // TODO:赤くする？

                            }
                        }
                        break;
                    case AssayModeParameter.AssayModeKind.SampleID:
                        // 検体ID分析での入力チェック
                        // ラックID＆ポジションが入力されているor検体IDが入力されている事
                        if (setData.RackID.Value != 0 && setData.RackPosition != 0)
                        {
                            // ラックID＆ポジションユニークチェック
                            if ((setData.RackID.Value == dat.Value.RackID.Value)
                                && (setData.RackPosition == dat.Value.RackPosition))
                            {
                                this.grdSpecimenRegistration.Rows[row.Index].Cells[STRING_GRD_RACKID].Value = String.Empty; // TODO:赤くする？
                                this.grdSpecimenRegistration.Rows[row.Index].Cells[STRING_GRD_RACKPOSITION].Value = String.Empty; // TODO:赤くする？
                                dataUnique = false;
                            }
                            // 検体IDユニークチェック
                            if ((setData.PatientID != String.Empty) && (setData.PatientID == dat.Value.PatientID))
                            {
                                // TODO:検体ID重複Msg
                                // TODO:検体ID重複時動作は仮
                                dataUnique = false;
                                inputValue = setData.PatientID;
                                this.grdSpecimenRegistration.Rows[row.Index].Cells[STRING_GRD_PATIENTID].Value = String.Empty; // TODO:赤くする？
                            }
                        }
                        else if (setData.PatientID != String.Empty)
                        {
                            // 検体IDユニークチェック
                            if (setData.PatientID == dat.Value.PatientID)
                            {
                                // TODO:検体ID重複Msg
                                // TODO:検体ID重複時動作は仮
                                dataUnique = false;
                                inputValue = setData.PatientID;
                                this.grdSpecimenRegistration.Rows[row.Index].Cells[STRING_GRD_PATIENTID].Value = String.Empty; // TODO:赤くする？
                            }
                        }
                        else
                        {
                            // 入力エラー
                            dataUnique = false;
                        }
                        break;
                    default:
                        break;
                }
                if (!dataUnique)
                {
                    if (inputValue != string.Empty)
                    {
                        String errMsg = String.Format(Properties.Resources.STRING_DLG_MSG_257, Properties.Resources.STRING_SPECIMENREGIST_003, inputValue);
                        DlgMessage.Show(errMsg, String.Empty, Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.Confirm);
                    }
                    break;
                }
            }

            if (dataUnique)
            {
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
                        this.existData[row.Index] = this.editData[row];
                        // 測定テーブル表示更新
                        this.analysisSettingPanel.ReLoadAnalyteInformation();

                        //// しない場合、検体種別をグリッドに再設定
                        //row[STRING_GRD_SPECIMENTYPE] = CarisXSubFunction.GetSampleKindGridItemString(beforeData.SpecimenType);
                    }
                    else
                    {
                        this.editData[row] = this.createGridViewDataSetFromRow(row);
                        this.existData[row.Index] = this.editData[row];
                    }

                }
                else
                {
                    // IDが編集データ上に重複しない場合登録。
                    this.editData[row] = this.createGridViewDataSetFromRow(row);
                    this.existData[row.Index] = this.editData[row];
                }

            }
            else
            {
                // 編集確定失敗
                result = false;
            }


            return result;

        }

        private void setAllowManualDilEdit(UltraDataRow row, SpecimenRegistrationGridViewDataSet setData)
        {
            // 手希釈倍率使用による編集可否の設定
            Int32 setCount = setData.Registered.Count((v) => v.Item1.HasValue);
            if (setCount == 0)
            {
                // 何も設定されていなければ、編集可
                this.grdSpecimenRegistration.Rows[row.Index].Cells[STRING_GRD_MANUALDILUTIONRATIO].Activation = Activation.AllowEdit;
            }
            else
            {
                // 手希釈不使用があれば編集不可
                if (this.isNoUseManualDilRow(setData))
                {
                    this.grdSpecimenRegistration.Rows[row.Index].Cells[STRING_GRD_MANUALDILUTIONRATIO].Activation = Activation.NoEdit;
                }
                else
                {
                    this.grdSpecimenRegistration.Rows[row.Index].Cells[STRING_GRD_MANUALDILUTIONRATIO].Activation = Activation.AllowEdit;
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
        private SpecimenRegistrationGridViewDataSet createGridViewDataSetFromRow(UltraDataRow dataRow)
        {
            SpecimenRegistrationGridViewDataSet data = new SpecimenRegistrationGridViewDataSet();


            data.RackID = dataRow[STRING_GRD_RACKID] == DBNull.Value ? String.Empty : (String)dataRow[STRING_GRD_RACKID];

            if (dataRow[STRING_GRD_RACKPOSITION] == null)
            {
                data.RackPosition = 0;
            }
            else
            {
                Int32.TryParse(dataRow[STRING_GRD_RACKPOSITION] == DBNull.Value ? "0" : dataRow[STRING_GRD_RACKPOSITION].ToString(), out data.RackPosition);
            }

            // 受付番号は登録タイミングで発番する
            // this.editData[row].ReceiptNumber = row[STRING_GRD_RECEIPTNO] = val.ReceiptNumber;

            if (dataRow[STRING_GRD_PATIENTID] == null || dataRow[STRING_GRD_PATIENTID] is DBNull)
            {
                data.PatientID = String.Empty;
            }
            else
            {
                data.PatientID = (String)dataRow[STRING_GRD_PATIENTID];
            }

            // 必ず何れかの値が入力されている（初期値で設定されている）
            if (dataRow[STRING_GRD_SPECIMENTYPE] == null || dataRow[STRING_GRD_SPECIMENTYPE] is DBNull)
            {
                data.SpecimenType = CarisXSubFunction.GetSampleKindFromGridItemString(String.Empty);
            }
            else
            {
                data.SpecimenType = CarisXSubFunction.GetSampleKindFromGridItemString((String)dataRow[STRING_GRD_SPECIMENTYPE]);
            }

            // Copy&Pasteの障害ここ
            // 編集データとしてRegisterdを扱わない（DBからの読込で存在する場合のみグリッドへ表示される）
            if (this.editData.ContainsKey(dataRow))
            {
                data.Registered = this.editData[dataRow].Registered;
            }
            else if (this.existData.ContainsKey(dataRow.Index))
            {
                data.Registered = this.existData[dataRow.Index].Registered;
            }
            //            data.Registered = this.splitProtocolNames( (String)dataRow[STRING_GRD_REGISTERED] );
            // 必ず何れかの値が入力されている（初期値で設定されている）
            //            data.ManualDil = this.getManualDilutionRatio( (String)dataRow[STRING_GRD_MANUALDILUTIONRATIO] );//                    row[STRING_GRD_MANUALDILUTIONRATIO]

            data.ManualDil = Convert.ToInt32(dataRow[STRING_GRD_MANUALDILUTIONRATIO]);
            if (dataRow[STRING_GRD_COMMENT] == null || dataRow[STRING_GRD_COMMENT] is DBNull)
            {
                data.Comment = String.Empty;
            }
            else
            {
                data.Comment = (String)dataRow[STRING_GRD_COMMENT];
            }

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
            return this.formSpecimenRegistrationSettings;
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
            this.formSpecimenRegistrationSettings = (FormSpecimenResistrationSettings)setting;
        }

        #endregion
        /// <summary>
        /// 検体登録グリッドセル変更イベント
        /// </summary>
        /// <remarks>
        /// 検体登録グリッドセル変更デバッグログ出力します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenRegistration_CellChange(object sender, CellEventArgs e)
        {
            // アクティブセルの取得
            UltraGridCell currentCell = ((CustomGrid)sender).ActiveCell;

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
                    Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.SpecimenPatientIDformatError, currentCell);
                }
            }
            //e.Cell.Row.
            System.Diagnostics.Debug.WriteLine("grdSpecimenRegistration_CellChange");
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, "grdSpecimenRegistration_CellChange");

            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }
        /// <summary>
        /// 検体登録グリッド編集モード終了前イベント
        /// </summary>
        /// <remarks>
        /// 検体登録グリッド編集確定します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenRegistration_BeforeExitEditMode(object sender, BeforeExitEditModeEventArgs e)
        {
            // TODO:編集セルからのフォーカス抜けをキャンセルするエラー処理はここ
            //this.grdSpecimenRegistration.DisplayLayout.Bands[0].Columns[0].InvalidValueBehavior = InvalidValueBehavior.RevertValueAndRetainFocus;
            //            e.Cancel = true;
            //            e.Cancel = true;
            if (!e.CancellingEditOperation)
            {
                var grd = sender as UltraGrid;
                if (grd.ActiveCell != null)
                {
                    this.updateGridEditData(this.dscSpecimenRegistration.Rows[grd.ActiveCell.Row.Index]);
                }
            }
            System.Diagnostics.Debug.WriteLine("grdSpecimenRegistration_BeforeExitEditMode");
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, "grdSpecimenRegistration_BeforeExitEditMode");
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
        private void setEditdata(String columnName, Int32 rowIndex, String editData)
        {
            if (rowIndex < 0)
            {
                return;
            }
            UltraDataRow editRow = this.dscSpecimenRegistration.Rows[this.grdSpecimenRegistration.Rows[rowIndex].Index];
            switch (columnName)
            {
                // 編集不可能列
                case STRING_GRD_RECEIPTNO:
                case STRING_GRD_REGISTERED:
                    {
                        break;
                    }

                // 編集可能列
                case STRING_GRD_RACKID:         // 分析方法がラックID時編集可
                case STRING_GRD_RACKPOSITION:   // 分析方法がラックID時編集可
                case STRING_GRD_PATIENTID:
                case STRING_GRD_SPECIMENTYPE:
                case STRING_GRD_MANUALDILUTIONRATIO:
                case STRING_GRD_COMMENT:
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
        /// 検体登録グリッド編集モード終了後イベント
        /// </summary>
        /// <remarks>
        /// 検体登録グリッド編集データ確定します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenRegistration_AfterExitEditMode(object sender, EventArgs e)
        {
            UltraGridCell currentCell = ((CustomGrid)sender).ActiveCell;
            if (currentCell != null)
            {
                String columnName = currentCell.Column.Key;
                Int32 rowIndex = currentCell.Row.Index;
                this.setEditdata(columnName, rowIndex, currentCell.Text);
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
        private void grdSpecimenRegistration_CellDataError(object sender, CellDataErrorEventArgs e)
        {
            e.RaiseErrorEvent = false;
        }
        /// <summary>
        /// 検体登録グリッド選択変更後イベント
        /// </summary>
        /// <remarks>
        /// 検体登録グリッド選択変更後デバッグログ出力します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenRegistration_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("grdSpecimenRegistration_AfterSelectChange");
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, "grdSpecimenRegistration_AfterSelectChange");
        }

        /// <summary>
        /// 検体登録グリッドセルクリックイベント
        /// </summary>
        /// <remarks>
        /// 検体登録グリッドセルクリックデバッグログ出力します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void grdSpecimenRegistration_ClickCell(object sender, Infragistics.Win.UltraWinGrid.ClickCellEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine( "grdSpecimenRegistration_AfterSelectChange" );

        }

        /// <summary>
        /// 検体登録グリッド行アクティブ後イベント
        /// </summary>
        /// <remarks>
        /// 保持されている選択項目を分析項目パネルに設定します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenRegistration_AfterRowActivate(object sender, EventArgs e)
        {
            // 保持されている選択項目を分析項目パネルに設定する。
            this.setAnalytesPanel((CustomGrid)sender);

        }

        /// <summary>
        /// 検体登録グリッドセルアクティブ後イベント
        /// </summary>
        /// <remarks>
        /// 分析項目パネル設定処理を実行します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenRegistration_AfterCellActivate(object sender, EventArgs e)
        {
            this.setAnalytesPanel((CustomGrid)sender);
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

            //var selected = sender.SearchSelectRow();
            //if ( selected.Count != 0 )
            if (sender.ActiveRow != null)
            {
                //String columnName = currentCell.Column.Key;
                //Int32 rowIndex = currentCell.Row.Index;
                //                UltraDataRow dataRow = this.dscSpecimenRegistration.Rows[sender.ActiveRow.Index];

                //if ( this.existData.ContainsKey( dataRow ) )
                //{
                //    var registerd = from v in this.editData[dataRow].Registered
                //                    where v.Item1.HasValue
                //                    select v.Item1.Value;
                //    this.analysisSettingPanel.SetProtocolSettingState( registerd.ToList() );

                if (this.existData.ContainsKey(sender.ActiveRow.Index))
                {
                    var registerd = from v in this.existData[sender.ActiveRow.Index].Registered
                                    where v.Item1.HasValue
                                    select new Tuple<Int32, Int32, Int32>(v.Item1.Value, v.Item2.Value, v.Item3.Value);
                    this.analysisSettingPanel.SetProtocolSettingState(registerd.ToList(), true);

                    // 選択行が登録済項目であれば、分析項目未設定を許可しない
                    var currentData = this.createGridViewDataSetFromRow(this.dscSpecimenRegistration.Rows[sender.ActiveRow.Index]);
                    Tuple<String, Int32, String> linkKey = new Tuple<String, Int32, String>(currentData.RackID.DispPreCharString, currentData.RackPosition, currentData.PatientID);
                    this.analysisSettingPanel.SetMustSelection(0);
                    if (this.dbLinkData.ContainsKey(linkKey))
                    {
                        Int32 index = this.dbLinkData[linkKey];
                        if (index < this.dscSpecimenRegistration.Rows.Count)
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

        /// <summary>
        /// FormClosedイベント
        /// </summary>
        /// <remarks>
        /// UI設定保存します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void FormSpecimenRegistration_FormClosed(object sender, FormClosedEventArgs e)
        {
            // UI設定保存
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenResistrationSettings.GridZoom = this.zoomPanel.Zoom;
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenResistrationSettings.GridColOrder = this.grdSpecimenRegistration.GetGridColumnOrder();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenResistrationSettings.GridColWidth = this.grdSpecimenRegistration.GetGridColmnWidth();
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

        private void grdSpecimenRegistration_AfterCellListCloseUp(object sender, CellEventArgs e)
        {
            Int32 rowIdx = e.Cell.Row.Index;
            var value = e.Cell.Value;
        }

    /// <summary>
    /// 分析項目選択ボタンの活性/非活性処理
    /// </summary>
    /// <remarks>
    /// 分析項目の急診と、モジュールの急診使用有無によって分析項目選択ボタンの活性/非活性を切り替える
    /// </remarks>
    private void protocolIndexToButtonDicEnabled()
        {
            enabledFlag = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.IsProtocolEnabledChangedInEmergencyMode();

            // 分析項目パネルに表示されている全ボタンの取得
            foreach (var btn in analysisSettingPanel.ProtocolIndexToButtonDic)
            {
                // ボタンのキーと対応する分析項目のプロトコルを取得
                var protocal = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(btn.Key);

                // 非活性にする必要がある場合
                if (enabledFlag)
                {
                    // 分析項目の急診使用がありの場合非活性、非選択にする
                    if (protocal.UseEmergencyMode == true)
                    {
                        btn.Value.Enabled = false;
                        btn.Value.CurrentState = false;
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
