using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;

namespace Oelco.Common.GUI
{
    /// <summary>
    /// CarisX����J�X�^���f�U�C��UltraOptionSet
    /// </summary>
    public class CustomURadioButton : UltraRadioButton
    {
        #region [�C���X�^���X�ϐ���`]

        /// <summary>
        /// GlyphInfo����l
        /// </summary>
        private GlyphInfoBase glyphInfoDefault = new RadioButtonImageGlyphInfo( Oelco.Common.Properties.Resources.Image_OptionSetGlyphInfo, "�J�X�^�� ���W�I �{�^�� �O���t" );

        #endregion

        #region [�R���X�g���N�^/�f�X�g���N�^]

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public CustomURadioButton()
        {
            this.HandleCreated += new EventHandler( CustomRadioButton_HandleCreated );
            this.BorderStyle = UIElementBorderStyle.None;
        }

        #endregion

        #region [�v���p�e�B]

        /// <summary>
        /// ���W�I �{�^�����`�悳�����@�����肵�܂��B
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new GlyphInfoBase GlyphInfo
        {
            get
            {
                if (base.GlyphInfo != this.glyphInfoDefault && base.GlyphInfo == null)
                    base.GlyphInfo = glyphInfoDefault;
                return base.GlyphInfo;
            }
            set
            {
                base.GlyphInfo = value;
            }
        }

        #endregion

        #region [private���\�b�h]

        /// <summary>
        /// �n���h�������C�x���g
        /// </summary>
        /// <remarks>
        /// �n���h�������C�x���g�������s���܂��B
        /// </remarks>
        /// <param name="sender">�Ăяo�����I�u�W�F�N�g</param>
        /// <param name="e">�C�x���g�f�[�^</param>
        void CustomRadioButton_HandleCreated( object sender, EventArgs e )
        {
            // GlyphInfo�̏�����(����)
            this.GlyphInfo = glyphInfoDefault;
        }

        /// <summary>
        /// �v���p�e�B�V���A�����K�v�L���̎擾
        /// </summary>
        /// <remarks>
        /// Infragistics.Win.UltraWinEditors.UltraOptionSet.GlyphInfo �v���p�e�B���f�t�H���g�l�ɐݒ肳��Ă��邩�ǂ����������u�[���l��Ԃ��܂��B
        /// </remarks>
        /// <returns>�v���p�e�B�̃V���A�������K�v���ǂ����������u�[���l</returns>
        private new Boolean ShouldSerializeGlyphInfo()
        {
            return this.GlyphInfo != glyphInfoDefault;
        }

        /// <summary>
        /// GlyphInfo�v���p�e�B�̃��Z�b�g
        /// </summary>
        /// <remarks>
        /// Infragistics.Win.UltraWinEditors.UltraOptionSet.GlyphInfo �v���p�e�B���f�t�H���g�l�Ƀ��Z�b�g���܂��B
        /// </remarks>
        private new void ResetGlyphInfo()
        {
            this.GlyphInfo = glyphInfoDefault;
        }

        #endregion

    }
}
