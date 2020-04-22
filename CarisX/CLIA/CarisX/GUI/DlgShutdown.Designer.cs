namespace Oelco.CarisX.GUI
{
    partial class DlgShutdown
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( System.Boolean disposing )
        {
            if (disposing && ( components != null ))
            {
                components.Dispose();
            }
            base.Dispose( disposing );
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
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnLogOut = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnAutomaticStartupWait = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnShutdown = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnAllShutdown = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.lblMessage = new Infragistics.Win.Misc.UltraLabel();
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
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 205);
            this.pnlDialogButton.Size = new System.Drawing.Size(910, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblMessage);
            this.pnlDialogMain.ClientArea.Controls.Add(this.btnLogOut);
            this.pnlDialogMain.ClientArea.Controls.Add(this.btnAllShutdown);
            this.pnlDialogMain.ClientArea.Controls.Add(this.btnShutdown);
            this.pnlDialogMain.ClientArea.Controls.Add(this.btnAutomaticStartupWait);
            this.pnlDialogMain.Size = new System.Drawing.Size(910, 205);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(910, 36);
            this.lblDialogTitle.Text = "End processing";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.Image = global::Oelco.CarisX.Properties.Resources.Image_Exit;
            appearance1.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnCancel.Appearance = appearance1;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance2.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.HotTrackAppearance = appearance2;
            this.btnCancel.ImageSize = new System.Drawing.Size(0, 0);
            this.btnCancel.Location = new System.Drawing.Point(733, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.PressedAppearance = appearance3;
            this.btnCancel.ShowFocusRect = false;
            this.btnCancel.ShowOutline = false;
            this.btnCancel.Size = new System.Drawing.Size(152, 39);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnLogOut
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BorderColor = System.Drawing.Color.Transparent;
            this.btnLogOut.Appearance = appearance4;
            appearance5.BorderColor = System.Drawing.Color.Transparent;
            this.btnLogOut.HotTrackAppearance = appearance5;
            this.btnLogOut.Location = new System.Drawing.Point(45, 102);
            this.btnLogOut.Name = "btnLogOut";
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            this.btnLogOut.PressedAppearance = appearance6;
            this.btnLogOut.ShowFocusRect = false;
            this.btnLogOut.ShowOutline = false;
            this.btnLogOut.Size = new System.Drawing.Size(195, 81);
            this.btnLogOut.TabIndex = 1;
            this.btnLogOut.Text = "Log out";
            this.btnLogOut.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
            // 
            // btnAutomaticStartupWait
            // 
            appearance13.BackColor = System.Drawing.Color.Transparent;
            appearance13.BorderColor = System.Drawing.Color.Transparent;
            this.btnAutomaticStartupWait.Appearance = appearance13;
            appearance14.BorderColor = System.Drawing.Color.Transparent;
            this.btnAutomaticStartupWait.HotTrackAppearance = appearance14;
            this.btnAutomaticStartupWait.Location = new System.Drawing.Point(260, 102);
            this.btnAutomaticStartupWait.Name = "btnAutomaticStartupWait";
            appearance15.BorderColor = System.Drawing.Color.Transparent;
            this.btnAutomaticStartupWait.PressedAppearance = appearance15;
            this.btnAutomaticStartupWait.ShowFocusRect = false;
            this.btnAutomaticStartupWait.ShowOutline = false;
            this.btnAutomaticStartupWait.Size = new System.Drawing.Size(195, 81);
            this.btnAutomaticStartupWait.TabIndex = 1;
            this.btnAutomaticStartupWait.Text = "Automatic start-up wait";
            this.btnAutomaticStartupWait.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.btnAutomaticStartupWait.Click += new System.EventHandler(this.btnAutomaticStartupWait_Click);
            // 
            // btnShutdown
            // 
            appearance10.BackColor = System.Drawing.Color.Transparent;
            appearance10.BorderColor = System.Drawing.Color.Transparent;
            this.btnShutdown.Appearance = appearance10;
            appearance11.BorderColor = System.Drawing.Color.Transparent;
            this.btnShutdown.HotTrackAppearance = appearance11;
            this.btnShutdown.Location = new System.Drawing.Point(475, 102);
            this.btnShutdown.Name = "btnShutdown";
            appearance12.BorderColor = System.Drawing.Color.Transparent;
            this.btnShutdown.PressedAppearance = appearance12;
            this.btnShutdown.ShowFocusRect = false;
            this.btnShutdown.ShowOutline = false;
            this.btnShutdown.Size = new System.Drawing.Size(195, 81);
            this.btnShutdown.TabIndex = 1;
            this.btnShutdown.Text = "Shutdown";
            this.btnShutdown.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.btnShutdown.Click += new System.EventHandler(this.btnShutdown_Click);
            // 
            // btnAllShutdown
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            appearance7.BorderColor = System.Drawing.Color.Transparent;
            this.btnAllShutdown.Appearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Transparent;
            this.btnAllShutdown.HotTrackAppearance = appearance8;
            this.btnAllShutdown.Location = new System.Drawing.Point(690, 102);
            this.btnAllShutdown.Name = "btnAllShutdown";
            appearance9.BorderColor = System.Drawing.Color.Transparent;
            this.btnAllShutdown.PressedAppearance = appearance9;
            this.btnAllShutdown.ShowFocusRect = false;
            this.btnAllShutdown.ShowOutline = false;
            this.btnAllShutdown.Size = new System.Drawing.Size(175, 81);
            this.btnAllShutdown.TabIndex = 1;
            this.btnAllShutdown.Text = "All shutdown";
            this.btnAllShutdown.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.btnAllShutdown.Click += new System.EventHandler(this.btnAllShutdown_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(45, 66);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(280, 20);
            this.lblMessage.TabIndex = 2;
            this.lblMessage.Text = "Please select an operation to perform";
            // 
            // DlgShutdown
            // 
            this.Caption = "End processing";
            this.ClientSize = new System.Drawing.Size(910, 262);
            this.Name = "DlgShutdown";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.PerformLayout();
            this.pnlDialogMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.NoBorderButton btnCancel;
        private Infragistics.Win.Misc.UltraLabel lblMessage;
        private Controls.NoBorderButton btnLogOut;
        private Controls.NoBorderButton btnAllShutdown;
        private Controls.NoBorderButton btnShutdown;
        private Controls.NoBorderButton btnAutomaticStartupWait;
    }
}
