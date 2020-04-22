using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.Common.Utility;
using Oelco.CarisX.Const;
using Oelco.CarisX.DB;
using Infragistics.Win.UltraWinGrid;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using Oelco.Common.Comm;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Parameter.ErrorCodeData;
using Oelco.Common.Const;
using Oelco.CarisX.Utility;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// システム履歴画面クラス
    /// </summary>
    public partial class FormSystemLog : FormChildBase
    {
        #region [定数定義]

        /// <summary>
        /// ファイル出力
        /// </summary>
        public const String EXPORT = "Export";

        /// <summary>
        /// 詳細表示
        /// </summary>
        public const String DETAILED = "Details";


        public const string PAGEErrorLog = "ErrorLog";
        public const string PAGEOperationLog = "OperationLog";
        public const string PAGEParameterChangeLog = "ParameterChangeLog";
        public const string PAGEOnlineLog = "OnlineLog";
        public const string PAGEAssayLog = "AssayLog";
       
        private int m_nCurrentRowIndex = 0;

        // 2020-02-27 CarisX IoT Add [START]
        public Dictionary<String, String> ErrorlogGridColumns
        {
            get
            {
                return new Dictionary<String, String>()
                {
                    { ErrorLogData.DataKeys.UserID, Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_001},
                    {ErrorLogData.DataKeys.WriteTime, Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_002},
                    {ErrorLogData.DataKeys.ErrorTitle, Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_003},
                    {ErrorLogData.DataKeys.ErrorLevel, Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_024},
                };
            }
        }

        public Dictionary<String, String> AnalyzelogGridColumns
        {
            get
            {
                return new Dictionary<String, String>()
                {
                    {AnalyzeLogData.DataKeys.UserID, Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_001},
                    {AnalyzeLogData.DataKeys.WriteTime, Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_002},
                    {AnalyzeLogData.DataKeys.Contents1, Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_013},
                    {AnalyzeLogData.DataKeys.Contents2, Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_014},
                    {AnalyzeLogData.DataKeys.Contents3, Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_015},
                    {AnalyzeLogData.DataKeys.Contents4, Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_016},
                    {AnalyzeLogData.DataKeys.Contents5, Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_017},

                };
            }
        }

        public Dictionary<String, String> OperationlogGridColumns
        {
            get
            {
                return new Dictionary<String, String>()
                {
                    {OperationLogData.DataKeys.UserID, Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_001},
                    {OperationLogData.DataKeys.WriteTime, Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_002},
                    {OperationLogData.DataKeys.Contents1, Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_006},

                };
            }
        }

        public Dictionary<String, String> ParameterChangeLogGridColumns
        {
            get
            {
                return new Dictionary<String, String>()
                {
                    {ParameterChangeLogData.DataKeys.UserID, Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_007},
                    {ParameterChangeLogData.DataKeys.WriteTime, Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_008},
                    {ParameterChangeLogData.DataKeys.Contents1, Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_009},
                    {ParameterChangeLogData.DataKeys.Contents2, Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_010},
                    {ParameterChangeLogData.DataKeys.Contents3, Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_011},
                    {ParameterChangeLogData.DataKeys.Contents4, Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_012},
                };
            }
        }
        // 2020-02-27 CarisX IoT Add [END]

        /// <summary>
        /// フィルタリングクリアフラグ
        /// </summary>
        private Boolean filterCleaFlag = true;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormSystemLog()
        {
            InitializeComponent();

            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UserLevelChanged, this.setUser);

            // コマンドバーのイベント追加
            this.tlbCommandBar.Tools[EXPORT].ToolClick += ( sender, e ) => this.exportData();
            this.tlbCommandBar.Tools[DETAILED].ToolClick += ( sender, e ) => this.detailedShow();

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
            System.OperatingSystem os = System.Environment.OSVersion;
            if (os.Platform == System.PlatformID.Win32NT && os.Version.Major == 6 && os.Version.Minor == 2)
            {
                this.grdAssayLog.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.grdErrorLog.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.grdOperationLog.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.grdParameterChangeLog.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));  
            }
            else
            {
                this.grdAssayLog.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.grdErrorLog.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.grdOperationLog.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.grdParameterChangeLog.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }

            this.grdErrorLog.DisplayLayout.AutoFitStyle = AutoFitStyle.None;
            this.grdParameterChangeLog.DisplayLayout.AutoFitStyle = AutoFitStyle.None;
            this.grdOnlineLog.DisplayLayout.AutoFitStyle = AutoFitStyle.None;
            this.grdAssayLog.DisplayLayout.AutoFitStyle = AutoFitStyle.None;

            this.grdErrorLog.DisplayLayout.Bands[0].Override.ColumnAutoSizeMode = ColumnAutoSizeMode.None;
            this.grdParameterChangeLog.DisplayLayout.Bands[0].Override.ColumnAutoSizeMode = ColumnAutoSizeMode.None;
            this.grdOnlineLog.DisplayLayout.Bands[0].Override.ColumnAutoSizeMode = ColumnAutoSizeMode.None;
            this.grdAssayLog.DisplayLayout.Bands[0].Override.ColumnAutoSizeMode = ColumnAutoSizeMode.None;

            // グリッド表示順
            this.grdErrorLog.SetGridColumnOrder( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SystemLogSettings.ErrorGridColOrder );
            this.grdOperationLog.SetGridColumnOrder( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SystemLogSettings.OperationGridColOrder );
            this.grdParameterChangeLog.SetGridColumnOrder( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SystemLogSettings.ParameterChangeGridColOrder );
            this.grdOnlineLog.SetGridColumnOrder( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SystemLogSettings.OnlinerGridColOrder );
            this.grdAssayLog.SetGridColumnOrder( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SystemLogSettings.AssayGridColOrder );

            // グリッド列幅
            this.grdErrorLog.SetGridColmnWidth( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SystemLogSettings.ErrorGridColWidth );
            this.grdOperationLog.SetGridColmnWidth( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SystemLogSettings.OperationGridColWidth );
            this.grdParameterChangeLog.SetGridColmnWidth( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SystemLogSettings.ParameterChangeGridColWidth );
            this.grdOnlineLog.SetGridColmnWidth( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SystemLogSettings.OnlineGridColWidth );
            this.grdAssayLog.SetGridColmnWidth( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SystemLogSettings.AssayGridColWidth );
           
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            this.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_018;

            this.tlbCommandBar.Tools[EXPORT].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMON_017;
            this.tlbCommandBar.Tools[DETAILED].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMON_016;

            this.tbpErrorLog.Tab.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_019;
            this.tbpOperationLog.Tab.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_020;
            this.tbpParameterChangeLog.Tab.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_021;
            this.tbpOnlineLog.Tab.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_022;
            this.tbpAssayLog.Tab.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_023;

            this.grdErrorLog.DisplayLayout.Bands[0].Columns[ErrorLogData.DataKeys.UserID].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_001;
            this.grdErrorLog.DisplayLayout.Bands[0].Columns[ErrorLogData.DataKeys.WriteTime].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_002;
            this.grdErrorLog.DisplayLayout.Bands[0].Columns[ErrorLogData.DataKeys.ErrorTitle].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_003;
            this.grdErrorLog.DisplayLayout.Bands[0].Columns[ErrorLogData.DataKeys.ModuleNo].Hidden = true;
            this.grdErrorLog.DisplayLayout.Bands[0].Columns[ErrorLogData.DataKeys.ModuleName].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_025;
            this.grdErrorLog.DisplayLayout.Bands[0].Columns[ErrorLogData.DataKeys.LogID].Hidden = true;
            this.grdErrorLog.DisplayLayout.Bands[0].Columns [ErrorLogData.DataKeys.Counter].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_028;
            this.grdErrorLog.DisplayLayout.Bands[0].Columns [ErrorLogData.DataKeys.ErrorLevel].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_024;

            this.grdOperationLog.DisplayLayout.Bands[0].Columns[OperationLogData.DataKeys.UserID].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_004;
            this.grdOperationLog.DisplayLayout.Bands[0].Columns[OperationLogData.DataKeys.WriteTime].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_005;
            this.grdOperationLog.DisplayLayout.Bands[0].Columns[OperationLogData.DataKeys.Contents1].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_006;
            this.grdOperationLog.DisplayLayout.Bands[0].Columns[OperationLogData.DataKeys.LogID].Hidden = true;

            this.grdParameterChangeLog.DisplayLayout.Bands[0].Columns[ParameterChangeLogData.DataKeys.UserID].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_007;
            this.grdParameterChangeLog.DisplayLayout.Bands[0].Columns[ParameterChangeLogData.DataKeys.WriteTime].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_008;
            this.grdParameterChangeLog.DisplayLayout.Bands[0].Columns[ParameterChangeLogData.DataKeys.Contents1].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_009;
            this.grdParameterChangeLog.DisplayLayout.Bands[0].Columns[ParameterChangeLogData.DataKeys.Contents2].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_010;
            this.grdParameterChangeLog.DisplayLayout.Bands[0].Columns[ParameterChangeLogData.DataKeys.Contents3].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_011;
            this.grdParameterChangeLog.DisplayLayout.Bands[0].Columns[ParameterChangeLogData.DataKeys.Contents4].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_012;
            this.grdParameterChangeLog.DisplayLayout.Bands[0].Columns[ParameterChangeLogData.DataKeys.LogID].Hidden = true;

            // TODO:オンラインログ列名設定

            this.grdAssayLog.DisplayLayout.Bands[0].Columns[AnalyzeLogData.DataKeys.UserID].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_007;
            this.grdAssayLog.DisplayLayout.Bands[0].Columns[AnalyzeLogData.DataKeys.WriteTime].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_008;
            this.grdAssayLog.DisplayLayout.Bands[0].Columns[AnalyzeLogData.DataKeys.Contents1].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_013;
            this.grdAssayLog.DisplayLayout.Bands[0].Columns[AnalyzeLogData.DataKeys.Contents2].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_014;
            this.grdAssayLog.DisplayLayout.Bands[0].Columns[AnalyzeLogData.DataKeys.Contents3].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_015;
            this.grdAssayLog.DisplayLayout.Bands[0].Columns[AnalyzeLogData.DataKeys.Contents4].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_016;
            this.grdAssayLog.DisplayLayout.Bands[0].Columns[AnalyzeLogData.DataKeys.Contents5].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_017;
            this.grdAssayLog.DisplayLayout.Bands[0].Columns[AnalyzeLogData.DataKeys.Contents6].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_026;
            this.grdAssayLog.DisplayLayout.Bands[0].Columns[AnalyzeLogData.DataKeys.Contents7].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_027;
            this.grdAssayLog.DisplayLayout.Bands[0].Columns[AnalyzeLogData.DataKeys.ModuleNo].Hidden = true;
            this.grdAssayLog.DisplayLayout.Bands[0].Columns[AnalyzeLogData.DataKeys.ModuleName].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_025;

            // エラー履歴フィルタリング表示設定
            this.btnFilter.Text = Properties.Resources.STRING_SPECIMENRESULT_022;

            // TODO:利用実態に従い変更
            this.grdAssayLog.DisplayLayout.Bands[0].Columns[AnalyzeLogData.DataKeys.Contents6].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);    //Hiddenなので!で符号逆転
            this.grdAssayLog.DisplayLayout.Bands[0].Columns[AnalyzeLogData.DataKeys.Contents7].Hidden
                = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);    //Hiddenなので!で符号逆転
            this.grdAssayLog.DisplayLayout.Bands[0].Columns[AnalyzeLogData.DataKeys.Contents8].Hidden = true;
            this.grdAssayLog.DisplayLayout.Bands[0].Columns[AnalyzeLogData.DataKeys.Contents9].Hidden = true;
            this.grdAssayLog.DisplayLayout.Bands[0].Columns[AnalyzeLogData.DataKeys.Contents10].Hidden = true;
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
            if (this.grdAssayLog.DisplayLayout.Bands[0].Columns.Count != 0)
            {
                //起動時はまだ列の定義が存在しない為、分岐させておく
                this.grdAssayLog.DisplayLayout.Bands[0].Columns[AnalyzeLogData.DataKeys.Contents6].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);    //Hiddenなので!で符号逆転
                this.grdAssayLog.DisplayLayout.Bands[0].Columns[AnalyzeLogData.DataKeys.Contents7].Hidden
                    = !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult);    //Hiddenなので!で符号逆転
            }
            this.useSearchLogWriteTimConditione(this.dispSearchLogInfoPanel);
        }

        /// <summary>
        /// フォーム表示
        /// </summary>
        /// <remarks>
        /// 履歴データを読込し、画面を表示します
        /// </remarks>
        public override void Show()
        {
            if (this.tabLog.ActiveTab == null)
            {
                 //默认为错误日志
                this.tabLog.ActiveTab = this.tabLog.Tabs.GetItem(0) as Infragistics.Win.UltraWinTabControl.UltraTab;
            }
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //sw.Start();
            switch (this.tabLog.ActiveTab.Key)
            {
                // タブコントロールのkey項目はプログラム内部利用のみでカルチャの影響を受けない。
                case PAGEErrorLog:
                    // エラー履歴をDBから読み込み(エラーコードデータの読み込み)
                    Singleton<ParameterFilePreserve<ErrorCodeDataManager>>.Instance.Load();

                    if (Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.Maintenance))
                    {
                        Singleton<MasterErrorLogDB>.Instance.LoadDB();
                    }
                    else
                    {
                        Singleton<ErrorLogDB>.Instance.LoadDB();
                    }
                    break;

                case PAGEOperationLog:
                    // 操作履歴をDBから読込み
                    Singleton<OperationLogDB>.Instance.LoadDB();
                    break;

                case PAGEParameterChangeLog:
                    // パラメータ変更履歴をDBから読込み
                    Singleton<ParameterChangeLogDB>.Instance.LoadDB();
                    break;

                case PAGEOnlineLog:
                    break;

                case PAGEAssayLog:
                    // 分析履歴をDBから読込み
                    Singleton<AnalyzeLogDB>.Instance.LoadDB();
                    break;

                default:
                    break;
            }      
            
            var selectRow = this.grdErrorLog.SearchSelectRow().FirstOrDefault();
            if (selectRow != null)
            {
                m_nCurrentRowIndex = selectRow.Index;
            }

            // 各種履歴の読込
            this.loadLogData();

            base.Show();
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// アクティベートイベント
        /// </summary>
        /// <remarks>
        /// 各種履歴読込します
        /// </remarks>
        private void FormSystemLog_Activated(object sender, EventArgs e)
        {

            //Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("FormSystemLog_Activated begin1:{0}", 0));         
        
            //   Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("FormSystemLog_Activated begin1:{0}", 1));
               // this.loadLogData();
          
        }

        /// <summary>
        /// ファイル出力
        /// </summary>
        /// <remarks>
        /// ファイル出力します
        /// </remarks>
        protected void exportData()
        {
            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_007 } );

            String fileName;
            switch ( tabLog.ActiveTab.Index )
            {
            // ErrorLog
            case 0:
                CarisXSubFunction.ShowSaveCSVFileDialog( out fileName, OutputFileKind.CSV, CarisXConst.EXPORT_CSV_ERRORLOG, tabLog.ActiveTab.Text, Singleton<CarisXUISettingManager>.Instance.SystemLogSettings );
                if ( !String.IsNullOrWhiteSpace( fileName ) )
                {
                    Singleton<DataHelper>.Instance.ExportCsv((List<ErrorLogData>)this.grdErrorLog.Rows.OfType<UltraGridRow>().Select((row) => (ErrorLogData)row.ListObject).ToList(), this.grdErrorLog.DisplayLayout.Bands[0].Columns.OfType<UltraGridColumn>(), fileName);
                }
                break;
            // OperationLog
            case 1:
                CarisXSubFunction.ShowSaveCSVFileDialog( out fileName, OutputFileKind.CSV, CarisXConst.EXPORT_CSV_OPERATIONLOG, tabLog.ActiveTab.Text, Singleton<CarisXUISettingManager>.Instance.SystemLogSettings );
                if ( !String.IsNullOrWhiteSpace( fileName ) )
                {
                    Singleton<DataHelper>.Instance.ExportCsv( (List<OperationLogData>)this.grdOperationLog.Rows.OfType<UltraGridRow>().Select( ( row ) => (OperationLogData)row.ListObject ).ToList(), this.grdOperationLog.DisplayLayout.Bands[0].Columns.OfType<UltraGridColumn>(), fileName );
                }
                break;
            // PrameterChangeLog
            case 2:
                CarisXSubFunction.ShowSaveCSVFileDialog( out fileName, OutputFileKind.CSV, CarisXConst.EXPORT_CSV_PARAMETERCHANGELOG, tabLog.ActiveTab.Text, Singleton<CarisXUISettingManager>.Instance.SystemLogSettings );
                if ( !String.IsNullOrWhiteSpace( fileName ) )
                {
                    Singleton<DataHelper>.Instance.ExportCsv( (List<ParameterChangeLogData>)this.grdParameterChangeLog.Rows.OfType<UltraGridRow>().Select( ( row ) => (ParameterChangeLogData)row.ListObject ).ToList(), this.grdParameterChangeLog.DisplayLayout.Bands[0].Columns.OfType<UltraGridColumn>(), fileName );
                }
                break;
            // OnlineLog
            case 3:
                CarisXSubFunction.ShowSaveCSVFileDialog( out fileName, OutputFileKind.CSV, CarisXConst.EXPORT_CSV_ONLINELOG, tabLog.ActiveTab.Text, Singleton<CarisXUISettingManager>.Instance.SystemLogSettings );
                if ( !String.IsNullOrWhiteSpace( fileName ) )
                {
                    Singleton<DataHelper>.Instance.ExportCsv( this.grdOnlineLog.Rows.OfType<UltraGridRow>().Select( ( row ) => new
                    {
                        log = (String)row.Cells[0].Value
                    } ).ToList(), this.grdOnlineLog.DisplayLayout.Bands[0].Columns.OfType<UltraGridColumn>(), fileName );
                }
                break;
            // AssayLog
            case 4:
                CarisXSubFunction.ShowSaveCSVFileDialog( out fileName, OutputFileKind.CSV, CarisXConst.EXPORT_CSV_ASSAYLOG, tabLog.ActiveTab.Text, Singleton<CarisXUISettingManager>.Instance.SystemLogSettings );
                if ( !String.IsNullOrWhiteSpace( fileName ) )
                {
                    Singleton<DataHelper>.Instance.ExportCsv( (List<AnalyzeLogData>)this.grdAssayLog.Rows.OfType<UltraGridRow>().Select( ( row ) => (AnalyzeLogData)row.ListObject ).ToList(), this.grdAssayLog.DisplayLayout.Bands[0].Columns.OfType<UltraGridColumn>(), fileName );
                }
                break;
            default:
                break;
            }
        }

        /// <summary>
        /// 詳細表示
        /// </summary>
        /// <remarks>
        /// 詳細画面表示します
        /// </remarks>
        protected void detailedShow()
        {
            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_030 } );

            // ErrorLogの場合、詳細画面表示
            var selectRow = this.grdErrorLog.SearchSelectRow().FirstOrDefault();
            if ( ( tabLog.ActiveTab.Index == 0 ) && ( selectRow != null ) )
            {
                using ( DlgErrorCodeMessage dlg = new DlgErrorCodeMessage() )
                {
                    ErrorLogData data = (ErrorLogData)selectRow.ListObject;
                    dlg.ShowErrorMessage( data.GetErrorCode().ToString(), data.GetErrorArg().ToString(), data.GetContents1(), data.GetModuleName(), data.ModuleNo);
                }
            }
        }

        /// <summary>
        ///  エラー履歴絞り込みパネル表示
        /// </summary>
        /// <remarks>
        ///  エラー履歴絞り込みパネルを表示します
        /// </remarks>
        protected void filterShow()
        {
            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String [] { this.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_091 });

            this.dispSearchLogInfoPanel = true;
        }

        /// <summary>
        /// 行ダブルクリック
        /// </summary>
        /// <remarks>
        /// エラーログの場合、詳細画面表示します
        /// </remarks>
        private void grdErrorLog_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            // エラーログの場合、詳細画面表示
            using ( DlgErrorCodeMessage dlg = new DlgErrorCodeMessage() )
            {
                ErrorLogData data = (ErrorLogData)e.Row.ListObject;
                dlg.ShowErrorMessage( data.GetErrorCode().ToString(), data.GetErrorArg().ToString(), data.GetContents1(), data.GetModuleName(), data.ModuleNo);
            }
        }

        /// <summary>
        /// 各種履歴読み込み
        /// </summary>
        /// <remarks>
        /// 各種履歴読み込みします
        /// </remarks>
        private void loadLogData()
        {
            // エラー履歴
            if (this.tabLog.ActiveTab.Key == PAGEErrorLog)
            {
                if (this.grdErrorLog.DataSource != null)
                {
                    // サービスマンの場合、マスターエラー履歴データを反映する
                    BindingList<ErrorLogData> list = (BindingList<ErrorLogData>)this.grdErrorLog.DataSource;
                    list.RaiseListChangedEvents = false;
                    list.Clear();

                    // フィルタリングクリアしない場合
                    if (!filterCleaFlag)
                    {
                        // サービスマンの場合、マスターエラー履歴データを反映する
                        if (Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.Maintenance))
                        {
                            Singleton<MasterErrorLogDB>.Instance.GetFilteringErrorLog(this.searchLogInfoPanelErrorLog).ForEach(( data ) => list.Add(data));
                        }
                        else
                        {
                            Singleton<ErrorLogDB>.Instance.GetFilteringErrorLog(this.searchLogInfoPanelErrorLog).ForEach(( data ) => list.Add(data));
                        }
                    }
                    // フィルタリングクリアする場合
                    else
                    {
                        // サービスマンの場合、マスターエラー履歴データを反映する
                        if (Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.Maintenance))
                        {
                            Singleton<MasterErrorLogDB>.Instance.GetFilteringErrorLog().ForEach(( data ) => list.Add(data));
                        }
                        else
                        {
                            Singleton<ErrorLogDB>.Instance.GetFilteringErrorLog().ForEach(( data ) => list.Add(data));
                        }
                    }
                    list.RaiseListChangedEvents = true;

                    this.grdErrorLog.DataBind();
                    //IssuesNo:5 防止账户切换或者日期变更时，m_nCurrentRowIndex超出当前grdErrorLog行数导致索引超出范围的BUG
                    if (m_nCurrentRowIndex != 0 && m_nCurrentRowIndex <= grdErrorLog.Rows.Count)
                    {
                        this.grdErrorLog.Rows[m_nCurrentRowIndex].Selected = true;
                        this.grdErrorLog.Rows[m_nCurrentRowIndex].Activate();
                        this.grdErrorLog.Focus();
                    }
                }
                else
                {
                    // サービスマンの場合、マスターエラー履歴データを反映する
                    if (Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.Maintenance))
                    {
                        this.grdErrorLog.DataSource = new BindingList<ErrorLogData>(Singleton<MasterErrorLogDB>.Instance.GetFilteringErrorLog());
                    }
                    else
                    {
                        this.grdErrorLog.DataSource = new BindingList<ErrorLogData>(Singleton<ErrorLogDB>.Instance.GetFilteringErrorLog());
                    }
                }
            }
            else if (this.tabLog.ActiveTab.Key == PAGEOperationLog)
            {
                // 操作履歴
                if (this.grdOperationLog.DataSource != null)
                {
                    BindingList<OperationLogData> list = (BindingList<OperationLogData>)this.grdOperationLog.DataSource;
                    list.RaiseListChangedEvents = false;
                    list.Clear();
                    Singleton<OperationLogDB>.Instance.GetOperationLog().ForEach((data) => list.Add(data));
                    list.RaiseListChangedEvents = true;
                    this.grdOperationLog.DataBind();
                }
                else
                {
                    this.grdOperationLog.DataSource = new BindingList<OperationLogData>(Singleton<OperationLogDB>.Instance.GetOperationLog());
                }
            }
            else if (this.tabLog.ActiveTab.Key == PAGEParameterChangeLog)
            {
                // パラメータ変更履歴
                if (this.grdParameterChangeLog.DataSource != null)
                {
                    BindingList<ParameterChangeLogData> list = (BindingList<ParameterChangeLogData>)this.grdParameterChangeLog.DataSource;
                    list.RaiseListChangedEvents = false;
                    list.Clear();
                    Singleton<ParameterChangeLogDB>.Instance.GetParameterChangeLog().ForEach((data) => list.Add(data));
                    list.RaiseListChangedEvents = true;
                    this.grdParameterChangeLog.DataBind();
                }
                else
                {
                    this.grdParameterChangeLog.DataSource = new BindingList<ParameterChangeLogData>(Singleton<ParameterChangeLogDB>.Instance.GetParameterChangeLog());
                }
            }
            else if (this.tabLog.ActiveTab.Key == PAGEOnlineLog)
            {
                // オンライン履歴
                this.grdOnlineLog.DataSource = Singleton<CarisXCommManager>.Instance.OnlineLog.Where(log => !String.IsNullOrWhiteSpace(log)).Select((log) => new
                {
                    log
                }).ToList();
                this.grdOnlineLog.DataBind();
                this.grdOnlineLog.DisplayLayout.AutoFitStyle = AutoFitStyle.ExtendLastColumn;
                this.grdOnlineLog.DisplayLayout.Bands[0].Columns[0].MinWidth = this.grdOnlineLog.DisplayLayout.Bands[0].Columns[0].Width;
                this.grdOnlineLog.DisplayLayout.Bands[0].Columns[0].MinWidth =
                    this.grdOnlineLog.DisplayLayout.Bands[0].Columns[0].CalculateAutoResizeWidth(PerformAutoSizeType.AllRowsInBand, false);
                this.grdOnlineLog.DisplayLayout.AutoFitStyle = AutoFitStyle.None;
            }
            else if (this.tabLog.ActiveTab.Key == PAGEAssayLog)
            {
                // 分析履歴
                if (this.grdAssayLog.DataSource != null)
                {
                    BindingList<AnalyzeLogData> list = (BindingList<AnalyzeLogData>)this.grdAssayLog.DataSource;
                    list.RaiseListChangedEvents = false;
                    list.Clear();
                    Singleton<AnalyzeLogDB>.Instance.GetAnalyzeLog().ForEach((data) => list.Add(data));
                    list.RaiseListChangedEvents = true;
                    this.grdAssayLog.DataBind();
                }
                else
                {
                    this.grdAssayLog.DataSource = new BindingList<AnalyzeLogData>(Singleton<AnalyzeLogDB>.Instance.GetAnalyzeLog());
                }
            }         
        }

        #endregion

        /// <summary>
        /// ログのアクティブタブ変更イベント
        /// </summary>
        /// <remarks>
        /// タブ切り替え時、タッチ操作でスクロールする対象を変更します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabLog_ActiveTabChanged( object sender, Infragistics.Win.UltraWinTabControl.ActiveTabChangedEventArgs e )
        {

            // タブ切り替え時、タッチ操作でスクロールする対象を変更する。
            switch ( this.tabLog.ActiveTab.Key )
            {
            // タブコントロールのkey項目はプログラム内部利用のみでカルチャの影響を受けない。
            case PAGEErrorLog:
                this.gesturePanel.ScrollProxy = this.grdErrorLog.ScrollProxy;
                break;
            case PAGEOperationLog:
                this.gesturePanel.ScrollProxy = this.grdOperationLog.ScrollProxy;
                break;
            case PAGEParameterChangeLog:
                this.gesturePanel.ScrollProxy = this.grdParameterChangeLog.ScrollProxy;
                break;
            case PAGEOnlineLog:
                this.gesturePanel.ScrollProxy = this.grdOnlineLog.ScrollProxy;
                break;
            case PAGEAssayLog:
                this.gesturePanel.ScrollProxy = this.grdAssayLog.ScrollProxy;
                break;
            default:
                break;
            }
            this.Show();
        }

        /// <summary>
        /// FormClosedイベント
        /// </summary>
        /// <remarks>
        /// UI設定保存します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormSystemLog_FormClosed( object sender, FormClosedEventArgs e )
        {
            // UI設定保存
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SystemLogSettings.ErrorGridColOrder = this.grdErrorLog.GetGridColumnOrder();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SystemLogSettings.OperationGridColOrder = this.grdOperationLog.GetGridColumnOrder();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SystemLogSettings.ParameterChangeGridColOrder = this.grdParameterChangeLog.GetGridColumnOrder();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SystemLogSettings.OnlinerGridColOrder = this.grdOnlineLog.GetGridColumnOrder();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SystemLogSettings.AssayGridColOrder = this.grdAssayLog.GetGridColumnOrder();

            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SystemLogSettings.ErrorGridColWidth = this.grdErrorLog.GetGridColmnWidth();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SystemLogSettings.OperationGridColWidth = this.grdOperationLog.GetGridColmnWidth();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SystemLogSettings.ParameterChangeGridColWidth = this.grdParameterChangeLog.GetGridColmnWidth();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SystemLogSettings.OnlineGridColWidth = this.grdOnlineLog.GetGridColmnWidth();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SystemLogSettings.AssayGridColWidth = this.grdAssayLog.GetGridColmnWidth();

        }

        /// <summary>
        /// タブ選択変更イベント
        /// </summary>
        /// <remarks>
        /// タブ選択変更します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabLog_SelectedTabChanged( object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e )
        {
            // ErrorLogタブを選択された場合のみ、Detailボタンを活性化する
            if ( this.tabLog.SelectedTab.Equals( tbpErrorLog.Tab ) )
            {
                this.tlbCommandBar.Tools[DETAILED].SharedProps.Enabled = true;
                this.btnFilter.Visible = true;
                this.btnFilter.Enabled = true;
            }
            else
            {
                this.tlbCommandBar.Tools[DETAILED].SharedProps.Enabled = false;
                this.btnFilter.Visible = false;
                this.btnFilter.Enabled = false;
            }
        }


        /// <summary>
        /// エラー履歴絞り込みパネルの表示状態の取得、設定
        /// </summary>
        private Boolean dispSearchLogInfoPanel
        {
            get
            {
                return this.searchLogInfoPanelErrorLog.Visible;
            }
            set
            {
                this.useSearchLogWriteTimConditione(value);
                this.searchLogInfoPanelErrorLog.Visible = value;
            }
        }

        /// <summary>
        /// エラー履歴絞り込みパネルの書き込み時刻の表示切り替え
        /// </summary>
        /// <param name="visble"></param>
        private void useSearchLogWriteTimConditione( Boolean visble )
        {
            // ユーザーレベル1,2,3の時は表示しない
            if (!Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.Maintenance))
            {
                visble = false;
            }

            searchLogInfoPanelErrorLog.chkWriteTime.Visible = visble;
            searchLogInfoPanelErrorLog.btnWriteTimeFrom.Visible = visble;
            searchLogInfoPanelErrorLog.btnWriteTimeTo.Visible = visble;
            searchLogInfoPanelErrorLog.lblHyphen1.Visible = visble;
        }

        /// <summary>
        /// エラー履歴絞り込みパネルOKボタンクリックイベント
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void searchLogInfoPanelErrorLog_OkClick( object sender, EventArgs e )
        {
            // フィルタリングクリアフラグをfalseにする
            filterCleaFlag = false;
            if (this.tabLog.SelectedTab.Equals(tbpErrorLog.Tab))
            {
                if (this.grdErrorLog.DataSource != null)
                {
                    // サービスマンの場合、マスターエラー履歴データを反映する
                    BindingList<ErrorLogData> list = (BindingList<ErrorLogData>)this.grdErrorLog.DataSource;
                    list.RaiseListChangedEvents = false;
                    list.Clear();
                    // サービスマンの場合、マスターエラー履歴データを反映する
                    if (Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.Maintenance))
                    {
                        Singleton<MasterErrorLogDB>.Instance.GetFilteringErrorLog(this.searchLogInfoPanelErrorLog).ForEach(( data ) => list.Add(data));
                    }
                    else
                    {
                        Singleton<ErrorLogDB>.Instance.GetFilteringErrorLog(this.searchLogInfoPanelErrorLog).ForEach(( data ) => list.Add(data));
                    }
                    list.RaiseListChangedEvents = true;

                    this.grdErrorLog.DataBind();

                }
                else
                {
                    // サービスマンの場合、マスターエラー履歴データを反映する
                    if (Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.Maintenance))
                    {
                        this.grdErrorLog.DataSource = new BindingList<ErrorLogData>(Singleton<MasterErrorLogDB>.Instance.GetFilteringErrorLog());
                    }
                    else
                    {
                        this.grdErrorLog.DataSource = new BindingList<ErrorLogData>(Singleton<ErrorLogDB>.Instance.GetFilteringErrorLog());
                    }
                }
            }

            // Filterボタンの画像をフィルタリング時のものにする
            this.btnFilter.Appearance.ImageBackground = CarisX.Properties.Resources.Image_SelectButton_selected;
        }

        /// エラー履歴絞り込みパネルCancelボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// フィルタリング状態を解除する
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void searchLogInfoPanelErrorLog_CanselClick( object sender, EventArgs e )
        {
            // フィルタリングクリアフラグをtrueにする
            filterCleaFlag = true;

            // グリッドの再読み込み
            this.loadLogData();

            // Filterボタンの画像を非フィルタリング時のものにする
            this.btnFilter.Appearance.ImageBackground = CarisX.Properties.Resources.Image_SelectButton;
        }

        /// <summary>
        /// エラー履歴絞り込みパネル閉じるボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// エラー履歴絞り込みパネルが非表示となります
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void searchLogInfoPanelErrorLog_CloseClick( object sender, EventArgs e )
        {
            // エラー履歴絞り込みパネルが非表示
            this.dispSearchLogInfoPanel = false;
        }

        /// <summary>
        /// filterボタン押下時
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnFilter_Click_1( object sender, EventArgs e )
        {
            // エラー履歴絞り込みパネルが表示
            filterShow();
        }

        ///// <summary>
        ///// On-line log読み込み
        ///// </summary>
        ///// <param name="logPath">On-line logファイルパス</param>
        ///// <returns>データテーブル</returns>
        //private DataTable ReadOnlineLog( String logPath )
        //{
        //    this.onlineLog = new DataTable();
        //    this.onlineLog.Clear();
        //    this.onlineLog.Columns.Add( Oelco.CarisX.Properties.Resources.STRING_SYSTEMLOG_000 );

        //    if ( System.IO.File.Exists( logPath ) )
        //    {
        //        System.IO.StreamReader reader = new System.IO.StreamReader( logPath );

        //        int rowCnt = 0;
        //        while ( reader.Peek() >= 0 )
        //        {
        //            this.onlineLog.Rows.Add( this.onlineLog.NewRow() );
        //            this.onlineLog.Rows[rowCnt][0] = reader.ReadLine();
        //            rowCnt++;
        //        }
        //        reader.Close();
        //    }
        //    return this.onlineLog;
        //}


    }
}
