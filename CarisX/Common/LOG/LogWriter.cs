//----------------------------------------------------------------
// Public Class.
//	  LogWriter
// Info.
//   ログクラス
// History
//   2011/09/01　　Ver1.00.00　　新規作成
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

using Oelco.Common.Utility;
using Oelco.Common.DB;
using Oelco.Common.Parameter;

namespace Oelco.Common.Log
{
    /// <summary>
    /// ログ出力クラス
    /// </summary>
    /// <remarks>
    /// ログ出力用のSQLキューを送信する
    /// デバッグログをファイルへ出力する
    /// </remarks>
    public class LogWriter
    {

        #region [ 定数 ]

        /// <summary>
        /// ログフォーマット
        /// </summary>
        private const String LOG_FORMAT = "{0}_{1}.log";

        #endregion

        #region [ インスタンス変数定義 ]

        /// <summary>
        /// ログ出力ディレクトリ
        /// </summary>
        private String m_logDir;

        /// <summary>
        /// ログ出力ディレクトリの取得、設定
        /// </summary>
        public String LogDir
        {
            get
            {
                return m_logDir;
            }
            set
            {
                m_logDir = value;
            }
        }
        /// <summary>
        /// ログファイル名プレフィックス
        /// </summary>
        private String m_logPrefix;

        /// <summary>
        /// ログファイル名プレフィックスの取得、設定
        /// </summary>
        public String LogPrefix
        {
            get
            {
                return m_logPrefix;
            }
            set
            {
                m_logPrefix = value;
            }
        }
        /// <summary>
        /// ログファイル名
        /// </summary>
        private String m_logName= string.Empty;

        /// <summary>
        /// ログファイル名の取得、設定
        /// </summary>
        public String LogName
        {
            get
            {
                return m_logName;
            }
            set
            {
                m_logName = value;
            }
        }
        /// <summary>
        /// ログファイルフルパス
        /// </summary>
        private String m_logPath = string.Empty;

        /// <summary>
        /// ログファイルフルパスの取得、設定
        /// </summary>
        public String LogPath
        {
            get
            {
                return m_logPath;
            }
            set
            {
                m_logPath = value;
            }
        }
        /// <summary>
        /// 現在のログ行数
        /// </summary>
        private Int32 m_logLines;

        /// <summary>
        /// 現在のログ行数の取得、設定
        /// </summary>
        public Int32 LogLines
        {
            get
            {
                return m_logLines;
            }
            set
            {
                m_logLines = value;
            }
        }
        /// <summary>
        /// １ファイルあたりの最大行数
        /// </summary>
        private Int32 m_maxLogLines = 10000;
        /// <summary>
        /// １ファイルあたりの最大行数の取得、設定
        /// </summary>
        public Int32 MaxLogLines
        {
            get
            {
                return m_maxLogLines;
            }
            set
            {
                m_maxLogLines = value;
            }
        }
        /// <summary>
        /// ログローテーションファイル数
        /// </summary>
        private Int32 m_maxLogLotateSeq = 10;

        /// <summary>
        /// ログローテーションファイル数の取得、設定
        /// </summary>
        public Int32 MaxLogLotateSeq
        {
            get
            {
                return m_maxLogLotateSeq;
            }
            set
            {
                m_maxLogLotateSeq = value;
            }
        }

        /// <summary>
        /// デバッグログ出力
        /// </summary>
        private Boolean m_debug = true;
        /// <summary>
        /// デバッグログ出力の取得、設定
        /// </summary>
        public Boolean Debug
        {
            get
            {
                return m_debug;
            }
            set
            {
                m_debug = value;
            }
        }

        /// <summary>
        /// ログファイル保持日数
        /// </summary>
        private TimeSpan  m_keepSpan = new TimeSpan(30,0,0,0);

        /// <summary>
        /// ログファイル保持日数の取得、設定
        /// </summary>
        public TimeSpan KeepSpan
        {
            get
            {
                return m_keepSpan;
            }
            set
            {
                m_keepSpan = value;
            }
        }

        /// <summary>
        /// StreamWriterオブジェクト
        /// </summary>
        private StreamWriter m_streamWriter;
        /// <summary>
        /// ファイルNo
        /// </summary>
        private Int32 fileNo = 0;

        //Int32 m_lineCnt = 0;
        #endregion

