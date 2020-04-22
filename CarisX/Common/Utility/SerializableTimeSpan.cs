using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Oelco.Common.Utility
{
    /// <summary>
    /// 時間間隔(シリアライズ対応版)
    /// </summary>
    public class SerializableTimeSpan
    {
        /// <summary>
        /// 値を示すタイマー刻みの数
        /// </summary>
        private Int64 ticks = 0;

        /// <summary>
        /// 秒数の取得、設定
        /// </summary>
        public Int64 Value
        {
            get
            {
                return (Int64)this.TimeSpan.TotalSeconds;
            }
            set
            {
                this.TimeSpan = new TimeSpan(0,0,(Int32)value);
            }
        }

        #region [コンストラクタ/デストラクタ]
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SerializableTimeSpan()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ticks">タイマー刻みの数</param>
        public SerializableTimeSpan( Int64 ticks )
        {
            this.ticks = ticks;
        }

        #endregion
        
        /// <summary>
        /// 時間間隔
        /// </summary>
        [XmlIgnore()]
        public TimeSpan TimeSpan
        {
            get
            {
                return this;
            }
            set
            {
                this.ticks = value.Ticks;
            }
        }
        /// <summary>
        /// TimeSpan型からの暗黙的変換
        /// </summary>
        /// <remarks>
        /// TimeSpan方から暗黙的に変換します。
        /// </remarks>
        /// <param name="value">変換元TimeSpan</param>
        /// <returns>変換結果</returns>
        public static implicit operator SerializableTimeSpan( TimeSpan value )
        {
            return new SerializableTimeSpan( value.Ticks );
        }

        /// <summary>
        /// TimeSpan型への暗黙的変換
        /// </summary>
        /// <remarks>
        /// TimeSpan方へ暗黙的に変換します。
        /// </remarks>
        /// <param name="value">変換元SerializableTimeSpan</param>
        /// <returns>変換結果</returns>
        public static implicit operator TimeSpan( SerializableTimeSpan value )
        {
            return new TimeSpan( value.ticks );
        }
        /// <summary>
        /// 1nt64への暗黙的変換
        /// </summary>
        /// <remarks>
        /// Int64へ暗黙的に変換します。
        /// </remarks>
        /// <param name="value">変換元SerializableTimeSpan</param>
        /// <returns>変換結果</returns>
        public static implicit operator Int64( SerializableTimeSpan value )
        {
            return value.ticks;
        }

    }
}
