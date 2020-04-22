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
    /// 分析モジュール接続台数ダイアログクラス
    /// </summary>
    public partial class DlgSysAssayModuleConnect : DlgCarisXBaseSys
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 分析モジュール接続台数
        /// </summary>
        private Dictionary<Int32, String> moduleConnectList = new Dictionary<Int32, String>()
        {
                {1,Properties.Resources.STRING_DLG_SYS_ASSAYMODULECONNECT_001},
                {2,Properties.Resources.STRING_DLG_SYS_ASSAYMODULECONNECT_002},
                {3,Properties.Resources.STRING_DLG_SYS_ASSAYMODULECONNECT_003},
                {4,Properties.Resources.STRING_DLG_SYS_ASSAYMODULECONNECT_004},
        };

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgSysAssayModuleConnect()
        {
            InitializeComponent();
            switch (Singleton<Status.SystemStatus>.Instance.Status)
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
            this.cmbAssayModuleConnect.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;
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
            this.Caption = Properties.Resources.STRING_DLG_SYS_ASSAYMODULECONNECT_000;

            // グループボックス
            this.gbxAssayModuleConnect.Text = Properties.Resources.STRING_DLG_SYS_ASSAYMODULECONNECT_005;

            // コンボボックス
            this.cmbAssayModuleConnect.Items.Clear();
            this.cmbAssayModuleConnect.DataSource = this.moduleConnectList.ToList();
            this.cmbAssayModuleConnect.ValueMember = "Key";
            this.cmbAssayModuleConnect.DisplayMember = "Value";
            this.cmbAssayModuleConnect.SelectedIndex = 0;

            // ボタン
            this.btnOK.Text = Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Properties.Resources.STRING_COMMON_003;
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
            // 設定値取得しパラメータ設定
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected = (Int32)this.cmbAssayModuleConnect.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.AssayModuleConnectParameter.NumOfConnected)
            {
                // パラメータ変更履歴登録
                String[] contents = new String[4];
                contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
                contents[1] = lblDialogTitle.Text;
                contents[2] = gbxAssayModuleConnect.Text; 
                contents[3] = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected
                                                                                  + Properties.Resources.STRING_LOG_MSG_001;
                Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, contents);
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
