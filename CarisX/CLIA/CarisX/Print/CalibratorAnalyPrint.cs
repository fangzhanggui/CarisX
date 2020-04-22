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
    /// キャリブレータ解析印刷表示内容
    /// </summary>
    /// <remarks>
    /// キャリブレータ解析印刷用にデータを持たせておくクラスです。
    /// </remarks>
    class CalibratorAnalyReportData
    {
        /// <summary>
        /// 検量線
        /// </summary>
        public String CalibCurve = String.Empty;
        /// <summary>
        /// 検量線有効期限
        /// </summary>
        public String CalibCurveLimit = String.Empty;
        /// <summary>
        /// 分析項目名
        /// </summary>
        public String Analytes = String.Empty;
        /// <summary>
        /// 基質ロットNo.
        /// </summary>
        public String SubLotNo = String.Empty;
        /// <summary>
        /// カートリッジロットNo.
        /// </summary>
        public String ReagentLotNo = String.Empty;
        /// <summary>
        /// 濃度
        /// </summary>
        public String Conc = String.Empty;
        /// <summary>
        /// カウント
        /// </summary>
        public Int32 Count = 0;
        /// <summary>
        /// 測定ポイント
        /// </summary>
        public String MeasPoint = String.Empty;
        /// <summary>
        /// 画像
        /// </summary>
        public System.Drawing.Bitmap Img = null;
        /// <summary>
        /// ラックポジション
        /// </summary>
        public String RackPosition = String.Empty;
        /// <summary>
        /// 検体多重測定回数
        /// </summary>
        public String MultiMeasNo = String.Empty;
        /// <summary>
        /// 出力日付
        /// </summary>
        public String PrintDateTime = String.Empty;
       
    }

    /// <summary>
    /// キャリブレータ解析印刷
    /// </summary>
    /// <remarks>
    /// キャリブレータ解析画面に表示したデータを印刷します。
    /// </remarks>
    public class CalibratorAnalyPrint : PrintBase, ISavePath
    {

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CalibratorAnalyPrint()
        {
            this.Initialize();
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 保存ファイルパス
        /// </summary>
        public String SavePath
        {
            get
            {
                // TODO:パラメータの保持階層確定次第埋める
                return String.Format( @"'{0}\CalibAnalyPrint.bmp'", CarisXConst.PathPrint );
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
        public override Boolean Print(Object dataSource)
        {
            // 型が CalibratorAnalyReportData かどうかをチェック
            if ( !( dataSource is List<CalibratorAnalyReportData> ) )
            {
                return false;
            }
            
            // CalibratorTrace表示用ピクチャパスを指定
            this.document.DataDefinition.FormulaFields["bmpCalibCurvePath"].Text = this.SavePath;

            // 印刷
            Boolean ret = base.Print(dataSource);

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
            Type t = Type.GetType( String.Format( "Oelco.CarisX.Print.CalibratorAnalyReport_{0}", SubFunction.GetRegionName(CarisXConst.SupportRegion) ) );

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
                TextObject calibCurve = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCalibCurve"];
                TextObject calibLimit = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCurveLimit"];
                TextObject analytes = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtAnalytes"];
                TextObject rackPosition = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtRackPosition"];
                TextObject multiMeasNo = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtMultiMeasNo"];
                TextObject ReagentLot = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtReagentLotNo"];
                TextObject conc = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtConc"];
                TextObject count = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCount"];
                TextObject measPoint = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtMeasPoint"];

                // レポートのテキストにデータを入れ込む処理
                txtUserID.Text = Singleton<CarisXUserLevelManager>.Instance.NowUserID;
                txtUserLevel.Text = Singleton<CarisXUserLevelManager>.Instance.NowUserLevel.ToTypeString();
                pageTitle.Text = Resources.STRING_COMMON_PRINT_000;
                userID.Text = Resources.STRING_COMMON_PRINT_001;
                userLevel.Text = Resources.STRING_COMMON_PRINT_002;
                date.Text = Resources.STRING_COMMON_PRINT_003;
                reportTitle.Text = Resources.STRING_CALIBANALYSIS_PRINT_000;
                calibCurve.Text = Resources.STRING_CALIBANALYSIS_PRINT_001;
                calibLimit.Text = Resources.STRING_CALIBANALYSIS_PRINT_002;
                analytes.Text = Resources.STRING_CALIBANALYSIS_PRINT_003;
                rackPosition.Text = Resources.STRING_CALIBANALYSIS_PRINT_004;
                multiMeasNo.Text = Resources.STRING_CALIBANALYSIS_PRINT_009;
                ReagentLot.Text = Resources.STRING_CALIBANALYSIS_PRINT_005;
                conc.Text = Resources.STRING_CALIBANALYSIS_PRINT_006;
                count.Text = Resources.STRING_CALIBANALYSIS_PRINT_007;
                measPoint.Text = Resources.STRING_CALIBANALYSIS_PRINT_008;
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
            if (dtCount == 0)
            {
            return 0;
            }
            List<CalibratorAnalyReportData> dataSource = new List<CalibratorAnalyReportData>();

            for ( int i = 0; i < dtCount; i++ )
            {
                dataSource.Add( new CalibratorAnalyReportData() );
            }
            return base.GetTotalPageCount( dataSource );
        
        }
        #endregion

    }
}

