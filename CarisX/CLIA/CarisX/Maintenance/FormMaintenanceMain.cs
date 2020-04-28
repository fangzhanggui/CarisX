using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.Comm;
using Oelco.Common.Log;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Log;
using Oelco.CarisX.Const;
using Oelco.CarisX.Parameter;
using System.Threading;
using Oelco.CarisX.Status;

namespace Oelco.CarisX.Maintenance
{
    public delegate void SetToolbarsDisp(int tabindex, int toolbarenable = -1);
    public delegate void SetTabPaint();
    public delegate void MaintenanceMainEnable(bool enable);

    /// <summary>
    /// メンテナンスメイン画面
    /// </summary>
    public partial class FormMaintenanceMain : Form
    {
        private MaintenanceMainNavi currentDisp = MaintenanceMainNavi.RackAllUnits;
        private MaintenanceMainNavi[] LastestDisp;
        public FormSampleRackTransferUnit subFormSampleRackTransfer;
        public FormRackSensorUnit subFormRackSensor;
        public FormCaseTransferUnit[] subFormCaseTransfer;
        public FormReagentStorageUnit[] subFormReagentStorage;
        public FormSTATUnit[] subFormSTAT;
        public FormSampleDispenseUnit[] subFormSampleDispense;
        public FormReactionCellTransferUnit[] subFormReactionCellTransfer;
        public FormReactionTableUnit[] subFormReactionTable;
        public FormBFTableUnit[] subFormBFTable;
        public FormTravelerDisposalUnit[] subFormTravelerDisposal;
        public FormR1DispenseUnit[] subFormR1Dispense;
        public FormR2DispenseUnit[] subFormR2Dispense;
        public FormBF1Unit[] subFormBF1;
        public FormBF2Unit[] subFormBF2;
        public FormDiluentDispenseUnit[] subFormDiluentDispense;
        public FormPretriggerUnit[] subFormPretrigger;
        public FormTriggerDispenseUnit[] subFormTriggerDispense;
        public FormFluidPipingUnit[] subFormFluidPiping;
        public FormSensorUnit[] subFormSensor;
        public FormTempUnit[] subFormTemp;
        public FormOther[] subFormOther;
        public int moduleIndex = -1;
        Thread threadPleaseWait = null;
        DlgWaitModuleChange DlgWaitModule;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormMaintenanceMain()
        {
            InitializeComponent();
            ToolBarControl(ToolBarEnable.Default);

            // 各モジュールへソフトウエア識別コマンドを送信
            foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                // オンラインとなっているモジュールへ送信
                if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                {
                    SlaveCommCommand_0401 cmd0401 = new SlaveCommCommand_0401();
                    cmd0401.SoftwareKind1 = SlaveCommCommand_0401.SoftwareKind.Maintenance;
                    cmd0401.Control = CommandControlParameter.Start;
                    Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((int)moduleindex, cmd0401);
                }
            }

            // ソフトウエア識別コマンド（ラック搬送）
            RackTransferCommCommand_0001 cmd0001 = new RackTransferCommCommand_0001();
            cmd0001.SoftwareKind1 = RackTransferCommCommand_0001.SoftwareKind.Maintenance;
            cmd0001.Control = CommandControlParameter.Start;
            Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(cmd0001);

            // コマンド受信イベント登録
            Singleton<CarisXCommMessageManager>.Instance.ReceiveCommCommand += new EventHandler<CommCommandEventArgs>(this.recvEvent);

            // 通知登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ParameterResponse, RecvParameter);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.MaintenanceAbort, RecvAbort);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ModuleConnect, moduleConnect);

            SetCulture();
            SetNaviKey();
            //接続するスレーブのマックス件数に合わせてタブを非表示にする
            SetVisibleModuleTab(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected);
            InitializationLastestDisp(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected);
            CreateInstanceSubFormArray(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected);
            this.TabControl.Tabs[(Int32)ModuleTabIndex.RackTransfer].Selected = true;
            CreateInstanceSubForm((Int32)RackModuleIndex.RackTransfer);

            ShowSubForm(subFormSampleRackTransfer);

            Singleton<ResponseLog>.Instance.UnitName = uipNavigatorMenu.SelectedGroup.Text;

            this.ToolbarsManager.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.ToolbarsManager_BeforeToolbarListDropdown);

            Singleton<Oelco.CarisX.Common.MaintenanceJournalInfoManager>.Instance.SetMaintenanceJournalType(MaintenanceJournalType.Serviceman);

