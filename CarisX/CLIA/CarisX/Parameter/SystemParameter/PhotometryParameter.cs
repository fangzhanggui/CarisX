using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// 測光設定
    /// </summary>
	public class PhotometryParameter : AttachmentParameter
	{
        #region [定数定義]

        /// <summary>
        /// 測光モード
        /// </summary>
        public enum PhotoModeKind
        {
            /// <summary>
            /// 面積法
            /// </summary>
            AREA,
            /// <summary>
            /// ピーク法
            /// </summary>
            PEAK
        }

        /// <summary>
        /// 面積法　最小設定時間（msec）
        /// </summary>
        public const Int32 METHOD_AREA_TIME_MIN = 100;
        /// <summary>
        /// 面積法　最大設定時間（msec）
        /// </summary>
        public const Int32 METHOD_AREA_TIME_MAX = 1000;
        /// <summary>
        /// ピーク法　最小設定時間（msec）
        /// </summary>
        public const Int32 METHOD_PEAK_TIME_MIN = 1;
        /// <summary>
        /// ピーク法　最大設定時間（msec）
        /// </summary>
        public const Int32 METHOD_PEAK_TIME_MAX = 10;

        /// <summary>
        /// 測光モード　デフォルト値設定
        /// </summary>
        public const PhotoModeKind PHOTO_MODE_DEFAULT = PhotoModeKind.AREA;
        /// <summary>
        /// 面積法　デフォルト設定時間（msec）
        /// </summary>
        public const Int32 METHOD_AREA_TIME_DEFAULT = METHOD_AREA_TIME_MIN;
        /// <summary>
        /// ピーク法　デフォルト設定時間（msec）
        /// </summary>
        public const Int32 METHOD_PEAK_TIME_DEFAULT = METHOD_PEAK_TIME_MIN;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 測光モード
        /// </summary>
        private PhotoModeKind photoMode = PHOTO_MODE_DEFAULT;
        /// <summary>
        /// 測定時間（msec）
        /// </summary>
        private Int32 measTime = METHOD_AREA_TIME_DEFAULT;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 測光モードの設定／取得
        /// </summary>
        public PhotoModeKind PhotoMode
        {
            get
            {
                return this.photoMode;
            }
            set
            {
                this.photoMode = value;
            }
        }

        /// <summary>
        /// 測定時間の設定／取得
        /// </summary>
        public Int32 MeasTime
        {
            get
            {
                return this.measTime;
            }
            set
            {
                this.measTime = value;
            }
        }

        #endregion
	}
	 
}
 
