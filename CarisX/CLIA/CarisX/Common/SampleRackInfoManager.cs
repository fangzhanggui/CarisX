using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.CarisX.Utility;
using Oelco.Common.Parameter;

using Oelco.CarisX.Const;
using Oelco.Common.Utility;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.CarisX.Parameter;
using System.Reflection;

namespace Oelco.CarisX.Common
{
    /// <summary>
    /// 設置状況変化情報クラス
    /// </summary>
    public class LaneContentChangeInfo
    {

        #region [定数定義]

        /// <summary>
        /// ステータス
        /// </summary>
        public enum ChangingStatus
        {
            /// <summary>
            /// 追加
            /// </summary>
            Add,
            /// <summary>
            /// 削除
            /// </summary>
            Remove
        }
        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// ステータス
        /// </summary>
        private ChangingStatus status;

        /// <summary>
        /// ラックID
        /// </summary>
        private CarisXIDString rackId;
        
        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="status">ステータス</param>
        /// <param name="rackId">ラックＩＤ</param>
        public LaneContentChangeInfo( ChangingStatus status, CarisXIDString rackId )
        {
            this.status = status;
            this.rackId = rackId;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ステータスの取得
        /// </summary>
        public ChangingStatus Status
        {
            get
            {
                return status;
            }
        }

        /// <summary>
        /// ラックIDの取得
        /// </summary>
        public CarisXIDString RackId
        {
            get
            {
                return this.rackId;
            }
        }
        #endregion

    }

    /// <summary>
    /// ラック情報詳細
    /// </summary>
    /// <remarks>
    /// ラックの内、ポジション毎に保持している情報を送信
    /// </remarks>
    public class RackInfoDetail
    {
        /// <summary>
        /// ポジション（１～５）
        /// </summary>
        public Int32 PositionNo { get; set; } = 0;
        /// <summary>
        /// 検体ID（１～５）
        /// </summary>
        public String SampleID { get; set; } = "";
        /// <summary>
        /// カップ種別
        /// </summary>
        public SpecimenCupKind CupKind { get; set; } = SpecimenCupKind.None;

        /// <summary>
        /// 分析予定
        /// </summary>
        public List<AssaySchedule> AssaySchedules { get; set; } = new List<AssaySchedule>();

        /// <summary>
        /// キャリブレータ自動登録用ラック移動先指定
        /// </summary>
        public List<RackModuleIndex> RegisteredModulesForAutoCalib { get; set; } = new List<RackModuleIndex>();

        /// <summary>
        /// 分析予定の分析項目設定
        /// </summary>
        /// <remarks>
        /// 分析予定を分析項目設定します。
        /// </remarks>
        public Boolean SetMeasItemInAssaySchedule(CarisXIDString rackId, MeasItem[] measArray, List<RackModuleIndex> preAssayModuleIdListForCalibrator)
        {
            Boolean rtnVal = false;
            String dbgMsg = String.Empty;

            //念のため、スケジュールを削除する
            AssaySchedules.Clear();

            foreach (var meas in measArray)
            {
                switch (rackId.PreChar)
                {
                    case CarisXConst.CONTROL_RACK_ID_PRECHAR:
                        //コントロールの場合
                        //分析予定はすべて登録する
                        break;
                    default:
                        //一般検体、またはキャリブレータの場合
                        //同一ポジション、プロトコルで既に分析予定が存在する場合は登録しない
                        if (AssaySchedules.Where(v => v.ProtoNo == meas.ProtoNo && v.ReagentLotNo == meas.ReagentLotNo).Count() != 0)
                        {
                            dbgMsg = String.Format("[[Investigation log]] ProtocolNo-LotNo:{0} Assay schedule already exists"
                                , meas.ProtoNo.ToString() + "-" + meas.ReagentLotNo);
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);

                            continue;
                        }
                        break;
                }

                AssaySchedule assaySchedule = new AssaySchedule();
                assaySchedule.ModuleId = RackModuleIndex.RackTransfer;  //データを作成する為に設定
                assaySchedule.ProtoNo = meas.ProtoNo;
                assaySchedule.ReagentLotNo = meas.ReagentLotNo;
                assaySchedule.RepCount = meas.RepCount;
                assaySchedule.AssayStarted = false;
                assaySchedule.PreModuleIdListForCalibrator = preAssayModuleIdListForCalibrator;
                AssaySchedules.Add(assaySchedule);

                rtnVal = true;
            }

