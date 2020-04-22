using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Oelco.CarisX.Comm;
using Oelco.CarisX.Const;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Status;
using Oelco.CarisX.Utility;
using Oelco.CarisX.DB;
using Oelco.CarisX.Log;
using Oelco.Common.Log;

namespace Oelco.CarisX.Common
{
    /// <summary>
    /// ラック移動管理クラス
    /// </summary>
    class RackMoveManager
    {
        #region コンストラクタ/デストラクタ
        public RackMoveManager()
        {

        }
        #endregion

        #region public

        /// <summary>
        /// 停止位置取得処理
        /// </summary>
        /// <param name="command">ラック移動位置問合せコマンド</param>
        /// <returns>ラック停止位置</returns>
        public RackPositionKind GetRackMoveStopPosition( RackTransferCommCommand_0119 command)
        {
            String dbgMsgHead = String.Format("[[Investigation log]]RackMoveManager::{0} ", MethodBase.GetCurrentMethod().Name);
            String dbgMsg = dbgMsgHead + String.Format("{0} ", command.RackID);

            // ラック停止位置
            RackPositionKind rtnRackPos = RackPositionKind.Rack;

            // 分析予定存在フラグ
            Boolean isAssaySchedule = false;

            // 対象ラックの分析予定にアッセイ開始していないデータがあるか確認する
            isAssaySchedule = Singleton<RackInfoManager>.Instance.ExistsAssaySchedule(command.RackID);

            // 対象のラックに対する対象データが存在する場合
            if (isAssaySchedule)
            {
                //アルゴリズムに応じて、Assay対象のモジュールを設定
                rtnRackPos = this.getRackMoveStopPositionByRackMovementMethod(command);
                dbgMsg = dbgMsg + String.Format("Move to {0}", rtnRackPos);
            }
            // 対象のラックに対する対象データが存在しない場合
            else
            {
                // ラック開始位置がBCRの場合
                if (command.StartPosition == RackTransferCommCommand_0119.PositionKind.BCR)
                {
                    // 回収レーンに戻す
                    rtnRackPos = RackPositionKind.CollectRack;
                    dbgMsg = dbgMsg + String.Format("No target data, move to Collect lane");
                }
                // それ以外(モジュール1～4)の場合
                else
                {
                    // 待機レーンに戻す
                    rtnRackPos = RackPositionKind.Rack;
                    dbgMsg = dbgMsg + String.Format("No target data, move to waiting lane");
                }
            }

            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
            return rtnRackPos;
        }

