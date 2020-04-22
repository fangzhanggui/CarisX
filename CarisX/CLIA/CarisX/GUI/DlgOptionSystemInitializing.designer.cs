namespace Oelco.CarisX.GUI
{
    partial class DlgOptionSystemInitializing
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
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            this.btnOk = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.optEditMode = new Oelco.Common.GUI.CustomUOptionSet();
            this.lblCaution = new Infragistics.Win.Misc.UltraLabel();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optEditMode)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOk);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 276);
            this.pnlDialogButton.Size = new System.Drawing.Size(499, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblCaution);
            this.pnlDialogMain.ClientArea.Controls.Add(this.ultraGroupBox1);
            this.pnlDialogMain.Size = new System.Drawing.Size(499, 276);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(499, 28);
            this.lblDialogTitle.Text = "System Initializing";
            // 
            // btnOk
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance1.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnOk.Appearance = appearance1;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnOk.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance2.BorderColor = System.Drawing.Color.Transparent;
            this.btnOk.HotTrackAppearance = appearance2;
            this.btnOk.ImageSize = new System.Drawing.Size(0, 0);
            this.btnOk.Location = new System.Drawing.Point(145, 9);
            this.btnOk.Name = "btnOk";
            this.btnOk.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnOk.PressedAppearance = appearance3;
            this.btnOk.ShowFocusRect = false;
            this.btnOk.ShowOutline = false;
            this.btnOk.Size = new System.Drawing.Size(152, 39);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BorderColor = System.Drawing.Color.Transparent;
            appearance4.Image = global::Oelco.CarisX.Properties.Resources.Image_Exit;
            appearance4.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnCancel.Appearance = appearance4;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance5.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.HotTrackAppearance = appearance5;
            this.btnCancel.ImageSize = new System.Drawing.Size(0, 0);
            this.btnCancel.Location = new System.Drawing.Point(321, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Drawing.Size(10, 0);
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.PressedAppearance = appearance6;
            this.btnCancel.ShowFocusRect = false;
            this.btnCancel.ShowOutline = false;
            this.btnCancel.Size = new System.Drawing.Size(152, 39);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.optEditMode);
            this.ultraGroupBox1.Location = new System.Drawing.Point(25, 59);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(448, 125);
            this.ultraGroupBox1.TabIndex = 1;
            this.ultraGroupBox1.Text = "Processing system initialization";
            // 
            // optEditMode
            // 
            this.optEditMode.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            valueListItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem1.DataValue = true;
            valueListItem1.DisplayText = "Initialization of the analyzer";
            valueListItem2.DataValue = false;
            valueListItem2.DisplayText = "Initialization of the state analysis";
            this.optEditMode.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.optEditMode.ItemSpacingVertical = 10;
            this.optEditMode.Location = new System.Drawing.Point(29, 35);
            this.optEditMode.Name = "optEditMode";
            this.optEditMode.Size = new System.Drawing.Size(387, 78);
            this.optEditMode.TabIndex = 0;
            // 
            // lblCaution
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            appearance7.FontData.Name = "default";
            appearance7.ForeColor = System.Drawing.Color.Red;
            this.lblCaution.Appearance = appearance7;
            this.lblCaution.Location = new System.Drawing.Point(25, 214);
            this.lblCaution.Name = "lblCaution";
            this.lblCaution.Size = new System.Drawing.Size(448, 39);
            this.lblCaution.TabIndex = 109;
            this.lblCaution.Text = "Measurement results of the analysis screen is cleared and the initialization of t" +
    "he state to do the analysis.";
            // 
            // DlgOptionSystemInitializing
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(499, 333);
            this.Name = "DlgOptionSystemInitializing";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DlgOptionSystemInitializing_FormClosed);
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optEditMode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.NoBorderButton btnOk;
        private Controls.NoBorderButton btnCancel;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Oelco.Common.GUI.CustomUOptionSet optEditMode;
        private Infragistics.Win.Misc.UltraLabel lblCaution;
    }
}