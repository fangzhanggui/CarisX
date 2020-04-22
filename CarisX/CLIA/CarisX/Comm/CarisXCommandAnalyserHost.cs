using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Comm;

namespace Oelco.CarisX.Comm
{
    /// <summary>
    /// 通信コマンド解析クラス
    /// </summary>
    /// <remarks>
    /// テキストデータから、通信コマンドを解析し、取得します。
    /// </remarks>
    public class CarisXCommandAnalyserHost : ICommCommandAnalyser
    {
        #region [定数定義]

        /// <summary>
        ///  コマンドID長
        /// </summary>
        private const Int32 COMMAND_ID_LENGTH = 1;
        #endregion        

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析インターフェース実装
        /// </summary>
        /// <remarks>
        /// コマンド解析を行います。
        /// </remarks>
        /// <param name="target">解析対象文字列</param>
        /// <returns>コマンドデータ</returns>
        public CommCommand AnalyseCommand( String target )
        {
            return AnalyseCarisXCommand( target );
        }

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// CarisXに使用されるコマンドを、文字列から解析します。
        /// </remarks>
        /// <param name="target">解析対象文字列</param>
        /// <returns>コマンドデータ</returns>
        public CarisXCommCommand AnalyseCarisXCommand( String target )
        {
            // 解析対象外であれば終了。
            if ( ( target == String.Empty )
                || ( target == "" )
                || ( target.Length < COMMAND_ID_LENGTH ) )
            {
                return null;
            }

            CarisXCommCommand result = null;
            Type cmdType = null;

            // コマンド識別
            String identify = target.Substring( 0, COMMAND_ID_LENGTH );
            cmdType = commandIdentify( identify );

            // インスタンス生成
            if ( cmdType != null )
            {
                result = (CarisXCommCommand)Activator.CreateInstance( cmdType );

                // データ設定 コマンドクラス内部で解析されて値が保持される。
                result.SetCommandString( target );
            }

            return result;
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// コマンドタイプ解析
        /// </summary>
        /// <remarks>
        /// コマンドIdから、コマンドのタイプを取得します。
        /// </remarks>
        /// <param name="commandId">コマンドId文字列</param>
        /// <returns>コマンドタイプ</returns>
        static protected Type commandIdentify( String commandId )
        {
            Type identified = null;
            switch ( commandId )
            {
                // 検査依頼問い合わせメッセージ委托检查咨询信息 仪器=〉主机
            case "R":
                identified = typeof( HostCommCommand_0001 );
                break;

            // 検査依頼メッセージ检查请求消息  主机=〉仪器
            case "W":
                identified = typeof( HostCommCommand_0002 );
                break;

            // 検査結果メッセージTest results message 仪器=〉主机
            case "D":
                identified = typeof( HostCommCommand_0003 );
                break;

            // 装置ステータス問い合わせメッセージ设备状态查询消息 主机=〉仪器
            case "Q":
                identified = typeof( HostCommCommand_0004 );
                break;

            // 装置ステータスメッセージDevice status message 仪器=〉主机
            case "S":
                identified = typeof( HostCommCommand_0005 );
                break;

            default:
                identified = null;
                break;

            }

            // TODO:最新のコマンドリストにより、増加分あり
            // TODO:ホスト側コマンドにも対応する。

            return identified;
        }
        #endregion

    }
}
