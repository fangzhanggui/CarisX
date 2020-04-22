//----------------------------------------------------------------
// Class.
//	         GlobalParameter.cs
// Info.
//   システム共通変数定義
// History
//   2012/05/09  Ver1.00.00  新規作成   Hiroyasu Matsumoto
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oelco.Common.Parameter
{
    /// <summary>
    /// システム共通変数
    /// </summary>
    /// <remarks>
    /// 全システムで共有可能な変数を定義します。
    /// </remarks>
    public class GlobalParameter
    {
        /// <summary>
        /// 自アプリケーション種別
        /// </summary>
        /// <remarks>出荷済みのCarisXへの影響がないように、デフォルトをCarisXとする。</remarks>
        static public ApplicationKind myApplicationKind = ApplicationKind.CarisX;

        /// <summary>
        /// 自アプリケーション種別
        /// </summary>
        public enum ApplicationKind : int
        {
            /// <summary>
            /// CarisX
            /// </summary>
            CarisX = 0,
            /// <summary>
            /// NS-Prime (AFT)
            /// </summary>
            NS_Prime
        }

    }
}
