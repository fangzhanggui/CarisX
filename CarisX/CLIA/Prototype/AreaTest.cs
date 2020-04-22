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
    public partial class AreaTest : Form
    {
        public AreaTest()
        {
            InitializeComponent();
        }

        private void ultraButton1_Click( object sender, EventArgs e )
        {
            MessageBox.Show( "ultraButton1_Click" );
        }

        private void ultraPanel1_Click( object sender, EventArgs e )
        {

            MessageBox.Show( "ultraPanel1_Click" );
        }

        private void ultraPictureBox1_Click( object sender, EventArgs e )
        {
            MessageBox.Show( "ultraPictureBox1_Click" );
        }

    }
}
