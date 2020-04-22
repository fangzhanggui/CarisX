namespace Oelco.CarisX.GUI.Controls
{
    partial class SearchInfoPanelCalibratorResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchInfoPanelCalibratorResult));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.txtCalibratorLot = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.chkCalibratorLot = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            ((System.ComponentModel.ISupportInitialize)(this.numConcentrationTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numConcentrationFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMeasuringTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxRemark)).BeginInit();
            this.gbxRemark.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkConcentration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSequenceNoTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSequenceNoFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSequenceNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRackIdTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRackIdFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRackId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lvwSelectedAnalytes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemarkCalibrationError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemarkDataWarning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemarkOnLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemarkDataEdited)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemarkExpirationDataError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemarkTemperatureError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCalibratorLot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCalibratorLot)).BeginInit();
            this.SuspendLayout();
            // 
            // lblHyphen3
            // 
            resources.ApplyResources(this.lblHyphen3, "lblHyphen3");
            // 
            // numSequenceNoTo
            // 
            resources.ApplyResources(this.numSequenceNoTo, "numSequenceNoTo");
            // 
            // numSequenceNoFrom
            // 
            resources.ApplyResources(this.numSequenceNoFrom, "numSequenceNoFrom");
            // 
            // chkSequenceNo
            // 
            resources.ApplyResources(this.chkSequenceNo, "chkSequenceNo");
            // 
            // lblHyphen2
            // 
            resources.ApplyResources(this.lblHyphen2, "lblHyphen2");
            // 
            // numRackIdTo
            // 
            this.numRackIdTo.FormatString = "000";
            resources.ApplyResources(this.numRackIdTo, "numRackIdTo");
            this.numRackIdTo.MaxValue = 999;
            // 
            // numRackIdFrom
            // 
            this.numRackIdFrom.FormatString = "000";
            resources.ApplyResources(this.numRackIdFrom, "numRackIdFrom");
            this.numRackIdFrom.MaxValue = 999;
            // 
            // lvwSelectedAnalytes
            // 
            this.lvwSelectedAnalytes.ViewSettingsList.ImageSize = new System.Drawing.Size(0, 0);
            this.lvwSelectedAnalytes.ViewSettingsList.MultiColumn = false;
            // 
            // lblRackIdPrefix1
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.FontData.Name = resources.GetString("resource.Name");
            appearance1.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints")));
            this.lblRackIdPrefix1.Appearance = appearance1;
            resources.ApplyResources(this.lblRackIdPrefix1, "lblRackIdPrefix1");
            // 
            // lblRackIdPrefix2
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.FontData.Name = resources.GetString("resource.Name1");
            appearance2.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints1")));
            this.lblRackIdPrefix2.Appearance = appearance2;
            resources.ApplyResources(this.lblRackIdPrefix2, "lblRackIdPrefix2");
            // 
            // txtCalibratorLot
            // 
            resources.ApplyResources(this.txtCalibratorLot, "txtCalibratorLot");
            this.txtCalibratorLot.Name = "txtCalibratorLot";
            // 
            // chkCalibratorLot
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.FontData.Name = resources.GetString("resource.Name2");
            appearance3.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints2")));
            appearance3.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckOFF;
            this.chkCalibratorLot.Appearance = appearance3;
            this.chkCalibratorLot.BackColor = System.Drawing.Color.Transparent;
            this.chkCalibratorLot.BackColorInternal = System.Drawing.Color.Transparent;
            this.chkCalibratorLot.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            appearance4.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckON;
            this.chkCalibratorLot.CheckedAppearance = appearance4;
            appearance5.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            this.chkCalibratorLot.HotTrackingAppearance = appearance5;
            appearance6.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            this.chkCalibratorLot.IndeterminateAppearance = appearance6;
            resources.ApplyResources(this.chkCalibratorLot, "chkCalibratorLot");
            this.chkCalibratorLot.Name = "chkCalibratorLot";
            this.chkCalibratorLot.Style = Infragistics.Win.EditCheckStyle.Custom;
            // 
            // SearchInfoPanelCalibratorResult
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtCalibratorLot);
            this.Controls.Add(this.chkCalibratorLot);
            this.Name = "SearchInfoPanelCalibratorResult";
            this.Controls.SetChildIndex(this.chkSequenceNo, 0);
            this.Controls.SetChildIndex(this.numSequenceNoFrom, 0);
            this.Controls.SetChildIndex(this.numSequenceNoTo, 0);
            this.Controls.SetChildIndex(this.lblHyphen3, 0);
            this.Controls.SetChildIndex(this.chkRackId, 0);
            this.Controls.SetChildIndex(this.numRackIdFrom, 0);
            this.Controls.SetChildIndex(this.lblHyphen2, 0);
            this.Controls.SetChildIndex(this.btnSelectAnalyte, 0);
            this.Controls.SetChildIndex(this.chkConcentration, 0);
            this.Controls.SetChildIndex(this.gbxRemark, 0);
            this.Controls.SetChildIndex(this.chkMeasuringTime, 0);
            this.Controls.SetChildIndex(this.btnMeasuringTimeFrom, 0);
            this.Controls.SetChildIndex(this.btnMeasuringTimeTo, 0);
            this.Controls.SetChildIndex(this.lblHyphen4, 0);
            this.Controls.SetChildIndex(this.numConcentrationFrom, 0);
            this.Controls.SetChildIndex(this.numConcentrationTo, 0);
            this.Controls.SetChildIndex(this.lblHyphen1, 0);
            this.Controls.SetChildIndex(this.lblRackIdPrefix2, 0);
            this.Controls.SetChildIndex(this.lblRackIdPrefix1, 0);
            this.Controls.SetChildIndex(this.numRackIdTo, 0);
            this.Controls.SetChildIndex(this.lvwSelectedAnalytes, 0);
            this.Controls.SetChildIndex(this.chkCalibratorLot, 0);
            this.Controls.SetChildIndex(this.txtCalibratorLot, 0);
            ((System.ComponentModel.ISupportInitialize)(this.numConcentrationTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numConcentrationFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMeasuringTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxRemark)).EndInit();
            this.gbxRemark.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkConcentration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSequenceNoTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSequenceNoFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSequenceNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRackIdTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRackIdFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRackId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lvwSelectedAnalytes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemarkCalibrationError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemarkDataWarning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemarkOnLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemarkDataEdited)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemarkExpirationDataError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemarkTemperatureError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCalibratorLot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCalibratorLot)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected Infragistics.Win.UltraWinEditors.UltraTextEditor txtCalibratorLot;
        protected Infragistics.Win.UltraWinEditors.UltraCheckEditor chkCalibratorLot;
    }
}
