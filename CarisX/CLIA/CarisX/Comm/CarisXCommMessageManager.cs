using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Comm;

namespace Oelco.CarisX.Comm
{
    /// <summary>
    /// CarisX通信メッセージ管理
    /// </summary>
    /// <remarks>
    /// CarisXの通信によるコマンド受信通知イベントの管理を行います。
    /// </remarks>
    public class CarisXCommMessageManager : CommMessageManager
    {
        //public event EventHandler<CommCommandEventArgs> ReceiveXXXCommand;

        #region [protectedメソッド]

        /// <summary>
        /// コマンド受信イベント呼び出し処理
        /// </summary>
        /// <remarks>
        /// 受信通知、及びコマンド個別の受信通知を行います。
        /// </remarks>
        /// <param name="command">コマンドデータ</param>
        protected override void OnCommCommand( CommCommand command )
        {
            // 各コマンドに対応したイベントの呼び出しを行う。
            base.OnCommCommand( command );
            //this.receiveEventCall( command );
        }

        ///// <summary>
        ///// 受信イベント呼び出し処理
        ///// </summary>
        ///// <remarks>
        ///// コマンド個別の受信通知を行い間す。
        ///// </remarks>
        ///// <param name="command">コマンドデータ</param>
        //protected void receiveEventCall( CommCommand command )
        //{
        //    // TODO:コマンド別呼び出し定義

        //    // CommKindで種別毎のイベント呼び出しを行う。
        //    // CommKindは、コマンドクラスが埋め込みで持っているenum値
        //    switch (((CarisXCommCommand)command).CommKind)
        //    {
        //    case CommandKind.Unknown:
        //        // この流れで追記していく
        //        if ( ReceiveXXXCommand != null )
        //        {
        //            ReceiveXXXCommand( this, new CommCommandEventArgs( command ) );
        //        }
        //        break;
        //    default:
        //        break;
        //    }
        //}

        #endregion

    }
}
