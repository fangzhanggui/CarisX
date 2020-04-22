using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.Common.Calculator
{
	/// <summary>
	/// 両対数計算処理クラス
	/// </summary>
	/// <remarks>
	/// 両対数計算処理クラスです。
	/// </remarks>
	public class DoubleLogarithmicMethod : ICalcMethod
	{
		#region [外部関数]

		#endregion

		#region [定数定義]

		/// <summary>
		/// 両対数計算次数定義
		/// </summary>
		public enum Dimention
		{
			/// <summary>
			/// 一次
			/// </summary>
			One = 1,

			/// <summary>
			/// 二次
			/// </summary>
			Two = 2
		}

		/// <summary>
        /// 計算モード設定Calculation mode setting
		/// </summary>
		public enum CalcMode
		{
			/// <summary>
			/// 実数値 实数值
			/// </summary>
			Linear,
			/// <summary>
			/// 対数値
			/// </summary>
			Log
		}

		#endregion

		#region [クラス変数定義]

		#endregion

		#region [インスタンス変数定義]

		/// <summary>
		/// 動作次数
		/// </summary>
		private Dimention dimention = Dimention.One;

		/// <summary>
		/// 係数A
		/// </summary>
		private Double coefA;

		/// <summary>
		/// 係数B
		/// </summary>
		private Double coefB;

		/// <summary>
		/// 係数C
		/// </summary>
		private Double coefC;
		
		/// <summary>
		/// 計算エラー
		/// </summary>
		private CalcuErr calcuError;

		/// <summary>
        /// 最大点数
		/// </summary>
		private Int32 maxPoint;

		/// <summary>
		/// 濃度値
		/// </summary>
		private Double[] concs;

		/// <summary>
		/// カウント値
		/// </summary>
		private Double[] counts;

		/// <summary>
		/// 点的个数
		/// </summary>
		private Int32 numOfPoint;

		/// <summary>
		/// 算出モード
		/// </summary>
		private CalcMode calcMode = CalcMode.Linear;

		#endregion

		#region [コンストラクタ/デストラクタ]

		/// <summary>
        /// Constructor
		/// </summary>
		/// <param name="dimention">次数</param>
		public DoubleLogarithmicMethod( Dimention dimention )
		{
			this.dimention = dimention;
			this.calcuError = CalcuErr.NoError;
		}
		
		#endregion

		#region [プロパティ]

		/// <summary>
        /// Calculation mode
		/// </summary>
		/// <remarks>
		/// この設定により、GetX及びGetYの出力値が実数/対数で切替わります。
		/// </remarks>
		public CalcMode CalcModeSettings
		{
			get
			{
				return this.calcMode;
			}
			set
			{
				this.calcMode = value;
			}
		}


		/// <summary>
		/// 係数A
		/// </summary>
		public Double CoefA
		{
			get
			{
				return coefA;
			}
			set
			{
				coefA = value;
			}
		}

		/// <summary>
		/// 係数B
		/// </summary>
		public Double CoefB
		{
			get
			{
				return coefB;
			}
			set
			{
				coefB = value;
			}
		}

		/// <summary>
		/// 係数C
		/// </summary>
		public Double CoefC
		{
			get
			{
				return coefC;
			}
			set
			{
				coefC = value;
			}
		}
		#endregion

		#region [publicメソッド]

		/// <summary>
		/// 計算エラー取得
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

		/// <summary>
		/// X値(濃度値)の取得
		/// </summary>
		/// <remarks>
		/// 濃度値を二分法から算出し、結果を返します。
		/// </remarks>
		/// <param name="yPoint">Y値(カウント値)</param>
		/// <returns>X値(濃度値)</returns>
		public Double GetX( Double yPoint )
		{
			Double calcYPoint = yPoint;
			if ( calcMode == CalcMode.Linear )
			{
				calcYPoint = Math.Log10( yPoint );
			}

			Double left_conc;
			Double right_conc;
			Double left_count;
			Double right_count;
			Int32 trend = 1;          // 1:右上がり 0:右下がり 1：右上升 0：右下降

            // determine the trend of the curve
			if ( this.counts[0] < this.counts[this.numOfPoint - 1] )
			{
				trend = 1;
			}
			else
			{
				trend = 0;
			}

			// 検量線の始点、終点から二分法を開始
			left_conc = this.concs[0];
			left_count = this.counts[0];
			right_conc = this.concs[this.numOfPoint - 1];
			right_count = this.counts[this.numOfPoint - 1];

			Double work_conc = 0;
			Double work_count = 0;
			//Double eps = Math.Log10( 10000 );
			Double eps = 10000;
            // 濃度算出(収束ループ)Concentration calculation (convergence loop)
			Int32 converge_cnt = 0;
			//Double logEps = Math.Log10( ConvergeConst.Eps );

			if ( trend == 1 )
			{				 
                // return the Low error if count is less than left_point
				if ( calcYPoint < left_count )
				{
					this.calcuError = CalcuErr.Low;
					right_conc = this.concs[0];
					right_count = this.counts[0];
					left_conc = 0;
					left_count = getYLog( left_conc );

				}

                // return the High error if the count is greater than right_point
				if ( calcYPoint > right_count )
				{
					this.calcuError = CalcuErr.High;

					left_conc = this.concs[this.numOfPoint - 1];
					left_count = this.counts[this.numOfPoint - 1];
					right_conc = left_conc * 100;
					right_count = getYLog( right_conc );
				}


				do
				{
					work_conc = ( left_conc + right_conc ) / 2;
					work_count = getYLog( work_conc );
					this.calcuError = this.GetError();

					if ( this.calcuError != CalcuErr.NoError && this.calcuError != CalcuErr.High && this.calcuError != CalcuErr.Low )
					{
						return 0.0;
					}

					if ( work_count < calcYPoint )
					{
						left_conc = work_conc;
						left_count = work_count;
					}
					else
					{
						right_conc = work_conc;
						right_count = work_count;
					}

					eps = Math.Abs( left_count - right_count );
					if ( converge_cnt++ > ConvergeConst.MaxCount )
					{
						this.calcuError = CalcuErr.NotConverge;
						break;
					}

//				} while ( eps > logEps );
				} while ( eps > ConvergeConst.Eps );
			}
			else
			{
                //return the High error if the count is greater than left_point
				if ( calcYPoint > left_count )
				{
					this.calcuError = CalcuErr.High;

					right_conc = this.concs[0];
					right_count = this.counts[0];
					left_conc = 0;
					left_count = getYLog( left_conc );
				}

                // return the Low error if count is less than right_point
				if ( calcYPoint < right_count )
				{
					calcuError = CalcuErr.Low;

					left_conc = this.concs[this.numOfPoint - 1];
					left_count = this.counts[this.numOfPoint - 1];
					right_conc = left_conc * 100;
					right_count = getYLog( right_conc );
				}

				do
				{
					work_conc = ( left_conc + right_conc ) / 2;
					work_count = getYLog( work_conc );
					this.calcuError = this.GetError();
					if ( this.calcuError != CalcuErr.NoError )
					{
						return 0.0;
					}

					if ( work_count > calcYPoint )
					{
						left_conc = work_conc;
						left_count = work_count;
					}
					else
					{
						right_conc = work_conc;
						right_count = work_count;
					}

					eps = Math.Abs( left_count - right_count );
					if ( converge_cnt++ > ConvergeConst.MaxCount )
					{
						this.calcuError = CalcuErr.NotConverge;
						break;
					}

				//} while ( eps > logEps );
				} while ( eps > ConvergeConst.Eps );
			}

			if ( this.calcMode == DoubleLogarithmicMethod.CalcMode.Linear )
			{
				// 得られる値は対数値の為、Pos(10,x)とする）
                //For the logarithmic value, the value obtained is the Pos (10, x))
				work_conc = Math.Pow( 10, work_conc );
			}

			return work_conc;
		}

		/// <summary>
		/// Y値(カウント値)の取得
		/// </summary>
		/// <remarks>
		/// 引数値より算出したY値(カウント値)を返します。
		/// </remarks>
		/// <param name="xPoint">X値(濃度値)</param>
		/// <returns>Y値(カウント値)</returns>
		public Double GetY( Double xPoint )
		{
			Double calcXpoint = xPoint;

			if ( this.calcMode == CalcMode.Linear )
			{
				calcXpoint = Math.Log10( xPoint );
			}

			this.calcuError = CalcuErr.NoError;
			//Double[] coef = new Double[4];

			// 濃度が1ﾎﾟｲﾝﾄ目よりも小さい場合はLowエラー発生
            //Low error occurred when the concentration is less than the first point
			if ( calcXpoint < this.concs[0] )
			{
				this.calcuError = CalcuErr.Low;
			}

			// 濃度がnﾎﾟｲﾝﾄ目よりも大きい場合はHighエラー発生
            //High error occurred when the concentration is greater than that of the n-th point
			if ( calcXpoint > this.concs[this.numOfPoint - 1] )
			{
				this.calcuError = CalcuErr.High;
			}


            // ｶｳﾝﾄを算出The calculated count
			Double ret_count = 0.0d;
			switch ( this.dimention )
			{
			case Dimention.One:
				ret_count = this.coefA * calcXpoint + this.coefB;
				break;
			case Dimention.Two:
				ret_count = this.coefA * Math.Pow( calcXpoint, 2 ) + this.coefB * calcXpoint + this.coefC;
				break;
			default:
				break;
			}

			if ( this.calcMode == DoubleLogarithmicMethod.CalcMode.Linear )
			{
				ret_count = Math.Pow( 10, ret_count );
			}

			return ret_count;
		}

		/// <summary>
		/// Y値(カウント値)の取得
		/// </summary>
		/// <remarks>
		/// 引数値より算出したY値(カウント値)を返します。
		/// </remarks>
		/// <param name="xPoint">X値(濃度値)</param>
		/// <returns>Y値(カウント値)</returns>
		private Double getYLog( Double xPoint )
		{
			this.calcuError = CalcuErr.NoError;
			//Double[] coef = new Double[4];

			// 濃度が1ﾎﾟｲﾝﾄ目よりも小さい場合はLowエラー発生
			if ( xPoint < this.concs[0] )
			{
				this.calcuError = CalcuErr.Low;
			}

			// 濃度がnﾎﾟｲﾝﾄ目よりも大きい場合はHighエラー発生
			if ( xPoint > this.concs[this.numOfPoint - 1] )
			{
				this.calcuError = CalcuErr.High;
			}

            // ｶｳﾝﾄを算出The calculated count
			Double ret_count = 0.0d;
			switch ( this.dimention )
			{
			case Dimention.One:
				ret_count = this.coefA * xPoint + this.coefB;
				break;
			case Dimention.Two:
				ret_count = this.coefA * Math.Pow( xPoint, 2 ) + this.coefB * xPoint + this.coefC;
				break;
			default:
				break;
			}
			
			return ret_count;
		}

		/// <summary>
        /// 係数計算
        /// </summary>
        /// <remarks>
        /// 係数計算を行います。
        /// </remarks>
        /// <param name="concs">濃度値</param>
        /// <param name="counts">カウント値</param>
        /// <param name="num_of_point">ポイント数</param>
		public void CalcuCoef( Double[] concs, Double[] counts, Int32 num_of_point )
		{
			Int32 dimention = (Int32)this.dimention;
			//this.concs = concs;
			//this.counts = counts;
			this.numOfPoint = num_of_point;
			this.maxPoint = num_of_point;

			// 係数計算時、対数に変換する
			//double[] calcConcs = concs.Select( ( v ) => Math.Log10( v ) ).ToArray();
			//double[] calcCounts = counts.Select( ( v ) => Math.Log10( v ) ).ToArray();
			this.concs = concs.Select( ( v ) => Math.Log10( v ) ).ToArray();
			this.counts = counts.Select( ( v ) => Math.Log10( v ) ).ToArray();

			double[] coef = new double[dimention + 1];
			Boolean result = this.LeastSq( num_of_point, this.concs, this.counts, (Int32)this.dimention, ref coef );
			if ( result )
			{
				this.coefA = coef[dimention];
				this.coefB = coef[dimention - 1];
				if ( dimention == 2 )
				{
					this.coefC = coef[dimention - 2];
				}
			}
			else
			{
				this.calcuError = CalcuErr.Other;
			}
		}
		#endregion

		#region [protectedメソッド]

		#endregion

		#region [privateメソッド]

		/// <summary>
		/// 両対数係数計算処理
		/// </summary>
		/// <remarks>
		/// 両対数の係数計算を行います（大塚電子様提供）
		/// </remarks>
		/// <param name="n">点数</param>
		/// <param name="x">X座標</param>
		/// <param name="y">Y座標</param>
		/// <param name="m">次数</param>
		/// <param name="c">計算結果係数</param>
		/// <returns>true:成功 false:失敗</returns>
		private bool LeastSq( int n, double[] x, double[] y, int m, ref double[] c )
		{
			double[,] a = new double[21, 22];
			double[] w = new double[42];

			double w1;
			double w2;
			double w3;
			double pivot;
			double aik;

			int mp1;
			int mp2;
			int m2;
			int ii;
			int jj;
			int kk;
			int idx;

			if ( m == 0 )
			{
				c[0] = 0.0f;
				for ( int i = 0; i < n; i++ )
					c[0] += y[i];

				if ( n != 0 )
					c[0] /= n;

				return true;
			}

			//			if( m >= n  ||  m < 1  ||  m > 20 )
			//				return false;

			if ( m >= n || m < 1 || m > 20 )
				return false;

			mp1 = m + 1;
			mp2 = m + 2;
			m2 = 2 * m;

			for ( ii = 0; ii < m2; ii++ )
			{
				w1 = 0.0;
				for ( jj = 0; jj < n; jj++ )
				{
					w2 = w3 = x[jj];
					for ( kk = 0; kk < ii; kk++ )
					{
						w2 *= w3;
					}
					w1 += w2;
				}
				w[ii] = w1;
			}

			for ( ii = 0; ii < mp1; ii++ )
			{
				for ( jj = 0; jj < mp1; jj++ )
				{
					idx = ii + jj - 1;
					if ( idx >= 0 )	// 1998/02/25 B.B. Shimak
					{
						a[ii, jj] = w[idx];
					}
				}
			}

			a[0, 0] = n;
			w1 = 0.0;
			for ( ii = 0; ii < n; ii++ )
			{
				w1 += y[ii];
			}

			a[0, mp1] = w1;
			for ( ii = 0; ii < m; ii++ )
			{
				w1 = 0.0;
				for ( jj = 0; jj < n; jj++ )
				{
					w2 = w3 = x[jj];
					for ( kk = 0; kk < ii; kk++ )
					{
						w2 *= w3;
					}
					w1 += y[jj] * w2;
				}
				a[ii + 1, mp1] = w1;
			}

			for ( kk = 0; kk < mp1; kk++ )
			{
				pivot = a[kk, kk];
				for ( jj = kk; jj < mp2; jj++ )
				{
					a[kk, jj] /= pivot;
				}
				for ( ii = 0; ii < mp1; ii++ )
				{
					if ( ii != kk )
					{
						aik = a[ii, kk];
						for ( jj = kk; jj < mp2; jj++ )
						{
							a[ii, jj] -= aik * a[kk, jj];
						}
					}
				}
			}

			for ( ii = 0; ii < mp1; ii++ )
			{
				c[ii] = (float)a[ii, mp1];
			}

			return true;
		}
		#endregion
	}
}
