using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.CarisX.Parameter;
using Oelco.Common.Parameter;
using Oelco.Common.Utility;
using Oelco.CarisX.DB;
using Oelco.CarisX.Const;
using Oelco.CarisX.Comm;
using Oelco.Common.DB;
using Oelco.CarisX.Common;
using Oelco.CarisX.Log;
using Oelco.Common.Log;

namespace Oelco.CarisX.Utility
{
    /// <summary>
    /// 混成データ中継クラス
    /// </summary>
    /// <remarks>
    /// 関連性の弱いクラス間に於いてのデータ・機能を参照する際に仲介を行います。
    /// </remarks>
    static public class HybridDataMediator
    {
        /// <summary>
        /// 試薬テーブル残量取得
        /// </summary>
        /// <remarks>
        /// 試薬DBから、各種試薬の残量を纏めたテーブルを返します。
        /// </remarks>
        /// <returns>分量テーブル</returns>
        static public AmountTable GetReagentDBAmount(Int32 moduleid)
        {
            AmountTable amount = new AmountTable();
            // DBに登録されている内容を取得
            // IRemainAmountInfoSetの実体を臨時で残量コマンドデータクラスで代用している。
            SlaveCommCommand_0508 remainAmount = new SlaveCommCommand_0508();
            IRemainAmountInfoSet remain = remainAmount;
            Singleton<ReagentDB>.Instance.GetReagentRemain(ref remain, moduleid);

            amount.PreTriggerAmountInfo.Amount = remain.PreTriggerRemainTable.RemainingAmount.Sum((v) => v.Remain);
            amount.TriggerAmountInfo.Amount = remain.TriggerRemainTable.RemainingAmount.Sum((v) => v.Remain);
            amount.RinceContainerAmountInfo.Amount = remain.RinceContainerRemain;
            amount.SampleTipAmountInfo.Amount = remain.SampleTipRemainTable.tipRemainTable.Sum();
            amount.DilutionAmountInfo.Amount = remain.DilutionRemainTable.RemainingAmount.Remain;

            // 試薬ロットでもグループを括るようにする
            var reags = from v in remain.ReagentRemainTable
                        where v.ReagCode != 0
                        orderby v.ReagCode
                        group v by new Tuple<Int32, String>(v.ReagCode, v.RemainingAmount.LotNumber);
            amount.WashContainerAmountInfo.Amount = remain.WashContainerRemain;
            foreach (var reag in reags)
            {
                ReagentAmountInfo info = new ReagentAmountInfo();
                info.ReagCode = reag.Key.Item1;
                // この段階では試薬コード・ロットによる制約のみでプロトコルによる分注量が利用できない為、
                // 残量を一つの値に纏めることは出来ない。Amountを設定せずにr1,r2...の試薬を設定する。
                foreach (var detail in reag)
                {
                    switch (detail.ReagTypeDetail)
                    {
                        case ReagentTypeDetail.R1:
                            info.R1Reagent += detail.RemainingAmount.Remain;
                            break;
                        case ReagentTypeDetail.R2:
                            info.R2Reagent += detail.RemainingAmount.Remain;
                            break;
                        case ReagentTypeDetail.T1:
                            info.PreProcess1 += detail.RemainingAmount.Remain;
                            break;
                        case ReagentTypeDetail.T2:
                            info.PreProcess2 += detail.RemainingAmount.Remain;
                            break;
                        case ReagentTypeDetail.M:
                            info.MReagent += detail.RemainingAmount.Remain;
                            break;
                        default:
                            break;
                    }
                }
                info.LotNo = reag.Key.Item2;

                // 試薬ロットを考慮しないようにする
                // 複数の同一試薬コードの試薬が設置されている場合、合算する。
                if (amount.ReagentAmountInfo.ContainsKey(info.ReagCode))
                {
                    var totalAmount = amount.ReagentAmountInfo[info.ReagCode];
                    totalAmount.R1Reagent += info.R1Reagent;
                    totalAmount.R2Reagent += info.R2Reagent;
                    totalAmount.MReagent += info.MReagent;
                    totalAmount.PreProcess1 += info.PreProcess1;
                    totalAmount.PreProcess2 += info.PreProcess2;
                }
                else
                {
                    amount.ReagentAmountInfo.Add(info.ReagCode, info);
                }
            }

            return amount;
        }

