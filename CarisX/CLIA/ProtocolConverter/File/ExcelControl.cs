using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.Data;
using System.Reflection;

namespace ProtocolConverter.File
{
    class ExcelControl
    {
        #region [定数定義]

        /// <summary>
        /// 読込Excelの行数
        /// </summary>
        const Int32 EXCEL_ROW_COUNT = 82;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// Excelから読み込んだデータを格納
        /// </summary>
        protected List<List<String>> excelDataBuff = new List<List<String>>();

        #endregion

        #region [プロパティ]

        /// <summary>
        /// Excelデータ
        /// </summary>
        public List<List<String>> ExcelData
        {
            get
            {
                return excelDataBuff;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// Excelからデータを読み込む
        /// </summary>
        /// <remarks>
        /// Excelからデータを読み込み、バッファに格納する
        /// </remarks>
        /// <param name="fileName">ファイル名</param>
        /// <param name="stratColumn">読み込み開始列</param>
        /// <returns>結果</returns>
        public Boolean SetExcelBuff( String fileName, Int32 stratColumn )
        {
            try
            {
                // ファイルが存在しているかどうか確認する
                if ( System.IO.File.Exists( fileName ) == false )
                {
                    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog(
                           Singleton<ConvertXmlControl>.Instance.ErrorNoList["Exception"], fileName.ToString());
                    return false;
                }
                else
                {
                    // Excelの全データを読み込む
                    this.excelDataBuff = xlsReader( fileName, stratColumn );
                    if ( this.excelDataBuff == null )
                    {
                        Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog(
                           Singleton<ConvertXmlControl>.Instance.ErrorNoList["Exception"], "this.excelDataBuff == null。");
                        return false;
                    }
                    else if ( excelDataBuff.Count == 0 )
                    {
                        Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog(
                            Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other9"], "分析項目が見つかりません。" );
                    }
                }
            }
            catch ( Exception e )
            {

                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( Singleton<ConvertXmlControl>.Instance.ErrorNoList["Exception"], e.Message + "\n" + e.StackTrace );
                return false;
            }

            return true;
        }

        /// <summary>
        /// Excelから全データを取得する
        /// </summary>
        /// <remarks>
        /// Excelファイルから読み込んだデータを返します。
        /// </remarks>
        /// <param name="inFilename"></param>
        /// <param name="startColumn"></param>
        /// <returns>読込データ</returns>
        /// 1つ目のシートを読込対象とします。
        protected List<List<String>> xlsReader( String inFilename, Int32 startColumn )
        {
            List<List<string>> retList = null;
            OleDbConnection connect = null;
            try
            {
                string connectionString;
                try
                {
                    connectionString = string.Format( @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties=""Excel 12.0;HDR=NO;IMEX=1;""", inFilename );
                    connect = new OleDbConnection( connectionString );
                    // XLSオープン
                    connect.Open();
                }
                catch ( InvalidOperationException )
                {
                    connect.Close();
                    connectionString = string.Format( @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}; Extended Properties=""Excel 8.0;HDR=NO;IMEX=1;""", inFilename );
                    connect = new OleDbConnection( connectionString );
                    // XLSオープン
                    connect.Open();
                }

                // テーブル一覧取得
                DataTable schemaTable = connect.GetOleDbSchemaTable( OleDbSchemaGuid.Tables,
                    new Object[] { null, null, null, "TABLE" } );

                // 先頭テーブル取得
                DataRow schemaTableRow = schemaTable.Rows[0];

                // テーブル名取得
                string tablename = schemaTableRow["TABLE_NAME"].ToString();

                // クエリ文生成
                string sql = "SELECT * FROM `" + tablename + "`";
                OleDbCommand command = connect.CreateCommand();
                command.CommandText = sql;

                // クエリ実行
                OleDbDataAdapter adapter = new OleDbDataAdapter( command );
                DataSet dts = new DataSet();
                adapter.Fill( dts );
                DataTable table = dts.Tables[0];

                // テーブルデータ解析
                retList = new List<List<string>>( ( table.Columns.Count - 1 ) - startColumn );

                // 列単位で検索
                for ( int column = startColumn; column < table.Columns.Count; column++ )
                {
                    if ( table.Rows[0][column].ToString() == "1" )
                    {
                        // 列リスト生成
                        List<string> list = new List<string>( table.Rows.Count );

                        // 行検索
                        for ( int row = 0; row < table.Rows.Count; row++ )
                        {
                            // データ取得
                            object data = table.Rows[row][column];
                            list.Add( data.ToString().Trim() );
                        }
                        // 列データ追加
                        retList.Add( list );
                    }
                    
                }
            }
            catch ( Exception e )
            {
                string error = e.Message;
                retList = null;
            }
            finally
            {
                if ( connect != null )
                {
                    connect.Close();
                }
            }

            return retList;

            //ExcelWrapper xlsApp = new ExcelWrapper();
            //List<List<String>> retList = new List<List<String>>();
            //try
            //{
            //    // Excelを非表示に
            //    xlsApp.Visible = false;
            //    // Excelファイルオープン
            //    xlsApp.Open( inFilename );
            //    // Excelに配置されているチェックボックスの数を取得
            //    Int32 chkCount = xlsApp.GetCheckBoxesCount();
            //    for ( Int32 i = 1; i <= chkCount; i++ )
            //    {
            //        // チェックがOnの列だけ読み込む
            //        Double value = 0;
            //        String range = String.Empty;
            //        var chk = xlsApp.GetCheckBox( i, out value, out range );
            //        if ( value == 1 )
            //        {
            //            // チェックされている列の位置を取得する                        
            //            String col = range.Length == 2 ? range.Substring( 0, 1 ) : range.Substring( 0, 2 );
            //            // 読み込んだ値をListに格納する
            //            List<String> list = new List<String>();
            //            for ( Int32 ii = 1; ii <= EXCEL_ROW_COUNT; ii++ )
            //            {
            //                list.Add( xlsApp.GetCellValue( col + ii.ToString() ) );
            //            }
            //            retList.Add( list );
            //        }
            //    }

            //}
            //catch ( Exception ex )
            //{
            //    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( Singleton<ConvertXmlControl>.Instance.ErrorNoList["Exception"], ex.Message + "\n" + ex.StackTrace );
            //}
            //finally
            //{
            //    // Excelを終了する
            //    xlsApp.Quit();
            //    xlsApp.Dispose();
            //}

            //return retList;
        }

        /// <summary>
        /// Excelデータを行単位で取得
        /// </summary>
        /// <remarks>
        /// Excelデータを行単位（項目単位）で取得します。
        /// </remarks>
        /// <param name="rowIndex">行番号</param>
        /// <returns></returns>
        public List<String> GetExcelRowData( Int32 rowIndex )
        {
            List<String> rtn = new List<String>();

            // 指定行のデータをリストに追加する
            foreach ( List<String> data in excelDataBuff )
            {
                rtn.Add( data[rowIndex] );
            }
            return rtn;
        }

        /// <summary>
        /// Excelのデータを取得します
        /// </summary>
        /// <param name="listIndex">退避領域Listのインデックス</param>
        /// <param name="rowIndex">行番号</param>
        /// <returns></returns>
        public String GetData( Int32 listIndex, Int32 rowIndex )
        {            
            return excelDataBuff[listIndex][rowIndex];;
        }
        #endregion

    }

    /// <summary>
    /// Excelラッパークラス
    /// </summary>
    /// <remarks>
    /// Excelオブジェクトのラッパークラスです。
    /// </remarks>
    public class ExcelWrapper : IDisposable
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// Excelアプリケーションオブジェクト
        /// </summary>
        private Object xlsApplication = null;

        /// <summary>
        /// Workbooksオブジェクト
        /// </summary>
        private Object xlsBooks = null;
        
        #endregion

        #region [プロパティ]

        /// <summary>
        /// Excelアプリケーションオブジェクト
        /// </summary>
        protected Object XlsApplication
        {
            get
            {
                try
                {
                    // 存在しない場合は作成する
                    if ( xlsApplication == null )
                    {
                        Type classType = Type.GetTypeFromProgID( "Excel.Application" );
                        xlsApplication = Activator.CreateInstance( classType );
                    }
                }
                catch ( Exception ex )
                {
                    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( Singleton<ConvertXmlControl>.Instance.ErrorNoList["Exception"], ex.Message + "\n" + ex.StackTrace );
                }                
                return xlsApplication;
            }
        }

        /// <summary>
        /// アプリケーション可視
        /// </summary>
        public Boolean Visible
        {
            set
            {
                try
                {
                    Object[] parameters = new Object[1];
                    parameters[0] = value;
                    XlsApplication.GetType().InvokeMember( "Visible", BindingFlags.SetProperty, null, XlsApplication, parameters );
                }
                catch ( Exception ex )
                {
                    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( Singleton<ConvertXmlControl>.Instance.ErrorNoList["Exception"], ex.Message + "\n" + ex.StackTrace );
                }
                
            }
        }

        /// <summary>
        /// Workbooksオブジェクト
        /// </summary>
        protected Object Workbooks
        {
            get
            {
                try
                {
                    if ( xlsBooks == null )
                    {
                        xlsBooks = XlsApplication.GetType().InvokeMember( "Workbooks", BindingFlags.GetProperty, null, XlsApplication, null );
                    }
                }
                catch ( Exception ex )
                {
                    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( Singleton<ConvertXmlControl>.Instance.ErrorNoList["Exception"], ex.Message + "\n" + ex.StackTrace );
                }
                
                return xlsBooks;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 破棄 
        /// </summary>
        /// <remarks>
        /// Excelオブジェクトのインスタンスを破棄します。
        /// </remarks>
        public void Dispose()
        {
            ReleaseComObject( xlsBooks );
            xlsBooks = null;

            ReleaseComObject( xlsApplication );
            xlsApplication = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        /// <summary>
        /// Excelオープン
        /// </summary>
        /// <remarks>
        /// Excelファイルをオープンします。
        /// </remarks>
        /// <param name="xlsFilePath"></param>
        public void Open( String xlsFilePath )
        {
            try
            {
                Object[] parameters = new Object[15];
                parameters[0] = xlsFilePath;

                for ( int i = 1; i < 15; i++ )
                {
                    parameters[i] = Type.Missing;
                }

                Workbooks.GetType().InvokeMember( "Open", BindingFlags.InvokeMethod, null, Workbooks, parameters );
            }
            catch ( Exception ex )
            {
                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( Singleton<ConvertXmlControl>.Instance.ErrorNoList["Exception"], ex.Message + "\n" + ex.StackTrace );
            }
            
        }

        /// <summary>
        /// Excel終了
        /// </summary>
        /// <remarks>
        /// Excelアプリケーションを終了します。
        /// </remarks>
        public void Quit()
        {
            try
            {
                XlsApplication.GetType().InvokeMember( "Quit", BindingFlags.InvokeMethod, null, XlsApplication, null );
            }
            catch ( Exception ex )
            {
                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( Singleton<ConvertXmlControl>.Instance.ErrorNoList["Exception"], ex.Message + "\n" + ex.StackTrace );
            }            
        }

        /// <summary>
        /// セルの値取得
        /// </summary>
        /// <remarks>
        /// セルの値を文字列で取得します。
        /// </remarks>
        /// <param name="range"></param>
        /// <returns></returns>
        public String GetCellValue( String range )
        {
            String rtn = string.Empty ;
            try
            {
                Object cells = this.GetRange( range );

                rtn= Convert.ToString( cells.GetType().InvokeMember( "Value", BindingFlags.GetProperty, null, cells, null ) );
            }
            catch ( Exception ex )
            {
                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( Singleton<ConvertXmlControl>.Instance.ErrorNoList["Exception"], ex.Message + "\n" + ex.StackTrace );
            }
            return rtn;
        }

        /// <summary>
        /// Excelのチェックボックス数取得
        /// </summary>
        /// <remarks>
        /// Excelシート内に配置されているチェックボックスの数を返します。
        /// </remarks>
        /// <param name="sheets"></param>
        /// <returns></returns>
        public Int32 GetCheckBoxesCount()
        {
            Object book = null;
            Object sheets = null;
            Object sheet = null;
            Object cells = null;
            Object cell = null;
            Object obj = null;
            Int32 count = 0;

            try
            {
                // WorkbooksオブジェクトからBookオブジェクトを取得
                book = GetBook( 1 );
                // BookオブジェクトからSheetsオブジェクトを取得
                sheets = GetSheets( book );
                // SheetsオブジェクトからSheetオブジェクトを取得
                sheet = GetSheet( sheets　);
                // CheckBoxes実行
                obj = sheet.GetType().InvokeMember( "CheckBoxes", BindingFlags.InvokeMethod, null, sheet, null );
                count = (Int32)obj.GetType().InvokeMember( "Count", BindingFlags.GetProperty, null, obj, null );
            }
            catch ( Exception ex )
            {
                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( Singleton<ConvertXmlControl>.Instance.ErrorNoList["Exception"], ex.Message + "\n" + ex.StackTrace );
            }
            finally
            {
                ReleaseComObject( cell );
                ReleaseComObject( cells );
                ReleaseComObject( sheet );
                ReleaseComObject( sheets );
                ReleaseComObject( book );
            }
            return count;
        }

        /// <summary>
        /// Excelのチェックボックスを取得
        /// </summary>
        /// <remarks>
        /// Excelシート内に配置されているチェックボックスを返します。
        /// </remarks>
        /// <param name="index">CheckBoxのインデックス</param>
        /// <param name="value">CheckBoxのValue値</param>
        /// <param name="range">CheckBoxが配置されているセルのRange文字列</param>
        /// <returns></returns>
        public Object GetCheckBox( Int32 index , out Double value , out String range )
        {
            Object book = null;
            Object sheets = null;
            Object sheet = null;
            Object chk = null;
            Object cell = null;

            value = 0;
            range = string.Empty;

            try
            {
                // WorkbooksオブジェクトからBookオブジェクトを取得
                book = GetBook( 1 );
                // BookオブジェクトからSheetsオブジェクトを取得
                sheets = GetSheets( book );
                // SheetsオブジェクトからSheetオブジェクトを取得
                sheet = GetSheet( sheets );
                // CheckBoxes実行
                Object[] parameters = new Object[1];
                parameters[0] = index;
                chk = sheet.GetType().InvokeMember( "CheckBoxes", BindingFlags.InvokeMethod, null, sheet, parameters );
                // チェックボックスのValue値を取得
                value = Convert.ToDouble (chk.GetType().InvokeMember( "Value", BindingFlags.GetProperty, null, chk, null ));
                // チェックボックスの配置セルを取得
                cell = chk.GetType().InvokeMember( "TopLeftCell", BindingFlags.GetProperty, null, chk, null );
                // チェックボックス配置セルのAddressを取得
                range = (String)cell.GetType().InvokeMember( "Address", BindingFlags.GetProperty, null, cell, null );
                range = range.Replace( "$", String.Empty );
            }
            catch ( Exception ex )
            {
                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( Singleton<ConvertXmlControl>.Instance.ErrorNoList["Exception"], ex.Message + "\n" + ex.StackTrace );
            }
            finally
            {
                ReleaseComObject( cell );
                ReleaseComObject( chk );
                ReleaseComObject( sheet );
                ReleaseComObject( sheets );
                ReleaseComObject( book );
            }
            return chk;
        }

        /// <summary>
        /// Rangeを取得
        /// </summary>
        /// <remarks>
        /// Rangeオブジェクトを返します。
        /// </remarks>
        /// <param name="range"></param>
        /// <returns></returns>
        public Object GetRange( String range )
        {
            Object book = null;
            Object sheets = null;
            Object sheet = null;
            Object rtn = null;

            try
            {
                // WorkbooksオブジェクトからBookオブジェクトを取得
                book = this.GetBook( 1 );
                // BookオブジェクトからSheetsオブジェクトを取得
                sheets = this.GetSheets( book );
                // SheetsオブジェクトからSheetオブジェクトを取得
                sheet = this.GetSheet( sheets );
                // SheetオブジェクトからRangeを取得
                rtn = this.GetRange( sheet, range );
            }
            catch ( Exception ex )
            {
                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( Singleton<ConvertXmlControl>.Instance.ErrorNoList["Exception"], ex.Message + "\n" + ex.StackTrace );
            }
            finally
            {
                ReleaseComObject( sheet );
                ReleaseComObject( sheets );
                ReleaseComObject( book );
            }
            return rtn;
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// COMオブジェクトのリリース
        /// </summary>
        /// <remarks>
        /// COMオブジェクトを開放します。
        /// </remarks>
        /// <param name="target"></param>
        private static void ReleaseComObject( Object target )
        {
            try
            {
                if ( ( target != null ) )
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject( target );
                }
            }
            catch ( Exception ex )
            {
                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( Singleton<ConvertXmlControl>.Instance.ErrorNoList["Exception"], ex.Message + "\n" + ex.StackTrace );
            }
            finally
            {
                target = null;
            }
        }

        /// <summary>
        /// Bookを取得
        /// </summary>
        /// <remarks>
        /// Bookオブジェクトを返します。
        /// </remarks>
        /// <param name="index"></param>
        /// <returns></returns>
        private Object GetBook( Int32 index )
        {
            Object rtn = null;
            try
            {
                Object[] parameters = new Object[1];
                parameters[0] = index;
                rtn= Workbooks.GetType().InvokeMember( "Item", BindingFlags.GetProperty, null, Workbooks, parameters );
            }
            catch ( Exception ex )
            {
                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( Singleton<ConvertXmlControl>.Instance.ErrorNoList["Exception"], ex.Message + "\n" + ex.StackTrace );
            }
            return rtn;
            
        }
     
        /// <summary>
        /// Sheetsを取得
        /// </summary>
        /// <remarks>
        /// Sheetsオブジェクトを返します。
        /// </remarks>
        /// <param name="book"></param>
        /// <returns></returns>
        private Object GetSheets( Object book )
        {
            Object rtn = null;
            try
            {
                rtn = book.GetType().InvokeMember( "Worksheets", BindingFlags.GetProperty, null, book, null );
            }
            catch ( Exception ex )
            {
                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( Singleton<ConvertXmlControl>.Instance.ErrorNoList["Exception"], ex.Message + "\n" + ex.StackTrace );
            }            

            return rtn;
        }

        /// <summary>
        /// Sheetを取得
        /// </summary>
        /// <remarks>
        /// Sheetを返します。
        /// </remarks>
        /// <param name="sheets"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        private Object GetSheet( Object sheets )
        {
            Object rtn = null;
            try
            {
                Object[] parameters = new Object[1];
                parameters[0] = 1;   // 1シート目が読込対象であることを前提とする
                return sheets.GetType().InvokeMember( "Item", BindingFlags.GetProperty, null, sheets, parameters );
            }
            catch ( Exception ex )
            {
                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( Singleton<ConvertXmlControl>.Instance.ErrorNoList["Exception"], ex.Message + "\n" + ex.StackTrace );
            }
            return rtn; 
        }
     
        /// <summary>
        /// Rangeを取得
        /// </summary>
        /// <remarks>
        /// Rangeを返します。
        /// </remarks>
        /// <param name="sheet"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        private Object GetRange( Object sheet, String range )
        {
            Object rtn = null;
            try
            {
                Object[] parameters = new Object[2];
                parameters[0] = range;
                parameters[1] = Type.Missing;
                rtn=  sheet.GetType().InvokeMember( "Range", BindingFlags.GetProperty, null, sheet, parameters );
            }
            catch ( Exception ex )
            {
                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( Singleton<ConvertXmlControl>.Instance.ErrorNoList["Exception"], ex.Message + "\n" + ex.StackTrace );
            }
            return rtn;
        }
        
        #endregion
    }
}
