using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Threading.Tasks;
using Oelco.Common.Log;
using Oelco.Common.Utility;

namespace Oelco.Common.DB
{
    /// <summary>
    /// DB行データの内部利用向けラッパー基底クラス
    /// </summary>
    /// <remarks>
    /// DataRowをラップした各種DB向けデータクラスの規定として、
    /// DataRowとデータの仲介を行します。
    /// DataRow中の各種データへのアクセス(派生クラス内部)は、
    /// Feildメソッドにて取得、SetFeildメソッドにて設定を行します。
    /// 同一インスタンスでない場合、主キーの一致の確認を
    /// DBTableKeyMatchメソッドで行い、データの同期等で使用します。
    /// 
    /// ●使用方法として
    /// グリッドのDataSourceにBindingListで設定する場合、本クラスの派生クラスでプロパティ公開により列の表示が行われ、
    /// 行ごとに非表示のデータが必要な場合、外部で取得編集が必要なものはpublicのSet/Getメソッドで公開
    /// 外部で取得編集が必要ない場合でも、protectedのSet/Getメソッドを用意することで、
    /// 随時公開設定の変更のみで対応可能となります。
    /// </remarks>
    public class DataRowWrapperBase
    {
        #region [定数定義]

        /// <summary>
        /// 変更対象の行データ
        /// </summary>
        private DataRow row;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// オブジェクトの状態を取得
        /// </summary>
        private DataRowState state;

