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
    /// 検体登録印刷表示内容
    /// </summary>
    /// <remarks>
    /// 検体登録印刷用にデータを持たせておくクラスです。
    /// </remarks>
    class SpecimenRegistrationReportData
    {
        /// <summary>
        /// ラックID
        /// </summary>
        public String RackID = null;
        /// <summary>
        /// ラックポジション
        /// </summary>
        public Int32 RackPosition = 0;
        /// <summary>
        /// 受付番号
        /// </summary>
        public Int32 ReceiptNumber = 0;
        /// <summary>
        /// 検体ID
        /// </summary>
        public String PatientID = String.Empty;
        /// <summary>
        /// 検体種別
        /// </summary>
        public String SpecimenType = String.Empty;
        /// <summary>
        /// 分析項目名
        /// </summary>
        public String ProtoName = String.Empty;
        /// <summary>
        /// 自動稀釈倍率
        /// </summary>
        public Int32 AutoDil = 0;
        /// <summary>
        /// 手稀釈倍率
        /// </summary>
        public Int32 ManualDil = 0;
        /// <summary>
        /// コメント
        /// </summary>
        public String Comment = String.Empty;
        /// <summary>
        /// 出力日付
        /// </summary>
        public String PrintDateTime = String.Empty;

    }

    /// <summary>
    /// 一般検体登録印刷
    /// </summary>
    /// <remarks>
    /// 一般検体登録画面に表示したデータを印刷します。
    /// </remarks>
    public class SpecimenRegistrationPrint : PrintBase
    {

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SpecimenRegistrationPrint()
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
        public override Boolean Print( Object dataSource)
        {
            // 型が SpecimenRegistrationGridViewDataSet かどうかをチェック
            if ( !( dataSource is List<SpecimenRegistrationReportData> ) )
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
            //this.document = new SpecimenRegistrationReport();

            //SpecimenRegistrationReport rpt = (SpecimenRegistrationReport)this.document;

            Type tofUs = typeof( SpecimenRegistrationReport_US );
            Type tofCn = typeof( SpecimenRegistrationReport_CN );


            Type t = Type.GetType( String.Format( "Oelco.CarisX.Print.SpecimenRegistrationReport_{0}", SubFunction.GetRegionName( CarisXConst.SupportRegion ) ) );

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
                sectionName = "GroupHeaderSection1";
                TextObject receipt = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtReceiptNo"];
                TextObject rackID = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtRackID"];
                TextObject manualDilution = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtManualDilution"];
                TextObject patientID = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtPatientID"];
                TextObject rackPosition = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtRackPosition"];
                TextObject specimenType = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtSpecimenType"];
                TextObject comment = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtComment"];
                TextObject analytes = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtAnalytes"];
                TextObject autoDilution = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtAutoDilution"];

                // レポートのテキストにデータを入れ込む処理
                txtUserID.Text = Singleton<CarisXUserLevelManager>.Instance.NowUserID;
                txtUserLevel.Text = Singleton<CarisXUserLevelManager>.Instance.NowUserLevel.ToTypeString();
                pageTitle.Text = Resources.STRING_COMMON_PRINT_000;
                userID.Text = Resources.STRING_COMMON_PRINT_001;
                userLevel.Text = Resources.STRING_COMMON_PRINT_002;
                date.Text = Resources.STRING_COMMON_PRINT_003;
                reportTitle.Text = Resources.STRING_SPECIMENREGIST_PRINT_000;
                receipt.Text = Resources.STRING_SPECIMENREGIST_PRINT_001;
                rackID.Text = Resources.STRING_SPECIMENREGIST_PRINT_002;
                manualDilution.Text = Resources.STRING_SPECIMENREGIST_PRINT_003;
                patientID.Text = Resources.STRING_SPECIMENREGIST_PRINT_004;
                rackPosition.Text = Resources.STRING_SPECIMENREGIST_PRINT_005;
                specimenType.Text = Resources.STRING_SPECIMENREGIST_PRINT_006;
                comment.Text = Resources.STRING_SPECIMENREGIST_PRINT_007;
                analytes.Text = Resources.STRING_SPECIMENREGIST_PRINT_008;
                autoDilution.Text = Resources.STRING_SPECIMENREGIST_PRINT_009;
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
            List<SpecimenRegistrationReportData> dataSource = new List<SpecimenRegistrationReportData>();

            for ( int i = 0; i < dtCount; i++ )
            {
                dataSource.Add( new SpecimenRegistrationReportData() );
            }
            return base.GetTotalPageCount( dataSource );
        }        

        #endregion

    }
}
