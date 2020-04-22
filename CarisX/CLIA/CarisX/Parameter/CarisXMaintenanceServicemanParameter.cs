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
    /// サービスマン用モジュールチェックパラメータ
    /// </summary>
    public class CarisXMaintenanceServicemanParameter : ISavePath
    {
        #region ISavePath メンバー
        /// <summary>
        /// 保存パス
        /// </summary>
        public String SavePath
        {
            get
            {
                return CarisXConst.PathParam + @"\MaintenanceServicemanParameter.xml";
            }
        }
        #endregion

        public List<ModuleConfig> SlaveList;

        // マンスリー全チェックフラグ
        public Boolean AllCheckMonthly;
        // マンスリー全チェックフラグ
        public Boolean AllCheckYearly;
        // マンスリー完了最終日時
        public DateTime AllFinishMonthly;
        // イヤーリー完了最終日時
        public DateTime AllFinishYearly;

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
            /// マンスリーチェック項目
            /// </summary>
            public List<Boolean> MonthlyCheckItem = new List<Boolean>();

            /// <summary>
            /// マンスリーチェック項目
            /// </summary>
            public List<Boolean> YearlyCheckItem = new List<Boolean>();

            #endregion

        }

    }
}
