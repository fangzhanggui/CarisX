using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.Common.Calculator
{
    /// <summary>
    /// 抑制率(INH)
    /// </summary>
    public class InhMethod : ICalcMethod
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 陽性カウント値
        /// </summary>
        protected Double pc;
        
        /// <summary>
        /// 陰性カウント値
        /// </summary>
        protected Double nc;

        /// <summary>
        /// 計算エラー
        /// </summary>
        protected CalcuErr calcuError = CalcuErr.NoError;

        /// <summary>
        /// 係数A
        /// </summary>
        protected Double coefA;

        /// <summary>
        /// 係数B
        /// </summary>
        protected Double coefB;

        /// <summary>
        /// 係数C
        /// </summary>
        protected Double coefC;

        /// <summary>
        /// 係数D
        /// </summary>
        protected Double coefD;

        /// <summary>
        /// 係数E
        /// </summary>
        protected Double coefE;

        #endregion
        
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pc">陽性カウント値</param>
        /// <param name="nc">陰性カウント値</param>
        /// <param name="coef_a">係数A</param>
        /// <param name="coef_b">係数B</param>
        /// <param name="coef_c">係数C</param>
        /// <param name="coef_d">係数D</param>
        /// <param name="coef_e">係数E</param>
        public InhMethod( Double pc, Double nc, Double coef_a, Double coef_b, Double coef_c, Double coef_d, Double coef_e )
        {
            this.SetPCNC( pc, nc );
            this.SetCoef( coef_a, coef_b, coef_c, coef_d, coef_e );
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 陽性/陰性カウント値設定
        /// </summary>
        /// <remarks>
        /// 陽性/陰性カウント値を設定します。
        /// </remarks>
        /// <param name="pc">陽性カウント値</param>
        /// <param name="nc">陰性カウント値</param>
        public void SetPCNC( Double pc, Double nc )
        {
            this.pc = pc;
            this.nc = nc;
        }

        /// <summary>
        /// 係数設定
        /// </summary>
        /// <remarks>
        /// 係数A～Eに引数値を設定します。
        /// </remarks>
        /// <param name="coef_a">係数A</param>
        /// <param name="coef_b">係数B</param>
        /// <param name="coef_c">係数C</param>
        /// <param name="coef_d">係数D</param>
        /// <param name="coef_e">係数E</param>
        public void SetCoef( Double coef_a, Double coef_b, Double coef_c, Double coef_d, Double coef_e )
        {
            this.coefA = coef_a;
            this.coefB = coef_b;
            this.coefC = coef_c;
            this.coefD = coef_d;
            this.coefE = coef_e;
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
        /// X値算出(濃度値取得)
        /// </summary>
        /// <remarks>
        /// X値(濃度値)の算出結果を返します。
        /// </remarks>
        /// <param name="yPoint">Y値(カウント値)</param>
        /// <returns>X値(濃度値)</returns>
        public Double GetX( Double yPoint )
        {
            this.calcuError = CalcuErr.NoError;

            if ( ( this.nc == 0 ) || ( this.coefC * this.nc + this.coefD * this.pc ) == 0 )
            {
                this.calcuError = CalcuErr.DivByZero;
                return 0.0;
            }

            Double val = ( this.coefA * this.nc + this.coefB * yPoint ) / ( ( this.coefC * this.nc + this.coefD * this.pc ) * this.coefE );

            return val;
        }

        #region ICalcMethod メンバー

        /// <summary>
        /// Y値取得(カウント値取得)
        /// </summary>
        /// <remarks>
        /// Y値を返します。
        /// </remarks>
        /// <param name="xPoint">X値(濃度値)</param>
        /// <remarks>本メソッドはインターフェースの共通化の為実装されていますが、使用はしないこと。</remarks>
        /// <returns>0</returns>
        public Double GetY( Double xPoint )
        {
            return 0;
        }

        #endregion

        #endregion

    }
}