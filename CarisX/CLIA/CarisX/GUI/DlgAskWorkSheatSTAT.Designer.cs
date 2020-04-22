namespace Oelco.CarisX.GUI
{
    partial class DlgAskWorkSheatSTAT
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( System.Boolean disposing )
        {
            if (disposing && ( components != null ))
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            this.lblSubTitle = new Infragistics.Win.Misc.UltraLabel();
            this.btnOk = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.lblSampleID = new Infragistics.Win.Misc.UltraLabel();
            this.txtSampleID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblCupKind = new Infragistics.Win.Misc.UltraLabel();
            this.cmbCupKind = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblNotes = new Infragistics.Win.Misc.UltraLabel();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSampleID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCupKind)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOk);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 200);
            this.pnlDialogButton.Size = new System.Drawing.Size(565, 60);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblNotes);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblCupKind);
            this.pnlDialogMain.ClientArea.Controls.Add(this.cmbCupKind);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblSampleID);
            this.pnlDialogMain.ClientArea.Controls.Add(this.txtSampleID);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblSubTitle);
            this.pnlDialogMain.Size = new System.Drawing.Size(565, 200);
            this.pnlDialogMain.TabIndex = 0;
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(565, 28);
            this.lblDialogTitle.Text = "Query to the host computer";
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.Location = new System.Drawing.Point(27, 54);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(517, 25);
            this.lblSubTitle.TabIndex = 4;
            this.lblSubTitle.Text = "Please specify the Sample ID to be queried.";
            // 
            // btnOk
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance1.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnOk.Appearance = appearance1;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnOk.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance2.BorderColor = System.Drawing.Color.Transparent;
            this.btnOk.HotTrackAppearance = appearance2;
            this.btnOk.ImageSize = new System.Drawing.Size(0, 0);
            this.btnOk.Location = new System.Drawing.Point(234, 9);
            this.btnOk.Name = "btnOk";
            this.btnOk.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnOk.PressedAppearance = appearance3;
            this.btnOk.ShowFocusRect = false;
            this.btnOk.ShowOutline = false;
            this.btnOk.Size = new System.Drawing.Size(152, 39);
            this.btnOk.TabIndex = 162;
            this.btnOk.Text = "OK";
            this.btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BorderColor = System.Drawing.Color.Transparent;
            appearance4.Image = global::Oelco.CarisX.Properties.Resources.Image_Exit;
            appearance4.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnCancel.Appearance = appearance4;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance5.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.HotTrackAppearance = appearance5;
            this.btnCancel.ImageSize = new System.Drawing.Size(0, 0);
            this.btnCancel.Location = new System.Drawing.Point(392, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Drawing.Size(10, 0);
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.PressedAppearance = appearance6;
            this.btnCancel.ShowFocusRect = false;
            this.btnCancel.ShowOutline = false;
            this.btnCancel.Size = new System.Drawing.Size(152, 39);
            this.btnCancel.TabIndex = 161;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblSampleID
            // 
            this.lblSampleID.Font = new System.Drawing.Font("Arial", 14.12F, System.Drawing.FontStyle.Bold);
            this.lblSampleID.Location = new System.Drawing.Point(27, 116);
            this.lblSampleID.Name = "lblSampleID";
            this.lblSampleID.Size = new System.Drawing.Size(285, 23);
            this.lblSampleID.TabIndex = 27;
            this.lblSampleID.Text = "SampleID";
            // 
            // txtSampleID
            // 
            appearance9.TextHAlignAsString = "Left";
            this.txtSampleID.Appearance = appearance9;
            this.txtSampleID.AutoSize = false;
            this.txtSampleID.Font = new System.Drawing.Font("Arial", 14.71F);
            this.txtSampleID.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtSampleID.Location = new System.Drawing.Point(27, 145);
            this.txtSampleID.MaxLength = 16;
            this.txtSampleID.Name = "txtSampleID";
            this.txtSampleID.Size = new System.Drawing.Size(285, 40);
            this.txtSampleID.TabIndex = 26;
            this.txtSampleID.TextChanged += new System.EventHandler(this.txtSampleID_TectChanged);
            // 
            // lblCupKind
            // 
            this.lblCupKind.Font = new System.Drawing.Font("Arial", 14.12F, System.Drawing.FontStyle.Bold);
            this.lblCupKind.Location = new System.Drawing.Point(318, 116);
            this.lblCupKind.Name = "lblCupKind";
            this.lblCupKind.Size = new System.Drawing.Size(183, 23);
            this.lblCupKind.TabIndex = 29;
            this.lblCupKind.Text = "CupKind";
            // 
            // cmbCupKind
            // 
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.TextVAlignAsString = "Middle";
            this.cmbCupKind.Appearance = appearance8;
            this.cmbCupKind.AutoSize = false;
            this.cmbCupKind.BackColor = System.Drawing.Color.White;
            this.cmbCupKind.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCupKind.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.cmbCupKind.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbCupKind.Font = new System.Drawing.Font("Arial", 12.71F);
            this.cmbCupKind.Location = new System.Drawing.Point(318, 145);
            this.cmbCupKind.Name = "cmbCupKind";
            this.cmbCupKind.Nullable = false;
            this.cmbCupKind.Size = new System.Drawing.Size(183, 40);
            this.cmbCupKind.TabIndex = 28;
            this.cmbCupKind.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lblNotes
            // 
            appearance7.ForeColor = System.Drawing.Color.Red;
            this.lblNotes.Appearance = appearance7;
            this.lblNotes.Font = new System.Drawing.Font("Arial", 8F);
            this.lblNotes.Location = new System.Drawing.Point(27, 85);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(517, 25);
            this.lblNotes.TabIndex = 30;
            this.lblNotes.Text = "If you query to the host computer, the registration data in the edit mode will be" +
    " deleted.";
            // 
            // DlgAskWorkSheatSTAT
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(565, 260);
            this.Name = "DlgAskWorkSheatSTAT";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtSampleID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCupKind)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Infragistics.Win.Misc.UltraLabel lblSubTitle;
        private Controls.NoBorderButton btnOk;
        private Controls.NoBorderButton btnCancel;
        private Infragistics.Win.Misc.UltraLabel lblSampleID;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSampleID;
        private Infragistics.Win.Misc.UltraLabel lblCupKind;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbCupKind;
        private Infragistics.Win.Misc.UltraLabel lblNotes;
    }
}