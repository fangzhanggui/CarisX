using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// ���̋z���G���[��̏���
    /// </summary>
	public class ProcessAfterSampleAspiratingErrorParameter : AttachmentParameter
	{
        #region [�萔��`]

        /// <summary>
        /// �ڋl�܂茟�m��̏����@�f���߂�
        /// </summary>
        public const Boolean PLUG_PUTBACK = true;
        /// <summary>
        /// �ڋl�܂茟�m��̏����@���s
        /// </summary>
        public const Boolean PLUG_CONTINUE = false;
        /// <summary>
        /// ���ʋz�����m��̏����@�f���߂�
        /// </summary>
        public const Boolean HALF_PUTBACK = true;
        /// <summary>
        /// ���ʋz�����m��̏����@���s
        /// </summary>
        public const Boolean HALF_CONTINUE = false;


        /// <summary>
        /// �G���[������̏����i�ڋl�܂茟�m�j�@�f�t�H���g�l�ݒ�
        /// </summary>
        public const Boolean ERR_PROC_PLUG_DEFAULT = PLUG_PUTBACK;
        /// <summary>
        /// �G���[������̏����i���ʋz�����m�j�@�f�t�H���g�l�ݒ�
        /// </summary>
        public const Boolean ERR_PROC_HALF_DEFAULT = HALF_PUTBACK;

        #endregion

        #region [�C���X�^���X�ϐ���`]

        /// <summary>
        /// �ڋl�܂茟�m��̏���
        /// </summary>
        private Boolean usablePutBack = ERR_PROC_PLUG_DEFAULT;

        /// <summary>
        /// ���ʋz�����m��̏���
        /// </summary>
        private Boolean usablePutBackAtHalf = ERR_PROC_HALF_DEFAULT;
        #endregion

        #region [�v���p�e�B]

        /// <summary>
        /// �ڋl�܂茟�m��̏����̐ݒ�^�擾
        /// </summary>
        public Boolean UsablePutBack
        {
            get
            {
                return this.usablePutBack;
            }
            set
            {
                this.usablePutBack = value;
            }
        }

        /// <summary>
        /// ���ʋz�����m��̏����̐ݒ�^�擾
        /// </summary>
        public Boolean UsablePutBackAtHalf
        {
            get
            {
                return this.usablePutBackAtHalf;
            }
            set
            {
                this.usablePutBackAtHalf = value;
            }
        }

        #endregion
	}
	 
}
 
