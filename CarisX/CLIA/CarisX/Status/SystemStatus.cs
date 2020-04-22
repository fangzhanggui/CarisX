using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;
using Oelco.CarisX.Const;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Comm;
using Oelco.Common.Comm;

namespace Oelco.CarisX.Status
{
    /// <summary>
    /// システムステータス管理
    /// </summary>
    /// <remarks>
    /// システムステータスを管理し、変更があった際は通知を行います。
    /// </remarks>
    public class SystemStatus
    {
        /// <summary>
        /// 副ステータス
        /// </summary>
        private Dictionary<SubStatusKind, Boolean> subStatus = new Dictionary<SubStatusKind, Boolean>();
        /// <summary>
        /// 副ステータス 取得
        /// </summary>
        public Dictionary<SubStatusKind, Boolean> SubStatus
        {
            get
            {
                return this.subStatus;
            }
        }

        /// <summary>
        /// 現システムステータス
        /// </summary>
        private SystemStatusKind systemStatus = SystemStatusKind.NoLink;

        /// <summary>
        /// 前システムステータス
        /// </summary>
        private SystemStatusKind prevSystemStatus = SystemStatusKind.NoLink;

        /// <summary>
        /// 現モジュールステータス（ラック＋モジュール１～４を管理）
        /// </summary>
        private SystemStatusKind[] moduleStatus;

        /// <summary>
        /// 前モジュールステータス（ラック＋モジュール１～４を管理）
        /// </summary>
        private SystemStatusKind[] prevModuleStatus;

        /// <summary>
        /// モジュールSTATステータス（モジュール１～４を管理）
        /// </summary>
        private STATStatus[] moduleSTATStatus;

        /// <summary>
        /// システムステータス
        /// </summary>
        /// <remarks>
        /// 副ステータス定義を辞書に追加します
        /// </remarks>
        public SystemStatus()
        {
            // 副ステータス初期化
            foreach (var value in Enum.GetValues(typeof(SubStatusKind)))
            {
                this.subStatus.Add((SubStatusKind)value, false);
            }

            //モジュールステータスの配列を初期化
            moduleStatus = new SystemStatusKind[Enum.GetValues(typeof(RackModuleIndex)).Length];
            prevModuleStatus = new SystemStatusKind[Enum.GetValues(typeof(RackModuleIndex)).Length];
            for (int i = 0; i < moduleStatus.Length; i++)
            {
                moduleStatus[i] = SystemStatusKind.NoLink;
                prevModuleStatus[i] = SystemStatusKind.NoLink;
            }

            PauseReason = new SamplingPauseReason[Enum.GetValues(typeof(RackModuleIndex)).Length];
            for (int i = 0; i < PauseReason.Length; i++)
            {
                PauseReason[i] = SamplingPauseReason.SAMPLINGPAUSEREASON_DEFAULT;
            }

            moduleSTATStatus = new STATStatus[Enum.GetValues( typeof( ModuleIndex ) ).Length];
            for (int i = 0; i < moduleSTATStatus.Length; i++)
            {
                moduleSTATStatus[i] = STATStatus.NotAccepted;
            }
        }

        /// <summary>
        /// 現システムステータス 設定/取得
        /// </summary>
        public SystemStatusKind Status
        {
            get
            {

                return this.systemStatus;
            }
            set
            {
                System.Diagnostics.Debug.WriteLine("[SystemStatusChange]【{0}】To【{1}】", this.systemStatus.ToString(), value.ToString());
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                    , String.Format("[SystemStatusChange]【{0}】To【{1}】", this.systemStatus.ToString(), value.ToString()));

                this.prevSystemStatus = this.systemStatus;
                this.systemStatus = value;
            }
        }

        /// <summary>
        /// 直前ステータス　設定/取得
        /// </summary>
        public SystemStatusKind PrevSystemStatus
        {
            get
            {
                return this.prevSystemStatus;
            }
        }

        /// <summary>
        /// モジュール毎の現ステータス 設定/取得
        /// </summary>
        public SystemStatusKind[] ModuleStatus
        {
            get
            {
                return this.moduleStatus;
            }
            //前ステータスの管理ができないので、Setアクセサは削除して別途メソッドにする
        }

        /// <summary>
        /// モジュール毎の前ステータス 設定/取得
        /// </summary>
        public SystemStatusKind[] PrevModuleStatus
        {
            get
            {
                return this.prevModuleStatus;
            }
        }

        /// <summary>
        /// サンプリング停止理由
        /// </summary>
        public SamplingPauseReason[] PauseReason { get; set; }

        /// <summary>
        /// モジュール毎のSTATステータス 設定/取得
        /// </summary>
        public STATStatus[] ModuleSTATStatus
        {
            get
            {
                return this.moduleSTATStatus;
            }
        }

