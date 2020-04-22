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
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Const;
using Oelco.CarisX.Utility;
using Infragistics.Win.UltraWinGrid;
using Oelco.Common.Parameter;
using Oelco.Common.DB;
using Oelco.CarisX.Print;
using Oelco.CarisX.Log;
using Oelco.Common.Log;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 制度管理登録画面クラス
    /// </summary>
    public partial class FormControlRegistration : FormChildBase
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

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 分析項目･試薬ロット選択ボタン
        /// </summary>
        private List<CustomUStateButton> buttonList;

        /// <summary>
        /// 現在の精度管理検体登録情報
        /// </summary>
        private List<ControlRegistData> currentControlRegistInfo = new List<ControlRegistData>();

        /// <summary>
        /// グリッド用バインドリスト
        /// </summary>
        private BindingList<ControlRegistData> grdBindingList;

        /// <summary>
        /// コピー(ペーストまで)
        /// </summary>
        public ControlRegistData copyDateTemp;

        /// <summary>
        /// 精度管理検体登録画面UI設定
        /// </summary>
        private FormControlResistrationSettings formControlRegistrationSettings;

        /// <summary>
        /// ラックIDセット
        /// </summary>
        private Dictionary<int, ControlRackID> dicRackId;

        /// <summary>
        /// 急診モードの分析項目ボタンを非活性にするかの判断フラグ
        /// </summary>
        private bool enabledFlag;

        #endregion

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormControlRegistration()
        {
            InitializeComponent();

            // コマンドバーのイベント追加
            this.tlbCommandBar.Tools[SAVE].ToolClick += ( sender, e ) => this.saveData();
            this.tlbCommandBar.Tools[DELETE].ToolClick += ( sender, e ) => this.deleteData();
            this.tlbCommandBar.Tools[DELETE_ALL].ToolClick += ( sender, e ) => this.deleteAllData();
            this.tlbCommandBar.Tools[PRINT].ToolClick += ( sender, e ) => this.printData();
            this.tlbCommandBar.Tools[COPY].ToolClick += ( sender, e ) => this.copyData();
            this.tlbCommandBar.Tools[PASTE].ToolClick += ( sender, e ) => this.pasteData();

            // 分析項目/試薬ロット選択ボタンリストの作成
            this.buttonList = new List<CustomUStateButton>(){
                this.btnReagent1,
                this.btnReagent2,
                this.btnReagent3,
                this.btnReagent4,
                this.btnReagent5,
                this.btnReagent6,
                this.btnReagent7,
                this.btnReagent8,
                this.btnReagent9,
                this.btnReagent10,
                this.btnReagent11,
                this.btnReagent12,
                this.btnReagent13,
                this.btnReagent14,
                this.btnReagent15,
                this.btnReagent16,
                this.btnReagent17,
                this.btnReagent18,
                this.btnReagent19,
                this.btnReagent20,
                this.btnReagent21,
                this.btnReagent22,
                this.btnReagent23,
                this.btnReagent24,
                this.btnReagent25,
                this.btnReagent26,
                this.btnReagent27,
                this.btnReagent28,
                this.btnReagent29,
                this.btnReagent30,
                this.btnReagent31,
                this.btnReagent32,
                this.btnReagent33,
                this.btnReagent34,
                this.btnReagent35,
                this.btnReagent36,
                this.btnReagent37,
                this.btnReagent38,
                this.btnReagent39,
                this.btnReagent40,
                this.btnReagent41,
                this.btnReagent42,
                this.btnReagent43,
                this.btnReagent44,
                this.btnReagent45,
                this.btnReagent46,
                this.btnReagent47,
                this.btnReagent48,
                this.btnReagent49,
                this.btnReagent50};

            // 分析項目測定テーブル変更前通知登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget( (Int32)NotifyKind.AnalyteRoutineTableChanging, onAnalyteRoutineTableChanging );
            // 分析項目測定テーブル変更後通知登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget( (Int32)NotifyKind.AnalyteRoutineTableChanged, onAnalyteRoutineTableChanged );
            // 印刷機能有無切替通知
            Singleton<NotifyManager>.Instance.AddNotifyTarget( (Int32)NotifyKind.UseOfPrint, this.onPrintParamChanged );
            // ラックID割り当て変更後通知
            Singleton<NotifyManager>.Instance.AddNotifyTarget( (Int32)NotifyKind.RackIdDefinitionChanged, this.onRackIdDefinitionChanged );
            // ユーザレベル変更通知
            Singleton<NotifyManager>.Instance.AddNotifyTarget( (Int32)NotifyKind.UserLevelChanged, this.setUser );

            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ChangeProtocolSetting, this.onChangeProtocolSetting);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.AssayModeUseOfEmergencyMode, this.onAssayModeKindChanged);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ControlPatientIDformatError, this.chengePatientID);

            this.dicRackId = new Dictionary<Int32, ControlRackID>();
            for ( var id = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDCtrl; id <= Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDCtrl; id++ )
            {
                this.dicRackId.Add( id, new ControlRackID()
                {
                    Value = id
                } );
            }
            //设置ToolBar的右键功能不可用
            this.tlbCommandBar.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.tlbCommandBar_BeforeToolbarListDropdown);
        }
        //设置ToolBar的右键功能不可用
        private void tlbCommandBar_BeforeToolbarListDropdown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventArgs e)
        {
            e.Cancel = true;
        }
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 分析項目設定変更時イベント
        /// </summary>
        /// <remarks>
        /// 測定テーブル読込します
        /// </remarks>
        public void ChageMeasureProtocolSetting()
        {
            this.loadRoutineTable();
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
            zoomPanel.Zoom = Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.ControlResistrationSettings.GridZoom;

            // 拡大率変更イベント登録
            this.zoomPanel.SetZoom += grdControlRegistration.SetGridZoom;

            this.tlbCommandBar.Tools[PASTE].SharedProps.Enabled = false;

            this.grdControlRegistration.DisplayLayout.Bands[0].Columns[ControlRegistData.DataKeys.RackID].CellActivation = Activation.NoEdit;
            this.grdControlRegistration.DisplayLayout.Bands[0].Columns[ControlRegistData.DataKeys.RackPosition].CellActivation = Activation.NoEdit;
            this.grdControlRegistration.DisplayLayout.Bands[0].Columns[ControlRegistData.DataKeys.ControlLotNo].CellActivation = Activation.AllowEdit;
            this.grdControlRegistration.DisplayLayout.Bands[0].Columns[ControlRegistData.DataKeys.ControlName].CellActivation = Activation.AllowEdit;
            this.grdControlRegistration.DisplayLayout.Bands[0].Columns[ControlRegistData.DataKeys.RegisterdStatus].CellActivation = Activation.NoEdit;
            this.grdControlRegistration.DisplayLayout.Bands[0].Columns[ControlRegistData.DataKeys.Comment].CellActivation = Activation.AllowEdit;

            this.grdControlRegistration.DisplayLayout.Bands[0].Columns[ControlRegistData.DataKeys.ControlLotNo].MaxLength = 16;
            this.grdControlRegistration.DisplayLayout.Bands[0].Columns[ControlRegistData.DataKeys.ControlName].MaxLength = 16;
            this.grdControlRegistration.DisplayLayout.Bands[0].Columns[ControlRegistData.DataKeys.Comment].MaxLength = 80;
            this.grdControlRegistration.DisplayLayout.Bands[0].Columns[ControlRegistData.DataKeys.ControlLotNo].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            this.grdControlRegistration.DisplayLayout.Bands[0].Columns[ControlRegistData.DataKeys.ControlName].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            this.grdControlRegistration.DisplayLayout.Bands[0].Columns[ControlRegistData.DataKeys.Comment].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;

            this.grdControlRegistration.SetGridRowBackgroundColorRuleFromIndex( CarisXConst.RACK_POS_COUNT, new List<Color>()
            {
                CarisXConst.GRID_ROWS_DEFAULT_COLOR,CarisXConst.GRID_ROWS_COLOR_PATTERN3
            } );

            // スクロール処理設定
            this.gesturePanel.ScrollProxy = this.grdControlRegistration.ScrollProxy;
            // グリッド表示順
            this.grdControlRegistration.SetGridColumnOrder( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.ControlResistrationSettings.GridColOrder );
            // グリッド列幅
            this.grdControlRegistration.SetGridColmnWidth( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.ControlResistrationSettings.GridColWidth );
            // 印刷ボタン表示設定
            this.tlbCommandBar.Tools[PRINT].SharedProps.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable;

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            this.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLREGIST_011;

            // コマンドバーアイテム名設定
            this.tlbCommandBar.Tools[SAVE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_001;
            this.tlbCommandBar.Tools[DELETE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_002;
            this.tlbCommandBar.Tools[DELETE_ALL].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_003;
            this.tlbCommandBar.Tools[PRINT].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_004;
            this.tlbCommandBar.Tools[COPY].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_005;
            this.tlbCommandBar.Tools[PASTE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_006;

            this.lblReagentLotSelection.Text = Oelco.CarisX.Properties.Resources.STRING_CONTROLREGIST_009;
            this.optLotSelection.DisplayMember = "Key";
            this.optLotSelection.ValueMember = "Value";
            this.optLotSelection.DataSource = Enum.GetValues( typeof( ReagentLotSelect ) ).OfType<ReagentLotSelect>().ToDictionary( ( reagentLotSelect ) => reagentLotSelect.ToTypeString() ).ToList();
            this.optLotSelection.CheckedIndex = 0;

            // グリッドカラムヘッダー表示設定
            this.grdControlRegistration.DisplayLayout.Bands[0].Columns[ControlRegistData.DataKeys.RackID].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CONTROLREGIST_003;
            this.grdControlRegistration.DisplayLayout.Bands[0].Columns[ControlRegistData.DataKeys.RackPosition].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CONTROLREGIST_004;
            this.grdControlRegistration.DisplayLayout.Bands[0].Columns[ControlRegistData.DataKeys.ControlLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CONTROLREGIST_005;
            this.grdControlRegistration.DisplayLayout.Bands[0].Columns[ControlRegistData.DataKeys.ControlName].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CONTROLREGIST_006;
            this.grdControlRegistration.DisplayLayout.Bands[0].Columns[ControlRegistData.DataKeys.RegisterdStatus].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CONTROLREGIST_007;
            this.grdControlRegistration.DisplayLayout.Bands[0].Columns[ControlRegistData.DataKeys.Comment].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CONTROLREGIST_008;
        }

        /// <summary>
        /// 登録情報全削除
        /// </summary>
        /// <remarks>
        /// 登録情報全削除します
        /// </remarks>
        /// <param name="value"></param>
        protected void onAnalyteRoutineTableChanging( object value )
        {
            // 登録情報全削除
            //防止在未打开界面时,点击更新项目清单
            if (this.currentControlRegistInfo.Count == 0)
            {
                loadData();
            }
             this.currentControlRegistInfo.DeleteAllDataList();
             Singleton<ControlRegistDB>.Instance.SetData( this.currentControlRegistInfo );
             Singleton<ControlRegistDB>.Instance.CommitData();
            
        }

        /// <summary>
        /// 測定テーブル更新
        /// </summary>
        /// <remarks>
        /// 測定テーブル更新します
        /// </remarks>
        /// <param name="value"></param>
        protected void onAnalyteRoutineTableChanged( object value )
        {
            // 測定テーブル更新
            this.loadRoutineTable();

            this.loadData();
            
            // ペーストボタンを利用可設定
            this.tlbCommandBar.Tools[PASTE].SharedProps.Enabled = false;
            this.copyDateTemp = null;


            // 分析項目ボタンの活性/非活性を変更
            this.protocolIndexToButtonDicEnabled();
        }

        #endregion

        #region [privateメソッド]

        #region _コマンドバー_

        /// <summary>
        /// 保存
        /// </summary>
        /// <remarks>
        /// 操作履歴に登録実行を登録し、精度管理検体登録情報データの登録実行して反映を行います
        /// </remarks>
        private void saveData()
        {
            // 操作履歴登録：登録実行
            Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_009 } );

            if ( ( this.grdControlRegistration.ActiveCell != null ) && ( this.grdControlRegistration.ActiveCell.IsInEditMode ) )
            {
                // 編集中セルからフォーカスを抜ける。
                // 変更中セル内容が確定できない場合、中断する。
                this.grdControlRegistration.ActiveCell.Activated = false;
                if ( this.grdControlRegistration.ActiveCell != null )
                {
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                    "grdControlRegistration ActiveCell != null && IsInEditMode == true");
                    return;
                }
            }

            var dataList = from Data in this.currentControlRegistInfo
                           where Data.GetRegisterdStatus().Count > 0 && !String.IsNullOrWhiteSpace( Data.ControlLotNo ) && !String.IsNullOrWhiteSpace( Data.ControlName )
                           select Data;


            // 必須項目チェック()
            var checkError = from Data in this.currentControlRegistInfo
                             let Register = Data.GetRegisterdStatus().Count == 0
                             let ControlLot = String.IsNullOrWhiteSpace( Data.ControlLotNo )
                             let ControlName = String.IsNullOrWhiteSpace( Data.ControlName )
                             let RequiredInputStatus = new Boolean[] { Register, ControlLot, ControlName }
                             where RequiredInputStatus.Count( ( state ) => state ) > 0 && RequiredInputStatus.Count( ( state ) => state ) < RequiredInputStatus.Count()
                             select new
                             {
                                 Register,
                                 ControlLot,
                                 ControlName
                             };

            var controlLotError = checkError.FirstOrDefault( ( error ) => error.ControlLot );
            var controlNameError = checkError.FirstOrDefault( ( error ) => error.ControlName );
            var registerError = checkError.FirstOrDefault( ( error ) => error.Register );
            if ( controlLotError != null )     // 精度管理検体ロット未入力
            {
                DlgMessage.Show( String.Format( CarisX.Properties.Resources.STRING_DLG_MSG_100, CarisX.Properties.Resources.STRING_CONTROLREGIST_005 ), String.Empty, String.Empty, MessageDialogButtons.Confirm );
            }
            else if ( controlNameError != null )    // 精度管理検体名未入力
            {
                DlgMessage.Show( String.Format( CarisX.Properties.Resources.STRING_DLG_MSG_100, CarisX.Properties.Resources.STRING_CONTROLREGIST_006 ), String.Empty, String.Empty, MessageDialogButtons.Confirm );
            }
            else if ( registerError != null )            // 分析項目、試薬ロット未登録
            {
                //DlgMessage.Show( String.Format( CarisX.Properties.Resources.STRING_DLG_MSG_100, CarisX.Properties.Resources.STRING_CONTROLREGIST_007 ), String.Empty, String.Empty, MessageDialogButtons.Confirm );
            }

            if (dataList.Count() > 0)
            {
                Singleton<ControlRegistDB>.Instance.SetData(dataList.ToList());
                Singleton<ControlRegistDB>.Instance.CommitData();
            }
            else
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                "dataList.Count() == 0");
            }

            this.loadData();

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// 選択中登録情報の削除
        /// </summary>
        /// <remarks>
        /// 操作履歴に削除実行を登録し、精度管理検体登録情報データの削除実行して反映を行います
        /// </remarks>
        private void deleteData()
        {
            // 操作履歴：削除実行
            Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_009 } );
            
            // TODO:編集中かどうか判断して、表示メッセージを変える
            DialogResult dlgResult;
            var availableData = this.grdControlRegistration.Rows.OfType<UltraGridRow>().Select( ( data ) => (ControlRegistData)data.ListObject ).Where( ( data ) => !String.IsNullOrWhiteSpace( data.ControlLotNo ) || !String.IsNullOrWhiteSpace( data.ControlName ) || data.GetRegisterdStatus().Count != 0 ).ToList();
            if ( availableData.Exists( ( data ) => data.IsAddedData() ) )
            {
                // 編集中のデータも消去されます。消去を行いますか？ 
                dlgResult = DlgMessage.Show( CarisX.Properties.Resources.STRING_DLG_MSG_024, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel );
            }
            else
            {
                // 消去します。よろしいですか？ 
                dlgResult = DlgMessage.Show( CarisX.Properties.Resources.STRING_DLG_MSG_019, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel );
            }

            if ( dlgResult != DialogResult.OK )
            {
                // 操作履歴：削除キャンセル
                Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_004 } );
                return;
            }          

            // 選択中のラックポジションのラックID、ラックポジションの精度管理登録情報データを抽出
            var selectRows = this.grdControlRegistration.SearchSelectRow();
            var targets = selectRows.Select( ( row ) => ( (ControlRegistData)row.ListObject ) ).ToList();

            // 削除対象の削除実施
            foreach ( var target in targets )
            {
                target.DeleteData();
            }

            // 変更を反映
            Singleton<ControlRegistDB>.Instance.SetData( targets );
            Singleton<ControlRegistDB>.Instance.CommitData();            

            this.loadData();

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// 全削除
        /// </summary>
        /// <remarks>
        /// 操作履歴に全消去実行を登録し、精度管理検体登録情報データの全削除実行して反映を行います
        /// </remarks>
        private void deleteAllData()
        {
            // 操作履歴登録：全消去実行
            Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_010 } );
            
            if ( DialogResult.OK == DlgMessage.Show( CarisX.Properties.Resources.STRING_DLG_MSG_171, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel ) ) // 削除確認ダイアログ
            {
                this.currentControlRegistInfo.DeleteAllDataList();

                Singleton<ControlRegistDB>.Instance.SetData( this.currentControlRegistInfo );
                Singleton<ControlRegistDB>.Instance.CommitData();
                
                this.loadData();

                // Form共通の編集中フラグOFF
                FormChildBase.IsEdit = false;
            }
            else
            {
                // 操作履歴登録：全消去キャンセル
                Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_011 } );
            }
        }

        /// <summary>
        /// 印刷
        /// </summary>
        /// <remarks>
        /// 操作履歴に印刷実行を登録し、精度管理検体登録情報データの印刷を実行します
        /// </remarks>
        private void printData()
        {
            // 操作履歴登録：印刷実行
            Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_007 } );
            
            TargetRange outputRange = DlgTargetSelectRange.Show();

            // 印刷対象を取得
            List<ControlRegistData> printData = null;
            switch ( outputRange )
            {
            case TargetRange.All:
                Singleton<ControlRegistDB>.Instance.LoadDB();
                printData = Singleton<ControlRegistDB>.Instance.GetData();
                break;
            case TargetRange.Specification:
                Singleton<ControlRegistDB>.Instance.LoadDB();
                var printDataBuff = Singleton<ControlRegistDB>.Instance.GetData();
                printData = new List<ControlRegistData>();
                var selectRows = this.grdControlRegistration.SearchSelectRow();
                var selectRowDatas = selectRows.Select( ( row ) => ( (ControlRegistData)row.ListObject ) ).ToList();

                foreach ( var data in printDataBuff )
                {
                    if ( selectRowDatas.GetMatchData( data ) != null )
                    {
                        printData.Add( data );
                    }
                }

                break;
            case TargetRange.None:
                // 操作履歴登録：印刷キャンセル
                Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_007 } );
                return;
            }

            if ( printData == null || printData.Count == 0 )
            {
                // 印刷するデータがありません。
                DlgMessage.Show( CarisX.Properties.Resources.STRING_DLG_MSG_064, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_003, MessageDialogButtons.Confirm );
                return;
            }

            enabledFlag = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.IsProtocolEnabledChangedInEmergencyMode();

            // 印刷用Listに取得データを格納
            List<ControlRegistrationReportData> rptData = new List<ControlRegistrationReportData>();
            foreach ( var row in printData )
            {               
                foreach ( var detail in row.GetRegisterdStatus() )
                {
                    ControlRegistrationReportData rptDataRow = new ControlRegistrationReportData();

                    // ヘッダ部
                    rptDataRow.RackID = row.RackID.ToString();
                    rptDataRow.RackPosition = row.RackPosition;
                    rptDataRow.ControlLot = row.ControlLotNo;
                    rptDataRow.ControlName = row.ControlName;
                    rptDataRow.Comment = row.Comment;
                    rptDataRow.PrintDateTime = DateTime.Now.ToDispString();

                    var protocal = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(detail.MeasureProtocolIndex);

                    // 全スレーブで急診無しかつ急診使用ありの分析項目は印刷しない
                    if (( enabledFlag == true ) && ( protocal.UseEmergencyMode == true ))
                    {
                        continue;
                    }

                    // 明細部
                    rptDataRow.ProtoName = protocal.ProtocolName;
                    
                    if ( detail.SelectReagentLot == ReagentLotSelect.CurrentLot )
                    {
                        rptDataRow.CartridgeLot = detail.SelectReagentLot.ToTypeString();
                    }
                    else
                    {
                        rptDataRow.CartridgeLot = detail.ReagentLotNo;
                    } 

                    rptData.Add( rptDataRow );
                } 
               
            }

            ControlRegistrationPrint prt = new ControlRegistrationPrint();
            Boolean ret = prt.Print( rptData );
            
        }

        /// <summary>
        /// コピー
        /// </summary>
        /// <remarks>
        /// 操作履歴にコピー実行を登録し、選択中のグリッドのセルのコピーを実行します
        /// </remarks>
        private void copyData()
        {
            // 操作履歴登録
            Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_016 } );
            
            if ( ( this.grdControlRegistration.ActiveCell != null ) && ( this.grdControlRegistration.ActiveCell.IsInEditMode ) )
            {
                // 編集中セルからフォーカスを抜ける。
                // 変更中セル内容が確定できない場合、中断する。
                this.grdControlRegistration.ActiveCell.Activated = false;
                if ( this.grdControlRegistration.ActiveCell != null )
                {
                    return;
                }
            }

            if ( this.grdControlRegistration.ActiveRow != null )
            {

                this.copyDateTemp = this.grdControlRegistration.ActiveRow.ListObject as ControlRegistData;

                // ペーストボタンを利用可設定
                this.tlbCommandBar.Tools[PASTE].SharedProps.Enabled = true;

            }
        }

        /// <summary>
        /// ペースト
        /// </summary>
        /// <remarks>
        /// 操作履歴にペースト実行を登録し、選択中のグリッドのセルのペーストを実行します
        /// </remarks>
        private void pasteData()
        {
            // 操作履歴登録     
            Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_017 } );
           
            Int32 selectedIndex = this.grdControlRegistration.ActiveRow.Index;
            using ( DlgPasteCountInput pasteDlg = new DlgPasteCountInput() )
            {
                pasteDlg.MaxValue = CarisXConst.SAMPLE_REGIST_MAX;

                if ( pasteDlg.ShowDialog() == DialogResult.OK )
                {
                    // ペースト先を抽出(登録済データに対してはペーストしない)
                    var pasteTargets = ( from row in this.grdControlRegistration.Rows
                                         let data = (ControlRegistData)row.ListObject
                                         where row.Index >= selectedIndex && row.Index < ( selectedIndex + pasteDlg.PasteCount )
                                         select new
                                         {
                                             row,
                                             data
                                         } ).Where( ( data ) => data.data.IsAddedData() );

                    UltraGridRow lastRow = null;
                    foreach ( var target in pasteTargets )
                    {
                        target.data.SetRegisterdStatus( this.copyDateTemp.GetRegisterdStatus() );
                        lastRow = target.row;
                    }

                    // ペースト最終行をアクティブ化
                    if ( lastRow != null )
                    {
                        lastRow.Activated = false;
                        lastRow.Activated = true;
                    }

                    // Form共通の編集中フラグON
                    FormChildBase.IsEdit = true;
                }
                else
                {
                    // 操作履歴：キャンセル   
                    Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_048 } );
                }
            }
        }

        #endregion

        /// <summary>
        /// パネル閉じるボタン
        /// </summary>
        /// <remarks>
        /// パネル閉じます
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnClose_Click( object sender, EventArgs e )
        {
            this.pnlSelectReagentLot.Visible = false;
        }

        /// <summary>
        /// フォーム読み込み時のイベント
        /// </summary>
        /// <remarks>
        /// 分析項目選択パネルの分析項目情報読み取りし精度管理検体登録情報取得します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void FormControlRegistration_Load( object sender, EventArgs e )
        {
            try
            {
                // 分析項目選択パネルの分析項目情報読み取り
                this.loadRoutineTable();

                Singleton<ControlRegistDB>.Instance.LoadDB();
                this.loadData();
            }
            catch ( Exception ex )
            {
                Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
                DlgMessage.Show( String.Empty, ex.Message,CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK );
            }           
        }

        /// <summary>
        /// フェードイン表示
        /// </summary>
        /// <remarks>
        /// 画面表示します
        /// </remarks>
        /// <param name="captScreenRect"></param>
        public override void Show( Rectangle captScreenRect )
        {
            this.setSelectReagentLot( null );
            this.loadData();
            base.Show( captScreenRect );
        }

        /// <summary>
        /// 測定テーブル読み込み
        /// </summary>
        /// <remarks>
        /// 分析項目取得します
        /// </remarks>
        private void loadRoutineTable()
        {
            try
            {
                List<MeasureProtocol> measureProtocols = Singleton<MeasureProtocolManager>.Instance.UseMeasureProtocolList;
                for (Int32 i = 0; measureProtocols.Count > i; i++)
                {
                    if (buttonList.Count <= i)
                        break;  //インデックスがボタンリストの数以上の場合は処理を抜ける

                    this.buttonList[i].Text = measureProtocols[i].ProtocolName;
                    this.buttonList[i].Visible = true;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// グリッドセルのアクティブ後イベント
        /// </summary>
        /// <remarks>
        /// 分析項目/試薬ロット選択パネル設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void grdControlRegistration_AfterCellActivate( object sender, EventArgs e )
        {
            this.setSelectReagentLot( this.grdControlRegistration.ActiveCell.Row.ListObject as ControlRegistData );
        }

        /// <summary>
        /// グリッド行のアクティブ後イベント
        /// </summary>
        /// <remarks>
        /// 分析項目/試薬ロット選択パネル設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void grdControlRegistration_AfterRowActivate( object sender, EventArgs e )
        {
            this.setSelectReagentLot( this.grdControlRegistration.ActiveRow.ListObject as ControlRegistData );
        }

        /// <summary>
        /// 分析項目/試薬ロット選択パネル設定
        /// </summary>
        /// <remarks>
        /// 分析項目/試薬ロット選択パネル設定します
        /// </remarks>
        /// <param name="registerdStatus"></param>
        private void setSelectReagentLot( ControlRegistData controlregistData )
        {
            Dictionary<Int32, ControlRegistData.RegistReagentLotInfo> registerdStatus = null;
            if ( controlregistData != null )
            {
                registerdStatus = controlregistData.GetRegisterdStatus().ToDictionary( ( data ) => data.MeasureProtocolIndex );
            }
            this.pnlSelectReagentLot.Visible = true;

            enabledFlag = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.IsProtocolEnabledChangedInEmergencyMode();

            foreach ( CustomUStateButton btn in this.buttonList )
            {
                btn.Visible = false;
                Int32 index = this.buttonList.IndexOf( btn );
                if ( index < Singleton<MeasureProtocolManager>.Instance.UseMeasureProtocolList.Count )
                {
                    btn.Visible = true;
                    MeasureProtocol measureProtocol = Singleton<MeasureProtocolManager>.Instance.UseMeasureProtocolList[index];
                    btn.Text = measureProtocol.ProtocolName;

                    if ( registerdStatus != null && registerdStatus.ContainsKey( measureProtocol.ProtocolIndex ) )
                    {
                        btn.CurrentState = true;
                        var info = registerdStatus[measureProtocol.ProtocolIndex];

                        switch ( info.SelectReagentLot )
                        {
                        case ReagentLotSelect.CurrentLot:           // 現ロット
                            break;
                        case ReagentLotSelect.LotSpecification:     // ロット指定
                            btn.Text += Environment.NewLine + info.ReagentLotNo;
                            break;
                        case ReagentLotSelect.LotAll:               // 全ロット
                            btn.Text += Environment.NewLine + Oelco.CarisX.Properties.Resources.STRING_CONTROLREGIST_002;
                            break;
                        default:
                            break;
                        }

                        // 全スレーブの急診使用無しの場合、急診使用無しの分析項目の選択状態を解除する
                        if (( enabledFlag == true ) && ( measureProtocol.UseEmergencyMode == true ))
                        {
                            btn.CurrentState = false;
                        }

                        continue;
                    }
                    else
                    {
                        btn.CurrentState = false;
                    }
                }
            }

            // 分析項目ボタンの活性/非活性を変更
            protocolIndexToButtonDicEnabled();
        }

        /// <summary>
        /// データの読み込み
        /// </summary>
        /// <remarks>
        /// 精度管理検体登録情報データ取得します
        /// </remarks>
        private void loadData()
        {
            if ( this.currentControlRegistInfo == null )
            {
                this.currentControlRegistInfo = new List<ControlRegistData>();
            }
            if ( grdBindingList == null )
            {
                this.grdBindingList = new BindingList<ControlRegistData>( this.currentControlRegistInfo );
                this.grdControlRegistration.DataSource = this.grdBindingList;
            }

            List<ControlRegistData> controlRegistInfo = Singleton<ControlRegistDB>.Instance.GetData();


            var dic = ( from v in controlRegistInfo
                        group v.RackPosition by v.RackID.Value into grp
                        select grp ).ToDictionary( ( grp ) => grp.Key );

            // ラックデータ
            for ( Int32 rack = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDCtrl;
                rack <= Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDCtrl;
                rack++ )
            {
                bool rackContains = !dic.ContainsKey( rack );

                // ポジションデータ
                for ( Int32 position = 1; position <= CarisXConst.RACK_POS_COUNT; position++ )
                {
                    // ポジションデータが無い場合、追加
                    if ( rackContains || !dic[rack].Contains( position ) )
                    {
                        var addPositionData = Singleton<ControlRegistDB>.Instance.AddData(
                            ref controlRegistInfo, this.dicRackId[rack], position, String.Empty, String.Empty, 0, null );
                    }
                }
            }
            
            this.currentControlRegistInfo.Clear();
            this.currentControlRegistInfo.AddRange( from v in controlRegistInfo.AsParallel()
                                                    orderby v.RackID.Value, v.RackPosition
                                                    select v );

            this.grdBindingList.ResetBindings();
            //this.grdControlRegistration.DisplayLayout.Bands[0].Columns[ControlRegistData.DataKeys.ControlLotNo].InvalidValueBehavior = InvalidValueBehavior.RevertValueAndRetainFocus;
            this.grdControlRegistration.DisplayLayout.Bands[0].Columns[ControlRegistData.DataKeys.ControlLotNo].MaxLength = 16;
        }

        /// <summary>
        /// 分析項目選択ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 分析項目選択します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnReagent_Click( object sender, EventArgs e )
        {
            IEnumerable<ControlRegistData> controlRegistDataList = null;

            controlRegistDataList = this.grdControlRegistration.SearchSelectRow().Select( ( row ) => (ControlRegistData)row.ListObject ).ToList();
            if ( controlRegistDataList.Count() == 0 )
            {
                ((CustomUStateButton)sender).CurrentState = false;
                return;
            }
            
            Int32 index = this.buttonList.IndexOf( (CustomUStateButton)sender );
            if ( index < Singleton<MeasureProtocolManager>.Instance.UseMeasureProtocolList.Count )
            {
                CustomUStateButton button = ( (CustomUStateButton)sender );
                MeasureProtocol measureProtocol = Singleton<MeasureProtocolManager>.Instance.UseMeasureProtocolList[index];
                button.Text = measureProtocol.ProtocolName;
                if ( button.CurrentState == true )
                {
                    // ボタン文字列の切り替え
                    String lotNo = null;
                    if ( this.optLotSelection.CheckedItem != null )
                    {
                        switch ( (ReagentLotSelect)this.optLotSelection.CheckedItem.ListIndex )
                        {
                        case ReagentLotSelect.CurrentLot:           // 現ロット
                            break;
                        case ReagentLotSelect.LotSpecification:     // ロット指定
                            using ( DlgReagentLotSelect dlg = new DlgReagentLotSelect() )
                            {
                                dlg.SetMeasureProtocol( measureProtocol.ProtocolIndex );
                                if ( dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK )
                                {
                                    lotNo = dlg.ReagentLotNo;
                                    if ( lotNo == null )
                                    {
                                        button.CurrentState = false;
                                        return;
                                    }
                                }
                                else
                                {
                                    button.CurrentState = false;
                                    return;
                                }
                            }
                            break;
                        case ReagentLotSelect.LotAll:               // 全ロット
                            lotNo = Oelco.CarisX.Properties.Resources.STRING_CONTROLREGIST_002;
                            break;
                        default:
                            break;
                        }

                        if ( lotNo != null )
                        {
                            button.Text += Environment.NewLine + lotNo;
                        }

                        // 選択状態のボタンの分析項目を登録状態へ
                        foreach(var controlRegistData in controlRegistDataList)
                        {
                            controlRegistData.AddRegisterdStatus( new ControlRegistData.RegistReagentLotInfo()
                            {
                                MeasureProtocolIndex = measureProtocol.ProtocolIndex,
                                SelectReagentLot = (ReagentLotSelect)this.optLotSelection.CheckedItem.ListIndex,
                                ReagentLotNo = lotNo
                            } );
                        }
                    }
                }
                else
                {
                    foreach ( var controlRegistData in controlRegistDataList )
                    {
                        // 非選択状態のボタンの分析項目を非登録状態へ
                        controlRegistData.RemoveRegisterdStatus( measureProtocol.ProtocolIndex );
                    }
                }
            }

            this.grdBindingList.ResetBindings();

            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        /// <summary>
        /// FormClosedイベント
        /// </summary>
        /// <remarks>
        /// UI設定保存します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void FormControlRegistration_FormClosed( object sender, FormClosedEventArgs e )
        {
            // UI設定保存
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.ControlResistrationSettings.GridZoom = this.zoomPanel.Zoom;
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.ControlResistrationSettings.GridColOrder = this.grdControlRegistration.GetGridColumnOrder();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.ControlResistrationSettings.GridColWidth = this.grdControlRegistration.GetGridColmnWidth();
        }
        
        /// <summary>
        /// 印刷パラメータ変更時処理
        /// </summary>
        /// <remarks>
        /// 印刷ボタン表示設定します
        /// </remarks>
        /// <param name="value"></param>
        private void onPrintParamChanged( Object value )
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
        private void onRackIdDefinitionChanged( Object value )
        {
            this.dicRackId = new Dictionary<Int32, ControlRackID>();
            for ( var id = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDCtrl; id <= Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDCtrl; id++ )
            {
                this.dicRackId.Add( id, new ControlRackID()
                {
                    Value = id
                } );
            }


            // 一般(優先)ラックID割り当て変更時
            var changeSampleKind = value as IEnumerable<SampleKind>;
            if ( ( changeSampleKind ?? new SampleKind[] { } ).Contains( SampleKind.Control ) )
            {
                this.currentControlRegistInfo.DeleteAllDataList();

                Singleton<ControlRegistDB>.Instance.SetData( this.currentControlRegistInfo );
                Singleton<ControlRegistDB>.Instance.CommitData();

                this.loadData();
            }
        }

        /// <summary>
        ///  FormProtocolSetting変更時処理
        /// </summary>
        /// <param name="value"></param>
        private void onChangeProtocolSetting( Object value )
        {
            // 分析項目ボタンの活性/非活性を変更
            protocolIndexToButtonDicEnabled();
        }

        /// <summary>
        /// 分析方法(急診)変更時
        /// </summary>
        /// <param name="value"></param>
        private void onAssayModeKindChanged( Object value )
        {
            // 分析項目ボタンに活性/非活性の変更
            this.protocolIndexToButtonDicEnabled();
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


            foreach (CustomUStateButton btn in this.buttonList)
            {
                Int32 index = this.buttonList.IndexOf(btn);
                if (index < Singleton<MeasureProtocolManager>.Instance.UseMeasureProtocolList.Count)
                {
                    if (enabledFlag)
                    {
                        MeasureProtocol protocal = Singleton<MeasureProtocolManager>.Instance.UseMeasureProtocolList [index];

                        if (protocal.UseEmergencyMode)
                        {
                            btn.Enabled = false;
                        }
                        else
                        {
                            btn.Enabled = true;
                        }
                    }
                    else
                    {
                        btn.Enabled = true;
                    }
                }
            }
        }

        #endregion

        #region IUISetting メンバー
        
        /// <summary>
        /// 精度管理検体登録画面UI設定の型取得
        /// </summary>
        /// <remarks>
        /// 精度管理検体登録画面UI設定の型を返します
        /// </remarks>
        /// <returns></returns>
        public Type GetSettingsType()
        {
            return typeof(FormControlResistrationSettings);
        }

        /// <summary>
        /// 精度管理検体登録画面UI設定の取得
        /// </summary>
        /// <remarks>
        /// 精度管理検体登録画面UI設定を返します
        /// </remarks>
        /// <returns></returns>
        public object GetSettings()
        {
            return this.formControlRegistrationSettings;
        }

        /// <summary>
        /// 精度管理検体登録画面UI設定の設定
        /// </summary>
        /// <remarks>
        /// 精度管理検体登録画面UI設定を設定します
        /// </remarks>
        /// <param name="setting"></param>
        public void SetSettings( object setting )
        {
            this.formControlRegistrationSettings = (FormControlResistrationSettings)setting;
        }

        #endregion

        /// <summary>
        /// 検体登録グリッドセル変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdControlRegistration_CellChange( object sender, CellEventArgs e )
        {
            // アクティブセルの取得
            UltraGridCell currentCell = ( (CustomGrid)sender ).ActiveCell;

            // アクティブセルが検体IDの場合
            if (currentCell.Column.Key == ControlRegistData.DataKeys.ControlName)
            {

                String formatCheck = String.Empty;

                // セル内のテキストから使用できない文字を削除する
                formatCheck = CarisXSubFunction.IsValidForPatientID(currentCell.Text);

                // 検体IDに使用できない文字があった場合
                Boolean formatError = formatCheck.Equals(currentCell.Text);

                if (!formatError)
                {
                    // 検体IDフォーマットエラー通知を行う
                    Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.ControlPatientIDformatError, currentCell);
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

        /// <summary>
        /// Lot Selection切り替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optLotSelection_ValueChanged(object sender, EventArgs e)
        {
            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }
    }
}
