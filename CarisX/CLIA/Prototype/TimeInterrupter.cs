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
    public partial class TimeInterrupter : Form
    {
        private String[] textList = new String[1];

        public TimeInterrupter()
        {
            InitializeComponent();
            this.interrupter.Start();
        }

        private void button1_Click( object sender, EventArgs e )
        {
//            this.textList.Add("Button");
            for ( Int32 i = 0; i < 100; i++ )
            {
                Array.Resize( ref this.textList, this.textList.Length + 1 );
                this.textList[this.textList.Length - 1] = DateTime.Now.ToShortTimeString() + " Button";
                this.textBox1.Lines = this.textList;
                //System.Threading.Thread.Sleep(1);
            }
        }

        private void interrupter_Tick( object sender, EventArgs e )
        {
            Array.Resize( ref this.textList, this.textList.Length + 1 );
            this.textList[this.textList.Length - 1] = DateTime.Now.ToShortTimeString() + " Interrupter" ;
            this.textBox1.Lines = this.textList;
        }

        private void textBox1_TextChanged( object sender, EventArgs e )
        {

        }

        private void textBox1_Enter( object sender, EventArgs e )
        {
            this.interrupter.Stop();
        }

        private void textBox1_Leave( object sender, EventArgs e )
        {

            this.interrupter.Start();
        }
    }
}
