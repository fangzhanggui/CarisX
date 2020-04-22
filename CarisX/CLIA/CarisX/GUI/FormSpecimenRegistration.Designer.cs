namespace Oelco.CarisX.GUI
{
    partial class FormSpecimenRegistration
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(System.Boolean disposing)
        {
            if (disposing && (components != null))
            {
                // 拡大率変更イベント解除
                this.zoomPanel.SetZoom -= grdSpecimenRegistration.SetGridZoom;

                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn9 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("RackID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn10 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Rack Position");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn11 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Receipt No.");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn12 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Patient ID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn13 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Specimen type");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn14 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Registered");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn15 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Manual dilution ratio");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn16 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Coment");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSpecimenRegistration));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("バンド 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RackID");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Rack Position");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Receipt No.");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Patient ID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Specimen type", -1, 435146849);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Registered");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Manual dilution ratio");
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Coment");
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.ValueList valueList1 = new Infragistics.Win.ValueList(435146849);
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueList valueList2 = new Infragistics.Win.ValueList(435383253);
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar2 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("ToolBar");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Save");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete all");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Print");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Copy");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Paste");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Query");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("last");
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Save");
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete");
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete all");
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Print");
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Copy");
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Paste");
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Query");
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("last");
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            this.dscSpecimenRegistration = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.pnlDockBase = new Infragistics.Win.Misc.UltraPanel();
            this.gesturePanel = new Oelco.Common.GUI.GesturePanel();
            this.grdSpecimenRegistration = new Oelco.Common.GUI.CustomGrid();
            this.splAnalyteTable = new Infragistics.Win.Misc.UltraSplitter();
            this.analysisSettingPanel = new Oelco.CarisX.GUI.Controls.GroupSelectAnalysisSettingPanel();
            this.zoomPanel = new Oelco.CarisX.GUI.Controls.ZoomPanel();
            this.tlbCommandBar = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this.pnlCommandBar = new Infragistics.Win.Misc.UltraPanel();
            this._ClientArea_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.dscSpecimenRegistration)).BeginInit();
            this.pnlDockBase.ClientArea.SuspendLayout();
            this.pnlDockBase.SuspendLayout();
            this.gesturePanel.ClientArea.SuspendLayout();
            this.gesturePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSpecimenRegistration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tlbCommandBar)).BeginInit();
            this.pnlCommandBar.ClientArea.SuspendLayout();
            this.pnlCommandBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // dscSpecimenRegistration
            // 
            this.dscSpecimenRegistration.Band.Columns.AddRange(new object[] {
            ultraDataColumn9,
            ultraDataColumn10,
            ultraDataColumn11,
            ultraDataColumn12,
            ultraDataColumn13,
            ultraDataColumn14,
            ultraDataColumn15,
            ultraDataColumn16});
            this.dscSpecimenRegistration.Band.Key = "バンド 0";
            this.dscSpecimenRegistration.Rows.AddRange(new object[] {
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0001")),
                        ((object)("Rack Position")),
                        ((object)("1")),
                        ((object)("Receipt No.")),
                        ((object)("1")),
                        ((object)("Patient ID")),
                        ((object)(((object)(resources.GetObject("dscSpecimenRegistration.Rows"))))),
                        ((object)("Registered")),
                        ((object)("AFP")),
                        ((object)("Manual dilution ratio")),
                        ((object)("X1"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0001")),
                        ((object)("Rack Position")),
                        ((object)("2")),
                        ((object)("Receipt No.")),
                        ((object)("1")),
                        ((object)("Patient ID")),
                        ((object)(((object)(resources.GetObject("dscSpecimenRegistration.Rows1"))))),
                        ((object)("Registered")),
                        ((object)("AFP")),
                        ((object)("Manual dilution ratio")),
                        ((object)("X1"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0001")),
                        ((object)("Rack Position")),
                        ((object)("3")),
                        ((object)("Receipt No.")),
                        ((object)("1")),
                        ((object)("Patient ID")),
                        ((object)(((object)(resources.GetObject("dscSpecimenRegistration.Rows2"))))),
                        ((object)("Registered")),
                        ((object)("AFP")),
                        ((object)("Manual dilution ratio")),
                        ((object)("X1"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0001")),
                        ((object)("Rack Position")),
                        ((object)("4")),
                        ((object)("Receipt No.")),
                        ((object)("1")),
                        ((object)("Patient ID")),
                        ((object)(((object)(resources.GetObject("dscSpecimenRegistration.Rows3"))))),
                        ((object)("Registered")),
                        ((object)("CA125,CEA")),
                        ((object)("Manual dilution ratio")),
                        ((object)("X1"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0001")),
                        ((object)("Rack Position")),
                        ((object)("5")),
                        ((object)("Receipt No.")),
                        ((object)("1")),
                        ((object)("Patient ID")),
                        ((object)(((object)(resources.GetObject("dscSpecimenRegistration.Rows4"))))),
                        ((object)("Registered")),
                        ((object)("CA125,CEA")),
                        ((object)("Manual dilution ratio")),
                        ((object)("X1"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0002")),
                        ((object)("Rack Position")),
                        ((object)("1")),
                        ((object)("Receipt No.")),
                        ((object)("2")),
                        ((object)("Patient ID")),
                        ((object)(((object)(resources.GetObject("dscSpecimenRegistration.Rows5"))))),
                        ((object)("Registered")),
                        ((object)("A-Hbe")),
                        ((object)("Manual dilution ratio")),
                        ((object)("X1"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0002")),
                        ((object)("Rack Position")),
                        ((object)("2")),
                        ((object)("Receipt No.")),
                        ((object)("2")),
                        ((object)("Patient ID")),
                        ((object)(((object)(resources.GetObject("dscSpecimenRegistration.Rows6"))))),
                        ((object)("Registered")),
                        ((object)("CEA(x10)")),
                        ((object)("Manual dilution ratio")),
                        ((object)("X1"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0002")),
                        ((object)("Rack Position")),
                        ((object)("3")),
                        ((object)("Receipt No.")),
                        ((object)("2")),
                        ((object)("Patient ID")),
                        ((object)(((object)(resources.GetObject("dscSpecimenRegistration.Rows7"))))),
                        ((object)("Manual dilution ratio")),
                        ((object)("X1"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0002")),
                        ((object)("Rack Position")),
                        ((object)("4")),
                        ((object)("Receipt No.")),
                        ((object)("2")),
                        ((object)("Patient ID")),
                        ((object)(((object)(resources.GetObject("dscSpecimenRegistration.Rows8"))))),
                        ((object)("Manual dilution ratio")),
                        ((object)("X1"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0002")),
                        ((object)("Rack Position")),
                        ((object)("5")),
                        ((object)("Receipt No.")),
                        ((object)("2")),
                        ((object)("Patient ID")),
                        ((object)(((object)(resources.GetObject("dscSpecimenRegistration.Rows9"))))),
                        ((object)("Manual dilution ratio")),
                        ((object)("X1"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0003"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0003"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0003"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0003"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0003"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0004"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0004"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0004"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0004"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0004"))})});
            // 
            // pnlDockBase
            // 
            // 
            // pnlDockBase.ClientArea
            // 
            this.pnlDockBase.ClientArea.Controls.Add(this.gesturePanel);
            this.pnlDockBase.ClientArea.Controls.Add(this.splAnalyteTable);
            this.pnlDockBase.ClientArea.Controls.Add(this.analysisSettingPanel);
            this.pnlDockBase.Location = new System.Drawing.Point(13, 141);
            this.pnlDockBase.Name = "pnlDockBase";
            this.pnlDockBase.Size = new System.Drawing.Size(1415, 834);
            this.pnlDockBase.TabIndex = 43;
            // 
            // gesturePanel
            // 
            // 
            // gesturePanel.ClientArea
            // 
            this.gesturePanel.ClientArea.Controls.Add(this.grdSpecimenRegistration);
            this.gesturePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gesturePanel.EnableGestureScroll = true;
            this.gesturePanel.FlickThreshold = ((short)(1500));
            this.gesturePanel.Location = new System.Drawing.Point(0, 0);
            this.gesturePanel.Name = "gesturePanel";
            this.gesturePanel.ScrollProxy = null;
            this.gesturePanel.Size = new System.Drawing.Size(1415, 402);
            this.gesturePanel.TabIndex = 44;
            this.gesturePanel.TouchClientLoc = new System.Drawing.Point(0, 0);
            this.gesturePanel.TouchClientSize = new System.Drawing.Size(0, 0);
            this.gesturePanel.UseFlick = false;
            this.gesturePanel.UseProxy = true;
            this.gesturePanel.UseTouchClientSize = false;
            // 
            // grdSpecimenRegistration
            // 
            this.grdSpecimenRegistration.DataSource = this.dscSpecimenRegistration;
            ultraGridBand1.ColHeaderLines = 2;
            appearance1.TextHAlignAsString = "Left";
            ultraGridColumn1.CellAppearance = appearance1;
            ultraGridColumn1.Header.Caption = "RackID123";
            ultraGridColumn1.Header.Editor = null;
            ultraGridColumn1.Header.VisiblePosition = 1;
            ultraGridColumn1.MaskInput = "nnnn";
            ultraGridColumn1.MaxLength = 4;
            ultraGridColumn1.PromptChar = ' ';
            ultraGridColumn1.RowLayoutColumnInfo.OriginX = 0;
            ultraGridColumn1.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn1.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(104, 41);
            ultraGridColumn1.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn1.RowLayoutColumnInfo.SpanY = 2;
            appearance2.TextHAlignAsString = "Left";
            ultraGridColumn2.CellAppearance = appearance2;
            ultraGridColumn2.Header.Editor = null;
            ultraGridColumn2.Header.VisiblePosition = 0;
            ultraGridColumn2.MaskInput = "n";
            ultraGridColumn2.MaxLength = 1;
            ultraGridColumn2.MaxValue = "5";
            ultraGridColumn2.MinValue = "1";
            ultraGridColumn2.PromptChar = ' ';
            ultraGridColumn2.RowLayoutColumnInfo.OriginX = 2;
            ultraGridColumn2.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn2.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(106, 41);
            ultraGridColumn2.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn2.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            appearance3.TextHAlignAsString = "Left";
            ultraGridColumn3.CellAppearance = appearance3;
            ultraGridColumn3.Header.Editor = null;
            ultraGridColumn3.Header.VisiblePosition = 7;
            ultraGridColumn3.RowLayoutColumnInfo.OriginX = 4;
            ultraGridColumn3.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn3.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(106, 27);
            ultraGridColumn3.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn3.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn4.Header.Editor = null;
            ultraGridColumn4.Header.VisiblePosition = 2;
            ultraGridColumn4.MaxLength = 16;
            ultraGridColumn4.RowLayoutColumnInfo.OriginX = 6;
            ultraGridColumn4.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn4.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(0, 41);
            ultraGridColumn4.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn4.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn4.Width = 98;
            ultraGridColumn5.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn5.Header.Editor = null;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.NullText = "Serum/Plasma";
            ultraGridColumn5.RowLayoutColumnInfo.OriginX = 8;
            ultraGridColumn5.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn5.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(137, 41);
            ultraGridColumn5.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn5.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn5.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ultraGridColumn5.Width = 965;
            ultraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn6.Header.Editor = null;
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn6.RowLayoutColumnInfo.OriginX = 10;
            ultraGridColumn6.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn6.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(201, 41);
            ultraGridColumn6.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn6.RowLayoutColumnInfo.SpanY = 2;
            appearance4.TextHAlignAsString = "Left";
            ultraGridColumn7.CellAppearance = appearance4;
            ultraGridColumn7.Header.Editor = null;
            ultraGridColumn7.Header.VisiblePosition = 3;
            ultraGridColumn7.MaskInput = "nnnn";
            ultraGridColumn7.MaxValue = 1000;
            ultraGridColumn7.MinValue = 1;
            ultraGridColumn7.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Disallow;
            ultraGridColumn7.NullText = "1";
            ultraGridColumn7.PromptChar = ' ';
            ultraGridColumn7.RegexPattern = "^((?<value>[0-9]+))$";
            ultraGridColumn7.RowLayoutColumnInfo.OriginX = 12;
            ultraGridColumn7.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn7.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(144, 41);
            ultraGridColumn7.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn7.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn8.Header.Editor = null;
            ultraGridColumn8.Header.VisiblePosition = 6;
            ultraGridColumn8.MaxLength = 80;
            ultraGridColumn8.MinWidth = 80;
            ultraGridColumn8.RowLayoutColumnInfo.OriginX = 14;
            ultraGridColumn8.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn8.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(224, 41);
            ultraGridColumn8.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn8.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn8.Width = 253;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8});
            ultraGridBand1.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            ultraGridBand1.Override.RowSelectorWidth = 50;
            this.grdSpecimenRegistration.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdSpecimenRegistration.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.grdSpecimenRegistration.DisplayLayout.Override.ActiveRowAppearance = appearance5;
            this.grdSpecimenRegistration.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdSpecimenRegistration.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdSpecimenRegistration.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdSpecimenRegistration.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            appearance6.BackColor = System.Drawing.Color.Transparent;
            this.grdSpecimenRegistration.DisplayLayout.Override.CardAreaAppearance = appearance6;
            this.grdSpecimenRegistration.DisplayLayout.Override.DefaultRowHeight = 50;
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(162)))), ((int)(((byte)(173)))));
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(132)))), ((int)(((byte)(142)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance7.FontData.BoldAsString = "True";
            appearance7.FontData.Name = "Arial";
            appearance7.FontData.SizeInPoints = 10F;
            appearance7.ForeColor = System.Drawing.Color.White;
            appearance7.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdSpecimenRegistration.DisplayLayout.Override.HeaderAppearance = appearance7;
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(162)))), ((int)(((byte)(173)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(132)))), ((int)(((byte)(142)))));
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grdSpecimenRegistration.DisplayLayout.Override.RowSelectorAppearance = appearance8;
            this.grdSpecimenRegistration.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.None;
            this.grdSpecimenRegistration.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            appearance9.ForeColor = System.Drawing.Color.Black;
            this.grdSpecimenRegistration.DisplayLayout.Override.SelectedRowAppearance = appearance9;
            this.grdSpecimenRegistration.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            scrollBarLook1.ViewStyle = Infragistics.Win.UltraWinScrollBar.ScrollBarViewStyle.Office2013;
            this.grdSpecimenRegistration.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdSpecimenRegistration.DisplayLayout.Scrollbars = Infragistics.Win.UltraWinGrid.Scrollbars.Both;
            valueList1.DisplayStyle = Infragistics.Win.ValueListDisplayStyle.DisplayText;
            valueListItem1.DataValue = "ValueListItem0";
            valueListItem1.DisplayText = "Serum/Plasm";
            valueListItem2.DataValue = "ValueListItem1";
            valueList1.ValueListItems.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.grdSpecimenRegistration.DisplayLayout.ValueLists.AddRange(new Infragistics.Win.ValueList[] {
            valueList1,
            valueList2});
            this.grdSpecimenRegistration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSpecimenRegistration.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdSpecimenRegistration.Location = new System.Drawing.Point(0, 0);
            this.grdSpecimenRegistration.Name = "grdSpecimenRegistration";
            this.grdSpecimenRegistration.Size = new System.Drawing.Size(1415, 402);
            this.grdSpecimenRegistration.TabIndex = 43;
            this.grdSpecimenRegistration.ZoomStep = 10;
            this.grdSpecimenRegistration.AfterCellActivate += new System.EventHandler(this.grdSpecimenRegistration_AfterCellActivate);
            this.grdSpecimenRegistration.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSpecimenRegistration_InitializeLayout);
            this.grdSpecimenRegistration.AfterExitEditMode += new System.EventHandler(this.grdSpecimenRegistration_AfterExitEditMode);
            this.grdSpecimenRegistration.AfterRowActivate += new System.EventHandler(this.grdSpecimenRegistration_AfterRowActivate);
            this.grdSpecimenRegistration.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdSpecimenRegistration_CellChange);
            this.grdSpecimenRegistration.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdSpecimenRegistration_AfterCellListCloseUp);
            this.grdSpecimenRegistration.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdSpecimenRegistration_AfterSelectChange);
            this.grdSpecimenRegistration.BeforeSelectChange += new Infragistics.Win.UltraWinGrid.BeforeSelectChangeEventHandler(this.grdSpecimenRegistration_BeforeSelectChange);
            this.grdSpecimenRegistration.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.grdSpecimenRegistration_BeforeExitEditMode);
            this.grdSpecimenRegistration.CellDataError += new Infragistics.Win.UltraWinGrid.CellDataErrorEventHandler(this.grdSpecimenRegistration_CellDataError);
            this.grdSpecimenRegistration.ClickCell += new Infragistics.Win.UltraWinGrid.ClickCellEventHandler(this.grdSpecimenRegistration_ClickCell);
            // 
            // splAnalyteTable
            // 
            this.splAnalyteTable.BackColor = System.Drawing.Color.Transparent;
            this.splAnalyteTable.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splAnalyteTable.Location = new System.Drawing.Point(0, 402);
            this.splAnalyteTable.Name = "splAnalyteTable";
            this.splAnalyteTable.RestoreExtent = 337;
            this.splAnalyteTable.Size = new System.Drawing.Size(1415, 0);
            this.splAnalyteTable.TabIndex = 42;
            // 
            // analysisSettingPanel
            // 
            this.analysisSettingPanel.BackColor = System.Drawing.SystemColors.Control;
            this.analysisSettingPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.analysisSettingPanel.BtnCloseVisible = true;
            this.analysisSettingPanel.ChkIsEditRegistInfoVisible = true;
            this.analysisSettingPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.analysisSettingPanel.Location = new System.Drawing.Point(0, 402);
            this.analysisSettingPanel.Name = "analysisSettingPanel";
            this.analysisSettingPanel.Size = new System.Drawing.Size(1415, 432);
            this.analysisSettingPanel.TabIndex = 1;
            this.analysisSettingPanel.ProtocolCheckChangedByGroup += new System.Action<int, int, int, bool>(this.analysisSettingPanel_ProtocolCheckChanged);
            this.analysisSettingPanel.ProtocolCheckChanged += new System.Action<int, bool>(this.analysisSettingPanel_ProtocolCheckChanged);
            this.analysisSettingPanel.ProtocolCheckChanging += new System.Action<int, Oelco.CarisX.GUI.Controls.AnalysisSettingPanel.AnalisisSettingPanelSelectChangingData>(this.analysisSettingPanel_ProtocolCheckChanging);
            this.analysisSettingPanel.Closed += new System.Action(this.analysisSettingPanel1_Closed);
            // 
            // zoomPanel
            // 
            this.zoomPanel.BackColor = System.Drawing.Color.Transparent;
            this.zoomPanel.Location = new System.Drawing.Point(1198, 97);
            this.zoomPanel.Margin = new System.Windows.Forms.Padding(0);
            this.zoomPanel.Name = "zoomPanel";
            this.zoomPanel.Size = new System.Drawing.Size(230, 40);
            this.zoomPanel.TabIndex = 42;
            this.zoomPanel.Zoom = 100;
            // 
            // tlbCommandBar
            // 
            appearance20.BackColor = System.Drawing.Color.Transparent;
            appearance20.FontData.Name = "Arial";
            appearance20.FontData.SizeInPoints = 12.71F;
            this.tlbCommandBar.Appearance = appearance20;
            this.tlbCommandBar.DesignerFlags = 1;
            this.tlbCommandBar.DockWithinContainer = this.pnlCommandBar.ClientArea;
            this.tlbCommandBar.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.Standard;
            this.tlbCommandBar.ImageSizeLarge = new System.Drawing.Size(56, 63);
            this.tlbCommandBar.ImageSizeSmall = new System.Drawing.Size(32, 32);
            this.tlbCommandBar.LockToolbars = true;
            this.tlbCommandBar.ShowFullMenusDelay = 500;
            this.tlbCommandBar.ShowQuickCustomizeButton = false;
            ultraToolbar2.DockedColumn = 0;
            ultraToolbar2.DockedRow = 0;
            ultraToolbar2.FloatingLocation = new System.Drawing.Point(518, 446);
            ultraToolbar2.FloatingSize = new System.Drawing.Size(1065, 90);
            buttonTool2.InstanceProps.IsFirstInGroup = true;
            buttonTool7.InstanceProps.IsFirstInGroup = true;
            buttonTool11.InstanceProps.IsFirstInGroup = true;
            buttonTool5.InstanceProps.IsFirstInGroup = true;
            buttonTool6.InstanceProps.IsFirstInGroup = true;
            buttonTool9.InstanceProps.IsFirstInGroup = true;
            labelTool1.InstanceProps.IsFirstInGroup = true;
            ultraToolbar2.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool2,
            buttonTool7,
            buttonTool11,
            buttonTool5,
            buttonTool6,
            buttonTool9,
            labelTool1});
            ultraToolbar2.Text = "ToolBar";
            this.tlbCommandBar.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar2});
            this.tlbCommandBar.ToolbarSettings.FillEntireRow = Infragistics.Win.DefaultableBoolean.True;
            appearance11.BackColor = System.Drawing.Color.Transparent;
            appearance11.BorderColor = System.Drawing.Color.Transparent;
            this.tlbCommandBar.ToolbarSettings.HotTrackAppearance = appearance11;
            this.tlbCommandBar.ToolbarSettings.PaddingBottom = 11;
            this.tlbCommandBar.ToolbarSettings.PaddingLeft = 36;
            this.tlbCommandBar.ToolbarSettings.PaddingTop = 12;
            this.tlbCommandBar.ToolbarSettings.ToolSpacing = 25;
            appearance22.Image = global::Oelco.CarisX.Properties.Resources.Image_Regist;
            buttonTool3.SharedPropsInternal.AppearancesLarge.Appearance = appearance22;
            buttonTool3.SharedPropsInternal.Caption = "Save";
            buttonTool3.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance23.Image = global::Oelco.CarisX.Properties.Resources.Image_Erase;
            buttonTool4.SharedPropsInternal.AppearancesLarge.Appearance = appearance23;
            buttonTool4.SharedPropsInternal.Caption = "Delete";
            buttonTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance24.Image = global::Oelco.CarisX.Properties.Resources.Image_Delete;
            buttonTool8.SharedPropsInternal.AppearancesLarge.Appearance = appearance24;
            buttonTool8.SharedPropsInternal.Caption = "Delete all";
            buttonTool8.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance25.Image = global::Oelco.CarisX.Properties.Resources.Image_Print;
            buttonTool12.SharedPropsInternal.AppearancesLarge.Appearance = appearance25;
            buttonTool12.SharedPropsInternal.Caption = "Print";
            buttonTool12.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance26.Image = global::Oelco.CarisX.Properties.Resources.Image_Copy;
            buttonTool15.SharedPropsInternal.AppearancesLarge.Appearance = appearance26;
            buttonTool15.SharedPropsInternal.Caption = "Copy";
            buttonTool15.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance27.Image = global::Oelco.CarisX.Properties.Resources.Image_Paste;
            buttonTool16.SharedPropsInternal.AppearancesLarge.Appearance = appearance27;
            buttonTool16.SharedPropsInternal.Caption = "Paste";
            buttonTool16.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance28.Image = global::Oelco.CarisX.Properties.Resources.Image_Online;
            buttonTool10.SharedPropsInternal.AppearancesLarge.Appearance = appearance28;
            buttonTool10.SharedPropsInternal.Caption = "Query";
            buttonTool10.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            this.tlbCommandBar.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool3,
            buttonTool4,
            buttonTool8,
            buttonTool12,
            buttonTool15,
            buttonTool16,
            buttonTool10,
            labelTool2});
            this.tlbCommandBar.UseLargeImagesOnToolbar = true;
            this.tlbCommandBar.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.tlbCommandBar_ToolClick);
            // 
            // pnlCommandBar
            // 
            appearance21.BackColor = System.Drawing.Color.Transparent;
            appearance21.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_MenuBarBackground;
            appearance21.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.pnlCommandBar.Appearance = appearance21;
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
            this.pnlCommandBar.TabIndex = 56;
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
            // FormSpecimenRegistration
            // 
            this.ClientSize = new System.Drawing.Size(1439, 1005);
            this.Controls.Add(this.zoomPanel);
            this.Controls.Add(this.pnlCommandBar);
            this.Controls.Add(this.pnlDockBase);
            this.DoubleBuffered = true;
            this.Name = "FormSpecimenRegistration";
            this.Text = "FormSpecimenRegistration";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormSpecimenRegistration_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dscSpecimenRegistration)).EndInit();
            this.pnlDockBase.ClientArea.ResumeLayout(false);
            this.pnlDockBase.ResumeLayout(false);
            this.gesturePanel.ClientArea.ResumeLayout(false);
            this.gesturePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSpecimenRegistration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tlbCommandBar)).EndInit();
            this.pnlCommandBar.ClientArea.ResumeLayout(false);
            this.pnlCommandBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinDataSource.UltraDataSource dscSpecimenRegistration;
        private Infragistics.Win.Misc.UltraPanel pnlDockBase;
        private Oelco.Common.GUI.CustomGrid grdSpecimenRegistration;
        private Infragistics.Win.Misc.UltraSplitter splAnalyteTable;
        private Controls.GroupSelectAnalysisSettingPanel analysisSettingPanel;
        private Controls.ZoomPanel zoomPanel;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager tlbCommandBar;
        private Oelco.Common.GUI.GesturePanel gesturePanel;
        private Infragistics.Win.Misc.UltraPanel pnlCommandBar;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Top;
    }
}