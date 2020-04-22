using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace Oelco.Common.Comm
{
    /// <summary>
    /// ボーレート定義
    /// </summary>
    public enum BaudRate
    {
        /// <summary>
        /// 2400
        /// </summary>
        Br2400 = 2400,
        /// <summary>
        /// 4800
        /// </summary>
        Br4800 = 4800,
        /// <summary>
        /// 9600
        /// </summary>
        Br9600 = 9600,
        /// <summary>
        /// 19200
        /// </summary>
        Br19200 = 19200,
        /// <summary>
        /// 38400
        /// </summary>
        Br38400 = 38400,
        /// <summary>
        /// 57600
        /// </summary>
        Br57600 = 57600,
        /// <summary>
        /// 115200
        /// </summary>
        Br115200 = 115200
    }

    /// <summary>
    /// データビット定義
    /// </summary>
    public enum DataBits
    {
        /// <summary>
        /// ビット7
        /// </summary>
        Bit7 = 7,
        /// <summary>
        /// ビット8
        /// </summary>
        Bit8 = 8
    }

    /// <summary>
    /// ストップビット定義
    /// </summary>
    public enum StopBitKind
    {
        /// <summary>
        /// 1ビット
        /// </summary>
        Bit1,
        /// <summary>
        /// 2ビット
        /// </summary>
        Bit2
    }

    /// <summary>
    /// シリアル接続パラメータ
    /// </summary>
    /// <remarks>
    /// シリアル接続の設定に必要なパラメータを定義します。
    /// </remarks>
    public class SerialParameter
    {

        #region [インスタンス変数定義]

        /// <summary>
        /// ボーレート
        /// </summary>
        private BaudRate baudRate;

        /// <summary>
        /// パリティ
        /// </summary>
        private Parity parity;

        /// <summary>
        /// データビット
        /// </summary>
        private DataBits dataBits;

        /// <summary>
        /// ストップビット
        /// </summary>
        private StopBits stopBits;

        /// <summary>
        /// 読込みタイムアウト
        /// </summary>
        private Int32 readTimeout;

        /// <summary>
        /// 書込みタイムアウト
        /// </summary>
        private Int32 writeTimeout;

        /// <summary>
        /// COMポート
        /// </summary>
        private String commPort;
        
        #endregion

        #region [プロパティ]

        /// <summary>
        /// ボーレート 設定/取得
        /// </summary>
        public BaudRate BaudRate
        {
            get
            {
                return this.baudRate;
            }
            set
            {
                this.baudRate = value;
            }
        }
        
        /// <summary>
        /// パリティ 設定/取得
        /// </summary>
        public Parity Parity
        {
            get
            {
                return this.parity;
            }
            set
            {
                this.parity = value;
            }
        }

        /// <summary>
        /// データビット 設定/取得
        /// </summary>
        public DataBits DataBits
        {
            get
            {
                return this.dataBits;
            }
            set
            {
                this.dataBits = value;
            }
        }

        /// <summary>
        /// ストップビット 設定/取得
        /// </summary>
        public StopBits StopBits
        {
            get
            {
                return this.stopBits;
            }
            set
            {
                this.stopBits = value;
            }
        }

        /// <summary>
        /// 読込みタイムアウト 設定/取得
        /// </summary>
        public Int32 ReadTimeout
        {
            get
            {
                return this.readTimeout;
            }
            set
            {
                this.readTimeout = value;
            }
        }

        /// <summary>
        /// 書込みタイムアウト 設定/取得
        /// </summary>
        public Int32 WriteTimeout
        {
            get
            {
                return this.writeTimeout;
            }
            set
            {
                this.writeTimeout = value;
            }
        }

        /// <summary>
        /// COMポート 設定/取得
        /// </summary>
        public String CommPort
        {
            get
            {
                return commPort;
            }
            set
            {
                commPort = value;
            }
        }

        #endregion

    }
}
