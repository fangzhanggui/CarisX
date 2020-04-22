using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// �T�C�N�����Ԑݒ�
    /// </summary>
	public class CycleTimeParameter : AttachmentParameter
	{
        #region [�萔��`]

        /// <summary>
        /// �T�C�N���^�C�}�@�ŏ��ݒ莞�ԁi�b�j
        /// </summary>
        public const Int32 CYCLE_TIME_MIN = 18;
        /// <summary>
        /// �T�C�N���^�C�}�@�ő�ݒ莞�ԁi�b�j
        /// </summary>
        public const Int32 CYCLE_TIME_MAX = 60;

        /// <summary>
        /// �T�C�N���^�C�}�@�f�t�H���g�ݒ莞�ԁi�b�j
        /// </summary>
        public const Int32 CYCLE_TIME_DEFAULT = CYCLE_TIME_MIN;

        #endregion

        #region [�v���p�e�B]

        /// <summary>
        /// �T�C�N�����ԁi�b�j�@�ݒ�^�擾
        /// </summary>
        public Int32 CycleTime { get; set; } = CYCLE_TIME_DEFAULT;

        #endregion
    }
	 
}
 
