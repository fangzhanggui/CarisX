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

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// メイン画面のスモールメニュー表示
    /// </summary>
    /// <remarks>
    /// メイン画面の上に透過用のダミーフォームを表示し、その上にスモールメニューのフォームを配置する
    /// </remarks>
    public partial class FormMainFrameSmallMenu : FormTransitionBase
    {

        #region [クラス変数定義]

        /// <summary>
        /// ダミー画面
        /// </summary>
        private FormBackScreen formBackScreen = null;

        #endregion

        #region インスタンス変数定義

        /// <summary>
        /// 各スモールメニュー毎に有効にするコントロールを保存する為の構造体
        /// </summary>
        private struct EnableControl
        {
            public String name;
            public Point location;
            public String parent;
        }

        /// <summary>
        /// 各スモールメニュー毎に有効にするコントロールを保存するリスト
        /// </summary>
        private List<List<EnableControl>> EnableControls;

        public readonly Point AdjustLocation = new Point(255, 0);      //コントロールの表示位置調整用

        readonly Point TitleLocation = new Point(863, 174);     //スモールメニューのタイトル表示場所
        readonly Point CloseLocation = new Point(1383, 174);    //スモールメニューの閉じるボタン表示場所

        #endregion

        #region プロパティ

        public SmallMenuKind SmallMenuType { get; set; }

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormMainFrameSmallMenu()
        {
            InitializeComponent();

            this.lblMenuSmallSpecimenTitle.Text = Resources.STRING_MAIN_FRAME_002;
            this.lblMenuSmallAssayTitle.Text = Resources.STRING_MAIN_FRAME_000;
            this.lblMenuSmallReagentTitle.Text = Resources.STRING_MAIN_FRAME_001;
            this.lblMenuSmallCalibTitle.Text = Resources.STRING_MAIN_FRAME_003;
            this.lblMenuSmallControlTitle.Text = Resources.STRING_MAIN_FRAME_004;
            this.lblMenuSmallSystemTitle.Text = Resources.STRING_MAIN_FRAME_005;

            this.lblAssayStatus.Text = Resources.STRING_MAIN_FRAME_014;
            this.lblReagentPreparation.Text = Resources.STRING_MAIN_FRAME_015;
            this.lblSpecimenRegistration.Text = Resources.STRING_MAIN_FRAME_016;
            this.lblSpecimenStatRegistration.Text = Resources.STRING_MAIN_FRAME_043;
            this.lblSpecimenResult.Text = Resources.STRING_MAIN_FRAME_017;
            this.lblSpecimenRetest.Text = Resources.STRING_MAIN_FRAME_018;
            this.lblCalibRegistration.Text = Resources.STRING_MAIN_FRAME_019;
            this.lblCalibStatus.Text = Resources.STRING_MAIN_FRAME_020;
            this.lblCalibAnalysis.Text = Resources.STRING_MAIN_FRAME_021;
            this.lblCalibResult.Text = Resources.STRING_MAIN_FRAME_022;
            this.lblControlRegistration.Text = Resources.STRING_MAIN_FRAME_023;
            this.lblControlQC.Text = Resources.STRING_MAIN_FRAME_024;
            this.lblControlResult.Text = Resources.STRING_MAIN_FRAME_025;
            this.lblSystemStatus.Text = Resources.STRING_MAIN_FRAME_026;
            this.lblSystemAnalytes.Text = Resources.STRING_MAIN_FRAME_027;
            this.lblSystemLog.Text = Resources.STRING_MAIN_FRAME_028;
            this.lblSystemOption.Text = Resources.STRING_MAIN_FRAME_029;
            this.lblSystemAdjustment.Text = Resources.STRING_MAIN_FRAME_030;
            this.lblSystemUser.Text = Resources.STRING_MAIN_FRAME_031;

            SetEnableControls();
        }

        #endregion

        #region イベントハンドラ

        #region Form
        private void FormMainFrameSmallMenu_Load(object sender, EventArgs e)
        {
            //画面に表示するコントロールの設定
            SetSmallMenuControl();

            if (SmallMenuType == SmallMenuKind.System)
            {
                EnabledControlByStatus();
                setUser();
            }

        }

        private void FormMainFrameSmallMenu_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Specimen
        private void btnSpecimenRegistration_Click(object sender, EventArgs e)
        {
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SmallMenuSpecimenRegistration, null);
            this.Close();
        }

        private void btnSpecimenStatRegistration_Click( object sender, EventArgs e )
        {
            Singleton<NotifyManager>.Instance.PushSignalQueue( (Int32)NotifyKind.SmallMenuSpecimenStatRegistration, null );
            this.Close();
        }

        private void btnSpecimenResult_Click(object sender, EventArgs e)
        {
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SmallMenuSpecimenResult, null);
            this.Close();
        }

        private void btnSpecimenRetest_Click(object sender, EventArgs e)
        {
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SmallMenuSpecimenRetest, null);
            this.Close();
        }

        #endregion

        #region Assay
        private void btnAssayStatus_Click(object sender, EventArgs e)
        {
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SmallMenuAssayStatus, null);
            this.Close();
        }
        #endregion

        #region Reagent
        private void btnReagentPreparation_Click(object sender, EventArgs e)
        {
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SmallMenuReagentPreparation, null);
            this.Close();
        }
        #endregion

        #region Calibration
        private void btnCalibRegistration_Click(object sender, EventArgs e)
        {
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SmallMenuCalibRegistration, null);
            this.Close();
        }

        private void btnCalibrationStatus_Click(object sender, EventArgs e)
        {
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SmallMenuCalibStatus, null);
            this.Close();
        }

        private void btnCalibAnalysis_Click(object sender, EventArgs e)
        {
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SmallMenuCalibAnalysis, null);
            this.Close();
        }

        private void btnClibResult_Click(object sender, EventArgs e)
        {
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SmallMenuCalibResult, null);
            this.Close();
        }
        #endregion

        #region Control
        private void btnControlRegistration_Click(object sender, EventArgs e)
        {
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SmallMenuControlRegistration, null);
            this.Close();
        }

        private void btnControlQC_Click(object sender, EventArgs e)
        {
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SmallMenuControlQC, null);
            this.Close();
        }

        private void btnControlResult_Click(object sender, EventArgs e)
        {
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SmallMenuControlResult, null);
            this.Close();
        }
        #endregion

        #region Option

        #endregion

        #region System
        private void btnSystemStatus_Click(object sender, EventArgs e)
        {
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SmallMenuSystemStatus, null);
            this.Close();
        }

        private void btnSystemAnalytes_Click(object sender, EventArgs e)
        {
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SmallMenuSystemAnalytes, null);
            this.Close();
        }

        private void btnSystemLog_Click(object sender, EventArgs e)
        {
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SmallMenuSystemLog, null);
            this.Close();
        }

        private void btnSystemOption_Click(object sender, EventArgs e)
        {
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SmallMenuSystemOption, null);
            this.Close();
        }

        private void btnSystemAdjustment_Click(object sender, EventArgs e)
        {
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SmallMenuSystemAdjustment, null);
            this.Close();
        }

        private void btnSystemUser_Click(object sender, EventArgs e)
        {
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SmallMenuSystemUser, null);
            this.Close();
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
        /// ユーザレベル設定
        /// </summary>
        /// <remarks>
        /// ユーザーレベルの設定を行います
        /// </remarks>
        private void setUser()
        {
            // メンテナンス(ユニットの調整)のレベル設定
            this.SetEnableSystemAdjustment();

            // ユーザ管理のレベル設定
            Boolean enable = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.UserManage);
            btnSystemUser.Visible = enable;
            btnSystemUser.Enabled = enable;
            lblSystemUser.Visible = enable;
            lblSystemUser.Enabled = enable;
        }

        /// <summary>
        /// 各モード毎に有効にするコントロールの設定
        /// </summary>
        /// <remarks>
        /// 表示するスモールメニューに応じて、表示するコントロールをListで管理します。
        /// コントロールを参照するためにコンストラクタで呼び出し。ロード時にListの内容をもとに自動でVisibleの設定を行う
        /// </remarks>
        private void SetEnableControls()
        {
            EnableControls = new List<List<EnableControl>>
            {
                //Specimen
                new List<EnableControl>
                {
                    new EnableControl(){name=lblMenuSmallSpecimenTitle.Name, location=TitleLocation, parent="" },
                    new EnableControl(){name=btnSpecimenRegistration.Name, location=new Point(CarisXConst.ButtonMAX4XLeft,CarisXConst.ButtonYUpper), parent="" },
                    new EnableControl(){name=lblSpecimenRegistration.Name, location=new Point(CarisXConst.ButtonMAX4XLeft,CarisXConst.LabelYUpper), parent=btnSpecimenRegistration.Name },
                    new EnableControl(){name=btnSpecimenStatRegistration.Name, location=new Point(CarisXConst.ButtonMAX4XRight,CarisXConst.ButtonYUpper), parent="" },
                    new EnableControl(){name=lblSpecimenStatRegistration.Name, location=new Point(CarisXConst.ButtonMAX4XRight,CarisXConst.LabelYUpper), parent=btnSpecimenStatRegistration.Name },
                    new EnableControl(){name=btnSpecimenResult.Name, location=new Point(CarisXConst.ButtonMAX4XLeft,CarisXConst.ButtonYLower), parent="" },
                    new EnableControl(){name=lblSpecimenResult.Name, location=new Point(CarisXConst.ButtonMAX4XLeft,CarisXConst.LabelYLower), parent=btnSpecimenResult.Name },
                    new EnableControl(){name=btnSpecimenRetest.Name, location=new Point(CarisXConst.ButtonMAX4XRight,CarisXConst.ButtonYLower), parent="" },
                    new EnableControl(){name=lblSpecimenRetest.Name, location=new Point(CarisXConst.ButtonMAX4XRight,CarisXConst.LabelYLower), parent=btnSpecimenRetest.Name },
                },

                //Assay
                new List<EnableControl>
                {
                    new EnableControl(){name=lblMenuSmallAssayTitle.Name, location=TitleLocation, parent="" },
                    new EnableControl(){name=btnAssayStatus.Name, location=new Point(CarisXConst.ButtonMAX6XMiddle,CarisXConst.ButtonYUpper), parent="" },
                    new EnableControl(){name=lblAssayStatus.Name, location=new Point(CarisXConst.ButtonMAX6XMiddle,CarisXConst.LabelYUpper), parent=btnAssayStatus.Name },
                },
                
                //Reagent
                new List<EnableControl>
                {
                    new EnableControl(){name=lblMenuSmallReagentTitle.Name, location=TitleLocation, parent="" },
                    new EnableControl(){name=btnReagentPreparation.Name, location=new Point(CarisXConst.ButtonMAX6XMiddle,CarisXConst.ButtonYUpper), parent="" },
                    new EnableControl(){name=lblReagentPreparation.Name, location=new Point(CarisXConst.ButtonMAX6XMiddle,CarisXConst.LabelYUpper), parent=btnReagentPreparation.Name },
                },
                
                //Calibration
                new List<EnableControl>
                {
                    new EnableControl(){name=lblMenuSmallCalibTitle.Name, location=TitleLocation, parent="" },
                    new EnableControl(){name=btnCalibRegistration.Name, location=new Point(CarisXConst.ButtonMAX4XLeft,CarisXConst.ButtonYUpper), parent="" },
                    new EnableControl(){name=lblCalibRegistration.Name, location=new Point(CarisXConst.ButtonMAX4XLeft,CarisXConst.LabelYUpper), parent=btnCalibRegistration.Name },
                    new EnableControl(){name=btnCalibStatus.Name, location=new Point(CarisXConst.ButtonMAX4XRight,CarisXConst.ButtonYUpper), parent="" },
                    new EnableControl(){name=lblCalibStatus.Name, location=new Point(CarisXConst.ButtonMAX4XRight,CarisXConst.LabelYUpper), parent=btnCalibStatus.Name },
                    new EnableControl(){name=btnCalibAnalysis.Name, location=new Point(CarisXConst.ButtonMAX4XLeft,CarisXConst.ButtonYLower), parent="" },
                    new EnableControl(){name=lblCalibAnalysis.Name, location=new Point(CarisXConst.ButtonMAX4XLeft,CarisXConst.LabelYLower), parent=btnCalibAnalysis.Name },
                    new EnableControl(){name=btnCalibResult.Name, location=new Point(CarisXConst.ButtonMAX4XRight,CarisXConst.ButtonYLower), parent="" },
                    new EnableControl(){name=lblCalibResult.Name, location=new Point(CarisXConst.ButtonMAX4XRight,CarisXConst.LabelYLower), parent=btnCalibResult.Name },
                },
                
                //Control
                new List<EnableControl>
                {
                    new EnableControl(){name=lblMenuSmallControlTitle.Name, location=TitleLocation, parent="" },
                    new EnableControl(){name=btnControlRegistration.Name, location=new Point(CarisXConst.ButtonMAX6XLeft,CarisXConst.ButtonYUpper), parent="" },
                    new EnableControl(){name=lblControlRegistration.Name, location=new Point(CarisXConst.ButtonMAX6XLeft,CarisXConst.LabelYUpper), parent=btnControlRegistration.Name },
                    new EnableControl(){name=btnControlQC.Name, location=new Point(CarisXConst.ButtonMAX6XMiddle,CarisXConst.ButtonYUpper), parent="" },
                    new EnableControl(){name=lblControlQC.Name, location=new Point(CarisXConst.ButtonMAX6XMiddle,CarisXConst.LabelYUpper), parent=btnControlQC.Name },
                    new EnableControl(){name=btnControlResult.Name, location=new Point(CarisXConst.ButtonMAX6XRight,CarisXConst.ButtonYUpper), parent="" },
                    new EnableControl(){name=lblControlResult.Name, location=new Point(CarisXConst.ButtonMAX6XRight,CarisXConst.LabelYUpper), parent=btnControlResult.Name },
                },

                //Option
                new List<EnableControl>
                {
                    new EnableControl(){name=lblMenuSmallSystemTitle.Name, location=TitleLocation, parent="" },
                },

                //System
                new List<EnableControl>
                {
                    new EnableControl(){name=lblMenuSmallSystemTitle.Name, location=TitleLocation, parent="" },
                    new EnableControl(){name=btnSystemStatus.Name, location=new Point(CarisXConst.ButtonMAX6XLeft,CarisXConst.ButtonYUpper), parent="" },
                    new EnableControl(){name=lblSystemStatus.Name, location=new Point(CarisXConst.ButtonMAX6XLeft,CarisXConst.LabelYUpper), parent=btnSystemStatus.Name },
                    new EnableControl(){name=btnSystemAnalytes.Name, location=new Point(CarisXConst.ButtonMAX6XMiddle,CarisXConst.ButtonYUpper), parent="" },
                    new EnableControl(){name=lblSystemAnalytes.Name, location=new Point(CarisXConst.ButtonMAX6XMiddle,CarisXConst.LabelYUpper), parent=btnSystemAnalytes.Name },
                    new EnableControl(){name=btnSystemLog.Name, location=new Point(CarisXConst.ButtonMAX6XRight,CarisXConst.ButtonYUpper), parent="" },
                    new EnableControl(){name=lblSystemLog.Name, location=new Point(CarisXConst.ButtonMAX6XRight,CarisXConst.LabelYUpper), parent=btnSystemLog.Name },
                    new EnableControl(){name=btnSystemOption.Name, location=new Point(CarisXConst.ButtonMAX6XLeft,CarisXConst.ButtonYLower), parent="" },
                    new EnableControl(){name=lblSystemOption.Name, location=new Point(CarisXConst.ButtonMAX6XLeft,CarisXConst.LabelYLower), parent=btnSystemOption.Name },
                    new EnableControl(){name=btnSystemAdjustment.Name, location=new Point(CarisXConst.ButtonMAX6XMiddle,CarisXConst.ButtonYLower), parent="" },
                    new EnableControl(){name=lblSystemAdjustment.Name, location=new Point(CarisXConst.ButtonMAX6XMiddle,CarisXConst.LabelYLower), parent=btnSystemAdjustment.Name },
                    new EnableControl(){name=btnSystemUser.Name, location=new Point(CarisXConst.ButtonMAX6XRight,CarisXConst.ButtonYLower), parent="" },
                    new EnableControl(){name=lblSystemUser.Name, location=new Point(CarisXConst.ButtonMAX6XRight,CarisXConst.LabelYLower), parent=btnSystemUser.Name },
                },
            };
        }

        /// <summary>
        /// SystemAdjustmentボタンの活性/非活性
        /// </summary>
        /// <remarks>
        /// メンテナンス(ユニットの調整)のレベル設定します
        /// </remarks>
        private void SetEnableSystemAdjustment()
        {
            // メンテナンス(ユニットの調整)のレベル設定
            btnSystemAdjustment.Visible = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.Maintenance);
            btnSystemAdjustment.Enabled = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.Maintenance);
            lblSystemAdjustment.Visible = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.Maintenance);
            lblSystemAdjustment.Enabled = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.Maintenance);

            //使用権限があっても、システムステータスを見て、使用不可の場合は非活性にする
            if (btnSystemAdjustment.Enabled)
            {
                switch (Singleton<SystemStatus>.Instance.Status)
                {
                    case SystemStatusKind.NoLink:
                    case SystemStatusKind.Assay:
                    case SystemStatusKind.ToEndAssay:
                    case SystemStatusKind.SamplingPause:
                    case SystemStatusKind.ReagentExchange:
#if false
                        //DEBUG実行時はラック搬送、スレーブに接続されていなくても
                        //adjustment画面へ遷移できるようにする
                        btnSystemAdjustment.Enabled = true;
                        lblSystemAdjustment.Enabled = true;
#else
                        btnSystemAdjustment.Enabled = false;
                        lblSystemAdjustment.Enabled = false;
#endif
                        break;
                }
            }
        }

        /// <summary>
        /// システム状態変化イベントハンドラ
        /// </summary>
        /// <remarks>
        /// システムのステータスに変化があった時呼び出されます。
        /// </remarks>
        /// <param name="parameter"></param>
        private void EnabledControlByStatus()
        {
            if (Singleton<SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId(Singleton<PublicMemory>.Instance.moduleIndex)] == SystemStatusKind.ReagentExchange)
            {
                //対象のモジュールが試薬交換中の場合
                this.btnSystemLog.Enabled = false;
                this.lblSystemLog.Enabled = false;
            }
            else
            {
                //対象のモジュールが試薬交換中以外の場合はシステムステータスに応じた制御を行う
                switch (Singleton<SystemStatus>.Instance.Status)
                {
                    case SystemStatusKind.NoLink:
                        this.btnSystemLog.Enabled = true;
                        this.lblSystemLog.Enabled = true;
                        break;

                    case SystemStatusKind.Standby:
                        this.btnSystemLog.Enabled = true;
                        this.lblSystemLog.Enabled = true;
                        break;

                    case SystemStatusKind.WaitSlaveResponce:
                        this.btnSystemLog.Enabled = false;
                        this.lblSystemLog.Enabled = false;
                        break;

                    case SystemStatusKind.Assay:
                        this.btnSystemLog.Enabled = true;
                        this.lblSystemLog.Enabled = true;
                        break;

                    case SystemStatusKind.ToEndAssay:
                        this.btnSystemLog.Enabled = true;
                        this.lblSystemLog.Enabled = true;
                        break;

                    case SystemStatusKind.SamplingPause:
                        this.btnSystemLog.Enabled = true;
                        this.lblSystemLog.Enabled = true;
                        break;

                    case SystemStatusKind.Shutdown:
                        this.btnSystemLog.Enabled = false;
                        this.lblSystemLog.Enabled = false;
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// スモールメニューのコントロール設定
        /// </summary>
        /// <remarks>
        /// スモールメニューに表示するコントロールのVisible、Location、Parentの設定を行う
        /// </remarks>
        private void SetSmallMenuControl()
        {
            //×ボタンは常に表示する
            btnMenuSmallClose.Location = CloseLocation - (Size)AdjustLocation;

            //EnableControlsに設定されている内容を元に、表示するコントロールを決定する
            foreach (EnableControl SmallMenuControl in EnableControls[(int)SmallMenuType])
            {
                //フォーム上のコントロールを検索する
                Control[] controls = Controls.Find(SmallMenuControl.name, true);
                foreach (Control ctl in controls)
                {
                    ctl.Visible = true;
                    ctl.Location = SmallMenuControl.location - (Size)AdjustLocation;
                    if (SmallMenuControl.parent != "")
                    {
                        //親コントロールが設定されている場合は、
                        //Parentに設定されているコントロールの子コントロールになるように設定する
                        Control ParentControl = Controls[SmallMenuControl.parent];
                        ctl.Parent = ParentControl;
                        ctl.Location -= (Size)ParentControl.Location;
                    }
                }
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
