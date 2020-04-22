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
    /// ユーザー用モジュールチェックパラメータ
    /// </summary>
    public class CarisXMaintenanceUserParameter : ISavePath
    {
        #region ISavePath メンバー
        /// <summary>
        /// 保存パス
        /// </summary>
        public String SavePath
        {
            get
            {
                return CarisXConst.PathParam + @"\MaintenanceUserParameter.xml";
            }
        }
        #endregion

        public List<ModuleConfig> SlaveList;

        // デイリー全チェックフラグ
        public Boolean AllCheckDaily;
        // ウィークリー全チェックフラグ
        public Boolean AllCheckWeekly;
        // マンスリー全チェックフラグ
        public Boolean AllCheckMonthly;
        // デイリー完了最終日時
        public DateTime AllFinishDaily;
        // ウィークリー完了最終日時
        public DateTime AllFinishWeekly;
        // マンスリー完了最終日時
        public DateTime AllFinishMonthly;

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
            /// デイリーチェック項目
            /// </summary>
            public List<Boolean> DailyCheckItem = new List<Boolean>();

            /// <summary>
            /// ウィークリーチェック項目
            /// </summary>
            public List<Boolean> WeeklyCheckItem = new List<Boolean>();

            /// <summary>
            /// マンスリーチェック項目
            /// </summary>
            public List<Boolean> MonthlyCheckItem = new List<Boolean>();

            #endregion

        }

    }
}
