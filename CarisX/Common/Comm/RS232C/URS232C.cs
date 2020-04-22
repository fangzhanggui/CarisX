
using System;
using System.IO;
using System.IO.Ports;

using System.Collections.Generic;
using System.Linq;
using System.Text;

//using UApplication.General;
using System.Threading;//Figu:

namespace Oelco.Common.Comm
{
    /// <summary>
    /// COM制御コード
    /// </summary>
    public enum RsChar
    {
        SOH = 0x01,
        STX = 0x02,
        ETX = 0x03,
        EOT = 0x04,
        ENQ = 0x05,
        ACK = 0x06,
        NAK = 0x15,
        CR = 0x0d,
        LF = 0x0a
    }
    
    /// <summary>
    /// URS232C
    /// </summary>
    public class URs232c : IDisposable
    {
        #region [定数定義]

        // for OnlineLog
        /// <summary>
        /// オンラインログリミット定義
        /// </summary>
        private enum OnlineLogLimit
        {
            /// <summary>
            /// 最大行数
            /// </summary>
            MAX_LINE = 1000,
            /// <summary>
            /// 最大文字数
            /// </summary>
            MAX_LINE_CHAR = 78
        }

        #endregion

        #region [インスタンス変数定義]

        // forRs232c
        /// <summary>
        /// COMポート
        /// </summary>
        private SerialPort comPort;

        /// <summary>
        /// Dispose済フラグ
        /// </summary>
        private Boolean isDisposed = false;

        /// <summary>
        /// ディレイ時間
        /// </summary>
        private Int32 delayTime = 0;

        /// <summary>
        /// 行カウンタ
        /// </summary>
        private Int32 xCounter = 0;

        /// <summary>
        /// 行中文字数カウンタ
        /// </summary>
        private Int32 yCounter = 0;

        /// <summary>
        /// ログ
        /// </summary>
        private List<String> log;
        public Mutex LogMutex = new Mutex();//Figu:for syc log

        /// <summary>
        /// 通信に使用するEncoding
        /// </summary>
        private String encoding = "shift_jis";

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="com_name">使用するポート名称</param>
        /// <param name="baud_rate">ボー レート</param>
        /// <param name="parity">System.IO.Ports.SerialPort.Parity 値の 1 つ</param>
        /// <param name="data_bits">データ ビット値</param>
        /// <param name="stop_bits">System.IO.Ports.SerialPort.StopBits 値の 1 つ</param>
        public URs232c( String com_name, BaudRate baud_rate, Parity parity, DataBits data_bits, StopBits stop_bits )
        {
            // COMが存在しない場合は待機する
            //waitForInitCom(com_name);

            Int32 iBaudRate = (Int32)baud_rate;
            Int32 iDataBits = (Int32)data_bits;
            this.comPort = new SerialPort( com_name, iBaudRate, (Parity)parity, iDataBits, (StopBits)stop_bits );
            this.comPort.WriteTimeout = 3000;
            this.comPort.ReadTimeout = 3000;
            this.delayTime = 0;

            //// ポートがCloseならオープンする
            //if (!mComPort.IsOpen)
            //{
            //    mComPort.Open();
            //}

            // OnlineLogの初期化
            this.xCounter = 0;
            this.yCounter = 0;

            LogMutex.WaitOne();
            this.log = new List<String>();
            for ( Int32 i = 0; i < (Int32)OnlineLogLimit.MAX_LINE; i++ )
            {
                this.log.Add( " " );
            }
            LogMutex.ReleaseMutex();

            // 通信に使用するEncodingをセットする
            this.setEncoding();
        }


        // デスクトラクタ
        ~URs232c()
        {
            Dispose();
        }


        /// <summary>
        /// 解放処理
        /// </summary>
        /// <remarks>
        /// 解放処理を行います。
        /// </remarks>
        public void Dispose()
        {
            if ( !this.isDisposed )
            {
                if ( this.comPort.IsOpen )
                {
                    this.comPort.Close();
                }
            }
            this.isDisposed = true;
            this.log.Clear();
        }

        #endregion

        #region [プロパティ]

