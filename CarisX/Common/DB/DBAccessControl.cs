using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

using System.Diagnostics;
using System.Threading;
using System.Data.SqlClient;

using Oelco.Common.Log;
using Oelco.Common.Utility;
using System.ComponentModel;

namespace Oelco.Common.DB
{
    /// <summary>
    /// SQLを実行するクラスが継承する基底クラス
    /// </summary>
    /// <remarks>
    /// SQLを実行するクラスが継承する基底クラスです。
    /// </remarks>
    public class DBAccessControl : SQLServerDBAccess
    {
        #region [定数定義]

        /// <summary>
        /// テーブル処理種別
        /// </summary>
        protected enum TableTransactKind
        {
            /// <summary>
            /// 無し
            /// </summary>
            None,
            /// <summary>
            /// 読込
            /// </summary>
            Read,
            /// <summary>
            /// 書込み
            /// </summary>
            Write
        }
        
        /// <summary>
        /// テーブル設定/取得タイムアウトデフォルト値(ms)
        /// </summary>
        protected const Int32 TABLE_TRANSACT_TIMEOUT_DEFAULT = 5000;

        /// <summary>
        /// 同期待機時間
        /// </summary>
        private const Int32 SQL_WAIT_TIME = 10 * 1000; // 10秒待機

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// キュー判定キー
        /// </summary>
        private Dictionary<String, Object> m_keys = new Dictionary<String, Object>();

        /// <summary>
        /// 完了待機
        /// </summary>
        private ManualResetEvent m_manualEvent = new ManualResetEvent( false );

        /// <summary>
        /// データテーブル
        /// </summary>
        private DataTable dataTable = null;

        /// <summary>
        /// データ保持最大件数
        /// </summary>
        private Int32 recordCountMax = Int32.MaxValue;

        /// <summary>
        /// SQLデータアダプタ
        /// </summary>
        private SqlDataAdapter adapter = null;

        /// <summary>
        /// SQLコマンドビルダ
        /// </summary>
        private SqlCommandBuilder builder = null;

        /// <summary>
        /// テーブルDBトランザクション待機用同期イベント
        /// </summary>
        private ManualResetEvent tableTransacted = new ManualResetEvent( false );

        /// <summary>
        /// テーブル処理種別
        /// </summary>
        private TableTransactKind transactKind = TableTransactKind.None;

        /// <summary>
        /// 利用者への完了通知イベント
        /// </summary>
        public event CompleteDBQueueHandler OnCompleteDBAccess;

        /// <summary>
        /// ミューテックス
        /// </summary>
        protected Mutex baseTableMutex = new Mutex();

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DBAccessControl()
            : this( null )
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="recordCountMax">データ保持最大件数</param>
        public DBAccessControl( Int32? recordCountMax )
        {
            this.TableTransactTimeout = TABLE_TRANSACT_TIMEOUT_DEFAULT;  // テーブル取得待ち時間デフォルト設定
            if ( recordCountMax.HasValue )
            {
                this.RecordCountMax = recordCountMax.Value;
            }

        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// データテーブル
        /// </summary>
        protected DataTable DataTable
        {
            get
            {
                return dataTable;
            }
        }

        /// <summary>
        /// データ保持最大件数
        /// </summary>
        /// <remarks>
        /// データテーブルの書込みを行う際、このプロパティを使用してデータの超過分を削除します。
        /// 保持件数だけでデータ件数の超過を判定出来ない場合、removeLimitOverのオーバーライドによって独自に処理を定義してください。
        /// </remarks>
        public Int32 RecordCountMax
        {
            get
            {
                return this.recordCountMax;
            }
            protected set
            {
                this.recordCountMax = value;
            }
        }

        /// <summary>
        /// データテーブル取得SQL
        /// </summary>
        protected virtual String baseTableSelectSql
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// データテーブル取得/設定タイムアウト
        /// </summary>
        protected virtual Int32 TableTransactTimeout
        {
            get;
            set;
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// キュー識別キー作成
        /// </summary>
        /// <remarks>
        /// キュー識別するキーを作成し、文字列を返します。
        /// </remarks>
        /// <returns>キュー識別キー文字列</returns>
        public String GetKey()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// データの取得
        /// </summary>
        /// <remarks>
        /// 指定したテーブルのデータを返します。
        /// </remarks>
        /// <param name="tableName">テーブル名</param>
        /// <param name="dt">取得データテーブル</param>
        /// <returns>取得行数</returns>
        public Int32 Fill( String strTableName, out DataTable dt )
        {
            return this.Fill( strTableName, "", out dt );
        }

        /// <summary>
        /// 条件付きデータ検索
        /// </summary>
        /// <remarks>
        /// 指定したテーブルのデータを条件付きで抽出して取得した結果を返します。
        /// </remarks>
        /// <param name="tableName">テーブル名</param>
        /// <param name="strWhere">絞り込み条件</param>
        /// <param name="dt">取得データテーブル</param>
        /// <returns>取得行数</returns>
        public Int32 Fill( String strTableName, String strWhere, out DataTable dt )
        {
            // SQL作成
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat( " SELECT * FROM dbo.{0}", strTableName );
            if ( !String.IsNullOrEmpty( strWhere ) )
            {
                sb.AppendFormat( " WHERE {0}", strWhere );
            }
            dt = this.OpenQuery( sb.ToString() );

            Int32 iRetNum = 0;
            if ( dt != null )
            {
                iRetNum = dt.Rows.Count;
            }
            return iRetNum;
        }

        /// <summary>
        /// DBのアクセスパス取得
        /// </summary>
        /// <returns></returns>
        public String GetDBAccessFilePath()
        {
            String resultPath = String.Empty;

            try
            {
                // SQL作成
                DataTable dt = this.OpenQuery(" SELECT physical_name FROM sys.database_files");
                if (dt != null)
                {
                    resultPath = dt.Rows[0][0].ToString();
                }
            }
            catch(Exception ex)
            {
                // DBのアクセスパスの取得動作が失敗しました。
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("Failure. DB access path = {0}:{1}", ex.Message, ex.InnerException.Message));
            }

            return resultPath;
        }

        /// <summary>
        /// DBデータの取得
        /// </summary>
        /// <remarks>
        /// データをDBから読み込みます。
        /// </remarks>
        public virtual void LoadDB()
        {
            Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "LoadDB you have not implemented was called.");
        }

