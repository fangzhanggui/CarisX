using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.CarisX.Comm;
using Oelco.Common.Utility;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Status;
using Oelco.CarisX.DB;
using Oelco.CarisX.Const;
using Oelco.CarisX.Common;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// システム初期化ダイアログクラス
    /// </summary>
    public partial class DlgOptionSystemInitializing : DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgOptionSystemInitializing()
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
            this.optEditMode.CheckedIndex = 0;
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONSYSTEMINITIALIZEING_000;

            // グループボックスタイトル
            this.ultraGroupBox1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONSYSTEMINITIALIZEING_001;
            // ラジオボタン
            this.optEditMode.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONSYSTEMINITIALIZEING_002;
            this.optEditMode.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONSYSTEMINITIALIZEING_003;
            // 注意文
            this.lblCaution.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONSYSTEMINITIALIZEING_004;        
        }

        #endregion

        #region [privateメソッド]
        /// <summary>
        /// OKボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 編集モードにより初期化画面を表示するか、各画面の初期化処理を実行し
        /// ダイアログ結果にOKを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnOk_Click( object sender, EventArgs e )
        {
            if (this.optEditMode.CheckedIndex == 0)
            {
                foreach (Int32 moduleIndex in Enum.GetValues(typeof(ModuleIndex)))
                {
                    if (Singleton<CarisXSequenceHelperManager>.Instance.Slave.ContainsKey(moduleIndex))
                    {
                        Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleIndex].InitializeSequenceModule(
                            InitializeSequencePattern.Module | InitializeSequencePattern.StartsBeforeUser);
                    }
                }
                Singleton<CarisXSequenceHelperManager>.Instance.RackTransfer.InitializeSequenceRackTransfer(
                    InitializeSequencePattern.RackTransfer|InitializeSequencePattern.StartsBeforeUser);
            }
            else
            {
                this.btnOk.Enabled = false;
                this.btnCancel.Enabled = false;

                // 分析状態の初期化
                // 分析ステータス画面を初期化する
                // 分析DB全消去
                Singleton<SpecimenAssayDB>.Instance.ClearAssayData();
                Singleton<SpecimenAssayDB>.Instance.CommitData();

                Singleton<CalibratorAssayDB>.Instance.ClearAssayData();
                Singleton<CalibratorAssayDB>.Instance.CommitData();

                Singleton<ControlAssayDB>.Instance.ClearAssayData();
                Singleton<ControlAssayDB>.Instance.CommitData();

                // 検体登録DB全消去
                Singleton<SpecimenGeneralDB>.Instance.DeleteAll();
                Singleton<SpecimenGeneralDB>.Instance.CommitSampleInfo();

                // STAT登録DB一時登録データ消去
                Singleton<SpecimenStatDB>.Instance.Delete(RegistType.Temporary);
                Singleton<SpecimenStatDB>.Instance.CommitSampleInfo();

                // シーケンス番号初期化
                Singleton<SequencialSampleNo>.Instance.ResetNumber();
                Singleton<SequencialPrioritySampleNo>.Instance.ResetNumber();
                Singleton<SequencialCalibNo>.Instance.ResetNumber();
                Singleton<SequencialControlNo>.Instance.ResetNumber();
                Singleton<ReceiptNo>.Instance.ResetNumber();

                // 各画面の検索関連UIの初期表示日付を更新
                // 表示更新
                RealtimeDataAgent.LoadAssayData();
                RealtimeDataAgent.LoadSampleData();
                RealtimeDataAgent.LoadStatData();

                this.btnOk.Enabled = true;
                this.btnCancel.Enabled = true;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
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
        private void btnCancel_Click( object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 画面クローズイベント
        /// </summary>
        /// <remarks>
        /// 画面を終了します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DlgOptionSystemInitializing_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        #endregion
    }
}
