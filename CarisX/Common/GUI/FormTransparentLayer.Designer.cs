namespace Oelco.Common.GUI
{
	partial class FormTransparentLayer
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
			this.SuspendLayout();
			// 
			// m_ShowTimer
			// 
			this.m_ShowTimer.Enabled = true;
			this.m_ShowTimer.Interval = 10;
			// 
			// TransparentLayerForm2
			// 
			this.ShowInTaskbar = false;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.ClientSize = new System.Drawing.Size(142, 316);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "TransparentLayerForm2";
			this.Text = "TransparentLayerForm2";
			this.ResumeLayout(false);

		}

		#endregion
	}
}