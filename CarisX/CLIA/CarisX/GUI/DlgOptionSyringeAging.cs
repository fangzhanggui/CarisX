using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.CarisX.Utility;
using Oelco.Common.Utility;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.Misc;
using Oelco.CarisX.Maintenance;
using Oelco.CarisX.Comm;
using System.Threading;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 分注器エージングダイアログクラス
    /// </summary>
    public partial class DlgOptionSyringeAging : DlgCarisXBase
    {
        #region [クラス変数定義]
        /// <summary>
        /// システムオブジェクト
        /// </summary>
        CarisXSequenceHelper.SequenceSyncObject syncObj = null;

        /// <summary>
        /// サンプル分注シリンジエージング設定回数
        /// </summary>
        Int32 sampleArgingNum = 0;
        /// <summary>
        /// サンプル分注シリンジエージング回数
        /// </summary>
        Int32 sampleArgingCount = 0;
        /// <summary>
        /// 希釈液分注シリンジエージング設定回数
        /// </summary>
        Int32 diluentDispensingArgingNum = 0;
        /// <summary>
        /// 希釈液分注シリンジエージング回数
        /// </summary>
        Int32 diluentDispensingArgingCount = 0;
        /// <summary>
        /// R1分注シリンジエージング設定回数
        /// </summary>
        Int32 r1ArgingNum = 0;
        /// <summary>
        /// R1分注シリンジエージング回数
        /// </summary>
        Int32 r1ArgingCount = 0;
        /// <summary>
        /// R2分注シリンジエージング設定回数
        /// </summary>
        Int32 r2ArgingNum = 0;
        /// <summary>
        /// R2分注シリンジエージング回数
        /// </summary>
        Int32 r2ArgingCount = 0;
        /// <summary>
        /// BF分注シリンジエージング設定回数
        /// </summary>
        Int32 bfArgingNum = 0;
        /// <summary>
        /// BF分注シリンジエージング回数
        /// </summary>
        Int32 bfArgingCount = 0;
        /// <summary>
        /// プレトリガ分注シリンジエージング設定回数
        /// </summary>
        Int32 pretriggerNum = 0;
        /// <summary>
        /// プレトリガ分注シリンジエージング回数
        /// </summary>
        Int32 pretriggerCount = 0;
        /// <summary>
        /// トリガ分注エージング設定回数
        /// </summary>
        Int32 triggerNum = 0;
        /// <summary>
        /// トリガ分注エージング回数
        /// </summary>
        Int32 triggerCount = 0;
        /// <summary>
        /// 中断フラグ
        /// </summary>
        Boolean isStop = false;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgOptionSyringeAging()
        {
            InitializeComponent();
        }
    
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// エージングシリンジ処理
        /// </summary>
        /// <remarks>
        /// エージングシリンジ処理を行います
        /// </remarks>
        public void StartSyringeAging()
        {
            this.isStop = false;
            Boolean isExecute = true;
            while (isExecute == true)
            {
                isExecute = false;      // コマンド送信処理を行えば、trueにする
                // チェックしている箇所を取得し、コマンド送信
                // サンプル分注シリング
                isExecute |= SendAgingCommand((Int32)MotorNoList.SampleDispenseSyringe,
                                                this.chkSample,
                                                this.lblSample,
                                                this.sampleArgingNum,
                                                ref this.sampleArgingCount
                                );
                // R1分注シリンジ
                isExecute |= SendAgingCommand((Int32)MotorNoList.R1DispenseSyringe,
                                                this.chkR1,
                                                this.lblR1,
                                                this.r1ArgingNum,
                                                ref this.r1ArgingCount
                                );
                // R2分注シリンジ
                isExecute |= SendAgingCommand((Int32)MotorNoList.R2DispenseSyringe,
                                                this.chkR2,
                                                this.lblR2,
                                                this.r2ArgingNum,
                                                ref this.r2ArgingCount
                                );
                // 希釈液分注シリンジ
                isExecute |= SendAgingCommand((Int32)MotorNoList.DiluentDispenseSyringe,
                                                this.chkDiluent,
                                                this.lblDiluent,
                                                this.diluentDispensingArgingNum,
                                                ref this.diluentDispensingArgingCount
                                );
                // 洗浄液シリンジ
                isExecute |= SendAgingCommand((Int32)MotorNoList.BFWashSyringe,
                                                this.chkBF,
                                                this.lblBF,
                                                this.bfArgingNum,
                                                ref this.bfArgingCount
                                );
                // プレトリガ分注シリンジ
                isExecute |= SendAgingCommand((Int32)MotorNoList.PreTriggerDispenseSyringe,
                                                this.chkPretrigger,
                                                this.lblPretrigger,
                                                this.pretriggerNum,
                                                ref this.pretriggerCount
                                );
                // トリガ分注シリンジ
                isExecute |= SendAgingCommand((Int32)MotorNoList.TriggerDispenseSyringe,
                                                this.chkTrigger,
                                                this.lblTrigger,
                                                this.triggerNum,
                                                ref this.triggerCount
                                );
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
            SetText(this.lblSample, this.sampleArgingCount.ToString());
            SetText(this.lblR1, this.r1ArgingCount.ToString());
            SetText(this.lblR2, this.r2ArgingCount.ToString());
            SetText(this.lblDiluent, this.diluentDispensingArgingCount.ToString());
            SetText(this.lblBF, this.bfArgingCount.ToString());
            SetText(this.lblPretrigger, this.pretriggerCount.ToString());
            SetText(this.lblTrigger, this.triggerCount.ToString());
        }

        /// <summary>
        /// コンポーネントの初期化
        /// </summary>
        /// <remarks>
        /// コンポーネントを初期化します
        /// </remarks>
        protected override void initializeFormComponent()
        {
            btnStop.Enabled = false;
            this.numSample.Enabled = false;
            this.numR1.Enabled = false;
            this.numR2.Enabled = false;
            this.numDiluent.Enabled = false;
            this.numBF.Enabled = false;
            this.numPretrigger.Enabled = false;
            this.numTrigger.Enabled = false;

            // 通知イベント登録
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.SyringeAgingEnd,
                                                                this.SyringeAgingEnd);
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
            this.btnOk.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_007;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_002;
            this.btnStop.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_009;

            // ダイアログタイトル
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIOINSYRINGEAGING_000;

            // 項目
            // 分注器名
            this.lblName.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIOINSYRINGEAGING_001;
            // 設定回数
            this.lblSetNumber.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIOINSYRINGEAGING_002;
            // エージング回数
            this.lblNumberOfArging.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIOINSYRINGEAGING_003;

            // サンプル分注シリンジ
            this.chkSample.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIOINSYRINGEAGING_004;
            // R1分注シリンジ
            this.chkR1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIOINSYRINGEAGING_005;
            // R2分注シリンジ
            this.chkR2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIOINSYRINGEAGING_006;
            // 希釈液分注シリンジ
            this.chkDiluent.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIOINSYRINGEAGING_008;
            // B/F1シリンジ
            this.chkBF.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIOINSYRINGEAGING_009;
            // プレトリガ分注シリンジ
            this.chkPretrigger.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIOINSYRINGEAGING_011;
            // トリガ分注シリンジ
            this.chkTrigger.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIOINSYRINGEAGING_012;
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// OKボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// エージングシリンジ処理を開始します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnOk_Click( object sender, EventArgs e )
        {
            this.btnStop.Enabled = true;
            // 操作可能な部品を全て操作不可にする
            AllUnitState(false);
            // 設定回数取得、回数クリア
            StartAging();

            SyringeAgingWorker workerObject = new SyringeAgingWorker();
            workerObject.InitialSetting(this);
            Thread workerThread = new Thread(workerObject.DoWork);
            workerThread.Start();
        }

        /// <summary>
        /// Cancelボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にキャンセルを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnCancel_Click( object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// コマンド送信
        /// </summary>
        /// <remarks>
        /// エージングコマンド送信を行います
        /// </remarks>
        /// <param name="nMotorNo">モーター番号</param>
        /// <param name="checkEditor">チェックボックス</param>
        /// <param name="lblCount">エージング回数ラベル</param>
        /// <param name="nArgingNum">エージング設定回数</param>
        /// <param name="nArgingCount">エージング回数</param>
        /// <returns>結果</returns>
        private Boolean SendAgingCommand(   Int32 nMotorNo,
                                            UltraCheckEditor checkEditor,
                                            UltraLabel lblCount,
                                            Int32 nArgingNum,
                                            ref Int32 nArgingCount
                                        )
        {
            Boolean isSucces = false;
            if (isStop == true)
            {
                // 中断フラグが真なら処理をしない
                return isSucces;
            }
            if ( ( checkEditor.Checked == true ) && ( nArgingNum > nArgingCount ) )
            {
                // 0483のコマンド送信
                this.syncObj = Singleton<CarisXSequenceHelperManager>.Instance.Slave[(int)Singleton<Common.PublicMemory>.Instance.moduleIndex].SyringeAgingSequence(nMotorNo);
                this.syncObj.EndSequence.WaitOne(); // 応答待ち
                if (this.syncObj.Status == CarisXSequenceHelper.SequenceSyncObject.SequenceStatus.Success)
                {
                    SlaveCommCommand_1483 temp = (SlaveCommCommand_1483)this.syncObj.SequenceResultData;
                }
                nArgingCount++;
                SetText(lblCount, nArgingCount.ToString());
                isSucces = true;
                this.syncObj = null;
            }
            return isSucces;
        }

        /// <summary>
        /// エージング開始処理
        /// </summary>
        /// <remarks>
        /// エージング処理を開始します
        /// </remarks>
        private void StartAging()
        {
            // エージング設定回数を取得
            this.sampleArgingNum = (Int32)this.numSample.Value;
            this.r1ArgingNum = (Int32)this.numR1.Value;
            this.r2ArgingNum = (Int32)this.numR2.Value;
            this.diluentDispensingArgingNum = (Int32)this.numDiluent.Value;
            this.bfArgingNum = (Int32)this.numBF.Value;
            this.pretriggerNum = (Int32)this.numPretrigger.Value;
            this.triggerNum = (Int32)this.numTrigger.Value;
            // 回数初期化
            this.sampleArgingCount = 0;
            this.r1ArgingCount = 0;
            this.r2ArgingCount = 0;
            this.diluentDispensingArgingCount = 0;
            this.bfArgingCount = 0;
            this.pretriggerCount = 0;
            this.triggerCount = 0;
            // 回数(表示)をクリア
            SetText(this.lblSample, this.sampleArgingCount.ToString());
            SetText(this.lblR1, this.r1ArgingCount.ToString());
            SetText(this.lblR2, this.r2ArgingCount.ToString());
            SetText(this.lblDiluent, this.diluentDispensingArgingCount.ToString());
            SetText(this.lblBF, this.bfArgingCount.ToString());
            SetText(this.lblPretrigger, this.pretriggerCount.ToString());
            SetText(this.lblTrigger, this.triggerCount.ToString());
        }

        /// <summary>
        /// テキスト設定
        /// </summary>
        /// <remarks>
        /// テキストを設定します
        /// </remarks>
        /// <param name="lbl"></param>
        /// <param name="text"></param>
        private void SetText(UltraLabel lbl, string text)
        {
            if (lbl.InvokeRequired == true)
            {
                // 別スレッドからの書き込み
                SetLabelText setLblText = new SetLabelText(SetText);
                this.Invoke(setLblText, new object[] { lbl, text });
            }
            else
            {
                // 同スレッドからの書き込み
                lbl.Text = text;
            }
        }

        /// <summary>
        /// エディットボックスイネーブル設定
        /// </summary>
        /// <remarks>
        /// エディットボックスの有効、無効を設定します
        /// </remarks>
        /// <param name="checkEditor">チェックボックス</param>
        /// <param name="numEditor">エディットボックス</param>
        private void EditStateChange(UltraCheckEditor checkEditor, UltraNumericEditor numEditor)
        {
            if (checkEditor.Checked == true)
            {
                numEditor.Enabled = true;
            }
            else
            {
                numEditor.Enabled = false;
            }
        }
        /// <summary>
        /// 操作部品の操作可否を一括変更(停止ボタンは除く)
        /// </summary>
        /// <remarks>
        /// 操作部品の操作可否を一括設定します
        /// </remarks>
        /// <param name="isEnable">操作可否</param>
        private void AllUnitState( Boolean isEnable )
        {
            // ボタンの操作可否を変更
            this.btnOk.Enabled = isEnable;
            this.btnCancel.Enabled = isEnable;
            // チェックボックスの操作可否を変更
            this.chkSample.Enabled = isEnable;
            this.chkR1.Enabled = isEnable;
            this.chkR2.Enabled = isEnable;
            this.chkDiluent.Enabled = isEnable;
            this.chkBF.Enabled = isEnable;
            this.chkPretrigger.Enabled = isEnable;
            this.chkTrigger.Enabled = isEnable;
            // 設定回数の入力可否を変更
            this.numSample.Enabled = isEnable;
            this.numR1.Enabled = isEnable;
            this.numR2.Enabled = isEnable;
            this.numDiluent.Enabled = isEnable;
            this.numBF.Enabled = isEnable;
            this.numPretrigger.Enabled = isEnable;
            this.numTrigger.Enabled = isEnable;           
        }

        /// <summary>
        /// 中断ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 中断フラグを設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (this.syncObj != null)
            {
                this.syncObj.EndSequence.Set();
            }
            // 中断フラグを真にする
            this.isStop = true;
            this.btnStop.Enabled = false;
        }
 
        /// <summary>
        /// サンプル分注シリンジチェックボックスクリックイベント
        /// </summary>
        /// <remarks>
        /// サンプル分注シリンジエディットボックスの有効、無効切り替えします
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void chkSample_CheckedChanged(object sender, EventArgs e)
        {
            EditStateChange(this.chkSample, this.numSample);
        }

        /// <summary>
        /// 希釈液分注シリンジチェックボックスクリックイベント
        /// </summary>
        /// <remarks>
        /// 希釈液分注シリンジエディットボックスの有効、無効切り替えします
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void chkDilution_CheckedChanged(object sender, EventArgs e)
        {
            EditStateChange(this.chkDiluent, this.numDiluent);
        }

        /// <summary>
        /// R1分注シリンジチェックボックスクリックイベント
        /// </summary>
        /// <remarks>
        /// R1分注シリンジエディットボックスの有効、無効切り替えします
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void chkR1_CheckedChanged(object sender, EventArgs e)
        {
            EditStateChange(this.chkR1, this.numR1);
        }

        /// <summary>
        /// R2分注シリンジチェックボックスクリックイベント
        /// </summary>
        /// <remarks>
        /// R2分注シリンジエディットボックスの有効、無効切り替えします
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void chkR2_CheckedChanged(object sender, EventArgs e)
        {
            EditStateChange(this.chkR2, this.numR2);
        }

        /// <summary>
        /// B/F1シリンジチェックボックスクリックイベント
        /// </summary>
        /// <remarks>
        /// B/F1シリンジエディットボックスの有効、無効切り替えします
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void chkBF1_CheckedChanged(object sender, EventArgs e)
        {
            EditStateChange(this.chkBF, this.numBF);
        }

        /// <summary>
        /// プレトリガ分注シリンジチェックボックスクリックイベント
        /// </summary>
        /// <remarks>
        /// プレトリガ分注シリンジエディットボックスの有効、無効切り替えします
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void chkPretrigger_CheckedChanged(object sender, EventArgs e)
        {
            EditStateChange(this.chkPretrigger, this.numPretrigger);
        }

        /// <summary>
        /// トリガ分注シリンジチェックボックスクリックイベント
        /// </summary>
        /// <remarks>
        /// トリガ分注シリンジエディットボックスの有効、無効切り替えします
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void chkTrigger_CheckedChanged(object sender, EventArgs e)
        {
            EditStateChange(this.chkTrigger, this.numTrigger);
        }

        /// <summary>
        /// エージングシリンジ終了
        /// </summary>
        /// <remarks>
        /// エージングシリンジ終了処理を行います
        /// </remarks>
        /// <param name="value"></param>
        private void SyringeAgingEnd(Object value)
        {
            this.btnStop.Enabled = false;

            // 操作可能な部品を全て操作可にする
            AllUnitState(true);
            // 設定回数の操作可否はチェックボックスによるので再設定
            EditStateChange(this.chkSample, this.numSample);
            EditStateChange(this.chkR1, this.numR1);
            EditStateChange(this.chkR2, this.numR2);
            EditStateChange(this.chkDiluent, this.numDiluent);
            EditStateChange(this.chkBF, this.numBF);
            EditStateChange(this.chkPretrigger, this.numPretrigger);
            EditStateChange(this.chkTrigger, this.numTrigger);
        }

        /// <summary>
        /// 画面クローズイベント
        /// </summary>
        /// <remarks>
        /// 画面終了処理を行います
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DlgOptionSyringeAging_FormClosed(object sender, FormClosedEventArgs e)
        {
            Singleton<NotifyManager>.Instance.RemoveNotifyTarget((Int32)NotifyKind.SyringeAgingEnd,
                                                                this.SyringeAgingEnd);
        }

        #endregion
        /// <summary>
        /// ラベルテキスト設定デリゲート
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="text"></param>
        delegate void SetLabelText(UltraLabel lbl, string text);
    }

    // TODO: 余裕があれば、CarisXSequenceHelper.csで処理するように変更する
    public class SyringeAgingWorker
    {
        /// <summary>
        /// 実行処理
        /// </summary>
        /// <remarks>
        /// 実行処理を行います
        /// </remarks>
        public void DoWork()
        {
            if (this.dlgAging != null)
            {
                this.dlgAging.StartSyringeAging();
            }
            this.dlgAging = null;
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SyringeAgingEnd, null);
        }

        /// <summary>
        /// 初期設定処理
        /// </summary>
        /// <remarks>
        /// 初期設定処理を行います
        /// </remarks>
        public void InitialSetting(DlgOptionSyringeAging dlgSyringeAging)
        {
            this.dlgAging = dlgSyringeAging;
        }

        /// <summary>
        /// エージングシリンジ画面オブジェクト
        /// </summary>
        private DlgOptionSyringeAging dlgAging = null;
    }
}
