namespace Oelco.CarisX.GUI
{
    partial class DlgSysWashDispense
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(System.Boolean disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.numPretrigger = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numWashSolution = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.lblPretriggerUnit = new Infragistics.Win.Misc.UltraLabel();
            this.lblWashSolutionUnit = new Infragistics.Win.Misc.UltraLabel();
            this.lblPretrigger = new Infragistics.Win.Misc.UltraLabel();
            this.lblWashSolution = new Infragistics.Win.Misc.UltraLabel();
            this.btnOK = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.lblTrigger = new Infragistics.Win.Misc.UltraLabel();
            this.lblTriggerUnit = new Infragistics.Win.Misc.UltraLabel();
            this.numTrigger = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPretrigger)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWashSolution)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTrigger)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOK);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 209);
            this.pnlDialogButton.Size = new System.Drawing.Size(584, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.numPretrigger);
            this.pnlDialogMain.ClientArea.Controls.Add(this.numTrigger);
            this.pnlDialogMain.ClientArea.Controls.Add(this.numWashSolution);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblPretriggerUnit);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblTriggerUnit);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblWashSolutionUnit);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblPretrigger);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblTrigger);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblWashSolution);
            this.pnlDialogMain.Size = new System.Drawing.Size(584, 209);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(584, 28);
            // 
            // numPretrigger
            // 
            this.numPretrigger.Location = new System.Drawing.Point(410, 112);
            this.numPretrigger.MaskInput = "nnn";
            this.numPretrigger.MaxValue = 999;
            this.numPretrigger.MinValue = 1;
            this.numPretrigger.Name = "numPretrigger";
            this.numPretrigger.PromptChar = ' ';
            this.numPretrigger.Size = new System.Drawing.Size(50, 27);
            this.numPretrigger.TabIndex = 90;
            // 
            // numWashSolution
            // 
            this.numWashSolution.Location = new System.Drawing.Point(410, 70);
            this.numWashSolution.MaskInput = "nnn";
            this.numWashSolution.MaxValue = 999;
            this.numWashSolution.MinValue = 1;
            this.numWashSolution.Name = "numWashSolution";
            this.numWashSolution.PromptChar = ' ';
            this.numWashSolution.Size = new System.Drawing.Size(50, 27);
            this.numWashSolution.TabIndex = 89;
            // 
            // lblPretriggerUnit
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            appearance7.FontData.Name = "default";
            this.lblPretriggerUnit.Appearance = appearance7;
            this.lblPretriggerUnit.Location = new System.Drawing.Point(465, 117);
            this.lblPretriggerUnit.Name = "lblPretriggerUnit";
            this.lblPretriggerUnit.Size = new System.Drawing.Size(56, 23);
            this.lblPretriggerUnit.TabIndex = 88;
            this.lblPretriggerUnit.Text = "µL";
            // 
            // lblWashSolutionUnit
            // 
            appearance9.BackColor = System.Drawing.Color.Transparent;
            appearance9.FontData.Name = "default";
            this.lblWashSolutionUnit.Appearance = appearance9;
            this.lblWashSolutionUnit.Location = new System.Drawing.Point(465, 75);
            this.lblWashSolutionUnit.Name = "lblWashSolutionUnit";
            this.lblWashSolutionUnit.Size = new System.Drawing.Size(56, 23);
            this.lblWashSolutionUnit.TabIndex = 87;
            this.lblWashSolutionUnit.Text = "µL";
            // 
            // lblPretrigger
            // 
            appearance10.BackColor = System.Drawing.Color.Transparent;
            appearance10.FontData.Name = "default";
            this.lblPretrigger.Appearance = appearance10;
            this.lblPretrigger.Location = new System.Drawing.Point(50, 117);
            this.lblPretrigger.Name = "lblPretrigger";
            this.lblPretrigger.Size = new System.Drawing.Size(350, 23);
            this.lblPretrigger.TabIndex = 86;
            this.lblPretrigger.Text = "Pretrigger";
            // 
            // lblWashSolution
            // 
            appearance12.BackColor = System.Drawing.Color.Transparent;
            appearance12.FontData.Name = "default";
            this.lblWashSolution.Appearance = appearance12;
            this.lblWashSolution.Location = new System.Drawing.Point(50, 75);
            this.lblWashSolution.Name = "lblWashSolution";
            this.lblWashSolution.Size = new System.Drawing.Size(350, 23);
            this.lblWashSolution.TabIndex = 85;
            this.lblWashSolution.Text = "Wash solution";
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
            this.btnOK.Location = new System.Drawing.Point(249, 9);
            this.btnOK.Name = "btnOK";
            this.btnOK.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnOK.PressedAppearance = appearance3;
            this.btnOK.ShowFocusRect = false;
            this.btnOK.ShowOutline = false;
            this.btnOK.Size = new System.Drawing.Size(152, 39);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
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
            this.btnCancel.Location = new System.Drawing.Point(407, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Drawing.Size(10, 0);
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.PressedAppearance = appearance6;
            this.btnCancel.ShowFocusRect = false;
            this.btnCancel.ShowOutline = false;
            this.btnCancel.Size = new System.Drawing.Size(152, 39);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblTrigger
            // 
            appearance11.BackColor = System.Drawing.Color.Transparent;
            appearance11.FontData.Name = "default";
            this.lblTrigger.Appearance = appearance11;
            this.lblTrigger.Location = new System.Drawing.Point(50, 159);
            this.lblTrigger.Name = "lblTrigger";
            this.lblTrigger.Size = new System.Drawing.Size(350, 23);
            this.lblTrigger.TabIndex = 86;
            this.lblTrigger.Text = "Trigger";
            // 
            // lblTriggerUnit
            // 
            appearance8.BackColor = System.Drawing.Color.Transparent;
            appearance8.FontData.Name = "default";
            this.lblTriggerUnit.Appearance = appearance8;
            this.lblTriggerUnit.Location = new System.Drawing.Point(465, 159);
            this.lblTriggerUnit.Name = "lblTriggerUnit";
            this.lblTriggerUnit.Size = new System.Drawing.Size(56, 23);
            this.lblTriggerUnit.TabIndex = 88;
            this.lblTriggerUnit.Text = "µL";
            // 
            // numTrigger
            // 
            this.numTrigger.Location = new System.Drawing.Point(410, 154);
            this.numTrigger.MaskInput = "nnn";
            this.numTrigger.MaxValue = 999;
            this.numTrigger.MinValue = 1;
            this.numTrigger.Name = "numTrigger";
            this.numTrigger.PromptChar = ' ';
            this.numTrigger.Size = new System.Drawing.Size(50, 27);
            this.numTrigger.TabIndex = 90;
            // 
            // DlgSysWashDispense
            // 
            this.ClientSize = new System.Drawing.Size(584, 266);
            this.Name = "DlgSysWashDispense";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.PerformLayout();
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numPretrigger)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWashSolution)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTrigger)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numPretrigger;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numWashSolution;
        private Infragistics.Win.Misc.UltraLabel lblPretriggerUnit;
        private Infragistics.Win.Misc.UltraLabel lblWashSolutionUnit;
        private Infragistics.Win.Misc.UltraLabel lblPretrigger;
        private Infragistics.Win.Misc.UltraLabel lblWashSolution;
        private Controls.NoBorderButton btnOK;
        private Controls.NoBorderButton btnCancel;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTrigger;
        private Infragistics.Win.Misc.UltraLabel lblTriggerUnit;
        private Infragistics.Win.Misc.UltraLabel lblTrigger;
    }
}
