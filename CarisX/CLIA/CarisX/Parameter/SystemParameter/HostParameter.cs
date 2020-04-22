using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;
using System.IO.Ports;
using Oelco.Common.Comm;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// �z�X�g�ݒ�
    /// </summary>
	public class HostParameter : AttachmentParameter
	{
        #region [�萔��`]
        
        /// <summary>
        /// ���M�x�����ԁ@�ŏ��ݒ莞�ԁimsec�j
        /// </summary>
        public const Int32 SEND_DELAY_TIME_MIN = 0;
        /// <summary>
        /// ���M�x�����ԁ@�ő�ݒ莞�ԁimsec�j
        /// </summary>
        public const Int32 SEND_DELAY_TIME_MAX = 100;

        /// <summary>
        /// �{�[���[�g�@�f�t�H���g�l�ݒ�
        /// </summary>
        public const BaudRate BAUDRATE_DEFAULT = BaudRate.Br9600;
        /// <summary>
        /// �p���e�B�@�f�t�H���g�l�ݒ�
        /// </summary>
        public const Parity PARITY_DEFAULT = Parity.Even;
        /// <summary>
        /// �f�[�^���@�f�t�H���g�l�ݒ�
        /// </summary>
        public const DataBits DATA_LENGTH_DEFAULT = DataBits.Bit8;
        /// <summary>
        /// �X�g�b�v�r�b�g�@�f�t�H���g�l�ݒ�
        /// </summary>
        public const StopBitKind STOP_BIT_DEFAULT = StopBitKind.Bit1;
        /// <summary>
        /// COM�|�[�g�@�f�t�H���g�l�ݒ�
        /// </summary>
        public const String COMM_PORT_DEFAULT = "COM1";
        /// <summary>
        /// ���̃��A���^�C���o�͗L���@�f�t�H���g�l�ݒ�
        /// </summary>
        public const Boolean REALTIME_OUTPUT_SAMP_DEFAULT = true;
        /// <summary>
        /// ���x�Ǘ����̃��A���^�C���o�͗L���@�f�t�H���g�l�ݒ�
        /// </summary>
        public const Boolean REALTIME_OUTPUT_CTRL_DEFAULT = true;
        /// <summary>
        /// �L�����u���[�^���A���^�C���o�͗L���@�f�t�H���g�l�ݒ�
        /// </summary>
        public const Boolean REALTIME_OUTPUT_CALIB_DEFAULT = true;
        /// <summary>
        /// ���M�x�����ԁ@�f�t�H���g�l�ݒ�imsec�j
        /// </summary>
        public const Int32 SEND_DELAY_TIME_DEFAULT = SEND_DELAY_TIME_MIN;
        /// <summary>
        /// ���̖₢���킹�L���@�f�t�H���g�l�ݒ�
        /// </summary>
        public const Boolean ASK_SAMP_DEFAULT = false;
        /// <summary>
        /// ���x�Ǘ����̖₢���킹�L���@�f�t�H���g�l�ݒ�
        /// </summary>
        public const Boolean ASK_CTRL_DEFAULT = false;
        /// <summary>
        /// �T���v���敪���p�L���@�f�t�H���g�l�ݒ�
        /// </summary>
        public const Boolean USE_SMPL_KIND_DEFAULT = false;
        /// <summary>
        /// �R�����g�g�p�L�� �f�t�H���g�l�ݒ�
        /// </summary>
        public const Boolean USE_COMMENT_DEFAULT = true;
        #endregion

        #region [�C���X�^���X�ϐ���`]

        /// <summary>
        /// ���̖₢���킹�ҋ@����
        /// </summary>
        private Int32 usableAskSampWaitTime = 18;

        #endregion

        #region [�v���p�e�B]

        /// <summary>
        /// �R�����g�g�p�L��
        /// </summary>
        public Boolean UsableComment { get; } = USE_COMMENT_DEFAULT;

        /// <summary>
        /// �T���v���敪�g�p�L��
        /// </summary>
        public Boolean UseHostSampleType { get; } = USE_SMPL_KIND_DEFAULT;

        /// <summary>
        /// �{�[���[�g
        /// </summary>
        public BaudRate Baudrate { get; set; } = BAUDRATE_DEFAULT;

        /// <summary>
        /// �p���e�B
        /// </summary>
        public Parity Parity { get; set; } = PARITY_DEFAULT;

        /// <summary>
        /// �f�[�^��
        /// </summary>
        public DataBits DataLength { get; set; } = DATA_LENGTH_DEFAULT;

        /// <summary>
        /// �X�g�b�v�r�b�g
        /// </summary>
        public StopBitKind StopBit { get; set; } = STOP_BIT_DEFAULT;
        
        /// <summary>
        /// COM�|�[�g
        /// </summary>
        public String CommPort { get; set; } = COMM_PORT_DEFAULT;

        /// <summary>
        /// ���̃��A���^�C���o��
        /// </summary>
        public Boolean UsableRealtimeOutputSamp { get; set; } = REALTIME_OUTPUT_SAMP_DEFAULT;

        /// <summary>
        /// ���x�Ǘ����̃��A���^�C���o��
        /// </summary>
        public Boolean UsableRealtimeOutputCtrl { get; set; } = REALTIME_OUTPUT_CTRL_DEFAULT;

        /// <summary>
        /// ���M�x�����ԁimsec�j
        /// </summary>
        public Int32 SendDelayTime { get; set; } = SEND_DELAY_TIME_DEFAULT;

        /// <summary>
        /// ���̖₢���킹�L��
        /// </summary>
        public Boolean UseRealtimeSampleAsk { get; set; } = ASK_SAMP_DEFAULT;

        /// <summary>
        /// ���̖₢���킹�ҋ@����
        /// </summary>
        public Int32 UseRealtimeSampleAskWaitTime
        {
            get
            {
                return this.usableAskSampWaitTime;
            }
            set
            {
                if (value < usableAskSampWaitTime && !HostParameterSetFlag)
                    value = usableAskSampWaitTime;
                this.usableAskSampWaitTime = value;
            }
        }

        public Boolean HostParameterSetFlag { get; set; } = false;

        #endregion
    }
	 
}
 
