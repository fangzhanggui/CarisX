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
using Oelco.CarisX.Utility;
using Oelco.CarisX.Common;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// シーケンス番号発番方法ダイアログクラス
    /// </summary>
    public partial class DlgSysSequenceNo : DlgCarisXBaseSys
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgSysSequenceNo()
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
            // 開始シーケンス番号（一般検体）
            this.numStartSequenceNoSpecimen.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HowToCreateSequenceNoParameter.StartSeqNoPat.ToString();
            // 開始シーケンス番号（優先検体）
            this.numStartSequenceNoStat.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HowToCreateSequenceNoParameter.StartSeqNoStat.ToString();
            // 開始シーケンス番号（精度管理検体）
            this.numStartSequenceNoControl.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HowToCreateSequenceNoParameter.StartSeqNoCtrl.ToString();
            // 開始シーケンス番号（キャリブレータ）
            this.numStartSequenceNoCalibrator.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HowToCreateSequenceNoParameter.StartSeqNoCalib.ToString();
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SEQUENCE_NO_000;

            // ラベル
            this.lblStartSequenceNoSpecimen.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SEQUENCE_NO_LBL_001;
            this.lblStartSequenceNoStat.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SEQUENCE_NO_LBL_002;
            this.lblStartSequenceNoControl.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SEQUENCE_NO_LBL_003;
            this.lblStartSequenceNoCalibrator.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SEQUENCE_NO_LBL_004;
            this.lblComment.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SEQUENCE_NO_LBL_005;
            this.lblSequenceNumRangeComment.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_SEQUENCE_NO_LBL_006;

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
            // 発番済データがあるか確認する。
            Int32 specimenSeqStart = (Int32)this.numStartSequenceNoSpecimen.Value;
            Int32 prioritySpecimenSeqStart = (Int32)this.numStartSequenceNoStat.Value;
            Int32 controlSeqStart = (Int32)this.numStartSequenceNoControl.Value;
            Int32 calibratorSeqStart = (Int32)this.numStartSequenceNoCalibrator.Value;

            // シーケンス番号重複チェック
            Boolean isEnable = ValueChecker.IsEnableSequenceStartNoSet(specimenSeqStart, prioritySpecimenSeqStart, controlSeqStart, calibratorSeqStart);
            if (!isEnable)
            {
                // シーケンス番号に重複あり
                // エラーメッセージを表示して抜ける
                DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_145, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                return;
            }

            Dictionary<Int32, Boolean> priorityDic;
            priorityDic = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SampleSequenceNumberHistory.History.ToDictionary((v) => (v.IndividuallyNumber), (v) => (v.IsPriority));
            if (!Singleton<SpecimenAssayDB>.Instance.GetData().All((v) => priorityDic.ContainsKey(v.GetIndividuallyNo())))
            {
                // AssayDBとパラメータファイルに相違がある（本体を途中で終了させた後、ファイルを直接編集した場合に発生の可能性）
                //AssayDB内容と発行済シーケンス番号データに不整合が検出されました
                Singleton<CarisXLogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "[ERROR] Inconsistency was detected in outstanding sequence number and data AssayDB content");
                // 警告ラベルを表示して関数終了
                this.lblComment.Visible = true;
                return;
            }



            // シーケンス番号発番済チェック情報取得
            var assayDBExist = (from existList in
                                    (from v in Singleton<SpecimenAssayDB>.Instance.GetData()
                                     select new
                                     {
                                         Kind = priorityDic[v.GetIndividuallyNo()] == true ? Const.SampleKind.Priority : Const.SampleKind.Sample,
                                         SequenceNo = v.SequenceNo
                                     })
                                        .Union((from v in Singleton<CalibratorAssayDB>.Instance.GetData()
                                                select new
                                                {
                                                    Kind = Const.SampleKind.Calibrator,
                                                    SequenceNo = v.SequenceNo
                                                }))
                                        .Union((from v in Singleton<ControlAssayDB>.Instance.GetData()
                                                select new
                                                {
                                                    Kind = Const.SampleKind.Control,
                                                    SequenceNo = v.SequenceNo
                                                }))
                                orderby existList.SequenceNo descending
                                group existList.SequenceNo by existList.Kind).ToDictionary((v) => v.Key, (v) => v.First());



            //var assayDBExist = ( from v in Singleton<InProcessSampleInfoManager>.Instance.GetInprocessLog()
            //                     orderby v.SequenceNumber descending
            //                     group v.SequenceNumber by v.SampleKind ).ToDictionary( ( v ) => v.Key, ( v ) => v.First() );

            // 一般検体チェック
            if ((assayDBExist.ContainsKey(Const.SampleKind.Sample)) && (assayDBExist[Const.SampleKind.Sample] >= specimenSeqStart))
            {
                // 警告ラベルを表示して関数終了
                this.lblComment.Visible = true;
                return;
            }

            // 優先検体チェック
            if ((assayDBExist.ContainsKey(Const.SampleKind.Priority)) && (assayDBExist[Const.SampleKind.Priority] >= prioritySpecimenSeqStart))
            {
                // 警告ラベルを表示して関数終了
                this.lblComment.Visible = true;
                return;
            }

            // 精度管理検体チェック
            if ((assayDBExist.ContainsKey(Const.SampleKind.Control)) && (assayDBExist[Const.SampleKind.Control] >= controlSeqStart))
            {
                // 警告ラベルを表示して関数終了
                this.lblComment.Visible = true;
                return;
            }

            // キャリブレータチェック
            if ((assayDBExist.ContainsKey(Const.SampleKind.Calibrator)) && (assayDBExist[Const.SampleKind.Calibrator] >= calibratorSeqStart))
            {
                // 警告ラベルを表示して関数終了
                this.lblComment.Visible = true;
                return;
            }




            // 設定値取得、及びパラメータ設定
            // 開始シーケンス番号（一般検体）
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HowToCreateSequenceNoParameter.StartSeqNoPat = (Int32)this.numStartSequenceNoSpecimen.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HowToCreateSequenceNoParameter.StartSeqNoPat
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.HowToCreateSequenceNoParameter.StartSeqNoPat)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblStartSequenceNoSpecimen.Text
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HowToCreateSequenceNoParameter.StartSeqNoPat + CarisX.Properties.Resources.STRING_LOG_MSG_001);

                // シーケンス番号を設定し、初期化する。
                Singleton<SequencialSampleNo>.Instance.StartCount = specimenSeqStart;
                Singleton<SequencialSampleNo>.Instance.ResetNumber();
            }
            // 開始シーケンス番号（優先検体）
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HowToCreateSequenceNoParameter.StartSeqNoStat = (Int32)this.numStartSequenceNoStat.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HowToCreateSequenceNoParameter.StartSeqNoStat
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.HowToCreateSequenceNoParameter.StartSeqNoStat)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblStartSequenceNoStat.Text
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HowToCreateSequenceNoParameter.StartSeqNoStat + CarisX.Properties.Resources.STRING_LOG_MSG_001);

                // シーケンス番号を設定し、初期化する。
                Singleton<SequencialPrioritySampleNo>.Instance.StartCount = prioritySpecimenSeqStart;
                Singleton<SequencialPrioritySampleNo>.Instance.ResetNumber();
            }
            // 開始シーケンス番号（精度管理検体）
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HowToCreateSequenceNoParameter.StartSeqNoCtrl = (Int32)this.numStartSequenceNoControl.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HowToCreateSequenceNoParameter.StartSeqNoCtrl
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.HowToCreateSequenceNoParameter.StartSeqNoCtrl)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblStartSequenceNoControl.Text
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HowToCreateSequenceNoParameter.StartSeqNoCtrl + CarisX.Properties.Resources.STRING_LOG_MSG_001);


                // シーケンス番号を設定し、初期化する。
                Singleton<SequencialControlNo>.Instance.StartCount = controlSeqStart;
                Singleton<SequencialControlNo>.Instance.ResetNumber();
            }
            // 開始シーケンス番号（キャリブレータ）
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HowToCreateSequenceNoParameter.StartSeqNoCalib = (Int32)this.numStartSequenceNoCalibrator.Value;
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HowToCreateSequenceNoParameter.StartSeqNoCalib
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.HowToCreateSequenceNoParameter.StartSeqNoCalib)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblStartSequenceNoCalibrator.Text
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HowToCreateSequenceNoParameter.StartSeqNoCalib + CarisX.Properties.Resources.STRING_LOG_MSG_001);


                // シーケンス番号を設定し、初期化する。
                Singleton<SequencialCalibNo>.Instance.StartCount = calibratorSeqStart;
                Singleton<SequencialCalibNo>.Instance.ResetNumber();
            }

            // シーケンス番号終了位置の設定
            CarisXSubFunction.SequenceEndCountChange();

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
