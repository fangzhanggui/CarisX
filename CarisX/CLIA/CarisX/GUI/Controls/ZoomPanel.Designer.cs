namespace Oelco.CarisX.GUI.Controls
{
    partial class ZoomPanel
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( System.Boolean disposing )
        {
            if (disposing && ( components != null ))
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.btnZoomOut = new Controls.NoBorderButton();
            this.btnZoomIn = new Controls.NoBorderButton();
            this.lblZoomPercentUnit = new Infragistics.Win.Misc.UltraLabel();
            this.pbxZoomIcon = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.lblZoomPercent = new Infragistics.Win.Misc.UltraLabel();
            this.SuspendLayout();
            // 
            // btnZoomOut
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Zoom_M;
            this.btnZoomOut.Appearance = appearance1;
            this.btnZoomOut.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            appearance2.BorderColor = System.Drawing.Color.Transparent;
            this.btnZoomOut.HotTrackAppearance = appearance2;
            this.btnZoomOut.Location = new System.Drawing.Point(164, 0);
            this.btnZoomOut.Margin = new System.Windows.Forms.Padding(0);
            this.btnZoomOut.Name = "btnZoomOut";
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnZoomOut.PressedAppearance = appearance3;
            this.btnZoomOut.ShowFocusRect = false;
            this.btnZoomOut.ShowOutline = false;
            this.btnZoomOut.Size = new System.Drawing.Size(66, 40);
            this.btnZoomOut.TabIndex = 74;
            this.btnZoomOut.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnZoomIn
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BorderColor = System.Drawing.Color.Transparent;
            appearance4.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Zoom_P;
            this.btnZoomIn.Appearance = appearance4;
            this.btnZoomIn.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            appearance5.BorderColor = System.Drawing.Color.Transparent;
            this.btnZoomIn.HotTrackAppearance = appearance5;
            this.btnZoomIn.ImageTransparentColor = System.Drawing.Color.Empty;
            this.btnZoomIn.Location = new System.Drawing.Point(100, 0);
            this.btnZoomIn.Margin = new System.Windows.Forms.Padding(0);
            this.btnZoomIn.Name = "btnZoomIn";
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            this.btnZoomIn.PressedAppearance = appearance6;
            this.btnZoomIn.ShowFocusRect = false;
            this.btnZoomIn.ShowOutline = false;
            this.btnZoomIn.Size = new System.Drawing.Size(66, 40);
            this.btnZoomIn.TabIndex = 73;
            this.btnZoomIn.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // lblZoomPercentUnit
            // 
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.TextHAlignAsString = "Left";
            this.lblZoomPercentUnit.Appearance = appearance7;
            this.lblZoomPercentUnit.Font = new System.Drawing.Font("Arial", 11.29F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZoomPercentUnit.ImageTransparentColor = System.Drawing.Color.Empty;
            this.lblZoomPercentUnit.Location = new System.Drawing.Point(71, 10);
            this.lblZoomPercentUnit.Margin = new System.Windows.Forms.Padding(0);
            this.lblZoomPercentUnit.Name = "lblZoomPercentUnit";
            this.lblZoomPercentUnit.Size = new System.Drawing.Size(20, 20);
            this.lblZoomPercentUnit.TabIndex = 72;
            this.lblZoomPercentUnit.Text = "%";
            // 
            // pbxZoomIcon
            // 
            this.pbxZoomIcon.AutoSize = true;
            this.pbxZoomIcon.BackColor = System.Drawing.Color.Transparent;
            this.pbxZoomIcon.BorderShadowColor = System.Drawing.Color.Empty;
            this.pbxZoomIcon.BorderShadowDepth = ((byte)(0));
            this.pbxZoomIcon.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.pbxZoomIcon.Image = global::Oelco.CarisX.Properties.Resources.Image_Zoom;
            this.pbxZoomIcon.Location = new System.Drawing.Point(0, 0);
            this.pbxZoomIcon.Margin = new System.Windows.Forms.Padding(0);
            this.pbxZoomIcon.Name = "pbxZoomIcon";
            this.pbxZoomIcon.Size = new System.Drawing.Size(100, 40);
            this.pbxZoomIcon.TabIndex = 71;
            // 
            // lblZoomPercent
            // 
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.TextHAlignAsString = "Right";
            this.lblZoomPercent.Appearance = appearance8;
            this.lblZoomPercent.Font = new System.Drawing.Font("Arial", 11.29F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZoomPercent.ImageTransparentColor = System.Drawing.Color.Empty;
            this.lblZoomPercent.Location = new System.Drawing.Point(38, 10);
            this.lblZoomPercent.Margin = new System.Windows.Forms.Padding(0);
            this.lblZoomPercent.Name = "lblZoomPercent";
            this.lblZoomPercent.Size = new System.Drawing.Size(35, 20);
            this.lblZoomPercent.TabIndex = 76;
            this.lblZoomPercent.Text = "100";
            this.lblZoomPercent.TextChanged += new System.EventHandler(this.lblZoomPercent_TextChanged);
            // 
            // ZoomPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblZoomPercent);
            this.Controls.Add(this.btnZoomOut);
            this.Controls.Add(this.btnZoomIn);
            this.Controls.Add(this.lblZoomPercentUnit);
            this.Controls.Add(this.pbxZoomIcon);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ZoomPanel";
            this.Size = new System.Drawing.Size(230, 40);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Controls.NoBorderButton btnZoomOut;
        private Controls.NoBorderButton btnZoomIn;
        private Infragistics.Win.Misc.UltraLabel lblZoomPercentUnit;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox pbxZoomIcon;
        private Infragistics.Win.Misc.UltraLabel lblZoomPercent;
    }
}
