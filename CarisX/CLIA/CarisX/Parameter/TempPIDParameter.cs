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
    /// <summary>
    /// 温度PIDパラメータ
    /// </summary>
    public class TempPIDParameter : ISavePath
    {

        #region [ISavePath メンバー]

        /// <summary>
        /// 保存パス
        /// </summary>
        public String SavePath
        {
            get
            {
                return CarisXConst.PathParam + @"\TempPIDParameter.xml";
            }
        }

        #endregion

        public List<ModuleConfig> SlaveList;

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
            public ReactionTableTempPIDParam reactionTableTempPIDParam;
            /// <summary>
            /// BFテーブル温度
            /// </summary>
            public BFTableTempPIDParam bFTableTempPIDParam;
            /// <summary>
            /// B/F1温度
            /// </summary>
            public BF1TempPIDParam bF1TempPIDParam;
            /// <summary>
            /// B/F2温度
            /// </summary>
            public BF2TempPIDParam bF2TempPIDParam;
            /// <summary>
            /// R1温度
            /// </summary>
            public R1TempPIDParam r1TempPIDParam;
            /// <summary>
            /// R2温度
            /// </summary>
            public R2TempPIDParam r2TempPIDParam;
            /// <summary>
            /// 化学発光測定部温度
            /// </summary>
            public PtotometryTempPIDParam ptotometryTempPIDParam;

            #endregion
        }

        //反応テーブル温度
        public class ReactionTableTempPIDParam : SlaveCommCommand_0474 { }

        //BFテーブル温度
        public class BFTableTempPIDParam : SlaveCommCommand_0474 { }

        //B/F1温度
        public class BF1TempPIDParam : SlaveCommCommand_0474 { }

        //B/F2温度
        public class BF2TempPIDParam : SlaveCommCommand_0474 { }

        //R1温度
        public class R1TempPIDParam : SlaveCommCommand_0474 { }

        //R2温度
        public class R2TempPIDParam : SlaveCommCommand_0474 { }

        //化学発光測定部温度
        public class PtotometryTempPIDParam : SlaveCommCommand_0474 { }
    }
}
