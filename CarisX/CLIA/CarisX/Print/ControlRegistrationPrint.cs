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
    /// 精度管理検体登録印刷表示内容
    /// </summary>
    /// <remarks>
    /// 精度管理検体登録印刷用にデータを持たせておくクラスです。
    /// </remarks>
    class ControlRegistrationReportData
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
        /// 精度管理検体名
        /// </summary>
        public String ControlName = String.Empty;
        /// <summary>
        /// 精度管理検体ロット
        /// </summary>
        public String ControlLot = String.Empty;
        /// <summary>
        /// 分析項目名
        /// </summary>
        public String ProtoName = String.Empty;
        /// <summary>
        /// カートリッジロット
        /// </summary>
        public String CartridgeLot = String.Empty;
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
    /// 精度管理検体登録印刷
    /// </summary>
    /// <remarks>
    /// 精度管理検体登録画面に表示したデータを印刷します。
    /// </remarks>
    public class ControlRegistrationPrint : PrintBase
    {

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ControlRegistrationPrint()
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
        public override Boolean Print( object dataSource )
        {
            // 型が ControlRegistrationReportData かどうかをチェック
            if ( !( dataSource is List<ControlRegistrationReportData> ) )
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
            //this.document = new ControlRegistrationReport();

            //ControlRegistrationReport rpt = (ControlRegistrationReport)this.document;

            Type t = Type.GetType( String.Format( "Oelco.CarisX.Print.ControlRegistrationReport_{0}", SubFunction.GetRegionName( CarisXConst.SupportRegion ) ) );

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
                sectionName = "GroupHeaderSection2";
                TextObject rackID = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtRackID"];
                TextObject rackPosition = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtRackPosition"];
                TextObject ctrlName = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtControlName"];
                TextObject ctrlLot = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtControlLot"];
                TextObject comment = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtComment"];
                TextObject analytes = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtAnalytes"];
                TextObject ReagentLot = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtReagentLotNo"];

                // レポートのテキストにデータを入れ込む処理
                txtUserID.Text = Singleton<CarisXUserLevelManager>.Instance.NowUserID;
                txtUserLevel.Text = Singleton<CarisXUserLevelManager>.Instance.NowUserLevel.ToTypeString();
                pageTitle.Text = Resources.STRING_COMMON_PRINT_000;
                userID.Text = Resources.STRING_COMMON_PRINT_001;
                userLevel.Text = Resources.STRING_COMMON_PRINT_002;
                date.Text = Resources.STRING_COMMON_PRINT_003;
                reportTitle.Text = Resources.STRING_CONTROLREGIST_PRINT_000;
                rackID.Text = Resources.STRING_CONTROLREGIST_PRINT_001;
                rackPosition.Text = Resources.STRING_CONTROLREGIST_PRINT_002;
                ctrlName.Text = Resources.STRING_CONTROLREGIST_PRINT_003;
                ctrlLot.Text = Resources.STRING_CONTROLREGIST_PRINT_004;
                comment.Text = Resources.STRING_CONTROLREGIST_PRINT_005;
                analytes.Text = Resources.STRING_CONTROLREGIST_PRINT_006;
                ReagentLot.Text = Resources.STRING_CONTROLREGIST_PRINT_007;
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
            List<ControlRegistrationReportData> dataSource = new List<ControlRegistrationReportData>();

            for ( int i = 0; i < dtCount; i++ )
            {
                dataSource.Add( new ControlRegistrationReportData() );
            }
            return base.GetTotalPageCount( dataSource );
        }

        #endregion
    }
}
