namespace Oelco.CarisX.GUI
{
    partial class FormSystemUserControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(System.Boolean disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("ToolBar");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Register");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Edit");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("last");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Edit");
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Register");
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete");
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool3 = new Infragistics.Win.UltraWinToolbars.LabelTool("last");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool4 = new Infragistics.Win.UltraWinToolbars.LabelTool("LabelTool1");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            this.tlbCommandBar = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this.pnlCommandBar = new Infragistics.Win.Misc.UltraPanel();
            this._ClientArea_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.pnlSystemUserControl = new System.Windows.Forms.Panel();
            this.gbxRegisterEdit = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblRegisterEdit = new Infragistics.Win.Misc.UltraLabel();
            this.lblAuthorityLevel = new Infragistics.Win.Misc.UltraLabel();
            this.txtUserID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.txtPassword = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblUserID = new Infragistics.Win.Misc.UltraLabel();
            this.cmbAuthorityLevel = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.btnRegister = new Infragistics.Win.Misc.UltraButton();
            this.lblPassword = new Infragistics.Win.Misc.UltraLabel();
            this.lblSelectUserID = new Infragistics.Win.Misc.UltraLabel();
            this.cmbUserID = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            ((System.ComponentModel.ISupportInitialize)(this.tlbCommandBar)).BeginInit();
            this.pnlCommandBar.ClientArea.SuspendLayout();
            this.pnlCommandBar.SuspendLayout();
            this.pnlSystemUserControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbxRegisterEdit)).BeginInit();
            this.gbxRegisterEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAuthorityLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUserID)).BeginInit();
            this.SuspendLayout();
            // 
            // tlbCommandBar
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.FontData.Name = "Arial";
            appearance1.FontData.SizeInPoints = 12.71F;
            this.tlbCommandBar.Appearance = appearance1;
            this.tlbCommandBar.DesignerFlags = 1;
            this.tlbCommandBar.DockWithinContainer = this.pnlCommandBar.ClientArea;
            this.tlbCommandBar.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.Standard;
            this.tlbCommandBar.ImageSizeLarge = new System.Drawing.Size(56, 63);
            this.tlbCommandBar.ImageSizeSmall = new System.Drawing.Size(32, 32);
            this.tlbCommandBar.LockToolbars = true;
            this.tlbCommandBar.ShowFullMenusDelay = 500;
            this.tlbCommandBar.ShowQuickCustomizeButton = false;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            buttonTool2.InstanceProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool2.InstanceProps.IsFirstInGroup = true;
            buttonTool6.InstanceProps.IsFirstInGroup = true;
            labelTool1.InstanceProps.IsFirstInGroup = true;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool2,
            buttonTool6,
            labelTool1});
            ultraToolbar1.Text = "ToolBar";
            this.tlbCommandBar.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            this.tlbCommandBar.ToolbarSettings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.tlbCommandBar.ToolbarSettings.HotTrackAppearance = appearance3;
            this.tlbCommandBar.ToolbarSettings.PaddingBottom = 11;
            this.tlbCommandBar.ToolbarSettings.PaddingLeft = 36;
            this.tlbCommandBar.ToolbarSettings.PaddingTop = 12;
            this.tlbCommandBar.ToolbarSettings.ToolSpacing = 25;
            appearance4.Image = global::Oelco.CarisX.Properties.Resources.Image_Edit;
            buttonTool3.SharedPropsInternal.AppearancesLarge.Appearance = appearance4;
            buttonTool3.SharedPropsInternal.Caption = "Edit";
            buttonTool3.SharedPropsInternal.CustomizerCaption = "Edit";
            appearance5.Image = global::Oelco.CarisX.Properties.Resources.Image_Regist;
            buttonTool4.SharedPropsInternal.AppearancesLarge.Appearance = appearance5;
            buttonTool4.SharedPropsInternal.Caption = "Register";
            buttonTool4.SharedPropsInternal.CustomizerCaption = "Register";
            buttonTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance6.Image = global::Oelco.CarisX.Properties.Resources.Image_Delete;
            buttonTool8.SharedPropsInternal.AppearancesLarge.Appearance = appearance6;
            buttonTool8.SharedPropsInternal.Caption = "Delete";
            buttonTool8.SharedPropsInternal.CustomizerCaption = "Delte";
            buttonTool8.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            labelTool4.SharedPropsInternal.Caption = "LabelTool1";
            this.tlbCommandBar.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool3,
            buttonTool4,
            buttonTool8,
            labelTool3,
            labelTool4});
            this.tlbCommandBar.UseLargeImagesOnToolbar = true;
            // 
            // pnlCommandBar
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_MenuBarBackground;
            this.pnlCommandBar.Appearance = appearance2;
            // 
            // pnlCommandBar.ClientArea
            // 
            this.pnlCommandBar.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Left);
            this.pnlCommandBar.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Right);
            this.pnlCommandBar.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Bottom);
            this.pnlCommandBar.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Top);
            this.pnlCommandBar.Location = new System.Drawing.Point(4, 3);
            this.pnlCommandBar.Name = "pnlCommandBar";
            this.pnlCommandBar.Size = new System.Drawing.Size(1433, 98);
            this.pnlCommandBar.TabIndex = 13;
            // 
            // _ClientArea_Toolbars_Dock_Area_Left
            // 
            this._ClientArea_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.Transparent;
            this._ClientArea_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._ClientArea_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 122);
            this._ClientArea_Toolbars_Dock_Area_Left.Name = "_ClientArea_Toolbars_Dock_Area_Left";
            this._ClientArea_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 0);
            this._ClientArea_Toolbars_Dock_Area_Left.ToolbarsManager = this.tlbCommandBar;
            // 
            // _ClientArea_Toolbars_Dock_Area_Right
            // 
            this._ClientArea_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.Transparent;
            this._ClientArea_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._ClientArea_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1433, 122);
            this._ClientArea_Toolbars_Dock_Area_Right.Name = "_ClientArea_Toolbars_Dock_Area_Right";
            this._ClientArea_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 0);
            this._ClientArea_Toolbars_Dock_Area_Right.ToolbarsManager = this.tlbCommandBar;
            // 
            // _ClientArea_Toolbars_Dock_Area_Bottom
            // 
            this._ClientArea_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.Transparent;
            this._ClientArea_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._ClientArea_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 98);
            this._ClientArea_Toolbars_Dock_Area_Bottom.Name = "_ClientArea_Toolbars_Dock_Area_Bottom";
            this._ClientArea_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1433, 0);
            this._ClientArea_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.tlbCommandBar;
            // 
            // _ClientArea_Toolbars_Dock_Area_Top
            // 
            this._ClientArea_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.Transparent;
            this._ClientArea_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._ClientArea_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ClientArea_Toolbars_Dock_Area_Top.Name = "_ClientArea_Toolbars_Dock_Area_Top";
            this._ClientArea_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1433, 122);
            this._ClientArea_Toolbars_Dock_Area_Top.ToolbarsManager = this.tlbCommandBar;
            // 
            // pnlSystemUserControl
            // 
            this.pnlSystemUserControl.BackColor = System.Drawing.Color.Transparent;
            this.pnlSystemUserControl.BackgroundImage = global::Oelco.CarisX.Properties.Resources.Image_FormSystemUserControlBackGround;
            this.pnlSystemUserControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlSystemUserControl.Controls.Add(this.gbxRegisterEdit);
            this.pnlSystemUserControl.Controls.Add(this.lblSelectUserID);
            this.pnlSystemUserControl.Controls.Add(this.cmbUserID);
            this.pnlSystemUserControl.Cursor = System.Windows.Forms.Cursors.Default;
            this.pnlSystemUserControl.Location = new System.Drawing.Point(-1, 109);
            this.pnlSystemUserControl.Name = "pnlSystemUserControl";
            this.pnlSystemUserControl.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.pnlSystemUserControl.Size = new System.Drawing.Size(1441, 891);
            this.pnlSystemUserControl.TabIndex = 8;
            // 
            // gbxRegisterEdit
            // 
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(233)))), ((int)(((byte)(233)))));
            this.gbxRegisterEdit.Appearance = appearance7;
            this.gbxRegisterEdit.Controls.Add(this.lblRegisterEdit);
            this.gbxRegisterEdit.Controls.Add(this.lblAuthorityLevel);
            this.gbxRegisterEdit.Controls.Add(this.txtUserID);
            this.gbxRegisterEdit.Controls.Add(this.btnCancel);
            this.gbxRegisterEdit.Controls.Add(this.txtPassword);
            this.gbxRegisterEdit.Controls.Add(this.lblUserID);
            this.gbxRegisterEdit.Controls.Add(this.cmbAuthorityLevel);
            this.gbxRegisterEdit.Controls.Add(this.btnRegister);
            this.gbxRegisterEdit.Controls.Add(this.lblPassword);
            this.gbxRegisterEdit.Enabled = false;
            this.gbxRegisterEdit.Location = new System.Drawing.Point(272, 233);
            this.gbxRegisterEdit.Name = "gbxRegisterEdit";
            this.gbxRegisterEdit.Size = new System.Drawing.Size(900, 500);
            this.gbxRegisterEdit.TabIndex = 42;
            // 
            // lblRegisterEdit
            // 
            this.lblRegisterEdit.Font = new System.Drawing.Font("Arial", 14.12F, System.Drawing.FontStyle.Bold);
            this.lblRegisterEdit.Location = new System.Drawing.Point(26, 15);
            this.lblRegisterEdit.Name = "lblRegisterEdit";
            this.lblRegisterEdit.Size = new System.Drawing.Size(160, 26);
            this.lblRegisterEdit.TabIndex = 42;
            this.lblRegisterEdit.Text = "Register/Edit";
            // 
            // lblAuthorityLevel
            // 
            this.lblAuthorityLevel.Font = new System.Drawing.Font("Arial", 14.12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthorityLevel.Location = new System.Drawing.Point(99, 267);
            this.lblAuthorityLevel.Name = "lblAuthorityLevel";
            this.lblAuthorityLevel.Size = new System.Drawing.Size(175, 30);
            this.lblAuthorityLevel.TabIndex = 7;
            this.lblAuthorityLevel.Text = "Authority (Level)";
            // 
            // txtUserID
            // 
            this.txtUserID.AutoSize = false;
            this.txtUserID.Font = new System.Drawing.Font("Arial", 12.71F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtUserID.Location = new System.Drawing.Point(277, 99);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(500, 40);
            this.txtUserID.TabIndex = 3;
            this.txtUserID.ValueChanged += new System.EventHandler(this.txtUserID_ValueChanged);
            // 
            // btnCancel
            // 
            appearance8.BackColor = System.Drawing.Color.Transparent;
            appearance8.BorderColor = System.Drawing.Color.Transparent;
            appearance8.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance8.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_SelectButton;
            this.btnCancel.Appearance = appearance8;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance9.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.HotTrackAppearance = appearance9;
            this.btnCancel.ImageSize = new System.Drawing.Size(0, 0);
            this.btnCancel.Location = new System.Drawing.Point(462, 371);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Drawing.Size(10, 0);
            appearance10.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.PressedAppearance = appearance10;
            this.btnCancel.ShowFocusRect = false;
            this.btnCancel.ShowOutline = false;
            this.btnCancel.Size = new System.Drawing.Size(240, 60);
            this.btnCancel.TabIndex = 36;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.AutoSize = false;
            this.txtPassword.Font = new System.Drawing.Font("Arial", 12.71F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtPassword.Location = new System.Drawing.Point(277, 180);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(500, 40);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.ValueChanged += new System.EventHandler(this.txtPassword_ValueChanged);
            // 
            // lblUserID
            // 
            this.lblUserID.Font = new System.Drawing.Font("Arial", 14.12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserID.Location = new System.Drawing.Point(99, 108);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(175, 30);
            this.lblUserID.TabIndex = 2;
            this.lblUserID.Text = "User ID";
            // 
            // cmbAuthorityLevel
            // 
            this.cmbAuthorityLevel.AutoSize = false;
            this.cmbAuthorityLevel.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbAuthorityLevel.Font = new System.Drawing.Font("Arial", 12.71F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbAuthorityLevel.Location = new System.Drawing.Point(278, 260);
            this.cmbAuthorityLevel.Name = "cmbAuthorityLevel";
            this.cmbAuthorityLevel.Size = new System.Drawing.Size(500, 40);
            this.cmbAuthorityLevel.TabIndex = 6;
            this.cmbAuthorityLevel.ValueChanged += new System.EventHandler(this.cmbAuthorityLevel_ValueChanged);
            // 
            // btnRegister
            // 
            appearance11.BackColor = System.Drawing.Color.Transparent;
            appearance11.BorderColor = System.Drawing.Color.Transparent;
            appearance11.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance11.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_SelectButton;
            this.btnRegister.Appearance = appearance11;
            this.btnRegister.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnRegister.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance12.BorderColor = System.Drawing.Color.Transparent;
            this.btnRegister.HotTrackAppearance = appearance12;
            this.btnRegister.ImageSize = new System.Drawing.Size(0, 0);
            this.btnRegister.Location = new System.Drawing.Point(198, 371);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Padding = new System.Drawing.Size(10, 0);
            appearance13.BorderColor = System.Drawing.Color.Transparent;
            this.btnRegister.PressedAppearance = appearance13;
            this.btnRegister.ShowFocusRect = false;
            this.btnRegister.ShowOutline = false;
            this.btnRegister.Size = new System.Drawing.Size(240, 60);
            this.btnRegister.TabIndex = 35;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // lblPassword
            // 
            this.lblPassword.Font = new System.Drawing.Font("Arial", 14.12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.Location = new System.Drawing.Point(99, 189);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(175, 30);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Password";
            // 
            // lblSelectUserID
            // 
            this.lblSelectUserID.Font = new System.Drawing.Font("Arial", 21.18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectUserID.Location = new System.Drawing.Point(367, 128);
            this.lblSelectUserID.Name = "lblSelectUserID";
            this.lblSelectUserID.Size = new System.Drawing.Size(115, 30);
            this.lblSelectUserID.TabIndex = 41;
            this.lblSelectUserID.Text = "User ID";
            // 
            // cmbUserID
            // 
            this.cmbUserID.AutoSize = false;
            this.cmbUserID.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbUserID.Font = new System.Drawing.Font("Arial", 12.71F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbUserID.Location = new System.Drawing.Point(552, 117);
            this.cmbUserID.Name = "cmbUserID";
            this.cmbUserID.Size = new System.Drawing.Size(498, 50);
            this.cmbUserID.TabIndex = 39;
            // 
            // FormSystemUserControl
            // 
            this.ClientSize = new System.Drawing.Size(1439, 1005);
            this.Controls.Add(this.pnlCommandBar);
            this.Controls.Add(this.pnlSystemUserControl);
            this.Name = "FormSystemUserControl";
            this.Text = "FormSystemUser";
            this.Load += new System.EventHandler(this.FormSystemUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tlbCommandBar)).EndInit();
            this.pnlCommandBar.ClientArea.ResumeLayout(false);
            this.pnlCommandBar.ResumeLayout(false);
            this.pnlSystemUserControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbxRegisterEdit)).EndInit();
            this.gbxRegisterEdit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAuthorityLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUserID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager tlbCommandBar;
        private System.Windows.Forms.Panel pnlSystemUserControl;
        private Infragistics.Win.Misc.UltraLabel lblSelectUserID;
        private Infragistics.Win.Misc.UltraLabel lblAuthorityLevel;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraLabel lblUserID;
        private Infragistics.Win.Misc.UltraButton btnRegister;
        private Infragistics.Win.Misc.UltraLabel lblPassword;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbAuthorityLevel;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPassword;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserID;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbUserID;
        private Infragistics.Win.Misc.UltraPanel pnlCommandBar;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Top;
        private Infragistics.Win.Misc.UltraGroupBox gbxRegisterEdit;
        private Infragistics.Win.Misc.UltraLabel lblRegisterEdit;
    }
}