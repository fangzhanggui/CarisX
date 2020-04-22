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
    /// センサー使用有無パラメータ
    /// </summary>
    public class CarisXSensorParameter : ISavePath
    {
        #region ISavePath メンバー
        /// <summary>
        /// 保存パス
        /// </summary>
        public String SavePath
        {
            get
            {
                return CarisXConst.PathParam + @"\SensorParameter.xml";
            }
        }

        /// <summary>
        /// バックアップ保存パス
        /// </summary>
        public String BackupSavePath
        {
            get
            {
                return CarisXConst.PathBackupParam + @"\SensorParameter.xml";
            }
        }


        #endregion

        public List<RackConfig> RackList;
        public List<ModuleConfig> SlaveList;

        public class RackConfig
        {
            #region [変数定義]

            /// <summary>
            /// シリアルＮＯ
            /// </summary>
            public String SerialNo { get; set; }

            /// <summary>
            /// モジュールＩＤ
            /// </summary>
            public Int32 ModuleId { get; set; }

            /// <summary>
            /// センサーステータス使用有無
            /// </summary>
            public SensorParameterUseNoUseRack sensorParameterUseNoUse;

            #endregion
        }

        public class ModuleConfig
        {
            #region [変数定義]

            /// <summary>
            /// シリアルＮＯ
            /// </summary>
            public String SerialNo { get; set; }

            /// <summary>
            /// モジュールＩＤ
            /// </summary>
            public Int32 ModuleId { get; set; }

            /// <summary>
            /// センサーステータス使用有無
            /// </summary>
            public SensorParameterUseNoUseSlave sensorParameterUseNoUse;

            #endregion
        }

        /// <summary>
        /// センサーステータス使用有無
        /// </summary>
        public class SensorParameterUseNoUseRack : RackTransferCommCommand_0041 { }
        public class SensorParameterUseNoUseSlave : SlaveCommCommand_0441 { }

    }

    /// <summary>
    /// Use/NoUse 
    /// </summary>
    public enum UseStatus
    {
        /// <summary>
        /// NoUse
        /// </summary>
        NoUse = 1,
        /// <summary>
        /// Use
        /// </summary>
        Use = 0,
    }

    /// <summary>
    /// センサーステータス追記
    /// </summary>
    public enum SenserStatusKind
    {
        /// <summary>
        /// Use
        /// </summary>
        OFF = 0,
        /// <summary>
        /// ON
        /// </summary>
        ON = 1,
    }
}
