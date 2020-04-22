using System;
using System.Linq;
using System.Windows.Forms;
using Oelco.Common.Utility;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Const;
using Oelco.Common.Comm;
using Oelco.CarisX.GUI;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;

namespace Oelco.CarisX.Maintenance
{
    /// <summary>
    /// メンテナンス画面開始ダイアログクラス
    /// </summary>
    public partial class DlgMaintenanceStart : DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgMaintenanceStart()
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
            this.Caption = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_MAINTENANCESTART_000;

            // お待ちください
            this.lblPleaseWait.Text = Properties.Resources_Maintenance.STRING_DLG_MAINTENANCE_MAINTENANCESTART_001;
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
            this.Close();
        }

        /// <summary>
        /// FormClosingイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void DlgPleaseWait_FormClosing( object sender, FormClosingEventArgs e )
        {
            // 画面が閉じられる際、モーター初期化完了通知対象からこの画面を外す。
            Singleton<NotifyManager>.Instance.RemoveNotifyTarget((Int32)NotifyKind.MaintenanceStartCompleted, this.onMaintenanceStartCompleted);
        }

        /// <summary>
        /// 画面表示完了イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void DlgPleaseWait_Shown( object sender, EventArgs e )
        {
            // 画面が表示される際、モーター初期化完了通知対象に登録する。
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.MaintenanceStartCompleted, this.onMaintenanceStartCompleted);

            Boolean askSuccess = true;

            //シーケンス同期オブジェクトと送信スキップ対象フラグの初期化
            CarisXSequenceHelper.SequenceSyncObject[] syncDataList = new CarisXSequenceHelper.SequenceSyncObject[Enum.GetValues(typeof(RackModuleIndex)).Length];
            Boolean[] SkipList = new Boolean[Enum.GetValues(typeof(RackModuleIndex)).Length];
            foreach (Int32 moduleid in Enum.GetValues(typeof(RackModuleIndex)))
            {
                syncDataList[moduleid] = new CarisXSequenceHelper.SequenceSyncObject();
                SkipList[moduleid] = true;  //接続されていない場合は無視する為、初期値はtrue（=無視）にしておく
            }

            ParameterFilePreserve<CarisXConfigParameter> config = Singleton<ParameterFilePreserve<CarisXConfigParameter>>.Instance;
            config.LoadRaw();

            //ラックが接続されているか
            if (Singleton<CarisXCommManager>.Instance.GetRackTransferCommStatus() == ConnectionStatus.Online)
            {
                SkipList[(Int32)RackModuleIndex.RackTransfer] = false;

                //接続されている場合はコンフィグパラメータの送信シーケンスを呼び出しする
                syncDataList[(Int32)RackModuleIndex.RackTransfer] = Singleton<CarisXSequenceHelperManager>.Instance.RackTransfer
                    .SendRackConfigParameter(config.Param.RackList[(Int32)RackModuleIndex.RackTransfer]);
            }

            foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                //モジュールが接続されているか
                if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                {
                    SkipList[CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)moduleindex)] = false;

                    //接続されている場合はコンフィグパラメータの送信シーケンスを呼び出しする
                    syncDataList[CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)moduleindex)] = Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleindex]
                        .SendModuleConfigParameter(config.Param.SlaveList[moduleindex]);
                }
            }

            //送信対象となったそれぞれのシーケンスが完了出来ている事を順番にチェックする。
            foreach (int idx in Enum.GetValues(typeof(RackModuleIndex)))
            {
                if (!SkipList[idx])
                {
                    //送信スキップ対象になっていない場合
                    while (!syncDataList[idx].EndSequence.WaitOne(10))
                    {
                        // ここをDoEventsでの待ちにしない場合、上位の処理をブロック単位に切り分けて複数段階での実行を行う事になり
                        // コード全体の見通しが悪くなる為使用する。メインスレッドをブロックして構わない場合この限りではない。
                        Application.DoEvents();
                    }
                    if (syncDataList[idx].Status != CarisXSequenceHelper.SequenceSyncObject.SequenceStatus.Success)
                    {
                        //success以外が設定された場合はエラー
                        askSuccess = false;
                    }
                }
            }

            if (askSuccess)
                //成功している場合はOK
                this.DialogResult = DialogResult.OK;
            else
                //成功していない場合はキャンセル
                this.DialogResult = DialogResult.Cancel;

            //ここでCloseを書いても閉じてくれないので、メッセージで処理する
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.MaintenanceStartCompleted);
        }

        /// <summary>
        /// モーター初期化完了処理（モジュール）
        /// </summary>
        /// <param name="value">モジュールインデックス</param>
        protected void onMaintenanceStartCompleted(Object value)
        {
            this.Close();
        }
        #endregion
    }

}
