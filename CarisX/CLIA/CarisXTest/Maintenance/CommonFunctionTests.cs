using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oelco.CarisX.Maintenance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.Maintenance.Tests
{
    [TestClass()]
    public class CommonFunctionTests
    {
        [TestMethod()]
        public void setSequenceListBoxTest()
        {
            CommonFunction ComFunc = new CommonFunction();

            object SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name="test1", No=1},
                new SequenceItem{Name="test2", No=2},
            };
            System.Windows.Forms.ListBox lbxIn = new System.Windows.Forms.ListBox();
            System.Windows.Forms.ListBox lbxOut = new System.Windows.Forms.ListBox();

            lbxOut = ComFunc.setSequenceListBox(lbxIn, SequenceList);

            Assert.AreEqual("Name", lbxOut.DisplayMember);
            Assert.AreEqual("No", lbxOut.ValueMember);
            Assert.AreEqual(SequenceList, lbxOut.DataSource);

        }

        [TestMethod()]
        public void getSelectedRadioButtonValueTest()
        {
            CommonFunction ComFunc = new CommonFunction();

            Infragistics.Win.Misc.UltraGroupBox gbxtest = new Infragistics.Win.Misc.UltraGroupBox();
            System.Windows.Forms.RadioButton rbttestON = new System.Windows.Forms.RadioButton();
            System.Windows.Forms.RadioButton rbttestOFF = new System.Windows.Forms.RadioButton();

            //ラジオボタンが含まれていないグループボックスの場合
            Assert.AreEqual(0, ComFunc.getSelectedRadioButtonValue(gbxtest));

            //ラジオボタンを追加
            gbxtest.Controls.Add(rbttestON);
            gbxtest.Controls.Add(rbttestOFF);

            //ラジオボタンがいずれもチェックされていない場合
            Assert.AreEqual(false, rbttestON.Checked);
            Assert.AreEqual(false, rbttestOFF.Checked);
            Assert.AreEqual(0, ComFunc.getSelectedRadioButtonValue(gbxtest));

            //ラジオボタンをチェック
            rbttestON.Checked = true;

            //ラジオボタンはチェックされているが、タグが設定されていない場合
            Assert.AreEqual(0, ComFunc.getSelectedRadioButtonValue(gbxtest));

            //タグを設定（数値以外）
            rbttestON.Tag = Const.RackTransferRadioValue.RotationCCW;

            //チェックされているラジオボタンのタグの値が数値以外の場合
            Assert.AreEqual(0, ComFunc.getSelectedRadioButtonValue(gbxtest));

            //タグを設定（数値）
            rbttestON.Tag = 10;

            //チェックされているラジオボタンのタグの値が数値の場合
            Assert.AreEqual(10, ComFunc.getSelectedRadioButtonValue(gbxtest));

        }

        [TestMethod()]
        public void chkExistsResponseTest()
        {

            CommonFunction ComFunc = new CommonFunction();
            Assert.AreEqual(false, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.RackAllUnits, 0));
            Assert.AreEqual(true, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.RackAllUnits, 6));
            Assert.AreEqual(true, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.RackAllUnits, 7));
            Assert.AreEqual(true, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.RackAllUnits, 8));

            Assert.AreEqual(false, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.TipCellCaseTransferUnit, 0));
            Assert.AreEqual(true, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.TipCellCaseTransferUnit, 2));

            Assert.AreEqual(false, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.ReagentStorageUnit, 0));
            Assert.AreEqual(true, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.ReagentStorageUnit, 9));
            Assert.AreEqual(true, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.ReagentStorageUnit, 10));
            Assert.AreEqual(true, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.ReagentStorageUnit, 11));
            Assert.AreEqual(true, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.ReagentStorageUnit, 12));
            Assert.AreEqual(true, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.ReagentStorageUnit, 16));
            Assert.AreEqual(true, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.ReagentStorageUnit, 17));
            Assert.AreEqual(true, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.ReagentStorageUnit, 18));
            Assert.AreEqual(true, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.ReagentStorageUnit, 19));

            Assert.AreEqual(false, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.SampleDispenseUnit, 0));
            Assert.AreEqual(true, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.SampleDispenseUnit, 2));
            Assert.AreEqual(true, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.SampleDispenseUnit, 3));
            Assert.AreEqual(true, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.SampleDispenseUnit, 4));
            Assert.AreEqual(true, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.SampleDispenseUnit, 7));

            Assert.AreEqual(false, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.ReactionCellTransferUnit, 0));
            Assert.AreEqual(true, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.ReactionCellTransferUnit, 3));
            Assert.AreEqual(true, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.ReactionCellTransferUnit, 4));

            Assert.AreEqual(false, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.TravelerandDisposalUnit, 0));
            Assert.AreEqual(true, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.TravelerandDisposalUnit, 2));

            Assert.AreEqual(false, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.TriggerDispensingUnitandChemiluminescenceMeasUnit, 0));
            Assert.AreEqual(true, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.TriggerDispensingUnitandChemiluminescenceMeasUnit, 6));

            Assert.AreEqual(false, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.Other, 0));
            Assert.AreEqual(true, ComFunc.chkExistsResponse(Const.MaintenanceMainNavi.Other, 99));

        }
    }
}