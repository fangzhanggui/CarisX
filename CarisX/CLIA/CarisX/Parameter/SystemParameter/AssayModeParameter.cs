using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.CarisX.Const;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;

namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// ���͕����ݒ�
    /// </summary>
	public class AssayModeParameter : AttachmentParameter
	{
        /// <summary>
        /// ���͕������
        /// </summary>
        public enum AssayModeKind
        {
            /// <summary>
            /// �V�[�P���V��������
            /// </summary>
            Sequencial = 0,
            /// <summary>
            /// ���b�NID����
            /// </summary>
            RackID = 1,
            /// <summary>
            /// ����ID����
            /// </summary>
            SampleID = 2
        }

        /// <summary>
        /// ������
        /// </summary>
        public enum MeasKind
        {
            /// <summary>
            /// ���b�NID
            /// </summary>
            RackID = 0,
            /// <summary>
            /// ����ID
            /// </summary>
            SampleID = 1
        }

        /// <summary>
        /// ���͕���
        /// </summary>
        public AssayModeKind AssayMode { get; set; } = AssayModeKind.RackID;

        /// <summary>
        /// �L�����u���[�^����
        /// </summary>
        public MeasKind CalibMeas { get; set; } = MeasKind.RackID;

        /// <summary>
        /// ���x�Ǘ����̑���
        /// </summary>
        public MeasKind ControlMeas { get; set; } = MeasKind.RackID;

        /// <summary>
        /// ���̓��[�h(Module1)
        /// </summary>
        public Boolean UseEmergencyModeForModule1 { get; set; } = false;
        /// <summary>
        /// ���̓��[�h(Module2)
        /// </summary>
        public Boolean UseEmergencyModeForModule2 { get; set; } = false;
        /// <summary>
        /// ���̓��[�h(Module3)
        /// </summary>
        public Boolean UseEmergencyModeForModule3 { get; set; } = false;
        /// <summary>
        /// ���̓��[�h(Module4)
        /// </summary>
        public Boolean UseEmergencyModeForModule4 { get; set; } = false;

        /// <summary>
        /// �}�f���[�h�擾
        /// </summary>
        /// <param name="moduleIndex"></param>
        public Boolean GetUseEmergencyMode( Int32 moduleIndex )
        {
            Boolean result = false;

            switch (moduleIndex)
            {
                case (Int32)ModuleIndex.Module1:
                    result = this.UseEmergencyModeForModule1;
                    break;
                case (Int32)ModuleIndex.Module2:
                    result = this.UseEmergencyModeForModule2;
                    break;
                case (Int32)ModuleIndex.Module3:
                    result = this.UseEmergencyModeForModule3;
                    break;
                case (Int32)ModuleIndex.Module4:
                    result = this.UseEmergencyModeForModule4;
                    break;
            }

            return result;
        }

        /// <summary>
        /// �ڑ��䐔���̃X���[�u�̋}�f�g�p�L���ɂ���āA�}�f���[�h�̕��͍��ڃ{�^����񊈐��ɂ��邩�̔��f�t���O
        /// </summary>
        /// <returns>true:�}�f���[�h�̕��͍��ڃ{�^����񊈐��ɂ���</returns>
        /// <returns>false:�}�f���[�h�̕��͍��ڃ{�^����񊈐��ɂ��Ȃ�</returns>
        public Boolean IsProtocolEnabledChangedInEmergencyMode()
        {
            bool enabledFlag = true;

            // �ڑ�����X���[�u�̃}�b�N�X�����ɍ��킹�Ċe�X���[�u�̋}�f�g�p�L�����`�F�b�N����
            for (int moduleIndex = 0; moduleIndex < Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected; moduleIndex++)
            {
                if (this.GetUseEmergencyMode(moduleIndex) == true)
                {
                    enabledFlag = false;
                    break;
                }
            }

            return enabledFlag;
        }
    }


}
 
