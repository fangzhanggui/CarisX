using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// ����s�����̕��͂̏󋵐ݒ�
    /// </summary>
	public class ProcessAtReagentShortageParameter : AttachmentParameter
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
        /// ���򕪐͂̏�ԁ@�f�t�H���g�l�ݒ�
        /// </summary>
        public const Boolean REAG_ASSAY_STATUS_DEFAULT = SAMPLING_STOP;

        #endregion

        #region [�C���X�^���X�ϐ���`]

        /// <summary>
        /// ����s�����̕��͂̏��
        /// </summary>
        private Boolean reagShortAssayStatus = REAG_ASSAY_STATUS_DEFAULT;

        #endregion

        #region [�v���p�e�B]

        /// <summary>
        /// ����s�����̕��͂̏�Ԃ̐ݒ�^�擾
        /// </summary>
        public Boolean ReagShortAssayStatus
        {
            get
            {
                return this.reagShortAssayStatus;
            }
            set
            {
                this.reagShortAssayStatus = value;
            }
        }

        #endregion
	}
	 
}
 
