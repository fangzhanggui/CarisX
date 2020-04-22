//----------------------------------------------------------------
// Public Class.
//	  SQLServerDBAccess
// Info.
//   SQLServerDBアクセスクラス
// History
//   2011/09/01　　Ver1.00.00　　新規作成
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;

using Oelco.Common.Log;
using Oelco.Common.Utility;
using System.Text.RegularExpressions;

namespace Oelco.Common.DB
{

    // TODO:コメント不十分

    #region [ キュー処理完了イベント定義 ]

    /// <summary>
    /// キュー処理完了イベント用デリゲート
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    public delegate void CompleteDBQueueHandler( object source, DBQueueEventArgs e );

    /// <summary>
    /// データベース処理結果イベントデータ
    /// </summary>
    public class DBQueueEventArgs : EventArgs
    {
        /// <summary>
        /// 発行元識別用のKey
        /// </summary>
        public String Key
        {
            get;
            set;
        }

        /// <summary>
        /// SQL実行結果データ
        /// </summary>
        /// <remarks>実行結果データの場合はDataTable　/　実行結果件数の場合はInt32</remarks>
        public object Result
        {
            get;
            set;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="strKey">キュー識別キー</param>
        /// <param name="objResult">実行結果</param>
        public DBQueueEventArgs( String strKey, object objResult )
        {
            this.Key = strKey;
            this.Result = objResult;
        }
    }

    #endregion

    /// <summary>
    /// SQLServerDBアクセスクラス
    /// </summary>
    /// <remarks>
    /// コネクションの開閉はこのクラスで行うこと
    /// </remarks>
    public class SQLServerDBAccess : ParameterizedThreadBase
    {
        #region [イベント定義]

        /// <summary>
        /// キュー処理完了通知イベント
        /// </summary>
        static public event CompleteDBQueueHandler OnCompleteDBQueue;

        #endregion

        #region [定数定義]

        // TODO : 接続文字列
        //private const String DB_CONNECT_FORMAT = "Server={0}; Trusted_Connection=yes; Connection Timeout={1}; AttachDbFilename={2};database={3}";
        /// <summary>
        /// 接続文字列
        /// </summary>
        private const String DB_CONNECT_FORMAT = "Data Source ={0}; Trusted_Connection=yes; Connection Timeout={1}; Initial Catalog ={2}; Integrated Security = SSPI";

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 接続文字列
        /// </summary>
        static private String m_connectionString = "";
        /// <summary>
        /// SQLDB接続
        /// </summary>
        static private SqlConnection dbSqlConnection = null;
        
        /// <summary>
        /// SQLコマンド
        /// </summary>
        private SqlCommand m_sqlCommand = null;
#if false// false:トランザクション無効
        /// <summary>
        /// ＠＠＠
        /// </summary>
        private SqlTransaction m_sqltran = null;

        /// <summary>
        /// ＠＠＠
        /// </summary>
        private Boolean m_istran = false;
#endif
        /// <summary>
        /// SQLキュー(排他)
        /// </summary>
        private LockObject<Queue<SqlInfo>> m_qSql = new LockObject<Queue<SqlInfo>>();

        //static private Thread baseThread = null;
        /// <summary>
        /// ベーススレッド終了シグナルイベント
        /// </summary>
        static private ManualResetEvent baseThreadEnd = new ManualResetEvent(false);

        /// <summary>
        /// ベーススレッド
        /// </summary>
        static protected Thread baseThread = null;

