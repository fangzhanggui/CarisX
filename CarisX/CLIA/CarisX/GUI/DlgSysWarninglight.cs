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
    /// 警告灯使用有無ダイアログクラス
    /// </summary>
    public partial class DlgSysWarninglight : DlgCarisXBaseSys
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgSysWarninglight()
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
                return Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WarningLightParameter.Enable;
            }
            set
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WarningLightParameter.Enable = value;
                if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WarningLightParameter.Enable
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.WarningLightParameter.Enable)
                {
                    // パラメータ変更履歴登録
                    String[] contents = new String[4];
                    contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
                    contents[1] = CarisX.Properties.Resources.STRING_DLG_SYS_WARNING_LIGHT_000;
                    contents[2] = CarisX.Properties.Resources.STRING_DLG_SYS_WARNING_LIGHT_GBX_001;
                    contents[3] = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WarningLightParameter.Enable
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
            // 警告灯使用有無
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WarningLightParameter.Enable)
            {
                this.optUseWarningLight.CheckedIndex = 0;
            }
            else
            {
                this.optUseWarningLight.CheckedIndex = 1;
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WARNING_LIGHT_000;

            // グループボックス
            this.gbxUseWarningLight.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WARNING_LIGHT_GBX_001;

            // オプションボタン
            this.optUseWarningLight.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WARNING_LIGHT_OPT_001;
            this.optUseWarningLight.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WARNING_LIGHT_OPT_002;

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
            // 警告灯使用有無
            Boolean usableWarningLight;
            if (this.optUseWarningLight.CheckedIndex == 0)
            {
                usableWarningLight = true;
            }
            else
            {
                usableWarningLight = false;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WarningLightParameter.Enable = usableWarningLight;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WarningLightParameter.Enable
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.WarningLightParameter.Enable)
            {
                // パラメータ変更履歴登録
                String[] contents = new String[4];
                contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
                contents[1] = CarisX.Properties.Resources.STRING_DLG_SYS_WARNING_LIGHT_000;
                contents[2] = CarisX.Properties.Resources.STRING_DLG_SYS_WARNING_LIGHT_GBX_001;
                contents[3] = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WarningLightParameter.Enable
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
