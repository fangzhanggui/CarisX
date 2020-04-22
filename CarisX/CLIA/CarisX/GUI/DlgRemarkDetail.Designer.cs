namespace Oelco.CarisX.GUI
{
    partial class DlgRemarkDetail
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
            this.lvwRemarkDetail = new Infragistics.Win.UltraWinListView.UltraListView();
            this.btnConfirm = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lvwRemarkDetail)).BeginInit();
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
            this.pnlDialogMain.ClientArea.Controls.Add(this.lvwRemarkDetail);
            this.pnlDialogMain.Size = new System.Drawing.Size(723, 535);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(723, 28);
            // 
            // lvwRemarkDetail
            // 
            this.lvwRemarkDetail.Font = new System.Drawing.Font("Arial", 14.25F);
            this.lvwRemarkDetail.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            this.lvwRemarkDetail.Location = new System.Drawing.Point(25, 64);
            this.lvwRemarkDetail.MainColumn.DataType = typeof(string);
            this.lvwRemarkDetail.MainColumn.ShowSortIndicators = Infragistics.Win.DefaultableBoolean.True;
            this.lvwRemarkDetail.Name = "lvwRemarkDetail";
            this.lvwRemarkDetail.Size = new System.Drawing.Size(673, 445);
            this.lvwRemarkDetail.TabIndex = 1;
            this.lvwRemarkDetail.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List;
            this.lvwRemarkDetail.ViewSettingsDetails.ColumnHeaderImageSize = new System.Drawing.Size(0, 0);
            this.lvwRemarkDetail.ViewSettingsDetails.ColumnHeadersVisible = false;
            this.lvwRemarkDetail.ViewSettingsDetails.ColumnsShowSortIndicators = false;
            this.lvwRemarkDetail.ViewSettingsDetails.ImageSize = new System.Drawing.Size(0, 0);
            this.lvwRemarkDetail.ViewSettingsList.ImageSize = new System.Drawing.Size(0, 0);
            this.lvwRemarkDetail.ViewSettingsList.MultiColumn = false;
            this.lvwRemarkDetail.ViewSettingsThumbnails.TextAreaAlignment = Infragistics.Win.UltraWinListView.TextAreaAlignment.Left;
            this.lvwRemarkDetail.ViewSettingsTiles.TextAreaAlignment = Infragistics.Win.UltraWinListView.TextAreaAlignment.Left;
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
            // DlgRemarkDetail
            // 
            this.ClientSize = new System.Drawing.Size(723, 592);
            this.Name = "DlgRemarkDetail";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lvwRemarkDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinListView.UltraListView lvwRemarkDetail;
        private Controls.NoBorderButton btnConfirm;
    }
}
