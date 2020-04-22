namespace Oelco.CarisX.GUI
{
    partial class DlgSysFlashPrime
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
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.gbxUseOfTheFlash = new Infragistics.Win.Misc.UltraGroupBox();
            this.optUseOfTheFlash = new Oelco.Common.GUI.CustomUOptionSet();
            this.gbxTime = new Infragistics.Win.Misc.UltraGroupBox();
            this.numTime = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.btnOK = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxUseOfTheFlash)).BeginInit();
            this.gbxUseOfTheFlash.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optUseOfTheFlash)).BeginInit();
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
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 327);
            this.pnlDialogButton.Size = new System.Drawing.Size(584, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.gbxTime);
            this.pnlDialogMain.ClientArea.Controls.Add(this.gbxUseOfTheFlash);
            this.pnlDialogMain.Size = new System.Drawing.Size(584, 327);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(584, 28);
            // 
            // gbxUseOfTheFlash
            // 
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.gbxUseOfTheFlash.Appearance = appearance8;
            this.gbxUseOfTheFlash.Controls.Add(this.optUseOfTheFlash);
            this.gbxUseOfTheFlash.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxUseOfTheFlash.Location = new System.Drawing.Point(45, 60);
            this.gbxUseOfTheFlash.Name = "gbxUseOfTheFlash";
            this.gbxUseOfTheFlash.Size = new System.Drawing.Size(270, 130);
            this.gbxUseOfTheFlash.TabIndex = 80;
            this.gbxUseOfTheFlash.Text = "Use of the flash";
            // 
            // optUseOfTheFlash
            // 
            appearance9.FontData.Name = "Georgia";
            appearance9.FontData.SizeInPoints = 12F;
            appearance9.TextHAlignAsString = "Center";
            appearance9.TextVAlignAsString = "Middle";
            this.optUseOfTheFlash.Appearance = appearance9;
            this.optUseOfTheFlash.BackColor = System.Drawing.Color.Transparent;
            this.optUseOfTheFlash.BackColorInternal = System.Drawing.Color.Transparent;
            this.optUseOfTheFlash.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            valueListItem3.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem3.DataValue = "デフォルト項目";
            valueListItem3.DisplayText = "Yes";
            valueListItem4.DataValue = "ValueListItem1";
            valueListItem4.DisplayText = "No";
            this.optUseOfTheFlash.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem3,
            valueListItem4});
            this.optUseOfTheFlash.ItemSpacingHorizontal = 10;
            this.optUseOfTheFlash.ItemSpacingVertical = 20;
            this.optUseOfTheFlash.Location = new System.Drawing.Point(33, 27);
            this.optUseOfTheFlash.Name = "optUseOfTheFlash";
            this.optUseOfTheFlash.Size = new System.Drawing.Size(162, 89);
            this.optUseOfTheFlash.TabIndex = 102;
            // 
            // gbxTime
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            this.gbxTime.Appearance = appearance7;
            this.gbxTime.Controls.Add(this.numTime);
            this.gbxTime.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxTime.Location = new System.Drawing.Point(45, 208);
            this.gbxTime.Name = "gbxTime";
            this.gbxTime.Size = new System.Drawing.Size(141, 67);
            this.gbxTime.TabIndex = 103;
            this.gbxTime.Text = "Time(Minutes)";
            // 
            // numTime
            // 
            this.numTime.Location = new System.Drawing.Point(33, 26);
            this.numTime.MaskInput = "nnn";
            this.numTime.MaxValue = 360;
            this.numTime.MinValue = 0;
            this.numTime.Name = "numTime";
            this.numTime.PromptChar = ' ';
            this.numTime.Size = new System.Drawing.Size(50, 27);
            this.numTime.TabIndex = 94;
            this.numTime.Value = 120;
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
            // DlgSysFlashPrime
            // 
            this.ClientSize = new System.Drawing.Size(584, 384);
            this.Name = "DlgSysFlashPrime";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbxUseOfTheFlash)).EndInit();
            this.gbxUseOfTheFlash.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optUseOfTheFlash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxTime)).EndInit();
            this.gbxTime.ResumeLayout(false);
            this.gbxTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox gbxUseOfTheFlash;
        private Oelco.Common.GUI.CustomUOptionSet optUseOfTheFlash;
        private Infragistics.Win.Misc.UltraGroupBox gbxTime;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numTime;
        private Controls.NoBorderButton btnOK;
        private Controls.NoBorderButton btnCancel;
    }
}
