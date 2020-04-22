using System;

namespace Oelco.CarisX.Common
{
    /// <summary>
    /// 温度設定
    /// </summary>
	public class Temperature
	{
        #region [プロパティ]

        /// <summary>
        /// 試薬保冷庫温度（℃）
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Double TempReagentCoolingBox { get; set; } = 0;

        /// <summary>
        /// 室温（℃）
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Double TempRoom { get; set; } = 0;

        /// <summary>
        /// 装置温度（℃）
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Double TempDevice { get; set; } = 0;

        /// <summary>
        /// 反応テーブル　温度（℃）
        /// </summary>
        public Double TempReactionTable { get; set; } = 0;

        /// <summary>
        /// BFテーブル　温度（℃）
        /// </summary>
        public Double TempBFTable { get; set; } = 0;

        /// <summary>
        /// B/F1プレヒート　温度（℃）
        /// </summary>
        public Double TempBF1PreHeat { get; set; } = 0;

        /// <summary>
        /// B/F2プレヒート　温度（℃）
        /// </summary>
        public Double TempBF2PreHeat { get; set; } = 0;

        /// <summary>
        /// R1プローブプレヒート　温度（℃）
        /// </summary>
        public Double TempR1ProbePreHeat { get; set; } = 0;

        /// <summary>
        /// R2プローブプレヒート　温度（℃）
        /// </summary>
        public Double TempR2ProbePreHeat { get; set; } = 0;

        /// <summary>
        /// 測光部　温度（℃）
        /// </summary>
        public Double TempChemiLightMeas { get; set; } = 0;

        #endregion
    }

}
 
