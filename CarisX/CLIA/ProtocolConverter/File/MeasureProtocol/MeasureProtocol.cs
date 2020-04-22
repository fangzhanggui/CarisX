using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ProtocolConverter.File;

namespace ProtocolConverter.File
{

    /// <summary>
    /// 分析項目定義クラス
    /// </summary>
    /// <remarks>
    /// 分析項目XMLファイルの定義クラスです。
    /// </remarks>
    public class MeasureProtocol : AttachmentParameter, ISavePath
	{
        /// <summary>
        /// 検体多重測定回数
        /// </summary>
        public Int32 RepNoForSample;
        /// <summary>
        /// 精度管理検体多重測定回数
        /// </summary>
        public Int32 RepNoForControl;
        /// <summary>
        /// キャリブレータ多重測定回数
        /// </summary>
        public Int32 RepNoForCalib;
        /// <summary>
        /// 検量線有効期間
        /// </summary>
        public Double ValidityOfCurve;
        /// <summary>
        /// 陽性判定閾値
        /// </summary>
        public Double PosiLine;
        /// <summary>
        /// 陰性判定閾値
        /// </summary>
        public Double NegaLine;
        /// <summary>
        /// 分析項目番号
        /// </summary>
        public Int32 ProtocolNo = 0;
        /// <summary>
        /// 分析項目インデックス
        /// </summary>
        public Int32 ProtocolIndex;
        /// <summary>
        /// 分析項目名称
        /// </summary>
        public String ProtocolName = String.Empty;
        /// <summary>
        /// 試薬名称
        /// </summary>
        public String ReagentName = String.Empty;
        /// <summary>
        /// アッセイシーケンス
        /// </summary>
        public AssaySequenceKind AssaySequence = AssaySequenceKind.OneStep;
        /// <summary>
        /// 前処理シーケンス
        /// </summary>
        public PreProcessSequenceKind PreProcessSequence = PreProcessSequenceKind.None;
        /// <summary>
        /// サンプル種別
        /// </summary>
        public SampleTypeKind SampleKind = SampleTypeKind.SerumOrPlasma;
        /// <summary>
        /// 自動希釈再検使用有無
        /// </summary>
        public Boolean UseAfterDil;
        /// <summary>
        /// 自動再検使用有無
        /// </summary>
        public Boolean UseAutoReTest;
        /// <summary>
        /// 自動希釈再検条件
        /// </summary>
        public ItemRange AutoDilutionReTest = new ItemRange();
        /// <summary>
        /// 自動希釈再検条件(希釈倍率)
        /// </summary>
        public AutoDilutionReTestRatioKind AutoDilutionReTestRatio = AutoDilutionReTestRatioKind.x10;
        /// <summary>
        /// 自動再検条件
        /// </summary>
        public ItemRange AutoReTest = new ItemRange();
        /// <summary>
        /// 重测范围
        /// </summary>
        public ItemRetestRange RetestRange = new ItemRetestRange();
        /// <summary>
        /// 手希釈使用有無
        /// </summary>
        public Boolean UseManualDil;
        /// <summary>
        /// 試薬コード
        /// </summary>
        public Int32 ReagentCode;
        /// <summary>
        /// サンプル分注量
        /// </summary>
        public Int32 SmpDispenseVolume;
        /// <summary>
        /// M試薬分注量
        /// </summary>
        public Int32 MReagDispenseVolume;
        /// <summary>
        /// R1試薬分注量
        /// </summary>
        public Int32 R1DispenseVolume;
        /// <summary>
        /// R2試薬分注量
        /// </summary>
        public Int32 R2DispenseVolume;
        /// <summary>
        /// 前処理液1分注量
        /// </summary>
        public Int32 PreProsess1DispenseVolume;
        /// <summary>
        /// 前処理液2分注量
        /// </summary>
        public Int32 PreProsess2DispenseVolume;
        /// <summary>
        /// キャリブレーションタイプ
        /// </summary>
        public CalibrationType CalibType = CalibrationType.Spline;
        /// <summary>
        /// 相関係数A
        /// </summary>
        public Double GainOfCorrelation = 1;
        /// <summary>
        /// 相関係数B
        /// </summary>
        public Double OffsetOfCorrelation;
        /// <summary>
        /// 【IssuesNo:1】质控品相关系数A
        /// </summary>
        public Double ControlGainOfCorrelation = 1;

