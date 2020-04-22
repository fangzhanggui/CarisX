using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// �T���v�����b�N�ːݕ��J�o�[�I�[�v���G���[�ʒm���Ԑݒ�
    /// </summary>
	public class SampleLoaderCoverOpenErrorNotificationTimeParameter : AttachmentParameter
	{
        #region [�萔��`]

        /// <summary>
        /// �J�o�[�I�[�v���G���[�ʒm���ԁ@�ŏ��ݒ莞���i���j
        /// </summary>
        public const Int32 COVER_OPEN_TIME_MIN = 1;
        /// <summary>
        /// �J�o�[�I�[�v���G���[�ʒm���ԁ@�ő�ݒ莞���i���j
        /// </summary>
        public const Int32 COVER_OPEN_TIME_MAX = 120;

        /// <summary>
        /// �J�o�[�I�[�v���G���[�ʒm���ԁ@�f�t�H���g�ݒ莞���i���j
        /// </summary>
        public const Int32 COVER_OPEN_TIME_DEFAULT = 5;

        #endregion

        #region [�C���X�^���X�ϐ���`]

        /// <summary>
        /// �J�o�[�I�[�v���G���[�ʒm���ԁi���j
        /// </summary>
        private Int32 sampCoverOpenTime = COVER_OPEN_TIME_DEFAULT;

        #endregion

        #region [�v���p�e�B]

        /// <summary>
        /// �J�o�[�I�[�v���G���[�ʒm���Ԃ̐ݒ�^�擾
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
 
