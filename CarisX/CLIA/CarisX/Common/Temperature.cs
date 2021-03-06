using System;

namespace Oelco.CarisX.Common
{
    /// <summary>
    /// ·xÝè
    /// </summary>
	public class Temperature
	{
        #region [vpeB]

        /// <summary>
        /// òÛâÉ·xij
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Double TempReagentCoolingBox { get; set; } = 0;

        /// <summary>
        /// º·ij
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Double TempRoom { get; set; } = 0;

        /// <summary>
        /// u·xij
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public Double TempDevice { get; set; } = 0;

        /// <summary>
        /// ½e[u@·xij
        /// </summary>
        public Double TempReactionTable { get; set; } = 0;

        /// <summary>
        /// BFe[u@·xij
        /// </summary>
        public Double TempBFTable { get; set; } = 0;

        /// <summary>
        /// B/F1vq[g@·xij
        /// </summary>
        public Double TempBF1PreHeat { get; set; } = 0;

        /// <summary>
        /// B/F2vq[g@·xij
        /// </summary>
        public Double TempBF2PreHeat { get; set; } = 0;

        /// <summary>
        /// R1v[uvq[g@·xij
        /// </summary>
        public Double TempR1ProbePreHeat { get; set; } = 0;

        /// <summary>
        /// R2v[uvq[g@·xij
        /// </summary>
        public Double TempR2ProbePreHeat { get; set; } = 0;

        /// <summary>
        /// ªõ@·xij
        /// </summary>
        public Double TempChemiLightMeas { get; set; } = 0;

        #endregion
    }

}
 
