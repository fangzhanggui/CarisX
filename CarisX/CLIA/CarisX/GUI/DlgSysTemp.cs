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
using Oelco.CarisX.Status;
using Oelco.CarisX.Const;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Common;
using Oelco.Common.Comm;
using Oelco.CarisX.Comm;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 温度ダイアログクラス
    /// </summary>
    public partial class DlgSysTemp : DlgCarisXBaseSys
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgSysTemp()
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
            Temperature moduleTemp = Singleton<PublicMemory>.Instance.moduleTemperature[(Int32)Singleton<PublicMemory>.Instance.moduleIndex];

            // パラメータ取得し、コントロールへ設定
            // 反応テーブル
            this.numReactionTable.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempReactionTable.ToString();
            // 反応テーブルオフセット
            this.numReactionTableOffset.Value = moduleTemp.TempReactionTable.ToString();

            // BFテーブル
            this.numBFTable.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempBFTable.ToString();
            // BFテーブルオフセット
            this.numBFTableOffset.Value = moduleTemp.TempBFTable.ToString();

            // B/F1プレヒート
            this.numBF1PreHeater.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempBF1PreHeat.ToString();
            // B/F1プレヒートオフセット
            this.numBF1PreHeaterOffset.Value = moduleTemp.TempBF1PreHeat.ToString();

            // B/F2プレヒート
            this.numBF2PreHeater.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempBF2PreHeat.ToString();
            // B/F2プレヒートオフセット
            this.numBF2PreHeaterOffset.Value = moduleTemp.TempBF2PreHeat.ToString();

            // R1プローブプレヒート
            this.numR1PreHeater.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempR1ProbePreHeat.ToString();
            // R1プローブプレヒートオフセット
            this.numR1PreHeaterOffset.Value = moduleTemp.TempR1ProbePreHeat.ToString();

            // R2プローブプレヒート
            this.numR2PreHeater.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempR2ProbePreHeat.ToString();
            // R2プローブプレヒートオフセット
            this.numR2PreHeaterOffset.Value = moduleTemp.TempR2ProbePreHeat.ToString();

            // 化学発光測定部
            this.numChemiluminescentDeterminationUnit.Value = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempChemiLightMeas.ToString();
            // 化学発光測定部オフセット
            this.numChemiluminescentDeterminationUnitOffset.Value = moduleTemp.TempChemiLightMeas.ToString();
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_000;

            // ラベル
            this.lblReactionTable.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_001;
            this.lblBFTable.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_006;
            this.lblBF1PreHeater.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_002;
            this.lblBF2PreHeater.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_003;
            this.lblR1PreHeater.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_004;
            this.lblR2PreHeater.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_005;
            this.lblChemiluminescentDeterminationUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_008;

            this.lblReactionTableOffset.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_009;
            this.lblBFTableOffset.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_014;
            this.lblBF1PreHeaterOffset.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_010;
            this.lblBF2PreHeaterOffset.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_011;
            this.lblR1PreHeaterOffset.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_012;
            this.lblR2PreHeaterOffset.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_013;
            this.lblChemiluminescentDeterminationUnitOffset.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_016;

            this.lblReactionTableUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_017;
            this.lblBFTableUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_017;
            this.lblBF1PreHeaterUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_017;
            this.lblBF2PreHeaterUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_017;
            this.lblR1PreHeaterUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_017;
            this.lblR2PreHeaterUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_017;
            this.lblChemiluminescentDeterminationUnitUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_017;

            this.lblReactionTableOffsetUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_017;
            this.lblBFTableOffsetUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_017;
            this.lblBF1PreHeaterOffsetUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_017;
            this.lblBF2PreHeaterOffsetUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_017;
            this.lblR1PreHeaterOffsetUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_017;
            this.lblR2PreHeaterOffsetUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_017;
            this.lblChemiluminescentDeterminationUnitOffsetUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_SYS_TEMP_LBL_017;

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
            // 反応テーブル
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempReactionTable = Double.Parse(this.numReactionTable.Value.ToString());
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempReactionTable
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.TemperatureParameter.TempReactionTable)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblReactionTable.Text
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempReactionTable + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // B/Fテーブル
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempBFTable = Double.Parse(this.numBFTable.Value.ToString());
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempBFTable
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.TemperatureParameter.TempBFTable)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblBFTable.Text
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempBFTable + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // B/F1プレヒート
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempBF1PreHeat = Double.Parse(this.numBF1PreHeater.Value.ToString());
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempBF1PreHeat
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.TemperatureParameter.TempBF1PreHeat)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblBF1PreHeater.Text
                    , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempBF1PreHeat + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // B/F2プレヒート
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempBF2PreHeat = Double.Parse(this.numBF2PreHeater.Value.ToString());
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempBF2PreHeat
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.TemperatureParameter.TempBF2PreHeat)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblBF2PreHeater.Text
                  , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempBF2PreHeat + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // R1プローブプレヒート
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempR1ProbePreHeat = Double.Parse(this.numR1PreHeater.Value.ToString());
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempR1ProbePreHeat
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.TemperatureParameter.TempR1ProbePreHeat)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblR1PreHeater.Text
                  , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempR1ProbePreHeat + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // R2プローブプレヒート
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempR2ProbePreHeat = Double.Parse(this.numR2PreHeater.Value.ToString());
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempR2ProbePreHeat
                      != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.TemperatureParameter.TempR2ProbePreHeat)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblR2PreHeater.Text
                  , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempR2ProbePreHeat + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }
            // 化学発光測定部
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempChemiLightMeas = Double.Parse(this.numChemiluminescentDeterminationUnit.Value.ToString());
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempChemiLightMeas
                     != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.OriginalParam.TemperatureParameter.TempChemiLightMeas)
            {
                // パラメータ変更履歴登録
                this.AddPramLogData(lblChemiluminescentDeterminationUnit.Text
                 , Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter.TempChemiLightMeas + CarisX.Properties.Resources.STRING_LOG_MSG_001);
            }

            // XMLへ保存
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Save();

            // 温度をスレーブへ設定
            foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                {
                    Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleindex].SetTemperature(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter);
                }
            }

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
        /// <summary>
        /// FormClosingイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DlgSysTemp_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 画面が閉じられる際、温度問合せタイマ動作停止を要求する
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SetAskTemperatureTimer, false);

            // 画面が閉じられる際、温度更新通知対象からこの画面を外す。
            Singleton<NotifyManager>.Instance.RemoveNotifyTarget((Int32)NotifyKind.UpdateTemperature, this.onTemperatureUpdate);
        }

        /// <summary>
        /// 画面表示完了イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DlgSysTemp_OnFadeShown(object sender, EventArgs e)
        {
            //// 画面が表示される際、温度更新通知対象に登録する。
            //Singleton<NotifyManager>.Instance.AddNotifyTarget( (Int32)NotifyKind.UpdateTemperature, this.onTemperatureUpdate );

            //// 画面が表示される際、温度問合せタイマ動作開始を要求する
            //Singleton<NotifyManager>.Instance.PushSignalQueue( (Int32)NotifyKind.SetAskTemperatureTimer, true );
        }

        /// <summary>
        /// 画面表示完了イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DlgSysTemp_Shown(object sender, EventArgs e)
        {
            // 画面が表示される際、温度更新通知対象に登録する。
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UpdateTemperature, this.onTemperatureUpdate);

            // 画面が表示される際、温度問合せタイマ動作開始を要求する
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SetAskTemperatureTimer, true);
        }

        /// <summary>
        /// 温度表示更新イベントハンドラ
        /// </summary>
        /// <param name="value"></param>
        private void onTemperatureUpdate(Object value)
        {
            // 表示データを更新
            // パラメータ取得し、コントロールへ設定
            Temperature moduleTemp = Singleton<PublicMemory>.Instance.moduleTemperature[(Int32)Singleton<PublicMemory>.Instance.moduleIndex];

            // 反応テーブルオフセット
            this.numReactionTableOffset.Value = moduleTemp.TempReactionTable.ToString();
            // B/Fテーブルオフセット
            this.numBFTableOffset.Value = moduleTemp.TempBFTable.ToString();
            // B/F1プレヒートオフセット
            this.numBF1PreHeaterOffset.Value = moduleTemp.TempBF1PreHeat.ToString();
            // B/F2プレヒートオフセット
            this.numBF2PreHeaterOffset.Value = moduleTemp.TempBF2PreHeat.ToString();
            // R1プローブプレヒートオフセット
            this.numR1PreHeaterOffset.Value = moduleTemp.TempR1ProbePreHeat.ToString();
            // R2プローブプレヒートオフセット
            this.numR2PreHeaterOffset.Value = moduleTemp.TempR2ProbePreHeat.ToString();
            // 化学発光測定部オフセット
            this.numChemiluminescentDeterminationUnitOffset.Value = moduleTemp.TempChemiLightMeas.ToString();
        }
        #endregion


    }
}
