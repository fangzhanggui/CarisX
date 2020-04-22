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
    /// 一般検体測定結果印刷表示内容
    /// </summary>
    /// <remarks>
    /// 一般検体測定結果印刷用にデータを持たせておくクラスです。
    /// </remarks>
    class SpecimenResultReportData
    {
        /// <summary>
        /// シーケンスNo.
        /// </summary>
        public Int32 SeqNo = 0;
        /// <summary>
        /// ラックID
        /// </summary>
        public String RackID = String.Empty;
        /// <summary>
        /// 種別
        /// </summary>
        public String SpecimenType = String.Empty;
        /// <summary>
        /// サンプルNo.
        /// </summary>
        public Int32 SampleNo = 0;
        /// <summary>
        /// 検体ID
        /// </summary>
        public String SampleID = String.Empty;
        /// <summary>
        /// コメント
        /// </summary>
        public String Comment = String.Empty;
        /// <summary>
        /// 分析項目名
        /// </summary>
        public String ProtoName = String.Empty;
        /// <summary>
        /// 手希釈倍率
        /// </summary>
        public Int32 ManualDil = 0;
        /// <summary>
        /// 自動希釈倍率
        /// </summary>
        public String AutoDil = String.Empty;
        /// <summary>
        /// 多重測定番号
        /// </summary>
        public Int32 MultiMeas = 0;
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
        /// 判定
        /// </summary>
        public String Judge = String.Empty;
        /// <summary>
        /// リマーク
        /// </summary>
        public String Remark = String.Empty;
        /// <summary>
        /// 試薬ロット番号
        /// </summary>
        public String ReagentLotNo = String.Empty;
        /// <summary>
        /// プレトリガロット番号
        /// </summary>
        public String PreTriggerLotNo = String.Empty;
        /// <summary>
        /// トリガロット番号
        /// </summary>
        public String TriggerLotNo = String.Empty;
        /// <summary>
        /// 測定時間
        /// </summary>
        public String MeasTime = String.Empty;
        /// <summary>
        /// グループ表示用測定日
        /// </summary>
        public String MeasDate = String.Empty;
        /// <summary>
        /// 出力日付
        /// </summary>
        public String PrintDateTime = String.Empty;

        /// <summary>
        /// 受付番号
        /// </summary>
        public Int32 ReceiptNo = 0;
    }

    /// <summary>
    /// 検体測定結果印刷
    /// </summary>
    /// <remarks>
    /// 検体測定結果画面に表示したデータを印刷します。
    /// </remarks>
    public class SpecimenResultPrint : PrintBase
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
        public SpecimenResultPrint()
        {
            //this.Initialize();
            rptType = Type.GetType( String.Format( "Oelco.CarisX.Print.SpecimenResultReport_{0}", SubFunction.GetRegionName( CarisXConst.SupportRegion ) ) );
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
        public Boolean Print( Object dataSource, Int32 startPageNo = 0 )
        {          
            // 型が SpecimenResultData かどうかをチェック
            if ( !( dataSource is List<SpecimenResultReportData> ) )
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
            //this.document = new SpecimenResultReport();
            //SpecimenResultReport rpt = (SpecimenResultReport)this.document;
            //Type t = Type.GetType( String.Format( "Oelco.CarisX.Print.SpecimenResultReport_{0}", SubFunction.GetRegionName( CarisXConst.SupportRegion ) ) );

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

                //sectionName = "GroupHeaderSection1";
                //TextObject seqNo = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtSeqNo"];
                //TextObject rackID = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtRackID"];
                //TextObject specimenType = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtSpecimenType"];
                //TextObject sampleNo = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtSampleNo"];
                TextObject sampleID = (TextObject)this.document.GetSection(sectionName).ReportObjects["txtSampleID"];
                //TextObject comment = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtComment"];
                TextObject analytes = (TextObject)this.document.GetSection(sectionName).ReportObjects["txtAnalytes"];
                //TextObject manualDil = (TextObject)this.document.GetSection(sectionName).ReportObjects["txtManualDil"];
                //TextObject autoDil = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtAutoDil"];
                //TextObject multiMeas = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtMultiMeasCount"];
                TextObject count = (TextObject)this.document.GetSection(sectionName).ReportObjects["txtCount"];
                //TextObject countAvg = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCountAvg"];
                TextObject conc = (TextObject)this.document.GetSection(sectionName).ReportObjects["txtConc"];
                //TextObject concAvg = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtConcAvg"];
                TextObject judge = (TextObject)this.document.GetSection(sectionName).ReportObjects["txtJudgement"];
                //TextObject remark = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtRemark"];
                TextObject reagentNo = (TextObject)this.document.GetSection(sectionName).ReportObjects["txtReagentLotNo"];
                //TextObject preTriggerLotNo = (TextObject)this.document.GetSection(sectionName).ReportObjects["txtPreTriggerLotNo"];
                //TextObject TriggerLotNo = (TextObject)this.document.GetSection(sectionName).ReportObjects["txtTriggerLotNo"];
                TextObject measTime = (TextObject)this.document.GetSection(sectionName).ReportObjects["txtMeasTime"];

                TextObject receiptNo = (TextObject)this.document.GetSection(sectionName).ReportObjects["txtReceiptNo"];

                // レポートのテキストにデータを入れ込む処理
                txtUserID.Text = Singleton<CarisXUserLevelManager>.Instance.NowUserID;
                txtUserLevel.Text = Singleton<CarisXUserLevelManager>.Instance.NowUserLevel.ToTypeString();
                pageTitle.Text = Resources.STRING_COMMON_PRINT_000;
                userID.Text = Resources.STRING_COMMON_PRINT_001;
                userLevel.Text = Resources.STRING_COMMON_PRINT_002;
                date.Text = Resources.STRING_COMMON_PRINT_003;
                reportTitle.Text = Resources.STRING_SPECIMENRESULT_PRINT_000;
                //seqNo.Text = Resources.STRING_SPECIMENRESULT_PRINT_001;
                //rackID.Text = Resources.STRING_SPECIMENRESULT_PRINT_002;
                //specimenType.Text = Resources.STRING_SPECIMENRESULT_PRINT_003;
                //sampleNo.Text = Resources.STRING_SPECIMENRESULT_PRINT_004;
                sampleID.Text = Resources.STRING_SPECIMENRESULT_PRINT_005;
                //comment.Text = Resources.STRING_SPECIMENRESULT_PRINT_006;
                analytes.Text = Resources.STRING_SPECIMENRESULT_PRINT_007;
                //manualDil.Text = Resources.STRING_SPECIMENRESULT_PRINT_008;
                //autoDil.Text = Resources.STRING_SPECIMENRESULT_PRINT_009;
                //multiMeas.Text = Resources.STRING_SPECIMENRESULT_PRINT_010;
                count.Text = Resources.STRING_SPECIMENRESULT_PRINT_011;
                //countAvg.Text = Resources.STRING_SPECIMENRESULT_PRINT_012;
                conc.Text = Resources.STRING_SPECIMENRESULT_PRINT_013;
                //concAvg.Text = Resources.STRING_SPECIMENRESULT_PRINT_014;
                judge.Text = Resources.STRING_SPECIMENRESULT_PRINT_015;
                //remark.Text = Resources.STRING_SPECIMENRESULT_PRINT_016;
                reagentNo.Text = Resources.STRING_SPECIMENRESULT_PRINT_017;
                //preTriggerLotNo.Text = Resources.STRING_SPECIMENRESULT_PRINT_018;
                //TriggerLotNo.Text = Resources.STRING_SPECIMENRESULT_PRINT_019;
                measTime.Text = Resources.STRING_SPECIMENRESULT_PRINT_020;
                receiptNo.Text = Resources.STRING_SPECIMENRESULT_PRINT_021;
            }           
        }

        /// <summary>
        /// 総ページ数取得
        /// </summary>
        /// baseの総ページ数取得処理の実行結果を返します。
        /// <param name="indUniqRepList">明細のデータ構成(IndividuallyNo,UniqueNo,ReplicationNo)</param>
        /// <returns></returns>
        public Int32 GetTotalPageCount( List<Tuple<Int32,Int32,Int32>> indUniqRepList  )
        {
            if ( indUniqRepList == null || indUniqRepList.Count == 0 )
            {
                return 0;
            }

            List<SpecimenResultReportData> dataSource = new List<SpecimenResultReportData>();

            foreach ( var data in indUniqRepList )
            {
                var dummy = new SpecimenResultReportData();
                // セクションのグループ区切り対象データを設定する。
                // シーケンス番号を検体識別番号で代用する。
                dummy.SeqNo = data.Item1;
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
            List<SpecimenResultReportData> dataSource = new List<SpecimenResultReportData>();

            for ( int i = 0; i < dtCount; i++ )
            {
                dataSource.Add( new SpecimenResultReportData() );
            }
            return base.GetTotalPageCount( dataSource );
        }        

        #endregion
    }
}

