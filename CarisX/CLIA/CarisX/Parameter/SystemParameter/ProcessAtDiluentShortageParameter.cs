using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// ��߉t�s�����̕��͏�Ԑݒ�
    /// </summary>
	public class ProcessAtDiluentShortageParameter : AttachmentParameter
	{
        #region [�萔��`]

        /// <summary>
        /// ���͂̏�� ���f
        /// </summary>
        public const Boolean SAMPLING_STOP = true;
        /// <summary>
        /// ���͂̏�� ���s
        /// </summary>
        public const Boolean CONTINUE = false;

        /// <summary>
        /// ���͂̏�ԁ@�f�t�H���g�l�ݒ�
        /// </summary>
        public const Boolean DILU_ASSAY_STATUS_DEFAULT = SAMPLING_STOP;

        #endregion

        #region [�C���X�^���X�ϐ���`]

        /// <summary>
        /// ��߉t�s�����̕��͂̏��
        /// </summary>
        private Boolean diluShortAssayStatus = DILU_ASSAY_STATUS_DEFAULT;

        #endregion

        #region [�v���p�e�B]

        /// <summary>
        /// ��߉t�s�����̕��͂̏�Ԃ̐ݒ�^�擾
        /// </summary>
        public Boolean DiluShortAssayStatus
        {
            get
            {
                return this.diluShortAssayStatus;
            }
            set
            {
                this.diluShortAssayStatus = value;
            }
        }

        #endregion
	}
	 
}
 
