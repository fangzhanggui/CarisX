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
    public partial class TimeLabeler : Form
    {
        public TimeLabeler()
        {
            InitializeComponent();
        }

        private void button1_Click( object sender, EventArgs e )
        {
            this.customLabel1.StartCountDown( TimeSpan.FromSeconds( 10 ) );
        }

        private void customLabel1_TimeOver( object sender, EventArgs e )
        {
            MessageBox.Show( "終" );
        }
    }
}
