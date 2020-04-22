using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// ���x�ݒ�
    /// </summary>
	public class TemperatureParameter : AttachmentParameter
	{
        #region [�萔��`]

        /// <summary>
        /// �����e�[�u���@�Œቷ�x�ݒ�i���j
        /// </summary>
        public const Double TEMP_REACT_TBL_MIN = 30.0;
        /// <summary>
        /// �����e�[�u���@�ō����x�ݒ�i���j
        /// </summary>
        public const Double TEMP_REACT_TBL_MAX = 70.0;
        /// <summary>
        /// BF�e�[�u���@�Œቷ�x�ݒ�i���j
        /// </summary>
        public const Double TEMP_BF_TBL_MIN = 30.0;
        /// <summary>
        /// BF�e�[�u���@�ō����x�ݒ�i���j
        /// </summary>
        public const Double TEMP_BF_TBL_MAX = 70.0;
        /// <summary>
        /// B/F1�v���q�[�g���x�@�Œቷ�x�ݒ�i���j
        /// </summary>
        public const Double TEMP_BF1_PRE_HEAT_MIN = 30.0;
        /// <summary>
        /// B/F1�v���q�[�g���x�@�ō����x�ݒ�i���j
        /// </summary>
        public const Double TEMP_BF1_PRE_HEAT_MAX = 70.0;
        /// <summary>
        /// B/F2�v���q�[�g���x�@�Œቷ�x�ݒ�i���j
        /// </summary>
        public const Double TEMP_BF2_PRE_HEAT_MIN = 30.0;
        /// <summary>
        /// B/F2�v���q�[�g���x�@�ō����x�ݒ�i���j
        /// </summary>
        public const Double TEMP_BF2_PRE_HEAT_MAX = 70.0;
        /// <summary>
        /// R1�v���[�u�v���q�[�g���x�@�Œቷ�x�ݒ�i���j
        /// </summary>
        public const Double TEMP_R1_PROBE_PRE_HEAT_MIN = 30.0;
        /// <summary>
        /// R1�v���[�u�v���q�[�g���x�@�ō����x�ݒ�i���j
        /// </summary>
        public const Double TEMP_R1_PROBE_PRE_HEAT_MAX = 70.0;
        /// <summary>
        /// R2�v���[�u�v���q�[�g���x�@�Œቷ�x�ݒ�i���j
        /// </summary>
        public const Double TEMP_R2_PROBE_PRE_HEAT_MIN = 30.0;
        /// <summary>
        /// R2�v���[�u�v���q�[�g���x�@�ō����x�ݒ�i���j
        /// </summary>
        public const Double TEMP_R2_PROBE_PRE_HEAT_MAX = 70.0;
        /// <summary>
        /// ���������x�@�Œቷ�x�ݒ�i���j
        /// </summary>
        public const Double TEMP_CHEMI_LIGHT_MEAS_MIN = 30.0;
        /// <summary>
        /// ���������x�@�ō����x�ݒ�i���j
        /// </summary>
        public const Double TEMP_CHEMI_LIGHT_MEAS_MAX = 70.0;

        /// <summary>
        /// �����e�[�u���@�f�t�H���g�l�ݒ�i���j
        /// </summary>
        public const Double TEMP_REACT_TBL_DEFAULT = TEMP_REACT_TBL_MIN;
        /// <summary>
        /// BF�e�[�u���@�f�t�H���g�l�ݒ�i���j
        /// </summary>
        public const Double TEMP_BF_TBL_DEFAULT = TEMP_BF_TBL_MIN;
        /// <summary>
        /// B/F1�v���q�[�g���x�@�f�t�H���g�l�ݒ�i���j
        /// </summary>
        public const Double TEMP_BF1_PRE_HEAT_DEFAULT = TEMP_BF1_PRE_HEAT_MIN;
        /// <summary>
        /// B/F2�v���q�[�g���x�@�f�t�H���g�l�ݒ�i���j
        /// </summary>
        public const Double TEMP_BF2_PRE_HEAT_DEFAULT = TEMP_BF2_PRE_HEAT_MIN;
        /// <summary>
        /// R1�v���[�u�v���q�[�g���x�@�f�t�H���g�l�ݒ�i���j
        /// </summary>
        public const Double TEMP_R1_PROBE_PRE_HEAT_DEFAULT = TEMP_R1_PROBE_PRE_HEAT_MIN;
        /// <summary>
        /// R2�v���[�u�v���q�[�g���x�@�f�t�H���g�l�ݒ�i���j
        /// </summary>
        public const Double TEMP_R2_PROBE_PRE_HEAT_DEFAULT = TEMP_R2_PROBE_PRE_HEAT_MIN;
        /// <summary>
        /// ���������x�@�f�t�H���g�l�ݒ�i���j
        /// </summary>
        public const Double TEMP_CHEMI_LIGHT_MEAS_DEFAULT = TEMP_CHEMI_LIGHT_MEAS_MIN;

        /// <summary>
        /// ���x�G���[���T���v�����O��~�L��
        /// </summary>
        public const Byte TEMP_ERROR_SAMPLING_STOP_EXISTENCE_DEFAULT = 0;

        /// <summary>
        /// ����ۗ�Ƀf�t�H���g�l
        /// </summary>
        public const Double TEMP_REAGENT_COOLING_BOX_DEFAULT = 0.0d;
        /// <summary>
        /// �����f�t�H���g�l
        /// </summary>
        public const Double TEMP_ROOM_DEFAULT = 0.0d;
        /// <summary>
        /// ���u���x�f�t�H���g�l
        /// </summary>
        public const Double TEMP_DEVICE_DEFAULT = 0.0d;

        #endregion

        #region [�v���p�e�B]

        /// <summary>
        /// ����ۗ�ɉ��x�i���j
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Double TempReagentCoolingBox { get; set; } = TEMP_REAGENT_COOLING_BOX_DEFAULT;

        /// <summary>
        /// �����i���j
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Double TempRoom { get; set; } = TEMP_ROOM_DEFAULT;

        /// <summary>
        /// ���u���x�i���j
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Double TempDevice { get; set; } = TEMP_DEVICE_DEFAULT;

        /// <summary>
        /// �����e�[�u���@���x�i���j
        /// </summary>
        public Double TempReactionTable { get; set; } = TEMP_REACT_TBL_DEFAULT;

        /// <summary>
        /// �����e�[�u���@�I�t�Z�b�g�i���j
        /// </summary>
        public Double TempReactionTableOffset { get; set; } = TEMP_REACT_TBL_DEFAULT;

        /// <summary>
        /// BF�e�[�u���@���x�i���j
        /// </summary>
        public Double TempBFTable { get; set; } = TEMP_BF_TBL_DEFAULT;

        /// <summary>
        /// BF�e�[�u���@�I�t�Z�b�g�i���j
        /// </summary>
        public Double TempBFTableOffset { get; set; } = TEMP_BF_TBL_DEFAULT;

        /// <summary>
        /// B/F1�v���q�[�g�@���x�i���j
        /// </summary>
        public Double TempBF1PreHeat { get; set; } = TEMP_BF1_PRE_HEAT_DEFAULT;

        /// <summary>
        /// B/F1�v���q�[�g�@�I�t�Z�b�g�i���j
        /// </summary>
        public Double TempBF1PreHeatOffset { get; set; } = TEMP_BF1_PRE_HEAT_DEFAULT;

        /// <summary>
        /// B/F2�v���q�[�g�@���x�i���j
        /// </summary>
        public Double TempBF2PreHeat { get; set; } = TEMP_BF2_PRE_HEAT_DEFAULT;

        /// <summary>
        /// B/F2�v���q�[�g�@�I�t�Z�b�g�i���j
        /// </summary>
        public Double TempBF2PreHeatOffset { get; set; } = TEMP_BF2_PRE_HEAT_DEFAULT;

        /// <summary>
        /// R1�v���[�u�v���q�[�g�@���x�i���j
        /// </summary>
        public Double TempR1ProbePreHeat { get; set; } = TEMP_R1_PROBE_PRE_HEAT_DEFAULT;

        /// <summary>
        /// R1�v���[�u�v���q�[�g�@�I�t�Z�b�g�i���j
        /// </summary>
        public Double TempR1ProbePreHeatOffset { get; set; } = TEMP_R1_PROBE_PRE_HEAT_DEFAULT;

        /// <summary>
        /// R2�v���[�u�v���q�[�g�@���x�i���j
        /// </summary>
        public Double TempR2ProbePreHeat { get; set; } = TEMP_R2_PROBE_PRE_HEAT_DEFAULT;

        /// <summary>
        /// R2�v���[�u�v���q�[�g�@�I�t�Z�b�g�i���j
        /// </summary>
        public Double TempR2ProbePreHeatOffset { get; set; } = TEMP_R2_PROBE_PRE_HEAT_DEFAULT;

        /// <summary>
        /// �������@���x�i���j
        /// </summary>
        public Double TempChemiLightMeas { get; set; } = TEMP_CHEMI_LIGHT_MEAS_DEFAULT;

        /// <summary>
        /// �������@�I�t�Z�b�g�i���j
        /// </summary>
        public Double TempChemiLightMeasOffset { get; set; } = TEMP_CHEMI_LIGHT_MEAS_DEFAULT;

        /// <summary>
        /// ���x�G���[���̃T���v�����O��~�L��
        /// </summary>
        public Byte TempErrorSamplingStopExistence { get; set; } = TEMP_ERROR_SAMPLING_STOP_EXISTENCE_DEFAULT;

        #endregion
    }

}
 
