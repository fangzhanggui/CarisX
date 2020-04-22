using System;
using System.Text;
using System.IO.Ports;

namespace Oelco.Common.Comm
{
    using APL_IO = Oelco.Common.Comm;

    /// <summary>
    /// 列挙型、通信プロトコル
    /// </summary>
    public enum Protocol
    {
        /// <summary>
        /// なし
        /// </summary>
        NONE,
        /// <summary>
        /// ハンドシェイク1
        /// </summary>
        HANDSHAKE1,
        /// <summary>
        /// ハンドシェイク2
        /// </summary>
        HANDSHAKE2
    };

    /// <summary>
    /// 列挙型、デリミタ
    /// </summary>
    public enum Delimiter
    {
        /// <summary>
        /// なし
        /// </summary>
        NONE,
        /// <summary>
        /// CR
        /// </summary>
        CR,
        /// <summary>
        /// CR/LF
        /// </summary>
        CRLF
    };

    /// <summary>
    /// チェックサム領域
    /// </summary>
    public enum CheckArea
    {
        /// <summary>
        /// STXからETXまで計算
        /// </summary>
        STX_TO_ETX,
        /// <summary>
        /// STXの次からETXまで計算
        /// </summary>
        TO_ETX
    };

    /// <summary>
    /// URSCOM
    /// </summary>
    public class URsCom : URs232c
    {
        #region [定数定義]