        #region [ コンストラクタ ]

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public LogWriter()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="strLogDir">ログ出力先</param>
        /// <param name="strLogPrefix">出力形式は{指定したプレフィックス名}_{連番}.log</param>
        /// <param name="intMaxLogLines">ログ最大行数</param>
        /// <param name="intLotate">ログローテーション数</param>
        /// <param name="blDebug">True:デバッグログ出力あり　False:デバッグログ出力なし</param>
        public LogWriter( String strLogDir,
                    String strLogPrefix,
                    Int32 intMaxLogLines,
                    Int32 intLotate,
                    Boolean blDebug
                    )
        {
            this.m_logDir = strLogDir;
            this.m_logPrefix = strLogPrefix;
            this.m_maxLogLines = intMaxLogLines;
            this.m_maxLogLotateSeq = intLotate;
            this.m_debug = blDebug;
        }

        #endregion

        #region [ デストラクタ ]

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~LogWriter()
        {
        }

        #endregion

        #region [ プロパティ ]

        /// <summary>
        /// ログファイル名生成
        /// </summary>
        /// <remarks>
        /// システム日付より生成したログファイル名を返します。　
        /// </remarks>
        protected virtual String LogFileNameFormat
        {
            get
            {
                return DateTime.Now.ToString( "yyyyMMdd_" ) + LOG_FORMAT;
            }
        }
        #endregion

        #region [ メソッド ]

        /// <summary>
        /// 使用ログファイル名取得
        /// </summary>
        /// <remarks>
        /// 使用ログファイル名を返します。
        /// </remarks>
        /// <returns></returns>
         private  String GetUseLogFileName()
        {
             String fileName = String.Empty;
            
            // 出力されているログファイルのチェック
            String[] files = Directory.GetFiles( this.m_logDir,
                String.Format( this.LogFileNameFormat, this.m_logPrefix, "*" ) );

            Boolean find = false;
            
            // 本日日付のログファイルの行数を確認する
            for ( Int32 i = 1; i <= files.Length; i++ )
            {
                string name = this.m_logDir + "\\" + String.Format(this.LogFileNameFormat, this.m_logPrefix, i.ToString("000"));
                if ( File.Exists( name ) )
                {
                    //如果没有找到相对应的文件，继续
                    if (fileNo != 0 && fileNo != i)
                    {
                        continue;
                    }
                    //不是第一次记录日志,不用遍历文件
                    if (this.LogLines > 0 && this.LogLines < m_maxLogLines)
                    {
                        fileName = String.Format(name);
                        find = true;
                        break;
                    }
                    // 行数が上限以内のファイルがあれば、そのファイル名を返す
                    FileStream fs = File.Open(name, FileMode.Open, FileAccess.Read );
                    StreamReader sr = new StreamReader( fs );
                    Int32 lineCnt = 0;
                    while ( sr.ReadLine() != null )
                    {
                        lineCnt++;
                    }
                    sr.Close();
                    sr.Dispose();

                    fs.Close();
                    fs.Dispose();
                    if ( lineCnt < m_maxLogLines )
                    {
                        fileName = String.Format( name );
                        this.LogLines = lineCnt;
                        fileNo = i;
                        find = true;
                        break;
                    }
                }
                else
                {
                    // 本日日付ログファイル数以内で、使用されていない番号があれば、その番号のファイル名を返す
                    fileName = String.Format(this.LogFileNameFormat, this.m_logPrefix, i.ToString("000"));
                    this.LogLines = 0;
                    fileNo = i;
                    find = true;
                    break;
                }
            }


            if ( !find )
            {
                // ファイル名未確定且つ、ファイル数が上限以内の場合は、現ファイル数をインクリメントしたファイル名を返す
                if ( files.Length < m_maxLogLotateSeq )
                {
                    fileName = String.Format(this.LogFileNameFormat, this.m_logPrefix, (files.Length + 1).ToString("000"));
                    fileNo = files.Length + 1;
                }
                else
                {
                    // ファイル名未確定且つ、ファイル数が上限の場合は連番１のファイル名を返す
                    fileName = String.Format(this.LogFileNameFormat, this.m_logPrefix, 1.ToString("000"));
                    fileNo = 1;
                }
                this.LogLines = 0;
            }

            return fileName; 
        }

