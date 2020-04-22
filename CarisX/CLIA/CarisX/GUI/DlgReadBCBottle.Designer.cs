namespace Oelco.CarisX.GUI
{
    partial class DlgReadBCBottle
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
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.lblMessage = new Infragistics.Win.Misc.UltraLabel();
            this.lblTitleBarcode = new Infragistics.Win.Misc.UltraLabel();
            this.txtBarcode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnOK = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarcode)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOK);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 191);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblMessage);
            this.pnlDialogMain.ClientArea.Controls.Add(this.txtBarcode);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblTitleBarcode);
            this.pnlDialogMain.Size = new System.Drawing.Size(723, 191);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(723, 28);
            // 
            // lblMessage
            // 
            appearance7.TextHAlignAsString = "Center";
            appearance7.TextVAlignAsString = "Middle";
            this.lblMessage.Appearance = appearance7;
            this.lblMessage.Location = new System.Drawing.Point(25, 67);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(673, 46);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Text = "Message";
            // 
            // lblTitleBarcode
            // 
            this.lblTitleBarcode.Location = new System.Drawing.Point(146, 123);
            this.lblTitleBarcode.Name = "lblTitleBarcode";
            this.lblTitleBarcode.Size = new System.Drawing.Size(135, 23);
            this.lblTitleBarcode.TabIndex = 1;
            this.lblTitleBarcode.Text = "Barcode";
            // 
            // txtBarcode
            // 
            this.txtBarcode.Location = new System.Drawing.Point(287, 119);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(307, 27);
            this.txtBarcode.TabIndex = 2;
            // 
            // btnCancel
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BorderColor = System.Drawing.Color.Transparent;
            appearance4.Image = global::Oelco.CarisX.Properties.Resources.Image_Exit;
            appearance4.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnCancel.Appearance = appearance4;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance5.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.HotTrackAppearance = appearance5;
            this.btnCancel.ImageSize = new System.Drawing.Size(0, 0);
            this.btnCancel.Location = new System.Drawing.Point(546, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Drawing.Size(10, 0);
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.PressedAppearance = appearance6;
            this.btnCancel.ShowFocusRect = false;
            this.btnCancel.ShowOutline = false;
            this.btnCancel.Size = new System.Drawing.Size(152, 39);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
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
            this.btnOK.Location = new System.Drawing.Point(388, 9);
            this.btnOK.Name = "btnOK";
            this.btnOK.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnOK.PressedAppearance = appearance3;
            this.btnOK.ShowFocusRect = false;
            this.btnOK.ShowOutline = false;
            this.btnOK.Size = new System.Drawing.Size(152, 39);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // DlgReadBCBottle
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(723, 248);
            this.Name = "DlgReadBCBottle";
            this.Shown += new System.EventHandler(this.DlgReadBCBottle_Shown);
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.PerformLayout();
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtBarcode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblMessage;
        private Infragistics.Win.Misc.UltraLabel lblTitleBarcode;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtBarcode;
        private Controls.NoBorderButton btnCancel;
        private Controls.NoBorderButton btnOK;
    }
}