        /// <summary>
        /// 取得データのバージョン
        /// </summary>
        private DataRowVersion version = DataRowVersion.Default;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="row">フィールドにマッピングする行データ</param>
        private DataRowWrapperBase( DataRow row )
        {
            this.row = row;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data">フィールドにマッピングする同行を持つデータ</param>
        protected DataRowWrapperBase( DataRowWrapperBase data )
        {
            this.row = data.row;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// メンバフィールドで実装する行データ
        /// </summary>
        protected DataRow Row
        {
            get
            {
                return this.row;
            }
        }

        /// <summary>
        /// オブジェクトの状態を取得
        /// </summary>
        protected DataRowState State
        {
            get
            {
                if ( this.state != DataRowState.Deleted )
                {
                    return this.Row.RowState;
                }
                return this.state;
            }
            private set
            {
                this.state = value;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// DataTable行データ(DataRow)の暗黙変換
        /// </summary>
        /// <remarks>
        /// DataRowからDataRowWrapperBaseへ暗黙的変換します。
        /// </remarks>
        /// <param name="row">変換元行データ</param>
        /// <returns>変換後ラップデータ</returns>
        public static implicit operator DataRowWrapperBase( DataRow row )
        {
            return new DataRowWrapperBase( row );
        }

        /// <summary>
        /// データ削除
        /// </summary>
        /// <remarks>
        /// データを削除状態にします。
        /// </remarks>
        public void DeleteData()
        {
            this.State = DataRowState.Deleted;
            //this.Row.Delete();
        }

        /// <summary>
        /// データのロールバック
        /// </summary>
        /// <remarks>
        /// データを取得時の状態へ戻します。
        /// </remarks>
        public void RollbackData()
        {
            this.Row.RejectChanges();
        }

        /// <summary>
        /// 追加データフラグ
        /// </summary>
        /// <remarks>
        /// 追加データであるかどうかを取得します。
        /// </remarks>
        /// <returns>true:追加データ/false:非追加データ</returns>
        public Boolean IsAddedData()
        {
            return ( ( this.Row.RowState & DataRowState.Added ) == DataRowState.Added || ( this.Row.RowState & DataRowState.Detached ) == DataRowState.Detached ) && ( this.State & DataRowState.Deleted ) != DataRowState.Deleted;
        }

        /// <summary>
        /// 編集データフラグ
        /// </summary>
        /// <remarks>
        /// 編集データであるかどうかを取得します。
        /// </remarks>
        /// <returns>true:編集データ/false:編集データ</returns>
        public Boolean IsModifyData()
        {
            return ( this.Row.RowState & DataRowState.Modified ) == DataRowState.Modified;
        }

        /// <summary>
        /// 削除データフラグ
        /// </summary>
        /// <remarks>
        /// 削除データであるかどうかを取得
        /// </remarks>
        /// <returns>true:削除データ/false:削除データ</returns>
        public Boolean IsDeletedData()
        {
            return ( this.State & DataRowState.Deleted ) == DataRowState.Deleted;
        }
        
        /// <summary>
        /// DBテーブルの主キーが一致するかどうかを取得
        /// </summary>
        /// <remarks>
        /// DBテーブルの主キーが比較対象と一致するかどうかを取得します。
        /// </remarks>
        /// <param name="target">比較対象</param>
        /// <returns>true:一致/false:非一致</returns>
        public Boolean DBTableKeyMatch( DataRowWrapperBase target )
        {
            return DBTableKeyMatch( target.Row );
        }

        /// <summary>
        /// DBテーブルの主キーが一致するかどうかを取得
        /// </summary>
        /// <remarks>
        /// DBテーブルの主キーが比較対象と一致するかどうかを取得します。
        /// </remarks>
        /// <param name="target">比較対象</param>
        /// <returns>true:一致/false:非一致</returns>
        public Boolean DBTableKeyMatch( DataRow targetRow )
        {
            // 主キーによる比較
            Boolean result = true;

            foreach ( DataColumn column in this.Row.Table.PrimaryKey )
            {
                if ( column.DataType == targetRow.Table.Columns[column.ColumnName].DataType )
                {
                    var data = this.Row.HasVersion( DataRowVersion.Original ) ? this.Row[column, DataRowVersion.Original] : this.Row[column];
                    result &= ( data.Equals( targetRow[column.ColumnName] ) );
                    if ( !result )
                    {
                        break;
                    }
                }
                else
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// データの追加
        /// </summary>
        /// <remarks>
        /// 追加データリストに対し、本データを追加します。
        /// </remarks>
        /// <typeparam name="T">DataRowWrapperBase継承インスタンス</typeparam>
        /// <param name="dataList">追加先データリスト</param>
        /// <param name="acceptChange">追加時の行状態をコミットするかどうか(既定:false=コミット不実施)</param>
        /// <returns>true:追加成功/false:追加失敗(データ不整合)</returns>
        public Boolean AddToDataList<T>( List<T> dataList, Boolean acceptChange = false )
            where T : DataRowWrapperBase
        {
            // テーブルへの追加
            try
            {
                DataTable dt;
                if ( dataList.Count > 0 )
                {
                    dt = dataList[0].Row.Table;
                }
                else
                {
                    dt = this.Row.Table.Clone();
                }
                this.row = dt.Rows.Add( this.Row.ItemArray );
                if ( acceptChange )
                {
                    this.Row.AcceptChanges();
                }
            }
            catch ( Exception )
            {
                return false;
            }

            // テーブルに追加されていない場合
            if ( ( this.Row.RowState & DataRowState.Detached ) == DataRowState.Detached )
            {
                return false;
            }

            // リストへの追加
            dataList.Add( (T)this );
            return true;
        }

        /// <summary>
        /// データコピー
        /// </summary>
        /// <remarks>
        /// データをコピーします。
        /// </remarks>
        /// <returns>コピーデータ</returns>
        public DataRowWrapperBase Copy()
        {
            DataTable table = this.row.Table.Clone();
            table.ImportRow( this.row );
            return (DataRowWrapperBase)table.Rows[0];
        }

        /// <summary>
        /// 同期先へ追加を同期
        /// </summary>
        /// <remarks>
        /// 同期先のテーブルに対し、データを追加し同期します。
        /// </remarks>
        /// <param name="table">同期先</param>
        /// <returns>true:同期成功/false:同期失敗</returns>
        public Boolean SyncAddData( DataTable table )
        {
            try
            {
                table.Rows.Add( this.row.ItemArray );
            }
            catch ( Exception ex )
            {
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("SyncAddData:failed to add data{0} {1}", ex.Message, ex.StackTrace));
                return false;
            }
            return true;
        }

        /// <summary>
        /// 同期先へ変更/削除/追加を同期
        /// </summary>
        /// <remarks>
        /// 同期先のデータに対し、編集・削除を同期します。</br>
        /// 追加対象テーブルを指定した場合、テーブルに追加します。
        /// </remarks>
        /// <param name="target">同期先(非NULL:編集削除実施/NULL:編集削除非実施)</param>
        /// <param name="table">追加対象テーブル(非NULL:追加実施/NULL:追加非実施)</param>
        /// <returns>true:同期成功/false:同期失敗</returns>
        public virtual Boolean SyncModifyData( DataRow target, DataTable table = null )
        {
            try
            {
                if ( target != null && target.RowState != DataRowState.Deleted && this.DBTableKeyMatch( target ) )
                {
                    if ( this.IsDeletedData() )
                    {
                        target.Delete();
                    }
                    else if ( this.IsModifyData() || this.IsAddedData() )
                    {
                        if ( target.RowState != DataRowState.Deleted )
                        {
                            // 変更プロパティを更新
                            foreach ( DataColumn column in this.Row.Table.Columns.OfType<DataColumn>() )
                            {
                                target.SetField( target.Table.Columns[column.ColumnName], this.Row[column] );
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else if ( table != null && this.IsAddedData() )
                {
                    this.SyncAddData( table );
                }
                else
                {
                    return false;
                }
            }
            catch ( Exception )
            {
                return false;
            }

            return true;
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// 厳密型指定による指定列の値の取得
        /// </summary>
        /// <remarks>
        /// 指定列名の列のデータをデータ型を指定し取得します。
        /// </remarks>
        /// <typeparam name="T">取得対象の型</typeparam>
        /// <param name="columnName">指定のDB列名</param>
        /// <returns>取得データ</returns>
        protected T Field<T>( String columnName )
        {
            return this.Row.Field<T>( columnName, this.version );
        }

        /// <summary>
        /// 厳密型指定による指定列の値の取得(変更前データ)
        /// </summary>
        /// <remarks>
        /// 指定列名の列の変更前データをデータ型を指定し取得します。
        /// </remarks>
        /// <typeparam name="T">取得対象の型</typeparam>
        /// <param name="columnName">指定のDB列名</param>
        /// <returns>元の(最終更新以降に未変更の)値</returns>
        protected T FieldOriginal<T>( String columnName )
        {
            if ( this.Row.HasVersion( DataRowVersion.Original ) )
            {
                return this.Row.Field<T>( columnName, DataRowVersion.Original );
            }
            return default( T );
        }

        /// <summary>
        /// 厳密型指定による列値の設定
        /// </summary>
        /// <remarks>
        /// 指定列名の列のデータをデータ型を指定し設定します。
        /// </remarks>
        /// <typeparam name="T">取得対象の型</typeparam>
        /// <param name="columnName">指定のDB列名</param>
        protected void SetField<T>( String columnName, T value )
        {
            if ( this.version != DataRowVersion.Original )
            {
                try
                {
                    this.Row.SetField<T>( columnName, value );
                }
                catch ( Exception ex)
                {
                    //Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, "!!! Failed !!! DataRow.SetField<T>()" );
                    Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("!!! Failed !!! DataRow.SetField<T>() Message = {0} StackTrace = {1}", ex.Message, ex.StackTrace));
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// 拡張メソッドクラス
    /// </summary>
    public static class DataRowWrapperListExtension
    {
        /// <summary>
        /// データの同期
        /// </summary>
        /// <remarks>
        /// データの同期を行います。
        /// </remarks>
        /// <typeparam name="T">データ型</typeparam>
        /// <param name="source">同期元データ</param>
        /// <param name="destinationDataTable">同期先データ</param>
        /// <returns>同期結果 trye:同期成功/false:同期失敗</returns>
        public static Boolean SyncDataListToDataTable<T>( this IEnumerable<T> sources, DataTable destinationDataTable )
            where T : DataRowWrapperBase
        {
            //// 変更/削除データの同期、同期結果の取得
            //var matchDataSyncResultList = ( from v in destinationDataTable.AsEnumerable().Where( ( row ) => row.RowState != DataRowState.Deleted )
            //                                let MatchData = sources.GetMatchData( v )
            //                                where MatchData != null
            //                                select new
            //                                {
            //                                    MatchData,
            //                                    SyncResult = MatchData.SyncModifyData( v )
            //                                } ).ToList();

            //var succesModifyDeleteSyncs = matchDataSyncResultList.All( ( matchData ) => matchData.SyncResult );

            //// 追加データの同期、同期結果の取得
            //var succesAddSyncs = ( from v in sources.Where( ( source ) => !matchDataSyncResultList.Exists( ( matchData ) => matchData.MatchData == source ) )
            //                       where v.IsAddedData()
            //                       select v.SyncAddData( destinationDataTable ) ).All( ( syncResult ) => syncResult );


            //// 同期結果
            //if ( !succesModifyDeleteSyncs || !succesAddSyncs )
            //{
            //    // 同期失敗時ロールバック
            //    destinationDataTable.RejectChanges();
            //    Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, "データの追加および更新：失敗!!!(ロールバック対応)" );
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}

            var changeDatas = sources.Where( ( data ) => data.IsAddedData() || data.IsDeletedData() || data.IsModifyData() ).ToList();
            var rows = destinationDataTable.AsEnumerable().Where( ( row ) => row.RowState != DataRowState.Deleted );

            // 同期結果(テーブルデータ数0件の場合は、追加のみ。削除データがテーブルに存在しない場合は、削除成功とする)※編集データがテーブルにない場合失敗する。
            if ( !changeDatas.All( ( data ) => rows.FirstOrDefault( ( row ) => data.SyncModifyData( row, destinationDataTable ) ) != null || data.IsDeletedData() || data.SyncModifyData( null, destinationDataTable ) ) )
            {
                try
                {
                    // 同期失敗時ロールバック
                    destinationDataTable.RejectChanges();
                    //データの追加および更新：失敗!!!(ロールバック対応)
                    Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, "Add and update of data:!! Failure (rollback enabled)");
                }
                catch ( Exception ex )
                {
                    //"データの追加および更新：更新失敗後、ロールバックに失敗しました。
                    Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, String.Format( "Add and update of data: update after the failure, I failed to roll back.{0} {1}", ex.Message, ex.StackTrace ) );
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// データリストロールバック
        /// </summary>
        /// <remarks>
        /// データリストを取得時の状態へ戻します。
        /// </remarks>
        /// <typeparam name="T">データ型</typeparam>
        /// <param name="sources">対象データリスト</param>
        /// <returns>true:成功/false:失敗</returns>
        public static Boolean RollBackDataList<T>( this IEnumerable<T> sources )
            where T : DataRowWrapperBase
        {
            try
            {
                sources.ToList().ForEach( ( source ) =>
                {
                    // ロールバック
                    source.RollbackData();
                } );

                return true;
            }
            catch ( Exception )
            {
                return false;
            }
        }

        /// <summary>
        /// データリスト全削除
        /// </summary>
        /// <remarks>
        /// データリストを全て削除します。
        /// </remarks>
        /// <typeparam name="T">データ型</typeparam>
        /// <param name="sources">対象データリスト</param>
        /// <returns>true:成功/false:失敗</returns>
        public static Boolean DeleteAllDataList<T>( this IEnumerable<T> sources )
            where T : DataRowWrapperBase
        {
            try
            {
                sources.ToList().ForEach( ( source ) =>
                {
                    // 削除
                    source.DeleteData();
                } );

                return true;
            }
            catch ( Exception )
            {
                return false;
            }
        }

        /// <summary>
        /// 主キーが一致するデータの取得
        /// </summary>
        /// <remarks>
        /// 確認データと主キーが一致するデータを返します。
        /// </remarks>
        /// <typeparam name="T">データ型</typeparam>
        /// <param name="targetList">対象データリスト</param>
        /// <param name="checkData">確認データ</param>
        /// <returns>確認データとの主キー一致データ</returns>
        public static T GetMatchData<T>( this IEnumerable<T> targetList, DataRowWrapperBase checkData )
            where T : DataRowWrapperBase
        {
            return ( from target in targetList
                     let IsMatch = target.DBTableKeyMatch( checkData )
                     where IsMatch
                     select target ).FirstOrDefault();
        }

        /// <summary>
        /// 主キーが一致するデータの取得
        /// </summary>
        /// <remarks>
        /// 確認データと主キーが一致するデータを返します。
        /// </remarks>
        /// <typeparam name="T">データ型</typeparam>
        /// <param name="targetList">対象データリスト</param>
        /// <param name="checkData">確認データ</param>
        /// <returns>確認データとの主キー一致データ</returns>
        public static T GetMatchData<T>( this IEnumerable<T> targetList, DataRow checkData )
            where T : DataRowWrapperBase
        {
            return ( from target in targetList
                     let IsMatch = target.DBTableKeyMatch( checkData )
                     where IsMatch
                     select target ).FirstOrDefault();
        }

    }
}
