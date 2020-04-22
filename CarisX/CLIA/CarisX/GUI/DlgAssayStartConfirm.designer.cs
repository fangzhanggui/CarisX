namespace Oelco.CarisX.GUI
{
    partial class DlgAssayStartConfirm
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            this.btnOK = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.chkRinseExecution = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.lblMessage = new Infragistics.Win.Misc.UltraLabel();
            this.lblErrorMessage = new Infragistics.Win.Misc.UltraLabel();
            this.lblErrorMessageRackModule = new Infragistics.Win.Misc.UltraLabel();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkRinseExecution)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOK);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 267);
            this.pnlDialogButton.Size = new System.Drawing.Size(630, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblErrorMessageRackModule);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblErrorMessage);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblMessage);
            this.pnlDialogMain.ClientArea.Controls.Add(this.chkRinseExecution);
            this.pnlDialogMain.Size = new System.Drawing.Size(630, 267);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(630, 28);
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
            this.btnOK.Location = new System.Drawing.Point(295, 9);
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
            this.btnCancel.Location = new System.Drawing.Point(453, 9);
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
            // chkRinseExecution
            // 
            appearance8.BackColor = System.Drawing.Color.Transparent;
            appearance8.FontData.Name = "Georgia";
            appearance8.FontData.SizeInPoints = 12F;
            appearance8.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckOFF;
            this.chkRinseExecution.Appearance = appearance8;
            this.chkRinseExecution.BackColor = System.Drawing.Color.Transparent;
            this.chkRinseExecution.BackColorInternal = System.Drawing.Color.Transparent;
            appearance9.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckON;
            this.chkRinseExecution.CheckedAppearance = appearance9;
            this.chkRinseExecution.Location = new System.Drawing.Point(25, 222);
            this.chkRinseExecution.Name = "chkRinseExecution";
            this.chkRinseExecution.Size = new System.Drawing.Size(290, 28);
            this.chkRinseExecution.Style = Infragistics.Win.EditCheckStyle.Custom;
            this.chkRinseExecution.TabIndex = 104;
            this.chkRinseExecution.Text = "RinseExecution";
            this.chkRinseExecution.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(25, 44);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(580, 91);
            this.lblMessage.TabIndex = 105;
            this.lblMessage.Text = "メッセージ";
            // 
            // lblErrorMessage
            // 
            this.lblErrorMessage.Location = new System.Drawing.Point(25, 141);
            this.lblErrorMessage.Name = "lblErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(580, 31);
            this.lblErrorMessage.TabIndex = 106;
            this.lblErrorMessage.Text = "モーターエラーメッセージ";
            // 
            // lblErrorMessageRackModule
            // 
            appearance7.ForeColor = System.Drawing.Color.Red;
            this.lblErrorMessageRackModule.Appearance = appearance7;
            this.lblErrorMessageRackModule.Location = new System.Drawing.Point(25, 178);
            this.lblErrorMessageRackModule.Name = "lblErrorMessageRackModule";
            this.lblErrorMessageRackModule.Size = new System.Drawing.Size(580, 25);
            this.lblErrorMessageRackModule.TabIndex = 107;
            this.lblErrorMessageRackModule.Text = "モーターエラー発生のラック、モジュール";
            // 
            // DlgAssayStartConfirm
            // 
            this.ClientSize = new System.Drawing.Size(630, 324);
            this.Name = "DlgAssayStartConfirm";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkRinseExecution)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.NoBorderButton btnOK;
        private Controls.NoBorderButton btnCancel;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkRinseExecution;
        private Infragistics.Win.Misc.UltraLabel lblMessage;
        private Infragistics.Win.Misc.UltraLabel lblErrorMessageRackModule;
        private Infragistics.Win.Misc.UltraLabel lblErrorMessage;
    }
}