        /// <summary>
        /// ログファイルオープン
        /// </summary>
        /// <remarks>
        /// ログファイルをオープンします。
        /// </remarks>
        protected virtual Boolean openFile()
        {
            // 出力ディレクトリが指定されてい or 存在しなければデフォルトのパスにする
            if ( String.IsNullOrEmpty( this.m_logDir ) || !Directory.Exists( this.m_logDir ) )
            {
                this.m_logDir = Path.GetDirectoryName( Environment.GetCommandLineArgs()[0] );
            }

            try
            {
               
                // パス生成
                //this.m_logName = GetUseLogFileName();
                if (this.m_logPath == string.Empty)
                {
                    if (this.m_logName == string.Empty)
                    {
                        this.m_logName = GetUseLogFileName();
                    }
                    this.m_logPath = Path.Combine(this.m_logDir, this.m_logName);
                }
                                             

                // 書き込み用に開く
                this.m_streamWriter = new StreamWriter( this.m_logPath, true );
            }
            catch ( Exception ex )
            {
                // TODO:無視？とりあえずイベントログに出しておきます
                EventLog.WriteEntry( System.Reflection.Assembly.GetExecutingAssembly().ToString(),
                    ex.ToString(), EventLogEntryType.Warning );
                return false;
            }

            return true;

        }

