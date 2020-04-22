namespace Oelco.CarisX.GUI.Controls
{
    partial class SearchLogInfoPanelBase
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose( bool disposing )
        {
            if (disposing && ( components != null ))
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
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            this.btnOk = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnCancel = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnClose = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.chkUserID = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.btnWriteTimeTo = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.btnWriteTimeFrom = new Oelco.CarisX.GUI.Controls.NoBorderButton();
            this.lblHyphen1 = new Infragistics.Win.Misc.UltraLabel();
            this.chkWriteTime = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.txtUserID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            ((System.ComponentModel.ISupportInitialize)(this.chkUserID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkWriteTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance1.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnOk.Appearance = appearance1;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnOk.Font = new System.Drawing.Font("Arial", 12F);
            appearance2.BorderColor = System.Drawing.Color.Transparent;
            this.btnOk.HotTrackAppearance = appearance2;
            this.btnOk.ImageSize = new System.Drawing.Size(0, 0);
            this.btnOk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOk.Location = new System.Drawing.Point(930, 35);
            this.btnOk.Name = "btnOk";
            this.btnOk.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnOk.PressedAppearance = appearance3;
            this.btnOk.ShowFocusRect = false;
            this.btnOk.ShowOutline = false;
            this.btnOk.Size = new System.Drawing.Size(180, 40);
            this.btnOk.TabIndex = 29;
            this.btnOk.Text = "OK";
            this.btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BorderColor = System.Drawing.Color.Transparent;
            appearance4.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance4.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnCancel.Appearance = appearance4;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 12F);
            appearance5.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.HotTrackAppearance = appearance5;
            this.btnCancel.ImageSize = new System.Drawing.Size(0, 0);
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(1130, 35);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Drawing.Size(10, 0);
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.PressedAppearance = appearance6;
            this.btnCancel.ShowFocusRect = false;
            this.btnCancel.ShowOutline = false;
            this.btnCancel.Size = new System.Drawing.Size(180, 40);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClose
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            appearance7.BorderColor = System.Drawing.Color.Transparent;
            appearance7.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_ExitLarge;
            this.btnClose.Appearance = appearance7;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnClose.Font = new System.Drawing.Font("Arial", 12F);
            appearance8.BorderColor = System.Drawing.Color.Transparent;
            this.btnClose.HotTrackAppearance = appearance8;
            this.btnClose.ImageSize = new System.Drawing.Size(0, 0);
            this.btnClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClose.Location = new System.Drawing.Point(1355, 30);
            this.btnClose.Name = "btnClose";
            this.btnClose.Padding = new System.Drawing.Size(10, 0);
            appearance9.BorderColor = System.Drawing.Color.Transparent;
            this.btnClose.PressedAppearance = appearance9;
            this.btnClose.ShowFocusRect = false;
            this.btnClose.ShowOutline = false;
            this.btnClose.Size = new System.Drawing.Size(30, 30);
            this.btnClose.TabIndex = 31;
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkUserID
            // 
            appearance10.BackColor = System.Drawing.Color.Transparent;
            appearance10.FontData.Name = "Arial";
            appearance10.FontData.SizeInPoints = 12.71F;
            appearance10.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckOFF;
            this.chkUserID.Appearance = appearance10;
            this.chkUserID.BackColor = System.Drawing.Color.Transparent;
            this.chkUserID.BackColorInternal = System.Drawing.Color.Transparent;
            appearance11.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckON;
            this.chkUserID.CheckedAppearance = appearance11;
            this.chkUserID.Location = new System.Drawing.Point(85, 106);
            this.chkUserID.Name = "chkUserID";
            this.chkUserID.Size = new System.Drawing.Size(120, 28);
            this.chkUserID.Style = Infragistics.Win.EditCheckStyle.Custom;
            this.chkUserID.TabIndex = 32;
            this.chkUserID.Text = "User ID";
            this.chkUserID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnWriteTimeTo
            // 
            appearance12.BackColor = System.Drawing.Color.Transparent;
            appearance12.BorderColor = System.Drawing.Color.Transparent;
            appearance12.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_ButtonLong;
            this.btnWriteTimeTo.Appearance = appearance12;
            this.btnWriteTimeTo.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnWriteTimeTo.Font = new System.Drawing.Font("Arial", 14.12F);
            appearance13.BorderColor = System.Drawing.Color.Transparent;
            this.btnWriteTimeTo.HotTrackAppearance = appearance13;
            this.btnWriteTimeTo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnWriteTimeTo.Location = new System.Drawing.Point(1130, 99);
            this.btnWriteTimeTo.Name = "btnWriteTimeTo";
            appearance14.BorderColor = System.Drawing.Color.Transparent;
            this.btnWriteTimeTo.PressedAppearance = appearance14;
            this.btnWriteTimeTo.ShowFocusRect = false;
            this.btnWriteTimeTo.ShowOutline = false;
            this.btnWriteTimeTo.Size = new System.Drawing.Size(240, 40);
            this.btnWriteTimeTo.TabIndex = 33;
            this.btnWriteTimeTo.Text = "10/05/2011";
            this.btnWriteTimeTo.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnWriteTimeTo.Click += new System.EventHandler(this.btnWriteTimeTo_Click);
            // 
            // btnWriteTimeFrom
            // 
            appearance15.BackColor = System.Drawing.Color.Transparent;
            appearance15.BorderColor = System.Drawing.Color.Transparent;
            appearance15.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_ButtonLong;
            this.btnWriteTimeFrom.Appearance = appearance15;
            this.btnWriteTimeFrom.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnWriteTimeFrom.Font = new System.Drawing.Font("Arial", 14.12F);
            appearance16.BorderColor = System.Drawing.Color.Transparent;
            this.btnWriteTimeFrom.HotTrackAppearance = appearance16;
            this.btnWriteTimeFrom.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnWriteTimeFrom.Location = new System.Drawing.Point(867, 99);
            this.btnWriteTimeFrom.Name = "btnWriteTimeFrom";
            appearance17.BorderColor = System.Drawing.Color.Transparent;
            this.btnWriteTimeFrom.PressedAppearance = appearance17;
            this.btnWriteTimeFrom.ShowFocusRect = false;
            this.btnWriteTimeFrom.ShowOutline = false;
            this.btnWriteTimeFrom.Size = new System.Drawing.Size(240, 40);
            this.btnWriteTimeFrom.TabIndex = 34;
            this.btnWriteTimeFrom.Text = "10/05/2011";
            this.btnWriteTimeFrom.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnWriteTimeFrom.Click += new System.EventHandler(this.btnWriteTimeFrom_Click);
            // 
            // lblHyphen1
            // 
            appearance18.BackColor = System.Drawing.Color.Transparent;
            appearance18.TextVAlignAsString = "Middle";
            this.lblHyphen1.Appearance = appearance18;
            this.lblHyphen1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblHyphen1.Location = new System.Drawing.Point(1114, 108);
            this.lblHyphen1.Name = "lblHyphen1";
            this.lblHyphen1.Size = new System.Drawing.Size(11, 23);
            this.lblHyphen1.TabIndex = 35;
            this.lblHyphen1.Text = "-";
            // 
            // chkWriteTime
            // 
            appearance19.BackColor = System.Drawing.Color.Transparent;
            appearance19.FontData.Name = "Arial";
            appearance19.FontData.SizeInPoints = 12.71F;
            appearance19.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckOFF;
            this.chkWriteTime.Appearance = appearance19;
            this.chkWriteTime.BackColor = System.Drawing.Color.Transparent;
            this.chkWriteTime.BackColorInternal = System.Drawing.Color.Transparent;
            appearance20.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckON;
            this.chkWriteTime.CheckedAppearance = appearance20;
            this.chkWriteTime.Location = new System.Drawing.Point(712, 106);
            this.chkWriteTime.Name = "chkWriteTime";
            this.chkWriteTime.Size = new System.Drawing.Size(130, 28);
            this.chkWriteTime.Style = Infragistics.Win.EditCheckStyle.Custom;
            this.chkWriteTime.TabIndex = 36;
            this.chkWriteTime.Text = "WriteTiem";
            this.chkWriteTime.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtUserID
            // 
            this.txtUserID.AutoSize = false;
            this.txtUserID.Font = new System.Drawing.Font("Arial", 12F);
            this.txtUserID.Location = new System.Drawing.Point(224, 100);
            this.txtUserID.MaxLength = 16;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(200, 40);
            this.txtUserID.TabIndex = 37;
            // 
            // SearchLogInfoPanelBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Oelco.CarisX.Properties.Resources.Image_AnalyteTableBackground;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.chkWriteTime);
            this.Controls.Add(this.lblHyphen1);
            this.Controls.Add(this.btnWriteTimeFrom);
            this.Controls.Add(this.btnWriteTimeTo);
            this.Controls.Add(this.chkUserID);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Name = "SearchLogInfoPanelBase";
            this.Size = new System.Drawing.Size(1415, 454);
            ((System.ComponentModel.ISupportInitialize)(this.chkUserID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkWriteTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private NoBorderButton btnOk;
        private NoBorderButton btnCancel;
        private NoBorderButton btnClose;
        protected Infragistics.Win.UltraWinEditors.UltraCheckEditor chkUserID;
        protected Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserID;
        public NoBorderButton btnWriteTimeTo;
        public NoBorderButton btnWriteTimeFrom;
        public Infragistics.Win.Misc.UltraLabel lblHyphen1;
        public Infragistics.Win.UltraWinEditors.UltraCheckEditor chkWriteTime;
    }
}