        /// <summary>
        /// 【IssuesNo:1】质控品相关系数B
        /// </summary>
        public Double ControlOffsetOfCorrelation;
        /// <summary>
        /// キャリブレーション方法
        /// </summary>
        public CalibrationMethod CalibMethod = CalibrationMethod.FullCalibration;
        /// <summary>
        /// キャリブレーションポイント数
        /// </summary>
        public Int32 NumOfMeasPointInCalib;
        /// <summary>
        /// 濃度
        /// </summary>
        public Double[] ConcsOfEach = new Double[8];
        /// <summary>
        /// 測定ポイント
        /// </summary>
        public Boolean[] CalibMeasPointOfEach = new Boolean[8];
        /// <summary>
        /// カウントチェック範囲
        /// </summary>
        public ItemRange[] CountRangesOfEach = new ItemRange[8];
        /// <summary>
        /// 濃度単位
        /// </summary>
        public String ConcUnit = String.Empty;
        /// <summary>
        /// 濃度値小数点以下桁数
        /// </summary>
        public Int32 LengthAfterDemPoint;
        /// <summary>
        /// 濃度ダイナミックレンジ
        /// </summary>
        public ItemRange ConcDynamicRange = new ItemRange();
        /// <summary>
        /// 多重測定内乖離限界CV%
        /// </summary>
        public Double MulMeasDevLimitCV;
        /// <summary>
        /// 分析項目使用フラグ（ルーチンテーブルにて選択された）
        /// </summary>
        public Boolean DisplayProtocol;

        //// 項目パラメータ
        //public Double CoefAOfLog;
        //public Double CoefBOfLog;

        ///// <summary>
        ///// サンプル表示容量
        ///// </summary>
        //public Int32 SmpDispVol;

        /// <summary>
        /// 自動希釈倍率演算可否
        /// </summary>
        public Boolean UseAfterDilAtCalcu;
        /// <summary>
        /// 手希釈倍率演算可否
        /// </summary>
        public Boolean UseManualDilAtCalcu;

        // 抑制率タイプ
        ///// <summary>
        ///// 抑制率上限値
        ///// </summary>
        //public Double INHCutOffForPos;   // 抑制率上限値
        ///// <summary>
        ///// 抑制率下限値
        ///// </summary>
        //public Double INHCutOffForNega;  // 抑制率下限値

        /// <summary>
        /// 係数A 
        /// </summary>
        public Double Coef_A;
        /// <summary>
        /// 係数B
        /// </summary>
        public Double Coef_B;
        /// <summary>
        /// 係数C
        /// </summary>
        public Double Coef_C;
        /// <summary>
        /// 係数D
        /// </summary>
        public Double Coef_D;
        /// <summary>
        /// 係数E
        /// </summary>
        public Double Coef_E;

        /// 对于每个点进行独立的CV控制
        /// </summary>
        public Double[] CVofEachPoint = new double[8];
        /// <summary>
        /// 是否使用浓度点不同，ＣＶ判断的不同
        /// </summary>
        public Boolean UseCVIndependence = false;

        /// <summary>
        /// 4参数法K值
        /// </summary>
        public Int32 FourPrameterMethodKValue = 0;

        // 4参数法类型

        public Int32 FourPrameterMethodType = 0;

        //校准品、自控品是否稀释

        public Int32 DiluCalibOrControl = 0;

        /// <summary>
        /// 是否是TB -IGRA项目
        /// </summary>
        public Boolean IsIGRA = false;
        
        /// <summary>
        /// 检测顺序
        /// </summary>
        public Int32 TurnOrder = 0;

		/// <summary>
		/// 試薬開封後有効期間
		/// </summary>
		public Int32 DayOfReagentValid;

        /// <summary>
        /// 分析シーケンス
        /// </summary>
        public enum AssaySequenceKind
        {
            /// <summary>
            /// 1ステップ法 =1
            /// </summary>
            OneStep = 1,
            /// <summary>
            /// 2ステップ法 =2
            /// </summary>
            TwoStep,
            /// <summary>
            /// 2ステップ法マイナス=3
            /// </summary>
            TwoStepMinus,
            /// <summary>
            /// 1.5ステップ法=4
            /// </summary>
            OnePointFiveStep,
            /// <summary>
            /// 希釈2ステップ法=5
            /// </summary>
            DilutionTwoStep,
            /// <summary>
            /// 希釈2ステップ法マイナス=6
            /// </summary>
            DilutionTwoStepMinus,
			/// <summary>
            /// 1.5ステップ法（RM）=7
			/// </summary>
			OnePointFiveStepRM,
            /// <summary>
            /// 稀释1000倍两步法负=8
            /// </summary>
            Dilution1000TwoStepMinus,
            ///// <summary>
            ///// 稀释8000倍两步法=9
            ///// </summary>
            //Dilution8000TwoStep,
            /// <summary>
            /// 稀释400倍两步法=9
            /// </summary>
            Dilution400TwoStep,
        }

        /// <summary>
        /// 前処理シーケンス
        /// </summary>
        public enum PreProcessSequenceKind
        {
            /// <summary>
            /// なし
            /// </summary>
            None = 0,
            /// <summary>
            /// sR1タイプ
            /// </summary>
            SR1,
            /// <summary>
            /// sT1タイプ
            /// </summary>
            ST1,
            /// <summary>
            /// sT1T2タイプ
            /// </summary>
            ST1T2,
            /// <summary>
            /// sT1sR1タイプ
            /// </summary>
            ST1SR1,
            /// <summary>
            /// sT1sT2タイプ
            /// </summary>
            ST1ST2        
        }

