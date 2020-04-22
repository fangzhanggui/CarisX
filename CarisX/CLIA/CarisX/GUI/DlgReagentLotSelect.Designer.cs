namespace Oelco.CarisX.GUI
{
    partial class DlgReagentLotSelect
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.lvwReagentLotSelect = new Infragistics.Win.UltraWinListView.UltraListView();
            this.btnOK = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnClose = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lvwReagentLotSelect)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOK);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnClose);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 274);
            this.pnlDialogButton.Size = new System.Drawing.Size(361, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.lvwReagentLotSelect);
            this.pnlDialogMain.Size = new System.Drawing.Size(361, 274);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(361, 28);
            // 
            // lvwReagentLotSelect
            // 
            this.lvwReagentLotSelect.Font = new System.Drawing.Font("Arial", 14.25F);
            this.lvwReagentLotSelect.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            appearance7.BackColor = System.Drawing.SystemColors.Highlight;
            appearance7.ForeColor = System.Drawing.Color.White;
            this.lvwReagentLotSelect.ItemSettings.SelectedAppearance = appearance7;
            this.lvwReagentLotSelect.ItemSettings.SelectionType = Infragistics.Win.UltraWinListView.SelectionType.Single;
            this.lvwReagentLotSelect.Location = new System.Drawing.Point(64, 56);
            this.lvwReagentLotSelect.MainColumn.DataType = typeof(string);
            this.lvwReagentLotSelect.MainColumn.ShowSortIndicators = Infragistics.Win.DefaultableBoolean.True;
            this.lvwReagentLotSelect.Name = "lvwReagentLotSelect";
            this.lvwReagentLotSelect.ShowGroups = false;
            this.lvwReagentLotSelect.Size = new System.Drawing.Size(232, 196);
            this.lvwReagentLotSelect.TabIndex = 5;
            this.lvwReagentLotSelect.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List;
            this.lvwReagentLotSelect.ViewSettingsDetails.ColumnHeaderImageSize = new System.Drawing.Size(0, 0);
            this.lvwReagentLotSelect.ViewSettingsDetails.ColumnHeadersVisible = false;
            this.lvwReagentLotSelect.ViewSettingsDetails.ColumnsShowSortIndicators = false;
            this.lvwReagentLotSelect.ViewSettingsDetails.ImageSize = new System.Drawing.Size(0, 0);
            this.lvwReagentLotSelect.ViewSettingsList.ImageSize = new System.Drawing.Size(0, 0);
            this.lvwReagentLotSelect.ViewSettingsList.MultiColumn = false;
            this.lvwReagentLotSelect.ViewSettingsThumbnails.TextAreaAlignment = Infragistics.Win.UltraWinListView.TextAreaAlignment.Left;
            this.lvwReagentLotSelect.ViewSettingsTiles.TextAreaAlignment = Infragistics.Win.UltraWinListView.TextAreaAlignment.Left;
            this.lvwReagentLotSelect.ItemActivating += new Infragistics.Win.UltraWinListView.ItemActivatingEventHandler(this.lvwReagentLotSelect_ItemActivating);
            this.lvwReagentLotSelect.ItemActivated += new Infragistics.Win.UltraWinListView.ItemActivatedEventHandler(this.lvwReagentLotSelect_ItemActivated);
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
            this.btnOK.Location = new System.Drawing.Point(25, 9);
            this.btnOK.Name = "btnOK";
            this.btnOK.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnOK.PressedAppearance = appearance3;
            this.btnOK.ShowFocusRect = false;
            this.btnOK.ShowOutline = false;
            this.btnOK.Size = new System.Drawing.Size(152, 39);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BorderColor = System.Drawing.Color.Transparent;
            appearance4.Image = global::Oelco.CarisX.Properties.Resources.Image_Exit;
            appearance4.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnClose.Appearance = appearance4;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnClose.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance5.BorderColor = System.Drawing.Color.Transparent;
            this.btnClose.HotTrackAppearance = appearance5;
            this.btnClose.ImageSize = new System.Drawing.Size(0, 0);
            this.btnClose.Location = new System.Drawing.Point(183, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Padding = new System.Drawing.Size(10, 0);
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            this.btnClose.PressedAppearance = appearance6;
            this.btnClose.ShowFocusRect = false;
            this.btnClose.ShowOutline = false;
            this.btnClose.Size = new System.Drawing.Size(152, 39);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // DlgReagentLotSelect
            // 
            this.ClientSize = new System.Drawing.Size(361, 331);
            this.Name = "DlgReagentLotSelect";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lvwReagentLotSelect)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinListView.UltraListView lvwReagentLotSelect;
        private Controls.NoBorderButton btnOK;
        private Controls.NoBorderButton btnClose;
    }
}
