using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// ���A�����ݒ�
    /// </summary>
	public class WashDispVolParameter : AttachmentParameter
	{
        #region [�萔��`]

        /// <summary>
        /// ���t�@�ŏ��l�ݒ�i��L�j
        /// </summary>
        public const Int32 DISP_VOLUME_WASH_MIN = 0;
        /// <summary>
        /// ���t�@�ő�l�ݒ�i��L�j
        /// </summary>
        public const Int32 DISP_VOLUME_WASH_MAX = 999;
        /// <summary>
        /// �v���g���K�@�ŏ��l�ݒ�i��L�j
        /// </summary>
        public const Int32 DISP_VOLUME_PRE_TRIG_MIN = 0;
        /// <summary>
        /// �v���g���K�@�ő�l�ݒ�i��L�j
        /// </summary>
        public const Int32 DISP_VOLUME_PRE_TRIG_MAX = 999;
        /// <summary>
        /// �g���K�@�ŏ��l�ݒ�i��L�j
        /// </summary>
        public const Int32 DISP_VOLUME_TRIG_MIN = 0;
        /// <summary>
        /// �g���K�@�ő�l�ݒ�i��L�j
        /// </summary>
        public const Int32 DISP_VOLUME_TRIG_MAX = 999;

        /// <summary>
        /// ���t�@�f�t�H���g�l�ݒ�i��L�j
        /// </summary>
        public const Int32 DISP_VOLUME_WASH_DEFAULT = 700;
        /// <summary>
        /// �v���g���K�@�f�t�H���g�l�ݒ�i��L�j
        /// </summary>
        public const Int32 DISP_VOLUME_PRE_TRIG_DEFAULT = DISP_VOLUME_PRE_TRIG_MIN;
        /// <summary>
        /// �g���K�@�f�t�H���g�l�ݒ�i��L�j
        /// </summary>
        public const Int32 DISP_VOLUME_TRIG_DEFAULT = DISP_VOLUME_TRIG_MIN;


        #endregion

        #region [�C���X�^���X�ϐ���`]

        /// <summary>
        /// ���t�����ʁi��L�j
        /// </summary>
        private Int32 dispVolWash = DISP_VOLUME_WASH_DEFAULT;
        /// <summary>
        /// �v���g���K�����ʁi��L�j
        /// </summary>
        private Int32 dispVolPreTrig = DISP_VOLUME_PRE_TRIG_DEFAULT;
        /// <summary>
        /// �g���K�����ʁi��L�j
        /// </summary>
        private Int32 dispVolTrig = DISP_VOLUME_TRIG_DEFAULT;

        #endregion

        #region [�v���p�e�B]

        /// <summary>
        /// ���t�����ʂ̐ݒ�^�擾
        /// </summary>
        public Int32 DispVolWash
        {
            get
            {
                return this.dispVolWash;
            }
            set
            {
                this.dispVolWash = value;
            }
        }
        /// <summary>
        /// �v���g���K�����ʂ̐ݒ�^�擾
        /// </summary>
        public Int32 DispVolPreTrig
        {
            get
            {
                return this.dispVolPreTrig;
            }
            set
            {
                this.dispVolPreTrig = value;
            }
        }
        /// <summary>
        /// �g���K�����ʂ̐ݒ�^�擾
        /// </summary>
        public Int32 DispVolTrig
        {
            get
            {
                return this.dispVolTrig;
            }
            set
            {
                this.dispVolTrig = value;
            }
        }

        #endregion
	}
	 
}
 
