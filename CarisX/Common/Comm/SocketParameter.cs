using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using Oelco.Common.Utility;

namespace Oelco.Common.Comm
{
    /// <summary>
    /// ソケットパラメータ
    /// </summary>
    /// <remarks>
    /// ソケット接続の設定に必要なパラメータを定義します。
    /// </remarks>
    public class SocketParameter
    {

        #region [インスタンス変数定義]

        /// <summary>
        /// モジュールＩＤ
        /// </summary>
        private Int32 moduleid = 1;

        /// <summary>
        /// IPアドレス
        /// </summary>
        private SerializableIpAddress ipAddress = IPAddress.Parse( "192.168.1.160" );

        /// <summary>
        /// 受信ポート
        /// </summary>
        private Int32 r_port = 65523;
        /// <summary>
        /// 送信ポート
        /// </summary>
        private Int32 s_port = 65522;

        /// <summary>
        /// タイムアウト
        /// </summary>
        private float timeout = (float)2.0;


        #endregion

        #region [プロパティ]

        /// <summary>
        /// モジュールＩＤの取得、設定
        /// </summary>
        public Int32 ModuleId
        {
            get
            {
                return moduleid;
            }
            set
            {
                moduleid = value;
            }
        }

        /// <summary>
        /// IPアドレスの取得、設定
        /// </summary>
        public SerializableIpAddress IpAddress
        {
            get
            {
                return ipAddress;
            }
            set
            {
                ipAddress = value;
            }
        }

        /// <summary>
        /// 受信ポートの取得、設定
        /// </summary>
        public Int32 R_Port
        {
            get
            {
                return r_port;
            }
            set
            {
                r_port = value;
            }
        }

        /// <summary>
        /// 送信ポートの取得、設定
        /// </summary>
        public Int32 S_Port
        {
            get
            {
                return s_port;
            }
            set
            {
                s_port = value;
            }
        }

        /// <summary>
        /// タイムアウト時間
        /// </summary>
        public float TimeOut
        {
            get
            {
                return timeout;
            }
            set
            {
                timeout = value;
            }

        }

        #endregion

    }
}
