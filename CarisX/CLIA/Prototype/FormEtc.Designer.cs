namespace Prototype
{
    partial class FormEtc
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button9 = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button10 = new System.Windows.Forms.Button();
            this.customUButton2 = new Oelco.Common.GUI.CustomUButton();
            this.ultraPictureBox1 = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.button11 = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 18);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(55, 22);
            this.button1.TabIndex = 0;
            this.button1.Text = "確保";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(67, 18);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(55, 22);
            this.button2.TabIndex = 0;
            this.button2.Text = "開放";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(128, 18);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(55, 22);
            this.button3.TabIndex = 0;
            this.button3.Text = "GC";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Location = new System.Drawing.Point(12, 204);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(191, 50);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "~確認";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button6);
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Location = new System.Drawing.Point(10, 149);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(193, 49);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "非同期dlg制御確認";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(116, 18);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(48, 25);
            this.button6.TabIndex = 3;
            this.button6.Text = "終了";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(62, 18);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(48, 25);
            this.button5.TabIndex = 3;
            this.button5.Text = "B開始";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(8, 18);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(48, 25);
            this.button4.TabIndex = 3;
            this.button4.Text = "A開始";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button8);
            this.groupBox3.Controls.Add(this.button7);
            this.groupBox3.Location = new System.Drawing.Point(12, 96);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(191, 47);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "シリアライズ確認";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(97, 18);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(73, 23);
            this.button8.TabIndex = 4;
            this.button8.Text = "Deserialize";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(16, 18);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(73, 23);
            this.button7.TabIndex = 4;
            this.button7.Text = "Serialize";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button9);
            this.groupBox4.Location = new System.Drawing.Point(12, 43);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(191, 47);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Regex確認";
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(97, 18);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(73, 23);
            this.button9.TabIndex = 4;
            this.button9.Text = "Regex";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button10);
            this.groupBox5.Location = new System.Drawing.Point(209, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(191, 47);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Flags確認";
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(97, 18);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(73, 23);
            this.button10.TabIndex = 4;
            this.button10.Text = "Flags";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // customUButton2
            // 
            this.customUButton2.Location = new System.Drawing.Point(559, 61);
            this.customUButton2.Name = "customUButton2";
            this.customUButton2.Size = new System.Drawing.Size(113, 57);
            this.customUButton2.TabIndex = 5;
            this.customUButton2.Text = "customUButton265";
            this.customUButton2.Click += new System.EventHandler(this.customUButton2_Click);
            // 
            // ultraPictureBox1
            // 
            this.ultraPictureBox1.BorderShadowColor = System.Drawing.Color.Empty;
            this.ultraPictureBox1.Location = new System.Drawing.Point(513, 167);
            this.ultraPictureBox1.Name = "ultraPictureBox1";
            this.ultraPictureBox1.Size = new System.Drawing.Size(211, 145);
            this.ultraPictureBox1.TabIndex = 6;
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(16, 345);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(57, 34);
            this.button11.TabIndex = 7;
            this.button11.Text = "中継IF確認";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.button12);
            this.groupBox6.Controls.Add(this.button13);
            this.groupBox6.Controls.Add(this.button14);
            this.groupBox6.Location = new System.Drawing.Point(12, 260);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(193, 49);
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "ManRes確認";
            // 
            // button12
            // 
            this.button12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button12.Location = new System.Drawing.Point(116, 18);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(48, 25);
            this.button12.TabIndex = 3;
            this.button12.Text = "終了";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button13.Location = new System.Drawing.Point(62, 18);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(48, 25);
            this.button13.TabIndex = 3;
            this.button13.Text = "B開始";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button14
            // 
            this.button14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button14.Location = new System.Drawing.Point(8, 18);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(48, 25);
            this.button14.TabIndex = 3;
            this.button14.Text = "A開始";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // FormEtc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 413);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.ultraPictureBox1);
            this.Controls.Add(this.customUButton2);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormEtc";
            this.Text = "etc";
            this.Load += new System.EventHandler(this.FormEtc_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button button10;
        private Oelco.Common.GUI.CustomUButton customUButton2;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox ultraPictureBox1;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
    }
}