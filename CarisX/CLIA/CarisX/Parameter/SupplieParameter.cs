using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.CarisX.Const;
using Oelco.Common.Parameter;
using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// 消耗品パラメータクラス
    /// </summary>
    public class SupplieParameter : ISavePath
    {

        #region ISavePath メンバー
        /// <summary>
        /// 保存パス
        /// </summary>
        public string SavePath
        {
            get
            {
                return CarisXConst.PathSystem + @"\SupplieParam.xml";
            }
        }

        /// <summary>
        /// バックアップ保存パス
        /// </summary>
        public String BackupSavePath
        {
            get
            {
                return CarisXConst.PathBackupSystem + @"\SupplieParam.xml";
            }
        }

        #endregion

        /// <summary>
        /// スレーブ設定
        /// </summary>
        public List<ModuleConfig> SlaveList;

        /// <summary>
        /// モジュール設定
        /// </summary>
        public class ModuleConfig
        {
            /// <summary>
            /// シリアルＮＯ
            /// </summary>
            public String SerialNo { get; set; }

            /// <summary>
            /// モジュールＩＤ
            /// </summary>
            public Int32 ModuleId { get; set; }

            // 消耗品情報
            // 参照↓↓
            // C仕様書 12.1
            // G説明書 239P
            // G消耗品交換画面

            /// <summary>
            /// 検体分注シリンジパッキン
            /// </summary>
            public CountTypeLife SampleDispensingSyringePackin = new CountTypeLife();
            /// <summary>
            /// R1分注シリンジパッキン
            /// </summary>
            public CountTypeLife R1DispensingSyringePackin = new CountTypeLife();
            /// <summary>
            /// R2分注シリンジパッキン
            /// </summary>
            public CountTypeLife R2DispensingSyringePackin = new CountTypeLife();
            /// <summary>
            /// 試薬分注洗浄液シリンジパッキン
            /// </summary>
            public CountTypeLife ReagentDispensingSyringePackin = new CountTypeLife();
            /// <summary>
            /// 希釈液分注シリンジパッキン
            /// </summary>
            public CountTypeLife DiluentDispensingSyringePackin = new CountTypeLife();
            /// <summary>
            /// 洗浄１分注シリンジパッキン
            /// </summary>
            public CountTypeLife Wash1DispensingSyringePackin = new CountTypeLife();
            /// <summary>
            /// 洗浄２分注シリンジパッキン
            /// </summary>
            public CountTypeLife Wash2DispensingSyringePackin = new CountTypeLife();
            /// <summary>
            /// プレトリガ分注シリンジパッキン
            /// </summary>
            public CountTypeLife PreTriggerDispensingSyringePackin = new CountTypeLife();
            /// <summary>
            /// トリガ分注シリンジパッキン
            /// </summary>
            public CountTypeLife TriggerDispensingSyringePackin = new CountTypeLife();
            /// <summary>
            /// 体外廃液ポンプ
            /// </summary>
            public TimeTypeLife OutDrainPump = new TimeTypeLife();
            /// <summary>
            /// 体外廃液ポンプチューブ
            /// </summary>
            public TimeTypeLife OutDrainPumpTube = new TimeTypeLife();

            /// <summary>
            /// 累計分析数
            /// </summary>
            public Int32 TotalAssay { get; set; }

            /// <summary>
            /// 消耗品リストの取得
            /// </summary>
            public List<RemainingLife> SupplieList
            {
                get
                {
                    List<RemainingLife> supplieList = new List<RemainingLife>()
                    {
                        this.SampleDispensingSyringePackin      ,
                        this.R1DispensingSyringePackin          ,
                        this.R2DispensingSyringePackin          ,
                        this.ReagentDispensingSyringePackin     ,
                        this.DiluentDispensingSyringePackin     ,
                        this.Wash1DispensingSyringePackin       ,
                        this.Wash2DispensingSyringePackin       ,
                        this.PreTriggerDispensingSyringePackin  ,
                        this.TriggerDispensingSyringePackin     ,
                        this.OutDrainPump                       ,
                        this.OutDrainPumpTube
                    };
                    return supplieList;
                }
            }
            /// <summary>
            /// オーバーリストの取得
            /// </summary>
            public List<RemainingLife> OverList
            {
                get
                {
                    IEnumerable<RemainingLife> searched = from v in this.SupplieList
                                                          where v.IsOver
                                                          select v;
                    return searched.ToList();
                }
            }
        }
    }

    /// <summary>
    /// 残りライフクラス
    /// </summary>
    abstract public class RemainingLife
    {
        /// <summary>
        /// 開始日時
        /// </summary>
        public DateTime StartTime = DateTime.MinValue;
        /// <summary>
        /// 経過状態の取得
        /// </summary>
        abstract public Boolean IsOver
        {
            get;
        }
    }
    /// <summary>
    /// 回数種別ライフクラス
    /// </summary>
    public class CountTypeLife : RemainingLife
    {
        /// <summary>
        /// 使用回数
        /// </summary>
        public Int32 UseCount = 0;
        /// <summary>
        /// 残り回数
        /// </summary>
        public Int32 RemainCount = 0;

        /// <summary>
        /// 経過状態の取得
        /// </summary>
        public override bool IsOver
        {
            get
            {
                return UseCount >= RemainCount;
            }
        }
    }
    /// <summary>
    /// 日時種別ライフクラス
    /// </summary>
    public class TimeTypeLife : RemainingLife
    {
        /// <summary>
        /// 使用日時
        /// </summary>
        public SerializableTimeSpan UseTime = TimeSpan.FromSeconds(0);
        /// <summary>
        /// 残り日時
        /// </summary>
        public SerializableTimeSpan RemainTime = TimeSpan.FromSeconds(0);
        /// <summary>
        /// 経過状態の取得
        /// </summary>
        public override bool IsOver
        {
            get
            {
                return UseTime >= RemainTime;
            }
        }
    }

}
