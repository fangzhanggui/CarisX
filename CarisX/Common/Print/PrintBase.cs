//----------------------------------------------------------------
// Public Class.
//	         ReportBase
// Info.
//   クリスタルレポートの基本印刷機能を提供する。
// History
//   2011/11/16  Ver1.00.00  作成  Tomoaki Hanamachi
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Oelco.Common.Log;
using Oelco.Common.Utility;

namespace Oelco.Common.Print
{
    /// <summary>
    /// 帳票印刷基底クラス
    /// </summary>
    public abstract class PrintBase
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 印刷ドキュメント
        /// </summary>
        /// <remarks>印刷を行うクリスタルレポートドキュメントを指定します</remarks>
        protected ReportClass document = null;

        /// <summary>
        /// 印刷用紙サイズ
        /// </summary>
        /// <remarks>規定はA4</remarks>
        protected PaperSize paperSize = PaperSize.PaperA4;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// プリンター名
        /// </summary>
        /// <remarks>指定されていない場合はデフォルトプリンターで印刷</remarks>
        public String PrinterName
        {
            get;
            set;
        }

        /// <summary>
        /// 印刷用紙サイズプロパティ
        /// </summary>
        /// <remarks>文字列で指定する（B5、A4、A3、Letter)</remarks>
        public String PrintSize
        {
            get
            {
                switch ( this.paperSize )
                {
                case PaperSize.PaperB5:
                    return "B5";
                case PaperSize.PaperA4:
                    return "A4";
                case PaperSize.PaperA3:
                    return "A3";
                case PaperSize.PaperLetter:
                    return "Letter";
                default:
                    this.paperSize = PaperSize.PaperA4;
                    return "A4";
                }
            }

            set
            {
                switch ( value )
                {
                case "B5":
                    this.paperSize = PaperSize.PaperB5;
                    break;
                case "A4":
                    this.paperSize = PaperSize.PaperA4;
                    break;
                case "A3":
                    this.paperSize = PaperSize.PaperA3;
                    break;
                case "Letter":
                    this.paperSize = PaperSize.PaperLetter;
                    break;
                default:
                    this.paperSize = PaperSize.PaperA4;
                    break;
                }
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 初期化
        /// </summary>
        /// <remarks>派生クラスで印刷するクリスタルレポートオブジェクトを必ず指定する</remarks>
        protected abstract void Initialize();

        /// <summary>
        /// 印刷
        /// </summary>
        /// <remarks>
        /// 印刷処理を行います。
        /// </remarks>
        /// <param name="dataSource">印刷データ</param>
        /// <returns>True:印刷成功　False:印刷失敗</returns>
        public virtual Boolean Print( Object dataSource )
        {
            try
            {
                // プリンタ指定
                if (!String.IsNullOrEmpty(this.PrinterName))
                {
                    this.document.PrintOptions.PrinterName = this.PrinterName;
                }
                // 印刷用紙サイズ指定
                this.document.PrintOptions.PaperSize = this.paperSize;
                // 印刷データ指定
                this.document.SetDataSource( new DataSet());
                this.document.SetDataSource( dataSource );
                // 印刷実行
                this.document.PrintToPrinter( 1, true, 0, 0 );
            }
            catch ( Exception ex )
            {
                System.Diagnostics.Debug.WriteLine(String.Format("{0} {1}", ex.Message, ex.StackTrace));

                return false;
            }

            return true;
        }
       
        /// <summary>
        /// 印刷（印刷サイズ指定）
        /// </summary>
        /// <remarks>
        ///　用紙サイズ指定で印刷処理を行います。
        /// </remarks>
        /// <param name="dataSource">印刷データ</param>
        /// <param name="printSize">印刷サイズを文字列で指定</param>
        /// <returns>True:印刷成功 False:印刷失敗</returns>
        public virtual Boolean Print( Object dataSource, String printSize )
        {
            this.PrintSize = printSize;
            return this.Print( dataSource );
        }

        /// <summary>
        /// 総ページ数取得
        /// </summary>
        /// <remarks>
        /// 帳票の総ページ数を返します。
        /// </remarks>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public virtual Int32 GetTotalPageCount( Object dataSource )
        {
            Int32 rtn = 0;
            try
            {
                // 印刷データ指定
                this.document.SetDataSource( dataSource );

                rtn = this.document.FormatEngine.GetLastPageNumber( new CrystalDecisions.Shared.ReportPageRequestContext() );
            }
            catch(Exception ex)
            {
                Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, ex.StackTrace);
            }           
            return rtn;
        }

        #endregion

    }
}
