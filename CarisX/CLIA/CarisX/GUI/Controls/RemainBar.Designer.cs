namespace Oelco.CarisX.GUI.Controls
{
    partial class RemainBar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemainBar));
            this.lblRemain = new Infragistics.Win.Misc.UltraLabel();
            this.prgRemain = new Oelco.Common.GUI.CustomProgressBar();
            this.SuspendLayout();
            // 
            // lblRemain
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Black;
            appearance1.TextHAlignAsString = "Right";
            appearance1.TextVAlignAsString = "Middle";
            this.lblRemain.Appearance = appearance1;
            this.lblRemain.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblRemain.Font = new System.Drawing.Font("Arial", 19F, System.Drawing.FontStyle.Bold);
            this.lblRemain.ImageTransparentColor = System.Drawing.Color.Empty;
            this.lblRemain.Location = new System.Drawing.Point(113, 7);
            this.lblRemain.Name = "lblRemain";
            this.lblRemain.Size = new System.Drawing.Size(82, 30);
            this.lblRemain.TabIndex = 1;
            this.lblRemain.Text = "3000";
            // 
            // prgRemain
            // 
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Indicator_WhiteLarge;
            appearance2.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Tiled;
            this.prgRemain.Appearance = appearance2;
            this.prgRemain.BarType = Oelco.Common.GUI.CustomProgressBar.ProgressBarType.Image;
            this.prgRemain.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.ImageBackground = ((System.Drawing.Image)(resources.GetObject("appearance3.ImageBackground")));
            appearance3.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Tiled;
            this.prgRemain.FillAppearance = appearance3;
            this.prgRemain.Location = new System.Drawing.Point(4, 11);
            this.prgRemain.Maximum = 960;
            this.prgRemain.Name = "prgRemain";
            this.prgRemain.Size = new System.Drawing.Size(130, 22);
            this.prgRemain.Style = Infragistics.Win.UltraWinProgressBar.ProgressBarStyle.Continuous;
            this.prgRemain.TabIndex = 7;
            this.prgRemain.Text = "[Range]";
            this.prgRemain.TextVisible = false;
            this.prgRemain.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // RemainBar
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblRemain);
            this.Controls.Add(this.prgRemain);
            this.Name = "RemainBar";
            this.Size = new System.Drawing.Size(200, 44);
            this.ResumeLayout(false);

        }

        #endregion
        private Infragistics.Win.Misc.UltraLabel lblRemain;
        private Oelco.Common.GUI.CustomProgressBar prgRemain;
    }
}
