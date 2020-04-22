using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.Const;

namespace Oelco.Common.Calculator
{
    /// <summary>
    /// 計算
    /// </summary>
    /// <remarks>
    /// 濃度および検量線の計算クラスです。
    /// </remarks>
    public class ConcentrationCalculator
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 検量線ポイント
        /// </summary>
        private ItemPoint[] calibPoint;

        /// <summary>
        /// 計算エラー
        /// </summary>
        private CalcuErr calcuError;
                
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 濃度配列の取得
        /// </summary>
        private Double[] concs
        {
            get
            {
                return this.calibPoint.Select( ( point ) => point.xPos ).ToArray();
            }
        }

        /// <summary>
        /// カウント配列の取得
        /// </summary>
        private Double[] counts
        {
            get
            {
                return this.calibPoint.Select( ( point ) => point.yPos ).ToArray();
            }
        }
        
        /// <summary>
        /// 検量線ポイント数の取得
        /// </summary>
        private Int32 numOfCalib
        {
            get
            {
                return this.calibPoint.Count();
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// キャリブレーションポイント設定
        /// </summary>
        /// <remarks>
        /// キャリブレーションポイントデータを設定します。
        /// </remarks>
        /// <param name="points">ポイントデータ</param>
        public void SetCalcData( ItemPoint[] points )
        {
            this.calibPoint = points.Select( ( point ) => (ItemPoint)point.Clone() ).ToArray();
        }

		/// <summary>
		/// グラフ用ポイント取得(両対数2次)
		/// </summary>
		/// <remarks>
		/// グラフ用のポイントを取得します。
		/// この関数を対数値として使用した場合、以後の測定座標は対数扱いとなります。
		/// </remarks>
		/// <returns>グラフ用ポイント取得</returns>
		public ItemPoint[] GetGraphPointDoubleLogTwo(DoubleLogarithmicMethod.CalcMode calcMode )
		{
			DoubleLogarithmicMethod method = new DoubleLogarithmicMethod( DoubleLogarithmicMethod.Dimention.Two );
			method.CalcModeSettings = calcMode;
			method.CalcuCoef( this.concs, this.counts, this.numOfCalib );

			// Log値の場合、入力濃度/カウントを対数変換する。
			if ( calcMode == DoubleLogarithmicMethod.CalcMode.Log )
			{
				this.calibPoint = this.calibPoint.Select( ( v ) => new ItemPoint( Math.Log10( v.xPos ), Math.Log10( v.yPos ) ) ).ToArray();
			}

			return this.GetGraphPoint( method );
		}

		/// <summary>
		/// グラフ用ポイント取得(両対数1次)
		/// </summary>
		/// <remarks>
		/// グラフ用のポイントを取得します。
		/// この関数を対数値として使用した場合、以後の測定座標は対数扱いとなります。
		/// </remarks>
		/// <returns>グラフ用ポイント取得</returns>
		public ItemPoint[] GetGraphPointDoubleLogOne( DoubleLogarithmicMethod.CalcMode calcMode )
		{
			DoubleLogarithmicMethod method = new DoubleLogarithmicMethod( DoubleLogarithmicMethod.Dimention.One );
			method.CalcModeSettings = calcMode;
			method.CalcuCoef( this.concs, this.counts, this.numOfCalib );

			// Log値の場合、入力濃度/カウントを対数変換する。
			if ( calcMode == DoubleLogarithmicMethod.CalcMode.Log )
			{
				this.calibPoint = this.calibPoint.Select( ( v ) => new ItemPoint( Math.Log10( v.xPos ), Math.Log10( v.yPos ) ) ).ToArray();
			}

			return this.GetGraphPoint( method );
		}

        /// <summary>
        /// グラフ用ポイント取得(Spline)
        /// </summary>
        /// <remarks>
        /// グラフ用のポイントを取得します。
        /// </remarks>
        /// <returns>グラフ用ポイント取得</returns>
        public ItemPoint[] GetGraphPointSpline()
        {
            SplineMethod method = new SplineMethod();
            method.CalcuCoef( this.concs, this.counts, this.numOfCalib );

            return this.GetGraphPoint( method );
        }

        /// <summary>
        /// グラフ用ポイント取得(Logit-Log)
        /// </summary>
        /// <remarks>
        /// グラフ用のポイントを取得します。
        /// </remarks>
        /// <returns>グラフ用ポイント取得</returns>
        public ItemPoint[] GetGraphPointLogitLog( Double coefA, Double coefB )
        {
            LogitLogMethod method = new LogitLogMethod( coefA, coefB );
           
            method.CalcuCoef(this.concs, this.counts, this.numOfCalib, false);

            double beforeCount;
            beforeCount = method.GetY(0.0);
            if (Double.IsNaN(beforeCount) || Double.IsInfinity(beforeCount))//最適化オプションにより Nan->Infinity
            {
                method.CalcuCoef(this.concs, this.counts, this.numOfCalib, true);
            }
            

            return this.GetGraphPoint( method );
        }
        
        /// <summary>
        /// グラフ用ポイント取得(4parameter)
        /// </summary>
        /// <remarks>
        /// グラフ用のポイントを取得します。
        /// </remarks>
        /// <returns>グラフ用ポイント取得</returns>
        public ItemPoint[] GetGraphPointFourParameter()
        {
            // 生成
            FourParameterMethod method = new FourParameterMethod( this.numOfCalib );

            // データを設定
            method.SetData( this.concs, this.counts, this.numOfCalib );

            // 計算
            method.Calc4Parameter();

            return GetGraphPoint( method );
        }

        /// <summary>
        /// 4パラメータ計算
        /// </summary>
        /// <remarks>
        /// 4パラメータの計算を行います。
        /// </remarks>
        /// <param name="paramA">パラメータA</param>
        /// <param name="paramB">パラメータB</param>
        /// <param name="paramC">パラメータC</param>
        /// <param name="paramD">パラメータD</param>
        /// <returns>計算結果</returns>
        public ItemPoint[] GetGraphPointWithCoefFourParameter(FourPTypeStruct Fparameter, Double? paramA, Double? paramB, Double? paramC, Double? paramD)
        {
            if ( ( paramA == null ) && ( paramB == null ) && ( paramC == null ) && ( paramD == null ) )
            {
                // 全パラメータがNullできた場合、これはマスターカーブの場合のみありえる。（マスターカーブには分析項目に対してではなく、試薬に対してのデータである為）
                var culced = this.GetGraphPointWithCoefFourParameter(Fparameter);
                return culced.Item1;
            }

            // 生成
            FourParameterMethod method = new FourParameterMethod( this.numOfCalib, paramA.Value, paramB.Value, paramC.Value, paramD.Value );

            // データを設定
            method.SetData( this.concs, this.counts, this.numOfCalib );

            return this.GetGraphPoint( method );
        }

        /// <summary>
        /// 4パラメータ計算
        /// </summary>
        /// <remarks>
        /// 4パラメータの計算を行います。
        /// </remarks>
        /// <returns>計算結果,パラメータA,パラメータB,パラメータC,パラメータD</returns>
        public Tuple<ItemPoint[], Double, Double, Double, Double> GetGraphPointWithCoefFourParameter(FourPTypeStruct Fparameter)
        {
            // 生成
            FourParameterMethod method = new FourParameterMethod( numOfCalib );
            method.FourPType = Fparameter.PType;
            if (Fparameter.PType == FourPType.str1Y_K)
            {
                method.ValueK = Fparameter.ValueK;
            }

            // データを設定
            method.SetData( this.concs, this.counts, this.numOfCalib );

            // 計算
            method.Calc4Parameter();

            return new Tuple<ItemPoint[], Double, Double, Double, Double>( this.GetGraphPoint( method ), method.CoefA, method.CoefB, method.CoefC, method.CoefD );
        }

        /// <summary>
        /// グラフポイントの取得
        /// </summary>
        /// <remarks>
        /// グラフポイントを返します。
        /// </remarks>
        /// <param name="method">計算処理インターフェース</param>
        /// <returns>グラフ用ポイントデータ</returns>
        private ItemPoint[] GetGraphPoint( ICalcMethod method )
        {
            Int32 numOfDiv = 200;

            ItemPoint[] ret_points = new ItemPoint[numOfDiv + 1];
            Double min;
            Double max;
            Double count;
            Double conc;
            Int32 i = 0;

            // ここでグラフのﾎﾟｲﾝﾄを返す
            /////////////////////////////////////////////////////////////////////////////////
            // 濃度(X)からｶｳﾝﾄ(Y)を求めてグラフを書く
            /////////////////////////////////////////////////////////////////////////////////

            min = this.calibPoint.Min( v => v.xPos );
            max = this.calibPoint.Max( v => v.xPos );
            Double div = ( max - min ) / numOfDiv;

            for ( Int32 j = 0; j < numOfDiv + 1; j++ )
            {
                conc = min + div * j;
                count = method.GetY( conc );
                ret_points[i++] = new ItemPoint( count, conc );
            }

            return ret_points;
        }

        /// <summary>
        /// 計算エラーの取得
        /// </summary>
        /// <remarks>
        /// 計算エラーを返します。
        /// </remarks>
        /// <returns>計算エラー</returns>
        public CalcuErr GetError()
        {
            CalcuErr ret_err = this.calcuError;
            this.calcuError = CalcuErr.NoError;
            return ret_err;
        }

        #endregion

    }

    /// <summary>
    /// 濃度計算処理インターフェース
    /// </summary>
    /// <remarks>
    /// 濃度計算処理のインターフェースです。
    /// </remarks>
    public interface ICalcMethod
    {
        #region [publicメソッド]

        /// <summary>
        /// X値取得
        /// </summary>
        /// <remarks>
        /// X値を返します。
        /// </remarks>
        /// <param name="yPoint">Y値</param>
        /// <returns>X値</returns>
        Double GetX( Double yPoint );

        /// <summary>
        /// Y値取得
        /// </summary>
        /// <remarks>
        /// Y値を返します。
        /// </remarks>
        /// <param name="yPoint">X値</param>
        /// <returns>Y値</returns>
        Double GetY( Double xPoint );

        /// <summary>
        /// 計算エラーの取得
        /// </summary>
        /// <remarks>
        /// 計算エラーを返します。
        /// </remarks>
        /// <returns>計算エラー</returns>
        CalcuErr GetError();

        #endregion

    }
}
