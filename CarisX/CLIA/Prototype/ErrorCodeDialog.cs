using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.Serialization.Formatters.Soap;
using Oelco.CarisX.GUI;
using Oelco.Common.Utility;
using System.IO;
using System.Text.RegularExpressions;
using Oelco.Common.Parameter;

namespace Prototype
{
    public partial class ErrorCodeDialog : Form
    {
        public ErrorCodeDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// エラーボタンが押された時
        /// </summary>
        /// <remarks>
        /// エラーボタンが押された時の処理
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnError_Click(object sender, EventArgs e)
        {
            DlgErrorCodeMessage errMsg = new DlgErrorCodeMessage();
            errMsg.ShowErrorMessage("1", "2", "");
        }
    }
}
