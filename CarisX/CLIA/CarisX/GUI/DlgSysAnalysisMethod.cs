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
using Oelco.CarisX.Const;
using Oelco.CarisX.DB;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.CarisX.GUI.Controls;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 分析方式ダイアログクラス
    /// </summary>
    public partial class DlgSysAnalysisMethod : DlgCarisXBaseSys
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgSysAnalysisMethod()
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
            // 分析方式
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.AssayMode == AssayModeParameter.AssayModeKind.RackID)
            {
                this.optAssay.CheckedIndex = 0;
            }
            else
            {
                this.optAssay.CheckedIndex = 1;
            }
            // 分析モード(Module1)
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.UseEmergencyModeForModule1 == false)
            {
                this.optQuickModule1.CheckedIndex = 1;
            }
            else
            {
                this.optQuickModule1.CheckedIndex = 0;
            }
            // 分析モード(Module2)
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.UseEmergencyModeForModule2 == false)
            {
                this.optQuickModule2.CheckedIndex = 1;
            }
            else
            {
                this.optQuickModule2.CheckedIndex = 0;
            }
            // 分析モード(Module3)
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.UseEmergencyModeForModule3 == false)
            {
                this.optQuickModule3.CheckedIndex = 1;
            }
            else
            {
                this.optQuickModule3.CheckedIndex = 0;
            }
            // 分析モード(Module4)
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.UseEmergencyModeForModule4 == false)
            {
                this.optQuickModule4.CheckedIndex = 1;
            }
            else
            {
                this.optQuickModule4.CheckedIndex = 0;
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANALYSIS_METHOD_000;

            // グループボックス
            this.gbxAssay.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANALYSIS_METHOD_GBX_001;
            this.gbxUseOfEmergencyMode.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANALYSIS_METHOD_GBX_002;
            this.gbxQuickModule1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANALYSIS_METHOD_GBX_003;
            this.gbxQuickModule2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANALYSIS_METHOD_GBX_004;
            this.gbxQuickModule3.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANALYSIS_METHOD_GBX_005;
            this.gbxQuickModule4.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANALYSIS_METHOD_GBX_006;

            // オプションボタン
            this.optAssay.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANALYSIS_METHOD_OPT_001;
            this.optAssay.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANALYSIS_METHOD_OPT_002;
            this.optQuickModule1.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANALYSIS_METHOD_OPT_003;
            this.optQuickModule1.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANALYSIS_METHOD_OPT_004;
            this.optQuickModule2.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANALYSIS_METHOD_OPT_003;
            this.optQuickModule2.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANALYSIS_METHOD_OPT_004;
            this.optQuickModule3.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANALYSIS_METHOD_OPT_003;
            this.optQuickModule3.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANALYSIS_METHOD_OPT_004;
            this.optQuickModule4.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANALYSIS_METHOD_OPT_003;
            this.optQuickModule4.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_ANALYSIS_METHOD_OPT_004;

            // ボタン
            this.btnOK.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;

            // 接続台数によって活性状態切り替え
            Int32 numOfConnected = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;
            switch (numOfConnected)
            {
                case 1:
                    this.gbxQuickModule2.Enabled = false;
                    this.gbxQuickModule3.Enabled = false;
                    this.gbxQuickModule4.Enabled = false;
                    break;
                case 2:
                    this.gbxQuickModule3.Enabled = false;
                    this.gbxQuickModule4.Enabled = false;
                    break;
                case 3:
                    this.gbxQuickModule4.Enabled = false;
                    break;
                case 4:
                default:
                    break;
            }
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// OKボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にOKを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            // 設定値取得、及びパラメータ設定
            // 分析方式
            AssayModeParameter.AssayModeKind assayModeKind;
            bool beforeEmergencyMode = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.IsProtocolEnabledChangedInEmergencyMode();

            if (this.optAssay.CheckedIndex == 0)
            {
                assayModeKind = AssayModeParameter.AssayModeKind.RackID;
            }
            else
            {
                assayModeKind = AssayModeParameter.AssayModeKind.SampleID;
            }
            AssayModeParameter.AssayModeKind beforeMode = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.AssayMode;
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.AssayMode = assayModeKind;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.AssayMode
                     != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.AssayModeParameter.AssayMode)
            {
                // パラメータ変更履歴登録                
                String[] contents = new String[4];
                contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
                contents[1] = lblDialogTitle.Text;
                contents[2] = gbxAssay.Text;
                contents[3] = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.AssayMode + CarisX.Properties.Resources.STRING_LOG_MSG_001;
                Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);

            }
            // 分析モード(Module1)
            Boolean analysisModeKindForModule1;
            if (this.optQuickModule1.CheckedIndex == 1)
            {
                analysisModeKindForModule1 = false;
            }
            else
            {
                analysisModeKindForModule1 = true;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.UseEmergencyModeForModule1 = analysisModeKindForModule1;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.UseEmergencyModeForModule1
                     != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.AssayModeParameter.UseEmergencyModeForModule1)
            {
                // パラメータ変更履歴登録                
                String[] contents = new String[4];
                contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
                contents[1] = this.lblDialogTitle.Text;
                contents[2] = String.Format("{0}({1})", this.gbxUseOfEmergencyMode.Text, gbxQuickModule1.Text);
                contents[3] = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.UseEmergencyModeForModule1 + CarisX.Properties.Resources.STRING_LOG_MSG_001;
                Singleton<CarisXLogManager>.Instance.Write( LogKind.ParamChangeHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents );
            }
            // 分析モード(Module2)
            Boolean analysisModeKindForModule2;
            if (this.optQuickModule2.CheckedIndex == 1)
            {
                analysisModeKindForModule2 = false;
            }
            else
            {
                analysisModeKindForModule2 = true;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.UseEmergencyModeForModule2 = analysisModeKindForModule2;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.UseEmergencyModeForModule2
                     != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.AssayModeParameter.UseEmergencyModeForModule2)
            {
                // パラメータ変更履歴登録                
                String[] contents = new String[4];
                contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
                contents[1] = this.lblDialogTitle.Text;
                contents[2] = String.Format( "{0}({1})", this.gbxUseOfEmergencyMode.Text, gbxQuickModule2.Text );
                contents[3] = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.UseEmergencyModeForModule2 + CarisX.Properties.Resources.STRING_LOG_MSG_001;
                Singleton<CarisXLogManager>.Instance.Write( LogKind.ParamChangeHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents );
            }
            // 分析モード(Module3)
            Boolean analysisModeKindForModule3;
            if (this.optQuickModule3.CheckedIndex == 1)
            {
                analysisModeKindForModule3 = false;
            }
            else
            {
                analysisModeKindForModule3 = true;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.UseEmergencyModeForModule3 = analysisModeKindForModule3;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.UseEmergencyModeForModule3
                     != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.AssayModeParameter.UseEmergencyModeForModule3)
            {
                // パラメータ変更履歴登録                
                String[] contents = new String[4];
                contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
                contents[1] = lblDialogTitle.Text;
                contents[2] = String.Format( "{0}({1})", this.gbxUseOfEmergencyMode.Text, gbxQuickModule3.Text );
                contents[3] = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.UseEmergencyModeForModule3 + CarisX.Properties.Resources.STRING_LOG_MSG_001;
                Singleton<CarisXLogManager>.Instance.Write( LogKind.ParamChangeHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents );
            }
            // 分析モード(Module4)
            Boolean analysisModeKindForModule4;
            if (this.optQuickModule4.CheckedIndex == 1)
            {
                analysisModeKindForModule4 = false;
            }
            else
            {
                analysisModeKindForModule4 = true;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.UseEmergencyModeForModule4 = analysisModeKindForModule4;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.UseEmergencyModeForModule4
                     != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.AssayModeParameter.UseEmergencyModeForModule4)
            {
                // パラメータ変更履歴登録                
                String[] contents = new String[4];
                contents[0] = CarisX.Properties.Resources.STRING_LOG_MSG_052;
                contents[1] = this.lblDialogTitle.Text;
                contents[2] = String.Format( "{0}({1})", this.gbxUseOfEmergencyMode.Text, gbxQuickModule4.Text );
                contents[3] = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.UseEmergencyModeForModule4 + CarisX.Properties.Resources.STRING_LOG_MSG_001;
                Singleton<CarisXLogManager>.Instance.Write( LogKind.ParamChangeHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents );
            }

            // 変更されている場合、通知を行う
            if ( beforeMode != assayModeKind )
            {
                Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.AssayModeKindChanged, assayModeKind);
            }
            bool emergencyMode = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.IsProtocolEnabledChangedInEmergencyMode();

            // 急診使用が変更されている場合、通知を行う
            if (beforeEmergencyMode != emergencyMode)
            {
                Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.AssayModeUseOfEmergencyMode);
            }

            // XMLへ保存
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Save();
            // パラメータ変更履歴更新
            Singleton<ParameterChangeLogDB>.Instance.CommitParameterChangeLog();

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
