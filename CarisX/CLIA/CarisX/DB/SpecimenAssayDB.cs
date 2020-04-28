using System;
using System.Collections.Generic;
using System.Linq;

using Oelco.Common.Utility;
using Oelco.CarisX.Utility;

using Oelco.Common.Log;
using Oelco.CarisX.Const;
using Oelco.Common.DB;
using Oelco.CarisX.Parameter;
using System.Data;
using Oelco.CarisX.Common;
using Oelco.CarisX.Calculator;
using Oelco.CarisX.Log;
using Oelco.Common.Parameter;


namespace Oelco.CarisX.DB
{
    /// <summary>
    /// 検体分析データクラス
    /// </summary>
    public class SpecimenAssayData : DataRowWrapperBase
    {

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data"></param>
        public SpecimenAssayData( DataRowWrapperBase data )
            : base( data )
        {
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// ユニーク番号の取得、設定
        /// </summary>
        protected Int32 UniqueNo
        {
            get
            {
                return this.Field<Int32>( SpecimenAssayDB.STRING_UNIQUENO );
            }
            set
            {
                this.SetField<Int32>( SpecimenAssayDB.STRING_UNIQUENO, value );
            }
        }

        /// <summary>
        /// レプリケーション番号の取得、設定
        /// </summary>
        public Int32 ReplicationNo
        {
            get
            {
                return this.Field<Int32>( SpecimenAssayDB.STRING_REPLICATIONNO );
            }
            protected set
            {
                this.SetField<Int32>( SpecimenAssayDB.STRING_REPLICATIONNO, value );
            }
        }

        /// <summary>
        /// ラックポジションの取得、設定
        /// </summary>
        public Int32? RackPosition
        {
            get
            {
                return this.Field<Int32?>( SpecimenAssayDB.STRING_RACKPOSITION );
            }
            protected set
            {
                this.SetField<Int32?>( SpecimenAssayDB.STRING_RACKPOSITION, value );
            }
        }

        /// <summary>
        /// 検体識別番号の取得、設定
        /// </summary>
        protected Int32 IndividuallyNo
        {
            get
            {
                return this.Field<Int32>( SpecimenAssayDB.STRING_INDIVIDUALLYNO );
            }
            set
            {
                this.SetField<Int32>( SpecimenAssayDB.STRING_INDIVIDUALLYNO, value );
            }
        }

        /// <summary>
        /// シーケンス番号の取得、設定
        /// </summary>
        public Int32 SequenceNo
        {
            get
            {
                return this.Field<Int32>( SpecimenAssayDB.STRING_SEQUENCENO );
            }
            protected set
            {
                this.SetField<Int32>( SpecimenAssayDB.STRING_SEQUENCENO, value );
            }
        }

        /// <summary>
        /// 受付番号の取得、設定
        /// </summary>
        public Int32 ReceiptNumber
        {
            get
            {
                return this.Field<Int32>( SpecimenAssayDB.STRING_RECEIPTNUMBER );
            }
            protected set
            {
                this.SetField<Int32>( SpecimenAssayDB.STRING_RECEIPTNUMBER, value );
            }
        }

        /// <summary>
        /// 検体IDの取得、設定
        /// </summary>
        public String PatientId
        {
            get
            {
                return this.Field<String>( SpecimenAssayDB.STRING_PATIENTID );
            }
            protected set
            {
                this.SetField<String>( SpecimenAssayDB.STRING_PATIENTID, value );
            }
        }

        /// <summary>
        /// 分析項目インデックスの取得、設定
        /// </summary>
        protected Int32 MeasureProtocolIndex
        {
            get
            {
                return this.Field<Int32>( SpecimenAssayDB.STRING_MEASUREPROTOCOLINDEX );
            }
            set
            {
                this.SetField<Int32>( SpecimenAssayDB.STRING_MEASUREPROTOCOLINDEX, value );
            }
        }

        /// <summary>
        /// ステータスの取得、設定
        /// </summary>
        protected SampleInfo.SampleMeasureStatus Status
        {
            get
            {
                return (SampleInfo.SampleMeasureStatus)this.Field<Int32>( SpecimenAssayDB.STRING_STATUS );
            }
            set
            {
                this.SetField<Int32>( SpecimenAssayDB.STRING_STATUS, (Int32)value );
            }
        }

        /// <summary>
        /// 残り時間の取得、設定
        /// </summary>
        public TimeSpan? RemainTime
        {
            get
            {
                Int64? field = this.Field<Int64?>( SpecimenAssayDB.STRING_REMAINTIME );
                if ( field.HasValue )
                {
                    return new TimeSpan( field.Value );
                }
                else
                {
                    return null;
                }
            }
            protected set
            {
                if ( value.HasValue )
                {
                    this.SetField<Int64?>( SpecimenAssayDB.STRING_REMAINTIME, value.Value.Ticks );
                }
                else
                {
                    this.SetField<Int64?>( SpecimenAssayDB.STRING_REMAINTIME, null );
                }
            }
        }

        /// <summary>
        /// カウント値の取得、設定
        /// </summary>
        protected Int32? CountValue
        {
            get
            {
                return this.Field<Int32?>( SpecimenAssayDB.STRING_COUNT );
            }
            set
            {
                this.SetField<Int32?>( SpecimenAssayDB.STRING_COUNT, value );
            }
        }

        public double? ConcentrationWithoutUnit
        {
            get
            {
                string conc = this.Field<String>(SpecimenAssayDB.STRING_CONCENTRATION);
                if (String.IsNullOrEmpty(conc))
                {
                    return null;
                }
                else
                {
                    //double.Parse(conc) != 0
                    double result ;
                    double.TryParse(conc, out result);
                    return result;
                }
            }

        }

        /// <summary>
        /// 濃度値の取得、設定
        /// </summary>
        public String Concentration
        {
            get
            {
                var conc = this.Field<String>( SpecimenAssayDB.STRING_CONCENTRATION );
                if ( !String.IsNullOrEmpty( conc ) || this.RemarkId == null || this.RemarkId.CanCalcConcentration )
                {
                    String unit = String.Empty;
                    String sign = String.Empty;
                    var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( this.MeasureProtocolIndex );
                    if ( !String.IsNullOrEmpty( conc ) && protocol != null )
                    {
                        // 表示する数値と濃度の間にスペースを含める
                        unit = CarisX.Properties.Resources.STRING_COMMON_013 + protocol.ConcUnit;
                        if (this.RemarkId != null)
                        {
                            if (this.RemarkId.HasRemark(Utility.Remark.RemarkBit.DynamicrangeUpperError))
                            {
                                sign = CarisXConst.CONCENTRATION_GREATER;
                            }
                            else if (this.RemarkId.HasRemark(Utility.Remark.RemarkBit.DynamicrangeLowerError))
                            {
                                if (double.Parse(conc) != 0)
                                {
                                    sign = CarisXConst.CONCENTRATION_LESS;
                                }
                            }
                        }
                    }

                    return sign + conc + unit;
                }

                return CarisXConst.COUNT_CONCENTRATION_NOTHING;
            }

            protected set
            {
                this.SetField<String>( SpecimenAssayDB.STRING_CONCENTRATION, value );
            }
        }

        /// <summary>
        /// 判定の取得、設定
        /// </summary>
        public String Judgement
        {
            get
            {
                return this.Field<String>( SpecimenAssayDB.STRING_JUDGEMENT );
            }
            protected set
            {
                this.SetField<String>( SpecimenAssayDB.STRING_JUDGEMENT, value );
            }
        }

        /// <summary>
        /// 手希釈倍率の取得、設定
        /// </summary>
        public Int32? ManualDilution
        {
            get
            {
                return this.Field<Int32?>( SpecimenAssayDB.STRING_MANUALDILUTION );
            }
            protected set
            {
                this.SetField<Int32?>( SpecimenAssayDB.STRING_MANUALDILUTION, value );
            }
        }

        /// <summary>
        /// 自動希釈倍率の取得、設定
        /// </summary>
        public Int32? AutoDilution
        {
            get
            {
                return this.Field<Int32?>( SpecimenAssayDB.STRING_AUTODILUTION );
            }
            protected set
            {
                this.SetField<Int32?>( SpecimenAssayDB.STRING_AUTODILUTION, value );
            }
        }

        /// <summary>
        /// リマークIDの取得、設定
        /// </summary>
        protected Remark RemarkId
        {
            get
            {
                return this.Field<Int64?>( SpecimenAssayDB.STRING_REMARKID );
            }
            set
            {
                this.SetField<Int64?>( SpecimenAssayDB.STRING_REMARKID, value );
            }
        }

        /// <summary>
        /// 測定日時の取得、設定
        /// </summary>
        public DateTime? MeasureDateTime
        {
            get
            {
                return this.Field<DateTime?>( SpecimenAssayDB.STRING_MEASUREDATETIME );
            }
            protected set
            {
                this.SetField<DateTime?>( SpecimenAssayDB.STRING_MEASUREDATETIME, value );
            }
        }

        /// <summary>
        /// 試薬ロット番号の取得、設定
        /// </summary>
        public String ReagentLotNo
        {
            get
            {
                return this.Field<String>( SpecimenAssayDB.STRING_REAGENTLOTNO );
            }
            protected set
            {
                this.SetField<String>( SpecimenAssayDB.STRING_REAGENTLOTNO, value );
            }
        }

        /// <summary>
        /// プレトリガロット番号の取得、設定
        /// </summary>
        public String PretriggerLotNo
        {
            get
            {
                return this.Field<String>( SpecimenAssayDB.STRING_PRETRIGGERLOTNO );
            }
            protected set
            {
                this.SetField<String>( SpecimenAssayDB.STRING_PRETRIGGERLOTNO, value );
            }
        }

        /// <summary>
        /// トリガロット番号の取得、設定
        /// </summary>
        public String TriggerLotNo
        {
            get
            {
                return this.Field<String>( SpecimenAssayDB.STRING_TRIGGERLOTNO );
            }
            protected set
            {
                this.SetField<String>( SpecimenAssayDB.STRING_TRIGGERLOTNO, value );
            }
        }

        /// <summary>
        /// 使用検量線の取得、設定
        /// </summary>
        public DateTime? CalibCurveDateTime
        {
            get
            {
                return this.Field<DateTime?>( SpecimenAssayDB.STRING_CALIBCURVEDATETIME );
            }
            protected set
            {
                this.SetField<DateTime?>( SpecimenAssayDB.STRING_CALIBCURVEDATETIME, value );
            }
        }

        /// <summary>
        /// 再検査の取得、設定
        /// </summary>
        protected Int32? ReMeasure
        {
            get
            {
                return this.Field<Int32?>( SpecimenAssayDB.STRING_REMEASURE );
            }
            set
            {
                this.SetField<Int32?>( SpecimenAssayDB.STRING_REMEASURE, value );
            }
        }

        /// <summary>
        /// コメントの取得、設定
        /// </summary>
        public String Comment
        {
            get
            {
                return this.Field<String>( SpecimenAssayDB.STRING_COMMENT );
            }
            protected set
            {
                this.SetField<String>( SpecimenAssayDB.STRING_COMMENT, value );
            }
        }

        /// <summary>
        /// 分析モードの取得、設定
        /// </summary>
        public String AnalysisMode
        {
            get
            {
                Int32? field = this.Field<Int32?>(SpecimenAssayDB.STRING_ANALYSISMODE);
                if (field != null)
                {
                    return ((AnalysisModeKind)field).ToTypeString();
                }
                return null;
            }
            protected set
            {
                AnalysisModeKind type = (AnalysisModeKind)0;
                value.SetTypeFromString(ref type);
                this.SetField<Int32?>(SpecimenAssayDB.STRING_ANALYSISMODE, (Int32)type);
            }
        }


        /// <summary>
        /// 分析項目名の取得
        /// </summary>
        public String Analytes
        {
            get
            {
                var measureProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( this.MeasureProtocolIndex );
                if ( measureProtocol != null )
                {
                    return measureProtocol.ProtocolName;
                }
                return null;
            }
        }

        /// <summary>
        /// ラックIDの取得、設定
        /// </summary>
        public CarisXIDString RackId
        {
            get
            {
                return this.Field<String>( SpecimenAssayDB.STRING_RACKID );
            }
            protected set
            {
                this.SetField<String>( SpecimenAssayDB.STRING_RACKID, value.DispPreCharString );
            }
        }

        /// <summary>
        /// サンプル種別の取得、設定
        /// </summary>
        public String SpecimenMaterialType
        {
            get
            {
                Int32? field = this.Field<Int32?>( SpecimenAssayDB.STRING_SPECIMENMATERIALTYPE );
                if ( field != null )
                {
                    return ( (SpecimenMaterialType)field ).ToTypeString();
                }
                return null;
            }
            protected set
            {
                SpecimenMaterialType type = (SpecimenMaterialType)0;
                value.SetTypeFromString( ref type );
                this.SetField<Int32?>( SpecimenAssayDB.STRING_SPECIMENMATERIALTYPE, (Int32)type );
            }
        }

        /// <summary>
        /// リマーク文字列の取得
        /// </summary>
        public String Remark
        {
            get
            {
                if ( this.RemarkId != null )
                {
                    return String.Join( ",", ( (Remark)this.RemarkId ).GetRemarkStrings() );
                }
                return null;
            }
        }

        /// <summary>
        /// ステータス文字列
        /// </summary>
        public String StatusString
        {
            get
            {
                return this.Status.ToTypeString();
            }
        }

        /// <summary>
        /// カウント値(文字列)の取得
        /// </summary>
        public String Count
        {
            get
            {
                if ( this.CountValue.HasValue )
                {
                    return this.CountValue.Value.ToString();
                }
                else if ( this.RemarkId.CanCalcCount )
                {
                    return null;
                }
                else
                {
                    return CarisXConst.COUNT_CONCENTRATION_NOTHING;
                }
            }
        }

        /// <summary>
        /// ダークカウント値の取得、設定
        /// </summary>
        public Int32? DarkCount
        {
            get
            {
                return this.Field<Int32?>(SpecimenAssayDB.STRING_DARKCOUNT);
            }
            set
            {
                this.SetField<Int32?>(SpecimenAssayDB.STRING_DARKCOUNT, value);
            }
        }

        /// <summary>
        /// バックグラウンドカウント値の取得、設定
        /// </summary>
        public Int32? BGCount
        {
            get
            {
                return this.Field<Int32?>(SpecimenAssayDB.STRING_BGCOUNT);
            }
            set
            {
                this.SetField<Int32?>(SpecimenAssayDB.STRING_BGCOUNT, value);
            }
        }

        /// <summary>
        /// 測定カウント値の取得、設定
        /// </summary>
        public Int32? ResultCount
        {
            get
            {
                return this.Field<Int32?>(SpecimenAssayDB.STRING_RESULTCOUNT);
            }
            set
            {
                this.SetField<Int32?>(SpecimenAssayDB.STRING_RESULTCOUNT, value);
            }
        }

        /// <summary>
        /// 分析モジュール番号の取得、設定
        /// </summary>
        public Int32 ModuleNo
        {
            get
            {
                return this.Field<Int32>(SpecimenAssayDB.STRING_MODULENO);
            }
            set
            {
                this.SetField<Int32>(SpecimenAssayDB.STRING_MODULENO, value);
            }
        }
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// モジュール番号の取得
        /// </summary>
        /// <remarks>
        /// モジュール番号を取得します。
        /// </remarks>
        public Int32 GetModuleNo()
        {
            return this.ModuleNo;
        }
        /// <summary>
        /// モジュール番号の設定
        /// </summary>
        /// <remarks>
        /// モジュール番号を取得します。
        /// </remarks>
        public void SetModuleNo(Int32 moduleNo)
        {
            this.ModuleNo = moduleNo;
        }

        /// <summary>
        /// ユニーク番号の取得
        /// </summary>
        /// <remarks>
        /// ユニーク番号を取得します。
        /// </remarks>
        public Int32 GetUniqueNo()
        {
            return this.UniqueNo;
        }
        /// <summary>
        /// ユニーク番号の設定
        /// </summary>
        /// <remarks>
        /// ユニーク番号を設定します。
        /// </remarks>
        public void SetUniqueNo( Int32 uniqueNo )
        {
            this.UniqueNo = uniqueNo;
        }
        /// <summary>
        /// レプリケーション番号の設定
        /// </summary>
        /// <remarks>
        /// レプリケーション番号を設定します
        /// </remarks>
        public void SetReplicationNo( Int32 replicationNo )
        {
            this.ReplicationNo = replicationNo;
        }
        /// <summary>
        /// ラックIDの設定
        /// </summary>
        /// <remarks>
        /// ラックIDを設定します。
        /// </remarks>
        public void SetRackId( String rackId )
        {
            this.RackId = rackId;
        }
        /// <summary>
        /// ラックポジションの設定
        /// </summary>
        /// <remarks>
        /// ラックポジションを設定します。
        /// </remarks>
        public void SetRackPosition( Int32? rackPosition )
        {
            this.RackPosition = rackPosition;
        }
        /// <summary>
        /// 検体識別番号の取得
        /// </summary>
        /// <remarks>
        /// 検体識別番号を取得します。
        /// </remarks>
        public Int32 GetIndividuallyNo()
        {
            return this.IndividuallyNo;
        }
        /// <summary>
        /// 検体識別番号の設定
        /// </summary>
        /// <remarks>
        /// 検体識別番号を設定します。
        /// </remarks>
        public void SetIndividuallyNo( Int32 individuallyNo )
        {
            this.IndividuallyNo = individuallyNo;
        }
        /// <summary>
        /// シーケンス番号の設定
        /// </summary>
        /// <remarks>
        /// シーケンス番号を設定します。
        /// </remarks>
        public void SetSequenceNo( Int32 sequenceNo )
        {
            this.SequenceNo = sequenceNo;
        }
        /// <summary>
        /// 受付番号の設定
        /// </summary>
        /// <remarks>
        /// 受付番号を返します。
        /// </remarks>
        public void SetReceiptNumber( Int32 receiptNumber )
        {
            this.ReceiptNumber = receiptNumber;
        }
        /// <summary>
        /// 検体IDの設定
        /// </summary>
        /// <remarks>
        /// 検体IDを設定します。
        /// </remarks>
        public void SetSampleId( String sampleId )
        {
            this.PatientId = sampleId;
        }
        /// <summary>
        /// 分析項目インデックスの取得
        /// </summary>
        /// <remarks>
        /// 分析項目インデックスを返します。
        /// </remarks>
        public Int32 GetMeasureProtocolIndex()
        {
            return this.MeasureProtocolIndex;
        }
        /// <summary>
        /// 分析項目インデックスの設定
        /// </summary>
        /// <remarks>
        /// 分析項目インデックスを設定します。
        /// </remarks>
        public void SetMeasureProtocolIndex( Int32 measProtocolIndex )
        {
            this.MeasureProtocolIndex = measProtocolIndex;
        }
        /// <summary>
        /// ステータスの取得
        /// </summary>
        /// <remarks>
        /// ステータスを返します。
        /// </remarks>
        public SampleInfo.SampleMeasureStatus GetStatus()
        {
            return this.Status;
        }
        /// <summary>
        /// ステータスの設定
        /// </summary>
        /// <remarks>
        /// ステータスを設定します。
        /// </remarks>
        public void SetStatus( SampleInfo.SampleMeasureStatus status )
        {
            this.Status = status;
        }
        /// <summary>
        /// 残時間の設定
        /// </summary>
        /// <remarks>
        /// 残時間を設定します。
        /// </remarks>
        /// <param name="remainTime"></param>
        public void SetRemainTime( TimeSpan? remainTime )
        {
            this.RemainTime = remainTime;
        }
        /// <summary>
        /// カウント値の取得
        /// </summary>
        /// <remarks>
        /// カウント値を返します。
        /// </remarks>
        /// <returns></returns>
        public Int32? GetCount()
        {
            return this.CountValue;
        }
        /// <summary>
        /// カウント値の取得、設定
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="count"></param>
        public void SetCount( Int32? count )
        {
            this.CountValue = count;
        }
       
        /// <summary>
        /// 濃度値の取得、設定
        /// </summary>
        /// <param name="concentration"></param>
        public void SetConcentration( String concentration )
        {
            this.Concentration = concentration;
        }
        /// <summary>
        /// 判定の取得、設定
        /// </summary>
        public void SetJudgement( String judgement )
        {
            this.Judgement = judgement;
        }
        /// <summary>
        ///  手希釈倍率の取得、設定
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="manualDilution"></param>
        public void SetManualDilution( Int32? manualDilution )
        {
            this.ManualDilution = manualDilution;
        }
        ///// <summary>
        ///// 自動希釈倍率の取得、設定
        ///// </summary>

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="autoDilution"></param>
        public void SetAutoDilution( Int32? autoDilution )
        {
            this.AutoDilution = autoDilution;
        }
        /// <summary>
        /// リマークIDの取得
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public Remark GetRemarkId()
        {
            return this.RemarkId;
        }
        ///// <summary>
        ///// リマークIDの取得、設定
        ///// </summary>
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="remarkId"></param>
        public void SetRemarkId( Remark remarkId )
        {
            this.RemarkId = remarkId;
        }
        ///// <summary>
        ///// 測定日時の取得、設定
        ///// </summary>
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="measureDateTime"></param>
        public void SetMeasureDateTime( DateTime? measureDateTime )
        {
            this.MeasureDateTime = measureDateTime;
        }
        ///// <summary>
        ///// 試薬ロット番号の取得、設定
        ///// </summary>
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="reagentLotNo"></param>
        public void SetReagentLotNo( String reagentLotNo )
        {
            this.ReagentLotNo = reagentLotNo;
        }
        ///// <summary>
        ///// プレトリガロット番号の取得、設定
        ///// </summary>
        

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="pretriggerLotNo"></param>
        public void SetPretriggerLotNo( String pretriggerLotNo )
        {
            this.PretriggerLotNo = pretriggerLotNo;
        }
        ///// <summary>
        ///// トリガロット番号の取得、設定
        ///// </summary>

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="triggerLotNo"></param>
        public void SetTriggerLotNo( String triggerLotNo )
        {
            this.TriggerLotNo = triggerLotNo;
        }
        ///// <summary>
        ///// 使用検量線の取得、設定
        ///// </summary>
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="calibCurveDateTime"></param>
        public void SetCalibCurveDateTime( DateTime? calibCurveDateTime )
        {
            this.CalibCurveDateTime = calibCurveDateTime;
        }
        /// <summary>
        /// 再検査の取得
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public Int32? GetReMeasure()
        {
            return this.ReMeasure;
        }
        ///// <summary>
        ///// 再検査の取得、設定
        ///// </summary>
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="reMeasure"></param>
        public void SetReMeasure( Int32? reMeasure )
        {
            this.ReMeasure = reMeasure;
        }
        ///// <summary>
        ///// コメントの取得、設定
        ///// </summary>

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="comment"></param>
        public void SetComment( String comment )
        {
            this.Comment = comment;
        }

        /// <summary>
        /// ダークカウント値の取得
        /// </summary>
        /// <remarks>
        /// ダークカウント値を返します。
        /// </remarks>
        /// <returns></returns>
        public Int32? GetDarkCount()
        {
            return this.DarkCount;
        }

        /// <summary>
        /// ダークカウント値の設定
        /// </summary>
        /// <remarks>
        /// ダークカウント値を設定します。
        /// </remarks>
        /// <param name="count"></param>
        public void SetDarkCount(Int32? count)
        {
            this.DarkCount = count;
        }

        /// <summary>
        /// バックグラウンドカウント値の取得
        /// </summary>
        /// <remarks>
        /// バックグラウンドカウント値を返します。
        /// </remarks>
        /// <returns></returns>
        public Int32? GetBGCount()
        {
            return this.BGCount;
        }

        /// <summary>
        /// バックグラウンドカウント値の設定
        /// </summary>
        /// <remarks>
        /// バックグラウンドカウント値を設定します。
        /// </remarks>
        /// <param name="count"></param>
        public void SetBGCount(Int32? count)
        {
            this.BGCount = count;
        }

        /// <summary>
        /// 測定カウント値の取得
        /// </summary>
        /// <remarks>
        /// 測定カウント値を返します。
        /// </remarks>
        /// <returns></returns>
        public Int32? GetResultCount()
        {
            return this.ResultCount;
        }

        /// <summary>
        /// 測定カウント値の設定
        /// </summary>
        /// <remarks>
        /// 測定カウント値を設定します。
        /// </remarks>
        /// <param name="count"></param>
        public void SetResultCount(Int32? count)
        {
            this.ResultCount = count;
        }

        #endregion

        #region [内部クラス]
        /// <summary>
        /// 列キー(グリッド向け列キー設定用)
        /// </summary>
        public struct DataKeys
        {
            public const String ReplicationNo = "ReplicationNo";
            public const String RackPosition = "RackPosition";
            public const String SequenceNo = "SequenceNo";
            public const String ReceiptNumber = "ReceiptNumber";
            public const String PatientId = "PatientId";
            public const String StatusString = "StatusString";
            public const String RemainTime = "RemainTime";
            public const String Count = "Count";
            public const String Concentration = "Concentration";
            public const String Judgement = "Judgement";
            public const String ManualDilution = "ManualDilution";
            public const String AutoDilution = "AutoDilution";
            public const String MeasureDateTime = "MeasureDateTime";
            public const String ReagentLotNo = "ReagentLotNo";
            public const String PretriggerLotNo = "PretriggerLotNo";
            public const String TriggerLotNo = "TriggerLotNo";
            public const String CalibCurveDateTime = "CalibCurveDateTime";
            public const String Comment = "Comment";
            public const String Analytes = "Analytes";
            public const String RackId = "RackId";
            public const String SpecimenMaterialType = "SpecimenMaterialType";
            public const String Remark = "Remark";
            public const String ConcentrationWithoutUnit = "ConcentrationWithoutUnit";
            public const String AnalysisMode = "AnalysisMode";
            /// <summary>
            /// ダークカウント
            /// </summary>
            public const String DarkCount = "DarkCount";
            /// <summary>
            /// バックグラウンドカウント
            /// </summary>
            public const String BGCount = "BGCount";
            /// <summary>
            /// 測定カウント
            /// </summary>
            public const String ResultCount = "ResultCount";
            /// <summary>
            /// 分析モジュール番号
            /// </summary>
            public const String ModuleNo = "ModuleNo";
        }

        #endregion
    }

