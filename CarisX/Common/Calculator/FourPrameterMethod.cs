using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.Const;

namespace Oelco.Common.Calculator
{
    /// <summary>
    /// 4パラメータ
    /// </summary>
    public struct FourParameter
    {
        /// <summary>
        /// CurveTop
        /// </summary>
        public Double CoefA;
        /// <summary>
        /// Slant
        /// </summary>
        public Double CoefB;
        /// <summary>
        /// CorrespondMiddleX
        /// </summary>
        public Double CoefC;
        /// <summary>
        /// CurveBottom
        /// </summary>
        public Double CoefD;
    }

    /// <summary>
    /// 4パラメータ計算処理クラス
    /// </summary>
    /// <remarks>
    /// 4パラメータ計算処理クラスです。
    /// </remarks>
    public class FourParameterMethod : ICalcMethod
    {
        #region [定数定義]

        /// <summary>
        /// 下限超過
        /// </summary>
        public const Int32 LO_ERR = -1;

        /// <summary>
        /// 4パラメータの精度カウンタ
        /// </summary>
        public const Int32 DEFAULT_PRECISION_COUNT = 100;		// 4パラメータの精度カウンタ

        /// <summary>
        /// パラメータ数
        /// </summary>
        public const Int32 PARAMETER_COUNT = 4;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 4パラメータ
        /// </summary>
        private FourParameter parameter = new FourParameter();

        /// <summary>
        /// 処理データ数
        /// </summary>
        private Int32 dataCount;

        /// <summary>
        /// X座標
        /// </summary>
        private Double[] xPoints;

        /// <summary>
        /// Y座標
        /// </summary>
        private Double[] yPoints;
        
        /// <summary>
        /// 最大データ数
        /// </summary>
        private Int32 max_data;

        /// <summary>
        /// ランダム生成用シード値
        /// </summary>
        private Int32 seed = 1;

        /// <summary>
        /// 4パラメータ精度カウンタ
        /// </summary>
        private Int32 precisionCount = FourParameterMethod.DEFAULT_PRECISION_COUNT;

        /// <summary>
        /// 計算エラー
        /// </summary>
        private CalcuErr calcuError;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 係数Aの取得、設定
        /// </summary>
        public Double CoefA
        {
            get
            {
                return this.parameter.CoefA;
            }
            set
            {
                this.parameter.CoefA = value;
            }
        }

        /// <summary>
        /// 係数Bの取得、設定
        /// </summary>
        public Double CoefB
        {
            get
            {
                return this.parameter.CoefB;
            }
            set
            {
                this.parameter.CoefB = value;
            }
        }

        /// <summary>
        /// 係数Cの取得、設定
        /// </summary>
        public Double CoefC
        {
            get
            {
                return this.parameter.CoefC;
            }
            set
            {
                this.parameter.CoefC = value;
            }
        }

        /// <summary>
        /// 係数Dの取得、設定
        /// </summary>
        public Double CoefD
        {
            get
            {
                return this.parameter.CoefD;
            }
            set
            {
                this.parameter.CoefD = value;
            }
        }

        #endregion

        /// <summary>
        /// 4参数加权方法类型
        /// </summary>
        private FourPType m_fourPType = FourPType.str1Y;

        public FourPType FourPType
        {
            set
            {
                m_fourPType = value;
            }
        }

