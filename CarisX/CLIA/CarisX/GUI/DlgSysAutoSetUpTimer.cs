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
    /// 自動起動タイマー使用有無ダイアログクラス
    /// </summary>
    public partial class DlgSysAutoSetUpTimer : DlgCarisXBaseSys
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgSysAutoSetUpTimer()
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
                return Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.Enable;
            }
            set
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.Enable = value;
                if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.Enable
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.AutoStartupTimerParameter.Enable)
                {
                    // パラメータ変更履歴登録
                    this.AddPramLogData(Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_AUTO_SETUP_TIMER_GBX_001
                      , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.Enable + CarisX.Properties.Resources.STRING_LOG_MSG_001);
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
            // 自動起動タイマー使用有無
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.Enable)
            {
                this.optUseAutomaticStartupTimer.CheckedIndex = 0;
            }
            else
            {
                this.optUseAutomaticStartupTimer.CheckedIndex = 1;
            }

            // 曜日（月曜）
            if ((Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.SelectDayOfWeek & AutoStartupTimerParameter.DayOfWeek.Mon) != 0)
            {
                chkDayOfWeek1.Checked = true;
            }
            else
            {
                chkDayOfWeek1.Checked = false;
            }
            // 曜日（火曜）
            if ((Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.SelectDayOfWeek & AutoStartupTimerParameter.DayOfWeek.Tue) != 0)
            {
                chkDayOfWeek2.Checked = true;
            }
            else
            {
                chkDayOfWeek2.Checked = false;
            }
            // 曜日（水曜）
            if ((Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.SelectDayOfWeek & AutoStartupTimerParameter.DayOfWeek.Wed) != 0)
            {
                chkDayOfWeek3.Checked = true;
            }
            else
            {
                chkDayOfWeek3.Checked = false;
            }
            // 曜日（木曜）
            if ((Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.SelectDayOfWeek & AutoStartupTimerParameter.DayOfWeek.Thu) != 0)
            {
                chkDayOfWeek4.Checked = true;
            }
            else
            {
                chkDayOfWeek4.Checked = false;
            }
            // 曜日（金曜）
            if ((Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.SelectDayOfWeek & AutoStartupTimerParameter.DayOfWeek.Fri) != 0)
            {
                chkDayOfWeek5.Checked = true;
            }
            else
            {
                chkDayOfWeek5.Checked = false;
            }
            // 曜日（土曜）
            if ((Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.SelectDayOfWeek & AutoStartupTimerParameter.DayOfWeek.Sat) != 0)
            {
                chkDayOfWeek6.Checked = true;
            }
            else
            {
                chkDayOfWeek6.Checked = false;
            }
            // 曜日（日曜）
            if ((Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.SelectDayOfWeek & AutoStartupTimerParameter.DayOfWeek.Sun) != 0)
            {
                chkDayOfWeek7.Checked = true;
            }
            else
            {
                chkDayOfWeek7.Checked = false;
            }
            // 時間（時）
            this.numTimeHour.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.AutoStartupHour.ToString();
            // 時間（分）
            this.numTimeMinutes.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.AutoStartupMinute.ToString();
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_AUTO_SETUP_TIMER_000;

            // グループボックス
            this.gbxUseAutomaticStartupTimer.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_AUTO_SETUP_TIMER_GBX_001;
            this.gbxDayOfWeek.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_AUTO_SETUP_TIMER_GBX_002;
            this.gbxTime.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_AUTO_SETUP_TIMER_GBX_003;

            // オプションボタン
            this.optUseAutomaticStartupTimer.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_AUTO_SETUP_TIMER_OPT_001;
            this.optUseAutomaticStartupTimer.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_AUTO_SETUP_TIMER_OPT_002;

            // ラベル
            this.lblTimeHour.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_AUTO_SETUP_TIMER_LBL_001;
            this.lblTimeMinutes.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_AUTO_SETUP_TIMER_LBL_002;

            // チェックボックス
            this.chkDayOfWeek1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_AUTO_SETUP_TIMER_CHK_001;
            this.chkDayOfWeek2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_AUTO_SETUP_TIMER_CHK_002;
            this.chkDayOfWeek3.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_AUTO_SETUP_TIMER_CHK_003;
            this.chkDayOfWeek4.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_AUTO_SETUP_TIMER_CHK_004;
            this.chkDayOfWeek5.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_AUTO_SETUP_TIMER_CHK_005;
            this.chkDayOfWeek6.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_AUTO_SETUP_TIMER_CHK_006;
            this.chkDayOfWeek7.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_AUTO_SETUP_TIMER_CHK_007;

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
            // 自動起動タイマー使用有無
            Boolean usableAutoStartupTimer;
            if (this.optUseAutomaticStartupTimer.CheckedIndex == 0)
            {
                usableAutoStartupTimer = true;
            }
            else
            {
                usableAutoStartupTimer = false;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.Enable = usableAutoStartupTimer;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.Enable
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.AutoStartupTimerParameter.Enable)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxUseAutomaticStartupTimer.Text
                  , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.Enable + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // 曜日（月曜）
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.SelectDayOfWeek = 0;
            if (chkDayOfWeek1.Checked)
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.SelectDayOfWeek |= AutoStartupTimerParameter.DayOfWeek.Mon;
            }
            // 曜日（火曜）
            if (chkDayOfWeek2.Checked)
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.SelectDayOfWeek |= AutoStartupTimerParameter.DayOfWeek.Tue;
            }
            // 曜日（水曜）
            if (chkDayOfWeek3.Checked)
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.SelectDayOfWeek |= AutoStartupTimerParameter.DayOfWeek.Wed;
            }
            // 曜日（木曜）
            if (chkDayOfWeek4.Checked)
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.SelectDayOfWeek |= AutoStartupTimerParameter.DayOfWeek.Thu;
            }
            // 曜日（金曜）
            if (chkDayOfWeek5.Checked)
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.SelectDayOfWeek |= AutoStartupTimerParameter.DayOfWeek.Fri;
            }
            // 曜日（土曜）
            if (chkDayOfWeek6.Checked)
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.SelectDayOfWeek |= AutoStartupTimerParameter.DayOfWeek.Sat;
            }
            // 曜日（日曜）
            if (chkDayOfWeek7.Checked)
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.SelectDayOfWeek |= AutoStartupTimerParameter.DayOfWeek.Sun;
            }
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.SelectDayOfWeek
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.AutoStartupTimerParameter.SelectDayOfWeek)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxDayOfWeek.Text
                  , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.SelectDayOfWeek + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // 時間（時）
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.AutoStartupHour = (Int32)this.numTimeHour.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.AutoStartupHour
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.AutoStartupTimerParameter.AutoStartupHour)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxTime.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + lblTimeHour.Text
                  , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.AutoStartupHour + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // 時間（分）
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.AutoStartupMinute = (Int32)this.numTimeMinutes.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.AutoStartupMinute
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.AutoStartupTimerParameter.AutoStartupMinute)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxTime.Text + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + lblTimeMinutes.Text
                  , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.AutoStartupMinute + CarisX.Properties.Resources.STRING_LOG_MSG_001);
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
            contents[1] = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_AUTO_SETUP_TIMER_000;
            contents[2] = titleStr;
            contents[3] = valueStr;
            Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);
        }
        #endregion

    }
}