        /// <summary>
        /// 前ステータス復元
        /// </summary>
        /// <remarks>
        /// 現在のステータスが設定される直前のステータスに戻します。
        /// </remarks>
        /// 
        public void RestorePrevStatus()
        {
            this.Status = this.prevSystemStatus;
        }

        /// <summary>
        /// モジュールステータスの設定
        /// </summary>
        /// <remarks>
        /// モジュールステータスを設定する
        /// </remarks>
        public void setModuleStatus(RackModuleIndex index, SystemStatusKind status)
        {
            // ステータス変更フラグ
            Boolean isStatusChange = false;

            // モーターエラー状態か確認
            if (this.moduleStatus[(int)index] == SystemStatusKind.MotorError)
            {
                // モーターエラー状態からの復旧操作
                if ((status == SystemStatusKind.NoLink)
                 || (status == SystemStatusKind.Shutdown))
                {
                    // 無接続または待機、シャットダウンは復旧操作と見なすため、変更可能
                    isStatusChange = true;
                }
            }
            else
            {
                // モーターエラー以外であれば、変更可能
                isStatusChange = true;
            }

            // ステータス変更可能か確認
            if(isStatusChange)
            {
                String logmsg = String.Format("[ModuleStatusChange]{0}:【{1}】To【{2}】", index.ToString(), this.moduleStatus[(int)index].ToString(), status.ToString());
                System.Diagnostics.Debug.WriteLine(logmsg);
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, logmsg);

                this.prevModuleStatus[(int)index] = this.moduleStatus[(int)index];
                this.moduleStatus[(int)index] = status;

                SystemStatusKind tmpSystemStatus = getSystemStatus();
                if (this.Status != tmpSystemStatus)
                {
                    this.Status = tmpSystemStatus;
                }

                // システムステータス変化通知（試薬交換中を反映する必要もあるので、システムステータスが変わったかどうかにかかわらず呼び出す）
                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SystemStatusChanged, CarisXSubFunction.ModuleIDToModuleIndex((int)index));
            }
        }

        /// <summary>
        /// 全モジュールステータスの設定
        /// </summary>
        /// <remarks>
        /// 全モジュールステータスを設定する
        /// </remarks>
        public void setAllModuleStatus(SystemStatusKind status)
        {
            foreach (RackModuleIndex index in Enum.GetValues(typeof(RackModuleIndex)))
            {
                if (status == SystemStatusKind.ToEndAssay && this.moduleStatus[(int)index] == SystemStatusKind.Standby)
                {
                    //分析終了時、なんらかの要因で既に該当モジュールのステータスが待機状態の場合はコマンドを送信しない
                    continue;
                }

                if (index == RackModuleIndex.RackTransfer)
                {
                    //ラックと接続されている場合のみ、ステータスを設定しにいく
                    if (Singleton<CarisXCommManager>.Instance.GetRackTransferCommStatus() == ConnectionStatus.Online)
                    {
                        setModuleStatus(index, status);
                    }
                }
                else
                {
                    //接続されているモジュールにのみ、ステータスを設定しにいく
                    if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(CarisXSubFunction.ModuleIDToModuleIndex((int)index)) == ConnectionStatus.Online)
                    {
                        setModuleStatus(index, status);
                    }
                }
            }
        }

        /// <summary>
        /// 全モジュールポーズ理由の設定
        /// </summary>
        /// <remarks>
        /// 全モジュールにポーズ理由を設定する
        /// </remarks>
        public void setAllModulePauseReason(SamplingPauseReason.SamplingPauseReasonBit samplingPauseReasonBit)
        {
            foreach (RackModuleIndex index in Enum.GetValues(typeof(RackModuleIndex)))
            {
                if (index == RackModuleIndex.RackTransfer)
                {
                    //ラックと接続されている場合のみ、ステータスを設定しにいく
                    if (Singleton<CarisXCommManager>.Instance.GetRackTransferCommStatus() == ConnectionStatus.Online)
                    {
                        PauseReason[(int)index] |= samplingPauseReasonBit;
                    }
                }
                else
                {
                    //接続されているモジュールにのみ、ステータスを設定しにいく
                    if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(CarisXSubFunction.ModuleIDToModuleIndex((int)index)) == ConnectionStatus.Online)
                    {
                        PauseReason[(int)index] |= samplingPauseReasonBit;
                    }
                }
            }
        }

        /// <summary>
        /// システムステータスの取得
        /// </summary>
        /// <remarks>
        /// 各モジュールステータスの内容から、システムステータスを取得します。
        /// ・基本的にはラックとモジュール１～４が同じステータスになっている場合以外はシステムステータスを維持する。
        /// ・接続されていないラック、モジュール１～４のステータスは基本的に無視する
        /// ・NoLink→Standbyの切替　…　ラックまたはモジュール１～４のいずれか１つでも接続されている場合に切り替える
        /// ・samplingPauseの切替　…　接続されているモジュール１～４がすべてサンプリング停止状態の場合に切り替える
        /// </remarks>
        public SystemStatusKind getSystemStatus()
        {
            SystemStatusKind rtnSystemStatus = SystemStatusKind.NoLink;

            var module1to4Status = getModule1to4Status(true);
            if (module1to4Status.ToList().Count == 0)
            {
                //モジュール１～４がいずれも接続されていない場合、ラックのステータスを設定しておく
                module1to4Status = new List<SystemStatusKind>() { this.ModuleStatus[(int)RackModuleIndex.RackTransfer] };
            }

            SystemStatusKind rackStatus;
            if (Singleton<CarisXCommManager>.Instance.GetRackTransferCommStatus() == ConnectionStatus.Online)
                rackStatus = this.ModuleStatus[(int)RackModuleIndex.RackTransfer];      //ラックが接続されている場合、モジュールステータスの内容を反映
            else
                rackStatus = module1to4Status.FirstOrDefault();                         //ラックが接続されていない場合、モジュール１～４のステータスの最初の一件を反映

            if (module1to4Status.Count() == 1 && module1to4Status.FirstOrDefault() == rackStatus)
            {
                //モジュールステータスがすべて同じ、かつモジュールステータスとラックステータスが同じ場合
                //ラックステータスがシステムステータスとなる
                rtnSystemStatus = rackStatus;
            }
            else
            {
                //モジュールステータスが複数ある場合

                if (this.Status == SystemStatusKind.NoLink
                    && (rackStatus != SystemStatusKind.NoLink || module1to4Status.Where(v => v != SystemStatusKind.NoLink).Count() >= 1))
                {
                    //システムのステータスがNoLinkかつ
                    //ラックのステータスがNoLink以外またはモジュール１～４にステータスがNoLink以外が存在する場合、システムステータスはStandby
                    rtnSystemStatus = SystemStatusKind.Standby;
                }
                else if (chkModule1to4Status(SystemStatusKind.SamplingPause, true))
                {
                    //モジュール１～４がサンプリング停止中の場合、システムステータスはサンプリング停止中
                    rtnSystemStatus = SystemStatusKind.SamplingPause;
                }
                else if (this.Status == SystemStatusKind.SamplingPause && (!chkModule1to4Status(SystemStatusKind.SamplingPause, true)))
                {
                    //システムのステータスがサンプリング停止中かつ
                    //モジュール１～４がすべてサンプリング停止中になっていない場合、システムステータスをAssayに戻す
                    rtnSystemStatus = SystemStatusKind.Assay;
                }
                else
                {
                    //いずれにも当てはまらない場合は、現在のシステムステータスのまま
                    rtnSystemStatus = this.Status;
                }
            }

            return rtnSystemStatus;
        }

        /// <summary>
        /// モジュールステータスのチェック
        /// </summary>
        /// <remarks>
        /// モジュール１～４のステータスが全て指定されたステータスとなっているかどうかチェックする
        /// ※接続されていないモジュールに関してはチェックしない
        /// </remarks>
        public Boolean chkModule1to4Status(SystemStatusKind statusKind, Boolean flgIgnoreReagetExchange)
        {
            var module1to4Status = getModule1to4Status(flgIgnoreReagetExchange);

            if (module1to4Status.Count() == 0)
            {
                //どことも接続されていない場合、チェックするステータスがNoLinkの場合はTrue、以外の場合はFalseを返す
                return (statusKind == SystemStatusKind.NoLink);
            }

            //モジュール１～４のステータスが指定されたステータスと同じ場合はtrue、以外の場合はfalse
            return (module1to4Status.Count() == 1 && module1to4Status.FirstOrDefault() == statusKind);
        }

        /// <summary>
        /// モジュール１～４のステータス取得
        /// </summary>
        /// <remarks>
        /// モジュール１～４のステータスを重複の除いて取得する
        /// ※接続されていないモジュールに関してはチェックしない
        /// </remarks>
        /// <param name="flgIgnoreReagetExchange">試薬交換中のステータスを無視するかどうかのフラグ</param>
        public IEnumerable<SystemStatusKind> getModule1to4Status(Boolean flgIgnoreReagetExchange)
        {
            List<SystemStatusKind> tmpmodule1to4Status = new List<SystemStatusKind>();

            //ModuleIdのEnum値の件数分繰り返す
            foreach (RackModuleIndex index in Enum.GetValues(typeof(RackModuleIndex)))
            {
                if (index == RackModuleIndex.RackTransfer)
                {
                    //ラックは無視
                }
                else
                {
                    //接続されているモジュールのみ、ステータスを取得
                    if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(CarisXSubFunction.ModuleIDToModuleIndex((int)index)) == ConnectionStatus.Online)
                    {
                        if (flgIgnoreReagetExchange && this.ModuleStatus[(int)index] == SystemStatusKind.ReagentExchange)
                        {
                            //試薬交換中を無視する設定で対象のモジュールステータスが試薬交換中場合、前回のモジュールステータスを取得する
                            tmpmodule1to4Status.Add(this.prevModuleStatus[(int)index]);
                        }
                        else
                        {
                            if(this.ModuleStatus[(int)index] != SystemStatusKind.MotorError)
                            {
                                //通常はモジュールステータスを取得する
                                tmpmodule1to4Status.Add(this.ModuleStatus[(int)index]);
                            }
                        }
                    }
                }
            }

            return tmpmodule1to4Status.Distinct();
        }

        /// <summary>
        /// 全モジュールステータスの一括変更
        /// </summary>
        /// <remarks>
        /// 現在のステータスが設定される直前のステータスに戻します。
        /// </remarks>
        public void RestoreAllModulePrevStatus()
        {
            foreach (RackModuleIndex index in Enum.GetValues(typeof(RackModuleIndex)))
            {
                if (index == RackModuleIndex.RackTransfer)
                {
                    //ラックと接続されている場合のみ、ステータスを設定しにいく
                    if (Singleton<CarisXCommManager>.Instance.GetRackTransferCommStatus() == ConnectionStatus.Online)
                    {
                        setModuleStatus(index, this.prevModuleStatus[(int)index]);
                    }
                }
                else
                {
                    //接続されているモジュールにのみ、ステータスを設定しにいく
                    if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(CarisXSubFunction.ModuleIDToModuleIndex((int)index)) == ConnectionStatus.Online)
                    {
                        // モーターエラーのスレーブは処理を行わない
                        if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[(Int32)index] == SystemStatusKind.MotorError)
                        {
                            continue;
                        }

                        setModuleStatus(index, this.prevModuleStatus[(int)index]);
                    }
                }
            }
        }

        /// <summary>
        /// STAT状態設定
        /// </summary>
        /// <param name="moduleIndex"></param>
        /// <param name="status"></param>
        public void SetSTATStatus( int moduleIndex, STATStatus status )
        {
            this.moduleSTATStatus[moduleIndex] = status;
        }

        /// <summary>
        /// STAT状態の存在チェック
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool IsExistModuleSTATStatus( STATStatus status )
        {
            bool result = false;
            foreach(STATStatus moduleStatus in this.moduleSTATStatus )
            {
                if( moduleStatus == status )
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 全モジュールステータスの一括変更
        /// </summary>
        /// <remarks>
        /// 現在のステータスが設定される直前のステータスに戻します。
        /// </remarks>
        public List<Int32> GetConnectedModuleId()
        {
            List<Int32> rtnVal = new List<Int32>();

            foreach (RackModuleIndex index in Enum.GetValues(typeof(RackModuleIndex)))
            {
                if (index == RackModuleIndex.RackTransfer)
                {
                    //ラックは無視
                    continue;
                }
                else
                {
                    //ステータスがNoLink以外の場合、接続されていると判定
                    if (ModuleStatus[(int)index] != SystemStatusKind.NoLink)
                    {
                        //返すリストに値を設定する
                        rtnVal.Add((int)index);
                    }
                }
            }

            return rtnVal;
        }
    }

    /// <summary>
    /// 副ステータス定義
    /// </summary>
    public enum SubStatusKind
    {
        /// <summary>
        /// ホスト問合せ中
        /// </summary>
        InAskHost,
    }

    /// <summary>
    /// システム状態定義
    /// </summary>
    public enum SystemStatusKind
    {
        /// <summary>
        /// 未接続状態
        /// </summary>
        NoLink,
        /// <summary>
        /// 待機中状態
        /// </summary>
        Standby,
        /// <summary>
        /// スレーブ応答待ち状態
        /// </summary>
        WaitSlaveResponce,
        /// <summary>
        /// 分析中状態
        /// </summary>
        Assay,
        /// <summary>
        /// 分析終了移行中状態
        /// </summary>
        ToEndAssay,
        /// <summary>
        /// サンプリング停止中状態
        /// </summary>
        SamplingPause,
        /// <summary>
        /// 終了処理中
        /// </summary>
        Shutdown,
        /// <summary>
        /// 試薬交換開始中
        /// </summary>
        ReagentExchange,
        /// <summary>
        /// モーターエラー状態
        /// </summary>
        MotorError
    }
}
