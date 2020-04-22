using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// �����ݒ�
    /// </summary>
	public class PhotometryParameter : AttachmentParameter
	{
        #region [�萔��`]

        /// <summary>
        /// �������[�h
        /// </summary>
        public enum PhotoModeKind
        {
            /// <summary>
            /// �ʐϖ@
            /// </summary>
            AREA,
            /// <summary>
            /// �s�[�N�@
            /// </summary>
            PEAK
        }

        /// <summary>
        /// �ʐϖ@�@�ŏ��ݒ莞�ԁimsec�j
        /// </summary>
        public const Int32 METHOD_AREA_TIME_MIN = 100;
        /// <summary>
        /// �ʐϖ@�@�ő�ݒ莞�ԁimsec�j
        /// </summary>
        public const Int32 METHOD_AREA_TIME_MAX = 1000;
        /// <summary>
        /// �s�[�N�@�@�ŏ��ݒ莞�ԁimsec�j
        /// </summary>
        public const Int32 METHOD_PEAK_TIME_MIN = 1;
        /// <summary>
        /// �s�[�N�@�@�ő�ݒ莞�ԁimsec�j
        /// </summary>
        public const Int32 METHOD_PEAK_TIME_MAX = 10;

        /// <summary>
        /// �������[�h�@�f�t�H���g�l�ݒ�
        /// </summary>
        public const PhotoModeKind PHOTO_MODE_DEFAULT = PhotoModeKind.AREA;
        /// <summary>
        /// �ʐϖ@�@�f�t�H���g�ݒ莞�ԁimsec�j
        /// </summary>
        public const Int32 METHOD_AREA_TIME_DEFAULT = METHOD_AREA_TIME_MIN;
        /// <summary>
        /// �s�[�N�@�@�f�t�H���g�ݒ莞�ԁimsec�j
        /// </summary>
        public const Int32 METHOD_PEAK_TIME_DEFAULT = METHOD_PEAK_TIME_MIN;

        #endregion

        #region [�C���X�^���X�ϐ���`]

        /// <summary>
        /// �������[�h
        /// </summary>
        private PhotoModeKind photoMode = PHOTO_MODE_DEFAULT;
        /// <summary>
        /// ���莞�ԁimsec�j
        /// </summary>
        private Int32 measTime = METHOD_AREA_TIME_DEFAULT;

        #endregion

        #region [�v���p�e�B]

        /// <summary>
        /// �������[�h�̐ݒ�^�擾
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
        /// ���莞�Ԃ̐ݒ�^�擾
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
 
