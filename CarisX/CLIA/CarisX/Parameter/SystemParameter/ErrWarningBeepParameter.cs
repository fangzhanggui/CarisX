using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// �G���[���A�x�����ݒ�
    /// </summary>
	public class ErrWarningBeepParameter : AttachmentParameter
    {
        #region [�萔��`]

        /// <summary>
        /// ���F�i�x���j�@�ŏ��l�ݒ�
        /// </summary>
        public const Int32 BEEP_WARNING_MIN = 1;
        /// <summary>
        /// ���F�i�x���j�@�ő�l�ݒ�
        /// </summary>
        public const Int32 BEEP_WARNING_MAX = 3;
        /// <summary>
        /// ���F�i�G���[�j�@�ŏ��l�ݒ�
        /// </summary>
        public const Int32 BEEP_ERROR_MIN = 1;
        /// <summary>
        /// ���F�i�G���[�j�@�ő�l�ݒ�
        /// </summary>
        public const Int32 BEEP_ERROR_MAX = 3;

        /// <summary>
        /// ����
        /// </summary>
        public enum BeepVolumeKind
        {
            /// <summary>
            /// ��
            /// </summary>
            None,
            /// <summary>
            /// ��
            /// </summary>
            Small,
            /// <summary>
            /// ��
            /// </summary>
            Middle,
            /// <summary>
            /// ��
            /// </summary>
            Large
        }

        /// <summary>
        /// ���F�i�x���j�@�f�t�H���g�l�ݒ�
        /// </summary>
        public const Int32 BEEP_WARNING_DEFAULT = BEEP_WARNING_MIN;
        /// <summary>
        /// ���F�i�G���[�j�@�f�t�H���g�l�ݒ�
        /// </summary>
        public const Int32 BEEP_ERROR_DEFAULT = 2;
        /// <summary>
        /// ���ʁ@�f�t�H���g�l�ݒ�
        /// </summary>
        public const BeepVolumeKind BEEP_VOLUME_DEFAULT = BeepVolumeKind.Small;

        #endregion

        #region [�R���X�g���N�^/�f�X�g���N�^]


        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public ErrWarningBeepParameter()
        {
            // �f�t�H���g�l �g�p
            this.Enable = true;
        }


        #endregion

        #region [�v���p�e�B]

        /// <summary>
        /// ���F�i�x���j
        /// </summary>
        public Int32 BeepWarning { get; set; } = BEEP_WARNING_DEFAULT;

        /// <summary>
        /// ���F�i�G���[�j
        /// </summary>
        public Int32 BeepError { get; set; } = BEEP_ERROR_DEFAULT;

        /// <summary>
        /// ����
        /// </summary>
        public BeepVolumeKind BeepVolume { get; set; } = BEEP_VOLUME_DEFAULT;

        #endregion
	}
	 
}
 
