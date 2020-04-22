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
    /// キャリブレータ測定結果印刷表示内容
    /// </summary>
    /// <remarks>
    /// キャリブレータ測定結果印刷用にデータを持たせておくクラスです。
    /// </remarks>
    class CalibResultReportData
    {
        /// <summary>
        /// シーケンスNo.
        /// </summary>
        public String SeqNo = String.Empty;
        /// <summary>
        /// キャリブレータロット
        /// </summary>
        public String CalibratorLot = String.Empty;
        /// <summary>
        /// 試薬ロット
        /// </summary>
        public String ReagentLot = String.Empty;
        /// <summary>
        /// 基質ロット
        /// </summary>
        public String SubLot = String.Empty;
        /// <summary>
        /// ラックID
        /// </summary>
        public String RackID = String.Empty;
        /// <summary>
        /// ラックポジション
        /// </summary>
        public Int32 RackPosition = 0;
        /// <summary>
        /// 分析項目名
        /// </summary>
        public String ProtoName = String.Empty;
        /// <summary>
        /// カウント
        /// </summary>
        public String Count = String.Empty;
        /// <summary>
        /// カウント平均
        /// </summary>
        public String CountAvg = String.Empty;
        /// <summary>
        /// 濃度
        /// </summary>
        public String Conc = String.Empty;
        /// <summary>
        /// 濃度平均
        /// </summary>
        public String ConcAvg = String.Empty;
        /// <summary>
        /// 多重測定番号
        /// </summary>
        public Int32 MultiMeas = 0;
        /// <summary>
        /// リマーク
        /// </summary>
        public String Remark = String.Empty;
        /// <summary>
        /// 測定時間
        /// </summary>
        public String MeasTime = String.Empty;
        /// <summary>
        /// グループ表示用測定日
        /// </summary>
        public String MeasDate = String.Empty;
        /// <summary>
        /// プレトリガロット番号
        /// </summary>
        public String PreTriggerLotNo = String.Empty;
        /// <summary>
        /// トリガロット番号
        /// </summary>
        public String TriggerLotNo = String.Empty;
        /// <summary>
        /// 出力日付
        /// </summary>
        public String PrintDateTime = String.Empty;
    }

    /// <summary>
    /// キャリブレータ測定結果印刷
    /// </summary>
    /// <remarks>
    /// キャリブレータ測定結果画面に表示したデータを印刷します。
    /// </remarks>
    public class CalibResultPrint : PrintBase
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// レポートのType
        /// </summary>
        Type rptType;
        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CalibResultPrint()
        {
            //this.Initialize();
            rptType = Type.GetType( String.Format( "Oelco.CarisX.Print.CalibResultReport_{0}", SubFunction.GetRegionName( CarisXConst.SupportRegion ) ) );
            if ( rptType != null )
            {
                this.document = (ReportClass)Activator.CreateInstance( rptType );
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
        /// <param name="startPageNo">開始ページ番号</param>
        /// <returns>True:印刷成功　False:印刷失敗</returns>
        public Boolean Print( Object dataSource, Int32 startPageNo  = 0 )
        {
            // 型が CalibResultReportData かどうかをチェック
            if ( !( dataSource is List<CalibResultReportData> ) )
            {
                return false;
            }

            // 初期化処理
            this.Initialize();

            // ページ番号式フィールドの設定
            if ( startPageNo == 0 )
            {
                this.document.DataDefinition.FormulaFields["PageNo"].Text = "WhilePrintingRecords;PageNumber;";
            }
            else
            {
                this.document.DataDefinition.FormulaFields["PageNo"].Text = String.Format( "WhilePrintingRecords; PageNumber + {0};", ( startPageNo - 1 ).ToString() );
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

            //Type t = Type.GetType( String.Format( "Oelco.CarisX.Print.CalibResultReport_{0}", SubFunction.GetRegionName( CarisXConst.SupportRegion ) ) );
            if ( rptType != null )
            {
                //this.document = (ReportClass)Activator.CreateInstance( t );                

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
                TextObject seqNo = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtSeqNo"];
                TextObject calibLot = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCalibLotNo"];
                TextObject ReagentLotNo = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtReagentLotNo"];
              //  TextObject PreTriggerLotNo = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtPreTriggerLotNo"];
              //  TextObject TriggerLotNo = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtTriggerLotNo"];
                TextObject rackID = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtRackID"];
                TextObject rackPosition = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtRackPosition"];
                TextObject analytes = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtAnalytes"];
                TextObject count = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCount"];
                TextObject countAvg = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCountAvg"];
                TextObject conc = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtConc"];
                TextObject concAvg = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtConcAvg"];
                TextObject multiMeas = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtMultiMeasCount"];
               // TextObject remark = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtRemark"];
                TextObject measTime = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtMeasTime"];

                // レポートのテキストにデータを入れ込む処理
                txtUserID.Text = Singleton<CarisXUserLevelManager>.Instance.NowUserID;
                txtUserLevel.Text = Singleton<CarisXUserLevelManager>.Instance.NowUserLevel.ToTypeString();
                pageTitle.Text = Resources.STRING_COMMON_PRINT_000;
                userID.Text = Resources.STRING_COMMON_PRINT_001;
                userLevel.Text = Resources.STRING_COMMON_PRINT_002;
                date.Text = Resources.STRING_COMMON_PRINT_003;
                reportTitle.Text = Resources.STRING_CALIBRESULT_PRINT_000;
                seqNo.Text = Resources.STRING_CALIBRESULT_PRINT_001;
                calibLot.Text = Resources.STRING_CALIBRESULT_PRINT_002;
                ReagentLotNo.Text = Resources.STRING_CALIBRESULT_PRINT_015;
               // PreTriggerLotNo.Text = Resources.STRING_CALIBRESULT_PRINT_003;
               // TriggerLotNo.Text = Resources.STRING_CALIBRESULT_PRINT_004;
                rackID.Text = Resources.STRING_CALIBRESULT_PRINT_005;
                rackPosition.Text = Resources.STRING_CALIBRESULT_PRINT_006;
                analytes.Text = Resources.STRING_CALIBRESULT_PRINT_007;
                count.Text = Resources.STRING_CALIBRESULT_PRINT_008;
                countAvg.Text = Resources.STRING_CALIBRESULT_PRINT_009;
                conc.Text = Resources.STRING_CALIBRESULT_PRINT_010;
                concAvg.Text = Resources.STRING_CALIBRESULT_PRINT_011;
                multiMeas.Text = Resources.STRING_CALIBRESULT_PRINT_012;
              //  remark.Text = Resources.STRING_CALIBRESULT_PRINT_013;
                measTime.Text = Resources.STRING_CALIBRESULT_PRINT_014;
            }
            

            
        }

        /// <summary>
        /// 総ページ数取得
        /// </summary>
        /// baseの総ページ数取得処理の実行結果を返します。
        /// <param name="indUniqRepList">明細のデータ構成(IndividuallyNo,UniqueNo,ReplicationNo)</param>
        /// <returns></returns>
        public Int32 GetTotalPageCount( List<Tuple<Int32, Int32, Int32>> indUniqRepList )
        {
            if ( indUniqRepList == null || indUniqRepList.Count == 0 )
            {
                return 0;
            }

            List<CalibResultReportData> dataSource = new List<CalibResultReportData>();

            foreach ( var data in indUniqRepList )
            {
                var dummy = new CalibResultReportData();
                // セクションのグループ区切り対象データを設定する。
                // シーケンス番号を検体識別番号で代用する。
                dummy.SeqNo = data.Item1.ToString();
                dataSource.Add( dummy );
            }

            return base.GetTotalPageCount( dataSource );
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
            List<CalibResultReportData> dataSource = new List<CalibResultReportData>();

            for ( int i = 0; i < dtCount; i++ )
            {
                dataSource.Add( new CalibResultReportData() );
            }
            return base.GetTotalPageCount( dataSource );
        }

        #endregion

    }
}
