using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Parameter;
using Oelco.Common.Utility;
using Oelco.Common.Calculator;
using Oelco.CarisX.Const;
using Oelco.CarisX.DB;
using Oelco.CarisX.GUI.Controls;
using System.Threading.Tasks;
using Oelco.CarisX.Status;
using Oelco.CarisX.Common;
using System.Collections;
using Oelco.CarisX.Log;
using Oelco.Common.Parameter;
using Oelco.CarisX.GUI;
using Oelco.Common.Log;
using System.Threading;
using Oelco.Common.Const;
using Oelco.CarisX.Comm;

namespace Oelco.CarisX.Calculator
{
	/// <summary>
	/// 計算処理クラス
	/// </summary>
	public class CarisXCalculator
	{
		#region [クラス変数]

		 /// <summary>
		/// カウント計算を許容する分析エラーリスト
		/// </summary>
		private static IEnumerable<AnalysisError> calcCountPermissibleError = new AnalysisError[] { AnalysisError.NoError, AnalysisError.HighError, AnalysisError.LowError };
		
		/// <summary>
		/// 濃度計算を許容する分析エラーリスト
		/// </summary>
		private static IEnumerable<AnalysisError> calcConcPermissibleError = new AnalysisError[] { AnalysisError.NoError, AnalysisError.HighError, AnalysisError.LowError };

		/// <summary>
		/// カウント平均計算を許容する分析エラーリスト
		/// </summary>
		private static IEnumerable<AnalysisError> calcAverageCountPermissibleError = new AnalysisError[] { AnalysisError.NoError, AnalysisError.HighError, AnalysisError.LowError };

		/// <summary>
		/// 最終算出拡張パラメータ
		/// </summary>
		private static List<String> lastExtParam = null;

		#endregion

		#region [publicメソッド]
		
		#region _再計算_

		/// <summary>
		/// 一般(優先)検体再計算
		/// </summary>
		/// <remarks>
		/// 一般検体(優先検体含む)の濃度、陽性/陰性の判定の再計算を行います。
		/// </remarks>
		/// <param name="recalcInfo">再計算情報</param>
		/// <param name="calcDataList">再計算対象データ</param>
		/// <param name="reCalcCompleteData">再計算済みデータ</param>
		/// <returns>true:再計算完了 false:再計算未完了</returns>
		public static Boolean ReCalcSpecimen( IRecalcInfo recalcInfo, List<CalcData> calcDataList, out List<CalcData> reCalcCompleteData )
		{
			return ReCalc( calcDataList, SampleKind.Sample, recalcInfo, out reCalcCompleteData );
		}

		/// <summary>
		/// 精度管理検体再計算
		/// </summary>
		/// <remarks>
		/// 精度管理検体の濃度、陽性/陰性の判定の再計算を行います。
		/// </remarks>
		/// <param name="recalcInfo">再計算情報</param>
		/// <param name="calcDataList">再計算対象データ</param>
		/// <param name="reCalcCompleteData">再計算済みデータ</param>
		/// <returns>true:再計算完了 false:再計算未完了</returns>
		public static Boolean ReCalcControl( IRecalcInfo recalcInfo, List<CalcData> calcDataList, out List<CalcData> reCalcCompleteData )
		{
			return ReCalc( calcDataList, SampleKind.Control, recalcInfo, out reCalcCompleteData );
		}
		#endregion

		#region _計算_

		/// <summary>
		/// 計算処理
		/// </summary>
		/// <remarks>
		/// 濃度、陽性/陰性の判定の計算を行います。
		/// </remarks>
		/// <param name="dark">ダーク値</param>
		/// <param name="blank">ブランク値</param>
		/// <param name="measuredCount">測光値</param>
		/// <param name="kind">検体種別</param>
		/// <param name="calcData">計算データ</param>
		/// <param name="analysisErrorInfoList">分析エラー情報リスト</param>
		/// <param name="averageCalcData">平均計算データ</param>
		/// <returns>計算処理</returns>
		public static Boolean Calc(Int32 moduleIndex, Int32 dark, Int32 blank, Int32 measuredCount, SampleKind kind, Int32 repCount, ref CalcData calcData, out List<AnalysisErrorInfo> analysisErrorInfoList, out CalcData averageCalcData )
		{
			// 計算開始ログ
			Singleton<CarisXLogManager>.Instance.WriteCommonLog( LogKind.DebugLog, "<$*$ Calculator $*$> start Calc" );

			// 平均計算データの初期化
			averageCalcData = null;

            // 分析エラーリストの初期化
            analysisErrorInfoList = new List<AnalysisErrorInfo>();

			#region _計算データクラス初期化_

			calcData.MeasureDateTime = DateTime.Now;
			calcData.UseCalcCalibCurveApprovalDate = null;
			calcData.CalcInfoAverage = null;
			calcData.Judgement = null;
			if ( calcData.CalcInfoReplication == null )
			{
				calcData.CalcInfoReplication = new CalcInfo( null );
			}
			calcData.CalcInfoReplication.CountValue = null;
			calcData.CalcInfoReplication.Concentration = null;

			#endregion

			// 分析項目の取得
			MeasureProtocol measureProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( calcData.ProtocolIndex );

			// 有効期限エラーチェック

			if ( HybridDataMediator.SearchReagentExpirationDateErrorFromReagentDB( measureProtocol.ReagentCode, calcData.ReagentLotNo, calcData.MeasureDateTime ) )
			{
                //2015/5/8 有効期限切れリマーク表示しない--------------------------
				// リマーク追加[試薬有効期限エラー]
                //calcData.CalcInfoReplication.Remark.AddRemark( Remark.RemarkBit.ReagentExpirationDateError );
                //Singleton<CarisXLogManager>.Instance.WriteCommonLog( LogKind.DebugLog, "<$*$ Calculator $*$> Calc 試薬有効期限エラー" );
                //-----------------------------------------------------------------
			}

			#region _計算処理(レプリケーション)_

			// 計算処理可能なリマークの場合
			if ( calcData.CalcInfoReplication.Remark.CanCalcConcentration )
			{
                Singleton<CarisXLogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "<$*$ Calculator $*$> 能Calc 濃度算出");
				#region _カウント値の算出(装置補正)_

				// カウント値の算出＜機差補正(装置補正係数)＞
				calcData.CalcInfoReplication.CountValue = CarisXCalculator.CalcCount( measuredCount, blank, moduleIndex);

				#endregion

				#region _計算_

				var calcInfo = calcData.CalcInfoReplication;

				// 計算(ダークエラー・測光エラー以外)
				if ( !CheckDarkError( dark, ref analysisErrorInfoList, ref calcInfo )
                    && !CheckMeasureError(measuredCount, blank, calcData.ProtocolIndex, kind, ref analysisErrorInfoList, ref calcInfo))
				{
                    //Singleton<CarisXLogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "<$*$ Calculator $*$> Calc Dark error /測光Error");
					#region __濃度計算__
					// 濃度、判定算出
					if ( CarisXCalculator.CalcConc( measureProtocol, kind, null, calcData, false, analysisErrorInfoList: analysisErrorInfoList ) )
					{
						// 濃度算出時の検体種別毎の処理
						CalcConcSuccesForSampleKind( kind, repCount, calcData, measureProtocol, analysisErrorInfoList );
					}
					#endregion
				}

				#endregion
			}

			// キャリブレータの場合、濃度値を登録データより取得
			if ( kind == SampleKind.Calibrator )
			{
				//calcData.CalcInfoReplication.Concentration = Double.Parse( HybridDataMediator.SearchConcentrationFromCalibRegistDB( calcData.ProtocolIndex, calcData.RackID, calcData.RackPosition.Value ) );
                calcData.CalcInfoReplication.Concentration = Double.Parse(Singleton<CalibratorAssayDB>.Instance.GetAssayConcentration(calcData.UniqueNo));
               
                // 定量項目の場合のみ
				if ( measureProtocol.CalibType.IsQuantitative() )
				{
                    //如果是任意校准的点且这个点不在MasterCurfe曲线上，不错这种偏高或偏低值得判断（暂时）
                    //以后取其相邻点的值判断
                    int nIndexCheckRange = 0;
                    bool bCheck = false;
                     
                    List<CalibrationCurveData> list = Singleton<CalibrationCurveDB>.Instance.GetMasterCurveData(measureProtocol.ProtocolIndex, calcData.ReagentLotNo);
                    double []concs = new double[list.Count];
                    if (list.Count != 0)
                    {                       
                        for (int i = 0; i < list.Count; i++)
                        {
                            concs[i] = double.Parse(list[i].Concentration);
                        }
                    }
                    else
                    {
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "<$*$ Calculator $*$> Caribrator Curfe is empty!");
                        return false;
                    }
                    //for (int i = 0; i < measureProtocol.ConcsOfEach.Count();i++ )
                    for (int i = 0; i < concs.Length; i++)
                    {
                        //if (calcData.CalcInfoReplication.Concentration <= measureProtocol.ConcsOfEach[i])
                        if (calcData.CalcInfoReplication.Concentration <= concs[i])
                        {
                            nIndexCheckRange = i;
                            bCheck = true;
                            break;
                        }
                    }
                    //如果比最大的值大，用最大值的Index
                     if (bCheck == false)
                     {
                         //nIndexCheckRange = measureProtocol.ConcsOfEach.Count() - 1;
                         nIndexCheckRange = concs.Length - 1;
                     }
					// カウント値範囲チェック
					//var countCheckRange = measureProtocol.CountRangesOfEach[Array.IndexOf( measureProtocol.ConcsOfEach, calcData.CalcInfoReplication.Concentration )];
                     var countCheckRange = measureProtocol.CountRangesOfEach[nIndexCheckRange];

					if ( calcData.CalcInfoReplication.CountValue > countCheckRange.Max )
					{
						AddErrorInfo( AnalysisError.CalibHighError, analysisErrorInfoList, calcData.CalcInfoReplication.Remark );
					}
					else if ( calcData.CalcInfoReplication.CountValue < countCheckRange.Min )
					{
						AddErrorInfo( AnalysisError.CalibLowError, analysisErrorInfoList, calcData.CalcInfoReplication.Remark );
					}
				}
			}

			#endregion

			#region _計算処理(平均)_

			#region ___平均算出用同一分析全レプリケーション結果取得___

			// 平均算出用(1分析中の全多重測定)
			List<CalcData> replicationList = null;

			var nowData = calcData;
			// 同一分析の多重測定中のレプリケーションがすべて終了の場合
			// 計算データクラス生成(一般/優先・精度管理)
			// (平均算出用に最終レプリケーションの計算済みの測定結果を含める)
			switch ( kind )
			{
			case SampleKind.Sample:
			case SampleKind.Priority:
				var SpecimenAssayDataList = HybridDataMediator.SearchHasCountSpecimenAssayData( nowData.UniqueNo );
				if ( SpecimenAssayDataList.
					All( ( data ) => 
						data.ReplicationNo == nowData.ReplicationNo 
						|| data.GetStatus() == SampleInfo.SampleMeasureStatus.End 
						|| data.GetStatus() == SampleInfo.SampleMeasureStatus.Error )
					&& SpecimenAssayDataList.Count() == repCount)
				{
					replicationList = SpecimenAssayDataList.Select( ( data ) => ( data.ReplicationNo == nowData.ReplicationNo ) ? nowData :
																createCalcData( data.GetModuleNo()
                                                                , data.GetMeasureProtocolIndex()
																, data.ReagentLotNo
																, data.GetIndividuallyNo()
																, data.GetUniqueNo()
																, data.ReplicationNo
																, data.ManualDilution.Value
																, data.AutoDilution.Value
																, data.MeasureDateTime.Value
																, data.RackId
																, data.RackPosition
																, data.GetCount()
																, data.GetRemarkId() ) 
																).ToList();
				}
				break;
			case SampleKind.Control:
				var controlAssayDataList = HybridDataMediator.SearchHasCountControlAssayData( nowData.UniqueNo );
				if ( controlAssayDataList.All( ( data ) => 
													data.ReplicationNo == nowData.ReplicationNo 
													|| data.GetStatus() == SampleInfo.SampleMeasureStatus.End 
													|| data.GetStatus() == SampleInfo.SampleMeasureStatus.Error )
					 && controlAssayDataList.Count() == measureProtocol.RepNoForControl )
				{
					replicationList = controlAssayDataList.Select( ( data ) => ( data.ReplicationNo == nowData.ReplicationNo ) ? nowData :
															createCalcData( data.GetModuleNo()
                                                            , data.GetMeasureProtocolIndex()
															, data.ReagentLotNo
															, data.GetIndividuallyNo()
															, data.GetUniqueNo()
															, data.ReplicationNo
															, 1
															, 1
															, data.MeasureDateTime.Value
															, data.RackId
															, data.RackPosition
															, data.GetCount()
															, data.GetRemarkId() ) 
															).ToList();
				}
				break;
			case SampleKind.Calibrator:
				var calibratorAssayDataList = HybridDataMediator.SearchHasCountCalibratorAssayData( nowData.UniqueNo );
				if ( calibratorAssayDataList.All( ( data ) => 
														data.ReplicationNo == nowData.ReplicationNo 
														|| data.GetStatus() == SampleInfo.SampleMeasureStatus.End 
														|| data.GetStatus() == SampleInfo.SampleMeasureStatus.Error ) 
					&& calibratorAssayDataList.Count() == measureProtocol.RepNoForCalib )
				{
					replicationList = calibratorAssayDataList.Select( ( data ) => ( data.ReplicationNo == nowData.ReplicationNo ) ? nowData :
																	createCalcData( data.GetModuleNo()
                                                                    , data.GetMeasureProtocolIndex()
																	, data.ReagentLotNo
																	, data.GetIndividuallyNo()
																	, data.GetUniqueNo()
																	, data.ReplicationNo
																	, 1
																	, 1
																	, data.MeasureDateTime.Value
																	, data.RackId
																	, data.RackPosition
																	, data.GetCount()
																	, data.GetRemarkId() ) 
																	).ToList();
                    // 对每个值进行附值，防止CV计算时剔除Count偏差最大的项目（有浓度值的项）导致的错误
                    foreach (CalcData item in replicationList)
                    {
                        item.CalcInfoReplication.Concentration = nowData.CalcInfoReplication.Concentration;
                    }
				}
				break;
			}

			#endregion

			if ( replicationList != null && replicationList.Count > 1 )
			{
				// 平均用エラー
				List<AnalysisErrorInfo> analysisErrorInfoListAve = new List<AnalysisErrorInfo>();

				#region ___平均カウント算出、平均濃度算出___

				// 平均カウント値算出
				Remark remark = new Remark();
				var lastReplicatioNo = replicationList.Max( ( data ) => data.ReplicationNo );
				averageCalcData = ( nowData.ReplicationNo == lastReplicatioNo ) ? nowData : replicationList.First( ( data ) => data.ReplicationNo == lastReplicatioNo );
                if (kind == SampleKind.Calibrator && measureProtocol.UseCVIndependence)
                {
                    averageCalcData.CalcInfoAverage = new CalcInfo(CarisXCalculator.CalcAveCount(measureProtocol, replicationList, ref analysisErrorInfoList, ref remark,true));
                }
                else
                {
				    averageCalcData.CalcInfoAverage = new CalcInfo( CarisXCalculator.CalcAveCount( measureProtocol, replicationList, ref analysisErrorInfoList, ref remark ) );
                }
				averageCalcData.CalcInfoAverage.Remark.AddRemark( remark );

				// 平均カウント値がある場合
				if ( averageCalcData.CalcInfoAverage.CountValue != null
					&& !replicationList.Exists( ( data ) =>
						data.CalcInfoReplication.Remark != null
						&& data.CalcInfoReplication.Remark.HasRemark( Remark.RemarkBit.CalibrationCurveError ) ) )
				{
					// 平均濃度計算
					if ( CarisXCalculator.CalcConc( measureProtocol, kind, null, averageCalcData, true, analysisErrorInfoListAve ) )
					{
						CalcConcSuccesForSampleKind( kind, repCount, averageCalcData, measureProtocol, analysisErrorInfoList, true );
					}
				}

				// 全レプリのリマークをOR
				foreach ( var rep in replicationList )
				{
					averageCalcData.CalcInfoAverage.Remark.AddRemark( (Remark)rep.CalcInfoReplication.Remark.Value );
				}

				#endregion

			}