        /// <summary>
        /// 全登録済検体必要試薬類分量取得
        /// </summary>
        /// <remarks>
        /// 検体DBに登録されている検体の情報から、必要な試薬分量を算出し、
        /// </remarks>
        /// <returns>分量テーブル</returns>
        static public AmountTable GetAllRegisterdSampleNeedAmount()
        {
            AmountTable amount = new AmountTable();

            // 全ての登録検体で使用される分析項目を取得する。
            var sampleUse = from v in GetSpecimenDBRegisteredProtocolAndRepCountAndAutoDil()
                            orderby v.Item1.ReagentCode
                            group v by v.Item1.ReagentCode;

            // sampleUseのカウントを各項目の多重数分(sample,calib,controlでそれぞれ違う、リストを纏める前にそれぞれ取得しておく。)
            // ??希釈倍率関連により使用量増減ありか
            // 取得した情報を元に、各試薬の必要量を設定する。

            // 参照用のシステムパラメータ
            var systemParameter = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param;

            // 分析全体で合計する項目
            Int32 tipRequireSum = 0;
            Int32 dilRequireSum = 0;
            Int32 triggerRequireSum = 0;
            Int32 preTriggerRequireSum = 0;
            Int32 preProcess1RequireSum = 0; // 前処理液1
            Int32 preProcess2RequireSum = 0; // 前処理液2

            // 各分析項目全てカウント
            // 各登録検体分の内容をカウントする
            foreach (var protocolInfo in sampleUse)
            {
                // 試薬コードでグループ化している為、試薬に関する情報は先頭データから取得する。(分析項目に関するデータは取得しない）
                var firstElement = protocolInfo.First();

                // 試薬単位で合計する項目
                Int32 r1RequireSum = 0;
                Int32 r2RequireSum = 0;
                Int32 mRequireSum = 0;

                // 試薬ロットを考慮しないようにする
                ReagentAmountInfoWithProtocol reagAmountInf = null;
                if (amount.ReagentAmountInfo.ContainsKey(firstElement.Item1.ReagentCode))
                {
                    reagAmountInf = amount.ReagentAmountInfo[firstElement.Item1.ReagentCode] as ReagentAmountInfoWithProtocol;
                }
                else
                {
                    reagAmountInf = new ReagentAmountInfoWithProtocol();
                    reagAmountInf.ReagCode = firstElement.Item1.ReagentCode;
                    amount.ReagentAmountInfo.Add(firstElement.Item1.ReagentCode, reagAmountInf);
                }

                // 分析項目毎に、各試薬の使用量に多重測定回数を掛けて総使用量を算出する
                foreach (var protocol in protocolInfo)
                {
                    // 分析項目によって紐付けられた必要テスト回数量
                    Int32 protocolGroupedTestCount = 0;

                    MeasureProtocol measureProtocol = protocol.Item1;
                    Int32 replicationCount = protocol.Item2;
                    Int32 autoDilutionRatio = protocol.Item3;
                    Int32 r1Require = 0;
                    Int32 r2Require = 0;
                    Int32 mRequire = 0;

                    Int32 tipRequire = 0;
                    Int32 dilRequire = 0;
                    Int32 triggerRequire = 0;
                    Int32 preTriggerRequire = 0;
                    Int32 preProcess1Require = 0;
                    Int32 preProcess2Require = 0;

                    // アッセイシーケンス消費量計算
                    switch (measureProtocol.AssaySequence)
                    {
                        case MeasureProtocol.AssaySequenceKind.OneStep:             // 1ステップ法
                        case MeasureProtocol.AssaySequenceKind.TwoStep:             // 2ステップ法
                        case MeasureProtocol.AssaySequenceKind.TwoStepMinus:        // 2ステップ法マイナス
                        case MeasureProtocol.AssaySequenceKind.OnePointFiveStep:    // 1.5ステップ法
                        case MeasureProtocol.AssaySequenceKind.OnePointFiveStepRM:  // 1.5ステップ法（RM）
                            r1Require += measureProtocol.R1DispenseVolume;
                            r2Require += measureProtocol.R2DispenseVolume;
                            mRequire += measureProtocol.MReagDispenseVolume;
                            ++protocolGroupedTestCount;
                            tipRequire += 1;
                            preTriggerRequire += systemParameter.WashDispVolParameter.DispVolPreTrig;
                            triggerRequire += systemParameter.WashDispVolParameter.DispVolTrig;
                            break;
                    }
                    //-----------------------------------------------------

                    //後希釈で使用する希釈液の計算
                    var afterDiluRequire = calcDilution((Int32)measureProtocol.ProtocolDilutionRatio);
                    tipRequire += afterDiluRequire.Item1;
                    dilRequire += afterDiluRequire.Item3;

                    /////////////////////////////////////////////
                    // 前処理消費量計算
                    switch (measureProtocol.PreProcessSequence)
                    {
                        case MeasureProtocol.PreProcessSequenceKind.None:
                            break;
                        case MeasureProtocol.PreProcessSequenceKind.SR1:
                            r1Require += measureProtocol.R1DispenseVolume;
                            r2Require += measureProtocol.R2DispenseVolume;
                            mRequire += measureProtocol.MReagDispenseVolume;
                            ++protocolGroupedTestCount;
                            tipRequire += 1;
                            break;
                        case MeasureProtocol.PreProcessSequenceKind.ST1:
                            preProcess1Require += measureProtocol.PreProsess1DispenseVolume;
                            tipRequire += 1;
                            break;
                        case MeasureProtocol.PreProcessSequenceKind.ST1T2:
                            preProcess1Require += measureProtocol.PreProsess1DispenseVolume;
                            preProcess2Require += measureProtocol.PreProsess2DispenseVolume;
                            tipRequire += 1;
                            break;
                        case MeasureProtocol.PreProcessSequenceKind.ST1SR1:
                            preProcess1Require += measureProtocol.PreProsess1DispenseVolume;
                            r1Require += measureProtocol.R1DispenseVolume;
                            r2Require += measureProtocol.R2DispenseVolume;
                            mRequire += measureProtocol.MReagDispenseVolume;
                            ++protocolGroupedTestCount;
                            tipRequire += 2;
                            break;
                        case MeasureProtocol.PreProcessSequenceKind.ST1ST2:
                            preProcess1Require += measureProtocol.PreProsess1DispenseVolume;
                            preProcess2Require += measureProtocol.PreProsess2DispenseVolume;
                            tipRequire += 2;
                            break;
                        default:
                            // エラーログ
                            Singleton<CarisXLogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("[GetAllRegisterdSampleNeedAmount]Detects a bad pre-processing sequence protocolIndex={0} PreProcessSequence={1}", measureProtocol.ProtocolIndex, (Int32)measureProtocol.PreProcessSequence));
                            break;
                    }

                    /////////////////////////////////////////////
                    //自動希釈で使用する希釈液の計算
                    var autoDiluRequire = calcDilution(autoDilutionRatio);
                    tipRequire += autoDiluRequire.Item1;
                    dilRequire += autoDiluRequire.Item3;

                    //////////////////////////
                    // 多重測定回数分乗算を行う
                    r1RequireSum += r1Require * replicationCount;
                    r2RequireSum += r2Require * replicationCount;
                    mRequireSum += mRequire * replicationCount;
                    protocolGroupedTestCount *= replicationCount;

                    preTriggerRequireSum += preTriggerRequire * replicationCount;
                    triggerRequireSum += triggerRequire * replicationCount;
                    tipRequireSum += tipRequire * replicationCount;
                    dilRequireSum += dilRequire * replicationCount;
                    preProcess1RequireSum += preProcess1Require * replicationCount;
                    preProcess2RequireSum += preProcess2Require * replicationCount;

                    TestCountWithDispenceVol useInfo = new TestCountWithDispenceVol();
                    useInfo.TestCount = protocolGroupedTestCount;
                    useInfo.R1DispenceVolume = measureProtocol.R1DispenseVolume;
                    useInfo.R2DispenceVolume = measureProtocol.R2DispenseVolume;
                    useInfo.MDispenceVolume = measureProtocol.MReagDispenseVolume;
                    reagAmountInf.UseVolumeList.Add(useInfo);
                }

                // 試薬使用量は、R1,R2,M試薬の最大値
                // 試薬の使用量はテスト数に換算して計算を行う。
                // これは複数の分析項目で同一の試薬が使用されているケースがある為（分析項目毎で分注量は異なる）
                reagAmountInf.Amount += Math.Max(mRequireSum, Math.Max(r1RequireSum, r2RequireSum));
                reagAmountInf.ProtocolIndex = firstElement.Item1.ProtocolIndex;
            }

            amount.PreTriggerAmountInfo.Amount = preTriggerRequireSum; //CarisXSubFunction.GetRealRemainCount( ReagentKind.Pretrigger, testCount );
            amount.TriggerAmountInfo.Amount = triggerRequireSum;//CarisXSubFunction.GetRealRemainCount( ReagentKind.Trigger, testCount );
            amount.DilutionAmountInfo.Amount = dilRequireSum;
            amount.SampleTipAmountInfo.Amount = tipRequireSum;
            amount.PreProcess1AmountInfo.Amount = preProcess1RequireSum;
            amount.PreProcess2AmountInfo.Amount = preProcess2RequireSum;
            // 洗浄液・リンス液残量はここではチェックせず、有り無しのみの確認

