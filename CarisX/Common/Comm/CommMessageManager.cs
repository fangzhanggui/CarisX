using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.Common.Comm
{
    /// <summary>
    /// 通信メッセージ管理
    /// </summary>
    /// <remarks>
    /// 通信によるコマンド受信通知イベントの管理を行います。
    /// </remarks>
    public class CommMessageManager
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// コマンド受信イベント
        /// </summary>
        public event EventHandler<CommCommandEventArgs> ReceiveCommCommand;

        /// <summary>
        /// コマンドキュー
        /// </summary>
        private Queue<CommCommand> commandQueue = new Queue<CommCommand>();

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド追加
        /// </summary>
        /// <remarks>
        /// 通知するコマンドを追加します。
        /// </remarks>
        /// <param name="command">コマンドデータ</param>
        public void PushCommand( CommCommand command )
        {
            this.commandQueue.Enqueue( command );
        }

        /// <summary>
        /// イベント発生
        /// </summary>
        /// <remarks>
        /// 保持しているコマンドキューから、通知イベントを発生させます。
        /// </remarks>
        public virtual void RaiseEvent()
        {
            // キューにあるコマンドを全て通知する。
            while ( this.commandQueue.Count != 0 )
            {
                this.OnCommCommand( this.commandQueue.Dequeue() );
            }
            //            this.commandQueue
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// コマンド受信イベント呼び出し処理
        /// </summary>
        /// <remarks>
        /// 受信通知を行います。
        /// </remarks>
        /// <param name="command">コマンドデータ</param>
        protected virtual void OnCommCommand( CommCommand command )
        {
            // TODO:Caris,AFTの特化実装は、このOnCommCommand関数をオーバーライドして、個別コマンドイベントを呼び出すようにする。
            if ( this.ReceiveCommCommand != null )
            {
                this.ReceiveCommCommand( this, new CommCommandEventArgs( command ) );
            }
        }

        #endregion


        ///// <summary>
        ///// コマンドキュー 取得/設定
        ///// </summary>
        //protected Queue<CommCommand> CommandQueue
        //{
        //    get
        //    {
        //        return commandQueue;
        //    }
        //    set
        //    {
        //        commandQueue = value;
        //    }
        //}

        // TODO:通信メッセージ管理


    }

    /// <summary>
    /// コマンドイベントクラス
    /// </summary>
    /// <remarks>
    /// 通信コマンド用イベントハンドラで使用されるクラス
    /// </remarks>
    public class CommCommandEventArgs : EventArgs
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// コマンドデータ
        /// </summary>
        CommCommand command;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="command">コマンドデータ</param>
        public CommCommandEventArgs( CommCommand command )
        {
            this.command = command;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// コマンドデータ 取得
        /// </summary>
        public CommCommand Command
        {
            get
            {
                return command;
            }
        }

        #endregion

    }
}
