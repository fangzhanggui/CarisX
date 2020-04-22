using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// 希釈液不足時の分析状態設定
    /// </summary>
	public class ProcessAtDiluentShortageParameter : AttachmentParameter
	{
        #region [定数定義]

        /// <summary>
        /// 分析の状態 中断
        /// </summary>
        public const Boolean SAMPLING_STOP = true;
        /// <summary>
        /// 分析の状態 続行
        /// </summary>
        public const Boolean CONTINUE = false;

        /// <summary>
        /// 分析の状態　デフォルト値設定
        /// </summary>
        public const Boolean DILU_ASSAY_STATUS_DEFAULT = SAMPLING_STOP;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 希釈液不足時の分析の状態
        /// </summary>
        private Boolean diluShortAssayStatus = DILU_ASSAY_STATUS_DEFAULT;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 希釈液不足時の分析の状態の設定／取得
        /// </summary>
        public Boolean DiluShortAssayStatus
        {
            get
            {
                return this.diluShortAssayStatus;
            }
            set
            {
                this.diluShortAssayStatus = value;
            }
        }

        #endregion
	}
	 
}
 
