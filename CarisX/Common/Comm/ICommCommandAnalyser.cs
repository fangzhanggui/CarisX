using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.Common.Comm
{
    /// <summary>
    /// コマンド解析インターフェース
    /// </summary>
    public interface ICommCommandAnalyser
    {
        /// <summary>
        /// 通信コマンド解析
        /// </summary>
        /// <remarks>
        /// 文字列から通信コマンドへ解析します。
        /// </remarks>
        /// <param name="target">コマンド文字列</param>
        /// <returns>通信コマンド</returns>
        CommCommand AnalyseCommand( String target );
    }
}
