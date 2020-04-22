using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// 試薬ロット切替わり時の処理設定
    /// </summary>
	public class ProcessAtReagentLotChange : AttachmentParameter
	{
        #region [定数定義]

        /// <summary>
        /// 試薬ロット切り替え 停止
        /// </summary>
        public const Boolean STOP = true;
        /// <summary>
        /// 試薬ロット切り替え 続行
        /// </summary>
        public const Boolean CONTINUE = false;

        /// <summary>
        /// 試薬ロット切り替え処理　デフォルト値設定
        /// </summary>
        public const Boolean REAG_LOT_PROC_DEFAULT = STOP;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 試薬ロット切り替わり時の処理
        /// </summary>
        private Boolean reagLotChangeProc = REAG_LOT_PROC_DEFAULT;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 試薬ロット切り替わり時の処理の設定／取得
        /// </summary>
        public Boolean ReagLotChangeProc
        {
            get
            {
                return this.reagLotChangeProc;
            }
            set
            {
                this.reagLotChangeProc = value;
            }
        }

        #endregion
	}
	 
}
 
