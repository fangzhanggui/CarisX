using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prototype
{
    public partial class ProgressBarTest : Form
    {
        public ProgressBarTest()
        {
            InitializeComponent();

            this.customProgressBar1.Minimum = 0;
            this.customProgressBar1.Maximum = 100;
            this.customProgressBar1.Value = 10;

            //this.customProgressBar1.BarColorSetting.AddColorRangePair( 10, Color.Red );
            //this.customProgressBar1.BarColorSetting.AddColorRangePair( 20, Color.Orange );
            //this.customProgressBar1.BarColorSetting.AddColorRangePair( 30, Color.Yellow );
            //this.customProgressBar1.BarColorSetting.AddColorRangePair( 50, Color.Green );
            //this.customProgressBar1.BarColorSetting.AddColorRangePair( 100, Color.Blue );

        }

        private void customUButton1_Click( object sender, EventArgs e )
        {

            if ( this.customProgressBar1.Value >= 100 )
            {
                this.customProgressBar1.Value = 0;
            }
            else
            {
                this.customProgressBar1.Value += 10;
            }
        }
    }
}
