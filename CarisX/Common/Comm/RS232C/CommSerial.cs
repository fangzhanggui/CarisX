using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Oelco.Common.Comm
{

    /// <summary>
    /// シリアル通信パラメータプロバイダー
    /// </summary>
    static public class SerialParameterProvider
    {
        /// <summary>
        /// チェックエリア
        /// </summary>
        public static CheckArea CheckArea = CheckArea.TO_ETX;

        /// <summary>
        /// 2バイトチェック領域フラグ
        /// </summary>
        public static Boolean Is2CheckByteLen = false;
    }
 
    /// <summary>
    /// シリアル通信オブジェクト
    /// </summary>
    /// <remarks>
    /// RS232Cによる通信処理を定義します。
    /// </remarks>
    public class CommSerial : IComm
    {
        #region [インスタンス変数定義]
        
        /// <summary>
        /// シリアルCOMオブジェクト
        /// </summary>
        private URsCom commObject = null;//= new URsCom(

        /// <summary>
        /// シリアル接続パラメータ
        /// </summary>
        private SerialParameter serialParameter = null;

        /// <summary>
        /// 接続状態
        /// </summary>
        private ConnectionStatus connectionStatus = ConnectionStatus.Offline;

        /// <summary>
        /// ミューテックス
        /// </summary>
        private Mutex commMutex = new Mutex();

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 受信データ存在確認
        /// </summary>
        public Boolean IsDataReceived
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 接続状態の取得
        /// </summary>
        public ConnectionStatus ConnectionStatus
        {
            get
            {
                return this.connectionStatus;
            }
        }

        /// <summary>
        /// ログの取得
        /// </summary>
        public List<String> Log
        {
            get
            {
                if ( this.commObject != null)
                {
                    return commObject.Log;
                }
                return new List<String>();
            }
        }

        /// <summary>
        /// 同步后的在线日志
        /// </summary>
        public List<String> SycLog
        {
            get
            {
                if (this.commObject != null)
                {
                    return commObject.SycLog;
                }
                return new List<String>();
            }
        }
  

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 接続オープン
        /// </summary>
        /// <remarks>
        /// 接続のオープン処理を実装します。
        /// </remarks>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean Open()
        {
            this.cleanupInstance();

            this.commObject = new URsCom( this.serialParameter.CommPort,
                this.serialParameter.BaudRate,
                this.serialParameter.Parity,
                this.serialParameter.DataBits,
                this.serialParameter.StopBits,
                SerialParameterProvider.CheckArea,
                SerialParameterProvider.Is2CheckByteLen,
                this.serialParameter.ReadTimeout );

            Boolean openSuccess = this.commObject.Open();
            if ( openSuccess )
            {
                this.connectionStatus = Comm.ConnectionStatus.Online;
            }
            return openSuccess;
        }

        /// <summary>
        /// 接続クローズ
        /// </summary>
        /// <remarks>
        /// 接続のクローズ処理を実装します。
        /// </remarks>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean Close()
        {
            Boolean closeSuccess = false;

            if ( ( this.commObject != null )
                && ( this.commObject.IsOpen ) )
            {
                this.commObject.Close();
                closeSuccess = true;
                this.connectionStatus = Comm.ConnectionStatus.Offline;
            }

            return closeSuccess;
        }

        /// <summary>
        /// 接続無効処理
        /// </summary>
        /// <remarks>
        /// 接続無効処理。Close()の前にCloseSession()を呼び出すようにする。
        /// </remarks>
        public void CloseSession()
        {
            // 処理無し（CommSocketにあるので、メソッドだけ作りました）
        }

        /// <summary>
        /// 通信ログファイルパス設定
        /// </summary>
        /// <remark>
        /// 通信ログファイルパス設定します
        /// </remark>
        /// <param name="path">通信ログ保存先</param>
        public void SaveFilePath( string path )
        {
            // 処理無し（CommSocketにあるので、メソッドだけ作りました）
        }

        /// <summary>
        /// 通信ログ出力起動
        /// </summary>
        /// <remark>
        /// 通信ログ出力起動します
        /// </remark>
        /// <param name="sw">True:モニタ起動 False:モニタ未起動</param>
        public void setMonitor( Boolean sw )
        {
            // 処理無し（CommSocketにあるので、メソッドだけ作りました）
        }

        /// <summary>
        /// 装置ID設定
        /// </summary>
        /// <remark>
        /// 装置ID設定します
        /// </remark>
        /// <param name="MyId">自装置ID</param>
        /// <param name="OtherId">接続先装置ID</param>
        public void SetMachineCode( short MyId, short OtherId )
        {
            // 処理無し（CommSocketにあるので、メソッドだけ作りました）
        }

        /// <summary>
        /// 接続パラメータ設定
        /// </summary>
        /// <remarks>
        /// シリアル接続パラメータを設定します。
        /// </remarks>
        /// <param name="connectParam">接続パラメータ</param>
        public void SetConnectParam( object connectParam )
        {
            this.serialParameter = connectParam as SerialParameter;

            // 接続パラメータの適用
            // このパラメータはOpenした後に設定されても既存の接続に対して適用される。
            if ( this.commObject != null )
            {
                this.commObject.BaudRate = this.serialParameter.BaudRate;
                this.commObject.Parity = this.serialParameter.Parity;
                this.commObject.DataBits = this.serialParameter.DataBits;
                this.commObject.StopBits = this.serialParameter.StopBits;
            }
        }

        /// <summary>
        /// テキストデータ送信
        /// </summary>
        /// <remarks>
        /// テキストデータを送信します。
        /// </remarks>
        /// <param name="data">送信データ</param>
        /// <returns>データ長（＞１）「０」：未接続、「-1」：送信異常、「-2」タイムアウト異常、「-3」：スレッド終了、「-4」パラメータ異常</returns>
        public Int32 SendText( String data )
        {
            const Int32 OPENSESSION_CROSS = -1; // OpenSessionがホストとぶつかった場合の戻り値
            const Int32 OPENSESSION_FAILUE = 0; // OpenSessionが失敗した場合の戻り値
            const Int32 OPENSESSION_SUCCESS = 1; // OpenSessionが成功した場合の戻り値

            Int32 sendTextLen = 0;
            Int32 sendSuccess = 0;
            if ( ( this.commObject != null )
                && ( this.commObject.IsOpen ) )
            {
                Boolean trySend = true;
                Boolean waitFromHostCommandEnd = false;
                while (trySend)
                {
                    trySend = false;

                    commMutex.WaitOne();
                    Int32 openSessionResult = this.commObject.OpenSession();

                    switch (openSessionResult)
                    {

                        case OPENSESSION_CROSS:
                            // お互いからオープンセッションのEQが送出された場合、HOST側からのコマンド送信を優先する為、
                            // 受信スレッド側がコマンド処理を終えるまで待機を行なう。
                            waitFromHostCommandEnd = true;
                            trySend = true;
                            sendSuccess = -2;
                            break;

                        case OPENSESSION_FAILUE:
                            // 失敗
                            //ManagerErr.Instance.ShowErrMessage(50, 0, openErrMsg1 + ":" + com_no);
                            sendSuccess = -1;
                            break;

                        case OPENSESSION_SUCCESS:
                            // オープンセッションが正常に行なわれた場合、続けてデータ送信を行なう。
                            sendTextLen = this.commObject.Send(data);
                            if (sendTextLen == 0)
                            {
                                //                           ManagerErr.Instance.ShowErrMessage(50, 0, dataErrMsg + ":" + com_no);
                            }
                            else
                            {
                                // データ送信が正常に行なわれた場合、クローズセッションを行なう。
                                if (!this.commObject.CloseSession())
                                {
                                    // ManagerErr.Instance.ShowErrMessage(50, 0, closeErrMsg + ":" + com_no);
                                    // エラーメッセージ表示
                                }

                                sendSuccess = sendTextLen;

                            }

                            break;
                        default:
                            break;
                    }
                    commMutex.ReleaseMutex();


                    if (waitFromHostCommandEnd)
                    {
                        System.Threading.Thread.Sleep(3500);
                    }

                }
            }

            return sendSuccess;
        }

        /// <summary>
        /// テキストデータ受信
        /// </summary>
        /// <remark>
        /// テキストデータを受信します。
        /// </remark>
        /// <param name="data">データ受信先</param>
        /// <returns>データ長（＞１）、「-1」：受信異常、「-3」：スレッド終了、「-4」パラメータ異常　</returns>
        public Int32 RecvText( out String data )
        {
            Int32 receiveSuccess = 0;
            data = String.Empty;

            if ( ( this.commObject != null )
                && ( this.commObject.IsOpen ) )
            {
                // 内部で失敗した場合、0が返される。
                commMutex.WaitOne();
                Int32 ret = this.commObject.ReceiveForHost( out data );
                commMutex.ReleaseMutex();

                if ( ret != 0 )
                {
                    receiveSuccess = ret;
                }
            }

            return receiveSuccess;
        }

        #endregion
        
        #region [protectedメソッド]

        /// <summary>
        /// インスタンスのクリーン
        /// </summary>
        /// <remarks>リソースを解放します。</remarks>
        protected void cleanupInstance()
        {
            if ( this.commObject != null )
            {
                this.commObject.Dispose();
                this.commObject = null;
            }
        }


        #endregion
    }
}
