using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.CarisX.Const;
using Oelco.Common.Parameter;
using System.IO;
using Oelco.Common.Utility;
using Oelco.CarisX.Log;
using Oelco.Common.Log;

namespace Oelco.CarisX.Parameter.ErrorCodeData
{

    /// <summary>
    /// エラーコードデータ管理クラス
    /// </summary>
    /// <remarks>
    /// エラーコードデータを管理します。
    /// </remarks>
    public class ErrorCodeDataManager : ISavePath
    {
        #region [クラス変数定義]
        /// <summary>
        /// エラーコード引数が分析項目番号の場合の変換処理
        /// </summary>
        private static Func<ErrorCodeData, ErrorCodeData> argumentProtocolNo = ( data ) =>
           {
               ErrorCodeData resultData = null;
               Int32 measureProtocolNo = 0;
               if ( Int32.TryParse( data.Argment, out measureProtocolNo ) )
               {
                   var measureProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo( measureProtocolNo );
                   resultData = data;
                   string sProtocolName = measureProtocol == null ? string.Empty : measureProtocol.ProtocolName;
                   resultData.Title = String.Format(data.Title, sProtocolName);
                   resultData.Message = String.Format(data.Message, sProtocolName);
               }
               return resultData;
           };

        /// <summary>
        /// エラーコード121-1専用変換処理
        /// </summary>
        private static Func<ErrorCodeData, ErrorCodeData> argumentTitleProtocolNo = ( data ) =>
        {
            ErrorCodeData resultData = null;
            Int32 measureProtocolNo = 0;
            if ( Int32.TryParse( data.Argment, out measureProtocolNo ) )
            {
                var measureProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo( measureProtocolNo );
                resultData = data;
                string sProtocolName = measureProtocol == null ? string.Empty : measureProtocol.ProtocolName;
                resultData.Title = String.Format(data.Title, sProtocolName);
                resultData.Message = String.Format(data.Message, sProtocolName);
                resultData.Argment = "1";
            }
            return resultData;
        };
        #endregion

        #region [インスタンス変数定義]
        /// <summary>
        /// エラーコードデータ
        /// </summary>
        private List<ErrorCodeData> codeDataList = new List<ErrorCodeData>();

        /// <summary>
        /// 引数が通常のエラーコード引数で無いエラーの処理
        /// </summary>
        private Dictionary<String, Func<ErrorCodeData, ErrorCodeData>> argumentDynamicConverter = new Dictionary<String, Func<ErrorCodeData, ErrorCodeData>>(){
        {"70",argumentProtocolNo},
        {"71",argumentProtocolNo},
        {"73",argumentProtocolNo},
        {"100",argumentProtocolNo},
        {"101",argumentProtocolNo},
        {"121",argumentTitleProtocolNo},
        {"203",argumentProtocolNo},
        {"204",argumentProtocolNo},
        };
        #endregion

        #region [プロパティ]
        /// <summary>
        /// エラーコードデータ 設定/取得
        /// </summary>
        public List<ErrorCodeData> CodeDataList
        {
            get
            {
                return this.codeDataList;
            }
            set
            {
                this.codeDataList = value;
            }
        }
        #endregion

        #region [publicメソッド]
        /// <summary>
        /// エラーコードデータ取得
        /// </summary>
        /// <remarks>
        /// エラーコード・エラーコード引数を元にエラーコードデータを取得します。
        /// </remarks>
        /// <param name="errorCode">エラーコード</param>
        /// <param name="errorCodeArgument">エラーコード引数</param>
        /// <returns>エラーコードデータ</returns>
        public ErrorCodeData GetCodeData( String errorCode, String errorCodeArgument )
        {
            ErrorCodeData data = null;

            // エラーコード・引数を元に保持データから検索する。
            var searchedGroup = ( from v in this.codeDataList
                                  where v.Code == errorCode
                                  select v ).ToDictionary( ( errorCodedata ) => errorCodedata.Argment );

            if ( !searchedGroup.TryGetValue( errorCodeArgument, out data ) && searchedGroup.TryGetValue( String.Empty, out data ) )
            {
                // codeDataListの実体に影響を及ぼさないよう変更（最後に表示したもののみ正常に出る状態になってしまうため）
                ErrorCodeData dynamicData = new ErrorCodeData();
                dynamicData.Argment = errorCodeArgument;
                dynamicData.Code = data.Code;
                dynamicData.ImagePath = data.ImagePath;
                dynamicData.Message = data.Message;
                dynamicData.Title = data.Title;
                data = dynamicData;
                //data.Argment = errorCodeArgument;
                Func<ErrorCodeData, ErrorCodeData> func;
                if ( this.argumentDynamicConverter.TryGetValue( data.Code, out func ) )
                {
                    data = this.argumentDynamicConverter[data.Code]( data );
                }
            }
            return data;
        }
        #endregion

        #region ISavePath メンバー

        /// <summary>
        /// 保存パス
        /// </summary>
        public String SavePath
        {
            get
            {
                return CarisXConst.PathError + @"\ErrorMessage.xml";
            }
        }

        #endregion
    }

    /// <summary>
    /// エラーコードデータ
    /// </summary>
    /// <remarks>
    /// エラーコードデータクラス
    /// </remarks>
    public class ErrorCodeData
    {
        /// <summary>
        /// エラーコード
        /// </summary>
        public String Code = String.Empty;
        /// <summary>
        /// エラーコード引数
        /// </summary>
        public String Argment = String.Empty;
        /// <summary>
        /// エラーメッセージタイトル
        /// </summary>
        public String Title = String.Empty;
        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public String Message = String.Empty;
        /// <summary>
        /// エラー画像パス
        /// </summary>
        public String ImagePath = String.Empty;

        /// <summary>
        /// エラー画像完全パス取得
        /// </summary>
        /// <remarks>
        /// エラー画像パスにフォルダ構成を適用して取得します。
        /// </remarks>
        /// <returns>エラー画像完全パス</returns>
        public String GetFullImagePath()
        {
            String returnPath = this.ImagePath;
            try
            {
                String root = Path.GetPathRoot( this.ImagePath );
                if ( ( root == String.Empty ) || ( root == @"\" ) )
                {
                    // パスがルートからでなければ、エラー画像パスを付与
                    returnPath = CarisXConst.PathErrImage + @"\" + this.ImagePath;
                }
            }
            catch ( Exception ex )
            {
                Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, ex.StackTrace );

                // GetPathRootで失敗したら空白
                returnPath = String.Empty;
            }
            return returnPath;
        }
    }
}
