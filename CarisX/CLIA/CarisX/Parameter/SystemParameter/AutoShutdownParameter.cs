using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// �����V���b�g�_�E���ݒ�
    /// </summary>
    public class AutoShutdownParameter : AttachmentParameter
    {
        #region [�萔��`]

        /// <summary>
        /// �����V���b�g�_�E�����ԁi���j�@�ŏ��ݒ莞���i���j
        /// </summary>
        public const Int32 AUTO_SHUTDOWN_TIME_MIN = 1;
        /// <summary>
        /// �����V���b�g�_�E�����ԁi���j�@�ő�ݒ莞���i���j
        /// </summary>
        public const Int32 AUTO_SHUTDOWN_TIME_MAX = 9999;

        /// <summary>
        /// �����V���b�g�_�E�����ԁi���j�@�f�t�H���g�ݒ莞���i���j
        /// </summary>
        public const Int32 AUTO_SHUTDOWN_TIME_DEFAULT = 10;

        #endregion

        #region [�v���p�e�B]

        /// <summary>
        /// �����V���b�g�_�E�����ԁi���j
        /// </summary>
        public Int32 AutoShutdownTime { get; set; } = AUTO_SHUTDOWN_TIME_DEFAULT;

        #endregion
    }
}

