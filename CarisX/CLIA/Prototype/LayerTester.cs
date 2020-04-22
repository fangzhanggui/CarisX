using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Oelco.Common.GUI;

namespace Prototype
{
    public partial class LayerTester : FormLayerManagedBase//FormLayerManagedBase
    {
        public LayerTester()
        {
            InitializeComponent();
        }

        private void button1_Click( object sender, EventArgs e )
        {

            var lay = new LayerTester();
            lay.SetLayer( Layer.Layer1 );
            lay.Text = "Level1";
            lay.Show();

        }

        private void button2_Click( object sender, EventArgs e )
        {

            var lay = new LayerTester();
            lay.SetLayer( Layer.Layer2 );
            lay.Text = "Level2";
            lay.Show();

        }

        private void button3_Click( object sender, EventArgs e )
        {

            var lay = new LayerTester();
            lay.SetLayer( Layer.Layer3 );
            lay.Text = "Level3";
            lay.Show();

        }
    }
}