        /// <summary>
        /// 分析予定の設定（モジュール）
        /// </summary>
        /// <param name="rackId">ラックID</param>
        /// <param name="stopPosition">ラック移動開始位置</param>
        /// <returns>モジュール間移動の有無 true:移動有り false:移動無し</returns>
        public Boolean SetModuleIdToAssaySchedule( String rackId, RackPositionKind stopPosition )
        {
            String dbgMsg = String.Format("[[Investigation log]]RackMoveManager::{0} ", MethodBase.GetCurrentMethod().Name);
            dbgMsg = dbgMsg + String.Format("{0} ", rackId);

            // モジュール間移動フラグ
            Boolean isMoveBetweenDevice = false;

            // ラック移動位置
            Int32 rackMovePosition = (Int32)stopPosition;

            //ラック情報の有無
            var rackInfoList = Singleton<RackInfoManager>.Instance.RackInfo.Where(x => x.RackId.DispPreCharString == rackId);

            // 接続台数
            Int32 numOfConneted = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;

            // ラック情報がない場合
            if (( rackInfoList == null ) || ( rackInfoList.Count() == 0 ))
            {
                // 以降の処理を行わない
                dbgMsg = dbgMsg + String.Format("No rack information");
                return isMoveBetweenDevice;
            }

            // ラック情報の先頭データを取得
            RackInfo rackInfo = rackInfoList.FirstOrDefault();

            //複数モジュールが接続されている場合、検索するモジュールの初期位置を調整する
            for (int i = 0; i < numOfConneted; i++)
            {
                //ラック移動位置が接続台数以上の場合、モジュール１の位置に戻す
                if (rackMovePosition > numOfConneted)
                {
                    rackMovePosition = (Int32)RackModuleIndex.Module1;
                }

                dbgMsg = dbgMsg + String.Format("Check Start {0} ", (RackModuleIndex)rackMovePosition);

                // 対象のモジュールの状態を取得
                SystemStatusKind systemStatus = Singleton<SystemStatus>.Instance.ModuleStatus [rackMovePosition];

                //対象のモジュールのモジュールステータスがAssay以外の場合は対象外にする
                if (systemStatus != SystemStatusKind.Assay)
                {
                    dbgMsg = dbgMsg + String.Format("Status is {0}, not assay. "
                       , Singleton<SystemStatus>.Instance.ModuleStatus[rackMovePosition]);

                    //次のモジュールをチェックする
                    rackMovePosition += 1;
                    continue;
                }

                // 分析予定追加用変数（コントロールで使用）
                List<AssaySchedule> addAssaySchedules;

                // ラックポジション数分調べる
                foreach (var rackDetail in rackInfo.RackPosition)
                {
                    // 分析予定追加用変数の初期化
                    addAssaySchedules = new List<AssaySchedule>();

                    // 各ポジションの分析予定を調べる
                    foreach (var assaySch in rackDetail.AssaySchedules.GroupBy(v => v.ProtoNo.ToString() + " " + v.ReagentLotNo)
                       .Select(v => v.OrderBy(vv => (Int32)vv.ModuleId).FirstOrDefault()))
                    {
                        // キャリブレータの場合
                        if (rackInfo.RackId.PreChar == CarisXConst.CALIB_RACK_ID_PRECHAR)
                        {
                            // 移動先モジュールが登録された分析モジュール番号リストに含まれているか確認
                            Boolean isTargetRackMovePosition = assaySch.PreModuleIdListForCalibrator.Contains((RackModuleIndex)rackMovePosition);
                            if (isTargetRackMovePosition == false)
                            {
                                // リストに含まれていない => 対象外

                                //次のスケジュールをチェックする
                                continue;
                            }
                        }

                        dbgMsg = dbgMsg + String.Format("Pos{0} ProtocolNo-ReagentLot:{1} ", rackDetail.PositionNo, assaySch.ProtoNo + "-" + assaySch.ReagentLotNo);

                        //試薬情報が存在するかチェック
                        String reagentLotNo = String.Empty;

                        // 試薬ロット番号が空の場合
                        if (assaySch.ReagentLotNo == String.Empty)
                        {
                            // 試薬コードを条件に取得した試薬ロットを取得
                            reagentLotNo = Singleton<ReagentDB>.Instance.GetNowReagentLotNo(assaySch.ProtoNo, moduleId: rackMovePosition);
                        }
                        // 試薬ロット番号が空ではない場合
                        else
                        {
                            // 試薬コードと試薬ロット番号から優先ロット番号を取得
                            reagentLotNo = Singleton<ReagentDB>.Instance.GetLotSpecificationNo(assaySch.ProtoNo, assaySch.ReagentLotNo, rackMovePosition);
                        }

                        // 試薬ロット番号が空ではない場合(試薬情報が存在する場合)
                        if (reagentLotNo != String.Empty)
                        {
                            // モジュールIDが未設定（＝0）の場合
                            if (assaySch.ModuleId == RackModuleIndex.RackTransfer)
                            {
                                // モジュールIDをラック移動位置に設定
                                assaySch.ModuleId = (RackModuleIndex)rackMovePosition;

                                dbgMsg = dbgMsg + String.Format("assay schedule set ");

                                RackMovementMethodKind rackMovementMethod = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackMovementMethodParameter.RackMovementMethod;

                                // パフォーマンスモード以外の場合
                                if (rackMovementMethod != RackMovementMethodKind.Performance)
                                {
                                    //前回移動先に移動先を設定する
                                    if (!Singleton<PublicMemory>.Instance.lastRackMovePosition.Keys.Contains(assaySch.ProtoNo))
                                    {
                                        Singleton<PublicMemory>.Instance.lastRackMovePosition.Add(assaySch.ProtoNo, rackMovePosition);
                                    }
                                }
                            }
                            // モジュールIDが設定されている場合
                            else
                            {
                                // キャリブレータまたは精度管理検体の場合
                                if ((rackInfo.RackId.PreChar == CarisXConst.CALIB_RACK_ID_PRECHAR)
                                 || (rackInfo.RackId.PreChar == CarisXConst.CONTROL_RACK_ID_PRECHAR))
                                {
                                    AssaySchedule addSch = new AssaySchedule();
                                    addSch.ProtoNo = assaySch.ProtoNo;
                                    addSch.ReagentLotNo = assaySch.ReagentLotNo;
                                    addSch.ModuleId = (RackModuleIndex)rackMovePosition;
                                    addSch.RepCount = assaySch.RepCount;
                                    addSch.AssayStarted = false;
                                    addSch.PreModuleIdListForCalibrator = assaySch.PreModuleIdListForCalibrator;
                                    addAssaySchedules.Add(addSch);

                                    dbgMsg = dbgMsg + String.Format("assay schedule add ");
                                }
                                else
                                {
                                    dbgMsg = dbgMsg + String.Format("assay schedule already set ");
                                }
                            }

                            // ラック移動先以外に分析予定がある場合(モジュール間移動がある場合)
                            if ((Int32)stopPosition != rackMovePosition)
                            {
                                // モジュール間移動フラグをtrueにする
                                isMoveBetweenDevice = true;

                                dbgMsg = dbgMsg + String.Format(" MoveBetweenDevice = true ");
                            }
                        }
                        else
                        {
                            dbgMsg = dbgMsg + String.Format("No reagent data ");
                        }
                    }

                    // 分析予定追加がある場合
                    if (addAssaySchedules.Count != 0)
                    {
                        // 分析予定を追加する
                        rackDetail.AssaySchedules.AddRange(addAssaySchedules);
                    }
                }

                rackMovePosition += 1;
            }

            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
            return isMoveBetweenDevice;
        }