        /// <summary>
        /// サンプル種
        /// </summary>
        [Flags]
        public enum SampleTypeKind : int
        {
            /// <summary>
            /// なし
            /// </summary>
            None = 0x00,
            /// <summary>
            /// 血清または血漿
            /// </summary>
            SerumOrPlasma = 0x01,
            /// <summary>
            /// 尿
            /// </summary>
            Urine = 0x02
        }

        /// <summary>
        /// 自動希釈再検条件希釈倍率
        /// </summary>
        public enum AutoDilutionReTestRatioKind : int
        {
            /// <summary>
            /// ×10
            /// </summary>
            x10 = 10,
            /// <summary>
            /// ×100
            /// </summary>
            x100 = 100,
            /// <summary>
            /// ×200
            /// </summary>
            x200 = 200,
            /// <summary>
            /// ×1000
            /// </summary>
            x1000 = 1000  
        }

        public enum AutoDilutionRatioKind : int
        {
            /// <summary>
            /// ×1
            /// </summary>
            x1 = 1,
            /// <summary>
            /// ×10
            /// </summary>
            x10 = 10,
            /// <summary>
            /// ×100
            /// </summary>
            x100 = 100,
            /// <summary>
            /// ×200
            /// </summary>
            x200 = 200,
            /// <summary>
            /// ×1000
            /// </summary>
            x1000 = 1000  

        }

        /// <summary>
        /// キャリブレーションタイプ
        /// </summary>
        public enum CalibrationType
        {
            /// <summary>
            /// Spline
            /// </summary>
            Spline = 1,
            /// <summary>
            /// Logit-Log　3次回帰
            /// </summary>
            LogitLog,
            /// <summary>
            /// 4 Parameters
            /// </summary>
            FourParameters, 
            /// <summary>
            /// カットオフタイプ
            /// </summary>
            CutOff,         
            /// <summary>
            /// 抑制率タイプ
            /// </summary>
            INH,
			/// <summary>
			/// 両対数1次
			/// </summary>
			DoubleLogarithmic1,
			/// <summary>
			/// 両対数2次
			/// </summary>
			DoubleLogarithmic2,
            /// <summary>
            /// なし
            /// </summary>
            NoType
        }

        /// <summary>
        /// キャリブレーション方法
        /// </summary>
        public enum CalibrationMethod
        {
            /// <summary>
            /// フルキャリブレーション
            /// </summary>
            FullCalibration = 0,
            /// <summary>
            /// マスタキャリブレーション
            /// </summary>
            MasterCalibration,
        }

        /// <summary>
        /// 範囲値
        /// </summary>
        public class ItemRange
        {
            /// <summary>
            /// 範囲値最大
            /// </summary>
            public Double Max = 0;
            /// <summary>
            /// 範囲値最小
            /// </summary>
            public Double Min = 0;

            //public ItemRange(Double max, Double min)
            //{
            //    Max = max;
            //    Min = min;
            //}
        }

         /// <summary>
        /// 重测范围区域选择
        /// </summary>
        public class ItemRetestRange
        {
            /// <summary>
            ///  低值
            /// </summary>
            public bool UseLow = false;
            /// <summary>
            /// 中间范围（灰区）
            /// </summary>
            public bool UseMiddle = false;
            /// <summary>
            /// 高值范围
            /// </summary>
            public bool UseHigh = false;            
        }

        #region [インスタンス変数定義]

        /// <summary>
        /// 読込、保存パス
        /// </summary>
        private String saveProtocolPath;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MeasureProtocol()
        {
        }

        #endregion

        #region [publicメソッド]       

        /// <summary>
        /// 読込、保存パスを指定
        /// </summary>
        /// <remarks>
        /// 読込、保存パスを指定パスに設定します
        /// </remarks>
        /// <param name="path">読込、保存パス</param>
        public void SetSaveProtocolPath(String path)
        {
            this.saveProtocolPath = path;
        }

        #endregion

        #region ISavePath メンバー
        /// <summary>
        /// 保存パス
        /// </summary>
        [XmlIgnore()]
        public String SavePath
        {
            get
            {
                return String.Format(@"{0}\MeasProtocol{1:00}.xml", this.saveProtocolPath, this.ProtocolIndex);
            }
            set
            {
                this.saveProtocolPath = value;
            }
        }

        #endregion

    }

    /// <summary>
    /// Enumの拡張メソッドクラス
    /// </summary>
    public static class EnumExtention
    {
        /// <summary>
        /// 値を文字型に変換し取得する
        /// </summary>
        /// <remarks>
        /// 値を文字列に変換し返します。
        /// </remarks>
        /// <param name="type">キャリブレーションタイプ</param>
        /// <returns>キャリブレーションタイプ文字列</returns>
		public static String ValueToString(this MeasureProtocol.CalibrationType type)
        {
            return Convert.ToInt32( type ).ToString();
        }
    }

	 
}
 
