using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.DB;
using Oelco.Common.Utility;
using Oelco.Common.Log;
using System.Collections;
using System.Data;
using Oelco.CarisX.Log;
using Oelco.CarisX.Const;
using Oelco.CarisX.Utility;

namespace Oelco.CarisX.DB
{
    /// <summary>
    /// 分析履歴データクラス
    /// </summary>
    /// <remarks>
    /// 分析履歴データクラスです。
    /// </remarks>
    public class AnalyzeLogData : DataRowWrapperBase
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data"></param>
        public AnalyzeLogData( DataRowWrapperBase data )
            : base( data )
        {
        }
        #endregion

        #region [プロパティ]
        /// <summary>
        /// ログIDの取得、設定
        /// </summary>
        protected Int32 LogID
        {
            get
            {
                return this.Field<Int32>( AnalyzeLogDB.STRING_LOGID );
            }
            set
            {
                this.SetField<Int32>( AnalyzeLogDB.STRING_LOGID, value );
            }
        }
        /// <summary>
        /// 日時の取得、設定
        /// </summary>
        public DateTime WriteTime
        {
            get
            {
                return this.Field<DateTime>( AnalyzeLogDB.STRING_WRITETIME );
            }
            protected set
            {
                this.SetField<DateTime>( AnalyzeLogDB.STRING_WRITETIME, value );
            }
        }
        /// <summary>
        /// ユーザーIDの取得、設定
        /// </summary>
        public String UserID
        {
            get
            {
                return this.Field<String>( AnalyzeLogDB.STRING_USERID );
            }
            protected set
            {
                this.SetField<String>( AnalyzeLogDB.STRING_USERID, value );
            }
        }
        /// <summary>
        /// 分析モジュール番号の取得、設定
        /// </summary>
        public Int32 ModuleNo
        {
            get
            {
                return this.Field<Int32>(AnalyzeLogDB.STRING_MODULENO);
            }
            protected set
            {
                this.SetField<Int32>(AnalyzeLogDB.STRING_MODULENO, value);
            }
        }
        /// <summary>
        /// コンテンツ1の取得、設定
        /// </summary>
        public String Contents1
        {
            get
            {
                return this.Field<String>( AnalyzeLogDB.STRING_CONTENTS1 );
            }
            protected set
            {
                this.SetField<String>( AnalyzeLogDB.STRING_CONTENTS1, value );
            }
        }
        /// <summary>
        /// コンテンツ2の取得、設定
        /// </summary>
        public String Contents2
        {
            get
            {
                return this.Field<String>( AnalyzeLogDB.STRING_CONTENTS2 );
            }
            protected set
            {
                this.SetField<String>( AnalyzeLogDB.STRING_CONTENTS2, value );
            }
        }
        /// <summary>
        /// コンテンツ3の取得、設定
        /// </summary>
        public String Contents3
        {
            get
            {
                return this.Field<String>( AnalyzeLogDB.STRING_CONTENTS3 );
            }
            protected set
            {
                this.SetField<String>( AnalyzeLogDB.STRING_CONTENTS3, value );
            }
        }
        /// <summary>
        /// コンテンツ4の取得、設定
        /// </summary>
        public String Contents4
        {
            get
            {
                return this.Field<String>( AnalyzeLogDB.STRING_CONTENTS4 );
            }
            protected set
            {
                this.SetField<String>( AnalyzeLogDB.STRING_CONTENTS4, value );
            }
        }
        /// <summary>
        /// コンテンツ5の取得、設定
        /// </summary>
        public String Contents5
        {
            get
            {
                return this.Field<String>( AnalyzeLogDB.STRING_CONTENTS5 );
            }
            protected set
            {
                this.SetField<String>( AnalyzeLogDB.STRING_CONTENTS5, value );
            }
        }
        /// <summary>
        /// コンテンツ6の取得、設定
        /// </summary>
        public String Contents6
        {
            get
            {
                return this.Field<String>( AnalyzeLogDB.STRING_CONTENTS6 );
            }
            protected set
            {
                this.SetField<String>( AnalyzeLogDB.STRING_CONTENTS6, value );
            }
        }
        /// <summary>
        /// コンテンツ7の取得、設定
        /// </summary>
        public String Contents7
        {
            get
            {
                return this.Field<String>( AnalyzeLogDB.STRING_CONTENTS7 );
            }
            protected set
            {
                this.SetField<String>( AnalyzeLogDB.STRING_CONTENTS7, value );
            }
        }
        /// <summary>
        /// コンテンツ8の取得、設定
        /// </summary>
        public String Contents8
        {
            get
            {
                return this.Field<String>( AnalyzeLogDB.STRING_CONTENTS8 );
            }
            protected set
            {
                this.SetField<String>( AnalyzeLogDB.STRING_CONTENTS8, value );
            }
        }
        /// <summary>
        /// コンテンツ9の取得、設定
        /// </summary>
        public String Contents9
        {
            get
            {
                return this.Field<String>( AnalyzeLogDB.STRING_CONTENTS9 );
            }
            protected set
            {
                this.SetField<String>( AnalyzeLogDB.STRING_CONTENTS9, value );
            }
        }
        /// <summary>
        /// コンテンツ10の取得、設定
        /// </summary>
        public String Contents10
        {
            get
            {
                return this.Field<String>( AnalyzeLogDB.STRING_CONTENTS10 );
            }
            protected set
            {
                this.SetField<String>( AnalyzeLogDB.STRING_CONTENTS10, value );
            }
        }