        #endregion
        
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SQLServerDBAccess()
        {
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// スレッド処理の取得
        /// </summary>
        protected override ParameterizedThreadBase.ParameterizedThread ThreadFunction
        {
            get
            {
                return baseThreadFunction;
            }
        }

        /// <summary>
        /// スレッドパラメータの取得
        /// </summary>
        protected override Object ThreadParameter
        {
            get
            {
                return null;
            }
        }
        
        /// <summary>
        /// SQLDB接続の取得、設定
        /// </summary>
        protected SqlConnection DbConnection
        {
            get
            {
                return dbSqlConnection;
            }
            set
            {
                dbSqlConnection = value;
            }
        }

        /// <summary>
        /// トランザクションの取得、設定
        /// </summary>
        protected SqlTransaction sqlTransaction
        {
            get
            {
                return this.m_sqlCommand.Transaction;
            }
            set
            {
                this.m_sqlCommand.Transaction = value;
            }
        }

        #endregion
        
        #region [publicメソッド]

        /// <summary>
        /// 開始処理
        /// </summary>
        /// <remarks>
        /// ベースのスレッドを動作させ、動作対象を登録します。
        /// </remarks>
        /// <returns></returns>
        public override Boolean Start()
        {
            Boolean result = true;

            // ベースのスレッドを動作させる
            if ( baseThread == null )
            {
                //this.connect();
                baseThreadEnd.Reset();
                result = base.Start();
                baseThread = this.Thread;
            }

            // 動作対象を登録する。
            classList.Lock();
            if ( classList.Get.Instance.Contains( this ) )
            {
                result = false;
            }
            else
            {
                classList.Get.Instance.Add( this );
            }
            classList.UnLock();

            return result;
        }

        /// <summary>
        /// スレッド停止
        /// </summary>
        /// <remarks>
        /// ベーススレッドを終了させます。
        /// </remarks>
        /// <returns></returns>
        public override Boolean EndJoin()
        {
            Boolean blnEnded = true;
            // 動作対象を解除する。
            classList.Lock();
            if ( classList.Get.Instance.Contains( this ) )
            {
                classList.Get.Instance.Remove( this );
            }
            queuePushNotify.Set();

            // 動作対象が0になればベーススレッドを終了
            if ( classList.Get.Instance.Count == 0 )
            {
                baseThreadEnd.Set();
                blnEnded = base.EndJoin();
                //this.disConnect();
            }
            classList.UnLock();

            return blnEnded;
        }

        /// <summary>
        /// サーバ名取得
        /// </summary>
        /// <remarks>
        /// サーバ名を返します。
        /// </remarks>
        /// <param name="server"></param>
        /// <returns></returns>
        static public Tuple<String,String> GetServerName( String server )//String server )
        {


            Tuple<String, String> appendString = new Tuple<String, String>(String.Empty,String.Empty);

            try
            {
                //var searchedServerInfo = ( from v in Microsoft.SqlServer.Management.Smo.SmoApplication.EnumAvailableSqlServers( false ).AsEnumerable()
                //                           where v.Field<String>( "server" ) == server || v.Field<String>( "server" ) == Environment.MachineName
                //                           group v.Field<String>( "name" ) by v.Field<String>( "server" ) );

                var searchedServerInfo = ( from v in Microsoft.SqlServer.Management.Smo.SmoApplication.EnumAvailableSqlServers( false ).AsEnumerable()
                                           where v.Field<String>( "server" ) == server || v.Field<String>( "server" ) == Environment.MachineName
                                           group v.Field<String>( "name" ) by v.Field<String>( "server" ) ).ToDictionary( ( v ) => v.Key );
//                var sqlServerInfo = Microsoft.SqlServer.Management.Smo.SmoApplication.EnumAvailableSqlServers( false );
                //var searchedServerInfo = from v in sqlServerInfo.Rows.OfType<DataRowCollection>() where v
                //var searchedServerInfo = from v in sqlServerInfo.AsEnumerable()
                //                         where v.Field<String>( "server" ) == server || v.Field<String>( "server" ) == Environment.MachineName
                //                         group v.Field<String>("name") by v.Field<String>( "server" );
                appendString = new Tuple<String,String>( searchedServerInfo.Keys.Contains( Environment.MachineName ) ? searchedServerInfo[Environment.MachineName].First() : String.Empty, searchedServerInfo.Keys.Contains( server ) ? searchedServerInfo[server].First() : String.Empty );

                //Object localEdition = Microsoft.SqlServer.Management.Smo.SmoApplication.EnumAvailableSqlServers( true ).Rows[0].ItemArray[2];
                //var smo = new Microsoft.SqlServer.Management.Smo.Server( "ASTEC00419" );
                //var smomo = smo.Databases;
                //var enm = smomo.GetEnumerator();

                //while ( enm.MoveNext() )
                //{
                //    var svr = enm.Current;
                //}

                ////foreach ( var srv in smomo.GetEnumerator() )
                ////{

                ////}
                //Object remoteEdition = Microsoft.SqlServer.Management.Smo.SmoApplication.EnumAvailableSqlServers( "ASTEC00419" ).Rows[0].ItemArray[2];

                //String edition = (String)Microsoft.SqlServer.Management.Smo.SmoApplication.EnumAvailableSqlServers( true ).Rows[0].ItemArray[2];
                //String vv = (String)Microsoft.SqlServer.Management.Smo.SmoApplication.EnumAvailableSqlServers( "ASTEC00419" ).Rows[0].ItemArray[2];
                //if ( edition.Length != 0 )
                //{
                //    edition.Insert( 0, @"\" );
                //}
                //using ( SqlConnection scnMASTER = new SqlConnection( "Data Source=(local)\\SQLEXPRESS;" + "Integrated Security=SSPI;" ) )
                //{
                //    scnMASTER.Open();

                //    String strSQL = " SELECT @@VERSION ";
                //    SqlCommand cmd = new SqlCommand( strSQL, scnMASTER );
                //    using ( SqlDataReader rdr = cmd.ExecuteReader() )
                //    {
                //        rdr.Read();

                //        if ( rdr[0] != DBNull.Value )
                //        {
                //            version = rdr[0].ToString();

                //        }
                //        //if ( rdr[0] != DBNull.Value )
                //        //    str = "VERSION：" + rdr[0].ToString() + "\r\n";
                //        //else
                //        //    str = "VERSION：（不明）\r\n";
                //        //                rdr.Close();
                //    }
                //}
            }
            catch ( Exception ex )
            {
                // バージョン確認失敗
                Debug.WriteLine( ex.Message );
                System.Diagnostics.Debug.WriteLine(String.Format("{0} {1}", ex.Message, ex.StackTrace));
            }
//            scnMASTER.Close();
            return appendString;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <remarks>
        /// 初期化処理を行います。
        /// </remarks>
        /// <param name="dbInstanceName"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public Boolean Initialize( String dbInstanceName, String strDataBase, Int32 interval, Int32 timeout, String dbFilePath = null )
        {
            Boolean bRet = true;
            Boolean sqlServerAlive = false;

            // 接続文字列作成
            SqlConnectionStringBuilder sqlConnectStringBuilder = new SqlConnectionStringBuilder();
            //"Data Source ={0}; Trusted_Connection=yes; Connection Timeout={1}; Initial Catalog ={2}; Integrated Security = SSPI"
            sqlConnectStringBuilder.DataSource = dbInstanceName;
            sqlConnectStringBuilder.InitialCatalog = strDataBase;
            sqlConnectStringBuilder.ConnectTimeout = timeout;
            sqlConnectStringBuilder.IntegratedSecurity = true; // Windows認証

//#if DEBUG_CARIS // TODO:CARIS開発用
#if false
            if ( System.Windows.Forms.MessageBox.Show( "ローカルDBファイルを使用しますか？", "", System.Windows.Forms.MessageBoxButtons.OKCancel ) != System.Windows.Forms.DialogResult.OK )
            {
                sqlConnectStringBuilder.DataSource = dbInstanceName;//@"ASTEC00419" + localAppendConnectString;
                sqlConnectStringBuilder.PersistSecurityInfo = true;
                sqlConnectStringBuilder.UserID = "caris";
                sqlConnectStringBuilder.Password = "caris";
                sqlConnectStringBuilder.IntegratedSecurity = false;
                sqlConnectStringBuilder.AttachDBFilename = String.Empty;

            }
            else
#endif
            {
                if ( dbFilePath != null )
                {
                    sqlConnectStringBuilder.AttachDBFilename = dbFilePath;
                    Microsoft.SqlServer.Management.Smo.Server server = null;
                    
                    // システム起動時立ち上げの場合、アプリがSQLServerより先に立ち上がっている時はコネクトに失敗する為、再試行を行う。
                    Boolean retry = true;
                    Int32 SQLSERVER_CONNECT_RETRY_COUNT = 10;
                    Int32 SQLSERVER_CONNECT_RETRY_INTERVAL = 1000;
                    Int32 connectTryCount = 0;
                    while( retry )
                    {
                        connectTryCount++;
                        try
                        {
                            server = new Microsoft.SqlServer.Management.Smo.Server( dbInstanceName );
                            retry = false;
                            sqlServerAlive = server.Databases.Contains( strDataBase );
                        }
                        catch ( Exception ex )
                        {
                            Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, String.Format( "{0}", ex.Message ) );

                            // 接続再試行回数を判定する
                            retry = connectTryCount <= SQLSERVER_CONNECT_RETRY_COUNT;

                            // 時間を置く
                            System.Threading.Thread.Sleep( SQLSERVER_CONNECT_RETRY_INTERVAL );
                        }
                    }
                    if ( sqlServerAlive )
                    {
                        try
                        {
                            server.DetachDatabase( strDataBase, false );
                        }
                        catch(Exception ex )
                        {
                            Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, String.Format( "{0}",ex.Message ) );
                        }
                    }
                }
            }

#if false



            //String appendConnectString  = @"\SQLEXPRESS";
            String serverName = strServer;
#if DEBUG
            var appendStrings = GetServerName( "ASTEC00419" );
#else
            var appendStrings = GetServerName( strServer );
#endif
            serverName = appendStrings.Item2;
            String localServerName = appendStrings.Item1;

            // 接続文字列作成
            SqlConnectionStringBuilder sqlConnectStringBuilder = new SqlConnectionStringBuilder();
            //"Data Source ={0}; Trusted_Connection=yes; Connection Timeout={1}; Initial Catalog ={2}; Integrated Security = SSPI"
            sqlConnectStringBuilder.DataSource = localServerName;
            sqlConnectStringBuilder.InitialCatalog = strDataBase;
            sqlConnectStringBuilder.ConnectTimeout = timeout;

            sqlConnectStringBuilder.IntegratedSecurity = true;

#if DEBUG_CARIS // TODO:CARIS開発用
            if ( System.Windows.Forms.MessageBox.Show( "ローカルDBファイルを使用しますか？", "", System.Windows.Forms.MessageBoxButtons.OKCancel ) != System.Windows.Forms.DialogResult.OK )
            {
                sqlConnectStringBuilder.DataSource = serverName;//@"ASTEC00419" + localAppendConnectString;
                sqlConnectStringBuilder.PersistSecurityInfo = true;
                sqlConnectStringBuilder.UserID = "caris";
                sqlConnectStringBuilder.Password = "caris";
                sqlConnectStringBuilder.IntegratedSecurity = false;
                sqlConnectStringBuilder.AttachDBFilename = String.Empty;

            }
            else
#endif
            {
                if ( dbFilePath != null )
                {
                    sqlConnectStringBuilder.AttachDBFilename = dbFilePath;
                    if ( new Microsoft.SqlServer.Management.Smo.Server( localServerName ).Databases.Contains( strDataBase ) )
                    {
                        new Microsoft.SqlServer.Management.Smo.Server( localServerName ).DetachDatabase( strDataBase, false );
                    }
                }
            }
#endif
            SQLServerDBAccess.m_connectionString = sqlConnectStringBuilder.ConnectionString;
            //// 接続文字列作成
            //this.m_connectionString = String.Format( DB_CONNECT_FORMAT, strServer, timeout, strDataBase );
            ////this.m_connectionString = String.Format( DB_CONNECT_FORMAT, strServer, timeout, strDbFilePath, strDataBase );


            // SQLコマンドキュー監視待機インターバル
            this.Interval = interval;

            return bRet && sqlServerAlive;
        }


