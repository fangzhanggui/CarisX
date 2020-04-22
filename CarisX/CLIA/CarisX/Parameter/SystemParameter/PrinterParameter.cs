using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// プリンタ設定
    /// </summary>
	public class PrinterParameter : AttachmentParameter
	{
        #region [定数定義]

        /// <summary>
        /// プリンタ設定　デフォルト値
        /// </summary>
        public const Boolean PRINTER_SET_DEFAULT = false;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// リアルタイム印刷有無
        /// </summary>
        private Boolean usableRealtimePrint = PRINTER_SET_DEFAULT;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// リアルタイム印刷有無の設定／取得
        /// </summary>
        public Boolean UsableRealtimePrint
        {
            get
            {
                return this.usableRealtimePrint;
            }
            set
            {
                this.usableRealtimePrint = value;
            }
        }

        #endregion
	}
	 
}
 
