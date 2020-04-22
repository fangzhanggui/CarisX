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
using Oelco.Common.Parameter;

namespace Oelco.CarisX.Print
{
    /// <summary>
    /// キャリブレータ登録印刷表示内容
    /// </summary>
    /// <remarks>
    /// キャリブレータ登録印刷用にデータを持たせておくクラスです。
    /// </remarks>
    class CalibRegistrationReportData
    {
        /// <summary>
        /// ラックID
        /// </summary>
        public String RackID = null;
        /// <summary>
        /// 分析項目名
        /// </summary>
        public String ProtoName = String.Empty;
        /// <summary>
        /// 濃度
        /// </summary>
        public String Conc = String.Empty;
        /// <summary>
        /// ロット選択
        /// </summary>
        public String LotSelect = String.Empty;
        /// <summary>
        /// ロットNo.
        /// </summary>
        public String LotNo = String.Empty;
        /// <summary>
        /// キャリブレータロット
        /// </summary>
        public String CalibratorLot = String.Empty;
        /// <summary>
        /// 出力日付
        /// </summary>
        public String PrintDateTime = String.Empty;
    }

    /// <summary>
    /// キャリブレータ登録印刷
    /// </summary>
    /// <remarks>
    /// キャリブレータ登録画面に表示したデータを印刷します。
    /// </remarks>
    public class CalibRegistrationPrint : PrintBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CalibRegistrationPrint()
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
            // 型が CalibratorRegistData かどうかをチェック
            if ( !( dataSource is List<CalibRegistrationReportData> ) )
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
            Type t = Type.GetType( String.Format( "Oelco.CarisX.Print.CalibRegistrationReport_{0}", SubFunction.GetRegionName( CarisXConst.SupportRegion ) ) );

            if ( t != null )
            {
                this.document = (ReportClass)Activator.CreateInstance( t );
                String sectionName = "Section2";

                // レポートのテキストを取得する処理
                TextObject txtUserID = (TextObject)this.document.GetSection( sectionName ).ReportObjects["UserID"];
                TextObject txtUserLevel = (TextObject)this.document.GetSection( sectionName ).ReportObjects["UserLevel"];
                TextObject pageTitle = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtPageTitle"];
                TextObject userID = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtUserID"];
                TextObject userLevel = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtUserLevel"];
                TextObject date = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtDate"];
                TextObject reportTitle = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtReportTitle"];
                TextObject rackID = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtRackID"];
                TextObject analytes = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtAnalytes"];
                TextObject conc = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtConc"];
                TextObject lotSelect = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtLotSelect"];
                TextObject lotNo = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtLotNo"];
                TextObject calibLot = (TextObject) this.document.GetSection( sectionName ).ReportObjects["txtCalibLot"];

                // レポートのテキストにデータを入れ込む処理
                txtUserID.Text = Singleton<CarisXUserLevelManager>.Instance.NowUserID;
                txtUserLevel.Text = Singleton<CarisXUserLevelManager>.Instance.NowUserLevel.ToTypeString();
                pageTitle.Text = Resources.STRING_COMMON_PRINT_000;
                userID.Text = Resources.STRING_COMMON_PRINT_001;
                userLevel.Text = Resources.STRING_COMMON_PRINT_002;
                date.Text = Resources.STRING_COMMON_PRINT_003;
                reportTitle.Text = Resources.STRING_CALIBREGIST_PRINT_000;
                rackID.Text = Resources.STRING_CALIBREGIST_PRINT_001;
                analytes.Text = Resources.STRING_CALIBREGIST_PRINT_002;
                conc.Text = Resources.STRING_CALIBREGIST_PRINT_003;
                lotSelect.Text = Resources.STRING_CALIBREGIST_PRINT_004;
                lotNo.Text = Resources.STRING_CALIBREGIST_PRINT_005;
                calibLot.Text = Resources.STRING_CALIBREGIST_PRINT_006;
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
            List<CalibRegistrationReportData> dataSource = new List<CalibRegistrationReportData>();

            for ( int i = 0; i < dtCount; i++ )
            {
                dataSource.Add( new CalibRegistrationReportData() );
            }
            return base.GetTotalPageCount( dataSource );
        }        

        #endregion
    }
}
