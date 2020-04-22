using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.Common.Comm
{
    /// <summary>
    /// 通信インターフェース
    /// </summary>
    /// <remarks>
    /// 通信に必要なインターフェースを定義します。
    /// </remarks>
    public interface IComm
    {
        #region [プロパティ]

        /// <summary>
        /// 受信データ存在確認
        /// </summary>
        Boolean IsDataReceived
        {
            get;
        }
        /// <summary>
        /// 接続状態
        /// </summary>
        ConnectionStatus ConnectionStatus
        {
            get;
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
        Boolean Open();
        /// <summary>
        /// 接続クローズ
        /// </summary>
        /// <remarks>
        /// 接続のクローズ処理を実装します。
        /// </remarks>
        /// <returns>True:成功 False:失敗</returns>
        Boolean Close();
        /// <summary>
        /// 接続無効処理
        /// </summary>
        /// <remarks>
        /// 接続無効処理。Close()の前にCloseSession()を呼び出すようにする。
        /// </remarks>
        void CloseSession();
        /// <summary>
        /// 接続パラメータ設定
        /// </summary>
        /// <remarks>
        /// 接続パラメータ設定処理を実装します。
        /// 接続パラメータの内容は実装に依存します。
        /// </remarks>
        /// <param name="connectParam">接続パラメータ</param>
        void SetConnectParam( Object connectParam );
        /// <summary>
        /// テキストデータ送信
        /// </summary>
        /// <remarks>
        /// テキストデータ送信処理を実装します。
        /// </remarks>
        /// <param name="data">送信データ</param>
        /// <returns>データ長（＞１）「０」：未接続、「-1」：送信異常、「-2」タイムアウト異常、「-3」：スレッド終了、「-4」パラメータ異常</returns>
        Int32 SendText( String data );
        /// <summary>
        /// テキストデータ受信
        /// </summary>
        /// <remark>
        /// テキストデータ受信処理を実装します。
        /// </remark>
        /// <param name="data">データ受信先</param>
        /// <returns>データ長（＞１）、「-1」：受信異常、「-3」：スレッド終了、「-4」パラメータ異常　</returns>
        Int32 RecvText(out String data);
        /// <summary>
        /// 通信ログファイルパス設定
        /// </summary>
        /// <remark>
        /// 通信ログファイルパス設定
        /// </remark>
        /// <param name="path">通信ログ保存先</param>
        void SaveFilePath(string path);
        /// <summary>
        /// 通信ログ出力起動
        /// </summary>
        /// <remark>
        /// 通信ログ出力起動
        /// </remark>
        /// <param name="sw">True:モニタ起動 False:モニタ未起動</param>
        void setMonitor(Boolean sw);
        /// <summary>
        /// 装置ID設定
        /// </summary>
        /// <remark>
        /// 装置ID設定
        /// </remark>
        /// <param name="MyId">自装置ID</param>
        /// <param name="OtherId">接続先装置ID</param>
        void SetMachineCode(short MyId, short OtherId);

        #endregion
    }

    /// <summary>
    /// 接続状態定義
    /// </summary>
    public enum ConnectionStatus
    {
        /// <summary>
        /// 非接続状態
        /// </summary>
        Offline,
        /// <summary>
        /// 接続状態
        /// </summary>
        Online,
        /// <summary>
        /// エラー発生
        /// </summary>
        Error
    }
}
