using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.UltraChart.Core;
using Infragistics.UltraChart.Core.Primitives;
using Infragistics.UltraChart.Shared.Styles;
using Oelco.Common.Calculator;
using Oelco.CarisX.DB;

namespace Prototype
{
    public partial class GraphTest : Form
    {
        public GraphTest()
        {
            InitializeComponent();

            for ( int i = 0; i < 8 ; i++ )
            {
                dgvInputData.Rows.Add();
                dgvInputData.Rows[i].Cells["Count"].Value = i.ToString();
                dgvInputData.Rows[i].Cells["Conc"].Value = i.ToString() + ".0";

            }

            this.chtCalibCurveInterDay.FillSceneGraph += ( sender, e ) =>
            {
				try
				{
					//　座標軸を取得
					IAdvanceAxis x = (IAdvanceAxis)e.Grid["X"];
					IAdvanceAxis y = (IAdvanceAxis)e.Grid["Y"];

					List<Symbol> symbolList = new List<Symbol>();
					var pointSetList = e.SceneGraph.OfType<Primitive>().Where( ( primitive ) => ( primitive as PointSet ) != null ).ToArray();
					foreach ( PointSet pointset in pointSetList )
					{
						//                    var charPoint = this.pointData.First( ( data ) => data.GroupIndex == pointset.Row ).DataPoint;
						var charPoint = this.pointData.First( ( data ) => data.GroupIndex == 1 ).DataPoint;
						var pe = pointset.PE.Clone();
						pe.FillOpacity = Byte.MaxValue;
						DataPoint[] dataPoint = charPoint.Select( ( plotPoint ) => new DataPoint( new Point( (Int32)x.Map( plotPoint.xPos ), (Int32)y.Map( plotPoint.yPos ) ) ) ).ToArray();
						PointSet pset = new PointSet( dataPoint, SymbolIcon.Circle, (SymbolIconSize)7 )
						{
							PE = pe
						};
						//e.SceneGraph.Insert( e.SceneGraph.IndexOf( pointset ) + 1, pset );
						e.SceneGraph.Add( pset );
					}
				}
				catch ( Exception ex )
				{
					MessageBox.Show( ex.Message );
				}
            };


            this.comboBox1.SelectedIndex = 0;
            this.ckbUseX.Checked = true;
            this.チェック反映();
        }

        /// <summary>
        /// データプロットポイント情報
        /// </summary>
        private IEnumerable<PointData> pointData;

        private struct PointData
        {
            public Int32 GroupIndex;
            public ItemPoint[] DataPoint;
            public ItemPoint[] ChartPoint;
        }

        private void GraphTest_Load( object sender, EventArgs e )
        {

        }

        private void btnCalcExecute_Click( object sender, EventArgs e )
        {
            this.setGraphData();
        }

