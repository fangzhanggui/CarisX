namespace Oelco.CarisX.GUI.Controls
{
    partial class SearchInfoPanelSpecimenResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchInfoPanelSpecimenResult));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            this.txtComment = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.chkComment = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.gbxJudgement = new Infragistics.Win.Misc.UltraGroupBox();
            this.optJudgement = new Oelco.Common.GUI.CustomUOptionSet();
            this.chkJudgement = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.cmbSpecimenType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.chkSpecimenMaterialType = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.txtPatientId = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.chkPatientId = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtComment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkComment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxJudgement)).BeginInit();
            this.gbxJudgement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optJudgement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkJudgement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSpecimenType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSpecimenMaterialType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatientId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkPatientId)).BeginInit();
            this.SuspendLayout();
            // 
            // lvwSelectedAnalytes
            // 
            this.lvwSelectedAnalytes.ViewSettingsList.ImageSize = new System.Drawing.Size(0, 0);
            this.lvwSelectedAnalytes.ViewSettingsList.MultiColumn = false;
            // 
            // txtComment
            // 
            resources.ApplyResources(this.txtComment, "txtComment");
            this.txtComment.MaxLength = 80;
            this.txtComment.Name = "txtComment";
            // 
            // chkComment
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.FontData.Name = resources.GetString("resource.Name");
            appearance1.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints")));
            appearance1.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckOFF;
            this.chkComment.Appearance = appearance1;
            this.chkComment.BackColor = System.Drawing.Color.Transparent;
            this.chkComment.BackColorInternal = System.Drawing.Color.Transparent;
            appearance2.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckON;
            this.chkComment.CheckedAppearance = appearance2;
            resources.ApplyResources(this.chkComment, "chkComment");
            this.chkComment.Name = "chkComment";
            this.chkComment.Style = Infragistics.Win.EditCheckStyle.Custom;
            this.chkComment.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // gbxJudgement
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(233)))), ((int)(((byte)(233)))));
            this.gbxJudgement.Appearance = appearance3;
            this.gbxJudgement.Controls.Add(this.optJudgement);
            resources.ApplyResources(this.gbxJudgement, "gbxJudgement");
            this.gbxJudgement.Name = "gbxJudgement";
            // 
            // optJudgement
            // 
            appearance4.FontData.BoldAsString = resources.GetString("resource.BoldAsString");
            appearance4.FontData.Name = resources.GetString("resource.Name1");
            appearance4.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints1")));
            resources.ApplyResources(appearance4, "appearance4");
            this.optJudgement.Appearance = appearance4;
            this.optJudgement.BackColor = System.Drawing.Color.Transparent;
            this.optJudgement.BackColorInternal = System.Drawing.Color.Transparent;
            this.optJudgement.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            valueListItem4.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem4.DataValue = 0;
            resources.ApplyResources(valueListItem4, "valueListItem4");
            valueListItem4.ForceApplyResources = "";
            valueListItem1.DataValue = 1;
            resources.ApplyResources(valueListItem1, "valueListItem1");
            valueListItem1.ForceApplyResources = "";
            valueListItem2.DataValue = 2;
            resources.ApplyResources(valueListItem2, "valueListItem2");
            valueListItem2.ForceApplyResources = "";
            this.optJudgement.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem4,
            valueListItem1,
            valueListItem2});
            this.optJudgement.ItemSpacingHorizontal = 74;
            resources.ApplyResources(this.optJudgement, "optJudgement");
            this.optJudgement.Name = "optJudgement";
            // 
            // chkJudgement
            // 
            appearance5.BackColor = System.Drawing.Color.Transparent;
            appearance5.FontData.Name = resources.GetString("resource.Name2");
            appearance5.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints2")));
            appearance5.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckOFF;
            this.chkJudgement.Appearance = appearance5;
            this.chkJudgement.BackColor = System.Drawing.Color.Transparent;
            this.chkJudgement.BackColorInternal = System.Drawing.Color.Transparent;
            appearance6.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckON;
            this.chkJudgement.CheckedAppearance = appearance6;
            resources.ApplyResources(this.chkJudgement, "chkJudgement");
            this.chkJudgement.Name = "chkJudgement";
            this.chkJudgement.Style = Infragistics.Win.EditCheckStyle.Custom;
            this.chkJudgement.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // cmbSpecimenType
            // 
            resources.ApplyResources(this.cmbSpecimenType, "cmbSpecimenType");
            this.cmbSpecimenType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbSpecimenType.Name = "cmbSpecimenType";
            // 
            // chkSpecimenMaterialType
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            appearance7.FontData.Name = resources.GetString("resource.Name3");
            appearance7.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints3")));
            appearance7.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckOFF;
            this.chkSpecimenMaterialType.Appearance = appearance7;
            this.chkSpecimenMaterialType.BackColor = System.Drawing.Color.Transparent;
            this.chkSpecimenMaterialType.BackColorInternal = System.Drawing.Color.Transparent;
            appearance8.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckON;
            this.chkSpecimenMaterialType.CheckedAppearance = appearance8;
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
            appearance9.BackColor = System.Drawing.Color.Transparent;
            appearance9.FontData.Name = resources.GetString("resource.Name4");
            appearance9.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints4")));
            appearance9.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckOFF;
            this.chkPatientId.Appearance = appearance9;
            this.chkPatientId.BackColor = System.Drawing.Color.Transparent;
            this.chkPatientId.BackColorInternal = System.Drawing.Color.Transparent;
            this.chkPatientId.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            appearance10.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckON;
            this.chkPatientId.CheckedAppearance = appearance10;
            appearance11.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            this.chkPatientId.HotTrackingAppearance = appearance11;
            appearance12.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            this.chkPatientId.IndeterminateAppearance = appearance12;
            resources.ApplyResources(this.chkPatientId, "chkPatientId");
            this.chkPatientId.Name = "chkPatientId";
            this.chkPatientId.Style = Infragistics.Win.EditCheckStyle.Custom;
            // 
            // SearchInfoPanelSpecimenResult
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.chkComment);
            this.Controls.Add(this.gbxJudgement);
            this.Controls.Add(this.chkJudgement);
            this.Controls.Add(this.cmbSpecimenType);
            this.Controls.Add(this.chkSpecimenMaterialType);
            this.Controls.Add(this.txtPatientId);
            this.Controls.Add(this.chkPatientId);
            this.Name = "SearchInfoPanelSpecimenResult";
            this.Controls.SetChildIndex(this.lblRackIdPrefix2, 0);
            this.Controls.SetChildIndex(this.lblRackIdPrefix1, 0);
            this.Controls.SetChildIndex(this.btnSelectAnalyte, 0);
            this.Controls.SetChildIndex(this.chkRackId, 0);
            this.Controls.SetChildIndex(this.numRackIdFrom, 0);
            this.Controls.SetChildIndex(this.numRackIdTo, 0);
            this.Controls.SetChildIndex(this.lblHyphen2, 0);
            this.Controls.SetChildIndex(this.chkSequenceNo, 0);
            this.Controls.SetChildIndex(this.numSequenceNoFrom, 0);
            this.Controls.SetChildIndex(this.numSequenceNoTo, 0);
            this.Controls.SetChildIndex(this.lblHyphen3, 0);
            this.Controls.SetChildIndex(this.chkConcentration, 0);
            this.Controls.SetChildIndex(this.gbxRemark, 0);
            this.Controls.SetChildIndex(this.chkMeasuringTime, 0);
            this.Controls.SetChildIndex(this.btnMeasuringTimeFrom, 0);
            this.Controls.SetChildIndex(this.btnMeasuringTimeTo, 0);
            this.Controls.SetChildIndex(this.lblHyphen4, 0);
            this.Controls.SetChildIndex(this.numConcentrationFrom, 0);
            this.Controls.SetChildIndex(this.numConcentrationTo, 0);
            this.Controls.SetChildIndex(this.lblHyphen1, 0);
            this.Controls.SetChildIndex(this.lvwSelectedAnalytes, 0);
            this.Controls.SetChildIndex(this.chkPatientId, 0);
            this.Controls.SetChildIndex(this.txtPatientId, 0);
            this.Controls.SetChildIndex(this.chkSpecimenMaterialType, 0);
            this.Controls.SetChildIndex(this.cmbSpecimenType, 0);
            this.Controls.SetChildIndex(this.chkJudgement, 0);
            this.Controls.SetChildIndex(this.gbxJudgement, 0);
            this.Controls.SetChildIndex(this.chkComment, 0);
            this.Controls.SetChildIndex(this.txtComment, 0);
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
            ((System.ComponentModel.ISupportInitialize)(this.txtComment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkComment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbxJudgement)).EndInit();
            this.gbxJudgement.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optJudgement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkJudgement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSpecimenType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSpecimenMaterialType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatientId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkPatientId)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected Infragistics.Win.UltraWinEditors.UltraTextEditor txtComment;
        protected Infragistics.Win.UltraWinEditors.UltraCheckEditor chkComment;
        protected Infragistics.Win.Misc.UltraGroupBox gbxJudgement;
        protected Oelco.Common.GUI.CustomUOptionSet optJudgement;
        protected Infragistics.Win.UltraWinEditors.UltraCheckEditor chkJudgement;
        protected Infragistics.Win.UltraWinEditors.UltraComboEditor cmbSpecimenType;
        protected Infragistics.Win.UltraWinEditors.UltraCheckEditor chkSpecimenMaterialType;
        protected Infragistics.Win.UltraWinEditors.UltraTextEditor txtPatientId;
        protected Infragistics.Win.UltraWinEditors.UltraCheckEditor chkPatientId;
    }
}
