using System;
using System.Linq;
using System.Drawing;
using System.Text;
using Oelco.Common.Comm;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Const;
using Oelco.Common.Utility;
using Oelco.CarisX.Parameter;
using Oelco.Common.Parameter;
using Oelco.Common.GUI;
using Infragistics.Win.Misc;
using System.Collections.Generic;
using Oelco.CarisX.Log;
using Oelco.CarisX.Utility;
using Oelco.Common.Log;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 初期化画面クラス
    /// </summary>
    public partial class DlgInitialize : DlgCarisXBase
    {
        /// <summary>
        /// 初期化処理無効モード
        /// </summary>
        private static bool noneInitializeMode;

        /// <summary>
        /// アクティブモジュールフラグ
        /// </summary>
        /// <remarks>
        /// アクティブなモジュールがわかるようにする。Trueがアクティブ。配列の0がラック、他がモジュール
        /// </remarks>
        private Boolean[] flgActiveModule = new Boolean[Enum.GetValues(typeof(RackModuleIndex)).Length];

        /// <summary>
        /// 各モジュールの進捗情報
        /// </summary>
        /// <remarks>
        /// 各モジュールの進捗情報を保持しておく
        /// </remarks>
        private ProgressInfo[] progressInfos = new ProgressInfo[Enum.GetValues(typeof(RackModuleIndex)).Length];

        private const Int32 dlgSizeHeight1 = 552;
        private const Int32 dlgSizeHeight2 = 581;
        private const Int32 dlgSizeHeight3 = 610;
        private const Int32 dlgSizeHeight4 = 639;

        /// <summary>
        /// 初期化処理無効モードの取得、設定
        /// </summary>
        public static bool NoneInitializeMode
        {
            get
            {
                return noneInitializeMode;
            }
            set
            {
                noneInitializeMode = value;
            }
        }

        /// <summary>
        /// プログレスバー終了位置の取得
        /// </summary>
        public Int32 ProgressEndPos
        {
            get
            {
                return this.prgInitializeProgressSlave1.Maximum;
            }
        }

        /// <summary>
        /// 画面初期化
        /// </summary>
        /// <remarks>
        /// 画面の初期化処理を行います
        /// </remarks>
        public DlgInitialize(ProgressInfo[] progressInfos)
        {
            InitializeComponent();

            //フラグを初期化
            for (int i = 0; i < flgActiveModule.Length; i++)
            {
                flgActiveModule[i] = false;
            }

            //既に初期シーケンスが一度でも動いていた場合、ProgressInfoから前回の内容を表示する
            foreach (var progressInfo in progressInfos)
            {
                if (progressInfo != null)
                    SetProgressInfo(progressInfo);
            }

            // 初期化処理無効モードか確認
            if (noneInitializeMode)
            {
                lblDialogTitle.Appearance.BackColor = System.Drawing.Color.Orange;
            }
            else
            {
                lblDialogTitle.Appearance.BackColor = System.Drawing.Color.LightGray;
            }

            //モジュール接続台数に応じて画面のコントロールの表示を変更
            AdjustControlbyConnectModule(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected);

            // エラーチェック処理追加
            Singleton<NotifyManager>.Instance.AddNotifyTarget((Int32)NotifyKind.CheckErrorRackModule, this.ChangeMotorErrorStatus);
        }

        /// <summary>
        /// プログレスバーの情報設定
        /// </summary>
        /// <remarks>
        /// プログレスバーの情報を設定します
        /// </remarks>
        /// <param name="info"></param>
        public void SetProgressInfo(ProgressInfo info)
        {
            //ポジションが10%以上の場合、対象のモジュールがアクティブと判断してフラグをオン
            if (info.ProgressPos > 0)
            {
                flgActiveModule[(int)info.TargetModuleNo] = true;
            }

            //受信した処理情報を退避
            progressInfos[(int)info.TargetModuleNo] = info;

            // ラック搬送とスレーブ毎に進捗率更新
            if (info.TargetModuleNo == RackModuleIndex.RackTransfer)
            {
                // --- ラック搬送 ---
                // 進捗率更新
                updateProgressBar(info, this.prgInitializeProgressRackTransfer);

                setEndStatusString(this.lblSystemInitStatusRackTransfer, info.EndSystemInit);
                setEndStatusString(this.lblMotorSelfStatusRackTransfer, info.EndMotorSelf);
            }
            else
            {
                // --- スレーブ ---

                switch (info.TargetModuleNo)
                {
                    case RackModuleIndex.Module1:
                        // 進捗率更新
                        updateProgressBar(info, this.prgInitializeProgressSlave1);
                        break;
                    case RackModuleIndex.Module2:
                        // 進捗率更新
                        updateProgressBar(info, this.prgInitializeProgressSlave2);
                        break;
                    case RackModuleIndex.Module3:
                        // 進捗率更新
                        updateProgressBar(info, this.prgInitializeProgressSlave3);
                        break;
                    case RackModuleIndex.Module4:
                        // 進捗率更新
                        updateProgressBar(info, this.prgInitializeProgressSlave4);
                        break;
                    default:
                        break;
                }

                //ステータスを設定したモジュールのProgressInfoだけ取得する
                ProgressInfo[] ActiveProgressInfos = getActiveModuleProgressInfos();

                //全モジュールの各進捗状態から画面に設定する文字列を決定し、画面に表示する
                setEndStatusString(this.lblSystemInitStatusSlave, ActiveProgressInfos.Select(v => v.EndSystemInit).ToArray());
                setEndStatusString(this.lblMotorSelfStatusSlave, ActiveProgressInfos.Select(v => v.EndMotorSelf).ToArray());
                setEndStatusString(this.lblOpticalSelfStatus, ActiveProgressInfos.Select(v => v.EndOpticalSelf).ToArray());
                setEndStatusString(this.lblReagentCheckStatus, ActiveProgressInfos.Select(v => v.EndReagentCheck).ToArray());
                setEndStatusString(this.lblPrimeStatus, ActiveProgressInfos.Select(v => v.EndPrime).ToArray());

            }
        }

        /// <summary>
        /// 稼働中のモジュールの進捗情報を取得する
        /// </summary>
        /// <remarks>
        /// モジュール１～４の内、稼働しているモジュールの進捗情報を取得する
        /// </remarks>
        /// <returns>進捗情報（配列）</returns>
        private ProgressInfo[] getActiveModuleProgressInfos()
        {
            //アクティブモジュールフラグと進捗情報を結合して、１要素にアクティブモジュールと進捗情報を持つ配列を作成する
            var infos = flgActiveModule.Skip(1).Zip(progressInfos.Skip(1).ToArray(), (status, info) => new { status, info });

            //アクティブとなっている進捗情報を取得する
            var ActiveInfos = infos.Where(v => v.status == true).Select(v => v.info);

            //進捗情報の配列型として返す
            return ActiveInfos.ToArray();
        }

        /// <summary>
        /// 進捗状態の画面表示文字列取得
        /// </summary>
        /// <remarks>
        /// 引数で渡された各モジュールの進捗状態の配列から、全体の進捗状態を判定し、画面表示する為の文字列を決定する
        /// 本来発生しないが、もし空配列を渡された場合は未完了扱いとする
        /// </remarks>
        /// <param name="status">対象の項目の進捗状態</param>
        /// <returns>画面表示する為の文字列</returns>
        private String setEndStatusString(UltraLabel label, ProgressInfoEndStatusKind[] status)
        {
            ProgressInfoEndStatusKind AllStatus;
            String endstatus = String.Empty;

            if (status.Length == 0)
            {
                AllStatus = ProgressInfoEndStatusKind.NotComplete;
            }
            else if (status.All(v => v == ProgressInfoEndStatusKind.Completed))
            {
                //全モジュールが完了状態
                AllStatus = ProgressInfoEndStatusKind.Completed;
            }
            else if (status.Any(v => v == ProgressInfoEndStatusKind.Error))
            {
                //エラー扱いのものがある
                AllStatus = ProgressInfoEndStatusKind.Error;
            }
            else
            {
                AllStatus = ProgressInfoEndStatusKind.NotComplete;
            }

            return setEndStatusString(label, AllStatus);
        }

        /// <summary>
        /// 進捗状態の画面表示文字列取得
        /// </summary>
        /// <remarks>
        /// 引数で渡されたモジュールの進捗状態から、画面表示する為の文字列を決定する
        /// </remarks>
        /// <param name="status">対象の項目の進捗状態</param>
        /// <returns>画面表示する為の文字列</returns>
        private String setEndStatusString(UltraLabel label, ProgressInfoEndStatusKind progressInfoKind)
        {
            String endstatus = String.Empty;

            switch (progressInfoKind)
            {
                case ProgressInfoEndStatusKind.NotComplete:
                    label.Text = Properties.Resources.STRING_STARTUP_009;
                    label.Appearance.ForeColor = new Color();
                    break;
                case ProgressInfoEndStatusKind.Completed:
                    label.Text = Properties.Resources.STRING_STARTUP_008;
                    label.Appearance.ForeColor = new Color();
                    break;
                case ProgressInfoEndStatusKind.Error:
                    label.Text = Properties.Resources.STRING_STARTUP_016;
                    label.Appearance.ForeColor = Color.Red;
                    break;
            }

            return endstatus;
        }

        /// <summary>
        /// モジュールの進捗状態エラーチェック
        /// </summary>
        /// <remarks>
        /// モジュールの進捗状態のいずれかにエラーが含まれているかをチェックする
        /// </remarks>
        /// <param name="info">進捗情報</param>
        /// <returns>true:エラー有り、false:エラーなし</returns>
        private void updateProgressBar(ProgressInfo info, CustomProgressBar bar)
        {
            //進捗率設定
            bar.Value = info.ProgressPos;

            String dbg = String.Format("DlgInitialize::updateProgressBar");
            dbg = dbg + String.Format(" TargetModuleNo = {0} EndSystemInit  = {1} EndMotorSelf  = {2} EndOpticalSelf  = {3} EndReagentCheck  = {4} EndPrime  = {5} "
                ,info.TargetModuleNo, info.EndSystemInit, info.EndMotorSelf, info.EndOpticalSelf, info.EndReagentCheck, info.EndPrime);

            //ステータスのいずれかがエラーの場合、バーの文字色を赤色にする
            if (info.EndSystemInit == ProgressInfoEndStatusKind.Error
                || info.EndMotorSelf == ProgressInfoEndStatusKind.Error
                || info.EndOpticalSelf == ProgressInfoEndStatusKind.Error
                || info.EndReagentCheck == ProgressInfoEndStatusKind.Error
                || info.EndPrime == ProgressInfoEndStatusKind.Error
                )
            {
                dbg = dbg + String.Format(" Red!! ");
                bar.Appearance.ForeColor = Color.Red;
                bar.FillAppearance.ForeColor = Color.Red;
            }
            else
            {
                dbg = dbg + String.Format(" new Color ");
                bar.Appearance.ForeColor = new Color();
                bar.FillAppearance.ForeColor = new Color();

            }

            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                            dbg);
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            // ラベル設定
            this.lblDialogTitle.Text = Properties.Resources.STRING_STARTUP_000;

            // スレーブ
            this.gbxSlave.Text = Properties.Resources.STRING_STARTUP_015;
            this.lblProcessColumnSlave.Text = Properties.Resources.STRING_STARTUP_001;
            this.lblStatusColumnSlave.Text = Properties.Resources.STRING_STARTUP_002;
            this.lblSystemInitSlave.Text = Properties.Resources.STRING_STARTUP_003;
            this.lblMotorSelfSlave.Text = Properties.Resources.STRING_STARTUP_004;
            this.lblOpticalSelf.Text = Properties.Resources.STRING_STARTUP_005;
            this.lblReagentCheck.Text = Properties.Resources.STRING_STARTUP_006;
            this.lblPrime.Text = Properties.Resources.STRING_STARTUP_007;

            // ラック搬送
            this.gbxRackTransfer.Text = Properties.Resources.STRING_STARTUP_010;
            this.lblProcessColumnRackTransfer.Text = Properties.Resources.STRING_STARTUP_001;
            this.lblStatusColumnRackTransfer.Text = Properties.Resources.STRING_STARTUP_002;
            this.lblSystemInitRackTransfer.Text = Properties.Resources.STRING_STARTUP_003;
            this.lblMotorSelfRackTransfer.Text = Properties.Resources.STRING_STARTUP_004;

            // 初期状態設定
            //ラックがアクティブではない場合
            if (!flgActiveModule[(int)RackModuleIndex.RackTransfer])
            {
                this.lblSystemInitStatusRackTransfer.Text = Properties.Resources.STRING_STARTUP_009;
                this.lblMotorSelfStatusRackTransfer.Text = Properties.Resources.STRING_STARTUP_009;
            }

            //ラック以外がすべてアクティブではない場合
            if (flgActiveModule.Skip(1).All(v => !v))
            {
                this.lblSystemInitStatusSlave.Text = Properties.Resources.STRING_STARTUP_009;
                this.lblMotorSelfStatusSlave.Text = Properties.Resources.STRING_STARTUP_009;
                this.lblOpticalSelfStatus.Text = Properties.Resources.STRING_STARTUP_009;
                this.lblReagentCheckStatus.Text = Properties.Resources.STRING_STARTUP_009;
                this.lblPrimeStatus.Text = Properties.Resources.STRING_STARTUP_009;
            }
        }

        /// <summary>
        /// 画面の終了可否チェック
        /// </summary>
        /// <remarks>
        /// 画面を終了してよいかチェックを行います
        /// </remarks>
        public Boolean ChkCanClose(ProgressInfo[] progressInfos)
        {

            String dbgMsg = String.Format("[[Investigation log]]DlgInitialize::ChkCanClose");

            Boolean flgCanClose = true;

            //既に初期シーケンスが一度でも動いていた場合、ProgressInfoから前回の内容を表示する
            foreach (var progressInfo in progressInfos)
            {
                //何も動いてない
                if (progressInfo == null)
                {
                    dbgMsg = dbgMsg + String.Format("No Progress");
                    continue;
                }

                dbgMsg = dbgMsg + String.Format(" TargetModuleNo = {0}", progressInfo.TargetModuleNo);

                //モジュールと繋がっていない
                switch (progressInfo.TargetModuleNo)
                {
                    case RackModuleIndex.RackTransfer:
                        dbgMsg = dbgMsg + String.Format("RackTransferCommStatus = {0}", Singleton<CarisXCommManager>.Instance.GetRackTransferCommStatus());
                        if (Singleton<CarisXCommManager>.Instance.GetRackTransferCommStatus() == ConnectionStatus.Offline)
                            continue;
                        break;
                    case RackModuleIndex.Module1:
                        dbgMsg = dbgMsg + String.Format("SlaveCommStatus = {0}", Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus((int)ModuleIndex.Module1));
                        if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus((int)ModuleIndex.Module1) == ConnectionStatus.Offline)
                            continue;
                        break;
                    case RackModuleIndex.Module2:
                        dbgMsg = dbgMsg + String.Format("SlaveCommStatus = {0}", Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus((int)ModuleIndex.Module2));
                        if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus((int)ModuleIndex.Module2) == ConnectionStatus.Offline)
                            continue;
                        break;
                    case RackModuleIndex.Module3:
                        dbgMsg = dbgMsg + String.Format("SlaveCommStatus = {0}", Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus((int)ModuleIndex.Module3));
                        if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus((int)ModuleIndex.Module3) == ConnectionStatus.Offline)
                            continue;
                        break;
                    case RackModuleIndex.Module4:
                        dbgMsg = dbgMsg + String.Format("SlaveCommStatus = {0}", Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus((int)ModuleIndex.Module4));
                        if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus((int)ModuleIndex.Module4) == ConnectionStatus.Offline)
                            continue;
                        break;
                }

                //中断の指示が出ている
                if (progressInfo.IsAbort)
                {
                    dbgMsg = dbgMsg + String.Format(" Abort");
                    continue;
                }

                //Progressバーが100%になっている場合はもう終わっている
                if (progressInfo.ProgressPos == ProgressEndPos)
                {
                    dbgMsg = dbgMsg + String.Format(" ProgressPos = {0}", ProgressEndPos);
                    continue;
                }

                dbgMsg = dbgMsg + String.Format(" Module is moveing ");

                //ここまで来るとこのモジュールはまだ動いているという事なので画面は閉じれない
                flgCanClose = false;
            }
            dbgMsg = dbgMsg + String.Format(" result = {0}", flgCanClose);
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);

            return flgCanClose;
        }

        /// <summary>
        /// 画面タイトルクリックイベント
        /// </summary>
        /// <remarks>
        /// 初初期化処理無効モードの設定、解除を行います
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblDialogTitle_Click(object sender, EventArgs e)
        {
            if (!noneInitializeMode)
            {
                lblDialogTitle.Appearance.BackColor = System.Drawing.Color.Orange;
                noneInitializeMode = true;
            }
            else
            {
                lblDialogTitle.Appearance.BackColor = System.Drawing.Color.LightGray;
                noneInitializeMode = false;
            }
        }

        /// <summary>
        /// モジュールの接続数によるコントロールの調整
        /// </summary>
        /// <param name="connectCount"></param>
        private void AdjustControlbyConnectModule(Int32 connectCount)
        {
            prgInitializeProgressSlave1.Visible = ((Int32)RackModuleIndex.Module1 <= connectCount);
            prgInitializeProgressSlave2.Visible = ((Int32)RackModuleIndex.Module2 <= connectCount);
            prgInitializeProgressSlave3.Visible = ((Int32)RackModuleIndex.Module3 <= connectCount);
            prgInitializeProgressSlave4.Visible = ((Int32)RackModuleIndex.Module4 <= connectCount);

            StringBuilder sb = new StringBuilder();
            if ((Int32)RackModuleIndex.Module2 <= connectCount)
            {
                sb = new StringBuilder();
                sb.Append(Properties.Resources.STRING_STARTUP_011);
                sb.Append(Infragistics.Win.UltraWinProgressBar.UltraProgressBar.LABEL_FORMATTED);
                this.prgInitializeProgressSlave1.Text = sb.ToString();  //モジュール２以降のバーを表示する場合のみ「Module1: 0%」のフォーマットにする

                sb = new StringBuilder();
                sb.Append(Properties.Resources.STRING_STARTUP_012);
                sb.Append(Infragistics.Win.UltraWinProgressBar.UltraProgressBar.LABEL_FORMATTED);
                this.prgInitializeProgressSlave2.Text = sb.ToString();
            }

            if ((Int32)RackModuleIndex.Module3 <= connectCount)
            {
                sb = new StringBuilder();
                sb.Append(Properties.Resources.STRING_STARTUP_013);
                sb.Append(Infragistics.Win.UltraWinProgressBar.UltraProgressBar.LABEL_FORMATTED);
                this.prgInitializeProgressSlave3.Text = sb.ToString();
            }

            if ((Int32)RackModuleIndex.Module4 <= connectCount)
            {
                sb = new StringBuilder();
                sb.Append(Properties.Resources.STRING_STARTUP_014);
                sb.Append(Infragistics.Win.UltraWinProgressBar.UltraProgressBar.LABEL_FORMATTED);
                this.prgInitializeProgressSlave4.Text = sb.ToString();
            }

            switch (connectCount)
            {
                case (Int32)RackModuleIndex.Module1:
                    this.Size = new Size(this.Size.Width, dlgSizeHeight1);
                    break;
                case (Int32)RackModuleIndex.Module2:
                    this.Size = new Size(this.Size.Width, dlgSizeHeight2);
                    break;
                case (Int32)RackModuleIndex.Module3:
                    this.Size = new Size(this.Size.Width, dlgSizeHeight3);
                    break;
                case (Int32)RackModuleIndex.Module4:
                    this.Size = new Size(this.Size.Width, dlgSizeHeight4);
                    break;
            }
        }

        /// <summary>
        /// スレーブ、ラックのモーターエラー状態を変更
        /// </summary>
        /// <param name="kind">ラック、モジュール番号</param>
        public void ChangeMotorErrorStatus(object kind)
        {
            if ((Int32)kind == (Int32)RackModuleIndex.RackTransfer)
            {
                if (!(Singleton<Utility.CarisXSequenceHelperManager>.Instance.RackTransfer.flgInitializeSequenceCompleted))
                {
                    // ラックのモーターエラーステータスを変更
                    Singleton<Status.SystemStatus>.Instance.setModuleStatus(RackModuleIndex.RackTransfer, Status.SystemStatusKind.MotorError);
                }
            }
            else
            {
                if (!(Singleton<Utility.CarisXSequenceHelperManager>.Instance.Slave[Utility.CarisXSubFunction.ModuleIDToModuleIndex((Int32)kind)].flgInitializeSequenceCompleted))
                {
                    switch (kind)
                    {
                        case (Int32)RackModuleIndex.Module1:
                            // モジュール1のモーターエラーステータスを変更
                            Singleton<Status.SystemStatus>.Instance.setModuleStatus(RackModuleIndex.Module1, Status.SystemStatusKind.MotorError);

                            break;
                        case (Int32)RackModuleIndex.Module2:
                            // モジュール2のモーターエラーステータスを変更
                            Singleton<Status.SystemStatus>.Instance.setModuleStatus(RackModuleIndex.Module2, Status.SystemStatusKind.MotorError);

                            break;
                        case (Int32)RackModuleIndex.Module3:
                            // モジュール3のモーターエラーステータスを変更
                            Singleton<Status.SystemStatus>.Instance.setModuleStatus(RackModuleIndex.Module3, Status.SystemStatusKind.MotorError);

                            break;
                        case (Int32)RackModuleIndex.Module4:
                            // モジュール4のモーターエラーステータスを変更
                            Singleton<Status.SystemStatus>.Instance.setModuleStatus(RackModuleIndex.Module4, Status.SystemStatusKind.MotorError);
                            break;
                    }
                }
            }
        }

    }


    /// <summary>
    /// スタートアップ画面進捗データクラス
    /// </summary>
    /// <remarks>
    /// スタートアップ画面に外部から進捗状態通知を行う際に利用するデータクラスです。
    /// </remarks>
    public class ProgressInfo : ICloneable
    {

        /// <summary>
        /// 表示進捗率
        /// </summary>
        private Int32 progressPos = 0;

        /// <summary>
        /// 表示進捗率の取得、設定
        /// </summary>
        public Int32 ProgressPos
        {
            get
            {
                return progressPos;
            }
            set
            {
                progressPos = value;
            }
        }

        /// <summary>
        /// システム初期化完了の取得、設定
        /// </summary>
        public ProgressInfoEndStatusKind EndSystemInit { get; set; } = ProgressInfoEndStatusKind.NotComplete;

        /// <summary>
        /// モーター自己診断完了の取得、設定
        /// </summary>
        public ProgressInfoEndStatusKind EndMotorSelf { get; set; } = ProgressInfoEndStatusKind.NotComplete;

        /// <summary>
        /// 光学系セルフチェック完了の取得、設定
        /// </summary>
        public ProgressInfoEndStatusKind EndOpticalSelf { get; set; } = ProgressInfoEndStatusKind.NotComplete;

        /// <summary>
        /// 残量チェック完了の取得、設定
        /// </summary>
        public ProgressInfoEndStatusKind EndReagentCheck { get; set; } = ProgressInfoEndStatusKind.NotComplete;

        /// <summary>
        /// 全プライム完了の取得、設定
        /// </summary>
        public ProgressInfoEndStatusKind EndPrime { get; set; } = ProgressInfoEndStatusKind.NotComplete;

        /// <summary>
        /// 進捗率更新対象のスレーブ番号
        /// </summary>
        private RackModuleIndex targetModuleNo = RackModuleIndex.Module1;

        /// <summary>
        /// 進捗率更新対象のスレーブ番号の取得、設定
        /// </summary>
        public RackModuleIndex TargetModuleNo
        {
            get
            {
                return targetModuleNo;
            }
            set
            {
                targetModuleNo = value;
            }
        }

        /// <summary>
        /// 中断
        /// </summary>
        private Boolean isAbort = false;

        /// <summary>
        /// 中断の取得、設定
        /// </summary>
        public Boolean IsAbort
        {
            get
            {
                return isAbort;
            }
            set
            {
                isAbort = value;
            }
        }

        /// <summary>
        /// 値をリセット
        /// </summary>
        public void Reset()
        {
            progressPos = 0;
            isAbort = false;
            EndSystemInit = ProgressInfoEndStatusKind.NotComplete;
            EndMotorSelf = ProgressInfoEndStatusKind.NotComplete;
            EndOpticalSelf = ProgressInfoEndStatusKind.NotComplete;
            EndReagentCheck = ProgressInfoEndStatusKind.NotComplete;
            EndPrime = ProgressInfoEndStatusKind.NotComplete;
        }

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
}
