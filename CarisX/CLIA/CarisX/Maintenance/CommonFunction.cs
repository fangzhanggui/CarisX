using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinTabControl;
using Oelco.CarisX.Const;
using Oelco.Common.GUI;
using System.Drawing;
using System.Windows.Forms;

namespace Oelco.CarisX.Maintenance
{
    public class CommonFunction
    {
        /// <summary>
        /// 機能番号のリストボックスの設定
        /// </summary>
        public ListBox setSequenceListBox(ListBox Listbox, object sequencelist)
        {
            Listbox.DisplayMember = "Name";
            Listbox.ValueMember = "No";
            Listbox.DataSource = sequencelist;
            return Listbox;
        }

        /// <summary>
        /// 対象のグループボックスから選択されているラジオボタンを取得
        /// </summary>
        public int getSelectedRadioButtonValue(UltraGroupBox gbx)
        {
            int rtnint = 0;
            foreach (Control ctl in gbx.Controls)
            {
                if (ctl is RadioButton rbtctl)
                {
                    if (rbtctl.Checked && !(rbtctl.Tag is null))
                    {
                        int.TryParse(rbtctl.Tag.ToString(), out rtnint);
                    }
                }
            }
            return rtnint;
        }

        /// <summary>
        /// 対象のグループボックスから選択されているラジオボタンを取得
        /// </summary>
        public int getSelectedCustomURadioButtonValue(UltraGroupBox gbx)
        {
            int rtnint = 0;
            foreach (Control ctl in gbx.Controls)
            {
                if (ctl is CustomURadioButton rbtctl)
                {
                    if (rbtctl.Checked && !(rbtctl.Tag is null))
                    {
                        int.TryParse(rbtctl.Tag.ToString(), out rtnint);
                    }
                }
            }
            return rtnint;
        }

        /// <summary>
        /// 対象のグループボックスの内、指定した値を持っているラジオボタンを選択
        /// </summary>
        public void setSelectedCustomURadioButtonCheck(UltraGroupBox gbx, object value)
        {
            foreach (Control ctl in gbx.Controls)
            {
                if (ctl is CustomURadioButton rbtctl)
                {
                    if (!(rbtctl.Tag is null) && (rbtctl.Tag.ToString() == value.ToString()))
                    {
                        rbtctl.Checked = true;
                    }
                }
            }
            return;
        }


