using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// �t���b�V���v���C���ݒ�
    /// </summary>
	public class FlushPrimeParameter : AttachmentParameter
    {
        #region [�萔��`]

        /// <summary>
        /// �t���b�V���v���C���N�����ԁi���j�@�ŏ��ݒ莞���i���j
        /// </summary>
        public const Int32 FLUSH_PRIME_TIME_MIN = 1;
        /// <summary>
        /// �t���b�V���v���C���N�����ԁi���j�@�ő�ݒ莞���i���j
        /// </summary>
        public const Int32 FLUSH_PRIME_TIME_MAX = 360;

        /// <summary>
        /// �t���b�V���v���C���N�����ԁi���j�@�f�t�H���g�ݒ莞���i���j
        /// </summary>
        public const Int32 FLUSH_PRIME_TIME_DEFAULT = 120;

        #endregion

        #region [�v���p�e�B]

        /// <summary>
        /// �t���b�V���v���C���N�����ԁi���j
        /// </summary>
        public Int32 FlushPrimeTime { get; set; } = FLUSH_PRIME_TIME_DEFAULT;

        #endregion
    }
	 
}
 