        // TODO : Carisデバッグ用オーバーロード
        /// <summary>
        /// 初期化
        /// </summary>
        /// <remarks>
        /// 初期化処理を行います。
        /// </remarks>
        /// <param name="strServer"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public Boolean Initialize( String strServer, String strDataBase, Int32 interval, Int32 timeout, String uid, String password )
        {
            Boolean bRet = true;

            // 接続文字列作成
            SqlConnectionStringBuilder sqlConnectStringBuilder = new SqlConnectionStringBuilder();
            sqlConnectStringBuilder.DataSource = strServer;
            sqlConnectStringBuilder.InitialCatalog = strDataBase;
            sqlConnectStringBuilder.ConnectTimeout = timeout;
            sqlConnectStringBuilder.PersistSecurityInfo = true;
            sqlConnectStringBuilder.UserID = uid;
            sqlConnectStringBuilder.Password = password;
            SQLServerDBAccess.m_connectionString = sqlConnectStringBuilder.ConnectionString;

            // SQLコマンドキュー監視待機インターバル
            this.Interval = interval;

            return bRet;
        }

        /// <summary>
        /// スレッド状態チェック
        /// </summary>
        /// <remarks>
        /// スレッドが停止していたら再スタートさせ、結果を返します。
        /// </remarks>
        /// <returns></returns>
        private Boolean checkThread()
        {
            Boolean bRet = true;

            if ( !this.IsAlive )
            {
                bRet = this.Start();
            }

            return bRet;
        }

