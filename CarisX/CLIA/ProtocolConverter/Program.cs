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
    //   2020/04/07  Ver1.00.01  :
    //      ①新增质控品相关系数A、B；【IssuesNo:1】
    //      ②将质控品系数的A、B的范围改为（-99~99）【IssuesNo:2】
    //      ③故障信息，日>>英
    //      ④补充TurnOrder参数的解析【IssuesNo:4】
    //      ⑤修改定量项目36(HBsAb)、37(HBsAg)的阴阳性阈值判定条件，允许其值大于0；【IssuesNo:5】
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
                                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( "1", "argument" + excelName + "Contains characters that cannot be used in the path.");
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
                                MessageBox.Show( foldName + "Contains characters that cannot be used in the path.", "ProtocolConverter" );
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
                                MessageBox.Show( logName + "Contains characters that cannot be used in the path.", "ProtocolConverter" );
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
                    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( "1", "Excel name is not specified");
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
