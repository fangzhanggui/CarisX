namespace Oelco.CarisX.GUI
{
    partial class DlgDateSelect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( System.Boolean disposing )
        {
            if (disposing && ( components != null ))
            {
                components.Dispose();
            }
            base.Dispose( disposing );
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.MonthOfYear monthOfYear1 = new Infragistics.Win.UltraWinSchedule.MonthOfYear(Infragistics.Win.UltraWinSchedule.YearMonthEnum.January);
            Infragistics.Win.UltraWinSchedule.MonthOfYear monthOfYear2 = new Infragistics.Win.UltraWinSchedule.MonthOfYear(Infragistics.Win.UltraWinSchedule.YearMonthEnum.February);
            Infragistics.Win.UltraWinSchedule.MonthOfYear monthOfYear3 = new Infragistics.Win.UltraWinSchedule.MonthOfYear(Infragistics.Win.UltraWinSchedule.YearMonthEnum.March);
            Infragistics.Win.UltraWinSchedule.MonthOfYear monthOfYear4 = new Infragistics.Win.UltraWinSchedule.MonthOfYear(Infragistics.Win.UltraWinSchedule.YearMonthEnum.April);
            Infragistics.Win.UltraWinSchedule.MonthOfYear monthOfYear5 = new Infragistics.Win.UltraWinSchedule.MonthOfYear(Infragistics.Win.UltraWinSchedule.YearMonthEnum.May);
            Infragistics.Win.UltraWinSchedule.MonthOfYear monthOfYear6 = new Infragistics.Win.UltraWinSchedule.MonthOfYear(Infragistics.Win.UltraWinSchedule.YearMonthEnum.June);
            Infragistics.Win.UltraWinSchedule.MonthOfYear monthOfYear7 = new Infragistics.Win.UltraWinSchedule.MonthOfYear(Infragistics.Win.UltraWinSchedule.YearMonthEnum.July);
            Infragistics.Win.UltraWinSchedule.MonthOfYear monthOfYear8 = new Infragistics.Win.UltraWinSchedule.MonthOfYear(Infragistics.Win.UltraWinSchedule.YearMonthEnum.August);
            Infragistics.Win.UltraWinSchedule.MonthOfYear monthOfYear9 = new Infragistics.Win.UltraWinSchedule.MonthOfYear(Infragistics.Win.UltraWinSchedule.YearMonthEnum.September);
            Infragistics.Win.UltraWinSchedule.MonthOfYear monthOfYear10 = new Infragistics.Win.UltraWinSchedule.MonthOfYear(Infragistics.Win.UltraWinSchedule.YearMonthEnum.October);
            Infragistics.Win.UltraWinSchedule.MonthOfYear monthOfYear11 = new Infragistics.Win.UltraWinSchedule.MonthOfYear(Infragistics.Win.UltraWinSchedule.YearMonthEnum.November);
            Infragistics.Win.UltraWinSchedule.MonthOfYear monthOfYear12 = new Infragistics.Win.UltraWinSchedule.MonthOfYear(Infragistics.Win.UltraWinSchedule.YearMonthEnum.December);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgDateSelect));
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            this.btnOk = new Controls.NoBorderButton();
            this.btnCancel = new Controls.NoBorderButton();
            this.cliDateSelect = new Infragistics.Win.UltraWinSchedule.UltraCalendarInfo(this.components);
            this.cllDateSelect = new Infragistics.Win.UltraWinSchedule.UltraCalendarLook(this.components);
            this.mthCalendarView = new Infragistics.Win.UltraWinSchedule.UltraMonthViewMulti();
            this.pnlDialogButton.ClientArea.SuspendLayout();
            this.pnlDialogButton.SuspendLayout();
            this.pnlDialogMain.ClientArea.SuspendLayout();
            this.pnlDialogMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mthCalendarView)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDialogButton
            // 
            // 
            // pnlDialogButton.ClientArea
            // 
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnOk);
            this.pnlDialogButton.ClientArea.Controls.Add(this.btnCancel);
            this.pnlDialogButton.Location = new System.Drawing.Point(0, 349);
            this.pnlDialogButton.Size = new System.Drawing.Size(364, 57);
            // 
            // pnlDialogMain
            // 
            // 
            // pnlDialogMain.ClientArea
            // 
            this.pnlDialogMain.ClientArea.Controls.Add(this.mthCalendarView);
            this.pnlDialogMain.Size = new System.Drawing.Size(364, 349);
            // 
            // lblDialogTitle
            // 
            this.lblDialogTitle.Size = new System.Drawing.Size(364, 28);
            this.lblDialogTitle.Text = "Date Select";
            // 
            // btnOk
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.Image = global::Oelco.CarisX.Properties.Resources.Image_Execute;
            appearance1.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnOk.Appearance = appearance1;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnOk.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance2.BorderColor = System.Drawing.Color.Transparent;
            this.btnOk.HotTrackAppearance = appearance2;
            this.btnOk.ImageSize = new System.Drawing.Size(0, 0);
            this.btnOk.Location = new System.Drawing.Point(29, 9);
            this.btnOk.Name = "btnOk";
            this.btnOk.Padding = new System.Drawing.Size(10, 0);
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.btnOk.PressedAppearance = appearance3;
            this.btnOk.ShowFocusRect = false;
            this.btnOk.ShowOutline = false;
            this.btnOk.Size = new System.Drawing.Size(152, 39);
            this.btnOk.TabIndex = 158;
            this.btnOk.Text = "OK";
            this.btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BorderColor = System.Drawing.Color.Transparent;
            appearance4.Image = global::Oelco.CarisX.Properties.Resources.Image_Exit;
            appearance4.ImageBackground = global::Oelco.CarisX.Properties.Resources.Image_Button;
            this.btnCancel.Appearance = appearance4;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance5.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.HotTrackAppearance = appearance5;
            this.btnCancel.ImageSize = new System.Drawing.Size(0, 0);
            this.btnCancel.Location = new System.Drawing.Point(187, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Drawing.Size(10, 0);
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            this.btnCancel.PressedAppearance = appearance6;
            this.btnCancel.ShowFocusRect = false;
            this.btnCancel.ShowOutline = false;
            this.btnCancel.Size = new System.Drawing.Size(152, 39);
            this.btnCancel.TabIndex = 157;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cliDateSelect
            // 
            this.cliDateSelect.AllowAllDayEvents = false;
            this.cliDateSelect.DataBindingsForAppointments.BindingContextControl = this;
            this.cliDateSelect.DataBindingsForOwners.BindingContextControl = this;
            monthOfYear1.ShortDescription = "1/";
            monthOfYear2.ShortDescription = "2/";
            monthOfYear3.ShortDescription = "3/";
            monthOfYear4.ShortDescription = "4/";
            monthOfYear5.ShortDescription = "5/";
            monthOfYear6.ShortDescription = "6/";
            monthOfYear7.ShortDescription = "7/";
            monthOfYear8.ShortDescription = "8/";
            monthOfYear9.ShortDescription = "9/";
            monthOfYear10.ShortDescription = "10/";
            monthOfYear11.ShortDescription = "11/";
            monthOfYear12.ShortDescription = "12/";
            this.cliDateSelect.MonthsOfYear.Add(monthOfYear1);
            this.cliDateSelect.MonthsOfYear.Add(monthOfYear2);
            this.cliDateSelect.MonthsOfYear.Add(monthOfYear3);
            this.cliDateSelect.MonthsOfYear.Add(monthOfYear4);
            this.cliDateSelect.MonthsOfYear.Add(monthOfYear5);
            this.cliDateSelect.MonthsOfYear.Add(monthOfYear6);
            this.cliDateSelect.MonthsOfYear.Add(monthOfYear7);
            this.cliDateSelect.MonthsOfYear.Add(monthOfYear8);
            this.cliDateSelect.MonthsOfYear.Add(monthOfYear9);
            this.cliDateSelect.MonthsOfYear.Add(monthOfYear10);
            this.cliDateSelect.MonthsOfYear.Add(monthOfYear11);
            this.cliDateSelect.MonthsOfYear.Add(monthOfYear12);
            this.cliDateSelect.ReminderImage = ((System.Drawing.Image)(resources.GetObject("cliDateSelect.ReminderImage")));
            this.cliDateSelect.SelectTypeDay = Infragistics.Win.UltraWinSchedule.SelectType.Single;
            this.cliDateSelect.BeforeSelectedDateRangeChange += new Infragistics.Win.UltraWinSchedule.BeforeSelectedDateRangeChangeEventHandler(this.cliDateSelect_BeforeSelectedDateRangeChange);
            // 
            // cllDateSelect
            // 
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            appearance8.BorderColor = System.Drawing.Color.Red;
            appearance8.ForeColor = System.Drawing.Color.Black;
            this.cllDateSelect.SelectedDayAppearance = appearance8;
            appearance9.FontData.SizeInPoints = 1F;
            this.cllDateSelect.WeekAppearance = appearance9;
            appearance10.FontData.SizeInPoints = 1F;
            this.cllDateSelect.WeekHeaderAppearance = appearance10;
            // 
            // mthCalendarView
            // 
            this.mthCalendarView.AllowWeekSelection = false;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.FontData.SizeInPoints = 18F;
            this.mthCalendarView.Appearance = appearance7;
            this.mthCalendarView.BackColor = System.Drawing.Color.White;
            this.mthCalendarView.CalendarInfo = this.cliDateSelect;
            this.mthCalendarView.CalendarLook = this.cllDateSelect;
            this.mthCalendarView.Location = new System.Drawing.Point(25, 60);
            this.mthCalendarView.MonthHeaderCaptionStyle = Infragistics.Win.UltraWinSchedule.MonthHeaderCaptionStyle.ShortDescription;
            this.mthCalendarView.MonthPadding = new System.Drawing.Size(3, 3);
            this.mthCalendarView.Name = "mthCalendarView";
            this.mthCalendarView.Size = new System.Drawing.Size(285, 270);
            this.mthCalendarView.TabIndex = 69;
            // 
            // DlgDateSelect
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Caption = "Date Select";
            this.ClientSize = new System.Drawing.Size(364, 406);
            this.Name = "DlgDateSelect";
            this.pnlDialogButton.ClientArea.ResumeLayout(false);
            this.pnlDialogButton.ResumeLayout(false);
            this.pnlDialogMain.ClientArea.ResumeLayout(false);
            this.pnlDialogMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mthCalendarView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.NoBorderButton btnOk;
        private Controls.NoBorderButton btnCancel;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarLook cllDateSelect;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarInfo cliDateSelect;
        private Infragistics.Win.UltraWinSchedule.UltraMonthViewMulti mthCalendarView;
    }
}