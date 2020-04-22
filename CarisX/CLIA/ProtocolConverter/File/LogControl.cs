using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ProtocolConverter.File
{
    public class LogControl : ISavePath
    {
        /// <summary>
        /// 出力ファイル数
        /// </summary>
        public Int32 ExportCount;
        /// <summary>
        /// 出力プロトコルインデックス
        /// </summary>
        public List<Int32> ExportProtocolIndex;
        /// <summary>
        /// エラー内容リスト
        /// </summary>
        public List<String> ErrorList = new List<String>();
        /// <summary>
        /// 読込、保存パス
        /// </summary>
        private String saveProtocolPath;

        /// <summary>
        /// エラー内容リスト追加
        /// </summary>
        public void AddErrorList( String index, String row, String errorNum, String data )
        {
            this.ErrorList.Add (String.Format("{0}\t{1}\t{2}\t{3}", index , row, errorNum, data));
        }

        /// <summary>
        /// ログリストにエラー内容を追加する
        /// </summary>
        /// <param name="errorNum">エラー番号</param>
        /// <param name="data">エラーデータ</param>
        /// <returns>結果</returns>
        /// <remarks>
        /// 例外フォーマットで、リストに格納する
        /// </remarks>
        public void AddExceptionLog( string errorNum, string data )
        {
            this.ErrorList.Add (String.Format("{0}\t{1}",errorNum,data));
        }

        #region ISavePath メンバー
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
