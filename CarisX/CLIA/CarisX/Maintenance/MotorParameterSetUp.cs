using Oelco.CarisX.Comm;
using Oelco.CarisX.Const;
using Oelco.CarisX.Parameter;
using Oelco.Common.Parameter;
using Oelco.Common.Utility;

namespace Oelco.CarisX.Maintenance
{
    class MotorParameterSetUp
    {
        ParameterFilePreserve<CarisXMotorParameter> motor = Singleton<ParameterFilePreserve<CarisXMotorParameter>>.Instance;

        public MotorParameterSetUp()
        {
            motor.LoadRaw();
        }

        /// <summary>
        /// 受信した各モーターパラメータをセットします。
        /// </summary>
        public void SetUpMotorParam(SlaveCommCommand_0509 cmd0509, int slaveListIdx)
        {
            switch (cmd0509.MotorNo)
            {
                case (int)MotorNoList.CaseTransferYAxis:
                    //ケース搬送部Y軸
                    motor.Param.SlaveList[slaveListIdx].caseTransferYAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].caseTransferYAxisParam.OffsetCaseCatchRelease  = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].caseTransferYAxisParam.OffsetReactionCellCatch = cmd0509.MotorOffset[1];
                    motor.Param.SlaveList[slaveListIdx].caseTransferYAxisParam.OffsetSamplingTipCatch = cmd0509.MotorOffset[2];
                    break;
                case (int)MotorNoList.CaseTransferZAxis:
                    //ケース搬送部Z軸
                    motor.Param.SlaveList[slaveListIdx].caseTransferZAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].caseTransferZAxisParam.OffsetCaseCatchRelease = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].caseTransferZAxisParam.OffsetReactionCellSamplingTipCatch = cmd0509.MotorOffset[1];
                    break;

                case (int)MotorNoList.ReagentStorageTableThetaAxis:
                    //試薬保冷庫テーブルθ軸
                    motor.Param.SlaveList[slaveListIdx].reagentStorageTableThetaAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].reagentStorageTableThetaAxisParam.OffsetMReagentIDReading = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].reagentStorageTableThetaAxisParam.OffsetRReagentIDReading = cmd0509.MotorOffset[1];
                    motor.Param.SlaveList[slaveListIdx].reagentStorageTableThetaAxisParam.OffsetR1UnitR1Aspiration = cmd0509.MotorOffset[2];
                    motor.Param.SlaveList[slaveListIdx].reagentStorageTableThetaAxisParam.OffsetR1UnitR2Aspiration = cmd0509.MotorOffset[3];
                    motor.Param.SlaveList[slaveListIdx].reagentStorageTableThetaAxisParam.OffsetR1UnitMReagentAspiration = cmd0509.MotorOffset[4];
                    motor.Param.SlaveList[slaveListIdx].reagentStorageTableThetaAxisParam.OffsetMRBottleCheck = cmd0509.MotorOffset[5];
                    motor.Param.SlaveList[slaveListIdx].reagentStorageTableThetaAxisParam.OffsetR2UnitMReagentAspiration = cmd0509.MotorOffset[6];
                    motor.Param.SlaveList[slaveListIdx].reagentStorageTableThetaAxisParam.OffsetR2UnitR2Aspiration = cmd0509.MotorOffset[7];
                    motor.Param.SlaveList[slaveListIdx].reagentStorageTableThetaAxisParam.OffsetEncodeThresh = cmd0509.MotorOffset[8];
                    break;
                case (int)MotorNoList.ReagentStorageMixingThetaAxis:
                    //試薬保冷庫撹拌θ軸
                    motor.Param.SlaveList[slaveListIdx].reagentStorageMixingThetaAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    break;
                case (int)MotorNoList.STATYAxis:
                    //スタット部Y軸
                    motor.Param.SlaveList[slaveListIdx].sTATYAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].sTATYAxisParam.OffsetSTATSampleAspiration = cmd0509.MotorOffset[0];
                    break;

                case (int)MotorNoList.SampleDispenseArmYAxis:
                    //サンプル分注移送部Y軸
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmYAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmYAxisParam.OffsetRackSample1Aspiration = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmYAxisParam.OffsetRackSample5Aspiration = cmd0509.MotorOffset[1];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmYAxisParam.OffsetSTATSampleAspiration = cmd0509.MotorOffset[2];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmYAxisParam.OffsetDiluentSampleAspiration = cmd0509.MotorOffset[3];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmYAxisParam.OffsetPretreatSampleAspiration = cmd0509.MotorOffset[4];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmYAxisParam.OffsetLineSampleAspiration = cmd0509.MotorOffset[5];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmYAxisParam.OffsetSampleDispense = cmd0509.MotorOffset[6];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmYAxisParam.OffsetSampleTipRemover = cmd0509.MotorOffset[7];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmYAxisParam.OffsetSampleTip1Catch = cmd0509.MotorOffset[8];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmYAxisParam.OffsetSampleTip6Catch = cmd0509.MotorOffset[9];
                    break;
                case (int)MotorNoList.SampleDispenseArmZAxis:
                    //サンプル分注移送部Z軸
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmZAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmZAxisParam.OffsetRackSampleAspiration = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmZAxisParam.OffsetSTATSampleAspiration = cmd0509.MotorOffset[1];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmZAxisParam.OffsetLineSampleAspiration = cmd0509.MotorOffset[2];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmZAxisParam.OffsetSampleDispenseDilutePretreatAspiration = cmd0509.MotorOffset[3];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmZAxisParam.OffsetSampleTipRemover = cmd0509.MotorOffset[4];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmZAxisParam.OffsetSampleTipCatch = cmd0509.MotorOffset[5];
                    break;
                case (int)MotorNoList.SampleDispenseArmThetaAxis:
                    //サンプル分注移送部θ軸
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmThetaAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmThetaAxisParam.OffsetRackSample1Aspiration = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmThetaAxisParam.OffsetRackSample5Aspiration = cmd0509.MotorOffset[1];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmThetaAxisParam.OffsetSTATSampleAspiration = cmd0509.MotorOffset[2];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmThetaAxisParam.OffsetDiluentSampleAspiration = cmd0509.MotorOffset[3];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmThetaAxisParam.OffsetPretreatSampleAspiration = cmd0509.MotorOffset[4];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmThetaAxisParam.OffsetLineSampleAspiration = cmd0509.MotorOffset[5];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmThetaAxisParam.OffsetSampleDispense = cmd0509.MotorOffset[6];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmThetaAxisParam.OffsetSampleTipRemover = cmd0509.MotorOffset[7];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmThetaAxisParam.OffsetSampleTip1Catch = cmd0509.MotorOffset[8];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseArmThetaAxisParam.OffsetSampleTip6Catch = cmd0509.MotorOffset[9];
                    break;
                case (int)MotorNoList.SampleDispenseSyringe:
                    //サンプル分注シリンジ
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseSyringeParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseSyringeParam.Gain = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseSyringeParam.Offset = cmd0509.MotorOffset[1];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseSyringeParam.GainOver100 = cmd0509.MotorOffset[2];
                    motor.Param.SlaveList[slaveListIdx].sampleDispenseSyringeParam.OffsetOver100 = cmd0509.MotorOffset[3];
                    break;

                case (int)MotorNoList.ReactionCellTransferXAxis:
                    //反応容器搬送部X軸
                    motor.Param.SlaveList[slaveListIdx].reactionCellTransferXAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].reactionCellTransferXAxisParam.OffsetReactionCellCatch = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].reactionCellTransferXAxisParam.OffsetReactionCellRelease = cmd0509.MotorOffset[1];
                    break;
                case (int)MotorNoList.ReactionCellTransferZAxis:
                    //反応容器搬送部Z軸
                    motor.Param.SlaveList[slaveListIdx].reactionCellTransferZAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].reactionCellTransferZAxisParam.OffsetReactionCellCatch = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].reactionCellTransferZAxisParam.OffsetReactionCellRelease = cmd0509.MotorOffset[1];
                    break;

                case (int)MotorNoList.ReactionTableThetaAxis:
                    //反応テーブル部θ軸
                    motor.Param.SlaveList[slaveListIdx].reactionTableThetaAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].reactionTableThetaAxisParam.OffsetHomePosition = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].reactionTableThetaAxisParam.OffsetEncodeThresh = cmd0509.MotorOffset[1];
                    break;
                case (int)MotorNoList.ReactionTableR1MixingZThetaAxis:
                    //撹拌部　R1撹拌Zθ
                    motor.Param.SlaveList[slaveListIdx].reactionTableR1MixingZThetaAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].reactionTableR1MixingZThetaAxisParam.OffsetAPos = cmd0509.MotorOffset[0];
                    break;

                case (int)MotorNoList.BFTableThetaAxis:
                    //BFテーブル部θ軸
                    motor.Param.SlaveList[slaveListIdx].bFTableThetaAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].bFTableThetaAxisParam.OffsetHomePosition = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].bFTableThetaAxisParam.OffsetEncodeThresh = cmd0509.MotorOffset[1];
                    break;
                case (int)MotorNoList.BFTableR2MixingZThetaAxis:
                    //撹拌部　R2撹拌Zθ
                    motor.Param.SlaveList[slaveListIdx].bFTableR2MixingZThetaAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].bFTableR2MixingZThetaAxisParam.OffsetAPos = cmd0509.MotorOffset[0];
                    break;
                case (int)MotorNoList.BFTableBF1MixingZThetaAxis:
                    //撹拌部　BF1撹拌Zθ
                    motor.Param.SlaveList[slaveListIdx].bFTableBF1MixingZThetaAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].bFTableBF1MixingZThetaAxisParam.OffsetAPos = cmd0509.MotorOffset[0];
                    break;
                case (int)MotorNoList.BFTableBF2MixingZThetaAxis:
                    //撹拌部　BF2撹拌Zθ
                    motor.Param.SlaveList[slaveListIdx].bFTableBF2MixingZThetaAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].bFTableBF2MixingZThetaAxisParam.OffsetAPos = cmd0509.MotorOffset[0];
                    break;
                case (int)MotorNoList.BFTablePreTriggerMixingZThetaAxis:
                    //撹拌部　pTr撹拌Zθ
                    motor.Param.SlaveList[slaveListIdx].bFTablePreTriggerMixingZThetaAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].bFTablePreTriggerMixingZThetaAxisParam.OffsetAPos = cmd0509.MotorOffset[0];
                    break;

                case (int)MotorNoList.TravelerXAxis:
                    //トラベラー・廃棄部X軸
                    motor.Param.SlaveList[slaveListIdx].travelerXAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].travelerXAxisParam.OffsetReactionTableInside = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].travelerXAxisParam.OffsetReactionTableOutside = cmd0509.MotorOffset[1];
                    motor.Param.SlaveList[slaveListIdx].travelerXAxisParam.OffsetReactionCellRemover = cmd0509.MotorOffset[2];
                    motor.Param.SlaveList[slaveListIdx].travelerXAxisParam.OffsetBFTableOutside = cmd0509.MotorOffset[3];
                    motor.Param.SlaveList[slaveListIdx].travelerXAxisParam.OffsetBFTableInside = cmd0509.MotorOffset[4];
                    break;
                case (int)MotorNoList.TravelerZAxis:
                    //トラベラー・廃棄部X軸
                    motor.Param.SlaveList[slaveListIdx].travelerZAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].travelerZAxisParam.OffsetReactionTableInside = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].travelerZAxisParam.OffsetReactionTableOutside = cmd0509.MotorOffset[1];
                    motor.Param.SlaveList[slaveListIdx].travelerZAxisParam.OffsetReactionCellRemover = cmd0509.MotorOffset[2];
                    motor.Param.SlaveList[slaveListIdx].travelerZAxisParam.OffsetBFTableOutside = cmd0509.MotorOffset[3];
                    motor.Param.SlaveList[slaveListIdx].travelerZAxisParam.OffsetBFTableInside = cmd0509.MotorOffset[4];
                    break;

                case (int)MotorNoList.R1DispenseArmThetaAxis:
                    //試薬分注1部θ軸
                    motor.Param.SlaveList[slaveListIdx].r1DispenseArmThetaAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].r1DispenseArmThetaAxisParam.OffsetR1Aspiration = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].r1DispenseArmThetaAxisParam.OffsetR2Aspiration = cmd0509.MotorOffset[1];
                    motor.Param.SlaveList[slaveListIdx].r1DispenseArmThetaAxisParam.OffsetMReagentAspiration = cmd0509.MotorOffset[2];
                    motor.Param.SlaveList[slaveListIdx].r1DispenseArmThetaAxisParam.OffsetCuvette = cmd0509.MotorOffset[3];
                    motor.Param.SlaveList[slaveListIdx].r1DispenseArmThetaAxisParam.OffsetReactionCellDispense = cmd0509.MotorOffset[4];
                    motor.Param.SlaveList[slaveListIdx].r1DispenseArmThetaAxisParam.OffsetEncodeThresh = cmd0509.MotorOffset[5];
                    break;
                case (int)MotorNoList.R1DispenseArmZAxis:
                    //試薬分注1部Z軸
                    motor.Param.SlaveList[slaveListIdx].r1DispenseArmZAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].r1DispenseArmZAxisParam.OffsetR1R2Aspiration = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].r1DispenseArmZAxisParam.OffsetMReagentAspiration = cmd0509.MotorOffset[1];
                    motor.Param.SlaveList[slaveListIdx].r1DispenseArmZAxisParam.OffsetCuvette = cmd0509.MotorOffset[2];
                    motor.Param.SlaveList[slaveListIdx].r1DispenseArmZAxisParam.OffsetReactionCellDispense = cmd0509.MotorOffset[3];
                    motor.Param.SlaveList[slaveListIdx].r1DispenseArmZAxisParam.OffsetPositioningProbe = cmd0509.MotorOffset[4];
                    break;
                case (int)MotorNoList.R1DispenseSyringe:
                    //R1分注シリンジ
                    motor.Param.SlaveList[slaveListIdx].r1DispenseSyringeParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].r1DispenseSyringeParam.Gain = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].r1DispenseSyringeParam.Offset = cmd0509.MotorOffset[1];
                    break;

                case (int)MotorNoList.R2DispenseArmThetaAxis:
                    //試薬分注2部θ軸
                    motor.Param.SlaveList[slaveListIdx].r2DispenseArmThetaAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].r2DispenseArmThetaAxisParam.OffsetR2Aspiration = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].r2DispenseArmThetaAxisParam.OffsetMReagentAspiration = cmd0509.MotorOffset[1];
                    motor.Param.SlaveList[slaveListIdx].r2DispenseArmThetaAxisParam.OffsetCuvette = cmd0509.MotorOffset[2];
                    motor.Param.SlaveList[slaveListIdx].r2DispenseArmThetaAxisParam.OffsetReactionCellDispense = cmd0509.MotorOffset[3];
                    motor.Param.SlaveList[slaveListIdx].r2DispenseArmThetaAxisParam.OffsetEncodeThresh = cmd0509.MotorOffset[4];
                    break;
                case (int)MotorNoList.R2DispenseArmZAxis:
                    //試薬分注2部Z軸
                    motor.Param.SlaveList[slaveListIdx].r2DispenseArmZAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].r2DispenseArmZAxisParam.OffsetR2Aspiration = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].r2DispenseArmZAxisParam.OffsetMReagentAspiration = cmd0509.MotorOffset[1];
                    motor.Param.SlaveList[slaveListIdx].r2DispenseArmZAxisParam.OffsetCuvette = cmd0509.MotorOffset[2];
                    motor.Param.SlaveList[slaveListIdx].r2DispenseArmZAxisParam.OffsetReactionCellDispense = cmd0509.MotorOffset[3];
                    motor.Param.SlaveList[slaveListIdx].r2DispenseArmZAxisParam.OffsetPositioningProbe = cmd0509.MotorOffset[4];
                    break;
                case (int)MotorNoList.R2DispenseSyringe:
                    //R2分注シリンジ
                    motor.Param.SlaveList[slaveListIdx].r2DispenseSyringeParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].r2DispenseSyringeParam.Gain = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].r2DispenseSyringeParam.Offset = cmd0509.MotorOffset[1];
                    break;

                case (int)MotorNoList.BF1NozzleZAxis:
                    //BF1部Z軸
                    motor.Param.SlaveList[slaveListIdx].bF1NozzleZAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].bF1NozzleZAxisParam.OffsetReactionCell = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].bF1NozzleZAxisParam.OffsetCuvette = cmd0509.MotorOffset[1];
                    break;
                case (int)MotorNoList.BF1WasteNozzleZAxis:
                    //BF1廃液部Z軸
                    motor.Param.SlaveList[slaveListIdx].bF1WasteNozzleZAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].bF1WasteNozzleZAxisParam.OffsetReactionCell = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].bF1WasteNozzleZAxisParam.OffsetCuvette = cmd0509.MotorOffset[1];
                    break;
                case (int)MotorNoList.BFWashSyringe:
                    //洗浄液ｼﾘﾝｼﾞ
                    motor.Param.SlaveList[slaveListIdx].bFWashSyringeParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].bFWashSyringeParam.Gain = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].bFWashSyringeParam.Offset = cmd0509.MotorOffset[1];
                    break;

                case (int)MotorNoList.BF2NozzleZAxis:
                    //BF2部Z軸
                    motor.Param.SlaveList[slaveListIdx].bF2NozzleZAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].bF2NozzleZAxisParam.OffsetReactionCell = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].bF2NozzleZAxisParam.OffsetCuvette = cmd0509.MotorOffset[1];
                    break;

                case (int)MotorNoList.DiluentDispenseArmZAxis:
                    //希釈液分注部Z軸
                    motor.Param.SlaveList[slaveListIdx].diluentDispenseArmZAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].diluentDispenseArmZAxisParam.OffsetReactionCell = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].diluentDispenseArmZAxisParam.OffsetCuvette = cmd0509.MotorOffset[1];
                    break;
                case (int)MotorNoList.DiluentDispenseSyringe:
                    //希釈液ｼﾘﾝｼﾞ
                    motor.Param.SlaveList[slaveListIdx].diluentDispenseSyringeParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].diluentDispenseSyringeParam.Gain = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].diluentDispenseSyringeParam.Offset = cmd0509.MotorOffset[1];
                    break;

                case (int)MotorNoList.TriggerAndPreTriggerDispenseNozzleZAxis:
                    //トリガ分注
                    motor.Param.SlaveList[slaveListIdx].triggerAndPreTriggerDispenseNozzleZAxisParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].triggerAndPreTriggerDispenseNozzleZAxisParam.OffsetReactionCell = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].triggerAndPreTriggerDispenseNozzleZAxisParam.OffsetCuvette = cmd0509.MotorOffset[1];
                    break;
                case (int)MotorNoList.TriggerDispenseSyringe:
                    //トリガ液ｼﾘﾝｼﾞ
                    motor.Param.SlaveList[slaveListIdx].triggerDispenseSyringeParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].triggerDispenseSyringeParam.Gain = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].triggerDispenseSyringeParam.Offset = cmd0509.MotorOffset[1];
                    break;

                case (int)MotorNoList.PreTriggerDispenseSyringe:
                    //プレトリガ液ｼﾘﾝｼﾞ
                    motor.Param.SlaveList[slaveListIdx].preTriggerDispenseSyringeParam.MotorSpeed = cmd0509.MotorSpeed;
                    motor.Param.SlaveList[slaveListIdx].preTriggerDispenseSyringeParam.Gain = cmd0509.MotorOffset[0];
                    motor.Param.SlaveList[slaveListIdx].preTriggerDispenseSyringeParam.Offset = cmd0509.MotorOffset[1];
                    break;

            }
        }

        /// <summary>
        /// 受信した各モーターパラメータをセットします。
        /// </summary>
        public void SetUpMotorParam(RackTransferCommCommand_0109 cmd0109)
        {
            switch (cmd0109.MotorNo)
            {
                case (int)MotorNoList.RackTransferSendingXAxisM1:
                    //ラック搬送部送りX軸（モジュール１）
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM1Param.MotorSpeed = cmd0109.MotorSpeed;
                    break;
                case (int)MotorNoList.RackTransferBackXAxisM1:
                    //ラック搬送部戻りX軸（モジュール１）
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM1Param.MotorSpeed = cmd0109.MotorSpeed;
                    break;
                case (int)MotorNoList.RackPullinYAxisM1:
                    //ラック引込部Y軸（モジュール１）
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM1Param.MotorSpeed = cmd0109.MotorSpeed;
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM1Param.OffsetHomePosition = cmd0109.MotorOffset[0];
                    break;
                case (int)MotorNoList.RackTransferSendingXAxisM2:
                    //ラック搬送部送りX軸（モジュール２）
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM2Param.MotorSpeed = cmd0109.MotorSpeed;
                    break;
                case (int)MotorNoList.RackTransferBackXAxisM2:
                    //ラック搬送部戻りX軸（モジュール２）
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM2Param.MotorSpeed = cmd0109.MotorSpeed;
                    break;
                case (int)MotorNoList.RackPullinYAxisM2:
                    //ラック引込部Y軸（モジュール２）
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM2Param.MotorSpeed = cmd0109.MotorSpeed;
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM2Param.OffsetHomePosition = cmd0109.MotorOffset[0];
                    break;
                case (int)MotorNoList.RackTransferSendingXAxisM3:
                    //ラック搬送部送りX軸（モジュール３）
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM3Param.MotorSpeed = cmd0109.MotorSpeed;
                    break;
                case (int)MotorNoList.RackTransferBackXAxisM3:
                    //ラック搬送部戻りX軸（モジュール３）
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM3Param.MotorSpeed = cmd0109.MotorSpeed;
                    break;
                case (int)MotorNoList.RackPullinYAxisM3:
                    //ラック引込部Y軸（モジュール３）
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM3Param.MotorSpeed = cmd0109.MotorSpeed;
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM3Param.OffsetHomePosition = cmd0109.MotorOffset[0];
                    break;
                case (int)MotorNoList.RackTransferSendingXAxisM4:
                    //ラック搬送部送りX軸（モジュール４）
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferSendingXAxisM4Param.MotorSpeed = cmd0109.MotorSpeed;
                    break;
                case (int)MotorNoList.RackTransferBackXAxisM4:
                    //ラック搬送部戻りX軸（モジュール４）
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackTransferBackXAxisM4Param.MotorSpeed = cmd0109.MotorSpeed;
                    break;
                case (int)MotorNoList.RackPullinYAxisM4:
                    //ラック引込部Y軸（モジュール４）
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM4Param.MotorSpeed = cmd0109.MotorSpeed;
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackForkYAxisM4Param.OffsetHomePosition = cmd0109.MotorOffset[0];
                    break;
                case (int)MotorNoList.RackSetLoadYAxis:
                    //ラック架設部　ラック設置Y軸
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadYAxisParam.MotorSpeed = cmd0109.MotorSpeed;
                    break;
                case (int)MotorNoList.RackSetUnLoadYAxis:
                    //ラック架設部　再検ラック待機Y軸
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadYAxisParam.MotorSpeed = cmd0109.MotorSpeed;
                    break;
                case (int)MotorNoList.RackSetTakeOutYAxis:
                    //ラック架設部　ラック回収Y軸
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetTakeOutYAxisParam.MotorSpeed = cmd0109.MotorSpeed;
                    break;
                case (int)MotorNoList.RackSetLoadFeederXAxis:
                    //ラック架設部　ラックフィーダX軸
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.MotorSpeed = cmd0109.MotorSpeed;
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.OffsetRackTakeout = cmd0109.MotorOffset[0];
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.OffsetRackLoad = cmd0109.MotorOffset[1];
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.OffsetTubeSensorReading = cmd0109.MotorOffset[2];
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.OffsetSampleIDReading = cmd0109.MotorOffset[3];
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetLoadFeederXAxisParam.OffsetRackIDReading = cmd0109.MotorOffset[4];
                    break;
                case (int)MotorNoList.RackSetUnLoadFeederXAxis:
                    //ラック架設部　再検ラックフィーダX軸
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadFeederXAxisParam.MotorSpeed = cmd0109.MotorSpeed;
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadFeederXAxisParam.OffsetRackTakeout = cmd0109.MotorOffset[0];
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadFeederXAxisParam.OffsetRackRetest = cmd0109.MotorOffset[1];
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetUnLoadFeederXAxisParam.OffsetRackUnLoad = cmd0109.MotorOffset[2];
                    break;
                case (int)MotorNoList.RackSetSliderXAxis:
                    //ラック架設部　ラックスライダーX軸
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetSliderXAxisParam.MotorSpeed = cmd0109.MotorSpeed;
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetSliderXAxisParam.OffsetRackLoad = cmd0109.MotorOffset[0];
                    motor.Param.RackList[Singleton<CarisXCommManager>.Instance.SelectedRackTransferIndex].rackSetSliderXAxisParam.OffsetRackUnLoad = cmd0109.MotorOffset[1];
                    break;

            }
        }

        /// <summary>
        /// モーターパラメータをXMLに保存します。
        /// </summary>
        public void MotorparamSave()
        {
            //モーターパラメータ保存        
            motor.SaveRaw();
        }
    }
}
