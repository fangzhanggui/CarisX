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
    public partial class FormNumericEditor : Form
    {
        public FormNumericEditor()
        {
            InitializeComponent();
        }

        private void Form1_Load( object sender, EventArgs e )
        {
           
        }

        private void FormUT_Shown( object sender, EventArgs e )
        {           
            textBox1.Text = customNumericEditor1.MaskInput;
            textBox2.Text = ultraNumericEditor1.MaskInput;
            //this.Size = new Size( 270 , 200 );
        }

        private void textBox1_Leave( object sender, EventArgs e )
        {
            try
            {
                customNumericEditor1.MaskInput = textBox1.Text;
            }
            catch
            {
                textBox1.Text = customNumericEditor1.MaskInput;
            }
        }

        private void textBox2_Leave( object sender, EventArgs e )
        {
            try
            {
                ultraNumericEditor1.MaskInput = textBox2.Text;
            }
            catch
            {
                textBox2.Text = ultraNumericEditor1.MaskInput;
            }
        }

        private void FormUT_MouseMove( object sender, MouseEventArgs e )
        {
            System.Diagnostics.Debug.WriteLine( "Move"+DateTime.Now.ToShortTimeString() );
        }

        private void FormUT_MouseUp( object sender, MouseEventArgs e )
        {

            System.Diagnostics.Debug.WriteLine( "Up" + DateTime.Now.ToShortTimeString() );
        }

        private void FormUT_MouseDown( object sender, MouseEventArgs e )
        {

            System.Diagnostics.Debug.WriteLine( "Down" + DateTime.Now.ToShortTimeString() );
        }

    }
}
