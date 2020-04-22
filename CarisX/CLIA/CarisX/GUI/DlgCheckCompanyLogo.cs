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
    public partial class DlgCheckCompanyLogo : Oelco.CarisX.GUI.DlgCarisXBaseSys
    {
        public DlgCheckCompanyLogo()
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

            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo == CompanyLogoParameter.CompanyLogoKind.LogoOne)
            {
                this.optCheck.CheckedIndex = 0;
            }
            else if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
            {
                this.optCheck.CheckedIndex = 1;
            }
        }
        protected override void setCulture()
        {
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_COMPANY_LOGO_000;

            this.gbxCheck.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_COMPANY_LOGO_GBX001;

            this.optCheck.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_COMPANY_LOGO_OPT001;
            this.optCheck.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_COMPANY_LOGO_OPT002;

            this.btnOK.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            CompanyLogoParameter.CompanyLogoKind companyLogoKind;
            if (this.optCheck.CheckedIndex == 0)
            {
                companyLogoKind = CompanyLogoParameter.CompanyLogoKind.LogoOne;
            }
            else
            {
                companyLogoKind = CompanyLogoParameter.CompanyLogoKind.LogoTwo;
            }

            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo = companyLogoKind;

            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Save();


            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
