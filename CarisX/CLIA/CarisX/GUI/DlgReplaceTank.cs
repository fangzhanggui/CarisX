using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.CarisX.Const;
using Oelco.Common.Utility;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Parameter;
using Oelco.Common.Parameter;
using Oelco.CarisX.Common;
using Oelco.CarisX.Comm;
using Oelco.Common.Comm;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// タンク入れ替えダイアログクラス
    /// </summary>
    public partial class DlgReplaceTank : DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgReplaceTank()
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
            // 洗浄液タンク交換表示設定
            this.chkWashsolutionTank.Visible = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashSolutionFromExterior.Enable;
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_REPLACETANK_000;

            this.chkWasteTank.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_REPLACETANK_001;
            this.chkWashsolutionTank.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_REPLACETANK_002;

            // ボタン
            this.btnStart.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_REPLACETANK_003;
            this.btnComplete.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_REPLACETANK_004;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// Startボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 画面のチェック状態に合わせてコマンドを送信する
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            //チェックされていない場合はメッセージを出して処理を抜ける
            if (!(chkWasteTank.Checked | chkWashsolutionTank.Checked))
            {
                DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_174, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.Confirm);
                return;
            }

            //画面を操作不能にする
            this.Enabled = false;

            Boolean[] SkipList = new Boolean[] { true, true, true, true };
            CarisXSequenceHelper.SequenceSyncObject[] syncDataWasteList = new CarisXSequenceHelper.SequenceSyncObject[Enum.GetValues(typeof(ModuleIndex)).Length];
            CarisXSequenceHelper.SequenceSyncObject[] syncDataWashSolutionList = new CarisXSequenceHelper.SequenceSyncObject[Enum.GetValues(typeof(ModuleIndex)).Length];
            foreach (Int32 moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                syncDataWasteList[moduleindex] = new CarisXSequenceHelper.SequenceSyncObject();
                syncDataWashSolutionList[moduleindex] = new CarisXSequenceHelper.SequenceSyncObject();
            }

            //全モジュールに対してコマンドを送信する
            foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                //モジュールが接続されているか
                if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                {
                    SkipList[moduleindex] = false;

                    //廃液タンクを交換する場合
                    if (chkWasteTank.Checked)
                    {
                        syncDataWasteList[moduleindex] = Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleindex].StartReplaceWasteTankSequence();
                    }

                    //洗浄液タンクを交換する場合
                    if (chkWashsolutionTank.Checked)
                    {
                        syncDataWashSolutionList[moduleindex] = Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleindex].StartReplaceWashsolutionTankSequence();
                    }
                }
            }

            //送信対象となったそれぞれのシーケンスが完了出来ている事を順番にチェックする。
            foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                if (!SkipList[moduleindex])
                {
                    //廃液タンクを交換する場合
                    if (chkWasteTank.Checked)
                    {
                        //送信対象になっている場合
                        while (!syncDataWasteList[moduleindex].EndSequence.WaitOne(10))
                        {
                            Application.DoEvents();
                        }
                    }

                    //洗浄液タンクを交換する場合
                    if (chkWashsolutionTank.Checked)
                    {
                        //送信対象になっている場合
                        while (!syncDataWashSolutionList[moduleindex].EndSequence.WaitOne(10))
                        {
                            Application.DoEvents();
                        }
                    }
                }
            }

            //廃液タンクor洗浄液タンクのどちらかにチェックが入っているか
            if (chkWasteTank.Checked | chkWashsolutionTank.Checked)
            {
                //ボタンを無効化
                btnStart.Enabled = false;
                btnCancel.Enabled = false;
                //Completeボタンを有効化
                btnComplete.Enabled = true;
            }

            this.Enabled = true;
        }

        /// <summary>
        /// Completeボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 画面のチェック状態に合わせてコマンドを送信する
        /// ダイアログ結果にOKを設定して画面を終了する
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnComplete_Click(object sender, EventArgs e)
        {
            Boolean[] SkipList = new Boolean[] { true, true, true, true };
            CarisXSequenceHelper.SequenceSyncObject[] syncDataWasteList = new CarisXSequenceHelper.SequenceSyncObject[Enum.GetValues(typeof(ModuleIndex)).Length];
            CarisXSequenceHelper.SequenceSyncObject[] syncDataWashSolutionList = new CarisXSequenceHelper.SequenceSyncObject[Enum.GetValues(typeof(ModuleIndex)).Length];
            foreach (Int32 moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                syncDataWasteList[moduleindex] = new CarisXSequenceHelper.SequenceSyncObject();
                syncDataWashSolutionList[moduleindex] = new CarisXSequenceHelper.SequenceSyncObject();
            }

            //全モジュールに対してコマンドを送信する
            foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                //モジュールが接続されているか
                if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                {
                    SkipList[moduleindex] = false;

                    //廃液タンクを交換していた場合
                    if (chkWasteTank.Checked)
                    {
                        syncDataWasteList[moduleindex] = Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleindex].StopReplaceWasteTankSequence();
                    }

                    //洗浄液タンクを交換していた場合
                    if (chkWashsolutionTank.Checked)
                    {
                        syncDataWashSolutionList[moduleindex] = Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleindex].StopReplaceWashsolutionTankSequence();
                    }
                }
            }

            //送信対象となったそれぞれのシーケンスが完了出来ている事を順番にチェックする。
            foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                if (!SkipList[moduleindex])
                {
                    //廃液タンクを交換していた場合
                    if (chkWasteTank.Checked)
                    {
                        //送信対象になっている場合
                        while (!syncDataWasteList[moduleindex].EndSequence.WaitOne(10))
                        {
                            Application.DoEvents();
                        }
                    }

                    //洗浄液タンクを交換していた場合
                    if (chkWashsolutionTank.Checked)
                    {
                        //送信対象になっている場合
                        while (!syncDataWashSolutionList[moduleindex].EndSequence.WaitOne(10))
                        {
                            Application.DoEvents();
                        }
                    }
                }
            }

            //洗浄液タンクを交換していた場合
            if (chkWashsolutionTank.Checked)
            {
                //洗浄液タンクを交換出来たとみなして、残量満タン扱いにする
                Singleton<PublicMemory>.Instance.WashSolutionTankStatus = WashSolutionTankStatusKind.Full;
                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.WashSolutionTankStatus);
            }

            //画面を終了する
            this.DialogResult = DialogResult.OK;
            this.Close();
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
        /// ダイアログ表示イベント
        /// </summary>
        /// <remarks>
        /// テキストボックスにフォーカスを設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void DlgReplaceTank_Shown(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 廃液タンクチェック状態変更前
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkWasteTank_BeforeCheckStateChanged(object sender, CancelEventArgs e)
        {
            if (btnComplete.Enabled)
            {
                //完了ボタンが押下出来る場合は、チェック状態の変更をキャンセルする
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 洗浄液タンクチェック状態変更前
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkWashsolutionTank_BeforeCheckStateChanged(object sender, CancelEventArgs e)
        {
            if (btnComplete.Enabled)
            {
                //完了ボタンが押下出来る場合は、チェック状態の変更をキャンセルする
                e.Cancel = true;
            }
        }

        #endregion
    }
}
