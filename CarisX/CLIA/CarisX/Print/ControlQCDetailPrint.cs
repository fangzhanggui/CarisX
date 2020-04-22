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
    /// コントロールQC詳細印刷表示内容
    /// </summary>
    /// <remarks>
    /// コントロールQC詳細印刷用にデータを持たせておくクラスです。
    /// </remarks>
    public class ControlQCDetailReportData
    {
        /// <summary>
        /// 分析項目名
        /// </summary>
        public String Analytes = String.Empty;

        /// <summary>
        /// 精度管理検体名
        /// </summary>
        public String ControlName = String.Empty;

        /// <summary>
        /// 精度管理検体ロット
        /// </summary>
        public String ControlLot = String.Empty;

        /// <summary>
        /// 期間From
        /// </summary>
        public String TermStart = String.Empty;

        /// <summary>
        /// 期間To
        /// </summary>
        public String TermEnd = String.Empty;

        /// <summary>
        /// シーケンス番号
        /// </summary>
        public String SequenceNo = String.Empty;

        /// <summary>
        /// 濃度
        /// </summary>
        public String Concentration = String.Empty;        

        /// <summary>
        /// 濃度平均値
        /// </summary>
        public String ConcentrationAve = String.Empty;

        /// <summary>
        /// 測定日
        /// </summary>
        public String MeasureDate = String.Empty;

        /// <summary>
        /// 試薬ロット番号
        /// </summary>
        public String ReagentLotNo = String.Empty;

        /// <summary>
        /// 出力日付
        /// </summary>
        public String PrintDateTime = String.Empty;
    }

    /// <summary>
    /// 精度管理印刷(明細部)
    /// </summary>
    /// <remarks>
    /// 精度管理画面グリッドに表示したデータを印刷します。
    /// </remarks>
    public class ControlQCDetailPrint : PrintBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ControlQCDetailPrint()
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
        public override Boolean Print( Object dataSource )
        {
            // 型が ControlQCReportData かどうかをチェック
            if ( !( dataSource is List<ControlQCDetailReportData> ) )
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
            //this.document = new ControlQCDetailReport();

            //ControlQCDetailReport rpt = (ControlQCDetailReport)this.document;

            Type t = Type.GetType( String.Format( "Oelco.CarisX.Print.ControlQCDetailReport_{0}", SubFunction.GetRegionName( CarisXConst.SupportRegion ) ) );

            if ( t != null )
            {
                this.document = (ReportClass)Activator.CreateInstance( t );                

                // レポートのテキストを取得する処理
                String sectionName = "Section2";
                TextObject txtUserID = (TextObject)this.document.GetSection( sectionName ).ReportObjects["UserID"];
                TextObject txtUserLevel = (TextObject)this.document.GetSection( sectionName ).ReportObjects["UserLevel"];
                TextObject pageTitle = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtPageTitle"];
                TextObject userID = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtUserID"];
                TextObject userLevel = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtUserLevel"];
                TextObject date = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtDate"];
                TextObject reportTitle = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtReportTitle"];
                TextObject analytes = (TextObject)this.document.GetSection( sectionName ).ReportObjects["TxtAnalytes"];
                TextObject ctrlName = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtControlName"];
                TextObject ctrlLot = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtControlLot"];
                TextObject term = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtTerm"];
                sectionName = "Section3";
                TextObject txtSequenceNo = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtSequenceNo"];
                TextObject txtConcentration = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtConcentration"];
                TextObject txtConcAve = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtConcAve"];
                TextObject txtMeasuringTime = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtMeasuringTime"];
                TextObject txtReagentLotNo = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtReagentLotNo"];

                // レポートのテキストにデータを入れ込む処理
                txtUserID.Text = Singleton<CarisXUserLevelManager>.Instance.NowUserID;
                txtUserLevel.Text = Singleton<CarisXUserLevelManager>.Instance.NowUserLevel.ToTypeString();
                pageTitle.Text = Resources.STRING_COMMON_PRINT_000;
                userID.Text = Resources.STRING_COMMON_PRINT_001;
                userLevel.Text = Resources.STRING_COMMON_PRINT_002;
                date.Text = Resources.STRING_COMMON_PRINT_003;
                reportTitle.Text = Resources.STRING_CONTROLQC_PRINT_000;
                analytes.Text = Resources.STRING_CONTROLQC_PRINT_001;
                ctrlName.Text = Resources.STRING_CONTROLQC_PRINT_002;
                ctrlLot.Text = Resources.STRING_CONTROLQC_PRINT_003;
                term.Text = Resources.STRING_CONTROLQC_PRINT_004;
                txtSequenceNo.Text = Resources.STRING_CONTROLQC_PRINT_018;
                txtConcentration.Text = Resources.STRING_CONTROLQC_PRINT_019;
                txtConcAve.Text = Resources.STRING_CONTROLQC_PRINT_020;
                txtMeasuringTime.Text = Resources.STRING_CONTROLQC_PRINT_021;
                txtReagentLotNo.Text = Resources.STRING_CONTROLQC_PRINT_022;      
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
            List<ControlQCDetailReportData> dataSource = new List<ControlQCDetailReportData>();

            for ( int i = 0; i < dtCount; i++ )
            {
                dataSource.Add( new ControlQCDetailReportData() );
            }
            return base.GetTotalPageCount( dataSource );
        }       

        #endregion

    }
}
