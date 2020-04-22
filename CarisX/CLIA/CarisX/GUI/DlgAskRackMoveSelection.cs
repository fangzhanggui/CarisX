using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Oelco.Common.Utility;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.DB;
using Oelco.CarisX.Const;
using Infragistics.Win.Misc;
using Oelco.CarisX.Common;
using Oelco.Common.Parameter;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 自動キャリブレータ登録用ラック搬送先問合せダイアログクラス
    /// </summary>
    public partial class DlgAskRackMoveSelection : DlgCarisXBase
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 無操作時間
        /// </summary>
        public const Int32 NO_OPERATION_TIME = 60;
    
        /// <summary>
        /// 残時間
        /// </summary>
        int remainTime = NO_OPERATION_TIME;

        /// <summary>
        /// ラック情報
        /// </summary>
        RackInfo rackInfomation = new RackInfo();

        /// <summary>
        /// ラックポジションアイテムリスト
        /// 試薬名 - キャリブレータロット - 濃度
        /// </summary>
        Dictionary<Int32, Tuple<UltraLabel, UltraLabel, UltraLabel>> rackPositionItems = new Dictionary<Int32, Tuple<UltraLabel, UltraLabel, UltraLabel>>();

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgAskRackMoveSelection( RackInfo rackInfo )
        {
            InitializeComponent();

            // ラック情報を保持
            this.rackInfomation = rackInfo;

            // ラック情報
            rackPositionItems.Add(1, new Tuple<UltraLabel, UltraLabel, UltraLabel>(this.lblReagName1, this.lblCalibLot1, this.lblConc1));
            rackPositionItems.Add(2, new Tuple<UltraLabel, UltraLabel, UltraLabel>(this.lblReagName2, this.lblCalibLot2, this.lblConc2));
            rackPositionItems.Add(3, new Tuple<UltraLabel, UltraLabel, UltraLabel>(this.lblReagName3, this.lblCalibLot3, this.lblConc3));
            rackPositionItems.Add(4, new Tuple<UltraLabel, UltraLabel, UltraLabel>(this.lblReagName4, this.lblCalibLot4, this.lblConc4));
            rackPositionItems.Add(5, new Tuple<UltraLabel, UltraLabel, UltraLabel>(this.lblReagName5, this.lblCalibLot5, this.lblConc5));

            // モジュールのチェック状態をOFF
            this.chkModule1.Checked = false;
            this.chkModule2.Checked = false;
            this.chkModule3.Checked = false;
            this.chkModule4.Checked = false;

            // 接続台数からモジュールの表示制御
            int numOfConnected = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;
            switch (numOfConnected)
            {
                case 1:
                    // 1台構成ではこのダイアログを表示することはない
                    break;

                case 2:
                    // 2台構成
                    this.chkModule1.Visible = true;
                    this.chkModule2.Visible = true;
                    this.chkModule3.Visible = false;
                    this.chkModule4.Visible = false;
                    break;

                case 3:
                    // 3台構成
                    this.chkModule1.Visible = true;
                    this.chkModule2.Visible = true;
                    this.chkModule3.Visible = true;
                    this.chkModule4.Visible = false;
                    break;

                case 4:
                    // 4台構成
                    this.chkModule1.Visible = true;
                    this.chkModule2.Visible = true;
                    this.chkModule3.Visible = true;
                    this.chkModule4.Visible = true;
                    break;

                default:
                    // 処理なし
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
            this.btnClose.Enabled = true;

            // ラック情報設定
            this.setRackInfomation();
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
            this.btnClose.Text = Properties.Resources.STRING_COMMON_001;

            // ダイアログタイトル
            this.Caption = Properties.Resources.STRING_DLG_SYS_ASKRACKMOVESELCTION_000;

            // ラックID
            this.gbxRackInfomation.Text = Properties.Resources.STRING_DLG_SYS_ASKRACKMOVESELCTION_001;

            // ラック情報タイトル
            this.lblPositionTitle.Text = Properties.Resources.STRING_DLG_SYS_ASKRACKMOVESELCTION_002;
            this.lblReagTitle.Text = Properties.Resources.STRING_DLG_SYS_ASKRACKMOVESELCTION_003;
            this.lblCailbratorLotTitle.Text = Properties.Resources.STRING_DLG_SYS_ASKRACKMOVESELCTION_004;
            this.lblConcTitle.Text = Properties.Resources.STRING_DLG_SYS_ASKRACKMOVESELCTION_005;

            // 「---」設定
            foreach(var item in rackPositionItems)
            {
                item.Value.Item1.Text = Properties.Resources.STRING_DLG_SYS_ASKRACKMOVESELCTION_007;
                item.Value.Item2.Text = Properties.Resources.STRING_DLG_SYS_ASKRACKMOVESELCTION_007;
                item.Value.Item3.Text = Properties.Resources.STRING_DLG_SYS_ASKRACKMOVESELCTION_007;
            }

            // メッセージ
            this.lblMessage.Text = Properties.Resources.STRING_DLG_SYS_ASKRACKMOVESELCTION_006;

            // 選択項目
            this.chkModule1.Text = Properties.Resources.STRING_MAIN_FRAME_039;
            this.chkModule2.Text = Properties.Resources.STRING_MAIN_FRAME_040;
            this.chkModule3.Text = Properties.Resources.STRING_MAIN_FRAME_041;
            this.chkModule4.Text = Properties.Resources.STRING_MAIN_FRAME_042;
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// Cancelボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にキャンセルを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnClose_Click( object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            // 搬送先モジュールの選択状態を保存
            this.saveSelectedModule();

            this.Close();
        }

        /// <summary>
        /// 残時間タイマー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeTimer_Tick(object sender, EventArgs e)
        {
            // 残時間をデクリメント
            remainTime--;

            // 時間経過した場合
            if(remainTime < 0)
            {
                // 搬送先モジュールの選択状態を保存
                this.saveSelectedModule();

                // ダイアログを閉じる
                this.Close();
            }
        }

        /// <summary>
        /// チェック処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkModule_CheckedChanged(object sender, EventArgs e)
        {
            // 時間延長
            remainTime = NO_OPERATION_TIME;
        }

        /// <summary>
        /// ラック情報設定
        /// </summary>
        private void setRackInfomation()
        {
            // ラックID
            this.gbxRackInfomation.Text = Properties.Resources.STRING_DLG_SYS_ASKRACKMOVESELCTION_001 + this.rackInfomation.RackId.ToString();

            // 「---」で初期化
            foreach (var item in rackPositionItems)
            {
                item.Value.Item1.Text = Properties.Resources.STRING_DLG_SYS_ASKRACKMOVESELCTION_007;
                item.Value.Item2.Text = Properties.Resources.STRING_DLG_SYS_ASKRACKMOVESELCTION_007;
                item.Value.Item3.Text = Properties.Resources.STRING_DLG_SYS_ASKRACKMOVESELCTION_007;
            }

            // ラックポジション情報に試薬名 - キャリブレータロット - 濃度値を設定
            foreach (RackInfoDetail rackInfoDetail in this.rackInfomation.RackPosition)
            {
                // サンプルIDが10桁以上の場合
                if (rackInfoDetail.SampleID.Length >= 10)
                {
                    // 自動登録指定のラックのため、サンプルIDを分解
                    String sampleId = rackInfoDetail.SampleID;

                    // 検体IDを分解する（000:試薬コード(プロトコル番号) 000000:CalibratorLot 0:ConcIndex）
                    int reagentCode = int.Parse(sampleId.Substring(0, 3));
                    String calibLot = sampleId.Substring(3, 6).Trim();
                    int concIdx = int.Parse(sampleId.Substring(9, 1));

                    // キャリブレータロットが5桁以上の場合
                    if (calibLot.Length >= 5)
                    {
                        MeasureProtocol measProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(reagentCode);

                        // 試薬名
                        this.rackPositionItems[rackInfoDetail.PositionNo].Item1.Text = measProtocol.ReagentName;

                        // キャリブレータロット
                        this.rackPositionItems[rackInfoDetail.PositionNo].Item2.Text = calibLot;

                        // 濃度値
                        String reagentLot = String.Empty;

                        // キャリブレータ情報の内、キャリブレータロットNoがCalibratorLotと一致する内容をモジュールID→ポート番号順に参照
                        var calibinfos = Singleton<CalibratorInfoManager>.Instance.CalibratorLot
                            .Where(v => v.ReagentCode == reagentCode
                                     && v.CalibratorLot.Exists(vv => vv.CalibratorLotNo == calibLot))
                            .OrderBy(v => v.ModuleId).ThenBy(v => v.PortNo);

                        foreach (var calibinfo in calibinfos)
                        {
                            // 試薬情報からキャリブレータ情報のポート番号、試薬コードと合致するM試薬を取得
                            var reagent = Singleton<ReagentDB>.Instance.GetReagentData(calibinfo.PortNo, calibinfo.ReagentCode, ReagentTypeDetail.M, calibinfo.ModuleId);
                            if (reagent == null)
                            {
                                // M試薬が参照できない場合は次のポートを処理
                                continue;
                            }

                            // M試薬の残量が300uL以上あるかチェック
                            if (reagent.Remain < 300)
                            {
                                // 300uL未満の場合、次のポートを処理
                                continue;
                            }

                            // 条件に合致する内容の内、一番大きいM試薬のロット番号の場合だけ適用
                            if (String.Compare(reagent.LotNo, reagentLot) > 0)
                            {
                                // キャリブレータ情報の補正ポイントの内、ConcIndex-1と一致する位置にある値を取得
                                var concList = calibinfo.CalibratorLot.Where(v => v.CalibratorLotNo == calibLot && v.ConcCount >= (concIdx - 1))
                                    .Select(v => v.Concentration).FirstOrDefault();
                                if (concList != null)
                                {
                                    // 濃度値設定
                                    this.rackPositionItems[rackInfoDetail.PositionNo].Item3.Text = concList[concIdx - 1].ToString();
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 搬送先モジュールの選択状態を保存
        /// </summary>
        private void saveSelectedModule()
        {
            // 設定用モジュール番号リスト
            List<RackModuleIndex> setModuleNoList = new List<RackModuleIndex>();

            // モジュール1
            if (this.chkModule1.Checked)
            {
                setModuleNoList.Add(RackModuleIndex.Module1);
            }

            // モジュール2
            if (this.chkModule2.Checked)
            {
                setModuleNoList.Add(RackModuleIndex.Module2);
            }

            // モジュール3
            if (this.chkModule3.Checked)
            {
                setModuleNoList.Add(RackModuleIndex.Module3);
            }

            // モジュール4
            if (this.chkModule4.Checked)
            {
                setModuleNoList.Add(RackModuleIndex.Module4);
            }

            // 移動先モジュール情報を設定
            int rackPositionCount = this.rackInfomation.RackPosition.Count();
            for (int positionIndex = 0; positionIndex < rackPositionCount; positionIndex++)
            {
                this.rackInfomation.RackPosition[positionIndex].RegisteredModulesForAutoCalib.AddRange(setModuleNoList);
            }

            // ラック情報に反映
            Singleton<RackInfoManager>.Instance.SetRackInfo(rackInfomation);
        }

        #endregion
    }
}