        /// <summary>
        /// COUNT SQLの実行
        /// </summary>
        /// <remarks>
        /// COUNT SQLを実行し件数を返します。
        /// </remarks>
        /// <param name="tableName">件数取得を行うテーブル名</param>
        /// <param name="strWhere">取得条件（Where句以降のSQL文を設定します。）</param>
        /// <returns>
        /// 正常時：件数　異常時：-1
        /// </returns>
        public Int32 Count( String strTableName, String strWhere )
        {
            // SQL作成
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat( " SELECT COUNT(*) FROM dbo.{0}", strTableName );
            if ( !String.IsNullOrEmpty( strWhere ) )
            {
                sb.AppendFormat( " WHERE {0}", strWhere );
            }

            String key = this.GetKey();
            this.preExecute( key );

            this.OpenQuery( sb.ToString(), key );

            // 完了待機
            if ( !this.m_manualEvent.WaitOne( SQL_WAIT_TIME ) )
            {
                //DBAccessControlのCountの結果を取得できませんでした。
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "It was not possible to get the results of the Count of DBAccessControl.");
                return -1;
            }

            DataTable dt = (DataTable)this.m_keys[key];

            this.m_keys.Remove( key );

            if ( dt == null )
            {
                return -1;
            }
            else
            {
                if ( dt.Rows.Count == 0 )
                {
                    return 0;
                }
                else
                {
                    return (Int32)dt.Rows[0].ItemArray[0];
                }
            }
        }
        
        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// データテーブル操作設定
        /// </summary>
        /// <remarks>
        /// データテーブル-DB間に操作を設定します。
        /// DBスレッドより処理されます。
        /// </remarks>
        /// <param name="kind">操作種別</param>
        protected void setTransact( TableTransactKind kind )
        {
            this.transactKind = kind;
            this.tableTransacted.Reset();

            SQLServerDBAccess.queuePushNotify.Set();
        }

