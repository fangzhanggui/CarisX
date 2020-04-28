using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Oelco.CarisX.Calculator;
using Oelco.CarisX.Common;
using Oelco.CarisX.Const;
using Oelco.CarisX.GUI.Controls;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Utility;
using Oelco.Common.DB;
using Oelco.Common.Log;
using Oelco.Common.Utility;
using Oelco.CarisX.Log;

namespace Oelco.CarisX.DB
{
    /// <summary>
    /// 精度管理精度検体測定結果情報
    /// </summary>
    public class ControlResultData : DataRowWrapperBase
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data"></param>
        public ControlResultData(DataRowWrapperBase data)
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
                return this.Field<Int32>(ControlResultDB.STRING_MODULENO);
            }
            set
            {
                this.SetField<Int32>(ControlResultDB.STRING_MODULENO, value);
            }
        }
        /// <summary>
        /// ユニーク番号の取得、設定
        /// </summary>
        protected Int32 UniqueNo
        {
            get
            {
                return this.Field<Int32>(ControlResultDB.STRING_UNIQUENO);
            }
            set
            {
                this.SetField<Int32>(ControlResultDB.STRING_UNIQUENO, value);
            }
        }
        /// <summary>
        /// 多重測定回数番号の取得、設定
        /// </summary>
        public Int32 ReplicationNo
        {
            get
            {
                return this.Field<Int32>(ControlResultDB.STRING_REPLICATIONNO);
            }
            protected set
            {
                this.SetField<Int32>(ControlResultDB.STRING_REPLICATIONNO, value);
            }
        }
        /// <summary>
        /// 検体識別番号の取得、設定
        /// </summary>
        protected Int32 IndividuallyNo
        {
            get
            {
                return this.Field<Int32>(ControlResultDB.STRING_INDIVIDUALLYNO);
            }
            set
            {
                this.SetField<Int32>(ControlResultDB.STRING_INDIVIDUALLYNO, value);
            }
        }
        /// <summary>
        /// シーケンス番号の取得、設定
        /// </summary>
        public Int32 SequenceNo
        {
            get
            {
                return this.Field<Int32>(ControlResultDB.STRING_SEQUENCENO);
            }
            protected set
            {
                this.SetField<Int32>(ControlResultDB.STRING_SEQUENCENO, value);
            }
        }
        /// <summary>
        /// ラックIDの取得、設定
        /// </summary>
        public CarisXIDString RackId
        {
            get
            {
                return this.Field<String>(ControlResultDB.STRING_RACKID);
            }
            protected set
            {
                this.SetField<String>(ControlResultDB.STRING_RACKID, value.DispPreCharString);
            }
        }
        /// <summary>
        /// ラックポジションの取得、設定
        /// </summary>
        public Int32 RackPosition
        {
            get
            {
                return this.Field<Int32>(ControlResultDB.STRING_RACKPOSITION);
            }
            protected set
            {
                this.SetField<Int32>(ControlResultDB.STRING_RACKPOSITION, value);
            }
        }
        /// <summary>
        /// 精度管理検体ロット番号の取得、設定
        /// </summary>
        public String ControlLotNo
        {
            get
            {
                return this.Field<String>(ControlResultDB.STRING_CONTROLLOTNO);
            }
            protected set
            {
                this.SetField<String>(ControlResultDB.STRING_CONTROLLOTNO, value);
            }
        }
        /// <summary>
        /// 精度管理検体名の取得、設定
        /// </summary>
        public String ControlName
        {
            get
            {
                return this.Field<String>(ControlResultDB.STRING_CONTROLNAME);
            }
            protected set
            {
                this.SetField<String>(ControlResultDB.STRING_CONTROLNAME, value);
            }
        }

        /// <summary>
        /// カウント値の取得、設定
        /// </summary>
        protected Int32? CountValue
        {
            get
            {
                return this.Field<Int32?>(ControlResultDB.STRING_COUNT);
            }
            set
            {
                this.SetField<Int32?>(ControlResultDB.STRING_COUNT, value);
            }
        }
        /// <summary>
        /// 濃度値の取得、設定
        /// </summary>
        public virtual String Concentration
        {
            get
            {
                var conc = this.Field<String>(ControlResultDB.STRING_CONCENTRATION);
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

            set
            {
                Double check;
                if (Double.TryParse(value, out check))
                {
                    this.SetField<String>(ControlResultDB.STRING_CONCENTRATION, value);
                }
            }
        }
        /// <summary>
        /// 多重測定回リマークIDの取得、設定
        /// </summary>
        protected Remark ReplicationRemarkId
        {
            get
            {
                return this.Field<Int64>(ControlResultDB.STRING_REPLICATIONREMARKID);
            }
            set
            {
                this.SetField<Int64>(ControlResultDB.STRING_REPLICATIONREMARKID, value);
            }
        }
        /// <summary>
        /// カウント平均値の取得、設定
        /// </summary>
        protected Int32? CountAveValue
        {
            get
            {
                return this.Field<Int32?>(ControlResultDB.STRING_COUNTAVE);
            }
            set
            {
                this.SetField<Int32?>(ControlResultDB.STRING_COUNTAVE, value);
            }
        }
        /// <summary>
        /// 濃度平均値の取得、設定
        /// </summary>
        public virtual String ConcentrationAve
        {
            get
            {
                var conc = this.Field<String>(ControlResultDB.STRING_CONCENTRATIONAVE);
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
                this.SetField<String>(ControlResultDB.STRING_CONCENTRATIONAVE, value);
            }
        }

        /// <summary>
        /// リマークIDの取得、設定
        /// </summary>
        protected Remark RemarkId
        {
            get
            {
                return this.Field<Int64?>(ControlResultDB.STRING_REMARKID);
            }
            set
            {
                this.SetField<Int64?>(ControlResultDB.STRING_REMARKID, value);
            }
        }
        /// <summary>
        /// 分析項目インデックスの取得、設定
        /// </summary>
        protected Int32 MeasureProtocolIndex
        {
            get
            {
                return this.Field<Int32>(ControlResultDB.STRING_MEASUREPROTOCOLINDEX);
            }
            set
            {
                this.SetField<Int32>(ControlResultDB.STRING_MEASUREPROTOCOLINDEX, value);
            }
        }
        /// <summary>
        /// 測定日時の取得、設定
        /// </summary>
        public DateTime MeasureDateTime
        {
            get
            {
                return this.Field<DateTime>(ControlResultDB.STRING_MEASUREDATETIME);
            }
            protected set
            {
                this.SetField<DateTime>(ControlResultDB.STRING_MEASUREDATETIME, value);
            }
        }
        /// <summary>
        /// 試薬ロット番号の取得、設定
        /// </summary>
        public String ReagentLotNo
        {
            get
            {
                return this.Field<String>(ControlResultDB.STRING_REAGENTLOTNO);
            }
            protected set
            {
                this.SetField<String>(ControlResultDB.STRING_REAGENTLOTNO, value);
            }
        }
        /// <summary>
        /// プレトリガロット番号の取得、設定
        /// </summary>
        public String PretriggerLotNo
        {
            get
            {
                return this.Field<String>(ControlResultDB.STRING_PRETRIGGERLOTNO);
            }
            protected set
            {
                this.SetField<String>(ControlResultDB.STRING_PRETRIGGERLOTNO, value);
            }
        }
        /// <summary>
        /// トリガーロット番号の取得、設定
        /// </summary>
        public String TriggerLotNo
        {
            get
            {
                return this.Field<String>(ControlResultDB.STRING_TRIGGERLOTNO);
            }
            protected set
            {
                this.SetField<String>(ControlResultDB.STRING_TRIGGERLOTNO, value);
            }
        }
        /// <summary>
        /// 使用検量線作成日時の取得、設定
        /// </summary>
        public DateTime? CalibCurveDateTime
        {
            get
            {
                return this.Field<DateTime?>(ControlResultDB.STRING_CALIBCURVEDATETIME);
            }
            protected set
            {
                this.SetField<DateTime?>(ControlResultDB.STRING_CALIBCURVEDATETIME, value);
            }
        }
        /// <summary>
        /// コメントの取得、設定
        /// </summary>
        public String Comment
        {
            get
            {
                return this.Field<String>(ControlResultDB.STRING_COMMENT);
            }
            protected set
            {
                this.SetField<String>(ControlResultDB.STRING_COMMENT, value);
            }
        }
        /// <summary>
        /// エラーコードの取得、設定
        /// </summary>
        public String ErrorCode
        {
            get
            {
                return this.Field<String>(ControlResultDB.STRING_ERRORCODE);
            }
            protected set
            {
                this.SetField<String>(ControlResultDB.STRING_ERRORCODE, value);
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
                return this.Field<Int32?>(ControlResultDB.STRING_DARKCOUNT);
            }
            set
            {
                this.SetField<Int32?>(ControlResultDB.STRING_DARKCOUNT, value);
            }
        }
        /// <summary>
        /// バックグラウンドカウント値の取得、設定
        /// </summary>
        public Int32? BGCount
        {
            get
            {
                return this.Field<Int32?>(ControlResultDB.STRING_BGCOUNT);
            }
            set
            {
                this.SetField<Int32?>(ControlResultDB.STRING_BGCOUNT, value);
            }
        }
        /// <summary>
        /// 測定カウント値の取得、設定
        /// </summary>
        public Int32? ResultCount
        {
            get
            {
                return this.Field<Int32?>(ControlResultDB.STRING_RESULTCOUNT);
            }
            set
            {
                this.SetField<Int32?>(ControlResultDB.STRING_RESULTCOUNT, value);
            }
        }
        #endregion

        #region [publicメソッド]

        public Int32 GetModuleNo()
        {
            return this.ModuleNo;
        }
        public void SetModuleNo(Int32 moduleNo)
        {
            this.ModuleNo = moduleNo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public Int32 GetUniqueNo()
        {
            return this.UniqueNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="uniqueNo"></param>
        public void SetUniqueNo(Int32 uniqueNo)
        {
            this.UniqueNo = uniqueNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="replicationNo"></param>
        public void SetReplicationNo(Int32 replicationNo)
        {
            this.ReplicationNo = replicationNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public Int32 GetIndividuallyNo()
        {
            return this.IndividuallyNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="individuallyNo"></param>
        public void SetIndividuallyNo(Int32 individuallyNo)
        {
            this.IndividuallyNo = individuallyNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="sequenceNo"></param>
        public void SetSequenceNo(Int32 sequenceNo)
        {
            this.SequenceNo = sequenceNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="rackId"></param>
        public void SetRackId(CarisXIDString rackId)
        {
            this.RackId = rackId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rackPosition"></param>
        public void SetRackPosition(Int32 rackPosition)
        {
            this.RackPosition = rackPosition;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="controlLotNo"></param>
        public void SetControlLotNo(String controlLotNo)
        {
            this.ControlLotNo = controlLotNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="controlName"></param>
        public void SetControlName(String controlName)
        {
            this.ControlName = controlName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public Int32 GetMeasureProtocolIndex()
        {
            return this.MeasureProtocolIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="measureProtocolIndex"></param>
        public void SetMeasureProtocolIndex(Int32 measureProtocolIndex)
        {
            this.MeasureProtocolIndex = measureProtocolIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public Int32? GetCount()
        {
            return this.CountValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="count"></param>
        public void SetCount(Int32? count)
        {
            this.CountValue = count;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="concentration"></param>
        public void SetConcentration(String concentration)
        {
            this.Concentration = concentration;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public Remark GetReplicationRemarkId()
        {
            return this.ReplicationRemarkId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="replicationRemarkId"></param>
        public void SetReplicationRemarkId(Remark replicationRemarkId)
        {
            this.ReplicationRemarkId = replicationRemarkId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public Int32? GetCountAveValue()
        {
            return this.CountAveValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="countAve"></param>
        public void SetCountAve(Int32? countAve)
        {
            this.CountAveValue = countAve;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="concentrationAve"></param>
        public void SetConcentrationAve(String concentrationAve)
        {
            this.ConcentrationAve = concentrationAve;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public Remark GetRemarkId()
        {
            return this.RemarkId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="remarkId"></param>
        public void SetRemarkId(Remark remarkId)
        {
            this.RemarkId = remarkId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="measureDateTime"></param>
        public void SetMeasureDateTime(DateTime measureDateTime)
        {
            this.MeasureDateTime = measureDateTime;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="reagentLotNo"></param>
        public void SetReagentLotNo(String reagentLotNo)
        {
            this.ReagentLotNo = reagentLotNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="pretriggerLotNo"></param>
        public void SetPretriggerLotNo(String pretriggerLotNo)
        {
            this.PretriggerLotNo = pretriggerLotNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="triggerLotNo"></param>
        public void SetTriggerLotNo(String triggerLotNo)
        {
            this.TriggerLotNo = triggerLotNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="calibCurveDateTime"></param>
        public void SetCalibCurveDateTime(DateTime? calibCurveDateTime)
        {
            this.CalibCurveDateTime = calibCurveDateTime;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="comment"></param>
        public void SetComment(String comment)
        {
            this.Comment = comment;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="errorCode"></param>
        public void SetErrorCode(String errorCode)
        {
            this.ErrorCode = errorCode;
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
            var conc = this.Field<String>(ControlResultDB.STRING_CONCENTRATION);
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
            var conc = this.Field<String>(ControlResultDB.STRING_CONCENTRATIONAVE);
            if (!String.IsNullOrEmpty(conc) || this.RemarkId == null || this.RemarkId.CanCalcConcentration)
            {
                return conc;
            }
            return CarisXConst.COUNT_CONCENTRATION_NOTHING;
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
        /// 列キー(グリッド向け列キー設定用)
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public struct DataKeys
        {
            public const String SequenceNo = "SequenceNo";
            public const String RackID = "RackId";
            public const String RackPosition = "RackPosition";
            public const String ControlLotNo = "ControlLotNo";
            public const String ControlName = "ControlName";
            public const String Count = "Count";
            public const String Concentration = "Concentration";
            public const String CountAve = "CountAve";
            public const String ConcentrationAve = "ConcentrationAve";
            public const String MeasureDateTime = "MeasureDateTime";
            public const String Comment = "Comment";
            public const String ReplicationNo = "ReplicationNo";
            public const String ReagentLotNo = "ReagentLotNo";
            public const String PretriggerLotNo = "PretriggerLotNo";
            public const String TriggerLotNo = "TriggerLotNo";
            public const String CalibCurveDateTime = "CalibCurveDateTime";
            public const String ErrorCode = "ErrorCode";
            public const String Analytes = "Analytes";
            public const String Remark = "Remark";
            public const String DarkCount = "DarkCount";
            public const String BGCount = "BGCount";
            public const String ResultCount = "ResultCount";
            public const String ModuleNo = "ModuleNo";
        };

        #endregion
    }

    /// <summary>
    /// コントロール測定データ出力クラス
    /// </summary>    
    public class OutPutControlResultData : ControlResultData
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data"></param>
        public OutPutControlResultData(DataRowWrapperBase data)
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
                var conc = this.Field<String>(ControlResultDB.STRING_CONCENTRATION);
                if (!String.IsNullOrEmpty(conc) && (this.ReplicationRemarkId == null || this.ReplicationRemarkId.CanCalcConcentration))
                {
                    String unit = String.Empty;
                    String sign = String.Empty;

                    var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(this.MeasureProtocolIndex);
                    if (!String.IsNullOrEmpty(conc) && protocol != null)
                    {

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
                else
                {
                    return CarisXConst.COUNT_CONCENTRATION_NOTHING;
                }
            }

            set
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
                var conc = this.Field<String>(ControlResultDB.STRING_CONCENTRATIONAVE);
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
    /// 精度管理精度検体測定結果情報(精度管理向け)
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class ControlResultDataQC : DataRowWrapperBase
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data"></param>
        public ControlResultDataQC(DataRowWrapperBase data)
            : base(data)
        {
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 分析モジュール番号の取得、設定
        /// </summary>
        protected Int32 ModuleNo
        {
            get
            {
                return this.Field<Int32>(ControlResultDB.STRING_MODULENO);
            }
            set
            {
                this.SetField<Int32>(ControlResultDB.STRING_MODULENO, value);
            }
        }
        /// <summary>
        /// ユニーク番号の取得、設定
        /// </summary>
        protected Int32 UniqueNo
        {
            get
            {
                return this.Field<Int32>(ControlResultDB.STRING_UNIQUENO);
            }
            set
            {
                this.SetField<Int32>(ControlResultDB.STRING_UNIQUENO, value);
            }
        }
        /// <summary>
        /// 多重測定回数番号の取得、設定
        /// </summary>
        protected Int32 ReplicationNo
        {
            get
            {
                return this.Field<Int32>(ControlResultDB.STRING_REPLICATIONNO);
            }
            set
            {
                this.SetField<Int32>(ControlResultDB.STRING_REPLICATIONNO, value);
            }
        }
        /// <summary>
        /// 検体識別番号の取得、設定
        /// </summary>
        protected Int32 IndividuallyNo
        {
            get
            {
                return this.Field<Int32>(ControlResultDB.STRING_INDIVIDUALLYNO);
            }
            set
            {
                this.SetField<Int32>(ControlResultDB.STRING_INDIVIDUALLYNO, value);
            }
        }
        /// <summary>
        /// シーケンス番号の取得、設定
        /// </summary>
        public Int32 SequenceNo
        {
            get
            {
                return this.Field<Int32>(ControlResultDB.STRING_SEQUENCENO);
            }
            protected set
            {
                this.SetField<Int32>(ControlResultDB.STRING_SEQUENCENO, value);
            }
        }

        /// <summary>
        /// ラックIDの取得、設定
        /// </summary>
        protected String RackId
        {
            get
            {
                return this.Field<String>(ControlResultDB.STRING_RACKID);
            }
            set
            {
                this.SetField<String>(ControlResultDB.STRING_RACKID, value);
            }
        }
        /// <summary>
        /// ラックポジションの取得、設定
        /// </summary>
        protected Int32 RackPosition
        {
            get
            {
                return this.Field<Int32>(ControlResultDB.STRING_RACKPOSITION);
            }
            set
            {
                this.SetField<Int32>(ControlResultDB.STRING_RACKPOSITION, value);
            }
        }
        /// <summary>
        /// 精度管理検体ロット番号の取得、設定
        /// </summary>
        public String ControlLotNo
        {
            get
            {
                return this.Field<String>(ControlResultDB.STRING_CONTROLLOTNO);
            }
            protected set
            {
                this.SetField<String>(ControlResultDB.STRING_CONTROLLOTNO, value);
            }
        }
        /// <summary>
        /// 精度管理検体名の取得、設定
        /// </summary>
        public String ControlName
        {
            get
            {
                return this.Field<String>(ControlResultDB.STRING_CONTROLNAME);
            }
            protected set
            {
                this.SetField<String>(ControlResultDB.STRING_CONTROLNAME, value);
            }
        }
        /// <summary>
        /// 分析項目インデックスの取得、設定
        /// </summary>
        protected Int32 MeasureProtocolIndex
        {
            get
            {
                return this.Field<Int32>(ControlResultDB.STRING_MEASUREPROTOCOLINDEX);
            }
            set
            {
                this.SetField<Int32>(ControlResultDB.STRING_MEASUREPROTOCOLINDEX, value);
            }
        }
        /// <summary>
        /// カウント値の取得、設定
        /// </summary>
        protected Int32? Count
        {
            get
            {
                return this.Field<Int32?>(ControlResultDB.STRING_COUNT);

            }
            set
            {
                this.SetField<Int32?>(ControlResultDB.STRING_COUNT, value);
            }
        }
        /// <summary>
        /// 濃度値の取得、設定
        /// </summary>
        public String Concentration
        {
            get
            {
                var conc = this.Field<String>(ControlResultDB.STRING_CONCENTRATION);
                if (!String.IsNullOrEmpty(conc) || ((Remark)this.ReplicationRemarkId).CanCalcConcentration)
                {
                    return conc;
                }
                return CarisXConst.COUNT_CONCENTRATION_NOTHING;
            }
            set
            {
                Double check;
                if (Double.TryParse(value, out check))
                {
                    this.SetField<String>(ControlResultDB.STRING_CONCENTRATION, value);
                }
            }
        }
        /// <summary>
        /// 多重測定回リマークIDの取得、設定
        /// </summary>
        protected Int64 ReplicationRemarkId
        {
            get
            {
                return this.Field<Int64>(ControlResultDB.STRING_REPLICATIONREMARKID);
            }
            set
            {
                this.SetField<Int64>(ControlResultDB.STRING_REPLICATIONREMARKID, value);
            }
        }
        /// <summary>
        /// カウント平均値の取得、設定
        /// </summary>
        protected Int32? CountAve
        {
            get
            {
                return this.Field<Int32?>(ControlResultDB.STRING_COUNTAVE);
            }
            set
            {
                this.SetField<Int32?>(ControlResultDB.STRING_COUNTAVE, value);
            }
        }
        /// <summary>
        /// 濃度平均値の取得、設定
        /// </summary>
        public String ConcentrationAve
        {
            get
            {
                return this.Field<String>(ControlResultDB.STRING_CONCENTRATIONAVE);
            }
            protected set
            {
                this.SetField<String>(ControlResultDB.STRING_CONCENTRATIONAVE, value);
            }
        }
        /// <summary>
        /// リマークIDの取得、設定
        /// </summary>
        protected Int64? RemarkId
        {
            get
            {
                return this.Field<Int64?>(ControlResultDB.STRING_REMARKID);
            }
            set
            {
                this.SetField<Int64?>(ControlResultDB.STRING_REMARKID, value);
            }
        }
        /// <summary>
        /// 測定日時の取得、設定
        /// </summary>
        public DateTime MeasureDateTime
        {
            get
            {
                return this.Field<DateTime>(ControlResultDB.STRING_MEASUREDATETIME);
            }
            protected set
            {
                this.SetField<DateTime>(ControlResultDB.STRING_MEASUREDATETIME, value);
            }
        }
        /// <summary>
        /// 試薬ロット番号の取得、設定
        /// </summary>
        public String ReagentLotNo
        {
            get
            {
                return this.Field<String>(ControlResultDB.STRING_REAGENTLOTNO);
            }
            protected set
            {
                this.SetField<String>(ControlResultDB.STRING_REAGENTLOTNO, value);
            }
        }
        /// <summary>
        /// プレトリガロット番号の取得、設定
        /// </summary>
        protected String PretriggerLotNo
        {
            get
            {
                return this.Field<String>(ControlResultDB.STRING_PRETRIGGERLOTNO);
            }
            set
            {
                this.SetField<String>(ControlResultDB.STRING_PRETRIGGERLOTNO, value);
            }
        }
        /// <summary>
        /// トリガーロット番号の取得、設定
        /// </summary>
        protected String TriggerLotNo
        {
            get
            {
                return this.Field<String>(ControlResultDB.STRING_TRIGGERLOTNO);
            }
            set
            {
                this.SetField<String>(ControlResultDB.STRING_TRIGGERLOTNO, value);
            }
        }
        /// <summary>
        /// 使用検量線作成日時の取得、設定
        /// </summary>
        protected DateTime? CalibCurveDateTime
        {
            get
            {
                return this.Field<DateTime?>(ControlResultDB.STRING_CALIBCURVEDATETIME);
            }
            set
            {
                this.SetField<DateTime?>(ControlResultDB.STRING_CALIBCURVEDATETIME, value);
            }
        }
        /// <summary>
        /// コメントの取得、設定
        /// </summary>
        protected String Comment
        {
            get
            {
                return this.Field<String>(ControlResultDB.STRING_COMMENT);
            }
            set
            {
                this.SetField<String>(ControlResultDB.STRING_COMMENT, value);
            }
        }
        /// <summary>
        /// エラーコードの取得、設定
        /// </summary>
        protected String ErrorCode
        {
            get
            {
                return this.Field<String>(ControlResultDB.STRING_ERRORCODE);
            }
            set
            {
                this.SetField<String>(ControlResultDB.STRING_ERRORCODE, value);
            }
        }

        #endregion

        #region [publicメソッド]
        public Int32 GetModuleNo()
        {
            return this.ModuleNo;
        }
        public void SetModuleNo(Int32 moduleNo)
        {
            this.ModuleNo = moduleNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public Int32 GetUniqueNo()
        {
            return this.UniqueNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="uniqueNo"></param>
        public void SetUniqueNo(Int32 uniqueNo)
        {
            this.UniqueNo = uniqueNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public Int32 GetReplicationNo()
        {
            return this.ReplicationNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="replicationNo"></param>
        public void SetReplicationNo(Int32 replicationNo)
        {
            this.ReplicationNo = replicationNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public Int32 GetIndividuallyNo()
        {
            return this.IndividuallyNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="individuallyNo"></param>
        public void SetIndividuallyNo(Int32 individuallyNo)
        {
            this.IndividuallyNo = individuallyNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="sequenceNo"></param>
        public void SetSequenceNo(Int32 sequenceNo)
        {
            this.SequenceNo = sequenceNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public String GetRackId()
        {
            return this.RackId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="rackId"></param>
        public void SetRackId(String rackId)
        {
            this.RackId = rackId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public Int32 GetRackPosition()
        {
            return this.RackPosition;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="rackPosition"></param>
        public void SetRackPosition(Int32 rackPosition)
        {
            this.RackPosition = rackPosition;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="controlLotNo"></param>
        public void SetControlLotNo(String controlLotNo)
        {
            this.ControlLotNo = controlLotNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="controlName"></param>
        public void SetControlName(String controlName)
        {
            this.ControlName = controlName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public Int32 GetMeasureProtocolIndex()
        {
            return this.MeasureProtocolIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="measureProtocolIndex"></param>
        public void SetMeasureProtocolIndex(Int32 measureProtocolIndex)
        {
            this.MeasureProtocolIndex = measureProtocolIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public Int32? GetCount()
        {
            return this.Count;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="count"></param>
        public void SetCount(Int32? count)
        {
            this.Count = count;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public Int64 GetReplicationRemarkId()
        {
            return this.ReplicationRemarkId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="replicationRemarkId"></param>
        public void SetReplicationRemarkId(Int64 replicationRemarkId)
        {
            this.ReplicationRemarkId = replicationRemarkId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public Int32? GetCountAve()
        {
            return this.CountAve;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="countAve"></param>
        public void SetCountAve(Int32? countAve)
        {
            this.CountAve = countAve;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="concentrationAve"></param>
        public void SetConcentrationAve(String concentrationAve)
        {
            this.ConcentrationAve = concentrationAve;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public Int64? GetRemarkId()
        {
            return this.RemarkId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="remarkId"></param>
        public void SetRemarkId(Int64? remarkId)
        {
            this.RemarkId = remarkId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="measureDateTime"></param>
        public void SetMeasureDateTime(DateTime measureDateTime)
        {
            this.MeasureDateTime = measureDateTime;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="reagentLotNo"></param>
        public void SetReagentLotNo(String reagentLotNo)
        {
            this.ReagentLotNo = reagentLotNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public String GetPretriggerLotNo()
        {
            return this.PretriggerLotNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="pretriggerLotNo"></param>
        public void SetPretriggerLotNo(String pretriggerLotNo)
        {
            this.PretriggerLotNo = pretriggerLotNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public String GetTriggerLotNo()
        {
            return this.TriggerLotNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="triggerLotNo"></param>
        public void SetTriggerLotNo(String triggerLotNo)
        {
            this.TriggerLotNo = triggerLotNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public DateTime? GetCalibCurveDateTime()
        {
            return this.CalibCurveDateTime;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="calibCurveDateTime"></param>
        public void SetCalibCurveDateTime(DateTime? calibCurveDateTime)
        {
            this.CalibCurveDateTime = calibCurveDateTime;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public String GetComment()
        {
            return this.Comment;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="comment"></param>
        public void SetComment(String comment)
        {
            this.Comment = comment;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        public String GetErrorCode()
        {
            return this.ErrorCode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="errorCode"></param>
        public void SetErrorCode(String errorCode)
        {
            this.ErrorCode = errorCode;
        }

        #endregion

        #region [内部クラス]

        /// <summary>
        /// 列キー(グリッド向け列キー設定用)
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public struct DataKeys
        {
            public const String SequenceNo = "SequenceNo";
            public const String ControlLotNo = "ControlLotNo";
            public const String ControlName = "ControlName";
            public const String Concentration = "Concentration";
            public const String ConcentrationAve = "ConcentrationAve";
            public const String MeasureDateTime = "MeasureDateTime";
            public const String ReagentLotNo = "ReagentLotNo";
        };

        #endregion
    }


    /// <summary>
    /// 精度管理精度検体測定結果情報DBクラス
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class ControlResultDB : DBAccessControl
    {

        #region [定数定義]
        /// <summary>
        /// 分析モジュール番号(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_MODULENO = "moduleNo";
        /// <summary>
        /// ユニーク番号(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_UNIQUENO = "uniqueNo";
        /// <summary>
        /// 多重測定回数番号(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_REPLICATIONNO = "replicationNo";
        /// <summary>
        /// 検体識別番号(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_INDIVIDUALLYNO = "individuallyNo";
        /// <summary>
        /// シーケンス番号(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_SEQUENCENO = "sequenceNo";
        /// <summary>
        /// ラックID(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_RACKID = "rackId";
        /// <summary>
        /// ラックポジション(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_RACKPOSITION = "rackPosition";
        /// <summary>
        /// 精度管理検体ロット番号(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_CONTROLLOTNO = "controlLotNo";
        /// <summary>
        /// 精度管理検体名(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_CONTROLNAME = "controlName";
        /// <summary>
        /// 分析項目インデックス(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_MEASUREPROTOCOLINDEX = "measureProtocolIndex";
        /// <summary>
        /// カウント値(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_COUNT = "count";
        /// <summary>
        /// 濃度値(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_CONCENTRATION = "concentration";
        /// <summary>
        /// 多重測定回リマークID(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_REPLICATIONREMARKID = "replicationRemarkId";
        /// <summary>
        /// カウント平均値(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_COUNTAVE = "countAve";
        /// <summary>
        /// 濃度平均値(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_CONCENTRATIONAVE = "concentrationAve";
        /// <summary>
        /// リマークID(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_REMARKID = "remarkId";
        /// <summary>
        /// 測定日時(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_MEASUREDATETIME = "measureDateTime";
        /// <summary>
        /// 試薬ロット番号(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_REAGENTLOTNO = "reagentLotNo";
        /// <summary>
        /// プレトリガロット番号(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_PRETRIGGERLOTNO = "pretriggerLotNo";
        /// <summary>
        /// トリガーロット番号(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_TRIGGERLOTNO = "triggerLotNo";
        /// <summary>
        /// 使用検量線作成日時(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_CALIBCURVEDATETIME = "calibCurveDateTime";
        /// <summary>
        /// コメント(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_COMMENT = "comment";
        /// <summary>
        /// エラーコード(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_ERRORCODE = "errorCode";
        /// <summary>
        /// バッチ番号(DBテーブル：controlResult列名)
        /// </summary>
        public const String STRING_BATCHNO = "batchNo";
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
                return "SELECT * FROM dbo.controlResult";
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
        /// <param name="controlName">コントロール名</param>
        /// <param name="comment">コメント</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean AddResultData(CalcData calcData, IMeasureResultData data, String controlName, String comment)
        {
            Boolean result = false;

            // 測定結果データ設定
            if (this.DataTable != null)
            {
                DataRow addRow = this.DataTable.Clone().NewRow();
                ControlResultData resultData = new ControlResultData(addRow);

                // 測定結果設定
                Int32 digits = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(calcData.ProtocolIndex).LengthAfterDemPoint;
                SampleInfo info = Singleton<InProcessSampleInfoManager>.Instance.SearchInProcessSampleFromUniqueNo(calcData.UniqueNo);
                resultData.SetModuleNo(data.ModuleID);
                resultData.SetUniqueNo(calcData.UniqueNo);
                resultData.SetReplicationNo(calcData.ReplicationNo);
                resultData.SetIndividuallyNo(calcData.IndividuallyNo);
                resultData.SetSequenceNo(info.SequenceNumber);
                resultData.SetRackId(calcData.RackID);
                resultData.SetRackPosition(calcData.RackPosition.Value);
                resultData.SetControlLotNo(info.SampleId);
                resultData.SetControlName(controlName);
                resultData.SetMeasureProtocolIndex(calcData.ProtocolIndex);
                resultData.SetDarkCount(data.DarkCount);
                resultData.SetBGCount(data.BGCount);
                resultData.SetResultCount(data.ResultCount);

                if (calcData.CalcInfoReplication != null)
                {
                    resultData.SetCount(calcData.CalcInfoReplication.CountValue);

                    if (calcData.CalcInfoReplication.Concentration.HasValue)
                    {
                        resultData.SetConcentration(SubFunction.ToRoundOffParse(calcData.CalcInfoReplication.Concentration.Value, digits));
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
                        resultData.SetConcentrationAve(SubFunction.ToRoundOffParse(calcData.CalcInfoAverage.Concentration.Value, digits));
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

                this.SetData(new List<ControlResultData>() { resultData });

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
        /// <remarks>
        /// 平均更新します
        /// </remarks>
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
                                          let data = new ControlResultData(v)
                                          where ( (data.GetUniqueNo() == calcData.UniqueNo)
                                               && (data.ReplicationNo == calcData.ReplicationNo) )
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

                        this.SetData(new List<ControlResultData>() { updateData });
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
        /// 精度検体測定結果情報テーブルの取得
        /// (分析項目インデックスによる絞込み)
        /// </summary>
        /// <remarks>
        /// 分析項目インデックスを条件に取得した、精度検体測定結果情報テーブルデータを返します。
        /// </remarks>
        /// <returns></returns>
        public List<ControlResultData> GetData(Int32? measureProtocolIndex = null)
        {
            List<ControlResultData> result = new List<ControlResultData>();

            if (this.DataTable != null)
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    var datas = from v in dataTableList
                                let data = new ControlResultData(v)
                                where !measureProtocolIndex.HasValue || data.GetMeasureProtocolIndex() == measureProtocolIndex.Value
                                orderby ((DateTime)v[STRING_MEASUREDATETIME]) descending
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
        /// 精度検体測定結果情報テーブルの取得
        /// (分析項目インデックス、日付、精度管理検体名、精度管理検体ロット番号()による絞込み)
        /// </summary>
        /// <remarks>
        ///　分析項目インデックス、日付、精度管理検体名、精度管理検体ロット番号を条件に取得した精度検体測定結果データを返します。
        /// </remarks>
        /// <param name="measureProtocolIndex">分析項目インデックス</param>
        /// <param name="startDate">抽出開始日</param>
        /// <param name="endDate">抽出終了日</param>
        /// <param name="controlName">精度管理検体名</param>
        /// <param name="controlLotNo">精度管理検体ロット番号</param>
        /// <returns></returns>
        public List<ControlResultDataQC> GetDataQC(Int32 measureProtocolIndex, DateTime? startDate, DateTime? endDate, String controlName, String controlLotNo = null)
        {
            List<ControlResultDataQC> result = new List<ControlResultDataQC>();

            if (this.DataTable != null)
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    var sortData = from v in dataTableList
                                   let data = new ControlResultDataQC(v)
                                   where ( (data.Concentration != CarisXConst.COUNT_CONCENTRATION_NOTHING)
                                        && (data.ConcentrationAve != CarisXConst.COUNT_CONCENTRATION_NOTHING)
                                        && (data.GetMeasureProtocolIndex() == measureProtocolIndex)
                                        && (data.ControlName == controlName)
                                        && ( (data.ControlLotNo == controlLotNo)
                                          || (String.IsNullOrEmpty(controlLotNo)) ) )
                                   orderby data.ControlLotNo, data.MeasureDateTime
                                   select data;

                    var datas = (from data in sortData
                                 group data by data.GetUniqueNo() into grp
                                 select new
                                 {
                                     grp,
                                     DateMax = grp.Max((grpData) => grpData.MeasureDateTime),
                                     DateMin = grp.Min((grpData) => grpData.MeasureDateTime),
                                 }).Where((value) => (value.DateMax >= (startDate ?? DateTime.MinValue).Date && value.DateMin < (endDate ?? DateTime.MaxValue.AddDays(-1)).Date.AddDays(1))).Select((value) => value.grp);

                    foreach (var data in datas)
                    {
                        result.AddRange(data);
                    }
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
        /// 精度検体測定結果情報テーブルの取得
        /// </summary>
        /// <remarks>
        /// 引数値を条件に取得した精度検体測定結果情報テーブルを返します。
        /// </remarks>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        public List<ControlResultData> GetSearchData(ISearchInfoControlResult searchInfo = null)
        {
            List<ControlResultData> result = new List<ControlResultData>();

            if (this.DataTable != null)
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    var datas = from v in dataTableList
                                let data = new ControlResultData(v)
                                where this.getControlResultDataWhere(searchInfo, data)
                                orderby ((DateTime)v[STRING_MEASUREDATETIME]) descending
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
        /// 精度検体測定結果情報テーブルの取得
        /// </summary>
        /// <remarks>
        /// 引数値を条件に取得した精度検体測定結果情報テーブルを返します。
        /// </remarks>
        /// <returns></returns>
        public List<ControlResultData> GetReCalcData(IRecalcInfoControlResult recalcInfo = null)
        {
            List<ControlResultData> result = new List<ControlResultData>();

            if (this.DataTable != null)
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    var uniqueNoList = (from v in dataTableList
                                        let data = new ControlResultData(v)
                                        where this.getControlResultDataWhere(recalcInfo, data)
                                        select data.GetUniqueNo()).Distinct().ToList();

                    var datas = from v in dataTableList
                                let data = new ControlResultData(v)
                                where uniqueNoList.Contains(data.GetUniqueNo())
                                orderby ((DateTime)v[STRING_MEASUREDATETIME]) descending
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
        /// 精度検体測定結果データの設定
        /// </summary>
        /// <remarks>
        /// 精度検体測定結果データの同期を行います。
        /// </remarks>
        /// <param name="list">変更、削除操作済みデータ</param>
        public void SetData(List<ControlResultData> list)
        {
            list.SyncDataListToDataTable(this.DataTable);
        }
        /// <summary>
        /// 精度検体測定結果データの設定
        /// </summary>
        /// <remarks>
        /// 精度検体測定結果データの同期を行います。
        /// </remarks>
        /// <param name="list">変更、削除操作済みデータ</param>
        public void SetData(List<ControlResultDataQC> list)
        {
            list.SyncDataListToDataTable(this.DataTable);
        }

        /// <summary>
        /// 精度管理精度検体測定結果情報テーブル取得
        /// </summary>
        /// <remarks>
        /// 精度管理精度検体測定結果情報をDBから読込みます。
        /// </remarks>
        public override void LoadDB()
        {
            this.fillBaseTable();
        }

        /// <summary>
        /// 精度管理精度検体測定結果情報テーブル書込み
        /// </summary>
        /// <remarks>
        /// 精度管理精度検体測定結果情報をDBに書き込みます。
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
            var dataResult = this.OpenQuery( String.Format( "SELECT MAX({0}) FROM dbo.controlResult", SpecimenResultDB.STRING_UNIQUENO ) );

            if (dataResult != null && dataResult.Rows.Count > 0)
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
        /// <remarks>
        /// 保存数(分析)上限超過分の検体測定結果を削除します。
        /// </remarks>
        protected override void removeLimitOver()
        {
            if (this.DataTable != null)
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    var datas = from v in dataTableList
                                let data = new ControlResultData(v)
                                orderby data.MeasureDateTime descending
                                where !data.IsDeletedData()
                                select data;


                    List<ControlResultData> deleteData;
                    var samples = datas.GroupBy((data) => data.GetIndividuallyNo());

                    // 保存数(分析)上限超過分のコントロール測定結果削除
                    var deleteIndividuallyList = datas.Skip(CarisXConst.MAX_CONTROLRESULT_TEST_COUNT).Select((data) => data.GetIndividuallyNo()).Distinct();
                    deleteData = (from individuallyNo in deleteIndividuallyList
                                  let grpData = samples.FirstOrDefault((grp) => grp.Key == individuallyNo)
                                  where grpData != null
                                  select grpData).SelectMany((grp) => grp.AsEnumerable()).ToList();
                    deleteData.DeleteAllDataList();
                    this.SetData(deleteData);

                }
                catch (Exception ex)
                {
                    // DB内部に不正データ
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                }
            }
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
        private Boolean getControlResultDataWhere(ISearchInfoControlResult searchInfo, ControlResultData data)
        {
            // 検索条件無しの場合
            if (searchInfo == null)
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
                if (searchInfo.RackIdSelect.Item2.Value > data.RackId.Value ||
                    searchInfo.RackIdSelect.Item3.Value < data.RackId.Value)
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

            // 精度管理検体ロット
            if (searchInfo.ControlLotSelect.Item1)
            {
                bResult = false;
                if (searchInfo.ControlLotSelect.Item2 != data.ControlLotNo)
                {
                    return false;
                }
                else
                {
                    bResult = true;
                }
            }

            // 精度管理検体ロット
            if (searchInfo.ControlNameSelect.Item1)
            {
                bResult = false;
                if (searchInfo.ControlNameSelect.Item2 != data.ControlName)
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
        /// 絞込み条件チェック
        /// </summary>
        /// <remarks>
        /// 絞り込み条件に適合するかどうかをチェックします。
        /// </remarks>
        /// <param name="recalcInfo">条件</param>
        /// <param name="data">チェック対象データ</param>
        /// <returns>チェック結果(true:適合)</returns>
        private Boolean getControlResultDataWhere(IRecalcInfoControlResult recalcInfo, ControlResultData data)
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

            // ラックID
            if (recalcInfo.RackIdSelect.Item1)
            {
                if (recalcInfo.RackIdSelect.Item2.Value > data.RackId.Value ||
                    recalcInfo.RackIdSelect.Item3.Value < data.RackId.Value)
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

            // 精度管理検体ロット
            if (recalcInfo.ControlLotNoSelect.Item1)
            {
                if (recalcInfo.ControlLotNoSelect.Item2 != data.ControlLotNo)
                {
                    return false;
                }
            }

            // 精度管理検体名
            if (recalcInfo.ControlNameSelect.Item1)
            {
                if (recalcInfo.ControlNameSelect.Item2 != data.ControlName)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}
