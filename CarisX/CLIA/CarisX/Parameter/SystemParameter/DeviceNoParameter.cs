using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// 装置No.設定
    /// </summary>
	public class DeviceNoParameter : AttachmentParameter
	{
        #region [定数定義]

        /// <summary>
        /// 装置No.　最小値設定
        /// </summary>
        public const Int32 DEVICE_NO_MIN = 0;
        /// <summary>
        /// 装置No.　最大値設定
        /// </summary>
        public const Int32 DEVICE_NO_MAX = 9;

        /// <summary>
        /// 装置No.　デフォルト設定
        /// </summary>
        public const Int32 DEVICE_NO_DEFAULT = 1;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 装置No.
        /// </summary>
        public Int32 DeviceNo { get; set; } = DEVICE_NO_DEFAULT;

        #endregion
	}
	 
}
 
