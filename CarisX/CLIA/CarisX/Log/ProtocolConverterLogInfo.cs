using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.Parameter;
using System.Xml.Serialization;

namespace Oelco.CarisX.Log
{
    /// <summary>
    /// プロトコルコンバータログ情報
    /// </summary>
    public class ProtocolConverterLogInfo : ISavePath
    {
        /// <summary>
        /// 出力ファイル数
        /// </summary>
        public Int32 ExportCount;
        /// <summary>
        /// 出力プロトコルインデックス
        /// </summary>
        public List<Int32> ExportProtocolIndex = new List<Int32>();
        /// <summary>
        /// エラー内容リスト
        /// </summary>
        public List<String> ErrorList = new List<String>();
        /// <summary>
        /// 読込、保存パス
        /// </summary>
        private String saveProtocolPath;

        #region ISavePath メンバー
        /// <summary>
        /// 保存パス
        /// </summary>
        [XmlIgnore()]
        public String SavePath
        {
            get
            {
                return saveProtocolPath;
            }
            set
            {
                saveProtocolPath = value;
            }
        }
        #endregion
    }
}
