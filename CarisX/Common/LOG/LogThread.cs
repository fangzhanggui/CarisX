//----------------------------------------------------------------
// Public Class.
//	  LogThread
// Info.
//   ログ出力スレッドクラス
// History
//   2011/09/01　　Ver1.00.00　　新規作成
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using Oelco.Common.Utility;

namespace Oelco.Common.Log
{
    /// <summary>
    /// ログ出力スレッド
    /// キューにセットされたログを出力する
    /// </summary>
    public class LogThread : ThreadBase
    {
        #region [インスタンス変数定義]
        
        /// <summary>
        /// ログキュー
        /// </summary>
        private LockObject<Queue<LogInfo>> m_queue = new LockObject<Queue<LogInfo>>();

        /// <summary>
        /// ログ出力
        /// </summary>
        private LogWriter m_logwriter = null;
        
        #endregion
                
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LogThread()
        {
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ログ出力の取得、設定
        /// </summary>
        protected LogWriter Logwriter
        {
            get
            {
                return m_logwriter;
            }
            set
            {
                m_logwriter = value;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 初期化
        /// </summary>
        /// <remarks>
        /// 初期化処理を行います。
        /// ※必ずアプリケーション開始時に呼び出して下さい
        /// </remarks>
        /// <param name="strLogDir">ログ出力先</param>
        /// <param name="strLogPrefix">ログプレフィックス名</param>
        /// <param name="maxLogLines">ログファイル内の最大行数</param>
        /// <param name="maxLogFileCount">ログファイルローテーション数</param>
        /// <param name="interval">キュー待ちインターバル</param>
        /// <param name="bDebug">True：デバッグログ出力あり　False：デバッグ出力なし</param>
        /// <param name="keepDay">ログファイル保持日数</param>
        public virtual void Initialize( String strLogDir,
                            String strLogPrefix,
                            Int32 intMaxLines,
                            Int32 intLotateCount,
                            Int32 interval,
                            Boolean bDebug,
                            TimeSpan keepDay)
        {
            this.m_logwriter = new LogWriter( strLogDir,
                                            strLogPrefix,
                                            intMaxLines,
                                            intLotateCount,
                                            bDebug );
                                            
            this.Interval = interval;

            //  保持日数より古いログファイルを削除する
            this.m_logwriter.DeleteOldLog();

        }

        /// <summary>
        /// ログ情報をキューに追加
        /// </summary>
        /// <remarks>
        /// ログ情報をキューに追加します。
        /// </remarks>
        /// <param name="log"></param>
        public virtual void Enqueue( LogInfo log )
        {
            // ログをキューに積む際にロックする
            this.m_queue.Lock();
            if ( log == null )
            {
                return;
            }
            this.m_queue.Get.Instance.Enqueue( log );
            this.m_queue.UnLock();
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// キュー監視・ログ出力処理
        /// </summary>
        /// <remarks>
        /// キュー監視及びログ出力処理を行います。
        /// </remarks>
        protected override void threadFunction()
        {
            // TODO:初期化してなかったら落ちる
            // 無限ループ
            while ( !this.EndEvent.WaitOne( this.Interval ) )
            {
                //// TODO : debug
                //Console.WriteLine( String.Format( " LogThread:threeadFunction (interval:{0}) now:{1}", this.Interval, DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss.fff" ) ) );

                putLog();
            }
            putLog();
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// ログ出力
        /// </summary>
        /// <remarks>
        /// ログ出力処理を行います。
        /// </remarks>
        private void putLog()
        {
            this.m_queue.Lock();
            if ( m_logwriter != null )
            {
                // キューに載っている分処理
                Int32 nCount = this.m_queue.Get.Instance.Count;
                for ( Int32 i = 0; i < nCount; i++ )
                {
                    LogInfo info = m_queue.Get.Instance.Dequeue();

                    // ログ情報がnullの場合はExceptionを出力する
                    try
                    {
                        // ログ出力
                        if( info.Kind == LogKind.DebugLog )
                        {
                            this.m_logwriter.WriteFile( info );
                        }
                        else
                        {
                            this.m_logwriter.WriteDB( info );
                        }
                    }
                    catch( Exception ex )
                    {
                        info = new LogInfo();
                        info.Kind = LogKind.DebugLog;
                        info.Level = LogLevel.Error;
                        info.WriteDateTime = DateTime.Now;
                        info.UserId = String.Empty;
                        info.Contents.Add( ex.Message + ex.StackTrace );
                        this.m_logwriter.WriteFile( info );
                    }
                }
            }
            this.m_queue.UnLock();
        }

        #endregion

    }

}
