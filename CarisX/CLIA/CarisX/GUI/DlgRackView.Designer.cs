namespace Oelco.CarisX.GUI
{
    partial class DlgRackView
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("バンド 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Sequence No.");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Rack ID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Patient ID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Rack Position");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Specimen type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Manual dilution ratio");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Analytes");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Status");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Remaining time");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Count");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Concentration");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Comment");
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            this.btnClose = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.grdRackInfo = new Oelco.Common.GUI.CustomGrid();
            this.gesturePanel = new Oelco.Common.GUI.GesturePanel();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRackInfo)).BeginInit();
            this.gesturePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnClose);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 432);
            this.pnlDialogButton.Size = new System.Drawing.Size(1326, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.grdRackInfo);
            this.pnlDialogMain.ClientArea.Controls.Add(this.gesturePanel);
            this.pnlDialogMain.Size = new System.Drawing.Size(1326, 432);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(1326, 28);
            this.lblDialogTitle.Text = "RackView";
            // 
            // btnClose
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.Image = global::Oelco.CarisX.Properties.Resources.Image_Exit;
            appearance1.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnClose.Appearance = appearance1;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnClose.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance2.BorderColor = System.Drawing.Color.Transparent;
            this.btnClose.HotTrackAppearance = appearance2;
            this.btnClose.ImageSize = new System.Drawing.Size(0, 0);
            this.btnClose.Location = new System.Drawing.Point(1149, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnClose.PressedAppearance = appearance3;
            this.btnClose.ShowFocusRect = false;
            this.btnClose.ShowOutline = false;
            this.btnClose.Size = new System.Drawing.Size(152, 39);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grdRackInfo
            // 
            appearance4.BackColor = System.Drawing.Color.White;
            this.grdRackInfo.DisplayLayout.Appearance = appearance4;
            ultraGridColumn1.Header.Editor = null;
            ultraGridColumn1.Header.VisiblePosition = 1;
            ultraGridColumn2.Header.Editor = null;
            ultraGridColumn2.Header.VisiblePosition = 2;
            ultraGridColumn3.Header.Editor = null;
            ultraGridColumn3.Header.VisiblePosition = 4;
            ultraGridColumn4.Header.Editor = null;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn5.Header.Editor = null;
            ultraGridColumn5.Header.VisiblePosition = 5;
            ultraGridColumn6.Header.Editor = null;
            ultraGridColumn6.Header.VisiblePosition = 6;
            ultraGridColumn7.Header.Editor = null;
            ultraGridColumn7.Header.VisiblePosition = 0;
            ultraGridColumn8.Header.Editor = null;
            ultraGridColumn8.Header.VisiblePosition = 7;
            ultraGridColumn9.Header.Editor = null;
            ultraGridColumn9.Header.VisiblePosition = 8;
            ultraGridColumn10.Header.Editor = null;
            ultraGridColumn10.Header.VisiblePosition = 9;
            ultraGridColumn11.Header.Editor = null;
            ultraGridColumn11.Header.VisiblePosition = 10;
            ultraGridColumn12.Header.Editor = null;
            ultraGridColumn12.Header.VisiblePosition = 11;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10,
            ultraGridColumn11,
            ultraGridColumn12});
            ultraGridBand1.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(106)))), ((int)(((byte)(197)))));
            ultraGridBand1.Override.SelectedRowAppearance = appearance5;
            this.grdRackInfo.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdRackInfo.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdRackInfo.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            appearance6.BackColor = System.Drawing.Color.Transparent;
            this.grdRackInfo.DisplayLayout.Override.CardAreaAppearance = appearance6;
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(135)))), ((int)(((byte)(214)))));
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(59)))), ((int)(((byte)(150)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance7.FontData.BoldAsString = "True";
            appearance7.FontData.Name = "Arial";
            appearance7.FontData.SizeInPoints = 10F;
            appearance7.ForeColor = System.Drawing.Color.White;
            appearance7.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdRackInfo.DisplayLayout.Override.HeaderAppearance = appearance7;
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(135)))), ((int)(((byte)(214)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(59)))), ((int)(((byte)(150)))));
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grdRackInfo.DisplayLayout.Override.RowSelectorAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.LightSteelBlue;
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(149)))), ((int)(((byte)(21)))));
            appearance9.ForeColor = System.Drawing.Color.Black;
            this.grdRackInfo.DisplayLayout.Override.SelectedRowAppearance = appearance9;
            this.grdRackInfo.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdRackInfo.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdRackInfo.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdRackInfo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdRackInfo.Location = new System.Drawing.Point(25, 51);
            this.grdRackInfo.Name = "grdRackInfo";
            this.grdRackInfo.Size = new System.Drawing.Size(1276, 361);
            this.grdRackInfo.TabIndex = 3;
            this.grdRackInfo.ZoomStep = 10;
            // 
            // gesturePanel
            // 
            this.gesturePanel.EnableGestureScroll = true;
            this.gesturePanel.FlickThreshold = ((short)(1500));
            this.gesturePanel.Location = new System.Drawing.Point(25, 51);
            this.gesturePanel.Name = "gesturePanel";
            this.gesturePanel.ScrollProxy = null;
            this.gesturePanel.Size = new System.Drawing.Size(1276, 361);
            this.gesturePanel.TabIndex = 4;
            this.gesturePanel.TouchClientLoc = new System.Drawing.Point(0, 0);
            this.gesturePanel.TouchClientSize = new System.Drawing.Size(0, 0);
            this.gesturePanel.UseFlick = false;
            this.gesturePanel.UseProxy = true;
            this.gesturePanel.UseTouchClientSize = false;
            // 
            // DlgRackView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Caption = "RackView";
            this.ClientSize = new System.Drawing.Size(1326, 489);
            this.Name = "DlgRackView";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DlgRackView_FormClosed);
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdRackInfo)).EndInit();
            this.gesturePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.NoBorderButton btnClose;
        private Oelco.Common.GUI.CustomGrid grdRackInfo;
        private Oelco.Common.GUI.GesturePanel gesturePanel;
    }
}