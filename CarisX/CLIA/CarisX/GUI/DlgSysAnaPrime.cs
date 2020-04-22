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

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// プライムダイアログクラス
    /// </summary>
    public partial class DlgSysAnaPrime : DlgCarisXBaseSys
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgSysAnaPrime()
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
            // 希釈液プライム回数
            this.numNumOfTimesDilutedLiquid.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountDilu.ToString();
            // R1プライム回数
            this.numNumOfTimesR1.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountR1.ToString();
            // R2プライム回数
            this.numNumOfTimesR2.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountR2.ToString();
            // B/F1プライム回数
            this.numNumOfTimesBF1.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountBF1.ToString();
            // B/F2プライム回数
            this.numNumOfTimesBF2.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountBF2.ToString();
            // プレトリガプライム回数
            this.numNumOfTimesPretrigger.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountPreTrig.ToString();
            // トリガプライム回数
            this.numNumOfTimesTrigger.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountTrig.ToString();
            // 希釈液プライム量（μL）
            this.numPrimingVolumeDilutedLiquid.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeDilu.ToString();
            // R1プライム量（μL）
            this.numPrimingVolumeR1.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeR1.ToString();
            // R2プライム量（μL）
            this.numPrimingVolumeR2.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeR2.ToString();
            // B/F1プライム量（μL）
            this.numPrimingVolumeBF1.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeBF1.ToString();
            // B/F2プライム量（μL）
            this.numPrimingVolumeBF2.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeBF2.ToString();
            // プレトリガプライム量（μL）
            this.numPrimingVolumePretrigger.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumePreTrig.ToString();
            // トリガプライム量（μL）
            this.numPrimingVolumeTrigger.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeTrig.ToString();

            // ボタン
            this.btnOK.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_000;

            // ラベル
            this.lblNumOfTimesDilutedLiquid.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_001;
            this.lblNumOfTimesR1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_002;
            this.lblNumOfTimesR2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_003;
            this.lblNumOfTimesBF1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_004;
            this.lblNumOfTimesBF2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_005;
            this.lblNumOfTimesPretrigger.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_006;
            this.lblNumOfTimesTrigger.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_007;
            this.lblPrimingVolumeDilutedLiquid.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_008;
            this.lblPrimingVolumeR1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_009;
            this.lblPrimingVolumeR2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_010;
            this.lblPrimingVolumeBF1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_011;
            this.lblPrimingVolumeBF2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_012;
            this.lblPrimingVolumePretrigger.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_013;
            this.lblPrimingVolumeTrigger.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_014;
            this.lblNumOfTimesDilutedLiquidUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_015;
            this.lblNumOfTimesR1Unit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_015;
            this.lblNumOfTimesR2Unit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_015;
            this.lblNumOfTimesBF1Unit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_015;
            this.lblNumOfTimesBF2Unit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_015;
            this.lblNumOfTimesPretriggerUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_015;
            this.lblNumOfTimesTriggerUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_015;
            this.lblPrimingVolumeDilutedLiquidUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_016;
            this.lblPrimingVolumeR1Unit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_016;
            this.lblPrimingVolumeR2Unit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_016;
            this.lblPrimingVolumeBF1Unit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_016;
            this.lblPrimingVolumeBF2Unit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_016;
            this.lblPrimingVolumePretriggerUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_016;
            this.lblPrimingVolumeTriggerUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANA_PRIME_LBL_016;
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
            // 希釈液プライム回数
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountDilu = (Int32)this.numNumOfTimesDilutedLiquid.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountDilu
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.PrimeParameter.PrimeCountDilu)
            {
                // パラメータ変更履歴登録                
                this.AddPramLogData(lblNumOfTimesDilutedLiquid.Text
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountDilu + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // R1プライム回数
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountR1 = (Int32)this.numNumOfTimesR1.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountR1
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.PrimeParameter.PrimeCountR1)
            {
                // パラメータ変更履歴登録             
                this.AddPramLogData(lblNumOfTimesR1.Text
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountR1 + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // R2プライム回数
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountR2 = (Int32)this.numNumOfTimesR2.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountR2
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.PrimeParameter.PrimeCountR2)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblNumOfTimesR2.Text
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountR2 + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // B/F1プライム回数
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountBF1 = (Int32)this.numNumOfTimesBF1.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountBF1
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.PrimeParameter.PrimeCountBF1)
            {
                // パラメータ変更履歴登録    
                this.AddPramLogData(lblNumOfTimesBF1.Text
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountBF1 + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // B/F2プライム回数
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountBF2 = (Int32)this.numNumOfTimesBF2.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountBF2
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.PrimeParameter.PrimeCountBF2)
            {
                // パラメータ変更履歴登録    
                this.AddPramLogData(lblNumOfTimesBF2.Text
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountBF2 + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // プレトリガプライム回数
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountPreTrig = (Int32)this.numNumOfTimesPretrigger.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountPreTrig
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.PrimeParameter.PrimeCountPreTrig)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblNumOfTimesPretrigger.Text
                   , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountPreTrig + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // トリガプライム回数
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountTrig = (Int32)this.numNumOfTimesTrigger.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountTrig
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.PrimeParameter.PrimeCountTrig)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblNumOfTimesTrigger.Text
                  , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeCountTrig + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // 希釈液プライム量（μL）
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeDilu = (Int32)this.numPrimingVolumeDilutedLiquid.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeDilu
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.PrimeParameter.PrimeVolumeDilu)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblPrimingVolumeDilutedLiquid.Text
                  , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeDilu + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // R1プライム量（μL）
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeR1 = (Int32)this.numPrimingVolumeR1.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeR1
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.PrimeParameter.PrimeVolumeR1)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblPrimingVolumeR1.Text
                  , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeR1 + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // R2プライム量（μL）
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeR2 = (Int32)this.numPrimingVolumeR2.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeR2
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.PrimeParameter.PrimeVolumeR2)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblPrimingVolumeR2.Text
                  , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeR2 + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // B/F1プライム量（μL）
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeBF1 = (Int32)this.numPrimingVolumeBF1.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeBF1
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.PrimeParameter.PrimeVolumeBF1)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblPrimingVolumeBF1.Text
                  , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeBF1 + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // B/F2プライム量（μL）
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeBF2 = (Int32)this.numPrimingVolumeBF2.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeBF2
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.PrimeParameter.PrimeVolumeBF2)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblPrimingVolumeBF2.Text
                  , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeBF2 + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // プレトリガプライム量（μL）
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumePreTrig = (Int32)this.numPrimingVolumePretrigger.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumePreTrig
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.PrimeParameter.PrimeVolumePreTrig)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblPrimingVolumePretrigger.Text
                  , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumePreTrig + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // トリガプライム量（μL）
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeTrig = (Int32)this.numPrimingVolumeTrigger.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeTrig
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.PrimeParameter.PrimeVolumeTrig)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblPrimingVolumeTrigger.Text
                  , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrimeParameter.PrimeVolumeTrig + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // XMLへ保存
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Save();

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
            contents[1] = lblDialogTitle.Text;
            contents[2] = titleStr;
            contents[3] = valueStr;
            Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);
        }

        #endregion
    }
}