        #region [ コネクション ]

        /// <summary>
        /// 接続
        /// </summary>
        /// <returns></returns>
        public Boolean Open()
        {
            Boolean bRet = true;
            try
            {
                //if ( dbSqlConnection == null )
                //{
                    dbSqlConnection = new SqlConnection( SQLServerDBAccess.m_connectionString );
                    //dbSqlConnection.Open();
                //}

                this.m_sqlCommand = new SqlCommand();               
                this.m_sqlCommand.Connection = dbSqlConnection;
                // m_sqlCommand.CommandTimeout = 30; // コマンド実行待機時間：defaultは30秒

                // SQLコマンドキュー監視スレッドスタート
                bRet = this.Start();
            }
            catch ( Exception ex )
            {
                Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, ex.ToString() );
                bRet = false;
            }

            return bRet;
        }
        /// <summary>
        /// DB接続処理
        /// </summary>
        /// <remarks>
        /// DB接続をオープンします。
        /// </remarks>
        /// <returns></returns>
        protected Boolean connect()
        {
            Boolean result = true;

            try
            {
                //dbSqlConnection.Open();
                if ( m_sqlCommand != null )
                {
                    m_sqlCommand.Connection.Open();
                }
            }
            catch ( Exception ex )
            {
                Debug.WriteLine( ex.Message );
                System.Diagnostics.Debug.WriteLine(String.Format("{0} {1}", ex.Message, ex.StackTrace));
                result = false;
            }
            return result;
        }
        /// <summary>
        /// DB切断
        /// </summary>
        /// <remarks>
        /// DB接続をクローズします。
        /// </remarks>
        /// <returns></returns>
        public Boolean disConnect()
        {
            Boolean bRet = true;
            try
            {
                if ( m_sqlCommand != null )
                {
                    m_sqlCommand.Connection.Close();
                }
                //dbSqlConnection.Close();
            }
            catch ( Exception ex )
            {
                Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, ex.ToString() );
                bRet = false;
            }

