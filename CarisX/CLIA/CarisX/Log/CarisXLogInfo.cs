using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Log;

namespace Oelco.CarisX.Log
{
    /// <summary>
    /// 履歴情報クラス
    /// </summary>
    public class CarisXLogInfo : LogInfo
    {
        /// <summary>
        /// 履歴の取得、設定
        /// </summary>
        public CarisXLogInfoBaseExtention OptionalValue
        {
            get;
            set;
        }

        public Int32 ModuleNo { get; set; } = 1;
        
        /// <summary>
        /// コピー関数
        /// </summary>
        public CarisXLogInfo Clone()
        {
            return (CarisXLogInfo)this.MemberwiseClone();
        }
    }

    /// <summary>
    /// 履歴追加
    /// </summary>
    public class CarisXLogInfoBaseExtention
    {
        /// <summary>
        /// 空履歴
        /// </summary>
        static private CarisXLogInfoBaseExtention emptyData = new CarisXLogInfoBaseExtention();

        /// <summary>
        /// 空履歴の取得
        /// </summary>
        public static CarisXLogInfoBaseExtention Empty
        {
            get
            {
                return CarisXLogInfoBaseExtention.emptyData;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CarisXLogInfoBaseExtention()
        {
        }
    }

    /// <summary>
    /// エラー履歴用追加オプション値
    /// </summary>
    /// <remarks>
    /// エラー履歴生成の際に使用します。
    /// </remarks>
    public class CarisXLogInfoErrorLogExtention : CarisXLogInfoBaseExtention
    {
        /// <summary>
        /// エラー発生時刻
        /// </summary>
        public DateTime WriteTime = new DateTime();
        /// <summary>
        /// エラーコード
        /// </summary>
        public Int32 ErrorCode = 0;
        /// <summary>
        /// 引数
        /// </summary>
        public Int32 ErrorArg = 0;
        /// <summary>
        /// 連続発生回数
        /// </summary>
        public Int32 Counter = 1;
    }

}
