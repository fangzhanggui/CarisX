using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// ���̃o�[�R�[�h���[�_�[�ݒ�
    /// </summary>
	public class SampleBCRParameter : AttachmentParameter
	{
        #region [�萔��`]

        /// <summary>
        /// ����ID�����@�ŏ������ݒ�
        /// </summary>
        public const Int32 RACK_ID_SAMP_DIGIT_MIN = 1;
        /// <summary>
        /// ����ID�����@�ő包���ݒ�
        /// </summary>
        public const Int32 RACK_ID_SAMP_DIGIT_MAX = 16;

        /// <summary>
        /// �o�[�R�[�h��ސݒ�
        /// </summary>
        public enum BarcodeKind
        {
            /// <summary>
            /// 1)ITF, NW-7, CODE39, CODE128
            /// </summary>
            Type1,
            /// <summary>
            /// 2)ITF, 2of5, CODE39, CODE128
            /// </summary>
            Type2,
            /// <summary>
            /// 3)ITF, NW-7, 2of5, CODE128
            /// </summary>
            Type3,
            /// <summary>
            /// 4)ITF, NW-7, CODE39, 2of5
            /// </summary>
            Type4
        }

        /// <summary>
        /// ����ID�Œ蒷�L���@�f�t�H���g�ݒ�
        /// </summary>
        public const Boolean RACK_ID_SAMP_FIX_DEFAULT = false;
        /// <summary>
        /// ����ID�����@�f�t�H���g�ݒ�
        /// </summary>
        public const Int32 RACK_ID_SAMP_DIGIT_DEFAULT = 12;
        /// <summary>
        /// C/D�L�����N�^�]���L���@�f�t�H���g�ݒ�
        /// </summary>
        public const Boolean CD_CHAR_TRANS_DEFAULT = true;
        /// <summary>
        /// C/D�`�F�b�N�L���@�f�t�H���g�ݒ�
        /// </summary>
        public const Boolean CD_CHECK_DEFAULT = true;
        /// <summary>
        /// ST/SP�]���L���@�f�t�H���g�ݒ�
        /// </summary>
        public const Boolean STSP_TRANS_DEFAULT = false;
        /// <summary>
        /// �o�[�R�[�h��ށ@�f�t�H���g�ݒ�
        /// </summary>
        public const BarcodeKind BARCODE_DEFAULT = BarcodeKind.Type1;

        #endregion

        #region [�C���X�^���X�ϐ���`]

        /// <summary>
        /// ����ID���Œ蒷�ł��邩
        /// </summary>
        private Boolean usableRackIDSampFixedLength = RACK_ID_SAMP_FIX_DEFAULT;
        /// <summary>
        /// ����ID����
        /// </summary>
        private Int32 rackIDSampDigit = RACK_ID_SAMP_DIGIT_DEFAULT;
        /// <summary>
        /// C/D�L�����N�^�]��
        /// </summary>
        private Boolean usableCDCharTrans = CD_CHAR_TRANS_DEFAULT;
        /// <summary>
        /// C/D�`�F�b�N
        /// </summary>
        private Boolean usableCDCheck = CD_CHECK_DEFAULT;
        /// <summary>
        /// ST/SP�]��
        /// </summary>
        private Boolean usableSTSPTrans = STSP_TRANS_DEFAULT;
        /// <summary>
        /// �o�[�R�[�h���
        /// </summary>
        private BarcodeKind selectBCKind = BARCODE_DEFAULT;

        #endregion

        #region [�v���p�e�B]

        /// <summary>
        /// ����ID�̌Œ蒷�L���̐ݒ�^�擾
        /// </summary>
        public Boolean UsableRackIDSampFixedLength
        {
            get
            {
                return this.usableRackIDSampFixedLength;
            }
            set
            {
                this.usableRackIDSampFixedLength = value;
            }
        }

        /// <summary>
        /// ����ID�����̐ݒ�^�擾
        /// </summary>
        public Int32 RackIDSampDigit
        {
            get
            {
                return this.rackIDSampDigit;
            }
            set
            {
                this.rackIDSampDigit = value;
            }
        }

        /// <summary>
        /// C/D�L�����N�^�]���̐ݒ�^�擾
        /// </summary>
        public Boolean UsableCDCharTrans
        {
            get
            {
                return this.usableCDCharTrans;
            }
            set
            {
                this.usableCDCharTrans = value;
            }
        }

        /// <summary>
        /// C/D�`�F�b�N�̐ݒ�^�擾
        /// </summary>
        public Boolean UsableCDCheck
        {
            get
            {
                return this.usableCDCheck;
            }
            set
            {
                this.usableCDCheck = value;
            }
        }

        /// <summary>
        /// ST/SP�]���̐ݒ�^�擾
        /// </summary>
        public Boolean UsableSTSPTrans
        {
            get
            {
                return this.usableSTSPTrans;
            }
            set
            {
                this.usableSTSPTrans = value;
            }
        }

        /// <summary>
        /// �o�[�R�[�h��ނ̐ݒ�^�擾
        /// </summary>
        public BarcodeKind SelectBCKind
        {
            get
            {
                return this.selectBCKind;
            }
            set
            {
                this.selectBCKind = value;
            }
        }

        #endregion
	}
	 
}
 
