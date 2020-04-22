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

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// バーコード読み取りダイアログクラス
    /// </summary>
    public partial class DlgReadBCBottle : DlgCarisXBase
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 試薬種別
        /// </summary>
        private ReagentKind reagentKind;

        /// <summary>
        /// バーコード読み取り対象の表示名
        /// </summary>
        private String targetName = String.Empty;

        /// <summary>
        /// バーコード読み取り結果
        /// </summary>
        private String barcodeResult = String.Empty;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgReadBCBottle()
        {
            InitializeComponent();
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 試薬種別の取得、設定
        /// </summary>
        public ReagentKind ReagentKind
        {
            get
            {
                return this.reagentKind;
            }
            set
            {
                this.reagentKind = value;
            }
        }

        /// <summary>
        /// バーコード読み取り対象の表示名
        /// </summary>
        public String TargetName
        {
            get
            {
                return this.targetName;
            }
            set
            {
                this.targetName = value;
            }
        }

        /// <summary>
        /// バーコード読み取り結果
        /// </summary>
        public String BarcodeResult
        {
            get
            {
                return this.barcodeResult;
            }
            set
            {
                this.barcodeResult = value;
            }
        }

		/// <summary>
		/// バーコード記載日時取得
		/// </summary>
		/// <remarks>
		/// このプロパティは、checkBC関数がtrueとなる時に有効な日時を返します。
		/// </remarks>
		public DateTime BarcodeResultDateTime
		{
			get
			{
				String yearDateMonth = this.BarcodeResult.Substring( CarisXConst.BC_TIME_POS, CarisXConst.BC_TIME_LENGTH );
				DateTime dateTime;
				SubFunction.DateTimeTryParseExactForYYMMDD( yearDateMonth, out dateTime );
				return dateTime;
			}
		}

		/// <summary>
		/// バーコード記載満杯容量値
		/// </summary>
		/// <remarks>
		/// このプロパティは、checkBC関数がtrueとなる時に有効な満杯容量値を返します。
		/// </remarks>
		public Int32 BarcodeResultCapacity
		{
			get
			{
				Int32 capacity = 0;
				String strCapa = this.BarcodeResult.Substring( CarisXConst.BC_CAPACITY_POS, CarisXConst.BC_CAPACITY_LENGTH );
				Int32.TryParse( strCapa, out capacity );
				return capacity;
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
                this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_READBCBOTTLE_000;
            }

            // ラベル
            this.lblMessage.Text = this.targetName;
            this.lblTitleBarcode.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_READBCBOTTLE_002;

            // ボタン
            this.btnOK.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
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
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.BarcodeResult = this.txtBarcode.Text;
            if (checkBC(this.BarcodeResult) == true)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
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
        private void DlgReadBCBottle_Shown(object sender, EventArgs e)
        {
            // テキストボックスにフォーカスセット
            this.txtBarcode.Focus();
        }

        /// <summary>
        /// バーコードのチェック
        /// </summary>
        /// <remarks>
        /// バーコードのチェックを行います
        /// </remarks>
        /// <param name="bc">バーコード入力値</param>
        private Boolean checkBC(string bc)
        {
            Int32 year;
            Int32 month;
            String strYear;
            String strMonth;			
            Int32 kind;
            String strKind;
            Int32 lotNum;
            String strLotNum;
            Int32 capa;
            String strCapa;

            try
            {
				// 長さチェック
				if ( bc.Length != CarisXConst.BC_INPUT_LENGTH || bc == String.Empty )
                {
                    //"入力されたバーコードの長さが不正です。"
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_165, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                    return false;
                }

                strYear = bc.Substring(9, 2);
                strMonth = bc.Substring(11, 2);
                // 年チェック
                if (!Int32.TryParse(strYear, out year))
                {
                    // "入力エラー"
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_159, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                    return false;
                }

                // 月チェック
                if (!Int32.TryParse(strMonth, out month))
                {
                    // "入力エラー"
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_159, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                    return false;
                }
                if (month > 12 || month < 1)
                {
                    // "入力エラー"
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_159, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                    return false;
                }

				// 有効期限切れエラー
				String yearDateMonth = bc.Substring(CarisXConst.BC_TIME_POS, CarisXConst.BC_TIME_LENGTH);
				DateTime dateTime;
				if ( !SubFunction.DateTimeTryParseExactForYYMMDD( yearDateMonth, out dateTime ) )
				{                   
					// "入力エラー"
					DlgMessage.Show( CarisX.Properties.Resources.STRING_DLG_MSG_159, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK );
				}
				if ( !checkTermOfValidity( dateTime ) )
                {
                    // "入力エラー"
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_159, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                    return false;
                }

                // 消耗品種別チェック
                strKind = bc.Substring(0, 1);
                if (!Int32.TryParse(strKind, out kind))
                {
                    // "入力エラー"
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_159, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                    return false;
                }
                if ( ((this.reagentKind == Const.ReagentKind.Diluent) && (kind != 1)) ||
                    ((this.reagentKind == Const.ReagentKind.Pretrigger) && (kind != 2)) ||
                    ((this.reagentKind == Const.ReagentKind.Trigger) && (kind != 3)) )
                {
                    // "入力エラー"
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_159, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                    return false;
                }

                // ロット番号チェック
                strLotNum = bc.Substring(1, 8);
                if (!Int32.TryParse(strLotNum, out lotNum))
                {
                    // "入力エラー"
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_159, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                    return false;
                }

				// 容量チェック
				strCapa = bc.Substring( CarisXConst.BC_CAPACITY_POS, CarisXConst.BC_CAPACITY_LENGTH );
                if (!Int32.TryParse(strCapa, out capa))
                {
                    // "入力エラー"
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_159, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                    return false;
                }
                if (capa > 1 || capa < 1)
                {
                    //容量は1：200mlのみ
                    // "入力エラー"
                    DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_159, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                    return false;
                }

                return true;
            }
            catch(Exception ex)
            {
                Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
                return false;
            }
        }

		/// <summary>
		/// 有効期限のチェック
		/// </summary>
		/// <remarks>
		/// 有効期限のチェックを行います
		/// </remarks>
		/// <param name="target">対象の日付</param>
		/// <returns>false:有効期限切れ</returns>
		public Boolean checkTermOfValidity( DateTime target )
		{
			Boolean result = true;

			// 比較方式は当日を許容する。(0時0分0秒が設定されているので、1日加算し、比較を行う）
			DateTime checkDateTime = target.AddDays( 1 );
			if ( DateTime.Now >= checkDateTime )
			{
				result = false;
			}
			return result;
		}

        #endregion
    }
}
