using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinEditors;

namespace Oelco.Common.GUI
{
    /// <summary>
    /// �s�N�`���[�{�b�N�X�R���g���[��(��ԕ\���ؑ։\)
    /// </summary>
    public class CustomStatePictureBox : UltraPictureBox
    {
        #region [�v���p�e�B]

        /// <summary>
        /// �C���f�b�N�X�̎擾�A�ݒ�
        /// </summary>
        /// <returns>�擾�ł��Ȃ��ꍇ�́A-1</returns>
        [RefreshProperties(RefreshProperties.Repaint)]
        public Int32 ViewIndex
        {
            get
            {
                if (this.ImageList != null)
                {
                    foreach (Image img in this.ImageList)
                    {
                        if ( this.Image == img )
                        {
                            return this.ImageList.IndexOf( img );
                        }
                    }
                }
                return -1;
            }
            set
            {
                if (this.ImageList != null && this.ImageList.Count > value)
                {
                    this.Image = this.ImageList[value];
                }
            }
        }

        /// <summary>
        /// ��ԃC���[�W���X�g�̎擾�A�ݒ�
        /// </summary>
        public List<Image> ImageList
        {
            get;
            set;
        }

        #endregion

    }
}
