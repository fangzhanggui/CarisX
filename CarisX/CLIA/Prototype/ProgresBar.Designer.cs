namespace Prototype
{
    partial class ProgressBarTest
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
            Infragistics.Win.UltraWinProgressBar.PercentSetting percentSetting1 = new Infragistics.Win.UltraWinProgressBar.PercentSetting();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinProgressBar.PercentSetting percentSetting2 = new Infragistics.Win.UltraWinProgressBar.PercentSetting();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinProgressBar.PercentSetting percentSetting3 = new Infragistics.Win.UltraWinProgressBar.PercentSetting();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.customProgressBar1 = new Oelco.Common.GUI.CustomProgressBar();
            this.customUButton1 = new Oelco.Common.GUI.CustomUButton();
            this.SuspendLayout();
            // 
            // customProgressBar1
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance1.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            appearance1.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            appearance1.BorderColor3DBase = System.Drawing.Color.Red;
            this.customProgressBar1.FillAppearance = appearance1;
            this.customProgressBar1.Location = new System.Drawing.Point(37, 47);
            this.customProgressBar1.Name = "customProgressBar1";
            appearance3.BackColor = System.Drawing.Color.Red;
            percentSetting1.FillAppearance = appearance3;
            percentSetting1.Percent = new decimal(new int[] {
            23,
            0,
            0,
            0});
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            percentSetting2.FillAppearance = appearance4;
            percentSetting2.Percent = new decimal(new int[] {
            80,
            0,
            0,
            0});
            appearance2.BackColor = System.Drawing.Color.Blue;
            percentSetting3.FillAppearance = appearance2;
            percentSetting3.Percent = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.customProgressBar1.PercentSettings.Add(percentSetting1);
            this.customProgressBar1.PercentSettings.Add(percentSetting2);
            this.customProgressBar1.PercentSettings.Add(percentSetting3);
            this.customProgressBar1.Size = new System.Drawing.Size(210, 100);
            this.customProgressBar1.TabIndex = 0;
            this.customProgressBar1.Text = "[Formatted]";
            this.customProgressBar1.UseAppStyling = false;
            this.customProgressBar1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.customProgressBar1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // customUButton1
            // 
            this.customUButton1.Location = new System.Drawing.Point(79, 171);
            this.customUButton1.Name = "customUButton1";
            this.customUButton1.Size = new System.Drawing.Size(138, 55);
            this.customUButton1.TabIndex = 1;
            this.customUButton1.Text = "customUButton1";
            this.customUButton1.Click += new System.EventHandler(this.customUButton1_Click);
            // 
            // ProgressBarTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.customUButton1);
            this.Controls.Add(this.customProgressBar1);
            this.Name = "ProgressBarTest";
            this.Text = "ProgresBar";
            this.ResumeLayout(false);

        }

        #endregion

        private Oelco.Common.GUI.CustomProgressBar customProgressBar1;
        private Oelco.Common.GUI.CustomUButton customUButton1;
    }
}