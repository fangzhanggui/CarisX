using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// 測定結果ファイル作成設定
    /// </summary>
	public class MeasurementResultFileParameter : AttachmentParameter
	{
        #region [定数定義]

        /// <summary>
        /// フォルダ名　デフォルト値設定
        /// </summary>
        public const String FOLDER_NAME_DEFAULT = "";

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// フォルダ名
        /// </summary>
        private String folderName = FOLDER_NAME_DEFAULT;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// フォルダ名の設定／取得
        /// </summary>
        public String FolderName
        {
            get
            {
                return this.folderName;
            }
            set
            {
                this.folderName = value;
            }
        }

        #endregion
	}
	 
}
 
