using Oelco.CarisX.Comm;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Utility;
using Oelco.Common.Comm;
using Oelco.Common.Parameter;
using Oelco.Common.Utility;
using System;
using System.Data;
using System.Windows.Forms;

namespace Oelco.CarisX.Maintenance
{
    public partial class FormTempUnit : FormUnitBase
    {
        IMaintenanceUnitStart unitStart = new UnitStart();
        volatile bool ResponseFlg;
        public int timeCount;
        public CommonFunction ComFunc = new CommonFunction();

        public FormTempUnit()
        {
            InitializeComponent();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ColumnsAdd();
            ComFunc.SetControlSettings(this);

            //システムパラメータコマンド
            SlaveCommCommand_0404 cmd0404 = new SlaveCommCommand_0404();
            cmd0404.SetSystemParameter(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param
                                     , Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex );

            numReactionTableSetTemp.Value = cmd0404.TempReActTable;
            numBFTableSetTemp.Value = cmd0404.TempBFTable;
            numBF1SetTemp.Value = cmd0404.TempPreheatBF1;
            numBF2SetTemp.Value = cmd0404.TempPreheatBF2;
            numR1SetTemp.Value = cmd0404.TempPreheatR1;
            numR2SetTemp.Value = cmd0404.TempPreheatR2;
            numChemilumiSetTemp.Value = cmd0404.TempPtotometry;

            setCulture();
        }

        #region リソース設定
        private void setCulture()
        {

            tabUnit.Tabs[0].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_008;
            tabUnit.Tabs[1].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_009;
            tabUnit.Tabs[2].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_010;

            gbxReactionTable.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_000;
            lblReactionTableTemp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_001;
            lblReactionTableSetTemp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_002;
            ChaReactionTable.TitleLeft.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_003;
            ChaReactionTable.TitleBottom.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_004;

            gbxBFTable.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_012;
            lblBFTableTemp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_001;
            lblBFTableSetTemp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_002;
            ChaBFTable.TitleLeft.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_003;
            ChaBFTable.TitleBottom.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_004;

            gbxBF1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_007;
            lblBF1Temp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_001;
            lblBF1SetTemp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_002;
            ChaBF1.TitleLeft.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_003;
            ChaBF1.TitleBottom.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_004;

            gbxBF2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_011;
            lblBF2Temp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_001;
            lblBF2SetTemp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_002;
            ChaBF2.TitleLeft.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_003;
            ChaBF2.TitleBottom.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_004;

            gbxR1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_005;
            lblR1Temp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_001;
            lblR1SetTemp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_002;
            ChaR1.TitleLeft.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_003;
            ChaR1.TitleBottom.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_004;

            gbxR2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_006;
            lblR2Temp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_001;
            lblR2SetTemp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_002;
            ChaR2.TitleLeft.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_003;
            ChaR2.TitleBottom.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_004;

            gbxChemilumi.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_015;
            lblChemilumiTemp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_001;
            lblChemilumiSetTemp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_002;
            ChaChemilumi.TitleLeft.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_003;
            ChaChemilumi.TitleBottom.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_004;

            gbxReagentStorage.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_016;
            lblReagentStorageTemp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_001;
            ChaReagentStorage.TitleLeft.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_003;
            ChaReagentStorage.TitleBottom.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_004;

            gbxRoom.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_017;
            lblRoomTemp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_001;
            ChaRoom.TitleLeft.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_003;
            ChaRoom.TitleBottom.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_004;

            gbxAnalyzer.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_013;
            lblAnalyzerTemp.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_001;
            ChaAnalyzer.TitleLeft.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_003;
            ChaAnalyzer.TitleBottom.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_004;

            btnPID.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_TEMPUNIT_018;
        }
        #endregion

        /// <summary>
        /// ユニットをスタートします。
        /// </summary>
        public override void UnitStart()
        {
            DispEnable(false);
            Singleton<CarisXSequenceHelperManager>.Instance.Maintenance.ExecuteSeqence(UnitStartPrc);
        }

