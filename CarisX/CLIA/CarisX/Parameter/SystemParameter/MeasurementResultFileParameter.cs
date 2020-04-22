using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// ���茋�ʃt�@�C���쐬�ݒ�
    /// </summary>
	public class MeasurementResultFileParameter : AttachmentParameter
	{
        #region [�萔��`]

        /// <summary>
        /// �t�H���_���@�f�t�H���g�l�ݒ�
        /// </summary>
        public const String FOLDER_NAME_DEFAULT = "";

        #endregion

        #region [�C���X�^���X�ϐ���`]

        /// <summary>
        /// �t�H���_��
        /// </summary>
        private String folderName = FOLDER_NAME_DEFAULT;

        #endregion

        #region [�v���p�e�B]

        /// <summary>
        /// �t�H���_���̐ݒ�^�擾
        /// </summary>
        public String FolderName
        {
            get
            {
                return this.folderName;
            }
            set
            {
                this.folderName = value;
            }
        }

        #endregion
	}
	 
}
 