        /// <summary>
        /// モジュール名
        /// </summary>
        public String ModuleName
        {
            get
            {
                String moduleName = String.Empty;

                Int32 moduleNo = this.Field<Int32>( AnalyzeLogDB.STRING_MODULENO );

                moduleName = CarisXSubFunction.ModuleIdToModuleName(moduleNo);

                return moduleName;
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
        /// <summary>
        /// 日時の設定
        /// </summary>
        /// <remarks>
        /// 日時を設定します。
        /// </remarks>
        public void SetWriteTime( DateTime writeTime )
        {
            this.WriteTime = writeTime;
        }
        /// <summary>
        /// ユーザーIDの設定
        /// </summary>
        /// <remarks>
        /// ユーザIDを設定します。
        /// </remarks>
        public void SetUserID( String userID )
        {
            this.UserID = userID;
        }
        /// <summary>
        /// 分析モジュール番号の設定
        /// </summary>
        /// <remarks>
        /// 分析モジュール番号を設定します。
        /// </remarks>
        public void SetModuleNo(Int32 ModuleNo)
        {
            this.ModuleNo = ModuleNo;
        }
        /// <summary>
        /// コンテンツ1の設定
        /// </summary>
        /// <remarks>
        /// コンテンツ1を設定します。
        /// </remarks>
        public void SetContents1( String contents1 )
        {
            this.Contents1 = contents1;
        }
        /// <summary>
        /// コンテンツ2の設定
        /// </summary>
        /// <remarks>
        /// コンテンツ2を設定します。
        /// </remarks>
        public void SetContents2( String contents2 )
        {
            this.Contents2 = contents2;
        }
        /// <summary>
        /// コンテンツ3の設定
        /// </summary>
        /// <remarks>
        /// コンテンツ3を設定します。
        /// </remarks>
        public void SetContents3( String contents3 )
        {
            this.Contents3 = contents3;
        }
        /// <summary>
        /// コンテンツ4の設定
        /// </summary>
        /// <remarks>
        /// コンテンツ4を設定します。
        /// </remarks>
        public void SetContents4( String contents4 )
        {
            this.Contents4 = contents4;
        }
        /// <summary>
        /// コンテンツ5の設定
        /// </summary>
        /// <remarks>
        /// コンテンツ5を設定します。
        /// </remarks>
        public void SetContents5( String contents5 )
        {
            this.Contents5 = contents5;
        }
        /// <summary>
        /// コンテンツ6の設定
        /// </summary>
        /// <remarks>
        /// コンテンツ6を設定します。
        /// </remarks>
        public void SetContents6( String contents6 )
        {
            this.Contents6 = contents6;
        }
        /// <summary>
        /// コンテンツ7の設定
        /// </summary>
        /// <remarks>
        /// コンテンツ7を設定します。
        /// </remarks>
        public void SetContents7( String contents7 )
        {
            this.Contents7 = contents7;
        }
        /// <summary>
        /// コンテンツ8の設定
        /// </summary>
        /// <remarks>
        /// コンテンツ8を設定します。
        /// </remarks>
        public void SetContents8( String contents8 )
        {
            this.Contents8 = contents8;
        }
        /// <summary>
        /// コンテンツ9の設定
        /// </summary>
        /// <remarks>
        /// コンテンツ9を設定します。
        /// </remarks>
        public void SetContents9( String contents9 )
        {
            this.Contents9 = contents9;
        }
        /// <summary>
        /// コンテンツ10の設定
        /// </summary>
        /// <remarks>
        /// コンテンツ10を設定します。
        /// </remarks>
        public void SetContents10( String contents10 )
        {
            this.Contents10 = contents10;
        }

        #endregion

        #region [内部クラス]
        /// <summary>
        /// 列キー
        /// </summary>
        /// <remarks>
        /// グリッド向け列キー設定用クラスです。
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
            /// モジュール番号
            /// </summary>
            public const String ModuleNo = "ModuleNo";
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
            /// コンテンツ5
            /// </summary>
            public const String Contents5 = "Contents5";
            /// <summary>
            /// コンテンツ6
            /// </summary>
            public const String Contents6 = "Contents6";
            /// <summary>
            /// コンテンツ7
            /// </summary>
            public const String Contents7 = "Contents7";
            /// <summary>
            /// コンテンツ8
            /// </summary>
            public const String Contents8 = "Contents8";
            /// <summary>
            /// コンテンツ9
            /// </summary>
            public const String Contents9 = "Contents9";
            /// <summary>
            /// コンテンツ10
            /// </summary>
            public const String Contents10 = "Contents10";
            /// <summary>
            /// モジュール名
            /// </summary>
            public const String ModuleName = "ModuleName";
        }
        #endregion
    }

