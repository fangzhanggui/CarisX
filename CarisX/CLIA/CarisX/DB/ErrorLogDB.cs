using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.DB;
using Oelco.Common.Utility;
using Oelco.Common.Log;
using System.Data;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter.ErrorCodeData;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Log;
using Oelco.CarisX.Const;
using System.ComponentModel;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.GUI.Controls;

namespace Oelco.CarisX.DB
{
    /// <summary>
    /// エラー履歴データクラス
    /// </summary>
    public class ErrorLogData : DataRowWrapperBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data"></param>
        public ErrorLogData( DataRowWrapperBase data )
            : base( data )
        {
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 日時の取得、設定
        /// </summary>
        public DateTime WriteTime
        {
            get
            {
                return this.Field<DateTime>( ErrorLogDB.STRING_WRITETIME );//.ToString();// //( "MM/dd/yyyy HH:mm:ss" );
            }
        }

        /// <summary>
        /// ユーザーIDの取得、設定
        /// </summary>
        public String UserID
        {
            get
            {
                return this.Field<String>( ErrorLogDB.STRING_USERID );
            }
            protected set
            {
                this.SetField<String>( ErrorLogDB.STRING_USERID, value );
            }
        }
        /// <summary>
        /// モジュール番号の取得、設定
        /// </summary>
        public Int32 ModuleNo
        {
            get
            {
                return this.Field<Int32>(ErrorLogDB.STRING_MODULENO);
            }
            set
            {
                this.SetField<Int32>(ErrorLogDB.STRING_MODULENO, value);
            }
        }
        /// <summary>
        /// コンテンツ1の取得、設定
        /// </summary>
        protected String Contents1
        {
            get
            {
                return this.Field<String>( ErrorLogDB.STRING_CONTENTS1 );
            }
            set
            {
                this.SetField<String>( ErrorLogDB.STRING_CONTENTS1, value );
            }
        }

        /// <summary>
        /// エラーコードの取得、設定
        /// </summary>
        protected Int32 ErrorCode
        {
            get
            {
                return this.Field<Int32>( ErrorLogDB.STRING_ERRORCODE );
            }
            set
            {
                this.SetField<Int32>( ErrorLogDB.STRING_ERRORCODE, value );
            }
        }

        /// <summary>
        /// エラー引数の取得、設定
        /// </summary>
        protected Int32 ErrorArg
        {
            get
            {
                return this.Field<Int32>( ErrorLogDB.STRING_ERRORARG );
            }
            set
            {
                this.SetField<Int32>( ErrorLogDB.STRING_ERRORARG, value );
            }
        }

        /// <summary>
        /// エラータイトルの取得
        /// </summary>
        public String ErrorTitle
        {
            get
            {
                ErrorCodeData errorData = Singleton<ParameterFilePreserve<ErrorCodeDataManager>>.Instance.Param.GetCodeData( this.ErrorCode.ToString(), this.ErrorArg.ToString() );

                // エラーデータが取得出来ない場合に空白ではなく、判別可能な範囲で出力する。
                String title = String.Empty;
                if ( errorData == null )
                {

                    title = this.ErrorCode.ToString() + Oelco.CarisX.Properties.Resources.STRING_COMMON_000 + this.ErrorArg.ToString()
                        + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + this.Contents1;
                }
                else
                {
                    title = this.ErrorCode.ToString() + Oelco.CarisX.Properties.Resources.STRING_COMMON_000 + this.ErrorArg.ToString()
                        + Oelco.CarisX.Properties.Resources.STRING_COMMON_013 + errorData.Title + this.Contents1;
                }

                return title;
            }
            set
            {
                this.SetField<String>(ErrorLogDB.STRING_CONTENTS1, value);
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

                Int32 moduleNo = this.Field<Int32>(ErrorLogDB.STRING_MODULENO);

                moduleName = CarisXSubFunction.ModuleIdToModuleName(moduleNo);

                return moduleName;
            }
        }

        /// <summary>
        /// ログIDの取得、設定
        /// </summary>
        public Int32 LogID
        {
            get
            {
                return this.Field<Int32>(ErrorLogDB.STRING_LOGID);
            }
        }


        /// <summary>
        /// エラー連続発生回数の取得、設定
        /// </summary>
        public Int32 Counter
        {
            get
            {
                // エラー発生回数を実装前のエラーはカラム内がnull
                Int32? temp = this.Field<Int32?>(ErrorLogDB.STRING_COUNTER);

                // エラー発生回数がnullの場合
                if (temp == null)
                {
                    temp = 1;
                }
                

                return (Int32)temp;

            }
            set
            {
                this.SetField<Int32?>(ErrorLogDB.STRING_COUNTER, value);
            }
        }

        /// <summary>
        /// エラーレベルの取得
        /// </summary>
        public String ErrorLevel
        {
            get
            {
                String errorLevel = "";
                ErrorLevelKind errorLevelKind = CarisXSubFunction.GetErrorLevel(ErrorCode, ErrorArg);
                switch (errorLevelKind)
                {
                    case ErrorLevelKind.Error:
                        errorLevel = Oelco.CarisX.Properties.Resources.STRING_ERRORLEVEL_1;
                        break;
                    case ErrorLevelKind.Warning:
                        errorLevel = Oelco.CarisX.Properties.Resources.STRING_ERRORLEVEL_2;
                        break;
                    case ErrorLevelKind.Hint:
                        errorLevel = Oelco.CarisX.Properties.Resources.STRING_ERRORLEVEL_3;
                        break;
                }
                return errorLevel;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コンテンツ1の取得
        /// </summary>
        /// <remarks>
        /// コンテンツ1を取得します。
        /// </remarks>
        public String GetContents1()
        {
            return this.Contents1;
        }
        
        /// <summary>
        /// エラーコードの取得
        /// </summary>
        /// <remarks>
        /// エラーコードを取得します。
        /// </remarks>
        public Int32 GetErrorCode()
        {
            return this.ErrorCode;
        }
        
        /// <summary>
        /// エラー引数の取得
        /// </summary>
        /// <remarks>
        /// エラー引数を取得します。
        /// </remarks>
        public Int32 GetErrorArg()
        {
            return this.ErrorArg;
        }

        /// <summary>
        /// モジュール名の取得
        /// </summary>
        /// <remarks>
        /// モジュール名を取得します。
        /// </remarks>
        public String GetModuleName()
        {
            return this.ModuleName;
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
            /// エラータイトル
            /// </summary>
            public const String ErrorTitle = "ErrorTitle";
            /// <summary>
            /// モジュール名
            /// </summary>
            public const String ModuleName = "ModuleName";
            /// <summary>
            /// ログID
            /// </summary>
            public const String LogID = "LogID";
            /// <summary>
            /// エラー連続発生回数
            /// </summary>
            public const String Counter = "counter";
            /// <summary>
            /// エラーレベル
            /// </summary>
            public const String ErrorLevel = "ErrorLevel";
        }

        #endregion

    }


    /// <summary>
    /// エラー履歴DBクラス
    /// </summary>
    /// <remarks>
    /// エラー履歴のアクセスを行うクラスです。
    /// </remarks>
    public class ErrorLogDB : DBAccessControl
    {
        #region [プロパティ]

        /// <summary>
        /// データテーブル取得SQL
        /// </summary>
        protected override String baseTableSelectSql
        {
            get
            {
                return "SELECT * FROM dbo.errorLog";
            }
        }

        #endregion

        #region [定数定義]

        /// <summary>
        /// ログID(DBテーブル：errorLog列名)
        /// </summary>
        public const String STRING_LOGID = "logID";
        /// <summary>
        /// 日時(DBテーブル：errorLog列名)
        /// </summary>
        public const String STRING_WRITETIME = "writeTime";
        /// <summary>
        /// ユーザーID(DBテーブル：errorLog列名)
        /// </summary>
        public const String STRING_USERID = "userID";
        /// <summary>
        /// 分析モジュール番号(DBテーブル：errorLog列名)
        /// </summary>
        public const String STRING_MODULENO = "moduleNo";
        /// <summary>
        /// コンテンツ1(DBテーブル：errorLog列名)
        /// </summary>
        public const String STRING_CONTENTS1 = "contents1";
        /// <summary>
        /// エラーコード(DBテーブル：errorLog列名)
        /// </summary>
        public const String STRING_ERRORCODE = "errorCode";
        /// <summary>
        /// エラー引数(DBテーブル：errorLog列名)
        /// </summary>
        public const String STRING_ERRORARG = "errorArg";
        /// <summary>
        /// エラー連続発生回数(DBテーブル:errorLog列名)
        /// </summary>
        public const String STRING_COUNTER = "counter";

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ErrorLogDB()
            : base( CarisXConst.MAX_LOG_COUNT )
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ErrorLogDB( Int32 maxLogCount )
            : base(maxLogCount)
        {
        }
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// エラーログ取得
        /// </summary>
        /// <remarks>
        /// 取得済みのデータテーブルから、エラーログを抽出します。
        /// </remarks>
        /// <returns>エラーログリスト</returns>
        public List<ErrorLogData> GetErrorLog()
        {
            if ( this.DataTable != null )
            {
                try
                {
                    var logData = this.DataTable.Copy().AsEnumerable().Select(row => new ErrorLogData(row))
                        .OrderByDescending(data => data.WriteTime).ThenByDescending(data => data.LogID).ToList();
                    return logData;
                }
                catch ( Exception ex )
                {
                    // DB内部に不正データ
                    //Singleton<LogManager>.Instance.Error( ex.Message );
                    Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
                }
            }

            return new List<ErrorLogData>();
        }

        /// <summary>
        /// フィルタリングされたエラーログ取得
        /// </summary>
        /// <param name="searchLogInfo">フィルタリング条件</param>
        /// <returns></returns>
        public List<ErrorLogData> GetFilteringErrorLog( ISearchLogInfoErrorLog searchLogInfo = null)
        {
            if ( this.DataTable != null )
            {
                try
                {
                    var logData = this.DataTable.Copy().AsEnumerable().Select(row => new ErrorLogData(row))
                         .Where(data => this.getLogDataWhere(searchLogInfo, data))
                         .OrderByDescending(data => data.WriteTime).ThenByDescending(data => data.LogID).ToList();

                    return logData;
                }
                catch (Exception ex)
                {
                    // DB内部に不正データ
                    //Singleton<LogManager>.Instance.Error( ex.Message );
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                }
            }
            return new List<ErrorLogData>();
        }

        /// <summary>
        /// フィルタリング結果を返す
        /// </summary>
        /// <param name="searchInfo">フィルタリング条件</param>
        /// <param name="data">エラー履歴</param>
        /// <returns>true:合致 false:異なる</returns>
        private Boolean getLogDataWhere( ISearchLogInfoErrorLog searchInfo, ErrorLogData data)
        {
            // フィルタリングフラグ
            // true:フィルタリング条件と合致
            // false:フィルタリング条件と異なる
            Boolean result = true;
            Boolean isServiceman = Singleton<CarisXUserLevelManager>.Instance.AskEnableAction(CarisXUserLevelManagedAction.Maintenance);

            // サービスマン以外(ユーザーレベル1,2,3)の場合
            if (!isServiceman)
            {
                // エラー履歴の書き込み時刻と比較し、日付が違う場合
                if (data.WriteTime.Date != DateTime.Today.Date)
                {
                    result = false;
                }
            }

            // 絞り込み条件がnullではない場合
            if (result && searchInfo != null)
            {
                // サービスマン(ユーザーレベル4,5)の場合かつエラー履歴書き込み時間でフィルタリングする場合
                if (isServiceman && searchInfo.WriteTimeSelect.Item1)
                {
                    // エラー履歴書き込み時間が設定範囲時間外の時
                    if (( searchInfo.WriteTimeSelect.Item2.Date > data.WriteTime.Date )
                        || ( searchInfo.WriteTimeSelect.Item3.Date < data.WriteTime.Date ))
                    {
                        result = false;
                    }
                }

                // ユーザーIDでフィルタリングする場合
                if (searchInfo.UserIDSelect.Item1)
                {
                    // エラー履歴のユーザIーDとフィルタリング条件が一致しない場合
                    if (!data.UserID.Equals(searchInfo.UserIDSelect.Item2.ToString()))
                    {
                        result = false;
                    }
                }

                // エラーコードでフィルタリングする場合
                if (searchInfo.ErrorCodeSelect.Item1)
                {
                    // エラー履歴のエラーコードとフィルタリング条件が一致しない場合
                    if (!data.GetErrorCode().ToString().Equals(searchInfo.ErrorCodeSelect.Item2.ToString()))
                    {
                        result = false;
                    }
                }

                // エラー引数でフィルタリングする場合
                if (searchInfo.ErrorArgSelect.Item1)
                {
                    // エラー履歴のエラー引数とフィルタリング条件が一致しない場合
                    if (!data.GetErrorArg().ToString().Equals(searchInfo.ErrorArgSelect.Item2.ToString()))
                    {
                        result = false;
                    }
                }

                // エラーレベルでフィルタリングする場合
                if (searchInfo.ErrorLevelSelect.Item1)
                {
                    // エラー履歴のエラーレベルとフィルタリング条件が一致しない場合
                    if (!data.ErrorLevel.Equals(searchInfo.ErrorLevelSelect.Item2))
                    {
                        result = false;
                    }
                }

                // エラーコメントでフィルタリングする場合
                if (searchInfo.ErrorContentSelect.Item1)
                {
                    // エラー履歴のコメントとフィルタリング条件が一致しない場合
                    if (!data.ErrorTitle.Contains(searchInfo.ErrorContentSelect.Item2))
                    {
                        result = false;
                    }
                }

                // モジュールでフィルタリングする場合
                if (searchInfo.ModuleSelect > 0 )
                {
                    List<Int32> targetModule = new List<Int32>();
                    if (searchInfo.ModuleSelect.HasFlag(ErrorFilteringCategory.Module1))
                    {
                        targetModule.Add((Int32)RackModuleIndex.Module1);
                    }
                    if (searchInfo.ModuleSelect.HasFlag(ErrorFilteringCategory.Module2))
                    {
                        targetModule.Add((Int32)RackModuleIndex.Module2);
                    }
                    if (searchInfo.ModuleSelect.HasFlag(ErrorFilteringCategory.Module3))
                    {
                        targetModule.Add((Int32)RackModuleIndex.Module3);
                    }
                    if (searchInfo.ModuleSelect.HasFlag(ErrorFilteringCategory.Module4))
                    {
                        targetModule.Add((Int32)RackModuleIndex.Module4);
                    }
                    if (searchInfo.ModuleSelect.HasFlag(ErrorFilteringCategory.RackTransfer))
                    {
                        targetModule.Add((Int32)RackModuleIndex.RackTransfer);
                    }
                    if (searchInfo.ModuleSelect.HasFlag(ErrorFilteringCategory.DPR))
                    {
                        targetModule.Add(-1);
                    }

                    if (!targetModule.Contains(data.ModuleNo))
                    {
                        result = false;
                    }
                }

                // エラー発生回数でフィルタリングする場合
                if (searchInfo.SumSelect.Item1)
                {
                    // エラー発生回数が設定範囲時間外の時
                    if (( searchInfo.SumSelect.Item2 > data.Counter)
                        || ( searchInfo.SumSelect.Item3 < data.Counter ))
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// DBのカラム追加
        /// <remarks>
        /// 今後、カラムの型変換を行う際は中のクエリを変更するような処理に変える
        /// </remarks>
        /// </summary>
        virtual public void AddColumn()
        {
            // NULL不許可（デフォルト=1）でカウンターカラムを追加
            String addColumnSql = String.Format("alter table dbo.errorLog add {0} int not null default '1'", ErrorLogDB.STRING_COUNTER);

            //　クエリ実行
            this.ExecuteSql(addColumnSql);
        }

        /// <summary>
        /// エラーログテーブル取得
        /// </summary>
        /// <remarks>
        /// エラーログをDBから読込みます。
        /// </remarks>
        public override void LoadDB()
        {
            this.fillBaseTable();
        }

        /// <summary>
        /// エラーログテーブル書込み
        /// </summary>
        /// <remarks>
        /// エラーログをDBに書き込みます。
        /// </remarks>
        public void CommitErrorLog()
        {
            this.updateBaseTable();
        }

        #endregion

    }

    /// <summary>
    /// エラー蓄積用クラス
    /// </summary>
    /// <remarks>
    /// エラー情報と回数を持つエラー蓄積用データクラス
    /// </remarks>
    public class ErrorDataStorage : DPRErrorCode
    {
        /// <summary>
        /// エラー連続発生回数
        /// </summary>
        private Int32 count = 1;

        /// <summary>
        /// エラー連続発生回数
        /// </summary>
        public Int32 Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
            }
        }

        /// <summary>
        /// エラー蓄積データの最終日時
        /// </summary>
        public DateTime LogDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 拡張文字列
        /// </summary>
        private String extStr;

        /// <summary>
        /// 拡張文字列
        /// </summary>
        public String ExtStr
        {
            get
            {
                return extStr;
            }
            set
            {
                extStr = value; 
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="code">エラーコード</param>
        /// <param name="arg">エラー引数</param>
        /// <param name="moduleId">モジュールID</param>
        /// <param name="extStr">拡張文字列</param>
        public ErrorDataStorage(Int32 code, Int32 arg, Int32 moduleId = -1, String extStr = "") : base (code, arg, moduleId)
        {
            this.LogDateTime = DateTime.Now;
            this.ExtStr = extStr;
        }

        /// <summary>
        /// エラー情報の更新
        /// </summary>
        /// <param name="extStr">拡張文字列</param>
        public void DataUpdate( String extStr )
        {
            // 連続発生回数を+1する
            this.Count += 1;

            // エラー情報の最終時刻を更新
            this.LogDateTime = DateTime.Now;

            // 拡張文字列を更新
            this.ExtStr = extStr;
        }
    }

    /// <summary>
    /// エラー蓄積管理クラス
    /// </summary>
    /// <remarks>
    /// エラー蓄積データの追加、更新、エラー履歴登録可能の判別、エラー履歴の登録、リスト削除などを行う
    /// </remarks>
    public class ErrorDataStorageManeger
    {
        // エラー蓄積データリスト
        private List<ErrorDataStorage> errorDataStorageList;

        /// <summary>
        /// 分析ステータス最終時間
        /// </summary>
        private DateTime assayStatusTime;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ErrorDataStorageManeger()
        {
            this.errorDataStorageList = new List<ErrorDataStorage>();
            this.assayStatusTime = new DateTime();
        }

        /// <summary>
        /// 蓄積データの更新または追加
        /// </summary>
        /// <param name="errorCode">追加するエラーデータ</param>
        /// <param name="extStr">拡張文字列</param>
        /// <returns>エラー発生回数</returns>
        public Int32 ErrorDataStorageListUpdateOrAdd( DPRErrorCode errorCode, String extStr)
        {
            String dbgMsg = String.Format("[[Investigation log]]ErrorDataStorageManeger::errorDataStorageListUpdateOrAdd ");

            // マスターエラーログ用のカウント
            Int32 masterLogCounter = 1;

            // 新規追加フラグ
            bool newDataFlag = true;

            foreach (ErrorDataStorage errorData in errorDataStorageList)
            {
                // エラー情報が既にリスト内にある場合
                if (( errorData.ErrorCode == errorCode.ErrorCode ) && ( errorData.Arg == errorCode.Arg ))
                {
                    // 更新前時刻の取得
                    DateTime dateTime = errorData.LogDateTime;

                    // 発生回数と発生時刻、拡張文字列を更新
                    errorData.DataUpdate(extStr);

                    // マスターエラーログ用のカウントアップの設定
                    masterLogCounter = errorData.Count;

                    // エラーログの更新
                    String sqlStr = this.makeUpdateSqlStr(dateTime, errorData);
                    Singleton<DBAccessControl>.Instance.ExecuteSql(sqlStr, String.Empty);
                    
                    newDataFlag = false;

                    dbgMsg += String.Format("[update]{0}-{1},WriteTime={2},Counter={3}", errorData.ErrorCode, errorData.Arg, errorData.LogDateTime, errorData.Count);

                    break;
                }
            }

            // 新規追加の場合
            if (newDataFlag)
            {
                // 蓄積データの新規作成
                ErrorDataStorage newData = new ErrorDataStorage(errorCode.ErrorCode, errorCode.Arg, errorCode.ModuleId);

                // リストへの追加
                errorDataStorageList.Add(newData);

                // マスターエラーログ用のカウントアップの設定
                masterLogCounter = newData.Count;

                // エラーログに登録
                this.registerErrorLog(newData);

                dbgMsg += String.Format("[add]{0}-{1},WriteTime={2},Counter={3}", newData.ErrorCode, newData.Arg, newData.LogDateTime, newData.Count);
            }

            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);

            return masterLogCounter;
        }

        /// <summary>
        /// エラー履歴への登録
        /// </summary>
        /// <param name="errorData"></param>
        private void registerErrorLog( ErrorDataStorage errorData )
        {
            CarisXLogInfoErrorLogExtention infoErrLog = new CarisXLogInfoErrorLogExtention()
            {
                WriteTime = errorData.LogDateTime,
                ErrorCode = errorData.ErrorCode,
                ErrorArg = errorData.Arg,
                Counter = errorData.Count
            };

            //エラー履歴に登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.ErrorHist
                                                     , Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                                                     , errorData.ModuleId
                                                     , infoErrLog
                                                     , errorData.ExtStr);
        }

        /// <summary>
        /// エラー履歴更新用SQL文作成
        /// </summary>
        /// <param name="errorDataBefore"></param>
        /// <param name="errorDataAfter"></param>
        /// <returns></returns>
        private String makeUpdateSqlStr( DateTime beforeDataTime, ErrorDataStorage errorData )
        {
            // SQL作成
            StringBuilder strSql = new StringBuilder();

            strSql.Append(" UPDATE ");
            strSql.Append("errorLog");
            strSql.Append(" SET ");
            strSql.AppendFormat("writeTime='{0}',contents1='{1}',counter='{2}'", errorData.LogDateTime, errorData.ExtStr, errorData.Count);
            strSql.Append(" WHERE ");
            strSql.AppendFormat("writeTime='{0}' AND moduleNo='{1}' AND errorCode='{2}' AND errorArg='{3}'",
                                 beforeDataTime, errorData.ModuleId,errorData.ErrorCode, errorData.Arg);
            strSql.Append(";");

            return strSql.ToString();
        }

        /// <summary>
        /// 連続発生しなくなったエラー蓄積データの削除
        /// </summary>
        private void deleteRegisterList()
        {
            if (errorDataStorageList != null)
            {
                // DB登録が行われてエラー情報をエラー蓄積データリストから削除
                errorDataStorageList.RemoveAll(i => ( i.LogDateTime.CompareTo(this.assayStatusTime) ) == -1);
            }
        }

        /// <summary>
        /// 連続発生しなくなったエラー蓄積データの削除と比較時間の取得
        /// </summary>
        public void DeleteFilteringData()
        {
            // 連続発生しなくなったエラー蓄積データの削除
            this.deleteRegisterList();
            
            // 分析ステータスの日時取得
            this.assayStatusTime = DateTime.Now;
        }

        /// <summary>
        /// 全削除
        /// </summary>
        public void DeleteAllList()
        {
            errorDataStorageList.Clear();
        }

    }

    /// <summary>
    /// エラー蓄積データリストの管理クラス
    /// </summary>
    /// <remarks>
    /// 各モジュールごとにエラー蓄積データリストを管理する
    /// </remarks>
    public class ErrorDataStorageListManeger
    {
        // エラー蓄積データリストのディクショナリ
        public List<ErrorDataStorageManeger> ErrorDataStorageList;

        // 接続台数
        Int32 connectModuleCount = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ErrorDataStorageListManeger()
        {
            connectModuleCount = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;
            ErrorDataStorageList = new List<ErrorDataStorageManeger>();

            // 接続台数分リスト作成
            for (int i = 0; i < connectModuleCount; i++)
            {
                ErrorDataStorageList.Add(new ErrorDataStorageManeger());
            }
        }

        /// <summary>
        /// すべてのエラー蓄積データリストを削除する
        /// </summary>
        public void DeleteAllList()
        {
            foreach(ErrorDataStorageManeger errorDataList in ErrorDataStorageList)
            {
                errorDataList.DeleteAllList();
            }
        }

    }
}
