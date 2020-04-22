namespace Oelco.CarisX.Maintenance
{
    partial class DlgWaitMotorInitialize
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
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.lblModule1Value = new Infragistics.Win.Misc.UltraLabel();
            this.lblModule1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblRackTransferValue = new Infragistics.Win.Misc.UltraLabel();
            this.lblRackTransfer = new Infragistics.Win.Misc.UltraLabel();
            this.lblModule2Value = new Infragistics.Win.Misc.UltraLabel();
            this.lblModule2 = new Infragistics.Win.Misc.UltraLabel();
            this.lblModule3Value = new Infragistics.Win.Misc.UltraLabel();
            this.lblModule3 = new Infragistics.Win.Misc.UltraLabel();
            this.lblModule4Value = new Infragistics.Win.Misc.UltraLabel();
            this.lblModule4 = new Infragistics.Win.Misc.UltraLabel();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 213);
            this.pnlDialogButton.Size = new System.Drawing.Size(461, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblModule4Value);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblModule4);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblModule3Value);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblModule3);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblModule2Value);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblModule2);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblModule1Value);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblModule1);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblRackTransferValue);
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblRackTransfer);
            this.pnlDialogMain.Size = new System.Drawing.Size(461, 213);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(461, 28);
            this.lblDialogTitle.Text = "MotorInitialize";
            // 
            // btnCancel
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.Image = global::Oelco.CarisX.Properties.Resources.Image_Exit;
            appearance1.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnCancel.Appearance = appearance1;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance2.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.HotTrackAppearance = appearance2;
            this.btnCancel.ImageSize = new System.Drawing.Size(0, 0);
            this.btnCancel.Location = new System.Drawing.Point(297, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.PressedAppearance = appearance3;
            this.btnCancel.ShowFocusRect = false;
            this.btnCancel.ShowOutline = false;
            this.btnCancel.Size = new System.Drawing.Size(152, 39);
            this.btnCancel.TabIndex = 159;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblModule1Value
            // 
            this.lblModule1Value.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModule1Value.Location = new System.Drawing.Point(298, 101);
            this.lblModule1Value.Name = "lblModule1Value";
            this.lblModule1Value.Size = new System.Drawing.Size(151, 23);
            this.lblModule1Value.TabIndex = 22;
            this.lblModule1Value.Text = "XXXXX";
            // 
            // lblModule1
            // 
            this.lblModule1.Location = new System.Drawing.Point(76, 101);
            this.lblModule1.Name = "lblModule1";
            this.lblModule1.Size = new System.Drawing.Size(122, 23);
            this.lblModule1.TabIndex = 21;
            this.lblModule1.Text = "Module1";
            // 
            // lblRackTransferValue
            // 
            this.lblRackTransferValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRackTransferValue.Location = new System.Drawing.Point(298, 61);
            this.lblRackTransferValue.Name = "lblRackTransferValue";
            this.lblRackTransferValue.Size = new System.Drawing.Size(151, 23);
            this.lblRackTransferValue.TabIndex = 7;
            this.lblRackTransferValue.Text = "XXXXX";
            // 
            // lblRackTransfer
            // 
            this.lblRackTransfer.Location = new System.Drawing.Point(76, 61);
            this.lblRackTransfer.Name = "lblRackTransfer";
            this.lblRackTransfer.Size = new System.Drawing.Size(122, 23);
            this.lblRackTransfer.TabIndex = 0;
            this.lblRackTransfer.Text = "RackTransfer";
            // 
            // lblModule2Value
            // 
            this.lblModule2Value.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModule2Value.Location = new System.Drawing.Point(298, 130);
            this.lblModule2Value.Name = "lblModule2Value";
            this.lblModule2Value.Size = new System.Drawing.Size(151, 23);
            this.lblModule2Value.TabIndex = 24;
            this.lblModule2Value.Text = "XXXXX";
            // 
            // lblModule2
            // 
            this.lblModule2.Location = new System.Drawing.Point(76, 130);
            this.lblModule2.Name = "lblModule2";
            this.lblModule2.Size = new System.Drawing.Size(122, 23);
            this.lblModule2.TabIndex = 23;
            this.lblModule2.Text = "Module2";
            // 
            // lblModule3Value
            // 
            this.lblModule3Value.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModule3Value.Location = new System.Drawing.Point(298, 159);
            this.lblModule3Value.Name = "lblModule3Value";
            this.lblModule3Value.Size = new System.Drawing.Size(151, 23);
            this.lblModule3Value.TabIndex = 26;
            this.lblModule3Value.Text = "XXXXX";
            // 
            // lblModule3
            // 
            this.lblModule3.Location = new System.Drawing.Point(76, 159);
            this.lblModule3.Name = "lblModule3";
            this.lblModule3.Size = new System.Drawing.Size(122, 23);
            this.lblModule3.TabIndex = 25;
            this.lblModule3.Text = "Module3";
            // 
            // lblModule4Value
            // 
            this.lblModule4Value.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModule4Value.Location = new System.Drawing.Point(298, 188);
            this.lblModule4Value.Name = "lblModule4Value";
            this.lblModule4Value.Size = new System.Drawing.Size(151, 23);
            this.lblModule4Value.TabIndex = 28;
            this.lblModule4Value.Text = "XXXXX";
            // 
            // lblModule4
            // 
            this.lblModule4.Location = new System.Drawing.Point(76, 188);
            this.lblModule4.Name = "lblModule4";
            this.lblModule4.Size = new System.Drawing.Size(122, 23);
            this.lblModule4.TabIndex = 27;
            this.lblModule4.Text = "Module4";
            // 
            // DlgWaitMotorInitialize
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(461, 270);
            this.Name = "DlgWaitMotorInitialize";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DlgWaitMotorInitialize_FormClosing);
            this.Shown += new System.EventHandler(this.DlgWaitMotorInitialize_Shown);
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        protected GUI.Controls.NoBorderButton btnCancel;
        protected Infragistics.Win.Misc.UltraLabel lblRackTransferValue;
        protected Infragistics.Win.Misc.UltraLabel lblRackTransfer;
        protected Infragistics.Win.Misc.UltraLabel lblModule1Value;
        protected Infragistics.Win.Misc.UltraLabel lblModule1;
        protected Infragistics.Win.Misc.UltraLabel lblModule4Value;
        protected Infragistics.Win.Misc.UltraLabel lblModule4;
        protected Infragistics.Win.Misc.UltraLabel lblModule3Value;
        protected Infragistics.Win.Misc.UltraLabel lblModule3;
        protected Infragistics.Win.Misc.UltraLabel lblModule2Value;
        protected Infragistics.Win.Misc.UltraLabel lblModule2;
    }
}