using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oelco.CarisX.Maintenance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.Misc;
using Oelco.CarisX.Comm;

namespace Oelco.CarisX.Maintenance.Tests
{
    [TestClass()]
    public class FormSTATTests
    {
        [TestMethod()]
        public void setCultureTest()
        {
            var stat = new FormSTATUnit();
            var pbObj = new PrivateObject(stat);
            var actual = pbObj.Invoke("setCulture");

            UltraTabControl tab = (UltraTabControl)stat.Controls.Find("tabSTAT", true)[0];
            Assert.AreEqual("Test", (object)tab.Tabs[0].Text);
            Assert.AreEqual("Configuration", (object)tab.Tabs[1].Text);
            Assert.AreEqual("Motor Parameters", (object)tab.Tabs[2].Text);

            UltraGroupBox gbxtestSequence = (UltraGroupBox)stat.Controls.Find("gbxtestSequence", true)[0];
            Assert.AreEqual("Sequence", (object)gbxtestSequence.Text);

            ListBox lbxtestSequenceListBox = (ListBox)stat.Controls.Find("lbxtestSequenceListBox", true)[0];
            lbxtestSequenceListBox.SelectedIndex = 0;
            Assert.AreEqual("1: Unit Initialization", lbxtestSequenceListBox.Text);
            lbxtestSequenceListBox.SelectedIndex = 1;
            Assert.AreEqual("2: Move to STAT Sorting Position", lbxtestSequenceListBox.Text);
            lbxtestSequenceListBox.SelectedIndex = 2;
            Assert.AreEqual("3: Move to STAT Standby Position", lbxtestSequenceListBox.Text);

            UltraGroupBox gbxtestParameters = (UltraGroupBox)stat.Controls.Find("gbxtestParameters", true)[0];
            Assert.AreEqual("Parameters", (object)gbxtestParameters.Text);

            UltraLabel lbltestRepeatFrequency = (UltraLabel)stat.Controls.Find("lbltestRepeatFrequency", true)[0];
            Assert.AreEqual("Repeat Frequency", (object)lbltestRepeatFrequency.Text);
            UltraLabel lbltestNumber = (UltraLabel)stat.Controls.Find("lbltestNumber", true)[0];
            Assert.AreEqual("Number", (object)lbltestNumber.Text);

            UltraGroupBox gbxtestResponce = (UltraGroupBox)stat.Controls.Find("gbxtestResponce", true)[0];
            Assert.AreEqual("Response", (object)gbxtestResponce.Text);
        }


    }
}