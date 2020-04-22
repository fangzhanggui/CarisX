using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Log;
using Oelco.Common.DB;
using Oelco.Common.Utility;
using System.Data.SqlClient;
using Oelco.CarisX.DB;

namespace Oelco.CarisX.Log
{
    /// <summary>
    /// ログ出力クラス
    /// </summary>
    public class CarisXLogWriter : LogWriter
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CarisXLogWriter()
        {
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// ログ出力
        /// </summary>
        /// <remarks>
        /// ログ出力します
        /// </remarks>
        /// <param name="info"></param>
        public void WriteDB( CarisXLogInfo info )
        {
            Singleton<DBAccessControl>.Instance.ExecuteSql( info.SqlInsertString(), String.Empty );
        }

        /// <summary>
        /// ログ出力
        /// </summary>
        /// <remarks>
        /// ログ出力します
        /// </remarks>
        /// <param name="info"></param>
        public override void WriteDB( LogInfo info )
        {
            // HACK : 古いレコードを削除する機能が必要か？
            // HACK : プロジェクトのDBに合わせてSQLは修正すること

            if ( info is CarisXLogInfo )
            {
                this.WriteDB( info as CarisXLogInfo );
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Log write failure: WriteDB argument is not a CarisXLogInfo");
                Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                              "Log write failure: WriteDB argument is not a CarisXLogInfo");
            }
        }

