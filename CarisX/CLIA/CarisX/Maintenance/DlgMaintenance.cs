using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Oelco.CarisX.Maintenance
{
    public partial class DlgMaintenance : Form
    {

        public DlgMaintenance(string msg ,bool OKCansel)
        {
            InitializeComponent();
            lbMsg.Text = msg;
            if (!OKCansel) btCancel.Visible = false;
        }

      
    }
}
