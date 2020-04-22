using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.Common.Calculator
{
    /// <summary>
    /// 収束定義
    /// </summary>
    public static class ConvergeConst
    {
        #region [クラス変数定義]

        /// <summary>
        /// 収束ループ回数最大
        /// </summary>
        public const Int32 MaxCount = 300;
        /// <summary>
        /// イプシロン(収束判定値)
        /// </summary>
        public const Double Eps = 0.000001;

        #endregion

    }

    /// <summary>
    /// 計算エラー
    /// </summary>
    public enum CalcuErr
    {
        /// <summary>
        /// 非エラー
        /// </summary>
        NoError = 0,
        /// <summary>
        /// ゼロ除算エラー
        /// </summary>
        DivByZero,
        /// <summary>
        /// 検量線なし
        /// </summary>
        NoCurveType,
        /// <summary>
        /// P/N比許容範囲外エラー
        /// </summary>
        PNRatio,
        /// <summary>
        /// 濃度正常範囲外(上限)エラー
        /// </summary>
        High,
        /// <summary>
        /// 濃度正常範囲外(下限)エラー
        /// </summary>
        Low,
        /// <summary>
        /// 値非収束エラー
        /// </summary>
        NotConverge,
        /// <summary>
        /// Abnormal1エラー
        /// </summary>
        Abnormal1,
        /// <summary>
        /// Abnormal2エラー
        /// </summary>
        Abnormal2,
        /// <summary>
        /// Abnormal3エラー
        /// </summary>
        Abnormal3,
        /// <summary>
        /// Abnormal4エラー
        /// </summary>
        Abnormal4,
        /// <summary>
        /// Abnormal5エラー
        /// </summary>
        Abnormal5,
        /// <summary>
        /// Abnormal6エラー
        /// </summary>
        Abnormal6,
        /// <summary>
        /// Abnormal7エラー
        /// </summary>
        Abnormal7,
        /// <summary>
        /// 濃度算出エラー
        /// </summary>
        Other,
    }

    //public enum FilterType
    //{
    //    C100,
    //    C1
    //}

    /// <summary>
    /// 計算値グラフ用ポイント
    /// </summary>
    public class ItemPoint : ICloneable
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// ポイントY値
        /// </summary>
        public Double yPos;
        /// <summary>
        /// ポイントX値
        /// </summary>
        public Double xPos;

        #endregion

        #region[コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="count">カウント値</param>
        /// <param name="conc">濃度値</param>
        public ItemPoint( Double count, Double conc )
        {
            yPos = count;
            xPos = conc;
        }

        #endregion


        #region [publicメソッド]

        /// <summary>
        /// クローン生成(簡易コピー)
        /// </summary>
        /// <remarks>
        /// 現在のインスタンスの複製(簡易コピー)を作成します。
        /// </remarks>
        /// <returns>コピー結果</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

    }

    ///// <summary>
    ///// C++の関数と同等の動作を行う関数群
    ///// </summary>
    //static class CppDummy
    //{
    //    static public Int32 atoi( String str )
    //    {
    //        Int32 result = 0;
    //        try
    //        {
    //            result = Int32.Parse( str );
    //        }
    //        catch
    //        {
    //        }
    //        return result;
    //    }
    //    static public Double atof( String str )
    //    {
    //        Double result = 0;
    //        try
    //        {
    //            result = Double.Parse( str );
    //        }
    //        catch
    //        {
    //        }
    //        return result;
    //    }
    //}
}
