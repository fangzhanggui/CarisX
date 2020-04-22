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
using Oelco.Common.Comm;
using System.IO.Ports;
using Oelco.CarisX.Comm;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// ホスト設定ダイアログクラス
    /// </summary>
    public partial class DlgSysHost : DlgCarisXBaseSys
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// バーコード種類
        /// </summary>
        private Dictionary<BaudRate, String> baudrateKindList = new Dictionary<BaudRate, String>()
        {
                {BaudRate.Br2400,Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_CMB_000},
                {BaudRate.Br4800,Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_CMB_001},
                {BaudRate.Br9600,Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_CMB_002},
                {BaudRate.Br19200,Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_CMB_003},
                {BaudRate.Br38400,Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_CMB_004},
                {BaudRate.Br57600,Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_CMB_005},
                {BaudRate.Br115200,Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_CMB_006}
        };

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgSysHost()
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
                return Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Enable;
            }
            set
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Enable = value;
                if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Enable
                     != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.HostParameter.Enable)
                {
                    // 値が変更された場合、通知を行う
                    Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.UseOfHost, value);
                    // パラメータ変更履歴登録
                    this.AddPramLogData(Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_GBX_001
                     , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Enable + CarisX.Properties.Resources.STRING_LOG_MSG_001);
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
            // ホスト使用有無
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Enable)
            {
                this.optUseHostComputer.CheckedIndex = 0;
            }
            else
            {
                this.optUseHostComputer.CheckedIndex = 1;
            }

            // ボーレート
            this.cmbBaudrate.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Baudrate;

            // パリティ
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Parity == Parity.Odd)
            {
                this.optParity.CheckedIndex = 0;
            }
            else if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Parity == Parity.Even)
            {
                this.optParity.CheckedIndex = 1;
            }
            else
            {
                this.optParity.CheckedIndex = 2;
            }
            // データ長
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.DataLength == DataBits.Bit7)
            {
                this.optDataLength.CheckedIndex = 0;
            }
            else
            {
                this.optDataLength.CheckedIndex = 1;
            }
            // ストップビット
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.StopBit == StopBitKind.Bit1)
            {
                this.optStopBit.CheckedIndex = 0;
            }
            else
            {
                this.optStopBit.CheckedIndex = 1;
            }
            // 検体リアルタイム出力
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UsableRealtimeOutputSamp)
            {
                this.optRealTimeOutputSpecimen.CheckedIndex = 0;
            }
            else
            {
                this.optRealTimeOutputSpecimen.CheckedIndex = 1;
            }

            // 精度管理検体リアルタイム出力
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UsableRealtimeOutputCtrl)
            {
                this.optRealTimeOutputControl.CheckedIndex = 0;
            }
            else
            {
                this.optRealTimeOutputControl.CheckedIndex = 1;
            }
            // 送信遅延時間
            this.numIntervalTimeBetweenBytes.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.SendDelayTime.ToString();
            // 検体問い合わせ
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UseRealtimeSampleAsk)
            {
                this.optQueryAboutPatientWorksheet.CheckedIndex = 0;
            }
            else
            {
                this.optQueryAboutPatientWorksheet.CheckedIndex = 1;
            }

            this.ultraNumericEditorWaitTime.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UseRealtimeSampleAskWaitTime.ToString();
            // 将来対応の可能性
            //// 精度管理検体問い合わせ
            //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UsableAskCtrl)
            //{
            //    this.optQueryAboutControlPatientWorksheet.CheckedIndex = 0;
            //}
            //else
            //{
            //    this.optQueryAboutControlPatientWorksheet.CheckedIndex = 1;
            //}
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_000;

            // グループボックス
            this.gbxUseHostComputer.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_GBX_001;
            this.gbxBaudrate.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_GBX_002;
            this.gbxParity.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_GBX_003;
            this.gbxDataLength.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_GBX_004;
            this.gbxStopBit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_GBX_005;
            this.gbxRealTimeOutputSpecimen.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_GBX_006;
            this.gbxRealTimeOutputControl.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_GBX_008;
            this.gbxIntervalTimeBetweenBytes.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_GBX_009;
            this.gbxQueryAboutPatientWorksheet.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_GBX_010;
            this.gbxQueryAboutControlPatientWorksheet.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_GBX_011;
            this.ultraLabelWaitTime.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_GBX_012;
            this.ultraLabelUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_GBX_013;

            // オプションボタン
            this.optUseHostComputer.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_OPT_001;
            this.optUseHostComputer.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_OPT_002;
            this.optParity.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_OPT_003;
            this.optParity.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_OPT_004;
            this.optParity.Items[2].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_OPT_005;
            this.optDataLength.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_OPT_006;
            this.optDataLength.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_OPT_007;
            this.optStopBit.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_OPT_008;
            this.optStopBit.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_OPT_009;
            this.optRealTimeOutputSpecimen.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_OPT_010;
            this.optRealTimeOutputSpecimen.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_OPT_011;
            this.optRealTimeOutputControl.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_OPT_010;
            this.optRealTimeOutputControl.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_OPT_011;
            this.optQueryAboutPatientWorksheet.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_OPT_012;
            this.optQueryAboutPatientWorksheet.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_OPT_013;
            this.optQueryAboutControlPatientWorksheet.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_OPT_012;
            this.optQueryAboutControlPatientWorksheet.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_OPT_013;

            // ラベル
            this.lblIntervalTimeBetweenBytes.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_LBL_001;

            // コンボボックス
            this.cmbBaudrate.Items.Clear();
            this.cmbBaudrate.DataSource = this.baudrateKindList.ToList();
            this.cmbBaudrate.ValueMember = "Key";
            this.cmbBaudrate.DisplayMember = "Value";
            this.cmbBaudrate.SelectedIndex = 0;

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
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.HostParameterSetFlag = true;

            // 設定値取得、及びパラメータ設定
            // ホスト使用有無
            Boolean usableHost;
            if (this.optUseHostComputer.CheckedIndex == 0)
            {
                usableHost = true;
            }
            else
            {
                usableHost = false;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Enable = usableHost;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Enable
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.HostParameter.Enable)
            {
                // 値が変更された場合、通知を行う
                Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.UseOfHost, usableHost);
                // パラメータ変更履歴登録
                this.AddPramLogData(Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_GBX_001
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Enable + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // ボーレート
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Baudrate = (BaudRate)this.cmbBaudrate.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Baudrate
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.HostParameter.Baudrate)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxBaudrate.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Baudrate + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // パリティ
            Parity parity;
            if (this.optParity.CheckedIndex == 0)
            {
                parity = Parity.Odd;
            }
            else if (this.optParity.CheckedIndex == 1)
            {
                parity = Parity.Even;
            }
            else
            {
                parity = Parity.None;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Parity = parity;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Parity
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.HostParameter.Parity)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxParity.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Parity + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // データ長
            DataBits dataLenghtKind;
            if (this.optDataLength.CheckedIndex == 0)
            {
                dataLenghtKind = DataBits.Bit7;
            }
            else
            {
                dataLenghtKind = DataBits.Bit8;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.DataLength = dataLenghtKind;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.DataLength
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.HostParameter.DataLength)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxDataLength.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.DataLength + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // ストップビット
            StopBitKind stopBit;
            if (this.optStopBit.CheckedIndex == 0)
            {
                stopBit = StopBitKind.Bit1;
            }
            else
            {
                stopBit = StopBitKind.Bit2;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.StopBit = stopBit;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.StopBit
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.HostParameter.StopBit)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxStopBit.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.StopBit + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // 検体リアルタイム出力
            Boolean usableRealtimeOutputSamp;
            if (this.optRealTimeOutputSpecimen.CheckedIndex == 0)
            {
                usableRealtimeOutputSamp = true;
            }
            else
            {
                usableRealtimeOutputSamp = false;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UsableRealtimeOutputSamp = usableRealtimeOutputSamp;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UsableRealtimeOutputSamp
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.HostParameter.UsableRealtimeOutputSamp)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxRealTimeOutputSpecimen.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UsableRealtimeOutputSamp + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }


            // 精度管理検体リアルタイム出力
            Boolean usableRealtimeOutputCtrl;
            if (this.optRealTimeOutputControl.CheckedIndex == 0)
            {
                usableRealtimeOutputCtrl = true;
            }
            else
            {
                usableRealtimeOutputCtrl = false;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UsableRealtimeOutputCtrl = usableRealtimeOutputCtrl;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UsableRealtimeOutputCtrl
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.HostParameter.UsableRealtimeOutputCtrl)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxRealTimeOutputControl.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UsableRealtimeOutputCtrl + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // 送信遅延時間
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.SendDelayTime = (Int32)this.numIntervalTimeBetweenBytes.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.SendDelayTime
                     != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.HostParameter.SendDelayTime)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxIntervalTimeBetweenBytes.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.SendDelayTime + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // 検体問い合わせ
            Boolean usableAskSamp;
            if (this.optQueryAboutPatientWorksheet.CheckedIndex == 0)
            {
                usableAskSamp = true;
            }
            else
            {
                usableAskSamp = false;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UseRealtimeSampleAsk = usableAskSamp;
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UseRealtimeSampleAskWaitTime = (Int32)this.ultraNumericEditorWaitTime.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UseRealtimeSampleAsk
                     != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.HostParameter.UseRealtimeSampleAsk)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxQueryAboutPatientWorksheet.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UseRealtimeSampleAsk + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // 将来対応の可能性あり
            // 精度管理検体問い合わせ
            //Boolean usableAskCtrl;
            //if (this.optQueryAboutControlPatientWorksheet.CheckedIndex == 0)
            //{
            //    usableAskCtrl = true;
            //}
            //else
            //{
            //    usableAskCtrl = false;
            //}

            // 将来対応の可能性あり
            //Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UsableAskCtrl = usableAskCtrl;
            //if ( Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UsableAskCtrl
            //         != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.HostParameter.UsableAskCtrl )
            //{
            //    // パラメータ変更履歴登録
            //    this.AddPramLogData( gbxQueryAboutControlPatientWorksheet.Text
            //     , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UsableAskCtrl + CarisX.Properties.Resources.STRING_LOG_MSG_001 ); 
            //}

            // XMLへ保存
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Save();

            // ホスト接続へパラメータを設定
            Singleton<CarisXCommManager>.Instance.SetHostParameter(
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.ConvertSerialParameter());

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
            contents[1] = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_HOST_000;
            contents[2] = titleStr;
            contents[3] = valueStr;
            Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);
        }
        #endregion
    }
}