        // Baudrate,Parity,DataBits,StopBitsへのセットを有効にする
        /// <summary>
        /// ボーレートの設定
        /// </summary>
        public BaudRate BaudRate
        {
            set
            {
                this.comPort.BaudRate = (Int32)value;
            }
        }
        /// <summary>
        /// パリティの設定
        /// </summary>
        public Parity Parity
        {
            set
            {
                this.comPort.Parity = (Parity)value;
            }
        }
        /// <summary>
        /// データビットの設定
        /// </summary>
        public DataBits DataBits
        {
            set
            {
                this.comPort.DataBits = (Int32)value;
            }
        }
        /// <summary>
        /// ストップビットの設定
        /// </summary>
        public StopBits StopBits
        {
            set
            {
                this.comPort.StopBits = (StopBits)value;
            }
        }
        /// <summary>
        /// 読込タイムアウトの設定
        /// </summary>
        public Int32 ReadTimeout
        {
            set
            {
                this.comPort.ReadTimeout = value;
            }
        }
        /// <summary>
        /// 書込タイムアウトの設定
        /// </summary>
        public Int32 WriteTimeout
        {
            set
            {
                this.comPort.WriteTimeout = value;
            }
        }
        /// <summary>
        /// ディレイタイムの設定
        /// </summary>
        public Int32 DelayTime
        {
            set
            {
                this.delayTime = value;
            }
        }
        /// <summary>
        /// ポートオープン状態の取得
        /// </summary>
        public Boolean IsOpen
        {
            get
            {
                return this.comPort.IsOpen;
            }
        }


        #region [同步在线日志]
        /// <summary>
        /// 当行数等于最大行数时，对新增加的数据进行行数计数
        /// 注意分为两种情况：（1）最后一次记录等于最大行数 （2）最后一次记录小于最大行数
        /// </summary>
        private int rows = 0;

        /// <summary>
        /// 同步后的在线日志
        /// </summary>
        List<string> sycLog = new List<string>();
        public List<String> SycLog
        {
            get
            {
                //Figu:syc
                LogMutex.WaitOne();
                try
                {
                    sycLog.Clear();
                    sycLog.AddRange(log);
                    sycLog.Add(rows.ToString());//Figu:注意进行解析
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    LogMutex.ReleaseMutex();
                }

                rows = 0;//Figu:reset
                return sycLog;

            }
        }
        #endregion

