using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtocolConverter.Check;

namespace ProtocolConverter.File
{
    /// <summary>
    /// コンバート処理設定用クラス
    /// </summary>
    public class ConvertXmlControl 
    {
        #region [定数定義]
        /// <summary>
        /// 分析項目インデックス
        /// </summary>
        public const String NM_PROTOCOLINDEX = "ProtocolIndex";
        /// <summary>
        /// 分析項目番号
        /// </summary>
        public const String NM_PROTOCOLNO = "ProtocolNo";
        /// <summary>
        /// 分析項目名称
        /// </summary>
        public const String NM_PROTOCOLNAME = "ProtocolName";
        /// <summary>
        /// 試薬コード
        /// </summary>
        public const String NM_REAGENTCODE = "ReagentCode";
        /// <summary>
        /// 試薬名称
        /// </summary>
        public const String NM_REAGENTNAME = "ReagentName";
        /// <summary>
        /// キャリブレータ多重測定回数
        /// </summary>
        public const String NM_REPNOFORCALIB = "RepNoForCalib";
        /// <summary>
        /// アッセイシーケンス
        /// </summary>
         public const String NM_ASSAYSEQ = "AssaySequence";
        /// <summary>
        /// 前処理シーケンス
        /// </summary>
         public const String NM_PREPROCESS_SEQ = "PreProcessSequence"; 
        /// <summary>
        /// サンプル種別
        /// </summary>
         public const String NM_SAMPLEKIND = "SampleKind";
        /// <summary>
        /// サンプル分注量 （μL)  
        /// </summary>
         public const String NM_SMP_DISPENSE_VOL = "SmpDispenseVolume";
        /// <summary>
        /// M試薬分注量 （μL)
        /// </summary>
         public const String NM_MREAGDISPENSEVOLUME = "MReagDispenseVolume";            
        /// <summary>
        /// // R1試薬分注量 （μL) 
        /// </summary>   
         public const String NM_R1DISPENSEVOLUME = "R1DispenseVolume";   
        /// <summary>
        /// R2試薬分注量 （μL) 
        /// </summary>   
         public const String NM_R2DISPENSEVOLUME = "R2DispenseVolume";  
        /// <summary>
        /// 前処理液1分注量 （μL)     
        /// </summary> 
        public const String NM_PREPROSESS_1_DISPENSEVOLUME  = "PreProsess1DispenseVolume";
        /// <summary>
        /// 前処理液2分注量 （μL)  
        /// </summary>  
        public const String NM_PREPROSESS_2_DISPENSEVOLUME  = "PreProsess2DispenseVolume";
        /// <summary>
        /// 多重測定内乖離限界（CV%）
        /// </summary>    
        public const String NM_MULMEASDEVLIMITCV  = "MulMeasDevLimitCV";
        /// <summary>
        /// ｷｬﾘﾌﾞﾚｰｼｮﾝ有効期限（日） 
        /// </summary>  
        public const String NM_VALIDITYOFCURVE = "ValidityOfCurve";                
        /// <summary>
        /// 手希釈使用有無
        /// </summary>
        public const String NM_USEMANUALDIL  = "UseManualDil";
        /// <summary>
        /// キャリブレーションタイプ   
        /// </summary> 
        public const String NM_CALIBTYPE  = "CalibType";
        /// <summary>
        /// キャリブレーションポイント数
        /// </summary>
        public const String NM_NUMOFMEASPOINTINCALIB = "NumOfMeasPointInCalib";
        /// <summary>
        /// キャリブレーション方法
        /// </summary>
        public const String NM_CALIBMETHOD = "CalibMethod";
        /// <summary>
        /// 濃度
        /// </summary>
        public const String NM_CONCSOFEACH = "ConcsOfEach{0}";
        /// <summary>
        /// 係数A 
        /// </summary>
        public const String NM_COEF_A  = "Coef_A";
        /// <summary>
        /// 係数B
        /// </summary>
        public const String NM_COEF_B  = "Coef_B";
        /// <summary>
        /// 係数C 
        /// </summary>
        public const String NM_COEF_C  = "Coef_C";
        /// <summary>
        /// 係数D 
        /// </summary>
        public const String NM_COEF_D  = "Coef_D";
        /// <summary>
        ///  係数E 
        /// </summary>
        public const String NM_COEF_E  = "Coef_E";
        /// <summary>
        /// 陽性判定閾値
        /// </summary>
        public const String NM_POSILINE  = "PosiLine";
        /// <summary>
        /// 陰性判定閾値
        /// </summary>
        public const String NM_NEGALINE  = "NegaLine";
        /// <summary>
        /// カウントチェック範囲　上限 
        /// </summary>
        public const String NM_COUNTRANGESOFEACH_MAX = "CountRangesOfEach{0}(Max)";	            
        /// <summary>
        /// カウントチェック範囲　下限 
        /// </summary>
        public const String NM_COUNTRANGESOFEACH_MIN = "CountRangesOfEach{0}(Min)";	  
        /// <summary>
        ///  濃度ダイナミックレンジ　上限 
        /// </summary>
        public const String NM_CONCDYNAMICRANGE_MAX = "ConcDynamicRange(Max)";
        /// <summary>
        ///  濃度ダイナミックレンジ　下限 
        /// </summary>
        public const String NM_CONCDYNAMICRANGE_MIN = "ConcDynamicRange(Min)";
        /// <summary>
        ///  係数A
        /// </summary>
        public const String NM_COEFAOFLOG = "CoefAOfLog";
        /// <summary>
        ///  係数B 
        /// </summary>
        public const String NM_COEFBOFLOG = "CoefBOfLog";
        /// <summary>
        /// 【IssuesNo:1】质控相关系数A
        /// </summary>
        public const String NM_CONTROL_COEFAOFLOG = "ControlCoefAOfLog";
        /// <summary>
        /// 【IssuesNo:1】质控相关系数B
        /// </summary>
        public const String NM_CONTROL_COEFBOFLOG = "ControlCoefBOfLog";
        /// <summary>
        ///  濃度単位 
        /// </summary>
        public const String NM_CONCUNIT = "ConcUnit";
        /// <summary>
        /// 濃度値小数点以下桁数
        /// </summary>
        public const String NM_LENGTHAFTERDEMPOINT = "LengthAfterDemPoint";
        ///// <summary>
        ///// 相関係数A 
        ///// </summary>
        //public const String NM_GAINOFCORRELATION = "GainOfCorrelation";
        ///// <summary>
        ///// 相関係数B
        ///// </summary>
        //public const String NM_OFFSETOFCORRELATION = "OffsetOfCorrelation";           
        /// <summary>
        /// 測定ポイント 
        /// </summary>
        public const String NM_CALIBMEASPOINTOFEACH = "CalibMeasPointOfEach{0}";
        /// <summary>
        /// 分析項目使用フラグ 
        /// </summary>
        public const String NM_DISPLAYPROTOCOL = "DisplayProtocol";
        /// <summary>
        /// 自動希釈倍率演算可否
        /// </summary>
        public const String NM_USEAFTERDILATCALCU = "UseAfterDilAtCalcu";
        /// <summary>
        /// 手希釈倍率演算可否 
        /// </summary>
        public const String NM_USEMANUALDILATCALCU = "UseManualDilAtCalcu";
        /// <summary>
        ///  自動希釈再検使用有無 
        /// </summary>
        public const String NM_USEAFTERDIL = "UseAfterDil";
        /// <summary>
        ///  自動再検使用有無 
        /// </summary>
        public const String NM_USEAUTORETEST = "UseAutoReTest";
        /// <summary>
        ///  自動希釈再検条件　上限 
        /// </summary>
        public const String NM_AUTODILUTIONRETEST_MAX = "AutoDilutionReTest(Max)";
        /// <summary>
        ///  自動希釈再検条件　下限 
        /// </summary>
        public const String NM_AUTODILUTIONRETEST_MIN = "AutoDilutionReTest(Min)";
        /// <summary>
        /// 自動希釈再検条件(希釈倍率) 
        /// </summary>
        public const String NM_AUTODILUTIONRETESTRATIO = "AutoDilutionReTestRatio";
        /// <summary>
        /// 自動再検条件　上限 
        /// </summary>
        public const String NM_AUTORETEST_MAX = "AutoReTest(Max)";
        /// <summary>
        /// 自動再検条件　下限 
        /// </summary>
        public const String NM_AUTORETEST_MIN = "AutoReTest(Min)";

