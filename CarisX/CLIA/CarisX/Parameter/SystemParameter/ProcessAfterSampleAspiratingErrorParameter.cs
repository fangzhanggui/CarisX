using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// 検体吸引エラー後の処理
    /// </summary>
	public class ProcessAfterSampleAspiratingErrorParameter : AttachmentParameter
	{
        #region [定数定義]

        /// <summary>
        /// 目詰まり検知後の処理　吐き戻し
        /// </summary>
        public const Boolean PLUG_PUTBACK = true;
        /// <summary>
        /// 目詰まり検知後の処理　続行
        /// </summary>
        public const Boolean PLUG_CONTINUE = false;
        /// <summary>
        /// 半量吸い検知後の処理　吐き戻し
        /// </summary>
        public const Boolean HALF_PUTBACK = true;
        /// <summary>
        /// 半量吸い検知後の処理　続行
        /// </summary>
        public const Boolean HALF_CONTINUE = false;


        /// <summary>
        /// エラー発生後の処理（目詰まり検知）　デフォルト値設定
        /// </summary>
        public const Boolean ERR_PROC_PLUG_DEFAULT = PLUG_PUTBACK;
        /// <summary>
        /// エラー発生後の処理（半量吸い検知）　デフォルト値設定
        /// </summary>
        public const Boolean ERR_PROC_HALF_DEFAULT = HALF_PUTBACK;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 目詰まり検知後の処理
        /// </summary>
        private Boolean usablePutBack = ERR_PROC_PLUG_DEFAULT;

        /// <summary>
        /// 半量吸い検知後の処理
        /// </summary>
        private Boolean usablePutBackAtHalf = ERR_PROC_HALF_DEFAULT;
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 目詰まり検知後の処理の設定／取得
        /// </summary>
        public Boolean UsablePutBack
        {
            get
            {
                return this.usablePutBack;
            }
            set
            {
                this.usablePutBack = value;
            }
        }

        /// <summary>
        /// 半量吸い検知後の処理の設定／取得
        /// </summary>
        public Boolean UsablePutBackAtHalf
        {
            get
            {
                return this.usablePutBackAtHalf;
            }
            set
            {
                this.usablePutBackAtHalf = value;
            }
        }

        #endregion
	}
	 
}
 