        /// <summary>
        /// 検量線補完算出(グラフ出力用)
        /// </summary>
        private ConcentrationCalculator concentrationCaliculator = new ConcentrationCalculator();
        /// <summary>
        /// グラフデータを設定する
        /// </summary>
        private void setGraphData()
        {
            // 4Para,Spline,LogiLog
            Int32 selected = comboBox1.SelectedIndex;

            // グラフの初期化
            var DataPoint = this.GetItemPoints( this.dgvInputData );
            this.concentrationCaliculator.SetCalcData( DataPoint );

            ItemPoint[] ChartPoint = null;
            Tuple<Double, Double, Double, Double> fourParamCoefs = new Tuple<Double, Double, Double, Double>( (Double)this.une4pCoefA.Value, (Double)this.une4pCoefB.Value, (Double)this.une4pCoefC.Value, (Double)this.une4pCoefD.Value );
            Tuple<ItemPoint[], double, double, double, double> four;
            // 分析項目のキャリブレータ検量線補完方法取得
            ICalcMethod method = null;
			Func<ItemPoint[], DataGridView, Boolean, Boolean, ItemPoint[]> pointExtender = ( v, vv, usex, write ) =>
			{
				List<ItemPoint> newPo = new List<ItemPoint>();
				try
				{
					ItemPoint[] pointz = this.GetExtendItemPoints( vv );

					Int32 dgvindex = 0;
					foreach ( var dt in pointz )
					{
						newPo.Add( new ItemPoint( !usex ? dt.yPos : method.GetY( dt.xPos ), usex ? dt.xPos : method.GetX( dt.yPos ) ) );
						if ( write )
						{
							vv.Rows[dgvindex].Cells[0].Value = newPo.Last().xPos.ToString();
							vv.Rows[dgvindex].Cells[1].Value = newPo.Last().yPos.ToString();
						}
						dgvindex++;
					}

				}
				catch ( Exception ex )
				{
					MessageBox.Show( ex.Message );
				}
				// vにvvを使って出した点を統合して返す
				var result = v.Union( newPo ).ToArray();       // ソートしていない版
				return result;
			};
            switch ( selected )
            {
			case 4:
				this.chtCalibCurveInterDay.Axis.X.NumericAxisType = NumericAxisType.Logarithmic;
				this.chtCalibCurveInterDay.Axis.Y.NumericAxisType = NumericAxisType.Logarithmic;
				ChartPoint = this.concentrationCaliculator.GetGraphPointDoubleLogTwo(DoubleLogarithmicMethod.CalcMode.Linear).Clone() as ItemPoint[];
				var metDl2 = new DoubleLogarithmicMethod( DoubleLogarithmicMethod.Dimention.Two );
				metDl2.CalcModeSettings = DoubleLogarithmicMethod.CalcMode.Linear;
				metDl2.CalcuCoef( DataPoint.Select( ( v ) => v.xPos ).ToArray(), DataPoint.Select( ( v ) => v.yPos ).ToArray(), DataPoint.Count() );
				method = metDl2;
				this.uneDoubleLogCoefA.Value = metDl2.CoefA;
				this.uneDoubleLogCoefB.Value = metDl2.CoefB;
				this.uneDoubleLogCoefC.Value = metDl2.CoefC;
				break;
			case 3:
				//ChartPoint = this.concentrationCaliculator.GetGraphPointDoubleLogOne( DoubleLogarithmicMethod.CalcMode.Log ).Clone() as ItemPoint[];
				this.chtCalibCurveInterDay.Axis.X.NumericAxisType = NumericAxisType.Logarithmic;
				this.chtCalibCurveInterDay.Axis.Y.NumericAxisType = NumericAxisType.Logarithmic;

				//this.chtCalibCurveInterDay.Axis.Y.TickmarkStyle = AxisTickStyle.Percentage;
				//this.chtCalibCurveInterDay.Axis.X.TickmarkStyle = AxisTickStyle.Percentage;
				

				ChartPoint = this.concentrationCaliculator.GetGraphPointDoubleLogOne( DoubleLogarithmicMethod.CalcMode.Linear ).Clone() as ItemPoint[];
				var metDl1 = new DoubleLogarithmicMethod( DoubleLogarithmicMethod.Dimention.One );
				metDl1.CalcModeSettings = DoubleLogarithmicMethod.CalcMode.Linear;
				metDl1.CalcuCoef( DataPoint.Select( ( v ) => v.xPos ).ToArray(), DataPoint.Select( ( v ) => v.yPos ).ToArray(), DataPoint.Count() );
				method = metDl1;
				this.uneDoubleLogCoefA.Value = metDl1.CoefA;
				this.uneDoubleLogCoefB.Value = metDl1.CoefB;
				this.uneDoubleLogCoefC.Value = 0.0d;
				break;
			case 2:
				this.chtCalibCurveInterDay.Axis.X.NumericAxisType = NumericAxisType.Linear;
				this.chtCalibCurveInterDay.Axis.Y.NumericAxisType = NumericAxisType.Linear;
                ChartPoint = this.concentrationCaliculator.GetGraphPointLogitLog( (Double)this.coefA.Value, (Double)this.coefB.Value ).Clone() as ItemPoint[];
                var metLo = new LogitLogMethod( (Double)this.coefA.Value, (Double)this.coefB.Value );
                metLo.CalcuCoef( DataPoint.Select( ( v ) => v.xPos ).ToArray(), DataPoint.Select( ( v ) => v.yPos ).ToArray(), DataPoint.Count(), false );
                method = metLo;
                break;
			case 1:
				this.chtCalibCurveInterDay.Axis.X.NumericAxisType = NumericAxisType.Linear;
				this.chtCalibCurveInterDay.Axis.Y.NumericAxisType = NumericAxisType.Linear;
                ChartPoint = this.concentrationCaliculator.GetGraphPointSpline().Clone() as ItemPoint[];
                var metSp = new SplineMethod();
                metSp.CalcuCoef( DataPoint.Select( ( v ) => v.xPos ).ToArray(), DataPoint.Select( ( v ) => v.yPos ).ToArray(), DataPoint.Count() );
                method = metSp;
                break;
			case 0:
				this.chtCalibCurveInterDay.Axis.X.NumericAxisType = NumericAxisType.Linear;
				this.chtCalibCurveInterDay.Axis.Y.NumericAxisType = NumericAxisType.Linear;
                FourParameterMethod met4;
                //if ( this.ckb4paramUse.Checked )
                //{
                //    ChartPoint = this.concentrationCaliculator.GetGraphPointWithCoefFourParameter( fourParamCoefs.Item1, fourParamCoefs.Item2, fourParamCoefs.Item3, fourParamCoefs.Item4 );
                //    met4 = new FourParameterMethod( DataPoint.Count() );
                //    met4.SetData( DataPoint.Select( ( v ) => v.xPos ).ToArray(), DataPoint.Select( ( v ) => v.yPos ).ToArray(), DataPoint.Count() );
                //    met4.Calc4Parameter();
                //}
                //else
                //{
                //    four = this.concentrationCaliculator.GetGraphPointWithCoefFourParameter();
                //    ChartPoint = four.Item1.Clone() as ItemPoint[];
                //    this.une4pCoefA.Value = four.Item2;
                //    this.une4pCoefB.Value = four.Item3;
                //    this.une4pCoefC.Value = four.Item4;
                //    this.une4pCoefD.Value = four.Item5;
                //    met4 = new FourParameterMethod( DataPoint.Count(), four.Item2, four.Item3, four.Item4, four.Item5 );

                //}
               // method = met4;
                method = null;
                break;
            default:
                break;
            }

            this.pointData = new List<PointData>()
            { new PointData()
                {
                    GroupIndex = 1,
                    DataPoint =  pointExtender( DataPoint, this.dgvNoruData,this.ckbUseX.Checked,false),
                    ChartPoint = pointExtender( ChartPoint,this.dgvNoruData,this.ckbUseX.Checked,true) 
                }
            };

            // ポイント作成の前に全体数を取得する
            double[,] points = new double[this.pointData.Sum( ( point ) => point.ChartPoint.Length ), 3];
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

            // データソースの適用
            this.chtCalibCurveInterDay.Data.DataSource = points;

            double rangeMaxX = -1;
			double rangeMaxY = -1;
			double rangeMinX = -1;
			double rangeMinY = -1;
            if ( this.pointData.Count() > 0 )
            {
                rangeMaxX = this.pointData.Max( ( data ) =>
                {
                    if ( data.ChartPoint != null && data.ChartPoint.Count() > 0 )
                    {
                        return data.ChartPoint.Max( ( point ) => point.xPos );
                    }
                    else
                    {
                        return -1;
                    }
                } );
                rangeMaxY = this.pointData.Max( ( data ) =>
                {
                    if ( data.ChartPoint != null && data.ChartPoint.Count() > 0 )
                    {
                        return data.ChartPoint.Max( ( point ) => point.yPos );
                    }
                    else
                    {
                        return -1;
                    }
                } );
				rangeMinX = this.pointData.Min( ( data ) =>
				{
					if ( data.ChartPoint != null && data.ChartPoint.Count() > 0 )
					{
						return data.ChartPoint.Min( ( point ) => point.xPos );
					}
					else
					{
						return -1;
					}
				} );
				rangeMinY = this.pointData.Min( ( data ) =>
				{
					if ( data.ChartPoint != null && data.ChartPoint.Count() > 0 )
					{
						return data.ChartPoint.Min( ( point ) => point.yPos );
					}
					else
					{
						return -1;
					}
				} );
            }
			if ( this.chtCalibCurveInterDay.Axis.Y.NumericAxisType == NumericAxisType.Logarithmic )
			{
				this.chtCalibCurveInterDay.Axis.X.RangeType = AxisRangeType.Custom;
				this.chtCalibCurveInterDay.Axis.Y.RangeType = AxisRangeType.Custom;
				this.chtCalibCurveInterDay.Axis.Y.TickmarkStyle = AxisTickStyle.DataInterval;
				this.chtCalibCurveInterDay.Axis.X.TickmarkStyle = AxisTickStyle.DataInterval;
				
				Double tmpVal;
				Int32 tmpVal2;

				tmpVal = Math.Log10( rangeMinX );
				tmpVal2 = tmpVal < (double)( (int)tmpVal ) ? (int)tmpVal - 1 : (int)tmpVal;
				this.chtCalibCurveInterDay.Axis.X.RangeMin = Math.Pow( 10, tmpVal2 );

				tmpVal = Math.Log10( rangeMaxX );
				tmpVal2 = tmpVal > (double)( (int)tmpVal ) ? (int)tmpVal + 1 : (int)tmpVal;
				this.chtCalibCurveInterDay.Axis.X.RangeMax = Math.Pow( 10, tmpVal2 );


				tmpVal = Math.Log10( rangeMinY );
				tmpVal2 = tmpVal < (double)( (int)tmpVal ) ? (int)tmpVal - 1 : (int)tmpVal;
				this.chtCalibCurveInterDay.Axis.Y.RangeMin = Math.Pow( 10, tmpVal2 );

				tmpVal = Math.Log10( rangeMaxY );
				tmpVal2 = tmpVal > (double)( (int)tmpVal ) ? (int)tmpVal + 1 : (int)tmpVal;
				this.chtCalibCurveInterDay.Axis.Y.RangeMax = Math.Pow( 10, tmpVal2 );

				this.chtCalibCurveInterDay.Axis.X.TickmarkInterval = 1;
				this.chtCalibCurveInterDay.Axis.Y.TickmarkInterval = 1;

				this.chtCalibCurveInterDay.Axis.X.MinorGridLines.Visible = true;
				this.chtCalibCurveInterDay.Axis.Y.MinorGridLines.Visible = true;
			}
			else
			{
				this.chtCalibCurveInterDay.Axis.X.RangeType = AxisRangeType.Automatic;
				this.chtCalibCurveInterDay.Axis.Y.RangeType = AxisRangeType.Automatic;
				this.chtCalibCurveInterDay.Axis.Y.TickmarkStyle = AxisTickStyle.Smart;
				this.chtCalibCurveInterDay.Axis.X.TickmarkStyle = AxisTickStyle.Smart;
				this.chtCalibCurveInterDay.Axis.X.RangeMin = 0;
				this.chtCalibCurveInterDay.Axis.X.RangeMax = rangeMaxX;
				this.chtCalibCurveInterDay.Axis.Y.RangeMin = 0;
				this.chtCalibCurveInterDay.Axis.Y.RangeMax = rangeMaxY;
				this.chtCalibCurveInterDay.Axis.X.TickmarkInterval = rangeMaxX/10;
				this.chtCalibCurveInterDay.Axis.Y.TickmarkInterval = rangeMaxY/10;
				this.chtCalibCurveInterDay.Axis.X.MinorGridLines.Visible = false;
				this.chtCalibCurveInterDay.Axis.Y.MinorGridLines.Visible = false;
				if ( rangeMaxX > 0 )
				{
					this.chtCalibCurveInterDay.Axis.X.RangeMax = rangeMaxX;
					this.chtCalibCurveInterDay.Axis.X.TickmarkInterval = rangeMaxX / 10;
				}
			}
            this.hokanPoints = points;
            this.chtCalibCurveInterDay.DataBind();
        }