        /// <summary>
        /// 重测部分低值区域 
        /// </summary>
        public const string NM_RETESTRANGE_LOW = "RetestRange(Low)";
        /// <summary>
        /// 重测部分中值（灰区）区域 
        /// </summary>
        public const string NM_RETESTRANGE_MIDDLE = "RetestRange(Middle)";
        /// <summary>
        /// 重测部分高值区域 
        /// </summary>
        public const string NM_RETESTRANGE_HIGH = "RetestRange(High)";
		/// <summary>
		/// 試薬開封後有効期間
		/// </summary>
		public const String NM_DAY_OF_REAGENT_VALID = "DayOfReagentValid";

        public const String NM_USECVINDEPENDENCE = "UseCVIndependence";

        public const String NM_CVOFEACHPOINT = "CVofEachPoint{0}";	


        //4参数加权K值

        public const String NM_FOURPRAMETERMETHODKVALUE = "FourPrameterMethodKValue";

        //4参数加权类型

        public const String NM_FOURPRAMETERMETHODTYPE = "FourPrameterMethodType";

        //校准品、质控品是否稀释

        public const String NM_DILUCALIBORCONTROL = "DILUCALIBORCONTROL";

        public const String NM_ISIGRA = "IsIGRA";

        public const String NM_TURNORDER = "TurnOrder";
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 読込開始列
        /// </summary>
        public Int32 SettingStartColumn
        {
            get;
            set;
        }

