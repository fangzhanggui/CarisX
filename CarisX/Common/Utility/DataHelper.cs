using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.GUI;
using System.Reflection;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinDataSource;
using System.IO;

namespace Oelco.Common.Utility
{
    /// <summary>
    /// データ出力補助クラス
    /// </summary>
    public class DataHelper
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 区切り文字
        /// </summary>
        protected String separator = String.Empty;

        /// <summary>
        /// エンコード
        /// </summary>
        protected Encoding enc;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataHelper()
        {
            // 区切り文字の初期化
            this.separator = ( System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator != "," ) ? "," : "\t";
			//modified by dong zhang for output the *.csv file ,use this format Mircrosoft Excel will load the file Normal way. 
            enc = Encoding.GetEncoding( "utf-8" );			
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// CSV出力(※列名なし)
        /// </summary>
        /// <remarks>
        /// 列名なしでCSV出力を行います。
        /// </remarks>
        /// <param name="dataSource">出力データ</param>
        /// <param name="savePath">保存先ファイル名</param>
        public void ExportCsv(Infragistics.Win.UltraWinDataSource.UltraDataSource dataSource, String savePath)
        {
            // TODO:ファイル操作のエラー処理未
            String dir = System.IO.Path.GetDirectoryName(savePath);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }

            System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(savePath, false, this.enc);

            foreach (var row in dataSource.Rows.OfType<UltraDataRow>())
            {
                streamWriter.WriteLine(this.formtaCSVLine(dataSource.Band.Columns.OfType<UltraDataColumn>().Select((column) =>
                    row.GetCellValue(column).ToString().Replace("\"", "\"\""))));
            }
            streamWriter.Close();
            streamWriter.Dispose();
            streamWriter = null;
            //dataSource = null;
        }

        /// <summary>
        /// CSV出力(※列名あり)
        /// </summary>
        /// <remarks>
        /// 列名ありでCSV出力を行います。
        /// </remarks>
        /// <typeparam name="T">出力データ型</typeparam>
        /// <param name="dataList">出力データ</param>
        /// <param name="columns">出力列</param>
        /// <param name="append">[既定]作成または上書き(false)/末尾に追加(true)/ファイルが存在する場合追加。存在しない場合に作成(null)</param>
        /// <param name="savePath">保存先ファイル名</param>
        public void ExportCsv<T>( IEnumerable<T> dataList, IEnumerable<UltraGridColumn> columns, String savePath, Boolean? append = false )
        {
            this.ExportCsv(
                dataList,
                columns.ToDictionary( ( column ) => column.Key, ( column ) => column.Header.Caption ),
                savePath,
                append );
        }

        /// <summary>
        /// CSV出力(※列名あり)
        /// </summary>
        /// <remarks>
        /// 列名ありでCSV出力を行います。
        /// </remarks>
        /// <typeparam name="T">出力データ型</typeparam>
        /// <param name="dataList">出力データ</param>
        /// <param name="columns">出力列名</param>
        /// <param name="append">[既定]作成または上書き(false)/末尾に追加(true)/ファイルが存在する場合追加。存在しない場合に作成(null)</param>
        /// <param name="savePath">保存先ファイル名</param>
        public void ExportCsv<T>( IEnumerable<T> dataList, Dictionary<String, String> columns, String savePath, Boolean? append = false )
        {
            try
            {
                String dir = System.IO.Path.GetDirectoryName( savePath );
                if ( !System.IO.Directory.Exists( dir ) )
                {
                    System.IO.Directory.CreateDirectory( dir );
                }

                append = append ?? System.IO.File.Exists( savePath );
                using ( System.IO.StreamWriter streamWriter = new System.IO.StreamWriter( savePath, append.Value, this.enc ) )
                {
                    // 新規作成、上書きの場合
                    if ( !append.Value )
                    {
                        // 列ヘッダの書き込み
                        streamWriter.WriteLine( this.formtaCSVLine( columns.Values.Select( ( column ) => column.Replace( "\"", "\"\"" ) ) ) );
                    }

                    // プロパティの
                    var propertyNames = typeof( T ).GetProperties().Select( ( info ) => info.Name ).ToList();

                    // 行データの書き込み
                    foreach ( var data in dataList )
                    {
                        streamWriter.WriteLine( this.formtaCSVLine( columns.Keys.Select( ( key ) => ( propertyNames.Contains( key ) ? typeof( T ).InvokeMember( key, BindingFlags.GetProperty, null, data, null ) ?? String.Empty : String.Empty ).ToString().Replace( "\"", "\"\"" ) ) ) );
                    }
                    //streamWriter.Close();
                }
            }
            catch ( Exception ex )
            {
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format("failed to ExportCsv:{0}", ex.StackTrace));
            }
        }

        /// <summary>
        /// ファイルが開いているかチェックします
        /// </summary>
        /// <param name="path">検証したいファイルへのフルパス</param>
        /// <returns>開いているかどうか</returns>
        public bool CheckFileOpen(string path)
        {
            FileStream stream = null;

            try
            {
                stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            }
            catch
            {
                // 開いている場合
                return false;

            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            // 開いていない場合
            return true;

        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// CSV1行文字列の成形
        /// </summary>
        /// <remarks>
        /// CSV1行文字列の成形を行います。
        /// </remarks>
        /// <param name="lineElements">1行分の要素文字列</param>
        /// <returns>成形済CSV1行文字列</returns>
        private String formtaCSVLine( IEnumerable<String> lineElements )
        {
            String resultString ="\"" + String.Join( "\"" + this.separator + "\"", lineElements ) + "\"";
            return resultString;
        }
        #endregion

    }
}