        double[,] hokanPoints = null;

        /// <summary>
        /// 検量線グラフキャリブレーションポイント座標生成
        /// </summary>
        /// <param name="list">検量線情報リスト</param>
        /// <returns></returns>
        private ItemPoint[] GetItemPoints( DataGridView dgv )
        {
            List<ItemPoint> points = new List<ItemPoint>();
            var posGroup = from v in dgv.Rows.OfType<DataGridViewRow>()
                           where v.Cells["Count"].Value != null && v.Cells["Conc"].Value != null
                           select new ItemPoint( Double.Parse( (String)v.Cells["Count"].Value ), Double.Parse( (String)v.Cells["Conc"].Value ) );
            return posGroup.ToArray();
        }

        private ItemPoint[] GetExtendItemPoints( DataGridView dgv )
        {
            List<ItemPoint> points = new List<ItemPoint>();
            var posGroup = from v in dgv.Rows.OfType<DataGridViewRow>()
                           where v.Cells["ContEx"].Value != null || v.Cells["ConcEx"].Value != null
                           select new ItemPoint( v.Cells["ContEx"].Value == null ? 0 : Double.Parse( (String)v.Cells["ContEx"].Value ), v.Cells["ConcEx"].Value == null ? 0 : Double.Parse( (String)v.Cells["ConcEx"].Value ) );
            return posGroup.ToArray();
        }
        private void ultraButton1_Click( object sender, EventArgs e )
        {
            if ( this.hokanPoints != null )
            {

                StringBuilder putStrings = new StringBuilder();
                putStrings.AppendLine( "Conc,Count" );
                for ( Int32 idx = 0; idx < this.hokanPoints.GetLength( 0 ); idx++ )
                {
                    putStrings.AppendLine( String.Format( "{0},{1}", this.hokanPoints[idx, 1].ToString(), this.hokanPoints[idx, 2].ToString() ) );
                }

                SaveFileDialog sdlg = new SaveFileDialog();
                sdlg.FileName = "output.csv";
                sdlg.InitialDirectory = System.IO.Path.GetDirectoryName( Application.ExecutablePath );
                if ( sdlg.ShowDialog() == System.Windows.Forms.DialogResult.OK )
                {
                    try
                    {
                        System.IO.File.WriteAllText( sdlg.FileName, putStrings.ToString(), Encoding.ASCII );


                    }
                    catch ( Exception ex  )
                    {
                        MessageBox.Show( ex.Message );    
                    }
                }
            }
        }