        /// <summary>
        /// ユニットスタート処理をおこないます。
        /// </summary>
        private String UnitStartPrc(String str, object[] param)
        {
            SlaveCommCommand_0437 StartComm = new SlaveCommCommand_0437();
            SlaveCommCommand_0438 StartCommCoolerRoom = new SlaveCommCommand_0438();
            ItemRSIncTemp SetTemp = new ItemRSIncTemp();
            timeCount = 0;

            //【IssuesNo:4】读取温度补偿校准信息
            ParameterFilePreserve<TempOffsetParameter> TempOffset = Singleton<ParameterFilePreserve<TempOffsetParameter>>.Instance;
            TempOffset.LoadRaw();

            StartComm.Ctrl = CommandControlParameter.Set;

            //【IssuesNo:4】添加温度补偿校准值
            SetTemp.ReactionTableTemp = (double)numReactionTableSetTemp.Value + (double)TempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableTempOffsetParam;
            SetTemp.BFTableTemp = (double)numBFTableSetTemp.Value + (double)TempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableTempOffsetParam;
            SetTemp.BF1PreHeatTemp = (double)numBF1SetTemp.Value + (double)TempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1TempOffsetParam;
            SetTemp.BF2PreHeatTemp = (double)numBF2SetTemp.Value + (double)TempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2TempOffsetParam;
            SetTemp.R1ProbeTemp = (double)numR1SetTemp.Value + (double)TempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1TempOffsetParam;
            SetTemp.R2ProbeTemp = (double)numR2SetTemp.Value + (double)TempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2TempOffsetParam;
            SetTemp.ChemiluminesoensePtotometryTemp = (double)numChemilumiSetTemp.Value + (double)TempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].ptotometryTempOffsetParam;
            StartComm.Temp = SetTemp;

            while (true)
            {
                DateTime dtToday = DateTime.Now;
                //中断処理
                if (AbortFlg)
                {
                    timeCount = 0;
                    AbortFlg = false;
                    break;
                }

                //コマンド送信
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(StartComm);

                System.Threading.Thread.Sleep(100);

                ResponseFlg = true;
                while (ResponseFlg)
                {
                    if (AbortFlg)
                    {
                        timeCount = 0;
                        break;
                    }
                }

                //コマンド送信
                StartCommCoolerRoom.Ctrl = CommandControlParameter.Ask;
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(StartCommCoolerRoom);

                System.Threading.Thread.Sleep(100);

                ResponseFlg = true;
                while (ResponseFlg)
                {
                    if (AbortFlg)
                    {
                        timeCount = 0;
                        break;
                    }
                }

                timeCount += 6;
                //今のところインターバル6秒
                TimeSpan ts = DateTime.Now - dtToday;
                //ここで待ちを入れる
                while (ts.Seconds < 6)
                {
                    ts = DateTime.Now - dtToday;
                    if (AbortFlg)
                    {
                        timeCount = 0;
                        break;
                    }
                }
                StartComm.Ctrl = CommandControlParameter.Ask;

            }
            AbortFlg = false;

            //メイン画面のナビゲータバーをEnableコマンドバーをデフォルトに戻す。
            Action Disp = () => { DispEnable(true); }; Invoke(Disp);
            return str;

        }

        private void PIDStop()
        {
            //PID制御開始コマンド
            SlaveCommCommand_0472 PIDStart = new SlaveCommCommand_0472();
            PIDStart.TempArea = PIDTempArea.All;
            PIDStart.Control = PIDControl.Stop;
            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(PIDStart);
        }

