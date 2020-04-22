namespace Oelco.CarisX.GUI
{
    partial class DlgLogin
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
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            this.btnOk = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.lblUserID = new Infragistics.Win.Misc.UltraLabel();
            this.txtUserID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtPassword = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblPassword = new Infragistics.Win.Misc.UltraLabel();
            this.lblInputErrorMessage = new Infragistics.Win.Misc.UltraLabel();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).BeginInit();
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
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 211);
            this.pnlDialogButton.TabIndex = 1;
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblInputErrorMessage);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblPassword);
            this.pnlDialogMain.ClientArea.Controls.Add(this.txtPassword);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblUserID);
            this.pnlDialogMain.ClientArea.Controls.Add(this.txtUserID);
            this.pnlDialogMain.Size = new System.Drawing.Size(723, 211);
            this.pnlDialogMain.TabIndex = 0;
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(723, 28);
            // 
            // btnOk
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BorderColor = System.Drawing.Color.Transparent;
            appearance4.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance4.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnOk.Appearance = appearance4;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnOk.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance5.BorderColor = System.Drawing.Color.Transparent;
            this.btnOk.HotTrackAppearance = appearance5;
            this.btnOk.ImageSize = new System.Drawing.Size(0, 0);
            this.btnOk.Location = new System.Drawing.Point(387, 9);
            this.btnOk.Name = "btnOk";
            this.btnOk.Padding = new System.Drawing.Size(10, 0);
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            this.btnOk.PressedAppearance = appearance6;
            this.btnOk.ShowFocusRect = false;
            this.btnOk.ShowOutline = false;
            this.btnOk.Size = new System.Drawing.Size(152, 39);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.Image = global::Oelco.CarisX.Properties.Resources.Image_Exit;
            appearance1.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnCancel.Appearance = appearance1;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance2.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.HotTrackAppearance = appearance2;
            this.btnCancel.ImageSize = new System.Drawing.Size(0, 0);
            this.btnCancel.Location = new System.Drawing.Point(546, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.PressedAppearance = appearance3;
            this.btnCancel.ShowFocusRect = false;
            this.btnCancel.ShowOutline = false;
            this.btnCancel.Size = new System.Drawing.Size(152, 39);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblUserID
            // 
            this.lblUserID.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserID.Location = new System.Drawing.Point(50, 76);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(115, 32);
            this.lblUserID.TabIndex = 4;
            this.lblUserID.Text = "User ID";
            // 
            // txtUserID
            // 
            this.txtUserID.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtUserID.Location = new System.Drawing.Point(240, 67);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(431, 37);
            this.txtUserID.TabIndex = 0;
            this.txtUserID.Enter += new System.EventHandler(this.txtUserID_Enter);
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtPassword.Location = new System.Drawing.Point(240, 127);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(431, 37);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Enter += new System.EventHandler(this.txtPassword_Enter);
            // 
            // lblPassword
            // 
            this.lblPassword.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.Location = new System.Drawing.Point(50, 136);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(115, 32);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Password";
            // 
            // lblInputErrorMessage
            // 
            appearance7.ForeColor = System.Drawing.Color.Red;
            appearance7.TextHAlignAsString = "Center";
            this.lblInputErrorMessage.Appearance = appearance7;
            this.lblInputErrorMessage.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInputErrorMessage.Location = new System.Drawing.Point(50, 176);
            this.lblInputErrorMessage.Name = "lblInputErrorMessage";
            this.lblInputErrorMessage.Size = new System.Drawing.Size(621, 32);
            this.lblInputErrorMessage.TabIndex = 5;
            this.lblInputErrorMessage.Text = "User ID or Password is incorrect.";
            this.lblInputErrorMessage.Visible = false;
            // 
            // DlgLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 268);
            this.Name = "DlgLogin";
            this.Shown += new System.EventHandler(this.DlgLogin_Shown);
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.PerformLayout();
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.NoBorderButton btnOk;
        private Controls.NoBorderButton btnCancel;
        private Infragistics.Win.Misc.UltraLabel lblPassword;
        private Infragistics.Win.Misc.UltraLabel lblUserID;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPassword;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserID;
        private Infragistics.Win.Misc.UltraLabel lblInputErrorMessage;
    }
}