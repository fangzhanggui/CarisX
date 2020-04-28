using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.DB;
using Oelco.Common.Utility;
using Oelco.Common.Log;
using System.Data;
using Oelco.CarisX.Log;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.DB
{
    /// <summary>
    /// 操作履歴データクラス
    /// </summary>
    public class OperationLogData : DataRowWrapperBase
    {
        #region [コンストラクタ/デストラクタ]
        public OperationLogData( DataRowWrapperBase data )
            : base( data )
        {
        }
        #endregion

        #region [プロパティ]
        /// <summary>
        /// ログIDの取得、設定
        /// </summary>
        public Int32 LogID
        {
            get
            {
                return this.Field<Int32>( OperationLogDB.STRING_LOGID );
            }
        }
        /// <summary>
        /// 日時の取得、設定
        /// </summary>
        public DateTime WriteTime
        {
            get
            {
                return this.Field<DateTime>( OperationLogDB.STRING_WRITETIME );
            }
            protected set
            {
                this.SetField<DateTime>( OperationLogDB.STRING_WRITETIME, value );
            }
        }
        /// <summary>
        /// ユーザーIDの取得、設定
        /// </summary>
        public String UserID
        {
            get
            {
                return this.Field<String>( OperationLogDB.STRING_USERID );
            }
            protected set
            {
                this.SetField<String>( OperationLogDB.STRING_USERID, value );
            }
        }
        /// <summary>
        /// コンテンツ1の取得、設定
        /// </summary>
        public String Contents1
        {
            get
            {
                return this.Field<String>( OperationLogDB.STRING_CONTENTS1 );
            }
            protected set
            {
                this.SetField<String>( OperationLogDB.STRING_CONTENTS1, value );
            }
        }
        #endregion

        #region [publicメソッド]
        /// <summary>
        /// ログIDの取得
        /// </summary>
        /// <remarks>
        /// ログIDを取得します。
        /// </remarks>
        public Int32 GetLogID()
        {
            return this.LogID;
        }       
        #endregion

        #region [内部クラス]
        /// <summary>
        /// 列キー
        /// </summary>
        /// <remarks>
        /// グリッド向け列キー設定用
        /// </remarks>
        public struct DataKeys
        {
            /// <summary>
            /// 日時
            /// </summary>
            public const String WriteTime = "WriteTime";
            /// <summary>
            /// ユーザーID
            /// </summary>
            public const String UserID = "UserID";
            /// <summary>
            /// コンテンツ1
            /// </summary>
            public const String Contents1 = "Contents1";
            /// <summary>
            /// ログID
            /// </summary>
            public const String LogID = "LogID";
        }
        #endregion
    }

    /// <summary>
    /// 操作履歴DBクラス
    /// </summary>
    /// <remarks>
    /// 操作履歴DBのアクセスを行うクラスです。
    /// </remarks>
    public class OperationLogDB : DBAccessControl
    {
        #region [定数定義]

        /// <summary>
        /// ログID(DBテーブル：operationLog列名)
        /// </summary>
        public const String STRING_LOGID = "logID";
        /// <summary>
        /// 日時(DBテーブル：operationLog列名)
        /// </summary>
        public const String STRING_WRITETIME = "writeTime";
        /// <summary>
        /// ユーザーID(DBテーブル：operationLog列名)
        /// </summary>
        public const String STRING_USERID = "userID";
        /// <summary>
        /// コンテンツ1(DBテーブル：operationLog列名)
        /// </summary>
        public const String STRING_CONTENTS1 = "contents1";

        #endregion

        #region [プロパティ]

        /// <summary>
        /// データテーブル取得SQL
        /// </summary>
        protected override String baseTableSelectSql
        {
            get
            {
                return "SELECT * FROM dbo.operationLog";
            }
        }

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OperationLogDB()
            : base( CarisXConst.MAX_LOG_COUNT )
        {
        }
        
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 操作ログ取得
        /// </summary>
        /// <remarks>
        /// 取得済みのデータテーブルから、操作ログを抽出します。
        /// </remarks>
        /// <returns>操作ログリスト</returns>
        public List<OperationLogData> GetOperationLog()
        {
            if ( this.DataTable != null )
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    return dataTableList.Select(row => new OperationLogData(row))
                        .OrderByDescending(data => data.WriteTime).ThenByDescending(data => data.LogID).ToList();
                }
                catch ( Exception ex )
                {
                    // DB内部に不正データ
                    //Singleton<LogManager>.Instance.Error( ex.Message );
                    Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
                }
            }
            return new List<OperationLogData>();
        }      

        /// <summary>
        /// 操作ログテーブル取得
        /// </summary>
        /// <remarks>
        /// 操作ログをDBから読込みます。
        /// </remarks>
        public override void LoadDB()
        {
            this.fillBaseTable();
        }

        /// <summary>
        /// 操作ログテーブル書込み
        /// </summary>
        /// <remarks>
        /// 操作ログをDBに書き込みます。
        /// </remarks>
        public void CommitOperationLog()
        {
            this.updateBaseTable();
        }

        #endregion

    }
}