        /// <summary>
        /// テーブルの読み込み
        /// </summary>
        /// <remarks>
        /// テーブルの読込処理を行います。
        /// </remarks>
        protected virtual void fillBaseTable()
        {
            this.baseTableMutex.WaitOne();
            this.setTransact( TableTransactKind.Read );
            if ( !this.tableTransacted.WaitOne( TableTransactTimeout ) )
            {
                // タイムアウト
                //テーブル読込がタイムアウトしました。
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "data Table read timed out.");
            }
            this.baseTableMutex.ReleaseMutex();
        }

        /// <summary>
        /// 最大保持件数オーバー時処理
        /// </summary>
        /// <remarks>
        /// データ最大件数超過の際の処理を定義します。ここでは保持するDataTableに対して変更を行います。
        /// 削除条件が複雑なテーブルの場合はこの関数をオーバーライドします。
        /// </remarks>
        protected virtual void removeLimitOver()
        {
            // デフォルトの削除動作 既定件数までを先頭から順に削除状態とする。
            if ( this.DataTable != null )
            {
                Int32 delCount = this.DataTable.Rows.Count - this.RecordCountMax;
                if ( delCount > 0 )
                {
                    for ( Int32 i = 0; i < delCount; i++ )
                    {
                        this.DataTable.Rows[i].Delete();
                    }
                }
            }
            //            Debug.WriteLine( String.Format( "このクラスはremoveLimitOverが実装されていません。({0})", this.GetType().Name ) );
        }

        /// <summary>
        /// テーブル更新処理
        /// </summary>
        /// <remarks>
        /// テーブルの更新処理を行います。
        /// </remarks>
        protected virtual void updateBaseTable()
        {
            this.baseTableMutex.WaitOne();
            //            this.transactKind = TableTransactKind.Write;
            this.removeLimitOver();
            this.setTransact( TableTransactKind.Write );
            if ( !this.tableTransacted.WaitOne( TableTransactTimeout ) )
            {
                // タイムアウト
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "write data Table timed out.");
                ////                Singleton<LogManager>.Instance.Error( "テーブル書込がタイムアウトしました。" );
            }
            this.baseTableMutex.ReleaseMutex();
        }

        /// <summary>
        /// DBコンポーネントリフレッシュ
        /// </summary>
        /// <remarks>
        /// 別スレッドでコンポーネント生成の為利用します。
        /// </remarks>
        protected virtual void refleshDbComponent()
        {
            if ( this.adapter == null )
            {
                this.adapter = new SqlDataAdapter( baseTableSelectSql, this.DbConnection );
                adapter.SelectCommand.Connection = this.DbConnection;
            }
            if ( this.builder == null )
            {
                this.builder = new SqlCommandBuilder( this.adapter );
            }
        }

        /// <summary>
        /// テーブル書込み
        /// </summary>
        /// <remarks>
        /// 保持しているテーブルデータをDBへ書き込みます。
        /// 失敗する場合はDBからの読込を行います。
        /// </remarks>
        protected void tryTableUpdate()
        {
            // この関数はテーブルアクセスの排他を取得してから呼ばれる為、排他権取得は不要

            // 書込みを試行し、失敗するようであれば読込を試行する。
            // 書込みに失敗した場合、既存のデータが読込まれる。
            // 読込みに失敗した場合、データテーブルはクリアされる。
            try
            {
                Int32 cnt = this.adapter.Update( this.dataTable );
            }
            catch ( Exception ex1 )
            {
                try
                {
                    this.dataTable = new DataTable();
                    this.adapter.Fill( this.dataTable );
                    //テーブルデータの書込み動作が失敗しました 保持しているテーブルデータには既存のデータが読込まれました。
                    Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, String.Format( "更新数据表失败。 {0}:{1}", ex1.Message, ex1.InnerException.Message ) );
                }
                catch ( Exception ex2 )
                {
                    //テーブルデータの書込み動作が失敗しました 保持しているテーブルデータはクリアされました
                    Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, String.Format( "写入数据表失败,数据表已被清空。{0}:{1}", ex2.Message, ex2.InnerException.Message ) );
                }
            }
        }

        /// <summary>
        /// SQL処理
        /// </summary>
        /// <remarks>
        /// SQLを処理します
        /// </remarks>
        protected override void sqlThreadFunction()
        {
            // この関数は別スレッドから呼び出される。

            // SQLを処理する。
            base.sqlThreadFunction();

            // DataTableの書込みを処理する。
            this.refleshDbComponent();

            // ベースのテーブル読書きを行う
            try
            {

                // コネクションオープン
                connect();
                //this.m_sqlCommand.Connection.Open();
                switch ( this.transactKind )
                {
                case TableTransactKind.Read:
                    this.dataTable = new DataTable();
                    this.adapter.FillSchema( this.dataTable, SchemaType.Mapped );
                    this.adapter.Fill( this.dataTable );
                    this.tableTransacted.Set();

                    break;
                case TableTransactKind.Write:
                    this.tryTableUpdate();
                    this.tableTransacted.Set();

                    break;
                default:
                    break;
                }

                // コネクションクローズ
                disConnect();
            }
            catch ( Exception ex )
            {
                // テーブルが無効な場合に発生。通常は発生し得ない
                System.Diagnostics.Debug.WriteLine( String.Format( "{0} {1}", ex.Message, ex.StackTrace ) );
                Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, String.Format( "{0} {1}", ex.Message, ex.StackTrace ) );
            }
            finally
            {
                this.transactKind = TableTransactKind.None;
            }

        }

        #endregion
        
        #region [privateメソッド]

        /// <summary>
        /// キュー処理完了イベント登録
        /// </summary>
        /// <remarks>
        /// キュー処理完了イベントを登録します。
        /// </remarks>
        private void addCompleteEvent()
        {
            SQLServerDBAccess.OnCompleteDBQueue += new CompleteDBQueueHandler( this.onCompleteSQLServerDBAccess );
        }

        /// <summary>
        /// キュー処理完了イベント削除
        /// </summary>
        /// <remarks>
        /// キュー処理完了イベントを削除します。
        /// </remarks>
        private void removeCompleteEvent()
        {
            SQLServerDBAccess.OnCompleteDBQueue -= new CompleteDBQueueHandler( this.onCompleteSQLServerDBAccess );
        }

        /// <summary>
        /// キュー完了時処理
        /// </summary>
        /// <remarks>
        /// 送信したキューの処理が完了したときに呼び出される
        /// </remarks>
        /// <param name="source"></param>
        /// <param name="e">
        /// </param>
        private void onCompleteSQLServerDBAccess( object source, DBQueueEventArgs e )
        {
            // TODO:Debug
            Console.WriteLine( String.Format( "onCompleteSQLServerDBAccess：Get DBQueue process complete. Key:{0}", e.Key ) );

            if ( !String.IsNullOrEmpty( e.Key ) )
            {
                // SQL実行結果を取得
                this.m_keys[e.Key] = e.Result;
                // 同期させる
                this.m_manualEvent.Set();
                // イベント削除
                this.removeCompleteEvent();
            }

            // 利用者へ完了を通知するイベントを発行
            if ( this.OnCompleteDBAccess != null )
            {
                this.OnCompleteDBAccess( this, e );
            }

        }

        /// <summary>
        /// SQLを実行する前の準備処理
        /// </summary>
        /// <remarks>
        /// 結果取得時に識別する為のKeyを作成し、完了通知を受け取る為の準備を行う
        /// </remarks>
        /// <param name="key">結果を取得する為のKey</param>
        private void preExecute( String key )
        {
            this.baseTableMutex.WaitOne();
            // 結果を取得する為のキーを登録
            if ( !this.m_keys.ContainsKey( key ) )
            {
                this.m_keys.Add( key, null );
            }
            // 完了通知イベントを受け取る
            this.addCompleteEvent();
            // 非シグナル状態にする
            this.m_manualEvent.Reset();
            this.baseTableMutex.ReleaseMutex();
        }

        /// <summary>
        /// クエリ実行
        /// </summary>
        /// <remarks>
        /// SELECT文を実行し結果をテーブルとして返します。
        /// </remarks>
        /// <param name="strSql"></param>
        /// <returns>
        /// 検索結果のDataTable。失敗時はNULL
        /// </returns>
        protected DataTable OpenQuery( String strSql )
        {
            DataTable dt;
            try
            {
                String key = this.GetKey();
                this.preExecute( key );

                // SQLキュー追加依頼
                this.OpenQuery( strSql, key );

                // 完了待機
                if ( !this.m_manualEvent.WaitOne( SQL_WAIT_TIME ) )
                {
                    Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "DBAccessControl的OpenQuery的結果It was not possible to get.");
                    return null;
                }

                // 結果を取得
                dt = (DataTable)this.m_keys[key];

                this.m_keys.Remove( key );

            }
            catch ( Exception ex )
            {
                System.Diagnostics.Debug.WriteLine( String.Format( "{0} {1}", ex.Message, ex.StackTrace ) );
                Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, ex.Message + " " + ex.StackTrace );
                dt = null;
            }
            return dt;
        }

        /// <summary>
        /// SQL実行
        /// </summary>
        /// <remarks>
        /// Insert/Update/Delete文を実行します。
        /// </remarks>
        /// <param name="strSql"></param>
        /// <returns>影響を受けたレコード数</returns>
        protected Int32 ExecuteSql( String strSql )
        {
            Console.WriteLine( "DBAccessControl:ExecuteSql" );

            Int32 res = -1;
            try
            {
                String key = this.GetKey();
                this.preExecute( key );

                this.ExecuteSql( strSql, key );

                // 完了待機
                if ( !this.m_manualEvent.WaitOne( SQL_WAIT_TIME ) )
                {
                    //DBAccessControlのExecuteSqlの結果を取得できませんでした
                    Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "It was not possible to get the results of ExecuteSql of DBAccessControl.");
                    return -1;
                }

                Console.WriteLine( "DBAccessControl:ExecuteSql Completed" );

                res = this.m_keys[key] != null ? (Int32)this.m_keys[key] : 0;

                this.m_keys.Remove( key );

            }
            catch ( Exception ex )
            {
                System.Diagnostics.Debug.WriteLine( String.Format( "{0} {1}", ex.Message, ex.StackTrace ) );
                Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, ex.Message + " " + ex.StackTrace );
                res = -1;
            }
            return res;
        }


        #endregion

    }
}
