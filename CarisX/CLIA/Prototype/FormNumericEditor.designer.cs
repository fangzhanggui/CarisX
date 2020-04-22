namespace Prototype
{
    partial class FormNumericEditor
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
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ultraNumericEditor1 = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.customNumericEditor1 = new Oelco.Common.GUI.CustomNumericEditor();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ultraNumericEditor1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customNumericEditor1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("MS UI Gothic", 14F);
            this.textBox2.Location = new System.Drawing.Point(185, 30);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(127, 26);
            this.textBox2.TabIndex = 4;
            this.textBox2.Leave += new System.EventHandler(this.textBox2_Leave);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("MS UI Gothic", 14F);
            this.textBox1.Location = new System.Drawing.Point(185, 30);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(127, 26);
            this.textBox1.TabIndex = 2;
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // ultraNumericEditor1
            // 
            this.ultraNumericEditor1.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ultraNumericEditor1.Location = new System.Drawing.Point(14, 28);
            this.ultraNumericEditor1.MaskInput = "{double:-5.3:c}";
            this.ultraNumericEditor1.Name = "ultraNumericEditor1";
            this.ultraNumericEditor1.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.ultraNumericEditor1.Size = new System.Drawing.Size(155, 29);
            this.ultraNumericEditor1.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.customNumericEditor1);
            this.groupBox1.Location = new System.Drawing.Point(16, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(327, 66);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "カスタムコントロール";
            // 
            // customNumericEditor1
            // 
            this.customNumericEditor1.Font = new System.Drawing.Font("MS UI Gothic", 14F);
            this.customNumericEditor1.Location = new System.Drawing.Point(14, 28);
            this.customNumericEditor1.MaskInput = "-nnnnn.nnn";
            this.customNumericEditor1.Name = "customNumericEditor1";
            this.customNumericEditor1.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.customNumericEditor1.Size = new System.Drawing.Size(155, 28);
            this.customNumericEditor1.TabIndex = 1;
            this.customNumericEditor1.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.ultraNumericEditor1);
            this.groupBox2.Location = new System.Drawing.Point(16, 102);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(327, 66);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "UltraNumericEditor （派生元）";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(212, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "MaskInputプロパティ";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(201, 179);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 84);
            this.label2.TabIndex = 20;
            this.label2.Text = "※MaskInputプロパティを変更してフォーカスを抜けると、左側のコントロールのMaskInputプロパティを再設定します。（エラーになるとキャンセルします。）" +
    "";
            // 
            // FormNumericEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 257);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormNumericEditor";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.FormUT_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormUT_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormUT_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormUT_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.ultraNumericEditor1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customNumericEditor1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor ultraNumericEditor1;
        private Oelco.Common.GUI.CustomNumericEditor customNumericEditor1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}