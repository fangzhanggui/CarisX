using System;

namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// 有効無効切替え可能パラメータ
    /// </summary>
	public class AttachmentParameter
	{
        /// <summary>
        /// パラメータ有効状態
        /// </summary>
        Boolean enable;

        /// <summary>
        /// パラメータ有効状態
        /// </summary>
        public Boolean Enable
        {
            get
            {
                return this.enable;
            }
            set
            {
                this.enable = value;
            }
        }
        
		 
	}
	 
}
 
