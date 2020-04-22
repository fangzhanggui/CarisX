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
    /// 試薬不足時の分析の状態ダイアログクラス
    /// </summary>
    public partial class DlgSysReagentShortSupply : DlgCarisXBaseSys
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgSysReagentShortSupply()
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
            // 試薬不足時の分析の状態
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ProcessAtReagentShortageParameter.ReagShortAssayStatus)
            {
                this.optSampling.CheckedIndex = 0;
            }
            else
            {
                this.optSampling.CheckedIndex = 1;
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_REAGENT_SHORT_SUPPLY_000;

            // グループボックス
            this.gbxSampling.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_REAGENT_SHORT_SUPPLY_GBX_001;

            // オプションボタン
            this.optSampling.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_REAGENT_SHORT_SUPPLY_OPT_001;
            this.optSampling.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_REAGENT_SHORT_SUPPLY_OPT_002;

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
            // 試薬不足時の分析の状態
            Boolean reagShortAssayStatus;
            if (this.optSampling.CheckedIndex == 0)
            {
                reagShortAssayStatus = true;
            }
            else
            {
                reagShortAssayStatus = false;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ProcessAtReagentShortageParameter.ReagShortAssayStatus = reagShortAssayStatus;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ProcessAtReagentShortageParameter.ReagShortAssayStatus
                     != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.ProcessAtReagentShortageParameter.ReagShortAssayStatus)
            {
                // パラメータ変更履歴登録
                String[] contents = new String[4];
                contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
                contents[1] = lblDialogTitle.Text;
                contents[2] = gbxSampling.Text;
                contents[3] = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ProcessAtReagentShortageParameter.ReagShortAssayStatus
                                                                                  + CarisX.Properties.Resources.STRING_LOG_MSG_001;
                Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);
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

        #endregion
    }
}
