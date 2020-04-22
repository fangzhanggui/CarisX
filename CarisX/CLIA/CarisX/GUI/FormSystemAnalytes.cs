using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.Misc;
using Oelco.Common.GUI;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Const;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Status;
using System.IO;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Infragistics.Win.UltraWinGrid;
using Oelco.CarisX.Parameter.AnalyteGroup;
using Oelco.CarisX.Common;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// システム分析項目画面クラス
    /// </summary>
    public partial class FormSystemAnalytes : FormChildBase
    {
        #region [定数定義]

        /// <summary>
        /// 分析項目名(列名キー)
        /// </summary>
        private const string PROTOCOLNAME = "DPRProtocolName";

        /// <summary>
        /// ホスト用番号(列キー)
        /// </summary>
        private const string HOSTNUMBER = "HostProtocolNumber";

        /// <summary>
        /// AnalyteGroup登録数上限
        /// </summary>
        private const Int32 ANALYTEGROUP_MAXCOUNT = 10;

        /// <summary>
        /// 影の初期位置(Ｘ座標）
        /// </summary>
        private const Int32 DefaultLocationXpnlSystemAnalytesShadowInside = 913;

        /// <summary>
        /// 影の初期位置(Ｙ座標）
        /// </summary>
        private const Int32 DefaultLocationYpnlSystemAnalytesShadowInside = 33;

        /// <summary>
        /// 影の初期サイズ(幅）
        /// </summary>
        private const Int32 DefaultSizeWidthpnlSystemAnalytesShadowInside = 516;

        /// <summary>
        /// 影の初期サイズ(高さ）
        /// </summary>
        private const Int32 DefaultSizeHeightpnlSystemAnalytesShadowInside = 13;
        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 測定テーブル登録用分析項目ボタンリスト
        /// </summary>
        private List<CustomUStateButton> AnalyteTableButtons = new List<CustomUStateButton>();

        /// <summary>
        /// 測定情報編集用分析項目ボタンリスト
        /// </summary>
        private List<CustomUStateButton> ProtocolSettingButtons = new List<CustomUStateButton>();

        /// <summary>
        /// ボタン選択状態色
        /// </summary>
        private Color selectButtonColor = Color.FromArgb(255, 128, 0);

        /// <summary>
        /// AnalyteGroup登録内容退避領域
        /// </summary>
        AnalyteGroupInfoManager analyteGroupInfoManager = new AnalyteGroupInfoManager();

        /// <summary>
        /// 現在編集中のAnalyteGroupInfo
        /// </summary>
        AnalyteGroupInfo activeAnalyteGroupInfo = new AnalyteGroupInfo();

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 项目的页编码（一页为120项目）
        /// </summary>
        private int ProtocolPageIndex = 1;

        /// <summary>
        /// 项目的最大页数，初始值为1
        /// </summary>
        private int MaxNumberOfProtocolPages = 1;

        /// <summary>
        /// 一页显示的项目数，为120；
        /// </summary>
        private const int MaxNumberOfOnePage = 120;

        /// <summary>
        /// 项目名称和是否被选中Struct
        /// </summary>
        struct ProtocolAndSelect
        {
            public string Name;
            public Boolean bSelect;
        }

        /// <summary>
        /// 整个项目的选中情况的Map
        /// </summary>
        private Dictionary<int, ProtocolAndSelect> MapProtocolAndSelect;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormSystemAnalytes()
        {
            InitializeComponent();

            // 分析項目ボタンのリスト化
            #region __測定情報編集用__
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol1);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol2);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol3);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol4);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol5);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol6);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol7);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol8);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol9);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol10);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol11);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol12);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol13);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol14);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol15);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol16);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol17);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol18);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol19);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol20);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol21);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol22);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol23);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol24);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol25);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol26);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol27);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol28);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol29);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol30);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol31);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol32);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol33);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol34);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol35);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol36);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol37);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol38);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol39);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol40);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol41);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol42);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol43);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol44);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol45);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol46);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol47);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol48);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol49);
            this.ProtocolSettingButtons.Add(this.btnMeasurementProtocol50);
            #endregion

            #region __測定テーブル登録用__
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol1);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol2);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol3);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol4);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol5);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol6);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol7);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol8);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol9);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol10);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol11);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol12);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol13);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol14);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol15);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol16);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol17);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol18);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol19);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol20);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol21);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol22);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol23);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol24);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol25);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol26);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol27);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol28);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol29);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol30);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol31);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol32);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol33);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol34);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol35);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol36);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol37);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol38);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol39);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol40);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol41);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol42);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol43);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol44);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol45);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol46);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol47);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol48);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol49);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol50);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol51);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol52);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol53);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol54);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol55);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol56);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol57);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol58);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol59);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol60);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol61);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol62);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol63);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol64);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol65);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol66);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol67);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol68);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol69);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol70);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol71);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol72);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol73);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol74);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol75);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol76);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol77);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol78);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol79);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol80);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol81);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol82);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol83);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol84);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol85);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol86);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol87);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol88);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol89);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol90);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol91);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol92);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol93);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol94);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol95);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol96);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol97);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol98);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol99);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol100);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol101);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol102);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol103);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol104);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol105);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol106);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol107);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol108);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol109);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol110);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol111);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol112);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol113);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol114);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol115);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol116);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol117);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol118);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol119);
            this.AnalyteTableButtons.Add(this.btnAnalyteTableProtocol120);
            #endregion

            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SystemStatusChanged, this.onSystemStatusChanged);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UseOfHost, this.onHostParamChanged);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.UserLevelChanged, this.setUser);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.AnalyteRoutineTableChanged, this.onAnalyteRoutineTableChanged);
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.ChangeProtocolSetting, this.onProtocolSettingChanged);
        }

        #endregion

        #region [publicメソッド]

        ///// <summary>
        ///// 各種データの更新
        ///// </summary>
        ///// <remarks>
        ///// 各種データの更新します
        ///// </remarks>
        //public override void RefleshData()
        //{
        //    // TODO:ルーチンテーブル読込直して画面状態に反映処理
        //}

        /// <summary>
        /// 非表示時処理
        /// </summary>
        public override void Hide()
        {
            // 子画面も非表示にする
            if (this.formProtocolSetting != null)
            {
                this.formProtocolSetting.Hide();
            }
            base.Hide();
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
            // TODO:
        }

        /// <summary>
        /// コンポーネントの初期化
        /// </summary>
        /// <remarks>
        /// コンポーネントを初期化します
        /// </remarks>
        protected override void initializeFormComponent()
        {
            CaculateTheMaxPages();

            //得到所有项目的选中状态
            InitProtocolSetStatusInfo();
            // 測定情報編集用分析項目ボタン名称設定
            updateProtocolSettingButtonText();

            // 測定テーブル登録用分析項目ボタン名称設定
            updateAnalyteTableButtonText();

            // 測定順序画面リスト設定
            updateMeasurementPriorityList();

            // 分析項目番号(ホスト用)設定
            updateAnalyteNo();
            this.tbpAnalyteNo.Tab.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Enable;
            ControlLocationAdjust();

            // 画面表示内容の設定
            this.initAnalyteGroupTab();

            // 初めて画面を開く時にtxtGroupName_ValueChangedを実行してしまうため
            // txtGroupNameによって、編集中フラグがONになっている場合があるため
            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// 计算项目页的大小
        /// </summary>
        private void CaculateTheMaxPages()
        {
            int nProtocolCount = Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList.Count;
            MaxNumberOfProtocolPages = nProtocolCount / MaxNumberOfOnePage;
            if (nProtocolCount % MaxNumberOfOnePage != 0)
            {
                MaxNumberOfProtocolPages++;
            }
            if (MaxNumberOfProtocolPages == 1)
            {
                btnPre.Visible = false;
                btnPost.Visible = false;
            }
        }

        /// <summary>
        /// 得到初始化的每个项目的选中的初始信息
        /// </summary>
        private void InitProtocolSetStatusInfo()
        {

            try
            {

                MapProtocolAndSelect = new Dictionary<int, ProtocolAndSelect>();
                List<MeasureProtocol> measureProtocolList = Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList;
                for (int i = 0; i < measureProtocolList.Count; i++)
                {
                    ProtocolAndSelect item = new ProtocolAndSelect();
                    item.Name = measureProtocolList[i].ProtocolName;
                    item.bSelect = measureProtocolList[i].DisplayProtocol;
                    if (!MapProtocolAndSelect.ContainsKey(measureProtocolList[i].ProtocolIndex))
                    {
                        //対象のインデックスが存在しない場合のみ設定する
                        MapProtocolAndSelect.Add(measureProtocolList[i].ProtocolIndex, item);
                    }
                    //dd =i;
                }
            }
            catch (System.Exception ex)
            {
                //MessageBox.Show(dd.ToString());
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("An exception occurred in InitProtocolSetStatusInfo : {0}", ex.StackTrace));
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
            // タイトル
            this.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_013;

            // タブ
            this.tbpAnalyteParameters.Tab.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_000;
            this.tbpAnalyteTable.Tab.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_001;
            this.tbpMeasurementPriority.Tab.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_002;
            this.tbpAnalyteNo.Tab.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_014;
            this.tbpAnalyteGroup.Tab.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_018;
            this.lblAnalyteParametersSummary.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_003;
            this.btnImportAnalyteParamteters.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_004;
            this.lblRagistrationMax.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_005;
            this.btnRagistrationAnalyteTable.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_006;
            this.btnUpMeasurementPriority.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_007;
            this.lblMeasurementPrioritySummary.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_008;
            this.lblPriorityHigh.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_009;
            this.lblPriority.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_010;
            this.lblPriorityLow.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_011;
            this.btnDownMeasurementPriority.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_012;
            this.lblAnalyteNoMessage.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_015;
            this.btnRegistrationAnalyteNo.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_006;
            this.grdAnalyteNo.DisplayLayout.Bands[0].Columns[PROTOCOLNAME].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_016;
            this.grdAnalyteNo.DisplayLayout.Bands[0].Columns[HOSTNUMBER].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_017;
            this.gbxEditMode.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_019;
            this.optGroupEditMode.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_020;
            this.optGroupEditMode.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_021;
            this.lblGroupName.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_022;
            this.lblGroupAnalyte.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_023;
            this.btnRegistAnalyteGroup.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_024;
            this.btnDeleteAnlyteGroup.Text = Oelco.CarisX.Properties.Resources.STRING_SYSTEMANALYES_025;
        }

        /// <summary>
        /// ユーザレベル設定
        /// </summary>
        /// <remarks>
        /// ユーザレベル設定します
        /// </remarks>
        protected override void setUser(Object value)
        {
            base.setUser(value);
            tabSystemAnalytes.Tabs[1].Visible = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.MeasureProtocolAdd);
            btnImportAnalyteParamteters.Visible = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.AnalyserParameterSetting);
            //【IssuesNo:17】如果用户不具备调整项目测定顺序的权限，隐藏此界面
            tabSystemAnalytes.Tabs[2].Visible = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.MeasurePriority);
            ControlLocationAdjust();
        }

        #endregion

        #region [privateメソッド]
        /// <summary>
        /// FormClosingイベント処理
        /// </summary>
        /// <remarks>
        /// 分析項目設定画面を解放します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormSystemAnalytes_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.formProtocolSetting != null)
            {
                this.formProtocolSetting.Dispose();
            }
        }

        /// <summary>
        /// 分析項目編集ボタン
        /// </summary>
        /// <remarks>
        /// 測定情報の詳細表示します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnMeasurementProtocol_Click(object sender, EventArgs e)
        {
            // プロトコル名からプロトコル番号の取得
            MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromName(((UltraButton)sender).Text);
            if (protocol != null)
            {
                // 取得結果ありの場合
                this.showDetail(protocol.ProtocolNo);
            }
        }

        /// <summary>
        /// 測定テーブル登録用分析項目ボタンのクリックイベント
        /// </summary>
        /// <remarks>
        /// 測定テーブル登録用分析項目をクリック時、Form共通の編集中フラグを変更します。
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnAnalyteTableProtocol_Click(object sender, EventArgs e)
        {
            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        /// <summary>
        /// 優先順位を１つ上げる
        /// </summary>
        /// <remarks>
        /// 優先順位を１つ上げる変更します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnUpMeasurementPriority_Click(object sender, EventArgs e)
        {
            this.MeasurementPriorityChange(true);
        }

        /// <summary>
        /// 優先順位を１つ下げる
        /// </summary>
        /// <remarks>
        /// 優先順位を１つ下げる変更します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnDownMeasurementPriority_Click(object sender, EventArgs e)
        {
            this.MeasurementPriorityChange(false);
        }

        /// <summary>
        /// 分析項目設定画面
        /// </summary>
        FormProtocolSetting formProtocolSetting = null;
        /// <summary>
        /// 測定情報の詳細表示
        /// </summary>
        /// <remarks>
        /// 分析項目設定画面を表示します
        /// </remarks>
        /// <param name="protocolNo">設定を行うプロトコルのNo.</param>
        private void showDetail(Int32 protocolNo)
        {

            if (this.formProtocolSetting != null)
            {
                this.formProtocolSetting.FormClose();
                this.formProtocolSetting.Dispose();
            }

            // 分析項目番号を設定
            this.formProtocolSetting = new FormProtocolSetting(protocolNo);
            formProtocolSetting.Show(new Rectangle(this.Location, this.Size));

        }

        /// <summary>
        /// 分析項目を測定テーブルに設定
        /// </summary>
        /// <remarks>
        /// 分析項目を測定テーブルに設定します
        /// </remarks>
        private void setAnalyteTable()
        {
            // 分析項目測定テーブル変更前通知
            Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.AnalyteRoutineTableChanging);

            for (int i = 0; i < MapProtocolAndSelect.Count; i++)
            {
                Singleton<MeasureProtocolManager>.Instance.SetDisplayProtocol(MapProtocolAndSelect[i + 1].Name, MapProtocolAndSelect[i + 1].bSelect);
            }

            // ファイルに保存
            Singleton<MeasureProtocolManager>.Instance.SaveAllMeasureProtocol();

            // 順序リストを同期させて、こちらもファイル保存
            Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.SyncMeasProtocolManager(Singleton<MeasureProtocolManager>.Instance);
            Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.InitTurnOrder();
            Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.SaveEncryption();

            // 測定情報編集用分析項目ボタン名称表示更新
            this.updateProtocolSettingButtonText();

            // 測定順序画面リスト表示更新
            this.updateMeasurementPriorityList();

            // 分析項目再読込み
            Singleton<MeasureProtocolManager>.Instance.LoadAllMeasureProtocol();

            // 分析項目測定テーブル変更後通知
            Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.AnalyteRoutineTableChanged);
        }

        /// <summary>
        /// 分析項目の優先順番を設定
        /// </summary>
        /// <remarks>
        /// 分析項目の優先順番を設定し、ファイルに保存します
        /// </remarks>
        private void setAnalyteSequence()
        {
            List<String> nameList = new List<String>();
            foreach (String name in this.lbxMeasurementPriority.Items)
            {
                nameList.Add(name);
            }
            Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.SetTurnOrder(nameList);
            // 順序をファイルに保存
            Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.SaveEncryption();
        }

        /// <summary>
        /// 測定情報編集用分析項目ボタン名称設定
        /// </summary>
        /// <remarks>
        /// 測定情報編集用分析項目ボタン名称設定します
        /// </remarks>
        private void updateProtocolSettingButtonText()
        {
            List<MeasureProtocol> measureProtocols = Singleton<MeasureProtocolManager>.Instance.UseMeasureProtocolList;
            Int32 i = 0;
            for (; (measureProtocols.Count > i) && (this.ProtocolSettingButtons.Count > i); i++)
            {
                this.ProtocolSettingButtons[i].Text = measureProtocols[i].ProtocolName;
                this.ProtocolSettingButtons[i].Visible = true;
            }
            for (; this.ProtocolSettingButtons.Count > i; i++)
            {
                this.ProtocolSettingButtons[i].Visible = false;
            }
        }

        /// <summary>
        /// 測定テーブル登録用分析項目ボタン名称設定
        /// </summary>
        /// <remarks>
        /// 測定テーブル登録用分析項目ボタン名称設定します
        /// </remarks>
        private void updateAnalyteTableButtonText()
        {
            try
            {
                List<MeasureProtocol> measureProtocols = Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList;
                Int32 i = 0;
                Int32 j = MaxNumberOfOnePage * (ProtocolPageIndex - 1) + i;

                for (; i < MaxNumberOfOnePage; i++)
                {
                    if ((MapProtocolAndSelect.ContainsKey(j + 1))
                        && ( MapProtocolAndSelect[j + 1].Name != String.Empty))
                    {
                        this.AnalyteTableButtons[i].Text = MapProtocolAndSelect[j + 1].Name;
                        this.AnalyteTableButtons[i].Visible = true;
                        this.AnalyteTableButtons[i].CurrentState = MapProtocolAndSelect[j + 1].bSelect;
                    }
                    else
                    {
                        this.AnalyteTableButtons[i].Text = string.Empty;
                        this.AnalyteTableButtons[i].Visible = false;
                        this.AnalyteTableButtons[i].CurrentState = false;
                    }

                    j++;
                }
                for (; this.AnalyteTableButtons.Count > i; i++)
                {
                    this.AnalyteTableButtons[i].Visible = false;
                }
            }
            catch (System.Exception ex)
            {
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("An exception occurred in updateAnalyteTableButtonText : {0}", ex.StackTrace));
            }
        }

        /// <summary>
        /// 測定順序画面リスト設定
        /// </summary>
        /// <remarks>
        /// 測定順序画面リスト設定します
        /// </remarks>
        private void updateMeasurementPriorityList()
        {
            this.lbxMeasurementPriority.Items.Clear();
            foreach (String protocolName in Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.MeasureProtocolTurnList)
            {
                this.lbxMeasurementPriority.Items.Add(protocolName);
            }
        }

        /// <summary>
        /// 分析項目番号(ホスト用)設定
        /// </summary>
        /// <remarks>
        /// 分析項目番号(ホスト用)設定します
        /// </remarks>
        private void updateAnalyteNo()
        {
            this.grdAnalyteNo.DataSource = new BindingList<DPRProtocolNameAndHostProtocolNumber>(Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.DPRNameAndHostNumber);
        }

        /// <summary>
        /// 優先順位変更
        /// </summary>
        /// <remarks>
        /// 優先順位変更します
        /// </remarks>
        /// <param name="up">true:上位へ移動/false:下位へ移動</param>
        private void MeasurementPriorityChange(Boolean up)
        {
            // 選択時のみ
            if (this.lbxMeasurementPriority.SelectedItems.Count > 0)
            {
                // 上下限未達時のみ
                if ((up && this.lbxMeasurementPriority.SelectedIndex > 0) // 上限
                    || (!up && this.lbxMeasurementPriority.SelectedIndex < this.lbxMeasurementPriority.Items.Count - 1))  // 下限
                {
                    // 選択項目と移動先項目と入れ替え
                    Object obj = this.lbxMeasurementPriority.SelectedItem;
                    Int32 nextIndex = this.lbxMeasurementPriority.SelectedIndex;

                    if (up)
                    {
                        nextIndex--;
                    }
                    else
                    {
                        nextIndex++;
                    }

                    this.lbxMeasurementPriority.Items[this.lbxMeasurementPriority.SelectedIndex] = this.lbxMeasurementPriority.Items[nextIndex];
                    this.lbxMeasurementPriority.Items[nextIndex] = obj;

                    // 選択状態の維持
                    this.lbxMeasurementPriority.SelectedIndex = nextIndex;

                    this.setAnalyteSequence();
                }
            }
        }

        /// <summary>
        /// 分析項目テーブルへの登録
        /// </summary>
        /// <remarks>
        /// 分析項目テーブルへの登録します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnRagistrationAnalyteTable_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateTheCurrentPageSelectPrococol();
                Int32 count = 0;
                for (int i = 0; i < MapProtocolAndSelect.Count; i++)
                {
                    if (MapProtocolAndSelect[i + 1].bSelect == true)
                    {
                        count++;
                    }
                }

                if (count > CarisXConst.MEAS_PROTO_REGIST_MAX)
                {
                    // 51以上なので登録不可
                    DlgMessage.Show(Oelco.CarisX.Properties.Resources.STRING_DLG_MSG_196, "", "", MessageDialogButtons.Confirm);
                }
                else
                {
                    DialogResult dlgRet = DlgMessage.Show(Oelco.CarisX.Properties.Resources.STRING_DLG_MSG_197, "", "", MessageDialogButtons.OKCancel);
                    if (dlgRet == DialogResult.OK)
                    {
                        // 非選択分析項目で、AnalyteGroupに使用されている分析項目がないかチェックし、あれば確認の上削除する
                        // 確認メッセージでキャンセルが押されたら、以降の処理中止
                        if (!this.deleteAnalyteGroupByNotUse())
                        {
                            return;
                        }
                        this.setAnalyteTable();

                        // Form共通の編集中フラグOFF
                        FormChildBase.IsEdit = false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }

        }

        /// <summary>
        /// AnalyteTable更新情報をAnalyteGroupの分析項目パネルに反映します。
        /// </summary>
        private void onAnalyteRoutineTableChanged(Object value)
        {
            // 測定テーブル表示更新
            this.analysisSettingPanel.ReLoadAnalyteInformation();
            this.viewActiveAnalyteGroup();
            updateAnalyteNo();
        }

        /// <summary>
        /// FormProtocolSettingにて更新処理が行われた際、AnalyteGroupタブの再描画を行います。
        /// </summary>
        private void onProtocolSettingChanged(Object value)
        {
            this.analysisSettingPanel.ReLoadAnalyteInformation();
            this.activeAnalyteGroupInfo = new AnalyteGroupInfo();
            this.initAnalyteGroupTab();
            this.activeAnalyteGroupInfo = this.GetAnalyteGroupInfoByComboBox();
            this.viewActiveAnalyteGroup();

            // txtGroupNameによって、編集中フラグがONになっている場合があるため
            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// 非選択分析項目を使用しているAnalyteGroupの取得
        /// </summary>
        /// <remarks>
        /// <para>
        /// 非選択になった分析項目を使用しているAnalyteGroupがないか検索し、
        /// 存在する場合は、該当AnalyteGroupを削除してよいか確認ダイアログを表示。
        /// </para>
        /// <para>
        ///  確認ダイアログでOKボタンが押下された場合は、該当AnalyteGroupを削除します。
        /// </para>
        /// </remarks>
        /// <returns>
        /// <para>
        /// ture:削除項目がない、もしくは、確認ダイアログでOKボタンが押下され、AnalyteGroupの削除処理を実施した
        /// </para>
        /// <para>
        /// fase:確認ダイアログでキャンセルボタンが押下された
        /// </para>
        /// </returns>
        private bool deleteAnalyteGroupByNotUse()
        {
            // AnalyteGroup最新情報を取得する。
            Singleton<ParameterFilePreserve<AnalyteGroupInfoManager>>.Instance.Load();
            this.analyteGroupInfoManager = Singleton<ParameterFilePreserve<AnalyteGroupInfoManager>>.Instance.Param;

            // 削除対象AnalyteGroupを、確認の上削除する
            return this.deleteGroupByAnalyteTable(getDelAnalyteGroups(), Oelco.CarisX.Properties.Resources.STRING_DLG_MSG_227);
        }

        /// <summary>
        /// 非選択分析項目使用AnalyteGroupの削除
        /// </summary>
        /// <param name="delProtUseGroups">削除処理対象AnalyteGroupInfoリスト</param>
        /// <remarks>
        /// 引数値<paramref name="delProtUseGroups"/>の要素が1以上の場合、削除実施有無確認ダイアログを表示し、
        /// OKボタンが押下された場合は<paramref name="delProtUseGroups"/>をAnalyteGroup.xmlより削除します。
        /// </remarks>
        /// <returns>
        /// <para>
        /// ture:削除項目がない、もしくは、確認ダイアログでOKボタンが押下され、AnalyteGroupの削除処理を実施した
        /// </para>
        /// <para>
        /// fase:確認ダイアログでキャンセルボタンが押下された
        /// </para>
        /// </returns>
        private bool deleteGroupByAnalyteTable(List<AnalyteGroupInfo> delProtUseGroups, string viewMessage)
        {
            if (delProtUseGroups.Count > 0)
            {
                // 非選択対象プロトコルを使用しているGroupがあれば、確認ダイアログを表示し、対象グループを削除する
                if (DlgMessage.Show(viewMessage, "", "", MessageDialogButtons.OKCancel) == DialogResult.OK)
                {
                    foreach (AnalyteGroupInfo delGroup in delProtUseGroups)
                    {
                        // activeAnalyteGroupInfoが削除対象のAnalyteGroupInfo情報を保持している場合、クリアする
                        if (delGroup.GroupName == this.activeAnalyteGroupInfo.GroupName)
                        {
                            this.activeAnalyteGroupInfo = new AnalyteGroupInfo();
                        }

                        List<AnalyteGroupInfo> delInfo = this.analyteGroupInfoManager.AnalyteGroupInfos
                                                .Where(x => x.GroupName == delGroup.GroupName).ToList();


                        this.analyteGroupInfoManager.AnalyteGroupInfos.Remove(delInfo.First());
                    }

                    // AnalyteGroup削除処理をXMLに反映
                    AnalyteGroupManager.SaveAnalyteGroup(this.analyteGroupInfoManager.AnalyteGroupInfos);

                    // AnalyteGroupタブに、最新情報を反映
                    this.initAnalyteGroupTab();
                }
                else
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// 削除対象AnalyteGroupの取得
        /// </summary>
        /// <remarks>
        /// AnalyteTableで非選択となった分析項目を使用している( = 削除対象)AnalyteGroupを取得します。
        /// </remarks>
        /// <returns>
        /// 削除対象と判定されたAnalyteGroupリスト
        /// </returns>
        private List<AnalyteGroupInfo> getDelAnalyteGroups()
        {
            // 非選択の分析項目の名称をリスト化する
            List<String> delProtcolNames = new List<string>();
            foreach (CustomUStateButton button in this.AnalyteTableButtons)
            {
                // 空白のボタンがあれば、以降は全部不使用ボタンなので処理を抜ける
                if (String.IsNullOrEmpty(button.Text))
                {
                    break;
                }
                // 非選択ボタンはリストに追加
                if (button.CurrentState == false)
                {
                    delProtcolNames.Add(button.Text);
                }
            }

            // OFFになった分析項目が使われているAnalyteGroupがないかチェックする				
            List<AnalyteGroupInfo> delProtUseGroups = new List<AnalyteGroupInfo>();
            foreach (AnalyteGroupInfo analyteGroupInfo in this.analyteGroupInfoManager.AnalyteGroupInfos)
            {
                foreach (string delProtName in delProtcolNames)
                {
                    // ボタンのTextからプロトコルインデックスを取得する
                    Int32 protcolIndex = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromName(delProtName).ProtocolIndex;
                    if (analyteGroupInfo.AnalyteInfos.Where(x => x.ProtocolIndex == protcolIndex).Count() > 0)
                    {
                        // 非選択対象プロトコルを使用していれば、リストに保持
                        delProtUseGroups.Add(analyteGroupInfo);
                        break;
                    }
                }
            }

            return delProtUseGroups;
        }

        /// <summary>
        /// 分析項目パラメータをxlsシートから読み取る
        /// </summary>
        /// <remarks>
        /// 分析項目パラメータをxlsシートから読み取りします
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnImportAnalyteParamteters_Click(object sender, EventArgs e)
        {
            // 分析項目パラメータ読み取りダイアログ表示
            DlgImportMeasProto dlgImportMeasProto = new DlgImportMeasProto();
            DialogResult dlgRet = dlgImportMeasProto.ShowDialog();
            if (dlgRet == DialogResult.OK)
            {
                // 分析項目のバージョン番号取得
                String protocolVersion = dlgImportMeasProto.getProtocolVersion();
                if (protocolVersion == String.Empty)
                {
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_243 + "\n" + CarisX.Properties.Resources.STRING_DLG_MSG_244, "",
                           CarisX.Properties.Resources.STRING_DLG_TITLE_005, MessageDialogButtons.OK);
                    return;
                }
                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.ProtocolVersion, protocolVersion);
                ProtocolVersionParameter protocolVersionParameter = new ProtocolVersionParameter();
                protocolVersionParameter.ProtocolVersion = protocolVersion;
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.ProtocolVersionParameter = protocolVersionParameter;
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Save();

                // 処理結果ログを読み込む
                ProtocolConverterLogInfo convLog = this.GetConvertLogData();
                if (convLog != null)
                {
                    if (convLog.ExportCount > 0 && convLog.ErrorList.Count == 0)
                    {
                        //　チェック処理が正常の場合のみ以降の処理を続ける
                        if (!CheckImportProtocols(convLog.ExportProtocolIndex))
                        {
                            // 操作履歴　"分析項目パラメータの読込みをキャンセルしました" 
                            Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist
                                , Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                                , CarisXLogInfoBaseExtention.Empty
                                , new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_073 });
                            return;
                        }

                        // インポート処理実行
                        Singleton<MeasureProtocolManager>.Instance.ImportMeasProtoParameter();

                        // メッセージを表示:"ソフトウェアを終了します。パラメータを有効にするためにソフトウェアの再起動が必要です。"
                        DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_205 + "\n" + CarisX.Properties.Resources.STRING_DLG_MSG_206
                                        , ""
                                        , CarisX.Properties.Resources.STRING_DLG_TITLE_001
                                        , MessageDialogButtons.OK);

                        // ソフト終了イベント通知
                        Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.SystemEnd);
                    }
                    else
                    {
                        if (convLog.ErrorList.Count > 0)
                        {
                            // インポートエラーダイアログを表示する
                            DlgConvertErrLog dlg = new DlgConvertErrLog();
                            dlg.setErrorList(convLog.ErrorList);
                            dlg.ShowDialog();
                        }
                    }
                }
            }
            else
            {
                // 操作履歴　"分析項目パラメータの読込みをキャンセルしました" 
                Singleton<CarisXLogManager>.Instance.Write( LogKind.OperationHist
                                                          , Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                                                          , CarisXLogInfoBaseExtention.Empty
                                                          , new String[] { this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_073 });
            }
        }

        /// <summary>
        /// インポート内容チェック処理
        /// </summary>
        /// <param name="protoIdx">チェック対象分析項目リスト</param>
        /// <returns>true:チェックOK、false:チェックNG</returns>
        private bool CheckImportProtocols(List<Int32> protoIdx)
        {
            List<MeasureProtocol> importProtocol;
            if (!Singleton<MeasureProtocolManager>.Instance.LoadExportProtocol(protoIdx, out importProtocol))
            {
                return false;
            }

            // 1.不使用項目をしているAnalyteGroupが存在すれば、確認メッセージ表示の上削除
            // 2.Importする分析項目を登録しているAnalyteGroupで、サンプル種別の矛盾がある場合、確認メッセージ表示のうえ削除
            //if (this.deleteGroupByAnalyteTable(this.getDeleteGroupsByImport(importProtocol), Oelco.CarisX.Properties.Resources.STRING_DLG_MSG_227) &&
            //    this.deleteGroupByAnalyteTable(this.getSumpleKindCheckErrorGroupNames(importProtocol), Oelco.CarisX.Properties.Resources.STRING_DLG_MSG_228))

            // 上記1.の処理は現時点では不要の為、呼び出し処理をコメントアウトして残しておく。
            // プロトコルコンバータ出力結果のxmlは、全てEnable=falseで出力される。
            // 本プログラムで反映する際に、Enableは反映前の値を引き継ぐようにしている為、（MeasureProtocolManager.importProtocolToProtocol参照）
            // 出力結果のEnable値のチェックはしてはいけない

            List<Tuple<Int32, Oelco.CarisX.Parameter.MeasureProtocol.SampleTypeKind>> checkProtocol
                    = (from m in importProtocol select new Tuple<Int32, Oelco.CarisX.Parameter.MeasureProtocol.SampleTypeKind>(m.ProtocolIndex, m.SampleKind)).ToList();
            if (this.deleteGroupByAnalyteTable(AnalyteGroupManager.GetSumpleKindCheckErrorGroupNames(checkProtocol), Oelco.CarisX.Properties.Resources.STRING_DLG_MSG_228))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 分析項目番号(ホスト用)編集完了後イベント
        /// </summary>
        /// <remarks>
        /// 編集データをチェックして分析項目番号が重複していればメッセージを表示します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void grdAnalyteNo_AfterExitEditMode(object sender, EventArgs e)
        {
            var grd = ((UltraGrid)sender);
            if (grd.ActiveCell.DataChanged)
            {
                var count = grd.Rows.Select((row) => (Int32)row.Cells[grd.ActiveCell.Column].Value).Count((data) => data == (Int32)grd.ActiveCell.Value);
                if (count > 1)
                {
                    grd.ActiveCell.Value = grd.ActiveCell.OriginalValue;
                    // メッセージ表示(重複)
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_122, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                }
            }
        }

        /// <summary>
        /// 分析項目番号(ホスト用)を設定
        /// </summary>
        /// <remarks>
        /// 分析項目番号(ホスト用)更新してパラメータ保存します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnRegistrationAnalyteNo_Click(object sender, EventArgs e)
        {
            this.grdAnalyteNo.UpdateData();
            Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.SaveEncryption();

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// システムステータス変化イベント
        /// </summary>
        /// <remarks>
        /// システムステータス変化によりボタン有効/無効状態を切替します
        /// </remarks>
        /// <param name="value"></param>
        private void onSystemStatusChanged(Object value)
        {
            bool enabled = true;
            if (Singleton<SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId(Singleton<PublicMemory>.Instance.moduleIndex)] == SystemStatusKind.ReagentExchange)
            {
                enabled = false;
            }
            else
            {
                switch (Singleton<SystemStatus>.Instance.Status)
                {
                    case SystemStatusKind.WaitSlaveResponce:
                    case SystemStatusKind.Assay:
                    case SystemStatusKind.SamplingPause:
                    case SystemStatusKind.ToEndAssay:
                        enabled = false;
                        break;
                }
            }

            this.btnImportAnalyteParamteters.Enabled = enabled;
            this.btnUpMeasurementPriority.Enabled = enabled;
            this.btnDownMeasurementPriority.Enabled = enabled;
            this.btnRagistrationAnalyteTable.Enabled = enabled;
            this.btnRegistrationAnalyteNo.Enabled = enabled;
        }

        /// <summary>
        /// ホストパラメータ変更通知
        /// </summary>
        /// <remarks>
        /// 印刷ボタン表示設定します
        /// </remarks>
        /// <param name="value"></param>
        private void onHostParamChanged(Object value)
        {
            // 印刷ボタン表示設定
            this.tbpAnalyteNo.Tab.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.Enable;
            ControlLocationAdjust();

        }

        /// <summary>
        /// コンバート処理結果ログ内容取得
        /// </summary>
        /// <remarks>
        /// コンバート処理結果ログ内容取得します
        /// </remarks>
        /// <returns></returns>
        private ProtocolConverterLogInfo GetConvertLogData()
        {
            ProtocolConverterLogInfo rtn = null;

            Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.SavePath = CarisXConst.ProtoConvExportDir + @"\" + CarisXConst.PROT_CONV_LOG;
            if (Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.LoadEncryption())
            {
                rtn = Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param;
            }

            if (rtn != null && rtn.ErrorList.Count > 0)
            {
                List<String> errorList = new List<String>();
                foreach (var err in rtn.ErrorList)
                {
                    String strText = err;
                    String[] items = strText.Replace("\t", ",").Split(',');
                    if (items.Length <= 2)
                    {
                        errorList.Add(strText);
                    }
                    else
                    {
                        List<String> list = new List<String>();
                        list.AddRange(items);
                        // エラー番号は削除
                        list.RemoveAt(2);
                        errorList.Add(String.Join("\t\t", list.ToArray()));
                    }

                }
                rtn.ErrorList = errorList;
            }

            return rtn;
        }


        /// <summary>
        /// AnalyteGroupタブ初期設定
        /// </summary>
        private void initAnalyteGroupTab()
        {
            // xmlの情報を読み込む
            Singleton<ParameterFilePreserve<AnalyteGroupInfoManager>>.Instance.Load();
            this.analyteGroupInfoManager = Singleton<ParameterFilePreserve<AnalyteGroupInfoManager>>.Instance.Param;

            // GroupNameコンボボックスのデータバインド
            this.BindAnalyteGroupNameCombo();

            // ↓ValueMemberにAnalyteInfosを設定しても、
            // ↓cmbGroupName.SelectedItem.ListObjectからでないとAnalyteInfos情報を取得できないのでコメントにする
            //this.cmbGroupName.ValueMember = "AnalyteInfos";

            // EditModeオプションの設定
            this.setAnalyteGroupEditMode(true);

            this.viewActiveAnalyteGroup();

        }

        /// <summary>
        /// EditModeオプションボタンの設定
        /// </summary>
        private void setAnalyteGroupEditMode(bool isInit)
        {
            // Analyte GroupのEditModeオプション選択値設定
            if (this.analyteGroupInfoManager.AnalyteGroupInfos.Count > 0)
            {
                if (isInit)
                {
                    // 登録済みのデータが存在すれば、Editを選択する				
                    this.optGroupEditMode.CheckedIndex = 0;
                    // GroupNameコンボボックスのリスト先頭を選択する
                    this.cmbGroupName.SelectedIndex = 0;
                }

                if (this.analyteGroupInfoManager.AnalyteGroupInfos.Count >= ANALYTEGROUP_MAXCOUNT)
                {
                    // 登録済みGroupが10件の場合、「CreateNew」を選択出来ないようにする
                    // ラジオボタンのItemだけ非活性に出来ないので、ラジオボタン全体を非活性にし、
                    // 非活性にしたい項目の文字色をグレーにする事で対応
                    this.optGroupEditMode.CheckedIndex = 0;
                    this.optGroupEditMode.Enabled = false;
                    this.optGroupEditMode.Items[0].Appearance.ForeColorDisabled = Color.Black;
                    this.optGroupEditMode.Items[1].Appearance.ForeColorDisabled = Color.Gray;
                }
                else
                {
                    // 登録済みGroupが10件以下の場合は、Edit,CreateNew共に有効
                    this.optGroupEditMode.Enabled = true;
                }
            }
            else
            {
                // 登録済みのデータが存在しなければ、CreateNewを選択する
                this.optGroupEditMode.CheckedIndex = 1;

                // 登録済みのデータが存在しなければ「Edit」を選択出来ないようにする。				
                // ラジオボタンのItemだけ非活性に出来ないので、ラジオボタン全体を非活性にし、
                // 非活性にしたい項目の文字色をグレーにする事で対応
                this.optGroupEditMode.Enabled = false;
                this.optGroupEditMode.Items[0].Appearance.ForeColorDisabled = Color.Gray;
                this.optGroupEditMode.Items[1].Appearance.ForeColorDisabled = Color.Black;
            }
        }

        /// <summary>
        /// コンボボックス選択変更イベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbGroupName_SelectionChanged(object sender, EventArgs e)
        {
            // コンボボックスのDataSourceから、選択したGroupのAnalyteGroupInfoのコピーを取得する
            // ※インスタンスをそのまま取得しないのは、登録ボタン押下してない時点でanalyteGroupInfoManagerに反映されたら困るから。
            // 登録ボタンを押す前に別リスト選択等して、画面が切り替わったら、元に戻っててほしい。
            activeAnalyteGroupInfo = CopyAnalyteGroupInfoByComboBox();

            // GroupNameテキストボックスの設定
            txtGroupName.Text = activeAnalyteGroupInfo.GroupName;
            // activeAnalyteGroupInfoの内容を画面に表示する
            this.viewActiveAnalyteGroup();
        }

        /// <summary>
        /// GroupNamekコンボボックスで選択したリストのAnalyteGroupInfosのインスタンスを返す
        /// </summary>
        /// <returns>コピーしたAnalyteGroupInfo</returns>
        private AnalyteGroupInfo GetAnalyteGroupInfoByComboBox()
        {
            if (cmbGroupName.DataSource == null || cmbGroupName.SelectedItem == null)
            {
                return new AnalyteGroupInfo();
            }

            // 選択したリストのAnalyteGroupInfoのインスタンスを取得する
            AnalyteGroupInfo returnObj = (cmbGroupName.SelectedItem.ListObject as AnalyteGroupInfo);

            return returnObj;
        }

        /// <summary>
        /// GroupNamekコンボボックスで選択したリストのAnalyteGroupInfosのコピーを返す
        /// </summary>
        /// <returns>コピーしたAnalyteGroupInfo</returns>
        private AnalyteGroupInfo CopyAnalyteGroupInfoByComboBox()
        {
            if (cmbGroupName.DataSource == null || cmbGroupName.SelectedItem == null)
            {
                return new AnalyteGroupInfo();
            }

            // 選択したリストのAnalyteGroupInfoのコピーを取得する
            AnalyteGroupInfo returnObj = (cmbGroupName.SelectedItem.ListObject as AnalyteGroupInfo).Copy();

            return returnObj;
        }

        /// <summary>
        /// 編集モード設定変更イベント
        /// </summary>
        /// <remarks>
        /// 編集モード設定変更します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void optEditMode_ValueChanged(object sender, EventArgs e)
        {
            if (this.optGroupEditMode.Value != null)
            {
                this.setEditMode((Boolean)this.optGroupEditMode.Value);
            }
        }

        /// <summary>
        /// 編集モード設定
        /// </summary> 
        /// <remarks>
        /// 編集モード設定します
        /// </remarks>
        /// <param name="isCreateNew">true=CreateNew,false=Edit</param>
        private void setEditMode(Boolean isCreateNew)
        {
            if (isCreateNew)
            {
                cmbGroupName.Enabled = false;
                txtGroupName.Enabled = true;
                //// 編集中AnalyteGroupInfoをnewする
                //activeAnalyteGroupInfo = new AnalyteGroupInfo();
                btnDeleteAnlyteGroup.Enabled = false;
            }
            else
            {
                cmbGroupName.Enabled = true;
                txtGroupName.Enabled = false;
                //txtGroupName.Text = String.Empty;
                // activeAnalyteGroupInfoをコンボボックスで選択したリストの内容(AnalyteGroupInfoのコピー)にする。
                // ※インスタンスをそのまま取得しないのは、登録ボタン押下してない時点でanalyteGroupInfoManagerに反映されたら困るから。
                // 登録ボタンを押す前に別リスト選択等して、画面が切り替わったら、元に戻っててほしい。
                activeAnalyteGroupInfo = CopyAnalyteGroupInfoByComboBox();

                // 登録済みデータが1件以上の場合のみ、Deleteボタンを有効にする
                if (this.analyteGroupInfoManager.AnalyteGroupInfos.Count > 0)
                {
                    btnDeleteAnlyteGroup.Enabled = true;
                }
            }

            // 編集対象AnalyteInfoの内容を画面に表示する。
            this.viewActiveAnalyteGroup(true);
        }

        /// <summary>
        /// 分析項目選択状態変更時イベント
        /// </summary>
        /// <remarks>
        /// 選択した分析項目で編集状態を更新します
        /// </remarks>
        /// <param name="protocolIndex">分析項目インデックス</param>
        /// <param name="selected">選択状態</param>
        private void analysisSettingPanel_ProtocolCheckChanged(Int32 protocolIndex, Boolean selected)
        {
            // GroupAnalyteテキストボックス選択状態を反映
            MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(protocolIndex);

            Int32 dil = this.analysisSettingPanel.Dilution;
            Int32 measTimes = this.analysisSettingPanel.MeasTimes;

            if (selected)
            {
                // activeAnalyteGroupListに択分析項目の要素を追加
                if (this.getAnalyteInfoInctance(protocolIndex) == null)
                {
                    activeAnalyteGroupInfo.AnalyteInfos.Add(new AnalyteInfo(protocol.ProtocolIndex, dil, measTimes));
                }
                else
                {
                    activeAnalyteGroupInfo.AnalyteInfos.Remove(this.getAnalyteInfoInctance(protocolIndex));
                    activeAnalyteGroupInfo.AnalyteInfos.Add(new AnalyteInfo(protocol.ProtocolIndex, dil, measTimes));
                }
            }
            else
            {
                // activeAnalyteGroupListから選択分析項目の要素を削除
                activeAnalyteGroupInfo.AnalyteInfos.Remove(this.getAnalyteInfoInctance(protocolIndex));
            }

            /// GroupAnalyteテキストボックスの表示更新
            this.viewActiveAnalyteGroup(true);

            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;

            return;
        }

        /// <summary>
        /// activeAnalyteGroupInfoから、プロトコルインデックスが合致した要素のインスタンスを返します
        /// </summary>
        /// <param name="protocolIndex">プロトコルインデックス</param>
        /// <returns>見つかったら、そのインスタンス、見つからなかったらnull</returns>
        private AnalyteInfo getAnalyteInfoInctance(int protocolIndex)
        {
            AnalyteInfo returnInfo = null;


            foreach (var analyteInfo in activeAnalyteGroupInfo.AnalyteInfos)
            {
                if (analyteInfo.ProtocolIndex == protocolIndex)
                {
                    returnInfo = analyteInfo;
                    break;
                }
            }

            return returnInfo;
        }

        /// <summary>
        /// 現在選択中のAnalyteGroupの分析項目を画面に表示します。
        /// </summary>
        private void viewActiveAnalyteGroup(bool isKeepGroupName = false)
        {
            // 表示する分析項目名を編集して返すFunc
            Func<AnalyteInfo, String> funcGetProtocolName = (x) =>
            {
                String returnText = "";

                String protocolName = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(x.ProtocolIndex).ProtocolName;

                // 希釈率が選択されていたら括弧つきで表示する
                if (x.AutoDilution != 1)
                {
                    returnText = String.Format("{0}({1}{2})[{3}]",
                                                protocolName, Oelco.CarisX.Properties.Resources.STRING_ANALYSISSETTINGPANEL_055, x.AutoDilution, x.MeasTimes);
                }
                else
                {
                    returnText = protocolName;
                }

                return returnText;
            };

            // activeAnalyteGroupListに保存されている分析項目の分析項目名を、カンマ区切りで表示する
            // （プロトコルインデックスでソート） 
            if (activeAnalyteGroupInfo != null)
            {
                if (!isKeepGroupName)
                {
                    // GroupNameテキストボックスの設定
                    txtGroupName.Text = activeAnalyteGroupInfo.GroupName;
                }

                this.txtGroupAnalyte.Text = String.Join(",", activeAnalyteGroupInfo.AnalyteInfos
                                                                .OrderBy(x => x.ProtocolIndex).ToList()
                                                                .Select(funcGetProtocolName));
                this.txtGroupAnalyte.Refresh();

                // analysisSettingPanelで、該当Groupの分析項目のボタンをONにする
                var registerd = from v in activeAnalyteGroupInfo.AnalyteInfos
                                select new Tuple<Int32, Int32, Int32>(v.ProtocolIndex, v.AutoDilution, v.MeasTimes);
                this.analysisSettingPanel.SetProtocolSettingState(registerd.ToList());
            }

        }

        /// <summary>
        /// 分析項目選択状態変更前イベント
        /// </summary>
        /// <remarks>
        /// 選択した分析項目が、現在選択中の分析項目のサンプル種別と矛盾する場合にキャンセルします。
        /// </remarks>
        /// <param name="protocolIndex">分析項目インデックス</param>
        /// <param name="data">分析項目選択状態変更データクラス</param>
        private void analysisSettingPanel_ProtocolCheckChanging(Int32 protocolIndex, Controls.AnalysisSettingPanel.AnalisisSettingPanelSelectChangingData data)
        {
            // 選択した分析項目のサンプル種別と、選択済みの分析項目のサンプル種別に相違があったらエラーとする
            // (「血清または血漿」「尿」のフラグが両方ONの分析項目はチェック対象外。それ以外の分析項目で、全て同じでないとエラーにする)
            // ボタンをONにした時だけチェックする			
            if (data.NextState)
            {
                if (!checkSampleKind(protocolIndex))
                {
                    // エラーメッセージを表示してキャンセル
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_159, String.Empty,
                                CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                    data.Cancel = true;
                }
            }

            return;
        }

        /// <summary>
        /// サンプル種別チェック処理
        /// </summary>
        /// <returns>true:チェックOK、false:チェックNG</returns>
        /// <remarks>現在選択状態になっている分析項目のサンプル種別の不整合がないかをチェックします。</remarks>
        private bool checkSampleKinds()
        {
            foreach (var info in activeAnalyteGroupInfo.AnalyteInfos)
            {
                if (!checkSampleKind(info.ProtocolIndex))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// サンプル種別チェック処理
        /// </summary>
        /// <param name="protocolIndex">プロトコルインデックス</param>
        /// <returns>true:チェックOK、false:チェックNG</returns>
        private bool checkSampleKind(Int32 protocolIndex)
        {
            // 選択した分析項目のサンプル種別を取得する。
            var sampleKind = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(protocolIndex).SampleKind;
            bool checkExit = !(sampleKind.HasFlag(MeasureProtocol.SampleTypeKind.SerumOrPlasma) && sampleKind.HasFlag(MeasureProtocol.SampleTypeKind.Urine));

            // 「血清または血漿」「尿」のフラグが両方ONの分析項目はチェック不要の為、処理を抜ける
            if (!checkExit)
            {
                return true;
            }

            // 選択中の分析項目との比較：片方のフラグがONの種別は全部同じはずなので、
            // 片方のフラグONの分析項目で、最初に見つかった分析項目とだけ比較する
            foreach (var info in activeAnalyteGroupInfo.AnalyteInfos)
            {
                var wksampleKind = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(info.ProtocolIndex).SampleKind;
                bool wkcheckExit = !(wksampleKind.HasFlag(MeasureProtocol.SampleTypeKind.SerumOrPlasma) && wksampleKind.HasFlag(MeasureProtocol.SampleTypeKind.Urine));

                // 「血清または血漿」「尿」のどちらかのフラグがONの分析項目のみチェック対象
                if (wkcheckExit)
                {
                    if (wksampleKind != sampleKind)
                    {
                        return false;
                    }
                    break;
                }
            }

            return true;
        }

        /// <summary>
        /// AnalyteGroupタブRegistrationボタン押下時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// AnalyteGroupの新規登録及び更新を行います。
        /// </remarks>
        private void btnRegistAnalyteGroup_Click(object sender, EventArgs e)
        {
            // 操作履歴「グループ登録実行」
            Singleton<CarisXLogManager>.Instance.Write(
                LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                CarisXLogInfoBaseExtention.Empty,
                this.Text + CarisX.Properties.Resources.STRING_COMMON_013 +
                string.Format(CarisX.Properties.Resources.STRING_LOG_MSG_087, (((Boolean)this.optGroupEditMode.Value) ? txtGroupName.Text : activeAnalyteGroupInfo.GroupName).ToString()));

            Boolean isCreateNew = (Boolean)this.optGroupEditMode.Value;

            // 入力チェック：以下の場合エラー
            // ・新規登録且つtxtGroupNameが未入力の場合
            // ・新規登録で、登録済みのGroupNameを指定した場合
            // ・新規登録/更新共に、分析項目が未選択の場合
            // ・サンプル種別が違う分析項目がONになっている場合
            if ((isCreateNew && String.IsNullOrEmpty(txtGroupName.Text))
                || (isCreateNew && (from m in this.analyteGroupInfoManager.AnalyteGroupInfos where m.GroupName == txtGroupName.Text select m).ToList().Count() > 0)
                || (activeAnalyteGroupInfo.AnalyteInfos.Count == 0)
                || !this.checkSampleKinds())
            {
                // エラーメッセージ
                DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_223, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                // 操作履歴「グループ登録実行をキャンセルしました。」
                Singleton<CarisXLogManager>.Instance.Write(
                    LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty,
                    this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_088);

                return;
            }

            if (isCreateNew)
            {
                // CreateNew選択時
                // AnalyteGroupInfoManagerクラスにactiveAnalyteGroupInfoの内容をコピーして追加する
                activeAnalyteGroupInfo.GroupName = txtGroupName.Text;
                this.analyteGroupInfoManager.AnalyteGroupInfos.Add(activeAnalyteGroupInfo.Copy());
            }
            else
            {
                // Edit選択時
                // 編集対象インスタンスに、activeAnalyteGroupInfoのコピーをセットする
                this.analyteGroupInfoManager.AnalyteGroupInfos[GetAnalyteGroupInfosIndex(activeAnalyteGroupInfo.GroupName)] = activeAnalyteGroupInfo.Copy();
            }

            // xmlに保存する
            AnalyteGroupManager.SaveAnalyteGroup(this.analyteGroupInfoManager.AnalyteGroupInfos);

            // 登録が完了しましたダイアログ表示
            DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_224, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OK);


            // コンボボックスのリスト再生成
            this.BindAnalyteGroupNameCombo();


            // 新規登録且つ、コンボボックスが何も選択されていなければ先頭を選択する
            if (isCreateNew)
            {
                if (this.cmbGroupName.SelectedIndex < 0)
                {
                    this.cmbGroupName.SelectedItem = this.cmbGroupName.Items[0];
                }

                // EditModeオプションボタンの設定
                this.setAnalyteGroupEditMode(false);
            }

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// GroupNameコンボボックス更新
        /// </summary>
        private void BindAnalyteGroupNameCombo()
        {
            this.cmbGroupName.DataSource = this.analyteGroupInfoManager.AnalyteGroupInfos;
            this.cmbGroupName.DisplayMember = "GroupName";
            this.cmbGroupName.DataBind();
        }

        /// <summary>
        /// AnalyteGroupタブDeleteボタン押下時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// AnalyteGroupの削除を行います。
        /// </remarks>
        private void btnDeleteAnlyteGroup_Click(object sender, EventArgs e)
        {
            // 操作履歴「グループ削除実行」
            Singleton<CarisXLogManager>.Instance.Write(
                LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                CarisXLogInfoBaseExtention.Empty,
                this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + string.Format(CarisX.Properties.Resources.STRING_LOG_MSG_089, activeAnalyteGroupInfo.GroupName));

            if (DialogResult.OK != DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_038, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel))
            {
                // 操作履歴「グループ削除実行をキャンセルしました。」
                Singleton<CarisXLogManager>.Instance.Write(
                    LogKind.OperationHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                    CarisXLogInfoBaseExtention.Empty,
                    this.Text + CarisX.Properties.Resources.STRING_COMMON_013 + CarisX.Properties.Resources.STRING_LOG_MSG_088);
                return;
            }

            // 更新後に削除リストの1つ前を選択するために、現在のリストインデックスを保持しておく
            Int32 beforeIndex = this.cmbGroupName.SelectedIndex;

            // 削除対象項目を退避領域から削除
            this.analyteGroupInfoManager.AnalyteGroupInfos.Remove(this.GetAnalyteGroupInfoByComboBox());

            // xmlに保存する
            AnalyteGroupManager.SaveAnalyteGroup(this.analyteGroupInfoManager.AnalyteGroupInfos);

            // コンボボックスのリスト再生成
            this.cmbGroupName.DataBind();

            // コンボボックスのリスト再選択
            if (this.analyteGroupInfoManager.AnalyteGroupInfos.Count > 0)
            {
                if (beforeIndex > 1)
                {
                    this.cmbGroupName.SelectedItem = this.cmbGroupName.Items[beforeIndex - 1];
                }
                else
                {
                    this.cmbGroupName.SelectedItem = this.cmbGroupName.Items[0];
                }
            }

            // EditModeオプションボタンの設定
            this.setAnalyteGroupEditMode(false);

            // Form共通の編集中フラグOFF
            FormChildBase.IsEdit = false;
        }

        /// <summary>
        /// AnalyteGroupInfosのインデックス取得
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        /// <remarks>
        /// analyteGroupInfoManager.AnalyteGroupInfosを
        /// GroupNameを条件に該当要素を検索し、要素位置のインデックスを返します。
        /// </remarks>
        private Int32 GetAnalyteGroupInfosIndex(string groupName)
        {
            Int32 index = 0;
            foreach (AnalyteGroupInfo analyteGroupInfo in this.analyteGroupInfoManager.AnalyteGroupInfos)
            {
                if (analyteGroupInfo.GroupName == groupName)
                {
                    break;
                }
                index++;
            }
            return index;
        }

        #endregion

        /// <summary>
        /// 更新选中的选项的更新状态
        /// </summary>
        private void UpdateTheCurrentPageSelectPrococol()
        {
            try
            {
                Int32 i = 0;
                Int32 j = MaxNumberOfOnePage * (ProtocolPageIndex - 1) + i;
                for (; i < MaxNumberOfOnePage; i++)
                {
                    ProtocolAndSelect item = new ProtocolAndSelect();

                    item.Name = this.AnalyteTableButtons[i].Text;
                    item.bSelect = this.AnalyteTableButtons[i].CurrentState;
                    MapProtocolAndSelect[j + 1] = item;
                    j++;
                }
            }
            catch (System.Exception ex)
            {
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("An exception occurred in UpdateTheCurrentPageSelectPrococol : {0}", ex.StackTrace));
            }

        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            UpdateTheCurrentPageSelectPrococol();
            ProtocolPageIndex--;
            if (ProtocolPageIndex == 0)
            {
                ProtocolPageIndex = MaxNumberOfProtocolPages;
            }
            updateAnalyteTableButtonText();

        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            UpdateTheCurrentPageSelectPrococol();
            ProtocolPageIndex++;
            if (ProtocolPageIndex == (MaxNumberOfProtocolPages + 1))
            {
                ProtocolPageIndex = 1;
            }
            updateAnalyteTableButtonText();
        }

        /// <summary>
        /// 画面のコントロールの位置を変更する
        /// </summary>
        void ControlLocationAdjust()
        {
            int adjustx = 180;
            int invisibleCount = 0;

            foreach (var tbp in this.tabSystemAnalytes.Tabs)
            {
                if (!tbp.Visible)
                    invisibleCount += 1;
            }

            //１タブ非表示の場合のLocationは734, 33
            //２タブ非表示の場合のLocationは554, 33
            pnlSystemAnalytesShadowInside.Location = new Point(DefaultLocationXpnlSystemAnalytesShadowInside - (adjustx * invisibleCount)
                                                                , DefaultLocationYpnlSystemAnalytesShadowInside);
            pnlSystemAnalytesShadowInside.Size = new Size(DefaultSizeWidthpnlSystemAnalytesShadowInside + (adjustx * invisibleCount)
                                                            , DefaultSizeHeightpnlSystemAnalytesShadowInside);

        }

        /// <summary>
        /// タブページ「Analyte No_」のグリッドセル変更イベント
        /// </summary>
        /// <remarks>
        /// タブページ「Analyte No_」の値変更時、Form共通の編集中フラグチェックを実施
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdAnalyteNo_CellChange(object sender, CellEventArgs e)
        {
            // Form共通の編集中フラグON
            FormChildBase.IsEdit = true;
        }

        /// <summary>
        /// タブページ「Analyte Group_」のセル変更イベント
        /// </summary>
        /// <remarks>
        /// タブページ「Analyte Group_」の値変更時、Form共通の編集中フラグチェックを実施
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtGroupName_ValueChanged(object sender, EventArgs e)
        {
            // Create New_の場合のみフラグを変更
           if (this.optGroupEditMode.CheckedIndex == 1)
            {
                // Form共通の編集中フラグON
                FormChildBase.IsEdit = true;
            }
        }

        /// <summary>
        /// タブページを切り替える時、Form共通の編集中フラグチェックを実施
        /// </summary>
        /// <remarks>
        /// 共通フラグを使用しているので、複数のタブページで編集した場合に判定が行えない為、
        /// タブページ毎にチェックを実施します。
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabSystemAnalytes_ActiveTabChanging(object sender, Infragistics.Win.UltraWinTabControl.ActiveTabChangingEventArgs e)
        {
            // 編集中かどうか、また編集中でも画面遷移するかどうか
            if (CarisXSubFunction.IsEditsMessageShow())
            {
                // タブページを切り替える

                // Form共通の編集中フラグOFF
                FormChildBase.IsEdit = false;
            }
            else
            {
                // タブページを切り替えない
                e.Cancel = true;
            }
        }
    }
}
