using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolConverter.File
{
    public enum AssaySequenceKind
    {
        OneStep = 1,        // 1ステップ法
        TwoStep,            // 2ステップ法
        TwoStepMinus,       // 2ステップ法マイナス
        OnePointFiveStep    // 1.5ステップ法
    }

    public enum PreProcessSequenceKind
    {
        None = 0,           // なし
        SR1,                // sR1タイプ
        ST1,                // sT1タイプ
        ST1T2,              // sT1T2タイプ
        ST1SR1,             // sT1sR1タイプ
        ST1ST2              // sT1sT2タイプ
    }

    public enum SampleTypeKind
    {
        SerumOrPlasma = 1,    // 血清または血漿
        Urine                       // 尿
    }

    public enum AutoDilutionReTestRatioKind : int
    {
        x10 = 10,         // ×10
        x100 = 100,       // ×100
        x200 = 200,       // ×200
        x1000 = 1000      // ×1000
    }

    public enum CalibrationType
    {
        Spline = 1,     // Spline
        LogitLog,       // Logit-Log　3次回帰
        FourParameters, // 4 Parameters
        CutOff,         // カットオフタイプ
        INH,            // 抑制率タイプ
        NoType          // なし
    }

    public enum CalibrationMethod
    {
        FullCalibration = 0,    // フルキャリブレーション
        MasterCalibration,      // マスタキャリブレーション
    }

    public class ItemRange
    {
        public Double Max = 0;
        public Double Min = 0;
    }
    
    /// <summary>
    /// Enumの拡張メソッドクラス
    /// </summary>
    public static class EnumExtention
    {
        /// <summary>
        /// 値を文字型に変換し取得する
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static String ValueToString( this CalibrationType type )
        {
            return Convert.ToInt32( type ).ToString();
        }
    }

    public class ProtocolXmlControl
    {
        
        
    }
}
