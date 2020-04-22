using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.DB;
using Oelco.Common.Utility;
using Oelco.Common.Log;
using Oelco.CarisX.Utility;
using System.Data;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.GUI.Controls;
using Oelco.CarisX.Const;
using Oelco.CarisX.Calculator;
using Oelco.CarisX.Common;
using Oelco.CarisX.Log;

namespace Oelco.CarisX.DB
{
    public class CalibratorResultData : DataRowWrapperBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data"></param>
        public CalibratorResultData( DataRowWrapperBase data )
            : base( data )
        {
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 分析モジュール番号の取得、設定
        /// </summary>
        public Int32 ModuleNo
        {
            get
            {
                return this.Field<Int32>(CalibratorResultDB.STRING_MODULENO);
            }
            set
            {
                this.SetField<Int32>(CalibratorResultDB.STRING_MODULENO, value);
            }
        }
        /// <summary>
        /// ユニーク番号の取得、設定
        /// </summary>
        protected Int32 UniqueNo
        {
            get
            {
                return this.Field<Int32>( CalibratorResultDB.STRING_UNIQUENO );
            }
            set
            {
                this.SetField<Int32>( CalibratorResultDB.STRING_UNIQUENO, value );
            }
        }
        /// <summary>
        /// 多重測定回数番号の取得、設定
        /// </summary>
        public Int32 ReplicationNo
        {
            get
            {
                return this.Field<Int32>( CalibratorResultDB.STRING_REPLICATIONNO );
            }
            protected set
            {
                this.SetField<Int32>( CalibratorResultDB.STRING_REPLICATIONNO, value );
            }
        }
        /// <summary>
        /// 検体識別番号の取得、設定
        /// </summary>
        protected Int32 IndividuallyNo
        {
            get
            {
                return this.Field<Int32>( CalibratorResultDB.STRING_INDIVIDUALLYNO );
            }
            set
            {
                this.SetField<Int32>( CalibratorResultDB.STRING_INDIVIDUALLYNO, value );
            }
        }
        /// <summary>
        /// シーケンス番号の取得、設定
        /// </summary>
        public Int32 SequenceNo
        {
            get
            {
                return this.Field<Int32>( CalibratorResultDB.STRING_SEQUENCENO );
            }
            protected set
            {
                this.SetField<Int32>( CalibratorResultDB.STRING_SEQUENCENO, value );
            }
        }
        /// <summary>
        /// ラックIDの取得、設定
        /// </summary>
        public CarisXIDString RackId
        {
            get
            {
                return this.Field<String>( CalibratorResultDB.STRING_RACKID );
            }
            protected set
            {
                this.SetField<String>( CalibratorResultDB.STRING_RACKID, value.DispPreCharString );
            }
        }
        /// <summary>
        /// ラックポジションの取得、設定
        /// </summary>
        public Int32 RackPosition
        {
            get
            {
                return this.Field<Int32>( CalibratorResultDB.STRING_RACKPOSITION );
            }
            protected set
            {
                this.SetField<Int32>( CalibratorResultDB.STRING_RACKPOSITION, value );
            }
        }
        /// <summary>
        /// キャリブレータロット番号の取得、設定
        /// </summary>
        public String CalibLotNo
        {
            get
            {
                return this.Field<String>( CalibratorResultDB.STRING_CALIBLOTNO );
            }
            set
            {
                this.SetField<String>( CalibratorResultDB.STRING_CALIBLOTNO, value );
            }
        }
        /// <summary>
        /// 分析項目インデックスの取得、設定
        /// </summary>
        protected Int32 MeasureProtocolIndex
        {
            get
            {
                return this.Field<Int32>( CalibratorResultDB.STRING_MEASUREPROTOCOLINDEX );
            }
            set
            {
                this.SetField<Int32>( CalibratorResultDB.STRING_MEASUREPROTOCOLINDEX, value );
            }
        }
        /// <summary>
        /// カウント値の取得、設定
        /// </summary>
        protected Int32? CountValue
        {
            get
            {
                return this.Field<Int32?>( CalibratorResultDB.STRING_COUNT );
            }
            set
            {
                this.SetField<Int32?>( CalibratorResultDB.STRING_COUNT, value );
            }
        }
        /// <summary>
        /// 濃度値の取得、設定
        /// </summary>
        public virtual String Concentration
        {
            get
            {
                var conc = this.Field<String>( CalibratorResultDB.STRING_CONCENTRATION );
                if ( conc != null  )
                {
                    String unit = String.Empty;
                    String sign = String.Empty;
                    var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( this.MeasureProtocolIndex );
                    if ( conc != String.Empty && protocol != null )
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
                        else if (this.ReplicationRemarkId != null)
                        {
                            if (this.ReplicationRemarkId.HasRemark(Utility.Remark.RemarkBit.DynamicrangeUpperError))
                            {
                                sign = CarisXConst.CONCENTRATION_GREATER;
                            }
                            else if (this.ReplicationRemarkId.HasRemark(Utility.Remark.RemarkBit.DynamicrangeLowerError))
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

                return String.Empty;
            }

            protected set
            {
                this.SetField<String>( CalibratorResultDB.STRING_CONCENTRATION, value );
            }
        }
        /// <summary>
        /// 多重測定回リマークIDの取得、設定
        /// </summary>
        protected Remark ReplicationRemarkId
        {
            get
            {
                return this.Field<Int64?>( CalibratorResultDB.STRING_REPLICATIONREMARKID );
            }
            set
            {
                this.SetField<Int64?>( CalibratorResultDB.STRING_REPLICATIONREMARKID, value );
            }
        }
        /// <summary>
        /// カウント平均値の取得、設定
        /// </summary>
        protected Int32? CountAveValue
        {
            get
            {
                return this.Field<Int32?>( CalibratorResultDB.STRING_COUNTAVE );
            }
            set
            {
                this.SetField<Int32?>( CalibratorResultDB.STRING_COUNTAVE, value );
            }
        }
        /// <summary>
        /// 濃度平均値の取得、設定
        /// </summary>
        public virtual String ConcentrationAve
        {
            get
            {
                var conc = this.Field<String>( CalibratorResultDB.STRING_CONCENTRATIONAVE );
                if ( conc != null )
                {
                    String unit = String.Empty;
                    String sign = String.Empty;
                    var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( this.MeasureProtocolIndex );
                    if ( conc != String.Empty && protocol != null )
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
                        else if (this.ReplicationRemarkId != null)
                        {
                            if (this.ReplicationRemarkId.HasRemark(Utility.Remark.RemarkBit.DynamicrangeUpperError))
                            {
                                sign = CarisXConst.CONCENTRATION_GREATER;
                            }
                            else if (this.ReplicationRemarkId.HasRemark(Utility.Remark.RemarkBit.DynamicrangeLowerError))
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

                return String.Empty;
            }

            protected set
            {
                this.SetField<String>( CalibratorResultDB.STRING_CONCENTRATIONAVE, value );
            }
        }
        /// <summary>
        /// リマークIDの取得、設定
        /// </summary>
        protected Remark RemarkId
        {
            get
            {
                return this.Field<Int64?>( CalibratorResultDB.STRING_REMARKID );
            }
            set
            {
                this.SetField<Int64?>( CalibratorResultDB.STRING_REMARKID, value );
            }
        }
        /// <summary>
        /// 測定日時の取得、設定
        /// </summary>
        public DateTime MeasureDateTime
        {
            get
            {
                return this.Field<DateTime>( CalibratorResultDB.STRING_MEASUREDATETIME );
            }
            protected set
            {
                this.SetField<DateTime>( CalibratorResultDB.STRING_MEASUREDATETIME, value );
            }
        }
        /// <summary>
        /// 試薬ロット番号の取得、設定
        /// </summary>
        public String ReagentLotNo
        {
            get
            {
                return this.Field<String>( CalibratorResultDB.STRING_REAGENTLOTNO );
            }
            protected set
            {
                this.SetField<String>( CalibratorResultDB.STRING_REAGENTLOTNO, value );
            }
        }
        /// <summary>
        /// プレトリガロット番号の取得、設定
        /// </summary>
        public String PretriggerLotNo
        {
            get
            {
                return this.Field<String>( CalibratorResultDB.STRING_PRETRIGGERLOTNO );
            }
            protected set
            {
                this.SetField<String>( CalibratorResultDB.STRING_PRETRIGGERLOTNO, value );
            }
        }
        /// <summary>
        /// トリガロット番号の取得、設定
        /// </summary>
        public String TriggerLotNo
        {
            get
            {
                return this.Field<String>( CalibratorResultDB.STRING_TRIGGERLOTNO );
            }
            protected set
            {
                this.SetField<String>( CalibratorResultDB.STRING_TRIGGERLOTNO, value );
            }
        }
        /// <summary>
        /// エラーコードの取得、設定
        /// </summary>
        public String ErrorCode
        {
            get
            {
                return this.Field<String>( CalibratorResultDB.STRING_ERRORCODE );
            }
           protected set
            {
                this.SetField<String>( CalibratorResultDB.STRING_ERRORCODE, value );
            }
        }

        /// <summary>
        /// 分析項目名の取得、設定
        /// </summary>
        public String Analytes
        {
            get
            {
                return Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( this.Field<Int32>( CalibratorResultDB.STRING_MEASUREPROTOCOLINDEX ) ).ProtocolName;
            }
            set
            {
                this.SetField<Int32>( CalibratorResultDB.STRING_MEASUREPROTOCOLINDEX, Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromName( value ).ProtocolIndex );
            }
        }
        /// <summary>
        /// リマークの取得
        /// </summary>
        public String Remark
        {
            get
            {
                if ( this.RemarkId != null )
                {
                    return String.Join( ",", this.RemarkId.GetRemarkStrings() );
                }
                else if ( this.ReplicationRemarkId != null )
                {
                    return String.Join( ",", this.ReplicationRemarkId.GetRemarkStrings() );
                }
                return null;
            }
        }

        /// <summary>
        /// 検量線作成済フラグの取得、設定
        /// </summary>
        protected Boolean IsCreatedCalibCurve
        {
            get
            {
                return this.Field<Boolean>( CalibratorResultDB.STRING_ISCREATED_CALIBCUREVE );
            }
            set
            {
                this.SetField<Boolean>( CalibratorResultDB.STRING_ISCREATED_CALIBCUREVE, value );
            }
        }

        /// <summary>
        /// カウント値(文字列)の取得
        /// </summary>
        public String Count
        {
            get
            {
                if ( this.CountValue.HasValue && ( this.ReplicationRemarkId != null || this.ReplicationRemarkId.CanCalcCount ) )
                {
                    return this.CountValue.Value.ToString();
                }
                else
                {
                    return CarisXConst.COUNT_CONCENTRATION_NOTHING;
                }
            }
        }

        /// <summary>
        /// カウント平均値(文字列)の取得
        /// </summary>
        public String CountAve
        {
            get
            {
                if ( this.CountAveValue.HasValue )
                {
                    return this.CountAveValue.Value.ToString();
                }
                else if ( this.RemarkId == null )//|| this.RemarkId.CanCalcCount )
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
                return this.Field<Int32?>(CalibratorResultDB.STRING_DARKCOUNT);
            }
            set
            {
                this.SetField<Int32?>(CalibratorResultDB.STRING_DARKCOUNT, value);
            }
        }
        /// <summary>
        /// バックグラウンドカウント値の取得、設定
        /// </summary>
        public Int32? BGCount
        {
            get
            {
                return this.Field<Int32?>(CalibratorResultDB.STRING_BGCOUNT);
            }
            set
            {
                this.SetField<Int32?>(CalibratorResultDB.STRING_BGCOUNT, value);
            }
        }
        /// <summary>
        /// 測定カウント値の取得、設定
        /// </summary>
        public Int32? ResultCount
        {
            get
            {
                return this.Field<Int32?>(CalibratorResultDB.STRING_RESULTCOUNT);
            }
            set
            {
                this.SetField<Int32?>(CalibratorResultDB.STRING_RESULTCOUNT, value);
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 分析モジュール番号の取得
        /// </summary>
        /// <remarks>
        /// 分析モジュール番号を取得します。
        /// </remarks>
        /// <returns>分析モジュール番号</returns>
        public Int32 GetModuleNo()
        {
            return this.ModuleNo;
        }

        /// <summary>
        /// 分析モジュール番号の設定
        /// </summary>
        /// <remarks>
        /// 分析モジュール番号を設定します。
        /// </remarks>
        /// <param name="uniqueNo"></param>
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
        /// <returns>ユニーク番号</returns>
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
        /// <param name="uniqueNo"></param>
        public void SetUniqueNo( Int32 uniqueNo )
        {
            this.UniqueNo = uniqueNo;
        }

        /// <summary>
        /// レプリケーション番号の設定
        /// </summary>
        /// <remarks>
        /// レプリケーション番号を設定します。
        /// </remarks>
        /// <param name="replicationNo"></param>
        public void SetReplicationNo( Int32 replicationNo )
        {
            this.ReplicationNo = replicationNo;
        }

        /// <summary>
        /// 検体識別番号の取得
        /// </summary>
        /// <remarks>
        /// 検体識別番号を取得します。
        /// </remarks>
        /// <returns>検体識別番号</returns>
        public Int32 GetIndividuallyNo()
        {
            return this.IndividuallyNo;
        }

        /// <summary>
        /// 検体識別番号をの設定
        /// </summary>
        /// <remarks>
        /// 検体識別番号を設定します。
        /// </remarks>
        /// <param name="individuallyNo"></param>
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
        /// <param name="sequenceNo"></param>
        public void SetSequenceNo( Int32 sequenceNo )
        {
            this.SequenceNo = sequenceNo;
        }

        /// <summary>
        /// ラックIDの設定
        /// </summary>
        /// <remarks>
        /// ラックIDを設定します。
        /// </remarks>
        /// <param name="rackId"></param>
        public void SetRackId( CarisXIDString rackId )
        {
            this.RackId = rackId;
        }

        /// <summary>
        /// ラックポジションの設定
        /// </summary>
        /// <remarks>
        /// ラックポジションを設定します。
        /// </remarks>
        /// <param name="rackPosition"></param>
        public void SetRackPosition( Int32 rackPosition )
        {
            this.RackPosition = rackPosition;
        }

        /// <summary>
        /// プロトコルインデックスの取得
        /// </summary>
        /// <remarks>
        /// プロトコルインデックスを取得します。
        /// </remarks>
        /// <returns>プロトコルインデックス</returns>
        public Int32 GetMeasureProtocolIndex()
        {
            return this.MeasureProtocolIndex;
        }

        /// <summary>
        /// プロトコルインデックスの設定
        /// </summary>
        /// <remarks>
        /// プロトコルインデックスを設定します。
        /// </remarks>
        /// <param name="measureProtocolIndex"></param>
        public void SetMeasureProtocolIndex( Int32 measureProtocolIndex )
        {
            this.MeasureProtocolIndex = measureProtocolIndex;
        }

        /// <summary>
        /// カウント値の取得
        /// </summary>
        /// <remarks>
        /// カウント値を取得します。
        /// </remarks>
        /// <returns>カウント値</returns>
        public Int32? GetCount()
        {
            return this.CountValue;
        }

        /// <summary>
        /// カウント値の設定
        /// </summary>
        /// <remarks>
        /// カウント値を設定します。
        /// </remarks>
        /// <param name="count"></param>
        public void SetCount( Int32? count )
        {
            this.CountValue = count;
        }

        /// <summary>
        /// 濃度値の設定
        /// </summary>
        /// <remarks>
        /// 濃度値を設定します。
        /// </remarks>
        /// <param name="concentration"></param>
        public void SetConcentration( String concentration )
        {
            this.Concentration = concentration;
        }

        /// <summary>
        /// 多重測定回リマークIDの取得
        /// </summary>
        /// <remarks>
        /// 多重測定回リマークIDを取得します。
        /// </remarks>
        /// <returns>多重測定回リマークID</returns>
        public Remark GetReplicationRemarkId()
        {
            return this.ReplicationRemarkId;
        }

        /// <summary>
        /// 多重測定回リマークIDの設定
        /// </summary>
        /// <remarks>
        /// 多重測定回リマークIDを設定します。
        /// </remarks>
        /// <param name="repricationRemarkId"></param>
        public void SetReplicationRemarkId( Remark repricationRemarkId )
        {
            this.ReplicationRemarkId = repricationRemarkId;
        }

        /// <summary>
        /// カウント平均値の取得
        /// </summary>
        /// <remarks>
        /// カウント平均値を取得します。
        /// </remarks>
        /// <returns>カウント平均値</returns>
        public Int32? GetCountAve()
        {
            return this.CountAveValue;
        }

        /// <summary>
        /// カウント平均値の設定
        /// </summary>
        /// <remarks>
        /// カウント平均値を設定します。
        /// </remarks>
        /// <param name="countAve"></param>
        public void SetCountAve( Int32? countAve )
        {
            this.CountAveValue = countAve;
        }

        /// <summary>
        /// 濃度平均値の設定
        /// </summary>
        /// <remarks>
        /// 濃度平均値を設定します。
        /// </remarks>
        /// <param name="concentrationAve"></param>
        public void SetConcentrationAve( String concentrationAve )
        {
            this.ConcentrationAve = concentrationAve;
        }

        /// <summary>
        /// リマークIDの取得
        /// </summary>
        /// <remarks>
        /// リマークIDを取得します。
        /// </remarks>
        /// <returns>リマークID</returns>
        public Remark GetRemarkId()
        {
            return this.RemarkId;
        }

        /// <summary>
        /// リマークIDの設定
        /// </summary>
        /// <remarks>
        /// リマークIDを設定します。
        /// </remarks>
        /// <param name="remarkId"></param>
        public void SetRemarkId( Remark remarkId )
        {
            this.RemarkId = remarkId;
        }

        /// <summary>
        /// 測定日時の設定
        /// </summary>
        /// <remarks>
        /// 測定日時を設定します。
        /// </remarks>
        /// <param name="measureDateTime"></param>
        public void SetMeasureDateTime( DateTime measureDateTime )
        {
            this.MeasureDateTime = measureDateTime;
        }

        /// <summary>
        /// 試薬ロット番号の設定
        /// </summary>
        /// <remarks>
        /// 試薬ロット番号を設定します。
        /// </remarks>
        /// <param name="reagentLotNo"></param>
        public void SetReagentLotNo( String reagentLotNo )
        {
            this.ReagentLotNo = reagentLotNo;
        }

        /// <summary>
        /// プレトリガロット番号の設定
        /// </summary>
        /// <remarks>
        /// プレトリガロット番号を設定します。
        /// </remarks>
        /// <param name="pretriggerLotNo"></param>
        public void SetPretriggerLotNo( String pretriggerLotNo )
        {
            this.PretriggerLotNo = pretriggerLotNo;
        }

        /// <summary>
        /// トリガロット番号の設定
        /// </summary>
        /// <remarks>
        /// トリガロット番号を設定します。
        /// </remarks>
        /// <param name="triggerLotNo"></param>
        public void SetTriggerLotNo( String triggerLotNo )
        {
            this.TriggerLotNo = triggerLotNo;
        }

        /// <summary>
        /// エラーコードの設定
        /// </summary>
        /// <remarks>
        /// エラーコードを設定します。
        /// </remarks>
        /// <param name="errorCode"></param>
        public void SetErrorCode( String errorCode )
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// 検量線作成済みフラグの取得
        /// </summary>
        /// <remarks>
        /// 検量線作成済みフラグを取得します。
        /// </remarks>
        /// <returns>True：作成済み、False:未作成</returns>
        public Boolean GetIsCreatedCalibCurve()
        {
            return this.IsCreatedCalibCurve;
        }

        /// <summary>
        /// 検量線作成済みフラグの設定
        /// </summary>
        /// <remarks>
        /// 検量線作成済みフラグを設定します。
        /// </remarks>
        /// <param name="isCreatedCalibCurve"></param>
        public void SetIsCreatedCalibCurve( Boolean isCreatedCalibCurve )
        {
            this.IsCreatedCalibCurve = isCreatedCalibCurve;
        }

        /// <summary>
        /// 濃度値取得
        /// </summary>
        /// <remarks>
        /// 単位無し濃度値を取得します。
        /// </remarks>
        /// <returns>濃度値</returns>
        public String GetConcentration()
        {
            var conc = this.Field<String>( CalibratorResultDB.STRING_CONCENTRATION );
            // キャリブレータは濃度が常に既知である為リマーク等を考慮せず表示する
            if ( conc != null )
            {
                return conc;
            }
            return String.Empty;
        }
        /// <summary>
        /// 濃度値平均取得
        /// </summary>
        /// <remarks>
        /// 単位無し濃度値平均を取得します。
        /// </remarks>
        /// <returns></returns>
        public String GetConcentrationAve()
        {
            var conc = this.Field<String>( CalibratorResultDB.STRING_CONCENTRATIONAVE );
            if ( conc != null )
            {
                return conc;
            }
            return String.Empty;
        }

        public Int32? GetDarkCount()
        {
            return this.DarkCount;
        }
        public void SetDarkCount(Int32? count)
        {
            this.DarkCount = count;
        }

        public Int32? GetBGCount()
        {
            return this.BGCount;
        }
        public void SetBGCount(Int32? count)
        {
            this.BGCount = count;
        }

        public Int32? GetResultCount()
        {
            return this.ResultCount;
        }
        public void SetResultCount(Int32? count)
        {
            this.ResultCount = count;
        }

        #endregion

        #region [内部クラス]

        /// <summary>
        /// 列キー
        /// </summary>
        /// <remarks>
        /// グリッド向け列キー設定用クラスです。
        /// </remarks>
        public struct DataKeys
        {
            public const String SequenceNo = "SequenceNo";
            public const String CalibLotNo = "CalibLotNo";
            public const String RackId = "RackId";
            public const String RackPosition = "RackPosition";
            public const String Analytes = "Analytes";
            public const String Count = "Count";
            public const String Concentration = "Concentration";
            public const String ReplicationNo = "ReplicationNo";
            public const String CountAve = "CountAve";
            public const String ConcentrationAve = "ConcentrationAve";
            public const String MeasureDateTime = "MeasureDateTime";
            public const String ReagentLotNo = "ReagentLotNo";
            public const String PretriggerLotNo = "PretriggerLotNo";
            public const String TriggerLotNo = "TriggerLotNo";
            public const String Remark = "Remark";
            public const String ErrorCode = "ErrorCode";
            public const String DarkCount = "DarkCount";
            public const String BGCount = "BGCount";
            public const String ResultCount = "ResultCount";
            public const String ModuleNo = "ModuleNo";
        };

        #endregion

    }

    /// <summary>
    /// キャリブレータ測定データ出力クラス
    /// </summary>
    public class OutPutCalibratorResultData : CalibratorResultData
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data"></param>
        public OutPutCalibratorResultData( DataRowWrapperBase data )
            : base( data )
        {
        }

        /// <summary>
        /// 濃度値の取得、設定
        /// </summary>
        public override String Concentration
        {
            get
            {
                var conc = this.Field<String>( CalibratorResultDB.STRING_CONCENTRATION );

                if ( conc != null  )
                {
                    String unit = String.Empty;
                    String sign = String.Empty;
                    var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( this.MeasureProtocolIndex );
                    if ( conc != String.Empty && protocol != null )
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
                      else if (this.ReplicationRemarkId != null)
                      {
                          if (this.ReplicationRemarkId.HasRemark(Utility.Remark.RemarkBit.DynamicrangeUpperError))
                          {
                              sign = CarisXConst.CONCENTRATION_GREATER;
                          }
                          else if (this.ReplicationRemarkId.HasRemark(Utility.Remark.RemarkBit.DynamicrangeLowerError))
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

                return String.Empty;
            }

            protected set
            {
                this.SetField<String>( CalibratorResultDB.STRING_CONCENTRATION, value );
            }
        }

        /// <summary>
        /// 濃度平均値の取得、設定
        /// </summary>
        public override String ConcentrationAve
        {
            get
            {
                var conc = this.Field<String>(CalibratorResultDB.STRING_CONCENTRATIONAVE);
                if (conc != null)
                {
                    String unit = String.Empty;
                    String sign = String.Empty;
                    var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(this.MeasureProtocolIndex);
                    if (conc != String.Empty && protocol != null)
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
                        else if (this.ReplicationRemarkId != null)
                        {
                            if (this.ReplicationRemarkId.HasRemark(Utility.Remark.RemarkBit.DynamicrangeUpperError))
                            {
                                sign = CarisXConst.CONCENTRATION_GREATER;
                            }
                            else if (this.ReplicationRemarkId.HasRemark(Utility.Remark.RemarkBit.DynamicrangeLowerError))
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

                return String.Empty;
            }

            protected set
            {
                this.SetField<String>( CalibratorResultDB.STRING_CONCENTRATIONAVE, value );
            }
        }

        /// <summary>
        /// 濃度単位の取得
        /// </summary>
        /// <remarks>
        /// 濃度算出可否を考慮した濃度単位の取得を行います。
        /// </remarks>
        /// <param name="concentration">濃度</param>
        /// <returns>濃度単位</returns>
        private String getConcUnit( String concentration )
        {
            String unit = String.Empty;

            // 濃度算出不能である場合String.Emptyを返す
            if ( !String.IsNullOrEmpty( concentration ) && ( concentration != CarisXConst.COUNT_CONCENTRATION_NOTHING )  )
            {
                var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( this.MeasureProtocolIndex );
                if ( protocol != null )
                {
                    unit = protocol.ConcUnit;
                }
            }

            return unit;
        }

        /// <summary>
        /// 濃度単位取得
        /// </summary>
        /// <remarks>
        /// 濃度単位の取得を行います。
        /// </remarks>
        /// <returns>濃度単位</returns>
        public String GetConcentrationUnit()
        {
            String unit = String.Empty;
            String conc = this.Concentration;
            unit = this.getConcUnit( conc );
            return unit;
        }

        /// <summary>
        /// 平均濃度単位取得
        /// </summary>
        /// <remarks>
        /// 平均濃度単位の取得を行います。
        /// </remarks>
        /// <returns>平均濃度単位</returns>
        public String GetConcentrationAveUnit()
        {
            String unit = String.Empty;
            String conc = this.ConcentrationAve;
            unit = this.getConcUnit( conc );
            return unit;
        }
    }

    /// <summary>
    /// キャリブレータ測定結果情報DBクラス
    /// </summary>
    /// <remarks>
    /// キャリブレータ測定結果情報のアクセスを行うクラスです。
    /// </remarks>
    public class CalibratorResultDB : DBAccessControl
    {
        #region [定数定義]
        /// <summary>
        /// 分析モジュール番号(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_MODULENO = "moduleNo";
        /// <summary>
        /// ユニーク番号(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_UNIQUENO = "uniqueNo";
        /// <summary>
        /// 多重測定回数番号(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_REPLICATIONNO = "replicationNo";
        /// <summary>
        /// 検体識別番号(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_INDIVIDUALLYNO = "individuallyNo";
        /// <summary>
        /// シーケンス番号(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_SEQUENCENO = "sequenceNo";
        /// <summary>
        /// ラックID(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_RACKID = "rackId";
        /// <summary>
        /// ラックポジション(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_RACKPOSITION = "rackPosition";
        /// <summary>
        /// キャリブレータロット番号(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_CALIBLOTNO = "calibLotNo";
        /// <summary>
        /// 分析項目インデックス(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_MEASUREPROTOCOLINDEX = "measureProtocolIndex";
        /// <summary>
        /// カウント値(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_COUNT = "count";
        /// <summary>
        /// 濃度値(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_CONCENTRATION = "concentration";
        /// <summary>
        /// 多重測定回リマークID(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_REPLICATIONREMARKID = "replicationRemarkId";
        /// <summary>
        /// カウント平均値(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_COUNTAVE = "countAve";
        /// <summary>
        /// 濃度平均値(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_CONCENTRATIONAVE = "concentrationAve";
        /// <summary>
        /// リマークID(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_REMARKID = "remarkId";
        /// <summary>
        /// 測定日時(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_MEASUREDATETIME = "measureDateTime";
        /// <summary>
        /// 試薬ロット番号(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_REAGENTLOTNO = "reagentLotNo";
        /// <summary>
        /// プレトリガロット番号(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_PRETRIGGERLOTNO = "pretriggerLotNo";
        /// <summary>
        /// トリガロット番号(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_TRIGGERLOTNO = "triggerLotNo";
        /// <summary>
        /// エラーコード(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_ERRORCODE = "errorCode";
        /// <summary>
        /// バッチ番号(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_BATCHNO = "batchNo";
        /// <summary>
        /// 検量線作成済みフラグ(DBテーブル：calibratorResult列名)
        /// </summary>
        public const String STRING_ISCREATED_CALIBCUREVE = "isCreatedCalibCurve";
        /// <summary>
        /// ダークカウント値(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_DARKCOUNT = "darkCount";
        /// <summary>
        /// バックグラウンドカウント値(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_BGCOUNT = "bGCount";
        /// <summary>
        /// 測定カウント値(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_RESULTCOUNT = "resultCount";

        #endregion

        #region [プロパティ]

        /// <summary>
        /// データテーブル取得SQL
        /// </summary>
        protected override String baseTableSelectSql
        {
            get
            {
                return "SELECT * FROM dbo.calibratorResult";
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 測定結果データ(計算済み)の設定
        /// </summary>
        /// <remarks>
        /// 1分析項目単位で測定結果データを設定します。
        /// 測定結果データは1分析項目単位での分析終了時にスレーブより送信されます。
        /// </remarks>
        /// <param name="calcData">計算済測定結果データ</param>
        /// <param name="data">測定結果データ</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean AddResultData( CalcData calcData, IMeasureResultData data )
        {
            Boolean result = false;

            // 測定結果データ設定
            if ( this.DataTable != null )
            {
                DataRow addRow = this.DataTable.Clone().NewRow();
                CalibratorResultData resultData = new CalibratorResultData( addRow );

                // 測定結果設定
                Int32 digits = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(calcData.ProtocolIndex).LengthAfterDemPoint;
   
                SampleInfo info = Singleton<InProcessSampleInfoManager>.Instance.SearchInProcessSampleFromUniqueNo( calcData.UniqueNo );
                resultData.SetModuleNo(data.ModuleID);
                resultData.SetUniqueNo(calcData.UniqueNo);
                resultData.SetReplicationNo(calcData.ReplicationNo);
                resultData.SetIndividuallyNo(calcData.IndividuallyNo);
                resultData.SetSequenceNo(info.SequenceNumber);
                resultData.SetRackId(calcData.RackID);
                resultData.SetRackPosition(calcData.RackPosition.Value);
                resultData.CalibLotNo = info.SampleId;
                resultData.SetMeasureProtocolIndex(calcData.ProtocolIndex);
                resultData.SetIsCreatedCalibCurve(false);
                resultData.SetDarkCount(data.DarkCount);
                resultData.SetBGCount(data.BGCount);
                resultData.SetResultCount(data.ResultCount);

                if ( calcData.CalcInfoReplication != null )
                {
                    resultData.SetCount( calcData.CalcInfoReplication.CountValue );

                    if ( calcData.CalcInfoReplication.Concentration.HasValue )
                    {
                        resultData.SetConcentration( SubFunction.ToRoundOffParse( calcData.CalcInfoReplication.Concentration.Value, digits ) );
                    }
                    else
                    {
                        resultData.SetConcentration( null );
                    }

                    resultData.SetReplicationRemarkId( calcData.CalcInfoReplication.Remark );
                }

                if ( calcData.CalcInfoAverage != null )
                {
                    if ( calcData.CalcInfoAverage.CountValue.HasValue )
                    {
                        resultData.SetCountAve( calcData.CalcInfoAverage.CountValue );
                    }
                    else
                    {
                        resultData.SetCountAve( null );
                    }

                    if ( calcData.CalcInfoAverage.Concentration.HasValue )
                    {
                        resultData.SetConcentrationAve( SubFunction.ToRoundOffParse( calcData.CalcInfoReplication.Concentration.Value, digits ) );
                    }
                    else
                    {
                        resultData.SetConcentrationAve( null );
                    }
                    resultData.SetRemarkId( calcData.CalcInfoAverage.Remark );
                }

                resultData.SetMeasureDateTime( calcData.MeasureDateTime );
                resultData.SetReagentLotNo( calcData.ReagentLotNo );
                resultData.SetPretriggerLotNo( data.PreTriggerLotNo );
                resultData.SetTriggerLotNo( data.TriggerLotNo );
                resultData.SetErrorCode( string.Join( ",", data.ErrorLog.ErrorCodeArg.Select( ( codeArg ) => codeArg.Item1 + "-" + codeArg.Item2 ).Where( ( error ) => error.Length > 1 ) ) );

                this.SetData( new List<CalibratorResultData>() { resultData } );

                // 分析履歴の登録
                String[] contents = new String[9];
                contents[0] = data.AnalysisLog.DiffSensor1.ToString();
                contents[1] = data.AnalysisLog.DiffSensor2.ToString();
                contents[2] = data.AnalysisLog.DiffSensor3.ToString();
                contents[3] = data.DarkCount.ToString();
                contents[4] = resultData.Count ?? String.Empty;
                contents[5] = data.BGCount.ToString();
                contents[6] = data.ResultCount.ToString();
                Singleton<CarisXLogManager>.Instance.Write(LogKind.AnalyseHist
                                                         , Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                                                         , data.ModuleID
                                                         , CarisXLogInfoBaseExtention.Empty
                                                         , contents );

                result = true;
            }
            return result;
        }

        /// <summary>
        /// 平均更新
        /// </summary>
        /// <remarks>
        /// 平均更新します
        /// </remarks>
        /// <param name="calcData">平均値保持データ</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean UpdateAverageData( CalcData calcData )
        {
            Boolean result = false;
            if ( calcData != null )
            {
                if ( this.DataTable != null )
                {
                    try
                    {
                        var updateData = ( from v in this.DataTable.Copy().Rows.OfType<DataRow>()
                                           let data = new CalibratorResultData( v )
                                           where data.GetUniqueNo() == calcData.UniqueNo && data.ReplicationNo == calcData.ReplicationNo
                                           select data ).SingleOrDefault();

                        if ( updateData != null )
                        {
                            if ( calcData.CalcInfoAverage != null )
                            {
                                // 濃度桁数
                                Int32 digits = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(calcData.ProtocolIndex).LengthAfterDemPoint;
                                if ( calcData.CalcInfoAverage.CountValue.HasValue )
                                {
                                    updateData.SetCountAve( calcData.CalcInfoAverage.CountValue );
                                }
                                else
                                {
                                    updateData.SetCountAve( null );
                                }

                                if ( calcData.CalcInfoAverage.Concentration.HasValue )
                                {
                                    updateData.SetConcentrationAve( SubFunction.ToRoundOffParse( calcData.CalcInfoAverage.Concentration.Value, digits ) );
                                }
                                else
                                {
                                    updateData.SetConcentrationAve( null );
                                }
                                updateData.SetRemarkId( calcData.CalcInfoAverage.Remark );
                            }
                        }

                        this.SetData( new List<CalibratorResultData>() { updateData } );
                        result = true;
                    }
                    catch ( Exception ex )
                    {

                        // DB内部に不正データ
                        Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                               CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// リマーク更新
        /// </summary>
        /// <remarks>
        /// リマーク更新します
        /// </remarks>
        /// <param name="calcData"></param>
        /// <returns></returns>
        public Boolean UpdateRemark( CalcData calcData )
        {
            Boolean result = false;
            // 測定結果データ設定
            if ( this.DataTable != null )
            {

                try
                {
                    var updataData = ( from v in this.DataTable.Copy().Rows.OfType<DataRow>()
                                       let data = new CalibratorResultData( v )
                                       where calcData.IndividuallyNo == data.GetIndividuallyNo() && calcData.UniqueNo == data.GetUniqueNo()
                                       orderby ( (DateTime)v[STRING_MEASUREDATETIME] ) descending
                                       select data ).FirstOrDefault();

                    if ( updataData != null )
                    {
                        if ( calcData.CalcInfoReplication != null )
                        {
                            updataData.SetReplicationRemarkId( calcData.CalcInfoReplication.Remark );
                        }

                        if ( calcData.CalcInfoAverage != null )
                        {
                            updataData.SetRemarkId( calcData.CalcInfoAverage.Remark );
                        }

                        result = true;
                    }
                }
                catch ( Exception ex )
                {
                    // DB内部に不正データ
                    Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
                }
            }

            return result;
        }

        /// <summary>
        /// キャリブレータ測定結果情報テーブルの取得
        /// </summary>
        /// <remarks>
        /// キャリブレータ測定結果情報テーブルを取得します。
        /// </remarks>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        public List<CalibratorResultData> GetData( ISearchInfoCalibratorResult searchInfo = null )
        {
            List<CalibratorResultData> result = new List<CalibratorResultData>();

            if ( this.DataTable != null )
            {
                try
                {
                    // 修改内容：コピー手順を削除して、大量のデータによるメモリオーバーフローを防ぎます。
                    //var datas = from v in this.DataTable.Copy().Rows.OfType<DataRow>()
                    //            let data = new CalibratorResultData( v )
                    //            where this.getCalibResultDataWhere( searchInfo, data )
                    //            orderby ( (DateTime)v[STRING_MEASUREDATETIME] ) descending
                    //            select data;
                    var datas = from v in this.DataTable.Rows.OfType<DataRow>()
                                let data = new CalibratorResultData(v)
                                where this.getCalibResultDataWhere(searchInfo, data)
                                orderby ((DateTime)v[STRING_MEASUREDATETIME]) descending
                                select data;

                    result.AddRange( datas );
                }
                catch ( Exception ex )
                {
                    // DB内部に不正データ
                    Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
                }
            }

            return result;
        }

        /// <summary>
        /// 絞込み条件チェック(Where)
        /// </summary>
        /// <remarks>
        /// 絞り込み条件に適合するかどうかをチェックします。
        /// </remarks>
        /// <param name="searchInfo">絞り込み条件</param>
        /// <param name="data">絞り込み条件比較対象</param>
        /// <returns>true:絞込み条件に適合</returns>
        private Boolean getCalibResultDataWhere( ISearchInfoCalibratorResult searchInfo, CalibratorResultData data )
        {
            // 検索条件無しの場合
            if ( searchInfo == null )
            {
                // 現在日時の日付のデータに限定
                if (data.MeasureDateTime.Date != DateTime.Today.Date)
                {
                    return false;
                }
                return true;
            }
            Boolean bResult = false;
            // 分析項目
            if ( searchInfo.AnalyteSelect.Count > 0 )
            {
                if ( !searchInfo.AnalyteSelect.Contains( data.Analytes ) )
                {
                    return false;
                }
                else
                {
                    bResult = true;
                }
            }

            // 濃度値
            if ( searchInfo.ConcentrationSelect.Item1 )
            {
                bResult = false;
                Double concentration = 0;

                if ( !Double.TryParse( data.GetConcentration(), out concentration ) ||
                    ( searchInfo.ConcentrationSelect.Item2 > concentration || searchInfo.ConcentrationSelect.Item3 < concentration ) )
                {
                    return false;
                }
                else
                {
                    bResult = true;
                }
            }

            // ラックID
            if ( searchInfo.RackIdSelect.Item1 )
            {
                bResult = false;
                if ( searchInfo.RackIdSelect.Item2.Value > data.RackId.Value ||
                    searchInfo.RackIdSelect.Item3.Value < data.RackId.Value )
                {
                    return false;
                }
                else
                {
                    bResult = true;
                }
            }

            // シーケンス番号
            if ( searchInfo.SequenceNoSelect.Item1 )
            {
                bResult = false;
                if ( searchInfo.SequenceNoSelect.Item2 > data.SequenceNo ||
                    searchInfo.SequenceNoSelect.Item3 < data.SequenceNo )
                {
                    return false;
                }
                else
                {
                    bResult = true;
                }
            }

            // 測定日
            if ( searchInfo.MeasuringTimeSelect.Item1 )
            {
                bResult = false;
                if ( searchInfo.MeasuringTimeSelect.Item2.Date > data.MeasureDateTime.Date ||
                    searchInfo.MeasuringTimeSelect.Item3.Date < data.MeasureDateTime.Date )
                {
                    return false;
                }
                else
                {
                    bResult = true;
                }
            }

            // リマーク
            if ( searchInfo.RemarkSelect > 0 )
            {
                bResult = false;
                if ( ( data.GetRemarkId() != null && !data.GetRemarkId().IsBelongCategory( searchInfo.RemarkSelect ) ) || !( data.GetReplicationRemarkId() ?? (Remark)0 ).IsBelongCategory( searchInfo.RemarkSelect ) )
                {
                    return false;
                }
                else
                {
                    bResult = true;
                }
            }

            // モジュールNo
            if (searchInfo.ModuleSelect > 0)
            {
                bResult = false;
                List<Int32> targetModule = new List<Int32>();
                if (searchInfo.ModuleSelect.HasFlag(ModuleCategory.Module1))
                {
                    targetModule.Add((Int32)RackModuleIndex.Module1);
                }
                if (searchInfo.ModuleSelect.HasFlag(ModuleCategory.Module2))
                {
                    targetModule.Add((Int32)RackModuleIndex.Module2);
                }
                if (searchInfo.ModuleSelect.HasFlag(ModuleCategory.Module3))
                {
                    targetModule.Add((Int32)RackModuleIndex.Module3);
                }
                if (searchInfo.ModuleSelect.HasFlag(ModuleCategory.Module4))
                {
                    targetModule.Add((Int32)RackModuleIndex.Module4);
                }

                if (!targetModule.Contains(data.GetModuleNo()))
                {
                    return false;
                }
                else
                {
                    bResult = true;
                }
            }

            // キャリブレータロット
            if ( searchInfo.CalibratorLotSelect.Item1 )
            {
                bResult = false;
                if ( searchInfo.CalibratorLotSelect.Item2 != data.CalibLotNo )
                {
                    return false;
                }
                else
                {
                    bResult = true;
                }
            }

            return bResult;
        }

        /// <summary>
        /// キャリブレータ測定結果データの設定
        /// </summary>
        /// <remarks>
        /// キャリブレータ測定結果データの同期を行います。
        /// </remarks>
        /// <param name="list">変更、削除操作済みデータ</param>
        public void SetData( List<CalibratorResultData> list )
        {
            list.SyncDataListToDataTable( this.DataTable );
        }

        /// <summary>
        /// キャリブレータ測定結果情報テーブル取得
        /// </summary>
        /// <remarks>
        /// キャリブレータ測定結果情報をDBから読込みます。
        /// </remarks>
        public override void LoadDB()
        {
            this.fillBaseTable();
        }

        /// <summary>
        /// キャリブレータ測定結果情報テーブル書込み
        /// </summary>
        /// <remarks>
        /// キャリブレータ測定結果情報をDBに書き込みます。
        /// </remarks>
        public void CommitData()
        {
            this.updateBaseTable();
        }

        /// <summary>
        /// ユニーク番号の最大値を取得
        /// </summary>
        /// <returns></returns>
        public Int32 GetMaxUniqueNo()
        {
            Int32 result = 0;

            // ユニーク番号の最大値を取得
            var dataResult = this.OpenQuery( String.Format( "SELECT MAX({0}) FROM dbo.calibratorResult", SpecimenResultDB.STRING_UNIQUENO ) );

            if (dataResult != null && dataResult.Rows.Count > 0)
            {
                try
                {
                    result = Convert.ToInt32( dataResult.Rows[0].ItemArray[0] );
                }
                catch ( Exception ex )
                {
                    // データ取得失敗
                    Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
                }
            }

            return result;
        }

        #endregion

        #region [protectedメソッド]
        /// <summary>
        /// 保存上限
        /// </summary>
        /// <remarks>
        /// 保存数(分析)上限超過分の検体測定結果を削除します。
        /// </remarks>
        protected override void removeLimitOver()
        {
            if ( this.DataTable != null )
            {
                try
                {
                    var datas = from v in this.DataTable.Copy().Rows.OfType<DataRow>()
                                let data = new CalibratorResultData( v )
                                orderby data.MeasureDateTime descending
                                where !data.IsDeletedData()
                                select data;

                    List<CalibratorResultData> deleteData;
                    var samples = datas.GroupBy( ( data ) => data.GetIndividuallyNo() );

                    // 保存数(分析)上限超過分の検体測定結果削除

                    var deleteIndividuallyList = datas.Skip( CarisXConst.MAX_CALIBRESULT_TEST_COUNT ).Select( ( data ) => data.GetIndividuallyNo() ).Distinct();
                    deleteData = ( from individuallyNo in deleteIndividuallyList
                                   let grpData = samples.FirstOrDefault( ( grp ) => grp.Key == individuallyNo )
                                   where grpData != null
                                   select grpData ).SelectMany( ( grp ) => grp.AsEnumerable() ).ToList();
                    deleteData.DeleteAllDataList();
                    this.SetData( deleteData );

                }
                catch ( Exception ex )
                {
                    // DB内部に不正データ
                    Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
                }
            }
        }
        #endregion

    }
}
