namespace Oelco.CarisX.GUI
{
    partial class DlgSysAssayModuleConnect
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
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.gbxAssayModuleConnect = new Infragistics.Win.Misc.UltraGroupBox();
            this.cmbAssayModuleConnect = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.btnOK = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxAssayModuleConnect)).BeginInit();
            this.gbxAssayModuleConnect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAssayModuleConnect)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOK);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 177);
            this.pnlDialogButton.Size = new System.Drawing.Size(584, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.gbxAssayModuleConnect);
            this.pnlDialogMain.Size = new System.Drawing.Size(584, 177);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(584, 28);
            // 
            // gbxAssayModuleConnect
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            this.gbxAssayModuleConnect.Appearance = appearance7;
            this.gbxAssayModuleConnect.Controls.Add(this.cmbAssayModuleConnect);
            this.gbxAssayModuleConnect.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxAssayModuleConnect.Location = new System.Drawing.Point(45, 65);
            this.gbxAssayModuleConnect.Name = "gbxAssayModuleConnect";
            this.gbxAssayModuleConnect.Size = new System.Drawing.Size(250, 75);
            this.gbxAssayModuleConnect.TabIndex = 86;
            this.gbxAssayModuleConnect.Text = "AssayModuleConnect";
            // 
            // cmbAssayModuleConnect
            // 
            this.cmbAssayModuleConnect.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbAssayModuleConnect.Location = new System.Drawing.Point(55, 29);
            this.cmbAssayModuleConnect.Name = "cmbAssayModuleConnect";
            this.cmbAssayModuleConnect.Size = new System.Drawing.Size(140, 27);
            this.cmbAssayModuleConnect.TabIndex = 63;
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
            // DlgSysAssayModuleConnect
            // 
            this.ClientSize = new System.Drawing.Size(584, 234);
            this.Name = "DlgSysAssayModuleConnect";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbxAssayModuleConnect)).EndInit();
            this.gbxAssayModuleConnect.ResumeLayout(false);
            this.gbxAssayModuleConnect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAssayModuleConnect)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox gbxAssayModuleConnect;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbAssayModuleConnect;
        private Controls.NoBorderButton btnOK;
        private Controls.NoBorderButton btnCancel;

    }
}
