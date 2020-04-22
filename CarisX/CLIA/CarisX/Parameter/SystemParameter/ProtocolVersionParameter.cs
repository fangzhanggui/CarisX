using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// プロトコルバージョン番号設定
    /// </summary>
    public class ProtocolVersionParameter : AttachmentParameter
    {
        /// <summary>
        /// プロトコルバージョン番号
        /// </summary>
        public String ProtocolVersion { get; set; } = String.Empty;
    }
}
