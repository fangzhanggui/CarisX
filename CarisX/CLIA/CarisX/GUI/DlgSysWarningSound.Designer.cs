﻿namespace Oelco.CarisX.GUI
{
    partial class DlgSysWarningSound
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
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.cmbToneError = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.gbxBeepSetup = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblVolume = new Infragistics.Win.Misc.UltraLabel();
            this.cmbVolume = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblToneError = new Infragistics.Win.Misc.UltraLabel();
            this.cmbToneWarning = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblToneWarning = new Infragistics.Win.Misc.UltraLabel();
            this.btnOK = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbToneError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxBeepSetup)).BeginInit();
            this.gbxBeepSetup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbToneWarning)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOK);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 327);
            this.pnlDialogButton.Size = new System.Drawing.Size(584, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.gbxBeepSetup);
            this.pnlDialogMain.Size = new System.Drawing.Size(584, 327);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(584, 28);
            // 
            // cmbToneError
            // 
            this.cmbToneError.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbToneError.Location = new System.Drawing.Point(199, 104);
            this.cmbToneError.Name = "cmbToneError";
            this.cmbToneError.Size = new System.Drawing.Size(140, 27);
            this.cmbToneError.TabIndex = 63;
            // 
            // gbxBeepSetup
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            this.gbxBeepSetup.Appearance = appearance7;
            this.gbxBeepSetup.Controls.Add(this.lblVolume);
            this.gbxBeepSetup.Controls.Add(this.cmbVolume);
            this.gbxBeepSetup.Controls.Add(this.lblToneError);
            this.gbxBeepSetup.Controls.Add(this.cmbToneWarning);
            this.gbxBeepSetup.Controls.Add(this.lblToneWarning);
            this.gbxBeepSetup.Controls.Add(this.cmbToneError);
            this.gbxBeepSetup.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxBeepSetup.Location = new System.Drawing.Point(45, 65);
            this.gbxBeepSetup.Name = "gbxBeepSetup";
            this.gbxBeepSetup.Size = new System.Drawing.Size(493, 221);
            this.gbxBeepSetup.TabIndex = 87;
            this.gbxBeepSetup.Text = "The beep setup";
            // 
            // lblVolume
            // 
            appearance8.BackColor = System.Drawing.Color.Transparent;
            appearance8.FontData.Name = "default";
            this.lblVolume.Appearance = appearance8;
            this.lblVolume.Location = new System.Drawing.Point(42, 156);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(108, 23);
            this.lblVolume.TabIndex = 80;
            this.lblVolume.Text = "Volume :";
            // 
            // cmbVolume
            // 
            this.cmbVolume.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbVolume.Location = new System.Drawing.Point(199, 156);
            this.cmbVolume.Name = "cmbVolume";
            this.cmbVolume.Size = new System.Drawing.Size(140, 27);
            this.cmbVolume.TabIndex = 79;
            // 
            // lblToneError
            // 
            appearance9.BackColor = System.Drawing.Color.Transparent;
            appearance9.FontData.Name = "default";
            this.lblToneError.Appearance = appearance9;
            this.lblToneError.Location = new System.Drawing.Point(42, 104);
            this.lblToneError.Name = "lblToneError";
            this.lblToneError.Size = new System.Drawing.Size(139, 23);
            this.lblToneError.TabIndex = 78;
            this.lblToneError.Text = "Tone (Error):";
            // 
            // cmbToneWarning
            // 
            this.cmbToneWarning.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbToneWarning.Location = new System.Drawing.Point(199, 52);
            this.cmbToneWarning.Name = "cmbToneWarning";
            this.cmbToneWarning.Size = new System.Drawing.Size(140, 27);
            this.cmbToneWarning.TabIndex = 77;
            // 
            // lblToneWarning
            // 
            appearance10.BackColor = System.Drawing.Color.Transparent;
            appearance10.FontData.Name = "default";
            this.lblToneWarning.Appearance = appearance10;
            this.lblToneWarning.Location = new System.Drawing.Point(42, 52);
            this.lblToneWarning.Name = "lblToneWarning";
            this.lblToneWarning.Size = new System.Drawing.Size(139, 29);
            this.lblToneWarning.TabIndex = 76;
            this.lblToneWarning.Text = "Tone (Warning):";
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
            // DlgSysWarningSound
            // 
            this.ClientSize = new System.Drawing.Size(584, 384);
            this.Name = "DlgSysWarningSound";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbToneError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxBeepSetup)).EndInit();
            this.gbxBeepSetup.ResumeLayout(false);
            this.gbxBeepSetup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbToneWarning)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox gbxBeepSetup;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbToneError;
        private Infragistics.Win.Misc.UltraLabel lblToneWarning;
        private Infragistics.Win.Misc.UltraLabel lblVolume;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbVolume;
        private Infragistics.Win.Misc.UltraLabel lblToneError;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbToneWarning;
        private Controls.NoBorderButton btnOK;
        private Controls.NoBorderButton btnCancel;
    }
}
