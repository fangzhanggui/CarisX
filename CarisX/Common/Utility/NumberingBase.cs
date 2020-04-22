using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Oelco.Common.Utility
{
    /// <summary>
    /// 連番生成クラス
    /// </summary>
    /// <remarks>
    /// 連番生成機能を提供します。
    /// 各種連番値はこのクラスを継承したクラスにより生成されます。
    /// </remarks>
    /// <typeparam name="NumType">連番使用型(プリミティブ型を設定してください)</typeparam>
    public class NumberingBaseT<NumType> where NumType : IComparable, IFormattable, IConvertible, IComparable<NumType>, IEquatable<NumType>
    {

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NumberingBaseT()
        {
            //this.StartCount = 0;
            //this.EndCount = Int32.MaxValue - 1;
            //this.IncrementCount = 1;
            //this.Number = this.StartCount;
        }

        #endregion

        #region [インスタンス変数]

        /// <summary>
        /// 初回発番フラグ
        /// </summary>
        private Boolean firstNumber = true;

        #endregion


        #region [プロパティ]
        /// <summary>
        /// インクリメント開始値 設定/取得
        /// </summary>
        public NumType StartCount
        {
            get;
            set;
        }
        /// <summary>
        /// インクリメント終了値 設定/取得
        /// </summary>
        public NumType EndCount
        {
            get;
            set;
        }
        /// <summary>
        /// インクリメント値 設定/取得
        /// </summary>
        public NumType IncrementCount
        {
            get;
            set;
        }
        private NumType nowValue = default(NumType);
        /// <summary>
        /// 現在値 設定/取得
        /// </summary>
        public NumType Number
        {
            get
            {
                return nowValue;
            }
            protected set
            {
                nowValue = value;
            }
        }
        /// <summary>
        /// 最新発番時刻 設定/取得
        /// </summary>
        public DateTime LatestNumberingDate
        {
            get;
            protected set;
        }
        #endregion

        #region [publicメソッド]


        /// <summary>
        /// 番号生成
        /// </summary>
        /// <remarks>
        /// 現在の番号に、インクリメントを行った値を返します。
        /// </remarks>
        /// <returns>新しく生成した値</returns>
        public virtual NumType CreateNumber()
        {
                
            this.increment();

            return this.Number;
        }

        /// <summary>
        /// 番号初期化
        /// </summary>
        /// <remarks>
        /// 現在の連番値を初期化します。
        /// </remarks>
        public virtual void ResetNumber()
        {
            this.Number = this.StartCount;
            this.LatestNumberingDate = DateTime.MinValue;
            this.judgeFirstNumber();
        }


        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <remarks>
        /// クラスの状態初期化を行います。
        /// この関数をオーバーライドする場合は、処理最後にこの関数を呼び出して下さい。
        /// </remarks>
        public virtual void Initialize()
        {
            this.judgeFirstNumber();
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// 初回発番判定処理
        /// </summary>
        /// <remarks>
        /// 現在の保持値が、設定された発番開始値と同一か判定を行います。
        /// The current retention value, it will do the same or judgment and numbering start value that has been set.
        /// </remarks>
        protected Boolean judgeFirstNumber()
        {
            // 初回発番フラグ設定
            this.firstNumber = ( this.LatestNumberingDate == DateTime.MinValue );
            return this.firstNumber;
        }


        /// <summary>
        /// インクリメント処理
        /// </summary>
        /// <remarks>
        /// 番号の現在値をIncrementCountに従って加算します。
        /// 最大値を超過した場合、overFlow関数が呼び出されます。
        /// </remarks>
        protected virtual void increment()
        {

            // 設定した値から発番が開始されるよう、初回呼び出しのみインクリメント無しで番号を返す
            dynamic number = this.Number;
            dynamic incrementCount = this.IncrementCount;
            dynamic endCount = this.EndCount;
            if ( !this.firstNumber )
            {
                //this.Number += this.IncrementCount;
                number += incrementCount;
            }
            this.Number = number;

            if ( number > endCount )
            {
                this.overFlow();
            }

            this.LatestNumberingDate = DateTime.Now;
            this.judgeFirstNumber();
        }

        /// <summary>
        /// 最大値超過処理
        /// </summary>
        /// <remarks>
        /// 現在値が最大値を超過した際に呼び出されます。
        /// 現在値を調整する処理を定義します。
        /// </remarks>
        protected virtual void overFlow()
        {
            this.ResetNumber();
        }


        #endregion

    }
}
