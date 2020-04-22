using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.Common.Calculator
{
    /// <summary>
    /// スプライン(3次自然)計算処理クラス
    /// </summary>
    /// <remarks>
    /// スプライン(3次自然)計算処理クラスです。
    /// </remarks>
    public class SplineMethod : ICalcMethod
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 係数
        /// </summary>
        private Double[,] coef;

        /// <summary>
        /// 計算エラー
        /// </summary>
        private CalcuErr calcuError;

        /// <summary>
        /// 最大ポイント数
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
        /// ポイント数
        /// </summary>
        private Int32 numOfPoint;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 係数の取得
        /// </summary>
        public Double[,] CoefForSpline
        {
            get
            {
                return this.coef;
            }
        }

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SplineMethod()
        {
            this.calcuError = CalcuErr.NoError;
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

        // 濃度値を二分法から算出
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
            Double left_conc;
            Double right_conc;
            Double left_count;
            Double right_count;
            Int32 trend = 1;          // 1:右上がり 0:右下がり

            // 曲線のトレンドを決定する
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
            Double eps = 10000;
            // 濃度算出(収束ループ)
            Int32 converge_cnt = 0;

            if ( trend == 1 )
            {
                // ｶｳﾝﾄがleft_pointよりも小さい場合はLowエラーを返す
                if ( yPoint < left_count )
                {
                    this.calcuError = CalcuErr.Low;
                    right_conc = this.concs[0];
                    right_count = this.counts[0];
                    left_conc = 0;
                    left_count = GetY( left_conc );

                }

                // ｶｳﾝﾄがright_pointよりも大きい場合はHighエラーを返す
                if ( yPoint > right_count )
                {
                    this.calcuError = CalcuErr.High;

                    left_conc = this.concs[this.numOfPoint - 1];
                    left_count = this.counts[this.numOfPoint - 1];
                    right_conc = left_conc * 100;
                    right_count = GetY( right_conc );
                }

                do
                {
                    work_conc = ( left_conc + right_conc ) / 2;
                    work_count = GetY( work_conc );
                    this.calcuError = this.GetError();

                    if ( this.calcuError != CalcuErr.NoError && this.calcuError != CalcuErr.High && this.calcuError != CalcuErr.Low )
                    {
                        return 0.0;
                    }

                    if ( work_count < yPoint )
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

                } while ( eps > ConvergeConst.Eps );
            }
            else
            {
                // ｶｳﾝﾄがleft_pointよりも大きい場合はHighエラーを返す
                if ( yPoint > left_count )
                {
                    this.calcuError = CalcuErr.High;

                    right_conc = this.concs[0];
                    right_count = this.counts[0];
                    left_conc = 0;
                    left_count = GetY( left_conc );
                }

                // ｶｳﾝﾄがright_pointよりも小さい場合はLowエラーを返す
                if ( yPoint < right_count )
                {
                    calcuError = CalcuErr.Low;

                    left_conc = this.concs[this.numOfPoint - 1];
                    left_count = this.counts[this.numOfPoint - 1];
                    right_conc = left_conc * 100;
                    right_count = GetY( right_conc );
                }

                do
                {
                    work_conc = ( left_conc + right_conc ) / 2;
                    work_count = GetY( work_conc );
                    this.calcuError = this.GetError();
                    if ( this.calcuError != CalcuErr.NoError )
                    {
                        return 0.0;
                    }

                    if ( work_count > yPoint )
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

                } while ( eps > ConvergeConst.Eps );
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
            this.calcuError = CalcuErr.NoError;
            Double[] coef = new Double[4];

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

            // 該当する区間の係数をセット
            if ( 0 <= xPoint && xPoint <= this.concs[0] )
            {
                coef[0] = this.coef[0, 0];
                coef[1] = this.coef[0, 1];
                coef[2] = this.coef[0, 2];
                coef[3] = this.coef[0, 3];
            }
            else if ( xPoint > this.concs[this.numOfPoint - 1] )
            {
                coef[0] = this.coef[this.numOfPoint - 1, 0];
                coef[1] = this.coef[this.numOfPoint - 1, 1];
                coef[2] = this.coef[this.numOfPoint - 1, 2];
                coef[3] = this.coef[this.numOfPoint - 1, 3];
            }
            else
            {
                Int32 point = 0;
                foreach ( Double cnc in this.concs )
                {
                    if ( cnc == xPoint )
                    {
                        return this.counts[point];
                    }

                    if ( cnc > xPoint )
                    {
                        this.calcuError = CalcuErr.NoError;
                        break;
                    }
                    point++;
                }
                coef[0] = this.coef[point, 0];
                coef[1] = this.coef[point, 1];
                coef[2] = this.coef[point, 2];
                coef[3] = this.coef[point, 3];
            }

            // ｶｳﾝﾄを算出
            Double ret_count = coef[0] * Math.Pow( xPoint, 3 ) + coef[1] * Math.Pow( xPoint, 2 ) + coef[2] * xPoint + coef[3];
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
            this.concs = concs;
            this.counts = counts;
            this.numOfPoint = num_of_point;
            this.maxPoint = num_of_point + 2;       // α、β列、Ciの和、CiXiの和の行を追加

            this.coef = new Double[num_of_point + 1, 4];

            Double[,] x_dt = new Double[this.maxPoint, this.maxPoint];    // 配列の行
            Double[] y_dt = new Double[this.maxPoint];               // 配列の列
            Double d_work;
            Double[] x_sv = new Double[this.maxPoint];
            Double[] y_sv = new Double[this.maxPoint];

            ///////////////////////////////////////////
            // 計算準備(ワークエリアにデータをセット)
            ///////////////////////////////////////////

            // 初期化
            for ( Int32 i = 0; i < this.maxPoint; i++ )
            {
                for ( Int32 j = 0; j < this.maxPoint; j++ )
                {
                    x_dt[i, j] = 0.0d;       // 濃度ワークアリア初期化
                }
                y_dt[i] = 0.0d;              // 測定値ワークエリア初期化
            }

            // α、β係数、測定値セット
            for ( Int32 i = 0; i < this.numOfPoint; i++ )
            {
                x_dt[i, 0] = concs[i];
                x_dt[i, 1] = 1.0d;
                y_dt[i] = counts[i];
            }

            // C1～Cn-1の(x[n]-x[n-1])^3をセット(縦方向にセット)
            for ( Int32 j = 2; j < this.numOfPoint + 1; j++ )
            {
                for ( Int32 i = j - 1; i < this.numOfPoint; i++ )
                {
                    d_work = ( concs[i] ) - ( concs[j - 2] );
                    x_dt[i, j] = Math.Pow( d_work, 3 );
                }
            }

            // n+1, n+2行セット
            for ( Int32 j = 2; j < this.numOfPoint + 2; j++ )
            {
                x_dt[num_of_point, j] = 1.0d;
                x_dt[num_of_point + 1, j] = concs[j - 2];
            }

            ////////////////////////////
            // ここからα、β、C1～Cnの算出を行う
            ////////////////////////////
            for ( Int32 j = 0; j < maxPoint; j++ )
            {
                x_sv[j] = x_dt[num_of_point + 1, j];    // n+2行データセーブ
                y_sv[j] = y_dt[j];                      // 測定値セーブ
            }

            // Cnから順次消去
            for ( Int32 i = this.numOfPoint; i >= 0; i-- )
            {
                y_sv[i] = y_sv[i + 1] * x_dt[i, i + 1] - y_sv[i] * x_sv[i + 1];
                for ( Int32 j = 0; j < i + 2; j++ )
                {
                    x_sv[j] = x_sv[j] * x_dt[i, i + 1] - x_dt[i, j] * x_sv[i + 1];
                }
            }

            // 係数算出　α、β、C1～Cn
            Double[] coef = new Double[maxPoint];

            coef[0] = y_sv[0] / x_sv[0];                // 係数αセット
            coef[1] = y_dt[0] - coef[0] * x_dt[0, 0];   // 係数βセット

            // 係数C1～Cnセット
            for ( Int32 i = 1; i <= this.numOfPoint; i++ )
            {
                d_work = 0.0d;
                for ( Int32 j = 0; j <= i; j++ )
                {
                    d_work += coef[j] * x_dt[i, j];
                }

                if ( x_dt[i, i + 1] != 0 )
                {
                    coef[i + 1] = ( y_dt[i] - d_work ) / x_dt[i, i + 1];
                }
                else
                {
                    this.calcuError = CalcuErr.DivByZero;
                    return;
                }
            }

            //////////////////////////////////////////
            // ここから各区間のファクターを計算する
            //////////////////////////////////////////

            // x < x1 時のファクター
            this.coef[0, 0] = 0.0d;
            this.coef[0, 1] = 0.0d;
            this.coef[0, 2] = coef[0];
            this.coef[0, 3] = coef[1];

            // xj <= x < xj+1 時のファクター & xn <= x時のファクター(c,d)
            for ( Int32 i = 1; i <= this.numOfPoint; i++ )
            {
                this.coef[i, 0] = 0.0d;
                this.coef[i, 1] = 0.0d;
                this.coef[i, 2] = 0.0d;
                this.coef[i, 3] = 0.0d;
                for ( Int32 j = 1; j <= i; j++ )
                {
                    this.coef[i, 0] += coef[j + 1];
                    this.coef[i, 1] -= (Double)3 * coef[j + 1] * x_dt[j - 1, 0];
                    this.coef[i, 2] += (Double)3 * coef[j + 1] * x_dt[j - 1, 0] * x_dt[j - 1, 0];
                    this.coef[i, 3] -= coef[j + 1] * x_dt[j - 1, 0] * x_dt[j - 1, 0] * x_dt[j - 1, 0];
                }
                this.coef[i, 2] += coef[0];
                this.coef[i, 3] += coef[1];
            }

            // xn <= x時のファクター(a,b)
            this.coef[num_of_point, 0] = 0.0d;
            this.coef[num_of_point, 1] = 0.0d;

            ///////////////////////////////////
            // 検量線の異常チェック
            ///////////////////////////////////
            for ( Int32 i = 0; i < this.numOfPoint + 1; i++ )
            {
                if ( this.coef[i, 0] == 0 && this.coef[i, 1] == 0 && this.coef[i, 2] == 0 )
                {
                    this.calcuError = CalcuErr.Abnormal1;
                    return;
                }
            }
        }

        #endregion

    }
}