        /// <summary>
        /// ログファイルのローテーション
        /// </summary>
        /// <remarks>
        /// ログファイル数が上限に達していれば、連番1のファイルを作成し、
        /// 上限以内であれば現在の連番+1のファイルを作成します。
        /// </remarks>
        /// <returns></returns>
        protected virtual Boolean lotate()
        {
            try
            {
                // 現在のファイルクローズ
                this.m_streamWriter.Close();
                this.m_streamWriter.Dispose();
               // this.m_streamWriter = null;

                if ( fileNo >= this.m_maxLogLotateSeq )
                {
                    // ファイル数が上限に達していたら、連番1のファイル
                    fileNo = 1;
                }
                else
                {
                    // ファイル数が上限以内であれば連番Max + 1のファイル                    
                    fileNo++; 
                }

                // 新しい出力先を取得
                this.m_logName = String.Format( this.LogFileNameFormat, this.m_logPrefix, fileNo.ToString("000") );
                this.m_logPath = Path.Combine( this.m_logDir, this.m_logName );

                // 対象ファイルが存在していたら一旦削除する
                if ( File.Exists( m_logPath ) )
                {
                    File.Delete( m_logPath );
                }

                // 行数カウンタリセット
                this.m_logLines = 0;

                // ストリームライターで開く
                this.m_streamWriter = new StreamWriter( this.m_logPath, true );
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
        /// 古いログファイルの削除
        /// </summary>
        /// <remarks>
        /// 古いログファイルを削除します。
        /// </remarks>
        public void DeleteOldLog()
        {
            // 出力ディレクトリが指定されてい or 存在しなければデフォルトのパスにする
            if ( String.IsNullOrEmpty( this.m_logDir ) || !Directory.Exists( this.m_logDir ) )
            {
                this.m_logDir = Path.GetDirectoryName( Environment.GetCommandLineArgs()[0] );
            }

            // ログファイル保持日数より過去のファイルを取得
            var logFiles = Directory.GetFiles( this.m_logDir, String.Format( LOG_FORMAT, "*" + this.m_logPrefix, "*" ) ).ToList();                         

            // ログファイルを削除
            foreach ( var fileName in logFiles )
            {                
                Int32 intDate;
                DateTime dtDelDate;
                if ( Int32.TryParse( Path.GetFileName( fileName ).Substring( 0, 8 ), out intDate ) && 
                    DateTime.TryParse(intDate.ToString("0000/00/00"),out dtDelDate ))
                {
                    // 日付に変換できる数値を含むファイル名のみ削除する
                    if ( File.Exists( fileName ) && dtDelDate <= DateTime.Now - KeepSpan)
                    {
                        File.Delete( fileName );
                    }

                }                
            }          
        }

        /// <summary>
        /// デバッグログ出力
        /// </summary>
        /// <remarks>
        /// デバッグログを出力します。
        /// </remarks>
        /// <param name="info"></param>
        public virtual void WriteFile( LogInfo info )
        {
            //Console.WriteLine( "ログファイルへの書き込みを行います。" );

            // ファイル出力設定が無効となっている場合は処理せず終了
            if ( !this.m_debug )
            {
                return;
            }

            // ファイルがオープンされていなかったら開く
            if ( this.m_streamWriter == null )
            {
                this.openFile();
            }

            // 出力内容の生成
            StringBuilder strRec = new StringBuilder();
            strRec.AppendFormat("{0}", info.WriteDateTime.ToString("yyyy/MM/dd HH:mm:ss.fff") );
            
            //Figu：修改于2018/5/3
            if (info.Kind == LogKind.OnlineHist)
            {
                strRec.AppendFormat(",{0}", "online");//Figu:online为什么会是大写???
            }
            else
            {
                strRec.AppendFormat( ",{0}", info.Level.ToString() );
            }

            foreach ( String cont in info.Contents )
            {
                strRec.Append( String.Format( @",{0}", cont ) );
            }

            try
            {
                StringReader sr = new StringReader( strRec.ToString() );
                String line;
                while ( ( line = sr.ReadLine() ) != null )
                {
                    // 書き込み
                    this.m_streamWriter.WriteLine( line );
                    this.m_streamWriter.Flush();
                    this.m_logLines++;

                    // ファイルの行数が上限に達していたら次のファイルへ
                    if ( this.m_logLines >= this.m_maxLogLines )
                    {
                        if ( !this.lotate() )
                        {
                            // TODO:とりあえずイベントログに出しておきます
                            EventLog.WriteEntry( System.Reflection.Assembly.GetExecutingAssembly().ToString(),
                                "ログのローテーションに失敗しました。", EventLogEntryType.Warning );
                            sr.Close();
                            sr.Dispose();
                            return;
                        }
                    }
                }
                sr.Close();
                sr.Dispose(); 
            }
            catch ( Exception ex )
            {
                // TODO:とりあえずイベントログに出しておきます
                EventLog.WriteEntry( System.Reflection.Assembly.GetExecutingAssembly().ToString(),
                    ex.ToString(), EventLogEntryType.Warning );
            }
            finally
            {
                //現在のファイルクローズ
                this.m_streamWriter.Close();
                this.m_streamWriter.Dispose();
                this.m_streamWriter = null;
            }
        }

        /// <summary>
        /// ログ出力
        /// </summary>
        /// <remarks>
        /// ログ出力を行います。
        /// </remarks>
        /// <param name="info"></param>
        public virtual void WriteDB( LogInfo info )
        {
            // HACK : 古いレコードを削除する機能が必要か？
            // HACK : プロジェクトのDBに合わせてSQLは修正すること

            String strTableName;

            switch (info.Kind)
	        {
            case LogKind.ErrorHist:
                strTableName = "errorLog";
                break;
            case LogKind.OperationHist:
                strTableName = "operationLog";
                break;
            // Onlineログは通信Libが実装
            //case LogKind.OnlineHist:
            //    strTableName = "onlineLog";
            //    break;
            case LogKind.AnalyseHist:
                strTableName = "analizeLog";
                break;
            case LogKind.ParamChangeHist:
                strTableName = "paramChangeLog";
                break;
            case LogKind.MaintenanceLogin:
                strTableName = "maintenanceLoginLog";
                break;
            case LogKind.MasterErrorHist:
                strTableName = "masterErrorLog";
                break;
            default:
                return;
	        }

            // SQL作成
            StringBuilder strSql = new StringBuilder();

            strSql.Append( " INSERT INTO " );
            strSql.Append( strTableName );
            strSql.Append( " (writeTime, userID" );

            #region [ AFTのみ操作履歴にユーザーレベルを追加 ]
            if ((info.Kind == LogKind.OperationHist) && (GlobalParameter.myApplicationKind == GlobalParameter.ApplicationKind.NS_Prime))
            {
                strSql.Append(", userLevel");
            }
            #endregion

            for (Int32 i = 0; i < info.Contents.Count; i++)
            {
                strSql.Append( ", contents" );
                strSql.Append( (i+1).ToString() );
            }
            strSql.Append( ") VALUES ( " );
            strSql.AppendFormat( @"'{0}'", info.WriteDateTime.ToString( "yyyy/MM/dd HH:mm:ss.fff" ) );
            strSql.AppendFormat( @",'{0}'", info.UserId );

            #region [ AFTのみ操作履歴にユーザーレベルを追加 ]
            if ((info.Kind == LogKind.OperationHist) && (GlobalParameter.myApplicationKind == GlobalParameter.ApplicationKind.NS_Prime))
            {
                strSql.AppendFormat(@",'{0}'", info.UserLevel);
            }
            #endregion

            foreach ( String val in info.Contents )
            {
                strSql.AppendFormat( @",'{0}'", val );
            }
            strSql.Append( ")" );

            // 実行結果はログクラスでは取得しない
            Singleton<SQLServerDBAccess>.Instance.Open(); // TODO : DBに書きこめない為、暫定処置記述　DB関連クラスの作りがLogクラス作成時と異なるため、最新に合わせた修正が必要か？
            Singleton<SQLServerDBAccess>.Instance.ExecuteSql( strSql.ToString(), "");
        }

        #endregion
    }
}
