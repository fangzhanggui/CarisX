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
    /// 精度管理分析データクラス
    /// </summary>
    /// <remarks>
    /// 精度管理分析データクラスです。
    /// </remarks>
    public class ControlAssayData : DataRowWrapperBase
    {
        #region [コンストラクタ/デストラクタ]
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data"></param>
        public ControlAssayData( DataRowWrapperBase data )
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
                return this.Field<Int32>( ControlAssayDB.STRING_UNIQUENO );
            }
            set
            {
                this.SetField<Int32>( ControlAssayDB.STRING_UNIQUENO, value );
            }
        }
        /// <summary>
        /// レプリケーション番号の取得、設定
        /// </summary>
        public Int32 ReplicationNo
        {
            get
            {
                return this.Field<Int32>( ControlAssayDB.STRING_REPLICATIONNO );
            }
            protected set
            {
                this.SetField<Int32>( ControlAssayDB.STRING_REPLICATIONNO, value );
            }
        }
        /// <summary>
        /// ラックポジションの取得、設定
        /// </summary>
        public Int32 RackPosition
        {
            get
            {
                return this.Field<Int32>( ControlAssayDB.STRING_RACKPOSITION );
            }
            protected set
            {
                this.SetField<Int32>( ControlAssayDB.STRING_RACKPOSITION, value );
            }
        }
        /// <summary>
        /// 検体識別番号の取得、設定
        /// </summary>
        protected Int32 IndividuallyNo
        {
            get
            {
                return this.Field<Int32>( ControlAssayDB.STRING_INDIVIDUALLYNO );
            }
            set
            {
                this.SetField<Int32>( ControlAssayDB.STRING_INDIVIDUALLYNO, value );
            }
        }
        /// <summary>
        /// シーケンス番号の取得、設定
        /// </summary>
        public Int32 SequenceNo
        {
            get
            {
                return this.Field<Int32>( ControlAssayDB.STRING_SEQUENCENO );
            }
            protected set
            {
                this.SetField<Int32>( ControlAssayDB.STRING_SEQUENCENO, value );
            }
        }
        /// <summary>
        /// 精度管理検体ロット番号の取得、設定
        /// </summary>
        public String ControlLotNo
        {
            get
            {
                return this.Field<String>( ControlAssayDB.STRING_CONTROLLOTNO );
            }
            protected set
            {
                this.SetField<String>( ControlAssayDB.STRING_CONTROLLOTNO, value );
            }
        }
        /// <summary>
        /// 精度管理検体名の取得、設定
        /// </summary>
        public String ControlName
        {
            get
            {
                return this.Field<String>( ControlAssayDB.STRING_CONTROLNAME );
            }
            protected set
            {
                this.SetField<String>( ControlAssayDB.STRING_CONTROLNAME, value );
            }
        }
        /// <summary>
        /// 分析項目インデックスの取得、設定
        /// </summary>
        protected Int32 MeasureProtocolIndex
        {
            get
            {
                return this.Field<Int32>( ControlAssayDB.STRING_MEASUREPROTOCOLINDEX );
            }
            set
            {
                this.SetField<Int32>( ControlAssayDB.STRING_MEASUREPROTOCOLINDEX, value );
            }
        }
        /// <summary>
        /// ステータスの取得、設定
        /// </summary>
        protected SampleInfo.SampleMeasureStatus Status
        {
            get
            {
                return (SampleInfo.SampleMeasureStatus)this.Field<Int32>( ControlAssayDB.STRING_STATUS );
            }
            set
            {
                this.SetField<Int32>( ControlAssayDB.STRING_STATUS, (Int32)value );
            }
        }
        /// <summary>
        /// 残時間の取得、設定
        /// </summary>
        public TimeSpan? RemainTime
        {
            get
            {
                Int64? field = this.Field<Int64?>( ControlAssayDB.STRING_REMAINTIME );
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
                    this.SetField<Int64?>( ControlAssayDB.STRING_REMAINTIME, value.Value.Ticks );
                }
                else
                {
                    this.SetField<Int64?>( ControlAssayDB.STRING_REMAINTIME, null );
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
                return this.Field<Int32?>( ControlAssayDB.STRING_COUNT );
            }
            set
            {
                this.SetField<Int32?>( ControlAssayDB.STRING_COUNT, value );
            }
        }
        /// <summary>
        /// 濃度値の取得、設定
        /// </summary>
        public String Concentration
        {
            get
            {
                var conc = this.Field<String>( ControlAssayDB.STRING_CONCENTRATION );
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
                this.SetField<String>( ControlAssayDB.STRING_CONCENTRATION, value );
            }
        }

        /// <summary>
        /// リマークIDの取得、設定
        /// </summary>
        protected Remark RemarkId
        {
            get
            {
                return this.Field<Int64?>( ControlAssayDB.STRING_REMARKID );
            }
            set
            {
                this.SetField<Int64?>( ControlAssayDB.STRING_REMARKID, value );
            }
        }

        /// <summary>
        /// 測定日時の取得、設定
        /// </summary>
        public DateTime? MeasureDateTime
        {
            get
            {
                return this.Field<DateTime?>( ControlAssayDB.STRING_MEASUREDATETIME );
            }
            protected set
            {
                this.SetField<DateTime?>( ControlAssayDB.STRING_MEASUREDATETIME, value );
            }
        }

        /// <summary>
        /// 試薬ロット番号の取得、設定
        /// </summary>
        public String ReagentLotNo
        {
            get
            {
                return this.Field<String>( ControlAssayDB.STRING_REAGENTLOTNO );
            }
            protected set
            {
                this.SetField<String>( ControlAssayDB.STRING_REAGENTLOTNO, value );
            }
        }

        /// <summary>
        /// プレトリガロット番号の取得、設定
        /// </summary>
        public String PretriggerLotNo
        {
            get
            {
                return this.Field<String>( ControlAssayDB.STRING_PRETRIGGERLOTNO );
            }
            protected set
            {
                this.SetField<String>( ControlAssayDB.STRING_PRETRIGGERLOTNO, value );
            }
        }

        /// <summary>
        /// トリガロット番号の取得、設定
        /// </summary>
        public String TriggerLotNo
        {
            get
            {
                return this.Field<String>( ControlAssayDB.STRING_TRIGGERLOTNO );
            }
            protected set
            {
                this.SetField<String>( ControlAssayDB.STRING_TRIGGERLOTNO, value );
            }
        }

        /// <summary>
        /// 使用検量線の取得、設定
        /// </summary>
        public DateTime? CalibCurveDateTime
        {
            get
            {
                return this.Field<DateTime?>( ControlAssayDB.STRING_CALIBCURVEDATETIME );
            }
            protected set
            {
                this.SetField<DateTime?>( ControlAssayDB.STRING_CALIBCURVEDATETIME, value );
            }
        }

        /// <summary>
        /// コメントの取得、設定
        /// </summary>
        public String Comment
        {
            get
            {
                return this.Field<String>( ControlAssayDB.STRING_COMMENT );
            }
            protected set
            {
                this.SetField<String>( ControlAssayDB.STRING_COMMENT, value );
            }
        }

        /// <summary>
        /// 分析項目名の取得
        /// </summary>
        public String Analytes
        {
            get
            {
                return Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( this.MeasureProtocolIndex ).ProtocolName;
            }
        }

        /// <summary>
        /// ラックIDの取得、設定
        /// </summary>
        public CarisXIDString RackId
        {
            get
            {
                return this.Field<String>( ControlAssayDB.STRING_RACKID );
            }
            protected set
            {
                this.SetField<String>( ControlAssayDB.STRING_RACKID, value.DispPreCharString );
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
                return this.Field<Int32?>(ControlAssayDB.STRING_DARKCOUNT);
            }
            set
            {
                this.SetField<Int32?>(ControlAssayDB.STRING_DARKCOUNT, value);
            }
        }

        /// <summary>
        /// バックグラウンドカウント値の取得、設定
        /// </summary>
        public Int32? BGCount
        {
            get
            {
                return this.Field<Int32?>(ControlAssayDB.STRING_BGCOUNT);
            }
            set
            {
                this.SetField<Int32?>(ControlAssayDB.STRING_BGCOUNT, value);
            }
        }

        /// <summary>
        /// 測定カウント値の取得、設定
        /// </summary>
        public Int32? ResultCount
        {
            get
            {
                return this.Field<Int32?>(ControlAssayDB.STRING_RESULTCOUNT);
            }
            set
            {
                this.SetField<Int32?>(ControlAssayDB.STRING_RESULTCOUNT, value);
            }
        }

        /// <summary>
        /// 分析モジュール番号の取得、設定
        /// </summary>
        public Int32 ModuleNo
        {
            get
            {
                return this.Field<Int32>(ControlAssayDB.STRING_MODULENO);
            }
            set
            {
                this.SetField<Int32>(ControlAssayDB.STRING_MODULENO, value);
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
        /// ユニーク番号の設定
        /// </summary>
        /// <remarks>
        /// ユニーク番号を設定します。
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
        /// レプリケーション番号を設定します。
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
        public void SetRackPosition( Int32 rackPosition )
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
        public void SetInidividuallyNo( Int32 inidividuallyNo )
        {
            this.IndividuallyNo = inidividuallyNo;
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
        /// 精度管理検体ロット番号の設定
        /// </summary>
        /// <remarks>
        /// 精度管理検体ロット番号を設定します。
        /// </remarks>
        public void SetControlLotNo( String controlLotNo )
        {
            this.ControlLotNo = controlLotNo;
        }

        /// <summary>
        /// 精度管理検体名の設定
        /// </summary>
        /// <remarks>
        /// 精度管理検体名を設定します。
        /// </remarks>
        public void SetControlName( String controlName )
        {
            this.ControlName = controlName;
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
        public void SetMeasureProtocolIndex( Int32 measureProtocolIndex )
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
        public void SetConcentration( String concentration )
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
        public void SetMeasureDateTime( DateTime? measureDateTime )
        {
            this.MeasureDateTime = measureDateTime;
        }

        /// <summary>
        /// 試薬ロット番号の設定
        /// </summary>
        /// <remarks>
        /// 試薬ロット番号を設定します。
        /// </remarks>
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
        public void SetTriggerLotNo( String triggerLotNo )
        {
            this.TriggerLotNo = triggerLotNo;
        }

        /// <summary>
        /// 使用検量線の設定
        /// </summary>
        /// <remarks>
        /// 使用検量線を設定します。
        /// </remarks>
        public void SetCalibCurveDateTime( DateTime? calibCurveDateTime )
        {
            this.CalibCurveDateTime = calibCurveDateTime;
        }

        /// <summary>
        /// コメントの設定
        /// </summary>
        /// <remarks>
        /// コメントを設定します。
        /// </remarks>
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
            /// 精度管理検体ロット番号
            /// </summary>
            public const String ControlLotNo = "ControlLotNo";
            /// <summary>
            /// 精度管理検体名
            /// </summary>
            public const String ControlName = "ControlName";
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
            /// <summary>
            /// 使用検量線
            /// </summary>
            public const String CalibCurveDateTime = "CalibCurveDateTime";
            /// <summary>
            /// コメント
            /// </summary>
            public const String Comment = "Comment";
            /// <summary>
            /// 分析項目名
            /// </summary>
            public const String Analytes = "Analytes";
            /// <summary>
            /// リマーク文字列
            /// </summary>
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
    /// 精度管理分析データDBクラス
    /// </summary>
    /// <remarks>
    /// 現在分析中の検体情報を保持します。
    /// 分析完了時、このテーブル内容は削除され、測定結果テーブルへ保存されます。
    /// </remarks>
    public class ControlAssayDB : DBAccessControl
    {
        #region [定数定義]

        /// <summary>
        /// 分析モジュール番号(DBテーブル：controlAssay列名)
        /// </summary>
        public const String STRING_MODULENO = "moduleNo";
        /// <summary>
        /// ユニーク番号(DBテーブル：controlAssay列名)
        /// </summary>
        public const String STRING_UNIQUENO = "uniqueNo";
        /// <summary>
        /// レプリケーション番号(DBテーブル：controlAssay列名)
        /// </summary>
        public const String STRING_REPLICATIONNO = "replicationNo";
        /// <summary>
        /// ラックID(DBテーブル：controlAssay列名)
        /// </summary>
        public const String STRING_RACKID = "rackId";
        /// <summary>
        /// ラックポジション(DBテーブル：controlAssay列名)
        /// </summary>
        public const String STRING_RACKPOSITION = "rackPosition";
        /// <summary>
        /// 検体識別番号(DBテーブル：controlAssay列名)
        /// </summary>
        public const String STRING_INDIVIDUALLYNO = "individuallyNo";
        /// <summary>
        /// シーケンス番号(DBテーブル：controlAssay列名)
        /// </summary>
        public const String STRING_SEQUENCENO = "sequenceNo";
        /// <summary>
        /// 精度管理検体ロット番号(DBテーブル：controlAssay列名)
        /// </summary>
        public const String STRING_CONTROLLOTNO = "controlLotNo";
        /// <summary>
        /// 精度管理検体名(DBテーブル：controlAssay列名)
        /// </summary>
        public const String STRING_CONTROLNAME = "controlName";
        /// <summary>
        /// 分析項目インデックス(DBテーブル：controlAssay列名)
        /// </summary>
        public const String STRING_MEASUREPROTOCOLINDEX = "measureProtocolIndex";
        /// <summary>
        /// ステータス(DBテーブル：controlAssay列名)
        /// </summary>
        public const String STRING_STATUS = "status";
        /// <summary>
        /// 残時間(DBテーブル：controlAssay列名)
        /// </summary>
        public const String STRING_REMAINTIME = "remainTime";
        /// <summary>
        /// カウント値(DBテーブル：controlAssay列名)
        /// </summary>
        public const String STRING_COUNT = "count";
        /// <summary>
        /// 濃度値(DBテーブル：controlAssay列名)
        /// </summary>
        public const String STRING_CONCENTRATION = "concentration";
        /// <summary>
        /// リマークID(DBテーブル：controlAssay列名)
        /// </summary>
        public const String STRING_REMARKID = "remarkId";
        /// <summary>
        /// 測定日時(DBテーブル：controlAssay列名)
        /// </summary>
        public const String STRING_MEASUREDATETIME = "measureDateTime";
        /// <summary>
        /// 試薬ロット番号(DBテーブル：controlAssay列名)
        /// </summary>
        public const String STRING_REAGENTLOTNO = "reagentLotNo";
        /// <summary>
        /// プレトリガロット番号(DBテーブル：controlAssay列名)
        /// </summary>
        public const String STRING_PRETRIGGERLOTNO = "pretriggerLotNo";
        /// <summary>
        /// トリガロット番号(DBテーブル：controlAssay列名)
        /// </summary>
        public const String STRING_TRIGGERLOTNO = "triggerLotNo";
        /// <summary>
        /// 使用検量線(DBテーブル：controlAssay列名)
        /// </summary>
        public const String STRING_CALIBCURVEDATETIME = "calibCurveDateTime";
        /// <summary>
        /// コメント(DBテーブル：controlAssay列名)
        /// </summary>
        public const String STRING_COMMENT = "comment";
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
                return "SELECT * FROM dbo.controlAssay";
            }
        }
        
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 精度管理分析データ取得
        /// </summary>
        /// <remarks>
        /// 精度管理分析データを取得します。
        /// </remarks>
        /// <param name="rack"></param>
        /// <returns>精度管理分析データ</returns>
        public List<ControlAssayData> GetData( CarisXIDString rackID = null )
        {
            List<ControlAssayData> result = null;

            if ( this.DataTable != null )
            {
                try
                {
                    var datas = from data in this.DataTable.Copy().AsEnumerable().AsParallel().Select( ( row ) => new ControlAssayData( row ) )
                                where ( rackID == null || ( rackID != null && rackID.DispPreCharString == data.RackId.DispPreCharString ) )
                                select data;

                    result = datas.ToList();
                }
                catch ( Exception ex )
                {
                    // DB内部に不正データ
                    //Singleton<LogManager>.Instance.Error( ex.Message );
                    Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
                }
            }

            return result ?? new List<ControlAssayData>();
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

                targetAssayData.SetCount( data.CalcInfoReplication.CountValue );
                Int32 digits = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( data.ProtocolIndex ).LengthAfterDemPoint;
                if ( data.CalcInfoReplication.Concentration.HasValue )
                {
                    targetAssayData.SetConcentration( SubFunction.ToRoundOffParse( data.CalcInfoReplication.Concentration.Value, digits ) );
                }
                else
                {
                    targetAssayData.SetConcentration( null );
                }
                targetAssayData.SetRemainTime( TimeSpan.FromTicks( 0 ) );
                targetAssayData.SetStatus( data.CalcInfoReplication.Remark.IsNeedReMeasure ? SampleInfo.SampleMeasureStatus.Error : SampleInfo.SampleMeasureStatus.End );


				// 時刻取得元の統一化
				targetAssayData.SetMeasureDateTime( data.MeasureDateTime );

                targetAssayData.SetRemarkId( data.CalcInfoReplication.Remark );

                //同じキャリブレータとサンプルが同時にテストされたときの時間の同期を防止します。
                targetAssayData.SetCalibCurveDateTime(data.UseCalcCalibCurveApprovalDate);

                targetAssayData.SetReagentLotNo( data.ReagentLotNo );

                this.SetData( new List<ControlAssayData>() { targetAssayData } );

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
        /// <param name="remain">経過時間</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean AssayStatusUpdate( Int32 uniqueNo, Int32 replicationNo, TimeSpan remain )
        {
            Boolean result = true;
            //var changeData = this.GetData().FirstOrDefault( ( targetData ) => targetData.GetUniqueNo() == uniqueNo && targetData.ReplicationNo == replicationNo );
            var changeData = this.DataTable.AsEnumerable().AsParallel().Select( ( row ) => new ControlAssayData( row ) ).FirstOrDefault( ( targetData ) => targetData.GetUniqueNo() == uniqueNo && targetData.ReplicationNo == replicationNo );
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

                //this.SetData( new List<ControlAssayData>() { changeData } );
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
        /// 分析データ追加
        /// </summary>
        /// <remarks>
        /// 検体の分析データ追加を行います。
        /// 分析データの追加は測定指示問合せのタイミングとなります。
        /// </remarks>
        /// <param name="data">登録データ</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean AddAssayData( IMeasureIndicate data )
        {

            Boolean result = true;
            /* シーケンス番号（精度管理検体）生成 */
            Int32 sequenceNumber = Singleton<SequencialControlNo>.Instance.CreateNumber();
            
            try
            {
                if ( this.DataTable != null )
                {
                    String comment = HybridDataMediator.SearchCommentFromControlRegistDB( data.RackID, data.SamplePosition );
                    String preTriggerLot = HybridDataMediator.SearchPreTriggerLotNoFromReagentDB();
                    String triggerLot = HybridDataMediator.SearchTriggerLotNoFromReagentDB();

                    String controlName = HybridDataMediator.SearchControlNameFromControlDB( data.SampleID,data.RackID,data.SamplePosition );

                    for ( int iLoop = 0; iLoop < data.MeasItemCount; iLoop++ )
                    {
                        var measItem = data.MeasItemArray[iLoop];
                        var measProto = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo( measItem.ProtoNo );
                        DateTime approvalDate = HybridDataMediator.SearchCalibDateFromCalibDB( measProto.ProtocolIndex, data.ModuleID );
                        for ( int iRepNumber = 1; iRepNumber <= measItem.RepCount; iRepNumber++ )
                        {

                            DataRow addRow = this.DataTable.NewRow();

                            addRow[ControlAssayDB.STRING_CONCENTRATION] = String.Empty;
                            addRow[ControlAssayDB.STRING_COUNT] = DBNull.Value;
                            addRow[ControlAssayDB.STRING_DARKCOUNT] = DBNull.Value;
                            addRow[ControlAssayDB.STRING_BGCOUNT] = DBNull.Value;
                            addRow[ControlAssayDB.STRING_RESULTCOUNT] = DBNull.Value;
                            addRow[ControlAssayDB.STRING_INDIVIDUALLYNO] = data.IndividuallyNumber;
                            addRow[ControlAssayDB.STRING_MEASUREDATETIME] = DBNull.Value;
                            addRow[ControlAssayDB.STRING_MEASUREPROTOCOLINDEX] = measProto.ProtocolIndex;
                            addRow[ControlAssayDB.STRING_RACKID] = data.RackID;
                            addRow[ControlAssayDB.STRING_RACKPOSITION] = data.SamplePosition;
                            addRow[ControlAssayDB.STRING_REAGENTLOTNO] = measItem.ReagentLotNo;

                            addRow[ControlAssayDB.STRING_CONTROLNAME] = controlName;
                            addRow[ControlAssayDB.STRING_CONTROLLOTNO] = data.SampleID;

                            // 残り時間(サイクル回数*サイクル時間)
                            addRow[ControlAssayDB.STRING_REMAINTIME] = HybridDataMediator.GetRemainTime().Ticks;

                            addRow[ControlAssayDB.STRING_REMARKID] = Remark.REMARK_DEFAULT;
                            addRow[ControlAssayDB.STRING_REPLICATIONNO] = iRepNumber;
                            addRow[ControlAssayDB.STRING_SEQUENCENO] = sequenceNumber;
                            addRow[ControlAssayDB.STRING_STATUS] = (Int32)SampleInfo.SampleMeasureStatus.Wait;
                            // プレトリガロット番号
                            addRow[ControlAssayDB.STRING_PRETRIGGERLOTNO] = preTriggerLot;
                            // トリガロット番号
                            addRow[ControlAssayDB.STRING_TRIGGERLOTNO] = triggerLot;

                            // 現ロット使用の場合分析DB登録段階では表示しない。
                            if ( !measItem.UseCurrentLot )
                            {
                                // 試薬ロット番号
                                addRow[ControlAssayDB.STRING_REAGENTLOTNO] = measItem.ReagentLotNo;
                            }
                            else
                            {
                                addRow[ControlAssayDB.STRING_REAGENTLOTNO] = String.Empty;
                            }
                            // COMMENT
                            addRow[ControlAssayDB.STRING_COMMENT] = comment;
                            // 検量線日付
                            addRow[ControlAssayDB.STRING_CALIBCURVEDATETIME] = approvalDate;
                            addRow[ControlAssayDB.STRING_UNIQUENO] = measItem.UniqNo;
                            addRow[ControlAssayDB.STRING_MODULENO] = data.ModuleID;


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
        /// 精度管理検体分析中データの設定
        /// </summary>
        /// <remarks>
        /// 精度管理検体分析中データの同期を行います。
        /// </remarks>
        /// <param name="list">変更、削除操作済みデータ</param>
        public void SetData( List<ControlAssayData> list )
        {
            list.SyncDataListToDataTable( this.DataTable );
        }

        /// <summary>
        /// 精度管理検体分析中情報テーブル取得
        /// </summary>
        /// <remarks>
        /// 精度管理検体分析中情報をDBから読込みます。
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
            if ( this.DataTable != null )
            {
                var deleteData = this.DataTable.AsEnumerable().Select( ( row ) => new SpecimenAssayData( row ) ).
                    Where( ( data ) => data.GetStatus() == SampleInfo.SampleMeasureStatus.Wait || data.GetStatus() == SampleInfo.SampleMeasureStatus.InProcess ).
                    ToList();
                deleteData.DeleteAllDataList();
                deleteData.SyncDataListToDataTable( this.DataTable );
            }
        }

        /// <summary>
        /// 精度管理検体分析中情報テーブル書込み
        /// </summary>
        /// <remarks>
        /// 精度管理検体分析中情報をDBに書き込みます。
        /// </remarks>
        public void CommitData()
        {
            this.updateBaseTable();
        }

        #endregion

    }
}
