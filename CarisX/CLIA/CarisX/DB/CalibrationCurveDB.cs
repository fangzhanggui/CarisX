using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.DB;

using Oelco.Common.Utility;
using Oelco.Common.Log;
using System.Data;
using Oelco.Common.Calculator;
using System.Globalization;
using Oelco.Common.GUI;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Log;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.DB
{
    /// <summary>
    /// キャリブレータ検量線情報データクラス
    /// </summary>
    /// <remarks>
    /// キャリブレータ検量線情報データクラスです。
    /// </remarks>
    public class CalibrationCurveData : DataRowWrapperBase
    {

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data"></param>
        public CalibrationCurveData( DataRowWrapperBase data )
            : base( data )
        {
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 分析項目
        /// </summary>
        public String Analytes
        {
            get
            {
                return Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( this.Field<Int32>( CalibrationCurveDB.STRING_MEASUREPROTOCOLINDEX ) ).ProtocolName;
            }
        }

        /// <summary>
        /// 試薬コードの取得、設定
        /// </summary>
        protected Int32 ReagentCode
        {
            get
            {
                return this.Field<Int32>( CalibrationCurveDB.STRING_REAGENT_CODE );
            }
            set
            {
                this.SetField<Int32>( CalibrationCurveDB.STRING_REAGENT_CODE, value );
            }
        }
        /// <summary>
        /// ラックIDの取得
        /// </summary>
        public CarisXIDString RackID
        {
            get
            {
                //return ( (CarisXIDString)this.Field<String>( CalibrationCurveDB.STRING_RACKID ) ).DispPreCharString;
                return this.Field<String>( CalibrationCurveDB.STRING_RACKID );
            }
            protected set
            {
                this.SetField<String>( CalibrationCurveDB.STRING_RACKID, ( ( value != null ) ? value.DispPreCharString : "" ) );
            }
        }

        /// <summary>
        /// ラックポジションの取得
        /// </summary>
        public Int32? RackPosition
        {
            get
            {
                return this.Field<Int32?>( CalibrationCurveDB.STRING_RACKPOSITION );
            }
            protected set
            {
                this.SetField<Int32?>( CalibrationCurveDB.STRING_RACKPOSITION, value );
            }
        }

        /// <summary>
        /// 検体多重測定回数の取得
        /// </summary>
        public Int32 MultiMeasNo
        {
            get
            {
                return this.Field<Int32>( CalibrationCurveDB.STRING_REPLICATIONNO );
            }
            protected set
            {
                this.SetField<Int32>( CalibrationCurveDB.STRING_REPLICATIONNO, value );
            }
        }

        /// <summary>
        /// 濃度の取得
        /// </summary>
        public String Concentration
        {
            get
            {
                return this.Field<String>( CalibrationCurveDB.STRING_CONCENTRATION );
            }
            protected set
            {
                this.SetField<String>( CalibrationCurveDB.STRING_CONCENTRATION, value );
            }
        }

        /// <summary>
        /// カウント値の取得、設定(※設定により編集マークが付く)
        /// </summary>
        public Int32? Count
        {
            get
            {
                return this.Field<Int32?>( CalibrationCurveDB.STRING_COUNTVAL );
            }
            set
            {
                //値が設定されている場合だけセットする
                if (value.HasValue)
                {
                    var oldCount = this.Field<Int32?>(CalibrationCurveDB.STRING_COUNTVAL);
                    this.SetField<Int32?>(CalibrationCurveDB.STRING_COUNTVAL, value);
                    if (oldCount.HasValue && oldCount.Value != value && this.IsModifyData())
                    {
                        this.IsEdited = true;
                    }
                }
            }
        }

        /// <summary>
        /// カウント値(平均)の取得
        /// </summary>
        public Int32? CountAverage
        {
            get
            {
                return this.Field<Int32?>( CalibrationCurveDB.STRING_COUNTAVERAGE );
            }
            set
            {
                this.SetField<Int32?>( CalibrationCurveDB.STRING_COUNTAVERAGE, value );
            }
        }

        /// <summary>
        /// 測定ポイント(*)
        /// </summary>
        public String MeasPoint
        {
            get
            {
                if ( ( this.Field<Int32>( CalibrationCurveDB.STRING_UNIQUENO ) ) > 0 )
                {
                    return Oelco.CarisX.Properties.Resources.STRING_COMMON_005;
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        /// <summary>
        /// 分析項目インデックスの取得
        /// </summary>
        protected Int32 MeasureProtocolIndex
        {
            get
            {
                return this.Field<Int32>( CalibrationCurveDB.STRING_MEASUREPROTOCOLINDEX );
            }
            set
            {
                this.SetField<Int32>( CalibrationCurveDB.STRING_MEASUREPROTOCOLINDEX, value );
            }
        }

        /// <summary>
        /// 測定日時(検量線策定日時)の取得
        /// </summary>
        protected DateTime ApprovalDateTime
        {
            get
            {
                return this.Field<DateTime>( CalibrationCurveDB.STRING_APPROBAL_DATETIME );
            }
            set
            {
                this.SetField<DateTime>( CalibrationCurveDB.STRING_APPROBAL_DATETIME, value );
            }
        }

        /// <summary>
        /// ユニーク番号の取得
        /// </summary>
        protected Int32 UniqueNo
        {
            get
            {
                return this.Field<Int32>( CalibrationCurveDB.STRING_UNIQUENO );
            }
            set
            {
                this.SetField<Int32>( CalibrationCurveDB.STRING_UNIQUENO, value );
            }
        }

        /// <summary>
        /// ユーザによる編集の有無の取得
        /// </summary>
        protected Boolean IsEdited
        {
            get
            {
                return this.Field<Boolean>( CalibrationCurveDB.STRING_IS_EDITED );
            }
            set
            {
                this.SetField<Boolean>( CalibrationCurveDB.STRING_IS_EDITED, value );
            }
        }

        /// <summary>
        /// 試薬ロット番号の取得
        /// </summary>
        protected String ReagentLotNo
        {
            get
            {
                return this.Field<String>( CalibrationCurveDB.STRING_REAGENT_LOTNO );
            }
            set
            {
                this.SetField<String>( CalibrationCurveDB.STRING_REAGENT_LOTNO, value );
            }
        }

        /// <summary>
        /// 検量線ポイント番号の取得
        /// </summary>
        protected Int32 PointNo
        {
            get
            {
                return this.Field<Int32>( CalibrationCurveDB.STRING_POINTNO );
            }
            set
            {
                this.SetField<Int32>( CalibrationCurveDB.STRING_POINTNO, value );
            }
        }

        /// <summary>
        /// 拡張用値1の取得
        /// </summary>
        protected String ExtendValue1
        {
            get
            {
                return this.Field<String>( CalibrationCurveDB.STRING_EXTEND_VALUE_1 );
            }
            set
            {
                this.SetField<String>( CalibrationCurveDB.STRING_EXTEND_VALUE_1, value );
            }
        }
        /// <summary>
        /// 拡張用値2の取得
        /// </summary>
        protected String ExtendValue2
        {
            get
            {
                return this.Field<String>( CalibrationCurveDB.STRING_EXTEND_VALUE_2 );
            }
            set
            {
                this.SetField<String>( CalibrationCurveDB.STRING_EXTEND_VALUE_2, value );
            }
        }

        /// <summary>
        /// 拡張用値3の取得
        /// </summary>
        protected String ExtendValue3
        {
            get
            {
                return this.Field<String>( CalibrationCurveDB.STRING_EXTEND_VALUE_3 );
            }
            set
            {
                this.SetField<String>( CalibrationCurveDB.STRING_EXTEND_VALUE_3, value );
            }
        }

        /// <summary>
        /// 拡張用値4の取得
        /// </summary>
        protected String ExtendValue4
        {
            get
            {
                return this.Field<String>( CalibrationCurveDB.STRING_EXTEND_VALUE_4 );
            }
            set
            {
                this.SetField<String>( CalibrationCurveDB.STRING_EXTEND_VALUE_4, value );
            }
        }


        /// <summary>
        /// 校准状态的显示
        /// </summary>
        protected int Status
        {
            get
            {
                return this.Field<int>(CalibrationCurveDB.STRING_STATUS);
            }
            set
            {
                this.SetField<int>(CalibrationCurveDB.STRING_STATUS, value);
            }
        }

        /// <summary>
        /// 分析モジュール番号の取得、設定
        /// </summary>
        public Int32 ModuleNo
        {
            get
            {
                return this.Field<Int32>(CalibrationCurveDB.STRING_MODULENO);
            }
            set
            {
                this.SetField<Int32>(CalibrationCurveDB.STRING_MODULENO, value);
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// ラックIDの設定
        /// </summary>
        /// <remarks>
        /// ラックIDを設定します。
        /// </remarks>
        /// <param name="rackID"></param>
        public void SetRackID( CarisXIDString rackID )
        {
            this.RackID = rackID;
        }

        /// <summary>
        /// ラックポジションの設定
        /// </summary>
        /// <remarks>
        /// ラックポジションを設定します。
        /// </remarks>
        /// <param name="rackPosition"></param>
        public void SetRackPosition( Int32? rackPosition )
        {
            this.RackPosition = rackPosition;
        }

        /// <summary>
        /// 検体多重測定回数の設定
        /// </summary>
        /// <remarks>
        /// 検体多重測定回数を設定します。
        /// </remarks>
        /// <param name="multiMeasNo"></param>
        public void SetMultiMeasNo( Int32 multiMeasNo )
        {
            this.MultiMeasNo = multiMeasNo;
        }

        /// <summary>
        /// 分析項目インデックスの取得
        /// </summary>
        /// <remarks>
        /// 分析項目インデックスを取得します。
        /// </remarks>
        /// <returns></returns>
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
        /// <param name="measureProtocolIndex"></param>
        public void SetMeasureProtocolIndex( Int32 measureProtocolIndex )
        {
            this.MeasureProtocolIndex = measureProtocolIndex;
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
        /// 測定日時(検量線策定日時)の取得
        /// </summary>
        /// <remarks>
        /// 測定日時(検量線策定日時)を取得します。
        /// </remarks>
        /// <returns></returns>
        public DateTime GetApprovalDateTime()
        {
            return this.ApprovalDateTime;
        }

        /// <summary>
        /// 測定日時(検量線策定日時)の設定
        /// </summary>
        /// <remarks>
        ///  測定日時(検量線策定日時)を設定します。
        /// </remarks>
        /// <param name="approvalDateTime"></param>
        public void SetApprovalDateTime( DateTime approvalDateTime )
        {
            this.ApprovalDateTime = approvalDateTime;
        }

        /// <summary>
        /// ユニーク番号の取得
        /// </summary>
        /// <remarks>
        /// ユニーク番号を取得します。
        /// </remarks>
        /// <returns></returns>
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
        /// ユーザによる変更の有無
        /// </summary>
        /// <remarks>
        /// ユーザによる変更の有無を取得します。
        /// </remarks>
        /// <returns></returns>
        public Boolean IsUserEdited()
        {
            return this.IsEdited;
        }

        /// <summary>
        /// ユーザによる変更の有無プロパティの初期化
        /// </summary>
        /// <remarks>
        /// ユーザによる変更の有無プロパティを初期化します。
        /// </remarks>
        public void InitIsUserEdited()
        {
            this.IsEdited = false;
        }

        /// <summary>
        /// 試薬ロットNo.の取得
        /// </summary>
        /// <remarks>
        /// 試薬ロットNo.を取得します。
        /// </remarks>
        /// <returns></returns>
        public String GetReagentLotNo()
        {
            return this.ReagentLotNo;
        }

        /// <summary>
        /// 試薬ロットNo.の設定
        /// </summary>
        /// <remarks>
        /// 試薬ロットNo.を設定します。
        /// </remarks>
        /// <param name="reagentLotNo"></param>
        public void SetReagentLotNo( String reagentLotNo )
        {
            this.ReagentLotNo = reagentLotNo;
        }

        /// <summary>
        /// 試薬コードの取得
        /// </summary>
        /// <remarks>
        /// 試薬コードを取得します。
        /// </remarks>
        /// <returns></returns>
        public Int32 GetReagentCode()
        {
            return this.ReagentCode;
        }

        /// <summary>
        /// 試薬コードの設定
        /// </summary>
        /// <remarks>
        /// 試薬コードを設定します。
        /// </remarks>
        /// <param name="reagentCode"></param>
        public void SetReagentCode( Int32 reagentCode )
        {
            this.ReagentCode = reagentCode;
        }

        /// <summary>
        /// 検量線ポイント番号の取得
        /// </summary>
        /// <remarks>
        /// 検量線ポイント番号を取得します。
        /// </remarks>
        /// <returns></returns>
        public Int32 GetPointNo()
        {
            return this.PointNo;
        }

        /// <summary>
        /// 検量線ポイント番号の設定
        /// </summary>
        /// <remarks>
        /// 検量線ポイント番号を設定します。
        /// </remarks>
        /// <param name="pointNo"></param>
        public void SetPointNo( Int32 pointNo )
        {
            this.PointNo = pointNo;
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
            this.Count = count;
        }

        /// <summary>
        /// カウント平均値の設定
        /// </summary>
        /// <remarks>
        /// カウント平均値を設定します。
        /// </remarks>
        /// <param name="countAve"></param>
        public void SetCountAverage( Int32? countAve )
        {
            this.CountAverage = countAve;
        }

        /// <summary>
        /// 拡張用値1の設定
        /// </summary>
        /// <remarks>
        /// 拡張用値1を設定します。
        /// </remarks>
        /// <param name="countAve"></param>
        public void SetExtendValue1( String extendValue )
        {
            this.ExtendValue1 = extendValue;
        }
        /// <summary>
        /// 拡張用値2の設定
        /// </summary>
        /// <remarks>
        /// 拡張用値2を設定します。
        /// </remarks>
        /// <param name="countAve"></param>
        public void SetExtendValue2( String extendValue )
        {
            this.ExtendValue2 = extendValue;
        }
        /// <summary>
        /// 拡張用値3の設定
        /// </summary>
        /// <remarks>
        /// 拡張用値3を設定します。
        /// </remarks>
        /// <param name="countAve"></param>
        public void SetExtendValue3( String extendValue )
        {
            this.ExtendValue3 = extendValue;
        }
        /// <summary>
        /// 拡張用値4の設定
        /// </summary>
        /// <remarks>
        /// 拡張用値4を設定します。
        /// </remarks>
        /// <param name="countAve"></param>
        public void SetExtendValue4( String extendValue )
        {
            this.ExtendValue4 = extendValue;
        }

        /// <summary>
        /// 校准曲线状态的设置
        /// </summary>
        /// <remarks>
        /// 校准曲线状态的设置
        /// </remarks>
        /// <param name="Status"></param>
        public void SetStatus(int status)
        {
            this.Status = status;
        }



        /// <summary>
        /// 拡張用値1の取得
        /// </summary>
        /// <remarks>
        /// 拡張用値1を取得します。
        /// </remarks>
        /// <param name="countAve"></param>
        public String GetExtendValue1()
        {
            return this.ExtendValue1;
        }
        /// <summary>
        /// 拡張用値2の取得
        /// </summary>
        /// <remarks>
        /// 拡張用値2を取得します。
        /// </remarks>
        /// <param name="countAve"></param>
        public String GetExtendValue2()
        {
            return this.ExtendValue2;
        }
        /// <summary>
        /// 拡張用値3の取得
        /// </summary>
        /// <remarks>
        /// 拡張用値3を取得します。
        /// </remarks>
        /// <param name="countAve"></param>
        public String GetExtendValue3()
        {
            return this.ExtendValue3;
        }
        /// <summary>
        /// 拡張用値4の取得
        /// </summary>
        /// <remarks>
        /// 拡張用値4を取得します。
        /// </remarks>
        /// <param name="countAve"></param>
        public String GetExtendValue4()
        {
            return this.ExtendValue4;
        }

        /// <summary>
        /// 校准状态的获得
        /// </summary>
        /// <remarks>
        /// 校准状态的获得
        /// </remarks>
        /// <param name="countAve"></param>
        public int  GetStatus()
        {
            return this.Status;
        }

        /// <summary>
        /// モジュール番号の取得
        /// </summary>
        /// <returns>モジュール番号</returns>
        public Int32 GetModuleNo()
        {
            return this.ModuleNo;
        }
        /// <summary>
        /// モジュール番号の設定
        /// </summary>
        /// <param name="moduleNo">モジュール番号</param>
        public void SetModuleNo(Int32 moduleNo)
        {
            this.ModuleNo = moduleNo;
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
            public const String Analytes = "Analytes";
            public const String RackID = "RackID";
            public const String RackPosition = "RackPosition";
            public const String MultiMeasNo = "MultiMeasNo";
            public const String Concentration = "Concentration";
            public const String Count = "Count";
            public const String CountAverage = "CountAverage";
            public const String MeasPoint = "MeasPoint";
            public const String ModuleNo = "ModuleNo";
        };

        #endregion

    }

    /// <summary>
    /// キャリブレータ検量線情報データクラス(解析向け)
    /// </summary>
    /// <remarks>
    /// キャリブレータ検量線情報データクラスです。(解析向け)
    /// </remarks>
    public class CalibrationCurveAnalysisData : CalibrationCurveData
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data"></param>
        public CalibrationCurveAnalysisData( DataRowWrapperBase data )
            : base( data )
        {
        }

        #endregion

        #region[プロパティ]

        /// <summary>
        /// 濃度(文字列)の取得
        /// </summary>
        public new String Concentration
        {
            get
            {
                MeasureProtocol measureProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( this.MeasureProtocolIndex );
                // 定性項目、表示(陰性/陽性)切替
                if ( measureProtocol.CalibType.IsQualitative() && this.PointNo >= 0 && this.PointNo <= CarisXConst.QUALITATIVE_POINT_COUNT )
                {
                    var negaPosi = new[] { CarisXConst.NEGATIVE_POSITION, CarisXConst.POSITIVE_POSITION };
                    return negaPosi[this.PointNo - 1];
                }
                else
                {
                    return base.Concentration;
                }

            }
        }

        #endregion
    }


    /// <summary>
    /// キャリブレータ検量線情報DBクラス
    /// </summary>
    /// <remarks>
    /// キャリブレータ検量線情報のアクセスを行うクラスです。
    /// </remarks>
    public class CalibrationCurveDB : DBAccessControl
    {
        #region [定数定義]

        /// <summary>
        /// 分析項目インデックス(DBテーブル：CalibrationCurve列名)
        /// </summary>
        public const String STRING_MEASUREPROTOCOLINDEX = "measureProtocolIndex";

        /// <summary>
        /// キャリブレーションポイント番号(DBテーブル：CalibrationCurve列名)
        /// </summary>
        public const String STRING_POINTNO = "pointNo";

        /// <summary>
        /// ラックID(DBテーブル：CalibrationCurve列名)
        /// </summary>
        public const String STRING_RACKID = "rackId";

        /// <summary>
        /// 試薬ロット番号(DBテーブル：CalibrationCurve列名)
        /// </summary>
        public const String STRING_REAGENT_LOTNO = "reagentLotNo";

        /// <summary>
        /// ラックポジション(DBテーブル：CalibrationCurve列名)
        /// </summary>
        public const String STRING_RACKPOSITION = "rackPosition";

        /// <summary>
        /// 検体多重測定回数(DBテーブル：CalibrationCurve列名)
        /// </summary>
        public const String STRING_REPLICATIONNO = "replicationNo";

        /// <summary>
        /// 濃度値(DBテーブル：CalibrationCurve列名)
        /// </summary>
        public const String STRING_CONCENTRATION = "concentration";

        /// <summary>
        /// カウント値(DBテーブル：CalibrationCurve列名)
        /// </summary>
        public const String STRING_COUNTVAL = "countVal";

        /// <summary>
        /// カウント平均値(DBテーブル：CalibrationCurve列名)
        /// </summary>
        public const String STRING_COUNTAVERAGE = "countAve";

        /// <summary>
        /// 測定日時(DBテーブル：CalibrationCurve列名)
        /// </summary>
        public const String STRING_APPROBAL_DATETIME = "approvalDateTime";

        /// <summary>
        /// ユニーク番号(DBテーブル：CalibrationCurve列名)
        /// </summary>
        public const String STRING_UNIQUENO = "uniqueNo";

        /// <summary>
        /// ユーザー変更フラグ(DBテーブル：CalibrationCurve列名)
        /// </summary>
        public const String STRING_IS_EDITED = "isEdited";

        /// <summary>
        /// 試薬コード(DBテーブル：CalibrationCurve列名)
        /// </summary>
        public const String STRING_REAGENT_CODE = "reagentCode";
        /// <summary>
        /// 拡張用値1(DBテーブル：CalibrationCurve列名)
        /// </summary>
        public const String STRING_EXTEND_VALUE_1 = "extendValue1";
        /// <summary>
        /// 拡張用値2(DBテーブル：CalibrationCurve列名)
        /// </summary>
        public const String STRING_EXTEND_VALUE_2 = "extendValue2";
        /// <summary>
        /// 拡張用値1(DBテーブル：CalibrationCurve列名)
        /// </summary>
        public const String STRING_EXTEND_VALUE_3 = "extendValue3";
        /// <summary>
        /// 拡張用値1(DBテーブル：CalibrationCurve列名)
        /// </summary>
        public const String STRING_EXTEND_VALUE_4 = "extendValue4";

        /// <summary>
        /// 校准品状态 0：normal 1: warning  3: error
        /// </summary>
        public const String STRING_STATUS = "status";

        /// <summary>
        /// 分析モジュール番号(DBテーブル：CalibrationCurve列名)
        /// </summary>
        public const String STRING_MODULENO = "moduleNo";

        #endregion

        #region [プロパティ]

        /// <summary>
        /// データテーブル取得SQL
        /// </summary>
        protected override String baseTableSelectSql
        {
            get
            {
                return "SELECT * FROM dbo.calibrationCurve";
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 検量線取得(取得検量線最大数25本･･･登録上限数による制限)
        /// ソート済(測定日時の降順、測定ポイントの昇順)
        /// </summary>
        /// <remarks>
        /// 検量線取得します
        /// </remarks>
        /// <param name="protocolIndex">分析項目インデックス</param>
        /// <returns>Key:試薬ロット番号 Value:(Key:測定日時.ToString() Value:キャリブレータ解析データ郡)　</returns>
        public Dictionary<String, Dictionary<String, List<CalibrationCurveData>>> GetData( Int32 protocolIndex, Int32 moduleNo = CarisXConst.ALL_MODULEID)
        {
            Dictionary<String, Dictionary<String, List<CalibrationCurveData>>> result = new Dictionary<String, Dictionary<String, List<CalibrationCurveData>>>();

            if ( this.DataTable != null )
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    List<CalibrationCurveData> datas = new List<CalibrationCurveData>();

                    // 全モジュール対象の場合
                    if (moduleNo == CarisXConst.ALL_MODULEID)
                    {
                        // 試薬コードが一致する検量線を取得
                        datas = ( from v in dataTableList
                                  where ( ( (Int32)v[STRING_MEASUREPROTOCOLINDEX] ) == protocolIndex )
                                  orderby ( (DateTime)v[STRING_APPROBAL_DATETIME] ) descending, ( (Int32)v[STRING_POINTNO] ) ascending
                                  select new CalibrationCurveData(v) ).ToList();
                    }
                    else
                    {
                        // 試薬コードとモジュールIDが一致する検量線を取得
                        datas = (from v in dataTableList
                                where (((Int32)v[STRING_MODULENO]) == moduleNo)
                                   && (((Int32)v[STRING_MEASUREPROTOCOLINDEX]) == protocolIndex)
                                orderby ((DateTime)v[STRING_APPROBAL_DATETIME]) descending, ((Int32)v[STRING_POINTNO]) ascending
                                 select new CalibrationCurveData(v)).ToList();
                    }


                    String key;
                    foreach ( var item in datas.GroupBy( ( data ) => data.GetReagentLotNo() ) )
                    {
                        if ( !result.ContainsKey( item.Key) )
                        {
                            result.Add( item.Key, new Dictionary<String, List<CalibrationCurveData>>() );
                        }
                        foreach ( var data in item )
                        {
                            key = String.Empty;

                            if ( data.GetUniqueNo() > -1 )                            //　マスターカーブ以外の場合
                            {
                                // 測定日時を文字列としてキーに設定
                                key = data.GetApprovalDateTime().ToString();
                            }
                            else                                                    // マスターカーブの場合
                            {
                                // マスターカーブ文字列をキーに設定
                                key = CarisX.Properties.Resources.STRING_CALIBANALYSIS_018;
                            }

                            if ( !result[item.Key].ContainsKey( key ) )
                            {
                                result[item.Key].Add( key, new List<CalibrationCurveData>() );
                            }

                            // 検量線情報追加
                            result[item.Key][key].Add( data );
                        }
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
        /// キャリブレータ検量線情報取得
        /// </summary>
        /// <remarks>
        /// 引数値を条件に取得したキャリブレータ検量線情報を返します。
        /// </remarks>
        /// <param name="protocolIndex"></param>
        /// <returns></returns>
        public Dictionary<String, Dictionary<String, List<CalibrationCurveAnalysisData>>> GetAnalysisData( Int32 protocolIndex, Int32 moduleNo )
        {
            Dictionary<String, Dictionary<String, List<CalibrationCurveAnalysisData>>> result = new Dictionary<String, Dictionary<String, List<CalibrationCurveAnalysisData>>>();

            if ( this.DataTable != null )
            {
                try
                {
                    //【IssuesNo:19】Innodx要求不显示主曲线，因此在此过滤主曲线数据
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    var datas = from v in dataTableList
                                where ((((Int32)v[STRING_MODULENO] ) == moduleNo) || (((Int32)v[STRING_UNIQUENO]) == -1))
                                   && (((Int32)v[STRING_MEASUREPROTOCOLINDEX]) == protocolIndex)
                                   && (((Int32)v[STRING_UNIQUENO]) > -1)
                                orderby ( (DateTime)v[STRING_APPROBAL_DATETIME] ) descending, ( (Int32)v[STRING_POINTNO] ) ascending
                                select new CalibrationCurveAnalysisData( v );

                    String key;
                    foreach ( var item in datas.GroupBy( ( data ) => data.GetReagentLotNo() ) )
                    {
                        if ( !result.ContainsKey( item.Key ) )
                        {
                            result.Add( item.Key, new Dictionary<String, List<CalibrationCurveAnalysisData>>() );
                        }
                        foreach ( var data in item )
                        {
                            key = String.Empty;

                            if ( data.GetUniqueNo() > -1 )                            //　マスターカーブ以外の場合
                            {
                                // 測定日時を文字列としてキーに設定
                                key = data.GetApprovalDateTime().ToString();
                            }
                            else                                                    // マスターカーブの場合
                            {
                                // マスターカーブ文字列をキーに設定
                                key = CarisX.Properties.Resources.STRING_CALIBANALYSIS_018;
                            }

                            if ( !result[item.Key].ContainsKey( key ) )
                            {
                                result[item.Key].Add( key, new List<CalibrationCurveAnalysisData>() );
                            }

                            // 検量線情報追加
                            result[item.Key][key].Add( data );
                        }
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
        /// 検量線取得(取得検量線最大数25本･･･登録上限数による制限)
        /// ソート済(測定日時の降順、測定ポイントの昇順)
        /// マスターカーブは含まない
        /// </summary>
        /// <remarks>
        /// 検量線取得します
        /// </remarks>
        /// <param name="protocolIndex">分析項目インデックス</param>
        /// <param name="reagentLotNo">試薬ロット番号</param>
        /// <returns>Key:測定日時.ToString() Value:キャリブレータ解析データ郡</returns>
        public Dictionary<String, List<CalibrationCurveData>> GetData( Int32 protocolIndex, String reagentLotNo, Int32 moduleNo = CarisXConst.ALL_MODULEID)
        {
            Dictionary<String, List<CalibrationCurveData>> result = new Dictionary<String, List<CalibrationCurveData>>();

            if ( this.DataTable != null )
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    if(moduleNo == CarisXConst.ALL_MODULEID)
                    {
                        var datas = (from v in dataTableList
                                     let Data = new CalibrationCurveData(v)
                                     let notMasterCurve = Data.GetUniqueNo() > -1
                                     where (((Int32)v[STRING_MEASUREPROTOCOLINDEX]) == protocolIndex)
                                        && (((String)v[STRING_REAGENT_LOTNO]) == reagentLotNo)
                                     orderby ((DateTime)v[STRING_APPROBAL_DATETIME]) descending, ((Int32)v[STRING_POINTNO]) ascending
                                     let Key = notMasterCurve ? Data.GetApprovalDateTime().ToString() : CarisX.Properties.Resources.STRING_CALIBANALYSIS_018 //　日時の文字列化(マスターカーブのみ専用文字列)
                                     group Data by Key into grp
                                     select new
                                     {
                                         grp.Key,
                                         Datas = grp.ToList()
                                     });

                        foreach (var data in datas)
                        {
                            // 検量線情報追加
                            result.Add(data.Key, data.Datas);
                        }
                    }
                    else
                    {
                        var datas = (from v in dataTableList
                                     let Data = new CalibrationCurveData(v)
                                     let notMasterCurve = Data.GetUniqueNo() > -1
                                     where (((Int32)v[STRING_MODULENO]) == moduleNo)
                                        && (((Int32)v[STRING_MEASUREPROTOCOLINDEX]) == protocolIndex)
                                        && (((String)v[STRING_REAGENT_LOTNO]) == reagentLotNo)
                                     orderby ((DateTime)v[STRING_APPROBAL_DATETIME]) descending, ((Int32)v[STRING_POINTNO]) ascending
                                     let Key = notMasterCurve ? Data.GetApprovalDateTime().ToString() : CarisX.Properties.Resources.STRING_CALIBANALYSIS_018 //　日時の文字列化(マスターカーブのみ専用文字列)
                                     group Data by Key into grp
                                     select new
                                     {
                                         grp.Key,
                                         Datas = grp.ToList()
                                     });

                        foreach (var data in datas)
                        {
                            // 検量線情報追加
                            result.Add(data.Key, data.Datas);
                        }
                    }
                }
                catch ( Exception ex )
                {
                    // DB内部に不正データ
                    //Singleton<LogManager>.Instance.Error( ex.Message );
                    Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
                }
            }

            return result;
        }

        /// <summary>
        /// 検量線取得(マスターカーブは含まない)
        /// ソート済(測定日時の降順、測定ポイントの昇順)
        /// </summary>
        /// <remarks>
        /// 検量線取得します
        /// </remarks>
        /// <param name="protocolIndex">分析項目インデックス</param>
        /// <param name="reagentLotNo">試薬ロット番号</param>
        /// <returns>Key:測定日時.ToString() Value:キャリブレータ解析データ郡　</returns>
        public Dictionary<String, List<CalibrationCurveData>> GetDataExcludeMasterCurve(Int32 protocolIndex, String reagentLotNo, Int32 moduleNo = CarisXConst.ALL_MODULEID)
        {
            Dictionary<String, List<CalibrationCurveData>> result = new Dictionary<String, List<CalibrationCurveData>>();

            if (this.DataTable != null)
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    if (moduleNo == CarisXConst.ALL_MODULEID)
                    {

                        var datas = (from v in dataTableList
                                     let Data = new CalibrationCurveData(v)
                                     where (((Int32)v[STRING_MEASUREPROTOCOLINDEX]) == protocolIndex)
                                        && (((String)v[STRING_REAGENT_LOTNO]) == reagentLotNo)
                                        && (((Int32)v[STRING_UNIQUENO]) > -1)
                                     orderby ((DateTime)v[STRING_APPROBAL_DATETIME]) descending, ((Int32)v[STRING_POINTNO]) ascending
                                     let Key = Data.GetApprovalDateTime().ToString()
                                     group Data by Key into grp
                                     select new
                                     {
                                         grp.Key,
                                         Datas = grp.ToList()
                                     });

                        foreach (var data in datas)
                        {
                            // 検量線情報追加
                            result.Add(data.Key, data.Datas);
                        }
                    }
                    else
                    {
                        var datas = (from v in dataTableList
                                     let Data = new CalibrationCurveData(v)
                                     where (((Int32)v[STRING_MODULENO]) == moduleNo)
                                        && (((Int32)v[STRING_MEASUREPROTOCOLINDEX]) == protocolIndex)
                                        && (((String)v[STRING_REAGENT_LOTNO]) == reagentLotNo)
                                        && (((Int32)v[STRING_UNIQUENO]) > -1)
                                     orderby ((DateTime)v[STRING_APPROBAL_DATETIME]) descending, ((Int32)v[STRING_POINTNO]) ascending
                                     let Key = Data.GetApprovalDateTime().ToString()
                                     group Data by Key into grp
                                     select new
                                     {
                                         grp.Key,
                                         Datas = grp.ToList()
                                     });

                        foreach (var data in datas)
                        {
                            // 検量線情報追加
                            result.Add(data.Key, data.Datas);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // DB内部に不正データ
                    //Singleton<LogManager>.Instance.Error( ex.Message );
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                }
            }

            return result;
        }

        /// <summary>
        /// 指定の作成日時の検量線取得
        /// ソート済(測定ポイントの昇順)
        /// </summary>
        /// <remarks>
        /// 分析項目インデックス、試薬ロット番号、検量線作成日時を条件に取得した検量線情報を返します。
        /// </remarks>
        /// <param name="protocolIndex">分析項目インデックス</param>
        /// <param name="reagentLotNo">試薬ロット番号</param>
        /// <param name="approvalDateTime">検量線作成日時</param>
        /// <returns>Key:測定日時.ToString() Value:キャリブレータ解析データ郡</returns>
        public List<CalibrationCurveData> GetData( Int32 protocolIndex, String reagentLotNo, DateTime approvalDateTime, Int32 moduleNo = CarisXConst.ALL_MODULEID )
        {
            List<CalibrationCurveData> result = new List<CalibrationCurveData>();

            if ( this.DataTable != null )
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    if( moduleNo == CarisXConst.ALL_MODULEID )
                    {
                        result = (from v in dataTableList
                                  let data = new CalibrationCurveData(v)
                                  where ( data.GetMeasureProtocolIndex() == protocolIndex )
                                     && ( data.GetReagentLotNo() == reagentLotNo )
                                     && ( data.GetApprovalDateTime() == approvalDateTime )
                                  orderby ((Int32)v[STRING_POINTNO]) ascending
                                  select data).ToList();
                    }
                    else
                    {
                        result = (from v in dataTableList
                                  let data = new CalibrationCurveData(v)
                                  where (data.GetModuleNo() == moduleNo)
                                     && (data.GetMeasureProtocolIndex() == protocolIndex)
                                     && (data.GetReagentLotNo() == reagentLotNo)
                                     && (data.GetApprovalDateTime() == approvalDateTime)
                                  orderby ((Int32)v[STRING_POINTNO]) ascending
                                  select data).ToList();
                    }
                }
                catch ( Exception ex )
                {
                    // DB内部に不正データ
                    //Singleton<LogManager>.Instance.Error( ex.Message );
                    Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
                }
            }

            return result;
        }

        /// <summary>
        /// 得到试剂条码上得到的基础曲线
        /// ソート済(測定ポイントの昇順)
        /// </summary>
        /// <remarks>
        /// 得到试剂条码上得到的基础曲线
        /// </remarks>
        /// <param name="protocolIndex">分析項目インデックス</param>
        /// <param name="reagentLotNo">試薬ロット番号</param>        /// 
        /// <returns>MasterCurfe List</returns>
        public List<CalibrationCurveData> GetMasterCurveData(Int32 protocolIndex, String reagentLotNo)
        {
            List<CalibrationCurveData> result = new List<CalibrationCurveData>();

            if (this.DataTable != null)
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    result = (from v in dataTableList
                              let data = new CalibrationCurveData(v)
                              where data.GetMeasureProtocolIndex() == protocolIndex && data.GetReagentLotNo() == reagentLotNo && data.GetUniqueNo() == -1
                              orderby ((Int32)v[STRING_POINTNO]) ascending
                              select data).ToList();
                }
                catch (Exception ex)
                {
                    // DB内部に不正データ
                    //Singleton<LogManager>.Instance.Error( ex.Message );
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                }
            }

            return result;
        }

        /// <summary>
        /// 最新日付の検量線取得(分析項目、試薬ロット別) ※マスターカーブを除く
        /// ソート済(測定ポイントの昇順)
        /// </summary>
        /// <remarks>
        /// 最新日付の検量線取得します
        /// </remarks>
        /// <returns>Key:分析項目インデックス,試薬ロット番号 Value:キャリブレータ解析データ郡)</returns>
        public Dictionary<Int32, Dictionary<Int32, Dictionary<String, List<CalibrationCurveData>>>> GetLastDateInfo( Int32 moduleNo, Int32? protocolIndex = null )
        {
            Dictionary < Int32, Dictionary < Int32, Dictionary<String, List<CalibrationCurveData>>>> result = new Dictionary<Int32, Dictionary<Int32, Dictionary<String, List<CalibrationCurveData>>>>();

            if ( this.DataTable != null )
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    var datas = from v in dataTableList.AsEnumerable()
                                orderby ((DateTime)v[STRING_APPROBAL_DATETIME]) descending, ((Int32)v[STRING_POINTNO]) ascending
                                let data = new CalibrationCurveData(v)
                                where data.GetUniqueNo() > -1 // マスターカーブを含めないNot including the master curve
                                && new Func<Boolean>(() =>
                               {
                                   if (protocolIndex.HasValue)
                                   {
                                       return data.GetMeasureProtocolIndex() == protocolIndex;
                                   }
                                   return true;
                               })()
                                group data by new
                                {
                                    ApprovalDate = data.GetApprovalDateTime(),
                                    MeasureProtocolIndex = data.GetMeasureProtocolIndex(),
                                    ReagentLotNo = data.GetReagentLotNo(),
                                    ModuleNo = data.GetModuleNo(),
                                } into grpSet           // 測定日時別データ
                                group grpSet by new
                                {
                                    grpSet.Key.MeasureProtocolIndex,
                                    grpSet.Key.ReagentLotNo,
                                    grpSet.Key.ModuleNo
                                } into LastDateGroup    // 分析項目、試薬ロット番号別データ(最新測定日時)
                                select LastDateGroup;

                    // 
                    foreach ( var data in datas )
                    {
                        if( result.Keys.Contains(data.Key.ModuleNo) )
                        {
                            if (result[data.Key.ModuleNo].Keys.Contains(data.Key.MeasureProtocolIndex))
                            {
                                if (result[data.Key.ModuleNo][data.Key.MeasureProtocolIndex].Keys.Contains(data.Key.ReagentLotNo))
                                {
                                    if (result[data.Key.ModuleNo][data.Key.MeasureProtocolIndex][data.Key.ReagentLotNo].Count < 1)
                                    {
                                        result[data.Key.ModuleNo][data.Key.MeasureProtocolIndex][data.Key.ReagentLotNo].AddRange(data.First().ToList());
                                    }
                                }
                                else
                                {
                                    result[data.Key.ModuleNo][data.Key.MeasureProtocolIndex].Add(data.Key.ReagentLotNo, data.First().ToList());
                                }
                            }
                            else
                            {
                                result[data.Key.ModuleNo].Add(data.Key.MeasureProtocolIndex, new Dictionary<String, List<CalibrationCurveData>>()
                                {
                                   {data.Key.ReagentLotNo,data.First().ToList()}
                                });
                            }
                        }
                        else
                        {
                            result.Add(data.Key.ModuleNo, new Dictionary<Int32, Dictionary<String, List<CalibrationCurveData>>>()
                            {
                                {
                                    data.Key.MeasureProtocolIndex, new Dictionary<String, List<CalibrationCurveData>>()
                                    {
                                        {data.Key.ReagentLotNo, data.First().ToList()}
                                    }
                                }
                            });
                        }
                    }
                }
                catch ( Exception ex )
                {
                    // DB内部に不正データ
                    //Singleton<LogManager>.Instance.Error( ex.Message );
                    Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
                }
            }

            return result;
        }

        /// <summary>
        /// 検量線情報登録
        /// </summary>
        /// <remarks>
        /// 検量線情報を登録します。
        /// </remarks>
        /// <param name="reagentLotNo">試薬ロット番号</param>
        /// <param name="rackId">ラックID</param>
        /// <param name="rackPosition">ラックポジション</param>
        /// <param name="measureProtocolIndex">分析項目インデックス</param>
        /// <param name="multiMeasNo">検体多重測定回数</param>
        /// <param name="concentration">濃度</param>
        /// <param name="count">カウント値</param>
        /// <param name="pointNo">検量線ポイント番号</param>
        /// <param name="countAve">カウント平均値</param>
        /// <param name="uniqueNo">ユニーク番号</param>
        public void AddCalibData( Int32 moduleNo, Int32 reagentCode, String reagentLotNo, CarisXIDString rackId, Int32? rackPosition, Int32 measureProtocolIndex, Int32 multiMeasNo, String concentration, Int32? count, Int32 pointNo, Int32? countAve, Int32 uniqueNo, DateTime approvalDateTime, List<String> extParam = null )
        {
            if ( this.DataTable != null )
            {
                var data = new CalibrationCurveData( this.DataTable.NewRow() );

                data.SetReagentLotNo( reagentLotNo );
                data.SetMeasureProtocolIndex( measureProtocolIndex );
                data.SetApprovalDateTime( DateTime.Now );
                data.SetMultiMeasNo( multiMeasNo );
                data.SetConcentration( concentration );
                data.SetRackID( rackId );
                data.SetRackPosition( rackPosition );
                data.SetReagentCode( Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( measureProtocolIndex ).ReagentCode );
                data.SetApprovalDateTime( approvalDateTime );
                data.SetPointNo( pointNo );
                data.SetCount( count );
                data.SetCountAverage( countAve );
                data.SetUniqueNo( uniqueNo );
                data.SetReagentCode( reagentCode );
                data.InitIsUserEdited();
                data.SetModuleNo( moduleNo );

                // 拡張パラメータ設定
                if ( extParam != null )
                {
                    List<Action<String>> setFuncs = new List<Action<String>>()
                    {
                        data.SetExtendValue1,
                        data.SetExtendValue2,
                        data.SetExtendValue3,
                        data.SetExtendValue4
                    };
                    for ( Int32 i = 0; ( i < extParam.Count() ) && ( i < setFuncs.Count ); i++ )
                    {
                        setFuncs[i]( extParam[i] );
                    }
                }

                data.SyncAddData( this.DataTable );
            }
        }

        /// <summary>
        /// 検量線データの設定
        /// </summary>
        /// <remarks>
        /// 検量線データの同期を行います。
        /// </remarks>
        /// <param name="calibCurveDatas">変更、削除操作済みデータ</param>
        public void SetCalibData( List<CalibrationCurveData> calibCurveDatas )
        {
            calibCurveDatas.SyncDataListToDataTable( this.DataTable );
        }

        /// <summary>
        /// DBのカラム追加
        /// <remarks>
        /// 今後、カラムの型変換を行う際は中のクエリを変更するような処理に変える
        /// </remarks>
        /// </summary>
        public void AddColumn()
        {
            // NULL不許可（デフォルト=1）でモジュール番号カラムを追加
            String addColumnSql = String.Format("alter table dbo.calibrationCurve add {0} int not null default '1'", CalibrationCurveDB.STRING_MODULENO);

            //　クエリ実行
            this.ExecuteSql(addColumnSql);
        }

        /// <summary>
        /// 検量線情報テーブル取得
        /// </summary>
        /// <remarks>
        /// 検量線情報をDBから読込みます。
        /// </remarks>
        public override void LoadDB()
        {
            this.fillBaseTable();
        }

        /// <summary>
        /// 検量線情報テーブル書込み
        /// </summary>
        /// <remarks>
        /// 検量線情報をDBに書き込みます。
        /// </remarks>
        public void CommitData()
        {
            this.updateBaseTable();
        }

        /// <summary>
        /// 保存上限
        /// </summary>
        /// <remarks>
        ///　保存上限を超えたデータを削除します
        /// </remarks>
        protected override void removeLimitOver()
        {
            //　コピーデータリストを取得
            var dataTableList = this.DataTable.AsEnumerable().ToList();

            // 検量線毎にグルーピング(未削除データ、日付昇順)
            var curveData = from v in dataTableList.Where( ( row ) => row.RowState != DataRowState.Deleted )
                            let data = new CalibrationCurveData( v )
                            orderby data.GetApprovalDateTime()
                            where !data.IsDeletedData() && data.GetApprovalDateTime() != DateTime.MinValue
                            group data by new
                            {
                                date = data.GetApprovalDateTime(),
                                protocol = data.GetMeasureProtocolIndex(),
                                reagentLot = data.GetReagentLotNo(),
                                moduleNo = data.GetModuleNo()
                            };

            // 削除対象として分析項目の同一試薬ロット数が5を超える古い検量線を取得
            var removeData = ( from v in curveData
                               group v by new
                               {    
                                   v.Key.protocol,
                                   v.Key.reagentLot,
                                   v.Key.moduleNo
                               } into grp
                               let count = grp.Count() - 5
                               where count > 0
                               select grp.Take( count ).SelectMany( ( data ) => data ) ).SelectMany( ( data ) => data );

            // 削除対象として分析項目の同一試薬ロット数が5を超える古い検量線(試薬ロット分)を取得
            removeData.Union( ( from v in curveData
                                group v by v.Key.protocol into grp
                                let lotNo = grp.Select( ( protocolData ) => protocolData.Key.reagentLot ).Distinct()
                                let count = lotNo.Count() - 5
                                let targetLotNo = grp.Select( ( protocolData ) => protocolData.Key.reagentLot ).Distinct().Take( count )
                                where count > 0
                                select grp.TakeWhile( ( data ) => targetLotNo.Contains( data.Key.reagentLot ) ).SelectMany( ( data ) => data ) ).SelectMany( ( data ) => data ) );

            // 削除
            var removeDataList = removeData.ToList();
            foreach ( var data in removeDataList )
            {
                data.DeleteData();
            }

            this.SetCalibData( removeDataList );
        }
        #endregion
    }
}
