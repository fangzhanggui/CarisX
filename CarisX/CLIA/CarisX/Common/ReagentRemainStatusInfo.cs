using Oelco.CarisX.Const;
using Oelco.CarisX.Parameter;
using Oelco.Common.Parameter;
using Oelco.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oelco.CarisX.Common
{
    /// <summary>
    /// 試薬残量ステータス閾値
    /// </summary>
    /// <remarks>
    /// グローバルの変数を保持しておく。Singletonで使用。
    /// </remarks>
    public class ReagentRemainStatusInfo
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ReagentRemainStatusInfo()
        {
            Reagent = new List<ReagentRemainStatusInfoItem<RemainStatus>>();
            Reagent.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.Reagent, RemainStatus.Empty, 0 , true));
            Reagent.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.Reagent, RemainStatus.Low, 1, true));
            Reagent.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.Reagent, RemainStatus.Full, 50, true));

            SamplingTipTotal = new List<ReagentRemainStatusInfoItem<RemainStatus>>();
            SamplingTipTotal.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.SamplingTip, RemainStatus.Empty, 0, true));
            SamplingTipTotal.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.SamplingTip, RemainStatus.Low, 1, true));
            SamplingTipTotal.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.SamplingTip, RemainStatus.Full, 49, true));  //１ケースの半分以上あれば緑

            SamplingTip = new List<ReagentRemainStatusInfoItem<RemainStatus>>();
            SamplingTip.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.SamplingTip, RemainStatus.Empty, 0, true));
            SamplingTip.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.SamplingTip, RemainStatus.Low, 1, true));
            SamplingTip.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.SamplingTip, RemainStatus.Full, 20, true));

            PretriggerBottle = new List<ReagentRemainStatusInfoItem<RemainStatus>>();
            PretriggerBottle.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.Pretrigger, RemainStatus.Empty, 0, true));
            PretriggerBottle.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.Pretrigger, RemainStatus.Low, 1, true));
            PretriggerBottle.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.Pretrigger, RemainStatus.Full, 100, true));

            TriggerBottle = new List<ReagentRemainStatusInfoItem<RemainStatus>>();
            TriggerBottle.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.Trigger, RemainStatus.Empty, 0, true));
            TriggerBottle.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.Trigger, RemainStatus.Low, 1, true));
            TriggerBottle.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.Trigger, RemainStatus.Full, 100, true));

            DiluentBottle = new List<ReagentRemainStatusInfoItem<RemainStatus>>();
            DiluentBottle.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.Diluent, RemainStatus.Empty, 0, true));
            DiluentBottle.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.Diluent, RemainStatus.Low, 1, true));
            DiluentBottle.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.Diluent, RemainStatus.Full, 40, true));

            WashSolution = new List<ReagentRemainStatusInfoItem<RemainStatus>>();
            WashSolution.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.WashSolutionBuffer, RemainStatus.Empty, 0, true));
            WashSolution.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.WashSolutionBuffer, RemainStatus.Low, 1, true));
            WashSolution.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.WashSolutionBuffer, RemainStatus.Middle, 2500, true));
            WashSolution.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.WashSolutionBuffer, RemainStatus.Full, 5001, true));

            WasteBuffer = new List<ReagentRemainStatusInfoItem<WasteStatus>>();
            WasteBuffer.Add(new ReagentRemainStatusInfoItem<WasteStatus>(ReagentKind.WasteBuffer, WasteStatus.None, 0, false));
            WasteBuffer.Add(new ReagentRemainStatusInfoItem<WasteStatus>(ReagentKind.WasteBuffer, WasteStatus.NotFull, 1, true));
            WasteBuffer.Add(new ReagentRemainStatusInfoItem<WasteStatus>(ReagentKind.WasteBuffer, WasteStatus.Full, 2, true));

            WasteBox = new List<ReagentRemainStatusInfoItem<WasteBoxViewStatus>>();
            WasteBox.Add(new ReagentRemainStatusInfoItem<WasteBoxViewStatus>(ReagentKind.WasteBox, WasteBoxViewStatus.None, 0, false));
            WasteBox.Add(new ReagentRemainStatusInfoItem<WasteBoxViewStatus>(ReagentKind.WasteBox, WasteBoxViewStatus.NotFull, 1, true));
            WasteBox.Add(new ReagentRemainStatusInfoItem<WasteBoxViewStatus>(ReagentKind.WasteBox, WasteBoxViewStatus.Warning, 2, true));
            WasteBox.Add(new ReagentRemainStatusInfoItem<WasteBoxViewStatus>(ReagentKind.WasteBox, WasteBoxViewStatus.Full, 3, true));

            WasteTank = new List<ReagentRemainStatusInfoItem<WasteStatus>>();
            WasteTank.Add(new ReagentRemainStatusInfoItem<WasteStatus>(ReagentKind.WasteTank, WasteStatus.None, 0, false));
            WasteTank.Add(new ReagentRemainStatusInfoItem<WasteStatus>(ReagentKind.WasteTank, WasteStatus.NotFull, 0, true));
            WasteTank.Add(new ReagentRemainStatusInfoItem<WasteStatus>(ReagentKind.WasteTank, WasteStatus.Full, 1, true));

            WashSolutionTank = new List<ReagentRemainStatusInfoItem<RemainStatus>>();
            WashSolutionTank.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.WashSolutionTank, RemainStatus.Empty, 0, false));
            WashSolutionTank.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.WashSolutionTank, RemainStatus.Low, 1, true));
            WashSolutionTank.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.WashSolutionTank, RemainStatus.Middle, 2, false));
            WashSolutionTank.Add(new ReagentRemainStatusInfoItem<RemainStatus>(ReagentKind.WashSolutionTank, RemainStatus.Full, 3, true));

        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 試薬の取得、設定
        /// </summary>
        public List<ReagentRemainStatusInfoItem<RemainStatus>> Reagent { get; set; }

        /// <summary>
        /// サンプル分注チップ(全ケース)の取得、設定
        /// </summary>
        public List<ReagentRemainStatusInfoItem<RemainStatus>> SamplingTipTotal { get; set; }

        /// <summary>
        /// サンプル分注チップの取得、設定
        /// </summary>
        public List<ReagentRemainStatusInfoItem<RemainStatus>> SamplingTip { get; set; }

        /// <summary>
        /// プレトリガの取得、設定
        /// </summary>
        public List<ReagentRemainStatusInfoItem<RemainStatus>> PretriggerBottle { get; set; }

        /// <summary>
        /// トリガの取得、設定
        /// </summary>
        public List<ReagentRemainStatusInfoItem<RemainStatus>> TriggerBottle { get; set; }

        /// <summary>
        /// 希釈液の取得、設定
        /// </summary>
        public List<ReagentRemainStatusInfoItem<RemainStatus>> DiluentBottle { get; set; }

        /// <summary>
        /// 洗浄液バッファの取得、設定
        /// </summary>
        public List<ReagentRemainStatusInfoItem<RemainStatus>> WashSolution { get; set; }

        /// <summary>
        /// 廃液バッファーの取得、設定
        /// </summary>
        public List<ReagentRemainStatusInfoItem<WasteStatus>> WasteBuffer { get; set; }

        /// <summary>
        /// 廃棄ボックス
        /// </summary>
        public List<ReagentRemainStatusInfoItem<WasteBoxViewStatus>> WasteBox { get; set; }

        /// <summary>
        /// 廃液タンクの取得、設定
        /// </summary>
        public List<ReagentRemainStatusInfoItem<WasteStatus>> WasteTank { get; set; }

        /// <summary>
        /// 洗浄液バッファの取得、設定
        /// </summary>
        public List<ReagentRemainStatusInfoItem<RemainStatus>> WashSolutionTank { get; set; }

        #endregion

        #region [Publicメソッド]
        /// <summary>
        /// 残照ステータス取得処理
        /// </summary>
        /// <param name="kind">試薬種別</param>
        /// <param name="remain">残量</param>
        /// <returns></returns>
        public RemainStatus GetRemainStatus(ReagentKind kind, Int32 remain)
        {
            RemainStatus rtnval = RemainStatus.Empty;
            ReagentRemainStatusInfoItem<RemainStatus> listStatus = null;

            switch (kind)
            {
                case ReagentKind.Pretrigger:
                    listStatus = PretriggerBottle.LastOrDefault(state => state.Remain <= remain);
                    break;
                case ReagentKind.Trigger:
                    listStatus = TriggerBottle.LastOrDefault(state => state.Remain <= remain);
                    break;
                case ReagentKind.Diluent:
                    listStatus = DiluentBottle.LastOrDefault(state => state.Remain <= remain);
                    break;
                case ReagentKind.SamplingTip:
                    listStatus = SamplingTip.LastOrDefault(state => state.Remain <= remain);
                    break;
                case ReagentKind.WashSolutionBuffer:
                    listStatus = WashSolution.LastOrDefault(state => state.Remain <= remain);
                    break;
                case ReagentKind.WashSolutionTank:
                    listStatus = WashSolutionTank.LastOrDefault(state => state.Remain <= remain);
                    break;
            }

            if (listStatus != null)
            {
                rtnval = listStatus.Status;
            }

            return rtnval;
        }

        /// <summary>
        /// 廃液ステータス取得
        /// </summary>
        /// <param name="kind">試薬種別</param>
        /// <param name="remain">満杯フラグ</param>
        /// <param name="isUse">設置有無</param>
        /// <returns></returns>
        public WasteStatus GetWasteStatus(ReagentKind kind, Int32 remain, Boolean isUse)
        {
            WasteStatus rtnval = WasteStatus.None;
            ReagentRemainStatusInfoItem<WasteStatus> listStatus = null;

            switch (kind)
            {
                case ReagentKind.WasteBuffer:
                    listStatus = WasteBuffer.LastOrDefault(state => state.Remain <= remain);
                    break;
                case ReagentKind.WasteTank:
                    listStatus = WasteTank.LastOrDefault(state => state.Remain <= remain && state.IsUse == isUse);
                    break;
            }

            if (listStatus != null)
            {
                rtnval = listStatus.Status;
            }

            return rtnval;
        }

        /// <summary>
        /// 廃棄ボックスステータス取得
        /// </summary>
        /// <param name="kind">試薬種別</param>
        /// <param name="remain">満杯フラグ</param>
        /// <param name="isUse">設置有無</param>
        /// <returns></returns>
        public WasteBoxViewStatus GetWasteBoxViewStatus(ReagentKind kind, Int32 remain, Boolean isUse)
        {
            WasteBoxViewStatus rtnval = WasteBoxViewStatus.None;
            ReagentRemainStatusInfoItem<WasteBoxViewStatus> listStatus = null;

            switch (kind)
            {
                case ReagentKind.WasteBox:
                    listStatus = WasteBox.LastOrDefault(state => state.Remain <= remain && state.IsUse == isUse);
                    break;
            }

            if (listStatus != null)
            {
                rtnval = listStatus.Status;
            }

            return rtnval;
        }
        #endregion
    }

    /// <summary>
    /// 残量別状態クラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReagentRemainStatusInfoItem<T>
        where T : struct
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 状態
        /// </summary>
        private T status;

        /// <summary>
        /// 残量
        /// </summary>
        private Int32 remain;

        /// <summary>
        /// 試薬種別
        /// </summary>
        private ReagentKind reagentKind;

        /// <summary>
        /// 使用状態
        /// </summary>
        private bool isUse;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 状態の取得、設定
        /// </summary>
        public T Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        /// <summary>
        /// 試薬種別の取得、設定
        /// </summary>
        public ReagentKind Kind
        {
            get
            {
                return this.reagentKind;
            }
            set
            {
                this.reagentKind = value;
            }
        }

        /// <summary>
        /// 残量の取得、設定
        /// </summary>
        public Int32 Remain
        {
            get
            {
                return this.remain;
            }
            set
            {
                this.remain = value;
            }
        }

        /// <summary>
        /// 使用状態の取得、設定
        /// </summary>
        public Boolean IsUse
        {
            get
            {
                return this.isUse;
            }
            set
            {
                this.isUse = value;
            }
        }

        #endregion

        #region [コンストラクタ/デストラクタ]
        public ReagentRemainStatusInfoItem(ReagentKind kind, T status, Int32 remain, Boolean isUse)
        {
            Status = status;
            Kind = kind;
            Remain = remain;
            IsUse = isUse;
        }
        #endregion
    }
}
