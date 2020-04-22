namespace Prototype
{
    partial class TimeLabeler
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
            this.customLabel1 = new Oelco.Common.GUI.CustomLabel();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // customLabel1
            // 
            this.customLabel1.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.customLabel1.Location = new System.Drawing.Point(70, 58);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(161, 34);
            this.customLabel1.TabIndex = 0;
            this.customLabel1.Text = "customLabel1";
            this.customLabel1.TimeFormatString = "mm\\:ss";
            this.customLabel1.TimeOver += new System.EventHandler(this.customLabel1_TimeOver);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(116, 186);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(50, 34);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TimeLabeler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.customLabel1);
            this.Name = "TimeLabeler";
            this.Text = "TimeLabeler";
            this.ResumeLayout(false);

        }

        #endregion

        private Oelco.Common.GUI.CustomLabel customLabel1;
        private System.Windows.Forms.Button button1;

    }
}