            return amount;
        }

        /// <summary>
        /// 希釈倍率に応じたチップ、セル、希釈液の必要数の算出
        /// </summary>
        /// <param name="ratio"></param>
        /// <returns>使用するチップ、セル、希釈液 Item1:チップ,Item2:セル,Item3:希釈液</returns>
        static private Tuple<Int32, Int32, Int32> calcDilution(int ratio)
        {
            Int32 tip = 0, cell = 0, dilu = 0; 

            switch (ratio)
            {
                case (int)MeasureProtocol.DilutionRatioKind.x1:
                    break;
                case (int)MeasureProtocol.DilutionRatioKind.x10:
                    tip += 1;
                    cell += 1;
                    dilu += CarisXConst.DIL_DISPENCE_VOLUME_FOR_X10;
                    break;
                case (int)MeasureProtocol.DilutionRatioKind.x20:
                    tip += 1;
                    cell += 1;
                    dilu += CarisXConst.DIL_DISPENCE_VOLUME_FOR_X20;
                    break;
                case (int)MeasureProtocol.DilutionRatioKind.x100:
                    tip += 2;
                    cell += 2;
                    dilu += CarisXConst.DIL_DISPENCE_VOLUME_FOR_X10 * 2;
                    break;
                case (int)MeasureProtocol.DilutionRatioKind.x200:
                    tip += 2;
                    cell += 2;
                    dilu += CarisXConst.DIL_DISPENCE_VOLUME_FOR_X10 + CarisXConst.DIL_DISPENCE_VOLUME_FOR_X20;
                    break;
                case (int)MeasureProtocol.DilutionRatioKind.x400:
                    tip += 2;
                    cell += 2;
                    dilu += CarisXConst.DIL_DISPENCE_VOLUME_FOR_X20 * 2;
                    break;
                case (int)MeasureProtocol.DilutionRatioKind.x1000:
                    tip += 3;
                    cell += 3;
                    dilu += CarisXConst.DIL_DISPENCE_VOLUME_FOR_X10 * 3;
                    break;
                case (int)MeasureProtocol.DilutionRatioKind.x2000:
                    tip += 3;
                    cell += 3;
                    dilu += (CarisXConst.DIL_DISPENCE_VOLUME_FOR_X10 * 2) + CarisXConst.DIL_DISPENCE_VOLUME_FOR_X20;
                    break;
                case (int)MeasureProtocol.DilutionRatioKind.x4000:
                    tip += 3;
                    cell += 3;
                    dilu += CarisXConst.DIL_DISPENCE_VOLUME_FOR_X10 + (CarisXConst.DIL_DISPENCE_VOLUME_FOR_X20 * 2);
                    break;
                case (int)MeasureProtocol.DilutionRatioKind.x8000:
                    tip += 3;
                    cell += 3;
                    dilu += CarisXConst.DIL_DISPENCE_VOLUME_FOR_X20 * 3;
                    break;
            }

            return new Tuple<Int32, Int32, Int32>(tip, cell, dilu);
        }

        /// <summary>
        /// 登録済検体使用試薬情報取得
        /// </summary>
        /// <remarks>
        /// 検体DBに登録されている情報から、登録内容に対しての使用試薬関連情報を取得します。
        /// </remarks>
        /// <returns>使用試薬関連情報 Item1:Protocol,Item2:多重測定回数,Item3:自動希釈倍率</returns>
        static public IEnumerable<Tuple<MeasureProtocol, Int32, Int32>> GetSpecimenDBRegisteredProtocolAndRepCountAndAutoDil()
        {
            // 登録済検体データの試薬使用量を取得する
            var sampleUse = from v in Singleton<SpecimenGeneralDB>.Instance.GetRegisterdProtocolsAndAutoDil()
                            select new Tuple<MeasureProtocol, Int32, Int32>( v.Item1, v.Item1.RepNoForSample, v.Item2 );

            // 登録済STATデータの試薬使用量を取得する
            var statUse = from v in Singleton<SpecimenStatDB>.Instance.GetRegisterdProtocolsAndAutoDil()
                            select new Tuple<MeasureProtocol, Int32, Int32>( v.Item1, v.Item1.RepNoForSample, v.Item2 );

            // キャリブレータ・精度管理検体の残量確認は行わない

            // 取得した各検体の試薬使用量リストを統合する
            IEnumerable<Tuple<MeasureProtocol, Int32, Int32>> unionList = sampleUse.Union( statUse );

            // 分析項目・多重測定回数・自動希釈倍率のリスト
            return unionList;
        }

        /// <summary>
        /// 登録検体分析項目情報取得
        /// </summary>
        /// <remarks>
        /// 登録済検体の使用分析項目・試薬コードのコレクションを返します。
        /// </remarks>
        /// <returns>使用分析項目・試薬コードコレクション</returns>
        static public IEnumerable<Tuple<MeasureProtocol, String>> GetAllDBRegisteredProtocolAndNowLotNo(int moduleId)
        {
            // 登録済検体データの試薬使用量を取得する
            // 残量0の試薬を無視して現ロットを取得しないよう変更
            var sampleUse = from v in Singleton<SpecimenGeneralDB>.Instance.GetRegisterdProtocolsAndAutoDil()
                            select new Tuple<MeasureProtocol, String>( v.Item1, Singleton<ReagentDB>.Instance.GetNowReagentLotNo( v.Item1.ReagentCode, true, moduleId) );

            // 登録済STATデータの試薬使用量を取得する
            var statUse = from v in Singleton<SpecimenStatDB>.Instance.GetRegisterdProtocolsAndAutoDil()
                            select new Tuple<MeasureProtocol, String>( v.Item1, Singleton<ReagentDB>.Instance.GetNowReagentLotNo( v.Item1.ReagentCode, true, moduleId) );

            // キャリブレータ・精度管理検体の残量確認は行わない

            // 取得した各検体の試薬使用量リストを統合する
            IEnumerable<Tuple<MeasureProtocol, String>> unionList = sampleUse.Union( statUse ).Distinct();

            // 分析項目・多重測定回数・自動希釈倍率のリスト
            return unionList;
        }

