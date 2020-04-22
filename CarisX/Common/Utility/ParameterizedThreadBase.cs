using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System;
using System.Diagnostics;


namespace Oelco.Common.Utility
{
    /// <summary>
    /// パラメータ付きスレッド処理
    /// </summary>
    public abstract class ParameterizedThreadBase : ThreadBase
    {
        /// <summary>
        /// パラメータ付きスレッド処理デリゲート
        /// </summary>
        /// <param name="parameter"></param>
        protected delegate void ParameterizedThread( Object parameter );

        // この二つをabstructなプロパティにすることで、静的な関数をスレッドとして扱える。
        /// <summary>
        /// パラメータ付きスレッド処理
        /// </summary>
        abstract protected ParameterizedThread ThreadFunction
        {
            get;
        }
        /// <summary>
        /// スレッド用パラメータ
        /// </summary>
        abstract protected Object ThreadParameter
        {
            get;
        }

        //abstract ParameterizedThreadStart ThreadFunction 
        //abstract protected void threadFunction( Object parameter );

        /// <summary>
        /// スレッド開始
        /// </summary>
        /// <remarks>
        /// スレッドの動作を開始します。
        /// </remarks>
        /// <returns>開始結果(true:成功/false:失敗)</returns>
        public override Boolean Start()
        {
            return this.Start( new ParameterizedThreadStart( this.ThreadFunction ), this.ThreadParameter );
        }


        /// <summary>
        /// スレッド開始
        /// </summary>
        /// <remarks>
        /// スレッド関数の動作を開始します。
        /// </remarks>
        /// <returns>開始結果(true:成功/false:失敗)</returns>
        protected virtual Boolean Start( ParameterizedThreadStart threadthreadStart, Object parameter )
        {
            // threadFunction開始
            this.Thread = new Thread( threadthreadStart );

            // スレッドに名前を付ける
            StackTrace st = new StackTrace( false );
            this.Thread.Name = st.GetFrame( 1 ).GetMethod().ReflectedType.FullName;

            try
            {
                // シグナル状態
                this.EndEvent = new ManualResetEvent( false );
                // 開始
                this.Thread.Start();
            }
            catch ( Exception ex )
            {
                // イベントログ
                EventLog.WriteEntry( System.Reflection.Assembly.GetExecutingAssembly().ToString(),
                    ex.ToString(), EventLogEntryType.Warning );
                return false;
            }

            return true;
        }

    }
}
