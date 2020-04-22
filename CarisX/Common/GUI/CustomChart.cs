using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Infragistics.UltraChart.Core.Primitives;
using System.Drawing;
using Infragistics.UltraChart.Core;
using System.Data;

using System.Threading.Tasks;
using Infragistics.UltraChart.Resources.Appearance;
using Infragistics.UltraChart.Shared.Events;
using Infragistics.Win.UltraWinChart;
using Oelco.Common.Const;

namespace Oelco.Common.GUI
{
    /// <summary>
    /// カスタムチャート(Y軸帯表示)
    /// </summary>
    public partial class CustomChart : UltraChart
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// Y軸帯表示コレクション
        /// </summary>
        private YAxisZoneCollection yAxisZoneItems = new YAxisZoneCollection();

        /// <summary>
        /// Y軸線表示コレクション
        /// </summary>
        private YAxisLineCollection yAxisLineItems = new YAxisLineCollection();

        /// <summary>
        /// チャート無効データ時のメッセージ
        /// </summary>
        private String invalidDataMessage = String.Empty;

        /// <summary>
        /// 無効データ時の表示設定
        /// </summary>
        private new ChartDataInvalidEventHandler invalidData = ( sender, e ) =>
            {
                e.LabelStyle.HorizontalAlign = StringAlignment.Center;
                e.LabelStyle.VerticalAlign = StringAlignment.Center;
                e.LabelStyle.FontSizeBestFit = false;
                e.LabelStyle.FontColor = GlobalConst.CHART_INVALIDATED_TEXT_COLOR;
                e.Text = Oelco.Common.Properties.Resources.STRING_CHART_001;
            };

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CustomChart()
            : base()
        {
            this.FillSceneGraph += new FillSceneGraphEventHandler( CustomUChart_FillSceneGraph );
            this.InvalidDataReceived += this.invalidData;
        }

        #endregion

        #region [プロパティ]

        ///// <summary>
        ///// チャート無効データ時のメッセージの取得、設定
        ///// </summary>
        //[DefaultValue("")]
        //public String InvalidDataMessage
        //{
        //    get
        //    {
        //        return this.invalidDataMessage;
        //    }
        //    set
        //    {
        //        if ( this.invalidData != null )
        //        {
        //            this.InvalidDataReceived -= this.invalidData;
        //        }

        //        if ( !String.IsNullOrEmpty( value ) )
        //        {
        //            this.InvalidDataReceived += this.invalidData = ( sender, e ) =>
        //            {
        //                e.LabelStyle.HorizontalAlign = StringAlignment.Center;
        //                e.LabelStyle.VerticalAlign = StringAlignment.Center;
        //                e.LabelStyle.FontSizeBestFit = false;
        //                e.LabelStyle.FontColor = GlobalConst.CHART_INVALIDATED_TEXT_COLOR;
        //                e.Text = value;
        //            };
        //        }
        //        else
        //        {
        //            this.invalidData = null;
        //        }
        //    }
        //}

        /// <summary>
        /// Y軸帯表示コレクションの取得
        /// </summary>
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
        public YAxisZoneCollection YAxisZoneItems
        {
            get
            {
                return this.yAxisZoneItems;
            }
        }
        /// <summary>
        /// Y軸線表示コレクションの取得
        /// </summary>
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
        public YAxisLineCollection YAxisLineItems
        {
            get
            {
                return this.yAxisLineItems;
            }
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// チャートプリミティブアクセスイベント
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void CustomUChart_FillSceneGraph( object sender, FillSceneGraphEventArgs e )
        {
            this.YAxisZoneItems.YAxisZoneSettingDisplay( e );
            this.YAxisLineItems.YAxisLineSettingDisplay( e );
        }

        #endregion
    }

    /// <summary>
    /// Y軸帯表示コレクションクラス
    /// </summary>
    [ListBindable( false )]
    public class YAxisZoneCollection : List<YAxisZone>
    {
        #region [publicメソッド]

