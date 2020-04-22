//----------------------------------------------------------------
// Public Class.
//	  SqlParam
// Info.
//   (ストアドプロシージャの)パラメータ情報クラス
// History
//   2011/09/01　　Ver1.00.00　　新規作成
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Oelco.Common.DB
{
    /// <summary>
    /// SQL種別
    /// </summary>
    /// <remarks>
    /// Select : Select
    /// Execute : Insert/Update/Delete
    /// Procedure : プロシージャ
    /// </remarks>
    public enum SqlKind
    {
        Select = 0,
        Execute,
        Procedure,
    }

    /// <summary>
    /// (ストアドプロシージャの)パラメータ情報
    /// </summary>
    public class SqlParam
    {
        /// <summary>
        /// パラメータ名
        /// </summary>
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// 値
        /// </summary>
        public object Value
        {
            get;
            set;
        }

        /// <summary>
        /// 型
        /// </summary>
        public SqlDbType Type
        {
            get;
            set;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="strName">パラメータ名</param>
        /// <param name="objValue">パラメータ値</param>
        /// <param name="sdtType">パラメータ型</param>
        public SqlParam( String strName, object objValue, SqlDbType sdtType )
        {
            this.Name = strName;
            this.Value = objValue;
            this.Type = sdtType;
        }
    }

    /// <summary>
    /// SQLキューの情報
    /// </summary>
    /// <remarks>
    /// SQLキュー情報格納クラスです。
    /// </remarks>
    public class SqlInfo
    {

        /// <summary>
        /// SQLキューを識別するユニークKey
        /// </summary>
        public String Key
        {
            get;
            set;
        }

        /// <summary>
        /// SQL もしくは ストアドプロシージャ名
        /// </summary>
        public String Sql
        {
            get;
            set;
        }

        /// <summary>
        /// SQL か ストアドプロシージャか
        /// </summary>
        public CommandType Type
        {
            get;
            set;
        }

        /// <summary>
        /// SQLの種類
        /// </summary>
        public SqlKind Kind
        {
            get;
            set;
        }

        /// <summary>
        /// (ストアドプロシージャの)パラメータ
        /// </summary>
        public List<SqlParam> Params
        {
            get;
            set;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SqlInfo()
        {
            this.Params = new List<SqlParam>();
        }
    }
}