            // メンテナンス日誌開くかチェックします
            if (Singleton<Oelco.CarisX.Common.MaintenanceJournalInfoManager>.Instance.IsShow())
            {
                // メンテナンス日誌（サービスマン用）を表示
                Oelco.CarisX.GUI.DlgMaintenanceList dlgMaintenanceList = new Oelco.CarisX.GUI.DlgMaintenanceList(MaintenanceJournalType.Serviceman);
                dlgMaintenanceList.ShowDialog();
                
            }
        }

        private void ToolbarsManager_BeforeToolbarListDropdown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        /// モジュール切替時待機用ダイアログの表示処理
        /// </summary>
        /// <remarks>タブの切替時に表示したいので、別スレッドを作成してそちらで表示する</remarks>
        private void ShowDialogWaitModuleChange()
        {
            DlgWaitModule = new DlgWaitModuleChange();
            DlgWaitModule.ShowDialog();
            }

        /// <summary>
        /// Module1～4タブの表示制御を行う
        /// </summary>
        private void InitializationLastestDisp(int SlaveConnectCount)
        {
            LastestDisp = new MaintenanceMainNavi[SlaveConnectCount + 1];
            LastestDisp[(int)ModuleTabIndex.RackTransfer] = MaintenanceMainNavi.RackAllUnits;
            for (int i = 1; i < LastestDisp.Length; i++)
            {
                LastestDisp[i] = MaintenanceMainNavi.TipCellCaseTransferUnit;
            }
        }

        /// <summary>
        /// Module1～4タブの表示制御を行う
        /// </summary>
        private void SetVisibleModuleTab(int ConnectCount)
        {
            //ConnectCount以下のModuleタブだけ表示する
            TabControl.Tabs[(Int32)ModuleTabIndex.Slave1].Visible = ((Int32)ModuleTabIndex.Slave1 <= ConnectCount);
            TabControl.Tabs[(Int32)ModuleTabIndex.Slave2].Visible = ((Int32)ModuleTabIndex.Slave2 <= ConnectCount);
            TabControl.Tabs[(Int32)ModuleTabIndex.Slave3].Visible = ((Int32)ModuleTabIndex.Slave3 <= ConnectCount);
            TabControl.Tabs[(Int32)ModuleTabIndex.Slave4].Visible = ((Int32)ModuleTabIndex.Slave4 <= ConnectCount);

            TabControl.Tabs[(Int32)ModuleTabIndex.RackTransfer].Enabled = 
                (Singleton<SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.RackTransfer] != SystemStatusKind.NoLink);
            TabControl.Tabs[(Int32)ModuleTabIndex.Slave1].Enabled = (Singleton<SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.Module1] != SystemStatusKind.NoLink);
            TabControl.Tabs[(Int32)ModuleTabIndex.Slave2].Enabled = (Singleton<SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.Module2] != SystemStatusKind.NoLink);
            TabControl.Tabs[(Int32)ModuleTabIndex.Slave3].Enabled = (Singleton<SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.Module3] != SystemStatusKind.NoLink);
            TabControl.Tabs[(Int32)ModuleTabIndex.Slave4].Enabled = (Singleton<SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.Module4] != SystemStatusKind.NoLink);

        }

        /// <summary>
        /// リソースから画面テキストを表示する
        /// </summary>
        private void SetCulture()
        {
            ToolbarsManager.Tools[0].SharedProps.Caption = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_000;//Start
            ToolbarsManager.Tools[1].SharedProps.Caption = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_001;//Pause
            ToolbarsManager.Tools[2].SharedProps.Caption = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_002;//Restart
            ToolbarsManager.Tools[3].SharedProps.Caption = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_003;//Abort
            ToolbarsManager.Tools[4].SharedProps.Caption = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_004;//Save

            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.RackAllUnits].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_005;                                         //Rack All Units
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.TipCellCaseTransferUnit].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_006;                              //Tip / Cell Case Transfer Unit
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.ReagentStorageUnit].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_007;                                   //Reagent Storage Unit
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.STATUnit].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_008;                                             //STAT Unit
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.SampleDispenseUnit].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_009;                                   //Sample Dispense Unit
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.ReactionCellTransferUnit].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_010;                             //Reaction Cell Transfer Unit
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.ReactionTableUnit].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_011;                                    //Reaction Table Unit
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.BFTableUnit].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_012;                                          //BF Table Unit
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.TravelerandDisposalUnit].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_013;                              //Traveler and Disposal Unit
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.ReagentDispense1Unit].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_014;                                 //Reagent Dispense 1 Unit
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.ReagentDispense2Unit].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_015;                                 //Reagent Dispense 2 Unit
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.BF1UnitandBF1WasteLiquidUnit].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_016;                         //BF 1 Unit and BF 1 Waste Liquid Unit
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.BF2Unit].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_017;                                              //BF 2 Unit
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.DiluentDispenseUnit].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_018;                                  //Diluent Dispense Unit
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.PreTriggerDispenseUnit].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_019;                               //Pre-Trigger Dispense Unit
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.TriggerDispensingUnitandChemiluminescenceMeasUnit].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_020;    //Trigger Dispensing Unit, Chemiluminescence Meas. Unit
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.FluidandPipingUnit].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_021;                                   //Fluid and Piping Unit
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.RackSensorUnit].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_030;                                       //Rack Senser Unit
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.SlaveSensorUnit].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_031;                                      //Module Senser Unit
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.TempUnit].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_032;                                             //Temp Unit
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.Other].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_022;                                                //Other

            btExit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_023;//Exit

            TabControl.Tabs[(Int32)ModuleTabIndex.RackTransfer].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_024;//RackTransfer
            TabControl.Tabs[(Int32)ModuleTabIndex.Slave1].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_025;//Slave1
            TabControl.Tabs[(Int32)ModuleTabIndex.Slave2].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_026;//Slave2
            TabControl.Tabs[(Int32)ModuleTabIndex.Slave3].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_027;//Slave3
            TabControl.Tabs[(Int32)ModuleTabIndex.Slave4].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_028;//Slave4

            btMaintenanceJournal.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_033;

        }

        /// <summary>
        /// ナビバーの各グループのキー値を設定する
        /// </summary>
        /// <remarks>
        /// ナビバーのどのグループが選択されたかの判定に使用
        /// </remarks>
        public void SetNaviKey()
        {
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.RackAllUnits].Key = MaintenanceMainNavi.RackAllUnits.ToString();
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.TipCellCaseTransferUnit].Key = MaintenanceMainNavi.TipCellCaseTransferUnit.ToString();
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.ReagentStorageUnit].Key = MaintenanceMainNavi.ReagentStorageUnit.ToString();
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.STATUnit].Key = MaintenanceMainNavi.STATUnit.ToString();
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.SampleDispenseUnit].Key = MaintenanceMainNavi.SampleDispenseUnit.ToString();
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.ReactionCellTransferUnit].Key = MaintenanceMainNavi.ReactionCellTransferUnit.ToString();
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.ReactionTableUnit].Key = MaintenanceMainNavi.ReactionTableUnit.ToString();
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.BFTableUnit].Key = MaintenanceMainNavi.BFTableUnit.ToString();
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.TravelerandDisposalUnit].Key = MaintenanceMainNavi.TravelerandDisposalUnit.ToString();
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.ReagentDispense1Unit].Key = MaintenanceMainNavi.ReagentDispense1Unit.ToString();
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.ReagentDispense2Unit].Key = MaintenanceMainNavi.ReagentDispense2Unit.ToString();
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.BF1UnitandBF1WasteLiquidUnit].Key = MaintenanceMainNavi.BF1UnitandBF1WasteLiquidUnit.ToString();
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.BF2Unit].Key = MaintenanceMainNavi.BF2Unit.ToString();
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.DiluentDispenseUnit].Key = MaintenanceMainNavi.DiluentDispenseUnit.ToString();
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.PreTriggerDispenseUnit].Key = MaintenanceMainNavi.PreTriggerDispenseUnit.ToString();
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.TriggerDispensingUnitandChemiluminescenceMeasUnit].Key = MaintenanceMainNavi.TriggerDispensingUnitandChemiluminescenceMeasUnit.ToString();
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.FluidandPipingUnit].Key = MaintenanceMainNavi.FluidandPipingUnit.ToString();
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.RackSensorUnit].Key = MaintenanceMainNavi.RackSensorUnit.ToString();
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.SlaveSensorUnit].Key = MaintenanceMainNavi.SlaveSensorUnit.ToString();
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.TempUnit].Key = MaintenanceMainNavi.TempUnit.ToString();
            uipNavigatorMenu.Groups[(int)MaintenanceMainNavi.Other].Key = MaintenanceMainNavi.Other.ToString();
        }

        /// <summary>
        /// サブ画面のインスタンスを管理する配列の要素を作成する
        /// </summary>
        public void CreateInstanceSubFormArray(int ConnectCount)
        {
            subFormCaseTransfer = new FormCaseTransferUnit[ConnectCount];
            subFormReagentStorage = new FormReagentStorageUnit[ConnectCount];
            subFormSTAT = new FormSTATUnit[ConnectCount];
            subFormSampleDispense = new FormSampleDispenseUnit[ConnectCount];
            subFormReactionCellTransfer = new FormReactionCellTransferUnit[ConnectCount];
            subFormReactionTable = new FormReactionTableUnit[ConnectCount];
            subFormBFTable = new FormBFTableUnit[ConnectCount];
            subFormTravelerDisposal = new FormTravelerDisposalUnit[ConnectCount];
            subFormR1Dispense = new FormR1DispenseUnit[ConnectCount];
            subFormR2Dispense = new FormR2DispenseUnit[ConnectCount];
            subFormBF1 = new FormBF1Unit[ConnectCount];
            subFormBF2 = new FormBF2Unit[ConnectCount];
            subFormDiluentDispense = new FormDiluentDispenseUnit[ConnectCount];
            subFormPretrigger = new FormPretriggerUnit[ConnectCount];
            subFormTriggerDispense = new FormTriggerDispenseUnit[ConnectCount];
            subFormFluidPiping = new FormFluidPipingUnit[ConnectCount];
            subFormSensor = new FormSensorUnit[ConnectCount];
            subFormTemp = new FormTempUnit[ConnectCount];
            subFormOther = new FormOther[ConnectCount];
        }

        /// <summary>
        /// 選択されているタブ・ユニットに対応するフォームのインスタンスを返す
        /// </summary>
        public FormUnitBase selectSubForm(MaintenanceMainNavi currentNavi, int idx)
        {
            FormUnitBase returnForm = null;

            switch (currentNavi)
            {
                case MaintenanceMainNavi.RackAllUnits:
                    //ラック搬送部
                    returnForm = subFormSampleRackTransfer;
                    break;

                case MaintenanceMainNavi.TipCellCaseTransferUnit:
                    //ケース搬送
                    returnForm = subFormCaseTransfer[idx];
                    break;

                case MaintenanceMainNavi.ReagentStorageUnit:
                    //試薬保冷庫
                    returnForm = subFormReagentStorage[idx];
                    break;

                case MaintenanceMainNavi.STATUnit:
                    //スタット部
                    returnForm = subFormSTAT[idx];
                    break;

                case MaintenanceMainNavi.SampleDispenseUnit:
                    //サンプル分注移送部
                    returnForm = subFormSampleDispense[idx];
                    break;

                case MaintenanceMainNavi.ReactionCellTransferUnit:
                    //反応容器搬送部
                    returnForm = subFormReactionCellTransfer[idx];
                    break;

                case MaintenanceMainNavi.ReactionTableUnit:
                    //反応テーブル部
                    returnForm = subFormReactionTable[idx];
                    break;

                case MaintenanceMainNavi.BFTableUnit:
                    //BFテーブル部
                    returnForm = subFormBFTable[idx];
                    break;

                case MaintenanceMainNavi.TravelerandDisposalUnit:
                    //トラベラー・廃棄部
                    returnForm = subFormTravelerDisposal[idx];
                    break;

                case MaintenanceMainNavi.ReagentDispense1Unit:
                    //試薬分注1部
                    returnForm = subFormR1Dispense[idx];
                    break;

                case MaintenanceMainNavi.ReagentDispense2Unit:
                    //試薬分注2部
                    returnForm = subFormR2Dispense[idx];
                    break;

                case MaintenanceMainNavi.BF1UnitandBF1WasteLiquidUnit:
                    //BF1部、BF1廃液部
                    returnForm = subFormBF1[idx];
                    break;

                case MaintenanceMainNavi.BF2Unit:
                    //BF2分注ユニット
                    returnForm = subFormBF2[idx];
                    break;

                case MaintenanceMainNavi.DiluentDispenseUnit:
                    //希釈液分注部
                    returnForm = subFormDiluentDispense[idx];
                    break;

                case MaintenanceMainNavi.PreTriggerDispenseUnit:
                    //プレトリガ
                    returnForm = subFormPretrigger[idx];
                    break;

                case MaintenanceMainNavi.TriggerDispensingUnitandChemiluminescenceMeasUnit:
                    //トリガ分注、化学発光測定部
                    returnForm = subFormTriggerDispense[idx];
                    break;

                case MaintenanceMainNavi.FluidandPipingUnit:
                    //流体配管部
                    returnForm = subFormFluidPiping[idx];
                    break;

                case MaintenanceMainNavi.RackSensorUnit:
                    //ラックセンサー部
                    returnForm = subFormRackSensor;
                    break;

                case MaintenanceMainNavi.SlaveSensorUnit:
                    //スレーブセンサー部
                    returnForm = subFormSensor[idx];
                    break;

                case MaintenanceMainNavi.TempUnit:
                    //温度測定部
                    returnForm = subFormTemp[idx];
                    break;

                case MaintenanceMainNavi.Other:
                    //その他
                    returnForm = subFormOther[idx];
                    break;
            }

            return returnForm;
        }

        /// <summary>
        /// 選択されたナビバーに応じてサブ画面を表示する
        /// </summary>
        private void uipNavigatorMenu_SelectedGroupChanged(object sender, Infragistics.Win.UltraWinExplorerBar.GroupEventArgs e)
        {
            ToolbarsManager.Enabled = false;

            if (!(null == uipNavigatorMenu.ActiveGroup))
            {
                //TabControl_SelectedTabChangedで切り替えた際、ActiveGroupが設定されていない為、
                //設定されている場合のみcurrentDispを置き換えるようにする
                currentDisp = (MaintenanceMainNavi)uipNavigatorMenu.ActiveGroup.Index;
            }

            FormUnitBase subForm = selectSubForm(currentDisp, moduleIndex);
            ShowSubForm(subForm);

            Singleton<ResponseLog>.Instance.UnitName = uipNavigatorMenu.SelectedGroup.Text;

            ToolbarsManager.Enabled = true;
        }

        /// <summary>
        /// サブ画面を表示する
        /// </summary>
        private void ShowSubForm(FormUnitBase subform)
        {
            subform.TopLevel = false;
            ContentPanel.ClientArea.Controls.Clear();
            ContentPanel.ClientArea.Controls.Add(subform);
            subform.ToolbarsDisp = ToolbarsDisp;
            subform.MainToolbarsDispEnable = MainNavigatorEsenable;
            subform.Show();
            subform.ToolbarsControl();
            subform.ConfigParamLoad();
            subform.MotorParamDisp();
            if (subform is FormTempUnit)
            {
                FormTempUnit subFormTempUnit = subform as FormTempUnit;
                if (subFormTempUnit.timeCount != 0)
                    ToolBarControl(ToolBarEnable.SensorAndTempStart);
            }

        }

        /// <summary>
        /// スタート、ポーズ、リスタート、終了、保存のボタン押下
        /// </summary>
        private void ToolbarsManager_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            DialogResult result = System.Windows.Forms.DialogResult.OK;
            switch (e.Tool.Key)
            {
                case "StartBt"://開始
                    switch (currentDisp)
                    {
                        case MaintenanceMainNavi.RackSensorUnit:
                        case MaintenanceMainNavi.SlaveSensorUnit:
                        case MaintenanceMainNavi.TempUnit:
                            ToolBarControl(ToolBarEnable.SensorAndTempStart);
                            break;
                        default:
                            ToolBarControl(ToolBarEnable.Start);
                            break;
                    }
                    break;
                case "PauseBt"://ポーズ
                    ToolBarControl(ToolBarEnable.Pause);
                    break;
                case "RestartBt"://リスタート
                    ToolBarControl(ToolBarEnable.Restart);
                    break;
                case "AbortBt"://終了
                    ToolBarControl(ToolBarEnable.Disable);
                    break;
                case "SaveBt"://保存
                    DlgMaintenance dlg = new DlgMaintenance(Properties.Resources_Maintenance.DlgSaveMsg, true);
                    result = dlg.ShowDialog();
                    break;
            }
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                switch (e.Tool.Key)
                {
                    case "SaveBt"://保存
                        ToolBarControl(ToolBarEnable.Disable);
                        break;
                }
                ToolBarsExecute(e.Tool.Key);
            }
        }

        /// <summary>
        /// コマンドバーのSTARTボタンが押された時の処理
        /// </summary>
        private void ToolBarsExecute(string Key)
        {
            FormUnitBase subForm = selectSubForm(currentDisp, moduleIndex);
            ModuleKind modulekind;
            if (currentDisp == MaintenanceMainNavi.RackAllUnits)
            {
                modulekind = ModuleKind.RackTransfer;
            }
            else
            {
                modulekind = ModuleKind.Slave;
            };

            subFormExecute(Key, currentDisp, subForm, modulekind);
        }

        /// <summary>
        /// サブ画面で定義されているコマンドバーのボタンに対応する処理を実行する
        /// </summary>
        /// <param name="Key">コマンドバーの値</param>
        /// <param name="subForm">対応するサブ画面</param>
        /// <param name="ModuleKind">スレーブかラック搬送か</param>
        public void subFormExecute(string Key, MaintenanceMainNavi DispUnit, FormUnitBase subForm, ModuleKind ModuleKind = ModuleKind.Slave)
        {
            Boolean FormEnabled;    //スタート時のフォームの活性状態を指定
            switch (DispUnit)
            {
                case MaintenanceMainNavi.RackSensorUnit:
                case MaintenanceMainNavi.SlaveSensorUnit:
                case MaintenanceMainNavi.TempUnit:
                    FormEnabled = true;
                    break;
                default:
                    FormEnabled = false;
                    break;
            }

            if (Key == "StartBt")
            {
                subForm.Enabled = FormEnabled;
                subForm.UnitStart();
            }
            if (Key == "PauseBt")
            {
                subForm.UnitPause(ModuleKind);
            }
            if (Key == "RestartBt")
            {
                subForm.UnitRestart(ModuleKind);
            }
            if (Key == "AbortBt")
            {
                subForm.Enabled = false;
                if (!subForm.UnitAbortexe())
                {
                    //モーター調整で「本当に停止する？」にNoと答えた場合はこっち
                    ToolBarControl(ToolBarEnable.Adjust);
                    subForm.Enabled = true;
                }
            }
            if (Key == "SaveBt")
            {
                //保存内容をスレーブ/ラックに送信中に画面操作ができないようにする
                subForm.Enabled = false;
                subForm.ParamSave();
            }
        }

        /// <summary>
        /// ツールバーの各ボタンの活性/非活性の制御
        /// </summary>
        private void ToolBarControl(ToolBarEnable tool)
        {
            switch (tool)
            {
                case ToolBarEnable.Default:
                    //デフォルトボタン制御
                    ToolbarsManager.Tools[0].SharedProps.Enabled = true;//開始
                    ToolbarsManager.Tools[1].SharedProps.Enabled = false;//停止
                    ToolbarsManager.Tools[2].SharedProps.Enabled = false;//再開
                    ToolbarsManager.Tools[3].SharedProps.Enabled = false;//終了
                    ToolbarsManager.Tools[4].SharedProps.Enabled = false;//保存
                    break;
                case ToolBarEnable.Start:
                    //メンテナンスメイン画面Disable
                    //ユニットがユニットテストの場合のみ、停止ボタンを活性化する
                    FormUnitBase subForm;
                    subForm = selectSubForm(currentDisp, moduleIndex);

                    uipNavigatorMenu.Enabled = false;
                    btExit.Enabled = false;
                    TabControl.Enabled = false;
                    ToolbarsManager.Tools[0].SharedProps.Enabled = false;//開始
                    ToolbarsManager.Tools[1].SharedProps.Enabled = (subForm.UnitMode == MaintenanceUnitMode.Test);//停止
                    ToolbarsManager.Tools[2].SharedProps.Enabled = false;//再開
                    ToolbarsManager.Tools[3].SharedProps.Enabled = true;//終了
                    ToolbarsManager.Tools[4].SharedProps.Enabled = false;//保存
                    break;
                case ToolBarEnable.Pause:
                    ToolbarsManager.Tools[0].SharedProps.Enabled = false;//開始
                    ToolbarsManager.Tools[1].SharedProps.Enabled = false;//停止
                    ToolbarsManager.Tools[2].SharedProps.Enabled = true;//再開
                    ToolbarsManager.Tools[3].SharedProps.Enabled = true;//終了
                    ToolbarsManager.Tools[4].SharedProps.Enabled = false;//保存
                    break;
                case ToolBarEnable.Restart:
                    ToolbarsManager.Tools[0].SharedProps.Enabled = false;//開始
                    ToolbarsManager.Tools[1].SharedProps.Enabled = true;//停止
                    ToolbarsManager.Tools[2].SharedProps.Enabled = false;//再開
                    ToolbarsManager.Tools[3].SharedProps.Enabled = true;//終了
                    ToolbarsManager.Tools[4].SharedProps.Enabled = false;//保存
                    break;
                case ToolBarEnable.Abort:
                    //メンテナンスメイン画面Disable
                    uipNavigatorMenu.Enabled = true;

                    if (chkTempInquiryRunning())
                    {
                        btExit.Enabled = false;
                        TabControl.Enabled = false;
                    }
                    else
                    {
                        btExit.Enabled = true;
                        TabControl.Enabled = true;
                    }
                    ToolbarsManager.Tools[0].SharedProps.Enabled = true;//開始
                    ToolbarsManager.Tools[1].SharedProps.Enabled = false;//停止
                    ToolbarsManager.Tools[2].SharedProps.Enabled = false;//再開
                    ToolbarsManager.Tools[3].SharedProps.Enabled = false;//終了
                    ToolbarsManager.Tools[4].SharedProps.Enabled = false;//保存
                    break;
                case ToolBarEnable.Disable:
                    uipNavigatorMenu.Enabled = false;
                    btExit.Enabled = true;              //Exitボタンは押下できるようにする
                    TabControl.Enabled = false;
                    ToolbarsManager.Tools[0].SharedProps.Enabled = false;//開始
                    ToolbarsManager.Tools[1].SharedProps.Enabled = false;//停止
                    ToolbarsManager.Tools[2].SharedProps.Enabled = false;//再開
                    ToolbarsManager.Tools[3].SharedProps.Enabled = false;//終了
                    ToolbarsManager.Tools[4].SharedProps.Enabled = false;//保存
                    break;
                case ToolBarEnable.DefaultSave:
                    ToolbarsManager.Tools[0].SharedProps.Enabled = false;//開始
                    ToolbarsManager.Tools[1].SharedProps.Enabled = false;//停止
                    ToolbarsManager.Tools[2].SharedProps.Enabled = false;//再開
                    ToolbarsManager.Tools[3].SharedProps.Enabled = false;//終了
                    ToolbarsManager.Tools[4].SharedProps.Enabled = true;//保存
                    break;
                case ToolBarEnable.SensorAndTempStart:
                    btExit.Enabled = false;     //メンテナンス画面の終了は不可
                    TabControl.Enabled = false; //モジュールの変更は不可
                    ToolbarsManager.Tools[0].SharedProps.Enabled = false;//開始
                    ToolbarsManager.Tools[1].SharedProps.Enabled = false;//停止
                    ToolbarsManager.Tools[2].SharedProps.Enabled = false;//再開
                    ToolbarsManager.Tools[3].SharedProps.Enabled = true;//終了
                    ToolbarsManager.Tools[4].SharedProps.Enabled = false;//保存
                    break;
                case ToolBarEnable.Adjust:
                    btExit.Enabled = false;     //メンテナンス画面の終了は不可
                    ToolbarsManager.Tools[0].SharedProps.Enabled = false;//開始
                    ToolbarsManager.Tools[1].SharedProps.Enabled = false;//停止
                    ToolbarsManager.Tools[2].SharedProps.Enabled = false;//再開
                    ToolbarsManager.Tools[3].SharedProps.Enabled = true;//終了
                    ToolbarsManager.Tools[4].SharedProps.Enabled = true;//保存
                    break;
                case ToolBarEnable.AdjustReplacingProbe:
                    uipNavigatorMenu.Enabled = true;
                    btExit.Enabled = true;     //メンテナンス画面の終了は可
                    TabControl.Enabled = true; //モジュールの変更は可
                    ToolbarsManager.Tools[0].SharedProps.Enabled = true;//開始
                    ToolbarsManager.Tools[1].SharedProps.Enabled = false;//停止
                    ToolbarsManager.Tools[2].SharedProps.Enabled = false;//再開
                    ToolbarsManager.Tools[3].SharedProps.Enabled = false;//終了
                    ToolbarsManager.Tools[4].SharedProps.Enabled = true;//保存
                    break;
            }
        }

        /// <summary>
        /// 定周期タイマー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            // 受信キュー移動
            Singleton<CarisXCommManager>.Instance.TransferReceiveQueueToMessageManager();
            // イベント発生
            Singleton<CarisXCommMessageManager>.Instance.RaiseEvent();
        }

        /// <summary>
        /// コマンド受信イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="command"></param>
        private void recvEvent(object sender, CommCommandEventArgs command)
        {
            Console.WriteLine("FormMaintenanceMain recvEvent");

            FormUnitBase subForm;

            //各レスポンスを受けた際の処理
            switch (command.Command.CommandId)
            {
                case (int)CommandKind.RackTransferCommand1012:
                case (int)CommandKind.RackTransferCommand1039:
                case (int)CommandKind.RackTransferCommand1073:
                case (int)CommandKind.RackTransferCommand1080:
                case (int)CommandKind.RackTransferCommand1081:
                case (int)CommandKind.Command1412:
                case (int)CommandKind.Command1439:
                case (int)CommandKind.Command1473:
                case (int)CommandKind.Command1480:
                case (int)CommandKind.Command1481:
                    subForm = selectSubForm(currentDisp, moduleIndex);
                    subForm.SetResponse(command);
                    break;
                case (int)CommandKind.RackTransferCommand1040:
                    //ラックセンサーステータスコマンドは必ずラックセンサーユニットで受ける
                    subForm = selectSubForm(MaintenanceMainNavi.RackSensorUnit, moduleIndex);
                    subForm.SetResponse(command);
                    break;
                case (int)CommandKind.Command1437:
                case (int)CommandKind.Command1438:
                    //温度問合せコマンドは必ず温度ユニットで受ける
                    subForm = selectSubForm(MaintenanceMainNavi.TempUnit, moduleIndex);
                    subForm.SetResponse(command);
                    break;
                case (int)CommandKind.Command1440:
                    //スレーブセンサーステータスコマンドは必ずスレーブセンサーユニットで受ける
                    subForm = selectSubForm(MaintenanceMainNavi.SlaveSensorUnit, moduleIndex);
                    subForm.SetResponse(command);
                    break;
                case (int)CommandKind.Command1497:
                    subForm = selectSubForm(currentDisp, moduleIndex);
                    subForm.SetResponse(command);
                    ToolBarControl(ToolBarEnable.AdjustReplacingProbe);
                    break;
            }

            //その他の処理
            switch (command.Command.CommandId)
            {
                case (int)CommandKind.RackTransferCommand1080:
                case (int)CommandKind.Command1480:
                    //調整開始コマンドのレスポンスを受けた場合ツールバー制御
                    ToolBarControl(ToolBarEnable.Adjust);
                    break;
            }
        }

        /// <summary>
        /// 停止コマンドに対するレスポンス受信時の処理
        /// </summary>
        /// <param name="Comm"></param>
        private void RecvAbort(object Comm)
        {
            if (Comm != null)
            {
                //制御を戻す
                ToolBarControl(ToolBarEnable.Abort);
            }
        }

        /// <summary>
        /// 各パラメータ保存コマンドに対するレスポンス受信時の処理
        /// </summary>
        /// <param name="Comm"></param>
        private void RecvParameter(object Comm)
        {
            FormUnitBase subForm;
            subForm = selectSubForm(currentDisp, moduleIndex);

            Boolean SendResult;
            if (Comm is bool)
            {
                SendResult = (bool)Comm;
            }
            else
            {
                SendResult = false;
            }

            subForm.SetParameterResponse(SendResult);
        }

        /// <summary>
        /// 子画面からタブを選択された時のツールバーの状態を切り替えます。
        /// </summary>
        private void ToolbarsDisp(int tabindex, int toolbarenable = -1)
        {
            ToolBarEnable toolenable = ToolBarEnable.Default;

            //ToolBarEnableのenumに変換できる値がtoolbarenableに設定されている場合、
            //toolbarenableに設定された内容でツールバーの状態を切り替える
            if (Enum.IsDefined(typeof(ToolBarEnable), toolbarenable))
            {
                toolenable = (ToolBarEnable)toolbarenable;
            }
            else
            {
                switch (tabindex)
                {
                    case (int)MaintenanceTabIndex.Test:
                    case (int)MaintenanceTabIndex.MAdjust:
                        toolenable = ToolBarEnable.Default;
                        break;
                    case (int)MaintenanceTabIndex.Config:
                    case (int)MaintenanceTabIndex.MParam:
                        toolenable = ToolBarEnable.DefaultSave;
                        break;
                }
            }

            ToolBarControl(toolenable);
        }

        /// <summary>
        /// 子画面からMaintenanceMain画面をEnable/Disableにします。
        /// </summary>
        private void MainNavigatorEsenable(bool enable)
        {
            if (enable)
            {
                uipNavigatorMenu.Enabled = true;

                if (chkTempInquiryRunning())
                {
                    btExit.Enabled = false;
                    TabControl.Enabled = false;
                }
                else
                {
                    btExit.Enabled = true;
                    TabControl.Enabled = true;
                }

                switch (currentDisp)
                {
                    case MaintenanceMainNavi.TempUnit:
                    case MaintenanceMainNavi.RackSensorUnit:
                    case MaintenanceMainNavi.SlaveSensorUnit:
                        break;
                    default:
                        ToolBarControl(ToolBarEnable.Default);
                        break;
                }
            }
            else
            {
                uipNavigatorMenu.Enabled = false;
                btExit.Enabled = false;
                TabControl.Enabled = false;
            }
        }

        /// <summary>
        /// フォーム終了イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMaintenanceMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 各モジュールへソフトウエア識別コマンドを送信
            foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                // オンラインとなっているモジュールへ送信
                if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                {
                    SlaveCommCommand_0401 cmd0401 = new SlaveCommCommand_0401();
                    cmd0401.SoftwareKind1 = SlaveCommCommand_0401.SoftwareKind.Running;
                    cmd0401.Control = CommandControlParameter.Start;
                    Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(moduleindex, cmd0401);
                }
            }

            // ラック搬送へソフトウエア識別コマンドを送信
            RackTransferCommCommand_0001 racktransfer_cmd0001 = new RackTransferCommCommand_0001();
            racktransfer_cmd0001.SoftwareKind1 = RackTransferCommCommand_0001.SoftwareKind.Running;
            racktransfer_cmd0001.Control = CommandControlParameter.Start;
            Singleton<CarisXCommManager>.Instance.PushSendQueueRackTransfer(racktransfer_cmd0001);
        }

        /// <summary>
        /// タブ変更時の処理をおこないます。
        /// </summary>
        /// <remarks>ラック、モジュール１～４の切替を行う</remarks>
        private void TabControl_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            Singleton<ResponseLog>.Instance.ModuleName = TabControl.SelectedTab.Text;

            if (e.PreviousSelectedTab != null)
            {
                //待機用ダイアログを表示する
                threadPleaseWait = new Thread(new ThreadStart(ShowDialogWaitModuleChange));
                threadPleaseWait.Start();

                //前回選択していたタブにある各ユニットのインスタンスを破棄
                DisposeInstanceSubForm(e.PreviousSelectedTab.Index);
            }

            switch (TabControl.SelectedTab.Index)
            {
                case (Int32)ModuleTabIndex.RackTransfer:
                    ToolbarsManager.Enabled = false;

                    //各ユニットの画面のインスタンスを作成する
                    CreateInstanceSubForm(TabControl.SelectedTab.Index);

                    Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex = (Int32)RackTransferNo.RackTransfer1;

                    // uipNavigatorMenuはグループを増やすと下から上に伸びていくため、
                    // 表示させたいグループを増減させる場合はサイズを調整しないと余白または表示されないグループが出てくる
                    ultraPanel2.Size = new Size(393, 76);
                    uipNavigatorMenu.Size = new Size(393, 86);
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.RackAllUnits].Visible = true;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.TipCellCaseTransferUnit].Visible = false;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.ReagentStorageUnit].Visible = false;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.STATUnit].Visible = false;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.SampleDispenseUnit].Visible = false;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.ReactionCellTransferUnit].Visible = false;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.ReactionTableUnit].Visible = false;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.BFTableUnit].Visible = false;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.TravelerandDisposalUnit].Visible = false;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.ReagentDispense1Unit].Visible = false;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.ReagentDispense2Unit].Visible = false;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.BF1UnitandBF1WasteLiquidUnit].Visible = false;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.BF2Unit].Visible = false;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.DiluentDispenseUnit].Visible = false;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.PreTriggerDispenseUnit].Visible = false;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.TriggerDispensingUnitandChemiluminescenceMeasUnit].Visible = false;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.FluidandPipingUnit].Visible = false;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.RackSensorUnit].Visible = true;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.SlaveSensorUnit].Visible = false;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.TempUnit].Visible = false;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.Other].Visible = false;

                    break;

                case (Int32)ModuleTabIndex.Slave1:
                case (Int32)ModuleTabIndex.Slave2:
                case (Int32)ModuleTabIndex.Slave3:
                case (Int32)ModuleTabIndex.Slave4:
                    ToolbarsManager.Enabled = false;

                    //選択されたタブに応じて、取得対象となるモジュールＩＤが何か特定する
                    switch (TabControl.SelectedTab.Index)
                    {
                        case (Int32)ModuleTabIndex.Slave1:
                            moduleIndex = (Int32)ModuleIndex.Module1;
                            break;
                        case (Int32)ModuleTabIndex.Slave2:
                            moduleIndex = (Int32)ModuleIndex.Module2;
                            break;
                        case (Int32)ModuleTabIndex.Slave3:
                            moduleIndex = (Int32)ModuleIndex.Module3;
                            break;
                        case (Int32)ModuleTabIndex.Slave4:
                            moduleIndex = (Int32)ModuleIndex.Module4;
                            break;
                    }

                    //XMLから取得した情報の内、対応するモジュールIDを持つListのインデックスを返す
                    ParameterFilePreserve<CarisXConfigParameter> config = Singleton<ParameterFilePreserve<CarisXConfigParameter>>.Instance;
                    config.LoadRaw();
                    for (int i = 0; i < config.Param.SlaveList.Count(); i++)
                    {
                        if (config.Param.SlaveList[i].ModuleId == moduleIndex)
                        {
                            Singleton<CarisXCommManager>.Instance.SelectedSlaveIndex = i;
                            break;
                        }
                    }

                    //サブ画面のインスタンスを作成する
                    CreateInstanceSubForm(TabControl.SelectedTab.Index);

                    // uipNavigatorMenuはグループを増やすと下から上に伸びていくため、
                    // 表示させたいグループを増減させる場合はサイズを調整しないと余白または表示されないグループが出てくる
                    ultraPanel2.Size = new Size(393, 722);
                    uipNavigatorMenu.Size = new Size(393, 732);
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.RackAllUnits].Visible = false;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.TipCellCaseTransferUnit].Visible = true;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.ReagentStorageUnit].Visible = true;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.STATUnit].Visible = true;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.SampleDispenseUnit].Visible = true;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.ReactionCellTransferUnit].Visible = true;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.ReactionTableUnit].Visible = true;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.BFTableUnit].Visible = true;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.TravelerandDisposalUnit].Visible = true;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.ReagentDispense1Unit].Visible = true;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.ReagentDispense2Unit].Visible = true;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.BF1UnitandBF1WasteLiquidUnit].Visible = true;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.BF2Unit].Visible = true;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.DiluentDispenseUnit].Visible = true;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.PreTriggerDispenseUnit].Visible = true;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.TriggerDispensingUnitandChemiluminescenceMeasUnit].Visible = true;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.FluidandPipingUnit].Visible = true;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.RackSensorUnit].Visible = false;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.SlaveSensorUnit].Visible = true;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.TempUnit].Visible = true;
                    uipNavigatorMenu.Groups[(Int32)MaintenanceMainNavi.Other].Visible = true;

                    break;

                default:
                    break;
            }

            //元々選択されていたタブで最後に画面に表示していたユニットは何か？を保存する
            if (e.PreviousSelectedTab != null)
            {
                LastestDisp[e.PreviousSelectedTab.Index] = currentDisp;
            }

            //前回このタブで表示されていたユニットを再度選択する
            uipNavigatorMenu.ActiveGroup = null;
            currentDisp = LastestDisp[TabControl.SelectedTab.Index];
            uipNavigatorMenu.Groups[(Int32)LastestDisp[TabControl.SelectedTab.Index]].Selected = true;

            //スレーブを切り替えてもユニットが同じだった場合、ナビバーのイベントで表示し直してくれないので、ここで表示し直す。
            if (e.PreviousSelectedTab != null && LastestDisp[e.PreviousSelectedTab.Index] == currentDisp)
            {
                FormUnitBase subForm = selectSubForm(currentDisp, moduleIndex);
                ShowSubForm(subForm);
            }

            ToolbarsManager.Enabled = true;

            if (e.PreviousSelectedTab != null)
            {
                // 描画が遅れているため、ここで表示処理を行う
                this.Refresh();

                // モジュール切替時待機用ダイアログの表示処理を行っているスレッドで、このメソッドを実行する
                DlgWaitModule.Invoke((MethodInvoker)delegate { DlgWaitModule.Close(); });

                threadPleaseWait.Join();
                threadPleaseWait = null;
            }
        }

        /// <summary>
        /// Exitボタン押下時の処理をおこないます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExit_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            using (DlgMaintenance dlg = new DlgMaintenance(Properties.Resources_Maintenance.DlgMotorInitialize, true))
            {
                // メンテナンス日誌開くかチェックします
                if (Singleton<Oelco.CarisX.Common.MaintenanceJournalInfoManager>.Instance.IsShow())
                {
                    // メンテナンス日誌（サービスマン用）のcsv保存、xml保存を行う
                    Oelco.CarisX.GUI.DlgMaintenanceList dlgMaintenanceList = new Oelco.CarisX.GUI.DlgMaintenanceList(MaintenanceJournalType.Serviceman);
                    dlgMaintenanceList.ServicemanExit();
                    
                }

                DialogResult result = dlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    DlgWaitMotorInitialize dlgWaitMotorInitialize = new DlgWaitMotorInitialize();
                    if (dlgWaitMotorInitialize.IsNeedShow())
                    {
                        //モーター初期化が未完了（＝モジュールorラックへの接続がある）の場合は画面を表示する
                        dlgWaitMotorInitialize.ShowDialog();
                    }
                }
            }
            this.Close();

            Singleton<CarisXCommMessageManager>.Instance.ReceiveCommCommand -= new EventHandler<CommCommandEventArgs>(this.recvEvent);
            Singleton<NotifyManager>.Instance.RemoveNotifyTarget((Int32)NotifyKind.ParameterResponse, RecvParameter);
            Singleton<NotifyManager>.Instance.RemoveNotifyTarget((Int32)NotifyKind.MaintenanceAbort, RecvAbort);
            Singleton<NotifyManager>.Instance.RemoveNotifyTarget((Int32)NotifyKind.ModuleConnect, moduleConnect);
            Singleton<CarisXLogManager>.Instance.Write(LogKind.OperationHist, Singleton<Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
                , new String[] { Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_029 + Properties.Resources.STRING_COMMON_013 + Properties.Resources.STRING_LOG_MSG_068 });
            DisposeInstanceSubFormAll();
        }

        /// <summary>
        /// 温度問合せ実行状況
        /// </summary>
        /// <returns></returns>
        private Boolean chkTempInquiryRunning()
        {
            Boolean flgTempInquiryRunning = false;

            //ラック搬送部が選択されている場合は関係ない
            if (!TabControl.Tabs[(Int32)ModuleTabIndex.RackTransfer].Selected)
            {
                //ラック搬送部以外（＝スレーブ１～４）が選択されている場合、該当モジュールの温度ユニットで温度問合せが実行中かチェックする
                FormTempUnit subFormTempUnit = (FormTempUnit)selectSubForm(MaintenanceMainNavi.TempUnit, moduleIndex);
                if (subFormTempUnit.timeCount != 0)
                {
                    //温度問合せが実行中
                    flgTempInquiryRunning = true;
                }
            }

            return flgTempInquiryRunning;
        }

        /// <summary>
        /// サブ画面のインスタンスを作成する
        /// </summary>
        public void CreateInstanceSubForm(int moduleId)
        {
            if (moduleId == (Int32)RackModuleIndex.RackTransfer)
            {
                //ラック搬送の各ユニットのインスタンスを作成

                if (subFormSampleRackTransfer == null)
                {
                    subFormSampleRackTransfer = new FormSampleRackTransferUnit();
                    subFormRackSensor = new FormRackSensorUnit();
                }
            }
            else
            {
                //モジュール１～４のインスタンスを作成

                Int32 moduleIndex = Utility.CarisXSubFunction.ModuleIDToModuleIndex(moduleId);

                if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected <= moduleIndex)
                {
                    //Slaveに接続している最大数よりも大きい数値が来た場合は処理しない（プログラムのバグ以外は発生しない）
                    return;
                }

                if (subFormCaseTransfer[moduleIndex] == null)
                {
                    //対象のインデックスに対してまだインスタンスを作成していなかった場合のみ処理する

                    subFormCaseTransfer[moduleIndex] = new FormCaseTransferUnit();
                    subFormReagentStorage[moduleIndex] = new FormReagentStorageUnit();
                    subFormSTAT[moduleIndex] = new FormSTATUnit();
                    subFormSampleDispense[moduleIndex] = new FormSampleDispenseUnit();
                    subFormReactionCellTransfer[moduleIndex] = new FormReactionCellTransferUnit();
                    subFormReactionTable[moduleIndex] = new FormReactionTableUnit();
                    subFormBFTable[moduleIndex] = new FormBFTableUnit();
                    subFormTravelerDisposal[moduleIndex] = new FormTravelerDisposalUnit();
                    subFormR1Dispense[moduleIndex] = new FormR1DispenseUnit();
                    subFormR2Dispense[moduleIndex] = new FormR2DispenseUnit();
                    subFormBF1[moduleIndex] = new FormBF1Unit();
                    subFormBF2[moduleIndex] = new FormBF2Unit();
                    subFormDiluentDispense[moduleIndex] = new FormDiluentDispenseUnit();
                    subFormPretrigger[moduleIndex] = new FormPretriggerUnit();
                    subFormTriggerDispense[moduleIndex] = new FormTriggerDispenseUnit();
                    subFormFluidPiping[moduleIndex] = new FormFluidPipingUnit();
                    subFormSensor[moduleIndex] = new FormSensorUnit();
                    subFormTemp[moduleIndex] = new FormTempUnit();
                    subFormOther[moduleIndex] = new FormOther();
                }
            }
        }

        /// <summary>
        /// サブ画面のインスタンスを破棄する
        /// </summary>
        public void DisposeInstanceSubForm(int moduleId)
        {
            if (moduleId == (Int32)RackModuleIndex.RackTransfer)
            {
                if (subFormSampleRackTransfer != null)
                {
                    //ラック搬送の各ユニットのインスタンスを削除
                    subFormSampleRackTransfer.Dispose();
                    subFormRackSensor.Dispose();

                    subFormSampleRackTransfer = null;
                    subFormRackSensor = null;
                }
            }
            else
            {
                //モジュール１～４のインスタンスを削除
                Int32 moduleIndex = Utility.CarisXSubFunction.ModuleIDToModuleIndex(moduleId);

                if (subFormCaseTransfer[moduleIndex] != null)
                {
                    subFormCaseTransfer[moduleIndex].Dispose();
                    subFormReagentStorage[moduleIndex].Dispose();
                    subFormSTAT[moduleIndex].Dispose();
                    subFormSampleDispense[moduleIndex].Dispose();
                    subFormReactionCellTransfer[moduleIndex].Dispose();
                    subFormReactionTable[moduleIndex].Dispose();
                    subFormBFTable[moduleIndex].Dispose();
                    subFormTravelerDisposal[moduleIndex].Dispose();
                    subFormR1Dispense[moduleIndex].Dispose();
                    subFormR2Dispense[moduleIndex].Dispose();
                    subFormBF1[moduleIndex].Dispose();
                    subFormBF2[moduleIndex].Dispose();
                    subFormDiluentDispense[moduleIndex].Dispose();
                    subFormPretrigger[moduleIndex].Dispose();
                    subFormTriggerDispense[moduleIndex].Dispose();
                    subFormFluidPiping[moduleIndex].Dispose();
                    subFormSensor[moduleIndex].Dispose();
                    subFormTemp[moduleIndex].Dispose();
                    subFormOther[moduleIndex].Dispose();

                    subFormCaseTransfer[moduleIndex] = null;
                    subFormReagentStorage[moduleIndex] = null;
                    subFormSTAT[moduleIndex] = null;
                    subFormSampleDispense[moduleIndex] = null;
                    subFormReactionCellTransfer[moduleIndex] = null;
                    subFormReactionTable[moduleIndex] = null;
                    subFormBFTable[moduleIndex] = null;
                    subFormTravelerDisposal[moduleIndex] = null;
                    subFormR1Dispense[moduleIndex] = null;
                    subFormR2Dispense[moduleIndex] = null;
                    subFormBF1[moduleIndex] = null;
                    subFormBF2[moduleIndex] = null;
                    subFormDiluentDispense[moduleIndex] = null;
                    subFormPretrigger[moduleIndex] = null;
                    subFormTriggerDispense[moduleIndex] = null;
                    subFormFluidPiping[moduleIndex] = null;
                    subFormSensor[moduleIndex] = null;
                    subFormTemp[moduleIndex] = null;
                    subFormOther[moduleIndex] = null;
                }
            }
        }

        /// <summary>
        /// サブ画面のインスタンスを全て破棄する
        /// </summary>
        public void DisposeInstanceSubFormAll()
        {
            DisposeInstanceSubForm((Int32)RackModuleIndex.RackTransfer);
            if (TabControl.Tabs[(Int32)ModuleTabIndex.Slave1].Visible) DisposeInstanceSubForm((Int32)RackModuleIndex.Module1);
            if (TabControl.Tabs[(Int32)ModuleTabIndex.Slave2].Visible) DisposeInstanceSubForm((Int32)RackModuleIndex.Module2);
            if (TabControl.Tabs[(Int32)ModuleTabIndex.Slave3].Visible) DisposeInstanceSubForm((Int32)RackModuleIndex.Module3);
            if (TabControl.Tabs[(Int32)ModuleTabIndex.Slave4].Visible) DisposeInstanceSubForm((Int32)RackModuleIndex.Module4);
        }

        /// <summary>
        /// モジュール接続時の動作
        /// </summary>
        /// <param name="Comm"></param>
        private void moduleConnect(object moduleIndex)
        {
            if (moduleIndex != null)
            {
                RackModuleIndex rackModuleIndex = (RackModuleIndex)moduleIndex;

                switch (rackModuleIndex)
                {
                    case RackModuleIndex.RackTransfer:
                        this.TabControl.Tabs[(Int32)ModuleTabIndex.RackTransfer].Enabled = true;
                        break;
                    case RackModuleIndex.Module1:
                        this.TabControl.Tabs[(Int32)ModuleTabIndex.Slave1].Enabled = true;
                        break;
                    case RackModuleIndex.Module2:
                        this.TabControl.Tabs[(Int32)ModuleTabIndex.Slave2].Enabled = true;
                        break;
                    case RackModuleIndex.Module3:
                        this.TabControl.Tabs[(Int32)ModuleTabIndex.Slave3].Enabled = true;
                        break;
                    case RackModuleIndex.Module4:
                        this.TabControl.Tabs[(Int32)ModuleTabIndex.Slave4].Enabled = true;
                        break;
                }
            }
        }

        private void btMaintenanceJournal_Click(object sender, EventArgs e)
        {
            // メンテナンス日誌開くかチェックします
            if (Singleton<Oelco.CarisX.Common.MaintenanceJournalInfoManager>.Instance.IsShow())
            {
                // メンテナンス日誌（サービスマン用）を表示します
                Oelco.CarisX.GUI.DlgMaintenanceList dlgMaintenanceList = new Oelco.CarisX.GUI.DlgMaintenanceList(MaintenanceJournalType.Serviceman);
                dlgMaintenanceList.ShowDialog();
                return;
            }

            // メンテナンス日誌選択画面開くかチェックします
            string servicemanParamPath = Singleton<CarisXMaintenanceServicemanParameter>.Instance.SavePath;
            if (Singleton<Oelco.CarisX.Common.MaintenanceJournalInfoManager>.Instance.IsShowSelect())
            {
                // ファイルが開いている場合、値の変更ができないため選択画面に遷移させません
                if (Singleton<DataHelper>.Instance.CheckFileOpen(servicemanParamPath))
                {
                    // メンテナンス日誌選択画面を表示
                    Oelco.CarisX.GUI.DlgOptionReconfirmMaintenanceJournal dlgMaintenanceSelect = new Oelco.CarisX.GUI.DlgOptionReconfirmMaintenanceJournal(MaintenanceJournalType.Serviceman);
                    if (DialogResult.OK == dlgMaintenanceSelect.ShowDialog())
                    {
                        Oelco.CarisX.GUI.DlgMaintenanceList dlgMaintenanceList = new Oelco.CarisX.GUI.DlgMaintenanceList(MaintenanceJournalType.Serviceman);
                        dlgMaintenanceList.ShowDialog();
                    }
                }
                else
                {
                    // メンテナンス日誌画面のロードに失敗しました。パラメータファイルを閉じてください。
                    Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_019));
                }
            }
        }
    }
}
