using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using Oelco.Common.Utility;
using Oelco.CarisX.DB;
using Infragistics.Win.UltraWinListView;
using Oelco.Common.Calculator;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinGrid;
using Oelco.CarisX.Print;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Const;
using Oelco.Common.DB;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Infragistics.UltraChart.Core.Primitives;
using Infragistics.UltraChart.Core;
using Infragistics.UltraChart.Shared.Styles;
using Oelco.CarisX.Calculator;
using System.Drawing.Imaging;
using Oelco.CarisX.Common;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// キャリブレータ解析画面クラス
    /// </summary>
    public partial class FormCalibAnalysis : FormChildBase
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
        /// 現在の分析項目
        /// </summary>
        private MeasureProtocol currentProtocol = null;

        /// <summary>
        /// 検量線補完算出(グラフ出力用)
        /// </summary>
        private ConcentrationCalculator concentrationCalculator = new ConcentrationCalculator();

        /// <summary>
        /// 現在の検量線情報
        /// </summary>
        private Dictionary<String, Dictionary<String, List<CalibrationCurveAnalysisData>>> currentCalibCurvesInfo;

        /// <summary>
        /// 現在のカウント列のActivation
        /// </summary>
        private Activation countColumnActivation;

        /// <summary>
        /// 現在のカウント列のセル背景色
        /// </summary>
        private Color countColumnCellBackColor;

        ///// <summary>
        ///// 既定の列表示
        ///// </summary>
        //private UltraGridBand defultBand;

        /// <summary>
        /// データプロットポイント情報
        /// </summary>
        private IEnumerable<PointData> pointData;

        //校准曲线显示方式
        private Dictionary<Int32, String> calibDisplayStyleKind = new Dictionary<Int32, String>()
        {
            {0,"X-Y"},
            {1,"X-Log(Y)"},
            {2,"X-Logit(Y)"},
        };

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormCalibAnalysis()
        {
            InitializeComponent();

            this.cmbCalibDiplayStyle.Items.Clear();
            this.cmbCalibDiplayStyle.DataSource = this.calibDisplayStyleKind.ToList();
            this.cmbCalibDiplayStyle.DisplayMember = "Value";
            this.cmbCalibDiplayStyle.ValueMember = "Key";
            this.cmbCalibDiplayStyle.SelectedIndex = 0;

            // コマンドバーのイベント追加
            this.tlbCommandBar.Tools[SAVE].ToolClick += ( sender, e ) => this.saveData();
            this.tlbCommandBar.Tools[DELETE].ToolClick += ( sender, e ) => this.deleteData();
            this.tlbCommandBar.Tools[PRINT].ToolClick += ( sender, e ) => this.printData();

            this.chtCalibCurveInterDay.ColorModel.ModelStyle = ColorModels.CustomLinear;
            this.chtCalibCurveInterDay.ColorModel.CustomPalette = CarisXConst.CHART_CALIB_CURVE_COLOR; 

            this.chtCalibCurveInterDay.FillSceneGraph += ( sender, e ) =>
            {
                // 座標軸を取得
                IAdvanceAxis x = (IAdvanceAxis)e.Grid["X"];
                IAdvanceAxis y = (IAdvanceAxis)e.Grid["Y"];

                List<Symbol> symbolList = new List<Symbol>();
                var pointSetList = e.SceneGraph.OfType<Primitive>().Where( ( primitive ) => ( primitive as PointSet ) != null ).ToArray();

					// 検量線元データがある場合
				if ( this.pointData != null && this.pointData.Count() > 0 && pointSetList.Count() > 0 )
				{
					// 表示中検量線元データの抽出
					var dataPointList = this.pointData.Where( ( data ) => pointSetList.Select( ( prim ) => prim.Row ).Contains( data.GroupIndex ) ).Select( ( data ) => data.DataPoint ).SelectMany( ( itemPoint ) => itemPoint );

					setCurrentGraphAxisSetting( dataPointList, x, y );

					foreach ( PointSet pointset in pointSetList )
					{
						var charPoint = this.pointData.First( ( data ) => data.GroupIndex == pointset.Row ).DataPoint;
						var pe = pointset.PE.Clone();
						pe.FillOpacity = Byte.MaxValue;
						DataPoint[] dataPoint = charPoint.Select( ( plotPoint ) => new DataPoint( new Point( (Int32)x.Map( plotPoint.xPos ), (Int32)y.Map( plotPoint.yPos ) ) ) ).ToArray();

						PointSet pset = new PointSet( dataPoint, SymbolIcon.Circle, (SymbolIconSize)7 )
						{
							PE = pe
						};
						e.SceneGraph.Add( pset );
					}
				}
            };

            Singleton<NotifyManager>.Instance.AddNotifyTarget( (Int32)NotifyKind.UseOfPrint, this.onPrintParamChanged );

            // ユーザレベル変更イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget( (Int32)NotifyKind.UserLevelChanged, this.setUser );

            // 分析項目測定テーブル変更後通知登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget( (Int32)NotifyKind.AnalyteRoutineTableChanged, this.onAnalyteRoutineTableChanged );

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
		
		protected void setCurrentGraphAxisSetting(IEnumerable<ItemPoint> pointsData,IAdvanceAxis xAxis, IAdvanceAxis yAxis )
		{
			if ( this.currentProtocol.CalibType.IsLogValue() )
			{
                Double tmpVal;
                Int32 tmpVal2;
                if (this.chtCalibCurveInterDay.Axis.Y.RangeType == AxisRangeType.Automatic)
                {
                    this.chtCalibCurveInterDay.Axis.Y.RangeType = AxisRangeType.Custom;
                    tmpVal = Math.Log10(pointsData.Min((dat) => dat.xPos));
                    tmpVal2 = tmpVal < (double)((int)tmpVal) ? (int)tmpVal - 1 : (int)tmpVal;
                    this.chtCalibCurveInterDay.Axis.X.RangeMin = Math.Pow(10, tmpVal2);
                    if (this.cmbCalibDiplayStyle.SelectedIndex == 2)
                    {
                        this.chtCalibCurveInterDay.Axis.X.RangeMin = Math.Log(Math.Pow(10, tmpVal2)) > 0 ? Math.Log(Math.Pow(10, tmpVal2)) : 1;
                    }

                    tmpVal = Math.Log10(pointsData.Max((dat) => dat.xPos));
                    tmpVal2 = tmpVal > (double)((int)tmpVal) ? (int)tmpVal + 1 : (int)tmpVal;
                    this.chtCalibCurveInterDay.Axis.X.RangeMax = Math.Pow(10, tmpVal2);
                    if (this.cmbCalibDiplayStyle.SelectedIndex == 2)
                    {
                        this.chtCalibCurveInterDay.Axis.X.RangeMax = Math.Log(Math.Pow(10, tmpVal2)) > 0 ? Math.Log(Math.Pow(10, tmpVal2)) : 1;
                    }

                    tmpVal = Math.Log10(pointsData.Min((dat) => dat.yPos));
                    tmpVal2 = tmpVal < (double)((int)tmpVal) ? (int)tmpVal - 1 : (int)tmpVal;

                    if (this.cmbCalibDiplayStyle.SelectedIndex == 0)
                    {
                        this.chtCalibCurveInterDay.Axis.Y.RangeMin = Math.Pow(10, tmpVal2);
                    }
                    if (this.cmbCalibDiplayStyle.SelectedIndex == 1 || this.cmbCalibDiplayStyle.SelectedIndex == 2)
                    {
                        this.chtCalibCurveInterDay.Axis.Y.RangeMin = Math.Log(Math.Pow(10, tmpVal2)) > 0 ? Math.Log(Math.Pow(10, tmpVal2)) : 1;
                    }

                    tmpVal = Math.Log10(pointsData.Max((dat) => dat.yPos));
                    tmpVal2 = tmpVal > (double)((int)tmpVal) ? (int)tmpVal + 1 : (int)tmpVal;

                    if (this.cmbCalibDiplayStyle.SelectedIndex == 0)
                    {
                        this.chtCalibCurveInterDay.Axis.Y.RangeMax = Math.Pow(10, tmpVal2);
                    }

                    if (this.cmbCalibDiplayStyle.SelectedIndex == 1 && this.cmbCalibDiplayStyle.SelectedIndex == 2)
                    {
                        this.chtCalibCurveInterDay.Axis.Y.RangeMax = Math.Log(Math.Pow(10, tmpVal2)) > 0 ? Math.Log(Math.Pow(10, tmpVal2)) : 1;
                    }
                }
			}
			else
			{
                if ((Double)xAxis.Maximum < pointsData.Select((point) => point.yPos).Max() && this.chtCalibCurveInterDay.Axis.Y.RangeType == AxisRangeType.Automatic)
                {
                    this.chtCalibCurveInterDay.SuspendLayout();
                    if (this.cmbCalibDiplayStyle.SelectedIndex == 0)
                    {
                        Double rangeMax = pointsData.Select((point) => point.yPos).Max();
                        Double rangeMin = (Double)xAxis.Minimum;
                        this.chtCalibCurveInterDay.Axis.Y.RangeMax = rangeMax;
                        this.chtCalibCurveInterDay.Axis.Y.RangeMin = rangeMin;

                    }

                    if (this.cmbCalibDiplayStyle.SelectedIndex == 1 || this.cmbCalibDiplayStyle.SelectedIndex == 2)
                    {
                        Double max = pointsData.Select((point) => point.yPos).Max();
                        Double min = (Double)xAxis.Minimum;
                        if (max > 0 && min > 0)
                        {
                            this.chtCalibCurveInterDay.Axis.Y.RangeMax = Math.Log(max);
                            this.chtCalibCurveInterDay.Axis.Y.RangeMin = Math.Log(min);
                        }

                    }
                    this.chtCalibCurveInterDay.Axis.Y.RangeType = AxisRangeType.Custom;
                }

                if ((Double)xAxis.Minimum > pointsData.Select((point) => point.yPos).Min() && this.chtCalibCurveInterDay.Axis.Y.RangeType == AxisRangeType.Automatic)
                {
                    this.chtCalibCurveInterDay.SuspendLayout();
                    if (this.cmbCalibDiplayStyle.SelectedIndex == 0)
                    {
                        this.chtCalibCurveInterDay.Axis.Y.RangeMin = pointsData.Select((point) => point.yPos).Min();
                        this.chtCalibCurveInterDay.Axis.Y.RangeMax = (Double)xAxis.Maximum;
                    }

                    if (this.cmbCalibDiplayStyle.SelectedIndex == 1 && this.cmbCalibDiplayStyle.SelectedIndex == 2)
                    {
                        Double max = (Double)xAxis.Maximum;
                        Double min = pointsData.Select((point) => point.yPos).Min();
                        if (max > 0 && min > 0)
                        {
                            this.chtCalibCurveInterDay.Axis.Y.RangeMin = Math.Log(min);
                            this.chtCalibCurveInterDay.Axis.Y.RangeMax = Math.Log(max);
                        }

                    }
                    this.chtCalibCurveInterDay.Axis.Y.RangeType = AxisRangeType.Custom;
                }
			}
		}
		protected void updateGraphAxisSetting()
		{
			if ( this.currentProtocol.CalibType.IsLogValue() )
			{
				// 対数グラフ表示設定			
				this.chtCalibCurveInterDay.Axis.X.NumericAxisType = NumericAxisType.Logarithmic;
				this.chtCalibCurveInterDay.Axis.Y.NumericAxisType = NumericAxisType.Logarithmic;

				this.chtCalibCurveInterDay.Axis.X.RangeType = AxisRangeType.Custom;
				this.chtCalibCurveInterDay.Axis.Y.RangeType = AxisRangeType.Automatic;
				this.chtCalibCurveInterDay.Axis.Y.TickmarkStyle = AxisTickStyle.DataInterval;
				this.chtCalibCurveInterDay.Axis.X.TickmarkStyle = AxisTickStyle.DataInterval;


				this.chtCalibCurveInterDay.Axis.X.TickmarkInterval = 1;
				this.chtCalibCurveInterDay.Axis.Y.TickmarkInterval = 1;

				this.chtCalibCurveInterDay.Axis.X.MinorGridLines.Visible = true;
				this.chtCalibCurveInterDay.Axis.Y.MinorGridLines.Visible = true;

				// 設定されていない場合、データをバインドされたときに仮設定(描画時に再度正しい値が設定される）
				this.chtCalibCurveInterDay.Axis.X.RangeMin = 1;
				this.chtCalibCurveInterDay.Axis.X.RangeMax = 100000;
				this.chtCalibCurveInterDay.Axis.Y.RangeMin = 1;
				this.chtCalibCurveInterDay.Axis.Y.RangeMax = 100000;

			}
			else
			{
                this.chtCalibCurveInterDay.Axis.X.MinorGridLines.Visible = false;
                this.chtCalibCurveInterDay.Axis.Y.MinorGridLines.Visible = false;
                this.chtCalibCurveInterDay.Axis.X.NumericAxisType = NumericAxisType.Linear;
                this.chtCalibCurveInterDay.Axis.Y.NumericAxisType = NumericAxisType.Linear;
                this.chtCalibCurveInterDay.Axis.X.RangeType = AxisRangeType.Automatic;
                this.chtCalibCurveInterDay.Axis.Y.RangeType = AxisRangeType.Automatic;
                this.chtCalibCurveInterDay.Axis.Y.TickmarkStyle = AxisTickStyle.Smart;
                this.chtCalibCurveInterDay.Axis.X.TickmarkStyle = AxisTickStyle.Smart;

                Double rangeMaxX = -1;
                if (this.pointData.Count() > 0)
                {
                    rangeMaxX = this.pointData.Max((data) =>
                    {
                        if (data.ChartPoint != null && data.ChartPoint.Count() > 0)
                        {
                            return data.ChartPoint.Max((point) => point.xPos);
                        }
                        else
                        {
                            return -1;
                        }
                    });
                }
                if (rangeMaxX > 0)
                {

                    this.chtCalibCurveInterDay.Axis.X.RangeMin = 0;
                    this.chtCalibCurveInterDay.Axis.X.RangeMax = rangeMaxX;
                    this.chtCalibCurveInterDay.Axis.X.TickmarkInterval = rangeMaxX / 10;

                    if (this.cmbCalibDiplayStyle.SelectedIndex == 0)
                    {
                        this.chtCalibCurveInterDay.Axis.Y.RangeMin = 0;
                    }

                    if (this.cmbCalibDiplayStyle.SelectedIndex == 2)
                    {
                        this.chtCalibCurveInterDay.Axis.X.RangeMax = Math.Log(rangeMaxX);
                        this.chtCalibCurveInterDay.Axis.X.TickmarkInterval = Math.Log(rangeMaxX) / 10;
                    }
                    if (this.cmbCalibDiplayStyle.SelectedIndex == 1 || this.cmbCalibDiplayStyle.SelectedIndex == 2)
                    {
                        Double rangeMaxY = -1;
                        if (this.pointData.Count() > 0)
                        {
                            rangeMaxY = this.pointData.Max((data) =>
                            {
                                if (data.ChartPoint != null && data.ChartPoint.Count() > 0)
                                {
                                    return data.ChartPoint.Max((point) => point.yPos);
                                }
                                else
                                {
                                    return -1;
                                }
                            });
                        }
                        if (rangeMaxY > 0)
                        {
                            this.chtCalibCurveInterDay.Axis.Y.RangeMin = 0;
                            this.chtCalibCurveInterDay.Axis.Y.RangeMax = Math.Log(rangeMaxY);
                            this.chtCalibCurveInterDay.Axis.Y.TickmarkInterval = Math.Log(rangeMaxY) / 10;
                        }

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
            this.gesturePanel.ScrollProxy = this.grdCalibCurve.ScrollProxy;                          

            // グリッド表示順
            this.grdCalibCurve.SetGridColumnOrder( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.CalibAnalysisSettings.GridColOrder );
            // グリッド列幅
            this.grdCalibCurve.SetGridColmnWidth( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.CalibAnalysisSettings.GridColWidth );

            // 印刷ボタン表示設定
            this.tlbCommandBar.Tools[PRINT].SharedProps.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable;

            // 保存ボタン
            this.tlbCommandBar.Tools[SAVE].SharedProps.Enabled = false;
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            this.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_019;

            // コマンドバーアイテム名設定
            this.tlbCommandBar.Tools[SAVE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_001;
            this.tlbCommandBar.Tools[DELETE].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_002;
            this.tlbCommandBar.Tools[PRINT].SharedProps.Caption = Oelco.CarisX.Properties.Resources.STRING_COMMANDBARITEM_004;

            // 設定パネルアイテム名設定
            this.lblTitleSetup.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_000;
            this.lblAnalytes.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_001;
            this.btnAnalytesSelect.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_002;
            this.lblReagentLotNoSelection.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_003;
            this.lblTitleCalibrationCurve.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_004;
            this.lblEditMode.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_005;
            this.optEditMode.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_006;
            this.optEditMode.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_007;

            this.lblModuleNoSelection.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_020;

            // 接続台数取得
            Int32 numOfConnectMax = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;

            // モジュール番号リスト設定
            Dictionary<String, Int32> tempModuleList = new Dictionary<String, Int32>()
            {
                { Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_021, CarisXSubFunction.ModuleIndexToModuleId( ModuleIndex.Module1 ) },
                { Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_022, CarisXSubFunction.ModuleIndexToModuleId( ModuleIndex.Module2 ) },
                { Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_023, CarisXSubFunction.ModuleIndexToModuleId( ModuleIndex.Module3 ) },
                { Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_024, CarisXSubFunction.ModuleIndexToModuleId( ModuleIndex.Module4 ) }
            };

            // モジュール番号選択へデータを設定
            this.cmbModuleNoSelection.Text = string.Empty;
            this.cmbModuleNoSelection.DataSource = tempModuleList.Take(numOfConnectMax).ToList();
            this.cmbModuleNoSelection.DisplayMember = "Key";
            this.cmbModuleNoSelection.ValueMember = "Value";
            this.cmbModuleNoSelection.SelectedIndex = 0;

            // グラフ表示文字列設定
            this.chtCalibCurveInterDay.TitleTop.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_008;
            this.chtCalibCurveInterDay.TitleLeft.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_009;
            this.chtCalibCurveInterDay.TitleBottom.Text = Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_010;

            // グリッドカラムヘッダー表示設定
            this.grdCalibCurve.DisplayLayout.Bands[0].Columns[CalibrationCurveData.DataKeys.Analytes].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_010;
            this.grdCalibCurve.DisplayLayout.Bands[0].Columns[CalibrationCurveData.DataKeys.RackID].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_011;
            this.grdCalibCurve.DisplayLayout.Bands[0].Columns[CalibrationCurveData.DataKeys.RackPosition].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_012;
            this.grdCalibCurve.DisplayLayout.Bands[0].Columns[CalibrationCurveData.DataKeys.MultiMeasNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_013;
            this.grdCalibCurve.DisplayLayout.Bands[0].Columns[CalibrationCurveData.DataKeys.Concentration].Header.Caption = String.Format( Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_014, Oelco.CarisX.Properties.Resources.STRING_COMMON_000 );   // 分析項目選択時に設定(単位が必要な為)
            this.grdCalibCurve.DisplayLayout.Bands[0].Columns[CalibrationCurveData.DataKeys.Count].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_015;
            this.grdCalibCurve.DisplayLayout.Bands[0].Columns[CalibrationCurveData.DataKeys.CountAverage].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_016;
            this.grdCalibCurve.DisplayLayout.Bands[0].Columns[CalibrationCurveData.DataKeys.MeasPoint].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_017;
            this.grdCalibCurve.DisplayLayout.Bands[0].Columns[CalibrationCurveData.DataKeys.ModuleNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_025;
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

            Boolean enable = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction( CarisXUserLevelManagedAction.CalibratorEditRecalc );
            gbxEditMode.Visible = enable;
            optEditMode.Enabled = enable;
            this.tlbCommandBar.Tools[DELETE].SharedProps.Enabled = enable;
            this.tlbCommandBar.Tools[SAVE].SharedProps.Enabled = enable;

            // EditModeが非表示の場合はReadを選択状態にする
            if ( !Singleton<CarisXUserLevelManager>.Instance.AskEnableAction( CarisXUserLevelManagedAction.CalibratorEditRecalc ) )
            {
                this.optEditMode.CheckedIndex = 0;
            }
            this.Refresh();

        }

        #endregion

        #region [privateメソッド]

        #region _コマンドバー_

        /// <summary>
        /// 保存
        /// </summary>
        /// <remarks>
        /// 操作履歴に登録実行を登録し、検量線の抽出して反映を行います
        /// </remarks>
        private void saveData()
        {
            // 操作履歴登録：登録実行   
            Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist
                                                      , Singleton<CarisXUserLevelManager>.Instance.NowUserID
                                                      , CarisXLogInfoBaseExtention.Empty
                                                      , new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_009 } );

            if ( this.grdCalibCurve.ActiveCell != null )
            {
                this.grdCalibCurve.ActiveCell.Activated = false;
            }

            // 編集を含む検量線の抽出
            var editCurveInfoList = this.currentCalibCurvesInfo[this.cmbReagentLotNo.SelectedItem.DisplayText].Where( ( editData ) => !editData.Value.All( ( data ) => !data.IsModifyData() ) );

            Boolean flgJudgeCurveEntry = false;

            foreach ( var editCurveInfo in editCurveInfoList )
            {
                Func<ItemPoint[] > getCurvePoint = null;

                int CalibrationWarningLevel = 0;
                //定量的
                if (!this.currentProtocol.CalibType.IsQualitative())
                {
                    
                     var measPoints = from data in
                                         from v in editCurveInfo.Value
                                         where !String.IsNullOrEmpty(v.MeasPoint)
                                         orderby v.MultiMeasNo
                                         group v by v.GetUniqueNo() into grp
                                         select grp.Last()
                                     select new ItemPoint(data.CountAverage ?? data.Count.Value, Double.Parse(data.Concentration));
                     getCurvePoint = CarisXCalculator.GetCreateCurvePoint(this.currentProtocol, measPoints.ToArray(), this.cmbReagentLotNo.SelectedItem.DisplayText,ref CalibrationWarningLevel);
                }
                else//定性的
                {

                    Func< CalibrationCurveAnalysisData,double> getConcentration = (data) =>
                    {
                        if (data.Concentration == CarisXConst.POSITIVE_POSITION)
                        {
                            return this.currentProtocol.PosiLine;
                        } 
                        else
                        {
                            return this.currentProtocol.NegaLine;
                        }
                     };

                    var measPoints = from data in
                                     from v in editCurveInfo.Value
                                     where !String.IsNullOrEmpty(v.MeasPoint)
                                     orderby v.MultiMeasNo
                                     group v by v.GetUniqueNo() into grp
                                     select grp.Last()
                                 select new ItemPoint(data.CountAverage ?? data.Count.Value, getConcentration(data)
                                    );
                    getCurvePoint = CarisXCalculator.GetCreateCurvePoint(this.currentProtocol, measPoints.ToArray(), this.cmbReagentLotNo.SelectedItem.DisplayText, ref CalibrationWarningLevel);
                }
                if ( getCurvePoint != null )
                {
                    var curvePoint = getCurvePoint();

                    flgJudgeCurveEntry = CarisXCalculator.JudgeCurveEntry(curvePoint, this.currentProtocol);
                    if (!flgJudgeCurveEntry)
                    {
                        break;
                    }

                    // 4Parameter係数算出
                    FourParameterMethod method = CarisXCalculator.GetCalcMethod(this.currentProtocol, curvePoint,false, new Double[FourParameterMethod.PARAMETER_COUNT]) as FourParameterMethod;
          
                    if ( method != null )
                    {
                        method.Calc4Parameter();
                    }

                    // 検量線の(補正、係数変更)を含む編集
                  
                    if (!this.currentProtocol.CalibType.IsQualitative())
                    {
                        ItemPoint point = null;
                        foreach (var item in editCurveInfo.Value)
                        {
                            // 濃度値に該当する検量線のポイント情報を取得
                            point = curvePoint.SingleOrDefault((data) => data.xPos == Double.Parse(item.Concentration));

                            // ポイント情報が取得できた場合のみ編集可
                            if (point != null)
                            {
                                // カウント値が異なる場合のみ
                                if (item.Count != (Int32)point.yPos && String.IsNullOrEmpty(item.MeasPoint))
                                {
                                    item.Count = (Int32)point.yPos;
                                }

                                // 4Parameter係数が異なる場合のみ
                                if (method != null)
                                {
                                    if (item.GetExtendValue1() != method.CoefA.ToString())
                                    {
                                        item.SetExtendValue1(method.CoefA.ToString());
                                    }
                                    if (item.GetExtendValue2() != method.CoefB.ToString())
                                    {
                                        item.SetExtendValue2(method.CoefB.ToString());
                                    }
                                    if (item.GetExtendValue3() != method.CoefC.ToString())
                                    {
                                        item.SetExtendValue3(method.CoefC.ToString());
                                    }
                                    if (item.GetExtendValue4() != method.CoefD.ToString())
                                    {
                                        item.SetExtendValue4(method.CoefD.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (!flgJudgeCurveEntry)
            {
                // 単調増加or単調減少判定に失敗()
                Singleton<CarisXLogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "<$*$ Calculator $*$> JudgeCurveEntry:false");
                DlgMessage.Show(Properties.Resources.STRING_ERROR_MESSAGE_000, String.Empty, Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);

                return;
            }

            // 変更を反映
            Singleton<CalibrationCurveDB>.Instance.SetCalibData( this.currentCalibCurvesInfo[this.cmbReagentLotNo.SelectedItem.DisplayText].SelectMany( ( pair ) => pair.Value.Cast<CalibrationCurveData>() ).ToList() );
            Singleton<CalibrationCurveDB>.Instance.CommitData();

            this.currentCalibCurvesInfo = Singleton<CalibrationCurveDB>.Instance.GetAnalysisData( this.currentProtocol.ProtocolIndex
                                                                                                , (int)this.cmbModuleNoSelection.Value );
            
            this.grdCalibCurve.DataSource = new BindingList<CalibrationCurveAnalysisData>( this.currentCalibCurvesInfo[this.cmbReagentLotNo.SelectedItem.DisplayText][this.tabCalibrationCurve.ActiveTab.Key] );
            foreach ( var calibCurve in this.currentCalibCurvesInfo[this.cmbReagentLotNo.SelectedItem.DisplayText] )
            {
                String asterisk = String.Empty;
                foreach ( CalibrationCurveData data in calibCurve.Value )
                {
                    if ( data.IsUserEdited() )
                    {
                        asterisk = Oelco.CarisX.Properties.Resources.STRING_COMMON_005;
                        break;
                    }
                }

                this.lvwCalibrationCurve.Items[calibCurve.Key].Value = calibCurve.Key + asterisk;
                if ( this.tabCalibrationCurve.Tabs.OfType<UltraTab>().Select( ( tab ) => tab.Key ).Contains( calibCurve.Key ) )
                {
                    this.tabCalibrationCurve.Tabs[calibCurve.Key].Text = calibCurve.Key + asterisk;
                }
            }
            this.setGraphData();

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// 削除
        /// </summary>
        /// <remarks>
        /// 操作履歴に削除実行を登録し、検出量情報の削除実行して反映を行います
        /// </remarks>
        private void deleteData()
        {
            if ( this.lvwCalibrationCurve.CheckedItems.Count > 0 )
            {
                // 操作履歴登録：削除実行
                Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_003 } );

                // マスターカーブを含めた削除を中断
                if ( this.lvwCalibrationCurve.CheckedItems.Count( ( item ) => item.Key == Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_018 ) > 0 )
                {
                    DlgMessage.Show( CarisX.Properties.Resources.STRING_DLG_MSG_059, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK );
                    return;
                }

                // UNDONE:削除します。よろしいですか？
                if ( DialogResult.OK != DlgMessage.Show( CarisX.Properties.Resources.STRING_DLG_MSG_038, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel ) )
                {
                    // 操作履歴登録：削除キャンセル   
                    Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_004 } );
                    return;
                }

                // 削除
                this.lvwCalibrationCurve.CheckedItems.ToList().ForEach( ( item ) =>
                {
                    if ( this.currentCalibCurvesInfo[this.cmbReagentLotNo.SelectedItem.DisplayText][item.Key].All( ( data ) => data.GetApprovalDateTime() != CarisXConst.MASTER_CURVE_DATE ) )
                    {
                        foreach ( var data in this.currentCalibCurvesInfo[this.cmbReagentLotNo.SelectedItem.DisplayText][item.Key] )
                        {
                            data.DeleteData();
                        }
                    }
                } );

                // 変更を反映
                Singleton<CalibrationCurveDB>.Instance.SetCalibData( this.currentCalibCurvesInfo[this.cmbReagentLotNo.SelectedItem.DisplayText].SelectMany( ( pair ) => pair.Value.Cast<CalibrationCurveData>() ).ToList() );
                Singleton<CalibrationCurveDB>.Instance.CommitData();

                var selectReagent = this.cmbReagentLotNo.SelectedIndex;
                this.cmbReagentLotNo.Items.Clear();
                this.lvwCalibrationCurve.Items.Clear();
                this.tabCalibrationCurve.Tabs.Clear();
                this.tabCalibrationCurve.Tabs.Add( String.Empty, String.Empty );
                changeCalibCurveShadowInside();
                this.tabCalibrationCurve.Tabs[0].TabPage.Controls.Add( this.gesturePanel );
                foreach ( var row in this.grdCalibCurve.Rows )
                {
                    row.Hidden = true;
                }
                this.loadData();
                this.cmbReagentLotNo.SelectedIndex = selectReagent;
                this.setGraphData();

                // 試薬テーブルの更新
                // ※検量線データが無くなった場合に試薬テーブル上のロット番号の色を変えるため
                RealtimeDataAgent.LoadReagentRemainData();

                // Form共通の編集中フラグOFF
                FormChildBase.IsEdit = false;
            }
            else
            {
                // TODO:削除できない旨のメッセージ
                DlgMessage.Show( CarisX.Properties.Resources.STRING_DLG_MSG_067, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK );
            }
        }

        /// <summary>
        /// 印刷
        /// </summary>
        /// <remarks>
        /// 操作履歴に印刷実行を登録し、検出量情報の印刷を行います
        /// </remarks>
        private void printData()
        {
            // 操作履歴登録：印刷実行   
            Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_005 } );

            TargetRange outputRange = DlgTargetSelectRange.Show();

            // 印刷対象を取得
            Dictionary<String, List<CalibrationCurveData>> printData = null;
            if ( TargetRange.None == outputRange )
            {
                // 操作履歴登録：印刷キャンセル   
                Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_006 } );
                return;
            }

            if ( this.currentProtocol != null )
            {
                // 出力範囲チェック
                switch ( outputRange )
                {
                    case TargetRange.All:
                        {
                            // 検量線データをロード
                            Singleton<CalibrationCurveDB>.Instance.LoadDB();

                            printData = Singleton<CalibrationCurveDB>.Instance.GetData(this.currentProtocol.ProtocolIndex
                                                                                      , this.cmbReagentLotNo.Text
                                                                                      , (int)this.cmbModuleNoSelection.Value);
                        }
                        break;

                    case TargetRange.Specification:
                        {

                            // 検量線データをロード
                            Singleton<CalibrationCurveDB>.Instance.LoadDB();

                            printData = Singleton<CalibrationCurveDB>.Instance.GetData(this.currentProtocol.ProtocolIndex
                                                                                      , this.cmbReagentLotNo.Text
                                                                                      , (int)this.cmbModuleNoSelection.Value);

                            var keys = from item in this.lvwCalibrationCurve.CheckedItems.OfType<UltraListViewItem>()
                                       select item.Key;

                            foreach (String key in printData.Keys.ToList())
                            {
                                if (!keys.Contains(key))
                                {
                                    printData.Remove(key);
                                }
                            }
                        }
                        break;

                    case TargetRange.None:
                    default:
                        // 処理なし
                        break;
                }
            }

            if ( printData == null || printData.Count == 0 || printData.Values.Sum( ( value ) => value.Count ) == 0 )
            {
                // 印刷するデータがありません。
                DlgMessage.Show( CarisX.Properties.Resources.STRING_DLG_MSG_064, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_003, MessageDialogButtons.Confirm );
                return;
            }

            // 印刷用Listに取得データを格納
            List<CalibratorAnalyReportData> rptData = new List<CalibratorAnalyReportData>();
            foreach ( var row in printData )
            {                
                List<CalibrationCurveData> prtRowList = (List<CalibrationCurveData>)row.Value;               

                foreach ( var prtRow in prtRowList )
                {                             
           
                    CalibratorAnalyReportData rptDataRow = new CalibratorAnalyReportData();
                    rptDataRow.CalibCurve = prtRow.GetApprovalDateTime().ToString();
                    rptDataRow.CalibCurveLimit = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( prtRow.GetMeasureProtocolIndex() ).ValidityOfCurve.ToString();
                    rptDataRow.Analytes = prtRow.Analytes;
                    rptDataRow.RackPosition = Convert.ToString( prtRow.RackPosition );
                    rptDataRow.MultiMeasNo = Convert.ToString( prtRow.MultiMeasNo );                                       
                    rptDataRow.ReagentLotNo = this.cmbReagentLotNo.Text;
                    rptDataRow.Conc = prtRow.Concentration;
                    rptDataRow.Count = (int)prtRow.Count;
                    rptDataRow.MeasPoint = prtRow.MeasPoint;
                    rptDataRow.PrintDateTime = DateTime.Now.ToDispString();
                    rptData.Add( rptDataRow );
                }
            }

            try
            {
                CalibratorAnalyPrint prt = new CalibratorAnalyPrint();
                String savePath = prt.SavePath.Replace( "'", String.Empty );

                // 画面グラフをキャプチャし、ファイルに保存
                // 縮小サイズ0.9倍
                double imageRatio = 0.9;

                // 画像の拡縮
                //デフォルトのサイズだとA4紙に収まらず、見切れたため縮小を行う
                Bitmap saveBmp = CarisXSubFunction.ScalingBitmap( chtCalibCurveInterDay.Image, imageRatio );

                // Jpegに変換し、保存
                saveBmp.Save( savePath, ImageFormat.Jpeg );

                // 印刷実行
                Boolean ret = prt.Print( rptData );
                System.IO.File.Delete( savePath );

                saveBmp.Dispose();
            }
            catch ( Exception ex )
            {
                System.Diagnostics.Debug.WriteLine( ex.Message );
                Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                          CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
            }


        }

        #endregion

        /// <summary>
        /// フォーム読み込みイベント
        /// </summary>
        /// <remarks>
        /// 画面初期化して、キャリブレータ検量線情報をロードします
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void FormCalibAnalysis_Load( object sender, EventArgs e )
        {
            this.lblAnalyte.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;
            this.cmbReagentLotNo.Items.Clear();
            this.lvwCalibrationCurve.Items.Clear();
            this.tabCalibrationCurve.Tabs.Clear();
            this.tabCalibrationCurve.Tabs.Add( String.Empty, String.Empty );
            this.tabCalibrationCurve.Tabs[0].TabPage.Controls.Add( this.gesturePanel );
            changeCalibCurveShadowInside();
            this.setEditMode( false );

            Singleton<CalibrationCurveDB>.Instance.LoadDB();
        }

        /// <summary>
        /// フェードイン表示
        /// </summary>
        /// <remarks>
        /// ログインして画面表示します
        /// </remarks>
        public override void Show(Rectangle captScreenRect)
        {
            // ログイン
            this.gbxEditMode.Visible = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction( CarisXUserLevelManagedAction.CalibratorEditRecalc );

            // 読み取り専用
            this.optEditMode.CheckedIndex = 0;

            base.Show( captScreenRect );
        }

        /// <summary>
        /// 分析項目選択ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 分析項目選択を更新します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnAnalytesSelect_Click( object sender, EventArgs e )
        {
            using ( DlgProtocolSelect dlg = new DlgProtocolSelect( false, 1 ) )
            {
                MeasureProtocol measureProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromName( this.lblAnalyte.Text );
                if ( measureProtocol != null )
                {
                    dlg.SelectedProtocolIndexs.Add( measureProtocol.ProtocolIndex );
                }
                if ( dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK )
                {
                    this.cmbReagentLotNo.Items.Clear();
                    this.lvwCalibrationCurve.Items.Clear();
                    this.tabCalibrationCurve.Tabs.Clear();
                    this.tabCalibrationCurve.Tabs.Add( String.Empty, String.Empty );
                    changeCalibCurveShadowInside();
                    this.tabCalibrationCurve.Tabs[0].TabPage.Controls.Add( this.gesturePanel );
                    foreach ( var row in this.grdCalibCurve.Rows )
                    {
                        row.Hidden = true;
                    }
                    this.chtCalibCurveInterDay.DataSource = null;

                    if ( dlg.SelectedProtocolIndexs.Count > 0 )
                    {
                        // 分析項目の取得
                        this.currentProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( dlg.SelectedProtocolIndexs[0] );

                        // 分析項目名を表示
                        this.lblAnalyte.Text = this.currentProtocol.ProtocolName;

                        // チャートの濃度単位を設定
                        this.chtCalibCurveInterDay.TitleBottom.Text = this.currentProtocol.ConcUnit;
                        this.grdCalibCurve.DisplayLayout.Bands[0].Columns[CalibrationCurveData.DataKeys.Concentration].Header.Caption = String.Format( Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_014, this.currentProtocol.ConcUnit );

                        this.loadData();
                    }
                    else
                    {
                        // 分析項目をクリア
                        this.currentCalibCurvesInfo = null;

                        // チャートの濃度単位を設定
                        this.chtCalibCurveInterDay.TitleBottom.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;
                        this.grdCalibCurve.DisplayLayout.Bands[0].Columns[CalibrationCurveData.DataKeys.Concentration].Header.Caption = String.Format( Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_014, Oelco.CarisX.Properties.Resources.STRING_COMMON_000 );

                        // 分析項目名を表示(選択なし)
                        this.lblAnalyte.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;

                        this.tlbCommandBar.Tools[SAVE].SharedProps.Enabled = false;
                    }

                    // Form共通の編集中フラグOFF
                    FormChildBase.IsEdit = false;
                }
            }
        }

        /// <summary>
        /// データの取得
        /// </summary>
        /// <remarks>
        /// 検量線情報を取得します
        /// </remarks>
        private void loadData()
        {
            if (this.currentProtocol != null )
            {
                // 検量線情報の取得
                this.currentCalibCurvesInfo = Singleton<CalibrationCurveDB>.Instance.GetAnalysisData(this.currentProtocol.ProtocolIndex
                                                                                                    , (int)this.cmbModuleNoSelection.Value);

                // 試薬ロット番号の設定
                var lotNo = from v in this.currentCalibCurvesInfo.Keys
                            select v;

                this.cmbReagentLotNo.DataSource = lotNo.ToList();
                this.cmbReagentLotNo.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 試薬ロット番号選択変更イベント
        /// </summary>
        /// <remarks>
        /// 試薬ロット番号選択を変更します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void cmbReagentLotNoSelection_ValueChanged( object sender, EventArgs e )
        {
            this.tabCalibrationCurve.Tabs.Clear();
            this.tabCalibrationCurve.Tabs.Add( String.Empty, String.Empty );
            changeCalibCurveShadowInside();
            this.tabCalibrationCurve.Tabs[0].TabPage.Controls.Add( this.gesturePanel );
            foreach ( var row in this.grdCalibCurve.Rows )
            {
                row.Hidden = true;
            }
            // グラフデータ設定
            this.setGraphData();

            // 検量線の取得
            this.setCalibrationCurve( this.currentProtocol.ProtocolIndex, this.cmbReagentLotNo.SelectedItem.DisplayText );

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// 検量線情報の設定
        /// </summary>
        /// <remarks>
        /// 検量線情報を設定します
        /// </remarks>
        /// <param name="protocolNo"></param>
        /// <param name="reagentLotNo"></param>
        private void setCalibrationCurve(Int32 protocolNo, String reagentLotNo)
        {
            // 検量線リストビューの初期化
            this.lvwCalibrationCurve.Items.Clear();

            // 検量線リストの追加
            foreach ( var calibCurve in this.currentCalibCurvesInfo[reagentLotNo] )
            {
                String asterisk = String.Empty;
                foreach ( CalibrationCurveData data in calibCurve.Value )
                {
                    if ( data.IsUserEdited() )
                    {
                        asterisk = Oelco.CarisX.Properties.Resources.STRING_COMMON_005;
                        break;
                    }
                }

                this.lvwCalibrationCurve.Items.Add( calibCurve.Key, calibCurve.Key + asterisk );
            }
        }

        /// <summary>
        /// 検量線情報の選択変更後イベント
        /// </summary>
        /// <remarks>
        /// 検量線情報の選択を変更しデータを設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void lvwCalibrationCurve_ItemSelectionChanged( object sender, ItemSelectionChangedEventArgs e )
        {
            foreach ( var item in e.SelectedItems )
            {
                switch ( item.CheckState )
                {
                case CheckState.Checked:
                    item.CheckState = CheckState.Unchecked;
                    break;
                case CheckState.Unchecked:
                    item.CheckState = CheckState.Checked;
                    break;
                case CheckState.Indeterminate:
                    item.CheckState = CheckState.Unchecked;
                    break;
                }
            }
            // タブ、グリッドの初期化
            this.tabCalibrationCurve.Tabs.Clear();

            // 編集中の全データをロールバック
            this.currentCalibCurvesInfo[this.cmbReagentLotNo.SelectedItem.DisplayText].Values.ToList().ForEach( ( listData ) => listData.RollBackDataList() );

            // タブ、グリッドの設定
            foreach ( UltraListViewItem item in this.lvwCalibrationCurve.CheckedItems )
            {
                item.Appearance.BackColor = CarisXConst.LIST_SELECT_COLOR;
                this.tabCalibrationCurve.Tabs.Add( item.Key, item.Text );
                changeCalibCurveShadowInside();
            }

            if ( this.tabCalibrationCurve.Tabs.Count == 0 )
            {
                this.tabCalibrationCurve.Tabs.Add( String.Empty, String.Empty );
                changeCalibCurveShadowInside();
                this.tabCalibrationCurve.Tabs[0].TabPage.Controls.Add( this.gesturePanel );
                foreach ( var row in this.grdCalibCurve.Rows )
                {
                    row.Hidden = true;
                }
            }

            foreach ( UltraListViewItem item in this.lvwCalibrationCurve.Items )
            {
                if ( !this.lvwCalibrationCurve.CheckedItems.Contains( item ) )
                {
                    item.Appearance.BackColor = this.lvwCalibrationCurve.ItemSettings.Appearance.BackColor;
                }
            }

            if ( e.SelectedItems.Count() > 0 )
            {
                foreach ( var item in e.SelectedItems )
                {
                    e.SelectedItems.Remove( item );
                }
            }

            // グラフデータ設定
            this.setGraphData();

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// 編集モード切替
        /// </summary>
        /// <remarks>
        /// 編集モードを切替します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void optEditMode_ValueChanged( object sender, EventArgs e )
        {
            if ( this.IsHandleCreated )
            {
                this.setEditMode( (Boolean)this.optEditMode.Value );
            }
        }
        /// <summary>
        /// ポイントデータ
        /// </summary>
        private struct PointData
        {
            public Int32 GroupIndex;
            public ItemPoint[] DataPoint;
            public ItemPoint[] ChartPoint;
        }

        /// <summary>
        /// グラフデータ設定
        /// </summary>
        /// <remarks>
        /// グラフデータを設定します
        /// </remarks>
        private void setGraphData()
        {
            // 定性項目のグラフ表示なし
            if ( this.currentProtocol.CalibType == MeasureProtocol.CalibrationType.CutOff
                    || this.currentProtocol.CalibType == MeasureProtocol.CalibrationType.INH )
            {
                this.chtCalibCurveInterDay.DataSource = null;
                return;
            }

            // グラフの初期化
            this.pointData = this.tabCalibrationCurve.Tabs.OfType<UltraTab>().Where( ( tab ) => tab.Key.Length > 0 ).Select( ( tab ) =>
            {
                var DataPoint = this.GetItemPoints( this.currentCalibCurvesInfo[this.cmbReagentLotNo.SelectedItem.DisplayText][tab.Key].Cast<CalibrationCurveData>().ToList() );
                this.concentrationCalculator.SetCalcData( DataPoint );

                ItemPoint[] ChartPoint = null;
                // 分析項目のキャリブレータ検量線補完方法取得
                switch ( this.currentProtocol.CalibType )
                {
                case MeasureProtocol.CalibrationType.LogitLog:
                    ChartPoint = this.concentrationCalculator.GetGraphPointLogitLog( CarisXConst.LOGITLOG_COEF_A, CarisXConst.LOGITLOG_COEF_B ).Clone() as ItemPoint[];
                    break;
                case MeasureProtocol.CalibrationType.Spline:
                    ChartPoint = this.concentrationCalculator.GetGraphPointSpline().Clone() as ItemPoint[];
                    break;
                case MeasureProtocol.CalibrationType.FourParameters:
                    // DBに4Parameterの算出値を保持している。
                    var firstData = this.currentCalibCurvesInfo[this.cmbReagentLotNo.SelectedItem.DisplayText][tab.Key].First();
                    Oelco.Common.Const.FourPTypeStruct Fparameter;
                    Fparameter.PType = (Oelco.Common.Const.FourPType)this.currentProtocol.FourPrameterMethodType;
                    Fparameter.ValueK = this.currentProtocol.FourPrameterMethodKValue;


                    ChartPoint = this.concentrationCalculator.GetGraphPointWithCoefFourParameter(
                        Fparameter,
                        firstData.GetExtendValue1() != null ? (Double?)Double.Parse( firstData.GetExtendValue1() ) : (Double?)null,
                        firstData.GetExtendValue2() != null ? (Double?)Double.Parse( firstData.GetExtendValue2() ) : (Double?)null,
                        firstData.GetExtendValue3() != null ? (Double?)Double.Parse( firstData.GetExtendValue3() ) : (Double?)null,
                        firstData.GetExtendValue4() != null ? (Double?)Double.Parse( firstData.GetExtendValue4() ) : (Double?)null ).Clone() as ItemPoint[];
					break;
				case MeasureProtocol.CalibrationType.DoubleLogarithmic1:
					ChartPoint = this.concentrationCalculator.GetGraphPointDoubleLogOne( DoubleLogarithmicMethod.CalcMode.Linear ).Clone() as ItemPoint[];
					break;
				case MeasureProtocol.CalibrationType.DoubleLogarithmic2:
					ChartPoint = this.concentrationCalculator.GetGraphPointDoubleLogTwo( DoubleLogarithmicMethod.CalcMode.Linear ).Clone() as ItemPoint[];
					break;
                default:
                    break;
                }

                return new PointData()
                {
                    GroupIndex = tab.Index,
                    DataPoint = DataPoint,
                    ChartPoint = ChartPoint
                };
            } ).ToList();

            // ポイント作成の前に全体数を取得する
            Double[,] points = new double[this.pointData.Sum( ( point ) => point.ChartPoint.Length ), 3];
            Int32 index = 0;
            // 複数の系列に対応、インデックスは通番となる。
            foreach ( var data in this.pointData )
            {
                foreach ( var point in data.ChartPoint )
                {
                    points[index, 0] = data.GroupIndex; // 0番目 グループID
                    points[index, 1] = point.xPos;      // 1番目 X座標
                    points[index++, 2] = point.yPos;    // 2番目 Y座標
                }
            }
            //修改曲线显示方式增加 "X-Y","X-Log(Y)","X-Logit(Y)"
            setCalibDisplayStyle();


            //this.chtCalibCurveInterDay.SuspendLayout();
            //this.chtCalibCurveInterDay.DataBind();
        }

        /// <summary>
        /// 検量線グラフキャリブレーションポイント座標生成
        /// </summary>
        /// <remarks>
        /// 検量線グラフキャリブレーションポイント座標を生成します
        /// </remarks>
        /// <param name="list">検量線情報リスト</param>
        /// <returns></returns>
        private ItemPoint[] GetItemPoints( List<CalibrationCurveData> list )
        {
            List<ItemPoint> points = new List<ItemPoint>();
            var posGroup = from v in list
                           group v by v.GetPointNo() into grp
                           select grp.OrderBy( ( data ) => data.MultiMeasNo );
            foreach ( var dataList in posGroup )
            {
                if ( dataList != null )
                {
                    if ( dataList.Count() > 1 )
                    {
                        points.Add( new ItemPoint( dataList.Last().CountAverage.Value, Double.Parse( dataList.Last().Concentration ) ) );
                    }
                    else
                    {
                        points.Add( new ItemPoint( dataList.Last().Count.Value, Double.Parse( dataList.Last().Concentration ) ) );
                    }
                }
            }

            return points.ToArray();
        }

        /// <summary>
        /// モード切替
        /// </summary>
        /// <remarks>
        /// モード切替します
        /// </remarks>
        /// <param name="mode">true:編集/false:読み込みのみ</param>
        private void setEditMode( Boolean mode )
        {
            this.countColumnActivation = Activation.NoEdit;
            this.countColumnCellBackColor = Color.Empty;

            if ( mode && this.tabCalibrationCurve.ActiveTab != null && this.tabCalibrationCurve.ActiveTab.Text != Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_018 )
            {
                // 編集可能なスタイルに変更
                countColumnActivation = Activation.AllowEdit;
                countColumnCellBackColor = CarisXConst.GRID_CELL_CAN_EDIT_COLOR;

                if ( this.cmbReagentLotNo.SelectedItem != null && this.currentCalibCurvesInfo != null && this.currentCalibCurvesInfo.ContainsKey( this.cmbReagentLotNo.SelectedItem.DisplayText ) && this.currentCalibCurvesInfo[this.cmbReagentLotNo.SelectedItem.DisplayText].Keys.Contains( this.tabCalibrationCurve.ActiveTab.Key ) )
                {
                    this.tlbCommandBar.Tools[SAVE].SharedProps.Enabled = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction( CarisXUserLevelManagedAction.CalibratorEditRecalc );
                }
            }
            else
            {

                this.tlbCommandBar.Tools[SAVE].SharedProps.Enabled = false;
            }

            this.grdCalibCurve.DisplayLayout.Bands[0].Columns[CalibrationCurveData.DataKeys.Count].CellActivation = countColumnActivation;
            this.grdCalibCurve.DisplayLayout.Bands[0].Columns[CalibrationCurveData.DataKeys.Count].CellAppearance.BackColor = countColumnCellBackColor;


        }

        /// <summary>
        /// フォームの表示状態切り替えイベント
        /// </summary>
        /// <remarks>
        /// 編集モードの初期化します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void FormCalibAnalysis_VisibleChanged( object sender, EventArgs e )
        {
            if ( this.Visible )
            {
                // 編集モードの初期化(Read指定)
                this.optEditMode.Items[0].CheckState = CheckState.Checked;
                this.setEditMode( false );
            }
        }

        /// <summary>
        /// タブの切り替えイベント
        /// </summary>
        /// <remarks>
        /// 分析項目列設定し、グリッド編集状態を切り替えします
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void tabCalibrationCurve_SelectedTabChanged( object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e )
        {
            if ( this.cmbReagentLotNo.SelectedItem != null && this.currentCalibCurvesInfo != null && this.currentCalibCurvesInfo.ContainsKey( this.cmbReagentLotNo.SelectedItem.DisplayText ) && this.currentCalibCurvesInfo[this.cmbReagentLotNo.SelectedItem.DisplayText].Keys.Contains( e.Tab.Key ) )
            {
                this.grdCalibCurve.DataSource = new BindingList<CalibrationCurveAnalysisData>( this.currentCalibCurvesInfo[this.cmbReagentLotNo.SelectedItem.DisplayText][e.Tab.Key] );

                if ( (Boolean)this.optEditMode.Value )
                {
                    this.tlbCommandBar.Tools[SAVE].SharedProps.Enabled = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction( CarisXUserLevelManagedAction.CalibratorEditRecalc );
                }

                // 分析項目列のセル値を設定
                foreach ( UltraGridRow row in this.grdCalibCurve.Rows )
                {
                    row.Cells[CalibrationCurveData.DataKeys.Analytes].Value = this.currentProtocol.ProtocolName;
                }
                e.Tab.TabPage.Controls.Add( this.gesturePanel );

                // グリッド編集状態を切り替え
                this.setEditMode( (Boolean)this.optEditMode.Value && e.Tab.Key != Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_018 );
            }
            else
            {
                this.tlbCommandBar.Tools[SAVE].SharedProps.Enabled = false;
            }
        }

        /// <summary>
        /// グリッドセル編集モード終了後イベント
        /// </summary>
        /// <remarks>
        /// カウント平均値の更新、グループ(ラックポジション)毎に平均算出しセルへ反映します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdCalibCurve_AfterExitEditMode( object sender, EventArgs e )
        {
            if ( (Boolean)this.optEditMode.Value )
            {
                // カウント値変更に伴う、カウント平均値の更新
                UltraGridRow rackPosRow = this.grdCalibCurve.Rows[0];
                Int32 countAverage = 0;
                Int32 multiMeasNoMax = 0; // 検体多重測定回数の最大値

                // ラックポジション毎にレプリケーションをグループ化
                var posData = from v in this.grdCalibCurve.Rows
                              group v by v.Cells[CalibrationCurveData.DataKeys.Concentration].Text into grp
                              select grp;

                // グループ(ラックポジション)毎に平均算出、セルへ反映
                foreach ( var repData in posData )
                {
                    multiMeasNoMax = repData.Max( ( data ) => (Int32)data.Cells[CalibrationCurveData.DataKeys.MultiMeasNo].Value );
                    if ( multiMeasNoMax > 1 )
                    {
                        countAverage = repData.Sum( ( data ) => (Int32)data.Cells[CalibrationCurveData.DataKeys.Count].Value ) / multiMeasNoMax;
                        var aveData = (CalibrationCurveData)repData.Single( ( data ) => (Int32)data.Cells[CalibrationCurveData.DataKeys.MultiMeasNo].Value == multiMeasNoMax ).ListObject;
                        aveData.CountAverage = countAverage;
                    }
                }

                this.grdCalibCurve.DataBind();
            }
        }

        /// <summary>
        /// セルのアクティブ前イベント
        /// </summary>
        /// <remarks>
        /// 編集モードにより確認メッセージ表示します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void grdCalibCurve_BeforeCellActivate( object sender, CancelableCellEventArgs e )
        {
            if ( (Boolean)this.optEditMode.Value )
            {
                if ( e.Cell.Column.Key == CalibrationCurveData.DataKeys.Count && String.IsNullOrEmpty( e.Cell.Row.Cells[CalibrationCurveData.DataKeys.MeasPoint].Text ) )
                {
                    e.Cancel = true;

                    // 
                    DlgMessage.Show( CarisX.Properties.Resources.STRING_DLG_MSG_180, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.Confirm );
                }
            }
        }

        /// <summary>
        /// セル入力エラー時処理
        /// </summary>
        /// <remarks>
        /// エラーイベントを発生します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdCalibCurve_CellDataError( object sender, CellDataErrorEventArgs e )
        {
            e.RaiseErrorEvent = false;
        }

        /// <summary>
        /// FormClosedイベント
        /// </summary>
        /// <remarks>
        /// UI設定保存します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormCalibAnalysis_FormClosed( object sender, FormClosedEventArgs e )
        {
            // UI設定保存
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.CalibAnalysisSettings.GridColOrder = this.grdCalibCurve.GetGridColumnOrder();
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.CalibAnalysisSettings.GridColWidth = this.grdCalibCurve.GetGridColmnWidth();
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
        /// ルーチンテーブル変更時処理
        /// </summary>
        /// <remarks>
        /// 分析項目の使用状況が変更された際の動作です。
        /// </remarks>
        /// <param name="value">不使用</param>
        private void onAnalyteRoutineTableChanged( object value )
        {
            if ( this.currentCalibCurvesInfo != null )
            {
                this.cmbReagentLotNo.Items.Clear();
                this.lvwCalibrationCurve.Items.Clear();
                this.tabCalibrationCurve.Tabs.Clear();

                this.tabCalibrationCurve.Tabs.Add( String.Empty, String.Empty );
                changeCalibCurveShadowInside();
                this.tabCalibrationCurve.Tabs[0].TabPage.Controls.Add( this.gesturePanel );
                foreach ( var row in this.grdCalibCurve.Rows )
                {
                    row.Hidden = true;
                }

                // 分析項目をクリア
                this.currentCalibCurvesInfo.Clear();
                this.currentCalibCurvesInfo = null;

                // チャートの濃度単位を設定
                this.chtCalibCurveInterDay.TitleBottom.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;
                this.grdCalibCurve.DisplayLayout.Bands[0].Columns[CalibrationCurveData.DataKeys.Concentration].Header.Caption = String.Format( Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_014, Oelco.CarisX.Properties.Resources.STRING_COMMON_000 );

                // 分析項目名を表示(選択なし)
                this.lblAnalyte.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;

                this.tlbCommandBar.Tools[SAVE].SharedProps.Enabled = false;

                // グラフクリア
                this.chtCalibCurveInterDay.DataSource = null;
            }
        }
        #endregion

        private void setCalibDisplayStyle()
        {
            if (null == this.pointData)
            {
                return;
            }

            int lenght = this.pointData.Sum((point) => point.ChartPoint.Length);
            Double[,] points = new double[lenght, 3];
            Int32 index = 0;

            if (this.pointData != null && this.pointData.Count() > 0)
            {               
                foreach (var data in this.pointData)
                {
                    foreach (var point in data.ChartPoint)
                    {
                        points[index, 0] = data.GroupIndex; // 0番目 グループID
                        if (this.cmbCalibDiplayStyle.SelectedIndex == 0)
                        {
                            points[index, 1] = point.xPos;      // 1番目 X座標
                            points[index++, 2] = point.yPos;    // 2番目 Y座標
                        }

                        if (this.cmbCalibDiplayStyle.SelectedIndex == 1)
                        {
                            if (point.yPos > 0)
                            {
                                points[index, 1] = point.xPos;      // 1番目 X座標
                                points[index++, 2] = Math.Log(point.yPos);    // 2番目 Y座標
                            }
                        }

                        if (this.cmbCalibDiplayStyle.SelectedIndex == 2)
                        {
                            if (point.xPos > 0 && point.yPos > 0)
                            {
                                points[index, 1] = Math.Log(point.xPos);
                                points[index++, 2] = Math.Log(point.yPos);
                            }

                        }

                    }
                }
                for (int i = index - 1; i < lenght; i++)
                {
                    points[i, 1] = points[i - 1, 1];
                    points[i, 2] = points[i - 1, 2];
                }

            }

            this.chtCalibCurveInterDay.SuspendLayout();
            this.updateGraphAxisSetting();
            this.chtCalibCurveInterDay.Data.DataSource = points;
            this.chtCalibCurveInterDay.DataBind(); 
        }

        private void cmbCalibDiplayStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            setCalibDisplayStyle();
        }

        /// <summary>
        /// CalibCurveのタブの上部にある影の位置、幅を変更する
        /// </summary>
        private void changeCalibCurveShadowInside()
        {
            Point baseShadowLocation = new Point(470, 600); //影の基準表示場所
            Size baseShadowSize = new Size(960, 13);        //影の基準表示サイズ
            Int32 firstTabSize = 212;                       //先頭タブ（＝必ず選択中になる）のサイズ
            Int32 otherTabSize = 209;                       //先頭以外のタブ（＝未選択）のサイズ
            Int32 adjustSize = 0;                           //調整幅

            switch (tabCalibrationCurve.Tabs.Count)
            {
                case 1:
                    //タブが１つだけの場合は調整幅が異なる
                    adjustSize = firstTabSize;
                    break;

                default:
                    //タブが複数の場合は先頭タブ＋残りの他のタブの幅で調整する
                    adjustSize = firstTabSize + (otherTabSize * (tabCalibrationCurve.Tabs.Count - 1));
                    break;
            }

            if (adjustSize >= baseShadowSize.Width)
            {
                //調整幅が影の表示サイズを超えている場合は影を表示しない
                pnlCalibrationCurveShadowInside.Visible = false;
            }
            else
            {
                //調整幅が影の表示サイズを超えていない場合は影の場所、サイズから調整幅を加算・減算する
                pnlCalibrationCurveShadowInside.Visible = true;
                pnlCalibrationCurveShadowInside.Location = new Point(baseShadowLocation.X + adjustSize, baseShadowLocation.Y);
                pnlCalibrationCurveShadowInside.Size = new Size(baseShadowSize.Width - adjustSize, baseShadowSize.Height);
            }
        }

        private void chtCalibCurveInterDay_ChartDataClicked( object sender, Infragistics.UltraChart.Shared.Events.ChartDataEventArgs e )
        {

        }

        /// <summary>
        /// モジュール番号切替処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbModuleNoSelection_ValueChanged(object sender, EventArgs e)
        {
            this.cmbReagentLotNo.Items.Clear();
            this.lvwCalibrationCurve.Items.Clear();
            this.tabCalibrationCurve.Tabs.Clear();
            this.tabCalibrationCurve.Tabs.Add(String.Empty, String.Empty);
            this.changeCalibCurveShadowInside();
            this.tabCalibrationCurve.Tabs[0].TabPage.Controls.Add(this.gesturePanel);
            foreach (var row in this.grdCalibCurve.Rows)
            {
                row.Hidden = true;
            }
            this.chtCalibCurveInterDay.DataSource = null;

            // 分析項目をクリア
            this.currentCalibCurvesInfo = null;

            // チャートの濃度単位を設定
            this.chtCalibCurveInterDay.TitleBottom.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;
            this.grdCalibCurve.DisplayLayout.Bands[0].Columns[CalibrationCurveData.DataKeys.Concentration].Header.Caption = String.Format(Oelco.CarisX.Properties.Resources.STRING_CALIBANALYSIS_014, Oelco.CarisX.Properties.Resources.STRING_COMMON_000);

            // 再ロード
            this.loadData();

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// キャリブカーブグリッドセル変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdCalibCurve_CellChange(object sender, CellEventArgs e)
        {
            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }
    }
}
