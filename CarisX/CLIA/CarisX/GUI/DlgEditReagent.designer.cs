namespace Oelco.CarisX.GUI
{
    partial class DlgEditReagent
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
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            this.lblTitleLotNo = new Infragistics.Win.Misc.UltraLabel();
            this.lblTitleRemainTest = new Infragistics.Win.Misc.UltraLabel();
            this.numRemainTest = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.btnOk = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.lblTitleNo = new Infragistics.Win.Misc.UltraLabel();
            this.lblNo = new Infragistics.Win.Misc.UltraLabel();
            this.txtLotNo = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblRemainTestUnit = new Infragistics.Win.Misc.UltraLabel();
            this.lblTitleSerialNo = new Infragistics.Win.Misc.UltraLabel();
            this.numSerialNo = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRemainTest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLotNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSerialNo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOk);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 214);
            this.pnlDialogButton.Size = new System.Drawing.Size(421, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblTitleSerialNo);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblNo);
            this.pnlDialogMain.ClientArea.Controls.Add(this.numSerialNo);
            this.pnlDialogMain.ClientArea.Controls.Add(this.txtLotNo);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblTitleNo);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblTitleLotNo);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblTitleRemainTest);
            this.pnlDialogMain.ClientArea.Controls.Add(this.numRemainTest);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblRemainTestUnit);
            this.pnlDialogMain.Size = new System.Drawing.Size(421, 214);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(421, 28);
            // 
            // lblTitleLotNo
            // 
            appearance11.BackColor = System.Drawing.Color.Transparent;
            appearance11.BorderColor = System.Drawing.Color.Black;
            this.lblTitleLotNo.Appearance = appearance11;
            this.lblTitleLotNo.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblTitleLotNo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleLotNo.ImageTransparentColor = System.Drawing.Color.Empty;
            this.lblTitleLotNo.Location = new System.Drawing.Point(40, 128);
            this.lblTitleLotNo.Name = "lblTitleLotNo";
            this.lblTitleLotNo.Size = new System.Drawing.Size(158, 23);
            this.lblTitleLotNo.TabIndex = 4;
            this.lblTitleLotNo.Text = "Lot No.";
            // 
            // lblTitleRemainTest
            // 
            appearance12.BackColor = System.Drawing.Color.Transparent;
            appearance12.BorderColor = System.Drawing.Color.Black;
            this.lblTitleRemainTest.Appearance = appearance12;
            this.lblTitleRemainTest.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblTitleRemainTest.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleRemainTest.ImageTransparentColor = System.Drawing.Color.Empty;
            this.lblTitleRemainTest.Location = new System.Drawing.Point(40, 84);
            this.lblTitleRemainTest.Name = "lblTitleRemainTest";
            this.lblTitleRemainTest.Size = new System.Drawing.Size(158, 23);
            this.lblTitleRemainTest.TabIndex = 3;
            this.lblTitleRemainTest.Text = "Remain";
            // 
            // numRemainTest
            // 
            this.numRemainTest.FormatString = "";
            this.numRemainTest.Location = new System.Drawing.Point(204, 79);
            this.numRemainTest.MaskInput = "nnnnnnn";
            this.numRemainTest.MaxValue = 9999999;
            this.numRemainTest.MinValue = 0;
            this.numRemainTest.Name = "numRemainTest";
            this.numRemainTest.PromptChar = ' ';
            this.numRemainTest.Size = new System.Drawing.Size(126, 27);
            this.numRemainTest.TabIndex = 5;
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
            this.btnOk.Location = new System.Drawing.Point(86, 9);
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
            this.btnCancel.Location = new System.Drawing.Point(244, 9);
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
            // lblTitleNo
            // 
            appearance10.BackColor = System.Drawing.Color.Transparent;
            appearance10.BorderColor = System.Drawing.Color.Black;
            this.lblTitleNo.Appearance = appearance10;
            this.lblTitleNo.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblTitleNo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleNo.ImageTransparentColor = System.Drawing.Color.Empty;
            this.lblTitleNo.Location = new System.Drawing.Point(40, 43);
            this.lblTitleNo.Name = "lblTitleNo";
            this.lblTitleNo.Size = new System.Drawing.Size(114, 23);
            this.lblTitleNo.TabIndex = 6;
            this.lblTitleNo.Text = "No.";
            // 
            // lblNo
            // 
            appearance8.BackColor = System.Drawing.Color.Transparent;
            appearance8.BorderColor = System.Drawing.Color.Black;
            this.lblNo.Appearance = appearance8;
            this.lblNo.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblNo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNo.ImageTransparentColor = System.Drawing.Color.Empty;
            this.lblNo.Location = new System.Drawing.Point(236, 43);
            this.lblNo.Name = "lblNo";
            this.lblNo.Size = new System.Drawing.Size(114, 23);
            this.lblNo.TabIndex = 7;
            this.lblNo.Text = "0";
            // 
            // txtLotNo
            // 
            appearance9.TextHAlignAsString = "Right";
            this.txtLotNo.Appearance = appearance9;
            this.txtLotNo.Location = new System.Drawing.Point(204, 123);
            this.txtLotNo.MaxLength = 8;
            this.txtLotNo.Name = "txtLotNo";
            this.txtLotNo.Size = new System.Drawing.Size(126, 27);
            this.txtLotNo.TabIndex = 8;
            this.txtLotNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLotNo_KeyDown);
            // 
            // lblRemainTestUnit
            // 
            appearance13.BackColor = System.Drawing.Color.Transparent;
            appearance13.BorderColor = System.Drawing.Color.Black;
            this.lblRemainTestUnit.Appearance = appearance13;
            this.lblRemainTestUnit.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblRemainTestUnit.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemainTestUnit.ImageTransparentColor = System.Drawing.Color.Empty;
            this.lblRemainTestUnit.Location = new System.Drawing.Point(346, 84);
            this.lblRemainTestUnit.Name = "lblRemainTestUnit";
            this.lblRemainTestUnit.Size = new System.Drawing.Size(50, 23);
            this.lblRemainTestUnit.TabIndex = 3;
            this.lblRemainTestUnit.Text = "Test";
            // 
            // lblTitleSerialNo
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            appearance7.BorderColor = System.Drawing.Color.Black;
            this.lblTitleSerialNo.Appearance = appearance7;
            this.lblTitleSerialNo.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblTitleSerialNo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleSerialNo.ImageTransparentColor = System.Drawing.Color.Empty;
            this.lblTitleSerialNo.Location = new System.Drawing.Point(40, 171);
            this.lblTitleSerialNo.Name = "lblTitleSerialNo";
            this.lblTitleSerialNo.Size = new System.Drawing.Size(158, 23);
            this.lblTitleSerialNo.TabIndex = 9;
            this.lblTitleSerialNo.Text = "Serial No.";
            // 
            // numSerialNo
            // 
            this.numSerialNo.FormatString = "";
            this.numSerialNo.Location = new System.Drawing.Point(204, 166);
            this.numSerialNo.MaskInput = "nnnnn";
            this.numSerialNo.MaxValue = 99999;
            this.numSerialNo.MinValue = 0;
            this.numSerialNo.Name = "numSerialNo";
            this.numSerialNo.PromptChar = ' ';
            this.numSerialNo.Size = new System.Drawing.Size(126, 27);
            this.numSerialNo.TabIndex = 10;
            // 
            // DlgEditReagent
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(421, 271);
            this.Name = "DlgEditReagent";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.PerformLayout();
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numRemainTest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLotNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSerialNo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblTitleLotNo;
        private Infragistics.Win.Misc.UltraLabel lblTitleRemainTest;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numRemainTest;
        private Controls.NoBorderButton btnOk;
        private Controls.NoBorderButton btnCancel;
        private Infragistics.Win.Misc.UltraLabel lblNo;
        private Infragistics.Win.Misc.UltraLabel lblTitleNo;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtLotNo;
        private Infragistics.Win.Misc.UltraLabel lblRemainTestUnit;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numSerialNo;
        private Infragistics.Win.Misc.UltraLabel lblTitleSerialNo;
    }
}