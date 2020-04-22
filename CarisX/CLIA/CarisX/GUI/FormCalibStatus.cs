using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.DB;
using Oelco.Common.Utility;
using Oelco.CarisX.Const;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Oelco.CarisX.Print;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.Common.Parameter;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// キャリブレータステータス画面クラス
    /// </summary>
    public partial class FormCalibStatus : FormChildBase
    {
        #region [定数定義]

        /// <summary>
        /// 印刷
        /// </summary>
        public const String PRINT = "Print";

        #endregion

        #region [インスタンス変数定義]
        /// <summary>
        /// 現在の検量線ステータス情報
        /// </summary>
        private List<CalibStatusData> currentCalibStatusData = new List<CalibStatusData>();
        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormCalibStatus()
        {
            InitializeComponent();

            // コマンドバーのイベント追加
            this.tlbCommandBar.Tools[PRINT].ToolClick += ( sender, e ) => this.printData();
            // 印刷パラメータ変更イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget( (Int32)NotifyKind.UseOfPrint, this.onPrintParamChanged );

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
        /// 画面表示
        /// </summary>
        /// <remarks>
        /// 画面表示します
        /// </remarks>
        /// <param name="captScreenRect"></param>
        public override void Show( Rectangle captScreenRect )
        {
            this.setGrid();
            base.Show( captScreenRect );
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

            this.grdCalibStatus.SetGridRowBackgroundColorRuleFromIndex( 1, new List<Color>() { CarisXConst.GRID_ROWS_DEFAULT_COLOR, CarisXConst.GRID_ROWS_COLOR_PATTERN1 } );
            Singleton<CalibrationCurveDB>.Instance.LoadDB();
            Singleton<ReagentDB>.Instance.LoadDB();
            this.setGrid();

            // スクロール処理設定
            this.gesturePanel.ScrollProxy = this.grdCalibStatus.ScrollProxy;            
            // グリッド表示順
            this.grdCalibStatus.SetGridColumnOrder( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.CalibStatusSettings.GridColOrder );
            // グリッド列幅
            this.grdCalibStatus.SetGridColmnWidth( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.CalibStatusSettings.GridColWidth );

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
            this.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBSTATUS_000;

            this.tlbCommandBar.Tools[PRINT].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_004;
            
            this.grdCalibStatus.DisplayLayout.Bands[0].Columns[CalibStatusData.STRING_PROTOCOLNAME].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CALIBSTATUS_005;
            this.grdCalibStatus.DisplayLayout.Bands[0].Columns[CalibStatusData.STRING_REMAIN].Header.Caption =Oelco.CarisX.Properties.Resources.STRING_CALIBSTATUS_006;
            this.grdCalibStatus.DisplayLayout.Bands[0].Columns[CalibStatusData.STRING_LOTNO].Header.Caption =Oelco.CarisX.Properties.Resources.STRING_CALIBSTATUS_007;
            this.grdCalibStatus.DisplayLayout.Bands[0].Columns[CalibStatusData.STRING_CURVESTATUS].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CALIBSTATUS_008;
        }

        #endregion

        #region [privateメソッド]

        #region _コマンドバー_

        /// <summary>
        /// 印刷
        /// </summary>
        /// <remarks>
        /// 操作履歴に印刷実行を登録し、検量線ステータス情報の印刷を行います
        /// </remarks>
        private void printData()
        {
            // 操作履歴登録：印刷実行
            Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_005 } );

            TargetRange outputRange = DlgTargetSelectRange.Show();

            // 印刷対象を取得
            List<CalibStatusData> printData = null;
            switch ( outputRange )
            {
            case TargetRange.All:
                printData = this.currentCalibStatusData;
                break;
            case TargetRange.Specification:
                printData = this.grdCalibStatus.SearchSelectRow().Select( ( row ) => (CalibStatusData)row.ListObject ).ToList();
                break;
            case TargetRange.None:
                // 操作履歴登録：印刷キャンセル
                Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[]{this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_006});  
                return;
            }

            if ( printData == null || printData.Count == 0 )
            {
                // 印刷するデータがありません。
                DlgMessage.Show( CarisX.Properties.Resources.STRING_DLG_MSG_064, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_003, MessageDialogButtons.Confirm );
                return;
            }

            // 印刷用Listに取得データを格納
            List<CalibStatsuReportData> rptData = new List<CalibStatsuReportData>();
            foreach ( var row in printData )
            {
                CalibStatsuReportData rptDataRow = new CalibStatsuReportData();
                rptDataRow.ProtocolName = row.ProtocolName;
                rptDataRow.Remain = row.Remain.ToString();
                rptDataRow.ReagentLotNo = row.LotNo;
                rptDataRow.CurveStatus = row.CurveStatus;
                rptDataRow.PrintDateTime = DateTime.Now.ToDispString();

                rptData.Add( rptDataRow );
            }

            CalibStatusPrint prt = new CalibStatusPrint();
            Boolean ret = prt.Print( rptData );
           
        }

        #endregion

        /// <summary>
        /// グリッド設定
        /// </summary>
        /// <remarks>
        /// グリッドに検量線情報を設定します
        /// </remarks>
        private void setGrid()
        {
            var reagentList = Singleton<ReagentDB>.Instance.GetData( ReagentKind.Reagent );

            List<CalibStatusData> gridData = new List<CalibStatusData>();
            var grpReagentCode = reagentList.GroupBy( ( data ) => data.ReagentCode );
            foreach ( var measureProtocol in Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList )
            {
                var reagentCodeData = grpReagentCode.FirstOrDefault( ( data ) => data.Key == measureProtocol.ReagentCode );
                if ( reagentCodeData != null )
                {
                    foreach ( var groupLotData in reagentCodeData.GroupBy( ( data ) => data.LotNo ) )
                    {
                        // if ( String.IsNullOrEmpty( groupLotData.Key ) || groupLotData.Count() != 3 )
                        //修正只能放一套瓶子才能显示的BUG
                        if (String.IsNullOrEmpty(groupLotData.Key) || groupLotData.Count()/3 == 0)
                        {
                            continue;
                        }

                        String curveState = CarisX.Properties.Resources.STRING_CALIBSTATUS_003;
                        var curveData = Singleton<CalibrationCurveDB>.Instance.GetDataExcludeMasterCurve( measureProtocol.ProtocolIndex, groupLotData.Key );
                        if ( curveData.Count != 0 )
                        {
                            var curves = curveData.First().Value;
                            if ( curves != null && curves.Count > 0 )
                            {
                                // 期限切れチェック
                                if ( curves[0].GetApprovalDateTime().AddDays( measureProtocol.ValidityOfCurve ) >= DateTime.Now )
                                {
                                    // 成立
                                    curveState = CarisX.Properties.Resources.STRING_CALIBSTATUS_001;
                                }
                                else
                                {
                                    // 期限切れ
                                    curveState = CarisX.Properties.Resources.STRING_CALIBSTATUS_002;
                                }
                            }
                        }                       
                        //var reagentR1R2M = new[] { measureProtocol.R1DispenseVolume, measureProtocol.R2DispenseVolume, measureProtocol.MReagDispenseVolume };
                        var reagentR1R2M = new[] { ((measureProtocol.R1DispenseVolume == 0)? 1:measureProtocol.R1DispenseVolume), measureProtocol.R2DispenseVolume, measureProtocol.MReagDispenseVolume };
                                           
                       // if ( reagentR1R2M.All( ( val ) => val > 0 ) )
                        {
                            //var lotGroupReagent = groupLotData.GroupBy( ( reagentData ) => ( reagentData.PortNo - 1 ) % 3 ).OrderBy( ( grp ) => grp.Key );
                            //var reagentRemainList = lotGroupReagent.Select( ( group ) => group.Sum( ( reagData ) => reagData.Remain.Value ) / reagentR1R2M[group.Key.Value] );
                            //var remain = reagentRemainList.Min();
                            int  remain = 0;
                            var lotGroupReagent = groupLotData.GroupBy( ( reagentData ) => ( reagentData.PortNo - 1 ) % 3 ).OrderBy( ( grp ) => grp.Key );
                            var reagentRemainList = lotGroupReagent.Select( ( group ) => group.Sum( ( reagData ) => reagData.Remain.Value ) / reagentR1R2M[group.Key.Value] );
                            if (measureProtocol.R1DispenseVolume != 0)//R1,R2,M
                            {
                                
                                remain = reagentRemainList.Min();
                            } 
                            else//R2,M
                            {                               
                              //  remain = reagentRemainList.Min();
                                int[] nResult = reagentRemainList.ToArray();

                                if ( nResult.Count() == 2 )
                                {
                                    if (nResult[0] > nResult[1])
                                    {
                                        remain = nResult[1];//M
                                    }
                                    else
                                    {
                                        remain = nResult[0];//R2
                                    }
                                }
                                else if (nResult.Count() == 3)
                                {
                                    if (nResult[1] > nResult[2])
                                    {
                                        remain = nResult[2];//M
                                    }
                                    else
                                    {
                                        remain = nResult[1];//R2
                                    }
                                }
                            }

                            gridData.Add( new CalibStatusData( measureProtocol.ProtocolName, remain, groupLotData.Key, curveState ) );
                        }
                        //else
                        //{
                        //    gridData.Add(new CalibStatusData(measureProtocol.ProtocolName, null, null, curveState));
                        //}

                    }
                }
            }

            if ( this.grdCalibStatus.DataSource != null )
            {
                ( (BindingList<CalibStatusData>)this.grdCalibStatus.DataSource ).RaiseListChangedEvents = false;
                this.currentCalibStatusData.Clear();
                this.currentCalibStatusData.AddRange( gridData );
                ( (BindingList<CalibStatusData>)this.grdCalibStatus.DataSource ).RaiseListChangedEvents = true;
                ( (BindingList<CalibStatusData>)this.grdCalibStatus.DataSource ).ResetBindings();
            }
            else
            {
                this.currentCalibStatusData.AddRange( gridData.ToList() );

                this.grdCalibStatus.DataSource = new BindingList<CalibStatusData>( this.currentCalibStatusData );

                // 検量線が成立ではない場合、文字色を変更(赤)
                var conditionAppearance = new Infragistics.Win.ConditionValueAppearance();
                OperatorCondition condition = new OperatorCondition( ConditionOperator.NotEquals, CarisX.Properties.Resources.STRING_CALIBSTATUS_001, true );
                conditionAppearance.Add( ( condition ), new Infragistics.Win.Appearance()
                {
                    ForeColor = Color.Red
                } );

                this.grdCalibStatus.DisplayLayout.Bands[0].Columns[CalibStatusData.STRING_CURVESTATUS].ValueBasedAppearance = conditionAppearance;
                foreach ( var column in this.grdCalibStatus.DisplayLayout.Bands[0].Columns )
                {
                    column.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
                }
            }
        }
        /// <summary>
        ///  FormClosedイベント
        /// </summary>
        /// <remarks>
        /// UI設定を保存します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void FormCalibStatus_FormClosed( object sender, FormClosedEventArgs e )
        {
            // UI設定保存
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.CalibStatusSettings.GridColOrder = this.grdCalibStatus.GetGridColumnOrder();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.CalibStatusSettings.GridColWidth = this.grdCalibStatus.GetGridColmnWidth();
        }

        /// <summary>
        /// 印刷パラメータ変更時処理
        /// </summary>
        /// <remarks>
        /// 印刷ボタン表示設定を変更します
        /// </remarks>
        /// <param name="value"></param>
        private void onPrintParamChanged( Object value )
        {
            // 印刷ボタン表示設定
            this.tlbCommandBar.Tools[PRINT].SharedProps.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable;
        }
        #endregion
    }

    /// <summary>
    /// 検量線情報表示データ
    /// </summary>
    public class CalibStatusData
    {
        #region [クラス定数]
        /// <summary>
        /// 分析項目名列キー
        /// </summary>
        public static String STRING_PROTOCOLNAME = "ProtocolName";

        /// <summary>
        /// 残量列キー
        /// </summary>
        public static String STRING_REMAIN = "Remain";

        /// <summary>
        /// ロット番号列キー
        /// </summary>
        public static String STRING_LOTNO = "LotNo";

        /// <summary>
        /// 検量線状態列キー
        /// </summary>
        public static String STRING_CURVESTATUS = "CurveStatus";
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="protocolName">分析項目名</param>
        /// <param name="remain">試薬残量</param>
        /// <param name="LotNo">試薬ロット番号</param>
        /// <param name="curveState">検量線状態文字列</param>
        public CalibStatusData( String protocolName, Int32? remain, String LotNo, String curveState )
        {
            this.ProtocolName = protocolName;
            this.Remain = remain;
            this.LotNo = LotNo;
            this.CurveStatus = curveState;
        }

        /// <summary>
        /// 分析項目名の取得
        /// </summary>
        public String ProtocolName
        {
            get;
            private set;
        }

        /// <summary>
        /// 試薬残量の取得
        /// </summary>
        public Int32? Remain
        {
            get;
            private set;
        }

        /// <summary>
        /// 試薬ロット番号の取得
        /// </summary>
        public String LotNo
        {
            get;
            private set;
        }

        /// <summary>
        /// 検量線状態の取得
        /// </summary>
        public String CurveStatus
        {
            get;
            private set;
        }
    }
}
