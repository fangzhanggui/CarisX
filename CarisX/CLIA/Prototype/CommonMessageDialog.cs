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
    public partial class CommonMessageDialog : Form
    {
        public CommonMessageDialog()
        {
            InitializeComponent();
        }

        private void button1_Click( object sender, EventArgs e )
        {
            DialogResult result;
            
            if (this.label1.Tag == null || (DialogResult)this.label1.Tag == DialogResult.Cancel )
            {
                result = DlgMessage.Show( "めっせーじ", "わーにんぐ", "Confirmタイトル", MessageDialogButtons.Confirm );
                this.label1.Text = result.ToString();
                this.label1.Tag = result;
            }
            else if ((DialogResult)this.label1.Tag== DialogResult.Yes )
            {
                result = DlgMessage.Show( "めっせーじ", "わーにんぐ", "OKタイトル", MessageDialogButtons.OK );
                this.label1.Text = result.ToString();
                this.label1.Tag = result;
            }
            else if ( (DialogResult)this.label1.Tag == DialogResult.OK )
            {
                result = DlgMessage.Show( "めっせーじ", "わーにんぐ", "OKCancelタイトル", MessageDialogButtons.OKCancel );
                this.label1.Text = result.ToString();
                this.label1.Tag = result;
            }
        } 
    }
}