        /// <summary>
        /// 加权参数K,此参数只在加权1/y^K方法上体现
        /// </summary>
        private double m_K = 1;
        public double ValueK
        {
            set
            {
                m_K = value;
            }
        }

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="no_data">最大データ数</param>
        /// <param name="paramA">CurveTop</param>
        /// <param name="paramB">Slant</param>
        /// <param name="paramC">CorrespondMiddleX</param>
        /// <param name="paramD">CurveBottom</param>
        public FourParameterMethod( Int32 no_data, Double paramA = 0.0, Double paramB = 0.0, Double paramC = 0.0, Double paramD = 0.0 )
        {
            this.calcuError = CalcuErr.NoError;
            this.dataCount = 0;

            // 最大データ数をセット
            this.max_data = no_data;

            try
            {
                // ワーク用ポインターに実メモリー領域をセット
                this.xPoints = new Double[no_data];
                this.yPoints = new Double[no_data];
            }
            catch
            {
                // もしメモリーが確保できなかったらＮＵＬＬ
                return;
            }

            // 指定された係数を設定
            this.parameter.CoefA = paramA;
            this.parameter.CoefB = paramB;
            this.parameter.CoefC = paramC;
            this.parameter.CoefD = paramD;
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// X,Y値の設定
        /// </summary>
        /// <remarks>
        /// X,Y値に引数値を設定します。
        /// </remarks>
        /// <param name="xval">X値(濃度値)</param>
        /// <param name="yval">Y値(カウント値)</param>
        /// <param name="no_data">データ数</param>
        public void SetData( Double[] xval, Double[] yval, Int32 no_data )
        {
            this.dataCount = no_data;	            // データ数をセットする。

            // データをセットする。
            for (Int32 i = 0; i < this.dataCount; ++i )
            {
                this.xPoints[i] = xval[i];
                this.yPoints[i] = yval[i];
            }
        }

        /// <summary>
        /// X値(濃度値)の取得
        /// </summary>
        /// <remarks>
        /// X値(濃度値)を返します。
        /// </remarks>
        /// <param name="yPoint">Y値(カウント値)</param>
        /// <returns>X値(濃度値)</returns>
        public Double GetX( Double yPoint )
        {
            Double xPoint = 0;
            this.GetX( ref xPoint, yPoint );
            return xPoint;
        }

        /// <summary>
        /// X値(濃度値)の取得
        /// </summary>
        /// <remarks>
        /// X値(濃度値)を返します。
        /// </remarks>
        /// <param name="x_val">X値(濃度値)</param>
        /// <param name="y_val">Y値(カウント値)</param>
        /// <returns>結果(0:成功/-1:失敗[下限超過])</returns>
        public Int32 GetX( ref Double x_val, Double y_val )
        {
            Int32 result = 0;

            Double maxX = this.xPoints.Max();
            Double minX = this.xPoints.Min();
            Double maxY = this.yPoints.Max();
            Double minY = this.yPoints.Min();

            FourParameter ans =  this.parameter;
            Double val1;
            Double val2;
            
            // ゼロ除算回避
            if ( ( y_val - ans.CoefD ) == 0.0 )
            {
                val1 = 0.0;
            }
            else
            {
                val1 = ( ans.CoefA - ans.CoefD ) / ( y_val - ans.CoefD );
            }
            val1 -= 1.0;

            // ゼロ除算回避
            if ( ans.CoefB == 0.0 )
            {
                val2 = 0.0;
            }
            else
            {
                val2 = 1.0 / ans.CoefB;
            }

            // val1が0未満でval2が整数のみでない、または、val1が0でval2が0の場合、x_valは-1.0
            if ( ( val1 < 0.0 && val2 != Math.Floor( val2 ) ) || ( val1 == 0.0 && val2 == 0.0 ) )
            {
                x_val = -1.0;
                result = FourParameterMethod.LO_ERR;

                //find the real MaxY and minY
                maxY = GetY(maxX);
                minY = GetY(minX);
                if (minY > maxY)
                {
                    double dTemp = maxY;
                    maxY = minY;
                    minY = dTemp;
                }
                if (maxY < y_val)
                {
                    this.calcuError = CalcuErr.High;
                }
                // else if (Math.Abs(minY - y_val) < DEFAULT_MIN_RANGER)
                else if (minY > y_val)
                {
                    this.calcuError = CalcuErr.Low;
                }                
            }
            else
            {
                x_val = ans.CoefC * Math.Pow( val1, val2 );

                if ( x_val != 0.0 && x_val == -0.0 )
                {
                    x_val = -1.0;
                    result = FourParameterMethod.LO_ERR;
                    //find the real MaxY and minY
                    maxY = GetY(maxX);
                    minY = GetY(minX);
                    if (minY > maxY)
                    {
                        double dTemp = maxY;
                        maxY = minY;
                        minY = dTemp;
                    }
                    if (maxY < y_val)
                    {
                        this.calcuError = CalcuErr.High;
                    }
                   // else if (Math.Abs(minY - y_val) < DEFAULT_MIN_RANGER)
                    else if (minY > y_val)
                    {
                        this.calcuError = CalcuErr.Low;
                    }
                }
                else if ( x_val < minX )
                {
                    x_val = minX;
                    result = FourParameterMethod.LO_ERR;
                    //find the real MaxY and minY
                    maxY = GetY(maxX);
                    minY = GetY(minX);                    
                    if (minY > maxY)
                    {
                        double dTemp = maxY;
                        maxY = minY;
                        minY = dTemp;
                    }

                    if (maxY < y_val)
                    {
                        this.calcuError = CalcuErr.High;
                    }
                    // else if (Math.Abs(minY - y_val) < DEFAULT_MIN_RANGER)
                    else if (minY > y_val)
                    {
                        this.calcuError = CalcuErr.Low;
                    }
                }
            }

            return result;
        }

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
            Double yPoint = 0;
            this.GetY( out yPoint, xPoint );
            return yPoint;
        }

