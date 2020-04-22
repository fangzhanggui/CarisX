using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.Utility
{
    /// <summary>
    /// シーケンス番号（一般検体）
    /// </summary>
    /// <remarks>
    /// 一般検体の分析時、スレーブからの測定指示データ問合せのタイミングで発番を行います。
    /// この番号は再検査の場合、新規に発番を行いません。
    /// </remarks>
    public class SequencialSampleNo : NumberingBase
    {
        #region [publicメソッド]

        /// <summary>
        /// データ初期化
        /// </summary>
        /// <remarks>
        /// 設定クラスから検体番号情報を取得し、
        /// 検体番号クラスの情報を初期化します。
        /// </remarks>
        public override void Initialize()
        {
            // 設定クラスから情報を取得
            //this.EndCount = Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SequencialSampleNoInfo.CountMax;
            //this.StartCount = Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SequencialSampleNoInfo.CountMin;

            this.EndCount = CarisXConst.SEQUENCE_NO_MAX;
            this.StartCount = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HowToCreateSequenceNoParameter.StartSeqNoPat;

            this.Number = Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SequencialSampleNoInfo.CountNow;
            this.LatestNumberingDate = Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SequencialSampleNoInfo.LatestNumberDate;

            base.Initialize();
        }
        #endregion


        #region [protectedメソッド]

        public override void ResetNumber()
        {
            base.ResetNumber();
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SequencialSampleNoInfo.CountNow = this.Number;
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SequencialSampleNoInfo.LatestNumberDate = this.LatestNumberingDate;
        }

        /// <summary>
        /// インクリメント
        /// </summary>
        /// <remarks>
        /// 検体番号のインクリメントを行います
        /// </remarks>
        protected override void increment()
        {
            base.increment();

            // 設定クラスへ情報を設定
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SequencialSampleNoInfo.CountNow = this.Number;
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SequencialSampleNoInfo.LatestNumberDate = this.LatestNumberingDate;

            // アプリケーションのフリーズ等発生した時も保持されているよう、即座に保存を行う。
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Save(); 
        }

        #endregion
    }
}
