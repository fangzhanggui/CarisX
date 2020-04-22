namespace Oelco.CarisX.GUI.Controls
{
    partial class RecalcInfoPanelControlResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecalcInfoPanelControlResult));
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.txtControlLot = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.chkControlLot = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkControlName = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.txtControlName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtControlLot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkControlLot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkControlName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtControlName)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxRemark
            // 
            resources.ApplyResources(this.gbxRemark, "gbxRemark");
            // 
            // lblHyphen1
            // 
            resources.ApplyResources(this.lblHyphen1, "lblHyphen1");
            // 
            // gbxFilterCondition
            // 
            this.gbxFilterCondition.Controls.Add(this.txtControlName);
            this.gbxFilterCondition.Controls.Add(this.txtControlLot);
            this.gbxFilterCondition.Controls.Add(this.chkControlName);
            this.gbxFilterCondition.Controls.Add(this.chkControlLot);
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
            this.gbxFilterCondition.Controls.SetChildIndex(this.chkControlLot, 0);
            this.gbxFilterCondition.Controls.SetChildIndex(this.chkControlName, 0);
            this.gbxFilterCondition.Controls.SetChildIndex(this.txtControlLot, 0);
            this.gbxFilterCondition.Controls.SetChildIndex(this.txtControlName, 0);
            // 
            // lblRackIdPrefix1
            // 
            resources.ApplyResources(this.lblRackIdPrefix1, "lblRackIdPrefix1");
            // 
            // lblRackIdPrefix2
            // 
            resources.ApplyResources(this.lblRackIdPrefix2, "lblRackIdPrefix2");
            // 
            // numRackIdFrom
            // 
            this.numRackIdFrom.FormatString = "000";
            resources.ApplyResources(this.numRackIdFrom, "numRackIdFrom");
            this.numRackIdFrom.MaskInput = "nnn";
            this.numRackIdFrom.MaxValue = 999;
            // 
            // numRackIdTo
            // 
            this.numRackIdTo.FormatString = "000";
            resources.ApplyResources(this.numRackIdTo, "numRackIdTo");
            this.numRackIdTo.MaskInput = "nnn";
            this.numRackIdTo.MaxValue = 999;
            // 
            // txtControlLot
            // 
            resources.ApplyResources(this.txtControlLot, "txtControlLot");
            this.txtControlLot.Name = "txtControlLot";
            // 
            // chkControlLot
            // 
            appearance5.FontData.Name = resources.GetString("resource.Name1");
            appearance5.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints1")));
            appearance5.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckOFF;
            this.chkControlLot.Appearance = appearance5;
            this.chkControlLot.BackColor = System.Drawing.Color.Transparent;
            this.chkControlLot.BackColorInternal = System.Drawing.Color.Transparent;
            this.chkControlLot.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            appearance6.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckON;
            this.chkControlLot.CheckedAppearance = appearance6;
            appearance7.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            this.chkControlLot.HotTrackingAppearance = appearance7;
            appearance8.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            this.chkControlLot.IndeterminateAppearance = appearance8;
            resources.ApplyResources(this.chkControlLot, "chkControlLot");
            this.chkControlLot.Name = "chkControlLot";
            this.chkControlLot.Style = Infragistics.Win.EditCheckStyle.Custom;
            // 
            // chkControlName
            // 
            appearance1.FontData.Name = resources.GetString("resource.Name");
            appearance1.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints")));
            appearance1.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckOFF;
            this.chkControlName.Appearance = appearance1;
            this.chkControlName.BackColor = System.Drawing.Color.Transparent;
            this.chkControlName.BackColorInternal = System.Drawing.Color.Transparent;
            this.chkControlName.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            appearance2.Image = global::Oelco.CarisX.Properties.Resources.Image_CheckON;
            this.chkControlName.CheckedAppearance = appearance2;
            appearance3.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            this.chkControlName.HotTrackingAppearance = appearance3;
            appearance4.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            this.chkControlName.IndeterminateAppearance = appearance4;
            resources.ApplyResources(this.chkControlName, "chkControlName");
            this.chkControlName.Name = "chkControlName";
            this.chkControlName.Style = Infragistics.Win.EditCheckStyle.Custom;
            // 
            // txtControlName
            // 
            resources.ApplyResources(this.txtControlName, "txtControlName");
            this.txtControlName.Name = "txtControlName";
            // 
            // RecalcInfoPanelControlResult
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "RecalcInfoPanelControlResult";
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
            ((System.ComponentModel.ISupportInitialize)(this.txtControlLot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkControlLot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkControlName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtControlName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected Infragistics.Win.UltraWinEditors.UltraTextEditor txtControlLot;
        protected Infragistics.Win.UltraWinEditors.UltraCheckEditor chkControlLot;
        protected Infragistics.Win.UltraWinEditors.UltraTextEditor txtControlName;
        protected Infragistics.Win.UltraWinEditors.UltraCheckEditor chkControlName;

    }
}
