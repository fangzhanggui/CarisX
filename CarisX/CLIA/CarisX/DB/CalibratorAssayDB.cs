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


namespace Oelco.CarisX.DB
{
    /// <summary>
    /// キャリブレータ分析データクラス
    /// </summary>
    public class CalibratorAssayData : DataRowWrapperBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="row"></param>
        public CalibratorAssayData(DataRow row)
            : base(row)
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
                return this.Field<Int32>(CalibratorAssayDB.STRING_UNIQUENO);
            }
            set
            {
                this.SetField<Int32>(CalibratorAssayDB.STRING_UNIQUENO, value);
            }
        }
        /// <summary>
        /// レプリケーション番号の取得、設定
        /// </summary>
        public Int32 ReplicationNo
        {
            get
            {
                return this.Field<Int32>(CalibratorAssayDB.STRING_REPLICATIONNO);
            }
            protected set
            {
                this.SetField<Int32>(CalibratorAssayDB.STRING_REPLICATIONNO, value);
            }
        }
        /// <summary>
        /// ラックポジションの取得、設定
        /// </summary>
        public Int32 RackPosition
        {
            get
            {
                return this.Field<Int32>(CalibratorAssayDB.STRING_RACKPOSITION);
            }
            protected set
            {
                this.SetField<Int32>(CalibratorAssayDB.STRING_RACKPOSITION, value);
            }
        }
        /// <summary>
        /// 検体識別番号の取得、設定
        /// </summary>
        protected Int32 IndividuallyNo
        {
            get
            {
                return this.Field<Int32>(CalibratorAssayDB.STRING_INDIVIDUALLYNO);
            }
            set
            {
                this.SetField<Int32>(CalibratorAssayDB.STRING_INDIVIDUALLYNO, value);
            }
        }
        /// <summary>
        /// シーケンス番号の取得、設定
        /// </summary>
        public Int32 SequenceNo
        {
            get
            {
                return this.Field<Int32>(CalibratorAssayDB.STRING_SEQUENCENO);
            }
            protected set
            {
                this.SetField<Int32>(CalibratorAssayDB.STRING_SEQUENCENO, value);
            }
        }
        /// <summary>
        /// キャリブレータロット番号の取得、設定
        /// </summary>
        public String CalibLotNo
        {
            get
            {
                return this.Field<String>(CalibratorAssayDB.STRING_CALIBLOTNO);
            }
            protected set
            {
                this.SetField<String>(CalibratorAssayDB.STRING_CALIBLOTNO, value);
            }
        }
        /// <summary>
        /// 分析項目インデックスの取得、設定
        /// </summary>
        protected Int32 MeasureProtocolIndex
        {
            get
            {
                return this.Field<Int32>(CalibratorAssayDB.STRING_MEASUREPROTOCOLINDEX);
            }
            set
            {
                this.SetField<Int32>(CalibratorAssayDB.STRING_MEASUREPROTOCOLINDEX, value);
            }
        }
        /// <summary>
        /// ステータスの取得、設定
        /// </summary>
        protected SampleInfo.SampleMeasureStatus Status
        {
            get
            {
                return (SampleInfo.SampleMeasureStatus)this.Field<Int32>(CalibratorAssayDB.STRING_STATUS);
            }
            set
            {
                this.SetField<Int32>(CalibratorAssayDB.STRING_STATUS, (Int32)value);
            }
        }
        /// <summary>
        /// 残時間の取得、設定
        /// </summary>
        public TimeSpan? RemainTime
        {
            get
            {
                Int64? field = this.Field<Int64?>(CalibratorAssayDB.STRING_REMAINTIME);
                if (field.HasValue)
                {
                    return new TimeSpan(field.Value);
                }
                else
                {
                    return null;
                }
            }
            protected set
            {
                if (value.HasValue)
                {
                    this.SetField<Int64?>(CalibratorAssayDB.STRING_REMAINTIME, value.Value.Ticks);
                }
                else
                {
                    this.SetField<Int64?>(CalibratorAssayDB.STRING_REMAINTIME, null);
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
                return this.Field<Int32?>(CalibratorAssayDB.STRING_COUNT);
            }
            set
            {
                this.SetField<Int32?>(CalibratorAssayDB.STRING_COUNT, value);
            }
        }

        public String GetConcentrationWithoutUnit()
        {
            return this.Field<String>(CalibratorAssayDB.STRING_CONCENTRATION);
        }

        /// <summary>
        /// 濃度値の取得、設定
        /// </summary>
        public String Concentration
        {
            get
            {
                var conc = this.Field<String>(CalibratorAssayDB.STRING_CONCENTRATION);
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
                            sign = CarisXConst.CONCENTRATION_LESS;
                        }
                    }
                }
                return sign + conc + unit;
            }
            protected set
            {
                this.SetField<String>(CalibratorAssayDB.STRING_CONCENTRATION, value);
            }
        }

        /// <summary>
        /// リマークIDの取得、設定
        /// </summary>
        protected Remark RemarkId
        {
            get
            {
                return this.Field<Int64?>(CalibratorAssayDB.STRING_REMARKID);
            }
            set
            {
                this.SetField<Int64?>(CalibratorAssayDB.STRING_REMARKID, value);
            }
        }
        /// <summary>
        /// 測定日時の取得、設定
        /// </summary>
        public DateTime? MeasureDateTime
        {
            get
            {
                return this.Field<DateTime?>(CalibratorAssayDB.STRING_MEASUREDATETIME);
            }
            protected set
            {
                this.SetField<DateTime?>(CalibratorAssayDB.STRING_MEASUREDATETIME, value);
            }
        }
        /// <summary>
        /// 試薬ロット番号の取得、設定
        /// </summary>
        public String ReagentLotNo
        {
            get
            {
                return this.Field<String>(CalibratorAssayDB.STRING_REAGENTLOTNO);
            }
            protected set
            {
                this.SetField<String>(CalibratorAssayDB.STRING_REAGENTLOTNO, value);
            }
        }
        /// <summary>
        /// プレトリガロット番号の取得、設定
        /// </summary>
        public String PretriggerLotNo
        {
            get
            {
                return this.Field<String>(CalibratorAssayDB.STRING_PRETRIGGERLOTNO);
            }
            protected set
            {
                this.SetField<String>(CalibratorAssayDB.STRING_PRETRIGGERLOTNO, value);
            }
        }
        /// <summary>
        /// トリガロット番号の取得、設定
        /// </summary>
        public String TriggerLotNo
        {
            get
            {
                return this.Field<String>(CalibratorAssayDB.STRING_TRIGGERLOTNO);
            }
            protected set
            {
                this.SetField<String>(CalibratorAssayDB.STRING_TRIGGERLOTNO, value);
            }
        }


        /// <summary>
        /// 分析項目名の取得
        /// </summary>
        public String Analytes
        {
            get
            {
                return Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(this.MeasureProtocolIndex).ProtocolName;
            }
        }
        /// <summary>
        /// ラックIDの取得、設定
        /// </summary>
        public CarisXIDString RackId
        {
            get
            {
                return this.Field<String>(CalibratorAssayDB.STRING_RACKID);
            }
            protected set
            {
                this.SetField<String>(CalibratorAssayDB.STRING_RACKID, value.DispPreCharString);
            }
        }
        /// <summary>
        /// リマーク文字列の取得
        /// </summary>
        public String Remark
        {
            get
            {
                if (this.RemarkId != null)
                {
                    return String.Join(",", ((Remark)this.RemarkId).GetRemarkStrings());
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
                if (this.CountValue.HasValue)
                {
                    return this.CountValue.Value.ToString();
                }
                else if (this.RemarkId.CanCalcCount)
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
                return this.Field<Int32?>(CalibratorAssayDB.STRING_DARKCOUNT);
            }
            set
            {
                this.SetField<Int32?>(CalibratorAssayDB.STRING_DARKCOUNT, value);
            }
        }

        /// <summary>
        /// バックグラウンドカウント値の取得、設定
        /// </summary>
        public Int32? BGCount
        {
            get
            {
                return this.Field<Int32?>(CalibratorAssayDB.STRING_BGCOUNT);
            }
            set
            {
                this.SetField<Int32?>(CalibratorAssayDB.STRING_BGCOUNT, value);
            }
        }

        /// <summary>
        /// 測定カウント値の取得、設定
        /// </summary>
        public Int32? ResultCount
        {
            get
            {
                return this.Field<Int32?>(CalibratorAssayDB.STRING_RESULTCOUNT);
            }
            set
            {
                this.SetField<Int32?>(CalibratorAssayDB.STRING_RESULTCOUNT, value);
            }
        }

        /// <summary>
        /// 分析モジュール番号の取得、設定
        /// </summary>
        public Int32 ModuleNo
        {
            get
            {
                return this.Field<Int32>(CalibratorAssayDB.STRING_MODULENO);
            }
            set
            {
                this.SetField<Int32>(CalibratorAssayDB.STRING_MODULENO, value);
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
        public void SetUniqueNo(Int32 uniqueNo)
        {
            this.UniqueNo = uniqueNo;
        }
        /// <summary>
        /// レプリケーション番号の設定
        /// </summary>
        /// <remarks>
        /// レプリケーション番号を設定します。
        /// </remarks>
        public void SetReplicationNo(Int32 replicationNo)
        {
            this.ReplicationNo = replicationNo;
        }
        /// <summary>
        /// ラックIDの設定
        /// </summary>
        /// <remarks>
        /// ラックIDを設定します。
        /// </remarks>
        public void SetRackId(CarisXIDString rackId)
        {
            this.RackId = rackId;
        }
        /// <summary>
        /// ラックポジションの設定
        /// </summary>
        /// <remarks>
        /// ラックポジションを設定します。
        /// </remarks>
        public void SetRackPosition(Int32 rackPosition)
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
        public void SetIndividuallyNo(Int32 individuallyNo)
        {
            this.IndividuallyNo = individuallyNo;
        }
        /// <summary>
        /// シーケンス番号の設定
        /// </summary>
        /// <remarks>
        /// シーケンス番号を設定します。
        /// </remarks>
        public void SetSequenceNo(Int32 sequenceNo)
        {
            this.SequenceNo = sequenceNo;
        }

        /// <summary>
        /// return SequenceNo
        /// </summary>
        /// <returns></returns>
        public Int32 GetSequenceNo()
        {
            return this.SequenceNo;
        }
        /// <summary>
        /// キャリブレータロット番号の設定
        /// </summary>
        /// <remarks>
        /// キャリブレータロット番号を設定します。
        /// </remarks>
        public void SetCalibLotNo(String calibLotNo)
        {
            this.CalibLotNo = calibLotNo;
        }
        /// <summary>
        /// 分析項目インデックスの取得
        /// </summary>
        /// <remarks>
        /// 分析項目インデックスを取得します。
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
        public void SetMeasureProtocolIndex(Int32 measureProtocolIndex)
        {
            this.MeasureProtocolIndex = measureProtocolIndex;
        }
        /// <summary>
        /// ステータスの取得
        /// </summary>
        /// <remarks>
        /// ステータスを取得します。
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
        public void SetStatus(SampleInfo.SampleMeasureStatus status)
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
        public void SetRemainTime(TimeSpan? remainTime)
        {
            this.RemainTime = remainTime;
        }
        /// <summary>
        /// カウント値の取得
        /// </summary>
        /// <remarks>
        /// カウント値を取得します。
        /// </remarks>
        /// <returns></returns>
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
        public void SetCount(Int32? count)
        {
            this.CountValue = count;
        }
        /// <summary>
        /// 濃度値の設定
        /// </summary>
        /// <remarks>
        /// 濃度値を設定いします。
        /// </remarks>
        public void SetConcentration(String concentration)
        {
            this.Concentration = concentration;
        }
        /// <summary>
        /// リマークIDの取得
        /// </summary>
        /// <remarks>
        /// リマークIDを取得します。
        /// </remarks>
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
        public void SetRemarkId(Remark remarkId)
        {
            this.RemarkId = remarkId;
        }
        /// <summary>
        /// 測定日時の設定
        /// </summary>
        /// <remarks>
        /// 測定日時を設定します。
        /// </remarks>
        public void SetMeasureDateTime(DateTime? measureDateTime)
        {
            this.MeasureDateTime = measureDateTime;
        }
        /// <summary>
        /// 試薬ロット番号の設定
        /// </summary>
        /// <remarks>
        /// 試薬ロット番号を設定します。
        /// </remarks>
        public void SetReagentLotNo(String reagentLotNo)
        {
            this.ReagentLotNo = reagentLotNo;
        }
        /// <summary>
        /// プレトリガロット番号の設定
        /// </summary>
        /// <remarks>
        /// プレトリガロット番号を設定します。
        /// </remarks>
        public void SetPretriggerLotNo(String pretriggerLotNo)
        {
            this.PretriggerLotNo = pretriggerLotNo;
        }
        /// <summary>
        /// トリガロット番号の設定
        /// </summary>
        /// <remarks>
        /// トリガロット番号を設定します。
        /// </remarks>
        public void SetTriggerLotNo(String triggerLotNo)
        {
            this.TriggerLotNo = triggerLotNo;
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
        /// 列キー
        /// </summary>
        /// <remarks>
        /// グリッド向け列キー設定用クラスです。
        /// </remarks>
        public struct DataKeys
        {
            /// <summary>
            /// レプリケーション番号
            /// </summary>
            public const String ReplicationNo = "ReplicationNo";
            /// <summary>
            /// ラックID
            /// </summary>
            public const String RackId = "RackId";
            /// <summary>
            /// ラックポジション
            /// </summary>
            public const String RackPosition = "RackPosition";
            /// <summary>
            /// シーケンス番号
            /// </summary>
            public const String SequenceNo = "SequenceNo";
            /// <summary>
            /// キャリブレータロット番号
            /// </summary>
            public const String CalibLotNo = "CalibLotNo";
            /// <summary>
            /// ステータス
            /// </summary>
            public const String StatusString = "StatusString";
            /// <summary>
            /// 残時間
            /// </summary>
            public const String RemainTime = "RemainTime";
            /// <summary>
            /// カウント値
            /// </summary>
            public const String Count = "Count";
            /// <summary>
            /// 濃度値
            /// </summary>
            public const String Concentration = "Concentration";
            /// <summary>
            /// 測定日時
            /// </summary>
            public const String MeasureDateTime = "MeasureDateTime";
            /// <summary>
            /// 試薬ロット番号
            /// </summary>
            public const String ReagentLotNo = "ReagentLotNo";
            /// <summary>
            /// プレトリガロット番号
            /// </summary>
            public const String PretriggerLotNo = "PretriggerLotNo";
            /// <summary>
            /// トリガロット番号
            /// </summary>
            public const String TriggerLotNo = "TriggerLotNo";
            public const String Analytes = "Analytes";
            public const String Remark = "Remark";
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
    /// キャリブレータ分析データDBクラス
    /// </summary>
    /// <remarks>
    /// 現在分析中の検体情報を保持します。
    /// 分析完了時、このテーブル内容は削除され、測定結果テーブルへ保存されます。
    /// </remarks>
    public class CalibratorAssayDB : DBAccessControl
    {
        #region [定数定義]

        /// <summary>
        /// 分析モジュール番号(DBテーブル：calibratorAssay列名)
        /// </summary>
        public const String STRING_MODULENO = "moduleNo";
        /// <summary>
        /// ユニーク番号(DBテーブル：calibratorAssay列名)
        /// </summary>
        public const String STRING_UNIQUENO = "uniqueNo";
        /// <summary>
        /// レプリケーション番号(DBテーブル：calibratorAssay列名)
        /// </summary>
        public const String STRING_REPLICATIONNO = "replicationNo";
        /// <summary>
        /// ラックID(DBテーブル：calibratorAssay列名)
        /// </summary>
        public const String STRING_RACKID = "rackId";
        /// <summary>
        /// ラックポジション(DBテーブル：calibratorAssay列名)
        /// </summary>
        public const String STRING_RACKPOSITION = "rackPosition";
        /// <summary>
        /// 検体識別番号(DBテーブル：calibratorAssay列名)
        /// </summary>
        public const String STRING_INDIVIDUALLYNO = "individuallyNo";
        /// <summary>
        /// シーケンス番号(DBテーブル：calibratorAssay列名)
        /// </summary>
        public const String STRING_SEQUENCENO = "sequenceNo";
        /// <summary>
        /// キャリブレータロット番号(DBテーブル：calibratorAssay列名)
        /// </summary>
        public const String STRING_CALIBLOTNO = "calibLotNo";
        /// <summary>
        /// 分析項目インデックス(DBテーブル：calibratorAssay列名)
        /// </summary>
        public const String STRING_MEASUREPROTOCOLINDEX = "measureProtocolIndex";
        /// <summary>
        /// ステータス(DBテーブル：calibratorAssay列名)
        /// </summary>
        public const String STRING_STATUS = "status";
        /// <summary>
        /// 残時間(DBテーブル：calibratorAssay列名)
        /// </summary>
        public const String STRING_REMAINTIME = "remainTime";
        /// <summary>
        /// カウント値(DBテーブル：calibratorAssay列名)
        /// </summary>
        public const String STRING_COUNT = "count";
        /// <summary>
        /// 濃度値(DBテーブル：calibratorAssay列名)
        /// </summary>
        public const String STRING_CONCENTRATION = "concentration";
        /// <summary>
        /// リマークID(DBテーブル：calibratorAssay列名)
        /// </summary>
        public const String STRING_REMARKID = "remarkId";
        /// <summary>
        /// 測定日時(DBテーブル：calibratorAssay列名)
        /// </summary>
        public const String STRING_MEASUREDATETIME = "measureDateTime";
        /// <summary>
        /// 試薬ロット番号(DBテーブル：calibratorAssay列名)
        /// </summary>
        public const String STRING_REAGENTLOTNO = "reagentLotNo";
        /// <summary>
        /// プレトリガロット番号(DBテーブル：calibratorAssay列名)
        /// </summary>
        public const String STRING_PRETRIGGERLOTNO = "pretriggerLotNo";
        /// <summary>
        /// トリガロット番号(DBテーブル：calibratorAssay列名)
        /// </summary>
        public const String STRING_TRIGGERLOTNO = "triggerLotNo";
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

        /// <summary>
        /// データテーブル取得SQL
        /// </summary>
        protected override String baseTableSelectSql
        {
            get
            {
                return "SELECT * FROM dbo.calibratorAssay";
            }
        }


        #endregion

        #region [publicメソッド]

        /// <summary>
        /// データ取得
        /// </summary>
        /// <remarks>
        /// ラックIDを条件に取得したデータを返します。
        /// </remarks>
        /// <param name="rack">絞り込みラックID</param>
        /// <returns>取得データ一覧</returns>
        public List<CalibratorAssayData> GetData(CarisXIDString rackID = null)
        {
            List<CalibratorAssayData> result = null;

            if (this.DataTable != null)
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    var datas = from data in dataTableList.AsParallel().Select((row) => new CalibratorAssayData(row))
                                where (rackID == null || (rackID != null && rackID.DispPreCharString == data.RackId.DispPreCharString))
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

            return result ?? new List<CalibratorAssayData>();
        }

        //
        public List<CalibratorAssayData> GetData(int nProtoclIndex, String strReagentLotNo, String strCaliLotNo)
        {
            List<CalibratorAssayData> result = null;

            if (this.DataTable != null)
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    var datas = from data in dataTableList.AsParallel().Select((row) => new CalibratorAssayData(row))
                                where (data.GetMeasureProtocolIndex() == nProtoclIndex && data.ReagentLotNo == strReagentLotNo && data.CalibLotNo == strCaliLotNo)
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

            return result ?? new List<CalibratorAssayData>();
        }
        public String GetAssayConcentration(int uniqueNo)
        {
            String strConcentration = String.Empty;

            if (this.DataTable != null)
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    var datas = from data in dataTableList.AsParallel().Select((row) => new CalibratorAssayData(row))
                                where (data.GetUniqueNo() == uniqueNo)
                                select data;
                    strConcentration = datas.First().GetConcentrationWithoutUnit();
                }
                catch (Exception ex)
                {
                    // DB内部に不正データ
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                }
            }

            return strConcentration;
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
        /// <param name="remain">経過時間</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean AssayStatusUpdate(Int32 uniqueNo, Int32 replicationNo, TimeSpan remain)
        {
            Boolean result = true;

            //　コピーデータリストを取得
            var dataTableList = this.DataTable.AsEnumerable().ToList();

            var changeData = dataTableList.AsParallel().Select((row) => new CalibratorAssayData(row))
                .FirstOrDefault((targetData) => (targetData.GetUniqueNo() == uniqueNo)
                                             && (targetData.ReplicationNo == replicationNo));
            if (changeData != null)
            {
                // ステータス更新
                // スレーブ側でリマーク発生の場合、試薬節約の為分析終了サイクルまで全て行わない可能性がある。
                // その場合測定データコマンドが来た後に分析ステータスコマンドが来ると、ステータス表示がInProcessのまま残ってしまう為、
                // ここでEndの場合ステータスを変更しない。
                if (changeData.GetStatus() != SampleInfo.SampleMeasureStatus.End)
                {
                    changeData.SetStatus(SampleInfo.SampleMeasureStatus.InProcess);
                }

                var sample = Singleton<InProcessSampleInfoManager>.Instance.SearchInProcessSampleFromUniqueNo(uniqueNo);

                // 残時間更新
                changeData.SetRemainTime(remain);

            }
            else
            {
                System.Diagnostics.Debug.WriteLine("剩余时间更新数据库数据失败");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, "更新数据库的数据不存在");
            }
            return result;
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
        public Boolean SetResultData(CalcData data, IMeasureResultData measureresult)
        {
            Boolean result = false;

            try
            {
                // 分析データ設定
                var targetAssayData = this.GetData(data.RackID).Single((targetData) =>
                {
                    return (targetData.GetUniqueNo() == data.UniqueNo && targetData.ReplicationNo == data.ReplicationNo);
                });

                Int32 digits = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(data.ProtocolIndex).LengthAfterDemPoint;

                targetAssayData.SetDarkCount(measureresult.DarkCount);
                targetAssayData.SetBGCount(measureresult.BGCount);
                targetAssayData.SetResultCount(measureresult.ResultCount);

                if (data.CalcInfoReplication.Concentration.HasValue)
                {
                    targetAssayData.SetCount(data.CalcInfoReplication.CountValue);
                }
                if (data.CalcInfoReplication.Concentration.HasValue)
                {
                    targetAssayData.SetConcentration(SubFunction.ToRoundOffParse(data.CalcInfoReplication.Concentration.Value, digits));
                }
                targetAssayData.SetRemainTime(TimeSpan.FromTicks(0));
                targetAssayData.SetStatus(data.CalcInfoReplication.Remark.IsNeedReMeasure ? SampleInfo.SampleMeasureStatus.Error : SampleInfo.SampleMeasureStatus.End);

                // 時刻取得元の統一化
                targetAssayData.SetMeasureDateTime(data.MeasureDateTime);
                targetAssayData.SetRemarkId(data.CalcInfoReplication.Remark);

                targetAssayData.SetReagentLotNo(data.ReagentLotNo);

                this.SetData(new List<CalibratorAssayData>() { targetAssayData });
                result = true;
            }
            catch (Exception ex)
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
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
        public Boolean AddAssayData(IMeasureIndicate data, out Int32 calibSequenceNo, String strConcentration = null)
        {

            Boolean result = true;
            Int32 createCalibSequenceTimes = 0;
            calibSequenceNo = 0;

            // シーケンス番号生成時、同一のキャリブレータ測定か判定する
            var findGroupe = HybridDataMediator.SearchInprocessCalibRegistDataFromRackIdPos(data.ModuleID, data.RackID, data.SamplePosition).ToList();

            if (findGroupe.Count() == 0)
            {
                if (data.MeasItemCount != 0)
                {
                    var measItem = data.MeasItemArray[0];
                    var calibdataList = HybridDataMediator.SearchCalibDataFromReagentLotCalibLotNo(measItem.ProtoNo, measItem.ReagentLotNo, data.SampleID);

                    //修正了在校准品取样完取出校准品架子当前实验结果未出前SequeceNO重复的问题
                    MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(measItem.ProtoNo);

                    if (calibdataList.Count() != 0)//可以增加校准点总个数的判断？？？
                    {
                        var processlist = Singleton<InProcessSampleInfoManager>.Instance.SearchInProcessSampleFromIndividuallyNumber(calibdataList.Last().GetIndividuallyNo());
                        if (processlist != null && processlist.Count != 0)//存在有校准品，且是同一个IndividuallyNo（同个位置）
                        {
                            calibSequenceNo = processlist.First().SequenceNumber;
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, String.Format("1 calibSequenceNo:{0}", calibSequenceNo));
                        }
                        else
                        {
                            /* シーケンス番号（キャリブレータ）生成 */
                            if (createCalibSequenceTimes < 1)
                            {
                                calibSequenceNo = Singleton<SequencialCalibNo>.Instance.CreateNumber();
                                createCalibSequenceTimes++;

                                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                      CarisXLogInfoBaseExtention.Empty, String.Format("2 calibSequenceNo:{0}", calibSequenceNo));

                            }
                        }
                    }
                    else
                    {
                        /* シーケンス番号（キャリブレータ）生成 */
                        if (createCalibSequenceTimes < 1)
                        {
                            calibSequenceNo = Singleton<SequencialCalibNo>.Instance.CreateNumber();
                            createCalibSequenceTimes++;

                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                             CarisXLogInfoBaseExtention.Empty, String.Format("3 calibSequenceNo:{0}", calibSequenceNo));
                        }
                    }

                    //修正了在校准品取样完取出校准品架子当前实验结果未出前SequeceNO重复的问题，                   
                    int nCountOfMasterCurf = protocol.NumOfMeasPointInCalib * protocol.RepNoForCalib;
                    //使用RackPosition排序会造成有时后最后一个校准点为前一个校准点
                    var groupCalibdataList = from v in HybridDataMediator.SearchCalibDataFromReagentLotCalibLotNo(measItem.ProtoNo, measItem.ReagentLotNo, data.SampleID)
                                             orderby v.SequenceNo
                                             group v by v.SequenceNo;

                    if (groupCalibdataList.Count() != 0)//
                    {
                        var lastGroup = groupCalibdataList.Last();//找到最后一组，判断最后一组是否是满的，不满，用原的SequencNO， 满，增加新的号码
                        if (lastGroup.Count() != 0 && lastGroup.Count() < nCountOfMasterCurf)
                        {
                            calibSequenceNo = lastGroup.First().SequenceNo;
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                         CarisXLogInfoBaseExtention.Empty, String.Format("4 calibSequenceNo:{0}", calibSequenceNo));
                        }
                        else
                        {
                            if (createCalibSequenceTimes < 1)
                            {
                                calibSequenceNo = Singleton<SequencialCalibNo>.Instance.CreateNumber();
                                createCalibSequenceTimes++;
                                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                             CarisXLogInfoBaseExtention.Empty, String.Format("5 calibSequenceNo:{0}", calibSequenceNo));
                            }
                        }
                    }
                    //may be no need this check ,but it is OK 
                    if (calibSequenceNo == 0)
                    {
                        if (createCalibSequenceTimes < 1)
                        {
                            calibSequenceNo = Singleton<SequencialCalibNo>.Instance.CreateNumber();
                            createCalibSequenceTimes++;
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                             CarisXLogInfoBaseExtention.Empty, String.Format("6 calibSequenceNo:{0}", calibSequenceNo));
                        }
                    }
                }
                else
                {
                    /* シーケンス番号（キャリブレータ）生成 */
                    if (createCalibSequenceTimes < 1)
                    {
                        calibSequenceNo = Singleton<SequencialCalibNo>.Instance.CreateNumber();
                        createCalibSequenceTimes++;
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                             CarisXLogInfoBaseExtention.Empty, String.Format("7 calibSequenceNo:{0}", calibSequenceNo));
                    }
                }
            }
            else
            {
                // シーケンス番号（キャリブレータ）取得
                calibSequenceNo = findGroupe.First().SequenceNumber;
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                         CarisXLogInfoBaseExtention.Empty, String.Format("8 calibSequenceNo:{0}", calibSequenceNo));
            }

            try
            {
                if (this.DataTable != null)
                {
                    String preTriggerLot = HybridDataMediator.SearchPreTriggerLotNoFromReagentDB();
                    String triggerLot = HybridDataMediator.SearchTriggerLotNoFromReagentDB();

                    for (int iLoop = 0; iLoop < data.MeasItemCount; iLoop++)
                    {
                        var measItem = data.MeasItemArray[iLoop];
                        var measProto = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(measItem.ProtoNo);

                        for (int iRepNumber = 1; iRepNumber <= measItem.RepCount; iRepNumber++)
                        {
                            DataRow addRow = this.DataTable.NewRow();

                            addRow[CalibratorAssayDB.STRING_CALIBLOTNO] = data.SampleID;
                            if (String.IsNullOrEmpty(strConcentration))
                            {
                                addRow[CalibratorAssayDB.STRING_CONCENTRATION] = String.Empty;
                            }
                            else
                            {
                                Int32 digits = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(measProto.ProtocolIndex).LengthAfterDemPoint;
                                double dConcentration = double.Parse(strConcentration);
                                addRow[CalibratorAssayDB.STRING_CONCENTRATION] = SubFunction.ToRoundOffParse(dConcentration, digits);
                            }

                            addRow[CalibratorAssayDB.STRING_COUNT] = DBNull.Value;
                            addRow[CalibratorAssayDB.STRING_DARKCOUNT] = DBNull.Value;
                            addRow[CalibratorAssayDB.STRING_BGCOUNT] = DBNull.Value;
                            addRow[CalibratorAssayDB.STRING_RESULTCOUNT] = DBNull.Value;
                            addRow[CalibratorAssayDB.STRING_INDIVIDUALLYNO] = data.IndividuallyNumber;
                            addRow[CalibratorAssayDB.STRING_MEASUREDATETIME] = DBNull.Value;
                            addRow[CalibratorAssayDB.STRING_MEASUREPROTOCOLINDEX] = measProto.ProtocolIndex;
                            addRow[CalibratorAssayDB.STRING_RACKID] = data.RackID;
                            addRow[CalibratorAssayDB.STRING_RACKPOSITION] = data.SamplePosition;

                            // 残り時間(サイクル回数*サイクル時間)
                            addRow[CalibratorAssayDB.STRING_REMAINTIME] = HybridDataMediator.GetRemainTime().Ticks;

                            addRow[CalibratorAssayDB.STRING_REMARKID] = Remark.REMARK_DEFAULT;
                            addRow[CalibratorAssayDB.STRING_REPLICATIONNO] = iRepNumber;
                            addRow[CalibratorAssayDB.STRING_SEQUENCENO] = calibSequenceNo;
                            addRow[CalibratorAssayDB.STRING_STATUS] = (Int32)SampleInfo.SampleMeasureStatus.Wait;
                            // プレトリガロット番号
                            addRow[CalibratorAssayDB.STRING_PRETRIGGERLOTNO] = preTriggerLot;
                            // トリガロット番号
                            addRow[CalibratorAssayDB.STRING_TRIGGERLOTNO] = triggerLot;
                            // 試薬ロット番号
                            addRow[CalibratorAssayDB.STRING_REAGENTLOTNO] = measItem.ReagentLotNo;
                            addRow[CalibratorAssayDB.STRING_UNIQUENO] = measItem.UniqNo;
                            // 分析モジュール番号
                            addRow[CalibratorAssayDB.STRING_MODULENO] = data.ModuleID;

                            this.DataTable.Rows.Add(addRow);
                        }
                    }

                    result = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("添加数据失败:{0}", ex.Message));
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, String.Format("添加数据失败:{0}", ex.StackTrace));
                result = false;
            }

            return result;
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

            if (this.DataTable != null)
            {
                // 全行削除
                foreach (var row in this.DataTable.AsEnumerable())
                {
                    row.Delete();
                }
                reuslt = true;
            }

            return reuslt;
        }

        /// <summary>
        /// キャリブレータ分析中データの設定
        /// </summary>
        /// <remarks>
        /// キャリブレータ分析中データの同期を行います。
        /// </remarks>
        /// <param name="list">変更、削除操作済みデータ</param>
        public void SetData(List<CalibratorAssayData> list)
        {
            list.SyncDataListToDataTable(this.DataTable);
        }

        /// <summary>
        /// キャリブレータ分析中情報テーブル取得
        /// </summary>
        /// <remarks>
        /// キャリブレータ分析中情報をDBから読込みます。
        /// </remarks>
        public override void LoadDB()
        {
            this.fillBaseTable();
        }

        /// <summary>
        /// 分析中DB全消去
        /// </summary>
        /// <remarks>
        /// 分析DBに残った分析中・待機の検体情報を削除します。
        /// </remarks>
        public void ClearWaitAndInprocess()
        {
            if (this.DataTable != null)
            {
                //　コピーデータリストを取得
                var dataTableList = this.DataTable.AsEnumerable().ToList();

                var deleteData = dataTableList.Select((row) => new SpecimenAssayData(row))
                    .Where((data) => (data.GetStatus() == SampleInfo.SampleMeasureStatus.Wait)
                                  || (data.GetStatus() == SampleInfo.SampleMeasureStatus.InProcess)).ToList();

                deleteData.DeleteAllDataList();
                deleteData.SyncDataListToDataTable(this.DataTable);
            }
        }

        /// <summary>
        /// キャリブレータ分析中情報テーブル書込み
        /// </summary>
        /// <remarks>
        /// キャリブレータ分析中情報をDBに書き込みます。
        /// </remarks>
        public void CommitData()
        {
            this.updateBaseTable();
        }

        #endregion

    }
}