            return bRet;
        }

        /// <summary>
        /// 切断
        /// </summary>
        /// <remarks>
        /// スレッドの停止を行います
        /// </remarks>
        /// <returns></returns>
        public Boolean Close()
        {
            Boolean bRet = true;
            try
            {
                //bRet = this.disConnect();
                bRet = this.EndJoin();
            }
            catch ( Exception ex )
            {
                Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, ex.ToString() );
                bRet = false;
            }

            return bRet;
        }

        #endregion

        #region [ クエリキュー追加処理 ]

        /// <summary>
        /// SQLの追加(Select)
        /// </summary>
        /// <remarks>
        /// SelectSQLをキューに追加します。
        /// </remarks>
        /// <param name="strSql"></param>
        /// <param name="uid"></param>
        public void OpenQuery( String strSql, String key )
        {
            this.checkThread();

            SqlInfo sqlInfo = new SqlInfo();
            sqlInfo.Key = key;
            sqlInfo.Type = CommandType.Text;
            sqlInfo.Kind = SqlKind.Select;
            sqlInfo.Sql = strSql;

            this.m_qSql.Lock();
            this.m_qSql.Get.Instance.Enqueue( sqlInfo );
            this.m_qSql.UnLock();

            queuePushNotify.Set();

        }

        /// <summary>
        /// SQLの追加(Insert/Update/Delete)
        /// </summary>
        /// <remarks>
        /// 更新SQLをキューに追加します。
        /// </remarks>
        /// <param name="strSql"></param>
        /// <param name="key"></param>
        public void ExecuteSql( String strSql, String key )
        {
            this.checkThread();

            SqlInfo sqlInfo = new SqlInfo();
            sqlInfo.Key = key;
            sqlInfo.Type = CommandType.Text;
            sqlInfo.Kind = SqlKind.Execute;
            sqlInfo.Sql = strSql;

            this.m_qSql.Lock();
            this.m_qSql.Get.Instance.Enqueue( sqlInfo );
            this.m_qSql.UnLock();


            queuePushNotify.Set();
        }

        /// <summary>
        /// ストアドプロシージャの追加
        /// </summary>
        /// <remarks>
        /// ストアドプロシージャをキューに追加します。
        /// </remarks>
        /// <param name="strSql"></param>
        public void ExecuteStoredProcedure( String StoredProcedureName, List<SqlParam> SqlParams, String key )
        {
            this.checkThread();

            SqlInfo sqlInfo = new SqlInfo();
            sqlInfo.Key = key;
            sqlInfo.Type = CommandType.StoredProcedure;
            sqlInfo.Kind = SqlKind.Procedure;
            sqlInfo.Sql = StoredProcedureName;

            if ( SqlParams != null )
            {
                foreach ( SqlParam p in SqlParams )
                {
                    sqlInfo.Params.Add( p );
                }
            }

            this.m_qSql.Lock();
            this.m_qSql.Get.Instance.Enqueue( sqlInfo );
            this.m_qSql.UnLock();
        }

        #endregion

