namespace Oelco.CarisX.GUI.Controls
{
    partial class NoBorderButton
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
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
            this.SuspendLayout();
            // 
            // NoBorderButton
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            this.Appearance = appearance1;
            appearance2.BorderColor = System.Drawing.Color.Transparent;
            this.HotTrackAppearance = appearance2;
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.PressedAppearance = appearance3;
            this.ShowFocusRect = false;
            this.ShowOutline = false;
            this.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ResumeLayout(false);

        }

        #endregion
    }
}
