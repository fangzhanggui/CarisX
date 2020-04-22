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
using Oelco.CarisX.DB;
using Oelco.Common.Utility;
using Oelco.CarisX.Parameter;
using Infragistics.UltraChart.Resources.Appearance;
using Infragistics.UltraChart.Shared.Styles;
using Oelco.Common.Parameter;
using Infragistics.Win.UltraWinGrid;
using Oelco.CarisX.Const;
using Oelco.CarisX.Print;
using System.Drawing.Imaging;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Infragistics.UltraChart.Core;
using Oelco.CarisX.Utility;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 精度管理検体精度管理画面クラス
    /// </summary>
    public partial class FormControlQC : FormChildBase
    {
        #region [定数定義]

        /// <summary>
        /// 保存
        /// </summary>
        private const String SAVE = "Save";

        /// <summary>
        /// 削除
        /// </summary>
        private const String DELETE = "Delete";

        /// <summary>
        /// 印刷
        /// </summary>
        private const String PRINT = "Print";

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 現在選択中の分析項目
        /// </summary>
        private MeasureProtocol currentMeasureProtocol = null;

        /// <summary>
        /// 精度管理情報
        /// </summary>
        private ControlQCData currentControlQCData = null;

        /// <summary>
        /// 現在データ選択中の精度管理検体測定結果情報
        /// </summary>
        private List<ControlResultDataQC> currentControlResultData = new List<ControlResultDataQC>();

        /// <summary>
        /// 現在データ選択中の日別精度管理権t内測定結果情報
        /// </summary>
        private IEnumerable<IGrouping<DateTime, ControlResultDataQC>> currentDaypoint = null;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormControlQC()
        {
            InitializeComponent();
            chtInterDayVarlation.DataBind();

            // コマンドバーのイベント追加
            this.tlbCommandBar.Tools[SAVE].ToolClick += ( sender, e ) => this.saveData();
            this.tlbCommandBar.Tools[DELETE].ToolClick += ( sender, e ) => this.deleteData();
            this.tlbCommandBar.Tools[PRINT].ToolClick += ( sender, e ) => this.printData();
            // 印刷パラメータ変更イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget( (Int32)NotifyKind.UseOfPrint, this.onPrintParamChanged );
            // ユーザレベル変更イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget( (Int32)NotifyKind.UserLevelChanged, this.setUser );

            this.grdControlResultDataList.SetGridRowBackgroundColorRuleFromCellData(
                new[] { ControlResultDataQC.DataKeys.SequenceNo }.ToList(),
                new[] { CarisXConst.GRID_ROWS_DEFAULT_COLOR, CarisXConst.GRID_ROWS_COLOR_PATTERN1 }.ToList() );

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
            // スクロール処理設定
            this.gesturePanel.ScrollProxy = this.grdControlResultDataList.ScrollProxy;
            // グリッド表示順
            this.grdControlResultDataList.SetGridColumnOrder( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.ControlQCSettings.GridColOrder );
            // グリッド列幅
            this.grdControlResultDataList.SetGridColmnWidth( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.ControlQCSettings.GridColWidth );

            // 印刷ボタン表示設定
            this.tlbCommandBar.Tools[PRINT].SharedProps.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable;
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            this.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_036;

            this.tlbCommandBar.Tools[SAVE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_001;
            this.tlbCommandBar.Tools[DELETE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_002;
            this.tlbCommandBar.Tools[PRINT].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_004;

            this.btnReferenceValueInput.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_035;
            this.btnSelectAnayte.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_001;
            this.lblTitleControlName.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_002;
            this.lblTitleControlLotNo.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_003;
            this.lblTitlePeriod.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_004;
            this.tabControlQC.Tabs[0].Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_005;
            this.tabControlQC.Tabs[1].Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_006;
            this.lblInterDayVarlation.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_007;
            this.lblRControlChart.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_008;
            this.lblIntraDayVarlation.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_009;
            this.lblTitleInterDayMean.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_010;
            this.lblTitleInterDaySD.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_011;
            this.lblTitleIntraDayDate.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_012;
            this.lblTitleIntraDayMean.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_010;
            this.lblTitleIntraDayCVPercent.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_013;
            this.lblTitleIntraDaySD.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_011;
            this.lblTitleIntraDayR.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_014;
            this.btnInterDayEditReferenceValue.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_030;
            this.btnRControlEditReferenceValue.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_030;
            this.btnEditYAxis.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_031;

            this.chtInterDayVarlation.TitleTop.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_007;
            this.chtInterDayVarlation.TitleLeft.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_017;
            this.chtInterDayVarlation.TitleBottom.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_015;
            this.chtRControl.TitleTop.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_008;
            this.chtRControl.TitleLeft.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_018;
            this.chtRControl.TitleBottom.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_015;
            this.chtIntraDayVarlation.TitleTop.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_009;
            this.chtIntraDayVarlation.TitleLeft.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_017;
            this.chtIntraDayVarlation.TitleBottom.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_016;

            this.lblEditMode.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_019;
            this.optEditMode.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_020;
            this.optEditMode.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_021;

            this.grdControlResultDataList.DisplayLayout.Bands[0].Columns[ControlResultDataQC.DataKeys.SequenceNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_022;
            this.grdControlResultDataList.DisplayLayout.Bands[0].Columns[ControlResultDataQC.DataKeys.ControlName].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_023;
            this.grdControlResultDataList.DisplayLayout.Bands[0].Columns[ControlResultDataQC.DataKeys.ControlLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_024;
            this.grdControlResultDataList.DisplayLayout.Bands[0].Columns[ControlResultDataQC.DataKeys.ReagentLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_025;
            this.grdControlResultDataList.DisplayLayout.Bands[0].Columns[ControlResultDataQC.DataKeys.Concentration].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_026;
            this.grdControlResultDataList.DisplayLayout.Bands[0].Columns[ControlResultDataQC.DataKeys.ConcentrationAve].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_027;
            this.grdControlResultDataList.DisplayLayout.Bands[0].Columns[ControlResultDataQC.DataKeys.MeasureDateTime].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_028;

            this.lblHyphon.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;
        }

        /// <summary>
        /// ユーザレベル設定
        /// </summary>
        /// <remarks>
        /// ユーザレベルを設定します
        /// </remarks>
        protected override void setUser( Object value )
        {
            base.setUser(value);

            //【IssuesNo:18】质控精度管理相关的编辑功能移至（检测结果删除功能）权限下进行管理
            Boolean enable = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction( CarisXUserLevelManagedAction.SampleDataEditDelete);
            gbxEditMode.Visible = enable;
            optEditMode.Enabled = enable;
            this.tlbCommandBar.Tools[SAVE].SharedProps.Enabled = enable;
            this.tlbCommandBar.Tools[DELETE].SharedProps.Enabled = enable;

            // EditModeが非表示の場合はReadを選択状態にする
            //【IssuesNo:18】质控精度管理相关的编辑功能移至移至（检测结果删除功能）权限下进行管理
            if ( !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction( CarisXUserLevelManagedAction.SampleDataEditDelete) )
            {
                this.optEditMode.CheckedIndex = 0;
            }
            this.Refresh();

        }

        #endregion

        #region [privateメソッド]

        #region _コマンドバー_

        /// <summary>
        /// 削除
        /// </summary>
        /// <remarks>
        /// 操作履歴に削除実行を登録し、精度管理検体測定結果データの削除実行して反映を行います
        /// </remarks>
        private void deleteData()
        {
            // 操作履歴：削除実行  
            Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_003 } );

            TargetRange targetRange = DlgTargetSelectRange.Show( Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_032, Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_033, Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_034 );   //"削除範囲指定", "選択期間削除", "リスト選択範囲削除" );
            // 削除対象を取得
            List<ControlResultDataQC> deleteData = null;
            switch ( targetRange )
            {
            case TargetRange.All:
                deleteData = this.currentControlResultData;
                break;
            case TargetRange.Specification:
                deleteData = this.grdControlResultDataList.SearchSelectRow().Select( ( row ) => ( (ControlResultDataQC)row.ListObject ) ).ToList();
                break;
            case TargetRange.None:
                // 操作履歴：削除キャンセル
                Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_004 } );
                return;
            }

            if ( deleteData == null || deleteData.Count == 0 )
            {
                DlgMessage.Show( CarisX.Properties.Resources.STRING_DLG_MSG_067, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.Confirm );
                return;
            }
            deleteData.ForEach( ( data ) => data.DeleteData() );
            Singleton<ControlResultDB>.Instance.SetData( deleteData );
            Singleton<ControlResultDB>.Instance.CommitData();

            var controlNameDatas = Singleton<ControlResultDB>.Instance.GetData( this.currentMeasureProtocol.ProtocolIndex );

            // 精度管理検体名(昇順)
            var controlNames = controlNameDatas.Select( ( data ) => data.ControlName ).OrderBy( ( str ) => str ).Distinct().ToList();
            if ( !controlNames.Contains( this.cmbControlName.SelectedItem.DisplayText ) )
            {
                this.cmbControlName.DataSource = controlNames;
                this.cmbControlName.DataBind();
                this.cmbControlName.SelectedIndex = 0;
            }
            else
            {
                var controlLotNoDatas = Singleton<ControlResultDB>.Instance.GetDataQC( this.currentMeasureProtocol.ProtocolIndex, null, null, this.cmbControlName.Text );

                // 精度管理検体ロット(降順)
                var controlLotNoList = controlLotNoDatas.Select( ( data ) => data.ControlLotNo ).OrderByDescending( ( str ) => str ).Distinct().ToList();
                if ( !controlLotNoList.Contains( this.cmbControlLotNo.SelectedItem.DisplayText ) )
                {
                    this.InitYAxisEditting();
                    controlLotNoList.Add( Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_000 );
                    this.cmbControlLotNo.DataSource = controlLotNoList;
                    this.cmbControlLotNo.DataBind();
                    this.cmbControlLotNo.SelectedIndex = 0;
                }
            }
            this.reloadDispData();

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <remarks>
        /// 操作履歴に登録実行を登録し、精度管理検体測定結果データの登録実行して反映を行います
        /// </remarks>
        private void saveData()
        {
            if ( this.currentMeasureProtocol != null )
            {
                // プロトコルが選択されていた時下記メッセージ表示
                DlgMessage.Show( CarisX.Properties.Resources.STRING_DLG_MSG_136, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.Confirm );

                if ( this.grdControlResultDataList.ActiveCell != null && this.grdControlResultDataList.ActiveCell.IsInEditMode )
                {
                    this.grdControlResultDataList.ActiveCell.Activated = false;
                    if ( this.grdControlResultDataList.ActiveCell != null )
                    {
                        return;
                    }
                }

                // 操作履歴登録：登録実行
                Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_009 } );

                // 変更を反映
                Singleton<ControlResultDB>.Instance.SetData( this.currentControlResultData );
                Singleton<ControlResultDB>.Instance.CommitData();

                this.reloadDispData();

                // Form共通の編集中フラグOFF
                FormChildBase.IsEdit = false;
            }
            else
            {
                // プロトコルが選択されていない時下記メッセージ表示
                DlgMessage.Show( CarisX.Properties.Resources.STRING_DLG_MSG_068, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.Confirm );
            }
        }

        /// <summary>
        /// 印刷
        /// </summary>
        /// <remarks>
        /// 操作履歴に印刷実行を登録し、精度管理検体測定結果データの印刷を実行します
        /// </remarks>
        private void printData()
        {
            // 操作履歴登録：印刷実行
            Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_005 } );

            if ( this.currentMeasureProtocol == null )
            {
                // 印刷するデータがありません。
                DlgMessage.Show( CarisX.Properties.Resources.STRING_DLG_MSG_064, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_003, MessageDialogButtons.Confirm );
                return;
            }

            // 印刷対象を取得
            List<ControlResultDataQC> printData = null;
            // データ全件取得
            Singleton<ControlResultDB>.Instance.LoadDB();
            if ( this.cmbControlLotNo.Text != Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_000 )
            {
                printData = Singleton<ControlResultDB>.Instance.GetDataQC( this.currentMeasureProtocol.ProtocolIndex, (DateTime)this.btnPeriodStart.Tag, (DateTime)this.btnPeriodEnd.Tag, this.cmbControlName.Text, this.cmbControlLotNo.Text );
            }
            else
            {
                printData = Singleton<ControlResultDB>.Instance.GetDataQC( this.currentMeasureProtocol.ProtocolIndex, (DateTime)this.btnPeriodStart.Tag, (DateTime)this.btnPeriodEnd.Tag, this.cmbControlName.Text );
            }

            if ( printData == null || printData.Count == 0 )
            {
                // 印刷するデータがありません。
                DlgMessage.Show( CarisX.Properties.Resources.STRING_DLG_MSG_064, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_003, MessageDialogButtons.Confirm );
            }
            else
            {
                // 精度管理データを印刷します。よろしいですか？
                if ( DialogResult.OK == DlgMessage.Show( String.Format( CarisX.Properties.Resources.STRING_DLG_MSG_120, CarisX.Properties.Resources.STRING_DLG_MSG_190 ), String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel ) )
                {
                    ControlQCPrint prt = new ControlQCPrint();

                    // グラフ部帳票
                    // 印刷用Listに取得データを格納
                    List<ControlQCReportData> rptData = new List<ControlQCReportData>();
                    ControlQCReportData rptDataRow = new ControlQCReportData();
                    rptDataRow.Analytes = lblAnalyte.Text;
                    rptDataRow.ControlName = cmbControlName.Text;
                    rptDataRow.ControlLot = cmbControlLotNo.Text;
                    rptDataRow.TermStart = btnPeriodStart.Text;
                    rptDataRow.TermEnd = btnPeriodEnd.Text;
                    rptDataRow.InterDayReferenceMean = ( (Double)( this.currentControlQCData.Mean ?? 0 ) ).ToString( "0.00" );
                    rptDataRow.InterDayReferenceConcRange = ( (Double)( this.currentControlQCData.ConcentrationWidth ?? 0 ) ).ToString( "0.00" );
                    rptDataRow.InterDayStatMean = lblInterDayMean.Text;
                    rptDataRow.InterDayStatSD = lblInterDaySD.Text;
                    rptDataRow.RControlChart = ( (Double)( this.currentControlQCData.ControlR ?? 0 ) ).ToString( "0.00" );
                    rptDataRow.IntraDayDate = ( (DateTime)this.btnIntraDayDate.Tag ).Date.ToShortDateString();
                    rptDataRow.IntraDayMean = lblIntraDayMean.Text;
                    rptDataRow.IntraDaySD = lblIntraDaySD.Text;
                    rptDataRow.IntraDayCV = lblIntraDayCVPercent.Text;
                    rptDataRow.IntraDayR = lblIntraDayR.Text;
                    rptDataRow.PrintDateTime = DateTime.Now.ToDispString();
                    rptData.Add( rptDataRow );

                    try
                    {

                        String savePath1 = prt.SavePathXBar.Replace("'", String.Empty);
                        String savePath2 = prt.SavePathRBar.Replace("'", String.Empty);
                        String savePath3 = prt.SavePathDey.Replace("'", String.Empty);

                        // 画面グラフをキャプチャし、ファイルに保存
                        // 拡縮サイズ0.82倍
                        double imageRatio = 0.82;

                        // 画像を拡縮
                        // デフォルトのサイズだとA4紙に収まらず、見切れたため縮小を行う
                        Bitmap saveBmp1 = CarisXSubFunction.ScalingBitmap( chtInterDayVarlation.Image, imageRatio );
                        Bitmap saveBmp2 = CarisXSubFunction.ScalingBitmap( chtRControl.Image, imageRatio );
                        Bitmap saveBmp3 = CarisXSubFunction.ScalingBitmap( chtIntraDayVarlation.Image, imageRatio );

                        // 保存
                        saveBmp1.Save( savePath1 );
                        saveBmp2.Save( savePath2 );
                        saveBmp3.Save( savePath3 );

                        // 印刷実行
                        Boolean ret = prt.Print(rptData);

                        System.IO.File.Delete(savePath1);
                        System.IO.File.Delete(savePath2);
                        System.IO.File.Delete(savePath3);

                        saveBmp1.Dispose();
                        saveBmp2.Dispose();
                        saveBmp3.Dispose();
                    }
                    catch ( Exception ex )
                    {
                        System.Diagnostics.Debug.WriteLine( ex.Message );
                        Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                                  CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
                    }

                    // グリッド部帳票
                    // 印刷用Listに取得データを格納
                    ControlQCDetailPrint prt1 = new ControlQCDetailPrint();
                    List<ControlQCDetailReportData> rptData1 = new List<ControlQCDetailReportData>();
                    foreach ( var row in printData )
                    {
                        ControlQCDetailReportData rptDataRow1 = new ControlQCDetailReportData();
                        rptDataRow1.Analytes = lblAnalyte.Text;
                        rptDataRow1.ControlName = cmbControlName.Text;
                        rptDataRow1.ControlLot = cmbControlLotNo.Text;
                        rptDataRow1.TermStart = btnPeriodStart.Text;
                        rptDataRow1.TermEnd = btnPeriodEnd.Text;
                        rptDataRow1.SequenceNo = row.SequenceNo.ToString();
                        rptDataRow1.Concentration = row.Concentration;
                        rptDataRow1.ConcentrationAve = row.ConcentrationAve;
                        rptDataRow1.MeasureDate = row.MeasureDateTime.ToString();
                        rptDataRow1.ReagentLotNo = row.ReagentLotNo;
                        rptDataRow1.PrintDateTime = rptDataRow.PrintDateTime;

                        rptData1.Add( rptDataRow1 );
                    }
                    Boolean ret1 = prt1.Print( rptData1 );
                }
                else
                {
                    // 操作履歴登録：印刷キャンセル
                    Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_006 } );
                }
            }
        }

        #endregion

        /// <summary>
        /// 日差変動データ設定ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 日差変動データ設定を行います
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnInterDayEditReferenceValue_Click( object sender, EventArgs e )
        {
            this.editInterDayReferenceValue();
        }

        /// <summary>
        /// 日差変動データ設定
        /// </summary>
        /// <remarks>
        /// 操作履歴に登録し、精度管理情報の日差変動データを設定します
        /// </remarks>
        private void editInterDayReferenceValue()
        {
            // 操作履歴
            Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_045 } );

            // 精度管理情報を設定
            using ( DlgInterDayReferenceValueInput dlg = new DlgInterDayReferenceValueInput() )
            {
                if ( this.currentControlQCData != null )
                {
                    dlg.Mean = this.currentControlQCData.Mean ?? 0;
                    dlg.ConcentrationWidth = this.currentControlQCData.ConcentrationWidth ?? 0;
                    dlg.MaskInput = CarisXSubFunction.GetConcNumericEditorMaskInput( this.currentMeasureProtocol.ProtocolIndex );

                    if ( dlg.ShowDialog() == DialogResult.OK )
                    {
                        this.currentControlQCData.Mean = dlg.Mean;
                        this.currentControlQCData.ConcentrationWidth = dlg.ConcentrationWidth;

                        this.saveControlQCData();

                        this.loadControlQCData();
                    }
                    else
                    {
                        // 操作履歴    
                        Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_046 } );
                    }
                }
            }
        }

        /// <summary>
        /// R管理図データ設定ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// R管理図データ設定を行います
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnRControlEditReferenceValue_Click( object sender, EventArgs e )
        {
            this.editRControlReferenceValue();

        }

        /// <summary>
        /// R管理図データ設定
        /// </summary>
        /// <remarks>
        /// 精度管理情報のR管理図データを設定します
        /// </remarks>
        private void editRControlReferenceValue()
        {
            // 精度管理情報を設定
            using ( DlgRControlReferenceValueInput dlg = new DlgRControlReferenceValueInput() )
            {
                if ( this.currentControlQCData != null )
                {
                    dlg.ControlR = this.currentControlQCData.ControlR ?? 0;
                    dlg.MaskInput = CarisXSubFunction.GetConcNumericEditorMaskInput( this.currentMeasureProtocol.ProtocolIndex );

                    if ( dlg.ShowDialog() == DialogResult.OK )
                    {
                        this.currentControlQCData.ControlR = dlg.ControlR;

                        this.saveControlQCData();

                        this.loadControlQCData();
                    }
                }
            }
        }
        /// <summary>
        /// 管理情報の読み込み
        /// </summary>
        /// <remarks>
        /// 精度管理情報を読み込みします
        /// </remarks>
        private void loadControlQCData()
        {
            if ( this.currentMeasureProtocol == null )
            {
                return;
            }
            // 精度管理情報を取得
            if ( Singleton<ParameterFilePreserve<ControlQC>>.Instance.Load() )
            {
                Func<ControlQCData, Boolean> match = ( data ) =>
                {
                    String lotNo = ( ( this.cmbControlLotNo.Text != Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_000 ) ? this.cmbControlLotNo.Text : ControlQCData.ALL );
                    return data.MeasureProtocolIndex == this.currentMeasureProtocol.ProtocolIndex
                        && data.ControlLotNo == lotNo
                    && data.ControlName == this.cmbControlName.Text;
                };
                this.currentControlQCData = Singleton<ParameterFilePreserve<ControlQC>>.Instance.Param.ControlQCList.LastOrDefault( match );
            }
            this.currentControlQCData = this.currentControlQCData ?? new Func<ControlQCData>( () =>
            {
                var data = new ControlQCData();
                data.MeasureProtocolIndex = this.currentMeasureProtocol.ProtocolIndex;
                data.ControlName = this.cmbControlName.Text;
                if ( this.cmbControlLotNo.Text != Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_000 )
                {
                    data.ControlLotNo = this.cmbControlLotNo.Text;
                }
                else
                {
                    data.ControlLotNo = ControlQCData.ALL;
                }
                return data;
            } )();

            // 日差変動グラフ
            if ( this.currentControlQCData.Mean != null )
            {
                // 平均線表示
                var line = this.chtInterDayVarlation.YAxisLineItems.FirstOrDefault() ?? new YAxisLine();
                line.Value = this.currentControlQCData.Mean.Value;
                if ( !this.chtInterDayVarlation.YAxisLineItems.Contains( line ) )
                {
                    this.chtInterDayVarlation.YAxisLineItems.Add( line );
                }

                // 濃度幅範囲帯表示
                var zone = this.chtInterDayVarlation.YAxisZoneItems.FirstOrDefault() ?? new YAxisZone();
                zone.Value1 = this.currentControlQCData.Mean.Value + this.currentControlQCData.ConcentrationWidth.Value;
                zone.Value2 = this.currentControlQCData.Mean.Value - this.currentControlQCData.ConcentrationWidth.Value;
                if ( !this.chtInterDayVarlation.YAxisZoneItems.Contains( zone ) )
                {
                    this.chtInterDayVarlation.YAxisZoneItems.Add( zone );
                }
            }
            else
            {
                this.chtInterDayVarlation.YAxisLineItems.Clear();
                this.chtInterDayVarlation.YAxisZoneItems.Clear();
            }
            this.chtInterDayVarlation.DataBind();

            // R管理図グラフ
            if ( this.currentControlQCData.ControlR != null )
            {
                // R管理値線表示
                var line = this.chtRControl.YAxisLineItems.FirstOrDefault() ?? new YAxisLine();
                line.Value = this.currentControlQCData.ControlR.Value;
                if ( !this.chtRControl.YAxisLineItems.Contains( line ) )
                {
                    this.chtRControl.YAxisLineItems.Add( line );
                }

                // R管理値範囲帯表示
                var zone = this.chtRControl.YAxisZoneItems.FirstOrDefault() ?? new YAxisZone();
                zone.Value1 = this.currentControlQCData.ControlR.Value;
                zone.Value2 = 0;
                if ( !this.chtRControl.YAxisZoneItems.Contains( zone ) )
                {
                    this.chtRControl.YAxisZoneItems.Add( zone );
                }
            }
            else
            {
                this.chtRControl.YAxisLineItems.Clear();
                this.chtRControl.YAxisZoneItems.Clear();
            }
            this.chtRControl.DataBind();
        }

        /// <summary>
        /// 現在の精度管理情報を保存
        /// </summary>
        /// <remarks>
        /// 精度管理情報を保存します
        /// </remarks>
        private void saveControlQCData()
        {
            if ( !Singleton<ParameterFilePreserve<ControlQC>>.Instance.Param.ControlQCList.Contains( this.currentControlQCData ) )
            {
                Singleton<ParameterFilePreserve<ControlQC>>.Instance.Param.ControlQCList.Add( this.currentControlQCData );
            }
            Singleton<ParameterFilePreserve<ControlQC>>.Instance.Save();
        }


        /// <summary>
        /// Y軸スケール設定ボタンイベント
        /// </summary>
        /// <remarks>
        /// Y軸スケールを設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnEditYAxis_Click( object sender, EventArgs e )
        {
            this.editYAxis();
        }

        /// <summary>
        /// Y軸スケール設定
        /// </summary>
        /// <remarks>
        /// 日差変動、R管理図、日内変動を設定します
        /// </remarks>
        private void editYAxis()
        {
            using ( DlgYAxisEdit dlg = new DlgYAxisEdit() )
            {
                // 日差変動
                dlg.InterDayMax = this.chtInterDayVarlation.Axis.Y.RangeMax;
                dlg.InterDayMin = this.chtInterDayVarlation.Axis.Y.RangeMin;
                if ( dlg.InterDayMax == dlg.InterDayMin )
                {
                    dlg.InterDayMax += 1;
                }

                // R管理図
                dlg.RControlMax = this.chtRControl.Axis.Y.RangeMax;
                dlg.RControlMin = this.chtRControl.Axis.Y.RangeMin;
                if ( dlg.RControlMax == dlg.RControlMin )
                {
                    dlg.RControlMax += 1;
                }
                // 日内変動
                dlg.IntraDayMax = this.chtIntraDayVarlation.Axis.Y.RangeMax;
                dlg.IntraDayMin = this.chtIntraDayVarlation.Axis.Y.RangeMin;
                if ( dlg.IntraDayMax == dlg.IntraDayMin )
                {
                    dlg.IntraDayMax += 1;
                }

                if ( DialogResult.OK == dlg.ShowDialog() )
                {
                    // 日差変動
                    //this.chtInterDayVarlation.Axis.Y.RangeType = AxisRangeType.Custom;
                    this.chtInterDayVarlation.Axis.Y.RangeMax = dlg.InterDayMax;
                    this.chtInterDayVarlation.Axis.Y.RangeMin = dlg.InterDayMin;
                    this.chtInterDayVarlation.CreateGraphics();

                    // R管理図
                    //this.chtRControl.Axis.Y.RangeType = AxisRangeType.Custom;
                    this.chtRControl.Axis.Y.RangeMax = dlg.RControlMax;
                    this.chtRControl.Axis.Y.RangeMin = dlg.RControlMin;
                    this.chtRControl.CreateGraphics();

                    // 日内変動
                    //this.chtIntraDayVarlation.Axis.Y.RangeType = AxisRangeType.Custom;
                    this.chtIntraDayVarlation.Axis.Y.RangeMax = dlg.IntraDayMax;
                    this.chtIntraDayVarlation.Axis.Y.RangeMin = dlg.IntraDayMin;
                    this.chtIntraDayVarlation.CreateGraphics();
                }
            }
        }

        /// <summary>
        /// 管理値入力ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 管理値入力します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnReferenceValueInput_Click( object sender, EventArgs e )
        {
            this.inputRefernceValue();
        }

        /// <summary>
        /// 管理値入力
        /// </summary>
        /// <remarks>
        /// 管理値入力します
        /// </remarks>
        private void inputRefernceValue()
        {
            using ( DlgReferenceValueInput dlg = new DlgReferenceValueInput() )
            {
                dlg.ShowDialog();
                this.loadControlQCData();
            }
        }

        /// <summary>
        /// 編集モード設定変更イベント
        /// </summary>
        /// <remarks>
        /// 編集モード設定変更します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void optEditMode_ValueChanged( object sender, EventArgs e )
        {
            this.setEditMode( (Boolean)this.optEditMode.Value );
        }

        /// <summary>
        /// 編集モード設定
        /// </summary> 
        /// <remarks>
        /// 編集モード設定します
        /// </remarks>
        /// <param name="editMode"></param>
        private void setEditMode( Boolean editMode )
        {
            if ( this.grdControlResultDataList.DisplayLayout.Bands[0].Columns.Count == 0 )
            {
                return;
            }

            // 濃度値の編集状態切り替え
            if ( editMode )
            {
                this.grdControlResultDataList.DisplayLayout.Bands[0].Columns[ControlResultDataQC.DataKeys.Concentration].CellActivation = Activation.AllowEdit;
                this.grdControlResultDataList.DisplayLayout.Bands[0].Columns[ControlResultDataQC.DataKeys.Concentration].CellAppearance.BackColor = CarisXConst.GRID_CELL_CAN_EDIT_COLOR;
            }
            else
            {
                this.grdControlResultDataList.DisplayLayout.Bands[0].Columns[ControlResultDataQC.DataKeys.Concentration].CellActivation = Activation.NoEdit;
                this.grdControlResultDataList.DisplayLayout.Bands[0].Columns[ControlResultDataQC.DataKeys.Concentration].CellAppearance.BackColor = Color.Empty;
            }
        }

        /// <summary>
        /// 分析項目選択ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 分析項目選択します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnSelectAnayte_Click( object sender, EventArgs e )
        {
            List<Int32> protocolNoList = new List<int>();
            foreach ( var index in Singleton<ControlResultDB>.Instance.GetData().Select( ( data ) => data.GetMeasureProtocolIndex() ).Distinct() )
            {
                protocolNoList.Add( Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( index ).ProtocolNo );
            }
            using ( DlgProtocolSelect dlg = new DlgProtocolSelect( false, 1, measureProtocolNo: protocolNoList ) )
            {
                if ( DialogResult.OK == dlg.ShowDialog() && dlg.SelectedProtocolIndexs.Count > 0 )
                {
                    this.InitYAxisEditting();
                    // ダイアログより選択された分析項目の取得
                    this.currentMeasureProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( dlg.SelectedProtocolIndexs.First() );
                    // 選択された分析項目をセット
                    this.lblAnalyte.Text = currentMeasureProtocol.ProtocolName;

                    var controlNameDatas = Singleton<ControlResultDB>.Instance.GetData( this.currentMeasureProtocol.ProtocolIndex );

                    // 精度管理検体名(昇順)
                    this.cmbControlName.DataSource = controlNameDatas.Select( ( data ) => data.ControlName ).OrderBy( ( str ) => str ).Distinct().ToList();
                    this.cmbControlName.DataBind();
                    this.cmbControlName.SelectedIndex = 0;

                    this.setLotNo();

                    //// 期間(現在まで1ヶ月間)
                    //this.ResetDateTimeStartEnd();

                    this.reloadDispData();
                    this.loadControlQCData();

                    // グリッドの濃度列の書式設定
                    Tuple<Int32, Int32> maskInput = Oelco.CarisX.Utility.CarisXSubFunction.GetDigitsConcentration( currentMeasureProtocol );
                    if ( maskInput.Item2 == 0 )
                    {
                        this.grdControlResultDataList.DisplayLayout.Bands[0].Columns[ControlResultDataQC.DataKeys.Concentration].MaskInput = "".PadLeft( maskInput.Item1, 'n' );
                    }
                    else
                    {
                        this.grdControlResultDataList.DisplayLayout.Bands[0].Columns[ControlResultDataQC.DataKeys.Concentration].MaskInput = "".PadLeft( maskInput.Item1, 'n' ) + "." + "".PadLeft( maskInput.Item2, 'n' );
                    }

                    // Form共通の編集中フラグOFF
                    FormChildBase.IsEdit = false;
                }
            }
        }

        /// <summary>
        /// 精度管理検体名選択変更コミットイベント
        /// </summary>
        /// <remarks>
        /// 精度管理検体ロット番号設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void cmbControlName_SelectionChangeCommitted( object sender, EventArgs e )
        {
            this.setLotNo();

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// 精度管理検体ロット番号設定
        /// </summary>
        /// <remarks>
        /// 精度管理検体ロット番号設定します
        /// </remarks>
        private void setLotNo()
        {
            if ( this.currentMeasureProtocol != null )
            {
                this.cmbControlLotNo.SelectedIndex = -1;
                this.InitYAxisEditting();
                var datas = Singleton<ControlResultDB>.Instance.GetDataQC( this.currentMeasureProtocol.ProtocolIndex, null, null, this.cmbControlName.Text );

                // 精度管理検体ロット(降順)
                var lotList = datas.Select( ( data ) => data.ControlLotNo ).OrderByDescending( ( str ) => str ).Distinct().ToList();
                lotList.Add( Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_000 );
                this.cmbControlLotNo.DataSource = lotList;
                this.cmbControlLotNo.DataBind();
                this.cmbControlLotNo.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 精度管理検体ロット番号選択変更コミットイベント
        /// </summary>
        /// <remarks>
        /// 精度管理検体ロット番号設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void cmbControlLotNo_SelectionChangeCommitted( object sender, EventArgs e )
        {
            this.InitYAxisEditting();
            if ( this.cmbControlLotNo.SelectedItem == null
                || this.cmbControlLotNo.SelectedItem.DisplayText == Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_000
                || this.cmbControlLotNo.SelectedItem.DisplayText == Oelco.CarisX.Properties.Resources.STRING_COMMON_000 )
            {
                this.btnInterDayEditReferenceValue.Enabled = false;
                this.btnRControlEditReferenceValue.Enabled = false;
                this.btnEditYAxis.Enabled = false;
            }
            else
            {
                this.btnInterDayEditReferenceValue.Enabled = true;
                this.btnRControlEditReferenceValue.Enabled = true;
                this.btnEditYAxis.Enabled = true;
            }

            this.reloadDispData();
            this.loadControlQCData();

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// 精度管理検体データ開始日付選択ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 精度管理検体データ開始日付選択します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnPeriodStart_Click( object sender, EventArgs e )
        {
            this.buttonDateSelect( Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_037, this.btnPeriodStart );
            this.reloadDispData();

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// 精度管理検体データ終了日付選択ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 精度管理検体データ終了日付選択します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnPeriodEnd_Click( object sender, EventArgs e )
        {
            this.buttonDateSelect( Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_037, this.btnPeriodEnd );
            this.reloadDispData();

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// Y軸編集の初期化
        /// </summary>
        /// <remarks>
        /// Y軸編集を初期化します
        /// </remarks>
        private void InitYAxisEditting()
        {
            this.chtInterDayVarlation.Axis.Y.RangeType = AxisRangeType.Automatic;
            this.chtRControl.Axis.Y.RangeType = AxisRangeType.Automatic;
            this.chtIntraDayVarlation.Axis.Y.RangeType = AxisRangeType.Automatic;
        }

        /// <summary>
        /// 表示データ更新
        /// </summary>
        /// <remarks>
        /// 表示データを更新します
        /// </remarks>
        private void reloadDispData()
        {
            // 表示用データの取得
            if ( this.currentMeasureProtocol != null
                && this.btnPeriodStart.Tag is DateTime
                && this.btnPeriodEnd.Tag is DateTime
                && !String.IsNullOrEmpty( this.cmbControlName.Text )
                && !String.IsNullOrEmpty( this.cmbControlLotNo.Text )
            )
            {
                if ( this.cmbControlLotNo.Text != Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_000 )
                {
                    this.currentControlResultData = Singleton<ControlResultDB>.Instance.GetDataQC( this.currentMeasureProtocol.ProtocolIndex, (DateTime)this.btnPeriodStart.Tag, (DateTime)this.btnPeriodEnd.Tag, this.cmbControlName.Text, this.cmbControlLotNo.Text );
                }
                else
                {
                    this.currentControlResultData = Singleton<ControlResultDB>.Instance.GetDataQC( this.currentMeasureProtocol.ProtocolIndex, (DateTime)this.btnPeriodStart.Tag, (DateTime)this.btnPeriodEnd.Tag, this.cmbControlName.Text );
                }
                this.grdControlResultDataList.DataSource = new BindingList<ControlResultDataQC>( this.currentControlResultData );
            }
            else
            {
                this.currentControlResultData = Singleton<ControlResultDB>.Instance.GetDataQC( 0, DateTime.MaxValue, DateTime.MaxValue, String.Empty, String.Empty );
                this.grdControlResultDataList.DataSource = new BindingList<ControlResultDataQC>( this.currentControlResultData );
            }
            // 精度管理検体測定結果の抽出(日付(時間を含まない)毎にグループ化)
            this.currentDaypoint = this.currentControlResultData.GroupBy( ( data ) => data.MeasureDateTime.Date );

            // グラフ表示へ反映
            if ( currentDaypoint.Count() > 0 )
            {
                Func<Func<IEnumerable<Double>, Double>, DataTable> calcGraphData = ( calcPointValue ) =>
                {
                    DataTable dataTable = new DataTable();
                    DataRow row = dataTable.Rows.Add();
                    currentDaypoint.SelectMany( ( grp ) => grp ).GroupBy( ( data ) => data.MeasureDateTime.ToShortDateString() ).ToList().ForEach( point =>
                    {
                        Double value = 0;
                        var validPoint = from data in point
                                         where Double.TryParse( data.Concentration, out value )
                                         select value;
                        if ( validPoint.Count() > 0 )
                        {
                            dataTable.Columns.Add( point.Key, typeof( Double ) );
                            row[point.Key] = calcPointValue( validPoint );
                        }
                    } );
                    return dataTable;
                };

                // 日差変動データプロット(日内平均を日毎にプロット)
                DataTable interDayData = calcGraphData( ( values ) => values.Average() );
                this.chtInterDayVarlation.DataSource = interDayData;
                this.chtInterDayVarlation.Axis.X.RangeType = AxisRangeType.Automatic;
                this.chtInterDayVarlation.Axis.Y.RangeType = AxisRangeType.Custom;
                if ( interDayData.Columns.Count > 0 )
                {
                    this.chtInterDayVarlation.Axis.Y.RangeMax = interDayData.Rows[0].ItemArray.Max( ( data ) => (Double)data );
                    this.chtInterDayVarlation.Axis.Y.RangeMin = interDayData.Rows[0].ItemArray.Min( ( data ) => (Double)data );
                }

                // 平均算出データの抽出
                var validControlResultData = this.currentControlResultData.Where( ( data ) =>
                {
                    Double value;
                    return Double.TryParse( data.Concentration, out value );
                } ).ToList();
                if ( validControlResultData.Count() > 0 )
                {
                    Double average, sd;
                    sd = validControlResultData.Select( ( value ) => Double.Parse( value.Concentration ) ).GetSD( out average );

                    this.lblInterDayMean.Text = SubFunction.ToRoundOffParse( average, this.currentMeasureProtocol.LengthAfterDemPoint );

                    // 標準偏差算出
                    this.lblInterDaySD.Text = SubFunction.ToRoundOffParse( sd, this.currentMeasureProtocol.LengthAfterDemPoint + 1 );
                }
                // R管理図データプロット(日内上下限の差異を日毎にプロット)
                DataTable rControlData = calcGraphData( ( values ) => Math.Abs( values.Max() - values.Min() ) );
                this.chtRControl.DataSource = rControlData;
                this.chtRControl.Axis.X.RangeType = AxisRangeType.Automatic;
                this.chtRControl.Axis.Y.RangeType = AxisRangeType.Custom;
                if ( rControlData.Columns.Count > 0 )
                {
                    this.chtRControl.Axis.Y.RangeMax = rControlData.Rows[0].ItemArray.Max( ( data ) => (Double)data );
                    this.chtRControl.Axis.Y.RangeMin = rControlData.Rows[0].ItemArray.Min( ( data ) => (Double)data );
                }

                // 日内変動データプロット
                this.intraDayDataPlot();
                this.chtIntraDayVarlation.Axis.X.RangeType = AxisRangeType.Automatic;
                this.chtIntraDayVarlation.Axis.Y.RangeType = AxisRangeType.Custom;
            }
            else
            {
                this.lblInterDayMean.Text = CarisX.Properties.Resources.STRING_COMMON_000;
                this.lblInterDaySD.Text = CarisX.Properties.Resources.STRING_COMMON_000;
                this.lblIntraDayMean.Text = CarisX.Properties.Resources.STRING_COMMON_000;
                this.lblIntraDayCVPercent.Text = CarisX.Properties.Resources.STRING_COMMON_000;
                this.lblIntraDaySD.Text = CarisX.Properties.Resources.STRING_COMMON_000;
                this.lblIntraDayR.Text = CarisX.Properties.Resources.STRING_COMMON_000;

                this.chtInterDayVarlation.ResetDataSource();
                this.chtRControl.ResetDataSource();
                this.chtIntraDayVarlation.ResetDataSource();
            }
        }

        /// <summary>
        /// 日内変動データプロット
        /// </summary>
        /// <remarks>
        /// 日内変動データをプロットします
        /// </remarks>
        private void intraDayDataPlot()
        {
            var dayPointsFirst = currentDaypoint.FirstOrDefault( ( dayPoint ) => dayPoint.Key.Date == ( (DateTime)this.btnIntraDayDate.Tag ).Date );
            if ( dayPointsFirst != null )
            {
                DataTable dataTable = new DataTable();
                DataRow row = dataTable.Rows.Add();
                Double value;
                dayPointsFirst.ToList().ForEach( point =>
                {
                    if ( Double.TryParse( point.Concentration, out value ) )
                    {
                        try
                        {
                            dataTable.Columns.Add( point.MeasureDateTime.ToLongTimeString(), typeof( Double ) );
                            row[point.MeasureDateTime.ToLongTimeString()] = value;
                        }
                        catch ( Exception ex )
                        {
                            Singleton<CarisXLogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("failed to set the data : {0}", ex.StackTrace));
                        }
                    }
                } );
                this.chtIntraDayVarlation.DataSource = dataTable;
                this.chtIntraDayVarlation.Axis.X.RangeType = AxisRangeType.Automatic;
                this.chtIntraDayVarlation.Axis.Y.RangeType = AxisRangeType.Custom;
                if ( dataTable.Columns.Count > 0 )
                {
                    this.chtIntraDayVarlation.Axis.Y.RangeMax = dataTable.Rows[0].ItemArray.Max( ( data ) => (Double)data );
                    this.chtIntraDayVarlation.Axis.Y.RangeMin = dataTable.Rows[0].ItemArray.Min( ( data ) => (Double)data );
                }

                var validData = dayPointsFirst.Where( ( data ) => data.Concentration != CarisXConst.COUNT_CONCENTRATION_NOTHING );
                if ( validData.Count() > 0 )
                {
                    Double sd, mean, cv;
                    cv = validData.Select( ( data ) => Double.Parse( data.Concentration ) ).GetCV( out sd, out mean, true );
                    this.lblIntraDayMean.Text = SubFunction.ToRoundOffParse( mean, this.currentMeasureProtocol.LengthAfterDemPoint );
                    this.lblIntraDaySD.Text = SubFunction.ToRoundOffParse( sd, this.currentMeasureProtocol.LengthAfterDemPoint + 1 );
                    this.lblIntraDayCVPercent.Text = SubFunction.ToRoundOffParse( cv, 1 );
                    this.lblIntraDayR.Text = SubFunction.ToRoundOffParse( (Double)( ( (Decimal)validData.Max( ( data ) => Double.Parse( data.Concentration ) ) ) - ( (Decimal)validData.Min( ( data ) => Double.Parse( data.Concentration ) ) ) ), this.currentMeasureProtocol.LengthAfterDemPoint );
                }
                else
                {
                    this.lblIntraDayMean.Text = CarisX.Properties.Resources.STRING_COMMON_000;
                    this.lblIntraDaySD.Text = CarisX.Properties.Resources.STRING_COMMON_000;
                    this.lblIntraDayCVPercent.Text = CarisX.Properties.Resources.STRING_COMMON_000;
                    this.lblIntraDayR.Text = CarisX.Properties.Resources.STRING_COMMON_000;
                }
            }
            else
            {
                this.chtIntraDayVarlation.DataSource = null;
                this.lblIntraDayMean.Text = CarisX.Properties.Resources.STRING_COMMON_000;
                this.lblIntraDaySD.Text = CarisX.Properties.Resources.STRING_COMMON_000;
                this.lblIntraDayCVPercent.Text = CarisX.Properties.Resources.STRING_COMMON_000;
                this.lblIntraDayR.Text = CarisX.Properties.Resources.STRING_COMMON_000;
            }
        }

        /// <summary>
        /// 日内変動データ日付選択ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 日内変動データ日付を選択し、プロットします
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnIntraDayDate_Click( object sender, EventArgs e )
        {
            this.buttonDateSelect( Oelco.CarisX.Properties.Resources.STRING_CONTROLQC_038, this.btnIntraDayDate );

            this.intraDayDataPlot();
        }

        /// <summary>
        /// 日付表示/選択ボタンの日付選択
        /// </summary>
        /// <remarks>
        /// 日付表示/選択ボタンの日付選択します
        /// </remarks>
        /// <param name="caption">日付選択ダイアログタイトル</param>
        /// <param name="button">日付情報保持対象ボタン</param>
        private void buttonDateSelect( String caption, UltraButton button )
        {
            DateTime date;
            DialogResult dlgDateSelectShow;
            if ( button.Tag != null )
            {
                dlgDateSelectShow = DlgDateSelect.Show( caption, out date, (DateTime)button.Tag );
            }
            else
            {
                dlgDateSelectShow = DlgDateSelect.Show( caption, out date );
            }

            if ( dlgDateSelectShow == System.Windows.Forms.DialogResult.OK )
            {
                button.Tag = date;
                button.Text = date.ToShortDateString();
            }
        }

        /// <summary>
        /// フォーム読み込みイベント
        /// </summary>
        /// <remarks>
        /// 画面初期化します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void FormControlQC_Load( object sender, EventArgs e )
        {
            // データ選択部初期設定
            this.lblAnalyte.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;
            this.cmbControlLotNo.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;
            this.cmbControlName.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;

            // 日付既定値設定
            this.ResetDateTimeStartEnd();

            // データ設定
            Singleton<ControlResultDB>.Instance.LoadDB();
            this.reloadDispData();

            // 編集モードを読み込みモードに設定
            this.optEditMode.Value = false;

            this.btnInterDayEditReferenceValue.Enabled = false;
            this.btnRControlEditReferenceValue.Enabled = false;
            this.btnEditYAxis.Enabled = false;
        }

        /// <summary>
        /// FormClosedイベント
        /// </summary>
        /// <remarks>
        /// UI設定保存します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void FormControlQC_FormClosed( object sender, FormClosedEventArgs e )
        {
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.ControlQCSettings.GridColOrder = this.grdControlResultDataList.GetGridColumnOrder();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.ControlQCSettings.GridColWidth = this.grdControlResultDataList.GetGridColmnWidth();
        }

        /// <summary>
        /// 日付の規定値セット
        /// </summary>
        /// <remarks>
        /// 日付の規定値セットします
        /// </remarks>
        private void ResetDateTimeStartEnd()
        {
            this.btnIntraDayDate.Tag = DateTime.Today;
            this.btnIntraDayDate.Text = DateTime.Today.ToShortDateString();
            this.btnPeriodStart.Tag = DateTime.Today.AddMonths( -1 );
            this.btnPeriodStart.Text = DateTime.Today.AddMonths( -1 ).ToShortDateString();
            this.btnPeriodEnd.Tag = DateTime.Today;
            this.btnPeriodEnd.Text = DateTime.Today.ToShortDateString();
        }

        /// <summary>
        /// セルデータエラーイベント
        /// </summary>
        /// <remarks>
        /// エラーイベント発生します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void grdControlResultDataList_CellDataError( object sender, CellDataErrorEventArgs e )
        {
            e.RaiseErrorEvent = false;
        }

        /// <summary>
        /// 印刷パラメータ変更時処理
        /// </summary>
        /// <remarks>
        /// 印刷パラメータ変更します
        /// </remarks>
        /// <param name="value"></param>
        private void onPrintParamChanged( Object value )
        {
            // 印刷ボタン表示設定
            this.tlbCommandBar.Tools[PRINT].SharedProps.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable;
        }

        #endregion

        private void tabControlQC_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            switch (e.Tab.Index)
            {
                case 0:
                    pnlValueSetting.Visible = true;
                    pnlEditModeSetting.Visible = false;
                    break;
                case 1:
                    pnlValueSetting.Visible = false;
                    pnlEditModeSetting.Visible = true;
                    break;
            }
        }

        /// <summary>
        /// 制度管理登録グリッドセル変更イベント
        /// </summary>
        /// <remarks>
        /// Form共通の編集中フラグをONにします。
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdControlResultDataList_CellChange(object sender, CellEventArgs e)
        {
            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }
    }
}
