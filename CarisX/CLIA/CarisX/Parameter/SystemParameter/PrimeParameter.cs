using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// プライム設定
    /// </summary>
	public class PrimeParameter : AttachmentParameter
	{
        #region [定数定義]

        /// <summary>
        /// 希釈液プライム回数　最小値設定
        /// </summary>
        public const Int32 PRIME_COUNT_DILU_MIN = 1;
        /// <summary>
        /// 希釈液プライム回数　最大値設定
        /// </summary>
        public const Int32 PRIME_COUNT_DILU_MAX = 99;
        /// <summary>
        /// R1プライム回数　最小値設定
        /// </summary>
        public const Int32 PRIME_COUNT_R1_MIN = 1;
        /// <summary>
        /// R1プライム回数　最大値設定
        /// </summary>
        public const Int32 PRIME_COUNT_R1_MAX = 99;
        /// <summary>
        /// R2プライム回数　最小値設定
        /// </summary>
        public const Int32 PRIME_COUNT_R2_MIN = 1;
        /// <summary>
        /// R2プライム回数　最大値設定
        /// </summary>
        public const Int32 PRIME_COUNT_R2_MAX = 99;
        /// <summary>
        /// B/F1プライム回数　最小値設定
        /// </summary>
        public const Int32 PRIME_COUNT_BF1_MIN = 1;
        /// <summary>
        /// B/F1プライム回数　最大値設定
        /// </summary>
        public const Int32 PRIME_COUNT_BF1_MAX = 99;
        /// <summary>
        /// B/F2プライム回数　最小値設定
        /// </summary>
        public const Int32 PRIME_COUNT_BF2_MIN = 1;
        /// <summary>
        /// B/F2プライム回数　最大値設定
        /// </summary>
        public const Int32 PRIME_COUNT_BF2_MAX = 99;
        /// <summary>
        /// プレトリガプライム回数　最小値設定
        /// </summary>
        public const Int32 PRIME_COUNT_PRE_TRIG_MIN = 1;
        /// <summary>
        /// プレトリガプライム回数　最大値設定
        /// </summary>
        public const Int32 PRIME_COUNT_PRE_TRIG_MAX = 99;
        /// <summary>
        /// トリガプライム回数　最小値設定
        /// </summary>
        public const Int32 PRIME_COUNT_TRIG_MIN = 1;
        /// <summary>
        /// トリガプライム回数　最大値設定
        /// </summary>
        public const Int32 PRIME_COUNT_TRIG_MAX = 99;

        /// <summary>
        /// 希釈液プライム量　最小値設定（μL）
        /// </summary>
        public const Int32 PRIME_VOLUME_DILU_MIN = 0;
        /// <summary>
        /// 希釈液プライム量　最大値設定（μL）
        /// </summary>
        public const Int32 PRIME_VOLUME_DILU_MAX = 9999;
        /// <summary>
        /// R1プライム量　最小値設定（μL）
        /// </summary>
        public const Int32 PRIME_VOLUME_R1_MIN = 0;
        /// <summary>
        /// R1プライム量　最大値設定（μL）
        /// </summary>
        public const Int32 PRIME_VOLUME_R1_MAX = 9999;
        /// <summary>
        /// R2プライム量　最小値設定（μL）
        /// </summary>
        public const Int32 PRIME_VOLUME_R2_MIN = 0;
        /// <summary>
        /// R2プライム量　最大値設定（μL）
        /// </summary>
        public const Int32 PRIME_VOLUME_R2_MAX = 9999;
        /// <summary>
        /// B/F1プライム量　最小値設定（μL）
        /// </summary>
        public const Int32 PRIME_VOLUME_BF1_MIN = 0;
        /// <summary>
        /// B/F1プライム量　最大値設定（μL）
        /// </summary>
        public const Int32 PRIME_VOLUME_BF1_MAX = 9999;
        /// <summary>
        /// B/F2プライム量　最小値設定（μL）
        /// </summary>
        public const Int32 PRIME_VOLUME_BF2_MIN = 0;
        /// <summary>
        /// B/F2プライム量　最大値設定（μL）
        /// </summary>
        public const Int32 PRIME_VOLUME_BF2_MAX = 9999;
        /// <summary>
        /// プレトリガプライム量　最小値設定（μL）
        /// </summary>
        public const Int32 PRIME_VOLUME_PRE_TRIG_MIN = 0;
        /// <summary>
        /// プレトリガプライム量　最大値設定（μL）
        /// </summary>
        public const Int32 PRIME_VOLUME_PRE_TRIG_MAX = 9999;
        /// <summary>
        /// トリガプライム量　最小値設定（μL）
        /// </summary>
        public const Int32 PRIME_VOLUME_TRIG_MIN = 0;
        /// <summary>
        /// トリガプライム量　最大値設定（μL）
        /// </summary>
        public const Int32 PRIME_VOLUME_TRIG_MAX = 9999;

        /// <summary>
        /// 希釈液プライム回数　デフォルト値設定
        /// </summary>
        public const Int32 PRIME_COUNT_DILU_DEFAULT = PRIME_COUNT_DILU_MIN;
        /// <summary>
        /// R1プライム回数　デフォルト値設定
        /// </summary>
        public const Int32 PRIME_COUNT_R1_DEFAULT = PRIME_COUNT_R1_MIN;
        /// <summary>
        /// R2プライム回数　デフォルト値設定
        /// </summary>
        public const Int32 PRIME_COUNT_R2_DEFAULT = PRIME_COUNT_R2_MIN;
        /// <summary>
        /// B/F1プライム回数　デフォルト値設定
        /// </summary>
        public const Int32 PRIME_COUNT_BF1_DEFAULT = PRIME_COUNT_BF1_MIN;
        /// <summary>
        /// B/F2プライム回数　デフォルト値設定
        /// </summary>
        public const Int32 PRIME_COUNT_BF2_DEFAULT = PRIME_COUNT_BF2_MIN;
        /// <summary>
        /// プレトリガプライム回数　デフォルト値
        /// </summary>
        public const Int32 PRIME_COUNT_PRE_TRIG_DEFAULT = PRIME_COUNT_PRE_TRIG_MIN;
        /// <summary>
        /// トリガプライム回数　デフォルト値設定
        /// </summary>
        public const Int32 PRIME_COUNT_TRIG_DEFAULT = PRIME_COUNT_TRIG_MIN;
        /// <summary>
        /// 希釈液プライム量　デフォルト値設定（μL）
        /// </summary>
        public const Int32 PRIME_VOLUME_DILU_DEFAULT = PRIME_VOLUME_DILU_MIN;
        /// <summary>
        /// R1プライム量　デフォルト値設定（μL）
        /// </summary>
        public const Int32 PRIME_VOLUME_R1_DEFAULT = PRIME_VOLUME_R1_MIN;
        /// <summary>
        /// R2プライム量　デフォルト値設定（μL）
        /// </summary>
        public const Int32 PRIME_VOLUME_R2_DEFAULT = PRIME_VOLUME_R2_MIN;
        /// <summary>
        /// B/F1プライム量　デフォルト値設定（μL）
        /// </summary>
        public const Int32 PRIME_VOLUME_BF1_DEFAULT = PRIME_VOLUME_BF1_MIN;
        /// <summary>
        /// B/F2プライム量　デフォルト値設定（μL）
        /// </summary>
        public const Int32 PRIME_VOLUME_BF2_DEFAULT = PRIME_VOLUME_BF2_MIN;
        /// <summary>
        /// プレトリガプライム量　デフォルト値設定（μL）
        /// </summary>
        public const Int32 PRIME_VOLUME_PRE_TRIG_DEFAULT = PRIME_VOLUME_PRE_TRIG_MIN;
        /// <summary>
        /// トリガプライム量　デフォルト値設定（μL）
        /// </summary>
        public const Int32 PRIME_VOLUME_TRIG_DEFAULT = PRIME_VOLUME_TRIG_MIN;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 希釈液プライム回数
        /// </summary>
        private Int32 primeCountDilu = PRIME_COUNT_DILU_DEFAULT;
        /// <summary>
        /// R1プライム回数
        /// </summary>
        private Int32 primeCountR1 = PRIME_COUNT_R1_DEFAULT;
        /// <summary>
        /// R2プライム回数
        /// </summary>
        private Int32 primeCountR2 = PRIME_COUNT_R2_DEFAULT;
        /// <summary>
        /// B/F1プライム回数
        /// </summary>
        private Int32 primeCountBF1 = PRIME_COUNT_BF1_DEFAULT;
        /// <summary>
        /// B/F2プライム回数
        /// </summary>
        private Int32 primeCountBF2 = PRIME_COUNT_BF2_DEFAULT;
        /// <summary>
        /// プレトリガプライム回数
        /// </summary>
        private Int32 primeCountPreTrig = PRIME_COUNT_PRE_TRIG_DEFAULT;
        /// <summary>
        /// トリガプライム回数
        /// </summary>
        private Int32 primeCountTrig = PRIME_COUNT_TRIG_DEFAULT;

        /// <summary>
        /// 希釈液プライム量（μL）
        /// </summary>
        private Int32 primeVolumeDilu = PRIME_VOLUME_DILU_DEFAULT;
        /// <summary>
        /// R1プライム量（μL）
        /// </summary>
        private Int32 primeVolumeR1 = PRIME_VOLUME_R1_DEFAULT;
        /// <summary>
        /// R2プライム量（μL）
        /// </summary>
        private Int32 primeVolumeR2 = PRIME_VOLUME_R2_DEFAULT;
        /// <summary>
        /// B/F1プライム量（μL）
        /// </summary>
        private Int32 primeVolumeBF1 = PRIME_VOLUME_BF1_DEFAULT;
        /// <summary>
        /// B/F2プライム量（μL）
        /// </summary>
        private Int32 primeVolumeBF2 = PRIME_VOLUME_BF2_DEFAULT;
        /// <summary>
        /// プレトリガプライム量（μL）
        /// </summary>
        private Int32 primeVolumePreTrig = PRIME_VOLUME_PRE_TRIG_DEFAULT;
        /// <summary>
        /// トリガプライム量（μL）
        /// </summary>
        private Int32 primeVolumeTrig = PRIME_VOLUME_TRIG_DEFAULT;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 希釈プライム回数の設定／取得
        /// </summary>
        public Int32 PrimeCountDilu
        {
            get
            {
                return this.primeCountDilu;
            }
            set
            {
                this.primeCountDilu = value;
            }
        }

        /// <summary>
        /// R1プライム回数の設定／取得
        /// </summary>
        public Int32 PrimeCountR1
        {
            get
            {
                return this.primeCountR1;
            }
            set
            {
                this.primeCountR1 = value;
            }
        }

        /// <summary>
        /// R2プライム回数の設定／取得
        /// </summary>
        public Int32 PrimeCountR2
        {
            get
            {
                return this.primeCountR2;
            }
            set
            {
                this.primeCountR2 = value;
            }
        }

        /// <summary>
        /// B/F1プライム回数の設定／取得
        /// </summary>
        public Int32 PrimeCountBF1
        {
            get
            {
                return this.primeCountBF1;
            }
            set
            {
                this.primeCountBF1 = value;
            }
        }

        /// <summary>
        /// B/F2プライム回数の設定／取得
        /// </summary>
        public Int32 PrimeCountBF2
        {
            get
            {
                return this.primeCountBF2;
            }
            set
            {
                this.primeCountBF2 = value;
            }
        }

        /// <summary>
        /// プレトリガプライム回数の設定／取得
        /// </summary>
        public Int32 PrimeCountPreTrig
        {
            get
            {
                return this.primeCountPreTrig;
            }
            set
            {
                this.primeCountPreTrig = value;
            }
        }

        /// <summary>
        /// トリガプライム回数の設定／取得
        /// </summary>
        public Int32 PrimeCountTrig
        {
            get
            {
                return this.primeCountTrig;
            }
            set
            {
                this.primeCountTrig = value;
            }
        }

        /// <summary>
        /// 希釈プライム量の設定／取得
        /// </summary>
        public Int32 PrimeVolumeDilu
        {
            get
            {
                return this.primeVolumeDilu;
            }
            set
            {
                this.primeVolumeDilu = value;
            }
        }

        /// <summary>
        /// R1プライム量の設定／取得
        /// </summary>
        public Int32 PrimeVolumeR1
        {
            get
            {
                return this.primeVolumeR1;
            }
            set
            {
                this.primeVolumeR1 = value;
            }
        }

        /// <summary>
        /// R2プライム量の設定／取得
        /// </summary>
        public Int32 PrimeVolumeR2
        {
            get
            {
                return this.primeVolumeR2;
            }
            set
            {
                this.primeVolumeR2 = value;
            }
        }

        /// <summary>
        /// B/F1プライム量の設定／取得
        /// </summary>
        public Int32 PrimeVolumeBF1
        {
            get
            {
                return this.primeVolumeBF1;
            }
            set
            {
                this.primeVolumeBF1 = value;
            }
        }

        /// <summary>
        /// B/F2プライム量の設定／取得
        /// </summary>
        public Int32 PrimeVolumeBF2
        {
            get
            {
                return this.primeVolumeBF2;
            }
            set
            {
                this.primeVolumeBF2 = value;
            }
        }

        /// <summary>
        /// プレトリガプライム量の設定／取得
        /// </summary>
        public Int32 PrimeVolumePreTrig
        {
            get
            {
                return this.primeVolumePreTrig;
            }
            set
            {
                this.primeVolumePreTrig = value;
            }
        }

        /// <summary>
        /// トリガプライム量の設定／取得
        /// </summary>
        public Int32 PrimeVolumeTrig
        {
            get
            {
                return this.primeVolumeTrig;
            }
            set
            {
                this.primeVolumeTrig = value;
            }
        }

        #endregion
	}
	 
}
 