    /// <summary>
    /// 検体分析ステータスDBアクセスクラス
    /// </summary>
    /// <remarks>
    /// 現在分析中の検体情報を保持します。
    /// 分析完了時、このテーブル内容は削除され、測定結果テーブルへ保存されます。
    /// </remarks>
    public class SpecimenAssayDB : DBAccessControl
    {
        #region [定数定義]

        /// <summary>
        /// モジュール番号(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_MODULENO = "moduleNo";
        /// <summary>
        /// ユニーク番号(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_UNIQUENO = "uniqueNo";
        /// <summary>
        /// レプリケーション番号(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_REPLICATIONNO = "replicationNo";
        /// <summary>
        /// ラックID(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_RACKID = "rackId";
        /// <summary>
        /// ラックポジション(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_RACKPOSITION = "rackPosition";
        /// <summary>
        /// 検体識別番号(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_INDIVIDUALLYNO = "individuallyNo";
        /// <summary>
        /// シーケンス番号(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_SEQUENCENO = "sequenceNo";
        /// <summary>
        /// 受付番号(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_RECEIPTNUMBER = "receiptNumber";
        /// <summary>
        /// 検体ID(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_PATIENTID = "patientId";
        /// <summary>
        /// 検体種別(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_SPECIMENMATERIALTYPE = "specimenMaterialType";
        /// <summary>
        /// 分析項目インデックス(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_MEASUREPROTOCOLINDEX = "measureProtocolIndex";
        /// <summary>
        /// ステータス(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_STATUS = "status";
        /// <summary>
        /// 残り時間(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_REMAINTIME = "remainTime";
        /// <summary>
        /// カウント値(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_COUNT = "count";
        /// <summary>
        /// 濃度値(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_CONCENTRATION = "concentration";
        /// <summary>
        /// 判定結果(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_JUDGEMENT = "judgement";
        /// <summary>
        /// 手希釈倍率(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_MANUALDILUTION = "manualDilution";
        /// <summary>
        /// 自動希釈倍率(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_AUTODILUTION = "autoDilution";
        /// <summary>
        /// リマーク(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_REMARKID = "remarkId";
        /// <summary>
        /// 測定日時(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_MEASUREDATETIME = "measureDateTime";
        /// <summary>
        /// 試薬ロット番号(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_REAGENTLOTNO = "reagentLotNo";
        /// <summary>
        /// プレトリガロット番号(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_PRETRIGGERLOTNO = "pretriggerLotNo";
        /// <summary>
        /// トリガロット番号(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_TRIGGERLOTNO = "triggerLotNo";
        /// <summary>
        /// 使用した検量線の成立日(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_CALIBCURVEDATETIME = "calibCurveDateTime";
        /// <summary>
        /// 再検査フラグ(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_REMEASURE = "reMeasure";
        /// <summary>
        /// コメント(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_COMMENT = "comment";
        /// <summary>
        /// 分析モード(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_ANALYSISMODE = "analysisMode";
        /// <summary>
        /// ダーク値(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_DARKCOUNT = "darkCount";
        /// <summary>
        /// バックグラウンドカウント値(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_BGCOUNT = "bGCount";
        /// <summary>
        /// 測定カウント値(DBテーブル：sampleAssay列名)
        /// </summary>
        public const String STRING_RESULTCOUNT = "resultCount";