			#endregion

            #region _分析ステータス更新_ Analysis of status updates

            if ( calcData.CalcInfoReplication.Remark.IsNeedReMeasure )
			{
				calcData.AssayStatus = SampleInfo.SampleMeasureStatus.Error;
				Singleton<InProcessSampleInfoManager>.Instance.SearchInProcessSampleFromUniqueNo( calcData.UniqueNo )
					.SetMeasureProtocolStatus( calcData.ProtocolIndex, calcData.ReplicationNo, SampleInfo.SampleMeasureStatus.Error );
			}
			else
			{
                calcData.AssayStatus = SampleInfo.SampleMeasureStatus.End;
                Singleton<InProcessSampleInfoManager>.Instance.SearchInProcessSampleFromUniqueNo( calcData.UniqueNo )
                    .SetMeasureProtocolStatus( calcData.ProtocolIndex, calcData.ReplicationNo, SampleInfo.SampleMeasureStatus.End );
			}

			#endregion

			Singleton<CarisXLogManager>.Instance.WriteCommonLog( LogKind.DebugLog, "<$*$ Calculator $*$> end Calc" );
			return true;
		}

		/// <summary>
		/// 測光エラーのチェック
		/// </summary>
		/// <remarks>
		/// 測光値をチェックし、測光エラー時にエラーリストにエラーを、計算情報にリマークを追加します。</br>
		/// 測光値：非Calibratorは80以下/Calibratorは50以下
		/// </remarks>
		/// <param name="measuredCount">実測カウント値</param>
		/// <param name="kind">検体種別</param>
		/// <param name="analysisErrorInfoList">エラーリスト</param>
		/// <param name="calcInfo">測光エラーチェック対象計算情報</param>
		/// <returns>チェック結果(true:測光エラー/false:エラー無)</returns>
		public static bool CheckMeasureError( Int32 measuredCount,Int32 blankCount,Int32 protocolIndex, SampleKind kind, ref List<AnalysisErrorInfo> analysisErrorInfoList, ref CalcInfo calcInfo )
		{
			// 測光上限値の取得
			Int32 limitCount = 0;
            Int32 measureLimitCount = 0;
			switch ( kind )
			{
                case SampleKind.Calibrator:
                    {
                        limitCount = CarisXConst.MEASURE_LIMIT_MAX_CALIB;
                        measureLimitCount = measuredCount;
                        break;
                    }
                default:
                    {
                       
                        limitCount = CarisXConst.MEASURE_LIMIT_MAX;
                        measureLimitCount = measuredCount;
                        break;
                    }
			}

            if(measureLimitCount == CarisXConst.ERRORCHECK_LIMIT_MAX && blankCount == CarisXConst.ERRORCHECK_LIMIT_MAX)
            {
                return true;
            }
			// 測光上限値超過の場合
            if (measureLimitCount <= limitCount)
			{
				// 測光値のエラー追加
                AnalysisErrorInfo errorInfo = AddErrorInfo(AnalysisError.PhotometryError, analysisErrorInfoList, calcInfo.Remark);
                String str = Oelco.CarisX.Properties.Resources.STRING_ASSAY_045 + "：" + measuredCount.ToString() + "，" + Properties.Resources_Maintenance.STRING_MAINTENANCE_TRIGGERDISP_064 + "：" + blankCount.ToString();
                errorInfo.EditInfo(null, null, null, null, null, new String[] {str}.ToList());
				// キャリブレータの場合
				if ( kind == SampleKind.Calibrator )
				{
					// 測定カウント値無効
					calcInfo.CountValue = null;
				}

				Singleton<CarisXLogManager>.Instance.WriteCommonLog( LogKind.DebugLog, "<$*$ Calculator $*$> 測光Error" );
				return true;
			}

			return false;
		}

		/// <summary>
		/// ダークエラーのチェック
		/// </summary>
		/// <remarks>
		/// ダーク値をチェックし、ダークエラー時にエラーリストにエラーを、計算情報にリマークを追加します。
		/// </remarks>
		/// <param name="dark">ダーク値</param>
		/// <param name="analysisErrorInfoList">エラーリスト</param>
		/// <param name="calcInfo">ダーク値チェック対象計算情報</param>
		/// <returns>true:ダークエラー</returns>
		/// <remarks>ダーク値：100以上時</remarks>
		public static bool CheckDarkError( Int32 dark, ref List<AnalysisErrorInfo> analysisErrorInfoList, ref CalcInfo calcInfo )
		{
			// ダーク値閾値以下の場合
			if ( dark >= CarisXConst.DARK_LIMIT_MIN )
			{
				// ダーク値エラー追加
				AddErrorInfo( AnalysisError.DarkError, analysisErrorInfoList, calcInfo.Remark );
                Singleton<CarisXLogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "<$*$ Calculator $*$> Dark error");
				return true;
			}

			return false;
		}

		#endregion

		#region _判定_

		/// <summary>
		/// 判定結果文字列の取得(定性項目)
		/// </summary>
		/// <remarks>
		/// 一般検体(優先検体含む)の定性項目の陽性/陰性の判定を行います。
		/// </remarks>
		/// <param name="measureProtocol">分析項目</param>
		/// <param name="concentration">判定対象濃度</param>
		/// <param name="kind">検体種別</param>
		/// <returns>判定結果文字列</returns>
		public static String GetJudgementString( MeasureProtocol measureProtocol, Double? concentration, SampleKind kind )
		{
			// 精度管理検体･キャリブレータは判定不要
			if ( kind == SampleKind.Sample || kind == SampleKind.Priority )
			{
                //【IssuesNo:20】Innodx要求增加定量项目36（HBsAb）、37（HBsAg）的阴阳性判定,为了兼容以前的项目版本，当该项目的判定值都大于0才能进行判定
                Double dbZero = 0.000001;
                Boolean bQuanJudgement = false;
                if((measureProtocol.ProtocolIndex.Equals(36) || measureProtocol.ProtocolIndex.Equals(37)) &&
                    (measureProtocol.PosiLine - 0.0 > dbZero) &&
                    (measureProtocol.NegaLine - 0.0 > dbZero))
                {
                    bQuanJudgement = true;
                }

				// 定性項目の場合
				if ( measureProtocol.CalibType.IsQualitative() || bQuanJudgement)
				{
					if ( concentration.HasValue )
					{
						// 判定
						return "(" + Judgement( measureProtocol, concentration.Value ).ToTypeString() + ")";
					}
				}
			}

			return String.Empty;
		}

		#endregion

		#region _カウント値取得_

		/// <summary>
		/// カウント値算出
		/// </summary>
		/// <remarks>
		/// カウント値を計算します。
		/// </remarks>
		/// <param name="measuredCount">測光値(80以上必須)</param>
		/// <param name="blank">ブランク値</param>
		/// <returns>カウント値</returns>
		public static Int32 CalcCount( Int32 measuredCount, Int32 blank, Int32 moduleIndex )
		{
			// カウント値計算開始
			Singleton<CarisXLogManager>.Instance.WriteCommonLog( LogKind.DebugLog, "<$*$ Calculator $*$> start CalcCount" );

			// カウント値算出(計算式：(測光値-ブランク値)*装置補正係数　の四捨五入結果)
			var count = (Int32)Math.Round( ( measuredCount - blank ) 
                * Singleton<ParameterFilePreserve<CarisXMotorParameter>>.Instance.Param.SlaveList[moduleIndex].instrumentCoef.InstrumentCoefficient
                , MidpointRounding.AwayFromZero );

			// カウント値が0以上の場合(正常)
			if ( count >= 0 )
			{
				Singleton<CarisXLogManager>.Instance.WriteCommonLog( LogKind.DebugLog, "<$*$ Calculator $*$> end CalcCount Count>=0" );
				return count;
			}

			// カウント値が0未満の場合(エラー)
			Singleton<CarisXLogManager>.Instance.WriteCommonLog( LogKind.DebugLog, "<$*$ Calculator $*$> end CalcCount count<0" );
			return 0;
		}

		#endregion

		#region _濃度計算_

		/// <summary>
		/// 濃度計算メソッドクラスの取得
		/// </summary>
		/// <remarks>
		/// キャリブレーションタイプ別の濃度計算処理インスタンスの取得
		/// </remarks>
		/// <param name="measureProtocol">分析項目</param>
		/// <param name="calibPoints">キャリブポイント</param>
		/// <param name="extendValue">拡張パラメータ値リスト</param>
		/// <returns>濃度計算メソッド</returns>
		public static ICalcMethod GetCalcMethod( MeasureProtocol measureProtocol, IEnumerable<ItemPoint> calibPoints,Boolean reLogitLog, IEnumerable<Double> extendValue = null )
		{
			ICalcMethod method = null;

			switch ( measureProtocol.CalibType )
			{
			case MeasureProtocol.CalibrationType.LogitLog:
				LogitLogMethod logitLogMethod = new LogitLogMethod( CarisXConst.LOGITLOG_COEF_A, CarisXConst.LOGITLOG_COEF_B );
                logitLogMethod.CalcuCoef(calibPoints.Select((point) => point.xPos).ToArray(), calibPoints.Select((point) => point.yPos).ToArray(), calibPoints.Count(), reLogitLog);
				method = logitLogMethod;
				break;

			case MeasureProtocol.CalibrationType.Spline:
				SplineMethod splineMethod = new SplineMethod();
				splineMethod.CalcuCoef( calibPoints.Select( ( point ) => point.xPos ).ToArray(), calibPoints.Select( ( point ) => point.yPos ).ToArray(), calibPoints.Count() );
				method = splineMethod;
				break;

			case MeasureProtocol.CalibrationType.FourParameters:
				if ( extendValue != null && extendValue.Count() == FourParameterMethod.PARAMETER_COUNT )
				{
					FourParameterMethod fourParameterMethod = new FourParameterMethod( calibPoints.Count(), extendValue.ElementAt( 0 ), extendValue.ElementAt( 1 ), extendValue.ElementAt( 2 ), extendValue.ElementAt( 3 ) );
					fourParameterMethod.SetData( calibPoints.Select( ( point ) => point.xPos ).ToArray(), calibPoints.Select( ( point ) => point.yPos ).ToArray(), calibPoints.Count() );

                    //设置4参数法的加权类型和K值  by marxsu
                    fourParameterMethod.FourPType = (FourPType)measureProtocol.FourPrameterMethodType;
                    if ((FourPType)measureProtocol.FourPrameterMethodType == FourPType.str1Y_K)
                    {
                        fourParameterMethod.ValueK = measureProtocol.FourPrameterMethodKValue;
                    }
					method = fourParameterMethod;
				}
				break;

			case MeasureProtocol.CalibrationType.INH:
				InhMethod inhMethod = new InhMethod( calibPoints.ElementAt( 1 ).yPos, calibPoints.ElementAt( 0 ).yPos, measureProtocol.Coef_A, measureProtocol.Coef_B, measureProtocol.Coef_C, measureProtocol.Coef_D, measureProtocol.Coef_E );
				method = inhMethod;
				break;

			case MeasureProtocol.CalibrationType.CutOff:
				CutOffMethod cutOffMethod = new CutOffMethod( calibPoints.ElementAt( 1 ).yPos, calibPoints.ElementAt( 0 ).yPos, measureProtocol.Coef_A, measureProtocol.Coef_B, measureProtocol.Coef_C, measureProtocol.Coef_D, measureProtocol.Coef_E );
				method = cutOffMethod;
				break;

			case MeasureProtocol.CalibrationType.DoubleLogarithmic1:
				DoubleLogarithmicMethod doubleLog1method = new DoubleLogarithmicMethod( DoubleLogarithmicMethod.Dimention.One );
				doubleLog1method.CalcuCoef( calibPoints.Select( ( point ) => point.xPos ).ToArray(), calibPoints.Select( ( point ) => point.yPos ).ToArray(), calibPoints.Count() );
				method = doubleLog1method;
				break;

			case MeasureProtocol.CalibrationType.DoubleLogarithmic2:
				DoubleLogarithmicMethod doubleLog2method = new DoubleLogarithmicMethod( DoubleLogarithmicMethod.Dimention.Two );
				doubleLog2method.CalcuCoef( calibPoints.Select( ( point ) => point.xPos ).ToArray(), calibPoints.Select( ( point ) => point.yPos ).ToArray(), calibPoints.Count() );
				method = doubleLog2method;
				break;
			}

			return method;
		}

		#endregion

		#region _検量線作成_

		/// <summary>
		/// 検量線作成
		/// </summary>
		/// <remarks>
		/// 検量線の作成を行います。
		/// </remarks>
		/// <param name="measureProtocolIndex">分析項目インデックス</param>
		/// <param name="individuallyNo">検体識別番号</param>
		/// <param name="analysisErrorInfoList">エラーリスト</param>
		public static void CreateCalibCurveData( Int32 measureProtocolIndex, CarisXIDString rackId, Int32 rackPosition, Int32 SequenceNo, out List<AnalysisErrorInfo> analysisErrorInfoList )
        {
			analysisErrorInfoList = new List<AnalysisErrorInfo>();
			MeasureProtocol measureProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( measureProtocolIndex );

			// キャリブレータ測定結果取得(分析項目、ラックID、ラックポジション)
            IEnumerable<CalibratorResultData> resultData = (from v in HybridDataMediator.SearchCalibResultDataFromCalibResultDb(measureProtocolIndex,SequenceNo)
                                                            orderby double.Parse(v.GetConcentration()), v.RackId.Value, v.RackPosition, v.ReplicationNo
                                                            where !v.GetIsCreatedCalibCurve() && v.SequenceNo == SequenceNo
                                                            select v).ToList();       

			// TODO:重複する検量線未作成データのチェック

			//var resultDataAll = from v in HybridDataMediator.SearchCalibResultDataFromCalibResultDBAndCalibRegistDB( measureProtocol.ProtocolIndex, rackId, rackPosition )
			//                 orderby v.RackId.Value, v.RackPosition, v.MeasureDateTime descending
			//                 where !v.GetIsCreatedCalibCurve()
			//                 group v by new
			//                 {
			//                     rackId = v.RackId.Value,
			//                     pos = v.RackPosition
			//                 } into grp
			//                 select grp;

			//// キャリブレータ測定結果取得(分析項目、ラックID、ラックポジション)
			//var resultDataAllSelect = resultDataAll.Select( ( grp ) => grp.FirstOrDefault() );
			//if ( !resultDataAll.All( ( data ) => data != null ) )
			//{
			//    return;
			//}

			//IEnumerable<CalibratorResultData> resultData = ( resultDataAllSelect ).ToList();
            //測定結果の数とキャリブポイント*リプリケーション数の確認

			if ( resultData.Count() < measureProtocol.NumOfMeasPointInCalib * measureProtocol.RepNoForCalib )
			{
				return;
			}           
            
                       
            Singleton<CarisXLogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "<$*$ CreateCalibCurve $*$> Calib*Reppoint OK");

			// 1検量線の分析状態取得(終了しているかどうか)
            var processStatus = Singleton<InProcessSampleInfoManager>.Instance.InProcessSampleList
                .Where((info) =>
                    resultData.Select((data) =>
                        data.SequenceNo).Contains(info.SequenceNumber))
                        .SelectMany((sInfo) => sInfo.GetMeasureProtocolStatusList(measureProtocolIndex));

			// 1検量線の分析にエラーが存在する
			if ( processStatus.Contains( SampleInfo.SampleMeasureStatus.Error ) || processStatus.All( ( status ) => status == SampleInfo.SampleMeasureStatus.End ) )
			{
				// 検量線作成済みフラグ(未作成→作成済)
				foreach ( var data in resultData )
				{
					data.SetIsCreatedCalibCurve( true );
				}

                Singleton<CarisXLogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "<$*$ CreateCalibCurve $*$> SetIsCreatedCalibCurve OK");

				Singleton<CalibratorResultDB>.Instance.SetData( resultData.ToList() );
				Singleton<CalibratorResultDB>.Instance.CommitData();
			}

            // 1検量線の分析状態が全て正常終了している場合
			if ( processStatus.All( ( status ) => status == SampleInfo.SampleMeasureStatus.End ) )
			{
                Singleton<CarisXLogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "<$*$ CreateCalibCurve $*$> processStatus.All OK");

				// キャリブレータ測定結果DBよりリマーク取得(レプリ、平均)
				var testData = resultData.GroupBy( ( repData ) => repData.GetUniqueNo());
                              
				// 検量線登録実施可能チェック
				if ( CheckCalibrationCurveRegist( measureProtocol, testData ) )
				{

                    Singleton<CarisXLogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "<$*$ CreateCalibCurve $*$> CheckCalibrationCurveRegist OK");

					String fullCalibration = ( measureProtocol.CalibMethod == MeasureProtocol.CalibrationMethod.FullCalibration ) 
						? Properties.Resources.STRING_ERROR_MESSAGE_005 
						: String.Empty;
					Func<ItemPoint[]> getCurvePoint;

                    var resultDataPoint = (from data in resultData
                                           let count = ((data.ReplicationNo > 1) ? data.GetCountAve() : data.GetCount())
                                           let conc = (data.ReplicationNo > 1) ? SubFunction.SafeParseDouble(data.GetConcentrationAve()) : SubFunction.SafeParseDouble(data.GetConcentration())
                                           where data.ReplicationNo == measureProtocol.RepNoForCalib && count.HasValue
                                           select new ItemPoint((Double)count, conc)
                                           ).OrderBy(data => data.xPos).ToArray();                   

					var reagentLotNo = resultData.First().ReagentLotNo;

                    int CalibrationWarningLevel = 0;//0 is normal

					getCurvePoint = GetCreateCurvePoint( measureProtocol, resultDataPoint, reagentLotNo,ref CalibrationWarningLevel);

                    if (CalibrationWarningLevel == 1)
                    {
                        string strPlus = string.Format("Warning:protocol {0} lot{1}  calibration count monotonicity  is diffent", measureProtocol.ProtocolName, reagentLotNo);

                        DPRErrorCode errCode = new DPRErrorCode(300, 1);

                        // エラー履歴に登録
                        CarisXSubFunction.WriteDPRErrorHist(errCode, 0, strPlus);
                    }
                    else if (CalibrationWarningLevel == 2)
                    {
                        string strPlus = string.Format("Error:protocol {0} lot{1}  calibration count monotonicity  is diffent,deviation is out of 20%", measureProtocol.ProtocolName, reagentLotNo);

                        DPRErrorCode errCode = new DPRErrorCode(300, 2);

                        // エラー履歴に登録
                        CarisXSubFunction.WriteDPRErrorHist(errCode, 0, strPlus);
                    }

                    var registCurveError = analysisErrorInfoList;
                    var failed = (Action<String>)((detailInfoMessage) =>
                    {
                        AnalysisErrorInfo analysisErrorInfo = new AnalysisErrorInfo();
                        analysisErrorInfo.EditInfo(errorCode: 34, errorArg: 1, errorDetailInfoParam: new List<String>() { detailInfoMessage });
                        registCurveError.Add(analysisErrorInfo);
                    });


                    // 发光值的平均值不能<=0 

                    if (resultData.Any((data) => data.GetCountAve() <= 0))
                    {
                        string strPlus = string.Format("Error:protocol {0} lot{1}  calibration countAve <= 0", measureProtocol.ProtocolName, reagentLotNo);

                        DPRErrorCode errCode = new DPRErrorCode(300, 2);

                        // エラー履歴に登録
                        CarisXSubFunction.WriteDPRErrorHist(errCode, 0, strPlus);

                        failed("calibration countAve <= 0");

                        return;
                    }
                   

					// 登録検量線情報取得可能な場合
					if ( getCurvePoint != null )
					{
						// 同一試薬ロットのみの測定結果である場合
                        
						//if ( resultData.Select( ( data ) => data.ReagentLotNo ).Distinct().Count() == 1 )
                        //ReagentLotNo !=string.Empty排除没有出结果的情况，如R试剂分注失败等原因造成
                        if (resultData.Where((data) => data.ReagentLotNo != string.Empty).Select((data1) => data1.ReagentLotNo).Distinct().Count() ==1)
						{                           
							var pointCureve = getCurvePoint();                             
							if ( pointCureve.Count() > 0 )
							{
								// 取得された検量線情報の全ポイントが存在する場合
								if ( pointCureve.All( ( point ) => point != null ) )
								{
									// 単調増加 or 単調減少を判定
									if ( JudgeCurveEntry( pointCureve, measureProtocol ) )
									{
                                        Singleton<CarisXLogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "<$*$ CreateCalibCurve $*$> JudgeCurveEntry OK");

										// 検量線データ保存
										Int32 pointNo = 1;
										DateTime approvalDateTime = DateTime.Now;
										List<String> extParams = GetLastExtParam();
										Int32 measurePointDataIndex = 0;
                                        Int32 moduleNo = resultData.FirstOrDefault().GetModuleNo();
                                        foreach ( var conc in pointCureve.Select( ( point ) => point.xPos ) )
										{
											for ( int repNo = 1; repNo <= measureProtocol.RepNoForCalib; repNo++ )
											{
												var result = resultData.ElementAtOrDefault( measureProtocol.RepNoForCalib * measurePointDataIndex + repNo - 1 );

												// 多重内の特定レプリに濃度****が存在するとパースに失敗する問題に対応
												if ( result != null )
												{
													Double tempValue = 0d;
													Boolean isDouble = Double.TryParse( result.GetConcentration(), out tempValue );
													if ( result.ReplicationNo != repNo || !isDouble || tempValue != conc )
													{
                                                        Singleton<CarisXLogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "<$*$ CreateCalibCurve $*$> result = null");
														result = null;
													}
												}

												if ( result != null )
												{// 測定ポイント
													Singleton<CalibrationCurveDB>.Instance.AddCalibData( moduleNo
                                                        , measureProtocol.ReagentCode
                                                        , reagentLotNo
														, result.RackId
														, result.RackPosition
														, measureProtocol.ProtocolIndex
														, repNo
														, SubFunction.TruncateParse( conc, measureProtocol.LengthAfterDemPoint )
														, result.GetCount()
														, pointNo
														, result.GetCountAve()
														, result.GetUniqueNo()
                                                        , approvalDateTime
                                                        
                                                        , extParams );
													if ( repNo == measureProtocol.RepNoForCalib )
													{
														measurePointDataIndex++;
													}
												}
												else if ( measureProtocol.CalibMethod == MeasureProtocol.CalibrationMethod.MasterCalibration )
												{// 補正ポイント
													Int32? countAve = null;
													if ( repNo == measureProtocol.RepNoForCalib && repNo > 1 )
													{
														countAve = (Int32?)pointCureve.SingleOrDefault( ( point ) => point.xPos == conc ).yPos;
													}
													Singleton<CalibrationCurveDB>.Instance.AddCalibData( moduleNo
                                                        , measureProtocol.ReagentCode
														, reagentLotNo
														, null
														, null
														, measureProtocol.ProtocolIndex
														, repNo
														, SubFunction.TruncateParse( conc, measureProtocol.LengthAfterDemPoint )
														, (Int32?)pointCureve.SingleOrDefault( ( point ) => point.xPos == conc ).yPos
														, pointNo
														, countAve
														, 0
														, approvalDateTime
                                                        , extParams );
												}
											}
											pointNo++;
										}
                                        Singleton<CarisXLogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "<$*$ CreateCalibCurve $*$> DB Write");
										Singleton<CalibrationCurveDB>.Instance.CommitData();
									}
									else
									{
										// 単調増加or単調減少判定に失敗()
										Singleton<CarisXLogManager>.Instance.WriteCommonLog( LogKind.DebugLog, "<$*$ Calculator $*$> JudgeCurveEntry:false" );
										failed( Properties.Resources.STRING_ERROR_MESSAGE_000 + fullCalibration );
									}
								}
								else
								{
									// マスターキャリブレーション補正に失敗
									Singleton<CarisXLogManager>.Instance.WriteCommonLog( LogKind.DebugLog, "<$*$ Calculator $*$> GetAdjustCurve:ItemPoint[] has Null point." );
									failed( Properties.Resources.STRING_ERROR_MESSAGE_000 + Properties.Resources.STRING_ERROR_MESSAGE_001 + fullCalibration );
								}
							}
							else
							{
								// 算出した測定ポイント数が0ポイントとなり失敗
								Singleton<CarisXLogManager>.Instance.WriteCommonLog( LogKind.DebugLog, "<$*$ Calculator $*$> GetAdjustCurve:Reagent lot is present various." );
								failed( Properties.Resources.STRING_ERROR_MESSAGE_000 + Properties.Resources.STRING_ERROR_MESSAGE_002 + fullCalibration );
							}
						}
						else
						{
							// キャリブレータ測定結果情報(同一分析項目、同一シーケンス番号)に異なる試薬ロット番号(非マスタ)があり失敗
							Singleton<CarisXLogManager>.Instance.WriteCommonLog( LogKind.DebugLog, "<$*$ Calculator $*$> GetAdjustCurve:Reagent lot is present various." );
							failed( Properties.Resources.STRING_ERROR_MESSAGE_000 + Properties.Resources.STRING_ERROR_MESSAGE_003 + fullCalibration );
						}
					}
					else
					{
						// マスターカーブが存在しない
						Singleton<CarisXLogManager>.Instance.WriteCommonLog( LogKind.DebugLog, "<$*$ Calculator $*$> MasterCurve is not exist!" );
						failed( Properties.Resources.STRING_ERROR_MESSAGE_000 + Properties.Resources.STRING_ERROR_MESSAGE_006 + fullCalibration );
					}
				}
			}
		}

		/// <summary>
		/// キャリブレーション方法別検量線作成の取得
		/// </summary>
		/// <remarks>
		/// カプセル化検量線ポイント取得メソッドを取得します。
		/// </remarks>
		/// <param name="measureProtocol">分析項目</param>
		/// <param name="resultDataPoint">測定結果ポイントデータ</param>
		/// <param name="reagentLotNo">試薬ロット</param>
		/// <returns>検量線ポイントデータ</returns>
		public static Func<ItemPoint[]> GetCreateCurvePoint( MeasureProtocol measureProtocol, ItemPoint[] resultDataPoint, string reagentLotNo,ref int CalibrationWarningLevel)
		{
            int nPointTrend =0;
			Func<ItemPoint[]> getCurvePoint;
			getCurvePoint = null;
			if ( measureProtocol.CalibType.IsQuantitative() )
			{
				switch ( measureProtocol.CalibMethod )
				{
				case MeasureProtocol.CalibrationMethod.FullCalibration:
					getCurvePoint = () => resultDataPoint;
					break;
				case MeasureProtocol.CalibrationMethod.MasterCalibration:
					// マスターカーブの取得
					var masterCurve = HybridDataMediator.SearchCalibCurveFromCalibCurveDB( measureProtocol.ProtocolIndex, reagentLotNo, CarisXConst.MASTER_CURVE_DATE );

					if ( masterCurve.Count() > 0 )
					{
                            // 結果から検量線作成
                            var masterCurvePoint = masterCurve.Select((data) => new ItemPoint(data.Count.Value, Double.Parse(data.Concentration))).ToArray();

                            //// マスターカーブとキャリブ測定結果の各ポイントの濃度値が1つ以上異なる場合、マスターカーブ無しとなり補正失敗
                            //if ( resultDataPoint.All( ( resPoint ) => masterCurvePoint.Select( ( point ) => point.xPos ).Contains( resPoint.xPos ) ) )
                            //{
                            //    getCurvePoint = () => GetAdjustCurve( resultDataPoint, masterCurvePoint );
                            //}
                            List<ItemPoint> itemList = new List<ItemPoint>();
                            List<ItemPoint> modifyResultDataList = new List<ItemPoint>();

                            if (!JudgeCurveEntry(masterCurvePoint, measureProtocol))
                            {
                                // 単調増加or単調減少判定に失敗()
                                Singleton<CarisXLogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "<$*$ Calculator $*$> JudgeCurveEntry:false");
                                // failed(Properties.Resources.STRING_ERROR_MESSAGE_000 + fullCalibration);
                            }
                            List<double> dLastExtParam = new List<double>();
                            //lastExtParam = new List<String>();
                            // 4Parameterの算出された係数はDBへ保存を行う為、判定の際に保持する。
                            if (lastExtParam != null && lastExtParam.Count == 4)//只有四参数法才有这组参数
                            {
                                for (int i = 0; i < lastExtParam.Count; i++)
                                {
                                    dLastExtParam.Add(double.Parse(lastExtParam[i]));
                                }
                            }                           
                          
                            ICalcMethod method = GetCalcMethod(measureProtocol, masterCurvePoint, false, dLastExtParam.ToArray());
                            
                            //add the free points to the master points
                            int j = 0;
                            for (int i = 0; i < masterCurvePoint.Count(); i++)
                            {
                                if (j < resultDataPoint.Count())
                                {
                                    //Double dConc = double.Parse(registDataList[j].Concentration);
                                    if (resultDataPoint[j].xPos > masterCurvePoint[i].xPos)
                                    {
                                    }
                                    else if (resultDataPoint[j].xPos == masterCurvePoint[i].xPos)//remove the same point
                                    {

                                        if (resultDataPoint[j].yPos >= masterCurvePoint[i].yPos)
                                        {
                                            ++nPointTrend;
                                        }
                                        else
                                        {
                                            --nPointTrend;
                                        }
                                        j++;
                                    }
                                    else 
                                    {
                                        double y = method.GetY(resultDataPoint[j].xPos);
                                        if (itemList.Count > 0 && itemList[itemList.Count - 1].xPos != resultDataPoint[j].xPos)
                                        {                                           
                                            itemList.Add(new ItemPoint(y, resultDataPoint[j].xPos));
                                        }                                       
                                        if (resultDataPoint[j].yPos >= y)
                                        {
                                            ++nPointTrend;
                                        }
                                        else
                                        {
                                            --nPointTrend;
                                        }
                                        j++;
                                    }
                                }
                                if (itemList.Count > 0 && itemList[itemList.Count -1].xPos !=masterCurvePoint[i].xPos)
                                {
                                    itemList.Add(masterCurvePoint[i]);
                                }
                                else
                                {
                                    itemList.Add(masterCurvePoint[i]);
                                }
                                                                
                            }
                                             

                            getCurvePoint = () => GetAdjustCurve(resultDataPoint, itemList.ToArray());
                        }
					break;
				default:
					break;
				}
			}
			else if ( measureProtocol.CalibType.IsQualitative() )
			{
				getCurvePoint = () => resultDataPoint;
			}
			return getCurvePoint;
		}

		/// <summary>
		/// 最終算出拡張パラメータの取得
		/// </summary>
		/// <remarks>
		/// 最終算出拡張パラメータを取得します。
		/// </remarks>
		/// <returns>最終算出拡張パラメータ</returns>
		public static List<String> GetLastExtParam()
		{
			return lastExtParam;
		}

		/// <summary>
		/// 検量線補正
		/// </summary>
		/// <remarks>
		/// 検量線の補正を行います。
		/// </remarks>
		/// <param name="resultDataPoint">測定結果データ</param>
		/// <param name="masterCurvePoint">マスターカーブ</param>
		/// <returns>補正後検量線ポイント</returns>
		public static ItemPoint[] GetAdjustCurve(ItemPoint[] resultDataPoint, ItemPoint[] masterCurvePoint)
		{
			return masterCurvePoint.Select((masterPoint) =>
			{
				// 測定ポイントの取得
				var retPoint = resultDataPoint.FirstOrDefault((data) => data.xPos == masterPoint.xPos);

				// 測定ポイントが無い場合、補正を行う
				if (retPoint == null)
				{
					retPoint = (ItemPoint)(masterPoint).Clone();

					// 補正ポイント前後の測定ポイント
					var beforePoint = resultDataPoint.Where((data) => data.xPos < masterPoint.xPos).OrderBy((data) => data.xPos).LastOrDefault();
					var afterPoint = resultDataPoint.Where((data) => data.xPos > masterPoint.xPos).OrderBy((data) => data.xPos).FirstOrDefault();
					
                    // 測定ポイントが2つだけでも補正ができるように変更（1ポジション目が補正ポイントでも補正可能）
					if (beforePoint == null)
					{
						beforePoint = resultDataPoint.Where((data) => data.xPos > masterPoint.xPos).OrderBy((data) => data.xPos).FirstOrDefault();
						afterPoint = resultDataPoint.Where((data) => data.xPos > beforePoint.xPos).OrderBy((data) => data.xPos).FirstOrDefault();

					}
					if (afterPoint == null)
					{
						afterPoint = resultDataPoint.Where((data) => data.xPos < masterPoint.xPos).OrderBy((data) => data.xPos).LastOrDefault();
						beforePoint = resultDataPoint.Where((data) => data.xPos < afterPoint.xPos).OrderBy((data) => data.xPos).LastOrDefault();
					}

					if (beforePoint != null && afterPoint != null)
					{
						// 補正ポイント前後の測定ポイントと同濃度値のマスターカーブのポイント
						var beforePointMaster = masterCurvePoint.FirstOrDefault((data) => data.xPos == beforePoint.xPos);
						var afterPointMaster = masterCurvePoint.FirstOrDefault((data) => data.xPos == afterPoint.xPos);

						if (beforePointMaster != null && afterPointMaster != null)
						{
							// マスターカーブの補正ポイントのカウント値 Mode one ,简化版公式
                            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CalibrationModeParameter.CalibrationMode == CalibrationModeParameter.CalibrationModeKind.ModeOne)
                            {
                                var remainder = masterPoint.yPos;
                               if (remainder != 0)
                                {                                    
                                   retPoint.yPos = (remainder) * ((afterPoint.yPos - beforePoint.yPos) / (afterPointMaster.yPos - beforePointMaster.yPos));
                                }
                                else
                                {
                                   retPoint = null;
                                }
                            }
                            else//mode two ，日方原版公式
                            {
                                var remainder = masterPoint.yPos - beforePointMaster.yPos;
                                if (remainder != 0)
                                {
                                    retPoint.yPos = beforePoint.yPos + (remainder) * ((afterPoint.yPos - beforePoint.yPos) / (afterPointMaster.yPos - beforePointMaster.yPos));                                    
                                }
                                else
                                {
                                    retPoint = null;
                                }
                            }
                           
						}
						else
						{
							retPoint = null;
						}
					}
					else
					{
						retPoint = (ItemPoint)(retPoint).Clone();
					}
				}
				else
				{
					retPoint = (ItemPoint)(retPoint).Clone();
				}

				return retPoint;
			}).ToArray();
		}

		#endregion

		#endregion

		#region [privateメソッド]

		#region _再計算_

		/// <summary>
		/// 再計算
		/// </summary>
		/// <remarks>
		/// 再計算します。
		/// 測定(多重測定1～最終)毎に並行処理で再計算を実行します。
		/// </remarks>
		/// <param name="calcDataList">計算データリスト</param>
		/// <param name="kind">検体種別</param>
		/// <param name="recalcInfo">再計算条件情報</param>
		/// <param name="reCalcCompleteData">再計算結果</param>
		/// <returns>再計算結果(true:完了)</returns>
		private static Boolean ReCalc( List<CalcData> calcDataList, SampleKind kind, IRecalcInfo recalcInfo, out List<CalcData> reCalcCompleteData )
		{
			// 再計算実施可能チェック
			if ( calcDataList == null || !CarisXCalculator.canReCalc( calcDataList.Count ) )
			{
				reCalcCompleteData = null;
				return false;
			}

			// 1検体の1分析毎(再計算の並列処理単位)にリスト化(検体識別番号、ユニーク番号、多重測定回の昇順)
			// 多重測定の1～最終を1要素としたリスト
			IEnumerable<IEnumerable<CalcData>> parallelDatas = from data in calcDataList
															   where recalcInfo.AnalyteSelect.Contains( data.ProtocolIndex )
															   group data by new
															   {
																   IndividuallyNo = data.IndividuallyNo,
																   UniqueNo = data.UniqueNo
															   } into grp
															   select from relatedData in calcDataList
																	  where relatedData.IndividuallyNo == grp.Key.IndividuallyNo && relatedData.UniqueNo == grp.Key.UniqueNo
																	  orderby relatedData.IndividuallyNo, relatedData.UniqueNo, relatedData.ReplicationNo
																	  select relatedData;

			// 検量線情報取得
			Func<Int32, Int32, String, List<CalibrationCurveData>> getCalibCurve = CarisXCalculator.getCalibCurveInfoList( recalcInfo.AnalyteSelect
                                                                                                                         , recalcInfo.ReagentLotNoSelect
                                                                                                                         , recalcInfo.CalibrationCurveApprovalDate );

			// 再計算完了データリスト
			SynchronizedCollection<CalcData> reCalcCompleteDataList = new SynchronizedCollection<CalcData>();

			// 再計算の並列処理
			ManualResetEvent endCalc = new ManualResetEvent( false );
			Int32 execCount = 0;

			// 多重測定毎にパラレルで再計算処理を実行
			Parallel.ForEach<IEnumerable<CalcData>, int>( parallelDatas, () =>
			{
				return Interlocked.Add( ref execCount, 1 );
			}, ( uniqueDatas, a, b ) =>
			{
				// 平均算出用多重測定回数すべての結果を取得
				List<CalcData> repCalcDataList = uniqueDatas.ToList();

				// 再計算使用検量線の選択
				List<CalibrationCurveData> calibCurveInfo = getCalibCurve(repCalcDataList[0].ModuleNo, repCalcDataList[0].ProtocolIndex, repCalcDataList[0].ReagentLotNo );

				// 検量線がある場合
				if ( calibCurveInfo != null )
				{
					List<CalcData> averageCalcRepCalcDataList = uniqueDatas.ToList();

                    Int32 moduleNo = calibCurveInfo.FirstOrDefault().GetModuleNo();

                    // 再計算実行(並列処理不可のリスト)
                    foreach ( var repCalcData in repCalcDataList )
					{
						// カウント値なしでも多重測定最終レプリケーションの場合のみ平均算出
						// 多重測定の最終レプリケーション以外でカウント値なし、
						// または、再計算可能リマーク以外は再計算から除外
						if ( ( repCalcData.CalcInfoReplication.CountValue == null || !repCalcData.CalcInfoReplication.Remark.CanRecalculation ) && repCalcDataList.Last() != repCalcData )
						{
							// 再計算不可
							Singleton<CarisXLogManager>.Instance.WriteCommonLog( LogKind.DebugLog, String.Format( "Unique_Replication:{0}_{1}", repCalcData.UniqueNo, repCalcData.ReplicationNo ) );
							continue;
						}

						#region _再計算後付加リマーク(コピー)_

						Remark addingRemarkReplication = 0;   // 再計算後付加リマーク
						Remark addingRemarkAverage = 0;       // 再計算後付加リマーク(平均用)

						// 多重測定レプリケーション別リマーク
						addingRemarkReplication = repCalcData.CalcInfoReplication.Remark.GetFilterRemark( Remark.RemarkCategory.DataEdited );
						// 多重測定平均リマーク
						if ( repCalcData.CalcInfoAverage != null )
						{
							addingRemarkAverage = repCalcData.CalcInfoAverage.Remark.GetFilterRemark( Remark.RemarkCategory.DataEdited );
						}

                        #endregion

                        // 再計算結果用
                        //【IssuesNo:9】 当为常规和优先样本再计算时，由于IGRA判定需要使用ReceiptNumber,因此增加
                        // 但质控品不需要ReceiptNumber,此值为默认值，不影响后续计算
						CalcData calcData = new CalcData( repCalcData.ModuleNo,
						                               	  repCalcData.ProtocolIndex,
						                               	  repCalcData.ReagentLotNo,
						                               	  repCalcData.IndividuallyNo,
						                               	  repCalcData.UniqueNo,
						                               	  repCalcData.ReplicationNo,
						                               	  repCalcData.ManualDilution,
						                               	  repCalcData.AutoDilution,
						                               	  repCalcData.MeasureDateTime,
                                                          repCalcData.ReceiptNumber,
						                               	  repCalcData.RackID,
						                               	  repCalcData.RackPosition,
						                               	  repCalcData.SampleID );

						#region _計算情報/平均計算情報生成(レプリケーション別)_

						// カウント値取得(濃度算出可能リマークのみ)
						//if ( repCalcData.CalcInfoReplication.Remark.CanCalcConcentration )
						//{
							calcData.CalcInfoReplication = new CalcInfo( repCalcData.CalcInfoReplication.CountValue );
						//}

						// カウント値(平均)を取得(最終レプリにカウント値(平均)がある場合のみ)
						if ( repCalcDataList.Count > 1 && repCalcData.ReplicationNo == repCalcDataList[repCalcDataList.Count - 1].ReplicationNo )
						{
							if ( repCalcData.CalcInfoAverage != null )  //&& repCalcData.CalcInfoAverage.Remark.CanCalcConcentration )
							{
								//if ( repCalcData.CalcInfoAverage.Remark.CanCalcCount )
								//{
									calcData.CalcInfoAverage = new CalcInfo( repCalcData.CalcInfoAverage.CountValue );
								//}
							}
							else
							{
								calcData.CalcInfoAverage = new CalcInfo( null );
							}
						}

						#endregion

						#region _濃度算出(レプリケーション別)_

						// 分析項目取得
						MeasureProtocol measureProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( repCalcData.ProtocolIndex );

						// 再計算(再計算可能リマークのみ)
						if ( repCalcData.CalcInfoReplication.Remark.CanRecalculation )
						{
                            if (CarisXCalculator.CalcConc(measureProtocol, kind, calibCurveInfo, calcData, false))
                            {
                                #region __リマーク付加__

                                // 再計算後のリマークにリマークを引き継ぎ(継承可能なリマークのみ)
                                calcData.CalcInfoReplication.Remark.AddRemark(repCalcData.CalcInfoReplication.Remark.ReCalcInheritRemark);

                                // 再計算後のリマークにリマークを引き継ぎ(再計算後付加リマークのみ)
                                calcData.CalcInfoReplication.Remark.AddRemark(addingRemarkReplication);

                                // 再計算リマーク付加
                                calcData.CalcInfoReplication.Remark.AddRemark(Remark.RemarkBit.EditOfReCalcu);

                                #endregion

                                // 平均算出用多重測定回数分へ結果を反映
                                //                        averageCalcRepCalcDataList.ToList().ForEach( ( repData ) =>
                                //{
                                //	if ( repData.ReplicationNo == calcData.ReplicationNo )
                                //	{
                                //		averageCalcRepCalcDataList[repCalcDataList.IndexOf( repData )] = calcData;
                                //	}
                                //} );
                                //【IssuesNo:8】由于目前使用.net 4.6.1，直接在foreach()修改元素会抛出异常，因此这里更换写法
                                for (int i = 0; i < averageCalcRepCalcDataList.Count(); i++)
                                {
                                    if (averageCalcRepCalcDataList[i].ReplicationNo == calcData.ReplicationNo)
                                    {
                                        averageCalcRepCalcDataList[repCalcDataList.IndexOf(averageCalcRepCalcDataList[i])] = calcData;
                                    }
                                }
							}
						}

						#endregion

						#region _検量線エラー_

						if ( calcData.CalcInfoReplication != null && calcData.CalcInfoReplication.Remark.HasRemark( Remark.RemarkBit.CalibrationCurveError ) ) //|| calcConcInfoAverage.Remark.HasRemark( Remark.RemarkBit.CalibrationCurveError ) ) // 不要(冗長･同義)
						{
							calcData.CalcInfoReplication.Concentration = null;
							if ( calcData.CalcInfoAverage != null )
							{
								calcData.CalcInfoAverage.Concentration = null;
							}
						}

						#endregion

						#region _平均算出_

						// 平均算出(最終レプリケーション)
						bool isAverage = averageCalcRepCalcDataList.Count > 1 && calcData.ReplicationNo == averageCalcRepCalcDataList.Last().ReplicationNo;
						if ( isAverage )
						{
							// コピー(編集リマークの削除版)
							var tempCalcDataList = ( from repData in averageCalcRepCalcDataList
													 select new Func<CalcData, CalcData>( ( copyData ) =>
													 {
														 copyData.CalcInfoReplication.Remark.RemoveRemark(
															 copyData.CalcInfoReplication.Remark.GetFilterRemark( Remark.RemarkCategory.DataEdited ) );

														 if ( copyData.CalcInfoAverage != null )
														 {
															 copyData.CalcInfoAverage.Remark.RemoveRemark(
																 copyData.CalcInfoAverage.Remark.GetFilterRemark( Remark.RemarkCategory.DataEdited ) );
														 }
														 return copyData;
													 } )( repData.Copy() ) ).ToList();

							// 平均カウント値算出
							List<AnalysisErrorInfo> analysisErrorInfoList = null;
							Remark remark = calcData.CalcInfoAverage.Remark;
							calcData.CalcInfoAverage.CountValue = CalcAveCount( measureProtocol, tempCalcDataList, ref analysisErrorInfoList, ref remark );

							// 平均濃度計算
							CarisXCalculator.CalcConc( measureProtocol, kind, calibCurveInfo, calcData, true );

							#region __リマーク付加__

							// 全レプリのリマークをOR
							foreach ( var repData in averageCalcRepCalcDataList )
							{
								calcData.CalcInfoAverage.Remark.AddRemark( (Remark.RemarkBit)repData.CalcInfoReplication.Remark.Value );
							}

							#endregion
						}

						#endregion

						CalcConcSuccesForSampleKind( kind, repCalcDataList.Count, calcData, measureProtocol, null, isAverage );

						// 再計算完了データリストへ追加
						reCalcCompleteDataList.Add( calcData );
					}
				}
				return 0;
			}, ( last ) =>
			{
				Interlocked.Add( ref execCount, -1 );
				if ( execCount <= 0 )
				{
					endCalc.Set();
				}
			} );

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // 【IssuesNo:9】增加重新计算时对IGRA的判定
            if(kind == SampleKind.Sample || kind == SampleKind.Priority)
            {
                foreach(var repIGRACalcData in reCalcCompleteDataList)
                {
                    //获取分析项目
                    MeasureProtocol measureProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(repIGRACalcData.ProtocolIndex);
                    if(measureProtocol.IsIGRA)
                    {
                        var repCalcDataList = from relatedData in reCalcCompleteDataList
                                              where relatedData.ProtocolIndex == repIGRACalcData.ProtocolIndex && relatedData.ReceiptNumber == repIGRACalcData.ReceiptNumber
                                              && relatedData.MeasureDateTime.Date == repIGRACalcData.MeasureDateTime.Date
                                              orderby relatedData.IndividuallyNo, relatedData.UniqueNo, relatedData.ReplicationNo
                                              select relatedData;
                        if(repIGRACalcData.UniqueNo == repCalcDataList.Last().UniqueNo)
                        {
                            IGRAMethod igraMethod = null;
                            if(measureProtocol.RepNoForSample == 1 && repCalcDataList.ToList().Count == 3)
                            {
                                int nIndex = 2;//有可能最后一个结果由于故障结果先出
                                for(int i = 0;i < repCalcDataList.ToList().Count; i++)
                                {
                                    if(repIGRACalcData.UniqueNo == repCalcDataList.ToList()[i].UniqueNo)
                                    {
                                        nIndex = i;
                                    }
                                }
                                if(nIndex == 0)
                                {
                                    igraMethod = new IGRAMethod(repIGRACalcData.CalcInfoReplication.Concentration, repCalcDataList.ToList()[1].CalcInfoReplication.Concentration, repCalcDataList.ToList()[2].CalcInfoReplication.Concentration);
                                }
                                else if(nIndex == 1)
                                {
                                    igraMethod = new IGRAMethod(repCalcDataList.ToList()[0].CalcInfoReplication.Concentration, repIGRACalcData.CalcInfoReplication.Concentration, repCalcDataList.ToList()[2].CalcInfoReplication.Concentration);
                                }
                                else if(nIndex == 2)
                                {
                                    igraMethod = new IGRAMethod(repCalcDataList.ToList()[0].CalcInfoReplication.Concentration, repCalcDataList.ToList()[1].CalcInfoReplication.Concentration, repIGRACalcData.CalcInfoReplication.Concentration);
                                }
                            }
                            else
                            {
                                if(repCalcDataList.ToList().Count == measureProtocol.RepNoForSample * 3)
                                {
                                    igraMethod = new IGRAMethod(repCalcDataList.ToList()[0].CalcInfoAverage.Concentration, repCalcDataList.ToList()[1].CalcInfoAverage.Concentration, repCalcDataList.ToList()[2].CalcInfoAverage.Concentration);
                                }
                            }
                            if(igraMethod != null)
                            {
                                JudgementType judgeType = igraMethod.CaculateJudge();
                                repIGRACalcData.Judgement = judgeType.ToTypeString();
                            }
                            else
                            {
                                repIGRACalcData.Judgement = null;
                            }
                        }
                        else
                        {
                            repIGRACalcData.Judgement = null;
                        }
                    }
                }
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // 再計算対象となるすべての測定(多重測定毎)の完了を待機
            endCalc.WaitOne();

			// 再計算完了リストを引数に設定(返却)
			reCalcCompleteData = reCalcCompleteDataList.ToList();
			return true;
		}

		/// <summary>
		/// 分析エラーの追加
		/// </summary>
		/// <remarks>
		/// 分析エラーの追加します
		/// </remarks>
		/// <param name="error">分析エラー</param>
		/// <param name="errorInfoList">エラー発生に伴う</param>
		/// <param name="remark">エラー発生に伴うリマーク追加対象</param>
		/// <param name="remarkBit">追加指定リマークビット</param>
		/// <returns></returns>
		private static AnalysisErrorInfo AddErrorInfo( AnalysisError error, List<AnalysisErrorInfo> errorInfoList = null, Remark remark = null, Remark.RemarkBit? remarkBit = null )
		{
			AnalysisErrorInfo errorInfo = new AnalysisErrorInfo( error );

			// エラー情報を追加
			if ( errorInfoList != null )
			{
				errorInfoList.Add( errorInfo );
			}

			// 分析エラーのリマークを編集
			if ( remarkBit.HasValue )
			{
				errorInfo.EditInfo( remark: remarkBit.Value );
			}
			
			// 分析エラーのリマークを取得
			if ( remark != null )
			{
				remark.AddRemark( errorInfo.Remark );
			}

			return errorInfo;
		}

		/// <summary>
		/// 再計算実施可能チェック
		/// </summary>
		/// <remarks>
		/// 再計算実施可能チェックし、不可の場合メッセージ表示します
		/// </remarks>
		/// <param name="count">再計算対象</param>
		/// <returns>再計算の実施可否</returns>
		private static Boolean canReCalc( Int32 count )
		{
			// 再計算対象チェック
			if ( count == 0 )
			{
				// 再計算対象無しのため再計算不可
				DlgMessage.Show( CarisX.Properties.Resources.STRING_DLG_MSG_065, "", CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OK );
				return false;
			}
			else if ( Singleton<SystemStatus>.Instance.Status == SystemStatusKind.Assay && count > CarisXConst.INASSAY_CALCDATA_LIMIT_MAX )
			{
				// 分析中再計算上限数超過時の再計算禁止
				DlgMessage.Show( String.Format( CarisX.Properties.Resources.STRING_DLG_MSG_141, CarisXConst.INASSAY_CALCDATA_LIMIT_MAX.ToString() ), "", CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OK );
				return false;
			}

			return true;
		}


		/// <summary>
		/// 検量線情報の取得
		/// </summary>
		/// <remarks>
		/// 検量線情報を取得します。
		/// </remarks>
		/// <param name="protocolIndexList">分析項目インデックスリスト</param>
		/// <param name="reagentLotNo">試薬ロット番号</param>
		/// <param name="calibrationCurveApprovalDate">検量線作成日</param>
		/// <returns>検量線取得メソッド</returns>
		private static Func<Int32, Int32, String, List<CalibrationCurveData>> getCalibCurveInfoList(List<Int32> protocolIndexList, String reagentLotNo, DateTime calibrationCurveApprovalDate )
		{
			if ( Singleton<MeasureProtocolManager>.Instance.UseMeasureProtocolList.All( ( protocol ) => protocolIndexList.Exists( ( protocolIndex ) => protocolIndex == protocol.ProtocolIndex ) ) )
			{
                // 分析項目の選択がすべての場合、
                // 各種分析項目、試薬ロット別の最新検量線情報を取得
                Dictionary<Int32, Dictionary<Int32, Dictionary<String, List<CalibrationCurveData>>>> lastCalibCurveInfos = HybridDataMediator.SearchLastCalibCurveFromCalibCurveDB();
				return ( moduleNo, protocolIndex, reagentLot ) =>
				{
					if ( lastCalibCurveInfos.ContainsKey( moduleNo )
                      && lastCalibCurveInfos[moduleNo].ContainsKey( protocolIndex )
                      && lastCalibCurveInfos[moduleNo][protocolIndex].ContainsKey( reagentLot ) )
					{
						return lastCalibCurveInfos[moduleNo][protocolIndex][reagentLot];
					}
					return null;
				};
			}
			else
			{
				// 分析項目の選択が指定の検量線の場合、
				// 単一の検量線情報を取得
				List<CalibrationCurveData> specifyCalibCurveInfo = HybridDataMediator.SearchCalibCurveFromCalibCurveDB( protocolIndexList[0], reagentLotNo, calibrationCurveApprovalDate );
				return ( moduleNo, protocolIndex, reagentLot ) =>
				{
					return specifyCalibCurveInfo;
				};
			}
		}

		#endregion

		#region _計算_

		/// <summary>
		/// 濃度算出成功時の検体種別毎の処理
		/// </summary>
		/// <remarks>
		/// 濃度算出成功時の検体種別毎の処理を実行します。
		/// </remarks>
		/// <param name="kind">検体種別</param>
		/// <param name="calcData">計算データ</param>
		/// <param name="measureProtocol">分析項目</param>
		/// <param name="isAverage">対象データが平均かどうか</param>
		private static void CalcConcSuccesForSampleKind( SampleKind kind, Int32 repCount, CalcData calcData, MeasureProtocol measureProtocol, List<AnalysisErrorInfo> analysisErrorInfoList, Boolean isAverage = false )
		{
			CalcInfo calcInfo = ( isAverage ) ? calcData.CalcInfoAverage : calcData.CalcInfoReplication;

			switch ( kind )
			{
			case SampleKind.Sample:
			case SampleKind.Priority:
				// 判定
				if (repCount == 1 || isAverage )
				{
					calcData.Judgement = GetJudgementString( measureProtocol, calcInfo.Concentration, kind );
				}
				break;
			case SampleKind.Control:
				// 管理値チェックとリマーク付加
				if ( !isAverage )
				{
					setControlConcentrationRemark( measureProtocol, calcInfo, calcData.SampleID, calcData.RackID, calcData.RackPosition.Value, false, analysisErrorInfoList );
				}
				break;
			default:
				break;
			}
		}

		/// <summary>
		/// 計算データの作成
		/// (平均は作成しない)
		/// </summary>
		/// <remarks>
		/// 計算データを作成します。
		/// </remarks>
		/// <param name="measureProtocolIndex">分析項目インデックス</param>
		/// <param name="reagentLotNo">試薬ロット番号</param>
		/// <param name="individuallyNo">検体識別番号</param>
		/// <param name="uniqueNo">ユニーク番号</param>
		/// <param name="replicationNo">レプリケーション番号</param>
		/// <param name="manualDilution">手希釈倍率</param>
		/// <param name="autoDilution">自動希釈倍率</param>
		/// <param name="measureDateTime">測定日時</param>
		/// <param name="rackId">ラックID</param>
		/// <param name="rackPosition">ラックポジション</param>
		/// <param name="count">カウント値</param>
		/// <param name="remark">リマーク</param>
		/// <returns>測定結果データ</returns>
		private static CalcData createCalcData( Int32 moduleNo, Int32 measureProtocolIndex, String reagentLotNo, Int32 individuallyNo, Int32 uniqueNo, Int32 replicationNo, Int32 manualDilution, Int32 autoDilution, DateTime measureDateTime, CarisXIDString rackId, Int32? rackPosition, Int32? count, Remark remark )
		{
			var calc = new CalcData( moduleNo, measureProtocolIndex, reagentLotNo, individuallyNo, uniqueNo, replicationNo, manualDilution, autoDilution, measureDateTime, rackId, rackPosition )
			{
				CalcInfoReplication = new CalcInfo( count )
				{
					Remark = remark
				}
			};
			return calc;
		}

		/// <summary>
		/// 精度管理検体の濃度値によるリマーク設定
		/// </summary>
		/// <remarks>
		/// 精度管理検体の濃度値によるリマーク設定します
		/// </remarks>
		/// <param name="measureProtocol">分析項目</param>
		/// <param name="calcInfo">計算情報</param>
		/// <param name="controlLot">精度管理検体ロット番号</param>
		/// <param name="analysisErrorInfoList">エラーリスト</param>
		/// <param name="isRecalc">再計算フラグ(true:再計算/false:非再計算)</param>
		private static void setControlConcentrationRemark( MeasureProtocol measureProtocol, CalcInfo calcInfo, String controlLot,CarisXIDString rackId, Int32 rackPos, Boolean isRecalc, List<AnalysisErrorInfo> analysisErrorInfoList = null )
		{
			// 管理値(判定不能/管理値範囲外エラー)
			if ( calcInfo != null )
			{
				// 管理値データの読み込み
				Singleton<ParameterFilePreserve<ControlQC>>.Instance.Load();

				// 精度管理検体ロット番号より、精度管理検体名の取得
				String controlName = HybridDataMediator.SearchControlNameFromControlDB( controlLot, rackId, rackPos );

				// 管理値の取得
				var controlQCData = ( from v in Singleton<ParameterFilePreserve<ControlQC>>.Instance.Param.ControlQCList
									  where v.MeasureProtocolIndex == measureProtocol.ProtocolIndex && v.ControlLotNo == controlLot && v.ControlName == controlName
									  select v ).FirstOrDefault();

				// 精度管理検体に対する管理値が存在する場合
				if ( controlQCData != null && controlQCData.Mean.HasValue && controlQCData.ConcentrationWidth.HasValue && controlQCData.ControlR.HasValue )
				{
					// 計算情報が濃度値を保持する場合
					if ( calcInfo.Concentration.HasValue )
					{
						// 精度管理検体の濃度値母集団平均値と濃度値の差を取得
						Double? concentration = ( isRecalc ) ? calcInfo.Concentration : null;   // 再計算時に母集団より除く、自身の濃度値
						Double diffConcPopulationAve = Math.Abs( HybridDataMediator.SearchTodayAverageFromControlDB( measureProtocol.ProtocolIndex, controlLot, controlName, concentration ) );

						// 平均、濃度幅、R管理値によるチェック
						if ( ( Math.Abs( calcInfo.Concentration.Value - controlQCData.Mean.Value ) > controlQCData.ConcentrationWidth.Value )
							|| ( Math.Abs( calcInfo.Concentration.Value - diffConcPopulationAve ) > controlQCData.ControlR ) )
						{
							// 管理値範囲外エラー(Xバー管理図)
							AddErrorInfo( AnalysisError.OutOfRangeOfControlErr, analysisErrorInfoList, calcInfo.Remark );
						}
					}
				}
				else
				{
					// 精度管理判定不能
					AddErrorInfo( AnalysisError.JudgeControlErr, analysisErrorInfoList, calcInfo.Remark );
				}
			}
		}

		#endregion
		
		#region _判定_

		/// <summary>
		/// 判定(定性項目)
		/// </summary>
		/// <remarks>
		/// 定性項目の判定をします
		/// </remarks>
		/// <param name="measureProtocol">分析項目</param>
		/// <param name="concentration">判定対象濃度値</param>
		/// <returns>判定結果</returns>
		private static JudgementType Judgement( MeasureProtocol measureProtocol, Double concentration )
		{
            //定义一个点为灰区的情况
            if (measureProtocol.PosiLine == measureProtocol.NegaLine
                && measureProtocol.PosiLine == concentration
                && measureProtocol.NegaLine == concentration)
            {
                return JudgementType.Half;
            }

            //凯瑞新需求，增加 一种ＩＮＨ的判定标准，即INH 参数a=0的情况下，浓度大于Positive,判定为阴性，反之亦然。
            if (measureProtocol.CalibType == MeasureProtocol.CalibrationType.INH
                && measureProtocol.Coef_A == 0)
            {
                // 定性項目の判定
                if (measureProtocol.PosiLine <= concentration)
                {
                    return JudgementType.Negative;
                }
                else if (measureProtocol.NegaLine > concentration)
                {
                    return JudgementType.Positive;
                }

            }
            else
            {
                // 定性項目の判定
                if (measureProtocol.PosiLine <= concentration)
                {
                    return JudgementType.Positive;
                }
                else if (measureProtocol.NegaLine > concentration)
                {
                    return JudgementType.Negative;
                }
            }
			
            //else
            //{
				return JudgementType.Half;
			//}
		}

		#endregion

		#region _カウント値取得_

		/// <summary>
		/// カウント平均値算出
		/// </summary>
		/// <remarks>
		/// カウント平均値を算出します。
		/// </remarks>
		/// <param name="measureProtocol">分析項目プロトコル</param>
		/// <param name="calcDataList">平均算出データリスト</param>
		/// <param name="analysisErrorInfoList">分析エラー情報リスト</param>
		/// <param name="remark">エラー発生に伴うリマーク追加対象</param>
		/// <returns>カウント平均</returns>
		private static Int32? CalcAveCount( MeasureProtocol measureProtocol, List<CalcData> calcDataList, ref List<AnalysisErrorInfo> analysisErrorInfoList, ref Remark remark ,bool bCalibator = false)
		{
			Int32? averageCount = null;
			AnalysisError error = AnalysisError.NoError;

            
			// カウント平均算出ポイントを抽出(リマーク無し＝正常ポイント)
            IEnumerable<CalcData> calcDatas = calcDataList.Where((data) => data.CalcInfoReplication.Remark == 0 && data.CalcInfoReplication.CountValue.HasValue);

            
			// 正常ポイント数が複数ポイントない場合、
			// 非正常のカウント平均算出可能ポイントを抽出
			if ( calcDatas.Count() < 2 )
			{
				// 注意ポイントを抽出
				calcDatas = calcDataList.Where( ( data ) =>
					data.CalcInfoReplication.Remark.CanIncludingCautionPoint &&
					data.CalcInfoReplication.CountValue.HasValue );
			}

			List<CalcData> useDatas = null;
			if ( calcDatas.Count() >= 2 )       // カウント平均の算出
			{
				useDatas = calcDatas.ToList();

				// 多重平均値算出(0:多重乖離エラー)
				bool retry = false;
				Func<List<CalcData>, Int32?> act = null;
				act = new Func<List<CalcData>, Int32?>( ( datas ) =>
				{
					// 有効データ数取得
					Int32 numOfUseData = datas.Count();

					// 有効データ数が複数の場合

					Int32? average;
					Double cv, sd, averageBuff;
					cv = datas.Where( ( data ) => 
										data.CalcInfoReplication.CountValue.HasValue )
										.Select( ( data ) => 
													(Double)data.CalcInfoReplication.CountValue.Value ).GetCV( out sd, out averageBuff, true, true );
					average = (Int32)averageBuff;
                    Double CVofProtocol = -1;
                    if (bCalibator)
                    {
                        string strLotNo = calcDataList[0].ReagentLotNo;
                        List<CalibrationCurveData> list = Singleton<CalibrationCurveDB>.Instance.GetMasterCurveData(measureProtocol.ProtocolIndex, strLotNo);
                        double[] concs = new double[list.Count];
                        if (list.Count != 0)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                concs[i] = double.Parse(list[i].Concentration);
                            }
                        }
                        else
                        {
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "<$*$ CalcAveCount( $*$> Caribrator Curfe is empty!");
                           // return false;
                        }
                        if (measureProtocol.CalibType.IsQualitative())//定性
                        {
                            //第一个校准点
                            
                            if (HybridDataMediator.searchCheckFirstRackAndPosition(useDatas[0].ProtocolIndex, useDatas[0].RackID, useDatas[0].RackPosition.Value,useDatas[0].UniqueNo))
                            {
                                CVofProtocol = measureProtocol.CVofEachPoint[0];
                            }
                            else//第二个校准点
                            {
                                CVofProtocol = measureProtocol.CVofEachPoint[1];
                            }
                              
                        }
                        else//定量的测试
                        {
                            Double con  = -1;
                            var VarCon = datas.First((data) => data.CalcInfoReplication.Concentration.HasValue) ;
                            if (VarCon == null)
                            {
                                CVofProtocol = measureProtocol.MulMeasDevLimitCV;
                            }
                            else
                            {
                                //CalcData data = VarCon as CalcData;
                               // con = data.CalcInfoReplication.Concentration.Value;
                                con = VarCon.CalcInfoReplication.Concentration.Value;

                                //for (int i = 0;i< measureProtocol.ConcsOfEach.Count(); i++)
                                for (int i = 0; i < concs.Length; i++)
                                {
                                    //if (con == measureProtocol.ConcsOfEach[i])
                                    if (con == concs[i])
                                    {
                                        CVofProtocol = measureProtocol.CVofEachPoint[i];
                                        break;
                                    }                                
                                }

                                if (CVofProtocol <0)//表明CV不在基础曲线上，找点最接近的ＣＶ的点
                                {
                                    int nIndexCheckRange = 0;
                                    bool bCheck = false;
                                    //for (int i = 0; i < measureProtocol.ConcsOfEach.Count(); i++)
                                    for (int i = 0; i < concs.Length; i++)
                                    {
                                        //if (con <= measureProtocol.ConcsOfEach[i])
                                        if (con <= concs[i])
                                        {
                                            nIndexCheckRange = i;
                                            bCheck = true;
                                            break;
                                        }
                                    }
                                    //如果比最大的值大，用最大值的Index
                                    if (bCheck == false)
                                    {
                                       // nIndexCheckRange = measureProtocol.ConcsOfEach.Count() - 1;
                                        nIndexCheckRange = concs.Length - 1;
                                    }

                                    if (nIndexCheckRange == 0)
                                    {
                                        CVofProtocol = measureProtocol.CVofEachPoint[0];
                                    }
                                    else
                                    {                              
                                        //Double absLeft= Math.Abs(measureProtocol.ConcsOfEach[nIndexCheckRange - 1] - con);
                                        //Double absRight = Math.Abs(measureProtocol.ConcsOfEach[nIndexCheckRange] - con);
                                        Double absLeft = Math.Abs(concs[nIndexCheckRange - 1] - con);
                                        Double absRight = Math.Abs(concs[nIndexCheckRange] - con);
                                        if (absLeft>= absRight)
                                        {
                                            CVofProtocol = measureProtocol.CVofEachPoint[nIndexCheckRange];//和右边的点接近
                                        }
                                        else
                                        {
                                            CVofProtocol = measureProtocol.CVofEachPoint[nIndexCheckRange - 1];//和左边的点接近
                                        }
                                    }                               
                                }
                            }

                        }
                        
                    }
                    else
                    {
                        CVofProtocol = measureProtocol.MulMeasDevLimitCV;
                    }

					// 多重乖離限界CV(%)チェックエラーの場合
					//if ( cv > measureProtocol.MulMeasDevLimitCV )
                    if (cv > CVofProtocol)
					{
						//if ( !retry)
                        if (!retry && datas.Count > 2)//两个值如果在去除一个，算CV及SD是有问题的，average为0
               			{// 初回時、リトライ
							CalcData removeTarget = datas.FirstOrDefault( data => !data.CalcInfoReplication.CountValue.HasValue );
							if ( removeTarget == null )
							{
								// 偏差の最大を削除対象に設定
								removeTarget = datas.OrderByDescending( data => Math.Sqrt( ( data.CalcInfoReplication.CountValue ?? 0 ) - average.Value ) ).First();
							}

							// エラーデータを除外
							datas.Remove( removeTarget );

							// リトライ(上限１回)
							retry = true;
							average = act( datas );
							if ( error == AnalysisError.DiffError )
							{
								average = (Int32)averageBuff;
							}
						}
						// 多重測定内乖離限界エラー
						error = AnalysisError.DiffError;
					}

					return average;
				} );

				averageCount = act( useDatas );
			}
			else if ( calcDatas.Count() == 1 )
			{
				averageCount = calcDatas.First().CalcInfoReplication.CountValue;
			}
			else //if ( calcDatas.Count() == 0 )    // 有効データなし
			{
				error = AnalysisError.CalcConcError;    // 濃度計算不能エラー
			}
           
			// 平均カウント算出エラーの場合
			if ( !calcAverageCountPermissibleError.Contains( error ) )
			{
				var analysisErrorInfo = AddErrorInfo( error, errorInfoList: analysisErrorInfoList, remark: remark );

				// 濃度算出エラーの場合
				if ( error == AnalysisError.CalcConcError )
				{
					analysisErrorInfo.EditInfo( errorComment: String.Empty ); // TODO:有効データなし
				}
			}
      
			return averageCount;
		}
		
		#endregion

		#region _濃度計算_

		/// <summary>
		/// 濃度計算の実行
		/// </summary>
		/// <remarks>
		/// 濃度値の計算を行います。
		/// </remarks>
		/// <param name="measureProtocol">分析項目情報</param>
		/// <param name="sampleKind">検体種別</param>
		/// <param name="calibCurveInfo">検量線情報(NULLの場合:最新検量線使用)</param>
		/// <param name="calcConcInfo">濃度計算用データ</param>
		/// <param name="calcConcInfoAve">平均濃度計算用データ(平均計算不要：デフォルト値)</param>
		/// <param name="analysisErrorInfoList">エラーリスト(再計算時：デフォルト値)</param>
		/// <returns>True:実行完了/False:実行中断</returns>
		private static Boolean CalcConc( MeasureProtocol measureProtocol, SampleKind sampleKind, List<CalibrationCurveData> calibCurveInfo, CalcData calcData, Boolean calcAverage, List<AnalysisErrorInfo> analysisErrorInfoList = null )
		{
			// 濃度算出の開始
			Singleton<CarisXLogManager>.Instance.WriteCommonLog( LogKind.DebugLog, "<$*$ Calculator $*$> start CalcConc " );

			DateTime MeasDate = calcData.MeasureDateTime;
			Int32 manualDil = calcData.ManualDilution;
			Int32 dilution = calcData.AutoDilution;
			CalcInfo calcConcInfo = calcAverage ? calcData.CalcInfoAverage : calcData.CalcInfoReplication;
            
			// 分析項目がある場合
			if ( measureProtocol != null )
			{
				// キャリブレータの場合、登録時の濃度を取得
				if ( sampleKind == SampleKind.Calibrator )
				{
					// 濃度取得(キャリブレータ登録情報より)
					//var conc = HybridDataMediator.SearchConcentrationFromCalibratorRegistDB( calcData.RackID, calcData.RackPosition.Value );
                                  
                    var conc = Singleton<CalibratorAssayDB>.Instance.GetAssayConcentration(calcData.UniqueNo);                   
					// 濃度を設定
					calcConcInfo.Concentration = Double.Parse( conc );   
                    return true;
				}
				else if ( calibCurveInfo == null )//if ( !calcAverage || calibCureveInfo != null )
				{
					#region __検量線取得__

					// 検量線の取得
					var lastCalibCurves = HybridDataMediator.SearchLastCalibCurveFromCalibCurveDB(calcData.ModuleNo);
                    if (lastCalibCurves.ContainsKey(calcData.ModuleNo))
                    {
                        if (lastCalibCurves[calcData.ModuleNo].ContainsKey(calcData.ProtocolIndex))
                        {
                            if (lastCalibCurves[calcData.ModuleNo][calcData.ProtocolIndex].ContainsKey(calcData.ReagentLotNo))
                            {
                                calibCurveInfo = lastCalibCurves[calcData.ModuleNo][calcData.ProtocolIndex][calcData.ReagentLotNo];
                            }
                        }
                    }

                    #endregion
                }
			}
			else
			{
				// 平均算出時はエラー(及びリマーク)追加しない
				if ( !calcAverage )
				{
					AddErrorInfo( AnalysisError.NotExistMeasProto, analysisErrorInfoList, calcConcInfo.Remark );
				}
				return ( sampleKind == SampleKind.Calibrator );
			}

			// 試薬ロット番号による検量線測定ポイントの取得
			List<ItemPoint> calibPoints = null;
			if ( calibCurveInfo != null && calibCurveInfo.Count > 0 )
			{
				var curveInfoGroup = calibCurveInfo.GroupBy( ( data ) => data.Concentration ).Where( ( data ) => data != null );
				if ( curveInfoGroup.Count() > 0 )
				{
					calibPoints = curveInfoGroup.Select( ( data ) => 
						( data.Count() > 1 )
						? new ItemPoint( ( data.Last().CountAverage ?? data.Last().Count ).Value, Double.Parse( data.Last().Concentration ) )
						: new ItemPoint( data.Last().Count.Value, Double.Parse( data.Last().Concentration ) ) ).ToList();

					// 定性項目(PC/NC同一値設定の検量線の場合)
					if ( measureProtocol.CalibType.IsQualitative() && calibPoints.Count() < CarisXConst.QUALITATIVE_POINT_COUNT )
					{
					   //calibPoints.Add( (ItemPoint)calibPoints[0].Clone() ); 

                        //group with uniqueNo ,Divide into the PC and NC  group.
                       var curveInfoGroup1 = calibCurveInfo.GroupBy((data) => data.GetUniqueNo()).Where((data) => data != null);
                       int nCount = curveInfoGroup1.Count();
                       calibPoints.Clear();
                       calibPoints = curveInfoGroup1.Select((data) =>
                       (data.Count() >= 1)
                       ? new ItemPoint((data.Last().CountAverage ?? data.Last().Count).Value, Double.Parse(data.Last().Concentration))
                       : new ItemPoint(data.Last().Count.Value, Double.Parse(data.Last().Concentration))).ToList();             
					}
				}
			}

			Remark remark = new Remark();

			// 検量線エラー(検量線が存在しない場合。または検量線測定ポイント数が分析項目情報の必要数を満たさない場合)
			Int32 calibPointCount = 0;    // キャリブレーションポイント数(補正ポイント含む)
			if ( measureProtocol.CalibMethod == MeasureProtocol.CalibrationMethod.FullCalibration )
			{
				// フルキャリブの場合、設定された測定ポイント数を検量線のポイント数と比較する。
				calibPointCount = measureProtocol.NumOfMeasPointInCalib;
			}
			else 
			{
				// マスターキャリブの場合、設定された（GUI非表示項目）測定ポイント情報の、有効個数を元に
				// 検量線の必要ポイント数を数える
				Int32 measPointCount = 0; // 測定ポイント数
				foreach ( var calibPoint in measureProtocol.CalibMeasPointOfEach )
				{
				
					calibPointCount++;
					if ( calibPoint )
					{
						measPointCount++;
						if ( measPointCount == measureProtocol.NumOfMeasPointInCalib )
						{
							// マスターキャリブ測定2ポイント可能改良により
							// 必ずしも最終ポイントに測定ポイントが来なくなった
                            if (calibPoints != null)
                            {
                                calibPointCount = calibPoints.Count();
                            }
                            break;
						}
					}
		
				}
			}

			// 以下のケースで検量線エラーを発生させる
			// ・検量線測定ポイントがnullの場合
			// ・定量項目かつ測定ポイント数が不一致（マスターキャリブの場合は設定の測定ポイントPosの数とも一致することを見る）
			// ・定性項目かつ測定ポイント数が不一致
			Boolean isCalibCurveError = ( calibPoints == null 
				|| ( measureProtocol.CalibType.IsQuantitative() && calibPointCount != calibPoints.Count() )
				|| ( measureProtocol.CalibType.IsQualitative() && CarisXConst.QUALITATIVE_POINT_COUNT != calibPoints.Count() ) );

		
			if ( isCalibCurveError )
			{
				// リマーク追加[検量線エラー]
				AddErrorInfo( AnalysisError.CalibCurveErr, analysisErrorInfoList, remark );
				Singleton<CarisXLogManager>.Instance.WriteCommonLog( LogKind.DebugLog,
                    String.Format("<$*$ Calculator $*$> CalcConc CalibCurveError CalibPointCount = {0} 定量 = {1} 定性 = {2} pointCount = {3} calibPoints = {4}",
					calibPoints == null ? "null" : calibPointCount.ToString(),
					measureProtocol.CalibType.IsQuantitative(),
					measureProtocol.CalibType.IsQualitative(),
					calibPointCount,
					calibPoints == null ? "null" : calibPoints.Count.ToString()
					) );
			}
			else
			{
				// 検量線有効期限エラー
				if ( MeasDate > calibCurveInfo[0].GetApprovalDateTime().AddDays( measureProtocol.ValidityOfCurve ) )
				{
					// リマーク追加[検量線有効期限エラー]
					AddErrorInfo( AnalysisError.CalibCurveApprovalErr, analysisErrorInfoList, remark );
					Singleton<CarisXLogManager>.Instance.WriteCommonLog( LogKind.DebugLog, "<$*$ Calculator $*$> CalcConc 検量線有効期限error" );
				}

				// データ編集(修正された検量線で再計算)
				if ( calibCurveInfo.Exists( ( data ) => data.IsUserEdited() ) && analysisErrorInfoList == null )    // ※再計算時のみ
				{
					// リマーク追加[データ編集(修正された検量線で再計算)]
					remark.AddRemark( Remark.RemarkBit.EditOfReCalcuByEditCurve );
                    Singleton<CarisXLogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "<$*$ Calculator $*$> CalcConc Data Edit(the calibration curve that has been modified Recalculation)");
				}
			}

			// レプリケーション濃度値、平均濃度値の計算
			if ( calcConcInfo != null )
			{
				calcConcInfo.Concentration = null;
				calcConcInfo.Remark.AddRemark( remark );
				if ( !isCalibCurveError )
				{
					// 拡張データの取得(DBデータより)
					List<Double> extendValues = null;
					try
					{
						calibCurveInfo.FirstOrDefault( ( data ) =>
						{
							var value = 0.0;
							if ( Double.TryParse( data.GetExtendValue1(), out value ) )
							{
								extendValues = new List<Double>();
								extendValues.Add( value );
								if ( Double.TryParse( data.GetExtendValue2(), out value ) )
								{
									extendValues.Add( value );
									if ( Double.TryParse( data.GetExtendValue3(), out value ) )
									{
										extendValues.Add( value );
										if ( Double.TryParse( data.GetExtendValue4(), out value ) )
										{
											extendValues.Add( value );
											return true;
										}
									}
								}
							}
							extendValues = null;
							return false;
						} );
					}
					catch ( Exception )
					{
						extendValues = null;
						Singleton<CarisXLogManager>.Instance.WriteCommonLog( LogKind.DebugLog, "<$*$ Calculator $*$> 4Parameter_ExtendValue:Get XXX Faild XXX" );
					}

					CalcConcSub( measureProtocol, sampleKind, calibPoints, manualDil, dilution, calcConcInfo, analysisErrorInfoList: analysisErrorInfoList, extendValues: extendValues );
					calcData.UseCalcCalibCurveApprovalDate = calibCurveInfo.First().GetApprovalDateTime();
				}
			}

			// 検量線エラーの場合
			if ( isCalibCurveError && calcAverage )
			{
				if ( calcConcInfo != null )
				{
					// 濃度算出不能時はNULL
					calcConcInfo.Concentration = null;
				}

                Singleton<CarisXLogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "<$*$ Calculator $*$> end CalcConc CalibCurveError");
				return false;
			}

			Singleton<CarisXLogManager>.Instance.WriteCommonLog( LogKind.DebugLog, "<$*$ Calculator $*$> end CalcConc 正常" );
			return true;
		}

		/// <summary>
		/// 濃度計算
		/// </summary>
		/// <remarks>
		/// 濃度値の計算を行います。
		/// </remarks>
		/// <param name="measureProtocol">測定プロトコル</param>
		/// <param name="sampKind">検体種別</param>
		/// <param name="calibPoints">キャリブ</param>
		/// <param name="manualDil">手希釈倍率</param>
		/// <param name="autoDil">自動希釈倍率</param>
		/// <param name="calcConcData">計算情報</param>
		/// <param name="analysisErrorInfoList">エラー(初回計算時のみ)</param>
		/// <param name="extendValues">拡張パラメータ値リスト</param>
		/// <returns>true:計算完了 false:計算未完了</returns>
		private static Boolean CalcConcSub( MeasureProtocol measureProtocol, SampleKind sampKind, List<ItemPoint> calibPoints, Int32 manualDil, Int32 autoDil, CalcInfo calcConcData, List<AnalysisErrorInfo> analysisErrorInfoList = null, List<Double> extendValues = null )
		{
			AnalysisError analysisError = AnalysisError.NoError;

			// カウント値がNULLの場合、計算未実行
			calcConcData.Concentration = null;
			if ( calcConcData.CountValue == null )
			{
				return false;
			}

			// 濃度計算メソッド取得
            ICalcMethod method = GetCalcMethod( measureProtocol, calibPoints,false, extendValues );

            // 濃度計算
            if (measureProtocol.CalibType == MeasureProtocol.CalibrationType.LogitLog)
            {
                double beforeCount = method.GetY(0.0);
                if (Double.IsNaN(beforeCount) || Double.IsInfinity(beforeCount))//最適化オプションにより Nan->Infinity
                {
                    method = GetCalcMethod(measureProtocol, calibPoints, true);
                    beforeCount = method.GetY(0.0);
                    if (!calcCountPermissibleError.Contains(method.GetError().ToAnalysisError()))
                    {
                        return false;
                    }
                    if (Double.IsNaN(beforeCount) || Double.IsInfinity(beforeCount))//最適化オプションにより Nan->Infinity
                    {
                        return false;
                    }
                }

            }

			Double concentration = 0.0;
			if ( method != null )
			{
				//濃度計算
                concentration = method.GetX( calcConcData.CountValue.Value );
                analysisError = method.GetError().ToAnalysisError();

                if ( analysisError != AnalysisError.NoError  || concentration < 0 )
				{
                    if (analysisError == AnalysisError.LowError)
                    {
                        AddErrorInfo(AnalysisError.DynamicLowErr,analysisErrorInfoList, remark: calcConcData.Remark);
                    }
                    else if (analysisError == AnalysisError.HighError)
                    {
                        AddErrorInfo(AnalysisError.DynamicHighErr, analysisErrorInfoList,remark: calcConcData.Remark);
                    }
                    else
                    {
                        // if Caculated the concentration is nagetive value, let it to be 0;
                        if((measureProtocol.CalibType == MeasureProtocol.CalibrationType.CutOff || 
                            measureProtocol.CalibType == MeasureProtocol.CalibrationType.INH) 
                            && concentration < 0)
                        {
                            concentration = 0;
                        }
                        else
                        {
                            //Calc Conc Error , no need to be continuted!
                            AddErrorInfo(AnalysisError.CalcConcError, analysisErrorInfoList,remark: calcConcData.Remark);
                            return false;
                        }                       
                    }
				}
			}
			else
			{
				analysisError = AnalysisError.NoCurveType;
			}

			// 濃度計算不可となるエラーが発生した場合
			if ( !CarisXCalculator.calcConcPermissibleError.Contains( analysisError ) )
			{
				AddErrorInfo( analysisError, analysisErrorInfoList, calcConcData.Remark );

				// 再計算時のみカウント値クリア
				if ( analysisErrorInfoList == null )
				{
					calcConcData.CountValue = null;
				}
				return false;
			}
            else if ((analysisError == AnalysisError.HighError || analysisError == AnalysisError.LowError))
			{
                // 最大値は、測定された動的濃度が計算で超過した場合に表示される
                if (analysisError == AnalysisError.HighError)
                {
                    concentration = measureProtocol.ConcDynamicRange.Max;
                }
                else
                {
                    concentration = measureProtocol.ConcDynamicRange.Min;
                }

                if (sampKind == SampleKind.Sample || sampKind == SampleKind.Priority)
                {
                    // 希釈倍率演算
                    if (measureProtocol.UseAfterDilAtCalcu)
                    {
                        concentration *= autoDil;
                    }

                    if (measureProtocol.UseManualDilAtCalcu)
                    {
                        concentration *= manualDil;
                    }
                }

                calcConcData.Concentration = Math.Round(concentration, measureProtocol.LengthAfterDemPoint, MidpointRounding.AwayFromZero);
                return true;
			}
            // 相関係数AおよびBの計算が最初に実行され、それが上限および下限に等しいかそれより大きいかどうかが決定される。
            // 相関係数かける(非ダイナミックレンジエラー)́

            //【IssuesNo:7】试剂厂商需求，将相关系数A、B分成质控品和样本
            if(sampKind == SampleKind.Control)
            {
                concentration *= measureProtocol.ControlGainOfCorrelation;
                concentration += measureProtocol.ControlOffsetOfCorrelation;
            }
            else
            {
                concentration *= measureProtocol.GainOfCorrelation;
                concentration += measureProtocol.OffsetOfCorrelation;
            }
            // ダイナミックレンジ判定
            if ( measureProtocol.ConcDynamicRange.Max < concentration )
			{
				// 濃度をダイナミックレンジ最大値に設定
				concentration = measureProtocol.ConcDynamicRange.Max;
				AddErrorInfo( AnalysisError.DynamicHighErr, analysisErrorInfoList, calcConcData.Remark );
			}
			else if ( measureProtocol.ConcDynamicRange.Min > concentration )
			{
				// 濃度をダイナミックレンジ最小値に設定
				concentration = measureProtocol.ConcDynamicRange.Min;
				AddErrorInfo( AnalysisError.DynamicLowErr, analysisErrorInfoList, calcConcData.Remark );
			}

			// GH,GL時も希釈倍率演算を行う
			if ( sampKind == SampleKind.Sample || sampKind == SampleKind.Priority )
			{
				// 希釈倍率演算
				if ( measureProtocol.UseAfterDilAtCalcu )
				{
					concentration *= autoDil;                    
                }

				if ( measureProtocol.UseManualDilAtCalcu )
				{
					concentration *= manualDil;
				}
			}

			calcConcData.Concentration = Math.Round( concentration, measureProtocol.LengthAfterDemPoint, MidpointRounding.AwayFromZero );

			return true;
		}

		#endregion

		#region _検量線作成_

		/// <summary>
		/// 検量線登録実施可能チェック
		/// </summary>
		/// <remarks>
		/// 検量線登録実施可能チェックします
		/// </remarks>
		/// <param name="measureProtocol">分析項目</param>
		/// <param name="checkCurveData">検量線登録元データ(キャリブレータ測定結果)</param>
		/// <returns>true:検量線登録可能</returns>
		private static Boolean CheckCalibrationCurveRegist( MeasureProtocol measureProtocol, IEnumerable<IGrouping<int, CalibratorResultData>> checkCurveData )
		{
			foreach ( var data in checkCurveData )
			{
				// 分析項目のキャリブレータの最終多重測定回の平均リマークに温度系エラー、有効期限系エラー、ホスト送信エラー以外のエラーの発生を確認し
				// 各測定ポイント毎に有効データ(温度系エラー、有効期限系エラー、ホスト送信エラー、洗浄不良エラー以外のエラーがないデータ)が存在しないか確認し、なければfalse
				if ( measureProtocol.RepNoForCalib > 1 )
				{
                    //when RemarkID is not null,this shows it will be average data(the last one)
					var averageData = data.SingleOrDefault( ( aveData ) => aveData.GetRemarkId() != null );
					if ( averageData != null )
					{
						Remark remarkAve = averageData.GetRemarkId();
						if ( !remarkAve.CanRegistCurve )
						{
							// 平均リマークに対して、平均リマークにのみ発生する濃度計算不可項目があるか確認し、
							// 該当する場合はfalseを返す
							if ( remarkAve.IsAverageOnlyCantRegistCurve )
							{
								return false;
							}
							else
							{
                                // 複数の計算の場合、異常な結果が1つしかない場合、検量線を生成することができる。
                                int nErrorTimes = 0; 
								foreach ( var remarkRep in data )
								{
									Remark remark = remarkRep.GetReplicationRemarkId();
									if ( !remark.CanRegistCurve )
									{
                                        // エラー判定回数をカウントアップ
                                        ++nErrorTimes;
									}
								}

                                // 2つ以上のエラーがある場合、検量線の生成は許可されません。
                                if (nErrorTimes >= 2)
                                {
                                    return false;
                                }
							}
						}
					}
				}
				else
				{
					// 分析項目のキャリブレータの非多重測定のリマークに温度系エラー、有効期限系エラー、ホスト送信エラー以外のエラーの発生を確認し
					// 発生の場合、false
					var replicationData = data.SingleOrDefault();
					if ( replicationData != null )
					{
						Remark remark = replicationData.GetReplicationRemarkId();
						if ( !remark.CanRegistCurve )
						{
							return false;
						}
					}
				}
			}
			return true;
		}
		
        /// <summary>
        /// テストコード
        /// </summary>
        public static void Curvetest()
        {
            ItemPoint[] items = new ItemPoint[5];
            //items[0] = new ItemPoint(52923, 60);
            //items[1] = new ItemPoint(141362, 20);
            //items[2] = new ItemPoint(345915, 6);
            //items[3] = new ItemPoint(767227, 2);
            //items[4] = new ItemPoint(1118331, 0.6);
            //items[5] = new ItemPoint(1525452, 0.2);

            items[0] = new ItemPoint(5832, 2.5);
            items[1] = new ItemPoint(16452, 10);
            items[2] = new ItemPoint(159581, 100);
            items[3] = new ItemPoint(870452, 1000);
            items[4] = new ItemPoint(1469804, 5000);
            //items[5] = new ItemPoint(300000, 60);



            //items[0] = new ItemPoint(10, 0);
            //items[1] = new ItemPoint(50, 20);
            //items[2] = new ItemPoint(3000, 30);
            //items[3] = new ItemPoint(6000, 40);
            //items[4] = new ItemPoint(1118331, 200);
            //items[5] = new ItemPoint(1525452, 500);

            //items[0] = new ItemPoint(52923, 20);
            //items[1] = new ItemPoint(141362, 60);
            //items[2] = new ItemPoint(345915, 200);
            //items[3] = new ItemPoint(767227, 600);
            //items[4] = new ItemPoint(1118331, 2000);
            //items[5] = new ItemPoint(1525452, 6000);
            //items[0] = new ItemPoint(1525452, 0.4);
            //items[1] = new ItemPoint(1118331, 1);
            //items[2] = new ItemPoint(767227, 5);
            //items[3] = new ItemPoint(345915, 10);
            //items[4] = new ItemPoint(141362, 50);
            //items[5] = new ItemPoint(52923, 100);

            //MeasureProtocol measureProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(1);

            //bool bResult = JudgeCurveEntry(items, measureProtocol);
            //List<AnalysisErrorInfo> errorList;
            //CreateCalibCurveData(1, "S003", 1,9006, out errorList);

             //SlaveCommCommand_0503 command
            //onAssayData( SlaveCommCommand_0503 command )
            
        }

		/// <summary>
		/// 検量線登録可能判定(単調増加/単調減少)
		/// </summary>
		/// <remarks>
		///  検量線登録可能判定(単調増加/単調減少)します
		/// </remarks>
		/// <param name="points">判定対象検量線ポイントリスト</param>
		/// <param name="measureProtocol">分析項目</param>
		/// <returns>true:単純増加or単純減少</returns>
		public static bool JudgeCurveEntry( ItemPoint[] points, MeasureProtocol measureProtocol )
		{

			// 測定ポイント無しは無効
			if ( points.Length <= 0 )
			{
				return false;
			}

			#region 定性項目の登録可否判定
			// 抑制率 カットオフ
			if ( points.Length == 2 )
			{
				if ( measureProtocol.CalibType == MeasureProtocol.CalibrationType.INH )
				{
					// 抑制率はNegaの方が大きい
					if ( points[0].yPos >= points.Max( ( point ) => point.yPos ) )
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else if ( measureProtocol.CalibType == MeasureProtocol.CalibrationType.CutOff )
				{
					// カットオフはPosiの方が大きい
					if ( points[1].yPos >= points.Max( ( point ) => point.yPos ) )
					{
						return true;
					}
					else
					{
						return false;
					}
				}
			}
			#endregion

			int dir = 0;
			Double nowCount = 0.0;
			Double beforeCount = 0;
			// Logit-logのみ元に戻す(30ポイントサーチ)
			if ( measureProtocol.CalibType == MeasureProtocol.CalibrationType.LogitLog )
			{
                //ReCoefCalc = false;
                // 計算メソッド取得
                //2103/7/25
                //ICalcMethod method = GetCalcMethod(measureProtocol, points);
                ICalcMethod method = GetCalcMethod(measureProtocol, points,false);
               
                Double div = measureProtocol.ConcDynamicRange.Max / 30;
                if (method != null)
                {
                    // 濃度計算
                    nowCount = method.GetY(div);
                    if (!calcCountPermissibleError.Contains(method.GetError().ToAnalysisError()))
                    {
                        return false;
                    }
                    
                    beforeCount = method.GetY(0.0);

                    if (Double.IsNaN(beforeCount) || Double.IsInfinity(beforeCount))//最適化オプションにより Nan->Infinity
                    {
                        //ReCoefCalc = true;
                        method = GetCalcMethod(measureProtocol, points, true);
                        beforeCount = method.GetY(0.0);
                        if (!calcCountPermissibleError.Contains(method.GetError().ToAnalysisError()))
                        {
                            return false;
                        }
                        nowCount = method.GetY(div);
                        if (!calcCountPermissibleError.Contains(method.GetError().ToAnalysisError()))
                        {
                            return false;
                        }

                        //右上がりならば return false
                        if (nowCount > beforeCount)
                        {
                            return false;
                        }
                    }

                    if (!calcCountPermissibleError.Contains(method.GetError().ToAnalysisError()))
                    {
                        return false;
                    }

                }

                if (nowCount > beforeCount)
                {
                    dir = 1;// 右上がり
                }
                else if (nowCount < beforeCount)
                {
                    dir = 2;// 右下がり
                }
                else
                {
                    return false;
                }

                for (Double p = div * 2; p <= measureProtocol.ConcDynamicRange.Max; p = p + div)
                {
                    beforeCount = nowCount;
                    nowCount = method.GetY(p);
                    if (!calcCountPermissibleError.Contains(method.GetError().ToAnalysisError()))
                    {
                        return false;
                    }

                    if (nowCount > beforeCount)
                    {
                        if (dir != 1)
                        {
                            return false;
                        }
                    }
                    else if (nowCount < beforeCount)
                    {
                        if (dir != 2)
                        {
                            return false;
                        }
                    }
                    // 濃度値が大きくなるとnowCountとbeforeCountがほぼおなじになり
                    // バイブレーションと判定されるのでイコールでもOkとした
                    else 
                    {
                        if (Double.IsNaN(nowCount) || Double.IsInfinity(nowCount))
                        {
                            return false;
                        }
                    }
                }

                // テストコード
                // double concentration = method.GetX(1525452);
			}
			else
			{
				// 濃度からカウントを求めて判断するように修正
				ConcentrationCalculator calc = new ConcentrationCalculator();
				calc.SetCalcData( points );
				ItemPoint[] gPoints = null;
				lastExtParam = null;

                // スプライン
				if ( measureProtocol.CalibType == MeasureProtocol.CalibrationType.Spline )
				{
					gPoints = calc.GetGraphPointSpline();
				}
                // 4パラメータ
				else if ( measureProtocol.CalibType == MeasureProtocol.CalibrationType.FourParameters )
				{
                    Oelco.Common.Const.FourPTypeStruct Fparameter;
                    Fparameter.PType = (Oelco.Common.Const.FourPType)measureProtocol.FourPrameterMethodType;
                    Fparameter.ValueK = measureProtocol.FourPrameterMethodKValue;
                    var calced = calc.GetGraphPointWithCoefFourParameter(Fparameter);
					gPoints = calced.Item1;
					lastExtParam = new List<String>();
					// 4Parameterの算出された係数はDBへ保存を行う為、判定の際に保持する。
					lastExtParam.Add( calced.Item2.ToString() );
					lastExtParam.Add( calced.Item3.ToString() );
					lastExtParam.Add( calced.Item4.ToString() );
					lastExtParam.Add( calced.Item5.ToString() );
				}
                // 両対数１次
				else if ( measureProtocol.CalibType == MeasureProtocol.CalibrationType.DoubleLogarithmic1 )
				{
					gPoints = calc.GetGraphPointDoubleLogOne( DoubleLogarithmicMethod.CalcMode.Linear );
				}
                // 両対数２次
                else if ( measureProtocol.CalibType == MeasureProtocol.CalibrationType.DoubleLogarithmic2 )
				{
					gPoints = calc.GetGraphPointDoubleLogTwo( DoubleLogarithmicMethod.CalcMode.Linear );
				}

				beforeCount = gPoints[0].yPos;
				nowCount = gPoints[1].yPos;
				if ( nowCount > beforeCount )
				{
					dir = 1;    // 右上がり
				}
				else if ( nowCount < beforeCount )
				{
					// 定性項目の右下がりは禁止
					if ( measureProtocol.CalibType.IsQualitative() )
					{
						return false;
					}
					dir = 2;    // 右下がり
				}
				else
				{
					return false;
				}

				for ( int idx = 2; idx < gPoints.Length; idx++ )
				{
					beforeCount = nowCount;
					nowCount = gPoints[idx].yPos;
					if ( nowCount > beforeCount )
					{
						if ( dir != 1 )
						{
							return false;
						}
					}
					else if ( nowCount < beforeCount )
					{
						if ( dir != 2 )
						{
							return false;
						}
					}
					else
					{
						return false;
					}
				}
			}

			return true;
		}

		#endregion

		#endregion
	}

	/// <summary>
	/// 計算データクラス
	/// </summary>
	public class CalcData
	{
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="protocolIndex">分析項インデックス</param>
        /// <param name="reagentLotNo">試薬ロット番号</param>
        /// <param name="invaduallyNo">検体識別番号</param>
        /// <param name="uniqueNo">ユニーク番号</param>
        /// <param name="replicationNo">多重測定回</param>
        /// <param name="manualDilution">手希釈倍率</param>
        /// <param name="autoDilution">自動希釈倍率</param>
        /// <param name="measureDateTime">測定日時</param>
        /// <param name="rackID">ラックID</param>
        /// <param name="rackPosition">ラックポジション</param>
        public CalcData(Int32 moduleNo,
            Int32 protocolIndex,
            String reagentLotNo,
            Int32 invaduallyNo,
            Int32 uniqueNo,
            Int32 replicationNo,
            Int32 manualDilution,
            Int32 autoDilution,
            DateTime measureDateTime,
            CarisXIDString rackID = null,
            Int32? rackPosition = null,
            String sampleID = null)
        {
            this.ModuleNo = moduleNo;
            this.ProtocolIndex = protocolIndex;
            this.ReagentLotNo = reagentLotNo;
            this.IndividuallyNo = invaduallyNo;
            this.UniqueNo = uniqueNo;
            this.ReplicationNo = replicationNo;
            this.ManualDilution = manualDilution;
            this.AutoDilution = autoDilution;
            this.MeasureDateTime = measureDateTime;
            this.RackID = rackID;
            this.RackPosition = rackPosition;
            this.SampleID = sampleID;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="protocolIndex">分析項インデックス</param>
        /// <param name="reagentLotNo">試薬ロット番号</param>
        /// <param name="invaduallyNo">検体識別番号</param>
        /// <param name="uniqueNo">ユニーク番号</param>
        /// <param name="replicationNo">多重測定回</param>
        /// <param name="manualDilution">手希釈倍率</param>
        /// <param name="autoDilution">自動希釈倍率</param>
        /// <param name="measureDateTime">測定日時</param>
        /// <param name="receiptNumber">測定日時</param>【IssuesNo:9】IGRA项目需要
        /// <param name="rackID">ラックID</param>
        /// <param name="rackPosition">ラックポジション</param>
        public CalcData( Int32 moduleNo,
            Int32 protocolIndex,
			String reagentLotNo,
			Int32 invaduallyNo,
			Int32 uniqueNo,
			Int32 replicationNo,
			Int32 manualDilution,
			Int32 autoDilution,
			DateTime measureDateTime,
            Int32 receiptNumber,
			CarisXIDString rackID = null,
			Int32? rackPosition = null,
			String sampleID = null )
		{
            this.ModuleNo = moduleNo;
			this.ProtocolIndex = protocolIndex;
			this.ReagentLotNo = reagentLotNo;
			this.IndividuallyNo = invaduallyNo;
			this.UniqueNo = uniqueNo;
			this.ReplicationNo = replicationNo;
			this.ManualDilution = manualDilution;
			this.AutoDilution = autoDilution;
			this.MeasureDateTime = measureDateTime;
            this.ReceiptNumber = receiptNumber;
			this.RackID = rackID;
			this.RackPosition = rackPosition;
			this.SampleID = sampleID;
		}

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 分析項目インデックス
        /// </summary>
        public Int32 ModuleNo
        {
            get;
            private set;
        }

        /// <summary>
        /// 分析項目インデックス
        /// </summary>
        public Int32 ProtocolIndex
		{
			get;
			private set;
		}

		/// <summary>
		/// 試薬ロット番号
		/// </summary>
		public String ReagentLotNo
		{
			get;
			private set;
		}

		/// <summary>
		/// 個体識別番号
		/// </summary>
		public Int32 IndividuallyNo
		{
			get;
			private set;
		}

		/// <summary>
		/// ユニーク番号
		/// </summary>
		public Int32 UniqueNo
		{
			get;
			private set;
		}

		/// <summary>
		/// 多重測定回数番号
		/// </summary>
		public Int32 ReplicationNo
		{
			get;
			private set;
		}

		/// <summary>
		/// ラックID
		/// </summary>
		public CarisXIDString RackID
		{
			get;
			private set;
		}

		/// <summary>
		/// ラックポジション
		/// </summary>
		public Int32? RackPosition
		{
			get;
			private set;
		}

		/// <summary>
		/// 検体ID/キャリブレータロット/精度管理検体ロット
		/// </summary>
		public String SampleID
		{
			get;
			private set;
		}

		/// <summary>
		/// 手希釈倍率
		/// </summary>
		public Int32 ManualDilution
		{
			get;
			private set;
		}

		/// <summary>
		/// 自動希釈倍率
		/// </summary>
		public Int32 AutoDilution
		{
			get;
			private set;
		}

		/// <summary>
		/// レプリケーション別計算情報
		/// </summary>
		public CalcInfo CalcInfoReplication
		{
			get;
			set;
		}

		/// <summary>
		/// 平均計算情報
		/// </summary>
		public CalcInfo CalcInfoAverage
		{
			get;
			set;
		}

		/// <summary>
		/// 定性項目判定結果
		/// </summary>
		public String Judgement
		{
			get;
			set;
		}

		/// <summary>
		/// 計算結果使用検量線
		/// </summary>
		public DateTime? UseCalcCalibCurveApprovalDate
		{
			get;
			set;
		}

		/// <summary>
		/// 測定日時
		/// </summary>
		public DateTime MeasureDateTime
		{
			get;
			set;
		}

		/// <summary>
		/// 分析ステータス
		/// </summary>
		public SampleInfo.SampleMeasureStatus AssayStatus
		{
			get;
			set;
		}

        /// <summary>
        /// 【IssuesNo:9】IGRA项目重新计算所需要
        /// 受付番号 設定/取得
        /// </summary>
        public Int32 ReceiptNumber
        {
            get;
            private set;
        }
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コピー
        /// </summary>
        /// <remarks>
        /// 計算データをコピーします
        /// </remarks>
        /// <returns>コピー済計算データ</returns>
        public CalcData Copy()
		{
			return new CalcData( this.ModuleNo, this.ProtocolIndex, this.ReagentLotNo, this.IndividuallyNo, this.UniqueNo, this.ReplicationNo, this.ManualDilution, this.AutoDilution, this.MeasureDateTime )
			{
				CalcInfoReplication = ( this.CalcInfoReplication != null ) ? new CalcInfo( this.CalcInfoReplication.CountValue )
				{
					Remark = this.CalcInfoReplication.Remark.Value,
					Concentration = this.CalcInfoReplication.Concentration
				} : null,
				CalcInfoAverage = ( this.CalcInfoAverage != null ) ? new CalcInfo( this.CalcInfoAverage.CountValue )
				{
					Remark = this.CalcInfoAverage.Remark.Value,
					Concentration = this.CalcInfoAverage.Concentration
				} : null
			};
		}

		#endregion
	}

	/// <summary>
	/// 計算情報
	/// </summary>
	public class CalcInfo
	{
		#region [コンストラクタ/デストラクタ]

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="countValue">カウント値</param>
		public CalcInfo( Int32? countValue )
		{
			this.CountValue = countValue;
			this.Remark = new Remark();
		}

		#endregion

		#region [プロパティ]

		/// <summary>
		/// カウント値の取得、設定
		/// </summary>
		public Int32? CountValue
		{
			get;
			set;
		}

		/// <summary>
		/// 濃度値の取得、設定
		/// </summary>
		public Double? Concentration
		{
			get;
			set;
		}

		/// <summary>
		/// リマークの取得、設定
		/// </summary>
		public Remark Remark
		{
			get;
			set;
		}

		#endregion

		#region [publicメソッド]

		/// <summary>
		/// 表示用カウント値の取得
		/// </summary>
		/// <remarks>
		/// 表示用カウント値を取得します。
		/// </remarks>
		/// <returns>表示用カウント値</returns>
		public String GetDisplayCount()
		{
			return ( Remark.CanCalcCount && this.CountValue.HasValue ) ? this.CountValue.ToString() : CarisXConst.COUNT_CONCENTRATION_NOTHING;
		}

		/// <summary>
		/// 表示用濃度値の取得
		/// </summary>
		/// <remarks>
		/// 表示用濃度地を取得します。
		/// </remarks>
		/// <param name="protocolIndex">分析項目インデックス</param>
		/// <returns>表示用濃度値</returns>
		public String GetDisplayConcentration( Int32 protocolIndex )
		{
			Int32 digits = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( protocolIndex ).LengthAfterDemPoint;
			return ( Remark.CanCalcConcentration && this.Concentration.HasValue ) ? SubFunction.ToRoundOffParse( this.Concentration.Value, digits ) : CarisXConst.COUNT_CONCENTRATION_NOTHING;
		}

		#endregion

	}

	/// <summary>
	/// 計算データ拡張クラス
	/// </summary>
	public static class CalcDataExtension
	{
		#region [publicメソッド]

		/// <summary>
		/// 測定データから計算データへの変換
		/// </summary>
		/// <remarks>
		/// 測定データから計算データへの変換を行います。
		/// </remarks>
		/// <param name="resultData">変換対象の測定データ</param>
		/// <param name="addExpirationDateErrorRemark">有効期限エラー付加フラグ(true:付加する)</param>
		/// <returns>計算データ</returns>
		public static CalcData Convert( this IMeasureResultData resultData, Boolean addExpirationDateErrorRemark = false )
		{
			CalcData data = new CalcData( resultData.ModuleID
                , Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo( resultData.MeasProtocolNumber ).ProtocolIndex
				, resultData.ReagentLotNumber
				, resultData.IndividuallyNumber
				, resultData.UniqueNo
				, resultData.RepNo
				, resultData.PreDilution
				, resultData.AfterDilution
				, DateTime.Now
				, resultData.RackID
				, resultData.SamplePos
				, resultData.SampleId );
			data.CalcInfoReplication = new CalcInfo( null )
			{
				Remark = resultData.Remark
			};

			if ( addExpirationDateErrorRemark )
			{
                //2015/5/8 有効期限切れリマーク表示しない--------------------------
                //if ( HybridDataMediator.SearchReagentExpirationDateErrorFromReagentDB( ReagentKind.Pretrigger, resultData.PreTriggerLotNo, data.MeasureDateTime ) )
                //{
                //    // リマーク追加[プレトリガ有効期限エラー]
                //    data.CalcInfoReplication.Remark.AddRemark( Remark.RemarkBit.PreTriggerExpirationDateError );
                //}
                //if ( HybridDataMediator.SearchReagentExpirationDateErrorFromReagentDB( ReagentKind.Trigger, resultData.TriggerLotNo, data.MeasureDateTime ) )
                //{
                //    // リマーク追加[トリガ有効期限エラー]
                //    data.CalcInfoReplication.Remark.AddRemark( Remark.RemarkBit.TriggerExpirationDateError );
                //}
                //------------------------------------------------------------------
				//2013/1/30 一時的に削除　アルゴリズムを次Verで変更
				//if ( HybridDataMediator.SearchReagentExpirationDateErrorFromReagentDB( ReagentKind.Diluent, null, data.MeasureDateTime ) )
				//{
				//    // リマーク追加[希釈液有効期限エラー]
				//    data.CalcInfoReplication.Remark.AddRemark( Remark.RemarkBit.DilutionExpirationDateError );
				//}
			}

			return data;
		}

		#endregion

	}
}
