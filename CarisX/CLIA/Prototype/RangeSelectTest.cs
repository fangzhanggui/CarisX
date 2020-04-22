using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.CarisX.GUI;

namespace Prototype
{
    public partial class RangeSelectTest : Form
    {
        public RangeSelectTest()
        {
            InitializeComponent();
        }

        private void button1_Click( object sender, EventArgs e )
        {
            TargetRange res = DlgTargetSelectRange.Show();
            this.label1.Text = res.ToString();
        }
    }
}
