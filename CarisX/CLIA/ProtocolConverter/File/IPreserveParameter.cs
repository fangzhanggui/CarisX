using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolConverter.File
{
    /// <summary>
    /// 保存パラメータインターフェース
    /// </summary>
    /// <remarks>
    /// 保存を行うパラメータに関係する操作を定義します。
    /// </remarks>
    public interface IPreserveParameter
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <remarks>
        /// パラメータ保存処理を実装します。
        /// </remarks>
        /// <returns>True:成功 False:失敗</returns>
        Boolean Save();

    }
}
