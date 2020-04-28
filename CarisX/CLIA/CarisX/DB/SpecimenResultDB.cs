using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.DB;
using System.Data;
using Oelco.Common.Utility;
using Oelco.Common.Log;
using Oelco.CarisX.GUI.Controls;
using Oelco.CarisX.Const;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Calculator;
using Oelco.CarisX.Common;
using Oelco.CarisX.Log;

namespace Oelco.CarisX.DB
{
    /// <summary>
    /// 一般検体測定結果データクラス
    /// </summary>
    public class SpecimenResultData : DataRowWrapperBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data"></param>
        public SpecimenResultData(DataRowWrapperBase data)
            : base(data)
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
                return this.Field<Int32>(SpecimenResultDB.STRING_MODULENO);
            }
            set
            {
                this.SetField<Int32>(SpecimenResultDB.STRING_MODULENO, value);
            }
        }
        /// <summary>
        /// ユニーク番号の取得、設定
        /// </summary>
        protected Int32 UniqueNo
        {
            get
            {
                return this.Field<Int32>(SpecimenResultDB.STRING_UNIQUENO);
            }
            set
            {
                this.SetField<Int32>(SpecimenResultDB.STRING_UNIQUENO, value);
            }
        }
        /// <summary>
        /// 多重測定回数番号の取得、設定
        /// </summary>
        public Int32 ReplicationNo
        {
            get
            {
                return this.Field<Int32>(SpecimenResultDB.STRING_REPLICATIONNO);
            }
            protected set
            {
                this.SetField<Int32>(SpecimenResultDB.STRING_REPLICATIONNO, value);
            }
        }
        /// <summary>
        /// 検体識別番号の取得、設定
        /// </summary>
        protected Int32 IndividuallyNo
        {
            get
            {
                return this.Field<Int32>(SpecimenResultDB.STRING_INDIVIDUALLYNO);
            }
            set
            {
                this.SetField<Int32>(SpecimenResultDB.STRING_INDIVIDUALLYNO, value);
            }
        }
        /// <summary>
        /// 受付番号の取得、設定
        /// </summary>
        public Int32 ReceiptNo
        {
            get
            {
                // DB互換性維持
                return SubFunction.SafeParseInt32(this.Row[SpecimenResultDB.STRING_RECEIPTNO]);
            }
            protected set
            {
                this.SetField<Int32>(SpecimenResultDB.STRING_RECEIPTNO, value);
            }
        }

        /// <summary>
        /// シーケンス番号の取得、設定
        /// </summary>
        public Int32 SequenceNo
        {
            get
            {
                return this.Field<Int32>(SpecimenResultDB.STRING_SEQUENCENO);
            }
            protected set
            {
                this.SetField<Int32>(SpecimenResultDB.STRING_SEQUENCENO, value);
            }
        }

        /// <summary>
        /// 分析モードの取得、設定
        /// </summary>
        public String AnalysisMode
        {
            get
            {
                Int32? field = this.Field<Int32?>(SpecimenResultDB.STRING_ANALYSISMODE);
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
                this.SetField<Int32?>(SpecimenResultDB.STRING_ANALYSISMODE, (Int32)type);
            }
        }

        /// <summary>
        /// ラックポジションの取得、設定
        /// </summary>
        public Int32 RackPosition
        {
            get
            {
                return this.Field<Int32>(SpecimenResultDB.STRING_RACKPOSITION);
            }
            protected set
            {
                this.SetField<Int32>(SpecimenResultDB.STRING_RACKPOSITION, value);
            }
        }
        /// <summary>
        /// 検体IDの取得、設定
        /// </summary>
        public String PatientId
        {
            get
            {
                return this.Field<String>(SpecimenResultDB.STRING_PATIENTID);
            }
            protected set
            {
                this.SetField<String>(SpecimenResultDB.STRING_PATIENTID, value);
            }
        }
        /// <summary>
        /// 分析項目インデックスの取得、設定
        /// </summary>
        protected Int32 MeasureProtocolIndex
        {
            get
            {
                return this.Field<Int32>(SpecimenResultDB.STRING_MEASUREPROTOCOLINDEX);
            }
            set
            {
                this.SetField<Int32>(SpecimenResultDB.STRING_MEASUREPROTOCOLINDEX, value);
            }
        }

        /// <summary>
        /// カウント値の取得、設定
        /// </summary>
        protected Int32? CountValue
        {
            get
            {
                return this.Field<Int32?>(SpecimenResultDB.STRING_COUNT);
            }
            set
            {
                this.SetField<Int32?>(SpecimenResultDB.STRING_COUNT, value);
            }
        }
        /// <summary>
        /// 濃度値の取得、設定
        /// </summary>
        public virtual String Concentration
        {
            get
            {
                var conc = this.Field<String>(SpecimenResultDB.STRING_CONCENTRATION);
                if (!String.IsNullOrEmpty(conc) && (this.ReplicationRemarkId == null || this.ReplicationRemarkId.CanCalcConcentration))
                {
                    String unit = String.Empty;
                    String sign = String.Empty;

                    var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(this.MeasureProtocolIndex);
                    if (!String.IsNullOrEmpty(conc) && protocol != null)
                    {
                        // 表示する数値と濃度の間にスペースを含める
                        unit = CarisX.Properties.Resources.STRING_COMMON_013 + protocol.ConcUnit;

                        if (this.RemarkId != null)
                        {
                            if (this.RemarkId.HasRemark(Utility.Remark.RemarkBit.DynamicrangeUpperError))
                            {
                                if (double.Parse(conc) == protocol.ConcDynamicRange.Max)
                                {
                                    sign = CarisXConst.CONCENTRATION_GREATER;
                                }
                            }
                            else if (this.RemarkId.HasRemark(Utility.Remark.RemarkBit.DynamicrangeLowerError))
                            {
                                double dCon = double.Parse(conc);
                                if (dCon == protocol.ConcDynamicRange.Min && dCon != 0)
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

                return CarisXConst.COUNT_CONCENTRATION_NOTHING;
            }

            protected set
            {
                this.SetField<String>(SpecimenResultDB.STRING_CONCENTRATION, value);
            }
        }

        /// <summary>
        /// 多重測定回リマークIDの取得、設定
        /// </summary>
        protected Remark ReplicationRemarkId
        {
            get
            {
                return this.Field<Int64>(SpecimenResultDB.STRING_REPLICATIONREMARKID);
            }
            set
            {
                this.SetField<Int64>(SpecimenResultDB.STRING_REPLICATIONREMARKID, value);
            }
        }
        /// <summary>
        /// カウント平均値の取得、設定
        /// </summary>
        protected Int32? CountAveValue
        {
            get
            {
                return this.Field<Int32?>(SpecimenResultDB.STRING_COUNTAVE);
            }
            set
            {
                this.SetField<Int32?>(SpecimenResultDB.STRING_COUNTAVE, value);
            }
        }
        /// <summary>
        /// 濃度平均値の取得、設定
        /// </summary>
        public virtual String ConcentrationAve
        {
            get
            {
                var conc = this.Field<String>(SpecimenResultDB.STRING_CONCENTRATIONAVE);
                if (!String.IsNullOrEmpty(conc) || this.RemarkId == null || this.RemarkId.CanCalcConcentration)
                {
                    String unit = String.Empty;
                    String sign = String.Empty;
                    var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(this.MeasureProtocolIndex);
                    if (!String.IsNullOrEmpty(conc) && protocol != null)
                    {
                        // 表示する数値と濃度の間にスペースを含める
                        unit = CarisX.Properties.Resources.STRING_COMMON_013 + protocol.ConcUnit;
                        if (this.RemarkId != null)
                        {
                            if (this.RemarkId.HasRemark(Utility.Remark.RemarkBit.DynamicrangeUpperError))
                            {
                                if (double.Parse(conc) == protocol.ConcDynamicRange.Max)
                                {
                                    sign = CarisXConst.CONCENTRATION_GREATER;
                                }
                            }
                            else if (this.RemarkId.HasRemark(Utility.Remark.RemarkBit.DynamicrangeLowerError))
                            {
                                if (double.Parse(conc) == protocol.ConcDynamicRange.Min && protocol.ConcDynamicRange.Min != 0)
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

                return CarisXConst.COUNT_CONCENTRATION_NOTHING;
            }

            protected set
            {
                this.SetField<String>(SpecimenResultDB.STRING_CONCENTRATIONAVE, value);
            }
        }
        /// <summary>
        /// リマークIDの取得、設定
        /// </summary>
        protected Remark RemarkId
        {
            get
            {
                return this.Field<Int64?>(SpecimenResultDB.STRING_REMARKID);
            }
            set
            {
                this.SetField<Int64?>(SpecimenResultDB.STRING_REMARKID, value);
            }
        }
        /// <summary>
        /// 測定日時の取得、設定
        /// </summary>
        public DateTime MeasureDateTime
        {
            get
            {
                return this.Field<DateTime>(SpecimenResultDB.STRING_MEASUREDATETIME);
            }
            protected set
            {
                this.SetField<DateTime>(SpecimenResultDB.STRING_MEASUREDATETIME, value);
            }
        }
        /// <summary>
        /// 試薬ロット番号の取得、設定
        /// </summary>
        public String ReagentLotNo
        {
            get
            {
                return this.Field<String>(SpecimenResultDB.STRING_REAGENTLOTNO);
            }
            protected set
            {
                this.SetField<String>(SpecimenResultDB.STRING_REAGENTLOTNO, value);
            }
        }
        /// <summary>
        /// プレトリガロット番号の取得、設定
        /// </summary>
        public String PretriggerLotNo
        {
            get
            {
                return this.Field<String>(SpecimenResultDB.STRING_PRETRIGGERLOTNO);
            }
            protected set
            {
                this.SetField<String>(SpecimenResultDB.STRING_PRETRIGGERLOTNO, value);
            }
        }
        /// <summary>
        /// トリガーロット番号の取得、設定
        /// </summary>
        public String TriggerLotNo
        {
            get
            {
                return this.Field<String>(SpecimenResultDB.STRING_TRIGGERLOTNO);
            }
            protected set
            {
                this.SetField<String>(SpecimenResultDB.STRING_TRIGGERLOTNO, value);
            }
        }
        /// <summary>
        /// 使用検量線作成日時の取得、設定
        /// </summary>
        public DateTime? CalibCurveDateTime
        {
            get
            {
                return this.Field<DateTime?>(SpecimenResultDB.STRING_CALIBCURVEDATETIME);
            }
            protected set
            {
                this.SetField<DateTime?>(SpecimenResultDB.STRING_CALIBCURVEDATETIME, value);
            }
        }
        /// <summary>
        /// コメントの取得、設定
        /// </summary>
        public String Comment
        {
            get
            {
                return this.Field<String>(SpecimenResultDB.STRING_COMMENT);
            }
            protected set
            {
                this.SetField<String>(SpecimenResultDB.STRING_COMMENT, value);
            }
        }
        /// <summary>
        /// エラーコードの取得、設定
        /// </summary>
        public String ErrorCode
        {
            get
            {
                return this.Field<String>(SpecimenResultDB.STRING_ERRORCODE);
            }
            protected set
            {
                this.SetField<String>(SpecimenResultDB.STRING_ERRORCODE, value);
            }
        }
        /// <summary>
        /// 判定の取得、設定
        /// </summary>
        public String Judgement
        {
            get
            {
                return this.Field<String>(SpecimenResultDB.STRING_JUDGEMENT);
            }
            protected set
            {
                this.SetField<String>(SpecimenResultDB.STRING_JUDGEMENT, value);
            }
        }
        /// <summary>
        /// 手希釈倍率の取得、設定
        /// </summary>
        public Int32 ManualDilution
        {
            get
            {
                return this.Field<Int32>(SpecimenResultDB.STRING_MANUALDILUTION);
            }
            set
            {
                this.SetField<Int32>(SpecimenResultDB.STRING_MANUALDILUTION, value);
            }
        }
        /// <summary>
        /// 自動希釈倍率の取得、設定
        /// </summary>
        public Int32 AutoDilution
        {
            get
            {
                return this.Field<Int32>(SpecimenResultDB.STRING_AUTODILUTION);
            }
            protected set
            {
                this.SetField<Int32>(SpecimenResultDB.STRING_AUTODILUTION, value);
            }
        }
        /// <summary>
        /// 再検査の取得、設定
        /// </summary>
        protected Int32? Remeasure
        {
            get
            {
                return this.Field<Int32?>(SpecimenResultDB.STRING_REMEASURE);
            }
            set
            {
                this.SetField<Int32?>(SpecimenResultDB.STRING_REMEASURE, value);
            }
        }

        /// <summary>
        /// 分析項目名の取得、設定
        /// </summary>
        public String Analytes
        {
            get
            {
                return Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(this.MeasureProtocolIndex).ProtocolName;
            }
            set
            {
                this.MeasureProtocolIndex = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromName(value).ProtocolIndex;
            }
        }

        /// <summary>
        /// ラックIDの取得、設定
        /// </summary>
        public String RackId
        {
            get
            {
                return ((CarisXIDString)this.Field<String>(SpecimenResultDB.STRING_RACKID)).ToString();
            }
            protected set
            {
                this.SetField<String>(SpecimenResultDB.STRING_RACKID, value);
            }
        }

        /// <summary>
        /// 検体種別の取得、設定
        /// </summary>
        public String SpecimenMaterialType
        {
            get
            {
                return ((SpecimenMaterialType)this.Field<Int32>(SpecimenResultDB.STRING_SPECIMENMATERIALTYPE)).ToTypeString();
            }
            protected set
            {
                SpecimenMaterialType type = (SpecimenMaterialType)0;
                value.SetTypeFromString(ref type);
                this.SetField<Int32>(SpecimenResultDB.STRING_SPECIMENMATERIALTYPE, (Int32)type);
            }
        }

        /// <summary>
        /// リマークの取得
        /// </summary>
        public String Remark
        {
            get
            {
                if (this.RemarkId != null)
                {
                    return String.Join(",", this.RemarkId.GetRemarkStrings());
                }
                else if (this.ReplicationRemarkId != null)
                {
                    return String.Join(",", this.ReplicationRemarkId.GetRemarkStrings());
                }
                return null;
            }
        }

        /// <summary>
        /// カウント値(文字列)の取得
        /// </summary>
        public String Count
        {
            get
            {
                if (this.CountValue.HasValue && (this.ReplicationRemarkId != null || this.ReplicationRemarkId.CanCalcCount))
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
                if (this.CountAveValue.HasValue)
                {
                    return this.CountAveValue.Value.ToString();
                }
                else if (this.RemarkId == null)//|| this.RemarkId.CanCalcCount )
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
                return this.Field<Int32?>(SpecimenResultDB.STRING_DARKCOUNT);
            }
            set
            {
                this.SetField<Int32?>(SpecimenResultDB.STRING_DARKCOUNT, value);
            }
        }

        /// <summary>
        /// バックグラウンドカウント値の取得、設定
        /// </summary>
        public Int32? BGCount
        {
            get
            {
                return this.Field<Int32?>(SpecimenResultDB.STRING_BGCOUNT);
            }
            set
            {
                this.SetField<Int32?>(SpecimenResultDB.STRING_BGCOUNT, value);
            }
        }

        /// <summary>
        /// 測定カウント値の取得、設定
        /// </summary>
        public Int32? ResultCount
        {
            get
            {
                return this.Field<Int32?>(SpecimenResultDB.STRING_RESULTCOUNT);
            }
            set
            {
                this.SetField<Int32?>(SpecimenResultDB.STRING_RESULTCOUNT, value);
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// サンプル種別の取得、設定
        /// </summary>
        public SpecimenMaterialType GetSpecimenMaterialType()
        {
            return (SpecimenMaterialType)this.Field<Int32>(SpecimenResultDB.STRING_SPECIMENMATERIALTYPE);
        }
        public Int32 GetReplicationNo()
        {
            return this.ReplicationNo;
        }

        public Int32 GetModuleNo()
        {
            return this.ModuleNo;
        }
        public void SetModuleNo(Int32 moduleNo)
        {
            this.ModuleNo = moduleNo;
        }
        public Int32 GetUniqueNo()
        {
            return this.UniqueNo;
        }
        public void SetUniqueNo(Int32 uniqueNo)
        {
            this.UniqueNo = uniqueNo;
        }
        public void SetReplicationNo(Int32 replicationNo)
        {
            this.ReplicationNo = replicationNo;
        }
        public Int32 GetIndividuallyNo()
        {
            return this.IndividuallyNo;
        }
        public void SetIndividuallyNo(Int32 individuallyNo)
        {
            this.IndividuallyNo = individuallyNo;
        }
        public void SetSequenceNo(Int32 sequenceNo)
        {
            this.SequenceNo = sequenceNo;
        }
        public void SetRackId(CarisXIDString rackId)
        {
            this.RackId = rackId.DispPreCharString;
        }
        public void SetRackPosition(Int32 rackPosition)
        {
            this.RackPosition = rackPosition;
        }
        public void SetPatientId(String patientId)
        {
            this.PatientId = patientId;
        }
        public Int32 GetMeasureProtocolIndex()
        {
            return this.MeasureProtocolIndex;
        }
        public void SetMeasureProtocolIndex(Int32 measureProtocolIndex)
        {
            this.MeasureProtocolIndex = measureProtocolIndex;
        }
        public Int32? GetCount()
        {
            return this.CountValue;
        }
        public void SetCount(Int32? count)
        {
            this.CountValue = count;
        }
        public void SetConcentration(String concentration)
        {
            this.Concentration = concentration;
        }
        public Remark GetReplicationRemarkId()
        {
            return this.ReplicationRemarkId;
        }
        public void SetReplicationRemarkId(Remark replicationRemarkId)
        {
            this.ReplicationRemarkId = replicationRemarkId;
        }
        public Int32? GetCountAve()
        {
            return this.CountAveValue;
        }
        public void SetCountAve(Int32? countAve)
        {
            this.CountAveValue = countAve;
        }
        public void SetConcentrationAve(String concentrationAve)
        {
            this.ConcentrationAve = concentrationAve;
        }
        public Remark GetRemarkId()
        {
            return this.RemarkId;
        }
        public void SetRemarkId(Remark remarkId)
        {
            this.RemarkId = remarkId;
        }
        public void SetMeasureDateTime(DateTime measureDateTime)
        {
            this.MeasureDateTime = measureDateTime;
        }
        public void SetReagentLotNo(String reagentLotNo)
        {
            this.ReagentLotNo = reagentLotNo;
        }
        public void SetPretriggerLotNo(String pretriggerLotNo)
        {
            this.PretriggerLotNo = pretriggerLotNo;
        }
        public void SetTriggerLotNo(String triggerLotNo)
        {
            this.TriggerLotNo = triggerLotNo;
        }
        public void SetCalibCurveDateTime(DateTime? calibCurveDateTime)
        {
            this.CalibCurveDateTime = calibCurveDateTime;
        }
        public void SetComment(String comment)
        {
            this.Comment = comment;
        }
        public void SetErrorCode(String errorCode)
        {
            this.ErrorCode = errorCode;
        }
        public void SetJudgement(String judgement)
        {
            this.Judgement = judgement;
        }
        public void SetAutoDilution(Int32 autoDilution)
        {
            this.AutoDilution = autoDilution;
        }
        public Int32? GetRemeasure()
        {
            return this.Remeasure;
        }
        public void SetRemeasure(Int32? remeasure)
        {
            this.Remeasure = remeasure;
        }
        public void SetSpecimenMaterialType(SpecimenMaterialType specimenMaterialType)
        {
            this.SetField<Int32>(SpecimenResultDB.STRING_SPECIMENMATERIALTYPE, (Int32)specimenMaterialType);
        }
        public void SetAnalysisMode(AnalysisModeKind analysisMode)
        {
            this.SetField<Int32>(SpecimenResultDB.STRING_ANALYSISMODE, (Int32)analysisMode);
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


        /// <summary>
        /// 受付番号の取得
        /// </summary>
        public Int32 GetReceiptNo()
        {
            // DB互換性維持
            return SubFunction.SafeParseInt32(this.Row[SpecimenResultDB.STRING_RECEIPTNO]);
        }

        /// <summary>
        /// 受付番号の設定
        /// </summary>
        /// <param name="receiptNo"></param>
        public void SetReceiptNo(Int32 receiptNo)
        {
            this.SetField<Int32>(SpecimenResultDB.STRING_RECEIPTNO, receiptNo);
        }

        /// <summary>
        /// 濃度値取得
        /// </summary>
        /// <remarks>
        /// 単位無しの濃度値を取得します
        /// </remarks>
        /// <returns>濃度値</returns>
        public String GetConcentration()
        {
            var conc = this.Field<String>(SpecimenResultDB.STRING_CONCENTRATION);
            if (!String.IsNullOrEmpty(conc) && (this.ReplicationRemarkId == null || this.ReplicationRemarkId.CanCalcConcentration))
            {
                return conc;
            }
            return CarisXConst.COUNT_CONCENTRATION_NOTHING;
        }

        /// <summary>
        /// 濃度値平均取得
        /// </summary>
        /// <remarks>
        /// 単位無しの濃度値平均を取得します。
        /// </remarks>
        /// <returns>濃度値平均</returns>
        public String GetConcentrationAve()
        {
            var conc = this.Field<String>(SpecimenResultDB.STRING_CONCENTRATIONAVE);
            if (!String.IsNullOrEmpty(conc) || this.RemarkId == null || this.RemarkId.CanCalcConcentration)
            {
                return conc;
            }
            return CarisXConst.COUNT_CONCENTRATION_NOTHING;
        }

        #endregion

        #region [内部クラス]

        /// <summary>
        /// 列キー(グリッド向け列キー設定用)
        /// </summary>
        public struct DataKeys
        {
            public const String SequenceNo = "SequenceNo";
            public const String RackId = "RackId";
            public const String RackPosition = "RackPosition";
            public const String PatientId = "PatientId";
            public const String ReplicationNo = "ReplicationNo";
            public const String SpecimenMaterialType = "SpecimenMaterialType";
            public const String Analytes = "Analytes";
            public const String ManualDilution = "ManualDilution";
            public const String AutoDilution = "AutoDilution";
            public const String Count = "Count";
            public const String Concentration = "Concentration";
            //public const String ReplicationRemark = "ReplicationRemark";
            public const String CountAve = "CountAve";
            public const String ConcentrationAve = "ConcentrationAve";
            public const String Remark = "Remark";
            public const String Judgement = "Judgement";
            public const String MeasureDateTime = "MeasureDateTime";
            public const String ReagentLotNo = "ReagentLotNo";
            public const String PretriggerLotNo = "PretriggerLotNo";
            public const String TriggerLotNo = "TriggerLotNo";
            public const String CalibCurveDateTime = "CalibCurveDateTime";
            public const String Comment = "Comment";
            public const String ErrorCode = "ErrorCode";
            public const String ReceiptNo = "ReceiptNo";
            public const String AnalysisMode = "AnalysisMode";
            public const String DarkCount = "DarkCount";
            public const String BGCount = "BGCount";
            public const String ResultCount = "ResultCount";
            public const String ModuleNo = "ModuleNo";
        };

        #endregion
    }

    /// <summary>
    /// 検体測定データ出力クラス
    /// </summary>
    public class OutPutSpecimenResultData : SpecimenResultData
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data"></param>
        public OutPutSpecimenResultData(DataRowWrapperBase data)
            : base(data)
        {
        }
        /// <summary>
        /// 濃度値の取得、設定
        /// </summary>
        public override string Concentration
        {
            get
            {
                var conc = this.Field<String>(SpecimenResultDB.STRING_CONCENTRATION);
                if (!String.IsNullOrEmpty(conc) && (this.ReplicationRemarkId == null || this.ReplicationRemarkId.CanCalcConcentration))
                {
                    String unit = String.Empty;
                    String sign = String.Empty;

                    var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(this.MeasureProtocolIndex);
                    if (!String.IsNullOrEmpty(conc) && protocol != null)
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

                return CarisXConst.COUNT_CONCENTRATION_NOTHING;
            }

            protected set
            {
                this.SetField<String>(SpecimenResultDB.STRING_CONCENTRATION, value);
            }
        }

        /// <summary>
        /// 濃度平均値の取得、設定
        /// </summary>
        public override String ConcentrationAve
        {
            get
            {
                var conc = this.Field<String>(SpecimenResultDB.STRING_CONCENTRATIONAVE);
                if (!String.IsNullOrEmpty(conc) || this.RemarkId == null || this.RemarkId.CanCalcConcentration)
                {
                    String unit = String.Empty;
                    String sign = String.Empty;

                    var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(this.MeasureProtocolIndex);
                    if (!String.IsNullOrEmpty(conc) && protocol != null)
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

                return CarisXConst.COUNT_CONCENTRATION_NOTHING;
            }

            protected set
            {
                this.SetField<String>(SpecimenResultDB.STRING_CONCENTRATIONAVE, value);
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
        private String getConcUnit(String concentration)
        {
            String unit = String.Empty;

            // 濃度算出不能である場合String.Emptyを返す
            if (!String.IsNullOrEmpty(concentration) && (concentration != CarisXConst.COUNT_CONCENTRATION_NOTHING))
            {
                var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(this.MeasureProtocolIndex);
                if (protocol != null)
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
            unit = this.getConcUnit(conc);
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
            unit = this.getConcUnit(conc);
            return unit;
        }

    }

    /// <summary>
    /// 検体測定結果DBアクセスクラス
    /// </summary>
    public class SpecimenResultDB : DBAccessControl
    {
        #region [定数定義]

        /// <summary>
        /// 分析モジュール番号(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_MODULENO = "moduleNo";
        /// <summary>
        /// ユニーク番号(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_UNIQUENO = "uniqueNo";
        /// <summary>
        /// 多重測定回数番号(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_REPLICATIONNO = "replicationNo";
        /// <summary>
        /// 検体識別番号(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_INDIVIDUALLYNO = "individuallyNo";
        /// <summary>
        /// シーケンス番号(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_SEQUENCENO = "sequenceNo";
        /// <summary>
        /// ラックID(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_RACKID = "rackId";
        /// <summary>
        /// ラックポジション(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_RACKPOSITION = "rackPosition";
        /// <summary>
        /// 検体ID(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_PATIENTID = "sampleId";
        /// <summary>
        /// 分析項目インデックス(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_MEASUREPROTOCOLINDEX = "measureProtocolIndex";
        /// <summary>
        /// カウント値(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_COUNT = "count";
        /// <summary>
        /// 濃度値(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_CONCENTRATION = "concentration";
        /// <summary>
        /// 多重測定回リマークID(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_REPLICATIONREMARKID = "replicationRemarkId";
        /// <summary>
        /// カウント平均値(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_COUNTAVE = "countAve";
        /// <summary>
        /// 濃度平均値(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_CONCENTRATIONAVE = "concentrationAve";
        /// <summary>
        /// リマークID(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_REMARKID = "remarkId";
        /// <summary>
        /// 測定日時(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_MEASUREDATETIME = "measureDateTime";
        /// <summary>
        /// 試薬ロット番号(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_REAGENTLOTNO = "reagentLotNo";
        /// <summary>
        /// プレトリガーロット番号(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_PRETRIGGERLOTNO = "pretriggerLotNo";
        /// <summary>
        /// トリガーロット番号(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_TRIGGERLOTNO = "triggerLotNo";
        /// <summary>
        /// 使用した検量線の測定日時(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_CALIBCURVEDATETIME = "calibCurveDateTime";
        /// <summary>
        /// コメント(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_COMMENT = "comment";
        /// <summary>
        /// エラーコード(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_ERRORCODE = "errorCode";
        /// <summary>
        /// 判定結果(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_JUDGEMENT = "judgement";
        /// <summary>
        /// 手希釈倍率(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_MANUALDILUTION = "manualDilution";
        /// <summary>
        /// 自動希釈倍率(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_AUTODILUTION = "autoDilution";
        /// <summary>
        /// 再検査フラグ(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_REMEASURE = "remeasure";
        /// <summary>
        /// 検体種別(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_SPECIMENMATERIALTYPE = "specimenMaterialType";
        /// <summary>
        /// 受付番号(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_RECEIPTNO = "receiptNo";
        /// <summary>
        /// 分析モード(DBテーブル：sampleResult列名)
        /// </summary>
        public const String STRING_ANALYSISMODE = "analysisMode";
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
                return "SELECT * FROM dbo.specimenResult";
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
        public Boolean AddResultData(CalcData calcData, IMeasureResultData data, Int32? remeasure, String comment)
        {
            Boolean result = false;

            // 測定結果データ設定
            if (this.DataTable != null)
            {
                DataRow addRow = this.DataTable.Clone().NewRow();
                SpecimenResultData resultData = new SpecimenResultData(addRow);

                // 測定結果設定
                var measProto = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(calcData.ProtocolIndex);
                SampleInfo info = Singleton<InProcessSampleInfoManager>.Instance.SearchInProcessSampleFromUniqueNo(calcData.UniqueNo);
                resultData.SetModuleNo(data.ModuleID);
                resultData.SetUniqueNo(calcData.UniqueNo);
                resultData.SetReplicationNo(calcData.ReplicationNo);
                resultData.SetIndividuallyNo(calcData.IndividuallyNo);
                resultData.SetSequenceNo(info.SequenceNumber);
                resultData.SetRackId(calcData.RackID);
                resultData.SetRackPosition(calcData.RackPosition.Value);
                resultData.SetPatientId(info.SampleId);
                resultData.SetMeasureProtocolIndex(calcData.ProtocolIndex);
                resultData.SetReceiptNo(info.ReceiptNumber);
                if (measProto.UseEmergencyMode)
                {
                    resultData.SetAnalysisMode(AnalysisModeKind.Emergency);
                }
                else
                {
                    resultData.SetAnalysisMode(AnalysisModeKind.Standard);
                }
                resultData.SetDarkCount(data.DarkCount);
                resultData.SetBGCount(data.BGCount);
                resultData.SetResultCount(data.ResultCount);

                if (calcData.CalcInfoReplication != null)
                {
                    resultData.SetCount(calcData.CalcInfoReplication.CountValue);

                    if (calcData.CalcInfoReplication.Concentration.HasValue)
                    {
                        resultData.SetConcentration(SubFunction.ToRoundOffParse(calcData.CalcInfoReplication.Concentration.Value, measProto.LengthAfterDemPoint));
                    }
                    else
                    {
                        resultData.SetConcentration(null);
                    }

                    resultData.SetReplicationRemarkId(calcData.CalcInfoReplication.Remark);
                }

                if (calcData.CalcInfoAverage != null)
                {
                    if (calcData.CalcInfoAverage.CountValue.HasValue)
                    {
                        resultData.SetCountAve(calcData.CalcInfoAverage.CountValue);
                    }
                    else
                    {
                        resultData.SetCountAve(null);
                    }

                    if (calcData.CalcInfoAverage.Concentration.HasValue)
                    {
                        resultData.SetConcentrationAve(SubFunction.ToRoundOffParse(calcData.CalcInfoAverage.Concentration.Value, measProto.LengthAfterDemPoint));
                    }
                    else
                    {
                        resultData.SetConcentrationAve(null);
                    }
                    resultData.SetRemarkId(calcData.CalcInfoAverage.Remark);
                }

                resultData.SetMeasureDateTime(calcData.MeasureDateTime);
                resultData.SetReagentLotNo(calcData.ReagentLotNo);
                resultData.SetPretriggerLotNo(data.PreTriggerLotNo);
                resultData.SetTriggerLotNo(data.TriggerLotNo);
                resultData.SetCalibCurveDateTime(calcData.UseCalcCalibCurveApprovalDate);
                resultData.SetComment(comment);
                resultData.SetErrorCode(string.Join(",", data.ErrorLog.ErrorCodeArg.Select((codeArg) => codeArg.Item1 + "-" + codeArg.Item2).Where((error) => error.Length > 1)));

                if (!String.IsNullOrEmpty(calcData.Judgement))
                {
                    resultData.SetJudgement(calcData.Judgement);
                }
                else
                {
                    resultData.SetJudgement(null);
                }

                resultData.ManualDilution = calcData.ManualDilution;
                resultData.SetAutoDilution(calcData.AutoDilution);
                resultData.SetRemeasure(remeasure);
                resultData.SetSpecimenMaterialType(data.SpecimenMaterialType);

                this.SetData(new List<SpecimenResultData>() { resultData });

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
                                                         , contents);

                result = true;
            }

            return result;
        }

        /// <summary>
        /// 平均更新
        /// </summary>
        /// <param name="calcData">平均値保持データ</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean UpdateAverageData(CalcData calcData)
        {
            Boolean result = false;
            if (calcData != null)
            {
                if (this.DataTable != null)
                {
                    try
                    {
                        //　コピーデータリストを取得
                        var dataTableList = this.DataTable.AsEnumerable().ToList();

                        var updateData = (from v in dataTableList
                                          let data = new SpecimenResultData(v)
                                          where data.GetUniqueNo() == calcData.UniqueNo && data.ReplicationNo == calcData.ReplicationNo
                                          select data).SingleOrDefault();

                        if (updateData != null)
                        {
                            if (calcData.CalcInfoAverage != null)
                            {
                                Int32 digits = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(calcData.ProtocolIndex).LengthAfterDemPoint;
                                if (calcData.CalcInfoAverage.CountValue.HasValue)
                                {
                                    updateData.SetCountAve(calcData.CalcInfoAverage.CountValue);
                                }
                                else
                                {
                                    updateData.SetCountAve(null);
                                }

                                if (calcData.CalcInfoAverage.Concentration.HasValue)
                                {
                                    updateData.SetConcentrationAve(SubFunction.ToRoundOffParse(calcData.CalcInfoAverage.Concentration.Value, digits));
                                }
                                else
                                {
                                    updateData.SetConcentrationAve(null);
                                }
                                updateData.SetRemarkId(calcData.CalcInfoAverage.Remark);
                            }
                        }

                        this.SetData(new List<SpecimenResultData>() { updateData });
                        result = true;
                    }
                    catch (Exception ex)
                    {

                        // DB内部に不正データ
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                               CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 更新IGRA项目的阴阳性判定（基于运行效率考虑）
        /// </summary>
        /// <param name="uniqueNo"></param>
        /// <param name="judge"></param>
        /// <returns></returns>
        public Boolean UpdateIGRAJudge(int uniqueNo, JudgementType judge)
        {

            Boolean result = false;

            if (this.DataTable != null)
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    var updateData = (from v in dataTableList
                                      let data = new SpecimenResultData(v)
                                      where data.GetUniqueNo() == uniqueNo
                                      select data).SingleOrDefault();

                    if (updateData != null)
                    {
                        updateData.SetJudgement(judge.ToTypeString());
                    }

                    this.SetData(new List<SpecimenResultData>() { updateData });
                    result = true;
                }
                catch (Exception ex)
                {
                    // DB内部に不正データ
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                }

            }
            return result;
        }

        /// <summary>
        /// 検体測定結果情報テーブルの取得
        /// </summary>
        /// <returns></returns>
        public List<SpecimenResultData> GetSearchData_Split(int pageCurrent = 0, ISearchInfoSpecimenResult searchInfo = null)
        {
            List<SpecimenResultData> result = new List<SpecimenResultData>();

            if (this.DataTable != null)
            {
                try
                {
                    // 表示範囲を取得
                    int displayRange = (pageCurrent - 1) / 10;

                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    // 表示範囲分のデータを取得
                    var tempDatas = dataTableList.Skip(displayRange * CarisXConst.MAX_SAMPLERESULT_TEST_GET_COUNT)
                                        .Take(CarisXConst.MAX_SAMPLERESULT_TEST_GET_COUNT).Select(v => new SpecimenResultData(v));

                    // 取得データを日付でソート
                    var datas = tempDatas.Where(v => this.getSpecimenResultDataWhere(searchInfo, v)).OrderBy(d => d.MeasureDateTime).Select(v => v);
                    result.AddRange(datas.ToList());
                }
                catch (Exception ex)
                {
                    // DB内部に不正データ
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                }
            }

            return result;
        }

        /// <summary>
        /// 検体測定結果情報テーブルの取得
        /// </summary>
        /// <returns></returns>
        public List<SpecimenResultData> GetSearchData(ISearchInfoSpecimenResult searchInfo = null)
        {
            List<SpecimenResultData> result = new List<SpecimenResultData>();

            if (this.DataTable != null)
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    var datas = from v in dataTableList
                                let data = new SpecimenResultData(v)
                                where this.getSpecimenResultDataWhere(searchInfo, data)
                                orderby data.MeasureDateTime descending
                                select data;

                    result.AddRange(datas.ToList());
                }
                catch (Exception ex)
                {
                    // DB内部に不正データ
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                }
            }

            return result;
        }

        /// <summary>
        /// 検体測定結果情報テーブルの取得
        /// </summary>
        /// <returns></returns>
        public List<SpecimenResultData> GetReCalcData(IRecalcInfoSpecimenResult recalcInfo = null)
        {
            List<SpecimenResultData> result = new List<SpecimenResultData>();

            if (this.DataTable != null)
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    var uniqueNoList = (from v in dataTableList
                                        let data = new SpecimenResultData(v)
                                        where this.getSpecimenResultDataWhere(recalcInfo, data)
                                        select data.GetUniqueNo()).Distinct().ToList();

                    var datas = from v in dataTableList
                                let data = new SpecimenResultData(v)
                                where uniqueNoList.Contains(data.GetUniqueNo())
                                orderby data.MeasureDateTime descending
                                select data;

                    result.AddRange(datas);
                }
                catch (Exception ex)
                {
                    // DB内部に不正データ
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                }
            }

            return result;
        }

        /// <summary>
        /// 検体測定結果データの設定
        /// </summary>
        /// <param name="list">変更、削除操作済みデータ</param>
        public void SetData(List<SpecimenResultData> list)
        {
            list.SyncDataListToDataTable(this.DataTable);
        }

        /// <summary>
        /// 検体測定結果情報テーブル取得
        /// </summary>
        /// <remarks>
        /// 検体測定結果情報をDBから読込みます。
        /// </remarks>
        public override void LoadDB()
        {
            this.fillBaseTable();
        }

        /// <summary>
        /// 検体測定結果情報テーブル書込み
        /// </summary>
        /// <remarks>
        /// 検体測定結果情報をDBに書き込みます。
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
            var dataResult = this.OpenQuery( String.Format( "SELECT MAX({0}) FROM dbo.specimenResult", SpecimenResultDB.STRING_UNIQUENO ) );

            if( dataResult != null && dataResult.Rows.Count > 0 )
            {
                try
                {
                    result = Convert.ToInt32( dataResult.Rows[0].ItemArray[0] );
                }
                catch (Exception ex)
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
        protected override void removeLimitOver()
        {
            if (this.DataTable != null)
            {
                try
                {
                    // 保存上限を超過判断フラグ
                    // true:超過 false:超過していない
                    Boolean isLimitOver = false;
                    int dataCount = this.DataTable.Rows.Count;

                    // データ数が保存上限以上の場合
                    if (dataCount > CarisXConst.MAX_SAMPLERESULT_TEST_COUNT)
                    {
                        isLimitOver = true;
                    }
                    // データ数が保存上限未満の場合
                    else
                    {
                        // 処理なし
                    }

                    // 保存上限を超過している場合
                    if (isLimitOver == true)
                    {
                        //　コピーデータリストを取得
                        var dataTableList = this.DataTable.AsEnumerable().ToList();

                        // 削除予定のデータ数を取得
                        int deleteDataCount = dataTableList.Select(v => new SpecimenResultData(v))
                                                .Where(d => d.IsDeletedData() == true).Count();

                        // 削除しないデータ数が保存上限以上の場合
                        if ((dataCount - deleteDataCount) > CarisXConst.MAX_SAMPLERESULT_TEST_COUNT)
                        {
                            isLimitOver = true;
                        }
                        // 削除しないデータ数が保存上限未満の場合
                        else
                        {
                            isLimitOver = false;
                        }
                    }
                    else
                    {
                        // 処理なし
                    }

                    // 保存上限を超過している場合
                    if (isLimitOver == true)
                    {
                        List<SpecimenResultData> deleteData = new List<SpecimenResultData>();

                        var dataTableList = this.DataTable.AsEnumerable().ToList();

                        // 削除しないデータで保存上限を超過しているデータを取得
                        var datas = dataTableList.Select(v => new SpecimenResultData(v))
                                      .OrderBy(data => data.MeasureDateTime).Where(data => !data.IsDeletedData())
                                      .Skip(CarisXConst.MAX_SAMPLERESULT_TEST_COUNT).ToList();

                        // 保存上限を超過しているデータの検体識別番号を取得
                        var deleteIndividuallyList = datas.Select((data) => data.GetIndividuallyNo()).Distinct();

                        // 保存上限を超過しているデータを探索
                        foreach (int individuallyNo in deleteIndividuallyList)
                        {
                            // データテーブルを探索
                            foreach (var data in dataTableList)
                            {
                                SpecimenResultData resultData = new SpecimenResultData(data);

                                // 検体識別番号が一致する場合
                                if (resultData.GetIndividuallyNo() == individuallyNo)
                                {
                                    // 削除データに追加
                                    deleteData.Add(resultData);
                                }
                                else
                                {
                                    // 処理なし
                                }

                            }

                        }

                        deleteData.DeleteAllDataList();
                        this.SetData(deleteData);
                    }
                    else
                    {
                        // 処理無し
                    }
                }
                catch( Exception ex )
                {
                    // データ削除失敗
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// データ数の取得
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        public int GetDataCount(ISearchInfoSpecimenResult searchInfo = null)
        {
            int count = 0;

            try
            {
                // 10万件ずつ、全データを取得する
                for (int idx = 0; idx < this.DataTable.Rows.Count; idx += CarisXConst.MAX_SAMPLERESULT_TEST_GET_COUNT)
                {
                    // 条件に合致する個数を加算する
                    count += this.DataTable.AsEnumerable().Skip(idx).Take(CarisXConst.MAX_SAMPLERESULT_TEST_GET_COUNT)
                              .Select(v => new SpecimenResultData(v)).Where(d => this.getSpecimenResultDataWhere(searchInfo, d)).Count();
                }
            }
            catch (Exception ex)
            {
                // データ取得失敗
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                       CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
            }

            return count;
        }
        #endregion

        #region [privateメソッド]

        /// <summary>
        /// 絞込み条件チェック
        /// </summary>
        /// <remarks>
        /// 絞り込み条件に適合するかどうかをチェックします。
        /// </remarks>
        /// <param name="searchInfo">条件</param>
        /// <param name="data">チェック対象データ</param>
        /// <returns>チェック結果(true:適合)</returns>
        private Boolean getSpecimenResultDataWhere(ISearchInfoSpecimenResult searchInfo, SpecimenResultData data)
        {
            // 検索条件無しの場合
            if (searchInfo == null)
            {
                // 現在日時の日付のデータに限定
                if (data.MeasureDateTime.Date != DateTime.Today.Date)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            Boolean bResult = false;
            // 分析項目
            if (searchInfo.AnalyteSelect.Count > 0)
            {
                if (!searchInfo.AnalyteSelect.Contains(data.Analytes))
                {
                    return false;
                }
                else
                {
                    bResult = true;
                }
            }

            // 濃度値
            if (searchInfo.ConcentrationSelect.Item1)
            {
                bResult = false;
                Double concentration = 0;
                if (!Double.TryParse(data.GetConcentration(), out concentration) ||
                    (searchInfo.ConcentrationSelect.Item2 > concentration || searchInfo.ConcentrationSelect.Item3 < concentration))
                {
                    return false;
                }
                else
                {
                    bResult = true;
                }
            }

            // ラックID
            if (searchInfo.RackIdSelect.Item1)
            {
                bResult = false;
                if (searchInfo.RackIdSelect.Item2.Value > ((CarisXIDString)data.RackId).Value ||
                    searchInfo.RackIdSelect.Item3.Value < ((CarisXIDString)data.RackId).Value)
                {
                    return false;
                }
                else
                {
                    bResult = true;
                }
            }

            // シーケンス番号
            if (searchInfo.SequenceNoSelect.Item1)
            {
                bResult = false;
                if (searchInfo.SequenceNoSelect.Item2 > data.SequenceNo ||
                    searchInfo.SequenceNoSelect.Item3 < data.SequenceNo)
                {
                    return false;
                }
                else
                {
                    bResult = true;
                }
            }

            // 測定日
            if (searchInfo.MeasuringTimeSelect.Item1)
            {
                bResult = false;
                if (searchInfo.MeasuringTimeSelect.Item2.Date > data.MeasureDateTime.Date ||
                    searchInfo.MeasuringTimeSelect.Item3.Date < data.MeasureDateTime.Date)
                {
                    return false;
                }
                else
                {
                    bResult = true;
                }
            }

            // リマーク
            if (searchInfo.RemarkSelect > 0)
            {
                bResult = false;
                if ((data.GetRemarkId() != null && !data.GetRemarkId().IsBelongCategory(searchInfo.RemarkSelect)) || !(data.GetReplicationRemarkId() ?? (Remark)0).IsBelongCategory(searchInfo.RemarkSelect))
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

            // 判定
            if (searchInfo.JudgementSelect.Item1)
            {
                bResult = false;
                if (String.IsNullOrEmpty(data.Judgement) || !data.Judgement.Contains("(" + searchInfo.JudgementSelect.Item2.ToTypeString() + ")"))
                {
                    return false;
                }
                else
                {
                    bResult = true;
                }
            }

            // 検体ID
            if (searchInfo.PatientIdSelect.Item1)
            {
                bResult = false;
                if (searchInfo.PatientIdSelect.Item2 != data.PatientId)
                {
                    return false;
                }
                else
                {
                    bResult = true;
                }
            }

            // 検体種別
            if (searchInfo.SpecimenMaterialTypeSelect.Item1)
            {
                bResult = false;
                if (searchInfo.SpecimenMaterialTypeSelect.Item2.ToTypeString() != data.SpecimenMaterialType)
                {
                    return false;
                }
                else
                {
                    bResult = true;
                }
            }

            // コメント
            if (searchInfo.CommentSelect.Item1)
            {
                bResult = false;
                if (searchInfo.CommentSelect.Item2 != data.Comment)
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
        /// 絞り込み条件チェック
        /// </summary>
        /// <param name="recalcInfo">条件</param>
        /// <param name="data">チェック対象データ</param>
        /// <returns>チェック結果(true:適合)</returns>
        private Boolean getSpecimenResultDataWhere(IRecalcInfoSpecimenResult recalcInfo, SpecimenResultData data)
        {
            // 検索条件無しの場合
            if (recalcInfo == null)
            {
                // 現在日時の日付のデータに限定
                if (data.MeasureDateTime.Date != DateTime.Today.Date)
                {
                    return false;
                }
                return true;
            }

            // 分析項目
            if (recalcInfo.AnalyteSelect.Count > 0)
            {
                if (!recalcInfo.AnalyteSelect.Contains(data.GetMeasureProtocolIndex()))
                {
                    return false;
                }
            }
            // ラックID
            if (recalcInfo.RackIdSelect.Item1)
            {
                if (recalcInfo.RackIdSelect.Item2.Value > ((CarisXIDString)data.RackId).Value ||
                    recalcInfo.RackIdSelect.Item3.Value < ((CarisXIDString)data.RackId).Value)
                {
                    return false;
                }
            }

            // シーケンス番号
            if (recalcInfo.SequenceNoSelect.Item1)
            {
                if (recalcInfo.SequenceNoSelect.Item2 > data.SequenceNo ||
                    recalcInfo.SequenceNoSelect.Item3 < data.SequenceNo)
                {
                    return false;
                }
            }

            // 測定日
            if (recalcInfo.MeasuringTimeSelect.Item1)
            {
                if (recalcInfo.MeasuringTimeSelect.Item2.Date > data.MeasureDateTime.Date ||
                    recalcInfo.MeasuringTimeSelect.Item3.Date < data.MeasureDateTime.Date)
                {
                    return false;
                }
            }

            // リマーク
            if (recalcInfo.RemarkSelect > 0)
            {
                if ((data.GetRemarkId() != null && !data.GetRemarkId().IsBelongCategory(recalcInfo.RemarkSelect)) || !(data.GetReplicationRemarkId() ?? (Remark)0).IsBelongCategory(recalcInfo.RemarkSelect))
                {
                    return false;
                }
            }

            // 検体ID
            if (recalcInfo.PatientIdSelect.Item1)
            {
                if (recalcInfo.PatientIdSelect.Item2 != data.PatientId)
                {
                    return false;
                }
            }

            // 検体種別
            if (recalcInfo.SpecimenMaterialTypeSelect.Item1)
            {
                if (recalcInfo.SpecimenMaterialTypeSelect.Item2.ToTypeString() != data.SpecimenMaterialType)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
