namespace Oelco.CarisX.GUI
{
    partial class DlgTargetSelectRange
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
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.customUOptionSet = new Oelco.Common.GUI.CustomUOptionSet();
            this.btnExecute = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customUOptionSet)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnExecute);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 216);
            this.pnlDialogButton.Size = new System.Drawing.Size(482, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.customUOptionSet);
            this.pnlDialogMain.Size = new System.Drawing.Size(482, 216);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(482, 28);
            this.lblDialogTitle.Text = "Output Target Range";
            // 
            // customUOptionSet
            // 
            this.customUOptionSet.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            valueListItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem1.DataValue = 1;
            valueListItem1.DisplayText = "All";
            valueListItem2.DataValue = 2;
            valueListItem2.DisplayText = "Specification";
            this.customUOptionSet.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.customUOptionSet.ItemSpacingVertical = 30;
            this.customUOptionSet.Location = new System.Drawing.Point(106, 68);
            this.customUOptionSet.Name = "customUOptionSet";
            this.customUOptionSet.Size = new System.Drawing.Size(260, 123);
            this.customUOptionSet.TabIndex = 69;
            // 
            // btnExecute
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance1.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnExecute.Appearance = appearance1;
            this.btnExecute.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnExecute.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance2.BorderColor = System.Drawing.Color.Transparent;
            this.btnExecute.HotTrackAppearance = appearance2;
            this.btnExecute.ImageSize = new System.Drawing.Size(0, 0);
            this.btnExecute.Location = new System.Drawing.Point(148, 9);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnExecute.PressedAppearance = appearance3;
            this.btnExecute.ShowFocusRect = false;
            this.btnExecute.ShowOutline = false;
            this.btnExecute.Size = new System.Drawing.Size(152, 39);
            this.btnExecute.TabIndex = 158;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
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
            this.btnCancel.Location = new System.Drawing.Point(306, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Drawing.Size(10, 0);
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.PressedAppearance = appearance6;
            this.btnCancel.ShowFocusRect = false;
            this.btnCancel.ShowOutline = false;
            this.btnCancel.Size = new System.Drawing.Size(152, 39);
            this.btnCancel.TabIndex = 157;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // DlgTargetSelectRange
            // 
            this.Caption = "Output Target Range";
            this.ClientSize = new System.Drawing.Size(482, 273);
            this.Name = "DlgTargetSelectRange";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customUOptionSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Oelco.Common.GUI.CustomUOptionSet customUOptionSet;
        private Controls.NoBorderButton btnExecute;
        private Controls.NoBorderButton btnCancel;

    }
}