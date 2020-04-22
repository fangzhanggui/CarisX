using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;


using Infragistics.Win.UltraWinDataSource;

using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.DB;
using Oelco.CarisX.Const;
using Infragistics.Win.UltraWinGrid;
using System.Text.RegularExpressions;
using Oelco.CarisX.Utility;


namespace Oelco.CarisX.GUI
{
    // receiptNo,individuallyNo,measProtocolNoのTuple,ReplicationNo
    using RetestDBKey = Tuple<Int32, Int32, Int32, Int32>;
    using Oelco.CarisX.Log;
    using Oelco.Common.Log;
    using Oelco.CarisX.Comm;
    using Oelco.CarisX.Common;
    using Oelco.CarisX.Status;

    /// <summary>
    /// 検体再検査
    /// </summary>
    public partial class FormSpecimenRetest : FormChildBase
    {
        #region [定数定義]

        /// <summary>
        /// 再検査
        /// </summary>
        private const String RETEST = "Retest";

        /// <summary>
        /// 削除
        /// </summary>
        private const String DELETE = "Delete";

        /// <summary>
        /// 希釈率保存
        /// </summary>
        private const String SAVE_DILUTION_RATIO = "Save dilution ratio";

        /// <summary>
        /// グリッド列名 シーケンス番号
        /// </summary>
        private const String STRING_GRD_SEQUENCE_NO = "Sequence No.";
        /// <summary>
        /// グリッド列名 受付番号
        /// </summary>
        private const String STRING_GRD_RECEIPTNO = "Receipt No.";
        /// <summary>
        /// グリッド列名 ラックID
        /// </summary>
        private const String STRING_GRD_RACKID = "Rack ID";
        /// <summary>
        /// グリッド列名 検体ID
        /// </summary>
        private const String STRING_GRD_PATIENTID = "Patient ID";
        /// <summary>
        /// グリッド列名 ラックポジション
        /// </summary>
        private const String STRING_GRD_RACKPOSITION = "Rack Position";
        /// <summary>
        /// グリッド列名 検体種別
        /// </summary>
        private const String STRING_GRD_SPECIMENTYPE = "Specimen type";
        /// <summary>
        /// グリッド列名 手稀釈倍率
        /// </summary>
        private const String STRING_GRD_MANUALDILUTIONRATIO = "Manual dilution ratio";
        /// <summary>
        /// グリッド列名 自動希釈倍率
        /// </summary>
        private const String STRING_GRD_AUTODILUTIONRATIO = "Auto dilution ratio";
        /// <summary>
        /// グリッド列名 エラー詳細
        /// </summary>
        private const String STRING_GRD_ERROR_DESCRIPTION = "Error description";
        /// <summary>
        /// グリッド列名 分析項目
        /// </summary>
        private const String STRING_GRD_ANALYTES = "Analytes";
        /// <summary>
        /// グリッド列名 カウント値
        /// </summary>
        private const String STRING_GRD_COUNT = "Count";
        /// <summary>
        /// グリッド列名 濃度値
        /// </summary>
        private const String STRING_GRD_CONC = "Concentration";
        /// <summary>
        /// グリッド列名 登録項目
        /// </summary>
        private const String STRING_GRD_REGISTERED = "Registered";
        /// <summary>
        /// グリッド列名 コメント
        /// </summary>
        private const String STRING_GRD_COMMENT = "Coment";

        /// <summary>
        /// グリッド列名 測定日時
        /// </summary>
        private const String STRING_GRD_MEASUREDATETIME = "Measure datetime";


        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormSpecimenRetest()
        {
            InitializeComponent();

            // 拡大率切替コントロール初期化
            this.zoomPanel.ZoomStep = CarisXConst.GRID_ZOOM_STEP;

            // 拡大率変更イベント登録
            this.zoomPanel.SetZoom += grdSpecimenRetest.SetGridZoom;

            // コマンドバーのイベント追加
            this.tlbCommandBar.Tools[RETEST].ToolClick += ( sender, e ) => this.reTest();
            this.tlbCommandBar.Tools[DELETE].ToolClick += ( sender, e ) => this.deleteData();
            this.tlbCommandBar.Tools[SAVE_DILUTION_RATIO].ToolClick += ( sender, e ) => this.saveDilutionRatio();

            // リアルタイムデータ更新イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget( (Int32)NotifyKind.RealtimeData, this.onRealTimeDataChanged );

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
        /// 日付変更
        /// </summary>
        /// <remarks>
        /// 再検査画面 情報更新します
        /// </remarks>
        /// <param name="kind"></param>
        protected void onRealTimeDataChanged( Object kind )
        {
           // Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("isOnlyReMeasure9 Individually={0} ",kind.ToString()));
            // 再検査データ
            if ( ( (RealtimeDataKind)kind ) == RealtimeDataKind.SampleRetest )
            {
               // Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("isOnlyReMeasure9 Individually={0} ", "update!!!!"));
                // 再検査画面 情報更新
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("isOnlyReMeasure8 IsAutoReMeasure={0}", kind.ToString()));
                this.loadGridData();
            }
        }

        /// <summary>
        /// ユーザレベル設定
        /// </summary>
        /// <remarks>
        /// ユーザレベル設定します
        /// </remarks>
        protected override void setUser( Object value )
        {
            // 機能制限

            // 登録削除機能
            this.tlbCommandBar.Tools[DELETE].SharedProps.Visible = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction( CarisXUserLevelManagedAction.SampleDataEditDelete );

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
            // グリッド設定
            this.initializeGridView();

            // グリッド表示データを取得
            this.loadGridData();

            // スクロール処理設定
            this.gesturePanelSpecimen.ScrollProxy = this.grdSpecimenRetest.ScrollProxy;

            // 拡大率切替コントロール初期化
            this.zoomPanel.Zoom = Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenRetestSettings.GridZoom;

            Singleton<NotifyManager>.Instance.AddNotifyTarget( (Int32)NotifyKind.SystemStatusChanged, this.onSystemStatusChanged );
        }

        /// <summary>
        /// システムステータス変化イベント
        /// </summary>
        /// <remarks>
        /// システムステータス変化によりボタン有効/無効状態を切替します
        /// </remarks>
        /// <param name="value"></param>
        protected void onSystemStatusChanged( object value )
        {
            if (Singleton<SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId(Singleton<PublicMemory>.Instance.moduleIndex)] == SystemStatusKind.ReagentExchange)
            {
                this.tlbCommandBar.Tools[RETEST].SharedProps.Enabled = false;
            }
            else
            {
                switch (Singleton<SystemStatus>.Instance.Status)
                {
                    case SystemStatusKind.Assay:
                        // Retestボタンは分析中のみ動作可能
                        this.tlbCommandBar.Tools[RETEST].SharedProps.Enabled = true;
                        break;

                    default:
                        this.tlbCommandBar.Tools[RETEST].SharedProps.Enabled = false;
                        break;
                }
            }
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            this.Text = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_017;

