using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// ���������グ�^�C�}�[�ݒ�
    /// </summary>
	public class AutoStartupTimerParameter : AttachmentParameter
	{
        #region [�萔��`]

        /// <summary>
        /// �j���̐ݒ�
        /// </summary>
        [Flags]
        public enum DayOfWeek : int
        {
            /// <summary>
            /// ���j��
            /// </summary>
            Mon = 0x01,
            /// <summary>
            /// �Ηj��
            /// </summary>
            Tue = 0x02,
            /// <summary>
            /// ���j��
            /// </summary>
            Wed = 0x04,
            /// <summary>
            /// �ؗj��
            /// </summary>
            Thu = 0x08,
            /// <summary>
            /// ���j��
            /// </summary>
            Fri = 0x10,
            /// <summary>
            /// �y�j��
            /// </summary>
            Sat = 0x20,
            /// <summary>
            /// ���j��
            /// </summary>
            Sun = 0x40
        }

        /// <summary>
        /// �����N���^�C�}�@�ŏ��ݒ莞�ԁi���j
        /// </summary>
        public const Int32 AUTO_STARTUP_HOUR_MIN = 0;
        /// <summary>
        /// �����N���^�C�}�@�ő�ݒ莞�ԁi���j
        /// </summary>
        public const Int32 AUTO_STARTUP_HOUR_MAX = 23;

        /// <summary>
        /// �����N���^�C�}�@�ő�ݒ莞�ԁi���j
        /// </summary>
        public const Int32 AUTO_STARTUP_MINUTE_MIN = 0;
        /// <summary>
        /// �����N���^�C�}�@�ő�ݒ莞�ԁi���j
        /// </summary>
        public const Int32 AUTO_STARTUP_MINUTE_MAX = 59;

        /// <summary>
        /// �j��  �f�t�H���g�l�ݒ�
        /// </summary>
        public const DayOfWeek DAY_OF_WEEK_DEFAULT = (DayOfWeek)0x00;
        /// <summary>
        /// �����N���^�C�}�@�f�t�H���g�ݒ莞�ԁi���j
        /// </summary>
        public const Int32 AUTO_STARTUP_HOUR_DEFAULT = AUTO_STARTUP_HOUR_MIN;
        /// <summary>
        /// �����N���^�C�}�@�f�t�H���g�ݒ莞�ԁi���j
        /// </summary>
        public const Int32 AUTO_STARTUP_MINUTE_DEFAULT = AUTO_STARTUP_MINUTE_MIN;

        #endregion

        #region[�v���p�e�B]

        /// <summary>
        /// ���������グ���i�j���j�@�ݒ�/�擾
        /// </summary>
        public DayOfWeek SelectDayOfWeek { get; set; } = DAY_OF_WEEK_DEFAULT;

        /// <summary>
        /// ���������グ���ԁi���j�@�ݒ�/�擾
        /// </summary>
        public Int32 AutoStartupHour { get; set; } = AUTO_STARTUP_HOUR_DEFAULT;

        /// <summary>
        /// ���������グ���ԁi���j�@�ݒ�/�擾
        /// </summary>
        public Int32 AutoStartupMinute { get; set; } = AUTO_STARTUP_MINUTE_DEFAULT;
        #endregion
    }
	 
}
 
