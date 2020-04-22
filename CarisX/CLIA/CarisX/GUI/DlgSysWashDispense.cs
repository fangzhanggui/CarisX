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
    /// 洗浄、分注ダイアログクラス
    /// </summary>
    public partial class DlgSysWashDispense : DlgCarisXBaseSys
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgSysWashDispense()
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
            // 洗浄液
            this.numWashSolution.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashDispVolParameter.DispVolWash.ToString();
            // プレトリガ
            this.numPretrigger.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashDispVolParameter.DispVolPreTrig.ToString();
            // トリガ
            this.numTrigger.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashDispVolParameter.DispVolTrig.ToString();
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WASH_DISPENSE_000;

            // ラベル
            this.lblWashSolution.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WASH_DISPENSE_LBL_001;
            this.lblPretrigger.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WASH_DISPENSE_LBL_002;
            this.lblTrigger.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WASH_DISPENSE_LBL_003;
            this.lblWashSolutionUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WASH_DISPENSE_LBL_004;
            this.lblPretriggerUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WASH_DISPENSE_LBL_004;
            this.lblTriggerUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WASH_DISPENSE_LBL_004;

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
            // 洗浄液
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashDispVolParameter.DispVolWash = (Int32)this.numWashSolution.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashDispVolParameter.DispVolWash
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.WashDispVolParameter.DispVolWash)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblWashSolution.Text
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashDispVolParameter.DispVolWash + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // プレトリガ
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashDispVolParameter.DispVolPreTrig = (Int32)this.numPretrigger.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashDispVolParameter.DispVolPreTrig
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.WashDispVolParameter.DispVolPreTrig)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblPretrigger.Text
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashDispVolParameter.DispVolPreTrig + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // トリガ
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashDispVolParameter.DispVolTrig = (Int32)this.numTrigger.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashDispVolParameter.DispVolTrig
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.WashDispVolParameter.DispVolTrig)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblTrigger.Text
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashDispVolParameter.DispVolTrig + CarisX.Properties.Resources.STRING_LOG_MSG_001);
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
