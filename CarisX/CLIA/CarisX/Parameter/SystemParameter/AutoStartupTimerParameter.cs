using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// 自動立ち上げタイマー設定
    /// </summary>
	public class AutoStartupTimerParameter : AttachmentParameter
	{
        #region [定数定義]

        /// <summary>
        /// 曜日の設定
        /// </summary>
        [Flags]
        public enum DayOfWeek : int
        {
            /// <summary>
            /// 月曜日
            /// </summary>
            Mon = 0x01,
            /// <summary>
            /// 火曜日
            /// </summary>
            Tue = 0x02,
            /// <summary>
            /// 水曜日
            /// </summary>
            Wed = 0x04,
            /// <summary>
            /// 木曜日
            /// </summary>
            Thu = 0x08,
            /// <summary>
            /// 金曜日
            /// </summary>
            Fri = 0x10,
            /// <summary>
            /// 土曜日
            /// </summary>
            Sat = 0x20,
            /// <summary>
            /// 日曜日
            /// </summary>
            Sun = 0x40
        }

        /// <summary>
        /// 自動起動タイマ　最小設定時間（時）
        /// </summary>
        public const Int32 AUTO_STARTUP_HOUR_MIN = 0;
        /// <summary>
        /// 自動起動タイマ　最大設定時間（時）
        /// </summary>
        public const Int32 AUTO_STARTUP_HOUR_MAX = 23;

        /// <summary>
        /// 自動起動タイマ　最大設定時間（分）
        /// </summary>
        public const Int32 AUTO_STARTUP_MINUTE_MIN = 0;
        /// <summary>
        /// 自動起動タイマ　最大設定時間（分）
        /// </summary>
        public const Int32 AUTO_STARTUP_MINUTE_MAX = 59;

        /// <summary>
        /// 曜日  デフォルト値設定
        /// </summary>
        public const DayOfWeek DAY_OF_WEEK_DEFAULT = (DayOfWeek)0x00;
        /// <summary>
        /// 自動起動タイマ　デフォルト設定時間（時）
        /// </summary>
        public const Int32 AUTO_STARTUP_HOUR_DEFAULT = AUTO_STARTUP_HOUR_MIN;
        /// <summary>
        /// 自動起動タイマ　デフォルト設定時間（分）
        /// </summary>
        public const Int32 AUTO_STARTUP_MINUTE_DEFAULT = AUTO_STARTUP_MINUTE_MIN;

        #endregion

        #region[プロパティ]

        /// <summary>
        /// 自動立ち上げ日（曜日）　設定/取得
        /// </summary>
        public DayOfWeek SelectDayOfWeek { get; set; } = DAY_OF_WEEK_DEFAULT;

        /// <summary>
        /// 自動立ち上げ時間（時）　設定/取得
        /// </summary>
        public Int32 AutoStartupHour { get; set; } = AUTO_STARTUP_HOUR_DEFAULT;

        /// <summary>
        /// 自動立ち上げ時間（分）　設定/取得
        /// </summary>
        public Int32 AutoStartupMinute { get; set; } = AUTO_STARTUP_MINUTE_DEFAULT;
        #endregion
    }
	 
}
 
