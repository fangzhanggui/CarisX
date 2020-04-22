namespace Oelco.CarisX.GUI
{
    partial class DlgImportMeasProto
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
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.btnOpen = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.txtFolder = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.btnImport = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.lblparamSheet = new Infragistics.Win.Misc.UltraLabel();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFolder)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnImport);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 162);
            this.pnlDialogButton.Size = new System.Drawing.Size(562, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblparamSheet);
            this.pnlDialogMain.ClientArea.Controls.Add(this.txtFolder);
            this.pnlDialogMain.ClientArea.Controls.Add(this.btnOpen);
            this.pnlDialogMain.Size = new System.Drawing.Size(562, 162);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(562, 28);
            this.lblDialogTitle.Text = "分析項目パラメータ読込み";
            // 
            // btnOpen
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            appearance7.BorderColor = System.Drawing.Color.Transparent;
            appearance7.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnOpen.Appearance = appearance7;
            this.btnOpen.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            appearance8.BorderColor = System.Drawing.Color.Transparent;
            this.btnOpen.HotTrackAppearance = appearance8;
            this.btnOpen.Location = new System.Drawing.Point(430, 83);
            this.btnOpen.Name = "btnOpen";
            appearance9.BorderColor = System.Drawing.Color.Transparent;
            this.btnOpen.PressedAppearance = appearance9;
            this.btnOpen.ShowFocusRect = false;
            this.btnOpen.ShowOutline = false;
            this.btnOpen.Size = new System.Drawing.Size(112, 39);
            this.btnOpen.TabIndex = 94;
            this.btnOpen.Text = "開く";
            this.btnOpen.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // txtFolder
            // 
            this.txtFolder.Location = new System.Drawing.Point(25, 88);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(399, 27);
            this.txtFolder.TabIndex = 95;
            // 
            // btnImport
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance1.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnImport.Appearance = appearance1;
            this.btnImport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnImport.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.BorderColor = System.Drawing.Color.Transparent;
            this.btnImport.HotTrackAppearance = appearance2;
            this.btnImport.ImageSize = new System.Drawing.Size(0, 0);
            this.btnImport.Location = new System.Drawing.Point(236, 9);
            this.btnImport.Name = "btnImport";
            this.btnImport.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnImport.PressedAppearance = appearance3;
            this.btnImport.ShowFocusRect = false;
            this.btnImport.ShowOutline = false;
            this.btnImport.Size = new System.Drawing.Size(152, 39);
            this.btnImport.TabIndex = 3;
            this.btnImport.Text = "Import";
            this.btnImport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
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
            appearance5.BackColor = System.Drawing.Color.Transparent;
            appearance5.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.HotTrackAppearance = appearance5;
            this.btnCancel.ImageSize = new System.Drawing.Size(0, 0);
            this.btnCancel.Location = new System.Drawing.Point(394, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Drawing.Size(10, 0);
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.PressedAppearance = appearance6;
            this.btnCancel.ShowFocusRect = false;
            this.btnCancel.ShowOutline = false;
            this.btnCancel.Size = new System.Drawing.Size(152, 39);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblparamSheet
            // 
            this.lblparamSheet.Location = new System.Drawing.Point(25, 61);
            this.lblparamSheet.Name = "lblparamSheet";
            this.lblparamSheet.Size = new System.Drawing.Size(427, 20);
            this.lblparamSheet.TabIndex = 96;
            this.lblparamSheet.Text = "分析項目パラメータシート";
            // 
            // DlgImportMeasProto
            // 
            this.ClientSize = new System.Drawing.Size(562, 219);
            this.Name = "DlgImportMeasProto";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.PerformLayout();
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFolder)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.NoBorderButton btnOpen;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtFolder;
        private Controls.NoBorderButton btnImport;
        private Controls.NoBorderButton btnCancel;
        private Infragistics.Win.Misc.UltraLabel lblparamSheet;
    }
}
