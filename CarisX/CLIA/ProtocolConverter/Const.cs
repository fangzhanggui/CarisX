using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolConverter
{
    /// <summary>
    /// 定数定義クラス
    /// </summary>
    class Const
    {
        /// <summary>
        /// Dictionaryに格納するExcel名のキー
        /// </summary>
        public const String EXCEL = "Excel";

        /// <summary>
        /// Dictionaryに格納するフォルダパスのキー
        /// </summary>
        public const String FOLDER = "Folder";

        /// <summary>
        /// Dictionaryに格納するログ名のキー
        /// </summary>
        public const String LOG = "Log";

        /// <summary>
        /// ログファイル名の初期値
        /// </summary>
        public const String INITIAL_LOG = "ConvertLog.xml";

        /// <summary>
        /// 単位[COI]
        /// </summary>
        public const String UNIT_COI = "COI";

        /// <summary>
        /// 単位[%]
        /// </summary>
        public const String UNIT_PERCENT = "INH%";

        /// <summary>
        /// カウントチェック範囲
        /// </summary>
        public const Int32 MAX_COUNT_RANGES = 8; 

        /// <summary>
        /// 測定ポイント 
        /// </summary>
        public const Int32 MAX_CALIB_MEAS_POINT = 8; 

        /// <summary>
        /// 濃度
        /// </summary>
        public const Int32 MAX_CONCS = 8; 

        /// <summary>
        /// プロトコルXML保存場所
        /// </summary>
        public const String PathProtocol = "";

        public const String ENCRYPT_PASSWORD = "CarisX";
    }
}
