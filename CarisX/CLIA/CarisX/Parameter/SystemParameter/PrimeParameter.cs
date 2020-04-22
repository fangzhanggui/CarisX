using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// �v���C���ݒ�
    /// </summary>
	public class PrimeParameter : AttachmentParameter
	{
        #region [�萔��`]

        /// <summary>
        /// ��߉t�v���C���񐔁@�ŏ��l�ݒ�
        /// </summary>
        public const Int32 PRIME_COUNT_DILU_MIN = 1;
        /// <summary>
        /// ��߉t�v���C���񐔁@�ő�l�ݒ�
        /// </summary>
        public const Int32 PRIME_COUNT_DILU_MAX = 99;
        /// <summary>
        /// R1�v���C���񐔁@�ŏ��l�ݒ�
        /// </summary>
        public const Int32 PRIME_COUNT_R1_MIN = 1;
        /// <summary>
        /// R1�v���C���񐔁@�ő�l�ݒ�
        /// </summary>
        public const Int32 PRIME_COUNT_R1_MAX = 99;
        /// <summary>
        /// R2�v���C���񐔁@�ŏ��l�ݒ�
        /// </summary>
        public const Int32 PRIME_COUNT_R2_MIN = 1;
        /// <summary>
        /// R2�v���C���񐔁@�ő�l�ݒ�
        /// </summary>
        public const Int32 PRIME_COUNT_R2_MAX = 99;
        /// <summary>
        /// B/F1�v���C���񐔁@�ŏ��l�ݒ�
        /// </summary>
        public const Int32 PRIME_COUNT_BF1_MIN = 1;
        /// <summary>
        /// B/F1�v���C���񐔁@�ő�l�ݒ�
        /// </summary>
        public const Int32 PRIME_COUNT_BF1_MAX = 99;
        /// <summary>
        /// B/F2�v���C���񐔁@�ŏ��l�ݒ�
        /// </summary>
        public const Int32 PRIME_COUNT_BF2_MIN = 1;
        /// <summary>
        /// B/F2�v���C���񐔁@�ő�l�ݒ�
        /// </summary>
        public const Int32 PRIME_COUNT_BF2_MAX = 99;
        /// <summary>
        /// �v���g���K�v���C���񐔁@�ŏ��l�ݒ�
        /// </summary>
        public const Int32 PRIME_COUNT_PRE_TRIG_MIN = 1;
        /// <summary>
        /// �v���g���K�v���C���񐔁@�ő�l�ݒ�
        /// </summary>
        public const Int32 PRIME_COUNT_PRE_TRIG_MAX = 99;
        /// <summary>
        /// �g���K�v���C���񐔁@�ŏ��l�ݒ�
        /// </summary>
        public const Int32 PRIME_COUNT_TRIG_MIN = 1;
        /// <summary>
        /// �g���K�v���C���񐔁@�ő�l�ݒ�
        /// </summary>
        public const Int32 PRIME_COUNT_TRIG_MAX = 99;

        /// <summary>
        /// ��߉t�v���C���ʁ@�ŏ��l�ݒ�i��L�j
        /// </summary>
        public const Int32 PRIME_VOLUME_DILU_MIN = 0;
        /// <summary>
        /// ��߉t�v���C���ʁ@�ő�l�ݒ�i��L�j
        /// </summary>
        public const Int32 PRIME_VOLUME_DILU_MAX = 9999;
        /// <summary>
        /// R1�v���C���ʁ@�ŏ��l�ݒ�i��L�j
        /// </summary>
        public const Int32 PRIME_VOLUME_R1_MIN = 0;
        /// <summary>
        /// R1�v���C���ʁ@�ő�l�ݒ�i��L�j
        /// </summary>
        public const Int32 PRIME_VOLUME_R1_MAX = 9999;
        /// <summary>
        /// R2�v���C���ʁ@�ŏ��l�ݒ�i��L�j
        /// </summary>
        public const Int32 PRIME_VOLUME_R2_MIN = 0;
        /// <summary>
        /// R2�v���C���ʁ@�ő�l�ݒ�i��L�j
        /// </summary>
        public const Int32 PRIME_VOLUME_R2_MAX = 9999;
        /// <summary>
        /// B/F1�v���C���ʁ@�ŏ��l�ݒ�i��L�j
        /// </summary>
        public const Int32 PRIME_VOLUME_BF1_MIN = 0;
        /// <summary>
        /// B/F1�v���C���ʁ@�ő�l�ݒ�i��L�j
        /// </summary>
        public const Int32 PRIME_VOLUME_BF1_MAX = 9999;
        /// <summary>
        /// B/F2�v���C���ʁ@�ŏ��l�ݒ�i��L�j
        /// </summary>
        public const Int32 PRIME_VOLUME_BF2_MIN = 0;
        /// <summary>
        /// B/F2�v���C���ʁ@�ő�l�ݒ�i��L�j
        /// </summary>
        public const Int32 PRIME_VOLUME_BF2_MAX = 9999;
        /// <summary>
        /// �v���g���K�v���C���ʁ@�ŏ��l�ݒ�i��L�j
        /// </summary>
        public const Int32 PRIME_VOLUME_PRE_TRIG_MIN = 0;
        /// <summary>
        /// �v���g���K�v���C���ʁ@�ő�l�ݒ�i��L�j
        /// </summary>
        public const Int32 PRIME_VOLUME_PRE_TRIG_MAX = 9999;
        /// <summary>
        /// �g���K�v���C���ʁ@�ŏ��l�ݒ�i��L�j
        /// </summary>
        public const Int32 PRIME_VOLUME_TRIG_MIN = 0;
        /// <summary>
        /// �g���K�v���C���ʁ@�ő�l�ݒ�i��L�j
        /// </summary>
        public const Int32 PRIME_VOLUME_TRIG_MAX = 9999;

        /// <summary>
        /// ��߉t�v���C���񐔁@�f�t�H���g�l�ݒ�
        /// </summary>
        public const Int32 PRIME_COUNT_DILU_DEFAULT = PRIME_COUNT_DILU_MIN;
        /// <summary>
        /// R1�v���C���񐔁@�f�t�H���g�l�ݒ�
        /// </summary>
        public const Int32 PRIME_COUNT_R1_DEFAULT = PRIME_COUNT_R1_MIN;
        /// <summary>
        /// R2�v���C���񐔁@�f�t�H���g�l�ݒ�
        /// </summary>
        public const Int32 PRIME_COUNT_R2_DEFAULT = PRIME_COUNT_R2_MIN;
        /// <summary>
        /// B/F1�v���C���񐔁@�f�t�H���g�l�ݒ�
        /// </summary>
        public const Int32 PRIME_COUNT_BF1_DEFAULT = PRIME_COUNT_BF1_MIN;
        /// <summary>
        /// B/F2�v���C���񐔁@�f�t�H���g�l�ݒ�
        /// </summary>
        public const Int32 PRIME_COUNT_BF2_DEFAULT = PRIME_COUNT_BF2_MIN;
        /// <summary>
        /// �v���g���K�v���C���񐔁@�f�t�H���g�l
        /// </summary>
        public const Int32 PRIME_COUNT_PRE_TRIG_DEFAULT = PRIME_COUNT_PRE_TRIG_MIN;
        /// <summary>
        /// �g���K�v���C���񐔁@�f�t�H���g�l�ݒ�
        /// </summary>
        public const Int32 PRIME_COUNT_TRIG_DEFAULT = PRIME_COUNT_TRIG_MIN;
        /// <summary>
        /// ��߉t�v���C���ʁ@�f�t�H���g�l�ݒ�i��L�j
        /// </summary>
        public const Int32 PRIME_VOLUME_DILU_DEFAULT = PRIME_VOLUME_DILU_MIN;
        /// <summary>
        /// R1�v���C���ʁ@�f�t�H���g�l�ݒ�i��L�j
        /// </summary>
        public const Int32 PRIME_VOLUME_R1_DEFAULT = PRIME_VOLUME_R1_MIN;
        /// <summary>
        /// R2�v���C���ʁ@�f�t�H���g�l�ݒ�i��L�j
        /// </summary>
        public const Int32 PRIME_VOLUME_R2_DEFAULT = PRIME_VOLUME_R2_MIN;
        /// <summary>
        /// B/F1�v���C���ʁ@�f�t�H���g�l�ݒ�i��L�j
        /// </summary>
        public const Int32 PRIME_VOLUME_BF1_DEFAULT = PRIME_VOLUME_BF1_MIN;
        /// <summary>
        /// B/F2�v���C���ʁ@�f�t�H���g�l�ݒ�i��L�j
        /// </summary>
        public const Int32 PRIME_VOLUME_BF2_DEFAULT = PRIME_VOLUME_BF2_MIN;
        /// <summary>
        /// �v���g���K�v���C���ʁ@�f�t�H���g�l�ݒ�i��L�j
        /// </summary>
        public const Int32 PRIME_VOLUME_PRE_TRIG_DEFAULT = PRIME_VOLUME_PRE_TRIG_MIN;
        /// <summary>
        /// �g���K�v���C���ʁ@�f�t�H���g�l�ݒ�i��L�j
        /// </summary>
        public const Int32 PRIME_VOLUME_TRIG_DEFAULT = PRIME_VOLUME_TRIG_MIN;

        #endregion

        #region [�C���X�^���X�ϐ���`]

        /// <summary>
        /// ��߉t�v���C����
        /// </summary>
        private Int32 primeCountDilu = PRIME_COUNT_DILU_DEFAULT;
        /// <summary>
        /// R1�v���C����
        /// </summary>
        private Int32 primeCountR1 = PRIME_COUNT_R1_DEFAULT;
        /// <summary>
        /// R2�v���C����
        /// </summary>
        private Int32 primeCountR2 = PRIME_COUNT_R2_DEFAULT;
        /// <summary>
        /// B/F1�v���C����
        /// </summary>
        private Int32 primeCountBF1 = PRIME_COUNT_BF1_DEFAULT;
        /// <summary>
        /// B/F2�v���C����
        /// </summary>
        private Int32 primeCountBF2 = PRIME_COUNT_BF2_DEFAULT;
        /// <summary>
        /// �v���g���K�v���C����
        /// </summary>
        private Int32 primeCountPreTrig = PRIME_COUNT_PRE_TRIG_DEFAULT;
        /// <summary>
        /// �g���K�v���C����
        /// </summary>
        private Int32 primeCountTrig = PRIME_COUNT_TRIG_DEFAULT;

        /// <summary>
        /// ��߉t�v���C���ʁi��L�j
        /// </summary>
        private Int32 primeVolumeDilu = PRIME_VOLUME_DILU_DEFAULT;
        /// <summary>
        /// R1�v���C���ʁi��L�j
        /// </summary>
        private Int32 primeVolumeR1 = PRIME_VOLUME_R1_DEFAULT;
        /// <summary>
        /// R2�v���C���ʁi��L�j
        /// </summary>
        private Int32 primeVolumeR2 = PRIME_VOLUME_R2_DEFAULT;
        /// <summary>
        /// B/F1�v���C���ʁi��L�j
        /// </summary>
        private Int32 primeVolumeBF1 = PRIME_VOLUME_BF1_DEFAULT;
        /// <summary>
        /// B/F2�v���C���ʁi��L�j
        /// </summary>
        private Int32 primeVolumeBF2 = PRIME_VOLUME_BF2_DEFAULT;
        /// <summary>
        /// �v���g���K�v���C���ʁi��L�j
        /// </summary>
        private Int32 primeVolumePreTrig = PRIME_VOLUME_PRE_TRIG_DEFAULT;
        /// <summary>
        /// �g���K�v���C���ʁi��L�j
        /// </summary>
        private Int32 primeVolumeTrig = PRIME_VOLUME_TRIG_DEFAULT;

        #endregion

        #region [�v���p�e�B]

        /// <summary>
        /// ��߃v���C���񐔂̐ݒ�^�擾
        /// </summary>
        public Int32 PrimeCountDilu
        {
            get
            {
                return this.primeCountDilu;
            }
            set
            {
                this.primeCountDilu = value;
            }
        }

        /// <summary>
        /// R1�v���C���񐔂̐ݒ�^�擾
        /// </summary>
        public Int32 PrimeCountR1
        {
            get
            {
                return this.primeCountR1;
            }
            set
            {
                this.primeCountR1 = value;
            }
        }

        /// <summary>
        /// R2�v���C���񐔂̐ݒ�^�擾
        /// </summary>
        public Int32 PrimeCountR2
        {
            get
            {
                return this.primeCountR2;
            }
            set
            {
                this.primeCountR2 = value;
            }
        }

        /// <summary>
        /// B/F1�v���C���񐔂̐ݒ�^�擾
        /// </summary>
        public Int32 PrimeCountBF1
        {
            get
            {
                return this.primeCountBF1;
            }
            set
            {
                this.primeCountBF1 = value;
            }
        }

        /// <summary>
        /// B/F2�v���C���񐔂̐ݒ�^�擾
        /// </summary>
        public Int32 PrimeCountBF2
        {
            get
            {
                return this.primeCountBF2;
            }
            set
            {
                this.primeCountBF2 = value;
            }
        }

        /// <summary>
        /// �v���g���K�v���C���񐔂̐ݒ�^�擾
        /// </summary>
        public Int32 PrimeCountPreTrig
        {
            get
            {
                return this.primeCountPreTrig;
            }
            set
            {
                this.primeCountPreTrig = value;
            }
        }

        /// <summary>
        /// �g���K�v���C���񐔂̐ݒ�^�擾
        /// </summary>
        public Int32 PrimeCountTrig
        {
            get
            {
                return this.primeCountTrig;
            }
            set
            {
                this.primeCountTrig = value;
            }
        }

        /// <summary>
        /// ��߃v���C���ʂ̐ݒ�^�擾
        /// </summary>
        public Int32 PrimeVolumeDilu
        {
            get
            {
                return this.primeVolumeDilu;
            }
            set
            {
                this.primeVolumeDilu = value;
            }
        }

        /// <summary>
        /// R1�v���C���ʂ̐ݒ�^�擾
        /// </summary>
        public Int32 PrimeVolumeR1
        {
            get
            {
                return this.primeVolumeR1;
            }
            set
            {
                this.primeVolumeR1 = value;
            }
        }

        /// <summary>
        /// R2�v���C���ʂ̐ݒ�^�擾
        /// </summary>
        public Int32 PrimeVolumeR2
        {
            get
            {
                return this.primeVolumeR2;
            }
            set
            {
                this.primeVolumeR2 = value;
            }
        }

        /// <summary>
        /// B/F1�v���C���ʂ̐ݒ�^�擾
        /// </summary>
        public Int32 PrimeVolumeBF1
        {
            get
            {
                return this.primeVolumeBF1;
            }
            set
            {
                this.primeVolumeBF1 = value;
            }
        }

        /// <summary>
        /// B/F2�v���C���ʂ̐ݒ�^�擾
        /// </summary>
        public Int32 PrimeVolumeBF2
        {
            get
            {
                return this.primeVolumeBF2;
            }
            set
            {
                this.primeVolumeBF2 = value;
            }
        }

        /// <summary>
        /// �v���g���K�v���C���ʂ̐ݒ�^�擾
        /// </summary>
        public Int32 PrimeVolumePreTrig
        {
            get
            {
                return this.primeVolumePreTrig;
            }
            set
            {
                this.primeVolumePreTrig = value;
            }
        }

        /// <summary>
        /// �g���K�v���C���ʂ̐ݒ�^�擾
        /// </summary>
        public Int32 PrimeVolumeTrig
        {
            get
            {
                return this.primeVolumeTrig;
            }
            set
            {
                this.primeVolumeTrig = value;
            }
        }

        #endregion
	}
	 
}
 