            return rtnVal;
        }
    }

    /// <summary>
    /// 分析予定クラス
    /// </summary>
    /// <remarks>
    /// 対象のラックID-ポジションで行う分析予定を保持
    /// 分析開始済フラグは測定情報問合せを受信した際に更新する
    /// </remarks>
    public class AssaySchedule
    {
        /// <summary>
        /// モジュールID
        /// </summary>
        public RackModuleIndex ModuleId { get; set; } = RackModuleIndex.Module1;

        /// <summary>
        /// プロトコル番号（分析項目番号）
        /// </summary>
        /// <remarks>
        /// MeasItemクラスと合わせている
        /// </remarks>
        public Int32 ProtoNo { get; set; } = 0;
        
        /// <summary>
        /// 試薬ロット番号
        /// </summary>
        public String ReagentLotNo { get; set; } = "";

        /// <summary>
        /// 繰返し回数
        /// </summary>
        /// <remarks>
        /// MeasItemクラスと合わせている
        /// </remarks>
        public Int32 RepCount { get; set; } = 1;

        /// <summary>
        /// 分析開始済フラグ
        /// </summary>
        public Boolean AssayStarted { get; set; } = false;

        /// <summary>
        /// キャリブレータ用の仮移動先モジュールリスト
        /// </summary>
        public List<RackModuleIndex> PreModuleIdListForCalibrator { get; set; } = new List<RackModuleIndex>();

        /// <summary>
        /// 分析予定存在チェック
        /// </summary>
        /// <remarks>
        /// 指定された試薬コード、試薬ロットに合致する分析予定があるかを返す。
        /// 試薬ロットの指定有無については、ラック種別によって異なる（呼出元で制御）
        /// ロット指定不可：一般検体、コントロール（現ロット）
        /// ロット指定必須：キャリブレータ、コントロール（ロット指定、全ロット）
        /// </remarks>
        /// <param name="protoNo">試薬コード</param>
        /// <param name="reagentLotNo">試薬ロット番号</param>
        /// <param name="moduleId">モジュールID（未指定時はモジュールは無視）</param>
        /// <returns>true:分析予定あり、false:分析予定なし</returns>
        public Boolean ExistsSchedule (Int32 protoNo, String reagentLotNo, Int32 moduleId = CarisXConst.ALL_MODULEID)
        {
            return (AssayStarted == false 
                && ProtoNo == protoNo 
                && (ReagentLotNo == reagentLotNo || ReagentLotNo == String.Empty) 
                && (ModuleId == (RackModuleIndex)moduleId || moduleId == CarisXConst.ALL_MODULEID));
        }
    }

    /// <summary>
    /// ラックステータス情報
    /// </summary>
    /// <remarks>
    /// 1列単位で保持します
    /// </remarks>
    public class RackInfo : ICloneable
    {
        #region [プロパティ]

        /// <summary>
        /// ラックID
        /// </summary>
        public CarisXIDString RackId { get; set; } = "";

        /// <summary>
        /// 検体種別（移動元）
        /// </summary>
        public SampleMoveSourceKind SampleMoveSource { get; set; } = SampleMoveSourceKind.Sample;

        /// <summary>
        /// 検体区分
        /// </summary>
        public SampleKind SampleKind
        {
            get
            {
                SampleKind retKind = SampleKind.Sample;
                switch (SampleMoveSource)
                {
                    case SampleMoveSourceKind.Sample:
                        //一般検体の場合、ラックIDから取得出来る検体区分を設定しておく
                        retKind = RackId.GetSampleKind();
                        break;
                    case SampleMoveSourceKind.Line:
                        //外部搬送から取得した検体の場合は搬送検体
                        retKind = SampleKind.Line;
                        break;
                    case SampleMoveSourceKind.STAT:
                        //STATから取得した検体の場合は優先検体
                        retKind = SampleKind.Priority;
                        break;
                    default:
                        retKind = SampleKind.Sample;
                        break;
                }
                return retKind;
            }
        }

