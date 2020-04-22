using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.DB;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// ラックID割り当てダイアログクラス
    /// </summary>
    public partial class DlgSysRackAssign : DlgCarisXBaseSys
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgSysRackAssign()
        {
            InitializeComponent();
            switch (Singleton<Oelco.CarisX.Status.SystemStatus>.Instance.Status)
            {
                // 分析中・サンプリング停止中・試薬交換開始状態はOK不可
                case Status.SystemStatusKind.WaitSlaveResponce:
                case Status.SystemStatusKind.Assay:
                case Status.SystemStatusKind.SamplingPause:
                case Status.SystemStatusKind.ReagentExchange:
                    this.btnOK.Enabled = false;
                    break;
                default:
                    this.btnOK.Enabled = true;
                    break;
            }
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
            // パラメータ取得し、コントロールへ設定
            // 検体ラックID
            this.numPatientRackIDFrom.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDSamp.ToString();
            this.numPatientRackIDTo.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDSamp.ToString();
            this.numPatientRackIDModeSampleIdFrom.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDSampModeSample.ToString();
            this.numPatientRackIDModeSampleIdTo.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDSampModeSample.ToString();

            // キャリブレータラックID
            this.numCalibratorRackIDFrom.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDCalib.ToString();
            this.numCalibratorRackIDTo.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDCalib.ToString();
            // 精度管理検体ラックID
            this.numControlRackIDFrom.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDCtrl.ToString();
            this.numControlRackIDTo.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDCtrl.ToString();
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_RACK_ASSIGN_000;

            // グループボックス
            this.gbxPatientRackID.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_RACK_ASSIGN_GBX_001;
            this.gbxCalibratorRackID.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_RACK_ASSIGN_GBX_002;
            this.gbxControlRackID.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_RACK_ASSIGN_GBX_003;

            // ラベル
            this.lblComment.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_RACK_ASSIGN_LBL_001;
            this.lblPatientIDAssayText.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_RACK_ASSIGN_LBL_002;
            this.lblPatientRackID.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;
            this.lblPatientRackIDModeSampleId.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;
            this.lblCalibratorRackID.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;
            this.lblControlRackID.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;
            this.lblCalibratorRackIDFrom.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_RACK_ASSIGN_LBL_003;
            this.lblCalibratorRackIDTo.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_RACK_ASSIGN_LBL_003;
            this.lblControlRackIDFrom.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_RACK_ASSIGN_LBL_004;
            this.lblControlRackIDTo.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_RACK_ASSIGN_LBL_004;


            // ボタン
            this.btnOK.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// OKボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 設定パラメータをファイルに保存し、
        /// ダイアログ結果にOKを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnOK_Click(object sender, EventArgs e)
        {

            // 入力チェック
            if (!CheckValue())
            {
                return;
            }

            // 設定値取得、及びパラメータ設定

            var changeSamepleKind = new List<SampleKind>();

            // 検体ラックID(ラックID分析時)
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDSamp = (Int32)this.numPatientRackIDFrom.Value;
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDSamp = (Int32)this.numPatientRackIDTo.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDSamp
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.RackIDDefinitionParameter.MinRackIDSamp)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(CarisX.Properties.Resources.STRING_LOG_MSG_053
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDSamp + CarisX.Properties.Resources.STRING_LOG_MSG_001);
                if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.AssayMode == AssayModeParameter.AssayModeKind.RackID)
                {
                    changeSamepleKind.Add(SampleKind.Sample);
                }
            }
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDSamp
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.RackIDDefinitionParameter.MaxRackIDSamp)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(CarisX.Properties.Resources.STRING_LOG_MSG_054
                , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDSamp + CarisX.Properties.Resources.STRING_LOG_MSG_001);
                if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.AssayMode == AssayModeParameter.AssayModeKind.RackID)
                {
                    changeSamepleKind.Add(SampleKind.Sample);
                }
            }

            // 検体ラックID（検体ID分析時）
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDSampModeSample = (Int32)this.numPatientRackIDModeSampleIdFrom.Value;
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDSampModeSample = (Int32)this.numPatientRackIDModeSampleIdTo.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDSampModeSample
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.RackIDDefinitionParameter.MinRackIDSampModeSample)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(CarisX.Properties.Resources.STRING_LOG_MSG_055
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDSampModeSample + CarisX.Properties.Resources.STRING_LOG_MSG_001);
                if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.AssayMode == AssayModeParameter.AssayModeKind.SampleID)
                {
                    changeSamepleKind.Add(SampleKind.Sample);
                }
            }
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDSampModeSample
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.RackIDDefinitionParameter.MaxRackIDSampModeSample)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(CarisX.Properties.Resources.STRING_LOG_MSG_056
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDSampModeSample + CarisX.Properties.Resources.STRING_LOG_MSG_001);
                if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.AssayMode == AssayModeParameter.AssayModeKind.SampleID)
                {
                    changeSamepleKind.Add(SampleKind.Sample);
                }
            }

            // キャリブレータラックID
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDCalib = (Int32)this.numCalibratorRackIDFrom.Value;
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDCalib = (Int32)this.numCalibratorRackIDTo.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDCalib
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.RackIDDefinitionParameter.MinRackIDCalib)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(CarisX.Properties.Resources.STRING_LOG_MSG_057
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDCalib + CarisX.Properties.Resources.STRING_LOG_MSG_001);
                changeSamepleKind.Add(SampleKind.Calibrator);
            }
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDCalib
                     != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.RackIDDefinitionParameter.MaxRackIDCalib)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(CarisX.Properties.Resources.STRING_LOG_MSG_058
                     , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDCalib + CarisX.Properties.Resources.STRING_LOG_MSG_001);
                changeSamepleKind.Add(SampleKind.Calibrator);
            }
            // 精度管理検体ラックID
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDCtrl = (Int32)this.numControlRackIDFrom.Value;
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDCtrl = (Int32)this.numControlRackIDTo.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDCtrl
                     != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.RackIDDefinitionParameter.MinRackIDCtrl)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(CarisX.Properties.Resources.STRING_LOG_MSG_059
                     , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MinRackIDCtrl + CarisX.Properties.Resources.STRING_LOG_MSG_001);
                changeSamepleKind.Add(SampleKind.Control);
            }
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDCtrl
                     != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.RackIDDefinitionParameter.MaxRackIDCtrl)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(CarisX.Properties.Resources.STRING_LOG_MSG_060
                     , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter.MaxRackIDCtrl + CarisX.Properties.Resources.STRING_LOG_MSG_001);
                changeSamepleKind.Add(SampleKind.Control);
            }

            // XMLへ保存
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Save();

            this.DialogResult = DialogResult.OK;

            // ラック割り当て変更後通知
            Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.RackIdDefinitionChanged, changeSamepleKind.Distinct());

            this.Close();
        }

        /// <summary>
        /// Cancelボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にキャンセルを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// パラメータ変更履歴追加
        /// </summary>
        /// <remarks>
        /// パラメータ変更履歴を追加します
        /// </remarks>
        /// <param name="titleStr"></param>
        /// <param name="valueStr"></param>
        private void AddPramLogData(string titleStr, string valueStr)
        {
            String[] contents = new String[4];
            contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
            contents[1] = lblDialogTitle.Text;
            contents[2] = titleStr;
            contents[3] = valueStr;
            Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);
        }

        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <remarks>
        /// 入力チェックを行います
        /// </remarks>
        /// <returns></returns>
        private Boolean CheckValue()
        {
            Boolean ret = true;
            Infragistics.Win.UltraWinEditors.UltraNumericEditor ctrl = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();

            // 入力項目の大小チェック
            if ((Int32)numPatientRackIDFrom.Value > (Int32)numPatientRackIDTo.Value)
            {
                ctrl = numPatientRackIDFrom;
                ret = false;
            }
            else if ((Int32)numPatientRackIDModeSampleIdFrom.Value > (Int32)numPatientRackIDModeSampleIdTo.Value)
            {
                ctrl = numPatientRackIDModeSampleIdFrom;
                ret = false;
            }
            else if ((Int32)numCalibratorRackIDFrom.Value > (Int32)numCalibratorRackIDTo.Value)
            {
                ctrl = numCalibratorRackIDFrom;
                ret = false;
            }
            else if ((Int32)numControlRackIDFrom.Value > (Int32)numControlRackIDTo.Value)
            {
                ctrl = numControlRackIDFrom;
                ret = false;
            }

            if (!ret)
            {
                // エラーダイアログ表示
                DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_012, "", CarisX.Properties.Resources.STRING_DLG_TITLE_005, MessageDialogButtons.OK);
                // 対象コントロールをフォーカス
                ctrl.Focus();
                ctrl.SelectAll();
            }


            return ret;
        }

        #endregion
    }
}
