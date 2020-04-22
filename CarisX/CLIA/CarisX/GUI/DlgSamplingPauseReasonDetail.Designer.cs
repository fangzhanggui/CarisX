namespace Oelco.CarisX.GUI
{
    partial class DlgSamplingPauseReasonDetail
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
            this.lvwSamplingPauseReasonDetail = new Infragistics.Win.UltraWinListView.UltraListView();
            this.btnConfirm = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lvwSamplingPauseReasonDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnConfirm);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 535);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.lvwSamplingPauseReasonDetail);
            this.pnlDialogMain.Size = new System.Drawing.Size(723, 535);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(723, 28);
            // 
            // lvwRemarkDetail
            // 
            this.lvwSamplingPauseReasonDetail.Font = new System.Drawing.Font("Arial", 14.25F);
            this.lvwSamplingPauseReasonDetail.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            this.lvwSamplingPauseReasonDetail.Location = new System.Drawing.Point(25, 64);
            this.lvwSamplingPauseReasonDetail.MainColumn.DataType = typeof(string);
            this.lvwSamplingPauseReasonDetail.MainColumn.ShowSortIndicators = Infragistics.Win.DefaultableBoolean.True;
            this.lvwSamplingPauseReasonDetail.Name = "lvwRemarkDetail";
            this.lvwSamplingPauseReasonDetail.Size = new System.Drawing.Size(673, 445);
            this.lvwSamplingPauseReasonDetail.TabIndex = 1;
            this.lvwSamplingPauseReasonDetail.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List;
            this.lvwSamplingPauseReasonDetail.ViewSettingsDetails.ColumnHeaderImageSize = new System.Drawing.Size(0, 0);
            this.lvwSamplingPauseReasonDetail.ViewSettingsDetails.ColumnHeadersVisible = false;
            this.lvwSamplingPauseReasonDetail.ViewSettingsDetails.ColumnsShowSortIndicators = false;
            this.lvwSamplingPauseReasonDetail.ViewSettingsDetails.ImageSize = new System.Drawing.Size(0, 0);
            this.lvwSamplingPauseReasonDetail.ViewSettingsList.ImageSize = new System.Drawing.Size(0, 0);
            this.lvwSamplingPauseReasonDetail.ViewSettingsList.MultiColumn = false;
            this.lvwSamplingPauseReasonDetail.ViewSettingsThumbnails.TextAreaAlignment = Infragistics.Win.UltraWinListView.TextAreaAlignment.Left;
            this.lvwSamplingPauseReasonDetail.ViewSettingsTiles.TextAreaAlignment = Infragistics.Win.UltraWinListView.TextAreaAlignment.Left;
            // 
            // btnConfirm
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.Image = global::Oelco.CarisX.Properties.Resources.Image_Exit;
            appearance1.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnConfirm.Appearance = appearance1;
            this.btnConfirm.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnConfirm.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance2.BorderColor = System.Drawing.Color.Transparent;
            this.btnConfirm.HotTrackAppearance = appearance2;
            this.btnConfirm.ImageSize = new System.Drawing.Size(0, 0);
            this.btnConfirm.Location = new System.Drawing.Point(546, 9);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnConfirm.PressedAppearance = appearance3;
            this.btnConfirm.ShowFocusRect = false;
            this.btnConfirm.ShowOutline = false;
            this.btnConfirm.Size = new System.Drawing.Size(152, 39);
            this.btnConfirm.TabIndex = 0;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // DlgSamplingStopReasonDetail
            // 
            this.ClientSize = new System.Drawing.Size(723, 592);
            this.Name = "DlgSamplingStopReasonDetail";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lvwSamplingPauseReasonDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinListView.UltraListView lvwSamplingPauseReasonDetail;
        private Controls.NoBorderButton btnConfirm;
    }
}