        #endregion

        #region [プロパティ]

        // TODO:実装
        /// <summary>
        /// データテーブル取得SQL
        /// </summary>
        protected override String baseTableSelectSql
        {
            get
            {
                return "SELECT * FROM dbo.specimenAssay";
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rack"></param>
        /// <returns></returns>
        public List<SpecimenAssayData> GetData( CarisXIDString rackID = null )
        {
            List<SpecimenAssayData> result = null;

            if ( this.DataTable != null )
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    var datas = from data in dataTableList.AsParallel().Select( ( row ) => new SpecimenAssayData( row ) )
                                where ( rackID == null || ( rackID != null && rackID.DispPreCharString == data.RackId.DispPreCharString ) )
                                select data;

                    result = datas.ToList();
                }
                catch ( Exception ex )
                {
                    // DB内部に不正データ
                    Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
                }
            }

            return result ?? new List<SpecimenAssayData>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rack"></param>
        /// <returns></returns>
        public List<SpecimenAssayData> GetData(int ReceiptNo, int protocolNo)
        {
            List<SpecimenAssayData> result = null;

            if (this.DataTable != null)
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    var datas = from data in dataTableList.AsParallel().Select((row) => new SpecimenAssayData(row))
                                where (ReceiptNo == data.ReceiptNumber && protocolNo == data.GetMeasureProtocolIndex())
                                select data;

                    result = datas.ToList();
                }
                catch (Exception ex)
                {
                    // DB内部に不正データ
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                }
            }

