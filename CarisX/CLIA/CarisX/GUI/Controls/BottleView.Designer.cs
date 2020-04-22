namespace Oelco.CarisX.GUI.Controls
{
    partial class BottleView
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
            this.pbxUseReagentKind = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.spbReagentKind = new Oelco.Common.GUI.CustomStatePictureBox();
            this.lblRemainUnit = new Infragistics.Win.Misc.UltraLabel();
            this.lblRemain = new Infragistics.Win.Misc.UltraLabel();
            this.lblReagentName = new Infragistics.Win.Misc.UltraLabel();
            this.SuspendLayout();
            // 
            // pbxUseReagentKind
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_UseBottleMark;
            appearance1.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.pbxUseReagentKind.Appearance = appearance1;
            this.pbxUseReagentKind.BorderShadowColor = System.Drawing.Color.Empty;
            this.pbxUseReagentKind.Location = new System.Drawing.Point(-2, 0);
            this.pbxUseReagentKind.Name = "pbxUseReagentKind";
            this.pbxUseReagentKind.Size = new System.Drawing.Size(15, 100);
            this.pbxUseReagentKind.TabIndex = 4;
            // 
            // spbReagentKind
            // 
            this.spbReagentKind.BorderShadowColor = System.Drawing.Color.Empty;
            this.spbReagentKind.ImageList = null;
            this.spbReagentKind.Location = new System.Drawing.Point(138, 20);
            this.spbReagentKind.Name = "spbReagentKind";
            this.spbReagentKind.Size = new System.Drawing.Size(56, 60);
            this.spbReagentKind.TabIndex = 0;
            this.spbReagentKind.ViewIndex = -1;
            // 
            // lblRemainUnit
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.BorderColor = System.Drawing.Color.Black;
            this.lblRemainUnit.Appearance = appearance2;
            this.lblRemainUnit.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblRemainUnit.Font = new System.Drawing.Font("Arial", 12.71F);
            this.lblRemainUnit.ImageTransparentColor = System.Drawing.Color.Empty;
            this.lblRemainUnit.Location = new System.Drawing.Point(285, 39);
            this.lblRemainUnit.Name = "lblRemainUnit";
            this.lblRemainUnit.Size = new System.Drawing.Size(50, 22);
            this.lblRemainUnit.TabIndex = 2;
            this.lblRemainUnit.Text = "test";
            // 
            // lblRemain
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.BorderColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Right";
            this.lblRemain.Appearance = appearance3;
            this.lblRemain.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblRemain.Font = new System.Drawing.Font("Arial", 24.71F, System.Drawing.FontStyle.Bold);
            this.lblRemain.ImageTransparentColor = System.Drawing.Color.Empty;
            this.lblRemain.Location = new System.Drawing.Point(200, 23);
            this.lblRemain.Name = "lblRemain";
            this.lblRemain.Size = new System.Drawing.Size(87, 38);
            this.lblRemain.TabIndex = 1;
            this.lblRemain.Text = "3000";
            // 
            // lblReagentName
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BorderColor = System.Drawing.Color.Black;
            this.lblReagentName.Appearance = appearance4;
            this.lblReagentName.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblReagentName.Font = new System.Drawing.Font("Arial", 12.71F);
            this.lblReagentName.ImageTransparentColor = System.Drawing.Color.Empty;
            this.lblReagentName.Location = new System.Drawing.Point(28, 39);
            this.lblReagentName.Name = "lblReagentName";
            this.lblReagentName.Size = new System.Drawing.Size(104, 22);
            this.lblReagentName.TabIndex = 6;
            this.lblReagentName.Text = "test";
            // 
            // BottleView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblReagentName);
            this.Controls.Add(this.pbxUseReagentKind);
            this.Controls.Add(this.spbReagentKind);
            this.Controls.Add(this.lblRemainUnit);
            this.Controls.Add(this.lblRemain);
            this.Name = "BottleView";
            this.Size = new System.Drawing.Size(338, 98);
            this.ResumeLayout(false);

        }

        #endregion

        private Oelco.Common.GUI.CustomStatePictureBox spbReagentKind;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox pbxUseReagentKind;
        private Infragistics.Win.Misc.UltraLabel lblRemainUnit;
        private Infragistics.Win.Misc.UltraLabel lblRemain;
        private Infragistics.Win.Misc.UltraLabel lblReagentName;
    }
}