        private void dgvInputData_KeyDown( object sender, KeyEventArgs e )
        {
            if ( e.Control == true  && e.KeyCode == Keys.V)
            {
                //現在のセルのある行から下にペーストする
                if ( this.dgvInputData.CurrentCell == null )
                    return;
                int insertRowIndex = this.dgvInputData.CurrentCell.RowIndex;
                int insertColIndex = this.dgvInputData.CurrentCell.ColumnIndex;
                //クリップボードの内容を取得して、行で分ける
                string pasteText = Clipboard.GetText();
                if ( string.IsNullOrEmpty( pasteText ) )
                    return;
                pasteText = pasteText.Replace( "\r\n", "\n" );
                pasteText = pasteText.Replace( '\r', '\n' );
                pasteText = pasteText.TrimEnd( new char[] { '\n' } );
                string[] lines = pasteText.Split( '\n' );

                //bool isHeader = true;
                foreach ( string line in lines )
                {
                    //列ヘッダーならば飛ばす
                    //if ( isHeader )
                    //{
                    //    isHeader = false;
                    //    continue;
                    //}

                    //タブで分割
                    string[] vals = line.Split( '\t' );
                    //列数が合っているか調べる
                    if ( vals.Length != this.dgvInputData.ColumnCount - insertColIndex )
                        return;
//                        throw new ApplicationException( "列数が違います。" );
                    if ( this.dgvInputData.Rows.Count <= insertRowIndex )
                    {
                        this.dgvInputData.Rows.Add();
                    }
                    DataGridViewRow row = this.dgvInputData.Rows[insertRowIndex];
                    //ヘッダーを設定
                    //row.HeaderCell.Value = vals[0];
                    //各セルの値を設定
                    for ( int i = 0; i < row.Cells.Count - insertColIndex; i++ )
                    {
                        row.Cells[i + insertColIndex].Value = vals[i];
                    }

                    //次の行へ
                    insertRowIndex++;
                }
            }
        }

        void チェック反映()
        {
            this.grp4ParamCoef.Enabled = this.ckb4paramUse.Checked;

        }
        private void ckb4paramUse_CheckedChanged( object sender, EventArgs e )
        {
            this.チェック反映();
        }

        private void ckbUseX_CheckedChanged( object sender, EventArgs e )
        {
            ckbUseY.Checked = !ckbUseX.Checked;
        }

        private void ckbUseY_CheckedChanged( object sender, EventArgs e )
        {
            ckbUseX.Checked = !ckbUseY.Checked;
        }

        private void tabPage2_Click( object sender, EventArgs e )
        {

        }

        private void gesturePanel4_PaintClient( object sender, PaintEventArgs e )
        {

        }
    }
}
