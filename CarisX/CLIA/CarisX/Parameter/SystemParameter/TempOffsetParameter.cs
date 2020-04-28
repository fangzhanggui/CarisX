using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.Parameter;
using Oelco.CarisX.Comm;
using System.Windows.Forms;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.Parameter
{
    ///【IssuesNo:4】温度补偿校准相关参数
    /// <summary>
    /// 温度Offsetパラメータ
    /// </summary>
    public class TempOffsetParameter : ISavePath
    {

        #region [ISavePath メンバー]

        public const double CONST_DEFAULT_TEMP = 1.0;

        /// <summary>
        /// 保存パス
        /// </summary>
        public String SavePath
        {
            get
            {
                return CarisXConst.PathParam + @"\TempOffsetParameter.xml";
            }
        }

        #endregion


        public List<ModuleConfig> SlaveList
        {
            get; set;
        }

        public class ModuleConfig
        {
            #region [変数定義]

            /// <summary>
            /// シリアルＮＯ
            /// </summary>
            public String SerialNo { get; set; }

            /// <summary>
            /// 反応テーブル温度
            /// </summary>
            public Double reactionTableTempOffsetParam { get; set; }
            /// <summary>
            /// BFテーブル温度
            /// </summary>
            public Double bFTableTempOffsetParam { get; set; }
            /// <summary>
            /// B/F1温度
            /// </summary>
            public Double bF1TempOffsetParam { get; set; }
            /// <summary>
            /// B/F2温度
            /// </summary>
            public Double bF2TempOffsetParam { get; set; }
            /// <summary>
            /// R1温度
            /// </summary>
            public Double r1TempOffsetParam { get; set; }
            /// <summary>
            /// R2温度
            /// </summary>
            public Double r2TempOffsetParam { get; set; }
            /// <summary>
            /// 化学発光測定部温度
            /// </summary>
            public Double ptotometryTempOffsetParam { get; set; }

            #endregion
        }

    }
}
