using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Oelco.CarisX.Const;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.CarisX.DB;

namespace Oelco.CarisX.GUI
{
    public partial class DlgCheckCalibrationMode : Oelco.CarisX.GUI.DlgCarisXBaseSys
    {


        public DlgCheckCalibrationMode()
        {
            InitializeComponent();
            switch (Singleton<Oelco.CarisX.Status.SystemStatus>.Instance.Status)
            {
                case Status.SystemStatusKind.WaitSlaveResponce:
                case Status.SystemStatusKind.Assay:
                case Status.SystemStatusKind.SamplingPause:
                case Status.SystemStatusKind.ReagentExchange:
                    this.btnOK.Enabled = false;
                    break;
                default:
                    this.btnOK.Enabled = true;
                    break;
            }
        }
        protected override void initializeFormComponent()
        {

            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CalibrationModeParameter.CalibrationMode == CalibrationModeParameter.CalibrationModeKind.ModeOne)
            {
                this.optCheck.CheckedIndex = 0;
            }
            else
            {
                this.optCheck.CheckedIndex = 1;
            }
        }
        protected override void setCulture()
        {
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_CALIBRATION_MODE_000;

            this.gbxCheck.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_CALIBRATION_MODE_GBX001;

            this.optCheck.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_CALIBRATION_MODE_OPT001;
            this.optCheck.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_CALIBRATION_MODE_OPT002;

            this.btnOK.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {

            CalibrationModeParameter.CalibrationModeKind calibrationModeKind;
            if (this.optCheck.CheckedIndex == 0)
            {
                calibrationModeKind = CalibrationModeParameter.CalibrationModeKind.ModeOne;
            }
            else
            {
                calibrationModeKind = CalibrationModeParameter.CalibrationModeKind.ModeTwo;
            }
            CalibrationModeParameter.CalibrationModeKind beforeMode = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CalibrationModeParameter.CalibrationMode;
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CalibrationModeParameter.CalibrationMode = calibrationModeKind;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CalibrationModeParameter.CalibrationMode
                     != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.CalibrationModeParameter.CalibrationMode)
            {
                String[] contents = new String[4];
                contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
                contents[1] = lblDialogTitle.Text;
                contents[2] = this.gbxCheck.Text;
                contents[3] = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CalibrationModeParameter.CalibrationMode + CarisX.Properties.Resources.STRING_LOG_MSG_001;
                Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);

            }
            if (beforeMode != calibrationModeKind)
            {
                Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.CalibrationModeKindChanged, calibrationModeKind);
            }


            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Save();

            Singleton<ParameterChangeLogDB>.Instance.CommitParameterChangeLog();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


    }
}
