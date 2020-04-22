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

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 分析開始前のリンス処理確認ダイアログクラス
    /// </summary>
    public partial class DlgAssayStartConfirm : DlgCarisXBaseSys
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgAssayStartConfirm()
        {
            InitializeComponent();
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
            
            // アッセイ前のリンス処理確認チェック状態設定
            this.chkRinseExecution.Checked = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RinseExecutionBeforeAssay.Enable;
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
            this.Caption = Properties.Resources.STRING_DLG_ASSAYSTART_000;

            //メッセージ
            this.lblMessage.Text = Properties.Resources.STRING_DLG_MSG_030;

            if ((Singleton<Status.SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.RackTransfer] == Status.SystemStatusKind.MotorError)
                || (Singleton<Status.SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.Module1] == Status.SystemStatusKind.MotorError)
                || (Singleton<Status.SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.Module2] == Status.SystemStatusKind.MotorError)
                || (Singleton<Status.SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.Module3] == Status.SystemStatusKind.MotorError)
                || (Singleton<Status.SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.Module4] == Status.SystemStatusKind.MotorError))
            {
                // エラーメッセージ
                this.lblErrorMessage.Text = Properties.Resources.STRING_DLG_MSG_268;

                // エラーラック、モジュール
                this.lblErrorMessageRackModule.Text = Utility.CarisXSubFunction.GetMotorErrorRackModule();
            }
            else
            {
                // エラーメッセージ
                this.lblErrorMessage.Visible = false;

                // エラーラック、モジュール
                this.lblErrorMessageRackModule.Visible = false;
            }
            

            // オプションボタン
            this.chkRinseExecution.Text = Properties.Resources.STRING_DLG_ASSAYSTART_001;

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
            // 設定値取得、及びパラメータ設定

            // プリンタ使用有無
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RinseExecutionBeforeAssay.Enable = chkRinseExecution.Checked;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RinseExecutionBeforeAssay.Enable
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.RinseExecutionBeforeAssay.Enable)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(chkRinseExecution.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RinseExecutionBeforeAssay.Enable + Properties.Resources.STRING_LOG_MSG_001);
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
            contents[0] = Properties.Resources.STRING_LOG_MSG_052;
            contents[1] = Properties.Resources.STRING_DLG_ASSAYSTART_000;
            contents[2] = titleStr;
            contents[3] = valueStr;
            Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<Utility.CarisXUserLevelManager>.Instance.NowUserID
                , CarisXLogInfoBaseExtention.Empty, contents);
        }

        #endregion
    }
}
