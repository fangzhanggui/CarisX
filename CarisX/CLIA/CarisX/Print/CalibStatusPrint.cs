using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.Print;
using System.Data;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using Oelco.CarisX.Properties;
using Oelco.Common.Utility;
using Oelco.CarisX.Parameter;
using System.Globalization;
using Oelco.CarisX.Const;
using Oelco.CarisX.Utility;
using Oelco.CarisX.DB;

namespace Oelco.CarisX.Print
{
    /// <summary>
    /// キャリブレータステータス印刷表示内容
    /// </summary>
    /// <remarks>
    /// キャリブレータステータス印刷用にデータを持たせておくクラスです。
    /// </remarks>
    class CalibStatsuReportData
    {
        /// <summary>
        /// 分析項目名称
        /// </summary>
        public String ProtocolName=String.Empty;

        /// <summary>
        ///  試薬残量
        /// </summary>
        public String Remain = String.Empty;
        
        /// <summary>
        /// 試薬ロット番号
        /// </summary>
        public String ReagentLotNo = String.Empty;        
        
        /// <summary>
        /// 検量線状態
        /// </summary>
        public String CurveStatus = String.Empty;
        /// <summary>
        /// 出力日付
        /// </summary>
        public String PrintDateTime = String.Empty;

    }

    /// <summary>
    /// キャリブレータステータス印刷
    /// </summary>
    /// <remarks>
    /// キャリブレータステータス画面に表示したデータを印刷します。
    /// </remarks>
    class CalibStatusPrint : PrintBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CalibStatusPrint()
        {
            this.Initialize();
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 印刷
        /// </summary>
        /// <remarks>
        /// データを加工し、印刷を行います。
        /// </remarks>
        /// <param name="dataSource">印刷データ</param>
        /// <returns>True:印刷成功　False:印刷失敗</returns>
        public override Boolean Print( object  dataSource )
        {
            // 型が CalibStatsuReportData かどうかをチェック
            if ( !( dataSource is List<CalibStatsuReportData> ) )
            {
                return false;
            }

            // 印刷
            Boolean ret = base.Print( dataSource );

            return ret;
        }

        #endregion

        #region [protectedメソッド]
        
        /// <summary>
        /// レポートタイトルの表示
        /// </summary>
        /// <remarks>
        /// レポートに表示する固定名称を入れます。
        /// </remarks>
        protected override void Initialize()
        {
            //this.document = new CalibStatusReport();

            //CalibStatusReport rpt = (CalibStatusReport)this.document;

            Type t = Type.GetType( String.Format( "Oelco.CarisX.Print.CalibStatusReport_{0}", SubFunction.GetRegionName( CarisXConst.SupportRegion ) ) );

            if ( t != null )
            {
                this.document = (ReportClass)Activator.CreateInstance( t );                

                //// レポートのテキストを取得する処理
                String sectionName = "Section2";
                TextObject txtUserID = (TextObject)this.document.GetSection( sectionName ).ReportObjects["UserID"];
                TextObject txtUserLevel = (TextObject)this.document.GetSection( sectionName ).ReportObjects["UserLevel"];
                TextObject pageTitle = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtPageTitle"];
                TextObject userID = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtUserID"];
                TextObject userLevel = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtUserLevel"];
                TextObject date = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtDate"];
                TextObject reportTitle = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtReportTitle"];
                TextObject protocolName = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtProtocolName"];
                TextObject remain = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtRemain"];
                TextObject reagentNo = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtReagentNo"];
                TextObject curveStatus = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCurveStatus"];

                //// レポートのテキストにデータを入れ込む処理
                txtUserID.Text = Singleton<CarisXUserLevelManager>.Instance.NowUserID;
                txtUserLevel.Text = Singleton<CarisXUserLevelManager>.Instance.NowUserLevel.ToTypeString();
                pageTitle.Text = Resources.STRING_COMMON_PRINT_000;
                userID.Text = Resources.STRING_COMMON_PRINT_001;
                userLevel.Text = Resources.STRING_COMMON_PRINT_002;
                date.Text = Resources.STRING_COMMON_PRINT_003;
                reportTitle.Text = Resources.STRING_CALIBSTATUS_PRINT_000;
                protocolName.Text = Resources.STRING_CALIBSTATUS_PRINT_001;
                remain.Text = Resources.STRING_CALIBSTATUS_PRINT_002;
                reagentNo.Text = Resources.STRING_CALIBSTATUS_PRINT_003;
                curveStatus.Text = Resources.STRING_CALIBSTATUS_PRINT_004;
            }            
        }

        /// <summary>
        /// 総ページ数取得
        /// </summary>
        /// baseの総ページ数取得処理の実行結果を返します。
        /// <param name="dtCount">明細の件数</param>
        /// <returns></returns>
        public Int32 GetTotalPageCount( Int32 dtCount )
        {
            if ( dtCount == 0 )
            {
                return 0;
            }
            List<CalibStatsuReportData> dataSource = new List<CalibStatsuReportData>();

            for ( int i = 0; i < dtCount; i++ )
            {
                dataSource.Add( new CalibStatsuReportData() );
            }
            return base.GetTotalPageCount( dataSource );
        }

        #endregion

    }
}
