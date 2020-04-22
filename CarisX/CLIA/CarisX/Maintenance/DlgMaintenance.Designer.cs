namespace Oelco.CarisX.Maintenance
{
    partial class DlgMaintenance
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.lbMsg = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraGroupBox2
            // 
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance8.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            this.ultraGroupBox2.Appearance = appearance8;
            appearance9.BorderColor = System.Drawing.Color.LightSlateGray;
            this.ultraGroupBox2.ContentAreaAppearance = appearance9;
            this.ultraGroupBox2.Controls.Add(this.lbMsg);
            this.ultraGroupBox2.Controls.Add(this.btCancel);
            this.ultraGroupBox2.Controls.Add(this.btOK);
            this.ultraGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox2.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(537, 210);
            this.ultraGroupBox2.TabIndex = 25;
            this.ultraGroupBox2.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // lbMsg
            // 
            this.lbMsg.BackColor = System.Drawing.Color.Transparent;
            this.lbMsg.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMsg.Location = new System.Drawing.Point(80, 83);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(371, 75);
            this.lbMsg.TabIndex = 2;
            this.lbMsg.Text = "Is it OK to Save?";
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(287, 12);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(109, 32);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btOK
            // 
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(412, 12);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(109, 32);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            // 
            // DlgMaintenance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 210);
            this.Controls.Add(this.ultraGroupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DlgMaintenance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private System.Windows.Forms.Label lbMsg;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOK;

    }
}