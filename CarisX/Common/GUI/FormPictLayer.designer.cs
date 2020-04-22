namespace Oelco.Common.GUI
{
	partial class FormPictLayer
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
			if ( disposing && ( components != null ) )
			{
				CleanResource();
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
            this.ultraPictureBox1 = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.SuspendLayout();
            // 
            // ultraPictureBox1
            // 
            appearance1.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.ultraPictureBox1.Appearance = appearance1;
            this.ultraPictureBox1.BorderShadowColor = System.Drawing.Color.Empty;
            this.ultraPictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPictureBox1.Location = new System.Drawing.Point( 0, 0 );
            this.ultraPictureBox1.MaintainAspectRatio = false;
            this.ultraPictureBox1.Name = "ultraPictureBox1";
            this.ultraPictureBox1.ScaleImage = Infragistics.Win.ScaleImage.Never;
            this.ultraPictureBox1.Size = new System.Drawing.Size( 292, 266 );
            this.ultraPictureBox1.TabIndex = 0;
            // 
            // FormPictLayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkCyan;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size( 292, 266 );
            this.ControlBox = false;
            this.Controls.Add( this.ultraPictureBox1 );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPictLayer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "FormPictLayer";

            this.ResumeLayout( false );

		}

		#endregion

		private Infragistics.Win.UltraWinEditors.UltraPictureBox ultraPictureBox1;
	}
}