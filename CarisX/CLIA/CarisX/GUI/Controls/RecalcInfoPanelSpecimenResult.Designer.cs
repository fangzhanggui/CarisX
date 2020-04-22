namespace Oelco.CarisX.GUI.Controls
{
    partial class RecalcInfoPanelSpecimenResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecalcInfoPanelSpecimenResult));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.cmbSpecimenType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.chkSpecimenMaterialType = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.txtPatientId = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.chkPatientId = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            ((System.ComponentModel.ISupportInitialize)(this.gbxRemark)).BeginInit();
            this.gbxRemark.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemarkExpirationDataError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemarkCalibrationError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMeasuringTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSequenceNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRackId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxFilterCondition)).BeginInit();
            this.gbxFilterCondition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRackIdFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRackIdTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSequenceNoTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSequenceNoFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSpecimenType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSpecimenMaterialType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatientId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkPatientId)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxFilterCondition
            // 
            this.gbxFilterCondition.Controls.Add(this.cmbSpecimenType);
            this.gbxFilterCondition.Controls.Add(this.chkSpecimenMaterialType);
            this.gbxFilterCondition.Controls.Add(this.txtPatientId);
            this.gbxFilterCondition.Controls.Add(this.chkPatientId);
            this.gbxFilterCondition.Controls.SetChildIndex(this.lblRackIdPrefix2, 0);
            this.gbxFilterCondition.Controls.SetChildIndex(this.lblRackIdPrefix1, 0);
            this.gbxFilterCondition.Controls.SetChildIndex(this.chkRackId, 0);
            this.gbxFilterCondition.Controls.SetChildIndex(this.numRackIdFrom, 0);
            this.gbxFilterCondition.Controls.SetChildIndex(this.numRackIdTo, 0);
            this.gbxFilterCondition.Controls.SetChildIndex(this.lblHyphen1, 0);
            this.gbxFilterCondition.Controls.SetChildIndex(this.chkSequenceNo, 0);
            this.gbxFilterCondition.Controls.SetChildIndex(this.numSequenceNoFrom, 0);
            this.gbxFilterCondition.Controls.SetChildIndex(this.numSequenceNoTo, 0);
            this.gbxFilterCondition.Controls.SetChildIndex(this.lblHyphen2, 0);
            this.gbxFilterCondition.Controls.SetChildIndex(this.chkMeasuringTime, 0);
            this.gbxFilterCondition.Controls.SetChildIndex(this.btnMeasuringTimeFrom, 0);
            this.gbxFilterCondition.Controls.SetChildIndex(this.btnMeasuringTimeTo, 0);
            this.gbxFilterCondition.Controls.SetChildIndex(this.lblHyphen3, 0);
            this.gbxFilterCondition.Controls.SetChildIndex(this.gbxRemark, 0);
            this.gbxFilterCondition.Controls.SetChildIndex(this.chkPatientId, 0);
            this.gbxFilterCondition.Controls.SetChildIndex(this.txtPatientId, 0);
            this.gbxFilterCondition.Controls.SetChildIndex(this.chkSpecimenMaterialType, 0);
            this.gbxFilterCondition.Controls.SetChildIndex(this.cmbSpecimenType, 0);
            // 
            // cmbSpecimenType
            // 
            resources.ApplyResources(this.cmbSpecimenType, "cmbSpecimenType");
            this.cmbSpecimenType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbSpecimenType.Name = "cmbSpecimenType";
            // 
            // chkSpecimenMaterialType
            // 
            appearance1.FontData.Name = resources.GetString("resource.Name");
            appearance1.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints")));
            appearance1.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckOFF;
            this.chkSpecimenMaterialType.Appearance = appearance1;
            this.chkSpecimenMaterialType.BackColor = System.Drawing.Color.Transparent;
            this.chkSpecimenMaterialType.BackColorInternal = System.Drawing.Color.Transparent;
            appearance2.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckON;
            this.chkSpecimenMaterialType.CheckedAppearance = appearance2;
            resources.ApplyResources(this.chkSpecimenMaterialType, "chkSpecimenMaterialType");
            this.chkSpecimenMaterialType.Name = "chkSpecimenMaterialType";
            this.chkSpecimenMaterialType.Style = Infragistics.Win.EditCheckStyle.Custom;
            this.chkSpecimenMaterialType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtPatientId
            // 
            resources.ApplyResources(this.txtPatientId, "txtPatientId");
            this.txtPatientId.MaxLength = 16;
            this.txtPatientId.Name = "txtPatientId";
            // 
            // chkPatientId
            // 
            appearance3.FontData.Name = resources.GetString("resource.Name1");
            appearance3.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints1")));
            appearance3.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckOFF;
            this.chkPatientId.Appearance = appearance3;
            this.chkPatientId.BackColor = System.Drawing.Color.Transparent;
            this.chkPatientId.BackColorInternal = System.Drawing.Color.Transparent;
            this.chkPatientId.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            appearance4.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckON;
            this.chkPatientId.CheckedAppearance = appearance4;
            appearance5.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            this.chkPatientId.HotTrackingAppearance = appearance5;
            appearance6.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            this.chkPatientId.IndeterminateAppearance = appearance6;
            resources.ApplyResources(this.chkPatientId, "chkPatientId");
            this.chkPatientId.Name = "chkPatientId";
            this.chkPatientId.Style = Infragistics.Win.EditCheckStyle.Custom;
            // 
            // RecalcInfoPanelSpecimenResult
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "RecalcInfoPanelSpecimenResult";
            ((System.ComponentModel.ISupportInitialize)(this.gbxRemark)).EndInit();
            this.gbxRemark.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkRemarkExpirationDataError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRemarkCalibrationError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMeasuringTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSequenceNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRackId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxFilterCondition)).EndInit();
            this.gbxFilterCondition.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numRackIdFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRackIdTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSequenceNoTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSequenceNoFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSpecimenType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSpecimenMaterialType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatientId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkPatientId)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected Infragistics.Win.UltraWinEditors.UltraComboEditor cmbSpecimenType;
        protected Infragistics.Win.UltraWinEditors.UltraCheckEditor chkSpecimenMaterialType;
        protected Infragistics.Win.UltraWinEditors.UltraTextEditor txtPatientId;
        protected Infragistics.Win.UltraWinEditors.UltraCheckEditor chkPatientId;
    }
}
