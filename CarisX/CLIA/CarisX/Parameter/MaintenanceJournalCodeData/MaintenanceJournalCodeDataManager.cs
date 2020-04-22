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

namespace Oelco.CarisX.Parameter.MaintenanceJournalCodeData
{

    /// <summary>
    /// メンテナンス日誌コードデータ管理クラス
    /// </summary>
    /// <remarks>
    /// メンテナンス日誌コードデータを管理します。
    /// </remarks>
    public class MaintenanceJournalCodeDataManager : ISavePath
    {
        #region [クラス変数定義]

        #endregion

        #region [インスタンス変数定義]
        /// <summary>
        /// メンテナンス日誌コードデータ
        /// </summary>
        private List<MaintenanceJournalCodeData> codeDataList = new List<MaintenanceJournalCodeData>();
        public MaintenanceJournalCodeData MaintenanceJournalCodeData;

        #endregion

        #region [プロパティ]
        /// <summary>
        /// メンテナンス日誌コードデータ 設定/取得
        /// </summary>
        public List<MaintenanceJournalCodeData> CodeDataList
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
        //public List<MaintenanceJournalCodeData> CodeDataList;

        #endregion

        #region [publicメソッド]
        /// <summary>
        /// メンテナンス日誌コードデータ取得
        /// </summary>
        /// <remarks>
        /// メンテナンス日誌コード・メンテナンス日誌コード引数を元にメンテナンス日誌コードデータを取得します。
        /// </remarks>
        /// <param name="maintenanceJournalCode">メンテナンス日誌コード</param>
        /// <param name="maintenanceJournalCodeArgument">メンテナンス日誌コード引数</param>
        /// <returns>メンテナンス日誌コードデータ</returns>
        public MaintenanceJournalCodeData GetCodeData(String maintenanceJournalCode, String maintenanceJournalCodeArgument)
        {
            MaintenanceJournalCodeData data = null;
            // メッセージリストの取得
            List<MaintenanceJournalCodeData> maintenanceJournalList = Singleton<ParameterFilePreserve<MaintenanceJournalCodeDataManager>>.Instance.Param.CodeDataList;

            var searchedGroup = (from v in this.codeDataList  //maintenanceJournalList
                                 where v.Code == maintenanceJournalCode
                                 select v).ToDictionary((MaintenanceJournalCodeData) => MaintenanceJournalCodeData.Argument);

            if (!searchedGroup.TryGetValue(maintenanceJournalCodeArgument, out data) && searchedGroup.TryGetValue(String.Empty, out data))
            {
                //codeDataListの実体に影響を及ぼさないよう変更（最後に表示したもののみ正常に出る状態になってしまうため）
                MaintenanceJournalCodeData dynamicData = new MaintenanceJournalCodeData();
                dynamicData.Argument = data.Argument;
                dynamicData.Code = data.Code; // data.Code;
                dynamicData.ImagePath = data.ImagePath;
                dynamicData.Message = data.Message;
                dynamicData.SubTitle = data.SubTitle;
                dynamicData.Title = data.Title;
                data = dynamicData;
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
                return CarisXConst.PathMaintenanceJournal + @"\MaintenanceJournalMessage.xml";
            }
        }
    }
    #endregion

    /// <summary>
    /// コードデータ
    /// </summary>
    /// <remarks>
    /// メンテナンス日誌コードデータクラス
    /// </remarks>
    public class MaintenanceJournalCodeData
    {
        /// <summary>
        /// メンテナンス日誌コード
        /// </summary>
        public String Code = String.Empty;
        /// <summary>
        /// メンテナンス日誌コード引数
        /// </summary>
        public String Argument = String.Empty;
        /// <summary>
        /// メンテナンス日誌メッセージタイトル
        /// </summary>
        public String Title = String.Empty;
        /// <summary>
        /// メンテナンス日誌メッセージサブタイトル
        /// </summary>
        public String SubTitle = String.Empty;
        /// <summary>
        /// メンテナンス日誌メッセージ
        /// </summary>
        public String Message = String.Empty;
        /// <summary>
        /// メンテナンス日誌画像パス
        /// </summary>
        public String ImagePath = String.Empty;

        /// <summary>
        /// メンテナンス日誌画像完全パス取得
        /// </summary>
        /// <remarks>
        /// メンテナンス日誌画像パスにフォルダ構成を適用して取得します。
        /// </remarks>
        /// <returns>メンテナンス日誌画像完全パス</returns>
        public String GetFullImagePath()
        {
            String returnPath = this.ImagePath;
            try
            {
                String root = Path.GetPathRoot(this.ImagePath);
                if ((root == String.Empty) || (root == @"\"))
                {
                    // パスがルートからでなければ、メンテナンス日誌画像パスを付与
                    returnPath = CarisXConst.PathMaintenanceJournalImage + @"\" + this.ImagePath;
                }
            }
            catch (Exception ex)
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, ex.StackTrace);

                // GetPathRootで失敗したら空白
                returnPath = String.Empty;
            }
            return returnPath;
        }
    }

}
