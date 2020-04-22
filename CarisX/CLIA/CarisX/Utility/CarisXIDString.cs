using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;
using System.Text.RegularExpressions;

using Oelco.CarisX.Const;


namespace Oelco.CarisX.Utility
{
    /// <summary>
    /// ID文字列クラス
    /// </summary>
    public abstract class CarisXIDString : IDString// PrecharType : IIDString, new()
    {
        /// <summary>
        /// 文字列をID文字列に変換
        /// </summary>
        /// <remarks>
        /// 文字列をID文字列に変換し、評価結果を返します
        /// </remarks>
        /// <param name="value"></param>
        /// <param name="idString"></param>
        /// <returns></returns>
        public static Boolean TryParse(String value, out CarisXIDString idString)
        {
            idString = null;

            // フォーマット確認
            CarisXIDString id = format(value);
            String preRemoved = value.Remove(0, id.PreChar.Length);
            Int32 parsed;
            Boolean isNum = Int32.TryParse(preRemoved, out parsed);
            if (isNum)
            {
                id.Value = parsed;
                idString = id;
            }

            return isNum;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator CarisXIDString(String value)
        {
            CarisXIDString id = format(value);
            id.DispPreCharString = value;
            return id;
        }

        /// <summary>
        /// 文字列をID文字列のフォーマットに変換
        /// </summary>
        /// <remarks>
        /// 文字列をID文字列のフォーマットに変換します
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        private static CarisXIDString format(String value)
        {
            Regex regex = new Regex("^(?<prechar>([a-z]|[A-Z]|[ ])+)");
            Match match = regex.Match(value);

            CarisXIDString id = null;

            if (match.Success)
            {
                switch (match.Groups["prechar"].Value)
                {
                    case CarisXConst.CALIB_RACK_ID_PRECHAR:
                        id = new CalibRackID();
                        break;
                    case CarisXConst.CONTROL_RACK_ID_PRECHAR:
                        id = new ControlRackID();
                        break;
                    case CarisXConst.STAT_RACK_ID_PRECHAR:
                        id = new StatRackID();
                        break;
                    default:
                        // 解析失敗
                        if (value == Properties.Resources.STRING_COMMON_028)
                            id = new StatRackID();      //「STAT」の文字の場合はSTAT
                        else
                            id = new GeneralRackID();   //STATでもない場合は一般検体
                        break;
                }
            }
            else
            {
                // 一般
                id = new GeneralRackID();
            }
            return id;
        }

        /// <summary>
        /// 表示用ID文字列変換
        /// </summary>
        /// <remarks>
        /// 表示用ID文字列の文字列を返します。
        /// STATの場合はvalueが０になってしまうので、valueの値を見ないようにする
        /// </remarks>
        /// <returns>表示用ID文字列</returns>
        public override String ToString()
        {
            if (this.GetSampleKind() == SampleKind.Priority)
            {
                return Properties.Resources.STRING_COMMON_028;
            }
            else
            {
                return base.ToString();
            }
        }
    }

    /// <summary>
    /// サンプル種別識別子クラス
    /// </summary>
    static public class SampleKindIDentifer
    {
        /// <summary>
        /// サンプル種別取得
        /// </summary>
        /// <remarks>
        /// サンプル種別取得します
        /// </remarks>
        /// <param name="idString"></param>
        /// <returns></returns>
        static public SampleKind GetSampleKind(this CarisXIDString idString)
        {
            SampleKind kind = SampleKind.Sample;
            switch (idString.PreChar)
            {
                case CarisXConst.GENERAL_RACK_ID_PRECHAR:
                    kind = SampleKind.Sample;
                    break;
                case CarisXConst.CALIB_RACK_ID_PRECHAR:
                    kind = SampleKind.Calibrator;
                    break;
                case CarisXConst.CONTROL_RACK_ID_PRECHAR:
                    kind = SampleKind.Control;
                    break;
                case CarisXConst.STAT_RACK_ID_PRECHAR:
                    kind = SampleKind.Priority;
                    break;
                default:
                    break;
            }
            return kind;
        }
    }

    /// <summary>
    /// 一般検体ラックIDクラス
    /// </summary>
    public class GeneralRackID : CarisXIDString
    {
        #region IPreCharID メンバー
        // Staticにしたいが、インターフェースはStaticに出来ない。
        /// <summary>
        /// 接頭文字の取得
        /// </summary>
        public override String PreChar
        {
            get
            {
                return CarisXConst.GENERAL_RACK_ID_PRECHAR;
            }
        }
        /// <summary>
        /// フォーマット文字列の取得
        /// </summary>
        public override String StringFormat
        {
            get
            {
                return "0000";
            }
        }

        #endregion
    }

    /// <summary>
    /// STAT検体ラックIDクラス
    /// </summary>
    public class StatRackID : CarisXIDString
    {
        #region IPreCharID メンバー
        // Staticにしたいが、インターフェースはStaticに出来ない。
        /// <summary>
        /// 接頭文字の取得
        /// </summary>
        public override String PreChar
        {
            get
            {
                return CarisXConst.STAT_RACK_ID_PRECHAR;
            }
        }
        /// <summary>
        /// フォーマット文字列の取得
        /// </summary>
        public override String StringFormat
        {
            get
            {
                return "000";
            }
        }

        #endregion
    }

    /// <summary>
    /// キャリブレータラックIDクラス
    /// </summary>
    public class CalibRackID : CarisXIDString
    {
        #region IPreCharID メンバー

        /// <summary>
        /// 接頭文字の取得
        /// </summary>
        public override String PreChar
        {
            get
            {
                return CarisXConst.CALIB_RACK_ID_PRECHAR;
            }
        }
        /// <summary>
        /// フォーマット文字列の取得
        /// </summary>
        public override String StringFormat
        {
            get
            {
                return "000";
            }
        }

        #endregion
    }

    /// <summary>
    /// 精度管理検体ラックIDクラス
    /// </summary>
    public class ControlRackID : CarisXIDString
    {
        #region IPreCharID メンバー

        /// <summary>
        /// 接頭文字の取得
        /// </summary>
        public override String PreChar
        {
            get
            {
                return CarisXConst.CONTROL_RACK_ID_PRECHAR;
            }
        }
        /// <summary>
        /// フォーマット文字列の取得
        /// </summary>
        public override String StringFormat
        {
            get
            {
                return "000";
            }
        }

        #endregion
    }

}
