namespace Oelco.CarisX.GUI
{
    partial class FormSpecimenStatRegistration
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
                this.zoomPanel.SetZoom -= grdSpecimenStatRegistration.SetGridZoom;

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
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("RackID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Rack Position");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Receipt No.");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn4 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Patient ID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn5 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Specimen type");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn6 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Registered");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn7 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Manual dilution ratio");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn8 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Coment");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn9 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Regist type");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn10 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Vessel type");
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("バンド 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RackID");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Rack Position");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Receipt No.");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Patient ID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Specimen type", -1, 435146849);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Registered");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Manual dilution ratio");
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Coment");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Regist type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Vessel type", -1, 435383253);
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
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn11 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("RackID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn12 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Rack Position");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn13 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Receipt No.");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn14 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Patient ID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn15 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Specimen type");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn16 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Registered");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn17 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Manual dilution ratio");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn18 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Coment");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn19 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Regist type");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn20 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Vessel type");
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("バンド 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RackID");
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Rack Position");
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn19 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Receipt No.");
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn20 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Patient ID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn21 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Specimen type", -1, 435146849);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn22 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Registered");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn23 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Manual dilution ratio");
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn24 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Coment");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn25 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Regist type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn26 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Vessel type", -1, 435383253);
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook2 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.ValueList valueList3 = new Infragistics.Win.ValueList(435146849);
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueList valueList4 = new Infragistics.Win.ValueList(435383253);
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("ToolBar");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Save");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete all");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Print");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Query");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("last");
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Save");
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete");
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete all");
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Print");
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Copy");
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Paste");
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Query");
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("last");
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            this.dscSpecimenStatRegistration = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.pnlDockBase = new Infragistics.Win.Misc.UltraPanel();
            this.gesturePanel = new Oelco.Common.GUI.GesturePanel();
            this.gesturePanelFixed = new Oelco.Common.GUI.GesturePanel();
            this.grdSpecimenStatRegistrationFixed = new Oelco.Common.GUI.CustomGrid();
            this.dscSpecimenStatRegistrationFixed = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.lblFixedRegistration = new Infragistics.Win.Misc.UltraLabel();
            this.grdSpecimenStatRegistration = new Oelco.Common.GUI.CustomGrid();
            this.lblTemporaryRegistration = new Infragistics.Win.Misc.UltraLabel();
            this.splAnalyteTable = new Infragistics.Win.Misc.UltraSplitter();
            this.analysisSettingPanel = new Oelco.CarisX.GUI.Controls.GroupSelectAnalysisSettingPanel();
            this.zoomPanel = new Oelco.CarisX.GUI.Controls.ZoomPanel();
            this.tlbCommandBar = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this.pnlCommandBar = new Infragistics.Win.Misc.UltraPanel();
            this._ClientArea_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.dscSpecimenStatRegistration)).BeginInit();
            this.pnlDockBase.ClientArea.SuspendLayout();
            this.pnlDockBase.SuspendLayout();
            this.gesturePanel.ClientArea.SuspendLayout();
            this.gesturePanel.SuspendLayout();
            this.gesturePanelFixed.ClientArea.SuspendLayout();
            this.gesturePanelFixed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSpecimenStatRegistrationFixed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dscSpecimenStatRegistrationFixed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSpecimenStatRegistration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tlbCommandBar)).BeginInit();
            this.pnlCommandBar.ClientArea.SuspendLayout();
            this.pnlCommandBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // dscSpecimenStatRegistration
            // 
            this.dscSpecimenStatRegistration.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2,
            ultraDataColumn3,
            ultraDataColumn4,
            ultraDataColumn5,
            ultraDataColumn6,
            ultraDataColumn7,
            ultraDataColumn8,
            ultraDataColumn9,
            ultraDataColumn10});
            this.dscSpecimenStatRegistration.Band.Key = "バンド 0";
            this.dscSpecimenStatRegistration.Rows.AddRange(new object[] {
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0001")),
                        ((object)("Rack Position")),
                        ((object)("1")),
                        ((object)("Receipt No.")),
                        ((object)("1")),
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
                        ((object)("Manual dilution ratio")),
                        ((object)("X1"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0002")),
                        ((object)("Rack Position")),
                        ((object)("4")),
                        ((object)("Receipt No.")),
                        ((object)("2")),
                        ((object)("Manual dilution ratio")),
                        ((object)("X1"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0002")),
                        ((object)("Rack Position")),
                        ((object)("5")),
                        ((object)("Receipt No.")),
                        ((object)("2")),
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
            this.gesturePanel.ClientArea.Controls.Add(this.gesturePanelFixed);
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
            // gesturePanelFixed
            // 
            // 
            // gesturePanelFixed.ClientArea
            // 
            this.gesturePanelFixed.ClientArea.Controls.Add(this.grdSpecimenStatRegistrationFixed);
            this.gesturePanelFixed.ClientArea.Controls.Add(this.lblFixedRegistration);
            this.gesturePanelFixed.ClientArea.Controls.Add(this.grdSpecimenStatRegistration);
            this.gesturePanelFixed.ClientArea.Controls.Add(this.lblTemporaryRegistration);
            this.gesturePanelFixed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gesturePanelFixed.EnableGestureScroll = true;
            this.gesturePanelFixed.FlickThreshold = ((short)(1500));
            this.gesturePanelFixed.Location = new System.Drawing.Point(0, 0);
            this.gesturePanelFixed.Name = "gesturePanelFixed";
            this.gesturePanelFixed.ScrollProxy = null;
            this.gesturePanelFixed.Size = new System.Drawing.Size(1415, 402);
            this.gesturePanelFixed.TabIndex = 45;
            this.gesturePanelFixed.TouchClientLoc = new System.Drawing.Point(0, 0);
            this.gesturePanelFixed.TouchClientSize = new System.Drawing.Size(0, 0);
            this.gesturePanelFixed.UseFlick = false;
            this.gesturePanelFixed.UseProxy = true;
            this.gesturePanelFixed.UseTouchClientSize = false;
            // 
            // grdSpecimenStatRegistrationFixed
            // 
            this.grdSpecimenStatRegistrationFixed.DataSource = this.dscSpecimenStatRegistrationFixed;
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
            ultraGridColumn4.CellAppearance = appearance2;
            ultraGridColumn4.Header.Editor = null;
            ultraGridColumn4.Header.VisiblePosition = 0;
            ultraGridColumn4.MaskInput = "n";
            ultraGridColumn4.MaxLength = 1;
            ultraGridColumn4.MaxValue = "5";
            ultraGridColumn4.MinValue = "1";
            ultraGridColumn4.PromptChar = ' ';
            ultraGridColumn4.RowLayoutColumnInfo.OriginX = 2;
            ultraGridColumn4.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn4.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(106, 41);
            ultraGridColumn4.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn4.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            appearance3.TextHAlignAsString = "Left";
            ultraGridColumn6.CellAppearance = appearance3;
            ultraGridColumn6.Header.Editor = null;
            ultraGridColumn6.Header.VisiblePosition = 7;
            ultraGridColumn6.RowLayoutColumnInfo.OriginX = 4;
            ultraGridColumn6.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn6.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(106, 27);
            ultraGridColumn6.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn6.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn10.Header.Editor = null;
            ultraGridColumn10.Header.VisiblePosition = 2;
            ultraGridColumn10.MaxLength = 16;
            ultraGridColumn10.RowLayoutColumnInfo.OriginX = 6;
            ultraGridColumn10.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn10.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(0, 41);
            ultraGridColumn10.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn10.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn10.Width = 98;
            ultraGridColumn11.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn11.Header.Editor = null;
            ultraGridColumn11.Header.VisiblePosition = 4;
            ultraGridColumn11.NullText = "Serum/Plasma";
            ultraGridColumn11.RowLayoutColumnInfo.OriginX = 8;
            ultraGridColumn11.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn11.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(137, 41);
            ultraGridColumn11.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn11.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn11.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ultraGridColumn11.Width = 403;
            ultraGridColumn12.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn12.Header.Editor = null;
            ultraGridColumn12.Header.VisiblePosition = 5;
            ultraGridColumn12.RowLayoutColumnInfo.OriginX = 10;
            ultraGridColumn12.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn12.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(201, 41);
            ultraGridColumn12.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn12.RowLayoutColumnInfo.SpanY = 2;
            appearance4.TextHAlignAsString = "Left";
            ultraGridColumn13.CellAppearance = appearance4;
            ultraGridColumn13.Header.Editor = null;
            ultraGridColumn13.Header.VisiblePosition = 3;
            ultraGridColumn13.MaskInput = "nnnn";
            ultraGridColumn13.MaxValue = 1000;
            ultraGridColumn13.MinValue = 1;
            ultraGridColumn13.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Disallow;
            ultraGridColumn13.NullText = "1";
            ultraGridColumn13.PromptChar = ' ';
            ultraGridColumn13.RegexPattern = "^((?<value>[0-9]+))$";
            ultraGridColumn13.RowLayoutColumnInfo.OriginX = 12;
            ultraGridColumn13.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn13.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(144, 41);
            ultraGridColumn13.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn13.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn14.Header.Editor = null;
            ultraGridColumn14.Header.VisiblePosition = 6;
            ultraGridColumn14.MaxLength = 80;
            ultraGridColumn14.MinWidth = 80;
            ultraGridColumn14.RowLayoutColumnInfo.OriginX = 14;
            ultraGridColumn14.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn14.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(224, 41);
            ultraGridColumn14.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn14.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn14.Width = 253;
            ultraGridColumn15.Header.Editor = null;
            ultraGridColumn15.Header.VisiblePosition = 8;
            ultraGridColumn16.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn16.Header.Editor = null;
            ultraGridColumn16.Header.VisiblePosition = 9;
            ultraGridColumn16.MinWidth = 17;
            ultraGridColumn16.NullText = "Cup";
            ultraGridColumn16.RowLayoutColumnInfo.OriginX = 8;
            ultraGridColumn16.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn16.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(137, 41);
            ultraGridColumn16.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn4,
            ultraGridColumn6,
            ultraGridColumn10,
            ultraGridColumn11,
            ultraGridColumn12,
            ultraGridColumn13,
            ultraGridColumn14,
            ultraGridColumn15,
            ultraGridColumn16});
            ultraGridBand1.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            ultraGridBand1.Override.RowSelectorWidth = 50;
            this.grdSpecimenStatRegistrationFixed.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdSpecimenStatRegistrationFixed.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.grdSpecimenStatRegistrationFixed.DisplayLayout.Override.ActiveRowAppearance = appearance5;
            this.grdSpecimenStatRegistrationFixed.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdSpecimenStatRegistrationFixed.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdSpecimenStatRegistrationFixed.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdSpecimenStatRegistrationFixed.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            appearance6.BackColor = System.Drawing.Color.Transparent;
            this.grdSpecimenStatRegistrationFixed.DisplayLayout.Override.CardAreaAppearance = appearance6;
            this.grdSpecimenStatRegistrationFixed.DisplayLayout.Override.DefaultRowHeight = 50;
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(162)))), ((int)(((byte)(173)))));
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(132)))), ((int)(((byte)(142)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance7.FontData.BoldAsString = "True";
            appearance7.FontData.Name = "Arial";
            appearance7.FontData.SizeInPoints = 10F;
            appearance7.ForeColor = System.Drawing.Color.White;
            appearance7.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdSpecimenStatRegistrationFixed.DisplayLayout.Override.HeaderAppearance = appearance7;
            this.grdSpecimenStatRegistrationFixed.DisplayLayout.Override.MaxSelectedCells = 1;
            this.grdSpecimenStatRegistrationFixed.DisplayLayout.Override.MaxSelectedRows = 1;
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(162)))), ((int)(((byte)(173)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(132)))), ((int)(((byte)(142)))));
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grdSpecimenStatRegistrationFixed.DisplayLayout.Override.RowSelectorAppearance = appearance8;
            this.grdSpecimenStatRegistrationFixed.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.None;
            this.grdSpecimenStatRegistrationFixed.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            appearance9.ForeColor = System.Drawing.Color.Black;
            this.grdSpecimenStatRegistrationFixed.DisplayLayout.Override.SelectedRowAppearance = appearance9;
            this.grdSpecimenStatRegistrationFixed.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSpecimenStatRegistrationFixed.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.SingleAutoDrag;
            scrollBarLook1.ViewStyle = Infragistics.Win.UltraWinScrollBar.ScrollBarViewStyle.Office2013;
            this.grdSpecimenStatRegistrationFixed.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdSpecimenStatRegistrationFixed.DisplayLayout.Scrollbars = Infragistics.Win.UltraWinGrid.Scrollbars.Both;
            valueList1.DisplayStyle = Infragistics.Win.ValueListDisplayStyle.DisplayText;
            valueListItem1.DataValue = "ValueListItem0";
            valueListItem1.DisplayText = "Serum/Plasm";
            valueListItem2.DataValue = "ValueListItem1";
            valueList1.ValueListItems.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.grdSpecimenStatRegistrationFixed.DisplayLayout.ValueLists.AddRange(new Infragistics.Win.ValueList[] {
            valueList1,
            valueList2});
            this.grdSpecimenStatRegistrationFixed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSpecimenStatRegistrationFixed.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdSpecimenStatRegistrationFixed.Location = new System.Drawing.Point(0, 164);
            this.grdSpecimenStatRegistrationFixed.Name = "grdSpecimenStatRegistrationFixed";
            this.grdSpecimenStatRegistrationFixed.Size = new System.Drawing.Size(1415, 238);
            this.grdSpecimenStatRegistrationFixed.TabIndex = 43;
            this.grdSpecimenStatRegistrationFixed.ZoomStep = 10;
            this.grdSpecimenStatRegistrationFixed.AfterCellActivate += new System.EventHandler(this.grdSpecimenStatRegistrationFixed_AfterCellActivate);
            this.grdSpecimenStatRegistrationFixed.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSpecimenStatRegistrationFixed_InitializeLayout);
            this.grdSpecimenStatRegistrationFixed.AfterExitEditMode += new System.EventHandler(this.grdSpecimenStatRegistrationFixed_AfterExitEditMode);
            this.grdSpecimenStatRegistrationFixed.AfterRowActivate += new System.EventHandler(this.grdSpecimenStatRegistrationFixed_AfterRowActivate);
            this.grdSpecimenStatRegistrationFixed.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdSpecimenStatRegistrationFixed_CellChange);
            this.grdSpecimenStatRegistrationFixed.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdSpecimenStatRegistrationFixed_AfterCellListCloseUp);
            this.grdSpecimenStatRegistrationFixed.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdSpecimenStatRegistrationFixed_AfterSelectChange);
            this.grdSpecimenStatRegistrationFixed.BeforeSelectChange += new Infragistics.Win.UltraWinGrid.BeforeSelectChangeEventHandler(this.grdSpecimenStatRegistrationFixed_BeforeSelectChange);
            this.grdSpecimenStatRegistrationFixed.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.grdSpecimenStatRegistrationFixed_BeforeExitEditMode);
            this.grdSpecimenStatRegistrationFixed.CellDataError += new Infragistics.Win.UltraWinGrid.CellDataErrorEventHandler(this.grdSpecimenStatRegistrationFixed_CellDataError);
            this.grdSpecimenStatRegistrationFixed.ClickCell += new Infragistics.Win.UltraWinGrid.ClickCellEventHandler(this.grdSpecimenStatRegistrationFixed_ClickCell);
            // 
            // dscSpecimenStatRegistrationFixed
            // 
            this.dscSpecimenStatRegistrationFixed.Band.Columns.AddRange(new object[] {
            ultraDataColumn11,
            ultraDataColumn12,
            ultraDataColumn13,
            ultraDataColumn14,
            ultraDataColumn15,
            ultraDataColumn16,
            ultraDataColumn17,
            ultraDataColumn18,
            ultraDataColumn19,
            ultraDataColumn20});
            this.dscSpecimenStatRegistrationFixed.Band.Key = "バンド 0";
            this.dscSpecimenStatRegistrationFixed.Rows.AddRange(new object[] {
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0001")),
                        ((object)("Rack Position")),
                        ((object)("1")),
                        ((object)("Receipt No.")),
                        ((object)("1")),
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
                        ((object)("Manual dilution ratio")),
                        ((object)("X1"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0002")),
                        ((object)("Rack Position")),
                        ((object)("4")),
                        ((object)("Receipt No.")),
                        ((object)("2")),
                        ((object)("Manual dilution ratio")),
                        ((object)("X1"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("RackID")),
                        ((object)("0002")),
                        ((object)("Rack Position")),
                        ((object)("5")),
                        ((object)("Receipt No.")),
                        ((object)("2")),
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
            // lblFixedRegistration
            // 
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(96)))), ((int)(((byte)(105)))));
            appearance10.BorderColor = System.Drawing.Color.Black;
            appearance10.ForeColor = System.Drawing.Color.White;
            appearance10.TextVAlignAsString = "Middle";
            this.lblFixedRegistration.Appearance = appearance10;
            this.lblFixedRegistration.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFixedRegistration.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblFixedRegistration.Location = new System.Drawing.Point(0, 136);
            this.lblFixedRegistration.Name = "lblFixedRegistration";
            this.lblFixedRegistration.Size = new System.Drawing.Size(1415, 28);
            this.lblFixedRegistration.TabIndex = 58;
            this.lblFixedRegistration.Text = "Fixed registration (Priority 2)";
            // 
            // grdSpecimenStatRegistration
            // 
            this.grdSpecimenStatRegistration.DataSource = this.dscSpecimenStatRegistration;
            ultraGridBand2.ColHeaderLines = 2;
            appearance11.TextHAlignAsString = "Left";
            ultraGridColumn17.CellAppearance = appearance11;
            ultraGridColumn17.Header.Caption = "RackID123";
            ultraGridColumn17.Header.Editor = null;
            ultraGridColumn17.Header.VisiblePosition = 1;
            ultraGridColumn17.Hidden = true;
            ultraGridColumn17.MaskInput = "nnnn";
            ultraGridColumn17.MaxLength = 4;
            ultraGridColumn17.PromptChar = ' ';
            ultraGridColumn17.RowLayoutColumnInfo.OriginX = 0;
            ultraGridColumn17.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn17.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(104, 41);
            ultraGridColumn17.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn17.RowLayoutColumnInfo.SpanY = 2;
            appearance12.TextHAlignAsString = "Left";
            ultraGridColumn18.CellAppearance = appearance12;
            ultraGridColumn18.Header.Editor = null;
            ultraGridColumn18.Header.VisiblePosition = 0;
            ultraGridColumn18.MaskInput = "n";
            ultraGridColumn18.MaxLength = 1;
            ultraGridColumn18.MaxValue = "5";
            ultraGridColumn18.MinValue = "1";
            ultraGridColumn18.PromptChar = ' ';
            ultraGridColumn18.RowLayoutColumnInfo.OriginX = 2;
            ultraGridColumn18.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn18.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(106, 41);
            ultraGridColumn18.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn18.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn19.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            appearance13.TextHAlignAsString = "Left";
            ultraGridColumn19.CellAppearance = appearance13;
            ultraGridColumn19.Header.Editor = null;
            ultraGridColumn19.Header.VisiblePosition = 7;
            ultraGridColumn19.RowLayoutColumnInfo.OriginX = 4;
            ultraGridColumn19.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn19.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(106, 27);
            ultraGridColumn19.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn19.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn20.Header.Editor = null;
            ultraGridColumn20.Header.VisiblePosition = 2;
            ultraGridColumn20.MaxLength = 16;
            ultraGridColumn20.RowLayoutColumnInfo.OriginX = 6;
            ultraGridColumn20.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn20.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(0, 41);
            ultraGridColumn20.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn20.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn20.Width = 98;
            ultraGridColumn21.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn21.Header.Editor = null;
            ultraGridColumn21.Header.VisiblePosition = 4;
            ultraGridColumn21.NullText = "Serum/Plasma";
            ultraGridColumn21.RowLayoutColumnInfo.OriginX = 8;
            ultraGridColumn21.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn21.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(137, 41);
            ultraGridColumn21.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn21.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn21.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ultraGridColumn21.Width = 296;
            ultraGridColumn22.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn22.Header.Editor = null;
            ultraGridColumn22.Header.VisiblePosition = 5;
            ultraGridColumn22.RowLayoutColumnInfo.OriginX = 10;
            ultraGridColumn22.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn22.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(201, 41);
            ultraGridColumn22.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn22.RowLayoutColumnInfo.SpanY = 2;
            appearance14.TextHAlignAsString = "Left";
            ultraGridColumn23.CellAppearance = appearance14;
            ultraGridColumn23.Header.Editor = null;
            ultraGridColumn23.Header.VisiblePosition = 3;
            ultraGridColumn23.MaskInput = "nnnn";
            ultraGridColumn23.MaxValue = 1000;
            ultraGridColumn23.MinValue = 1;
            ultraGridColumn23.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Disallow;
            ultraGridColumn23.NullText = "1";
            ultraGridColumn23.PromptChar = ' ';
            ultraGridColumn23.RegexPattern = "^((?<value>[0-9]+))$";
            ultraGridColumn23.RowLayoutColumnInfo.OriginX = 12;
            ultraGridColumn23.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn23.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(144, 41);
            ultraGridColumn23.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn23.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn24.Header.Editor = null;
            ultraGridColumn24.Header.VisiblePosition = 6;
            ultraGridColumn24.MaxLength = 80;
            ultraGridColumn24.MinWidth = 80;
            ultraGridColumn24.RowLayoutColumnInfo.OriginX = 14;
            ultraGridColumn24.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn24.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(224, 41);
            ultraGridColumn24.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn24.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn24.Width = 253;
            ultraGridColumn25.Header.Editor = null;
            ultraGridColumn25.Header.VisiblePosition = 8;
            ultraGridColumn26.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn26.Header.Editor = null;
            ultraGridColumn26.Header.VisiblePosition = 9;
            ultraGridColumn26.NullText = "Cup";
            ultraGridColumn26.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn17,
            ultraGridColumn18,
            ultraGridColumn19,
            ultraGridColumn20,
            ultraGridColumn21,
            ultraGridColumn22,
            ultraGridColumn23,
            ultraGridColumn24,
            ultraGridColumn25,
            ultraGridColumn26});
            ultraGridBand2.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            ultraGridBand2.Override.RowSelectorWidth = 50;
            this.grdSpecimenStatRegistration.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.grdSpecimenStatRegistration.DisplayLayout.MaxRowScrollRegions = 1;
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.grdSpecimenStatRegistration.DisplayLayout.Override.ActiveRowAppearance = appearance15;
            this.grdSpecimenStatRegistration.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdSpecimenStatRegistration.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdSpecimenStatRegistration.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdSpecimenStatRegistration.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            appearance16.BackColor = System.Drawing.Color.Transparent;
            this.grdSpecimenStatRegistration.DisplayLayout.Override.CardAreaAppearance = appearance16;
            this.grdSpecimenStatRegistration.DisplayLayout.Override.DefaultRowHeight = 50;
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(162)))), ((int)(((byte)(173)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(132)))), ((int)(((byte)(142)))));
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance17.FontData.BoldAsString = "True";
            appearance17.FontData.Name = "Arial";
            appearance17.FontData.SizeInPoints = 10F;
            appearance17.ForeColor = System.Drawing.Color.White;
            appearance17.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdSpecimenStatRegistration.DisplayLayout.Override.HeaderAppearance = appearance17;
            appearance18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(162)))), ((int)(((byte)(173)))));
            appearance18.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(132)))), ((int)(((byte)(142)))));
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grdSpecimenStatRegistration.DisplayLayout.Override.RowSelectorAppearance = appearance18;
            this.grdSpecimenStatRegistration.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.None;
            this.grdSpecimenStatRegistration.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            appearance19.ForeColor = System.Drawing.Color.Black;
            this.grdSpecimenStatRegistration.DisplayLayout.Override.SelectedRowAppearance = appearance19;
            this.grdSpecimenStatRegistration.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            scrollBarLook2.ViewStyle = Infragistics.Win.UltraWinScrollBar.ScrollBarViewStyle.Office2013;
            this.grdSpecimenStatRegistration.DisplayLayout.ScrollBarLook = scrollBarLook2;
            this.grdSpecimenStatRegistration.DisplayLayout.Scrollbars = Infragistics.Win.UltraWinGrid.Scrollbars.Horizontal;
            valueList3.DisplayStyle = Infragistics.Win.ValueListDisplayStyle.DisplayText;
            valueListItem3.DataValue = "ValueListItem0";
            valueListItem3.DisplayText = "Serum/Plasm";
            valueListItem4.DataValue = "ValueListItem1";
            valueList3.ValueListItems.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem3,
            valueListItem4});
            this.grdSpecimenStatRegistration.DisplayLayout.ValueLists.AddRange(new Infragistics.Win.ValueList[] {
            valueList3,
            valueList4});
            this.grdSpecimenStatRegistration.Dock = System.Windows.Forms.DockStyle.Top;
            this.grdSpecimenStatRegistration.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdSpecimenStatRegistration.Location = new System.Drawing.Point(0, 28);
            this.grdSpecimenStatRegistration.Name = "grdSpecimenStatRegistration";
            this.grdSpecimenStatRegistration.Size = new System.Drawing.Size(1415, 108);
            this.grdSpecimenStatRegistration.TabIndex = 43;
            this.grdSpecimenStatRegistration.ZoomStep = 10;
            this.grdSpecimenStatRegistration.AfterCellActivate += new System.EventHandler(this.grdSpecimenStatRegistration_AfterCellActivate);
            this.grdSpecimenStatRegistration.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSpecimenStatRegistration_InitializeLayout);
            this.grdSpecimenStatRegistration.AfterExitEditMode += new System.EventHandler(this.grdSpecimenStatRegistration_AfterExitEditMode);
            this.grdSpecimenStatRegistration.AfterRowActivate += new System.EventHandler(this.grdSpecimenStatRegistration_AfterRowActivate);
            this.grdSpecimenStatRegistration.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdSpecimenStatRegistration_CellChange);
            this.grdSpecimenStatRegistration.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdSpecimenStatRegistration_AfterCellListCloseUp);
            this.grdSpecimenStatRegistration.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdSpecimenStatRegistration_AfterSelectChange);
            this.grdSpecimenStatRegistration.BeforeSelectChange += new Infragistics.Win.UltraWinGrid.BeforeSelectChangeEventHandler(this.grdSpecimenStatRegistration_BeforeSelectChange);
            this.grdSpecimenStatRegistration.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.grdSpecimenStatRegistration_BeforeExitEditMode);
            this.grdSpecimenStatRegistration.CellDataError += new Infragistics.Win.UltraWinGrid.CellDataErrorEventHandler(this.grdSpecimenStatRegistration_CellDataError);
            this.grdSpecimenStatRegistration.ClickCell += new Infragistics.Win.UltraWinGrid.ClickCellEventHandler(this.grdSpecimenStatRegistration_ClickCell);
            // 
            // lblTemporaryRegistration
            // 
            appearance20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(96)))), ((int)(((byte)(105)))));
            appearance20.BorderColor = System.Drawing.Color.Black;
            appearance20.ForeColor = System.Drawing.Color.White;
            appearance20.TextVAlignAsString = "Middle";
            this.lblTemporaryRegistration.Appearance = appearance20;
            this.lblTemporaryRegistration.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTemporaryRegistration.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTemporaryRegistration.Location = new System.Drawing.Point(0, 0);
            this.lblTemporaryRegistration.Name = "lblTemporaryRegistration";
            this.lblTemporaryRegistration.Size = new System.Drawing.Size(1415, 28);
            this.lblTemporaryRegistration.TabIndex = 57;
            this.lblTemporaryRegistration.Text = "Temporary registration (Priority 1)";
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
            appearance21.BackColor = System.Drawing.Color.Transparent;
            appearance21.FontData.Name = "Arial";
            appearance21.FontData.SizeInPoints = 12.71F;
            this.tlbCommandBar.Appearance = appearance21;
            this.tlbCommandBar.DesignerFlags = 1;
            this.tlbCommandBar.DockWithinContainer = this.pnlCommandBar.ClientArea;
            this.tlbCommandBar.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.Standard;
            this.tlbCommandBar.ImageSizeLarge = new System.Drawing.Size(56, 63);
            this.tlbCommandBar.ImageSizeSmall = new System.Drawing.Size(32, 32);
            this.tlbCommandBar.LockToolbars = true;
            this.tlbCommandBar.ShowFullMenusDelay = 500;
            this.tlbCommandBar.ShowQuickCustomizeButton = false;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.FloatingLocation = new System.Drawing.Point(518, 446);
            ultraToolbar1.FloatingSize = new System.Drawing.Size(1065, 90);
            buttonTool2.InstanceProps.IsFirstInGroup = true;
            buttonTool7.InstanceProps.IsFirstInGroup = true;
            buttonTool11.InstanceProps.IsFirstInGroup = true;
            buttonTool9.InstanceProps.IsFirstInGroup = true;
            labelTool1.InstanceProps.IsFirstInGroup = true;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool2,
            buttonTool7,
            buttonTool11,
            buttonTool9,
            labelTool1});
            ultraToolbar1.Text = "ToolBar";
            this.tlbCommandBar.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            this.tlbCommandBar.ToolbarSettings.FillEntireRow = Infragistics.Win.DefaultableBoolean.True;
            appearance23.BackColor = System.Drawing.Color.Transparent;
            appearance23.BorderColor = System.Drawing.Color.Transparent;
            this.tlbCommandBar.ToolbarSettings.HotTrackAppearance = appearance23;
            this.tlbCommandBar.ToolbarSettings.PaddingBottom = 11;
            this.tlbCommandBar.ToolbarSettings.PaddingLeft = 36;
            this.tlbCommandBar.ToolbarSettings.PaddingTop = 12;
            this.tlbCommandBar.ToolbarSettings.ToolSpacing = 25;
            appearance24.Image = global::Oelco.CarisX.Properties.Resources.Image_Regist;
            buttonTool3.SharedPropsInternal.AppearancesLarge.Appearance = appearance24;
            buttonTool3.SharedPropsInternal.Caption = "Save";
            buttonTool3.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance25.Image = global::Oelco.CarisX.Properties.Resources.Image_Erase;
            buttonTool4.SharedPropsInternal.AppearancesLarge.Appearance = appearance25;
            buttonTool4.SharedPropsInternal.Caption = "Delete";
            buttonTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance26.Image = global::Oelco.CarisX.Properties.Resources.Image_Delete;
            buttonTool8.SharedPropsInternal.AppearancesLarge.Appearance = appearance26;
            buttonTool8.SharedPropsInternal.Caption = "Delete all";
            buttonTool8.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance27.Image = global::Oelco.CarisX.Properties.Resources.Image_Print;
            buttonTool12.SharedPropsInternal.AppearancesLarge.Appearance = appearance27;
            buttonTool12.SharedPropsInternal.Caption = "Print";
            buttonTool12.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance28.Image = global::Oelco.CarisX.Properties.Resources.Image_Copy;
            buttonTool15.SharedPropsInternal.AppearancesLarge.Appearance = appearance28;
            buttonTool15.SharedPropsInternal.Caption = "Copy";
            buttonTool15.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance29.Image = global::Oelco.CarisX.Properties.Resources.Image_Paste;
            buttonTool16.SharedPropsInternal.AppearancesLarge.Appearance = appearance29;
            buttonTool16.SharedPropsInternal.Caption = "Paste";
            buttonTool16.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance30.Image = global::Oelco.CarisX.Properties.Resources.Image_Online;
            buttonTool10.SharedPropsInternal.AppearancesLarge.Appearance = appearance30;
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
            appearance22.BackColor = System.Drawing.Color.Transparent;
            appearance22.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_MenuBarBackground;
            appearance22.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.pnlCommandBar.Appearance = appearance22;
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
            // FormSpecimenStatRegistration
            // 
            this.ClientSize = new System.Drawing.Size(1439, 1005);
            this.Controls.Add(this.zoomPanel);
            this.Controls.Add(this.pnlCommandBar);
            this.Controls.Add(this.pnlDockBase);
            this.DoubleBuffered = true;
            this.Name = "FormSpecimenStatRegistration";
            this.Text = "FormSpecimenStatRegistration";
            this.OnFadeShown += new System.EventHandler(this.FormSpecimenStatRegistration_OnFadeShown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormSpecimenStatRegistration_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dscSpecimenStatRegistration)).EndInit();
            this.pnlDockBase.ClientArea.ResumeLayout(false);
            this.pnlDockBase.ResumeLayout(false);
            this.gesturePanel.ClientArea.ResumeLayout(false);
            this.gesturePanel.ResumeLayout(false);
            this.gesturePanelFixed.ClientArea.ResumeLayout(false);
            this.gesturePanelFixed.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSpecimenStatRegistrationFixed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dscSpecimenStatRegistrationFixed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSpecimenStatRegistration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tlbCommandBar)).EndInit();
            this.pnlCommandBar.ClientArea.ResumeLayout(false);
            this.pnlCommandBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinDataSource.UltraDataSource dscSpecimenStatRegistration;
        private Infragistics.Win.Misc.UltraPanel pnlDockBase;
        private Oelco.Common.GUI.CustomGrid grdSpecimenStatRegistration;
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
        private Oelco.Common.GUI.GesturePanel gesturePanelFixed;
        private Oelco.Common.GUI.CustomGrid grdSpecimenStatRegistrationFixed;
        private Infragistics.Win.Misc.UltraLabel lblTemporaryRegistration;
        private Infragistics.Win.Misc.UltraLabel lblFixedRegistration;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource dscSpecimenStatRegistrationFixed;
    }
}