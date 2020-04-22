using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolConverter.File
{
    /// <summary>
    /// ���͍��ڃt�@�C������N���X
    /// </summary>
    /// <remarks>
    /// ���͍��ڃt�@�C���̓Ǎ��݂⏑���݂��s���N���X�ł��B
    /// </remarks>
	public class MeasureProtocolManager
    {

        #region [�萔��`]

        /// <summary>
        /// ���͍��ڍő吔
        /// </summary>
        private const Int32 PROTOCOL_MAX_COUNT = 200;
 
        #endregion

        #region [�N���X�ϐ���`]

        #endregion

        #region [�C���X�^���X�ϐ���`]

        /// <summary>
        /// ���͍���
        /// </summary>
		private List<ParameterFilePreserve< MeasureProtocol >> measureProtocol = new List<ParameterFilePreserve< MeasureProtocol >>();

        #endregion

        #region [�R���X�g���N�^/�f�X�g���N�^]

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public MeasureProtocolManager()
        {           
        }

        #endregion

        #region [�v���p�e�B]
        
        #endregion

        #region [public���\�b�h]

        /// <summary>
        /// ���͍��ڑޔ�̈�̒ǉ�
        /// </summary>
        /// <param name="index">�v���g�R���C���f�b�N�X</param>
        /// <returns>�ǉ������̈�</returns>
        public ParameterFilePreserve<MeasureProtocol> AddMeasureProtocol( Int32 index )
        {            
            ParameterFilePreserve<MeasureProtocol> protocol = new ParameterFilePreserve<MeasureProtocol>();                          

            // ���͍��ڔԍ����쐬
            if ( this.GetMeasureProtocolFromProtocolIndex( index ) == null )
            {
                protocol.Param.ProtocolIndex = index;
                this.measureProtocol.Add( protocol );
            }
            else
            {
                protocol = null;
            }
              
            return protocol; 
        }

        /// <summary>
        /// �S�v���g�R���̈ꊇ�ۑ�
        /// </summary>
        /// <param name="path">�ۑ��p�X</param>
        /// <returns></returns>
        public Int32 SaveAllMeasureProtocol(String path)
        {
            Int32 result =0;

            // ���͍��ڂ��t�@�C���֑S�ď�������
            foreach ( var protocol in this.measureProtocol )
            {
                protocol.Param.SetSaveProtocolPath(path);
                if ( protocol.Save())
                {
                   result++;  
                }
            }
            return result;
        }       

        /// <summary>
        /// ���͍��ڐݒ�擾
        /// </summary>
        /// <remarks>
        /// ���͍��ڐݒ���𕪐͍��ڃC���f�b�N�X����擾���܂��B
        /// </remarks>
        /// <param name="name">���͍��ڃC���f�b�N�X</param>
        /// <returns>���͍��ڏ��</returns>
        public MeasureProtocol GetMeasureProtocolFromProtocolIndex( Int32 protocolIndex )
        {
            MeasureProtocol protocol = null;

            // ���͍��ڃC���f�b�N�X���瑪�荀�ڐݒ������
            IEnumerable<MeasureProtocol> searchResult = from p in this.measureProtocol
                                                        where p.Param.ProtocolIndex == protocolIndex
                                                        select p.Param;

            // �������ʂ��擾
            if ( searchResult.Count() != 0 )
            {
                protocol = searchResult.First();
            }

            return protocol;
        }       

     
        #endregion

        #region [protected���\�b�h]

        #endregion

        #region [private���\�b�h]

        #endregion 
        
	}
	 
}
 