    /// <summary>
    /// 分析履歴DBクラス
    /// </summary>
    /// <remarks>
    /// 分析履歴のアクセスを行うクラスです。
    /// </remarks>
    public class AnalyzeLogDB : DBAccessControl
    {
        #region [定数定義]

        /// <summary>
        /// ログID(DBテーブル：analyzeLog列名)
        /// </summary>
        public const String STRING_LOGID = "logID";
        /// <summary>
        /// 日時(DBテーブル：analyzeLog列名)
        /// </summary>
        public const String STRING_WRITETIME = "writeTime";
        /// <summary>
        /// ユーザーID(DBテーブル：analyzeLog列名)
        /// </summary>
        public const String STRING_USERID = "userID";
        /// <summary>
        /// 分析モジュール番号(DBテーブル：analyzeLog列名)
        /// </summary>
        public const String STRING_MODULENO = "moduleNo";
        /// <summary>
        /// コンテンツ1(DBテーブル：analyzeLog列名)
        /// </summary>
        public const String STRING_CONTENTS1 = "contents1";
        /// <summary>
        /// コンテンツ2(DBテーブル：analyzeLog列名)
        /// </summary>
        public const String STRING_CONTENTS2 = "contents2";
        /// <summary>
        /// コンテンツ3(DBテーブル：analyzeLog列名)
        /// </summary>
        public const String STRING_CONTENTS3 = "contents3";
        /// <summary>
        /// コンテンツ4(DBテーブル：analyzeLog列名)
        /// </summary>
        public const String STRING_CONTENTS4 = "contents4";
        /// <summary>
        /// コンテンツ5(DBテーブル：analyzeLog列名)
        /// </summary>
        public const String STRING_CONTENTS5 = "contents5";
        /// <summary>
        /// コンテンツ6(DBテーブル：analyzeLog列名)
        /// </summary>
        public const String STRING_CONTENTS6 = "contents6";
        /// <summary>
        /// コンテンツ7(DBテーブル：analyzeLog列名)
        /// </summary>
        public const String STRING_CONTENTS7 = "contents7";
        /// <summary>
        /// コンテンツ8(DBテーブル：analyzeLog列名)
        /// </summary>
        public const String STRING_CONTENTS8 = "contents8";
        /// <summary>
        /// コンテンツ9(DBテーブル：analyzeLog列名)
        /// </summary>
        public const String STRING_CONTENTS9 = "contents9";
        /// <summary>
        /// コンテンツ10(DBテーブル：analyzeLog列名)
        /// </summary>
        public const String STRING_CONTENTS10 = "contents10";
        
