using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.Log;
using Oelco.Common.Utility;

namespace Oelco.Common.Comm
{
    /// <summary>
    /// ホスト通信手順管理
    /// </summary>
    /// <remarks>
    /// 対ホストの通信手順管理を行います
    /// </remarks>
    public class HostProtocolManager : CommProtocolManager
    {
        #region [プロパティ]

        /// <summary>
        /// ログの取得
        /// </summary>
        public new List<String> Log
        {
            get
            {
                return ( (CommSerial)this.CommObject ).Log;
            }
        }

        /// <summary>
        /// 同步后的在线日志
        /// </summary>
        public List<String> SycLog
        {
            get
            {
                return ((CommSerial)this.CommObject).SycLog;
            }
        }
     

        #endregion

        // TODO:ホスト通信手順管理
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HostProtocolManager()
        {
            this.CommObject = new CommSerial();
        }

        #endregion
        
        /// <summary>
        /// 送信
        /// </summary>
        /// <remarks>
        /// コマンドデータをテキスト化し、送信します。
        /// </remarks>
        /// <param name="command">送信コマンド</param>
        /// <returns>True:成功 False:失敗</returns>
        public override Boolean Send( CommCommand command )
        {

            // CommmProtocolManagerのレベルでは、SendText(command.CommandId.ToString("d4") + command.CommandText);
            // で送っているが、command.CommandId.ToString("d4")が不要な為オーバーライドして対応した。

            Boolean sendSuccess = false;

            if ( this.CommObject != null )
            {
                Int32 ret = 0;
                try
                {
                    ret = this.CommObject.SendText( command.CommandText );
                }
                catch ( Exception ex )
                {
                    Singleton<LogManager>.Instance.Write( Oelco.Common.Log.LogKind.DebugLog, String.Format( "*Command Send Error! Type = {0} Message = {1} StackTrace = {2}", command == null ? "unknown" : command.GetType().Name, ex.Message, ex.StackTrace ) );
                    ret = -1;   // 例外発生時は送信異常（-1）とする
                }

                if ( ret > 0 )
                {
                    sendSuccess = true;
                }

                // TODO:「０」：未接続、「-1」：送信異常、「-2」タイムアウト異常、「-3」：スレッド終了、「-4」パラメータ異常 の場合、エラー表示する。
                // TODO:ユーザープログラムのみを起動した場合は、コマンド送信時エラーが発生してもエラー表示は行なわない。
                //（G1200では接続確認コマンドが送信できた場合のみ、装置が起動していると認識し、エラー表示をするように行なっていたと思います。）
                // →今回の装置の起動状態は通信Libの接続状態を見ることで判定
                Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, String.Format( "*{3}Command Sended : Type = {0} success = {1} detail = {2}", command == null ? "unknown" : command.GetType().Name, sendSuccess.ToString(), ret.ToString(), DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss" ) ) );
                System.Diagnostics.Debug.WriteLine( String.Format( "*{3}Command Sended : Type = {0} success = {1} detail = {2}", command == null ? "unknown" : command.GetType().Name, sendSuccess.ToString(), ret.ToString(), DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss" ) ) );
                this.commandReflection( command );

                // コマンド送信結果コールバック呼び出し
                if ( dlgCommSendResult != null )
                {
                    dlgCommSendResult( ret, command );
                }
            }

            return sendSuccess;
        }
    }
}
