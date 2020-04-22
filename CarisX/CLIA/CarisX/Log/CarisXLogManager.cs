using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Log;
using Oelco.Common.Utility;

using Oelco.CarisX.Comm;
using Oelco.CarisX.Const;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace Oelco.CarisX.Log
{
    /// <summary>
    /// 保存在线日志
    /// </summary>
    public class CarisXOnlineLogWriter : ThreadBase
    {
       
        private string dateFlagForOnlineLog = string.Empty;//Figu：日期标记
        private List<string> lastOnlineLog = new List<string>();//Figu：保存上一次的所有记录
        private const int MAX_LINE = 1000;
        private CarisXLogManager carisXLogManager = null;//Figu：注意不能使用单例模式
        private Mutex writeMutex = new Mutex();
        private bool isExit = false;
        private string fullOnlinePath = string.Empty;
        private UInt64 flag = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CarisXOnlineLogWriter()
        {
            this.fullOnlinePath = CarisXConst.PathOnline;
            if (!Directory.Exists(fullOnlinePath))
            {
                //Figu:no exist,create it
                try
                {
                    Directory.CreateDirectory(fullOnlinePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invoked CreateDirectory failed\r\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 回收函数
        /// </summary>
        public void Dispose()
        {
            writeMutex.WaitOne();
            isExit = true;
            //↓↓↓アプリケーション終了時の例外発生対応 2019/2/19↓↓↓
            // threadFunction関数でisExitを処理させるためにミューテックスを
            // 一旦開放する
            writeMutex.ReleaseMutex();
            //↑↑↑アプリケーション終了時の例外発生対応 2019/2/19↑↑↑
            if (!base.EndJoin())
            {
                MessageBox.Show("Invoked EndJoin failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //↓↓↓アプリケーション終了時の例外発生対応 2019/2/19↓↓↓
            // スレッドが終了しているはずなので、carisXLogManagerは安全にアクセス
            // できるはずだが、当初の実装と意味を同じくするためにミューテックスでロックする
            writeMutex.WaitOne();
            //↑↑↑アプリケーション終了時の例外発生対応 2019/2/19↑↑↑
            //Figu:release
            if (carisXLogManager != null)
            {
                carisXLogManager.Dispose();
                carisXLogManager = null;
            }
            writeMutex.ReleaseMutex();
        }

        /// <summary>
        /// 线程入口函数
        /// </summary>
        protected override void threadFunction()
        {
            Thread.Sleep(15000);

            while (true)
            {
                writeMutex.WaitOne();
                if (isExit)
                {
                    writeMutex.ReleaseMutex();
                    break;
                }

                try
                {
                    savingOnlineLog();
                    Thread.Sleep(15000);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invoked savingOnlineLog failed\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    carisXLogManager.Write(LogKind.OnlineHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, ex.Message);
                    isExit = true;
                }
                finally
                {
                    writeMutex.ReleaseMutex();
                }
               
            }
        }

        /// <summary>
        /// 保存在线日志
        /// </summary>
        private void savingOnlineLog()
        {
            bool isEx = false;
            try
            {
                if (string.IsNullOrEmpty(dateFlagForOnlineLog))
                {
                    dateFlagForOnlineLog = DateTime.Now.ToString("yyyy-MM-dd");

                    carisXLogManager = new CarisXLogManager();
                    carisXLogManager.Initialize(fullOnlinePath, "online", 50000, 10, 100, true, new TimeSpan(30, 0, 0, 0));//Figu:新建文件夹用于保存在线日志
                }
                else if (dateFlagForOnlineLog != DateTime.Now.ToString("yyyy-MM-dd") && carisXLogManager != null)
                {
                    dateFlagForOnlineLog = DateTime.Now.ToString("yyyy-MM-dd");

                    carisXLogManager.Dispose();
                    carisXLogManager = null;

                    carisXLogManager = new CarisXLogManager();
                    carisXLogManager.Initialize(fullOnlinePath, "online", 50000, 10, 100, true, new TimeSpan(30, 0, 0, 0));
                }

                writingOnlineLog();
            }
            catch (Exception ex)
            {
                isEx = true;
                //Debug.WriteLine(String.Format("Invoked saveOnlineLog failed:{0}", ex.Message));              
                throw ex;
            }
            finally
            {
                if (isEx)
                {
                    carisXLogManager.Dispose();
                    carisXLogManager = null;
                }

            }
        }

        /// <summary>
        /// 写记录
        /// </summary>
        private void writingOnlineLog()
        {
            if (null == carisXLogManager)
            {
                return;
            }

            try
            {
                
                List<string> wholeOnlineLog = new List<string>();
                //Figu:OnlineLog,mutil thread will operate it,the application will crash if access it directly
                wholeOnlineLog.AddRange(Singleton<CarisXCommManager>.Instance.SycOnlineLog.Where(log => !string.IsNullOrWhiteSpace(log)).ToList());

                if (wholeOnlineLog.Count <= 3)
                {
                    return;
                }

                bool isNewLogComing = false;
                int newRows = int.Parse(wholeOnlineLog[wholeOnlineLog.Count - 1]);
                wholeOnlineLog.RemoveAt(wholeOnlineLog.Count - 1);


                if (lastOnlineLog.Count > 0)
                {
                    if (lastOnlineLog.Count == MAX_LINE)
                    {
                        if (newRows > MAX_LINE) //Figu：理论上是存在大于MAX LINE的可能性的
                        {
                            //Figu:会遗漏了部分记录，但在我们的项目中不会发生，如果要彻底解决此问题，需要做比较大的改动
                            for (int i = 0; i < wholeOnlineLog.Count - 2; i++)
                            {
                                isNewLogComing = true;
                                flag = 0;
                                carisXLogManager.Write(LogKind.OnlineHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, wholeOnlineLog[i]);
                            }
                        }
                        else//Figu:eq and less than
                        {
                            for (int i = wholeOnlineLog.Count - newRows; i < wholeOnlineLog.Count - 2; i++)
                            {
                                isNewLogComing = true;
                                flag = 0;
                                carisXLogManager.Write(LogKind.OnlineHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, wholeOnlineLog[i]);
                            }
                        }
                    }
                    else if (lastOnlineLog.Count < MAX_LINE)
                    {
                        if (wholeOnlineLog.Count < MAX_LINE)//Figu:最后一次和这次的记录都小于MAX LINE
                        {
                            for (int i = lastOnlineLog.Count; i < wholeOnlineLog.Count - 2; i++)
                            {
                                isNewLogComing = true;
                                flag = 0;
                                carisXLogManager.Write(LogKind.OnlineHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, wholeOnlineLog[i]);
                            }
                        }
                        else if (wholeOnlineLog.Count == MAX_LINE)//Figu:最后一次小于MAX LINE，这次等于MAX LINE
                        {
                            int totalRows = newRows + (MAX_LINE - lastOnlineLog.Count);
                            if (totalRows <= MAX_LINE)
                            {
                                for (int i = wholeOnlineLog.Count - totalRows; i < wholeOnlineLog.Count - 2; i++)
                                {
                                    isNewLogComing = true;
                                    flag = 0;
                                    carisXLogManager.Write(LogKind.OnlineHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, wholeOnlineLog[i]);
                                }
                            }
                            else//Figu：理论上是存在大于MAX LINE的可能性的
                            {
                                //Figu:会遗漏了部分记录，但在我们的项目中不会发生，如果要彻底解决此问题，需要做比较大的改动
                                for (int i = 0; i < wholeOnlineLog.Count - 2; i++)
                                {
                                    isNewLogComing = true;
                                    flag = 0;
                                    carisXLogManager.Write(LogKind.OnlineHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, wholeOnlineLog[i]);
                                }
                            }
                        }
                    }

                    lastOnlineLog.Clear();
                    lastOnlineLog.AddRange(wholeOnlineLog);
                    lastOnlineLog.RemoveAt(lastOnlineLog.Count - 1);
                    lastOnlineLog.RemoveAt(lastOnlineLog.Count - 1);
 
                }
                else
                {
                    lastOnlineLog.Clear();
                    lastOnlineLog.AddRange(wholeOnlineLog);

                    //Figu:删除最后两条记录，因为最后两条记录会出现不完整现象
                    lastOnlineLog.RemoveAt(lastOnlineLog.Count - 1);
                    lastOnlineLog.RemoveAt(lastOnlineLog.Count - 1);

                    foreach (var temp in lastOnlineLog)
                    {
                        //Figu:保存每条在线日志
                        isNewLogComing = true;
                        flag = 0;
                        carisXLogManager.Write(LogKind.OnlineHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, temp);
                    }
                }


                if (!isNewLogComing)
                {
                    flag++;
                    if (1 == flag)
                    {
                        carisXLogManager.Write(LogKind.OnlineHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, wholeOnlineLog[wholeOnlineLog.Count - 2]);
                        carisXLogManager.Write(LogKind.OnlineHist, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, wholeOnlineLog[wholeOnlineLog.Count - 1]);
                        lastOnlineLog.Add(wholeOnlineLog[wholeOnlineLog.Count - 2]);
                        lastOnlineLog.Add(wholeOnlineLog[wholeOnlineLog.Count - 1]);
                    }
                }
                        
                wholeOnlineLog.Clear();
                wholeOnlineLog = null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    /// <summary>
    /// 履歴管理クラス
    /// </summary>
    public class CarisXLogManager : LogManager
    {
        #region [ インスタンス変数定義 ]

        /// <summary>
        /// ログ出力キュー監視スレッド
        /// </summary>
        private CarisXLogThread m_logThread = new CarisXLogThread();

        /// <summary>
        /// ログ出力キュー監視スレッドの取得、設定
        /// </summary>
        protected CarisXLogThread CarisXLogThread
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

        #region [ メソッド ]

        /// <summary>
        /// ログ出力キュー監視スレッド開始
        /// </summary>
        /// <remarks>
        /// ログ出力キュー監視スレッド開始します
        /// </remarks>
        /// <returns></returns>
        protected override Boolean threadStart()
        {
            Boolean bRet = true;

            if ( this.CarisXLogThread.IsAlive == false )
            {
                bRet = this.CarisXLogThread.Start();
            }

            return bRet;
        }

        /// <summary>
        /// 初期化
        /// アプリケーション開始時に必ず呼び出すこと
        /// </summary>
        /// <remarks>
        /// ログ出力キュー監視スレッドを初期化して開始します
        /// </remarks>
        /// <param name="strLogDir">ログ出力先</param>
        /// <param name="strLogPrefix">出力形式は{指定したプレフィックス名}_{連番}.log</param>
        /// <param name="intMaxLogLines">ログ最大行数</param>
        /// <param name="intLotate">ログローテーション数</param>
        /// <param name="interval">キュー処理待機時間</param>
        /// <param name="blDebug">True:デバッグログ出力あり　False:デバッグログ出力なし</param>
        /// <param name="intKeepSpan">ログ保持期限</param>
        /// <remarks></remarks>
        public override Boolean Initialize( String strLogDir,
                    String strLogPrefix,
                    Int32 intMaxLogLines,
                    Int32 intLotate,
                    Int32 interval,
                    Boolean blDebug,
                    TimeSpan keepSpan )
        {
            Boolean bRet;

            this.CarisXLogThread.Initialize( strLogDir,
                                    strLogPrefix,
                                    intMaxLogLines,
                                    intLotate,
                                    interval,
                                    blDebug,
                                    keepSpan );

            // ログ出力キュー監視スレッド開始
            bRet = this.threadStart();

            return bRet;
        }

        /// <summary>
        /// 解放
        /// </summary>
        /// <remarks>
        /// ログ出力キュー監視スレッド停止します
        /// </remarks>
        public override void Dispose()
        {
            // スレッドの停止(失敗しても無視)
            //this.m_logThread.End();
            this.CarisXLogThread.EndJoin();
        }

        /// <summary>
        /// 履歴ログ出力（DB）
        /// </summary>
        /// <remarks>
        /// 履歴ログ出力（DB）します
        /// </remarks>
        /// <param name="logKind">ログ種別</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="contents">出力メッセージ</param>
        public override void Write( LogKind logKind, String userId, List<String> contents )
        {
            this.Write( logKind, userId, CarisXLogInfoBaseExtention.Empty, contents );
            // スレッドが終了している場合は開始
            //this.threadStart();

            //LogInfo info = new LogInfo();

            //info.Kind = logKind;
            //info.Level = LogLevel.Debug;    // ←履歴ログでは無効なのでダミーを設定
            //info.WriteDateTime = DateTime.Now;
            //info.UserId = userId;
            //info.Contents = contents;

            //this.LogThread.Enqueue( info );
        }

        /// <summary>
        /// 履歴ログ出力（DB）
        /// </summary>
        /// <remarks>
        /// 履歴ログ出力（DB）します
        /// </remarks>
        /// <param name="logKind">ログ種別</param>
        /// <param name="userId">出力ユーザID</param>
        /// <param name="optionalValue">追加情報</param>
        /// <param name="contents">出力メッセージ</param>
        public void Write( LogKind logKind, String userId, CarisXLogInfoBaseExtention optionalValue, List<String> contents, Int32 moduleNo = (Int32)RackModuleIndex.Module1 )
        {
            CarisXLogInfo info = new CarisXLogInfo();

            info.Kind = logKind;
            info.Level = LogLevel.Debug;    // ←履歴ログでは無効なのでダミーを設定
            info.WriteDateTime = DateTime.Now;
            info.UserId = userId;
            info.ModuleNo = moduleNo;
            info.Contents = contents;
            info.OptionalValue = optionalValue;
            this.CarisXLogThread.Enqueue( info );
        }

        /// <summary>
        /// 履歴ログ出力（DB）
        /// </summary>
        /// <remarks>
        /// 履歴ログ出力（DB）します
        /// </remarks>
        /// <param name="logKind">ログ種別</param>
        /// <param name="userId">出力ユーザID</param>
        /// <param name="contents">出力メッセージ（message1, message2, message3, . . .）</param>
        public override void Write( LogKind logKind, String userId, params String[] contents )
        {
            this.Write( logKind, userId, CarisXLogInfoBaseExtention.Empty, contents );
            //List<String> cont = new List<String>();

            //foreach ( String msg in contents )
            //{
            //    cont.Add( msg );
            //}

            //this.Write( logKind, userId, cont );
        }

        /// <summary>
        /// 履歴ログ出力（DB）
        /// </summary>
        /// <remarks>
        /// 履歴ログ出力（DB）します
        /// </remarks>
        /// <param name="logKind">ログ種別</param>
        /// <param name="userId">出力ユーザID</param>
        /// <param name="optionalValue">追加情報</param>
        /// <param name="contents">出力メッセージ（message1, message2, message3, . . .）</param>
        public void Write(LogKind logKind, String userId, CarisXLogInfoBaseExtention optionalValue, params String[] contents )
        {
            this.Write( logKind, userId, optionalValue, contents.ToList() );
        }

        /// <summary>
        /// 履歴ログ出力（DB）
        /// </summary>
        /// <remarks>
        /// 履歴ログ出力（DB）します
        /// </remarks>
        /// <param name="logKind">ログ種別</param>
        /// <param name="userId">出力ユーザID</param>
        /// <param name="moduleNo">出力ユーザID</param>
        /// <param name="optionalValue">追加情報</param>
        /// <param name="contents">出力メッセージ（message1, message2, message3, . . .）</param>
        public void Write(LogKind logKind, String userId, Int32 moduleNo, CarisXLogInfoBaseExtention optionalValue, params String[] contents)
        {
            this.Write(logKind, userId, optionalValue, contents.ToList(), moduleNo);
        }

        ///// <summary>
        ///// デバッグログ出力（Debug）
        ///// </summary>
        ///// <param name="message">出力メッセージ</param>
        //public override void Debug( String message )
        //{
        //    // スレッドが終了している場合は開始
        //    //this.threadStart();

        //    CarisXLogInfo info = new CarisXLogInfo();

        //    info.Kind = LogKind.DebugLog;
        //    info.Level = LogLevel.Debug;
        //    info.WriteDateTime = DateTime.Now;
        //    info.UserId = String.Empty;
        //    info.Contents = new List<String>() { message };
        //    info.OptionalValue = null;

        //    this.CarisXLogThread.Enqueue( info );
        //}

        ///// <summary>
        ///// デバッグログ出力（Error）
        ///// </summary>
        ///// <param name="strMessage">出力メッセージ</param>
        //public override void Error( String message )
        //{
        //    // スレッドが終了している場合は開始
        //    //this.threadStart();

        //    CarisXLogInfo info = new CarisXLogInfo();

        //    info.Kind = LogKind.DebugLog;
        //    info.Level = LogLevel.Error;
        //    info.WriteDateTime = DateTime.Now;
        //    info.UserId = String.Empty;
        //    info.Contents = new List<String>() { message };
        //    info.OptionalValue = null;

        //    this.CarisXLogThread.Enqueue( info );
        //}

        #endregion
    }
}
