namespace Oelco.CarisX.GUI
{
    partial class DlgSysAutoShutDown
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
            this.gbxUseAutomaticShutdown = new Infragistics.Win.Misc.UltraGroupBox();
            this.optUseAutomaticShutdown = new Oelco.Common.GUI.CustomUOptionSet();
            this.gbxTime = new Infragistics.Win.Misc.UltraGroupBox();
            this.numTime = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.btnOK = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxUseAutomaticShutdown)).BeginInit();
            this.gbxUseAutomaticShutdown.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optUseAutomaticShutdown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxTime)).BeginInit();
            this.gbxTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTime)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOK);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 306);
            this.pnlDialogButton.Size = new System.Drawing.Size(584, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.gbxUseAutomaticShutdown);
            this.pnlDialogMain.ClientArea.Controls.Add(this.gbxTime);
            this.pnlDialogMain.Size = new System.Drawing.Size(584, 306);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(584, 28);
            // 
            // gbxUseAutomaticShutdown
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            this.gbxUseAutomaticShutdown.Appearance = appearance7;
            this.gbxUseAutomaticShutdown.Controls.Add(this.optUseAutomaticShutdown);
            this.gbxUseAutomaticShutdown.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxUseAutomaticShutdown.Location = new System.Drawing.Point(45, 60);
            this.gbxUseAutomaticShutdown.Name = "gbxUseAutomaticShutdown";
            this.gbxUseAutomaticShutdown.Size = new System.Drawing.Size(270, 130);
            this.gbxUseAutomaticShutdown.TabIndex = 78;
            this.gbxUseAutomaticShutdown.Text = "Use of the automatic shutdown";
            // 
            // optUseAutomaticShutdown
            // 
            appearance8.FontData.Name = "Georgia";
            appearance8.FontData.SizeInPoints = 12F;
            appearance8.TextHAlignAsString = "Center";
            appearance8.TextVAlignAsString = "Middle";
            this.optUseAutomaticShutdown.Appearance = appearance8;
            this.optUseAutomaticShutdown.BackColor = System.Drawing.Color.Transparent;
            this.optUseAutomaticShutdown.BackColorInternal = System.Drawing.Color.Transparent;
            this.optUseAutomaticShutdown.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            valueListItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem1.DataValue = "デフォルト項目";
            valueListItem1.DisplayText = "Yes";
            valueListItem2.DataValue = "ValueListItem1";
            valueListItem2.DisplayText = "No";
            this.optUseAutomaticShutdown.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.optUseAutomaticShutdown.ItemSpacingHorizontal = 10;
            this.optUseAutomaticShutdown.ItemSpacingVertical = 20;
            this.optUseAutomaticShutdown.Location = new System.Drawing.Point(33, 27);
            this.optUseAutomaticShutdown.Name = "optUseAutomaticShutdown";
            this.optUseAutomaticShutdown.Size = new System.Drawing.Size(162, 89);
            this.optUseAutomaticShutdown.TabIndex = 102;
            // 
            // gbxTime
            // 
            appearance9.BackColor = System.Drawing.Color.Transparent;
            this.gbxTime.Appearance = appearance9;
            this.gbxTime.Controls.Add(this.numTime);
            this.gbxTime.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxTime.Location = new System.Drawing.Point(45, 208);
            this.gbxTime.Name = "gbxTime";
            this.gbxTime.Size = new System.Drawing.Size(141, 67);
            this.gbxTime.TabIndex = 81;
            this.gbxTime.Text = "Time(Minutes)";
            // 
            // numTime
            // 
            this.numTime.Location = new System.Drawing.Point(33, 26);
            this.numTime.MaskInput = "nnnn";
            this.numTime.MaxValue = 9999;
            this.numTime.MinValue = 10;
            this.numTime.Name = "numTime";
            this.numTime.PromptChar = ' ';
            this.numTime.Size = new System.Drawing.Size(50, 27);
            this.numTime.TabIndex = 94;
            this.numTime.Value = 10;
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
            // DlgSysAutoShutDown
            // 
            this.ClientSize = new System.Drawing.Size(584, 363);
            this.Name = "DlgSysAutoShutDown";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbxUseAutomaticShutdown)).EndInit();
            this.gbxUseAutomaticShutdown.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optUseAutomaticShutdown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxTime)).EndInit();
            this.gbxTime.ResumeLayout(false);
            this.gbxTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox gbxUseAutomaticShutdown;
        private Oelco.Common.GUI.CustomUOptionSet optUseAutomaticShutdown;
        private Infragistics.Win.Misc.UltraGroupBox gbxTime;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTime;
        private Controls.NoBorderButton btnOK;
        private Controls.NoBorderButton btnCancel;
    }
}
