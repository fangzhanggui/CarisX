namespace Oelco.CarisX.GUI
{
    partial class DlgSplash
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
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
            this.lblInitialize = new Infragistics.Win.Misc.UltraLabel();
            this.ultraPictureBox1 = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.SuspendLayout();
            // 
            // lblInitialize
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            this.lblInitialize.Appearance = appearance1;
            this.lblInitialize.Location = new System.Drawing.Point(12, 171);
            this.lblInitialize.Name = "lblInitialize";
            this.lblInitialize.Size = new System.Drawing.Size(100, 23);
            this.lblInitialize.TabIndex = 0;
            this.lblInitialize.Text = "Initialize...";
            // 
            // ultraPictureBox1
            // 
            this.ultraPictureBox1.BorderShadowColor = System.Drawing.Color.Empty;
            this.ultraPictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPictureBox1.Location = new System.Drawing.Point(0, 0);
            this.ultraPictureBox1.Name = "ultraPictureBox1";
            this.ultraPictureBox1.ScaleImage = Infragistics.Win.ScaleImage.Never;
            this.ultraPictureBox1.Size = new System.Drawing.Size(370, 206);
            this.ultraPictureBox1.TabIndex = 1;
            // 
            // DlgSplash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 206);
            this.Controls.Add(this.lblInitialize);
            this.Controls.Add(this.ultraPictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DlgSplash";
            this.Text = "DlgSplash";
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblInitialize;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox ultraPictureBox1;
    }
}