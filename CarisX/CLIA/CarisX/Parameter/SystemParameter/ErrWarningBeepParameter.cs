using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// エラー音、警告音設定
    /// </summary>
	public class ErrWarningBeepParameter : AttachmentParameter
    {
        #region [定数定義]

        /// <summary>
        /// 音色（警告）　最小値設定
        /// </summary>
        public const Int32 BEEP_WARNING_MIN = 1;
        /// <summary>
        /// 音色（警告）　最大値設定
        /// </summary>
        public const Int32 BEEP_WARNING_MAX = 3;
        /// <summary>
        /// 音色（エラー）　最小値設定
        /// </summary>
        public const Int32 BEEP_ERROR_MIN = 1;
        /// <summary>
        /// 音色（エラー）　最大値設定
        /// </summary>
        public const Int32 BEEP_ERROR_MAX = 3;

        /// <summary>
        /// 音量
        /// </summary>
        public enum BeepVolumeKind
        {
            /// <summary>
            /// 無
            /// </summary>
            None,
            /// <summary>
            /// 小
            /// </summary>
            Small,
            /// <summary>
            /// 中
            /// </summary>
            Middle,
            /// <summary>
            /// 大
            /// </summary>
            Large
        }

        /// <summary>
        /// 音色（警告）　デフォルト値設定
        /// </summary>
        public const Int32 BEEP_WARNING_DEFAULT = BEEP_WARNING_MIN;
        /// <summary>
        /// 音色（エラー）　デフォルト値設定
        /// </summary>
        public const Int32 BEEP_ERROR_DEFAULT = 2;
        /// <summary>
        /// 音量　デフォルト値設定
        /// </summary>
        public const BeepVolumeKind BEEP_VOLUME_DEFAULT = BeepVolumeKind.Small;

        #endregion

        #region [コンストラクタ/デストラクタ]


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ErrWarningBeepParameter()
        {
            // デフォルト値 使用
            this.Enable = true;
        }


        #endregion

        #region [プロパティ]

        /// <summary>
        /// 音色（警告）
        /// </summary>
        public Int32 BeepWarning { get; set; } = BEEP_WARNING_DEFAULT;

        /// <summary>
        /// 音色（エラー）
        /// </summary>
        public Int32 BeepError { get; set; } = BEEP_ERROR_DEFAULT;

        /// <summary>
        /// 音量
        /// </summary>
        public BeepVolumeKind BeepVolume { get; set; } = BEEP_VOLUME_DEFAULT;

        #endregion
	}
	 
}
 
