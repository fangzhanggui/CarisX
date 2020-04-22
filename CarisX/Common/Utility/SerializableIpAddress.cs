using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Serialization;

namespace Oelco.Common.Utility
{

    /// <summary>
    /// シリアル化IPアドレス
    /// </summary>
    public class SerializableIpAddress
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// IPアドレス
        /// </summary>
        private IPAddress address;

        /// <summary>
        /// IPアドレス(数値化)
        /// </summary>
        private Int64 value = 0;
        
        #endregion

        #region [プロパティ]

        /// <summary>
        /// アドレス文字列の取得、設定
        /// </summary>
        public String Address
        {
            get
            {
                return this.address.ToString();
            }
            set
            {
                IPAddress addressTemp;
                IPAddress.TryParse( value, out addressTemp );
                this.IpAddress = addressTemp;
            }

        }

        /// <summary>
        /// IPアドレスの取得、設定
        /// </summary>
        [XmlIgnore()]
        public IPAddress IpAddress
        {
            get
            {
                return this.address;
            }
            set
            {
                // 保持するIPAddressに設定し、数値変換も行う
                this.address = value;
                var addressBytes = this.address.GetAddressBytes();
                this.value = 0;
                for ( Int32 i = 0; i < addressBytes.Count(); i++ )
                {
                    this.value <<= 8;
                    this.value += addressBytes[addressBytes.Count() - i - 1];
                }
            }
        }

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SerializableIpAddress()
        {
            this.address = new IPAddress( this.value );
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="address">IPアドレス</param>
        public SerializableIpAddress(IPAddress address )
        {
            this.IpAddress = address;
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// IPアドレスを標準表記変換
        /// </summary>
        /// <remarks>
        /// IPアドレスを標準表記へ変換します。
        /// </remarks>
        /// <returns></returns>
        public override String ToString()
        {
            return this.address.ToString();
        }

        /// <summary>
        /// IPAddress型からの暗黙的変換
        /// </summary>
        /// <remarks>
        /// IPAddress型からの暗黙的変換を行います。
        /// </remarks>
        /// <param name="value">変換対象IPアドレス</param>
        /// <returns>変換済みシリアライズIPアドレス</returns>
        public static implicit operator SerializableIpAddress( IPAddress value )
        {
            return new SerializableIpAddress( value );
        }
        
        
        /// <summary>
        /// IPAddress型への暗黙的変換
        /// </summary>
        /// <remarks>
        /// IPAddress型への暗黙的変換を行います。
        /// </remarks>
        /// <param name="value">変換対象シリアライズIPアドレス</param>
        /// <returns>変換済IPアドレス</returns>
        public static implicit operator IPAddress( SerializableIpAddress value )
        {
            return value.address;
        }
        
        #endregion
        
    }
}