        /// <summary>
        /// モジュール間移動確認
        /// </summary>
        /// <param name="rackId"></param>
        /// <param name="stopPosition"></param>
        /// <returns></returns>
        public Boolean IsMoveBetweenDevice( String rackId, RackTransferCommCommand_0119.PositionKind strtPosition, RackPositionKind stopPosition )
        {
            String dbgMsg = String.Format("[[Investigation log]]RackMoveManager::{0} ", MethodBase.GetCurrentMethod().Name);
            dbgMsg = dbgMsg + String.Format("{0} ", rackId);

            // モジュール間移動フラグ
            Boolean isMoveBetweenDevice = false;

            // ラック移動位置
            Int32 rackMovePosition = (Int32)stopPosition;

            //ラック情報の有無
            var rackInfoList = Singleton<RackInfoManager>.Instance.RackInfo.Where(x => x.RackId.DispPreCharString == rackId);

            // 接続台数
            Int32 numOfConneted = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;

            // ラック情報がない場合
            if (( rackInfoList == null ) || ( rackInfoList.Count() == 0 ))
            {
                // 以降の処理を行わない
                dbgMsg = dbgMsg + String.Format("No rack information");
                return isMoveBetweenDevice;
            }

            // ラック情報の先頭データを取得
            RackInfo rackInfo = rackInfoList.FirstOrDefault();

            //複数モジュールが接続されている場合、検索するモジュールの初期位置を調整する
            for (int i = 0; i < numOfConneted; i++)
            {
                //ラック移動位置が接続台数以上の場合、モジュール１の位置に戻す
                if (rackMovePosition > numOfConneted)
                {
                    rackMovePosition = (Int32)RackModuleIndex.Module1;
                }

                dbgMsg = dbgMsg + String.Format("Check Start {0} ", (RackModuleIndex)rackMovePosition);

                // 対象のモジュールの状態を取得
                SystemStatusKind systemStatus = Singleton<SystemStatus>.Instance.ModuleStatus[rackMovePosition];

                //対象のモジュールのモジュールステータスがAssay以外の場合は対象外にする
                if (systemStatus != SystemStatusKind.Assay)
                {
                    dbgMsg = dbgMsg + String.Format("Status is {0}, not assay. "
                       , Singleton<SystemStatus>.Instance.ModuleStatus[rackMovePosition]);

                    //次のモジュールをチェックする
                    rackMovePosition += 1;
                    continue;
                }

                // ラックポジション数分調べる
                foreach (var rackDetail in rackInfo.RackPosition)
                {
                    // 各ポジションの分析予定を調べる
                    foreach (var assaySch in rackDetail.AssaySchedules.GroupBy(v => v.ProtoNo.ToString() + " " + v.ReagentLotNo)
                       .Select(v => v.OrderBy(vv => (Int32)vv.ModuleId).FirstOrDefault()))
                    {
                        // キャリブレータの場合
                        if (rackInfo.RackId.PreChar == CarisXConst.CALIB_RACK_ID_PRECHAR)
                        {
                            // 移動先モジュールが登録された分析モジュール番号リストに含まれているか確認
                            Boolean isTargetRackMovePosition = assaySch.PreModuleIdListForCalibrator.Contains((RackModuleIndex)rackMovePosition);
                            if (isTargetRackMovePosition == false)
                            {
                                // リストに含まれていない => 対象外

                                //次のスケジュールをチェックする
                                continue;
                            }
                        }

                        dbgMsg = dbgMsg + String.Format("Pos{0} ProtocolNo-ReagentLot:{1} ", rackDetail.PositionNo, assaySch.ProtoNo + "-" + assaySch.ReagentLotNo);

                        //試薬情報が存在するかチェック
                        String reagentLotNo = String.Empty;

                        // 試薬ロット番号が空の場合
                        if (assaySch.ReagentLotNo == String.Empty)
                        {
                            // 試薬コードを条件に取得した試薬ロットを取得
                            reagentLotNo = Singleton<ReagentDB>.Instance.GetNowReagentLotNo(assaySch.ProtoNo, moduleId: rackMovePosition);
                        }
                        // 試薬ロット番号が空ではない場合
                        else
                        {
                            // 試薬コードと試薬ロット番号から優先ロット番号を取得
                            reagentLotNo = Singleton<ReagentDB>.Instance.GetLotSpecificationNo(assaySch.ProtoNo, assaySch.ReagentLotNo, rackMovePosition);
                        }

                        // 試薬ロット番号が空ではない場合(試薬情報が存在する場合)
                        if (reagentLotNo != String.Empty)
                        {
                            // モジュールごとの分析開始済フラグを取得
                            Boolean assayStarted = rackDetail.AssaySchedules.Where(v => (Int32)v.ModuleId == rackMovePosition).Select(v => v.AssayStarted).FirstOrDefault();

                            // 分析開始済フラグが分析前かつ、ラック移動先以外に分析予定がある場合(モジュール間移動がある場合)
                            if (( assayStarted == false) && ((Int32)stopPosition != rackMovePosition))
                            {
    
                                isMoveBetweenDevice = true;

                                dbgMsg = dbgMsg + String.Format(" MoveBetweenDevice = true ");
                            }
                        }
                        else
                        {
                            dbgMsg = dbgMsg + String.Format("No reagent data ");
                        }
                    }
                }

                rackMovePosition += 1;
            }

            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
            return isMoveBetweenDevice;
        }
        #endregion

