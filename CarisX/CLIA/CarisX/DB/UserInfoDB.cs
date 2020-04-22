using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.DB;
using Oelco.Common.Utility;
using Oelco.Common.Log;
using Oelco.CarisX.Log;

namespace Oelco.CarisX.DB
{
    /// <summary>
    /// ユーザ情報DB
    /// </summary>
    /// <remarks>
    /// ユーザ情報を扱うDB操作を提供します。
    /// </remarks>
    public class UserInfoDB : DBAccessControl
    {
        #region [定数定義]
        /// <summary>
        /// ユーザー名(DBテーブル：userInfo列名)
        /// </summary>
        public const String STRING_NAME = "name";
        /// <summary>
        /// パスワード(DBテーブル：userInfo列名)
        /// </summary>
        public const String STRING_PASSWORD = "password";
        /// <summary>
        /// ユーザーレベル(DBテーブル：userInfo列名)
        /// </summary>
        public const String STRING_LEVEL = "level";

        #endregion

        #region [プロパティ]

        /// <summary>
        /// データテーブル取得SQL
        /// </summary>
        protected override String baseTableSelectSql
        {
            get
            {
                return "SELECT name, password, level FROM dbo.userInfo";
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// ユーザ情報取得
        /// </summary>
        /// <remarks>
        /// 取得済みのデータテーブルから、ユーザ情報を抽出します。
        /// </remarks>
        /// <returns>ユーザ情報リスト</returns>
        public List<Tuple<String, String, UserLevel>> GetUserInformation()
        {
            List<Tuple<String, String, UserLevel>> info = new List<Tuple<String, String, UserLevel>>();

            if ( this.DataTable != null )
            {
                // テーブル内容全て返す
                foreach ( System.Data.DataRow row in this.DataTable.Rows )
                {
                    try
                    {
                        info.Add( new Tuple<String, String, UserLevel>
                        ( (String)row[STRING_NAME], (String)row[STRING_PASSWORD], (UserLevel)row[STRING_LEVEL] ) );
                    }
                    catch ( Exception ex )
                    {
                        // DB内部に不正データ
                        Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
                    }
                }
            }

            return info;
        }

        /// <summary>
        /// ユーザ情報追加
        /// </summary>
        /// <remarks>
        /// ユーザ情報をデータテーブルに追加します。
        /// </remarks>
        /// <param name="info">ユーザ情報</param>
        public void AddUserInformation( Tuple<String, String, UserLevel> info )
        {
            if ( this.DataTable != null )
            {
                // 主キーのガードはここではかけない
                this.DataTable.Rows.Add( info.Item1, info.Item2, (Int32)info.Item3 );
            }
        }

        /// <summary>
        /// ユーザ情報削除
        /// </summary>
        /// <remarks>
        /// ユーザ情報を削除します。
        /// </remarks>
        /// <param name="name">ユーザ名称</param>
        public void RemoveUserInformation( String name )
        {
            if ( this.DataTable != null )
            {
                foreach ( System.Data.DataRow row in this.DataTable.Rows )
                {
                    // 該当ネームを持つデータを削除状態へ
                    if ( (String)row[STRING_NAME] == name )
                    {
                        row.Delete();
                    }
                }
            }
        }

        /// <summary>
        /// ユーザ情報設定
        /// </summary>
        /// <remarks>
        /// ユーザ情報を設定します。
        /// </remarks>
        /// <param name="info">ユーザ情報</param>
        public void SetUserInformation( Tuple<String, String, UserLevel> info )
        {
            if ( this.DataTable != null )
            {
                foreach ( System.Data.DataRow row in this.DataTable.Rows )
                {
                    if ( (String)row[STRING_NAME] == info.Item1 )
                    {
                        row[STRING_PASSWORD] = info.Item2;
                        row[STRING_LEVEL] = (Int32)info.Item3;
                    }
                }
            }
        }

        /// <summary>
        /// ユーザ情報テーブル取得
        /// </summary>
        /// <remarks>
        /// ユーザ情報をDBから読込みます。
        /// </remarks>
        public override void LoadDB()
        {
            this.fillBaseTable();
            RemoveL4_L5();
            CommitUserInfo();
            
        }

        /// <summary>
        /// ユーザ情報テーブル書込み
        /// </summary>
        /// <remarks>
        /// ユーザ情報をDBに書き込みます。
        /// </remarks>
        public void CommitUserInfo()
        {           
            this.updateBaseTable();
        }

        public void RemoveL4_L5()
        {
            if (this.DataTable != null)
            {
                foreach (System.Data.DataRow row in this.DataTable.Rows)
                {
                    // 該当ネームを持つデータを削除状態へ
                    if ((UserLevel)row[STRING_LEVEL] == UserLevel.Level4 || ((UserLevel)row[STRING_LEVEL] == UserLevel.Level5))
                    {
                        row.Delete();
                    }
                }
            }
        } 


        #endregion

    }
}
