namespace Oelco.CarisX.GUI
{
    partial class DlgCarisXBase
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
            this.pnlDialogButton = new Infragistics.Win.Misc.UltraPanel();
            this.pnlDialogMain = new Infragistics.Win.Misc.UltraPanel();
            this.lblDialogTitle = new Infragistics.Win.Misc.UltraLabel();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            this.pnlDialogButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 841);
            this.pnlDialogButton.Margin = new System.Windows.Forms.Padding(4);
            this.pnlDialogButton.Name = "pnlDialogButton";
            this.pnlDialogButton.Size = new System.Drawing.Size(723, 57);
            this.pnlDialogButton.TabIndex = 0;
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblDialogTitle);
            this.pnlDialogMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDialogMain.Location = new System.Drawing.Point(0, 0);
            this.pnlDialogMain.Margin = new System.Windows.Forms.Padding(4);
            this.pnlDialogMain.Name = "pnlDialogMain";
            this.pnlDialogMain.Size = new System.Drawing.Size(723, 841);
            this.pnlDialogMain.TabIndex = 1;
            this.pnlDialogMain.SizeChanged += new System.EventHandler(this.pnlDialogMain_SizeChanged);
            // 
            // lblDialogTitle
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(96)))), ((int)(((byte)(105)))));
            appearance1.BorderColor = System.Drawing.Color.Black;
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.lblDialogTitle.Appearance = appearance1;
            this.lblDialogTitle.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblDialogTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDialogTitle.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblDialogTitle.ImageTransparentColor = System.Drawing.Color.Empty;
            this.lblDialogTitle.Location = new System.Drawing.Point(0, 0);
            this.lblDialogTitle.Margin = new System.Windows.Forms.Padding(4);
            this.lblDialogTitle.Name = "lblDialogTitle";
            this.lblDialogTitle.Size = new System.Drawing.Size(723, 36);
            this.lblDialogTitle.TabIndex = 0;
            // 
            // DlgCarisXBase
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(243)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(723, 898);
            this.Controls.Add(this.pnlDialogMain);
            this.Controls.Add(this.pnlDialogButton);
            this.Font = new System.Drawing.Font("Arial", 12F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DlgCarisXBase";
            this.Text = "";
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected Infragistics.Win.Misc.UltraPanel pnlDialogButton;
        protected Infragistics.Win.Misc.UltraPanel pnlDialogMain;
        protected Infragistics.Win.Misc.UltraLabel lblDialogTitle;

    }
}
