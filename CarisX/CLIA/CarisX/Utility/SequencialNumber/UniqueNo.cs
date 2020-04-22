using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;


namespace Oelco.CarisX.Utility
{

    // ○ユニーク番号について
    // 　CarisXの処理能力はは1時間200テストなので、
    // 　仮に24時間365日フル稼働(=1752000テスト/年)を続けたとしても
    // 　Int32範囲(最大2147483647)でユニーク番号が枯渇する事は無く、常に一意を保つ。
    //   ユーザにより登録と削除を相当数繰り返される場合、この番号は枯渇の可能性がある。

    /// <summary>
    /// ユニーク番号
    /// </summary>
    /// <remarks>
    /// 1検体の1分析項目毎に、分析実施のタイミングで発番されます。
    /// 全ての分析に対してユニークとなります。
    /// </remarks>
    class UniqueNo : NumberingBase
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
            //this.EndCount = Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.UniqueNoInfo.CountMax;
            //this.StartCount = Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.UniqueNoInfo.CountMin;

            this.EndCount = Int32.MaxValue-1;
            this.StartCount = 1;

            this.Number = Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.UniqueNoInfo.CountNow;
            this.LatestNumberingDate = Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.UniqueNoInfo.LatestNumberDate;

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
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.UniqueNoInfo.CountNow = this.Number;
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.UniqueNoInfo.LatestNumberDate = this.LatestNumberingDate;

            // アプリケーションのフリーズ等発生した時も保持されているよう、即座に保存を行う。
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Save(); 
        }

        #endregion
    }
}
