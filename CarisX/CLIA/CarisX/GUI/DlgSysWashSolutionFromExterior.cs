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
    /// 洗浄液の外部からの供給使用有無ダイアログクラス
    /// </summary>
    public partial class DlgSysWashSolutionFromExterior : DlgCarisXBaseSys
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
		public DlgSysWashSolutionFromExterior()
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
                return Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashSolutionFromExterior.Enable;
            }
            set
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashSolutionFromExterior.Enable = value;
                if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashSolutionFromExterior.Enable
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.WashSolutionFromExterior.Enable)
                {
                    //【IssuesNo:2】发送有无使用外部洗液状态的通知
                    Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.WashSolutionExteriorUsable, value);

                    // パラメータ変更履歴登録
                    String[] contents = new String[4];
                    contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
                    contents[1] = CarisX.Properties.Resources.STRING_DLG_SYSWASHSOLUTIONFROMEXTERIOR_000;
                    contents[2] = CarisX.Properties.Resources.STRING_DLG_SYSWASHSOLUTIONFROMEXTERIOR_GBX_001;
                    contents[3] = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashSolutionFromExterior.Enable
                                                                                      + CarisX.Properties.Resources.STRING_LOG_MSG_001;
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);
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
            // 洗浄液の外部からの供給使用有無
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashSolutionFromExterior.Enable)
            {
                this.optUseWashSolutionFromExterior.CheckedIndex = 0;
            }
            else
            {
                this.optUseWashSolutionFromExterior.CheckedIndex = 1;
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_SYSWASHSOLUTIONFROMEXTERIOR_000;

            // グループボックス
            this.gbxWashSolutionFromExterior.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYSWASHSOLUTIONFROMEXTERIOR_GBX_001;

            // オプションボタン
            this.optUseWashSolutionFromExterior.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYSWASHSOLUTIONFROMEXTERIOR_OPT_001;
            this.optUseWashSolutionFromExterior.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYSWASHSOLUTIONFROMEXTERIOR_OPT_002;

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
            // 洗浄液の外部からの供給使用有無
            Boolean usableWashSolutionFromExterior;
            if (this.optUseWashSolutionFromExterior.CheckedIndex == 0)
            {
                usableWashSolutionFromExterior = true;
            }
            else
            {
                usableWashSolutionFromExterior = false;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashSolutionFromExterior.Enable = usableWashSolutionFromExterior;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashSolutionFromExterior.Enable
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.WashSolutionFromExterior.Enable)
            {
                //【IssuesNo:2】修复使用外部洗液时，试剂界面按钮没有联动刷新问题
                Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.WashSolutionExteriorUsable, usableWashSolutionFromExterior);

                // パラメータ変更履歴登録
                String[] contents = new String[4];
                contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
                contents[1] = CarisX.Properties.Resources.STRING_DLG_SYSWASHSOLUTIONFROMEXTERIOR_000;
                contents[2] = CarisX.Properties.Resources.STRING_DLG_SYSWASHSOLUTIONFROMEXTERIOR_GBX_001;
                contents[3] = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashSolutionFromExterior.Enable
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