        /// <summary>
        /// Y値(カウント値)の取得
        /// </summary>
        /// <remarks>
        /// Y値(カウント値)を返します。
        /// </remarks>
        /// <param name="y_val">X値(濃度値)</param>
        /// <param name="x_val">Y値(カウント値)</param>
        /// <returns>結果 成功:1 失敗:0</returns>
        public Int32 GetY( out Double y_val, Double x_val )
        {
            Int32 result = 0;
            Double val1, val2;

            if ( this.parameter.CoefC == 0.0 )
            {
                val1 = 0.0;
            }
            else
            {
                val1 = x_val / this.parameter.CoefC;
            }
            val2 = Math.Pow( val1, this.parameter.CoefB ) + 1.0;

            if ( val2 == 0.0 )
            {
                y_val = this.parameter.CoefD;
            }
            else
            {
                y_val = this.parameter.CoefD +
                    ( this.parameter.CoefA - this.parameter.CoefD ) / ( val2 );
            }
            result = 1;

            return result;
        }

        /// <summary>
        /// Y値(カウント値)の取得
        /// </summary>
        /// <remarks>
        /// Y値(カウント値)を返します。
        /// </remarks>
        /// <param name="param">4パラメータ</param>
        /// <param name="y_val">X値(濃度値)</param>
        /// <param name="x_val">Y値(カウント値)</param>
        /// <returns>結果 成功:1 失敗:0</returns>
        protected Int32 GetY( FourParameter param, out Double y_val, Double x_val )
        {
            if ( param.CoefC == 0 )
            {
                y_val = 0;
                return 0;
            }

            y_val = param.CoefD + ( param.CoefA - param.CoefD ) / ( 1 + Math.Pow( ( x_val / param.CoefC ), param.CoefB ) );
            return 1;
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

        /// <summary>
        /// 4パラメータを計算する。
        /// </summary>
        /// <remarks>
        /// 4パラメータを返します。
        /// </remarks>
        /// <returns>結果(true:計算成功/falsae:計算失敗)</returns>
        public Boolean Calc4Parameter()
        {
            FourParameter result = new FourParameter()
            {
                CoefB = CoefC = CoefD = CoefA = 0.0d
            };

            if ( !this.calcu_4parameter( ref result, this.yPoints, this.xPoints, this.dataCount ) )
            {
                return false;
            }

            this.parameter = result;

            return true;
        }

        #region _計算処理部_

        /// <summary>
        /// 4パラメータを繰り返し求め精度を上げる
        /// </summary>
        /// <remarks>
        /// 4パラメータを繰り返し求め精度を上げる処理を行います。
        /// </remarks>
        /// <param name="result_param">calcu_method 1：係数B,C
        ///                                   　　　2：係数A,B,C,D</param>
        /// <param name="y">Y値(配列)</param>
        /// <param name="x">X値(配列)</param>
        /// <param name="method">1：result[0] = B, result[1] = D
        ///                            2：result[0] = A, result[1] = B, result[2] = C, result[4] = D</param>
        /// <param name="no_of_data">x,y値の配列数</param>
        /// <param name="p_a">calcu_method 1の時、設定された係数A</param>
        /// <param name="p_d">calcu_method 1の時、設定された係数D</param>
        /// <returns>true   :   成功
        ///          false  :   失敗</returns>
        public Boolean calcu_4parameter( ref FourParameter result_param, Double[] y, Double[] x, Int32 no_of_data )
        {
            this.seed = 1;
            Int32 Loop_cnt = 0;
            Int32 err_cnt = 0;
            Double min_diff = double.MaxValue;
            Double diff = 0;

            while ( true )
            {
                if ( !calcu_param( ref diff ) )
                {
                    return false;
                }

                if ( diff == 0 )
                {
                    if ( err_cnt > 200 )
                    {
                        return false;
                    }
                    err_cnt++;
                }
                else
                {
                    Loop_cnt++;
                    if ( Loop_cnt < this.precisionCount )
                    {
                        if ( diff < min_diff )
                        {
                            min_diff = diff;
                            result_param = this.parameter;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (min_diff > Math.Pow(10, 14))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// パラメーターCの初期値を算出する
        /// </summary>
        private bool calcu_init_param_c(ref double paramC)
        {
            bool ret = false;

            //Y軸最大値の半値
            double half_y = yPoints[dataCount - 1] / 2;
            //Y軸最大値の半値が含まれるポイントのインデックス
            int point = -1;
            //Y軸最大値の半値が含まれるポイントをサーチ
            //Y軸最大値を処理してしまうと例外が発生するので処理しない
            for (int i = 0; i < dataCount - 1; i++)
            {
                //半値が含まれていれば、ポイントをセットし終了
                if (yPoints[i] <= half_y && half_y < yPoints[i + 1])
                {
                    point = i;
                    break;
                }
            }

            if (point >= 0)
            {
                //X(paramC) = (X1-X2)/(Y1-Y2)*(Y(half)-Y1)+X1
                paramC = (xPoints[point] - xPoints[point + 1]) / (yPoints[point] - yPoints[point + 1]) * (half_y - yPoints[point]) + xPoints[point];
                ret = true;
            }
            return ret;
        }

        #region [privateメソッド]

        #region _計算処理部_

        /// <summary>
        /// 4パラメータを算出する
        /// </summary>
        /// <remarks>
        /// 4パラメータを算出します。
        /// </remarks>
        /// <param name="diff_cnt">測定値と4パラメータ曲線とのトータル誤差</param>
        /// <returns>true   :   成功
        ///          false  :   失敗</returns>
        private Boolean calcu_param( ref Double diff )
        {
            Int32 jl = 0, jh = 0, js = 0;

            // 計算途中データの格納場所
            // data[*,0]:それぞれのトータル誤差を格納
            // no_param 1の時　→　data[*,1]:B値、data[*,2]:C値
            //          2の時　→　data[*,1]:A値、data[*,2]:B値、data[*,3]:C値、data[*,4]:D値
            Double[,] data = new Double[5, 6];

            double w_param_c = 0;
            if (!calcu_init_param_c(ref w_param_c))
                return false;

            data[1, 1] = yPoints[0] - 100;
            data[2, 1] = 1.0;
            data[3, 1] = w_param_c;
            data[4, 1] = yPoints[dataCount - 1] + 1;
//-----------------------------------------------------------------

            Double rand1;
            Double rand2;

            Random ran = new System.Random( this.seed++ );

            // 任意の係数を求める
            for ( Int32 j = 2; j <= 4 + 1; j++ )
            {
                for ( Int32 i = 1; i <= 4; i++ )
                {
                    rand1 = (Double)ran.Next(10000000);     //(1000);//(RAND_MAX);
                    rand2 = (Double)ran.Next(10000000);     //(1000);//(RAND_MAX);
                    data[i, j] = 2 * rand1 * data[i, 1] + 0.01 * ( rand2 - 0.5 );
                }
            }

            FourParameter parametor = new FourParameter();
            Double w_diff = 0;

            // 各パラメータでの曲線グラフと測定値とのトータル誤差を求める
            for ( Int32 j = 1; j <= 4 + 1; j++ )
            {
                parametor.CoefA = data[1, j];
                parametor.CoefB = data[2, j];
                parametor.CoefC = data[3, j];
                parametor.CoefD = data[4, j];

                if ( !calcu_diff( parametor, out w_diff ) )
                {
                    return false;
                }
                data[0, j] = w_diff;
            }

            Double diff_cnt;
            Double diff_cnt_before = 0;
            Double diff_high = 0;
            Double diff_low = Double.MaxValue;  // Math.Pow(10, 14);
            Double diff_sec = 0;
            Double[] Ave = new Double[5];

            Int32 Loop_cnt = 0;
            
            while ( Loop_cnt < 500 )    // 最大500回で終了
            {
                Loop_cnt++;

                diff_cnt = 0;
                for ( Int32 i = 1; i <= 4 + 1; i++ )
                {
                    diff_cnt += data[0, i];
                }
                // ここでdiff_cnt_before * 0.0000001に変えることによりFitting精度が上がることを確認する
                if (Math.Abs(diff_cnt - diff_cnt_before) > (diff_cnt_before * 0.0000001))
                {
                    diff_cnt_before = diff_cnt;
                }
                else
                {
                    this.parameter.CoefA = data[1, jl];
                    this.parameter.CoefB = data[2, jl];
                    this.parameter.CoefC = data[3, jl];
                    this.parameter.CoefD = data[4, jl];

                    diff = data[0, jl];
                    break;
                }

                diff_high = 0;
                diff_low = Double.MaxValue;  // Math.Pow(10, 14);

                // ズレの最大のものと最小のものを抜き出す
                for ( Int32 j = 1; j <= 4 + 1; j++ )
                {
                    if ( diff_high < data[0, j] )
                    {
                        diff_high = data[0, j];
                        jh = j;
                    }

                    if ( diff_low > data[0, j] )
                    {
                        diff_low = data[0, j];
                        jl = j;
                    }
                }
                // ひとまずココで最小のものをセットする
                this.parameter.CoefA = data[1, jl];
                this.parameter.CoefB = data[2, jl];
                this.parameter.CoefC = data[3, jl];
                this.parameter.CoefD = data[4, jl];

                diff = diff_low;

                diff_sec = 0;
                // 二番目にズレているものを抜き出す
                for ( Int32 j = 1; j <= 4 + 1; j++ )
                {
                    if ( ( j != jh ) && diff_sec < data[0, j] )
                    {
                        js = j;
                        diff_sec = data[0, js];
                    }
                }

                // ズレが最大のものを除いた各パラメータの平均値を求める
                // 2*平均値のパラメータ　-　最大ズレ時のパラメータ　←　平均値を基準に最大時のパラメータから離れる
                for ( Int32 i = 1; i <= 4; i++ )
                {
                    Ave[i] = 0;
                    for ( Int32 j = 1; j <= 4 + 1; j++ )
                    {
                        if ( j != jh )
                        {
                            Ave[i] += data[i, j];
                        }
                    }
                    Ave[i] /= 4;
                }

                for ( Int32 i = 1; i <= 4; i++ )
                {
                    data[i, 0] = 2 * Ave[i] - data[i, jh];

                    // パラメータCがマイナスにならないように･･･
                    if ( i == 3 && ( data[i, 0] <= 0 ) )
                    {
                        data[i, 0] = Ave[i] / 2;
                    }
                }
                parametor.CoefA = data[1, 0];
                parametor.CoefB = data[2, 0];
                parametor.CoefC = data[3, 0];
                parametor.CoefD = data[4, 0];

                // 上記で求めたパラメータでズレを算出
                if ( !calcu_diff( parametor, out w_diff ) )
                {
                    return false;
                }

                // 二番目にズレているものよりズレている場合
                if ( w_diff > diff_sec )
                {
                    // 最もズレているものよりズレていなければ置き換える
                    if ( w_diff < diff_high )
                    {
                        for ( Int32 i = 1; i <= 4; i++ )
                        {
                            data[i, jh] = data[i, 0];
                        }
                        data[0, jh] = w_diff;
                        diff_high = w_diff;
                    }

                    // 最大ズレ時のパラメータ * 0.5  +  平均 * 0.5　←　平均値を基準に最大時のパラメータに近づける
                    for ( Int32 i = 1; i <= 4; i++ )
                    {
                        data[i, 0] = 0.5 * data[i, jh] + 0.5 * Ave[i];
                    }
                    parametor.CoefA = data[1, 0];
                    parametor.CoefB = data[2, 0];
                    parametor.CoefC = data[3, 0];
                    parametor.CoefD = data[4, 0];

                    // 上記で求めたパラメータでズレを算出
                    if ( !calcu_diff( parametor, out w_diff ) )
                    {
                        return false;
                    }

                    // 最大よりも小さい場合は最大と入れ替える
                    if ( w_diff < diff_high )
                    {
                        for ( Int32 i = 1; i <= 4; i++ )
                        {
                            data[i, jh] = data[i, 0];
                        }
                        data[0, jh] = w_diff;
                        diff_high = w_diff;
                    }
                    else
                    {
                        for ( Int32 j = 1; j <= 4 + 1; j++ )
                        {
                            // すべてのパラメータをズレ最小時のパラメータに近づける
                            // その時のズレを求め、パラメータ及びズレ値を更新する
                            for ( Int32 i = 1; i <= 4; i++ )
                            {
                                data[i, j] = ( data[i, j] + data[i, jl] ) / 2;
                            }
                            parametor.CoefA = data[1, j];
                            parametor.CoefB = data[2, j];
                            parametor.CoefC = data[3, j];
                            parametor.CoefD = data[4, j];

                            // 上記で求めたパラメータでズレを算出
                            if ( !calcu_diff( parametor, out w_diff ) )
                            {
                                return false;
                            }
                            data[0, j] = w_diff;
                        }
                    }

                }
                // 最小のズレよりも小さい場合
                else if ( w_diff < diff_low )
                {
                    data[0, 0] = w_diff;

                    // 求めたパラメータ * 2  -  平均　←　求めたパラメータを基準に平均のパラメータから離す
                    parametor.CoefA = 2 * data[1, 0] - Ave[1];
                    parametor.CoefB = 2 * data[2, 0] - Ave[2];
                    parametor.CoefC = 2 * data[3, 0] - Ave[3];
                    if ( parametor.CoefC <= 0 )
                    {
                        parametor.CoefC = data[3, 0] / 2;
                    }
                    parametor.CoefD = 2 * data[4, 0] - Ave[4];

                    // 上記で求めたパラメータでズレを算出
                    if ( !calcu_diff( parametor, out w_diff ) )
                    {
                        return false;
                    }

                    // もし先程求めたズレよりも小さい場合は最大時のパラメータと置き換える
                    if ( w_diff < data[0, 0] )
                    {
                        data[0, jh] = w_diff;
                        data[1, jh] = parametor.CoefA;
                        data[2, jh] = parametor.CoefB;
                        data[3, jh] = parametor.CoefC;
                        data[4, jh] = parametor.CoefD;
                    }
                    // 先程求めたパラメータと最大時のパラメータと置き換える
                    else
                    {
                        for ( Int32 i = 1; i <= 4; i++ )
                        {
                            data[i, jh] = data[i, 0];
                        }
                        data[0, jh] = data[0, 0];
                    }
                }
                // その他(最小よりも大きくて、二番目よりも小さい)の場合は最大時のパラメータと置き換える
                else
                {
                    for ( Int32 i = 1; i <= 4; i++ )
                    {
                        data[i, jh] = data[i, 0];
                    }
                    data[0, jh] = w_diff;
                }
            }

            return true;
        }

        /// <summary>
        /// 4パラメータによるy値と実際のy値のズレを算出する
        /// </summary>
        /// <remarks>
        ///　4パラメータによるy値と実際のy値のズレを算出します。
        /// </remarks>
        /// <param name="param">4パラメータ</param>
        /// <param name="diff">計算値と真値とのズレ</param>
        /// <returns>true   :   成功
        ///          false  :   失敗</returns>
        private Boolean calcu_diff( FourParameter param, out Double diff )
        {

            Double val_y = 0;
            diff = 0;            
            for (Int32 i = 0; i < this.dataCount; i++)
            {
                if (this.GetY(param, out val_y, this.xPoints[i]) == 0)
                {
                    return false;
                }
                // diff += Math.Pow((this.yPoints[i] - val_y), 2.0);
                if (m_fourPType == FourPType.str1X)
                {
                    diff += Math.Pow((this.yPoints[i] - val_y), 2.0) / this.xPoints[i];
                }
                else if (m_fourPType == FourPType.str1X2)
                {
                    diff += Math.Pow((this.yPoints[i] - val_y) / this.xPoints[i], 2.0);
                }
                else if (m_fourPType == FourPType.str1Y)
                {
                    diff += Math.Pow((this.yPoints[i] - val_y), 2.0) / Math.Abs(val_y);

                }
                else if (m_fourPType == FourPType.str1Y2)
                {
                    diff += Math.Pow((this.yPoints[i] - val_y) / val_y, 2.0);
                }
                else if (m_fourPType == FourPType.str1Y_K)
                {
                    diff += Math.Pow((this.yPoints[i] - val_y), 2.0) / Math.Pow(Math.Abs(val_y), m_K);
                }
                else
                {
                    diff += Math.Pow((this.yPoints[i] - val_y), 2.0);
                    //diff += Math.Pow((this.yPoints[i] - val_y ), 2.0)/Math.Abs(val_y);
                    // diff += Math.Pow((this.yPoints[i] - val_y)/val_y, 2.0);
                }

            }

            return true;
        }

        #endregion

        #endregion
        
    }
}
