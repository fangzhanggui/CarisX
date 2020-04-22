using Oelco.CarisX.Comm;
using Oelco.CarisX.Const;
using Oelco.Common.Parameter;
using System;
using System.Collections.Generic;

namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// モーターパラメータ
    /// </summary>
    /// <remarks>
    /// パラメータを追加する際には、初期値を設定しておく。
    /// ※インストーラを配信した時にパラメータを上書きしない為、
    /// 　初期値を設定しておかないとパラメータが追加されない
    /// 
    /// 初期値はオフセットがすべて０、ゲインがすべて１で設定しておく。
    /// 追加後は現地でオフセット値の調整を行う
    /// </remarks>
    public class CarisXMotorParameter : ISavePath
    {

        #region ISavePath メンバー
        /// <summary>
        /// 保存パス
        /// </summary>
        public String SavePath
        {
            get
            {
                return CarisXConst.PathParam + @"\MotorParameter.xml";
            }
        }

        /// <summary>
        /// バックアップ保存パス
        /// </summary>
        public String BackupSavePath
        {
            get
            {
                return CarisXConst.PathBackupParam + @"\MotorParameter.xml";
            }
        }

        #endregion

        public List<RackConfig> RackList;
        public List<ModuleConfig> SlaveList;

        public class RackConfig
        {
            /// <summary>
            /// シリアルＮＯ
            /// </summary>
            public String SerialNo { get; set; }

            /// <summary>
            /// モジュールＩＤ
            /// </summary>
            public Int32 ModuleId { get; set; }

            //ラック搬送ユニット
            /// <summary>
            /// ラック搬送部送りX軸（モジュール１）
            /// </summary>
            public RackTransferSendingXAxisParam rackTransferSendingXAxisM1Param;
            /// <summary>
            /// ラック搬送部戻りX軸（モジュール１）
            /// </summary>
            public RackTransferBackXAxisParam rackTransferBackXAxisM1Param;
            /// <summary>
            /// ラック引込部Y軸（モジュール１）
            /// </summary>
            public RackForkYAxisParam rackForkYAxisM1Param;
            /// <summary>
            /// ラック搬送部送りX軸（モジュール２）
            /// </summary>
            public RackTransferSendingXAxisParam rackTransferSendingXAxisM2Param;
            /// <summary>
            /// ラック搬送部戻りX軸（モジュール２）
            /// </summary>
            public RackTransferBackXAxisParam rackTransferBackXAxisM2Param;
            /// <summary>
            /// ラック引込部Y軸（モジュール２）
            /// </summary>
            public RackForkYAxisParam rackForkYAxisM2Param;
            /// <summary>
            /// ラック搬送部送りX軸（モジュール３）
            /// </summary>
            public RackTransferSendingXAxisParam rackTransferSendingXAxisM3Param;
            /// <summary>
            /// ラック搬送部戻りX軸（モジュール３）
            /// </summary>
            public RackTransferBackXAxisParam rackTransferBackXAxisM3Param;
            /// <summary>
            /// ラック引込部Y軸（モジュール３）
            /// </summary>
            public RackForkYAxisParam rackForkYAxisM3Param;
            /// <summary>
            /// ラック搬送部送りX軸（モジュール４）
            /// </summary>
            public RackTransferSendingXAxisParam rackTransferSendingXAxisM4Param;
            /// <summary>
            /// ラック搬送部戻りX軸（モジュール４）
            /// </summary>
            public RackTransferBackXAxisParam rackTransferBackXAxisM4Param;
            /// <summary>
            /// ラック引込部Y軸（モジュール４）
            /// </summary>
            public RackForkYAxisParam rackForkYAxisM4Param;
            /// <summary>
            /// ラック架設部　ラック設置Y軸
            /// </summary>
            public RackSetLoadYAxisParam rackSetLoadYAxisParam;
            /// <summary>
            /// ラック架設部　再検ラック待機Y軸
            /// </summary>
            public RackSetUnLoadYAxisParam rackSetUnLoadYAxisParam;
            /// <summary>
            /// ラック架設部　ラック回収Y軸
            /// </summary>
            public RackSetTakeOutYAxisParam rackSetTakeOutYAxisParam;
            /// <summary>
            /// ラック架設部　ラックフィーダX軸
            /// </summary>
            public RackSetLoadFeederXAxisParam rackSetLoadFeederXAxisParam;
            /// <summary>
            /// ラック架設部　再検ラックフィーダX軸
            /// </summary>
            public RackSetUnLoadFeederXAxisParam rackSetUnLoadFeederXAxisParam;
            /// <summary>
            /// ラック架設部　ラックスライダーX軸
            /// </summary>
            public RackSetSliderXAxisParam rackSetSliderXAxisParam;

        }

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

            //ケース搬送ユニット
            /// <summary>
            /// ケース搬送部Y軸
            /// </summary>
            public CaseTransferYAxisParam caseTransferYAxisParam;
            /// <summary>
            /// ケース搬送部Z軸
            /// </summary>
            public CaseTransferZAxisParam caseTransferZAxisParam;

            //試薬保冷庫ユニット
            /// <summary>
            /// 試薬保冷庫部テーブルθ軸
            /// </summary>
            public ReagentStorageTableThetaAxisParam reagentStorageTableThetaAxisParam;
            /// <summary>
            /// 試薬保冷庫撹拌θ軸
            /// </summary>
            public ReagentStorageMixingThetaAxisParam reagentStorageMixingThetaAxisParam;

            //スタット部
            /// <summary>
            /// スタット部Y軸
            /// </summary>
            public STATYAxisParam sTATYAxisParam;

            //サンプル分注部
            /// <summary>
            /// サンプル分注移送部Y軸
            /// </summary>
            public SampleDispenseArmYAxisParam sampleDispenseArmYAxisParam;
            /// <summary>
            /// サンプル分注移送部Z軸
            /// </summary>
            public SampleDispenseArmZAxisParam sampleDispenseArmZAxisParam;
            /// <summary>
            /// サンプル分注移送部θ軸
            /// </summary>
            public SampleDispenseArmThetaAxisParam sampleDispenseArmThetaAxisParam;
            /// <summary>
            /// サンプル分注移送部ｻﾝﾌﾟﾙｼﾘﾝｼﾞ
            /// </summary>
            public SampleDispenseSyringeParam sampleDispenseSyringeParam;

            //反応容器搬送部
            /// <summary>
            /// 反応容器搬送部X軸
            /// </summary>
            public ReactionCellTransferXAxisParam reactionCellTransferXAxisParam;
            /// <summary>
            /// 反応容器搬送部Z軸
            /// </summary>
            public ReactionCellTransferZAxisParam reactionCellTransferZAxisParam;

            //反応テーブル部
            /// <summary>
            /// 反応テーブル部θ軸
            /// </summary>
            public ReactionTableThetaAxisParam reactionTableThetaAxisParam;
            /// <summary>
            /// 撹拌部　R1撹拌Zθ
            /// </summary>
            public ReactionTableR1MixingZThetaAxisParam reactionTableR1MixingZThetaAxisParam;

            //BFテーブル部
            /// <summary>
            /// BFテーブル部θ軸
            /// </summary>
            public BFTableThetaAxisParam bFTableThetaAxisParam;
            /// <summary>
            /// 撹拌部　R2撹拌Zθ
            /// </summary>
            public BFTableR2MixingZThetaAxisParam bFTableR2MixingZThetaAxisParam;
            /// <summary>
            /// 撹拌部　BF1撹拌Zθ
            /// </summary>
            public BFTableBF1MixingZThetaAxisParam bFTableBF1MixingZThetaAxisParam;
            /// <summary>
            /// 撹拌部　BF2撹拌Zθ
            /// </summary>
            public BFTableBF2MixingZThetaAxisParam bFTableBF2MixingZThetaAxisParam;
            /// <summary>
            /// 撹拌部　ｐTr撹拌Zθ
            /// </summary>
            public BFTablePreTriggerMixingZThetaAxisParam bFTablePreTriggerMixingZThetaAxisParam;

            //トラベラー・廃棄部
            /// <summary>
            /// トラベラー・廃棄部X軸
            /// </summary>
            public TravelerXAxisParam travelerXAxisParam;
            /// <summary>
            /// トラベラー・廃棄部Z軸
            /// </summary>
            public TravelerZAxisParam travelerZAxisParam;

            //試薬分注1部
            /// <summary>
            /// 試薬分注1部θ軸
            /// </summary>
            public R1DispenseArmThetaAxisParam r1DispenseArmThetaAxisParam;
            /// <summary>
            /// 試薬分注1部Z軸
            /// </summary>
            public R1DispenseArmZAxisParam r1DispenseArmZAxisParam;
            /// <summary>
            /// 試薬分注1ｼﾘﾝｼﾞ
            /// </summary>
            public R1DispenseSyringeParam r1DispenseSyringeParam;

            //試薬分注2部
            /// <summary>
            /// 試薬分注2部θ軸
            /// </summary>
            public R2DispenseArmThetaAxisParam r2DispenseArmThetaAxisParam;
            /// <summary>
            /// 試薬分注2部Z軸
            /// </summary>
            public R2DispenseArmZAxisParam r2DispenseArmZAxisParam;
            /// <summary>
            /// 試薬分注2ｼﾘﾝｼﾞ
            /// </summary>
            public R2DispenseSyringeParam r2DispenseSyringeParam;

            //BF1部、BF1廃液部
            /// <summary>
            /// BF1部Z軸
            /// </summary>
            public BF1NozzleZAxisParam bF1NozzleZAxisParam;
            /// <summary>
            /// BF1廃液部Z軸
            /// </summary>
            public BF1WasteNozzleZAxisParam bF1WasteNozzleZAxisParam;
            /// <summary>
            /// 洗浄液ｼﾘﾝｼﾞ
            /// </summary>
            public BFWashSyringeParam bFWashSyringeParam;

            //BF2部
            /// <summary>
            /// BF2部Z軸
            /// </summary>
            public BF2NozzleZAxisParam bF2NozzleZAxisParam;

            //希釈液分注部
            /// <summary>
            /// 希釈液分注部Z軸
            /// </summary>
            public DiluentDispenseArmZAxisParam diluentDispenseArmZAxisParam;
            /// <summary>
            /// 希釈液ｼﾘﾝｼﾞ
            /// </summary>
            public DiluentDispenseSyringeParam diluentDispenseSyringeParam;

            //トリガ分注部
            /// <summary>
            /// プレトリガ・トリガノズルZ軸
            /// </summary>
            public TriggerAndPreTriggerDispenseNozzleZAxisParam triggerAndPreTriggerDispenseNozzleZAxisParam;
            /// <summary>
            /// トリガ液ｼﾘﾝｼﾞ
            /// </summary>
            public TriggerDispenseSyringeParam triggerDispenseSyringeParam;

            //プレトリガ分注部
            /// <summary>
            /// プレトリガ液ｼﾘﾝｼﾞ
            /// </summary>
            public PreTriggerDispenseSyringeParam preTriggerDispenseSyringeParam;

            //装置補正係数
            /// <summary>
            /// 装置補正係数
            /// </summary>
            public InstrumentCoef instrumentCoef;
        }

        #region [ラック搬送]
        /// <summary>
        /// ラック搬送部送りX軸
        /// </summary>
        public class RackTransferSendingXAxisParam : ItemSetMotorParam { }

        /// <summary>
        /// ラック搬送部戻りX軸
        /// </summary>
        public class RackTransferBackXAxisParam : ItemSetMotorParam { }

        /// <summary>
        /// ラック引込部Y軸
        /// </summary>
        public class RackForkYAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// ラック受取位置オフセット
            /// </summary>
            public double OffsetHomePosition = 0;
        }

        /// <summary>
        /// ラック架設部　ラック設置Y軸
        /// </summary>
        public class RackSetLoadYAxisParam : ItemSetMotorParam { }

        /// <summary>
        /// ラック架設部　再検ラック待機Y軸
        /// </summary>
        public class RackSetUnLoadYAxisParam : ItemSetMotorParam { }

        /// <summary>
        /// ラック架設部　ラック回収Y軸
        /// </summary>
        public class RackSetTakeOutYAxisParam : ItemSetMotorParam { }

        /// <summary>
        /// ラック架設部　ラックフィーダX軸
        /// </summary>
        public class RackSetLoadFeederXAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// ラック回収フィーダ位置オフセット
            /// </summary>
            public double OffsetRackTakeout = 0;
            /// <summary>
            /// ラック設置フィーダー位置オフセット
            /// </summary>
            public double OffsetRackLoad = 0;
            /// <summary>
            /// 容器有無確認位置オフセット
            /// </summary>
            public double OffsetTubeSensorReading = 0;
            /// <summary>
            /// 検体BC-ID読取位置オフセット
            /// </summary>
            public double OffsetSampleIDReading = 0;
            /// <summary>
            /// ラックBC-ID読取位置オフセット
            /// </summary>
            public double OffsetRackIDReading = 0;
        }

        /// <summary>
        /// ラック架設部　再検ラックフィーダX軸
        /// </summary>
        public class RackSetUnLoadFeederXAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// ラック回収フィーダ位置オフセット
            /// </summary>
            public double OffsetRackTakeout = 0;
            /// <summary>
            /// ラック再検フィーダ位置オフセット
            /// </summary>
            public double OffsetRackRetest = 0;
            /// <summary>
            /// ラック待機フィーダ位置オフセット
            /// </summary>
            public double OffsetRackUnLoad = 0;
        }

        /// <summary>
        /// ラック架設部　ラックスライダーX軸
        /// </summary>
        public class RackSetSliderXAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// ラック受け渡しスライダー位置
            /// </summary>
            public double OffsetRackLoad = 0;
            /// <summary>
            /// ラックロードスライダー位置
            /// </summary>
            public double OffsetRackUnLoad = 0;
        }

        #endregion

        #region [ケース搬送部]

        /// <summary>
        /// ケース搬送部X軸
        /// </summary>
        public class CaseTransferYAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// ケースキャッチ/返却
            /// </summary>
            public double OffsetCaseCatchRelease = 0;
            /// <summary>
            /// 反応容器キャッチ位置
            /// </summary>
            public double OffsetReactionCellCatch = 0;
            /// <summary>
            /// チップキャッチ位置
            /// </summary>
            public double OffsetSamplingTipCatch = 0;
        }

        /// <summary>
        /// ケース搬送部Z軸
        /// </summary>
        public class CaseTransferZAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// ケースキャッチ/返却
            /// </summary>
            public double OffsetCaseCatchRelease = 0;
            /// <summary>
            /// 反応容器/チップキャッチ位置
            /// </summary>
            public double OffsetReactionCellSamplingTipCatch = 0;
        }
        
        #endregion
        
        #region [試薬保冷庫]
        
        /// <summary>
        /// 試薬保冷庫テーブルθ軸
        /// </summary>
        public class ReagentStorageTableThetaAxisParam : ItemSetMotorParam
        {

            /// <summary>
            /// M試薬ボトルBCR読取位置
            /// </summary>
            public double OffsetMReagentIDReading = 0;
            /// <summary>
            /// R試薬ボトルBCR読取位置
            /// </summary>
            public double OffsetRReagentIDReading = 0;
            /// <summary>
            /// R1ユニット R1吸引位置
            /// </summary>
            public double OffsetR1UnitR1Aspiration = 0;
            /// <summary>
            /// R1ユニット R2吸引位置
            /// </summary>
            public double OffsetR1UnitR2Aspiration = 0;
            /// <summary>
            /// R試薬ボトルBCR読取位置
            /// </summary>
            public double OffsetR1UnitMReagentAspiration = 0;
            /// <summary>
            /// M/R Bottle Check位置
            /// </summary>
            public double OffsetMRBottleCheck = 0;
            /// <summary>
            /// R2ユニット M吸引位置
            /// </summary>
            public double OffsetR2UnitMReagentAspiration = 0;
            /// <summary>
            /// R2ユニット R2吸引位置
            /// </summary>
            public double OffsetR2UnitR2Aspiration = 0;
            /// <summary>
            /// エンコード許容量オフセット
            /// </summary>
            public double OffsetEncodeThresh = 0;
        }
        /// <summary>
        /// 試薬保冷庫撹拌θ軸
        /// </summary>
        public class ReagentStorageMixingThetaAxisParam : ItemSetMotorParam{ }

        #endregion

        #region [スタット部]
        /// <summary>
        /// STATユニット
        /// </summary>
        public class STATYAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// STATサンプル分取位置
            /// </summary>
            public double OffsetSTATSampleAspiration = 0;
        }
        #endregion

        #region [サンプル分注部]
        /// <summary>
        /// サンプル分注移送部Y軸
        /// </summary>
        public class SampleDispenseArmYAxisParam : ItemSetMotorParam 
        {
            /// <summary>
            /// ラックサンプル1分取位置
            /// </summary>
            public double OffsetRackSample1Aspiration = 0;
            /// <summary>
            /// ラックサンプル5分取位置
            /// </summary>
            public double OffsetRackSample5Aspiration = 0;
            /// <summary>
            /// STATサンプル分取位置
            /// </summary>
            public double OffsetSTATSampleAspiration = 0;
            /// <summary>
            /// 希釈検体分取位置
            /// </summary>
            public double OffsetDiluentSampleAspiration = 0;
            /// <summary>
            /// 前処理検体分取位置
            /// </summary>
            public double OffsetPretreatSampleAspiration = 0;
            /// <summary>
            /// 外部検体分取位置
            /// </summary>
            public double OffsetLineSampleAspiration = 0;
            /// <summary>
            /// 検体吐出位置
            /// </summary>
            public double OffsetSampleDispense = 0;
            /// <summary>
            /// チップリムーブ位置
            /// </summary>
            public double OffsetSampleTipRemover = 0;
            /// <summary>
            /// チップキャッチ1位置
            /// </summary>
            public double OffsetSampleTip1Catch = 0;
            /// <summary>
            /// チップキャッチ6位置
            /// </summary>
            public double OffsetSampleTip6Catch = 0;
        }

        /// <summary>
        /// サンプル分注移送部Z軸
        /// </summary>
        public class SampleDispenseArmZAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// ラックサンプル分取位置
            /// </summary>
            public double OffsetRackSampleAspiration = 0;
            /// <summary>
            /// STATサンプル分取位置
            /// </summary>
            public double OffsetSTATSampleAspiration = 0;
            /// <summary>
            /// 外部検体分取位置
            /// </summary>
            public double OffsetLineSampleAspiration = 0;
            /// <summary>
            /// 検体吐出位置/希釈吸引開始/前処理吸引開始
            /// </summary>
            public double OffsetSampleDispenseDilutePretreatAspiration = 0;
            /// <summary>
            /// チップリムーブ位置
            /// </summary>
            public double OffsetSampleTipRemover = 0;
            /// <summary>
            /// チップキャッチ位置
            /// </summary>
            public double OffsetSampleTipCatch = 0;
        }

        /// <summary>
        /// サンプル分注移送部θ軸
        /// </summary>
        public class SampleDispenseArmThetaAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// ラックサンプル1分取位置
            /// </summary>
            public double OffsetRackSample1Aspiration = 0;
            /// <summary>
            /// ラックサンプル5分取位置
            /// </summary>
            public double OffsetRackSample5Aspiration = 0;
            /// <summary>
            /// STATサンプル分取位置
            /// </summary>
            public double OffsetSTATSampleAspiration = 0;
            /// <summary>
            /// 希釈検体分取位置
            /// </summary>
            public double OffsetDiluentSampleAspiration = 0;
            /// <summary>
            /// 前処理検体分取位置
            /// </summary>
            public double OffsetPretreatSampleAspiration = 0;
            /// <summary>
            /// 外部検体分取位置
            /// </summary>
            public double OffsetLineSampleAspiration = 0;
            /// <summary>
            /// 検体吐出位置
            /// </summary>
            public double OffsetSampleDispense = 0;
            /// <summary>
            /// チップリムーブ位置
            /// </summary>
            public double OffsetSampleTipRemover = 0;
            /// <summary>
            /// チップキャッチ1位置
            /// </summary>
            public double OffsetSampleTip1Catch = 0;
            /// <summary>
            /// チップキャッチ6位置
            /// </summary>
            public double OffsetSampleTip6Catch = 0;
        }

        /// <summary>
        /// サンプル分注移送部ｻﾝﾌﾟﾙｼﾘﾝｼﾞ
        /// </summary>
        public class SampleDispenseSyringeParam : ItemSetMotorParam
        {
            /// <summary>
            /// ゲイン
            /// </summary>
            public double Gain = 1;
            /// <summary>
            /// オフセット
            /// </summary>
            public double Offset = 0;
            /// <summary>
            /// ゲイン（＞100uL）
            /// </summary>
            public double GainOver100 = 1;
            /// <summary>
            /// オフセット（＞100uL）
            /// </summary>
            public double OffsetOver100 = 0;
        }

        #endregion

        #region [反応容器搬送部]

        /// <summary>
        /// 反応容器搬送部X軸
        /// </summary>
        public class ReactionCellTransferXAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// 反応容器キャッチ位置オフセット
            /// </summary>
            public double OffsetReactionCellCatch = 0;
            /// <summary>
            /// 反応容器リリース位置オフセット
            /// </summary>
            public double OffsetReactionCellRelease = 0;
        }

        /// <summary>
        /// 反応容器搬送部Z軸
        /// </summary>
        public class ReactionCellTransferZAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// 反応容器キャッチ位置オフセット
            /// </summary>
            public double OffsetReactionCellCatch = 0;
            /// <summary>
            /// 容器リムーブ位置オフセット
            /// </summary>
            public double OffsetReactionCellRelease = 0;
        }

        #endregion

        #region [反応テーブル部]

        /// <summary>
        /// 反応テーブル部θ軸
        /// </summary>
        public class ReactionTableThetaAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// 原点オフセット
            /// </summary>
            public double OffsetHomePosition = 0;
            /// <summary>
            /// エンコード許容量オフセット
            /// </summary>
            public double OffsetEncodeThresh = 0;
        }

        /// <summary>
        /// 撹拌部　R1撹拌Zθ
        /// </summary>
        public class ReactionTableR1MixingZThetaAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// A Posオフセット
            /// </summary>
            public double OffsetAPos = 0;
        }

        #endregion

        #region [BFテーブル部]

        /// <summary>
        /// BFテーブル部θ軸
        /// </summary>
        public class BFTableThetaAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// 原点オフセット
            /// </summary>
            public double OffsetHomePosition = 0;
            /// <summary>
            /// エンコード許容量オフセット
            /// </summary>
            public double OffsetEncodeThresh = 0;
        }

        /// <summary>
        /// 撹拌部　R2撹拌Zθ
        /// </summary>
        public class BFTableR2MixingZThetaAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// A Posオフセット
            /// </summary>
            public double OffsetAPos = 0;
        }

        /// <summary>
        /// 撹拌部　BF1撹拌Zθ
        /// </summary>
        public class BFTableBF1MixingZThetaAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// A Posオフセット
            /// </summary>
            public double OffsetAPos = 0;
        }

        /// <summary>
        /// 撹拌部　BF2撹拌Zθ
        /// </summary>
        public class BFTableBF2MixingZThetaAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// A Posオフセット
            /// </summary>
            public double OffsetAPos = 0;
        }

        /// <summary>
        /// 撹拌部　ｐTr撹拌Zθ
        /// </summary>
        public class BFTablePreTriggerMixingZThetaAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// A Posオフセット
            /// </summary>
            public double OffsetAPos = 0;
        }

        #endregion

        #region [トラベラー・廃棄部]

        /// <summary>
        /// トラベラー・廃棄部X軸
        /// </summary>
        public class TravelerXAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// 反応テーブル（内周）設置位置オフセット
            /// </summary>
            public double OffsetReactionTableInside = 0;
            /// <summary>
            /// 反応テーブル（外周）設置位置オフセット
            /// </summary>
            public double OffsetReactionTableOutside = 0;
            /// <summary>
            /// BFテーブル（内周）設置位置オフセット
            /// </summary>
            public double OffsetBFTableInside = 0;
            /// <summary>
            /// BFテーブル（外周）設置位置オフセット
            /// </summary>
            public double OffsetBFTableOutside = 0;
            /// <summary>
            /// 容器廃棄位置オフセット
            /// </summary>
            public double OffsetReactionCellRemover = 0;
        }

        /// <summary>
        /// トラベラー・廃棄部Z軸
        /// </summary>
        public class TravelerZAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// 反応テーブル（内周）設置位置オフセット
            /// </summary>
            public double OffsetReactionTableInside = 0;
            /// <summary>
            /// 反応テーブル（外周）設置位置オフセット
            /// </summary>
            public double OffsetReactionTableOutside = 0;
            /// <summary>
            /// BFテーブル（内周）設置位置オフセット
            /// </summary>
            public double OffsetBFTableInside = 0;
            /// <summary>
            /// BFテーブル（外周）設置位置オフセット
            /// </summary>
            public double OffsetBFTableOutside = 0;
            /// <summary>
            /// 容器廃棄位置オフセット
            /// </summary>
            public double OffsetReactionCellRemover = 0;
        }

        #endregion

        #region [試薬分注1部]

        /// <summary>
        /// 試薬分注1部θ軸
        /// </summary>
        public class R1DispenseArmThetaAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// R1吸引位置オフセット
            /// </summary>
            public double OffsetR1Aspiration = 0;
            /// <summary>
            /// R2吸引位置オフセット
            /// </summary>
            public double OffsetR2Aspiration = 0;
            /// <summary>
            /// M試薬吸引位置オフセット
            /// </summary>
            public double OffsetMReagentAspiration = 0;
            /// <summary>
            /// 洗浄層位置オフセット
            /// </summary>
            public double OffsetCuvette = 0;
            /// <summary>
            /// 反応テーブル吐出位置オフセット
            /// </summary>
            public double OffsetReactionCellDispense = 0;
            /// <summary>
            /// エンコード許容量オフセット
            /// </summary>
            public double OffsetEncodeThresh = 0;
        }

        /// <summary>
        /// 試薬分注1部Z軸
        /// </summary>
        public class R1DispenseArmZAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// R1/R2試薬吸引位置オフセット
            /// </summary>
            public double OffsetR1R2Aspiration = 0;
            /// <summary>
            /// M試薬吸引位置オフセット
            /// </summary>
            public double OffsetMReagentAspiration = 0;
            /// <summary>
            /// 洗浄層位置オフセット
            /// </summary>
            public double OffsetCuvette = 0;
            /// <summary>
            /// 反応テーブル吐出位置オフセット
            /// </summary>
            public double OffsetReactionCellDispense = 0;
            /// <summary>
            /// プローブ交換調整位置オフセット
            /// </summary>
            public double OffsetPositioningProbe = 0;
        }

        /// <summary>
        /// 試薬分注1ｼﾘﾝｼﾞ
        /// </summary>
        public class R1DispenseSyringeParam : ItemSetMotorParam
        {
            /// <summary>
            /// ゲイン
            /// </summary>
            public double Gain = 1;
            /// <summary>
            /// オフセット
            /// </summary>
            public double Offset = 0;
        }

        #endregion

        #region [試薬分注2部]

        /// <summary>
        /// 試薬分注2部θ軸
        /// </summary>
        public class R2DispenseArmThetaAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// R2試薬吸引位置オフセット
            /// </summary>
            public double OffsetR2Aspiration = 0;
            /// <summary>
            /// M試薬吸引位置オフセット
            /// </summary>
            public double OffsetMReagentAspiration = 0;
            /// <summary>
            /// 洗浄層位置オフセット
            /// </summary>
            public double OffsetCuvette = 0;
            /// <summary>
            /// 反応テーブル吐出位置オフセット
            /// </summary>
            public double OffsetReactionCellDispense = 0;
            /// <summary>
            /// エンコード許容量オフセット
            /// </summary>
            public double OffsetEncodeThresh = 0;
        }

        /// <summary>
        /// 試薬分注2部Z軸
        /// </summary>
        public class R2DispenseArmZAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// R2試薬吸引位置オフセット
            /// </summary>
            public double OffsetR2Aspiration = 0;
            /// <summary>
            /// M試薬吸引位置オフセット
            /// </summary>
            public double OffsetMReagentAspiration = 0;
            /// <summary>
            /// 洗浄層位置オフセット
            /// </summary>
            public double OffsetCuvette = 0;
            /// <summary>
            /// 反応テーブル吐出位置オフセット
            /// </summary>
            public double OffsetReactionCellDispense = 0;
            /// <summary>
            /// プローブ交換調整位置オフセット
            /// </summary>
            public double OffsetPositioningProbe = 0;
        }

        /// <summary>
        /// 試薬分注2ｼﾘﾝｼﾞ
        /// </summary>
        public class R2DispenseSyringeParam : ItemSetMotorParam
        {
            /// <summary>
            /// ゲイン
            /// </summary>
            public double Gain = 1;
            /// <summary>
            /// オフセット
            /// </summary>
            public double Offset = 0;
        }

        #endregion

        #region [BF1部、BF1廃液部]
        /// <summary>
        /// BF1部Z軸
        /// </summary>
        public class BF1NozzleZAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// 反応テーブル位置オフセット
            /// </summary>
            public double OffsetReactionCell = 0;
            /// <summary>
            /// 洗浄層位置オフセット
            /// </summary>
            public double OffsetCuvette = 0;
        }

        /// <summary>
        /// BF1廃液部Z軸
        /// </summary>
        public class BF1WasteNozzleZAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// 反応テーブル位置オフセット
            /// </summary>
            public double OffsetReactionCell = 0;
            /// <summary>
            /// 洗浄層位置オフセット
            /// </summary>
            public double OffsetCuvette = 0;
        }

        /// <summary>
        /// 洗浄液ｼﾘﾝｼﾞ
        /// </summary>
        public class BFWashSyringeParam : ItemSetMotorParam
        {
            /// <summary>
            /// ゲイン
            /// </summary>
            public double Gain = 1;
            /// <summary>
            /// オフセット
            /// </summary>
            public double Offset = 0;
        }
        #endregion

        #region [BF2部]
        /// <summary>
        /// BF2部Z軸
        /// </summary>
        public class BF2NozzleZAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// 反応テーブル位置オフセット
            /// </summary>
            public double OffsetReactionCell = 0;
            /// <summary>
            /// 洗浄層位置オフセット
            /// </summary>
            public double OffsetCuvette = 0;
        }
        #endregion

        #region [希釈液分注部]

        /// <summary>
        /// 希釈液分注部Z軸
        /// </summary>
        public class DiluentDispenseArmZAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// 反応テーブル位置オフセット
            /// </summary>
            public double OffsetReactionCell = 0;
            /// <summary>
            /// 洗浄層位置オフセット
            /// </summary>
            public double OffsetCuvette = 0;
        }

        /// <summary>
        /// 希釈液ｼﾘﾝｼﾞ
        /// </summary>
        public class DiluentDispenseSyringeParam : ItemSetMotorParam
        {
            /// <summary>
            /// ゲイン
            /// </summary>
            public double Gain = 1;
            /// <summary>
            /// オフセット
            /// </summary>
            public double Offset = 0;
        }

        #endregion

        #region [トリガ分注部]     
        /// <summary>
        /// プレトリガ・トリガノズルZ軸
        /// </summary>
        public class TriggerAndPreTriggerDispenseNozzleZAxisParam : ItemSetMotorParam
        {
            /// <summary>
            /// 反応テーブル位置オフセット
            /// </summary>
            public double OffsetReactionCell = 0;
            /// <summary>
            /// 洗浄層位置オフセット
            /// </summary>
            public double OffsetCuvette = 0;
        }

        /// <summary>
        /// トリガ液ｼﾘﾝｼﾞ
        /// </summary>
        public class TriggerDispenseSyringeParam : ItemSetMotorParam
        {
            /// <summary>
            /// ゲイン
            /// </summary>
            public double Gain = 1;
            /// <summary>
            /// オフセット
            /// </summary>
            public double Offset = 0;
        }
        #endregion

        #region [プレトリガ分注部]     
        /// <summary>
        /// プレトリガ液ｼﾘﾝｼﾞ
        /// </summary>
        public class PreTriggerDispenseSyringeParam : ItemSetMotorParam
        {
            /// <summary>
            /// ゲイン
            /// </summary>
            public double Gain = 1;
            /// <summary>
            /// オフセット
            /// </summary>
            public double Offset = 0;
        }

        #endregion

        #region [装置補正係数]
        /// <summary>
        /// 装置補正係数
        /// </summary>
        public class InstrumentCoef
        {
            /// <summary>
            /// 装置補正係数
            /// </summary>
            public double InstrumentCoefficient = 0;
        }
        #endregion

        #region [モーターパラメータ]
        /// <summary>
        /// モーターパラメータ設定パラメータ
        /// </summary>
        public class ItemSetMotorParam
        {
            /// <summary>
            /// コマンド番号(受信時のみ使用)
            /// </summary>
            public Int32 CmdNo;
            /// <summary>
            /// モーター番号
            /// </summary>
            public Int32 MotorNo;
            /// <summary>
            /// モータースピード
            /// </summary>
            public ItemMParam[] MotorSpeed;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ItemSetMotorParam()
            {
                MotorSpeed = new ItemMParam[6];
                for (Int32 i = 0; i < MotorSpeed.Length; i++)
                    MotorSpeed[i] = new ItemMParam();
            }
        }
        #endregion

    }
}
