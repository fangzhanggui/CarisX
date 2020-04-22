using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Oelco.CarisX.Utility
{
    /// <summary>
    /// リマークユーティリティ
    /// </summary>
    /// <remarks>
    /// リマークの処理に関連する機能を提供します。
    /// </remarks>
    public class Remark
    {
        #region [定数定義]
        /// <summary>
        /// リマークビットデフォルト
        /// </summary>
        public const Int64 REMARK_DEFAULT = 0;
        /// <summary>
        /// リマークビット定義
        /// </summary>
        [Flags]
        public enum RemarkBit : long
        {
            /// <summary>
            /// Ｍ試薬吸引エラー 
            /// </summary>
            MReagentSuckingUpError = 0x0000000000000001,
            /// <summary>
            /// Ｒ１試薬吸引エラー
            /// </summary>
            R1ReagentSuckingUpError = 0x0000000000000002,
            /// <summary>
            /// Ｒ２試薬吸引エラー
            /// </summary>
            R2ReagentSuckingUpError = 0x0000000000000004,
            /// <summary>
            /// 前処理液吸引エラー
            /// </summary>
            PreProcessLiquidSuckingUpError = 0x0000000000000008,
            /// <summary>
            /// サンプルなしエラー
            /// </summary>
            NoSampleError = 0x0000000000000010,
            /// <summary>
            /// サンプル詰まりエラー
            /// </summary>
            SampleStoppedUpError = 0x0000000000000020,
            /// <summary>
            /// サンプル空吸エラー
            /// </summary>
            NotSampleSuckingUpError = 0x0000000000000040,
            /// <summary>
            /// サンプル分注エラー
            /// </summary>
            SampleDispenseError = 0x0000000000000080,
            /// <summary>
            /// 希釈液なしエラー
            /// </summary>
            NoDilutionError = 0x0000000000000100,
            /// <summary>
            /// 洗浄不良エラー
            /// </summary>
            WashingFailureError = 0x0000000000000200,
            /// <summary>
            /// 光学系エラー
            /// </summary>
            DetectorError = 0x0000000000000400,
            /// <summary>
            /// サンプル分注チップ装着エラー
            /// </summary>
            SampleDispenseTipSetError = 0x0000000000000800,
            /// <summary>
            /// サンプル分注チップ廃棄エラー
            /// </summary>
            SampleDispenseTipDisposalError = 0x0000000000001000,
            /// <summary>
            /// ダークエラー
            /// </summary>
            DarkError = 0x0000000000002000,
            /// <summary>
            /// 測光エラー
            /// </summary>
            PhotometryError = 0x0000000000004000,
            /// <summary>
            /// 免疫反応槽部温度エラー
            /// </summary>
            TempOfImmunoreactionError = 0x0000000000008000,
            /// <summary>
            /// B/F1プレヒート部温度エラー
            /// </summary>
            TempOfBF1PreHeatError = 0x0000000000010000,
            /// <summary>
            /// B/F2プレヒート部温度エラー
            /// </summary>
            TempOfBF2PreHeatError = 0x0000000000020000,
            /// <summary>
            /// R1プローブプレヒート部温度エラー
            /// </summary>
            TempOfR1ProbePreHeatError = 0x0000000000040000,
            /// <summary>
            /// R2プローブプレヒート部温度エラー
            /// </summary>
			TempOfR2ProbePreHeatError = 0x0000000000080000,
			/// <summary>
			/// 試薬有効期限エラー
			/// </summary>
			ReagentExpirationDateError = 0x0000000000100000,
			/// <summary>
			/// 希釈液有効期限エラー
			/// </summary>
			DilutionExpirationDateError = 0x0000000000200000,
            /// <summary>
            /// フォトマル部温度エラー
            /// </summary>
            TempOfDetectorError = 0x0000000000400000,
            /// <summary>
            /// 試薬保冷庫温度エラー
            /// </summary>
            TempOfReagentCoolerError = 0x0000000000800000,
            /// <summary>
            /// 希釈液保冷庫温度エラー
            /// </summary>
            TempOfDilutionCoolerError = 0x0000000001000000,
            /// <summary>
            /// サイクルタイムオーバー
            /// </summary>
            CycleTimeOverError = 0x0000000002000000,
            /// <summary>
            /// 未測定（ラック強制排出による）
            /// </summary>
            NoMeasuredError = 0x0000000004000000,
            /// <summary>
            /// 反応容器搬送エラー
            /// </summary>
			ReactionVesselCarryError = 0x0000000008000000,
			/// <summary>
			/// プレトリガ有効期限エラー
			/// </summary>
			PreTriggerExpirationDateError = 0x0000000010000000,
            /// <summary>
            /// プレトリガ部温度エラー
            /// </summary>
            TempOfPreTriggertemperatureError = 0x0000000020000000,
            /// <summary>
            /// トリガ有効期限エラー
            /// /// </summary>
            TriggerExpirationDateError = 0x0000000040000000,

            // <summary>
            // 空き
            // </summary>

            /// <summary>
            /// 検量線エラー
            /// </summary>
			CalibrationCurveError = 0x0000000100000000,

            // <summary>
            // 予約
            // </summary>
            
            /// <summary>
            /// 検量線有効期限エラー
            /// </summary>
            CalibExpirationDateError = 0x0000002000000000,
            /// <summary>
            /// 多重測定時のデータ間乖離許容範囲外エラー
            /// </summary>
            DiffError = 0x0000004000000000,
            /// <summary>
            /// 濃度値算出不能エラー
            /// </summary>
            CalcConcError = 0x0000008000000000,
            /// <summary>
            /// キャリブレーション正常範囲外エラー
            /// </summary>
            CalibError = 0x0000010000000000,
            /// <summary>
            /// ダイナミックレンジ正常範囲外エラー／上限値オーバー
            /// </summary>
            DynamicrangeUpperError = 0x0000020000000000,
            /// <summary>
            /// ダイナミックレンジ正常範囲外エラー／下限値オーバー
            /// </summary>
            DynamicrangeLowerError = 0x0000040000000000,
            /// <summary>
            /// 管理値範囲外エラー（Xバー管理図）
            /// </summary>
            ControlRangeError = 0x0000080000000000,
            /// <summary>
            /// 精度管理判定不能
            /// </summary>
            ControlError = 0x0000100000000000,
            /// <summary>
            /// データ編集（手希釈倍率修正）
            /// </summary>
            EditOfManualDil = 0x0000200000000000,
            /// <summary>
            /// データ編集（再計算）
            /// </summary>
            EditOfReCalcu = 0x0000400000000000,
            /// <summary>
            /// データ編集（検量線カウント修正）
            /// </summary>
            EditOfCalibCount = 0x0000800000000000,
            /// <summary>
            /// データ編集（修正された検量線で再計算）
            /// </summary>
            EditOfReCalcuByEditCurve = 0x0001000000000000,
            /// <summary>
            /// データ編集（精度管理濃度修正）
            /// </summary>
            EditOfControlConc = 0x0002000000000000,

            // <summary>
            // 予約
            // </summary>

            /// <summary>
            /// 測定情報異常エラー
            /// </summary>
            WorksheetError = 0x1000000000000000,
            /// <summary>
            /// 撹拌異常エラー
            /// </summary>
            MixingError = 0x2000000000000000,
            /// <summary>
            /// 競合異常エラー
            /// </summary>
            ConflictError = 0x4000000000000000
        }

        /// <summary>
        /// リマーク判定ビット
        /// </summary>
        /// <remarks>
        /// 計算、再計算、再計算登録等で使用する判定用フラグ
        /// </remarks>
        [Flags]
        public enum RemarkCriterion : long
        {
            /// <summary>
            /// カウント値算出可能フラグ
            /// </summary>
            CanCalcCount = 0x0000000000000001,
            /// <summary>
            /// 濃度算出可能フラグ
            /// </summary>
            CanCalcConcentration = 0x0000000000000002,
            /// <summary>
            /// 再検査登録フラグRe-examination registration flag
            /// </summary>
            RegistRemeasure = 0x0000000000000004,
            /// <summary>
            /// 再計算可能フラグ
            /// </summary>
            CanRecalculation = 0x0000000000000008,
            /// <summary>
            /// 検量線登録可能フラグ
            /// </summary>
            CanRegistCalibCurve = 0x0000000000000010,
            /// <summary>
            /// 再計算後引き継ぎフラグ
            /// </summary>
            CanAfterReCalcInherit = 0x0000000000000020,
            /// <summary>
            /// 注意ポイント包含可能フラグ
            /// </summary>
            CanIncludingCautionPoint = 0x0000000000000040
        }

        /// <summary>
        /// リマークカテゴリビット
        /// </summary>
        /// <remarks>
        /// リマークによる分析結果の絞込みの条件として使用する分類
        /// ※再計算、絞り込みで使用
        /// </remarks>
        [Flags]
        public enum RemarkCategory : long
        {
            /// <summary>
            /// 温度エラー系
            /// </summary>
            TemperatureError = 0x0000000000000001,
            /// <summary>
            /// キャリブレーションエラー系
            /// </summary>
            CalibrationError = 0x0000000000000002,
            /// <summary>
            /// 有効期限エラー系
            /// </summary>
            ExpirationDataError = 0x0000000000000004,
            /// <summary>
            /// データ警告系
            /// </summary>
            DataWarning = 0x0000000000000008,
            /// <summary>
            /// データ編集系
            /// </summary>
            DataEdited = 0x0000000000000010,
            /// <summary>
            /// オンライン系(ホスト関連)
            /// </summary>
            OnLine = 0x0000000000000020,
        }
        /// <summary>
        /// Ｍ試薬吸引エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_M_REAGENT_SUCKING_UP_ERROR = "Rm";
        /// <summary>
        /// Ｒ１試薬吸引エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_R1_REAGENT_SUCKING_UP_ERROR = "R1";
        /// <summary>
        /// Ｒ２試薬吸引エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_R2_REAGENT_SUCKING_UP_ERROR = "R2";
        /// <summary>
        /// 前処理液吸引エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_PRE_PROCESS_LIQUID_SUCKING_UP_ERROR = "Rt";
        /// <summary>
        /// サンプルなしエラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_NO_SAMPLE_ERROR = "Sn";
        /// <summary>
        /// サンプル詰まりエラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_SAMPLE_STOPPED_UP_ERROR = "Sc";
        /// <summary>
        /// サンプル空吸エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_NOT_SAMPLE_SUCKING_UP_ERROR = "Se";
        /// <summary>
        /// サンプル分注エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_SAMPLE_DISPENSE_ERROR = "Sd";
        /// <summary>
        /// 希釈液なしエラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_NO_DILUTION_ERROR = "Dn";
        /// <summary>
        /// 洗浄不良エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_WASHING_FAILURE_ERROR = "We";
        /// <summary>
        /// 光学系エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_DETECTOR_ERROR = "Oe";
        /// <summary>
        /// サンプル分注チップ装着エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_SAMPLE_DISPENSE_TIP_SET_ERROR = "Ty";
        /// <summary>
        /// サンプル分注チップ廃棄エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_SAMPLE_DISPENSE_TIP_DISPOSAL_ERROR = "Tx";
        /// <summary>
        /// ダークエラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_DARK_ERROR = "Md";
        /// <summary>
        /// 測光エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_PHOTOMETRY_ERROR = "Ms";
        /// <summary>
        /// 免疫反応槽部温度エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_TEMP_OF_IMMUNO_REACTION_ERROR = "Ti";
        /// <summary>
        /// B/F1プレヒート部温度エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_TEMP_OF_BF1_PRE_HEAT_ERROR = "Tb";
        /// <summary>
        /// B/F2プレヒート部温度エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_TEMP_OF_BF2_PRE_HEAT_ERROR = "Tc";
        /// <summary>
        /// R1プローブプレヒート部温度エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_TEMP_OF_R1_PROBE_PRE_HEAT_ERROR = "Tr";
        /// <summary>
        /// R2プローブプレヒート部温度エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_TEMP_OF_R2_PROBE_PRE_HEAT_ERROR = "Ts";
        /// <summary>
        /// フォトマル部温度エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_TEMP_OF_DETECTOR_ERROR = "Tm";
        /// <summary>
        /// 試薬保冷庫温度エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_TEMP_OF_REAGENT_COOLER_ERROR = "Te";
        /// <summary>
        /// 希釈液保冷庫温度エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_TEMP_OF_DILUTION_COOLER_ERROR = "Td";
        /// <summary>
        /// サイクルタイムオーバーリマーク文字列
        /// </summary>
        public const String REMARK_STR_CYCLE_TIME_OVER_ERROR = "Cy";
        /// <summary>
        /// 未測定（ラック強制排出による）リマーク文字列
        /// </summary>
        public const String REMARK_STR_NO_MEASURED_ERROR = "Uk";
        /// <summary>
        /// 反応容器搬送エラーリマーク文字列
        /// </summary>
		public const String REMARK_STR_REACTION_VESSEL_CARRY_ERROR = "Ct";
        /// <summary>
        /// プレトリガ部温度エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_TEMP_OF_PRE_TRIGGER_TEMPERATURE_ERROR = "Tq";
        /// <summary>
        /// トリガ部温度エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_TEMP_OF_TRIGGER_TEMPERATURE_ERROR = "Tz";
        
        // <summary>
        // 予約
        // </summary>

        /// <summary>
        /// 検量線エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_CALIBRATION_CURVE_ERROR = "Cn";
        /// <summary>
        /// 試薬有効期限エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_REAGENT_EXPIRATION_DATE_ERROR = "Er";
        /// <summary>
        /// 希釈液有効期限エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_DILUTION_EXPIRATION_DATE_ERROR = "Ed";
        /// <summary>
        /// プレトリガ有効期限エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_PRE_TRIGGER_EXPIRATION_DATE_ERROR = "Ep";
        /// <summary>
        /// トリガ有効期限エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_TRIGGER_EXPIRATION_DATE_ERROR = "Et";
        /// <summary>
        /// 検量線有効期限エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_CALIB_EXPIRATION_DATE_ERROR = "Ec";
        /// <summary>
        /// 多重測定時のデータ間乖離許容範囲外エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_DIFF_ERROR = "Cv";
        /// <summary>
        /// 濃度値算出不能エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_CALC_CONC_ERROR = "Ce";
        /// <summary>
        /// キャリブレーション正常範囲外エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_CALIB_ERROR = "Cr";
        /// <summary>
        /// ダイナミックレンジ正常範囲外エラー／上限値オーバーリマーク文字列
        /// </summary>
        public const String REMARK_STR_DYNAMIC_RANGE_UPPER_ERROR = "CU";
        /// <summary>
        /// ダイナミックレンジ正常範囲外エラー／下限値オーバーリマーク文字列
        /// </summary>
        public const String REMARK_STR_DYNAMIC_RANGE_LOWER_ERROR = "CL";
        /// <summary>
        /// 管理値範囲外エラー（Xバー管理図）リマーク文字列
        /// </summary>
        public const String REMARK_STR_CONTROL_RANGE_ERROR = "Qr";
        /// <summary>
        /// 精度管理判定不能リマーク文字列
        /// </summary>
        public const String REMARK_STR_CONTROL_ERROR = "Qs";
        /// <summary>
        /// データ編集（手希釈倍率修正）リマーク文字列
        /// </summary>
        public const String REMARK_STR_EDIT_OF_MANUAL_DIL = "Ed";
        /// <summary>
        /// データ編集（再計算）リマーク文字列
        /// </summary>
        public const String REMARK_STR_EDIT_OF_RE_CALCU = "Ec";
        /// <summary>
        /// データ編集（検量線カウント修正）リマーク文字列
        /// </summary>
        public const String REMARK_STR_EDIT_OF_CALIB_COUNT = "Es";
        /// <summary>
        /// データ編集（修正された検量線で再計算）リマーク文字列
        /// </summary>
        public const String REMARK_STR_EDIT_OF_RE_CALCU_BY_EDIT_CURVE = "Ez";
        /// <summary>
        /// データ編集（精度管理濃度修正）リマーク文字列
        /// </summary>
        public const String REMARK_STR_EDIT_OF_CONTROL_CONC = "Eq";

        // <summary>
        // 予約
        // </summary>

        /// <summary>
        /// 測定情報異常エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_WORKSHEET_ERROR = "WS";
        /// <summary>
        /// 撹拌異常エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_MIXING_ERROR = "Mx";
        /// <summary>
        /// 競合異常エラーリマーク文字列
        /// </summary>
        public const String REMARK_STR_CONFLICT_ERROR = "CF";

        /// <summary>
        /// リマーク条件リスト
        /// </summary>
        private static readonly Dictionary<RemarkBit, RemarkCondition> dicRemarkCondition = new Dictionary<RemarkBit, RemarkCondition>() { 
            { RemarkBit.MReagentSuckingUpError,             new RemarkCondition( RemarkCriterion.RegistRemeasure | RemarkCriterion.CanAfterReCalcInherit, 0 ) },
            { RemarkBit.R1ReagentSuckingUpError,            new RemarkCondition( RemarkCriterion.RegistRemeasure | RemarkCriterion.CanAfterReCalcInherit, 0 ) },
            { RemarkBit.R2ReagentSuckingUpError,            new RemarkCondition( RemarkCriterion.RegistRemeasure | RemarkCriterion.CanAfterReCalcInherit, 0 ) },
            { RemarkBit.PreProcessLiquidSuckingUpError,     new RemarkCondition( RemarkCriterion.RegistRemeasure | RemarkCriterion.CanAfterReCalcInherit, 0 ) },
            { RemarkBit.NoSampleError,                      new RemarkCondition( RemarkCriterion.RegistRemeasure | RemarkCriterion.CanAfterReCalcInherit, 0 ) },
            { RemarkBit.SampleStoppedUpError,               new RemarkCondition( RemarkCriterion.RegistRemeasure | RemarkCriterion.CanAfterReCalcInherit, 0 ) },
            { RemarkBit.NotSampleSuckingUpError,            new RemarkCondition( RemarkCriterion.RegistRemeasure | RemarkCriterion.CanAfterReCalcInherit, 0 ) },
            { RemarkBit.SampleDispenseError,                new RemarkCondition( RemarkCriterion.RegistRemeasure | RemarkCriterion.CanAfterReCalcInherit, 0 ) },
            { RemarkBit.NoDilutionError,                    new RemarkCondition( RemarkCriterion.RegistRemeasure | RemarkCriterion.CanAfterReCalcInherit, 0 ) },
            { RemarkBit.WashingFailureError,                new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.RegistRemeasure | RemarkCriterion.CanRecalculation | RemarkCriterion.CanAfterReCalcInherit, 0 ) },
            { RemarkBit.DetectorError,                      new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.RegistRemeasure | RemarkCriterion.CanAfterReCalcInherit, 0 ) },
            { RemarkBit.SampleDispenseTipSetError,          new RemarkCondition( RemarkCriterion.RegistRemeasure | RemarkCriterion.CanAfterReCalcInherit, 0 ) },
            { RemarkBit.SampleDispenseTipDisposalError,     new RemarkCondition( RemarkCriterion.RegistRemeasure | RemarkCriterion.CanAfterReCalcInherit, 0 ) },
            { RemarkBit.DarkError,                          new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.RegistRemeasure | RemarkCriterion.CanAfterReCalcInherit, 0 ) },
            { RemarkBit.PhotometryError,                    new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.RegistRemeasure | RemarkCriterion.CanAfterReCalcInherit, 0 ) },
            { RemarkBit.TempOfImmunoreactionError,          new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanRegistCalibCurve | RemarkCriterion.CanAfterReCalcInherit | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.TemperatureError ) },
            { RemarkBit.TempOfBF1PreHeatError,              new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanRegistCalibCurve | RemarkCriterion.CanAfterReCalcInherit | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.TemperatureError ) },
            { RemarkBit.TempOfBF2PreHeatError,              new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanRegistCalibCurve | RemarkCriterion.CanAfterReCalcInherit | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.TemperatureError ) },
            { RemarkBit.TempOfR1ProbePreHeatError,          new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanRegistCalibCurve | RemarkCriterion.CanAfterReCalcInherit | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.TemperatureError ) },
            { RemarkBit.TempOfR2ProbePreHeatError,          new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanRegistCalibCurve | RemarkCriterion.CanAfterReCalcInherit | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.TemperatureError ) },
			{ RemarkBit.TempOfDetectorError,                new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanRegistCalibCurve | RemarkCriterion.CanAfterReCalcInherit | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.TemperatureError ) },
			{ RemarkBit.TempOfReagentCoolerError,           new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanRegistCalibCurve | RemarkCriterion.CanAfterReCalcInherit | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.TemperatureError ) },
            { RemarkBit.TempOfDilutionCoolerError,          new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanRegistCalibCurve | RemarkCriterion.CanAfterReCalcInherit | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.TemperatureError ) },
            { RemarkBit.CycleTimeOverError,                 new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanRegistCalibCurve | RemarkCriterion.CanAfterReCalcInherit, 0 ) },
            { RemarkBit.NoMeasuredError,                    new RemarkCondition( RemarkCriterion.CanAfterReCalcInherit, RemarkCategory.DataWarning ) },
            { RemarkBit.ReactionVesselCarryError,           new RemarkCondition( RemarkCriterion.RegistRemeasure | RemarkCriterion.CanAfterReCalcInherit, 0 ) },		
            { RemarkBit.TempOfPreTriggertemperatureError,   new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanRegistCalibCurve | RemarkCriterion.CanAfterReCalcInherit | RemarkCriterion.CanIncludingCautionPoint, 0 ) },
            { RemarkBit.CalibrationCurveError,              new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanRecalculation | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.CalibrationError ) },
            { RemarkBit.ReagentExpirationDateError,         new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanRegistCalibCurve | RemarkCriterion.CanAfterReCalcInherit | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.ExpirationDataError ) },
            { RemarkBit.DilutionExpirationDateError,        new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanRegistCalibCurve | RemarkCriterion.CanAfterReCalcInherit | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.ExpirationDataError ) },
            { RemarkBit.PreTriggerExpirationDateError,      new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanRegistCalibCurve | RemarkCriterion.CanAfterReCalcInherit | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.ExpirationDataError ) },
            { RemarkBit.TriggerExpirationDateError,         new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanRegistCalibCurve | RemarkCriterion.CanAfterReCalcInherit | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.ExpirationDataError ) },
            { RemarkBit.CalibExpirationDateError,           new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanRegistCalibCurve | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.ExpirationDataError ) },
            { RemarkBit.DiffError,                          new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.RegistRemeasure, RemarkCategory.DataWarning ) },
            { RemarkBit.CalcConcError,                      new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.RegistRemeasure | RemarkCriterion.CanRecalculation, RemarkCategory.DataWarning ) },
            { RemarkBit.CalibError,                         new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation, RemarkCategory.DataWarning ) },
            { RemarkBit.DynamicrangeUpperError,             new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.DataWarning ) },
            { RemarkBit.DynamicrangeLowerError,             new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.DataWarning ) },
            { RemarkBit.ControlRangeError,                  new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation, RemarkCategory.DataWarning ) },
            { RemarkBit.ControlError,                       new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation, RemarkCategory.DataWarning ) },
            { RemarkBit.EditOfManualDil,                    new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.DataEdited ) },
            { RemarkBit.EditOfReCalcu,                      new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.DataEdited ) },
            { RemarkBit.EditOfCalibCount,                   new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.DataEdited ) },
            { RemarkBit.EditOfReCalcuByEditCurve,           new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.DataEdited ) },
            { RemarkBit.EditOfControlConc,                  new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.DataEdited ) },
            { RemarkBit.WorksheetError,                     new RemarkCondition( RemarkCriterion.RegistRemeasure | RemarkCriterion.CanAfterReCalcInherit, 0 ) },
            { RemarkBit.MixingError,                        new RemarkCondition( RemarkCriterion.CanCalcCount | RemarkCriterion.CanCalcConcentration | RemarkCriterion.CanRecalculation | RemarkCriterion.CanRegistCalibCurve | RemarkCriterion.CanAfterReCalcInherit | RemarkCriterion.CanIncludingCautionPoint, RemarkCategory.TemperatureError ) },
            { RemarkBit.ConflictError,                      new RemarkCondition( RemarkCriterion.RegistRemeasure | RemarkCriterion.CanAfterReCalcInherit, 0 ) },
        };

        /// <summary>
        /// 平均リマークにのみ登場する濃度計算不可のリマークリスト
        /// </summary>
        private static List<RemarkBit> averageOnlyCanCalcConcRemark = new List<RemarkBit>()
        {
            RemarkBit.DiffError
        };

        #endregion

        #region [クラス変数定義]

        /// <summary>
        /// リマークビット-リマーク文字列対応
        /// </summary>
        static protected Dictionary<String, String> remarkNoToChar = new Dictionary<String, String>()
        {
            {RemarkBit.MReagentSuckingUpError.ToString()              ,REMARK_STR_M_REAGENT_SUCKING_UP_ERROR},
            {RemarkBit.R1ReagentSuckingUpError.ToString()             ,REMARK_STR_R1_REAGENT_SUCKING_UP_ERROR},
            {RemarkBit.R2ReagentSuckingUpError.ToString()             ,REMARK_STR_R2_REAGENT_SUCKING_UP_ERROR},
            {RemarkBit.PreProcessLiquidSuckingUpError.ToString()      ,REMARK_STR_PRE_PROCESS_LIQUID_SUCKING_UP_ERROR},
            {RemarkBit.NoSampleError.ToString()                       ,REMARK_STR_NO_SAMPLE_ERROR},
            {RemarkBit.SampleStoppedUpError.ToString()                ,REMARK_STR_SAMPLE_STOPPED_UP_ERROR},
            {RemarkBit.NotSampleSuckingUpError.ToString()             ,REMARK_STR_NOT_SAMPLE_SUCKING_UP_ERROR},
            {RemarkBit.SampleDispenseError.ToString()                 ,REMARK_STR_SAMPLE_DISPENSE_ERROR},
            {RemarkBit.NoDilutionError.ToString()                     ,REMARK_STR_NO_DILUTION_ERROR},
            {RemarkBit.WashingFailureError.ToString()                 ,REMARK_STR_WASHING_FAILURE_ERROR},
            {RemarkBit.DetectorError.ToString()                       ,REMARK_STR_DETECTOR_ERROR},
            {RemarkBit.SampleDispenseTipSetError.ToString()           ,REMARK_STR_SAMPLE_DISPENSE_TIP_SET_ERROR},
            {RemarkBit.SampleDispenseTipDisposalError.ToString()      ,REMARK_STR_SAMPLE_DISPENSE_TIP_DISPOSAL_ERROR},
            {RemarkBit.DarkError.ToString()                           ,REMARK_STR_DARK_ERROR},
            {RemarkBit.PhotometryError.ToString()                     ,REMARK_STR_PHOTOMETRY_ERROR},
            {RemarkBit.TempOfImmunoreactionError.ToString()           ,REMARK_STR_TEMP_OF_IMMUNO_REACTION_ERROR},
            {RemarkBit.TempOfBF1PreHeatError.ToString()               ,REMARK_STR_TEMP_OF_BF1_PRE_HEAT_ERROR},
            {RemarkBit.TempOfBF2PreHeatError.ToString()               ,REMARK_STR_TEMP_OF_BF2_PRE_HEAT_ERROR},
            {RemarkBit.TempOfR1ProbePreHeatError.ToString()           ,REMARK_STR_TEMP_OF_R1_PROBE_PRE_HEAT_ERROR},
            {RemarkBit.TempOfR2ProbePreHeatError.ToString()           ,REMARK_STR_TEMP_OF_R2_PROBE_PRE_HEAT_ERROR},
            {RemarkBit.TempOfDetectorError.ToString()                 ,REMARK_STR_TEMP_OF_DETECTOR_ERROR},
            {RemarkBit.TempOfReagentCoolerError.ToString()            ,REMARK_STR_TEMP_OF_REAGENT_COOLER_ERROR},
            {RemarkBit.TempOfDilutionCoolerError.ToString()           ,REMARK_STR_TEMP_OF_DILUTION_COOLER_ERROR},
            {RemarkBit.CycleTimeOverError.ToString()                  ,REMARK_STR_CYCLE_TIME_OVER_ERROR},
            {RemarkBit.NoMeasuredError.ToString()                     ,REMARK_STR_NO_MEASURED_ERROR},
            {RemarkBit.ReactionVesselCarryError.ToString()            ,REMARK_STR_REACTION_VESSEL_CARRY_ERROR},
            {RemarkBit.TempOfPreTriggertemperatureError.ToString()    ,REMARK_STR_TEMP_OF_PRE_TRIGGER_TEMPERATURE_ERROR},
            {RemarkBit.CalibrationCurveError.ToString()               ,REMARK_STR_CALIBRATION_CURVE_ERROR},
            {RemarkBit.ReagentExpirationDateError.ToString()          ,REMARK_STR_REAGENT_EXPIRATION_DATE_ERROR},
            {RemarkBit.DilutionExpirationDateError.ToString()         ,REMARK_STR_DILUTION_EXPIRATION_DATE_ERROR},
            {RemarkBit.PreTriggerExpirationDateError.ToString()       ,REMARK_STR_PRE_TRIGGER_EXPIRATION_DATE_ERROR},
            {RemarkBit.TriggerExpirationDateError.ToString()          ,REMARK_STR_TRIGGER_EXPIRATION_DATE_ERROR},
            {RemarkBit.CalibExpirationDateError.ToString()            ,REMARK_STR_CALIB_EXPIRATION_DATE_ERROR},
            {RemarkBit.DiffError.ToString()                           ,REMARK_STR_DIFF_ERROR},
            {RemarkBit.CalcConcError.ToString()                       ,REMARK_STR_CALC_CONC_ERROR},
            {RemarkBit.CalibError.ToString()                          ,REMARK_STR_CALIB_ERROR},
            {RemarkBit.DynamicrangeUpperError.ToString()              ,REMARK_STR_DYNAMIC_RANGE_UPPER_ERROR},
            {RemarkBit.DynamicrangeLowerError.ToString()              ,REMARK_STR_DYNAMIC_RANGE_LOWER_ERROR},
            {RemarkBit.ControlRangeError.ToString()                   ,REMARK_STR_CONTROL_RANGE_ERROR},
            {RemarkBit.ControlError.ToString()                        ,REMARK_STR_CONTROL_ERROR},
            {RemarkBit.EditOfManualDil.ToString()                     ,REMARK_STR_EDIT_OF_MANUAL_DIL},
            {RemarkBit.EditOfReCalcu.ToString()                       ,REMARK_STR_EDIT_OF_RE_CALCU},
            {RemarkBit.EditOfCalibCount.ToString()                    ,REMARK_STR_EDIT_OF_CALIB_COUNT},
            {RemarkBit.EditOfReCalcuByEditCurve.ToString()            ,REMARK_STR_EDIT_OF_RE_CALCU_BY_EDIT_CURVE},
            {RemarkBit.EditOfControlConc.ToString()                   ,REMARK_STR_EDIT_OF_CONTROL_CONC},
            {RemarkBit.WorksheetError.ToString()                      ,REMARK_STR_WORKSHEET_ERROR},
            {RemarkBit.MixingError.ToString()                         ,REMARK_STR_MIXING_ERROR},
            {RemarkBit.ConflictError.ToString()                       ,REMARK_STR_CONFLICT_ERROR}
        };

        /// <summary>
        /// リマークビット-リマーク名称対応
        /// </summary>
        static protected Dictionary<String, String> remarkNoToName = new Dictionary<String, String>()
        {
            {RemarkBit.MReagentSuckingUpError.ToString()              ,Oelco.CarisX.Properties.Resources.STRING_REMARK_001},
            {RemarkBit.R1ReagentSuckingUpError.ToString()             ,Oelco.CarisX.Properties.Resources.STRING_REMARK_002},
            {RemarkBit.R2ReagentSuckingUpError.ToString()             ,Oelco.CarisX.Properties.Resources.STRING_REMARK_003},
            {RemarkBit.PreProcessLiquidSuckingUpError.ToString()      ,Oelco.CarisX.Properties.Resources.STRING_REMARK_004},
            {RemarkBit.NoSampleError.ToString()                       ,Oelco.CarisX.Properties.Resources.STRING_REMARK_005},
            {RemarkBit.SampleStoppedUpError.ToString()                ,Oelco.CarisX.Properties.Resources.STRING_REMARK_006},
            {RemarkBit.NotSampleSuckingUpError.ToString()             ,Oelco.CarisX.Properties.Resources.STRING_REMARK_007},
            {RemarkBit.SampleDispenseError.ToString()                 ,Oelco.CarisX.Properties.Resources.STRING_REMARK_008},
            {RemarkBit.NoDilutionError.ToString()                     ,Oelco.CarisX.Properties.Resources.STRING_REMARK_009},
            {RemarkBit.WashingFailureError.ToString()                 ,Oelco.CarisX.Properties.Resources.STRING_REMARK_010},
            {RemarkBit.DetectorError.ToString()                       ,Oelco.CarisX.Properties.Resources.STRING_REMARK_011},
            {RemarkBit.SampleDispenseTipSetError.ToString()           ,Oelco.CarisX.Properties.Resources.STRING_REMARK_012},
            {RemarkBit.SampleDispenseTipDisposalError.ToString()      ,Oelco.CarisX.Properties.Resources.STRING_REMARK_013},
            {RemarkBit.DarkError.ToString()                           ,Oelco.CarisX.Properties.Resources.STRING_REMARK_014},
            {RemarkBit.PhotometryError.ToString()                     ,Oelco.CarisX.Properties.Resources.STRING_REMARK_015},
            {RemarkBit.TempOfImmunoreactionError.ToString()           ,Oelco.CarisX.Properties.Resources.STRING_REMARK_016},
            {RemarkBit.TempOfBF1PreHeatError.ToString()               ,Oelco.CarisX.Properties.Resources.STRING_REMARK_017},
            {RemarkBit.TempOfBF2PreHeatError.ToString()               ,Oelco.CarisX.Properties.Resources.STRING_REMARK_018},
            {RemarkBit.TempOfR1ProbePreHeatError.ToString()           ,Oelco.CarisX.Properties.Resources.STRING_REMARK_019},
            {RemarkBit.TempOfR2ProbePreHeatError.ToString()           ,Oelco.CarisX.Properties.Resources.STRING_REMARK_020},
            {RemarkBit.TempOfDetectorError.ToString()                 ,Oelco.CarisX.Properties.Resources.STRING_REMARK_023},
            {RemarkBit.TempOfReagentCoolerError.ToString()            ,Oelco.CarisX.Properties.Resources.STRING_REMARK_024},
            {RemarkBit.TempOfDilutionCoolerError.ToString()           ,Oelco.CarisX.Properties.Resources.STRING_REMARK_025},
            {RemarkBit.CycleTimeOverError.ToString()                  ,Oelco.CarisX.Properties.Resources.STRING_REMARK_026},
            {RemarkBit.NoMeasuredError.ToString()                     ,Oelco.CarisX.Properties.Resources.STRING_REMARK_027},
            {RemarkBit.ReactionVesselCarryError.ToString()            ,Oelco.CarisX.Properties.Resources.STRING_REMARK_028},
            {RemarkBit.TempOfPreTriggertemperatureError.ToString()    ,Oelco.CarisX.Properties.Resources.STRING_REMARK_021},
            {RemarkBit.CalibrationCurveError.ToString()               ,Oelco.CarisX.Properties.Resources.STRING_REMARK_033},
            {RemarkBit.ReagentExpirationDateError.ToString()          ,Oelco.CarisX.Properties.Resources.STRING_REMARK_034},
            {RemarkBit.DilutionExpirationDateError.ToString()         ,Oelco.CarisX.Properties.Resources.STRING_REMARK_035},
            {RemarkBit.PreTriggerExpirationDateError.ToString()       ,Oelco.CarisX.Properties.Resources.STRING_REMARK_036},
            {RemarkBit.TriggerExpirationDateError.ToString()          ,Oelco.CarisX.Properties.Resources.STRING_REMARK_037},
            {RemarkBit.CalibExpirationDateError.ToString()            ,Oelco.CarisX.Properties.Resources.STRING_REMARK_038},
            {RemarkBit.DiffError.ToString()                           ,Oelco.CarisX.Properties.Resources.STRING_REMARK_039},
            {RemarkBit.CalcConcError.ToString()                       ,Oelco.CarisX.Properties.Resources.STRING_REMARK_040},
            {RemarkBit.CalibError.ToString()                          ,Oelco.CarisX.Properties.Resources.STRING_REMARK_041},
            {RemarkBit.DynamicrangeUpperError.ToString()              ,Oelco.CarisX.Properties.Resources.STRING_REMARK_042},
            {RemarkBit.DynamicrangeLowerError.ToString()              ,Oelco.CarisX.Properties.Resources.STRING_REMARK_043},
            {RemarkBit.ControlRangeError.ToString()                   ,Oelco.CarisX.Properties.Resources.STRING_REMARK_044},
            {RemarkBit.ControlError.ToString()                        ,Oelco.CarisX.Properties.Resources.STRING_REMARK_045},
            {RemarkBit.EditOfManualDil.ToString()                     ,Oelco.CarisX.Properties.Resources.STRING_REMARK_046},
            {RemarkBit.EditOfReCalcu.ToString()                       ,Oelco.CarisX.Properties.Resources.STRING_REMARK_047},
            {RemarkBit.EditOfCalibCount.ToString()                    ,Oelco.CarisX.Properties.Resources.STRING_REMARK_048},
            {RemarkBit.EditOfReCalcuByEditCurve.ToString()            ,Oelco.CarisX.Properties.Resources.STRING_REMARK_049},
            {RemarkBit.EditOfControlConc.ToString()                   ,Oelco.CarisX.Properties.Resources.STRING_REMARK_050},
            {RemarkBit.WorksheetError.ToString()                      ,Oelco.CarisX.Properties.Resources.STRING_REMARK_054},
            {RemarkBit.MixingError.ToString()                         ,Oelco.CarisX.Properties.Resources.STRING_REMARK_053},
            {RemarkBit.ConflictError.ToString()                       ,Oelco.CarisX.Properties.Resources.STRING_REMARK_055}
        };

        /// <summary>
        /// リマークビット-リマーク説明対応
        /// </summary>
        static protected Dictionary<String, String> remarkNoToDescription = new Dictionary<String, String>()
        {
            {RemarkBit.MReagentSuckingUpError.ToString()              ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_001},
            {RemarkBit.R1ReagentSuckingUpError.ToString()             ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_002},
            {RemarkBit.R2ReagentSuckingUpError.ToString()             ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_003},
            {RemarkBit.PreProcessLiquidSuckingUpError.ToString()      ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_004},
            {RemarkBit.NoSampleError.ToString()                       ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_005},
            {RemarkBit.SampleStoppedUpError.ToString()                ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_006},
            {RemarkBit.NotSampleSuckingUpError.ToString()             ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_007},
            {RemarkBit.SampleDispenseError.ToString()                 ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_008},
            {RemarkBit.NoDilutionError.ToString()                     ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_009},
            {RemarkBit.WashingFailureError.ToString()                 ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_010},
            {RemarkBit.DetectorError.ToString()                       ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_011},
            {RemarkBit.SampleDispenseTipSetError.ToString()           ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_012},
            {RemarkBit.SampleDispenseTipDisposalError.ToString()      ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_013},
            {RemarkBit.DarkError.ToString()                           ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_014},
            {RemarkBit.PhotometryError.ToString()                     ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_015},
            {RemarkBit.TempOfImmunoreactionError.ToString()           ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_016},
            {RemarkBit.TempOfBF1PreHeatError.ToString()               ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_017},
            {RemarkBit.TempOfBF2PreHeatError.ToString()               ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_018},
            {RemarkBit.TempOfR1ProbePreHeatError.ToString()           ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_019},
            {RemarkBit.TempOfR2ProbePreHeatError.ToString()           ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_020},
            {RemarkBit.TempOfDetectorError.ToString()                 ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_023},
            {RemarkBit.TempOfReagentCoolerError.ToString()            ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_024},
            {RemarkBit.TempOfDilutionCoolerError.ToString()           ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_025},
            {RemarkBit.CycleTimeOverError.ToString()                  ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_026},
            {RemarkBit.NoMeasuredError.ToString()                     ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_027},
            {RemarkBit.ReactionVesselCarryError.ToString()            ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_028},
            {RemarkBit.TempOfPreTriggertemperatureError.ToString()    ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_021},
            {RemarkBit.CalibrationCurveError.ToString()               ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_033},
            {RemarkBit.ReagentExpirationDateError.ToString()          ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_034},
            {RemarkBit.DilutionExpirationDateError.ToString()         ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_035},
            {RemarkBit.PreTriggerExpirationDateError.ToString()       ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_036},
            {RemarkBit.TriggerExpirationDateError.ToString()          ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_037},
            {RemarkBit.CalibExpirationDateError.ToString()            ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_038},
            {RemarkBit.DiffError.ToString()                           ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_039},
            {RemarkBit.CalcConcError.ToString()                       ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_040},
            {RemarkBit.CalibError.ToString()                          ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_041},
            {RemarkBit.DynamicrangeUpperError.ToString()              ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_042},
            {RemarkBit.DynamicrangeLowerError.ToString()              ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_043},
            {RemarkBit.ControlRangeError.ToString()                   ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_044},
            {RemarkBit.ControlError.ToString()                        ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_045},
            {RemarkBit.EditOfManualDil.ToString()                     ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_046},
            {RemarkBit.EditOfReCalcu.ToString()                       ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_047},
            {RemarkBit.EditOfCalibCount.ToString()                    ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_048},
            {RemarkBit.EditOfReCalcuByEditCurve.ToString()            ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_049},
            {RemarkBit.EditOfControlConc.ToString()                   ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_050},
            {RemarkBit.WorksheetError.ToString()                      ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_054},
            {RemarkBit.MixingError.ToString()                         ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_053},
            {RemarkBit.ConflictError.ToString()                       ,Oelco.CarisX.Properties.Resources.STRING_REMARK_DESCRIPTION_055}
        };

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// リマーク値
        /// </summary>
        RemarkBit remarkValue = REMARK_DEFAULT;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Remark()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="remark">リマーク値</param>
        public Remark( Int64 remark )
        {
            this.remarkValue = (RemarkBit)remark;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="remark">リマーク値</param>
        public Remark( RemarkBit remarkbit )
        {
            this.remarkValue = remarkbit;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 再検査要否の取得
        /// </summary>
        public Boolean IsNeedReMeasure
        {
            get
            {
                // 再検査記載の必要があるリマークを持っていること
                return this.HasRemark( (RemarkBit)Remark.dicRemarkCondition.Sum( ( pair ) => ( pair.Value.RegistRemeasure ) ? (Int64)pair.Key : 0 ) );
            }
        }

        /// <summary>
        /// カウント算出可否の取得
        /// </summary>
        public Boolean CanCalcCount
        {
            get
            {
                // カウント算出不可のリマークを持っていないこと
                return !this.HasRemark( (RemarkBit)Remark.dicRemarkCondition.Sum( ( pair ) => ( !pair.Value.CanCalcCount ) ? (Int64)pair.Key : 0 ) );
            }
        }
        /// <summary>
        /// 濃度算出可否の取得
        /// </summary>
        public Boolean CanCalcConcentration
        {
            get
            {
                // 濃度算出不可のリマークを持っていないこと
                return !this.HasRemark( (RemarkBit)Remark.dicRemarkCondition.Sum( ( pair ) => ( !pair.Value.CanCalcConcentration ) ? (Int64)pair.Key : 0 ) );
            }
        }

        /// <summary>
        /// 再計算可否の取得
        /// </summary>
        public Boolean CanRecalculation
        {
            get
            {
                // 再計算不可のリマークを持っていないこと
                return !this.HasRemark( (RemarkBit)Remark.dicRemarkCondition.Sum( ( pair ) => ( !pair.Value.CanCalcConcentration && !pair.Value.CanRecalculation ) ? (Int64)pair.Key : 0 ) );
            }
        }

        /// <summary>
        /// 検量線登録可否の取得
        /// </summary>
        public Boolean CanRegistCurve
        {
            get
            {
                // 検量線登録不可のリマークを持っていないこと
                return !this.HasRemark( (RemarkBit)Remark.dicRemarkCondition.Sum( ( pair ) => ( !pair.Value.CanRegistCalibCurve ) ? (Int64)pair.Key : 0 ) );
            }
        }

        public Boolean IsAverageOnlyCantRegistCurve
        {
            get
            {

                Boolean contain = false;

                // 平均リマークにのみ発生する濃度計算不可リマークを含んでいるか確認する
                foreach( var averageOnlyCantCalc in averageOnlyCanCalcConcRemark )
                {
                    contain |= this.HasRemark( averageOnlyCantCalc );
                }

                return contain;
            }
        }

        /// <summary>
        /// 注意ポイント包含可否の取得
        /// </summary>
        public Boolean CanIncludingCautionPoint
        {
            get
            {
                // 注意ポイント包含不可のポイントを持っていないこと
                return !this.HasRemark( (RemarkBit)Remark.dicRemarkCondition.Sum( ( pair ) => ( !pair.Value.CanIncludingCautionPoint ) ? (Int64)pair.Key : 0 ) );
            }
        }

        /// <summary>
        /// リマーク値の取得、設定
        /// </summary>
        public Int64 Value
        {
            get
            {
                return (Int64)this.remarkValue;
            }
            set
            {
                this.remarkValue = (RemarkBit)value;
            }
        }

        /// <summary>
        /// 再計算後に継承されるリマークの取得
        /// </summary>
        public Remark ReCalcInheritRemark
        {
            get
            {
                return this.GetFilterRemark( Remark.RemarkCriterion.CanAfterReCalcInherit );
            }
        }

        /// <summary>
        /// 温度エラーの取得
        /// </summary>
        public Boolean IsTemperatureError
        {
            get
            {
                return this.HasRemark( (RemarkBit)Remark.dicRemarkCondition.Sum( ( pair ) => ( pair.Value.IsTemperatureError ) ? (Int64)pair.Key : 0 ) );
            }
        }

        /// <summary>
        /// キャリブレーションエラーの取得
        /// </summary>
        public Boolean IsCalibrationError
        {
            get
            {
                return this.HasRemark( (RemarkBit)Remark.dicRemarkCondition.Sum( ( pair ) => ( pair.Value.IsCalibrationError ) ? (Int64)pair.Key : 0 ) );
            }
        }

        /// <summary>
        /// 有効期限エラーの取得
        /// </summary>
        public Boolean IsExpirationDataError
        {
            get
            {
                return this.HasRemark( (RemarkBit)Remark.dicRemarkCondition.Sum( ( pair ) => ( pair.Value.IsExpirationDataError ) ? (Int64)pair.Key : 0 ) );
            }
        }

        /// <summary>
        /// データ警告の取得
        /// </summary>
        public Boolean IsDataWarning
        {
            get
            {
                return this.HasRemark( (RemarkBit)Remark.dicRemarkCondition.Sum( ( pair ) => ( pair.Value.IsDataWarning ) ? (Int64)pair.Key : 0 ) );
            }
        }

        /// <summary>
        /// データ編集の取得
        /// </summary>
        public Boolean IsDataEdited
        {
            get
            {
                return this.HasRemark( (RemarkBit)Remark.dicRemarkCondition.Sum( ( pair ) => ( pair.Value.IsDataEdited ) ? (Int64)pair.Key : 0 ) );
            }
        }

        /// <summary>
        /// オンラインの取得
        /// </summary>
        public Boolean IsOnLine
        {
            get
            {
                return this.HasRemark( (RemarkBit)Remark.dicRemarkCondition.Sum( ( pair ) => ( pair.Value.IsOnLine ) ? (Int64)pair.Key : 0 ) );
            }
        }
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// Int64からの暗黙的変換
        /// </summary>
        /// <remarks>
        /// Int64から暗黙的に変換します。
        /// </remarks>
        /// <param name="value">変換元Int64</param>
        /// <returns>変換後Remark</returns>
        public static implicit operator Remark( Int64 value )
        {
            return new Remark( value );
        }
        /// <summary>
        /// Int64への暗黙的変換
        /// </summary>
        /// <remarks>
        /// Int64へ暗黙的に変換します。
        /// </remarks>
        /// <param name="remark">変換元Remark</param>
        /// <returns>変換後Int64</returns>
        public static implicit operator Int64( Remark remark )
        {
            if ( remark != null )
            {
                return remark.Value;
            }
            return REMARK_DEFAULT;
        }

        /// <summary>
        /// RemarkBitからの暗黙的変換
        /// </summary>
        /// <remarks>
        /// RemarkBitから暗黙的に変換します。
        /// </remarks>
        /// <param name="remarkbit">変換元RemarkBit</param>
        /// <returns>Remark</returns>
        public static implicit operator Remark( RemarkBit remarkbit )
        {
            return new Remark( remarkbit );
        }
        /// <summary>
        /// RemarkBitへの暗黙的変換
        /// </summary>
        /// <remarks>
        /// RemarkBitへ暗黙的に変換します。
        /// </remarks>
        /// <param name="remark">変換元Remark</param>
        /// <returns>変換後RemarkBit</returns>
        public static implicit operator RemarkBit( Remark remark )
        {
            if ( remark != null )
            {
                return remark.remarkValue;
            }
            return REMARK_DEFAULT;
        }

        /// <summary>
        /// リマーク追加
        /// </summary>
        /// <remarks>
        /// 現在のリマーク内容に指定リマークを追加します。
        /// </remarks>
        /// <param name="setRemark">追加対象リマークビット</param>
        public void AddRemark( RemarkBit setRemark )
        {
            this.remarkValue |= setRemark;
        }

        /// <summary>
        /// リマーク削除
        /// </summary>
        /// <remarks>
        /// リマークを削除します。
        /// </remarks>
        /// <param name="removeRemark">削除対象リマークビット</param>
        public void RemoveRemark( RemarkBit removeRemark )
        {
            // ORしてからのXORで取る
            this.remarkValue |= removeRemark;
            this.remarkValue ^= removeRemark;
        }

        /// <summary>
        /// リマーク文字列配列取得
        /// </summary>
        /// <remarks>
        /// 現在クラスが保持しているリマーク値から、
        /// リマーク文字列の配列を作成します。
        /// </remarks>
        /// <returns>リマーク文字列配列</returns>
        public String[] GetRemarkStrings()
        {
            // リマーク文字列配列作成
            String[] str = this.remarkValue == 0 ? new String[0] : this.remarkValue.ToString().Split( new String[] { ", " }, StringSplitOptions.None );
            List<String> remarkCharStr = new List<String>();
            foreach ( String remarkEnumName in str )
            {
				// RemarkのEnum値に定義されていない値が渡されたときは、Remark == 0 と同様の扱い（エラーなし）とする
				if (!remarkNoToChar.ContainsKey(remarkEnumName))
				{
					continue;
				}

                remarkCharStr.Add( remarkNoToChar[remarkEnumName] );
            }
            return remarkCharStr.ToArray();
        }

        /// <summary>
        /// リマーク文字列配列取得
        /// </summary>
        /// <remarks>
        /// 現在クラスが保持しているリマーク値から、
        /// リマーク文字列の配列を作成します。
        /// </remarks>
        /// <returns>リマーク文字列配列</returns>
        public void SetRemarkStrings( String[] remarks )
        {
            // リマーク文字列からリマークenum定義を検索しRemarkBitに変換して保持値に追加する。
            this.remarkValue = (RemarkBit)remarkNoToChar.Sum( ( pair ) => ( remarks.Contains( pair.Value ) ) ? (Int64)Enum.Parse( typeof( RemarkBit ), pair.Key ) : REMARK_DEFAULT );
        }


        /// <summary>
        /// リマーク名称文文字列配列取得
        /// </summary>
        /// <remarks>
        /// 現在クラスが保持しているリマーク値から、
        /// リマーク名称の文字列配列を作成します。
        /// </remarks>
        /// <returns>リマーク名称文字列配列</returns>
        public String[] GetRemarkNameStrings()
        {
            // リマーク文字列配列作成
            String[] str = this.remarkValue == 0 ? new String[0] : this.remarkValue.ToString().Split( new String[] { ", " }, StringSplitOptions.None );
            List<String> remarkCharStr = new List<String>();
            foreach ( String remarkEnumName in str )
            {
                remarkCharStr.Add( remarkNoToName[remarkEnumName] );
            }
            return remarkCharStr.ToArray();
        }


        /// <summary>
        /// リマーク説明文文字列配列取得
        /// </summary>
        /// <remarks>
        /// 現在クラスが保持しているリマーク値から、
        /// リマーク説明文の文字列配列を作成します。
        /// </remarks>
        /// <returns>リマーク説明文文字列配列</returns>
        public String[] GetRemarkDescriptionStrings()
        {
            // リマーク文字列配列作成
            String[] str = this.remarkValue == 0 ? new String[0] : this.remarkValue.ToString().Split( new String[] { ", " }, StringSplitOptions.None );
            List<String> remarkCharStr = new List<String>();
            foreach ( String remarkEnumName in str )
            {
                remarkCharStr.Add( remarkNoToDescription[remarkEnumName] );
            }
            return remarkCharStr.ToArray();
        }

        /// <summary>
        /// 内包チェック処理
        /// </summary>
        /// <remarks>
        /// 指定のリマークが一種類以上内包しているかどうかを取得します。
        /// </remarks>
        /// <param name="remarkBit">内包チェック対象</param>
        /// <returns>true:内包している/false:内包していない</returns>
        public Boolean HasRemark( RemarkBit remarkBit )
        {
            return ( this.remarkValue & remarkBit ) != 0;
        }

        /// <summary>
        /// カテゴリに属するリマーク
        /// </summary>
        /// <remarks>
        /// 指定のカテゴリにリマークが属しているかどうかを取得します。
        /// </remarks>
        /// <param name="category">カテゴリ</param>
        /// <param name="remark">リマーク</param>
        /// <returns>true:属する</returns>
        public Boolean IsBelongCategory(Remark.RemarkCategory category )
        {
            if ( ( ( ( category & RemarkCategory.TemperatureError ) != 0 ) ? this.IsTemperatureError : false ) ||
                ( ( ( category & RemarkCategory.CalibrationError ) != 0 ) ? this.IsCalibrationError : false ) ||
                 ( ( ( category & RemarkCategory.ExpirationDataError ) != 0 ) ? this.IsExpirationDataError : false ) ||
                 ( ( ( category & RemarkCategory.DataWarning ) != 0 ) ? this.IsDataWarning : false ) ||
                 ( ( ( category & RemarkCategory.DataEdited ) != 0 ) ? this.IsDataEdited : false ) ||
                ( ( ( category & RemarkCategory.OnLine ) != 0 ) ? this.IsOnLine : false ) )
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// フィルタ後リマーク
        /// </summary>
        /// <remarks>
        /// 指定のリマーク判定ビットが有効なリマークのフィルタ後のリマークを取得します。
        /// </remarks>
        /// <param name="remarkCriterion">リマーク判定ビット</param>
        /// <returns>フィルタ後のリマーク</returns>
        public Remark GetFilterRemark( RemarkCriterion remarkCriterion )
        {
            Remark remark = this.Value;
            remark.RemoveRemark( (RemarkBit)Remark.dicRemarkCondition.Sum( ( pair ) => ( !pair.Value.MatchCondition( remarkCriterion ) ) ? (Int64)pair.Key : 0 ) );
            return remark;
        }

        /// <summary>
        /// フィルタ後リマーク
        /// </summary>
        /// <remarks>
        /// 指定のリマークカテゴリビットが有効なリマークのフィルタ後のリマークを取得します。
        /// </remarks>
        /// <param name="remarkCategory">リマークカテゴリビット</param>
        /// <returns>フィルタ後のリマーク</returns>
        public Remark GetFilterRemark( RemarkCategory remarkCategory )
        {
            Remark remark = this.Value;
            remark.RemoveRemark( (RemarkBit)Remark.dicRemarkCondition.Sum( ( pair ) => ( !pair.Value.MatchCondition( remarkCategory ) ) ? (Int64)pair.Key : 0 ) );
            return remark;
        }

        #endregion

        #region [内部クラス]

        /// <summary>
        /// リマーク条件
        /// </summary>
        private class RemarkCondition
        {
            #region [インスタンス変数定義]

            /// <summary>
            /// リマーク判定ビット
            /// </summary>
            private RemarkCriterion criterion = 0;

            /// <summary>
            /// リマークカテゴリビット
            /// </summary>
            private RemarkCategory category = 0;

            #endregion

            #region [コンストラクタ/デストラクタ]

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="criterion">リマーク判定基準ビット</param>
            /// <param name="category">リマークカテゴリビット</param>
            public RemarkCondition( RemarkCriterion criterion, RemarkCategory category )
            {
                this.criterion = criterion;
                this.category = category;
            }
            #endregion

            #region [プロパティ]

            #region _判定基準_
            /// <summary>
            /// カウント算出可能フラグの取得
            /// </summary>
            public Boolean CanCalcCount
            {
                get
                {
                    return ( this.criterion & RemarkCriterion.CanCalcCount ) != 0;
                }
            }

            /// <summary>
            /// 濃度算出可能フラグの取得
            /// </summary>
            public Boolean CanCalcConcentration
            {
                get
                {
                    return ( this.criterion & RemarkCriterion.CanCalcConcentration ) != 0;
                }
            }

            /// <summary>
            /// 再検査登録フラグの取得
            /// </summary>
            public Boolean RegistRemeasure
            {
                get
                {
                    return ( this.criterion & RemarkCriterion.RegistRemeasure ) != 0;
                }
            }

            /// <summary>
            /// 再計算可能フラグの取得
            /// </summary>
            public Boolean CanRecalculation
            {
                get
                {
                    return ( this.criterion & RemarkCriterion.CanRecalculation ) != 0;
                }
            }

            /// <summary>
            /// 検量線登録可能フラグの取得
            /// </summary>
            public Boolean CanRegistCalibCurve
            {
                get
                {
                    return ( this.criterion & RemarkCriterion.CanRegistCalibCurve ) != 0;
                }
            }

            /// <summary>
            /// 再計算後引き継ぎ可能フラグの取得
            /// </summary>
            public Boolean CanAfterReCalcInherit
            {
                get
                {
                    return ( this.criterion & RemarkCriterion.CanAfterReCalcInherit ) != 0;
                }
            }

            /// <summary>
            /// 注意ポイント包含可能フラグの取得
            /// </summary>
            public Boolean CanIncludingCautionPoint
            {
                get
                {
                    return ( this.criterion & RemarkCriterion.CanIncludingCautionPoint ) != 0;
                }
            }
            #endregion

            #region _カテゴリ_
            
            /// <summary>
            /// 温度エラーの取得
            /// </summary>
            public Boolean IsTemperatureError
            {
                get
                {
                    return ( this.category & RemarkCategory.TemperatureError) != 0;
                }
            }
            /// <summary>
            /// キャリブレーションエラーの取得
            /// </summary>
            public Boolean IsCalibrationError
            {
                get
                {
                    return ( this.category & RemarkCategory.CalibrationError ) != 0;
                }
            }
            /// <summary>
            /// 有効期限エラーの取得
            /// </summary>
            public Boolean IsExpirationDataError
            {
                get
                {
                    return ( this.category & RemarkCategory.ExpirationDataError ) != 0;
                }
            }
            /// <summary>
            /// データ警告の取得
            /// </summary>
            public Boolean IsDataWarning
            {
                get
                {
                    return ( this.category & RemarkCategory.DataWarning ) != 0;
                }
            }
            /// <summary>
            /// データ編集の取得
            /// </summary>
            public Boolean IsDataEdited
            {
                get
                {
                    return ( this.category & RemarkCategory.DataEdited ) != 0;
                }
            }
            /// <summary>
            /// オンラインの取得
            /// </summary>
            public Boolean IsOnLine
            {
                get
                {
                    return ( this.category & RemarkCategory.OnLine ) != 0;
                }
            }
            #endregion

            #endregion

            #region [publicメソッド]
            /// <summary>
            /// リマーク判定ビット一致判定
            /// </summary>
            /// <remarks>
            /// 指定のリマーク判定ビットを有する(一致するものがある)かどうかを取得します。
            /// </remarks>
            /// <param name="criterion">リマーク判定ビット</param>
            /// <returns>true:一致あり</returns>
            public Boolean MatchCondition( RemarkCriterion criterion )
            {
                return ( this.criterion & criterion ) > 0;
            }
            /// <summary>
            /// リマークカテゴリビット一致判定
            /// </summary>
            /// <remarks>
            /// 指定のリマークカテゴリビットを有する(一致するものがある)かどうかを取得します。
            /// </remarks>
            /// <param name="category">リマークカテゴリビット</param>
            /// <returns>true:一致あり</returns>
            public Boolean MatchCondition( RemarkCategory category )
            {
                return ( this.category & category ) > 0;
            }
            #endregion

        }

        #endregion

    }
}
