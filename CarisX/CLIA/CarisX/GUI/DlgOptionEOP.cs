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
using Oelco.CarisX.Utility;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Const;
using Oelco.CarisX.Common;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// EOP処理ダイアログクラス
    /// </summary>
    public partial class DlgOptionEOP : DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgOptionEOP()
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
            this.btnOk.Enabled = true;
            this.btnCancel.Enabled = true;
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
            this.btnOk.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;

            // ダイアログタイトル
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONEOP_000;

            // 終了処理確認文
            this.ultraLabel1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONEOP_001;
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// OKボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にOKを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            // ボタンを操作不可にする。
            this.btnOk.Enabled = false;
            this.btnCancel.Enabled = false;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            //メッセージを変更
            this.ultraLabel1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONEOP_002;

            // ファイルからデータ読み込み
            Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance.Load();
            CarisXSequenceHelper.SequenceSyncObject syncData = Singleton<CarisXSequenceHelperManager>.Instance.Slave[(Int32)Singleton<PublicMemory>.Instance.moduleIndex].AskReagentRemain();
            syncData.OnEndSequence += this.ReagentRemain; // 終了時イベント登録
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
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// リンス液残量
        /// </summary>
        /// <remarks>
        /// リンス液残量を設定します
        /// </remarks>
        /// <param name="value"></param>
        private void ReagentRemain(CarisXSequenceHelper.SequenceSyncObject obj)
        {
            obj.OnEndSequence -= this.ReagentRemain; // イベント登録の解除

            Int32 moduleIndex = (Int32)Singleton<PublicMemory>.Instance.moduleIndex;

            CarisXSensorParameter param = Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance.Param;
            if (obj.Status == CarisXSequenceHelper.SequenceSyncObject.SequenceStatus.Success)
            {
                SlaveCommCommand_1414 remainData = (SlaveCommCommand_1414)obj.SequenceResultData;

                // リンス液のセンサーチェック
                // →センサー使用しているなら、残量チェック
                if (param.SlaveList[moduleIndex].sensorParameterUseNoUse.UsablePurifiedWater == (byte)UseStatus.Use)
                {
                    if (remainData.RinceContainerRemain == 0)
                    {
                        // リンス液なし

                        // メッセージ表示 リソースから読み込むようにすること
                        DlgMessage.Show(String.Empty, Properties.Resources.STRING_DLG_MSG_172,
                            CarisX.Properties.Resources.STRING_DLG_TITLE_004, MessageDialogButtons.OK);

                        if (this.InvokeRequired)
                        {
                            this.Invoke((Action)((() => { this.Close(); })));
                        }
                        return;
                    }
                }
            }
            // TODO: ボタンの更新処理?  (リンス)
            // リンス処理コマンド送信(0010)
            CarisXSequenceHelper.SequenceSyncObject syncObj = Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleIndex].RinsingSequence();
            syncObj.OnEndSequence += this.RinsingSequence;
        }

        /// <summary>
        /// リンス処理
        /// </summary>
        /// <remarks>
        /// リンス液残量を設定します
        /// </remarks>
        /// <param name="value"></param>
        private void RinsingSequence(CarisXSequenceHelper.SequenceSyncObject obj)
        {
            obj.OnEndSequence -= this.RinsingSequence; // 通知解除
            // TODO: ボタンの更新処理?  (wait)

            if (this.InvokeRequired)
            {
                this.Invoke((Action)((() => { this.Close(); })));
            }
        }
        #endregion
    }
}
