namespace Oelco.CarisX.GUI
{
    partial class DlgMessage
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
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.btnButton2 = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnButton1 = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnButton1);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnButton2);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 267);
            this.pnlDialogButton.Size = new System.Drawing.Size(630, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.ultraLabel2);
            this.pnlDialogMain.ClientArea.Controls.Add(this.ultraLabel1);
            this.pnlDialogMain.Size = new System.Drawing.Size(630, 267);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(630, 28);
            this.lblDialogTitle.Text = "Dlg";
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Location = new System.Drawing.Point(25, 67);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(580, 101);
            this.ultraLabel1.TabIndex = 69;
            this.ultraLabel1.Text = "メッセージ";
            // 
            // ultraLabel2
            // 
            appearance7.ForeColor = System.Drawing.Color.Red;
            this.ultraLabel2.Appearance = appearance7;
            this.ultraLabel2.Location = new System.Drawing.Point(25, 174);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(580, 56);
            this.ultraLabel2.TabIndex = 70;
            this.ultraLabel2.Text = "メッセージ";
            // 
            // btnButton2
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BorderColor = System.Drawing.Color.Transparent;
            appearance4.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance4.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnButton2.Appearance = appearance4;
            this.btnButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnButton2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance5.BorderColor = System.Drawing.Color.Transparent;
            this.btnButton2.HotTrackAppearance = appearance5;
            this.btnButton2.ImageSize = new System.Drawing.Size(0, 0);
            this.btnButton2.Location = new System.Drawing.Point(453, 9);
            this.btnButton2.Name = "btnButton2";
            this.btnButton2.Padding = new System.Drawing.Size(10, 0);
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            this.btnButton2.PressedAppearance = appearance6;
            this.btnButton2.ShowFocusRect = false;
            this.btnButton2.ShowOutline = false;
            this.btnButton2.Size = new System.Drawing.Size(152, 39);
            this.btnButton2.TabIndex = 156;
            this.btnButton2.Text = "2";
            this.btnButton2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnButton1
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance1.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnButton1.Appearance = appearance1;
            this.btnButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnButton1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance2.BorderColor = System.Drawing.Color.Transparent;
            this.btnButton1.HotTrackAppearance = appearance2;
            this.btnButton1.ImageSize = new System.Drawing.Size(0, 0);
            this.btnButton1.Location = new System.Drawing.Point(295, 9);
            this.btnButton1.Name = "btnButton1";
            this.btnButton1.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnButton1.PressedAppearance = appearance3;
            this.btnButton1.ShowFocusRect = false;
            this.btnButton1.ShowOutline = false;
            this.btnButton1.Size = new System.Drawing.Size(152, 39);
            this.btnButton1.TabIndex = 156;
            this.btnButton1.Text = "1";
            this.btnButton1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // DlgMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Caption = "Dlg";
            this.ClientSize = new System.Drawing.Size(630, 324);
            this.Name = "DlgMessage";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Controls.NoBorderButton btnButton2;
        private Controls.NoBorderButton btnButton1;
    }
}