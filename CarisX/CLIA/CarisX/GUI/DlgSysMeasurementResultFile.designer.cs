namespace Oelco.CarisX.GUI
{
    partial class DlgSysMeasurementResultFile
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
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.btnFolder = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.txtFolder = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.gbxUseCommonFile = new Infragistics.Win.Misc.UltraGroupBox();
            this.optUseCommonFile = new Oelco.Common.GUI.CustomUOptionSet();
            this.btnOK = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFolder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxUseCommonFile)).BeginInit();
            this.gbxUseCommonFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optUseCommonFile)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOK);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 274);
            this.pnlDialogButton.Size = new System.Drawing.Size(584, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.gbxUseCommonFile);
            this.pnlDialogMain.ClientArea.Controls.Add(this.btnFolder);
            this.pnlDialogMain.ClientArea.Controls.Add(this.txtFolder);
            this.pnlDialogMain.Size = new System.Drawing.Size(584, 274);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(584, 28);
            // 
            // btnFolder
            // 
            appearance9.BackColor = System.Drawing.Color.Transparent;
            appearance9.BorderColor = System.Drawing.Color.Transparent;
            this.btnFolder.Appearance = appearance9;
            appearance10.BorderColor = System.Drawing.Color.Transparent;
            this.btnFolder.HotTrackAppearance = appearance10;
            this.btnFolder.Location = new System.Drawing.Point(407, 205);
            this.btnFolder.Name = "btnFolder";
            appearance11.BorderColor = System.Drawing.Color.Transparent;
            this.btnFolder.PressedAppearance = appearance11;
            this.btnFolder.ShowFocusRect = false;
            this.btnFolder.ShowOutline = false;
            this.btnFolder.Size = new System.Drawing.Size(142, 39);
            this.btnFolder.TabIndex = 92;
            this.btnFolder.Text = "Select folder";
            this.btnFolder.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // txtFolder
            // 
            this.txtFolder.Location = new System.Drawing.Point(39, 210);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.ReadOnly = true;
            this.txtFolder.Size = new System.Drawing.Size(362, 27);
            this.txtFolder.TabIndex = 93;
            // 
            // gbxUseCommonFile
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            this.gbxUseCommonFile.Appearance = appearance7;
            this.gbxUseCommonFile.Controls.Add(this.optUseCommonFile);
            this.gbxUseCommonFile.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxUseCommonFile.Location = new System.Drawing.Point(45, 60);
            this.gbxUseCommonFile.Name = "gbxUseCommonFile";
            this.gbxUseCommonFile.Size = new System.Drawing.Size(218, 130);
            this.gbxUseCommonFile.TabIndex = 94;
            this.gbxUseCommonFile.Text = "Use of the common file";
            // 
            // optUseCommonFile
            // 
            appearance8.FontData.Name = "Georgia";
            appearance8.FontData.SizeInPoints = 12F;
            appearance8.TextHAlignAsString = "Center";
            appearance8.TextVAlignAsString = "Middle";
            this.optUseCommonFile.Appearance = appearance8;
            this.optUseCommonFile.BackColor = System.Drawing.Color.Transparent;
            this.optUseCommonFile.BackColorInternal = System.Drawing.Color.Transparent;
            this.optUseCommonFile.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            valueListItem3.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem3.DataValue = "デフォルト項目";
            valueListItem3.DisplayText = "Yes";
            valueListItem4.DataValue = "ValueListItem1";
            valueListItem4.DisplayText = "No";
            this.optUseCommonFile.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem3,
            valueListItem4});
            this.optUseCommonFile.ItemSpacingHorizontal = 10;
            this.optUseCommonFile.ItemSpacingVertical = 20;
            this.optUseCommonFile.Location = new System.Drawing.Point(33, 27);
            this.optUseCommonFile.Name = "optUseCommonFile";
            this.optUseCommonFile.Size = new System.Drawing.Size(162, 89);
            this.optUseCommonFile.TabIndex = 102;
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
            // DlgSysMeasurementResultFile
            // 
            this.ClientSize = new System.Drawing.Size(584, 331);
            this.Name = "DlgSysMeasurementResultFile";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.PerformLayout();
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFolder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxUseCommonFile)).EndInit();
            this.gbxUseCommonFile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optUseCommonFile)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtFolder;
        private Controls.NoBorderButton btnFolder;
        private Infragistics.Win.Misc.UltraGroupBox gbxUseCommonFile;
        private Oelco.Common.GUI.CustomUOptionSet optUseCommonFile;
        private Controls.NoBorderButton btnOK;
        private Controls.NoBorderButton btnCancel;
    }
}