        /// <summary>
        /// true値
        /// </summary>
        public const Int32 TRUE = 1;
        /// <summary>
        /// false値
        /// </summary>
        public const Int32 FALSE = 0;
        /// <summary>
        /// リトライ回数
        /// </summary>
        private const Int32 RETRY_TIMES = 6;
        /// <summary>
        /// チェックサムサイズ
        /// </summary>
        private const Int32 SIZE_CHECK_BYTE = 4;
        /// <summary>
        /// 最大ブロックサイズ
        /// </summary>
        private const Int32 MAX_BLOCKSIZE = 15360;
        /// <summary>
        /// ブロック長
        /// </summary>
        private const Int32 BLOCK_LEN = 1024;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// デリミタ
        /// </summary>
        private Delimiter delimiter;
        /// <summary>
        /// チェックサム領域
        /// </summary>
        private CheckArea checkArea;
        /// <summary>
        /// チェックサム使用フラグ
        /// </summary>
        private Boolean checkByteUse;
        /// <summary>
        /// チェックバイト数2バイトフラグ
        /// </summary>
        private Boolean is2CheckByteLen;
        /// <summary>
        /// 受信タイムアウト既定値
        /// </summary>
        private Int32 initTimeoutForReceive;
        /// <summary>
        /// 送信タイムアウト既定値
        /// </summary>
        private Int32 byteIntervalForReceive;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /* コンストラクタ ホストに使用 ReadTimeoutを設定できるように*/
        /// <summary>
        /// コンストラクタ
        /// タイムアウト 3sec
        /// </summary>
        /// <param name="comName">Comポート名</param>
        /// <param name="proto">通信プロトコル</param>
        /// <param name="baud_rate">ボーレート</param>
        /// <param name="parity">パリティ</param>
        /// <param name="data_bits">データビット</param>
        /// <param name="stop_bits">ストップビット</param>
        /// <param name="check_area">チェックエリア</param>
        /// <param name="is_2_check_byte_len">チェックバイト数</param>
        public URsCom( String comName/*, Protocol proto*/, BaudRate baud_rate, Parity parity, DataBits data_bits, StopBits stop_bits, APL_IO.CheckArea check_area, Boolean is_2_check_byte_len, Int32 read_timeout )
            : base( comName, baud_rate, parity, data_bits, stop_bits )
        {
            //mProtocol = proto;
            this.delimiter = APL_IO.Delimiter.CRLF;
            //mCounter = 0;
            this.SetTimeout( 100, read_timeout );
            this.checkArea = check_area;
            this.checkByteUse = true;
            this.is2CheckByteLen = is_2_check_byte_len;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// デリミタの設定
        /// </summary>
        public Delimiter Delimiter
        {
            set
            {
                this.delimiter = value;
            }
        }

        /// <summary>
        /// チェックサム領域の設定
        /// </summary>
        public CheckArea CheckArea
        {
            set
            {
                this.checkArea = value;
            }
        }
        /// <summary>
        /// チェックサム使用フラグの設定
        /// </summary>
        public Boolean CheckbyteUse
        {
            set
            {
                this.checkByteUse = value;
            }
        }
        /// <summary>
        /// チェックバイト数2バイトフラグ
        /// </summary>
        public Boolean Is2CheckByteLen
        {
            set
            {
                this.is2CheckByteLen = value;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// セッションを開く
        /// </summary>
        /// <remarks>
        /// セッションをオープンします。
        /// </remarks>
        /// <returns>成功:１  失敗:0   
        ///         相手からセッションを開こうとしていた場合:-1</returns>
        public Int32 OpenSession()
        {
            Int32 retry = 0;
            char[] w_c = new char[1];
            char r_c;

            // バイトインターバルタイムアウトのセット
            this.ReadTimeout = byteIntervalForReceive;

            // ENQ付き通信手順の場合
            //if (mProtocol == APL_IO.Protocol.HANDSHAKE2)
            //{
            // ENQの再送タイミングは3secであることを確認。ReadTimeout = 3000
            while ( true )
            {
                // ENQ送信
                if ( !this.WriteChar( (char)RsChar.ENQ ) )
                {
                    return URsCom.FALSE;
                }

                while ( true )
                {
                    // レスポンス待ち(ACK)
                    if ( !this.ReadChar( out r_c ) )
                    {
                        break;
                    }
                    else
                    {
                        // もしENQとENQがぶつかったら
                        if ( r_c == (char)RsChar.ENQ )
                        {
                            return -1;
                        }
                        else if ( r_c != (char)RsChar.ACK )
                        {
                            break;
                        }
                        else
                        {
                            return URsCom.TRUE;
                        }
                    }
                }

                if ( ++retry > URsCom.RETRY_TIMES )
                {
                    return URsCom.FALSE;
                }
            }
            //}
            //return TRUE;
        }

        /// <summary>
        /// セッションを閉じる
        /// </summary>
        /// <remarks>
        /// セッションをクローズします。
        /// </remarks>
        /// <returns>成功：true
        ///         失敗：false</returns>
        public Boolean CloseSession()
        {
            // 手順つきプロトコルの場合
            //if (mProtocol != APL_IO.Protocol.NONE)
            //{
            // EOTを送信する
            if ( !this.WriteChar( (char)RsChar.EOT ) )
            {
                return false;
            }
            //}
            return true;
        }

        /// <summary>
        /// テキスト送信
        /// </summary>
        /// <remarks>
        /// テキストを送信します。
        /// </remarks>
        /// <param name="text">テキスト</param>
        /// <returns>送信した文字数</returns>
        public Int32 Send( String text )
        {
            Int32 lenToSend = 0;
            Int32 len = 0;
            Int32 block;
            String buf;

            //switch (mProtocol)
            //{
            //    case APL_IO.Protocol.NONE:
            //        len = SendOnNoneProtocol(Text);
            //        break;
            //    case APL_IO.Protocol.HANDSHAKE1:
            //        goto case APL_IO.Protocol.HANDSHAKE2;
            //    case APL_IO.Protocol.HANDSHAKE2:
            lenToSend = text.Length;
            for ( block = 0; ; block++ )
            {
                if ( lenToSend <= URsCom.BLOCK_LEN )
                {
                    buf = text.Substring( block * URsCom.BLOCK_LEN, lenToSend );
                }
                else
                {
                    buf = text.Substring( block * URsCom.BLOCK_LEN, URsCom.BLOCK_LEN );
                }

                len = SendOnHandshake( buf );
                if ( len == 0 )
                {
                    return len;
                }

                if ( lenToSend <= URsCom.BLOCK_LEN )
                {
                    break;
                }
                lenToSend -= URsCom.BLOCK_LEN;
            }

            //    break;
            //default:
            //    break;
            //}

            return len;
        }

        /// <summary>
        /// テキストを受信する(ホスト用)
        /// </summary>
        /// <remarks>
        /// ホスト用のテキストを受信します。
        /// </remarks>
        /// <param name="text">テキスト</param>
        /// <returns>受信した文字数
        /// 　　　　 失敗：FALSE</returns>
        public Int32 ReceiveForHost( out String text )
        {
            Int32 len = 0;

            //switch (mProtocol)
            //{
            //    case APL_IO.Protocol.NONE:
            //        len = ReceiveOnNoneProtocol(out Text);
            //        if (len > 0)
            //        {
            //            ++mCounter;
            //        }
            //        break;
            //    case APL_IO.Protocol.HANDSHAKE1:
            //        goto case APL_IO.Protocol.HANDSHAKE2;
            //case APL_IO.Protocol.HANDSHAKE2:
            len = ReceiveOnHandshakeForHost( out text );
            //if (len > 0)
            //{
            //    ++mCounter;
            //}
            //        break;
            //    default:
            //        Text = null;
            //        break;
            //}
            return len;
        }

        /// <summary>
        /// 送受信時タイムアウト時間セット処理        
        /// </summary>
        /// <remarks>
        ///　送受信時のタイムアウト時間をセットします。
        /// </remarks>
        /// <param name="initTimeout">始めに受信するまでの待ち時間(msec)</param>
        /// <param name="byteInterval">送信、受信時の待ち時間(msec)</param>
        public void SetTimeout( Int32 initTimeout, Int32 byteInterval )
        {
            // ハンドシェイクを使用する場合、タイムアウトの設定はこのメソッドを使用してください。
            this.initTimeoutForReceive = initTimeout;
            this.byteIntervalForReceive = byteInterval;
            this.ReadTimeout = byteInterval;
            this.WriteTimeout = byteInterval;
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// ハンドシェイク無しのテキスト送信
        /// </summary>
        /// <remarks>
        /// ハンドシェイク無しでテキストを送信する
        /// </remarks>
        /// <param name="text">テキスト</param>
        /// <returns>送信した文字数
        /// 　　　　 失敗：FALSE</returns>
        private Int32 SendOnNoneProtocol( String text )
        {
            Int32 len = text.Length;
            char[] c = text.ToCharArray();

            // テキストデータを送信する
            for ( Int32 i = 0; i < len; i++ )
            {
                if ( !this.WriteChar( c[i] ) )
                {
                    return URsCom.FALSE;
                }
            }
            if ( this.delimiter == APL_IO.Delimiter.CR )
            {
                this.WriteChar( (char)RsChar.CR );
            }
            else if ( this.delimiter == APL_IO.Delimiter.CRLF )
            {
                this.WriteChar( (char)RsChar.CR );
                this.WriteChar( (char)RsChar.LF );
            }
            return len;
        }

        /// <summary>
        /// ハンドシェイク無しのテキスト受信
        /// </summary>
        /// <remarks>
        /// ハンドシェイク無しでテキストを受信します。
        /// </remarks>
        /// <param name="text">テキスト</param>
        /// <returns>受信した文字数
        /// 　　　　 失敗：－１</returns>
        private Int32 ReceiveOnNoneProtocol( out String text )
        {
            Int32 i = 0;
            char[] cText = new char[100];
            char c;

            text = null;

            while ( true )
            {
                if ( !this.ReadChar( out c ) )
                {
                    text = null;
                    return URsCom.FALSE;
                }

                // もし受信データがプリンタブルアスキーコードでない場合は無視
                if ( c >= ' ' )
                {
                    cText[i++] = c;
                    continue;
                }

                // デリミタがCRで設定されいるとき
                if ( this.delimiter == APL_IO.Delimiter.CR )
                {
                    if ( c == (char)RsChar.CR )
                    {
                        text = new String( cText );
                    }
                    return i;
                }
                else if ( this.delimiter == APL_IO.Delimiter.CRLF )
                {
                    if ( c == (char)RsChar.LF )
                    {
                        if ( cText[i - 1] == (char)RsChar.CR )
                        {
                            cText[i - 1] = ' ';
                        }
                        else
                        {
                            cText[i] = ' ';
                        }
                        text = new String( cText );
                        return i;
                    }
                }
            }
        }

        /// <summary>
        /// ハンドシェイク有りのテキスト送信
        /// </summary>
        /// <remarks>
        /// ハンドシェイク有りでテキストを送信します。
        /// </remarks>
        /// <param name="text">テキスト</param>
        /// <returns>送信した文字数
        /// 　　　　 失敗：-1</returns>
        private Int32 SendOnHandshake( String text )
        {
            Int32 len = 0;
            Int32 retry = 0;
            Int32 sum;       // フレーミングされたテキストの総和
            char r_c;
            char[] check_byte = new char[URsCom.SIZE_CHECK_BYTE + 1];
            char[] c_Text = text.ToCharArray();
            String w_str;

            // バイトインターバルタイムアウトのセット
            this.ReadTimeout = this.byteIntervalForReceive;

            while ( true )
            {
                if ( this.checkArea == APL_IO.CheckArea.STX_TO_ETX )
                {
                    sum = (Int32)RsChar.STX + (Int32)RsChar.ETX;
                }
                else
                {
                    sum = (Int32)RsChar.ETX;
                }

                // STXを送信する
                if ( !this.WriteChar( (char)RsChar.STX ) )
                {
                    return URsCom.FALSE;
                }
                // 半角カタカナに対応するため、Unicodeをshift_jisに変換する
                char[] after;

                after = this.ChangeUnicodeToXcode( c_Text );

                // データテキストを送信する
                len = after.Length;

                for ( Int32 i = 0; i < len; i++ )
                {
                    if ( !this.WriteChar( after[i] ) )
                    {
                        return URsCom.FALSE;
                    }
                    if ( this.is2CheckByteLen )
                    {
                        sum ^= (Int32)after[i];
                    }
                    else
                    {
                        sum += (Int32)after[i];
                    }
                }

                // EXTを送信する
                this.WriteChar( (char)RsChar.ETX );

                if ( !this.is2CheckByteLen )
                {
                    w_str = String.Format( "{0:X4}", sum & 0xffff );
                    check_byte = w_str.ToCharArray();
                }
                else
                {
                    w_str = String.Format( "{0,2:x}", sum & 0xff );
                    check_byte = w_str.ToCharArray();
                }

                if ( this.checkByteUse )
                {
                    this.WriteString( new String( check_byte ) );
                }

                // レスポンス(ACK)待ち
                while ( true )
                {
                    if ( !this.ReadChar( out r_c ) )
                    {
                        break;
                    }
                    else
                    {
                        if ( r_c != (char)RsChar.ACK )
                        {
                            break;
                        }
                        else
                        {
                            return len;
                        }
                    }
                }
                // 正常にレスポンスが返らない場合リトライする
                if ( ++retry > URsCom.RETRY_TIMES )
                {
                    return URsCom.FALSE;
                }
                else
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// ハンドシェイク有りのテキスト受信
        /// </summary>
        /// <remarks>
        /// ハンドシェイク有りでテキストを受信します。
        /// </remarks>
        /// <param name="text">受信テキスト</param>
        /// <returns>受信結果(1:成功/0:失敗)</returns>
        private Int32 ReceiveOnHandshake( out String text )
        {
            Int32 sum = 0;      // カウントしたチェックサム
            Int32 comp = 0;       // 受信したチェックサム
            Int32 check_counter = 0;// チェックサム用カウンター
            Int32 data_counter = 0; // データテキスト用カウンター
            Boolean check_flg = false; // チェックサムカウント終了フラグ
            Boolean data_ok = false; // データを正しく受信できたか
            Boolean recv_enq = false;// ＥＮＱを受信したかどうか
            char[] checkbyte;// = new char [5];  // チェックサムバッファー
            char c;         // １文字入力用データ
            char[] databuf = new char[URsCom.MAX_BLOCKSIZE];   // データテキスト用バッファー
            Boolean need_to_change = true;

            text = " ";

            Int32 checkLen;
            if ( !this.is2CheckByteLen )
            {
                checkLen = 4;
            }
            else
            {
                checkLen = 2;
            }
            checkbyte = new char[checkLen];

            while ( true )
            {
                if ( need_to_change )
                {
                    if ( !recv_enq )
                    {
                        this.ReadTimeout = initTimeoutForReceive;
                    }
                    else
                    {
                        this.ReadTimeout = byteIntervalForReceive;
                        need_to_change = false;
                    }
                }
                while ( true )
                {
                    // データ待ち
                    if ( !this.ReadChar( out c ) )
                    {
                        return URsCom.FALSE;
                    }
                    else
                    {
                        break;
                    }
                }
                // 制御コードのチェック
                switch ( c )
                {
                case (char)RsChar.ENQ:
                    if ( !recv_enq )
                    {
                        // ＥＮＱがきたらＡＣＫを返す。
                        this.WriteChar( (char)RsChar.ACK );
                        recv_enq = true;
                    }
                    break;
                case (char)RsChar.STX:

                    check_flg = false;
                    check_counter = 0;
                    data_counter = 0;                        // ここからチェックサムのカウントが始まる。
                    if ( this.checkArea == APL_IO.CheckArea.STX_TO_ETX )
                    {
                        sum = (Int32)RsChar.STX;
                    }
                    else
                    {
                        sum = 0;
                    }
                    break;
                case (char)RsChar.ETX:
                    // 排他的論理和を取る
                    if ( checkLen == 2 )
                    {
                        // ここでチェックサムのカウント終了
                        sum ^= (Int32)RsChar.ETX;
                    }
                    else
                    {
                        // ここでチェックサムのカウント終了
                        sum += (Int32)RsChar.ETX;
                    }

                    if ( this.checkByteUse )
                    {
                        check_flg = true;
                    }
                    else
                    {
                        this.WriteChar( (char)RsChar.ACK );
                        // 半角カタカナに対応するため、Unicodeをshift_jisに変換する

                        char[] before = this.ChangeXcodeToUnicode( databuf );
                        text = new String( before );

                        data_ok = true;
                    }
                    break;
                case (char)RsChar.EOT:
                    // データの終結
                    if ( data_ok )
                    {
                        return data_counter;
                    }
                    return URsCom.FALSE;
                default:
                    // ＥＴＸを受信後は        
                    if ( check_flg )
                    {

                        // チェックサムバイトコードをセット
                        checkbyte[check_counter++] = c;
                        // チェックサム４バイト受信すれば
                        if ( check_counter == checkLen )
                        {
                            check_flg = false;

                            // 先頭に空白ある場合、変換できない。
                            String w_s = new String( checkbyte ).Trim();
                            try
                            {
                                comp = Convert.ToInt32( w_s, 16 );
                                Int32 mask;
                                if ( this.is2CheckByteLen )
                                {
                                    mask = 0xff;
                                    sum = sum & mask;
                                }
                                else
                                {
                                    mask = 0xffff;
                                    sum = sum & mask;
                                }

                                // カウントされたチェックサムと比較
                                if ( sum == comp )
                                {
                                    // 一致していればＡＣＫを返し正常終了
                                    this.WriteChar( (char)RsChar.ACK );
                                    // 半角カタカナに対応するため、Unicodeをshift_jisに変換する
                                    char[] before = this.ChangeXcodeToUnicode( databuf );
                                    text = new String( before );

                                    data_ok = true;
                                    continue;
                                }
                                else
                                {
                                    // 一致していない場合はＮＡＫを送信しリトライ
                                    this.WriteChar( (char)RsChar.NAK );
                                    check_counter = 0;
                                    data_counter = 0;
                                    continue;
                                }
                            }
                            catch
                            {
                                // ＮＡＫを送信しリトライ
                                this.WriteChar( (char)RsChar.NAK );
                                check_counter = 0;
                                data_counter = 0;
                                continue;
                            }
                        }
                    }
                    // テキストデータをバッファーに入れる。
                    else
                    {
                        if ( (char)c >= 0x20 )
                        {
                            // 排他的論理和を取る
                            if ( checkLen == 2 )
                            {
                                sum ^= (Int32)c;
                            }
                            else
                            {
                                sum += (Int32)c;
                            }
                            databuf[data_counter++] = c;
                        }
                    }
                    break;
                }
                // もしデータ数が許容範囲を超えたら失敗
                if ( data_counter >= MAX_BLOCKSIZE )
                {
                    return URsCom.FALSE;
                }
            }
        }

        // ホスト用ハンドシェイク
        /// <summary>
        /// ハンドシェイク有りのテキスト受信する(ホスト用)
        /// </summary>
        /// <remarks>
        /// ハンドシェイク有りでテキストを受信します。
        /// </remarks>
        /// <param name="text">受信テキスト</param>
        /// <returns>受信結果(1:成功/0:失敗)</returns>
        private Int32 ReceiveOnHandshakeForHost( out String text )
        {
            Int32 sum = 0;      // カウントしたチェックサム
            Int32 comp = 0;       // 受信したチェックサム
            Int32 check_counter = 0;// チェックサム用カウンター
            Int32 data_counter = 0; // データテキスト用カウンター
            Boolean check_flg = false; // チェックサムカウント終了フラグ
            Boolean data_ok = false; // データを正しく受信できたか
            Boolean recv_enq = false;// ＥＮＱを受信したかどうか
            char[] checkbyte;// = new char [5];  // チェックサムバッファー
            char c;         // １文字入力用データ
            char[] databuf = new char[URsCom.MAX_BLOCKSIZE];   // データテキスト用バッファー
            Boolean need_to_change = true;

            text = " ";

            Int32 checkLen;
            if ( !this.is2CheckByteLen )
            {
                checkLen = 4;
            }
            else
            {
                checkLen = 2;
            }
            checkbyte = new char[checkLen];

            while ( true )
            {
                if ( need_to_change )
                {
                    if ( !recv_enq )
                    {
                        this.ReadTimeout = initTimeoutForReceive;
                    }
                    else
                    {
                        this.ReadTimeout = byteIntervalForReceive;
                        need_to_change = false;
                    }
                }
                while ( true )
                {
                    // データ待ち
                    if ( !ReadChar( out c ) )
                    {
                        return URsCom.FALSE;
                    }
                    else
                    {
                        break;
                    }
                }
                // 制御コードのチェック
                switch ( c )
                {
                case (char)RsChar.ENQ:
                    if ( !recv_enq )
                    {
                        // ＥＮＱがきたらＡＣＫを返す。
                        this.WriteChar( (char)RsChar.ACK );
                        recv_enq = true;
                    }
                    break;
                case (char)RsChar.STX:

                    check_flg = false;
                    check_counter = 0;
                    data_counter = 0;                        // ここからチェックサムのカウントが始まる。
                    if ( this.checkArea == APL_IO.CheckArea.STX_TO_ETX )
                    {
                        sum = (Int32)RsChar.STX;
                    }
                    else
                    {
                        sum = 0;
                    }
                    break;
                case (char)RsChar.ETX:
                    // 排他的論理和を取る
                    if ( checkLen == 2 )
                    {
                        // ここでチェックサムのカウント終了
                        sum ^= (Int32)RsChar.ETX;
                    }
                    else
                    {
                        // ここでチェックサムのカウント終了
                        sum += (Int32)RsChar.ETX;
                    }

                    if ( this.checkByteUse )
                    {
                        check_flg = true;
                    }
                    else
                    {
                        this.WriteChar( (char)RsChar.ACK );
                        // 半角カタカナに対応するため、Unicodeをshift_jisに変換する
                        char[] before = this.ChangeXcodeToUnicode( databuf );
                        text = new String( before );

                        data_ok = true;
                        //add umic
					//	this.WriteChar( (char)RsChar.ACK );
                    //    return data_counter;
                        //end add
                    }
                    break;
                case (char)RsChar.EOT:
                    // データの終結
                    // ENQを受信していなければ、成立させない
                    if ( data_ok && recv_enq )
                    {
                        return data_counter;
                    }
                    return URsCom.FALSE;
                default:
                    // ＥＴＸを受信後は        
                    if ( check_flg )
                    {
                        // チェックサムバイトコードをセット
                        checkbyte[check_counter++] = c;
                        // チェックサム４バイト受信すれば
                        if ( check_counter == checkLen )
                        {
                            check_flg = false;

                            // 先頭に空白ある場合、変換できない。
                            String w_s = new String( checkbyte ).Trim();

                            try
                            {
                                comp = Convert.ToInt32( w_s, 16 );
                                Int32 mask;
                                if ( this.is2CheckByteLen )
                                {
                                    mask = 0xff;
                                    sum = sum & mask;
                                }
                                else
                                {
                                    mask = 0xffff;
                                    sum = sum & mask;
                                }

                                // カウントされたチェックサムと比較
                                if ( sum == comp )
                                {
                                    // 一致していればＡＣＫを返し正常終了
                                    this.WriteChar( (char)RsChar.ACK );
                                    // 半角カタカナに対応するため、Unicodeをshift_jisに変換する
                                    char[] before = this.ChangeXcodeToUnicode( databuf );
                                    text = new String( before );

                                    data_ok = true;
                                    continue;
                                }
                                else
                                {
                                    // 一致していない場合はＮＡＫを送信しリトライ
                                    this.WriteChar( (char)RsChar.NAK );
                                    check_counter = 0;
                                    data_counter = 0;
                                    continue;
                                }
                            }
                            catch
                            {
                                // ＮＡＫを送信しリトライ
                                this.WriteChar( (char)RsChar.NAK );
                                check_counter = 0;
                                data_counter = 0;
                                continue;
                            }
                        }
                    }
                    // テキストデータをバッファーに入れる。
                    else
                    {
                        if ( (char)c >= 0x20 )
                        {
                            // 排他的論理和を取る
                            if ( checkLen == 2 )
                            {
                                sum ^= (Int32)c;
                            }
                            else
                            {
                                sum += (Int32)c;
                            }
                            databuf[data_counter++] = c;
                        }
                    }
                    break;
                }

                // もしデータ数が許容範囲を超えたら失敗
                if ( data_counter >= URsCom.MAX_BLOCKSIZE )
                {
                    return URsCom.FALSE;
                }
            }
        }

        #endregion

    }
}