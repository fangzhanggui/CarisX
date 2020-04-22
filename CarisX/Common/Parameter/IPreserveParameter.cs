using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.Common.Parameter
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

        /// <summary>
        /// 読込
        /// </summary>
        /// <remarks>
        /// パラメータ読込処理を実装します。
        /// </remarks>
        /// <returns>True:成功 False:失敗</returns>
        Boolean Load();
    }
}
