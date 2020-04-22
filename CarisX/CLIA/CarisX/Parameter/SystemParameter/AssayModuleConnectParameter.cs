using System;

namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// 分析モジュール接続台数
    /// </summary>
	public class AssayModuleConnectParameter : AttachmentParameter
	{
        #region [プロパティ]

        /// <summary>
        /// 分析モジュール接続台数の処理の設定／取得
        /// </summary>
        public Int32 NumOfConnected { get; set; } = 1;

        #endregion
	}
	 
}
 