        /// <summary>
        /// 全登録検体情報使用分析項目取得
        /// </summary>
        /// <remarks>
        /// 検体・精度管理検体・の登録情報から、使用する分析項目コレクションを取得します。
        /// </remarks>
        /// <returns>分析項目コレクション</returns>
        static public IEnumerable<MeasureProtocol> GetDBRegisteredProtocol()
        {
            List<MeasureProtocol> sampleUse = Singleton<SpecimenGeneralDB>.Instance.GetRegisterdProtocols();

            // 検体登録DBからはWS問合せで削除される為、分析DBの待機中・分析中のものも取得する
            var sampleAssayUse = from v in Singleton<SpecimenAssayDB>.Instance.GetData()
                                 let status = v.GetStatus()
                                 where (status == SampleInfo.SampleMeasureStatus.Wait) || (status == SampleInfo.SampleMeasureStatus.InProcess)
                                 select Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(v.GetMeasureProtocolIndex());

            IEnumerable<MeasureProtocol> controlUse = from v in Singleton<ControlRegistDB>.Instance.GetData()
                                                      from vv in v.GetRegisterdStatus()
                                                      select Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(vv.MeasureProtocolIndex);// from vv in v.) Singleton<ControlRegistDB>.Instance.GetData() select v.GetRegisterdStatus()

            var unionList = sampleUse.Union(controlUse).Union(sampleAssayUse);
            return unionList;
        }

        /// <summary>
        /// キャリブレータ登録検体使用分析項目取得
        /// </summary>
        /// <remarks>
        /// キャリブレータの登録情報から、使用する分析項目コレクションを取得します。
        /// </remarks>
        /// <returns></returns>
        static public IEnumerable<Tuple<MeasureProtocol, String>> GetCalibDBRegisterdProtocol()
        {
            IEnumerable<Tuple<MeasureProtocol, String>> calibUse = from v in Singleton<CalibratorRegistDB>.Instance.GetData()
                                                                   select new Tuple<MeasureProtocol, String>(Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(v.GetMeasureProtocolIndex()), v.ReagentLotNo);
            return calibUse;
        }

        /// <summary>
        /// 全使用分析項目取得
        /// </summary>
        /// <remarks>
        /// 検体・キャリブレータ・精度管理検体に登録されている全分析項目を取得します。
        /// </remarks>
        /// <returns>分析項目リスト（キャリブレータ以外）,キャリブレータで登録されている分析項目リスト</returns>
        static public Tuple<List<MeasureProtocol>, List<Tuple<MeasureProtocol, String>>> GetAllRegisteredProtocol()
        {
            // 全登録DBから使用している分析項目を取得・結合・単一化の後ルーチンテーブル順にソートしてリストを返す
            List<MeasureProtocol> unionList = GetDBRegisteredProtocol().Distinct()
                .OrderBy((value) => Singleton<MeasureProtocolManager>.Instance.GetRoutineTableOrder(value.ProtocolIndex)).ToList();
            List<Tuple<MeasureProtocol, String>> calibList = GetCalibDBRegisterdProtocol().Distinct()
                .OrderBy((value) => Singleton<MeasureProtocolManager>.Instance.GetRoutineTableOrder(value.Item1.ProtocolIndex)).ToList();

            return new Tuple<List<MeasureProtocol>, List<Tuple<MeasureProtocol, String>>>(unionList, calibList);
        }

        /// <summary>
        /// 使用試薬ロット取得
        /// </summary>
        /// <remarks>
        /// 検体・キャリブレータ・精度管理検体で指定した分析項目の使用試薬ロットを取得します。
        /// 現ロットはString.Emptyで取得されます。
        /// </remarks>
        /// <param name="protocol"></param>
        /// <returns></returns>
        static public Tuple<List<String>, List<String>, List<String>> GetAllRegisteredProtocolReagentLot(MeasureProtocol protocol, Int32 moduleId)
        {
            // ***String.Emptyは現ロット扱いとする。

            // 検体登録DBから取得
            var sampleUse = from v in Singleton<SpecimenGeneralDB>.Instance.GetRegisterdProtocols()
                            where v.ProtocolIndex == protocol.ProtocolIndex
                            select String.Empty;

            // STAT登録DBから取得
            var statUse = from v in Singleton<SpecimenStatDB>.Instance.GetRegisterdProtocols()
                            where v.ProtocolIndex == protocol.ProtocolIndex
                            select String.Empty;

            // 検体登録DBからはWS問合せで削除される為、分析DBの待機中・分析中のものも取得する
            var sampleAssayUse = from v in Singleton<SpecimenAssayDB>.Instance.GetData()
                                 let status = v.GetStatus()
                                 where v.GetMeasureProtocolIndex() == protocol.ProtocolIndex
                                     && ((status == SampleInfo.SampleMeasureStatus.Wait) || (status == SampleInfo.SampleMeasureStatus.InProcess))
                                 select v.ReagentLotNo;

            // 検体登録DB・STAT登録DB・検体分析DBのデータ統合する。
            var protocolFilterdSampleUse = sampleUse.Union(statUse).Union(sampleAssayUse).Distinct();

            // キャリブレータ登録DBからデータを取得する。
            var calibUse = (from v in Singleton<CalibratorRegistDB>.Instance.GetData(moduleId)
                            where v.GetMeasureProtocolIndex() == protocol.ProtocolIndex
                            select v.ReagentLotNo).Distinct();

            // 精度管理検体で登録されている情報を分析項目で絞込み
            var controlUse = from v in Singleton<ControlRegistDB>.Instance.GetData()
                             from vv in v.GetRegisterdStatus()
                             where vv.MeasureProtocolIndex == protocol.ProtocolIndex
                             select vv;
            // 現ロット、ロット指定、全ロットを見て展開し、単一化する。
            var controlLotUse = controlUse.SelectMany((v) =>
               {
                   List<String> reagentLotList = new List<String>();
                   switch (v.SelectReagentLot)
                   {
                       case ReagentLotSelect.CurrentLot:
                           reagentLotList.Add(String.Empty);
                           break;
                       case ReagentLotSelect.LotSpecification:
                           reagentLotList.Add(v.ReagentLotNo);
                           break;
                       case ReagentLotSelect.LotAll:
                           reagentLotList.AddRange(Singleton<ReagentDB>.Instance.GetReagentLotNo(protocol.ReagentCode, moduleId));
                           break;
                       default:
                            //不完全な精度管理検体登録データを検出
                            Singleton<CarisXLogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "[GetAllRegisteredProtocolAtSpecifyReagentLot] Detect incomplete controls  registration data");
                           reagentLotList.Add(String.Empty);
                           break;
                   }
                   return reagentLotList;
               }).Distinct();

