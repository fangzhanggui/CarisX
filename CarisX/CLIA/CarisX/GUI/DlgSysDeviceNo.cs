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
    /// 装置No.ダイアログクラス
    /// </summary>
    public partial class DlgSysDeviceNo : DlgCarisXBaseSys
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 装置No.
        /// </summary>
        private Dictionary<Int32, String> deviceNoList = new Dictionary<Int32, String>()
        {
                {0,Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_DEVICE_NO_CMB_000},
                {1,Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_DEVICE_NO_CMB_001},
                {2,Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_DEVICE_NO_CMB_002},
                {3,Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_DEVICE_NO_CMB_003},
                {4,Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_DEVICE_NO_CMB_004},
                {5,Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_DEVICE_NO_CMB_005},
                {6,Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_DEVICE_NO_CMB_006},
                {7,Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_DEVICE_NO_CMB_007},
                {8,Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_DEVICE_NO_CMB_008},
                {9,Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_DEVICE_NO_CMB_009}
        };

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgSysDeviceNo()
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
            this.cmbDeviceNo.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.DeviceNoParameter.DeviceNo;
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_DEVICE_NO_000;

            // グループボックス
            this.gbxDeviceNo.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_DEVICE_NO_GBX_001;

            // コンボボックス
            this.cmbDeviceNo.Items.Clear();
            this.cmbDeviceNo.DataSource = this.deviceNoList.ToList();
            this.cmbDeviceNo.ValueMember = "Key";
            this.cmbDeviceNo.DisplayMember = "Value";
            this.cmbDeviceNo.SelectedIndex = 0;

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
            // 設定値取得しパラメータ設定
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.DeviceNoParameter.DeviceNo = (Int32)this.cmbDeviceNo.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.DeviceNoParameter.DeviceNo
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.DeviceNoParameter.DeviceNo)
            {
                // パラメータ変更履歴登録
                String[] contents = new String[4];
                contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
                contents[1] = lblDialogTitle.Text;
                contents[2] = gbxDeviceNo.Text;
                contents[3] = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.DeviceNoParameter.DeviceNo
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
