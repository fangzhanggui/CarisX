namespace Oelco.CarisX.GUI
{
    partial class DlgSysEnableTBIGRA
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
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.gbxUsePrinter = new Infragistics.Win.Misc.UltraGroupBox();
            this.optUsePrinter = new Oelco.Common.GUI.CustomUOptionSet();
            this.gbxRealTimePrint = new Infragistics.Win.Misc.UltraGroupBox();
            this.optRealTimePrint = new Oelco.Common.GUI.CustomUOptionSet();
            this.btnOK = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxUsePrinter)).BeginInit();
            this.gbxUsePrinter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optUsePrinter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxRealTimePrint)).BeginInit();
            this.gbxRealTimePrint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optRealTimePrint)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOK);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 394);
            this.pnlDialogButton.Size = new System.Drawing.Size(584, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.gbxRealTimePrint);
            this.pnlDialogMain.ClientArea.Controls.Add(this.gbxUsePrinter);
            this.pnlDialogMain.Size = new System.Drawing.Size(584, 394);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(584, 28);
            // 
            // gbxUsePrinter
            // 
            appearance9.BackColor = System.Drawing.Color.Transparent;
            this.gbxUsePrinter.Appearance = appearance9;
            this.gbxUsePrinter.Controls.Add(this.optUsePrinter);
            this.gbxUsePrinter.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxUsePrinter.Location = new System.Drawing.Point(45, 60);
            this.gbxUsePrinter.Name = "gbxUsePrinter";
            this.gbxUsePrinter.Size = new System.Drawing.Size(270, 130);
            this.gbxUsePrinter.TabIndex = 79;
            this.gbxUsePrinter.Text = "The use of printer";
            // 
            // optUsePrinter
            // 
            appearance10.FontData.Name = "Georgia";
            appearance10.FontData.SizeInPoints = 12F;
            appearance10.TextHAlignAsString = "Center";
            appearance10.TextVAlignAsString = "Middle";
            this.optUsePrinter.Appearance = appearance10;
            this.optUsePrinter.BackColor = System.Drawing.Color.Transparent;
            this.optUsePrinter.BackColorInternal = System.Drawing.Color.Transparent;
            this.optUsePrinter.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            valueListItem3.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem3.DataValue = "デフォルト項目";
            valueListItem3.DisplayText = "Yes";
            valueListItem4.DataValue = "ValueListItem1";
            valueListItem4.DisplayText = "No";
            this.optUsePrinter.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem3,
            valueListItem4});
            this.optUsePrinter.ItemSpacingHorizontal = 10;
            this.optUsePrinter.ItemSpacingVertical = 20;
            this.optUsePrinter.Location = new System.Drawing.Point(33, 27);
            this.optUsePrinter.Name = "optUsePrinter";
            this.optUsePrinter.Size = new System.Drawing.Size(162, 89);
            this.optUsePrinter.TabIndex = 102;
            // 
            // gbxRealTimePrint
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            this.gbxRealTimePrint.Appearance = appearance7;
            this.gbxRealTimePrint.Controls.Add(this.optRealTimePrint);
            this.gbxRealTimePrint.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxRealTimePrint.Location = new System.Drawing.Point(45, 215);
            this.gbxRealTimePrint.Name = "gbxRealTimePrint";
            this.gbxRealTimePrint.Size = new System.Drawing.Size(270, 130);
            this.gbxRealTimePrint.TabIndex = 103;
            this.gbxRealTimePrint.Text = "Real-time print";
            // 
            // optRealTimePrint
            // 
            appearance8.FontData.Name = "Georgia";
            appearance8.FontData.SizeInPoints = 12F;
            appearance8.TextHAlignAsString = "Center";
            appearance8.TextVAlignAsString = "Middle";
            this.optRealTimePrint.Appearance = appearance8;
            this.optRealTimePrint.BackColor = System.Drawing.Color.Transparent;
            this.optRealTimePrint.BackColorInternal = System.Drawing.Color.Transparent;
            this.optRealTimePrint.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            valueListItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem1.DataValue = "デフォルト項目";
            valueListItem1.DisplayText = "Yes";
            valueListItem2.DataValue = "ValueListItem1";
            valueListItem2.DisplayText = "No";
            this.optRealTimePrint.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.optRealTimePrint.ItemSpacingHorizontal = 10;
            this.optRealTimePrint.ItemSpacingVertical = 20;
            this.optRealTimePrint.Location = new System.Drawing.Point(33, 27);
            this.optRealTimePrint.Name = "optRealTimePrint";
            this.optRealTimePrint.Size = new System.Drawing.Size(162, 89);
            this.optRealTimePrint.TabIndex = 102;
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
            // DlgSysEnableTBIGRA
            // 
            this.ClientSize = new System.Drawing.Size(584, 451);
            this.Name = "DlgSysEnableTBIGRA";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbxUsePrinter)).EndInit();
            this.gbxUsePrinter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optUsePrinter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxRealTimePrint)).EndInit();
            this.gbxRealTimePrint.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optRealTimePrint)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox gbxRealTimePrint;
        private Oelco.Common.GUI.CustomUOptionSet optRealTimePrint;
        private Infragistics.Win.Misc.UltraGroupBox gbxUsePrinter;
        private Oelco.Common.GUI.CustomUOptionSet optUsePrinter;
        private Controls.NoBorderButton btnOK;
        private Controls.NoBorderButton btnCancel;

    }
}
