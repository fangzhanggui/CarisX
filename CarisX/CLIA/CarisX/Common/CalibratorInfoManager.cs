using System;
using System.Collections.Generic;
using System.Linq;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.Common
{

    /// <summary>
    /// �L�����u���[�^���
    /// </summary>
    public class CalibratorInfo
    {
        #region [�v���p�e�B]

        /// <summary>
        /// ���W���[��ID
        /// </summary>
        public Int32 ModuleId { get; set; }

        /// <summary>
        /// �|�[�g�ԍ�
        /// </summary>
        public Int32 PortNo { get; set; }

        /// <summary>
        /// ����R�[�h
        /// </summary>
        public Int32 ReagentCode { get; set; }

        /// <summary>
        /// �L�����u���[�^�{��
        /// </summary>
        public Int32 CalibratorLotCount { get; set; }

        /// <summary>
        /// �L�����u���[�^���b�g
        /// </summary>
        public List<CalibratorLot> CalibratorLot { get; set; }

        #endregion
    }

    /// <summary>
    /// �L�����u���[�^���Ǘ�
    /// </summary>
    public class CalibratorInfoManager
	{
        #region [�C���X�^���X�ϐ���`]

        /// <summary>
        /// �����������t���O
        /// </summary>
        public Boolean blnInitialized = false;

        #endregion

        #region [�v���p�e�B]

        /// <summary>
        /// �L�����u���[�^���
        /// </summary>
        public List<CalibratorInfo> CalibratorLot { get; set; } = new List<CalibratorInfo>();

        #endregion

        #region [public���\�b�h]

        /// <summary>
        /// ���b�N��񏉊���
        /// </summary>
        /// <remarks>
        /// ���b�N��񏉊������܂�
        /// </remarks>
        public void CalibratorInfoInitialize()
        {
            if (!this.blnInitialized)
            {
                //�L�����u���[�^�����N���A
                CalibratorLot.Clear();

                //�L�����u���[�^��񏉊���
                CalibratorLot = new List<CalibratorInfo>();

                this.blnInitialized = true;
            }
        }

        /// <summary>
        /// �L�����u���[�^���ݒ�
        /// </summary>
        /// <remarks>
        /// �L�����u���[�^����ݒ肵�܂��B
        /// </remarks>
        /// <param name="info">�L�����u���[�^���̃C���X�^���X</param>
        public void SetCalibratorInfo(CalibratorInfo info)
        {
            //�Z�b�g���悤�Ƃ��Ă���L�����u���[�^��񂪂��łɑ��݂���ꍇ�͒u��������B
            if (this.CalibratorLot.Exists(v => v.ModuleId == info.ModuleId && v.PortNo == info.PortNo))
            {
                this.CalibratorLot.RemoveAt(this.CalibratorLot.FindIndex(v => v.ModuleId == info.ModuleId && v.PortNo == info.PortNo));
            }

            this.CalibratorLot.Add(info);
        }

        /// <summary>
        /// �ێ��f�[�^�N���A
        /// </summary>
        /// <remarks>
        /// �ێ��f�[�^���N���A���܂��B
        /// </remarks>
        public void Clear()
        {
            // ���b�N�X�e�[�^�X�N���A����
            this.CalibratorLot.Clear();
            this.blnInitialized = false;
        }

        #endregion
    }
}
 
