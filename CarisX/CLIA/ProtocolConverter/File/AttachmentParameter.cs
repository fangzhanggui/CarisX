using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolConverter.File
{
    /// <summary>
    /// 有効無効切替え可能パラメータ
    /// </summary>
    /// <remarks>
    /// 有効無効状態を持つパラメータクラスに継承されます。
    /// </remarks>
    public class AttachmentParameter
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// パラメータの有効状態
        /// </summary>
        Boolean enable = false;

        #endregion
        
        #region [プロパティ]

        /// <summary>
        /// パラメータの有効状態を取得／設定
        /// </summary>
        public Boolean Enable
        {
            get
            {
                return enable;
            }
            set
            {
                enable = value;
            }
        }

        #endregion
        
    }
}
