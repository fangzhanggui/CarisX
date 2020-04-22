namespace Prototype
{
    partial class AreaTest
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
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPictureBox1 = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ultraPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraButton1
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            this.ultraButton1.Appearance = appearance1;
            this.ultraButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Borderless;
            this.ultraButton1.Location = new System.Drawing.Point(12, 74);
            this.ultraButton1.Name = "ultraButton1";
            appearance4.AlphaLevel = ((short)(100));
            appearance4.BackColor = System.Drawing.Color.Orange;
            this.ultraButton1.PressedAppearance = appearance4;
            this.ultraButton1.ShowFocusRect = false;
            this.ultraButton1.ShowOutline = false;
            this.ultraButton1.Size = new System.Drawing.Size(113, 235);
            this.ultraButton1.TabIndex = 0;
            this.ultraButton1.Text = "ultraButton1";
            this.ultraButton1.UseAppStyling = false;
            this.ultraButton1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraButton1.Click += new System.EventHandler(this.ultraButton1_Click);
            // 
            // ultraPanel1
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ultraPanel1.Appearance = appearance2;
            this.ultraPanel1.Location = new System.Drawing.Point(148, 77);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(116, 231);
            this.ultraPanel1.TabIndex = 1;
            this.ultraPanel1.Click += new System.EventHandler(this.ultraPanel1_Click);
            // 
            // ultraPictureBox1
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            this.ultraPictureBox1.Appearance = appearance3;
            this.ultraPictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.ultraPictureBox1.BorderShadowColor = System.Drawing.Color.Empty;
            this.ultraPictureBox1.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.ultraPictureBox1.Location = new System.Drawing.Point(285, 78);
            this.ultraPictureBox1.Name = "ultraPictureBox1";
            this.ultraPictureBox1.Size = new System.Drawing.Size(117, 230);
            this.ultraPictureBox1.TabIndex = 2;
            this.ultraPictureBox1.UseAppStyling = false;
            this.ultraPictureBox1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraPictureBox1.Click += new System.EventHandler(this.ultraPictureBox1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(306, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "左からボタン、パネル、ピクチャボックス 不可視で操作可能か試す";
            // 
            // AreaTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Prototype.Properties.Resources.Panel2;
            this.ClientSize = new System.Drawing.Size(411, 321);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ultraPictureBox1);
            this.Controls.Add(this.ultraPanel1);
            this.Controls.Add(this.ultraButton1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AreaTest";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "AreaTest";
            this.ultraPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton ultraButton1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox ultraPictureBox1;
        private System.Windows.Forms.Label label1;
    }
}