#if false// false:トランザクション無効
        #region [ トランザクション処理 ]
        

        /// <summary>
        /// トランザクションの開始
        /// </summary>
        /// <remarks>
        /// トランザクションを開始します
        /// </remarks>
        public void StartTransaction()
        {
            Console.WriteLine( "StartTransaction" );

            // トランザクションの開始
            this.m_sqltran = SQLServerDBAccess.dbSqlConnection.BeginTransaction();
            this.m_sqlCommand.Transaction = this.m_sqltran;
        }

        /// <summary>
        /// トランザクションの終了
        /// </summary>
        /// <remarks>
        /// トランザクションを終了します。
        /// </remarks>
        public void EndTransaction()
        {
            this.m_sqltran = null;
            this.m_sqlCommand.Transaction = null;
            this.m_istran = false;

            Console.WriteLine( "EndTransaction" );

        }

        /// <summary>
        /// コミット
        /// </summary>
        /// <remarks>
        /// トランザクションをコミットします。
        /// </remarks>
        /// <returns></returns>
        /// <returns>true:成功、false:失敗</returns>
        public Boolean Commit()
        {
            try
            {
                this.m_sqltran.Commit();
            }
            catch ( Exception ex )
            {
                Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, ex.ToString() );
                return false;
            }

            Console.WriteLine( "Commit" );

            return true;
        }

        /// <summary>
        /// ロールバック
        /// </summary>
        /// <remarks>
        /// トランザクションをロールバックします。
        /// </remarks>
        /// <returns></returns>
        /// <returns>true:成功、false:失敗</returns>
        public Boolean RollBack()
        {
            try
            {
                this.m_sqltran.Rollback();
            }
            catch ( Exception ex )
            {
                Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, ex.ToString() );
                return false;
            }

            Console.WriteLine( "RollBack" );

            return true;
        }

        #endregion
#endif

        /// <summary>
        /// 解放
        /// </summary>
        /// <remarks>
        /// スレッドの停止を行います。
        /// </remarks>
        public void Dispose()
        {
            // スレッドの停止(失敗しても無視)
            this.EndJoin();
            //this.EndJoin();
        }


        #region [ クエリキュー監視・実行 ]
        /// <remarks>
        /// キュー追加通知シグナルイベント
        /// </remarks>
        static protected ManualResetEvent queuePushNotify = new ManualResetEvent(false);
        /// <summary>
        /// SQL実行対象クラスリスト
        /// </summary>
        static private LockObject<List<SQLServerDBAccess>> classList = new LockObject<List<SQLServerDBAccess>>();
        /// <summary>
        /// ベーススレッド処理
        /// </summary>
        /// <remarks>
        /// ベーススレッドの処理を実行します。
        /// </remarks>
        /// <param name="parameter"></param>
        static protected void baseThreadFunction( Object parameter )
        {
            Debug.WriteLine( "baseThreadFunction" );
            while ( !baseThreadEnd.WaitOne( 0 ) )
            {
                if ( queuePushNotify.WaitOne() )
                {
                    if ( baseThreadEnd.WaitOne( 0 ) )
                    {
                        // 終了
                        break;
                    }
                    queuePushNotify.Reset();
                    // スレッド開始動作を行っている自インスタンスのスレッド関数呼び出しを行う。
                    // 単独スレッドからのDBアクセスを保ちつつ、スレッド内部処理を分離する。
                    classList.Lock();
                    List<SQLServerDBAccess> tasks = classList.Get.Instance;
                    SQLServerDBAccess[] tmpAry = tasks.ToArray();
                    classList.UnLock();

                    foreach ( SQLServerDBAccess instance in tmpAry )
                    {
                        instance.sqlThreadFunction();
                    }
                }
            }
            //disConnect();
        }


        /// <summary>
        /// SQL処理
        /// </summary>
        /// <remarks>
        /// SQLを処理します
        /// </remarks>
        protected virtual void sqlThreadFunction()
        {
            //SQLServerDBAccess instance = parameter as SQLServerDBAccess;

            //Console.WriteLine( "!!! SQLSereverDBAccess:threadFunction　スレッドを開始しました。" );

            //while ( this.EndEvent.WaitOne( this.Interval ) )
            //{
            //Console.WriteLine( String.Format( " SQLServerDBAccess:threeadFunction (interval:{0}) now:{1}",
            //                    this.Interval, DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss.fff" ) ) );

            // 結果格納オブジェクト
            Object objResult = null;
            //// SQLキュー情報
            //SqlInfo info = null;

            // SQLキューを取り出し
            this.m_qSql.Lock();
            SqlInfo[] infoList = this.m_qSql.Get.Instance.ToArray();
            this.m_qSql.Get.Instance.Clear();
            this.m_qSql.UnLock();


            // キューに載っている分処理
            Int32 nCount = infoList.Count();
            if ( nCount > 0 )
            {
                String lastSqlKey = String.Empty;

                // コネクションオープン
                this.connect();
                //this.m_sqlCommand.Connection.Open();
#if false// false:トランザクション無効
                // トランザクション処理
                if ( this.m_istran )
                {
                    this.StartTransaction();
                }
#endif
                foreach ( var info in infoList )
                {
                    try
                    {
                        if (info.Sql.Equals(String.Empty))
                        {
                            continue;
                        }

                        // 前回の結果をクリア
                        objResult = null;
                        //info = null;

                        // 実行するSQLコマンドを作成する
                        //info = this.m_qSql.Get.Instance.Dequeue();
                        this.m_sqlCommand.CommandType = info.Type;
                        this.m_sqlCommand.CommandText = info.Sql;
                        // 前回のSQLパラメータをクリア
                        this.m_sqlCommand.Parameters.Clear();
                        // 今回のSQLパラメータを作成
                        for ( Int32 j = 0; j < info.Params.Count; j++ )
                        {
                            this.m_sqlCommand.Parameters.Add(
                                info.Params[j].Name,
                                info.Params[j].Type ).Value
                                    = info.Params[j].Value;
                        }

                        // SQLログ出力
                        Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, this.m_sqlCommand.CommandText.ToString() );

                        //// Unicode対応を行う
                        //this.m_sqlCommand.CommandText = this.SqlUnicodeStringFormat( this.m_sqlCommand.CommandText );

                        // 実行
                        if ( info.Kind == SqlKind.Select ||
                             info.Kind == SqlKind.Procedure )
                        {
                            // Select句・プロシージャ
                            SqlDataAdapter sqlAdapter = new SqlDataAdapter( this.m_sqlCommand );
                            objResult = new DataTable();
                            sqlAdapter.Fill( (DataTable)objResult );
                        }
                        else
                        {
                            // Insert・Delete・Update
                            objResult = this.m_sqlCommand.ExecuteNonQuery();
                        }
                    }
                    catch ( Exception ex )
                    {
                        Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, ex.ToString() );
#if false// false:トランザクション無効
                        // トランザクション処理中ならロールバック
                        if ( this.m_istran )
                        {
                            this.RollBack();
                            this.EndTransaction();
                            this.m_qSql.Get.Instance.Clear();
                            objResult = -1;

                            // 完了通知
                            if ( OnCompleteDBQueue != null )
                            {
                                OnCompleteDBQueue( this, new DBQueueEventArgs( info.Key, objResult ) );
                            }

                            break;
                        }
#endif
                    }
                    finally
                    {
#if false// false:トランザクション無効
                        // トランザクション処理でなければ実行単位で完了通知
                        if ( !this.m_istran )
                        {
#endif
                            // 完了通知
                            if ( OnCompleteDBQueue != null )
                            {
                                OnCompleteDBQueue( this, new DBQueueEventArgs( info.Key, objResult ) );
                            }
#if false// false:トランザクション無効
                        }
#endif

                        lastSqlKey = info.Key;
                    }
                }
                
