using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// スプラッシュ画面クラス
    /// </summary>
    public partial class DlgSplash : Form
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgSplash()
        {
            InitializeComponent();
            //this.ultraPictureBox1.Image = CarisX.Properties.Resources.Image_Execute;
            this.lblInitialize.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SPLASH_000;
        }
    }
}
