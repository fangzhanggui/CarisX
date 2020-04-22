namespace Oelco.CarisX.GUI
{
    partial class DlgCalibrationConfirm
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
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.lblProtocols = new Infragistics.Win.Misc.UltraLabel();
            this.btnNo = new Controls.NoBorderButton();
            this.btnYes = new Controls.NoBorderButton();
            this.lblProtocolTitle = new Infragistics.Win.Misc.UltraLabel();
            this.lblConfirmMessage = new Infragistics.Win.Misc.UltraLabel();
            this.lblLotTitle = new Infragistics.Win.Misc.UltraLabel();
            this.lblLotNumber = new Infragistics.Win.Misc.UltraLabel();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnYes);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnNo);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 533);
            this.pnlDialogButton.Size = new System.Drawing.Size(656, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblProtocolTitle);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblLotTitle);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblProtocols);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblConfirmMessage);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblLotNumber);
            this.pnlDialogMain.Size = new System.Drawing.Size(656, 533);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(656, 28);
            this.lblDialogTitle.Text = "Dlg";
            // 
            // lblProtocols
            // 
            this.lblProtocols.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.lblProtocols.Location = new System.Drawing.Point(152, 67);
            this.lblProtocols.Name = "lblProtocols";
            this.lblProtocols.Size = new System.Drawing.Size(478, 91);
            this.lblProtocols.TabIndex = 69;
            this.lblProtocols.Text = "メッセージ";
            // 
            // btnNo
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BorderColor = System.Drawing.Color.Transparent;
            appearance4.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance4.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnNo.Appearance = appearance4;
            this.btnNo.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnNo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance5.BorderColor = System.Drawing.Color.Transparent;
            this.btnNo.HotTrackAppearance = appearance5;
            this.btnNo.ImageSize = new System.Drawing.Size(0, 0);
            this.btnNo.Location = new System.Drawing.Point(481, 9);
            this.btnNo.Name = "btnNo";
            this.btnNo.Padding = new System.Drawing.Size(10, 0);
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            this.btnNo.PressedAppearance = appearance6;
            this.btnNo.ShowFocusRect = false;
            this.btnNo.ShowOutline = false;
            this.btnNo.Size = new System.Drawing.Size(152, 39);
            this.btnNo.TabIndex = 156;
            this.btnNo.Text = "2";
            this.btnNo.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnNo.Click += new System.EventHandler(this.No_Click);
            // 
            // btnYes
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance1.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnYes.Appearance = appearance1;
            this.btnYes.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnYes.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance2.BorderColor = System.Drawing.Color.Transparent;
            this.btnYes.HotTrackAppearance = appearance2;
            this.btnYes.ImageSize = new System.Drawing.Size(0, 0);
            this.btnYes.Location = new System.Drawing.Point(323, 9);
            this.btnYes.Name = "btnYes";
            this.btnYes.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnYes.PressedAppearance = appearance3;
            this.btnYes.ShowFocusRect = false;
            this.btnYes.ShowOutline = false;
            this.btnYes.Size = new System.Drawing.Size(152, 39);
            this.btnYes.TabIndex = 156;
            this.btnYes.Text = "1";
            this.btnYes.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnYes.Click += new System.EventHandler(this.Yes_Click);
            // 
            // lblProtocolTitle
            // 
            this.lblProtocolTitle.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.lblProtocolTitle.Location = new System.Drawing.Point(25, 67);
            this.lblProtocolTitle.Name = "lblProtocolTitle";
            this.lblProtocolTitle.Size = new System.Drawing.Size(121, 35);
            this.lblProtocolTitle.TabIndex = 69;
            this.lblProtocolTitle.Text = "メッセージ";
            // 
            // lblConfirmMessage
            // 
            this.lblConfirmMessage.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.lblConfirmMessage.Location = new System.Drawing.Point(26, 205);
            this.lblConfirmMessage.Name = "lblConfirmMessage";
            this.lblConfirmMessage.Size = new System.Drawing.Size(605, 307);
            this.lblConfirmMessage.TabIndex = 69;
            this.lblConfirmMessage.Text = "メッセージ";
            // 
            // lblLotTitle
            // 
            this.lblLotTitle.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.lblLotTitle.Location = new System.Drawing.Point(26, 164);
            this.lblLotTitle.Name = "lblLotTitle";
            this.lblLotTitle.Size = new System.Drawing.Size(121, 35);
            this.lblLotTitle.TabIndex = 69;
            this.lblLotTitle.Text = "メッセージ";
            // 
            // lblLotNumber
            // 
            this.lblLotNumber.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.lblLotNumber.Location = new System.Drawing.Point(152, 164);
            this.lblLotNumber.Name = "lblLotNumber";
            this.lblLotNumber.Size = new System.Drawing.Size(478, 35);
            this.lblLotNumber.TabIndex = 69;
            this.lblLotNumber.Text = "メッセージ";
            // 
            // DlgCalibrationConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Caption = "Dlg";
            this.ClientSize = new System.Drawing.Size(656, 590);
            this.Name = "DlgCalibrationConfirm";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblProtocols;
        private Controls.NoBorderButton btnNo;
        private Controls.NoBorderButton btnYes;
        private Infragistics.Win.Misc.UltraLabel lblProtocolTitle;
        private Infragistics.Win.Misc.UltraLabel lblLotTitle;
        private Infragistics.Win.Misc.UltraLabel lblConfirmMessage;
        private Infragistics.Win.Misc.UltraLabel lblLotNumber;
    }
}