using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// サイクル時間設定
    /// </summary>
	public class CycleTimeParameter : AttachmentParameter
	{
        #region [定数定義]

        /// <summary>
        /// サイクルタイマ　最小設定時間（秒）
        /// </summary>
        public const Int32 CYCLE_TIME_MIN = 18;
        /// <summary>
        /// サイクルタイマ　最大設定時間（秒）
        /// </summary>
        public const Int32 CYCLE_TIME_MAX = 60;

        /// <summary>
        /// サイクルタイマ　デフォルト設定時間（秒）
        /// </summary>
        public const Int32 CYCLE_TIME_DEFAULT = CYCLE_TIME_MIN;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// サイクル時間（秒）　設定／取得
        /// </summary>
        public Int32 CycleTime { get; set; } = CYCLE_TIME_DEFAULT;

        #endregion
    }
	 
}
 
