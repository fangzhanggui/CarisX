//----------------------------------------------------------------
// Public Class.
//	  ThreadBase
// Info.
//   スレッド処理
// History
//   2011/09/01　　Ver1.00.00　　新規作成
//----------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System;
using System.Diagnostics;

namespace Oelco.Common.Utility
{
    /// <summary>
    /// スレッド処理
    /// </summary>
    /// <remarks>
    /// スレッド動作を行うクラスは、このクラスを継承します。
    /// </remarks>
    public abstract class ThreadBase
    {
        #region [ 定数定義 ]

        /// <summary>
        /// スレッド終了待機時間
        /// </summary>
    	//↓↓↓アプリケーション終了時の例外発生対応 2019/2/19↓↓↓
        // 全ての通信が切断されている場合、ホスト、ラック、スレーブ×４で同時に
        // Connect処理が行われる可能性があり、それぞれ2秒のタイムアウト待ちが
        // 発生することから、スレッド処理が通信だけで12秒は待たされる可能性がある。
        // 
        // 他にも待ちを行う処理があるため、余裕を見てその倍の24秒を設定している。
        private const Int32 THREAD_END_JOIN_TIMEOUT = 5000;
        //private const Int32 THREAD_END_JOIN_TIMEOUT = 24 * 1000;
        //↑↑↑アプリケーション終了時の例外発生対応 2019/2/19↑↑↑

        #endregion

        #region [ インスタンス変数定義 ]

        /// <summary>
        /// スレッドインスタンス
        /// </summary>
        private Thread thread = null;

        /// <summary>
        /// スレッド周期時間(ms)
        /// </summary>
        private Int32 interval = 1 * 1000; // 1秒
        /// <summary>
        /// スレッド終了イベント
        /// </summary>
        private ManualResetEvent threadEndEvent = null;

        #endregion

        #region [ プロパティ ]

        /// <summary>
        /// ループ待機時間
        /// </summary>
        public Int32 Interval
        {
            get
            {
                return this.interval;
            }
            set
            {
                this.interval = value;
            }
        }

        /// <summary>
        /// スレッドの状態を返す
        /// </summary>
        public Boolean IsAlive
        {
            get
            {
                if ( this.thread == null )
                {
                    return false;
                }
                return this.thread.IsAlive;
            }
        }

        /// <summary>
        /// スレッド停止イベント
        /// </summary>
        protected ManualResetEvent EndEvent
        {
            get
            {
                return this.threadEndEvent;
            }
            set
            {
                this.threadEndEvent = value;
            }
        }

        /// <summary>
        /// スレッドインスタンス
        /// </summary>
        protected Thread Thread
        {
            get
            {
                return thread;
            }
            set
            {
                thread = value;
            }
        }

        #endregion

        #region [ メソッド ]

        /// <summary>
        /// スレッド開始
        /// </summary>
        /// <remarks>
        /// スレッド関数の動作を開始します。
        /// </remarks>
        public virtual Boolean Start()
        {
            // threadFunction開始
            this.thread = new Thread( this.threadFunction );

            // スレッドに名前を付ける
            StackTrace st = new StackTrace( false );
            this.thread.Name = st.GetFrame( 1 ).GetMethod().ReflectedType.FullName;

            try
            {
                // シグナル状態
                this.EndEvent = new ManualResetEvent( false );
                // 開始
                this.thread.Start();
            }
            catch ( Exception ex )
            {
                // TODO:とりあえずイベントログに出しておきます
                EventLog.WriteEntry( System.Reflection.Assembly.GetExecutingAssembly().ToString(),
                    ex.ToString(), EventLogEntryType.Warning );
                return false;
            }

            return true;
        }

        ///// <summary>
        ///// スレッド終了
        ///// </summary>
        ///// <remarks>
        ///// スレッド停止依頼
        ///// </remarks>
        //public virtual void End()
        //{
        //    try
        //    {
        //        this.EndEvent.Set();
        //        this.EndJoin();
        //        this.thread = null;
        //    }
        //    catch ( Exception ex )
        //    {
        //        // TODO:とりあえずイベントログに出しておきます
        //        EventLog.WriteEntry( System.Reflection.Assembly.GetExecutingAssembly().ToString(),
        //            ex.ToString(), EventLogEntryType.Warning );
        //    }
        //}

        /// <summary>
        /// スレッド停止
        /// </summary>
        /// <remarks>
        /// スレッド停止待ちを行う、スレッドが終了するまで待機される。
        /// </remarks>
        /// <returns>true:成功、false:失敗</returns>
        public virtual Boolean EndJoin()
        {
            // threadFunctionの終了待ち
            try
            {
                this.EndEvent.Set();

                if ( !this.thread.Join( THREAD_END_JOIN_TIMEOUT ) )
                {
                    // タイムアウト時
                    this.Abort();
                }
                this.thread = null;
            }
            catch ( Exception ex )
            {
                // TODO:とりあえずイベントログに出しておきます
                EventLog.WriteEntry( System.Reflection.Assembly.GetExecutingAssembly().ToString(),
                    ex.ToString(), EventLogEntryType.Warning );
                return false;
            }

            return true;
        }

        /// <summary>
        /// 強制終了
        /// </summary>
        /// <remarks>
        /// スレッドの強制終了
        /// </remarks>
        /// <returns>true:成功、false:失敗</returns>
        public virtual Boolean Abort()
        {
            // スレッドの強制終了
            try
            {
                this.thread.Abort();
                this.thread = null;
            }
            catch ( Exception ex )
            {
                // TODO:とりあえずイベントログに出しておきます
                EventLog.WriteEntry( System.Reflection.Assembly.GetExecutingAssembly().ToString(),
                    ex.ToString(), EventLogEntryType.Warning );
                return false;
            }

            return true;
        }

        /// <summary>
        /// スレッド動作関数
        /// </summary>
        /// <remarks>
        /// ここでは空。
        /// 派生したthreadFunctionはできる限りEndEventを
        /// 監視すること。
        /// </remarks>
        protected virtual void threadFunction()
        {
        }

        #endregion
    }
}