        /// <summary>
        /// ログの取得
        /// </summary>
        public List<String> Log
        {
            get
            {
                return this.log;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 232Cポートに指定された長さだけ読み込む
        /// </summary>
        /// <remarks>
        /// 232Cポートに指定された長さだけ読み込みます。
        /// </remarks>
        /// <param name="buffer">読み込んだデータ</param>
        /// <param name="len">読み込む長さ</param>
        /// <returns>読み込んだ長さ</returns>
        public Int32 ReadData( out char[] buffer, Int64 len )
        {
            Int32 i;
            buffer = new char[len];

            for ( i = 0; i < len; i++ )
            {
                if ( !this.ReadChar( out buffer[i] ) )
                {
                    return i + 1;
                }
            }
            return i + 1;
        }

        /// <summary>
        /// 232Cポートに指定された長さだけ書き込む
        /// </summary>
        /// <remarks>
        /// 232Cポートに指定された長さだけ書き込みます。
        /// </remarks>
        /// <param name="buffer">書き込むデータ</param>
        /// <param name="len">書き込む長さ</param>
        /// <returns>書き込んだ長さ</returns>
        public Int32 WriteData( char[] buffer, Int64 len )
        {
            Int32 i;

            for ( i = 0; i < len; i++ )
            {
                if ( !this.WriteChar( buffer[i] ) )
                {
                    return i + 1;
                }
            }
            return i + 1;
        }

        /// <summary>
        /// 232Cポートから1バイト読み込む
        /// </summary>
        /// <remarks>
        /// 232Cポートから1バイト読み込みます。
        /// </remarks>
        /// <param name="data">読み込んだデータ</param>
        /// <returns>true :読み込み成功/false:読み込み失敗</returns>
        public Boolean ReadChar( out char data )
        {
            Int32 i_data;
            data = ' ';
            Int32 timeOut = this.comPort.ReadTimeout;
            Int32 timeOutDiv = 10;
            Int32 waitSum = 0;
            try
            {
                while ( this.comPort.BytesToRead == 0 )
                {
                    System.Threading.Thread.Sleep( timeOutDiv );
                    waitSum += timeOutDiv;
                    if ( waitSum >= timeOut )
                    {
                        return false;
                    }
                }

                //if ( this.mComPort.BytesToRead == 0 )
                //{
                //    return false;
                //}
                i_data = this.comPort.ReadByte();

                data = (char)i_data;
            }
            catch ( Exception )
            {
                return false;
            }

            // OnlineLogへ書き込む
            this.SetRcvChar( data );
            return true;
        }

        /// <summary>
        /// 232Cポートに1バイト書き込む
        /// </summary>
        /// <remarks>
        ///　232Cポートに1バイト書き込みます。
        /// </remarks>
        /// <param name="data">書き込むデータ</param>
        /// <returns>true :書き込み成功
        /// 			 false:書き込み失敗</returns>
        public Boolean WriteChar( char data )
        {
            char[] w_array = { data };
            try
            {
                // 文字ではなくバイナリで送信する
                Byte[] bytes = new byte[1];
                bytes[0] = (byte)w_array[0];
                this.comPort.Write( bytes, 0, 1 );
            }
            catch
            {
                return false;
            }

            if ( this.delayTime > 0 )
            {
                System.Threading.Thread.Sleep( this.delayTime );
            }

            // OnlineLogへ書き込む
            this.SetSendChar( data );
            return true;
        }

        /// <summary>
        /// 232Cポートに文字列を書き込む
        /// </summary>
        /// <remarks>
        /// 232Cポートに文字列を書き込みます。
        /// </remarks>
        /// <param name="data">書き込むデータ</param>
        /// <returns>書き込んだ長さ</returns>
        public Int32 WriteString( String data )
        {
            Int32 i = 0;

            foreach ( char c in data )
            {
                if ( !this.WriteChar( c ) )
                {
                    return i;
                }
                i++;
            }
            return i;
        }

        /// <summary>
        /// 使用できるポートを取得する
        /// </summary>
        /// <remarks>
        /// 使用できるポートを返します。
        /// </remarks>
        /// <returns>使用できるポート名</returns>
        public static String[] GetUsePort()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>
        /// ポートを開く
        /// </summary>
        /// <remarks>
        /// シリアルポートを開きます。
        /// </remarks>
        /// <returns>true:成功/false:失敗</returns>
        public Boolean Open()
        {
            Boolean openSuccess = false;
            try
            {
                if ( !this.comPort.IsOpen )
                {
                    this.comPort.Open();
                    openSuccess = true;
                }
            }
            catch ( Exception )
            {
                openSuccess = false;
            }

            return openSuccess;
        }

        /// <summary>
        /// ポートを閉じる
        /// </summary>
        /// <remarks>
        /// シリアルポートをクローズします。
        /// </remarks>
        public void Close()
        {
            if ( this.comPort.IsOpen )
            {
                this.comPort.Close();
            }
        }

        #endregion

        #region [protectedメソッド]


        // 通信に使用するEncodingを動的に変更する
        // 半角カタカナの送受信に対応する

        /// <summary>
        /// テキストのエンコードをmyEncodingからUnicodeに変換する
        /// </summary>
        /// <remarks>
        /// テキストのエンコードをmyEncodingからUnicodeに変換します。
        /// </remarks>
        /// <param name="c">Unicode変換対象文字</param>
        /// <returns>変換後Unicode文字</returns>
        protected char[] ChangeXcodeToUnicode( char[] c_text )
        {
            char[] encodingText;
            try
            {
                System.Text.Encoding x_code = System.Text.Encoding.GetEncoding( this.encoding );
                byte[] encodingValue = new byte[c_text.Length];
                for ( Int32 i = 0; i < c_text.Length; i++ )
                {
                    encodingValue[i] = (byte)c_text[i];
                }
                String str_text = x_code.GetString( encodingValue );
                encodingText = str_text.ToCharArray();
            }
            catch
            {
                encodingText = c_text;
            }

            return encodingText;
        }

        /// <summary>
        /// テキストのエンコードをUnicodeからmyEncodingに変換する
        /// </summary>
        /// <remarks>
        /// テキストのエンコードをUnicodeからmyEncodingに変換します。
        /// </remarks>
        /// <param name="c_text">Unicode文字</param>
        /// <returns>通信用エンコード変換文字</returns>
        protected char[] ChangeUnicodeToXcode( char[] c_text )
        {
            char[] encodingText;

            try
            {
                System.Text.Encoding x_code = System.Text.Encoding.GetEncoding( this.encoding );
                Byte[] encodingValue = x_code.GetBytes( c_text );
                encodingText = new char[encodingValue.Length];
                for ( Int32 j = 0; j < encodingValue.Length; j++ )
                {
                    encodingText[j] = (char)encodingValue[j];
                }
            }
            catch
            {
                encodingText = c_text;
            }

            return encodingText;
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// 送信されたデータを通信リストに登録する
        /// </summary>
        /// <remarks>
        /// 送信されたデータを通信リストに登録します。
        /// </remarks>
        /// <param name="c">送信文字</param>
        private void SetSendChar( char c )
        {
            // 半角カタカナの送受信に対応するため、shift_jisをUnicodeに変換する
            char[] char_array = new char[1];
            char_array[0] = c;
            char_array = this.ChangeXcodeToUnicode( char_array );
            c = char_array[0];

            if ( this.yCounter == (Int32)OnlineLogLimit.MAX_LINE )
            {
                this.log.RemoveAt( 0 );
                this.log.RemoveAt( 0 );
                this.log.Add( " " );		// ノイズ除去
                this.log.Add( " " );		// ノイズ除去
                this.yCounter += -2;
                this.rows += 2;//Figu:
            }
            if ( this.xCounter == 0 )
            {
                this.log[this.yCounter] = "S:";
                this.log[this.yCounter + 1] = "R:";
            }
            if ( this.xCounter < (Int32)OnlineLogLimit.MAX_LINE_CHAR )
            {
                this.log[this.yCounter] = this.log[this.yCounter] + this.GetDispChar( c );

                if ( this.GetDispChar( c ).Length > 1 )
                {
                    this.log[this.yCounter + 1] = this.log[this.yCounter + 1] + "  ";
                }
                else
                {
                    this.log[this.yCounter + 1] = this.log[this.yCounter + 1] + "  ";
                }

                this.xCounter++;
                if ( this.xCounter == (Int32)OnlineLogLimit.MAX_LINE_CHAR )
                {
                    this.yCounter += 2;
                    this.xCounter = 0;
                }
            }
        }

        /// <summary>
        /// 受信されたデータを通信リストに登録する
        /// </summary>
        /// <remarks>
        /// 受信されたデータを通信リストに登録します。
        /// </remarks>
        /// <param name="c">受信文字</param>
        private void SetRcvChar( char c )
        {
            // 半角カタカナの送受信に対応するため、shift_jisをUnicodeに変換する
            char[] char_array = new char[1];
            char_array[0] = c;
            char_array = this.ChangeXcodeToUnicode( char_array );
            c = char_array[0];

            if ( this.yCounter == (Int32)OnlineLogLimit.MAX_LINE )
            {
                log.RemoveAt( 0 );
                log.RemoveAt( 0 );
                log.Add( " " );		// ノイズ除去
                log.Add( " " );		// ノイズ除去
                yCounter += -2;
                this.rows += 2;//Figu:
            }
            if ( this.xCounter == 0 )
            {
                this.log[this.yCounter] = "S:";
                this.log[this.yCounter + 1] = "R:";
            }
            if ( this.xCounter < (Int32)OnlineLogLimit.MAX_LINE_CHAR )
            {
                if ( this.GetDispChar( c ).Length > 1 )
                {
                    this.log[this.yCounter] = this.log[this.yCounter] + "  ";
                }
                else
                {
                    this.log[this.yCounter] = this.log[this.yCounter] + "  ";
                }

                this.log[this.yCounter + 1] = this.log[this.yCounter + 1] + this.GetDispChar( c );
                this.xCounter++;
                if ( this.xCounter == (Int32)OnlineLogLimit.MAX_LINE_CHAR )
                {
                    this.yCounter += 2;
                    this.xCounter = 0;
                }
            }
        }

        /// <summary>
        /// データを文字列に変換する
        /// </summary>
        /// <remarks>
        /// データを文字列に変換します。
        /// </remarks>
        /// <param name="c">データ</param>
        /// <returns>文字列</returns>
        private String GetDispChar( char c )
        {
            String str;

            switch ( c )
            {
            case (char)RsChar.ENQ:
                str = "EQ";
                break;
            case (char)RsChar.STX:
                str = "SX";
                break;
            case (char)RsChar.ETX:
                str = "EX";
                break;
            case (char)RsChar.ACK:
                str = "AK";
                break;
            case (char)RsChar.NAK:
                str = "NK";
                break;
            case (char)RsChar.EOT:
                str = "ET";
                break;
            default:
                str = c.ToString() + " ";
                break;
            }
            return str;
        }

        /// <summary>
        /// Encodingセット処理
        /// </summary>
        /// <remarks>
        /// 通信に使用するEncodingをセットします。
        /// </remarks>
        private void setEncoding()
        {
            // 言語設定を取得する
            switch ( System.Globalization.CultureInfo.CurrentCulture.Name )
            {
            case "ja-JP":
            case "en-EN":
                this.encoding = "shift_jis";
                break;
            case "fr-FR":
            case "it-IT":
                this.encoding = "iso-8859-1";
                break;
            default:
                this.encoding = "shift_jis";
                break;
            }
        }

        #endregion

    }
}