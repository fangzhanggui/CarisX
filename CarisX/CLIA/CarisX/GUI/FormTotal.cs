using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using Oelco.Common.Utility;
using Oelco.Common.GUI;
using Oelco.CarisX.Const;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Status;
using Oelco.CarisX.Properties;
using Oelco.CarisX.Common;
using Oelco.CarisX.Parameter;
using Oelco.Common.Parameter;
using Infragistics.Win.UltraWinGrid;
using Oelco.CarisX.DB;
using Infragistics.Win;
using Oelco.CarisX.GUI.Controls;
using Infragistics.Win.UltraWinDataSource;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using System.Reflection;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// トータル画面
    /// </summary>
    /// <remarks>
    /// メイン画面の上に透過用のダミーフォームを表示し、その上にトータル画面を配置する
    /// </remarks>
    public partial class FormTotal : FormTransitionBase
    {
        #region [定数定義]

        /// <summary>
        /// 種別
        /// </summary>
        private const String COL_KIND = "Kind";
        /// <summary>
        /// 合計
        /// </summary>
        private const String COL_TOTAL = "Total";
        /// <summary>
        /// ラック
        /// </summary>
        private const String COL_BASE = "Base";
        /// <summary>
        /// モジュール１
        /// </summary>
        private const String COL_MODULE1 = "Module1";
        /// <summary>
        /// モジュール２
        /// </summary>
        private const String COL_MODULE2 = "Module2";
        /// <summary>
        /// モジュール３
        /// </summary>
        private const String COL_MODULE3 = "Module3";
        /// <summary>
        /// モジュール４
        /// </summary>
        private const String COL_MODULE4 = "Module4";

        #endregion

        #region [クラス変数定義]

        /// <summary>
        /// ダミー画面
        /// </summary>
        private FormBackScreen formBackScreen = null;

        #endregion

        #region インスタンス変数定義

        public readonly Point AdjustLocation = new Point(255, 0);      //コントロールの表示位置調整用

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormTotal()
        {
            InitializeComponent();

            // リアルタイムデータ更新イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.RealtimeData, this.onRealTimeDataChanged);

            // 洗浄液タンク状態イベント
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.WashSolutionTankStatus, this.onWashSolutionTankStatusChanged);

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
            //タイトル
            this.lblMenuSmallTotalTitle.Text = Properties.Resources.STRING_TOTAL_001;

            // グリッドカラムヘッダー表示設定
            //消耗品
            this.grdConsumablesList.DisplayLayout.Bands[0].Columns[COL_KIND].Header.Caption = Properties.Resources.STRING_TOTAL_009;
            this.grdConsumablesList.DisplayLayout.Bands[0].Columns[COL_TOTAL].Header.Caption = Properties.Resources.STRING_TOTAL_010;
            this.grdConsumablesList.DisplayLayout.Bands[0].Columns[COL_MODULE1].Header.Caption = Properties.Resources.STRING_TOTAL_012;
            this.grdConsumablesList.DisplayLayout.Bands[0].Columns[COL_MODULE2].Header.Caption = Properties.Resources.STRING_TOTAL_013;
            this.grdConsumablesList.DisplayLayout.Bands[0].Columns[COL_MODULE3].Header.Caption = Properties.Resources.STRING_TOTAL_014;
            this.grdConsumablesList.DisplayLayout.Bands[0].Columns[COL_MODULE4].Header.Caption = Properties.Resources.STRING_TOTAL_015;

            //試薬
            this.grdReagent.DisplayLayout.Bands[0].Columns[COL_KIND].Header.Caption = Properties.Resources.STRING_TOTAL_009;
            this.grdReagent.DisplayLayout.Bands[0].Columns[COL_TOTAL].Header.Caption = Properties.Resources.STRING_TOTAL_010;
            this.grdReagent.DisplayLayout.Bands[0].Columns[COL_MODULE1].Header.Caption = Properties.Resources.STRING_TOTAL_012;
            this.grdReagent.DisplayLayout.Bands[0].Columns[COL_MODULE2].Header.Caption = Properties.Resources.STRING_TOTAL_013;
            this.grdReagent.DisplayLayout.Bands[0].Columns[COL_MODULE3].Header.Caption = Properties.Resources.STRING_TOTAL_014;
            this.grdReagent.DisplayLayout.Bands[0].Columns[COL_MODULE4].Header.Caption = Properties.Resources.STRING_TOTAL_015;

            //バッファ
            this.grdBufferList.DisplayLayout.Bands[0].Columns[COL_KIND].Header.Caption = Properties.Resources.STRING_TOTAL_009;
            this.grdBufferList.DisplayLayout.Bands[0].Columns[COL_BASE].Header.Caption = Properties.Resources.STRING_TOTAL_011;
            this.grdBufferList.DisplayLayout.Bands[0].Columns[COL_MODULE1].Header.Caption = Properties.Resources.STRING_TOTAL_012;
            this.grdBufferList.DisplayLayout.Bands[0].Columns[COL_MODULE2].Header.Caption = Properties.Resources.STRING_TOTAL_013;
            this.grdBufferList.DisplayLayout.Bands[0].Columns[COL_MODULE3].Header.Caption = Properties.Resources.STRING_TOTAL_014;
            this.grdBufferList.DisplayLayout.Bands[0].Columns[COL_MODULE4].Header.Caption = Properties.Resources.STRING_TOTAL_015;
        }
        #endregion

        #region イベントハンドラ

        #region Form
        private void FormTotal_Load(object sender, EventArgs e)
        {

            loadRemainInfo();

            SetVisibleGridColumn(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected);
        }

        /// <summary>
        /// フォームがアクティブでなくなった時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMainFrameSmallMenu_Deactivate(object sender, EventArgs e)
        {
            //フォームを閉じる
            this.Close();
        }

        /// <summary>
        /// フォームを閉じた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormTotal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Singleton<NotifyManager>.Instance.RemoveNotifyTarget((Int32)NotifyKind.RealtimeData, this.onRealTimeDataChanged);
            Singleton<NotifyManager>.Instance.RemoveNotifyTarget((Int32)NotifyKind.WashSolutionTankStatus, this.onWashSolutionTankStatusChanged);
        }

        #endregion

        #region Others
        private void btnMenuSmallClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #endregion

        #region Privateメソッド

        /// <summary>
        /// 残量情報の読み込み
        /// </summary>
        /// <remarks>
        /// 残量情報の読み込みを行い、画面へ設定する
        /// </remarks>
        public void loadRemainInfo()
        {
            //試薬情報を取得（接続されてる全モジュール＋ラックの情報）
            var AllModuleReagentData = Singleton<ReagentDB>.Instance.GetData(moduleId: CarisXConst.ALL_MODULEID);
            var rackReagentData = Singleton<ReagentDB>.Instance.GetData(moduleId: (int)RackModuleIndex.RackTransfer);

            //消耗品欄
            UltraDataSource dscConsumables;
            if (this.grdConsumablesList.DataSource == null)
            {
                //データソースがない（初回表示）時はデータソースの定義を作成
                dscConsumables = new UltraDataSource();
                dscConsumables.Band.Columns.Add(COL_KIND);
                dscConsumables.Band.Columns.Add(COL_TOTAL);
                dscConsumables.Band.Columns.Add(COL_MODULE1);
                dscConsumables.Band.Columns.Add(COL_MODULE2);
                dscConsumables.Band.Columns.Add(COL_MODULE3);
                dscConsumables.Band.Columns.Add(COL_MODULE4);
                dscConsumables.Band.Key = "バンド 0";
            }
            else
            {
                //データソースがある（表示されてる状態での更新）時はデータソースを取得する
                dscConsumables = (UltraDataSource)this.grdConsumablesList.DataSource;
                dscConsumables.Rows.Clear();    //一度中身をクリア
            }

            String ConsumablesKind = "";
            Int32 ConsumablesTotal = 0;
            Int32 ConsumablesModule1 = 0;
            Int32 ConsumablesModule2 = 0;
            Int32 ConsumablesModule3 = 0;
            Int32 ConsumablesModule4 = 0;
            Int32 remain = 0;

            Int32[] targetKind = new Int32[] { (Int32)ReagentKind.Pretrigger, (Int32)ReagentKind.Trigger
                , (Int32)ReagentKind.SamplingTip, (Int32)ReagentKind.Diluent };
            IEnumerable<ReagentData> consumablesData = AllModuleReagentData.Where(remainData => targetKind.Contains(remainData.ReagentKind));
            if (consumablesData != null && consumablesData.Count() != 0)
            {
                //消耗品の残量情報が取得できた場合

                foreach (var dataPerReagKind in consumablesData.GroupBy(v => String.Format("{0}", v.ReagentKind)))
                {
                    //試薬種別毎に処理実施

                    //明細の左端に表示する内容を設定
                    switch ((ReagentKind)Int32.Parse(dataPerReagKind.Key))
                    {
                        case ReagentKind.Pretrigger:
                            ConsumablesKind = Properties.Resources.STRING_TOTAL_002;
                            break;
                        case ReagentKind.Trigger:
                            ConsumablesKind = Properties.Resources.STRING_TOTAL_003;
                            break;
                        case ReagentKind.SamplingTip:
                            ConsumablesKind = Properties.Resources.STRING_TOTAL_004;
                            break;
                        case ReagentKind.Diluent:
                            ConsumablesKind = Properties.Resources.STRING_TOTAL_005;
                            break;
                        default:
                            continue;
                    }

                    ConsumablesTotal = 0;
                    ConsumablesModule1 = 0;
                    ConsumablesModule2 = 0;
                    ConsumablesModule3 = 0;
                    ConsumablesModule4 = 0;

                    foreach (var dataPerKindPerModule in dataPerReagKind.GroupBy(lotDataSet => String.Format("{0}", lotDataSet.ModuleNo)))
                    {
                        //モジュール毎に処理実施

                        var data = dataPerKindPerModule.Sum(v => v.Remain);
                        if (data.HasValue)
                        {
                            remain = CarisXSubFunction.GetDispRemainCount((ReagentKind)Int32.Parse(dataPerReagKind.Key), data.Value);

                            //合計欄には残量を追加していく
                            ConsumablesTotal += remain;

                            //モジュールと対応する項目に値を加算する
                            switch (dataPerKindPerModule.FirstOrDefault().ModuleNo)
                            {
                                case (Int32)RackModuleIndex.Module1:
                                    ConsumablesModule1 += remain;
                                    break;
                                case (Int32)RackModuleIndex.Module2:
                                    ConsumablesModule2 += remain;
                                    break;
                                case (Int32)RackModuleIndex.Module3:
                                    ConsumablesModule3 += remain;
                                    break;
                                case (Int32)RackModuleIndex.Module4:
                                    ConsumablesModule4 += remain;
                                    break;
                            }
                        }
                    }

                    dscConsumables.Rows.Add(new Object[] { ConsumablesKind, ConsumablesTotal, ConsumablesModule1, ConsumablesModule2, ConsumablesModule3, ConsumablesModule4 });
                }
            }

            if (this.grdConsumablesList.DataSource == null)
            {
                //初回表示でまだデータソースが設定されていない場合
                //編集した変数をデータソースに設定する
                this.grdConsumablesList.DataSource = dscConsumables;
            }


            //試薬情報欄
            UltraDataSource dscReagent;
            if (this.grdReagent.DataSource == null)
            {
                //初回表示でまだデータソースが設定されていない場合
                //新たにデータソースのインスタンスを作成する
                dscReagent = new UltraDataSource();
                dscReagent.Band.Columns.Add(COL_KIND);
                dscReagent.Band.Columns.Add(COL_TOTAL);
                dscReagent.Band.Columns.Add(COL_MODULE1);
                dscReagent.Band.Columns.Add(COL_MODULE2);
                dscReagent.Band.Columns.Add(COL_MODULE3);
                dscReagent.Band.Columns.Add(COL_MODULE4);
                dscReagent.Band.Key = "バンド 0";

            }
            else
            {
                //既に表示済みでデータソースが設定済みの場合
                //既に設定されているデータソースのインスタンスをコピーする
                //以降はコピーした変数の内容を変更していけば、自動で反映される
                dscReagent = (UltraDataSource)this.grdReagent.DataSource;
                dscReagent.Rows.Clear();    //一度中身をクリア
            }

            String reagentKind = String.Empty;
            Int32 reagentTotal = 0;
            Int32 reagentModule1 = 0;
            Int32 reagentModule2 = 0;
            Int32 reagentModule3 = 0;
            Int32 reagentModule4 = 0;
            Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList.ForEach((protocol) =>
            {
                reagentKind = String.Empty;
                reagentTotal = 0;
                reagentModule1 = 0;
                reagentModule2 = 0;
                reagentModule3 = 0;
                reagentModule4 = 0;

                IEnumerable<ReagentData> data = AllModuleReagentData
                .Where(remainData => remainData.ReagentKind == (Int32)ReagentKind.Reagent
                    && remainData.ReagentCode == protocol.ReagentCode
                    && (remainData.ReagentType == (Int32)ReagentType.M || remainData.ReagentType == (Int32)ReagentType.R1R2)
                    && !String.IsNullOrEmpty(remainData.LotNo));
                if (data != null && data.Count() != 0)
                {
                    foreach (var grpData in data.GroupBy(lotDataSet => String.Format("{0,-8}{1,-2}{2}", lotDataSet.LotNo, lotDataSet.MakerCode, lotDataSet.ModuleNo)))
                    {
                        // 試薬保冷庫1ポジション毎に全て(M、R1、R2)のロット番号が一致するものだけ抽出
                        var reagentRemainData = grpData.GroupBy(v => (v.PortNo - 1) / 3)
                            .Where(v => v.Count() == CarisXConst.REAGENT_BOTTLE_SETTING_POSITION_COUNT)
                            .SelectMany(v => v.AsEnumerable());

                        // 分析項目毎でロット番号が一致するもののみリストに追加
                        if (reagentRemainData.Count() > 0)
                        {
                            remain = new ReagentRemainData(reagentRemainData, protocol.ProtocolName).Remain[0].Value;

                            reagentKind = protocol.ProtocolName;

                            reagentTotal += remain;

                            switch (reagentRemainData.FirstOrDefault().ModuleNo)
                            {
                                case (Int32)RackModuleIndex.Module1:
                                    reagentModule1 += remain;
                                    break;
                                case (Int32)RackModuleIndex.Module2:
                                    reagentModule2 += remain;
                                    break;
                                case (Int32)RackModuleIndex.Module3:
                                    reagentModule3 += remain;
                                    break;
                                case (Int32)RackModuleIndex.Module4:
                                    reagentModule4 += remain;
                                    break;
                            }
                        }
                    }

                    //表示するデータがなかった場合は表示しない
                    if (reagentKind != String.Empty)
                    {
                        dscReagent.Rows.Add(new Object[] { reagentKind, reagentTotal, reagentModule1, reagentModule2, reagentModule3, reagentModule4 });
                    }
                }
            });
            if (this.grdReagent.DataSource == null)
            {
                //初回表示でまだデータソースが設定されていない場合
                //編集した変数をデータソースに設定する
                this.grdReagent.DataSource = dscReagent;
            }


            //バッファー欄
            UltraDataSource dscBuffer;
            if (this.grdBufferList.DataSource == null)
            {
                //初回表示でまだデータソースが設定されていない場合
                //新たにデータソースのインスタンスを作成する
                dscBuffer = new UltraDataSource();
                dscBuffer.Band.Columns.Add(COL_KIND);
                dscBuffer.Band.Columns.Add(COL_BASE);
                dscBuffer.Band.Columns.Add(COL_MODULE1);
                dscBuffer.Band.Columns.Add(COL_MODULE2);
                dscBuffer.Band.Columns.Add(COL_MODULE3);
                dscBuffer.Band.Columns.Add(COL_MODULE4);
                dscBuffer.Band.Key = "バンド 0";
            }
            else
            {
                //既に表示済みでデータソースが設定済みの場合
                //既に設定されているデータソースのインスタンスをコピーする
                //以降はコピーした変数の内容を変更していけば、自動で反映される
                dscBuffer = (UltraDataSource)this.grdBufferList.DataSource;
                dscBuffer.Rows.Clear();    //一度中身をクリア
            }

            String bufferKind = "";
            TotalBufferStatus bufferBase = TotalBufferStatus.None;
            TotalBufferStatus bufferModule1 = TotalBufferStatus.None;
            TotalBufferStatus bufferModule2 = TotalBufferStatus.None;
            TotalBufferStatus bufferModule3 = TotalBufferStatus.None;
            TotalBufferStatus bufferModule4 = TotalBufferStatus.None;
            object status = new object();

            targetKind = new Int32[] { (Int32)ReagentKind.WasteBuffer, (Int32)ReagentKind.WasteBox, (Int32)ReagentKind.WashSolutionBuffer };
            IEnumerable<ReagentData> bufferData = AllModuleReagentData.Where(remainData => targetKind.Contains(remainData.ReagentKind));
            if (bufferData != null && bufferData.Count() != 0)
            {
                foreach (var lotData in bufferData.GroupBy(lotDataSet => String.Format("{0}", lotDataSet.ReagentKind)))
                {
                    bufferBase = TotalBufferStatus.None;
                    bufferModule1 = TotalBufferStatus.None;
                    bufferModule2 = TotalBufferStatus.None;
                    bufferModule3 = TotalBufferStatus.None;
                    bufferModule4 = TotalBufferStatus.None;

                    //Baseの情報を編集
                    switch ((ReagentKind)Int32.Parse(lotData.Key))
                    {
                        case ReagentKind.WasteBuffer:
                            bufferKind = Properties.Resources.STRING_TOTAL_006;
                            var wasteTank = rackReagentData.FirstOrDefault(reagentDataItem => reagentDataItem.ReagentKind == (Int32)ReagentKind.WasteTank);
                            if (wasteTank != null)
                            {
                                status = Singleton<ReagentRemainStatusInfo>.Instance.GetWasteStatus(ReagentKind.WasteTank, (wasteTank.Remain ?? 0), (wasteTank.IsUse ?? false));
                            }
                            else
                            {
                                status = WasteStatus.None;
                            }
                            bufferBase = cnvStatustoDisplayStatus(ReagentKind.WasteTank, status);   //廃液タンクの情報を設定

                            break;
                        case ReagentKind.WasteBox:
                            bufferKind = Properties.Resources.STRING_TOTAL_007;
                            bufferBase = TotalBufferStatus.None;    //廃棄ボックスはBaseにはない
                            break;
                        case ReagentKind.WashSolutionBuffer:
                            bufferKind = Properties.Resources.STRING_TOTAL_008;
                            status = Singleton<ReagentRemainStatusInfo>.Instance
                                .GetRemainStatus(ReagentKind.WashSolutionTank, (Int32)Singleton<PublicMemory>.Instance.WashSolutionTankStatus);
                            bufferBase = cnvStatustoDisplayStatus(ReagentKind.WashSolutionTank, status);    //洗浄液タンクの情報を設定
                            break;
                        default:
                            continue;
                    }

                    foreach (var item in lotData.GroupBy(lotDataSet => String.Format("{0}", lotDataSet.ModuleNo)))
                    {
                        var items = item.FirstOrDefault();
                        if (items != null)
                        {
                            switch ((ReagentKind)Int32.Parse(lotData.Key))
                            {
                                case ReagentKind.WasteBuffer:
                                    status = Singleton<ReagentRemainStatusInfo>.Instance.GetWasteStatus(ReagentKind.WasteBuffer, (items.Remain ?? 0), false);
                                    break;
                                case ReagentKind.WasteBox:
                                    status = Singleton<ReagentRemainStatusInfo>.Instance.GetWasteBoxViewStatus(ReagentKind.WasteBox, (items.Remain ?? 0), (items.IsUse ?? false));
                                    break;
                                case ReagentKind.WashSolutionBuffer:
                                    status = Singleton<ReagentRemainStatusInfo>.Instance.GetRemainStatus(ReagentKind.WashSolutionBuffer, (items.Remain ?? 0));
                                    break;
                                default:
                                    continue;
                            }

                            switch (item.FirstOrDefault().ModuleNo)
                            {
                                case (Int32)RackModuleIndex.Module1:
                                    bufferModule1 = cnvStatustoDisplayStatus((ReagentKind)Int32.Parse(lotData.Key), status);
                                    break;
                                case (Int32)RackModuleIndex.Module2:
                                    bufferModule2 = cnvStatustoDisplayStatus((ReagentKind)Int32.Parse(lotData.Key), status);
                                    break;
                                case (Int32)RackModuleIndex.Module3:
                                    bufferModule3 = cnvStatustoDisplayStatus((ReagentKind)Int32.Parse(lotData.Key), status);
                                    break;
                                case (Int32)RackModuleIndex.Module4:
                                    bufferModule4 = cnvStatustoDisplayStatus((ReagentKind)Int32.Parse(lotData.Key), status);
                                    break;
                            }
                        }
                    }

                    dscBuffer.Rows.Add(new Object[] { bufferKind, (Int32)bufferBase, (Int32)bufferModule1, (Int32)bufferModule2, (Int32)bufferModule3, (Int32)bufferModule4 });
                }
            }

            if (this.grdBufferList.DataSource == null)
            {
                this.grdBufferList.DataSource = dscBuffer;
            }

        }

        /// <summary>
        /// 表示ステータスへの変換
        /// </summary>
        /// <param name="reagentKind"></param>
        /// <param name="status"></param>
        /// <remarks>試薬種別とステータスの組合せに対して、対応する〇、！、×、－のステータスを返す</remarks>
        /// <returns>Total画面のBuffer明細用のステータス</returns>
        private TotalBufferStatus cnvStatustoDisplayStatus(ReagentKind reagentKind, object status)
        {
            TotalBufferStatus rtnval = TotalBufferStatus.None;

            switch (reagentKind)
            {
                case ReagentKind.WasteBuffer:
                case ReagentKind.WasteTank:
                    switch ((WasteStatus)status)
                    {
                        case WasteStatus.None:      // 白：タンクなし
                            rtnval = TotalBufferStatus.None;
                            break;
                        case WasteStatus.NotFull:   // 緑：廃液注入可
                            rtnval = TotalBufferStatus.circle;
                            break;
                        case WasteStatus.Full:      // 赤：満杯
                            rtnval = TotalBufferStatus.cross;
                            break;
                        default:
                            break;
                    }
                    break;
                case ReagentKind.WasteBox:
                    switch ((WasteBoxViewStatus)status)
                    {
                        case WasteBoxViewStatus.None:
                            rtnval = TotalBufferStatus.None;
                            break;
                        case WasteBoxViewStatus.NotFull:
                            rtnval = TotalBufferStatus.circle;
                            break;
                        case WasteBoxViewStatus.Warning:
                            rtnval = TotalBufferStatus.exclamation;
                            break;
                        case WasteBoxViewStatus.Full:
                            rtnval = TotalBufferStatus.cross;
                            break;
                    }
                    break;
                case ReagentKind.WashSolutionBuffer:
                case ReagentKind.WashSolutionTank:
                    switch ((RemainStatus)status)
                    {
                        case RemainStatus.Empty:
                            rtnval = TotalBufferStatus.None;
                            break;
                        case RemainStatus.Full:
                            rtnval = TotalBufferStatus.circle;
                            break;
                        case RemainStatus.Middle:
                            rtnval = TotalBufferStatus.exclamation;
                            break;
                        case RemainStatus.Low:
                            rtnval = TotalBufferStatus.cross;
                            break;
                    }
                    break;
                default:
                    break;
            }

            return rtnval;
        }

        /// <summary>
        /// 洗浄液タンク状態更新イベント
        /// </summary>
        /// <remarks>
        /// 洗浄液タンクの状態を変更する
        /// </remarks>
        /// <param name="kind"></param>
        protected void onWashSolutionTankStatusChanged(object kind)
        {
            //画面表示、終了のタイミングでたまたま動作した時に変なエラーにならないよう、念のためtryで括っておく
            try
            {
                //バッファー欄
                UltraDataSource dscBuffer;
                if (this.grdBufferList.DataSource != null)
                {
                    dscBuffer = (UltraDataSource)this.grdBufferList.DataSource;

                    var status = Singleton<ReagentRemainStatusInfo>.Instance
                        .GetRemainStatus(ReagentKind.WashSolutionTank, (Int32)Singleton<PublicMemory>.Instance.WashSolutionTankStatus);

                    foreach (UltraDataRow row in dscBuffer.Rows)
                    {
                        if (row[COL_KIND].ToString() == Properties.Resources.STRING_TOTAL_008)
                        {
                            row[COL_BASE] = (Int32)cnvStatustoDisplayStatus(ReagentKind.WashSolutionTank, status);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                String dbgMsg = String.Format("[[Investigation log]]FormTotal::{0} ", MethodBase.GetCurrentMethod().Name);
                dbgMsg = dbgMsg + String.Format("{0} {1}", ex.Message, ex.StackTrace);
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
            }
        }

        /// <summary>
        /// Buffer明細の表示レイアウト初期化
        /// </summary>
        /// <remarks></remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdBufferList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            BufferStatusEditor editorBase = new BufferStatusEditor();
            BufferStatusEditor editorModule1 = new BufferStatusEditor();
            BufferStatusEditor editorModule2 = new BufferStatusEditor();
            BufferStatusEditor editorModule3 = new BufferStatusEditor();
            BufferStatusEditor editorModule4 = new BufferStatusEditor();
            e.Layout.Bands[0].Columns[COL_BASE].Editor = editorBase;
            e.Layout.Bands[0].Columns[COL_MODULE1].Editor = editorModule1;
            e.Layout.Bands[0].Columns[COL_MODULE2].Editor = editorModule2;
            e.Layout.Bands[0].Columns[COL_MODULE3].Editor = editorModule3;
            e.Layout.Bands[0].Columns[COL_MODULE4].Editor = editorModule4;
        }

        /// <summary>
        /// Module1～4タブの表示制御を行う
        /// </summary>
        private void SetVisibleGridColumn(int ConnectCount)
        {
            //ConnectCount以下のColumnだけ表示する
            this.grdConsumablesList.DisplayLayout.Bands[0].Columns[COL_MODULE1].Hidden = !((Int32)ModuleTabIndex.Slave1 <= ConnectCount);
            this.grdReagent.DisplayLayout.Bands[0].Columns[COL_MODULE1].Hidden = !((Int32)ModuleTabIndex.Slave1 <= ConnectCount);
            this.grdBufferList.DisplayLayout.Bands[0].Columns[COL_MODULE1].Hidden = !((Int32)ModuleTabIndex.Slave1 <= ConnectCount);

            this.grdConsumablesList.DisplayLayout.Bands[0].Columns[COL_MODULE2].Hidden = !((Int32)ModuleTabIndex.Slave2 <= ConnectCount);
            this.grdReagent.DisplayLayout.Bands[0].Columns[COL_MODULE2].Hidden = !((Int32)ModuleTabIndex.Slave2 <= ConnectCount);
            this.grdBufferList.DisplayLayout.Bands[0].Columns[COL_MODULE2].Hidden = !((Int32)ModuleTabIndex.Slave2 <= ConnectCount);

            this.grdConsumablesList.DisplayLayout.Bands[0].Columns[COL_MODULE3].Hidden = !((Int32)ModuleTabIndex.Slave3 <= ConnectCount);
            this.grdReagent.DisplayLayout.Bands[0].Columns[COL_MODULE3].Hidden = !((Int32)ModuleTabIndex.Slave3 <= ConnectCount);
            this.grdBufferList.DisplayLayout.Bands[0].Columns[COL_MODULE3].Hidden = !((Int32)ModuleTabIndex.Slave3 <= ConnectCount);

            this.grdConsumablesList.DisplayLayout.Bands[0].Columns[COL_MODULE4].Hidden = !((Int32)ModuleTabIndex.Slave4 <= ConnectCount);
            this.grdReagent.DisplayLayout.Bands[0].Columns[COL_MODULE4].Hidden = !((Int32)ModuleTabIndex.Slave4 <= ConnectCount);
            this.grdBufferList.DisplayLayout.Bands[0].Columns[COL_MODULE4].Hidden = !((Int32)ModuleTabIndex.Slave4 <= ConnectCount);

        }

        #endregion

        #region [Protectedメソッド]

        /// <summary>
        /// リアルタイムデータ変更イベント
        /// </summary>
        /// <remarks>
        /// 画面種別により画面情報を更新します
        /// </remarks>
        /// <param name="kind"></param>
        protected void onRealTimeDataChanged(object kind)
        {
            // Assayデータ
            switch ((RealtimeDataKind)kind)
            {
                case RealtimeDataKind.ReagentData:
                    this.loadRemainInfo();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// メッセージボックスダイアログ表示
        /// </summary>
        /// <remarks>
        /// フェード付メッセージボックスダイアログの表示</br>
        /// 本ダイアログより背面のウィンドウは黒透過背面となり、操作不能とします。
        /// </remarks>
        /// <returns>押下ボタン</returns>
        public override void Show(Rectangle captScreenRect)
        {
            // 背景を表示する
            formBackScreen = new FormBackScreen();
            formBackScreen.Size = Screen.PrimaryScreen.Bounds.Size;
            formBackScreen.Location = captScreenRect.Location;
            formBackScreen.StartPosition = FormStartPosition.Manual;
            formBackScreen.Opacity = 0.0d;
            formBackScreen.Visible = false;
            formBackScreen.ShowInTaskbar = false;
            formBackScreen.Show();

            // ダミー画面表示
            // ダミーをZオーダー2位、自身を1位にする。
            formBackScreen.BringToFront();
            this.Owner.AddOwnedForm(formBackScreen);    //メイン画面に黒背景画面を所有させる（メイン画面->黒背景）
            formBackScreen.AddOwnedForm(this);          //黒背景画面に小項目メニュー画面を所有させる（黒背景->小項目メニュー）
            formBackScreen.Opacity = 0.91d;
            formBackScreen.Visible = true;

            base.Show(captScreenRect);
        }

        /// <summary>
        /// フォームクローズ
        /// </summary>
        /// <remarks>
        /// フォームクローズ処理を行います。
        /// </remarks>
        public override void FormClose()
        {
            base.FormClose();
        }

        /// <summary>
        /// フォームクローズ前イベントハンドラ
        /// </summary>
        /// <remarks>
        /// フォームクローズ前イベント処理を行います。
        /// </remarks>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            this.Opacity = 0;

            base.OnClosing(e);
        }

        /// <summary>
        /// フォームクローズ後イベントハンドラ
        /// </summary>
        /// <remarks>
        /// フォームクローズ後イベント処理を行います。
        /// </remarks>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            formBackScreen.RemoveOwnedForm(this);                   //黒背景画面から自画面の所有を解除（黒背景->小項目メニューを解除）
            formBackScreen.Owner.RemoveOwnedForm(formBackScreen);   //メイン画面から黒背景画面の所有を解除（メイン画面->黒背景を解除）

            //ダミー画面も閉じる
            formBackScreen.Close();
            formBackScreen.Dispose();
            formBackScreen = null;
        }

        #endregion

        #region [内部クラス]

        /// <summary>
        /// 背景黒画面
        /// </summary>
        protected class FormBackScreen : Form
        {
            #region [インスタンス変数定義]

            /// <summary>
            /// Required designer variable.
            /// </summary>
            private System.ComponentModel.IContainer components = null;

            #endregion

            #region [コンストラクタ/デストラクタ]

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public FormBackScreen()
            {
                InitializeComponent();
            }

            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            /// <remarks>
            /// Dispose処理を行います。
            /// </remarks>
            /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
            protected override void Dispose(Boolean disposing)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
                Console.WriteLine("FormBackScreen's Dispose called");
            }

            #endregion

            /// <summary>
            /// ウィンドウパラメータ作成
            /// </summary>
            /// <remarks>
            /// ウィンドウの表示パラメータを作成します。
            /// この関数は.NETフレームワークから呼び出されるため、
            /// このプログラム内からの呼び出しはありませんが必要となります。
            /// </remarks>
            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams createParams = base.CreateParams;
                    createParams.ExStyle |= Win32API.WS_EX_TOOLWINDOW;
                    return createParams;
                }
            }

            /// <summary>
            /// メッセージプロシージャ
            /// </summary>
            /// <remarks>
            /// メッセージの内、マウスに関連するメッセージのみ通知を行います。
            /// </remarks>
            /// <param name="m">メッセージ内容</param>
            protected override void WndProc(ref Message m)
            {
                // マウス関連のメッセージを通知する。
                base.WndProc(ref m);
                // マウスクリック（正確にはマウスアップ）の処理をフックする
                this.HookClick(ref m);
            }

            /// <summary>
            /// クリックをフックし、クリックした座標にある「メイン画面」のボタンを動作させる
            /// </summary>
            /// <param name="m"></param>
            protected void HookClick(ref Message m)
            {
                const Int32 WM_LBUTTONUP = 0x0202;

                switch (m.Msg)
                {
                    case WM_LBUTTONUP:

                        Int32 x = (Int32)m.LParam & 0xFFFF;
                        Int32 y = ((Int32)m.LParam >> 16) & 0xFFFF;

                        // 親フォームの座標が送られてくるので変換する
                        Point pOwner = this.Owner.PointToScreen(new Point(x, y));

                        // 保持するコントロール全て確認
                        this.reflectMouseMsg(this.Owner.Controls, pOwner, ref m);

                        break;
                }

            }

            /// <summary>
            /// マウスメッセージ通知
            /// </summary>
            /// <remarks>
            /// 透過フォームに対するマウス操作を、自身に適用します。
            /// </remarks>
            /// <param name="ctlCol">コントロールコレクション</param>
            /// <param name="pPoint">通知座標</param>
            /// <param name="m">メッセージ内容</param>
            protected void reflectMouseMsg(Control.ControlCollection ctlCol, Point pPoint, ref Message m)
            {
                foreach (Control ctl in ctlCol)
                {
                    //コントロールがさらにコントロールを持っている場合は再帰処理を行う
                    if (ctl.Controls.Count != 0)
                    {
                        this.reflectMouseMsg(ctl.Controls, pPoint, ref m);
                    }

                    //ボタンの表示場所がクリックした座標を含む場合、ボタンのクリックイベントを実行する
                    Rectangle rect = ctl.RectangleToScreen(ctl.ClientRectangle);
                    if (rect.IntersectsWith(new Rectangle(pPoint.X, pPoint.Y, 1, 1)))
                    {
                        if (ctl is Infragistics.Win.Misc.UltraButton)
                        {
                            //クリックイベントを実行する
                            (ctl as Infragistics.Win.Misc.UltraButton).PerformClick();
                        }
                    }
                }
            }

            #region Windows Form Designer generated code

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
                this.SuspendLayout();
                // 
                // Dummy
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(85)))), ((int)(((byte)(94)))));
                this.BackgroundImage = global::Oelco.CarisX.Properties.Resources.Image_MenuBackground;
                this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                this.ClientSize = new System.Drawing.Size(1920, 1080);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.DoubleBuffered = true;
                this.Name = "SmallMenuBackground";
                this.Opacity = 0.90D;
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                this.Text = "Dummy";
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(85)))), ((int)(((byte)(94)))));
                this.ResumeLayout(false);
            }

            #endregion

        }

        #endregion
    }
}

public class BufferStatusEditor : ControlContainerEditor
{
    #region Private Members
    private BufferStatus bufferStatus;
    #endregion

    #region Constructor
    public BufferStatusEditor()
    {
        this.bufferStatus = new BufferStatus();
        this.bufferStatus.Initialize();

        // コントロールをレンダリング コントロールとして設定します
        this.RenderingControl = bufferStatus;
    }
    #endregion

    #region override RendererValue
    protected override object RendererValue
    {
        get
        {
            return this.bufferStatus.Status;
        }
        set
        {
            TotalBufferStatus renderValue = (TotalBufferStatus)Int32.Parse(value.ToString());

            this.bufferStatus.Status = renderValue;
        }
    }
    #endregion

}
