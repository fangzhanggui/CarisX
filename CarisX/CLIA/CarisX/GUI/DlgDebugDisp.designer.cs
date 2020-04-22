namespace Oelco.CarisX.GUI
{
    partial class DlgDebugDisp
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(System.Boolean disposing)
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.btnOK = new Infragistics.Win.Misc.UltraButton();
            this.lvwDialogName = new System.Windows.Forms.ListView();
            this.clmName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnDialogDisp = new Infragistics.Win.Misc.UltraButton();
            this.gbxErrorCodeMessage = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.btnErrorCodeMessageDisp = new Infragistics.Win.Misc.UltraButton();
            this.txtErrorCodeMessageArguments = new System.Windows.Forms.TextBox();
            this.txtErrorCodeMessageCode = new System.Windows.Forms.TextBox();
            this.gbxDialog = new Infragistics.Win.Misc.UltraGroupBox();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxErrorCodeMessage)).BeginInit();
            this.gbxErrorCodeMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxDialog)).BeginInit();
            this.gbxDialog.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOK);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 394);
            this.pnlDialogButton.Size = new System.Drawing.Size(683, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.gbxDialog);
            this.pnlDialogMain.ClientArea.Controls.Add(this.gbxErrorCodeMessage);
            this.pnlDialogMain.Size = new System.Drawing.Size(683, 394);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(683, 28);
            this.lblDialogTitle.Text = "ダイアログデバッグ";
            // 
            // btnOK
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance1.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnOK.Appearance = appearance1;
            this.btnOK.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnOK.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ImageSize = new System.Drawing.Size(0, 0);
            this.btnOK.Location = new System.Drawing.Point(519, 7);
            this.btnOK.Name = "btnOK";
            this.btnOK.Padding = new System.Drawing.Size(10, 0);
            this.btnOK.ShapeImage = global::Oelco.CarisX.Properties.Resources.SharpImage_Button;
            this.btnOK.Size = new System.Drawing.Size(152, 39);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "終了";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lvwDialogName
            // 
            this.lvwDialogName.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmName});
            this.lvwDialogName.HideSelection = false;
            this.lvwDialogName.Location = new System.Drawing.Point(9, 25);
            this.lvwDialogName.MultiSelect = false;
            this.lvwDialogName.Name = "lvwDialogName";
            this.lvwDialogName.Size = new System.Drawing.Size(335, 257);
            this.lvwDialogName.TabIndex = 3;
            this.lvwDialogName.UseCompatibleStateImageBehavior = false;
            this.lvwDialogName.View = System.Windows.Forms.View.Details;
            this.lvwDialogName.SelectedIndexChanged += new System.EventHandler(this.lstvDialogName_SelectedIndexChanged);
            // 
            // clmName
            // 
            this.clmName.Text = "名称";
            this.clmName.Width = 50;
            // 
            // btnDialogDisp
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance2.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnDialogDisp.Appearance = appearance2;
            this.btnDialogDisp.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnDialogDisp.ImageSize = new System.Drawing.Size(0, 0);
            this.btnDialogDisp.Location = new System.Drawing.Point(244, 288);
            this.btnDialogDisp.Name = "btnDialogDisp";
            this.btnDialogDisp.Padding = new System.Drawing.Size(10, 0);
            this.btnDialogDisp.ShapeImage = global::Oelco.CarisX.Properties.Resources.SharpImage_Button;
            this.btnDialogDisp.Size = new System.Drawing.Size(100, 39);
            this.btnDialogDisp.TabIndex = 4;
            this.btnDialogDisp.Text = "表示";
            this.btnDialogDisp.Click += new System.EventHandler(this.btnDialogDisp_Click);
            // 
            // gbxErrorCodeMessage
            // 
            this.gbxErrorCodeMessage.Controls.Add(this.ultraLabel3);
            this.gbxErrorCodeMessage.Controls.Add(this.ultraLabel1);
            this.gbxErrorCodeMessage.Controls.Add(this.btnErrorCodeMessageDisp);
            this.gbxErrorCodeMessage.Controls.Add(this.txtErrorCodeMessageArguments);
            this.gbxErrorCodeMessage.Controls.Add(this.txtErrorCodeMessageCode);
            this.gbxErrorCodeMessage.Location = new System.Drawing.Point(383, 54);
            this.gbxErrorCodeMessage.Name = "gbxErrorCodeMessage";
            this.gbxErrorCodeMessage.Size = new System.Drawing.Size(288, 133);
            this.gbxErrorCodeMessage.TabIndex = 5;
            this.gbxErrorCodeMessage.Text = "DlgErrorCodeMessageを表示";
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Location = new System.Drawing.Point(182, 27);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel3.TabIndex = 1;
            this.ultraLabel3.Text = "引数";
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Location = new System.Drawing.Point(76, 27);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel1.TabIndex = 1;
            this.ultraLabel1.Text = "コード";
            // 
            // btnErrorCodeMessageDisp
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance3.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnErrorCodeMessageDisp.Appearance = appearance3;
            this.btnErrorCodeMessageDisp.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnErrorCodeMessageDisp.ImageSize = new System.Drawing.Size(0, 0);
            this.btnErrorCodeMessageDisp.Location = new System.Drawing.Point(182, 88);
            this.btnErrorCodeMessageDisp.Name = "btnErrorCodeMessageDisp";
            this.btnErrorCodeMessageDisp.Padding = new System.Drawing.Size(10, 0);
            this.btnErrorCodeMessageDisp.ShapeImage = global::Oelco.CarisX.Properties.Resources.SharpImage_Button;
            this.btnErrorCodeMessageDisp.Size = new System.Drawing.Size(100, 39);
            this.btnErrorCodeMessageDisp.TabIndex = 4;
            this.btnErrorCodeMessageDisp.Text = "表示";
            this.btnErrorCodeMessageDisp.Click += new System.EventHandler(this.btnErrorCodeMessageDisp_Click);
            // 
            // txtErrorCodeMessageArguments
            // 
            this.txtErrorCodeMessageArguments.Location = new System.Drawing.Point(182, 56);
            this.txtErrorCodeMessageArguments.Name = "txtErrorCodeMessageArguments";
            this.txtErrorCodeMessageArguments.Size = new System.Drawing.Size(100, 26);
            this.txtErrorCodeMessageArguments.TabIndex = 0;
            // 
            // txtErrorCodeMessageCode
            // 
            this.txtErrorCodeMessageCode.Location = new System.Drawing.Point(76, 56);
            this.txtErrorCodeMessageCode.Name = "txtErrorCodeMessageCode";
            this.txtErrorCodeMessageCode.Size = new System.Drawing.Size(100, 26);
            this.txtErrorCodeMessageCode.TabIndex = 0;
            // 
            // gbxDialog
            // 
            this.gbxDialog.Controls.Add(this.btnDialogDisp);
            this.gbxDialog.Controls.Add(this.lvwDialogName);
            this.gbxDialog.Location = new System.Drawing.Point(27, 54);
            this.gbxDialog.Name = "gbxDialog";
            this.gbxDialog.Size = new System.Drawing.Size(350, 333);
            this.gbxDialog.TabIndex = 6;
            this.gbxDialog.Text = "ダイアログを表示";
            // 
            // DlgDebugDisp
            // 
            this.ClientSize = new System.Drawing.Size(683, 451);
            this.Name = "DlgDebugDisp";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbxErrorCodeMessage)).EndInit();
            this.gbxErrorCodeMessage.ResumeLayout(false);
            this.gbxErrorCodeMessage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxDialog)).EndInit();
            this.gbxDialog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Infragistics.Win.Misc.UltraButton btnOK;
        private System.Windows.Forms.ListView lvwDialogName;
        private System.Windows.Forms.ColumnHeader clmName;
        private Infragistics.Win.Misc.UltraButton btnDialogDisp;
        private Infragistics.Win.Misc.UltraGroupBox gbxErrorCodeMessage;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private System.Windows.Forms.TextBox txtErrorCodeMessageArguments;
        private System.Windows.Forms.TextBox txtErrorCodeMessageCode;
        private Infragistics.Win.Misc.UltraGroupBox gbxDialog;
        private Infragistics.Win.Misc.UltraButton btnErrorCodeMessageDisp;
    }
}
