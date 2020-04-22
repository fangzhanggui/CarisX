namespace Oelco.CarisX.GUI
{
    partial class DlgSysReagentLotChange
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.gbxProcessChangeReagentLot = new Infragistics.Win.Misc.UltraGroupBox();
            this.optProcessChangeReagentLot = new Oelco.Common.GUI.CustomUOptionSet();
            this.btnOK = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxProcessChangeReagentLot)).BeginInit();
            this.gbxProcessChangeReagentLot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optProcessChangeReagentLot)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOK);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 221);
            this.pnlDialogButton.Size = new System.Drawing.Size(584, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.gbxProcessChangeReagentLot);
            this.pnlDialogMain.Size = new System.Drawing.Size(584, 221);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(584, 28);
            // 
            // gbxProcessChangeReagentLot
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            this.gbxProcessChangeReagentLot.Appearance = appearance7;
            this.gbxProcessChangeReagentLot.Controls.Add(this.optProcessChangeReagentLot);
            this.gbxProcessChangeReagentLot.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxProcessChangeReagentLot.Location = new System.Drawing.Point(45, 60);
            this.gbxProcessChangeReagentLot.Name = "gbxProcessChangeReagentLot";
            this.gbxProcessChangeReagentLot.Size = new System.Drawing.Size(412, 130);
            this.gbxProcessChangeReagentLot.TabIndex = 109;
            this.gbxProcessChangeReagentLot.Text = "Process change reagent lot";
            // 
            // optProcessChangeReagentLot
            // 
            appearance8.FontData.Name = "Georgia";
            appearance8.FontData.SizeInPoints = 12F;
            appearance8.TextHAlignAsString = "Center";
            appearance8.TextVAlignAsString = "Middle";
            this.optProcessChangeReagentLot.Appearance = appearance8;
            this.optProcessChangeReagentLot.BackColor = System.Drawing.Color.Transparent;
            this.optProcessChangeReagentLot.BackColorInternal = System.Drawing.Color.Transparent;
            this.optProcessChangeReagentLot.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            valueListItem3.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem3.DataValue = "デフォルト項目";
            valueListItem3.DisplayText = "Stop";
            valueListItem4.DataValue = "ValueListItem1";
            valueListItem4.DisplayText = "Continue";
            this.optProcessChangeReagentLot.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem3,
            valueListItem4});
            this.optProcessChangeReagentLot.ItemSpacingHorizontal = 10;
            this.optProcessChangeReagentLot.ItemSpacingVertical = 20;
            this.optProcessChangeReagentLot.Location = new System.Drawing.Point(33, 27);
            this.optProcessChangeReagentLot.Name = "optProcessChangeReagentLot";
            this.optProcessChangeReagentLot.Size = new System.Drawing.Size(194, 89);
            this.optProcessChangeReagentLot.TabIndex = 102;
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
            // DlgSysReagentLotChange
            // 
            this.ClientSize = new System.Drawing.Size(584, 278);
            this.Name = "DlgSysReagentLotChange";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbxProcessChangeReagentLot)).EndInit();
            this.gbxProcessChangeReagentLot.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optProcessChangeReagentLot)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox gbxProcessChangeReagentLot;
        private Oelco.Common.GUI.CustomUOptionSet optProcessChangeReagentLot;
        private Controls.NoBorderButton btnOK;
        private Controls.NoBorderButton btnCancel;
    }
}