        #endregion

        #region [プロパティ]

        /// <summary>
        /// データテーブル取得SQL
        /// </summary>
        protected override String baseTableSelectSql
        {
            get
            {
                return "SELECT * FROM dbo.analyzeLog";
            }
        }

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AnalyzeLogDB()
            : base( CarisXConst.MAX_LOG_COUNT )
        {
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 分析ログ取得
        /// </summary>
        /// <remarks>
        /// 取得済みのデータテーブルから、分析ログを抽出します。
        /// </remarks>
        /// <returns>分析ログリスト</returns>
        public List<AnalyzeLogData> GetAnalyzeLog()
        {
            if ( this.DataTable != null )
            {
                try
                {
                    return this.DataTable.Copy().AsEnumerable().Select( ( row ) => new AnalyzeLogData( row ) ).OrderByDescending( ( data ) => data.WriteTime ).ToList();
                }
                catch ( Exception ex )
                {
                    // DB内部に不正データ
                    //Singleton<LogManager>.Instance.Error( ex.Message );
                    Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
                }
            }
            return new List<AnalyzeLogData>();
        }

        /// <summary>
        /// 分析ログ追加
        /// </summary>
        /// <remarks>
        /// 分析ログをデータテーブルに追加します。
        /// </remarks>
        /// <param name="anaLog">分析ログ</param>
        public void AddAnalyzeLog( String userID, DateTime writeTime, Int32 moduleNo, String contents1, String contents2, String contents3, String contents4, String contents5, String contents6, String contents7, String contents8, String contents9, String contents10 )
        {
            if ( this.DataTable != null )
            {
                var data = new AnalyzeLogData( this.DataTable.NewRow() );

                data.SetUserID( userID );
                data.SetWriteTime( writeTime );
                data.SetModuleNo( moduleNo );
                data.SetContents1( contents1 );
                data.SetContents2( contents2 );
                data.SetContents3( contents3 );
                data.SetContents4( contents4 );
                data.SetContents5( contents5 );
                data.SetContents6( contents6 );
                data.SetContents7( contents7 );
                data.SetContents8( contents8 );
                data.SetContents9( contents9 );
                data.SetContents10( contents10 );

                new List<AnalyzeLogData>() { data }.SyncDataListToDataTable( this.DataTable );
            }
        }

        /// <summary>
        /// 分析ログテーブル取得
        /// </summary>
        /// <remarks>
        /// 分析ログをDBから読込みます。
        /// </remarks>
        public override void LoadDB()
        {
            this.fillBaseTable();
        }

        /// <summary>
        /// 分析ログテーブル書込み
        /// </summary>
        /// <remarks>
        /// 分析ログをDBに書き込みます。
        /// </remarks>
        public void CommitAnalyzeLog()
        {
            this.updateBaseTable();
        }

        #endregion

    }
}
