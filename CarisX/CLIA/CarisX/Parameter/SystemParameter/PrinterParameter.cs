using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// �v�����^�ݒ�
    /// </summary>
	public class PrinterParameter : AttachmentParameter
	{
        #region [�萔��`]

        /// <summary>
        /// �v�����^�ݒ�@�f�t�H���g�l
        /// </summary>
        public const Boolean PRINTER_SET_DEFAULT = false;

        #endregion

        #region [�C���X�^���X�ϐ���`]

        /// <summary>
        /// ���A���^�C������L��
        /// </summary>
        private Boolean usableRealtimePrint = PRINTER_SET_DEFAULT;

        #endregion

        #region [�v���p�e�B]

        /// <summary>
        /// ���A���^�C������L���̐ݒ�^�擾
        /// </summary>
        public Boolean UsableRealtimePrint
        {
            get
            {
                return this.usableRealtimePrint;
            }
            set
            {
                this.usableRealtimePrint = value;
            }
        }

        #endregion
	}
	 
}
 
