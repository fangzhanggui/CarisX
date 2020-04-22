using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;

namespace Oelco.CarisX.Utility
{
    /// <summary>
    /// 個体識別番号
    /// </summary>
    /// <remarks>
    /// 検体の分析時、スレーブからの測定指示データ問合せのタイミングで発番を行います。
    /// この番号は再検査の場合、新規に発番を行いません。
    /// </remarks>
    public class IndividuallyNo : NumberingBase
    {
        #region [publicメソッド]

        /// <summary>
        /// データ初期化
        /// </summary>
        /// <remarks>
        /// 設定クラスから受付番号情報を取得し、
        /// 受付番号クラスの情報を初期化します。
        /// </remarks>
        public override void Initialize()
        {
            // 設定クラスから情報を取得
            //this.EndCount = Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.IndividuallyNoInfo.CountMax;
            //this.StartCount = Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.IndividuallyNoInfo.CountMin;

            this.EndCount = Int32.MaxValue - 1;
            this.StartCount = 1;

            this.Number = Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.IndividuallyNoInfo.CountNow;
            this.LatestNumberingDate = Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.IndividuallyNoInfo.LatestNumberDate;

            base.Initialize();
        }

        #endregion


        #region [protectedメソッド]

        /// <summary>
        /// インクリメント
        /// </summary>
        /// <remarks>
        /// 受付番号のインクリメントを行います
        /// </remarks>
        protected override void increment()
        {
            base.increment();

            // 設定クラスへ情報を設定
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.IndividuallyNoInfo.CountNow = this.Number;
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.IndividuallyNoInfo.LatestNumberDate = this.LatestNumberingDate;

            // アプリケーションのフリーズ等発生した時も保持されているよう、即座に保存を行う。
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Save(); 
        }

        #endregion
    }
}
