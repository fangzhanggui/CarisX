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
    /// 分析項目印刷表示内容
    /// </summary>
    /// <remarks>
    /// 分析項目印刷用にデータを持たせておくクラスです。
    /// </remarks>
    class ProtocolReportData
    {
        /// <summary>
        /// 分析項目番号
        /// </summary>
        public Int32 ProtoNo = 0;
        /// <summary>
        /// 分析項目名
        /// </summary>
        public String ProtoName = String.Empty;
        /// <summary>
        /// 試薬名
        /// </summary>
        public String ReagName = String.Empty;
        /// <summary>
        /// 試薬コード
        /// </summary>
        public Int32 ReagCode = 0;
        /// <summary>
        /// 検体多重測定回数
        /// </summary>
        public Int32 MeasCount = 0;
        /// <summary>
        /// 制度管理検体多重測定回数
        /// </summary>
        public Int32 CtrlMeasCount = 0;
        /// <summary>
        /// キャリブレータ多重測定回数
        /// </summary>
        public Int32 CalibMeasCount = 0;
        /// <summary>
        /// キャリブレーション有効期限
        /// </summary>
        public String CalibLimit = String.Empty;
        /// <summary>
        /// 相関係数　係数A
        /// </summary>
        public String CoefA = String.Empty;
        /// <summary>
        /// 相関係数　係数B
        /// </summary>
        public String CoefB = String.Empty;
        /// <summary>
        /// パニック閾値
        /// </summary>
        public String PanicValue = String.Empty;
        /// <summary>
        /// Ra(オーバー)　上限値
        /// </summary>
        public String UpperOfRa = String.Empty;
        /// <summary>
        /// Ra(オーバー)　下限値
        /// </summary>
        public String LowerOfRa = String.Empty;
        /// <summary>
        /// Rb(範囲)　上限値
        /// </summary>
        public String UpperOfRb = String.Empty;
        /// <summary>
        /// Rb(範囲)　下限値
        /// </summary>
        public String LowerOfRb = String.Empty;

        /// <summary>
        /// アッセイシーケンス
        /// </summary>
        public String AssaySeq = String.Empty;
        /// <summary>
        /// サンプル種
        /// </summary>
        public String SampleType = String.Empty;
        /// <summary>
        /// 前希釈倍率
        /// </summary>
        public Int32 PreDil = 0;
        /// <summary>
        /// 濃度単位
        /// </summary>
        public String Unit = String.Empty;
        /// <summary>
        /// 自動希釈倍率使用有無
        /// </summary>
        public Boolean UseableAutoDil = false;
        /// <summary>
        /// 自動希釈倍率演算可否
        /// </summary>
        public Boolean UseableCalcAutoDil = false;
        /// <summary>
        /// 手希釈倍率使用有無
        /// </summary>
        public Boolean UseableManualDil = false;
        /// <summary>
        /// 手希釈倍率演算可否
        /// </summary>
        public Boolean UseableCalcManualDil = false;
        /// <summary>
        /// ダイナミックレンジ 上限
        /// </summary>
        public String UpperOfDynamicRange = String.Empty;
        /// <summary>
        /// ダイナミックレンジ 下限
        /// </summary>
        public String LowerOfDynamicRange = String.Empty;
        /// <summary>
        /// 濃度値Log(Ax+B) 係数A
        /// </summary>
        public String NumberOfConcLogA = String.Empty;
        /// <summary>
        /// 濃度値Log(Ax+B) 係数B
        /// </summary>
        public String NumberOfConcLogB = String.Empty;
        /// <summary>
        /// 多重測定内乖離限界比上限
        /// </summary>
        public String UpperOfFractLimit = String.Empty;
        /// <summary>
        /// 多重測定内乖離限界比下限
        /// </summary>
        public String LowerOfFractLimit = String.Empty;
        /// <summary>
        /// 多重測定内乖離限界差上限
        /// </summary>
        public String UpperOfDiffLimit = String.Empty;
        /// <summary>
        /// 多重測定内乖離限界差下限
        /// </summary>
        public String LowerOfDiffLimit = String.Empty;
        /// <summary>
        /// サンプル分注量
        /// </summary>
        public Int32 DispenseOfSample = 0;
        /// <summary>
        /// 標識液分注量
        /// </summary>
        public String DispenseOfConjugate = String.Empty;
        /// <summary>
        /// 標識液液量
        /// </summary>
        public String ConjugateVolume = String.Empty;
        /// <summary>
        /// フェライト粒子液量
        /// </summary>
        public String FerriteVolume = String.Empty;
        /// <summary>
        /// サンプル吸引量
        /// </summary>
        public String SampleSuckingUp = String.Empty;
        /// <summary>
        /// 検体前処理吸引量
        /// </summary>
        public String PreSuckingUp = String.Empty;
        /// <summary>
        /// 標識液槽位置
        /// </summary>
        public String ConjugatePoint = String.Empty;
        /// <summary>
        /// HBcrAg変換係数
        /// </summary>
        public String HBcrAgCoef = String.Empty;
        /// <summary>
        /// 小数点以下の桁数
        /// </summary>
        public Int32 NumberOfDecimal = 0;
        /// <summary>
        /// ダイナミックレンジ
        /// </summary>
        public Int32 DynamicRange = 0;
        /// <summary>
        /// HBcrAgタイプ
        /// </summary>
        public String TypeHBcrAg = String.Empty;
        /// <summary>
        /// 陽性判断閾値
        /// </summary>
        public String PositiveValue = String.Empty;
        /// <summary>
        /// 陰性判断閾値
        /// </summary>
        public String NegativeValue = String.Empty;
        /// <summary>
        /// キャリブレーションタイプ
        /// </summary>
        public String CalibrationType = String.Empty;
        /// <summary>
        /// フルキャリブレーションポイント数
        /// </summary>
        public Int32 CalibPoint = 0;
        /// <summary>
        /// 濃度1
        /// </summary>
        public String Conc1 = String.Empty;
        /// <summary>
        /// 濃度2
        /// </summary>
        public String Conc2 = String.Empty;
        /// <summary>
        /// 濃度3
        /// </summary>
        public String Conc3 = String.Empty;
        /// <summary>
        /// 濃度4
        /// </summary>
        public String Conc4 = String.Empty;
        /// <summary>
        /// 濃度5
        /// </summary>
        public String Conc5 = String.Empty;
        /// <summary>
        /// 濃度6
        /// </summary>
        public String Conc6 = String.Empty;
        /// <summary>
        /// 濃度7
        /// </summary>
        public String Conc7 = String.Empty;
        /// <summary>
        /// 濃度8
        /// </summary>
        public String Conc8 = String.Empty;
        /// <summary>
        /// 濃度1 カウント範囲 下限
        /// </summary>
        public String Conc1Min = String.Empty;
        /// <summary>
        /// 濃度2 カウント範囲 下限
        /// </summary>
        public String Conc2Min = String.Empty;
        /// <summary>
        /// 濃度3 カウント範囲 下限
        /// </summary>
        public String Conc3Min = String.Empty;
        /// <summary>
        /// 濃度4 カウント範囲 下限
        /// </summary>
        public String Conc4Min = String.Empty;
        /// <summary>
        /// 濃度5 カウント範囲 下限
        /// </summary>
        public String Conc5Min = String.Empty;
        /// <summary>
        /// 濃度6 カウント範囲 下限
        /// </summary>
        public String Conc6Min = String.Empty;
        /// <summary>
        /// 濃度7 カウント範囲 下限
        /// </summary>
        public String Conc7Min = String.Empty;
        /// <summary>
        /// 濃度8 カウント範囲 下限
        /// </summary>
        public String Conc8Min = String.Empty;
        /// <summary>
        /// 濃度1 カウント範囲 上限
        /// </summary>
        public String Conc1Max = String.Empty;
        /// <summary>
        /// 濃度2 カウント範囲 上限
        /// </summary>
        public String Conc2Max = String.Empty;
        /// <summary>
        /// 濃度3 カウント範囲 上限
        /// </summary>
        public String Conc3Max = String.Empty;
        /// <summary>
        /// 濃度4 カウント範囲 上限
        /// </summary>
        public String Conc4Max = String.Empty;
        /// <summary>
        /// 濃度5 カウント範囲 上限
        /// </summary>
        public String Conc5Max = String.Empty;
        /// <summary>
        /// 濃度6 カウント範囲 上限
        /// </summary>
        public String Conc6Max = String.Empty;
        /// <summary>
        /// 濃度7 カウント範囲 上限
        /// </summary>
        public String Conc7Max = String.Empty;
        /// <summary>
        /// 濃度8 カウント範囲 上限
        /// </summary>
        public String Conc8Max = String.Empty;
        /// <summary>
        /// 判定基準[+] 判定上限値
        /// </summary>
        public String Standard1Max = String.Empty;
        /// <summary>
        /// 判定基準[+2] 判定上限値
        /// </summary>
        public String Standard2Max = String.Empty;
        /// <summary>
        /// 判定基準[+3] 判定上限値
        /// </summary>
        public String Standard3Max = String.Empty;
        /// <summary>
        /// 判定基準[+] PG比
        /// </summary>
        public String Standard1PG = String.Empty;
        /// <summary>
        /// 判定基準[+2] PG比
        /// </summary>
        public String Standard2PG = String.Empty;
        /// <summary>
        /// 判定基準[+3] PG比
        /// </summary>
        public String Standard3PG = String.Empty;
        /// <summary>
        /// 陽性判定上限値
        /// </summary>
        public String PosiMax = String.Empty;
        /// <summary>
        /// 陽性判定閾値
        /// </summary>
        public String PosiLine = String.Empty;
        /// <summary>
        /// 陰性判定閾値
        /// </summary>
        public String NegaLine = String.Empty;
        /// <summary>
        /// 抑制率算出対照項目下限値
        /// </summary>
        public String LowerCalc = String.Empty;
        /// <summary>
        /// 出力日付
        /// </summary>
        public String PrintDateTime = String.Empty;
    }


    /// <summary>
    /// 分析項目印刷
    /// </summary>
    /// <remarks>
    /// 分析項目画面に表示したデータを印刷します。
    /// </remarks>
    public class  ProtocolPrint : PrintBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProtocolPrint()
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
        public Boolean Print(MeasureProtocol dataSource)
        {
            // 型が MeasureProtocol かどうかをチェック
            if (!(dataSource is MeasureProtocol))
            {
                return false;
            }

            // MeasureProtocol から　印刷用のデータテーブルへデータ加工
            MeasureProtocol dat = dataSource;

            //ProtocolReportDataにデータを格納する
            ProtocolReportData rptDT = new ProtocolReportData();

//TODO:CarisXの項目にする必要がある。とりあえずビルドエラーになるので何箇所かコメントにしてます。by KUSU
            
            rptDT.ProtoNo = dat.ProtocolNo;
            rptDT.ProtoName = dat.ProtocolName;
            rptDT.ReagName = dat.ReagentName;
            rptDT.ReagCode = dat.ReagentCode;
            rptDT.MeasCount = dat.RepNoForSample;
            rptDT.CtrlMeasCount = dat.RepNoForControl;
            rptDT.CalibMeasCount = dat.RepNoForCalib;
//            rptDT.CalibLimit = dat.CalibValidity.ToString();
//            rptDT.CoefA = dat.Coef_A.ToString();
//            rptDT.CoefB = dat.Coef_B.ToString();
//            rptDT.PanicValue = dat.PanicValue.ToString();
//            rptDT.UpperOfRa = dat.UpperOfRa.ToString();
//            rptDT.LowerOfRa = dat.LowerOfRa.ToString();
//            rptDT.UpperOfRb = dat.UpperOfRb.ToString();
//            rptDT.LowerOfRb = dat.LowerOfRb.ToString();
            rptDT.AssaySeq = dat.AssaySequence.ToString();
            rptDT.SampleType = CarisXSubFunction.GetSampleKindGridItemString((SpecimenMaterialType)dat.SampleKind);
//            rptDT.PreDil = dat.PreDilutionRatio;
            rptDT.Unit = dat.ConcUnit;
//            rptDT.UseableAutoDil = dat.UseAfterDil;
//            rptDT.UseableCalcAutoDil = dat.UseAfterDilAtCalcu;
//            rptDT.UseableManualDil = dat.UseManualDil;
//            rptDT.UseableCalcManualDil = dat.UseManualDilAtCalcu;
            rptDT.UpperOfDynamicRange = dat.ConcDynamicRange.Max.ToString();
            rptDT.LowerOfDynamicRange = dat.ConcDynamicRange.Min.ToString();
//            rptDT.NumberOfConcLogA = dat.CoefAOfLog.ToString();
//            rptDT.NumberOfConcLogB = dat.CoefBOfLog.ToString();
//            rptDT.UpperOfFractLimit = dat.MaxOfMulDiffLimitRatio.ToString();
//            rptDT.LowerOfFractLimit = dat.MinOfMulDiffLimitRatio.ToString();
//            rptDT.UpperOfDiffLimit = dat.MaxOfMulDiffLimitDiff.ToString();
//            rptDT.LowerOfDiffLimit = dat.MinOfMulDiffLimitDiff.ToString();
            rptDT.DispenseOfSample = dat.SmpDispenseVolume;
//            rptDT.DispenseOfConjugate = dat.ConjugateDispVol.ToString();
//            rptDT.ConjugateVolume = dat.ConjugateVol.ToString();
//            rptDT.FerriteVolume = dat.FerriteVol.ToString();
//            rptDT.SampleSuckingUp = dat.SmplAspVol.ToString();
//            rptDT.PreSuckingUp = dat.SmplPreLiqAspVol.ToString();
//            rptDT.ConjugatePoint = dat.ConjugatePosition.ToString();
            rptDT.NumberOfDecimal = dat.LengthAfterDemPoint;
//            rptDT.DynamicRange = dat.LengthAfterDemPointForDyn;
            rptDT.PositiveValue = dat.PosiLine.ToString();
            rptDT.NegativeValue = dat.NegaLine.ToString();
            rptDT.CalibrationType = dat.CalibType.ToString();
            rptDT.CalibPoint = dat.NumOfMeasPointInCalib;
            rptDT.Conc1 = dat.ConcsOfEach[0].ToString();
            rptDT.Conc2 = dat.ConcsOfEach[1].ToString();
            rptDT.Conc3 = dat.ConcsOfEach[2].ToString();
            rptDT.Conc4 = dat.ConcsOfEach[3].ToString();
            rptDT.Conc5 = dat.ConcsOfEach[4].ToString();
            rptDT.Conc6 = dat.ConcsOfEach[5].ToString();
            rptDT.Conc7 = dat.ConcsOfEach[6].ToString();
            rptDT.Conc8 = dat.ConcsOfEach[7].ToString();
            rptDT.Conc1Min = dat.CountRangesOfEach[0].Min.ToString();
            rptDT.Conc2Min = dat.CountRangesOfEach[1].Min.ToString();
            rptDT.Conc3Min = dat.CountRangesOfEach[2].Min.ToString();
            rptDT.Conc4Min = dat.CountRangesOfEach[3].Min.ToString();
            rptDT.Conc5Min = dat.CountRangesOfEach[4].Min.ToString();
            rptDT.Conc6Min = dat.CountRangesOfEach[5].Min.ToString();
            rptDT.Conc7Min = dat.CountRangesOfEach[6].Min.ToString();
            rptDT.Conc8Min = dat.CountRangesOfEach[7].Min.ToString();
            rptDT.Conc1Max = dat.CountRangesOfEach[0].Max.ToString();
            rptDT.Conc2Max = dat.CountRangesOfEach[1].Max.ToString();
            rptDT.Conc3Max = dat.CountRangesOfEach[2].Max.ToString();
            rptDT.Conc4Max = dat.CountRangesOfEach[3].Max.ToString();
            rptDT.Conc5Max = dat.CountRangesOfEach[4].Max.ToString();
            rptDT.Conc6Max = dat.CountRangesOfEach[5].Max.ToString();
            rptDT.Conc7Max = dat.CountRangesOfEach[6].Max.ToString();
            rptDT.Conc8Max = dat.CountRangesOfEach[7].Max.ToString();
//            rptDT.Standard1Max = dat.MaxValForPlus1.ToString();
//            rptDT.Standard2Max = dat.MaxValForPlus2.ToString();
//            rptDT.Standard3Max = dat.MaxValForPlus3.ToString();
//            rptDT.Standard1PG = dat.MaxRatioForPlus1.ToString();
//            rptDT.Standard2PG = dat.MaxRatioForPlus2.ToString();
//            rptDT.Standard3PG = dat.MaxRatioForPlus3.ToString();
//            rptDT.PosiMax = dat.MaxRatioForPSA.ToString();
            rptDT.PosiLine = dat.PosiLine.ToString();
            rptDT.NegaLine = dat.NegaLine.ToString();
//            rptDT.LowerCalc = dat.MinValForContrast.ToString();

            //// TODO : HBcrAgの受け渡し実装
            //// HBcrAgにチェックが入っていた時の処理
            //ProtocolReport rpt = (ProtocolReport)this.document;

            //TextObject coefHBcrAg = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCoefHBcrAg"];
            //TextObject HBcrAgType = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtHBcrAgType"];
            //if (dat.IsHBcrAgType == true)
            //{
            //    rptDT.HBcrAgCoef = dat.CoefOfHBcrAg.ToString();
            //    rptDT.TypeHBcrAg = dat.LengthAfterDemPointForHBcrAg.ToString();
            //    coefHBcrAg.Text = Resources.STRING_PROTOCOL_PRINT_092;
            //    HBcrAgType.Text = Resources.STRING_PROTOCOL_PRINT_093;
            //}
            //else
            //{
            //    rptDT.TypeHBcrAg = String.Empty;
            //    rptDT.HBcrAgCoef = String.Empty;
            //    coefHBcrAg.Text = String.Empty;
            //    HBcrAgType.Text = String.Empty;
            //}

            //// TODO:分析項目の受け渡し実装
            //// 分析項目がPG1の時
            //if ( ProtocolNo == "PG1" )
            //{
            //    rpt.DetailSection4.SectionFormat.EnableSuppress = false;

            //    TextObject culcSetPG = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtAnalytesCulcSetPG"];
            //    TextObject standard1 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtStandard1"];
            //    TextObject standard2 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtStandard2"];
            //    TextObject standard3 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtStandard3"];
            //    TextObject standardUpper1 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtStandardUpper1"];
            //    TextObject standardUpper2 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtStandardUpper2"];
            //    TextObject standardUpper3 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtStandardUpper3"];
            //    TextObject pg1 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtPG1"];
            //    TextObject pg2 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtPG2"];
            //    TextObject pg3 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtPG3"];
            //    TextObject culcSetPSA = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtAnalytesCulcSetPSA"];
            //    TextObject posiUpper = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtPosiUpper"];
            //    TextObject culcSetTest = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtAnalytesCulcSetTest"];
            //    TextObject posiLine = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtPosiLine"];
            //    TextObject negaLine = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtNegaLine"];
            //    TextObject lower = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtLower"];

            //    culcSetPG.Text = Resources.STRING_PROTOCOL_PRINT_076;
            //    standard1.Text = Resources.STRING_PROTOCOL_PRINT_077;
            //    standard2.Text = Resources.STRING_PROTOCOL_PRINT_078;
            //    standard3.Text = Resources.STRING_PROTOCOL_PRINT_079;
            //    standardUpper1.Text = Resources.STRING_PROTOCOL_PRINT_080;
            //    standardUpper2.Text = Resources.STRING_PROTOCOL_PRINT_081;
            //    standardUpper3.Text = Resources.STRING_PROTOCOL_PRINT_082;
            //    pg1.Text = Resources.STRING_PROTOCOL_PRINT_083;
            //    pg2.Text = Resources.STRING_PROTOCOL_PRINT_084;
            //    pg3.Text = Resources.STRING_PROTOCOL_PRINT_085;
            //    culcSetPSA.Text = Resources.STRING_PROTOCOL_PRINT_086;
            //    posiUpper.Text = Resources.STRING_PROTOCOL_PRINT_087;
            //    culcSetTest.Text = Resources.STRING_PROTOCOL_PRINT_088;
            //    posiLine.Text = Resources.STRING_PROTOCOL_PRINT_089;
            //    negaLine.Text = Resources.STRING_PROTOCOL_PRINT_090;
            //    lower.Text = Resources.STRING_PROTOCOL_PRINT_091;
            //}

            // 印刷
            Boolean ret = base.Print(rptDT);

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
            //this.document = new ProtocolReport();

            //ProtocolReport rpt = (ProtocolReport)this.document;
            Type t = Type.GetType( String.Format( "Oelco.CarisX.Print.ProtocolReport_{0}", SubFunction.GetRegionName( CarisXConst.SupportRegion ) ) );

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
                sectionName = "DetailSection2";
                TextObject analytesNo = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtAnalytesNo"];
                TextObject analytes = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtAnalytes"];
                TextObject reagName = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtReagName"];
                TextObject reagCD = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtReagCode"];
                TextObject measCount = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtMeasCount"];
                TextObject specMeasCount = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtSpecimenMeasCount"];
                TextObject ctrlMeasCount = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCtrlMeasCount"];
                TextObject calibMeasCount = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCalibMeasCount"];
                TextObject calibAssayCondition = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCalibAssayCondition"];
                TextObject calibLimit = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCalibrationLimit"];
                TextObject fixedAssayCondition = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtFixedAssayCondition"];
                TextObject coefA = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCoefA"];
                TextObject coefB = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCoefB"];
                TextObject panicValue = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtPanicValue"];
                TextObject retestRange = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtRetestRange"];
                TextObject raOver = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtRaOver"];
                TextObject raUpper = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtRaUpper"];
                TextObject raLower = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtRaLower"];
                TextObject rbRange = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtRbRange"];
                TextObject rbUpper = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtRbUpper"];
                TextObject rbLower = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtRbLower"];
                sectionName = "DetailSection3";
                TextObject dtlTitle = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtDtlTitle"];
                TextObject assayCondition = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtAssayCondition"];
                TextObject assaySeq = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtAssaySequence"];
                TextObject sampleType = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtSampleType"];
                TextObject presetDilRatio = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtPresetDilutionRatio"];
                TextObject unit = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtUnit"];
                TextObject useableAutoDilRatio = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtUseableAutoDilRatio"];
                TextObject usableAutoDilRatioOpe = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtUsableAutoDilRatioOperation"];
                TextObject useableManualDilRatio = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtUseableManualDilRatio"];
                TextObject useableManualDilRatioOpe = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtUseableManualDilRatioOperation"];
                TextObject FixedAssayCondition2 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtFixedAssayCondition2"];
                TextObject dynamicRangeUpper = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtDynamicRangeUpper"];
                TextObject dynamicRangeLower = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtDynamicRangeLower"];
                TextObject concentLogCoefA = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtConcentLogCoefA"];
                TextObject concentLogCoefB = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtConcentLogCoefB"];
                TextObject fractionUpper = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtFractionUpper"];
                TextObject fractionLower = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtFractionLower"];
                TextObject differenceUpper = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtDifferenceUpper"];
                TextObject differenceLower = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtDifferenceLower"];
                TextObject dispCondition = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtDispensingCondition"];
                TextObject sampleDispVolume = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtSampleDispVolume"];
                TextObject conjugateDisp = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtConjugateDisp"];
                TextObject conjugateVolume = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtConjugateVolume"];
                TextObject ferriteVolume = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtFerriteVolume"];
                TextObject sampleSuckingUp = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtSampleSuckingUp"];
                TextObject presetSuckingUp = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtPresetSuckingUp"];
                TextObject conjugateWell = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtConjugateWell"];
                TextObject option = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtOption"];
                TextObject analytesNo2 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtAnalytesNo2"];
                TextObject decimalPlace = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtDecimalPlace"];
                TextObject numberOfDecimal = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtNumberOfDecimal"];
                TextObject dynamicRange = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtDynamicRange"];
                TextObject negaPosiValue = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtNegativePositiveValue"];
                TextObject posiValue = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtPositiveValue"];
                TextObject negaValue = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtNegativeValue"];
                TextObject calibAssayCondition2 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCalibAssayCondition2"];
                TextObject calibrationType = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCalibrationType"];
                TextObject calibrationPoint = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCalibrationPoint"];
                TextObject conc1 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtConc1"];
                TextObject conc2 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtConc2"];
                TextObject conc3 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtConc3"];
                TextObject conc4 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtConc4"];
                TextObject conc5 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtConc5"];
                TextObject conc6 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtConc6"];
                TextObject conc7 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtConc7"];
                TextObject conc8 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtConc8"];
                TextObject countRange1 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCountRange1"];
                TextObject countRange2 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCountRange2"];
                TextObject countRange3 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCountRange3"];
                TextObject countRange4 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCountRange4"];
                TextObject countRange5 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCountRange5"];
                TextObject countRange6 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCountRange6"];
                TextObject countRange7 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCountRange7"];
                TextObject countRange8 = (TextObject)this.document.GetSection( sectionName ).ReportObjects["txtCountRange8"];

                // レポートのテキストにデータを入れ込む処理
                txtUserID.Text = Singleton<CarisXUserLevelManager>.Instance.NowUserID;
                txtUserLevel.Text = Singleton<CarisXUserLevelManager>.Instance.NowUserLevel.ToTypeString();
                pageTitle.Text = Resources.STRING_COMMON_PRINT_000;
                userID.Text = Resources.STRING_COMMON_PRINT_001;
                userLevel.Text = Resources.STRING_COMMON_PRINT_002;
                date.Text = Resources.STRING_COMMON_PRINT_003;
                reportTitle.Text = Resources.STRING_PROTOCOL_PRINT_000;
                analytesNo.Text = Resources.STRING_PROTOCOL_PRINT_001;
                analytes.Text = Resources.STRING_PROTOCOL_PRINT_002;
                reagName.Text = Resources.STRING_PROTOCOL_PRINT_003;
                reagCD.Text = Resources.STRING_PROTOCOL_PRINT_004;
                measCount.Text = Resources.STRING_PROTOCOL_PRINT_005;
                specMeasCount.Text = Resources.STRING_PROTOCOL_PRINT_006;
                ctrlMeasCount.Text = Resources.STRING_PROTOCOL_PRINT_007;
                calibMeasCount.Text = Resources.STRING_PROTOCOL_PRINT_008;
                calibAssayCondition.Text = Resources.STRING_PROTOCOL_PRINT_009;
                calibLimit.Text = Resources.STRING_PROTOCOL_PRINT_010;
                fixedAssayCondition.Text = Resources.STRING_PROTOCOL_PRINT_011;
                coefA.Text = Resources.STRING_PROTOCOL_PRINT_012;
                coefB.Text = Resources.STRING_PROTOCOL_PRINT_013;
                panicValue.Text = Resources.STRING_PROTOCOL_PRINT_014;
                retestRange.Text = Resources.STRING_PROTOCOL_PRINT_015;
                raOver.Text = Resources.STRING_PROTOCOL_PRINT_016;
                raUpper.Text = Resources.STRING_PROTOCOL_PRINT_017;
                raLower.Text = Resources.STRING_PROTOCOL_PRINT_018;
                rbRange.Text = Resources.STRING_PROTOCOL_PRINT_019;
                rbUpper.Text = Resources.STRING_PROTOCOL_PRINT_020;
                rbLower.Text = Resources.STRING_PROTOCOL_PRINT_021;

                // 詳細3、4を非表示
                this.document.GetSection( sectionName ).SectionFormat.EnableSuppress = true;
                this.document.GetSection( "DetailSection4" ).SectionFormat.EnableSuppress = true;

                // 一定の権限を持ったもののときは詳細を表示
                // それ以外は非表示
                if ( Singleton<CarisXUserLevelManager>.Instance.NowUserLevel == UserLevel.Level5 )
                {
                    this.document.GetSection( sectionName ).SectionFormat.EnableSuppress = false;

                    dtlTitle.Text = Resources.STRING_PROTOCOL_PRINT_022;
                    assayCondition.Text = Resources.STRING_PROTOCOL_PRINT_023;
                    assaySeq.Text = Resources.STRING_PROTOCOL_PRINT_024;
                    sampleType.Text = Resources.STRING_PROTOCOL_PRINT_025;
                    presetDilRatio.Text = Resources.STRING_PROTOCOL_PRINT_026;
                    unit.Text = Resources.STRING_PROTOCOL_PRINT_027;
                    useableAutoDilRatio.Text = Resources.STRING_PROTOCOL_PRINT_028;
                    usableAutoDilRatioOpe.Text = Resources.STRING_PROTOCOL_PRINT_029;
                    useableManualDilRatio.Text = Resources.STRING_PROTOCOL_PRINT_030;
                    useableManualDilRatioOpe.Text = Resources.STRING_PROTOCOL_PRINT_031;
                    FixedAssayCondition2.Text = Resources.STRING_PROTOCOL_PRINT_032;
                    dynamicRangeUpper.Text = Resources.STRING_PROTOCOL_PRINT_033;
                    dynamicRangeLower.Text = Resources.STRING_PROTOCOL_PRINT_034;
                    concentLogCoefA.Text = Resources.STRING_PROTOCOL_PRINT_035;
                    concentLogCoefB.Text = Resources.STRING_PROTOCOL_PRINT_036;
                    fractionUpper.Text = Resources.STRING_PROTOCOL_PRINT_037;
                    fractionLower.Text = Resources.STRING_PROTOCOL_PRINT_038;
                    differenceUpper.Text = Resources.STRING_PROTOCOL_PRINT_039;
                    differenceLower.Text = Resources.STRING_PROTOCOL_PRINT_040;
                    dispCondition.Text = Resources.STRING_PROTOCOL_PRINT_041;
                    sampleDispVolume.Text = Resources.STRING_PROTOCOL_PRINT_042;
                    conjugateDisp.Text = Resources.STRING_PROTOCOL_PRINT_043;
                    conjugateVolume.Text = Resources.STRING_PROTOCOL_PRINT_044;
                    ferriteVolume.Text = Resources.STRING_PROTOCOL_PRINT_045;
                    sampleSuckingUp.Text = Resources.STRING_PROTOCOL_PRINT_046;
                    presetSuckingUp.Text = Resources.STRING_PROTOCOL_PRINT_047;
                    conjugateWell.Text = Resources.STRING_PROTOCOL_PRINT_048;
                    option.Text = Resources.STRING_PROTOCOL_PRINT_049;
                    analytesNo2.Text = Resources.STRING_PROTOCOL_PRINT_050;
                    decimalPlace.Text = Resources.STRING_PROTOCOL_PRINT_051;
                    numberOfDecimal.Text = Resources.STRING_PROTOCOL_PRINT_052;
                    dynamicRange.Text = Resources.STRING_PROTOCOL_PRINT_053;
                    negaPosiValue.Text = Resources.STRING_PROTOCOL_PRINT_054;
                    posiValue.Text = Resources.STRING_PROTOCOL_PRINT_055;
                    negaValue.Text = Resources.STRING_PROTOCOL_PRINT_056;
                    calibAssayCondition2.Text = Resources.STRING_PROTOCOL_PRINT_057;
                    calibrationType.Text = Resources.STRING_PROTOCOL_PRINT_058;
                    calibrationPoint.Text = Resources.STRING_PROTOCOL_PRINT_059;
                    conc1.Text = Resources.STRING_PROTOCOL_PRINT_060;
                    conc2.Text = Resources.STRING_PROTOCOL_PRINT_061;
                    conc3.Text = Resources.STRING_PROTOCOL_PRINT_062;
                    conc4.Text = Resources.STRING_PROTOCOL_PRINT_063;
                    conc5.Text = Resources.STRING_PROTOCOL_PRINT_064;
                    conc6.Text = Resources.STRING_PROTOCOL_PRINT_065;
                    conc7.Text = Resources.STRING_PROTOCOL_PRINT_066;
                    conc8.Text = Resources.STRING_PROTOCOL_PRINT_067;
                    countRange1.Text = Resources.STRING_PROTOCOL_PRINT_068;
                    countRange2.Text = Resources.STRING_PROTOCOL_PRINT_069;
                    countRange3.Text = Resources.STRING_PROTOCOL_PRINT_070;
                    countRange4.Text = Resources.STRING_PROTOCOL_PRINT_071;
                    countRange5.Text = Resources.STRING_PROTOCOL_PRINT_072;
                    countRange6.Text = Resources.STRING_PROTOCOL_PRINT_073;
                    countRange7.Text = Resources.STRING_PROTOCOL_PRINT_074;
                    countRange8.Text = Resources.STRING_PROTOCOL_PRINT_075;
                }
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
            List<MeasureProtocol> dataSource = new List<MeasureProtocol>();

            for ( int i = 0; i < dtCount; i++ )
            {
                dataSource.Add( new MeasureProtocol() );
            }
            return base.GetTotalPageCount( dataSource );
        }        

        #endregion

    }
}

