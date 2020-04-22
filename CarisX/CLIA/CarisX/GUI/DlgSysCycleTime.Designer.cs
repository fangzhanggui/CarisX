namespace Oelco.CarisX.GUI
{
    partial class DlgSysCycleTime
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
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.lblCycleTime = new Infragistics.Win.Misc.UltraLabel();
            this.numCycleTime = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.gbxCycleTime = new Infragistics.Win.Misc.UltraGroupBox();
            this.btnOK = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCycleTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxCycleTime)).BeginInit();
            this.gbxCycleTime.SuspendLayout();
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
            this.pnlDialogMain.ClientArea.Controls.Add(this.gbxCycleTime);
            this.pnlDialogMain.Size = new System.Drawing.Size(584, 177);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(584, 28);
            // 
            // lblCycleTime
            // 
            appearance8.BackColor = System.Drawing.Color.Transparent;
            appearance8.FontData.Name = "default";
            this.lblCycleTime.Appearance = appearance8;
            this.lblCycleTime.Location = new System.Drawing.Point(123, 34);
            this.lblCycleTime.Name = "lblCycleTime";
            this.lblCycleTime.Size = new System.Drawing.Size(70, 23);
            this.lblCycleTime.TabIndex = 75;
            this.lblCycleTime.Text = "seconds";
            // 
            // numCycleTime
            // 
            this.numCycleTime.Location = new System.Drawing.Point(46, 29);
            this.numCycleTime.MaskInput = "nn";
            this.numCycleTime.MaxValue = 60;
            this.numCycleTime.MinValue = 18;
            this.numCycleTime.Name = "numCycleTime";
            this.numCycleTime.PromptChar = ' ';
            this.numCycleTime.Size = new System.Drawing.Size(50, 27);
            this.numCycleTime.TabIndex = 84;
            this.numCycleTime.Value = 30;
            // 
            // gbxCycleTime
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            this.gbxCycleTime.Appearance = appearance7;
            this.gbxCycleTime.Controls.Add(this.numCycleTime);
            this.gbxCycleTime.Controls.Add(this.lblCycleTime);
            this.gbxCycleTime.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxCycleTime.Location = new System.Drawing.Point(45, 65);
            this.gbxCycleTime.Name = "gbxCycleTime";
            this.gbxCycleTime.Size = new System.Drawing.Size(250, 75);
            this.gbxCycleTime.TabIndex = 85;
            this.gbxCycleTime.Text = "Cycle time";
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
            // DlgSysCycleTime
            // 
            this.ClientSize = new System.Drawing.Size(584, 234);
            this.Name = "DlgSysCycleTime";
            this.Load += new System.EventHandler(this.DlgSysCycleTime_Load);
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numCycleTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxCycleTime)).EndInit();
            this.gbxCycleTime.ResumeLayout(false);
            this.gbxCycleTime.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblCycleTime;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numCycleTime;
        private Infragistics.Win.Misc.UltraGroupBox gbxCycleTime;
        private Controls.NoBorderButton btnOK;
        private Controls.NoBorderButton btnCancel;
    }
}
