using System;
using System.Collections.Generic;

namespace Oelco.CarisX.Utility
{
    /// <summary>
    /// サンプリング停止理由ユーティリティ
    /// </summary>
    /// <remarks>
    /// サンプリング停止理由の処理に関連する機能を提供します。
    /// </remarks>
    public class SamplingPauseReason
    {
        #region [定数定義]

        /// <summary>
        /// サンプリング停止理由ビットデフォルト
        /// </summary>
        public const Int64 SAMPLINGPAUSEREASON_DEFAULT = 0;

        /// <summary>
        /// サンプリング停止理由ビット定義
        /// </summary>
        [Flags]
        public enum SamplingPauseReasonBit : long
        {
            /// <summary>
            /// サンプリング停止キーが押された
            /// </summary>
            SamplingStopKeyPressed = 0x00000001,
            /// <summary>
            /// M、R1、R2試薬、前処理液の残量不足
            /// </summary>
            RemainShortage = 0x00000002,
            /// <summary>
            /// 希釈液の液量不足
            /// </summary>
            DiluentRemainShortage = 0x00000004,
            /// <summary>
            /// プレトリガの液量不足
            /// </summary>
            PretriggerRemainShortage = 0x00000008,
            /// <summary>
            /// トリガの液量不足
            /// </summary>
            TriggerRemainShortage = 0x00000010,
            /// <summary>
            /// 洗浄液の液量不足
            /// </summary>
            WashSolutionRemainShortage = 0x00000020,
            /// <summary>
            /// サンプルチップの残量不足
            /// </summary>
            SampleTipShortage = 0x00000040,
            /// <summary>
            /// 反応容器の残量不足
            /// </summary>
            CellShortage = 0x00000080,
            /// <summary>
            /// 廃液ボトルなし
            /// </summary>
            NoWasteBottle = 0x00000100,
            /// <summary>
            /// 廃液ボトル満杯
            /// </summary>
            WasteBottleFull = 0x00000200,
            /// <summary>
            /// 廃液バッファ満杯
            /// </summary>
            WasteBufferFull = 0x00000400,
            /// <summary>
            /// 廃液ボトル未接続
            /// </summary>
            WasteBottleNotConnect = 0x00000800,
            /// <summary>
            /// 廃液ポンプ異常
            /// </summary>
            WastePumpAbnormal = 0x00001000,
            /// <summary>
            /// 廃棄ボックスなし
            /// </summary>
            NoWasteBox = 0x00002000,
            /// <summary>
            /// 廃棄ボックス満杯
            /// </summary>
            WasteBoxFull = 0x00004000,
            /// <summary>
            /// 温度異常
            /// </summary>
            TemperatureAbnormal = 0x00010000,
            /// <summary>
            /// ケース満杯エラー
            /// </summary>
            CaseFullError = 0x00020000,
            /// <summary>
            /// チップ破棄エラー
            /// </summary>
            TipDiscardError = 0x00040000,
            /// <summary>
            /// チューブ有無検出センサーエラー
            /// </summary>
            TubeSensorError = 0x00080000,
            /// <summary>
            /// 希釈初回プライムのため
            /// </summary>
            FirstDiluentPrime = 0x00100000,
            /// <summary>
            /// プレトリガボトルの切り替わり
            /// </summary>
            PretriggerBottleSwitching = 0x00200000,
            /// <summary>
            /// トリガボトルの切り替わり
            /// </summary>
            TriggerBottleSwitching = 0x00400000,
            /// <summary>
            /// M、R1、R2試薬準備のため
            /// </summary>
			PreparationReagent = 0x00800000,
            /// <summary>
            /// 動作異常時
            /// </summary>
            Malfunction = 0x01000000,
            /// <summary>
            /// ラックストレージ満杯
            /// </summary>
            RackStorageFull = 0x02000000,
            /// <summary>
            /// ラックカバーオープン
            /// </summary>
            RackCoverOpen = 0x08000000,
            /// <summary>
            /// 試薬ロットの切り替わり
            /// </summary>
            ReagentLotSwitching = 0x10000000,
            /// <summary>
            /// ラック搬送エラー
            /// </summary>
            RackTransferError = 0x20000000,
            /// <summary>
            /// 希釈準備のため
            /// </summary>
            PreparationDiluent = 0x40000000,
        }

        #endregion

        #region [クラス変数定義]

