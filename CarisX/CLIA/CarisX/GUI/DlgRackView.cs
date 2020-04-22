using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.CarisX.Common;
using Oelco.CarisX.Const;
using Oelco.CarisX.DB;
using Oelco.Common.Utility;
using Oelco.CarisX.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using System.Collections;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// ラック状態表示ダイアログ
    /// </summary>
    public partial class DlgRackView : DlgCarisXBase
    {
        #region [定数定義]

        #region _グリッド列Key_

        /// <summary>
        /// グリッドカラムKey(シーケンス番号)
        /// </summary>                    
        private const String STRING_GRD_SEQUENCENO = "Sequence No.";
        
        /// <summary>
        /// グリッドカラムKey(ラックID)
        /// </summary>
        private const String STRING_GRD_RACK = "Rack ID";

        /// <summary>
        /// グリッドカラムKey(検体ID)
        /// </summary>
        private const String STRING_GRD_PATIENTID = "Patient ID";

        /// <summary>
        /// グリッドカラムKey(ラックポジション)
        /// </summary>
        private const String STRING_GRD_RACKPOSITION = "Rack Position";

        /// <summary>
        /// グリッドカラムKey(サンプル種別)
        /// </summary>
        private const String STRING_GRD_SPECIMENTYPE = "Specimen type";

        /// <summary>
        /// グリッドカラムKey(手希釈倍率)
        /// </summary>
        private const String STRING_GRD_MANUALDILUTIONRATIO = "Manual dilution ratio";

        /// <summary>
        /// グリッドカラムKey(分析項目)
        /// </summary>
        private const String STRING_GRD_ANALYTES = "Analytes";

        /// <summary>
        /// グリッドカラムKey(ステータス)
        /// </summary>
        private const String STRING_GRD_STATUS = "Status";

        /// <summary>
        /// グリッドカラムKey(残り時間)
        /// </summary>
        private const String STRING_GRD_REMAININGTIME = "Remaining time";

        /// <summary>
        /// グリッドカラムKey(カウント)
        /// </summary>
        private const String STRING_GRD_COUNT = "Count";

        /// <summary>
        /// グリッドカラムKey(濃度)
        /// </summary>
        private const String STRING_GRD_CONCENTRATION = "Concentration";

        /// <summary>
        /// グリッドカラムKey(コメント)
        /// </summary>
        private const String STRING_GRD_COMMENT = "Comment";

        #endregion

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 表示検体種別
        /// </summary>
        private SampleKind displayDataKind = SampleKind.Sample;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// クリックされたラックのポジションを、フィルタリング条件に追加する
        /// </remarks>>
        public DlgRackView(CarisXIDString rackId, Int32 rackPos)
        {
            InitializeComponent();

            if ( rackId != null )
            {
                this.displayDataKind = rackId.GetSampleKind();

				// クリックされたラックのポジションを、フィルタリング条件に追加する
				var individuallyNoList = Singleton<SampleRackInfoManager>.Instance.SamplingStageRackStatus.Where((statusInfo) => (statusInfo.RackId.DispPreCharString == rackId.DispPreCharString) && (statusInfo.RackPos == rackPos)).Select((statusInfo) => statusInfo.IndividuallyNumber)
					 .Union(Singleton<SampleRackInfoManager>.Instance.NormalRackStatus.Where((statusInfo) => (statusInfo.RackId.DispPreCharString == rackId.DispPreCharString) && (statusInfo.RackPos == rackPos)).Select((statusInfo) => statusInfo.IndividuallyNumber)
					 .Union(Singleton<SampleRackInfoManager>.Instance.PrioritiyRackStatus.Where((statusInfo) => (statusInfo.RackId.DispPreCharString == rackId.DispPreCharString) && (statusInfo.RackPos == rackPos)).Select((statusInfo) => statusInfo.IndividuallyNumber)));

                IList list = null;
                switch ( this.displayDataKind )
                {
                case SampleKind.Sample:
                    list = Singleton<SpecimenAssayDB>.Instance.GetData( rackId ).Where( ( data ) => individuallyNoList.Contains( data.GetIndividuallyNo() ) ).OrderByDescending((data)=>data.GetUniqueNo()).ToList();
                    break;
                case SampleKind.Calibrator:
                    list = Singleton<CalibratorAssayDB>.Instance.GetData( rackId ).Where( ( data ) => individuallyNoList.Contains( data.GetIndividuallyNo() ) ).OrderByDescending( ( data ) => data.GetUniqueNo() ).ToList();
                    break;
                case SampleKind.Control:
                    list = Singleton<ControlAssayDB>.Instance.GetData( rackId ).Where( ( data ) => individuallyNoList.Contains( data.GetIndividuallyNo() ) ).OrderByDescending( ( data ) => data.GetUniqueNo() ).ToList();
                    break;
                }

                if ( list != null && list.Count > 0 )
                {
                    this.grdRackInfo.DataSource = list;
                }
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// メッセージボックスダイアログ表示
        /// </summary>
        /// <remarks>
        /// メッセージボックスダイアログを表示します
        /// </remarks>
        /// <returns></returns>
        public override DialogResult ShowDialog()
        {
            if ( this.grdRackInfo.DataSource != null )
            {
                return base.ShowDialog();
            }
            return DialogResult.Abort;
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// リソースの初期化
        /// </summary>
        /// <remarks>
        /// リソースを初期化します
        /// </remarks>
        protected override void initializeResource()
        {
        }

        /// <summary>
        /// コンポーネントの初期化
        /// </summary>
        /// <remarks>
        /// コンポーネントを初期化します
        /// </remarks>
        protected override void initializeFormComponent()
        {
            // スクロール処理設定
            this.gesturePanel.ScrollProxy = this.grdRackInfo.ScrollProxy;
            switch ( this.displayDataKind )
            {
            case SampleKind.Sample:
            case SampleKind.Priority:
                // グリッド表示順
                this.grdRackInfo.SetGridColumnOrder( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.RackViewSettings.GridColOrderSpecimen );
                // グリッド列幅
                this.grdRackInfo.SetGridColmnWidth( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.RackViewSettings.GridColWidthSpecimen );

                // 数値右寄せ
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.SequenceNo].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ReceiptNumber].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.RackPosition].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ReplicationNo].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ManualDilution].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.AutoDilution].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.Count].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.Concentration].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ReagentLotNo].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.PretriggerLotNo].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.TriggerLotNo].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                break;

            case SampleKind.Calibrator:
                // グリッド表示順
                this.grdRackInfo.SetGridColumnOrder( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.RackViewSettings.GridColOrderControl );
                // グリッド列幅
                this.grdRackInfo.SetGridColmnWidth( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.RackViewSettings.GridColWidthControl );

                // 数値右寄せ
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.SequenceNo].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.RackPosition].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.ReplicationNo].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.Count].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.Concentration].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.ReagentLotNo].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.PretriggerLotNo].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.TriggerLotNo].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                break;

            case SampleKind.Control:
                // グリッド表示順
                this.grdRackInfo.SetGridColumnOrder( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.RackViewSettings.GridColOrderCalibrator );
                // グリッド列幅
                this.grdRackInfo.SetGridColmnWidth( Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.RackViewSettings.GridColWidthCalibrator );

                // 数値右寄せ
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.SequenceNo].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.RackPosition].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.ReplicationNo].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.Count].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.Concentration].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.ReagentLotNo].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.PretriggerLotNo].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.TriggerLotNo].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                break;

            default:
                break;
            }
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            // ダイアログタイトル
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_000;

            this.btnClose.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_002;

            // カラム設定
            switch ( this.displayDataKind )
            {
            case SampleKind.Sample:
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.SequenceNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_001;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ReceiptNumber].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_013;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.RackId].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_002;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.RackPosition].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_004;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.PatientId].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_003;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ReplicationNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_014;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.StatusString].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_008;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.RemainTime].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_009;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.Analytes].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_007;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.SpecimenMaterialType].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_005;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ManualDilution].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_006;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.AutoDilution].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_015;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.Count].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_010;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.Concentration].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_011;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.Judgement].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_016;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.MeasureDateTime].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_017;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.CalibCurveDateTime].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_018;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.ReagentLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_019;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.PretriggerLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_020;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.TriggerLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_021;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.Remark].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_022;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[SpecimenAssayData.DataKeys.Comment].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_012;
                break;
            case SampleKind.Calibrator:
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.SequenceNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_001;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.RackId].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_002;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.RackPosition].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_004;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.CalibLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_023;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.ReplicationNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_014;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.StatusString].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_008;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.RemainTime].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_009;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.Analytes].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_007;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.Count].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_010;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.Concentration].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_011;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.MeasureDateTime].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_017;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.ReagentLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_019;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.PretriggerLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_020;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.TriggerLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_021;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[CalibratorAssayData.DataKeys.Remark].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_022;
                break;
            case SampleKind.Control:
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.SequenceNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_001;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.RackId].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_002;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.RackPosition].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_004;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.ControlLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_025;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.ControlName].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_024;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.ReplicationNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_014;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.StatusString].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_008;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.RemainTime].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_009;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.Analytes].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_007;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.Count].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_010;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.Concentration].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_011;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.MeasureDateTime].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_017;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.CalibCurveDateTime].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_018;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.ReagentLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_019;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.PretriggerLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_020;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.TriggerLotNo].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_021;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.Remark].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_022;
                this.grdRackInfo.DisplayLayout.Bands[0].Columns[ControlAssayData.DataKeys.Comment].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_RACKVIEW_012;
                break;
            default:
                break;
            }
        }

        #endregion
        
        #region [privateメソッド]

        /// <summary>
        /// 閉じるボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 画面を終了します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click( object sender, EventArgs e )
        {
            this.Close();
        }

        #endregion

        /// <summary>
        /// FormClosedイベント
        /// </summary>
        /// <remarks>
        /// UI設定を保存します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DlgRackView_FormClosed( object sender, FormClosedEventArgs e )
        {
            //UI設定保存
            switch ( this.displayDataKind )
            {
            case SampleKind.Sample:
            case SampleKind.Priority:
                Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.RackViewSettings.GridColOrderSpecimen = this.grdRackInfo.GetGridColumnOrder();
                Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.RackViewSettings.GridColWidthSpecimen = this.grdRackInfo.GetGridColmnWidth();
                break;
            case SampleKind.Control:
                Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.RackViewSettings.GridColOrderCalibrator = this.grdRackInfo.GetGridColumnOrder();
                Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.RackViewSettings.GridColWidthCalibrator = this.grdRackInfo.GetGridColmnWidth();
                break;
            case SampleKind.Calibrator:
                Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.RackViewSettings.GridColOrderControl = this.grdRackInfo.GetGridColumnOrder();
                Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.RackViewSettings.GridColWidthControl = this.grdRackInfo.GetGridColmnWidth();
                break;
            default:
                break;
            }
        }
    }
}
