using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ProtocolConverter.File;


namespace ProtocolConverter
{
    //----------------------------------------------------------------
    // Public Class.
    //      Program.cs	         
    // Info.
    //   
    // History
    //   2012/06/29  Ver1.00.00  作成  R.Oota
    //----------------------------------------------------------------
    static class Program
    {
        #region [privateメソッド]
        /// <summary>
        /// アプリケーションのメインエントリポイント
        /// </summary>
        /// <remarks>
        /// コマンドライン引数を解析し、コンバート処理を開始します。
        /// </remarks>
        /// <param name="args">コマンドライン引数</param>
        [STAThread]
        static void Main( string[] args )
        {           

            // エクセル名、フォルダパス、ログ名を格納
            Dictionary<string, string> commandData = new Dictionary<string, string>();

            // Convertクラスのインスタンス
            ConvertProtocol convertInstance = new ConvertProtocol();

            //パスに使用できない文字を取得
            char[] invalidChars = System.IO.Path.GetInvalidPathChars();

            try
            {
                foreach ( string data in args )
                {
                    if ( data.StartsWith( "/xls:" ) )
                    {
                        string excelName = data.Substring( 5 );
                        if ( excelName != string.Empty )
                        {
                            if ( excelName.IndexOfAny( invalidChars ) < 0 )
                            {
                                //excelファイル名を取り出す
                                commandData[Const.EXCEL] = System.IO.Path.GetFullPath( excelName );
                            }
                            else
                            {
                                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( "1", "引数" + excelName + "に、パスに使用できない文字が含まれています。" );
                            }
                        }
                    }
                    else if ( data.StartsWith( "/out:" ) )
                    {
                        string foldName = data.Substring( 5 );
                        if ( foldName != string.Empty )
                        {
                            if ( foldName.IndexOfAny( invalidChars ) < 0 )
                            {
								// 指定したフォルダが存在しなかったらログ出力も出来ないので、フォルダ作成をここで実施する。
								string outPath = System.IO.Path.GetFullPath(foldName);
								if (!System.IO.Directory.Exists(outPath))
								{
									System.IO.Directory.CreateDirectory(outPath);
								}
								commandData[Const.FOLDER] = outPath;
                            }
                            else
                            {
                                MessageBox.Show( foldName + "にパスに使用できない文字が含まれています。", "ProtocolConverter" );
                                return;
                            }
                        }
                    }
                    else if ( data.StartsWith( "/log:" ) )
                    {
                        string logName = data.Substring( 5 );
                        if ( logName != string.Empty )
                        {
                            if ( logName.IndexOfAny( invalidChars ) < 0 )
                            {
                                //ログファイル名を取り出す
                                commandData[Const.LOG] = logName;
                            }
                            else
                            {
                                MessageBox.Show( logName + "にパスに使用できない文字が含まれています。", "ProtocolConverter" );
                                return;
                            }
                        }
                    }
                    else
                    {
                        //処理なし
                    }
                }

                // フォルダ名が取得できているか確認
                if ( commandData.ContainsKey( Const.FOLDER ) == false || commandData[Const.FOLDER] == string.Empty )
                {
                    // デフォルト値を使う
                    commandData[Const.FOLDER] = System.IO.Directory.GetCurrentDirectory();
                }

                // ログ名が取得できたか確認する
                if ( commandData.ContainsKey( Const.LOG ) == false || commandData[Const.LOG] == string.Empty )
                {
                    // デフォルト値を使う
                    commandData[Const.LOG] = Const.INITIAL_LOG;
                }

                // Excelパスが取得できているか確認する。
                if ( commandData.ContainsKey( Const.EXCEL ) == false || commandData[Const.EXCEL] == string.Empty )
                {
                    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( "1", "引数にExcel名が指定されていません。" );
                }
                else
                {
                    // コンバート処理を開始
                    convertInstance.Execute( commandData );
                }

                // ログファイル出力先の設定
                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.SavePath = commandData[Const.FOLDER] + @"\" + commandData[Const.LOG];
                // ログファイル出力
                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Save();             

            }
            catch ( Exception e )
            {
                MessageBox.Show( e.Message, "ProtocolConverter" );
            }
        }
        #endregion
        
    }
}
