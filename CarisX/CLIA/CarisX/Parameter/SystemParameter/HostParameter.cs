using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;
using System.IO.Ports;
using Oelco.Common.Comm;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// ホスト設定
    /// </summary>
	public class HostParameter : AttachmentParameter
	{
        #region [定数定義]
        
        /// <summary>
        /// 送信遅延時間　最小設定時間（msec）
        /// </summary>
        public const Int32 SEND_DELAY_TIME_MIN = 0;
        /// <summary>
        /// 送信遅延時間　最大設定時間（msec）
        /// </summary>
        public const Int32 SEND_DELAY_TIME_MAX = 100;

        /// <summary>
        /// ボーレート　デフォルト値設定
        /// </summary>
        public const BaudRate BAUDRATE_DEFAULT = BaudRate.Br9600;
        /// <summary>
        /// パリティ　デフォルト値設定
        /// </summary>
        public const Parity PARITY_DEFAULT = Parity.Even;
        /// <summary>
        /// データ長　デフォルト値設定
        /// </summary>
        public const DataBits DATA_LENGTH_DEFAULT = DataBits.Bit8;
        /// <summary>
        /// ストップビット　デフォルト値設定
        /// </summary>
        public const StopBitKind STOP_BIT_DEFAULT = StopBitKind.Bit1;
        /// <summary>
        /// COMポート　デフォルト値設定
        /// </summary>
        public const String COMM_PORT_DEFAULT = "COM1";
        /// <summary>
        /// 検体リアルタイム出力有無　デフォルト値設定
        /// </summary>
        public const Boolean REALTIME_OUTPUT_SAMP_DEFAULT = true;
        /// <summary>
        /// 精度管理検体リアルタイム出力有無　デフォルト値設定
        /// </summary>
        public const Boolean REALTIME_OUTPUT_CTRL_DEFAULT = true;
        /// <summary>
        /// キャリブレータリアルタイム出力有無　デフォルト値設定
        /// </summary>
        public const Boolean REALTIME_OUTPUT_CALIB_DEFAULT = true;
        /// <summary>
        /// 送信遅延時間　デフォルト値設定（msec）
        /// </summary>
        public const Int32 SEND_DELAY_TIME_DEFAULT = SEND_DELAY_TIME_MIN;
        /// <summary>
        /// 検体問い合わせ有無　デフォルト値設定
        /// </summary>
        public const Boolean ASK_SAMP_DEFAULT = false;
        /// <summary>
        /// 精度管理検体問い合わせ有無　デフォルト値設定
        /// </summary>
        public const Boolean ASK_CTRL_DEFAULT = false;
        /// <summary>
        /// サンプル区分利用有無　デフォルト値設定
        /// </summary>
        public const Boolean USE_SMPL_KIND_DEFAULT = false;
        /// <summary>
        /// コメント使用有無 デフォルト値設定
        /// </summary>
        public const Boolean USE_COMMENT_DEFAULT = true;
        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 検体問い合わせ待機時間
        /// </summary>
        private Int32 usableAskSampWaitTime = 18;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// コメント使用有無
        /// </summary>
        public Boolean UsableComment { get; } = USE_COMMENT_DEFAULT;

        /// <summary>
        /// サンプル区分使用有無
        /// </summary>
        public Boolean UseHostSampleType { get; } = USE_SMPL_KIND_DEFAULT;

        /// <summary>
        /// ボーレート
        /// </summary>
        public BaudRate Baudrate { get; set; } = BAUDRATE_DEFAULT;

        /// <summary>
        /// パリティ
        /// </summary>
        public Parity Parity { get; set; } = PARITY_DEFAULT;

        /// <summary>
        /// データ長
        /// </summary>
        public DataBits DataLength { get; set; } = DATA_LENGTH_DEFAULT;

        /// <summary>
        /// ストップビット
        /// </summary>
        public StopBitKind StopBit { get; set; } = STOP_BIT_DEFAULT;
        
        /// <summary>
        /// COMポート
        /// </summary>
        public String CommPort { get; set; } = COMM_PORT_DEFAULT;

        /// <summary>
        /// 検体リアルタイム出力
        /// </summary>
        public Boolean UsableRealtimeOutputSamp { get; set; } = REALTIME_OUTPUT_SAMP_DEFAULT;

        /// <summary>
        /// 精度管理検体リアルタイム出力
        /// </summary>
        public Boolean UsableRealtimeOutputCtrl { get; set; } = REALTIME_OUTPUT_CTRL_DEFAULT;

        /// <summary>
        /// 送信遅延時間（msec）
        /// </summary>
        public Int32 SendDelayTime { get; set; } = SEND_DELAY_TIME_DEFAULT;

        /// <summary>
        /// 検体問い合わせ有無
        /// </summary>
        public Boolean UseRealtimeSampleAsk { get; set; } = ASK_SAMP_DEFAULT;

        /// <summary>
        /// 検体問い合わせ待機時間
        /// </summary>
        public Int32 UseRealtimeSampleAskWaitTime
        {
            get
            {
                return this.usableAskSampWaitTime;
            }
            set
            {
                if (value < usableAskSampWaitTime && !HostParameterSetFlag)
                    value = usableAskSampWaitTime;
                this.usableAskSampWaitTime = value;
            }
        }

        public Boolean HostParameterSetFlag { get; set; } = false;

        #endregion
    }
	 
}
 
