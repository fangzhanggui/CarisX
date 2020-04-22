using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.Utility;

namespace Oelco.CarisX.Utility
{
    /// <summary>
    /// 連番生成クラス
    /// </summary>
    public class NumberingBase : NumberingBaseT<Int32>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NumberingBase()
        {
            this.StartCount = 0;
            this.EndCount = Int32.MaxValue - 1;
            this.IncrementCount = 1;
            this.Number = this.StartCount;
        }
    }
}
