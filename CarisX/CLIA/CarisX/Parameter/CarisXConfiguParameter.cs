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
    /// コンフィグパラメータ
    /// </summary>
    /// <remarks>
    /// パラメータを追加する際には、初期値を設定しておく。
    /// ※インストーラを配信した時にパラメータを上書きしない為、
    /// 　初期値を設定しておかないとパラメータが追加されない
    /// </remarks>
    public class CarisXConfigParameter : ISavePath
    {

        #region [ISavePath メンバー]

        /// <summary>
        /// 保存パス
        /// </summary>
        public String SavePath
        {
            get
            {
                return CarisXConst.PathParam + @"\ConfigParameter.xml";
            }
        }

        /// <summary>
        /// バックアップ保存パス
        /// </summary>
        public String BackupSavePath
        {
            get
            {
                return CarisXConst.PathBackupParam + @"\ConfigParameter.xml";
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
            /// ラック搬送ユニットパラメータ
            /// </summary>
            public RackTransferUnitConfigParam rackTransferUnitConfigParam;

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
            /// ケース搬送ユニットパラメータ
            /// </summary>
            public TipCellCaseTransferUnitConfigParam tipCellCaseTransferUnitConfigParam;
            /// <summary>
            /// 試薬保冷庫ユニットパラメータ
            /// </summary>
            public ReagentStorageUnitConfigParam reagentStorageUnitConfigParam;
            /// <summary>
            /// STATユニットパラメータ
            /// </summary>
            public STATUnitConfigParam sTATUnitConfigParam;
            /// <summary>
            /// サンプル分注ユニットパラメータ
            /// </summary>
            public SampleDispenseConfigParam sampleDispenseConfigParam;
            /// <summary>
            /// 反応容器搬送ユニット
            /// </summary>
            public ReactionCellTransferUnitConfigParam reactionCellTransferUnitConfigParam;
            /// <summary>
            /// 反応テーブルユニット
            /// </summary>
            public ReactionTableUnitConfigParam reactionTableUnitConfigParam;
            /// <summary>
            /// BFテーブルユニット
            /// </summary>
            public BFTableUnitConfigParam bFTableUnitConfigParam;
            /// <summary>
            /// トラベラー・廃棄ユニットパラメータ
            /// </summary>
            public TravelerAndDisposalUnitConfigParam travelerAndDisposalUnitConfigParam;
            /// <summary>
            /// R1分注ユニットパラメータ
            /// </summary>
            public R1DispenseUnitConfigParam r1DispenseUnitConfigParam;
            /// <summary>
            /// R2分注ユニットパラメータ
            /// </summary>
            public R2DispenseUnitConfigParam r2DispenseUnitConfigParam;
            /// <summary>
            /// B/F1ユニットパラメータ
            /// </summary>
            public BF1UnitConfigParam bF1UnitConfigParam;
            /// <summary>
            /// B/F2ユニットパラメータ
            /// </summary>
            public BF2UnitConfigParam bF2UnitConfigParam;
            /// <summary>
            /// 希釈分注ユニットパラメータ
            /// </summary>
            public DilutionDispenseUnitConfigParam dilutionDispenseUnitConfigParam;
            /// <summary>
            /// プレトリガ分注ユニットパラメータ
            /// </summary>
            public PretriggerUnitConfigParam pretriggerUnitConfigParam;
            /// <summary>
            /// トリガ分注ユニットパラメータ
            /// </summary>
            public TriggerUnitConfigParam triggerUnitConfigParam;
            /// <summary>
            /// 流体配管ユニットパラメータ
            /// </summary>
            public FluidAndPipingUnitConfigParam fluidAndPipingUnitConfigParam;

            #endregion
        }

        //ラック搬送ユニット
        public class RackTransferUnitConfigParam : RackTransferCommCommand_0047 { }
        //ケース搬送ユニット
        public class TipCellCaseTransferUnitConfigParam : SlaveCommCommand_0448 { }
        //試薬保冷庫ユニット
        public class ReagentStorageUnitConfigParam : SlaveCommCommand_0449 { }
        //STATユニット
        public class STATUnitConfigParam : SlaveCommCommand_0450 { }
        //サンプル分注ユニット
        public class SampleDispenseConfigParam : SlaveCommCommand_0451 { }
        //反応容器搬送ユニット
        public class ReactionCellTransferUnitConfigParam : SlaveCommCommand_0452 { }
        //反応テーブルユニット
        public class ReactionTableUnitConfigParam : SlaveCommCommand_0453 { }
        //BFテーブルユニット
        public class BFTableUnitConfigParam : SlaveCommCommand_0454 { }
        //トラベラー・廃棄ユニット
        public class TravelerAndDisposalUnitConfigParam : SlaveCommCommand_0455 { }
        //R1分注ユニット
        public class R1DispenseUnitConfigParam : SlaveCommCommand_0456 { }
        //R2分注ユニット
        public class R2DispenseUnitConfigParam : SlaveCommCommand_0457 { }
        //B/F1ユニット
        public class BF1UnitConfigParam : SlaveCommCommand_0458 { }
        //B/F2ユニット
        public class BF2UnitConfigParam : SlaveCommCommand_0459 { }
        //希釈分注ユニット
        public class DilutionDispenseUnitConfigParam : SlaveCommCommand_0460 { }
        //プレトリガ分注ユニット
        public class PretriggerUnitConfigParam : SlaveCommCommand_0461 { }
        //トリガ分注ユニット
        public class TriggerUnitConfigParam : SlaveCommCommand_0462 { }
        //流体配管ユニット
        public class FluidAndPipingUnitConfigParam : SlaveCommCommand_0463 { }
    }
}
