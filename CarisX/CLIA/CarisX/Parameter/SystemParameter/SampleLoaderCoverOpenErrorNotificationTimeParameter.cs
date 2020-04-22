using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// サンプルラック架設部カバーオープンエラー通知時間設定
    /// </summary>
	public class SampleLoaderCoverOpenErrorNotificationTimeParameter : AttachmentParameter
	{
        #region [定数定義]

        /// <summary>
        /// カバーオープンエラー通知時間　最小設定時刻（分）
        /// </summary>
        public const Int32 COVER_OPEN_TIME_MIN = 1;
        /// <summary>
        /// カバーオープンエラー通知時間　最大設定時刻（分）
        /// </summary>
        public const Int32 COVER_OPEN_TIME_MAX = 120;

        /// <summary>
        /// カバーオープンエラー通知時間　デフォルト設定時刻（分）
        /// </summary>
        public const Int32 COVER_OPEN_TIME_DEFAULT = 5;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// カバーオープンエラー通知時間（分）
        /// </summary>
        private Int32 sampCoverOpenTime = COVER_OPEN_TIME_DEFAULT;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// カバーオープンエラー通知時間の設定／取得
        /// </summary>
        public Int32 SampCoverOpenTime
        {
            get
            {
                return this.sampCoverOpenTime;
            }
            set
            {
                this.sampCoverOpenTime = value;
            }
        }

        #endregion
	}
	 
}
 
