namespace Oelco.CarisX.GUI.Controls
{
    partial class TipCellView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TipCellView));
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            this.lblRemainUnit = new Infragistics.Win.Misc.UltraLabel();
            this.lblRemain = new Infragistics.Win.Misc.UltraLabel();
            this.lblReagentName = new Infragistics.Win.Misc.UltraLabel();
            this.prgRemain = new Oelco.Common.GUI.CustomProgressBar();
            this.lblUse = new Infragistics.Win.Misc.UltraLabel();
            this.pbxUseTipCell = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.SuspendLayout();
            // 
            // lblRemainUnit
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Black;
            this.lblRemainUnit.Appearance = appearance1;
            this.lblRemainUnit.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblRemainUnit.Font = new System.Drawing.Font("Arial", 12.71F);
            this.lblRemainUnit.ImageTransparentColor = System.Drawing.Color.Empty;
            this.lblRemainUnit.Location = new System.Drawing.Point(285, 28);
            this.lblRemainUnit.Name = "lblRemainUnit";
            this.lblRemainUnit.Size = new System.Drawing.Size(50, 22);
            this.lblRemainUnit.TabIndex = 2;
            this.lblRemainUnit.Text = "test";
            // 
            // lblRemain
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.BorderColor = System.Drawing.Color.Black;
            appearance2.TextHAlignAsString = "Right";
            this.lblRemain.Appearance = appearance2;
            this.lblRemain.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblRemain.Font = new System.Drawing.Font("Arial", 24.71F, System.Drawing.FontStyle.Bold);
            this.lblRemain.ImageTransparentColor = System.Drawing.Color.Empty;
            this.lblRemain.Location = new System.Drawing.Point(200, 12);
            this.lblRemain.Name = "lblRemain";
            this.lblRemain.Size = new System.Drawing.Size(87, 38);
            this.lblRemain.TabIndex = 1;
            this.lblRemain.Text = "3000";
            // 
            // lblReagentName
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.BorderColor = System.Drawing.Color.Black;
            this.lblReagentName.Appearance = appearance3;
            this.lblReagentName.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblReagentName.Font = new System.Drawing.Font("Arial", 12.71F);
            this.lblReagentName.ImageTransparentColor = System.Drawing.Color.Empty;
            this.lblReagentName.Location = new System.Drawing.Point(28, 28);
            this.lblReagentName.Name = "lblReagentName";
            this.lblReagentName.Size = new System.Drawing.Size(166, 22);
            this.lblReagentName.TabIndex = 6;
            this.lblReagentName.Text = "test";
            // 
            // prgRemain
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Indicator_WhiteLarge;
            appearance4.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Tiled;
            this.prgRemain.Appearance = appearance4;
            this.prgRemain.BarType = Oelco.Common.GUI.CustomProgressBar.ProgressBarType.Image;
            this.prgRemain.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.ImageBackground = ((System.Drawing.Image)(resources.GetObject("appearance5.ImageBackground")));
            appearance5.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Tiled;
            this.prgRemain.FillAppearance = appearance5;
            this.prgRemain.Location = new System.Drawing.Point(28, 56);
            this.prgRemain.Maximum = 960;
            this.prgRemain.Name = "prgRemain";
            this.prgRemain.Size = new System.Drawing.Size(210, 21);
            this.prgRemain.Style = Infragistics.Win.UltraWinProgressBar.ProgressBarStyle.Continuous;
            this.prgRemain.TabIndex = 7;
            this.prgRemain.Text = "[Range]";
            this.prgRemain.TextVisible = false;
            this.prgRemain.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lblUse
            // 
            appearance6.BackColor = System.Drawing.Color.Transparent;
            appearance6.BorderAlpha = Infragistics.Win.Alpha.UseAlphaLevel;
            appearance6.BorderColor = System.Drawing.Color.Gray;
            appearance6.BorderColor2 = System.Drawing.Color.Transparent;
            appearance6.TextHAlignAsString = "Right";
            this.lblUse.Appearance = appearance6;
            this.lblUse.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblUse.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.None;
            this.lblUse.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblUse.Font = new System.Drawing.Font("Arial", 24.71F, System.Drawing.FontStyle.Bold);
            this.lblUse.ImageTransparentColor = System.Drawing.Color.Empty;
            this.lblUse.Location = new System.Drawing.Point(288, 56);
            this.lblUse.Name = "lblUse";
            this.lblUse.Padding = new System.Drawing.Size(10, 2);
            this.lblUse.Size = new System.Drawing.Size(53, 43);
            this.lblUse.TabIndex = 8;
            this.lblUse.Text = "8";
            this.lblUse.UseAppStyling = false;
            // 
            // pbxUseTipCell
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            appearance7.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_UseBottleMark;
            appearance7.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.pbxUseTipCell.Appearance = appearance7;
            this.pbxUseTipCell.BorderShadowColor = System.Drawing.Color.Empty;
            this.pbxUseTipCell.Location = new System.Drawing.Point(289, 57);
            this.pbxUseTipCell.Name = "pbxUseTipCell";
            this.pbxUseTipCell.Size = new System.Drawing.Size(10, 41);
            this.pbxUseTipCell.TabIndex = 9;
            // 
            // TipCellView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.pbxUseTipCell);
            this.Controls.Add(this.lblUse);
            this.Controls.Add(this.prgRemain);
            this.Controls.Add(this.lblReagentName);
            this.Controls.Add(this.lblRemainUnit);
            this.Controls.Add(this.lblRemain);
            this.Name = "TipCellView";
            this.Size = new System.Drawing.Size(338, 98);
            this.ResumeLayout(false);

        }

        #endregion
        private Infragistics.Win.Misc.UltraLabel lblRemainUnit;
        private Infragistics.Win.Misc.UltraLabel lblRemain;
        private Infragistics.Win.Misc.UltraLabel lblReagentName;
        private Oelco.Common.GUI.CustomProgressBar prgRemain;
        private Infragistics.Win.Misc.UltraLabel lblUse;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox pbxUseTipCell;
    }
}