        #endregion

    }

    /// <summary>
    /// CarisXログ情報拡張クラス
    /// </summary>
    internal static class CarisXLogInfoExtension
    {
        #region [publicメソッド]

        /// <summary>
        /// ログ追加SQL文取得
        /// </summary>
        /// <remarks>
        /// ログ追加SQL文取得します
        /// </remarks>
        /// <param name="info">CarisXログ情報</param>
        /// <returns>ログ追加SQL文</returns>
        public static String SqlInsertString( this CarisXLogInfo info )
        {
            String sql = String.Empty;

            // 追加オプション(エラー履歴時)
            if ( info.OptionalValue != null && info.OptionalValue is CarisXLogInfoErrorLogExtention )
            {
                sql = errorLogSqlBuilder( info, info.OptionalValue as CarisXLogInfoErrorLogExtention );
            }
            else
            {
                sql = commonLogSqlBuilder( info );
            }

            return sql;
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// テーブル名取得
        /// </summary>
        /// <remarks>
        /// テーブル名取得します
        /// </remarks>
        /// <param name="kind">ログ種別</param>
        /// <returns>テーブル名</returns>
        private static String dbTableName( this LogKind kind )
        {
            String dbName = String.Empty;

            switch ( kind )
            {
                case LogKind.ErrorHist:
                    dbName = "errorLog";
                    break;
                case LogKind.OperationHist:
                    dbName = "operationLog";
                    break;
                case LogKind.AnalyseHist:
                    dbName = "analyzeLog";
                    break;
                case LogKind.ParamChangeHist:
                    dbName = "parameterChangeLog";
                    break;
                case LogKind.MaintenanceLogin:
                    dbName = "maintenanceLoginLog";
                    break;
                case LogKind.MasterErrorHist:
                    dbName = "masterErrorLog";
                    break;
                default:
                    break;
            }

            return dbName;
        }

        /// <summary>
        /// ログ件数上限取得
        /// </summary>
        /// <remarks>
        /// ログ件数上限取得します
        /// </remarks>
        /// <param name="kind">ログ情報</param>
        /// <returns></returns>
        private static Int32 dbRecordMax( this LogKind kind )
        {
            Int32 recordMax = Int32.MaxValue;

            switch ( kind )
            {
            case LogKind.ErrorHist:
                recordMax = Singleton<ErrorLogDB>.Instance.RecordCountMax;
                break;
            case LogKind.OperationHist:
                recordMax = Singleton<OperationLogDB>.Instance.RecordCountMax;
                break;
            case LogKind.AnalyseHist:
                recordMax = Singleton<AnalyzeLogDB>.Instance.RecordCountMax;
                break;
            case LogKind.ParamChangeHist:
                recordMax = Singleton<ParameterChangeLogDB>.Instance.RecordCountMax;
                break;
            default:
                break;
            }

            return recordMax;
        }

        /// <summary>
        /// エラー履歴追加用SQL文作成
        /// </summary>
        /// <remarks>
        /// エラー履歴追加用SQL文作成します
        /// </remarks>
        /// <param name="info">CarisXログ情報</param>
        /// <param name="ex">追加オプション</param>
        /// <returns>エラー履歴追加用SQL文</returns>
        private static String errorLogSqlBuilder( CarisXLogInfo info, CarisXLogInfoErrorLogExtention ex )
        {
            // SQL作成
            StringBuilder strSql = new StringBuilder();

            strSql.Append( " INSERT INTO " );
            strSql.Append( info.Kind.dbTableName() );
            strSql.Append( " (writeTime, userID" );
            switch (info.Kind)
            {
                case LogKind.ErrorHist:
                case LogKind.AnalyseHist:
                case LogKind.MasterErrorHist:
                    strSql.Append(", moduleNo");
                    break;
            }
            for ( Int32 i = 0; i < info.Contents.Count; i++ )
            {
                strSql.Append( ", contents" );
                strSql.Append( ( i + 1 ).ToString() );
            }
            strSql.Append(", errorCode, errorArg, counter");
            strSql.Append( ") VALUES ( " );

            /// エラーフィルタリング時はログ記述時間ではなく発生時間をsql文に入れる
            /// エラーフィルタリングを行わないエラー時はex.WriteTimeが初期値のため下記の条件式になる
            // エラー履歴かつ、エラー履歴追加オプションの発生時刻が初期値以外の場合
            if (info.Kind == LogKind.ErrorHist && ex.WriteTime != new DateTime())
            {
                strSql.AppendFormat(@"'{0}'", ex.WriteTime);
            }
            else
            {
                strSql.AppendFormat(@"'{0}'", info.WriteDateTime);
            }
            
            strSql.AppendFormat( @",N'{0}'", info.UserId );
            switch (info.Kind)
            {
                case LogKind.ErrorHist:
                case LogKind.AnalyseHist:
                case LogKind.MasterErrorHist:
                    strSql.AppendFormat(@", {0}", info.ModuleNo);
                    break;
            }
            foreach ( String val in info.Contents )
            {
                strSql.AppendFormat( @",N'{0}'", val );
            }
            strSql.AppendFormat(@",'{0}', '{1}', '{2}'", ex.ErrorCode, ex.ErrorArg, ex.Counter);
            strSql.Append( ");" );

            strSql.AppendFormat( @"DELETE top(select case when count (logID) > {1} then count(logID) - {1} else 0 end from {0}) from {0}", info.Kind.dbTableName(), info.Kind.dbRecordMax() );
                
            return strSql.ToString();
        }

        /// <summary>
        /// 履歴追加用既定SQL文作成
        /// </summary>
        /// <remarks>
        /// 履歴追加用既定SQL文作成します
        /// </remarks>
        /// <param name="info">CarisXログ情報</param>
        /// <returns>履歴追加用既定SQL文</returns>
        private static String commonLogSqlBuilder( CarisXLogInfo info )
        {
            // SQL作成
            StringBuilder strSql = new StringBuilder();

            strSql.Append( " INSERT INTO " );
            strSql.Append( info.Kind.dbTableName() );
            strSql.Append( " (writeTime, userID" );
            switch (info.Kind)
            {
                case LogKind.ErrorHist:
                case LogKind.AnalyseHist:
                case LogKind.MasterErrorHist:
                    strSql.Append(", moduleNo");
                    break;
            }
            for ( Int32 i = 0; i < info.Contents.Count; i++ )
            {
                strSql.Append( ", contents" );
                strSql.Append( ( i + 1 ).ToString() );
            }
            strSql.Append( ") VALUES ( " );
            strSql.AppendFormat( @"'{0}'", info.WriteDateTime );
            strSql.AppendFormat( @",N'{0}'", info.UserId );
            switch (info.Kind)
            {
                case LogKind.ErrorHist:
                case LogKind.AnalyseHist:
                case LogKind.MasterErrorHist:
                    strSql.AppendFormat(@", {0}", info.ModuleNo);
                    break;
            }
            foreach ( String val in info.Contents )
            {
                strSql.AppendFormat( @",N'{0}'", val );
            }
            strSql.Append( ");" );

            strSql.AppendFormat( @"DELETE top(select case when count (logID) > {1} then count(logID) - {1} else 0 end from {0}) from {0}", info.Kind.dbTableName(), info.Kind.dbRecordMax() );

            return strSql.ToString();
        }

        #endregion

    }
}
