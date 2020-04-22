using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// ���򃍃b�g�ؑւ�莞�̏����ݒ�
    /// </summary>
	public class ProcessAtReagentLotChange : AttachmentParameter
	{
        #region [�萔��`]

        /// <summary>
        /// ���򃍃b�g�؂�ւ� ��~
        /// </summary>
        public const Boolean STOP = true;
        /// <summary>
        /// ���򃍃b�g�؂�ւ� ���s
        /// </summary>
        public const Boolean CONTINUE = false;

        /// <summary>
        /// ���򃍃b�g�؂�ւ������@�f�t�H���g�l�ݒ�
        /// </summary>
        public const Boolean REAG_LOT_PROC_DEFAULT = STOP;

        #endregion

        #region [�C���X�^���X�ϐ���`]

        /// <summary>
        /// ���򃍃b�g�؂�ւ�莞�̏���
        /// </summary>
        private Boolean reagLotChangeProc = REAG_LOT_PROC_DEFAULT;

        #endregion

        #region [�v���p�e�B]

        /// <summary>
        /// ���򃍃b�g�؂�ւ�莞�̏����̐ݒ�^�擾
        /// </summary>
        public Boolean ReagLotChangeProc
        {
            get
            {
                return this.reagLotChangeProc;
            }
            set
            {
                this.reagLotChangeProc = value;
            }
        }

        #endregion
	}
	 
}
 
