using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.CarisX.Const;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Utility;
using System.IO;


namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// Content:IoT設定ダイアログクラス;Add by:Fang;Date:2019-01-03
    /// 【IssuesNo:16】针对联机版本的界面处理
    /// </summary>
    public partial class DlgSysIoT : DlgCarisXBaseSys
    {

        IoTHub ioTHub;

		//以前设计是Caris200 => 1;Wan200+ =>2，现在固定设备类型为Wan200+,因此这里是2,窗体上也取消机型选择的Control
        const Int32 MODEL_ID = 2;

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgSysIoT()
        {
            InitializeComponent();

            //IOT连接状态设定
            switch (Singleton<CarisXCommIoTManager>.Instance.IoTStatus)
            {
                case IoTStatusKind.OnLine:
                    this.btnConnection.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_024;
                    this.lbConnecttionStatus.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_LABEL_012;
                    this.lbConnecttionStatus.Appearance.ForeColor = Color.Green;
                    break;
                default:
                    this.btnConnection.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_023;
                    this.lbConnecttionStatus.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_LABEL_011;
                    this.lbConnecttionStatus.Appearance.ForeColor = Color.Red;
                    break;
            }

            //联机数量设定
            SetVisibleModuleTab(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected);

            //权限状态设定
            if (Singleton<CarisXUserLevelManager>.Instance.NowUserLevel == UserLevel.Level4 ||
                Singleton<CarisXUserLevelManager>.Instance.NowUserLevel == UserLevel.Level5)
            {
                this.tabIOTSetting.Tabs[1].Enabled = true;
                this.tabIOTSetting.Tabs[2].Enabled = true;
                this.gbxRealTimeUploadMeasurementData.Visible = true;
                this.gbxRealTimeUploadDueDate.Visible = true;

            }
            else
            {
                this.tabIOTSetting.Tabs[1].Enabled = false;
                this.tabIOTSetting.Tabs[2].Enabled = false;
                this.gbxRealTimeUploadMeasurementData.Visible = false;
                this.gbxRealTimeUploadDueDate.Visible = false;

            }

            //系统状态设定
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

            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.IoTStatus, this.UpdateIoTStatus);

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


            // IoT 连接字符串
            this.teConnKey.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.IoTConnectionKey;
            this.teTestConnKey.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.IoTConnectionKey;

            // 分析单元1设备号
            this.teSlave1No.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave1No;
            this.teTestNo.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave1No;

            // 分析单元2设备号
            this.teSlave2No.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave2No;

            // 分析单元3设备号
            this.teSlave3No.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave3No;

            // 分析单元4设备号
            this.teSlave4No.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave4No;

            // 測定日時(開始日、終了日)の初期化
            this.btnCreateTimeFrom.Text = DateTime.Today.ToShortDateString();
            this.btnCreateTimeFrom.Tag = DateTime.Today;
            this.btnCreateTimeTo.Text = DateTime.Today.ToShortDateString();
            this.btnCreateTimeTo.Tag = DateTime.Today.Add(TimeSpan.FromDays(1) - TimeSpan.FromSeconds(1)); // 23:59:59

            SetButtonEnable(false);
            this.btnTestConn.Enabled = true;
        }

        /// <summary>
        /// 根据分析单元的连接数量设定【详细】页面中的显示内容
        /// </summary>
        /// <param name="ConnectCount"></param>
        private void SetVisibleModuleTab(int ConnectCount)
        {
            //ConnectCount以下のModuleタブだけ表示する
            this.lbSlave1.Visible = ((Int32)ModuleTabIndex.Slave1 <= ConnectCount);
            this.lbSlave2.Visible = ((Int32)ModuleTabIndex.Slave2 <= ConnectCount);
            this.lbSlave3.Visible = ((Int32)ModuleTabIndex.Slave3 <= ConnectCount);
            this.lbSlave4.Visible = ((Int32)ModuleTabIndex.Slave4 <= ConnectCount);
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
            this.gbxConnecttionStatus.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_011;
            this.gbxRealTimeUploadMeasurementData.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_005;
            this.gbxRealTimeUploadErrorCommand.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_006;
            this.gbxRealTimeUploadDueDate.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_007;
            this.gbxIOTConnSetting.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_003;
            this.gbxConnectionTest.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_003;
            this.gbxSendTest.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_012;
            this.gbxSendTest.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_010;
            this.gbxUploadLogfileAfterAnalysising.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_008;
            this.gbxConnectionLog.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_009;

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

            // ボタン
            this.btnOK.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;
            this.btnSendMeasureData.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_022;
            this.btnSendErrorData.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_022;
            this.btnSendDueData.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_022;
            this.btnSendLogFiles.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_022;
            this.btnClear.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_025;
            this.btnTestConn.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_023;

            // label
            this.lbConnectionKey.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_LABEL_001;
            this.lbSlave1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_LABEL_003;
            this.lbSlave2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_LABEL_004;
            this.lbSlave3.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_LABEL_005;
            this.lbSlave4.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_LABEL_006;
            this.lbTestConnKey.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_LABEL_001;
            this.lbTestNo.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_LABEL_002;
            this.lbMeasureData.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_LABEL_007;
            this.lbErrorData.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_LABEL_008;
            this.lbDueDateData.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_LABEL_009;
            this.lbSystemFiles.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_LABEL_010;

            //Tab
            this.tabIOTSetting.Tabs[0].Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_TAB_001;
            this.tabIOTSetting.Tabs[1].Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_TAB_002;
            this.tabIOTSetting.Tabs[2].Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_TAB_003;

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
                    Singleton<CarisXCommIoTManager>.Instance.DisConnectIoT();
                }
                // 値が変更された場合、通知を行う
                Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.UseOfIoT, usableIoT);
                // パラメータ変更履歴登録
                this.AddPramLogData(Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_GBX_001
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Enable + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // 是否改变连接字符串
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.IoTConnectionKey = this.teConnKey.Text;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.IoTConnectionKey
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.IoTParameter.IoTConnectionKey)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxIOTConnSetting.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.IoTConnectionKey + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // 分析单元1编号
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave1No = (Int32)this.teSlave1No.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave1No
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.IoTParameter.Slave1No)
            {
                //パラメータ変更履歴登録
                this.AddPramLogData(gbxIOTConnSetting.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave1No + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // 分析单元2编号
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave2No = (Int32)this.teSlave2No.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave2No
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.IoTParameter.Slave2No)
            {
                //パラメータ変更履歴登録
                this.AddPramLogData(gbxIOTConnSetting.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave2No + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // 分析单元3编号
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave3No = (Int32)this.teSlave3No.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave3No
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.IoTParameter.Slave3No)
            {
                //パラメータ変更履歴登録
                this.AddPramLogData(gbxIOTConnSetting.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave3No + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // 分析单元4编号
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave4No = (Int32)this.teSlave4No.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave4No
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.IoTParameter.Slave4No)
            {
                //パラメータ変更履歴登録
                this.AddPramLogData(gbxIOTConnSetting.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave4No + CarisX.Properties.Resources.STRING_LOG_MSG_001);
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

        private void btnConnection_Click(object sender, EventArgs e)
        {
            if (this.btnConnection.Text == Oelco.CarisX.Properties.Resources.STRING_COMMON_023)
            {
                Singleton<CarisXCommIoTManager>.Instance.ConnectIoT(this.teConnKey.Text, this.teSlave1No.Value.ToString());
            }
            else
            {
                Singleton<CarisXCommIoTManager>.Instance.DisConnectIoT();
            }

            this.UseWaitCursor = false;
        }

        #region 调试Tab按钮功能
        private async void btnTestConn_Click(object sender, EventArgs e)
        {
            ResponseResult result = new ResponseResult("", ResponseStatus.Failure);
            if (btnTestConn.Text == Oelco.CarisX.Properties.Resources.STRING_COMMON_023)
            {
                string sIotConnKey = this.teTestConnKey.Text;
                string sConnNo = this.teTestNo.Value.ToString();
                ioTHub = new IoTHub(sIotConnKey, sConnNo);
                result = await ioTHub.ConnectIotHub();
                this.btnTestConn.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_024;
                SetButtonEnable(true);
            }
            else
            {
                if (ioTHub != null)
                {
                    result = await ioTHub.DisconnectIotHub();
                }
                btnTestConn.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_023;
                SetButtonEnable(false);
            }

            teLog.Text = result.Message + Environment.NewLine;
            btnTestConn.Enabled = true;
            this.UseWaitCursor = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.teLog.Text = string.Empty;
        }

        private async void btnSendMeasureData_Click(object sender, EventArgs e)
        {
            ResponseResult result = new ResponseResult("", ResponseStatus.Failure);
            IoTCommCommand_0010 measureData = new IoTCommCommand_0010();
            measureData.Model_id = MODEL_ID;
            measureData.Machine_serial_number = (Int32)this.teTestNo.Value;
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
            result = await ioTHub.SendMessagetoDevice(message);
            teLog.Text += result.Message + Environment.NewLine;
        }

        private async void btnSendDueData_Click(object sender, EventArgs e)
        {
            ResponseResult result = new ResponseResult("", ResponseStatus.Failure);
            IoTCommConmand_0030 sendData = new IoTCommConmand_0030();
            sendData.Model_id = MODEL_ID;
            sendData.Machine_serial_number = (Int32)this.teTestNo.Value;
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
            result = await ioTHub.SendMessagetoDevice(message);
            teLog.Text += result.Message + Environment.NewLine;
        }

        private async void btnSendError_Click(object sender, EventArgs e)
        {
            ResponseResult result = new ResponseResult("", ResponseStatus.Failure);
            IoTCommConmand_0020 sendData = new IoTCommConmand_0020();
            sendData.Model_id = MODEL_ID;
            sendData.Machine_serial_number = (Int32)this.teTestNo.Value;
            sendData.Command_id = 20;
            sendData.Acquired_datetime = DateTime.Now;
            sendData.Error_code = 0;
            sendData.Error_arg = 0;
            sendData.Error_level = 1;
            sendData.Reagent_item = string.Empty;
            sendData.Contents = "Test"; ;
            string message = CarisXSubFunction.CreateJSon(sendData);
            result = await ioTHub.SendMessagetoDevice(message);
            teLog.Text += result.Message + Environment.NewLine;
           
        }

        private void btnCreateTimeFrom_Click(object sender, EventArgs e)
        {
            DateTime date;
            if (DialogResult.OK == DlgDateSelect.Show(String.Empty, out date, (DateTime)this.btnCreateTimeFrom.Tag))
            {
                this.btnCreateTimeFrom.Text = date.ToShortDateString();
                this.btnCreateTimeFrom.Tag = date;
            }

        }

        private void btnCreateTimeTo_Click(object sender, EventArgs e)
        {
            DateTime date;
            if (DialogResult.OK == DlgDateSelect.Show(String.Empty, out date, (DateTime)this.btnCreateTimeTo.Tag))
            {
                this.btnCreateTimeTo.Text = date.ToShortDateString();
                this.btnCreateTimeTo.Tag = date;
            }
        }

        private async void btnSendLogFiles_Click(object sender, EventArgs e)
        {
            ResponseResult reslut = new ResponseResult("", ResponseStatus.Failure);
            DateTime t1 = (DateTime)this.btnCreateTimeFrom.Tag;
            DateTime t2 = (DateTime)this.btnCreateTimeTo.Tag;
            SetButtonEnable(false);

            if (DateTime.Compare(t1, t2) > 0)
            {
                teLog.Text += "The start date must be earlier than or equal to the end date!" + Environment.NewLine;
                SetButtonEnable(true);
                return;
            }
            else
            {
                CarisXSubFunction.PackageSystemLogs(t1, t2); 
            }

            string msg = string.Empty;
            msg = "********Start sending files...********" + Environment.NewLine;
            foreach (string fileName in Directory.GetFiles(CarisXConst.PathTemp,"*.zip"))
            {
                reslut = await ioTHub.SendToBlobAsync(fileName);
                msg += reslut.Message + Environment.NewLine;
                if(CarisXSubFunction.CheckFileStatus(fileName))
                {
                    File.Delete(fileName);
                }

            }

            teLog.Text += msg + Environment.NewLine;
            SetButtonEnable(true);
        }

        /// <summary>
        /// 设置调试界面按钮状态
        /// </summary>
        /// <param name="enable"></param>
        private void SetButtonEnable(bool enable)
        {
            this.btnTestConn.Enabled = enable;
            this.btnSendMeasureData.Enabled = enable;
            this.btnSendErrorData.Enabled = enable;
            this.btnSendDueData.Enabled = enable;
            this.btnSendLogFiles.Enabled = enable;
        }
        #endregion

        private void UpdateIoTStatus(object obj)
        {
            IoTStatusKind status = (IoTStatusKind)obj;
            switch (status)
            {
                case IoTStatusKind.OnLine:
                    this.btnConnection.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_024;
                    this.lbConnecttionStatus.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_LABEL_012;
                    this.lbConnecttionStatus.Appearance.ForeColor = Color.Green;
                    break;
                default:
                    this.btnConnection.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_023;
                    this.lbConnecttionStatus.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_IOT_LABEL_011;
                    this.lbConnecttionStatus.Appearance.ForeColor = Color.Red;
                    break;
            }
        }
    }
}
