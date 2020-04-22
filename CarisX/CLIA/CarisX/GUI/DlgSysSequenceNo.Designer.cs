namespace Oelco.CarisX.GUI
{
    partial class DlgSysSequenceNo
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
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            this.lblStartSequenceNoSpecimen = new Infragistics.Win.Misc.UltraLabel();
            this.numStartSequenceNoSpecimen = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.lblStartSequenceNoStat = new Infragistics.Win.Misc.UltraLabel();
            this.numStartSequenceNoStat = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.lblStartSequenceNoControl = new Infragistics.Win.Misc.UltraLabel();
            this.numStartSequenceNoControl = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.lblStartSequenceNoCalibrator = new Infragistics.Win.Misc.UltraLabel();
            this.numStartSequenceNoCalibrator = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.lblComment = new Infragistics.Win.Misc.UltraLabel();
            this.btnOK = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.lblSequenceNumRangeComment = new Infragistics.Win.Misc.UltraLabel();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStartSequenceNoSpecimen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartSequenceNoStat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartSequenceNoControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartSequenceNoCalibrator)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOK);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 393);
            this.pnlDialogButton.Size = new System.Drawing.Size(584, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblSequenceNumRangeComment);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblComment);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblStartSequenceNoCalibrator);
            this.pnlDialogMain.ClientArea.Controls.Add(this.numStartSequenceNoCalibrator);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblStartSequenceNoControl);
            this.pnlDialogMain.ClientArea.Controls.Add(this.numStartSequenceNoControl);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblStartSequenceNoStat);
            this.pnlDialogMain.ClientArea.Controls.Add(this.numStartSequenceNoStat);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblStartSequenceNoSpecimen);
            this.pnlDialogMain.ClientArea.Controls.Add(this.numStartSequenceNoSpecimen);
            this.pnlDialogMain.Size = new System.Drawing.Size(584, 393);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(584, 28);
            // 
            // lblStartSequenceNoSpecimen
            // 
            appearance11.BackColor = System.Drawing.Color.Transparent;
            appearance11.FontData.Name = "default";
            this.lblStartSequenceNoSpecimen.Appearance = appearance11;
            this.lblStartSequenceNoSpecimen.Location = new System.Drawing.Point(54, 65);
            this.lblStartSequenceNoSpecimen.Name = "lblStartSequenceNoSpecimen";
            this.lblStartSequenceNoSpecimen.Size = new System.Drawing.Size(240, 23);
            this.lblStartSequenceNoSpecimen.TabIndex = 89;
            this.lblStartSequenceNoSpecimen.Text = "Start sequence No.(Specimen)";
            // 
            // numStartSequenceNoSpecimen
            // 
            this.numStartSequenceNoSpecimen.Location = new System.Drawing.Point(472, 60);
            this.numStartSequenceNoSpecimen.MaskInput = "nnnn";
            this.numStartSequenceNoSpecimen.MaxValue = 9999;
            this.numStartSequenceNoSpecimen.MinValue = 1;
            this.numStartSequenceNoSpecimen.Name = "numStartSequenceNoSpecimen";
            this.numStartSequenceNoSpecimen.PromptChar = ' ';
            this.numStartSequenceNoSpecimen.Size = new System.Drawing.Size(50, 27);
            this.numStartSequenceNoSpecimen.TabIndex = 88;
            this.numStartSequenceNoSpecimen.Value = 1;
            // 
            // lblStartSequenceNoStat
            // 
            appearance10.BackColor = System.Drawing.Color.Transparent;
            appearance10.FontData.Name = "default";
            this.lblStartSequenceNoStat.Appearance = appearance10;
            this.lblStartSequenceNoStat.Location = new System.Drawing.Point(54, 118);
            this.lblStartSequenceNoStat.Name = "lblStartSequenceNoStat";
            this.lblStartSequenceNoStat.Size = new System.Drawing.Size(240, 23);
            this.lblStartSequenceNoStat.TabIndex = 91;
            this.lblStartSequenceNoStat.Text = "Start sequence No.(Stat)";
            // 
            // numStartSequenceNoStat
            // 
            this.numStartSequenceNoStat.Location = new System.Drawing.Point(472, 113);
            this.numStartSequenceNoStat.MaskInput = "nnnn";
            this.numStartSequenceNoStat.MaxValue = 9999;
            this.numStartSequenceNoStat.MinValue = 1;
            this.numStartSequenceNoStat.Name = "numStartSequenceNoStat";
            this.numStartSequenceNoStat.PromptChar = ' ';
            this.numStartSequenceNoStat.Size = new System.Drawing.Size(50, 27);
            this.numStartSequenceNoStat.TabIndex = 90;
            this.numStartSequenceNoStat.Value = 1;
            // 
            // lblStartSequenceNoControl
            // 
            appearance9.BackColor = System.Drawing.Color.Transparent;
            appearance9.FontData.Name = "default";
            this.lblStartSequenceNoControl.Appearance = appearance9;
            this.lblStartSequenceNoControl.Location = new System.Drawing.Point(54, 172);
            this.lblStartSequenceNoControl.Name = "lblStartSequenceNoControl";
            this.lblStartSequenceNoControl.Size = new System.Drawing.Size(240, 23);
            this.lblStartSequenceNoControl.TabIndex = 93;
            this.lblStartSequenceNoControl.Text = "Start sequence No.(Control)";
            // 
            // numStartSequenceNoControl
            // 
            this.numStartSequenceNoControl.Location = new System.Drawing.Point(472, 167);
            this.numStartSequenceNoControl.MaskInput = "nnnn";
            this.numStartSequenceNoControl.MaxValue = 9999;
            this.numStartSequenceNoControl.MinValue = 1;
            this.numStartSequenceNoControl.Name = "numStartSequenceNoControl";
            this.numStartSequenceNoControl.PromptChar = ' ';
            this.numStartSequenceNoControl.Size = new System.Drawing.Size(50, 27);
            this.numStartSequenceNoControl.TabIndex = 92;
            this.numStartSequenceNoControl.Value = 1;
            // 
            // lblStartSequenceNoCalibrator
            // 
            appearance8.BackColor = System.Drawing.Color.Transparent;
            appearance8.FontData.Name = "default";
            this.lblStartSequenceNoCalibrator.Appearance = appearance8;
            this.lblStartSequenceNoCalibrator.Location = new System.Drawing.Point(54, 222);
            this.lblStartSequenceNoCalibrator.Name = "lblStartSequenceNoCalibrator";
            this.lblStartSequenceNoCalibrator.Size = new System.Drawing.Size(240, 23);
            this.lblStartSequenceNoCalibrator.TabIndex = 95;
            this.lblStartSequenceNoCalibrator.Text = "Start sequence No.(Calibrator)";
            // 
            // numStartSequenceNoCalibrator
            // 
            this.numStartSequenceNoCalibrator.Location = new System.Drawing.Point(472, 217);
            this.numStartSequenceNoCalibrator.MaskInput = "nnnn";
            this.numStartSequenceNoCalibrator.MaxValue = 9999;
            this.numStartSequenceNoCalibrator.MinValue = 1;
            this.numStartSequenceNoCalibrator.Name = "numStartSequenceNoCalibrator";
            this.numStartSequenceNoCalibrator.PromptChar = ' ';
            this.numStartSequenceNoCalibrator.Size = new System.Drawing.Size(50, 27);
            this.numStartSequenceNoCalibrator.TabIndex = 94;
            this.numStartSequenceNoCalibrator.Value = 1;
            // 
            // lblComment
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            appearance7.FontData.Name = "default";
            appearance7.ForeColor = System.Drawing.Color.Red;
            this.lblComment.Appearance = appearance7;
            this.lblComment.Location = new System.Drawing.Point(50, 310);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(486, 72);
            this.lblComment.TabIndex = 96;
            this.lblComment.Text = "The sequence number has been already issued.Please initialize the assay status us" +
    "ing the system initialization in the option window.";
            this.lblComment.Visible = false;
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
            // lblSequenceNumRangeComment
            // 
            appearance19.BackColor = System.Drawing.Color.Transparent;
            appearance19.FontData.Name = "default";
            appearance19.ForeColor = System.Drawing.Color.Red;
            this.lblSequenceNumRangeComment.Appearance = appearance19;
            this.lblSequenceNumRangeComment.Location = new System.Drawing.Point(50, 260);
            this.lblSequenceNumRangeComment.Name = "lblSequenceNumRangeComment";
            this.lblSequenceNumRangeComment.Size = new System.Drawing.Size(500, 44);
            this.lblSequenceNumRangeComment.TabIndex = 97;
            this.lblSequenceNumRangeComment.Text = "The range of each Sequence number is from \"Initial Sequence No.\" to \"Next larger " +
    "Initial Sequence No. - 1\".";
            // 
            // DlgSysSequenceNo
            // 
            this.ClientSize = new System.Drawing.Size(584, 450);
            this.Name = "DlgSysSequenceNo";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.PerformLayout();
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numStartSequenceNoSpecimen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartSequenceNoStat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartSequenceNoControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartSequenceNoCalibrator)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblStartSequenceNoSpecimen;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numStartSequenceNoSpecimen;
        private Infragistics.Win.Misc.UltraLabel lblStartSequenceNoCalibrator;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numStartSequenceNoCalibrator;
        private Infragistics.Win.Misc.UltraLabel lblStartSequenceNoControl;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numStartSequenceNoControl;
        private Infragistics.Win.Misc.UltraLabel lblStartSequenceNoStat;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numStartSequenceNoStat;
        private Infragistics.Win.Misc.UltraLabel lblComment;
        private Controls.NoBorderButton btnOK;
        private Controls.NoBorderButton btnCancel;
        private Infragistics.Win.Misc.UltraLabel lblSequenceNumRangeComment;
    }
}
