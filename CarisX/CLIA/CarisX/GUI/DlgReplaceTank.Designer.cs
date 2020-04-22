namespace Oelco.CarisX.GUI
{
    partial class DlgReplaceTank
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
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnComplete = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnStart = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.chkWashsolutionTank = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkWasteTank = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkWashsolutionTank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkWasteTank)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnStart);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnComplete);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 191);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.chkWasteTank);
            this.pnlDialogMain.ClientArea.Controls.Add(this.chkWashsolutionTank);
            this.pnlDialogMain.Size = new System.Drawing.Size(723, 191);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(723, 28);
            // 
            // btnCancel
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            appearance7.BorderColor = System.Drawing.Color.Transparent;
            appearance7.Image = global::Oelco.CarisX.Properties.Resources.Image_Exit;
            appearance7.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnCancel.Appearance = appearance7;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance8.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.HotTrackAppearance = appearance8;
            this.btnCancel.ImageSize = new System.Drawing.Size(0, 0);
            this.btnCancel.Location = new System.Drawing.Point(546, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Drawing.Size(10, 0);
            appearance9.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.PressedAppearance = appearance9;
            this.btnCancel.ShowFocusRect = false;
            this.btnCancel.ShowOutline = false;
            this.btnCancel.Size = new System.Drawing.Size(152, 39);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnComplete
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BorderColor = System.Drawing.Color.Transparent;
            appearance4.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance4.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnComplete.Appearance = appearance4;
            this.btnComplete.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnComplete.Enabled = false;
            this.btnComplete.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance5.BorderColor = System.Drawing.Color.Transparent;
            this.btnComplete.HotTrackAppearance = appearance5;
            this.btnComplete.ImageSize = new System.Drawing.Size(0, 0);
            this.btnComplete.Location = new System.Drawing.Point(388, 9);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Padding = new System.Drawing.Size(10, 0);
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            this.btnComplete.PressedAppearance = appearance6;
            this.btnComplete.ShowFocusRect = false;
            this.btnComplete.ShowOutline = false;
            this.btnComplete.Size = new System.Drawing.Size(152, 39);
            this.btnComplete.TabIndex = 1;
            this.btnComplete.Text = "COMPLETE";
            this.btnComplete.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // btnStart
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance1.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnStart.Appearance = appearance1;
            this.btnStart.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnStart.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance2.BorderColor = System.Drawing.Color.Transparent;
            this.btnStart.HotTrackAppearance = appearance2;
            this.btnStart.ImageSize = new System.Drawing.Size(0, 0);
            this.btnStart.Location = new System.Drawing.Point(230, 9);
            this.btnStart.Name = "btnStart";
            this.btnStart.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnStart.PressedAppearance = appearance3;
            this.btnStart.ShowFocusRect = false;
            this.btnStart.ShowOutline = false;
            this.btnStart.Size = new System.Drawing.Size(152, 39);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "START";
            this.btnStart.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // chkWashsolutionTank
            // 
            appearance12.BackColor = System.Drawing.Color.Transparent;
            appearance12.FontData.Name = "Georgia";
            appearance12.FontData.SizeInPoints = 12F;
            appearance12.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckOFF;
            this.chkWashsolutionTank.Appearance = appearance12;
            this.chkWashsolutionTank.BackColor = System.Drawing.Color.Transparent;
            this.chkWashsolutionTank.BackColorInternal = System.Drawing.Color.Transparent;
            appearance13.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckON;
            this.chkWashsolutionTank.CheckedAppearance = appearance13;
            this.chkWashsolutionTank.Location = new System.Drawing.Point(92, 117);
            this.chkWashsolutionTank.Name = "chkWashsolutionTank";
            this.chkWashsolutionTank.Size = new System.Drawing.Size(290, 28);
            this.chkWashsolutionTank.Style = Infragistics.Win.EditCheckStyle.Custom;
            this.chkWashsolutionTank.TabIndex = 1;
            this.chkWashsolutionTank.Text = "WASHSOLUTION SUPPLY";
            this.chkWashsolutionTank.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.chkWashsolutionTank.BeforeCheckStateChanged += new Infragistics.Win.ToggleEditorBase.BeforeCheckStateChangedHandler(this.chkWashsolutionTank_BeforeCheckStateChanged);
            // 
            // chkWasteTank
            // 
            appearance10.BackColor = System.Drawing.Color.Transparent;
            appearance10.FontData.Name = "Georgia";
            appearance10.FontData.SizeInPoints = 12F;
            appearance10.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckOFF;
            this.chkWasteTank.Appearance = appearance10;
            this.chkWasteTank.BackColor = System.Drawing.Color.Transparent;
            this.chkWasteTank.BackColorInternal = System.Drawing.Color.Transparent;
            appearance11.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckON;
            this.chkWasteTank.CheckedAppearance = appearance11;
            this.chkWasteTank.Location = new System.Drawing.Point(92, 57);
            this.chkWasteTank.Name = "chkWasteTank";
            this.chkWasteTank.Size = new System.Drawing.Size(290, 28);
            this.chkWasteTank.Style = Infragistics.Win.EditCheckStyle.Custom;
            this.chkWasteTank.TabIndex = 2;
            this.chkWasteTank.Text = "WASTE TANK";
            this.chkWasteTank.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.chkWasteTank.BeforeCheckStateChanged += new Infragistics.Win.ToggleEditorBase.BeforeCheckStateChangedHandler(this.chkWasteTank_BeforeCheckStateChanged);
            // 
            // DlgReplaceTank
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(723, 248);
            this.Name = "DlgReplaceTank";
            this.Shown += new System.EventHandler(this.DlgReplaceTank_Shown);
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkWashsolutionTank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkWasteTank)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.NoBorderButton btnCancel;
        private Controls.NoBorderButton btnComplete;
        private Controls.NoBorderButton btnStart;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkWashsolutionTank;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkWasteTank;
    }
}