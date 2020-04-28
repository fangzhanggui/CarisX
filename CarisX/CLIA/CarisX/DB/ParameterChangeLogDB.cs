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
    /// パラメータ変更履歴データクラス
    /// </summary>
    /// <remarks>
    /// パラメータ変更履歴データクラスです。
    /// </remarks>
    public class ParameterChangeLogData : DataRowWrapperBase
    {

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data"></param>
        public ParameterChangeLogData( DataRowWrapperBase data )
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
                return this.Field<Int32>( ParameterChangeLogDB.STRING_LOGID );
            }
        }
        /// <summary>
        /// 日時の取得、設定
        /// </summary>
        public DateTime WriteTime
        {
            get
            {
                return this.Field<DateTime>( ParameterChangeLogDB.STRING_WRITETIME );
            }
            protected set
            {
                this.SetField<DateTime>( ParameterChangeLogDB.STRING_WRITETIME, value );
            }
        }
        /// <summary>
        /// ユーザーIDの取得、設定
        /// </summary>
        public String UserID
        {
            get
            {
                return this.Field<String>( ParameterChangeLogDB.STRING_USERID );
            }
            protected set
            {
                this.SetField<String>( ParameterChangeLogDB.STRING_USERID, value );
            }
        }
        /// <summary>
        /// コンテンツ1の取得、設定
        /// </summary>
        public String Contents1
        {
            get
            {
                return this.Field<String>( ParameterChangeLogDB.STRING_CONTENTS1 );
            }
            protected set
            {
                this.SetField<String>( ParameterChangeLogDB.STRING_CONTENTS1, value );
            }
        }
        /// <summary>
        /// コンテンツ2の取得、設定
        /// </summary>
        public String Contents2
        {
            get
            {
                return this.Field<String>( ParameterChangeLogDB.STRING_CONTENTS2 );
            }
            protected set
            {
                this.SetField<String>( ParameterChangeLogDB.STRING_CONTENTS2, value );
            }
        }
        /// <summary>
        /// コンテンツ3の取得、設定
        /// </summary>
        public String Contents3
        {
            get
            {
                return this.Field<String>( ParameterChangeLogDB.STRING_CONTENTS3 );
            }
            protected set
            {
                this.SetField<String>( ParameterChangeLogDB.STRING_CONTENTS3, value );
            }
        }
        /// <summary>
        /// コンテンツ4の取得、設定
        /// </summary>
        public String Contents4
        {
            get
            {
                return this.Field<String>( ParameterChangeLogDB.STRING_CONTENTS4 );
            }
            protected set
            {
                this.SetField<String>( ParameterChangeLogDB.STRING_CONTENTS4, value );
            }
        }
        #endregion

        #region [publicメソッド]
        /// <summary>
        /// ログIDの取得
        /// </summary>
        public Int32 GetLogID()
        {
            return this.LogID;
        }

        #endregion

        #region [内部クラス]
        /// <summary>
        /// 列キー(グリッド向け列キー設定用)
        /// </summary>
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
            /// コンテンツ2
            /// </summary>
            public const String Contents2 = "Contents2";
            /// <summary>
            /// コンテンツ3
            /// </summary>
            public const String Contents3 = "Contents3";
            /// <summary>
            /// コンテンツ4
            /// </summary>
            public const String Contents4 = "Contents4";
            /// <summary>
            /// ログID
            /// </summary>
            public const String LogID = "LogID";
        }
        #endregion
    }

    /// <summary>
    /// パラメータ変更履歴DBクラス
    /// </summary>
    public class ParameterChangeLogDB : DBAccessControl
    {
        #region [定数定義]

        /// <summary>
        /// ログID(DBテーブル：paramChangeLog列名)
        /// </summary>
        public const String STRING_LOGID = "logID";
        /// <summary>
        /// ユーザーID(DBテーブル：paramChangeLog列名)
        /// </summary>
        public const String STRING_USERID = "userID";
        /// <summary>
        /// 日時(DBテーブル：paramChangeLog列名)
        /// </summary>
        public const String STRING_WRITETIME = "writeTime";
        /// <summary>
        /// コンテンツ1(DBテーブル：paramChangeLog列名)
        /// </summary>
        public const String STRING_CONTENTS1 = "contents1";
        /// <summary>
        /// コンテンツ2(DBテーブル：paramChangeLog列名)
        /// </summary>
        public const String STRING_CONTENTS2 = "contents2";
        /// <summary>
        /// コンテンツ3(DBテーブル：paramChangeLog列名)
        /// </summary>
        public const String STRING_CONTENTS3 = "contents3";
        /// <summary>
        /// コンテンツ4(DBテーブル：paramChangeLog列名)
        /// </summary>
        public const String STRING_CONTENTS4 = "contents4";

        #endregion

        #region [プロパティ]

        /// <summary>
        /// データテーブル取得SQL
        /// </summary>
        protected override String baseTableSelectSql
        {
            get
            {
                return "SELECT * FROM dbo.parameterChangeLog";
            }
        }

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ParameterChangeLogDB()
            : base( CarisXConst.MAX_LOG_COUNT )
        {
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// パラメータ変更ログ取得
        /// </summary>
        /// <remarks>
        /// 取得済みのデータテーブルから、パラメータ変更ログを抽出します。
        /// </remarks>
        /// <returns>パラメータ変更ログリスト</returns>
        public List<ParameterChangeLogData> GetParameterChangeLog()
        {
            if ( this.DataTable != null )
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    return dataTableList.Select(row => new ParameterChangeLogData(row))
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
            return new List<ParameterChangeLogData>();
        }

        /// <summary>
        /// パラメータ変更ログテーブル取得
        /// </summary>
        /// <remarks>
        /// パラメータ変更ログをDBから読込みます。
        /// </remarks>
        public override void LoadDB()
        {
            this.fillBaseTable();
        }

        /// <summary>
        /// パラメータ変更ログテーブル書込み
        /// </summary>
        /// <remarks>
        /// パラメータ変更ログをDBに書き込みます。
        /// </remarks>
        public void CommitParameterChangeLog()
        {
            this.updateBaseTable();
        }

        #endregion
    }
}
