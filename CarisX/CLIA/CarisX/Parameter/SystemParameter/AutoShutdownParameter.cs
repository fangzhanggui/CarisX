using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// 自動シャットダウン設定
    /// </summary>
    public class AutoShutdownParameter : AttachmentParameter
    {
        #region [定数定義]

        /// <summary>
        /// 自動シャットダウン時間（分）　最小設定時刻（分）
        /// </summary>
        public const Int32 AUTO_SHUTDOWN_TIME_MIN = 1;
        /// <summary>
        /// 自動シャットダウン時間（分）　最大設定時刻（分）
        /// </summary>
        public const Int32 AUTO_SHUTDOWN_TIME_MAX = 9999;

        /// <summary>
        /// 自動シャットダウン時間（分）　デフォルト設定時刻（分）
        /// </summary>
        public const Int32 AUTO_SHUTDOWN_TIME_DEFAULT = 10;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 自動シャットダウン時間（分）
        /// </summary>
        public Int32 AutoShutdownTime { get; set; } = AUTO_SHUTDOWN_TIME_DEFAULT;

        #endregion
    }
}

