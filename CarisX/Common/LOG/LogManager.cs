//----------------------------------------------------------------
// Public Class.
//	  LogManager
// Info.
//   ログ出力クラス
// History
//   2011/09/01　　Ver1.00.00　　新規作成
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Oelco.Common.Log
{
    /// <summary>
    /// ログ出力クラス
    /// (シングルトンで生成されることを想定しています。)
    /// </summary>
    public class LogManager : IDisposable
    {
        #region [ インスタンス変数定義 ]

        /// <summary>
        /// ログスレッド
        /// </summary>
        private LogThread m_logThread = new LogThread();

        /// <summary>
        /// ログスレッド
        /// </summary>
        protected virtual LogThread LogThread
        {
            get
            {
                return m_logThread;
            }
            set
            {
                m_logThread = value;
            }
        }

        #endregion

        #region [ コンストラクタ ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LogManager()
        {
        }

        #endregion

        #region [ デストラクタ ]

        ///// <summary>
        ///// デストラクタ
        ///// </summary>
        //~LogManager()
        //{
        //    this.Dispose();
        //}

        #endregion

        #region [ メソッド ]

        /// <summary>
        /// ログ出力キュー監視スレッド開始
        /// </summary>
        /// <remarks>
        /// ログ出力キュー監視スレッドを開始します。
        /// </remarks>
        /// <returns></returns>
        protected virtual Boolean threadStart()
        {
            Boolean bRet = true;

            if ( this.LogThread.IsAlive == false )
            {
                bRet = this.LogThread.Start();
            }

            return bRet;
        }

        /// <summary>
        /// 初期化
        /// アプリケーション開始時に必ず呼び出すこと
        /// </summary>
        /// <remarks>
        /// 初期化処理を行います。
        /// </remarks>
        /// <param name="strLogDir">ログ出力先</param>
        /// <param name="strLogPrefix">出力形式は{指定したプレフィックス名}_{連番}.log</param>
        /// <param name="intMaxLogLines">ログ最大行数</param>
        /// <param name="intLotate">ログローテーション数</param>
        /// <param name="interval">キュー処理待機時間</param>
        /// <param name="blDebug">True:デバッグログ出力あり　False:デバッグログ出力なし</param>
        /// <param name="keepSpan">ログファイル保持日数</param>
        /// <remarks></remarks>
        public virtual Boolean Initialize( String strLogDir,
                    String strLogPrefix,
                    Int32 intMaxLogLines,
                    Int32 intLotate,
                    Int32 interval,
                    Boolean blDebug,
                    TimeSpan  keepSpan)
        {
            Boolean bRet;

            this.LogThread.Initialize( strLogDir,
                                    strLogPrefix,
                                    intMaxLogLines,
                                    intLotate,
                                    interval,
                                    blDebug,
                                    keepSpan);

            // ログ出力キュー監視スレッド開始
            bRet = this.threadStart();

            return bRet;
        }

        /// <summary>
        /// 解放
        /// </summary>
        /// <remarks>
        /// クラス解放時の処理を行います。
        /// </remarks>
        public virtual void Dispose()
        {
            // スレッドの停止(失敗しても無視)
            //this.m_logThread.End();
            this.LogThread.EndJoin();
        }

        /// <summary>
        /// 履歴ログ出力（DB）
        /// </summary>
        /// <remarks>
        /// 履歴ログ情報をキューに追加します。
        /// </remarks>
        /// <param name="logKind">ログ種別</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="contents">出力メッセージ</param>
        public virtual void Write( LogKind logKind, String userId, List<String> contents )
        {
            // スレッドが終了している場合は開始
            //this.threadStart();

            LogInfo info = new LogInfo();

            info.Kind = logKind;
            info.Level = LogLevel.Debug;    // ←履歴ログでは無効なのでダミーを設定
            info.WriteDateTime = DateTime.Now;
            info.UserId = userId;
            info.Contents = contents;

            this.LogThread.Enqueue( info );
        }

        /// <summary>
        /// 履歴ログ出力（DB）
        /// </summary>
        /// <remarks>
        /// 履歴ログの出力を行います。
        /// </remarks>
        /// <param name="logKind">ログ種別</param>
        /// <param name="userId">出力ユーザID</param>
        /// <param name="contents">出力メッセージ（message1, message2, message3, . . .）</param>
        public virtual void Write( LogKind logKind, String userId, params String[] contents )
        {
            List<String> cont = new List<String>();

            foreach ( String msg in contents )
            {
                cont.Add( msg );
            }

            this.Write( logKind, userId, cont );
        }

        #region [ AFTのみ操作履歴にユーザーレベルを追加 ]

        /// <summary>
        /// 履歴ログ出力（DB）
        /// </summary>
        /// <remarks>
        /// 履歴ログの出力を行います。
        /// </remarks>
        /// <param name="logKind">ログ種別</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="userLvl">出力ユーザレベル</param>
        /// <param name="contents">出力メッセージ</param>
        public virtual void Write(LogKind logKind, String userId, Int32 userLvl, List<String> contents)
        {
            // スレッドが終了している場合は開始
            //this.threadStart();

            LogInfo info = new LogInfo();

            info.Kind = logKind;
            info.Level = LogLevel.Debug;    // ←履歴ログでは無効なのでダミーを設定
            info.WriteDateTime = DateTime.Now;
            info.UserId = userId;
            info.UserLevel = userLvl.ToString();
            info.Contents = contents;

            this.LogThread.Enqueue(info);
        }

        /// <summary>
        /// 履歴ログ出力（DB）
        /// </summary>
        /// <remarks>
        /// 履歴ログの出力を行います。
        /// </remarks>
        /// <param name="logKind">ログ種別</param>
        /// <param name="userId">出力ユーザID</param>
        /// <param name="userLvl">出力ユーザレベル</param>
        /// <param name="contents">出力メッセージ（message1, message2, message3, . . .）</param>
        public virtual void Write(LogKind logKind, String userId, Int32 userLvl, params String[] contents)
        {
            List<String> cont = new List<String>();

            foreach (String msg in contents)
            {
                cont.Add(msg);
            }

            this.Write(logKind, userId, userLvl, cont);
        }

        #endregion

        /// <summary>
        /// ログインスタンス
        /// </summary>
        static private LogManager instance = null;
        /// <summary>
        /// ログインスタンス設定
        /// </summary>
        /// <remarks>
        /// WriteCommonLogを使用する前に、現在利用中のログクラスのインスタンスを設定します。
        /// </remarks>
        /// <param name="logManager"></param>
        public void SetInstance( )
        {
            instance = this;
        }

        /// <summary>
        /// ログ出力（共通用）
        /// </summary>
        /// <remarks>
        /// このクラスに設定されたログインスタンスを使用して出力処理を行います。
        /// このクラスを継承したクラスのインスタンスを利用して、別パッケージのネームスペースでログ出力処理を共通化する際使用します。
        /// （継承クラスのインスタンスがアクセス権を保持し、書き込みを行っているファイルを共有する為）
        /// </remarks>
        /// <param name="logKind">ログ種別</param>
        /// <param name="userId">出力ユーザID</param>
        /// <param name="contents">出力メッセージ（message1, message2, message3, . . .）</param>
        public virtual void WriteCommonLog( LogKind logKind, params String[] contents )
        {

            if ( instance != null )
            {
                instance.Write( logKind, String.Empty, contents.ToList() );
            }
            else
            {
                System.Diagnostics.Debug.WriteLine( "WriteCommonLogの無効な呼び出しが行われました。" );
            }

        }

        ///// <summary>
        ///// デバッグログ出力（Debug）
        ///// </summary>
        ///// <param name="message">出力メッセージ</param>
        //public virtual void Debug( String message )
        //{
        //    // スレッドが終了している場合は開始
        //    //this.threadStart();

        //    LogInfo info = new LogInfo();

        //    info.Kind = LogKind.DebugLog;
        //    info.Level = LogLevel.Debug;
        //    info.WriteDateTime = DateTime.Now;
        //    info.UserId = "";
        //    info.Contents = new List<String>() { message };

        //    this.LogThread.Enqueue( info );
        //}

        ///// <summary>
        ///// デバッグログ出力（Error）
        ///// </summary>
        ///// <param name="strMessage">出力メッセージ</param>
        //public virtual void Error( String message )
        //{
        //    // スレッドが終了している場合は開始
        //    //this.threadStart();

        //    LogInfo info = new LogInfo();

        //    info.Kind = LogKind.DebugLog;
        //    info.Level = LogLevel.Error;
        //    info.WriteDateTime = DateTime.Now;
        //    info.UserId = "";
        //    info.Contents = new List<String>() { message };

        //    this.LogThread.Enqueue( info );
        //}

        #endregion
    }
}
