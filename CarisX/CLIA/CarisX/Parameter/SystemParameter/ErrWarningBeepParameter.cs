using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// 僄儔乕壒丄寈崘壒愝掕
    /// </summary>
	public class ErrWarningBeepParameter : AttachmentParameter
    {
        #region [掕悢掕媊]

        /// <summary>
        /// 壒怓乮寈崘乯丂嵟彫抣愝掕
        /// </summary>
        public const Int32 BEEP_WARNING_MIN = 1;
        /// <summary>
        /// 壒怓乮寈崘乯丂嵟戝抣愝掕
        /// </summary>
        public const Int32 BEEP_WARNING_MAX = 3;
        /// <summary>
        /// 壒怓乮僄儔乕乯丂嵟彫抣愝掕
        /// </summary>
        public const Int32 BEEP_ERROR_MIN = 1;
        /// <summary>
        /// 壒怓乮僄儔乕乯丂嵟戝抣愝掕
        /// </summary>
        public const Int32 BEEP_ERROR_MAX = 3;

        /// <summary>
        /// 壒検
        /// </summary>
        public enum BeepVolumeKind
        {
            /// <summary>
            /// 柍
            /// </summary>
            None,
            /// <summary>
            /// 彫
            /// </summary>
            Small,
            /// <summary>
            /// 拞
            /// </summary>
            Middle,
            /// <summary>
            /// 戝
            /// </summary>
            Large
        }

        /// <summary>
        /// 壒怓乮寈崘乯丂僨僼僅儖僩抣愝掕
        /// </summary>
        public const Int32 BEEP_WARNING_DEFAULT = BEEP_WARNING_MIN;
        /// <summary>
        /// 壒怓乮僄儔乕乯丂僨僼僅儖僩抣愝掕
        /// </summary>
        public const Int32 BEEP_ERROR_DEFAULT = 2;

        /// <summary>
        /// 【IssuesNo:6】默认提示音
        /// </summary>
        public const Int32 BEEP_HINT_DEFAULT = 3;
        /// <summary>
        /// 壒検丂僨僼僅儖僩抣愝掕
        /// </summary>
        public const BeepVolumeKind BEEP_VOLUME_DEFAULT = BeepVolumeKind.Small;

        #endregion

        #region [僐儞僗僩儔僋僞/僨僗僩儔僋僞]


        /// <summary>
        /// 僐儞僗僩儔僋僞
        /// </summary>
        public ErrWarningBeepParameter()
        {
            // 僨僼僅儖僩抣 巊梡
            this.Enable = true;
        }


        #endregion

        #region [僾儘僷僥傿]

        /// <summary>
        /// 壒怓乮寈崘乯
        /// </summary>
        public Int32 BeepWarning { get; set; } = BEEP_WARNING_DEFAULT;

        /// <summary>
        /// 壒怓乮僄儔乕乯
        /// </summary>
        public Int32 BeepError { get; set; } = BEEP_ERROR_DEFAULT;

        /// <summary>
        /// 【IssuesNo:6】提示音设置
        /// </summary>
        public Int32 BeepHint { get; set; } = BEEP_HINT_DEFAULT;

        /// <summary>
        /// 壒検
        /// </summary>
        public BeepVolumeKind BeepVolume { get; set; } = BEEP_VOLUME_DEFAULT;

        #endregion
	}
	 
}
 
