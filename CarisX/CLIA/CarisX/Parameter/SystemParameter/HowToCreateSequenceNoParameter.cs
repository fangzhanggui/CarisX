using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// �V�[�P���X�ԍ����ԕ��@�ݒ�
    /// </summary>
	public class HowToCreateSequenceNoParameter : AttachmentParameter
	{
        #region [�萔��`]

        /// <summary>
        /// �J�n�V�[�P���X�ԍ��i��ʌ��́j�@�ŏ��l�ݒ�
        /// </summary>
        public const Int32 START_SEQ_NO_PAT_MIN = 1;
        /// <summary>
        /// �J�n�V�[�P���X�ԍ��i��ʌ��́j�@�ő�l�ݒ�
        /// </summary>
        public const Int32 START_SEQ_NO_PAT_MAX = 9999;
        /// <summary>
        /// �J�n�V�[�P���X�ԍ��i�D�挟�́j�@�ŏ��l�ݒ�
        /// </summary>
        public const Int32 START_SEQ_NO_STAT_MIN = 1;
        /// <summary>
        /// �J�n�V�[�P���X�ԍ��i�D�挟�́j�@�ő�l�ݒ�
        /// </summary>
        public const Int32 START_SEQ_NO_STAT_MAX = 9999;
        /// <summary>
        /// �J�n�V�[�P���X�ԍ��i���x�Ǘ����́j�@�ŏ��l�ݒ�
        /// </summary>
        public const Int32 START_SEQ_NO_CTRL_MIN = 1;
        /// <summary>
        /// �J�n�V�[�P���X�ԍ��i���x�Ǘ����́j�@�ő�l�ݒ�
        /// </summary>
        public const Int32 START_SEQ_NO_CTRL_MAX = 9999;
        /// <summary>
        /// �J�n�V�[�P���X�ԍ��i�L�����u���[�^�j�@�ŏ��l�ݒ�
        /// </summary>
        public const Int32 START_SEQ_NO_CALIB_MIN = 1;
        /// <summary>
        /// �J�n�V�[�P���X�ԍ��i�L�����u���[�^�j�@�ő�l�ݒ�
        /// </summary>
        public const Int32 START_SEQ_NO_CALIB_MAX = 9999;

        /// <summary>
        /// �J�n�V�[�P���X�ԍ��i��ʌ��́j�f�t�H���g�l�ݒ�
        /// </summary>
        public const Int32 START_SEQ_NO_PAT_DEFAULT = START_SEQ_NO_PAT_MIN;
        /// <summary>
        /// �J�n�V�[�P���X�ԍ��i�D�挟�́j�f�t�H���g�l�ݒ�
        /// </summary>
        public const Int32 START_SEQ_NO_STAT_DEFAULT = 7001;
        /// <summary>
        /// �J�n�V�[�P���X�ԍ��i���x�Ǘ����́j�f�t�H���g�l�ݒ�
        /// </summary>
        public const Int32 START_SEQ_NO_CTRL_DEFAULT = 8001;
        /// <summary>
        /// �J�n�V�[�P���X�ԍ��i�L�����u���[�^�j�f�t�H���g�l�ݒ�
        /// </summary>
        public const Int32 START_SEQ_NO_CALIB_DEFAULT = 9001;

        #endregion

        #region [�C���X�^���X�ϐ���`]

        /// <summary>
        /// �J�n�V�[�P���X�ԍ��i��ʌ��́j
        /// </summary>
        private Int32 startSeqNoPat = START_SEQ_NO_PAT_DEFAULT;

        /// <summary>
        /// �J�n�V�[�P���X�ԍ��i�D�挟�́j
        /// </summary>
        private Int32 startSeqNoStat = START_SEQ_NO_STAT_DEFAULT;

        /// <summary>
        /// �J�n�V�[�P���X�ԍ��i���x�Ǘ����́j
        /// </summary>
        private Int32 startSeqNoCtrl = START_SEQ_NO_CTRL_DEFAULT;

        /// <summary>
        /// �J�n�V�[�P���X�ԍ��i�L�����u���[�^���́j
        /// </summary>
        private Int32 startSeqNoCalib = START_SEQ_NO_CALIB_DEFAULT;

        #endregion

        #region [�v���p�e�B]

        /// <summary>
        /// �J�n�V�[�P���X�ԍ��i��ʌ��́j�̐ݒ�^�擾
        /// </summary>
        public Int32 StartSeqNoPat
        {
            get
            {
                return this.startSeqNoPat;
            }
            set
            {
                this.startSeqNoPat = value;
            }
        }

        /// <summary>
        /// �J�n�V�[�P���X�ԍ��i�D�挟�́j�̐ݒ�^�擾
        /// </summary>
        public Int32 StartSeqNoStat
        {
            get
            {
                return this.startSeqNoStat;
            }
            set
            {
                this.startSeqNoStat = value;
            }
        }

        /// <summary>
        /// �J�n�V�[�P���X�ԍ��i���x�Ǘ����́j�̐ݒ�^�擾
        /// </summary>
        public Int32 StartSeqNoCtrl
        {
            get
            {
                return this.startSeqNoCtrl;
            }
            set
            {
                this.startSeqNoCtrl = value;
            }
        }

        /// <summary>
        /// �J�n�V�[�P���X�ԍ��i�L�����u���[�^�j�̐ݒ�^�擾
        /// </summary>
        public Int32 StartSeqNoCalib
        {
            get
            {
                return this.startSeqNoCalib;
            }
            set
            {
                this.startSeqNoCalib = value;
            }
        }

        #endregion
	}
	 
}
 
