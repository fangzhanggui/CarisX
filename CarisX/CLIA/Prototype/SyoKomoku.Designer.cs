namespace Prototype
{
    partial class SyoKomoku
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SyoKomoku));
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.button1 = new System.Windows.Forms.Button();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraPanel1
            // 
            appearance1.ImageBackground = global::Prototype.Properties.Resources.Panel2;
            this.ultraPanel1.Appearance = appearance1;
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraLabel1);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraButton1);
            this.ultraPanel1.Location = new System.Drawing.Point(265, 70);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(247, 301);
            this.ultraPanel1.TabIndex = 0;
            // 
            // ultraLabel1
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel1.Appearance = appearance3;
            this.ultraLabel1.Location = new System.Drawing.Point(25, 112);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(72, 23);
            this.ultraLabel1.TabIndex = 3;
            this.ultraLabel1.Text = "Text";
            // 
            // ultraButton1
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.ImageBackground = ((System.Drawing.Image)(resources.GetObject("appearance2.ImageBackground")));
            this.ultraButton1.Appearance = appearance2;
            this.ultraButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Borderless;
            this.ultraButton1.ImageSize = new System.Drawing.Size(0, 0);
            this.ultraButton1.Location = new System.Drawing.Point(25, 44);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.ShowFocusRect = false;
            this.ultraButton1.Size = new System.Drawing.Size(67, 62);
            this.ultraButton1.TabIndex = 2;
            this.ultraButton1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraButton1.Click += new System.EventHandler(this.ultraButton1_Click);
            this.ultraButton1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ultraButton1_MouseUp);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(29, 79);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 83);
            this.button1.TabIndex = 1;
            this.button1.Text = "押";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SyoKomoku
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 394);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ultraPanel1);
            this.Name = "SyoKomoku";
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraButton ultraButton1;
        private System.Windows.Forms.Button button1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
    }
}