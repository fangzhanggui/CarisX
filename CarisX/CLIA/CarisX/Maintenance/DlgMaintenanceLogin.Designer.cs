namespace Oelco.CarisX.Maintenance
{
    partial class DlgMaintenanceLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
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
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgMaintenanceLogin));
            this.btnOk = new GUI.Controls.NoBorderButton();
            this.btnCancel = new GUI.Controls.NoBorderButton();
            this.lblLoginReason = new Infragistics.Win.Misc.UltraLabel();
            this.txtLoginReason = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtPassword = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblPassword = new Infragistics.Win.Misc.UltraLabel();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginReason)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOk);
            this.pnlDialogButton.TabIndex = 1;
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblPassword);
            this.pnlDialogMain.ClientArea.Controls.Add(this.txtPassword);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblLoginReason);
            this.pnlDialogMain.ClientArea.Controls.Add(this.txtLoginReason);
            this.pnlDialogMain.Size = new System.Drawing.Size(723, 327);
            this.pnlDialogMain.TabIndex = 0;
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Location = new System.Drawing.Point(25, 8);
            this.lblDialogTitle.Size = new System.Drawing.Size(673, 28);
            // 
            // btnOk
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance4.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnOk.Appearance = appearance4;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnOk.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ImageSize = new System.Drawing.Size(0, 0);
            this.btnOk.Location = new System.Drawing.Point(387, 11);
            this.btnOk.Name = "btnOk";
            this.btnOk.Padding = new System.Drawing.Size(10, 0);
            this.btnOk.ShapeImage = global::Oelco.CarisX.Properties.Resources.SharpImage_Button;
            this.btnOk.Size = new System.Drawing.Size(152, 39);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.Image = global::Oelco.CarisX.Properties.Resources.Image_Exit;
            appearance3.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnCancel.Appearance = appearance3;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ImageSize = new System.Drawing.Size(0, 0);
            this.btnCancel.Location = new System.Drawing.Point(546, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Drawing.Size(10, 0);
            this.btnCancel.ShapeImage = global::Oelco.CarisX.Properties.Resources.SharpImage_Button;
            this.btnCancel.Size = new System.Drawing.Size(152, 39);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblLoginReason
            // 
            this.lblLoginReason.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoginReason.Location = new System.Drawing.Point(50, 102);
            this.lblLoginReason.Name = "lblLoginReason";
            this.lblLoginReason.Size = new System.Drawing.Size(139, 32);
            this.lblLoginReason.TabIndex = 4;
            this.lblLoginReason.Text = "LoginReason";
            // 
            // txtLoginReason
            // 
            this.txtLoginReason.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtLoginReason.Location = new System.Drawing.Point(195, 93);
            this.txtLoginReason.MaxLength = 200;
            this.txtLoginReason.Multiline = true;
            this.txtLoginReason.Name = "txtLoginReason";
            this.txtLoginReason.Size = new System.Drawing.Size(503, 222);
            this.txtLoginReason.TabIndex = 1;
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtPassword.Location = new System.Drawing.Point(195, 46);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(503, 41);
            this.txtPassword.TabIndex = 0;
            // 
            // lblPassword
            // 
            this.lblPassword.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.Location = new System.Drawing.Point(50, 55);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(139, 32);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Password";
            // 
            // DlgMaintenanceLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 384);
            this.Name = "DlgMaintenanceLogin";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.PerformLayout();
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginReason)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GUI.Controls.NoBorderButton btnOk;
        private GUI.Controls.NoBorderButton btnCancel;
        private Infragistics.Win.Misc.UltraLabel lblPassword;
        private Infragistics.Win.Misc.UltraLabel lblLoginReason;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPassword;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtLoginReason;
    }
}