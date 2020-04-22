using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinSchedule;
using Infragistics.Win;
using Infragistics.Win.UltraWinSchedule.MonthViewMulti;
using System.Globalization;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 日付選択ダイアログクラス
    /// </summary>
    public partial class DlgDateSelect : DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="caption">ダイアログタイトル</param>
        /// <param name="currentDateTime">既定の選択日付</param>
        public DlgDateSelect( String caption, DateTime defaultSelectDateTime )
        {
            InitializeComponent();

            this.Caption = caption;

            this.SelectedDate = DateTime.Today;
            this.cliDateSelect.SelectedDateRanges.Clear();
            this.cliDateSelect.SelectedDateRanges.Add( defaultSelectDateTime );
            this.cliDateSelect.ActivateDay( defaultSelectDateTime );
            this.mthCalendarView.CreationFilter = new MonthHeaderCreationFilter();
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 選択中の日付を取得します。
        /// </summary>
        public DateTime SelectedDate
        {
            get;
            protected set;
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 日付選択ダイアログの表示
        /// </summary>
        /// <remarks>
        /// 日付選択ダイアログを表示します
        /// </remarks>
        /// <param name="caption">ダイアログタイトル</param>
        /// <param name="currentDateTime">選択日付(時刻は「00:00:00」)</param>
        /// <param name="currentDateTime">既定の選択日付</param>
        /// <returns>選択された日付</returns>
        public static DialogResult Show( String caption, out DateTime selectDateTime, DateTime defaultSelectDateTime )
        {
            DlgDateSelect dlg = new DlgDateSelect( caption, defaultSelectDateTime );
            DialogResult result = dlg.ShowDialog();
            selectDateTime = dlg.SelectedDate;
            return result;
        }
        /// <summary>
        /// 日付選択ダイアログの表示
        /// </summary>
        /// <remarks>
        /// 日付選択ダイアログを表示します
        /// </remarks>
        /// <param name="caption">ダイアログタイトル</param>
        /// <returns>選択された日付</returns>
        public static DialogResult Show( String caption, out DateTime selectDateTime )
        {
            return Show( caption, out selectDateTime, DateTime.Today  );
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
            // タイトル
            if (this.Caption == String.Empty)
            {
                this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_DATESELECT_000;
            }

            // ボタン
            this.btnOk.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;
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
        private void btnOk_Click( object sender, EventArgs e )
        {
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
        /// 日付の選択
        /// </summary>
        /// <remarks>
        /// 日付を選択します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void cliDateSelect_BeforeSelectedDateRangeChange( object sender, Infragistics.Win.UltraWinSchedule.BeforeSelectedDateRangeChangeEventArgs e )
        {
            this.SelectedDate = e.NewSelectedDateRanges[0].StartDate.Date;
        }
        
        #endregion

        #region [内部クラス]
        
        /// <summary> 
        /// 月ヘッダーカスタムフォーマット表示用クラス
        /// </summary> 
        public class MonthHeaderCreationFilter : IUIElementCreationFilter
        {
            /// <summary>
            /// 月ヘッダーカスタムフォーマット 作成後処理
            /// </summary>
            /// <remarks>
            /// 月ヘッダーカスタムフォーマットの作成後処理を行います
            /// </remarks>
            /// <param name="parent"></param>
            void IUIElementCreationFilter.AfterCreateChildElements( Infragistics.Win.UIElement parent )
            {
                var headerElement = parent as MonthHeaderAreaUIElement;
                if ( headerElement != null )
                {
                    var textElement = headerElement.GetDescendant( typeof( TextUIElement ) ) as TextUIElement;
                    if ( textElement != null )
                    {
                        var month = headerElement.GetContext( typeof( Month ) ) as Month;
                        if ( month != null )
                        {
                            textElement.Text = new DateTime( month.Year.YearNumber, month.MonthNumber, 1 ).ToString( CultureInfo.CurrentCulture.DateTimeFormat.YearMonthPattern );
                        }
                    }
                }

                var item = parent as MonthPopupItemUIElement;
                if ( item != null )
                {
                    var textElement = item.GetDescendant( typeof( DependentTextUIElement ) ) as DependentTextUIElement;
                    
                    // TODO:ポップアップ表示の年月表示もフォーマットに沿った表示にする
                }
            }

            /// <summary>
            /// 月ヘッダーカスタムフォーマット 作成前処理
            /// </summary>
            /// <remarks>
            /// 月ヘッダーカスタムフォーマットの作成前処理を行います
            /// </remarks>
            /// <param name="parent"></param>
            /// <returns></returns>
            Boolean Infragistics.Win.IUIElementCreationFilter.BeforeCreateChildElements( UIElement parent )
            {
                return false;
            }
        }
        #endregion
    }
}
