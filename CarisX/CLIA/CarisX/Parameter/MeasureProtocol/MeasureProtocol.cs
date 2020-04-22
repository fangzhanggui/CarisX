using System;
using System.Collections.Generic;
using System.Linq;

using System.Xml.Serialization;

using Oelco.CarisX.Const;
using Oelco.Common.Parameter;

namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// 分析項目クラス
    /// </summary>
    public class MeasureProtocol : AttachmentParameter, ISavePath
	{

        //检测顺序
        public Int32 TurnOrder = 0;

        /// <summary>
        /// 是否是IGRA项目
        /// </summary>
        public Boolean IsIGRA = false;
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
        /// プロトコル希釈倍率
        /// </summary>
        public DilutionRatioKind ProtocolDilutionRatio = DilutionRatioKind.x1;
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
        /// 【IssuesNo:7】质控品相关系数A
        /// </summary>
        public Double ControlGainOfCorrelation = 1;

        /// <summary>
        /// 【IssuesNo:7】质控品相关系数B
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
        /// 对于每个点进行独立的CV控制
        /// </summary>
        public Double[] CVofEachPoint = new double[8];
        /// <summary>
        /// 是否使用浓度点不同，ＣＶ判断的不同
        /// </summary>
        public Boolean UseCVIndependence = false;
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


// TODO:以下使っている個所あるが本当にあってるのか不明。by KUSU

        /// <summary>
        /// 分析項目使用フラグ（ルーチンテーブルにて選択された）
        /// </summary>
        public Boolean DisplayProtocol;

        /// <summary>
        /// 4参数法K值
        /// </summary>
        public Int32 FourPrameterMethodKValue = 0;

        // 4参数法类型

        public Int32 FourPrameterMethodType = 0;


        //校准品、质控品是否稀释

        public Int32 DiluCalibOrControl = 0;
       

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
		/// <summary>
		/// 試薬開封後有効期間
		/// </summary>
		public Int32 DayOfReagentValid = 365;

        /// <summary>
        /// 急診有無
        /// </summary>
        public Boolean UseEmergencyMode = false;

        /// <summary>
        /// R1ユニットの分注順逆転
        /// </summary>
        public Boolean ReverseDispensingOrderR1 = false;

        /// <summary>
        /// 分析シーケンス
        /// </summary>
        public enum AssaySequenceKind
        {
            /// <summary>
            /// 1ステップ法
            /// </summary>
            OneStep = 1,
            /// <summary>
            /// 2ステップ法
            /// </summary>
            TwoStep = 2,
            /// <summary>
            /// 2ステップ法マイナス
            /// </summary>
            TwoStepMinus = 3,
            /// <summary>
            /// 1.5ステップ法
            /// </summary>
            OnePointFiveStep = 4,

            /// <summary>
            /// 希釈2ステップ法
            /// </summary>
            DilutionTwoStep = 5,            //画面上からは削除だが、互換性の為にEnum値は残す。ロードされた場合、2ステップ法(x10)扱いにする
            /// <summary>
            /// 希釈2ステップ法マイナス
            /// </summary>
            DilutionTwoStepMinus = 6,       //画面上からは削除だが、互換性の為にEnum値は残す。ロードされた場合、2ステップ法マイナス(x10)扱いにする

            /// <summary>
            /// 1.5ステップ法（RM）
            /// </summary>
            OnePointFiveStepRM = 7,

            /// <summary>
            /// 希釈2ステップ法(1000倍)
            /// </summary>
            Dilution1000TwoStepMinus = 8,   //画面上からは削除だが、互換性の為にEnum値は残す。ロードされた場合、2ステップ法(x1000)扱いにする
            /// <summary>
            /// 希釈2ステップ法(400倍)
            /// </summary>
            Dilution400TwoStep = 9,         //画面上からは削除だが、互換性の為にEnum値は残す。ロードされた場合、2ステップ法(x400)扱いにする
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
            /// 10倍
            /// </summary>
            x10 = 10,
            /// <summary>
            /// 20倍
            /// </summary>
            x20 = 20,
            /// <summary>
            /// 100倍
            /// </summary>
            x100 = 100,
            /// <summary>
            /// 200倍
            /// </summary>
            x200 = 200,
            /// <summary>
            /// 400倍
            /// </summary>
            x400 = 400,
            /// <summary>
            /// 1000倍
            /// </summary>
            x1000 = 1000,
            /// <summary>
            /// 2000倍
            /// </summary>
            x2000 = 2000,
            /// <summary>
            /// 4000倍
            /// </summary>
            x4000 = 4000,
            /// <summary>
            /// 8000倍
            /// </summary>
            x8000 = 8000,
        }

        /// <summary>
        /// 希釈倍率
        /// </summary>
        public enum DilutionRatioKind : int
        {
            /// <summary>
            /// 等倍
            /// </summary>
            x1 = 1,
            /// <summary>
            /// 10倍
            /// </summary>
            x10 = 10,
            /// <summary>
            /// 20倍
            /// </summary>
            x20 = 20,
            /// <summary>
            /// 100倍
            /// </summary>
            x100 = 100,
            /// <summary>
            /// 200倍
            /// </summary>
            x200 = 200,
            /// <summary>
            /// 400倍
            /// </summary>
            x400 = 400,
            /// <summary>
            /// 1000倍
            /// </summary>
            x1000 = 1000,
            /// <summary>
            /// 2000倍
            /// </summary>
            x2000 = 2000,
            /// <summary>
            /// 4000倍
            /// </summary>
            x4000 = 4000,
            /// <summary>
            /// 8000倍
            /// </summary>
            x8000 = 8000,
        }

        /// <summary>
        /// 自動希釈倍率
        /// </summary>
        /// <remarks>
        /// プロトコル自体には設定されず、検体の登録時に設定する
        /// </remarks>
        public enum AutoDilutionRatioKind : int
        {
            /// <summary>
            /// 等倍
            /// </summary>
            x1 = 1,
            /// <summary>
            /// 10倍
            /// </summary>
            x10 = 10,
            /// <summary>
            /// 20倍
            /// </summary>
            x20 = 20,
            /// <summary>
            /// 100倍
            /// </summary>
            x100 = 100,
            /// <summary>
            /// 200倍
            /// </summary>
            x200 = 200,
            /// <summary>
            /// 400倍
            /// </summary>
            x400 = 400,
            /// <summary>
            /// 1000倍
            /// </summary>
            x1000 = 1000,
            /// <summary>
            /// 2000倍
            /// </summary>
            x2000 = 2000,
            /// <summary>
            /// 4000倍
            /// </summary>
            x4000 = 4000,
            /// <summary>
            /// 8000倍
            /// </summary>
            x8000 = 8000,
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
            for (Int32 i = 0; i < CountRangesOfEach.Length; i++ )
            {
                CountRangesOfEach[i] = new ItemRange();
            }

            // 読込、保存パスを設定
            SetSaveProtocolPath();
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 読込、保存パスをDefaultにする
        /// </summary>
        /// <remarks>
        /// 読込、保存パスをDefaultに設定します
        /// </remarks>
        public void SetSaveProtocolPathDefault()
        {
            this.saveProtocolPath = CarisXConst.PathDefaultProtocol;
        }

        /// <summary>
        /// 読込、保存パスを本来の位置に戻す
        /// </summary>
        /// <remarks>
        /// 読込、保存パスを分析設定保存パスに設定します
        /// </remarks>
        public void SetSaveProtocolPath()
        {
            this.saveProtocolPath = CarisXConst.PathProtocol;
        }

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

        /// <summary>
        /// 残量⇒テスト数変換
        /// </summary>
        /// <remarks>
        /// パラメータに従って残量の計算を行います。
        /// </remarks>
        /// <param name="amount">R1,R2,M,T1,T2を統合した残量</param>
        /// <returns>テスト回数</returns>
        public Int32 ConvertReagentRemainToTestCount( Int32 amount )
        {
            return this.ConvertReagentRemainToTestCount( amount, amount, amount, amount, amount );
        }

        /// <summary>
        /// 残量⇒テスト数変換
        /// </summary>
        /// <remarks>
        /// パラメータに従って残量の計算を行います。
        /// </remarks>
        /// <param name="r1Reagent">R1試薬残量</param>
        /// <param name="r2Reagent">R2試薬残量</param>
        /// <param name="mReagent">M試薬残量</param>
        /// <param name="preProcess1">前処理液1残量</param>
        /// <param name="preProcess2">前処理液2残量</param>
        /// <returns>テスト回数</returns>
        public Int32 ConvertReagentRemainToTestCount( Int32 r1Reagent, Int32 r2Reagent, Int32 mReagent, Int32 preProcess1, Int32 preProcess2 )
        {
            Int32 testCount = 0;
                try
                {
                    System.Collections.Generic.List<Int32> dispenceList = new List<Int32>()
                    {
                        ( r1Reagent / this.R1DispenseVolume ),
                        ( r2Reagent / this.R2DispenseVolume ),
                        ( mReagent / this.MReagDispenseVolume),                       
#if false // 対応保留
                    ( preProcess1 / protocol.PreProsess1DispenseVolume );
                    ( preProcess2 / protocol.PreProsess2DispenseVolume );
#endif
                   };
                    testCount = dispenceList.Min();
                }
                catch ( Exception ex )
                {
                    Oelco.Common.Utility.Singleton<Oelco.CarisX.Log.CarisXLogManager>.Instance.Write( Oelco.Common.Log.LogKind.DebugLog, Oelco.Common.Utility.Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                                Oelco.CarisX.Log.CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
                }
            return testCount;
        }

        /// <summary>
        /// 正常に読込できている分析項目パラメータか判定
        /// </summary>
        /// <returns></returns>
        public bool IsError()
        {
            // 分析項目番号が「0」はあり得ない
            if (this.ProtocolNo == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 旧アッセイシーケンスから新アッセイシーケンスとそれに伴った希釈倍率に変換
        /// </summary>
        public void ConvertAssaySequenceAndDilutionRatio()
        {
            // アッセイシーケンスをチェック
            AssaySequenceKind check = this.AssaySequence;
            switch (check)
            {
                // 1) 1ステップ法
                case AssaySequenceKind.OneStep:
                // 2) 2ステップ法
                case AssaySequenceKind.TwoStep:
                // 3) 2ステップ法マイナス
                case AssaySequenceKind.TwoStepMinus:
                // 4) 1.5ステップ法
                case AssaySequenceKind.OnePointFiveStep:
                // 7) 1.5ステップ法（RM）
                case AssaySequenceKind.OnePointFiveStepRM:
                    break;

                // 5) 希釈2ステップ法
                case AssaySequenceKind.DilutionTwoStep:
                    this.AssaySequence = AssaySequenceKind.TwoStep;         // 2ステップ法
                    this.ProtocolDilutionRatio = DilutionRatioKind.x10;     // x10
                    break;
                // 6) 希釈2ステップ法マイナス
                case AssaySequenceKind.DilutionTwoStepMinus:
                    this.AssaySequence = AssaySequenceKind.TwoStepMinus;    // 2ステップ法マイナス 
                    this.ProtocolDilutionRatio = DilutionRatioKind.x10;     // x10
                    break;
                // 8) 希釈2ステップ法(1000倍)
                case AssaySequenceKind.Dilution1000TwoStepMinus:
                    this.AssaySequence = AssaySequenceKind.TwoStep;         // 2ステップ法
                    this.ProtocolDilutionRatio = DilutionRatioKind.x1000;   // x1000
                    break;
                // 9) 希釈2ステップ法(400倍)
                case AssaySequenceKind.Dilution400TwoStep:
                    this.AssaySequence = AssaySequenceKind.TwoStep;         // 2ステップ法
                    this.ProtocolDilutionRatio = DilutionRatioKind.x400;    // x400
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region ISavePath メンバー

        /// <summary>
        /// 保存パス
        /// </summary>
        public String SavePath
        {
            get
            {
                return String.Format(@"{0}\MeasProtocol{1:00}.xml", this.saveProtocolPath, this.ProtocolIndex);
            }
        }

        #endregion

    }
	 
}
 