            return result ?? new List<SpecimenAssayData>();
        }

        /// <summary>
        /// 測定結果データ(計算済み)の設定
        /// </summary>
        /// <remarks>
        /// 1分析項目単位で測定結果データを設定します。
        /// 測定結果データは1分析項目単位での分析終了時にスレーブより送信されます。
        /// </remarks>
        /// <param name="data">測定結果データ</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean SetResultData( CalcData data, IMeasureResultData measureresult)
        {
            Boolean result = false;

            try
            {
                // 分析データ設定
                var targetAssayData = this.GetData( data.RackID ).Single( ( targetData ) =>
                {
                    return ( targetData.GetUniqueNo() == data.UniqueNo && targetData.ReplicationNo == data.ReplicationNo );
                } );

                targetAssayData.SetDarkCount(measureresult.DarkCount);
                targetAssayData.SetBGCount(measureresult.BGCount);
                targetAssayData.SetResultCount(measureresult.ResultCount);

                targetAssayData.SetCount(data.CalcInfoReplication.CountValue);
                Int32 digits = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( data.ProtocolIndex ).LengthAfterDemPoint;
                if ( data.CalcInfoReplication.Concentration.HasValue )
                {
                    targetAssayData.SetConcentration( SubFunction.ToRoundOffParse( data.CalcInfoReplication.Concentration.Value, digits ) );
                }
                else
                {
                    targetAssayData.SetConcentration( null );
                }
                targetAssayData.SetJudgement( data.Judgement );

                targetAssayData.SetRemainTime( TimeSpan.FromTicks( 0 ) );
                targetAssayData.SetStatus( data.CalcInfoReplication.Remark.IsNeedReMeasure ? SampleInfo.SampleMeasureStatus.Error : SampleInfo.SampleMeasureStatus.End );

				// 時刻取得元の統一化
				targetAssayData.SetMeasureDateTime( data.MeasureDateTime );

                // 同じアイテムキャリブレータとサンプルが同時にテストされたときの時間の同期を防止します。
                targetAssayData.SetCalibCurveDateTime(data.UseCalcCalibCurveApprovalDate);

                targetAssayData.SetRemarkId( data.CalcInfoReplication.Remark );

                targetAssayData.SetReagentLotNo( data.ReagentLotNo );

                this.SetData( new List<SpecimenAssayData>() { targetAssayData } );

                // TODO:全レプリ完了時消す
                result = true;
            }
            catch ( Exception ex )
            {
                Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
            }

            return result;
        }

