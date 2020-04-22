//----------------------------------------------------------------
// Public Class.
//	  LogInfo
// Info.
//   ログ情報クラス
// History
//   2011/09/01　　Ver1.00.00　　新規作成
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.Common.Log
{
    /// <summary>
    /// ログ種別
    /// </summary>
    public enum LogKind
    {
        /// <summary>
        ///　エラー履歴
        /// </summary>
        ErrorHist = 0,
        /// <summary>
        /// 操作履歴
        /// </summary>
        OperationHist,
        /// <summary>
        /// 在线日志(LIS)
        /// </summary>
        OnlineHist, // 通信Libが実装  //HOWTO:defined by Figu Lin
        /// <summary>
        /// 分析履歴
        /// </summary>
        AnalyseHist,
        /// <summary>
        /// パラメータ変更履歴
        /// </summary>
        ParamChangeHist,
        /// <summary>
        /// メンテナンス画面ログイン履歴
        /// </summary>
        MaintenanceLogin,
        /// <summary>
        /// メンテナンス画面ログイン履歴
        /// </summary>
        MasterErrorHist,
        /// <summary>
        /// デバッグログ
        /// </summary>
        DebugLog,
    }

    /// <summary>
    /// ログレベル(ログ種別がDebugLogの場合のみ影響)
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// エラー
        /// </summary>
        Error = 0,
        /// <summary>
        /// デバッグ
        /// </summary>
        Debug,
    }

    /// <summary>
    /// ログ情報
    /// </summary>
    public class LogInfo
    {
        #region [ ログ情報 ]
        /// <summary>
        /// ログ種別
        /// </summary>
        public LogKind Kind
        {
            get;
            set;
        }

        /// <summary>
        /// 日時
        /// </summary>
        public DateTime WriteDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// ユーザID
        /// </summary>
        public String UserId
        {
            get;
            set;
        }

        /// <summary>
        /// 履歴DBコンテンツ
        /// </summary>
        public List<String> Contents
        {
            get;
            set;
        }

        #endregion

        #region [ ログ種別がDebugLogの場合の追加情報 ]

        /// <summary>
        /// ログレベル
        /// </summary>
        public LogLevel Level
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public LogInfo()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="lKind"></param>
        /// <param name="lLevel"></param>
        /// <param name="dtOutputDateTime"></param>
        /// <param name="strUid"></param>
        /// <param name="contents"></param>
        public LogInfo( LogKind lKind,
                       LogLevel lLevel,
                       DateTime dtOutputDateTime,
                       String strUid,
                       List<String> contents)
        {
            this.Kind = lKind;
            this.Level = lLevel;
            this.WriteDateTime = dtOutputDateTime;
            this.UserId = strUid;
            this.Contents = contents;
        }

        #region [ AFTのみ操作履歴にユーザーレベルを追加 ]

        /// <summary>
        /// ユーザーレベル
        /// </summary>
        public String UserLevel
        {
            get;
            set;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="lKind"></param>
        /// <param name="lLevel"></param>
        /// <param name="dtOutputDateTime"></param>
        /// <param name="strUid"></param>
        /// <param name="contents"></param>
        public LogInfo( LogKind lKind,
                       LogLevel lLevel,
                       DateTime dtOutputDateTime,
                       String strUid,
                       String strUlvl,
                       List<String> contents)
        {
            this.Kind = lKind;
            this.Level = lLevel;
            this.WriteDateTime = dtOutputDateTime;
            this.UserId = strUid;
            this.UserLevel = strUlvl;
            this.Contents = contents;
        }

        #endregion
    }
}
