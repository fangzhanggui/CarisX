namespace Oelco.CarisX.GUI
{
    partial class DlgErrorCodeMessage
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.pnlErrorStatus = new Infragistics.Win.Misc.UltraPanel();
            this.lblModuleNo = new Infragistics.Win.Misc.UltraLabel();
            this.lblErrorContents = new Infragistics.Win.Misc.UltraLabel();
            this.lblErrorCodeNo = new Infragistics.Win.Misc.UltraLabel();
            this.lblErrorCode = new Infragistics.Win.Misc.UltraLabel();
            this.pnlErrorDetail = new Infragistics.Win.Misc.UltraPanel();
            this.pnlErrorExplanation = new Infragistics.Win.Misc.UltraPanel();
            this.lblErrorExplanation = new Infragistics.Win.Misc.UltraLabel();
            this.lblErrorExplanationTitle = new Infragistics.Win.Misc.UltraLabel();
            this.pnlErrorPicture = new Infragistics.Win.Misc.UltraPanel();
            this.pbxErrorPicture = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.lblErrorPictureTitle = new Infragistics.Win.Misc.UltraLabel();
            this.btnMute = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnClose = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            this.pnlErrorStatus.ClientArea.SuspendLayout();
            this.pnlErrorStatus.SuspendLayout();
            this.pnlErrorDetail.ClientArea.SuspendLayout();
            this.pnlErrorDetail.SuspendLayout();
            this.pnlErrorExplanation.ClientArea.SuspendLayout();
            this.pnlErrorExplanation.SuspendLayout();
            this.pnlErrorPicture.ClientArea.SuspendLayout();
            this.pnlErrorPicture.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnMute);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnClose);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 747);
            this.pnlDialogButton.Size = new System.Drawing.Size(944, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.pnlErrorStatus);
            this.pnlDialogMain.ClientArea.Controls.Add(this.pnlErrorDetail);
            this.pnlDialogMain.Size = new System.Drawing.Size(944, 747);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(944, 28);
            // 
            // pnlErrorStatus
            // 
            appearance7.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.pnlErrorStatus.Appearance = appearance7;
            this.pnlErrorStatus.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            // 
            // pnlErrorStatus.ClientArea
            // 
            this.pnlErrorStatus.ClientArea.Controls.Add(this.lblModuleNo);
            this.pnlErrorStatus.ClientArea.Controls.Add(this.lblErrorContents);
            this.pnlErrorStatus.ClientArea.Controls.Add(this.lblErrorCodeNo);
            this.pnlErrorStatus.ClientArea.Controls.Add(this.lblErrorCode);
            this.pnlErrorStatus.Location = new System.Drawing.Point(25, 43);
            this.pnlErrorStatus.Name = "pnlErrorStatus";
            this.pnlErrorStatus.Size = new System.Drawing.Size(894, 217);
            this.pnlErrorStatus.TabIndex = 1;
            // 
            // lblModuleNo
            // 
            appearance8.TextVAlignAsString = "Middle";
            this.lblModuleNo.Appearance = appearance8;
            this.lblModuleNo.Font = new System.Drawing.Font("Arial", 20F);
            this.lblModuleNo.Location = new System.Drawing.Point(10, 66);
            this.lblModuleNo.Name = "lblModuleNo";
            this.lblModuleNo.Size = new System.Drawing.Size(871, 35);
            this.lblModuleNo.TabIndex = 2;
            this.lblModuleNo.Text = "ModuleNo";
            // 
            // lblErrorContents
            // 
            appearance9.BorderColor = System.Drawing.Color.Black;
            this.lblErrorContents.Appearance = appearance9;
            this.lblErrorContents.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorContents.Location = new System.Drawing.Point(10, 118);
            this.lblErrorContents.Name = "lblErrorContents";
            this.lblErrorContents.Size = new System.Drawing.Size(871, 87);
            this.lblErrorContents.TabIndex = 1;
            this.lblErrorContents.Text = "Detail";
            // 
            // lblErrorCodeNo
            // 
            appearance10.TextVAlignAsString = "Middle";
            this.lblErrorCodeNo.Appearance = appearance10;
            this.lblErrorCodeNo.Font = new System.Drawing.Font("Arial", 20F);
            this.lblErrorCodeNo.Location = new System.Drawing.Point(123, 25);
            this.lblErrorCodeNo.Name = "lblErrorCodeNo";
            this.lblErrorCodeNo.Size = new System.Drawing.Size(758, 35);
            this.lblErrorCodeNo.TabIndex = 1;
            this.lblErrorCodeNo.Text = "Arg";
            // 
            // lblErrorCode
            // 
            appearance11.TextVAlignAsString = "Middle";
            this.lblErrorCode.Appearance = appearance11;
            this.lblErrorCode.Font = new System.Drawing.Font("Arial", 20F);
            this.lblErrorCode.Location = new System.Drawing.Point(10, 25);
            this.lblErrorCode.Name = "lblErrorCode";
            this.lblErrorCode.Size = new System.Drawing.Size(100, 35);
            this.lblErrorCode.TabIndex = 0;
            this.lblErrorCode.Text = "Code";
            // 
            // pnlErrorDetail
            // 
            // 
            // pnlErrorDetail.ClientArea
            // 
            this.pnlErrorDetail.ClientArea.Controls.Add(this.pnlErrorExplanation);
            this.pnlErrorDetail.ClientArea.Controls.Add(this.pnlErrorPicture);
            this.pnlErrorDetail.Location = new System.Drawing.Point(25, 266);
            this.pnlErrorDetail.Name = "pnlErrorDetail";
            this.pnlErrorDetail.Size = new System.Drawing.Size(894, 470);
            this.pnlErrorDetail.TabIndex = 2;
            // 
            // pnlErrorExplanation
            // 
            // 
            // pnlErrorExplanation.ClientArea
            // 
            this.pnlErrorExplanation.ClientArea.Controls.Add(this.lblErrorExplanation);
            this.pnlErrorExplanation.ClientArea.Controls.Add(this.lblErrorExplanationTitle);
            this.pnlErrorExplanation.Location = new System.Drawing.Point(449, 3);
            this.pnlErrorExplanation.Name = "pnlErrorExplanation";
            this.pnlErrorExplanation.Size = new System.Drawing.Size(442, 463);
            this.pnlErrorExplanation.TabIndex = 1;
            // 
            // lblErrorExplanation
            // 
            appearance12.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            appearance12.BorderColor = System.Drawing.Color.Black;
            this.lblErrorExplanation.Appearance = appearance12;
            this.lblErrorExplanation.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblErrorExplanation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblErrorExplanation.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorExplanation.Location = new System.Drawing.Point(0, 23);
            this.lblErrorExplanation.Name = "lblErrorExplanation";
            this.lblErrorExplanation.Size = new System.Drawing.Size(442, 440);
            this.lblErrorExplanation.TabIndex = 1;
            this.lblErrorExplanation.Text = "ultraLabel3";
            // 
            // lblErrorExplanationTitle
            // 
            appearance13.BorderColor = System.Drawing.Color.Black;
            appearance13.TextHAlignAsString = "Center";
            appearance13.TextVAlignAsString = "Middle";
            this.lblErrorExplanationTitle.Appearance = appearance13;
            this.lblErrorExplanationTitle.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblErrorExplanationTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblErrorExplanationTitle.Location = new System.Drawing.Point(0, 0);
            this.lblErrorExplanationTitle.Name = "lblErrorExplanationTitle";
            this.lblErrorExplanationTitle.Size = new System.Drawing.Size(442, 23);
            this.lblErrorExplanationTitle.TabIndex = 0;
            this.lblErrorExplanationTitle.Text = "ultraLabel2";
            // 
            // pnlErrorPicture
            // 
            appearance14.BorderColor = System.Drawing.Color.Black;
            this.pnlErrorPicture.Appearance = appearance14;
            // 
            // pnlErrorPicture.ClientArea
            // 
            this.pnlErrorPicture.ClientArea.Controls.Add(this.pbxErrorPicture);
            this.pnlErrorPicture.ClientArea.Controls.Add(this.lblErrorPictureTitle);
            this.pnlErrorPicture.Location = new System.Drawing.Point(3, 3);
            this.pnlErrorPicture.Name = "pnlErrorPicture";
            this.pnlErrorPicture.Size = new System.Drawing.Size(440, 463);
            this.pnlErrorPicture.TabIndex = 0;
            // 
            // pbxErrorPicture
            // 
            this.pbxErrorPicture.BorderShadowColor = System.Drawing.Color.Empty;
            this.pbxErrorPicture.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.pbxErrorPicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbxErrorPicture.Location = new System.Drawing.Point(0, 23);
            this.pbxErrorPicture.MaintainAspectRatio = false;
            this.pbxErrorPicture.Name = "pbxErrorPicture";
            this.pbxErrorPicture.Size = new System.Drawing.Size(440, 440);
            this.pbxErrorPicture.TabIndex = 1;
            // 
            // lblErrorPictureTitle
            // 
            appearance15.BorderColor = System.Drawing.Color.Black;
            appearance15.TextHAlignAsString = "Center";
            appearance15.TextVAlignAsString = "Middle";
            this.lblErrorPictureTitle.Appearance = appearance15;
            this.lblErrorPictureTitle.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblErrorPictureTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblErrorPictureTitle.Location = new System.Drawing.Point(0, 0);
            this.lblErrorPictureTitle.Name = "lblErrorPictureTitle";
            this.lblErrorPictureTitle.Size = new System.Drawing.Size(440, 23);
            this.lblErrorPictureTitle.TabIndex = 0;
            this.lblErrorPictureTitle.Text = "ultraLabel1";
            // 
            // btnMute
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance1.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnMute.Appearance = appearance1;
            this.btnMute.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnMute.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance2.BorderColor = System.Drawing.Color.Transparent;
            this.btnMute.HotTrackAppearance = appearance2;
            this.btnMute.ImageSize = new System.Drawing.Size(0, 0);
            this.btnMute.Location = new System.Drawing.Point(609, 9);
            this.btnMute.Name = "btnMute";
            this.btnMute.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnMute.PressedAppearance = appearance3;
            this.btnMute.ShowFocusRect = false;
            this.btnMute.ShowOutline = false;
            this.btnMute.Size = new System.Drawing.Size(152, 39);
            this.btnMute.TabIndex = 0;
            this.btnMute.Text = "Mute";
            this.btnMute.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnMute.Click += new System.EventHandler(this.btnMute_Click);
            // 
            // btnClose
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BorderColor = System.Drawing.Color.Transparent;
            appearance4.Image = global::Oelco.CarisX.Properties.Resources.Image_Exit;
            appearance4.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnClose.Appearance = appearance4;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnClose.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance5.BorderColor = System.Drawing.Color.Transparent;
            this.btnClose.HotTrackAppearance = appearance5;
            this.btnClose.ImageSize = new System.Drawing.Size(0, 0);
            this.btnClose.Location = new System.Drawing.Point(767, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Padding = new System.Drawing.Size(10, 0);
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            this.btnClose.PressedAppearance = appearance6;
            this.btnClose.ShowFocusRect = false;
            this.btnClose.ShowOutline = false;
            this.btnClose.Size = new System.Drawing.Size(152, 39);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // DlgErrorCodeMessage
            // 
            this.ClientSize = new System.Drawing.Size(944, 804);
            this.Name = "DlgErrorCodeMessage";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            this.pnlErrorStatus.ClientArea.ResumeLayout(false);
            this.pnlErrorStatus.ResumeLayout(false);
            this.pnlErrorDetail.ClientArea.ResumeLayout(false);
            this.pnlErrorDetail.ResumeLayout(false);
            this.pnlErrorExplanation.ClientArea.ResumeLayout(false);
            this.pnlErrorExplanation.ResumeLayout(false);
            this.pnlErrorPicture.ClientArea.ResumeLayout(false);
            this.pnlErrorPicture.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.NoBorderButton btnClose;
        private Controls.NoBorderButton btnMute;
        private Infragistics.Win.Misc.UltraPanel pnlErrorStatus;
        private Infragistics.Win.Misc.UltraLabel lblErrorContents;
        private Infragistics.Win.Misc.UltraPanel pnlErrorDetail;
        private Infragistics.Win.Misc.UltraPanel pnlErrorPicture;
        private Infragistics.Win.Misc.UltraPanel pnlErrorExplanation;
        private Infragistics.Win.Misc.UltraLabel lblErrorExplanation;
        private Infragistics.Win.Misc.UltraLabel lblErrorExplanationTitle;
        private Infragistics.Win.Misc.UltraLabel lblErrorPictureTitle;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox pbxErrorPicture;
        private Infragistics.Win.Misc.UltraLabel lblErrorCodeNo;
        private Infragistics.Win.Misc.UltraLabel lblErrorCode;
        private Infragistics.Win.Misc.UltraLabel lblModuleNo;
    }
}