        /// <summary>
        /// エラー番号情報
        /// </summary>
        private Dictionary<String, String> errorNoList = new Dictionary<String, String>();
        public Dictionary<String,String> ErrorNoList
        {
            get
            {
                return errorNoList;
            }
            set
            {
                errorNoList = value;
            }
        }

        /// <summary>
        /// 読込定義
        /// </summary>
        private List<ConvertList> convertList = new List<ConvertList>();
        public List<ConvertList> ConvertList
        {
            get
            {
                return convertList;
            }
            set
            {
                convertList = value;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 行番号取得
        /// </summary>
        /// <remarks>
        /// 引数[name]に指定された名称より取得した
        /// 行番号を返します。
        /// </remarks>
        /// <param name="name"></param>
        /// <returns></returns>
        public String  GetRowNo(String name)
        {
            var rtn = from m in ConvertList
                         where m.Name == name
                         select m.RowNo;

            return Convert.ToInt32(rtn.First()).ToString();
        }

        /// <summary>
        /// ListのIndexを取得
        /// </summary>
        /// <remarks>
        /// 引数[name]に指定された名称より取得した、
        /// 配列のインデックスを返します。
        /// </remarks>
        /// <param name="name"></param>
        /// <returns></returns>
        public Int32 GetListIndex( String name )
        {
            var rtn = from m in ConvertList
                      where m.Name == name
                      select m.RowNo;

            return Convert.ToInt32( rtn.First() ) - 1;
        }
        #endregion

    }

    ///// <summary>
    ///// プログラムの設定定義
    ///// </summary>
    //public class ConvertXmlSetting
    //{
    //    /// <summary>
    //    /// 読み込み開始列番号
    //    /// </summary>
    //    public Int32 StartColumn
    //    {
    //        get;
    //        set;
    //    }
    //}

    ///// <summary>
    ///// エラー番号定義
    ///// </summary>
    //public class ConvertXmlErrorList
    //{
    //    /// <summary>
    //    /// エラー情報KEY
    //    /// </summary>
    //    public String ErrKey
    //    {
    //        get;
    //        set;
    //    }
    //    /// <summary>
    //    /// エラー番号
    //    /// </summary>
    //    public Int32 ErrNo
    //    {
    //        get;
    //        set;
    //    }
    //}

    /// <summary>
    /// 読込定義
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class ConvertList
    {
       /// <summary>
       /// 行番号
       /// </summary>
        public Int32 RowNo
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// チェック情報
        /// </summary>
        private List<DataCheckerTypeCheck> validation = new List<DataCheckerTypeCheck>();
        public List<DataCheckerTypeCheck> Validation
        {
            get
            {
                return validation;
            }
            set
            {
                validation = value;
            }
        }

    }
}
