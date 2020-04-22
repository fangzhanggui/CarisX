using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.CarisX.Parameter;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Const;
using Oelco.CarisX.Status;
using Oelco.CarisX.Common;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 消耗品の確認ダイアログクラス
    /// </summary>
    public partial class DlgOptionCheckConsumables : DlgCarisXBase
    {
        #region [定数定義]
        /// <summary>
        /// 消耗品情報クラス
        /// </summary>
        public struct ConsumablesInfo
        {
            public Infragistics.Win.Misc.UltraLabel lblNum;                             // テスト/時間
            public Infragistics.Win.Misc.UltraLabel lblExchange;                        // 交換目安
            public Infragistics.Win.Misc.UltraLabel lblExchangeDate;                    // 交換日
        }

        /// <summary>
        /// 消耗品情報
        /// </summary>
        protected ConsumablesInfo[] consumablesinfo;
        #endregion

        CarisXSequenceHelper.SequenceSyncObject syncObj = null;

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgOptionCheckConsumables()
        {
            InitializeComponent();

            // 通知イベントの設定
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.StateConsumables, this.StateConsumables);

            //プローブ交換完了通知
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ProbeChangeResponse, this.ProbeAdjustResponse);

            this.btnStart.Enabled = Singleton<SystemStatus>.Instance.Status != SystemStatusKind.Assay && Singleton<SystemStatus>.Instance.Status != SystemStatusKind.SamplingPause;
            ProbeText.Visible = false;
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
            consumablesinfo = new ConsumablesInfo[11];

            // 検体分注シリンジパッキン
            consumablesinfo[0].lblNum = this.lblSyringeSamplesNum;
            consumablesinfo[0].lblExchange = this.lblSyringeSamplesExchange;
            consumablesinfo[0].lblExchangeDate = this.lblSyringeSamplesDate;
            this.lblSyringeSamplesDate.Text = null;
            // R1分注シリンジパッキン
            consumablesinfo[1].lblNum = this.lblR1DispensingNum;
            consumablesinfo[1].lblExchange = this.lblR1DispensingExchange;
            consumablesinfo[1].lblExchangeDate = this.lblR1DispensingDate;
            this.lblR1DispensingDate.Text = null;
            // R2分注シリンジパッキン
            consumablesinfo[2].lblNum = this.lblR2DispensingNum;
            consumablesinfo[2].lblExchange = this.lblR2DispensingExchange;
            consumablesinfo[2].lblExchangeDate = this.lblR2DispensingDate;
            this.lblR2DispensingDate.Text = null;
            // 試薬分注洗浄液シリンジパッキン
            consumablesinfo[3].lblNum = this.lblReagentNum;
            consumablesinfo[3].lblExchange = this.lblReagentExchange;
            consumablesinfo[3].lblExchangeDate = this.lblReagentDate;
            this.lblReagentDate.Text = null;
            // 希釈液分注シリンジパッキン
            consumablesinfo[4].lblNum = this.lblDiluentNum;
            consumablesinfo[4].lblExchange = this.lblDiluentExchange;
            consumablesinfo[4].lblExchangeDate = this.lblDiluentDate;
            this.lblDiluentDate.Text = null;
            // 洗浄1分注シリンジパッキン
            consumablesinfo[5].lblNum = this.lblWash1Num;
            consumablesinfo[5].lblExchange = this.lblWash1Exchange;
            consumablesinfo[5].lblExchangeDate = this.lblWash1Date;
            this.lblWash1Date.Text = null;
            // 洗浄2分注シリンジパッキン
            consumablesinfo[6].lblNum = this.lblWash2Num;
            consumablesinfo[6].lblExchange = this.lblWash2Exchange;
            consumablesinfo[6].lblExchangeDate = this.lblWash2Date;
            this.lblWash2Date.Text = null;
            // プレトリガ分注シリンジパッキン
            consumablesinfo[7].lblNum = this.lblPreTriggerNum;
            consumablesinfo[7].lblExchange = this.lblPreTriggerExchange;
            consumablesinfo[7].lblExchangeDate = this.lblPreTriggerDate;
            this.lblPreTriggerDate.Text = null;
            // トリガ分注シリンジパッキン
            consumablesinfo[8].lblNum = this.lblTriggerNum;
            consumablesinfo[8].lblExchange = this.lblTriggerExchange;
            consumablesinfo[8].lblExchangeDate = this.lblTriggerDate;
            this.lblTriggerDate.Text = null;
            // 体外廃液ポンプ
            consumablesinfo[9].lblNum = this.lblPumpNum;
            consumablesinfo[9].lblExchange = this.lblPumpExchange;
            consumablesinfo[9].lblExchangeDate = this.lblPumpDate;
            this.lblPumpDate.Text = null;
            // 体外廃液ポンプチューブ
            consumablesinfo[10].lblNum = this.lblTubeNum;
            consumablesinfo[10].lblExchange = this.lblTubeExchange;
            consumablesinfo[10].lblExchangeDate = this.lblTubeDate;
            this.lblTubeDate.Text = null;

            // データの反映
            LoadSupplieParameter();
        }

        /// <summary>
        /// コンポーネントの初期化
        /// </summary>
        /// <remarks>
        /// コンポーネントを初期化します
        /// </remarks>
        protected override void initializeFormComponent()
        {
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            // パネル既定ボタン
            this.btnStart.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_007;
            this.btnClose.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_002;

            // ダイアログタイトル
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_000;

            // 項目タイトル
            // 部品名
            this.lblPartName.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_001;
            // テスト数/時間
            this.lblNumberOfTest.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_002;
            // 交換目安
            this.lblReplacementGuideline.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_003;
            // 交換
            this.lblExchange.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_004;
            // 交換日
            this.lblReplacementDate.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_005;

            // 検体分注シリンジパッキン
            this.lblSyringeSamples.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_006;
            this.lblUnit1_1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_007;
            this.lblUnit1_2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_008;
            // R1分注シリンジパッキン
            this.lblR1Dispensing.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_009;
            this.lblUnit2_1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_010;
            this.lblUnit2_2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_011;
            // R2分注シリンジパッキン
            this.lblR2Dispensing.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_012;
            this.lblUnit3_1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_013;
            this.lblUnit3_2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_014;
            // 試薬分注洗浄液シリンジパッキン
            this.lblReagent.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_015;
            this.lblUnit4_1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_016;
            this.lblUnit4_2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_017;
            // 希釈液分注シリンジパッキン
            this.lblDiluent.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_018;
            this.lblUnit5_1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_019;
            this.lblUnit5_2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_020;
            // 洗浄1分注シリンジパッキン
            this.lblWash1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_021;
            this.lblUnit6_1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_022;
            this.lblUnit6_2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_023;
            // 洗浄2分注シリンジパッキン
            this.lblWash2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_024;
            this.lblUnit7_1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_025;
            this.lblUnit7_2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_026;
            // プレトリガ分注シリンジパッキン
            this.lblPreTrigger.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_027;
            this.lblUnit8_1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_028;
            this.lblUnit8_2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_029;
            // トリガ分注シリンジパッキン
            this.lblTrigger.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_030;
            this.lblUnit9_1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_031;
            this.lblUnit9_2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_032;
            // 体外廃液ポンプ
            this.lblPump.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_033;
            this.lblUnit10_1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_034;
            this.lblUnit10_2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_035;
            // 体外廃液ポンプチューブ
            this.lblTube.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_036;
            this.lblUnit11_1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_037;
            this.lblUnit11_2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_038;
            // 総アッセイ数
            this.lblTotal.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_039;
            this.lblTotalUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_040;
            // プローブ交換
            lbProbeReplacement.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_042;
            rbProbeExchange.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_043;
            rbProbeExchange.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_044;
            btProbeAdjust.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHECKCONSUMABLES_045;


            this.lblSyringeSamplesDate.Text = String.Empty;
            this.lblR1DispensingDate.Text = String.Empty;
            this.lblR2DispensingDate.Text = String.Empty;
            this.lblReagentDate.Text = String.Empty;
            this.lblDiluentDate.Text = String.Empty;
            this.lblWash1Date.Text = String.Empty;
            this.lblWash2Date.Text = String.Empty;
            this.lblPreTriggerDate.Text = String.Empty;
            this.lblTriggerDate.Text = String.Empty;
            this.lblPumpDate.Text = String.Empty;
            this.lblTubeDate.Text = String.Empty;
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// Startボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 交換処理を開始します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            // 交換開始
            Int32 selectExchange = (Int32)this.optExchange.CheckedIndex;
            if (-1 == selectExchange)
            {
                // 選択されてない
                return;
            }

            // プローブ交換以外の場合
            if ((Int32)this.optExchange.CheckedIndex != 11)
            {
                DateTime today = DateTime.Today;
                // 日付フォーマットを切り替える
                consumablesinfo[selectExchange].lblExchangeDate.Text = today.ToShortDateString();

                consumablesinfo[selectExchange].lblNum.Text = "0";

                // 交換する対象のデータを保存
                SaveSupplieParameter();

                // 各交換対象別のコマンドのやり取りを実装
                this.SendParam();
            }
            // プローブ交換の場合
            else if ((Int32)this.optExchange.CheckedIndex == 11)
            {
                //プローブ交換位置に移動
                SlaveCommCommand_0439 StartComm = new SlaveCommCommand_0439();
                if (rbProbeExchange.CheckedIndex == 0)
                {
                    StartComm.UnitNo = (int)UnitNoList.ReagentDispense1;
                    StartComm.FuncNo = (int)R1DispenceSequence.ProbeReplacement;
                }
                else if (rbProbeExchange.CheckedIndex == 1)
                {
                    StartComm.UnitNo = (int)UnitNoList.ReagentDispense2;
                    StartComm.FuncNo = (int)R2DispenceSequence.ProbeReplacement;
                }
                StartComm.Control = CommandControlParameter.Start;

                //ユニットテストコマンド　0439
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(StartComm);
            }
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
        /// パラメータ読み込み
        /// </summary>
        /// <remarks>
        /// パラメータ読み込み処理を行います
        /// </remarks>
        private void LoadSupplieParameter()
        {
            SupplieParameter supplie = Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param;
            Int32 moduleIndex = (Int32)Singleton<PublicMemory>.Instance.moduleIndex;

            // 検体分注シリンジパッキン
            this.lblSyringeSamplesNum.Text = supplie.SlaveList[moduleIndex].SampleDispensingSyringePackin.UseCount.ToString();
            this.lblSyringeSamplesExchange.Text = supplie.SlaveList[moduleIndex].SampleDispensingSyringePackin.RemainCount.ToString();
            if (DateTime.MinValue != supplie.SlaveList[moduleIndex].SampleDispensingSyringePackin.StartTime)
            {
                this.lblSyringeSamplesDate.Text = supplie.SlaveList[moduleIndex].SampleDispensingSyringePackin.StartTime.ToShortDateString();
            }
            // R1分注シリンジパッキン
            this.lblR1DispensingNum.Text = supplie.SlaveList[moduleIndex].R1DispensingSyringePackin.UseCount.ToString();
            this.lblR1DispensingExchange.Text = supplie.SlaveList[moduleIndex].R1DispensingSyringePackin.RemainCount.ToString();
            if (DateTime.MinValue != supplie.SlaveList[moduleIndex].R1DispensingSyringePackin.StartTime)
            {
                this.lblR1DispensingDate.Text = supplie.SlaveList[moduleIndex].R1DispensingSyringePackin.StartTime.ToShortDateString();
            }
            // R2分注シリンジパッキン
            this.lblR2DispensingNum.Text = supplie.SlaveList[moduleIndex].R2DispensingSyringePackin.UseCount.ToString();
            this.lblR2DispensingExchange.Text = supplie.SlaveList[moduleIndex].R2DispensingSyringePackin.RemainCount.ToString();
            if (DateTime.MinValue != supplie.SlaveList[moduleIndex].R2DispensingSyringePackin.StartTime)
            {
                this.lblR2DispensingDate.Text = supplie.SlaveList[moduleIndex].R2DispensingSyringePackin.StartTime.ToShortDateString();
            }
            // 試薬分注洗浄液シリンジパッキン
            this.lblReagentNum.Text = supplie.SlaveList[moduleIndex].ReagentDispensingSyringePackin.UseCount.ToString();
            this.lblReagentExchange.Text = supplie.SlaveList[moduleIndex].ReagentDispensingSyringePackin.RemainCount.ToString();
            if (DateTime.MinValue != supplie.SlaveList[moduleIndex].ReagentDispensingSyringePackin.StartTime)
            {
                this.lblReagentDate.Text = supplie.SlaveList[moduleIndex].ReagentDispensingSyringePackin.StartTime.ToShortDateString();
            }
            // 希釈液分注シリンジパッキン
            this.lblDiluentNum.Text = supplie.SlaveList[moduleIndex].DiluentDispensingSyringePackin.UseCount.ToString();
            this.lblDiluentExchange.Text = supplie.SlaveList[moduleIndex].DiluentDispensingSyringePackin.RemainCount.ToString();
            if (DateTime.MinValue != supplie.SlaveList[moduleIndex].DiluentDispensingSyringePackin.StartTime)
            {
                this.lblDiluentDate.Text = supplie.SlaveList[moduleIndex].DiluentDispensingSyringePackin.StartTime.ToShortDateString();
            }
            // 洗浄1分注シリンジパッキン
            this.lblWash1Num.Text = supplie.SlaveList[moduleIndex].Wash1DispensingSyringePackin.UseCount.ToString();
            this.lblWash1Exchange.Text = supplie.SlaveList[moduleIndex].Wash1DispensingSyringePackin.RemainCount.ToString();
            if (DateTime.MinValue != supplie.SlaveList[moduleIndex].Wash1DispensingSyringePackin.StartTime)
            {
                this.lblWash1Date.Text = supplie.SlaveList[moduleIndex].Wash1DispensingSyringePackin.StartTime.ToShortDateString();
            }
            // 洗浄2分注シリンジパッキン
            this.lblWash2Num.Text = supplie.SlaveList[moduleIndex].Wash2DispensingSyringePackin.UseCount.ToString();
            this.lblWash2Exchange.Text = supplie.SlaveList[moduleIndex].Wash2DispensingSyringePackin.RemainCount.ToString();
            if (DateTime.MinValue != supplie.SlaveList[moduleIndex].Wash2DispensingSyringePackin.StartTime)
            {
                this.lblWash2Date.Text = supplie.SlaveList[moduleIndex].Wash2DispensingSyringePackin.StartTime.ToShortDateString();
            }
            // プレトリガ分注シリンジパッキン
            this.lblPreTriggerNum.Text = supplie.SlaveList[moduleIndex].PreTriggerDispensingSyringePackin.UseCount.ToString();
            this.lblPreTriggerExchange.Text = supplie.SlaveList[moduleIndex].PreTriggerDispensingSyringePackin.RemainCount.ToString();
            if (DateTime.MinValue != supplie.SlaveList[moduleIndex].PreTriggerDispensingSyringePackin.StartTime)
            {
                this.lblPreTriggerDate.Text = supplie.SlaveList[moduleIndex].PreTriggerDispensingSyringePackin.StartTime.ToShortDateString();
            }
            // トリガ分注シリンジパッキン
            this.lblTriggerNum.Text = supplie.SlaveList[moduleIndex].TriggerDispensingSyringePackin.UseCount.ToString();
            this.lblTriggerExchange.Text = supplie.SlaveList[moduleIndex].TriggerDispensingSyringePackin.RemainCount.ToString();
            if (DateTime.MinValue != supplie.SlaveList[moduleIndex].TriggerDispensingSyringePackin.StartTime)
            {
                this.lblTriggerDate.Text = supplie.SlaveList[moduleIndex].TriggerDispensingSyringePackin.StartTime.ToShortDateString();
            }
            // 体外廃液ポンプ
            this.lblPumpNum.Text = ((UInt64)supplie.SlaveList[moduleIndex].OutDrainPump.UseTime.TimeSpan.TotalHours).ToString();
            this.lblPumpExchange.Text = ((UInt64)supplie.SlaveList[moduleIndex].OutDrainPump.RemainTime.TimeSpan.TotalHours).ToString();
            if (DateTime.MinValue != supplie.SlaveList[moduleIndex].OutDrainPump.StartTime)
            {
                this.lblPumpDate.Text = supplie.SlaveList[moduleIndex].OutDrainPump.StartTime.ToShortDateString();
            }
            // 体外廃液ポンプチューブ
            this.lblTubeNum.Text = ((UInt64)supplie.SlaveList[moduleIndex].OutDrainPumpTube.UseTime.TimeSpan.TotalHours).ToString();
            this.lblTubeExchange.Text = ((UInt64)supplie.SlaveList[moduleIndex].OutDrainPumpTube.RemainTime.TimeSpan.TotalHours).ToString();
            if (DateTime.MinValue != supplie.SlaveList[moduleIndex].OutDrainPumpTube.StartTime)
            {
                this.lblTubeDate.Text = supplie.SlaveList[moduleIndex].OutDrainPumpTube.StartTime.ToShortDateString();
            }

            // 累計分析数
            this.lblTotalNum.Text = supplie.SlaveList[moduleIndex].TotalAssay.ToString();
        }

        /// <summary>
        /// パラメータ書き込み
        /// </summary>
        /// <remarks>
        /// パラメータ書き込み処理を行います
        /// </remarks>
        private void SaveSupplieParameter()
        {
            SupplieParameter supplie = Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param;
            Int32 moduleIndex = (Int32)Singleton<PublicMemory>.Instance.moduleIndex;

            // 検体分注シリンジパッキン
            supplie.SlaveList[moduleIndex].SampleDispensingSyringePackin.StartTime = CarisXSubFunction.GetValidDateTime(this.lblSyringeSamplesDate.Text);
            supplie.SlaveList[moduleIndex].SampleDispensingSyringePackin.UseCount = CarisXSubFunction.Int32InnerTryParse(this.lblSyringeSamplesNum.Text);
            supplie.SlaveList[moduleIndex].SampleDispensingSyringePackin.RemainCount = CarisXSubFunction.Int32InnerTryParse(this.lblSyringeSamplesExchange.Text);
            // R1分注シリンジパッキン
            supplie.SlaveList[moduleIndex].R1DispensingSyringePackin.StartTime = CarisXSubFunction.GetValidDateTime(this.lblR1DispensingDate.Text);
            supplie.SlaveList[moduleIndex].R1DispensingSyringePackin.UseCount = CarisXSubFunction.Int32InnerTryParse(this.lblR1DispensingNum.Text);
            supplie.SlaveList[moduleIndex].R1DispensingSyringePackin.RemainCount = CarisXSubFunction.Int32InnerTryParse(this.lblR1DispensingExchange.Text);
            // R2分注シリンジパッキン
            supplie.SlaveList[moduleIndex].R2DispensingSyringePackin.StartTime = CarisXSubFunction.GetValidDateTime(this.lblR2DispensingDate.Text);
            supplie.SlaveList[moduleIndex].R2DispensingSyringePackin.UseCount = CarisXSubFunction.Int32InnerTryParse(this.lblR2DispensingNum.Text);
            supplie.SlaveList[moduleIndex].R2DispensingSyringePackin.RemainCount = CarisXSubFunction.Int32InnerTryParse(this.lblR2DispensingExchange.Text);
            // 試薬分注洗浄液シリンジパッキン
            supplie.SlaveList[moduleIndex].ReagentDispensingSyringePackin.StartTime = CarisXSubFunction.GetValidDateTime(this.lblReagentDate.Text);
            supplie.SlaveList[moduleIndex].ReagentDispensingSyringePackin.UseCount = CarisXSubFunction.Int32InnerTryParse(this.lblReagentNum.Text);
            supplie.SlaveList[moduleIndex].ReagentDispensingSyringePackin.RemainCount = CarisXSubFunction.Int32InnerTryParse(this.lblReagentExchange.Text);
            // 希釈液分注シリンジパッキン
            supplie.SlaveList[moduleIndex].DiluentDispensingSyringePackin.StartTime = CarisXSubFunction.GetValidDateTime(this.lblDiluentDate.Text);
            supplie.SlaveList[moduleIndex].DiluentDispensingSyringePackin.UseCount = CarisXSubFunction.Int32InnerTryParse(this.lblDiluentNum.Text);
            supplie.SlaveList[moduleIndex].DiluentDispensingSyringePackin.RemainCount = CarisXSubFunction.Int32InnerTryParse(this.lblDiluentExchange.Text);
            // 洗浄1分注シリンジパッキン
            supplie.SlaveList[moduleIndex].Wash1DispensingSyringePackin.StartTime = CarisXSubFunction.GetValidDateTime(this.lblWash1Date.Text);
            supplie.SlaveList[moduleIndex].Wash1DispensingSyringePackin.UseCount = CarisXSubFunction.Int32InnerTryParse(this.lblWash1Num.Text);
            supplie.SlaveList[moduleIndex].Wash1DispensingSyringePackin.RemainCount = CarisXSubFunction.Int32InnerTryParse(this.lblWash1Exchange.Text);
            // 洗浄2分注シリンジパッキン
            supplie.SlaveList[moduleIndex].Wash2DispensingSyringePackin.StartTime = CarisXSubFunction.GetValidDateTime(this.lblWash2Date.Text);
            supplie.SlaveList[moduleIndex].Wash2DispensingSyringePackin.UseCount = CarisXSubFunction.Int32InnerTryParse(this.lblWash2Num.Text);
            supplie.SlaveList[moduleIndex].Wash2DispensingSyringePackin.RemainCount = CarisXSubFunction.Int32InnerTryParse(this.lblWash2Exchange.Text);
            // プレトリガ分注シリンジパッキン
            supplie.SlaveList[moduleIndex].PreTriggerDispensingSyringePackin.StartTime = CarisXSubFunction.GetValidDateTime(this.lblPreTriggerDate.Text);
            supplie.SlaveList[moduleIndex].PreTriggerDispensingSyringePackin.UseCount = CarisXSubFunction.Int32InnerTryParse(this.lblPreTriggerNum.Text);
            supplie.SlaveList[moduleIndex].PreTriggerDispensingSyringePackin.RemainCount = CarisXSubFunction.Int32InnerTryParse(this.lblPreTriggerExchange.Text);
            // トリガ分注シリンジパッキン
            supplie.SlaveList[moduleIndex].TriggerDispensingSyringePackin.StartTime = CarisXSubFunction.GetValidDateTime(this.lblTriggerDate.Text);
            supplie.SlaveList[moduleIndex].TriggerDispensingSyringePackin.UseCount = CarisXSubFunction.Int32InnerTryParse(this.lblTriggerNum.Text);
            supplie.SlaveList[moduleIndex].TriggerDispensingSyringePackin.RemainCount = CarisXSubFunction.Int32InnerTryParse(this.lblTriggerExchange.Text);
            // 体外廃液ポンプ
            supplie.SlaveList[moduleIndex].OutDrainPump.StartTime = CarisXSubFunction.GetValidDateTime(this.lblPumpDate.Text);
            supplie.SlaveList[moduleIndex].OutDrainPump.UseTime.TimeSpan = CarisXSubFunction.TimeSpanInnerTryParseFromHour(this.lblPumpNum.Text);
            supplie.SlaveList[moduleIndex].OutDrainPump.RemainTime = CarisXSubFunction.TimeSpanInnerTryParseFromHour(this.lblPumpExchange.Text);
            // 体外廃液ポンプチューブ
            supplie.SlaveList[moduleIndex].OutDrainPumpTube.StartTime = CarisXSubFunction.GetValidDateTime(this.lblTubeDate.Text);
            supplie.SlaveList[moduleIndex].OutDrainPumpTube.UseTime.TimeSpan = CarisXSubFunction.TimeSpanInnerTryParseFromHour(this.lblTubeNum.Text);
            supplie.SlaveList[moduleIndex].OutDrainPumpTube.RemainTime = CarisXSubFunction.TimeSpanInnerTryParseFromHour(this.lblTubeExchange.Text);

            Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Save();
        }

        /// <summary>
        /// 寿命部品使用回数設定問い合わせコマンド(0044)送信
        /// </summary>
        /// <remarks>
        /// 消耗品設定シーケンスの呼び出し処理を行います
        /// </remarks>
        private void SendParam()
        {
            // 消耗品状態取得
            Singleton<CarisXSequenceHelperManager>.Instance.Slave[(int)Singleton<PublicMemory>.Instance.moduleIndex].SetStateConsumables();
        }

        /// <summary>
        /// 消耗品状態設定
        /// </summary>
        /// <remarks>
        /// 消耗品状態を設定します
        /// </remarks>
        /// <param name="value"></param>
        private void StateConsumables(Object value)
        {
            SupplieParameter supplie = Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param;
            CarisXSequenceHelper.SequenceCommData seqData = (CarisXSequenceHelper.SequenceCommData)value;
            if (seqData.WaitSuccess == Oelco.CarisX.Utility.CarisXSequenceHelper.SequenceCommData.WaitResult.Success)
            {
                SlaveCommCommand_1444 State = (SlaveCommCommand_1444)seqData.RcvCommand;

                // 取得したデータを反映
                CommandDataMediator.SetSupplieCmdToParam(State, ref supplie);

                // 画面の内容を再表示
                Int32 moduleIndex = (Int32)Singleton<PublicMemory>.Instance.moduleIndex;

                // 検体分注シリンジパッキン
                lblSyringeSamplesNum.Text = supplie.SlaveList[moduleIndex].SampleDispensingSyringePackin.UseCount.ToString();
                // R1分注シリンジパッキン
                lblR1DispensingNum.Text = supplie.SlaveList[moduleIndex].R1DispensingSyringePackin.UseCount.ToString();
                // R2分注シリンジパッキン
                lblR2DispensingNum.Text = supplie.SlaveList[moduleIndex].R2DispensingSyringePackin.UseCount.ToString();
                // 試薬分注洗浄液シリンジパッキン
                lblReagentNum.Text = supplie.SlaveList[moduleIndex].ReagentDispensingSyringePackin.UseCount.ToString();
                // 希釈液分注シリンジパッキン
                lblDiluentNum.Text = supplie.SlaveList[moduleIndex].DiluentDispensingSyringePackin.UseCount.ToString();
                // 洗浄1分注シリンジパッキン
                lblWash1Num.Text = supplie.SlaveList[moduleIndex].Wash1DispensingSyringePackin.UseCount.ToString();
                // 洗浄2分注シリンジパッキン
                lblWash2Num.Text = supplie.SlaveList[moduleIndex].Wash2DispensingSyringePackin.UseCount.ToString();
                // プレトリガ分注シリンジパッキン
                lblPreTriggerNum.Text = supplie.SlaveList[moduleIndex].PreTriggerDispensingSyringePackin.UseCount.ToString();
                // トリガ分注シリンジパッキン
                lblTriggerNum.Text = supplie.SlaveList[moduleIndex].TriggerDispensingSyringePackin.UseCount.ToString();
                // 体外廃液ポンプ
                lblPumpNum.Text = ((UInt64)supplie.SlaveList[moduleIndex].OutDrainPump.UseTime.TimeSpan.TotalHours).ToString();
                // 体外廃液ポンプチューブ
                lblTubeNum.Text = ((UInt64)supplie.SlaveList[moduleIndex].OutDrainPumpTube.UseTime.TimeSpan.TotalHours).ToString();

                // データ保存
                SaveSupplieParameter();
            }
        }

        /// <summary>
        /// 画面クローズイベント
        /// </summary>
        /// <remarks>
        /// 画面終了処理を行います
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DlgOptionCheckConsumables_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 消耗品設定通知
            Singleton<NotifyManager>.Instance.RemoveNotifyTarget((Int32)NotifyKind.StateConsumables, this.StateConsumables);

            //プローブ交換完了通知
            Singleton<NotifyManager>.Instance.RemoveNotifyTarget((Int32)NotifyKind.ProbeChangeResponse, this.ProbeAdjustResponse);
        }

        /// <summary>
        /// プローブ交換完了通知受信処理
        /// </summary>
        /// <param name="comm"></param>
        public void ProbeAdjustResponse(Object comm)
        {
            ProbeText.Visible = true;
            CarisXSequenceHelper.SequenceCommData sequenceData = comm as CarisXSequenceHelper.SequenceCommData;
            SlaveCommCommand_1497 responce = (SlaveCommCommand_1497)sequenceData.RcvCommand;
            if (responce.Result == 1)
            {
                ProbeText.Text = Properties.Resources.STRING_COMMON_001;
            }
            else
            {
                ProbeText.Text = Properties.Resources.STRING_ASSAY_026;
            }

            btnStart.Enabled = true;
            btnClose.Enabled = true;
            btProbeAdjust.Enabled = true;
            this.Enabled = true;

            if (responce.Result == 1)
            {
                ParameterFilePreserve<CarisXMotorParameter> motor = Singleton<ParameterFilePreserve<CarisXMotorParameter>>.Instance;
                motor.LoadRaw();

                Int32 moduleIndex = (Int32)Singleton<PublicMemory>.Instance.moduleIndex;

                if (rbProbeExchange.CheckedIndex == 0)
                {
                    motor.Param.SlaveList[moduleIndex].r1DispenseArmZAxisParam.OffsetMReagentAspiration += responce.Offset;
                    motor.Param.SlaveList[moduleIndex].r1DispenseArmZAxisParam.OffsetR1R2Aspiration += responce.Offset;
                    motor.Param.SlaveList[moduleIndex].r1DispenseArmZAxisParam.OffsetCuvette += responce.Offset;
                    motor.Param.SlaveList[moduleIndex].r1DispenseArmZAxisParam.OffsetReactionCellDispense += responce.Offset;
                    motor.Param.SlaveList[moduleIndex].r1DispenseArmZAxisParam.OffsetPositioningProbe += responce.Offset;
                    motor.SaveRaw();
                }
                else if (rbProbeExchange.CheckedIndex == 1)
                {
                    motor.Param.SlaveList[moduleIndex].r2DispenseArmZAxisParam.OffsetMReagentAspiration += responce.Offset;
                    motor.Param.SlaveList[moduleIndex].r2DispenseArmZAxisParam.OffsetR2Aspiration += responce.Offset;
                    motor.Param.SlaveList[moduleIndex].r2DispenseArmZAxisParam.OffsetCuvette += responce.Offset;
                    motor.Param.SlaveList[moduleIndex].r2DispenseArmZAxisParam.OffsetReactionCellDispense += responce.Offset;
                    motor.Param.SlaveList[moduleIndex].r2DispenseArmZAxisParam.OffsetPositioningProbe += responce.Offset;
                    motor.SaveRaw();
                }

                sequenceData.Dispose();
                sequenceData = null;

            }
        }

        /// <summary>
        /// Adjustment Probeボタンクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btProbeAdjust_Click(object sender, EventArgs e)
        {
            ProbeText.Visible = false;
            btnStart.Enabled = false;
            btnClose.Enabled = false;
            btProbeAdjust.Enabled = false;
            this.Enabled = false;

            //試薬プローブ位置調整コマンド　0497
            this.syncObj = Singleton<CarisXSequenceHelperManager>.Instance.Slave[(int)Singleton<PublicMemory>.Instance.moduleIndex].SetStartProbeSeq(rbProbeExchange.CheckedIndex);
        }

        #endregion
    }
}
