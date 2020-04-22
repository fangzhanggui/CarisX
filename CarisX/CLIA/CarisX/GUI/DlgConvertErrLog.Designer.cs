namespace Oelco.CarisX.GUI
{
    partial class DlgConvertErrLog
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
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
            this.btnOK = new Controls.NoBorderButton();
            this.listPartsName = new System.Windows.Forms.ListBox();
            this.lblColTitle1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblColTitle2 = new Infragistics.Win.Misc.UltraLabel();
            this.lblColTitle3 = new Infragistics.Win.Misc.UltraLabel();
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
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOK);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 452);
            this.pnlDialogButton.Size = new System.Drawing.Size(488, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblColTitle3);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblColTitle2);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblColTitle1);
            this.pnlDialogMain.ClientArea.Controls.Add(this.listPartsName);
            this.pnlDialogMain.Size = new System.Drawing.Size(488, 452);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(488, 28);
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
            this.btnOK.Location = new System.Drawing.Point(304, 9);
            this.btnOK.Name = "btnOK";
            this.btnOK.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnOK.PressedAppearance = appearance3;
            this.btnOK.ShowFocusRect = false;
            this.btnOK.ShowOutline = false;
            this.btnOK.Size = new System.Drawing.Size(152, 39);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // listPartsName
            // 
            this.listPartsName.FormattingEnabled = true;
            this.listPartsName.ItemHeight = 18;
            this.listPartsName.Location = new System.Drawing.Point(25, 111);
            this.listPartsName.Name = "listPartsName";
            this.listPartsName.Size = new System.Drawing.Size(431, 310);
            this.listPartsName.TabIndex = 1;
            // 
            // lblColTitle1
            // 
            this.lblColTitle1.Location = new System.Drawing.Point(25, 83);
            this.lblColTitle1.Name = "lblColTitle1";
            this.lblColTitle1.Size = new System.Drawing.Size(118, 24);
            this.lblColTitle1.TabIndex = 2;
            this.lblColTitle1.Text = "ultraLabel1";
            // 
            // lblColTitle2
            // 
            this.lblColTitle2.Location = new System.Drawing.Point(162, 83);
            this.lblColTitle2.Name = "lblColTitle2";
            this.lblColTitle2.Size = new System.Drawing.Size(118, 24);
            this.lblColTitle2.TabIndex = 3;
            this.lblColTitle2.Text = "ultraLabel1";
            // 
            // lblColTitle3
            // 
            this.lblColTitle3.Location = new System.Drawing.Point(304, 83);
            this.lblColTitle3.Name = "lblColTitle3";
            this.lblColTitle3.Size = new System.Drawing.Size(118, 24);
            this.lblColTitle3.TabIndex = 4;
            this.lblColTitle3.Text = "ultraLabel1";
            // 
            // DlgConvertErrLog
            // 
            this.ClientSize = new System.Drawing.Size(488, 509);
            this.Name = "DlgConvertErrLog";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.NoBorderButton btnOK;
        private System.Windows.Forms.ListBox listPartsName;
        private Infragistics.Win.Misc.UltraLabel lblColTitle1;
        private Infragistics.Win.Misc.UltraLabel lblColTitle3;
        private Infragistics.Win.Misc.UltraLabel lblColTitle2;
    }
}
