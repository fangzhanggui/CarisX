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
    /// 測定結果ファイル作成有無ダイアログクラス
    /// </summary>
    public partial class DlgSysMeasurementResultFile : DlgCarisXBaseSys
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgSysMeasurementResultFile()
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
                return Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.MeasurementResultFileParameter.Enable;
            }
            set
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.MeasurementResultFileParameter.Enable = value;
                if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.MeasurementResultFileParameter.Enable
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.MeasurementResultFileParameter.Enable)
                {
                    // パラメータ変更履歴登録
                    this.AddPramLogData(Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_MEASUREMENT_RESULT_FILE_GBX_001
                     , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.MeasurementResultFileParameter.Enable + CarisX.Properties.Resources.STRING_LOG_MSG_001);
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
            // 測定結果ファイル作成有無
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.MeasurementResultFileParameter.Enable)
            {
                this.optUseCommonFile.CheckedIndex = 0;
            }
            else
            {
                this.optUseCommonFile.CheckedIndex = 1;
            }

            // フォルダ名
            this.txtFolder.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.MeasurementResultFileParameter.FolderName;
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_MEASUREMENT_RESULT_FILE_000;

            // グループボックス
            this.gbxUseCommonFile.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_MEASUREMENT_RESULT_FILE_GBX_001;

            // オプションボタン
            this.optUseCommonFile.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_MEASUREMENT_RESULT_FILE_OPT_001;
            this.optUseCommonFile.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_MEASUREMENT_RESULT_FILE_OPT_002;

            // ボタン
            this.btnFolder.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_MEASUREMENT_RESULT_FILE_BTN_001;
            this.btnOK.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// フォルダ選択ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// フォルダ選択ダイアログを表示します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            //最初に選択するフォルダを指定する
            folderBrowserDialog.SelectedPath = this.txtFolder.Text;
            //folderBrowserDialog.Description = "フォルダを指定してください。";
            //ルートフォルダを指定する(デフォルトでDesktop)
            //folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
            //ユーザーが新しいフォルダを作成できるようにする
            //folderBrowserDialog.ShowNewFolderButton = true;

            //ダイアログを表示
            if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.txtFolder.Value = folderBrowserDialog.SelectedPath;
            }
        }

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
            // 測定結果ファイル作成有無
            Boolean usableMeasurementResultFile;
            if (this.optUseCommonFile.CheckedIndex == 0)
            {
                usableMeasurementResultFile = true;
            }
            else
            {
                usableMeasurementResultFile = false;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.MeasurementResultFileParameter.Enable = usableMeasurementResultFile;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.MeasurementResultFileParameter.Enable
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.MeasurementResultFileParameter.Enable)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_MEASUREMENT_RESULT_FILE_GBX_001
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.MeasurementResultFileParameter.Enable + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // フォルダ名
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.MeasurementResultFileParameter.FolderName = this.txtFolder.Text;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.MeasurementResultFileParameter.FolderName
                     != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.MeasurementResultFileParameter.FolderName)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(CarisX.Properties.Resources.STRING_LOG_MSG_063
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.MeasurementResultFileParameter.FolderName + CarisX.Properties.Resources.STRING_LOG_MSG_001);
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
            contents[1] = CarisX.Properties.Resources.STRING_DLG_SYS_MEASUREMENT_RESULT_FILE_000;
            contents[2] = titleStr;
            contents[3] = valueStr;
            Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);
        }
        #endregion
    }
}
