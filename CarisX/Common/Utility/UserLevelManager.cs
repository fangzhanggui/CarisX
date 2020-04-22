using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.Common.Utility
{
    /// <summary>
    /// ユーザレベル定義
    /// </summary>
    public enum UserLevel
    {
        /// <summary>
        /// 未ログイン
        /// </summary>
        None,
        /// <summary>
        /// レベル1
        /// </summary>
        Level1,
        /// <summary>
        /// レベル2
        /// </summary>
        Level2,
        /// <summary>
        /// レベル3
        /// </summary>
        Level3,
        /// <summary>
        /// レベル4
        /// </summary>
        Level4,
        /// <summary>
        /// レベル5
        /// </summary>
        Level5
    }

    /// <summary>
    /// ユーザレベル管理
    /// </summary>
    /// <remarks>
    /// ユーザレベルの管理機能を提供します。
    /// </remarks>
    public abstract class UserLevelManager
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// ログイン中のユーザレベル
        /// </summary>
        protected UserLevel nowUserLevel = UserLevel.None;

        /// <summary>
        /// ログイン中のアカウント
        /// </summary>
        protected Tuple<String, String> nowAccount = null;

        /// <summary>
        /// アカウント辞書
        /// </summary>
        protected Dictionary<Tuple<String, String>, UserLevel> accountDictionary = new Dictionary<Tuple<String, String>, UserLevel>();
        
        #endregion

        #region [プロパティ]

        /// <summary>
        /// ログイン中のユーザレベル
        /// </summary>
        public UserLevel NowUserLevel
        {
            get
            {
                return nowUserLevel;
            }
        }
        /// <summary>
        /// ログイン中のユーザID
        /// </summary>
        public String NowUserID
        {
            get
            {
                return this.nowAccount != null ? this.nowAccount.Item1 : "";
            }
        }
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 有効アカウント追加
        /// </summary>
        /// <remarks>
        /// 引数値で渡されたID、パスワードの組み合わせが
        /// 格納済みの値でない場合、ディクショナリに追加します。
        /// </remarks>
        /// <param name="id">アカウントId</param>
        /// <param name="password">アカウントパスワード</param>
        /// <param name="level">ユーザレベル</param>
        public void AddAccount( String id, String password, UserLevel level )
        {

            Tuple<String, String> account = new Tuple<String, String>( id, password );

            // 既知のid,passwordペアでなければ追加する
            if ( !this.accountDictionary.ContainsKey( account ) )
            {
                this.accountDictionary.Add( account, level );
            }
        }

        /// <summary>
        /// 有効アカウント情報クリア
        /// </summary>
        /// <remarks>
        /// 現在ログイン中のアカウント情報を除いて、全てのアカウントを削除します。
        /// </remarks>
        public void ClearAccountInfo()
        {
            var delList = from v in this.accountDictionary
                          where v.Key != this.nowAccount
                          select v.Key;
            while ( delList.Count() != 0 )
            {
                this.accountDictionary.Remove( delList.First() );
            }
        }

        /// <summary>
        /// ログイン
        /// </summary>
        /// <remarks>
        /// ログイン処理を行います。
        /// </remarks>
        /// <param name="id">アカウントId</param>
        /// <param name="password">アカウントパスワード</param>
        /// <returns>True:ログイン成功 False:ログイン失敗</returns>
        public Boolean Login( String id, String password )
        {
            Boolean logined = false;
            Tuple<String, String> account = new Tuple<String, String>( id, password );

            // 既知のid,passwordペアであればログインする
            if ( this.accountDictionary.ContainsKey( account ) )
            {
                this.nowAccount = account;
                this.nowUserLevel = this.accountDictionary[account];
                logined = true;
            }

            return logined;
        }

        /// <summary>
        /// ログアウト
        /// </summary>
        /// <remarks>
        /// ログアウト処理を行います。
        /// </remarks>
        /// <returns>True:ログアウト成功 False:ログアウト失敗</returns>
        public Boolean Logout()
        {
            Boolean logouted = false;

            if ( this.nowAccount != null )
            {
                this.nowAccount = null;
                this.nowUserLevel = UserLevel.None;
                logouted = true;
            }

            return logouted;
        }

        /// <summary>
        /// パスワードチェック
        /// </summary>
        /// <remarks>
        /// パスワードチェック処理を行います。
        /// </remarks>
        /// <param name="id">アカウントId</param>
        /// <param name="password">アカウントパスワード</param>
        /// <returns>True:ログイン成功 False:ログイン失敗</returns>
        public Boolean PasswordCheck(String password)
        {
            Boolean checkresult = false;
            Tuple<String, String> account = new Tuple<String, String>(this.nowAccount.Item1, password);

            // 既知のid,passwordペアであればログインする
            if (this.accountDictionary.ContainsKey(account))
            {
                checkresult = true;
            }

            return checkresult;
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// ユーザ存在確認
        /// </summary>
        /// <remarks>
        /// 引数で渡されたユーザIDアカウント辞書にが存在するかどうかを返します。
        /// </remarks>
        /// <param name="userId">確認対象ユーザID</param>
        /// <returns>True:存在する、False:存在しない</returns>
        protected Boolean isContainUser( String userId )
        {
            // change by marxsu 
            userId = userId.ToLower();
            var containCheck = from v in this.accountDictionary.Keys
                               where v.Item1.ToLower() == userId
                               select v.Item1;
            return containCheck.Count() != 0;
        }
        /// <summary>
        /// ユーザ削除
        /// </summary>
        /// <remarks>
        /// 引数値で渡されたユーザIDをアカウント辞書から削除します。
        /// </remarks>
        /// <param name="userId"></param>
        protected void removeUser( String userId )
        {
            foreach ( var account in ( from v in this.accountDictionary
                                     where v.Key.Item1 == userId
                                     select v.Key ).ToList() )
            {
                this.accountDictionary.Remove( account );
            }
        }
        #endregion

    }
}
