namespace Oelco.CarisX.GUI
{
    partial class DlgAskReceiptNumberInput
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
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.numReceiptNumberMax = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.lblReceiptNumberTitle = new Infragistics.Win.Misc.UltraLabel();
            this.numReceiptNumberMin = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.lblSubTitle = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.btnOk = new Controls.NoBorderButton();
            this.btnCancel = new Controls.NoBorderButton();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numReceiptNumberMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReceiptNumberMin)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOk);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 170);
            this.pnlDialogButton.Size = new System.Drawing.Size(565, 60);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblSubTitle);
            this.pnlDialogMain.ClientArea.Controls.Add(this.numReceiptNumberMax);
            this.pnlDialogMain.ClientArea.Controls.Add(this.ultraLabel1);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblReceiptNumberTitle);
            this.pnlDialogMain.ClientArea.Controls.Add(this.numReceiptNumberMin);
            this.pnlDialogMain.Size = new System.Drawing.Size(565, 170);
            this.pnlDialogMain.TabIndex = 0;
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(565, 28);
            this.lblDialogTitle.Text = "Query to the host computer";
            // 
            // numReceiptNumberMax
            // 
            this.numReceiptNumberMax.Location = new System.Drawing.Point(448, 116);
            this.numReceiptNumberMax.MaskInput = "nnnn";
            this.numReceiptNumberMax.MaxValue = 9999;
            this.numReceiptNumberMax.MinValue = 1;
            this.numReceiptNumberMax.Name = "numReceiptNumberMax";
            this.numReceiptNumberMax.PromptChar = ' ';
            this.numReceiptNumberMax.Size = new System.Drawing.Size(100, 27);
            this.numReceiptNumberMax.TabIndex = 0;
            this.numReceiptNumberMax.Value = 1;
            // 
            // lblReceiptNumberTitle
            // 
            this.lblReceiptNumberTitle.Location = new System.Drawing.Point(27, 119);
            this.lblReceiptNumberTitle.Name = "lblReceiptNumberTitle";
            this.lblReceiptNumberTitle.Size = new System.Drawing.Size(172, 25);
            this.lblReceiptNumberTitle.TabIndex = 4;
            this.lblReceiptNumberTitle.Text = "Receipt Number ";
            // 
            // numReceiptNumberMin
            // 
            this.numReceiptNumberMin.Location = new System.Drawing.Point(310, 116);
            this.numReceiptNumberMin.MaskInput = "nnnn";
            this.numReceiptNumberMin.MaxValue = 9999;
            this.numReceiptNumberMin.MinValue = 1;
            this.numReceiptNumberMin.Name = "numReceiptNumberMin";
            this.numReceiptNumberMin.PromptChar = ' ';
            this.numReceiptNumberMin.Size = new System.Drawing.Size(100, 27);
            this.numReceiptNumberMin.TabIndex = 0;
            this.numReceiptNumberMin.Value = 1;
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.Location = new System.Drawing.Point(27, 54);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(495, 25);
            this.lblSubTitle.TabIndex = 4;
            this.lblSubTitle.Text = "Please specify the receipt No. to be queried.";
            // 
            // ultraLabel1
            // 
            appearance5.ForeColor = System.Drawing.Color.Red;
            this.ultraLabel1.Appearance = appearance5;
            this.ultraLabel1.Font = new System.Drawing.Font("Arial", 8F);
            this.ultraLabel1.Location = new System.Drawing.Point(46, 85);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(494, 25);
            this.ultraLabel1.TabIndex = 4;
            this.ultraLabel1.Text = "If you query to the host computer, the registration data in the edit mode will be" +
    " deleted.";
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
            this.btnOk.ImageSize = new System.Drawing.Size(0, 0);
            this.btnOk.Location = new System.Drawing.Point(234, 9);
            this.btnOk.Name = "btnOk";
            this.btnOk.Padding = new System.Drawing.Size(10, 0);
            this.btnOk.ShowFocusRect = false;
            this.btnOk.ShowOutline = false;
            this.btnOk.Size = new System.Drawing.Size(152, 39);
            this.btnOk.TabIndex = 162;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.BorderColor = System.Drawing.Color.Transparent;
            appearance2.Image = global::Oelco.CarisX.Properties.Resources.Image_Exit;
            appearance2.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnCancel.Appearance = appearance2;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.HotTrackAppearance = appearance3;
            this.btnCancel.ImageSize = new System.Drawing.Size(0, 0);
            this.btnCancel.Location = new System.Drawing.Point(392, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Drawing.Size(10, 0);
            appearance4.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.PressedAppearance = appearance4;
            this.btnCancel.ShowFocusRect = false;
            this.btnCancel.ShowOutline = false;
            this.btnCancel.Size = new System.Drawing.Size(152, 39);
            this.btnCancel.TabIndex = 161;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // DlgAskReceiptNumberInput
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(565, 230);
            this.Name = "DlgAskReceiptNumberInput";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.PerformLayout();
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numReceiptNumberMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReceiptNumberMin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numReceiptNumberMax;
        private Infragistics.Win.Misc.UltraLabel lblReceiptNumberTitle;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numReceiptNumberMin;
        private Infragistics.Win.Misc.UltraLabel lblSubTitle;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Controls.NoBorderButton btnOk;
        private Controls.NoBorderButton btnCancel;
    }
}