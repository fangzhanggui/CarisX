using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinListView;
using Oelco.Common.GUI;
using Oelco.CarisX.Const;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Parameter;
using Oelco.Common.Utility;

namespace Oelco.CarisX.GUI.Controls
{
    public partial class SearchLogInfoPanelBase : UserControl , ISearchLogInfo
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// OKボタンクリックイベントハンドラ
        /// </summary>
        public event EventHandler OkClick;

        /// <summary>
        /// Cancelボタンクリックイベントハンドラ
        /// </summary>
        public event EventHandler CancelClick;

        /// <summary>
        /// Closeボタンクリックイベントハンドラ
        /// </summary>
        public event EventHandler CloseClick;
        #endregion

        #region [コンストラクタ/デストラクタ]

        public SearchLogInfoPanelBase()
        {
            InitializeComponent();

            // 履歴書き込み時刻の初期化
            this.btnWriteTimeFrom.Text = DateTime.Today.ToShortDateString();
            this.btnWriteTimeFrom.Tag = DateTime.Today;
            this.btnWriteTimeTo.Text = DateTime.Today.ToShortDateString();
            this.btnWriteTimeTo.Tag = DateTime.Today.Add(TimeSpan.FromDays(1) - TimeSpan.FromSeconds(1));

            // パネル既定ボタン
            this.btnOk.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;

            // 各種チェックボックス
            this.chkUserID.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_023;
            this.chkWriteTime.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_024;

            // 各ハイフン
            this.lblHyphen1.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 選択中の履歴書き込み時刻の取得、設定
        /// </summary>
        // プロパティウインドウに表示しない
        [BrowsableAttribute(false)]
        Tuple<bool, DateTime, DateTime> ISearchLogInfo.WriteTimeSelect
        {
            get
            {
                // 履歴書き込み時刻ボタンチェック時
                if (this.chkWriteTime.Checked)
                {
                    // 履歴書き込み時刻ボタンのチェック状態と選択時刻を返す
                    return new Tuple<Boolean, DateTime, DateTime>(this.chkWriteTime.Checked, (DateTime)this.btnWriteTimeFrom.Tag, (DateTime)this.btnWriteTimeTo.Tag);
                }
                else
                {
                    // 履歴書き込み時刻ボタンのチェック状態と時刻の最小値を返す
                    return new Tuple<Boolean, DateTime, DateTime>(this.chkWriteTime.Checked, DateTime.MinValue, DateTime.MinValue);
                }
            }
            set
            {
                // 各種値を入れる
                this.chkWriteTime.Checked = value.Item1;
                this.btnWriteTimeFrom.Tag = value.Item2;
                this.btnWriteTimeFrom.Text = value.Item2.ToShortDateString();
                this.btnWriteTimeTo.Tag = value.Item3;
                this.btnWriteTimeTo.Text = value.Item3.ToShortDateString();
            }
        }

        /// <summary>
        /// ユーザーIDの取得、設定
        /// </summary>
        // プロパティウインドウに表示しない
        [BrowsableAttribute(false)]
        Tuple<bool, string> ISearchLogInfo.UserIDSelect
        {
            get
            {
                // ユーザーIDボタンチェック時
                if (this.chkUserID.Checked)
                {
                    // ユーザーIDボタンのチェック状態と入力されたユーザーIDを返す
                    return new Tuple<Boolean, String>(this.chkUserID.Checked, (String)this.txtUserID.Text);
                }
                else
                {
                    // ユーザーIDボタンのチェック状態と空の文字列を返す
                    return new Tuple<Boolean, String>(this.chkUserID.Checked, String.Empty);
                }
            }
            set
            {
                // 各種値を入れる
                this.chkWriteTime.Checked = value.Item1;
                this.txtUserID.Text = value.Item2;
            }
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// OKボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// OKボタンクリックイベントを実行します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click( object sender, EventArgs e )
        {
            if (this.OkClick != null)
            {
                this.OkClick(sender, e);
            }
        }

        /// <summary>
        /// キャンセルボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// キャンセルボタンクリックイベントを実行します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnCancel_Click( object sender, EventArgs e )
        {
            if (this.CancelClick != null)
            {
                this.CancelClick(sender, e);
            }
        }

        /// <summary>
        /// 閉じるボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 閉じるボタンクリックイベントを実行します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnClose_Click( object sender, EventArgs e )
        {
            if (this.CloseClick != null)
            {
                this.CloseClick(sender, e);
            }
        }

        /// <summary>
        /// 日付(開始日)ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 日付選択ダイアログを表示し日付(開始日)を設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnWriteTimeFrom_Click( object sender, EventArgs e )
        {
            // 日付選択ダイアログの呼び出し
            DateTime date;
            DialogResult result = DlgDateSelect.Show(String.Empty, out date, (DateTime)this.btnWriteTimeFrom.Tag);
            if (DialogResult.OK == result)
            {
                this.btnWriteTimeFrom.Text = date.ToShortDateString();
                this.btnWriteTimeFrom.Tag = date;
            }
        }

        /// <summary>
        /// 日付(終了日)ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 日付選択ダイアログを表示し日付(終了日)を設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnWriteTimeTo_Click( object sender, EventArgs e )
        {
            // 日付選択ダイアログの呼び出し
            DateTime date;
            DialogResult result = DlgDateSelect.Show(String.Empty, out date, (DateTime)this.btnWriteTimeTo.Tag);
            if (DialogResult.OK == result)
            {
                date = date.Add(TimeSpan.FromDays(1) - TimeSpan.FromSeconds(1));
                this.btnWriteTimeTo.Text = date.ToShortDateString();
                this.btnWriteTimeTo.Tag = date;
            }
        }

        #endregion
    }
}
