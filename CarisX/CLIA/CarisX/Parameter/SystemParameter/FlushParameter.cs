using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// フラッシュプライム設定
    /// </summary>
	public class FlushPrimeParameter : AttachmentParameter
    {
        #region [定数定義]

        /// <summary>
        /// フラッシュプライム起動時間（分）　最小設定時刻（分）
        /// </summary>
        public const Int32 FLUSH_PRIME_TIME_MIN = 1;
        /// <summary>
        /// フラッシュプライム起動時間（分）　最大設定時刻（分）
        /// </summary>
        public const Int32 FLUSH_PRIME_TIME_MAX = 360;

        /// <summary>
        /// フラッシュプライム起動時間（分）　デフォルト設定時刻（分）
        /// </summary>
        public const Int32 FLUSH_PRIME_TIME_DEFAULT = 120;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// フラッシュプライム起動時間（分）
        /// </summary>
        public Int32 FlushPrimeTime { get; set; } = FLUSH_PRIME_TIME_DEFAULT;

        #endregion
    }
	 
}
 