#if false// false:トランザクション無効
                // トランザクション処理の場合、すべてのSQLの実行が完了したらコミットしてイベント通知
                if ( this.m_istran )
                {
                    this.Commit();
                    this.EndTransaction();

                    // 完了通知（最後のSQL実行結果）
                    if ( OnCompleteDBQueue != null )
                    {
                        OnCompleteDBQueue( this, new DBQueueEventArgs( lastSqlKey, objResult ) );
                    }
                }
#endif

                //// コネクションクローズ
                this.m_sqlCommand.Connection.Close();
                disConnect();
            }

            //}

            //Console.WriteLine( "!!! SQLSereverDBAccess:threadFunction　タイムアウトしました。" );
        }

        /// <summary>
        /// SQL構文ユニコード対応処理
        /// </summary>
        /// <remarks>
        /// SQL構文内のValue部に対してUnicode対応を行います。
        /// </remarks>
        /// <param name="sql">SQL構文</param>
        /// <returns>Unicode対応済SQL</returns>
        private String SqlUnicodeStringFormat( String sql )
        {
            // TODO:パターン漏れが懸念されるためこの関数は現在利用不可です。

            const String UNICODE_SYMBOL = "N";
            Int32 unicodeSymbolLength = UNICODE_SYMBOL.Length;

            // SQL文の"Value()"部にあるシングルクォーテーションで囲まれた値に対してNを付加する。（Unicode対応）
            Regex replaceSentenceRegex = new Regex( @"values *\(.*?\)", RegexOptions.IgnoreCase );
            Regex replaceTargetRegex = new Regex( @"(, *?')|(\( *')" );
            String result = sql;

            if ( replaceSentenceRegex.IsMatch( sql ) )
            {
                var replaceMatches = replaceSentenceRegex.Matches( result );
                Int32 indexAdjustCount = 0; // 付与位置補正

                foreach ( Match replaceSentence in replaceMatches )
                {
                    if ( replaceTargetRegex.IsMatch( replaceSentence.Value ) )
                    {

                        // 文字の付与により増加する文字列長に対して、付与前のヒット位置インデックスからの補正を行う。
                        Int32 shiftCount = 0;
                        var replacedSentence = replaceTargetRegex.Replace( replaceSentence.Value, ( replaceTarget ) =>
                        {
                            shiftCount += unicodeSymbolLength;
                            return replaceTarget.Value.Insert( replaceTarget.Value.Length - 1, UNICODE_SYMBOL );
                        } );

                        result = replaceSentenceRegex.Replace( result, replacedSentence, 1, replaceSentence.Index + indexAdjustCount );
                        indexAdjustCount += shiftCount;
                    }
                }
            }

            return result;
        }

        ///// <summary>
        ///// キューにあるSQLの実行
        ///// </summary>
        //protected override void threadFunction()
        //{
        //    Console.WriteLine( "!!! SQLSereverDBAccess:threadFunction　スレッドを開始しました。" );

        //    while ( this.EndEvent.WaitOne( this.Interval ) )
        //    {
        //        Console.WriteLine( String.Format( " SQLServerDBAccess:threeadFunction (interval:{0}) now:{1}",
        //                            this.Interval, DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss.fff" ) ) );

        //        m_qSql.Lock();

        //        // 結果格納オブジェクト
        //        Object objResult = null;
        //        // SQLキュー情報
        //        SqlInfo info = null;

        //        // コネクションオープン
        //        this.Open();

        //        // トランザクション処理
        //        if ( this.m_istran )
        //        {
        //            this.StartTransaction();
        //        }

        //        // キューに載っている分処理
        //        Int32 nCount = this.m_qSql.Get.Instance.Count;
        //        for ( Int32 i = 0; i < nCount; i++ )
        //        {
        //            try
        //            {
        //                // 前回の結果をクリア
        //                objResult = null;
        //                info = null;

        //                // 実行するSQLコマンドを作成する
        //                info = this.m_qSql.Get.Instance.Dequeue();
        //                this.m_sqlCommand.CommandType = info.Type;
        //                this.m_sqlCommand.CommandText = info.Sql;
        //                // 前回のSQLパラメータをクリア
        //                this.m_sqlCommand.Parameters.Clear();
        //                // 今回のSQLパラメータを作成
        //                for ( Int32 j = 0; j < info.Params.Count; j++ )
        //                {
        //                    this.m_sqlCommand.Parameters.Add(
        //                        info.Params[j].Name,
        //                        info.Params[j].Type ).Value
        //                            = info.Params[j].Value;
        //                }

        //                // SQLログ出力
        //                Singleton<LogManager>.Instance.Debug( this.m_sqlCommand.CommandText.ToString() );

        //                // 実行
        //                if ( info.Kind == SqlKind.Select ||
        //                     info.Kind == SqlKind.Procedure )
        //                {
        //                    // Select句・プロシージャ
        //                    SqlDataAdapter sqlAdapter = new SqlDataAdapter( this.m_sqlCommand );
        //                    objResult = new DataTable();
        //                    sqlAdapter.Fill( (DataTable)objResult );
        //                }
        //                else
        //                {
        //                    // Insert・Delete・Update
        //                    objResult = m_sqlCommand.ExecuteNonQuery();
        //                }
        //            }
        //            catch ( Exception ex )
        //            {
        //                Singleton<LogManager>.Instance.Error( ex.ToString() );
        //                // トランザクション処理中ならロールバック
        //                if ( this.m_istran )
        //                {
        //                    this.RollBack();
        //                    this.EndTransaction();
        //                    this.m_qSql.Get.Instance.Clear();
        //                    objResult = -1;

        //                    // 完了通知
        //                    if ( OnCompleteDBQueue != null )
        //                    {
        //                        OnCompleteDBQueue( this, new DBQueueEventArgs( info.Key, objResult ) );
        //                    }

        //                    break;
        //                }
        //            }
        //            finally
        //            {
        //                // トランザクション処理でなければ実行単位で完了通知
        //                if ( !this.m_istran )
        //                {
        //                    // 完了通知
        //                    if ( OnCompleteDBQueue != null )
        //                    {
        //                        OnCompleteDBQueue( this, new DBQueueEventArgs( info.Key, objResult ) );
        //                    }
        //                }
        //                // トランザクション処理の場合、すべてのSQLの実行が完了したらコミットしてイベント通知
        //                else if ( this.m_qSql.Get.Instance.Count == 0 )
        //                {
        //                    this.Commit();
        //                    this.EndTransaction();

        //                    // 完了通知（最後のSQL実行結果）
        //                    if ( OnCompleteDBQueue != null )
        //                    {
        //                        OnCompleteDBQueue( this, new DBQueueEventArgs( info.Key, objResult ) );
        //                    }
        //                }
        //            }
        //        }

        //        // コネクションクローズ
        //        this.Close();

        //        m_qSql.UnLock();
        //    }

        //    Console.WriteLine( "!!! SQLSereverDBAccess:threadFunction　タイムアウトしました。" );
        //}

        #endregion

        #endregion
   }

}
