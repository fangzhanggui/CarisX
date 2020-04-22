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
using Oelco.CarisX.Utility;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// Content:IoT設定ダイアログクラス;Add by:Fang;Date:2019-01-03
    /// </summary>
    public partial class DlgSysIoT : DlgCarisXBaseSys
    {

        IoTHub ioTHub;

        #region [仪器型号]

        /// <summary>
        /// 仪器種類
        /// </summary>
        private Dictionary<Int16, String> ModelTypeList = new Dictionary<Int16, String>()
        {
                {1,Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_CMB_000},
                {2,Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_CMB_001},

        };

        private List<FormBase> childFormList
        {
            get
            {
                List<FormBase> childs = new List<FormBase>()
                {
                    Singleton<FormSystemLog>.Instance
                };

                return childs;
            }
        }

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgSysIoT()
        {
            InitializeComponent();

            // 測定日時(開始日、終了日)の初期化
            this.btnCreateTimeFrom.Text = DateTime.Today.ToShortDateString();
            this.btnCreateTimeFrom.Tag = DateTime.Today;
            this.btnCreateTimeTo.Text = DateTime.Today.ToShortDateString();
            this.btnCreateTimeTo.Tag = DateTime.Today.Add(TimeSpan.FromDays(1) - TimeSpan.FromSeconds(1)); // 23:59:59

            if (Singleton<CarisXUserLevelManager>.Instance.NowUserLevel == UserLevel.Level4 ||
                Singleton<CarisXUserLevelManager>.Instance.NowUserLevel == UserLevel.Level5)
            {
                this.teIoTConnStr.Enabled = true;
                this.teMachineSerialNumber.Enabled = true;
                this.cmbModelID.Enabled = true;
                this.btnSendMeasureData.Visible = true;
                this.btnSendError.Visible = true;
                this.btnSendDueData.Visible = true;
                this.btnSendFiles.Visible = true;
                this.gbxCreateLogfilesByDate.Visible = true;
            }
            else
            {
                this.teIoTConnStr.Enabled = false;
                this.teMachineSerialNumber.Enabled = false;
                this.cmbModelID.Enabled = false;
                this.btnSendMeasureData.Visible = false;
                this.btnSendError.Visible = false;
                this.btnSendDueData.Visible = false;
                this.btnSendFiles.Visible = false;
                this.gbxCreateLogfilesByDate.Visible = false;
            }

            switch (Singleton<Oelco.CarisX.Status.SystemStatus>.Instance.Status)
            {
                // 分析中・サンプリング停止中・試薬交換開始状態はOK不可
                case Status.SystemStatusKind.WaitSlaveResponce:
                case Status.SystemStatusKind.Assay:
                case Status.SystemStatusKind.SamplingPause:
                case Status.SystemStatusKind.ToEndAssay:
                case Status.SystemStatusKind.ReagentExchange:
                    this.btnOK.Enabled = false;
                    break;
                default:
                    this.btnOK.Enabled = true;
                    break;
            }

            this.btnReconnect.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_027;

            //加载日志
            Singleton<ErrorLogDB>.Instance.LoadDB();
            Singleton<OperationLogDB>.Instance.LoadDB();
            Singleton<ParameterChangeLogDB>.Instance.LoadDB();
            Singleton<AnalyzeLogDB>.Instance.LoadDB();
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
                return Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Enable;
            }
            set
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Enable = value;
                if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Enable
                     != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.IoTParameter.Enable)
                {
                    // 値が変更された場合、通知を行う
                    Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.UseOfIoT, value);
                    // パラメータ変更履歴登録
                    this.AddPramLogData(Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_001
                     , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Enable + CarisX.Properties.Resources.STRING_LOG_MSG_001);
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
            // IoT 使用有無
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Enable)
            {
                this.optUseIoT.CheckedIndex = 0;
            }
            else
            {
                this.optUseIoT.CheckedIndex = 1;
            }

            // IoT 连接字符串
            this.teIoTConnStr.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.IoTConnectionStr;

            // 仪器设备号码
            this.teMachineSerialNumber.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.MachineSerialNumber;

            // 机型 如Caris:1;wan+:2
            this.cmbModelID.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.ModelId;

            // 测试数据实时上传管理
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.UploadMeasurementData)
            {
                this.optRealTimeUploadMeasurementData.CheckedIndex = 0;
            }
            else
            {
                this.optRealTimeUploadMeasurementData.CheckedIndex = 1;
            }

            // 错误信息实时上传管理
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.UploadErrorCommand)
            {
                this.optRealTimeUploadErrorCommand.CheckedIndex = 0;
            }
            else
            {
                this.optRealTimeUploadErrorCommand.CheckedIndex = 1;
            }

            // 时间信息实时上传管理
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.UploadDueDate)
            {
                this.optRealTimeUploadDueDate.CheckedIndex = 0;
            }
            else
            {
                this.optRealTimeUploadDueDate.CheckedIndex = 1;
            }

            // 系统日志上传管理
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.UploadLogfileAftAnalys)
            {
                this.optUploadLogfileAfterAnalysising.CheckedIndex = 0;
            }
            else
            {
                this.optUploadLogfileAfterAnalysising.CheckedIndex = 1;
            }

            this.btnReconnect.Enabled = false;
            this.btnReconnect.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_026;
            Connect();

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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_000;

            // グループボックス
            this.gbxUseIoT.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_001;
            this.gbxMachineSerialNumber.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_002;
            this.gbxIotHubConnectionString.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_003;
            this.gbxModelID.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_004;
            this.gbxRealTimeUploadMeasurementData.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_005;
            this.gbxRealTimeUploadErrorCommand.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_006;
            this.gbxRealTimeUploadDueDate.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_007;
            this.gbxCreateLogfilesByDate.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_010;
            this.gbxUploadLogfileAfterAnalysising.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_008;
            this.gbxReconnected.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_009;

            // オプションボタン
            this.optUseIoT.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_OPT_001;
            this.optUseIoT.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_OPT_002;
            this.optRealTimeUploadMeasurementData.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_OPT_001;
            this.optRealTimeUploadMeasurementData.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_OPT_002;
            this.optRealTimeUploadErrorCommand.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_OPT_001;
            this.optRealTimeUploadErrorCommand.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_OPT_002;
            this.optRealTimeUploadDueDate.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_OPT_001;
            this.optRealTimeUploadDueDate.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_OPT_002;
            this.optUploadLogfileAfterAnalysising.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_OPT_001;
            this.optUploadLogfileAfterAnalysising.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_OPT_002;

            // ModelID Combox 初始化
            this.cmbModelID.Items.Clear();
            this.cmbModelID.DataSource = this.ModelTypeList.ToList();
            this.cmbModelID.ValueMember = "Key";
            this.cmbModelID.DisplayMember = "Value";
            this.cmbModelID.SelectedIndex = 0;

            // ボタン
            this.btnOK.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;
            this.btnSendMeasureData.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_022;
            this.btnSendError.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_022;
            this.btnSendDueData.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_022;
            this.btnSendFiles.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_022;
            this.btnClear.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_026;
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
            // IoT使用有無
            Boolean usableIoT;
            if (this.optUseIoT.CheckedIndex == 0)
            {
                usableIoT = true;
            }
            else
            {
                usableIoT = false;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Enable = usableIoT;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Enable
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.IoTParameter.Enable)
            {
                if (!usableIoT)
                {
                    Disconnect();
                }
                // 値が変更された場合、通知を行う
                Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.UseOfIoT, usableIoT);
                // パラメータ変更履歴登録
                this.AddPramLogData(Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_001
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Enable + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // 是否改变连接字符串
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.IoTConnectionStr = this.teIoTConnStr.Text;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.IoTConnectionStr
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.IoTParameter.IoTConnectionStr)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxIotHubConnectionString.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.IoTConnectionStr + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // 仪器编号
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.MachineSerialNumber = (Int32)this.teMachineSerialNumber.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.MachineSerialNumber
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.IoTParameter.MachineSerialNumber)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxMachineSerialNumber.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.MachineSerialNumber + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // 机型
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.ModelId = short.Parse(this.cmbModelID.Value.ToString());
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.ModelId
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.IoTParameter.ModelId)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxModelID.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.ModelId + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // 是否上传测试数据
            Boolean usableUploadMeasurementData;
            if (this.optRealTimeUploadMeasurementData.CheckedIndex == 0)
            {
                usableUploadMeasurementData = true;
            }
            else
            {
                usableUploadMeasurementData = false;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.UploadMeasurementData = usableUploadMeasurementData;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.UploadMeasurementData
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.IoTParameter.UploadMeasurementData)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxRealTimeUploadMeasurementData.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.UploadMeasurementData + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // 是否上传错误命令
            Boolean usableUploadErrorCommand;
            if (this.optRealTimeUploadErrorCommand.CheckedIndex == 0)
            {
                usableUploadErrorCommand = true;
            }
            else
            {
                usableUploadErrorCommand = false;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.UploadErrorCommand = usableUploadErrorCommand;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.UploadErrorCommand
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.IoTParameter.UploadErrorCommand)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxRealTimeUploadErrorCommand.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.UploadErrorCommand + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // 是否上传日期数据
            Boolean usableUploadDueDate;
            if (this.optRealTimeUploadDueDate.CheckedIndex == 0)
            {
                usableUploadDueDate = true;
            }
            else
            {
                usableUploadDueDate = false;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.UploadDueDate = usableUploadDueDate;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.UploadDueDate
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.IoTParameter.UploadDueDate)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxRealTimeUploadDueDate.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.UploadDueDate + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // 是否上传系统日志（分析终止）
            Boolean usableUploadLogfiles;
            if (this.optUploadLogfileAfterAnalysising.CheckedIndex == 0)
            {
                usableUploadLogfiles = true;
            }
            else
            {
                usableUploadLogfiles = false;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.UploadLogfileAftAnalys = usableUploadLogfiles;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.UploadLogfileAftAnalys
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.IoTParameter.UploadLogfileAftAnalys)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxUploadLogfileAfterAnalysising.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.UploadLogfileAftAnalys + CarisX.Properties.Resources.STRING_LOG_MSG_001);
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
            contents[1] = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_000;
            contents[2] = titleStr;
            contents[3] = valueStr;
            Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);
        }
        #endregion

        private void btnReconnect_Click(object sender, EventArgs e)
        {
            if (btnReconnect.Text == Oelco.CarisX.Properties.Resources.STRING_COMMON_027)
            {
                Connect();
            }
            else
            {
                Disconnect();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.teLog.Text = string.Empty;
        }

        private void btnSendMeasureData_Click(object sender, EventArgs e)
        {
            SendMeasureData();
        }

        private void btnSendDueData_Click(object sender, EventArgs e)
        {
            SendDueData();
        }

        private void btnSendError_Click(object sender, EventArgs e)
        {
            SendErrorData();
        }

        private void btnSendFiles_Click(object sender, EventArgs e)
        {
            SendFiles();
        }

        private void UnenableSendBtn()
        {
            btnReconnect.Enabled = false;
            btnSendMeasureData.Enabled = false;
            btnSendError.Enabled = false;
            btnSendDueData.Enabled = false;
            btnSendFiles.Enabled = false;
            btnCreate.Enabled = false;
        }

        private void EnableSendBtn()
        {
            btnReconnect.Enabled = true;
            btnSendMeasureData.Enabled = true;
            btnSendError.Enabled = true;
            btnSendDueData.Enabled = true;
            btnSendFiles.Enabled = true;
            btnCreate.Enabled = true;
        }

        /// <summary>
        /// 连接
        /// </summary>
        private async void Connect()
        {
            try
            {
                string sIotConnStr = this.teIoTConnStr.Text;
                string sMachineSerialNumber = this.teMachineSerialNumber.Value.ToString();
                ioTHub = new IoTHub(sIotConnStr, sMachineSerialNumber);
                await ioTHub.ConnectIotHub();
                Singleton<CarisXCommIoTManager>.Instance.ConnectIoT(sIotConnStr, sMachineSerialNumber);
                btnReconnect.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_024;
                EnableSendBtn();
                teLog.Text = ("IoT connect successfully!" + "\r\n\r\n");
            }
            catch (Exception ex)
            {
                teLog.Text = "Failed to Connect : " + ex.Message + "\r\n\r\n";
                btnReconnect.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_027;
            }
            btnReconnect.Enabled = true;
            this.UseWaitCursor = false;

        }

        /// <summary>
        /// 断开连接
        /// </summary>
        private async void Disconnect()
        {
            try
            {
                if (ioTHub != null)
                {
                    await ioTHub.DisconnectIotHub();
                }
                Singleton<CarisXCommIoTManager>.Instance.DisConnectIoT();
                btnReconnect.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_027;
                teLog.Text = ("IoT disconnect successfully!" + "\r\n\r\n");
                UnenableSendBtn();
            }
            catch (Exception ex)
            {
                teLog.Text = "Failed to disconnect : " + ex.Message + "\r\n\r\n";
            }
            btnReconnect.Enabled = true;
            this.UseWaitCursor = false;

        }

        /// <summary>
        /// 模拟发送系统文件结果;Add by:Fang;Date:2019-2-28
        /// </summary>
        public async void SendFiles()
        {
            try
            {
                teLog.Text += await PackageLogfilesByDate(DateTime.Now);
                UnenableSendBtn();
                btnReconnect.Enabled = false;
                foreach (string fileName in Directory.GetFiles(CarisXConst.PathTemp))
                {
                    teLog.Text += string.Format("Send {0} ..." + "\r\n\r\n", Path.GetFileName(fileName));
                    await ioTHub.SendToBlobAsync(fileName);
                }
                teLog.Text += "Finish to send logfiles!" + "\r\n\r\n";

            }
            catch (Exception ex)
            {
                teLog.Text += String.Format("Failed to send logfiles : {0}" + "\r\n\r\n", ex.Message);
            }
            EnableSendBtn();
            btnReconnect.Enabled = true;
        }

        /// <summary>
        /// 模拟发送测试结果;Add by:Fang;Date:2019-2-28
        /// </summary>
        public async void SendMeasureData()
        {
            try
            {
                IoTCommCommand_0010 measureData = new IoTCommCommand_0010();
                measureData.Model_id = short.Parse(this.cmbModelID.Value.ToString());
                measureData.Machine_serial_number = (Int32)this.teMachineSerialNumber.Value;
                measureData.Command_id = 10;
                measureData.Sample_meas_kind = 0;
                measureData.Receipt_number = 0;
                measureData.Sequence_no = 0;
                measureData.Rack_id = string.Empty;
                measureData.Rack_position = 0;
                measureData.Specimen_material_type = 0;
                measureData.Sample_lot = string.Empty;
                measureData.Control_name = string.Empty;
                measureData.Manual_dilution = 0;
                measureData.Reagent_item = "Test";
                measureData.Count_value = 0;
                measureData.Concentration = 0;
                measureData.Judgment = string.Empty;
                measureData.Remark = 0;
                measureData.Auto_dilution_ratio = 0;
                measureData.Reagent_lot_no = string.Empty;
                measureData.Pretrigger_lot_no = string.Empty;
                measureData.Trigger_lot_no = string.Empty;
                measureData.Calibcurve_datetime = DateTime.Now;
                measureData.Measuring_datetime = DateTime.Now;
                measureData.S1 = 0;
                measureData.S2 = 0;
                measureData.S3 = 0;
                measureData.Dispense_volume = 0;
                measureData.Sample_aspiration = 0;
                measureData.M_Reagent_port_no = 0;
                measureData.M_Sample_position = 0;
                measureData.R1_reagent_port_no = 0;
                measureData.R1_sample_position = 0;
                measureData.R2_reagent_port_no = 0;
                measureData.R2_sample_position = 0;
                measureData.Temperature_1 = 0;
                measureData.Temperature_2 = 0;
                measureData.Temperature_3 = 0;
                measureData.Temperature_4 = 0;
                measureData.Temperature_5 = 0;
                measureData.Temperature_6 = 0;
                measureData.Temperature_7 = 0;
                string message = CarisXSubFunction.CreateJSon(measureData);
                await ioTHub.SendMessagetoDevice(message);
                teLog.Text += String.Format("Success to send message : {0}" + "\r\n\r\n", message);

            }
            catch (Exception ex)
            {
                teLog.Text += String.Format("Failed to send message : {0}" + "\r\n\r\n", ex.Message);
            }
        }

        /// <summary>
        /// 模拟发送故障信息;Add by:Fang;Date:2019-2-28
        /// </summary>
        public async void SendErrorData()
        {
            try
            {
                IoTCommConmand_0020 sendData = new IoTCommConmand_0020();
                sendData.Model_id = short.Parse(this.cmbModelID.Value.ToString());
                sendData.Machine_serial_number = (Int32)this.teMachineSerialNumber.Value;
                sendData.Command_id = 20;
                sendData.Acquired_datetime = DateTime.Now;
                sendData.Error_code = 0;
                sendData.Error_arg = 0;
                sendData.Error_level = 1;
                sendData.Reagent_item = string.Empty;
                sendData.Contents = "Test"; ;
                string message = CarisXSubFunction.CreateJSon(sendData);
                await ioTHub.SendMessagetoDevice(message);
                teLog.Text += String.Format("Success to send message : {0}" + "\r\n\r\n", message);

            }
            catch (Exception ex)
            {
                teLog.Text += String.Format("Failed to send message : {0}" + "\r\n\r\n", ex.Message);
            }
        }

        /// <summary>
        /// 模拟发送仪器初始日期;Add by:Fang;Date:2019-2-28
        /// </summary>
        public async void SendDueData()
        {
            try
            {
                IoTCommConmand_0030 sendData = new IoTCommConmand_0030();
                sendData.Model_id = short.Parse(this.cmbModelID.Value.ToString());
                sendData.Machine_serial_number = (Int32)this.teMachineSerialNumber.Value;
                sendData.Command_id = 30;
                if (DateTime.Compare(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Delivery_date, DateTime.MaxValue) == 0)
                {
                    sendData.Datetime = DateTime.Now;
                }
                else
                {
                    sendData.Datetime = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Delivery_date;
                }
                string message = CarisXSubFunction.CreateJSon(sendData);
                await ioTHub.SendMessagetoDevice(message);
                teLog.Text += String.Format("Success to send message : {0}" + "\r\n\r\n", message);
            }
            catch (Exception ex)
            {
                teLog.Text += String.Format("Failed to send message : {0}" + "\r\n\r\n", ex.Message);
            }
        }

        private void btnCreateTimeFrom_Click(object sender, EventArgs e)
        {
            DateTime date;
            DialogResult result = DlgDateSelect.Show(String.Empty, out date, (DateTime)this.btnCreateTimeFrom.Tag);
            if (DialogResult.OK == result)
            {
                this.btnCreateTimeFrom.Text = date.ToShortDateString();
                this.btnCreateTimeFrom.Tag = date;
            }

        }

        private void btnCreateTimeTo_Click(object sender, EventArgs e)
        {
            DateTime date;
            DialogResult result = DlgDateSelect.Show(String.Empty, out date, (DateTime)this.btnCreateTimeTo.Tag);
            if (DialogResult.OK == result)
            {
                this.btnCreateTimeTo.Text = date.ToShortDateString();
                this.btnCreateTimeTo.Tag = date;
            }

        }

        private async void btnCreate_Click(object sender, EventArgs e)
        {
            DateTime t1 = (DateTime)this.btnCreateTimeFrom.Tag;
            DateTime t2 = (DateTime)this.btnCreateTimeTo.Tag;
            UnenableSendBtn();

            if(DateTime.Compare(t1, t2) > 0)
            {
                teLog.Text += "The start date must be earlier than or equal to the end date!" + "\r\n\r\n";
                EnableSendBtn();
                return;
            }

            while (DateTime.Compare(t1, t2) <= 0)
            {
                teLog.Text += await PackageLogfilesByDate(t1);
                t1 = t1.AddDays(1);
            }

            teLog.Text += "Finish to create logfiles!" + "\r\n\r\n";
            EnableSendBtn();

        }

        private async Task<string> PackageLogfilesByDate(DateTime targetDate)
        {
            string sResult = string.Empty;
            await Task.Run(() =>
            {
                try
                {
                    if (!Directory.Exists(CarisXConst.PathTemp))
                    {
                        Directory.CreateDirectory(CarisXConst.PathTemp);
                    }

                    string filePath = string.Empty;
                    filePath = CarisXConst.PathTemp + "\\" + CarisXConst.EXPORT_CSV_ERRORLOG + targetDate.ToString("yyMMdd") + ".csv ";
                    Singleton<DataHelper>.Instance.ExportCsv(
                                Singleton<ErrorLogDB>.Instance.GetErrorLog().FindAll(data => data.WriteTime.Date.Equals(targetDate.Date)),
                                ((FormSystemLog)this.childFormList.Single((form) => form is FormSystemLog)).ErrorlogGridColumns,
                                filePath,
                                null);
                    filePath = CarisXConst.PathTemp + "\\" + CarisXConst.EXPORT_CSV_OPERATIONLOG + targetDate.ToString("yyMMdd") + ".csv ";
                    Singleton<DataHelper>.Instance.ExportCsv(
                                Singleton<OperationLogDB>.Instance.GetOperationLog().FindAll(data => data.WriteTime.Date.Equals(targetDate.Date)),
                                ((FormSystemLog)this.childFormList.Single((form) => form is FormSystemLog)).OperationlogGridColumns,
                                filePath,
                                null);
                    filePath = CarisXConst.PathTemp + "\\" + CarisXConst.EXPORT_CSV_PARAMETERCHANGELOG + targetDate.ToString("yyMMdd") + ".csv ";
                    Singleton<DataHelper>.Instance.ExportCsv(
                                Singleton<ParameterChangeLogDB>.Instance.GetParameterChangeLog().FindAll(data => data.WriteTime.Date.Equals(targetDate.Date)),
                                ((FormSystemLog)this.childFormList.Single((form) => form is FormSystemLog)).ParameterChangeLogGridColumns,
                                filePath,
                                null);
                    filePath = CarisXConst.PathTemp + "\\" + CarisXConst.EXPORT_CSV_ASSAYLOG + targetDate.ToString("yyMMdd") + ".csv ";
                    Singleton<DataHelper>.Instance.ExportCsv(
                                Singleton<AnalyzeLogDB>.Instance.GetAnalyzeLog().FindAll(data => data.WriteTime.Date.Equals(targetDate.Date)),
                                ((FormSystemLog)this.childFormList.Single((form) => form is FormSystemLog)).AnalyzelogGridColumns,
                                filePath,
                                null);

                    CarisXSubFunction.CopySysLog(CarisXConst.PathDebug, CarisXConst.PathTemp, targetDate, String.Format("{0}_{1}.log", "*" + "debuglog", "*"));
                    CarisXSubFunction.CopySysLog(CarisXConst.PathOnline, CarisXConst.PathTemp, targetDate, String.Format("{0}_{1}.log", "*" + "online", "*"));
                    CarisXSubFunction.CopySysLog(CarisXConst.PathLog, CarisXConst.PathTemp, targetDate, String.Format("{0}.txt", "*" + "sendfile"));
                    CarisXSubFunction.CopySysLog(CarisXConst.PathLog, CarisXConst.PathTemp, targetDate, String.Format("{0}.txt", "*" + "revfile"));
                    string logName = CarisXSubFunction.CreateLogName(this.teMachineSerialNumber.Value.ToString(), this.cmbModelID.Value.ToString(), targetDate.ToString("yyMMdd"), CarisXConst.EXPORT_ZIP_LOGFILES);
                    string sZipName = CarisXConst.PathTemp + "\\" + logName + ".zip";
                    CarisXSubFunction.ZipFiles(CarisXConst.PathTemp, sZipName);
                    sResult = string.Format("Create {0}..." + "\r\n\r\n", Path.GetFileName(sZipName));

                }
                catch (Exception ex)
                {
                    sResult = String.Format("Failed to create logfiles : {0}" + "\r\n\r\n", ex.Message);
                }
            });
            return sResult;
        }
    }
}
