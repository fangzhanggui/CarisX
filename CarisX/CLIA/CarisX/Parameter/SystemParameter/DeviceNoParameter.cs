using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// ���uNo.�ݒ�
    /// </summary>
	public class DeviceNoParameter : AttachmentParameter
	{
        #region [�萔��`]

        /// <summary>
        /// ���uNo.�@�ŏ��l�ݒ�
        /// </summary>
        public const Int32 DEVICE_NO_MIN = 0;
        /// <summary>
        /// ���uNo.�@�ő�l�ݒ�
        /// </summary>
        public const Int32 DEVICE_NO_MAX = 9;

        /// <summary>
        /// ���uNo.�@�f�t�H���g�ݒ�
        /// </summary>
        public const Int32 DEVICE_NO_DEFAULT = 1;

        #endregion

        #region [�v���p�e�B]

        /// <summary>
        /// ���uNo.
        /// </summary>
        public Int32 DeviceNo { get; set; } = DEVICE_NO_DEFAULT;

        #endregion
	}
	 
}
 