            // 戻り値作成
            Tuple<List<String>, List<String>, List<String>> result = new Tuple<List<String>, List<String>, List<String>>(protocolFilterdSampleUse.ToList(), calibUse.ToList(), controlLotUse.ToList());
            return result;
        }

        /// <summary>
        /// 分析所要間取得
        /// </summary>
        /// <remarks>
        /// 分析に必要な時間を取得します。
        /// </remarks>
        /// <returns>分析所要時間</returns>
        static public TimeSpan GetRemainTime()
        {
            // 残り時間の最大値を取得する。
            TimeSpan remainTime = CarisXSubFunction.GetTimeSpanFromAssayStatusPosition(0);
            return remainTime;
        }

        /// <summary>
        /// 検体情報検索
        /// </summary>
        /// <remarks>
        /// 指定した情報から登録済検体の受付番号を検索します。
        /// </remarks>
        /// <param name="rackId">ラックID</param>
        /// <param name="rackPos">ラックポジション</param>
        /// <param name="sampleId">検体ID</param>
        /// <returns>受付番号</returns>
        static public Int32 SearchReceiptNumberFromSpecimenRegisteredDB(CarisXIDString rackId, Int32 rackPos, String sampleId, Boolean isPriority )
        {

            Int32 receiptNumber = 0;

            if (!isPriority)
            {
                // 一時検体登録
                receiptNumber = Singleton<SpecimenGeneralDB>.Instance.SearchReceiptNumber( rackId, rackPos, sampleId );
            }
            else
            {
                // STAT検体登録
                receiptNumber = Singleton<SpecimenStatDB>.Instance.SearchReceiptNumber( rackId, sampleId );
            }

            return receiptNumber;
        }
        /// <summary>
        /// 検体コメント検索
        /// </summary>
        /// <remarks>
        /// 検体DBからコメント情報を取得します。
        /// </remarks>
        /// <param name="rackId">ラックID</param>
        /// <param name="rackPos">ラックポジション</param>
        /// <param name="sampleId">検体ID</param>
        /// <returns>コメント情報</returns>
        static public String SearchCommentFromSpecimenRegisteredDB(CarisXIDString rackId, Int32 rackPos, String sampleId, Boolean isPriority )
        {
            String comment = String.Empty;

            if (!isPriority)
            {
                // 一般検体登録
                comment = Singleton<SpecimenGeneralDB>.Instance.SearchComment( rackId, rackPos, sampleId );
            }
            else
            {
                // STAT検体登録
                comment = Singleton<SpecimenStatDB>.Instance.SearchComment( rackId, sampleId );
            }

            return comment;
        }

        /// <summary>
        /// 精度管理検体コメント検索
        /// </summary>
        /// <remarks>
        /// 精度管理検体登録情報からコメント情報を取得します。
        /// </remarks>
        /// <param name="rackId">ラックID</param>
        /// <param name="rackPos">ラックポジション</param>
        /// <returns>コメント情報</returns>
        static public String SearchCommentFromControlRegistDB(CarisXIDString rackId, Int32 rackPos)
        {
            String comment = String.Empty;
            try
            {
                var searched = from v in Singleton<ControlRegistDB>.Instance.GetData()
                               where v.RackID.DispPreCharString == rackId.DispPreCharString && v.RackPosition == rackPos
                               select v;
                comment = searched.First().Comment;
            }
            catch (Exception)
            {
            }

            return comment;
        }

        /// <summary>
        /// プレトリガロット番号検索
        /// </summary>
        /// <remarks>
        /// 試薬DBから現プレトリガロット番号を検索します。
        /// </remarks>
        /// <returns>プレトリガロット番号</returns>
        static public String SearchPreTriggerLotNoFromReagentDB()
        {
            String searched = String.Empty;
            var preTrigger = Singleton<ReagentDB>.Instance.GetData(Const.ReagentKind.Pretrigger).Where((v) => v.IsUse.HasValue && v.IsUse.Value);
            if (preTrigger.Count() != 0)
            {
                searched = preTrigger.First().LotNo;
            }

            return searched;
        }
        /// <summary>
        /// トリガロット番号検索
        /// </summary>
        /// <remarks>
        /// 試薬DBから現トリガロット番号を検索します。
        /// </remarks>
        /// <returns>トリガロット番号</returns>
        static public String SearchTriggerLotNoFromReagentDB()
        {
            String searched = String.Empty;
            var trigger = Singleton<ReagentDB>.Instance.GetData(Const.ReagentKind.Trigger).Where((v) => v.IsUse.HasValue && v.IsUse.Value);
            if (trigger.Count() != 0)
            {
                searched = trigger.First().LotNo;
            }

            return searched;
        }
        /// <summary>
        /// 検量線データ検索
        /// </summary>
        /// <remarks>
        /// 検量線DBから指定分析項目の検量線を検索します。
        /// </remarks>
        /// <param name="protocolIndex">分析項目インデックス</param>
        /// <returns>検量線成立日時</returns>
        static public DateTime SearchCalibDateFromCalibDB(Int32 protocolIndex, Int32 moduleNo )
        {
            DateTime dtm = DateTime.MinValue;
            var calib = Singleton<CalibrationCurveDB>.Instance.GetData(protocolIndex, moduleNo);
            if (calib.Count != 0)
            {
                dtm = calib.First().Value.First().Value.First().GetApprovalDateTime();
            }
            return dtm;
        }

        /// <summary>
        /// 精度管理検体名検索
        /// </summary>
        /// <remarks>
        /// 精度管理検体ロット番号から精度管理検体名を検索します。
        /// </remarks>
        /// <param name="controlLot">精度管理検体ロット番号</param>
		/// <returns>精度管理検体名</returns>
		static public String SearchControlNameFromControlDB(String controlLot, CarisXIDString rackId, Int32 rackPos)
        {
            String controlName = String.Empty;

            var control = Singleton<ControlRegistDB>.Instance.GetData()
                .Where((v) => (v.ControlLotNo == controlLot)
                                   && (v.RackID.DispPreCharString == rackId.DispPreCharString)
                                   && (v.RackPosition == rackPos));

            if (control.Count() != 0)
            {
                controlName = control.First().ControlName;
            }

            return controlName;

        }

        /// <summary>
        /// 精度管理データ平均算出
        /// </summary>
        /// <remarks>
        /// 精度管理検体測定結果より、指定した条件の測定結果から平均カウントを算出します。
        /// </remarks>
        /// <param name="measureProtocolIndex">分析項目インデックス</param>
        /// <param name="controlLot">精度管理ロット番号</param>
        /// <param name="controlName">精度管理検体名</param>
        /// <param name="concentration">濃度値</param>
        /// <returns>平均カウント</returns>
        static public Double SearchTodayAverageFromControlDB(Int32 measureProtocolIndex, String controlLot, String controlName, Double? concentration = null)
        {
            var control = Singleton<ControlResultDB>.Instance.GetData(measureProtocolIndex).
                Where((v) => v.ControlLotNo == controlLot &&
               v.ControlName == controlName &&
               v.GetConcentration() != null &&
               !v.GetConcentration().Contains(CarisXConst.COUNT_CONCENTRATION_NOTHING)).
                Select((data) => Double.Parse(data.GetConcentration()));

            return (control.Sum() - (control.Count() > 1 ? concentration ?? 0 : 0)) /
                (control.Count() - (concentration.HasValue && control.Count() > 1 ? 1 : 0));
        }

        /// <summary>
        /// 試薬コード別現ロット取得
        /// </summary>
        /// <remarks>
        /// 試薬DBより試薬ロットの現ロットを取得します。
        /// </remarks>
        /// <param name="reagentCode">試薬コード</param>
        /// <returns>現ロット</returns>
        static public String SearchReagentLotFromReagentDB(Int32 reagentCode)
        {
            String reagentLot = Singleton<ReagentDB>.Instance.GetNowReagentLotNo(reagentCode);
            return reagentLot;
        }

        /// <summary>
        /// 試薬ロットの取得
        /// </summary>
        /// <remarks>
        /// 試薬DBより指定の試薬コードに該当する試薬ロットを取得します。
        /// </remarks>
        /// <param name="reagentCode">試薬コード</param>
        /// <returns>試薬ロット</returns>
        static public String[] SearchAllReagentLotFromReagentDB(Int32 reagentCode, Int32 moduleId)
        {
            return Singleton<ReagentDB>.Instance.GetReagentLotNo(reagentCode, moduleId);
        }

        /// <summary>
        /// キャリブレーション登録情報の濃度値取得
        /// </summary>
        /// <remarks>
        /// キャリブレーション登録情報DBより指定の条件に一致する濃度値を取得します。
        /// </remarks>
        /// <param name="rackID">ラックID</param>
        /// <param name="rackPosition">ラックポジション</param>
        /// <returns>抽出データの濃度値</returns>
        static public String SearchConcentrationFromCalibratorRegistDB(Int32 moduleId, CarisXIDString rackID, Int32 rackPosition)
        {
            return Singleton<CalibratorRegistDB>.Instance.GetData(moduleId).Single((data) => data.RackID.DispPreCharString == rackID.DispPreCharString && data.RackPosition == rackPosition).Concentration;
        }

        /// <summary>
        /// 分析項目、試薬ロット毎の最新検量線取得Analysis items, latest calibration curve acquisition of each reagent lot
        /// </summary>
        /// <remarks>
        /// 検量線情報DBより、分析項目・試薬ロットをキーにそれぞれの最新の検量線を取得します。
        /// </remarks>
        /// <returns>Key:分析項目インデックス,試薬ロット番号 Value:キャリブレータ解析データ郡)Key: analysis item index, reagent lot number Value: calibrator analysis data County)</returns>
        static public Dictionary<Int32, Dictionary<Int32, Dictionary<String, List<CalibrationCurveData>>>> SearchLastCalibCurveFromCalibCurveDB( Int32 moduleNo = CarisXConst.ALL_MODULEID )
        {
            return Singleton<CalibrationCurveDB>.Instance.GetLastDateInfo( moduleNo );
        }

        /// <summary>
        /// 検量線情報DBより検量線の取得
        /// </summary>
        /// <remarks>
        /// 指定の条件で検量線情報を取得します。
        /// </remarks>
        /// <param name="protocolIndex">分析項目インデックス</param>
        /// <param name="reagentLotNo">試薬ロット番号</param>
        /// <param name="approvalDateTime">検量線作成日時</param>
        /// <returns>検量線情報</returns>
        static public List<CalibrationCurveData> SearchCalibCurveFromCalibCurveDB( Int32 protocolIndex, String reagentLotNo, DateTime approvalDateTime, Int32 moduleNo = CarisXConst.ALL_MODULEID )
        {
            return Singleton<CalibrationCurveDB>.Instance.GetData(protocolIndex, reagentLotNo, approvalDateTime, moduleNo);
        }

        /// <summary>
        /// 同一ユニーク番号データ取得
        /// </summary>
        /// <remarks>
        /// 一般検体(優先検体)分析情報DBより指定のユニーク番号と同一のユニーク番号を持つデータを抽出し取得します。
        /// </remarks>
        /// <param name="uniqueNo">ユニーク番号</param>
        /// <returns>抽出データ</returns>
        static public IEnumerable<SpecimenAssayData> SearchHasCountSpecimenAssayData(Int32 uniqueNo)
        {
            return Singleton<SpecimenAssayDB>.Instance.GetData().Where((data) => data.GetUniqueNo() == uniqueNo);
        }

        /// <summary>
        /// 同一ユニーク番号データ取得
        /// </summary>
        /// <remarks>
        /// 精度管理検体分析情報DBより指定のユニーク番号と同一のユニーク番号を持つデータを抽出し取得します。
        /// </remarks>
        /// <param name="uniqueNo">ユニーク番号</param>
        /// <returns>抽出データ</returns>
        static public IEnumerable<ControlAssayData> SearchHasCountControlAssayData(Int32 uniqueNo)
        {
            return Singleton<ControlAssayDB>.Instance.GetData().Where((data) => data.GetUniqueNo() == uniqueNo);
        }

        /// <summary>
        /// 同一ユニーク番号データ取得
        /// </summary>
        /// <remarks>
        /// キャリブレーション分析情報DBより指定のユニーク番号と同一のユニーク番号を持つデータを抽出し取得します。
        /// </remarks>
        /// <param name="uniqueNo">ユニーク番号</param>
        /// <returns>抽出データ</returns>
        static public IEnumerable<CalibratorAssayData> SearchHasCountCalibratorAssayData(Int32 uniqueNo)
        {
            return Singleton<CalibratorAssayDB>.Instance.GetData().Where((data) => data.GetUniqueNo() == uniqueNo);
        }

        /// <summary>
        /// 同一検量線を算出するキャリブレーションを取得
        /// </summary>
        /// <remarks>
        /// キャリブレーション測定結果DBより指定のキャリブレーションと同一検量線を算出するための  全てのキャリブレーション測定結果を取得します。
        /// </remarks>
        /// <param name="protocolIndex">指定のキャリブレーションの分析項目</param>
        /// <param name="rackId">指定のキャリブレーションのラックID</param>
        /// <param name="rackPosition">指定のキャリブレーションのラックポジション</param>
        /// <returns>同検量線上の測定ポイントの元データとなる測定結果(検量線未作成のみ)</returns>
        static public IEnumerable<CalibratorResultData> SearchCalibResultDataFromCalibResultDBAndCalibRegistDB(Int32 protocolIndex, CarisXIDString rackId, Int32 rackPosition)
        {
            var resultDataList = Singleton<CalibratorResultDB>.Instance.GetData();
            var registDataList = Singleton<CalibratorRegistDB>.Instance.GetData();
            var registData = registDataList.First((data) => data.GetMeasureProtocolIndex() == protocolIndex && data.RackID.DispPreCharString == rackId.DispPreCharString && data.RackPosition == rackPosition);
            var registDataOfCurve = registDataList.Where((data) => data.GetStartRackID().DispPreCharString == registData.GetStartRackID().DispPreCharString && data.GetStartRackPosition() == registData.GetStartRackPosition());
            return resultDataList.Where((data) => registDataOfCurve.Count((regData) => regData.RackID.DispPreCharString == data.RackId.DispPreCharString && regData.RackPosition == data.RackPosition) > 0);
        }


        static public IEnumerable<CalibratorResultData> SearchCalibResultDataFromCalibResultDb(Int32 protocolIndex, int sequenceNo)
        {
            var resultDataList = Singleton<CalibratorResultDB>.Instance.GetData();
            return resultDataList.Where((data) => data.GetMeasureProtocolIndex() == protocolIndex && data.SequenceNo == sequenceNo);
        }

        /// <summary>
        /// キャリブレーション登録情報取得
        /// </summary>
        /// <remarks>
        /// キャリブレーション登録情報DBより指定の条件でデータ抽出し取得します。
        /// </remarks>
        /// <param name="protocolIndex">分析項目インデックス</param>
        /// <param name="startRackId">キャリブレーション開始ラックID</param>
        /// <param name="startRackPosition">キャリブレーション開始ラックポジション</param>
        /// <returns>抽出データ</returns>
        static public IEnumerable<CalibratorRegistData> SearchCalibRegistDataFromCalibRegistDB(Int32 protocolIndex, CarisXIDString startRackId, Int32 startRackPosition)
        {
            return Singleton<CalibratorRegistDB>.Instance.GetData().Where((data) => data.GetMeasureProtocolIndex() == protocolIndex && data.GetStartRackID().DispPreCharString == startRackId.DispPreCharString && data.GetStartRackPosition() == startRackPosition);
        }

        /// <summary>
        /// キャリブレーション登録情報取得
        /// </summary>
        /// <remarks>
        /// キャリブレーション登録DBよりラックID・ラックポジションを使用して、
        /// 同一検量線測定登録として登録されたデータ集合を取得します。
        /// </remarks>
        /// <param name="rackId">ラックID</param>
        /// <param name="rackPos">ラックポジション</param>
        /// <returns>キャリブレーション登録情報</returns>
        static public IEnumerable<CalibratorRegistData> SearchCalibRegistDataFromRackIdPos(Int32 moduleId, CarisXIDString rackId, Int32 rackPos)
        {
            var dataList = Singleton<CalibratorRegistDB>.Instance.GetData(moduleId);
            var result = from v in dataList
                         where v.RackID.Value == rackId.Value && v.RackPosition == rackPos
                         from vv in dataList
                         where vv.GetStartRackID().Value == v.GetStartRackID().Value && vv.GetStartRackPosition() == v.GetStartRackPosition()
                         select vv;
            return result;
        }

        static public IEnumerable<CalibratorAssayData> SearchCalibDataFromReagentLotCalibLotNo(int nProtoclIndex, String strReagentLotNo, String strCaliLotNo)
        {
            return Singleton<CalibratorAssayDB>.Instance.GetData(nProtoclIndex, strReagentLotNo, strCaliLotNo);
        }

        /// <summary>
        /// キャリブレーション分析問合せ
        /// </summary>
        /// <remarks>
        /// キャリブレーション登録DB・分析中検体情報を参照し、
        /// 指定したラックID・ラックポジションのデータ検出可否を返します。
        /// </remarks>
        /// <param name="rackId">ラックID</param>
        /// <param name="rackPos">ラックポジション</param>
        /// <returns>検出結果 True:検出 False:非検出</returns>
        static public IEnumerable<SampleInfo> SearchInprocessCalibRegistDataFromRackIdPos(Int32 moduleId, CarisXIDString rackId, Int32 rackPos)
        {
            IEnumerable<SampleInfo> result = null;

            // ラックID・Posからキャリブレータ登録DB内の同一登録項目を取得する。
            // 取得した内容から、現在分析中の検体内に同一ラックID・Posがあれば検出扱いとする。
            //var searchRackPos = SearchCalibRegistDataFromRackIdPos( rackId, rackPos );
            //if ( searchRackPos.Count() != 0 )
            //{


            // TODO:ここを直す！
            // 同じIDPosに大してふくすうの登録の場合を考えた処理に変える！
            var rackIdPosGrp = from v in SearchCalibRegistDataFromRackIdPos(moduleId, rackId, rackPos) orderby v.RackID.Value group v by v.RackID.DispPreCharString;
            result = from v in rackIdPosGrp
                     let protocolIndex = v.First().GetMeasureProtocolIndex()
                     let rackInProcess = Singleton<InProcessSampleInfoManager>.Instance.SearchInProcessSampleFromRackId(v.Key)
                     from vv in rackInProcess
                     from vvv in v
                     where (vv.RackPos == vvv.RackPosition)
                        && (vv.GetRegisterdProtocolIndexList().Contains(protocolIndex))
                        && (vv.ModuleID == moduleId)
                     select vv;

            //result = from v in SearchCalibRegistDataFromRackIdPos( rackId, rackPos )
            //         let rackInProcess = Singleton<InProcessSampleInfoManager>.Instance.SearchInProcessSampleFromRackId( v.RackID )
            //         from vv in rackInProcess
            //         where vv.RackPos == v.RackPosition
            //         select vv;

            //}

            return result;
        }



        /// <summary>
        /// 濃度値文字列取得
        /// </summary>
        /// <remarks>
        /// キャリブレーション登録情報DBより指定の条件でデータを抽出し、濃度値の文字列を取得します。
        /// </remarks>
        /// <param name="protocolIndex">分析項目インデックス</param>
        /// <param name="rackId">ラックID</param>
        /// <param name="rackPosition">ラックポジション</param>
        /// <returns>濃度値文字列</returns>
        static public String SearchConcentrationFromCalibRegistDB(Int32 protocolIndex, CarisXIDString rackId, Int32 rackPosition)
        {
            return Singleton<CalibratorRegistDB>.Instance.GetData().Where((data) => data.GetMeasureProtocolIndex() == protocolIndex && data.RackID.DispPreCharString == rackId.DispPreCharString && data.RackPosition == rackPosition).Single().Concentration;
        }

        /// <summary>
        /// 判断输入的架子位置是不是校准品测试的第一个位置
        /// </summary>
        /// <param name="ProtoclIndex"></param>
        /// <param name="rackId"></param>
        /// <param name="rackPosition"></param>
        /// <returns></returns>
        static public bool searchCheckFirstRackAndPosition(Int32 ProtoclIndex, CarisXIDString rackId, Int32 rackPosition, int nUniqueNo)
        {
            //如果是手动注册，则查找校准品注册界面是否是第一个校准点
            var registData = Singleton<CalibratorRegistDB>.Instance.GetData().Where((data) => data.GetMeasureProtocolIndex() == ProtoclIndex && data.RackID.DispPreCharString == rackId.DispPreCharString && data.RackPosition == rackPosition);
            if (registData != null && registData.Count() != 0)
            {
                return registData.First().GetStartRackID().DispPreCharString == rackId.DispPreCharString && registData.First().GetStartRackPosition() == rackPosition;
            }
            //如果是自动注册，判断是否是第一个校准方法：下一个UniqueNo和查询的UniqueNo是否是同一组的，若是，则为第一个，若否，则非第一个
            CalibratorAssayData calibAssayData1 = Singleton<CalibratorAssayDB>.Instance.GetData(rackId).Find((assay) => assay.GetUniqueNo() == nUniqueNo);
            if (calibAssayData1 != null)
            {
                CalibratorAssayData calibAssayData2 = Singleton<CalibratorAssayDB>.Instance.GetData().Find((assay) => assay.GetUniqueNo() == (calibAssayData1.GetUniqueNo() + 1));
                if (calibAssayData2 != null)
                {
                    if (calibAssayData1.GetSequenceNo() == calibAssayData2.GetSequenceNo())
                    {
                        return true;
                    }
                }
            }

            return false;



            //try
            //{
            //    return registData.First().GetStartRackID().DispPreCharString == rackId.DispPreCharString && registData.First().GetStartRackPosition() == rackPosition;
            //}
            // catch (System.Exception ex)
            // {

            //    return false;
            //}         

        }

        //static public bool searchCheckFirstReckAndPositionFromCalibRegeistDB(CarisXIDString rackId, Int32 rackPosition)
        //{
        //    var registData = Singleton<CalibratorRegistDB>.Instance.GetData().Where((data) => data.RackID.DispPreCharString == rackId.DispPreCharString && data.RackPosition == rackPosition);
        //    if (registData == null)
        //    {
        //        return false;
        //    }
        //    return registData.First().GetStartRackID().DispPreCharString == rackId.DispPreCharString && registData.First().GetStartRackPosition() == rackPosition;
        //}
        /// <summary>
        /// 同一検量線を算出するキャリブレーションを取得
        /// </summary>
        /// <remarks>
        /// 指定のキャリブレーションと同一検量線を算出するための全てのキャリブレーション測定結果を取得します。
        /// </remarks>
        /// <param name="protocolIndex">分析項目インデックス</param>
        /// <param name="rackId">ラックID</param>
        /// <param name="rackPosition">ラックポジション</param>
        /// <returns>キャリブレーション登録情報</returns>
        static public IEnumerable<CalibratorRegistData> SearchCalibCurveCalibRegistDataFromCalibRegistDB(Int32 protocolIndex, CarisXIDString rackId, Int32 rackPosition)
        {
            var registData = Singleton<CalibratorRegistDB>.Instance.GetData().Where((data) => data.GetMeasureProtocolIndex() == protocolIndex && data.RackID.DispPreCharString == rackId.DispPreCharString && data.RackPosition == rackPosition);
            return Singleton<CalibratorRegistDB>.Instance.GetData().Where((data) => data.GetStartRackID().DispPreCharString == registData.First().GetStartRackID().DispPreCharString && data.GetStartRackPosition() == registData.First().GetStartRackPosition());
        }

        //static public Remark SearchExpiredRemaksFromReagentDB(Int32 reagentCode,String reagentLotNo = null)
        //{
        //    var reagentList = Singleton<ReagentDB>.Instance.GetData().Where((data)=> ((data.ReagentKind == (Int32)ReagentKind.Reagent && data.ReagentCode == reagentCode && ((reagentLotNo ?? data.LotNo) == data.LotNo )) || (data.IsUse ?? false ) &&( (data.ExpirationDate ?? DateTime.Today.AddDays(-1)) > DateTime.Today.Date));

        //    Remark remark = new Remark();
        //    foreach ( var kind in reagentList.Select((data)=>(ReagentKind)data.ReagentKind))
        //    {
        //        switch (kind)
        //        {
        //            case ReagentKind.Reagent:
        //                remark.AddRemark(Remark.RemarkBit.ReagentExpirationDateError);
        //                break;
        //            case ReagentKind.Diluent:
        //                remark.AddRemark(Remark.RemarkBit.DilutionExpirationDateError);
        //                break;
        //            case ReagentKind.Pretrigger:
        //                remark.AddRemark(Remark.RemarkBit.PreTriggerExpirationDateError);
        //                break;
        //            case ReagentKind.Trigger:
        //                remark.AddRemark(Remark.RemarkBit.TriggerExpirationDateError);
        //                break;
        //            default:
        //                break;
        //        }
        //    }

        //    return remark;
        //}

        /// <summary>
        /// プレトリガ、トリガ、希釈液が有効期限エラーかどうか
        /// </summary>
        /// <remarks>
        /// 指定のプレトリガ、トリガ、希釈液が指定の測定日時の時有効期限エラーとなるかどうかを取得します。
        /// </remarks>
        /// <param name="reagentKind">試薬種別</param>
        /// <param name="lotNo">試薬ロット番号</param>
        /// <param name="measureDateTime">測定日時</param>
        /// <returns>true:エラー</returns>
        public static Boolean SearchReagentExpirationDateErrorFromReagentDB(ReagentKind reagentKind, String lotNo, DateTime measureDateTime)
        {
            var expirationData = Singleton<ReagentDB>.Instance.GetData(reagentKind).Where((reagentData) =>
             (String.IsNullOrEmpty(lotNo) && (reagentData.IsUse ?? false)) ||
             !String.IsNullOrEmpty(lotNo) && reagentData.LotNo == lotNo).
                Select((data) => data.ExpirationDate).FirstOrDefault();

            if (expirationData != null && measureDateTime >= expirationData.Value.AddMonths(1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 試薬期限エラー検索
        /// </summary>
        /// <remarks>
        /// 指定の試薬種別、試薬ロット番号の試薬時有効期限エラーとなるかどうかを取得します。
        /// </remarks>
        /// <param name="reagentCode">試薬コード</param>
        /// <param name="lotNo">試薬ロット番号</param>
        /// <param name="measureDateTime">測定日時</param>
        /// <returns>true:エラー</returns>
        public static Boolean SearchReagentExpirationDateErrorFromReagentDB(Int32 reagentCode, String lotNo, DateTime measureDateTime)
        {
            var expirationData = from reagentData in Singleton<ReagentDB>.Instance.GetData(ReagentKind.Reagent)
                                 where (reagentData.ReagentCode == reagentCode) &&                          // 試薬コード一致
                                       (!String.IsNullOrEmpty(lotNo) && (reagentData.LotNo == lotNo))		// 試薬ロット一致
                                 && (reagentData.Remain.HasValue && (reagentData.Remain.Value > 0))         // 指定残量以下を除外 */
                                 orderby reagentData.ExpirationDate ascending                                   // 有効期限昇順ソートにより最古の有効期限を先頭とする
                                 select reagentData.ExpirationDate;

            Boolean result = false;
            if (expirationData.Count() != 0)
            {
                if (measureDateTime >= expirationData.First().Value.AddMonths(1))
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
