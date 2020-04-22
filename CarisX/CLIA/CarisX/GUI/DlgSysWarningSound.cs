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
    /// エラー音、警告音ダイアログクラス
    /// </summary>
    public partial class DlgSysWarningSound : DlgCarisXBaseSys
    {

        #region [インスタンス変数定義]

        /// <summary>
        /// 音色（警告）
        /// </summary>
        private Dictionary<Int32, String> beepWarningList = new Dictionary<Int32, String>();

        /// <summary>
        /// 音色（エラー）
        /// </summary>
        private Dictionary<Int32, String> beepErrorList = new Dictionary<Int32, String>();

        /// <summary>
        /// 【IssuesNo:6】音色（消息）
        /// </summary>
        private Dictionary<Int32, String> beepHintList = new Dictionary<int, string>();

        /// <summary>
        /// バーコード種類 【IssuesNo:6】去除“None”,独立开关
        /// </summary>
        private Dictionary<ErrWarningBeepParameter.BeepVolumeKind, String> beepVolumeList = new Dictionary<ErrWarningBeepParameter.BeepVolumeKind, String>()
        {
                {ErrWarningBeepParameter.BeepVolumeKind.Small,String.Empty},
                {ErrWarningBeepParameter.BeepVolumeKind.Middle,String.Empty},
                {ErrWarningBeepParameter.BeepVolumeKind.Large,String.Empty}
        };

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgSysWarningSound()
        {
            InitializeComponent();

            // コンボボックスの中身設定
            // 音色（警告）
            for (Int32 i = ErrWarningBeepParameter.BEEP_WARNING_MIN; i <= ErrWarningBeepParameter.BEEP_WARNING_MAX; i++)
            {
                this.beepWarningList.Add(i, i.ToString());
            }
            // 音色（エラー）
            for (Int32 i = ErrWarningBeepParameter.BEEP_ERROR_MIN; i <= ErrWarningBeepParameter.BEEP_ERROR_MAX; i++)
            {
                this.beepErrorList.Add(i, i.ToString());
            }
            //【IssuesNo:6】增加提示音设置，用于提示错误等级为“消息”的故障
            for(Int32 i = ErrWarningBeepParameter.BEEP_ERROR_MIN; i <= ErrWarningBeepParameter.BEEP_ERROR_MAX; i++)
            {
                this.beepHintList.Add(i, i.ToString());
            }

            //【Issues:10】可以保存信息，但音量大小不可调节
            switch (Singleton<Oelco.CarisX.Status.SystemStatus>.Instance.Status)
            {
                // 分析中・サンプリング停止中・試薬交換開始状態はOK不可
                case Status.SystemStatusKind.WaitSlaveResponce:
                case Status.SystemStatusKind.Assay:
                case Status.SystemStatusKind.SamplingPause:
                case Status.SystemStatusKind.ReagentExchange:
                    this.cmbVolume.Enabled = false;
                    break;
                default:
                    this.cmbVolume.Enabled = true;
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
            // 音色（警告）
            this.cmbToneWarning.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.BeepWarning;
            // 音色（エラー）
            this.cmbToneError.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.BeepError;
            // 【IssuesNo:6】音色（消息）
            this.cmbToneHint.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.BeepHint;
            // 音量
            this.cmbVolume.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.BeepVolume;

            //【IssuesNo:10】去掉音量无的场景，为兼容以前的设置，当音量为无时，将音量设置为小，并且设置成不使用警告音。否则按照系统配置文件显示
            if(ErrWarningBeepParameter.BeepVolumeKind.None == Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.BeepVolume)
            {
                this.cmbVolume.Value = ErrWarningBeepParameter.BeepVolumeKind.Small;
                this.optUseWarningSound.CheckedIndex = 1;
            }
            else
            {
                this.cmbVolume.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.BeepVolume;
                if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.Enable)
                {
                    this.optUseWarningSound.CheckedIndex = 0;
                }
                else
                {
                    this.optUseWarningSound.CheckedIndex = 1;
                }
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WARNING_SOUND_000;

            //【IssuesNo:10】独立音量开关控制汉化
            this.gbxUseWarningSound.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WARNING_SOUND_GBX_002;
            this.optUseWarningSound.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WARNING_SOUND_OPT_001;
            this.optUseWarningSound.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WARNING_SOUND_OPT_002;

            // グループボックス
            this.gbxBeepSetup.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WARNING_SOUND_GBX_001;

            // ラベル
            this.lblToneWarning.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WARNING_SOUND_LBL_001;
            this.lblToneError.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WARNING_SOUND_LBL_002;
            //【IssuesNo:6】消息音汉化
            this.lblToneHint.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WARNING_SOUND_LBL_004;
            this.lblVolume.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WARNING_SOUND_LBL_003;

            // コンボボックス
            // 音色（警告）
            this.cmbToneWarning.Items.Clear();
            this.cmbToneWarning.DataSource = this.beepWarningList.ToList();
            this.cmbToneWarning.ValueMember = "Key";
            this.cmbToneWarning.DisplayMember = "Value";
            this.cmbToneWarning.SelectedIndex = 0;

            // 音色（エラー）
            this.cmbToneError.Items.Clear();
            this.cmbToneError.DataSource = this.beepErrorList.ToList();
            this.cmbToneError.ValueMember = "Key";
            this.cmbToneError.DisplayMember = "Value";
            this.cmbToneError.SelectedIndex = 0;

            //【IssuesNo:6】音色（消息）
            this.cmbToneHint.Items.Clear();
            this.cmbToneHint.DataSource = this.beepHintList.ToList();
            this.cmbToneHint.ValueMember = "Key";
            this.cmbToneHint.DisplayMember = "Value";
            this.cmbToneHint.SelectedIndex = 0;

            // 音量 【IssuesNo:10】去除“None”,音量开启/关闭独立控制
            this.beepVolumeList[ErrWarningBeepParameter.BeepVolumeKind.Small] = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WARNING_SOUND_CMB_002;
            this.beepVolumeList[ErrWarningBeepParameter.BeepVolumeKind.Middle] = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WARNING_SOUND_CMB_003;
            this.beepVolumeList[ErrWarningBeepParameter.BeepVolumeKind.Large] = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_WARNING_SOUND_CMB_004;
            this.cmbVolume.Items.Clear();
            this.cmbVolume.DataSource = this.beepVolumeList.ToList();
            this.cmbVolume.ValueMember = "Key";
            this.cmbVolume.DisplayMember = "Value";
            this.cmbVolume.SelectedIndex = 0;

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
            //【IssuesNo:10】音量有无独立控制
            Boolean usableWarningSound;
            if (this.optUseWarningSound.CheckedIndex == 0)
            {
                usableWarningSound = true;
            }
            else
            {
                usableWarningSound = false;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.Enable = usableWarningSound;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.Enable
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.ErrWarningBeepParameter.Enable)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxUseWarningSound.Text
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.Enable + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // 設定値取得、及びパラメータ設定
            // 音色（警告）
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.BeepWarning = (Int32)this.cmbToneWarning.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.BeepWarning
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.ErrWarningBeepParameter.BeepWarning)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblToneWarning.Text
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.BeepWarning + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // 音色（エラー）
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.BeepError = (Int32)this.cmbToneError.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.BeepError
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.ErrWarningBeepParameter.BeepError)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblToneError.Text
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.BeepError + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // 【IssuesNo:6】音色（消息）
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.BeepHint = (Int32)this.cmbToneHint.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.BeepHint
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.ErrWarningBeepParameter.BeepHint)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblToneHint.Text
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.BeepHint + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // 音量
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.BeepVolume = (ErrWarningBeepParameter.BeepVolumeKind)this.cmbVolume.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.BeepVolume
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.ErrWarningBeepParameter.BeepVolume)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblVolume.Text
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ErrWarningBeepParameter.BeepVolume + CarisX.Properties.Resources.STRING_LOG_MSG_001);
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
