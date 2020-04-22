namespace Oelco.CarisX.GUI
{
    partial class DlgCheckCompanyLogo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.gbxCheck = new Infragistics.Win.Misc.UltraGroupBox();
            this.optCheck = new Oelco.Common.GUI.CustomUOptionSet();
            this.btnOK = new Controls.NoBorderButton();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxCheck)).BeginInit();
            this.gbxCheck.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optCheck)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOK);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 227);
            this.pnlDialogButton.Size = new System.Drawing.Size(584, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.gbxCheck);
            this.pnlDialogMain.Size = new System.Drawing.Size(584, 227);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(584, 28);
            // 
            // gbxCheck
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            this.gbxCheck.Appearance = appearance4;
            this.gbxCheck.Controls.Add(this.optCheck);
            this.gbxCheck.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxCheck.Location = new System.Drawing.Point(25, 61);
            this.gbxCheck.Name = "gbxCheck";
            this.gbxCheck.Size = new System.Drawing.Size(376, 130);
            this.gbxCheck.TabIndex = 87;
            this.gbxCheck.Text = "CheckLogo";
            // 
            // optCheck
            // 
            appearance5.FontData.Name = "Georgia";
            appearance5.FontData.SizeInPoints = 12F;
            appearance5.TextHAlignAsString = "Center";
            appearance5.TextVAlignAsString = "Middle";
            this.optCheck.Appearance = appearance5;
            this.optCheck.BackColor = System.Drawing.Color.Transparent;
            this.optCheck.BackColorInternal = System.Drawing.Color.Transparent;
            this.optCheck.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.optCheck.CheckedIndex = 0;
            valueListItem3.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem3.DataValue = "达安";
            valueListItem3.DisplayText = "DAAN";
            valueListItem4.DataValue = "万泰";
            valueListItem4.DisplayText = "WANTAI";
            this.optCheck.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem3,
            valueListItem4});
            this.optCheck.ItemSpacingHorizontal = 10;
            this.optCheck.ItemSpacingVertical = 20;
            this.optCheck.Location = new System.Drawing.Point(33, 26);
            this.optCheck.Name = "optCheck";
            this.optCheck.Size = new System.Drawing.Size(330, 89);
            this.optCheck.TabIndex = 102;
            this.optCheck.Text = "DAAN";
            // 
            // btnOK
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance1.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnOK.Appearance = appearance1;
            this.btnOK.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnOK.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance2.BorderColor = System.Drawing.Color.Transparent;
            this.btnOK.HotTrackAppearance = appearance2;
            this.btnOK.ImageSize = new System.Drawing.Size(0, 0);
            this.btnOK.Location = new System.Drawing.Point(420, 9);
            this.btnOK.Name = "btnOK";
            this.btnOK.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnOK.PressedAppearance = appearance3;
            this.btnOK.ShowFocusRect = false;
            this.btnOK.ShowOutline = false;
            this.btnOK.Size = new System.Drawing.Size(152, 39);
            this.btnOK.TabIndex = 85;
            this.btnOK.Text = "OK";
            this.btnOK.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // DlgCheckCompanyLogo
            // 
            this.ClientSize = new System.Drawing.Size(584, 284);
            this.Name = "DlgCheckCompanyLogo";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbxCheck)).EndInit();
            this.gbxCheck.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optCheck)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.NoBorderButton btnOK;
        private Infragistics.Win.Misc.UltraGroupBox gbxCheck;
        private Oelco.Common.GUI.CustomUOptionSet optCheck;
    }
}
