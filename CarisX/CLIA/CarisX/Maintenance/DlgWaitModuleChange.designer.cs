namespace Oelco.CarisX.Maintenance
{
    partial class DlgWaitModuleChange
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.lblPleaseWait = new Infragistics.Win.Misc.UltraLabel();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 213);
            this.pnlDialogButton.Size = new System.Drawing.Size(460, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.lblPleaseWait);
            this.pnlDialogMain.Size = new System.Drawing.Size(460, 213);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(460, 28);
            this.lblDialogTitle.Text = "Module Change";
            // 
            // lblPleaseWait
            // 
            appearance2.TextHAlignAsString = "Center";
            this.lblPleaseWait.Appearance = appearance2;
            this.lblPleaseWait.Location = new System.Drawing.Point(130, 123);
            this.lblPleaseWait.Name = "lblPleaseWait";
            this.lblPleaseWait.Size = new System.Drawing.Size(200, 24);
            this.lblPleaseWait.TabIndex = 0;
            this.lblPleaseWait.Text = "PleaseWait";
            // 
            // DlgModuleChange
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(460, 270);
            this.Name = "DlgModuleChange";
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        protected Infragistics.Win.Misc.UltraLabel lblPleaseWait;
    }
}