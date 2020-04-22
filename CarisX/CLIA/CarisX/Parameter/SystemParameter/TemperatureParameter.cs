using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// 温度設定
    /// </summary>
	public class TemperatureParameter : AttachmentParameter
	{
        #region [定数定義]

        /// <summary>
        /// 反応テーブル　最低温度設定（℃）
        /// </summary>
        public const Double TEMP_REACT_TBL_MIN = 30.0;
        /// <summary>
        /// 反応テーブル　最高温度設定（℃）
        /// </summary>
        public const Double TEMP_REACT_TBL_MAX = 70.0;
        /// <summary>
        /// BFテーブル　最低温度設定（℃）
        /// </summary>
        public const Double TEMP_BF_TBL_MIN = 30.0;
        /// <summary>
        /// BFテーブル　最高温度設定（℃）
        /// </summary>
        public const Double TEMP_BF_TBL_MAX = 70.0;
        /// <summary>
        /// B/F1プレヒート温度　最低温度設定（℃）
        /// </summary>
        public const Double TEMP_BF1_PRE_HEAT_MIN = 30.0;
        /// <summary>
        /// B/F1プレヒート温度　最高温度設定（℃）
        /// </summary>
        public const Double TEMP_BF1_PRE_HEAT_MAX = 70.0;
        /// <summary>
        /// B/F2プレヒート温度　最低温度設定（℃）
        /// </summary>
        public const Double TEMP_BF2_PRE_HEAT_MIN = 30.0;
        /// <summary>
        /// B/F2プレヒート温度　最高温度設定（℃）
        /// </summary>
        public const Double TEMP_BF2_PRE_HEAT_MAX = 70.0;
        /// <summary>
        /// R1プローブプレヒート温度　最低温度設定（℃）
        /// </summary>
        public const Double TEMP_R1_PROBE_PRE_HEAT_MIN = 30.0;
        /// <summary>
        /// R1プローブプレヒート温度　最高温度設定（℃）
        /// </summary>
        public const Double TEMP_R1_PROBE_PRE_HEAT_MAX = 70.0;
        /// <summary>
        /// R2プローブプレヒート温度　最低温度設定（℃）
        /// </summary>
        public const Double TEMP_R2_PROBE_PRE_HEAT_MIN = 30.0;
        /// <summary>
        /// R2プローブプレヒート温度　最高温度設定（℃）
        /// </summary>
        public const Double TEMP_R2_PROBE_PRE_HEAT_MAX = 70.0;
        /// <summary>
        /// 測光部温度　最低温度設定（℃）
        /// </summary>
        public const Double TEMP_CHEMI_LIGHT_MEAS_MIN = 30.0;
        /// <summary>
        /// 測光部温度　最高温度設定（℃）
        /// </summary>
        public const Double TEMP_CHEMI_LIGHT_MEAS_MAX = 70.0;

        /// <summary>
        /// 反応テーブル　デフォルト値設定（℃）
        /// </summary>
        public const Double TEMP_REACT_TBL_DEFAULT = TEMP_REACT_TBL_MIN;
        /// <summary>
        /// BFテーブル　デフォルト値設定（℃）
        /// </summary>
        public const Double TEMP_BF_TBL_DEFAULT = TEMP_BF_TBL_MIN;
        /// <summary>
        /// B/F1プレヒート温度　デフォルト値設定（℃）
        /// </summary>
        public const Double TEMP_BF1_PRE_HEAT_DEFAULT = TEMP_BF1_PRE_HEAT_MIN;
        /// <summary>
        /// B/F2プレヒート温度　デフォルト値設定（℃）
        /// </summary>
        public const Double TEMP_BF2_PRE_HEAT_DEFAULT = TEMP_BF2_PRE_HEAT_MIN;
        /// <summary>
        /// R1プローブプレヒート温度　デフォルト値設定（℃）
        /// </summary>
        public const Double TEMP_R1_PROBE_PRE_HEAT_DEFAULT = TEMP_R1_PROBE_PRE_HEAT_MIN;
        /// <summary>
        /// R2プローブプレヒート温度　デフォルト値設定（℃）
        /// </summary>
        public const Double TEMP_R2_PROBE_PRE_HEAT_DEFAULT = TEMP_R2_PROBE_PRE_HEAT_MIN;
        /// <summary>
        /// 測光部温度　デフォルト値設定（℃）
        /// </summary>
        public const Double TEMP_CHEMI_LIGHT_MEAS_DEFAULT = TEMP_CHEMI_LIGHT_MEAS_MIN;

        /// <summary>
        /// 温度エラー時サンプリング停止有無
        /// </summary>
        public const Byte TEMP_ERROR_SAMPLING_STOP_EXISTENCE_DEFAULT = 0;

        /// <summary>
        /// 試薬保冷庫デフォルト値
        /// </summary>
        public const Double TEMP_REAGENT_COOLING_BOX_DEFAULT = 0.0d;
        /// <summary>
        /// 室温デフォルト値
        /// </summary>
        public const Double TEMP_ROOM_DEFAULT = 0.0d;
        /// <summary>
        /// 装置温度デフォルト値
        /// </summary>
        public const Double TEMP_DEVICE_DEFAULT = 0.0d;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 試薬保冷庫温度（℃）
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Double TempReagentCoolingBox { get; set; } = TEMP_REAGENT_COOLING_BOX_DEFAULT;

        /// <summary>
        /// 室温（℃）
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Double TempRoom { get; set; } = TEMP_ROOM_DEFAULT;

        /// <summary>
        /// 装置温度（℃）
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Double TempDevice { get; set; } = TEMP_DEVICE_DEFAULT;

        /// <summary>
        /// 反応テーブル　温度（℃）
        /// </summary>
        public Double TempReactionTable { get; set; } = TEMP_REACT_TBL_DEFAULT;

        /// <summary>
        /// 反応テーブル　オフセット（℃）
        /// </summary>
        public Double TempReactionTableOffset { get; set; } = TEMP_REACT_TBL_DEFAULT;

        /// <summary>
        /// BFテーブル　温度（℃）
        /// </summary>
        public Double TempBFTable { get; set; } = TEMP_BF_TBL_DEFAULT;

        /// <summary>
        /// BFテーブル　オフセット（℃）
        /// </summary>
        public Double TempBFTableOffset { get; set; } = TEMP_BF_TBL_DEFAULT;

        /// <summary>
        /// B/F1プレヒート　温度（℃）
        /// </summary>
        public Double TempBF1PreHeat { get; set; } = TEMP_BF1_PRE_HEAT_DEFAULT;

        /// <summary>
        /// B/F1プレヒート　オフセット（℃）
        /// </summary>
        public Double TempBF1PreHeatOffset { get; set; } = TEMP_BF1_PRE_HEAT_DEFAULT;

        /// <summary>
        /// B/F2プレヒート　温度（℃）
        /// </summary>
        public Double TempBF2PreHeat { get; set; } = TEMP_BF2_PRE_HEAT_DEFAULT;

        /// <summary>
        /// B/F2プレヒート　オフセット（℃）
        /// </summary>
        public Double TempBF2PreHeatOffset { get; set; } = TEMP_BF2_PRE_HEAT_DEFAULT;

        /// <summary>
        /// R1プローブプレヒート　温度（℃）
        /// </summary>
        public Double TempR1ProbePreHeat { get; set; } = TEMP_R1_PROBE_PRE_HEAT_DEFAULT;

        /// <summary>
        /// R1プローブプレヒート　オフセット（℃）
        /// </summary>
        public Double TempR1ProbePreHeatOffset { get; set; } = TEMP_R1_PROBE_PRE_HEAT_DEFAULT;

        /// <summary>
        /// R2プローブプレヒート　温度（℃）
        /// </summary>
        public Double TempR2ProbePreHeat { get; set; } = TEMP_R2_PROBE_PRE_HEAT_DEFAULT;

        /// <summary>
        /// R2プローブプレヒート　オフセット（℃）
        /// </summary>
        public Double TempR2ProbePreHeatOffset { get; set; } = TEMP_R2_PROBE_PRE_HEAT_DEFAULT;

        /// <summary>
        /// 測光部　温度（℃）
        /// </summary>
        public Double TempChemiLightMeas { get; set; } = TEMP_CHEMI_LIGHT_MEAS_DEFAULT;

        /// <summary>
        /// 測光部　オフセット（℃）
        /// </summary>
        public Double TempChemiLightMeasOffset { get; set; } = TEMP_CHEMI_LIGHT_MEAS_DEFAULT;

        /// <summary>
        /// 温度エラー時のサンプリング停止有無
        /// </summary>
        public Byte TempErrorSamplingStopExistence { get; set; } = TEMP_ERROR_SAMPLING_STOP_EXISTENCE_DEFAULT;

        #endregion
    }

}
 