        /// <summary>
        /// 分析ステータス更新
        /// </summary>
        /// <remarks>
        /// 分析ステータスコマンドを受信した際に呼び出されます。
        /// 分析DBのレコードに対する更新を行います。
        /// </remarks>
        /// <param name="uniqueNo">ユニーク番号</param>
        /// <param name="replicationNo">多重測定番号</param>
        /// <param name="remain">残時間</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean AssayStatusUpdate( Int32 uniqueNo, Int32 replicationNo, TimeSpan remain )
        {
            Boolean result = true;

            //　コピーデータリストを取得
            var dataTableList = this.DataTable.AsEnumerable().ToList();

            var changeData = dataTableList.AsParallel().Select( ( row ) => new SpecimenAssayData( row ) )
                .FirstOrDefault( ( targetData ) => (targetData.GetUniqueNo() == uniqueNo)
                                                && (targetData.ReplicationNo == replicationNo) );
            if ( changeData != null )
            {
                // ステータス更新
                // スレーブ側でリマーク発生の場合、試薬節約の為分析終了サイクルまで全て行わない可能性がある。
                // その場合測定データコマンドが来た後に分析ステータスコマンドが来ると、ステータス表示がInProcessのまま残ってしまう為、
                // ここでEndの場合ステータスを変更しない。
                if ( changeData.GetStatus() != SampleInfo.SampleMeasureStatus.End )
                {
                    changeData.SetStatus( SampleInfo.SampleMeasureStatus.InProcess );
                }

                var sample = Singleton<InProcessSampleInfoManager>.Instance.SearchInProcessSampleFromUniqueNo( uniqueNo );

                // 残時間更新
                changeData.SetRemainTime( remain );
            }
            else
            {
                System.Diagnostics.Debug.WriteLine( "剩余时间更新数据库数据失败" );
                Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, "剩余时间更新数据库数据失败" );
            }

