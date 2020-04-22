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
using Oelco.CarisX.Parameter.ErrorCodeData;
using Oelco.CarisX.Utility;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// ダイアログ表示用デバッグダイアログクラス
    /// </summary>
    /// <remarks>
    /// 強制的に各種デバッグを表示します。
    /// デバッグ用途専用ですのでリリース版では使用しないようにしてください。
    /// </remarks>
    partial class DlgDebugDisp : DlgCarisXBaseSys
    {
        #region [定数定義]

        /// <summary>
        /// リストビューの無効行の文字色
        /// </summary>
        private readonly Color LSTV_DISABLE_COLOR = Color.LightGray;

        /// <summary>
        /// リストビューの有効行の文字色
        /// </summary>
        private readonly Color LSTV_ENABLE_COLOR = Color.Black;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgDebugDisp()
        {
            InitializeComponent();

#if DEBUG
            // 万が一、リリース版にリンクされ暴発した場合は何もしない
            // 無害なダイアログになるようにしておく


            //// 「ダイアログを表示」グループの初期化
            
            // ダイアログの名称をリストビューに追加
            string[] dialogName = {
                "DlgAskReceiptNumberInput",
                "DlgAutoSetup",
                "DlgCalibCurveConfirm", /* 実装ファイル名はDlgCalibCurveComfirm */
                "DlgCalibrationConfirm",
                //"DlgCarisXBase",      /* ベースなのでこれを直接生成しても無意味（終了するボタンもない） */
                //"DlgCarisXBaseSys",   /* ベースなのでこれを直接生成しても無意味（終了するボタンもない） */
                "DlgCheckCalibrationMode",
                "DlgCheckCompanyLogo",
                "DlgConsumableDetailed",
                "DlgConvertErrLog",
                "DlgDateSelect",
                "DlgEditReagent",
                "DlgEditRegistInfo",
                "DlgErrorCodeMessage",
                "DlgImportMeasProto",
                //"DlgInitialize",      /* 終了するボタンがない */
                "DlgInterDayReferenceValueInput",
                "DlgLogin",
                //"DlgMessage",         /* 終了するボタンがない */
                "DlgOptionAnalyzerTemperature",
                "DlgOptionCheckConsumables",
                "DlgOptionEOP",
                "DlgOptionSyringeAging",
                "DlgOptionSystemInitializing",
                "DlgOptionTroubleCountermeasure",
                "DlgOptionCorrectedByModel",
                "DlgPasteCountInput",
                "DlgProtocolSelect",
                "DlgRackView",
                "DlgRControlReferenceValueInput",
                "DlgReadBCBottle",
                "DlgReagentLotSelect",
                "DlgReferenceValueInput",
                "DlgRemarkDetail",
                "DlgShortReagentView",
                "DlgShutdown",
                "DlgSplash",
                "DlgSupplieComfirm",
                "DlgSysAnalysisMethod",
                "DlgSysAnaPrime",
                "DlgSysAutoPrime",
                "DlgSysAutoSetUpTimer",
                "DlgSysAutoShutDown",
                "DlgSysBubble",
                "DlgSysCheckRemainAtStartUp",
                "DlgSysCycleTime",
                "DlgSysDeviceNo",
                "DlgSysDilutedLiquidShortSupply",
                "DlgSysFlashPrime",
                "DlgSysHost",
                "DlgSysKeyBoard",
                "DlgSysMeasurementResultFile",
                "DlgSysPhotometry",
                "DlgSysPrinter",
                "DlgSysRackAssign",
                "DlgSysRackCoverNotificationTime",
                "DlgSysReagentLotChange",
                "DlgSysReagentShortSupply",
                "DlgSysSampleBC",
                "DlgSysSequenceNo",
                "DlgSysSmplSuckingUpErr",
                "DlgSysTemp",
                "DlgSysWarninglight",
                "DlgSysWarningSound",
                "DlgSysWashDispense",
                "DlgSysWashSolutionFromExterior",
                "DlgTargetSelectRange",
                "DlgTurnTable",
                "DlgWaitAnalyzerTemperature",
                "DlgWaitAskWorkSheet",
                "DlgYAxisEdit",
                "DlgSysDispensingOrder",
                "DlgSysEnableUrgentDiagnosis",
                "DlgSysTypeOfDiluent",
                "DlgSysTypeOfCalculated",
                "DlgSysEnableTBIGRA",
                "DlgRinseCheckBeforeAssay",
                "DlgCheckHoldsAvailableReag",
            };

            foreach (string name in dialogName)
            {
                lvwDialogName.Items.Add(name);
            }

            // 一度、ダイアログを生成してみて生成できないものについては
            // リスト表示の色を変える
            foreach (ListViewItem target in lvwDialogName.Items)
            {
                using (DlgCarisXBase dlg = CreateDialog(target.Text))
                {
                    // ダイアログが取得できない場合を確認
                    if (dlg == null)
                    {
                        // 無効色を設定
                        target.ForeColor = LSTV_DISABLE_COLOR;
                    }
                    else
                    {
                        // 有効色を設定
                        target.ForeColor = LSTV_ENABLE_COLOR;
                    }
                }
            }

            // 列の幅を自動調整
            foreach (ColumnHeader target in lvwDialogName.Columns)
            {
                // 項目とヘッダに基づいて幅を自動調整する
                // ※横スクロールバーを出したくないのでColumが余るが-2では行わない
                target.Width = -1;
            }

            // ダイアログの表示ボタンを無効化
            btnDialogDisp.Enabled = false;


            //// 「DlgErrorCodeMessageを表示」グループの初期化
            txtErrorCodeMessageCode.Text = "1";
            txtErrorCodeMessageArguments.Text = "1";
#endif
        }

        #endregion

        #region [プロパティ]

        #endregion

        #region [protectedメソッド]

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// OKボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion

        private void lstvDialogName_SelectedIndexChanged(object sender, EventArgs e)
        {
#if DEBUG
            // 選択されていることを確認
            if (0 < lvwDialogName.SelectedItems.Count)
            {
                // リストビューコントロールはMultiSelectがfalseなので、
                // 先頭だけを考慮すればよい

                // 選択された項目が有効の場合を確認
                if (lvwDialogName.SelectedItems[0].SubItems[0].ForeColor == LSTV_ENABLE_COLOR)
                {
                    // 表示ボタンを有効化
                    btnDialogDisp.Enabled = true;
                }
                else
                {
                    // 表示ボタンを無効化
                    btnDialogDisp.Enabled = false;
                }
            }
#endif
        }

        private void btnDialogDisp_Click(object sender, EventArgs e)
        {
#if DEBUG
            // リストボックスで何らかの選択が行われている場合を確認
            if (0 < lvwDialogName.SelectedItems.Count)
            {
                // ダイアログの生成を行う
                using (DlgCarisXBase dialog = CreateDialog(lvwDialogName.SelectedItems[0].Text))
                {
                    if (dialog == null)
                    {
                        MessageBox.Show("強制表示が未実装のダイアログです");
                    }
                    else
                    {
                        dialog.ShowDialog();
                    }
                }
            }
#endif
        }

        private void btnErrorCodeMessageDisp_Click(object sender, EventArgs e)
        {
#if DEBUG
            // ダイアログの生成を行う
            using (DlgCarisXBase dialog = CreateDialog("DlgErrorCodeMessage"))
            {
                // エラー検知ダイアログはエラーコード、エラーコード引数が不正な場合、ダイアログを表示しないが
                // 外部から表示しなかったことを判定できないので事前に判定する
                string code = txtErrorCodeMessageCode.Text.Trim();
                string arguments = txtErrorCodeMessageArguments.Text.Trim();
                if (IsErrorCodeValid(code, arguments))
                {
                    ((DlgErrorCodeMessage)dialog).ShowErrorMessage(code, arguments, "");
                }
                else
                {
                    MessageBox.Show("エラーコードまたは引数が不正です");
                }
            }
#endif
        }

#if DEBUG
        /// <summary>
        /// ダイアログ生成
        /// </summary>
        /// <param name="name">生成するダイアログ名</param>
        /// <returns>生成に成功した場合はインスタンスを返す。失敗した場合はnullを返す</returns>
        private DlgCarisXBase CreateDialog(string name)
        {
            DlgCarisXBase dialog = null;

            try
            {
                // 各ダイアログの生成を行う
                switch (name)
                {
                    #region デフォルトコンストラクタで生成できるダイアログ
                    case "DlgAskReceiptNumberInput": dialog = new DlgAskReceiptNumberInput(); break;
                    case "DlgAutoSetup":dialog = new DlgAutoSetup(); break;
                    case "DlgCalibCurveConfirm": dialog = new DlgCalibCurveConfirm(); break;
                    case "DlgCalibrationConfirm": dialog = new DlgCalibrationConfirm(); break;
                    case "DlgCarisXBase": dialog = new DlgCarisXBase(); break;
                    case "DlgCarisXBaseSys": dialog = new DlgCarisXBaseSys(); break;
                    case "DlgCheckCalibrationMode": dialog = new DlgCheckCalibrationMode(); break;
                    case "DlgCheckCompanyLogo": dialog = new DlgCheckCompanyLogo(); break;
                    case "DlgConsumableDetailed": dialog = new DlgConsumableDetailed(); break;
                    case "DlgConvertErrLog": dialog = new DlgConvertErrLog(); break;
                    case "DlgEditReagent": dialog = new DlgEditReagent(); break;
                    case "DlgErrorCodeMessage": dialog = new DlgErrorCodeMessage(); break;
                    case "DlgImportMeasProto": dialog = new DlgImportMeasProto(); break;
                    case "DlgInitialize": dialog = new DlgInitialize(); break;
                    case "DlgInterDayReferenceValueInput": dialog = new DlgInterDayReferenceValueInput(); break;
                    case "DlgLogin": dialog = new DlgLogin(); break;
                    case "DlgMessage": dialog = new DlgMessage(); break;
                    case "DlgOptionAnalyzerTemperature": dialog = new DlgOptionAnalyzerTemperature(); break;
                    case "DlgOptionCheckConsumables": dialog = new DlgOptionCheckConsumables(); break;
                    case "DlgOptionEOP": dialog = new DlgOptionEOP(); break;
                    case "DlgOptionSyringeAging": dialog = new DlgOptionSyringeAging(); break;
                    case "DlgOptionSystemInitializing": dialog = new DlgOptionSystemInitializing(); break;
                    case "DlgOptionTroubleCountermeasure": dialog = new DlgOptionTroubleCountermeasure(); break;
                    case "DlgPasteCountInput": dialog = new DlgPasteCountInput(); break;
                    case "DlgProtocolSelect": dialog = new DlgProtocolSelect(); break;
                    case "DlgRControlReferenceValueInput": dialog = new DlgRControlReferenceValueInput(); break;
                    case "DlgReadBCBottle": dialog = new DlgReadBCBottle(); break;
                    case "DlgReagentLotSelect": dialog = new DlgReagentLotSelect(); break;
                    case "DlgReferenceValueInput": dialog = new DlgReferenceValueInput(); break;
                    case "DlgSupplieComfirm": dialog = new DlgSupplieComfirm(); break;
                    case "DlgSysAnalysisMethod": dialog = new DlgSysAnalysisMethod(); break;
                    case "DlgSysAnaPrime": dialog = new DlgSysAnaPrime(); break;
                    case "DlgSysAutoPrime": dialog = new DlgSysAutoPrime(); break;
                    case "DlgSysAutoSetUpTimer": dialog = new DlgSysAutoSetUpTimer(); break;
                    case "DlgSysAutoShutDown": dialog = new DlgSysAutoShutDown(); break;
                    case "DlgSysBubble": dialog = new DlgSysBubble(); break;
                    case "DlgSysCheckRemainAtStartUp": dialog = new DlgSysCheckRemainAtStartUp(); break;
                    case "DlgSysCycleTime": dialog = new DlgSysCycleTime(); break;
                    case "DlgSysDeviceNo": dialog = new DlgSysDeviceNo(); break;
                    case "DlgSysDilutedLiquidShortSupply": dialog = new DlgSysDilutedLiquidShortSupply(); break;
                    case "DlgSysFlashPrime": dialog = new DlgSysFlashPrime(); break;
                    case "DlgSysHost": dialog = new DlgSysHost(); break;
                    case "DlgSysKeyBoard": dialog = new DlgSysKeyBoard(); break;
                    case "DlgSysMeasurementResultFile": dialog = new DlgSysMeasurementResultFile(); break;
                    case "DlgSysPhotometry": dialog = new DlgSysPhotometry(); break;
                    case "DlgSysPrinter": dialog = new DlgSysPrinter(); break;
                    case "DlgSysRackAssign": dialog = new DlgSysRackAssign(); break;
                    case "DlgSysRackCoverNotificationTime": dialog = new DlgSysRackCoverNotificationTime(); break;
                    case "DlgSysReagentLotChange": dialog = new DlgSysReagentLotChange(); break;
                    case "DlgSysReagentShortSupply": dialog = new DlgSysReagentShortSupply(); break;
                    case "DlgSysSampleBC": dialog = new DlgSysSampleBC(); break;
                    case "DlgSysSequenceNo": dialog = new DlgSysSequenceNo(); break;
                    case "DlgSysSmplSuckingUpErr": dialog = new DlgSysSmplSuckingUpErr(); break;
                    case "DlgSysTemp": dialog = new DlgSysTemp(); break;
                    case "DlgSysWarninglight": dialog = new DlgSysWarninglight(); break;
                    case "DlgSysWarningSound": dialog = new DlgSysWarningSound(); break;
                    case "DlgSysWashDispense": dialog = new DlgSysWashDispense(); break;
                    case "DlgSysWashSolutionFromExterior": dialog = new DlgSysWashSolutionFromExterior(); break;
                    case "DlgTargetSelectRange": dialog = new DlgTargetSelectRange(); break;
                    case "DlgWaitAnalyzerTemperature": dialog = new DlgWaitAnalyzerTemperature(); break;
                    case "DlgWaitAskWorkSheet": dialog = new DlgWaitAskWorkSheet(); break;
                    case "DlgYAxisEdit": dialog = new DlgYAxisEdit(); break;
                    case "DlgOptionCorrectedByModel": dialog = new DlgOptionCorrectedByModel(); break;
                    case "DlgSysDispensingOrder": dialog = new DlgSysDispensingOrder(); break;
                    case "DlgSysEnableUrgentDiagnosis": dialog = new DlgSysEnableUrgentDiagnosis(); break;
                    case "DlgSysTypeOfDiluent": dialog = new DlgSysTypeOfDiluent(); break;
                    case "DlgSysTypeOfCalculated": dialog = new DlgSysTypeOfCalculated(); break;
                    case "DlgSysEnableTBIGRA": dialog = new DlgSysEnableTBIGRA(); break;
                    case "DlgRinseCheckBeforeAssay": dialog = new DlgRinseCheckBeforeAssay(); break;
                    case "DlgCheckHoldsAvailableReag": dialog = new DlgCheckHoldsAvailableReag(); break;
                    #endregion

                    case "DlgDateSelect":
                        dialog = new DlgDateSelect("テスト", DateTime.Today);
                        break;

                    case "DlgEditRegistInfo":
                        dialog = new DlgEditRegistInfo(1, 0, 0);
                        break;

                    case "DlgShutdown":
                        {
                            FormClosedEventHandler dammy = (o, args) => {};
                            dialog = new DlgShutdown(dammy, dammy, dammy, dammy);
                        }
                        break;

                    #region 直接生成できないダイアログ
                    case "DlgRackView":
                        // データーベースにラックが登録されていないと表示できないようである
                        //CarisXIDString dammy = new GeneralRackID();
                        //dialog = new DlgRackView(dammy, 0);
                        break;

                    case "DlgRemarkDetail":
                        // コンストラクタが公開されていない
                        //dialog = new DlgRemarkDetail();
                        break;

                    case "DlgShortReagentView":
                        // コンストラクタが公開されていない
                        //dialog = new DlgShortReagentView();
                        break;

                    case "DlgSplash":
                        // DlgCarisXBase派生クラスではなくFormなので生成できない
                        //dialog = new DlgSplash();
                        break;

                    case "DlgTurnTable":
                        // 正しくSlaveと通信を行わないとボタンの表示がされないので、表示を行えるが終了ができない
                        //dialog = new DlgTurnTable();
                        break;
                    #endregion
                }
            }
            catch(Exception)
            {
                // 何らかの例外が発生した場合はダイアログを破棄する
                if (dialog != null)
                {
                    dialog.Dispose();
                    dialog = null;
                }
            }

            return dialog;
        }

        /// <summary>
        /// エラーコード、エラーコード引数の正当性確認
        /// </summary>
        /// <remarks>
        /// DlgErrorCodeMessage.ShowErrorMessageインターフェイスの処理をコピペ
        /// </remarks>
        /// <param name="code">エラーコード</param>
        /// <param name="arguments">エラーコード引数</param>
        /// <returns></returns>
        private bool IsErrorCodeValid(string code, string arguments)
        {
            // パラメータ表示処理
            Singleton<ParameterFilePreserve<ErrorCodeDataManager>>.Instance.Load();
            ErrorCodeData errorData = Singleton<ParameterFilePreserve<ErrorCodeDataManager>>.Instance.Param.GetCodeData(code, arguments);
            if (errorData == null)
            {
                // 定義されていないエラーコードが指定された。
                return false;
            }

            return true;
        }
#endif
                }
            }