            // グリッド列名設定
            this.grdSpecimenRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_SEQUENCE_NO].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_000;
            this.grdSpecimenRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_RECEIPTNO].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_001;
            this.grdSpecimenRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_PATIENTID].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_002;
            this.grdSpecimenRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_RACKID].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_003;
            this.grdSpecimenRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_RACKPOSITION].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_004;
            this.grdSpecimenRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_SPECIMENTYPE].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_005;
            this.grdSpecimenRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_ANALYTES].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_006;
            this.grdSpecimenRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_MANUALDILUTIONRATIO].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_007;
            this.grdSpecimenRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_AUTODILUTIONRATIO].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_008;
            this.grdSpecimenRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_MEASUREDATETIME].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_019;
            this.grdSpecimenRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_COUNT].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_009;
            this.grdSpecimenRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_CONC].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_010;
            this.grdSpecimenRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_ERROR_DESCRIPTION].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_011;
            this.grdSpecimenRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_COMMENT].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_012;
            this.grdStatRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_SEQUENCE_NO].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_000;
            this.grdStatRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_RECEIPTNO].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_001;
            this.grdStatRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_PATIENTID].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_002;
            this.grdStatRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_RACKID].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_003;
            this.grdStatRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_RACKPOSITION].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_004;
            this.grdStatRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_SPECIMENTYPE].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_005;
            this.grdStatRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_ANALYTES].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_006;
            this.grdStatRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_MANUALDILUTIONRATIO].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_007;
            this.grdStatRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_AUTODILUTIONRATIO].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_008;
            this.grdStatRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_MEASUREDATETIME].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_019;
            this.grdStatRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_COUNT].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_009;
            this.grdStatRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_CONC].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_010;
            this.grdStatRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_ERROR_DESCRIPTION].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_011;
            this.grdStatRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_COMMENT].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_012;

            this.tbpSpecimenRetest.Tab.Text = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_025;
            this.tbpStatRetest.Tab.Text = Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_026;

            // コマンドバーアイテム名設定
            this.tlbCommandBar.Tools[RETEST].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_008;
            this.tlbCommandBar.Tools[DELETE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_002;
            this.tlbCommandBar.Tools[SAVE_DILUTION_RATIO].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_009;

            // 倍率選択設定
            this.grdSpecimenRetest.DisplayLayout.ValueLists[0].ValueListItems.Clear();
            this.grdSpecimenRetest.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_013.Remove(0, 1));
            this.grdSpecimenRetest.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_014.Remove(0, 1));
            this.grdSpecimenRetest.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_020.Remove(0, 1));
            this.grdSpecimenRetest.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_015.Remove(0, 1));
            this.grdSpecimenRetest.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_018.Remove(0, 1));
            this.grdSpecimenRetest.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_021.Remove(0, 1));
            this.grdSpecimenRetest.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_016.Remove(0, 1));
            this.grdSpecimenRetest.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_022.Remove(0, 1));
            this.grdSpecimenRetest.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_023.Remove(0, 1));
            this.grdSpecimenRetest.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_024.Remove(0, 1));
            this.grdStatRetest.DisplayLayout.ValueLists[0].ValueListItems.Clear();
            this.grdStatRetest.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_013.Remove(0, 1));
            this.grdStatRetest.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_014.Remove(0, 1));
            this.grdStatRetest.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_020.Remove(0, 1));
            this.grdStatRetest.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_015.Remove(0, 1));
            this.grdStatRetest.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_018.Remove(0, 1));
            this.grdStatRetest.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_021.Remove(0, 1));
            this.grdStatRetest.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_016.Remove(0, 1));
            this.grdStatRetest.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_022.Remove(0, 1));
            this.grdStatRetest.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_023.Remove(0, 1));
            this.grdStatRetest.DisplayLayout.ValueLists[0].ValueListItems.Add(Oelco.CarisX.Properties.Resources.STRING_SPECIMENRETEST_024.Remove(0, 1));

        }

        /// <summary>
        /// グリッド表示初期化
        /// </summary>
        /// <remarks>
        /// グリッド表示内容の空状態を作成します。
        /// </remarks>
        protected void initializeGridView()
        {
            // グリッド初期化
            this.dscSpecimenRetest.Rows.Clear();
            this.dscStatRetest.Rows.Clear();

            // 編集可能列背景色設定
            this.grdSpecimenRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_MANUALDILUTIONRATIO].CellAppearance.BackColor = CarisXConst.GRID_CELL_CAN_EDIT_COLOR2;
            this.grdSpecimenRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_AUTODILUTIONRATIO].CellAppearance.BackColor = CarisXConst.GRID_CELL_CAN_EDIT_COLOR2;
            this.grdStatRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_MANUALDILUTIONRATIO].CellAppearance.BackColor = CarisXConst.GRID_CELL_CAN_EDIT_COLOR2;
            this.grdStatRetest.DisplayLayout.Bands[0].Columns[STRING_GRD_AUTODILUTIONRATIO].CellAppearance.BackColor = CarisXConst.GRID_CELL_CAN_EDIT_COLOR2;


            // 背景色の設定
            this.grdSpecimenRetest.SetGridRowBackgroundColorRuleFromIndex(1, new List<Color>() { CarisXConst.GRID_ROWS_DEFAULT_COLOR, CarisXConst.GRID_ROWS_COLOR_PATTERN2 });
            this.grdStatRetest.SetGridRowBackgroundColorRuleFromIndex(1, new List<Color>() { CarisXConst.GRID_ROWS_DEFAULT_COLOR, CarisXConst.GRID_ROWS_COLOR_PATTERN2 });

            // グリッド表示順
            this.grdSpecimenRetest.SetGridColumnOrder( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenRetestSettings.GridColOrder );
            this.grdStatRetest.SetGridColumnOrder(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.StatRetestSettings.GridColOrder);

            // グリッド列幅
            this.grdSpecimenRetest.SetGridColmnWidth( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenRetestSettings.GridColWidth );
            this.grdStatRetest.SetGridColmnWidth(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.StatRetestSettings.GridColWidth);
        }

        /// <summary>
        /// 編集データ
        /// </summary>
        private Dictionary<UltraDataRow, SpecimenReMeasureGridViewDataSet> editData = new Dictionary<UltraDataRow, SpecimenReMeasureGridViewDataSet>();
        private Dictionary<UltraDataRow, SpecimenStatReMeasureGridViewDataSet> editDataStat = new Dictionary<UltraDataRow, SpecimenStatReMeasureGridViewDataSet>();

        /// <summary>
        /// DBレコードマッピングデータ(Gridの行Index-DBのキー項目)
        /// </summary>
        private Dictionary<Int32, RetestDBKey> dbLinkData = new Dictionary<Int32, RetestDBKey>();
        private Dictionary<Int32, RetestDBKey> dbLinkDataStat = new Dictionary<Int32, RetestDBKey>();

        /// <summary>
        /// DBデータ（編集前）
        /// </summary>
        private List<SpecimenReMeasureGridViewDataSet> dispDataOriginal = new List<SpecimenReMeasureGridViewDataSet>();
        private List<SpecimenStatReMeasureGridViewDataSet> dispDataOriginalStat = new List<SpecimenStatReMeasureGridViewDataSet>();

        /// <summary>
        /// DBのデータに対して、編集データを重ね合わせた作業用データ(グリッドRowインデックス->データ) rowIndex ,data
        /// </summary>
        Dictionary<Int32, SpecimenReMeasureGridViewDataSet> existData = new Dictionary<Int32, SpecimenReMeasureGridViewDataSet>();
        Dictionary<Int32, SpecimenStatReMeasureGridViewDataSet> existDataStat = new Dictionary<Int32, SpecimenStatReMeasureGridViewDataSet>();

        /// <summary>
        /// グリッドセルの編集可否設定
        /// </summary>
        /// <remarks>
        /// グリッドセルの編集可否設定します
        /// </remarks>
        /// <param name="row"></param>
        private void setItemEditable( UltraGridRow row )
        {
            row.Cells[STRING_GRD_SEQUENCE_NO].Activation = Activation.NoEdit;
            row.Cells[STRING_GRD_RECEIPTNO].Activation = Activation.NoEdit;
            row.Cells[STRING_GRD_PATIENTID].Activation = Activation.NoEdit;
            row.Cells[STRING_GRD_RACKID].Activation = Activation.NoEdit;
            row.Cells[STRING_GRD_RACKPOSITION].Activation = Activation.NoEdit;
            row.Cells[STRING_GRD_SPECIMENTYPE].Activation = Activation.NoEdit;
            row.Cells[STRING_GRD_ANALYTES].Activation = Activation.NoEdit;
            row.Cells[STRING_GRD_MANUALDILUTIONRATIO].Activation = Activation.AllowEdit;
            row.Cells[STRING_GRD_AUTODILUTIONRATIO].Activation = Activation.AllowEdit;
            row.Cells[STRING_GRD_MEASUREDATETIME].Activation = Activation.NoEdit;
            row.Cells[STRING_GRD_COUNT].Activation = Activation.NoEdit;
            row.Cells[STRING_GRD_CONC].Activation = Activation.NoEdit;
            row.Cells[STRING_GRD_ERROR_DESCRIPTION].Activation = Activation.NoEdit;
            row.Cells[STRING_GRD_COMMENT].Activation = Activation.NoEdit;
        }

        /// <summary>
        /// グリッドデータ読込
        /// </summary>
        /// <remarks>
        /// グリッド表示データをDBから取得します。
        /// </remarks>
        private void loadGridData( Int32 currentIndex = 0 )
        {
            #region Specimen
            // DBからデータを取得
            Singleton<SpecimenReMeasureDB>.Instance.LoadDB();
            this.dispDataOriginal = Singleton<SpecimenReMeasureDB>.Instance.GetDispData();
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("dispDataOriginal count={0}", this.dispDataOriginal.Count));

            // グリッドにデータを設定
            this.dscSpecimenRetest.Rows.Clear();
            this.dbLinkData.Clear();
            this.existData.Clear();
            foreach ( SpecimenReMeasureGridViewDataSet val in dispDataOriginal )
            {
                this.dscSpecimenRetest.Rows.Add();
                Int32 lastIndex = this.dscSpecimenRetest.Rows.Count - 1;
                UltraDataRow row = this.dscSpecimenRetest.Rows[lastIndex];
                row[STRING_GRD_SEQUENCE_NO] = val.SequenceNumber.ToString();
                row[STRING_GRD_RECEIPTNO] = val.ReceiptNumber.ToString();
                row[STRING_GRD_PATIENTID] = val.SampleID;
                row[STRING_GRD_RACKID] = val.RackID.Value == 0 ? String.Empty : val.RackID.DispPreCharString;
                row[STRING_GRD_RACKPOSITION] = val.RackPosition == 0 ? String.Empty : val.RackPosition.ToString();
                row[STRING_GRD_SPECIMENTYPE] = CarisXSubFunction.GetSampleKindGridItemString( val.SpecimenMaterialType );
                MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo( val.MeasprotocolNo );
                row[STRING_GRD_ANALYTES] = protocol != null ? protocol.ProtocolName : String.Empty;
                row[STRING_GRD_MANUALDILUTIONRATIO] = val.ManualDilution.ToString();
                row[STRING_GRD_AUTODILUTIONRATIO] = val.AutoDilution.ToString();
                row[STRING_GRD_MEASUREDATETIME] = val.MeasureDateTime;
                row[STRING_GRD_COUNT] = val.Count;
                row[STRING_GRD_CONC] = val.Concentration;
                row[STRING_GRD_ERROR_DESCRIPTION] = String.Join( CarisX.Properties.Resources.STRING_COMMON_006, val.Remark.GetRemarkStrings() );
                row[STRING_GRD_COMMENT] = val.Comment;
                UltraGridRow gridRow = this.grdSpecimenRetest.Rows[lastIndex];
                this.setItemEditable(gridRow);
         
                this.dbLinkData.Add( row.Index, new RetestDBKey(val.ReceiptNumber, val.IndividuallyNo, val.MeasprotocolNo, val.ReplicationNo ) );
                this.existData[row.Index] = val;

                // スレーブの急診モード使用フラグ
                bool enabledFlag = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.IsProtocolEnabledChangedInEmergencyMode();

                // 試薬、スレーブ両方で急診の使用がありの場合
                if (( protocol.UseEmergencyMode == true ) && ( enabledFlag == false ))
                {
                    // 自動希釈倍率のグリッドセルの選択、編集ができないようにする
                    gridRow.Cells[STRING_GRD_AUTODILUTIONRATIO].Activation = Activation.Disabled;
                }
                else
                {
                    // 自動希釈倍率のグリッドセルのアクティブ化は上部のsetItemEditable(gridRow)で行われるため、ここでは不要
                }
            }

            // 設定行を選択状態にする。
            if ( grdSpecimenRetest.Rows.Count > 0 )
            {
                this.grdSpecimenRetest.Rows[currentIndex].Selected = true;
                this.grdSpecimenRetest.Rows[currentIndex].Activate();
            }
            #endregion

            #region STAT
            // DBからデータを取得
            Singleton<SpecimenStatReMeasureDB>.Instance.LoadDB();
            this.dispDataOriginalStat = Singleton<SpecimenStatReMeasureDB>.Instance.GetDispData();
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("dispDataOriginalStat count={0}", this.dispDataOriginalStat.Count));

            // グリッドにデータを設定
            this.dscStatRetest.Rows.Clear();
            this.dbLinkDataStat.Clear();
            this.existDataStat.Clear();
            foreach (SpecimenStatReMeasureGridViewDataSet val in dispDataOriginalStat)
            {
                this.dscStatRetest.Rows.Add();
                Int32 lastIndex = this.dscStatRetest.Rows.Count - 1;
                UltraDataRow row = this.dscStatRetest.Rows[lastIndex];
                row[STRING_GRD_SEQUENCE_NO] = val.SequenceNumber.ToString();
                row[STRING_GRD_RECEIPTNO] = val.ReceiptNumber.ToString();
                row[STRING_GRD_PATIENTID] = val.SampleID;
                row[STRING_GRD_RACKID] = val.RackID.ToString();
                row[STRING_GRD_RACKPOSITION] = val.RackPosition == 0 ? String.Empty : val.RackPosition.ToString();
                row[STRING_GRD_SPECIMENTYPE] = CarisXSubFunction.GetSampleKindGridItemString(val.SpecimenMaterialType);
                MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(val.MeasprotocolNo);
                row[STRING_GRD_ANALYTES] = protocol != null ? protocol.ProtocolName : String.Empty;
                row[STRING_GRD_MANUALDILUTIONRATIO] = val.ManualDilution.ToString();
                row[STRING_GRD_AUTODILUTIONRATIO] = val.AutoDilution.ToString();
                row[STRING_GRD_MEASUREDATETIME] = val.MeasureDateTime;
                row[STRING_GRD_COUNT] = val.Count;
                row[STRING_GRD_CONC] = val.Concentration;
                row[STRING_GRD_ERROR_DESCRIPTION] = String.Join(CarisX.Properties.Resources.STRING_COMMON_006, val.Remark.GetRemarkStrings());
                row[STRING_GRD_COMMENT] = val.Comment;
                UltraGridRow gridRow = this.grdStatRetest.Rows[lastIndex];
                this.setItemEditable(gridRow);

                this.dbLinkDataStat.Add(row.Index, new RetestDBKey(val.ReceiptNumber, val.IndividuallyNo, val.MeasprotocolNo, val.ReplicationNo));
                this.existDataStat[row.Index] = val;

                // スレーブの急診モード使用フラグ
                bool enabledFlag = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.IsProtocolEnabledChangedInEmergencyMode();

                // 試薬、スレーブ両方で急診の使用がありの場合
                if (( protocol.UseEmergencyMode == true ) && ( enabledFlag == false ))
                {
                    // 自動希釈倍率のグリッドセルの選択、編集ができないようにする
                    gridRow.Cells[STRING_GRD_AUTODILUTIONRATIO].Activation = Activation.Disabled;
                }
                else
                {
                    // 自動希釈倍率のグリッドセルのアクティブ化は上部のsetItemEditable(gridRow)で行われるため、ここでは不要
                }
            }

            // 設定行を選択状態にする。
            if (grdStatRetest.Rows.Count > 0)
            {
                this.grdStatRetest.Rows[currentIndex].Selected = true;
                this.grdStatRetest.Rows[currentIndex].Activate();
            }
            #endregion
        }
        #endregion

        #region [privateメソッド]

        /// <summary>
        /// 再検査検体情報削除
        /// </summary>
        /// <remarks>
        /// 再検査検体情報削除します
        /// </remarks>
        protected void deleteData()
        {
            List<UltraGridRow> grdData = new List<UltraGridRow>();

            if (tabSpecimenRetest.SelectedTab.TabPage == tbpSpecimenRetest)
                grdData = this.grdSpecimenRetest.SearchSelectRow();
            else
                grdData = this.grdStatRetest.SearchSelectRow();

            if (grdData.Count != 0 )
            {
                // 操作履歴：削除実行
                Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_003 } );

                // 確認メッセージ
                if ( DialogResult.OK != DlgMessage.Show( CarisX.Properties.Resources.STRING_DLG_MSG_038, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel ) )
                {
                    // 操作履歴：削除キャンセル
                    Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_004 } );
                    return;
                }

                List<RetestDBKey> delList = new List<RetestDBKey>();

                foreach ( UltraGridRow val in grdData)
                {
                    if (tabSpecimenRetest.SelectedTab.TabPage == tbpSpecimenRetest)
                    {
                        UltraDataRow dataRow = this.dscSpecimenRetest.Rows[val.Index];
                        delList.Add(this.dbLinkData[val.Index]);

                        this.editData.Remove(dataRow);          // 編集データ削除
                    }
                    else
                    {
                        UltraDataRow dataRow = this.dscStatRetest.Rows[val.Index];
                        delList.Add(this.dbLinkDataStat[val.Index]);

                        this.editDataStat.Remove(dataRow);      // 編集データ削除
                    }
                }

                // 削除・コミット・グリッド表示内容再生成
                if (tabSpecimenRetest.SelectedTab.TabPage == tbpSpecimenRetest)
                {
                    Singleton<SpecimenReMeasureDB>.Instance.DeleteData(delList);
                    Singleton<SpecimenReMeasureDB>.Instance.CommitSampleReMeasureInfo();
                }
                else
                {
                    Singleton<SpecimenStatReMeasureDB>.Instance.DeleteData(delList);
                    Singleton<SpecimenStatReMeasureDB>.Instance.CommitSampleReMeasureInfo();
                }

                this.initializeGridView();
                this.loadGridData();

                // Form共通の編集中フラグOFF
                FormChildBase.IsEdit = false;
            }
        }

        /// <summary>
        /// 再検査データの登録
        /// </summary>
        /// <remarks>
        /// 再検査リストから検体DBへ選択したデータを登録します。
        /// </remarks>
        protected void reTest()
        {
            //TODO:操作履歴
            if (tabSpecimenRetest.SelectedTab.TabPage == tbpSpecimenRetest)
            {
                //Specimenタブを選択している場合
                List<SpecimenReMeasureGridViewDataSet> retestData = new List<SpecimenReMeasureGridViewDataSet>();
                IEnumerable<UltraGridRow> selectedRep = null;

                if (this.grdSpecimenRetest.ActiveCell != null && this.grdSpecimenRetest.ActiveCell.IsInEditMode)
                {
                    // 編集中セルからフォーカスを抜ける。
                    // 変更中セル内容が確定できない場合、中断する。
                    this.grdSpecimenRetest.ActiveCell.Activated = false;
                    if (this.grdSpecimenRetest.ActiveCell != null)
                    {
                        return;
                    }
                }

                // 確認ダイアログ表示
                TargetRange outputRange = DlgTargetSelectRange.Show();

                if ( outputRange == TargetRange.All )
                {
                    // 全体なら。
                    selectedRep = this.grdSpecimenRetest.Rows.OfType<UltraGridRow>().ToList();
                }
                else if ( outputRange == TargetRange.Specification )
                {
                    // 選択なら。
                    List<UltraGridRow> selected = this.grdSpecimenRetest.SearchSelectRow();

                    // 同一ポジションのデータへ選択範囲を拡張する。It wants to extend the selection to the same position of the data.
                    selectedRep = from convertedRow in
                                        from v in this.grdSpecimenRetest.Rows.OfType<UltraGridRow>()
                                        select new
                                        {
                                            Dat = this.createGridViewDataSetFromRow(this.dscSpecimenRetest.Rows[v.Index]),
                                            Row = v
                                        }
                                    from convertedSelectRow in
                                        from vv in selected
                                        select this.createGridViewDataSetFromRow(this.dscSpecimenRetest.Rows[vv.Index])
                                    where (convertedRow.Dat.IndividuallyNo == convertedSelectRow.IndividuallyNo) &&
                                    (convertedRow.Dat.RackID.DispPreCharString == convertedSelectRow.RackID.DispPreCharString) &&
                                    (convertedRow.Dat.RackPosition == convertedSelectRow.RackPosition)
                                    select convertedRow.Row;
                }
                else
                {
                    // キャンセルCancel
                }
                if ( selectedRep != null )
                {
                    // 選択した項目を再検査コマンドで送り、グリッドに対しては非表示状態となるようにする
                    // 実際にデータベースからデータが削除されるのは、再検査内容が測定指示問い合わせコマンドにより、問い合わせされた時。
                    // 選択した項目を再検査コマンドで送信する前に、指定のIDのラックがサンプリングステージにある場合、エラーとなる。
                    // 自動再検査の場合はサンプリングステージから戻るった時に動作させる
                    Boolean allowRetest = false;
                    Int32 retestSendModuleNo = -1;

                    foreach ( UltraGridRow row in selectedRep )
                    {
                        var data = this.createGridViewDataSetFromRow( this.dscSpecimenRetest.Rows[row.Index] );
                        var findData = CarisXSubFunction.SearchRack( data.RackID );
                        if ( findData == RackFindResult.NotFound )
                        {
                            // 選択しているラックが見つからないため、再検査不可
                            //レーンに存在しないラックの再検査を試みました
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("tried to re-examination of the rack that does not exist in the lane{0}-{1}", data.RackID, data.RackPosition));
                            allowRetest = false;
                            break;
                        }
                        else if ( findData != RackFindResult.FindOnCollectRack )
                        {
                            //選択しているラックが回収レーン以外で見つかった為、再検査不可
                            DlgMessage.Show(Properties.Resources.STRING_DLG_MSG_253, "", "", MessageDialogButtons.OK);
                            allowRetest = false;
                            break;
                        }
                        retestSendModuleNo = Singleton<ReagentDB>.Instance.GetTargetModuleNo(data.MeasprotocolNo, data.ModuleNo);
                        if (retestSendModuleNo == -1)
                        {
                            //再検査を行えるモジュールが無い為、再検査不可
                            DlgMessage.Show(Properties.Resources.STRING_DLG_MSG_254, "", "", MessageDialogButtons.OK);
                            allowRetest = false;
                            break;
                        }
                        allowRetest = true;
                        data.WaitMeasureIndicate = true;//再検コマンドの待機開始を設定する（设置开始等待重测命令）
                        data.IndividuallyNo = this.dbLinkData[row.Index].Item2;
                        retestData.Add( data );
                    }
                    if ( allowRetest )
                    {
                        // 再検DB更新
                        Singleton<SpecimenReMeasureDB>.Instance.SetDispData(retestData);
                        Singleton<SpecimenReMeasureDB>.Instance.CommitSampleReMeasureInfo();

                        // 編集データをクリアする。
                        this.editData.Clear();

                        // 保存データを読み直す
                        this.initializeGridView();
                        this.loadGridData();
                    }
                }
            }
            else
            {
                //STATタブを選択している場合
                List<SpecimenStatReMeasureGridViewDataSet> retestData = new List<SpecimenStatReMeasureGridViewDataSet>();
                IEnumerable<UltraGridRow> selectedRep = null;

                if (this.grdStatRetest.ActiveCell != null && this.grdStatRetest.ActiveCell.IsInEditMode)
                {
                    // 編集中セルからフォーカスを抜ける。
                    // 変更中セル内容が確定できない場合、中断する。
                    this.grdStatRetest.ActiveCell.Activated = false;
                    if (this.grdStatRetest.ActiveCell != null)
                    {
                        return;
                    }
                }

                //既に登録済の測定指示問合せ待ちのSTAT再検が存在する場合は再検指示を行えない
                if (Singleton<SpecimenStatReMeasureDB>.Instance.existsWaitMeasureIndicate())
                {
                    DlgMessage.Show(Properties.Resources.STRING_DLG_MSG_261, String.Empty, Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.Confirm);
                    return;
                }

                //選択行が１行以外の場合は再検指示を行えない
                List<UltraGridRow> selected = this.grdStatRetest.SearchSelectRow();
                if (selected.Count != 1)
                {
                    // 2つ以上選択してるので注意を促すダイアログを表示する
                    DlgMessage.Show(Properties.Resources.STRING_DLG_MSG_175, String.Empty, Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.Confirm);
                    return;
                }

                // 同一ポジションのデータへ選択範囲を拡張する。
                selectedRep = from convertedRow in
                                  from v in this.grdStatRetest.Rows.OfType<UltraGridRow>()
                                  select new
                                  {
                                      Dat = this.createGridViewDataSetFromStatRow(this.dscStatRetest.Rows[v.Index]),
                                      Row = v
                                  }
                              from convertedSelectRow in
                                  from vv in selected
                                  select this.createGridViewDataSetFromStatRow(this.dscStatRetest.Rows[vv.Index])
                              where (convertedRow.Dat.IndividuallyNo == convertedSelectRow.IndividuallyNo) &&
                              (convertedRow.Dat.RackID.DispPreCharString == convertedSelectRow.RackID.DispPreCharString) &&
                              (convertedRow.Dat.RackPosition == convertedSelectRow.RackPosition)
                              select convertedRow.Row;

                if (selectedRep != null)
                {
                    foreach (UltraGridRow row in selectedRep)
                    {
                        var data = this.createGridViewDataSetFromStatRow(this.dscStatRetest.Rows[row.Index]);

                        data.WaitMeasureIndicate = true;//再検コマンドの待機開始を設定する（设置开始等待重测命令）
                        data.IndividuallyNo = this.dbLinkDataStat[row.Index].Item2;
                        retestData.Add(data);
                    }

                    // STAT再検DB更新
                    Singleton<SpecimenStatReMeasureDB>.Instance.SetDispData(retestData);
                    Singleton<SpecimenStatReMeasureDB>.Instance.CommitSampleReMeasureInfo();

                    // 編集データをクリアする。
                    this.editDataStat.Clear();

                    // 保存データを読み直す
                    this.initializeGridView();
                    this.loadGridData();
                }

                // 一時登録データの検体IDを取得する
                String registedTemporaryPatientId = retestData[0].SampleID;

                List<Int32> protocolList = new List<Int32>();

                foreach (var data in retestData)
                {
                    protocolList.Add(data.MeasprotocolNo);
                }

                CarisXSubFunction.ShowCheckStatMeasurableModule(registedTemporaryPatientId, protocolList);
            }
        }

        /// <summary>
        /// 希釈倍率保存
        /// </summary>
        /// <remarks>
        /// 希釈倍率保存します
        /// </remarks>
        protected void saveDilutionRatio()
        {
            if (tabSpecimenRetest.SelectedTab.TabPage == tbpSpecimenRetest)
            {
                // アクティブ行インデックの退避
                var selected = this.grdSpecimenRetest.SearchSelectRow();
                Int32 currentRowIndex = 0;
                if (selected.Count != 0)
                {
                    currentRowIndex = selected.First().Index;
                }

                // TODO:失敗ケース
                if (this.grdSpecimenRetest.ActiveCell != null && this.grdSpecimenRetest.ActiveCell.IsInEditMode)
                {
                    // 編集中セルからフォーカスを抜ける。
                    // 変更中セル内容が確定できない場合、中断する。
                    this.grdSpecimenRetest.ActiveCell.Activated = false;
                    if (this.grdSpecimenRetest.ActiveCell != null)
                    {
                        return;
                    }
                }

                if (this.editData.Count != 0)
                {
                    // TODO:ここまでに編集データチェック
                    Singleton<SpecimenReMeasureDB>.Instance.SetDispData(this.editData.Values.ToList());
                    Singleton<SpecimenReMeasureDB>.Instance.CommitSampleReMeasureInfo();

                    // 編集データをクリアする。
                    this.editData.Clear();

                    this.initializeGridView();
                    this.loadGridData(currentRowIndex);
                }
            }
            else
            {
                // アクティブ行インデックスの退避
                var selected = this.grdStatRetest.SearchSelectRow();
                Int32 currentRowIndex = 0;
                if (selected.Count != 0)
                {
                    currentRowIndex = selected.First().Index;
                }

                // TODO:失敗ケース
                if (this.grdStatRetest.ActiveCell != null && this.grdStatRetest.ActiveCell.IsInEditMode)
                {
                    // 編集中セルからフォーカスを抜ける。
                    // 変更中セル内容が確定できない場合、中断する。
                    this.grdStatRetest.ActiveCell.Activated = false;
                    if (this.grdStatRetest.ActiveCell != null)
                    {
                        return;
                    }
                }

                if (this.editDataStat.Count != 0)
                {
                    // TODO:ここまでに編集データチェック
                    Singleton<SpecimenStatReMeasureDB>.Instance.SetDispData(this.editDataStat.Values.ToList());
                    Singleton<SpecimenStatReMeasureDB>.Instance.CommitSampleReMeasureInfo();

                    // 編集データをクリアする。
                    this.editDataStat.Clear();

                    this.initializeGridView();
                    this.loadGridData(currentRowIndex);
                }
            }

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// グリッド行データから再検査データ作成
        /// </summary>
        /// <remarks>
        /// グリッド行データから再検査データ作成します
        /// </remarks>
        /// <param name="dataRow">行データ</param>
        /// <returns>再検査データ</returns>
        private SpecimenReMeasureGridViewDataSet createGridViewDataSetFromRow( UltraDataRow dataRow )
        {
            // グリッドからデータを取得
            SpecimenReMeasureGridViewDataSet data = new SpecimenReMeasureGridViewDataSet();
            data.SequenceNumber = Int32.Parse( (String)dataRow[STRING_GRD_SEQUENCE_NO] );
            data.ReceiptNumber = Int32.Parse( (String)( dataRow[STRING_GRD_RECEIPTNO] ) );
            data.SampleID = (String)dataRow[STRING_GRD_PATIENTID];
            data.RackID = (String)dataRow[STRING_GRD_RACKID];
            Int32.TryParse( (String)dataRow[STRING_GRD_RACKPOSITION], out data.RackPosition );
            data.SpecimenMaterialType = CarisXSubFunction.GetSampleKindFromGridItemString( (String)dataRow[STRING_GRD_SPECIMENTYPE] );

            // 検体識別番号を取得する
            data.IndividuallyNo = this.dbLinkData[dataRow.Index].Item2;
            data.ReplicationNo = this.dbLinkData[dataRow.Index].Item4;

            data.ModuleNo = CarisXConst.ALL_MODULEID;

            MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromName( (String)dataRow[STRING_GRD_ANALYTES] );
            data.MeasprotocolNo = protocol.ProtocolNo;
            data.ManualDilution = Int32.Parse( ( (String)dataRow[STRING_GRD_MANUALDILUTIONRATIO] ) );
            data.AutoDilution = Int32.Parse( ( (String)dataRow[STRING_GRD_AUTODILUTIONRATIO] ) );
            data.Count = dataRow[STRING_GRD_COUNT] == DBNull.Value ? (Int32?)null : (String)dataRow[STRING_GRD_COUNT] == "****" ? (Int32?)null : (Int32?)Int32.Parse( (String)dataRow[STRING_GRD_COUNT] );
            data.Concentration = dataRow[STRING_GRD_CONC] == DBNull.Value ? (String)null : (String)dataRow[STRING_GRD_CONC] == "****" ? (String)null : (String)dataRow[STRING_GRD_CONC];
            data.Remark.SetRemarkStrings( ( (String)( dataRow[STRING_GRD_ERROR_DESCRIPTION] == DBNull.Value ? String.Empty : dataRow[STRING_GRD_ERROR_DESCRIPTION] ) ).Split( ",".ToCharArray() ) );
            data.Comment = (String)dataRow[STRING_GRD_COMMENT];

            return data;
        }

        /// <summary>
        /// グリッド行データから再検査データ作成
        /// </summary>
        /// <remarks>
        /// グリッド行データから再検査データ作成します
        /// </remarks>
        /// <param name="dataRow">行データ</param>
        /// <returns>再検査データ</returns>
        private SpecimenStatReMeasureGridViewDataSet createGridViewDataSetFromStatRow(UltraDataRow dataRow)
        {
            // グリッドからデータを取得
            SpecimenStatReMeasureGridViewDataSet data = new SpecimenStatReMeasureGridViewDataSet();
            data.SequenceNumber = Int32.Parse((String)dataRow[STRING_GRD_SEQUENCE_NO]);
            data.ReceiptNumber = Int32.Parse((String)(dataRow[STRING_GRD_RECEIPTNO]));
            data.SampleID = (String)dataRow[STRING_GRD_PATIENTID];
            data.RackID = (String)dataRow[STRING_GRD_RACKID];
            Int32.TryParse((String)dataRow[STRING_GRD_RACKPOSITION], out data.RackPosition);
            data.SpecimenMaterialType = CarisXSubFunction.GetSampleKindFromGridItemString((String)dataRow[STRING_GRD_SPECIMENTYPE]);

            // 検体識別番号を取得する
            data.IndividuallyNo = this.dbLinkDataStat[dataRow.Index].Item2;
            data.ReplicationNo = this.dbLinkDataStat[dataRow.Index].Item4;

            data.ModuleNo = CarisXConst.ALL_MODULEID;

            MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromName((String)dataRow[STRING_GRD_ANALYTES]);
            data.MeasprotocolNo = protocol.ProtocolNo;
            data.ManualDilution = Int32.Parse(((String)dataRow[STRING_GRD_MANUALDILUTIONRATIO]));
            data.AutoDilution = Int32.Parse(((String)dataRow[STRING_GRD_AUTODILUTIONRATIO]));
            data.Count = dataRow[STRING_GRD_COUNT] == DBNull.Value ? (Int32?)null : (String)dataRow[STRING_GRD_COUNT] == "****" ? (Int32?)null : (Int32?)Int32.Parse((String)dataRow[STRING_GRD_COUNT]);
            data.Concentration = dataRow[STRING_GRD_CONC] == DBNull.Value ? (String)null : (String)dataRow[STRING_GRD_CONC] == "****" ? (String)null : (String)dataRow[STRING_GRD_CONC];
            data.Remark.SetRemarkStrings(((String)(dataRow[STRING_GRD_ERROR_DESCRIPTION] == DBNull.Value ? String.Empty : dataRow[STRING_GRD_ERROR_DESCRIPTION])).Split(",".ToCharArray()));
            data.Comment = (String)dataRow[STRING_GRD_COMMENT];

            return data;
        }

        /// <summary>
        /// 再測定検体グリッドデータを更新
        /// </summary>
        /// <remarks>
        /// 再測定検体グリッドデータを更新します
        /// </remarks>
        /// <param name="row"></param>
        /// <returns></returns>
        private Boolean updateGridEditData( UltraDataRow row )
        {
            Boolean result = true;
            SpecimenReMeasureGridViewDataSet setData = this.createGridViewDataSetFromRow( row );

            // 編集状態更新（処理が重い場合、setEditDataにて1レコードずつやる）
            foreach ( KeyValuePair<UltraDataRow, SpecimenReMeasureGridViewDataSet> dat in this.editData )
            {
                // editDataのkeyはグリッドデータと紐づいており、グリッドデータがクリアされた際にeditData.key.Indexの値が-1になることがある。
                // indexが-1の場合
                if (dat.Key.Index == -1)
                {
                    // DBレコードマッピングデータから編集データと同一データを持のグリッドインデックスを取得する
                    var targetData = dbLinkData.Where(v => (v.Value.Item1 == dat.Value.ReceiptNumber) 
                                                         && (v.Value.Item2 == dat.Value.IndividuallyNo) 
                                                         && (v.Value.Item3 == dat.Value.MeasprotocolNo)
                                                         && (v.Value.Item4 == dat.Value.ReplicationNo));

                    // 対象のデータがnullではなく、対象のデータがある場合
                    if ((targetData != null )&& (targetData.Count() > 0))
                    {
                        // 最初の対象データのkeyを取得
                        Int32 targetDataIndex = targetData.FirstOrDefault().Key;
                        this.existData[targetDataIndex] = dat.Value;
                    }
                }
                else
                {
                    this.existData[dat.Key.Index] = dat.Value;
                }

                

            }

            // データの重複チェック等はユーザ入力による編集がないので行わない。
            var data = this.createGridViewDataSetFromRow( row );
            this.editData[row] = data;

            // 同一測定項目(シーケンス番号が同じ場合)に対する希釈倍率同期処理 
            // 手希釈は検体全体に同時反映する。
            var syncDatasManualDil = from v in this.existData
                                     where v.Value.SequenceNumber == data.SequenceNumber
                                     select v;
            foreach ( var sameSample in syncDatasManualDil )
            {
                // 追従先のデータを設定
                var editTargetRow = this.dscSpecimenRetest.Rows[sameSample.Key];
                var editTargetData = this.createGridViewDataSetFromRow( editTargetRow );

                editTargetData.ManualDilution = data.ManualDilution;
                this.editData[editTargetRow] = editTargetData;

                sameSample.Value.ManualDilution = data.ManualDilution;
                this.dscSpecimenRetest.Rows[sameSample.Key][STRING_GRD_MANUALDILUTIONRATIO] = data.ManualDilution;
            }
            // 自動希釈は分析項目全体に同時反映する。
            var syncDatasAutoDil = from v in this.existData
                                   where v.Value.SequenceNumber == data.SequenceNumber && v.Value.MeasprotocolNo == data.MeasprotocolNo
                                   select v;
            foreach ( var sameSample in syncDatasAutoDil )
            {
                // 追従先のデータを設定
                var editTargetRow = this.dscSpecimenRetest.Rows[sameSample.Key];
                var editTargetData = this.createGridViewDataSetFromRow( editTargetRow );
                editTargetData.AutoDilution = data.AutoDilution;
                this.editData[editTargetRow] = editTargetData;

                sameSample.Value.AutoDilution = data.AutoDilution;
                this.dscSpecimenRetest.Rows[sameSample.Key][STRING_GRD_AUTODILUTIONRATIO] = data.AutoDilution;
            }
            this.grdSpecimenRetest.DataBind();

            return result;

        }

        /// <summary>
        /// 再測定検体グリッドデータを更新
        /// </summary>
        /// <remarks>
        /// 再測定検体グリッドデータを更新します
        /// </remarks>
        /// <param name="row"></param>
        /// <returns></returns>
        private Boolean updateGridEditDataStat(UltraDataRow row)
        {
            Boolean result = true;
            SpecimenStatReMeasureGridViewDataSet setData = this.createGridViewDataSetFromStatRow(row);

            // 編集状態更新（処理が重い場合、setEditDataにて1レコードずつやる）
            foreach (KeyValuePair<UltraDataRow, SpecimenStatReMeasureGridViewDataSet> dat in this.editDataStat)
            {
                // editDataのkeyはグリッドデータと紐づいており、グリッドデータがクリアされた際にeditData.key.Indexの値が-1になることがある。
                // indexが-1の場合
                if (dat.Key.Index == -1)
                {
                    // DBレコードマッピングデータから編集データと同一データを持のグリッドインデックスを取得する
                    var targetData = dbLinkDataStat.Where(v => ( v.Value.Item1 == dat.Value.ReceiptNumber )
                                                         && ( v.Value.Item2 == dat.Value.IndividuallyNo )
                                                         && ( v.Value.Item3 == dat.Value.MeasprotocolNo )
                                                         && ( v.Value.Item4 == dat.Value.ReplicationNo ));

                    // 対象のデータがnullではなく、対象のデータがある場合
                    if (( targetData != null ) && ( targetData.Count() > 0 ))
                    {
                        // 最初の対象データのkeyを取得
                        Int32 targetDataIndex = targetData.FirstOrDefault().Key;
                        this.existDataStat[targetDataIndex] = dat.Value;
                    }
                }
                else
                {
                    this.existDataStat[dat.Key.Index] = dat.Value;
                }
            }

            // データの重複チェック等はユーザ入力による編集がないので行わない。
            var data = this.createGridViewDataSetFromStatRow(row);
            this.editDataStat[row] = data;

            // 同一測定項目(シーケンス番号が同じ場合)に対する希釈倍率同期処理
            // 手希釈は検体全体に同時反映する。
            var syncDatasManualDil = from v in this.existDataStat
                                     where v.Value.SequenceNumber == data.SequenceNumber
                                     select v;
            foreach (var sameSample in syncDatasManualDil)
            {
                // 追従先のデータを設定
                var editTargetRow = this.dscStatRetest.Rows[sameSample.Key];
                var editTargetData = this.createGridViewDataSetFromStatRow(editTargetRow);

                editTargetData.ManualDilution = data.ManualDilution;
                this.editDataStat[editTargetRow] = editTargetData;

                sameSample.Value.ManualDilution = data.ManualDilution;
                this.dscStatRetest.Rows[sameSample.Key][STRING_GRD_MANUALDILUTIONRATIO] = data.ManualDilution;
            }
            // 自動希釈は分析項目全体に同時反映する。
            var syncDatasAutoDil = from v in this.existDataStat
                                   where v.Value.SequenceNumber == data.SequenceNumber && v.Value.MeasprotocolNo == data.MeasprotocolNo
                                   select v;
            foreach (var sameSample in syncDatasAutoDil)
            {
                // 追従先のデータを設定
                var editTargetRow = this.dscStatRetest.Rows[sameSample.Key];
                var editTargetData = this.createGridViewDataSetFromStatRow(editTargetRow);
                editTargetData.AutoDilution = data.AutoDilution;
                this.editDataStat[editTargetRow] = editTargetData;

                sameSample.Value.AutoDilution = data.AutoDilution;
                this.dscStatRetest.Rows[sameSample.Key][STRING_GRD_AUTODILUTIONRATIO] = data.AutoDilution;
            }
            this.grdStatRetest.DataBind();

            return result;

        }

        /// <summary>
        /// 編集データ設定
        /// </summary>
        /// <remarks>
        /// 再測定検体グリッドに編集データを反映します
        /// </remarks>
        /// <param name="columnName"></param>
        /// <param name="rowIndex"></param>
        /// <param name="editData"></param>
        private void setEditdata( String columnName, Int32 rowIndex, String editData )
        {
            if ( rowIndex < 0 )
            {
                return;
            }

            if (tabSpecimenRetest.SelectedTab.TabPage == tbpSpecimenRetest)
            {
                UltraDataRow editRow = this.dscSpecimenRetest.Rows[this.grdSpecimenRetest.Rows[rowIndex].Index];
                switch (columnName)
                {
                    // 編集可能列
                    case STRING_GRD_AUTODILUTIONRATIO:
                    case STRING_GRD_MANUALDILUTIONRATIO:
                        {
                            // 編集内容を更新
                            this.updateGridEditData(editRow);
                            break;
                        }
                    default:
                        // 編集不可能列
                        break;
                }
            }
            else
            {
                UltraDataRow editRow = this.dscStatRetest.Rows[this.grdStatRetest.Rows[rowIndex].Index];
                switch (columnName)
                {
                    // 編集可能列
                    case STRING_GRD_AUTODILUTIONRATIO:
                    case STRING_GRD_MANUALDILUTIONRATIO:
                        {
                            // 編集内容を更新
                            this.updateGridEditDataStat(editRow);
                            break;
                        }
                    default:
                        // 編集不可能列
                        break;
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
        private void FormSpecimenRetest_FormClosed( object sender, FormClosedEventArgs e )
        {
            // UI設定保存
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenRetestSettings.GridZoom = this.zoomPanel.Zoom;
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenRetestSettings.GridColOrder = this.grdSpecimenRetest.GetGridColumnOrder();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SpecimenRetestSettings.GridColWidth = this.grdSpecimenRetest.GetGridColmnWidth();
        }

        /// <summary>
        /// セル編集終了イベント
        /// </summary>
        /// <remarks>
        /// 再測定検体グリッド編集データ設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void grdSpecimenRetest_AfterExitEditMode( object sender, EventArgs e )
        {

            UltraGridCell currentCell = ( (CustomGrid)sender ).ActiveCell;
            String columnName = currentCell.Column.Key;
            Int32 rowIndex = currentCell.Row.Index;
            this.setEditdata( columnName, rowIndex, currentCell.Text );
        }

        private void grdSpecimenRetest_BeforeExitEditMode(object sender, BeforeExitEditModeEventArgs e)
        {
            if (!e.CancellingEditOperation)
            {
                var grd = sender as UltraGrid;
                if (grd.ActiveCell != null && grd.ActiveCell.Column.Key == STRING_GRD_AUTODILUTIONRATIO)
                {
                    UltraDataRow editRow = this.dscSpecimenRetest.Rows[grd.ActiveRow.Index];

                    //プロトコル情報の取得
                    MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromName((String)editRow[STRING_GRD_ANALYTES]);
                    
                    //画面に表示する希釈倍率の内容を制御
                    if (((Int32)protocol.ProtocolDilutionRatio * int.Parse(grd.ActiveCell.Text)) > CarisXConst.MaxDILUTION)
                    {
                        DlgMessage.Show(Properties.Resources.STRING_DLG_MSG_260, String.Empty, Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.Confirm);
                        if (e.ForceExit)
                        {
                            grd.ActiveCell.CancelUpdate();
                            return;
                        }
                        e.Cancel = true;
                    }
                }
            }
        }

        /// <summary>
        /// セル編集終了イベント
        /// </summary>
        /// <remarks>
        /// 再測定検体グリッド編集データ設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void grdStatRetest_AfterExitEditMode(object sender, EventArgs e)
        {
            UltraGridCell currentCell = ((CustomGrid)sender).ActiveCell;
            String columnName = currentCell.Column.Key;
            Int32 rowIndex = currentCell.Row.Index;
            this.setEditdata(columnName, rowIndex, currentCell.Text);
        }

        private void grdStatRetest_BeforeExitEditMode(object sender, BeforeExitEditModeEventArgs e)
        {
            if (!e.CancellingEditOperation)
            {
                var grd = sender as UltraGrid;
                if (grd.ActiveCell != null && grd.ActiveCell.Column.Key == STRING_GRD_AUTODILUTIONRATIO)
                {
                    UltraDataRow editRow = this.dscStatRetest.Rows[grd.ActiveRow.Index];

                    //プロトコル情報の取得
                    MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromName((String)editRow[STRING_GRD_ANALYTES]);

                    //画面に表示する希釈倍率の内容を制御
                    if (((Int32)protocol.ProtocolDilutionRatio * int.Parse(grd.ActiveCell.Text)) > CarisXConst.MaxDILUTION)
                    {
                        DlgMessage.Show(Properties.Resources.STRING_DLG_MSG_260, String.Empty, Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.Confirm);
                        if (e.ForceExit)
                        {
                            grd.ActiveCell.CancelUpdate();
                            return;
                        }
                        e.Cancel = true;
                    }
                }
            }
        }

        /// <summary>
        /// 検体再検査グリッドセル変更イベント
        /// </summary>
        /// <remarks>
        /// 値変更時、Form共通の編集中フラグをONにします。
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSpecimenRetest_CellChange(object sender, CellEventArgs e)
        {
            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        /// <summary>
        /// 緊急検体再検査グリッドセル変更イベント
        /// </summary>
        /// <remarks>
        /// 値変更時、Form共通の編集中フラグをONにします。
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdStatRetest_CellChange(object sender, CellEventArgs e)
        {
            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        /// <summary>
        /// タブページを切り替える時、Form共通の編集中フラグチェックを実施
        /// </summary>
        /// <remarks>
        /// 共通フラグを使用しているので、複数のタブページで編集した場合に判定が行えない為、
        /// タブページ毎にチェックを実施します。
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabSpecimenRetest_ActiveTabChanging(object sender, Infragistics.Win.UltraWinTabControl.ActiveTabChangingEventArgs e)
        {
            // 編集中かどうか、また編集中でも画面遷移するかどうか
            if (CarisXSubFunction.IsEditsMessageShow())
            {
                // タブページを切り替える

                // Form共通の編集中フラグOFF
                FormChildBase.IsEdit = false;
            }
            else
            {
                // タブページを切り替えない
                e.Cancel = true;
            }
        }

        #endregion
    }
}
