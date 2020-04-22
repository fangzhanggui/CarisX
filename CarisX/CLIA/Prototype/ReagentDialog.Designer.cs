namespace Prototype
{
    partial class ReagentDialog
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
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton2 = new Infragistics.Win.Misc.UltraButton();
            this.SuspendLayout();
            // 
            // ultraButton1
            // 
            this.ultraButton1.Location = new System.Drawing.Point(59, 46);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(175, 64);
            this.ultraButton1.TabIndex = 0;
            this.ultraButton1.Text = "試薬確認ダイアログ";
            this.ultraButton1.Click += new System.EventHandler(this.ultraButton1_Click);
            // 
            // ultraButton2
            // 
            this.ultraButton2.Location = new System.Drawing.Point(59, 134);
            this.ultraButton2.Name = "ultraButton2";
            this.ultraButton2.Size = new System.Drawing.Size(175, 64);
            this.ultraButton2.TabIndex = 0;
            this.ultraButton2.Text = "試薬交換ダイアログ";
            this.ultraButton2.Click += new System.EventHandler(this.ultraButton2_Click);
            // 
            // ReagentDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.ultraButton2);
            this.Controls.Add(this.ultraButton1);
            this.Name = "ReagentDialog";
            this.Text = "ReagentDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton ultraButton1;
        private Infragistics.Win.Misc.UltraButton ultraButton2;
    }
}