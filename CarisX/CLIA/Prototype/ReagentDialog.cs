using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.CarisX.GUI;

namespace Prototype
{
    public partial class ReagentDialog : Form
    {
        public ReagentDialog()
        {
            InitializeComponent();
        }

        private void ultraButton1_Click( object sender, EventArgs e )
        {
            DlgTurnTable dlg = new DlgTurnTable();
            dlg.DispMode = DlgTurnTable.TurnTableDispMode.Check;
            dlg.ShowDialog();
        }

        private void ultraButton2_Click( object sender, EventArgs e )
        {
            DlgTurnTable dlg = new DlgTurnTable();
            dlg.DispMode = DlgTurnTable.TurnTableDispMode.Change;
            dlg.ShowDialog();
        }
    }
}
