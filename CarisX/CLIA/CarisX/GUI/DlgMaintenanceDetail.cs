using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.CarisX.Parameter.MaintenanceJournalCodeData;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// メンテナンス日誌詳細ダイアログクラス
    /// </summary>
    public partial class DlgMaintenanceDetail : DlgCarisXBase
    {
        #region [変数]
        /// <summary>
        /// ページの進むと戻る判定フラグ
        /// </summary>
        /// <remarks>
        /// 戻る: false
        /// 進む: true
        /// </remarks>
        private Boolean PageAction = false;

        /// <summary>
        /// 開いている詳細ページのメンテナンス日誌番号
        /// </summary>
        private String PageMaintenanceJournalNo = "";

        /// <summary>
        /// 開いている詳細ページの種別
        /// </summary>
        private String PageMaintenanceJournalKind = "";

        /// <summary>
        /// 開いている詳細ページのユニット番号
        /// </summary>
        private String PageMaintenanceJournalUnit = "";

        /// <summary>
        /// コード内の最小引数（最初のページ）
        /// </summary>
        private String PageMinArgument = "";

        /// <summary>
        /// コード内の最小引数（最初のページ）
        /// </summary>
        private String PageMaxArgument = "";

        /// <summary>
        /// コード内の現在引数（現在のページ）
        /// </summary>
        private String PageNowArgument = "";

        /// <summary>
        /// コード内の現在引数
        /// </summary>
        private String PageArgument = "";

        #endregion

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgMaintenanceDetail()
        {
            InitializeComponent();
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// パラメータ表示処理
        /// </summary>
        /// <remarks>
        /// 呼び出し元で指定されたコード、引数から情報を取得し表示する処理。
        /// </remarks>
        /// <param name="code">メンテナンス日誌番号</param>
        /// <param name="kind">メンテナンス日誌種別</param>
        /// <param name="unit">メンテナンス日誌ユニット番号</param>
        public void ShowMaintenanceDetail(String code, String kind, String unit)
        {
            // 取得したコード（メンテナンス日誌番号）
            PageMaintenanceJournalNo = code;
            // 取得した種別
            PageMaintenanceJournalKind = kind;
            // 取得したユニット番号
            PageMaintenanceJournalUnit = unit;

            // メッセージリストの取得
            List<MaintenanceJournalCodeData> maintenanceJournalListDist = MaintenanceJournalDist();

            // ページの最小値 ページを閉じない限り変更しない
            PageMinArgument = "1";
            // ページの最大値 ページを閉じない限り変更しない
            PageMaxArgument = maintenanceJournalListDist.Count.ToString();
            // ページの現在値
            PageNowArgument = "1";
            // 表示するページ引数の設定
            PageArgument = maintenanceJournalListDist[0].Argument;

            // ダブルクリックされた行のCodeにあるArgumentの数をページ右側に設定
            this.lblPage.Text = PageMinArgument + " / " + PageMaxArgument;

            // メンテナンス日誌番号・タイトル・画像パス・メッセージ・ページ・画像ファイルのチェック
            PageInformationSet();

            //進む戻るボタン活性・非活性処理
            BtnChange();

            this.ShowDialog();
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            // タイトル
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALDETAIL_000;

            // ラベル
            this.lblMaintenanceJournalNo.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALDETAIL_003;
            this.lblMaintenanceJournalExplanationTitle.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALDETAIL_002;
            this.lblMaintenanceJournalPictureTitle.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALDETAIL_001;

            // ボタン
            this.btnClose.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_002;
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// メッセージリスト重複削除
        /// </summary>
        /// <remarks>
        /// メッセージリストから現在のメンテナンス日誌番号のみのメッセージリストを作成します。
        /// </remarks>
        private void PageInformationSet()
        {
            // ダブルクリックしたコードと引数で値を取得する
            MaintenanceJournalCodeData maintenanceJournalData = Singleton<ParameterFilePreserve<MaintenanceJournalCodeDataManager>>.Instance.Param.GetCodeData(PageMaintenanceJournalNo, PageArgument, PageMaintenanceJournalKind, PageMaintenanceJournalUnit);

            // メンテナンス日誌番号・タイトル・画像パス・メッセージ・ページ
            this.lblMaintenanceJournalCodeNo.Text = PageMaintenanceJournalNo;
            this.lblMaintenanceJournalContentsItem.Text = maintenanceJournalData.SubTitle;
            this.lblMaintenanceJournalExplanation.Text = maintenanceJournalData.Message;
            String imagePath = maintenanceJournalData.GetFullImagePath();

            // 画像ファイルのチェック
            try
            {
                this.pbxMaintenanceJournalPicture.Image = Image.FromFile(imagePath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
            }
            return ;
        }

        /// <summary>
        /// メッセージリスト重複削除
        /// </summary>
        /// <remarks>
        /// メッセージリストから現在のメンテナンス日誌番号のみのメッセージリストを作成します。
        /// </remarks>
        private List<MaintenanceJournalCodeData> MaintenanceJournalDist()
        {
            // パラメータ表示処理
            Singleton<ParameterFilePreserve<MaintenanceJournalCodeDataManager>>.Instance.LoadRaw();
            // メッセージリストの取得
            List<MaintenanceJournalCodeData> maintenanceJournalList = Singleton<ParameterFilePreserve<MaintenanceJournalCodeDataManager>>.Instance.Param.CodeDataList;

            // コードリストから現在のコードだけのリストを作成
            var searachedGroup = (from v in maintenanceJournalList
                                  where v.Code == PageMaintenanceJournalNo & v.Kind == PageMaintenanceJournalKind & v.Unit == PageMaintenanceJournalUnit
                                  orderby v.Argument ascending
                                  select v);

            // 戻り値の型に変更
            List<MaintenanceJournalCodeData> maintenanceJournalListDist = new List<MaintenanceJournalCodeData>(searachedGroup);
            return maintenanceJournalListDist;
        }

        /// <summary>
        /// ページ移動処理
        /// </summary>
        /// <remarks>
        /// 戻るページ、次へページの処理を実行します。
        /// </remarks>
        private void MovePage()
        {
            // 現在ページの値を変更
            if (PageAction == false)
            {
                PageNowArgument = (int.Parse(PageNowArgument) - 1).ToString();
                PageArgument = (int.Parse(PageArgument) - 1).ToString();
            }
            else
            {
                PageNowArgument = (int.Parse(PageNowArgument) + 1).ToString();
                PageArgument = (int.Parse(PageArgument) + 1).ToString();
            }

            // ダブルクリックされた行のCodeにあるArgumentの数をページ右側に設定
            this.lblPage.Text = PageNowArgument + " / " + PageMaxArgument;

            // メンテナンス日誌番号・タイトル・画像パス・メッセージ・ページ・画像ファイルのチェック
            PageInformationSet();

        }


        /// <summary>
        /// 進む戻るボタンの活性・非活性
        /// </summary>
        /// <remarks>
        /// 進むボタンと戻るボタンを活性・非活性に切り替えます
        /// </remarks>
        private void BtnChange()
        {

            // 2ページ以上ある場合、進むボタンの活性
            if (this.PageMinArgument == this.PageMaxArgument)
            {
                this.btnPagePreview.Enabled = false;
                this.btnPageNext.Enabled = false;
            }
            else
            {
                // ページ遷移処理後のページが1ページ目
                if (int.Parse(PageNowArgument) == int.Parse(PageMinArgument))
                {
                    this.btnPagePreview.Enabled = false;
                    this.btnPageNext.Enabled = true;
                }
                // ページ遷移処理後のページが最終ページ
                else if (int.Parse(PageNowArgument) == int.Parse(PageMaxArgument))
                {
                    this.btnPagePreview.Enabled = true;
                    this.btnPageNext.Enabled = false;
                }
                // ページ遷移処理後のページが途中ページ
                else
                {
                    this.btnPagePreview.Enabled = true;
                    this.btnPageNext.Enabled = true;
                }
            }
        }



        /// <summary>
        /// 閉じる処理
        /// </summary>
        /// <remarks>
        /// メンテナンス日誌詳細画面を閉じる処理
        /// </remarks>
        /// <param name="sender">閉じるボタン</param>
        /// <param name="e">押下イベント</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            // 画面を閉じます
            this.Close();
        }

        /// <summary>
        /// ページ戻る処理
        /// </summary>
        /// <remarks>
        /// メンテナンス日誌詳細ページを戻る処理
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnPageBack_Click(object sender, EventArgs e)
        {
            // フラグを戻るに変更
            this.PageAction = false;
            try
            {
                // ページ遷移処理
                MovePage();

                // 進む戻るボタン活性・非活性処理
                BtnChange();
            }
            catch (Exception ex)
            {
                // ページの遷移に失敗しました。
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format("Page transition failed :{0}", ex.Message));
            }
        }

        /// <summary>
        /// ページ進む処理
        /// </summary>
        /// <remarks>
        /// メンテナンス日誌詳細ページを進む処理
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnPageForward_Click(object sender, EventArgs e)
        {
            // フラグを進むに変更
            this.PageAction = true;
            try
            {
                // ページ遷移処理
                MovePage();

                // 進む戻るボタン活性・非活性処理
                BtnChange();
            }

            catch (Exception ex)
            {
                // ページの遷移に失敗しました。
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format("Page transition failed :{0}", ex.Message));
            }
        }

        #endregion
    }

}
