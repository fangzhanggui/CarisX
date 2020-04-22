using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.Common.Calculator
{
    /// <summary>
    /// Logit-Log計算処理クラス
    /// </summary>
    public class LogitLogMethod : ICalcMethod
    {
        #region [インスタンス変数定義]
        
        /// <summary>
        /// 係数
        /// </summary>
        private Double[] coef;

        /// <summary>
        /// Log変換係数A
        /// </summary>
        private Double coefA;

        /// <summary>
        /// Log変換係数B
        /// </summary>
        private Double coefB;

        /// <summary>
        /// カウント値
        /// </summary>
        private Double[] counts;

        /// <summary>
        /// 濃度値
        /// </summary>
        private Double[] concs;

        /// <summary>
        /// ポイント数
        /// </summary>
        private Int32 numOfPoint;

        /// <summary>
        /// 右下がり
        /// </summary>
        private Boolean lowerRight;

        /// <summary>
        /// 計算エラー
        /// </summary>
        private CalcuErr calcuError;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="coef_a">Log変換係数A</param>
        /// <param name="coef_b">Log変換係数B</param>
        public LogitLogMethod( Double coef_a, Double coef_b )
        {
            this.coefA = coef_a;       // Log変換係数A
            this.coefB = coef_b;       // Log変換係数B
            this.coef = new Double[4]; // 係数
            this.numOfPoint = 0;
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// Log変換係数のセット
        /// </summary>
        /// <remarks>
        /// Log変換係数に引数値を設定します。
        /// </remarks>
        /// <param name="coef_a">Log変換係数A</param>
        /// <param name="coef_b">Log変換係数B</param>
        public void SetLogCoef( Double coef_a, Double coef_b )
        {
            this.coefA = coef_a;       // Log変換係数A
            this.coefB = coef_b;       // Log変換係数B
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

        // 濃度値を二分法から算出(Calcu)
        /// <summary>
        /// X値(濃度値)の取得
        /// </summary>
        /// <remarks>
        /// 濃度値を二分法から算出(Calcu)し、算出結果を返します。
        /// </remarks>
        /// <param name="yPoint">Y値(カウント値)</param>
        /// <returns>X値(濃度値)</returns>
        public Double GetX( Double yPoint )
        {
            // 検量線の始点、終点から二分法を開始
            Double left_conc = this.concs[0];
            Double left_count = GetY( left_conc );
            Double right_conc = this.concs[this.numOfPoint - 1];
            Double right_count = GetY( right_conc );

            // カウントがleft_pointよりも大きい場合はLowエラーを返す。（整数で比較）
            if ( yPoint > Math.Round(left_count,0,MidpointRounding.AwayFromZero))
            {
                this.calcuError = CalcuErr.Low;

                right_conc = this.concs[0];
                right_count = GetY( right_conc );
                left_conc = 0;
                left_count = GetY( left_conc );
            }

            // カウントがright_pointよりも小さい場合はHighエラーを返す。（整数で比較）
            if (yPoint < Math.Round(right_count, 0, MidpointRounding.AwayFromZero))
            {
                this.calcuError = CalcuErr.High;

                left_conc = this.concs[this.numOfPoint - 1];
                left_count = GetY( left_conc );
                right_conc = left_conc * 100;
                right_count = GetY( right_conc );
            }

            Double work_conc = 0;
            Double work_count = 0;
            Double eps = 10000;

            // 濃度算出(収束ループ)
            Int32 converge_cnt = 0;
            do
            {
                work_conc = ( left_conc + right_conc ) / 2;
                work_count = GetY( work_conc );
                this.calcuError = this.GetError();

                if ( this.calcuError != CalcuErr.NoError && this.calcuError != CalcuErr.High && this.calcuError != CalcuErr.Low )
                {
                    return 0.0d;
                }
                if ( work_count < yPoint )
                {
                    right_conc = work_conc;
                    right_count = work_count;
                }
                else
                {
                    left_conc = work_conc;
                    left_count = work_count;
                }

                eps = Math.Abs( left_count - right_count );
                if ( converge_cnt++ > ConvergeConst.MaxCount )
                {
                    this.calcuError = CalcuErr.NotConverge;
                    break;
                }

            } while ( eps > ConvergeConst.Eps );

            return work_conc;
        }

        // ｶｳﾝﾄ計算(CalcuCnt)
        /// <summary>
        /// Y値(カウント値)の取得
        /// </summary>
        /// <remarks>
        /// Y値(カウント値)を返します。
        /// </remarks>
        /// <param name="xPoint">X値(濃度値)</param>
        /// <returns>Y値(カウント値)</returns>
        public Double GetY( Double xPoint )
        {
            this.calcuError = CalcuErr.NoError;

            // 濃度が1ﾎﾟｲﾝﾄ目よりも小さい場合はLowエラーを返す。
            if ( xPoint < this.concs[0] )
            {
                this.calcuError = CalcuErr.Low;
            }

            // 濃度がnﾎﾟｲﾝﾄ目よりも大きい場合はHighエラーを返す。
            if ( xPoint > this.concs[this.numOfPoint - 1] )
            {
                this.calcuError = CalcuErr.High;
            }

            Double t = this.calcuTFromConc( xPoint );
            return ( Math.Pow( 10, t ) * 1.00000001 ) / ( 1 + Math.Pow( 10, t ) ) * this.counts.Max();
            
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
        public void CalcuCoef( Double[] concsPoint, Double[] countsPoint, Int32 num_of_point,bool ReCoefCalc )
        {
            lowerRight = false;
            Double[] concsTemp = new Double[concsPoint.Length];
            concsPoint.CopyTo(concsTemp, 0);
            if (ReCoefCalc)
            {
                //1ポイント目の値を他のポイントから引く。（1ポイント目を0にする）右下がりのみ
                double concsPointOne = concsPoint[0];
                for (int i = 0; i < concsPoint.Length; i++)
                {
                    concsPoint[i] = concsPoint[i] - concsPointOne;
                }
                
            }

            this.counts = countsPoint;
            this.concs = concsPoint;
            this.numOfPoint = num_of_point;

            //////////////////////////////////////
            // 計算準備
            //////////////////////////////////////            
            if ( counts[0] < 0 )
            {
                this.calcuError = CalcuErr.Other;        // ポイント1測定値 <= 0
                return;
            }

            var concCounts = concs.Zip( counts, ( conc, count ) => new
            {
                conc,
                count
            } );

            var logitLog = (from concCount in concCounts
                            let log = Math.Log10(((this.coefA > 0 && concCount.conc > 0) || this.coefB > 0) ? this.coefA * concCount.conc + this.coefB : this.coefB)
                            let logit = Math.Log10((concCount.count / counts.Max()) / ((Double)1.00000001 - (concCount.count / counts.Max())))
                            select new
                            {
                                logit,
                                log
                            }).Take(num_of_point);
            
            ////////////////////////////////
            //第一ステップ
            ////////////////////////////////

            var calcFirstStepData = new
            {
                sumLogitLog1 = logitLog.Sum( ( item ) => item.log * item.logit ),
                sumLogitLog2 = logitLog.Sum( ( item ) => Math.Pow( item.log, 2 ) * item.logit ),
                sumLogitLog3 = logitLog.Sum( ( item ) => Math.Pow( item.log, 3 ) * item.logit ),

                aveLogit = logitLog.Average( ( item ) => item.logit ),

                sumLog1 = logitLog.Sum( ( item ) => item.log ),
                sumLog2 = logitLog.Sum( ( item ) => Math.Pow( item.log, 2 ) ),
                sumLog3 = logitLog.Sum( ( item ) => Math.Pow( item.log, 3 ) ),
                sumLog4 = logitLog.Sum( ( item ) => Math.Pow( item.log, 4 ) ),
                sumLog5 = logitLog.Sum( ( item ) => Math.Pow( item.log, 5 ) ),
                sumLog6 = logitLog.Sum( ( item ) => Math.Pow( item.log, 6 ) ),

                aveLog1 = logitLog.Average( ( item ) => item.log ),
                aveLog2 = logitLog.Average( ( item ) => Math.Pow( item.log, 2 ) ),
                aveLog3 = logitLog.Average( ( item ) => Math.Pow( item.log, 3 ) ),
            };

            Double p1y = calcFirstStepData.sumLogitLog1 - ( calcFirstStepData.aveLogit ) * calcFirstStepData.sumLog1;
            Double p2y = calcFirstStepData.sumLogitLog2 - ( calcFirstStepData.aveLogit ) * calcFirstStepData.sumLog2;
            Double p3y = calcFirstStepData.sumLogitLog3 - ( calcFirstStepData.aveLogit ) * calcFirstStepData.sumLog3;

            Double p11 = calcFirstStepData.sumLog2 - ( calcFirstStepData.aveLog1 ) * calcFirstStepData.sumLog1;
            Double p12 = calcFirstStepData.sumLog3 - ( calcFirstStepData.aveLog2 ) * calcFirstStepData.sumLog1;
            Double p13 = calcFirstStepData.sumLog4 - ( calcFirstStepData.aveLog3 ) * calcFirstStepData.sumLog1;

            Double p22 = calcFirstStepData.sumLog4 - ( calcFirstStepData.aveLog2 ) * calcFirstStepData.sumLog2;
            Double p23 = calcFirstStepData.sumLog5 - ( calcFirstStepData.aveLog3 ) * calcFirstStepData.sumLog2;

            Double p33 = calcFirstStepData.sumLog6 - ( calcFirstStepData.aveLog3 ) * calcFirstStepData.sumLog3;

            ////////////////////////////////
            // 第二ステップ
            ////////////////////////////////
            Double q1 = ( p11 * p22 ) - ( p12 * p12 );
            Double q2 = ( p11 * p23 ) - ( p12 * p13 );
            Double q3 = ( p11 * p33 ) - ( p13 * p13 );
            Double q4 = ( q1 * q3 ) - ( q2 * q2 );

            ////////////////////////////////
            // 第三ステップ
            ////////////////////////////////
            if ( q4 == 0 )
            {
                this.calcuError = CalcuErr.DivByZero;
                return;
            }
            Double r11 = ( p11 * p22 * p33 - p11 * p23 * p23 ) / q4;
            Double r12 = ( q2 * p13 - q3 * p12 ) / q4;
            Double r13 = ( q2 * p12 - q1 * p13 ) / q4;
            Double r22 = ( q3 * p11 ) / q4;
            Double r23 = -1 * ( q2 * p11 ) / q4;
            Double r33 = ( q1 * p11 ) / q4;

            ///////////////////////////////
            // ファクタ算出
            ///////////////////////////////
            this.coef[0] = ( r13 * p1y ) + ( r23 * p2y ) + ( r33 * p3y );
            this.coef[1] = ( r12 * p1y ) + ( r22 * p2y ) + ( r23 * p3y );
            this.coef[2] = ( r11 * p1y ) + ( r12 * p2y ) + ( r13 * p3y );
            this.coef[3] = ( calcFirstStepData.aveLogit )
                - ( this.coef[0] * ( calcFirstStepData.aveLog3 ) )
                - ( this.coef[1] * ( calcFirstStepData.aveLog2 ) )
                - ( this.coef[2] * ( calcFirstStepData.aveLog1 ) );

            if (ReCoefCalc)
            {
                this.concs = concsTemp;
                lowerRight = true;
            }

            ///////////////////////////////
            // 検量線異常チェック
            ///////////////////////////////
            if ( this.coef[0] == 0 && this.coef[1] == 0 && this.coef[2] == 0 )
            {
                this.calcuError = CalcuErr.Abnormal2;
                return;
            }

            /* 乖離度チェックは必要か？ */
        }

        #endregion


     

        #region [privateメソッド]

        /// <summary>
        /// 濃度値からTを算出
        /// </summary>
        /// <remarks>
        /// 濃度値からTを算出します。
        /// </remarks>
        /// <param name="conc">濃度値</param>
        /// <returns>T</returns>
        private Double calcuTFromConc( Double conc )
        {
            // 1ポイント目の濃度を引く　右下がりのみ
            if (lowerRight)
            {
                //誤差対策
                if (Math.Round(conc - concs[0], 8, MidpointRounding.AwayFromZero) > 0.0)
                {
                    conc = conc - concs[0];
                }
                else
                {
                    conc = 0.0;
                }
            }


            Double s = Math.Log10( this.coefA * conc + this.coefB );
            return this.coef[0] * Math.Pow( s, 3 ) + this.coef[1] * Math.Pow( s, 2 ) + this.coef[2] * s + this.coef[3];
        }

        #endregion

    }
}
