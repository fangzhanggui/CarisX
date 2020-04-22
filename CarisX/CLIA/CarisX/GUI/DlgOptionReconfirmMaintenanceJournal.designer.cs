namespace Oelco.CarisX.GUI
{
    partial class DlgOptionReconfirmMaintenanceJournal
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
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.btnOK = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.lblComment = new Infragistics.Win.Misc.UltraLabel();
            this.lblDescription = new Infragistics.Win.Misc.UltraLabel();
            this.chkKind1 = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkKind2 = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkKind3 = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkKind1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkKind2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkKind3)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOK);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 293);
            this.pnlDialogButton.Size = new System.Drawing.Size(554, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.chkKind3);
            this.pnlDialogMain.ClientArea.Controls.Add(this.chkKind2);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblDescription);
            this.pnlDialogMain.ClientArea.Controls.Add(this.chkKind1);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblComment);
            this.pnlDialogMain.Size = new System.Drawing.Size(554, 293);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(554, 28);
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
            this.btnOK.Location = new System.Drawing.Point(219, 9);
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
            this.btnCancel.Location = new System.Drawing.Point(377, 9);
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
            // lblComment
            // 
            appearance14.BackColor = System.Drawing.Color.Transparent;
            appearance14.FontData.Name = "default";
            appearance14.ForeColor = System.Drawing.Color.Red;
            this.lblComment.Appearance = appearance14;
            this.lblComment.Location = new System.Drawing.Point(27, 225);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(505, 52);
            this.lblComment.TabIndex = 115;
            this.lblComment.Text = "All items in the maintenance log have been confirmed. \r\nTo reconfirm, select the " +
    "item to be reconfirmed.";
            // 
            // lblDescription
            // 
            appearance11.BackColor = System.Drawing.Color.Transparent;
            appearance11.FontData.Name = "default";
            appearance11.ForeColor = System.Drawing.Color.Black;
            this.lblDescription.Appearance = appearance11;
            this.lblDescription.Font = new System.Drawing.Font("Arial", 12F);
            this.lblDescription.Location = new System.Drawing.Point(27, 45);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(505, 33);
            this.lblDescription.TabIndex = 116;
            this.lblDescription.Text = "Please select the item you want to check again.";
            // 
            // chkKind1
            // 
            appearance12.BackColor = System.Drawing.Color.Transparent;
            appearance12.FontData.Name = "Georgia";
            appearance12.FontData.SizeInPoints = 12F;
            appearance12.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckOFF;
            this.chkKind1.Appearance = appearance12;
            this.chkKind1.BackColor = System.Drawing.Color.Transparent;
            this.chkKind1.BackColorInternal = System.Drawing.Color.Transparent;
            this.chkKind1.Checked = true;
            appearance13.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckON;
            this.chkKind1.CheckedAppearance = appearance13;
            this.chkKind1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkKind1.Location = new System.Drawing.Point(43, 84);
            this.chkKind1.Name = "chkKind1";
            this.chkKind1.Size = new System.Drawing.Size(453, 41);
            this.chkKind1.Style = Infragistics.Win.EditCheckStyle.Custom;
            this.chkKind1.TabIndex = 60;
            this.chkKind1.Text = "Daily";
            this.chkKind1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // chkKind2
            // 
            appearance9.BackColor = System.Drawing.Color.Transparent;
            appearance9.FontData.Name = "Georgia";
            appearance9.FontData.SizeInPoints = 12F;
            appearance9.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckOFF;
            this.chkKind2.Appearance = appearance9;
            this.chkKind2.BackColor = System.Drawing.Color.Transparent;
            this.chkKind2.BackColorInternal = System.Drawing.Color.Transparent;
            this.chkKind2.Checked = true;
            appearance10.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckON;
            this.chkKind2.CheckedAppearance = appearance10;
            this.chkKind2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkKind2.Location = new System.Drawing.Point(43, 131);
            this.chkKind2.Name = "chkKind2";
            this.chkKind2.Size = new System.Drawing.Size(453, 41);
            this.chkKind2.Style = Infragistics.Win.EditCheckStyle.Custom;
            this.chkKind2.TabIndex = 61;
            this.chkKind2.Text = "Weekly";
            this.chkKind2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // chkKind3
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            appearance7.FontData.Name = "Georgia";
            appearance7.FontData.SizeInPoints = 12F;
            appearance7.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckOFF;
            this.chkKind3.Appearance = appearance7;
            this.chkKind3.BackColor = System.Drawing.Color.Transparent;
            this.chkKind3.BackColorInternal = System.Drawing.Color.Transparent;
            appearance8.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckON;
            this.chkKind3.CheckedAppearance = appearance8;
            this.chkKind3.Location = new System.Drawing.Point(43, 178);
            this.chkKind3.Name = "chkKind3";
            this.chkKind3.Size = new System.Drawing.Size(453, 41);
            this.chkKind3.Style = Infragistics.Win.EditCheckStyle.Custom;
            this.chkKind3.TabIndex = 64;
            this.chkKind3.Text = "Monthly";
            this.chkKind3.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // DlgOptionMaintenanceJournalSelect
            // 
            this.ClientSize = new System.Drawing.Size(554, 350);
            this.Name = "DlgOptionMaintenanceJournalSelect";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkKind1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkKind2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkKind3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.NoBorderButton btnOK;
        private Controls.NoBorderButton btnCancel;
        private Infragistics.Win.Misc.UltraLabel lblComment;
        private Infragistics.Win.Misc.UltraLabel lblDescription;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkKind3;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkKind2;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkKind1;
    }
}
