using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Log;
using Oelco.Common.Utility;

namespace Oelco.CarisX.Log
{
    /// <summary>
    /// ログ出力キュー監視スレッドクラス
    /// </summary>
    public class CarisXLogThread : LogThread
    {
        /// <summary>
        /// ログ出力
        /// </summary>
        private CarisXLogWriter CarisXLogWriter = null;
        /// <summary>
        /// ログ情報キュー
        /// </summary>
        private LockObject<Queue<CarisXLogInfo>> logQueue = new LockObject<Queue<CarisXLogInfo>>();

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <remarks>
        /// アプリケーション開始時に呼び出します。
        /// </remarks>
        /// <param name="strlogDirectory">ログ出力先</param>
        /// <param name="logPrefix">ログプレフィックス名</param>
        /// <param name="maxLogLines">ログファイル内の最大行数</param>
        /// <param name="maxLogFileCount">ログファイルローテーション数</param>
        /// <param name="writeInterval">キュー待ちインターバル</param>
        /// <param name="isDebug">True：デバッグログ出力あり　False：デバッグ出力なし</param>
        /// <param name="intKeepSpan">ログ保持期限</param>
        public override void Initialize( String strlogDirectory,
                            String logPrefix,
                            Int32 maxFileLineCount,
                            Int32 fileSeqNoLotateCount,
                            Int32 writeInterval,
                            Boolean isDebug,
                            TimeSpan  intKeepSpan)
        {
            if ( this.CarisXLogWriter == null )
            {
                this.CarisXLogWriter = new CarisXLogWriter();
                this.Logwriter = this.CarisXLogWriter;

                this.CarisXLogWriter.LogDir = strlogDirectory;
                this.CarisXLogWriter.LogPrefix = logPrefix;
                this.CarisXLogWriter.MaxLogLines = maxFileLineCount;
                this.CarisXLogWriter.MaxLogLotateSeq = fileSeqNoLotateCount;
                this.CarisXLogWriter.Debug = isDebug;
                this.CarisXLogWriter.KeepSpan = intKeepSpan;
                this.Interval = writeInterval;

                //  保持日数より古いログファイルを削除する
                this.CarisXLogWriter.DeleteOldLog();
            }
        }


        /// <summary>
        /// ログ情報をキューに追加
        /// </summary>
        /// <remarks>
        /// ログ情報をキューに追加します
        /// </remarks>
        /// <param name="log"></param>
        public virtual void Enqueue( CarisXLogInfo log )
        {
            try
            {
                this.logQueue.Get.Instance.Enqueue( log );
            }
            catch(Exception ex )
            {
                System.Diagnostics.Debug.WriteLine( String.Format("ログキューの追加に失敗しました:{0}", ex.Message ));                  
            }
        }

        /// <summary>
        /// キュー監視・ログ出力処理
        /// </summary>
        /// <remarks>
        /// キュー監視・ログ出力処理します
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

        /// <summary>
        /// ログ出力
        /// </summary>
        /// <remarks>
        /// ログ出力します
        /// </remarks>
        private void putLog()
        {

            try
            {
                this.logQueue.Lock();
                if ( this.CarisXLogWriter != null )
                {
                    // キューにある分処理
                    Int32 nCount = this.logQueue.Get.Instance.Count;
                    for ( Int32 i = 0; i < nCount; i++ )
                    {
                        if ( logQueue.Get.Instance.Count != 0 )
                        {
                            CarisXLogInfo info = logQueue.Get.Instance.Dequeue();
                            if ( info != null )
                            {
                                // ログ出力
                                if (info.Kind == LogKind.DebugLog || info.Kind == LogKind.OnlineHist)//Figu:修改判断条件，增加于2018/5/3
                                {
                                    this.CarisXLogWriter.WriteFile( info );
                                }
                                else
                                {
                                    this.CarisXLogWriter.WriteDB( info );
                                }
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("Error: Log queue content acquisition failure");
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Error: ログキュークエリが失敗しました");

                        }
                    }
                }
            }
            catch ( Exception ex )
            {
                System.Diagnostics.Debug.WriteLine("Failed to log output:{0} {1}", ex.Message, ex.StackTrace);
                try
                {
                    using ( System.IO.FileStream hStream = new System.IO.FileStream( Oelco.Common.Utility.SubFunction.GetApplicationDirectory() + @"\error.txt", System.IO.FileMode.Append, System.IO.FileAccess.Write ) )
                    {
                        // 作成時に返される FileStream を利用して閉じる
                        if ( hStream != null )
                        {
                            using ( System.IO.StreamWriter writer = new System.IO.StreamWriter( hStream ) )
                            {
                                writer.WriteLine("{2}Failed to log output:{0} {1}", ex.Message, ex.StackTrace, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));
                            }
                            hStream.Close();
                        }
                    }
                }
                catch ( Exception )
                {
                }
            }
            finally
            {
                this.logQueue.UnLock();
            }
        }

    }

}
