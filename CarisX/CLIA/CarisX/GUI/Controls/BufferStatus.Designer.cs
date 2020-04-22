namespace Oelco.CarisX.GUI.Controls
{
    partial class BufferStatus
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
            this.pbxBufferIcon = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.SuspendLayout();
            // 
            // pbxBufferIcon
            // 
            this.pbxBufferIcon.BackColor = System.Drawing.Color.DarkGray;
            this.pbxBufferIcon.BorderShadowColor = System.Drawing.Color.Empty;
            this.pbxBufferIcon.DefaultImage = global::Oelco.CarisX.Properties.Resources.Image_TotalBufferStatus_None;
            this.pbxBufferIcon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbxBufferIcon.Location = new System.Drawing.Point(0, 0);
            this.pbxBufferIcon.Name = "pbxBufferIcon";
            this.pbxBufferIcon.Size = new System.Drawing.Size(48, 44);
            this.pbxBufferIcon.TabIndex = 0;
            // 
            // BufferStatus
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pbxBufferIcon);
            this.Name = "BufferStatus";
            this.Size = new System.Drawing.Size(48, 44);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraPictureBox pbxBufferIcon;
    }
}
