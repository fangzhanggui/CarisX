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
    public partial class Blinker : Form
    {
        public Blinker()
        {
            InitializeComponent();
        }

        private void label1_Click( object sender, EventArgs e )
        {

        }

        private void button1_Click( object sender, EventArgs e )
        {
            foreach ( BlinkButton btn in this.panel1.Controls )
            {
                btn.Appearance.ImageBackground = this.pictureBox1.Image;
                btn.Click += new System.EventHandler( this.customUButton1_Click );
            }
        }

        private void customUButton1_Click( object sender, EventArgs e )
        {
            BlinkButton btn = sender as BlinkButton;
            if ( btn.IsBlink )
            {
                btn.BlinkEnd();
            }
            else
            {
                btn.BlinkStart( this.pictureBox1.Image, this.pictureBox2.Image, Int32.Parse( this.labelCT.Text ), Int32.Parse( this.labelIT.Text ) );
            }
        }

        private void button2_Click( object sender, EventArgs e )
        {
            foreach ( BlinkButton btn in this.panel1.Controls )
            {
                if ( btn.IsBlink )
                {
                    btn.BlinkEnd();
                }
                else
                {
                    btn.BlinkStart( this.pictureBox1.Image, this.pictureBox2.Image, Int32.Parse( this.labelCT.Text ), Int32.Parse( this.labelIT.Text ) );
                }
            }

        }

        private void trackBar1_Scroll( object sender, EventArgs e )
        {
            this.labelCT.Text = this.trackBar1.Value.ToString();
        }

        private void trackBar2_Scroll( object sender, EventArgs e )
        {
            this.labelIT.Text = this.trackBar2.Value.ToString();
        }

        private void pictureBox1_DragDrop( object sender, DragEventArgs e )
        {
            String[] path = (String[])e.Data.GetData( System.Windows.Forms.DataFormats.FileDrop );
            (sender as PictureBox).Image = new Bitmap( path[0] );         
        }

        private void Blinker_Load( object sender, EventArgs e )
        {
            this.pictureBox1.AllowDrop = true;
            this.pictureBox2.AllowDrop = true;

        }

        private void pictureBox1_DragEnter( object sender, DragEventArgs e )
        {
            e.Effect = DragDropEffects.All;
        }

        private void button3_Click( object sender, EventArgs e )
        {
            foreach ( BlinkButton btn in this.panel1.Controls )
            {
                    btn.BlinkEnd();

            }
        }
    }
}