        #region private

        /// <summary>
        /// ラック移動方式による停止位置取得処理
        /// </summary>
        /// <param name="command">ラック移動位置問合せコマンド</param>
        /// <returns>ラック停止位置</returns>
        private RackPositionKind getRackMoveStopPositionByRackMovementMethod( RackTransferCommCommand_0119 command )
        {
            String dbgMsg = String.Format("[[Investigation log]]RackMoveManager::{0} ", MethodBase.GetCurrentMethod().Name);
            dbgMsg = dbgMsg + String.Format("{0} ", command.RackID);

            // ラック停止位置
            RackPositionKind rtnRackPos = RackPositionKind.Rack;

            // 接続台数
            Int32 numOfConneted = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;

            // ラック移動方式の設定
            RackMovementMethodKind rackMovementMethod = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackMovementMethodParameter.RackMovementMethod;

            // ラック移動位置
            Int32 RackMovePosition = (Int32)RackModuleIndex.Module1;

            // モジュール1～4への移動可能フラグ
            Boolean isRackMove = false;

            // 移動可能モジュールリスト
            List<Int32> moveModule = new List<Int32>();

            // ラック開始位置
            RackTransferCommCommand_0119.PositionKind commStartPos = command.StartPosition;

            // 複数モジュールが接続されている場合
            if (numOfConneted > 1)
            {
                // 検索するモジュールの初期位置を調整する

                // BCRから移動する場合
                if (commStartPos == RackTransferCommCommand_0119.PositionKind.BCR)
                {
                    switch (rackMovementMethod)
                    {
                        // パフォーマンスモードの場合
                        case RackMovementMethodKind.Performance:

                            // 前回移動先の次のモジュールから調べる
                            RackMovePosition = Singleton<PublicMemory>.Instance.lastRackMovePosition [0] + 1;
                            break;

                        // それ以外の場合
                        default:

                            // 分析予定から最小の分析項目番号を取得する
                            Int32 ProtNo = Singleton<RackInfoManager>.Instance.GetMinimumProtoNoInAssaySch(command.RackID);

                            // 分析項目番号が最後に測定したモジュールがあるかのフラグ
                            bool isLastMeasureModule = Singleton<PublicMemory>.Instance.lastRackMovePosition.Keys.Contains(ProtNo);

                            // 最後に測定したモジュールがある場合
                            if (isLastMeasureModule)
                            {
                                // 最後に測定したモジュールから調べる
                                RackMovePosition = Singleton<PublicMemory>.Instance.lastRackMovePosition [ProtNo];
                            }
                            // 最後に測定したモジュールがない場合
                            else
                            {
                                //分析項目が初めての場合はモジュール１から調べる
                                RackMovePosition = (Int32)RackModuleIndex.Module1;
                            }
                            break;
                    }
                }
                // それ以外(モジュール1～4)の場合
                else
                {
                    //今のモジュールの次のモジュールを次の停止位置と仮定して処理する
                    RackMovePosition = (Int32)commStartPos + 1;
                }
            }

            Int32 errArg = 0;

            // 接続台数分探す
            for (int i = 0; i < numOfConneted; i++)
            {
                isRackMove = false;

                //ラック移動位置が接続台数以上の場合、モジュール１の位置に戻す
                if (RackMovePosition > numOfConneted)
                {
                    RackMovePosition = (Int32)RackModuleIndex.Module1;
                }

                dbgMsg = dbgMsg + String.Format("Check Start {0} ", (RackModuleIndex)RackMovePosition);

                // 対象のモジュールの状態を取得
                SystemStatusKind systemStatus = Singleton<SystemStatus>.Instance.ModuleStatus [RackMovePosition];

                // 対象のモジュールがAssay以外の場合
                if (systemStatus != SystemStatusKind.Assay)
                {
                    dbgMsg = dbgMsg + String.Format("Status is {0}, not assay. "
                       , Singleton<SystemStatus>.Instance.ModuleStatus[RackMovePosition]);

                    //次のモジュールをチェックする
                    RackMovePosition += 1;

                    // 分析可能モジュールなしエラー
                    errArg = (Int32)ErrorCollectKind.AssayPlanError;

                    continue;
                }

                List<AssaySchedule> assaySchedules = new List<AssaySchedule>();

                // 開始位置がBCR以外(モジュール1～4)の場合かつ、ラックが精度管理の場合
                if (( commStartPos != RackTransferCommCommand_0119.PositionKind.BCR )
                    && ( ( (CarisXIDString)command.RackID ).PreChar == CarisXConst.CONTROL_RACK_ID_PRECHAR ))
                {
                    // 分析予定を取得
                    assaySchedules = Singleton<RackInfoManager>.Instance.GetAssaySchedules(command.RackID, RackMovePosition);

                    // 分析予定が存在しない場合
                    if (assaySchedules.Count == 0)
                    {
                        dbgMsg = dbgMsg + String.Format("No assay schedule. "
                            , Singleton<SystemStatus>.Instance.ModuleStatus[RackMovePosition]);

                        //次のモジュールをチェックする
                        RackMovePosition += 1;
                        continue;
                    }
                }
                else
                {
                    // 分析予定の取得
                    assaySchedules = Singleton<RackInfoManager>.Instance.GetAssaySchedules(command.RackID);
                }

                // 分析予定を捜索
                foreach (AssaySchedule item in assaySchedules)
                {
                    // キャリブレータの場合
                    if (((CarisXIDString)command.RackID).PreChar == CarisXConst.CALIB_RACK_ID_PRECHAR)
                    {
                        // 移動先モジュールが登録分析モジュール番号リストに含まれているか確認
                        Boolean isTargetRackMovePosition = item.PreModuleIdListForCalibrator.Contains((RackModuleIndex)RackMovePosition);
                        if(isTargetRackMovePosition == false)
                        {
                            // リストに含まれていない => 対象外

                            //次のスケジュールをチェックする
                            continue;
                        }
                    }

                    String reagentLotNo = String.Empty;

                    // 試薬ロット番号が空の場合
                    if (item.ReagentLotNo == String.Empty)
                    {
                        // 試薬コードを条件に取得した試薬ロットを取得
                        reagentLotNo = Singleton<ReagentDB>.Instance.GetNowReagentLotNo(item.ProtoNo, moduleId: RackMovePosition);
                    }
                    // 試薬ロット番号が空ではない場合
                    else
                    {
                        // 試薬コードと試薬ロット番号から優先ロット番号を取得
                        reagentLotNo = Singleton<ReagentDB>.Instance.GetLotSpecificationNo(item.ProtoNo, item.ReagentLotNo, RackMovePosition);
                    }

                    // 試薬ロット番号が取得できた場合
                    if (reagentLotNo != String.Empty)
                    {
                        // モジュール移動可能フラグをtrueにする
                        isRackMove = true;
                        break;
                    }
                    else
                    {
                        // 試薬情報無しエラー
                        errArg = (Int32)ErrorCollectKind.ReagentError;
                        dbgMsg = dbgMsg + String.Format("[90-2]Insufficient reagent or no reagent information.");
                    }
                }

                // モジュール移動可能フラグがfalseの時
                if (isRackMove == false)
                {
                    dbgMsg = dbgMsg + String.Format("No reagent data. ");

                    //次のモジュールをチェックする
                    RackMovePosition += 1;
                    continue;
                }

                //該当モジュールに移動可能なのでリストに追加
                moveModule.Add(RackMovePosition);

                dbgMsg = dbgMsg + String.Format("OK ");

                // ラック移動方式の設定がパフォーマンスモード以外の場合
                if (rackMovementMethod != RackMovementMethodKind.Performance)
                {
                    break;
                }

                //次のループを行う為にインクリメントする
                RackMovePosition += 1;
            }

            //ラック移動先があった場合の処理
            if (moveModule.Count != 0)
            {
                // ラック移動方式の設定がパフォーマンスモードの場合
                if (rackMovementMethod == RackMovementMethodKind.Performance)
                {
                    //移動可能なモジュールの内、どのモジュールに移動するかを決定する
                    RackMovePosition = this.decideWhichModulesToMove(moveModule);

                    // ラック開始位置がBCRの場合
                    if (commStartPos == RackTransferCommCommand_0119.PositionKind.BCR)
                    {
                        //前回移動先に移動先を設定する
                        Singleton<PublicMemory>.Instance.lastRackMovePosition [0] = RackMovePosition;
                    }
                }
            }

            // 移動先モジュールが存在する場合
            if (moveModule.Count != 0)
            {
                //モジュール１～４へのラック移動がある場合
                rtnRackPos = (RackPositionKind)RackMovePosition;
            }
            // 移動先モジュールが存在しない場合
            else
            {
                // ラック開始位置がBCRの場合
                if (commStartPos == RackTransferCommCommand_0119.PositionKind.BCR)
                {
                    // 回収レーンに戻す
                    rtnRackPos = RackPositionKind.CollectRack;
                }
                // それ以外(モジュール1～4)の場合
                else
                {
                    // 待機レーンに戻す
                    rtnRackPos = RackPositionKind.Rack;
                }
            }

            // 回収レーンに戻る場合
            if (( rtnRackPos == RackPositionKind.CollectRack ) && ( errArg != (Int32)ErrorCollectKind.NoError ))
            {
                // エラー付与文字列の生成 " RackID:XXXX"
                String errStr = Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_000 + Oelco.CarisX.Properties.Resources.STRING_COMMON_019 + command.RackID;

                // エラー履歴に登録
                CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.RackTransferError, errArg, errStr);
            }

            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);

