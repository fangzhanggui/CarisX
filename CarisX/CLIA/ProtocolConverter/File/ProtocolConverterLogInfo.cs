using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ProtocolConverter.File
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

        /// <summary>
        /// チェック処理エラーリスト追加
        /// </summary>
        public void AddErrorList( String index, String row, String errorNum, String data )
        {
            this.ErrorList.Add (String.Format("{0}\t{1}\t{2}\t{3}", index , row, errorNum, data));
        }

        /// <summary>
        /// ログリストにエラー内容を追加する
        /// </summary>
        /// <remarks>
        /// 例外フォーマットで、リストに格納する
        /// </remarks>
        /// <param name="errorNum">エラー番号</param>
        /// <param name="data">エラーデータ</param>
        /// <returns>結果</returns>

        public void AddExceptionLog( string errorNum, string data )
        {
            this.ErrorList.Add (String.Format("{0}\t{1}",errorNum,data));
        }

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
