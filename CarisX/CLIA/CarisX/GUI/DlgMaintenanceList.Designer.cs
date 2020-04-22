namespace Oelco.CarisX.GUI
{
    partial class DlgMaintenanceList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( System.Boolean disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("バンド 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("No");
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CheckItem");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Module1");
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Module2");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Module3");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Module4");
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("MaintenanceJournalNo", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Kind", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UnitNo", 2);
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.grdMaintenanceJournalList = new Oelco.Common.GUI.CustomGrid();
            this.btnOK = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMaintenanceJournalList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOK);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 756);
            this.pnlDialogButton.Size = new System.Drawing.Size(1242, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.grdMaintenanceJournalList);
            this.pnlDialogMain.Size = new System.Drawing.Size(1242, 756);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(1242, 28);
            this.lblDialogTitle.Text = "Check List";
            // 
            // grdMaintenanceJournalList
            // 
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacterWithLineLimit;
            this.grdMaintenanceJournalList.DisplayLayout.Appearance = appearance4;
            ultraGridBand1.ColHeaderLines = 2;
            appearance5.TextHAlignAsString = "Center";
            ultraGridColumn13.CellAppearance = appearance5;
            ultraGridColumn13.Header.Editor = null;
            ultraGridColumn13.Header.VisiblePosition = 0;
            ultraGridColumn13.PromptChar = ' ';
            ultraGridColumn13.RowLayoutColumnInfo.OriginX = 0;
            ultraGridColumn13.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn13.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(84, 0);
            ultraGridColumn13.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn13.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn13.Width = 67;
            ultraGridColumn2.Header.Editor = null;
            ultraGridColumn2.Header.VisiblePosition = 2;
            ultraGridColumn2.PromptChar = ' ';
            ultraGridColumn2.RowLayoutColumnInfo.OriginX = 2;
            ultraGridColumn2.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn2.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(96, 0);
            ultraGridColumn2.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn2.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn2.Width = 708;
            appearance6.TextHAlignAsString = "Right";
            ultraGridColumn18.CellAppearance = appearance6;
            ultraGridColumn18.Header.Editor = null;
            ultraGridColumn18.Header.VisiblePosition = 3;
            ultraGridColumn18.PromptChar = ' ';
            ultraGridColumn18.RowLayoutColumnInfo.OriginX = 4;
            ultraGridColumn18.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn18.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(73, 0);
            ultraGridColumn18.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn18.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn18.Width = 82;
            ultraGridColumn15.Header.Editor = null;
            ultraGridColumn15.Header.VisiblePosition = 4;
            ultraGridColumn15.PromptChar = ' ';
            ultraGridColumn15.RowLayoutColumnInfo.OriginX = 6;
            ultraGridColumn15.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn15.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(102, 0);
            ultraGridColumn15.RowLayoutColumnInfo.SpanX = 1;
            ultraGridColumn15.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn15.Width = 69;
            ultraGridColumn16.Header.Editor = null;
            ultraGridColumn16.Header.VisiblePosition = 5;
            ultraGridColumn16.PromptChar = ' ';
            ultraGridColumn16.RowLayoutColumnInfo.OriginX = 7;
            ultraGridColumn16.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn16.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(81, 0);
            ultraGridColumn16.RowLayoutColumnInfo.SpanX = 1;
            ultraGridColumn16.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn16.Width = 81;
            ultraGridColumn17.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance7.TextHAlignAsString = "Right";
            ultraGridColumn17.CellAppearance = appearance7;
            ultraGridColumn17.Header.Editor = null;
            ultraGridColumn17.Header.VisiblePosition = 6;
            ultraGridColumn17.PromptChar = ' ';
            ultraGridColumn17.RowLayoutColumnInfo.OriginX = 8;
            ultraGridColumn17.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn17.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(133, 0);
            ultraGridColumn17.RowLayoutColumnInfo.SpanX = 1;
            ultraGridColumn17.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn17.Width = 72;
            ultraGridColumn1.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.OnCellActivate;
            ultraGridColumn1.Header.Editor = null;
            ultraGridColumn1.Header.VisiblePosition = 7;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.Width = 149;
            ultraGridColumn3.Header.Editor = null;
            ultraGridColumn3.Header.VisiblePosition = 1;
            ultraGridColumn3.Width = 95;
            ultraGridColumn4.Header.Editor = null;
            ultraGridColumn4.Header.VisiblePosition = 8;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn4.Width = 166;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn13,
            ultraGridColumn2,
            ultraGridColumn18,
            ultraGridColumn15,
            ultraGridColumn16,
            ultraGridColumn17,
            ultraGridColumn1,
            ultraGridColumn3,
            ultraGridColumn4});
            ultraGridBand1.Header.Editor = null;
            ultraGridBand1.Header.FixOnRight = Infragistics.Win.DefaultableBoolean.True;
            this.grdMaintenanceJournalList.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdMaintenanceJournalList.DisplayLayout.EmptyRowSettings.Style = Infragistics.Win.UltraWinGrid.EmptyRowStyle.AlignWithDataRows;
            this.grdMaintenanceJournalList.DisplayLayout.MaxColScrollRegions = 1;
            this.grdMaintenanceJournalList.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.grdMaintenanceJournalList.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdMaintenanceJournalList.DisplayLayout.Override.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.grdMaintenanceJournalList.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.TextHAlignAsString = "Center";
            appearance9.TextVAlignAsString = "Middle";
            this.grdMaintenanceJournalList.DisplayLayout.Override.CellAppearance = appearance9;
            this.grdMaintenanceJournalList.DisplayLayout.Override.DefaultRowHeight = 33;
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(162)))), ((int)(((byte)(173)))));
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(132)))), ((int)(((byte)(142)))));
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance10.FontData.BoldAsString = "True";
            appearance10.FontData.Name = "Arial";
            appearance10.FontData.SizeInPoints = 9.88F;
            appearance10.ForeColor = System.Drawing.Color.White;
            appearance10.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdMaintenanceJournalList.DisplayLayout.Override.HeaderAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(162)))), ((int)(((byte)(173)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(132)))), ((int)(((byte)(142)))));
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grdMaintenanceJournalList.DisplayLayout.Override.RowSelectorAppearance = appearance11;
            this.grdMaintenanceJournalList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdMaintenanceJournalList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.AutoFixed;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.grdMaintenanceJournalList.DisplayLayout.Override.SelectedRowAppearance = appearance12;
            this.grdMaintenanceJournalList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdMaintenanceJournalList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdMaintenanceJournalList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdMaintenanceJournalList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdMaintenanceJournalList.Font = new System.Drawing.Font("Arial", 16.2F);
            this.grdMaintenanceJournalList.Location = new System.Drawing.Point(25, 51);
            this.grdMaintenanceJournalList.Name = "grdMaintenanceJournalList";
            this.grdMaintenanceJournalList.Size = new System.Drawing.Size(1193, 682);
            this.grdMaintenanceJournalList.TabIndex = 68;
            this.grdMaintenanceJournalList.ZoomStep = 10;
            this.grdMaintenanceJournalList.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdMaintenanceList_ClickCellButton);
            this.grdMaintenanceJournalList.DoubleClickRow += new Infragistics.Win.UltraWinGrid.DoubleClickRowEventHandler(this.grdMaintenanceList_DoubleClickRow);
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
            this.btnOK.Location = new System.Drawing.Point(1066, 7);
            this.btnOK.Name = "btnOK";
            this.btnOK.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnOK.PressedAppearance = appearance3;
            this.btnOK.ShowFocusRect = false;
            this.btnOK.ShowOutline = false;
            this.btnOK.Size = new System.Drawing.Size(152, 39);
            this.btnOK.TabIndex = 71;
            this.btnOK.Text = "OK";
            this.btnOK.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // DlgMaintenanceList
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Caption = "Check List";
            this.ClientSize = new System.Drawing.Size(1242, 813);
            this.Name = "DlgMaintenanceList";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DlgMaintenanceList_FormClosing);
            this.Load += new System.EventHandler(this.DlgMaintenanceList_Load);
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMaintenanceJournalList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Oelco.Common.GUI.CustomGrid grdMaintenanceJournalList;
        private Controls.NoBorderButton btnOK;
    }
}