        /// <summary>
        /// 引数の機能がレスポンスのある機能かどうかを返す
        /// </summary>
        public bool chkExistsResponse(MaintenanceMainNavi Unit, int FuncNo)
        {
            bool chkReslut = false;

            switch (Unit)
            {
                case MaintenanceMainNavi.RackAllUnits:
                    switch (FuncNo)
                    {
                        case (int)RackTransferSequence.RackIDReading:
                        case (int)RackTransferSequence.SampleIDReading:
                        case (int)RackTransferSequence.SampleCupTubeCheck:
                        case (int)RackTransferSequence.AllRackOperation:
                            chkReslut = true;
                            break;
                    }
                    break;

                case MaintenanceMainNavi.TipCellCaseTransferUnit:
                    switch (FuncNo)
                    {
                        case (int)CaseTransferSequence.AlltheCasesChecking:
                            chkReslut = true;
                            break;
                    }
                    break;

                case MaintenanceMainNavi.ReagentStorageUnit:
                    switch (FuncNo)
                    {
                        case (int)ReagentStorageSequence.RBottleCheck:
                        case (int)ReagentStorageSequence.MBottlesCheck:
                        case (int)ReagentStorageSequence.RBottleBCID:
                        case (int)ReagentStorageSequence.MBottlesBCID:
                        case (int)ReagentStorageSequence.RBottleCheckEX1:
                        case (int)ReagentStorageSequence.MBottlesCheckEX1:
                        case (int)ReagentStorageSequence.RBottleBCIDEX1:
                        case (int)ReagentStorageSequence.MBottlesBCIDEX1:
                            chkReslut = true;
                            break;
                    }
                    break;

                case MaintenanceMainNavi.SampleDispenseUnit:
                    switch (FuncNo)
                    {
                        case (int)SampleDispenseSequence.RackDispensing:
                        case (int)SampleDispenseSequence.STATorExternalTransferDispensing:
                        case (int)SampleDispenseSequence.ReactionTableDispensing:
                        case (int)SampleDispenseSequence.DetectionSensor:
                            chkReslut = true;
                            break;
                    }
                    break;

                case MaintenanceMainNavi.ReactionCellTransferUnit:
                    switch (FuncNo)
                    {
                        case (int)ReactionCellTransferSequence.ReleasestoSettingPosition:
                        case (int)ReactionCellTransferSequence.CatchesfromStorageandRelease:
                            chkReslut = true;
                            break;
                    }
                    break;

                case MaintenanceMainNavi.ReactionTableUnit:
                    switch (FuncNo)
                    {
                        case (int)ReactionTableSequence.Mixing:
                            chkReslut = true;
                            break;
                    }
                    break;

                case MaintenanceMainNavi.BFTableUnit:
                    switch (FuncNo)
                    {
                        case (int)BFTableSequence.MixingR2:
                        case (int)BFTableSequence.MixingBF1:
                        case (int)BFTableSequence.MixingPTr:
                        case (int)BFTableSequence.MixingBF2:
                            chkReslut = true;
                            break;
                    }
                    break;

                case MaintenanceMainNavi.TravelerandDisposalUnit:
                    switch (FuncNo)
                    {
                        case (int)TravelerDisposalSequence.DisposalReactionCell:
                            chkReslut = true;
                            break;
                    }
                    break;

                case MaintenanceMainNavi.ReagentDispense1Unit:
                    switch (FuncNo)
                    {
                        case (int)R1DispenceSequence.R1DispenseandProbeWashing:
                        case (int)R1DispenceSequence.R2DispenseandProbeWashing:
                        case (int)R1DispenceSequence.MDispenseandProbeWashing:
                        case (int)R1DispenceSequence.DetectionSensor:
                            chkReslut = true;
                            break;
                    }
                    break;

                case MaintenanceMainNavi.ReagentDispense2Unit:
                    switch (FuncNo)
                    {
                        case (int)R2DispenceSequence.R2DispenseandProbeWashing:
                        case (int)R2DispenceSequence.MDispenseandProbeWashing:
                        case (int)R2DispenceSequence.DetectionSensor:
                            chkReslut = true;
                            break;
                    }
                    break;

                case MaintenanceMainNavi.TriggerDispensingUnitandChemiluminescenceMeasUnit:
                    switch (FuncNo)
                    {
                        case (int)TriggerDispenseSequence.Measurement:
                            chkReslut = true;
                            break;
                    }
                    break;

                case MaintenanceMainNavi.Other:
                    switch (FuncNo)
                    {
                        case (int)OtherSequence.SoftWareTEST1:
                        case (int)OtherSequence.SoftWareTEST2:
                        case (int)OtherSequence.CPLDRead:
                            chkReslut = true;
                            break;
                    }
                    break;
            }

            return chkReslut;
        }

        public void SetControlSettings(Control Parent)
        {

            foreach (Control ctl in Parent.Controls)
            {
                if (ctl is UltraGroupBox gbxctl)
                {
                    gbxctl.Appearance.BackColor = System.Drawing.Color.Moccasin;
                    gbxctl.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.None;
                    gbxctl.Appearance.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
                    gbxctl.ContentAreaAppearance.BorderColor = Color.LightSlateGray;
                }

                if (ctl is UltraTabControl tabctl)
                {
                    tabctl.ViewStyle = ViewStyle.Standard;
                    tabctl.AllowTabClosing = false;
                    tabctl.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
                    tabctl.TabStop = false;

                    tabctl.ActiveTabAppearance.BackColor = default(Color);
                    tabctl.ActiveTabAppearance.BackColor2 = default(Color);
                    tabctl.ActiveTabAppearance.BackGradientAlignment = Infragistics.Win.GradientAlignment.Default;
                    tabctl.ActiveTabAppearance.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
                    tabctl.ActiveTabAppearance.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_TabONRight;

                    tabctl.Appearance.BackColor = System.Drawing.Color.Moccasin;
                    tabctl.Appearance.BackColor2 = default(Color);
                    tabctl.Appearance.BorderColor = System.Drawing.Color.Black;
                    tabctl.Appearance.BorderColor3DBase = System.Drawing.Color.Transparent;
                    tabctl.Appearance.ForeColor = System.Drawing.Color.White;
                    tabctl.Appearance.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
                    tabctl.Appearance.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_TabOFFRight;

                    tabctl.ClientAreaAppearance.BackColor = System.Drawing.Color.Moccasin;
                    tabctl.ClientAreaAppearance.BorderColor3DBase = System.Drawing.Color.Moccasin;
                    tabctl.ClientAreaAppearance.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Tiled;
                    tabctl.ClientAreaAppearance.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Transparent;

                    tabctl.TabHeaderAreaAppearance.BackColor = System.Drawing.Color.Moccasin;
                }

                Control ctlParent = ctl;
                SetControlSettings(ctlParent);
            }
        }


    }
}
