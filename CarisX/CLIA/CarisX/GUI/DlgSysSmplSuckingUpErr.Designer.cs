namespace Oelco.CarisX.GUI
{
    partial class DlgSysSmplSuckingUpErr
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
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.gbxDescription = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblDescription = new Infragistics.Win.Misc.UltraLabel();
            this.gbxProcessAfterCloggingDetected = new Infragistics.Win.Misc.UltraGroupBox();
            this.optProcessAfterCloggingDetected = new Oelco.Common.GUI.CustomUOptionSet();
            this.gbxProcessAfterAampleShortageDetected = new Infragistics.Win.Misc.UltraGroupBox();
            this.optProcessAfterAampleShortageDetected = new Oelco.Common.GUI.CustomUOptionSet();
            this.btnOK = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxDescription)).BeginInit();
            this.gbxDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxProcessAfterCloggingDetected)).BeginInit();
            this.gbxProcessAfterCloggingDetected.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optProcessAfterCloggingDetected)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxProcessAfterAampleShortageDetected)).BeginInit();
            this.gbxProcessAfterAampleShortageDetected.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optProcessAfterAampleShortageDetected)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOK);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 506);
            this.pnlDialogButton.Size = new System.Drawing.Size(584, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.gbxProcessAfterAampleShortageDetected);
            this.pnlDialogMain.ClientArea.Controls.Add(this.gbxProcessAfterCloggingDetected);
            this.pnlDialogMain.ClientArea.Controls.Add(this.gbxDescription);
            this.pnlDialogMain.Size = new System.Drawing.Size(584, 506);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(584, 28);
            // 
            // gbxDescription
            // 
            appearance11.BackColor = System.Drawing.Color.Transparent;
            this.gbxDescription.Appearance = appearance11;
            this.gbxDescription.Controls.Add(this.lblDescription);
            this.gbxDescription.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxDescription.Location = new System.Drawing.Point(50, 342);
            this.gbxDescription.Name = "gbxDescription";
            this.gbxDescription.Size = new System.Drawing.Size(407, 130);
            this.gbxDescription.TabIndex = 104;
            this.gbxDescription.Text = "Description";
            // 
            // lblDescription
            // 
            appearance12.BackColor = System.Drawing.Color.Transparent;
            appearance12.FontData.Name = "default";
            appearance12.TextVAlignAsString = "Middle";
            this.lblDescription.Appearance = appearance12;
            this.lblDescription.Location = new System.Drawing.Point(21, 26);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(363, 98);
            this.lblDescription.TabIndex = 79;
            this.lblDescription.Text = "The parameter specifies whether the sample is returned to the sample tube or to b" +
    "e dispensed continuously after clogging or sample shortage.";
            // 
            // gbxProcessAfterCloggingDetected
            // 
            appearance9.BackColor = System.Drawing.Color.Transparent;
            this.gbxProcessAfterCloggingDetected.Appearance = appearance9;
            this.gbxProcessAfterCloggingDetected.Controls.Add(this.optProcessAfterCloggingDetected);
            this.gbxProcessAfterCloggingDetected.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxProcessAfterCloggingDetected.Location = new System.Drawing.Point(45, 60);
            this.gbxProcessAfterCloggingDetected.Name = "gbxProcessAfterCloggingDetected";
            this.gbxProcessAfterCloggingDetected.Size = new System.Drawing.Size(412, 130);
            this.gbxProcessAfterCloggingDetected.TabIndex = 109;
            this.gbxProcessAfterCloggingDetected.Text = "Process after clogging detected";
            // 
            // optProcessAfterCloggingDetected
            // 
            appearance10.FontData.Name = "Georgia";
            appearance10.FontData.SizeInPoints = 12F;
            appearance10.TextHAlignAsString = "Center";
            appearance10.TextVAlignAsString = "Middle";
            this.optProcessAfterCloggingDetected.Appearance = appearance10;
            this.optProcessAfterCloggingDetected.BackColor = System.Drawing.Color.Transparent;
            this.optProcessAfterCloggingDetected.BackColorInternal = System.Drawing.Color.Transparent;
            this.optProcessAfterCloggingDetected.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            valueListItem3.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem3.DataValue = "デフォルト項目";
            valueListItem3.DisplayText = "Return the sample.";
            valueListItem4.DataValue = "ValueListItem1";
            valueListItem4.DisplayText = "Continue to dispense.";
            this.optProcessAfterCloggingDetected.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem3,
            valueListItem4});
            this.optProcessAfterCloggingDetected.ItemSpacingHorizontal = 10;
            this.optProcessAfterCloggingDetected.ItemSpacingVertical = 20;
            this.optProcessAfterCloggingDetected.Location = new System.Drawing.Point(33, 27);
            this.optProcessAfterCloggingDetected.Name = "optProcessAfterCloggingDetected";
            this.optProcessAfterCloggingDetected.Size = new System.Drawing.Size(194, 89);
            this.optProcessAfterCloggingDetected.TabIndex = 102;
            // 
            // gbxProcessAfterAampleShortageDetected
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            this.gbxProcessAfterAampleShortageDetected.Appearance = appearance7;
            this.gbxProcessAfterAampleShortageDetected.Controls.Add(this.optProcessAfterAampleShortageDetected);
            this.gbxProcessAfterAampleShortageDetected.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxProcessAfterAampleShortageDetected.Location = new System.Drawing.Point(45, 196);
            this.gbxProcessAfterAampleShortageDetected.Name = "gbxProcessAfterAampleShortageDetected";
            this.gbxProcessAfterAampleShortageDetected.Size = new System.Drawing.Size(412, 130);
            this.gbxProcessAfterAampleShortageDetected.TabIndex = 110;
            this.gbxProcessAfterAampleShortageDetected.Text = "Process after sample shortage detected.";
            // 
            // optProcessAfterAampleShortageDetected
            // 
            appearance8.FontData.Name = "Georgia";
            appearance8.FontData.SizeInPoints = 12F;
            appearance8.TextHAlignAsString = "Center";
            appearance8.TextVAlignAsString = "Middle";
            this.optProcessAfterAampleShortageDetected.Appearance = appearance8;
            this.optProcessAfterAampleShortageDetected.BackColor = System.Drawing.Color.Transparent;
            this.optProcessAfterAampleShortageDetected.BackColorInternal = System.Drawing.Color.Transparent;
            this.optProcessAfterAampleShortageDetected.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            valueListItem5.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem5.DataValue = "デフォルト項目";
            valueListItem5.DisplayText = "Return the sample.";
            valueListItem6.DataValue = "ValueListItem1";
            valueListItem6.DisplayText = "Continue to dispense.";
            this.optProcessAfterAampleShortageDetected.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem5,
            valueListItem6});
            this.optProcessAfterAampleShortageDetected.ItemSpacingHorizontal = 10;
            this.optProcessAfterAampleShortageDetected.ItemSpacingVertical = 20;
            this.optProcessAfterAampleShortageDetected.Location = new System.Drawing.Point(33, 27);
            this.optProcessAfterAampleShortageDetected.Name = "optProcessAfterAampleShortageDetected";
            this.optProcessAfterAampleShortageDetected.Size = new System.Drawing.Size(194, 89);
            this.optProcessAfterAampleShortageDetected.TabIndex = 102;
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
            // DlgSysSmplSuckingUpErr
            // 
            this.ClientSize = new System.Drawing.Size(584, 563);
            this.Name = "DlgSysSmplSuckingUpErr";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbxDescription)).EndInit();
            this.gbxDescription.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbxProcessAfterCloggingDetected)).EndInit();
            this.gbxProcessAfterCloggingDetected.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optProcessAfterCloggingDetected)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxProcessAfterAampleShortageDetected)).EndInit();
            this.gbxProcessAfterAampleShortageDetected.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optProcessAfterAampleShortageDetected)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox gbxDescription;
        private Infragistics.Win.Misc.UltraLabel lblDescription;
        private Infragistics.Win.Misc.UltraGroupBox gbxProcessAfterAampleShortageDetected;
        private Oelco.Common.GUI.CustomUOptionSet optProcessAfterAampleShortageDetected;
        private Infragistics.Win.Misc.UltraGroupBox gbxProcessAfterCloggingDetected;
        private Oelco.Common.GUI.CustomUOptionSet optProcessAfterCloggingDetected;
        private Controls.NoBorderButton btnOK;
        private Controls.NoBorderButton btnCancel;
    }
}
