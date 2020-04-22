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
    /// コントロールQC印刷表示内容
    /// </summary>
    /// <remarks>
    /// コントロールQC印刷用にデータを持たせておくクラスです。
    /// </remarks>
    public class ControlQCReportData
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
        /// 日差変動用管理値平均
        /// </summary>
        public String InterDayReferenceMean = String.Empty;

        /// <summary>
        /// 日差変動用管理値濃度幅
        /// </summary>
        public String InterDayReferenceConcRange = String.Empty;

        /// <summary>
        /// 日差変動用平均値
        /// </summary>
        public String InterDayStatMean = String.Empty;

        /// <summary>
        /// 日差変動標準偏差値
        /// </summary>
        public String InterDayStatSD = String.Empty;

        /// <summary>
        /// R管理図
        /// </summary>
        public String RControlChart = String.Empty;

        /// <summary>
        /// 日内変動指定日
        /// </summary>
        public String IntraDayDate = String.Empty; 

        /// <summary>
        /// 日内変動平均
        /// </summary>
        public String IntraDayMean = String.Empty;

        /// <summary>
        /// 日内変動標準偏差
        /// </summary>
        public String IntraDaySD = String.Empty;

        /// <summary>
        /// 日内変動変動係数
        /// </summary>
        public String IntraDayCV = String.Empty;

        /// <summary>
        /// 日内変動管理値R
        /// </summary>
        public String IntraDayR = String.Empty;

        /// <summary>
        /// 出力日付
        /// </summary>
        public String PrintDateTime = String.Empty;

    }


    /// <summary>
    /// 精度管理印刷
    /// </summary>
    /// <remarks>
    /// 精度管理画面に表示したデータを印刷します。
    /// </remarks>
    public class ControlQCPrint : PrintBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ControlQCPrint()
        {
            this.Initialize();
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 保存ファイルパス(XBar)
        /// </summary>
        public String SavePathXBar
        {
            get
            {
                // TODO:パラメータの保持階層確定次第埋める
                return String.Format( @"'{0}\ControlQCXBar.bmp'", CarisXConst.PathPrint );
            }
        }
        /// <summary>
        /// 保存ファイルパス(RBar)
        /// </summary>
        public String SavePathRBar
        {
            get
            {
                // TODO:パラメータの保持階層確定次第埋める
                return String.Format( @"'{0}\ControlQCRBar.bmp'", CarisXConst.PathPrint );
            }
        }
        /// <summary>
        /// 保存ファイルパス(Dey)
        /// </summary>
        public String SavePathDey
        {
            get
            {
                // TODO:パラメータの保持階層確定次第埋める
                return String.Format( @"'{0}\ControlQCDey.bmp'", CarisXConst.PathPrint );
            }
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
            if ( !( dataSource is List<ControlQCReportData> ) )
            {
                return false;
            }

            // CalibratorTrace表示用ピクチャパスを指定
            this.document.DataDefinition.FormulaFields["bmpCtrlQCXBarPath"].Text = this.SavePathXBar;
            this.document.DataDefinition.FormulaFields["bmpCtrlQCRBarPath"].Text = this.SavePathRBar;
            this.document.DataDefinition.FormulaFields["bmpCtrlQCDeyPath"].Text = this.SavePathDey;            

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
            //this.document = new ControlQCReport();

            //ControlQCReport rpt = (ControlQCReport)this.document;

            Type t = Type.GetType( String.Format( "Oelco.CarisX.Print.ControlQCReport_{0}", SubFunction.GetRegionName( CarisXConst.SupportRegion ) ) );

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
                TextObject graphTitle1 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtGraphTitle1"];
                TextObject ctrlXBarAvg = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtControlValueXBar1"];
                TextObject ctrlXBarConc = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtControlValueXBar2"];
                TextObject staticXBarAvg = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtStaticValueXBar1"];
                TextObject staticXBarSD = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtStaticValueXBar2"];
                TextObject graphTitle2 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtGraphTitle2"];
                TextObject ctrlRBar = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtControlValueRBar1"];
                TextObject graphTitle3 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtGraphTitle3"];
                TextObject measDate = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtMeasDate"];
                TextObject ctrlDeyAvg = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtControlValueDey1"];
                TextObject ctrlDeySD = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtControlValueDey2"];
                TextObject ctrlDeyCV = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtControlValueDey3"];
                TextObject ctrlDeyR = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtControlValueDey4"];

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
                graphTitle1.Text = Resources.STRING_CONTROLQC_PRINT_005;
                ctrlXBarAvg.Text = Resources.STRING_CONTROLQC_PRINT_006;
                ctrlXBarConc.Text = Resources.STRING_CONTROLQC_PRINT_007;
                staticXBarAvg.Text = Resources.STRING_CONTROLQC_PRINT_008;
                staticXBarSD.Text = Resources.STRING_CONTROLQC_PRINT_009;
                graphTitle2.Text = Resources.STRING_CONTROLQC_PRINT_010;
                ctrlRBar.Text = Resources.STRING_CONTROLQC_PRINT_011;
                graphTitle3.Text = Resources.STRING_CONTROLQC_PRINT_012;
                measDate.Text = Resources.STRING_CONTROLQC_PRINT_013;
                ctrlDeyAvg.Text = Resources.STRING_CONTROLQC_PRINT_014;
                ctrlDeySD.Text = Resources.STRING_CONTROLQC_PRINT_015;
                ctrlDeyCV.Text = Resources.STRING_CONTROLQC_PRINT_016;
                ctrlDeyR.Text = Resources.STRING_CONTROLQC_PRINT_017;
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
            List<ControlQCReportData> dataSource = new List<ControlQCReportData>();

            for ( int i = 0; i < dtCount; i++ )
            {
                dataSource.Add( new ControlQCReportData() );
            }
            return base.GetTotalPageCount( dataSource );
        }        

        #endregion
    }
}
