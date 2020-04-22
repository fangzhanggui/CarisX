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
using Oelco.CarisX.Common;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// ラック移動方式ダイアログクラス
    /// </summary>
    public partial class DlgSysRackMovementMethod : DlgCarisXBaseSys
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// ラック移動方式
        /// </summary>
        private Dictionary<RackMovementMethodKind, String> rackMovementMethodList = new Dictionary<RackMovementMethodKind, String>()
        {
                {RackMovementMethodKind.Performance,Properties.Resources.STRING_DLG_SYS_RACKMOVEMENTMETHOD_002},
                {RackMovementMethodKind.Cost,Properties.Resources.STRING_DLG_SYS_RACKMOVEMENTMETHOD_003},
        };

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgSysRackMovementMethod()
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
            this.cmbRackMovementMethod.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackMovementMethodParameter.RackMovementMethod;
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
            this.Caption = Properties.Resources.STRING_DLG_SYS_RACKMOVEMENTMETHOD_000;

            // グループボックス
            this.gbxRackMovementMethod.Text = Properties.Resources.STRING_DLG_SYS_RACKMOVEMENTMETHOD_001;

            // コンボボックス
            this.cmbRackMovementMethod.Items.Clear();
            this.cmbRackMovementMethod.DataSource = this.rackMovementMethodList.ToList();
            this.cmbRackMovementMethod.ValueMember = "Key";
            this.cmbRackMovementMethod.DisplayMember = "Value";
            this.cmbRackMovementMethod.SelectedIndex = 0;

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
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackMovementMethodParameter.RackMovementMethod
                = (RackMovementMethodKind)this.cmbRackMovementMethod.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackMovementMethodParameter.RackMovementMethod
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.RackMovementMethodParameter.RackMovementMethod)
            {
                // パラメータ変更履歴登録
                String[] contents = new String[4];
                contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
                contents[1] = lblDialogTitle.Text;
                contents[2] = gbxRackMovementMethod.Text;
                contents[3] = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackMovementMethodParameter.RackMovementMethod
                                                                                  + Properties.Resources.STRING_LOG_MSG_001;
                Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, contents);
            }

            // XMLへ保存
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Save();

            //最終ラック移動位置をクリアする
            Singleton<PublicMemory>.Instance.lastRackMovePosition.Clear();
            Singleton<PublicMemory>.Instance.lastRackMovePosition.Add(0, 0);

            // 複数台構成の場合、ガイダンス表示を通知する
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected > 1)
            {
                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ShowReagentSetGuidance, null);
            }

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
