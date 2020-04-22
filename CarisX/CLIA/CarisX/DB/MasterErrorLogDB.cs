using System;
using System.Collections.Generic;
using System.Linq;
using Oelco.Common.DB;
using Oelco.Common.Utility;
using Oelco.Common.Log;
using System.Data;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter.ErrorCodeData;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Log;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.DB
{
    /// <summary>
    /// マスターエラー履歴データクラス
    /// </summary>
    public class MasterErrorLogData : ErrorLogData
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data"></param>
        public MasterErrorLogData( DataRowWrapperBase data )
            : base( data )
        {
        }

        #endregion

        #region [プロパティ]

        #endregion

        #region [publicメソッド]

        #endregion

        #region [内部クラス]

        #endregion

    }


    /// <summary>
    /// エラー履歴DBクラス
    /// </summary>
    /// <remarks>
    /// エラー履歴のアクセスを行うクラスです。
    /// </remarks>
    public class MasterErrorLogDB : ErrorLogDB
    {
        #region [プロパティ]

        /// <summary>
        /// データテーブル取得SQL
        /// </summary>
        protected override String baseTableSelectSql
        {
            get
            {
                return "SELECT * FROM dbo.masterErrorLog";
            }
        }

        #endregion

        #region [定数定義]

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MasterErrorLogDB()
            : base( Int32.MaxValue )
        {
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// DBのカラム追加
        /// <remarks>
        /// 今後、カラムの型変換を行う際は中のクエリを変更するような処理に変える
        /// </remarks>
        /// </summary>
        override public void AddColumn()
        {
            // NULL不許可（デフォルト=1）でカウンターカラムを追加
            String addColumnSql = String.Format("alter table dbo.masterErrorLog add {0} int not null default '1'", ErrorLogDB.STRING_COUNTER);

            //　クエリ実行
            this.ExecuteSql(addColumnSql);
        }

        #endregion

    }
}
