using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oelco.CarisX.Maintenance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.Misc;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.Maintenance.Tests
{
    [TestClass()]
    public class FormMaintenanceMainTests
    {
        [TestMethod()]
        public void SetVisibleModuleTabTest()
        {
            FormMaintenanceMain main = new FormMaintenanceMain();
            var pbObj = new PrivateObject(main);

            var actual = pbObj.Invoke("SetVisibleModuleTab", 1);
            UltraTabControl tab = (UltraTabControl)main.Controls.Find("TabControl", true)[0];
            Assert.AreEqual(true, (object)tab.Tabs[(Int32)ModuleTabIndex.Slave1].Enabled);
            Assert.AreEqual(false, (object)tab.Tabs[(Int32)ModuleTabIndex.Slave2].Enabled);
            Assert.AreEqual(false, (object)tab.Tabs[(Int32)ModuleTabIndex.Slave3].Enabled);
            Assert.AreEqual(false, (object)tab.Tabs[(Int32)ModuleTabIndex.Slave4].Enabled);

            actual = pbObj.Invoke("SetVisibleModuleTab", 2);
            tab = (UltraTabControl)main.Controls.Find("TabControl", true)[0];
            Assert.AreEqual(true, (object)tab.Tabs[(Int32)ModuleTabIndex.Slave1].Enabled);
            Assert.AreEqual(true, (object)tab.Tabs[(Int32)ModuleTabIndex.Slave2].Enabled);
            Assert.AreEqual(false, (object)tab.Tabs[(Int32)ModuleTabIndex.Slave3].Enabled);
            Assert.AreEqual(false, (object)tab.Tabs[(Int32)ModuleTabIndex.Slave4].Enabled);

            actual = pbObj.Invoke("SetVisibleModuleTab", 3);
            tab = (UltraTabControl)main.Controls.Find("TabControl", true)[0];
            Assert.AreEqual(true, (object)tab.Tabs[(Int32)ModuleTabIndex.Slave1].Enabled);
            Assert.AreEqual(true, (object)tab.Tabs[(Int32)ModuleTabIndex.Slave2].Enabled);
            Assert.AreEqual(true, (object)tab.Tabs[(Int32)ModuleTabIndex.Slave3].Enabled);
            Assert.AreEqual(false, (object)tab.Tabs[(Int32)ModuleTabIndex.Slave4].Enabled);

            actual = pbObj.Invoke("SetVisibleModuleTab", 4);
            tab = (UltraTabControl)main.Controls.Find("TabControl", true)[0];
            Assert.AreEqual(true, (object)tab.Tabs[(Int32)ModuleTabIndex.Slave1].Enabled);
            Assert.AreEqual(true, (object)tab.Tabs[(Int32)ModuleTabIndex.Slave2].Enabled);
            Assert.AreEqual(true, (object)tab.Tabs[(Int32)ModuleTabIndex.Slave3].Enabled);
            Assert.AreEqual(true, (object)tab.Tabs[(Int32)ModuleTabIndex.Slave4].Enabled);

            actual = pbObj.Invoke("SetVisibleModuleTab", 0);
            tab = (UltraTabControl)main.Controls.Find("TabControl", true)[0];
            Assert.AreEqual(false, (object)tab.Tabs[(Int32)ModuleTabIndex.Slave1].Enabled);
            Assert.AreEqual(false, (object)tab.Tabs[(Int32)ModuleTabIndex.Slave2].Enabled);
            Assert.AreEqual(false, (object)tab.Tabs[(Int32)ModuleTabIndex.Slave3].Enabled);
            Assert.AreEqual(false, (object)tab.Tabs[(Int32)ModuleTabIndex.Slave4].Enabled);

        }
    }
}