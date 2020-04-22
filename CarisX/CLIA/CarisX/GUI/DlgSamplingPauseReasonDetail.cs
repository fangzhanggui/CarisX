using System;
using System.Data;
using Oelco.CarisX.Utility;
using Infragistics.Win.UltraWinListView;
using System.Linq;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// サンプリング停止理由詳細ダイアログクラス
    /// </summary>
    public partial class DlgSamplingPauseReasonDetail : DlgCarisXBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="remark"></param>
        private DlgSamplingPauseReasonDetail(Int32 moduleId, SamplingPauseReason rackpausereason, SamplingPauseReason slavepausereason)
        {
            InitializeComponent();

            //ラックの理由を追加
            if (rackpausereason != null && rackpausereason.GetSamplingStopReasonNameStrings().Length != 0)
            {
                this.lvwSamplingPauseReasonDetail.Items.AddRange(rackpausereason.GetSamplingStopReasonNameStrings().Select(name => new UltraListViewItem(name)
                {
                    Key = Properties.Resources.STRING_DLG_SAMPLINGSTOPREASONDETAIL_001 + name,
                    Value = Properties.Resources.STRING_DLG_SAMPLINGSTOPREASONDETAIL_001 + name
                }).ToArray());
            }

            //モジュールの理由を追加
            if (slavepausereason != null && slavepausereason.GetSamplingStopReasonNameStrings().Length != 0)
            {
                this.lvwSamplingPauseReasonDetail.Items.AddRange(slavepausereason.GetSamplingStopReasonNameStrings().Select(name => new UltraListViewItem(name)
                {
                    Key = String.Format(Properties.Resources.STRING_DLG_SAMPLINGSTOPREASONDETAIL_002, moduleId) + name,
                    Value = String.Format(Properties.Resources.STRING_DLG_SAMPLINGSTOPREASONDETAIL_002, moduleId) + name
                }).ToArray());
            }
        }

        /// <summary>
        /// 画面表示
        /// </summary>
        /// <remarks>
        /// 画面表示します
        /// </remarks>
        /// <param name="remark"></param>
        public static void Show(Int32 moduleId, SamplingPauseReason rackpausereason, SamplingPauseReason slavepausereason)
        {
            if ((slavepausereason != null && slavepausereason != SamplingPauseReason.SAMPLINGPAUSEREASON_DEFAULT && slavepausereason.GetSamplingStopReasonNameStrings().Length != 0)
                || (rackpausereason != null && rackpausereason != SamplingPauseReason.SAMPLINGPAUSEREASON_DEFAULT && rackpausereason.GetSamplingStopReasonNameStrings().Length != 0))
            {
                // RemarkのEnum値に定義されていない値が渡されたときは、Remark == 0 と同様の扱い（エラーなし）とする

                new DlgSamplingPauseReasonDetail(moduleId, rackpausereason, slavepausereason).ShowDialog();
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
            this.Caption = Properties.Resources.STRING_DLG_SAMPLINGSTOPREASONDETAIL_000;

            // ボタン
            this.btnConfirm.Text = Properties.Resources.STRING_COMMON_004;
        }

        /// <summary>
        /// 確認ボタン押下イベント
        /// </summary>
        /// <remarks>
        /// 画面終了します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
