using System;
using System.Linq;
using System.Windows.Forms;
using Oelco.Common.Utility;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Const;
using Oelco.Common.Comm;
using Oelco.CarisX.GUI;

namespace Oelco.CarisX.Maintenance
{
    /// <summary>
    /// モーター初期化ダイアログクラス
    /// </summary>
    public partial class DlgWaitMotorInitialize : DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgWaitMotorInitialize()
        {
            InitializeComponent();
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
            updateMotorInitStatus();
            conditionalFormClose();
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
            this.btnCancel.Text = Properties.Resources.STRING_COMMON_002;

            // ダイアログタイトル
            this.Caption = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_000;

            // ラック搬送
            this.lblRackTransfer.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_001;
            this.lblRackTransferValue.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_008;

            // モジュール１
            this.lblModule1.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_002;
            this.lblModule1Value.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_008;

            // モジュール２
            this.lblModule2.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_003;
            this.lblModule2Value.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_008;

            // モジュール３
            this.lblModule3.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_004;
            this.lblModule3Value.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_008;

            // モジュール４
            this.lblModule4.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_005;
            this.lblModule4Value.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_008;
        }

        /// <summary>
        /// Cancelボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にキャンセルを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        protected virtual void btnCancel_Click( object sender, EventArgs e )
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// FormClosingイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void DlgWaitMotorInitialize_FormClosing( object sender, FormClosingEventArgs e )
        {
            // 画面が閉じられる際、モーター初期化完了通知対象からこの画面を外す。
            Singleton<NotifyManager>.Instance.RemoveNotifyTarget((Int32)NotifyKind.MotorInitializeCompleted, this.onMotorInitializeCompleted);
        }

        /// <summary>
        /// 画面表示完了イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void DlgWaitMotorInitialize_Shown( object sender, EventArgs e )
        {
            // 画面が表示される際、モーター初期化完了通知対象に登録する。
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.MotorInitializeCompleted, this.onMotorInitializeCompleted);

            //ラック搬送
            if (Singleton<CarisXCommManager>.Instance.GetRackTransferCommStatus() == ConnectionStatus.Online)
            {
                // 接続されている場合のみ、モーター初期化を実行
                Singleton<CarisXSequenceHelperManager>.Instance.RackTransfer.MotorInitializeRackSequence();
            }

            //モジュール１～４
            foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                {
                    // 接続されている場合のみ、モーター初期化を実行
                    Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleindex].MotorInitializeModuleSequence();
                }
            }

        }

        /// <summary>
        /// モーター初期化完了処理（モジュール）
        /// </summary>
        /// <param name="value">モジュールインデックス</param>
        protected void onMotorInitializeCompleted( Object value )
        {
            String resultValue = "";

            //設定する値のフラグを設定
            if (((MotorInitCompStatusKind)value).HasFlag(MotorInitCompStatusKind.Completed))
                resultValue = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_007;
            else
                resultValue = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_009;

            //Enumの値を配列で取る
            var MotorInitCompStatusValues = Enum.GetValues(typeof(MotorInitCompStatusKind));
            foreach (MotorInitCompStatusKind MotorInitCompStatusValue in MotorInitCompStatusValues)
            {
                //ANDビット演算で引数の値にEnumの値が立っているかチェックする
                if (((MotorInitCompStatusKind)value & MotorInitCompStatusValue) == MotorInitCompStatusValue)
                {
                    switch (MotorInitCompStatusValue)
                    {
                        case MotorInitCompStatusKind.Rack:
                            lblRackTransferValue.Text = resultValue;
                            break;
                        case MotorInitCompStatusKind.Module1:
                            lblModule1Value.Text = resultValue;
                            break;
                        case MotorInitCompStatusKind.Module2:
                            lblModule2Value.Text = resultValue;
                            break;
                        case MotorInitCompStatusKind.Module3:
                            lblModule3Value.Text = resultValue;
                            break;
                        case MotorInitCompStatusKind.Module4:
                            lblModule4Value.Text = resultValue;
                            break;
                    }
                }
            }

            conditionalFormClose();
        }

        /// <summary>
        /// 条件付き画面終了処理
        /// </summary>
        protected void conditionalFormClose()
        {
            if (chkCompletedMotorInitialize())
            {
                //Not Completedを含まない場合、画面を終了する
                this.Close();
            }
        }

        /// <summary>
        /// モーター初期化完了チェック
        /// </summary>
        /// <returns>true:完了、false:処理中</returns>
        protected Boolean chkCompletedMotorInitialize()
        {
            String[] ultraLabels = new String[] { lblRackTransferValue.Text, lblModule1Value.Text, lblModule2Value.Text, lblModule3Value.Text, lblModule4Value.Text };

            if (ultraLabels.Contains(Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_008))
            {
                //Not Completedを含む場合、モーター初期化は完了していない
                return false;
            }

            //Not Completedを含む場合、モーター初期化は完了している
            return true;
        }

        /// <summary>
        /// モーター初期化状態を更新
        /// </summary>
        protected void updateMotorInitStatus()
        {
            //ラック搬送
            if (Singleton<CarisXCommManager>.Instance.GetRackTransferCommStatus() == ConnectionStatus.Online)
                lblRackTransferValue.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_008;
            else
                lblRackTransferValue.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_006;

            //モジュール１
            if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus((int)ModuleIndex.Module1) == ConnectionStatus.Online)
                lblModule1Value.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_008;
            else
                lblModule1Value.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_006;

            //モジュール２
            if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus((int)ModuleIndex.Module2) == ConnectionStatus.Online)
                lblModule2Value.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_008;
            else
                lblModule2Value.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_006;

            //モジュール３
            if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus((int)ModuleIndex.Module3) == ConnectionStatus.Online)
                lblModule3Value.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_008;
            else
                lblModule3Value.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_006;

            //モジュール４
            if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus((int)ModuleIndex.Module4) == ConnectionStatus.Online)
                lblModule4Value.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_008;
            else
                lblModule4Value.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_WAITMOTORINIT_006;

        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 表示必要性
        /// </summary>
        /// <remarks>
        /// 現在このダイアログを表示する必要があるかどうかを取得します。
        /// </remarks>
        /// <returns>true:要 false:不要</returns>
        public Boolean IsNeedShow()
        {
            //画面の内容を更新
            updateMotorInitStatus();
            return !chkCompletedMotorInitialize();
        }
        #endregion
    }

}
