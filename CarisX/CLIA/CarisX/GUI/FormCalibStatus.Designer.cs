namespace Oelco.CarisX.GUI
{
    partial class FormCalibStatus
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
            if (disposing && ( components != null ))
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("バンド 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ProtocolName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Remain");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LotNo");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurveStatus");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("ToolBar");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Print");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("x");
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Print");
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("x");
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            this.grdCalibStatus = new Oelco.Common.GUI.CustomGrid();
            this.gesturePanel = new Oelco.Common.GUI.GesturePanel();
            this.tlbCommandBar = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this.pnlCommandBar = new Infragistics.Win.Misc.UltraPanel();
            this._ClientArea_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.pnlSpecimenResultShadow = new Infragistics.Win.Misc.UltraPanel();
            ((System.ComponentModel.ISupportInitialize)(this.grdCalibStatus)).BeginInit();
            this.gesturePanel.ClientArea.SuspendLayout();
            this.gesturePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tlbCommandBar)).BeginInit();
            this.pnlCommandBar.ClientArea.SuspendLayout();
            this.pnlCommandBar.SuspendLayout();
            this.pnlSpecimenResultShadow.ClientArea.SuspendLayout();
            this.pnlSpecimenResultShadow.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdCalibStatus
            // 
            this.grdCalibStatus.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand1.ColHeaderLines = 2;
            ultraGridColumn13.Header.Editor = null;
            ultraGridColumn13.Header.VisiblePosition = 0;
            ultraGridColumn13.Width = 331;
            appearance1.TextHAlignAsString = "Right";
            ultraGridColumn3.CellAppearance = appearance1;
            ultraGridColumn3.Header.Editor = null;
            ultraGridColumn3.Header.VisiblePosition = 1;
            ultraGridColumn3.Width = 376;
            appearance2.TextHAlignAsString = "Right";
            ultraGridColumn2.CellAppearance = appearance2;
            ultraGridColumn2.Header.Editor = null;
            ultraGridColumn2.Header.VisiblePosition = 2;
            ultraGridColumn2.Width = 241;
            ultraGridColumn1.Header.Editor = null;
            ultraGridColumn1.Header.VisiblePosition = 3;
            ultraGridColumn1.Width = 369;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn13,
            ultraGridColumn3,
            ultraGridColumn2,
            ultraGridColumn1});
            ultraGridBand1.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdCalibStatus.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdCalibStatus.DisplayLayout.MaxRowScrollRegions = 1;
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.grdCalibStatus.DisplayLayout.Override.ActiveRowAppearance = appearance3;
            this.grdCalibStatus.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdCalibStatus.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdCalibStatus.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            appearance4.BackColor = System.Drawing.Color.Transparent;
            this.grdCalibStatus.DisplayLayout.Override.CardAreaAppearance = appearance4;
            appearance5.TextVAlignAsString = "Middle";
            this.grdCalibStatus.DisplayLayout.Override.CellAppearance = appearance5;
            this.grdCalibStatus.DisplayLayout.Override.DefaultRowHeight = 52;
            appearance6.BackColor = System.Drawing.Color.LightSteelBlue;
            this.grdCalibStatus.DisplayLayout.Override.FilteredInRowAppearance = appearance6;
            this.grdCalibStatus.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow;
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(162)))), ((int)(((byte)(173)))));
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(132)))), ((int)(((byte)(142)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance7.FontData.BoldAsString = "True";
            appearance7.FontData.Name = "Arial";
            appearance7.FontData.SizeInPoints = 9.88F;
            appearance7.ForeColor = System.Drawing.Color.White;
            appearance7.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdCalibStatus.DisplayLayout.Override.HeaderAppearance = appearance7;
            this.grdCalibStatus.DisplayLayout.Override.RowFilterAction = Infragistics.Win.UltraWinGrid.RowFilterAction.AppearancesOnly;
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(162)))), ((int)(((byte)(173)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(132)))), ((int)(((byte)(142)))));
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grdCalibStatus.DisplayLayout.Override.RowSelectorAppearance = appearance8;
            this.grdCalibStatus.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.None;
            this.grdCalibStatus.DisplayLayout.Override.RowSelectorWidth = 80;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            appearance9.ForeColor = System.Drawing.Color.Black;
            this.grdCalibStatus.DisplayLayout.Override.SelectedRowAppearance = appearance9;
            this.grdCalibStatus.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdCalibStatus.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdCalibStatus.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            scrollBarLook1.ViewStyle = Infragistics.Win.UltraWinScrollBar.ScrollBarViewStyle.Office2013;
            this.grdCalibStatus.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdCalibStatus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdCalibStatus.Location = new System.Drawing.Point(0, 0);
            this.grdCalibStatus.Name = "grdCalibStatus";
            this.grdCalibStatus.Size = new System.Drawing.Size(1416, 834);
            this.grdCalibStatus.TabIndex = 5;
            this.grdCalibStatus.ZoomStep = 10;
            // 
            // gesturePanel
            // 
            // 
            // gesturePanel.ClientArea
            // 
            this.gesturePanel.ClientArea.Controls.Add(this.grdCalibStatus);
            this.gesturePanel.EnableGestureScroll = true;
            this.gesturePanel.FlickThreshold = ((short)(1500));
            this.gesturePanel.Location = new System.Drawing.Point(13, 13);
            this.gesturePanel.Name = "gesturePanel";
            this.gesturePanel.ScrollProxy = null;
            this.gesturePanel.Size = new System.Drawing.Size(1415, 834);
            this.gesturePanel.TabIndex = 6;
            this.gesturePanel.TouchClientLoc = new System.Drawing.Point(0, 0);
            this.gesturePanel.TouchClientSize = new System.Drawing.Size(0, 0);
            this.gesturePanel.UseFlick = false;
            this.gesturePanel.UseProxy = true;
            this.gesturePanel.UseTouchClientSize = false;
            // 
            // tlbCommandBar
            // 
            appearance10.BackColor = System.Drawing.Color.Transparent;
            appearance10.FontData.Name = "Arial";
            appearance10.FontData.SizeInPoints = 12.71F;
            this.tlbCommandBar.Appearance = appearance10;
            this.tlbCommandBar.DesignerFlags = 1;
            this.tlbCommandBar.DockWithinContainer = this.pnlCommandBar.ClientArea;
            this.tlbCommandBar.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.Standard;
            this.tlbCommandBar.HorizontalToolbarGrabHandleWidth = 32;
            this.tlbCommandBar.ImageSizeLarge = new System.Drawing.Size(56, 63);
            this.tlbCommandBar.ImageSizeSmall = new System.Drawing.Size(32, 32);
            this.tlbCommandBar.LockToolbars = true;
            this.tlbCommandBar.ShowFullMenusDelay = 500;
            this.tlbCommandBar.ShowQuickCustomizeButton = false;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            buttonTool11.InstanceProps.IsFirstInGroup = true;
            buttonTool1.InstanceProps.IsFirstInGroup = true;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool11,
            buttonTool1});
            ultraToolbar1.Text = "ToolBar";
            this.tlbCommandBar.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            this.tlbCommandBar.ToolbarSettings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.Color.Transparent;
            appearance12.BorderColor = System.Drawing.Color.Transparent;
            this.tlbCommandBar.ToolbarSettings.HotTrackAppearance = appearance12;
            this.tlbCommandBar.ToolbarSettings.PaddingBottom = 11;
            this.tlbCommandBar.ToolbarSettings.PaddingLeft = 36;
            this.tlbCommandBar.ToolbarSettings.PaddingTop = 12;
            this.tlbCommandBar.ToolbarSettings.ToolSpacing = 25;
            appearance13.Image = global::Oelco.CarisX.Properties.Resources.Image_Print;
            buttonTool12.SharedPropsInternal.AppearancesLarge.Appearance = appearance13;
            buttonTool12.SharedPropsInternal.Caption = "Print";
            buttonTool12.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool2.SharedPropsInternal.Enabled = false;
            this.tlbCommandBar.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool12,
            buttonTool2});
            this.tlbCommandBar.UseLargeImagesOnToolbar = true;
            // 
            // pnlCommandBar
            // 
            appearance11.BackColor = System.Drawing.Color.Transparent;
            appearance11.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_MenuBarBackground;
            this.pnlCommandBar.Appearance = appearance11;
            // 
            // pnlCommandBar.ClientArea
            // 
            this.pnlCommandBar.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Left);
            this.pnlCommandBar.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Right);
            this.pnlCommandBar.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Bottom);
            this.pnlCommandBar.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Top);
            this.pnlCommandBar.Location = new System.Drawing.Point(4, 3);
            this.pnlCommandBar.Name = "pnlCommandBar";
            this.pnlCommandBar.Size = new System.Drawing.Size(1433, 98);
            this.pnlCommandBar.TabIndex = 7;
            // 
            // _ClientArea_Toolbars_Dock_Area_Left
            // 
            this._ClientArea_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.Transparent;
            this._ClientArea_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._ClientArea_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 97);
            this._ClientArea_Toolbars_Dock_Area_Left.Name = "_ClientArea_Toolbars_Dock_Area_Left";
            this._ClientArea_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 1);
            this._ClientArea_Toolbars_Dock_Area_Left.ToolbarsManager = this.tlbCommandBar;
            // 
            // _ClientArea_Toolbars_Dock_Area_Right
            // 
            this._ClientArea_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.Transparent;
            this._ClientArea_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._ClientArea_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1433, 97);
            this._ClientArea_Toolbars_Dock_Area_Right.Name = "_ClientArea_Toolbars_Dock_Area_Right";
            this._ClientArea_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 1);
            this._ClientArea_Toolbars_Dock_Area_Right.ToolbarsManager = this.tlbCommandBar;
            // 
            // _ClientArea_Toolbars_Dock_Area_Bottom
            // 
            this._ClientArea_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.Transparent;
            this._ClientArea_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._ClientArea_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 98);
            this._ClientArea_Toolbars_Dock_Area_Bottom.Name = "_ClientArea_Toolbars_Dock_Area_Bottom";
            this._ClientArea_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1433, 0);
            this._ClientArea_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.tlbCommandBar;
            // 
            // _ClientArea_Toolbars_Dock_Area_Top
            // 
            this._ClientArea_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.Transparent;
            this._ClientArea_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._ClientArea_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ClientArea_Toolbars_Dock_Area_Top.Name = "_ClientArea_Toolbars_Dock_Area_Top";
            this._ClientArea_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1433, 97);
            this._ClientArea_Toolbars_Dock_Area_Top.ToolbarsManager = this.tlbCommandBar;
            // 
            // pnlSpecimenResultShadow
            // 
            appearance14.BackColor = System.Drawing.Color.Transparent;
            appearance14.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_BackgroundSpecimenResult;
            this.pnlSpecimenResultShadow.Appearance = appearance14;
            // 
            // pnlSpecimenResultShadow.ClientArea
            // 
            this.pnlSpecimenResultShadow.ClientArea.Controls.Add(this.gesturePanel);
            this.pnlSpecimenResultShadow.Location = new System.Drawing.Point(0, 139);
            this.pnlSpecimenResultShadow.Name = "pnlSpecimenResultShadow";
            this.pnlSpecimenResultShadow.Size = new System.Drawing.Size(1441, 860);
            this.pnlSpecimenResultShadow.TabIndex = 21;
            // 
            // FormCalibStatus
            // 
            this.ClientSize = new System.Drawing.Size(1439, 1005);
            this.Controls.Add(this.pnlCommandBar);
            this.Controls.Add(this.pnlSpecimenResultShadow);
            this.DoubleBuffered = true;
            this.Name = "FormCalibStatus";
            this.Text = "FormCalibStatus";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormCalibStatus_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.grdCalibStatus)).EndInit();
            this.gesturePanel.ClientArea.ResumeLayout(false);
            this.gesturePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tlbCommandBar)).EndInit();
            this.pnlCommandBar.ClientArea.ResumeLayout(false);
            this.pnlCommandBar.ResumeLayout(false);
            this.pnlSpecimenResultShadow.ClientArea.ResumeLayout(false);
            this.pnlSpecimenResultShadow.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Oelco.Common.GUI.CustomGrid grdCalibStatus;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager tlbCommandBar;
        private Oelco.Common.GUI.GesturePanel gesturePanel;
        private Infragistics.Win.Misc.UltraPanel pnlCommandBar;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Top;
        private Infragistics.Win.Misc.UltraPanel pnlSpecimenResultShadow;
    }
}