        /// <summary>
        /// ラックのポジション毎の情報
        /// </summary>
        public List<RackInfoDetail> RackPosition { get; set; }

        /// <summary>
        /// ラック内に自動再検対象の検体が存在するか。
        /// </summary>
        public Boolean IsAutoReMeasure
        {
            get
            {
                foreach (RackInfoDetail rid in RackPosition)
                {
                    foreach (AssaySchedule sch in rid.AssaySchedules)
                    {
                        MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(sch.ProtoNo);
                        if (protocol.UseAutoReTest | protocol.UseAfterDil)
                        {
                            //　自動再検有り
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        #endregion

        #region  [コンストラクタ/デストラクタ]
        public RackInfo()
        {
            RackPosition = new List<RackInfoDetail>();
        }
        #endregion

        #region ICloneable メンバー


        /// <summary>
        /// クローン
        /// </summary>
        /// <remarks>
        /// クローンを作成します
        /// </remarks>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }

    /// <summary>
    /// ラックデータ
    /// </summary>
    /// <remarks>
    /// ラック情報通知で受信したラックの情報を保持します。
    /// このデータは、前回起動時とことなる日付に起動された時、
    /// 前回分析時と異なる日付に分析開始された時にクリアします。
    /// </remarks>
    public class RackInfoManager
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 初期化完了フラグ
        /// </summary>
        public Boolean blnInitialized = false;

        /// <summary>
        /// サンプリングステージラックステータス
        /// </summary>
        private List<RackInfo> rackInfo = new List<RackInfo>();

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラック情報
        /// </summary>
        public List<RackInfo> RackInfo
        {
            get
            {
                return this.rackInfo;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// ラック情報初期化
        /// </summary>
        /// <remarks>
        /// ラック情報初期化します
        /// </remarks>
        public void RackInfoInitialize()
        {
            if (!this.blnInitialized)
            {
                //ラック情報をクリア
                this.rackInfo.Clear();

                //ラック情報初期化
                this.rackInfo.Add(new RackInfo());
                this.rackInfo.Last().RackId = String.Empty;
                this.rackInfo.Last().SampleMoveSource = SampleMoveSourceKind.Sample;

                for (int i = 0; i < CarisXConst.RACK_POS_COUNT; i++)
                {
                    this.rackInfo.Last().RackPosition.Add(new RackInfoDetail());
                    this.rackInfo.Last().RackPosition.Last().PositionNo = i + 1;
                    this.rackInfo.Last().RackPosition.Last().SampleID = String.Empty;
                    this.rackInfo.Last().RackPosition.Last().CupKind = SpecimenCupKind.None;
                    this.rackInfo.Last().RackPosition.Last().AssaySchedules = new List<AssaySchedule>();
                }

                this.blnInitialized = true;
            }
        }

        /// <summary>
        /// ラック情報設定
        /// </summary>
        /// <remarks>
        /// ラック情報を設定します。
        /// </remarks>
        /// <param name="info">ラック情報のインスタンス</param>
        public void SetRackInfo(RackInfo info)
        {
            //セットしようとしているラック情報がすでに存在する場合は置き換える。
            if (this.rackInfo.Exists(x => x.RackId.DispPreCharString == info.RackId.DispPreCharString))
            {
                this.rackInfo.RemoveAt(this.rackInfo.FindIndex(x => x.RackId.DispPreCharString == info.RackId.DispPreCharString));
            }

            this.rackInfo.Add(info);
        }

        /// <summary>
        /// 保持データクリア
        /// </summary>
        /// <remarks>
        /// 保持データをクリアします。
        /// </remarks>
        public void Clear()
        {
            // ラックステータスクリア処理
            this.rackInfo.Clear();
            this.blnInitialized = false;
        }

        /// <summary>
        /// 分析予定への開始設定
        /// </summary>
        /// <remarks>
        /// 分析予定に対して開始フラグを設定する
        /// </remarks>
        public void SetAssayStarted(IMeasureIndicate wsdata)
        {
            String dbgMsgHead = String.Format("[[Investigation log]]FormMainFrame::{0} ", MethodBase.GetCurrentMethod().Name);
            String dbgMsg = dbgMsgHead;

            String rackId = wsdata.RackID;
            Int32 rackPosition = wsdata.SamplePosition;

            var rackInfo = this.rackInfo.Where(x => x.RackId.DispPreCharString == rackId).FirstOrDefault();
            if (rackInfo == null)
            {
                //ラック情報がない場合は以降の処理を行わない
                return;
            }

            var rackInfoDetail = rackInfo.RackPosition.Where(v => v.PositionNo == rackPosition).FirstOrDefault();
            if (rackInfoDetail == null)
            {
                //対象ポジションのラック情報詳細がない場合、分析予定がないはずなのでWSDataの取得結果はクリアする
                //（発生しない想定）
                return;
            }

            foreach (var measItem in wsdata.MeasItemArray)
            {
                //コントロールの場合、モジュールIDまで一致する必要がある
                Int32 moduleId = CarisXConst.ALL_MODULEID;
                if (rackInfo.RackId.PreChar == CarisXConst.CONTROL_RACK_ID_PRECHAR)
                {
                    moduleId = wsdata.ModuleID;
                }
                
                var assaySch = rackInfoDetail.AssaySchedules.Where(v => v.AssayStarted == false && v.ProtoNo == measItem.ProtoNo
                    && (v.ReagentLotNo == measItem.ReagentLotNo || v.ReagentLotNo == String.Empty) 
                    && (v.ModuleId == (RackModuleIndex)moduleId || moduleId == CarisXConst.ALL_MODULEID)).FirstOrDefault();
                if (assaySch != null)
                {
                    dbgMsg = dbgMsgHead + String.Format("{0}-{1} ProtocolNo-ReagentLotNo:{2} {3} starts assay in {4} "
                        , rackId, rackPosition, assaySch.ProtoNo + "-" + assaySch.ReagentLotNo, assaySch.ModuleId, (RackModuleIndex)wsdata.ModuleID);
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                    assaySch.AssayStarted = true;
                }
            }
        }

        /// <summary>
        /// 分析予定存在確認
        /// </summary>
        /// <remarks>
        /// 対象のラックにまだAssayが開始していない分析予定が１つでも存在するか確認する
        /// </remarks>
        /// <param name="rackId">対象のラックID</param>
        /// <returns>
        /// true:存在する、false:存在しない
        /// </returns>
        public Boolean ExistsAssaySchedule(String rackId)
        {
            Boolean rtnVal = false;

            var rackInfo = this.rackInfo.Where(x => x.RackId.DispPreCharString == rackId).FirstOrDefault();
            if (rackInfo != null)
            {
                rtnVal = rackInfo.RackPosition.Exists(v => v.AssaySchedules.Exists(vv => vv.AssayStarted == false));
            }

            return rtnVal;
        }

        /// <summary>
        /// 分析予定取得
        /// </summary>
        /// <remarks>
        /// 対象のラックから分析開始していない分析予定を取得する
        /// </remarks>
        /// <param name="rackId">対象のラックID</param>
        /// <returns>
        /// true:存在する、false:存在しない
        /// </returns>
        public List<AssaySchedule> GetAssaySchedules(String rackId, Int32 moduleId = CarisXConst.ALL_MODULEID)
        {
            List<AssaySchedule> rtnVal = new List<AssaySchedule>();

            foreach (var rackInfo in this.rackInfo.Where(x => rackId == String.Empty || x.RackId.DispPreCharString == rackId))
            {
                foreach (var rackDetail in rackInfo.RackPosition)
                {
                    foreach (var assaySchedule in rackDetail.AssaySchedules
                        .Where(v => v.AssayStarted == false && (v.ModuleId == (RackModuleIndex)moduleId || moduleId == CarisXConst.ALL_MODULEID)))
                    {
                        rtnVal.Add(assaySchedule);
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 分析予定から最小の分析項目番号を取得する
        /// </summary>
        /// <remarks>
        /// 対象ラックの未開始分析予定から、最小の分析項目番号を取得する
        /// </remarks>
        /// <param name="rackId">対象のラックID</param>
        /// <returns>
        /// 分析項目番号
        /// </returns>
        public Int32 GetMinimumProtoNoInAssaySch(String rackId)
        {
            Int32 rtnVal = 999;

            //未開始の分析予定があるかチェックする
            if (ExistsAssaySchedule(rackId))
            {
                var rackInfo = this.rackInfo.Where(x => x.RackId.DispPreCharString == rackId).FirstOrDefault();

                foreach (var rackDetail in rackInfo.RackPosition)
                {
                    foreach (var assaySch in rackDetail.AssaySchedules.Select(v => v.ProtoNo).Distinct())
                    {
                        //保持しているプロトコル番号よりも取得した分析予定のプロトコル番号の方が小さい場合は上書きする
                        if (rtnVal > assaySch)
                        {
                            rtnVal = assaySch;
                        }
                    }
                }
            }

            return rtnVal;
        }

        #endregion         
    }

    /// <summary>
    /// ラック設置状況データ
    /// </summary>
    /// <remarks>
    /// 分析中、スレーブから送信されるラック設置状況を保持します。
    /// このデータは、前回起動時とことなる日付に起動された時、
    /// 前回分析時と異なる日付に分析開始された時にクリアします。
    /// </remarks>
    public class RackSettingStatusManager : ISavePath // データはアプリ終了の際も保持される。
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 初期化完了フラグ
        /// </summary>
        public Boolean blnInitialized = false;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラック引込奥ステータス
        /// </summary>
        public SampleRackData RackPullBackStatus { get; set; } = new SampleRackData();

        /// <summary>
        /// ラック引込手前ステータス
        /// </summary>
        public SampleRackData RackPullFrontStatus { get; set; } = new SampleRackData();

        /// <summary>
        /// STATステータス
        /// </summary>
        public STATData STATStatus { get; set; } = new STATData();

        /// <summary>
        /// 外部搬送
        /// </summary>
        public OutsideTransfer OutsideTransfer { get; set; } = new OutsideTransfer();

        /// <summary>
        /// 順番
        /// </summary>
        public Int32[] TurnOrder { get; set; } = new Int32[4];


        #endregion

        #region [publicメソッド]

        /// <summary>
        /// ラック設置状況初期化
        /// </summary>
        /// <remarks>
        /// ラック設置状況の内容を初期化します
        /// </remarks>
        public void RackSettingStatusInitialize()
        {
            if (!this.blnInitialized)
            {
                //ラック引込奥
                this.RackPullBackStatus.RackID = string.Empty;
                for (int i = 0; i < CarisXConst.RACK_POS_COUNT; i++)
                {
                    this.RackPullBackStatus.SampleID[i] = 0;
                }
                this.RackPullBackStatus.RackStatus = RackStatus.Empty;
                this.RackPullBackStatus.NumRemainingCycles = 0;

                //ラック引込手前
                this.RackPullFrontStatus.RackID = string.Empty;
                for (int i = 0; i < CarisXConst.RACK_POS_COUNT; i++)
                {
                    this.RackPullFrontStatus.SampleID[i] = 0;
                }
                this.RackPullFrontStatus.RackStatus = RackStatus.Empty;
                this.RackPullFrontStatus.NumRemainingCycles = 0;

                //STAT
                for (int i = 0; i < CarisXConst.STAT_POS_COUNT; i++)
                {
                    this.STATStatus.SampleID[i] = 0;
                }
                this.STATStatus.RackStatus = RackStatus.Empty;
                this.STATStatus.NumRemainingCycles = 0 ;

                //外部搬送
                for (int i = 0; i < CarisXConst.OUTSIDETRANSFER_POS_COUNT; i++)
                {
                    this.OutsideTransfer.SampleID [i] = 0;
                }
                this.OutsideTransfer.RackStatus = RackStatus.Empty;
                this.OutsideTransfer.NumRemainingCycles = 0;

                //順番
                for (int i = 0; i < TurnOrder.Length; i++)
                {
                    this.TurnOrder[i] = 0;
                }


                this.blnInitialized = true;
            }
        }

        /// <summary>
        /// 保持データクリア
        /// </summary>
        /// <remarks>
        /// 保持データをクリアします。
        /// </remarks>
        public void Clear()
        {
            RackPullBackStatus = new SampleRackData();
            RackPullFrontStatus = new SampleRackData();
            STATStatus = new STATData();
            OutsideTransfer = new OutsideTransfer();
            TurnOrder = new Int32[4];

            this.blnInitialized = false;
        }

        #endregion         

        #region ISavePath メンバー

        /// <summary>
        /// 保存パス
        /// </summary>
        public String SavePath
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

    }

    public class RackSettingStatusManagerList
    {
        public List<RackSettingStatusManager> rackSettingStatusManagersList;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackSettingStatusManagerList()
        {
            rackSettingStatusManagersList = new List<RackSettingStatusManager>();
            Int32 connectModuleCount = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;

            // 接続台数分リスト作成
            for (int i = 0; i < connectModuleCount; i++)
            {
                rackSettingStatusManagersList.Add(new RackSettingStatusManager());
            }

        }

        /// <summary>
        /// ラック設置状況の全削除
        /// </summary>
        public void RackSettingDataAllClear()
        {
            foreach(RackSettingStatusManager data in rackSettingStatusManagersList)
            {
                data.Clear();
            }
        }


    }

    /// <summary>
    /// ラック位置マネージャ
    /// </summary>
    /// <remarks>
    /// 分析中、ラック移動位置問合せ（0119）を行った結果を格納。
    /// このデータは、前回起動時とことなる日付に起動された時、
    /// 前回分析時と異なる日付に分析開始された時にクリア。
    /// Caris200の時にあったSampleRackInfoManagerの代替。
    /// </remarks>
    public class RackPositionManager
    {
        /// <summary>
        /// 初期化完了フラグ
        /// </summary>
        private Boolean blnInitialized = false;

        private Dictionary<String, RackPositionKind> rackPosition = new Dictionary<String, RackPositionKind>();

        #region [プロパティ]

        /// <summary>
        /// サンプリングステージラックステータス 取得/設定
        /// </summary>
        public Dictionary<String, RackPositionKind> RackPosition
        {
            get
            {
                return this.rackPosition;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// ラック位置初期化
        /// </summary>
        /// <remarks>
        /// ラック位置初期化します
        /// </remarks>
        public void RackPositionInitialize()
        {
            if (!this.blnInitialized)
            {
                this.rackPosition.Clear();
                this.rackPosition = new Dictionary<String, RackPositionKind>();

                this.blnInitialized = true;
            }
        }

        /// <summary>
        /// ラック位置設定
        /// </summary>
        /// <remarks>
        /// ラック位置を設定します。
        /// </remarks>
        public void SetRackPosition(CarisXIDString rackID, RackPositionKind rackPosition)
        {
            if (this.rackPosition.ContainsKey(rackID.DispPreCharString))
                this.rackPosition[rackID.DispPreCharString] = rackPosition;
            else
                this.rackPosition.Add(rackID.DispPreCharString, rackPosition);
        }

        /// <summary>
        /// 保持データクリア
        /// </summary>
        /// <remarks>
        /// 保持データをクリアします。
        /// </remarks>
        public void Clear()
        {
            this.rackPosition.Clear();
        }

        #endregion         

    }

}
