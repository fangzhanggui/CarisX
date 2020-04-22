using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolConverter
{
    /// <summary>
    /// シングルトンクラス
    /// </summary>
    /// <remarks>
    /// 任意のクラスをシングルトンクラスとして生成する。
    /// 1種類のクラスにつき1つのインスタンスを保持する。
    /// </remarks>
    /// <typeparam name="T">
    /// シングルトン適用型
    /// ・クラスであること
    /// ・デフォルトコンストラクタを持つこと
    /// </typeparam>
    /// <example>
    /// Singleton<適用クラス名>.Instance.xxx
    /// </example>
    public class Singleton<T> where T : class, new()
    {
        /// <summary>
        /// 対象インスタンス
        /// </summary>
        static private T instance = null;
        /// <summary>
        /// 対象インスタンス取得
        /// </summary>
        static public T Instance
        {
            get
            {
                // 初回呼び出しの際にnewを行う。
                if ( instance == null )
                {
                    instance = new T();
                }

                return instance;
            }
        }
    }
}

