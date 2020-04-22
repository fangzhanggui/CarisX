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
    /// 項目間演算の種類ダイアログクラス
    /// </summary>
    public partial class DlgSysTypeOfCalculated : DlgCarisXBaseSys
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgSysTypeOfCalculated()
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
                return Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable;
            }
            set
            {
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable = value;
                // 値が変更された場合、通知を行う
                if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable
                     != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.PrinterParameter.Enable)
                {
                    // イベント通知
                    Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.UseOfPrint, value);
                    // パラメータ変更履歴登録
                    this.AddPramLogData(Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_PRINTER_GBX_001
                     , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable + CarisX.Properties.Resources.STRING_LOG_MSG_001);
                }
                //　パラメータを保存する
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
            // プリンタ使用有無
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable)
            {
                this.optUsePrinter.CheckedIndex = 0;
            }
            else
            {
                this.optUsePrinter.CheckedIndex = 1;
            }
            // リアルタイム印刷
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.UsableRealtimePrint)
            {
                this.optRealTimePrint.CheckedIndex = 0;
            }
            else
            {
                this.optRealTimePrint.CheckedIndex = 1;
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_PRINTER_000;

            // グループボックス
            this.gbxUsePrinter.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_PRINTER_GBX_001;
            this.gbxRealTimePrint.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_PRINTER_GBX_002;

            // オプションボタン
            this.optUsePrinter.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_PRINTER_OPT_001;
            this.optUsePrinter.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_PRINTER_OPT_002;
            this.optRealTimePrint.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_PRINTER_OPT_003;
            this.optRealTimePrint.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_PRINTER_OPT_004;

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
            // プリンタ使用有無
            Boolean usablePrinter;
            if (this.optUsePrinter.CheckedIndex == 0)
            {
                usablePrinter = true;
            }
            else
            {
                usablePrinter = false;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable = usablePrinter;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.PrinterParameter.Enable)
            {
                // イベント通知
                Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.UseOfPrint, usablePrinter);
                // パラメータ変更履歴登録
                this.AddPramLogData(Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_PRINTER_GBX_001
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.Enable + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // リアルタイム印刷
            Boolean usableRealtimePrint;
            if (this.optRealTimePrint.CheckedIndex == 0)
            {
                usableRealtimePrint = true;
            }
            else
            {
                usableRealtimePrint = false;
            }
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.UsableRealtimePrint = usableRealtimePrint;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.UsableRealtimePrint
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.PrinterParameter.UsableRealtimePrint)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(gbxRealTimePrint.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.PrinterParameter.UsableRealtimePrint + CarisX.Properties.Resources.STRING_LOG_MSG_001);
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
            contents[1] = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_PRINTER_000;
            contents[2] = titleStr;
            contents[3] = valueStr;
            Singleton<CarisXLogManager>.Instance.Write(LogKind.ParamChangeHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, contents);
        }

        #endregion
    }
}