        /// <summary>
        /// 受信データ処理をおこないます。
        /// </summary>
        public override void SetResponse(CommCommandEventArgs comm)
        {
            if (timeCount == 0)
            {
                ResponseFlg = false;
                return;
            }

            Double Interval = timeCount / 30;
            bool LineNull;
            string file_path;

            //【IssuesNo:4】加载本地温度补偿信息
            ParameterFilePreserve<TempOffsetParameter> TempOffset = Singleton<ParameterFilePreserve<TempOffsetParameter>>.Instance;
            TempOffset.LoadRaw();

            switch (comm.Command.CommandId)
            {
                case (int)CommandKind.Command1437:
                    //インキュベータ温度設定問合せ
                    SlaveCommCommand_1437 ds = new SlaveCommCommand_1437();

                    ds = (SlaveCommCommand_1437)comm.Command;
                    if (timeCount == 6) PlotClera();

                    //【IssuesNo:4】经过温度补偿校准后的温度值
                    double reactionTableTemp = ds.Temp.ReactionTableTemp + (double)TempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].reactionTableTempOffsetParam;
                    double BFTableTemp = ds.Temp.BFTableTemp + (double)TempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bFTableTempOffsetParam;
                    double BF1Temp = ds.Temp.BF1PreHeatTemp + (double)TempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF1TempOffsetParam;
                    double BF2Temp = ds.Temp.BF2PreHeatTemp + (double)TempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].bF2TempOffsetParam;
                    double r1Temp = ds.Temp.R1ProbeTemp + (double)TempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r1TempOffsetParam;
                    double r2Temp = ds.Temp.R2ProbeTemp + (double)TempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].r2TempOffsetParam;
                    double ChemilumiTemp = ds.Temp.ChemiluminesoensePtotometryTemp + (double)TempOffset.Param.SlaveList[Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex].ptotometryTempOffsetParam;

                    ReactionTabledata.Rows.Add(new Object[] { timeCount.ToString(), reactionTableTemp });
                    ChaReactionTable.DataSource = ReactionTabledata;
                    ChaReactionTable.Axis.X.TickmarkInterval = Interval;
                    lblReactionTableTempDsp.Text = reactionTableTemp.ToString("#0.0");

                    BFTabledata.Rows.Add(new Object[] { timeCount.ToString(), BFTableTemp });
                    ChaBFTable.DataSource = BFTabledata;
                    ChaBFTable.Axis.X.TickmarkInterval = Interval;
                    lblBFTableTempDsp.Text = BFTableTemp.ToString("#0.0");

                    BF1Preheater.Rows.Add(new Object[] { timeCount.ToString(), BF1Temp });
                    ChaBF1.DataSource = BF1Preheater;
                    ChaBF1.Axis.X.TickmarkInterval = Interval;
                    lblBF1TempDsp.Text = BF1Temp.ToString("#0.0");

                    BF2Preheater.Rows.Add(new Object[] { timeCount.ToString(), BF2Temp });
                    ChaBF2.DataSource = BF2Preheater;
                    ChaBF2.Axis.X.TickmarkInterval = Interval;
                    lblBF2TempDsp.Text = BF2Temp.ToString("#0.0");

                    R1Probe.Rows.Add(new Object[] { timeCount.ToString(), r1Temp });
                    ChaR1.DataSource = R1Probe;
                    ChaR1.Axis.X.TickmarkInterval = Interval;
                    lblR1TempDsp.Text = r1Temp.ToString("#0.0");

                    R2Probe.Rows.Add(new Object[] { timeCount.ToString(), r2Temp });
                    ChaR2.DataSource = R2Probe;
                    ChaR2.Axis.X.TickmarkInterval = Interval;
                    lblR2TempDsp.Text = r2Temp.ToString("#0.0");

                    ChemiluminescenceMeasurement.Rows.Add(new Object[] { timeCount.ToString(), ChemilumiTemp });
                    ChaChemilumi.DataSource = ChemiluminescenceMeasurement;
                    ChaChemilumi.Axis.X.TickmarkInterval = Interval;
                    lblChemilumiTempDsp.Text = ChemilumiTemp.ToString("#0.0");

                    //textファイルに書き出しLOG
                    //ファイルまでのディレクトリのパスをセット
                    LineNull = false;
                    file_path = Application.StartupPath + @"\TempLog.txt";

                    if (System.IO.File.Exists(file_path))
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(file_path))
                        {
                            if (sr.ReadLine() == null) LineNull = true;
                        }
                    }
                    else
                    {
                        LineNull = true;

                    }
                    using (System.IO.FileStream fs = System.IO.File.Open(file_path, System.IO.FileMode.Append))
                    {
                        using (System.IO.StreamWriter writer = new System.IO.StreamWriter(fs))
                        {

                            if (LineNull)
                                writer.WriteLine("Time," + "ReactionTabledata," + "BFTabledata," + "BF1Preheater," + "BF2Preheater," + "R1Preheater," + "R2Preheater," + "ChemiluminescenceMeasurement");
                            writer.WriteLine(System.DateTime.Now.ToString() + ","
                                + reactionTableTemp.ToString("#0.0") + ","
                                + BFTableTemp.ToString("#0.0") + ","
                                + BF1Temp.ToString("#0.0") + ","
                                + BF2Temp.ToString("#0.0") + ","
                                + r1Temp.ToString("#0.0") + ","
                                + r2Temp.ToString("#0.0") + ","
                                + ChemilumiTemp.ToString("#0.0")
                                );
                        }
                    }

                    ResponseFlg = false;
                    break;
                case (int)CommandKind.Command1438:
                    //試薬保冷庫温度設定問合せ
                    SlaveCommCommand_1438 dsCoolerRoom = new SlaveCommCommand_1438();

                    dsCoolerRoom = (SlaveCommCommand_1438)comm.Command;
                    if (timeCount == 6) PlotCoolerRoomClera();

                    ReagentStorage.Rows.Add(new Object[] { timeCount.ToString(), dsCoolerRoom.CoolerTemp });
                    ChaReagentStorage.DataSource = ReagentStorage;
                    ChaReagentStorage.Axis.X.TickmarkInterval = Interval;
                    lblReagentStorageTempDsp.Text = dsCoolerRoom.CoolerTemp.ToString();

                    Room.Rows.Add(new Object[] { timeCount.ToString(), dsCoolerRoom.RoomTemp });
                    ChaRoom.DataSource = Room;
                    ChaRoom.Axis.X.TickmarkInterval = Interval;
                    lblRoomTempDsp.Text = dsCoolerRoom.RoomTemp.ToString();

                    Analyzer.Rows.Add(new Object[] { timeCount.ToString(), dsCoolerRoom.AnalyzerTemp });
                    ChaAnalyzer.DataSource = Analyzer;
                    ChaAnalyzer.Axis.X.TickmarkInterval = Interval;
                    lblDeviceTempDsp.Text = dsCoolerRoom.AnalyzerTemp.ToString();

                    LineNull = false;
                    file_path = Application.StartupPath + @"\TempCoolerLog.txt";

                    if (System.IO.File.Exists(file_path))
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(file_path))
                        {
                            if (sr.ReadLine() == null) LineNull = true;
                        }
                    }
                    else
                    {
                        LineNull = true;

                    }
                    using (System.IO.FileStream fs = System.IO.File.Open(file_path, System.IO.FileMode.Append))
                    {
                        using (System.IO.StreamWriter writer = new System.IO.StreamWriter(fs))
                        {

                            if (LineNull)
                                writer.WriteLine("Time," + "Reagent Storage," + "Room," + "Analyzer");
                            writer.WriteLine(System.DateTime.Now.ToString() + ","
                                + dsCoolerRoom.CoolerTemp.ToString("#0.0") + ","
                                + dsCoolerRoom.RoomTemp.ToString("#0.0") + ","
                                + dsCoolerRoom.AnalyzerTemp.ToString("#0.0")
                                );
                        }
                    }

                    ResponseFlg = false;
                    break;
                case (int)CommandKind.Command1412:
                    //ポーズ、再開、停止のいずれか
                    switch (LastestPauseCtl)
                    {
                        case CommandControlParameter.Abort:
                            //停止処理時
                            PauseFlg = false;
                            AbortFlg = true;

                            this.Enabled = true;
                            break;
                        case CommandControlParameter.Pause:
                            //ポーズ処理時
                            //レスポンスを受けてする処理なし
                            break;
                        case CommandControlParameter.Restart:
                            //再開処理時
                            PauseFlg = false;
                            break;
                    }
                    break;
            }

        }

        /// <summary>
        /// プロットをクリアします。
        /// </summary>
        private void PlotClera()
        {
            ReactionTabledata.Clear();
            BFTabledata.Clear();
            BF1Preheater.Clear();
            BF2Preheater.Clear();
            R1Probe.Clear();
            R2Probe.Clear();
            ChemiluminescenceMeasurement.Clear();

        }
        /// <summary>
        /// プロットをクリアします。(室温、保冷庫)
        /// </summary>
        private void PlotCoolerRoomClera()
        {
            ReagentStorage.Clear();
            Room.Clear();
            Analyzer.Clear();
        }

        private void TempSetEnable(bool enable)
        {
            numReactionTableSetTemp.Enabled = enable;
            numBFTableSetTemp.Enabled = enable;
            numBF1SetTemp.Enabled = enable;
            numBF2SetTemp.Enabled = enable;
            numR1SetTemp.Enabled = enable;
            numR2SetTemp.Enabled = enable;
            numChemilumiSetTemp.Enabled = enable;

        }

        /// <summary>
        /// タブが切り替えられたときにメンテナンスメイン画面のコマンドバーの表示切り替えをおこないます。
        /// </summary>
        private void TabControl_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            ToolbarsControl();
        }

        /// <summary>
        /// メンテナンスメイン画面のコマンドバーの表示切り替えをおこないます。
        /// </summary>
        public override void ToolbarsControl(int toolBarEnablekind = -1)
        {
            //デリゲートでCall
            ToolbarsDisp(0, toolBarEnablekind);
        }

        /// <summary>
        /// Maintenanc画面のEnable/Disableを切り替えます。
        /// </summary>
        private void DispEnable(bool enable)
        {
            TempSetEnable(enable);
            //MainToolbarsDispEnable(enable);
        }

        DataTable ReactionTabledata = new DataTable();
        DataTable BFTabledata = new DataTable();
        DataTable BF1Preheater = new DataTable();
        DataTable BF2Preheater = new DataTable();
        DataTable R1Probe = new DataTable();
        DataTable R2Probe = new DataTable();
        DataTable ChemiluminescenceMeasurement = new DataTable();
        DataTable ReagentStorage = new DataTable();
        DataTable Room = new DataTable();
        DataTable Analyzer = new DataTable();

        private void ColumnsAdd()
        {
            // 列と列名を定義します。
            ReactionTabledata.Columns.Add("Series Label", typeof(string));
            ReactionTabledata.Columns.Add("TempData", typeof(double));

            BFTabledata.Columns.Add("Series Label", typeof(string));
            BFTabledata.Columns.Add("TempData", typeof(double));

            BF1Preheater.Columns.Add("Series Label", typeof(string));
            BF1Preheater.Columns.Add("TempData", typeof(double));

            BF2Preheater.Columns.Add("Series Label", typeof(string));
            BF2Preheater.Columns.Add("TempData", typeof(double));

            R1Probe.Columns.Add("Series Label", typeof(string));
            R1Probe.Columns.Add("TempData", typeof(double));

            R2Probe.Columns.Add("Series Label", typeof(string));
            R2Probe.Columns.Add("TempData", typeof(double));

            ChemiluminescenceMeasurement.Columns.Add("Series Label", typeof(string));
            ChemiluminescenceMeasurement.Columns.Add("TempData", typeof(double));

            ReagentStorage.Columns.Add("Series Label", typeof(string));
            ReagentStorage.Columns.Add("TempData", typeof(double));

            Room.Columns.Add("Series Label", typeof(string));
            Room.Columns.Add("TempData", typeof(double));

            Analyzer.Columns.Add("Series Label", typeof(string));
            Analyzer.Columns.Add("TempData", typeof(double));
        }

        private void TabControl_SelectedTabChanged(object sender, EventArgs e)
        {
            ToolbarsControl();
        }

        private void btnPID_Click(object sender, EventArgs e)
        {
            using (FormTempPID TempPIDDlg = new FormTempPID())
            {
                TempPIDDlg.ShowDialog();
            }
        }

        /// <summary>
        /// 【IssuesNo:4】温度补偿调整按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTempOffset_Click(object sender, EventArgs e)
        {
            //【IssuesNo:4】打开温度补偿校准窗体
            using (FormTempOffset TempOffsetDlg = new FormTempOffset())
            {
                TempOffsetDlg.ShowDialog();
            }
        }
    }
}
