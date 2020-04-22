namespace Prototype
{
    partial class ErrorCodeDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
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
            this.btnError = new Infragistics.Win.Misc.UltraButton();
            this.SuspendLayout();
            // 
            // btnError
            // 
            this.btnError.Location = new System.Drawing.Point(39, 34);
            this.btnError.Name = "btnError";
            this.btnError.Size = new System.Drawing.Size(75, 23);
            this.btnError.TabIndex = 0;
            this.btnError.Text = "エラー検知";
            this.btnError.Click += new System.EventHandler(this.btnError_Click);
            // 
            // ErrorCodeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.btnError);
            this.Name = "ErrorCodeDialog";
            this.Text = "ErrorCodeDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnError;
    }
}