            return result;
        }

        /// <summary>
        /// 分析中DB全消去
        /// </summary>
        /// <remarks>
        /// 分析DBに残った分析中・待機の検体情報を削除します。
        /// </remarks>
        public void ClearWaitAndInprocess()
        {
            if ( this.DataTable != null )
            {
                //　コピーデータリストを取得
                var dataTableList = this.DataTable.AsEnumerable().ToList();

                var deleteData = dataTableList.Select( ( row ) => new SpecimenAssayData( row ) )
                    .Where( ( data ) => (data.GetStatus() == SampleInfo.SampleMeasureStatus.Wait)
                                     || (data.GetStatus() == SampleInfo.SampleMeasureStatus.InProcess) ).ToList();

                deleteData.DeleteAllDataList();
                deleteData.SyncDataListToDataTable( this.DataTable );
            }
        }

        /// <summary>
        /// 分析データ登録
        /// </summary>
        /// <param name="sequenceNumber"></param>
        /// <param name="data"></param>
        /// <param name="isHost"></param>
        /// <param name="isRemeasure"></param>
        /// <param name="externalReceiptNumber"></param>
        /// <param name="isPriority"></param>
        /// <returns></returns>
        protected Boolean addAssayData( Int32 sequenceNumber, AskWorkSheetData data, Boolean isHost, Boolean isRemeasure, Int32 externalReceiptNumber, Boolean isPriority )
        {

            Boolean result = true;
            try
            {
                if ( this.DataTable != null )
                {
                    Int32 receiptNumber = 0;

                    // 再検査の場合は、再検リストから検索してきた番号を使用する。
                    if (isHost || isRemeasure)
                    {
                        // 外部からの受付番号がある場合、使用する
                        // (再検査 or ホスト)
                        receiptNumber = externalReceiptNumber;
                        if ( receiptNumber == 0 )
                        {
                            // 番号指定なしの場合、生成する。
                            receiptNumber = Singleton<ReceiptNo>.Instance.CreateNumber();
                        }
                    }
                    else
                    {
                        // 検体登録情報からレシピ番号を取得
                        receiptNumber = HybridDataMediator.SearchReceiptNumberFromSpecimenRegisteredDB( data.AskData.RackID
                                                                                                      , data.AskData.SamplePosition
                                                                                                      , data.AskData.SampleID
                                                                                                      , isPriority );
                    }

                    String comment;

                    if ( isHost )
                    {
                        comment = data.FromHostCommand.Comment;
                    }
                    else
                    {
                        // 検体登録情報からコメントを取得
                        comment = HybridDataMediator.SearchCommentFromSpecimenRegisteredDB( data.AskData.RackID
                                                                                          , data.AskData.SamplePosition
                                                                                          , data.AskData.SampleID
                                                                                          , isPriority );
                    }

                    String preTriggerLot = HybridDataMediator.SearchPreTriggerLotNoFromReagentDB();
                    String triggerLot = HybridDataMediator.SearchTriggerLotNoFromReagentDB();

                    for ( int iLoop = 0; iLoop < data.AskData.MeasItemCount; iLoop++ )
                    {
                        var measItem = data.AskData.MeasItemArray[iLoop];
                        var measProto = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo( measItem.ProtoNo );
                        DateTime approvalDate = HybridDataMediator.SearchCalibDateFromCalibDB( measProto.ProtocolIndex, data.AskData.ModuleID );
                        for ( int iRepNumber = 1; iRepNumber <= measItem.RepCount; iRepNumber++ )
                        {
                            DataRow addRow = this.DataTable.NewRow();

                            addRow[SpecimenAssayDB.STRING_AUTODILUTION] = measItem.AutoDil;
                            addRow[SpecimenAssayDB.STRING_CONCENTRATION] = String.Empty;
                            addRow[SpecimenAssayDB.STRING_COUNT] = DBNull.Value;
                            addRow[SpecimenAssayDB.STRING_DARKCOUNT] = DBNull.Value;
                            addRow[SpecimenAssayDB.STRING_BGCOUNT] = DBNull.Value;
                            addRow[SpecimenAssayDB.STRING_RESULTCOUNT] = DBNull.Value;
                            addRow[SpecimenAssayDB.STRING_INDIVIDUALLYNO] = data.AskData.IndividuallyNumber;
                            addRow[SpecimenAssayDB.STRING_JUDGEMENT] = String.Empty;
                            addRow[SpecimenAssayDB.STRING_MANUALDILUTION] = data.AskData.PreDil;
                            addRow[SpecimenAssayDB.STRING_MEASUREDATETIME] = DBNull.Value;
                            addRow[SpecimenAssayDB.STRING_MEASUREPROTOCOLINDEX] = measProto.ProtocolIndex;
                            addRow[SpecimenAssayDB.STRING_PATIENTID] = data.AskData.SampleID;
                            addRow[SpecimenAssayDB.STRING_RACKID] = data.AskData.RackID;
                            addRow[SpecimenAssayDB.STRING_RACKPOSITION] = data.AskData.SamplePosition;
                            addRow[SpecimenAssayDB.STRING_REAGENTLOTNO] = String.Empty; // 測定完了時に現ロットは表示する。

                            // 受付番号
                            addRow[SpecimenAssayDB.STRING_RECEIPTNUMBER] = receiptNumber;

                            // 残り時間(サイクル回数*サイクル時間)
                            addRow[SpecimenAssayDB.STRING_REMAINTIME] = HybridDataMediator.GetRemainTime().Ticks;

                            addRow[SpecimenAssayDB.STRING_REMARKID] = Remark.REMARK_DEFAULT;
                            addRow[SpecimenAssayDB.STRING_REPLICATIONNO] = iRepNumber;
                            addRow[SpecimenAssayDB.STRING_SEQUENCENO] = sequenceNumber;
                            addRow[SpecimenAssayDB.STRING_SPECIMENMATERIALTYPE] = (Int32)data.AskData.SpecimenMaterial;
                            addRow[SpecimenAssayDB.STRING_STATUS] = (Int32)SampleInfo.SampleMeasureStatus.Wait;
                            // プレトリガロットNo
                            addRow[SpecimenAssayDB.STRING_PRETRIGGERLOTNO] = preTriggerLot;
                            // トリガロット番号
                            addRow[SpecimenAssayDB.STRING_TRIGGERLOTNO] = triggerLot;
                            // 再検査フラグ
                            addRow[SpecimenAssayDB.STRING_REMEASURE] = isRemeasure;
                            // COMMENT
                            addRow[SpecimenAssayDB.STRING_COMMENT] = comment;
                            // 検量線日付
                            // 検量線データの検索結果がない場合は何も入れない
                            if (approvalDate.CompareTo(DateTime.MinValue) != 0)
                            {
                                addRow [SpecimenAssayDB.STRING_CALIBCURVEDATETIME] = approvalDate;
                            }
                            
                            //ユニーク番号
                            addRow[SpecimenAssayDB.STRING_UNIQUENO] = measItem.UniqNo;

                            //分析モジュール番号
                            addRow[SpecimenAssayDB.STRING_MODULENO] = data.AskData.ModuleID;
                            //分析モード
                            if (measProto.UseEmergencyMode)
                            {
                                addRow[SpecimenAssayDB.STRING_ANALYSISMODE] = (Int32)AnalysisModeKind.Emergency;
                            }
                            else
                            {
                                addRow[SpecimenAssayDB.STRING_ANALYSISMODE] = (Int32)AnalysisModeKind.Standard;
                            }


                            this.DataTable.Rows.Add( addRow );
                        }
                    }

                    result = true;
                }
            }
            catch ( Exception ex )
            {
                System.Diagnostics.Debug.WriteLine( String.Format( "添加数据失败:{0}", ex.Message ) );
                Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, String.Format( "添加数据失败:{0}", ex.StackTrace ) );
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 分析データ追加
        /// </summary>
        /// <remarks>
        /// 検体の分析データ追加を行います。
        /// 分析データの追加は測定指示問合せのタイミングとなります。
        /// </remarks>
        /// <param name="data">登録データ</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean AddAssayData( AskWorkSheetData data, Boolean isHost, Boolean isRemeasure, Int32 remeasSequenceNumber = 0, Int32 remeasReceiptNumber = 0 )
        {
            Int32 sequenceNumber = 0;
            sequenceNumber = this.getSequenceNumber<SequencialSampleNo>( isRemeasure, remeasSequenceNumber );

            // 分析データ追加
            return this.addAssayData( sequenceNumber, data, isHost, isRemeasure, remeasReceiptNumber, false );
        }

        /// <summary>
        /// シーケンス番号取得
        /// </summary>
        /// <typeparam name="SequenceNumberType">シーケンス番号種別</typeparam>
        /// <param name="isReMeasure">再検査フラグ</param>
        /// <param name="remeasSequenceNumber"></param>
        /// <returns></returns>
        public Int32 getSequenceNumber<SequenceNumberType>( Boolean isReMeasure, Int32 remeasSequenceNumber ) where SequenceNumberType : NumberingBase, new()
        {
            Int32 sequenceNumber;

            if ( isReMeasure )
            {
                // 再検はシーケンス番号を生成しない
                sequenceNumber = remeasSequenceNumber;
            }
            else
            {
                /* シーケンス番号生成 */
                sequenceNumber = Singleton<SequenceNumberType>.Instance.CreateNumber();
            }

            return sequenceNumber;
        }

        /// <summary>
        /// 優先検体分析データ追加
        /// </summary>
        /// <remarks>
        /// 優先検体の分析データ追加を行います。
        /// 分析データの追加は測定指示問合せのタイミングとなります。
        /// </remarks>
        /// <param name="data">登録データ</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean AddPriorityAssayData( AskWorkSheetData data, Boolean isHost, Boolean isRemeasure, Int32 remeasSequenceNumber = 0, Int32 remeasReceiptNumber = 0 )
        {
            Int32 sequenceNumber = 0;
            sequenceNumber = this.getSequenceNumber<SequencialPrioritySampleNo>( isRemeasure, remeasSequenceNumber );

            // 分析データ追加
            return this.addAssayData( sequenceNumber, data, isHost, isRemeasure, remeasReceiptNumber, true );
        }

        /// <summary>
        /// 全データ削除
        /// </summary>
        /// <remarks>
        /// 分析ステータス情報を全て削除します。
        /// 日替わり処理・新規分析開始時に呼び出されます。
        /// </remarks>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean ClearAssayData()
        {
            Boolean reuslt = false;

            if ( this.DataTable != null )
            {
                // 全行削除
                foreach ( var row in this.DataTable.AsEnumerable() )
                {
                    row.Delete();
                }
                reuslt = true;
            }

            return reuslt;
        }

        /// <summary>
        /// 検体分析中データの設定
        /// </summary>
        /// <param name="list">変更、削除操作済みデータ</param>
        public void SetData( List<SpecimenAssayData> list )
        {
            list.SyncDataListToDataTable( this.DataTable );
        }

        /// <summary>
        /// 検体分析中情報テーブル取得
        /// </summary>
        /// <remarks>
        /// 検体分析中情報をDBから読込みます。
        /// </remarks>
        public override void LoadDB()
        {
            this.fillBaseTable();
        }

        /// <summary>
        /// 検体分析中情報テーブル書込み
        /// </summary>
        /// <remarks>
        /// 検体分析中情報をDBに書き込みます。
        /// </remarks>
        public void CommitData()
        {
            this.updateBaseTable();
        }

        #endregion
    }
}
