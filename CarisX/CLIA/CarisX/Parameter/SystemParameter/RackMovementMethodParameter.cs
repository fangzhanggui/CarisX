using Oelco.CarisX.Const;

namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// ラック移動方式
    /// </summary>
	public class RackMovementMethodParameter : AttachmentParameter
	{
        #region [プロパティ]

        /// <summary>
        /// ラック移動方式の設定／取得
        /// </summary>
        public RackMovementMethodKind RackMovementMethod { get; set; } = RackMovementMethodKind.Performance;

        #endregion
	}
	 
}
 