        /// <summary>
        /// サンプリング停止理由ビット-サンプリング停止理由文字列対応
        /// </summary>
        static protected Dictionary<String, String> SamplingPauseReasonToName = new Dictionary<String, String>()
        {
            {SamplingPauseReasonBit.SamplingStopKeyPressed.ToString()               ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_000},
            {SamplingPauseReasonBit.RemainShortage.ToString()                       ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_001},
            {SamplingPauseReasonBit.DiluentRemainShortage.ToString()                ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_002},
            {SamplingPauseReasonBit.PretriggerRemainShortage.ToString()             ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_003},
            {SamplingPauseReasonBit.TriggerRemainShortage.ToString()                ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_004},
            {SamplingPauseReasonBit.WashSolutionRemainShortage.ToString()           ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_005},
            {SamplingPauseReasonBit.SampleTipShortage.ToString()                    ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_006},
            {SamplingPauseReasonBit.CellShortage.ToString()                         ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_007},
            {SamplingPauseReasonBit.NoWasteBottle.ToString()                        ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_008},
            {SamplingPauseReasonBit.WasteBottleFull.ToString()                      ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_009},
            {SamplingPauseReasonBit.WasteBufferFull.ToString()                      ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_010},
            {SamplingPauseReasonBit.WasteBottleNotConnect.ToString()                ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_011},
            {SamplingPauseReasonBit.WastePumpAbnormal.ToString()                    ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_012},
            {SamplingPauseReasonBit.NoWasteBox.ToString()                           ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_013},
            {SamplingPauseReasonBit.WasteBoxFull.ToString()                         ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_014},
            {SamplingPauseReasonBit.TemperatureAbnormal.ToString()                  ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_015},
            {SamplingPauseReasonBit.CaseFullError.ToString()                        ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_016},
            {SamplingPauseReasonBit.TipDiscardError.ToString()                      ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_023},
            {SamplingPauseReasonBit.TubeSensorError.ToString()                      ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_024},
            {SamplingPauseReasonBit.FirstDiluentPrime.ToString()                    ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_025},
            {SamplingPauseReasonBit.PretriggerBottleSwitching.ToString()            ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_017},
            {SamplingPauseReasonBit.TriggerBottleSwitching.ToString()               ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_018},
            {SamplingPauseReasonBit.PreparationReagent.ToString()                   ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_019},
            {SamplingPauseReasonBit.Malfunction.ToString()                          ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_020},
            {SamplingPauseReasonBit.RackStorageFull.ToString()                      ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_026},
            {SamplingPauseReasonBit.RackCoverOpen.ToString()                        ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_027},
            {SamplingPauseReasonBit.ReagentLotSwitching.ToString()                  ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_021},
            {SamplingPauseReasonBit.RackTransferError.ToString()                    ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_028},
            {SamplingPauseReasonBit.PreparationDiluent.ToString()                   ,Properties.Resources.STRING_SAMPLINGPAUSEREASON_022},
        };

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// サンプリング停止理由値
        /// </summary>
        SamplingPauseReasonBit samplingPauseReasonValue = SAMPLINGPAUSEREASON_DEFAULT;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SamplingPauseReason()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="samplingPauseReason">サンプリング停止理由値</param>
        public SamplingPauseReason( Int64 samplingPauseReason)
        {
            this.samplingPauseReasonValue = (SamplingPauseReasonBit)samplingPauseReason;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="samplingPauseReasonBit">サンプリング停止理由値</param>
        public SamplingPauseReason( SamplingPauseReasonBit samplingPauseReasonBit)
        {
            this.samplingPauseReasonValue = samplingPauseReasonBit;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// サンプリング停止理由値の取得、設定
        /// </summary>
        public Int64 Value
        {
            get
            {
                return (Int64)this.samplingPauseReasonValue;
            }
            set
            {
                this.samplingPauseReasonValue = (SamplingPauseReasonBit)value;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// Int64からの暗黙的変換
        /// </summary>
        /// <remarks>
        /// Int64から暗黙的に変換します。
        /// </remarks>
        /// <param name="value">変換元Int64</param>
        /// <returns>変換後SamplingPauseReason</returns>
        public static implicit operator SamplingPauseReason( Int64 value )
        {
            return new SamplingPauseReason( value );
        }

        /// <summary>
        /// Int64への暗黙的変換
        /// </summary>
        /// <remarks>
        /// Int64へ暗黙的に変換します。
        /// </remarks>
        /// <param name="samplingpausereason">変換元SamplingPauseReason</param>
        /// <returns>変換後Int64</returns>
        public static implicit operator Int64(SamplingPauseReason samplingpausereason)
        {
            if (samplingpausereason != null )
            {
                return samplingpausereason.Value;
            }
            return SAMPLINGPAUSEREASON_DEFAULT;
        }

        /// <summary>
        /// SamplingPauseReasonBitからの暗黙的変換
        /// </summary>
        /// <remarks>
        /// SamplingPauseReasonBitから暗黙的に変換します。
        /// </remarks>
        /// <param name="samplingpausereasonbit">変換元SamplingPauseReasonBit</param>
        /// <returns>SamplingPauseReason</returns>
        public static implicit operator SamplingPauseReason( SamplingPauseReasonBit samplingpausereasonbit)
        {
            return new SamplingPauseReason(samplingpausereasonbit);
        }

        /// <summary>
        /// SamplingPauseReasonBitへの暗黙的変換
        /// </summary>
        /// <remarks>
        /// SamplingPauseReasonBitへ暗黙的に変換します。
        /// </remarks>
        /// <param name="samplingpausereason">変換元SamplingPauseReason</param>
        /// <returns>変換後SamplingPauseReasonBit</returns>
        public static implicit operator SamplingPauseReasonBit(SamplingPauseReason samplingpausereason)
        {
            if (samplingpausereason != null )
            {
                return samplingpausereason.samplingPauseReasonValue;
            }
            return SAMPLINGPAUSEREASON_DEFAULT;
        }

        /// <summary>
        /// サンプリング停止理由名称文文字列配列取得
        /// </summary>
        /// <remarks>
        /// 現在クラスが保持しているサンプリング停止理由値から、
        /// サンプリング停止理由名称の文字列配列を作成します。
        /// </remarks>
        /// <returns>サンプリング停止理由名称文字列配列</returns>
        public String[] GetSamplingStopReasonNameStrings()
        {
            // サンプリング停止理由文字列配列作成
            String[] str = this.samplingPauseReasonValue == 0 ? new String[0] : this.samplingPauseReasonValue.ToString().Split( new String[] { ", " }, StringSplitOptions.None );
            List<String> samplingPauseReasonCharStr = new List<String>();
            foreach ( String samplingPauseReasonEnumName in str )
            {
                samplingPauseReasonCharStr.Add( SamplingPauseReasonToName[samplingPauseReasonEnumName] );
            }
            return samplingPauseReasonCharStr.ToArray();
        }

        #endregion

    }
}
