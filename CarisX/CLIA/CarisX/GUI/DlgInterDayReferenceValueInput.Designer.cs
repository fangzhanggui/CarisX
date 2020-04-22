namespace Oelco.CarisX.GUI
{
    partial class DlgInterDayReferenceValueInput
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
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.lblTitleInterDayConcentrationWidth = new Infragistics.Win.Misc.UltraLabel();
            this.lblTitleInterDayMean = new Infragistics.Win.Misc.UltraLabel();
            this.numInterDayMean = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numInterDayConcentrationWidth = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.btnOk = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterDayMean)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInterDayConcentrationWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOk);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 163);
            this.pnlDialogButton.Size = new System.Drawing.Size(360, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblTitleInterDayConcentrationWidth);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblTitleInterDayMean);
            this.pnlDialogMain.ClientArea.Controls.Add(this.numInterDayConcentrationWidth);
            this.pnlDialogMain.ClientArea.Controls.Add(this.numInterDayMean);
            this.pnlDialogMain.Size = new System.Drawing.Size(360, 163);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(360, 28);
            // 
            // lblTitleInterDayConcentrationWidth
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            appearance7.BorderColor = System.Drawing.Color.Black;
            this.lblTitleInterDayConcentrationWidth.Appearance = appearance7;
            this.lblTitleInterDayConcentrationWidth.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblTitleInterDayConcentrationWidth.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleInterDayConcentrationWidth.ImageTransparentColor = System.Drawing.Color.Empty;
            this.lblTitleInterDayConcentrationWidth.Location = new System.Drawing.Point(40, 118);
            this.lblTitleInterDayConcentrationWidth.Name = "lblTitleInterDayConcentrationWidth";
            this.lblTitleInterDayConcentrationWidth.Size = new System.Drawing.Size(114, 23);
            this.lblTitleInterDayConcentrationWidth.TabIndex = 4;
            this.lblTitleInterDayConcentrationWidth.Text = "Conc. Width";
            // 
            // lblTitleInterDayMean
            // 
            appearance8.BackColor = System.Drawing.Color.Transparent;
            appearance8.BorderColor = System.Drawing.Color.Black;
            this.lblTitleInterDayMean.Appearance = appearance8;
            this.lblTitleInterDayMean.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblTitleInterDayMean.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleInterDayMean.ImageTransparentColor = System.Drawing.Color.Empty;
            this.lblTitleInterDayMean.Location = new System.Drawing.Point(40, 71);
            this.lblTitleInterDayMean.Name = "lblTitleInterDayMean";
            this.lblTitleInterDayMean.Size = new System.Drawing.Size(114, 23);
            this.lblTitleInterDayMean.TabIndex = 3;
            this.lblTitleInterDayMean.Text = "Mean";
            // 
            // numInterDayMean
            // 
            this.numInterDayMean.FormatString = "";
            this.numInterDayMean.Location = new System.Drawing.Point(160, 66);
            this.numInterDayMean.Name = "numInterDayMean";
            this.numInterDayMean.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numInterDayMean.PromptChar = ' ';
            this.numInterDayMean.Size = new System.Drawing.Size(161, 27);
            this.numInterDayMean.TabIndex = 5;
            // 
            // numInterDayConcentrationWidth
            // 
            this.numInterDayConcentrationWidth.Location = new System.Drawing.Point(160, 113);
            this.numInterDayConcentrationWidth.MinValue = 0D;
            this.numInterDayConcentrationWidth.Name = "numInterDayConcentrationWidth";
            this.numInterDayConcentrationWidth.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numInterDayConcentrationWidth.PromptChar = ' ';
            this.numInterDayConcentrationWidth.Size = new System.Drawing.Size(161, 27);
            this.numInterDayConcentrationWidth.TabIndex = 5;
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
            this.btnOk.Location = new System.Drawing.Point(25, 9);
            this.btnOk.Name = "btnOk";
            this.btnOk.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnOk.PressedAppearance = appearance3;
            this.btnOk.ShowFocusRect = false;
            this.btnOk.ShowOutline = false;
            this.btnOk.Size = new System.Drawing.Size(152, 39);
            this.btnOk.TabIndex = 160;
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
            this.btnCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance5.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.HotTrackAppearance = appearance5;
            this.btnCancel.ImageSize = new System.Drawing.Size(0, 0);
            this.btnCancel.Location = new System.Drawing.Point(183, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Drawing.Size(10, 0);
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.PressedAppearance = appearance6;
            this.btnCancel.ShowFocusRect = false;
            this.btnCancel.ShowOutline = false;
            this.btnCancel.Size = new System.Drawing.Size(152, 39);
            this.btnCancel.TabIndex = 159;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // DlgInterDayReferenceValueInput
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(360, 220);
            this.Name = "DlgInterDayReferenceValueInput";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.PerformLayout();
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numInterDayMean)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInterDayConcentrationWidth)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblTitleInterDayConcentrationWidth;
        private Infragistics.Win.Misc.UltraLabel lblTitleInterDayMean;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numInterDayConcentrationWidth;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numInterDayMean;
        private Controls.NoBorderButton btnOk;
        private Controls.NoBorderButton btnCancel;
    }
}