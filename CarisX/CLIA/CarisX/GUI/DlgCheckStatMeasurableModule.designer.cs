namespace Oelco.CarisX.GUI
{
    partial class DlgCheckStatMeasurableModule
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
            this.btnClose = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.lblModule4Name = new Infragistics.Win.Misc.UltraLabel();
            this.lblModule3Name = new Infragistics.Win.Misc.UltraLabel();
            this.lblModule2Name = new Infragistics.Win.Misc.UltraLabel();
            this.lblModule1Name = new Infragistics.Win.Misc.UltraLabel();
            this.lblPatientID = new Infragistics.Win.Misc.UltraLabel();
            this.lblModule4StatOk = new Infragistics.Win.Misc.UltraLabel();
            this.lblModule3StatOk = new Infragistics.Win.Misc.UltraLabel();
            this.lblModule2StatOk = new Infragistics.Win.Misc.UltraLabel();
            this.lblModule1StatOk = new Infragistics.Win.Misc.UltraLabel();
            this.pctModule1Check = new System.Windows.Forms.PictureBox();
            this.pctModule2Check = new System.Windows.Forms.PictureBox();
            this.pctModule3Check = new System.Windows.Forms.PictureBox();
            this.pctModule4Check = new System.Windows.Forms.PictureBox();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctModule1Check)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctModule2Check)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctModule3Check)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctModule4Check)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnClose);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 353);
            this.pnlDialogButton.Size = new System.Drawing.Size(500, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.pctModule4Check);
            this.pnlDialogMain.ClientArea.Controls.Add(this.pctModule3Check);
            this.pnlDialogMain.ClientArea.Controls.Add(this.pctModule2Check);
            this.pnlDialogMain.ClientArea.Controls.Add(this.pctModule1Check);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblModule4StatOk);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblModule3StatOk);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblModule2StatOk);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblModule1StatOk);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblModule4Name);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblModule3Name);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblModule2Name);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblModule1Name);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblPatientID);
            this.pnlDialogMain.Size = new System.Drawing.Size(500, 353);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(500, 28);
            this.lblDialogTitle.Text = "Check STAT measurable Module";
            // 
            // btnClose
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.Image = global::Oelco.CarisX.Properties.Resources.Image_Exit;
            appearance1.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnClose.Appearance = appearance1;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance2.BorderColor = System.Drawing.Color.Transparent;
            this.btnClose.HotTrackAppearance = appearance2;
            this.btnClose.ImageSize = new System.Drawing.Size(0, 0);
            this.btnClose.Location = new System.Drawing.Point(341, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnClose.PressedAppearance = appearance3;
            this.btnClose.ShowFocusRect = false;
            this.btnClose.ShowOutline = false;
            this.btnClose.Size = new System.Drawing.Size(152, 39);
            this.btnClose.TabIndex = 159;
            this.btnClose.Text = "Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblModule4Name
            // 
            this.lblModule4Name.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModule4Name.Location = new System.Drawing.Point(91, 271);
            this.lblModule4Name.Name = "lblModule4Name";
            this.lblModule4Name.Size = new System.Drawing.Size(98, 23);
            this.lblModule4Name.TabIndex = 124;
            this.lblModule4Name.Text = "Module4";
            // 
            // lblModule3Name
            // 
            this.lblModule3Name.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModule3Name.Location = new System.Drawing.Point(91, 222);
            this.lblModule3Name.Name = "lblModule3Name";
            this.lblModule3Name.Size = new System.Drawing.Size(98, 23);
            this.lblModule3Name.TabIndex = 123;
            this.lblModule3Name.Text = "Module3";
            // 
            // lblModule2Name
            // 
            this.lblModule2Name.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModule2Name.Location = new System.Drawing.Point(91, 175);
            this.lblModule2Name.Name = "lblModule2Name";
            this.lblModule2Name.Size = new System.Drawing.Size(98, 23);
            this.lblModule2Name.TabIndex = 122;
            this.lblModule2Name.Text = "Module2";
            // 
            // lblModule1Name
            // 
            this.lblModule1Name.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModule1Name.Location = new System.Drawing.Point(91, 126);
            this.lblModule1Name.Name = "lblModule1Name";
            this.lblModule1Name.Size = new System.Drawing.Size(98, 23);
            this.lblModule1Name.TabIndex = 121;
            this.lblModule1Name.Text = "Module1";
            // 
            // lblPatientID
            // 
            this.lblPatientID.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatientID.Location = new System.Drawing.Point(60, 73);
            this.lblPatientID.Name = "lblPatientID";
            this.lblPatientID.Size = new System.Drawing.Size(392, 23);
            this.lblPatientID.TabIndex = 120;
            this.lblPatientID.Text = "Patient ID: ------";
            // 
            // lblModule4StatOk
            // 
            this.lblModule4StatOk.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModule4StatOk.Location = new System.Drawing.Point(237, 271);
            this.lblModule4StatOk.Name = "lblModule4StatOk";
            this.lblModule4StatOk.Size = new System.Drawing.Size(98, 23);
            this.lblModule4StatOk.TabIndex = 128;
            this.lblModule4StatOk.Text = "STAT OK";
            this.lblModule4StatOk.UseWaitCursor = true;
            // 
            // lblModule3StatOk
            // 
            this.lblModule3StatOk.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModule3StatOk.Location = new System.Drawing.Point(237, 222);
            this.lblModule3StatOk.Name = "lblModule3StatOk";
            this.lblModule3StatOk.Size = new System.Drawing.Size(98, 23);
            this.lblModule3StatOk.TabIndex = 127;
            this.lblModule3StatOk.Text = "STAT OK";
            this.lblModule3StatOk.UseWaitCursor = true;
            // 
            // lblModule2StatOk
            // 
            this.lblModule2StatOk.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModule2StatOk.Location = new System.Drawing.Point(237, 175);
            this.lblModule2StatOk.Name = "lblModule2StatOk";
            this.lblModule2StatOk.Size = new System.Drawing.Size(98, 23);
            this.lblModule2StatOk.TabIndex = 126;
            this.lblModule2StatOk.Text = "STAT OK";
            this.lblModule2StatOk.UseWaitCursor = true;
            // 
            // lblModule1StatOk
            // 
            this.lblModule1StatOk.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModule1StatOk.Location = new System.Drawing.Point(237, 126);
            this.lblModule1StatOk.Name = "lblModule1StatOk";
            this.lblModule1StatOk.Size = new System.Drawing.Size(98, 23);
            this.lblModule1StatOk.TabIndex = 125;
            this.lblModule1StatOk.Text = "STAT OK";
            this.lblModule1StatOk.UseWaitCursor = true;
            // 
            // pctModule1Check
            // 
            this.pctModule1Check.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckON;
            this.pctModule1Check.Location = new System.Drawing.Point(383, 123);
            this.pctModule1Check.Name = "pctModule1Check";
            this.pctModule1Check.Size = new System.Drawing.Size(26, 26);
            this.pctModule1Check.TabIndex = 129;
            this.pctModule1Check.TabStop = false;
            // 
            // pctModule2Check
            // 
            this.pctModule2Check.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckON;
            this.pctModule2Check.Location = new System.Drawing.Point(383, 172);
            this.pctModule2Check.Name = "pctModule2Check";
            this.pctModule2Check.Size = new System.Drawing.Size(26, 26);
            this.pctModule2Check.TabIndex = 130;
            this.pctModule2Check.TabStop = false;
            // 
            // pctModule3Check
            // 
            this.pctModule3Check.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckON;
            this.pctModule3Check.Location = new System.Drawing.Point(383, 219);
            this.pctModule3Check.Name = "pctModule3Check";
            this.pctModule3Check.Size = new System.Drawing.Size(26, 26);
            this.pctModule3Check.TabIndex = 131;
            this.pctModule3Check.TabStop = false;
            // 
            // pctModule4Check
            // 
            this.pctModule4Check.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckON;
            this.pctModule4Check.Location = new System.Drawing.Point(383, 268);
            this.pctModule4Check.Name = "pctModule4Check";
            this.pctModule4Check.Size = new System.Drawing.Size(26, 26);
            this.pctModule4Check.TabIndex = 132;
            this.pctModule4Check.TabStop = false;
            // 
            // DlgCheckStatMeasurableModule
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(500, 410);
            this.Name = "DlgCheckStatMeasurableModule";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pctModule1Check)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctModule2Check)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctModule3Check)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctModule4Check)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.NoBorderButton btnClose;
        private Infragistics.Win.Misc.UltraLabel lblModule4StatOk;
        private Infragistics.Win.Misc.UltraLabel lblModule3StatOk;
        private Infragistics.Win.Misc.UltraLabel lblModule2StatOk;
        private Infragistics.Win.Misc.UltraLabel lblModule1StatOk;
        private Infragistics.Win.Misc.UltraLabel lblModule4Name;
        private Infragistics.Win.Misc.UltraLabel lblModule3Name;
        private Infragistics.Win.Misc.UltraLabel lblModule2Name;
        private Infragistics.Win.Misc.UltraLabel lblModule1Name;
        private Infragistics.Win.Misc.UltraLabel lblPatientID;
        private System.Windows.Forms.PictureBox pctModule4Check;
        private System.Windows.Forms.PictureBox pctModule3Check;
        private System.Windows.Forms.PictureBox pctModule2Check;
        private System.Windows.Forms.PictureBox pctModule1Check;
    }
}