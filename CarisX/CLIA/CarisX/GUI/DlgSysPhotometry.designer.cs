namespace Oelco.CarisX.GUI
{
    partial class DlgSysPhotometry
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
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            this.gbxPhotometryMode = new Infragistics.Win.Misc.UltraGroupBox();
            this.optPhotometryMode = new Oelco.Common.GUI.CustomUOptionSet();
            this.gbxAreaMethodTime = new Infragistics.Win.Misc.UltraGroupBox();
            this.numAreaMethodTime = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.btnOK = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.gbxPeekMethodTime = new Infragistics.Win.Misc.UltraGroupBox();
            this.optPeekMethodTime = new Oelco.Common.GUI.CustomUOptionSet();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxPhotometryMode)).BeginInit();
            this.gbxPhotometryMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optPhotometryMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxAreaMethodTime)).BeginInit();
            this.gbxAreaMethodTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAreaMethodTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxPeekMethodTime)).BeginInit();
            this.gbxPeekMethodTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optPeekMethodTime)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOK);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 336);
            this.pnlDialogButton.Size = new System.Drawing.Size(584, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.gbxPhotometryMode);
            this.pnlDialogMain.ClientArea.Controls.Add(this.gbxAreaMethodTime);
            this.pnlDialogMain.ClientArea.Controls.Add(this.gbxPeekMethodTime);
            this.pnlDialogMain.Size = new System.Drawing.Size(584, 336);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(584, 28);
            // 
            // gbxPhotometryMode
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            this.gbxPhotometryMode.Appearance = appearance7;
            this.gbxPhotometryMode.Controls.Add(this.optPhotometryMode);
            this.gbxPhotometryMode.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxPhotometryMode.Location = new System.Drawing.Point(45, 60);
            this.gbxPhotometryMode.Name = "gbxPhotometryMode";
            this.gbxPhotometryMode.Size = new System.Drawing.Size(423, 89);
            this.gbxPhotometryMode.TabIndex = 80;
            this.gbxPhotometryMode.Text = "Photometry mode";
            // 
            // optPhotometryMode
            // 
            appearance8.FontData.Name = "Georgia";
            appearance8.FontData.SizeInPoints = 12F;
            appearance8.TextHAlignAsString = "Center";
            appearance8.TextVAlignAsString = "Middle";
            this.optPhotometryMode.Appearance = appearance8;
            this.optPhotometryMode.BackColor = System.Drawing.Color.Transparent;
            this.optPhotometryMode.BackColorInternal = System.Drawing.Color.Transparent;
            this.optPhotometryMode.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            valueListItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem1.DataValue = "デフォルト項目";
            valueListItem1.DisplayText = "Area method";
            valueListItem2.DataValue = "ValueListItem1";
            valueListItem2.DisplayText = "Peak method";
            this.optPhotometryMode.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.optPhotometryMode.ItemSpacingHorizontal = 10;
            this.optPhotometryMode.ItemSpacingVertical = 20;
            this.optPhotometryMode.Location = new System.Drawing.Point(33, 27);
            this.optPhotometryMode.Name = "optPhotometryMode";
            this.optPhotometryMode.Size = new System.Drawing.Size(357, 48);
            this.optPhotometryMode.TabIndex = 102;
            this.optPhotometryMode.ValueChanged += new System.EventHandler(this.optPhotometryMode_ValueChanged);
            // 
            // gbxAreaMethodTime
            // 
            appearance9.BackColor = System.Drawing.Color.Transparent;
            this.gbxAreaMethodTime.Appearance = appearance9;
            this.gbxAreaMethodTime.Controls.Add(this.numAreaMethodTime);
            this.gbxAreaMethodTime.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxAreaMethodTime.Location = new System.Drawing.Point(45, 162);
            this.gbxAreaMethodTime.Name = "gbxAreaMethodTime";
            this.gbxAreaMethodTime.Size = new System.Drawing.Size(423, 67);
            this.gbxAreaMethodTime.TabIndex = 103;
            this.gbxAreaMethodTime.Text = "Area method photometry time(msec)";
            // 
            // numAreaMethodTime
            // 
            this.numAreaMethodTime.Location = new System.Drawing.Point(33, 26);
            this.numAreaMethodTime.MaskInput = "nnnn";
            this.numAreaMethodTime.MaxValue = 1000;
            this.numAreaMethodTime.MinValue = 100;
            this.numAreaMethodTime.Name = "numAreaMethodTime";
            this.numAreaMethodTime.PromptChar = ' ';
            this.numAreaMethodTime.Size = new System.Drawing.Size(50, 27);
            this.numAreaMethodTime.TabIndex = 94;
            this.numAreaMethodTime.Value = 100;
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
            this.btnOK.TabIndex = 3;
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
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gbxPeekMethodTime
            // 
            appearance10.BackColor = System.Drawing.Color.Transparent;
            this.gbxPeekMethodTime.Appearance = appearance10;
            this.gbxPeekMethodTime.Controls.Add(this.optPeekMethodTime);
            this.gbxPeekMethodTime.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxPeekMethodTime.Location = new System.Drawing.Point(45, 245);
            this.gbxPeekMethodTime.Name = "gbxPeekMethodTime";
            this.gbxPeekMethodTime.Size = new System.Drawing.Size(423, 73);
            this.gbxPeekMethodTime.TabIndex = 103;
            this.gbxPeekMethodTime.Text = "Peak method photometry time(msec)";
            // 
            // optPeekMethodTime
            // 
            appearance11.BackColor = System.Drawing.Color.Transparent;
            appearance11.BackColorDisabled = System.Drawing.Color.Transparent;
            appearance11.FontData.Name = "Georgia";
            appearance11.FontData.SizeInPoints = 12F;
            appearance11.TextHAlignAsString = "Center";
            appearance11.TextVAlignAsString = "Middle";
            this.optPeekMethodTime.Appearance = appearance11;
            this.optPeekMethodTime.BackColor = System.Drawing.Color.Transparent;
            this.optPeekMethodTime.BackColorInternal = System.Drawing.Color.Transparent;
            this.optPeekMethodTime.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            valueListItem3.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem3.DataValue = "デフォルト項目";
            valueListItem3.DisplayText = "1msec";
            valueListItem4.DataValue = "ValueListItem1";
            valueListItem4.DisplayText = "10msec";
            this.optPeekMethodTime.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem3,
            valueListItem4});
            this.optPeekMethodTime.ItemSpacingHorizontal = 10;
            this.optPeekMethodTime.ItemSpacingVertical = 20;
            this.optPeekMethodTime.Location = new System.Drawing.Point(34, 26);
            this.optPeekMethodTime.Name = "optPeekMethodTime";
            this.optPeekMethodTime.Size = new System.Drawing.Size(187, 41);
            this.optPeekMethodTime.TabIndex = 102;
            // 
            // DlgSysPhotometry
            // 
            this.ClientSize = new System.Drawing.Size(584, 393);
            this.Name = "DlgSysPhotometry";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbxPhotometryMode)).EndInit();
            this.gbxPhotometryMode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optPhotometryMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxAreaMethodTime)).EndInit();
            this.gbxAreaMethodTime.ResumeLayout(false);
            this.gbxAreaMethodTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAreaMethodTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxPeekMethodTime)).EndInit();
            this.gbxPeekMethodTime.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optPeekMethodTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox gbxPhotometryMode;
        private Oelco.Common.GUI.CustomUOptionSet optPhotometryMode;
        private Infragistics.Win.Misc.UltraGroupBox gbxAreaMethodTime;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numAreaMethodTime;
        private Controls.NoBorderButton btnOK;
        private Controls.NoBorderButton btnCancel;
        private Infragistics.Win.Misc.UltraGroupBox gbxPeekMethodTime;
        private Oelco.Common.GUI.CustomUOptionSet optPeekMethodTime;
    }
}
