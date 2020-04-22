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
using Oelco.CarisX.Comm;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 検体バーコードリーダー使用有無ダイアログクラス
    /// </summary>
    public partial class DlgSysSampleBC : DlgCarisXBaseSys
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// バーコード種類
        /// </summary>
        private Dictionary<SampleBCRParameter.BarcodeKind, String> barcodeTypeList = new Dictionary<SampleBCRParameter.BarcodeKind, String>()
        {
                {SampleBCRParameter.BarcodeKind.Type1,String.Empty},
                {SampleBCRParameter.BarcodeKind.Type2,String.Empty},
                {SampleBCRParameter.BarcodeKind.Type3,String.Empty},
                {SampleBCRParameter.BarcodeKind.Type4,String.Empty}
        };

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgSysSampleBC()
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

        #region [プロパティ]

        /// <summary>
        /// 使用・未使用の取得・設定（設定の場合、保存もする）
        /// </summary>
        public override Boolean UseConfig
        {
            get
            {
                return Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.Enable;
            }
            set
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.Enable = value;
                if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.Enable
                       != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.SampleBCRParameter.Enable)
                {
                    // パラメータ変更履歴登録
                    this.AddPramLogData(Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_GBX_001
                         , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.Enable + CarisX.Properties.Resources.STRING_LOG_MSG_001);
                }
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Save();
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
            // 使用有無
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.Enable)
            {
                this.optUseOfSampleBC.CheckedIndex = 0;
            }
            else
            {
                this.optUseOfSampleBC.CheckedIndex = 1;
            }

            // 検体IDを固定長
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableRackIDSampFixedLength)
            {
                this.optFixedNumPatientID.CheckedIndex = 0;
            }
            else
            {
                this.optFixedNumPatientID.CheckedIndex = 1;
            }

            // 検体ID桁数
            this.numNumOfPatientID.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.RackIDSampDigit.ToString();

            // C/Dキャラクタ転送
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableCDCharTrans)
            {
                this.optCDCharTransmission.CheckedIndex = 0;
            }
            else
            {
                this.optCDCharTransmission.CheckedIndex = 1;
            }

            // C/Dチェック
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableCDCheck)
            {
                this.optCDCheck.CheckedIndex = 0;
            }
            else
            {
                this.optCDCheck.CheckedIndex = 1;
            }

            // ST/SP転送
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableSTSPTrans)
            {
                this.optSTSPCharTransmission.CheckedIndex = 0;
            }
            else
            {
                this.optSTSPCharTransmission.CheckedIndex = 1;
            }

            // バーコード種類
            // (ValueといってもここではKeyを指すので注意！)
            this.cmbBarcodeType.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.SelectBCKind;
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_000;

            // グループボックス
            this.gbxUseOfSampleBC.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_GBX_001;
            this.gbxFixedNumPatientID.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_GBX_002;
            this.gbxNumOfPatientID.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_GBX_003;
            this.gbxCDCharTransmission.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_GBX_004;
            this.gbxCDCheck.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_GBX_005;
            this.gbxSTSPCharTransmission.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_GBX_006;
            this.gbxBarcodeType.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_GBX_007;

            // オプションボタン
            this.optUseOfSampleBC.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_OPT_001;
            this.optUseOfSampleBC.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_OPT_002;
            this.optFixedNumPatientID.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_OPT_001;
            this.optFixedNumPatientID.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_OPT_002;
            this.optCDCharTransmission.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_OPT_001;
            this.optCDCharTransmission.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_OPT_002;
            this.optCDCheck.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_OPT_001;
            this.optCDCheck.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_OPT_002;
            this.optSTSPCharTransmission.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_OPT_001;
            this.optSTSPCharTransmission.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_OPT_002;

            // ラベル
            this.lblComment.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_LBL_001;

            // コンボボックス
            this.barcodeTypeList[SampleBCRParameter.BarcodeKind.Type1] = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_CMB_001;
            this.barcodeTypeList[SampleBCRParameter.BarcodeKind.Type2] = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_CMB_002;
            this.barcodeTypeList[SampleBCRParameter.BarcodeKind.Type3] = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_CMB_003;
            this.barcodeTypeList[SampleBCRParameter.BarcodeKind.Type4] = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_CMB_004;
            this.cmbBarcodeType.Items.Clear();
            this.cmbBarcodeType.DataSource = this.barcodeTypeList.ToList();
            this.cmbBarcodeType.ValueMember = "Key";
            this.cmbBarcodeType.DisplayMember = "Value";
            this.cmbBarcodeType.SelectedIndex = 0;

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
            // 設定値取得、及びパラメータ設定
            // 使用有無            
            if (this.optUseOfSampleBC.CheckedIndex == 0)
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.Enable = true;
            }
            else
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.Enable = false;
            }
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.Enable
                       != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.SampleBCRParameter.Enable)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxUseOfSampleBC.Text
                     , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.Enable + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // 検体IDを固定長
            if (this.optFixedNumPatientID.CheckedIndex == 0)
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableRackIDSampFixedLength = true;
            }
            else
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableRackIDSampFixedLength = false;
            }
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableRackIDSampFixedLength
                       != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.SampleBCRParameter.UsableRackIDSampFixedLength)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxFixedNumPatientID.Text
                     , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableRackIDSampFixedLength + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }


            // 検体ID桁数
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.RackIDSampDigit = (Int32)this.numNumOfPatientID.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.RackIDSampDigit
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.SampleBCRParameter.RackIDSampDigit)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxNumOfPatientID.Text
                     , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.RackIDSampDigit + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // C/Dキャラクタ転送
            if (this.optCDCharTransmission.CheckedIndex == 0)
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableCDCharTrans = true;
            }
            else
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableCDCharTrans = false;
            }
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableCDCharTrans
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.SampleBCRParameter.UsableCDCharTrans)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxCDCharTransmission.Text
                     , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableCDCharTrans + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // C/Dチェック
            if (this.optCDCheck.CheckedIndex == 0)
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableCDCheck = true;
            }
            else
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableCDCheck = false;
            }
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableCDCheck
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.SampleBCRParameter.UsableCDCheck)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxCDCheck.Text
                     , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableCDCheck + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // ST/SP転送
            if (this.optSTSPCharTransmission.CheckedIndex == 0)
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableSTSPTrans = true;
            }
            else
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableSTSPTrans = false;
            }
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableSTSPTrans
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.SampleBCRParameter.UsableSTSPTrans)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxSTSPCharTransmission.Text
                     , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableSTSPTrans + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // バーコード種類
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.SelectBCKind = (SampleBCRParameter.BarcodeKind)this.cmbBarcodeType.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.SelectBCKind
                     != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.SampleBCRParameter.SelectBCKind)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxBarcodeType.Text
                     , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.SelectBCKind + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // XMLへ保存
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Save();

            // ラック搬送へ検体バーコード設定コマンドを送信する
            RackTransferCommCommand_0082 cmd0082 = new RackTransferCommCommand_0082()
            {
                CDCheck = Convert.ToByte(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableCDCheck),
                CDTrans = Convert.ToByte(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableCDCharTrans),
                STSPTrans = Convert.ToByte(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.UsableSTSPTrans),
            };

            switch (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleBCRParameter.SelectBCKind)
            {
                case SampleBCRParameter.BarcodeKind.Type1:
                    cmd0082.Kind1 = RackTransferCommCommand_0082.Kind.NW7ITFCODE39CODE128;
                    break;
                case SampleBCRParameter.BarcodeKind.Type2:
                    cmd0082.Kind1 = RackTransferCommCommand_0082.Kind.ITF2of5CODE39CODE128;
                    break;
                case SampleBCRParameter.BarcodeKind.Type3:
                    cmd0082.Kind1 = RackTransferCommCommand_0082.Kind.NW7ITF2of5CODE128;
                    break;
                case SampleBCRParameter.BarcodeKind.Type4:
                    cmd0082.Kind1 = RackTransferCommCommand_0082.Kind.NW7ITFCODE392of5;
                    break;
            }

            Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0082);

            this.DialogResult = DialogResult.OK;
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
            contents[1] = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SAMPLE_BC_000;
            contents[2] = titleStr;
            contents[3] = valueStr;
            Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);
        }

        #endregion
    }
}
