﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Sock;
using Oelco.CarisX.Const;


namespace Oelco.Common.Comm
{

    /// <summary>
    /// ソケット通信オブジェクト
    /// </summary>
    /// <remarks>
    /// ソケットによる通信処理を定義します。
    /// </remarks>
    public class CarisXCommSocket : IComm
    {
        #region [定数定義]

        /// <summary>
        /// 接続完了
        /// </summary>
        private const Int32 FIN_CONNECT = 1;

        /// <summary>
        /// エラー終了Error end
        /// </summary>
        private const Int32 ERR_KIND_TERMINATED = -3;

        #endregion

        #region [インスタンス変数定義]

        // TODO:ソケット通信オブジェクト
        /// <summary>
        /// ソケットパラメータ
        /// </summary>
        private List<SocketParameter> sockParam = null;

        /// <summary>
        /// ソケット接続ライブラリ
        /// </summary>
        private SockLIB socklib = null;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 受信データ存在確認の取得
        /// </summary>
        public Boolean IsDataReceived
        {
            get
            {
                return true;

                // データ受信しているかをチェック
                //                return this.ns.DataAvailable;
                //throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 接続状態の取得
        /// </summary>
        public ConnectionStatus ConnectionStatus
        {
            get
            {
                ConnectionStatus conSts;
                //接続確認チェック
                Boolean bret = socklib.IsConnected(0);

                if ( bret == true )
                {
                    conSts = ConnectionStatus.Online;
                }
                else
                {
                    conSts = ConnectionStatus.Offline;
                }
                return conSts;
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
            if ( socklib != null || sockParam == null || sockParam.Count == 0)
            {
                return false;
            }

            //SockLIBクラスを作成
            //            socklib = new SockLIB((float)0.5, (float)2.0);
            socklib = new SockLIB( (float)0.5,
                        this.sockParam[0].TimeOut,
                        this.sockParam[0].R_Port,
                        (short)MachineCode.PC,
                        this.sockParam[0].S_Port,
                        this.sockParam[0].IpAddress.ToString(),
                        (short)this.sockParam[0].ModuleId);

            if (sockParam.Count > 1)
            {
                for (int sockIdx = 1; sockIdx < sockParam.Count; sockIdx++)
                {
                    bool isAdd = socklib.AddClient(
                                        this.sockParam[sockIdx].S_Port,
                                        this.sockParam[sockIdx].IpAddress.ToString(),
                                        (short)this.sockParam[sockIdx].ModuleId);
                    if (isAdd == false)
                    {
                        break;
                    }
                }
            }

            return true;
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
            if ( socklib == null )
            {
                return false;
            }

            socklib.Dispose();
            socklib = null;

            return true;
        }

        /// <summary>
        /// 接続無効処理
        /// </summary>
        /// <remarks>
        /// 接続無効処理。Close()の前にCloseSession()を呼び出すようにする。
        /// </remarks>
        public void CloseSession()
        {
            if ( socklib == null )
            {
                return;
            }

            socklib.CloseSession();

            return;

        }

        /// <summary>
        /// 接続パラメータ設定
        /// </summary>
        /// <remarks>
        /// 接続パラメータ設定処理を実装します。
        /// 接続パラメータの内容は実装に依存します。
        /// </remarks>
        /// <param name="connectParam">接続パラメータ</param>
        public void SetConnectParam( object connectParam )
        {
            // ソケットパラメータ（ＩＰアドレス、ポート番号）設定
            this.sockParam = connectParam as List<SocketParameter>;

        }

        /// <summary>
        /// テキストデータ送信
        /// </summary>
        /// <remarks>
        /// テキストデータ送信処理を実装します。
        /// </remarks>
        /// <param name="data">送信データ</param>
        /// <returns>データ長（＞１）「０」：未接続、「-1」：送信異常、「-2」タイムアウト異常、「-3」：スレッド終了、「-4」パラメータ異常</returns>
        public Int32 SendText(string data)
        {
            if (socklib == null)
            {
                return CarisXCommSocket.ERR_KIND_TERMINATED;
            }

            //接続処理
            Int32 iret = socklib.OpenSession();

            if (iret != CarisXCommSocket.FIN_CONNECT)
            {
                return iret;
            }

            //送信処理
            iret = socklib.SendStream(data, data.Length);

            return iret;
        }

        /// <summary>
        /// テキストデータ送信
        /// </summary>
        /// <remarks>
        /// テキストデータ送信処理を実装します。
        /// </remarks>
        /// <param name="data">送信データ</param>
        /// <returns>データ長（＞１）「０」：未接続、「-1」：送信異常、「-2」タイムアウト異常、「-3」：スレッド終了、「-4」パラメータ異常</returns>
        public Int32 SendText( string data, short id )
        {
            if ( socklib == null )
            {
                return CarisXCommSocket.ERR_KIND_TERMINATED;
            }

            //接続処理
            Int32 iret = socklib.OpenSession(id);

            if ( iret != CarisXCommSocket.FIN_CONNECT )
            {
                return iret;
            }

            //送信処理
            iret = socklib.SendStream( data, data.Length, id );

            return iret;
        }

        /// <summary>
        /// テキストデータ受信
        /// </summary>
        /// <remark>
        /// テキストデータ受信処理を実装します。
        /// </remark>
        /// <param name="data">データ受信先</param>
        /// <returns>データ長（＞１）、「-1」：受信異常、「-3」：スレッド終了、「-4」パラメータ異常　</returns>
        public Int32 RecvText(out string data)
        {
            if (socklib == null)
            {
                data = "";
                //System.Threading.Thread.Sleep(100);
                return CarisXCommSocket.ERR_KIND_TERMINATED;
            }
            //データ受信処理
            short id = 0;
            Int32 ilen = socklib.ReceiveStream(out data, out id);

            return ilen;
        }

        /// <summary>
        /// テキストデータ受信
        /// </summary>
        /// <remark>
        /// テキストデータ受信処理を実装します。
        /// </remark>
        /// <param name="data">データ受信先</param>
        /// <returns>データ長（＞１）、「-1」：受信異常、「-3」：スレッド終了、「-4」パラメータ異常　</returns>
        public Int32 RecvText(out string data, out short id)
        {
            if (socklib == null)
            {
                data = "";
                id = 0;
                //System.Threading.Thread.Sleep(100);
                return CarisXCommSocket.ERR_KIND_TERMINATED;
            }
            //データ受信処理
            Int32 ilen = socklib.ReceiveStream(out data, out id);

            return ilen;
        }

        /// <summary>
        /// テキストデータ送信
        /// </summary>
        /// <remarks>
        /// テキストデータ送信処理を実装します。
        /// </remarks>
        /// <param name="data">送信データ</param>
        /// <returns>データ長（＞１）「０」：未接続、「-1」：送信異常、「-2」タイムアウト異常、「-3」：スレッド終了、「-4」パラメータ異常</returns>
        public Int32 SendData( ValueType[] data, short id )
        {
            if ( socklib == null )
            {
                return CarisXCommSocket.ERR_KIND_TERMINATED;
            }

            //接続処理
            Int32 iret = socklib.OpenSession(id);

            if ( iret != CarisXCommSocket.FIN_CONNECT )
            {
                return iret;
            }

            socklib.ResetUseMonitor();

            //送信処理
            iret = socklib.SendStream( data, data.Length, id );

            socklib.SetUseMonitor();

            return iret;
        }

        /// <summary>
        /// テキストデータ受信
        /// </summary>
        /// <remark>
        /// テキストデータ受信処理を実装します。
        /// </remark>
        /// <param name="data">データ受信先</param>
        /// <returns>データ長（＞１）、「-1」：受信異常、「-3」：スレッド終了、「-4」パラメータ異常　</returns>
        public Int32 RecvData( out ValueType[] data, out short id )
        {
            if ( socklib == null )
            {
                data = null;
                id = 0;
                return CarisXCommSocket.ERR_KIND_TERMINATED;
            }
            socklib.ResetUseMonitor();

            //データ受信処理
            Int32 ilen = socklib.ReceiveStream( out data, out id );

            socklib.SetUseMonitor();

            return ilen;

        }


        /// <summary>
        /// 通信ログファイルパス設定
        /// </summary>
        /// <remark>
        /// 通信ログファイルパス設定
        /// </remark>
        /// <param name="path">通信ログ保存先</param>
        public void SaveFilePath(string path)
        {
            if (socklib == null)
            {
                return;
            }

            //ファイルパス設定
            socklib.SaveFilePath(path);
        }

        /// <summary>
        /// 通信ログファイルパス設定
        /// </summary>
        /// <remark>
        /// 通信ログファイルパス設定
        /// </remark>
        /// <param name="path">通信ログ保存先</param>
        public void SaveFilePath( string path, short id )
        {
            if ( socklib == null )
            {
                return;
            }

            //ファイルパス設定
            socklib.SaveFilePath( path, id );
        }

        /// <summary>
        /// 通信ログ出力起動
        /// </summary>
        /// <remark>
        /// 通信ログ出力起動
        /// </remark>
        /// <param name="sw">True:モニタ起動 False:モニタ未起動</param>
        public void setMonitor( Boolean sw )
        {
            if ( socklib == null )
            {
                return;
            }

            if ( sw )
            {
                //モニターを起動する。
                socklib.SetUseMonitor();
            }
            else
            {
                //モニターを起動しない。
                socklib.ResetUseMonitor();
            }
        }

        /// <summary>
        /// 装置ID設定
        /// </summary>
        /// <remark>
        /// 装置ID設定
        /// </remark>
        /// <param name="MyId">自装置ID</param>
        /// <param name="OtherId">接続先装置ID</param>
        public void SetMachineCode( short MyId, short OtherId )
        {
            if ( socklib == null )
            {
                return;
            }

            //接続元、接続先IDの設定
            socklib.SetMachineCode( MyId, OtherId );
        }

        /// <summary>
        /// 接続状態の取得
        /// </summary>
        public ConnectionStatus GetConnectionStatus(short id)
        {
            ConnectionStatus conSts;
            //接続確認チェック
            Boolean bret = false;

            try
            {
                bret = socklib.IsConnected(id);
            }
            catch (Exception)
            {
                //socklibのインスタンスがない場合はオフライン扱いとする
            }

            if (bret == true)
            {
                conSts = ConnectionStatus.Online;
            }
            else
            {
                conSts = ConnectionStatus.Offline;
            }
            return conSts;
        }

        #endregion
    }
}
