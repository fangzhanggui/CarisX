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
    /// ピクチャーボックスコントロール(状態表示切替可能)
    /// </summary>
    public class CustomStatePictureBox : UltraPictureBox
    {
        #region [プロパティ]

        /// <summary>
        /// インデックスの取得、設定
        /// </summary>
        /// <returns>取得できない場合は、-1</returns>
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
        /// 状態イメージリストの取得、設定
        /// </summary>
        public List<Image> ImageList
        {
            get;
            set;
        }

        #endregion

    }
}