        /// <summary>
        /// Y軸帯表示の表示設定
        /// </summary>
        /// <param name="e">イベントデータ</param>
        public void YAxisZoneSettingDisplay( FillSceneGraphEventArgs e )
        {
            // 座標軸を取得
            IAdvanceAxis x = (IAdvanceAxis)e.Grid["X"];
            IAdvanceAxis y = (IAdvanceAxis)e.Grid["Y"];

            if ( x == null || y == null )
            {
                return;
            }

            Parallel.ForEach( this, ( yAxisZone ) =>
            {
                yAxisZone.SettingMap( x, y );
            } );

            // 設定を画面に反映
            foreach ( Primitive primitive in e.SceneGraph.OfType<Primitive>().Reverse() )
            {
                if ( primitive.GetType() == typeof( Line ) || primitive.GetType() == typeof( PointSet ) )
                {
                    Int32 index = e.SceneGraph.IndexOf( primitive );
                    this.ForEach( ( zone ) => e.SceneGraph.Insert( index++, zone ) );
                    break;
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Y軸帯表示クラス
    /// </summary>
    public class YAxisZone
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 帯表示用ボックス
        /// </summary>
        private Box box;
        /// <summary>
        /// 帯表示領域指定矩形
        /// </summary>
        private Rectangle rect = new Rectangle();

        /// <summary>
        /// このプリミティブの描画に使用する描画要素。
        /// </summary>
        private PaintElement pe = new PaintElement();

        /// <summary>
        /// Y軸(帯幅ポイント1)
        /// </summary>
        private Double value1 = new Double();

        /// <summary>
        /// Y軸(帯幅ポイント2)
        /// </summary>
        private Double value2 = new Double();

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public YAxisZone()
        {
            // 既定値の設定
            this.pe.Fill = Color.Pink;
            this.pe.FillOpacity = 128;
            this.pe.Stroke = Color.Transparent;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 描画エリア色の取得、設定
        /// </summary>
        public Color FillColor
        {
            get
            {
                return this.pe.Fill;
            }
            set
            {
                this.pe.Fill = value;
            }
        }

        /// <summary>
        /// 描画エリア枠線の取得、設定
        /// </summary>
        public Color StrokeColor
        {
            get
            {
                return this.pe.Stroke;
            }
            set
            {
                this.pe.Stroke = value;
            }
        }

        /// <summary>
        /// 描画エリア不透明度
        /// </summary>
        public byte Opacity
        {
            get
            {
                return this.pe.FillOpacity;
            }
            set
            {
                this.pe.FillOpacity = value;
            }
        }

        /// <summary>
        /// Y軸(帯幅ポイント1)の取得、設定
        /// </summary>
        public Double Value1
        {
            get
            {
                return this.value1;
            }
            set
            {
                this.value1 = value;
            }
        }

        /// <summary>
        /// Y軸(帯幅ポイント2)の取得、設定
        /// </summary>
        public Double Value2
        {
            get
            {
                return this.value2;
            }
            set
            {
                this.value2 = value;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 暗黙的型変換(Primitiveへ)
        /// </summary>
        /// <remarks>
        /// 暗黙的型変換を行います。
        /// </remarks>
        /// <param name="zone">型変換対象Y軸帯表示</param>
        /// <returns>Primitive</returns>
        public static implicit operator Primitive( YAxisZone zone )
        {
            return zone.box;
        }

        /// <summary>
        /// Y軸帯グラフ表示設定
        /// </summary>
        /// <remarks>
        /// Y軸帯グラフの表示設定を行います。
        /// </remarks>
        /// <param name="x">グラフX軸</param>
        /// <param name="y">グラフY軸</param>
        public void SettingMap( IAdvanceAxis x, IAdvanceAxis y )
        {
            if ( x != null && y != null )
            {
                Double max = y.Map(Math.Max( this.value1, this.value2 ));
                Double min = y.Map(Math.Min( this.value1, this.value2 ));

                if ( max < y.MapMaximum )
                {
                    max = y.MapMaximum;
                }
                if ( min > y.MapMinimum )
                {
                    min = y.MapMinimum;
                }

                this.rect.X = (Int32)x.MapMinimum;
                this.rect.Y = (Int32)max;
                this.rect.Width = (Int32)x.MapRange;
                this.rect.Height = (Int32)(min-max);

                if ( ( this.rect.Y + this.rect.Height ) > y.MapMinimum )
                {
                    this.rect.Height = (Int32)y.MapMinimum - this.rect.Y;
                }
            }

            this.box = new Box( this.rect );
            this.box.PE = this.pe;
        }

        #endregion
    }

    /// <summary>
    /// Y軸線表示コレクションクラス
    /// </summary>
    [ListBindable( false )]
    public class YAxisLineCollection : List<YAxisLine>
    {
        #region [publicメソッド]

        /// <summary>
        /// Y軸線表示の表示設定
        /// </summary>
        /// <remarks>
        /// Y軸線表示の表示設定を行います。
        /// </remarks>
        /// <param name="e">イベントデータ</param>
        public void YAxisLineSettingDisplay( FillSceneGraphEventArgs e )
        {
            // 座標軸を取得
            IAdvanceAxis x = (IAdvanceAxis)e.Grid["X"];
            IAdvanceAxis y = (IAdvanceAxis)e.Grid["Y"];

            if ( x == null || y == null )
            {
                return;
            }

            Parallel.ForEach( this, ( yAxisLine ) =>
            {
                yAxisLine.SettingMap( x, y );
            } );

            // 設定を画面に反映
            foreach ( Primitive primitive in e.SceneGraph.OfType<Primitive>().Reverse() )
            {
                if ( primitive.GetType() == typeof( Line ) || primitive.GetType() == typeof( PointSet ) )
                {
                    Int32 index = e.SceneGraph.IndexOf( primitive );
                    this.ForEach( ( line ) => e.SceneGraph.Insert( index++, line ) );
                    break;
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// Y軸線表示クラス
    /// </summary>
    public class YAxisLine
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 帯表示用ボックス
        /// </summary>
        private Line line;

        /// <summary>
        /// このプリミティブの描画に使用する描画要素。
        /// </summary>
        private PaintElement pe = new PaintElement();

        /// <summary>
        /// Y軸(帯幅ポイント)
        /// </summary>
        private Double value = new Double();



        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public YAxisLine()
        {
            // 規定値の設定
            this.pe.StrokeWidth = 2;
            this.pe.Stroke = Color.Red;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 描画線の太さの取得、設定
        /// </summary>
        public Int32 StrokeWidth
        {
            get
            {
                return this.pe.StrokeWidth;
            }
            set
            {
                this.pe.StrokeWidth = value;
            }
        }

        /// <summary>
        /// 描画線の取得、設定
        /// </summary>
        public Color StrokeColor
        {
            get
            {
                return this.pe.Stroke;
            }
            set
            {
                this.pe.Stroke = value;
            }
        }

        /// <summary>
        /// 描画線不透明度
        /// </summary>
        public byte Opacity
        {
            get
            {
                return this.pe.StrokeOpacity;
            }
            set
            {
                this.pe.StrokeOpacity = value;
            }
        }

        /// <summary>
        /// Y軸ポイントの取得、設定
        /// </summary>
        public Double Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 暗黙的型変換(Primitiveへ)
        /// </summary>
        /// <remarks>
        /// 暗黙的型変換を行います。
        /// </remarks>
        /// <param name="line">型変換対象Y軸帯表示</param>
        /// <returns>Primitive</returns>
        public static implicit operator Primitive( YAxisLine line )
        {
            return line.line;
        }

        /// <summary>
        /// Y軸線グラフ表示設定
        /// </summary>
        /// <remarks>
        /// Y軸線グラフの表示設定を行います。
        /// </remarks>
        /// <param name="x">グラフX軸</param>
        /// <param name="y">グラフY軸</param>
        public void SettingMap( IAdvanceAxis x, IAdvanceAxis y )
        {
            if ( x != null && y != null )
            {
                if ( this.value <= (Double)y.WindowMaximum && this.value >= (Double)y.WindowMinimum )
                {
                    this.line = new Line( new Point( (Int32)x.MapMinimum, (Int32)y.Map( this.value ) ), new Point( (Int32)x.MapMaximum, (Int32)y.Map( this.value ) ) );
                }
                else
                {
                    this.line = new Line();
                }
            }
            else
            {
                this.line = new Line();
            }
            
            this.line.PE = this.pe;
        }

        #endregion
    }
}
