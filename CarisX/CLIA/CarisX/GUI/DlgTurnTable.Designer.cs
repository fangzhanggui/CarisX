namespace Oelco.CarisX.GUI
{
    partial class DlgTurnTable
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
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("バンド 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PortNo");
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Analytes");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LotNo");
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ExpirationDate");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StabilityDate");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Remain");
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Check");
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.grdReagentList = new Oelco.Common.GUI.CustomGrid();
            this.btnSelectAll = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnTurn = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnOK = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdReagentList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOK);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnSelectAll);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnTurn);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 816);
            this.pnlDialogButton.Size = new System.Drawing.Size(700, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.grdReagentList);
            this.pnlDialogMain.Size = new System.Drawing.Size(700, 816);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(700, 28);
            this.lblDialogTitle.Text = "Reagent List";
            // 
            // grdReagentList
            // 
            appearance13.BackColor = System.Drawing.Color.White;
            this.grdReagentList.DisplayLayout.Appearance = appearance13;
            this.grdReagentList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand1.ColHeaderLines = 2;
            appearance14.TextHAlignAsString = "Center";
            ultraGridColumn13.CellAppearance = appearance14;
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
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.PromptChar = ' ';
            ultraGridColumn2.RowLayoutColumnInfo.OriginX = 2;
            ultraGridColumn2.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn2.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(96, 0);
            ultraGridColumn2.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn2.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn2.Width = 159;
            appearance15.TextHAlignAsString = "Right";
            ultraGridColumn18.CellAppearance = appearance15;
            ultraGridColumn18.Header.Editor = null;
            ultraGridColumn18.Header.VisiblePosition = 2;
            ultraGridColumn18.PromptChar = ' ';
            ultraGridColumn18.RowLayoutColumnInfo.OriginX = 4;
            ultraGridColumn18.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn18.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(73, 0);
            ultraGridColumn18.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn18.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn18.Width = 123;
            ultraGridColumn15.Header.Editor = null;
            ultraGridColumn15.Header.VisiblePosition = 3;
            ultraGridColumn15.PromptChar = ' ';
            ultraGridColumn15.RowLayoutColumnInfo.OriginX = 6;
            ultraGridColumn15.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn15.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(102, 0);
            ultraGridColumn15.RowLayoutColumnInfo.SpanX = 1;
            ultraGridColumn15.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn15.Width = 119;
            ultraGridColumn16.Header.Editor = null;
            ultraGridColumn16.Header.VisiblePosition = 4;
            ultraGridColumn16.Hidden = true;
            ultraGridColumn16.PromptChar = ' ';
            ultraGridColumn16.RowLayoutColumnInfo.OriginX = 7;
            ultraGridColumn16.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn16.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(81, 0);
            ultraGridColumn16.RowLayoutColumnInfo.SpanX = 1;
            ultraGridColumn16.RowLayoutColumnInfo.SpanY = 2;
            appearance16.TextHAlignAsString = "Right";
            ultraGridColumn17.CellAppearance = appearance16;
            ultraGridColumn17.Header.Editor = null;
            ultraGridColumn17.Header.VisiblePosition = 5;
            ultraGridColumn17.PromptChar = ' ';
            ultraGridColumn17.RowLayoutColumnInfo.OriginX = 8;
            ultraGridColumn17.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn17.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(133, 0);
            ultraGridColumn17.RowLayoutColumnInfo.SpanX = 1;
            ultraGridColumn17.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn17.Width = 118;
            ultraGridColumn1.Header.Editor = null;
            ultraGridColumn1.Header.VisiblePosition = 6;
            ultraGridColumn1.Width = 63;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn13,
            ultraGridColumn2,
            ultraGridColumn18,
            ultraGridColumn15,
            ultraGridColumn16,
            ultraGridColumn17,
            ultraGridColumn1});
            ultraGridBand1.Header.Editor = null;
            ultraGridBand1.Header.FixOnRight = Infragistics.Win.DefaultableBoolean.True;
            this.grdReagentList.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdReagentList.DisplayLayout.EmptyRowSettings.Style = Infragistics.Win.UltraWinGrid.EmptyRowStyle.AlignWithDataRows;
            this.grdReagentList.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.grdReagentList.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.None;
            this.grdReagentList.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdReagentList.DisplayLayout.Override.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            appearance17.BackColor = System.Drawing.Color.Transparent;
            this.grdReagentList.DisplayLayout.Override.CardAreaAppearance = appearance17;
            appearance18.TextVAlignAsString = "Middle";
            this.grdReagentList.DisplayLayout.Override.CellAppearance = appearance18;
            this.grdReagentList.DisplayLayout.Override.DefaultRowHeight = 36;
            appearance19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(162)))), ((int)(((byte)(173)))));
            appearance19.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(132)))), ((int)(((byte)(142)))));
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance19.FontData.BoldAsString = "True";
            appearance19.FontData.Name = "Arial";
            appearance19.FontData.SizeInPoints = 9.88F;
            appearance19.ForeColor = System.Drawing.Color.White;
            appearance19.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdReagentList.DisplayLayout.Override.HeaderAppearance = appearance19;
            appearance20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(162)))), ((int)(((byte)(173)))));
            appearance20.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(132)))), ((int)(((byte)(142)))));
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grdReagentList.DisplayLayout.Override.RowSelectorAppearance = appearance20;
            this.grdReagentList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdReagentList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.AutoFixed;
            appearance21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            appearance21.ForeColor = System.Drawing.Color.Black;
            this.grdReagentList.DisplayLayout.Override.SelectedRowAppearance = appearance21;
            this.grdReagentList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdReagentList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdReagentList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdReagentList.DisplayLayout.Scrollbars = Infragistics.Win.UltraWinGrid.Scrollbars.None;
            this.grdReagentList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdReagentList.Font = new System.Drawing.Font("Arial", 16.2F);
            this.grdReagentList.Location = new System.Drawing.Point(25, 51);
            this.grdReagentList.Name = "grdReagentList";
            this.grdReagentList.Size = new System.Drawing.Size(651, 742);
            this.grdReagentList.TabIndex = 68;
            this.grdReagentList.ZoomStep = 10;
            this.grdReagentList.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdReagentList_InitializeLayout);
            this.grdReagentList.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdReagentList_ClickCellButton);
            this.grdReagentList.CellDataError += new Infragistics.Win.UltraWinGrid.CellDataErrorEventHandler(this.grdReagentList_CellDataError);
            this.grdReagentList.ClickCell += new Infragistics.Win.UltraWinGrid.ClickCellEventHandler(this.grdReagentList_ClickCell);
            // 
            // btnSelectAll
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BorderColor = System.Drawing.Color.Transparent;
            appearance4.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance4.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnSelectAll.Appearance = appearance4;
            this.btnSelectAll.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnSelectAll.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance5.BorderColor = System.Drawing.Color.Transparent;
            this.btnSelectAll.HotTrackAppearance = appearance5;
            this.btnSelectAll.ImageSize = new System.Drawing.Size(0, 0);
            this.btnSelectAll.Location = new System.Drawing.Point(185, 9);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Padding = new System.Drawing.Size(10, 0);
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            this.btnSelectAll.PressedAppearance = appearance6;
            this.btnSelectAll.ShowFocusRect = false;
            this.btnSelectAll.ShowOutline = false;
            this.btnSelectAll.Size = new System.Drawing.Size(152, 39);
            this.btnSelectAll.TabIndex = 70;
            this.btnSelectAll.Text = "Select all/cancel";
            this.btnSelectAll.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSelectAll.Visible = false;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnTurn
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            appearance7.BorderColor = System.Drawing.Color.Transparent;
            appearance7.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance7.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnTurn.Appearance = appearance7;
            this.btnTurn.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnTurn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance8.BorderColor = System.Drawing.Color.Transparent;
            this.btnTurn.HotTrackAppearance = appearance8;
            this.btnTurn.ImageSize = new System.Drawing.Size(0, 0);
            this.btnTurn.Location = new System.Drawing.Point(25, 9);
            this.btnTurn.Name = "btnTurn";
            this.btnTurn.Padding = new System.Drawing.Size(10, 0);
            appearance9.BorderColor = System.Drawing.Color.Transparent;
            this.btnTurn.PressedAppearance = appearance9;
            this.btnTurn.ShowFocusRect = false;
            this.btnTurn.ShowOutline = false;
            this.btnTurn.Size = new System.Drawing.Size(152, 39);
            this.btnTurn.TabIndex = 67;
            this.btnTurn.Text = "Turn";
            this.btnTurn.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnTurn.Visible = false;
            this.btnTurn.Click += new System.EventHandler(this.btnTurn_Click);
            // 
            // btnCancel
            // 
            appearance10.BackColor = System.Drawing.Color.Transparent;
            appearance10.BorderColor = System.Drawing.Color.Transparent;
            appearance10.Image = global::Oelco.CarisX.Properties.Resources.Image_Exit;
            appearance10.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnCancel.Appearance = appearance10;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance11.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.HotTrackAppearance = appearance11;
            this.btnCancel.ImageSize = new System.Drawing.Size(0, 0);
            this.btnCancel.Location = new System.Drawing.Point(357, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Drawing.Size(10, 0);
            appearance12.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.PressedAppearance = appearance12;
            this.btnCancel.ShowFocusRect = false;
            this.btnCancel.ShowOutline = false;
            this.btnCancel.Size = new System.Drawing.Size(152, 39);
            this.btnCancel.TabIndex = 69;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
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
            this.btnOK.Location = new System.Drawing.Point(524, 9);
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
            this.btnOK.Visible = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // DlgTurnTable
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Caption = "Reagent List";
            this.ClientSize = new System.Drawing.Size(700, 873);
            this.Name = "DlgTurnTable";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DlgTurnTable_FormClosing);
            this.Load += new System.EventHandler(this.DlgTurnTable_Load);
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdReagentList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.NoBorderButton btnTurn;
        private Controls.NoBorderButton btnCancel;
        private Controls.NoBorderButton btnSelectAll;
        private Oelco.Common.GUI.CustomGrid grdReagentList;
        private Controls.NoBorderButton btnOK;
    }
}