            return rtnRackPos;
        }

        /// <summary>
        /// 移動するモジュールを決定する。
        /// </summary>
        /// <remarks>
        /// STATで待機中のデータが同一モジュールで複数存在する事はない前提。（検体ID１のSTATと検体ID２のSTATがどちらもStandByはありえない）
        /// </remarks>
        /// <param name="canMoveModules">移動可能なモジュールのリスト</param>
        /// <returns>移動先モジュール番号</returns>
        private Int32 decideWhichModulesToMove( List<Int32> moveModule )
        {
            String dbgMsg = String.Format("[[Investigation log]]RackMoveManager::{0} ", MethodBase.GetCurrentMethod().Name);

            // 移動先モジュール番号
            Int32 rtnRackPos = (Int32)RackModuleIndex.Module1;

            // 移動先モジュールのフォーク空きフラグ
            Boolean rtnRackPosFixed = false;

            //モジュールに引込しているモジュール別ラックリスト
            Dictionary<Int32, List<String>> rackInModuleList = new Dictionary<Int32, List<String>>();

            // モジュール別待機件数リスト
            Dictionary<Int32, Int32> moduleWaitCount = new Dictionary<Int32, Int32>();

            // 移動可能なモジュールを昇順に並び替え
            moveModule = moveModule.OrderBy(v => v).ToList();

            dbgMsg = dbgMsg + String.Format("get RackID List. ");

            // モジュールに引込しているラックのリストを作成しておく
            foreach (var moduleNo in moveModule)
            {
                dbgMsg = dbgMsg + String.Format("moduleNo={0} ", moduleNo);
                rackInModuleList.Add(moduleNo, new List<string>());
                var rackIDs = Singleton<RackPositionManager>.Instance.RackPosition.Where(v => v.Value == (RackPositionKind)moduleNo);
                if (rackIDs.Count() != 0)
                {
                    rackInModuleList[moduleNo] = rackIDs.Select(v => v.Key).ToList();
                }
                dbgMsg = dbgMsg + String.Format("rackInModuleList={0} ", String.Join(",", rackInModuleList[moduleNo]));
            }

            //各分析中データを取得してまとめる
            var specimenAssay = Singleton<SpecimenAssayDB>.Instance.GetData()
                .Select(v => new AssayDataStructure(v.ModuleNo, v.RackId, v.GetStatus()));
            var CalibAssay = Singleton<CalibratorAssayDB>.Instance.GetData()
                .Select(v => new AssayDataStructure(v.ModuleNo, v.RackId, v.GetStatus()));
            var ControlAssay = Singleton<ControlAssayDB>.Instance.GetData()
                .Select(v => new AssayDataStructure(v.ModuleNo, v.RackId, v.GetStatus()));
            var allAssay = specimenAssay.Concat(CalibAssay.Concat(ControlAssay));

            //まとめた分析中データから待機中のデータを取得する
            List<AssayDataStructure> standbyData = allAssay.Where(v => v.Status == SampleInfo.SampleMeasureStatus.Wait).ToList();

            if (standbyData != null)
            {
                // モジュールとラックIDごとにまとめる
                var temp = standbyData.GroupBy(v => new { ModuleNo = v.ModuleNo, RackID = v.RackID.DispPreCharString })
                    .Select(d => new { ModuleNo = d.Key.ModuleNo, RackID = d.Key.RackID, NumRemainingCycles = d.Count() }).ToList();

                // まとめた待機中分析中データに残サイクル数を設定
                standbyData = temp.Select(v => new AssayDataStructure(v.ModuleNo, v.RackID, SampleInfo.SampleMeasureStatus.Wait, v.NumRemainingCycles)).ToList();

                // まとめた分析中データに,501コマンドで与えられるラックが存在すれば置き換える
                foreach (var moduleNo in moveModule)
                {

                    List<AssayDataStructure> tempRackPullBackStatus = standbyData.Where(v => Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[( moduleNo - 1 )].RackPullBackStatus.RackID.Contains(v.RackID.DispPreCharString)
                        && (( Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[( moduleNo - 1 )].RackPullBackStatus.RackStatus == RackStatus.InProcess )
                        || ( Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[( moduleNo - 1 )].RackPullBackStatus.RackStatus == RackStatus.Completed )))
                        .Select(v => new AssayDataStructure(v.ModuleNo, v.RackID, SampleInfo.SampleMeasureStatus.Wait, Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[( moduleNo - 1 )].RackPullBackStatus.NumRemainingCycles)).ToList();

                    List<AssayDataStructure> tempRackPullFrontStatus = standbyData.Where(v => Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[( moduleNo - 1 )].RackPullFrontStatus.RackID.Contains(v.RackID.DispPreCharString)
                        && ( ( Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[( moduleNo - 1 )].RackPullFrontStatus.RackStatus == RackStatus.InProcess )
                        || ( Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[( moduleNo - 1 )].RackPullBackStatus.RackStatus == RackStatus.Completed ) ))
                        .Select(v => new AssayDataStructure(v.ModuleNo, v.RackID, SampleInfo.SampleMeasureStatus.Wait, Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[( moduleNo - 1 )].RackPullFrontStatus.NumRemainingCycles)).ToList();

                    if (tempRackPullBackStatus.Count > 0)
                    {
                        standbyData.RemoveAll(v => Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[( moduleNo - 1 )].RackPullBackStatus.RackID.Contains(v.RackID.DispPreCharString));
                        standbyData = standbyData.Concat(tempRackPullBackStatus).ToList();
                    }

                    if (tempRackPullFrontStatus.Count > 0)
                    {
                        standbyData.RemoveAll(v => Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[( moduleNo - 1 )].RackPullFrontStatus.RackID.Contains(v.RackID.DispPreCharString));
                        standbyData = standbyData.Concat(tempRackPullFrontStatus).ToList();
                    }
                }
            }

            dbgMsg = dbgMsg + String.Format("get standbycount. ");

            //判定用の待機件数を取得
            foreach (Int32 moduleNo in moveModule)
            {
                dbgMsg = dbgMsg + String.Format("moduleNo={0} ", moduleNo);

                Int32 standbyCount = 0;
                Int32 standbySTATCount = 0;
                String targetRackID = String.Empty;

                Int32 firstOrder = Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[( moduleNo - 1 )].TurnOrder[0];

                //STATの件数を取得する（引き込まれている場合のみ）
                if (firstOrder == 3)
                {
                    standbySTATCount = Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[( moduleNo - 1 )].STATStatus.NumRemainingCycles;
                }

                dbgMsg = dbgMsg + String.Format("standbySTATCount={0} ", standbySTATCount);

                //処理中のラックが存在した場合、該当ラックの待機件数を取得する
                if (firstOrder != 0)
                {
                    switch (firstOrder)
                    {
                        case 1:
                            standbyCount = Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[( moduleNo - 1 )].RackPullBackStatus.NumRemainingCycles;
                            targetRackID = Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[( moduleNo - 1 )].RackPullBackStatus.RackID;
                            break;
                        case 2:
                            standbyCount = Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[( moduleNo - 1 )].RackPullFrontStatus.NumRemainingCycles;
                            targetRackID = Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[( moduleNo - 1 )].RackPullFrontStatus.RackID;
                            break;
                        case 3:
                            standbyCount = standbyData.Where(v => v.ModuleNo == moduleNo).Sum(v => v.NumRemainingCycles);
                            break;
                    }

                    //分析予定から処理中のラックの処理予定のデータを取得する（処理数は繰返し回数の合算）　※対象のラックがワークシート問合せ中の場合を考慮
                    var targetRackAssayScheduleProcCount = Singleton<RackInfoManager>.Instance.GetAssaySchedules(targetRackID)
                        .Where(v => (Int32)v.ModuleId == moduleNo).Sum(v => v.RepCount);
                    standbyCount = standbyCount + targetRackAssayScheduleProcCount;
                }
                else
                {
                    dbgMsg = dbgMsg + String.Format(" not processing Rack ");

                    // 処理中のラックが存在しなかった場合、該当モジュールの全待機件数を取得する
                    standbyCount = standbyData.Where(v => v.ModuleNo == moduleNo && rackInModuleList[moduleNo].Contains(v.RackID.DispPreCharString)).Sum(v => v.NumRemainingCycles);

                    //分析予定から該当モジュールで処理予定のデータを取得する（処理数は繰返し回数の合算）
                    var assayScheduleProcCount = Singleton<RackInfoManager>.Instance.GetAssaySchedules(String.Empty)
                        .Where(v => (Int32)v.ModuleId == moduleNo).Sum(v => v.RepCount);

                    // モジュール吸引リストから自分以外のモジュールで吸引されているラックを取得
                    var allMoudulerackInModuleList = rackInModuleList.Where(v => v.Key != moduleNo).ToList();

                    // 吸引されたラックIDをひとつのリストにする
                    var allRaclID = allMoudulerackInModuleList.SelectMany(v => v.Value.Select(s => s)).ToList();

                    Int32 assayScheduleProcrackInModuleCount = 0;

                    // 該当モジュール以外で吸引されているラックから該当モジュールで処理予定のデータを取得する
                    foreach (var rackID in allRaclID)
                    {
                        assayScheduleProcrackInModuleCount += Singleton<RackInfoManager>.Instance.GetAssaySchedules(rackID.ToString())
                        .Where(v => (Int32)v.ModuleId == moduleNo).Sum(v => v.RepCount);
                    }

                    dbgMsg = dbgMsg + String.Format(" all module assay Schedule={0}, my module assay Schedule={1} ", assayScheduleProcCount, assayScheduleProcrackInModuleCount);

                    // 該当モジュールで処理予定のデータからラックで吸引されている分析予定データを引く
                    assayScheduleProcCount = assayScheduleProcCount - assayScheduleProcrackInModuleCount;

                    dbgMsg = dbgMsg + String.Format(" assay schedule procCount ={0} ", assayScheduleProcCount );

                    standbyCount = standbyCount + assayScheduleProcCount;
                }

                //STATがいる場合STATが優先されるので、STATの待機件数を加味しておく
                standbyCount = standbyCount + standbySTATCount;

                //モジュール毎に待機件数を保持する
                moduleWaitCount.Add(moduleNo, standbyCount);

                dbgMsg = dbgMsg + String.Format("standbyCount={0} ", standbyCount);
            }


            //待機件数が少ないモジュール順にソート（待機件数が同じ場合はモジュール番号の小さい方）
            var sortedModuleWaitCount = moduleWaitCount.OrderBy(v => v.Value).ThenBy(v => v.Key).Select(v => v.Key).ToList();

            dbgMsg = dbgMsg + String.Format("check fork. ");
            //モジュール順にフォークが空いてるモジュールを調べる
            foreach (var moduleNo in sortedModuleWaitCount)
            {
                dbgMsg = dbgMsg + String.Format("moduleNo={0} ", moduleNo);

                //該当モジュールでラックを２つ以上処理していないかどうか。（モジュール別ラックリストにはSTATも含まれるので、ラックは除外する）
                if (( Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[( moduleNo - 1 )].RackPullBackStatus.RackStatus != RackStatus.Empty )
                    && ( Singleton<RackSettingStatusManagerList>.Instance.rackSettingStatusManagersList[( moduleNo - 1 )].RackPullFrontStatus.RackStatus != RackStatus.Empty ))
                {
                    dbgMsg = dbgMsg + String.Format("no space on the fork. ", moduleNo);
                    //２つ以上処理しているので、別のモジュールをチェックする
                    continue;
                }

                //該当モジュールで処理可能
                rtnRackPos = moduleNo;
                rtnRackPosFixed = true;
                dbgMsg = dbgMsg + String.Format("can move. ");
                break;
            }

            // フォークに空きがない場合
            if (!rtnRackPosFixed)
            {
                dbgMsg = dbgMsg + String.Format("find free fork first. ");

                //モジュール順に残件数を調べる
                foreach (var moduleNo in sortedModuleWaitCount)
                {
                    dbgMsg = dbgMsg + String.Format("moduleNo={0} ", moduleNo);

                    //分析予定から該当モジュールで処理予定のデータを取得する
                    var assayScheduleList = Singleton<RackInfoManager>.Instance.GetAssaySchedules(String.Empty).Where(v => (Int32)v.ModuleId == moduleNo).ToList();
                    if (assayScheduleList != null)
                    {
                        // 繰返し回数の合算した処理数を取得
                        var assayScheduleProcCount = assayScheduleList.Sum(v => v.RepCount);
                        if (assayScheduleProcCount != 0)
                        {
                            dbgMsg = dbgMsg + String.Format("assay schedule found overwrite before waitCount={0} after ", moduleWaitCount[moduleNo]);

                            //分析予定が存在する場合は待機中の件数を全待機数にする
                            moduleWaitCount[moduleNo] = standbyData.Where(v => v.ModuleNo == moduleNo && rackInModuleList[moduleNo].Contains(v.RackID.DispPreCharString)).Sum(v => v.NumRemainingCycles);
                        }
                    }
                    dbgMsg = dbgMsg + String.Format("waitCount={0} ", moduleWaitCount[moduleNo]);
                }

                //一番待機中の件数が少ないモジュール番号を返す（件数が同数の場合はモジュール番号の大きい（＝装置が遠い）もの）
                rtnRackPos = moduleWaitCount.OrderBy(v => v.Value).ThenByDescending(v => v.Key).FirstOrDefault().Key;
                rtnRackPosFixed = true;
            }

            dbgMsg = dbgMsg + String.Format("rtnRackPos=>{0} ", rtnRackPos);
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);

            return rtnRackPos;
        }
       
        #endregion
    }

    /// <summary>
    /// 各分析データ
    /// </summary>
    class AssayDataStructure
    {
        /// <summary>
        /// モジュール番号
        /// </summary>
        public Int32 ModuleNo = 0;

        /// <summary>
        /// ラックID
        /// </summary>
        public CarisXIDString RackID = null;

        /// <summary>
        /// 分析項目ステータス
        /// </summary>
        public SampleInfo.SampleMeasureStatus Status = SampleInfo.SampleMeasureStatus.Wait;

        /// <summary>
        /// 残サイクル数
        /// </summary>
        public Int32 NumRemainingCycles = 0;

        public AssayDataStructure( Int32 moduleNo, CarisXIDString rackID, SampleInfo.SampleMeasureStatus status, Int32 numRemainingCycles = 1)
        {
            this.ModuleNo = moduleNo;
            this.RackID = rackID;
            this.Status = status;
            this.NumRemainingCycles = numRemainingCycles;
        }
    }

    
}
